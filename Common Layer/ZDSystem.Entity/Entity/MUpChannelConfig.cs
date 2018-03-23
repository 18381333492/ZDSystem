using System;
using System.Collections.Generic;
using System.Text;
using Lib4Net.Comm;
using Lib4Net.ORM;


namespace ZDSystem.Entity
{

    ///<summary>
    ///实体：上游配置表
    ///</summary>
    [ODXmlConfig(EntityConfigType.Xml, "", "~/config/econfig/UpChannelConfig.xml")]
    public class MUpChannelConfig : EntityBase
    {

        private string _RechargeUrl;
        private string _QueryUrl;
        private string _NotifyUrl;
        private string _RechargeScript;
        private string _QueryScript;
        private string _ApiUid;
        private string _ApiKey;
        private string _NotifyScript;


        ///<summary>
        ///充值地址
        ///</summary>
        public string RechargeUrl { get { return _RechargeUrl; } set { _RechargeUrl = value; } }


        ///<summary>
        ///查询地址
        ///</summary>
        public string QueryUrl { get { return _QueryUrl; } set { _QueryUrl = value; } }


        ///<summary>
        ///通知地址
        ///</summary>
        public string NotifyUrl { get { return _NotifyUrl; } set { _NotifyUrl = value; } }


        ///<summary>
        ///充值脚本
        ///</summary>
        public string RechargeScript { get { return _RechargeScript; } set { _RechargeScript = value; } }


        ///<summary>
        ///查询脚本
        ///</summary>
        public string QueryScript { get { return _QueryScript; } set { _QueryScript = value; } }


        ///<summary>
        ///接口id
        ///</summary>
        public string ApiUid { get { return _ApiUid; } set { _ApiUid = value; } }


        ///<summary>
        ///接口Key
        ///</summary>
        public string ApiKey { get { return _ApiKey; } set { _ApiKey = value; } }


        ///<summary>
        ///通知脚本
        ///</summary>
        public string NotifyScript { get { return _NotifyScript; } set { _NotifyScript = value; } }

        /// <summary>
        /// 产品查询地址
        /// </summary>
        public string ProductQueryUrl { get; set; }

        /// <summary>
        /// 号段查询地址
        /// </summary>
        public string MobileQueryUrl { get; set; }

        /// <summary>
        /// 下游渠道编号
        /// </summary>
        public int? DownChannelNo { get; set; }

        /// <summary>
        /// 主键编号
        /// </summary>
        public long? Id { get; set; }

    }
}