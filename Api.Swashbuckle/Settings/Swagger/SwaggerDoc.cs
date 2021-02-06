using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Swashbuckle.Settings.Swagger
{
    public class SwaggerDoc
    {
        public string name { get; set; }
        public Microsoft.OpenApi.Models.OpenApiInfo OpenApiInfo { get; set; }
    }
}
