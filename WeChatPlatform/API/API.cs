using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using WeChatPlatform.Config;

namespace WeChatPlatform.API
{
    public class API
    {
        /// <summary>
        /// 获取微信服务器IP地址
        /// </summary>
        /// <returns>返回数据</returns>
        public string GetIP()
        {
            string url = CreateUrl(UrlConfig.IPURL);
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

        #region 消息功能接口
        /// <summary>
        /// 设置行业
        /// </summary>
        /// <param name="id1">主行业</param>
        /// <param name="id2">副行业</param>
        public void SetIndustry(string id1,string id2)
        {
            string url = CreateUrl(UrlConfig.SetIndustry);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["industry_id1"] = id1;
            dic["industry_id1"] = id2;
            RequestHelper request = new RequestHelper();
            string response = request.GetResponseString(request.CreatePostHttpResponse(url,dic));

        }
        /// <summary>
        /// 获取行业信息
        /// </summary>
        /// <returns>行业信息json</returns>
        public string GetIndustry()
        {
            string url = CreateUrl(UrlConfig.GetIndustry);
            RequestHelper request = new RequestHelper();
            string response = request.GetResponseString(request.CreateGetHttpResponse(url));
            return response;
        }
        /// <summary>
        /// 获取模板ID
        /// </summary>
        /// <param name="id">shortID</param>
        /// <returns>返回信息</returns>
        public string GetTemplateID(string id)
        {
            string url = CreateUrl(UrlConfig.GetTemplateID);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["template_id_short"] = id;
            RequestHelper request = new RequestHelper();
            string response = request.GetResponseString(request.CreatePostHttpResponse(url, dic));
            JObject obj = JObject.Parse(response);
            if ((int)obj["errcode"] != 0)
            {
                throw new Exception(obj["errmsg"].ToString());
            }
            else
            {
                return response;
            }
        }
        /// <summary>
        /// 获取模板list
        /// </summary>
        /// <returns>模板列表json数据</returns>
        public string GetTemplateList()
        {
            string url = CreateUrl(UrlConfig.GetTemplateList);
            RequestHelper request = new RequestHelper();
            string response = request.GetResponseString(request.CreateGetHttpResponse(url));
            return response;
        }
        /// <summary>
        /// 删除模板
        /// </summary>
        /// <param name="id">模板id</param>
        /// <returns>删除消息</returns>
        public string DelTempalte(string id)
        {
            string url = CreateUrl(UrlConfig.DelTemplate);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["template_id"] = id;
            RequestHelper request = new RequestHelper();
            string response = request.GetResponseString(request.CreatePostHttpResponse(url, dic));
            JObject obj = JObject.Parse(response);
            if ((int)obj["errcode"] != 0)
            {
                throw new Exception(obj["errmsg"].ToString());
            }
            else
            {
                return response;
            }
        }

        public string SendTemplateMsg(string openid, string templateid, object data, string appurl = null, object miniprogram= null,string pagepath=null,string color = null)
        {
            string url= CreateUrl(UrlConfig.SendTemplateMsg);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["touser"] = openid;
            dic["template_id"] = templateid;
            if (appurl != null)
            {
                dic["url"] = appurl;
            }
            if (miniprogram != null)
            {
                dic["miniprogram"] = miniprogram;
            }
            dic["appid"] = UserConfig.appid;
            if (pagepath != null)
            {
                dic["pagepath"] = pagepath;
            }
            dic["data"] = data;
            if (color != null)
            {
                dic["color"] = color;
            }
            RequestHelper request = new RequestHelper();
            string response = request.GetResponseString(request.CreatePostHttpResponse(url, dic));
            JObject obj = JObject.Parse(response);
            if ((int)obj["errcode"] != 0)
            {
                throw new Exception(obj["errmsg"].ToString());
            }
            else
            {
                return response;
            }
        }


        #endregion

        /// <summary>
        /// 拼接URL方法
        /// </summary>
        /// <param name="partUrl">发起请求的部分URL</param>
        /// <returns>拼接完成后的URL</returns>
        public string CreateUrl(string partUrl)
        {
            if (UserConfig.token == null || UserConfig.token == "")
            {
                throw new Exception("未获取有效Token");
            }
            else
            {
                return "https://" + UrlConfig.BaseApiURL + partUrl + UserConfig.token;
            }

        }
    }
}
