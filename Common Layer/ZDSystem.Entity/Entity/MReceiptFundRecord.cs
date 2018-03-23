using System;
using System.Collections.Generic;
using System.Text;
using Lib4Net.Comm;
using Lib4Net.ORM;


namespace ZDSystem.Entity
{

      ///<summary>
      ///实体：收款账户资金变动表
      ///</summary>
     [ODXmlConfig(EntityConfigType.Xml,"","~/config/econfig/ReceiptFundRecord.xml")]
    public class MReceiptFundRecord:EntityBase
    {    

                private long? _RecordId;
        private long? _AccountId;
        private int? _ChangeType;
        private decimal? _ChangeAmount;
        private decimal? _Balance;
        private DateTime? _ChangeTime;
        private string _Operator;
        private string _Remark;
        private int? _ProductType;
        private decimal? _ServiceLoss;
        private string _OrderNO;


                 ///<summary>
        ///记录编号
        ///</summary>
        public long? RecordId{get { return _RecordId; }set { _RecordId= value; }}


        ///<summary>
        ///账户编号
        ///</summary>
        public long? AccountId{get { return _AccountId; }set { _AccountId= value; }}


        ///<summary>
        ///变动类型1:支付2:退款2:提现
        ///</summary>
        public int? ChangeType{get { return _ChangeType; }set { _ChangeType= value; }}


        ///<summary>
        ///变动金额
        ///</summary>
        public decimal? ChangeAmount{get { return _ChangeAmount; }set { _ChangeAmount= value; }}


        ///<summary>
        ///当前余额
        ///</summary>
        public decimal? Balance{get { return _Balance; }set { _Balance= value; }}


        ///<summary>
        ///变动时间
        ///</summary>
        public DateTime? ChangeTime{get { return _ChangeTime; }set { _ChangeTime= value; }}


        ///<summary>
        ///操作人
        ///</summary>
        public string Operator{get { return _Operator; }set { _Operator= value; }}


        ///<summary>
        ///备注
        ///</summary>
        public string Remark{get { return _Remark; }set { _Remark= value; }}


        ///<summary>
        ///产品类型1:话费2:流量
        ///</summary>
        public int? ProductType{get { return _ProductType; }set { _ProductType= value; }}

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNO { get { return _OrderNO; } set { _OrderNO = value; } }


        /// <summary>
        /// 亏损的服务费
        /// </summary>
        public decimal? ServiceLoss { get { return _ServiceLoss; } set { _ServiceLoss = value; } }


    }
}