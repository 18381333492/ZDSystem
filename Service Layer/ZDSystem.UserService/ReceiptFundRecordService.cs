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
    /// 服务操作: ReceiptFundRecord(收款账户资金变动表)
    /// </summary>
   public class ReceiptFundRecordService:Singleton<ReceiptFundRecordService>
    {
       private IReceiptFundRecordHandler handler;
       private static readonly  string ORDER_BY="";
       public ReceiptFundRecordService()
       {
           handler = BusinessLogicFactory.Instance.GetProvider<IReceiptFundRecordHandler>();
       }
         /// <summary>
       /// 查询单条数据,用于详细页面显示
       /// </summary>
       /// <param name="id">主键编号</param>
       /// <returns></returns>
       public ReceiptFundRecordItemModel Query(string id)
       {
           ReceiptFundRecordItemModel model = new ReceiptFundRecordItemModel();
           model.CurrentModel = handler.GetData(id);
           model.Id = id;
           return model;
       }
         /// <summary>
       /// 查询单条数据，用于新增编辑
       /// </summary>
       /// <param name="id">主键编号</param>
       /// <returns></returns>
       public ReceiptFundRecordItemModel QueryItem(string id)
       {
           ReceiptFundRecordItemModel model = new ReceiptFundRecordItemModel();
           model.CurrentModel = handler.GetData(id);
           model.Id = id;
           return model;
       }
        /// <summary>
       /// 查询单条数据，用于页面预览
       /// </summary>
       /// <param name="id">主键编号</param>
       /// <returns></returns>
       public ReceiptFundRecordViewModel View(string id)
       {
           ReceiptFundRecordViewModel model = new ReceiptFundRecordViewModel();
           model.CurrentModel = handler.GetData(id);
           model.Id = id;
           return model;
       }
        /// <summary>
       /// 获取数据列表
       /// </summary>
       /// <returns></returns>
       public List<MReceiptFundRecord> GetDataList()
       {
           return handler.GetDataList(ORDER_BY);
       }
        /// <summary>
       /// 获取数据列表
       /// </summary>
       /// <param name="json">json数据</param>
       /// <returns></returns>
       public List<MReceiptFundRecord> GetDataList(string json)
       {
           MReceiptFundRecord data =
               JsonData.JavaScriptDeserialize<MReceiptFundRecord>(json);
           return handler.GetDataList(data, ORDER_BY, Lib4Net.DB.MatchMode.Exact);
       }
        /// <summary>
       /// 查询列表数据
       /// </summary>
       /// <param name="nvc">参数集合</param>
       /// <returns></returns>
       public ReceiptFundRecordListModel Query(NameValueCollection nvc)
       {
           ReceiptFundRecordListModel model = new ReceiptFundRecordListModel();
           MReceiptFundRecord entity = new MReceiptFundRecord();
           StringBuilder WhereString = new StringBuilder();
           //下游渠道查询
           if (!string.IsNullOrEmpty(nvc["PayType"]))
           {
               WhereString.AppendFormat(" and t1.account_type={0}", nvc["PayType"]);
           }
           if (!string.IsNullOrEmpty(nvc["ChannelNo"]))
           {
               WhereString.AppendFormat(" and t1.down_channel_no={0}", nvc["ChannelNo"]);
           }

           model.PageSize = CommFun.ToInt(nvc["ps"],
               SettingHelper.Instance.GetInt("PageSize", 10)).Value;
           model.PageIndex= CommFun.ToInt(nvc["pi"],
               SettingHelper.Instance.GetInt("PageIndex", 0)).Value+1;
           entity.SetData(nvc,false);
           entity.TrimEmptyProperty();   
           entity.AddData(":PS",model.PageSize);
           entity.AddData(":PI",model.PageIndex);
           entity.AddData(":WhereString", string.IsNullOrEmpty(WhereString.ToString()) ? " and 1=1" : WhereString.ToString());

           //时间
           DateTime st = CommFun.ToDateTime(nvc["s"], DateTime.Now.AddDays(-1)).Value;
           DateTime et = CommFun.ToDateTime(nvc["e"], DateTime.Now).Value;
           entity.AddData("ST", st.ToString("yyyy-MM-dd"));
           entity.AddData("ET", et.ToString("yyyy-MM-dd"));

           //关键字
           string keyWords = CommFun.GetString(nvc["KeyWords"]);
           if (!string.IsNullOrEmpty(keyWords)) {
               entity.OrderNO = keyWords;
           }
           model.TotalCount = CommFun.ToInt(handler.GetScalarByXmlTemplate("getCount", entity), 0).GetValueOrDefault();
            if(model.TotalCount > 0)
                model.List = handler.GetDataListByTemplate("getList",entity);
           return model;
       }
        /// <summary>
       /// 保存实体数据
       /// </summary>
       /// <param name="id">主键编号</param>
       /// <param name="entity">实体数据</param>
       /// <returns></returns>
       public IResult Save(string id,MReceiptFundRecord entity)
       {
           
           return handler.Save(id,entity);
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