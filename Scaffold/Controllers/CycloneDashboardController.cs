// we are used replace on 28-03-2025  for replace status!='Tie-Down' To  status!='Tied-Down' from the  every  query becasue Tied down’ or ‘De-planked’  The Preparation Hours is not reducing. 
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Office2010.Excel;
using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;

namespace Scaffold.Controllers
{
    public class CycloneDashboardController : Controller
    {
        ScaffoldEntities dbcontext = new ScaffoldEntities();

        // GET: CycloneDashboard
        public ActionResult CycloneDashboard()
        {
            return View();
        }

        public ActionResult BindallDropdown(string id)
        {

            var ddlsites = dbcontext.Database.SqlQuery<DropDownViewModel>("select SiteLocID,SiteLocation from tblsitelocation where ClientId='" + databaseCon.Decrypt(id) + "' Order by SiteLocation").ToList();

            var ddlScaffReff = dbcontext.Database.SqlQuery<DropDownViewModel>("select Right ('000000' + convert (varchar, [scaffref]),6) as ScaffRef,RegisterId from VW_tblRegister where ClientID = '" + databaseCon.Decrypt(id) + "'and (DateTagged<>'01/01/1900' and DateTagged is not null) and (DateOfDismantle='01/01/1900' or DateOfDismantle is null)");

            return Json(new { ddlsites = ddlsites, ddlScaffReff = ddlScaffReff }, JsonRequestBehavior.AllowGet);

        }
        /// Cyclone Lable Query Start
        List<ScaffoldAllDashBoardViewModel> AllHours = new List<ScaffoldAllDashBoardViewModel> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod(EnableSession = true)]
        // public List<Scaffold> ManandPrepHrs(string id, string SiteId)
        public ActionResult ManandPrepHrs(string id, string ddlselectarea, string SiteId,string ddlRiskRating)
        {
            //Change By Vishal 19/11/2018
            //string qry = "Select Sum(isnull(DismantleHrs, 0)) as TotalPrepHrs,(select isnull(Sum(isnull(ManHoursAvailable, 0)),0) from tblSiteLocation where tblSiteLocation.ClientId = '" + id + "'  and ( SiteLocId='" + SiteId + "' or 'All'='" + SiteId + "')) as ManHoursAvailable , Case when (select Sum(isnull(ManHoursAvailable, 0)) from tblSiteLocation where tblSiteLocation.ClientId ='" + id + "'  and ( SiteLocId='" + SiteId + "' or 'All'='" + SiteId + "'))< Sum(isnull(DismantleHrs, 0)) then 'Red' else 'Green' end as Status from (select 'De-Plank Only' as CycloneAction, isnull(Sum(case when CycloneAction = 'De-plank Only' then case when UseEnteredValue=1 then TotalLabourDismantleEdit else TotalLabourDismantle end end), 0) as DismantleHrs  from VW_TblRegister where ClientID ='" + id + "'  and (SiteLocId='" + SiteId + "' or 'All'='" + SiteId + "') and('" + ddlRiskRating + "' = 'All'  or PriorityRiskRating = '" + ddlRiskRating + "') and (DateTagged is not null  and DateTagged != '01/01/1900' and status !='De-Planked' and status!='Tied-Down' and(DateOfDismantle is null or DateOfDismantle = '01/01/1900'))  union all select 'Full Strip' as CycloneAction, isnull(Sum(case when CycloneAction = 'Full Strip' then ManHrsDismantle end), 0) as DismantleHrs from VW_TblRegister where ClientID = '" + id + "' and ( SiteLocId='" + SiteId + "' or 'All'='" + SiteId + "') and('" + ddlRiskRating + "' = 'All'  or PriorityRiskRating = '" + ddlRiskRating + "') and (DateTagged is not null  and DateTagged!='01/01/1900' and status !='De-Planked' and status!='Tied-Down' and (DateOfDismantle is null or DateOfDismantle='01/01/1900')) union all select 'Tie-Down' as CycloneAction, isnull(Sum(case when CycloneAction = 'Tie-Down' then case when UseEnteredValue = 1 then TotalLabourDismantleEdit else TotalLabourDismantle end end), 0) as DismantleHrs from VW_TblRegister  where ClientID = '" + id + "' and(SiteLocId = '" + SiteId + "' or 'All' = 'All')and(DateTagged is not null  and DateTagged != '01/01/1900' and status !='De-Planked' and status!='Tied-Down' and('" + ddlRiskRating + "' = 'All'  or PriorityRiskRating = '" + ddlRiskRating + "') and(DateOfDismantle is null or DateOfDismantle = '01/01/1900' ))) UnionQuery ";
            string FunctionName = "ManandPrepHrs";
            string qry = "exec Pro_CycloneDashboard '" + id + "', '" + SiteId + "', '"+ ddlselectarea + "','', '" + ddlRiskRating + "','', '" + FunctionName + "'";
            // string qry = "Select Sum(isnull(DismantleHrs, 0)) as TotalPrepHrs, (select Sum(isnull(ManHoursAvailable, 0)) from tblSiteLocation where tblSiteLocation.ClientId = " + databaseCon.Decrypt(Request.QueryString["id"]) + "  and ( SiteLocId='" + SiteId + "' or 'All'='" + SiteId + "')) as ManHoursAvailable , Case when (select Sum(isnull(ManHoursAvailable, 0)) from tblSiteLocation where tblSiteLocation.ClientId =" + databaseCon.Decrypt(Request.QueryString["id"]) + "  and ( SiteLocId='" + SiteId + "' or 'All'='" + SiteId + "'))< Sum(isnull(DismantleHrs, 0)) then 'Red' else 'Green' end as Status from (select 'De-Plank Only' as CycloneAction, isnull(Sum(case when CycloneAction = 'De-plank Only' then case when UseEnteredValue=1 then TotalLabourDismantleEdit else TotalLabourDismantle end end), 0) as DismantleHrs  from VW_TblRegister where ClientID =" + databaseCon.Decrypt(Request.QueryString["id"]) + "  and ( SiteLocId='" + siteid + "' or 'All'='" + SiteId + "')  and (DateTagged is not null  and DateTagged != '01/01/1900' and(DateOfDismantle is null or DateOfDismantle = '01/01/1900'))  union all select 'Full Strip' as CycloneAction, isnull(Sum(case when CycloneAction = 'Full Strip' then ManHrsDismantle end), 0) as DismantleHrs from VW_TblRegister where ClientID = " + databaseCon.Decrypt(Request.QueryString["id"]) + " and(SiteLocId = '" + SiteId + "' or 'All' = '" + SiteId + "') ) UnionQuery ";

            DataSet ds = null;
            ds = databaseCon.GetDataSet(qry);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ScaffoldAllDashBoardViewModel pro = new ScaffoldAllDashBoardViewModel();
                    if (!(item["ManHoursAvailable"] is DBNull) && item["ManHoursAvailable"].ToString() != "")
                    {
                        pro.Mahhrsavail = item["ManHoursAvailable"].ToString();
                    }
                    if (!(item["TotalPrepHrs"] is DBNull) && item["TotalPrepHrs"].ToString() != "")
                    {
                        pro.totalprephrs = item["TotalPrepHrs"].ToString();
                    }
                    if (!(item["Status"] is DBNull) && item["Status"].ToString() != "")
                    {
                        pro.ColorStatus = item["Status"].ToString();
                    }
                    AllHours.Add(pro);
                }
            }
            // return AllHours;
            return Json(AllHours, JsonRequestBehavior.AllowGet);
        }
        /// Cyclone Lable Query End


        /// Cyclone Dashboard, Weeklyprogress Dashboard and SiteAnalysis Dashboard Location Dropdown Start

        List<ScaffoldAllDashBoardViewModel> ddlJob = new List<ScaffoldAllDashBoardViewModel> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod(EnableSession = true)]
        // public List<Scaffold> ddlJobDashboard(string id, string SiteId)
        public ActionResult ddlJobDashboard(string id, string SiteId)
        {
            string query = "select distinct Location,LocationId from VW_TblRegister where (Tonnage is not null or ManHrsDismantle is not null) and (SiteLocId='" + SiteId + "' or 'All'='" + SiteId + "') and ClientID='" + id + "' Order by Location";

            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ScaffoldAllDashBoardViewModel pro = new ScaffoldAllDashBoardViewModel();
                    if (!(item["Location"] is DBNull) && item["Location"].ToString() != "")
                    {
                        pro.SiteLoca = item["Location"].ToString();
                    }
                    if (!(item["LocationId"] is DBNull) && item["LocationId"].ToString() != "")
                    {
                        pro.SiteLocID = item["LocationId"].ToString();
                    }

                    ddlJob.Add(pro);
                }
            }
            // return ddlJob;
            return Json(ddlJob, JsonRequestBehavior.AllowGet);
        }
        ///Cyclone Dashboard, Weeklyprogress Dashboard and SiteAnalysis Dashboard Location Dropdown End

        /// Cyclone Dashboard First Chart Bar click query Start

        List<ScaffoldAllDashBoardViewModel> CycloneDashboardClick = new List<ScaffoldAllDashBoardViewModel> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod(EnableSession = true)]
        // public List<Scaffold> CycloneActionGetByCategory(string CycloneActionName, string ClientID, string siteid, string jobselect)
        public ActionResult CycloneActionGetByCategory(string CycloneActionName, string ClientID, string siteid, string jobselect,string ddlRiskRating)
        {
            string ConditionAddNoAction = "";
            if (CycloneActionName == "No Action")
            {
                ConditionAddNoAction = "and (CycloneAction='" + CycloneActionName + "' or CycloneAction is null) ";
            }
            else
            {
                ConditionAddNoAction = "and CycloneAction='" + CycloneActionName + "'";
            }
            // Change on 25 March 2020
            // string query = "select Right ('000000' + convert (varchar, [ScaffRef]),6) as ScaffRef,Description,Location,BuildingNo,AssetNo,FloorLevel,DesignType,MaterialType, TotalLabourDismantleEdit,PriorityRiskRating from VW_TblRegister where clientid='" + ClientID + "' and DateOfDismantle is NULL and(DateTagged <> '01/01/1900' and DateTagged is not null) and('" + siteid + "' = 'All'  or SiteLocId = '" + siteid + "') and ('" + jobselect + "' = 'All'  or LocationId = '" + jobselect + "') " + ConditionAddNoAction + " Order by Location,BuildingNo";
            //string query = "select Right ('000000' + convert (varchar, [ScaffRef]),6) as ScaffRef,Description,Location,BuildingNo,AssetNo,FloorLevel,DesignType,MaterialType,case when CycloneAction = 'Full Strip' then ManHrsDismantle When CycloneAction <> 'Full Strip' and UseEnteredValue = 1 then TotalLabourDismantleEdit else TotalLabourDismantle End TotalLabourDismantleEdit,PriorityRiskRating from VW_TblRegister where clientid='" + ClientID + "' and DateOfDismantle is NULL and(DateTagged <> '01/01/1900' and DateTagged is not null and status !='De-Planked' and status!='Tied-Down') and('" + siteid + "' = 'All'  or SiteLocId = '" + siteid + "') and('" + ddlRiskRating + "' = 'All'  or PriorityRiskRating = '" + ddlRiskRating + "') and ('" + jobselect + "' = 'All'  or LocationId = '" + jobselect + "') " + ConditionAddNoAction + " Order by Location,BuildingNo";
            string FunctionName = "CycloneActionGetByCategory";
            string Registerid = "";
            string query = "exec Pro_CycloneDashboard '" + ClientID + "', '" + siteid + "', '" + jobselect + "','" + Registerid + "', '" + ddlRiskRating + "','" + CycloneActionName + "', '" + FunctionName + "'";

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

                    CycloneDashboardClick.Add(pro);
                }
            }
            // return CycloneDashboardClick;
            return Json(CycloneDashboardClick, JsonRequestBehavior.AllowGet);
        }
        /// Cyclone Dashboard First Chart Bar click query End

        /// Cyclone Dashboard First Chart Query Start
        List<ScaffoldAllDashBoardViewModel> CycloneDashboardChart1 = new List<ScaffoldAllDashBoardViewModel> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod(EnableSession = true)]
        // public List<Scaffold> CycloneFirst(string id, string jobselect, string siteid)
        public ActionResult CycloneFirst(string id, string jobselect, string siteid,string ddlRiskRating)
        {

            //change on 15 Nov
            //string query = "select 'No Action' as CycloneAction ,COUNT(case when CycloneAction = 'No Action' then RegisterId  else case when isnull(CycloneAction,'') = '' then RegisterId end end ) as Counts from  VW_TblRegister where ClientID = '" + id + "'  and DateOfDismantle is NULL and(DateTagged <> '01/01/1900' and DateTagged is not null) and('" + siteid + "' = 'All'  or  SiteLocId = '" + siteid + "') and ('" + jobselect + "' = 'All'  or LocationId = '" + jobselect + "') union all select 'De-plank Only' as  CycloneAction ,COUNT(case when CycloneAction = 'De-plank Only' then RegisterId end) as Counts from VW_TblRegister where ClientID = '" + id + "'  and DateOfDismantle is NULL and(DateTagged <> '01/01/1900' and DateTagged is not null) and('" + siteid + "' = 'All'  or SiteLocId = '" + siteid + "') and ('" + jobselect + "' = 'All'  or LocationId = '" + jobselect + "') union all select 'Full Strip' as CycloneAction ,COUNT(case when CycloneAction = 'Full Strip' then RegisterId end ) as Counts from VW_TblRegister where ClientID='" + id + "'  and DateOfDismantle is NULL and(DateTagged <> '01/01/1900' and DateTagged is not null) and ('" + siteid + "' = 'All'  or SiteLocId = '" + siteid + "') and ('" + jobselect + "' = 'All'  or LocationId = '" + jobselect + "')";

            //Changed By Vishal 19/11/2018
            //string query = "select 'No Action' as CycloneAction ,COUNT(case when CycloneAction = 'No Action' then RegisterId  else case when isnull(CycloneAction,'') = '' then RegisterId end end ) as Counts from  VW_TblRegister where ClientID = '" + id + "'  and DateOfDismantle is NULL and(DateTagged <> '01/01/1900' and DateTagged is not null and status !='De-Planked' and status!='Tied-Down') and('" + siteid + "' = 'All'  or  SiteLocId = '" + siteid + "') and('" + ddlRiskRating + "' = 'All'  or PriorityRiskRating = '" + ddlRiskRating + "')and ('" + jobselect + "' = 'All'  or LocationId = '" + jobselect + "') union all select 'De-plank Only' as  CycloneAction ,COUNT(case when CycloneAction = 'De-plank Only' then RegisterId end) as Counts from VW_TblRegister where ClientID = '" + id + "'  and DateOfDismantle is NULL and(DateTagged <> '01/01/1900' and DateTagged is not null and status !='De-Planked' and status!='Tied-Down') and('" + siteid + "' = 'All'  or SiteLocId = '" + siteid + "') and('" + ddlRiskRating + "' = 'All'  or PriorityRiskRating = '" + ddlRiskRating + "') and ('" + jobselect + "' = 'All'  or LocationId = '" + jobselect + "') union all select 'Tie-Down' as CycloneAction ,COUNT(case when CycloneAction = 'Tie-Down' then RegisterId end ) as Counts from VW_TblRegister where ClientID='" + id + "'  and DateOfDismantle is NULL and(DateTagged <> '01/01/1900' and DateTagged is not null and status !='De-Planked' and status!='Tie-Down') and ('" + siteid + "' = 'All'  or SiteLocId = '" + siteid + "') and('" + ddlRiskRating + "' = 'All'  or PriorityRiskRating = '" + ddlRiskRating + "') and ('" + jobselect + "' = 'All'  or LocationId = '" + jobselect + "')union all select 'Full Strip' as CycloneAction ,COUNT(case when CycloneAction = 'Full Strip' then RegisterId end) as Counts from VW_TblRegister where ClientID = '" + id + "' and DateOfDismantle is NULL and(DateTagged <> '01/01/1900' and DateTagged is not null and status !='De-Planked' and status!='Tied-Down') and('" + siteid + "' = 'All'  or SiteLocId = '" + siteid + "') and('" + jobselect + "' = 'All'  or LocationId = '" + jobselect + "')and('" + ddlRiskRating + "' = 'All'  or PriorityRiskRating = '" + ddlRiskRating + "')";
            string FunctionName = "CycloneFirst";
            string Registerid = "";
            string CycloneActionName = "";
            string query = "exec Pro_CycloneDashboard '" + id + "', '" + siteid + "', '" + jobselect + "','" + Registerid + "', '" + ddlRiskRating + "','" + CycloneActionName + "', '" + FunctionName + "'";
            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ScaffoldAllDashBoardViewModel pro = new ScaffoldAllDashBoardViewModel();
                    if (!(item["CycloneAction"] is DBNull) && item["CycloneAction"].ToString() != "")
                    {
                        pro.CycloneAction = item["CycloneAction"].ToString();
                    }
                    if (!(item["Counts"] is DBNull) && item["Counts"].ToString() != "")
                    {
                        pro.TotalCounts = Convert.ToDecimal(item["Counts"].ToString());
                    }


                    CycloneDashboardChart1.Add(pro);
                }
            }
            // return CycloneDashboardChart1;
            return Json(CycloneDashboardChart1, JsonRequestBehavior.AllowGet);
        }
        /// Cyclone Dashboard First Chart Query End

        /// cyclone Dashboard Second Chart Query Start
        List<ScaffoldAllDashBoardViewModel> CycloneDashboardChart2 = new List<ScaffoldAllDashBoardViewModel> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod(EnableSession = true)]
        //public List<Scaffold> CycloneSecond(string id, string jobselect, string siteid)
        public ActionResult CycloneSecond(string id, string jobselect, string siteid,string ddlRiskRating)
        {
            //change on 15 Nov
            //string query = "select 'De-Plank Only' as CycloneAction ,isnull(Sum(case when CycloneAction = 'De-plank Only' then case when UseEnteredValue = 1 then TotalLabourDismantleEdit else TotalLabourDismantle end end), 0) as DismantleHrs  from VW_TblRegister where ClientID = '" + id + "' and DateOfDismantle is NULL and(DateTagged <> '01/01/1900' and DateTagged is not null)   and ('" + siteid + "' = 'All'  or SiteLocId = '" + siteid + "') and ('" + jobselect + "' = 'All'  or LocationId = '" + jobselect + "') union all select 'Full Strip' as CycloneAction ,isnull(Sum(case when CycloneAction = 'Full Strip' then ManHrsDismantle end), 0) as DismantleHrs from VW_TblRegister where ClientID = '" + id + "' and DateOfDismantle is NULL and(DateTagged <> '01/01/1900' and DateTagged is not null) and ('" + siteid + "' = 'All'  or SiteLocId = '" + siteid + "') and ('" + jobselect + "' = 'All'  or LocationId = '" + jobselect + "')";

            //Changed By Vishal 19/11/2018
            //string query = "select 'De-Plank Only' as CycloneAction ,isnull(Sum(case when CycloneAction = 'De-plank Only' then case when UseEnteredValue = 1 then TotalLabourDismantleEdit else TotalLabourDismantle end end), 0) as DismantleHrs  from VW_TblRegister where ClientID = '" + id + "' and DateOfDismantle is NULL and(DateTagged <> '01/01/1900' and DateTagged is not null and status !='De-Planked' and status!='Tied-Down')   and ('" + siteid + "' = 'All'  or SiteLocId = '" + siteid + "') and('" + ddlRiskRating + "' = 'All'  or PriorityRiskRating = '" + ddlRiskRating + "') and ('" + jobselect + "' = 'All'  or LocationId = '" + jobselect + "') union all select 'Tie-Down' as CycloneAction ,isnull(Sum(case when CycloneAction = 'Tie-Down' then case when UseEnteredValue = 1 then TotalLabourDismantleEdit else TotalLabourDismantle end end), 0) as DismantleHrs from VW_TblRegister  where ClientID = '" + id + "' and DateOfDismantle is NULL and(DateTagged <> '01/01/1900' and DateTagged is not null and status !='De-Planked' and status!='Tied-Down') and ('" + siteid + "' = 'All'  or SiteLocId = '" + siteid + "') and('" + ddlRiskRating + "' = 'All'  or PriorityRiskRating = '" + ddlRiskRating + "') and ('" + jobselect + "' = 'All'  or LocationId = '" + jobselect + "')union all select 'Full Strip' as CycloneAction , isnull(Sum( case when CycloneAction = 'Full Strip' then ManHrsDismantle end), 0) as DismantleHrs from VW_TblRegister where ClientID = '" + id + "' and DateOfDismantle is NULL and(DateTagged <> '01/01/1900' and DateTagged is not null and status !='De-Planked' and status!='Tied-Down') and('" + siteid + "' = 'All'  or SiteLocId = '" + siteid + "') and('" + jobselect + "' = 'All'  or LocationId = '" + jobselect + "') and('" + ddlRiskRating + "' = 'All'  or PriorityRiskRating = '" + ddlRiskRating + "')";
            string FunctionName = "CycloneSecond";
            string Registerid = "";
            string CycloneActionName = "";
            string query = "exec Pro_CycloneDashboard '" + id + "', '" + siteid + "', '" + jobselect + "','" + Registerid + "', '" + ddlRiskRating + "','" + CycloneActionName + "', '" + FunctionName + "'";

            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ScaffoldAllDashBoardViewModel pro = new ScaffoldAllDashBoardViewModel();
                    if (!(item["CycloneAction"] is DBNull) && item["CycloneAction"].ToString() != "")
                    {
                        pro.CycloneActionHr = item["CycloneAction"].ToString();
                    }
                    if (!(item["DismantleHrs"] is DBNull) && item["DismantleHrs"].ToString() != "")
                    {
                        pro.TotalCountsDismantle = Convert.ToDecimal(item["DismantleHrs"].ToString());
                    }
                    CycloneDashboardChart2.Add(pro);
                }
            }
            // return CycloneDashboardChart2;
            return Json(CycloneDashboardChart2, JsonRequestBehavior.AllowGet);
        }
        /// cyclone Dashboard Second Chart Query End

        /// cyclone Dashboard 3 Chart Query Tonnage Start 

        List<ScaffoldAllDashBoardViewModel> CycloneDashboardData = new List<ScaffoldAllDashBoardViewModel> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod(EnableSession = true)]
        // public List<Scaffold> CycloneQty(string id, string Lid, string siteid)
        public ActionResult CycloneQty(string id, string Lid, string siteid, string ddlRiskRating)
        {
            //string query = "select Right ('000000' + convert (varchar, [scaffref]),6) as ScaffRef,Convert(Decimal(18,2),(Tonnage/1000 ))Tonnage,Convert(Decimal(18,2),Tonnage/TotTonnage) Pct , case when CycloneAction = 'De-plank Only' then '#f9cb13' else case when CycloneAction = 'No Action' then '#4c9640' else case when CycloneAction = 'Full Strip' then '#fc2828' else '#4c9640' end end end as CycloneColor from VW_TblRegister  ,(select Convert(Decimal(18, 2), (Sum(Tonnage))) as TotTonnage from VW_TblRegister where ClientID = " + id + " and('" + Lid + "'  = 'All'  or LocationId = '" + Lid + "' ) and('" + siteid + "' = 'All'  or SiteLocId = '" + siteid + "'))def where ClientID = " + id + " and('" + Lid + "' = 'All'  or LocationId = '" + Lid + "') and('" + siteid + "' = 'All'  or SiteLocId = '" + siteid + "') order by Tonnage desc";
            //string query = "select Right ('000000' + convert (varchar, [scaffref]),6) as ScaffRef,Convert(Decimal(18,2),(Tonnage/1000 ))Tonnage,case when TotTonnage=0.00 then 0 else Convert(Decimal(18,2),Tonnage/TotTonnage) end as Pct , case when CycloneAction = 'De-plank Only' then '#f9cb13' else case when CycloneAction = 'No Action' then '#4c9640' else case when CycloneAction = 'Full Strip' then '#fc2828' else '#4c9640' end end end as CycloneColor from VW_TblRegister  ,(select Convert(Decimal(18, 2), (Sum(Tonnage))) as TotTonnage from VW_TblRegister where ClientID = '" + id + "' and DateOfDismantle is NULL and(DateTagged <> '01/01/1900' and DateTagged is not null)  and('All'  = 'All'  or LocationId = 'All' ) and('" + siteid + "' = 'All'  or SiteLocId = '" + siteid + "'))def where ClientID = " + id + " and DateOfDismantle is NULL and(DateTagged <> '01/01/1900' and DateTagged is not null)  and('" + Lid + "' = 'All'  or LocationId = '" + Lid + "') and('" + siteid + "' = 'All'  or SiteLocId = '" + siteid + "') order by ScaffRef";

            //Change By Vishal 19/11/2018
            //string query = "select Right ('000000' + convert (varchar, [scaffref]),6) as ScaffRef,Convert(Decimal(18,2),(Tonnage/1000 ))Tonnage,case when TotTonnage=0.00 then 0 else Convert(Decimal(18,2),Tonnage/TotTonnage) end as Pct , case when CycloneAction = 'De-plank Only' then '#f9cb13' else case when CycloneAction = 'No Action' then '#4c9640' else case when CycloneAction = 'Full Strip' then '#fc2828' else case when CycloneAction = 'Tie-Down' then '#0000ff'  else '#4c9640' end end end end as CycloneColor from VW_TblRegister  ,(select Convert(Decimal(18, 2), (Sum(Tonnage))) as TotTonnage from VW_TblRegister where ClientID = '" + id + "' and DateOfDismantle is NULL and(DateTagged <> '01/01/1900' and DateTagged is not null and status !='De-Planked' and status!='Tied-Down')  and('All'  = 'All'  or LocationId = 'All' ) and('" + siteid + "' = 'All'  or SiteLocId = '" + siteid + "') and('" + ddlRiskRating + "' = 'All'  or PriorityRiskRating = '" + ddlRiskRating + "'))def where ClientID = " + id + " and DateOfDismantle is NULL and(DateTagged <> '01/01/1900' and DateTagged is not null and status !='De-Planked' and status!='Tied-Down')  and('" + Lid + "' = 'All'  or LocationId = '" + Lid + "') and('" + siteid + "' = 'All'  or SiteLocId = '" + siteid + "') and('" + ddlRiskRating + "' = 'All'  or PriorityRiskRating = '" + ddlRiskRating + "') order by ScaffRef";
            string FunctionName = "CycloneQty";
            string Registerid = "";
            string CycloneActionName = "";
            string query = "exec Pro_CycloneDashboard '" + id + "', '" + siteid + "', '" + Lid + "','" + Registerid + "', '" + ddlRiskRating + "','" + CycloneActionName + "', '" + FunctionName + "'";
            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ScaffoldAllDashBoardViewModel pro = new ScaffoldAllDashBoardViewModel();
                    if (!(item["ScaffRef"] is DBNull) && item["ScaffRef"].ToString() != "")
                    {
                        pro.Cyclone3Scafref = item["ScaffRef"].ToString();
                    }
                    if (!(item["Tonnage"] is DBNull) && item["Tonnage"].ToString() != "")
                    {
                        pro.Cyclone3Tonnage = Convert.ToDecimal(item["Tonnage"].ToString());
                    }
                    if (!(item["CycloneColor"] is DBNull) && item["CycloneColor"].ToString() != "")
                    {
                        pro.CycloneColor = item["CycloneColor"].ToString();
                    }
                    if (!(item["Pct"] is DBNull) && item["Pct"].ToString() != "")
                    {
                        pro.PctTonnage = Convert.ToDecimal(item["Pct"]);
                    }

                    CycloneDashboardData.Add(pro);
                }
            }
            // return CycloneDashboard;
            return Json(CycloneDashboardData, JsonRequestBehavior.AllowGet);
        }
        /// cyclone Dashboard 3 Chart Query Tonnage End

        /// cyclone Dashboard 4 Chart Query Man Hours Srart

        List<ScaffoldAllDashBoardViewModel> CycloneDashboard1 = new List<ScaffoldAllDashBoardViewModel> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod(EnableSession = true)]
        //public List<Scaffold> CycloneQty1(string id, string Lid, string siteid)
        public ActionResult CycloneQty1(string id, string Lid, string siteid,string ddlRiskRating)
        {

            //string query = "select Right ('000000' + convert (varchar, [scaffref]),6) as ScaffRef,ManHrsDismantle,case when CycloneAction='De-plank Only' then '#f9cb13' else case when CycloneAction = 'No Action' then '#4c9640' else case when CycloneAction = 'Full Strip' then '#fc2828' else '#4c9640' end end end as CycloneColor from VW_TblRegister where ClientID='" + id + "' and DateOfDismantle is NULL and(DateTagged <> '01/01/1900' and DateTagged is not null)  and ('" + Lid + "' = 'All'  or LocationId = '" + Lid + "') and ('" + siteid + "' = 'All'  or SiteLocId = '" + siteid + "') order by ScaffRef";

            ////Change By Vishal 19/11/2018
            //string query = "select Right ('000000' + convert (varchar, [scaffref]),6) as ScaffRef,ManHrsDismantle,case when CycloneAction='De-plank Only' then '#f9cb13' else case when CycloneAction = 'No Action' then '#4c9640' else case when CycloneAction = 'Full Strip' then '#fc2828' else case when CycloneAction='Tie-Down' then '#0000ff'  else '#4c9640' end end end end as CycloneColor from VW_TblRegister where ClientID='" + id + "' and DateOfDismantle is NULL and(DateTagged <> '01/01/1900' and DateTagged is not null)  and ('" + Lid + "' = 'All'  or LocationId = '" + Lid + "') and ('" + siteid + "' = 'All'  or SiteLocId = '" + siteid + "') and ('" + ddlRiskRating + "' = 'All'  or PriorityRiskRating = '" + ddlRiskRating + "') order by ScaffRef";
            string FunctionName = "CycloneQty1";
            string Registerid = "";
            string CycloneActionName = "";
            string query = "exec Pro_CycloneDashboard '" + id + "', '" + siteid + "', '" + Lid + "','" + Registerid + "', '" + ddlRiskRating + "','" + CycloneActionName + "', '" + FunctionName + "'";
            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ScaffoldAllDashBoardViewModel pro = new ScaffoldAllDashBoardViewModel();
                    if (!(item["ScaffRef"] is DBNull) && item["ScaffRef"].ToString() != "")
                    {
                        pro.Cyclone3Scafref = item["ScaffRef"].ToString();
                    }
                    if (!(item["ManHrsDismantle"] is DBNull) && item["ManHrsDismantle"].ToString() != "")
                    {
                        pro.Cyclone3Manhrs = Convert.ToDecimal(item["ManHrsDismantle"].ToString());
                    }
                    if (!(item["CycloneColor"] is DBNull) && item["CycloneColor"].ToString() != "")
                    {
                        pro.CycloneColor = item["CycloneColor"].ToString();
                    }

                    CycloneDashboard1.Add(pro);
                }
            }
            // return CycloneDashboard1;
            return Json(CycloneDashboard1, JsonRequestBehavior.AllowGet);
        }
        /// cyclone Dashboard 4 Chart Query Man Hours End


        ///cyclone Dashboard 5 Chart Query Start but display none

        List<ScaffoldAllDashBoardViewModel> CycloneDashboard5 = new List<ScaffoldAllDashBoardViewModel> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod(EnableSession = true)]
        //public List<Scaffold> CycloneChart5(string id, string rid)
        public ActionResult CycloneChart5(string id, string rid)
        {
            //string qry = "select vw_manhr.RegisterId,vw_manhr.ScaffRef as Scafpac,isnull( ManHrsDismantle,0)ManHrsDismantle,isnull(Tonnage,0)Tonnage,vw_manhr.CycloneColor as color from(select Right('000000' + convert(varchar, [scaffref]), 6) as ScaffRef,RegisterId, ManHrsDismantle,Convert(Decimal(18, 2), (Tonnage / 1000))Tonnage,case when CycloneAction = 'De-plank Only' then '#f9cb13' else case when CycloneAction = 'No Action' then '#4c9640' else case when CycloneAction = 'Full Strip' then '#fc2828' else '#4c9640' end end end as CycloneColor from VW_TblRegister where ClientID = " + id + " and DateOfDismantle is NULL and(DateTagged <> '01/01/1900' and DateTagged is not null))vw_manhr where ('All' = '" + rid + "'  or vw_manhr.RegisterId = '" + rid + "')";

            //Change By Vishal 19/11/2018
            //string qry = "select vw_manhr.RegisterId,vw_manhr.ScaffRef as Scafpac,isnull( ManHrsDismantle,0)ManHrsDismantle,isnull(Tonnage,0)Tonnage,vw_manhr.CycloneColor as color from(select Right('000000' + convert(varchar, [scaffref]), 6) as ScaffRef,RegisterId, ManHrsDismantle,Convert(Decimal(18, 2), (Tonnage / 1000))Tonnage,case when CycloneAction = 'De-plank Only' then '#f9cb13' else case when CycloneAction = 'No Action' then '#4c9640' else case when CycloneAction = 'Full Strip' then '#fc2828' else case when CycloneAction = 'Tie-Down' then '#0000ff'  else '#4c9640' end end end end as CycloneColor from VW_TblRegister where ClientID = " + id + " and DateOfDismantle is NULL and(DateTagged <> '01/01/1900' and DateTagged is not null and status !='De-Planked' and status!='Tied-Down'))vw_manhr where ('All' = '" + rid + "'  or vw_manhr.RegisterId = '" + rid + "')";
            string FunctionName = "CycloneChart5";
            string qry = "exec Pro_CycloneDashboard '" + id + "', '', '','" + rid + "', '','', '" + FunctionName + "'";

            DataSet ds = null;
            ds = databaseCon.GetDataSet(qry);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ScaffoldAllDashBoardViewModel pro = new ScaffoldAllDashBoardViewModel();
                    if (!(item["Scafpac"] is DBNull) && item["Scafpac"].ToString() != "")
                    {
                        pro.Scafpac = item["Scafpac"].ToString();
                    }
                    if (!(item["ManHrsDismantle"] is DBNull) && item["ManHrsDismantle"].ToString() != "")
                    {
                        pro.ManHrs5 = Convert.ToDecimal(item["ManHrsDismantle"]);
                    }
                    if (!(item["Tonnage"] is DBNull) && item["Tonnage"].ToString() != "")
                    {
                        pro.Tonnage5 = Convert.ToDecimal(item["Tonnage"].ToString());
                    }
                    if (!(item["color"] is DBNull) && item["color"].ToString() != "")
                    {
                        pro.color5 = item["color"].ToString();
                    }
                    CycloneDashboard5.Add(pro);
                }
            }
            // return CycloneDashboard5;
            return Json(CycloneDashboard5, JsonRequestBehavior.AllowGet);
        }
        ///cyclone Dashboard 5 Chart Query End but display none

        public ActionResult ScaffoldsDePlanked(string id, string jobselect, string siteid, string ddlRiskRating)
        {
            //String query = "select 'De-planked' as  CycloneAction ,COUNT(case when CycloneAction = 'De-plank Only' then RegisterId end) as Counts from VW_TblRegister where  ClientID ='" + id + "'  and DateOfDismantle is NULL and (DateTagged <> '01/01/1900' and DateTagged is not null) and ('" + siteid + "' = 'All'  or SiteLocId = '" + siteid + "')  and('" + jobselect + "' = 'All'  or LocationId = '" + jobselect + "')and (Status = 'De-Planked' or Status = 'Tie-Down') and(CycloneAction = 'De-Plank Only' or CycloneAction = 'Tie-Down')union all select 'Tied-Down' as CycloneAction ,COUNT(case when CycloneAction = 'Tie-Down' then RegisterId end) as Counts from VW_TblRegister where  ClientID ='" + id + "'  and DateOfDismantle is NULL and (DateTagged <> '01/01/1900' and DateTagged is not null) and ('" + siteid + "' = 'All'  or SiteLocId = '" + siteid + "') and('" + jobselect + "' = 'All'  or LocationId = '" + jobselect + "') and(Status = 'De-Planked' or Status = 'Tie-Down')and(CycloneAction = 'De-Plank Only' or CycloneAction = 'Tie-Down')";
            //String query = "select 'De-Planked' as  CycloneAction ,COUNT(case when Status = 'De-Planked' then RegisterId  end ) as Counts from VW_TblRegister where  ClientID ='" + id + "'  and DateOfDismantle is NULL and (DateTagged <> '01/01/1900' and DateTagged is not null) and ('" + siteid + "' = 'All'  or SiteLocId = '" + siteid + "') and('" + ddlRiskRating + "' = 'All'  or PriorityRiskRating = '" + ddlRiskRating + "') and('" + jobselect + "' = 'All'  or LocationId = '" + jobselect + "') and Status = 'De-Planked'  union all select 'Tied-Down' as CycloneAction ,COUNT(case when Status = 'Tied-Down' then RegisterId end ) as Counts from VW_TblRegister where  ClientID ='" + id + "'  and DateOfDismantle is NULL and (DateTagged <> '01/01/1900' and DateTagged is not null) and ('" + siteid + "' = 'All'  or SiteLocId = '" + siteid + "') and('" + ddlRiskRating + "' = 'All'  or PriorityRiskRating = '" + ddlRiskRating + "') and('" + jobselect + "' = 'All'  or LocationId = '" + jobselect + "') and Status = 'Tied-Down'";
            string FunctionName = "ScaffoldsDePlanked";
            string Registerid = "";
            string CycloneActionName = "";
            string query = "exec Pro_CycloneDashboard '" + id + "', '" + siteid + "', '" + jobselect + "','" + Registerid + "', '" + ddlRiskRating + "','" + CycloneActionName + "', '" + FunctionName + "'";

            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ScaffoldAllDashBoardViewModel pro = new ScaffoldAllDashBoardViewModel();
                    if (!(item["CycloneAction"] is DBNull) && item["CycloneAction"].ToString() != "")
                    {
                        pro.CycloneActionHr = item["CycloneAction"].ToString();
                    }
                    if (!(item["Counts"] is DBNull) && item["Counts"].ToString() != "")
                    {
                        pro.TotalCountsDismantle = Convert.ToDecimal(item["Counts"].ToString());
                    }
                    CycloneDashboardChart2.Add(pro);
                }
            }
            // return CycloneDashboardChart2;
            return Json(CycloneDashboardChart2, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ScaffoldsTieDown(string id, string jobselect, string siteid,string ddlRiskRating)
        {
            //   String query = "select 'Tie-Down' as CycloneAction ,isnull(Sum(case when CycloneAction = 'Tie-Down' then case when UseEnteredValue = 1 then TotalLabourDismantleEdit else TotalLabourDismantle end end), 0) as DismantleHrs from VW_TblRegister  where ClientID ='" + id + "' and DateOfDismantle != '01/01/1900' and DateOfDismantle is Not Null and status != 'De-Planked' and ('" + siteid + "' =  'All'  or SiteLocId ='" + siteid + "') and('" + jobselect + "' = 'All'  or LocationId = '" + jobselect + "') and ('" + SubOwner.Replace("'", "''") + "' = 'All'  or Case when ScaffoldOwner = 'C&I' then 'C&I' when ScaffoldOwner = 'Shutdowns' then 'Shutdowns' else 'Maintenance' End =  '" + SubOwner.Replace("'", "''") + "')";
            //String query = "select 'De-planked' as CycloneAction ,isnull(Sum(case when CycloneAction = 'De-Plank Only' then case when UseEnteredValue = 1 then TotalLabourDismantleEdit else TotalLabourDismantle end end), 0) as DismantleHrs from VW_TblRegister  where ClientID = '" + id + "'  and DateOfDismantle is NULL and (DateTagged <> '01/01/1900' and DateTagged is not null) and ('" + siteid + "' = 'All'  or SiteLocId = '" + siteid + "')  and('" + jobselect + "' = 'All'  or LocationId = '" + jobselect + "') and (Status = 'De-Planked' or Status = 'Tie-Down') and(CycloneAction = 'De-Plank Only' or CycloneAction = 'Tie-Down')union all select 'Tied-Down' as CycloneAction , isnull(Sum(case when CycloneAction = 'Tie-Down' then case when UseEnteredValue = 1 then TotalLabourDismantleEdit else TotalLabourDismantle end end), 0) as DismantleHrs from VW_TblRegister  where ClientID = '" + id + "' and DateOfDismantle is NULL and (DateTagged <> '01/01/1900' and DateTagged is not null) and ('" + siteid + "' = 'All'  or SiteLocId = '" + siteid + "') and('" + jobselect + "' = 'All'  or LocationId = '" + jobselect + "') and(Status = 'De-Planked' or Status = 'Tie-Down')and(CycloneAction = 'De-Plank Only' or CycloneAction = 'Tie-Down')";
            //String query = "select 'De-Planked' as CycloneAction ,isnull(Sum(case when Status = 'Full Strip' then ManHrsDismantle When CycloneAction<> 'Full Strip' and UseEnteredValue = 1 then TotalLabourDismantleEdit else TotalLabourDismantle end ), 0) as DismantleHrs from VW_TblRegister  where ClientID = '" + id + "'  and DateOfDismantle is NULL and (DateTagged <> '01/01/1900' and DateTagged is not null) and ('" + siteid + "' = 'All'  or SiteLocId = '" + siteid + "') and('" + ddlRiskRating + "' = 'All'  or PriorityRiskRating = '" + ddlRiskRating + "') and('" + jobselect + "' = 'All'  or LocationId = '" + jobselect + "') and Status = 'De-Planked' union all select 'Tied-Down' as CycloneAction , isnull(Sum(case when Status = 'Full Strip' then ManHrsDismantle When CycloneAction<> 'Full Strip' and UseEnteredValue = 1 then TotalLabourDismantleEdit else TotalLabourDismantle end ), 0) as DismantleHrs from VW_TblRegister  where ClientID = '" + id + "' and DateOfDismantle is NULL and (DateTagged <> '01/01/1900' and DateTagged is not null) and ('" + siteid + "' = 'All'  or SiteLocId = '" + siteid + "') and('" + jobselect + "' = 'All'  or LocationId = '" + jobselect + "')and('" + ddlRiskRating + "' = 'All'  or PriorityRiskRating = '" + ddlRiskRating + "') and  Status = 'Tied-Down'";
            string FunctionName = "ScaffoldsTieDown";
            string Registerid = "";
            string CycloneActionName = "";
            string query = "exec Pro_CycloneDashboard '" + id + "', '" + siteid + "', '" + jobselect + "','" + Registerid + "', '" + ddlRiskRating + "','" + CycloneActionName + "', '" + FunctionName + "'";

            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ScaffoldAllDashBoardViewModel pro = new ScaffoldAllDashBoardViewModel();
                    if (!(item["CycloneAction"] is DBNull) && item["CycloneAction"].ToString() != "")
                    {
                        pro.CycloneActionHr = item["CycloneAction"].ToString();
                    }
                    if (!(item["DismantleHrs"] is DBNull) && item["DismantleHrs"].ToString() != "")
                    {
                        pro.TotalCountsDismantle = Convert.ToDecimal(item["DismantleHrs"].ToString());
                    }
                    CycloneDashboardChart2.Add(pro);
                }
            }
            // return CycloneDashboardChart2;
            return Json(CycloneDashboardChart2, JsonRequestBehavior.AllowGet);
        }
        // All categories to csv start here
        public void ExportAllCategories(string id, string siteID, string ddlarea, /*string SubOwner,*/ string SummDetail, string CycAction,string ddlRiskRating)
        {
            //var SubOwner1 = "";
            //if (SubOwner == "C")
            //{
            //    var Query = "C&I";
            //    SubOwner1 = SubOwner;
            //    SubOwner1 = SubOwner1.Replace("C", Query);
            //}
            //else
            //{
            //    SubOwner1 = SubOwner;
            //}

            //string allcategory = "exec Pro_CycloneGetData  '" + siteID + "','" + ddlarea + "','" + databaseCon.Decrypt(id) + "','" + SubOwner1 + "','" + SummDetail + "','" + CycAction + "'";
            string allcategory = "exec Pro_CycloneGetData  '" + siteID + "','" + ddlarea + "','" + databaseCon.Decrypt(id) + "','" + SummDetail + "','" + CycAction + "','" + ddlRiskRating + "'";
            DataTable dt = databaseCon.GetData(allcategory);

            Response.ContentType = "Application/x-msexcel";
            Response.AddHeader("content-disposition", "attachment;filename='All Categories' - " + DateTime.Now.Date.ToString("dd'-'MM'-'yyyy") + ".csv");
            Response.Write(csvfile(dt));
            Response.End();
        }

        public ActionResult ScaffoldsDePlankeddata(string CycloneActionName, string ClientID, string siteid, string jobselect, string ddlRiskRating)
        {
            string ConditionForCheck = "";
            if (CycloneActionName == "Tied-Down")
            {
                ConditionForCheck = "Tied-Down";
            }
            else
            {
                ConditionForCheck = "De-Planked";
            }

            var ConditionAddNoAction = "and Status='" + ConditionForCheck + "'";

            // String query = "select Right ('000000' + convert (varchar, [ScaffRef]),6) as ScaffRef,Description,Location,BuildingNo,AssetNo,FloorLevel,DesignType,MaterialType,TotalLabourDismantleEdit,PriorityRiskRating from VW_TblRegister where clientid ='" + ClientID + "'  and DateOfDismantle != '01/01/1900' and DateOfDismantle is Not Null and ('" + siteid + "'= 'All'  or SiteLocId  = '" + siteid + "') and('" + jobselect + "'= 'All'  or LocationId = '" + jobselect + "')  and('" + SubOwner.Replace("'", "''") + "'  = 'All'  or Case when ScaffoldOwner = 'C&I' then 'C&I' when ScaffoldOwner = 'Shutdowns' then 'Shutdowns' else 'Maintenance' End ='" + SubOwner.Replace("'", "''") + "') and Status = 'Completed' and CycloneAction = 'De-Plank Only' Order by Location,BuildingNo";
            //string query = "select Right ('000000' + convert (varchar, [ScaffRef]),6) as ScaffRef,Description,Location,BuildingNo,AssetNo,FloorLevel,DesignType,MaterialType, case when CycloneAction = 'Full Strip' then ManHrsDismantle When CycloneAction <> 'Full Strip' and UseEnteredValue = 1 then TotalLabourDismantleEdit else TotalLabourDismantle End TotalLabourDismantleEdit,PriorityRiskRating from VW_TblRegister where clientid='" + ClientID + "' and DateOfDismantle is NULL and(DateTagged <> '01/01/1900' and DateTagged is not null) and('" + siteid + "' = 'All'  or SiteLocId = '" + siteid + "') and ('" + jobselect + "' = 'All'  or LocationId = '" + jobselect + "') " + ConditionAddNoAction + " and(Status = 'De-Planked' or Status = 'Tie-Down')and(CycloneAction = 'De-Plank Only' or CycloneAction = 'Tie-Down') Order by Location,BuildingNo";
            string query = "select Right ('000000' + convert (varchar, [ScaffRef]),6) as ScaffRef,Description,Location,BuildingNo,AssetNo,FloorLevel,DesignType,CycloneAction,MaterialType, case when CycloneAction = 'Full Strip' then ManHrsDismantle When CycloneAction <> 'Full Strip' and UseEnteredValue = 1 then TotalLabourDismantleEdit else TotalLabourDismantle End TotalLabourDismantleEdit,PriorityRiskRating from VW_TblRegister where clientid='" + ClientID + "' and DateOfDismantle is NULL and(DateTagged <> '01/01/1900' and DateTagged is not null) and('" + siteid + "' = 'All'  or SiteLocId = '" + siteid + "') and('" + ddlRiskRating + "' = 'All'  or PriorityRiskRating = '" + ddlRiskRating + "') and ('" + jobselect + "' = 'All'  or LocationId = '" + jobselect + "') " + ConditionAddNoAction + "  Order by Location,BuildingNo";

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

                    CycloneDashboardClick.Add(pro);
                }
            }
            // return CycloneDashboardClick;
            return Json(CycloneDashboardClick, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ScaffoldsTieDowndata(string CycloneActionName, string ClientID, string siteid, string jobselect, string ddlRiskRating)
        {
            string ConditionForCheck = "";
            if (CycloneActionName == "Tied-Down")
            {
                ConditionForCheck = "Tied-Down";
            }
            else
            {
                ConditionForCheck = "De-Planked";
            }

            var ConditionAddNoAction = "and Status='" + ConditionForCheck + "'";

            //string query = "select Right ('000000' + convert (varchar, [ScaffRef]),6) as ScaffRef,Description,Location,BuildingNo,AssetNo,FloorLevel,DesignType,MaterialType, case when CycloneAction = 'Full Strip' then ManHrsDismantle When CycloneAction <> 'Full Strip' and UseEnteredValue = 1 then TotalLabourDismantleEdit else TotalLabourDismantle End TotalLabourDismantleEdit,PriorityRiskRating from VW_TblRegister where clientid='" + ClientID + "' and DateOfDismantle is NULL and(DateTagged <> '01/01/1900' and DateTagged is not null) and('" + siteid + "' = 'All'  or SiteLocId = '" + siteid + "') and ('" + jobselect + "' = 'All'  or LocationId = '" + jobselect + "') " + ConditionAddNoAction + " and(Status = 'De-Planked' or Status = 'Tie-Down')and(CycloneAction = 'De-Plank Only' or CycloneAction = 'Tie-Down') Order by Location,BuildingNo";
            string query = "select Right ('000000' + convert (varchar, [ScaffRef]),6) as ScaffRef,Description,Location,BuildingNo,AssetNo,FloorLevel,DesignType,CycloneAction,MaterialType, case when CycloneAction = 'Full Strip' then ManHrsDismantle When CycloneAction <> 'Full Strip' and UseEnteredValue = 1 then TotalLabourDismantleEdit else TotalLabourDismantle End TotalLabourDismantleEdit,PriorityRiskRating from VW_TblRegister where clientid='" + ClientID + "' and DateOfDismantle is NULL and(DateTagged <> '01/01/1900' and DateTagged is not null) and('" + siteid + "' = 'All'  or SiteLocId = '" + siteid + "') and('" + ddlRiskRating + "' = 'All'  or PriorityRiskRating = '" + ddlRiskRating + "') and ('" + jobselect + "' = 'All'  or LocationId = '" + jobselect + "') " + ConditionAddNoAction + " Order by Location,BuildingNo";

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

                    CycloneDashboardClick.Add(pro);
                }
            }
            // return CycloneDashboardClick;
            return Json(CycloneDashboardClick, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveManHoursData(string ClientId, string UserId, string SiteId, string ManHoursAvailable)
        {
            string query = "exec Pro_TBLSITELOCATION_MANHOURS_UPDATE '" + databaseCon.Decrypt(ClientId) + "', '" + databaseCon.Decrypt(UserId) + "','" + SiteId + "','" + ManHoursAvailable +"'";

            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);
            string success = "0";
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                success = ds.Tables[0].Rows[0]["Success"].ToString();
            }

            return Json(new { Success = success }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetManHourData(string siteId, string ClientId)
        {
            var result = new
            {
                success = false,
                data = new
                {
                    ManHoursAvailable = ""
                    
                },
                message = ""
            };

            try
            {
                string query = "SELECT  ManHoursAvailable " + "FROM TBLSITELOCATION WHERE SiteLocID='" + siteId + "' AND ClientId='" + databaseCon.Decrypt(ClientId) + "'";
                DataTable dt = databaseCon.GetData(query);

                if (dt.Rows.Count > 0)
                {
                    result = new
                    {
                        success = true,
                        data = new
                        {
                            ManHoursAvailable = dt.Rows[0]["ManHoursAvailable"].ToString(),
                           
                        },
                        message = "Data loaded successfully"
                    };
                }
                else
                {
                    result = new
                    {
                        success = false,
                        data = result.data,
                        message = "No data found for the selected site."
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);

                result = new
                {
                    success = false,
                    data = result.data,
                    message = "An error occurred while fetching data."
                };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
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