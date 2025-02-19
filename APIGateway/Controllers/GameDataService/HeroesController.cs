using APIGateway.Infrastructure.Core;
using Asp.Versioning;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GameData.Heroes.Gen;
using APIGateway.Infrastructure.GameDataService.Models.Heroes;
using APIGateway.Infrastructure.GameDataService.Models.Heroes.Params;
using APIGateway.Infrastructure.GameDataService.Responses.Heroes;
using APIGateway.Infrastructure.GameDataService.Requests.Heroes;

namespace APIGateway.Controllers.GameDataService;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/heroes")]
[Produces("application/json")]
[ApiController]
public class HeroesController : ControllerBase
{
    private readonly ILogger<HeroesController> _logger;
    private readonly HeroesService.HeroesServiceClient _heroesService;

    public HeroesController(
        ILogger<HeroesController> logger,
        HeroesService.HeroesServiceClient heroesService)
    {
        _logger = logger;
        _heroesService = heroesService;
    }

    [HttpGet]
    [Route("{id:int}")]
    [Authorize]
    [ProducesResponseType(typeof(RestApiResponse<HeroDto>), 200)]
    public async Task<ActionResult<RestApiResponse<HeroDto>>> Get([FromRoute] int id, [FromQuery] HeroesConvertParamsDto? сonvertParams)
    {
        var response = await _heroesService.GetAsync(new GetHeroRequest
        {
            Id = id,
            HeroesConvertParams = сonvertParams != null ? HeroesConvertParamsDto.ToEntity(сonvertParams) : null
        });

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<HeroDto>.Success(HeroDto.FromEntity(response)));
    }

    [HttpGet]
    [Authorize]
    [Route("get-by-filter")]
    [ProducesResponseType(typeof(RestApiResponse<GetHeroesResponseDto>), 200)]
    public async Task<ActionResult<RestApiResponse<GetHeroesResponseDto>>> Get([FromQuery] GetHeroesRequestDto request)
    {
        var response = await _heroesService.GetByFilterAsync(GetHeroesRequestDto.ToEntity(request));

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<GetHeroesResponseDto>.Success(GetHeroesResponseDto.FromEntity(response)));
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 201)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Create([FromBody] HeroDto request)
    {
        await _heroesService.CreateOrUpdateAsync(HeroDto.ToEntity(request));

        return StatusCode(StatusCodes.Status201Created, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }

    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 200)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Update([FromRoute] int id, [FromBody] HeroDto request)
    {
        var entity = HeroDto.ToEntity(request);
        entity.Id = id;

        await _heroesService.CreateOrUpdateAsync(entity);

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }

    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 204)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Delete([FromRoute] int id)
    {
        await _heroesService.DeleteAsync(new DeleteHeroRequest { Id = id });

        return StatusCode(StatusCodes.Status204NoContent, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }
}
