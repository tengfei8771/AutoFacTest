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
            //Token token = new Token();
            API api = new API();
            //string str=WxUntil.GetTimeSpan(0);
            //string str1 = WxUntil.GetTimeSpan(30);
            //string str2 = token.GetToken();
            //string str3 = api.GetIP();
            //data d = new data()
            //{
            //    User = new User()
            //    {
            //        name = "杨泽",
            //        color = "#173177"
            //    },
            //    Prod = new Prod()
            //    {
            //        name = "A202",
            //        color = "#173177"
            //    },
            //    Date = new Date()
            //    {
            //        time = "5",
            //        color = "#173177"
            //    },
            //    Money = new Money()
            //    {
            //        value = "5000000",
            //        color = "#173177"
            //    }
            //};
            string data= @"{
	'User': {

        'name': 'Bill',
		'color': '#173177'

    },
	'Prod': {
		'name': 'A-202',
		'color': '#173177'
	},
	'Date': {
		'time': '5',
		'color': '#173177'
	},
	'Money': {
		'value': '500000',
		'color': 'red'
	}
}";
            JObject obj = JObject.Parse(data);
            string str4 = api.SendTemplateMsg("oaleEuK_SDBBqhpcE6nrehfSaGWg", "YjcPXw9ZujmRuVmO5TN8auEyr4D2_ijG3dexjtnzGDY", obj);
            //string str4 = api.GetTemplateList();

            return Ok(str4);
            //return Ok(_services.dosomething());
        }
    }
}