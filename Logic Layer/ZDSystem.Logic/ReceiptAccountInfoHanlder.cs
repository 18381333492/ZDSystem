using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Lib4Net.Comm;
using Lib4Net.DB;
using Lib4Net.Core;
using ZDSystem.Entity;
using ZDSystem.Interfaces.Logic;
using ZDSystem.Interfaces.DBAccess;
using ZDSystem.DataAccessFactories;
using System.Collections;
using Oracle.DataAccess.Client;

namespace ZDSystem.Logic
{
    /// <summary>
    /// 逻辑处理：MReceiptAccountInfo(收款账户信息表)
    /// </summary>
    public class ReceiptAccountInfoHandler : IReceiptAccountInfoHandler
    {
        private IReceiptAccountInfoDataAccess dbAccess;
        public ReceiptAccountInfoHandler()
        {
            dbAccess = DataAccessFactory.Instance.GetProvider<IReceiptAccountInfoDataAccess>();
        }
        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        public MReceiptAccountInfo GetData(string id)
        {
            return dbAccess.GetData(id);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        public bool Delete(string id)
        {
            return dbAccess.Delete(id);
        }
        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="mmode">字符串匹配模式 Exact：精确匹配 Vague：模糊匹配</param>
        /// <param name="cmode">条件连接字符串 And 或 OR</param>
        /// <returns></returns>   
        public MReceiptAccountInfo GetSingleData(MReceiptAccountInfo query, string orderBy = "",
            MatchMode mmode = MatchMode.Exact,
            ConnectMode cmode = ConnectMode.And)
        {
            return dbAccess.GetSingleData(query, orderBy, mmode, cmode);
        }
         /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public List<MReceiptAccountInfo> GetDataList(string orderBy = "")
        {
            return dbAccess.GetDataList(dbAccess.EmptyEntity, orderBy);
        }

        /// <summary>
        /// 获取指定条件的数据列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="orderBy">排序字段,{f:属性名} asc</param>
        /// <param name="mmode">字符串匹配模式 Exact：精确匹配 Vague：模糊匹配</param>
        /// <param name="cmode">条件连接字符串 And 或 OR</param>
        /// <returns></returns>
        public List<MReceiptAccountInfo> GetDataList(MReceiptAccountInfo query, string orderBy = "", MatchMode mmode = MatchMode.Exact,
            ConnectMode cmode = ConnectMode.And)
        {
            return dbAccess.GetDataList(query, orderBy, mmode, cmode);
        }
        /// <summary>
        /// 获取指定条件的数据集
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="orderBy">排序字段,{f:属性名} asc</param>
        /// <param name="mmode">字符串匹配模式 Exact：精确匹配 Vague：模糊匹配</param>
        /// <param name="cmode">条件连接字符串 And 或 OR</param>
        /// <returns></returns>
        public DataSet GetDataSet(MReceiptAccountInfo query, string orderBy = "", MatchMode mmode = MatchMode.Exact,
              ConnectMode cmode = ConnectMode.And)
        {
            return dbAccess.GetDataSet(query, orderBy, mmode, cmode);
        }
        /// <summary>
        /// 获取分页数据列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="pageSize">分页</param>
        /// <param name="pageIndex">页索引从1开始</param>
        /// <param name="totalCount">总条数</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="mmode">字符串匹配模式 Exact：精确匹配 Vague：模糊匹配</param>
        /// <param name="cmode">条件连接字符串 And 或 OR</param>
        /// <returns></returns>
       public List<MReceiptAccountInfo> GetPagerDataList(MReceiptAccountInfo query, int pageSize, int pageIndex, out int totalCount, string orderBy = "", MatchMode mmode = MatchMode.Exact,
              ConnectMode cmode = ConnectMode.And)
        {
            totalCount = dbAccess.GetCount(query, mmode, cmode);
            return dbAccess.GetPagerDataList(query, pageSize, pageIndex, orderBy, mmode, cmode);

        }
        /// <summary>
        /// 获取分页数据集
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="pageSize">分页</param>
        /// <param name="pageIndex">页索引从1开始</param>
        /// <param name="totalCount">总条数</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="mmode">字符串匹配模式 Exact：精确匹配 Vague：模糊匹配</param>
        /// <param name="cmode">条件连接字符串 And 或 OR</param>
        /// <returns></returns>
       public DataSet GetPagerDataSet(MReceiptAccountInfo query, int pageSize, int pageIndex, out int totalCount, string orderBy = "", MatchMode mmode = MatchMode.Exact,
              ConnectMode cmode = ConnectMode.And)
        {
            totalCount = dbAccess.GetCount(query, mmode, cmode);
            return dbAccess.GetPagerData(query, pageSize, pageIndex, orderBy, mmode, cmode);

        }
        /// <summary>
        /// 添加或修改数据,id为空时为添加，否则为修改
        /// </summary>
        /// <param name="id">主键值</param>
        /// <param name="vo">实体数据</param>
        /// <returns></returns>
        public IResult Save(string id, MReceiptAccountInfo vo)
        {
            bool status=false;
            IResult result = new Result(status);
            if (string.IsNullOrEmpty(id))
                status= dbAccess.CreateNew(vo);
            else
                status= dbAccess.Update(id, vo);
           if (status)
            {
                result=new Result(true);
                result["id"] = CommFun.GetString(dbAccess.Builder.ODMapConfig.PrimaryKeyField.GetValue(vo));
            }
            return result;
        }
        /// <summary>
        /// 添加或修改数据,id为空时为添加，否则为修改
        /// </summary>
        /// <param name="vo">实体数据</param>
        /// <returns></returns>
        public IResult Save(MReceiptAccountInfo vo)
        {
            IResult result = new Result(false);
           bool status=dbAccess.Save(vo)>0;
            if (status)
            {
                result = new Result(true);
                result["id"] = CommFun.GetString(dbAccess.Builder.ODMapConfig.PrimaryKeyField.GetValue(vo));
            }
            return result;
        }
        /// <summary>
        /// 添加新数据
        /// </summary>
        /// <param name="vo">实体数据</param>
        /// <returns></returns>
        public bool CreateNew(MReceiptAccountInfo vo)
        {
            return dbAccess.CreateNew(vo);
        }
        /// <summary>
        /// 根据主键值修改实体数据
        /// </summary>
        /// <param name="vo">实体数据</param>
        /// <returns></returns>
        public bool Update(MReceiptAccountInfo vo)
        {
            return dbAccess.Update(vo);
        }

        /// <summary>
        /// 获取指定条件的记录总数
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="mmode">字符串匹配模式 Exact：精确匹配 Vague：模糊匹配</param>
        /// <param name="cmode">条件连接字符串 And 或 OR</param>
        /// <returns></returns>
        public int GetCount(MReceiptAccountInfo query, MatchMode mmode = MatchMode.Exact,
              ConnectMode cmode = ConnectMode.And)
        {
            return dbAccess.GetCount(query, mmode, cmode);
        }

        /// <summary>
        /// 根据模板名称获取第一行一列的值
        /// </summary>
        /// <param name="xmlTemplateName">模板名称</param>
        /// <param name="vo">实体</param>
        /// <returns></returns>
        public object GetScalarByXmlTemplate(string xmlTemplateName, MReceiptAccountInfo vo)
        {
            return dbAccess.GetScalarByTemplate(xmlTemplateName, vo);
        }

        /// <summary>
        /// 根据模板获取实体信息
        /// </summary>
        /// <param name="xmlTemplateName">模板名称</param>
        /// <param name="vo">实体</param>
        /// <returns></returns>
        public List<MReceiptAccountInfo> GetDataListByTemplate(string xmlTemplateName, MReceiptAccountInfo vo)
        {
            return dbAccess.GetDataListByTemplate(xmlTemplateName, vo);
        }
        /// <summary>
        /// 根据模版获取DataSet
        /// </summary>
        /// <param name="xmlTemplateName">模板名称</param>
        /// <param name="vo">实体</param>
        /// <returns></returns>
        public DataSet GetDataSetByTemplate(string xmlTemplateName, MReceiptAccountInfo vo)
        {
            return dbAccess.GetDataSetByTemplate(xmlTemplateName, vo);
        }
        /// <summary>
        /// 查询单实体信息(单实体数据)
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        public MReceiptAccountInfo GetSingleDataByTemplate(string xmlTemplateName, MReceiptAccountInfo vo)
        {
            return dbAccess.GetSingleDataByTemplate(xmlTemplateName, vo);
        }

        /// <summary>
        /// 用户提现
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Draw(MReceiptFundRecord model)
        {
            List<DbParameter> Params = new List<DbParameter>();
            Params.Add(new DbParameter(":v_account_id", model.AccountId));
            Params.Add(new DbParameter(":v_order_no", string.Empty));
            Params.Add(new DbParameter(":v_change_type", model.ChangeType));
            Params.Add(new DbParameter(":v_change_amount", model.ChangeAmount));
            Params.Add(new DbParameter(":v_card_type", model.ProductType));
            Params.Add(new DbParameter(":v_operator", model.Operator));
            Params.Add(new DbParameter(":v_remark", model.Remark));
            Params.Add(new DbParameter { Name = "v_code", Direction = System.Data.ParameterDirection.Output });
            var res=dbAccess.DbProvider.ExcuteProcToArray("sp_draw_account_info", Params.ToArray());
            if (Convert.ToInt32(res[0]) == 100)
                return true;
            else
                return false;
        }


        /// <summary>
        /// 统计账户信息
        /// </summary>
        /// <param name="start_date">开始时间</param>
        /// <param name="end_date">结束时间</param>
        /// <param name="channel_no">下游渠道编号</param>
        /// <param name="pay_type">支付类型</param>
        /// <returns></returns>
        public DataTable AccountCount(string start_date, string end_date, string channel_no, string pay_type, string card_type)
        {
            var data = new OracleParameter("v_out_data", OracleDbType.RefCursor, "", System.Data.ParameterDirection.Output);
            DataSet ds = dbAccess.DbProvider.GetDataSetByProcedure("sp_count_trade_per_day",
                new OracleParameter("v_start_date", start_date),
                new OracleParameter("v_end_date", end_date),
                new OracleParameter("v_channel_no", channel_no),
                new OracleParameter("v_pay_type", pay_type),
                new OracleParameter("v_card_type", card_type),
                data);
            if (ds.Tables.Count == 1)
            {
                DataTable dt = ds.Tables[0];
                DataTable newTable = new DataTable();
                newTable.Columns.Add("create_time");
                newTable.Columns.Add("down_channel_no");
                newTable.Columns.Add("card_type");
                newTable.Columns.Add("pay_type");
                newTable.Columns.Add("allprice");
                newTable.Columns.Add("allordernumber");
                newTable.Columns.Add("price");
                newTable.Columns.Add("successordernumber");
                newTable.Columns.Add("service_fee");
                newTable.Columns.Add("channel_name");
                newTable.Columns.Add("card_name");
                newTable.Columns.Add("pay_name");
                newTable.Columns.Add("refund_money");
                newTable.Columns.Add("refund_number");
                newTable.Columns.Add("loss_service");
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dr = newTable.NewRow();
                    int i = 0;
                    for (i = 0; i < dt.Columns.Count; i++)
                    {
                        dr[i] = row[i];
                    }
                    DataTable refundTable = GetRefundInfo(Convert.ToString(row["create_time"]), Convert.ToString(row["create_time"]), Convert.ToString(row["down_channel_no"]), Convert.ToString(row["pay_type"]), Convert.ToString(row["card_type"]));
                    for (var j = 0; j < refundTable.Columns.Count; j++)
                    {
                        dr[i + j] = refundTable.Rows[0][j];
                    }
                    newTable.Rows.Add(dr);
                }
                return newTable;
            }
            return null;
        }

        /// <summary>
        /// 获取退款的信息
        /// </summary>
        /// <param name="channel_no">渠道编号</param>
        /// <param name="pay_type">支付方式</param>
        /// <param name="card_type"></param>
        /// <returns></returns>
        public DataTable GetRefundInfo(string start_date, string end_date, string channel_no, string pay_type, string card_type)
        {
            start_date = Convert.ToDateTime(start_date).ToString("yyyy-MM-dd");
            end_date = start_date;
            var data = new OracleParameter("v_ret_data", OracleDbType.RefCursor, "", System.Data.ParameterDirection.Output);
            DataSet ds = dbAccess.DbProvider.GetDataSetByProcedure("sp_count_refund_info",
                new OracleParameter("v_start_date", start_date),
                new OracleParameter("v_end_date", end_date),
                new OracleParameter("v_channel_no", channel_no),
                new OracleParameter("v_pay_type", pay_type),
                new OracleParameter("v_card_type", card_type),
                data);
            if (ds.Tables.Count == 1)
            {
                var dt = ds.Tables[0];
                return dt;
            }
            else
            {
                return null;
            }
        }

    }
}