using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;

namespace Scaffold.Controllers
{
    public class ModularVsTubeFitDashboardController : Controller
    {
        ScaffoldEntities dbcontext = new ScaffoldEntities();
        // GET: ModularVsTubeFitDashboard
        public ActionResult ModularVsTubeFitDashboard()
        {
            // ddlmaterial1.SelectedValue = "AT-PAC";
            ViewBag.ddlmaterial1 = "AT-PAC";
            //string query = "select distinct(MaterialType) as MaterialType from tblMaterialType where MaterialStatus='Active' and MaterialType <>'' and MaterialType is not null and  ClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' order by MaterialType";
            string query = "select distinct(MaterialType) as MaterialType from tblInventory where MaterialType <>'' and MaterialType is not null and  ClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' order by MaterialType";
            DataTable dt1 = databaseCon.GetData(query);
            if (dt1.Rows.Count > 0)
            {

                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    string MaterialType = dt1.Rows[i]["MaterialType"].ToString();

                    if (MaterialType == "Tube & Fit")
                    {
                       // ddlmaterial2.SelectedValue = "Tube & Fit";
                         ViewBag.ddlmaterial2= "Tube & Fit";
                    }
                    else if (MaterialType == "Conventional")
                    {
                       //ddlmaterial2.SelectedValue = "Conventional";
                        ViewBag.ddlmaterial2 = "Conventional";
                    }
                }

            }

            return View();
        }

