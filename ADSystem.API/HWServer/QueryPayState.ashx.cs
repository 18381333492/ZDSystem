using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PayComon;
using Lib4Net.Logs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Domain;
using Aop.Api.Response;

namespace ADSystem.API.HWServer
{
    /// <summary>
    /// 查询微信支付状态
    /// </summary>
    public class QueryPayState : IHttpHandler
    {
        private static readonly ILogger logger = LoggerManager.Instance.GetLogger("HWQueryPayState");
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string orderno = context.Request.Form["orderno"];//查询的订单号
                string appid = context.Request.Form["appid"];
                string account_type = context.Request.Form["account_type"];
                if (!string.IsNullOrEmpty(orderno))
                {
                    //查询订单支付状态
                    //获取账户信息
                    var type = Convert.ToInt32(account_type);
                    QueryAccountResult accountResult = ApiHelper.QueryPayAccount(appid, type, logger);
                    if (type == 2)
                    {//查询微信支付状态

                        string wxPayType = System.Configuration.ConfigurationManager.AppSettings["wxPayMtd"];
                        wxPayType = string.IsNullOrEmpty(wxPayType) ? "weixin" : wxPayType;
                        Message result = null;
                        var config = new TenPayConfig()
                        {
                            appid = accountResult.Appid,
                            key = accountResult.Pubkey,
                            mch_id = accountResult.Mchid
                        };
                        if (wxPayType.Equals("weixin"))
                            result = TenPayMode.OrderQuery(config, orderno);
                        else if (wxPayType.Equals("pingan"))
                            result = PingAnPay.OrderQuery(config, orderno);
                        else
                        {
                            logger.Info("微信查询支付订单的wxPayType参数错误");//日志输出
                            context.Response.ContentType = "text/plain";
                            context.Response.Write(JsonConvert.SerializeObject(new { success = false, info = "查询出错" }));
                            return;
                        }

                        if (result.state)
                        {//支付成功
                            logger.Info("微信查询支付订单的返回参数" + JsonConvert.SerializeObject(result));//日志输出
                            context.Response.ContentType = "text/plain";
                            context.Response.Write(JsonConvert.SerializeObject(new { success = true, info = "支付成功" }));
                        }
                        else
                        {
                            logger.Info("微信查询支付订单的返回参数" + JsonConvert.SerializeObject(result));//日志输出
                            context.Response.ContentType = "text/plain";
                            context.Response.Write(JsonConvert.SerializeObject(new { success = false, info = "支付失败" }));
                        }
                    }
                    else
                    {
                        //查询支付宝支付状态
                           IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do",
                           accountResult.Appid,    //"2017081108144704",//支付宝分配给开发者的应用ID
                           accountResult.Prikey,
                           "json",//仅支持JSON
                           "1.0", //调用的接口版本，固定为：1.0
                           "RSA2",//商户生成签名字符串所使用的签名算法类型，目前支持RSA2和RSA，推荐使用RSA2
                           accountResult.Pubkey,
                           "utf-8",
                           false);
                        AlipayTradeQueryRequest request = new AlipayTradeQueryRequest();
                        AlipayTradeQueryModel model = new AlipayTradeQueryModel();
                        model.OutTradeNo = orderno;
                        request.SetBizModel(model);
                        AlipayTradeQueryResponse response = client.Execute(request);
                        //获取响应返回的数据
                        if (!response.IsError)
                        {
                            if (response.TradeStatus == "TRADE_SUCCESS")
                            {
                                context.Response.ContentType = "text/plain";
                                context.Response.Write(JsonConvert.SerializeObject(new { success = true, info = "支付成功" }));
                            }
                            else
                            {
                                logger.Info("支付宝查询支付订单的返回参数" + response.Body);//日志输出
                                context.Response.ContentType = "text/plain";
                                context.Response.Write(JsonConvert.SerializeObject(new { success = false, info = "支付失败" }));
                            }
                        }
                        else
                        {//错误信息
                            logger.Info("支付宝查询支付订单的返回参数：" + response.Code+response.SubMsg);//日志输出
                            context.Response.ContentType = "text/plain";
                            context.Response.Write(JsonConvert.SerializeObject(new { success = false, info = "支付失败" }));
                        }

                    }
                }
                else
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write(JsonConvert.SerializeObject(new { success = false, info = "缺少参数" }));
                }
            }
            catch (Exception e)
            {
                logger.Info(e.Message);
                logger.Fatal(e.Message, e);
                var res = new { success = false, info = "服务错误" };
                context.Response.ContentType = "text/plain";
                context.Response.Write(JsonConvert.SerializeObject(res));
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