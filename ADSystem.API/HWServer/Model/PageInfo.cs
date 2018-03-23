using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADSystem.API.HWServer.Model
{
    /// <summary>
    /// 分页model
    /// </summary>
    public class PageInfo
    {
         /// <summary>
        /// 当前页索引
        /// </summary>
        public int page=1;
        /// <summary>
        /// 页面数据的大小
        /// </summary>
        public int rows=10;

        /// <summary>
        ///  用户账户
        /// </summary>
        public string account;
    }
}