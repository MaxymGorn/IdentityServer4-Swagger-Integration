using System;
using System.Collections.Generic;
using System.Linq;
using Api.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Swashbuckle.Controllers
{

    [Authorize]
    [Route("[controller]")]
    [ApiController]
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
        [Produces("application/json", "application/xml", "text/json", "text/xml")]
        public IEnumerable<WeatherForecast> Get()
        {
            logger.LogInformation("Test logging");
            return weatherForecast.GetIndexBinder();
        }
    }
}
