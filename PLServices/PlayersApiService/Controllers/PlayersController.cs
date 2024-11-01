using Common;
using Grpc.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PlayersApiService.Common;
using PlayersApiService.Infrastructure;
using PlayersApiService.Infrastructure.Requests.Players;
using PlayersApiService.Infrastructure.Responses.Core;
using PlayersApiService.Protos;

namespace PlayersApiService.Controllers;

[Route("api/v1/players")]
[Produces("application/json")]
[ApiController]
public class PlayersController : ControllerBase
{
    private readonly PlayersAPI.PlayersAPIClient _grpcClient;
    private readonly ILogger<PlayersController> _logger;

    public  PlayersController(PlayersAPI.PlayersAPIClient grpcClient, ILogger<PlayersController> logger)
    {
        _grpcClient = grpcClient;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(RestApiResponse<CreatePlayerProtoResponse>), 200)]
    [Route("create")]
    public async Task<ActionResult<RestApiResponse<CreatePlayerProtoResponse>>> Create([FromBody] CreatePlayerRequest request)
    {
        if (request.Password != request.RepeatPassword)
        {
            return BadRequest(RestApiResponseBuilder<CreatePlayerProtoResponse>.Fail(Constants.ErrorMessages.PasswordMustMatch, Constants.ErrorMessages.ErrorKey));
        }

        if (!Helpers.IsPasswordValid(request.Password))
        {
            return BadRequest(RestApiResponseBuilder<CreatePlayerProtoResponse>.Fail(Constants.ErrorMessages.NotValidPassword, Constants.ErrorMessages.ErrorKey));
        }

        if (!Helpers.IsEmailValid(request.Email))
        {
            return BadRequest(RestApiResponseBuilder<CreatePlayerProtoResponse>.Fail(Constants.ErrorMessages.NotValidEmail, Constants.ErrorMessages.ErrorKey));
        }

        var grpcRequest = new CreatePlayerProtoRequest
        {
            Email = request.Email,
            Nickname = request.Nickname,
            Password = request.Password
        };

        try
        {
            var grpcResponse = await _grpcClient.CreateAsync(grpcRequest);

            return Ok(RestApiResponseBuilder<CreatePlayerProtoResponse>.Success(grpcResponse));
        }
        catch (RpcException ex)
        {
            _logger.LogError($"Во время создания пользователя {request.Nickname} произошла ошибка: {ex.Message}");

            return StatusCode(StatusCodes.Status500InternalServerError, RestApiResponseBuilder<CreatePlayerProtoResponse>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Во время создания пользователя {request.Nickname} произошла ошибка: {ex.Message}");

            return StatusCode(StatusCodes.Status500InternalServerError, RestApiResponseBuilder<CreatePlayerProtoResponse>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
        }
    }

}
