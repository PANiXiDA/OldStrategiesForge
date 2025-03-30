using Common.Helpers;
using GamePlayService.BL.BL.Interfaces;
using GamePlayService.Extensions.Helpers;
using GamePlayService.Infrastructure.Enums;
using GamePlayService.Infrastructure.Models;
using GamePlayService.Infrastructure.Requests;
using GamePlayService.Infrastructure.Responses;
using GamePlayService.Infrastructure.Responses.Core;
using Games.Gen;
using Hangfire;
using Profile.Players.Gen;
using RedLockNet;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Tools.Redis;

namespace GamePlayService.Services;

public class GamePlayServiceImpl : BackgroundService
{
    private const string GameSessionKeyPrefix = "game";
    private const string ClientMessageAckKeyPrefix = "client_ack";
    private const string ServerMessageAckKeyPrefix = "server_ack";
    private const string ProcessedMessageKeyPrefix = "processed_message";

    private ILogger<GamePlayServiceImpl> _logger;

    private readonly UdpClient _udpServer;
    private readonly JwtHelper _jwtHelper;

    private readonly IDistributedLockFactory _distributedLockFactory;
    private readonly IServiceProvider _serviceProvider;
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly IRedisCache _redisCache;

    private readonly GamesService.GamesServiceClient _gamesService;
    private readonly ProfilePlayers.ProfilePlayersClient _profilePlayers;

    public GamePlayServiceImpl(
        ILogger<GamePlayServiceImpl> logger,
        UdpClient udpClient,
        JwtHelper jwtHelper,
        IDistributedLockFactory distributedLockFactory,
        IServiceProvider serviceProvider,
        IBackgroundJobClient backgroundJobClient,
        IRedisCache redisCache,
        GamesService.GamesServiceClient gamesService,
        ProfilePlayers.ProfilePlayersClient profilePlayers)
    {
        _logger = logger;

        _udpServer = udpClient;
        _jwtHelper = jwtHelper;

        _distributedLockFactory = distributedLockFactory;
        _serviceProvider = serviceProvider;
        _backgroundJobClient = backgroundJobClient;
        _redisCache = redisCache;

        _gamesService = gamesService;
        _profilePlayers = profilePlayers;
    }

    public async Task CloseGameSession(string gameId)
    {
        var (found, gameSession) = await _redisCache.TryGetAsync<GameSession>($"{GameSessionKeyPrefix}:{gameId}");
        if (found && gameSession!.GameState == GameState.WaitingForPlayers)
        {
            await SendGameClosed(gameSession);
            await _gamesService.CloseAsync(new CloseGameRequest() { Id = gameId });
            await _redisCache.RemoveAsync($"{GameSessionKeyPrefix}:{gameId}");
        }
    }

    public async Task EndDeployment(string gameId)
    {
        var (found, gameSession) = await _redisCache.TryGetAsync<GameSession>($"{GameSessionKeyPrefix}:{gameId}");
        if (found && gameSession!.GameState == GameState.Deployment)
        {
            gameSession.GameState = GameState.InProgress;
            await SendGameStart(gameSession);
            await _redisCache.SetAsync($"{GameSessionKeyPrefix}:{gameId}", gameSession);
        }
    }

    public async Task ClientMessageAck(Guid messageId)
    {
        var (found, clientEndpoint) = await _redisCache.TryGetAsync<IPEndPoint>($"{ClientMessageAckKeyPrefix}:{messageId}");
        if (found && clientEndpoint != null)
        {
            await SendClientMessageAck(clientEndpoint, messageId);
        }
    }

