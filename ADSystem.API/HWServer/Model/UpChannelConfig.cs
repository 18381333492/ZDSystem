using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADSystem.API.HWServer.Model
{

    /// <summary>
    /// 上游渠道配置model
    /// </summary>
    public class UpChannelConfig
    {
        /// <summary>
        /// 接口商户标识
        /// </summary>
        public string API_UID
        {
            get;
            set;
        }

        /// <summary>
        /// 签名key
        /// </summary>
        public string API_KEY
        {
            get;
            set;
        }

        /// <summary>
        /// 产品查询地址
        /// </summary>
        public string PRODUCT_QUERY_URL
        {
            get;
            set;
        }

        /// <summary>
        /// 手机号段查询地址
        /// </summary>
        public string MOBILE_QUERY_URL
        {
            get;
            set;
        }
    }
}