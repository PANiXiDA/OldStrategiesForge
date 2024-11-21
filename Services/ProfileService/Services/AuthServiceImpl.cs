using Profile.Auth.Gen;
using Common;
using Common.SearchParams.ProfileService;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using ProfileService.DAL.Interfaces;
using ProfileService.Dto;
using Common.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Common.Configurations;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using Tools.RabbitMQ;

namespace ProfileService.Services;

public class AuthServiceImpl : ProfileAuth.ProfileAuthBase
{
    private const string _queue = "email_sender_requests";

    private readonly ILogger<AuthServiceImpl> _logger;
    private readonly JwtSettings _jwtSettings;
    private readonly IPlayersDAL _playersDAL;
    private readonly ITokensDAL _tokensDAL;
    private readonly IRabbitMQClient _rabbitMQClient;

    public AuthServiceImpl(
        ILogger<AuthServiceImpl> logger,
        IOptions<JwtSettings> jwtSettings,
        IPlayersDAL playersDAL,
        ITokensDAL tokensDAL,
        IRabbitMQClient rabbitMQClient)
    {
        _logger = logger;
        _jwtSettings = jwtSettings.Value;
        _playersDAL = playersDAL;
        _tokensDAL = tokensDAL;
        _rabbitMQClient = rabbitMQClient;
    }

    public override async Task<Empty> Registration(RegistrationPlayerRequest request, ServerCallContext context)
    {
        bool isEmailExist = await _playersDAL.ExistsAsync(new PlayersSearchParams() { Email = request.Email });
        if (isEmailExist)
        {
            throw new RpcException(new Status(StatusCode.AlreadyExists, Constants.ErrorMessages.ExistsEmail));
        }

        bool isNicknameExist = await _playersDAL.ExistsAsync(new PlayersSearchParams() { Nickname = request.Nickname });
        if (isNicknameExist)
        {
            throw new RpcException(new Status(StatusCode.AlreadyExists, Constants.ErrorMessages.ExistsNicknane));
        }

        var playerId = await _playersDAL.AddOrUpdateAsync(new PlayersDto().PlayersDtoFromProtoAuth(request));

        try
        {
            _logger.LogInformation($"Sending an email to the account confirmation: {request.Email}.");

            var result = await _rabbitMQClient.CallAsync<(string, int), string>(
                (request.Email, playerId),
                _queue,
                TimeSpan.FromSeconds(30)
            );

            if (string.IsNullOrEmpty(result))
            {
                throw new RpcException(new Status(StatusCode.Internal, "Failed to send confirmation email."));
            }

            _logger.LogInformation($"Message for {request.Email} successfully processed: {result}.");
        }
        catch (TimeoutException)
        {
            throw new RpcException(new Status(StatusCode.DeadlineExceeded, "Timeout while waiting for email confirmation."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error sending an account confirmation message {request.Email}");
            throw new RpcException(new Status(StatusCode.Internal, "Error while sending confirmation email."));
        }

        return new Empty();
    }

    public override async Task<ConfirmEmailResponse> Confirm(ConfirmEmailRequest request, ServerCallContext context)
    {
        var player = await _playersDAL.GetAsync(request.PlayerId);

        if (player.Blocked)
        {
            throw new RpcException(new Status(StatusCode.PermissionDenied, Constants.ErrorMessages.PlayerBlocked));
        }
        player.Confirmed = true;

        await _playersDAL.AddOrUpdateAsync(player);

        var response = new ConfirmEmailResponse()
        {
            Nickname = player.Nickname
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

        var (accessToken, refreshToken) = await GenerateTokens(player.Id, (PlayerRole)player.Role);

        var response = new LoginPlayerResponse()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };

        return await Task.FromResult(response);
    }

    public override async Task<RefreshTokenResponse> Refresh(RefreshTokenRequest request, ServerCallContext context)
    {
        var token = await _tokensDAL.GetAsync(request.RefreshToken);
        if (token == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, Constants.ErrorMessages.BadRefreshToken));
        }

        var player = await _playersDAL.GetAsync(token.PlayerId);

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

        var (accessToken, refreshToken) = await GenerateTokens(player.Id, (PlayerRole)player.Role, token);

        var response = new RefreshTokenResponse()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };

        return await Task.FromResult(response);
    }

    private async Task<(string AccessToken, string RefreshToken)> GenerateTokens(int playerId, PlayerRole playerRole, TokensDto? token = null)
    {
        var accessToken = GenerateAccessToken(playerId, playerRole);
        var refreshToken = await GenerateRefreshToken(playerId, token);

        await _tokensDAL.AddOrUpdateAsync(refreshToken);

        return (accessToken, refreshToken.RefreshToken);
    }

    private string GenerateAccessToken(int playerId, PlayerRole playerRole)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, playerId.ToString()),
            new Claim(ClaimTypes.Role, playerRole.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var accessToken = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExp),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(accessToken);
    }

    private async Task<TokensDto> GenerateRefreshToken(int playerId, TokensDto? token = null)
    {
        var refreshToken = GenerateRandomString();
        while (await _tokensDAL.ExistsAsync(new TokensSearchParams() { RefreshToken = refreshToken }))
        {
            refreshToken = GenerateRandomString();
        }

        token ??= new TokensDto
        {
            Id = 0,
            PlayerId = playerId
        };

        token.RefreshToken = refreshToken;
        token.RefreshTokenExp = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExp);

        return token;
    }

    private string GenerateRandomString()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
