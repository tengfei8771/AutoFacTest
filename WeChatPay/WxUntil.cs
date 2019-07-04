using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;

namespace WeChatPay
{
    public static class WxUntil
    {
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
            DateTime dt = DateTime.Now.AddMinutes(Min).ToUniversalTime();
            //DateTime dt = DateTime.UtcNow.Add(TimeSpan.FromMinutes(Min));       
            TimeSpan ts = dt - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
    }
}
