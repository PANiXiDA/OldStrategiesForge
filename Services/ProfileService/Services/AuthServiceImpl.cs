using Profile.Auth.Gen;
using Common;
using Common.SearchParams.PlayersService;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using ProfileService.DAL.Interfaces;
using ProfileService.Dto.Players;

namespace ProfileService.Services;

public class AuthServiceImpl : ProfileAuth.ProfileAuthBase
{
    private readonly ILogger<AuthServiceImpl> _logger;
    private readonly IPlayersDAL _playersDAL;

    public AuthServiceImpl(ILogger<AuthServiceImpl> logger, IPlayersDAL playersDAL)
    {
        _logger = logger;
        _playersDAL = playersDAL;
    }

    public override async Task<Empty> Registration(RegistrationPlayerRequest request, ServerCallContext context)
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

        await _playersDAL.AddOrUpdateAsync(new PlayersDto().PlayersDtoFromProtoAuth(request));

        return new Empty();
    }

    public override async Task<LoginPlayerResponse> Login(LoginPlayerRequest request, ServerCallContext context)
    {
        var player = await _playersDAL.GetAsync(request.Email);

        if (player == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, Constants.ErrorMessages.PlayerNotFound));
        }
        if (!player.Confirmed)
        {
            throw new RpcException(new Status(StatusCode.FailedPrecondition, Constants.ErrorMessages.PlayerNotConfirm));
        }
        if (player.Blocked)
        {
            throw new RpcException(new Status(StatusCode.PermissionDenied, Constants.ErrorMessages.PlayerBlocked));
        }

        var response = new LoginPlayerResponse()
        {
        };

        return await Task.FromResult(response);
    }
}
