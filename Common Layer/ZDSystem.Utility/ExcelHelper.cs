using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;

namespace ZDSystem.Utility
{
    public class ExcelHelper
    {
        #region 公共程序（导出excel）
        private static string ExportTable(DataTable tb)
        {
            StringBuilder data = new StringBuilder();
            object temp;
            data.Append("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
            //写出列名
            data.Append("<tr style=\"font-weight: bold; white-space: nowrap;\">");
            foreach (DataColumn column in tb.Columns)
            {
                data.AppendFormat("<td>{0}</td>", column.ColumnName);
            }
            data.Append("</tr>");

            //写出数据
            foreach (DataRow row in tb.Rows)
            {
                data.Append("<tr>");
                foreach (DataColumn column in tb.Columns)
                {
                    temp = row[column];
                    if (temp == null || temp == DBNull.Value)
                    {
                        temp = string.Empty;
                    }

                    string tdWithStyle = "<td style=\"mso-number-format:'";
                    tdWithStyle += @"\@';";
                    tdWithStyle += "\">";
                    tdWithStyle += "{0}</td>";

                    data.AppendFormat("<td style='mso-number-format:\"@\";'>{0}</td>", temp.ToString());
                }
                data.Append("</tr>");
            }

            if (tb.Rows.Count == 0)
            {
                data.AppendFormat("<tr><td colspan='{0}' style='text-align:center;'>对不起,没有相关数据信息!</td></tr>", tb.Columns.Count);
            }

            data.Append("</table>");

            return data.ToString();
        }


        private static string GetExcelHead()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">");
            sb.Append("<head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" /></head>");
            sb.Append(" <!--[if gte mso 9]><xml>");
            sb.Append("<x:ExcelWorkbook>");
            sb.Append("<x:ExcelWorksheets>");
            sb.Append("<x:ExcelWorksheet>");
            sb.Append("<x:Name></x:Name>");
            sb.Append("<x:WorksheetOptions>");
            sb.Append("<x:Print>");
            sb.Append("<x:ValidPrinterInfo />");
            sb.Append(" </x:Print>");
            sb.Append("</x:WorksheetOptions>");
            sb.Append("</x:ExcelWorksheet>");
            sb.Append("</x:ExcelWorksheets>");
            sb.Append("</x:ExcelWorkbook>");
            sb.Append("</xml>");
            sb.Append("<![endif]-->");
            sb.Append(" </head>");
            sb.Append("<body>");
            return sb.ToString();

        }
        private static string GetExcelFooter()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("</body>");
            sb.Append("</html>");
            return sb.ToString();
        }

        /// <summary>
        /// 输出Excel文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="content"></param>
        public static void RenderExcel(string fileName, DataTable dt)
        {
            HttpContext curContext = System.Web.HttpContext.Current;
            string header = GetExcelHead();
            string content = ExportTable(dt);
            string footer = GetExcelFooter();
            curContext.Response.Clear();
            curContext.Response.ContentType = "application/vnd.ms-excel.numberformat:@";
            curContext.Response.ContentEncoding = System.Text.Encoding.UTF8;
            //curContext.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, Encoding.UTF8).ToString());
            curContext.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
            curContext.Response.Charset = "utf-8";
            curContext.Response.Write(header);//显示excel的网格线
            curContext.Response.Write(content);//导出
            curContext.Response.Write(footer);//显示excel的网格线
            curContext.Response.Flush();
            curContext.Response.End();
        }
        #endregion
    }
}