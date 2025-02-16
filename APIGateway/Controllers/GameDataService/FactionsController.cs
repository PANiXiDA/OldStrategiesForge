//using Asp.Versioning;
//using Microsoft.AspNetCore.Mvc;
//using GameData.Factions.Gen;
//using APIGateway.Infrastructure.Extensions;
//using APIGateway.Infrastructure.Requests.Avatars;
//using Common.Enums;
//using Microsoft.AspNetCore.Authorization;

//namespace APIGateway.Controllers.GameDataService;

//[ApiVersion("1.0")]
//[Route("api/v{version:apiVersion}/factions")]
//[Produces("application/json")]
//[ApiController]
//public class FactionsController : ControllerBase
//{
//    private readonly ILogger<FactionsController> _logger;
//    private readonly FactionsService.FactionsServiceClient _factionsService;

//    public FactionsController(
//        ILogger<FactionsController> logger,
//        FactionsService.FactionsServiceClient factionsService)
//    {
//        _logger = logger;
//        _factionsService = factionsService;
//    }

//    [HttpPost]
//    [Authorize(Roles = $"{nameof(PlayerRole.Developer)}")]
//    [ProducesResponseType(typeof(RestApiResponse<NoContent>), 201)]
//    public async Task<ActionResult<RestApiResponse<NoContent>>> Create([FromBody] CreateAvatarRequestDto request)
//    {
//        var grpcResponse = await _avatarsClient.CreateAsync(request.CreateAvatarRequestDtoToProto());

//        return StatusCode(StatusCodes.Status201Created, RestApiResponseBuilder<NoContent>.Success(new NoContent()));
//    }
//}
