using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using redis;
using System.Configuration;

namespace ADSystem.API
{
    public class RedisHelper
    {
        public static RedisCommand command = new RedisCommand(ConfigurationManager.AppSettings["redisConn"], 3000);

        public static void SetCache(string key, string data, int expire=0)
        {
            command.Exec(new List<string> { "SET", key, data }, new List<string>());
            if (expire > 0)
            {
                command.Exec(new List<string> { "EXPIRE", key, expire.ToString() }, new List<string>());
            }
        }

        public static string GetCache(string key)
        {
            List<string> lstOutput = new List<string>();
            command.Exec(new List<string> { "GET", key }, lstOutput);
            if (lstOutput.Count > 0)
            {
                return lstOutput[0];
            }
            return string.Empty;
        }
    }
}