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
    public class DownChannelSerivce : Singleton<DownChannelSerivce>
    {
        private IDownChannelHandler handler;
        private static readonly string ORDER_BY = "";
        public DownChannelSerivce()
        {
            handler = BusinessLogicFactory.Instance.GetProvider<IDownChannelHandler>();
        }

        public List<MDownChannel> GetDataList()
        {
            return handler.GetDataList(ORDER_BY);
        }

        public IResult Delete(string id)
        {
            return new Result(handler.Delete(id));
        }

        public IResult Save(string id, MDownChannel entity)
        {
            return handler.Save(id, entity);
        }

        public DownChannelListModel Query(NameValueCollection nvc)
        {
            DownChannelListModel model = new DownChannelListModel();
            MDownChannel entity = new MDownChannel();
            model.PageSize = CommFun.ToInt(nvc["ps"],
                SettingHelper.Instance.GetInt("PageSize", 10)).Value;
            model.PageIndex = CommFun.ToInt(nvc["pi"],
                SettingHelper.Instance.GetInt("PageIndex", 0)).Value + 1;
            entity.SetData(nvc, false);
            entity.TrimEmptyProperty();
            entity.AddData(":PS", model.PageSize);
            entity.AddData(":PI", model.PageIndex);
            if (!string.IsNullOrEmpty(nvc["ChannelName"]))
            {
                entity.AddData(":ChannelName", " t.Channel_Name like '%" + nvc["ChannelName"] + "%'");
            }
            model.TotalCount = CommFun.ToInt(handler.GetScalarByXmlTemplate("getCount", entity), 0).GetValueOrDefault();
            if (model.TotalCount > 0)
                model.List = handler.GetDataListByTemplate("getList", entity);
            return model;
        }

        public DownChannelItemModel QueryItem(string id)
        {
            DownChannelItemModel model = new DownChannelItemModel();
            model.Id = id;
            model.CurrentModel = handler.GetData(id);
            return model;
        }
    }
}
