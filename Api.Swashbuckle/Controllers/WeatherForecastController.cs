using System;
using System.Collections.Generic;
using System.Linq;
using Api.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Swashbuckle.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IWeatherForecastService weatherForecast;
        private readonly ILogger logger;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService weatherForecast)
        {
            this.logger = logger;
            this.weatherForecast = weatherForecast;
        }

        
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            logger.LogInformation("Test logging");
            return weatherForecast.GetIndexBinder();
        }
    }
}
