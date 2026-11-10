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
    public class WeeklySummaryReportController : Controller
    {
        ScaffoldEntities dbcontext = new ScaffoldEntities();
        // GET: WeeklySummaryReport
        public ActionResult WeeklySummaryReport()
        {
            string ClientQuery = "select ClientImage from tblClient where ClientId = '" + databaseCon.Decrypt(Request.QueryString["id"]) + "'";
            DataTable clientdt = databaseCon.GetData(ClientQuery);

            if (clientdt.Rows.Count > 0)
            {
                if (clientdt.Rows[0]["ClientImage"].ToString() != "")
                {
                    var uri = new Uri(databaseCon.clientimageLink);
                    string imgname = clientdt.Rows[0]["ClientImage"].ToString();
                    if (clientdt.Rows[0]["ClientImage"].ToString() == "")
                    {
                        //imgLogo.Attributes.Add("src", "/images/logo.png");
                        ViewBag.ClientImage = "/images / logo.png";
                    }
                    else
                    {
                        //imgLogo.Attributes.Add("src", (("/images/clientlogo/") + clientdt.Rows[0]["ClientImage"].ToString()));
                        ViewBag.ClientImage = "/images/clientlogo/" + clientdt.Rows[0]["ClientImage"].ToString();
                    }
                }
                else
                {
                    //imgLogo.Attributes.Add("src", "images/logo.png");
                    ViewBag.ClientImage = "/images / logo.png";
                }
            }
            return View();
        }


        public ActionResult ddlsitedropdownbindquery(string id)
        {
            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select SiteLocID,SiteLocation from dbo.tblSiteLocation where ClientId='" + id + "' order by SiteLocation").ToList();
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }

        List<ScaffoldAllDashBoardViewModel> SitedataDetails = new List<ScaffoldAllDashBoardViewModel> { };
        // [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        // [WebMethod(EnableSession = true)]
        public ActionResult SiteDetails(string Siteid, string ClientID)
        {
            //change on nov 15
            //string query = "Select MAX(Erectedscaffold)as Erectedscaffold ,Max(StandingScaffold) as StandingScaffold,MAX(ErrectedTonnage) as ErrectedTonnage, MAX(StandingTonnage) as StandingTonnage,MAX(AverageTonnage) as AverageTonnage,MAX(AvgHireDays)as AvgHireDays    from (select COUNT(*) as Erectedscaffold, 0 as StandingScaffold, 0 as ErrectedTonnage, 0 as StandingTonnage, 0 as AverageTonnage, 0 as AvgHireDays   from VW_TblRegister where  ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(DateTagged <> '01/01/1900' and DateTagged is not null) union all select 0 as Erectedscaffold, COUNT(*) as StandingScaffold, 0 as ErrectedTonnage, 0 as StandingTonnage, 0 as AverageTonnage, 0 as AvgHireDays from VW_TblRegister where  ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "')  and(DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) union all select 0 as Erectedscaffold, 0 as StandingScaffold, cast(sum(isnull(Tonnage, 0)) / 1000 as decimal(18, 2)) as ErrectedTonnage ,0 as StandingTonnage, 0 as AverageTonnage,0 as AvgHireDays from VW_TblRegister where  ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(DateTagged <> '01/01/1900' and DateTagged is not null) union all select 0 as Erectedscaffold,0 as StandingScaffold,0 as ErrectedTonnage,cast(sum(isnull(Tonnage, 0)) / 1000 as decimal(18, 2)) as StandingTonnage, 0 as AverageTonnage,0 as AvgHireDays from VW_TblRegister where  ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "')  and(DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) union all select 0 as Erectedscaffold,0 as StandingScaffold,0 as ErrectedTonnage,0 as StandingTonnage,case when sum(isnull(Tonnage, 0)) = 0 then 0 else cast(sum(isnull(Tonnage, 0)) / (1000 * COUNT(*)) as decimal(18, 2)) End as AverageTonnage ,0 as AvgHireDays from VW_TblRegister where  ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(DateTagged <> '01/01/1900' and DateTagged is not null)  union all select 0 as Erectedscaffold,0 as StandingScaffold,0 as ErrectedTonnage,0 as StandingTonnage, 0 as AverageTonnage,convert(decimal(18), cast(Convert(decimal(18, 2), SUM(HireDays)) / Convert(decimal(18, 2), AVG(Counts)) as float)) as AvgHireDays from(select SUM(isnull(HireDaysToDate, 0)) as HireDays, COUNT(*) as Counts from VW_TblRegister where  ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "')  and(DateTagged <> '01/01/1900' or DateTagged is not null)   union all select Sum(isnull(TotalHireDays, 0)) as HireDays, COUNT(*) as Counts from VW_TblRegister where  ClientId = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(DateTagged <> '01/01/1900' and DateTagged is not null))AverageDays ) forSiteQuery ";

            string query = "Select MAX(Erectedscaffold)as Erectedscaffold ,Max(StandingScaffold) as StandingScaffold,MAX(ErrectedTonnage) as ErrectedTonnage, MAX(StandingTonnage) as StandingTonnage,MAX(AverageTonnage) as AverageTonnage,MAX(AvgHireDays)as AvgHireDays    from (select COUNT(*) as Erectedscaffold, 0 as StandingScaffold, 0 as ErrectedTonnage, 0 as StandingTonnage, 0 as AverageTonnage, 0 as AvgHireDays   from TblRegister where  ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(DateTagged <> '01/01/1900' and DateTagged is not null) union all select 0 as Erectedscaffold, COUNT(*) as StandingScaffold, 0 as ErrectedTonnage, 0 as StandingTonnage, 0 as AverageTonnage, 0 as AvgHireDays from TblRegister where  ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "')  and(DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) union all select 0 as Erectedscaffold, 0 as StandingScaffold, cast(sum(isnull(Tonnage, 0)) / 1000 as decimal(18, 2)) as ErrectedTonnage ,0 as StandingTonnage, 0 as AverageTonnage,0 as AvgHireDays from TblRegister where  ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(DateTagged <> '01/01/1900' and DateTagged is not null) union all select 0 as Erectedscaffold,0 as StandingScaffold,0 as ErrectedTonnage,cast(sum(isnull(Tonnage, 0)) / 1000 as decimal(18, 2)) as StandingTonnage, 0 as AverageTonnage,0 as AvgHireDays from TblRegister where  ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "')  and(DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) union all select 0 as Erectedscaffold,0 as StandingScaffold,0 as ErrectedTonnage,0 as StandingTonnage,case when sum(isnull(Tonnage, 0)) = 0 then 0 else cast(sum(isnull(Tonnage, 0)) / (1000 * COUNT(*)) as decimal(18, 2)) End as AverageTonnage ,0 as AvgHireDays from TblRegister where  ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(DateTagged <> '01/01/1900' and DateTagged is not null)  union all select 0 as Erectedscaffold,0 as StandingScaffold,0 as ErrectedTonnage,0 as StandingTonnage, 0 as AverageTonnage,convert(decimal(18), cast(Convert(decimal(18, 2), SUM(HireDays)) / Convert(decimal(18, 2), AVG(Counts)) as float)) as AvgHireDays from(select SUM(isnull(CASE WHEN DateTagged !='1900-01-01' AND DateOfDismantle ='1900-01-01'   THEN DateDiff(dd, DateTagged, CONVERT(date, Getdate()))   ELSE NULL END, 0)) as HireDays, COUNT(*) as Counts from TblRegister where  ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "')  and(DateTagged <> '01/01/1900' or DateTagged is not null)   union all select Sum(isnull(CASE WHEN DateTagged !='1900-01-01' AND DateOfDismantle !='1900-01-01'   THEN DateDiff(dd,DateTagged, DateOfDismantle) + 1 ELSE NULL END, 0)) as HireDays, COUNT(*) as Counts from TblRegister where  ClientId = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(DateTagged <> '01/01/1900' and DateTagged is not null))AverageDays ) forSiteQuery ";

            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ScaffoldAllDashBoardViewModel pro = new ScaffoldAllDashBoardViewModel();
                    if (!(item["Erectedscaffold"] is DBNull) && item["Erectedscaffold"].ToString() != "")
                    {
                        pro.ErectedDate1 = item["Erectedscaffold"].ToString();
                    }
                    if (!(item["StandingScaffold"] is DBNull) && item["StandingScaffold"].ToString() != "")
                    {
                        pro.ScaffoldsStanding1 = item["StandingScaffold"].ToString();
                    }
                    if (!(item["ErrectedTonnage"] is DBNull) && item["ErrectedTonnage"].ToString() != "")
                    {
                        pro.TonnageErected1 = item["ErrectedTonnage"].ToString();
                    }
                    if (!(item["StandingTonnage"] is DBNull) && item["StandingTonnage"].ToString() != "")
                    {
                        pro.TonnageStanding1 = item["StandingTonnage"].ToString();
                    }
                    if (!(item["AverageTonnage"] is DBNull) && item["AverageTonnage"].ToString() != "")
                    {
                        pro.AverageTonnage1 = item["AverageTonnage"].ToString();
                    }
                    if (!(item["AvgHireDays"] is DBNull) && item["AvgHireDays"].ToString() != "")
                    {
                        pro.HireDays1 = item["AvgHireDays"].ToString();
                    }
                    SitedataDetails.Add(pro);
                }
            }
            return Json(SitedataDetails, JsonRequestBehavior.AllowGet);
        }
        List<ScaffoldAllDashBoardViewModel> TonnageAllsites = new List<ScaffoldAllDashBoardViewModel> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod(EnableSession = true)]
        //public List<Scaffold> AllSitesDashboard(string Siteid, string ClientID)
        public ActionResult AllSitesDashboard(string Siteid, string ClientID)
        {
            string query = "select MaterialType,TonnageStanding,TotalTonnage,TonnageStanding/TotalTonnage as Pct from ( select MaterialType,convert(decimal(18,2),Sum(totalWeight)/1000) as TonnageStanding from vw_jobInventory where ToId in (select RegisterID from tblRegister where ClientId=" + ClientID + "  and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and (DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null)) Group by MaterialType)BasedOnMaterial,(select Sum(totalWeight)/1000 as TotalTonnage from vw_jobInventory where ToId in (select RegisterID from tblRegister where ClientId=" + ClientID + "  and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "')and(DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null)))ForTotal";

            //query = "select case when MaterialType!='' and MaterialType is not null then MaterialType else 'Material Type not assigned to Job' end as MaterialType,convert(decimal(18,2),Sum(isnull(Tonnage,0))/1000) TonnageStanding ,case when totalTonnageStanding >0 then cast(round(convert(decimal(18, 2), Sum(isnull(Tonnage, 0)) / 1000) / totalTonnageStanding, 2) as numeric(36, 2)) else 0 end as Pct,totalTonnageStanding from VW_TblRegister ,(select  convert(decimal(18, 2), Sum(isnull(Tonnage, 0)) / 1000) totalTonnageStanding from VW_TblRegister where ClientID = '" + ClientID + "' and('"+ Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "')and(DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) )temp where ClientID = '" + ClientID + "' and('"+ Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "')and(DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) group by case when MaterialType!='' and MaterialType is not null then MaterialType else 'Material Type not assigned to Job' end,totalTonnageStanding order by Sum(isnull(Tonnage, 0)) / 1000 desc, MaterialType";

            //select MaterialType,convert(decimal(18,2),Sum(isnull(Tonnage,0))/1000) TonnageStanding from VW_TblRegister where ClientID='" + ClientID + "' and ('"+ Siteid + "'='all'  or SiteLocId='" + Siteid + "' )and (DateTagged<>'01/01/1900' and DateTagged is not null) and (DateOfDismantle='01/01/1900' or DateOfDismantle is null) group by MaterialType order by Sum(isnull(Tonnage,0))/1000 desc , MaterialType";

            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ScaffoldAllDashBoardViewModel pro = new ScaffoldAllDashBoardViewModel();
                    if (!(item["MaterialType"] is DBNull) && item["MaterialType"].ToString() != "")
                    {
                        pro.MaterialType = item["MaterialType"].ToString();
                    }
                    if (!(item["TonnageStanding"] is DBNull) && item["TonnageStanding"].ToString() != "")
                    {
                        pro.TonnageStanding = Convert.ToDecimal(item["TonnageStanding"]);
                    }
                    if (!(item["Pct"] is DBNull) && item["Pct"].ToString() != "")
                    {
                        pro.PctAllSite = Convert.ToDecimal(item["Pct"]);
                    }
                    TonnageAllsites.Add(pro);
                }
            }
            // return TonnageAllsites;
            return Json(TonnageAllsites, JsonRequestBehavior.AllowGet);
        }
        List<ScaffoldAllDashBoardViewModel> LockedInDashboardClick = new List<ScaffoldAllDashBoardViewModel> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod(EnableSession = true)]
        //public List<Scaffold> LockedInClick(string Siteid, string ClientID)
        public ActionResult LockedInClick(string Siteid, string ClientID)
        {

            string query = "select Right ('000000' + convert (varchar, [ScaffRef]),6) as ScaffRef,Description,Location,BuildingNo,AssetNo,FloorLevel,DesignType,MaterialType from VW_TblRegister where clientid='" + ClientID + "' and DateTagged <> '01/01/1900' and DateTagged is not null and('" + Siteid + "' = 'All'  or SiteLocId = '" + Siteid + "') and LockedIn = '1' Order by Location,BuildingNo ";

            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ScaffoldAllDashBoardViewModel pro = new ScaffoldAllDashBoardViewModel();
                    if (!(item["ScaffRef"] is DBNull) && item["ScaffRef"].ToString() != "")
                    {
                        pro.ScaffRefAction = item["ScaffRef"].ToString();
                    }
                    if (!(item["Description"] is DBNull) && item["Description"].ToString() != "")
                    {
                        pro.DescriptionAction = item["Description"].ToString();
                    }
                    else
                    {
                        pro.DescriptionAction = "";
                    }
                    if (!(item["Location"] is DBNull) && item["Location"].ToString() != "")
                    {
                        pro.Location = item["Location"].ToString();
                    }
                    if (!(item["BuildingNo"] is DBNull) && item["BuildingNo"].ToString() != "")
                    {
                        pro.BuildingNo = item["BuildingNo"].ToString();
                    }
                    else
                    {
                        pro.BuildingNo = "";
                    }
                    if (!(item["AssetNo"] is DBNull) && item["AssetNo"].ToString() != "")
                    {
                        pro.assetId = item["AssetNo"].ToString();
                    }
                    else
                    {
                        pro.assetId = "";
                    }
                    if (!(item["FloorLevel"] is DBNull) && item["FloorLevel"].ToString() != "")
                    {
                        pro.FloorLevel = item["FloorLevel"].ToString();
                    }
                    if (!(item["DesignType"] is DBNull) && item["DesignType"].ToString() != "")
                    {
                        pro.DesignType = item["DesignType"].ToString();
                    }
                    else
                    {
                        pro.DesignType = "";
                    }
                    if (!(item["MaterialType"] is DBNull) && item["MaterialType"].ToString() != "")
                    {
                        pro.MaterialType = item["MaterialType"].ToString();
                    }
                    else
                    {
                        pro.MaterialType = "";
                    }
                    LockedInDashboardClick.Add(pro);
                }
            }
            // return LockedInDashboardClick;
            return Json(LockedInDashboardClick, JsonRequestBehavior.AllowGet);
        }
        List<ScaffoldAllDashBoardViewModel> LocationBySites = new List<ScaffoldAllDashBoardViewModel> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod(EnableSession = true)]
        //public List<Scaffold> GetBySitesDashboard(string Siteid, string ClientID)
        public ActionResult GetBySitesDashboard(string Siteid, string ClientID)
        {
            // 09 nov Total Scaffold Built to Date Scaffold     
            //string query = "Select Max(TotalRegister) as TotalRegister, Max(LockedInRegister) as LockedInRegister, Case when Max(TotalRegister) = 0 Or Max(LockedInRegister) = 0 then 0 else  Round((Convert(float, Max(LockedInRegister)) * 100.00) / Convert(float, Max(TotalRegister)), 0) end as LockedPct From(select COUNT(*) as TotalRegister, '' as LockedInRegister from tblregister where ClientID = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "')  and(DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) union all select '' as TotalRegister, COUNT(*) as LockedInRegister from tblregister where ClientID = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "')  and(DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) and LockedIn = 1) LockedQuery";

            string query = "Select Max(TotalRegister) as TotalRegister, Max(LockedInRegister) as LockedInRegister, Case when Max(TotalRegister) = 0 Or Max(LockedInRegister) = 0 then 0 else  Round((Convert(float, Max(LockedInRegister)) * 100.00) / Convert(float, Max(TotalRegister)), 0) end as LockedPct From(select COUNT(*) as TotalRegister, '' as LockedInRegister from tblregister where ClientID = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "')  and(DateTagged <> '01/01/1900' and DateTagged is not null)  union all select '' as TotalRegister, COUNT(*) as LockedInRegister from tblregister where ClientID = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "')  and(DateTagged <> '01/01/1900' and DateTagged is not null)  and LockedIn = 1) LockedQuery";

            DataSet ds = null;
            dbcontext.Database.CommandTimeout = 15 * 60;
            ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ScaffoldAllDashBoardViewModel pro = new ScaffoldAllDashBoardViewModel();
                    if (!(item["TotalRegister"] is DBNull) && item["TotalRegister"].ToString() != "")
                    {
                        pro.TotalRegisterforsite = item["TotalRegister"].ToString();
                    }
                    if (!(item["LockedInRegister"] is DBNull) && item["LockedInRegister"].ToString() != "")
                    {
                        pro.LockedinReg = item["LockedInRegister"].ToString();
                    }
                    if (!(item["LockedPct"] is DBNull) && item["LockedPct"].ToString() != "")
                    {
                        pro.LockedPerct = item["LockedPct"].ToString();
                    }

                    LocationBySites.Add(pro);
                }
            }
            //  return LocationBySites;
            return Json(LocationBySites, JsonRequestBehavior.AllowGet);
        }
        List<ScaffoldAllDashBoardViewModel> WeeklyDashboard = new List<ScaffoldAllDashBoardViewModel> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod(EnableSession = true)]
        //public List<Scaffold> WeeklyProgress(string id, string site, string jobid, string getnewdate)
        public ActionResult WeeklyProgress(string id, string site, string jobid, string getnewdate)
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

            //added on 25nov24 by pooja,  requested that  the Weekly Progress and Weekly Summary Graphs reflect the actual Planning Week Number rather than just showing the last 10 weeks for prelude
            string qry = "select WeekNo,replace(CONVERT(varchar(11),WeekStartDate,106),' ','-') as WeekStartDate,replace(CONVERT(varchar(11),WeekEndDate,106),' ','-') as WeekEndDate, Sum(TotErectedRegister) as TotErectedRegister, Sum(TotDismantleRegister) as TotDismantleRegister, convert(decimal(18, 2),Sum(TotErectedTonnage) / 1000) as TotErectedTonnage, convert(decimal(18, 2),Sum(TotDismantleTonnage) / 1000) as TotDismantleTonnage, Sum(TotManHrsErect) as TotManHrsErect, Sum(TotManHrsDismantle) as TotManHrsDismantle from(select WeekNo, WeekStartDate, WeekEndDate, Count(RegisterID) as TotErectedRegister, Sum(isnull(Tonnage, 0)) as TotErectedTonnage, 0 as TotDismantleRegister, 0 as TotDismantleTonnage, Sum(isnull(ManHrsErect, 0)) as TotManHrsErect, 0 as TotManHrsDismantle from(select * from Fn_TenWeekDate('" + id + "','" + weekdays + "','" + getnewdate + "'))WeeksDate Left join tblRegister on WeeksDate.weekStartDate <= datetagged and WeeksDate.weekEndDate >= datetagged and  clientId = '" + id + "' and (SiteLocId='" + site + "' or 'All'='" + site + "') and ('" + jobid + "' = 'All'  or LocationId = '" + jobid + "') group by weekNo, weekStartDate, weekEndDate union all select WeekNo, WeekStartDate, WeekEndDate , 0 as TotErectedRegister, 0 as TotErectedTonnage , Count(RegisterID) as TotDismantleRegister, Sum(isnull(Tonnage, 0)) as TotDismantleTonnage , 0 as TotManHrsErect, Sum(isnull(ManHrsDismantle, 0)) as TotManHrsDismantle from (select * from Fn_TenWeekDate('" + id + "','" + weekdays + "','" + getnewdate + "'))WeeksDate Left join tblRegister on WeeksDate.weekStartDate <= dateofdismantle and WeeksDate.weekEndDate >= dateofdismantle and  clientId = '" + id + "' and (SiteLocId='" + site + "' or 'All'='" + site + "') and ('" + jobid + "' = 'All'  or LocationId = '" + jobid + "') group by weekNo, weekStartDate, weekEndDate )AllQuery group by weekNo, weekStartDate, weekEndDate order by weekno";
            
            DataSet ds = null;
            //dbcontext.Database.CommandTimeout = 15 * 60;
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
            //return Json(new { YearWeekClientId, CurrentYearWeek, WeeklyDashboard }, JsonRequestBehavior.AllowGet);
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
                string maxEndDateQuery = "select max(WeekEndDate) from tblYearWeekDate2024 where ClientId = '"+ id + "'";
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
    }
}