using System;
using System.Collections.Generic;
using System.Text;
using Lib4Net.Comm;
using Lib4Net.ORM;


namespace ZDSystem.Entity
{

      ///<summary>
      ///实体：操作日志
      ///</summary>
     [ODXmlConfig(EntityConfigType.Xml,"","~/config/econfig/OperateLog.xml")]
    public class MOperateLog:EntityBase
    {    

                private long? _LogId;
        private string _OrderNo;
        private string _Content;
        private string _Operator;
        private DateTime? _CreateTime;


                 ///<summary>
        ///编号
        ///</summary>
        public long? LogId{get { return _LogId; }set { _LogId= value; }}


        ///<summary>
        ///订单号
        ///</summary>
        public string OrderNo{get { return _OrderNo; }set { _OrderNo= value; }}


        ///<summary>
        ///备注
        ///</summary>
        public string Content{get { return _Content; }set { _Content= value; }}


        ///<summary>
        ///操作人
        ///</summary>
        public string Operator{get { return _Operator; }set { _Operator= value; }}


        ///<summary>
        ///创建时间
        ///</summary>
        public DateTime? CreateTime{get { return _CreateTime; }set { _CreateTime= value; }}




    }
}