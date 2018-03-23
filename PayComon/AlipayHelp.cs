using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Domain;


namespace PayComon
{
   /// <summary>
   /// 支付宝支付相关
   /// </summary>
   public class AlipayHelp
   {

       /// <summary>
       /// 支付宝支付请求
       /// </summary>
       /// <param name="config">支付宝配置信息</param>
       /// <param name="subject">交易标题/订单标题</param>
       /// <param name="out_trade_no">商户订单号</param>
       /// <param name="total_amount">支付金额</param>
       /// <param name="notify_url">异步通知</param>
       /// <param name="return_url">同步通知</param>
       /// <returns></returns>
       public static Message Alipay(AlipayConfig config, string subject, string out_trade_no, string total_amount, string notify_url, string return_url, int time_expire=2)
       {
           Message result = new Message();
           try
           {
               IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do",
                   config.app_id,    //"2017081108144704",//支付宝分配给开发者的应用ID
                   config.merchant_private_key,
                   "json",//仅支持JSON
                   "1.0", //调用的接口版本，固定为：1.0
                   "RSA2",//商户生成签名字符串所使用的签名算法类型，目前支持RSA2和RSA，推荐使用RSA2
                   config.alipay_public_key,
                   "utf-8",
                   false);
               AlipayTradeWapPayRequest request = new AlipayTradeWapPayRequest();

               AlipayTradeWapPayModel model = new AlipayTradeWapPayModel();
               model.OutTradeNo = out_trade_no;
               model.ProductCode = "QUICK_WAP_WAY";
               model.Subject = subject;//商品的标题/交易标题/订单标题/订单关键字等。
               model.TimeoutExpress =string.Format("{0}h",time_expire);//订单失效时间
               model.TotalAmount = total_amount;//订单总金额，单位为元，精确到小数点后两位，取值范围[0.01,100000000]
               request.SetBizModel(model);
               request.SetNotifyUrl(notify_url);//设置异步通知地址
               request.SetReturnUrl(return_url);//设置同步通知地址
               string form = client.pageExecute(request).Body;
               result.state = true;
               result.data = form;
               return result;
           }
           catch (Exception e)
           {
               result.state = false;
               result.error = e.Message;
           }
           return result;
       }
   }
}
