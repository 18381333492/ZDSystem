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
    public class DownChannelController : MainBaseController
    {
        //
        // GET: /DownChannel/

        public ActionResult Index()
        {
            return View(DownChannelSerivce.Instance.Query(Request.QueryString));
        }

        public ActionResult Item(string id)
        {
            return View(DownChannelSerivce.Instance.QueryItem(id));
        }

        [HttpPost]
        public IResult Delete(string id)
        {
            IResult result = DownChannelSerivce.Instance.Delete(id);
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

        [HttpPost]
        public IResult Item()
        {
            try
            {
                MDownChannel entity = new MDownChannel();
                entity.SetData(Request.Form);
                entity.TrimEmptyProperty();
                string id = Request.Form["__id"];
                entity.Status = CommFun.ToInt(Request["StatusN"], 1);
                IResult result = DownChannelSerivce.Instance.Save(id, entity);
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
    }
}
