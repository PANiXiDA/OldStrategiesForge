using APIGateway.Infrastructure.Core;
using Asp.Versioning;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GameData.HeroClasses.Gen;
using APIGateway.Infrastructure.GameDataService.Models.HeroClasses;
using APIGateway.Infrastructure.GameDataService.Models.HeroClasses.Params;
using APIGateway.Infrastructure.GameDataService.Responses.HeroClasses;
using APIGateway.Infrastructure.GameDataService.Requests.HeroClasses;

namespace APIGateway.Controllers.GameDataService;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/hero-classes")]
[Produces("application/json")]
[ApiController]
public class HeroClassesController : ControllerBase
{
    private readonly ILogger<HeroClassesController> _logger;
    private readonly HeroClassesService.HeroClassesServiceClient _heroClassesService;

    public HeroClassesController(
        ILogger<HeroClassesController> logger,
        HeroClassesService.HeroClassesServiceClient heroClassesService)
    {
        _logger = logger;
        _heroClassesService = heroClassesService;
    }

    [HttpGet]
    [Route("{id:int}")]
    [Authorize]
    [ProducesResponseType(typeof(RestApiResponse<HeroClassDto>), 200)]
    public async Task<ActionResult<RestApiResponse<HeroClassDto>>> Get([FromRoute] int id, [FromQuery] HeroClassesConvertParamsDto? сonvertParams)
    {
        var response = await _heroClassesService.GetAsync(new GetHeroClassRequest
        {
            Id = id,
            HeroClassesConvertParams = сonvertParams != null ? HeroClassesConvertParamsDto.ToEntity(сonvertParams) : null
        });

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<HeroClassDto>.Success(HeroClassDto.FromEntity(response)));
    }

    [HttpGet]
    [Authorize]
    [Route("get-by-filter")]
    [ProducesResponseType(typeof(RestApiResponse<GetHeroClassesResponseDto>), 200)]
    public async Task<ActionResult<RestApiResponse<GetHeroClassesResponseDto>>> Get([FromQuery] GetHeroClassesRequestDto request)
    {
        var response = await _heroClassesService.GetByFilterAsync(GetHeroClassesRequestDto.ToEntity(request));

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<GetHeroClassesResponseDto>.Success(GetHeroClassesResponseDto.FromEntity(response)));
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 201)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Create([FromBody] HeroClassDto request)
    {
        await _heroClassesService.CreateOrUpdateAsync(HeroClassDto.ToEntity(request));

        return StatusCode(StatusCodes.Status201Created, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }

    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 200)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Update([FromRoute] int id, [FromBody] HeroClassDto request)
    {
        var entity = HeroClassDto.ToEntity(request);
        entity.Id = id;

        await _heroClassesService.CreateOrUpdateAsync(entity);

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }

    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 204)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Delete([FromRoute] int id)
    {
        await _heroClassesService.DeleteAsync(new DeleteHeroClassRequest { Id = id });

        return StatusCode(StatusCodes.Status204NoContent, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }
}
