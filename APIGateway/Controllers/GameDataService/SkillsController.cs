using APIGateway.Infrastructure.Core;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using GameData.Skills.Gen;
using APIGateway.Infrastructure.GameDataService.Models.Skills;
using APIGateway.Infrastructure.GameDataService.Models.Skills.Params;
using APIGateway.Infrastructure.GameDataService.Responses.Skills;
using APIGateway.Infrastructure.GameDataService.Requests.Skills;

namespace APIGateway.Controllers.GameDataService;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/skills")]
[Produces("application/json")]
[ApiController]
public class SkillsController : ControllerBase
{
    private readonly ILogger<SkillsController> _logger;
    private readonly SkillsService.SkillsServiceClient _skillsService;

    public SkillsController(
        ILogger<SkillsController> logger,
        SkillsService.SkillsServiceClient skillsService)
    {
        _logger = logger;
        _skillsService = skillsService;
    }

    [HttpGet]
    [Route("{id:int}")]
    [Authorize]
    [ProducesResponseType(typeof(RestApiResponse<SkillDto>), 200)]
    public async Task<ActionResult<RestApiResponse<SkillDto>>> Get([FromRoute] int id, [FromQuery] SkillsConvertParamsDto? сonvertParams)
    {
        var response = await _skillsService.GetAsync(new GetSkillRequest
        {
            Id = id,
            SkillsConvertParams = сonvertParams != null ? SkillsConvertParamsDto.ToEntity(сonvertParams) : null
        });

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<SkillDto>.Success(SkillDto.FromEntity(response)));
    }

    [HttpGet]
    [Authorize]
    [Route("get-by-filter")]
    [ProducesResponseType(typeof(RestApiResponse<GetSkillsResponseDto>), 200)]
    public async Task<ActionResult<RestApiResponse<GetSkillsResponseDto>>> Get([FromQuery] GetSkillsRequestDto request)
    {
        var response = await _skillsService.GetByFilterAsync(GetSkillsRequestDto.ToEntity(request));

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<GetSkillsResponseDto>.Success(GetSkillsResponseDto.FromEntity(response)));
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 201)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Create([FromBody] SkillDto request)
    {
        await _skillsService.CreateOrUpdateAsync(SkillDto.ToEntity(request));

        return StatusCode(StatusCodes.Status201Created, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }

    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 200)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Update([FromRoute] int id, [FromBody] SkillDto request)
    {
        var entity = SkillDto.ToEntity(request);
        entity.Id = id;

        await _skillsService.CreateOrUpdateAsync(entity);

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }

    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 204)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Delete([FromRoute] int id)
    {
        await _skillsService.DeleteAsync(new DeleteSkillRequest { Id = id });

        return StatusCode(StatusCodes.Status204NoContent, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }
}
