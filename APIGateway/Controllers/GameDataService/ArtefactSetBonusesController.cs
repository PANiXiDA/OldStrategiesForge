using APIGateway.Infrastructure.Core;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GameData.ArtefactSetBonuses.Gen;
using APIGateway.Infrastructure.GameDataService.Models.ArtefactSetBonuses;
using APIGateway.Infrastructure.GameDataService.Models.ArtefactSetBonuses.Params;
using APIGateway.Infrastructure.GameDataService.Responses.ArtefactSetBonuses;
using APIGateway.Infrastructure.GameDataService.Requests.ArtefactSetBonuses;
using Asp.Versioning;

namespace APIGateway.Controllers.GameDataService;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/artefact-set-bonuses")]
[Produces("application/json")]
[ApiController]
public class ArtefactSetBonusesController : ControllerBase
{
    private readonly ILogger<ArtefactSetBonusesController> _logger;
    private readonly ArtefactSetBonusesService.ArtefactSetBonusesServiceClient _artefactSetBonusesService;

    public ArtefactSetBonusesController(
        ILogger<ArtefactSetBonusesController> logger,
        ArtefactSetBonusesService.ArtefactSetBonusesServiceClient artefactSetBonusesService)
    {
        _logger = logger;
        _artefactSetBonusesService = artefactSetBonusesService;
    }

    [HttpGet]
    [Route("{id:int}")]
    [Authorize]
    [ProducesResponseType(typeof(RestApiResponse<ArtefactSetBonusDto>), 200)]
    public async Task<ActionResult<RestApiResponse<ArtefactSetBonusDto>>> Get([FromRoute] int id, [FromQuery] ArtefactSetBonusesConvertParamsDto? сonvertParams)
    {
        var response = await _artefactSetBonusesService.GetAsync(new GetArtefactSetBonusRequest
        {
            Id = id,
            ArtefactSetBonusesConvertParams = сonvertParams != null ? ArtefactSetBonusesConvertParamsDto.ToEntity(сonvertParams) : null
        });

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<ArtefactSetBonusDto>.Success(ArtefactSetBonusDto.FromEntity(response)));
    }

    [HttpGet]
    [Authorize]
    [Route("get-by-filter")]
    [ProducesResponseType(typeof(RestApiResponse<GetArtefactSetBonusesResponseDto>), 200)]
    public async Task<ActionResult<RestApiResponse<GetArtefactSetBonusesResponseDto>>> Get([FromQuery] GetArtefactSetBonusesRequestDto request)
    {
        var response = await _artefactSetBonusesService.GetByFilterAsync(GetArtefactSetBonusesRequestDto.ToEntity(request));

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<GetArtefactSetBonusesResponseDto>.Success(GetArtefactSetBonusesResponseDto.FromEntity(response)));
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 201)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Create([FromBody] ArtefactSetBonusDto request)
    {
        await _artefactSetBonusesService.CreateOrUpdateAsync(ArtefactSetBonusDto.ToEntity(request));

        return StatusCode(StatusCodes.Status201Created, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }

    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 200)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Update([FromRoute] int id, [FromBody] ArtefactSetBonusDto request)
    {
        var entity = ArtefactSetBonusDto.ToEntity(request);
        entity.Id = id;

        await _artefactSetBonusesService.CreateOrUpdateAsync(entity);

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }

    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 204)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Delete([FromRoute] int id)
    {
        await _artefactSetBonusesService.DeleteAsync(new DeleteArtefactSetBonusRequest { Id = id });

        return StatusCode(StatusCodes.Status204NoContent, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }
}
