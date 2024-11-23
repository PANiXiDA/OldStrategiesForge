using Asp.Versioning;
using Profile.Auth.Gen;
using Common;
using Grpc.Core;
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

        try
        {
            var grpcResponse = await _authClient.RegistrationAsync(request.RegistrationPlayerRequestDtoToProto());

            return StatusCode(StatusCodes.Status201Created, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
        }
        catch (RpcException ex)
        {
            if (ex.StatusCode == Grpc.Core.StatusCode.AlreadyExists)
            {
                return StatusCode(StatusCodes.Status409Conflict, RestApiResponseBuilder<NoContent>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
            }

            _logger.LogError($"Во время создания пользователя {request.Nickname} произошла ошибка: {ex.Message}");

            return StatusCode(StatusCodes.Status500InternalServerError, RestApiResponseBuilder<NoContent>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
        }
        catch (TimeoutException ex)
        {
            return StatusCode(StatusCodes.Status408RequestTimeout, RestApiResponseBuilder<NoContent>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, RestApiResponseBuilder<NoContent>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
        }
    }

    [HttpPost]
    [Route("login")]
    [ProducesResponseType(typeof(RestApiResponse<LoginPlayerResponseDto>), 200)]
    public async Task<ActionResult<RestApiResponse<LoginPlayerResponseDto>>> Login([FromBody] LoginPlayerRequestDto request)
    {
        try
        {
            var grpcResponse = await _authClient.LoginAsync(request.LoginPlayerRequestDtoToProto());

            return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<LoginPlayerResponseDto>.Success(new LoginPlayerResponseDto().LoginPlayerResponseDtoFromProto(grpcResponse)));
        }
        catch (RpcException ex)
        {
            switch (ex.Status.StatusCode)
            {
                case Grpc.Core.StatusCode.NotFound:
                    return NotFound(RestApiResponseBuilder<LoginPlayerResponseDto>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
                case Grpc.Core.StatusCode.PermissionDenied:
                    return StatusCode(StatusCodes.Status403Forbidden, RestApiResponseBuilder<LoginPlayerResponseDto>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
                case Grpc.Core.StatusCode.FailedPrecondition:
                    return StatusCode(StatusCodes.Status412PreconditionFailed, RestApiResponseBuilder<LoginPlayerResponseDto>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
                default:
                    _logger.LogError($"Во время логина пользователя {request.Email} произошла ошибка: {ex.Message}");
                    return StatusCode(StatusCodes.Status500InternalServerError, RestApiResponseBuilder<LoginPlayerResponseDto>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
            }
        }
    }

    [HttpPost]
    [Route("confirm-email")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 200)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> ConfirmEmail([FromBody] ConfirmEmailRequestDto request)
    {
        try
        {
            var grpcResponse = await _authClient.ConfirmEmailAsync(request.ConfirmEmailRequestDtoToProto());

            return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
        }
        catch (RpcException ex)
        {
            if (ex.StatusCode == Grpc.Core.StatusCode.NotFound)
            {
                return StatusCode(StatusCodes.Status404NotFound, RestApiResponseBuilder<NoContent>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
            }
            if (ex.StatusCode == Grpc.Core.StatusCode.AlreadyExists)
            {
                return StatusCode(StatusCodes.Status409Conflict, RestApiResponseBuilder<NoContent>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
            }

            _logger.LogError($"Во время подтверждения почты: {request.Email} произошла ошибка: {ex.Message}");

            return StatusCode(StatusCodes.Status500InternalServerError, RestApiResponseBuilder<NoContent>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
        }
        catch (TimeoutException ex)
        {
            return StatusCode(StatusCodes.Status408RequestTimeout, RestApiResponseBuilder<NoContent>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, RestApiResponseBuilder<NoContent>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
        }
    }

    [HttpGet]
    [Route("confirm")]
    public async Task<IActionResult> СonfirmAccount([FromQuery] string token)
    {
        var playerId = _encryption.Decrypt<int>(token);

        try
        {
            var grpcResponse = await _authClient.ConfirmAccountAsync(new ConfirmAccountRequest() { PlayerId = playerId });
            var nickname = grpcResponse.Nickname;

            return Redirect($"{_domen}/Profile/Confirm?nickname={nickname}");
        }
        catch (RpcException ex)
        {
            switch (ex.Status.StatusCode)
            {
                case Grpc.Core.StatusCode.NotFound:
                    return NotFound(RestApiResponseBuilder<LoginPlayerResponseDto>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
                case Grpc.Core.StatusCode.PermissionDenied:
                    return StatusCode(StatusCodes.Status403Forbidden, RestApiResponseBuilder<LoginPlayerResponseDto>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
                default:
                    _logger.LogError($"Во время подтверждения почты пользователя с  id {playerId} произошла ошибка: {ex.Message}");
                    return StatusCode(StatusCodes.Status500InternalServerError, RestApiResponseBuilder<LoginPlayerResponseDto>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
            }
        }
    }

    [HttpPost]
    [Route("recovery-password")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 200)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> RecoveryPassword([FromBody] RecoveryPasswordRequestDto request)
    {
        try
        {
            var grpcResponse = await _authClient.RecoveryPasswordAsync(request.RecoveryPasswordRequestDtoToProto());

            return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
        }
        catch (RpcException ex)
        {
            switch (ex.Status.StatusCode)
            {
                case Grpc.Core.StatusCode.NotFound:
                    return NotFound(RestApiResponseBuilder<NoContent>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
                case Grpc.Core.StatusCode.PermissionDenied:
                    return StatusCode(StatusCodes.Status403Forbidden, RestApiResponseBuilder<NoContent>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
                case Grpc.Core.StatusCode.FailedPrecondition:
                    return StatusCode(StatusCodes.Status412PreconditionFailed, RestApiResponseBuilder<NoContent>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
                default:
                    _logger.LogError($"Во время восстановления пароля игрока с почтой: {request.Email} произошла ошибка: {ex.Message}");
                    return StatusCode(StatusCodes.Status500InternalServerError, RestApiResponseBuilder<NoContent>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
            }
        }
        catch (TimeoutException ex)
        {
            _logger.LogError($"Во время восстановления пароля игрока с почтой: {request.Email} произошла ошибка: {ex.Message}");
            return StatusCode(StatusCodes.Status408RequestTimeout, RestApiResponseBuilder<NoContent>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Во время восстановления пароля игрока с почтой: {request.Email} произошла ошибка: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, RestApiResponseBuilder<NoContent>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
        }
    }

    [HttpGet]
    [Route("recovery")]
    public async Task<IActionResult> RecoveryPassword([FromQuery] string token)
    {
        var playerId = _encryption.Decrypt<int>(token);

        try
        {
            var grpcResponse = await _authClient.ChangePasswordAsync(new ChangePasswordRequest() { PlayerId = playerId });

            return Redirect($"{_domen}/Profile/Recovery");
        }
        catch (RpcException ex)
        {
            switch (ex.Status.StatusCode)
            {
                case Grpc.Core.StatusCode.NotFound:
                    return NotFound(RestApiResponseBuilder<NoContent>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
                case Grpc.Core.StatusCode.PermissionDenied:
                    return StatusCode(StatusCodes.Status403Forbidden, RestApiResponseBuilder<NoContent>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
                case Grpc.Core.StatusCode.FailedPrecondition:
                    return StatusCode(StatusCodes.Status412PreconditionFailed, RestApiResponseBuilder<NoContent>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
                default:
                    _logger.LogError($"Во время подтверждения почты пользователя с  id {playerId} произошла ошибка: {ex.Message}");
                    return StatusCode(StatusCodes.Status500InternalServerError, RestApiResponseBuilder<NoContent>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
            }
        }
    }


    [HttpPost]
    [Authorize]
    [Route("logout")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 200)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Logout(LogoutRequestDto request)
    {
        try
        {
            var grpcResponse = await _authClient.LogoutAsync(request.LogoutRequestDtoToProto());

            return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
        }
        catch (RpcException ex)
        {
            if (ex.StatusCode == Grpc.Core.StatusCode.NotFound)
            {
                return StatusCode(StatusCodes.Status404NotFound, RestApiResponseBuilder<NoContent>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
            }

            _logger.LogError($"Во время удаления рефреш токена: {request.RefreshToken} произошла ошибка: {ex.Message}");

            return StatusCode(StatusCodes.Status500InternalServerError, RestApiResponseBuilder<NoContent>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
        }
    }

    [HttpPost]
    [Authorize]
    [Route("logout-from-all-devices")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 200)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> LogoutFromAllDevices()
    {
        try
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
        catch (RpcException ex)
        {
            if (ex.StatusCode == Grpc.Core.StatusCode.NotFound)
            {
                return StatusCode(StatusCodes.Status404NotFound, RestApiResponseBuilder<NoContent>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
            }

            _logger.LogError($"Во время выхода со всех устройств произошла ошибка: {ex.Message}");

            return StatusCode(StatusCodes.Status500InternalServerError, RestApiResponseBuilder<NoContent>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
        }
    }

    [HttpPost]
    [Route("refresh")]
    [ProducesResponseType(typeof(RestApiResponse<RefreshTokenResponseDto>), 200)]
    public async Task<ActionResult<RestApiResponse<RefreshTokenResponseDto>>> Refresh([FromBody] RefreshTokenRequestDto request)
    {
        try
        {
            var grpcResponse = await _authClient.RefreshAsync(request.RefreshTokenRequestDtoToProto());

            return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<RefreshTokenResponseDto>.Success(new RefreshTokenResponseDto().RefreshTokenResponseDtoFromProto(grpcResponse)));
        }
        catch (RpcException ex)
        {
            switch (ex.Status.StatusCode)
            {
                case Grpc.Core.StatusCode.NotFound:
                    return NotFound(RestApiResponseBuilder<LoginPlayerResponseDto>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
                case Grpc.Core.StatusCode.PermissionDenied:
                    return StatusCode(StatusCodes.Status403Forbidden, RestApiResponseBuilder<LoginPlayerResponseDto>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
                case Grpc.Core.StatusCode.FailedPrecondition:
                    return StatusCode(StatusCodes.Status412PreconditionFailed, RestApiResponseBuilder<LoginPlayerResponseDto>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
                default:
                    _logger.LogError($"Во время обновления токена {request.RefreshToken} произошла ошибка: {ex.Message}");
                    return StatusCode(StatusCodes.Status500InternalServerError, RestApiResponseBuilder<LoginPlayerResponseDto>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
            }
        }
    }
}
