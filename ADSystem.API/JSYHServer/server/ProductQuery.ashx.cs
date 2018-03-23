using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lib4Net.Logs;
using PayComon;
using Newtonsoft.Json;
using System.Configuration;
using System.Data;
using ADSystem.API.HWServer.Model;
using ADSystem.API.HWServer;

namespace ADSystem.API.JSYHServer.server
{
    /// <summary>
    /// 建设银行产品查询
    /// </summary>
    public class ProductQuery : IHttpHandler
    {
        private static readonly ILogger logger = LoggerManager.Instance.GetLogger("JSYHProductQuery");

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                int interfaceType = Convert.ToInt32(context.Request["interfaceType"]);//1-获取好码归属地 2-获取产品
                var UpConfig = GetUpConfig();
                if (UpConfig != null)
                {
                    if (interfaceType == 1)
                    {//查询号码区段
                        string sPhone = context.Request["sPhone"];
                        string cid = UpConfig.API_UID;
                        string key = UpConfig.API_KEY;
                        string sign = TenPayHelp.MD5(cid + sPhone + key).ToLower();
                        string sUrl = string.Format("{0}?cid={1}&ph={2}&sign={3}", UpConfig.MOBILE_QUERY_URL, cid, sPhone, sign);
                        string result = HttpHelper.HttpGet(sUrl);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(result);
                    }
                    else
                    {//获取产品接口
                        string sPhone = context.Request["sPhone"];
                        string businessType = context.Request["businessType"];
                        string cid = UpConfig.API_UID;
                        string key = UpConfig.API_KEY;
                        string sign = TenPayHelp.MD5(cid + sPhone + businessType + key).ToLower();
                        string sUrl = string.Format("{0}?cid={1}&ph={2}&ct={3}&sign={4}", UpConfig.PRODUCT_QUERY_URL, cid, sPhone, businessType, sign);
                        string result = HttpHelper.HttpGet(sUrl);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(result);
                    }
                }
                else
                {
                    var res = new { code = 200, msg = "获取配置信息失败" };
                    context.Response.ContentType = "text/plain";
                    context.Response.Write(JsonConvert.SerializeObject(res));
                }
            }
            catch (Exception e)
            {
                logger.Info(e.Message);
                logger.Fatal(e.Message,e);
                var res = new { code = 200, msg = "服务错误" };
                context.Response.ContentType = "text/plain";
                context.Response.Write(JsonConvert.SerializeObject(res));
            }
         
        }

        /// <summary>
        /// 获取上游相关配置信息
        /// </summary>
        /// <returns></returns>
        public UpChannelConfig GetUpConfig()
        {
            var down_channel_no = ConfigurationManager.AppSettings["jsyh_channel_no"];//获取下游渠道编号
            logger.Info(down_channel_no);
            string sSql = string.Format(@"select * from UP_CHANNEL_CONFIG t where t.DOWN_CHANNEL_NO={0}", down_channel_no);
            DataTable dt = SqlHelper.GetDataTable(sSql);
            if (dt.Rows.Count == 1)
            {
                string res = JsonConvert.SerializeObject(dt).TrimStart('[').TrimEnd(']');
                logger.Info(res);
                var config = JsonConvert.DeserializeObject<UpChannelConfig>(res);
                return config;
            }
            else
            {
                return null;
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