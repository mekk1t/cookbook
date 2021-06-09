using KitProjects.MasterChef.WebApplication.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace KitProjects.MasterChef.WebApplication.Controllers
{
    [Route("api")]
    [Produces("application/json")]
    public class ControllerBaseExtended : ControllerBase
    {
        protected IActionResult ApiError(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest) =>
            StatusCode((int)statusCode, new ApiErrorResponse(new[] { message }));

        protected IActionResult ApiError(string[] messages, HttpStatusCode statusCode = HttpStatusCode.BadRequest) =>
            StatusCode((int)statusCode, new ApiErrorResponse(messages));
    }
}