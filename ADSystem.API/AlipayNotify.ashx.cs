using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lib4Net.Logs;
using Aop.Api;
using Aop.Api.Util;
using PayComon;

namespace ADSystem.API
{
    /// <summary>
    /// 支付宝的支付通知回调
    /// </summary>
    public class AlipayNotify : IHttpHandler
    {
        private static readonly ILogger logger = LoggerManager.Instance.GetLogger("AlipayNotify");
        public void ProcessRequest(HttpContext context)
        {
            var result=new Message();
            try
            {
                logger.Info("----------------支付宝支付回调开始---------------------");
                byte[] bNetStream = context.Request.BinaryRead(context.Request.ContentLength);
                string RequestParams = System.Text.Encoding.UTF8.GetString(bNetStream);
                RequestParams = HttpUtility.UrlDecode(RequestParams);
                logger.Info("请求参数:" + RequestParams);
                //请求的数据集合
                var requestDictionary = new AopDictionary();

                foreach (string key in context.Request.Form.Keys)
                {
                    requestDictionary.Add(key, context.Request.Form[key]);
                }
                //获取支付宝账户配置信息
                var config = ApiHelper.QueryPayAccount(requestDictionary["app_id"], 1, logger);
                if (string.IsNullOrEmpty(config.Pubkey))
                {
                    logger.Info("支付宝公钥为空,导致签名验证失败");
                }
                //验证签名
                if (AlipaySignature.RSACheckV1(requestDictionary, config.Pubkey, requestDictionary["charset"], requestDictionary["sign_type"], false))
                {
                    if (requestDictionary["app_id"] == config.Appid)
                    {
                        result.error = "success";
                        if (requestDictionary["trade_status"] == "TRADE_SUCCESS")
                        {//支付成功
                            result.state = true;
                            logger.Info("支付成功");
                        }
                        else
                        {
                            result.state = false;
                            logger.Info("未付款交易超时关闭，或支付完成后全额退款");
                        }
                        decimal total_amount = Convert.ToDecimal(requestDictionary["total_amount"]) * 100;
                        ApiHelper.WxPayNotifyHandle(requestDictionary["out_trade_no"],//商户订单号
                                                    result.state,//支付状态
                                                    requestDictionary["trade_no"],//支付宝交易号
                                                    total_amount.ToString(),//本次交易支付的订单金额
                                                    requestDictionary["buyer_logon_id"],//买家支付宝账号
                                                    logger);
                    }
                    else
                    {//通知异常(假通知)
                        //不予处理业务逻辑结果
                        logger.Info("通知异常(假通知)");
                        result.error = "通知异常";
                    }
                }
                else
                {
                    logger.Info("签名验证失败");
                    result.error = "签名验证失败";
                }
            }
            catch (Exception e)
            {
                logger.Info("----------------支付宝支付通知异常---------------------");
                logger.Info("异常错误信息：" + e.Message, e);
                logger.Fatal("异常错误信息：" + e.Message, e);
                result.error = "Systyem Server Error";
            }
            context.Response.Clear();
            context.Response.ContentType = "text/plain";
            context.Response.Write(result.error);
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