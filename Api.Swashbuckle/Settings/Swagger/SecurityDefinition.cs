using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Swashbuckle.Settings.Swagger
{
    public class SecurityDefinition
    {
        public string name { get; set; }
        public OpenApiSecurityScheme OpenApiSecurityScheme { get; set; }
    }
}
