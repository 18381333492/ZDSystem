using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace PayComon
{
   
   public class PayRequest
   {
       private static PayRequest instance = null;

       public static PayRequest Instance
       {
           get
           {
               if (instance == null)
                   instance = new PayRequest();
               return instance;
           }
       }

       /// <summary>
       /// 支付下单
       /// </summary>
       /// <param name="config">公共参数</param>
       /// <param name="sClientIp">客户端的原始Ip(不能是代理IP必须是真实的IP)</param>
       /// <param name="sOrderNo">订单号</param>
       /// <param name="sDescription">订单描述,展示在支付页面上的文字描述</param>
       /// <param name="total_fee">支付金额(单位是分)</param>
       /// <param name="notify_url">异步通知地址</param>
       /// <param name="return_url">同步通知地址</param>
       /// <param name="iPayType">支付方式 1-支付宝 ;2-微信</param>
       /// <returns></returns>
       public Message bookOrder(TenPayConfig config, string sClientIp, string sOrderNo, string sDescription, decimal total_fee, string notify_url, string return_url, int iPayType,int time_expire=2)
       {
           if (iPayType == 1)
           {//支付宝支付
               var res = AlipayHelp.Alipay(new AlipayConfig()
                 {
                     app_id = config.appid,
                     alipay_public_key = config.key,
                     merchant_private_key = config.merchant_private_key
                 }, sDescription, sOrderNo, total_fee.ToString(), notify_url, return_url, time_expire);
               return res;
           }
           else
           {
               if (string.IsNullOrEmpty(sClientIp))
               {
                   return new Message()
                   {
                       state = false,
                       error = "用户真实IP必传"
                   };
               }
               string wxPayType = System.Configuration.ConfigurationManager.AppSettings["wxPayMtd"];
               total_fee = total_fee * 100;
               wxPayType = string.IsNullOrEmpty(wxPayType) ? "weixin" : wxPayType;
               Message res = null;
               if (wxPayType.Equals("weixin"))
                   res=TenPayMode.UniteOrder_First(config, sDescription, sOrderNo, Convert.ToInt32(total_fee).ToString(), sClientIp, notify_url, time_expire);
               else if (wxPayType.Equals("pingan"))
                   res = PingAnPay.UniteOrder_First(config, sDescription, sOrderNo, Convert.ToInt32(total_fee).ToString(), sClientIp, notify_url, time_expire);
               else
               {
                  return res=new Message()
                   {
                       state = false,
                       error = "微信支付类型错误."
                   };
               }
               if (res.state)
               {//对url进行urlencode处理
                   string redirect_url = return_url;
                   if (redirect_url != null)
                   {
                       res.data = string.Format("{0}&redirect_url={1}", res.data, TenPayHelp.UrlEncode(redirect_url));
                   }
               }
               return res;
           }
       }


       /// <summary>
       /// 根据订单号查询微信支付状态
       /// </summary>
       /// <param name="config">公共参数</param>
       /// <param name="sOrderNo"></param>
       /// <returns></returns>
       public Message QueryPayState(TenPayConfig config,string sOrderNo)
       {
           var res = TenPayMode.OrderQuery(config,sOrderNo);
           return res;
       }

       
       /// <summary>
       /// 微信申请退款
       /// </summary>
       /// <param name="sOrderNo">订单号</param>
       /// <param name="sRefundNo">退款订单号</param>
       /// <param name="total_fee">支付的金额</param>
       /// <param name="refund_fee">申请退款的金额</param>
       /// <param name="refund_desc">退款描述</param>
       /// <returns></returns>
       public Message PayRefund(TenPayConfig config, string sOrderNo, string sRefundNo, int total_fee, int refund_fee, string refund_desc)
       {

           string wxPayType = System.Configuration.ConfigurationManager.AppSettings["wxPayMtd"];
           wxPayType = string.IsNullOrEmpty(wxPayType) ? "weixin" : wxPayType;
           Message result = null;
           if (wxPayType.Equals("weixin"))
               result = TenPayMode.PayRefund(config,sOrderNo, sRefundNo, total_fee.ToString(), refund_fee.ToString(),refund_desc);
           else if (wxPayType.Equals("pingan"))
               result = PingAnPay.PayRefund(config,sOrderNo,sRefundNo,total_fee,refund_fee,refund_desc);
           else
           {
               result = new Message() { state = false, error = "支付方式(wxPayType)参数错误" };
           }
           return result;
       }


       /// <summary>
       /// 获取微信支付异步通知参数和签名验证结果
       /// </summary>
       /// <param name="request"></param>
       /// <returns></returns>
       public Message GetPayNotityResult(string ApiKey, Dictionary<string, string> Parameters)
       {
           var result = new Message();
           if (TenPaySign.CheckSign(Parameters, ApiKey))
           {//验证签名
               if (Parameters["return_code"] == "SUCCESS")
               {
                   if (Parameters["result_code"] == "SUCCESS")
                   {//支付成功
                       result.state = true;
                       result.data = Parameters["out_trade_no"];//商户订单号
                       result.error = "支付成功";          
                   }
                   else
                   {
                       result.error = Parameters["err_code_des"];
                   }
               }
               else
               {
                   result.error = Parameters["return_msg"];
               }
           }
           else
           {
               result.error = "签名验证失败";
           }
           return result;
       }


       /// <summary>
       /// 获取微信退款异步通知结果
       /// 接受退款通知不需要验证签名
       /// </summary>
       /// <param name="request"></param>
       /// <returns></returns>
       public Message GetPayRefundNotifyResult(string ApiKey, Dictionary<string, string> Parameters)
       {
           var result = new Message();
           if (Parameters["return_code"] == "SUCCESS")
           {
               var strA = Parameters["req_info"];//得到加密字符串
               string key = TenPayHelp.MD5(ApiKey).ToLower();
               var strB = TenPayHelp.AESDecrypt(strA, key);//AES解密
               Parameters = Parameters.Union(TenPayHelp.GetDictionaryFromCDATAXml(strB)).ToDictionary(pi => pi.Key, pi => pi.Value);
               if (Parameters["refund_status"] == "SUCCESS")
               {//支付成功
                   result.state = true;
                   result.error = "退款成功";
               }
               else
               {
                   if (Parameters["refund_status"] == "CHANGE")
                       result.error = "退款异常";
                   if (Parameters["refund_status"] == "REFUNDCLOSE")
                       result.error = "退款关闭";
               }
               result.data = string.Format("{0},{1}", Parameters["out_refund_no"], Parameters["refund_id"]);
               result.paramsData = strB;
           }
           else
           {
               result.error = Parameters["return_msg"];
           }
           return result;
       }
   }
}
