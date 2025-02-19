using APIGateway.Infrastructure.Core;
using Asp.Versioning;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GameData.Artefacts.Gen;
using APIGateway.Infrastructure.GameDataService.Models.Artefacts;
using APIGateway.Infrastructure.GameDataService.Models.Artefacts.Params;
using APIGateway.Infrastructure.GameDataService.Responses.Artefacts;
using APIGateway.Infrastructure.GameDataService.Requests.Artefacts;

namespace APIGateway.Controllers.GameDataService;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/artefacts")]
[Produces("application/json")]
[ApiController]
public class ArtefactsController : ControllerBase
{
    private readonly ILogger<ArtefactsController> _logger;
    private readonly ArtefactsService.ArtefactsServiceClient _artefactsService;

    public ArtefactsController(
        ILogger<ArtefactsController> logger,
        ArtefactsService.ArtefactsServiceClient artefactsService)
    {
        _logger = logger;
        _artefactsService = artefactsService;
    }

    [HttpGet]
    [Route("{id:int}")]
    [Authorize]
    [ProducesResponseType(typeof(RestApiResponse<ArtefactDto>), 200)]
    public async Task<ActionResult<RestApiResponse<ArtefactDto>>> Get([FromRoute] int id, [FromQuery] ArtefactsConvertParamsDto? сonvertParams)
    {
        var response = await _artefactsService.GetAsync(new GetArtefactRequest
        {
            Id = id,
            ArtefactsConvertParams = сonvertParams != null ? ArtefactsConvertParamsDto.ToEntity(сonvertParams) : null
        });

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<ArtefactDto>.Success(ArtefactDto.FromEntity(response)));
    }

    [HttpGet]
    [Authorize]
    [Route("get-by-filter")]
    [ProducesResponseType(typeof(RestApiResponse<GetArtefactsResponseDto>), 200)]
    public async Task<ActionResult<RestApiResponse<GetArtefactsResponseDto>>> Get([FromQuery] GetArtefactsRequestDto request)
    {
        var response = await _artefactsService.GetByFilterAsync(GetArtefactsRequestDto.ToEntity(request));

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<GetArtefactsResponseDto>.Success(GetArtefactsResponseDto.FromEntity(response)));
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 201)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Create([FromBody] ArtefactDto request)
    {
        await _artefactsService.CreateOrUpdateAsync(ArtefactDto.ToEntity(request));

        return StatusCode(StatusCodes.Status201Created, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }

    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 200)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Update([FromRoute] int id, [FromBody] ArtefactDto request)
    {
        var entity = ArtefactDto.ToEntity(request);
        entity.Id = id;

        await _artefactsService.CreateOrUpdateAsync(entity);

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }

    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 204)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Delete([FromRoute] int id)
    {
        await _artefactsService.DeleteAsync(new DeleteArtefactRequest { Id = id });

        return StatusCode(StatusCodes.Status204NoContent, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }
}
