using Api.Domain;
using Api.Domain.Interfaces;
using Api.Domain.Services;
using Api.Swashbuckle;
using Autofac;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Api.Composition
{
    public class WeatherInstaller
    {
        public WeatherInstaller()
        {

        }
		public void Install(ContainerBuilder builder)
		{
            Assembly ServiceAssembly = typeof(Init).GetTypeInfo().Assembly;
            builder.RegisterType<WeatherService>().As<IWeatherForecastService>().
            AsSelf().InstancePerDependency();
        }
	}
}
