using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lib4Net.Framework.Settings;
using LoginSystem.Model;
using LoginSystem.Service;
using Lib4Net.Data;
using ZDSystem.Utility;
using ZDSystem.UserService;
using LoginSystem.SDK;

namespace ZDSystem.UserWeb.Controllers
{
    public class SharedController : MainBaseController
    {
        //
        // GET: /Shared/

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult NoPermission()
        {
            return View();
        }

        /// <summary>
        /// 用户导航菜单
        /// </summary>
        /// <returns></returns>
        public ActionResult NavigateMenu()
        {
            if (LoginStatus.IsLogin)
            {
                ViewBag.MenuList = MemCacheSDK.GetUserMenuList(SettingHelper.Instance.GetData("SystemNo"), LoginStatus.UserName, LoginStatus.RoleID.Value, SettingHelper.Instance.GetData("GetValueUrl")); 
                return View();
            }
            else
            {
                throw new Exception("未登录用户，请登录");
            }
        }
    }
}