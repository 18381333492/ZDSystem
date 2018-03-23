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
using System.Data;
using System.Text;

namespace ZDSystem.UserWeb.Controllers
{
    /// <summary>
    /// Controller：ReceiptAccountInfo(收款账户信息表)
    /// </summary>
    public class ReceiptAccountInfoController : MainBaseController
    {
        /// <summary>
        /// 显示列表页面数据
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(ReceiptAccountInfoService.Instance.Query(Request.QueryString));
        }

        /// <summary>
        /// 账户统计报表
        /// </summary>
        /// <returns></returns>
        public ActionResult AccountCount()
        {
            return View(ReceiptAccountInfoService.Instance.AccountCount(Request.QueryString));
        }

        /// <summary>
        /// 账户统计报表(运营管理)
        /// </summary>
        /// <returns></returns>
        public ActionResult AccountCountBusiness()
        {
            return View(ReceiptAccountInfoService.Instance.AccountCount(Request.QueryString));
        }

        /// <summary>
        ///  导出报表
        /// </summary>
        public void Export()
        {
            DataTable dt = ReceiptAccountInfoService.Instance.AccountCount(Request.Form);
            StringBuilder sTableHtml = new StringBuilder();

            decimal allprice = 0;//总金额
            int allordernumber = 0;//总订单
            decimal successprice = 0;//成功的金额
            int successordernumber = 0;// 成功的订单
            decimal service_fee = 0;//手续费
            decimal refund_money = 0;//退款金额
            int refund_number = 0;//退款笔数
            decimal loss_service = 0;//亏损的手续费

            sTableHtml.Append("<table border='1'>");
            sTableHtml.Append("<thead>");
            sTableHtml.Append("<tr>");
            sTableHtml.Append("<th>统计时间</th>");
            sTableHtml.Append("<th>下游渠道</th>");
            sTableHtml.Append("<th>业务类型</th>");
            sTableHtml.Append("<th>支付类型</th>");
            sTableHtml.Append("<th>支付金额</th>");
            sTableHtml.Append("<th>支付笔数</th>");
            sTableHtml.Append("<th>成功金额</th>");
            sTableHtml.Append("<th>成功笔数</th>");
            sTableHtml.Append("<th>手续费</th>");
            sTableHtml.Append("<th>退款金额</th>");
            sTableHtml.Append("<th>退款笔数</th>");
            sTableHtml.Append("<th>亏损的手续费</th>");
            sTableHtml.Append("</thead>");
            sTableHtml.Append("<tbody>");
            foreach (DataRow row in dt.Rows)
            {
                allprice += Convert.ToDecimal(row["allprice"]);
                allordernumber += Convert.ToInt32(row["allordernumber"]);
                successprice += Convert.ToDecimal(row["price"]);
                successordernumber += Convert.ToInt32(row["successordernumber"]);
                service_fee += Convert.ToDecimal(row["service_fee"]);
                refund_money += Convert.ToDecimal(row["refund_money"]);
                refund_number += Convert.ToInt32(row["refund_number"]);
                loss_service += Convert.ToDecimal(row["loss_service"]);      

                sTableHtml.Append("<tr>");
                sTableHtml.Append("<td>"+row[0]+"</td>");
                sTableHtml.Append("<td>" + row["channel_name"] + "</td>");
                sTableHtml.Append("<td>" + row["card_name"] + "</td>");
                sTableHtml.Append("<td>" + row["pay_name"] + "</td>");
                sTableHtml.Append("<td>" + row["allprice"] + "</td>");
                sTableHtml.Append("<td>" + row["allordernumber"] + "</td>");
                sTableHtml.Append("<td>" + row["price"] + "</td>");
                sTableHtml.Append("<td>" + row["successordernumber"] + "</td>");
                sTableHtml.Append("<td>" + row["service_fee"] + "</td>");
                sTableHtml.Append("<td>" + row["refund_money"] + "</td>");
                sTableHtml.Append("<td>" + row["refund_number"] + "</td>");
                sTableHtml.Append("<td>" + row["loss_service"] + "</td>");
                sTableHtml.Append("</tr>");
            }
            //添加总计
            sTableHtml.Append("<tr>");
            sTableHtml.Append("<td>总计</td>");
            sTableHtml.Append("<td></td>");
            sTableHtml.Append("<td></td>");
            sTableHtml.Append("<td></td>");
            sTableHtml.Append("<td>" + allprice + "</td>");
            sTableHtml.Append("<td>" + allordernumber + "</td>");
            sTableHtml.Append("<td>" + successprice + "</td>");
            sTableHtml.Append("<td>" + successordernumber + "</td>");
            sTableHtml.Append("<td>" + service_fee + "</td>");
            sTableHtml.Append("<td>" + refund_money + "</td>");
            sTableHtml.Append("<td>" + refund_number + "</td>");
            sTableHtml.Append("<td>" + loss_service + "</td>");
            sTableHtml.Append("</tr>");

            sTableHtml.Append("</tbody>");
            sTableHtml.Append("</table>");
            string sExcelName = string.Format("账户统计报表({0}--{1})", Request.Form["start_date"],Request.Form["end_date"]);

            System.Web.HttpContext.Current.Response.Charset = "UTF-8";
            System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(sExcelName, System.Text.Encoding.UTF8).ToString() + ".xls");
            System.Web.HttpContext.Current.Response.ContentType = "application/ms-excel";
            System.IO.StringWriter tw = new System.IO.StringWriter();
            System.Web.HttpContext.Current.Response.Output.Write(sTableHtml.ToString());
            System.Web.HttpContext.Current.Response.Flush();
            System.Web.HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 保存或修改页面
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        public ActionResult Edit(string id)
        {
            return View(ReceiptAccountInfoService.Instance.QueryItem(id));
        }

