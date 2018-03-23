using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lib4Net.Logs;
using LoginSystem.Model;
using LoginSystem.Service;
using Lib4Net.Framework.Data;
using ZDSystem.Utility;
using Lib4Net.Framework.Settings;
using LoginSystem.SDK;
using LoginSystem.RedisCached;

namespace ZDSystem.UserWeb.Controllers
{
    public class MainBaseController : Controller
    {
        private static ILogger logger;
        static MainBaseController()
        {
            logger = LoggerManager.Instance.GetLogger("web");
        }
        private UserLoginStatus _status;
        protected UserLoginStatus LoginStatus
        {
            get
            {
                if (_status == null)
                {
                    _status = MemCacheSDK.GetLoginStatus(SettingHelper.Instance.GetData("SystemNo"), SettingHelper.Instance.GetData("GetValueUrl"));
                }
                return _status;
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);

            //判断登录状态
            if (LoginStatus == null || LoginStatus.IsLogin == false)
            {
                string returnUrl = "http://" + System.Web.HttpContext.Current.Request.Url.Authority + "/Member/MarkLoginInfo";
                returnUrl = HttpUtility.UrlEncode(returnUrl, System.Text.Encoding.UTF8);
                string loginUrl = Lib4Net.Framework.Settings.SettingHelper.Instance.GetData("LoginUrl");
                returnUrl = string.Format("{0}?redirectUrl={1}", loginUrl + "/Member/LoginOn", returnUrl);
                filterContext.Result = new RedirectResult(returnUrl);
                return;
            }
            //判断用户页面权限
            string matchUrl = UrlMappingUtility.Instance.Match().MatchUrl.ToLower();
            if (!MemCacheSDK.CheckPageRight(LoginStatus.UserName, LoginStatus.RoleID.Value, SettingHelper.Instance.GetData("SystemNo"), SettingHelper.Instance.GetData("GetValueUrl"), matchUrl))
            {
                filterContext.Result = new RedirectResult("/Shared/NoPermission");
                return;
            }

            base.OnActionExecuting(filterContext);
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            Exception ex = filterContext.Exception;

            LogException(ex);
            base.OnException(filterContext);
        }

        protected void LogException(Exception ex)
        {
            string msg = "请求地址:" + Request.Url + " \r\n错误信息:" + ex.Message;

            if (ex is ActionException)
                logger.Error(msg, ex);
            else
                logger.Fatal(msg, ex);
        }

        protected void LogException(string message)
        {
            logger.Error(message);
        }

    }
}