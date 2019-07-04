using System;
using System.Collections.Generic;
using System.Text;

namespace WeChatPay.WxApi
{
    public class Native
    {
        public void UniteOrder()
        {
            WxPayData wd = new WxPayData();
            wd.SetValue("appid", WxPayConfig.appid);
            wd.SetValue("mch_id", WxPayConfig.mchid);
            wd.SetValue("device_info", "Web");
            wd.SetValue("nonce_str", WxUntil.GetRandomStr());
            wd.SetValue("sign_type", "MD5");
            wd.SetValue("fee_type", "CNY");
        }
    }
}
