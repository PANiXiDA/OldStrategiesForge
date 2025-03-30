using GameEngine.Domains;
using GameEngine.DTO.ATBCalculator;
using GameEngine.DTO.Enums;
using GameEngine.Interfaces;
using GamePlayService.BL.BL.Interfaces;
using GamePlayService.Extensions.Helpers;
using GamePlayService.Infrastructure.Enums;
using GamePlayService.Infrastructure.Models;
using Games.ConvertParams.Gen;
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

    private readonly IGridGenerator _gridGenerator;
    private readonly IATBCalculator _atBCalculator;

    public ConnectionsBL(
        ILogger<ConnectionsBL> logger,
        JwtHelper jwtHelper,
        SessionsService.SessionsServiceClient sessionsService,
        IPlayerBuildsFactory playerBuildsFactory,
        IGridGenerator gridGenerator,
        IATBCalculator atBCalculator)
    {
        _logger = logger;
        _jwtHelper = jwtHelper;
        _sessionsService = sessionsService;
        _playerBuildsFactory = playerBuildsFactory;
        _gridGenerator = gridGenerator;
        _atBCalculator = atBCalculator;
    }

    public async Task<GameSession> CreateGameSession(Session session, IPEndPoint clientEndpoint)
    {
        var (hero, units) = await _playerBuildsFactory.GetGameEntities(session.BuildId);

        var player = new Player(
            id: session.PlayerId,
            sessionId: session.Id,
            buildId: session.BuildId,
            countMissedMoves: 0,
            side: PlayerSide.Left, // TODO потом нужно перенести на этап создания игры в базе
            columnsToDeployment: 2, // TODO позже проверять на наличие нужных навыков/артефактов
            confirmedDeployment: false,
            hero: hero,
            units: units,
            iPEndPoints: new List<IPEndPoint>() { clientEndpoint });

        var gameType = (GameType)session.Game.GameType;
        var grid = _gridGenerator.GenerateGrid(gameType);

        var roundState = new RoundState(
            grid: grid,
            heroes: new List<Hero>() { hero },
            units: units,
            atb: new List<GameEntity>(),
            lastCommand: null);

        var gameSession = new GameSession(
            gameState: GameState.WaitingForPlayers,
            gameType: gameType,
            roundState: roundState,
            gameHistory: new List<RoundState>(),
            players: new List<Player>() { player });

        return gameSession;
    }

    public async Task HandleConnection(GameSession gameSession, Session session, IPEndPoint clientEndpoint)
    {
        var player = gameSession.Players.FirstOrDefault(player => player.Id == session.PlayerId);
        if (player != null)
        {
            if (!player.IPEndPoints.Contains(clientEndpoint))
            {
                player.IPEndPoints.Add(clientEndpoint);
            }
        }
        else
        {
            var (hero, units) = await _playerBuildsFactory.GetGameEntities(session.BuildId); //TODO в сессии на самом деле сейчас нет билда. Нужно добавить в базу.

            player = new Player(
                id: session.PlayerId,
                sessionId: session.Id,
                buildId: session.BuildId,               
                countMissedMoves: 0,
                confirmedDeployment: false,
                side: PlayerSide.Right, // TODO потом нужно перенести на этап создания игры в базе
                columnsToDeployment: 2, // TODO позже проверять на наличие нужных навыков/артефактов
                hero: hero,
                units: units,
                iPEndPoints: new List<IPEndPoint>() { clientEndpoint });

            gameSession.Players.Add(player);
            gameSession.RoundState.Heroes.Add(hero);
            gameSession.RoundState.Units.AddRange(units);
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

        var session = await _sessionsService.GetAsync(
            new GetSessionRequest() 
            { 
                Id = sessionId,
                SessionsConvertParams = new SessionsConvertParams { IncludeGame = true }
            });

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

    public void UpdateGameState(GameSession gameSession)
    {
        switch (gameSession.GameType)
        {
            case GameType.Duel:
                gameSession.GameState = gameSession.Players.Count == 2 ? GameState.Deployment : GameState.WaitingForPlayers;
                break;
            case GameType.Random2x2:
                gameSession.GameState = gameSession.Players.Count == 4 ? GameState.Deployment : GameState.WaitingForPlayers;
                break;
            case GameType.Team2x2:
                gameSession.GameState = gameSession.Players.Count == 4 ? GameState.Deployment : GameState.WaitingForPlayers;
                break;
            default:
                break;
        }

        if (gameSession.GameState == GameState.Deployment)
        {
            foreach (var player in gameSession.Players)
            {
                gameSession.RoundState.Grid = _gridGenerator.BaseDeploymentGrid(gameSession.RoundState.Grid, player.Units, player.Side, player.ColumnsToDeployment);
            }

            var gameEntityInitiatives = gameSession.RoundState.Units
                .Select(unit => new GameEntityInitiative(gameEntityId: unit.Id, initiative: unit.CurrentInitiative)).ToList();
            gameEntityInitiatives.AddRange(gameSession.RoundState.Heroes
                .Select(hero => new GameEntityInitiative(gameEntityId: hero.Id, initiative: hero.Initiative)));

            gameSession.RoundState.ATB = _atBCalculator.SetStartingPosition(gameEntityInitiatives);
        }
    }
}
