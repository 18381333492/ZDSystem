using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using Lib4Net.Framework.Settings;
using Lib4Net.Comm;

namespace ZDSystem.Utility
{
    public static class CommonHelper
    {
        public static SortedDictionary<string, string> NameValueToDictionary(NameValueCollection nvc)
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();

            if (nvc == null || nvc.AllKeys.Count() == 0)
            {
                return sArray;
            }

            // Get names of all forms into a string array.
            string[] requestKeys = nvc.AllKeys;

            for (i = 0; i < requestKeys.Length; i++)
            {
                sArray.Add(requestKeys[i], nvc[requestKeys[i]]);
            }

            return sArray;
        }

        public static string GetSignOrgin(SortedDictionary<string, string> dic, string split = "&", string connect = "=")
        {
            StringBuilder sb = new StringBuilder();
            if (dic == null || dic.Keys.Count == 0)
            {
                return sb.ToString();
            }
            foreach (var item in dic)
            {
                if (item.Key != "sign")
                {
                    if (sb.Length > 0)
                        sb.Append(split);
                    sb.AppendFormat("{0}{1}{2}", item.Key, connect, item.Value);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取验证码图片路径
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetValidateCodePath(string fileName)
        {
            return string.Format("{0}{1}", SettingHelper.Instance.GetData("CodeImagePath"), fileName);
        }

        public static string GetResultPath(string fileName)
        {
            return string.Format("{0}{1}", SettingHelper.Instance.GetData("RequstPath"), fileName);
        }

        // <summary>
        /// 加密密码(对用户密码加密请都用此方法)
        /// </summary>
        /// <param name="pwd">用户密码</param>
        /// <returns></returns>
        public static string Md5Encrypt(string pwd)
        {
            return Md5Helper.Encrypt(pwd);
        }     

    }
}