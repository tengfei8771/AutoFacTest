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
        /// 获取关注本公众号内前10000个人的openid,超过10000万人递归拼接成一条完整的消息
        /// </summary>
        /// <param name="openid">从哪个人开始查询，可不填</param>
        /// <param name="GetAll">是否获取全部关注的用户信息，默认false，填入true则递归拉取全部数据</param>
        /// <returns></returns>
        public string GetUserList(string openid=null,bool GetAll=false)
        {
            string url = Until.CreateUrl(UrlConfig.GetUserList)+ "&next_openid="+openid;
            RequestHelper request = new RequestHelper();
            string response = request.GetResponseString(request.CreateGetHttpResponse(url));
            JObject obj = JObject.Parse(response);
            if (response.Contains("errcode"))
            {
                throw new Exception(errMsg.GetErrMsg((int)obj["errcode"]));
            }
            if (obj["next_openid"] != null && obj["next_openid"].ToString() != ""&&GetAll)
            {
                JArray UserList = JArray.Parse(obj["openid"].ToString());
                GetUserChildrenList(obj["next_openid"].ToString(), UserList);
                return obj.ToString();
            }
            else
            {
                return obj.ToString();
            }
        }

        public void GetUserChildrenList(string openid, JArray UserList)
        {
            string url = Until.CreateUrl(UrlConfig.GetUserList) + "&next_openid=" + openid;
            RequestHelper request = new RequestHelper();
            string response = request.GetResponseString(request.CreateGetHttpResponse(url));
            JObject obj = JObject.Parse(response);
            if (response.Contains("errcode"))
            {
                throw new Exception(errMsg.GetErrMsg((int)obj["errcode"]));
            }
            if (obj["next_openid"] != null && obj["next_openid"].ToString() != "")
            {
                JArray UserChildrenList = JArray.Parse(obj["openid"].ToString());
                foreach(var item in UserChildrenList)
                {
                    UserList.Add(item);
                }
                GetUserChildrenList(obj["next_openid"].ToString(), UserList);
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
                throw new Exception(errMsg.GetErrMsg((int)obj["errcode"]));
            }
            else
            {
                return response;
            }
        }

        public string GetUserTag()
        {
            string url = Until.CreateUrl(UrlConfig.GetUserTag);
            RequestHelper request = new RequestHelper();
            string response = request.GetResponseString(request.CreateGetHttpResponse(url));
            JObject obj = JObject.Parse(response);
            if (response.Contains("errcode"))
            {
                throw new Exception(errMsg.GetErrMsg((int)obj["errcode"]));
            }
            else
            {
                return response;
            }
        }

    }
}
