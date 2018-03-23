using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using Lib4Net.Logs;
using Oracle.ManagedDataAccess.Client;
using SecurityUtil;

namespace ADSystem.API
{
    public static class ApiHelper
    {
        private static readonly string pub_key = ConfigurationManager.AppSettings["pub_key"];
       // private static readonly string es_key = ConfigurationManager.AppSettings["es_key"];

        public static bool CheckOrderParams(dynamic obj)
        {
            if (string.IsNullOrEmpty(obj.detailurl + "") || string.IsNullOrEmpty(obj.payway + "") || string.IsNullOrEmpty(obj.notifyurl + "") || string.IsNullOrEmpty(obj.mobile + "") || string.IsNullOrEmpty(obj.money + "") || string.IsNullOrEmpty(obj.productid + "") || string.IsNullOrEmpty(obj.price + "") || string.IsNullOrEmpty(obj.sign + ""))
            {
                return false;
            }
            return true;
        }

        public static bool CheckRefundParams(dynamic obj)
        {
            if (string.IsNullOrEmpty(obj.orderid + "") || string.IsNullOrEmpty(obj.detailurl + "") || string.IsNullOrEmpty(obj.mobile + "") || string.IsNullOrEmpty(obj.price + "") || string.IsNullOrEmpty(obj.sign + ""))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 根据订单号获取ES_KEY
        /// </summary>
        /// <param name="orderno"></param>
        /// <returns></returns>
        public static string Get_es_key(string api_uid)
        {
            string sSql = string.Format("SELECT API_KEY FROM UP_CHANNEL_CONFIG t WHERE t.api_uid=:api_uid ");
            OracleParameter[] opams = { 
                    new OracleParameter(":api_uid",api_uid)};
            var dt = SqlHelper.GetDataTable(sSql, CommandType.Text, opams);
            if (dt.Rows.Count == 1)
            {
                return Convert.ToString(dt.Rows[0][0]);
            }
            else
            {
                return string.Empty;
            }
        }


        public static RevOrderResult SaveOrder(dynamic obj, ILogger log)
        {
            int business_type=Convert.ToInt32(obj.cardtype)==1?2:5;
            var down_channel_no = ConfigurationManager.AppSettings["zd_channel_no"];
            OracleParameter[] pars =  
            {
                        new OracleParameter(":flow_type", OracleDbType.Varchar2){Value=obj.userlogin},
                        new OracleParameter(":partner_order_no",OracleDbType.Varchar2){Value=obj.detailurl},
                        new OracleParameter(":card_type", OracleDbType.Decimal){Value=obj.cardtype},
                        new OracleParameter(":pay_type", OracleDbType.Decimal){Value=obj.payway},
                        new OracleParameter(":product_name", OracleDbType.Varchar2){Value=obj.prodname},
                        new OracleParameter(":notify_url", OracleDbType.Varchar2){Value=obj.notifyurl},
                        new OracleParameter(":mobile", OracleDbType.Varchar2){Value=obj.mobile},
                        new OracleParameter(":account", OracleDbType.Varchar2){Value=obj.account},
                        new OracleParameter(":product_id", OracleDbType.Varchar2){Value=obj.productid},
                        new OracleParameter(":face", OracleDbType.Decimal){Value=obj.money},
                        new OracleParameter(":price", OracleDbType.Decimal){Value=Convert.ToDecimal(obj.price+"")/100},
                        new OracleParameter(":couponprice", OracleDbType.Decimal){Value=obj.couponprice},
                        new OracleParameter(":couponid", OracleDbType.Varchar2){Value=obj.couponid},
                        new OracleParameter(":couponch", OracleDbType.Varchar2){Value=obj.couponch},
                        new OracleParameter(":ordertime", OracleDbType.Varchar2){Value=obj.ordertime},
                        new OracleParameter(":v_user_ip", OracleDbType.Varchar2){Value=obj.userip},
                        new OracleParameter(":v_down_channel_no", OracleDbType.Varchar2){Value=down_channel_no},
                        new OracleParameter(":v_business_type", OracleDbType.Decimal){Value=business_type},
                        new OracleParameter(":out_status", OracleDbType.Decimal){ Direction= ParameterDirection.Output},
                        new OracleParameter(":out_msg", OracleDbType.Varchar2,1024){Direction= ParameterDirection.Output},
                        new OracleParameter(":out_order_no", OracleDbType.Varchar2,64){ Direction= ParameterDirection.Output},
                        new OracleParameter(":out_payed_fee", OracleDbType.Decimal){ Direction= ParameterDirection.Output},
                        new OracleParameter(":out_sync_url", OracleDbType.Varchar2,1024){ Direction= ParameterDirection.Output},
                        new OracleParameter(":out_nonsync_url", OracleDbType.Varchar2,1024){ Direction= ParameterDirection.Output},
                        new OracleParameter(":out_appid", OracleDbType.Varchar2,128){ Direction= ParameterDirection.Output},
                        new OracleParameter(":out_account_type", OracleDbType.Decimal){ Direction= ParameterDirection.Output}
            };

            log.Info("输入参数:");
            foreach (var item in pars.Where(v => v.Direction != ParameterDirection.Output).ToList())
            {
                log.Info(string.Format("{0}:{1}", item.ParameterName, item.Value));
            }

            SqlHelper.ExecuteNonQuery("sp_recv_order", CommandType.StoredProcedure, pars);

            log.Info("输出参数:");
            foreach (var item in pars.Where(v => v.Direction == ParameterDirection.Output).ToList())
            {
                log.Info(string.Format("{0}:{1}", item.ParameterName, item.Value));
            }

            RevOrderResult result = new RevOrderResult();
            result.Status = pars[pars.Length - 8].Value.ToString() == "100";
            result.ResultMsg = pars[pars.Length - 7].Value.ToString();
            if (result.Status)
            {
                result.AccountType = Convert.ToInt32(pars[pars.Length - 1].Value.ToString());
                result.AppId = pars[pars.Length - 2].Value.ToString();
                result.AsyncUrl = pars[pars.Length - 3].Value.ToString();
                result.SyncUrl = pars[pars.Length - 4].Value.ToString();
                result.PayFee = Convert.ToDecimal(pars[pars.Length - 5].Value.ToString());
                result.OrderNo = pars[pars.Length - 6].Value.ToString();
            }
            return result;
        }

        public static QueryAccountResult QueryPayAccount(string appid, int account_type, ILogger log)
        {
            OracleParameter[] pars =  
            {
                        new OracleParameter(":appid", OracleDbType.Varchar2){Value=appid},
                        new OracleParameter(":account_type", OracleDbType.Decimal){Value=account_type},
                        new OracleParameter(":out_code", OracleDbType.Decimal){ Direction= ParameterDirection.Output},
                        new OracleParameter(":out_appid", OracleDbType.Varchar2,128){ Direction= ParameterDirection.Output},
                        new OracleParameter(":out_mchid", OracleDbType.Varchar2,128){ Direction= ParameterDirection.Output},
                        new OracleParameter(":out_pubkey", OracleDbType.Varchar2,1024){ Direction= ParameterDirection.Output},
                        new OracleParameter(":out_prikey", OracleDbType.Varchar2,2048){ Direction= ParameterDirection.Output},
                        new OracleParameter(":cert_path", OracleDbType.Varchar2,1024){ Direction= ParameterDirection.Output},
                        new OracleParameter(":ext1", OracleDbType.Varchar2,1024){ Direction= ParameterDirection.Output},
                        new OracleParameter(":ext2", OracleDbType.Varchar2,1024){ Direction= ParameterDirection.Output},
                        new OracleParameter(":ext3", OracleDbType.Varchar2,1024){ Direction= ParameterDirection.Output},
                        new OracleParameter(":ext4", OracleDbType.Varchar2,1024){ Direction= ParameterDirection.Output},
                        new OracleParameter(":ext5", OracleDbType.Varchar2,1024){ Direction= ParameterDirection.Output}
            };

            log.Info("输入参数:");
            foreach (var item in pars.Where(v => v.Direction != ParameterDirection.Output).ToList())
            {
                log.Info(string.Format("{0}:{1}", item.ParameterName, item.Value));
            }

            SqlHelper.ExecuteNonQuery("sp_receipt_account_get", CommandType.StoredProcedure, pars);

            log.Info("输出参数:");
            foreach (var item in pars.Where(v => v.Direction == ParameterDirection.Output).ToList())
            {
                log.Info(string.Format("{0}:{1}", item.ParameterName, item.Value));
            }

            QueryAccountResult result = new QueryAccountResult();
            result.Status = pars[pars.Length - 11].Value.ToString() == "100";
            if (result.Status)
            {
                result.Ext5 = pars[pars.Length - 1].Value.ToString();
                result.Ext4 = pars[pars.Length - 2].Value.ToString();
                result.Ext3 = pars[pars.Length - 3].Value.ToString();
                result.Ext2 = pars[pars.Length - 4].Value.ToString();
                result.Ext1 = pars[pars.Length - 5].Value.ToString();
                result.CertPath = pars[pars.Length - 6].Value.ToString();
                result.Prikey = pars[pars.Length - 7].Value.ToString();
                result.Pubkey = pars[pars.Length - 8].Value.ToString();
                result.Mchid = pars[pars.Length - 9].Value.ToString();
                result.Appid = pars[pars.Length - 10].Value.ToString();
            }
            return result;
        }

        public static bool SetFail(string order_no, string remark, ILogger log)
        {
            OracleParameter[] pars =  {
                                            new OracleParameter(":order_no", OracleDbType.Varchar2){Value=order_no},
                                            new OracleParameter(":result", OracleDbType.Decimal){Value=2},
                                            new OracleParameter(":user", OracleDbType.Varchar2){Value="_sys"},
                                            new OracleParameter(":remark", OracleDbType.Varchar2){Value=remark},
                                            new OracleParameter(":status",OracleDbType.Decimal){ Direction= ParameterDirection.Output},
                                            new OracleParameter(":msg",OracleDbType.Varchar2,1024){ Direction= ParameterDirection.Output}
                                       };

            log.Info("输入参数:");
            foreach (var item in pars.Where(v => v.Direction != ParameterDirection.Output).ToList())
            {
                log.Info(string.Format("{0}:{1}", item.ParameterName, item.Value));
            }

            SqlHelper.ExecuteNonQuery("sp_pay_manual", CommandType.StoredProcedure, pars);

            log.Info("输出参数:");
            foreach (var item in pars.Where(v => v.Direction == ParameterDirection.Output).ToList())
            {
                log.Info(string.Format("{0}:{1}", item.ParameterName, item.Value));
            }

            if (pars[pars.Length - 2].Value.ToString() == "100")
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public static bool SaveRefund(string orderNo, string partner_order_no, decimal refundFace, ILogger log, out string msg)
        {
            OracleParameter[] pars = { 
                                        new OracleParameter(":order_no", OracleDbType.Varchar2){Value=orderNo},
                                        new OracleParameter(":partner_order_no", OracleDbType.Varchar2){Value=partner_order_no},
                                        new OracleParameter(":refund_fee", OracleDbType.Decimal){Value=refundFace},
                                        new OracleParameter(":operator", OracleDbType.Varchar2){Value="_sys"},
                                        new OracleParameter(":remark", OracleDbType.Varchar2){Value="中大主动发起退款"},
                                        new OracleParameter(":out_refund_id",OracleDbType.Varchar2,64){ Direction= ParameterDirection.Output},
                                        new OracleParameter(":out_status",OracleDbType.Decimal){ Direction= ParameterDirection.Output},
                                        new OracleParameter(":out_msg",OracleDbType.Varchar2,1024){ Direction= ParameterDirection.Output}
                                     };
            log.Info("输入参数:");
            foreach (var item in pars.Where(v => v.Direction != ParameterDirection.Output).ToList())
            {
                log.Info(string.Format("{0}:{1}", item.ParameterName, item.Value));
            }

            SqlHelper.ExecuteNonQuery("sp_refund_create", CommandType.StoredProcedure, pars);

            log.Info("输出参数:");
            foreach (var item in pars.Where(v => v.Direction == ParameterDirection.Output).ToList())
            {
                log.Info(string.Format("{0}:{1}", item.ParameterName, item.Value));
            }

            msg = pars[pars.Length - 1].Value.ToString();
            if (pars[pars.Length - 2].Value.ToString() == "100")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool CheckRefundSign(dynamic obj, ILogger log)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("detailurl", obj.detailurl + "");
            dict.Add("orderid", obj.orderid + "");
            dict.Add("mobile", obj.mobile + "");
            dict.Add("price", obj.price + "");

            string origin_str = CreateSignStr(dict);

            if (!SecurityCore.VerifyFromBase64(origin_str, pub_key, obj.sign, "SHA256withRSA", "utf-8"))
            {
                log.Info(string.Format("签名失败,签名原串:{0}", origin_str));
                return false;
            }
            return true;
        }

        public static bool CheckOrderSign(dynamic obj, ILogger log)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("userlogin", obj.userlogin + "");
            dict.Add("detailurl", obj.detailurl + "");
            dict.Add("cardtype", obj.cardtype + "");
            dict.Add("payway", obj.payway + "");
            dict.Add("prodname", obj.prodname + "");
            dict.Add("notifyurl", obj.notifyurl + "");
            dict.Add("mobile", obj.mobile + "");
            dict.Add("account", obj.account + "");
            dict.Add("productid", obj.productid + "");
            dict.Add("money", obj.money + "");
            dict.Add("price", obj.price + "");
            dict.Add("couponprice", obj.couponprice + "");
            dict.Add("couponid", obj.couponid + "");
            dict.Add("couponch", obj.couponch + "");
            dict.Add("ordertime", obj.ordertime + "");
            dict.Add("userip", obj.userip + "");

            string origin_str = CreateSignStr(dict);

            if (!SecurityCore.VerifyFromBase64(origin_str, pub_key, obj.sign, "SHA256withRSA", "utf-8"))
            {
                log.Info(string.Format("签名失败,签名原串:{0}", origin_str));
                return false;
            }
            return true;
        }

        private static string CreateSignStr(Dictionary<string, string> dict)
        {
            var sort_dict = dict.OrderBy(a => { return a.Key; });
            StringBuilder sb = new StringBuilder();
            foreach (var item in sort_dict)
            {
                sb.AppendFormat("{0}={1}&", item.Key, item.Value);
            }
            return sb.ToString().TrimEnd('&');
        }


        #region 18回调
        /// <summary>
        /// sp_delivery_save 保存18Esalse通知结果
        /// </summary>
        /// <param name="deliveryId">订单号</param>
        /// <param name="upOrderNo">上游订单号</param>
        /// <param name="sucFace">成功金额</param>
        /// <param name="msg">结果消息</param>
        /// <param name="status">充值结果编码</param>
        /// <returns></returns>
        public static bool SaveNotify(long? deliveryId, string upOrderNo, decimal? sucFace, string msg, int? status, decimal? cost_price, decimal? price, ILogger logger)
        {
            string paraMsg = string.Format(@"sp_delivery_save输入参数：
                                                deliveryId={0},
                                                upOrderNo={1},
                                                sucFace={2},
                                                msg={3},
                                                status={4},
                                                cost_price={5},
                                                price={6}", deliveryId, upOrderNo, sucFace, msg, status, cost_price, price);
            logger.Info(paraMsg);
            try
            {
                OracleParameter[] pars = { 
                                        new OracleParameter(":order_no", OracleDbType.Decimal){Value=deliveryId},
                                        new OracleParameter(":up_order_no", OracleDbType.Varchar2){Value=upOrderNo},
                                        new OracleParameter(":suc_face", OracleDbType.Decimal){Value=sucFace},
                                        new OracleParameter(":msg", OracleDbType.Varchar2){Value=msg},
                                        new OracleParameter(":result",OracleDbType.Decimal){Value=status},
                                        new OracleParameter(":cost_price",OracleDbType.Decimal){Value=cost_price},
                                        new OracleParameter(":price",OracleDbType.Decimal){Value=price},
                                        new OracleParameter(":out_status",OracleDbType.Decimal){ Direction= ParameterDirection.Output},
                                        new OracleParameter(":out_msg",OracleDbType.Varchar2,1024){ Direction= ParameterDirection.Output}
                                     };
                SqlHelper.ExecuteNonQuery("sp_delivery_save", CommandType.StoredProcedure, pars);
                string outPutStr = string.Format(@"sp_delivery_save输出参数：
                                                        out_status={0},
                                                        out_msg={1}", pars[pars.Length - 2].Value.ToString(), pars[pars.Length - 1].Value.ToString());
                logger.Info(outPutStr);
                if (pars[pars.Length - 2].Value.ToString() == "100")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                string rstMsg = ex.Message;
                return false;
            }

        }

        /// <summary>
        /// 18Esalse回调签名校验
        /// </summary>
        /// <param name="obj">数据对象</param>
        /// <param name="key">签名Key</param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static bool EsalseNotifyCheckSign(dynamic obj, ILogger logger)
        {
            string es_key = Get_es_key(obj.cid).Trim();
            string origin_str = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}", obj.sid, obj.ste, obj.cid, obj.pid, obj.oid, obj.pn, obj.tf, obj.fm, es_key);
            logger.Info("es_key:" + es_key);
            string sign = SecurityCore.ToHex(SecurityCore.MD5(origin_str, Encoding.GetEncoding("GBK")), true).ToLower();
            if (!sign.Equals(obj.sign, StringComparison.OrdinalIgnoreCase))
            {
                logger.Info(string.Format("签名失败,签名原串:{0},签名:{1},源签名:{2}", origin_str, sign, obj.sign));
                return false;
            }
            return true;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="model">参数对象</param>
        /// <param name="logger">日志对象</param>
        /// <returns>true：检查成功，false:检查失败</returns>
        public static bool CheckParams(NotifyDataModel model, ILogger logger)
        {
            if (string.IsNullOrEmpty(model.sid) || string.IsNullOrEmpty(model.ste) ||
                string.IsNullOrEmpty(model.cid) || string.IsNullOrEmpty(model.pid) ||
                string.IsNullOrEmpty(model.oid) || string.IsNullOrEmpty(model.pn) ||
                string.IsNullOrEmpty(model.tf) || string.IsNullOrEmpty(model.fm) ||
                    string.IsNullOrEmpty(model.sign))
            {

                StringBuilder sb = new StringBuilder();
                JavaScriptSerializer json = new JavaScriptSerializer();
                json.Serialize(model, sb);
                logger.Info("通知参数错误:必填参数为空,回调数据【" + sb.ToString() + "】");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取订单充值结果
        /// </summary>
        /// <param name="orderStatus"></param>
        /// <returns></returns>
        public static string GetCfgError(string orderStatus)
        {
            if (orderStatus == "0")
            {
                return "充值成功";
            }
            else if (orderStatus == "1")
            {
                return "充值失败";
            }
            return "";
        }

        /// <summary>
        /// 获取充值结果编码
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static int? GetRstStatus(string status)
        {
            if (status == "0")
            {
                return 1;
            }
            if (status == "1")
            {
                return 2;
            }
            return null;
        }
        #endregion

        #region 18esales退款
        public static bool Save18RefundApply(string orderNo, decimal? sucFace, ILogger logger)
        {
            string paraMsg = string.Format(@"sp_esales_refund输入参数：
                                                v_order_no={0},
                                                v_refund_fee={1}", orderNo, sucFace);
            logger.Info(paraMsg);
            try
            {
                OracleParameter[] pars = { 
                                        new OracleParameter(":v_deliver_id", OracleDbType.Varchar2){Value=orderNo},
                                        new OracleParameter(":v_refund_fee", OracleDbType.Decimal){Value=sucFace},
                                        new OracleParameter(":v_operator", OracleDbType.Varchar2){Value="__system"},
                                        new OracleParameter(":v_remark", OracleDbType.Varchar2){Value="18esales退款"},
                                        new OracleParameter(":out_refund_id",OracleDbType.Decimal){ Direction= ParameterDirection.Output},
                                        new OracleParameter(":out_status",OracleDbType.Decimal){ Direction= ParameterDirection.Output},
                                        new OracleParameter(":out_msg",OracleDbType.Varchar2,1024){ Direction= ParameterDirection.Output}
                                     };
                SqlHelper.ExecuteNonQuery("sp_esales_refund", CommandType.StoredProcedure, pars);
                string outPutStr = string.Format(@"sp_delivery_save输出参数：
                                                        out_status={1},
                                                        out_msg={2},
out_refund_id={0}", pars[pars.Length - 3].Value, pars[pars.Length - 2].Value, pars[pars.Length - 1].Value);
                logger.Info(outPutStr);
                if (pars[pars.Length - 2].Value.ToString() == "100")
                {
                    return true;
                }
                else
                {
                    logger.Error(pars[pars.Length - 1].Value.ToString());
                    return false;
                }
            }
            catch (Exception ex)
            {
                
                string rstMsg = ex.Message;
                logger.Error(rstMsg);
                logger.Info(rstMsg);
                return false;
            }

        }
        #endregion





        #region  支付异步回调和退款回调处理

        /// <summary>
        /// 支付异步回调处理
        /// </summary>
        /// <param name="sOrderNo">商户订单号</param>
        /// <param name="iState">支付状态</param>
        /// <returns></returns>
        public static bool WxPayNotifyHandle(string sOrderNo, bool iState, string transaction_id, string total_fee, string openid, ILogger logger)
        {
            var result = false;
            int v_result = iState == true ? 1 : 2;
            decimal totalfee = decimal.Parse(total_fee) / 100;
            logger.Info("调用的存储过程:sp_pay_save");
            logger.Info(string.Format(@"v_order_no={0},
                                        v_result:{1},
                                        v_up_order_no:{2},
                                        v_payed_fee:{3},
                                        v_pay_user:{4}", sOrderNo, v_result, transaction_id, totalfee, openid));
            OracleParameter[] Params =  
            {
                        new OracleParameter(":v_order_no", OracleDbType.Varchar2){Value=sOrderNo},
                        new OracleParameter(":v_result", OracleDbType.Decimal){Value=v_result},
                        new OracleParameter(":v_up_order_no", OracleDbType.Varchar2){Value=transaction_id},
                        new OracleParameter(":v_payed_fee", OracleDbType.Decimal){Value=totalfee},
                        new OracleParameter(":v_pay_user", OracleDbType.Varchar2){Value=openid},
                        new OracleParameter(":v_out_status", OracleDbType.Decimal){ Direction= ParameterDirection.Output},
                        new OracleParameter(":v_out_msg", OracleDbType.Varchar2,1024){Direction= ParameterDirection.Output}
            };
            SqlHelper.ExecuteNonQuery("sp_pay_save", CommandType.StoredProcedure, Params);
            if (Params[Params.Length - 2].Value.ToString() == "100")
            {//业务逻辑处理成功
                result = true;
            }
            logger.Info(string.Format("存储过程输出参数:v_out_status={0},v_out_msg={1}",
                        Params[Params.Length - 2].Value.ToString(),
                        Params[Params.Length - 1].Value.ToString()));
            return result;
        }


        /// <summary>
        /// 退款异步回调处理
        /// </summary>
        /// <param name="sRefundOrderNo">商户退款单号</param>
        /// <param name="refund_id">微信退款单号</param>
        /// <param name="iState">退款状态</param>
        /// <param name="iState">退款消息</param>
        /// <returns></returns>
        public static bool WxRefundNotifyHandle(string sRefundOrderNo, string refund_id, bool iState, string refundMsg, ILogger logger)
        {
            var result = false;
            int v_refund_code = iState == true ? 0 : 90;
            logger.Info("调用的存储过程:sp_refund_save");
            logger.Info(string.Format(@"v_refund_id={0},
                                        v_refund_no:{1},
                                        v_refund_code:{2},
                                        v_refund_msg:{3}", sRefundOrderNo, refund_id, v_refund_code, refundMsg));
            OracleParameter[] Params =  
            {
                        new OracleParameter(":v_refund_id", OracleDbType.Varchar2){Value=sRefundOrderNo},
                        new OracleParameter(":v_refund_no", OracleDbType.Varchar2){Value=refund_id},
                        new OracleParameter(":v_refund_code", OracleDbType.Decimal){Value=v_refund_code},
                        new OracleParameter(":v_refund_msg", OracleDbType.Varchar2){Value=refundMsg},
                        new OracleParameter(":v_out_status", OracleDbType.Decimal){ Direction= ParameterDirection.Output},
                        new OracleParameter(":v_out_msg", OracleDbType.Varchar2,1024){Direction= ParameterDirection.Output}
            };


            SqlHelper.ExecuteNonQuery("sp_refund_save", CommandType.StoredProcedure, Params);
            if (Params[Params.Length - 2].Value.ToString() == "100")
            {//业务逻辑处理成功
                result = true;
            }
            logger.Info(string.Format("存储过程输出参数:v_out_status={0},v_out_msg={1}",
                        Params[Params.Length - 2].Value.ToString(),
                        Params[Params.Length - 1].Value.ToString()));
            return result;
        }


        #endregion
    }
}