using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Lib4Net.Core;
using Lib4Net.Logs;

namespace ZDSystem.UserWeb
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "System", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
        
        protected void Application_Error(object sender, EventArgs e)
        {
            var error = Server.GetLastError().GetBaseException();
            Server.ClearError();
            ILogger logger = LoggerManager.Instance.GetLogger("web");
            string msg = string.Format("系统发生异常,Url:{0}", Request.Url);
            logger.Error(msg, error);
            Application["aa"] = "ddd";
            Response.Redirect("/Shared/Error?msg=" + CommFun.UrlEncode(error.Message, "utf-8"), true);
        }
    }
}