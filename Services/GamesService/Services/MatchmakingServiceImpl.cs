using GamesService.DAL.Interfaces;
using GamesService.Dto;
using GamesService.Dto.Entities;
using Grpc.Core;
using Matchmaking.Gen;
using Profile.Players.Gen;
using System.Collections.Concurrent;
using Common.Enums;
using ImageService.S3Images.Gen;

namespace GamesService.Services;

public class GamesServiceImpl : GameMatchmaking.GameMatchmakingBase
{
    private readonly ILogger<GamesServiceImpl> _logger;
    private readonly ProfilePlayers.ProfilePlayersClient _playersClient;
    private readonly S3Images.S3ImagesClient _s3ImagesClient;
    private readonly IServiceScopeFactory _scopeFactory;

    private readonly SemaphoreSlim _playersQueueLock = new(1, 1);
    private static readonly SemaphoreSlim _gameCreationLock = new(1, 1);
    private static readonly SemaphoreSlim _sessionCreationLock = new(1, 1);

    private static readonly List<PlayerQueue> _playersQueue = new();
    private static readonly ConcurrentDictionary<int, IServerStreamWriter<MatchmakingResponse>> _responseStreams = new();
    private static readonly ConcurrentDictionary<int, CancellationTokenSource> _playerCts = new();

    private const int _searchDelaySeconds = 10;

    public GamesServiceImpl(
        ILogger<GamesServiceImpl> logger,
        ProfilePlayers.ProfilePlayersClient playersClient,
        S3Images.S3ImagesClient s3ImagesClient,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _playersClient = playersClient;
        _s3ImagesClient = s3ImagesClient;
        _scopeFactory = scopeFactory;

        _ = Task.Run(() => ProcessOutgoingPlayersAsync());
    }

    public override async Task Matchmaking(
        MatchmakingRequest request,
        IServerStreamWriter<MatchmakingResponse> responseStream,
        ServerCallContext context)
    {
        var player = await _playersClient.GetAsync(new GetPlayerRequest { Id = request.PlayerId });
        var avatarPresignedUrl = await _s3ImagesClient.GetPresignedUrlAsync(new GetPresignedUrlRequest() { S3Paths = { player.Avatar.S3Path } });
        var framePresignedUrl = await _s3ImagesClient.GetPresignedUrlAsync(new GetPresignedUrlRequest() { S3Paths = { player.Frame.S3Path } });
        player.Avatar.S3Path = avatarPresignedUrl.FileUrls.First();
        player.Frame.S3Path = framePresignedUrl.FileUrls.First();

        var userSpecificCts = new CancellationTokenSource();
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(context.CancellationToken, userSpecificCts.Token);

        _playerCts[player.Id] = userSpecificCts;

        var playerQueue = new PlayerQueue(
            player.Id,
            player.Nickname,
            player.Mmr,
            player.Rank,
            player.Level,
            player.Avatar,
            player.Frame);

        await _playersQueueLock.WaitAsync(linkedCts.Token);
        try
        {
            _playersQueue.Add(playerQueue);
            _responseStreams[player.Id] = responseStream;
        }
        finally
        {
            _playersQueueLock.Release();
        }

        try
        {
            var queueLifetimeTask = Task.Run(async () =>
            {
                while (!linkedCts.Token.IsCancellationRequested)
                {
                    await Task.Delay(100, linkedCts.Token);
                }
            }, linkedCts.Token);

            await queueLifetimeTask;
        }
        catch (OperationCanceledException)
        {
            await _playersQueueLock.WaitAsync();
            try
            {
                _playersQueue.RemoveAll(p => p.Id == player.Id);
                _responseStreams.TryRemove(player.Id, out _);
                _logger.LogInformation("Matchmaking OperationCanceled for {playerId}", player.Id);
            }
            finally
            {
                _playersQueueLock.Release();
            }
        }
        finally
        {
            _playerCts.TryRemove(player.Id, out _);
        }
    }

    private async Task ProcessOutgoingPlayersAsync()
    {
        while (true)
        {
            try
            {
                await _playersQueueLock.WaitAsync();
                try
                {
                    for (int i = 0; i < _playersQueue.Count; i++)
                    {
                        var player = _playersQueue[i];

                        var opponentIndex = _playersQueue
                            .FindIndex(opponent =>
                                opponent != player &&
                                Math.Abs(opponent.Mmr - player.Mmr) <= player.SearchMmrRange &&
                                Math.Abs(opponent.Mmr - player.Mmr) <= opponent.SearchMmrRange
                            );

                        if (opponentIndex != -1)
                        {
                            var opponent = _playersQueue[opponentIndex];

                            _playersQueue.RemoveAt(opponentIndex);
                            if (opponentIndex < i) i--;
                            _playersQueue.RemoveAt(i);
                            i--;

                            var gameId = await CreateGame();
                            var playerSessionId = await CreateSession(player.Id, gameId);
                            var opponentSessionId = await CreateSession(opponent.Id, gameId);

                            await NotifyPlayers(player, gameId, opponent.Id);
                            await NotifyPlayers(opponent, gameId, player.Id);

                            if (_playerCts.TryGetValue(player.Id, out var cts1))
                            {
                                cts1.Cancel();
                            }
                            if (_playerCts.TryGetValue(opponent.Id, out var cts2))
                            {
                                cts2.Cancel();
                            }
                        }
                        else
                        {
                            player.UpdateSearchSettings();
                        }
                    }
                }
                finally
                {
                    _playersQueueLock.Release();
                }

                await Task.Delay(TimeSpan.FromSeconds(_searchDelaySeconds));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ProcessOutgoingPlayersAsync");
            }
        }
    }

    private async Task NotifyPlayers(PlayerQueue opponent, Guid gameId, int playerId)
    {
        var response = new MatchmakingResponse
        {
            GameId = gameId.ToString(),
            OpponentId = opponent.Id,
            OpponentNickname = opponent.Nickname,
            Mmr = opponent.Mmr,
            Rank = opponent.Rank,
            Level = opponent.Level,
            Avatar = opponent.Avatar,
            Frame = opponent.Frame
        };

        if (_responseStreams.TryGetValue(playerId, out var responseStream1))
        {
            await responseStream1.WriteAsync(response);
        }
    }

    private async Task<Guid> CreateGame()
    {
        await _gameCreationLock.WaitAsync();
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var gamesDAL = scope.ServiceProvider.GetRequiredService<IGamesDAL>();

            var game = new GameDto(GameType.Duel);

            while (await gamesDAL.ExistsAsync(game.Id))
            {
                game = new GameDto(GameType.Duel);
            }
            await gamesDAL.AddOrUpdateAsync(game);

            return game.Id;
        }
        finally
        {
            _gameCreationLock.Release();
        }
    }

    private async Task<Guid> CreateSession(int playerId, Guid gameId)
    {
        await _sessionCreationLock.WaitAsync();
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var sessionsDAL = scope.ServiceProvider.GetRequiredService<ISessionsDAL>();

            var session = new SessionDto(playerId, gameId, true);

            while (await sessionsDAL.ExistsAsync(session.Id))
            {
                session = new SessionDto(playerId, gameId, true);
            }
            await sessionsDAL.AddOrUpdateAsync(session);

            return session.Id;
        }
        finally
        {
            _sessionCreationLock.Release();
        }
    }
}