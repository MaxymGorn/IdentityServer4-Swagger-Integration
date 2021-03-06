﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Swashbuckle.Settings.Swagger
{
    public class SwaggerGenSettings
    {
        public string name { get; set; }
        public SwaggerDoc SwaggerDoc { get; set; }
        public SecurityDefinition SecurityDefinition { get; set; }
    }
}
