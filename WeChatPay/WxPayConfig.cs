using System;
using System.Linq;

namespace WeChatPay
{
    public class WxPayConfig
    {
        public static WxPayConfig Instance = new WxPayConfig();

        public static readonly string appid = "";//APPID

        public static readonly string mchid = "";//商户号

        public static readonly string key = "aaaaa";//商户API密钥

        public static readonly string appSecret = "";//公众号支付和app支付时候将用到

        public static readonly string notify_url = "http://www.baidu.com/Pay/WxNotify";//回调页地址

        
        public static readonly string BaseUrl = "https://api.mch.weixin.qq.com";
        public static readonly string BaseUrl2 = "https://api2.mch.weixin.qq.com";
        public static readonly string WxPay = "/pay/unifiedorder";//微信支付调用接口地址
        public static readonly string OrderQuery = "/pay/orderquery";//订单查询接口
        public static readonly string CloseOrder = "/pay/closeorder";//关闭订单接口
        public static readonly string Refund = "/secapi/pay/refund";//退款接口
        public static readonly string RefundQuery = "/secapi/pay/refundquery";//退款查询接口
        public static readonly string DownloadBill = "/pay/downloadbill";//下载对账单接口
        public static readonly string DownloadFundflow = "/pay/downloadfundflow";//下载资金对账单
        public static readonly string BatchQueryComment = "/billcommentsp/batchquerycomment";//拉取评价数据
    }
}
