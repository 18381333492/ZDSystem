﻿using System;
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
    /// 逻辑接口:订单退款
    /// </summary>   
    public interface IOrderRefundHandler
    {
    
     /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        MOrderRefund GetData(string id);
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
        MOrderRefund GetSingleData(MOrderRefund query, string orderBy = "", MatchMode mmode = MatchMode.Exact,
            ConnectMode cmode = ConnectMode.And);

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        List<MOrderRefund> GetDataList(string orderBy = "");


        /// <summary>
        /// 获取指定条件的数据列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="orderBy">排序字段,{f:属性名} asc</param>
        /// <param name="mmode">字符串匹配模式 Exact：精确匹配 Vague：模糊匹配</param>
        /// <param name="cmode">条件连接字符串 And 或 OR</param>
        /// <returns></returns>
        List<MOrderRefund> GetDataList(MOrderRefund query,string orderBy="",MatchMode mmode=MatchMode.Exact,
            ConnectMode cmode=ConnectMode.And);


        /// <summary>
        /// 获取指定条件的数据集
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="orderBy">排序字段,{f:属性名} asc</param>
        /// <param name="mmode">字符串匹配模式 Exact：精确匹配 Vague：模糊匹配</param>
        /// <param name="cmode">条件连接字符串 And 或 OR</param>
        /// <returns></returns>
        DataSet GetDataSet(MOrderRefund query, string orderBy = "", MatchMode mmode = MatchMode.Exact,
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
        List<MOrderRefund> GetPagerDataList(MOrderRefund query,int pageSize,int pageIndex,out int totalCount, string orderBy = "", MatchMode mmode = MatchMode.Exact,
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
        DataSet GetPagerDataSet(MOrderRefund query, int pageSize, int pageIndex, out int totalCount, string orderBy = "", MatchMode mmode = MatchMode.Exact,
            ConnectMode cmode = ConnectMode.And);
        /// <summary>
        /// 添加或修改数据,id为空时为添加，否则为修改
        /// </summary>
        /// <param name="id">主键值</param>
        /// <param name="vo">实体数据</param>
        /// <returns></returns>
        IResult Save(string id, MOrderRefund vo);
        /// <summary>
        /// 添加或修改数据
        /// </summary>
        /// <param name="vo">实体数据</param>
        /// <returns></returns>
        IResult Save(MOrderRefund vo);
        /// <summary>
        /// 添加新数据
        /// </summary>
        /// <param name="vo">实体数据</param>
        /// <returns></returns>
        bool CreateNew(MOrderRefund vo);
        /// <summary>
        /// 根据主键值修改实体数据
        /// </summary>
        /// <param name="vo">实体数据</param>
        /// <returns></returns>
        bool Update(MOrderRefund vo);

        /// <summary>
        /// 获取指定条件的记录总数
        /// </summary>
        /// <param name="query">查询条件</param>
         /// <param name="mmode">字符串匹配模式 Exact：精确匹配 Vague：模糊匹配</param>
        /// <param name="cmode">条件连接字符串 And 或 OR</param>
        /// <returns></returns>
        int GetCount(MOrderRefund query, MatchMode mmode = MatchMode.Exact,
            ConnectMode cmode = ConnectMode.And);

        /// <summary>
        /// 根据模板名称获取第一行一列的值
        /// </summary>
        /// <param name="xmlTemplateName">模板名称</param>
        /// <param name="vo">实体</param>
        /// <returns></returns>
        object GetScalarByXmlTemplate(string xmlTemplateName, MOrderRefund vo);
        /// <summary>
        /// 根据模板获取实体信息
        /// </summary>
        /// <param name="xmlTemplateName">模板名称</param>
        /// <param name="vo">实体</param>
        /// <returns></returns>
        List<MOrderRefund> GetDataListByTemplate(string xmlTemplateName, MOrderRefund vo);
        /// <summary>
        /// 根据模版获取DataSet
        /// </summary>
        /// <param name="xmlTemplateName">模板名称</param>
        /// <param name="vo">实体</param>
        /// <returns></returns>
        DataSet GetDataSetByTemplate(string xmlTemplateName, MOrderRefund vo);
        /// <summary>
        /// 查询单实体信息(单实体数据)
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        MOrderRefund GetSingleDataByTemplate(string xmlTemplateName, MOrderRefund vo);
    }
}