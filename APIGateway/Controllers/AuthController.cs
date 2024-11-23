using Asp.Versioning;
using Profile.Auth.Gen;
using Common;
using Microsoft.AspNetCore.Mvc;
using APIGateway.Infrastructure.Extensions;
using APIGateway.Infrastructure.Requests.Auth;
using APIGateway.Infrastructure.Responses.Auth;
using Tools.Encryption;
using Microsoft.AspNetCore.Authorization;
using APIGateway.Infrastructure.Responses.Players;
using System.Security.Claims;

namespace APIGateway.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
[Produces("application/json")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly AesEncryption _encryption;
    private readonly ProfileAuth.ProfileAuthClient _authClient;

    private readonly string? _domen;

    public AuthController(
        IConfiguration configuration,
        ILogger<AuthController> logger,
        AesEncryption encryption,
        ProfileAuth.ProfileAuthClient authClient)
    {
        _logger = logger;
        _encryption = encryption;
        _authClient = authClient;

        _domen = configuration["Domen"];
    }

    [HttpPost]
    [Route("sign-up")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 201)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Registration([FromBody] RegistrationPlayerRequestDto request)
    {
        var validator = new CreatePlayerDtoValidator();
        var results = validator.Validate(request);
        if (!results.IsValid)
        {
            return BadRequest(RestApiResponseBuilder<NoContent>.Fail(results.Errors.First().ErrorMessage, results.Errors.First().ErrorCode));
        }

        var grpcResponse = await _authClient.RegistrationAsync(request.RegistrationPlayerRequestDtoToProto());

        return StatusCode(StatusCodes.Status201Created, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }

    [HttpPost]
    [Route("login")]
    [ProducesResponseType(typeof(RestApiResponse<LoginPlayerResponseDto>), 200)]
    public async Task<ActionResult<RestApiResponse<LoginPlayerResponseDto>>> Login([FromBody] LoginPlayerRequestDto request)
    {
        var grpcResponse = await _authClient.LoginAsync(request.LoginPlayerRequestDtoToProto());

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<LoginPlayerResponseDto>.Success(new LoginPlayerResponseDto().LoginPlayerResponseDtoFromProto(grpcResponse)));
    }

    [HttpPost]
    [Route("confirm-email")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 200)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> ConfirmEmail([FromBody] ConfirmEmailRequestDto request)
    {
        var grpcResponse = await _authClient.ConfirmEmailAsync(request.ConfirmEmailRequestDtoToProto());

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }

    [HttpGet]
    [Route("confirm")]
    public async Task<IActionResult> СonfirmAccount([FromQuery] string token)
    {
        var playerId = _encryption.Decrypt<int>(token);

        var grpcResponse = await _authClient.ConfirmAccountAsync(new ConfirmAccountRequest() { PlayerId = playerId });
        var nickname = grpcResponse.Nickname;

        return Redirect($"{_domen}/Profile/Confirm?nickname={nickname}");
    }

    [HttpPost]
    [Route("recovery-password")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 200)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> RecoveryPassword([FromBody] RecoveryPasswordRequestDto request)
    {
        var grpcResponse = await _authClient.RecoveryPasswordAsync(request.RecoveryPasswordRequestDtoToProto());

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }

    [HttpGet]
    [Route("recovery")]
    public async Task<IActionResult> RecoveryPassword([FromQuery] string token)
    {
        var playerId = _encryption.Decrypt<int>(token);
        var grpcResponse = await _authClient.ChangePasswordAsync(new ChangePasswordRequest() { PlayerId = playerId });

        return Redirect($"{_domen}/Profile/Recovery");
    }


    [HttpPost]
    [Authorize]
    [Route("logout")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 200)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Logout(LogoutRequestDto request)
    {
        var grpcResponse = await _authClient.LogoutAsync(request.LogoutRequestDtoToProto());

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }

    [HttpPost]
    [Authorize]
    [Route("logout-from-all-devices")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 200)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> LogoutFromAllDevices()
    {

        var playerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (playerIdClaim == null)
        {
            return StatusCode(StatusCodes.Status401Unauthorized, RestApiResponseBuilder<GetPlayerResponseDto>.Fail(Constants.ErrorMessages.Unauthorized, Constants.ErrorMessages.ErrorKey));
        }

        int playerId = int.Parse(playerIdClaim.Value);

        var grpcResponse = await _authClient.LogoutFromAllDevicesAsync(new LogoutFromAllDevicesRequest() { PlayerId = playerId });

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }

    [HttpPost]
    [Route("refresh")]
    [ProducesResponseType(typeof(RestApiResponse<RefreshTokenResponseDto>), 200)]
    public async Task<ActionResult<RestApiResponse<RefreshTokenResponseDto>>> Refresh([FromBody] RefreshTokenRequestDto request)
    {
        var grpcResponse = await _authClient.RefreshAsync(request.RefreshTokenRequestDtoToProto());

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<RefreshTokenResponseDto>.Success(new RefreshTokenResponseDto().RefreshTokenResponseDtoFromProto(grpcResponse)));
    }
}