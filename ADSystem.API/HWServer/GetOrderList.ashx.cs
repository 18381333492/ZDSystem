using System;
using System.Data;
using System.Web;
using Lib4Net.Logs;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Oracle.ManagedDataAccess.Client;

namespace ADSystem.API
{
    /// <summary>
    /// GetOrderList 的摘要说明
    /// </summary>
    public class GetOrderList : IHttpHandler
    {
        private static readonly ILogger logger = LoggerManager.Instance.GetLogger("HWGetOrderList");
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string Account = context.Request["Account"];
            string pi = context.Request["page"];
            string ps = context.Request["rows"];
            try
            {
                string ret = GetOrder(pi, ps, Account);
                context.Response.Write(ret);
            }
            catch (Exception e)
            {
                logger.Info(e.Message);
                logger.Fatal(e.Message, e);
            }

        }


        public static string GetOrder(string pi, string ps, string account)
        {
            DataTable model = new DataTable();
            string sql = @"SELECT 
                                  t.order_no OrderNo,
                                  t.create_time CreateTime,
                                  t.pay_type PayType,
                                  t.user_payed UserPayed,
                                  t.mobile Mobile,
                                  t.product_name ProductName,
                                  (case
                                    when t.refund_state <> 10 
                                    then t1.name 
                                    when t.pay_status = 30 
                                    then '等待付款' 
                                    when t.pay_status = 90 
                                    then '已失效' 
                                    else t2.name end) status
                                    from (select RID
                                            from (select R.RID,ROWNUM LINENUM
                                                    from (select t.rowid RID from order_main t
                                                        where  
                                                        t.account =:Account
                                                        and (t.create_time > sysdate - 1 or (t.pay_status <> 90 and t.create_time  > sysdate - 90))
                                                        order by t.create_time desc
                                                        ) R where ROWNUM <= :PS * :PI)
                                                    where LINENUM > :PS * (:PI - 1)) TAB1
                                    inner join order_main t on t.rowid = tab1.rid
                                    left join sys_dictionary t1 on t.refund_state = t1.value and t1.type = 'ReStatus'
                                    left join sys_dictionary t2 on t.order_status = t2.value and t2.type = 'OrderStatus' 
                                    order by t.create_time desc";
            OracleParameter[] opams = { 
                    new OracleParameter(":Account",account),
                    new OracleParameter(":PS",ps),
                    new OracleParameter(":PI",pi)
                                      };
            model = SqlHelper.GetDataTable(sql, CommandType.Text, opams);
            string ret = JsonConvert.SerializeObject(model, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            return ret;
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