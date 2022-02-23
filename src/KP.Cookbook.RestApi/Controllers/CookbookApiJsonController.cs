using KP.Api.AspNetCore;
using KP.Cookbook.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace KP.Cookbook.RestApi.Controllers
{
    public abstract class CookbookApiJsonController : ApiJsonController
    {
        private readonly ILogger<ApiJsonController> _logger;

        protected CookbookApiJsonController(ILogger<ApiJsonController> logger, bool catchAllExceptions = false)
            : base(logger, catchAllExceptions)
        {
            _logger = logger;
        }

        protected override IActionResult Wrap(Func<IActionResult> getActionResult)
        {
            try
            {
                return getActionResult();
            }
            catch (InvariantException ex)
            {
                _logger.LogError(ex, ex.ToString());
                return ApiError($"Ошибка предметной области: {ex.Message}", HttpStatusCode.Conflict);
            }
            catch (CookbookException ex)
            {
                _logger.LogError(ex, ex.ToString());
                return ApiError(ex.Message, MapErrorCode(ex.ErrorCode));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.ToString());
                return ApiError("Произошла ошибка на стороне сервера", HttpStatusCode.InternalServerError);
            }
        }

        private static HttpStatusCode MapErrorCode(CookbookErrorCode errorCode)
        {
            return errorCode switch
            {
                CookbookErrorCode.AccessDenied => HttpStatusCode.Forbidden,
                _ => throw new NotSupportedException($"{errorCode} не поддерживается"),
            };
        }
    }
}
