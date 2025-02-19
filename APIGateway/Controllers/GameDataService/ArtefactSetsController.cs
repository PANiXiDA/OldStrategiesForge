using APIGateway.Infrastructure.Core;
using Asp.Versioning;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GameData.ArtefactSets.Gen;
using APIGateway.Infrastructure.GameDataService.Models.ArtefactSets;
using APIGateway.Infrastructure.GameDataService.Models.ArtefactSets.Params;
using APIGateway.Infrastructure.GameDataService.Responses.ArtefactSets;
using APIGateway.Infrastructure.GameDataService.Requests.ArtefactSets;

namespace APIGateway.Controllers.GameDataService;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/artefact-sets")]
[Produces("application/json")]
[ApiController]
public class ArtefactSetsController : ControllerBase
{
    private readonly ILogger<ArtefactSetsController> _logger;
    private readonly ArtefactSetsService.ArtefactSetsServiceClient _artefactSetsService;

    public ArtefactSetsController(
        ILogger<ArtefactSetsController> logger,
        ArtefactSetsService.ArtefactSetsServiceClient artefactSetsService)
    {
        _logger = logger;
        _artefactSetsService = artefactSetsService;
    }

    [HttpGet]
    [Route("{id:int}")]
    [Authorize]
    [ProducesResponseType(typeof(RestApiResponse<ArtefactSetDto>), 200)]
    public async Task<ActionResult<RestApiResponse<ArtefactSetDto>>> Get([FromRoute] int id, [FromQuery] ArtefactSetsConvertParamsDto? сonvertParams)
    {
        var response = await _artefactSetsService.GetAsync(new GetArtefactSetRequest
        {
            Id = id,
            ArtefactSetsConvertParams = сonvertParams != null ? ArtefactSetsConvertParamsDto.ToEntity(сonvertParams) : null
        });

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<ArtefactSetDto>.Success(ArtefactSetDto.FromEntity(response)));
    }

    [HttpGet]
    [Authorize]
    [Route("get-by-filter")]
    [ProducesResponseType(typeof(RestApiResponse<GetArtefactSetsResponseDto>), 200)]
    public async Task<ActionResult<RestApiResponse<GetArtefactSetsResponseDto>>> Get([FromQuery] GetArtefactSetsRequestDto request)
    {
        var response = await _artefactSetsService.GetByFilterAsync(GetArtefactSetsRequestDto.ToEntity(request));

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<GetArtefactSetsResponseDto>.Success(GetArtefactSetsResponseDto.FromEntity(response)));
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 201)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Create([FromBody] ArtefactSetDto request)
    {
        await _artefactSetsService.CreateOrUpdateAsync(ArtefactSetDto.ToEntity(request));

        return StatusCode(StatusCodes.Status201Created, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }

    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 200)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Update([FromRoute] int id, [FromBody] ArtefactSetDto request)
    {
        var entity = ArtefactSetDto.ToEntity(request);
        entity.Id = id;

        await _artefactSetsService.CreateOrUpdateAsync(entity);

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }

    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 204)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Delete([FromRoute] int id)
    {
        await _artefactSetsService.DeleteAsync(new DeleteArtefactSetRequest { Id = id });

        return StatusCode(StatusCodes.Status204NoContent, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }
}
