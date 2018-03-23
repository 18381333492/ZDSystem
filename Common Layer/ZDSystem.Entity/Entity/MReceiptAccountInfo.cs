using System;
using System.Collections.Generic;
using System.Text;
using Lib4Net.Comm;
using Lib4Net.ORM;


namespace ZDSystem.Entity
{

    ///<summary>
    ///实体：收款账户信息表
    ///</summary>
    [ODXmlConfig(EntityConfigType.Xml, "", "~/config/econfig/ReceiptAccountInfo.xml")]
    public class MReceiptAccountInfo : EntityBase
    {

        private long? _AccountId;
        private int? _AccountType;
        private string _Appid;
        private string _MchId;
        private string _PubKey;
        private string _PriKey;
        private string _CertificatePath;
        private decimal? _Balance;
        private decimal? _ServiceRadio;
        private string _Ext1;
        private string _Ext2;
        private string _Ext3;
        private string _Ext4;
        private string _Ext5;
        private string _Remark;
        private int? _Status;
        private string _SyncNotifyUrl;
        private string _NonsyncNotifyUrl;
        private decimal? _ServiceLoss;
        /// <summary>
        /// 退款亏损
        /// </summary>
        public decimal? ServiceLoss
        {
            get { return _ServiceLoss; }
            set { _ServiceLoss = value; }
        }
        ///<summary>
        ///账户编号
        ///</summary>
        public long? AccountId { get { return _AccountId; } set { _AccountId = value; } }


        ///<summary>
        ///账户类型1:支付宝2:微信3:优惠券
        ///</summary>
        public int? AccountType { get { return _AccountType; } set { _AccountType = value; } }


        ///<summary>
        ///收款账号ID
        ///</summary>
        public string Appid { get { return _Appid; } set { _Appid = value; } }


        ///<summary>
        ///商户号
        ///</summary>
        public string MchId { get { return _MchId; } set { _MchId = value; } }


        ///<summary>
        ///支付宝公钥,微信秘钥
        ///</summary>
        public string PubKey { get { return _PubKey; } set { _PubKey = value; } }


        ///<summary>
        ///私钥
        ///</summary>
        public string PriKey { get { return _PriKey; } set { _PriKey = value; } }


        ///<summary>
        ///证书路径
        ///</summary>
        public string CertificatePath { get { return _CertificatePath; } set { _CertificatePath = value; } }


        ///<summary>
        ///余额
        ///</summary>
        public decimal? Balance { get { return _Balance; } set { _Balance = value; } }


        ///<summary>
        ///手续费率
        ///</summary>
        public decimal? ServiceRadio { get { return _ServiceRadio; } set { _ServiceRadio = value; } }


        ///<summary>
        ///扩展1
        ///</summary>
        public string Ext1 { get { return _Ext1; } set { _Ext1 = value; } }


        ///<summary>
        ///扩展2
        ///</summary>
        public string Ext2 { get { return _Ext2; } set { _Ext2 = value; } }


        ///<summary>
        ///扩展3
        ///</summary>
        public string Ext3 { get { return _Ext3; } set { _Ext3 = value; } }


        ///<summary>
        ///扩展4
        ///</summary>
        public string Ext4 { get { return _Ext4; } set { _Ext4 = value; } }


        ///<summary>
        ///扩展5
        ///</summary>
        public string Ext5 { get { return _Ext5; } set { _Ext5 = value; } }


        ///<summary>
        ///备注
        ///</summary>
        public string Remark { get { return _Remark; } set { _Remark = value; } }


        ///<summary>
        ///状态0有效1无效
        ///</summary>
        public int? Status { get { return _Status; } set { _Status = value; } }


        ///<summary>
        ///同步通知地址
        ///</summary>
        public string SyncNotifyUrl { get { return _SyncNotifyUrl; } set { _SyncNotifyUrl = value; } }


        ///<summary>
        ///异步通知地址
        ///</summary>
        public string NonsyncNotifyUrl { get { return _NonsyncNotifyUrl; } set { _NonsyncNotifyUrl = value; } }


        /// <summary>
        /// 退款lua脚本名称
        /// </summary>
        public string RefundScriptUrl { get; set; }

        /// <summary>
        /// 下游渠道编号
        /// </summary>
        public int? DownChannelNo { get; set; }
    }
}