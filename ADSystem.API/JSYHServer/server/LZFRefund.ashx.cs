using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lib4Net.Logs;
using PayComon;
using System.Collections.Specialized;
using System.Text;
using System.Collections;

namespace ADSystem.API.JSYHServer.server
{
    /// <summary>
    /// 龙支付的退款接口
    /// </summary>
    public class LZFRefund : IHttpHandler
    {
        private static readonly ILogger logger = LoggerManager.Instance.GetLogger("JSYHRefund");
        public void ProcessRequest(HttpContext context)
        {
            var result = new Message();
            try
            {
                logger.Info("----------------龙支付退款开始---------------------");
                string appid = context.Request["appid"];
                string out_trade_no = context.Request["out_trade_no"];
                string out_refund_no = context.Request["out_refund_no"];
                string total_fee = decimal.Parse(context.Request["total_fee"]).ToString();
                string refund_fee = decimal.Parse(context.Request["refund_fee"]).ToString(); ;
                string refund_desc = context.Request["refund_desc"];

                logger.Info(string.Format(@"龙支付退款的输入参数:appid={0},
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
                     //获取龙支付账户配置信息
                    var config = ApiHelper.QueryPayAccount(appid, 4, logger);
                    //拼接退款参数
                    StringBuilder requestParams = new StringBuilder();
                    requestParams.Append("<?xml version=\"1.0\" encoding=\"GB2312\" standalone=\"yes\" ?>");
                    requestParams.AppendFormat(@"<TX>
                                        <REQUEST_SN>{0}</REQUEST_SN>
                                        <CUST_ID>{1}</CUST_ID> 
                                        <USER_ID>{2}</USER_ID> 
                                        <PASSWORD>{3}</PASSWORD> 
                                        <TX_CODE>5W1004</TX_CODE> 
                                        <LANGUAGE>CN</LANGUAGE>
                                        <TX_INFO> 
                                        <MONEY>{4}</MONEY> 
                                        <ORDER>{5}</ORDER> 
                                        </TX_INFO>
                                        <SIGN_INFO>签名信息</SIGN_INFO> 
                                        <SIGNCERT>签名CA信息</SIGNCERT> 
                                        </TX>", DateTime.Now.ToString("yyyyMMddHHmmssff"), config.Appid, config.Ext3, config.Ext4, refund_fee, out_trade_no);
                    var respone=TCPHelper.Request(requestParams.ToString(), logger);
                    result.data = respone;
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