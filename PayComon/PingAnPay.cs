using System.Configuration;
using Newtonsoft.Json.Linq;


namespace PayComon
{
    public class PingAnPay
    {
        static string openId = ConfigurationManager.AppSettings["open_id"];
        static string openKey = ConfigurationManager.AppSettings["open_key"];
        static string wapUrl = ConfigurationManager.AppSettings["wap_url"];
        static string wapName = ConfigurationManager.AppSettings["wap_name"];

        public static Message UniteOrder_First(TenPayConfig config, string body, string out_trade_no, string total_fee,
            string spbill_create_ip, string notify_url, int time_expire = 2)
        {
            notify_url = "http://hwsh.qianxingniwo.com:9211/WxPayNotify.ashx";
            Message payMsg = new Message();
            string scene = "{\"h5_info\": {\"type\":\"Wap\",\"wap_url\": \"" + wapUrl + "\",\"wap_name\": \"" + wapName + "\"}} ";
            JObject paramJObject = new JObject
            {
                new JProperty("out_no", out_trade_no),
                 new JProperty("trade_amount", total_fee),
                 new JProperty("original_amount", total_fee),
                 new JProperty("trade_type", "MWEB"),
                new JProperty("pmt_tag", "WeixinOL"),
                new JProperty("notify_url", notify_url),
                new JProperty("spbill_create_ip", spbill_create_ip),
                new JProperty("scene_info", scene)
            };
            if (string.IsNullOrEmpty(openId) || string.IsNullOrEmpty(openKey))
            {
                payMsg.state = false;
                payMsg.data = payMsg.error = "openid或openKey没有配置";
                return payMsg;
            }
            string bizData = PingAnPayHelp.MakeDataJson(paramJObject.ToString(), openKey);
            string postData = PingAnPayHelp.SignAndPostData(bizData, openId, openKey);
            //请求统一下单支付API
            string requestUrl = ConfigurationManager.AppSettings["pingan_interface"] + "payorder";
            string responseStr = PingAnPayHelp.Post(requestUrl, postData);//调用接口
            JObject jResult = JObject.Parse(responseStr);

            //查看返回结果
            if (!jResult.SelectToken("errcode").ToString().Equals("0"))
            {

                payMsg.state = false;
                payMsg.data = payMsg.error = jResult.SelectToken("msg").ToString();
                return payMsg;
            }

            //验签
            if (!PingAnPayHelp.CheckSign(jResult.SelectToken("data").ToString(), jResult.SelectToken("errcode").ToString(),
                openKey, jResult.SelectToken("msg").ToString(),
                jResult.SelectToken("sign").ToString(), jResult.SelectToken("timestamp").ToString()))
            {
                payMsg.state = false;
                payMsg.data = payMsg.error = "签名不一致!";
                return payMsg;
            }

            string dataStr = PingAnPayHelp.GetDataJson(jResult.SelectToken("data").ToString(), openKey);
            JObject jData = JObject.Parse(dataStr);

            //验证主要参数
            if (!total_fee.Equals(jData.SelectToken("trade_amount").ToString()))
            {
                payMsg.state = false;
                payMsg.data = payMsg.error = "请求支付金额与实际支付金额不一致!";
                return payMsg;
            }
            string tradeResultStr = jData.SelectToken("trade_result").ToString();
            JObject tradeResultJo = JObject.Parse(tradeResultStr);
            if (!"SUCCESS".Equals(tradeResultJo.SelectToken("return_code").ToString()))
            {
                payMsg.state = false;
                payMsg.data = payMsg.error = "支付请求失败!" + tradeResultJo.SelectToken("return_msg");
                return payMsg;
            }
            payMsg.data = jData.SelectToken("mweb_url").ToString();
            payMsg.state = true;
            payMsg.error = "支付请求成功!";
            return payMsg;
        }

