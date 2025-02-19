using APIGateway.Infrastructure.Core;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using GameData.Abilities.Gen;
using APIGateway.Infrastructure.GameDataService.Models.Abilities.Params;
using APIGateway.Infrastructure.GameDataService.Models.Abilities;
using APIGateway.Infrastructure.GameDataService.Responses.Abilities;
using APIGateway.Infrastructure.GameDataService.Requests.Abilities;

namespace APIGateway.Controllers.GameDataService;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/abilities")]
[Produces("application/json")]
[ApiController]
public class AbilitiesController : ControllerBase
{
    private readonly ILogger<AbilitiesController> _logger;
    private readonly AbilitiesService.AbilitiesServiceClient _abilitiesService;

    public AbilitiesController(
        ILogger<AbilitiesController> logger,
        AbilitiesService.AbilitiesServiceClient abilitiesService)
    {
        _logger = logger;
        _abilitiesService = abilitiesService;
    }

    [HttpGet]
    [Route("{id:int}")]
    [Authorize]
    [ProducesResponseType(typeof(RestApiResponse<AbilityDto>), 200)]
    public async Task<ActionResult<RestApiResponse<AbilityDto>>> Get([FromRoute] int id, [FromQuery] AbilitiesConvertParamsDto? сonvertParams)
    {
        var response = await _abilitiesService.GetAsync(new GetAbilityRequest
        {
            Id = id,
            AbilitiesConvertParams = сonvertParams != null ? AbilitiesConvertParamsDto.ToEntity(сonvertParams) : null
        });

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<AbilityDto>.Success(AbilityDto.FromEntity(response)));
    }

    [HttpGet]
    [Authorize]
    [Route("get-by-filter")]
    [ProducesResponseType(typeof(RestApiResponse<GetAbilitiesResponseDto>), 200)]
    public async Task<ActionResult<RestApiResponse<GetAbilitiesResponseDto>>> Get([FromQuery] GetAbilitiesRequestDto request)
    {
        var response = await _abilitiesService.GetByFilterAsync(GetAbilitiesRequestDto.ToEntity(request));

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<GetAbilitiesResponseDto>.Success(GetAbilitiesResponseDto.FromEntity(response)));
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 201)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Create([FromBody] AbilityDto request)
    {
        await _abilitiesService.CreateOrUpdateAsync(AbilityDto.ToEntity(request));

        return StatusCode(StatusCodes.Status201Created, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }

    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 200)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Update([FromRoute] int id, [FromBody] AbilityDto request)
    {
        var entity = AbilityDto.ToEntity(request);
        entity.Id = id;

        await _abilitiesService.CreateOrUpdateAsync(entity);

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }

    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 204)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Delete([FromRoute] int id)
    {
        await _abilitiesService.DeleteAsync(new DeleteAbilityRequest { Id = id });

        return StatusCode(StatusCodes.Status204NoContent, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }
}
