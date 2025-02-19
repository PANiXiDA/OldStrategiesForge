using APIGateway.Infrastructure.Core;
using Asp.Versioning;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GameData.Spells.Gen;
using APIGateway.Infrastructure.GameDataService.Models.Spells;
using APIGateway.Infrastructure.GameDataService.Models.Spells.Params;
using APIGateway.Infrastructure.GameDataService.Responses.Spells;
using APIGateway.Infrastructure.GameDataService.Requests.Spells;

namespace APIGateway.Controllers.GameDataService;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/spells")]
[Produces("application/json")]
[ApiController]
public class SpellsController : ControllerBase
{
    private readonly ILogger<SpellsController> _logger;
    private readonly SpellsService.SpellsServiceClient _spellsService;

    public SpellsController(
        ILogger<SpellsController> logger,
        SpellsService.SpellsServiceClient spellsService)
    {
        _logger = logger;
        _spellsService = spellsService;
    }

    [HttpGet]
    [Route("{id:int}")]
    [Authorize]
    [ProducesResponseType(typeof(RestApiResponse<SpellDto>), 200)]
    public async Task<ActionResult<RestApiResponse<SpellDto>>> Get([FromRoute] int id, [FromQuery] SpellsConvertParamsDto? сonvertParams)
    {
        var response = await _spellsService.GetAsync(new GetSpellRequest
        {
            Id = id,
            SpellsConvertParams = сonvertParams != null ? SpellsConvertParamsDto.ToEntity(сonvertParams) : null
        });

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<SpellDto>.Success(SpellDto.FromEntity(response)));
    }

    [HttpGet]
    [Authorize]
    [Route("get-by-filter")]
    [ProducesResponseType(typeof(RestApiResponse<GetSpellsResponseDto>), 200)]
    public async Task<ActionResult<RestApiResponse<GetSpellsResponseDto>>> Get([FromQuery] GetSpellsRequestDto request)
    {
        var response = await _spellsService.GetByFilterAsync(GetSpellsRequestDto.ToEntity(request));

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<GetSpellsResponseDto>.Success(GetSpellsResponseDto.FromEntity(response)));
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 201)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Create([FromBody] SpellDto request)
    {
        await _spellsService.CreateOrUpdateAsync(SpellDto.ToEntity(request));

        return StatusCode(StatusCodes.Status201Created, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }

    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 200)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Update([FromRoute] int id, [FromBody] SpellDto request)
    {
        var entity = SpellDto.ToEntity(request);
        entity.Id = id;

        await _spellsService.CreateOrUpdateAsync(entity);

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }

    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 204)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Delete([FromRoute] int id)
    {
        await _spellsService.DeleteAsync(new DeleteSpellRequest { Id = id });

        return StatusCode(StatusCodes.Status204NoContent, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }
}
