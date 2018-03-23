using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using Lib4Net.Core;
using Lib4Net.Framework.Settings;
using Lib4Net.Comm;
using Lib4Net.Data;
using ZDSystem.Entity;
using ZDSystem.Model;
using ZDSystem.Interfaces.Logic;
using ZDSystem.LogicFactories;
using ZDSystem.Utility;


namespace ZDSystem.UserService
{
    /// <summary>
    /// 服务操作: OrderRefund(订单退款)
    /// </summary>
    public class OrderRefundService : Singleton<OrderRefundService>
    {
        private IOrderRefundHandler handler;
        private static readonly string ORDER_BY = "";
        public OrderRefundService()
        {
            handler = BusinessLogicFactory.Instance.GetProvider<IOrderRefundHandler>();
        }
        /// <summary>
        /// 查询单条数据,用于详细页面显示
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        public OrderRefundItemModel Query(string id)
        {
            OrderRefundItemModel model = new OrderRefundItemModel();
            model.CurrentModel = handler.GetData(id);
            model.Id = id;
            return model;
        }
        /// <summary>
        /// 查询单条数据，用于新增编辑
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        public OrderRefundItemModel QueryItem(string id)
        {
            OrderRefundItemModel model = new OrderRefundItemModel();
            model.CurrentModel = handler.GetData(id);
            model.Id = id;
            return model;
        }
        /// <summary>
        /// 查询单条数据，用于页面预览
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        public OrderRefundViewModel View(string id)
        {
            OrderRefundViewModel model = new OrderRefundViewModel();
            model.CurrentModel = handler.GetData(id);
            model.Id = id;
            return model;
        }
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <returns></returns>
        public List<MOrderRefund> GetDataList()
        {
            return handler.GetDataList(ORDER_BY);
        }
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="json">json数据</param>
        /// <returns></returns>
        public List<MOrderRefund> GetDataList(string json)
        {
            MOrderRefund data =
                JsonData.JavaScriptDeserialize<MOrderRefund>(json);
            return handler.GetDataList(data, ORDER_BY, Lib4Net.DB.MatchMode.Exact);
        }
        /// <summary>
        /// 查询列表数据
        /// </summary>
        /// <param name="nvc">参数集合</param>
        /// <returns></returns>
        public OrderRefundListModel Query(NameValueCollection nvc)
        {
            OrderRefundListModel model = new OrderRefundListModel();
            MOrderRefund entity = new MOrderRefund();
            model.PageSize = CommFun.ToInt(nvc["ps"],
                SettingHelper.Instance.GetInt("PageSize", 10)).Value;
            model.PageIndex = CommFun.ToInt(nvc["pi"],
                SettingHelper.Instance.GetInt("PageIndex", 0)).Value + 1;
            entity.SetData(nvc, false);
            entity.TrimEmptyProperty();
            entity.AddData(":PS", model.PageSize);
            entity.AddData(":PI", model.PageIndex);
            DateTime st = CommFun.ToDateTime(nvc["s"], DateTime.Now.AddDays(-1)).Value;
            DateTime et = CommFun.ToDateTime(nvc["e"], DateTime.Now).Value;
            entity.AddData("ST", st.ToString("yyyy-MM-dd"));
            entity.AddData("ET", et.ToString("yyyy-MM-dd"));
            if (!string.IsNullOrEmpty(nvc["ReStatus"]))
            {
                entity.Status = CommFun.ToInt(nvc["ReStatus"], null);
            }
            if (!string.IsNullOrEmpty(nvc["KeyWords"]))
            {
                switch (nvc["keytype"])
                {
                    case "1": entity.OrderNo = CommFun.GetString(nvc["KeyWords"], null);
                        break;
                    case "2": entity.RecordId = CommFun.ToLong(nvc["KeyWords"], null);
                        break;
                    case "3": entity.RefundFee = CommFun.ToDecimal(nvc["KeyWords"], null);
                        break;
                }
            }
            model.TotalCount = CommFun.ToInt(handler.GetScalarByXmlTemplate("getCount", entity), 0).GetValueOrDefault();
            if (model.TotalCount > 0)
                model.List = handler.GetDataListByTemplate("getList", entity);
            return model;
        }
        /// <summary>
        /// 保存实体数据
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public IResult Save(string id, MOrderRefund entity)
        {

            return handler.Save(id, entity);
        }
        /// <summary>
        /// 删除单条数据
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        public IResult Delete(string id)
        {
            return new Result(handler.Delete(id));
        }

    }
}