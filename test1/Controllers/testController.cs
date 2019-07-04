using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.IServices;
using WeChatPay;

namespace test1.Controllers
{
    [AllowAnonymous]
    [Route("test")]
    public class testController : Controller
    {
        private IDoServices _services;
        public testController(IDoServices services)
        {
            _services = services;
        }
        [HttpGet("Index")]
        [Obsolete]
        public IActionResult Index()
        {
            string str=WxUntil.GetTimeSpan();
            return Ok(_services.dosomething());
        }
    }
}