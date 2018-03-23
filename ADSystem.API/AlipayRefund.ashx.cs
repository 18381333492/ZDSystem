using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lib4Net.Logs;
using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Domain;
using Aop.Api.Response;
using PayComon;
using System.Collections.Specialized;
using System.Text;
using System.Collections;

namespace ADSystem.API
{
    /// <summary>
    /// 支付宝退款
    /// </summary>
    public class AlipayRefund : IHttpHandler
    {
        private static readonly ILogger logger = LoggerManager.Instance.GetLogger("AlipayRefund");
        public void ProcessRequest(HttpContext context)
        {
            var result = new Message();
            try
            {
                logger.Info("----------------支付宝退款---------------------");
                string appid = context.Request["appid"];
                string out_trade_no = context.Request["out_trade_no"];
                string out_refund_no = context.Request["out_refund_no"];
                string total_fee = decimal.Parse(context.Request["total_fee"]).ToString();
                string refund_fee = decimal.Parse(context.Request["refund_fee"]).ToString(); ;
                string refund_desc = context.Request["refund_desc"];

                logger.Info(string.Format(@"支付宝退款的输入参数:appid={0},
                                                                        out_trade_no:{1},
                                                                        out_refund_no:{2},
                                                                        totalfee:{3}
                                                                        refundfee:{4}
                                                                        refund_desc:{5}", appid, out_trade_no, out_refund_no, total_fee, refund_fee, refund_desc));
                string sign = context.Request["sign"];
                //签名Key
                string sSignKey = TenPayHelp.MD5(appid).ToLower();
                string newSign = GetSign(context.Request.Form, sSignKey);
                if (newSign == sign)
                {
                    //获取支付宝账户配置信息
                    var config = ApiHelper.QueryPayAccount(appid, 1, logger);
                    logger.Info(string.Format(@"Appid={0},Prikey={1},Pubkey={2}", config.Appid, config.Prikey, config.Pubkey));
                    IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do",
                        config.Appid,
                        config.Prikey,
                        "json",
                        "1.0",
                        "RSA2",
                        config.Pubkey,
                        "utf-8",
                        false);
                    AlipayTradeRefundRequest request = new AlipayTradeRefundRequest();
                    AlipayTradeRefundModel model = new AlipayTradeRefundModel();
                    model.OutTradeNo = out_trade_no;//商户订单号  //"20170821141701640";
                    model.OutRequestNo = out_refund_no;//退款编号
                    model.RefundAmount = refund_fee;//退款金额
                    model.RefundReason = refund_desc;//退款原因
                    request.SetBizModel(model);
                    AlipayTradeRefundResponse response = client.Execute(request);
                    result.data = response.Body;
                }
                else
                {
                    result.data = "内部签名验证失败";
                }
            }
            catch (Exception e)
            {
                logger.Info("----------------支付宝退款异常---------------------");
                logger.Info("异常错误信息：" + e.Message, e);
                logger.Fatal("异常错误信息：" + e.Message, e);
                result.data = "Systyem Server Error";
            }
            context.Response.Clear();
            context.Response.ContentType = "text/plain";
            context.Response.Write(result.data);
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