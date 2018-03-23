using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoginSystem.Service;
using LoginSystem.SDK;
using Lib4Net.Framework.Settings;

namespace ZDSystem.UserWeb.Controllers
{
    public class MemberController : Controller
    {
        //
        // GET: /Member/

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginOn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginOn(string userName, string password)
        {
            if (MemCacheSDK.GetLoginStatus(SettingHelper.Instance.GetData("SystemNo"), SettingHelper.Instance.GetData("GetValueUrl")).IsLogin)
                MemCacheSDK.LoginOut(SettingHelper.Instance.GetData("SystemNo"), MemCacheSDK.GetLoginStatus(SettingHelper.Instance.GetData("SystemNo"), SettingHelper.Instance.GetData("GetValueUrl")).UserName);

            if (!LoginService.Instance.UserEnabled(userName))
            {
                ModelState.AddModelError("SummaryError", "用户不存在或被禁用");
                return View();
            }
            bool retLogin = LoginService.Instance.LoginOn(userName.ToLowerInvariant(), password);
            if (retLogin)
            {
                string redirectUrl = "/system/index"; //?url=" + RedirectBack(userName);
                return new RedirectResult(redirectUrl, false);
            }
            ModelState.AddModelError("SummaryError", "用户名或密码输入错误");
            ViewBag.UserName = userName;
            return View();
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginOut()
        {
            MemCacheSDK.LoginOut(SettingHelper.Instance.GetData("SystemNo"), MemCacheSDK.GetLoginStatus(SettingHelper.Instance.GetData("SystemNo"), SettingHelper.Instance.GetData("GetValueUrl")).UserName);
            string returnUrl = "http://" + System.Web.HttpContext.Current.Request.Url.Authority + "/Member/MarkLoginInfo";
            string loginUrl = Lib4Net.Framework.Settings.SettingHelper.Instance.GetData("LoginUrl");
            return new RedirectResult(loginUrl + "/Member/loginOut?redirectUrl=" + returnUrl, false);
        }


        public ActionResult MarkLoginInfo(string guid)
        {
            if (!string.IsNullOrEmpty(guid))
            {
                MemCacheSDK.MarkLoginLocal(guid);
            }
            return new RedirectResult("/system/index", false);
        }

    }
}