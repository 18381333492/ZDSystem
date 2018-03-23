using System;
using System.Collections.Generic;
using System.Text;
using Lib4Net.Comm;
using Lib4Net.ORM;


namespace ZDSystem.Entity
{
    [ODXmlConfig(EntityConfigType.Xml, "", "~/config/econfig/DownChannel.xml")]
    public class MDownChannel : EntityBase
    {
        /// <summary>
        /// 下游渠道编号
        /// </summary>
        public long? DownChannelNo { get; set; }

        /// <summary>
        /// 渠道名称
        /// </summary>
        public string ChannelName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 通知脚本
        /// </summary>
        public string NotifyScript { get; set; }

        /// <summary>
        /// 通知地址
        /// </summary>
        public string NotifyUrl { get; set; }

        /// <summary>
        /// cp名称
        /// </summary>
        public string CpName { get; set; }

        /// <summary>
        /// 产品线
        /// </summary>
        public string ProductLine { get; set; }

        /// <summary>
        /// 订单详情页面
        /// </summary>
        public string DeailUrl { get; set; }

        /// <summary>
        /// 是否需要充值话费  0-需要 1-不需要
        /// </summary>
        public int? NeedRechargeTel { get; set; }

        /// <summary>
        /// 是否需要充值流量  0-需要 1-不需要
        /// </summary>
        public int? NeedRechargeFlow { get; set; }

        /// <summary>
        /// 是否需要通知  0-需要 1-不需要
        /// </summary>
        public int? NeedNotity { get; set; }
    }
}
