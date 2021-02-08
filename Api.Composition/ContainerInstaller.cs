using Autofac;
using System;

namespace Api.Composition
{
    public class ContainerInstaller
    {
        public ContainerInstaller()
        {

        }
		public ContainerBuilder Install()
		{
            ContainerBuilder builder = new ContainerBuilder();
            new WeatherInstaller().Install(builder);
            return builder;
		}
	}
}
