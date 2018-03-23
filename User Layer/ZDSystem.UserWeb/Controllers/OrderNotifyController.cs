using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lib4Net.Core;
using Lib4Net.Framework.Settings;
using Lib4Net.Comm;
using Lib4Net.Logs;
using ZDSystem.UserService;
using ZDSystem.Entity;
using ZDSystem.Model;
using ZDSystem.Utility;

namespace ZDSystem.UserWeb.Controllers
{
    /// <summary>
    /// Controller：OrderNotify(订单通知)
    /// </summary>
    public class OrderNotifyController : MainBaseController
    {
        /// <summary>
        /// 显示列表页面数据
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
          return View(OrderNotifyService.Instance.Query(Request.QueryString));
        }
    }
}