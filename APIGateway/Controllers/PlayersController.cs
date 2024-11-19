using Microsoft.AspNetCore.Mvc;
using Players.Backend.Gen;
using Asp.Versioning;

namespace APIGateway.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/players")]
[Produces("application/json")]
[ApiController]
public class PlayersController : ControllerBase
{
    private readonly ILogger<PlayersController> _logger;
    private readonly PlayersBackend.PlayersBackendClient _playersBackendClient;

    public PlayersController(ILogger<PlayersController> logger, PlayersBackend.PlayersBackendClient playersBackendClient)
    {
        _logger = logger;
        _playersBackendClient = playersBackendClient;
    }
}
