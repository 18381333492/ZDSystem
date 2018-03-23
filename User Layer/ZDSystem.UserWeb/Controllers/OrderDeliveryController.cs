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
    /// Controller：OrderDelivery(订单发货表)
    /// </summary>
    public class OrderDeliveryController : MainBaseController
    {
        /// <summary>
        /// 显示列表页面数据
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
          return View(OrderDeliveryService.Instance.Query(Request.QueryString));
        }
        /// <summary>
        /// 预览详细信息页面
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        public ActionResult Details(string id)
        {
            //获取订单信息
            OrderMainItemModel orderModel = OrderMainService.Instance.QueryOrderDetials(id);
            //获取发货信息
            List<MOrderDelivery> deliveryList = OrderDeliveryService.Instance.QueryDeliveryListByOrderNo(id);
            //获取通知信息
            List<MOrderNotify> notifyList = OrderNotifyService.Instance.QueryNotifyListByOrderNo(id);
            ViewBag.DeliveryList = deliveryList;
            ViewBag.NotifyList = notifyList.FindAll(item=>item.NotifyType==3);
            return View(orderModel);
        }       
       
    }
}