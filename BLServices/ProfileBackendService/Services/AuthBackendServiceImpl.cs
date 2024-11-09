using Common.Enums;
using Common;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Auth.Backend.Gen;
using Auth.Database.Gen;
using RegistrationPlayerServerRequest = Auth.Backend.Gen.RegistrationPlayerRequest;
using RegistrationPlayerServerResponse = Auth.Backend.Gen.RegistrationPlayerResponse;
using RegistrationPlayerClientRequest = Auth.Database.Gen.RegistrationPlayerRequest;
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
}
