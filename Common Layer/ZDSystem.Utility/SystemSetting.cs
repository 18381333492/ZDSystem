using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lib4Net.Framework.Settings;

namespace ZDSystem.Utility
{
    public static class SystemSetting
    {
        public static int PageSize
        {
            get { return SettingHelper.Instance.GetInt("pagesize", 10); }
        }
        public static string SystemLoginUrl
        {
            get { return SettingHelper.Instance.GetData("SystemLoginUrl"); }
        }
        public static string LogoImg
        {
            get { return SettingHelper.Instance.GetData("LogoImg"); }
        }
        public static string SystemName
        {
            get { return SettingHelper.Instance.GetData("SystemName"); }
        }
        public static string ValidateKey
        {
            get { return SettingHelper.Instance.GetData("ValidateKey"); }
        }
        public static string UserNaviCacheKeyPre
        {
            get { return SettingHelper.Instance.GetData("UserNaviCacheKeyPre"); }
        }        
        /// <summary>
        /// 用户管理系统中的系统编号
        /// </summary>
        public static string SystemNo
        {
            get { return SettingHelper.Instance.GetData("SystemNo"); }
        }
    }
}