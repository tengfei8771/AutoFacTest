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
        public IActionResult Index()
        {
            //WxPayData wx = new WxPayData();
            //wx.SetValue("name", "小王");
            //wx.SetValue("age", "15");
            //string str=wx.GetRandomStr();
            //wx.SetValue("sec", str);
            //wx.SetSignValue();
            //string xml = wx.DicToXml();
            //wx.XmlToDic(xml);
            string str = WxUntil.GetCpuInfo();
            return Ok(_services.dosomething());
        }
    }
}