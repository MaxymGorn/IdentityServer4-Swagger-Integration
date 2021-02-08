using System;
using System.Collections.Generic;
using System.Linq;
using Api.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Swashbuckle.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IWeatherForecastService weatherForecast;
        public WeatherForecastController(IWeatherForecastService weatherForecast)
        {
            this.weatherForecast = weatherForecast;
        }

        
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return weatherForecast.GetIndexBinder();
        }
    }
}
