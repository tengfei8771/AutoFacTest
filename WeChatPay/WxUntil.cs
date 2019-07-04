using System;
using System.Collections.Generic;
using System.Management;
using System.Net;
using System.Text;

namespace WeChatPay
{
    public class WxUntil
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

        [Obsolete]
        public static string GetIPAdress()
        {
            string strHostName = Dns.GetHostName(); //得到本机的主机名

            IPHostEntry ipEntry = Dns.GetHostByName(strHostName); //取得本机IP

            string strAddr = ipEntry.AddressList[0].ToString();
            return (strAddr);
        }

    }
}
