using KitProjects.Api.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KP.Cookbook.RestApi.Controllers.Auth
{
    public class AuthController : ApiJsonController
    {
        public AuthController(ILogger<AuthController> logger) : base(logger)
        {

        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Authorize() { }
    }
}
