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
    public class DashboardHereController : Controller
    {
        ScaffoldEntities dbcontext = new ScaffoldEntities();
        // GET: DashboardHere
        public ActionResult DashboardHere()
        {
            int clientId;

            if (!string.IsNullOrEmpty(HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["id"]))) && Request.QueryString["id"] != null)
            {
                clientId = Convert.ToInt32(HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["id"])));
            }
            else
            {
                string query = "select distinct(Name+' '+'('+status+')') as name=,ClientId from tblclient where ClientId!='1' Order by name";
                DataTable dt = databaseCon.GetData(query);
                clientId = Convert.ToInt32(dt.Rows[0]["ClientId"]);

            }

           // hdnClientID.Value = Convert.ToString(clientId);

            ViewBag.ClientID= Convert.ToString(clientId);

            string ClientQuery = "select ClientImage from tblClient where ClientId = '" + clientId + "'";
            DataTable clientdt = databaseCon.GetData(ClientQuery);

            if (clientdt.Rows.Count > 0)
            {
                if (clientdt.Rows[0]["ClientImage"].ToString() != "")
                {
                    var uri = new Uri(databaseCon.clientimageLink);
                    string imgname = clientdt.Rows[0]["ClientImage"].ToString();
                    // var container = new CloudBlobContainer(uri);
                    // var blob = container.GetBlockBlobReference(clientdt.Rows[0]["ClientImage"].ToString());
                    //string a = databaseCon.siteimagelink + "Item1.png";
                    // imga.Attributes.Add("src", databaseCon.siteimagelink + "Item1.png");
                    // var exists = blob.Exists();
                    if (clientdt.Rows[0]["ClientImage"].ToString() == "")
                    {
                       // imgLogo.Attributes.Add("src", "/images/logo.png");
                       // ViewBag.ClientImage = "/images/logo.png";
                        ViewBag.ClientImage = databaseCon.logoimage;
                    }
                    else
                    {
                        //  string a = databaseCon.clientimageLink + clientdt.Rows[0]["ClientImage"].ToString();
                        //imgLogo.Attributes.Add("src", databaseCon.clientimageLink + clientdt.Rows[0]["ClientImage"].ToString());
                       // imgLogo.Attributes.Add("src", (("/images/clientlogo/") + clientdt.Rows[0]["ClientImage"].ToString()));
                        ViewBag.ClientImage ="/images/clientlogo/" + clientdt.Rows[0]["ClientImage"].ToString();
                    }
                }
                else
                {
                    // imgLogo.Attributes.Add("src", "images/logo.png");
                    // ViewBag.ClientImage = "/images/logo.png";
                    ViewBag.ClientImage = databaseCon.logoimage;
                }
            }
           return View();
        }


        /// Site Analytics Dashboard Site dropdown query Start

        public ActionResult ddlsitedropdownbindquery(string id, string ClientID, string UID)
        {
            var SiteOwnerName = "";
            string query1 = "select SiteOwnerName from tblUser where ClientId='" + ClientID + "' and UserId='" + databaseCon.Decrypt(UID) + "'";
            DataTable dt1 = databaseCon.GetData(query1);
            if (dt1.Rows.Count > 0)
                {
                SiteOwnerName = dt1.Rows[0]["SiteOwnerName"].ToString();
                }
            if (SiteOwnerName != "")
            {
                // Escape single quote by replacing ' with ''
                SiteOwnerName = SiteOwnerName.Replace("'", "''");
                var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select SiteLocID,SiteLocation from tblSiteLocation where ClientId='" + ClientID + "' and OwnerName='" + SiteOwnerName + "' order by SiteLocation").ToList();
                return Json(GetData, JsonRequestBehavior.AllowGet);
                }
            else
                {
                var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select SiteLocID,SiteLocation from tblSiteLocation where ClientId='" + ClientID + "' order by SiteLocation").ToList();
                return Json(GetData, JsonRequestBehavior.AllowGet);
                }

            }
        /// Site Analytics Dashboard Site dropdown query End

        /// Site Analytics Dashboard Scaffolds by Op Centre dropdown Query Start
         public ActionResult ddllocationdropdownbindquery(string Siteid, string ClientID)
        {
            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select area,areaid  from tblarea where AreaStatus='Active' and  Area <>'' and Area is not null and ClientID='" + ClientID + "' and('" + Siteid + "' = 'all' or SiteLocId = '" + Siteid + "') order by area").ToList();
            
             return Json(GetData, JsonRequestBehavior.AllowGet);
        }
        /// Site Analytics Dashboard Scaffolds by Op Centre dropdown Query End

        /// Site Analytics Dashboard Tonnage by Building dropdown query Start
    
        public ActionResult ddlbuildingdropdownbindquery(string Siteid, string locid, string ClientID)
        {
            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select buildingid,buildingno  from tblbuilding join tblArea on tblbuilding.AreaId=tblArea.AreaId where BuildingStatus='Active' and tblbuilding.ClientID='" + ClientID + "' and ('" + locid + "'='all' or tblbuilding.Areaid='" + locid + "') and('" + Siteid + "' = 'all' or SiteLocId = '" + Siteid + "') order by buildingno").ToList();

            //Commented on (02-02-2024) we added new client Acrow Snowy Hydro for which when we runned this query it returned 718 Records because of which we were getting object:object error. so to overcome the issue we added MaxJsonLength which will handle maximum response.
            //return Json(GetData, JsonRequestBehavior.AllowGet);

            //Added on (02-02-2024)
            var jsonResult = Json(GetData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }
        /// Site Analytics Dashboard Tonnage by Building dropdown query End

        /// Site Analytics Dashboard Scaffold By Site Textboxes query Start
        List<ScaffoldAllDashBoardViewModel> SitedataDetails = new List<ScaffoldAllDashBoardViewModel> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod(EnableSession = true)]
        public ActionResult SiteDetails(string Siteid, string ClientID)
        {
            //change on nov 15
            //string query = "Select MAX(Erectedscaffold)as Erectedscaffold ,Max(StandingScaffold) as StandingScaffold,MAX(ErrectedTonnage) as ErrectedTonnage, MAX(StandingTonnage) as StandingTonnage,MAX(AverageTonnage) as AverageTonnage,MAX(AvgHireDays)as AvgHireDays    from (select COUNT(*) as Erectedscaffold, 0 as StandingScaffold, 0 as ErrectedTonnage, 0 as StandingTonnage, 0 as AverageTonnage, 0 as AvgHireDays   from VW_TblRegister where  ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(DateTagged <> '01/01/1900' and DateTagged is not null) union all select 0 as Erectedscaffold, COUNT(*) as StandingScaffold, 0 as ErrectedTonnage, 0 as StandingTonnage, 0 as AverageTonnage, 0 as AvgHireDays from VW_TblRegister where  ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "')  and(DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) union all select 0 as Erectedscaffold, 0 as StandingScaffold, cast(sum(isnull(Tonnage, 0)) / 1000 as decimal(18, 2)) as ErrectedTonnage ,0 as StandingTonnage, 0 as AverageTonnage,0 as AvgHireDays from VW_TblRegister where  ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(DateTagged <> '01/01/1900' and DateTagged is not null) union all select 0 as Erectedscaffold,0 as StandingScaffold,0 as ErrectedTonnage,cast(sum(isnull(Tonnage, 0)) / 1000 as decimal(18, 2)) as StandingTonnage, 0 as AverageTonnage,0 as AvgHireDays from VW_TblRegister where  ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "')  and(DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) union all select 0 as Erectedscaffold,0 as StandingScaffold,0 as ErrectedTonnage,0 as StandingTonnage,case when sum(isnull(Tonnage, 0)) = 0 then 0 else cast(sum(isnull(Tonnage, 0)) / (1000 * COUNT(*)) as decimal(18, 2)) End as AverageTonnage ,0 as AvgHireDays from VW_TblRegister where  ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(DateTagged <> '01/01/1900' and DateTagged is not null)  union all select 0 as Erectedscaffold,0 as StandingScaffold,0 as ErrectedTonnage,0 as StandingTonnage, 0 as AverageTonnage,convert(decimal(18), cast(Convert(decimal(18, 2), SUM(HireDays)) / Convert(decimal(18, 2), AVG(Counts)) as float)) as AvgHireDays from(select SUM(isnull(HireDaysToDate, 0)) as HireDays, COUNT(*) as Counts from VW_TblRegister where  ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "')  and(DateTagged <> '01/01/1900' or DateTagged is not null)   union all select Sum(isnull(TotalHireDays, 0)) as HireDays, COUNT(*) as Counts from VW_TblRegister where  ClientId = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(DateTagged <> '01/01/1900' and DateTagged is not null))AverageDays ) forSiteQuery ";

            //// Changed Query on 31-OCT-2020
            //string query = "Select MAX(Erectedscaffold)as Erectedscaffold ,Max(StandingScaffold) as StandingScaffold,MAX(ErrectedTonnage) as ErrectedTonnage, MAX(StandingTonnage) as StandingTonnage,MAX(AverageTonnage) as AverageTonnage,MAX(AvgHireDays)as AvgHireDays    from ( select COUNT('x') as Erectedscaffold, 0 as StandingScaffold, 0 as ErrectedTonnage, 0 as StandingTonnage, 0 as AverageTonnage, 0 as AvgHireDays  from TblRegister where  ClientId = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(DateTagged <> '01/01/1900' and DateTagged is not null)  union all  select 0 as Erectedscaffold, COUNT('x') as StandingScaffold, 0 as ErrectedTonnage, 0 as StandingTonnage, 0 as AverageTonnage, 0 as AvgHireDays from TblRegister where  ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "')  and(DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) union all select 0 as Erectedscaffold, 0 as StandingScaffold, cast(sum(isnull(Tonnage, 0)) / 1000 as decimal(18, 2)) as ErrectedTonnage ,0 as StandingTonnage, 0 as AverageTonnage,0 as AvgHireDays from TblRegister where  ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(DateTagged <> '01/01/1900' and DateTagged is not null) union all select 0 as Erectedscaffold,0 as StandingScaffold,0 as ErrectedTonnage,cast(sum(isnull(Tonnage, 0)) / 1000 as decimal(18, 2)) as StandingTonnage, 0 as AverageTonnage,0 as AvgHireDays from TblRegister where  ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "')  and(DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) union all select 0 as Erectedscaffold,0 as StandingScaffold,0 as ErrectedTonnage,0 as StandingTonnage,case when sum(isnull(Tonnage, 0)) = 0 then 0 else cast(sum(isnull(Tonnage, 0)) / (1000 * COUNT('x')) as decimal(18, 2)) End as AverageTonnage ,0 as AvgHireDays from TblRegister where ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(DateTagged <> '01/01/1900' and DateTagged is not null)  union all select 0 as Erectedscaffold,0 as StandingScaffold,0 as ErrectedTonnage,0 as StandingTonnage, 0 as AverageTonnage,convert(decimal(18), cast(Convert(decimal(18, 2), SUM(HireDays)) / Convert(decimal(18, 2), AVG(Counts)) as float)) as AvgHireDays from(        select SUM(isnull(HireDaysToDate, 0)) as HireDays, COUNT('x') as Counts from VW_TblRegister where  ClientId = '" + ClientID + "' and        ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "')  and(DateTagged <> '01/01/1900' and DateTagged is not null)         and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null)            union all         select Sum(CASE WHEN DateTagged !='1900-01-01' AND DateOfDismantle !='1900-01-01' THEN DateDiff(dd, DateTagged, DateOfDismantle) +1 ELSE NULL END)  as HireDays  , Count('x') as Counts from TblRegister where  ClientId = '" + ClientID + "' and ('" + Siteid + "' = 'all'          or SiteLocId = '" + Siteid + "') and(DateOfDismantle <> '01/01/1900' and DateOfDismantle is not null) )AverageDays ) forSiteQuery ";

            // Changed Query on 04-MAr-2022
            //string query = "Select MAX(Erectedscaffold)as Erectedscaffold ,Max(StandingScaffold) as StandingScaffold,MAX(ErrectedTonnage) as ErrectedTonnage, MAX(StandingTonnage) as StandingTonnage,MAX(AverageTonnage) as AverageTonnage,MAX(AvgHireDays)as AvgHireDays    from ( select COUNT('x') as Erectedscaffold, 0 as StandingScaffold, 0 as ErrectedTonnage, 0 as StandingTonnage, 0 as AverageTonnage, 0 as AvgHireDays  from TblRegister where  ClientId = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(DateTagged <> '01/01/1900' and DateTagged is not null)  union all  select 0 as Erectedscaffold, COUNT('x') as StandingScaffold, 0 as ErrectedTonnage, 0 as StandingTonnage, 0 as AverageTonnage, 0 as AvgHireDays from TblRegister where  ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "')  and(DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) union all select 0 as Erectedscaffold, 0 as StandingScaffold, cast(sum(isnull(Tonnage, 0)) / 1000 as decimal(18, 2)) as ErrectedTonnage ,0 as StandingTonnage, 0 as AverageTonnage,0 as AvgHireDays from TblRegister where  ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(DateTagged <> '01/01/1900' and DateTagged is not null) union all select 0 as Erectedscaffold,0 as StandingScaffold,0 as ErrectedTonnage,cast(sum(isnull(Tonnage, 0)) / 1000 as decimal(18, 2)) as StandingTonnage, 0 as AverageTonnage,0 as AvgHireDays from TblRegister where  ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "')  and(DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) union all select 0 as Erectedscaffold,0 as StandingScaffold,0 as ErrectedTonnage,0 as StandingTonnage,case when sum(isnull(Tonnage, 0)) = 0 then 0 else cast(sum(isnull(Tonnage, 0)) / (1000 * COUNT('x')) as decimal(18, 2)) End as AverageTonnage ,0 as AvgHireDays from TblRegister where ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(DateTagged <> '01/01/1900' and DateTagged is not null)  union all select 0 as Erectedscaffold,0 as StandingScaffold,0 as ErrectedTonnage,0 as StandingTonnage, 0 as AverageTonnage,convert(decimal(18), cast(Convert(decimal(18, 2), SUM(HireDays)) / Convert(decimal(18, 2), AVG(Counts)) as float)) as AvgHireDays from(        select SUM(isnull(HireDaysToDate, 0)) as HireDays, COUNT('x') as Counts from VW_TblRegister where  ClientId = '" + ClientID + "' and        ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "')  and(DateTagged <> '01/01/1900' and DateTagged is not null)            union all         select Sum(CASE WHEN DateTagged !='1900-01-01' AND DateOfDismantle !='1900-01-01' THEN DateDiff(dd, DateTagged, DateOfDismantle) +1 ELSE NULL END)  as HireDays  , Count('x') as Counts from TblRegister where  ClientId = '" + ClientID + "' and ('" + Siteid + "' = 'all'          or SiteLocId = '" + Siteid + "') and(DateTagged <> '01/01/1900' and DateTagged is not null) )AverageDays ) forSiteQuery ";
            //Chaneged on 04-MarshalByRefObject-2022

            // Commented on 31-10-2023 Dashboard here is working slow for unispan after reviewing query we came to know that it was using vw_tblregister because of which it was getting slow so we are now using tblregister instead of vw_tblregister.
            //string query = "Select MAX(Erectedscaffold)as Erectedscaffold ,Max(StandingScaffold) as StandingScaffold,MAX(ErrectedTonnage) as ErrectedTonnage, MAX(StandingTonnage) as StandingTonnage,MAX(AverageTonnage) as AverageTonnage,MAX(AvgHireDays) as AvgHireDays,MAX(AverageStandingTonnage) as AverageStandingTonnage,MAX(AvgStandingHireDays) as AvgStandingHireDays from(select COUNT('x') as Erectedscaffold, 0 as StandingScaffold, 0 as ErrectedTonnage, 0 as StandingTonnage, 0 as AverageTonnage, 0 as AvgHireDays, 0 as AverageStandingTonnage, 0 as AvgStandingHireDays from TblRegister where ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and (DateTagged <> '01/01/1900' and DateTagged is not null) union all select 0 as Erectedscaffold, COUNT('x') as StandingScaffold, 0 as ErrectedTonnage, 0 as StandingTonnage, 0 as AverageTonnage, 0 as AvgHireDays, 0 as AverageStandingTonnage, 0 as AvgStandingHireDays from TblRegister where ClientId = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "')  and(DateTagged <> '01/01/1900' and DateTagged is not null) and (DateOfDismantle = '01/01/1900' or DateOfDismantle is null) union all select 0 as Erectedscaffold, 0 as StandingScaffold, cast(sum(isnull(Tonnage, 0)) / 1000 as decimal(18, 2)) as ErrectedTonnage, 0 as StandingTonnage, 0 as AverageTonnage, 0 as AvgHireDays, 0 as AverageStandingTonnage, 0 as AvgStandingHireDays from TblRegister where ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(DateTagged <> '01/01/1900' and DateTagged is not null) union all select 0 as Erectedscaffold, 0 as StandingScaffold, 0 as ErrectedTonnage, cast(sum(isnull(Tonnage, 0)) / 1000 as decimal(18, 2)) as StandingTonnage, 0 as AverageTonnage, 0 as AvgHireDays, 0 as AverageStandingTonnage, 0 as AvgStandingHireDays from TblRegister where ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "')  and(DateTagged <> '01/01/1900' and DateTagged is not null) and (DateOfDismantle = '01/01/1900' or DateOfDismantle is null)  union all select 0 as Erectedscaffold, 0 as StandingScaffold , 0 as ErrectedTonnage, 0 as StandingTonnage,case when sum(isnull(Tonnage, 0)) = 0 then 0 else cast(sum(isnull(Tonnage, 0)) / (1000 * COUNT('x')) as decimal(18, 2)) End as AverageTonnage , 0 as AvgHireDays,0 as AverageStandingTonnage, 0 as AvgStandingHireDays from TblRegister where ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and (DateTagged <> '01/01/1900' and DateTagged is not null) union all select 0 as Erectedscaffold,0 as StandingScaffold ,0 as ErrectedTonnage,0 as StandingTonnage, 0 as AverageTonnage,convert(decimal(18), cast(Convert(decimal(18, 2), SUM(HireDays)) / Convert(decimal(18, 2), AVG(Counts)) as float)) as AvgHireDays,0 as AverageStandingTonnage, 0 as AvgStandingHireDays from(select SUM(isnull(HireDaysToDate, 0)) as HireDays, COUNT('x') as Counts from VW_TblRegister where ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "')  and(DateTagged <> '01/01/1900' and DateTagged is not null) union all  select Sum(CASE WHEN DateTagged != '1900-01-01' AND DateOfDismantle != '1900-01-01' THEN DateDiff(dd, DateTagged, DateOfDismantle) + 1 ELSE NULL END) as HireDays , Count('x') as Counts from TblRegister where ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and (DateTagged <> '01/01/1900' and DateTagged is not null))AverageDays  union all select 0 as Erectedscaffold,0 as StandingScaffold ,0 as ErrectedTonnage,0 as StandingTonnage,0 as AverageTonnage , 0 as AvgHireDays,case when sum(isnull(Tonnage, 0)) = 0 then 0 else cast(sum(isnull(Tonnage, 0)) / (1000 * COUNT('x')) as decimal(18, 2)) End as AverageStandingTonnage, 0 as AvgStandingHireDays from TblRegister where ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(DateTagged <> '01/01/1900' and DateTagged is not null) and (DateOfDismantle = '01/01/1900' or DateOfDismantle is null)  union all select 0 as Erectedscaffold,0 as StandingScaffold ,0 as ErrectedTonnage,0 as StandingTonnage, 0 as AverageTonnage,0 as AvgHireDays,0 as AverageStandingTonnage, convert(decimal(18), cast(Convert(decimal(18, 2), SUM(HireDays)) / Convert(decimal(18, 2), AVG(Counts)) as float)) as AvgStandingHireDays from(select SUM(isnull(HireDaysToDate, 0)) as HireDays, COUNT('x') as Counts from VW_TblRegister where ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(DateTagged <> '01/01/1900' and DateTagged is not null) and (DateOfDismantle = '01/01/1900' or DateOfDismantle is null)  union all  select Sum(CASE WHEN DateTagged != '1900-01-01' AND DateOfDismantle != '1900-01-01' THEN DateDiff(dd, DateTagged, DateOfDismantle) + 1 ELSE NULL END) as HireDays , Count('x') as Counts from TblRegister where ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(DateTagged <> '01/01/1900' and DateTagged is not null) and (DateOfDismantle = '01/01/1900' or DateOfDismantle is null) )AverageStandingDays) forSiteQuery ";

            // Added on 31-10-2023 Dashboard here is working slow for unispan after reviewing query we came to know that it was using vw_tblregister because of which it was getting slow so we are now using tblregister instead of vw_tblregister.
            string query = "Select MAX(Erectedscaffold)as Erectedscaffold ,Max(StandingScaffold) as StandingScaffold,MAX(ErrectedTonnage) as ErrectedTonnage, MAX(StandingTonnage) as StandingTonnage,MAX(AverageTonnage) as AverageTonnage,MAX(AvgHireDays) as AvgHireDays,MAX(AverageStandingTonnage) as AverageStandingTonnage,MAX(AvgStandingHireDays) as AvgStandingHireDays from(select COUNT('x') as Erectedscaffold, 0 as StandingScaffold, 0 as ErrectedTonnage, 0 as StandingTonnage, 0 as AverageTonnage, 0 as AvgHireDays, 0 as AverageStandingTonnage, 0 as AvgStandingHireDays from TblRegister where ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and (DateTagged <> '01/01/1900' and DateTagged is not null) union all select 0 as Erectedscaffold, COUNT('x') as StandingScaffold, 0 as ErrectedTonnage, 0 as StandingTonnage, 0 as AverageTonnage, 0 as AvgHireDays, 0 as AverageStandingTonnage, 0 as AvgStandingHireDays from TblRegister where ClientId = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "')  and(DateTagged <> '01/01/1900' and DateTagged is not null) and (DateOfDismantle = '01/01/1900' or DateOfDismantle is null) union all select 0 as Erectedscaffold, 0 as StandingScaffold, cast(sum(isnull(Tonnage, 0)) / 1000 as decimal(18, 2)) as ErrectedTonnage, 0 as StandingTonnage, 0 as AverageTonnage, 0 as AvgHireDays, 0 as AverageStandingTonnage, 0 as AvgStandingHireDays from TblRegister where ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(DateTagged <> '01/01/1900' and DateTagged is not null) union all select 0 as Erectedscaffold, 0 as StandingScaffold, 0 as ErrectedTonnage, cast(sum(isnull(Tonnage, 0)) / 1000 as decimal(18, 2)) as StandingTonnage, 0 as AverageTonnage, 0 as AvgHireDays, 0 as AverageStandingTonnage, 0 as AvgStandingHireDays from TblRegister where ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "')  and(DateTagged <> '01/01/1900' and DateTagged is not null) and (DateOfDismantle = '01/01/1900' or DateOfDismantle is null)  union all select 0 as Erectedscaffold, 0 as StandingScaffold , 0 as ErrectedTonnage, 0 as StandingTonnage,case when sum(isnull(Tonnage, 0)) = 0 then 0 else cast(sum(isnull(Tonnage, 0)) / (1000 * COUNT('x')) as decimal(18, 2)) End as AverageTonnage , 0 as AvgHireDays,0 as AverageStandingTonnage, 0 as AvgStandingHireDays from TblRegister where ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and (DateTagged <> '01/01/1900' and DateTagged is not null) union all select 0 as Erectedscaffold,0 as StandingScaffold ,0 as ErrectedTonnage,0 as StandingTonnage, 0 as AverageTonnage,convert(decimal(18), cast(Convert(decimal(18, 2), SUM(HireDays)) / Convert(decimal(18, 2), AVG(Counts)) as float)) as AvgHireDays,0 as AverageStandingTonnage, 0 as AvgStandingHireDays from(select SUM(isnull(CASE WHEN DateTagged !='1900-01-01' AND DateOfDismantle ='1900-01-01' THEN DateDiff(dd, DateTagged, CONVERT(date, Getdate())) ELSE NULL END, 0)) as HireDays, COUNT('x') as Counts from TblRegister where ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "')  and(DateTagged <> '01/01/1900' and DateTagged is not null) union all  select Sum(CASE WHEN DateTagged != '1900-01-01' AND DateOfDismantle != '1900-01-01' THEN DateDiff(dd, DateTagged, DateOfDismantle) + 1 ELSE NULL END) as HireDays , Count('x') as Counts from TblRegister where ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and (DateTagged <> '01/01/1900' and DateTagged is not null))AverageDays  union all select 0 as Erectedscaffold,0 as StandingScaffold ,0 as ErrectedTonnage,0 as StandingTonnage,0 as AverageTonnage , 0 as AvgHireDays,case when sum(isnull(Tonnage, 0)) = 0 then 0 else cast(sum(isnull(Tonnage, 0)) / (1000 * COUNT('x')) as decimal(18, 2)) End as AverageStandingTonnage, 0 as AvgStandingHireDays from TblRegister where ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(DateTagged <> '01/01/1900' and DateTagged is not null) and (DateOfDismantle = '01/01/1900' or DateOfDismantle is null)  union all select 0 as Erectedscaffold,0 as StandingScaffold ,0 as ErrectedTonnage,0 as StandingTonnage, 0 as AverageTonnage,0 as AvgHireDays,0 as AverageStandingTonnage, convert(decimal(18), cast(Convert(decimal(18, 2), SUM(HireDays)) / Convert(decimal(18, 2), AVG(Counts)) as float)) as AvgStandingHireDays from(select SUM(isnull(CASE WHEN DateTagged !='1900-01-01' AND DateOfDismantle ='1900-01-01' THEN DateDiff(dd, DateTagged, CONVERT(date, Getdate())) ELSE NULL END, 0)) as HireDays, COUNT('x') as Counts from TblRegister where ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(DateTagged <> '01/01/1900' and DateTagged is not null) and (DateOfDismantle = '01/01/1900' or DateOfDismantle is null)  union all  select Sum(CASE WHEN DateTagged != '1900-01-01' AND DateOfDismantle != '1900-01-01' THEN DateDiff(dd, DateTagged, DateOfDismantle) + 1 ELSE NULL END) as HireDays , Count('x') as Counts from TblRegister where ClientId = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and(DateTagged <> '01/01/1900' and DateTagged is not null) and (DateOfDismantle = '01/01/1900' or DateOfDismantle is null) )AverageStandingDays) forSiteQuery ";
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
                    if (!(item["AverageStandingTonnage"] is DBNull) && item["AverageStandingTonnage"].ToString() != "")
                    {
                        pro.AverageStandingTonnage1 = item["AverageStandingTonnage"].ToString();
                    }
                    if (!(item["AvgStandingHireDays"] is DBNull) && item["AvgStandingHireDays"].ToString() != "")
                    {
                        pro.AvgStandingHireDays1 = item["AvgStandingHireDays"].ToString();
                    }
                    SitedataDetails.Add(pro);
                }
            }
            // return SitedataDetails;
            //Commented on (02-02-2024) we added new client Acrow Snowy Hydro for which when we runned this query it returned 718 Records because of which we were getting object:object error. so to overcome the issue we added MaxJsonLength which will handle maximum response.
            //return Json(SitedataDetails,JsonRequestBehavior.AllowGet);

            //Added on (02-02-2024)
            var jsonResult = Json(SitedataDetails, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        /// Site Analytics Dashboard Scaffold By Site Textboxes query End
         

        /// Site Analytics Dashboard Standing Tonnage Breakdown (All Sites) Chart 1 Start
        List<ScaffoldAllDashBoardViewModel> TonnageAllsites = new List<ScaffoldAllDashBoardViewModel> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod(EnableSession = true)]
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
            //Commented on (02-02-2024) we added new client Acrow Snowy Hydro for which when we runned this query it returned 718 Records because of which we were getting object:object error. so to overcome the issue we added MaxJsonLength which will handle maximum response.
            //return Json(TonnageAllsites,JsonRequestBehavior.AllowGet);

            //Added on (02-02-2024)
            var jsonResult = Json(TonnageAllsites, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        /// Site Analytics Dashboard Standing Tonnage Breakdown (All Sites) Chart 1 Start
        /// 
        /// Site Analytics Dashboard LockedIn Click Chart 2 Start
        List<ScaffoldAllDashBoardViewModel> LockedInDashboardClick = new List<ScaffoldAllDashBoardViewModel> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod(EnableSession = true)]
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

            //Commented on (02-02-2024) we added new client Acrow Snowy Hydro for which when we runned this query it returned 718 Records because of which we were getting object:object error. so to overcome the issue we added MaxJsonLength which will handle maximum response.
            //return Json(LockedInDashboardClick, JsonRequestBehavior.AllowGet);

            //Added on (02-02-2024)
            var jsonResult = Json(LockedInDashboardClick, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        /// Site Analytics Dashboard LockedIn Click Chart 2 End

        /// Site Analytics Dashboard Scaffolds Locked In % Chart 2 Start

        List<ScaffoldAllDashBoardViewModel> LocationBySites = new List<ScaffoldAllDashBoardViewModel> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod(EnableSession = true)]
        public ActionResult GetBySitesDashboard(string Siteid, string ClientID)
        {
            // 09 nov Total Scaffold Built to Date Scaffold     
            //string query = "Select Max(TotalRegister) as TotalRegister, Max(LockedInRegister) as LockedInRegister, Case when Max(TotalRegister) = 0 Or Max(LockedInRegister) = 0 then 0 else  Round((Convert(float, Max(LockedInRegister)) * 100.00) / Convert(float, Max(TotalRegister)), 0) end as LockedPct From(select COUNT(*) as TotalRegister, '' as LockedInRegister from tblregister where ClientID = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "')  and(DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) union all select '' as TotalRegister, COUNT(*) as LockedInRegister from tblregister where ClientID = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "')  and(DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) and LockedIn = 1) LockedQuery";

            string query = "Select Max(TotalRegister) as TotalRegister, Max(LockedInRegister) as LockedInRegister, Case when Max(TotalRegister) = 0 Or Max(LockedInRegister) = 0 then 0 else  Round((Convert(float, Max(LockedInRegister)) * 100.00) / Convert(float, Max(TotalRegister)), 0) end as LockedPct From(select COUNT(*) as TotalRegister, '' as LockedInRegister from tblregister where ClientID = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "')  and(DateTagged <> '01/01/1900' and DateTagged is not null)  union all select '' as TotalRegister, COUNT(*) as LockedInRegister from tblregister where ClientID = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "')  and(DateTagged <> '01/01/1900' and DateTagged is not null)  and LockedIn = 1) LockedQuery";

            DataSet ds = null;
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
            // return LocationBySites;
            //Commented on(02 - 02 - 2024) we added new client Acrow Snowy Hydro for which when we runned this query it returned 718 Records because of which we were getting object:object error.so to overcome the issue we added MaxJsonLength which will handle maximum response.
             //return Json(LocationBySites, JsonRequestBehavior.AllowGet);

            //Added on (02-02-2024)
             var jsonResult = Json(LocationBySites, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        /// Site Analytics Dashboard Scaffolds Locked In % Chart 2 End
        /// 
        /// Site Analytics Dashboard Scaffolds by Op Centre Textboxes Query Start
        List<ScaffoldAllDashBoardViewModel> LocationdataDetails = new List<ScaffoldAllDashBoardViewModel> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod(EnableSession = true)]
        public ActionResult LocationDetails(string Siteid, string locid, string ClientID)
        {
            //change on 15 Nov
            //string query = "Select MAX(Erectedscaffold)as Erectedscaffold ,Max(StandingScaffold) as StandingScaffold,MAX(ErrectedTonnage) as ErrectedTonnage, MAX(StandingTonnage) as StandingTonnage,MAX(AverageTonnage) as AverageTonnage,MAX(AvgHireDays)as AvgHireDays    from (select COUNT(*) as Erectedscaffold, 0 as StandingScaffold, 0 as ErrectedTonnage, 0 as StandingTonnage, 0 as AverageTonnage, 0 as AvgHireDays   from VW_TblRegister where  ClientId = '" + ClientID + "'and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "'='all'  or LocationId='" + locid + "' ) and (DateTagged <> '01/01/1900' and DateTagged is not null) union all select 0 as Erectedscaffold, COUNT(*) as StandingScaffold, 0 as ErrectedTonnage, 0 as StandingTonnage, 0 as AverageTonnage, 0 as AvgHireDays from VW_TblRegister where  ClientId = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "'='all'  or LocationId='" + locid + "' ) and (DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) union all select 0 as Erectedscaffold, 0 as StandingScaffold, cast(sum(isnull(Tonnage, 0)) / 1000 as decimal(18, 2)) as ErrectedTonnage ,0 as StandingTonnage, 0 as AverageTonnage,0 as AvgHireDays from VW_TblRegister where  ClientId = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "'='all'  or LocationId='" + locid + "' ) and (DateTagged <> '01/01/1900' and DateTagged is not null) union all select 0 as Erectedscaffold,0 as StandingScaffold,0 as ErrectedTonnage,cast(sum(isnull(Tonnage, 0)) / 1000 as decimal(18, 2)) as StandingTonnage, 0 as AverageTonnage,0 as AvgHireDays from VW_TblRegister where  ClientId = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "'='all'  or LocationId='" + locid + "' ) and (DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) union all select 0 as Erectedscaffold,0 as StandingScaffold,0 as ErrectedTonnage,0 as StandingTonnage,case when sum(isnull(Tonnage, 0)) = 0 then 0 else cast(sum(isnull(Tonnage, 0)) / (1000 * COUNT(*)) as decimal(18, 2)) End as AverageTonnage ,0 as AvgHireDays from VW_TblRegister where  ClientId = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "'='all'  or LocationId='" + locid + "' )and (DateTagged <> '01/01/1900' and DateTagged is not null)  union all select 0 as Erectedscaffold,0 as StandingScaffold,0 as ErrectedTonnage,0 as StandingTonnage, 0 as AverageTonnage, Convert(decimal(18,0),cast(Convert(decimal(18, 2), SUM(HireDays)) / Convert(decimal(18, 2), AVG(Counts)) as float)) as AvgHireDays from(select SUM(isnull(HireDaysToDate, 0)) as HireDays, COUNT(*) as Counts from VW_TblRegister where  ClientId = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "'='all'  or LocationId='" + locid + "' ) and (DateTagged <> '01/01/1900' or DateTagged is not null) union all select Sum(isnull(TotalHireDays, 0)) as HireDays, COUNT(*) as Counts from VW_TblRegister where  ClientId = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "'='all'  or LocationId='" + locid + "' ) and (DateTagged <> '01/01/1900' and DateTagged is not null))AverageDays ) forSiteQuery ";

            //// changed Query on 31/Oct/2020.
            //string query = "Select MAX(Erectedscaffold)as Erectedscaffold ,Max(StandingScaffold) as StandingScaffold,MAX(ErrectedTonnage) as ErrectedTonnage, MAX(StandingTonnage) as StandingTonnage,MAX(AverageTonnage) as AverageTonnage,MAX(AvgHireDays)as AvgHireDays    from (select COUNT('x') as Erectedscaffold, 0 as StandingScaffold, 0 as ErrectedTonnage, 0 as StandingTonnage, 0 as AverageTonnage, 0 as AvgHireDays   from TblRegister where  ClientId = '" + ClientID + "'and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "'='all'  or LocationId='" + locid + "' ) and (DateTagged <> '01/01/1900' and DateTagged is not null) union all select 0 as Erectedscaffold, COUNT('x') as StandingScaffold, 0 as ErrectedTonnage, 0 as StandingTonnage, 0 as AverageTonnage, 0 as AvgHireDays from TblRegister where  ClientId = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "'='all'  or LocationId='" + locid + "' ) and (DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) union all select 0 as Erectedscaffold, 0 as StandingScaffold, cast(sum(isnull(Tonnage, 0)) / 1000 as decimal(18, 2)) as ErrectedTonnage ,0 as StandingTonnage, 0 as AverageTonnage,0 as AvgHireDays from TblRegister where  ClientId = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "'='all'  or LocationId='" + locid + "' ) and (DateTagged <> '01/01/1900' and DateTagged is not null) union all select 0 as Erectedscaffold,0 as StandingScaffold,0 as ErrectedTonnage,cast(sum(isnull(Tonnage, 0)) / 1000 as decimal(18, 2)) as StandingTonnage, 0 as AverageTonnage,0 as AvgHireDays from TblRegister where  ClientId = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "'='all'  or LocationId='" + locid + "' ) and (DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) union all select 0 as Erectedscaffold,0 as StandingScaffold,0 as ErrectedTonnage,0 as StandingTonnage,case when sum(isnull(Tonnage, 0)) = 0 then 0 else cast(sum(isnull(Tonnage, 0)) / (1000 * COUNT('x')) as decimal(18, 2)) End as AverageTonnage ,0 as AvgHireDays from TblRegister where  ClientId = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "'='all'  or LocationId='" + locid + "' )and (DateTagged <> '01/01/1900' and DateTagged is not null)  union all select 0 as Erectedscaffold,0 as StandingScaffold,0 as ErrectedTonnage,0 as StandingTonnage, 0 as AverageTonnage, Convert(decimal(18,0),cast(Convert(decimal(18, 2), SUM(HireDays)) / Convert(decimal(18, 2), AVG(Counts)) as float)) as AvgHireDays from(select SUM(isnull(HireDaysToDate, 0)) as HireDays, COUNT('x') as Counts from VW_TblRegister where  ClientId = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "'='all'  or LocationId='" + locid + "' ) and (DateTagged <> '01/01/1900' and DateTagged is not null) and (DateofDismantle = '01/01/1900' or DateofDismantle is null) union all select Sum(CASE WHEN DateTagged !='1900-01-01' AND DateOfDismantle !='1900-01-01' THEN DateDiff(dd, DateTagged, DateOfDismantle) +1 ELSE NULL END)  as HireDays  , Count('x') as Counts from TblRegister where  ClientId = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "'='all'  or LocationId='" + locid + "' ) and (DateofDismantle <> '01/01/1900' and DateofDismantle is not null))AverageDays ) forSiteQuery ";

            // changed Query on 04/NMAr/2022.
            //string query = "Select MAX(Erectedscaffold)as Erectedscaffold ,Max(StandingScaffold) as StandingScaffold,MAX(ErrectedTonnage) as ErrectedTonnage, MAX(StandingTonnage) as StandingTonnage,MAX(AverageTonnage) as AverageTonnage,MAX(AvgHireDays)as AvgHireDays    from (select COUNT('x') as Erectedscaffold, 0 as StandingScaffold, 0 as ErrectedTonnage, 0 as StandingTonnage, 0 as AverageTonnage, 0 as AvgHireDays   from TblRegister where  ClientId = '" + ClientID + "'and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "'='all'  or LocationId='" + locid + "' ) and (DateTagged <> '01/01/1900' and DateTagged is not null) union all select 0 as Erectedscaffold, COUNT('x') as StandingScaffold, 0 as ErrectedTonnage, 0 as StandingTonnage, 0 as AverageTonnage, 0 as AvgHireDays from TblRegister where  ClientId = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "'='all'  or LocationId='" + locid + "' ) and (DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) union all select 0 as Erectedscaffold, 0 as StandingScaffold, cast(sum(isnull(Tonnage, 0)) / 1000 as decimal(18, 2)) as ErrectedTonnage ,0 as StandingTonnage, 0 as AverageTonnage,0 as AvgHireDays from TblRegister where  ClientId = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "'='all'  or LocationId='" + locid + "' ) and (DateTagged <> '01/01/1900' and DateTagged is not null) union all select 0 as Erectedscaffold,0 as StandingScaffold,0 as ErrectedTonnage,cast(sum(isnull(Tonnage, 0)) / 1000 as decimal(18, 2)) as StandingTonnage, 0 as AverageTonnage,0 as AvgHireDays from TblRegister where  ClientId = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "'='all'  or LocationId='" + locid + "' ) and (DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) union all select 0 as Erectedscaffold,0 as StandingScaffold,0 as ErrectedTonnage,0 as StandingTonnage,case when sum(isnull(Tonnage, 0)) = 0 then 0 else cast(sum(isnull(Tonnage, 0)) / (1000 * COUNT('x')) as decimal(18, 2)) End as AverageTonnage ,0 as AvgHireDays from TblRegister where  ClientId = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "'='all'  or LocationId='" + locid + "' )and (DateTagged <> '01/01/1900' and DateTagged is not null)  union all select 0 as Erectedscaffold,0 as StandingScaffold,0 as ErrectedTonnage,0 as StandingTonnage, 0 as AverageTonnage, Convert(decimal(18,0),cast(Convert(decimal(18, 2), SUM(HireDays)) / Convert(decimal(18, 2), AVG(Counts)) as float)) as AvgHireDays from(select SUM(isnull(HireDaysToDate, 0)) as HireDays, COUNT('x') as Counts from VW_TblRegister where  ClientId = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "'='all'  or LocationId='" + locid + "' ) and (DateTagged <> '01/01/1900' and DateTagged is not null)  union all select Sum(CASE WHEN DateTagged !='1900-01-01' AND DateOfDismantle !='1900-01-01' THEN DateDiff(dd, DateTagged, DateOfDismantle) +1 ELSE NULL END)  as HireDays  , Count('x') as Counts from TblRegister where  ClientId = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "'='all'  or LocationId='" + locid + "' ) and (DateTagged <> '01/01/1900' and DateTagged is not null))AverageDays ) forSiteQuery ";

            // Commented on 31-10-2023 Dashboard here is working slow for unispan after reviewing query we came to know that it was using vw_tblregister because of which it was getting slow so we are now using tblregister instead of vw_tblregister.
            //Chaneged on 04/Mar/2022
            //string query = "Select MAX(Erectedscaffold)as Erectedscaffold ,Max(StandingScaffold) as StandingScaffold,MAX(ErrectedTonnage) as ErrectedTonnage,  MAX(StandingTonnage) as StandingTonnage,MAX(AverageTonnage) as AverageTonnage,MAX(AvgHireDays) as AvgHireDays ,MAX(AverageStandingTonnage) as AverageStandingTonnage,MAX(AvgStandingHireDays) as AvgStandingHireDays from (select COUNT('x') as Erectedscaffold, 0 as StandingScaffold, 0 as ErrectedTonnage, 0 as StandingTonnage, 0 as AverageTonnage, 0 as AvgHireDays, 0 as AverageStandingTonnage, 0 as AvgStandingHireDays from TblRegister where ClientId = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "'='all'  or LocationId='" + locid + "' ) and (DateTagged <> '01/01/1900' and DateTagged is not null)  union all select 0 as Erectedscaffold, COUNT('x') as StandingScaffold, 0 as ErrectedTonnage, 0 as StandingTonnage, 0 as AverageTonnage, 0 as AvgHireDays, 0 as AverageStandingTonnage, 0 as AvgStandingHireDays from TblRegister where ClientId = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "'='all'  or LocationId='" + locid + "' ) and(DateTagged <> '01/01/1900' and DateTagged is not null) and (DateOfDismantle = '01/01/1900' or DateOfDismantle is null)  union all select 0 as Erectedscaffold, 0 as StandingScaffold, cast(sum(isnull(Tonnage, 0)) / 1000 as decimal(18, 2)) as ErrectedTonnage, 0 as StandingTonnage, 0 as AverageTonnage, 0 as AvgHireDays, 0 as AverageStandingTonnage, 0 as AvgStandingHireDays from TblRegister where ClientId = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "'='all'  or LocationId='" + locid + "' ) and(DateTagged <> '01/01/1900' and DateTagged is not null) union all select 0 as Erectedscaffold, 0 as StandingScaffold, 0 as ErrectedTonnage, cast(sum(isnull(Tonnage, 0)) / 1000 as decimal(18, 2)) as StandingTonnage, 0 as AverageTonnage, 0 as AvgHireDays, 0 as AverageStandingTonnage, 0 as AvgStandingHireDays from TblRegister where ClientId = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "'='all'  or LocationId='" + locid + "' )  and(DateTagged <> '01/01/1900' and DateTagged is not null) and (DateOfDismantle = '01/01/1900' or DateOfDismantle is null)  union all select 0 as Erectedscaffold, 0 as StandingScaffold , 0 as ErrectedTonnage, 0 as StandingTonnage,case when sum(isnull(Tonnage, 0)) = 0 then 0 else cast(sum(isnull(Tonnage, 0)) / (1000 * COUNT('x')) as decimal(18, 2)) End as AverageTonnage , 0 as AvgHireDays,0 as AverageStandingTonnage, 0 as AvgStandingHireDays from TblRegister where ClientId = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "'='all'  or LocationId='" + locid + "' ) and   (DateTagged <> '01/01/1900' and DateTagged is not null)  union all select 0 as Erectedscaffold,0 as StandingScaffold ,0 as ErrectedTonnage,0 as StandingTonnage, 0 as AverageTonnage,convert(decimal(18), cast(Convert(decimal(18, 2), SUM(HireDays)) / Convert(decimal(18, 2), AVG(Counts)) as float)) as AvgHireDays,0 as AverageStandingTonnage, 0 as AvgStandingHireDays from(select SUM(isnull(HireDaysToDate, 0)) as HireDays, COUNT('x') as Counts from VW_TblRegister where ClientId = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "'='all'  or LocationId='" + locid + "' )  and(DateTagged <> '01/01/1900' and DateTagged is not null)  union all  select Sum(CASE WHEN DateTagged != '1900-01-01' AND DateOfDismantle != '1900-01-01' THEN DateDiff(dd, DateTagged, DateOfDismantle) + 1 ELSE NULL END) as HireDays , Count('x') as Counts from TblRegister where ClientId = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "'='all'  or LocationId='" + locid + "' ) and (DateTagged <> '01/01/1900' and DateTagged is not null))AverageDays   union all select 0 as Erectedscaffold,0 as StandingScaffold ,0 as ErrectedTonnage,0 as StandingTonnage,0 as AverageTonnage , 0 as AvgHireDays,case when sum(isnull(Tonnage, 0)) = 0 then 0 else cast(sum(isnull(Tonnage, 0)) / (1000 * COUNT('x')) as decimal(18, 2)) End as AverageStandingTonnage, 0 as AvgStandingHireDays from TblRegister where ClientId = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "'='all'  or LocationId='" + locid + "' ) and(DateTagged <> '01/01/1900' and DateTagged is not null) and (DateOfDismantle = '01/01/1900' or DateOfDismantle is null)   union all select 0 as Erectedscaffold,0 as StandingScaffold ,0 as ErrectedTonnage,0 as StandingTonnage, 0 as AverageTonnage,0 as AvgHireDays,0 as AverageStandingTonnage, convert(decimal(18), cast(Convert(decimal(18, 2), SUM(HireDays)) / Convert(decimal(18, 2), AVG(Counts)) as float)) as AvgStandingHireDays from(select SUM(isnull(HireDaysToDate, 0)) as HireDays, COUNT('x') as Counts from VW_TblRegister where ClientId = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "'='all'  or LocationId='" + locid + "' ) and(DateTagged <> '01/01/1900' and DateTagged is not null) and (DateOfDismantle = '01/01/1900' or DateOfDismantle is null)    union all  select Sum(CASE WHEN DateTagged != '1900-01-01' AND DateOfDismantle != '1900-01-01' THEN DateDiff(dd, DateTagged, DateOfDismantle) + 1 ELSE NULL END) as HireDays , Count('x') as Counts from TblRegister where ClientId = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "'='all'  or LocationId='" + locid + "' ) and(DateTagged <> '01/01/1900' and DateTagged is not null) and (DateOfDismantle = '01/01/1900' or DateOfDismantle is null) )AverageStandingDays) forSiteQuery  ";

            // Added on 31-10-2023 Dashboard here is working slow for unispan after reviewing query we came to know that it was using vw_tblregister because of which it was getting slow so we are now using tblregister instead of vw_tblregister.
            string query = "Select MAX(Erectedscaffold)as Erectedscaffold ,Max(StandingScaffold) as StandingScaffold,MAX(ErrectedTonnage) as ErrectedTonnage,  MAX(StandingTonnage) as StandingTonnage,MAX(AverageTonnage) as AverageTonnage,MAX(AvgHireDays) as AvgHireDays ,MAX(AverageStandingTonnage) as AverageStandingTonnage,MAX(AvgStandingHireDays) as AvgStandingHireDays from (select COUNT('x') as Erectedscaffold, 0 as StandingScaffold, 0 as ErrectedTonnage, 0 as StandingTonnage, 0 as AverageTonnage, 0 as AvgHireDays, 0 as AverageStandingTonnage, 0 as AvgStandingHireDays from TblRegister where ClientID =  '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "'='all'  or LocationId='" + locid + "' ) and (DateTagged <> '01/01/1900' and DateTagged is not null)  union all select 0 as Erectedscaffold, COUNT('x') as StandingScaffold, 0 as ErrectedTonnage, 0 as StandingTonnage, 0 as AverageTonnage, 0 as AvgHireDays, 0 as AverageStandingTonnage, 0 as AvgStandingHireDays from TblRegister where ClientID =  '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "'='all'  or LocationId='" + locid + "' ) and(DateTagged <> '01/01/1900' and DateTagged is not null) and (DateOfDismantle = '01/01/1900' or DateOfDismantle is null)  union all select 0 as Erectedscaffold, 0 as StandingScaffold, cast(sum(isnull(Tonnage, 0)) / 1000 as decimal(18, 2)) as ErrectedTonnage, 0 as StandingTonnage, 0 as AverageTonnage, 0 as AvgHireDays, 0 as AverageStandingTonnage, 0 as AvgStandingHireDays from TblRegister where ClientID = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "'='all'  or LocationId='" + locid + "' ) and(DateTagged <> '01/01/1900' and DateTagged is not null) union all select 0 as Erectedscaffold, 0 as StandingScaffold, 0 as ErrectedTonnage, cast(sum(isnull(Tonnage, 0)) / 1000 as decimal(18, 2)) as StandingTonnage, 0 as AverageTonnage, 0 as AvgHireDays, 0 as AverageStandingTonnage, 0 as AvgStandingHireDays from TblRegister where ClientID = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "'='all'  or LocationId='" + locid + "' )  and(DateTagged <> '01/01/1900' and DateTagged is not null) and (DateOfDismantle = '01/01/1900' or DateOfDismantle is null)  union all select 0 as Erectedscaffold, 0 as StandingScaffold , 0 as ErrectedTonnage, 0 as StandingTonnage,case when sum(isnull(Tonnage, 0)) = 0 then 0 else cast(sum(isnull(Tonnage, 0)) / (1000 * COUNT('x')) as decimal(18, 2)) End as AverageTonnage , 0 as AvgHireDays,0 as AverageStandingTonnage, 0 as AvgStandingHireDays from TblRegister where ClientID = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "'='all'  or LocationId='" + locid + "' ) and   (DateTagged <> '01/01/1900' and DateTagged is not null)  union all select 0 as Erectedscaffold,0 as StandingScaffold ,0 as ErrectedTonnage,0 as StandingTonnage, 0 as AverageTonnage,convert(decimal(18), cast(Convert(decimal(18, 2), SUM(HireDays)) / Convert(decimal(18, 2), AVG(Counts)) as float)) as AvgHireDays,0 as AverageStandingTonnage, 0 as AvgStandingHireDays from(select SUM(isnull(CASE WHEN DateTagged !='1900-01-01' AND DateOfDismantle ='1900-01-01' THEN DateDiff(dd, DateTagged, CONVERT(date, Getdate())) ELSE NULL END, 0)) as HireDays, COUNT('x') as Counts from TblRegister where ClientID = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "'='all'  or LocationId='" + locid + "' )  and(DateTagged <> '01/01/1900' and DateTagged is not null)  union all  select Sum(CASE WHEN DateTagged != '1900-01-01' AND DateOfDismantle != '1900-01-01' THEN DateDiff(dd, DateTagged, DateOfDismantle) + 1 ELSE NULL END) as HireDays , Count('x') as Counts from TblRegister where ClientID = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "'='all'  or LocationId='" + locid + "' ) and (DateTagged <> '01/01/1900' and DateTagged is not null))AverageDays   union all select 0 as Erectedscaffold,0 as StandingScaffold ,0 as ErrectedTonnage,0 as StandingTonnage,0 as AverageTonnage , 0 as AvgHireDays,case when sum(isnull(Tonnage, 0)) = 0 then 0 else cast(sum(isnull(Tonnage, 0)) / (1000 * COUNT('x')) as decimal(18, 2)) End as AverageStandingTonnage, 0 as AvgStandingHireDays from TblRegister where ClientID = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "'='all'  or LocationId='" + locid + "' ) and(DateTagged <> '01/01/1900' and DateTagged is not null) and (DateOfDismantle = '01/01/1900' or DateOfDismantle is null)   union all select 0 as Erectedscaffold,0 as StandingScaffold ,0 as ErrectedTonnage,0 as StandingTonnage, 0 as AverageTonnage,0 as AvgHireDays,0 as AverageStandingTonnage, convert(decimal(18), cast(Convert(decimal(18, 2), SUM(HireDays)) / Convert(decimal(18, 2), AVG(Counts)) as float)) as AvgStandingHireDays from(select SUM(isnull(CASE WHEN DateTagged !='1900-01-01' AND DateOfDismantle ='1900-01-01' THEN DateDiff(dd, DateTagged, CONVERT(date, Getdate())) ELSE NULL END, 0)) as HireDays, COUNT('x') as Counts from TblRegister where ClientID = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "'='all'  or LocationId='" + locid + "' ) and(DateTagged <> '01/01/1900' and DateTagged is not null) and (DateOfDismantle = '01/01/1900' or DateOfDismantle is null)    union all  select Sum(CASE WHEN DateTagged != '1900-01-01' AND DateOfDismantle != '1900-01-01' THEN DateDiff(dd, DateTagged, DateOfDismantle) + 1 ELSE NULL END) as HireDays , Count('x') as Counts from TblRegister where ClientID = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "' = 'all'  or LocationId='" + locid + "' ) and(DateTagged <> '01/01/1900' and DateTagged is not null) and (DateOfDismantle = '01/01/1900' or DateOfDismantle is null) )AverageStandingDays) forSiteQuery";
            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ScaffoldAllDashBoardViewModel pro = new ScaffoldAllDashBoardViewModel();
                    if (!(item["Erectedscaffold"] is DBNull) && item["Erectedscaffold"].ToString() != "")
                    {
                        pro.ErectedDate2 = item["Erectedscaffold"].ToString();
                    }
                    if (!(item["StandingScaffold"] is DBNull) && item["StandingScaffold"].ToString() != "")
                    {
                        pro.ScaffoldsStanding2 = item["StandingScaffold"].ToString();
                    }
                    if (!(item["ErrectedTonnage"] is DBNull) && item["ErrectedTonnage"].ToString() != "")
                    {
                        pro.TonnageErected2 = item["ErrectedTonnage"].ToString();
                    }
                    if (!(item["StandingTonnage"] is DBNull) && item["StandingTonnage"].ToString() != "")
                    {
                        pro.TonnageStanding2 = item["StandingTonnage"].ToString();
                    }
                    if (!(item["AverageTonnage"] is DBNull) && item["AverageTonnage"].ToString() != "")
                    {
                        pro.AverageTonnage2 = item["AverageTonnage"].ToString();
                    }
                    if (!(item["AvgHireDays"] is DBNull) && item["AvgHireDays"].ToString() != "")
                    {
                        pro.HireDays2 = item["AvgHireDays"].ToString();
                    }
                    if (!(item["AverageStandingTonnage"] is DBNull) && item["AverageStandingTonnage"].ToString() != "")
                    {
                        pro.AverageStandingTonnage2 = item["AverageStandingTonnage"].ToString();
                    }
                    if (!(item["AvgStandingHireDays"] is DBNull) && item["AvgStandingHireDays"].ToString() != "")
                    {
                        pro.AvgStandingHireDays2 = item["AvgStandingHireDays"].ToString();
                    }
                    
                    LocationdataDetails.Add(pro);
                }
            }
            // return LocationdataDetails;

            //Commented on(02 - 02 - 2024) we added new client Acrow Snowy Hydro for which when we runned this query it returned 718 Records because of which we were getting object:object error.so to overcome the issue we added MaxJsonLength which will handle maximum response.
            //return Json(LocationdataDetails, JsonRequestBehavior.AllowGet);

            //Added on (02-02-2024)
            var jsonResult = Json(LocationdataDetails, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        /// Site Analytics Dashboard Scaffolds by Op Centre Textboxes Query End

        /// Site Analytics Dashboard Tonnage by Building Chart 3 Start
        List<ScaffoldAllDashBoardViewModel> TonnageByJobDashboard = new List<ScaffoldAllDashBoardViewModel> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod(EnableSession = true)]
        public ActionResult GetTonnageByJobDashboard(string Siteid, string locid, string buildid, string ClientID)
        {
            // 08-nov-2018 ignore these 2 lines  string query = "Select Max(TotalRegister) as TotalRegister, Max(LockedInRegister) as LockedInRegister, Case when Max(TotalRegister) = 0 Or Max(LockedInRegister) = 0 then 0 else  Round((Convert(float, Max(LockedInRegister)) * 100.00) / Convert(float, Max(TotalRegister)), 0) end as LockedPct From(select COUNT(*) as TotalRegister, '' as LockedInRegister from tblregister where ClientID = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "')  and(DateTagged <> '01/01/1900' and DateTagged is not null) and (DateOfDismantle = '01/01/1900' or DateOfDismantle is null) union all select '' as TotalRegister, COUNT(*) as LockedInRegister from tblregister where ClientID = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "')  and(DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) and LockedIn = 1) LockedQuery";
            //    string query = "Select Max(TotalRegister) as TotalRegister, Max(LockedInRegister) as LockedInRegister, Case when Max(TotalRegister) = 0 Or Max(LockedInRegister) = 0 then 0 else  Round((Convert(float, Max(LockedInRegister)) * 100.00) / Convert(float, Max(TotalRegister)), 0) end as LockedPct From(select COUNT(*) as TotalRegister, '' as LockedInRegister from tblregister where ClientID = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "')  and(DateTagged <> '01/01/1900' and DateTagged is not null)  union all select '' as TotalRegister, COUNT(*) as LockedInRegister from tblregister where ClientID = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "')  and(DateTagged <> '01/01/1900' and DateTagged is not null)  and LockedIn = 1) LockedQuery";          

            string query = "select  Right ('000000' + convert (varchar, [scaffref]),6) as Description,convert(decimal(18,2),Sum(isnull(Tonnage,0))/1000) TonnageStanding ,case when totalTonnageStanding > 0 then round(convert(decimal(18,2),Sum(isnull(Tonnage,0))/1000)/totalTonnageStanding,2) else 0 end as Pct,totalTonnageStanding from TblRegister,(select  convert(decimal(18, 2), Sum(isnull(Tonnage, 0)) / 1000) totalTonnageStanding from TblRegister where ClientID = '" + ClientID + "' and ('" + Siteid + "'='all'  or SiteLocId='" + Siteid + "' ) and ('" + locid + "'='all'  or LocationId='" + locid + "' ) and  ('" + buildid + "'='all'  or buildingId='" + buildid + "' ) and(DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null)) as temp where ClientID = '" + ClientID + "' and ('" + Siteid + "'='all'  or SiteLocId='" + Siteid + "' ) and ('" + locid + "'='all'  or LocationId='" + locid + "' ) and  ('" + buildid + "'='all'  or buildingId='" + buildid + "' ) and(DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) group by scaffref ,totalTonnageStanding order by Sum(isnull(Tonnage, 0)) / 1000 desc, scaffref";

            //select top 20 Right ('000000' + convert (varchar, [scaffref]),6) as Description,convert(decimal(18,2),Sum(isnull(Tonnage,0))/1000) TonnageStanding from VW_TblRegister where ClientID = '" + ClientID + "' and ('" + Siteid + "'='all'  or SiteLocId='" + Siteid + "' ) and ('" + locid + "'='all'  or LocationId='" + locid + "' ) and  ('" + buildid + "'='all'  or buildingId='" + buildid + "' ) and (DateTagged<>'01/01/1900' and DateTagged is not null) and (DateOfDismantle='01/01/1900' or DateOfDismantle is null) group by scaffref order by Sum(isnull(Tonnage,0))/1000 desc,scaffref";


            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ScaffoldAllDashBoardViewModel pro = new ScaffoldAllDashBoardViewModel();
                    if (!(item["Description"] is DBNull) && item["Description"].ToString() != "")
                    {
                        pro.Description = item["Description"].ToString();
                    }
                    if (!(item["TonnageStanding"] is DBNull) && item["TonnageStanding"].ToString() != "")
                    {
                        pro.TonnageStanding = Convert.ToDecimal(item["TonnageStanding"]);
                    }
                    if (!(item["Pct"] is DBNull) && item["Pct"].ToString() != "")
                    {
                        pro.TonnageByJob1Pct = Convert.ToDecimal(item["Pct"]);
                    }
                    if (!(item["totalTonnageStanding"] is DBNull) && item["totalTonnageStanding"].ToString() != "")
                    {
                        pro.totalTonnageStanding = Convert.ToDecimal(item["totalTonnageStanding"]);
                    }
                    TonnageByJobDashboard.Add(pro);
                }
            }
            // return TonnageByJobDashboard;
            //Commented on (02-02-2024) we added new client Acrow Snowy Hydro for which when we runned this query it returned 718 Records because of which we were getting object:object error. so to overcome the issue we added MaxJsonLength which will handle maximum response.
            //return Json(TonnageByJobDashboard,JsonRequestBehavior.AllowGet);
            //Added on (02-02-2024)
            var jsonResult = Json(TonnageByJobDashboard, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        /// Site Analytics Dashboard Tonnage by Building Chart 3 Start

        /// Site Analytics Dashboard Avg Standing Days (Currently Standing) Chart 4 Start
        List<ScaffoldAllDashBoardViewModel> HireperjobDashboard = new List<ScaffoldAllDashBoardViewModel> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod(EnableSession = true)]
        public ActionResult GetHireperjobDashboard(string Siteid, string locid, string buildid, string ClientID)
        {
            // Commented on 31-10-2023 Dashboard here is working slow for unispan after reviewing query we came to know that it was using vw_tblregister because of which it was getting slow so we are now using tblregister instead of vw_tblregister.
            //string query = "select Right ('000000' + convert (varchar, [scaffref]),6) as Description,Sum(isnull(HireDaysToDate,0)) as HireDays ,Convert(decimal(18,0),Round(Convert(decimal(18, 2),TotalDays)/Convert(decimal(18, 2),TotalRegister),0)) as AvgHireDays from VW_TblRegister, (select  Sum(isnull(HireDaysToDate, 0)) as totalDays, COUNT(*) as TotalRegister  from VW_TblRegister where ClientID = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and('" + locid + "' = 'all'  or LocationId = '" + locid + "') and('" + buildid + "' = 'all'  or buildingId = '" + buildid + "') and(DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null)) as temp where ClientID = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and('" + locid + "' = 'all'  or LocationId = '" + locid + "') and('" + buildid + "' = 'all'  or buildingId = '" + buildid + "') and(DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) group by scaffref,TotalDays,TotalRegister order by Sum(isnull(HireDaysToDate, 0)) desc, scaffref";

            // Added on 31-10-2023 Dashboard here is working slow for unispan after reviewing query we came to know that it was using vw_tblregister because of which it was getting slow so we are now using tblregister instead of vw_tblregister.
            string query = "select Right ('000000' + convert (varchar, [scaffref]),6) as Description,Sum(isnull(CASE WHEN DateTagged !='1900-01-01' AND DateOfDismantle ='1900-01-01' THEN DateDiff(dd, DateTagged, CONVERT(date, Getdate()))  ELSE NULL END,0)) as HireDays ,Convert(decimal(18, 0), Round(Convert(decimal(18, 2), TotalDays) / Convert(decimal(18, 2), TotalRegister), 0)) as AvgHireDays from TblRegister, (select  Sum(isnull(CASE WHEN DateTagged != '1900-01-01' AND DateOfDismantle = '1900-01-01' THEN DateDiff(dd, DateTagged, CONVERT(date, Getdate())) ELSE NULL END, 0)) as totalDays, COUNT(*) as TotalRegister  from TblRegister where ClientId = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "' = 'all'  or LocationId = '" + locid + "') and('" + buildid + "' = 'all'  or buildingId = '" + buildid + "') and (DateTagged <> '01/01/1900' and DateTagged is not null) and (DateOfDismantle = '01/01/1900' or DateOfDismantle is null)) as temp where ClientId = '" + ClientID + "' and ('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and ('" + locid + "' = 'all'  or LocationId = '" + locid + "') and ('" + buildid + "' = 'all'  or buildingId = '" + buildid + "') and (DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) group by scaffref,TotalDays,TotalRegister order by Sum(isnull(CASE WHEN DateTagged != '1900-01-01' AND DateOfDismantle = '1900-01-01' THEN DateDiff(dd, DateTagged, CONVERT(date, Getdate())) ELSE NULL END, 0)) desc, scaffref";

            //select top 20 Right ('000000' + convert (varchar, [scaffref]),6) as Description,Sum(isnull(HireDaysToDate,0)) as HireDays from VW_TblRegister where ClientID='" + ClientID + "' and ('" + Siteid + "'='all'  or SiteLocId='" + Siteid + "' ) and ('" + locid + "'='all'  or LocationId='" + locid + "' ) and  ('" + buildid + "'='all'  or buildingId='" + buildid + "' ) and (DateTagged<>'01/01/1900' and DateTagged is not null) and (DateOfDismantle='01/01/1900' or DateOfDismantle is null) group by scaffref  order by Sum(isnull(HireDaysToDate,0)) desc, scaffref";

            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ScaffoldAllDashBoardViewModel pro = new ScaffoldAllDashBoardViewModel();
                    if (!(item["Description"] is DBNull) && item["Description"].ToString() != "")
                    {
                        pro.Description = item["Description"].ToString();
                    }
                    if (!(item["HireDays"] is DBNull) && item["HireDays"].ToString() != "")
                    {
                        pro.HireDays = Convert.ToDecimal(item["HireDays"]);
                    }
                    if (!(item["AvgHireDays"] is DBNull) && item["AvgHireDays"].ToString() != "")
                    {
                        pro.AvgHireDays1 = item["AvgHireDays"].ToString();
                    }

                    HireperjobDashboard.Add(pro);
                }
            }
            //return HireperjobDashboard;
            //Commented on (02-02-2024) we added new client Acrow Snowy Hydro for which when we runned this query it returned 718 Records because of which we were getting object:object error. so to overcome the issue we added MaxJsonLength which will handle maximum response.
            //return Json(HireperjobDashboard,JsonRequestBehavior.AllowGet);

            //Added on (02-02-2024)
            var jsonResult = Json(HireperjobDashboard, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        /// Site Analytics Dashboard Avg Standing Days (Currently Standing) Chart 4 End 

    }
}