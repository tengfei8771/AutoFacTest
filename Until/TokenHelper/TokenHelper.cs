using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Until.TokenHelper
{
    public class TokenHelper
    {
        public static string securityKey { get; set; }

        private static string getKey()
        {
            string str;
            try
            {
                using (StreamReader fs = File.OpenText(Directory.GetCurrentDirectory() + "\\appsettings.json"))
                {
                    using(JsonTextReader reader = new JsonTextReader(fs))
                    {
                        JObject t = (JObject)JToken.ReadFrom(reader);
                        str = t["securityKey"].ToString();
                    }
                }
            }
            catch(Exception e)
            {
                throw e;
            }
            return str;
        }

        public static long ToUnixEpochDate(DateTime date) =>
            (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        public static Dictionary<string, object> GetPayLoad(string encodeJwt)
        {
            var jwtArr = encodeJwt.Split('.');
            var payLoad = JsonConvert.DeserializeObject<Dictionary<string, object>>(Base64UrlEncoder.Decode(jwtArr[1]));
            return payLoad;
        }
        /// <summary>
        /// 手动实现生成Token的方法，和下面的方法实现效果一致
        /// </summary>
        /// <param name="payLoad">载荷，存放不重要的用户数据，例如用户ID</param>
        /// <param name="expiresMinute">过期时间</param>
        /// <param name="header">Http请求头部信息</param>
        /// <returns>生成的token</returns>
        public static string CreateToken(Dictionary<string, object> payLoad, int expiresMinute, Dictionary<string, object> header = null)
        {
            if (header == null)
            {
                header = new Dictionary<string, object>(new List<KeyValuePair<string, object>>() {
                    new KeyValuePair<string, object>("alg", "HS256"),
                    new KeyValuePair<string, object>("typ", "JWT")
                });
            }
            getKey();
            //添加jwt可用时间（应该必须要的）
            var now = DateTime.UtcNow;
            payLoad["nbf"] = ToUnixEpochDate(now);//可用时间起始
            payLoad["exp"] = ToUnixEpochDate(now.Add(TimeSpan.FromMinutes(expiresMinute)));//可用时间结束

            var encodedHeader = Base64UrlEncoder.Encode(JsonConvert.SerializeObject(header));
            var encodedPayload = Base64UrlEncoder.Encode(JsonConvert.SerializeObject(payLoad));

            var hs256 = new HMACSHA256(Encoding.ASCII.GetBytes(securityKey));
            var encodedSignature = Base64UrlEncoder.Encode(hs256.ComputeHash(Encoding.UTF8.GetBytes(string.Concat(encodedHeader, ".", encodedPayload))));

            var encodedJwt = string.Concat(encodedHeader, ".", encodedPayload, ".", encodedSignature);
            return encodedJwt;
        }
        /// <summary>
        /// 微软的方法实现token生成
        /// </summary>
        /// <param name="payLoad">载荷</param>
        /// <param name="expiresMinute">过期时间</param>
        /// <returns>生成的token</returns>
        public static string CreateTookenByHandler(Dictionary<string,object>payLoad, int expiresMinute)
        {
            var now = DateTime.UtcNow;
            var claim = new List<Claim>();
            foreach(var key in payLoad.Keys)
            {
                var tempCliam = new Claim(key, payLoad[key]?.ToString());
                claim.Add(tempCliam);
            }
            getKey();
            var jwt = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claim,
                notBefore: now,
                expires: now.Add(TimeSpan.FromMinutes(expiresMinute)),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(securityKey)), SecurityAlgorithms.HmacSha256));
            var encodeJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodeJwt;
        }
        /// <summary>
        /// 验证token
        /// </summary>
        /// <param name="encodeJwt">token</param>
        /// <param name="validatePayLoad">自定义验证载荷</param>
        /// <returns>验证是否成功</returns>

        public static bool Validate(string encodeJwt)//Func<Dictionary<string, object>, bool> validatePayLoad 自定义验证参数
        {
            var success = true;
            var jwtArr = encodeJwt.Split('.');
            var header = JsonConvert.DeserializeObject<Dictionary<string, object>>(Base64UrlEncoder.Decode(jwtArr[0]));
            var payLoad = JsonConvert.DeserializeObject<Dictionary<string, object>>(Base64UrlEncoder.Decode(jwtArr[1]));

            var hs256 = new HMACSHA256(Encoding.ASCII.GetBytes(securityKey));
            //首先验证签名是否正确（必须的）
            success = success && string.Equals(jwtArr[2], Base64UrlEncoder.Encode(hs256.ComputeHash(Encoding.UTF8.GetBytes(string.Concat(jwtArr[0], ".", jwtArr[1])))));
            if (!success)
            {
                return success;//签名不正确直接返回
            }
            //其次验证是否在有效期内（也应该必须）
            var now = ToUnixEpochDate(DateTime.UtcNow);
            success = success && (now >= long.Parse(payLoad["nbf"].ToString()) && now < long.Parse(payLoad["exp"].ToString()));

            //再其次 进行自定义的验证
            //success = success && validatePayLoad(payLoad);

            return success;
        }
        public  TokenHelper()
        {
            securityKey = getKey();
        }
    }
}
