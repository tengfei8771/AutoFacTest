using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using WeChatPlatform.Config;

namespace WeChatPlatform.API
{
    public class Token
    {
        /// <summary>
        /// 获取Token方法
        /// </summary>
        /// <returns>返回值</returns>
        public string GetToken()
        {
            string url = "https://" + UrlConfig.BaseApiURL + UrlConfig.TokenURL + "grant_type=client_credential&appid=" + UserConfig.appid + "&secret=" + UserConfig.secret;
            RequestHelper request = new RequestHelper();
            string response = request.GetResponseString(request.CreateGetHttpResponse(url));
            JObject obj = JObject.Parse(response);
            if (response.Contains("errcode"))
            {
                string errmsg = obj["errmsg"].ToString();
                throw new Exception(errmsg);
            }
            UserConfig.token = obj["access_token"].ToString();
            return response;
        }


    }
}
