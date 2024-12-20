using Microsoft.AspNetCore.Mvc;
using Profile.Players.Gen;
using Asp.Versioning;
using APIGateway.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Common.Constants;
using System.Security.Claims;
using APIGateway.Infrastructure.Responses.Players;
using APIGateway.Infrastructure.Requests.Players;
using ImageService.S3Images.Gen;

namespace APIGateway.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/players")]
[Produces("application/json")]
[ApiController]
public class PlayersController : ControllerBase
{
    private readonly ILogger<PlayersController> _logger;
    private readonly ProfilePlayers.ProfilePlayersClient _playersClient;
    private readonly S3Images.S3ImagesClient _s3ImagesClient;

    public PlayersController(
        ILogger<PlayersController> logger,
        ProfilePlayers.ProfilePlayersClient playersClient,
        S3Images.S3ImagesClient s3ImagesClient)
    {
        _logger = logger;
        _playersClient = playersClient;
        _s3ImagesClient = s3ImagesClient;
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(RestApiResponse<GetPlayerResponseDto>), 200)]
    public async Task<ActionResult<RestApiResponse<GetPlayerResponseDto>>> Get()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return StatusCode(StatusCodes.Status401Unauthorized, RestApiResponseBuilder<GetPlayerResponseDto>.Fail(ErrorMessages.Unauthorized, ErrorMessages.ErrorKey));
        }
        int userId = int.Parse(userIdClaim.Value);

        var getPlayerResponse = await _playersClient.GetAsync(new GetPlayerRequest() { Id = userId });

        var avatarPresignedUrlResponse = await _s3ImagesClient.GetPresignedUrlAsync(new GetPresignedUrlRequest() { S3Paths = { getPlayerResponse.Avatar.S3Path } });
        getPlayerResponse.Avatar.S3Path = avatarPresignedUrlResponse.FileUrls.First();

        var framePresignedUrlResponse = await _s3ImagesClient.GetPresignedUrlAsync(new GetPresignedUrlRequest() { S3Paths = { getPlayerResponse.Frame.S3Path } });
        getPlayerResponse.Frame.S3Path = framePresignedUrlResponse.FileUrls.First();

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<GetPlayerResponseDto>.Success(GetPlayerResponseDto.GetPlayerResponseDtoFromProto(getPlayerResponse)));
    }

    [HttpPost]
    [Authorize]
    [Route("update-avatar")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 200)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> UpdateAvatar(UpdateAvatarRequestDto request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return StatusCode(StatusCodes.Status401Unauthorized, RestApiResponseBuilder<NoContent>.Fail(ErrorMessages.Unauthorized, ErrorMessages.ErrorKey));
        }
        int userId = int.Parse(userIdClaim.Value);

        await _playersClient.UpdateAvatarAsync(new UpdateAvatarRequest()
        {
            AvatarId = request.AvatarId,
            PlayerId = userId
        });

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }

    [HttpPost]
    [Authorize]
    [Route("update-frame")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 200)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> UpdateFrame(UpdateFrameRequestDto request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return StatusCode(StatusCodes.Status401Unauthorized, RestApiResponseBuilder<NoContent>.Fail(ErrorMessages.Unauthorized, ErrorMessages.ErrorKey));
        }
        int userId = int.Parse(userIdClaim.Value);

        await _playersClient.UpdateFrameAsync(new UpdateFrameRequest()
        {
            FrameId = request.FrameId,
            PlayerId = userId
        });

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }
}
