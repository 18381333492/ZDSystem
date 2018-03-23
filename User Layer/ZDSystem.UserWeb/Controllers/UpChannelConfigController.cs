using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZDSystem.Entity;
using Lib4Net.Comm;
using ZDSystem.UserService;
using ZDSystem.Model;
using Lib4Net.Core;

namespace ZDSystem.UserWeb.Controllers
{
    public class UpChannelConfigController : MainBaseController
    {
        //
        // GET: /UpChannelConfig/

        public ActionResult Index()
        {
            return View(UpChannelConfigService.Instance.Query(Request.QueryString));
        }

        public ActionResult Item(string id)
        {
            return View(UpChannelConfigService.Instance.QueryItem(id));
        }


        [HttpPost]
        public IResult Delete(string id)
        {
            IResult result = UpChannelConfigService.Instance.Delete(id);
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
                MUpChannelConfig entity = new MUpChannelConfig();
                entity.SetData(Request.Form);
                entity.DownChannelNo = CommFun.ToInt(Request["DChannelNo"], null);
                string id = CommFun.GetString(Request["__id"]);
                IResult result = UpChannelConfigService.Instance.Save(id, entity);
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
