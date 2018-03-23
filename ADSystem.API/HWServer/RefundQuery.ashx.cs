using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PayComon;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using Lib4Net.Logs;

namespace ADSystem.API.HWServer
{
    /// <summary>
    /// RefundQuery 的摘要说明
    /// 用于平安银行的退款查询
    /// </summary>
    public class RefundQuery : IHttpHandler
    {

        private static readonly ILogger logger = LoggerManager.Instance.GetLogger("PingAnRefundQuery");
        public void ProcessRequest(HttpContext context)
        {
            string sTipMessage = string.Empty;
            try
            {
                logger.Info("----------------微信退款查询开始---------------------");

                string appid = context.Request.Form["appid"];
                string out_refund_no = context.Request.Form["out_refund_no"];

                string sign = context.Request["sign"];
                //签名Key
                QueryAccountResult payAccount = ApiHelper.QueryPayAccount(appid, 2, logger);
                string sSignKey = "HJHSJHFSUOUSF564564HJJJHHJSDSDSD";// payAccount.Pubkey;
                string newSign = GetSign(context.Request.Form, sSignKey);
                logger.Info(string.Format(@"微信退款查询的输入参数:订单号:
                                                                          out_refund_no:{0}",  out_refund_no));
                if (sign == newSign)
                {//签名认证成功
                    //获取支付账户信息
                    var result = PingAnPay.PayRefundQuery(new TenPayConfig()
                    {
                        appid = payAccount.Appid,
                        mch_id = payAccount.Mchid,
                        key = payAccount.Pubkey,
                        SSLCERT_PATH = payAccount.CertPath
                    }, out_refund_no);
                    sTipMessage = result.data;
                }
                else
                {
                    sTipMessage = "签名验证失败";
                }
            }
            catch (Exception e)
            {
                logger.Info("----------------微信退款查询异常---------------------");
                logger.Fatal("异常错误信息：" + e.Message, e);
                sTipMessage = "System Server Error";
            }
            context.Response.Clear();
            context.Response.ContentType = "text/xml";
            context.Response.Write(sTipMessage);
        }


        /// <summary>
        /// 创建签名
        /// </summary>
        /// <param name="Parameters"></param>
        /// <param name="sSignKey"></param>
        /// <returns></returns>
        public string GetSign(NameValueCollection Parameters, string sSignKey)
        {
            StringBuilder Sign = new StringBuilder();
            var Keys = new ArrayList(Parameters.Keys);
            Keys.Sort();//字典排序
            foreach (string key in Keys)
            {
                if (!string.IsNullOrEmpty(Parameters[key]) && key != "sign")
                {//拼接成键值对字符串
                    Sign.Append(key + "=" + Parameters[key] + "&");
                }
            }
            Sign.Append("key=" + sSignKey);
            string sign = TenPayHelp.MD5(Sign.ToString()).ToLower();
            return sign;
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