using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
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
            string str = authStr.ToString().Trim();//读取到的原始token字符串
            if (!str.StartsWith("Bearer"))
            {
                throw new UnauthorizedAccessException("token格式不正确,禁止访问");
            }
            string token = str.Replace("Bearer", "");
            result = TokenHelper.Validate(token);
            if (!result)
            {
                throw new UnauthorizedAccessException("验证失败");
            }
            await _next(context);
        }
    }
}
