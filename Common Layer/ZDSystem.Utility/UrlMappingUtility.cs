using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lib4Net.Framework.Routing;

namespace ZDSystem.Utility
{
    public class UrlMappingUtility : GroupMapping
    {
        protected static readonly object obj = new object();
        private static UrlMappingUtility instance;

        /// <summary>
        /// 获取当前实例
        /// </summary>
        public static UrlMappingUtility Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (obj)
                    {
                        if (instance == null)
                        {
                            instance = new UrlMappingUtility();
                        }
                    }
                }
                return instance;
            }
        }
        public UrlMappingUtility()
            : base("UrlMapping")
        {
        }
    }
}