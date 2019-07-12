using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Services;
using Services.IServices;
using test1.Module;
using WeChatPay;
using WeChatPlatform.API;
using WeChatPlatform.Config;

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
            #region 消息推送接口测试方法
            MsgAPI api = new MsgAPI();
            string data = @"{
                             'User': {

                                    'value': 'Bill',
                              'color': '#173177'

                                },
                             'Prod': {
                              'value': 'A-202',
                              'color': '#173177'
                             },
                             'Date': {
                              'value': '5',
                              'color': '#173177'
                             },
                             'Money': {
                              'value': '500000',
                              'color': 'red'
                             }
                            }";
            JObject obj = JObject.Parse(data);
            string str4 = api.SendTemplateMsg("oaleEuK_SDBBqhpcE6nrehfSaGWg", "s11wtORaOEKO_BLL4Kd7OrAynTzO5KNCvQPvOAwvJu0", obj, UserConfig.appid);
            #endregion
            #region 用户信息获取测试方法
            //UserInfoAPI userInfo = new UserInfoAPI();
            //var str4 = userInfo.GetUserList();
            #endregion
            //string str = Token.GetToken();
            return Ok(str4);
            //return Ok(_services.dosomething());
        }
    }
}