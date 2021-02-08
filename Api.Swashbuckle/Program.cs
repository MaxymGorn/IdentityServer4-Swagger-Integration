namespace Api.Swashbuckle
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using Serilog;

    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>().ConfigureKestrel(serverOptions =>
            {
                serverOptions.Limits.MinRequestBodyDataRate = null;
            })
            .UseSerilog((hostingContext, loggerConfiguration) => {
                loggerConfiguration
                .ReadFrom
                .Configuration(hostingContext.Configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console();
            }).Build();
    }
}
