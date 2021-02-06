using System;
using System.Collections.Generic;
using System.Linq;
using Api.Swashbuckle.OperationFilter;
using Api.Swashbuckle.Settings;
using Api.Swashbuckle.Settings.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
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

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
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
            SwaggerGenSettings swaggerGenSettings = new SwaggerGenSettings();
            Configuration.GetSection("SwaggerGenSettings").Bind(swaggerGenSettings);
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(swaggerGenSettings.SwaggerDoc.name, swaggerGenSettings.SwaggerDoc.OpenApiInfo);

                //options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                //{
                //    Type = SecuritySchemeType.OAuth2,
                //    Flows = new OpenApiOAuthFlows
                //    {
                //        AuthorizationCode = new OpenApiOAuthFlow
                //        {
                //            AuthorizationUrl = new Uri("https://localhost:6001/connect/authorize"),
                //            TokenUrl = new Uri("https://localhost:6001/connect/token"),
                //            Scopes = new Dictionary<string, string>
                //            {
                //                {"api1", "Demo API - full access"}
                //            }
                //        }
                //    }
                //});

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
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();

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

            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
        }
    }
}
