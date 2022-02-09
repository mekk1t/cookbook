using KP.Cookbook.Domain.Entities;
using KP.Cookbook.Features.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace KP.Cookbook.RestApi
{
    public class AccessValidator : IAccessValidator
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private const string API_KEY_HEADER = "cookbook-test";
        private const string API_KEY_ADMIN = "admin";

        public AccessValidator(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public bool IsCurrentUserOfType(UserType userType)
            => _contextAccessor.HttpContext!.Request.Headers.Contains(
                new KeyValuePair<string, StringValues>(API_KEY_HEADER, new StringValues(API_KEY_ADMIN)));
    }
}
