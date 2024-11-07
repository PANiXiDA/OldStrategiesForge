using Grpc.Core;
using Players.Backend.Gen;

namespace ProfileBackendService.Services;

public class PlayersBackendServiceImpl : PlayersBackend.PlayersBackendBase
{
    public override Task<CreatePlayerProtoResponse> Create(CreatePlayerProtoRequest request, ServerCallContext context)
    {
        var response = new CreatePlayerProtoResponse
        {
            Message = $"User {request.Nickname} created successfully!"
        };

        return Task.FromResult(response);
    }
}
