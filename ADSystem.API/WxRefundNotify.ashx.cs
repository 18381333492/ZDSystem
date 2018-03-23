using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lib4Net.Logs;
using PayComon;

namespace ADSystem.API
{
    /// <summary>
    /// WxRefundNotifu 的摘要说明
    /// </summary>
    public class WxRefundNotify : IHttpHandler
    {
        private static readonly ILogger logger = LoggerManager.Instance.GetLogger("WxRefundNotify");
        public void ProcessRequest(HttpContext context)
        {
            string requestParmas = string.Empty;
            var ResponeParams = new Dictionary<string, string>();
            try
            {
                logger.Info("----------------微信退款回调开始---------------------");
                byte[] bNetStream = context.Request.BinaryRead(context.Request.ContentLength);//获取微信异步回调的请求数据的字节流
                string sRequestParam = System.Text.Encoding.UTF8.GetString(bNetStream);
                var Parameters = TenPayHelp.GetDictionaryFromCDATAXml(sRequestParam);
                //获取微信key
                string ApiKey = ApiHelper.QueryPayAccount(Parameters["appid"], 2, logger).Pubkey;
                if (string.IsNullOrEmpty(ApiKey))
                {//获取支付Key为空
                    logger.Info("微信支付APIKey为空,导致签名验证失败");
                }
                //验证支付签名
                var result = PayRequest.Instance.GetPayRefundNotifyResult(ApiKey, Parameters);
                requestParmas = sRequestParam + result.paramsData;
                logger.Info("请求参数:" + requestParmas);
                logger.Info("退款状态：" + result.state.ToString());
                logger.Info("信息描述：" + result.error);
                if (!string.IsNullOrEmpty(result.data))
                {//处理业务逻辑
                    List<string> listData = result.data.Split(',').ToList();
                    ApiHelper.WxRefundNotifyHandle(listData[0], listData[1], result.state, result.error, logger);
                    ResponeParams.Add("return_code", "SUCCESS");
                    ResponeParams.Add("return_msg", "OK");
                }
                else
                {
                    ResponeParams.Add("return_code", "FAIL");
                    ResponeParams.Add("return_msg", "FAIL");
                }
            }
            catch (Exception e)
            {
                logger.Fatal("程序错误信息：" + e.Message, e);
                logger.Info("程序错误信息：" + e.Message, e);
                ResponeParams.Add("return_code", "FAIL");
                ResponeParams.Add("return_msg", "Systyem Server Error");
            }
            string res = TenPayHelp.InstallCDATAXml(ResponeParams);
            logger.Info("返回给微信的参数：" + res);
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