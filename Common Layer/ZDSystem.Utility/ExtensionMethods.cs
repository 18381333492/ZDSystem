using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using System.Collections;
using Lib4Net.Framework.Settings;
namespace ZDSystem.Utility
{
    public static class ExtensionMethods
    {  
        private static string _defaultString = "-";

        /// <summary>
        /// 获取枚举类型的中文描述信息
        /// </summary>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <param name="input">输入参数</param>
        /// <returns>枚举描述信息</returns>
        public static string GetEnumDescription(this object input)
        {
            if (input == null)
                return _defaultString;
            Type enumType = input.GetType();
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("所传递的参数不是枚举类型");
            }
            var name = Enum.GetName(enumType, Convert.ToInt32(input));
            if (name == null)
                return string.Empty;
            object[] objs = enumType.GetField(name).GetCustomAttributes(typeof(DescriptionAttribute),false);
            if (objs == null || objs.Length == 0)
            {
                return string.Empty;
            }
            else
            {
                DescriptionAttribute attr = objs[0] as DescriptionAttribute;
                return attr.Description;
            }
        }



        /// <summary>
        /// 如果数据为空就显示为-
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToCustomerString(this object input)
        {
            if (input == null)
                return "---";
            else
                return input.ToString();
        }

        /// <summary>
        /// 去掉对象中的空字符串
        /// </summary>
        /// <param name="input">对象</param>
        public static void TrimEmptyProperty(this object input)
        {
            if (input == null)
                return;
            Type type = input.GetType();
            string stringFulllName = typeof(string).FullName;
            PropertyInfo[] infos = type.GetProperties();
            object val = null;
            string strval = null;
            foreach (PropertyInfo info in infos)
            {
                val = info.GetValue(input, null);
                if (val != null)
                {
                    if (val.GetType().FullName == stringFulllName)
                    {
                        strval = val as string;
                        if (string.IsNullOrWhiteSpace(strval))
                        {
                            strval = null;
                        }
                        else
                        {
                            strval = strval.Trim();
                        }
                        if (info.CanWrite)
                        {
                            info.SetValue(input, strval, null);
                        }
                    }
                }
            }
        }

        public static string ToFormatString(this DateTime? input)
        {
            if (input == null)
                return _defaultString;
            else
            {
                return input.Value.ToString("yyyy-MM-dd HH:ss:mm");
            }
        }

        public static string ToMoney(this decimal? input)
        {
            if (input == null)
                return _defaultString;
            else
                return input.Value.ToString("C");
        }

        public static string ToNumberString(this int? input)
        {
            if (input == null)
            {
                return _defaultString;
            }
            else return input.Value.ToString();
        }

        public static string FormatDate(this DateTime? date)
        {
            if (date == null)
                return "";
            return date.Value.ToString(
                SettingHelper.Instance.GetData("DateFormat"));
        }

        public static string FormatShortDate(this DateTime? date, string format="yyyy-MM-dd")
        {
            if (date == null)
                return "";
            return date.Value.ToString(format);
        }
         
        public static string FormatDateWithWeek(this DateTime? date)
        {
            if (date == null) return "";

            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("zh-CN");
            string week_cn = "(" + cultureInfo.DateTimeFormat.GetAbbreviatedDayName(date.Value.DayOfWeek).ToString() + ")";
            return date.Value.ToString("yyyy-MM-dd") + week_cn;
        }       
    }
}