using Auth.Database.Gen;
using Common;
using Common.SearchParams.PlayersService;
using Grpc.Core;
using ProfileDatabaseService.DAL.Interfaces;
using ProfileDatabaseService.Dto.Players;

namespace ProfileDatabaseService.Services;

public class AuthDatabaseServiceImpl : AuthDatabase.AuthDatabaseBase
{
    private readonly ILogger<AuthDatabaseServiceImpl> _logger;
    private readonly IPlayersDAL _playersDAL;

    public AuthDatabaseServiceImpl(ILogger<AuthDatabaseServiceImpl> logger, IPlayersDAL playersDAL)
    {
        _logger = logger;
        _playersDAL = playersDAL;
    }

    public override async Task<RegistrationPlayerResponse> Registration(RegistrationPlayerRequest request, ServerCallContext context)
    {
        bool isEmailUnique = !(await _playersDAL.ExistsAsync(new PlayersSearchParams() { Email = request.Email }));
        if (!isEmailUnique)
        {
            throw new RpcException(new Status(StatusCode.AlreadyExists, Constants.ErrorMessages.ExistsEmail));
        }

        bool isNicknameUnique = !(await _playersDAL.ExistsAsync(new PlayersSearchParams() { Nickname = request.Nickname }));
        if (!isNicknameUnique)
        {
            throw new RpcException(new Status(StatusCode.AlreadyExists, Constants.ErrorMessages.ExistsNicknane));
        }

        int id = await _playersDAL.AddOrUpdateAsync(new PlayersDto().PlayersDtoFromProto(request));

        var response = new RegistrationPlayerResponse
        {
            Id = id
        };

        return await Task.FromResult(response);
    }
}
