using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
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

        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            //直接确认，否则打不开    
            return true;
        }
        /// <summary>
        /// 发起post请求
        /// </summary>
        /// <param name="url">发起请求的url</param>
        /// <param name="XMLString">xml数据</param>
        /// <param name="timeout">响应过期时间，默认10秒</param>
        /// <param name="isUseCert">https是否使用证书认证</param>
        /// <returns></returns>
        public static HttpWebResponse CreateWxPayRequest(string url,string XMLString,int timeout=10,bool isUseCert=true)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "post";
            request.ContentType = "text/xml";
            request.Timeout = timeout * 1000;
            ServicePointManager.DefaultConnectionLimit = 200;
            try
            {
                if (url.StartsWith("https",StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                }
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
            if (!isUseCert)
            {
                string path = Assembly.GetEntryAssembly().Location;
                X509Certificate2 cert = new X509Certificate2();
                request.ClientCertificates.Add(cert);
            }
            if (!String.IsNullOrEmpty(XMLString))
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
        /// <summary>
        /// 获取时间字符串
        /// </summary>
        /// <param name="min">过期时间</param>
        /// <returns></returns>
        public static string GetNowStr(double min=0)
        {
            return DateTime.Now.AddMinutes(min).ToString("yyyyMMddhhmmss");
        }

        public static string CreateOrderNo(int type=0)
        {
            const string conStr = @"0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder sb = new StringBuilder();
            if (type == 0)
            {
                sb.Append("DD");
            }
            else if (type == 1)
            {
                sb.Append("TK");
            }
            else
            {
                throw new WxPayException("请输入正确的订单类型");
            }
            sb.Append(DateTime.Now.ToString("yyyyMMddhhmmss"));
            Random rd = new Random();
            for (int i = 0; i < 5; i++)
            {
                sb.Append(conStr[rd.Next(0, conStr.Length)]);
            }
            return sb.ToString();
        }
    }
}
