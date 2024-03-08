using System.Globalization;
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;

namespace Service_Bus_Topics.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpPost]
    public async Task Post(WeatherForecast data)
    {
        // Assumes we write this to a database
        var message = new WeatherForecastAdded
        {
            Id = Guid.NewGuid(),
            CreatedDateTime = DateTime.UtcNow,
            ForDate = data.Date
        };

        var connectionString = "Endpoint=sb://youtube-demo.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=A1B30KhfxMO0rxkvPzMzAwXottCczg5hM+ASbEpOYng=";
        var client = new ServiceBusClient(connectionString);
        var sender = client.CreateSender("weather-forecast-added");
        var body = JsonSerializer.Serialize(message);
        var messageBus = new ServiceBusMessage(body);
        messageBus.ApplicationProperties.Add("Month", data.Date.ToString("MMMM",CultureInfo.GetCultureInfo("en-US")));
        await sender.SendMessageAsync(messageBus);

    }
}
