using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KP.Cookbook.RestApi.Controllers.Auth
{
    public class AuthController : CookbookApiJsonController
    {
        public AuthController(ILogger<AuthController> logger) : base(logger)
        {

        }
    }
}
