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
    public class ManualReviewController : MainBaseController
    {
        //
        // GET: /ManualReview/
        /// <summary>
        /// 订单表审核
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderReview()
        {
            return View(OrderMainService.Instance.ReviewQuery(Request.QueryString));
        }
        public ActionResult OrderManaul(string id)
        {
            ViewBag.Id = id;
            return View();
        }
        [HttpPost]
        public IResult OrderManaul()
        {
            try
            {
                string user = LoginStatus.UserName;
                IResult result = OrderMainService.Instance.OrderManaul(user, Request.Form);
                if (result.Status)
                {
                    result.SetSuccessMessage("审核成功");
                }
                else
                {
                    result.SetErrorMessage("审核失败");
                }
                return result;
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }
        /// <summary>
        /// 订单发货表审核
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderShipReview()
        {
            return View(OrderDeliveryService.Instance.ReviewQuery(Request.QueryString));
        }

        public ActionResult OrderShipManaul(string id)
        {
            ViewBag.Id = id;
            ViewBag.deliveryId = Request["deliveryId"];
            MOrderMain model = new MOrderMain();
            model = OrderMainService.Instance.getFace(id);
            return View(model);
        }
        [HttpPost]
        public IResult OrderShipManaul()
        {
            try
            {
                string user = LoginStatus.UserName;
                IResult result = OrderDeliveryService.Instance.OrderShipManaul(user, Request.Form);
                if (result.Status)
                {
                    result.SetSuccessMessage("审核成功");
                }
                else
                {
                    result.SetErrorMessage("审核失败");
                }
                return result;
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }
    }
}
