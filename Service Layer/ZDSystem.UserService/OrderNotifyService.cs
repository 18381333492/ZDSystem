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
    /// 服务操作: OrderNotify(订单通知)
    /// </summary>
   public class OrderNotifyService:Singleton<OrderNotifyService>
    {
       private IOrderNotifyHandler handler;
       private static readonly  string ORDER_BY="";
       public OrderNotifyService()
       {
           handler = BusinessLogicFactory.Instance.GetProvider<IOrderNotifyHandler>();
       }
         /// <summary>
       /// 查询单条数据,用于详细页面显示
       /// </summary>
       /// <param name="id">主键编号</param>
       /// <returns></returns>
       public OrderNotifyItemModel Query(string id)
       {
           OrderNotifyItemModel model = new OrderNotifyItemModel();
           model.CurrentModel = handler.GetData(id);
           model.Id = id;
           return model;
       }
         /// <summary>
       /// 查询单条数据，用于新增编辑
       /// </summary>
       /// <param name="id">主键编号</param>
       /// <returns></returns>
       public OrderNotifyItemModel QueryItem(string id)
       {
           OrderNotifyItemModel model = new OrderNotifyItemModel();
           model.CurrentModel = handler.GetData(id);
           model.Id = id;
           return model;
       }
        /// <summary>
       /// 查询单条数据，用于页面预览
       /// </summary>
       /// <param name="id">主键编号</param>
       /// <returns></returns>
       public OrderNotifyViewModel View(string id)
       {
           OrderNotifyViewModel model = new OrderNotifyViewModel();
           model.CurrentModel = handler.GetData(id);
           model.Id = id;
           return model;
       }
        /// <summary>
       /// 获取数据列表
       /// </summary>
       /// <returns></returns>
       public List<MOrderNotify> GetDataList()
       {
           return handler.GetDataList(ORDER_BY);
       }
        /// <summary>
       /// 获取数据列表
       /// </summary>
       /// <param name="json">json数据</param>
       /// <returns></returns>
       public List<MOrderNotify> GetDataList(string json)
       {
           MOrderNotify data =
               JsonData.JavaScriptDeserialize<MOrderNotify>(json);
           return handler.GetDataList(data, ORDER_BY, Lib4Net.DB.MatchMode.Exact);
       }
        /// <summary>
       /// 查询列表数据
       /// </summary>
       /// <param name="nvc">参数集合</param>
       /// <returns></returns>
       public OrderNotifyListModel Query(NameValueCollection nvc)
       {
           OrderNotifyListModel model = new OrderNotifyListModel();
           MOrderNotify entity = new MOrderNotify();
           model.PageSize = CommFun.ToInt(nvc["ps"],
               SettingHelper.Instance.GetInt("PageSize", 10)).Value;
           model.PageIndex= CommFun.ToInt(nvc["pi"],
               SettingHelper.Instance.GetInt("PageIndex", 0)).Value+1;
           entity.SetData(nvc,false);
           entity.TrimEmptyProperty();   
           entity.AddData(":PS",model.PageSize);
           entity.AddData(":PI",model.PageIndex);
           DateTime st = CommFun.ToDateTime(nvc["s"], DateTime.Now.AddDays(-1)).Value;
           DateTime et = CommFun.ToDateTime(nvc["e"], DateTime.Now).Value;
           entity.AddData("ST", st.ToString("yyyy-MM-dd"));
           entity.AddData("ET", et.ToString("yyyy-MM-dd"));
           
           model.TotalCount = CommFun.ToInt(handler.GetScalarByXmlTemplate("getCount", entity), 0).GetValueOrDefault();
            if(model.TotalCount > 0)
                model.List = handler.GetDataListByTemplate("getList",entity);
           return model;
       }

       /// <summary>
       ///  订单详情页面 --根据订单号获取通知记录
       /// </summary>
       /// <param name="orderNo"></param>
       /// <returns></returns>
       public List<MOrderNotify> QueryNotifyListByOrderNo(string orderNo) {

           MOrderNotify entity = new MOrderNotify();
           entity.OrderNo = orderNo;
           return handler.GetDataListByTemplate("getNotifyDetialsByOrderNo", entity);
       }


      
    }
}