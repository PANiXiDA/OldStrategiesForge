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
[Route("v{version:apiVersion}/auth")]
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
    [ProducesResponseType(typeof(RestApiResponse<RegistrationPlayerResponseDto>), 200)]
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
}
