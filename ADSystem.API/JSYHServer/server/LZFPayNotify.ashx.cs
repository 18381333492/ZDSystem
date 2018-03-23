using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lib4Net.Logs;
using Newtonsoft.Json;
using System.Text;
using CCBSign;
using System.Configuration;
using System.Data;
using System.IO;

namespace ADSystem.API.JSYHServer.server
{
    /// <summary>
    /// 建设银行龙支付异步回调
    /// </summary>
    public class LZFPayNotify : IHttpHandler
    {
        private static readonly ILogger logger = LoggerManager.Instance.GetLogger("JSYHPayNotify");
        public void ProcessRequest(HttpContext context)
        {
            string requestParmas = string.Empty;
            string content = string.Empty;
            try
            {
                logger.Info("----------------建设银行龙支付回调开始---------------------");
                var requestDictionary = new Dictionary<string, object>();
                logger.Info(JsonConvert.SerializeObject(context.Request.QueryString));//打印参数顺序
                var SignString = new StringBuilder();
                foreach (string key in context.Request.QueryString.Keys)
                {
                    requestDictionary.Add(key, context.Request.QueryString[key]);
                }
                requestParmas = JsonConvert.SerializeObject(requestDictionary);//打印参数
                logger.Info("请求参数:" + requestParmas);
                //拼接签名字符串
                SignString.AppendFormat("POSID={0}&", context.Request.QueryString["POSID"]);
                SignString.AppendFormat("BRANCHID={0}&", context.Request.QueryString["BRANCHID"]);
                SignString.AppendFormat("ORDERID={0}&", context.Request.QueryString["ORDERID"]);
                SignString.AppendFormat("PAYMENT={0}&", context.Request.QueryString["PAYMENT"]);
                SignString.AppendFormat("CURCODE={0}&", context.Request.QueryString["CURCODE"]);
                SignString.AppendFormat("REMARK1={0}&", context.Request.QueryString["REMARK1"]);
                SignString.AppendFormat("REMARK2={0}&", context.Request.QueryString["REMARK2"]);
                if (context.Request.QueryString["ACC_TYPE"] != null)
                    SignString.AppendFormat("ACC_TYPE={0}&", context.Request.QueryString["ACC_TYPE"]);
                SignString.AppendFormat("SUCCESS={0}&", context.Request.QueryString["SUCCESS"]);
                SignString.AppendFormat("TYPE={0}&", context.Request.QueryString["TYPE"]);
                SignString.AppendFormat("REFERER={0}&", context.Request.QueryString["REFERER"]);
                SignString.AppendFormat("CLIENTIP={0}&", context.Request.QueryString["CLIENTIP"]);
                if (context.Request.QueryString["DISCOUNT"] != null)
                    SignString.AppendFormat("DISCOUNT={0}&", context.Request.QueryString["DISCOUNT"]);
                string signString = SignString.ToString().TrimEnd('&');

                logger.Info("参与数字签名的字符串:" + signString);
                string pubKey = GetPubkey().Trim();//获取pubKey
                logger.Info("pubKey:" + pubKey);
                string oldSign = context.Request.QueryString["SIGN"];
                logger.Info("原签名:" + oldSign);
                //开始签名
                RSASig sign = new RSASig();
                sign.setPublicKey(pubKey);
                bool result =sign.verifySigature(oldSign, signString);
                if (result)
                {
                    logger.Info("签名验证成功");
                    bool iState = false;
                    if (context.Request.QueryString["SUCCESS"] == "Y")
                    {//支付成功
                        logger.Info("支付成功");
                        iState = true;
                    }
                    else
                    {//支付失败
                        logger.Info("支付失败");
                        iState = false;
                    }
                    decimal total_amount = Convert.ToDecimal(requestDictionary["PAYMENT"]) * 100;
                    bool res=ApiHelper.WxPayNotifyHandle(requestDictionary["ORDERID"].ToString(),//商户订单号
                                                iState,//支付状态
                                                string.Empty,//支付宝交易号
                                                total_amount.ToString(),//本次交易支付的订单金额
                                                string.Empty,
                                                logger);
                    if (res)
                    {
                        //返回处理成功页面
                        StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "JSYHServer\\html\\success.html", System.Text.Encoding.GetEncoding("utf-8"));
                         content = sr.ReadToEnd().ToString();
                        sr.Close();
                    }
                    else
                    {
                        //返回处理失败页面
                        StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "JSYHServer\\html\\error.html", System.Text.Encoding.GetEncoding("utf-8"));
                        content = sr.ReadToEnd().ToString();
                        sr.Close();

                    }
                }
                else
                {
                    logger.Info("签名验证失败");
                    //返回处理失败页面
                    StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "JSYHServer\\html\\error.html", System.Text.Encoding.GetEncoding("utf-8"));
                    content = sr.ReadToEnd().ToString();
                    sr.Close();

                }
                  context.Response.Clear();
                  context.Response.Write(content);
            }
            catch (Exception ex)
            {
                logger.Info("----------------龙支付回调异常---------------------");
                logger.Info("异常错误信息：" + ex.Message);
                logger.Fatal("异常错误信息：" + ex.Message, ex);
                //返回处理失败页面
                StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "JSYHServer\\html\\error.html", System.Text.Encoding.GetEncoding("utf-8"));
                content = sr.ReadToEnd().ToString();
                sr.Close();
                context.Response.Clear();
                context.Response.Write(content);
            }
        }

        /// <summary>
        /// 获取Pubkey
        /// </summary>
        /// <returns></returns>
        public string GetPubkey()
        {
            var down_channel_no = ConfigurationManager.AppSettings["jsyh_channel_no"];//获取下游渠道编号
            logger.Info(down_channel_no);
            string sSql = string.Format(@"select PRI_KEY from RECEIPT_ACCOUNT_INFO t where t.DOWN_CHANNEL_NO={0}", down_channel_no);
            DataTable dt = SqlHelper.GetDataTable(sSql);
            if (dt.Rows.Count == 1)
            {
                string pubKey =Convert.ToString(dt.Rows[0][0]);
                logger.Info(pubKey);
                return pubKey;
            }
            else
            {
                return null;
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