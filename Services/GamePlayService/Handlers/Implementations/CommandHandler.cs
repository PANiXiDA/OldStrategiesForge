using Common.Helpers;
using GamePlayService.BL.BL.Interfaces;
using GamePlayService.Common;
using GamePlayService.Handlers.Interfaces;
using GamePlayService.Infrastructure.Enums;
using GamePlayService.Infrastructure.Models;
using GamePlayService.Infrastructure.Requests;
using GamePlayService.Infrastructure.Requests.Commands;
using GamePlayService.Infrastructure.Requests.Commands.Core;
using GamePlayService.Messaging.Interfaces;
using Hangfire;
using System.Net;
using Tools.Redis;

namespace GamePlayService.Handlers.Implementations;

public class CommandHandler : ICommandHandler
{
    private readonly ILogger<CommandHandler> _logger;

    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly IRedisCache _redisCache;

    private readonly IMessageSender _messageSender;
    private readonly IMessageTasks _messageTasks;

    private readonly ICommandsBL _commandsBL;

    public CommandHandler(
        ILogger<CommandHandler> logger,
        IBackgroundJobClient backgroundJobClient,
        IRedisCache redisCache,
        IMessageSender messageSender,
        IMessageTasks messageTasks,
        ICommandsBL commandsBL)
    {
        _logger = logger;

        _backgroundJobClient = backgroundJobClient;
        _redisCache = redisCache;

        _messageSender = messageSender;
        _messageTasks = messageTasks;

        _commandsBL = commandsBL;
    }

    public async Task Handle(IncomingMessage message, IPEndPoint clientEndpoint)
    {
        if (!JsonHelper.TryDeserialize<CommandMessage>(message.Message, out var commandMessage) || commandMessage == null)
        {
            _logger.LogError("Ошибка десериализации JSON: {Message}", message.Message);
            return;
        }

        var (found, gameSession) = await _redisCache.TryGetAsync<GameSession>($"{Constants.GameSessionKeyPrefix}:{message.GameId}");
        if (!found || gameSession!.GameState != GameState.InProgress)
        {
            _logger.LogError($"Игра по id: {message.GameId} не найдена в пуле текущих игр или не находится в статусе InProgress: {gameSession}");
            return;
        }

        Action handlingCommand = commandMessage.CommandType switch
        {
            CommandType.Move => () => HandleMove(commandMessage, gameSession, message.PlayerId),
            CommandType.Attack => () => HandleAttack(commandMessage, gameSession, message.PlayerId),
            CommandType.Defence => () => HandleDefence(commandMessage, gameSession, message.PlayerId),
            CommandType.Wait => () => HandleWait(commandMessage, gameSession, message.PlayerId),
            CommandType.UseAbility => () => HandleUseAbility(commandMessage, gameSession, message.PlayerId),
            CommandType.UseMagic => () => HandleUseMagic(commandMessage, gameSession, message.PlayerId),
            _ => () => LogUnknownCommandAsync(commandMessage.CommandType)
        };
        handlingCommand();

        _backgroundJobClient.Schedule(() => _messageTasks.EndTurn(message.GameId, gameSession.RoundState.ATB.First().GameEntityId), TimeSpan.FromSeconds(30));
        await _messageSender.SendCommandDoneAsync(gameSession, clientEndpoint, message.MessageId);

        ResetPlayerMissedMoves(gameSession, message.PlayerId);
        ValidateGameEnd(gameSession, message.GameId);
        await _redisCache.SetAsync($"{Constants.GameSessionKeyPrefix}:{message.GameId}", gameSession, TimeSpan.FromDays(1));
    }

    private void HandleMove(CommandMessage commandMessage, GameSession gameSession, int playerId)
    {
        if (!JsonHelper.TryDeserialize<MoveCommand>(commandMessage.Command, out var moveCommand) || moveCommand == null)
        {
            _logger.LogError("Ошибка десериализации JSON: {Message}", commandMessage.Command);
            return;
        }

        _commandsBL.Move(moveCommand, gameSession, playerId);
    }

    private void HandleAttack(CommandMessage commandMessage, GameSession gameSession, int playerId)
    {
        if (!JsonHelper.TryDeserialize<AttackCommand>(commandMessage.Command, out var attackCommand) || attackCommand == null)
        {
            _logger.LogError("Ошибка десериализации JSON: {Message}", commandMessage.Command);
            return;
        }

        _commandsBL.Attack(attackCommand, gameSession, playerId);
    }

    private void HandleDefence(CommandMessage commandMessage, GameSession gameSession, int playerId)
    {
        if (!JsonHelper.TryDeserialize<DefenceCommand>(commandMessage.Command, out var defenceCommand) || defenceCommand == null)
        {
            _logger.LogError("Ошибка десериализации JSON: {Message}", commandMessage.Command);
            return;
        }

        _commandsBL.Defence(defenceCommand, gameSession, playerId);
    }

    private void HandleWait(CommandMessage commandMessage, GameSession gameSession, int playerId)
    {
        if (!JsonHelper.TryDeserialize<WaitCommand>(commandMessage.Command, out var waitCommand) || waitCommand == null)
        {
            _logger.LogError("Ошибка десериализации JSON: {Message}", commandMessage.Command);
            return;
        }

        _commandsBL.Wait(waitCommand, gameSession, playerId);
    }

    private void HandleUseAbility(CommandMessage commandMessage, GameSession gameSession, int playerId)
    {
        if (!JsonHelper.TryDeserialize<UseAbilityCommand>(commandMessage.Command, out var useAbilityCommand) || useAbilityCommand == null)
        {
            _logger.LogError("Ошибка десериализации JSON: {Message}", commandMessage.Command);
            return;
        }
        
        // TODO: В будущем продумать и добавить обработку
        //_commandsBL.UseAbility(useAbilityCommand, gameSession, playerId);
    }

    private void HandleUseMagic(CommandMessage commandMessage, GameSession gameSession, int playerId)
    {
        if (!JsonHelper.TryDeserialize<UseMagicCommand>(commandMessage.Command, out var useMagicCommand) || useMagicCommand == null)
        {
            _logger.LogError("Ошибка десериализации JSON: {Message}", commandMessage.Command);
            return;
        }

        // TODO: В будущем продумать и добавить обработку
        //_commandsBL.UseAbility(useMagicCommand, gameSession, playerId);
    }

    private void LogUnknownCommandAsync(CommandType commandType)
    {
        _logger.LogWarning($"Неизвестный тип комманды: {commandType}");
        return;
    }

    private void ResetPlayerMissedMoves(GameSession gameSession, int playerId)
    {
        var player = gameSession.Players.FirstOrDefault(player => player.Id == playerId);
        if (player != null)
        {
            player.CountMissedMoves = 0;
        }
    }

    private void ValidateGameEnd(GameSession gameSession, string gameId)
    {
        foreach (var player in gameSession.Players)
        {
            if (!player.Units.Any())
            {
                _backgroundJobClient.Schedule(() => _messageTasks.GameEnd(gameId, player.Id), TimeSpan.FromSeconds(1));
                gameSession.GameState = GameState.GameOver;
                break;
            }
        }
    }
}
