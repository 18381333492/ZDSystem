using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using Oracle.ManagedDataAccess.Client;


namespace ADSystem.API
{
    public static class SqlHelper
    {
        static string constr = ConfigurationManager.ConnectionStrings["db_str"].ConnectionString;

        public static int ExecuteNonQuery(string sql, CommandType cmdType = CommandType.Text, params OracleParameter[] pars)
        {
            using (OracleConnection conn = new OracleConnection(constr))
            {
                using (OracleCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = cmdType;
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(pars);
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }


        public static object ExecuteScalar(string sql, CommandType cmdType = CommandType.Text, params OracleParameter[] pars)
        {
            using (OracleConnection conn = new OracleConnection(constr))
            {
                using (OracleCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = cmdType;
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(pars);
                    conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }

        public static DataTable GetDataTable(string sql, CommandType cmdType = CommandType.Text, params OracleParameter[] pars)
        {
            DataTable dt = new DataTable();
            using (OracleConnection conn = new OracleConnection(constr))
            {
                conn.Open();
                using (OracleCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.CommandType = cmdType;
                    cmd.Parameters.AddRange(pars);
                    using (OracleDataAdapter adapter = new OracleDataAdapter())
                    {
                        adapter.SelectCommand = cmd;
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public static OracleDataReader ExecuteReader(string sql, CommandType cmdType = CommandType.Text, params OracleParameter[] pars)
        {
            OracleConnection conn = new OracleConnection(constr);
            using (OracleCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = cmdType;
                cmd.CommandText = sql;
                cmd.Parameters.AddRange(pars);
                conn.Open();
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }
    }
}