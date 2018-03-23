using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PayComon;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using Lib4Net.Logs;

namespace ADSystem.API
{
    /// <summary>
    /// WxApplyRefund 的摘要说明
    /// 微信申请退款
    /// </summary>0
    public class WxApplyRefund : IHttpHandler
    {
        private static readonly ILogger logger = LoggerManager.Instance.GetLogger("WxApplyRefund");
        public void ProcessRequest(HttpContext context)
        {
            string sTipMessage = string.Empty;
            Message result=new Message();
            try
            {
                logger.Info("----------------微信退款申请开始---------------------");
         
                string appid = context.Request["appid"];
                string out_trade_no = context.Request["out_trade_no"];
                string out_refund_no = context.Request["out_refund_no"];
                string total_fee = context.Request["total_fee"];
                string refund_fee = context.Request["refund_fee"];
                string refund_desc =  context.Request["refund_desc"];
                string sign = context.Request["sign"];

                logger.Info(string.Format(@"原始参数:订单号:appid={0},
                                                                          out_trade_no:{1},
                                                                          out_refund_no:{2},
                                                                          totalfee:{3},
                                                                          refundfee:{4},
                                                                          refund_desc:{5},
                                                                          sign:{6}", appid, out_trade_no, out_refund_no, refund_fee, refund_desc, refund_desc, sign));

                //签名Key
                string sSignKey = TenPayHelp.MD5(appid).ToLower();
                string newSign = GetSign(context.Request.Form, sSignKey);
                int totalfee =Convert.ToInt32((Convert.ToDecimal(total_fee) * 100));
                int refundfee = Convert.ToInt32((Convert.ToDecimal(refund_fee) * 100));

                logger.Info(string.Format(@"微信退款申请的输入参数:订单号:appid={0},
                                                                          out_trade_no:{1},
                                                                          out_refund_no:{2},
                                                                          totalfee:{3}
                                                                          refundfee:{4}
                                                                          refund_desc:{5}", appid, out_trade_no, out_refund_no, totalfee, refundfee, refund_desc));
                if (sign == newSign)
                {//签名认证成功
                    //获取支付账户信息
                    var PayAccount = ApiHelper.QueryPayAccount(appid, 2, logger);
                      result = PayRequest.Instance.PayRefund(new TenPayConfig()
                      {
                          appid = PayAccount.Appid,
                          mch_id = PayAccount.Mchid,
                          key = PayAccount.Pubkey,
                          SSLCERT_PATH = PayAccount.CertPath
                      }, out_trade_no, out_refund_no, totalfee, refundfee, refund_desc);
                    sTipMessage = result.data;     
                }
                else
                {
                    sTipMessage = "签名验证失败";
                }
            }
            catch(Exception e)
            {
                logger.Info("----------------微信退款申请异常---------------------");
                logger.Fatal("异常错误信息：" + e.Message, e);
                sTipMessage = "System Server Error";
            }
            logger.Info("返回结果:" + sTipMessage);
            context.Response.Clear();
            context.Response.ContentType = "text/plain";
            context.Response.Write(sTipMessage);
        }
        

        /// <summary>
        /// 创建签名
        /// </summary>
        /// <param name="Parameters"></param>
        /// <param name="sSignKey"></param>
        /// <returns></returns>
        public string GetSign(NameValueCollection Parameters,string sSignKey)
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