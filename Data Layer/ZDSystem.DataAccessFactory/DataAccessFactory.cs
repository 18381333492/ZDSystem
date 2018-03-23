using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lib4Net.Xml;

namespace ZDSystem.DataAccessFactories
{
    public class DataAccessFactory : ProviderFactory
    {
        private static readonly object obj = new object();
        private static DataAccessFactory _instance;
        public DataAccessFactory()
            : base("DataAccessPath")
        {
        }
        /// <summary>
        /// 获取当前实例
        /// </summary>
        public static DataAccessFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new DataAccessFactory();
                        }
                    }
                }
                return _instance;
            }
        }
    }

}