        public ActionResult BindallDropdown(string id)
        {

            var ddlsites = dbcontext.Database.SqlQuery<DropDownViewModel>("select SiteLocID,SiteLocation from tblsitelocation where ClientId='" + id + "' Order by SiteLocation").ToList();

            var ddlMaterialType = dbcontext.Database.SqlQuery<DropDownViewModel>("select distinct(MaterialType) as MaterialType from tblInventory where MaterialType <>'' and MaterialType is not null and  ClientId='" + id + "' order by MaterialType");

            return Json(new { ddlsites = ddlsites, ddlMaterialType = ddlMaterialType }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ddlJobDashboard(string id, string SiteId)
        {
            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select distinct Location as area,LocationId as areaid from VW_TblRegister where (Tonnage is not null or ManHrsDismantle is not null) and (SiteLocId='" + SiteId + "' or 'All'='" + SiteId + "') and ClientID='" + id + "' Order by Location").ToList();

            return Json(GetData, JsonRequestBehavior.AllowGet);
        }

        List<ScaffoldAllDashBoardViewModel> MaterialTypDashboard = new List<ScaffoldAllDashBoardViewModel> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod(EnableSession = true)]
        //public List<Scaffold> MaterialProgress(string id, string site, string jobid, string MaterialType1, string MaterialType2, string fromdate, string todat)
        public ActionResult MaterialProgress(string id, string site, string jobid, string MaterialType1, string MaterialType2, string fromdate, string todat)
        {
            string weekdays = "";
            string ClientQuery = "select  weekStartDay from tblClient where ClientId='" + id + "'";
            DataSet ds1 = null;
            ds1 = databaseCon.GetDataSet(ClientQuery);

            foreach (DataRow item in ds1.Tables[0].Rows)
            {
                weekdays = item[0].ToString();
            }



            string qry = "exec Pro_GetMultipleMaterialData '" + id + "','" + site + "','" + jobid + "','" + fromdate + "','" + todat + "','" + MaterialType1 + "','" + MaterialType2 + "'";

            // string qry = "select WeekNo,replace(CONVERT(varchar(11),WeekStartDate,106),' ','-') as WeekStartDate,replace(CONVERT(varchar(11),WeekEndDate,106),' ','-') as WeekEndDate, Sum(MaterialType1Quantity) as MaterialType1Tonnage, Sum(MaterialType2Tonnage) as MaterialType2Tonnage, convert(decimal(18, 2),Sum(MaterialType1Quantity) / 1000) as MaterialType1Quantity, convert(decimal(18, 2),Sum(MaterialType2Quantity) / 1000) as MaterialType2Quantity, Sum(TotManHrsErect) as TotManHrsErect, Sum(TotManHrsDismantle) as TotManHrsDismantle from(select WeekNo, WeekStartDate, WeekEndDate, Count(RegisterID) as TotErectedRegister, Sum(isnull(Tonnage, 0)) as MaterialType1Quantity, 0 as MaterialType2Tonnage, 0 as MaterialType2Quantity, Sum(isnull(ManHrsErect, 0)) as TotManHrsErect, 0 as TotManHrsDismantle from(select * from Fn_TenWeekDate('" + weekdays + "','" + todat + "'))WeeksDate Left join vw_tblRegister on WeeksDate.weekStartDate <= datetagged and WeeksDate.weekEndDate >= datetagged and  clientId = '" + id + "' and (SiteLocId='" + site + "' or 'All'='" + site + "') and ('" + jobid + "' = 'All'  or LocationId = '" + jobid + "') group by weekNo, weekStartDate, weekEndDate union all select WeekNo, WeekStartDate, WeekEndDate , 0 as MaterialType1Tonnage, 0 as MaterialType1Quantity , Count(RegisterID) as MaterialType2Tonnage, Sum(isnull(Tonnage, 0)) as MaterialType2Quantity , 0 as TotManHrsErect, Sum(isnull(ManHrsDismantle, 0)) as TotManHrsDismantle from (select * from Fn_TenWeekDate('" + weekdays + "','" + todat + "'))WeeksDate Left join vw_tblRegister on WeeksDate.weekStartDate <= dateofdismantle and WeeksDate.weekEndDate >= dateofdismantle and  clientId = '" + id + "' and (SiteLocId='" + site + "' or 'All'='" + site + "') and ('" + jobid + "' = 'All'  or LocationId = '" + jobid + "') group by weekNo, weekStartDate, weekEndDate )AllQuery group by weekNo, weekStartDate, weekEndDate order by weekno";

            DataSet ds = null;
            ds = databaseCon.GetDataSet(qry);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ScaffoldAllDashBoardViewModel prosb = new ScaffoldAllDashBoardViewModel();
                    if (!(item["WeekNo"] is DBNull) && item["WeekNo"].ToString() != "")
                    {
                        prosb.WeekNo = Convert.ToInt32(item["WeekNo"].ToString());
                    }
                    if (!(item["MaterialType1Qty"] is DBNull) && item["MaterialType1Qty"].ToString() != "")
                    {
                        prosb.MaterialType1Quantity = Convert.ToDecimal(item["MaterialType1Qty"].ToString());
                    }
                    if (!(item["MaterialType2Qty"] is DBNull) && item["MaterialType2Qty"].ToString() != "")
                    {
                        prosb.MaterialType2Quantity = Convert.ToDecimal(item["MaterialType2Qty"].ToString());
                    }

                    if (!(item["MaterialType1TotalWeight"] is DBNull) && item["MaterialType1TotalWeight"].ToString() != "")
                    {
                        prosb.MaterialType1Tonnage = Convert.ToDecimal(item["MaterialType1TotalWeight"].ToString());
                    }
                    if (!(item["MaterialType2TotalWeight"] is DBNull) && item["MaterialType2TotalWeight"].ToString() != "")
                    {
                        prosb.MaterialType2Tonnage = Convert.ToDecimal(item["MaterialType2TotalWeight"].ToString());
                    }
                    if (!(item["WeekStart"] is DBNull) && item["WeekStart"].ToString() != "")
                    {
                        prosb.WeekStartDate = Convert.ToDateTime(item["WeekStart"]).ToString("dd-MMM-yyyy");
                    }
                    if (!(item["WeekEnd"] is DBNull) && item["WeekEnd"].ToString() != "")
                    {
                        prosb.WeekEndDate = Convert.ToDateTime(item["WeekEnd"]).ToString("dd-MMM-yyyy");
                    }
                    //if (!(item["WeekNo"] is DBNull) && item["WeekNo"].ToString() != "")
                    //{
                    //    prosb.WeekNo = Convert.ToInt32(item["WeekNo"].ToString());
                    //}
                    //if (!(item["MaterialType1Quantity"] is DBNull) && item["MaterialType1Quantity"].ToString() != "")
                    //{
                    //    prosb.MaterialType1Quantity = Convert.ToDecimal(item["MaterialType1Quantity"].ToString());
                    //}
                    //if (!(item["MaterialType2Quantity"] is DBNull) && item["MaterialType2Quantity"].ToString() != "")
                    //{
                    //    prosb.MaterialType2Quantity = Convert.ToDecimal(item["MaterialType2Quantity"].ToString());
                    //}
                    //if (!(item["TotManHrsErect"] is DBNull) && item["TotManHrsErect"].ToString() != "")
                    //{
                    //    prosb.TotManHrsErect = Convert.ToDecimal(item["TotManHrsErect"].ToString());
                    //}
                    //if (!(item["TotManHrsDismantle"] is DBNull) && item["TotManHrsDismantle"].ToString() != "")
                    //{
                    //    prosb.TotManHrsDismantle = Convert.ToDecimal(item["TotManHrsDismantle"].ToString());
                    //}
                    //if (!(item["MaterialType1Tonnage"] is DBNull) && item["MaterialType1Tonnage"].ToString() != "")
                    //{
                    //    prosb.MaterialType1Tonnage = Convert.ToDecimal(item["MaterialType1Tonnage"].ToString());
                    //}
                    //if (!(item["MaterialType2Tonnage"] is DBNull) && item["MaterialType2Tonnage"].ToString() != "")
                    //{
                    //    prosb.MaterialType2Tonnage = Convert.ToDecimal(item["MaterialType2Tonnage"].ToString());
                    //}
                    //if (!(item["weekStartDate"] is DBNull) && item["weekStartDate"].ToString() != "")
                    //{
                    //    prosb.WeekStartDate = Convert.ToDateTime(item["weekStartDate"]).ToString("dd-MMM-yyyy");
                    //}
                    //if (!(item["weekEndDate"] is DBNull) && item["weekEndDate"].ToString() != "")
                    //{
                    //    prosb.WeekEndDate = Convert.ToDateTime(item["weekEndDate"]).ToString("dd-MMM-yyyy");
                    //}
                    MaterialTypDashboard.Add(prosb);
                }
            }
            //  return MaterialTypDashboard;
            return Json(MaterialTypDashboard, JsonRequestBehavior.AllowGet);
        }
    }
}