using System;
using System.Data;
using System.Web;
using Lib4Net.Logs;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using  Oracle.ManagedDataAccess.Client;
namespace ADSystem.API.HWServer
{
    /// <summary>
    /// 根据订单号获取订单详情
    /// </summary>
    public class GetOrderDetail : IHttpHandler
    {
        private static readonly ILogger logger = LoggerManager.Instance.GetLogger("HWGetOrderDetail");
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                //获取订单号
                string sOrderNo = context.Request["sOrderNo"];
                string sql = @"SELECT 
                              t.order_no,
                              t.create_time, 
                              t.mobile,
                              t.price,
                              (case when t.refund_state <> 10 then t1.name when t.pay_status = 30 then '等待付款' when t.pay_status = 90 then '已失效' else t2.name end) status,
                              t.product_name
                              FROM order_main t
                              left join sys_dictionary t1 on t.refund_state = t1.value and t1.type = 'ReStatus'                              
                              left join sys_dictionary t2 on t.order_status = t2.value and t2.type = 'OrderStatus' 
                              WHERE t.order_no=:sOrderNo";

                OracleParameter[] opams = { 
                    new OracleParameter(":sOrderNo",sOrderNo)};
                var dt = SqlHelper.GetDataTable(sql, CommandType.Text, opams);
                if (dt.Rows.Count > 0)
                {
                    var res = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(dt, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" }).TrimStart('[').TrimEnd(']'));
                    context.Response.ContentType = "text/plain";
                    context.Response.Write(JsonConvert.SerializeObject(new { success = true, data = res }));
                }
                else
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write(JsonConvert.SerializeObject(new { success = false, info = "订单数据丢失" }));
                }
            }
            catch (Exception e)
            {
                logger.Info(e.Message);
                logger.Fatal(e.Message, e);
                var res = new { success = false, info = "服务错误" };
                context.Response.ContentType = "text/plain";
                context.Response.Write(JsonConvert.SerializeObject(res));
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