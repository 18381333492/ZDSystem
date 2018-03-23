using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PayComon;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ADSystem.API.HWServer.Model;
using System.Data;
using Lib4Net.Logs;
using System.Configuration;

namespace ADSystem.API.HWServer
{
    /// <summary>
    /// 查询号码归属地
    /// </summary>
    public class MobileQuery : IHttpHandler
    {
        private static readonly ILogger logger = LoggerManager.Instance.GetLogger("HWMobileQuery");

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
                        string rm = context.Request["rm"] == null ? "0" : context.Request["rm"];
                        string sPhone = context.Request["sPhone"];
                        string businessType = context.Request["businessType"];//业务类型(2=手机 5=全国流量 5=省内流量)
                        string cid = UpConfig.API_UID;
                        string key = UpConfig.API_KEY;
                        string sign = TenPayHelp.MD5(cid + sPhone + businessType + key).ToLower();
                        string sUrl = string.Format("{0}?cid={1}&ph={2}&ct={3}&sign={4}&rm={5}", UpConfig.PRODUCT_QUERY_URL, cid, sPhone, businessType, sign,rm);
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
            var down_channel_no = ConfigurationManager.AppSettings["hw_channel_no"];//获取下游渠道编号
            logger.Info(down_channel_no);
            string sSql = string.Format(@"select * from UP_CHANNEL_CONFIG t where t.DOWN_CHANNEL_NO={0}", down_channel_no);
            DataTable dt=SqlHelper.GetDataTable(sSql);
            if (dt.Rows.Count==1)
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