using APIGateway.Infrastructure.Core;
using Asp.Versioning;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GameData.Subfactions.Gen;
using APIGateway.Infrastructure.GameDataService.Models.Subfactions;
using APIGateway.Infrastructure.GameDataService.Models.Subfactions.Params;
using APIGateway.Infrastructure.GameDataService.Responses.Subfactions;
using APIGateway.Infrastructure.GameDataService.Requests.Subfactions;

namespace APIGateway.Controllers.GameDataService;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/subfactions")]
[Produces("application/json")]
[ApiController]
public class SubfactionsController : ControllerBase
{
    private readonly ILogger<SubfactionsController> _logger;
    private readonly SubfactionsService.SubfactionsServiceClient _subfactionsService;

    public SubfactionsController(
        ILogger<SubfactionsController> logger,
        SubfactionsService.SubfactionsServiceClient subfactionsService)
    {
        _logger = logger;
        _subfactionsService = subfactionsService;
    }

    [HttpGet]
    [Route("{id:int}")]
    [Authorize]
    [ProducesResponseType(typeof(RestApiResponse<SubfactionDto>), 200)]
    public async Task<ActionResult<RestApiResponse<SubfactionDto>>> Get([FromRoute] int id, [FromQuery] SubfactionsConvertParamsDto? сonvertParams)
    {
        var response = await _subfactionsService.GetAsync(new GetSubfactionRequest
        {
            Id = id,
            SubfactionsConvertParams = сonvertParams != null ? SubfactionsConvertParamsDto.ToEntity(сonvertParams) : null
        });

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<SubfactionDto>.Success(SubfactionDto.FromEntity(response)));
    }

    [HttpGet]
    [Authorize]
    [Route("get-by-filter")]
    [ProducesResponseType(typeof(RestApiResponse<GetSubfactionsResponseDto>), 200)]
    public async Task<ActionResult<RestApiResponse<GetSubfactionsResponseDto>>> Get([FromQuery] GetSubfactionsRequestDto request)
    {
        var response = await _subfactionsService.GetByFilterAsync(GetSubfactionsRequestDto.ToEntity(request));

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<GetSubfactionsResponseDto>.Success(GetSubfactionsResponseDto.FromEntity(response)));
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 201)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Create([FromBody] SubfactionDto request)
    {
        await _subfactionsService.CreateOrUpdateAsync(SubfactionDto.ToEntity(request));

        return StatusCode(StatusCodes.Status201Created, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }

    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 200)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Update([FromRoute] int id, [FromBody] SubfactionDto request)
    {
        var entity = SubfactionDto.ToEntity(request);
        entity.Id = id;

        await _subfactionsService.CreateOrUpdateAsync(entity);

        return StatusCode(StatusCodes.Status200OK, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }

    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 204)]
    public async Task<ActionResult<RestApiResponse<NoContent>>> Delete([FromRoute] int id)
    {
        await _subfactionsService.DeleteAsync(new DeleteSubfactionRequest { Id = id });

        return StatusCode(StatusCodes.Status204NoContent, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
    }
}
