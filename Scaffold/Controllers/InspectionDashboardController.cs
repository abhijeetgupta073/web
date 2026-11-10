using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;

namespace Scaffold.Controllers
{
    public class InspectionDashboardController : Controller
    {
        ScaffoldEntities dbcontext = new ScaffoldEntities();
        // GET: InspectionDashboard
        public ActionResult InspectionDashboard()
        {
            return View();
        }

        /// InspectionDashboard  Site Dropdown Start


        public ActionResult SiteDropDownBind(string id, string UID)
        {
            var SiteOwnerName = "";
            var Site = "";
            string query1 = "select Site, SiteOwnerName from tblUser where ClientId='" + id + "' and UserId='" + databaseCon.Decrypt(UID) + "'";
            DataTable dt1 = databaseCon.GetData(query1);
            if (dt1.Rows.Count > 0)
            {
                SiteOwnerName = dt1.Rows[0]["SiteOwnerName"].ToString();
                Site = dt1.Rows[0]["Site"].ToString();
            }
            if (SiteOwnerName != "")
            {
                // Escape single quote by replacing ' with ''
                SiteOwnerName = SiteOwnerName.Replace("'", "''");
                var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select SiteLocID,SiteLocation from tblsitelocation where ClientId='" + id + "'and OwnerName='" + SiteOwnerName + "' Order by SiteLocation").ToList();
                return Json(GetData, JsonRequestBehavior.AllowGet);
            }
            else if (!Site.IsNullOrWhiteSpace())
            {
                var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select SiteLocID,SiteLocation from tblsitelocation where ClientId='" + id + "'and SiteLocID='" + Site + "' Order by SiteLocation").ToList();
                return Json(GetData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select * from (select 0 as SiteLocID,'All' as SiteLocation union select SiteLocID,SiteLocation from tblsitelocation where ClientId = '" + id + "') as sitelisting order by SiteLocation").ToList();
                return Json(GetData, JsonRequestBehavior.AllowGet);
            }
        }
        ///InspectionDashboard Site Dropdown End

        /// InspectionDashboard  Location Dropdown Start

        //public ActionResult ddlJobDashboard(string id, string SiteId)
        //{
        //    var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select distinct Location as area,LocationId as areaid from VW_TblRegister where (Tonnage is not null or ManHrsDismantle is not null) and (SiteLocId='" + SiteId + "' or 'All'='" + SiteId + "') and ClientID='" + id + "' Order by Location").ToList();
        //    return Json(GetData, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult ddlJobDashboard(string id, string SiteId, string UID)
        {
            var Location = "";
            var Site = "";
            string query1 = "select Site, Location from tblUser where ClientId='" + id + "' and UserId='" + databaseCon.Decrypt(UID) + "'";
            DataTable dt1 = databaseCon.GetData(query1);
            if (dt1.Rows.Count > 0)
            {
                Location = dt1.Rows[0]["Location"].ToString();
                Site = dt1.Rows[0]["Site"].ToString();
            }
            if (!Site.IsNullOrWhiteSpace() && !Location.IsNullOrWhiteSpace())
            {
                var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select distinct Location as area,LocationId as areaid from VW_TblRegister where (Tonnage is not null or ManHrsDismantle is not null) and SiteLocId='" + Site + "'and LocationId ='" + Location + "' and ClientID='" + id + "' Order by Location").ToList();
                if(GetData.Count == 0)
                {
                     GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select Area + ' - No Jobs for This Location' as Area ,0 as areaid  from tblArea where SiteLocId='" + Site + "' and  AreaID = '" + Location + "' and ClientID='" + id + "' ").ToList();
                }

                return Json(GetData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select distinct Location as area,LocationId as areaid from VW_TblRegister where (Tonnage is not null or ManHrsDismantle is not null) and (SiteLocId='" + SiteId + "' or 'All'='" + SiteId + "') and ClientID='" + id + "' Order by Location").ToList();
                return Json(GetData, JsonRequestBehavior.AllowGet);
            }


        }
        ///InspectionDashboard Location Dropdown End


        //16-Aug-2019-InspectionDashboard complianceGuadequary start

        List<ScaffoldAllDashBoardViewModel> CompliantByClientid = new List<ScaffoldAllDashBoardViewModel> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod(EnableSession = true)]
        // public List<Scaffold> GetByClientidcompliantDashboard(string ClientID, string siteid, string locationID)
        public ActionResult GetByClientidcompliantDashboard(string ClientID, string siteid, string locationID)
        {

            // Query Change On 02 Sep 2019

            //  string query = "Select Isnull(Sum(Case when Compliance='Compliance' then 1 Else 0 End),0) as Compliance,Isnull(Sum(Case when Compliance='NonCompliance' then 1 Else 0 End),0) as NonCompliance,Isnull(Sum(Case when Compliance='Compliance' then 1 Else 0 End)+Sum(Case when Compliance='NonCompliance' then 1 Else 0 End) ,0) as Total,Isnull(Case when Sum(Case when Compliance='Compliance' then 1 Else 0 End)=0 or (Sum(Case when Compliance='Compliance' then 1 Else 0 End)+Sum(Case when Compliance='NonCompliance' then 1 Else 0 End))=0 then 0 Else Round((Convert(float,Sum(Case when Compliance='Compliance' then 1 Else 0 End))*100.00)/Convert(float,(Sum(Case when Compliance='Compliance' then 1 Else 0 End)+Sum(Case when Compliance='NonCompliance' then 1 Else 0 End))),0) End,0) CompliancePct,Isnull(Case when Sum(Case when Compliance='NonCompliance' then 1 Else 0 End)=0 or (Sum(Case when Compliance='Compliance' then 1 Else 0 End)+Sum(Case when Compliance='NonCompliance' then 1 Else 0 End))=0 then 0 Else Round((Convert(float,Sum(Case when Compliance='NonCompliance' then 1 Else 0 End))*100.00)/Convert(float,(Sum(Case when Compliance='Compliance' then 1 Else 0 End)+Sum(Case when Compliance='NonCompliance' then 1 Else 0 End))),0) End ,0) NonCompliancePct From(Select AllWithDaysDiff.*,Compliance from (Select FirstQuery.*,Case when SEcondQuery.InspectionDate is null then Datediff(d,FirstQuery.InspectionDate,getdate()) Else Datediff(d,FirstQuery.InspectionDate,SEcondQuery.InspectionDate) End  as DaysDifference from (select InspectionID,Convert(Date,InspectionDate)InspectionDate,tblInspection.RegisterID ,DateTagged,InspTimeFrame,ROW_NUMBER() OVER (Order by tblInspection.RegisterId,InspectionDate,InspectionID) RowNumber From tblInspection join tblRegister on tblRegister.RegisterId=tblInspection.RegisterID where ClientID='" + ClientID + "' and  (SiteLocId = '" + siteid + "' or '" + siteid + "' = 'ALL') and(LocationId = '" + locationID + "'or '" + locationID + "' = 'ALL') and (DateTagged!='01/01/1900' and DateTagged is not null)and (DateOfDismantle is null or DateOfDismantle='01/01/1900' ) )FirstQuery Left join (select InspectionID,Convert(Date,InspectionDate)InspectionDate,tblInspection.RegisterID ,DateTagged,InspTimeFrame,ROW_NUMBER() OVER (Order by tblInspection.RegisterId,InspectionDate,InspectionID) RowNumber From tblInspection join tblRegister on tblRegister.RegisterId=tblInspection.RegisterID where ClientID='" + ClientID + "' and(SiteLocId = '" + siteid + "' or '" + siteid + "' = 'ALL') and(LocationId = '" + locationID + "' or '" + locationID + "' = 'ALL') and (DateTagged!='01/01/1900' and DateTagged is not null) and (DateOfDismantle is null or DateOfDismantle='01/01/1900' ) )SecondQuery on FirstQuery.RegisterId=SEcondQuery.RegisterId and FirstQuery.InspectionDate<SEcondQuery.InspectionDate and FirstQuery.RowNumber+1=SecondQuery.RowNumber )AllWithDaysDiff join (Select *,Case when Colour='Red' then 'NonCompliance' else 'Compliance' End as Compliance from tblStatusColourMaping )tblStatusColourMaping on AllWithDaysDiff.InspTimeFrame=tblStatusColourMaping.TimeFrame and DaysDifference >= DaysDiffFrom and DaysDifference < DaysDiffTo)AllComplaince";
            
            
            //Commented on 10-dec-2024 by Abhijeet Gupta

            //string query = "Select Sum(Compliance) as Compliance,Sum(NonCompliance) as NonCompliance, Sum(Compliance) + Sum(NonCompliance) as Total,Case when(Sum(Compliance) + Sum(NonCompliance)) > 0 then Round(Convert(decimal(18, 2), (Convert(decimal(18, 2), Sum(Compliance)) /(Sum(Compliance) + Sum(NonCompliance)) * 100.00)), 0) End as CompliancePct,Case when Sum(Compliance) + Sum(NonCompliance) > 0 then Round(Convert(decimal(18, 2), (Convert(decimal(18, 2), Sum(NonCompliance)) /(Sum(Compliance) + Sum(NonCompliance)) * 100.00)), 0) End as NonCompliancePct from(Select COUNT(*) as Compliance, 0 as NonCompliance from vw_TblRegister where ClientID='" + ClientID + "' and(SiteLocId = '" + siteid + "' or '" + siteid + "' = 'ALL') and(LocationId = '" + locationID + "' or '" + locationID + "' = 'ALL')  and  Status in ('Green', 'Yellow', 'Out of service') union all Select 0 as Compliance, COUNT(*) as NonCompliance from vw_TblRegister where ClientID='" + ClientID + "' and(SiteLocId = '" + siteid + "' or '" + siteid + "' = 'ALL') and(LocationId = '" + locationID + "' or '" + locationID + "' = 'ALL')  and  Status in ('Red')) AllComplaince";


            string query = "Select Sum(Compliance) as Compliance,Sum(NonCompliance) as NonCompliance, Sum(Compliance) + Sum(NonCompliance) as Total,Case when(Sum(Compliance) + Sum(NonCompliance)) > 0 then Convert(decimal(18, 2), (Convert(decimal(18, 2), Sum(Compliance)) /(Sum(Compliance) + Sum(NonCompliance)) * 100.00)) End as CompliancePct,Case when Sum(Compliance) + Sum(NonCompliance) > 0 then Convert(decimal(18, 2), (Convert(decimal(18, 2), Sum(NonCompliance)) /(Sum(Compliance) + Sum(NonCompliance)) * 100.00)) End as NonCompliancePct from(Select COUNT(*) as Compliance, 0 as NonCompliance from vw_TblRegister where ClientID='" + ClientID + "' and(SiteLocId = '" + siteid + "' or '" + siteid + "' = 'ALL') and(LocationId = '" + locationID + "' or '" + locationID + "' = 'ALL')  and  Status in ('Green', 'Yellow', 'Out of service','Restricted Access') union all Select 0 as Compliance, COUNT(*) as NonCompliance from vw_TblRegister where ClientID='" + ClientID + "' and(SiteLocId = '" + siteid + "' or '" + siteid + "' = 'ALL') and(LocationId = '" + locationID + "' or '" + locationID + "' = 'ALL')  and  Status in ('Red')) AllComplaince";

            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ScaffoldAllDashBoardViewModel pros = new ScaffoldAllDashBoardViewModel();
                    if (!(item["Compliance"] is DBNull) && item["Compliance"].ToString() != "")
                    {
                        pros.TotalCompliance = item["Compliance"].ToString();
                    }
                    if (!(item["NonCompliance"] is DBNull) && item["NonCompliance"].ToString() != "")
                    {
                        pros.TotaNonCompliance = item["NonCompliance"].ToString();
                    }
                    if (!(item["Total"] is DBNull) && item["Total"].ToString() != "")
                    {
                        pros.InspectDashTotal = item["Total"].ToString();
                    }
                    if (!(item["CompliancePct"] is DBNull) && item["CompliancePct"].ToString() != "")
                    {
                        pros.CompliancePct = item["CompliancePct"].ToString();
                    }
                    if (!(item["NonCompliancePct"] is DBNull) && item["NonCompliancePct"].ToString() != "")
                    {
                        pros.NonCompliancePct = item["NonCompliancePct"].ToString();
                    }

                    CompliantByClientid.Add(pros);
                }
            }
            //  return CompliantByClientid;
            return Json(CompliantByClientid, JsonRequestBehavior.AllowGet);
        }


