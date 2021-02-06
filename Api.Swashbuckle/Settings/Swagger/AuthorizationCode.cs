using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Swashbuckle.Settings.Swagger
{
    public class AuthorizationCode
    {
        public string AuthorizationUrl { get; set; }
        public string TokenUrl { get; set; }
        public IDictionary<string, string> Scopes { get; set; }
    }
}
