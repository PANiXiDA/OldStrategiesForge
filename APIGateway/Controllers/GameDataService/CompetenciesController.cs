using APIGateway.Infrastructure.Core;
using Asp.Versioning;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GameData.Competencies.Gen;
using APIGateway.Infrastructure.GameDataService.Models.Competencies;
using APIGateway.Infrastructure.GameDataService.Models.Competencies.Params;
using APIGateway.Infrastructure.GameDataService.Responses.Competencies;
using APIGateway.Infrastructure.GameDataService.Requests.Competencies;

namespace APIGateway.Controllers.GameDataService;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/competencies")]
[Produces("application/json")]
[ApiController]
public class CompetenciesController : ControllerBase
{
    private readonly ILogger<CompetenciesController> _logger;
    private readonly CompetenciesService.CompetenciesServiceClient _competenciesService;

    public CompetenciesController(
        ILogger<CompetenciesController> logger,
        CompetenciesService.CompetenciesServiceClient competenciesService)
    {
        _logger = logger;
        _competenciesService = competenciesService;
    }

    [HttpGet]
    [Route("{id:int}")]
    [Authorize]
    [ProducesResponseType(typeof(RestApiResponse<CompetenceDto>), 200)]
    public async Task<ActionResult<RestApiResponse<CompetenceDto>>> Get([FromRoute] int id, [FromQuery] CompetenciesConvertParamsDto? сonvertParams)
    {
        var response = await _competenciesService.GetAsync(new GetCompetenceRequest
        {
            Id = id,
            CompetenciesConvertParams = сonvertParams != null ? CompetenciesConvertParamsDto.ToEntity(сonvertParams) : null
        });

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<CompetenceDto>.Success(CompetenceDto.FromEntity(response)));
    }

    [HttpGet]
    [Authorize]
    [Route("get-by-filter")]
    [ProducesResponseType(typeof(RestApiResponse<GetCompetenciesResponseDto>), 200)]
    public async Task<ActionResult<RestApiResponse<GetCompetenciesResponseDto>>> Get([FromQuery] GetCompetenciesRequestDto request)
    {
        var response = await _competenciesService.GetByFilterAsync(GetCompetenciesRequestDto.ToEntity(request));

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<GetCompetenciesResponseDto>.Success(GetCompetenciesResponseDto.FromEntity(response)));
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 201)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Create([FromBody] CompetenceDto request)
    {
        await _competenciesService.CreateOrUpdateAsync(CompetenceDto.ToEntity(request));

        return StatusCode(StatusCodes.Status201Created, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }

    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 200)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Update([FromRoute] int id, [FromBody] CompetenceDto request)
    {
        var entity = CompetenceDto.ToEntity(request);
        entity.Id = id;

        await _competenciesService.CreateOrUpdateAsync(entity);

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }

    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 204)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Delete([FromRoute] int id)
    {
        await _competenciesService.DeleteAsync(new DeleteCompetenceRequest { Id = id });

        return StatusCode(StatusCodes.Status204NoContent, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }
}
