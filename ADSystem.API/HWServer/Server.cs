using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lib4Net.Logs;
using ADSystem.API.HWServer.Model;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using Newtonsoft.Json;
using System.Configuration;


namespace ADSystem.API.HWServer
{
    public class Server
    {
        //版本号 1开发 2测试 3预生产 4生产
        public static readonly string version = ConfigurationManager.AppSettings["version"];

        /// <summary>
        /// 根据订单编号获取订单数据
        /// </summary>
        /// <param name="orderno"></param>
        /// <returns></returns>
        public static HWOrder GetOrderInfo(string orderno,decimal pay_type)
        {
            string sql = @"SELECT 
                              t.order_no orderno,
                              t.account, 
                              t.pay_type, 
                              t.card_type, 
                              t.flow_type,
                              t.product_name,
                              t.mobile,
                              t.product_id,
                              t.price,
                              t.face,
                              t.business_type businessType
                              FROM order_main t
                              WHERE t.order_no=:sOrderNo";
            OracleParameter[] opams = { 
                    new OracleParameter(":sOrderNo",orderno)};
            var dt = SqlHelper.GetDataTable(sql, CommandType.Text, opams);
            if (dt.Rows.Count == 1)
            {
                HWOrder orderinfo = JsonConvert.DeserializeObject<HWOrder>(JsonConvert.SerializeObject(dt).TrimStart('[').TrimEnd(']'));
                orderinfo.pay_type = pay_type;//支付方式
                return orderinfo;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 创建充值订单
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static RevOrderResult SaveOrder(HWOrder orderInfo, ILogger log)
        {
            OracleParameter[] pars =  
            {
                        new OracleParameter(":flow_type", OracleDbType.Varchar2){Value=orderInfo.flow_type},
                        new OracleParameter(":partner_order_no",OracleDbType.Varchar2){Value=orderInfo.orderno},
                        new OracleParameter(":card_type", OracleDbType.Decimal){Value=orderInfo.card_type},
                        new OracleParameter(":pay_type", OracleDbType.Decimal){Value=orderInfo.pay_type},
                        new OracleParameter(":product_name", OracleDbType.Varchar2){Value=orderInfo.product_name},
                        new OracleParameter(":v_notify_url", OracleDbType.Varchar2){Value=string.Empty},
                        new OracleParameter(":mobile", OracleDbType.Varchar2){Value=orderInfo.mobile},
                        new OracleParameter(":account", OracleDbType.Varchar2){Value=orderInfo.account},
                        new OracleParameter(":product_id", OracleDbType.Varchar2){Value=orderInfo.product_id},
                        new OracleParameter(":face", OracleDbType.Decimal){Value=orderInfo.face},
                        new OracleParameter(":price", OracleDbType.Decimal){Value=orderInfo.price},
                        new OracleParameter(":couponprice", OracleDbType.Decimal){Value=0},
                        new OracleParameter(":couponid", OracleDbType.Varchar2){Value=string.Empty},
                        new OracleParameter(":couponch", OracleDbType.Varchar2){Value=string.Empty},
                        new OracleParameter(":ordertime", OracleDbType.Varchar2){Value=DateTime.Now.ToString("yyyyMMddHHmmss")},
                        new OracleParameter(":v_user_ip", OracleDbType.Varchar2){Value=orderInfo.user_ip},
                        new OracleParameter(":v_DOWN_CHANNEL_NO", OracleDbType.Varchar2){Value=orderInfo.down_channel_no},
                        new OracleParameter(":v_business_type", OracleDbType.Decimal){Value=orderInfo.businessType},
                        new OracleParameter(":out_status", OracleDbType.Decimal){ Direction= System.Data.ParameterDirection.Output},
                        new OracleParameter(":out_msg", OracleDbType.Varchar2,1024){Direction= System.Data.ParameterDirection.Output},
                        new OracleParameter(":out_order_no", OracleDbType.Varchar2,64){ Direction= System.Data.ParameterDirection.Output},
                        new OracleParameter(":out_payed_fee", OracleDbType.Decimal){ Direction= System.Data.ParameterDirection.Output},
                        new OracleParameter(":out_sync_url", OracleDbType.Varchar2,1024){ Direction= System.Data.ParameterDirection.Output},
                        new OracleParameter(":out_nonsync_url", OracleDbType.Varchar2,1024){ Direction= System.Data.ParameterDirection.Output},
                        new OracleParameter(":out_appid", OracleDbType.Varchar2,128){ Direction= System.Data.ParameterDirection.Output},
                        new OracleParameter(":out_account_type", OracleDbType.Decimal){ Direction= System.Data.ParameterDirection.Output}
            };
            log.Info("存储过程sp_recv_order输入参数:");
            foreach (var item in pars.Where(v => v.Direction != System.Data.ParameterDirection.Output).ToList())
            {
                log.Info(string.Format("{0}:{1}", item.ParameterName, item.Value));
            }
            SqlHelper.ExecuteNonQuery("sp_recv_order", System.Data.CommandType.StoredProcedure, pars);
            log.Info("存储过程sp_recv_order输出参数:");
            foreach (var item in pars.Where(v => v.Direction == System.Data.ParameterDirection.Output).ToList())
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
                if (string.IsNullOrEmpty(result.SyncUrl) || result.SyncUrl == "null") result.SyncUrl = null;
            }
            return result;
        }


        /// <summary>
        /// 创建华为手机Wap端慢充充值订单
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static RevOrderResult SaveSlowOrder(HWOrder orderInfo,ILogger log)
        {
            OracleParameter[] pars =  
            {
                        new OracleParameter(":flow_type", OracleDbType.Varchar2){Value=orderInfo.flow_type},
                        new OracleParameter(":partner_order_no",OracleDbType.Varchar2){Value=orderInfo.orderno},
                        new OracleParameter(":card_type", OracleDbType.Decimal){Value=orderInfo.card_type},
                        new OracleParameter(":pay_type", OracleDbType.Decimal){Value=orderInfo.pay_type},
                        new OracleParameter(":product_name", OracleDbType.Varchar2){Value=orderInfo.product_name},
                        new OracleParameter(":v_notify_url", OracleDbType.Varchar2){Value=string.Empty},
                        new OracleParameter(":mobile", OracleDbType.Varchar2){Value=orderInfo.mobile},
                        new OracleParameter(":account", OracleDbType.Varchar2){Value=orderInfo.account},
                        new OracleParameter(":product_id", OracleDbType.Varchar2){Value=orderInfo.product_id},
                        new OracleParameter(":face", OracleDbType.Decimal){Value=orderInfo.face},
                        new OracleParameter(":price", OracleDbType.Decimal){Value=orderInfo.price},
                        new OracleParameter(":couponprice", OracleDbType.Decimal){Value=0},
                        new OracleParameter(":couponid", OracleDbType.Varchar2){Value=string.Empty},
                        new OracleParameter(":couponch", OracleDbType.Varchar2){Value=string.Empty},
                        new OracleParameter(":ordertime", OracleDbType.Varchar2){Value=DateTime.Now.ToString("yyyyMMddHHmmss")},
                        new OracleParameter(":v_user_ip", OracleDbType.Varchar2){Value=orderInfo.user_ip},
                        new OracleParameter(":v_DOWN_CHANNEL_NO", OracleDbType.Varchar2){Value=orderInfo.down_channel_no},
                        new OracleParameter(":v_business_type", OracleDbType.Decimal){Value=orderInfo.businessType},
                        new OracleParameter(":v_recharge_mode", OracleDbType.Decimal){Value=orderInfo.recharge_mode},
                        new OracleParameter(":v_arg1", OracleDbType.Decimal){Value=version},
                        new OracleParameter(":v_arg2", OracleDbType.Decimal){Value=orderInfo.clientType},
                        new OracleParameter(":v_arg3", OracleDbType.Decimal){Value=orderInfo.recharge_mode},
                        new OracleParameter(":v_arg4", OracleDbType.Decimal){Value=orderInfo.recharge_mode},
                        new OracleParameter(":out_status", OracleDbType.Decimal){ Direction= System.Data.ParameterDirection.Output},
                        new OracleParameter(":out_msg", OracleDbType.Varchar2,1024){Direction= System.Data.ParameterDirection.Output},
                        new OracleParameter(":out_order_no", OracleDbType.Varchar2,64){ Direction= System.Data.ParameterDirection.Output},
                        new OracleParameter(":out_payed_fee", OracleDbType.Decimal){ Direction= System.Data.ParameterDirection.Output},
                        new OracleParameter(":out_sync_url", OracleDbType.Varchar2,1024){ Direction= System.Data.ParameterDirection.Output},
                        new OracleParameter(":out_nonsync_url", OracleDbType.Varchar2,1024){ Direction= System.Data.ParameterDirection.Output},
                        new OracleParameter(":out_appid", OracleDbType.Varchar2,128){ Direction= System.Data.ParameterDirection.Output},
                        new OracleParameter(":out_account_type", OracleDbType.Decimal){ Direction= System.Data.ParameterDirection.Output},
                        new OracleParameter(":v_out_arg1", OracleDbType.Varchar2,1024){ Direction= System.Data.ParameterDirection.Output},
                        new OracleParameter(":v_out_arg2", OracleDbType.Varchar2,1024){ Direction= System.Data.ParameterDirection.Output},
                        new OracleParameter(":v_out_arg3", OracleDbType.Varchar2,1024){ Direction= System.Data.ParameterDirection.Output},
                        new OracleParameter(":v_out_arg4", OracleDbType.Varchar2,1024){ Direction= System.Data.ParameterDirection.Output}
            };
            log.Info("存储过程sp_recv_order输入参数:");
            foreach (var item in pars.Where(v => v.Direction != System.Data.ParameterDirection.Output).ToList())
            {
                log.Info(string.Format("{0}:{1}", item.ParameterName, item.Value));
            }
            SqlHelper.ExecuteNonQuery("sp_recv_order_ii", System.Data.CommandType.StoredProcedure, pars);
            log.Info("存储过程sp_recv_order_ii输出参数:");
            foreach (var item in pars.Where(v => v.Direction == System.Data.ParameterDirection.Output).ToList())
            {
                log.Info(string.Format("{0}:{1}", item.ParameterName, item.Value));
            }
            RevOrderResult result = new RevOrderResult();
            result.Status = pars[pars.Length - 12].Value.ToString() == "100";
            result.ResultMsg = pars[pars.Length - 11].Value.ToString();
            if (result.Status)
            {
                result.AccountType = Convert.ToInt32(pars[pars.Length - 5].Value.ToString());
                result.AppId = pars[pars.Length - 6].Value.ToString();
                result.AsyncUrl = pars[pars.Length - 7].Value.ToString();
                result.SyncUrl = pars[pars.Length - 8].Value.ToString();
                result.PayFee = Convert.ToDecimal(pars[pars.Length - 9].Value.ToString());
                result.OrderNo = pars[pars.Length - 10].Value.ToString();
                if (string.IsNullOrEmpty(result.SyncUrl) || result.SyncUrl == "null") result.SyncUrl = null;
            }
            return result;
        }
    }
}