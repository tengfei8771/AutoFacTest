using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test1.Log4NetHelper;

namespace test1.Filter
{
    public class GlobalExceptionsFilter:IExceptionFilter
    {
        private readonly IHostingEnvironment _env;
        private readonly ILogHelper _log;
        public GlobalExceptionsFilter(IHostingEnvironment env,ILogHelper log)
        {
            _env = env;
            _log = log;
        }

        public void OnException(ExceptionContext context)
        {
            var json = new JsonErrorResponse();
            json.Message = context.Exception.Message;
            if (_env.IsDevelopment())
            {
                json.DevelopmentMessage = context.Exception.StackTrace;
            }
            context.Result = new InternalServerErrorObjectResult(json);
            _log.Error(json.Message, WriteLog(json.Message, context.Exception));
        }
        public class JsonErrorResponse
        {
            /// <summary>
            /// 生产环境的消息
            /// </summary>
            public string Message { get; set; }
            /// <summary>
            /// 开发环境的消息
            /// </summary>
            public string DevelopmentMessage { get; set; }
        }
        public string WriteLog(string throwMsg, Exception ex)
        {
            return string.Format("【自定义错误】：{0} \r\n【异常类型】：{1} \r\n【异常信息】：{2} \r\n【堆栈调用】：{3}", new object[] { throwMsg,
                ex.GetType().Name, ex.Message, ex.StackTrace });
        }
        public class InternalServerErrorObjectResult : ObjectResult
        {
            public InternalServerErrorObjectResult(object value) : base(value)
            {
                StatusCode = StatusCodes.Status500InternalServerError;
            }
        }
    }
}
