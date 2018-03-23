using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LoginSystem.SDK;
using LoginSystem.LoginCache;

namespace ZDSystem.UserWeb
{
    /// <summary>
    /// delPermission 的摘要说明
    /// </summary>
    public class delPermission : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string key = context.Request["key"];
            //MemcacheHelper.MemcacheHelper.Remove(LoginSystem.Memcached.CacheType.UserNavigates, key);
            LoginSystem.SDK.MemCacheSDK.Remove(CacheType.UserNavigates, key);
            context.Response.Write("ok");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}