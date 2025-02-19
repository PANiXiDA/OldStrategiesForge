using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using GameData.Factions.Gen;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using APIGateway.Infrastructure.Core;
using APIGateway.Infrastructure.GameDataService.Models.Factions;
using APIGateway.Infrastructure.GameDataService.Models.Factions.Params;
using APIGateway.Infrastructure.GameDataService.Responses.Factions;
using APIGateway.Infrastructure.GameDataService.Requests.Factions;

namespace APIGateway.Controllers.GameDataService;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/factions")]
[Produces("application/json")]
[ApiController]
public class FactionsController : ControllerBase
{
    private readonly ILogger<FactionsController> _logger;
    private readonly FactionsService.FactionsServiceClient _factionsService;

    public FactionsController(
        ILogger<FactionsController> logger,
        FactionsService.FactionsServiceClient factionsService)
    {
        _logger = logger;
        _factionsService = factionsService;
    }

    [HttpGet]
    [Route("{id:int}")]
    [Authorize]
    [ProducesResponseType(typeof(RestApiResponse<FactionDto>), 200)]
    public async Task<ActionResult<RestApiResponse<FactionDto>>> Get([FromRoute] int id, [FromQuery] FactionsConvertParamsDto? сonvertParams)
    {
        var response = await _factionsService.GetAsync(new GetFactionRequest
        {
            Id = id,
            FactionsConvertParams = сonvertParams != null ? FactionsConvertParamsDto.ToEntity(сonvertParams) : null
        });

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<FactionDto>.Success(FactionDto.FromEntity(response)));
    }

    [HttpGet]
    [Authorize]
    [Route("get-by-filter")]
    [ProducesResponseType(typeof(RestApiResponse<GetFactionsResponseDto>), 200)]
    public async Task<ActionResult<RestApiResponse<GetFactionsResponseDto>>> Get([FromQuery] GetFactionsRequestDto request)
    {
        var response = await _factionsService.GetByFilterAsync(GetFactionsRequestDto.ToEntity(request));

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<GetFactionsResponseDto>.Success(GetFactionsResponseDto.FromEntity(response)));
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 201)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Create([FromBody] FactionDto request)
    {
        await _factionsService.CreateOrUpdateAsync(FactionDto.ToEntity(request));

        return StatusCode(StatusCodes.Status201Created, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }

    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 200)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Update([FromRoute] int id, [FromBody] FactionDto request)
    {
        var entity = FactionDto.ToEntity(request);
        entity.Id = id;

        await _factionsService.CreateOrUpdateAsync(entity);

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }

    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 204)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Delete([FromRoute] int id)
    {
        await _factionsService.DeleteAsync(new DeleteFactionRequest { Id = id });

        return StatusCode(StatusCodes.Status204NoContent, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }
}