        /// <summary>
        /// 订单查询
        /// </summary>
        /// <param name="config"></param>
        /// <param name="out_trade_no">自己系统订单号</param>
        /// <returns></returns>
        public static Message OrderQuery(TenPayConfig config, string out_trade_no)
        {
            Message payMsg = new Message();
            JObject paramJObject = new JObject
            {
                new JProperty("out_no", out_trade_no)
            };
            string bizData = PingAnPayHelp.MakeDataJson(paramJObject.ToString(), openKey);
            string postData = PingAnPayHelp.SignAndPostData(bizData, openId, openKey);
            //请求统一下单支付API
            string requestUrl = ConfigurationManager.AppSettings["pingan_interface"] + "paystatus";
            string responseStr = PingAnPayHelp.Post(requestUrl, postData);//调用接口
            JObject jResult = JObject.Parse(responseStr);
            //查看返回结果
            if (!jResult.SelectToken("errcode").ToString().Equals("0"))
            {
                payMsg.state = false;
                payMsg.data = payMsg.error = jResult.SelectToken("msg").ToString();
                return payMsg;
            }
            //验签
            if (!PingAnPayHelp.CheckSign(jResult.SelectToken("data").ToString(), jResult.SelectToken("errcode").ToString(),
                openKey, jResult.SelectToken("msg").ToString(),
                jResult.SelectToken("sign").ToString(), jResult.SelectToken("timestamp").ToString()))
            {
                payMsg.state = false;
                payMsg.data = payMsg.error = "签名不一致!";
                return payMsg;
            }

            string dataStr = PingAnPayHelp.GetDataJson(jResult.SelectToken("data").ToString(), openKey);
            JObject jData = JObject.Parse(dataStr);

            //查询订单号
            if (!out_trade_no.Equals((jData.SelectToken("out_no").ToString())))
            {
                payMsg.state = false;
                payMsg.data = payMsg.error = "查询订单号与返回订单号不一致!";
                return payMsg;
            }

            string status = jData.SelectToken("status").ToString();
            switch (status)
            {
                case "1":
                    payMsg.state = true;
                    payMsg.data = payMsg.error = "支付成功!";
                    return payMsg;
                case "2":
                    payMsg.state = false;
                    payMsg.data = payMsg.error = "待支付!";
                    return payMsg;
                case "4":
                    payMsg.state = false;
                    payMsg.data = payMsg.error = "已取消!";
                    return payMsg;
                case "9":
                    payMsg.state = false;
                    payMsg.data = payMsg.error = "等待用户输入密码确认!";
                    return payMsg;
                default:
                    payMsg.state = false;
                    payMsg.data = payMsg.error = "状态未知!";
                    return payMsg;
            }
        }

        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="config"></param>
        /// <param name="body"></param>
        /// <param name="out_trade_no"></param>
        /// <param name="total_fee"></param>
        /// <returns></returns>
        public static Message PayRefund(TenPayConfig config, string out_trade_no, string out_refund_no, int total_fee, int refund_fee, string refund_desc)
        {
            Message payMsg = new Message();
            string refundPwd = ConfigurationManager.AppSettings["pingan_refund_pwd"];
            JObject paramJObject = new JObject
            {
                new JProperty("out_no", out_trade_no),
                new JProperty("refund_out_no", out_refund_no),
                new JProperty("refund_ord_name", refund_desc),
                new JProperty("refund_amount", refund_fee),
                new JProperty("shop_pass", Utilities.SHA1(refundPwd).ToLower())
            };
            string bizData = PingAnPayHelp.MakeDataJson(paramJObject.ToString(), openKey);

            string postData = PingAnPayHelp.RefundSignAndPostData(bizData, openId, openKey);
            //请求统一下单支付API
            string requestUrl = ConfigurationManager.AppSettings["pingan_interface"] + "payrefund";
            string responseStr = PingAnPayHelp.Post(requestUrl, postData);//调用接口
            JObject jResult = JObject.Parse(responseStr);

            //查看返回结果
            if (!jResult.SelectToken("errcode").ToString().Equals("0"))
            {
                payMsg.state = false;
                payMsg.data = payMsg.error = jResult.SelectToken("msg").ToString();
                return payMsg;
            }
            //验签
            if (!PingAnPayHelp.CheckSign(jResult.SelectToken("data").ToString(), jResult.SelectToken("errcode").ToString(),
                openKey, jResult.SelectToken("msg").ToString(),
                jResult.SelectToken("sign").ToString(), jResult.SelectToken("timestamp").ToString()))
            {
                payMsg.state = false;
                payMsg.data = payMsg.error = "签名不一致!";
                return payMsg;
            }


            string dataStr = PingAnPayHelp.GetDataJson(jResult.SelectToken("data").ToString(), openKey);
            JObject jData = JObject.Parse(dataStr);

            //验证主要参数
            if (total_fee != (int)jData.SelectToken("trade_amount"))
            {
                payMsg.state = false;
                payMsg.data = payMsg.error = "请求退款金额与实际退款金额不一致!";
                return payMsg;
            }
            if ("1".Equals(jData.SelectToken("status").ToString()))
            {
                payMsg.state = true;
                payMsg.data = PingAnPayHelp.MakeXmlRep(jResult.SelectToken("errcode").ToString(), jData.SelectToken("status").ToString(),
                    jData.SelectToken("ord_no").ToString(), jResult.SelectToken("msg").ToString(),
                    "");
                payMsg.error = "退款请求成功!";
                return payMsg;
            }
            payMsg.state = false;
            payMsg.data = payMsg.error = "退款失败!";
            return payMsg;
        }

