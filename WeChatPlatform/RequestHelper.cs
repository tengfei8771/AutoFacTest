using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace WeChatPlatform
{
    public class RequestHelper
    {
        /// <summary>
        /// 后台调用post请求方法
        /// </summary>
        /// <param name="url">调用的网址</param>
        /// <param name="body">请求的post参数</param>
        /// <returns>目标网站的返回信息</returns>
        public HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string,object> body,int timeout=10)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "post";
            request.ContentType= "application / x - www - form - urlencoded";
            request.Timeout = timeout * 1000;
            ServicePointManager.DefaultConnectionLimit = 200;
            if (body.Count != 0 && body != null)
            {
                string json = JsonConvert.SerializeObject(body);
                //StringBuilder sb = new StringBuilder();
                //foreach (string key in body.Keys)
                //{
                //    sb.AppendFormat("{0}={1}&", key, body[key]);
                //}
                //string str = sb.ToString();
                //str = str.TrimEnd('&');
                byte[] data = Encoding.UTF8.GetBytes(json);
                using(Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            return request.GetResponse() as HttpWebResponse;
        }
        /// <summary>
        /// 后台get方法
        /// </summary>
        /// <param name="url">网址</param>
        /// <returns>网站返回信息</returns>
        public HttpWebResponse CreateGetHttpResponse(string url,int timeout=10)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "post";
            request.ContentType = "application / x - www - form - urlencoded";
            request.Timeout = timeout * 1000;
            ServicePointManager.DefaultConnectionLimit = 200;
            return request.GetResponse() as HttpWebResponse;
        }
        /// <summary>
        /// 解析网站返回信息
        /// </summary>
        /// <param name="response">网站返回信息</param>
        /// <returns>解析出的字符串</returns>
        public string GetResponseString(HttpWebResponse response)
        {
            using(Stream stream = response.GetResponseStream())
            {
                StreamReader Reader = new StreamReader(stream, Encoding.UTF8);
                return Reader.ReadToEnd();
            }
        }
    }
}
