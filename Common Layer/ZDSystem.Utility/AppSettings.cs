using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZDSystem.Utility
{
    public class AppSettings
    {
        public static string JSVerson
        {
            get
            {
                return Lib4Net.Framework.Settings.SettingHelper.Instance.GetData("JSVerson");
            }
        }


        /// <summary>
        /// 从Appsetting中获取配置文件节点值
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>value</returns>
        public static string GetValue(string key)
        {
            if (string.IsNullOrEmpty(key))
                return null;
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// 系统编号
        /// </summary>
        public static string SystemNo
        {
            get { return GetValue("SystemNo"); }
        }


    }
}