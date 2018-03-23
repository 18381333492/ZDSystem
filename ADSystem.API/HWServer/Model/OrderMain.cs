using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADSystem.API.HWServer.Model
{
    public class OrderMain
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 订单时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 充值类型
        /// </summary>
        public int? CardType { get; set; }

        /// <summary>
        /// 支付类型
        /// </summary>
        public int? PayType { get; set; }


        /// <summary>
        /// 用户支付金额
        /// </summary>
        public decimal? UserPayed { get; set; }


        /// <summary>
        /// 充值号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public int? Status { get; set; }


    }
}