using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;

namespace WeChatPay
{
    public static class WxUntil
    {
        /// <summary>
        /// 发送POST请求并解析返回值
        /// </summary>
        /// <param name="url">请求网址</param>
        /// <param name="XMLString">xml格式数据</param>
        /// <returns>返回</returns>
        public static string GetPostFinallyStr(string url, string XMLString)
        {
            return GetResponseStr(CreateWxPayRequest(url, XMLString));
        }
        /// <summary>
        /// 发起微信POST请求
        /// </summary>
        /// <param name="url">发起请求的网址</param>
        /// <param name="XMLString">XML字符串</param>
        /// <returns>返回response</returns>
        public static HttpWebResponse CreateWxPayRequest(string url,string XMLString)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "post";
            request.ContentType = "application / x - www - form - urlencoded";
            if (XMLString != null && XMLString != "")
            {
                byte[] data = Encoding.UTF8.GetBytes(XMLString);
                using(Stream s = request.GetRequestStream())
                {
                    s.Write(data, 0, data.Length);
                }
                return request.GetResponse() as HttpWebResponse;

            }
            else
            {
                throw new Exception("请输入xml数据!");
            }
        }

        /// <summary>
        /// 解析返回的response
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static string GetResponseStr(HttpWebResponse response)
        {
            using (Stream s = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                return reader.ReadToEnd();
            }
        }
        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <returns>随机字符串</returns>
        public static string GetRandomStr()
        { 
            const string conStr = @"0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            string randomStr = string.Empty;
            Random rd = new Random();
            for (int i = 0; i < conStr.Length / 2; i++)
            {
                randomStr += conStr[rd.Next(0, conStr.Length)].ToString();
            }
            return randomStr;
        }
        /// <summary>
        /// 获取CPU序列号作为设备号
        /// </summary>
        /// <returns></returns>
        public static string GetCpuInfo()
        {
            try
            {
                string CpuInfo = string.Empty;
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (var mo in moc)
                {
                    CpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                }
                mc.Dispose();
                moc.Dispose();
                return CpuInfo;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static string GetIPAdress(this HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress.ToString();
            }
            return ip;
        }

        public static string GetTimeSpan(double Min=0)
        {
            //DateTime dt = DateTime.UtcNow.Add(TimeSpan.FromMinutes(Min));       
            TimeSpan ts = DateTime.Now.AddMinutes(Min).ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
    }
}
