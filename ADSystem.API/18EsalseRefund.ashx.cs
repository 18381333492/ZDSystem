using System;
using System.Text;
using System.Web;
using Lib4Net.Logs;
using SecurityUtil;

namespace ADSystem.API
{
    /// <summary>
    /// _18EsalseRefund 的摘要说明
    /// </summary>
    public class _18EsalseRefund : IHttpHandler
    {
        private static readonly ILogger logger = LoggerManager.Instance.GetLogger("18EsalseRefund");

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                context.Response.ContentType = "text/plain";
                logger.Info("------------------Esalse退款开始------------------------");
                logger.Info("请求参数：" + context.Request.RawUrl);
                string channelNo = context.Request["cid"];
                string orderNo = context.Request["oid"];
                string orderFace = context.Request["fm"];
                string sucFace = context.Request["rm"];
                string tsp = context.Request["tsp"];
                string signStr  = context.Request["sign"];
                //校验参数
                if (string.IsNullOrEmpty(channelNo) || string.IsNullOrEmpty(orderNo) ||
                string.IsNullOrEmpty(orderFace) || string.IsNullOrEmpty(sucFace) ||
                string.IsNullOrEmpty(signStr))
            {

                logger.Info("参数错误:必填参数为空,收到参数【" + context.Request.QueryString + "】");
                context.Response.Write("Fail");
                return;
            };

                logger.Info("-------------------签名校验开始---------------------------");

                string esKey = ApiHelper.Get_es_key(channelNo);
                string originStr = string.Format("{0}{1}{2}{3}{4}{5}", channelNo, orderNo, orderFace, sucFace,tsp,esKey);
                logger.Info("es_key:" + esKey);
                string sign = SecurityCore.ToHex(SecurityCore.MD5(originStr, Encoding.GetEncoding("GBK")), true).ToLower();
                if (!sign.Equals(signStr, StringComparison.OrdinalIgnoreCase))
                {
                    logger.Info(string.Format("签名失败,签名原串:{0},签名:{1},源签名:{2}", originStr, sign, signStr));
                    context.Response.Write("Fail");
                    return;
                }

                logger.Info("-------------------保存退款申请开始-------------------------");

                bool rst = ApiHelper.Save18RefundApply(orderNo, decimal.Parse(sucFace), logger);
                context.Response.Write(rst ? "success" : "Fail");
                return;
            }
            catch (Exception ex)
            {
                logger.Info("异常：" + ex.Message, ex);
                logger.Fatal("异常：" + ex.Message, ex);
                context.Response.Write("Fail System Error");
                return;
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}