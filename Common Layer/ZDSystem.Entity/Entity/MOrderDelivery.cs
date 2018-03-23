using System;
using System.Collections.Generic;
using System.Text;
using Lib4Net.Comm;
using Lib4Net.ORM;


namespace ZDSystem.Entity
{

    ///<summary>
    ///实体：订单发货表
    ///</summary>
    [ODXmlConfig(EntityConfigType.Xml, "", "~/config/econfig/OrderDelivery.xml")]
    public class MOrderDelivery : EntityBase
    {

        private long? _DeliveryId;
        private string _OrderNo;
        private string _RobotIp;
        private string _UpOrderNo;
        private int? _Status;
        private int? _ManualStatus;
        private string _ResultMsg;
        private int? _QueryStatus;
        private string _QueryMsg;
        private string _QueryIp;
        private DateTime? _CreateTime;
        private DateTime? _FinishTime;
        private DateTime? _QueryCreateTime;
        private DateTime? _NextQueryTime;
        private int? _QueryCntr;


        ///<summary>
        ///发货编号
        ///</summary>
        public long? DeliveryId { get { return _DeliveryId; } set { _DeliveryId = value; } }


        ///<summary>
        ///订单号
        ///</summary>
        public string OrderNo { get { return _OrderNo; } set { _OrderNo = value; } }


        ///<summary>
        ///发货ip
        ///</summary>
        public string RobotIp { get { return _RobotIp; } set { _RobotIp = value; } }


        ///<summary>
        ///上游订单号
        ///</summary>
        public string UpOrderNo { get { return _UpOrderNo; } set { _UpOrderNo = value; } }


        ///<summary>
        ///状态:20-等待发货30-正在发货90-失败0-成功
        ///</summary>
        public int? Status { get { return _Status; } set { _Status = value; } }


        ///<summary>
        ///人工状态:10-无需20-等待人工0-成功
        ///</summary>
        public int? ManualStatus { get { return _ManualStatus; } set { _ManualStatus = value; } }


        ///<summary>
        ///结果消息
        ///</summary>
        public string ResultMsg { get { return _ResultMsg; } set { _ResultMsg = value; } }


        ///<summary>
        ///查询状态10-无需20-等待查询90-失败0-成功
        ///</summary>
        public int? QueryStatus { get { return _QueryStatus; } set { _QueryStatus = value; } }


        ///<summary>
        ///查询消息
        ///</summary>
        public string QueryMsg { get { return _QueryMsg; } set { _QueryMsg = value; } }


        ///<summary>
        ///查询机器IP
        ///</summary>
        public string QueryIp { get { return _QueryIp; } set { _QueryIp = value; } }


        ///<summary>
        ///创建时间
        ///</summary>
        public DateTime? CreateTime { get { return _CreateTime; } set { _CreateTime = value; } }


        ///<summary>
        ///完成时间
        ///</summary>
        public DateTime? FinishTime { get { return _FinishTime; } set { _FinishTime = value; } }


        ///<summary>
        ///查询创建时间
        ///</summary>
        public DateTime? QueryCreateTime { get { return _QueryCreateTime; } set { _QueryCreateTime = value; } }


        ///<summary>
        ///下次查询时间
        ///</summary>
        public DateTime? NextQueryTime { get { return _NextQueryTime; } set { _NextQueryTime = value; } }


        ///<summary>
        ///查询次数
        ///</summary>
        public int? QueryCntr { get { return _QueryCntr; } set { _QueryCntr = value; } }

        /// <summary>
        /// 下游渠道编号
        /// </summary>
        public int? DownChannelNo { get; set; }
    }
}