    public async Task CheckServerMessageAck(Guid messageId, OutgoingMessageType messageType, IPEndPoint clientEndpoint, int waitingSeconds)
    {
        var (found, message) = await _redisCache.TryGetAsync<string>($"{ServerMessageAckKeyPrefix}:{messageId}");
        if (found && message != null)
        {
            await SendRepeatMessage(clientEndpoint, messageType, messageId, message);
            int newWaitingSeconds = waitingSeconds + 5;
            _backgroundJobClient.Schedule(() => CheckServerMessageAck(messageId, messageType, clientEndpoint, newWaitingSeconds), TimeSpan.FromSeconds(newWaitingSeconds));
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var received = await _udpServer.ReceiveAsync(stoppingToken);
                var clientEndpoint = received.RemoteEndPoint;
                var incomingMessage = Encoding.UTF8.GetString(received.Buffer);

                await HandleIncomingMessage(incomingMessage, clientEndpoint);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обработке входящего сообщения");
            }
        }
    }

    private async Task HandleIncomingMessage(string incomingMessage, IPEndPoint clientEndpoint)
    {
        if (!JsonHelper.TryDeserialize<IncomingMessage>(incomingMessage, out var message) || message == null)
        {
            _logger.LogError($"Ошибка десериализации JSON: {incomingMessage}");
            return;
        }

        var (found, processedMessage) = await _redisCache.TryGetAsync<IncomingMessage>($"{ProcessedMessageKeyPrefix}:{message.MessageId}");
        if (found)
        {
            await SendClientMessageAck(clientEndpoint, message.MessageId);
            return;
        }

        if (message.NeedAck)
        {
            await _redisCache.SetAsync($"{ClientMessageAckKeyPrefix}:{message.MessageId}", clientEndpoint, TimeSpan.FromMinutes(1));
            _backgroundJobClient.Schedule(() => ClientMessageAck(message.MessageId), TimeSpan.FromSeconds(3));
        }

        if (message.AckMessageId.HasValue)
        {
            await _redisCache.RemoveAsync($"{ServerMessageAckKeyPrefix}:{message.AckMessageId.Value}");
        }

        await _redisCache.SetAsync($"{ProcessedMessageKeyPrefix}:{message.MessageId}", message);

        await ChooseHandleStategy(message, clientEndpoint);
    }

    private async Task ChooseHandleStategy(IncomingMessage message, IPEndPoint clientEndpoint)
    {
        switch (message.MessageType)
        {
            case IncomingMessageType.Connection:
                await HandleConnectionMessage(message, clientEndpoint);
                break;

            case IncomingMessageType.Deployment:
                await HandleDeploymentMessage(message, clientEndpoint);
                break;

            case IncomingMessageType.Command:
                // Обработка команды
                break;

            case IncomingMessageType.Surrender:
                await HandleSurrenderMessage(message, clientEndpoint);
                break;

            case IncomingMessageType.MessageAck:
                break;

            default:
                _logger.LogWarning($"Неизвестный тип сообщения: {message.MessageType}");
                break;
        }
    }

    private async Task HandleConnectionMessage(IncomingMessage message, IPEndPoint clientEndpoint)
    {
        using var scope = _serviceProvider.CreateScope();
        var connectionBL = scope.ServiceProvider.GetRequiredService<IConnectionsBL>();

        if (!JsonHelper.TryDeserialize<ConnectionMessage>(message.Message, out var connectionMessage) || connectionMessage == null)
        {
            _logger.LogError("Ошибка десериализации JSON: {Message}", message.Message);
            return;
        }

        var session = await connectionBL.GetUserSession(message.AuthToken, connectionMessage.SessionId);
        if (session == null)
        {
            _logger.LogError("Сессия не найдена по {SessionId}", connectionMessage.SessionId);
            return;
        }

        var lockKey = $"lock:{GameSessionKeyPrefix}:{session.GameId}";
        const int maxRetryAttempts = 3;
        bool lockAcquired = false;

        for (int attempt = 1; attempt <= maxRetryAttempts; attempt++)
        {
            await using (var redLock = await _distributedLockFactory.CreateLockAsync(
                                    resource: lockKey,
                                    expiryTime: TimeSpan.FromSeconds(30),
                                    waitTime: TimeSpan.FromSeconds(10),
                                    retryTime: TimeSpan.FromMilliseconds(200)))
            {
                if (redLock.IsAcquired)
                {
                    lockAcquired = true;

                    var (found, gameSession) = await _redisCache.TryGetAsync<GameSession>($"{GameSessionKeyPrefix}:{session.GameId}");
                    if (found)
                    {
                        await connectionBL.HandleConnection(gameSession!, session, clientEndpoint);
                        connectionBL.UpdateGameState(gameSession!);
                    }
                    else
                    {
                        gameSession = await connectionBL.CreateGameSession(session, clientEndpoint);
                        _backgroundJobClient.Schedule(() => CloseGameSession(session.GameId), TimeSpan.FromMinutes(2));
                    }

                    await SendConnectionConfirmed(clientEndpoint, message.MessageId);

                    if (gameSession!.GameState == GameState.Deployment)
                    {
                        gameSession.GameState = GameState.InProgress;
                        await SendGameStart(gameSession); // TODO убрать когда появится расстановка на клиенте и перенести ниже сохранения в редисе
                        await SendDeploymentStart(gameSession); // Сейчас игнорируется
                        _backgroundJobClient.Schedule(() => EndDeployment(session.GameId), TimeSpan.FromMinutes(2));
                    }

                    await _redisCache.SetAsync($"{GameSessionKeyPrefix}:{message.GameId}", gameSession, TimeSpan.FromDays(1));

                    return;
                }
            }

            _logger.LogWarning("Попытка {Attempt}: не удалось получить блокировку для игры {GameId}", attempt, session.GameId);
            await Task.Delay(500);
        }

        if (!lockAcquired)
        {
            _logger.LogError("Не удалось получить блокировку для игры {GameId} после {MaxRetryAttempts} попыток.", session.GameId, maxRetryAttempts);
        }
    }

    private async Task HandleDeploymentMessage(IncomingMessage message, IPEndPoint clientEndpoint)
    {
        using var scope = _serviceProvider.CreateScope();
        var deploymentBL = scope.ServiceProvider.GetRequiredService<IDeploymentBL>();

        if (!JsonHelper.TryDeserialize<DeploymentMessage>(message.Message, out var deploymentMessage) || deploymentMessage == null)
        {
            _logger.LogError("Ошибка десериализации JSON: {Message}", message.Message);
            return;
        }

        var (found, gameSession) = await _redisCache.TryGetAsync<GameSession>($"{GameSessionKeyPrefix}:{message.GameId}");
        if (!found || gameSession!.GameState != GameState.Deployment)
        {
            _logger.LogError("Игра по id: {GameId} не найдена в пуле текущих игр или не находится в статусе Deployment: {GameSession}", message.GameId, gameSession);
            return;
        }

        if (!deploymentBL.ValidateDeployment(gameSession, message.AuthToken, deploymentMessage.Deployment))
        {
            _logger.LogError("Ошибка при валидации следующей расстановки: {Deployment}", deploymentMessage.Deployment);
            return;
        }

        deploymentBL.ApplyDeployment(gameSession.RoundState.Grid, deploymentMessage.Deployment); // TODO: добавить подтверждение игроком расстановки

        if (gameSession.Players.All(player => player.ConfirmedDeployment))
        {
            await SendGameStart(gameSession);
        }

        if (message.NeedAck)
        {
            await _redisCache.SetAsync($"{ClientMessageAckKeyPrefix}:{message.MessageId}", clientEndpoint, TimeSpan.FromMinutes(1));
            await SendClientMessageAck(clientEndpoint, message.MessageId);
            _backgroundJobClient.Schedule(() => ClientMessageAck(message.MessageId), TimeSpan.FromSeconds(3));
        }

        await _redisCache.SetAsync($"{GameSessionKeyPrefix}:{message.GameId}", gameSession);
    }

    private async Task HandleSurrenderMessage(IncomingMessage message, IPEndPoint clientEndpoint)
    {
        var (found, gameSession) = await _redisCache.TryGetAsync<GameSession>($"{GameSessionKeyPrefix}:{message.GameId}");
        if (!found || gameSession!.GameState != GameState.InProgress)
        {
            _logger.LogError("Игра по id: {GameId} не найдена в пуле текущих игр или не находится в статусе InProgress: {GameSession}", message.GameId, gameSession);
            return;
        }

        var playerId = _jwtHelper.ValidateToken(message.AuthToken);
        if (!playerId.HasValue)
        {
            _logger.LogError($"Ошибка во время валидации следующего токена: {message.AuthToken}");
            return;
        }
        var winnerId = gameSession!.Players.FirstOrDefault(player => player.Id != playerId)?.Id ?? playerId.Value; // TODO переписать на список, когда добавятся команды и больше 1 победителя.

        var updatePlayersStatisticAfterGameRequest = new UpdatePlayersStatisticAfterGameRequest()
        {
            WinnerId = winnerId
        };
        updatePlayersStatisticAfterGameRequest.PlayerIds.AddRange(gameSession!.Players.Select(player => player.Id));
        var updatePlayersStatisticAfterGameResponse = (await _profilePlayers.UpdatePlayersStatisticAfterGameAsync(updatePlayersStatisticAfterGameRequest)).UpdatePlayersStatisticResult;

        var gameResults = new List<GameResult>();
        foreach(var result in updatePlayersStatisticAfterGameResponse)
        {
            var gameResult = new GameResult(
                nickName: result.Nickname,
                mmrChanges: result.MmrChanges);
            gameResults.Add(gameResult);
        }

        await SendGameEnd(gameSession!, gameResults);

        await _gamesService.EndAsync(new EndGameRequest() 
        { 
            GameId = message.GameId,
            WinnerId = winnerId
        });
        await _redisCache.RemoveAsync($"{GameSessionKeyPrefix}:{message.GameId}");

        if (message.NeedAck)
        {
            await _redisCache.SetAsync($"{ClientMessageAckKeyPrefix}:{message.MessageId}", clientEndpoint, TimeSpan.FromMinutes(1));
            await SendClientMessageAck(clientEndpoint, message.MessageId);
            _backgroundJobClient.Schedule(() => ClientMessageAck(message.MessageId), TimeSpan.FromSeconds(3));
        }
    }

    private async Task SendConnectionConfirmed(IPEndPoint clientEndpoint, Guid? messageId)
    {
        var message = new OutgoingMessage<string>(
            messageId: Guid.NewGuid(),
            needAck: false,
            ackMessageId: messageId,
            messageType: OutgoingMessageType.ConnectionConfirmed,
            message: string.Empty);

        string json = JsonSerializer.Serialize(message);
        byte[] responseData = Encoding.UTF8.GetBytes(json);
        await _udpServer.SendAsync(responseData, responseData.Length, clientEndpoint);

        await _redisCache.RemoveAsync($"{ClientMessageAckKeyPrefix}:{messageId}");
    }

    private async Task SendDeploymentStart(GameSession gameSession)
    {
        foreach (var player in gameSession.Players)
        {
            foreach (var clientEndpoint in player.IPEndPoints)
            {
                var message = new OutgoingMessage<DeploymentStartMessage>(
                    messageId: Guid.NewGuid(),
                    needAck: true,
                    ackMessageId: null,
                    messageType: OutgoingMessageType.DeploymentStart,
                    message: new DeploymentStartMessage(
                        grid: gameSession.RoundState.Grid,
                        units: player.Units));

                string json = JsonSerializer.Serialize(message);
                byte[] responseData = Encoding.UTF8.GetBytes(json);

                await _udpServer.SendAsync(responseData, responseData.Length, clientEndpoint);

                await _redisCache.SetAsync($"{ServerMessageAckKeyPrefix}:{message.MessageId}", message, TimeSpan.FromMinutes(1));
                int waitingSeconds = 5;
                _backgroundJobClient.Schedule(() => CheckServerMessageAck(message.MessageId, message.MessageType, clientEndpoint, waitingSeconds), TimeSpan.FromSeconds(waitingSeconds));
            }
        }
    }

    private async Task SendGameStart(GameSession gameSession)
    {
        foreach (var player in gameSession.Players)
        {
            foreach (var clientEndpoint in player.IPEndPoints)
            {
                var message = new OutgoingMessage<RoundState>(
                    messageId: Guid.NewGuid(),
                    needAck: true,
                    ackMessageId: null,
                    messageType: OutgoingMessageType.GameStart,
                    message: gameSession.RoundState);

                string json = JsonSerializer.Serialize(message);
                byte[] responseData = Encoding.UTF8.GetBytes(json);

                await _udpServer.SendAsync(responseData, responseData.Length, clientEndpoint);

                await _redisCache.SetAsync($"{ServerMessageAckKeyPrefix}:{message.MessageId}", message, TimeSpan.FromMinutes(1));
                int waitingSeconds = 5;
                _backgroundJobClient.Schedule(() => CheckServerMessageAck(message.MessageId, message.MessageType, clientEndpoint, waitingSeconds), TimeSpan.FromSeconds(waitingSeconds));
            }
        }
    }

    private async Task SendGameEnd(GameSession gameSession, List<GameResult> gameResults)
    {
        foreach (var player in gameSession.Players)
        {
            foreach (var clientEndpoint in player.IPEndPoints)
            {
                var message = new OutgoingMessage<GameEndMessage>(
                    messageId: Guid.NewGuid(),
                    needAck: true,
                    ackMessageId: null,
                    messageType: OutgoingMessageType.GameStart,
                    message: new GameEndMessage(gameResults: gameResults));

                string json = JsonSerializer.Serialize(message);
                byte[] responseData = Encoding.UTF8.GetBytes(json);

                await _udpServer.SendAsync(responseData, responseData.Length, clientEndpoint);

                await _redisCache.SetAsync($"{ServerMessageAckKeyPrefix}:{message.MessageId}", message, TimeSpan.FromMinutes(1));
                int waitingSeconds = 5;
                _backgroundJobClient.Schedule(() => CheckServerMessageAck(message.MessageId, message.MessageType, clientEndpoint, waitingSeconds), TimeSpan.FromSeconds(waitingSeconds));
            }
        }
    }

    private async Task SendGameClosed(GameSession gameSession)
    {
        var message = new OutgoingMessage<string>(
            messageId: Guid.NewGuid(),
            needAck: true,
            ackMessageId: null,
            messageType: OutgoingMessageType.GameClosed,
            message: string.Empty);

        string json = JsonSerializer.Serialize(message);
        byte[] responseData = Encoding.UTF8.GetBytes(json);

        foreach (var player in gameSession.Players)
        {
            foreach (var clientEndpoint in player.IPEndPoints)
            {
                await _udpServer.SendAsync(responseData, responseData.Length, clientEndpoint);

                await _redisCache.SetAsync($"{ServerMessageAckKeyPrefix}:{message.MessageId}", message, TimeSpan.FromMinutes(1));
                int waitingSeconds = 5;
                _backgroundJobClient.Schedule(() => CheckServerMessageAck(message.MessageId, message.MessageType, clientEndpoint, waitingSeconds), TimeSpan.FromSeconds(waitingSeconds));
            }
        }
    }

    private async Task SendClientMessageAck(IPEndPoint clientEndpoint, Guid messageId)
    {
        var message = new OutgoingMessage<string>(
            messageId: Guid.NewGuid(),
            needAck: false,
            ackMessageId: messageId,
            messageType: OutgoingMessageType.MessageAck,
            message: string.Empty);

        string json = JsonSerializer.Serialize(message);
        byte[] responseData = Encoding.UTF8.GetBytes(json);
        await _udpServer.SendAsync(responseData, responseData.Length, clientEndpoint);

        await _redisCache.RemoveAsync($"{ClientMessageAckKeyPrefix}:{messageId}");
    }

    private async Task SendRepeatMessage(IPEndPoint clientEndpoint, OutgoingMessageType messageType, Guid messageId, string repeatMessage)
    {
        var message = new OutgoingMessage<string>(
            messageId: messageId,
            needAck: true,
            ackMessageId: null,
            messageType: messageType,
            message: repeatMessage);

        string json = JsonSerializer.Serialize(message);
        byte[] responseData = Encoding.UTF8.GetBytes(json);
        await _udpServer.SendAsync(responseData, responseData.Length, clientEndpoint);
    }
}