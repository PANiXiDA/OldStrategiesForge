using GameEngine.Domains;
using GamePlayService.Extensions.Helpers;
using GamePlayService.Infrastructure.Models;
using GamePlayService.Infrastructure.Requests;
using GamePlayService.Services;
using Games.Entities.Gen;
using Games.Gen;
using Sessions.Gen;
using System.Net;

namespace GamePlayService.BL.BL.Standard;

public class ConnectionBL
{
    private ILogger<ConnectionBL> _logger;

    private readonly JwtHelper _jwtHelper;

    private readonly SessionsService.SessionsServiceClient _sessionsService;

    public ConnectionBL(
        ILogger<ConnectionBL> logger,
        JwtHelper jwtHelper,
        SessionsService.SessionsServiceClient sessionsService)
    {
        _logger = logger;
        _jwtHelper = jwtHelper;
        _sessionsService = sessionsService;
    }

    public async Task HandleConnection(ConnectionMessage message, IPEndPoint clientEndpoint, GameSession gameSession)
    {
        var playerId = _jwtHelper.ValidateToken(message.AuthToken);
        if (!playerId.HasValue)
        {
            _logger.LogError($"Ошибка во время валидации следующего токена: {message.AuthToken}");
            return;
        }

        var session = await GetUserSession(playerId.Value, message.SessionId);
        if (session == null)
        {
            return;
        }

        if (!Guid.TryParse(session.GameId, out var gameId))
        {
            _logger.LogError($"Невалидный Id игры: {session.GameId}");
        }

        //var player = gameSession.Players.FirstOrDefault(player => player.Id == playerId);
        //if (player != null)
        //{
        //    player.IPEndPoint = clientEndpoint;
        //}
        //else
        //{
        //    player = new Player(
        //        id: playerId.Value,
        //        sessionId: message.SessionId,
        //        buildId: message.BuildId,
        //        iPEndPoint: clientEndpoint,
        //        countMissedMoves: 0,
        //        hero: null,
        //        units: new List<Unit>());
        //}
    }

    private async Task<Session?> GetUserSession(int playerId, string sessionId)
    {
        var session = await _sessionsService.GetAsync(new GetSessionRequest() { Id = sessionId });

        if (session.IsActive == false)
        {
            _logger.LogError($"Игрок с id: {playerId} пытается подключиться к неактивной сессии: {sessionId}");
            return null;
        }
        if (session.PlayerId != playerId)
        {
            _logger.LogError($"Игрок с id: {playerId} пытается подключиться к сессии: {sessionId} другого игрока: {session.PlayerId}");
            return null;
        }

        return session;
    }
}
