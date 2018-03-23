using System;
using System.Collections.Generic;
using System.Text;
using Lib4Net.Comm;
using Lib4Net.ORM;


namespace ZDSystem.Entity
{

    ///<summary>
    ///实体：错误日志
    ///</summary>
    [ODXmlConfig(EntityConfigType.Xml, "", "~/config/econfig/SystemLog.xml")]
    public class MSystemLog : EntityBase
    {

        private long? _LogId;
        private string _ObjectName;
        private string _Content;
        private DateTime? _CreateTime;
        private string _TraceInfo;


        ///<summary>
        ///编号
        ///</summary>
        public long? LogId { get { return _LogId; } set { _LogId = value; } }


        ///<summary>
        ///对象名称
        ///</summary>
        public string ObjectName { get { return _ObjectName; } set { _ObjectName = value; } }


        ///<summary>
        ///内容
        ///</summary>
        public string Content { get { return _Content; } set { _Content = value; } }


        ///<summary>
        ///创建时间
        ///</summary>
        public DateTime? CreateTime { get { return _CreateTime; } set { _CreateTime = value; } }

        /// <summary>
        /// 堆栈信息
        /// </summary>
        public string TraceInfo
        {
            get { return _TraceInfo; }
            set { _TraceInfo = value; }
        }

    }
}