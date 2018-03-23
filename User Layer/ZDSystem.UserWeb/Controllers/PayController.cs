//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using PayComon;
//using ZDSystem.UserWeb.Controllers;

//namespace Web.Controllers
//{
//    public class PayController : MainBaseController
//    {
//        //
//        // GET: /Pay/

//        public ActionResult Index()
//        {
//            return View();
//        }

//        /// <summary>
//        /// 下单
//        /// </summary>
//        /// <returns></returns>
//        public ActionResult BookOrder()
//        {
//            string ip = Request.UserHostAddress;
//            if (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
//                ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(',')[0];//获取用户的真是IP
//            //var result = PayRequest.Instance.bookOrder(ip,
//            //                                 DateTime.Now.ToString("yyyyMMddHHmmssfff"),
//            //                               "测试订单",
//            //                                 1,
//            //                                 2);
//            return Json(new { url = string.Empty });
//        }

        
//        /// <summary>
//        /// 查询订单支付状态
//        /// </summary>
//        /// <returns></returns>
//        public ActionResult QueryOrder(string sOrderNo)
//        {
//           var result= PayRequest.Instance.QueryPayState(sOrderNo);
//           if (result.state)
//           {//支付成功

//               //处理业务逻辑
//           }
//           else
//           {//支付失败

//               //处理业务逻辑
//           }
//            return Json(new { url = result.error });
//        }


//        /// <summary>
//        /// 微信支付异步通知
//        /// </summary>
//        /// <returns></returns>
//        public void notify()
//        {
//            /*分为三个步骤
//              * 1.验证签名和结果通知
//              * 2.处理业务逻辑
//              * 3.同步返回微信系统结果
//              */
//            var result = PayRequest.Instance.GetPayNotityResult(Request);
//            if (result.state)
//            {//支付成功

//                //处理成功相关的业务逻辑
//            }
//            else
//            {//支付失败

//                //处理失败相关的业务逻辑
//            }
//            var returnParams = PayRequest.Instance.ReturnResult();
//            Response.Clear();
//            Response.ContentType = "text/xml";
//            Response.Write(returnParams);
//        }



//        /// <summary>
//        /// 申请退款
//        /// </summary>
//        /// <returns></returns>
//        public ActionResult PayRefund(string sOrderNo)
//        {
//            var result = PayRequest.Instance.PayRefund(sOrderNo, DateTime.Now.ToString("yyyyMMddHHmmssfff"), 1, 1);
//            if (result.state)
//            {//申请退款成功

//                //处理业务逻辑

//            }
//            else
//            {//申请退款失败

//                //处理业务逻辑
//            }
//            return Json(new { url = result.state });
//        }



//        /// <summary>
//        /// 微信退款异步通知
//        /// </summary>
//        public void RefundNotify()
//        {
//            /*分为三个步骤
//             * 1.验证签名和结果通知
//             * 2.处理业务逻辑
//             * 3.同步返回微信系统结果
//             */
//          var result=PayRequest.Instance.GetPayRefundNotifyResult(Request);
//          if (result.state)
//          {//退款成功

//              //处理成功相关的业务逻辑
//          }
//          else
//          {//退款失败

//              //处理失败相关的业务逻辑
//          }
//          var returnParams = PayRequest.Instance.ReturnResult();
//          Response.Clear();
//          Response.ContentType = "text/xml";
//          Response.Write(returnParams);

//        }
//    }
//}
