using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Lib4Net.DB;
using ZDSystem.Entity;
using Lib4Net.Comm;
namespace ZDSystem.Interfaces.Logic
{
    /// <summary>
    /// 逻辑接口:收款账户信息表
    /// </summary>   
    public interface IReceiptAccountInfoHandler
    {
    
     /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        MReceiptAccountInfo GetData(string id);
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        bool Delete(string id);
        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="mmode">字符串匹配模式 Exact：精确匹配 Vague：模糊匹配</param>
        /// <param name="cmode">条件连接字符串 And 或 OR</param>
        /// <returns></returns>
        MReceiptAccountInfo GetSingleData(MReceiptAccountInfo query, string orderBy = "", MatchMode mmode = MatchMode.Exact,
            ConnectMode cmode = ConnectMode.And);

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        List<MReceiptAccountInfo> GetDataList(string orderBy = "");


        /// <summary>
        /// 获取指定条件的数据列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="orderBy">排序字段,{f:属性名} asc</param>
        /// <param name="mmode">字符串匹配模式 Exact：精确匹配 Vague：模糊匹配</param>
        /// <param name="cmode">条件连接字符串 And 或 OR</param>
        /// <returns></returns>
        List<MReceiptAccountInfo> GetDataList(MReceiptAccountInfo query,string orderBy="",MatchMode mmode=MatchMode.Exact,
            ConnectMode cmode=ConnectMode.And);


        /// <summary>
        /// 获取指定条件的数据集
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="orderBy">排序字段,{f:属性名} asc</param>
        /// <param name="mmode">字符串匹配模式 Exact：精确匹配 Vague：模糊匹配</param>
        /// <param name="cmode">条件连接字符串 And 或 OR</param>
        /// <returns></returns>
        DataSet GetDataSet(MReceiptAccountInfo query, string orderBy = "", MatchMode mmode = MatchMode.Exact,
            ConnectMode cmode = ConnectMode.And);

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
        List<MReceiptAccountInfo> GetPagerDataList(MReceiptAccountInfo query,int pageSize,int pageIndex,out int totalCount, string orderBy = "", MatchMode mmode = MatchMode.Exact,
            ConnectMode cmode = ConnectMode.And);

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
        DataSet GetPagerDataSet(MReceiptAccountInfo query, int pageSize, int pageIndex, out int totalCount, string orderBy = "", MatchMode mmode = MatchMode.Exact,
            ConnectMode cmode = ConnectMode.And);
        /// <summary>
        /// 添加或修改数据,id为空时为添加，否则为修改
        /// </summary>
        /// <param name="id">主键值</param>
        /// <param name="vo">实体数据</param>
        /// <returns></returns>
        IResult Save(string id, MReceiptAccountInfo vo);
        /// <summary>
        /// 添加或修改数据
        /// </summary>
        /// <param name="vo">实体数据</param>
        /// <returns></returns>
        IResult Save(MReceiptAccountInfo vo);
        /// <summary>
        /// 添加新数据
        /// </summary>
        /// <param name="vo">实体数据</param>
        /// <returns></returns>
        bool CreateNew(MReceiptAccountInfo vo);
        /// <summary>
        /// 根据主键值修改实体数据
        /// </summary>
        /// <param name="vo">实体数据</param>
        /// <returns></returns>
        bool Update(MReceiptAccountInfo vo);

        /// <summary>
        /// 获取指定条件的记录总数
        /// </summary>
        /// <param name="query">查询条件</param>
         /// <param name="mmode">字符串匹配模式 Exact：精确匹配 Vague：模糊匹配</param>
        /// <param name="cmode">条件连接字符串 And 或 OR</param>
        /// <returns></returns>
        int GetCount(MReceiptAccountInfo query, MatchMode mmode = MatchMode.Exact,
            ConnectMode cmode = ConnectMode.And);

        /// <summary>
        /// 根据模板名称获取第一行一列的值
        /// </summary>
        /// <param name="xmlTemplateName">模板名称</param>
        /// <param name="vo">实体</param>
        /// <returns></returns>
        object GetScalarByXmlTemplate(string xmlTemplateName, MReceiptAccountInfo vo);
        /// <summary>
        /// 根据模板获取实体信息
        /// </summary>
        /// <param name="xmlTemplateName">模板名称</param>
        /// <param name="vo">实体</param>
        /// <returns></returns>
        List<MReceiptAccountInfo> GetDataListByTemplate(string xmlTemplateName, MReceiptAccountInfo vo);
        /// <summary>
        /// 根据模版获取DataSet
        /// </summary>
        /// <param name="xmlTemplateName">模板名称</param>
        /// <param name="vo">实体</param>
        /// <returns></returns>
        DataSet GetDataSetByTemplate(string xmlTemplateName, MReceiptAccountInfo vo);
        /// <summary>
        /// 查询单实体信息(单实体数据)
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        MReceiptAccountInfo GetSingleDataByTemplate(string xmlTemplateName, MReceiptAccountInfo vo);

        /// <summary>
        /// 用户提现
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool Draw(MReceiptFundRecord model);

        /// <summary>
       /// 统计账户信息
       /// </summary>
       /// <param name="start_date">开始时间</param>
       /// <param name="end_date">结束时间</param>
       /// <param name="channel_no">下游渠道编号</param>
       /// <param name="pay_type">支付类型</param>
       /// <returns></returns>
        DataTable AccountCount(string start_date, string end_date, string channel_no, string pay_type, string card_type);

    }
}