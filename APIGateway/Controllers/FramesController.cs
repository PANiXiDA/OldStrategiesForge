using APIGateway.Infrastructure.Extensions;
using APIGateway.Infrastructure.Requests.Frames;
using APIGateway.Infrastructure.Responses.Frames;
using Asp.Versioning;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profile.Frames.Gen;

namespace APIGateway.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/frames")]
[Produces("application/json")]
[ApiController]
public class FramesController : ControllerBase
{
    private readonly ILogger<FramesController> _logger;
    private readonly ProfileFrames.ProfileFramesClient _framesClient;

    public FramesController(
        ILogger<FramesController> logger,
        ProfileFrames.ProfileFramesClient framesClient)
    {
        _logger = logger;
        _framesClient = framesClient;
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 201)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Create([FromBody] CreateFrameRequestDto request)
    {
        var grpcResponse = await _framesClient.CreateAsync(request.CreateFrameRequestDtoToProto());

        return StatusCode(StatusCodes.Status201Created, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }

    [HttpGet]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<GetFrameResponseDto>), 200)]
    public async Task<ActionResult<RestApiResponse<GetFrameResponseDto>>> Get([FromQuery] int id)
    {
        var grpcResponse = await _framesClient.GetAsync(new GetFrameRequest() { Id = id });

        return StatusCode(StatusCodes.Status201Created, RestApiResponseBuilder<GetFrameResponseDto>.Success(GetFrameResponseDto.GetFrameResponseFromProtoToDto(grpcResponse)));
    }

    [HttpGet]
    [Authorize]
    [Route("get-available")]
    [ProducesResponseType(typeof(RestApiResponse<List<GetFrameResponseDto>>), 200)]
    public async Task<ActionResult<RestApiResponse<List<GetFrameResponseDto>>>> GetAvailable()
    {
        var grpcResponse = await _framesClient.GetAvailableAsync(new Google.Protobuf.WellKnownTypes.Empty());
        var avatars = new List<GetFrameResponseDto>();
        foreach (var avatar in grpcResponse.Avatars)
        {
            avatars.Add(GetFrameResponseDto.GetFrameResponseFromProtoToDto(avatar));
        }

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<List<GetFrameResponseDto>>.Success(avatars));
    }


    [HttpPut]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 200)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Update([FromBody] UpdateFrameRequestDto request)
    {
        var grpcResponse = await _framesClient.UpdateAsync(request.UpdateFrameRequestDtoToProto());

        return StatusCode(StatusCodes.Status201Created, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }

    [HttpDelete]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 204)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Delete([FromQuery] int id)
    {
        var grpcResponse = await _framesClient.DeleteAsync(new DeleteFrameRequest() { Id = id });

        return StatusCode(StatusCodes.Status204NoContent, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }
}
