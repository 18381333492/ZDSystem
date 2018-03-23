using System;
using System.Collections.Generic;
using System.Text;
using Lib4Net.Comm;
using Lib4Net.ORM;


namespace ZDSystem.Entity
{

      ///<summary>
      ///实体：每日账户快照
      ///</summary>
     [ODXmlConfig(EntityConfigType.Xml,"","~/config/econfig/DailyAccountSnapshot.xml")]
    public class MDailyAccountSnapshot:EntityBase
    {    

                private DateTime? _CreateTime;
        private decimal? _DownChannelNo;
        private decimal? _CardType;
        private decimal? _PayType;
        private decimal? _BusinessType;
        private decimal? _ProductFace;
        private decimal? _AllPrice;
        private decimal? _AllOrderCntr;
        private decimal? _SuccessMoney;
        private decimal? _SuccessOrderCntr;
        private decimal? _SuccessCoupon;
        private decimal? _ServiceFee;
        private decimal? _RefundMoney;
        private decimal? _RefundOrderCntr;
        private decimal? _LossService;


                 ///<summary>
        ///快照时间
        ///</summary>
        public DateTime? CreateTime{get { return _CreateTime; }set { _CreateTime= value; }}


        ///<summary>
        ///下游渠道编号
        ///</summary>
        public decimal? DownChannelNo{get { return _DownChannelNo; }set { _DownChannelNo= value; }}


        ///<summary>
        ///业务类型
        ///</summary>
        public decimal? CardType{get { return _CardType; }set { _CardType= value; }}


        ///<summary>
        ///支付方式
        ///</summary>
        public decimal? PayType{get { return _PayType; }set { _PayType= value; }}


        ///<summary>
        ///业务类型
        ///</summary>
        public decimal? BusinessType{get { return _BusinessType; }set { _BusinessType= value; }}


        ///<summary>
        ///订单面值
        ///</summary>
        public decimal? ProductFace{get { return _ProductFace; }set { _ProductFace= value; }}


        ///<summary>
        ///订单总金额
        ///</summary>
        public decimal? AllPrice{get { return _AllPrice; }set { _AllPrice= value; }}


        ///<summary>
        ///订单总笔数
        ///</summary>
        public decimal? AllOrderCntr{get { return _AllOrderCntr; }set { _AllOrderCntr= value; }}


        ///<summary>
        ///成功支付金额
        ///</summary>
        public decimal? SuccessMoney{get { return _SuccessMoney; }set { _SuccessMoney= value; }}


        ///<summary>
        ///成功支付笔数
        ///</summary>
        public decimal? SuccessOrderCntr{get { return _SuccessOrderCntr; }set { _SuccessOrderCntr= value; }}


        ///<summary>
        ///成功优惠卷金额
        ///</summary>
        public decimal? SuccessCoupon{get { return _SuccessCoupon; }set { _SuccessCoupon= value; }}


        ///<summary>
        ///服务费
        ///</summary>
        public decimal? ServiceFee{get { return _ServiceFee; }set { _ServiceFee= value; }}


        ///<summary>
        ///退款金额
        ///</summary>
        public decimal? RefundMoney{get { return _RefundMoney; }set { _RefundMoney= value; }}


        ///<summary>
        ///退款笔数
        ///</summary>
        public decimal? RefundOrderCntr{get { return _RefundOrderCntr; }set { _RefundOrderCntr= value; }}


        ///<summary>
        ///退款亏损服务费
        ///</summary>
        public decimal? LossService{get { return _LossService; }set { _LossService= value; }}




    }
}