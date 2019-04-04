using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;

namespace Redis
{
    public class RedisHelper:IRedisHelper
    {
        private ConnectionMultiplexer redis { get; set; }
        private IDatabase db { get; set; }
        private static string ConStr { get; set; }

        public static string getConStr()
        {
            string ConStr = "";
            try
            {
                using (StreamReader fs = File.OpenText(Directory.GetCurrentDirectory() + "\\DBConfig.json"))
                {
                    using (JsonTextReader reader = new JsonTextReader(fs))
                    {
                        JObject t = (JObject)JToken.ReadFrom(reader);
                        ConStr = t["REDIS"].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return ConStr;
        }
        public RedisHelper()
        {
            ConStr = getConStr();
            redis = ConnectionMultiplexer.Connect(ConStr);
            db = redis.GetDatabase();
        }
        /// <summary>
        /// redis插入方法
        /// </summary>
        /// <param name="Key">键</param>
        /// <param name="Value">值</param>
        /// <returns>操作是否成功</returns>
        public bool SetValue(string Key,string Value)
        {
            return db.StringSet(Key, Value);
        }

        /// <summary>
        /// redis删除入方法
        /// </summary>
        /// <param name="Key">键</param>
        /// <returns>操作是否成功</returns>
        public bool DelValue(string Key)
        {
            return db.KeyDelete(Key);
        }
        /// <summary>
        /// redis查询方法
        /// </summary>
        /// <param name="Key">键</param>
        /// <returns>返回值</returns>
        public string GetValue(string Key)
        {
            return db.StringGet(Key);
        }
    }
}
