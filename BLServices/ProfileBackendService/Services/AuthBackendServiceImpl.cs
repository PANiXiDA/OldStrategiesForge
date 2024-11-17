using Common.Enums;
using Common;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Auth.Backend.Gen;
using Auth.Database.Gen;
using RegistrationPlayerServerRequest = Auth.Backend.Gen.RegistrationPlayerRequest;
using RegistrationPlayerServerResponse = Auth.Backend.Gen.RegistrationPlayerResponse;
using RegistrationPlayerClientRequest = Auth.Database.Gen.RegistrationPlayerRequest;
using LoignPlayerServerRequest = Auth.Backend.Gen.LoginPlayerRequest;
using LoginPlayerServerResponse = Auth.Backend.Gen.LoginPlayerResponse;
using LoginPlayerClientRequest = Auth.Database.Gen.LoginPlayerRequest;
using AutoMapper;

namespace ProfileBackendService.Services;

public class AuthBackendServiceImpl : AuthBackend.AuthBackendBase
{
    private readonly ILogger<AuthBackendServiceImpl> _logger;
    private readonly AuthDatabase.AuthDatabaseClient _authDatabaseClient;
    private readonly IMapper _mapper;

    public AuthBackendServiceImpl(
        ILogger<AuthBackendServiceImpl> logger,
        AuthDatabase.AuthDatabaseClient authDatabaseClient,
        IMapper mapper)
    {
        _logger = logger;
        _authDatabaseClient = authDatabaseClient;
        _mapper = mapper;
    }

    public override async Task<RegistrationPlayerServerResponse> Registration(RegistrationPlayerServerRequest request, ServerCallContext context)
    {
        var grpcRequest = new RegistrationPlayerClientRequest
        {
            Id = 0,
            CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow),
            UpdatedAt = Timestamp.FromDateTime(DateTime.UtcNow),
            DeletedAt = null,
            Email = request.Email,
            Password = Helpers.GetPasswordHash(request.Password),
            Nickname = request.Nickname,
            Confirmed = true, //как добавлю подтверждение аккаунта, нужно поменять
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
            var grpcResponse = await _authDatabaseClient.RegistrationAsync(grpcRequest);

            var response = _mapper.Map<RegistrationPlayerServerResponse>(grpcRequest);
            response.Id = grpcResponse.Id;

            return await Task.FromResult(response);
        }
        catch (RpcException)
        {
            throw;
        }
    }

    public override async Task<LoginPlayerServerResponse> Login(LoignPlayerServerRequest request, ServerCallContext context)
    {
        var grpcRequest = new LoginPlayerClientRequest()
        {
            Email = request.Email,
            Password = request.Password
        };

        try
        {
            var grpcResponse = await _authDatabaseClient.LoginAsync(grpcRequest);

            if (Helpers.GetPasswordHash(request.Password) != grpcResponse.Password)
            {
                throw new RpcException(new Status(StatusCode.PermissionDenied, Constants.ErrorMessages.PlayerPasswordIncorrect));
            }

            var response = _mapper.Map<LoginPlayerServerResponse>(grpcResponse);

            return await Task.FromResult(response);
        }
        catch (RpcException)
        {
            throw;
        }
    }
}
