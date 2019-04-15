using Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Until.TokenHelper
{
    public class JwtCustomerAuthorizeMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtCustomerAuthorizeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var result = context.Request.Headers.TryGetValue("X-Token", out StringValues authStr);
            if (string.IsNullOrEmpty(result.ToString()) || !result)
            {
                throw new UnauthorizedAccessException("未读取到Token,请确认登录状态");
            }
            var token = authStr.ToString().Trim();
            result = TokenHelper.Validate(token);
            if (!result)
            {
                throw new UnauthorizedAccessException("验证失败");
            }
            await _next(context);
        }
    }
}
