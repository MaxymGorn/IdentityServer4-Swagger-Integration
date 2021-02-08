namespace Api.Swashbuckle.OperationFilter
{
    using global::Swashbuckle.AspNetCore.SwaggerGen;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.OpenApi.Models;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="AuthorizeCheckOperationFilter" />.
    /// </summary>
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        /// <summary>
        /// The Apply.
        /// </summary>
        /// <param name="operation">The operation<see cref="OpenApiOperation"/>.</param>
        /// <param name="context">The context<see cref="OperationFilterContext"/>.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            bool hasAuthorize = context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() ||
                               context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

            if (hasAuthorize)
            {
                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "oauth2"}
                        }] = new[] {"api1"}
                    }
                };
            }
        }
    }
}
