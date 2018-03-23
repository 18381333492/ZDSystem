using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADSystem.API.JSYHServer.server
{
    /// <summary>
    /// 建设银行扫一扫入口
    /// </summary>
    public class Index : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var mobileType = 0;//手机类型 0-非建设银行手机APP打开 1-IOS 2-安卓
            string mobieParams=context.Request.Headers["CCBWebView-User-Agent"];
            if (!string.IsNullOrEmpty(context.Request.Headers["CCBWebView-User-Agent"]))
            {
                if (mobieParams.Contains("iPhone"))
                    mobileType = 1;
                if (mobieParams.Contains("Android"))
                    mobileType = 2;
            }
            //获取用户user_id;
            string user_id =context.Request.QueryString["user_id"];

            context.Response.Redirect("/JSYHServer/html/activity.html?mobileType=" + mobileType + "&user_id="+user_id);
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