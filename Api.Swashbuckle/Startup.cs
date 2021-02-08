using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Api.Composition;
using Api.Swashbuckle.OperationFilter;
using Api.Swashbuckle.Settings;
using Api.Swashbuckle.Settings.Swagger;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using HealthChecks.UI.Client;
using HealthChecks.UI.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using OpenApiInfo = Microsoft.OpenApi.Models.OpenApiInfo;
using OpenApiSecurityScheme = Microsoft.OpenApi.Models.OpenApiSecurityScheme;

namespace Api.Swashbuckle
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        private const string HealthChecksUIPolicy = nameof(HealthChecksUIPolicy);

        public IConfiguration Configuration { get; }
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            #region Health Checks
            services.AddHealthChecks();
            services.AddHealthChecksUI().AddInMemoryStorage();
            #endregion
            #region Identity Server Config
            IdentityServerSettings identityServerSettings = new IdentityServerSettings();
            Configuration.GetSection("IdentityServerSettings").Bind(identityServerSettings);
            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication("Bearer", options =>
                {
                    options.ApiName = identityServerSettings.ApiName;
                    options.Authority = identityServerSettings.Authority;
                });
            #endregion
            #region Swagger Config
            SwaggerGenSettings swaggerGenSettings = new SwaggerGenSettings();
            Configuration.GetSection("SwaggerGenSettings").Bind(swaggerGenSettings);
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(swaggerGenSettings.SwaggerDoc.name, swaggerGenSettings.SwaggerDoc.OpenApiInfo);
                SecurityDefinition securityDefinition = swaggerGenSettings.SecurityDefinition;
                Settings.Swagger.AuthorizationCode authorizationCode = securityDefinition.OpenApiSecurityScheme.AuthorizationCode;
                options.AddSecurityDefinition(securityDefinition.name, new OpenApiSecurityScheme
                {
                    Type = securityDefinition.OpenApiSecurityScheme.SecuritySchemeType,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(authorizationCode.AuthorizationUrl),
                            TokenUrl = new Uri(authorizationCode.TokenUrl),
                            Scopes = authorizationCode.Scopes
                        }
                    }
                });
                options.OperationFilter<AuthorizeCheckOperationFilter>();
            });
            #endregion
            #region Autofac Composition root 
            // Install the container, using our configuration
            ContainerInstaller installer = new ContainerInstaller();
            ContainerBuilder builder = installer.Install();

            // Pull the .net core dependencies into the container, like controllers
            builder.Populate(services);

            IContainer container = builder.Build();
            #endregion
            // return the IServiceProvider implementation
            return new AutofacServiceProvider(container);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            app.UseHealthChecksUI(delegate (Options options)
            {
                options.UIPath = "/hc-ui";
                options.AddCustomStylesheet("./Customization/custom.css");
            });
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                options.OAuthClientId("demo_api_swagger");
                options.OAuthAppName("Demo API - Swagger");
                options.OAuthUsePkce();
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
