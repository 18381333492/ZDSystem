using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lib4Net.ORM;
using Lib4Net.Comm;
namespace ZDSystem.Entity
{

    ///<summary>
    ///实体：使用过的中大优惠券
    ///</summary>
    [ODXmlConfig(EntityConfigType.Xml, "", "~/config/econfig/ZdCouponUsed.xml")]
    public class MZdCouponUsed : EntityBase
    {
        private string _CouponId;      
        private string _CouponChannel;       
        private decimal? _CouponPrice;       
        private string _OrderNo;      
        private DateTime? _UseTime;
        private string _DownOrderNo;
        /// <summary>
        /// 优惠券id
        /// </summary>
        public string CouponId
        {
            get { return _CouponId; }
            set { _CouponId = value; }
        }
        /// <summary>
        /// 优惠渠道
        /// </summary>
        public string CouponChannel
        {
            get { return _CouponChannel; }
            set { _CouponChannel = value; }
        }
        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal? CouponPrice
        {
            get { return _CouponPrice; }
            set { _CouponPrice = value; }
        }
        /// <summary>
        /// 使用订单编号
        /// </summary>
        public string OrderNo
        {
            get { return _OrderNo; }
            set { _OrderNo = value; }
        }
        /// <summary>
        /// 使用时间
        /// </summary>
        public DateTime? UseTime
        {
            get { return _UseTime; }
            set { _UseTime = value; }
        }
        /// <summary>
        /// 下游订单号
        /// </summary>
        public string DownOrderNo
        {
            get { return _DownOrderNo; }
            set { _DownOrderNo = value; }
        }      
    }
}
