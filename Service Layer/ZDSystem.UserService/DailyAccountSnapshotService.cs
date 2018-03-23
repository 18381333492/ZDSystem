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
    /// 服务操作: DailyAccountSnapshot(每日账户快照)
    /// </summary>
    public class DailyAccountSnapshotService : Singleton<DailyAccountSnapshotService>
    {
        private IDailyAccountSnapshotHandler handler;
        private static readonly string ORDER_BY = "";
        public DailyAccountSnapshotService()
        {
            handler = BusinessLogicFactory.Instance.GetProvider<IDailyAccountSnapshotHandler>();
        }
        /// <summary>
        /// 查询单条数据,用于详细页面显示
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        public DailyAccountSnapshotItemModel Query(string id)
        {
            DailyAccountSnapshotItemModel model = new DailyAccountSnapshotItemModel();
            model.CurrentModel = handler.GetData(id);
            model.Id = id;
            return model;
        }
        /// <summary>
        /// 查询单条数据，用于新增编辑
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        public DailyAccountSnapshotItemModel QueryItem(string id)
        {
            DailyAccountSnapshotItemModel model = new DailyAccountSnapshotItemModel();
            model.CurrentModel = handler.GetData(id);
            model.Id = id;
            return model;
        }
        /// <summary>
        /// 查询单条数据，用于页面预览
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        public DailyAccountSnapshotViewModel View(string id)
        {
            DailyAccountSnapshotViewModel model = new DailyAccountSnapshotViewModel();
            model.CurrentModel = handler.GetData(id);
            model.Id = id;
            return model;
        }
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <returns></returns>
        public List<MDailyAccountSnapshot> GetDataList()
        {
            return handler.GetDataList(ORDER_BY);
        }
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="json">json数据</param>
        /// <returns></returns>
        public List<MDailyAccountSnapshot> GetDataList(string json)
        {
            MDailyAccountSnapshot data =
                JsonData.JavaScriptDeserialize<MDailyAccountSnapshot>(json);
            return handler.GetDataList(data, ORDER_BY, Lib4Net.DB.MatchMode.Exact);
        }
        /// <summary>
        /// 查询列表数据
        /// </summary>
        /// <param name="nvc">参数集合</param>
        /// <returns></returns>
        public DailyAccountSnapshotListModel Query(NameValueCollection nvc)
        {
            DailyAccountSnapshotListModel model = new DailyAccountSnapshotListModel();
            MDailyAccountSnapshot entity = new MDailyAccountSnapshot();
            model.PageSize = CommFun.ToInt(nvc["ps"],
                SettingHelper.Instance.GetInt("PageSize", 10)).Value;
            model.PageIndex = CommFun.ToInt(nvc["pi"],
                SettingHelper.Instance.GetInt("PageIndex", 0)).Value + 1;
            entity.SetData(nvc, false);
            entity.TrimEmptyProperty();
            entity.AddData(":PS", model.PageSize);
            entity.AddData(":PI", model.PageIndex);

            DateTime st = CommFun.ToDateTime(nvc["s"], DateTime.Now).Value;
            DateTime et = CommFun.ToDateTime(nvc["e"], DateTime.Now).Value.AddDays(1);
            entity.AddData("ST", st.ToString("yyyy-MM-dd"));
            entity.AddData("ET", et.ToString("yyyy-MM-dd"));

            model.TotalCount = CommFun.ToInt(handler.GetScalarByXmlTemplate("getCount", entity), 0).GetValueOrDefault();
            if (model.TotalCount > 0)
            {
                model.Statistics = handler.GetSingleDataByTemplate("getStatistics", entity);
                model.List = handler.GetDataListByTemplate("getList", entity);
            }
            return model;
        }
        /// <summary>
        /// 保存实体数据
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public IResult Save(string id, MDailyAccountSnapshot entity)
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