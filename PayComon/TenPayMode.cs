using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayComon
{
    /// <summary>
    /// 微信新H5支付模式
    /// </summary>
    public class TenPayMode
    {
        /// <summary>
        /// 统一下单支付接口
        /// </summary>
        /// <param name="config">公共参数</param>
        /// <param name="body">描述</param>
        /// <param name="out_trade_no">商户系统内部的订单编号</param>
        /// <param name="total_fee">订单总金额，单位为分</param>
        /// <param name="spbill_create_ip">APP和网页支付提交用户端ip，Native支付填调用微信支付API的机器IP.</param>
        /// <param name="notify_url">接收微信支付异步通知回调地址,不能携带参数</param>
        /// <returns></returns>
        public static Message UniteOrder_First(TenPayConfig config, string body, string out_trade_no, string total_fee, string spbill_create_ip, string notify_url, int time_expire=2)
        {
            /************************* 调用微信统一支付API所必需的参数******************************************/
            //公众账号ID      appid                  微信分配的公众账号ID（企业号corpid即为此appId）
            //商户号          mch_id                 微信支付分配的商户号
            //随机字符串      nonce_str              随机字符串，不长于32位。推荐随机数生成算法
            //签名            sign                   签名，详见签名生成算法
            //商品描述        body                   商品或支付单简要描述
            //商户订单号      out_trade_no           商户系统内部的订单号,32个字符内、可包含字母, 其他说明见商户订单号
            //总金额          total_fee              订单总金额，单位为分，详见支付金额
            //终端IP          spbill_create_ip       APP和网页支付提交用户端ip，Native支付填调用微信支付API的机器IP。
            //通知地址        notify_url             接收微信支付异步通知回调地址，通知url必须为直接可访问的url，不能携带参数。
            //交易类型        trade_type             取值如下：JSAPI，NATIVE，APP，详细说明见参数规定
            //商品ID          product_id             trade_type=NATIVE，此参数必传。此id为二维码中包含的商户定义的商品id或者订单号，商户自行定义。
            /************************* 调用微信统一支付API所必需的参数******************************************/
            var Dic = new Dictionary<string, string>();
            Dic.Add("appid", config.appid);
            Dic.Add("mch_id", config.mch_id);
            Dic.Add("nonce_str", TenPayConfig.nonce_str());
            Dic.Add("body", body);
            Dic.Add("out_trade_no", out_trade_no);
            Dic.Add("total_fee", total_fee);
            Dic.Add("spbill_create_ip", spbill_create_ip);
            Dic.Add("notify_url", notify_url);
            Dic.Add("trade_type", "MWEB");//新h5模式
            Dic.Add("time_expire", DateTime.Now.AddHours(time_expire).ToString("yyyyMMddHHmmss"));//订单的失效时间
            string sign = TenPaySign.CreateSign(Dic, config.key);//创建签名
            Dic.Add("sign", sign);

            string RequestData = TenPayHelp.InstallXml(Dic);

            //请求统一下单支付API
            string sUrl = "https://api.mch.weixin.qq.com/pay/unifiedorder";
            string sResult = TenPayHelp.HttpPost(sUrl, RequestData);//调用接口

            var Parameters = TenPayHelp.GetDictionaryFromCDATAXml(sResult);

            Message Msg = new Message();
            if (TenPaySign.CheckSign(Parameters,config.key))
            {//验证签名
                if (Parameters["return_code"] == "SUCCESS")
                {
                    if (Parameters["result_code"] == "SUCCESS")
                    {//统一下单成功
                        Msg.state = true;
                        Msg.data = Parameters["mweb_url"];
                    }
                    else
                    {//统一下单失败
                        Msg.error = Parameters["err_code_des"];//错误信息描述
                    }
                }
                else
                {
                    Msg.error = Parameters["return_msg"];
                }
            }
            else
            {
                Msg.error = sResult;
            }

            return Msg;
        }


        /// <summary>
        /// 查询订单支付状态
        /// </summary>
        /// <param name="config">公共参数</param>
        /// <param name="out_trade_no"></param>
        /// <returns></returns>
        public static Message OrderQuery(TenPayConfig config, string out_trade_no)
        {
             var RequsetParam = new Dictionary<string, string>();
             RequsetParam.Add("appid", config.appid);
             RequsetParam.Add("mch_id", config.mch_id);
             RequsetParam.Add("nonce_str", TenPayConfig.nonce_str());
             RequsetParam.Add("out_trade_no", out_trade_no);
             RequsetParam.Add("sign_type", "MD5");
             string sign = TenPaySign.CreateSign(RequsetParam,config.key);//创建签名
             RequsetParam.Add("sign", sign);

             string RequestData = TenPayHelp.InstallXml(RequsetParam);//组装数据
             //微信支付统一查询API地址
             string sUrl = "https://api.mch.weixin.qq.com/pay/orderquery";
             string sResult = TenPayHelp.HttpPost(sUrl, RequestData);//调用接口

             var ResponeData = TenPayHelp.GetDictionaryFromCDATAXml(sResult);
             Message Msg = new Message();
             if (TenPaySign.CheckSign(ResponeData, config.key))
             {//验证签名
                 if (ResponeData["return_code"] == "SUCCESS")
                 {
                     if (ResponeData["result_code"] == "SUCCESS")
                     {
                         if (ResponeData["trade_state"] == "SUCCESS")
                         {
                             Msg.state = true;
                             Msg.error = "订单支付成功";
                         }
                         else
                         {
                             Msg.error = ResponeData["trade_state_desc"];
                         }
                     }
                     else
                     {
                         Msg.error = ResponeData["err_code_des"];//错误信息描述
                     }
                 }
                 else
                 {
                     Msg.error = ResponeData["return_msg"];
                 }
             }
             else
             {
                 Msg.error = sResult;
             }
             return Msg;
        }


        /// <summary>
        /// 微信申请退款
        /// </summary>
        /// <param name="config">公共参数</param>
        /// <param name="out_trade_no"></param>
        /// <param name="out_refund_no"></param>
        /// <param name="total_fee"></param>
        /// <param name="refund_fee"></param>
        /// <param name="refund_desc"></param>
        /// <returns></returns>
        public static Message PayRefund(TenPayConfig config,string out_trade_no, string out_refund_no, string total_fee, string refund_fee,string refund_desc)
        {
            var RequsetParam = new Dictionary<string, string>();
            RequsetParam.Add("appid", config.appid);
            RequsetParam.Add("mch_id", config.mch_id);
            RequsetParam.Add("nonce_str", TenPayConfig.nonce_str());
            RequsetParam.Add("sign_type", "MD5");
            RequsetParam.Add("out_trade_no", out_trade_no);
            RequsetParam.Add("out_refund_no", out_refund_no);
            RequsetParam.Add("total_fee", total_fee);
            RequsetParam.Add("refund_fee", refund_fee);
            if (!string.IsNullOrEmpty(refund_desc))
            {//退款原因
                RequsetParam.Add("refund_desc", refund_desc);
            }
            string sign = TenPaySign.CreateSign(RequsetParam, config.key);//创建签名
            RequsetParam.Add("sign", sign);

            string RequestData = TenPayHelp.InstallXml(RequsetParam);//组装数据
            //微信退款申请API
            string sUrl = "https://api.mch.weixin.qq.com/secapi/pay/refund";
            string sResult = TenPayHelp.HttpPost(sUrl, RequestData, true, config);//调用接口
            
            var ResponeData = TenPayHelp.GetDictionaryFromCDATAXml(sResult);
            Message result = new Message();
            if (TenPaySign.CheckSign(ResponeData, config.key))
            {//验证签名
                if (ResponeData["return_code"] == "SUCCESS")
                {
                    if (ResponeData["result_code"] == "SUCCESS")
                    {
                        result.state = true;
                        result.error = "退款申请成功";
                    }
                    else
                    {
                        result.error = ResponeData["err_code_des"];//错误信息描述
                    }
                }
                else
                {
                    result.error = ResponeData["return_msg"];
                }
            }
            else
            {
                result.error = sResult;
            }
            result.data = sResult;
            return result;
        }


        /// <summary>
        /// 查询订单退款状态
        /// </summary>
        /// <param name="config">公共参数</param>
        /// <param name="out_refund_no">商户退款单号</param>
        /// <returns></returns>
        public static Message RefundQuery(TenPayConfig config, string out_refund_no)
        {
            var RequsetParam = new Dictionary<string, string>();
            RequsetParam.Add("appid", config.appid);
            RequsetParam.Add("mch_id", config.mch_id);
            RequsetParam.Add("nonce_str", TenPayConfig.nonce_str());
            RequsetParam.Add("out_refund_no", out_refund_no);
            RequsetParam.Add("sign_type", "MD5");
            string sign = TenPaySign.CreateSign(RequsetParam, config.key);//创建签名
            RequsetParam.Add("sign", sign);

            string RequestData = TenPayHelp.InstallXml(RequsetParam);//组装数据
            //微信支付统一查询API地址
            string sUrl = "https://api.mch.weixin.qq.com/pay/refundquery";
            string sResult = TenPayHelp.HttpPost(sUrl, RequestData);//调用接口

            var ResponeData = TenPayHelp.GetDictionaryFromCDATAXml(sResult);
            Message Msg = new Message();
            if (TenPaySign.CheckSign(ResponeData, config.key))
            {//验证签名
                if (ResponeData["return_code"] == "SUCCESS")
                {
                    if (ResponeData["result_code"] == "SUCCESS")
                    {
                        if (ResponeData["refund_status_0"] == "SUCCESS")
                        {
                            Msg.state = true;
                            Msg.error = "退款成功";
                        }
                        else
                        {
                            if (ResponeData["refund_status_0"] == "PROCESSING")
                                Msg.error = "退款处理中";
                            if (ResponeData["refund_status_0"] == "REFUNDCLOSE")
                                Msg.error = "退款关闭";
                            if (ResponeData["refund_status_0"] == "CHANGE")
                                Msg.error = "退款异常";
                        }
                    }
                    else
                    {
                        Msg.error = ResponeData["err_code_des"];//错误信息描述
                    }
                }
                else
                {
                    Msg.error = ResponeData["return_msg"];
                }
            }
            else
            {
                Msg.error = sResult;
            }
            return Msg;
        }
    }
}