        //16-Aug-2019-InspectionDashboard complianceGuadequary end

        //16 Aug 2019 Inspection dashboard onclickQuery start

        List<ScaffoldAllDashBoardViewModel> InspectionDashboardClick = new List<ScaffoldAllDashBoardViewModel> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod(EnableSession = true)]
        //public List<Scaffold> InspectionGetBystatus(string Status, string ClientID, string siteid, string locationID)
        public ActionResult InspectionGetBystatus(string Status, string ClientID, string siteid, string locationID)
        {
            string ConditionAddNoAction = "";
            if (Status == "No Action")
            {
                ConditionAddNoAction = "and (Status='" + Status + "' or CycloneAction is null) ";
            }
            else
            {
                ConditionAddNoAction = "and Status='" + Status + "'";
            }
            // string query = "select Right ('000000' + convert (varchar, [ScaffRef]),6) as ScaffRef,Description,Location,BuildingNo,AssetNo,FloorLevel,DesignType,MaterialType, TotalLabourDismantleEdit,PriorityRiskRating from VW_TblRegister where (SiteLocId = '" + siteid + "' or '" + siteid + "' = 'ALL') and  (LocationID = '" + locationID + "' or '" + locationID + "' = 'ALL') and ClientID = '" + ClientID + "' and Status = '" + Status + "'";
            string query = "select Right ('000000' + convert (varchar, [ScaffRef]),6) as ScaffRef,Description,Location,BuildingNo,AssetNo,FloorLevel,WONumber,ClientScaffref,DesignType,MaterialType, TotalLabourDismantleEdit,PriorityRiskRating,SiteLocation,DateTagged,DaysSinceLastInsp,RecentInspDate,HireDaysToDate from VW_TblRegister where (SiteLocId = '" + siteid + "' or '" + siteid + "' = 'ALL') and  (LocationID = '" + locationID + "' or '" + locationID + "' = 'ALL') and ClientID = '" + ClientID + "' and Status = '" + Status + "'";

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
                    if (!(item["WONumber"] is DBNull) && item["WONumber"].ToString() != "")
                    {
                        pro.WONumber = item["WONumber"].ToString();
                    }
                    else
                    {
                        pro.WONumber = "";
                    }
                    if (!(item["ClientScaffref"] is DBNull) && item["ClientScaffref"].ToString() != "")
                    {
                        pro.ClientScaffref = item["ClientScaffref"].ToString();
                    }
                    else
                    {
                        pro.ClientScaffref = "";
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

                    if (!(item["SiteLocation"] is DBNull) && item["SiteLocation"].ToString() != "")
                    {
                        pro.SiteLocation = item["SiteLocation"].ToString();
                    }
                    else
                    {
                        pro.SiteLocation = "";
                    }
                    if (!(item["DateTagged"] is DBNull) && item["DateTagged"].ToString() != "")
                    {
                        pro.DateTagged = item["DateTagged"].ToString();
                    }
                    else
                    {
                        pro.DateTagged = "";
                    }
                    if (!(item["DaysSinceLastInsp"] is DBNull) && item["DaysSinceLastInsp"].ToString() != "")
                    {
                        pro.DaysSinceLastInsp = item["DaysSinceLastInsp"].ToString();
                    }
                    else
                    {
                        pro.DaysSinceLastInsp = "";
                    }
                    if (!(item["RecentInspDate"] is DBNull) && item["RecentInspDate"].ToString() != "")
                    {
                        pro.RecentInspDate = item["RecentInspDate"].ToString();
                    }
                    else
                    {
                        pro.RecentInspDate = "";
                    }

                    if (!(item["HireDaysToDate"] is DBNull) && item["HireDaysToDate"].ToString() != "")
                    {
                        pro.HireDaysToDate = item["HireDaysToDate"].ToString();
                    }
                    else
                    {
                        pro.HireDaysToDate = "";
                    }

                    InspectionDashboardClick.Add(pro);
                }
            }
            // return InspectionDashboardClick;
            return Json(InspectionDashboardClick, JsonRequestBehavior.AllowGet);
        }
        //14 Aug 2019 inspection dashboard onclickQuery End

        // Start Site Total Count Start

        public ActionResult InspectionGetBySiteTotal(string ClientID, string siteid)
        {
            // Scaffold site = new Scaffold();
            var SiteTotal = "";
            string query = "Select COUNT(*) as SiteTotal from vw_TblRegister where Status in ('Green','Red','Yellow', 'Out of service') and ClientId = '" + ClientID + "' and (SiteLocId = '" + siteid + "' or '" + siteid + "' = 'ALL')";
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                //site.SiteTotal = dt.Rows[0]["SiteTotal"].ToString();
                SiteTotal = dt.Rows[0]["SiteTotal"].ToString();
            }
            // return site.SiteTotal;
            return Json(new { SiteTotal = SiteTotal }, JsonRequestBehavior.AllowGet);
        }
        // Start Site  Total Count End

        // Start  Location Total Count Start

        public ActionResult InspectionGetByLocationTotal(string ClientID, string siteid, string locationID)
        {
            // Scaffold location = new Scaffold();
            var LocationTotal = "";
            string query = "Select COUNT(*) as LocationTotal from vw_TblRegister where Status in ('Green','Red','Yellow', 'Out of service') and ClientId = '" + ClientID + "' and (SiteLocId = '" + siteid + "' or '" + siteid + "' = 'ALL') and(LocationID = '" + locationID + "' or '" + locationID + "' = 'ALL') ";
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                //location.LocationTotal = dt.Rows[0]["LocationTotal"].ToString();
                LocationTotal = dt.Rows[0]["LocationTotal"].ToString();
            }
            //return location.LocationTotal;
            return Json(new { LocationTotal = LocationTotal }, JsonRequestBehavior.AllowGet);
        }
        // Start  Location Total Count End

        //Start create Inspectiondashboard -17-July-2019
        List<ScaffoldAllDashBoardViewModel> InspectionDashboardChart1 = new List<ScaffoldAllDashBoardViewModel> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod(EnableSession = true)]
        //public List<Scaffold> InspectionFirst(string id, string locationID, string siteid)
        public ActionResult InspectionFirst(string id, string locationID, string siteid)

        {

            //   string query = "Select 'Green' as Status, Sum(case when Status='Green' then 1 else 0 End)Counts from vw_TblRegister where ClientId='" + id + "' and  (SiteLocation = '" + siteid + "' or '" + siteid + "' = 'ALL') and (Location = '" + jobselect + "' or '" + jobselect + "' = 'ALL')union all Select 'Yellow' as Status, Sum(case when Status='Yellow' then 1 else 0 End)Counts from vw_TblRegister where ClientId='" + id + "' and  (SiteLocation = '" + siteid + "' or '" + siteid + "' = 'ALL') and (Location = '" + jobselect + "' or '" + jobselect + "' = 'ALL')union all Select 'Red' as Status, Sum(case when Status='Red' then 1 else 0 End)Counts from vw_TblRegister where ClientId='" + id + "' and  (SiteLocation = '" + siteid + "' or '" + siteid + "' = 'ALL') and (Location = '" + jobselect + "' or '" + jobselect + "' = 'ALL')union all Select 'Out of service' as Status, Sum(case when Status='Out of service' then 1 else 0 End) Counts from vw_TblRegister where ClientId='" + id + "' and  (SiteLocation = '" + siteid + "' or '" + siteid + "' = 'ALL') and(Location = '" + jobselect + "' or '" + jobselect + "' = 'ALL') ";
            string query = "Select 'Green' as Status, Sum(case when Status='Green' then 1 else 0 End)Counts from vw_TblRegister where ClientId='" + id + "' and  (SiteLocId = '" + siteid + "' or '" + siteid + "' = 'ALL') and (LocationID = '" + locationID + "' or '" + locationID + "' = 'ALL')union all Select 'Yellow' as Status, Sum(case when Status='Yellow' then 1 else 0 End)Counts from vw_TblRegister where ClientId='" + id + "' and  (SiteLocId = '" + siteid + "' or '" + siteid + "' = 'ALL') and (LocationID = '" + locationID + "' or '" + locationID + "' = 'ALL')union all Select 'Red' as Status, Sum(case when Status='Red' then 1 else 0 End)Counts from vw_TblRegister where ClientId='" + id + "' and  (SiteLocId = '" + siteid + "' or '" + siteid + "' = 'ALL') and (LocationID = '" + locationID + "' or '" + locationID + "' = 'ALL')union all Select 'Out of service' as Status, Sum(case when Status='Out of service' then 1 else 0 End) Counts from vw_TblRegister where ClientId='" + id + "' and  (SiteLocId = '" + siteid + "' or '" + siteid + "' = 'ALL') and(LocationID = '" + locationID + "' or '" + locationID + "' = 'ALL') union all Select 'Restricted Access' as Status, Sum(case when Status='Restricted Access' then 1 else 0 End) Counts from vw_TblRegister where ClientId='" + id + "' and  (SiteLocId = '" + siteid + "' or '" + siteid + "' = 'ALL') and(LocationID = '" + locationID + "' or '" + locationID + "' = 'ALL') ";



            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ScaffoldAllDashBoardViewModel Inspec = new ScaffoldAllDashBoardViewModel();
                    if (!(item["Status"] is DBNull) && item["Status"].ToString() != "")
                    {
                        Inspec.Status = item["Status"].ToString();
                    }
                    if (!(item["Counts"] is DBNull) && item["Counts"].ToString() != "")
                    {
                        Inspec.TotalCounts = Convert.ToDecimal(item["Counts"].ToString());
                    }


                    InspectionDashboardChart1.Add(Inspec);
                }
            }
            // return InspectionDashboardChart1;
            return Json(InspectionDashboardChart1, JsonRequestBehavior.AllowGet);
        }
        //End -17-July-2019



        public void ExportAllCategories(string id, string siteID, string ddlarea)
        {

            string allcategory = "select Right ('000000' + convert (varchar, [ScaffRef]),6) as ScaffRef,Replace(Replace(Description,char(13),' '), CHAR(10), ' ') as Description,Location,BuildingNo,AssetNo,FloorLevel,WONumber,ClientScaffref,DesignType,MaterialType, TotalLabourDismantleEdit,PriorityRiskRating,SiteLocation,DateTagged,DaysSinceLastInsp,RecentInspDate,HireDaysToDate as 'Standing Days' from VW_TblRegister where (SiteLocId = '" + siteID + "' or '" + siteID + "' = 'ALL') and  (LocationID = '" + ddlarea + "' or '" + ddlarea + "' = 'ALL') and ClientID = '" + id + "' and Status in ('Red' ,'Green','Out of Service','Yellow','Restricted Access') order by ScaffRef";
            DataTable dt = databaseCon.GetData(allcategory);

            Response.ContentType = "Application/x-msexcel";
            Response.AddHeader("content-disposition", "attachment;filename='All Categories' - " + DateTime.Now.Date.ToString("dd'-'MM'-'yyyy") + ".csv");
            Response.Write(csvfile(dt));
            Response.End();
        }




        private string csvfile(DataTable dtTable)
        {
            StringBuilder sbldr = new StringBuilder();
            if (dtTable.Columns.Count != 0)
            {
                foreach (DataColumn col in dtTable.Columns)
                {
                    sbldr.Append(col.ColumnName + ',');
                }
                sbldr.Append("\r\n");
                foreach (DataRow row in dtTable.Rows)
                {
                    foreach (DataColumn column in dtTable.Columns)
                    {
                        if (row[column].ToString().Contains(','))
                        {
                            sbldr.Append("\"" + row[column].ToString() + "\"" + ',');
                        }
                        else
                        {
                            sbldr.Append(row[column].ToString() + ',');
                        }
                        //sbldr.Append(row[column].ToString().Replace(",", ";") + ',');
                    }
                    sbldr.Append("\r\n");
                }
            }
            return sbldr.ToString();
        }
    }
}