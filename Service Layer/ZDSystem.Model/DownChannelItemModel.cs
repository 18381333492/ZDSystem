using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZDSystem.Entity;
namespace ZDSystem.Model
{
    public class DownChannelItemModel
    {
        private MDownChannel _currentModel = new MDownChannel();
        /// <summary>
        /// 当前实体元素
        /// </summary>
        public MDownChannel CurrentModel
        {
            get { return _currentModel; }
            set { _currentModel = value; }
        }
        /// <summary>
        /// 主键编号
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 处理消息
        /// </summary>
        public string Message { get; set; }

    }
}
