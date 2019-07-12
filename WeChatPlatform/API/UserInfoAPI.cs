using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using WeChatPlatform.Config;

namespace WeChatPlatform.API
{
    public class UserInfoAPI
    {
        /// <summary>
        /// 获取关注本公众号内前10000个人的openid
        /// </summary>
        /// <param name="openid">从哪个人开始查询，可不填</param>
        /// <returns></returns>
        public string GetUserList(string openid=null)
        {
            string url = Until.CreateUrl(UrlConfig.GetUserList)+ "&next_openid="+openid;
            RequestHelper request = new RequestHelper();
            string response = request.GetResponseString(request.CreateGetHttpResponse(url));
            JObject obj = JObject.Parse(response);
            if (response.Contains("errcode"))
            {
                throw new Exception(obj["errcode"].ToString() + obj["rrmsg"].ToString());
            }
            else
            {
                return response;
            }

        }
        /// <summary>
        /// 添加标签
        /// </summary>
        /// <param name="TagName">标签名</param>
        /// <returns>返回内容</returns>
        public string CreateUserTag(string TagName)
        {
            string url = Until.CreateUrl(UrlConfig.CreateUserTag);
            RequestHelper request = new RequestHelper();
            string response = request.GetResponseString(request.CreateGetHttpResponse(url));
            JObject obj = JObject.Parse(response);
            if (response.Contains("errcode"))
            {
                throw new Exception(obj["errcode"].ToString() + obj["rrmsg"].ToString());
            }
            else
            {
                return response;
            }
        }
    }
}
