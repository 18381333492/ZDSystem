using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lib4Net.Comm;
using ZDSystem.Interfaces.Logic;
using ZDSystem.LogicFactories;
using ZDSystem.Model;
using ZDSystem.Entity;
using Lib4Net.Framework.Settings;
using Lib4Net.Core;
using System.Collections.Specialized;
using Lib4Net.Data;
namespace ZDSystem.UserService
{
    /// <summary>
    /// 服务操作: ZdCouponUsed(使用过的中大优惠券)
    /// </summary>
    public class ZdCouponUsedService : Singleton<ZdCouponUsedService>
    {
        private IZdCouponUsedHandler handler;
        private static readonly string ORDER_BY = "";
        public ZdCouponUsedService()
        {
            handler = BusinessLogicFactory.Instance.GetProvider<IZdCouponUsedHandler>();
        }
        /// <summary>
        /// 查询单条数据,用于详细页面显示
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        public ZdCouponUsedItemModel Query(string id)
        {
            ZdCouponUsedItemModel model = new ZdCouponUsedItemModel();
            model.CurrentModel = handler.GetData(id);
            model.Id = id;
            return model;
        }
        /// <summary>
        /// 查询单条数据，用于新增编辑
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        public ZdCouponUsedItemModel QueryItem(string id)
        {
            ZdCouponUsedItemModel model = new ZdCouponUsedItemModel();
            model.CurrentModel = handler.GetData(id);
            model.Id = id;
            return model;
        }


        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <returns></returns>
        public List<MZdCouponUsed> GetDataList()
        {
            return handler.GetDataList(ORDER_BY);
        }
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="json">json数据</param>
        /// <returns></returns>
        public List<MZdCouponUsed> GetDataList(string json)
        {
            MZdCouponUsed data =
                JsonData.JavaScriptDeserialize<MZdCouponUsed>(json);
            return handler.GetDataList(data, ORDER_BY, Lib4Net.DB.MatchMode.Exact);
        }
        /// <summary>
        /// 查询列表数据
        /// </summary>
        /// <param name="nvc">参数集合</param>
        /// <returns></returns>
        public ZdCouponUsedListModel Query(NameValueCollection nvc)
        {
            ZdCouponUsedListModel model = new ZdCouponUsedListModel();
            MZdCouponUsed entity = new MZdCouponUsed();
            model.PageSize = CommFun.ToInt(nvc["ps"],
                SettingHelper.Instance.GetInt("PageSize", 10)).Value;
            model.PageIndex = CommFun.ToInt(nvc["pi"],
                SettingHelper.Instance.GetInt("PageIndex", 0)).Value + 1;
            entity.SetData(nvc, false);
            entity.AddData(":PS", model.PageSize);
            entity.AddData(":PI", model.PageIndex);
            //时间
            DateTime starttime = CommFun.ToDateTime(nvc["s"], DateTime.Now.AddDays(-1)).Value;
            DateTime endtime = CommFun.ToDateTime(nvc["e"], DateTime.Now).Value.AddDays(1);
            entity.AddData(":ST", " t.use_time>=to_date('" + starttime + "','yyyy-mm-dd  hh24:mi:ss')");
            entity.AddData(":ET", "t.use_time<to_date('" + endtime + "','yyyy-mm-dd  hh24:mi:ss')");
            
            //关键字
            string keyWords = CommFun.GetString(nvc["KeyWords"]);
            if (!string.IsNullOrEmpty(keyWords))
            {
                int? type = CommFun.ToInt(nvc["keytype"], null);
                if (type == 1)
                {
                    entity.OrderNo = keyWords;
                }
                else if (type == 2)
                {

                    entity.DownOrderNo = keyWords;
                }
                else if (type == 3)
                {
                    entity.CouponPrice = CommFun.ToDecimal(keyWords, null);
                }
            }
            model.TotalCount = CommFun.ToInt(handler.GetScalarByXmlTemplate("getCount", entity), 0).GetValueOrDefault();
            if (model.TotalCount > 0)
                model.List = handler.GetDataListByTemplate("getList", entity);
            return model;
        }

    }
}
