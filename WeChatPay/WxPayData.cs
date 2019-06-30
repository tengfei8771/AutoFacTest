using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace WeChatPay
{
    public class WxPayData
    {
        //微信通讯数据格式变量必须根据ascii码顺序进行排序，使用SortedDictionary进行排序
        private SortedDictionary<string, object> _keyValues = new SortedDictionary<string, object>();
        /// <summary>
        /// 设置变量值
        /// </summary>
        /// <param name="Key">变量名称</param>
        /// <param name="Value">变量值</param>
        public void SetValue(string Key,object Value)
        {
            _keyValues[Key] = Value;
        }
        /// <summary>
        /// 根据键名查询键值
        /// </summary>
        /// <param name="Key">键名</param>
        /// <returns>键值</returns>
        public object GetValue(string Key)
        {
            object t = null;
            _keyValues.TryGetValue(Key, out t);
            return t;
        }
        /// <summary>
        /// 查询键名是否已经被占用
        /// </summary>
        /// <param name="Key">键名</param>
        /// <returns>true or false</returns>
        public bool IsSet(string Key)
        {
            object t = null;
            _keyValues.TryGetValue(Key, out t);
            if (t != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 将Dic类型数据转成xml字符串
        /// </summary>
        /// <returns>xml字符串</returns>
        public string DicToXml()
        {
            string xml = "<xml>";
            if (_keyValues.Count == 0)
            {
                throw new WxPayException("dic内不能为空！");
            }
            else
            {
                foreach(var PairAndValue in _keyValues)
                {
                    if (PairAndValue.Value == null)
                    {
                        throw new WxPayException("不能为空值!");
                    }
                    else
                    {
                        if (PairAndValue.Value.GetType() == typeof(int))
                        {
                            xml += "<" + PairAndValue.Key.ToString() + ">" + PairAndValue.Value + "</" + PairAndValue.Key.ToString() + ">";
                        }
                        else if(PairAndValue.Value.GetType() == typeof(string))
                        {
                            xml += "<" + PairAndValue.Key + ">" + "<![CDATA[" + PairAndValue.Value + "]]></" + PairAndValue.Key + ">";
                        }
                        else
                        {
                            throw new WxPayException("只能包含int或者string类型的数据");
                        }
                    }
                }
                xml += "</xml>";
                return xml;
            }
        }

        public void SetSignValue()
        {
            if (IsSet("sign"))
            {
                throw new WxPayException("已有签名字段!");
            }
            else
            {
                SetValue("sign", MD5Sign(SecretStr(DicToUrl())));
            }
        }

        public SortedDictionary<string,object> XmlToDic(string XMLstring)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(XMLstring);
            XmlNode FirstNode = xml.FirstChild;
            XmlNodeList list = FirstNode.ChildNodes;
            foreach(XmlNode node in list)
            {
                _keyValues[node.Name] = node.InnerText;
            }
            try
            {
                if (CheckSign())
                {
                    return _keyValues;
                }
                else
                {
                    throw new WxPayException("sign字段认证失败!");
                }
            }
            catch(WxPayException e)
            {
                throw new WxPayException(e.Message);
            }

        }

        public bool CheckSign()
        {
            if (!IsSet("sign"))
            {
                throw new WxPayException("返回格式数据未签名!");
            }
            else
            {
                //if (GetValue("sign") == null || GetValue("sign") == "")
                if (string.IsNullOrEmpty(GetValue("sign").ToString()))
                {
                    throw new WxPayException("返回数据签名存在但是为空!");
                }
                string sign = GetValue("sign").ToString();
                _keyValues.Remove("sign");
                string MD5Str = MD5Sign(SecretStr(DicToUrl()));
                if(sign== MD5Str)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }
        /// <summary>
        /// 将字典键值转化为url格式
        /// </summary>
        /// <returns>url格式字符串</returns>
        public string DicToUrl()
        {
            string str = string.Empty;
            if (_keyValues.Count == 0)
            {
                throw new Exception("dic内不能为空！");
            }
            else
            {
                foreach (var pairs in _keyValues)
                {
                    if (pairs.Value == null)
                    {
                        throw new Exception("键值不能为空！！");
                    }
                    else if (pairs.Key == "sign")
                    {
                        throw new Exception("数据键名不能为'sign'！！");
                    }
                    else
                    {
                        str += pairs.Key.ToString() + "=" + pairs.Value + "&";
                    }
                }
                str=str.TrimEnd('&');
            }
            return str; 
        }
        /// <summary>
        /// 将key和URL格式的进行拼接
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string SecretStr(string str)
        {
            WxPayConfig wxPayConfig = new WxPayConfig();
            string key = wxPayConfig.key;
            string CompositeString = str +"&" +key;
            return CompositeString;
        }
        /// <summary>
        /// MD5加密算法
        /// </summary>
        /// <param name="CompositeString">参数字符串和秘钥字符串拼接而成的字符串</param>
        /// <returns>MD5加密后的字符串</returns>
        public string MD5Sign(string CompositeString)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bt = Encoding.UTF8.GetBytes(CompositeString);
            byte[] bt1 = md5.ComputeHash(bt);
            string byte2String = String.Empty;
            foreach(byte b in bt1)
            {
                byte2String += b.ToString("X2");
            }
            return byte2String;
        }
        /// <summary>
        /// 随机字符串生成方法
        /// </summary>
        /// <returns></returns>
        public string GetRandomStr()
        {
            const string conStr= @"0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            string randomStr = string.Empty;
            Random rd = new Random();
            for (int i = 0; i < conStr.Length/2; i++)
            {
                randomStr += conStr[rd.Next(0, conStr.Length)].ToString();
            }
            return randomStr;
        }
    }
}
