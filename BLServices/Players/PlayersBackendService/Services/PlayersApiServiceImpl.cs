using Grpc.Core;
using PlayersApiService.Protos;

namespace PlayersBackendService.Services;

public class PlayersApiServiceImpl : PlayersAPI.PlayersAPIBase
{
    public override Task<CreatePlayerProtoResponse> Create(CreatePlayerProtoRequest request, ServerCallContext context)
    {
        // Логика создания пользователя на сервере
        var response = new CreatePlayerProtoResponse
        {
            Message = $"User {request.Nickname} created successfully!"
        };

        return Task.FromResult(response);
    }
}