        /// <summary>
        /// 保存实体数据
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        [HttpPost]
        public IResult Edit()
        {
            IResult rst = null;
            try
            {
                MReceiptAccountInfo entity = new MReceiptAccountInfo();
                entity.SetData(Request.Form);
                entity.TrimEmptyProperty();
                string id = Request.Form["__id"];
                int? status = CommFun.ToInt(Request["Status"], 1);
                entity.Status = status;
                entity.DownChannelNo = CommFun.ToInt(Request["DChannelNo"], null);
                rst = ReceiptAccountInfoService.Instance.Save(id, entity);
                if (rst.Status)
                {
                    rst = new Result(true, "编辑成功");
                }
                else
                {
                    rst = new Result(false, "编辑失败");
                }
                return rst;
            }
            catch (Exception ex)
            {
                rst = new Result(false, ex.Message);

            }
            return rst;
        }
        /// <summary>
        /// 删除指定编号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IResult Delete(string id)
        {
            IResult rst = null;
            try
            {
                rst = ReceiptAccountInfoService.Instance.Delete(id);
                if (rst.Status)
                {
                    rst = new Result(true, "删除成功");
                }
                else
                {
                    rst = new Result(false, "删除失败");
                }
            }
            catch (Exception ex)
            {
                rst = new Result(false, ex.Message);
            }
            return rst;
        }


        /// <summary>
        /// 账户体现
        /// </summary>
        /// <returns></returns>
        public ActionResult Draw(string id)
        {
            object rst = null;
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    return View(ReceiptAccountInfoService.Instance.QueryItem(id));
                }
                else
                {
                    foreach (var key in Request.Form.AllKeys)
                    {
                        if (string.IsNullOrEmpty(Request.Form[key]))
                        {
                            rst = new { success = false, info = "缺少参数" };
                            break;
                        }
                    }
                    MReceiptFundRecord record = new MReceiptFundRecord();//资金变动实体model
                    record.ChangeAmount = Convert.ToDecimal(Request.Form["ChangeAmount"]);
                    record.AccountId = Convert.ToInt64(Request.Form["__id"]);
                    record.Remark = Request.Form["Remark"];
                    record.Operator = LoginStatus.UserName;
                    record.ChangeType = Convert.ToInt32(Request.Form["ChangeType"]);
                    if (record.ChangeAmount <= ReceiptAccountInfoService.Instance.QueryItem(record.AccountId.ToString()).CurrentModel.Balance)
                    {
                        if (ReceiptAccountInfoService.Instance.Draw(record))
                        {
                            rst = new { success = true, info = "操作成功" };
                        }
                        else
                            rst = new { success = false, info = "操作失败" };
                    }
                    else
                    {
                        rst = new { success = false, info = "余额不足" };
                    } 
                    return Json(rst);
                }
            }
            catch (Exception ex)
            {
                rst = new { success = false, info = ex.Message };
                return Json(rst);
            }
        
        }
    }
}