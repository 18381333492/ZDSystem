using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lib4Net.Logs;
using Newtonsoft.Json;

namespace ADSystem.API
{
    /// <summary>
    /// 验证码相关
    /// </summary>
    public class CodeHelper : IHttpHandler
    {
        private static readonly ILogger logger = LoggerManager.Instance.GetLogger("HWCodeHelper");
        public void ProcessRequest(HttpContext context)
        {
            var res = new result();
            try
            {
                int FuncType = Convert.ToInt32(context.Request["FuncType"]);//1-获取 2验证
                string sPhone = context.Request["phone"];//获取手机号码
                res.success = true;
                if (FuncType == 1)
                {
                    string code = new Random().Next(11111, 99999).ToString();
                    res.success = CheckCode.Send(sPhone, code);
                }
                else
                {
                    string code = context.Request["code"];
                    res.success = CheckCode.Check(sPhone, code);
                }
                context.Response.ContentType = "text/plain";
                context.Response.Write(JsonConvert.SerializeObject(res));
            }
            catch (Exception e)
            {
                logger.Info(e.Message);
                logger.Fatal(e.Message, e);
                res.success = false;
                res.info = "Exception";
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

    public class result
    {
        public bool success;
        public string info;
    }
}