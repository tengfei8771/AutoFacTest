using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace WeChatPay
{
    public class WxPayData
    {
        public const string SIGN_TYPE_MD5 = "MD5";
        public const string SIGN_TYPE_HMAC_SHA256 = "HMAC-SHA256";
        //微信通讯数据格式变量必须根据ascii码顺序进行排序，使用排序dictionary可以排序
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
            object t = _keyValues.TryGetValue(Key, out t);
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
            t = _keyValues.TryGetValue(Key, out t);
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
        /// 将dic类型数据转成xml字符串
        /// </summary>
        /// <returns>xml字符串</returns>
        public string DicToXml()
        {
            string xml = "<xml>";
            if (_keyValues.Count == 0)
            {
                throw new Exception("dic内不能为空！");
            }
            else
            {
                foreach(var PairAndValue in _keyValues)
                {
                    if (PairAndValue.Value == null)
                    {
                        throw new Exception("不能为空值!");
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
                            throw new Exception("只能包含int或者string类型的数据");
                        }
                    }
                }
                xml += "</xml>";
                return xml;
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

        public string SecretStr(string str)
        {
            WxPayConfig wxPayConfig = new WxPayConfig();
            string key = wxPayConfig.key;
            string CompositeString = str + key;
            return CompositeString;
        }
        public string MakeSign(string SignType)
        {
            string result = string.Empty;
            if(SignType== SIGN_TYPE_HMAC_SHA256)
            {

            }
        }

        public string SHA256(string str)
        {
            byte[] SHA256Data = Encoding.UTF8.GetBytes(str);
            SHA256Managed Sha256 = new SHA256Managed();
            byte[] by = Sha256.ComputeHash(SHA256Data);
            return BitConverter.ToString(by).Replace("-", "").ToLower();
        }

    }
}
