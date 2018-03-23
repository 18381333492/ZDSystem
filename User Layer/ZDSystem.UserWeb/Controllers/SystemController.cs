using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoginSystem.Model;
using LoginSystem.Service;
using Lib4Net.Data;
using ZDSystem.Utility;
using LoginSystem.SDK;
using Lib4Net.Framework.Settings;

namespace ZDSystem.UserWeb.Controllers
{
    public class SystemController : MainBaseController
    {
        public ActionResult Index()
        {
            if (LoginStatus.IsLogin)
            {
                List<MRolePermission> list = MemCacheSDK.GetUserMenuList(SettingHelper.Instance.GetData("SystemNo"), LoginStatus.UserName, LoginStatus.RoleID.Value, SettingHelper.Instance.GetData("GetValueUrl"));
                ViewBag.Modules = list;
                ViewBag.userName = LoginStatus.UserName;
                return View();
            }
            else
            {
                throw new Exception("未登录用户，请登录");
            }
        }
        /// <summary>
        /// 获取用户消息
        /// </summary>
        /// <returns></returns>
        public string GetMsgSituation()
        {
            LoginSystem.Model.UserMessageSituation state = LoginSystem.Service.LoginService.Instance.GetUserPageMsgCntr(LoginStatus.UserName);
            state.RecvType = LoginStatus.PageAlert.Value;
            return JsonData.JavaScriptSerialize(state);
        }
    }
}