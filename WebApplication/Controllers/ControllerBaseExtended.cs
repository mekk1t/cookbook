using KitProjects.MasterChef.WebApplication.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace KitProjects.MasterChef.WebApplication.Controllers
{
    [Route("api")]
    [ApiController]
    [Produces("application/json")]
    public class ControllerBaseExtended : ControllerBase
    {
        protected IActionResult ApiError(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest) =>
            StatusCode((int)statusCode, new ApiErrorResponse(new[] { message }));

        protected IActionResult ApiError(string[] messages, HttpStatusCode statusCode = HttpStatusCode.BadRequest) =>
            StatusCode((int)statusCode, new ApiErrorResponse(messages));

        protected IActionResult ProcessRequest(Action action)
        {
            try
            {
                action();
                return Ok();
            }
            catch (Exception ex)
            {
                return ApiError(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        protected IActionResult ProcessRequest<TResult>(Func<TResult> function)
        {
            try
            {
                var result = function();
                if (result == null)
                    return ApiError("Не удалось получить данные по запросу.", HttpStatusCode.NotFound);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return ApiError(ex.Message, HttpStatusCode.InternalServerError);
            }
        }
    }
}