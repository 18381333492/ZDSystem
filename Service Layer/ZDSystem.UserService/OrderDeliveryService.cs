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
    /// 服务操作: OrderDelivery(订单发货表)
    /// </summary>
    public class OrderDeliveryService : Singleton<OrderDeliveryService>
    {
        private IOrderDeliveryHandler handler;
        private static readonly string ORDER_BY = "";
        public OrderDeliveryService()
        {
            handler = BusinessLogicFactory.Instance.GetProvider<IOrderDeliveryHandler>();
        }
        /// <summary>
        /// 查询单条数据,用于详细页面显示
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        public OrderDeliveryItemModel Query(string id)
        {
            OrderDeliveryItemModel model = new OrderDeliveryItemModel();
            model.CurrentModel = handler.GetData(id);
            model.Id = id;
            return model;
        }
        /// <summary>
        /// 查询单条数据，用于新增编辑
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        public OrderDeliveryItemModel QueryItem(string id)
        {
            OrderDeliveryItemModel model = new OrderDeliveryItemModel();
            model.CurrentModel = handler.GetData(id);
            model.Id = id;
            return model;
        }
        /// <summary>
        /// 查询单条数据，用于页面预览
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        public OrderDeliveryViewModel View(string id)
        {
            OrderDeliveryViewModel model = new OrderDeliveryViewModel();
            model.CurrentModel = handler.GetData(id);
            model.Id = id;
            return model;
        }
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <returns></returns>
        public List<MOrderDelivery> GetDataList()
        {
            return handler.GetDataList(ORDER_BY);
        }
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="json">json数据</param>
        /// <returns></returns>
        public List<MOrderDelivery> GetDataList(string json)
        {
            MOrderDelivery data =
                JsonData.JavaScriptDeserialize<MOrderDelivery>(json);
            return handler.GetDataList(data, ORDER_BY, Lib4Net.DB.MatchMode.Exact);
        }
        /// <summary>
        /// 查询列表数据
        /// </summary>
        /// <param name="nvc">参数集合</param>
        /// <returns></returns>
        public OrderDeliveryListModel Query(NameValueCollection nvc)
        {
            OrderDeliveryListModel model = new OrderDeliveryListModel();
            MOrderDelivery entity = new MOrderDelivery();
            model.PageSize = CommFun.ToInt(nvc["ps"],
                SettingHelper.Instance.GetInt("PageSize", 10)).Value;
            model.PageIndex = CommFun.ToInt(nvc["pi"],
                SettingHelper.Instance.GetInt("PageIndex", 0)).Value + 1;
            entity.SetData(nvc, false);
            entity.TrimEmptyProperty();
            entity.AddData(":PS", model.PageSize);
            entity.AddData(":PI", model.PageIndex);
            DateTime st = CommFun.ToDateTime(nvc["s"], DateTime.Now.AddDays(-1)).Value;
            DateTime et = CommFun.ToDateTime(nvc["t"], DateTime.Now).Value;
            entity.AddData("ST", st.ToString("yyyy-MM-dd"));
            entity.AddData("ET", et.ToString("yyyy-MM-dd"));
            string keyWord = CommFun.GetString(nvc["KeyWords"]);
            if (!string.IsNullOrEmpty(keyWord))
            {
                int? type = CommFun.ToInt(nvc["keytype"], null);
                switch (type)
                {
                    case 1:
                        entity.OrderNo = nvc["KeyWords"];
                        break;
                    case 2: entity.DeliveryId = CommFun.ToLong(nvc["KeyWords"], -1);
                        break;
                    case 3: entity.UpOrderNo = nvc["KeyWords"];
                        break;
                }
            }
            model.TotalCount = CommFun.ToInt(handler.GetScalarByXmlTemplate("getCount", entity), 0).GetValueOrDefault();
            if (model.TotalCount > 0)
                model.List = handler.GetDataListByTemplate("getList", entity);
            return model;
        }


        /// <summary>
        /// 订单页面详情页面 -- 获取发货记录
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public List<MOrderDelivery> QueryDeliveryListByOrderNo(string orderNo)
        {
            MOrderDelivery entity = new MOrderDelivery();
            entity.OrderNo = orderNo;
            return handler.GetDataListByTemplate("getDeliveryDetialsByOrderNo", entity);
        }


        public OrderDeliveryListModel ReviewQuery(NameValueCollection nvc)
        {
            OrderDeliveryListModel model = new OrderDeliveryListModel();
            MOrderDelivery entity = new MOrderDelivery();
            model.PageSize = CommFun.ToInt(nvc["ps"],
                SettingHelper.Instance.GetInt("PageSize", 10)).Value;
            model.PageIndex = CommFun.ToInt(nvc["pi"],
                SettingHelper.Instance.GetInt("PageIndex", 0)).Value + 1;
            entity.SetData(nvc, false);
            entity.TrimEmptyProperty();
            entity.AddData(":PS", model.PageSize);
            entity.AddData(":PI", model.PageIndex);
            DateTime st = CommFun.ToDateTime(nvc["s"], DateTime.Now.AddDays(-1)).Value;
            DateTime et = CommFun.ToDateTime(nvc["t"], DateTime.Now).Value;
            entity.AddData("ST", st.ToString("yyyy-MM-dd"));
            entity.AddData("ET", et.ToString("yyyy-MM-dd"));

            string keyWord =CommFun.GetString( nvc["KeyWords"]);
            if (!string.IsNullOrEmpty(keyWord))
            {
                int? type = CommFun.ToInt(nvc["keytype"], null);
                switch (type)
                {
                    case 1:
                        entity.OrderNo = nvc["KeyWords"];
                        break;
                    case 2: entity.DeliveryId = CommFun.ToLong(nvc["KeyWords"], -1);
                        break;
                    case 3: entity.UpOrderNo = nvc["KeyWords"];
                        break;
                }
            }
            model.TotalCount = CommFun.ToInt(handler.GetScalarByXmlTemplate("getReviewCount", entity), 0).GetValueOrDefault();
            if (model.TotalCount > 0)
                model.List = handler.GetDataListByTemplate("getReviewList", entity);
            return model;
        }

        public IResult OrderShipManaul(string user, NameValueCollection nvc)
        {
            string deliveryId = nvc["deliveryId"];
            string face = nvc["face"];
            string manaul = nvc["Manaul"];
            string remark = nvc["Remark"];
            return handler.OrderShipManaul(deliveryId, user, face, manaul, remark);
        }
    }
}