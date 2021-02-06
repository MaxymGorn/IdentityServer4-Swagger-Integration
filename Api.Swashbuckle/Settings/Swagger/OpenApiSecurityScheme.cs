using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Swashbuckle.Settings.Swagger
{
    public class OpenApiSecurityScheme
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public SecuritySchemeType SecuritySchemeType { get; set; }
        public AuthorizationCode AuthorizationCode { get; set; }
    }
}