        /// <summary>
        /// 退款查询
        /// </summary>
        /// <param name="config"></param>
        /// <param name="body"></param>
        /// <param name="outTradeNo"></param>
        /// <param name="outRefundNo"></param>
        /// <param name="totalFee"></param>
        /// <returns></returns>
        public static Message PayRefundQuery(TenPayConfig config, string outRefundNo)
        {
            Message payMsg = new Message();
            JObject paramJObject = new JObject
            {
                new JProperty("refund_out_no", outRefundNo)
            };
            string bizData = PingAnPayHelp.MakeDataJson(paramJObject.ToString(), openKey);
            string postData = PingAnPayHelp.SignAndPostData(bizData, openId, openKey);
            //请求统一下单支付API
            string requestUrl = ConfigurationManager.AppSettings["pingan_interface"] + "payrefundquery";
            string responseStr = PingAnPayHelp.Post(requestUrl, postData);//调用接口
            JObject jResult = JObject.Parse(responseStr);
            //查看返回结果
            if (!jResult.SelectToken("errcode").ToString().Equals("0"))
            {
                payMsg.state = false;
                payMsg.data = payMsg.error = jResult.SelectToken("msg").ToString();
                return payMsg;
            }
            //验签
            if (!PingAnPayHelp.CheckSign(jResult.SelectToken("data").ToString(), jResult.SelectToken("errcode").ToString(),
                openKey, jResult.SelectToken("msg").ToString(),
                jResult.SelectToken("sign").ToString(), jResult.SelectToken("timestamp").ToString()))
            {
                payMsg.state = false;
                payMsg.data = payMsg.error = "签名不一致!";
                return payMsg;
            }



            string dataStr = PingAnPayHelp.GetDataJson(jResult.SelectToken("data").ToString(), openKey);
            JObject jData = JObject.Parse(dataStr);
            if ("1".Equals(jData.SelectToken("status").ToString()))
            {
                payMsg.state = true;
                payMsg.data = PingAnPayHelp.MakeXmlRep(jResult.SelectToken("errcode").ToString(), jData.SelectToken("status").ToString(), "", jResult.SelectToken("msg").ToString(),
                   "");
                payMsg.error = "退款成功!";
                return payMsg;
            }
            payMsg.state = false;
            payMsg.data = payMsg.error = "退款失败!";
            return payMsg;
        }
    }
}
