using Asp.Versioning;
using Profile.Auth.Gen;
using Common;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using APIGateway.Infrastructure.Extensions;
using APIGateway.Infrastructure.Requests.Auth;
using APIGateway.Infrastructure.Responses.Auth;


namespace APIGateway.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
[Produces("application/json")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly ProfileAuth.ProfileAuthClient _authClient;

    public AuthController(ILogger<AuthController> logger, ProfileAuth.ProfileAuthClient authClient)
    {
        _logger = logger;
        _authClient = authClient;
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
    }

    [HttpPost]
    [Route("login")]
    [ProducesResponseType(typeof(RestApiResponse<LoginPlayerResponseDto>), 200)]
    public async Task<ActionResult<RestApiResponse<LoginPlayerResponseDto>>> Registration([FromBody] LoginPlayerRequestDto request)
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
}
