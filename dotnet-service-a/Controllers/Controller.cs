using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace dotnet_service_a.Controllers;

[ApiController]
[Route("[controller]")]
public class Controller : ControllerBase
{

    private IConfiguration _configuration;

    private readonly ILogger<Controller> _logger;

    public Controller(ILogger<Controller> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    [HttpGet("message/{text}")]
    public async Task<String> Get(string text)
    {
        _logger.LogInformation("received message: {}", text);

        using var httpClient = new HttpClient();
        
        string? serviceUrl = Environment.GetEnvironmentVariable("SERVICE_URL");
        string response = await httpClient.GetStringAsync(serviceUrl + "/" + text);

        return string.Format("service-a received message: {0}, {1}", text, response);
    }
}
