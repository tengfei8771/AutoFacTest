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
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace WeChatPay
{
    public static class WxUntil
    {
        /// <summary>
        /// 发起post请求并将回应解析为string
        /// </summary>
        /// <param name="url">发起请求的url</param>
        /// <param name="XMLString">解析后的xml数据</param>
        /// <param name="isUseCert">是否使用证书</param>
        /// <param name="timeout">超时时间</param>
        /// <returns></returns>
        public static string GetPostFinallyStr(string url, string XMLString,bool isUseCert=false,int timeout = 10)
        {
            return GetResponseStr(CreateWxPayRequest(url, XMLString,isUseCert,timeout));
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
        /// <returns>HttpWebResponse的返回值</returns>
        public static HttpWebResponse CreateWxPayRequest(string url,string XMLString,bool isUseCert=false,int timeout=10)
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
        /// <returns>返回的字符串</returns>
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
        /// <returns>cpu字符串</returns>
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
        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <param name="context">上下文</param>
        /// <returns>ip字符串</returns>
        public static string GetIPAdress(this HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress.ToString();
            }
            return ip;
        }
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="Min">时间偏移量，单位分钟</param>
        /// <returns>返回时间戳字符串</returns>
        public static string GetTimeSpan(double Min=0)
        {
            //DateTime dt = DateTime.UtcNow.Add(TimeSpan.FromMinutes(Min));       
            TimeSpan ts = DateTime.Now.AddMinutes(Min).ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
        /// <summary>
        /// 获取时间字符串
        /// </summary>
        /// <param name="min">过期时间，单位分钟</param>
        /// <returns>返回时间字符串</returns>
        public static string GetNowStr(double min=0)
        {
            return DateTime.Now.AddMinutes(min).ToString("yyyyMMddhhmmss");
        }

        /// <summary>
        /// 生成商户订单编号，前两个字符代表是订单还是退款，3-14位为当前时间字符串，最后5位为随机字符串
        /// </summary>
        /// <param name="type">生成的订单编号类型，默认为0，生成订单号，1生成提款订单号，填入其他参数抛出异常</param>
        /// <returns>订单编号</returns>
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
        /// <summary>
        /// 分析退款结果方法
        /// </summary>
        /// <param name="XMLStr">接收到的xml数据</param>
        /// <returns>将明文xml和秘文xml组装成一个SortedDictionary</returns>
        public static SortedDictionary<string, object> AnalysisNotification(string XMLStr)
        {
            SortedDictionary<string, object> s = new SortedDictionary<string, object>();
            XMLToDic(XMLStr, s);
            if (s["return_code"].ToString().ToUpper()!= "SUCCESS")
            {
                throw new WxPayException(s["return_msg"].ToString());
            }
            else
            {
                string req_info = DecodeBase64(s["req_info"].ToString());
                string key = MD5Sign(WxPayConfig.key);
                string info = DecryptAES256ECB(req_info, key);
                XMLToDic(info, s);
                return s;
            }
        }

        public static void XMLToDic(string XMLStr,SortedDictionary<string,object> s)
        {
            XmlDocument xml = new XmlDocument();
            try
            {
                xml.Load(XMLStr);
                XmlNode FirstNode = xml.FirstChild;
                XmlNodeList NodeList = FirstNode.ChildNodes;
                foreach (XmlNode node in NodeList)
                {
                    s.Add(node.Name, node.InnerText);
                }
            }
            catch(Exception e)
            {
                throw new WxPayException(e.Message);
            }
           
        }
        /// <summary>
        /// base64加密方法
        /// </summary>
        /// <param name="msg">加密前的数据</param>
        /// <returns>加密后的数据</returns>
        public static string EncodeBase64(string msg)
        {
            byte[] bt = Encoding.UTF8.GetBytes(msg);
            return Convert.ToBase64String(bt);
        }
        /// <summary>
        /// base64解密方法
        /// </summary>
        /// <param name="msg">解密前的数据</param>
        /// <returns>解密后的数据</returns>
        public static string DecodeBase64(string msg)
        {
            byte[] bt = Convert.FromBase64String(msg);
            return Encoding.UTF8.GetString(bt);
        }
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="CompositeString">加密前的数据</param>
        /// <returns>加密后的数据</returns>
        public static string MD5Sign(string CompositeString)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bt = Encoding.UTF8.GetBytes(CompositeString);
            byte[] bt1 = md5.ComputeHash(bt);
            string byte2String = String.Empty;
            foreach (byte b in bt1)
            {
                byte2String += b.ToString("X2");
            }
            return byte2String;
        }
        /// <summary>
        /// sha256加密方法
        /// </summary>
        /// <param name="CompositeString">加密前的数据</param>
        /// <returns>加密后的数据</returns>
        public static string SHA256(string CompositeString)
        {
            byte[] msg = Encoding.UTF8.GetBytes(CompositeString);
            byte[] key = Encoding.UTF8.GetBytes(WxPayConfig.key);
            string byte2String = String.Empty;
            using (HMACSHA256 h = new HMACSHA256(key))
            {
                byte[] hash = h.ComputeHash(msg);
                foreach (byte b in hash)
                {
                    byte2String += b.ToString("X2");
                }
                return byte2String;
            }
        }

        /// <summary>
        /// AES-256-ECB（PKCS7Padding）加密方法
        /// </summary>
        /// <param name="msg">加密前的数据</param>
        /// <param name="key">加密key</param>
        /// <returns>加密后的数据</returns>
        public static string EncryptAES256ECB(string msg,string key)
        {
            byte[] bkey = Encoding.UTF8.GetBytes(key);
            byte[] bmsg = Encoding.UTF8.GetBytes(msg);
            RijndaelManaged rijndael = new RijndaelManaged();
            rijndael.Key = bkey;
            rijndael.Mode = CipherMode.ECB;
            rijndael.Padding = PaddingMode.PKCS7;
            ICryptoTransform cryptoTransform = rijndael.CreateDecryptor();
            byte[] result = cryptoTransform.TransformFinalBlock(bmsg, 0, bmsg.Length);
            return Convert.ToBase64String(result);
        }
        /// <summary>
        /// AES-256-ECB（PKCS7Padding）解密方法
        /// </summary>
        /// <param name="msg">加密后的数据</param>
        /// <param name="key">解密key</param>
        /// <returns>解密后的数据</returns>
        public static string DecryptAES256ECB(string msg,string key)
        {
            byte[] bkey = Encoding.UTF8.GetBytes(key);
            byte[] bmsg = Encoding.UTF8.GetBytes(msg);
            RijndaelManaged rijndael = new RijndaelManaged();
            rijndael.Key = bkey;
            rijndael.Mode = CipherMode.ECB;
            rijndael.Padding = PaddingMode.PKCS7;
            ICryptoTransform cryptoTransform = rijndael.CreateDecryptor();
            byte[] result = cryptoTransform.TransformFinalBlock(bmsg, 0, bmsg.Length);
            return Convert.ToBase64String(result);
        }

    }
}
