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
    /// 服务操作: SysDictionary(差异)
    /// </summary>
    public class SysDictionaryService : Singleton<SysDictionaryService>
    {
        private ISysDictionaryHandler handler;
        private static readonly string ORDER_BY = "";
        public SysDictionaryService()
        {
            handler = BusinessLogicFactory.Instance.GetProvider<ISysDictionaryHandler>();
        }
        /// <summary>
        /// 查询单条数据,用于详细页面显示
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        public SysDictionaryItemModel Query(string id)
        {
            SysDictionaryItemModel model = new SysDictionaryItemModel();
            model.CurrentModel = handler.GetData(id);
            model.Id = id;
            return model;
        }
        /// <summary>
        /// 查询单条数据，用于新增编辑
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        public SysDictionaryItemModel QueryItem(string id)
        {
            SysDictionaryItemModel model = new SysDictionaryItemModel();
            model.CurrentModel = handler.GetData(id);
            model.Id = id;
            return model;
        }
        /// <summary>
        /// 查询单条数据，用于页面预览
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        public SysDictionaryViewModel View(string id)
        {
            SysDictionaryViewModel model = new SysDictionaryViewModel();
            model.CurrentModel = handler.GetData(id);
            model.Id = id;
            return model;
        }
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <returns></returns>
        public List<MSysDictionary> GetDataList()
        {
            return handler.GetDataList(ORDER_BY);
        }
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="json">json数据</param>
        /// <returns></returns>

        public List<MSysDictionary> GetDataList(string typeName)
        {
            List<MSysDictionary> list = handler.GetDataListByTypeName(typeName);
            if (typeName == "BusinessType")
            {
                list = SDOrder(list);
            }
            return list;
        }

        public List<MSysDictionary> SDOrder(List<MSysDictionary> list)
        {
            for (int i = list.Count; i > 0; i--)
            {
                for (int j = 0; j < i - 1; j++)
                {
                    int firstNum = Convert.ToInt32(list[j].Value);
                    int lastNum = Convert.ToInt32(list[j + 1].Value);
                    if (firstNum > lastNum)
                    {
                        MSysDictionary temp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = temp;
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// 查询列表数据
        /// </summary>
        /// <param name="nvc">参数集合</param>
        /// <returns></returns>
        public SysDictionaryListModel Query(NameValueCollection nvc)
        {
            SysDictionaryListModel model = new SysDictionaryListModel();
            MSysDictionary entity = new MSysDictionary();
            model.PageSize = CommFun.ToInt(nvc["ps"],
                SettingHelper.Instance.GetInt("PageSize", 10)).Value;
            model.PageIndex = CommFun.ToInt(nvc["pi"],
                SettingHelper.Instance.GetInt("PageIndex", 0)).Value + 1;
            entity.SetData(nvc, false);
            entity.TrimEmptyProperty();
            entity.AddData(":PS", model.PageSize);
            entity.AddData(":PI", model.PageIndex);
            if (!string.IsNullOrEmpty(nvc["name"]))
            {
                entity.AddData(":Name", " t.name like '%" + nvc["name"] + "%'");
            }
            if (!string.IsNullOrEmpty(nvc["type"]))
            {
                entity.AddData(":Type", " t.type like '%" + nvc["type"] + "%'");
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
        public IResult Save(string id, MSysDictionary entity)
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

        /// <summary>
        /// 根据类型获取字典数据列表
        /// </summary>
        /// <param name="TypeName"></param>
        /// <returns></returns>
        public List<MSysDictionary> GetDataListByType(string TypeName)
        {
            return handler.GetDataListByTypeName(TypeName);
        }

    }
}