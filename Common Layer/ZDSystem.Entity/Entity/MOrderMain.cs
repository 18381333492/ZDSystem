using System;
using System.Collections.Generic;
using System.Text;
using Lib4Net.Comm;
using Lib4Net.ORM;


namespace ZDSystem.Entity
{

    ///<summary>
    ///实体：订单主表
    ///</summary>
    [ODXmlConfig(EntityConfigType.Xml, "", "~/config/econfig/OrderMain.xml")]
    public class MOrderMain : EntityBase
    {

        private string _OrderNo;
        private string _PartnerOrderNo;
        private int? _PayType;
        private string _ProductName;
        private string _Mobile;
        private string _Account;
        private string _ProductId;
        private decimal? _Face;
        private decimal? _Price;
        private DateTime? _ZdOrderTime;
        private int? _CardType;
        private string _FlowType;
        private int? _PayStatus;
        private int? _OrderStatus;
        private int? _RefundState;
        private DateTime? _CreateTime;
        private DateTime? _RefundTime;
        private DateTime? _PayTime;
        private DateTime? _FinishTime;
        private decimal? _SucFace;
        private int? _BusinessType;
        private decimal? _Couponprice;
        private string _Couponid;
        private string _Couponch;
        private decimal? _ServiceFee;



        ///<summary>
        ///订单号
        ///</summary>
        public string OrderNo { get { return _OrderNo; } set { _OrderNo = value; } }


        ///<summary>
        ///中大订单号
        ///</summary>
        public string PartnerOrderNo { get { return _PartnerOrderNo; } set { _PartnerOrderNo = value; } }


        ///<summary>
        ///支付类型1.支付宝2微信
        ///</summary>
        public int? PayType { get { return _PayType; } set { _PayType = value; } }


        ///<summary>
        ///产品名称
        ///</summary>
        public string ProductName { get { return _ProductName; } set { _ProductName = value; } }


        ///<summary>
        ///充值账号
        ///</summary>
        public string Mobile { get { return _Mobile; } set { _Mobile = value; } }


        ///<summary>
        ///中大用户名
        ///</summary>
        public string Account { get { return _Account; } set { _Account = value; } }


        ///<summary>
        ///产品编号
        ///</summary>
        public string ProductId { get { return _ProductId; } set { _ProductId = value; } }


        ///<summary>
        ///充值面值
        ///</summary>
        public decimal? Face { get { return _Face; } set { _Face = value; } }


        ///<summary>
        ///支付金额
        ///</summary>
        public decimal? Price { get { return _Price; } set { _Price = value; } }


        ///<summary>
        ///中大订单时间
        ///</summary>
        public DateTime? ZdOrderTime { get { return _ZdOrderTime; } set { _ZdOrderTime = value; } }


        ///<summary>
        ///充值类型1-话费2-流量
        ///</summary>
        public int? CardType { get { return _CardType; } set { _CardType = value; } }


        ///<summary>
        ///流量类型
        ///</summary>
        public string FlowType { get { return _FlowType; } set { _FlowType = value; } }


        ///<summary>
        ///支付状态:20-等待支付30-正在支付90-失败0-成功
        ///</summary>
        public int? PayStatus { get { return _PayStatus; } set { _PayStatus = value; } }


        ///<summary>
        ///订单状态:20-等待充值30-正在充值90-失败0-成功
        ///</summary>
        public int? OrderStatus { get { return _OrderStatus; } set { _OrderStatus = value; } }


        ///<summary>
        ///退款状态10-无需20-等待退款30-正在退款90-失败0-成功
        ///</summary>
        public int? RefundState { get { return _RefundState; } set { _RefundState = value; } }


        ///<summary>
        ///创建时间
        ///</summary>
        public DateTime? CreateTime { get { return _CreateTime; } set { _CreateTime = value; } }


        ///<summary>
        ///退款时间
        ///</summary>
        public DateTime? RefundTime { get { return _RefundTime; } set { _RefundTime = value; } }


        ///<summary>
        ///支付完成时间
        ///</summary>
        public DateTime? PayTime { get { return _PayTime; } set { _PayTime = value; } }


        ///<summary>
        ///完成时间
        ///</summary>
        public DateTime? FinishTime { get { return _FinishTime; } set { _FinishTime = value; } }
        /// <summary>
        /// 成功面值
        /// </summary>
        public decimal? SucFace
        {
            get { return _SucFace; }
            set { _SucFace = value; }
        }
        /// <summary>
        /// 业务类型
        /// </summary>
        public int? BusinessType
        {
            get { return _BusinessType; }
            set { _BusinessType = value; }
        }
        /// <summary>
        /// 优惠券金额
        /// </summary>
        public decimal? Couponprice
        {
            get { return _Couponprice; }
            set { _Couponprice = value; }
        }
        /// <summary>
        /// 优惠券ID
        /// </summary>
        public string Couponid
        {
            get { return _Couponid; }
            set { _Couponid = value; }
        }
        /// <summary>
        /// 优惠券渠道
        /// </summary>
        public string Couponch
        {
            get { return _Couponch; }
            set { _Couponch = value; }
        }
        /// <summary>
        /// 支付手续费用
        /// </summary>
        public decimal? ServiceFee
        {
            get { return _ServiceFee; }
            set { _ServiceFee = value; }
        }

        /// <summary>
        /// 用户支付金额
        /// </summary>
        public decimal? UserPayed { get; set; }

        /// <summary>
        /// 支付结果回调状态
        /// </summary>
        public int? PayNotifyState { get; set; }

        /// <summary>
        /// 订单状态回调状态
        /// </summary>
        public int? OrderNotifyState { get; set; }

        /// <summary>
        /// 支付结果回调时间
        /// </summary>
        public DateTime? PayNotifyTime { get; set; }
        /// <summary>
        /// 订单结果回调时间
        /// </summary>
        public DateTime? OrderNotifyTime { get; set; }
        /// <summary>
        /// 收款账户编号
        /// </summary>
        public int? ReceiptAccountId { get; set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMsg { get; set; }
        /// <summary>
        /// 真实ip
        /// </summary>
        public string UserIp { get; set; }
        /// <summary>
        /// 支付方流水号
        /// </summary>
        public string PayFlowOrder { get; set; }
        /// <summary>
        /// 支付用户编号
        /// </summary>
        public string PayUserId { get; set; }
        /// <summary>
        /// 实际充值成本
        /// </summary>
        public decimal? CostPrice { get; set; }
        /// <summary>
        /// 是否充值话费流量
        /// </summary>
        public int? NeedRecharge { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public int? ManualStatus { get; set; }

        /// <summary>
        /// 18 系统成本金额
        /// </summary>
        public decimal? EsalesCostPrice { get; set; }

        /// <summary>
        /// 下游渠道编号
        /// </summary>
        public int? DownChannelNo { get; set; }
    }
}