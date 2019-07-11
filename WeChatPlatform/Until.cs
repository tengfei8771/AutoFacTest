using System;
using System.Collections.Generic;
using System.Text;
using WeChatPlatform.Config;

namespace WeChatPlatform
{
    public class Until
    {
        public static double GetTimeSpan(double Min = 0)
        {     
            TimeSpan ts = DateTime.Now.AddMinutes(Min).ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return ts.TotalSeconds;
        }

        /// <summary>
        /// 拼接URL方法
        /// </summary>
        /// <param name="partUrl">发起请求的部分URL</param>
        /// <returns>拼接完成后的URL</returns>
        public static string CreateUrl(string partUrl)
        {
            if (UserConfig.token == null || UserConfig.token == "")
            {
                throw new Exception("未获取有效Token");
            }
            //else if (!(Until.GetTimeSpan()<UserConfig.expireTime))
            //{
            //    Token.GetToken();
            //    return "https://" + UrlConfig.BaseApiURL + partUrl + UserConfig.token;
            //}
            else
            {
                return "https://" + UrlConfig.BaseApiURL + partUrl + UserConfig.token;
            }
        }
    }
}
