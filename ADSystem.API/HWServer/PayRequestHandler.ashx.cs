using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lib4Net.Logs;
using Newtonsoft.Json;
using ADSystem.API.HWServer.Model;
using PayComon;
using System.Configuration;

namespace ADSystem.API.HWServer
{
    /// <summary>
    /// 华为订单充值的支付请求
    /// </summary>
    public class PayRequestHandler : IHttpHandler
    {
        private static readonly ILogger logger = LoggerManager.Instance.GetLogger("HWPayrequest");
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                var result = string.Empty;//返回结果
                //创建订单
                string requestData = context.Request.Form["orderInfo"];
                logger.Info("华为支付请求参数:" + requestData);
                HWOrder orderInfo = JsonConvert.DeserializeObject<HWOrder>(requestData);
                if (!string.IsNullOrEmpty(orderInfo.orderno))
                {//订单编号不为空
                    //根据订单编号获取订单数据
                   orderInfo=Server.GetOrderInfo(orderInfo.orderno,orderInfo.pay_type);
                   if (orderInfo == null)
                   {
                       logger.Info("订单数据异常");
                       result = JsonConvert.SerializeObject(new { success = false, info = "订单数据异常" });
                   }
                }
                //获取用户的真实IP
                string ip = context.Request.UserHostAddress;
                if (context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                    ip = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(',')[0];//获取用户的真实的IP
                orderInfo.user_ip =ip.Split(':')[0];
                orderInfo.down_channel_no = ConfigurationManager.AppSettings["hw_channel_no"];//获取下游渠道编号
                RevOrderResult revOrderResult=null;
                //判断是否是慢充
                //if (orderInfo.recharge_mode == 0) 
                //    revOrderResult = Server.SaveOrder(orderInfo, logger);
                //else
                revOrderResult = Server.SaveSlowOrder(orderInfo, logger);
                logger.Info(string.Format("订单状态:{0},消息:{1}", revOrderResult.Status, revOrderResult.ResultMsg));
                if (revOrderResult.Status)
                {
                    logger.Info("订单保存成功");
                    QueryAccountResult accountResult = ApiHelper.QueryPayAccount(revOrderResult.AppId, revOrderResult.AccountType, logger);
                    if (!accountResult.Status)
                    {
                        logger.Info("获取支付信息失败");
                        result = JsonConvert.SerializeObject(new { success = false, info = "获取支付信息失败" });
                    }
                    //请求支付
                    var message = PayRequest.Instance.bookOrder(new TenPayConfig()
                    {
                        appid = accountResult.Appid,
                        mch_id = accountResult.Mchid,
                        key = accountResult.Pubkey,
                        merchant_private_key=accountResult.Prikey
                    }, orderInfo.user_ip, revOrderResult.OrderNo, orderInfo.product_name, revOrderResult.PayFee, revOrderResult.AsyncUrl, revOrderResult.SyncUrl, Convert.ToInt32(orderInfo.pay_type),6);
                    if (message.state)
                    {
                        logger.Info("支付请求成功");
                        result = JsonConvert.SerializeObject(new { success = true,
                                                                    url = message.data,
                                                                    info = "支付请求成功", 
                                                                    orderno = revOrderResult.OrderNo,
                                                                    appid = revOrderResult.AppId,
                                                                    account_type = revOrderResult.AccountType
                        });
                    }
                    else
                    {
                        logger.Info("支付请求失败");
                        logger.Info("支付返回的数据:" + message.error);
                        result = JsonConvert.SerializeObject(new { success = false, info = "支付请求失败" });
                    }
                }
                else
                {
                    logger.Info("订单保存失败");
                    result = JsonConvert.SerializeObject(new { success = false, info = "订单保存失败" });
                    ApiHelper.SetFail(revOrderResult.OrderNo, revOrderResult.ResultMsg, logger);
                }
                context.Response.ContentType = "text/plain";
                context.Response.Write(result);
            }
            catch (Exception e)
            {
                logger.Info(e.Message);
                logger.Fatal(e.Message, e);
                var res = new { code = 200, msg = "服务错误" };
                context.Response.ContentType = "text/plain";
                context.Response.Write(JsonConvert.SerializeObject(res));
            }
         
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}