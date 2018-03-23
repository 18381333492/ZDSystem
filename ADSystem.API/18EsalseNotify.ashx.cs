using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lib4Net.Logs;

namespace ADSystem.API
{
    /// <summary>
    /// _18EsalseNotify 的摘要说明
    /// </summary>
    public class _18EsalseNotify : IHttpHandler
    {
        private static readonly ILogger logger = LoggerManager.Instance.GetLogger("EsalseNotify");
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                context.Response.ContentType = "text/plain";
                logger.Info("------------------Esalse回调开始------------------------");
                logger.Info("请求参数：" + context.Request.RawUrl);
                NotifyDataModel model = new NotifyDataModel();
                model.sid = context.Request["sid"];
                model.ste = context.Request["ste"];
                model.cid = context.Request["cid"];
                model.pid = context.Request["pid"];
                model.oid = context.Request["oid"];
                model.pn = context.Request["pn"];
                model.tf = context.Request["tf"];
                model.fm = context.Request["fm"];
                model.dm = context.Request["dm"];
                model.price = context.Request["price"];
                model.sign = context.Request["sign"];
                if (!ApiHelper.CheckParams(model, logger))
                {
                    context.Response.Write("Fail");
                    return;
                }

                logger.Info("-------------------签名校验开始---------------------------");
                if (!ApiHelper.EsalseNotifyCheckSign(model, logger))
                {
                    context.Response.Write("Fail");
                    return;
                }

                logger.Info("-------------------通知订单结果开始-------------------------");
                //获取校验码
                string orderStatus = ApiHelper.GetCfgError(model.ste);
                if (!string.IsNullOrEmpty(orderStatus))
                {
                    logger.Info("结果已知：[" + orderStatus + "]");
                    int? status = ApiHelper.GetRstStatus(model.ste);
                    string msg = status == 1 ? "N|充值成功" : "N|充值失败";
                    decimal? cost_price = 0;
                    if (!string.IsNullOrEmpty(model.dm)) {
                        cost_price = Convert.ToDecimal(model.fm) * Convert.ToDecimal(model.dm);
                    }
                  bool rst =  ApiHelper.SaveNotify(Convert.ToInt64(model.oid), model.sid, Convert.ToDecimal(model.fm), msg, status, cost_price, Convert.ToDecimal(model.price), logger);
                  if (rst)
                  {
                      context.Response.Write("success");
                  }
                  else {
                      context.Response.Write("Fail");
                  }
                    return;
                }
                else
                {
                    string rstMsg = string.Format("结果未知:ste={0}", model.ste);
                    logger.Info(rstMsg);
                    context.Response.Write("Fail");
                    return;
                }
            }
            catch (Exception ex)
            {
                logger.Info("异常："+ex.Message,ex);
                logger.Fatal("异常：" + ex.Message, ex);
                context.Response.Write("Fail System Error");
                return;
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