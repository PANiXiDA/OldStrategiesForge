using Microsoft.AspNetCore.Mvc;
using Profile.Players.Gen;
using Asp.Versioning;
using APIGateway.Infrastructure.Extensions;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Common;
using System.Security.Claims;
using APIGateway.Infrastructure.Responses.Players;

namespace APIGateway.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/players")]
[Produces("application/json")]
[ApiController]
public class PlayersController : ControllerBase
{
    private readonly ILogger<PlayersController> _logger;
    private readonly ProfilePlayers.ProfilePlayersClient _playersClient;

    public PlayersController(ILogger<PlayersController> logger, ProfilePlayers.ProfilePlayersClient playersClient)
    {
        _logger = logger;
        _playersClient = playersClient;
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(RestApiResponse<GetPlayerResponseDto>), 200)]
    public async Task<ActionResult<RestApiResponse<GetPlayerResponseDto>>> Get()
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, RestApiResponseBuilder<GetPlayerResponseDto>.Fail(Constants.ErrorMessages.Unauthorized, Constants.ErrorMessages.ErrorKey));
            }

            int userId = int.Parse(userIdClaim.Value);

            var grpcResponse = await _playersClient.GetAsync(new GetPlayerRequest() { Id = userId });

            return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<GetPlayerResponseDto>.Success(new GetPlayerResponseDto().GetPlayerResponseDtoFromProto(grpcResponse)));
        }
        catch (RpcException ex)
        {
            if (ex.StatusCode == Grpc.Core.StatusCode.NotFound)
            {
                return NotFound(RestApiResponseBuilder<GetPlayerResponseDto>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
            }

            _logger.LogError($"При получении пользователя произошла ошибка: {ex.Message}");

            return StatusCode(StatusCodes.Status500InternalServerError, RestApiResponseBuilder<GetPlayerResponseDto>.Fail(ex.Message, Constants.ErrorMessages.ErrorKey));
        }
    }
}
