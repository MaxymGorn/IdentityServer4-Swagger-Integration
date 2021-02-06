﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Swashbuckle.Settings
{
    /// <summary>
    /// This class is a representation of the configuration of the API for Identity Server
    /// </summary>
    public class IdentityServerSettings
    {
        // Authority is the Identity Server URL
        public string Authority { get; set; }

        // Current API/Resource Name
        public string ApiName { get; set; }
    }
}
