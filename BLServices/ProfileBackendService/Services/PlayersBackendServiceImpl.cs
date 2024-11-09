using Grpc.Core;
using Players.Backend.Gen;
using Players.Database.Gen;
using CreatePlayerServerRequest = Players.Backend.Gen.CreatePlayerRequest;
using CreatePlayerServerResponse = Players.Backend.Gen.CreatePlayerResponse;
using CreatePlayerClientRequest = Players.Database.Gen.CreatePlayerRequest;
using Google.Protobuf.WellKnownTypes;
using Common.Enums;
using Common;

namespace ProfileBackendService.Services;

public class PlayersBackendServiceImpl : PlayersBackend.PlayersBackendBase
{
    private readonly ILogger<PlayersBackendServiceImpl> _logger;
    private readonly PlayersDatabase.PlayersDatabaseClient _playersDatabaseClient;

    public PlayersBackendServiceImpl(ILogger<PlayersBackendServiceImpl> logger, PlayersDatabase.PlayersDatabaseClient playersDatabaseClient)
    {
        _logger = logger;
        _playersDatabaseClient = playersDatabaseClient;
    }

    public override async Task<CreatePlayerServerResponse> Create(CreatePlayerServerRequest request, ServerCallContext context)
    {
        var grpcRequest = new CreatePlayerClientRequest
        {
            Id = 0,
            CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow),
            UpdatedAt = Timestamp.FromDateTime(DateTime.UtcNow),
            DeletedAt = null,
            Email = request.Email,
            Password = Helpers.GetPasswordHash(request.Password),
            Nickname = request.Nickname,
            Confirmed = false,
            Blocked = false,
            Role = (int)PlayerRole.Player,
            LastLogin = Timestamp.FromDateTime(DateTime.UtcNow),
            AvatarId = 1,
            FrameId = 1,
            Games = 0,
            Wins = 0,
            Loses = 0,
            Mmr = 1500,
            Rank = 0,
            Premium = false,
            Gold = 0,
            Level = 1,
            Experience = 0
        };

        try
        {
            var grpcResponse = await _playersDatabaseClient.CreateAsync(grpcRequest);

            var response = new CreatePlayerServerResponse
            {
                Id = grpcResponse.Id,
                CreatedAt = grpcResponse.CreatedAt,
                UpdatedAt = grpcResponse.UpdatedAt,
                DeletedAt = grpcResponse.DeletedAt,
                Email = grpcResponse.Email,
                Password = grpcResponse.Password,
                Nickname = grpcResponse.Nickname,
                Confirmed = grpcResponse.Confirmed,
                Blocked = grpcResponse.Blocked,
                Role = grpcResponse.Role,
                LastLogin = grpcResponse.LastLogin,
                AvatarId = grpcResponse.AvatarId,
                FrameId = grpcResponse.FrameId,
                Games = grpcResponse.Games,
                Wins = grpcResponse.Wins,
                Loses = grpcResponse.Loses,
                Mmr = grpcResponse.Mmr,
                Rank = grpcResponse.Rank,
                Premium = grpcResponse.Premium,
                Gold = grpcResponse.Gold,
                Level = grpcResponse.Level,
                Experience = grpcResponse.Experience
            };

            return await Task.FromResult(response);
        }
        catch (RpcException)
        {
            throw;
        }
    }
}
