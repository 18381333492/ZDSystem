using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADSystem.API.HWServer.Model
{
    /// <summary>
    /// 华为订单实体model
    /// </summary>
    public class HWOrder
    {

        /// <summary>
        /// 订单编号
        /// </summary>
        public string orderno { get; set; }

        /// <summary>
        /// 用户账号
        /// </summary>
        public string account {get; set;}

        /// <summary>
        /// 支付方式 1支付宝 2微信 4-龙支付
        /// </summary>
        public decimal pay_type { get; set; }


        /// <summary>
        /// 充值类型 1-话费 2-流量
        /// </summary>
        public decimal card_type { get; set; }

        /// <summary>
        /// 流量类型
        /// </summary>
        public string flow_type { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string product_name { get; set; }
      

        /// <summary>
        /// 充值的手机号
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        public string product_id  { get; set; }

        /// <summary>
        /// 用户的真实IP
        /// </summary>
        public string user_ip { get; set; }

        /// <summary>
        /// 产品售价
        /// </summary>
        public decimal price { get; set; }

        /// <summary>
        /// 产品面值
        /// </summary>
        public decimal face { get; set; }

        /// <summary>
        /// 下游渠道编号
        /// </summary>
        public string down_channel_no { get; set; }

        public decimal businessType
        {
            get;
            set;
        }

        /// <summary>
        /// 慢充类型
        /// </summary>
        public int recharge_mode
        {
            get;
            set;
        }

        /// <summary>
        /// 终端类型 1直充App 2直充H5 3慢充App 4慢充H5
        /// </summary>
        public int clientType
        {
            get;
            set;
        }
    }
}