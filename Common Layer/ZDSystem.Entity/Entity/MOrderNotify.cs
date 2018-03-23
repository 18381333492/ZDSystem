using System;
using System.Collections.Generic;
using System.Text;
using Lib4Net.Comm;
using Lib4Net.ORM;


namespace ZDSystem.Entity
{

    ///<summary>
    ///实体：订单通知
    ///</summary>
    [ODXmlConfig(EntityConfigType.Xml, "", "~/config/econfig/OrderNotify.xml")]
    public class MOrderNotify : EntityBase
    {
        private int? _NotifyId;
        private string _OrderNo;
        private int? _Status;
        private string _NotifyUrl;
        private int? _LimitNotify;
        private string _RobotIp;
        private int? _NotifyCntr;
        private DateTime? _CreateTime;
        private DateTime? _NextTime;
        private DateTime? _FinishTime;
        private int? _NotifyType;
        private string _ResultMsg;


        /// <summary>
        /// 通知编号
        /// </summary>
        public int? NotifyId
        {
            get { return _NotifyId; }
            set { _NotifyId = value; }
        }
        ///<summary>
        ///订单号
        ///</summary>
        public string OrderNo { get { return _OrderNo; } set { _OrderNo = value; } }


        ///<summary>
        ///回调状态10:无需处理20:等待处理30:正在处理0:处理成功90:处理失败
        ///</summary>
        public int? Status { get { return _Status; } set { _Status = value; } }


        ///<summary>
        ///通知地址
        ///</summary>
        public string NotifyUrl { get { return _NotifyUrl; } set { _NotifyUrl = value; } }


        ///<summary>
        ///限制回调次数
        ///</summary>
        public int? LimitNotify { get { return _LimitNotify; } set { _LimitNotify = value; } }


        ///<summary>
        ///机器IP
        ///</summary>
        public string RobotIp { get { return _RobotIp; } set { _RobotIp = value; } }


        ///<summary>
        ///回调次数
        ///</summary>
        public int? NotifyCntr { get { return _NotifyCntr; } set { _NotifyCntr = value; } }


        ///<summary>
        ///创建时间
        ///</summary>
        public DateTime? CreateTime { get { return _CreateTime; } set { _CreateTime = value; } }


        ///<summary>
        ///下次通知时间
        ///</summary>
        public DateTime? NextTime { get { return _NextTime; } set { _NextTime = value; } }


        ///<summary>
        ///完成时间
        ///</summary>
        public DateTime? FinishTime { get { return _FinishTime; } set { _FinishTime = value; } }

        /// <summary>
        /// 通知类型:1.支付,2.退款,3.充值
        /// </summary>
        public int? NotifyType
        {
            get { return _NotifyType; }
            set { _NotifyType = value; }
        }
        /// <summary>
        /// 结果消息
        /// </summary>
        public string ResultMsg
        {
            get { return _ResultMsg; }
            set { _ResultMsg = value; }
        }

        /// <summary>
        /// 下游渠道编号
        /// </summary>
        public int? DownChannelNo { get; set; }
    }
}