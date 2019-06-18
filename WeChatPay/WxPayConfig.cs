﻿using System;
using System.Linq;

namespace WeChatPay
{
    public class WxPayConfig
    {
        public static WxPayConfig Instance = new WxPayConfig();

        public string appid = "";//APPID

        public string mchid = "";//商户号

        public string key = "";//商户API密钥

        public string appSecret = "";//公众号支付和app支付时候将用到

        public string notify_url = "http://www.baidu.com/Pay/WxNotify";//回调页地址

        public string api_url = "https://api.mch.weixin.qq.com/pay/unifiedorder";//微信支付调用接口地址
    }
}
