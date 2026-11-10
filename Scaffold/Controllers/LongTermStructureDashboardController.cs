using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scaffold.Controllers
{
    public class LongTermStructureDashboardController : Controller
    {
        ScaffoldEntities dbcontext = new ScaffoldEntities();
        // GET: LongTermStructureDashboard
        public ActionResult LongTermStructureDashboard()
        {
            int clientId;

            if (!string.IsNullOrEmpty(HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["id"]))) && Request.QueryString["id"] != null)
                {
                clientId = Convert.ToInt32(HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["id"])));
                }
            else
                {
                string query = "select distinct(Name+' '+'('+status+')') as name,ClientId from tblclient where ClientId!='1' Order by name";
                DataTable dt = databaseCon.GetData(query);
                clientId = Convert.ToInt32(dt.Rows[0]["ClientId"]);

                }

            // hdnClientID.Value = Convert.ToString(clientId);

            ViewBag.ClientID = Convert.ToString(clientId);

            string ClientQuery = "select ClientImage from tblClient where ClientId = '" + clientId + "'";
            DataTable clientdt = databaseCon.GetData(ClientQuery);

            if (clientdt.Rows.Count > 0)
                {
                if (clientdt.Rows[0]["ClientImage"].ToString() != "")
                    {
                    var uri = new Uri(databaseCon.clientimageLink);
                    string imgname = clientdt.Rows[0]["ClientImage"].ToString();
                   
                    if (clientdt.Rows[0]["ClientImage"].ToString() == "")
                        {

                        //ViewBag.ClientImage = "/images/logo.png";
                        ViewBag.ClientImage = databaseCon.logoimage;
                    }
                    else
                        {

                        ViewBag.ClientImage = "/images/clientlogo/" + clientdt.Rows[0]["ClientImage"].ToString();
                        }
                    }
                else
                    {

                    //ViewBag.ClientImage = "/images/logo.png";
                    ViewBag.ClientImage = databaseCon.logoimage;
                }
                }

            return View();
        }


        public ActionResult ddlsitedropdownbindquery(string id, string ClientID)
        {
            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select SiteLocID,SiteLocation from tblSiteLocation where ClientId='" + ClientID + "' order by SiteLocation").ToList();

            return Json(GetData, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ddlJobDashboard(string id, string SiteId)
        {

            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select distinct Location as area,LocationId as areaid from VW_TblRegister where (Tonnage is not null or ManHrsDismantle is not null) and (SiteLocId='" + SiteId + "' or 'All'='" + SiteId + "') and ClientID='" + id + "' Order by Location").ToList();

            return Json(GetData, JsonRequestBehavior.AllowGet);

        }
        public ActionResult ddlDisciplinesdropdownbindquery(string Siteid, string ClientID, string LocId)
        {
            //var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select distinct Case when ScaffoldOwner='C&I' then 'C&I' else 'Maintenance' End as ScaffoldOwner from tblRegister where ClientId = '" + ClientID + "' and (SiteLocId='" + Siteid + "' or 'All'='" + Siteid + "') and  ScaffoldOwner is not null and ScaffoldOwner!= ''  Order by Case when ScaffoldOwner='C&I' then 'C&I' else 'Maintenance' End").ToList();

            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select distinct Case when ScaffoldOwner='C&I' then 'C&I' else 'Maintenance' End as ScaffoldOwner from tblRegister where ClientId = '" + ClientID + "' and (SiteLocId='" + Siteid + "' or 'All'='" + Siteid + "') and (LocationId='" + LocId + "' or 'all'='" + LocId + "') and  ScaffoldOwner is not null and ScaffoldOwner!= ''  Order by Case when ScaffoldOwner='C&I' then 'C&I' else 'Maintenance' End").ToList();

            return Json(GetData, JsonRequestBehavior.AllowGet);

        }

        public ActionResult CycloneHours( string Siteid, string ClientID, string SubOwner, string ddlarea) {
            var PreparationHrs = "";
            DataTable dt;
            string query = "Select Convert(decimal(18, 2), sum(Isnull(TotalLabourDismantle,0)+Isnull(TotalLabourDismantleEdit,0))) as PreparationHrs from tblRegister where ClientID = '" + ClientID + "' and(SiteLocId = '" + Siteid + "' or 'all' = '" + Siteid + "') and Isnull(LongTermStructure,0)= 1 and(DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) and(Case when ScaffoldOwner = 'C&I' then 'C&I' else 'Maintenance' End = '" + SubOwner.Replace("'", "''") + "' or 'all' = '" + SubOwner.Replace("'", "''") + "') and (LocationId = '" + ddlarea + "' or 'all' = '" + ddlarea + "')";
            dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
                {
                 PreparationHrs = dt.Rows[0]["PreparationHrs"].ToString();
                }

            var result = new { PreparationHrs = PreparationHrs };
            return Json(result, JsonRequestBehavior.AllowGet);
            }



        List<ScaffoldAllDashBoardViewModel> SitedataDetails = new List<ScaffoldAllDashBoardViewModel> { };
        public ActionResult SiteDetails(string Siteid, string ClientID ,string SubOwner, string ddlarea)
        {

            //string query = "Select 'Total Standing' as GraphName, MAX(StandingScaffold) as StandingScaffold , MAX(StandingTonnage) as StandingTonnage,MAX(ATPACTonnage) as ATPACTonnage,MAX(TubeFitTonnage) as TubeFitTonnage,MAX(AvgHireDays) as AvgHireDays from( select COUNT('x') as StandingScaffold, cast(sum(isnull(Tonnage, 0)) / 1000 as decimal(18, 2)) as StandingTonnage,0 as ATPACTonnage,0 as TubeFitTonnage, 0 as AvgHireDays from TblRegister where ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(LocationId = '" + ddlarea + "' or 'all' = '" + ddlarea + "') and (Case when ScaffoldOwner = 'C&I' then 'C&I' else 'Maintenance' End = '" + SubOwner + "' or 'all' = '" + SubOwner + "') and (DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) union all select 0 as StandingScaffold,0 as StandingTonnage,convert(decimal(18, 2), Sum(totalWeight) / 1000) as ATPACTonnage,0 as TubeFitTonnage, 0 as AvgHireDays from vw_jobInventory where ToId in (select RegisterID from tblRegister where  ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(LocationId = '" + ddlarea + "' or 'all' = '" + ddlarea + "') and(Case when ScaffoldOwner = 'C&I' then 'C&I' else 'Maintenance' End = '" + SubOwner + "' or 'all' = '" + SubOwner + "') and (DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) ) and MaterialType = 'At-Pac' union all select 0 as StandingScaffold,0 as StandingTonnage,0 as ATPACTonnage,convert(decimal(18, 2), Sum(totalWeight) / 1000) as TubeFitTonnage, 0 as AvgHireDays from vw_jobInventory where ToId in ( select RegisterID from tblRegister where  ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(LocationId = '" + ddlarea + "' or 'all' = '" + ddlarea + "') and (Case when ScaffoldOwner = 'C&I' then 'C&I' else 'Maintenance' End = '" + SubOwner + "' or 'all' = '" + SubOwner + "') and (DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) ) and MaterialType = 'Tube & Fit' union all select 0 as StandingScaffold , 0 as StandingTonnage,0 as ATPACTonnage,0 as TubeFitTonnage,	convert(decimal(18), cast(Convert(decimal(18, 2), SUM(isnull(HireDaysToDate, 0))) / Convert(decimal(18, 2), Count('x')) as float)) as AvgHireDays from VW_TblRegister where ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(LocationId = '" + ddlarea + "' or 'all' = '" + ddlarea + "') and (Case when ScaffoldOwner = 'C&I' then 'C&I' else 'Maintenance' End = '" + SubOwner + "' or 'all' = '" + SubOwner + "') and (DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null)) AllRecords Union all Select 'Total Long Term Standing' as GraphName, MAX(StandingScaffold) as StandingScaffold , MAX(StandingTonnage) as StandingTonnage,MAX(ATPACTonnage) as ATPACTonnage,MAX(TubeFitTonnage) as TubeFitTonnage,MAX(AvgHireDays) as AvgHireDays from( select COUNT('x') as StandingScaffold, cast(sum(isnull(Tonnage, 0)) / 1000 as decimal(18, 2)) as StandingTonnage, 0 as ATPACTonnage,0 as TubeFitTonnage,0 as AvgHireDays from TblRegister where ClientId = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(LocationId = '" + ddlarea + "' or 'all' = '" + ddlarea + "') and (Case when ScaffoldOwner = 'C&I' then 'C&I' else 'Maintenance' End = '" + SubOwner + "' or 'all' = '" + SubOwner + "') and (DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) and Isnull(LongTermStructure,0)= 1 union all select 0 as StandingScaffold,0 as StandingTonnage,convert(decimal(18, 2), Sum(totalWeight) / 1000) as ATPACTonnage,0 as TubeFitTonnage, 0 as AvgHireDays from vw_jobInventory where ToId in ( select RegisterID from tblRegister where  ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(LocationId = '" + ddlarea + "' or 'all' = '" + ddlarea + "') and (Case when ScaffoldOwner = 'C&I' then 'C&I' else 'Maintenance' End = 'all' or 'all' = 'all') and (DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null)and Isnull(LongTermStructure,0)= 1) and MaterialType = 'At-Pac' union all  select 0 as StandingScaffold,0 as StandingTonnage,0 as ATPACTonnage,convert(decimal(18, 2), Sum(totalWeight) / 1000) as TubeFitTonnage, 0 as AvgHireDays from vw_jobInventory where ToId in ( select RegisterID from tblRegister where  ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(LocationId = '" + ddlarea + "' or 'all' = '" + ddlarea + "') and (Case when ScaffoldOwner = 'C&I' then 'C&I' else 'Maintenance' End = '" + SubOwner + "' or 'all' = '" + SubOwner + "') and (DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null)and Isnull(LongTermStructure,0)= 1 )	and MaterialType = 'Tube & Fit' union all select 0 as StandingScaffold , 0 as StandingTonnage,0 as ATPACTonnage,0 as TubeFitTonnage, convert(decimal(18), cast(Convert(decimal(18, 2), SUM(isnull(HireDaysToDate, 0))) / Convert(decimal(18, 2), Count('x')) as float)) as AvgHireDays from VW_TblRegister where ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(LocationId = '" + ddlarea + "' or 'all' = '" + ddlarea + "') and  (Case when ScaffoldOwner = 'C&I' then 'C&I' else 'Maintenance' End = '" + SubOwner + "' or 'all' = '" + SubOwner + "') and (DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) and Isnull(LongTermStructure,0)= 1) AllRecords";

            string query = "Select 'Total Standing' as GraphName, MAX(StandingScaffold) as StandingScaffold , MAX(StandingTonnage) as StandingTonnage,MAX(ATPACTonnage) as ATPACTonnage,MAX(TubeFitTonnage) as TubeFitTonnage,Max(OtherTonnage)OtherTonnage,MAX(AvgHireDays) as AvgHireDays from(select COUNT('x') as StandingScaffold, cast(sum(isnull(Tonnage, 0)) / 1000 as decimal(18, 2)) as StandingTonnage,0 as ATPACTonnage,0 as TubeFitTonnage, 0 as OtherTonnage ,0 as AvgHireDays from TblRegister where ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(LocationId = '" + ddlarea + "' or 'all' = '" + ddlarea + "') and (Case when ScaffoldOwner = 'C&I' then 'C&I' else 'Maintenance' End = '" + SubOwner + "' or 'all' = '" + SubOwner + "') and (DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) union all select 0 as StandingScaffold,0 as StandingTonnage,convert(decimal(18, 2), Sum(totalWeight) / 1000) as ATPACTonnage,0 as TubeFitTonnage,0 as OtherTonnage, 0 as AvgHireDays from vw_jobInventory where ToId in (select RegisterID from tblRegister where  ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(LocationId = '" + ddlarea + "' or 'all' = '" + ddlarea + "') and(Case when ScaffoldOwner = 'C&I' then 'C&I' else 'Maintenance' End = '" + SubOwner + "'or 'all' = '" + SubOwner + "') and(DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) ) and MaterialType = 'At-Pac' union all select 0 as StandingScaffold,0 as StandingTonnage,0 as ATPACTonnage,convert(decimal(18, 2), Sum(totalWeight) / 1000) as TubeFitTonnage,0 as OtherTonnage, 0 as AvgHireDays from vw_jobInventory where ToId in (select RegisterID from tblRegister where  ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(LocationId = '" + ddlarea + "' or 'all' = '" + ddlarea + "') and(Case when ScaffoldOwner = 'C&I' then 'C&I' else 'Maintenance' End = '" + SubOwner + "' or 'all' = '" + SubOwner + "') and(DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) ) and MaterialType = 'Tube & Fit' Union All select 0 as StandingScaffold,0 as StandingTonnage,0 as ATPACTonnage,0 as TubeFitTonnage,convert(decimal(18, 2), Sum(totalWeight) / 1000) as OtherTonnage, 0 as AvgHireDays from vw_jobInventory where ToId in (select RegisterID from tblRegister where   ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(LocationId = '" + ddlarea + "' or 'all' = '" + ddlarea + "') and(Case when ScaffoldOwner = 'C&I' then 'C&I' else 'Maintenance' End = '" + SubOwner + "' or 'all' ='" + SubOwner + "') and(DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) ) and MaterialType != 'At-Pac' and MaterialType!= 'Tube & Fit' union all select 0 as StandingScaffold , 0 as StandingTonnage,0 as ATPACTonnage,0 as TubeFitTonnage,0 as OtherTonnage,convert(decimal(18), cast(Convert(decimal(18, 2), SUM(isnull(HireDaysToDate, 0))) / Convert(decimal(18, 2), Count('x')) as float)) as AvgHireDays from VW_TblRegister where ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(LocationId = '" + ddlarea + "' or 'all' = '" + ddlarea + "') and(Case when ScaffoldOwner = 'C&I' then 'C&I' else 'Maintenance' End = '" + SubOwner + "' or 'all' = '" + SubOwner + "') and (DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null)) AllRecords Union all Select 'Total Long Term Standing' as GraphName, MAX(StandingScaffold) as StandingScaffold , MAX(StandingTonnage) as StandingTonnage,MAX(ATPACTonnage) as ATPACTonnage,MAX(TubeFitTonnage) as TubeFitTonnage,Max(OtherTonnage)OtherTonnage,MAX(AvgHireDays) as AvgHireDays from(select COUNT('x') as StandingScaffold,cast(sum(isnull(Tonnage, 0)) / 1000 as decimal(18, 2)) as StandingTonnage, 0 as ATPACTonnage,0 as TubeFitTonnage,0 as OtherTonnage,0 as AvgHireDays from TblRegister where ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(LocationId = '" + ddlarea + "' or 'all' = '" + ddlarea + "') and(Case when ScaffoldOwner = 'C&I' then 'C&I' else 'Maintenance' End = '" + SubOwner + "' or 'all' = '" + SubOwner + "') and(DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) and Isnull(LongTermStructure,0)= 1 union all select 0 as StandingScaffold,0 as StandingTonnage,convert(decimal(18, 2), Sum(totalWeight) / 1000) as ATPACTonnage,0 as TubeFitTonnage,0 as OtherTonnage, 0 as AvgHireDays from vw_jobInventory where ToId in (select RegisterID from tblRegister where  ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(LocationId = '" + ddlarea + "' or 'all' = '" + ddlarea + "') and (Case when ScaffoldOwner = 'C&I' then 'C&I' else 'Maintenance' End = '" + SubOwner + "'or 'all' = '" + SubOwner + "') and (DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) and Isnull(LongTermStructure,0)= 1) and MaterialType = 'At-Pac' union all  select 0 as StandingScaffold,0 as StandingTonnage, 0 as ATPACTonnage,convert(decimal(18, 2), Sum(totalWeight) / 1000) as TubeFitTonnage, 0 as OtherTonnage,0 as AvgHireDays from vw_jobInventory where ToId in (select RegisterID from tblRegister where  ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(LocationId = '" + ddlarea + "' or 'all' = '" + ddlarea + "') and(Case when ScaffoldOwner = 'C&I' then 'C&I' else 'Maintenance' End = '" + SubOwner + "' or 'all' = '" + SubOwner + "') and (DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) and Isnull(LongTermStructure,0)= 1 )        and MaterialType = 'Tube & Fit' Union All select 0 as StandingScaffold,0 as StandingTonnage,0 as ATPACTonnage, 0 as TubeFitTonnage,convert(decimal(18, 2), Sum(totalWeight) / 1000) as OtherTonnage, 0 as AvgHireDays from vw_jobInventory where ToId in (select RegisterID from tblRegister where   ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(LocationId = '" + ddlarea + "'or 'all' = '" + ddlarea + "') and(Case when ScaffoldOwner = 'C&I' then 'C&I' else 'Maintenance' End = '" + SubOwner + "' or 'all' = '" + SubOwner + "') and(DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null)and Isnull(LongTermStructure,0)= 1         ) and MaterialType != 'At-Pac' and MaterialType!= 'Tube & Fit' union all select 0 as StandingScaffold , 0 as StandingTonnage,0 as ATPACTonnage,0 as TubeFitTonnage,0 as OtherTonnage, convert(decimal(18), cast(Convert(decimal(18, 2), SUM(isnull(HireDaysToDate, 0))) / Convert(decimal(18, 2), Count('x')) as float)) as AvgHireDays from VW_TblRegister where ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(LocationId = '" + ddlarea + "' or 'all' = '" + ddlarea + "') and(Case when ScaffoldOwner = 'C&I' then 'C&I' else 'Maintenance' End = '" + SubOwner + "' or 'all' = '" + SubOwner + "') and (DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) and Isnull(LongTermStructure,0)= 1) AllRecords";

            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
                {
                foreach (DataRow item in ds.Tables[0].Rows)
                    {
                    ScaffoldAllDashBoardViewModel pro = new ScaffoldAllDashBoardViewModel();
                    if (!(item["GraphName"] is DBNull) && item["GraphName"].ToString() != "")
                        {
                        pro.GraphName = item["GraphName"].ToString();
                        }
                    if (!(item["StandingScaffold"] is DBNull) && item["StandingScaffold"].ToString() != "")
                        {
                        pro.StandingScaffold = item["StandingScaffold"].ToString();
                        }
                    if (!(item["StandingTonnage"] is DBNull) && item["StandingTonnage"].ToString() != "")
                        {
                        pro.StandingTonnage = item["StandingTonnage"].ToString();
                        }
                    if (!(item["ATPACTonnage"] is DBNull) && item["ATPACTonnage"].ToString() != "")
                        {
                        pro.ATPACTonnage = item["ATPACTonnage"].ToString();
                        }
                    if (!(item["TubeFitTonnage"] is DBNull) && item["TubeFitTonnage"].ToString() != "")
                        {
                        pro.TubeFitTonnage = item["TubeFitTonnage"].ToString();
                        }
                    if (!(item["AvgHireDays"] is DBNull) && item["AvgHireDays"].ToString() != "")
                        {
                        pro.AvgHireDays2 = item["AvgHireDays"].ToString();
                        }
                    if (!(item["OtherTonnage"] is DBNull) && item["OtherTonnage"].ToString() != "")
                    {
                        pro.OtherTonnage = item["OtherTonnage"].ToString();
                    }
                    SitedataDetails.Add(pro);
                    }
                }

               return Json(SitedataDetails, JsonRequestBehavior.AllowGet);
            }

        List<ScaffoldAllDashBoardViewModel> TonnageAllsites = new List<ScaffoldAllDashBoardViewModel> { };
        public ActionResult AllGraphDashboard(string Siteid, string ClientID, string GraphData, string SubOwner, string ddlarea)
        {
            string query = "exec Pro_GetLongTermGraph '" + ClientID + "','" + Siteid + "','" + GraphData + "','" + SubOwner + "','" + ddlarea + "'";


            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
             {
                foreach (DataRow item in ds.Tables[0].Rows)
                 {
                    ScaffoldAllDashBoardViewModel pro = new ScaffoldAllDashBoardViewModel();
                    if (!(item["Series"] is DBNull) && item["Series"].ToString() != "")
                    {
                        pro.Series = item["Series"].ToString();
                    }
                    if (!(item["SeriesValue"] is DBNull) && item["SeriesValue"].ToString() != "")
                    {
                        pro.SeriesValue = Convert.ToDecimal(item["SeriesValue"]);
                    }
                    if (!(item["Pct"] is DBNull) && item["Pct"].ToString() != "")
                    {
                        pro.PctAllSite = Convert.ToDecimal(item["Pct"]);
                    }
                    TonnageAllsites.Add(pro);
                 }
                }
            return Json(TonnageAllsites, JsonRequestBehavior.AllowGet);
            }

        List<ScaffoldAllDashBoardViewModel> SiteAnaysisDashboardChart1 = new List<ScaffoldAllDashBoardViewModel> { };

        public ActionResult ScaffoldDesignType(string id, string site, string SubOwner, string ddlarea)
        {

            string qry = "select case when tblRegister.DesignType='' or tblRegister.DesignType is null then 'Design Type not assigned to job' else tblRegister.DesignType end as DesignType , count(RegisterId) as RegisterIdCount,tblDesign.DifficultyRating as difficultyRating from tblRegister join tblDesign on tblRegister.DesignType = tblDesign.DesignType and tblDesign.ClientID = tblRegister.ClientID  where (DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle is null or DateOfDismantle = '01/01/1900') and tblDesign.ClientID = '" + id + "' and (SiteLocId = '"+ site + "' or 'all' = '" + site + "') and Isnull(LongTermStructure,0)= 1 and (Case when ScaffoldOwner = 'C&I' then 'C&I' else 'Maintenance' End = '" + SubOwner.Replace("'", "''") + "' or 'all' = '" + SubOwner.Replace("'", "''") + "') and(LocationId = '" + ddlarea + "' or 'all' = '" + ddlarea + "') group by tblRegister.DesignType,tblDesign.DifficultyRating order by RegisterIdCount desc";

            DataSet ds = null;
            ds = databaseCon.GetDataSet(qry);
            if (ds.Tables.Count > 0)
                {
                foreach (DataRow item in ds.Tables[0].Rows)
                    {
                    ScaffoldAllDashBoardViewModel pro = new ScaffoldAllDashBoardViewModel();
                    if (!(item["DesignType"] is DBNull) && item["DesignType"].ToString() != "")
                    {
                        pro.designtype = item["DesignType"].ToString();
                    }
                    if (!(item["RegisterIdCount"] is DBNull) && item["RegisterIdCount"].ToString() != "")
                    {
                        pro.rquantity = Convert.ToInt32(item["RegisterIdCount"].ToString());
                    }
                    if (!(item["difficultyRating"] is DBNull) && item["difficultyRating"].ToString() != "")
                    {
                        pro.Chart1difficultyrating = item["difficultyRating"].ToString();
                    }

                    SiteAnaysisDashboardChart1.Add(pro);
                    }
                }
                   return Json(SiteAnaysisDashboardChart1, JsonRequestBehavior.AllowGet);
            }

        List<ScaffoldAllDashBoardViewModel> DesignTypeBarClick = new List<ScaffoldAllDashBoardViewModel> { };
        public ActionResult LongTermDesignClick(string DesignType, string ID, string Site, string SubOwner, string ddlarea)
        {
            var JobDesignType = "";
            if (DesignType == "Design Type not assigned to job")
            {
                JobDesignType = "";
            }
            else
            {
                JobDesignType = DesignType;
            }

            string query = "select Right ('000000' + convert (varchar, [ScaffRef]),6) as ScaffRef,Description,Location,BuildingNo,AssetNo,FloorLevel,DesignType,MaterialType, case when CycloneAction = 'Full Strip' then ManHrsDismantle When CycloneAction <> 'Full Strip' and UseEnteredValue = 1 then TotalLabourDismantleEdit else TotalLabourDismantle End TotalLabourDismantleEdit,PriorityRiskRating from VW_TblRegister where clientid='" + ID + "' and DateOfDismantle is NULL and(DateTagged <> '01/01/1900' and DateTagged is not null) and('" + Site + "' = 'all'  or SiteLocId = '" + Site + "') and ('" + ddlarea + "' = 'all'  or LocationId = '" + ddlarea + "')  and ('" + SubOwner.Replace("'", "''") + "' = 'all'  or Case when ScaffoldOwner='C&I' then 'C&I' else 'Maintenance' End = '" + SubOwner.Replace("'", "''") + "') and DesignType= '"+ JobDesignType.Replace("'", "''") + "' and Isnull(LongTermStructure,0)= 1 Order by Location,BuildingNo";

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
                    if (!(item["TotalLabourDismantleEdit"] is DBNull) && item["TotalLabourDismantleEdit"].ToString() != "")
                    {
                        pro.PreparationHrs = item["TotalLabourDismantleEdit"].ToString();
                    }
                    else
                    {
                        pro.PreparationHrs = "";
                    }

                    if (!(item["PriorityRiskRating"] is DBNull) && item["PriorityRiskRating"].ToString() != "")
                    {
                        pro.RiskRating = item["PriorityRiskRating"].ToString();
                    }
                    else
                    {
                        pro.RiskRating = "";
                    }

                    DesignTypeBarClick.Add(pro);
                }
            }

            return Json(DesignTypeBarClick, JsonRequestBehavior.AllowGet);
        }

    }
}