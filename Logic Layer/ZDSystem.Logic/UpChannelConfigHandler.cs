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

namespace ZDSystem.Logic
{
    /// <summary>
    /// 逻辑处理：MUpChannelConfig(上游配置表)
    /// </summary>
    public class UpChannelConfigHandler : IUpChannelConfigHandler
    {
        private IUpChannelConfigDataAccess dbAccess;
        public UpChannelConfigHandler()
        {
            dbAccess = DataAccessFactory.Instance.GetProvider<IUpChannelConfigDataAccess>();
        }
        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        public MUpChannelConfig GetData(string id)
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
        public MUpChannelConfig GetSingleData(MUpChannelConfig query, string orderBy = "",
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
        public List<MUpChannelConfig> GetDataList(string orderBy = "")
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
        public List<MUpChannelConfig> GetDataList(MUpChannelConfig query, string orderBy = "", MatchMode mmode = MatchMode.Exact,
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
        public DataSet GetDataSet(MUpChannelConfig query, string orderBy = "", MatchMode mmode = MatchMode.Exact,
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
        public List<MUpChannelConfig> GetPagerDataList(MUpChannelConfig query, int pageSize, int pageIndex, out int totalCount, string orderBy = "", MatchMode mmode = MatchMode.Exact,
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
        public DataSet GetPagerDataSet(MUpChannelConfig query, int pageSize, int pageIndex, out int totalCount, string orderBy = "", MatchMode mmode = MatchMode.Exact,
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
        public IResult Save(string id, MUpChannelConfig vo)
        {
            bool status = false;
            IResult result = new Result(status);
            if (string.IsNullOrEmpty(id))
                status = dbAccess.CreateNew(vo);
            else
                status = dbAccess.Update(id, vo);
            if (status)
            {
                result = new Result(true);
                result["id"] = CommFun.GetString(dbAccess.Builder.ODMapConfig.PrimaryKeyField.GetValue(vo));
            }
            return result;
        }
        /// <summary>
        /// 添加或修改数据,id为空时为添加，否则为修改
        /// </summary>
        /// <param name="vo">实体数据</param>
        /// <returns></returns>
        public IResult Save(MUpChannelConfig vo)
        {
            IResult result = new Result(false);
            bool status = dbAccess.Save(vo) > 0;
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
        public bool CreateNew(MUpChannelConfig vo)
        {
            return dbAccess.CreateNew(vo);
        }
        /// <summary>
        /// 根据主键值修改实体数据
        /// </summary>
        /// <param name="vo">实体数据</param>
        /// <returns></returns>
        public bool Update(MUpChannelConfig vo)
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
        public int GetCount(MUpChannelConfig query, MatchMode mmode = MatchMode.Exact,
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
        public object GetScalarByXmlTemplate(string xmlTemplateName, MUpChannelConfig vo)
        {
            return dbAccess.GetScalarByTemplate(xmlTemplateName, vo);
        }

        /// <summary>
        /// 根据模板获取实体信息
        /// </summary>
        /// <param name="xmlTemplateName">模板名称</param>
        /// <param name="vo">实体</param>
        /// <returns></returns>
        public List<MUpChannelConfig> GetDataListByTemplate(string xmlTemplateName, MUpChannelConfig vo)
        {
            return dbAccess.GetDataListByTemplate(xmlTemplateName, vo);
        }
        /// <summary>
        /// 根据模版获取DataSet
        /// </summary>
        /// <param name="xmlTemplateName">模板名称</param>
        /// <param name="vo">实体</param>
        /// <returns></returns>
        public DataSet GetDataSetByTemplate(string xmlTemplateName, MUpChannelConfig vo)
        {
            return dbAccess.GetDataSetByTemplate(xmlTemplateName, vo);
        }
        /// <summary>
        /// 查询单实体信息(单实体数据)
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        public MUpChannelConfig GetSingleDataByTemplate(string xmlTemplateName, MUpChannelConfig vo)
        {
            return dbAccess.GetSingleDataByTemplate(xmlTemplateName, vo);
        }
    }
}