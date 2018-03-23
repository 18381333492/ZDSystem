using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lib4Net.Core;
using Lib4Net.Framework.Settings;
using Lib4Net.Comm;
using Lib4Net.Logs;
using ZDSystem.UserService;
using ZDSystem.Entity;
using ZDSystem.Model;
using ZDSystem.Utility;

namespace ZDSystem.UserWeb.Controllers
{
    /// <summary>
    /// Controller：SysDictionary(差异)
    /// </summary>
    public class SysDictionaryController : MainBaseController
    {
        /// <summary>
        /// 显示列表页面数据
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(SysDictionaryService.Instance.Query(Request.QueryString));
        }
        /// <summary>
        /// 预览详细信息页面
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        public ActionResult Details(string id)
        {
            return View(SysDictionaryService.Instance.Query(id));
        }
        /// <summary>
        /// 保存或修改页面
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        public ActionResult Item(string id)
        {
            ViewBag.Id = id;
            return View(SysDictionaryService.Instance.QueryItem(id));
        }
        /// <summary>
        /// 预览页面
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        public ActionResult View(string id)
        {
            return View(SysDictionaryService.Instance.View(id));
        }
        /// <summary>
        /// 保存实体数据
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        [HttpPost]
        public IResult Item()
        {
            try
            {
                MSysDictionary entity = new MSysDictionary();
                entity.SetData(Request.Form);
                entity.TrimEmptyProperty();
                string id = Request.Form["__id"];
                IResult result = SysDictionaryService.Instance.Save(id, entity);
                if (result.Status)
                {
                    result.SetSuccessMessage("保存成功");
                }
                else
                {
                    result.SetErrorMessage("保存失败");
                }
                return result;
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }

        }
        /// <summary>
        /// 删除指定编号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IResult Delete(string id)
        {
            IResult result = SysDictionaryService.Instance.Delete(id);
            if (result.Status)
            {
                result.SetSuccessMessage("删除成功");
            }
            else
            {
                result.SetErrorMessage("删除失败");
            }
            return result;
        }




    }
}