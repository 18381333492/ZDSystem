using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZDSystem.UserWeb.Controllers
{
    /// <summary>
    /// Controller : ZdCouponUsed(使用过的中大优惠券)
    /// </summary>
    public class ZdCouponUsedController : MainBaseController
    {
        public ActionResult Index()
        {
            return View(ZDSystem.UserService.ZdCouponUsedService.Instance.Query(Request.QueryString));
        }

    }
}
