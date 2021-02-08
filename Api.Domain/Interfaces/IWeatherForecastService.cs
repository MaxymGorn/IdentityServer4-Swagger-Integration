using Api.Swashbuckle;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Domain.Interfaces
{
    public interface IWeatherForecastService
    {
        public IEnumerable<WeatherForecast> GetIndexBinder();
    }
}
