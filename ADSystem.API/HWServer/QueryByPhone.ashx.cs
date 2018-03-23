using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lib4Net.Logs;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace ADSystem.API.HWServer
{
    /// <summary>
    /// 通过手机号查询订单
    /// </summary>
    public class QueryByPhone : IHttpHandler
    {
        private static readonly ILogger logger = LoggerManager.Instance.GetLogger("HWQueryByPhone");
        public void ProcessRequest(HttpContext context)
        {

            string mobile = context.Request["mobile"];
            string page = context.Request["page"];
            string rows = context.Request["rows"];
            logger.Info(string.Format("请求的参数：{0},{1},{2}", mobile, page, rows));
            try
            {
                if (string.IsNullOrEmpty(mobile) || string.IsNullOrEmpty(page) || string.IsNullOrEmpty(rows))
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("参数错误");
                }
                else
                {
                    DataTable dt = new DataTable();
                    string sql = @"SELECT 
                                  t.order_no orderno,
                                  t.create_time createtime,
                                  t.user_payed userpay,
                                  t.product_name productname,
                                  t.mobile,
                                  t.face,
                                  (case
                                    when t.refund_state <> 10 
                                    then t1.name 
                                    else t2.name end) status
                                    from (select RID
                                            from (select R.RID,ROWNUM LINENUM
                                                    from (select t.rowid RID from order_main t
                                                        where  
                                                        t.pay_status=0 and t.down_channel_no=105 and  
                                                        t.mobile =:Mobile 
                                                        order by t.create_time desc
                                                        ) R where ROWNUM <= :PS * :PI)
                                                    where LINENUM > :PS * (:PI - 1)) TAB1
                                    inner join order_main t on t.rowid = tab1.rid
                                    left join sys_dictionary t1 on t.refund_state = t1.value and t1.type = 'ReStatus'
                                    left join sys_dictionary t2 on t.order_status = t2.value and t2.type = 'OrderStatus' 
                                    order by t.create_time desc";
                    OracleParameter[] opams = { 
                    new OracleParameter(":Mobile",mobile),
                    new OracleParameter(":PS",rows),
                    new OracleParameter(":PI",page)
                                      };
                    dt = SqlHelper.GetDataTable(sql, CommandType.Text, opams);
                    string ret = JsonConvert.SerializeObject(new { rows = dt }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    context.Response.ContentType = "text/plain";
                    context.Response.Write(ret);
                }
            }
            catch (Exception ex)
            {
                logger.Info(ex.Message);
                logger.Fatal(ex.Message, ex);
                context.Response.ContentType = "text/plain";
                context.Response.Write("Server Error");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}