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
using System.Data;


namespace ZDSystem.UserService
{
   /// <summary>
    /// 服务操作: ReceiptAccountInfo(收款账户信息表)
    /// </summary>
   public class ReceiptAccountInfoService:Singleton<ReceiptAccountInfoService>
    {
       private IReceiptAccountInfoHandler handler;
       private static readonly  string ORDER_BY="";
       public ReceiptAccountInfoService()
       {
           handler = BusinessLogicFactory.Instance.GetProvider<IReceiptAccountInfoHandler>();
       }
         /// <summary>
       /// 查询单条数据,用于详细页面显示
       /// </summary>
       /// <param name="id">主键编号</param>
       /// <returns></returns>
       public ReceiptAccountInfoItemModel Query(string id)
       {
           ReceiptAccountInfoItemModel model = new ReceiptAccountInfoItemModel();
           model.CurrentModel = handler.GetData(id);
           model.Id = id;
           return model;
       }
         /// <summary>
       /// 查询单条数据，用于新增编辑
       /// </summary>
       /// <param name="id">主键编号</param>
       /// <returns></returns>
       public ReceiptAccountInfoItemModel QueryItem(string id)
       {
           ReceiptAccountInfoItemModel model = new ReceiptAccountInfoItemModel();
           model.CurrentModel = handler.GetData(id);
           model.Id = id;
           return model;
       }
        /// <summary>
       /// 查询单条数据，用于页面预览
       /// </summary>
       /// <param name="id">主键编号</param>
       /// <returns></returns>
       public ReceiptAccountInfoViewModel View(string id)
       {
           ReceiptAccountInfoViewModel model = new ReceiptAccountInfoViewModel();
           model.CurrentModel = handler.GetData(id);
           model.Id = id;
           return model;
       }
        /// <summary>
       /// 获取数据列表
       /// </summary>
       /// <returns></returns>
       public List<MReceiptAccountInfo> GetDataList()
       {
           return handler.GetDataList(ORDER_BY);
       }
        /// <summary>
       /// 获取数据列表
       /// </summary>
       /// <param name="json">json数据</param>
       /// <returns></returns>
       public List<MReceiptAccountInfo> GetDataList(string json)
       {
           MReceiptAccountInfo data =
               JsonData.JavaScriptDeserialize<MReceiptAccountInfo>(json);
           return handler.GetDataList(data, ORDER_BY, Lib4Net.DB.MatchMode.Exact);
       }
        /// <summary>
       /// 查询列表数据
       /// </summary>
       /// <param name="nvc">参数集合</param>
       /// <returns></returns>
       public ReceiptAccountInfoListModel Query(NameValueCollection nvc)
       {
           ReceiptAccountInfoListModel model = new ReceiptAccountInfoListModel();
           MReceiptAccountInfo entity = new MReceiptAccountInfo();
           model.PageSize = CommFun.ToInt(nvc["ps"],
               SettingHelper.Instance.GetInt("PageSize", 10)).Value;
           model.PageIndex= CommFun.ToInt(nvc["pi"],
               SettingHelper.Instance.GetInt("PageIndex", 0)).Value+1;
           entity.SetData(nvc,false);
           entity.TrimEmptyProperty();   
           entity.AddData(":PS",model.PageSize);
           entity.AddData(":PI",model.PageIndex);
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
       public IResult Save(string id,MReceiptAccountInfo entity)
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
       
       /// <summary>
       /// 提现
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool Draw(MReceiptFundRecord model)
       {
           return handler.Draw(model);
       }

        
       /// <summary>
       /// 统计账户信息
       /// </summary>
       /// <param name="start_date">开始时间</param>
       /// <param name="end_date">结束时间</param>
       /// <param name="channel_no">下游渠道编号</param>
       /// <param name="pay_type">支付类型</param>
       /// <returns></returns>
       public DataTable AccountCount(NameValueCollection nvc)
       {
           string start_date=nvc["start_date"];
           string end_date = nvc["end_date"];
           string channel_no =nvc["channel_no"];
           string pay_type =nvc["pay_type"];
           string card_type = nvc["card_type"];
           var dt = handler.AccountCount(start_date, end_date, channel_no, pay_type, card_type);
           return dt;
       }
    }
}