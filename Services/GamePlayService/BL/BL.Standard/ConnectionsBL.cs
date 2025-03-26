using GamePlayService.BL.BL.Interfaces;
using GamePlayService.Extensions.Helpers;
using GamePlayService.Infrastructure.Models;
using Games.Entities.Gen;
using Sessions.Gen;
using System.Net;

namespace GamePlayService.BL.BL.Standard;

public class ConnectionsBL : IConnectionsBL
{
    private ILogger<ConnectionsBL> _logger;

    private readonly JwtHelper _jwtHelper;

    private readonly SessionsService.SessionsServiceClient _sessionsService;

    private readonly IPlayerBuildsFactory _playerBuildsFactory;

    public ConnectionsBL(
        ILogger<ConnectionsBL> logger,
        JwtHelper jwtHelper,
        SessionsService.SessionsServiceClient sessionsService,
        IPlayerBuildsFactory playerBuildsFactory)
    {
        _logger = logger;
        _jwtHelper = jwtHelper;
        _sessionsService = sessionsService;
        _playerBuildsFactory = playerBuildsFactory;
    }

    public async Task HandleConnection(GameSession gameSession, Session session, IPEndPoint clientEndpoint)
    {
        var player = gameSession.Players.FirstOrDefault(player => player.Id == session.PlayerId);
        if (player != null)
        {
            player.IPEndPoints.Add(clientEndpoint);
        }
        else
        {
            var (hero, units) = await _playerBuildsFactory.GetGameEntities(session.BuildId);

            player = new Player(
                id: session.PlayerId,
                sessionId: session.Id,
                buildId: session.BuildId,
                iPEndPoint: clientEndpoint,
                countMissedMoves: 0,
                hero: hero,
                units: units);

            gameSession.Players.Add(player);
        }
    }

    public async Task<Session?> GetUserSession(string authToken, string sessionId)
    {
        var playerId = _jwtHelper.ValidateToken(authToken);
        if (!playerId.HasValue)
        {
            _logger.LogError($"Ошибка во время валидации следующего токена: {authToken}");
            return null;
        }

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
