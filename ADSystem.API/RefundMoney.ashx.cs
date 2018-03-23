using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lib4Net.Logs;
using System.Text;
using Lib4Net.Core;
using System.IO;
using Codeplex.Data;

namespace ADSystem.API
{
    /// <summary>
    /// Summary description for RefundMoney
    /// </summary>
    public class RefundMoney : IHttpHandler
    {
        private static readonly ILogger logger = LoggerManager.Instance.GetLogger("refund");
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/json";
            string post_content = string.Empty;
            dynamic obj = null;
            try
            {
                using (StreamReader sr = new StreamReader(context.Request.InputStream, Encoding.UTF8))
                {
                    post_content = sr.ReadToEnd();
                }
                logger.Info("------------订单退款开始处理--------------");
                logger.Info("请求原串:" + post_content);
                try
                {
                    obj = DynamicJson.Parse(post_content);
                }
                catch
                {
                    Write(DynamicJson.Serialize(new { code = "9999", msg = "parse_fail", detailurl = "" }));
                    return;
                }

                if (!ApiHelper.CheckRefundParams(obj))
                {
                    Write(DynamicJson.Serialize(new { code = "9999", msg = "params_fail", detailurl = obj.detailurl }));
                    return;
                }

                if (!ApiHelper.CheckRefundSign(obj, logger))
                {
                    Write(DynamicJson.Serialize(new { code = "9999", msg = "sign_error", detailurl = obj.detailurl }));
                    return;
                }

                string msg = string.Empty;
                if (ApiHelper.SaveRefund(obj.orderid, obj.detailurl, Convert.ToDecimal(obj.price), logger, out msg))
                {
                    Write(DynamicJson.Serialize(new { code = "0000", msg = "success", detailurl = obj.detailurl }));
                }
                else
                {
                    Write(DynamicJson.Serialize(new { code = "9999", msg = msg, detailurl = obj.detailurl }));
                }
                logger.Info("保存退款结果:" + msg);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex.Message, ex);
            }
        }

        public void Write(string msg)
        {
            logger.Info(msg);
            HttpContext.Current.Response.Write(msg);
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