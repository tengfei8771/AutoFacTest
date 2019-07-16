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
            //wd.SetValue("sign_type", "MD5");//签名类型
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
                        throw new WxPayException("当支付类型为JSAPI时，openID不能为空");
                    }
                    wd.SetValue("openid", openid);
                    wd.SetValue("trade_type", "JSAPI");
                    break;
                case 1:
                    if (String.IsNullOrEmpty(prodID))
                    {
                        throw new WxPayException("当支付类型为NATIVE时，prodNo不能为空!");
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
                    throw new WxPayException("您输入的支付方式代码有误!");
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
            return WxUntil.GetPostFinallyStr(WxPayConfig.BaseUrl+WxPayConfig.WxPay, wd.DicToXml());
        }
        /// <summary>
        /// 查询订单
        /// </summary>
        /// <param name="transaction_id">微信订单号</param>
        /// <param name="out_trade_no">商家内部订单号</param>
        /// <returns></returns>
        public string QueryOrder(string transaction_id = null, string out_trade_no = null)
        {
            WxPayData wd = new WxPayData();
            wd.SetValue("appid", WxPayConfig.appid);
            wd.SetValue("mch_id", WxPayConfig.mchid);
            if (String.IsNullOrEmpty(transaction_id) && String.IsNullOrEmpty(out_trade_no))
            {
                throw new WxPayException("商家订单号和微信订单号不能同时为空!");
            }
            if (String.IsNullOrEmpty(transaction_id))
            {
                wd.SetValue("transaction_id", transaction_id);
            }
            else
            {
                wd.SetValue("out_trade_no", out_trade_no );
            }
            wd.SetValue("nonce_str", WxUntil.GetRandomStr());
            wd.SetValue("sign", wd.MakeSign());
            return WxUntil.GetPostFinallyStr(WxPayConfig.BaseUrl + WxPayConfig.OrderQuery, wd.DicToXml());
        }

        /// <summary>
        /// 关闭订单接口
        /// </summary>
        /// <param name="out_trade_no">商家内部订单号</param>
        /// <returns></returns>
        public string CloseOrder(string out_trade_no)
        {
            WxPayData wd = new WxPayData();
            wd.SetValue("appid", WxPayConfig.appid);
            wd.SetValue("mch_id", WxPayConfig.mchid);
            wd.SetValue("out_trade_no", out_trade_no);
            wd.SetValue("nonce_str", WxUntil.GetRandomStr());
            wd.SetValue("sign", wd.MakeSign());
            return WxUntil.GetPostFinallyStr(WxPayConfig.BaseUrl + WxPayConfig.CloseOrder,wd.DicToXml());

        }
        /// <summary>
        /// 微信申请退款接口
        /// </summary>
        /// <param name="out_refund_no">商户退款单号</param>
        /// <param name="total_fee">订单金额</param>
        /// <param name="refund_fee">退款金额</param>
        /// <param name="refund_fee_type">退款货币种类</param>
        /// <param name="refund_desc">退款原因</param>
        /// <param name="refund_account">退款资金来源</param>
        /// <param name="notify_url">回调地址</param>
        /// <param name="transaction_id">微信订单号</param>
        /// <param name="out_trade_no">商家内部订单号</param>
        /// <returns></returns>
        public string Refund(string out_refund_no,int total_fee, int refund_fee,string refund_fee_type=null,string refund_desc=null,bool refund_account=true,string notify_url=null, string transaction_id = null, string out_trade_no = null)
        {
            WxPayData wd = new WxPayData();
            wd.SetValue("appid", WxPayConfig.appid);
            wd.SetValue("mch_id", WxPayConfig.mchid);
            wd.SetValue("nonce_str", WxUntil.GetRandomStr());
            if (String.IsNullOrEmpty(transaction_id) && String.IsNullOrEmpty(out_trade_no))
            {
                throw new WxPayException("商家订单号和微信订单号不能同时为空!");
            }
            if (String.IsNullOrEmpty(transaction_id))
            {
                wd.SetValue("transaction_id", transaction_id);
            }
            else
            {
                wd.SetValue("out_trade_no", out_trade_no);
            }
            wd.SetValue("out_refund_no", out_refund_no);
            wd.SetValue("refund_fee", refund_fee);
            wd.SetValue("total_fee", total_fee);
            if (!String.IsNullOrEmpty(refund_fee_type))
            {
                wd.SetValue("refund_fee_type", refund_fee_type);
            }
            if (!String.IsNullOrEmpty(refund_desc))
            {
                wd.SetValue("refund_desc", refund_desc);
            }
            if (!refund_account)
            {
                wd.SetValue("refund_account", "REFUND_SOURCE_RECHARGE_FUNDS");
            }
            if (!string.IsNullOrEmpty(notify_url))
            {
                wd.SetValue("notify_url", notify_url);
            }
            wd.SetValue("sign", wd.MakeSign());
            return WxUntil.GetPostFinallyStr(WxPayConfig.BaseUrl + WxPayConfig.Refund, wd.DicToXml());
        }
        /// <summary>
        /// 微信退款查询接口
        /// </summary>
        /// <param name="refund_id">微信退款单号</param>
        /// <param name="out_refund_no">商户退款单号</param>
        /// <param name="transaction_id">微信订单号</param>
        /// <param name="out_trade_no">商户订单号</param>
        /// <param name="offset">偏移量</param>
        /// <returns></returns>
        public string RefundQuery(string refund_id=null, string out_refund_no=null,string transaction_id=null,string out_trade_no=null, int offset=0)
        {
            WxPayData wd = new WxPayData();
            wd.SetValue("appid", WxPayConfig.appid);
            wd.SetValue("mch_id", WxPayConfig.mchid);
            wd.SetValue("nonce_str", WxUntil.GetRandomStr());
            if (!String.IsNullOrEmpty(refund_id))
            {
                wd.SetValue("refund_id", refund_id);
            }
            else
            {
                if (!String.IsNullOrEmpty(out_refund_no))
                {
                    wd.SetValue("out_refund_no", out_refund_no);
                }
                else
                {
                    if (!String.IsNullOrEmpty(transaction_id))
                    {
                        wd.SetValue("transaction_id", transaction_id);
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(out_trade_no))
                        {
                            wd.SetValue("out_trade_no", out_trade_no);
                        }
                        else
                        {
                            throw new WxPayException("请您输入必要的订单参数!");
                        }
                    }
                }
            }
            if (offset != 0)
            {
                wd.SetValue("offset", offset);
            }
            wd.SetValue("sign", wd.MakeSign());
            return WxUntil.GetPostFinallyStr(WxPayConfig.BaseUrl + WxPayConfig.RefundQuery, wd.DicToXml());
        }

    }
}
