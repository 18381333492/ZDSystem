using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using Codeplex.Data;
using Lib4Net.Logs;
using PayComon;
using System.Text.RegularExpressions;

namespace ADSystem.API
{
    /// <summary>
    /// Summary description for OrderRequest
    /// </summary>
    public class OrderRequest : IHttpHandler
    {
        private static readonly ILogger logger = LoggerManager.Instance.GetLogger("request");

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/json";

            string post_content = string.Empty;
            dynamic obj = null;
            try
            {
                using (StreamReader sr = new StreamReader(context.Request.InputStream, Encoding.UTF8))
                {
                    post_content = sr.ReadToEnd();
                }
                logger.Info("----------------收单开始---------------------");
                logger.Info("请求原串:" + post_content);


                try
                {
                    obj = DynamicJson.Parse(post_content);
                }
                catch
                {
                    logger.Info("解析数据失败");
                    Write(DynamicJson.Serialize(new { code = "9999", msg = "parse_fail", detailurl = "" }));
                    return;
                }

                if (!ApiHelper.CheckOrderParams(obj))
                {
                    logger.Info("参数错误");
                    Write(DynamicJson.Serialize(new { code = "9999", msg = "params_fail", detailurl = obj.detailurl }));
                    return;
                }

                if (!ApiHelper.CheckOrderSign(obj, logger))
                {
                    logger.Info("签名错误");
                    Write(DynamicJson.Serialize(new { code = "9999", msg = "sign_fail", detailurl = obj.detailurl }));
                    return;
                }

                RevOrderResult revOrderResult = ApiHelper.SaveOrder(obj, logger);
                logger.Info(string.Format("订单保存状态:{0},消息:{1}", revOrderResult.Status, revOrderResult.ResultMsg));
                if (revOrderResult.Status)
                {
                    logger.Info("订单保存成功");
                    QueryAccountResult accountResult = ApiHelper.QueryPayAccount(revOrderResult.AppId, revOrderResult.AccountType, logger);
                    if (!accountResult.Status)
                    {
                        logger.Info("获取支付信息失败");
                        Write(DynamicJson.Serialize(new { code = "9999", msg = "pay_fail", detailurl = obj.detailur }));
                        return;
                    }
                    string msg;
                    bool status = Pay(accountResult, revOrderResult, obj, out msg);
                    if (status)
                    {
                        logger.Info("收单成功");
                        string result = DynamicJson.Serialize(new { code = "0000", msg = "success", detailurl = obj.detailurl, orderid = revOrderResult.OrderNo, form =HttpUtility.UrlEncode(msg,Encoding.UTF8) });
                        Write(result);
                    }
                    else
                    {
                        logger.Info("生成支付信息失败");
                        Write(DynamicJson.Serialize(new { code = "9999", msg = msg, detailurl = obj.detailurl }));
                        ApiHelper.SetFail(revOrderResult.OrderNo, "支付失败" + msg, logger);
                    }
                }
                else
                {
                    logger.Info("订单保存失败");
                    Write(DynamicJson.Serialize(new { code = "9999", msg = revOrderResult.ResultMsg, detailurl = obj.detailurl }));
                    ApiHelper.SetFail(revOrderResult.OrderNo, revOrderResult.ResultMsg, logger);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex.Message, ex);
                Write(DynamicJson.Serialize(new { code = "9999", msg = "收单失败", detailurl = obj.detailurl }));
            }
        }

        public void Write(string msg)
        {
            logger.Info(msg);
            HttpContext.Current.Response.Write(msg);
        }


        public bool Pay(QueryAccountResult accountResult, RevOrderResult revOrderResult, dynamic obj, out string msg)
        {
            logger.Info(accountResult.Appid + "," + accountResult.Mchid + "," + accountResult.Pubkey + "," + accountResult.Prikey + "," + obj.userip + "," + revOrderResult.OrderNo + "," + obj.prodname + "," + revOrderResult.PayFee + "," + revOrderResult.AsyncUrl + "," + revOrderResult.SyncUrl + "," + revOrderResult.AccountType);
            Message message = PayRequest.Instance.bookOrder(new TenPayConfig() { appid = accountResult.Appid, mch_id = accountResult.Mchid, key = accountResult.Pubkey, merchant_private_key = accountResult.Prikey }, obj.userip, revOrderResult.OrderNo, obj.prodname, revOrderResult.PayFee, revOrderResult.AsyncUrl, revOrderResult.AccountType == 1 ? revOrderResult.SyncUrl : null, revOrderResult.AccountType);
            if (message.state)
            {
                msg = message.data;
                return true;
            }
            msg = message.error;
            return false;
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