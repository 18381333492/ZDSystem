using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZDSystem.Entity;
namespace ZDSystem.Model
{
    /// <summary>
    /// 实体:MZdCouponUsed 的列表页面Model
    /// </summary>
    public class ZdCouponUsedViewModel
    {
        private MZdCouponUsed _currentModel = new MZdCouponUsed();
        /// <summary>
        /// 当前实体元素
        /// </summary>
        public MZdCouponUsed CurrentModel
        {
            get { return _currentModel; }
            set { _currentModel = value; }
        }
        /// <summary>
        /// 主键编号
        /// </summary>
        public string Id { get; set; }


    }
}