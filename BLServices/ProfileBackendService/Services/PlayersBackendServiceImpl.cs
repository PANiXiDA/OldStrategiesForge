using Grpc.Core;
using Players.Backend.Gen;
using Players.Database.Gen;
using CreatePlayerServerRequest = Players.Backend.Gen.CreatePlayerRequest;
using CreatePlayerServerResponse = Players.Backend.Gen.CreatePlayerResponse;
using CreatePlayerClientRequest = Players.Database.Gen.CreatePlayerRequest;

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
            Email = request.Email,
            Nickname = request.Nickname,
            Password = request.Password
        };

        try
        {
            var grpcResponse = await _playersDatabaseClient.CreateAsync(grpcRequest);

            var response = new CreatePlayerServerResponse
            {
                Message = grpcResponse.Message,
            };

            return await Task.FromResult(response);
        }
        catch (RpcException)
        {
            throw;
        }
    }
}
