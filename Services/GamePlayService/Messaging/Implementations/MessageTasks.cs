using GamePlayService.BL.BL.Interfaces;
using GamePlayService.Common;
using GamePlayService.Infrastructure.Enums;
using GamePlayService.Infrastructure.Models;
using GamePlayService.Infrastructure.Requests.Commands;
using GamePlayService.Infrastructure.Responses;
using GamePlayService.Messaging.Interfaces;
using Games.Gen;
using Hangfire;
using Profile.Players.Gen;
using System.Net;
using Tools.Redis;

namespace GamePlayService.Messaging.Implementations;

public class MessageTasks : IMessageTasks
{
    private const int MaxMissedMoves = 10;
    private const int AckDelayIncrement = 5;

    private readonly ILogger<MessageTasks> _logger;

    private readonly IRedisCache _redisCache;
    private readonly IBackgroundJobClient _backgroundJobClient;

    private readonly IMessageSender _messageSender;
    private readonly ICommandsBL _commandsBL; // TODO: подумать, как избаваться от этой зависимости в будущем

    private readonly GamesService.GamesServiceClient _gamesService;
    private readonly ProfilePlayers.ProfilePlayersClient _profilePlayers;

    public MessageTasks(
        ILogger<MessageTasks> logger,
        IRedisCache redisCache,
        IBackgroundJobClient backgroundJobClient,
        IMessageSender messageSender,
        ICommandsBL commandsBL,
        GamesService.GamesServiceClient gamesService,
        ProfilePlayers.ProfilePlayersClient profilePlayers)
    {
        _logger = logger;

        _redisCache = redisCache;
        _backgroundJobClient = backgroundJobClient;

        _messageSender = messageSender;
        _commandsBL = commandsBL;

        _gamesService = gamesService;
        _profilePlayers = profilePlayers;
    }

    #region Helper Methods

    private string GetClientMessageAckKey(Guid messageId) =>
        $"{Constants.ClientMessageAckKeyPrefix}:{messageId}";

    private string GetServerMessageAckKey(Guid messageId) =>
        $"{Constants.ServerMessageAckKeyPrefix}:{messageId}";

    private string GetGameSessionKey(string gameId) =>
        $"{Constants.GameSessionKeyPrefix}:{gameId}";

    private async Task<GameSession?> GetGameSessionAsync(string gameId)
    {
        var key = GetGameSessionKey(gameId);
        var (found, session) = await _redisCache.TryGetAsync<GameSession>(key);
        if (!found || session == null)
        {
            _logger.LogWarning("Game session not found for gameId: {GameId}", gameId);
            return null;
        }
        return session;
    }

    #endregion

    public async Task CheckClientMessageAck(Guid messageId)
    {
        var key = GetClientMessageAckKey(messageId);
        var (found, clientEndpoint) = await _redisCache.TryGetAsync<IPEndPoint>(key);
        if (found && clientEndpoint != null)
        {
            _logger.LogInformation("Sending client ack for message {MessageId} to endpoint {Endpoint}", messageId, clientEndpoint);
            await _messageSender.SendClientMessageAck(clientEndpoint, messageId);
        }
    }

    public async Task CheckServerMessageAck(Guid messageId, OutgoingMessageType messageType, IPEndPoint clientEndpoint, int waitingSeconds)
    {
        var key = GetServerMessageAckKey(messageId);
        var (found, message) = await _redisCache.TryGetAsync<string>(key);
        if (found && message != null)
        {
            _logger.LogInformation("Resending message {MessageId} after {WaitingSeconds} seconds", messageId, waitingSeconds);
            await _messageSender.SendRepeatMessage(clientEndpoint, messageType, messageId, message);
            int newWaitingSeconds = waitingSeconds + AckDelayIncrement;
            _backgroundJobClient.Schedule(() => CheckServerMessageAck(messageId, messageType, clientEndpoint, newWaitingSeconds),
                TimeSpan.FromSeconds(newWaitingSeconds));
        }
    }

    public async Task CloseGameSession(string gameId)
    {
        var session = await GetGameSessionAsync(gameId);
        if (session != null && session.GameState == GameState.WaitingForPlayers)
        {
            _logger.LogInformation("Closing game session for gameId: {GameId}", gameId);
            await _messageSender.SendGameClosed(session);
            await _gamesService.CloseAsync(new CloseGameRequest { Id = gameId });
            await _redisCache.RemoveAsync(GetGameSessionKey(gameId));
        }
    }

    public async Task EndDeployment(string gameId)
    {
        var session = await GetGameSessionAsync(gameId);
        if (session != null && session.GameState == GameState.Deployment)
        {
            _logger.LogInformation("Ending deployment phase for gameId: {GameId}", gameId);
            session.GameState = GameState.InProgress;
            await _messageSender.SendGameStart(session);
            await _redisCache.SetAsync(GetGameSessionKey(gameId), session);
        }
    }

    public async Task EndTurn(string gameId, Guid gameObjectId)
    {
        var session = await GetGameSessionAsync(gameId);
        if (session != null && session.GameState == GameState.InProgress &&
            session.RoundState.ATB.FirstOrDefault()?.GameEntityId == gameObjectId)
        {
            var player = session.Players.FirstOrDefault(p => p.Units.Any(u => u.Id == gameObjectId));
            if (player == null)
            {
                _logger.LogWarning("Player with gameObjectId {GameObjectId} not found in session {GameId}", gameObjectId, gameId);
                return;
            }

            player.CountMissedMoves++;
            if (player.CountMissedMoves >= MaxMissedMoves)
            {
                _logger.LogInformation("Player {PlayerId} exceeded missed moves limit. Ending game {GameId}", player.Id, gameId);
                await GameEnd(gameId, player.Id);
                return;
            }

            _commandsBL.Wait(new WaitCommand(gameObjectId), session, player.Id);
            await _messageSender.SendCurrentRoundState(session);
            await _redisCache.SetAsync(GetGameSessionKey(gameId), session);
        }
    }

    public async Task GameEnd(string gameId, int loserId)
    {
        var session = await GetGameSessionAsync(gameId);
        if (session != null && session.GameState == GameState.InProgress)
        {
            var winner = session.Players.FirstOrDefault(p => p.Id != loserId); // TODO: переписать, когда появятся командные игры
            if (winner == null)
            {
                _logger.LogError("Winner not found for gameId: {GameId}", gameId);
                return;
            }
            int winnerId = winner.Id;

            var updateRequest = new UpdatePlayersStatisticAfterGameRequest { WinnerId = winnerId };
            updateRequest.PlayerIds.AddRange(session.Players.Select(p => p.Id));
            var updateResponse = (await _profilePlayers.UpdatePlayersStatisticAfterGameAsync(updateRequest))
                                    .UpdatePlayersStatisticResult;

            var gameResults = updateResponse.Select(result =>
                new GameResult(result.Nickname, result.MmrChanges)).ToList();

            await _messageSender.SendGameEnd(session, gameResults);
            await _gamesService.EndAsync(new EndGameRequest { GameId = gameId, WinnerId = winnerId });
            await _redisCache.RemoveAsync(GetGameSessionKey(gameId));
        }
    }
}
