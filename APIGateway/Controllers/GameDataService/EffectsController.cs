using APIGateway.Infrastructure.Core;
using APIGateway.Infrastructure.GameDataService.Models.Effects;
using APIGateway.Infrastructure.GameDataService.Models.Effects.Params;
using APIGateway.Infrastructure.GameDataService.Requests.Effects;
using APIGateway.Infrastructure.GameDataService.Responses.Effects;
using Asp.Versioning;
using Common.Enums;
using GameData.Effects.Gen;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIGateway.Controllers.GameDataService;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/effects")]
[Produces("application/json")]
[ApiController]
public class EffectsController : ControllerBase
{
    private readonly ILogger<EffectsController> _logger;
    private readonly EffectsService.EffectsServiceClient _effectsService;

    public EffectsController(
        ILogger<EffectsController> logger,
        EffectsService.EffectsServiceClient effectsService)
    {
        _logger = logger;
        _effectsService = effectsService;
    }

    [HttpGet]
    [Route("{id:int}")]
    [Authorize]
    [ProducesResponseType(typeof(RestApiResponse<EffectDto>), 200)]
    public async Task<ActionResult<RestApiResponse<EffectDto>>> Get([FromRoute] int id, [FromQuery] EffectsConvertParamsDto? сonvertParams)
    {
        var response = await _effectsService.GetAsync(new GetEffectRequest
        {
            Id = id,
            EffectsConvertParams = сonvertParams != null ? EffectsConvertParamsDto.ToEntity(сonvertParams) : null
        });

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<EffectDto>.Success(EffectDto.FromEntity(response)));
    }

    [HttpGet]
    [Authorize]
    [Route("get-by-filter")]
    [ProducesResponseType(typeof(RestApiResponse<GetEffectsResponseDto>), 200)]
    public async Task<ActionResult<RestApiResponse<GetEffectsResponseDto>>> Get([FromQuery] GetEffectsRequestDto request)
    {
        var response = await _effectsService.GetByFilterAsync(GetEffectsRequestDto.ToEntity(request));

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<GetEffectsResponseDto>.Success(GetEffectsResponseDto.FromEntity(response)));
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 201)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Create([FromBody] EffectDto request)
    {
        await _effectsService.CreateOrUpdateAsync(EffectDto.ToEntity(request));

        return StatusCode(StatusCodes.Status201Created, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }

    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 200)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Update([FromRoute] int id, [FromBody] EffectDto request)
    {
        var entity = EffectDto.ToEntity(request);
        entity.Id = id;

        await _effectsService.CreateOrUpdateAsync(entity);

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }

    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 204)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Delete([FromRoute] int id)
    {
        await _effectsService.DeleteAsync(new DeleteEffectRequest { Id = id });

        return StatusCode(StatusCodes.Status204NoContent, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }
}
