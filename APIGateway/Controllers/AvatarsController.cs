using APIGateway.Infrastructure.Extensions;
using APIGateway.Infrastructure.Requests.Avatars;
using APIGateway.Infrastructure.Responses.Avatars;
using Asp.Versioning;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profile.Avatar.Gen;

namespace APIGateway.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/avatars")]
[Produces("application/json")]
[ApiController]
public class AvatarsController : ControllerBase
{
    private readonly ILogger<PlayersController> _logger;
    private readonly ProfileAvatars.ProfileAvatarsClient _avatarsClient;

    public AvatarsController(
        ILogger<PlayersController> logger,
        ProfileAvatars.ProfileAvatarsClient avatarsClient)
    {
        _logger = logger;
        _avatarsClient = avatarsClient;
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 201)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Create([FromBody] CreateAvatarRequestDto request)
    {
        var grpcResponse = await _avatarsClient.CreateAsync(request.CreateAvatarRequestDtoToProto());

        return StatusCode(StatusCodes.Status201Created, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }

    [HttpGet]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<GetAvatarResponseDto>), 200)]
    public async Task<ActionResult<RestApiResponse<GetAvatarResponseDto>>> Get([FromQuery] int id)
    {
        var grpcResponse = await _avatarsClient.GetAsync(new GetAvatarRequest() { Id = id });

        return StatusCode(StatusCodes.Status201Created, RestApiResponseBuilder<GetAvatarResponseDto>.Success(GetAvatarResponseDto.GetAvatarResponseFromProtoToDto(grpcResponse)));
    }

    [HttpPut]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 200)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Update([FromBody] UpdateAvatarRequestDto request)
    {
        var grpcResponse = await _avatarsClient.UpdateAsync(request.UpdateAvatarRequestDtoToProto());

        return StatusCode(StatusCodes.Status201Created, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }

    [HttpDelete]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 204)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Delete([FromQuery] int id)
    {
        var grpcResponse = await _avatarsClient.DeleteAsync(new DeleteAvatarRequest() { Id = id });

        return StatusCode(StatusCodes.Status204NoContent, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }
}
