using Asp.Versioning;
using Auth.Backend.Gen;
using Common;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using ProfileApiService.Infrastructure.Extensions;
using ProfileApiService.Infrastructure.Requests.Auth;
using ProfileApiService.Infrastructure.Responses.Auth;

namespace ProfileApiService.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
[Produces("application/json")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly AuthBackend.AuthBackendClient _authBackendClient;

    public AuthController(ILogger<AuthController> logger, AuthBackend.AuthBackendClient authBackendClient)
    {
        _logger = logger;
        _authBackendClient = authBackendClient;
    }

    [HttpPost]
    [Route("sign-up")]
    [ProducesResponseType(typeof(RestApiResponse<RegistrationPlayerResponseDto>), 201)]
    public async Task<ActionResult<RestApiResponse<RegistrationPlayerResponseDto>>> Registration([FromBody] RegistrationPlayerRequestDto request)
    {
        var validator = new CreatePlayerDtoValidator();
        var results = validator.Validate(request);
        if (!results.IsValid)
        {
            return BadRequest(RestApiResponseBuilder<RegistrationPlayerResponseDto>.Fail(results.Errors.First().ErrorMessage, results.Errors.First().ErrorCode));
        }

        try
        {
            var grpcResponse = await _authBackendClient.RegistrationAsync(request.RegistrationPlayerRequestDtoToProto());

            return StatusCode(StatusCodes.Status201Created, RestApiResponseBuilder<RegistrationPlayerResponseDto>.Success(new RegistrationPlayerResponseDto().RegistrationPlayerResponseDtoFromProto(grpcResponse)));
        }
        catch (RpcException ex)
        {
            _logger.LogError($"Во время создания пользователя {request.Nickname} произошла ошибка: {ex.Message}");

            if (ex.StatusCode == Grpc.Core.StatusCode.AlreadyExists)
            {
                return StatusCode(StatusCodes.Status409Conflict, RestApiResponseBuilder<RegistrationPlayerResponseDto>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
            }
            return StatusCode(StatusCodes.Status500InternalServerError, RestApiResponseBuilder<RegistrationPlayerResponseDto>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
        }
    }

    [HttpPost]
    [Route("login")]
    [ProducesResponseType(typeof(RestApiResponse<LoginPlayerResponseDto>), 200)]
    public async Task<ActionResult<RestApiResponse<LoginPlayerResponseDto>>> Registration([FromBody] LoginPlayerRequestDto request)
    {
        try
        {
            var grpcResponse = await _authBackendClient.LoginAsync(request.LoginPlayerRequestDtoToProto());

            return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<LoginPlayerResponseDto>.Success(new LoginPlayerResponseDto().LoginPlayerResponseDtoFromProto(grpcResponse)));
        }
        catch (RpcException ex)
        {
            _logger.LogError($"Во время логина пользователя {request.Email} произошла ошибка: {ex.Message}");

            switch (ex.Status.StatusCode)
            {
                case Grpc.Core.StatusCode.NotFound:
                    return NotFound(RestApiResponseBuilder<LoginPlayerResponseDto>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
                case Grpc.Core.StatusCode.FailedPrecondition:
                    return StatusCode(StatusCodes.Status412PreconditionFailed, RestApiResponseBuilder<LoginPlayerResponseDto>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
                case Grpc.Core.StatusCode.PermissionDenied:
                    return StatusCode(StatusCodes.Status403Forbidden, RestApiResponseBuilder<LoginPlayerResponseDto>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
                default:
                    return StatusCode(StatusCodes.Status500InternalServerError, RestApiResponseBuilder<LoginPlayerResponseDto>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
            }
        }

    }
}
