using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PayComon;
using Lib4Net.Logs;

namespace ADSystem.API
{
    /// <summary>
    /// WxPayNotify 的摘要说明
    /// 微信支付异步回调通知
    /// </summary>
    public class WxPayNotify : IHttpHandler
    {
        private static readonly ILogger logger = LoggerManager.Instance.GetLogger("WxPayNotify");
        public void ProcessRequest(HttpContext context)
        {
            string requestParmas = string.Empty;
            var ResponeParams = new Dictionary<string, string>();
            try
            {
                logger.Info("----------------微信支付回调开始---------------------");
                byte[] bNetStream = context.Request.BinaryRead(context.Request.ContentLength);//获取微信异步回调的请求数据的字节流
                string sRequestParam = System.Text.Encoding.UTF8.GetString(bNetStream);
                logger.Info("原始参数:" + sRequestParam);
                requestParmas = sRequestParam;
                var Parameters = TenPayHelp.GetDictionaryFromCDATAXml(sRequestParam);
                //获取微信key
                string ApiKey = ApiHelper.QueryPayAccount(Parameters["appid"], 2, logger).Pubkey;
                if (string.IsNullOrEmpty(ApiKey))
                {//获取支付Key为空
                    logger.Info("微信支付APIKey为空,导致签名验证失败");
                }
                //验证支付签名
                var result = PayRequest.Instance.GetPayNotityResult(ApiKey, Parameters);
                logger.Info("请求参数:" + sRequestParam);
                logger.Info("支付状态：" + result.state.ToString());
                logger.Info("信息描述：" + result.error);
                //支付处理业务逻辑
                if (!string.IsNullOrEmpty(result.data))
                {
                    ApiHelper.WxPayNotifyHandle(result.data, result.state, Parameters["transaction_id"], Parameters["total_fee"], Parameters["openid"], logger);
                }
                if (result.error == "签名验证失败")
                {
                    ResponeParams.Add("return_code", "FAIL");
                    ResponeParams.Add("return_msg", "FAIL");
                }
                else
                {
                    ResponeParams.Add("return_code", "SUCCESS");
                    ResponeParams.Add("return_msg", "OK");
                }
            }
            catch (Exception e)
            {
                logger.Info("----------------微信支付回调异常---------------------");
                logger.Info("异常错误信息：" + e.Message, e);
                logger.Fatal("异常错误信息：" + e.Message, e);
                ResponeParams.Add("return_code", "FAIL");
                ResponeParams.Add("return_msg", "Systyem Server Error");
            }
            string res = TenPayHelp.InstallCDATAXml(ResponeParams);
            logger.Info("返回给微信的参数："+res);
            context.Response.Clear();
            context.Response.ContentType = "text/xml";
            context.Response.Write(res);
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