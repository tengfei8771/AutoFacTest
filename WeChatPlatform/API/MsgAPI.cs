using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using WeChatPlatform.Config;

namespace WeChatPlatform.API
{
    public class MsgAPI
    {
        /// <summary>
        /// 获取微信服务器IP地址
        /// </summary>
        /// <returns>返回数据</returns>
        public string GetIP()
        {
            string url = Until.CreateUrl(UrlConfig.IPURL);
            RequestHelper request = new RequestHelper();
            string response = request.GetResponseString(request.CreateGetHttpResponse(url));
            JObject obj = JObject.Parse(response);
            if (response.Contains("errcode"))
            {
                throw new Exception(errMsg.GetErrMsg((int)obj["errcode"]));
            }
            
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
            string url = Until.CreateUrl(UrlConfig.SetIndustry);
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
            string url = Until.CreateUrl(UrlConfig.GetIndustry);
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
            string url = Until.CreateUrl(UrlConfig.GetTemplateID);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["template_id_short"] = id;
            RequestHelper request = new RequestHelper();
            string response = request.GetResponseString(request.CreatePostHttpResponse(url, dic));
            JObject obj = JObject.Parse(response);
            if ((int)obj["errcode"] != 0)
            {
                throw new Exception(errMsg.GetErrMsg((int)obj["errcode"]));
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
            string url = Until.CreateUrl(UrlConfig.GetTemplateList);
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
            string url = Until.CreateUrl(UrlConfig.DelTemplate);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["template_id"] = id;
            RequestHelper request = new RequestHelper();
            string response = request.GetResponseString(request.CreatePostHttpResponse(url, dic));
            JObject obj = JObject.Parse(response);
            if ((int)obj["errcode"] != 0)
            {
                throw new Exception(errMsg.GetErrMsg((int)obj["errcode"]));
            }
            else
            {
                return response;
            }
        }
        /// <summary>
        /// 推送模板消息
        /// </summary>
        /// <param name="openid">推送目标人openid</param>
        /// <param name="templateid">推送的模板id</param>
        /// <param name="data">推送的data数据，必须为json格式</param>
        /// <param name="appid">点击跳转的appid，绑定在平台上</param>
        /// <param name="appurl">模板跳转url</param>
        /// <param name="miniprogram">跳小程序所需数据</param>
        /// <param name="pagepath">所需跳转到小程序的具体页面路径</param>
        /// <param name="color">模板内容字体颜色，不填默认为黑色</param>
        /// <returns></returns>
        public string SendTemplateMsg(string openid, string templateid, object data,string appid,string appurl = null, object miniprogram= null,string pagepath=null,string color = null)
        {
            string url= Until.CreateUrl(UrlConfig.SendTemplateMsg);
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
            dic["appid"] = appid;
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
                throw new Exception(errMsg.GetErrMsg((int)obj["errcode"]));
            }
            else
            {
                return response;
            }
        }
        #endregion


    }
}
