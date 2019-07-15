using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeChatPay.WxApi
{
    public class Native
    {
        /// <summary>
        /// 统一下单接口
        /// </summary>
        /// <param name="body">商品描述</param>
        /// <param name="tradeNo">内部生成的订单号</param>
        /// <param name="cost">费用，单位为分</param>
        /// <param name="IP">设备的IP</param>
        /// <param name="startTime">交易开始时间</param>
        /// <param name="endTime">交易结束时间</param>
        /// <param name="notifyurl">回调URL</param>
        /// <param name="type">交易类型 Native时，prodID为必填项；若为JS支付，则openid为必填项</param>
        /// <param name="prodID">商品编号</param>
        /// <param name="limit">指定支付方式</param>
        /// <param name="openid">用户openid</param>
        /// <param name="receipt">是否开具发票</param>
        /// <param name="detail">商品详细描述</param>
        /// <param name="attach">商品附加信息</param>
        /// <param name="tags">商品优惠标记</param>
        /// <param name="scence">场景</param>
        /// <returns>请求结果</returns>
        public string UniteOrder(string body,string tradeNo,int cost,string IP, string notifyurl, int type,string prodID=null,bool limit=false,string openid=null,bool receipt=false, string startTime = null, string endTime = null, string detail=null,string attach=null,string tags=null,string scence=null )
        {
            WxPayData wd = new WxPayData();
            wd.SetValue("appid", WxPayConfig.appid);//用户公众号appid
            wd.SetValue("mch_id", WxPayConfig.mchid);//商户号appid
            wd.SetValue("device_info", "Web");//设备号
            wd.SetValue("nonce_str", WxUntil.GetRandomStr());//随机字符串
            wd.SetValue("sign_type", "MD5");//签名类型
            wd.SetValue("fee_type", "CNY");//货币种类
            wd.SetValue("body", body);//商品描述
            wd.SetValue("out_trade_no", tradeNo);//系统内部生成的订单号
            wd.SetValue("total_fee", cost);//消费金额
            wd.SetValue("spbill_create_ip", IP);//IP地址
            wd.SetValue("notify_url", notifyurl);//回调地址
            switch (type)
            {
                case 0:
                    if (String.IsNullOrEmpty(openid))
                    {
                        throw new Exception("当支付类型为JSAPI时，openID不能为空");
                    }
                    wd.SetValue("openid", openid);
                    wd.SetValue("trade_type", "JSAPI");
                    break;
                case 1:
                    if (String.IsNullOrEmpty(prodID))
                    {
                        throw new Exception("当支付类型为NATIVE时，prodNo不能为空!");
                    }
                    wd.SetValue("product_id", prodID);
                    wd.SetValue("trade_type", "NATIVE");
                    break;
                case 2:
                    wd.SetValue("trade_type", "APP");
                    break;
                case 3:
                    wd.SetValue("trade_type", "MWEB");
                    break;
                default:
                    throw new Exception("您输入的支付方式代码有误!");
            }
            if (!String.IsNullOrEmpty(openid))
            {
                wd.SetValue("openid", openid);
            }
            if (!String.IsNullOrEmpty(prodID))
            {
                wd.SetValue("product_id", prodID);
            }
            if (limit)
            {
                wd.SetValue("limit_pay", "no_credit");
            }
            if (receipt)
            {
                wd.SetValue("receipt", "Y");
            }
            if (!String.IsNullOrEmpty(scence))
            {
                wd.SetValue("scene_info", JObject.Parse(scence));
            }
            if (!String.IsNullOrEmpty(startTime))
            {
                wd.SetValue("time_start", startTime);
            }
            if (!String.IsNullOrEmpty(endTime))
            {
                wd.SetValue("time_expire", endTime);
            }
            if (!String.IsNullOrEmpty(tags))
            {
                wd.SetValue("goods_tag", tags);
            }
            if (!String.IsNullOrEmpty(detail))
            {
                wd.SetValue("detail", detail);
            }
            if (!String.IsNullOrEmpty(attach))
            {
                wd.SetValue("detail", attach);
            }
            return WxUntil.GetPostFinallyStr(WxPayConfig.api_url, wd.DicToXml());


        }

    }
}
