using Microsoft.AspNetCore.Mvc;

namespace dotnet_service_b.Controllers;

[ApiController]
[Route("[controller]")]
public class Controller : ControllerBase
{
    private readonly ILogger<Controller> _logger;

    public Controller(ILogger<Controller> logger)
    {
        _logger = logger;
    }

    [HttpGet("message/{text}")]
    public string Get(string text)
    {
        _logger.LogInformation("received message: " + text);
        return "service-b received message: " + text;
    }
}
