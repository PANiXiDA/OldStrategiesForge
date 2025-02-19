using APIGateway.Infrastructure.Core;
using Asp.Versioning;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GameData.Units.Gen;
using APIGateway.Infrastructure.GameDataService.Models.Units;
using APIGateway.Infrastructure.GameDataService.Models.Units.Params;
using APIGateway.Infrastructure.GameDataService.Requests.Units;
using APIGateway.Infrastructure.GameDataService.Responses.Units;

namespace APIGateway.Controllers.GameDataService;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/units")]
[Produces("application/json")]
[ApiController]
public class UnitsController : ControllerBase
{
    private readonly ILogger<UnitsController> _logger;
    private readonly UnitsService.UnitsServiceClient _unitsService;

    public UnitsController(
        ILogger<UnitsController> logger,
        UnitsService.UnitsServiceClient unitsService)
    {
        _logger = logger;
        _unitsService = unitsService;
    }

    [HttpGet]
    [Route("{id:int}")]
    [Authorize]
    [ProducesResponseType(typeof(RestApiResponse<UnitDto>), 200)]
    public async Task<ActionResult<RestApiResponse<UnitDto>>> Get([FromRoute] int id, [FromQuery] UnitsConvertParamsDto? сonvertParams)
    {
        var response = await _unitsService.GetAsync(new GetUnitRequest
        {
            Id = id,
            UnitsConvertParams = сonvertParams != null ? UnitsConvertParamsDto.ToEntity(сonvertParams) : null
        });

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<UnitDto>.Success(UnitDto.FromEntity(response)));
    }

    [HttpGet]
    [Authorize]
    [Route("get-by-filter")]
    [ProducesResponseType(typeof(RestApiResponse<GetUnitsResponseDto>), 200)]
    public async Task<ActionResult<RestApiResponse<GetUnitsResponseDto>>> Get([FromQuery] GetUnitsRequestDto request)
    {
        var response = await _unitsService.GetByFilterAsync(GetUnitsRequestDto.ToEntity(request));

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<GetUnitsResponseDto>.Success(GetUnitsResponseDto.FromEntity(response)));
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 201)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Create([FromBody] UnitDto request)
    {
        await _unitsService.CreateOrUpdateAsync(UnitDto.ToEntity(request));

        return StatusCode(StatusCodes.Status201Created, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }

    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 200)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Update([FromRoute] int id, [FromBody] UnitDto request)
    {
        var entity = UnitDto.ToEntity(request);
        entity.Id = id;

        await _unitsService.CreateOrUpdateAsync(entity);

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }

    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 204)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Delete([FromRoute] int id)
    {
        await _unitsService.DeleteAsync(new DeleteUnitRequest { Id = id });

        return StatusCode(StatusCodes.Status204NoContent, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }
}
