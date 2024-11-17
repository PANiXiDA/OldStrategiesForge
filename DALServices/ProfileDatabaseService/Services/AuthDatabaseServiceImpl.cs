using Auth.Database.Gen;
using Common;
using Common.SearchParams.PlayersService;
using Google.Protobuf.WellKnownTypes;
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

        var response = new RegistrationPlayerResponse()
        {
            Id = id
        };

        return await Task.FromResult(response);
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
            Id = player.Id,
            Email = player.Email,
            Password = player.Password,
            Nickname = player.Nickname,
            LastLogin = player.LastLogin.ToTimestamp(),
            AvatarId = player.AvatarId,
            FrameId = player.FrameId,
            Games = player.Games,
            Wins = player.Wins,
            Loses = player.Loses,
            Mmr = player.Mmr,
            Rank = player.Rank,
            Premium = player.Premium,
            Gold = player.Gold,
            Level = player.Level,
            Experience = player.Experience
        };

        return await Task.FromResult(response);
    }
}
