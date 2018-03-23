using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayComon
{
    /// <summary>
    /// 微信支付相关的一些配置参数
    /// </summary>
    public class TenPayConfig
    {
        /// <summary>
        /// 微信公众号唯一标识/支付宝分配给开发者的应用ID
        /// </summary>
        public string appid
        {
            get;
            set;
        }
  
        /// <summary>
        /// 商户号(微信支付分配的商户号)
        /// </summary>
        public string mch_id
        {
            get;
            set;
        }

        /// <summary>
        /// 密钥(key设置路径：微信商户平台(pay.weixin.qq.com)-->账户设置-->API安全-->密钥设置)
        /// </summary>
        public  string key
        {
            get;
            set;
        }

        /// <summary>
        /// 商户私钥（支付宝专用）
        /// </summary>
        public string merchant_private_key
        {
            get;
            set;
        }


        /// <summary>
        /// 证书的路径(申请退款的时候需要)
        /// </summary>
        public string SSLCERT_PATH
        {
            get;
            set;
        }

        /// <summary>
        /// 获取随机字符串
        /// </summary>
        /// <returns></returns>
        public static string nonce_str()
        {
            // 生成随机数算法
            //微信支付API接口协议中包含字段nonce_str，主要保证签名不可预测。
            //我们推荐生成随机数算法如下：调用随机数函数生成，将得到的值转换为字符串。
            Random Ran = new Random();
            string nonce_str = Ran.Next(11111111, 99999999).ToString() + "CNMB";
            return TenPayHelp.MD5(nonce_str);
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        public static string time_stamp()
        {
            //时间戳
            //标准北京时间，时区为东八区，自1970年1月1日 0点0分0秒以来的秒数。
            //注意：部分系统取到的值为毫秒级，需要转换成秒(10位数字).
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
    }

    /// <summary>
    /// 统一下单的消息model
    /// </summary>
    public class Message
    {
        public bool state = false;
        public string data;
        public string error;
        public string paramsData=string.Empty;
    }

}
