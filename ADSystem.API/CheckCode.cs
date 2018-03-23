using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oracle.ManagedDataAccess.Client;

namespace ADSystem.API
{
    public class CheckCode
    {
        public static readonly string CodeKey = "SHFW:Query:CheckCode:";
        public static bool Send(string phone, string code)
        {
            List<OracleParameter> parList = new List<OracleParameter>();
            parList.Add(new OracleParameter("v_partner_id", OracleDbType.Varchar2) { Value = "CN3" });
            parList.Add(new OracleParameter("v_order_no", OracleDbType.Varchar2) { Value = Guid.NewGuid().ToString("N") });
            parList.Add(new OracleParameter("v_title", OracleDbType.Varchar2) { Value = "生活服务查询验证码" });
            parList.Add(new OracleParameter("v_content", OracleDbType.Varchar2) { Value = "【手机充值】您的生活服务查询验证码:" + code });
            parList.Add(new OracleParameter("v_level", OracleDbType.Decimal) { Value = 5 });
            parList.Add(new OracleParameter("v_receive_no", OracleDbType.Varchar2) { Value = phone });
            parList.Add(new OracleParameter("v_business_type", OracleDbType.Decimal) { Value = 3 });
            parList.Add(new OracleParameter("v_out_status", OracleDbType.Decimal) { Direction = System.Data.ParameterDirection.Output });
            parList.Add(new OracleParameter("v_out_msg", OracleDbType.Varchar2, 2048) { Direction = System.Data.ParameterDirection.Output });
            SqlHelper.ExecuteNonQuery("st_sp_sms_create", System.Data.CommandType.StoredProcedure, parList.ToArray());
            if (parList[parList.Count - 2].Value.ToString() == "100")
            {
                RedisHelper.SetCache(CodeKey + phone, code);
                return true;
            }
            return false;
        }

        public static bool Check(string phone, string code)
        {
            if (RedisHelper.GetCache(CodeKey + phone).Equals(code, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }
    }
}