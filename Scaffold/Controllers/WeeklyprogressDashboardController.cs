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
    public class WeeklyprogressDashboardController : Controller
    {
        ScaffoldEntities dbcontext = new ScaffoldEntities();
        // GET: WeeklyprogressDashboard
        public ActionResult WeeklyprogressDashboard()
        {
            return View();
        }
        public ActionResult SiteDropDownBind(string id)
        {
            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select SiteLocID,SiteLocation from tblsitelocation where ClientId='" +id + "' Order by SiteLocation").ToList();
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ddlJobDashboard(string id, string SiteId)
        {
            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select distinct Location as area,LocationId as areaid from VW_TblRegister where (Tonnage is not null or ManHrsDismantle is not null) and (SiteLocId='" + SiteId + "' or 'All'='" + SiteId + "') and ClientID='" + id + "' Order by Location").ToList();
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }

        List<ScaffoldAllDashBoardViewModel> WeeklyDashboard = new List<ScaffoldAllDashBoardViewModel> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod(EnableSession = true)]
        //public List<Scaffold> WeeklyProgress(string id, string site, string jobid, string getnewdate)
        public ActionResult WeeklyProgress(string id, string site, string jobid, string getnewdate, string WorkGroup)
        {
            string weekdays = "";
            string ClientQuery = "select  weekStartDay from tblClient where ClientId='" + id + "'";
            DataSet ds1 = null;
            ds1 = databaseCon.GetDataSet(ClientQuery);

            foreach (DataRow item in ds1.Tables[0].Rows)
            {
                weekdays = item[0].ToString();
            }
            // string qry = "select WeekNo,replace(CONVERT(varchar(11),WeekStartDate,106),' ','-') as WeekStartDate,replace(CONVERT(varchar(11),WeekEndDate,106),' ','-') as WeekEndDate, Sum(TotErectedRegister) as TotErectedRegister, Sum(TotDismantleRegister) as TotDismantleRegister, convert(decimal(18, 2),Sum(TotErectedTonnage) / 1000) as TotErectedTonnage, convert(decimal(18, 2),Sum(TotDismantleTonnage) / 1000) as TotDismantleTonnage, Sum(TotManHrsErect) as TotManHrsErect, Sum(TotManHrsDismantle) as TotManHrsDismantle from(select WeekNo, WeekStartDate, WeekEndDate, Count(RegisterID) as TotErectedRegister, Sum(isnull(Tonnage, 0)) as TotErectedTonnage, 0 as TotDismantleRegister, 0 as TotDismantleTonnage, Sum(isnull(ManHrsErect, 0)) as TotManHrsErect, 0 as TotManHrsDismantle from(select * from Fn_TenWeekDate('" + weekdays + "'))WeeksDate Left join vw_tblRegister on WeeksDate.weekStartDate <= datetagged and WeeksDate.weekEndDate >= datetagged and  clientId = '" + id + "' and (SiteLocId='" + site + "' or 'All'='" + site + "') and ('" + jobid + "' = 'All'  or LocationId = '" + jobid + "') group by weekNo, weekStartDate, weekEndDate union all select WeekNo, WeekStartDate, WeekEndDate , 0 as TotErectedRegister, 0 as TotErectedTonnage , Count(RegisterID) as TotDismantleRegister, Sum(isnull(Tonnage, 0)) as TotDismantleTonnage , 0 as TotManHrsErect, Sum(isnull(ManHrsDismantle, 0)) as TotManHrsDismantle from (select * from Fn_TenWeekDate('" + weekdays + "'))WeeksDate Left join vw_tblRegister on WeeksDate.weekStartDate <= dateofdismantle and WeeksDate.weekEndDate >= dateofdismantle and  clientId = '" + id + "' and (SiteLocId='" + site + "' or 'All'='" + site + "') and ('" + jobid + "' = 'All'  or LocationId = '" + jobid + "') group by weekNo, weekStartDate, weekEndDate )AllQuery group by weekNo, weekStartDate, weekEndDate order by weekno";

            //string qry = "select WeekNo,replace(CONVERT(varchar(11),WeekStartDate,106),' ','-') as WeekStartDate,replace(CONVERT(varchar(11),WeekEndDate,106),' ','-') as WeekEndDate, Sum(TotErectedRegister) as TotErectedRegister, Sum(TotDismantleRegister) as TotDismantleRegister, convert(decimal(18, 2),Sum(TotErectedTonnage) / 1000) as TotErectedTonnage, convert(decimal(18, 2),Sum(TotDismantleTonnage) / 1000) as TotDismantleTonnage, Sum(TotManHrsErect) as TotManHrsErect, Sum(TotManHrsDismantle) as TotManHrsDismantle from(select WeekNo, WeekStartDate, WeekEndDate, Count(RegisterID) as TotErectedRegister, Sum(isnull(Tonnage, 0)) as TotErectedTonnage, 0 as TotDismantleRegister, 0 as TotDismantleTonnage, Sum(isnull(ManHrsErect, 0)) as TotManHrsErect, 0 as TotManHrsDismantle from(select * from Fn_TenWeekDate('" + weekdays + "','" + getnewdate + "'))WeeksDate Left join vw_tblRegister on WeeksDate.weekStartDate <= datetagged and WeeksDate.weekEndDate >= datetagged and  clientId = '" + id + "' and (SiteLocId='" + site + "' or 'All'='" + site + "') and ('" + jobid + "' = 'All'  or LocationId = '" + jobid + "') group by weekNo, weekStartDate, weekEndDate union all select WeekNo, WeekStartDate, WeekEndDate , 0 as TotErectedRegister, 0 as TotErectedTonnage , Count(RegisterID) as TotDismantleRegister, Sum(isnull(Tonnage, 0)) as TotDismantleTonnage , 0 as TotManHrsErect, Sum(isnull(ManHrsDismantle, 0)) as TotManHrsDismantle from (select * from Fn_TenWeekDate('" + weekdays + "','" + getnewdate + "'))WeeksDate Left join vw_tblRegister on WeeksDate.weekStartDate <= dateofdismantle and WeeksDate.weekEndDate >= dateofdismantle and  clientId = '" + id + "' and (SiteLocId='" + site + "' or 'All'='" + site + "') and ('" + jobid + "' = 'All'  or LocationId = '" + jobid + "') group by weekNo, weekStartDate, weekEndDate )AllQuery group by weekNo, weekStartDate, weekEndDate order by weekno";


            //added on 25nov24 by pooja,  requested that  the Weekly Progress and Weekly Summary Graphs reflect the actual Planning Week Number rather than just showing the last 10 weeks for prelude
            string qry = "select WeekNo,replace(CONVERT(varchar(11),WeekStartDate,106),' ','-') as WeekStartDate,replace(CONVERT(varchar(11),WeekEndDate,106),' ','-') as WeekEndDate, Sum(TotErectedRegister) as TotErectedRegister, Sum(TotDismantleRegister) as TotDismantleRegister, convert(decimal(18, 2),Sum(TotErectedTonnage) / 1000) as TotErectedTonnage, convert(decimal(18, 2),Sum(TotDismantleTonnage) / 1000) as TotDismantleTonnage, Sum(TotManHrsErect) as TotManHrsErect, Sum(TotManHrsDismantle) as TotManHrsDismantle from(select WeekNo, WeekStartDate, WeekEndDate, Count(RegisterID) as TotErectedRegister, Sum(isnull(Tonnage, 0)) as TotErectedTonnage, 0 as TotDismantleRegister, 0 as TotDismantleTonnage, Sum(isnull(ManHrsErect, 0)) as TotManHrsErect, 0 as TotManHrsDismantle from(select * from Fn_TenWeekDate('" + id + "','" + weekdays + "','" + getnewdate + "'))WeeksDate Left join vw_tblRegister on WeeksDate.weekStartDate <= datetagged and WeeksDate.weekEndDate >= datetagged and  clientId = '" + id + "' and (SiteLocId='" + site + "' or 'All'='" + site + "') and ('" + jobid + "' = 'All'  or LocationId = '" + jobid + "') and ('" + WorkGroup + "' = 'All'  or WorkGroupId  = '" + WorkGroup + "') group by weekNo, weekStartDate, weekEndDate union all select WeekNo, WeekStartDate, WeekEndDate , 0 as TotErectedRegister, 0 as TotErectedTonnage , Count(RegisterID) as TotDismantleRegister, Sum(isnull(Tonnage, 0)) as TotDismantleTonnage , 0 as TotManHrsErect, Sum(isnull(ManHrsDismantle, 0)) as TotManHrsDismantle from (select * from Fn_TenWeekDate('" + id + "','" + weekdays + "','" + getnewdate + "'))WeeksDate Left join vw_tblRegister on WeeksDate.weekStartDate <= dateofdismantle and WeeksDate.weekEndDate >= dateofdismantle and  clientId = '" + id + "' and (SiteLocId='" + site + "' or 'All'='" + site + "') and ('" + jobid + "' = 'All'  or LocationId = '" + jobid + "') and ('" + WorkGroup + "' = 'All'  or WorkGroupId  = '" + WorkGroup + "') group by weekNo, weekStartDate, weekEndDate )AllQuery group by weekNo, weekStartDate, weekEndDate order by weekno";

            DataSet ds = null;
            ds = databaseCon.GetDataSet(qry);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ScaffoldAllDashBoardViewModel pro = new ScaffoldAllDashBoardViewModel();
                    if (!(item["WeekNo"] is DBNull) && item["WeekNo"].ToString() != "")
                    {
                        pro.WeekNo = Convert.ToInt32(item["WeekNo"].ToString());
                    }
                    if (!(item["TotErectedTonnage"] is DBNull) && item["TotErectedTonnage"].ToString() != "")
                    {
                        pro.TotErectedTonnage = Convert.ToDecimal(item["TotErectedTonnage"].ToString());
                    }
                    if (!(item["TotDismantleTonnage"] is DBNull) && item["TotDismantleTonnage"].ToString() != "")
                    {
                        pro.TotDismantleTonnage = Convert.ToDecimal(item["TotDismantleTonnage"].ToString());
                    }
                    if (!(item["TotManHrsErect"] is DBNull) && item["TotManHrsErect"].ToString() != "")
                    {
                        pro.TotManHrsErect = Convert.ToDecimal(item["TotManHrsErect"].ToString());
                    }
                    if (!(item["TotManHrsDismantle"] is DBNull) && item["TotManHrsDismantle"].ToString() != "")
                    {
                        pro.TotManHrsDismantle = Convert.ToDecimal(item["TotManHrsDismantle"].ToString());
                    }
                    if (!(item["TotErectedRegister"] is DBNull) && item["TotErectedRegister"].ToString() != "")
                    {
                        pro.TotErectedRegister = Convert.ToDecimal(item["TotErectedRegister"].ToString());
                    }
                    if (!(item["TotDismantleRegister"] is DBNull) && item["TotDismantleRegister"].ToString() != "")
                    {
                        pro.TotDismantleRegister = Convert.ToDecimal(item["TotDismantleRegister"].ToString());
                    }
                    if (!(item["weekStartDate"] is DBNull) && item["weekStartDate"].ToString() != "")
                    {
                        pro.WeekStartDate = Convert.ToDateTime(item["weekStartDate"]).ToString("dd-MMM-yyyy");
                    }
                    if (!(item["weekEndDate"] is DBNull) && item["weekEndDate"].ToString() != "")
                    {
                        pro.WeekEndDate = Convert.ToDateTime(item["weekEndDate"]).ToString("dd-MMM-yyyy");
                    }
                    WeeklyDashboard.Add(pro);
                }
            }
            // return WeeklyDashboard;
            return Json(WeeklyDashboard, JsonRequestBehavior.AllowGet);
        }

        public ActionResult generateActualWeekPlan(string id, string UID)
        {
            id = databaseCon.Decrypt(id);
            string checkActualPlanningWeekQuery = "select ClientId from tblYearWeekDate2024 where ClientId = '" + id + "'";
            var check = databaseCon.GetData(checkActualPlanningWeekQuery);
            if (check != null && check.Rows.Count > 0)
            {
                string LoginUserQuery = "select LoginId from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'";
                DataTable LoginUserInfo = databaseCon.GetData(LoginUserQuery);
                string maxEndDateQuery = "select max(WeekEndDate) from tblYearWeekDate2024 where ClientId = '" + id + "'";
                DataTable maxEndDateResult = databaseCon.GetData(maxEndDateQuery);
                var maxEndDate = maxEndDateResult.Rows[0][0];
                DateTime FromDate = Convert.ToDateTime(maxEndDate).AddDays(1);
                string Fromdate = FromDate.ToString("yyyy-MM-dd");
                DateTime today = DateTime.Now;
                if (today >= Convert.ToDateTime(maxEndDate))
                {
                    string generateQuery = $"exec Pro_GenerateActualPlanningWeek @Fromdate='{Fromdate}',@ClientId='{id}',@GeneratedBy='{LoginUserInfo.Rows[0][0].ToString()}'";
                    var generate = databaseCon.GetData(generateQuery);
                }
            }

            return Json(new { Result = "Success", Message = "Actual Planning week generated successfully." });
        }
        public ActionResult ddlWorkgroupDashboard(string ClientID, string SiteId, string ddlselectarea)
        {
            var WorkData = dbcontext.Database.SqlQuery<DropDownViewModel>("select distinct tblRegister.WorkGroupID,tblWorkGroup.WorkGroup + ' (' + SiteLocation + ')' as WorkGroup from tblRegister join tblWorkGroup on tblRegister.clientid=tblWorkGroup.clientid and tblRegister.SiteLocId=tblWorkGroup.SiteLocId and tblRegister.WorkGroupId=tblWorkGroup.WorkGroupId join tblsitelocation on tblsitelocation.sitelocid = tblregister.sitelocid where tblRegister.clientid = '" + ClientID + "' and (tblRegister.SiteLocId='" + SiteId + "' or 'All'='" + SiteId + "')  and (LocationId = '" + ddlselectarea + "' or 'All' = '" + ddlselectarea + "') and (tblWorkGroup.WorkGroup is not null) order by tblWorkGroup.WorkGroup + ' (' + SiteLocation + ')'").ToList();

            return Json(WorkData, JsonRequestBehavior.AllowGet);
        }

    }
}