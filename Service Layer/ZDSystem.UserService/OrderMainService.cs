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
    /// 服务操作: OrderMain(订单主表)
    /// </summary>
    public class OrderMainService : Singleton<OrderMainService>
    {
        private IOrderMainHandler handler;
        private static readonly string ORDER_BY = "";
        public OrderMainService()
        {
            handler = BusinessLogicFactory.Instance.GetProvider<IOrderMainHandler>();
        }
        /// <summary>
        /// 查询单条数据,用于详细页面显示
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        public OrderMainItemModel Query(string id)
        {
            OrderMainItemModel model = new OrderMainItemModel();
            model.CurrentModel = handler.GetData(id);
            model.Id = id;
            return model;
        }
        /// <summary>
        /// 查询单条数据，用于新增编辑
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        public OrderMainItemModel QueryItem(string id)
        {
            OrderMainItemModel model = new OrderMainItemModel();
            model.CurrentModel = handler.GetData(id);
            model.Id = id;
            return model;
        }


        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <returns></returns>
        public List<MOrderMain> GetDataList()
        {
            return handler.GetDataList(ORDER_BY);
        }
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="json">json数据</param>
        /// <returns></returns>
        public List<MOrderMain> GetDataList(string json)
        {
            MOrderMain data =
                JsonData.JavaScriptDeserialize<MOrderMain>(json);
            return handler.GetDataList(data, ORDER_BY, Lib4Net.DB.MatchMode.Exact);
        }
        /// <summary>
        /// 查询列表数据
        /// </summary>
        /// <param name="nvc">参数集合</param>
        /// <returns></returns>
        public OrderMainListModel Query(NameValueCollection nvc)
        {
            OrderMainListModel model = new OrderMainListModel();
            MOrderMain entity = new MOrderMain();
            model.PageSize = CommFun.ToInt(nvc["ps"],
                SettingHelper.Instance.GetInt("PageSize", 10)).Value;
            model.PageIndex = CommFun.ToInt(nvc["pi"],
                SettingHelper.Instance.GetInt("PageIndex", 0)).Value + 1;
            entity.SetData(nvc, false);
            entity.TrimEmptyProperty();
            entity.AddData(":PS", model.PageSize);
            entity.AddData(":PI", model.PageIndex);
            //时间
            DateTime starttime = CommFun.ToDateTime(nvc["e"], DateTime.Now).Value;
            DateTime endtime = starttime.AddDays(1);
            if (!string.IsNullOrEmpty(nvc["h"]))
            {
                //近半个小时
                entity.AddData(":ST", " t.create_time >= sysdate-30/24/60");
                entity.AddData(":ET", " t.create_time < sysdate+30/24/60");
            }
            else
            {
                entity.AddData(":ST", " t.create_time>=to_date('" + starttime + "','yyyy-mm-dd  hh24:mi:ss')");
                entity.AddData(":ET", "t.create_time<to_date('" + endtime + "','yyyy-mm-dd  hh24:mi:ss')");
            }
            //关键字
            string keyWords = CommFun.GetString(nvc["KeyWords"]);
            string accountCondit = "1=1";
            if (!string.IsNullOrEmpty(keyWords))
            {
                int? type = CommFun.ToInt(nvc["keytype"], null);
                if (type == 1)
                {
                    entity.OrderNo = keyWords;
                }
                else if (type == 2)
                {
                    entity.PartnerOrderNo = keyWords;

                }
                else if (type == 3)
                {
                    accountCondit = "(t.account like '%" + keyWords + "%' or t.mobile like '%" + keyWords + "%')";
                }
                else if (type == 4)
                {
                    entity.Face = CommFun.ToDecimal(keyWords, null);
                }
            }

            entity.AddData(":Condition", accountCondit);
            model.TotalCount = CommFun.ToInt(handler.GetScalarByXmlTemplate("getCount", entity), 0).GetValueOrDefault();
            if (model.TotalCount > 0)
                model.List = handler.GetDataListByTemplate("getList", entity);
            return model;
        }

        /// <summary>
        /// 根据订单编号获取数据信息
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public OrderMainItemModel QueryOrderDetials(string orderNo)
        {
            OrderMainItemModel model = new OrderMainItemModel();
            MOrderMain entity = new MOrderMain();
            entity.OrderNo = orderNo;
            model.CurrentModel = handler.GetDataListByTemplate("getOrderDetailsByOrderNo", entity).FirstOrDefault();
            model.Id = orderNo;
            return model;
        }


        public object ReviewQuery(NameValueCollection nvc)
        {
            OrderMainListModel model = new OrderMainListModel();
            MOrderMain entity = new MOrderMain();
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
            //关键字
            string keyWords = CommFun.GetString(nvc["KeyWords"]);
            string accountCondit = "1=1";
            if (!string.IsNullOrEmpty(keyWords))
            {
                int? type = CommFun.ToInt(nvc["keytype"], null);
                if (type == 1)
                {
                    entity.OrderNo = keyWords;
                }
                else if (type == 2)
                {
                    entity.PartnerOrderNo = keyWords;

                }
                else if (type == 3)
                {
                    accountCondit = "(t.account like '%" + keyWords + "%' or t.mobile like '%" + keyWords + "%')";
                }
                else if (type == 4)
                {
                    entity.Face = CommFun.ToDecimal(keyWords, null);
                }
            }

            entity.AddData(":Condition", accountCondit);
            model.TotalCount = CommFun.ToInt(handler.GetScalarByXmlTemplate("getReviewCount", entity), 0).GetValueOrDefault();
            if (model.TotalCount > 0)
                model.List = handler.GetDataListByTemplate("getReviewList", entity);
            return model;
        }

        public IResult OrderManaul(string user, NameValueCollection nvc)
        {
            string id = nvc["__id"];
            string manaul = nvc["Manaul"];
            string remark = nvc["Remark"];
            return handler.OrderManaul(id, user, manaul, remark);
        }

        public MOrderMain getFace(string id)
        {
            return handler.GetData(id);
        }

        public List<MOrderMain> GetOrderList(string Account)
        {
            return handler.GetOrderListByAccount(Account);
        }

        public object SimpleOrderQuery(NameValueCollection nvc)
        {
            OrderMainListModel model = new OrderMainListModel();
            MOrderMain entity = new MOrderMain();
            model.PageSize = CommFun.ToInt(nvc["ps"],
                SettingHelper.Instance.GetInt("PageSize", 10)).Value;
            model.PageIndex = CommFun.ToInt(nvc["pi"],
                SettingHelper.Instance.GetInt("PageIndex", 0)).Value + 1;
            entity.SetData(nvc, false);
            entity.TrimEmptyProperty();
            entity.AddData(":PS", model.PageSize);
            entity.AddData(":PI", model.PageIndex);
            
            //关键字
            string keyWords = CommFun.GetString(nvc["KeyWords"]);
            string accountCondit = "1=1";
            if (!string.IsNullOrEmpty(keyWords))
            {
                int? type = CommFun.ToInt(nvc["keytype"], null);
                if (type == 1)
                {
                    entity.OrderNo = keyWords;
                }
                else if (type == 2)
                {
                    entity.PartnerOrderNo = keyWords;

                }
                else if (type == 3)
                {
                    accountCondit = "(t.account like '%" + keyWords + "%' or t.mobile like '%" + keyWords + "%')";
                }
            }

            entity.AddData(":Condition", accountCondit);
            model.TotalCount = CommFun.ToInt(handler.GetScalarByXmlTemplate("SimpleOrderCount", entity), 0).GetValueOrDefault();
            if (model.TotalCount > 0)
                model.List = handler.GetDataListByTemplate("SimpleOrderList", entity);
            return model;
        }
    }
}