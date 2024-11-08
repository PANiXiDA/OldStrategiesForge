using Common;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using ProfileApiService.Common;
using ProfileApiService.Infrastructure.Extensions;
using Players.Backend.Gen;
using ProfileApiService.Infrastructure.Models.Players;

namespace ProfileApiService.Controllers;

[Route("api/v1/players")]
[Produces("application/json")]
[ApiController]
public class PlayersController : ControllerBase
{
    private readonly ILogger<PlayersController> _logger;
    private readonly PlayersBackend.PlayersBackendClient _playersBackendClient;

    public PlayersController(ILogger<PlayersController> logger, PlayersBackend.PlayersBackendClient playersBackendClient)
    {
        _logger = logger;
        _playersBackendClient = playersBackendClient;
    }

    [HttpPost]
    [ProducesResponseType(typeof(RestApiResponse<CreatePlayerResponse>), 200)]
    public async Task<ActionResult<RestApiResponse<CreatePlayerResponse>>> Create([FromBody] CreatePlayerDto request)
    {
        //if (request.Password != request.RepeatPassword)
        //{
        //    return BadRequest(RestApiResponseBuilder<CreatePlayerResponse>.Fail(Constants.ErrorMessages.PasswordMustMatch, Constants.ErrorMessages.ErrorKey));
        //}

        //if (!Helpers.IsPasswordValid(request.Password))
        //{
        //    return BadRequest(RestApiResponseBuilder<CreatePlayerResponse>.Fail(Constants.ErrorMessages.NotValidPassword, Constants.ErrorMessages.ErrorKey));
        //}

        var grpcRequest = new CreatePlayerRequest
        {
            Email = request.Email,
            Nickname = request.Nickname,
            Password = request.Password
        };

        try
        {
            var grpcResponse = await _playersBackendClient.CreateAsync(grpcRequest);

            return Ok(RestApiResponseBuilder<CreatePlayerResponse>.Success(grpcResponse));
        }
        catch (RpcException ex)
        {
            _logger.LogError($"Во время создания пользователя {request.Nickname} произошла ошибка: {ex.Message}");

            return StatusCode(StatusCodes.Status500InternalServerError, RestApiResponseBuilder<CreatePlayerResponse>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
        }
    }
}
