using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lib4Net.Logs;
using Newtonsoft.Json;
using ADSystem.API.HWServer.Model;
using System.Configuration;
using ADSystem.API.HWServer;
using System.Text;
using PayComon;

namespace ADSystem.API.JSYHServer.server
{
    /// <summary>
    /// 建设银行下单支付请求
    /// </summary>
    public class BookOrder : IHttpHandler
    {
        private static readonly ILogger logger = LoggerManager.Instance.GetLogger("JSYHBookOrder");
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                var result = string.Empty;//返回结果
                //创建订单
                string requestData = context.Request.Form["orderInfo"];
                logger.Info("请求参数:" + requestData);
                var orderInfo = JsonConvert.DeserializeObject<HWOrder>(requestData);
                //获取用户的真实IP
                string ip = context.Request.UserHostAddress;
                if (context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                    ip = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(',')[0];//获取用户的真实的IP
                orderInfo.user_ip = ip.Split(':')[0];
                orderInfo.down_channel_no = ConfigurationManager.AppSettings["jsyh_channel_no"];//获取下游渠道编号
                var revOrderResult = Server.SaveOrder(orderInfo, logger);
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
                    else
                    {
                        //拼接支付信息  HttpUtility.UrlEncode
                        var PayParams = new Dictionary<string, object>();
                        PayParams.Add("MERCHANTID", accountResult.Appid);//商户代码
                        PayParams.Add("POSID", accountResult.Mchid);//商户柜台代码
                        PayParams.Add("BRANCHID", accountResult.Ext1);//分行代码
                        PayParams.Add("ORDERID", revOrderResult.OrderNo);//商户订单号PAYMENT
                        PayParams.Add("PAYMENT", revOrderResult.PayFee);//付款金额
                        PayParams.Add("CURCODE", "01");//币种
                        PayParams.Add("TXCODE", accountResult.Ext2);//交易码
                        PayParams.Add("REMARK1", string.Empty);
                        PayParams.Add("REMARK2", string.Empty);
                        PayParams.Add("TYPE", 1);//接口类型
                        PayParams.Add("PUB", accountResult.Pubkey);
                        PayParams.Add("GATEWAY", 0);//网关类型 默认送0
                        PayParams.Add("CLIENTIP", string.Empty);
                        PayParams.Add("REGINFO", string.Empty);
                        PayParams.Add("PROINFO", string.Empty);
                        PayParams.Add("REFERER", string.Empty);
                        StringBuilder Sign = new StringBuilder();
                        foreach (var key in PayParams.Keys)
                        {
                            Sign.AppendFormat("{0}={1}&", key, PayParams[key].ToString());
                        }
                        string sign = TenPayHelp.MD5(Sign.ToString().TrimEnd('&')).ToLower();
                        PayParams.Add("TIMEOUT", string.Empty);
                        PayParams.Add("MAC", sign);
                        PayParams.Remove("PUB");
                        StringBuilder PayInfo = new StringBuilder();
                        foreach (var key in PayParams.Keys)
                        {
                            PayInfo.AppendFormat("{0}={1}&", key, PayParams[key].ToString());
                        }
                        string payinfo = PayInfo.ToString().TrimEnd('&');
                        result = JsonConvert.SerializeObject(new
                           {
                               success = true,
                               info = "支付请求成功",
                               data = payinfo
                           });
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
                var res = new { code = 200, info = "服务错误" };
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