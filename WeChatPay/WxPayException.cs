using System;
using System.Collections.Generic;
using System.Text;

namespace WeChatPay
{
    public class WxPayException:Exception
    {
        public WxPayException(string msg):base(msg)
        {

        }
    }
}
