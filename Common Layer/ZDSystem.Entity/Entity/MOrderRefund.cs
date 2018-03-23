using System;
using System.Collections.Generic;
using System.Text;
using Lib4Net.Comm;
using Lib4Net.ORM;


namespace ZDSystem.Entity
{

    ///<summary>
    ///实体：订单退款
    ///</summary>
    [ODXmlConfig(EntityConfigType.Xml, "", "~/config/econfig/OrderRefund.xml")]
    public class MOrderRefund : EntityBase
    {

        private long? _RecordId;
        private string _OrderNo;
        private decimal? _RefundFee;
        private DateTime? _CreateTime;
        private string _CreateUser;
        private string _Remark;
        private string _RefundNo;
        private int? _Status;
        private DateTime? _FinishTime;
        private string _RefundMsg;
        private DateTime? _SendTime;
        private decimal? _LossServiceFee;

        /// <summary>
        /// 亏损手续费
        /// </summary>
        public decimal? LossServiceFee
        {
            get { return _LossServiceFee; }
            set { _LossServiceFee = value; }
        }
        ///<summary>
        ///退款编号
        ///</summary>
        public long? RecordId { get { return _RecordId; } set { _RecordId = value; } }


        ///<summary>
        ///订单号
        ///</summary>
        public string OrderNo { get { return _OrderNo; } set { _OrderNo = value; } }


        ///<summary>
        ///退款金额
        ///</summary>
        public decimal? RefundFee { get { return _RefundFee; } set { _RefundFee = value; } }


        ///<summary>
        ///创建时间
        ///</summary>
        public DateTime? CreateTime { get { return _CreateTime; } set { _CreateTime = value; } }


        ///<summary>
        ///创建人
        ///</summary>
        public string CreateUser { get { return _CreateUser; } set { _CreateUser = value; } }


        ///<summary>
        ///备注
        ///</summary>
        public string Remark { get { return _Remark; } set { _Remark = value; } }


        ///<summary>
        ///退款单号
        ///</summary>
        public string RefundNo { get { return _RefundNo; } set { _RefundNo = value; } }


        ///<summary>
        ///状态20-等待退款30-正在退款90-失败0-成功
        ///</summary>
        public int? Status { get { return _Status; } set { _Status = value; } }


        ///<summary>
        ///完成时间
        ///</summary>
        public DateTime? FinishTime { get { return _FinishTime; } set { _FinishTime = value; } }

        /// <summary>
        /// 上游退款消息
        /// </summary>
        public string RefundMsg
        {
            get { return _RefundMsg; }
            set { _RefundMsg = value; }
        }
        /// <summary>
        /// 发送上游退款的时间
        /// </summary>
        public DateTime? SendTime
        {
            get { return _SendTime; }
            set { _SendTime = value; }
        }
        /// <summary>
        /// 退款机器人
        /// </summary>
        public string RobotIP { get; set; }

        /// <summary>
        /// 查询机器人
        /// </summary>
        public string QueryIP { get; set; }

        /// <summary>
        /// 下次查询时间  
        /// </summary>
        public DateTime? NextTime { get; set; }

        /// <summary>
        /// 回调状态
        /// </summary>
        public int? NotifyStatus { get; set; }

        /// <summary>
        /// 回调时间
        /// </summary>
        public DateTime? NotifyTime { get; set; }

        /// <summary>
        /// 退款原因
        /// </summary>
        public string RefundDesc { get; set; }

        /// <summary>
        /// 是否需要通知
        /// </summary>
        public int? NeedNotify { get; set; }

        /// <summary>
        /// 下游渠道编号
        /// </summary>
        public int? DownChannelNo { get; set; }
    }
}