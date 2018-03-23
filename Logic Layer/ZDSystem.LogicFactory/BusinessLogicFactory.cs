using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lib4Net.Data;
using Lib4Net.Xml;
using Lib4Net.Core;
using Lib4Net.Config;
using Lib4Net.IO;
using System.IO;
namespace ZDSystem.LogicFactories
{
    /// <summary>
    /// 通过配置文件中的信息加载程序集
    /// </summary>
    public class BusinessLogicFactory:ProviderFactory
    {
        private static readonly object obj = new object();
        private static BusinessLogicFactory _instance;
        public BusinessLogicFactory():base("BusinessLogicPath")
        {           
        }
        /// <summary>
        /// 获取当前实例
        /// </summary>
        public static BusinessLogicFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new BusinessLogicFactory();
                        }
                    }
                }
                return _instance;
            }
        }
    }
}