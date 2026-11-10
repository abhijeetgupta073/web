//using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using OfficeOpenXml;
using Scaffold.App_Code;
using Scaffold_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebGrease.Css.Ast;

namespace Scaffold.Controllers
{
    public class MultipleReportController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: MultipleReport
        public ActionResult MultipleReport(string UID, string UserRole, string id)
        {
            return View();
        }

        //Commented on 15-07-2024 By Abhijeet For Handle ' in date of Dismantle Entered by Report

        //public ActionResult GetDescriptionValueData(string listboxAvailableReports)
        //{

        //    string query = "";
        //    string listdescription = "";
        //    query = "select isnull(reportdesc,'') as reportdesc from tblreports where reportname='" + listboxAvailableReports + "'";
        //    DataTable dt = databaseCon.GetData(query);
        //    if (dt.Rows.Count > 0)
        //    {
        //        listdescription = dt.Rows[0]["reportdesc"].ToString();
        //        //listdescription.DataSource = dt;

        //        //listdescription.DataValueField = "ReportDesc";
        //        //listdescription.DataTextField = "ReportDesc";
        //        //listdescription.DataBind();
        //    }
        //    return Json(listdescription, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult GetDescriptionValueData(string listboxAvailableReports)
        {
            string query = "";
            string listdescription = "";

            // Escape single quotes in the input
            string safeListboxAvailableReports = listboxAvailableReports.Replace("'", "''");

            query = "SELECT ISNULL(reportdesc, '') AS reportdesc FROM tblreports WHERE reportname = '" + safeListboxAvailableReports + "'";
            DataTable dt = databaseCon.GetData(query);

            if (dt.Rows.Count > 0)
            {
                listdescription = dt.Rows[0]["reportdesc"].ToString();
            }

            return Json(listdescription, JsonRequestBehavior.AllowGet);
        }

        //Commented By Abhijeet On 18-07-2024 for Join Table TblHideshow

        //public ActionResult ListBoxAvailableReportBind()
        //{
        //    string query = "";

        //    //query = "select ReportID,ReportName,ReportDesc from tblReports where hide is null Order by ReportName";
        //    //Changed done on 28/Oct/2020 (displaying By DisplayOrder)
        //    query = "select ReportID,ReportName,ReportDesc from tblReports where hide is null Order by DisplayOrder";
        //    var response = dbContext.Database.SqlQuery<BoxAvailableViewModel>(query);
        //    //dt = databaseCon.GetData(query);
        //    //listboxAvailableReports.DataSource = dt;
        //    ////  ViewState["tblReportsData"] = dt;
        //    //listboxAvailableReports.DataTextField = "ReportName";
        //    //listboxAvailableReports.DataValueField = "ReportID";
        //    //listboxAvailableReports.DataBind();
        //    return Json(response, JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult ListBoxAvailableReportBind(string UID, string id)
        //{

        //    string UserRole = "Select RoleNumber from TblUserRole left join TblUser on TblUserRole.Rolename = TblUser.Role where Userid = '"+ databaseCon.Decrypt(UID) + "'";
        //    DataTable dt0 = databaseCon.GetData(UserRole);
        //    UserRole = dt0.Rows[0]["RoleNumber"].ToString();
        //    string query = "select ReportID,ReportName,ReportDesc from tblReports left Join TblHideshow on" +
        //        " TblHideshow.Pagename=tblReports.Reportname where hide is null and ClientId='" + databaseCon.Decrypt(id) +
        //        "' and HideFromRole not Like '%" + UserRole + "%' Order by DisplayOrder";
        //    var response = dbContext.Database.SqlQuery<BoxAvailableViewModel>(query);

        //    return Json(response, JsonRequestBehavior.AllowGet);
        //}
        //Commented on 18-nov-2024 by abhijeet gupta
        //public ActionResult ListBoxAvailableReportBind(string UID, string id, string UserRole)
        //{
        //    //var Userrole = databaseCon.Decrypt(UserRole);
        //    string query = "select ReportID,ReportName,ReportDesc from tblReports left Join TblHideshow on" +
        //        " TblHideshow.Pagename=tblReports.Reportname where hide is null and ClientId='" + databaseCon.Decrypt(id) +
        //        "' and HideFromRole not Like '%" + databaseCon.Decrypt(UserRole) + "%' Order by DisplayOrder";
        //    var response = dbContext.Database.SqlQuery<BoxAvailableViewModel>(query);

        //    return Json(response, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult ListBoxAvailableReportBind(string UID, string id, string UserRole)
        {
            string decryptedId = databaseCon.Decrypt(id);
            string decryptedUserRole = databaseCon.Decrypt(UserRole);

            string query = @"
        SELECT ReportID, ReportName, ReportDesc 
        FROM tblReports 
        LEFT JOIN TblHideshow ON TblHideshow.Pagename = tblReports.Reportname 
        WHERE hide IS NULL 
        AND ClientId = @ClientId 
        AND (TblHideshow.HideFromRole IS NULL OR 
             NOT EXISTS (
                SELECT 1 
                FROM STRING_SPLIT(TblHideshow.HideFromRole, ',') 
                WHERE value = @UserRole
             ))
        ORDER BY DisplayOrder";

            var parameters = new[]
            {
        new SqlParameter("@ClientId", decryptedId),
        new SqlParameter("@UserRole", decryptedUserRole)
    };

            var response = dbContext.Database.SqlQuery<BoxAvailableViewModel>(query, parameters);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult ddlownername(string id)
        //{
        //    string query = "select distinct ScaffoldOwner from tblRegister where ClientId='" + databaseCon.Decrypt(id) + "' and ScaffoldOwner is not null and ScaffoldOwner!='' order by ScaffoldOwner";

        //    DataTable dt = databaseCon.GetData(query);
        //    if (dt.Rows.Count > 0)
        //    {

        //        //ddlScaffoldOwner.DataSource = dt;
        //        //ddlScaffoldOwner.DataValueField = "ScaffoldOwner";
        //        //ddlScaffoldOwner.DataTextField = "ScaffoldOwner";
        //        //ddlScaffoldOwner.DataBind();
        //        //ddlScaffoldOwner.Items.Insert(0,new ListItem("",""));
        //    }
        //    //add dropdown-19-Oct-2019
        //    string querysite = "select SiteLocID,SiteLocation from tblSiteLocation where ClientId='" + databaseCon.Decrypt(id) + "' Order by SiteLocation";
        //    DataTable dtsite = databaseCon.GetData(querysite);
        //    if (dtsite.Rows.Count > 0)
        //    {
        //        //ddlsiteloctnid.DataSource = dtsite;
        //        //ddlsiteloctnid.DataValueField = "SiteLocID";
        //        //ddlsiteloctnid.DataTextField = "SiteLocation";
        //        //ddlsiteloctnid.DataBind();
        //        //ddlsiteloctnid.Items.Insert(0, new ListItem("", ""));
        //    }
        //}
        public ActionResult ddlSiteLocation(string id, string UserRole, string UID)
        {
            var decryptedUserRole = databaseCon.Decrypt(UserRole);
            string querysite = "";
            /*Added on 12 Sep 2026 for specific user roles ClientViewSingleSite for show only assigned sites */
            if (decryptedUserRole == "60") {
                querysite = "select sl.SiteLocID, sl.SiteLocation from tblSiteLocation sl join tblUser on sl.OwnerName = tblUser.SiteOwnerName where sl.ClientId='" + databaseCon.Decrypt(id) + "' and tblUser.UserId ='" + databaseCon.Decrypt(UID) + "' Order by sl.SiteLocation";
               
            }
            /*Added on 12 Sep 2026 for specific user roles ClientViewSingleSiteSingleLocation for show not showing any sites */
            else if (decryptedUserRole == "61") {
                
                
            }
            else
            {
                querysite = "select SiteLocID,SiteLocation from tblSiteLocation where ClientId='" + databaseCon.Decrypt(id) + "' Order by SiteLocation";
            }
            var response = dbContext.Database.SqlQuery<BoxAvailableViewModel>(querysite);
            
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ddlownername(string id)
        {
            string query = "select distinct ScaffoldOwner from tblRegister where ClientId='" + databaseCon.Decrypt(id) + "' and ScaffoldOwner is not null and ScaffoldOwner!='' order by ScaffoldOwner";
            var response = dbContext.Database.SqlQuery<BoxAvailableViewModel>(query);
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        public ActionResult listdescription_TextChanged(string listboxAvailableReports, string listdescription)
        {
            string Result = "";
            string Message = "";
            if (listboxAvailableReports == "")
            {
                //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                //art1.Style.Add("display", "block");
                //lblalert.Text = "<strong>Alert ! </strong>Please Select Report name from the list..";
                //listdescription.Text = "";
                Result = "Failed";
                Message = "Please Select Report name from the list..";
            }
            else
            {
                string query = "Update tblreports set ReportDesc='" + listdescription + "' where ReportName='" + listboxAvailableReports + "'";
                int Count = databaseCon.RunCmd(query);
                if (Count > 0)
                {
                    //art1.Style.Add("display", "block");
                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                    //lblalert.Text = "<strong>Success !</strong> Record Saved Succefully.";
                    Result = "Success";
                    Message = "Record Saved Successfully.";
                }
            }
            var response = new { Result = Result, Message = Message };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        //public void ExportToCsv_Click(string UID, string id, string site, string Owner, string availablereportname, string btn)
        //{

        //    var csvfilename = "";
        //    string Query = "";
        //    //var Ownername = Owner;
        //    //Added on 24-04-2024 for Decrypt Owner for Speacial Character like C&I
        //    string Ownername = databaseCon.Decrypt(Owner).Replace("'", "''").Trim();
        //    var siteid = site;
        //    //if (databaseCon.Decrypt(Request.QueryString["availablereportname"]) == "Over 90 days Standing Report")
        //    if (databaseCon.Decrypt(Request.QueryString["availablereportname"]) == "Approved for Dismantle Report")
        //    {
        //        csvfilename = "Approved for Dismantle Report";
        //        //Query = "select ScaffRef,SiteLocation, WoNumber as Work Order No, BuildingNo,FloorLevel,Bearing,Replace(Replace(Description,Char(13),' '),char(10),' ') as Description,AssetNo,DateTagged,DaysSinceLastInsp,Tonnage,DesignType,MaterialType from VW_TblRegister where clientid ='" + databaseCon.Decrypt(id) + "' and SiteLocId ='" + databaseCon.Decrypt(siteid) ";
        //        //Query = " select Right('000000' + Convert(varchar, ScaffRef),6)  as ScaffRef,SiteLocation as Site,WONumber as 'Work Order No', DateTagged, DismantleApprovedDate,ManHrsDismantle as 'Dismantle Hours', Location, BuildingNo,scaffoldOwner,SapNumber,AssetNo,Description from VW_TblRegister where ClientID = '" + databaseCon.Decrypt(id) + "'and  SiteLocId ='" + databaseCon.Decrypt(siteid) + "' and ScaffoldOwner='" + databaseCon.Decrypt(Ownername.Replace("'", "''")).Trim() + "' and DateOfDismantle is null and (DismantleApprovedDate Is not Null and DismantleApprovedDate != '' and DismantleApprovedDate != '1900/01/01') order by Location,BuildingNo";

        //        // Commented on 17-05-2023 to Rename SAP Number to Outage Code.
        //        //Query = "select  Right('000000'+Convert(varchar,ScaffRef),6)  as ScaffRef, SiteLocation as Site,WoNumber as 'Work Order No',Description as 'Reason For Access', DateTagged, DismantleApprovedDate as 'Dis.Appr.Date',ManHrsDismantle as 'Dismantle Hours', Location, BuildingNo as 'Building/Module No.',AssetNo as 'Asset/Func.Loc',SapNumber    from VW_TblRegister where ClientID='" + databaseCon.Decrypt(id) + "' and SiteLocId ='" + databaseCon.Decrypt(siteid) + "' and ('All'='" + databaseCon.Decrypt(Ownername.Replace("'", "''")).Trim() + "' or ScaffoldOwner='" + databaseCon.Decrypt(Ownername.Replace("'", "''")).Trim() + "') and DateOfDismantle is null and (DismantleApprovedDate Is not Null and DismantleApprovedDate != '' and DismantleApprovedDate != '1900/01/01') order by Location,BuildingNoorder by ScaffRef";
        //        //Commented on 11-08-2023 added status 
        //        //Query = "select  Right('000000'+Convert(varchar,ScaffRef),6)  as ScaffRef, SiteLocation as Site,WoNumber as 'Work Order No',Description as 'Reason For Access', DateTagged, DismantleApprovedDate as 'Dis.Appr.Date',ManHrsDismantle as 'Dismantle Hours', Location, BuildingNo as 'Building/Module No.',AssetNo as 'Asset/Func.Loc',SapNumber as OutageCode,status   from VW_TblRegister where ClientID='" + databaseCon.Decrypt(id) + "' and SiteLocId ='" + databaseCon.Decrypt(siteid) + "' and ('All'='" + databaseCon.Decrypt(Ownername.Replace("'", "''")).Trim() + "' or ScaffoldOwner='" + databaseCon.Decrypt(Ownername.Replace("'", "''")).Trim() + "') and DateOfDismantle is null and (DismantleApprovedDate Is not Null and DismantleApprovedDate != '' and DismantleApprovedDate != '1900/01/01') order by Location,BuildingNo,ScaffRef";
        //        //Added Status 11-08-2023 
        //        Query = "select  Right('000000'+Convert(varchar,ScaffRef),6)  as ScaffRef, SiteLocation as Site,WoNumber as 'Work Order No',Description as 'Reason For Access', DateTagged, DismantleApprovedDate as 'Dis.Appr.Date',ManHrsDismantle as 'Dismantle Hours', Location, BuildingNo as 'Building/Module No.',AssetNo as 'Asset/Func.Loc',SapNumber,Status    from VW_TblRegister where ClientID='" + databaseCon.Decrypt(id) + "' and SiteLocId ='" + databaseCon.Decrypt(siteid) + "' and ('All'='" + databaseCon.Decrypt(Ownername.Replace("'", "''")).Trim() + "' or ScaffoldOwner='" + databaseCon.Decrypt(Ownername.Replace("'", "''")).Trim() + "') and DateOfDismantle is null and (DismantleApprovedDate Is not Null and DismantleApprovedDate != '' and DismantleApprovedDate != '1900/01/01') and (DateTagged Is not Null and DateTagged != '' and DateTagged != '1900/01/01') order by Location,BuildingNo , ScaffRef";

        //    }

        //    if (databaseCon.Decrypt(Request.QueryString["availablereportname"]) == "Over 90 days less than 180 days Standing Report")
        //    {
        //        csvfilename = "Over 90 days less than 180 days Standing Report";
        //        //Query = "select * from VW_TblRegister where ClientID = '" + id + "' and SiteLocId ='" + siteid + "'  and DateTagged>DATEADD(d,-90,Convert(date,getdate()))and DateOFDismantle is null";
        //        //Query = "select ScaffRef,Location,BuildingNo,FloorLevel,Bearing,Description,AssetNo,DateTagged,DaysSinceLastInsp,Tonnage,DesignType,MaterialType,HireDaysToDate,Length,Width,Height from VW_TblRegister where clientid ='" + databaseCon.Decrypt(id) + "' and SiteLocId ='" + databaseCon.Decrypt(siteid) + "' and HireDaysToDate>90 and HireDaysToDate<=180";
        //        //Commented on 11-08-2023 added status 
        //        //Query = "select ScaffRef,WONumber,Location,BuildingNo,FloorLevel,Bearing,Replace(Replace(Description,Char(13),' '),char(10),' ') as Description,AssetNo,DateTagged,DaysSinceLastInsp,convert(decimal(18, 2),((tonnage)/1000)) as Tonnage,DesignType,MaterialType,HireDaysToDate,Length,Width,Height from VW_TblRegister where clientid ='" + databaseCon.Decrypt(id) + "' and SiteLocId ='" + databaseCon.Decrypt(siteid) + "' and ('All'='" + Owner + "' or ScaffoldOwner ='" + Owner + "') and HireDaysToDate>90 and HireDaysToDate<=180 order by DateTagged,ScaffRef";
        //        //Added Status 11-08-2023
        //        Query = "select ScaffRef,WONumber,Location,BuildingNo,FloorLevel,Bearing,Replace(Replace(Description,Char(13),' '),char(10),' ') as Description,AssetNo,DateTagged,DaysSinceLastInsp,convert(decimal(18, 2),((tonnage)/1000)) as Tonnage,DesignType,MaterialType,HireDaysToDate,Length,Width,Height,Status from VW_TblRegister where clientid ='" + databaseCon.Decrypt(id) + "' and SiteLocId ='" + databaseCon.Decrypt(siteid) + "' and ('All'='" + Ownername + "' or ScaffoldOwner ='" + Ownername + "') and HireDaysToDate>90 and HireDaysToDate<=180 order by DateTagged,ScaffRef";
        //    }

        //    if (databaseCon.Decrypt(Request.QueryString["availablereportname"]) == "Over 180 days Standing Report")
        //    {
        //        // csvfilename = "Over 90 days less than 180 days Standing Report";
        //        csvfilename = "Over 180 days Standing Report";
        //        //Query = "select * from VW_TblRegister where ClientID = '" + id + "' and SiteLocId ='" + siteid + "'  and DateTagged>DATEADD(d,-180,Convert(date,getdate()))and DateOFDismantle is null";

        //        //Query = "select ScaffRef,Location,BuildingNo,FloorLevel,Bearing,Description,AssetNo,DateTagged,DaysSinceLastInsp,Tonnage,DesignType,MaterialType,HireDaysToDate,Length,Width,Height from VW_TblRegister where clientid ='" + databaseCon.Decrypt(id) + "' and SiteLocId='" + databaseCon.Decrypt(siteid) + "' and HireDaysToDate>180";
        //        //Commented on 11-08-2023 added status 
        //        //Query = "select ScaffRef,WONumber,Location,BuildingNo,FloorLevel,Bearing,Replace(Replace(Description,Char(13),' '),char(10),' ') as Description,AssetNo,DateTagged,DaysSinceLastInsp,convert(decimal(18, 2),((tonnage)/1000)) as Tonnage,DesignType,MaterialType,HireDaysToDate,Length,Width,Height from VW_TblRegister where clientid ='" + databaseCon.Decrypt(id) + "' and SiteLocId='" + databaseCon.Decrypt(siteid) + "' and ('All'='" + Owner + "' or ScaffoldOwner ='" + Owner + "') and HireDaysToDate>180 order by DateTagged,ScaffRef";
        //        //Added Status 11-08-2023
        //        Query = "select ScaffRef,WONumber,Location,BuildingNo,FloorLevel,Bearing,Replace(Replace(Description,Char(13),' '),char(10),' ') as Description,AssetNo,DateTagged,DaysSinceLastInsp,convert(decimal(18, 2),((tonnage)/1000)) as Tonnage,DesignType,MaterialType,HireDaysToDate,Length,Width,Height,Status from VW_TblRegister where clientid ='" + databaseCon.Decrypt(id) + "' and SiteLocId='" + databaseCon.Decrypt(siteid) + "' and ('All'='" + Ownername + "' or ScaffoldOwner ='" + Ownername + "') and HireDaysToDate>180 order by DateTagged,ScaffRef";

        //    }

        //    if (databaseCon.Decrypt(Request.QueryString["availablereportname"]) == "Over 30 days less than 90 days Standing Report")
        //    {
        //        //csvfilename = "Over 90 days less than 180 days Standing Report";
        //        csvfilename = "Over 30 days less than 90 days Standing Report";
        //        //Query = "select ScaffRef,Location,BuildingNo,FloorLevel,Bearing,Description,AssetNo,DateTagged,DaysSinceLastInsp,Tonnage,DesignType,MaterialType,HireDaysToDate,Length,Width,Height  from VW_TblRegister where clientid ='" + databaseCon.Decrypt(id) + "' and SiteLocId='" + databaseCon.Decrypt(siteid) + "' and HireDaysToDate>30 and HireDaysToDate<=90";
        //        //Commented on 11-08-2023 added status
        //        //Query = "select ScaffRef,WONumber,Location,BuildingNo,FloorLevel,Bearing,Replace(Replace(Description,Char(13),' '),char(10),' ') as Description,AssetNo,DateTagged,DaysSinceLastInsp,convert(decimal(18, 2),((tonnage)/1000)) as Tonnage,DesignType,MaterialType,HireDaysToDate,Length,Width,Height  from VW_TblRegister where clientid ='" + databaseCon.Decrypt(id) + "' and SiteLocId='" + databaseCon.Decrypt(siteid) + "' and ('All'='" + Owner + "' or ScaffoldOwner ='" + Owner + "') and HireDaysToDate>30 and HireDaysToDate<=90 order by DateTagged,ScaffRef";
        //        //Added Status 11-08-2023
        //        Query = "select ScaffRef,WONumber,Location,BuildingNo,FloorLevel,Bearing,Replace(Replace(Description,Char(13),' '),char(10),' ') as Description,AssetNo,DateTagged,DaysSinceLastInsp,convert(decimal(18, 2),((tonnage)/1000)) as Tonnage,DesignType,MaterialType,HireDaysToDate,Length,Width,Height,Status  from VW_TblRegister where clientid ='" + databaseCon.Decrypt(id) + "' and SiteLocId='" + databaseCon.Decrypt(siteid) + "' and ('All'='" + Ownername + "' or ScaffoldOwner ='" + Ownername + "') and HireDaysToDate>30 and HireDaysToDate<=90 order by DateTagged,ScaffRef";
        //    }

        //    if (databaseCon.Decrypt(Request.QueryString["availablereportname"]) == "Over 30 days Standing Report")
        //    {
        //        // csvfilename = "Over 90 days less than 180 days Standing Report";
        //        csvfilename = "Over 30 days Standing Report";
        //        //Query = "select ScaffRef,Location,BuildingNo,FloorLevel,Bearing,Description,AssetNo,DateTagged,DaysSinceLastInsp,Tonnage,DesignType,MaterialType,HireDaysToDate,Length,Width,Height  from VW_TblRegister where clientid ='" + databaseCon.Decrypt(id) + "' and SiteLocId='" + databaseCon.Decrypt(siteid) + "' and HireDaysToDate>30";
        //        //Commented on 11-08-2023 added status  
        //        //Query = "select ScaffRef,WONumber,Location,BuildingNo,FloorLevel,Bearing,Replace(Replace(Description,Char(13),' '),char(10),' ') as Description,AssetNo,DateTagged,DaysSinceLastInsp,convert(decimal(18, 2),((tonnage)/1000)) as Tonnage,DesignType,MaterialType,HireDaysToDate,Length,Width,Height  from VW_TblRegister where clientid ='" + databaseCon.Decrypt(id) + "' and SiteLocId='" + databaseCon.Decrypt(siteid) + "' and ('All'='" + Owner + "' or ScaffoldOwner ='" + Owner + "') and HireDaysToDate>30 order by DateTagged,ScaffRef";
        //        //Added Status 11-08-2023
        //        Query = "select ScaffRef,WONumber,Location,BuildingNo,FloorLevel,Bearing,Replace(Replace(Description,Char(13),' '),char(10),' ') as Description,AssetNo,DateTagged,DaysSinceLastInsp,convert(decimal(18, 2),((tonnage)/1000)) as Tonnage,DesignType,MaterialType,HireDaysToDate,Length,Width,Height,Status  from VW_TblRegister where clientid ='" + databaseCon.Decrypt(id) + "' and SiteLocId='" + databaseCon.Decrypt(siteid) + "' and ('All'='" + Ownername + "' or ScaffoldOwner ='" + Ownername + "') and HireDaysToDate>30 order by DateTagged,ScaffRef";

        //    }

        //    if (databaseCon.Decrypt(Request.QueryString["availablereportname"]) == "ALL Current Standing Scaffolds Report")
        //    {
        //        // csvfilename = "Over 90 days less than 180 days Standing Report";
        //        csvfilename = "ALL Current Standing Scaffolds Report";
        //        //Query = "select ScaffRef,Location,BuildingNo,FloorLevel,Bearing,Description,AssetNo,DateTagged,DaysSinceLastInsp,Tonnage,DesignType,MaterialType,HireDaysToDate,Length,Width,Height  from VW_TblRegister where clientid ='" + databaseCon.Decrypt(id) + "' and SiteLocId='" + databaseCon.Decrypt(siteid) + "' and HireDaysToDate>30";
        //        //Commented on 11-08-2023 added status 
        //        //Query = "select ScaffRef,WONumber,Location,BuildingNo,FloorLevel,Bearing,Replace(Replace(Description,Char(13),' '),char(10),' ') as Description,AssetNo,DateTagged,DaysSinceLastInsp,convert(decimal(18, 2),((tonnage)/1000)) as Tonnage,DesignType,MaterialType,HireDaysToDate,Length,Width,Height  from VW_TblRegister where clientid ='" + databaseCon.Decrypt(id) + "' and SiteLocId='" + databaseCon.Decrypt(siteid) + "' and ('All'='" + Owner + "' or ScaffoldOwner ='" + Owner + "') and HireDaysToDate is not null order by DateTagged,ScaffRef";
        //        //Added Status 11-08-2023
        //        Query = "select ScaffRef,WONumber,Location,BuildingNo,FloorLevel,Bearing,Replace(Replace(Description,Char(13),' '),char(10),' ') as Description,AssetNo,DateTagged,DaysSinceLastInsp,convert(decimal(18, 2),((tonnage)/1000)) as Tonnage,DesignType,MaterialType,HireDaysToDate,Length,Width,Height,Status  from VW_TblRegister where clientid ='" + databaseCon.Decrypt(id) + "' and SiteLocId='" + databaseCon.Decrypt(siteid) + "' and ('All'='" + Ownername + "' or ScaffoldOwner ='" + Ownername + "') and HireDaysToDate is not null order by DateTagged,ScaffRef";

        //    }

        //    DataTable dt = databaseCon.GetData(Query);


        //    Response.ContentType = "Application/x-msexcel";
        //    Response.AddHeader("content-disposition", "attachment;filename='" + csvfilename + "'.csv");
        //    Response.Write(csvfile(dt));
        //    Response.End();


        //}

        //public void ExportToCsvWeekly_Click(string UID, string id, string site, string Owner, string availablereportname, string btn)
        //{
        //    var csvfilename = "";
        //    string Query1 = "";
        //    string Query2 = "";
        //    //var Ownername = Owner;
        //    //Added on 24-04-2024 for Decrypt Owner for Speacial Character like C&I
        //    string Ownername = databaseCon.Decrypt(Owner).Replace("'", "''").Trim();
        //    var siteid = site;

        //    csvfilename = "Last 7 Day Activity Report.";

        //    Query1 = "SELECT ScaffRef as 'Scaff Ref #', WONumber as 'WO #', Location, BuildingNo as 'Building / Module No', FloorLevel as 'Floor Level', Bearing, Description as 'Reason For Access', AssetNo as 'Asset # / Func. Loc', FORMAT(DateTagged, 'dd-MMM-yyyy') AS 'Date Built',  FORMAT(DateOFDismantle, 'dd-MMM-yyyy') AS 'Date OF Dismantle', DaysSinceLastInsp as 'Days Since Last Insp', CONVERT(decimal(18, 2), (tonnage / 1000)) as Tonnage, DesignType as 'Design Type', MaterialType as 'Material Type', HireDaysToDate as 'Hire Days To Date', Length, Width, Height as 'Height (TWP)', Status from VW_TblRegister where clientid ='" + databaseCon.Decrypt(id) + "' and SiteLocId='" + databaseCon.Decrypt(siteid) + "' and ('All'='" + Ownername + "' or ScaffoldOwner ='" + Ownername + "') and isnull(DateTagged,'1900-01-01') > convert(date,DATEADD(day, -7, GETDATE())) order by DateTagged,ScaffRef";

        //    Query2 = "SELECT ScaffRef as 'Scaff Ref #', WONumber as 'WO #', Location, BuildingNo as 'Building / Module No', FloorLevel as 'Floor Level', Bearing, Description as 'Reason For Access', AssetNo as 'Asset # / Func. Loc', FORMAT(DateTagged, 'dd-MMM-yyyy') AS 'Date Built',  FORMAT(DateOFDismantle, 'dd-MMM-yyyy') AS 'Date OF Dismantle', DaysSinceLastInsp as 'Days Since Last Insp', CONVERT(decimal(18, 2), (tonnage / 1000)) as Tonnage, DesignType as 'Design Type', MaterialType as 'Material Type', HireDaysToDate as 'Hire Days To Date', Length, Width, Height as 'Height (TWP)', Status from VW_TblRegister where clientid ='" + databaseCon.Decrypt(id) + "' and SiteLocId='" + databaseCon.Decrypt(siteid) + "' and ('All'='" + Ownername + "' or ScaffoldOwner ='" + Ownername + "') and isnull(DateOfDismantle,'1900-01-01') > convert(date,DATEADD(day, -7, GETDATE())) order by DateTagged,ScaffRef";


        //    DataTable dt1 = databaseCon.GetData(Query1);
        //    DataTable dt2 = databaseCon.GetData(Query2);

        //    using (ExcelPackage excelPackage = new ExcelPackage())
        //    {

        //        ExcelWorksheet worksheet1 = excelPackage.Workbook.Worksheets.Add("Built in last 7 days");
        //        ExcelWorksheet worksheet2 = excelPackage.Workbook.Worksheets.Add("Dismantled in last 7 days");

        //        AddDataToSheet(worksheet1, dt1, "Built in last 7 days", true);
        //        AddDataToSheet(worksheet2, dt2, "Dismantled in last 7 days", true);

        //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        Response.AddHeader("content-disposition", "attachment;filename=" + csvfilename + ".xlsx");
        //        Response.BinaryWrite(excelPackage.GetAsByteArray());
        //        Response.End();
        //    }
        //}

        //Edited By Neeraj


        //public void ExportToCsvDateofDismantleReport_Click(string UID, string id, string site, string Owner, string availablereportname, string btn, string ClientID, string SiteLocId)
        //{
        //    var csvfilename = "";
        //    string Query1 = "";
        //    string Ownername = databaseCon.Decrypt(Owner).Replace("'", "''").Trim();
        //    var siteid = site;

        //    csvfilename = "Date_of_Dismantle_Entered_By";

        //    Query1 = $@"
        // SELECT 
        //    ScaffRef AS [Scaf Ref #],
        //    ClientScaffRef AS [Client Scaf Ref],  
        //    WONumber AS [WO No.], 
        //    SiteLocation AS [Site], 
        //    Location, 
        //    BuildingNo AS [Building/Module No.], 
        //    FloorLevel AS [Floor Level], 
        //    Bearing,
        //    Description AS [Reason For Access.],
        //    AssetNo AS [Asset # /Func. Loc], 
        //    DateTagged AS [Date Built], 
        //    CASE 
        //        WHEN DateOfDismantle IS NULL OR DateOfDismantle = '1900-01-01' 
        //        THEN ''
        //        ELSE CONVERT(varchar, DateOfDismantle, 121)
        //    END AS [Date Dismantled], 
        //    CASE 
        //        WHEN DateOfDismantle IS NULL OR DateOfDismantle = '1900-01-01' 
        //        THEN '' 
        //        ELSE ISNULL(UpdatedBy, ISNULL(CreatedBy, '')) 
        //    END AS [Date Dismantled Entered by]
        //FROM 
        //    dbo.VW_TblRegister 
        //WHERE 
        //    ClientID = '{ClientID}' AND 
        //    SiteLocId = '{SiteLocId}' AND 
        //    ('All' = 'All' OR ScaffoldOwner = 'All') AND
        //    isnull(DateOfDismantle,'1900-01-01')!='1900-01-01'
        //ORDER BY 

        //    ScaffRef";


        //    DataTable dt1 = databaseCon.GetData(Query1);

        //    using (ExcelPackage excelPackage = new ExcelPackage())
        //    {
        //        ExcelWorksheet worksheet1 = excelPackage.Workbook.Worksheets.Add("Date of Dismantle Entered By");
        //        AddDataToSheet(worksheet1, dt1, "'Date of Dismantle' Entered By", true);
        //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        Response.AddHeader("content-disposition", "attachment;filename=" + csvfilename + ".xlsx");
        //        Response.BinaryWrite(excelPackage.GetAsByteArray());
        //        Response.End();
        //    }
        //}

        //void AddDataToSheet(ExcelWorksheet worksheet, DataTable dt, string sheetName, bool includeHeadings)
        //{
        //    worksheet.Cells["A1"].Value = sheetName;
        //    if (includeHeadings)
        //    {
        //        for (int i = 0; i < dt.Columns.Count; i++)
        //        {
        //            worksheet.Cells[2, i + 1].Value = dt.Columns[i].ColumnName;
        //        }
        //    }
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        for (int j = 0; j < dt.Columns.Count; j++)
        //        {
        //            worksheet.Cells[i + (includeHeadings ? 3 : 2), j + 1].Value = dt.Rows[i][j].ToString();
        //        }
        //    }
        //    worksheet.Cells.AutoFitColumns();
        //    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
        //}


        //void AddDataToSheet(ExcelWorksheet worksheet, DataTable dt, string sheetName, bool includeHeadings)
        //{
        //    worksheet.Cells["A1"].Value = sheetName;
        //    if (includeHeadings)
        //    {
        //        for (int i = 0; i < dt.Columns.Count; i++)
        //        {
        //            worksheet.Cells[2, i + 1].Value = dt.Columns[i].ColumnName;
        //        }
        //    }
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        for (int j = 0; j < dt.Columns.Count; j++)
        //        {
        //            string cellValue = dt.Rows[i][j].ToString();
        //            if (dt.Columns[j].ColumnName == "Date Built" ||
        //                dt.Columns[j].ColumnName == "Date Dismantled" ||
        //                dt.Columns[j].ColumnName == "Date Dismantled Entered by" ||
        //                dt.Columns[j].ColumnName == "DateTagged")
        //            {
        //                DateTime dateValue;
        //                if (DateTime.TryParse(cellValue, out dateValue))
        //                {
        //                    if (dateValue != DateTime.Parse("1900-01-01"))
        //                    {
        //                        cellValue = dateValue.ToString("dd-MMM-yyyy");
        //                    }
        //                    else
        //                    {
        //                        cellValue = "";
        //                    }
        //                }
        //            }

        //            worksheet.Cells[i + (includeHeadings ? 3 : 2), j + 1].Value = dt.Rows[i][j].ToString();
        //        }
        //    }
        //    worksheet.Cells.AutoFitColumns();
        //    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
        //}
        //private string csvfile(DataTable dtTable)
        //{
        //    StringBuilder sbldr = new StringBuilder();
        //    if (dtTable.Columns.Count != 0)
        //    {
        //        foreach (DataColumn col in dtTable.Columns)
        //        {
        //            sbldr.Append(col.ColumnName + ',');
        //        }
        //        sbldr.Append("\r\n");
        //        foreach (DataRow row in dtTable.Rows)
        //        {
        //            foreach (DataColumn column in dtTable.Columns)
        //            {
        //                if (row[column].ToString().Contains(','))
        //                {
        //                    sbldr.Append("\"" + row[column].ToString() + "\"" + ',');
        //                }
        //                else
        //                {
        //                    if (column.ColumnName == "DateTagged")
        //                    {
        //                        //sbldr.Append(row[column]).ToString() + ',');
        //                        sbldr.Append(Convert.ToDateTime(row[column]).ToString("dd-MMM-yyyy") + ',');
        //                    }
        //                    else
        //                    {
        //                        sbldr.Append(row[column].ToString() + ',');
        //                    }
        //                }
        //                //sbldr.Append(row[column].ToString().Replace(",", ";") + ',');
        //            }
        //            sbldr.Append("\r\n");
        //        }
        //    }
        //    return sbldr.ToString();
        //}
        //        public void ExportToCsvDateofDismantleReport_Click(string UID, string id, string site, string Owner, string availablereportname, string btn, string ClientID, string SiteLocId)
        //{
        //    var csvfilename = "";
        //        string Query1 = "";
        //        string Ownername = databaseCon.Decrypt(Owner).Replace("'", "''").Trim();
        //        var siteid = site;

        //        csvfilename = "Date_of_Dismantle_Entered_By";

        //    Query1 = $@"
        //        SELECT 
        //            ScaffRef AS [Scaf Ref #],
        //            ClientScaffRef AS [Client Scaf Ref],  
        //            WONumber AS [WO No.], 
        //            SiteLocation AS [Site], 
        //            Location, 
        //            BuildingNo AS [Building/Module No.], 
        //            FloorLevel AS [Floor Level], 
        //            Bearing,
        //            Description AS [Reason For Access.],
        //            AssetNo AS [Asset # /Func. Loc], 
        //            DateTagged AS [Date Built], 
        //            CASE 
        //                WHEN DateOfDismantle IS NULL OR DateOfDismantle = '1900-01-01' 
        //                THEN ''
        //                ELSE CONVERT(varchar, DateOfDismantle, 121)
        //            END AS [Date Dismantled], 
        //            CASE 
        //                WHEN DateOfDismantle IS NULL OR DateOfDismantle = '1900-01-01' 
        //                THEN '' 
        //                ELSE ISNULL(UpdatedBy, ISNULL(CreatedBy, '')) 
        //            END AS [Date Dismantled Entered by]
        //        FROM 
        //            dbo.VW_TblRegister 
        //        WHERE 
        //            ClientID = '{ClientID}' AND 
        //            SiteLocId = '{SiteLocId}' AND 
        //            ('All' = 'All' OR ScaffoldOwner = 'All') AND
        //            isnull(DateOfDismantle,'1900-01-01')!='1900-01-01'
        //        ORDER BY 
        //            ScaffRef";

        //    DataTable dt1 = databaseCon.GetData(Query1);

        //    using (ExcelPackage excelPackage = new ExcelPackage())
        //    {
        //        ExcelWorksheet worksheet1 = excelPackage.Workbook.Worksheets.Add("Date of Dismantle Entered By");

        //    // Add a title row with merged cells
        //    string siteLocation = dt1.Rows.Count > 0 ? dt1.Rows[0]["Site"].ToString() : "";
        //    string currentDate = DateTime.Now.ToString("dd/MM/yyyy");
        //    string title = $"Site Location: {siteLocation} - Printed On: {currentDate}";
        //    worksheet1.Cells["A1:L1"].Merge = true;
        //        worksheet1.Cells["A1:L1"].Value = title;
        //        worksheet1.Cells["A1:L1"].Style.Font.Bold = true;
        //        worksheet1.Cells["A1:L1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

        //        // Add the data to the sheet
        //        AddDataToSheet(worksheet1, dt1, "'Date of Dismantle' Entered By", true);

        //    // Set response for file download
        //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        Response.AddHeader("content-disposition", "attachment;filename=" + csvfilename + ".xlsx");
        //        Response.BinaryWrite(excelPackage.GetAsByteArray());
        //        Response.End();
        //    }
        //}

        //void AddDataToSheet(ExcelWorksheet worksheet, DataTable dt, string sheetName, bool includeHeadings)
        //{
        //    worksheet.Cells["A1"].Value = sheetName;
        //    if (includeHeadings)
        //    {
        //        for (int i = 0; i < dt.Columns.Count; i++)
        //        {
        //            worksheet.Cells[2, i + 1].Value = dt.Columns[i].ColumnName;
        //            worksheet.Cells[2, i + 1].Style.Font.Bold = true;
        //        }
        //    }
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        for (int j = 0; j < dt.Columns.Count; j++)
        //        {
        //            string cellValue = dt.Rows[i][j].ToString();
        //            if (dt.Columns[j].ColumnName == "Date Built" ||
        //                dt.Columns[j].ColumnName == "Date Dismantled" ||
        //                dt.Columns[j].ColumnName == "Date Dismantled Entered by" ||
        //                dt.Columns[j].ColumnName == "DateTagged")
        //            {
        //                DateTime dateValue;
        //                if (DateTime.TryParse(cellValue, out dateValue))
        //                {
        //                    if (dateValue != DateTime.Parse("1900-01-01"))
        //                    {
        //                        cellValue = dateValue.ToString("dd-MMM-yyyy");
        //                    }
        //                    else
        //                    {
        //                        cellValue = "";
        //                    }
        //                }
        //            }

        //            worksheet.Cells[i + (includeHeadings ? 3 : 2), j + 1].Value = cellValue;
        //        }
        //    }
        //    worksheet.Cells.AutoFitColumns();
        //    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
        //}

        //private string csvfile(DataTable dtTable)
        //{
        //    StringBuilder sbldr = new StringBuilder();
        //    if (dtTable.Columns.Count != 0)
        //    {
        //        foreach (DataColumn col in dtTable.Columns)
        //        {
        //            sbldr.Append(col.ColumnName + ',');
        //        }
        //        sbldr.Append("\r\n");
        //        foreach (DataRow row in dtTable.Rows)
        //        {
        //            foreach (DataColumn column in dtTable.Columns)
        //            {
        //                if (row[column].ToString().Contains(','))
        //                {
        //                    sbldr.Append("\"" + row[column].ToString() + "\"" + ',');
        //                }
        //                else
        //                {
        //                    if (column.ColumnName == "DateTagged")
        //                    {
        //                        sbldr.Append(Convert.ToDateTime(row[column]).ToString("dd-MMM-yyyy") + ',');
        //                    }
        //                    else
        //                    {
        //                        sbldr.Append(row[column].ToString() + ',');
        //                    }
        //                }
        //            }
        //            sbldr.Append("\r\n");
        //        }
        //    }
        //    return sbldr.ToString();
        //}


        //public void ExportToCsv_Click(string UID, string id, string site, string Owner, string availablereportname, string btn, string SiteLocId)
        //{
        //    var csvfilename = "";
        //    string Query = "";
        //    // Decrypt Owner for Special Character like C&I
        //    string Ownername = databaseCon.Decrypt(Owner).Replace("'", "''").Trim();
        //    var siteid = databaseCon.Decrypt(site);
        //    // var SiteLocid = SiteLocId;

        //    string decryptedReportName = databaseCon.Decrypt(Request.QueryString["availablereportname"]);
        //    string siteName = "SELECT SiteLocation FROM TblSiteLocation WHERE ClientID =" + databaseCon.Decrypt(id) + "AND SiteLocId = '" + siteid + "'";
        //    DataTable dt0 = databaseCon.GetData(siteName);
        //    siteName = dt0.Rows[0]["SiteLocation"].ToString();
        //    string currentDate = DateTime.Now.ToString("dd-MMM-yyyy");
        //    if (decryptedReportName == "Approved for Dismantle Report")
        //    {
        //        csvfilename = "Approved for Dismantle Report";
        //        Query = "select Right('000000'+Convert(varchar,ScaffRef),6) as ScaffRef, SiteLocation as Site, WoNumber as 'Work Order No', Description as 'Reason For Access', DateTagged, DismantleApprovedDate as 'Dis.Appr.Date', ManHrsDismantle as 'Dismantle Hours', Location, BuildingNo as 'Building/Module No.', AssetNo as 'Asset/Func.Loc', SapNumber, status from VW_TblRegister where ClientID='" + databaseCon.Decrypt(id) + "' and SiteLocId='" + siteid + "' and ('All'='" + Ownername + "' or ScaffoldOwner='" + Ownername + "') and DateOfDismantle is null and (DismantleApprovedDate Is not Null and DismantleApprovedDate != '' and DismantleApprovedDate != '1900/01/01') and (DateTagged Is not Null and DateTagged != '' and DateTagged != '1900/01/01') order by Location, BuildingNo, ScaffRef";

        //        DataTable dt = databaseCon.GetData(Query);
        //        using (ExcelPackage pck = new ExcelPackage())
        //        {
        //            string title = $"Site Location: {siteName}                                  Report Name: {csvfilename}                                      Printed On: {currentDate}";

        //            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet1");
        //            ws.Cells["A1:L1"].Merge = true;
        //            ws.Cells["A1:L1"].Value = title;
        //            ws.Cells["A1:L1"].Style.Font.Bold = true;
        //            ws.Cells["A1:L1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

        //            AddDataToSheet(ws, dt, title, true);

        //            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //            Response.AddHeader("content-disposition", "attachment;filename=" + csvfilename + ".xlsx");
        //            Response.BinaryWrite(pck.GetAsByteArray());
        //            Response.End();
        //        }
        //    }

        //    else
        //    {
        //        if (decryptedReportName == "Over 90 days less than 180 days Standing Report")
        //        {
        //            csvfilename = "Over 90 days less than 180 days Standing Report";
        //            Query = "select ScaffRef,WONumber,Location,BuildingNo,FloorLevel,Bearing,Replace(Replace(Description,Char(13),' '),char(10),' ') as Description,AssetNo,DateTagged,DaysSinceLastInsp,convert(decimal(18, 2),((tonnage)/1000)) as Tonnage,DesignType,MaterialType,HireDaysToDate,Length,Width,Height,Status from VW_TblRegister where clientid ='" + databaseCon.Decrypt(id) + "' and SiteLocId ='" + siteid + "' and ('All'='" + Ownername + "' or ScaffoldOwner ='" + Ownername + "') and HireDaysToDate>90 and HireDaysToDate<=180 order by DateTagged,ScaffRef";
        //            DataTable dt1 = databaseCon.GetData(Query);
        //            using (ExcelPackage pck = new ExcelPackage())
        //            {
        //                string title = $"Site Location: {siteName}                                  Report Name: {csvfilename}                                      Printed On: {currentDate}";

        //                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet1");
        //                ws.Cells["A1:L1"].Merge = true;
        //                ws.Cells["A1:L1"].Value = title;
        //                ws.Cells["A1:L1"].Style.Font.Bold = true;
        //                ws.Cells["A1:L1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

        //                AddDataToSheet(ws, dt1, title, true);

        //                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //                Response.AddHeader("content-disposition", "attachment;filename=" + csvfilename + ".xlsx");
        //                Response.BinaryWrite(pck.GetAsByteArray());
        //                Response.End();
        //            }

        //        }
        //        else if (decryptedReportName == "Over 180 days Standing Report")
        //        {
        //            csvfilename = "Over 180 days Standing Report";
        //            Query = "select ScaffRef,WONumber,Location,BuildingNo,FloorLevel,Bearing,Replace(Replace(Description,Char(13),' '),char(10),' ') as Description,AssetNo,DateTagged,DaysSinceLastInsp,convert(decimal(18, 2),((tonnage)/1000)) as Tonnage,DesignType,MaterialType,HireDaysToDate,Length,Width,Height,Status from VW_TblRegister where clientid ='" + databaseCon.Decrypt(id) + "' and SiteLocId='" + siteid + "' and ('All'='" + Ownername + "' or ScaffoldOwner ='" + Ownername + "') and HireDaysToDate>180 order by DateTagged,ScaffRef";
        //            //DataTable dt2 = databaseCon.GetData(siteName);
        //            DataTable dt2 = databaseCon.GetData(Query);
        //            using (ExcelPackage pck = new ExcelPackage())
        //            {
        //                string title = $"Site Location: {siteName}                                  Report Name: {csvfilename}                                      Printed On: {currentDate}";

        //                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet1");
        //                ws.Cells["A1:L1"].Merge = true;
        //                ws.Cells["A1:L1"].Value = title;
        //                ws.Cells["A1:L1"].Style.Font.Bold = true;
        //                ws.Cells["A1:L1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

        //                AddDataToSheet(ws, dt2, title, true);

        //                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //                Response.AddHeader("content-disposition", "attachment;filename=" + csvfilename + ".xlsx");
        //                Response.BinaryWrite(pck.GetAsByteArray());
        //                Response.End();
        //            }
        //        }
        //        else if (decryptedReportName == "Over 30 days less than 90 days Standing Report")
        //        {
        //            csvfilename = "Over 30 days less than 90 days Standing Report";
        //            Query = "select ScaffRef,WONumber,Location,BuildingNo,FloorLevel,Bearing,Replace(Replace(Description,Char(13),' '),char(10),' ') as Description,AssetNo,DateTagged,DaysSinceLastInsp,convert(decimal(18, 2),((tonnage)/1000)) as Tonnage,DesignType,MaterialType,HireDaysToDate,Length,Width,Height,Status from VW_TblRegister where clientid ='" + databaseCon.Decrypt(id) + "' and SiteLocId='" + siteid + "' and ('All'='" + Ownername + "' or ScaffoldOwner ='" + Ownername + "') and HireDaysToDate>30 and HireDaysToDate<=90 order by DateTagged,ScaffRef";
        //            DataTable dt3 = databaseCon.GetData(Query);
        //            using (ExcelPackage pck = new ExcelPackage())
        //            {
        //                string title = $"Site Location: {siteName}                                  Report Name: {csvfilename}                                      Printed On: {currentDate}";

        //                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet1");
        //                ws.Cells["A1:L1"].Merge = true;
        //                ws.Cells["A1:L1"].Value = title;
        //                ws.Cells["A1:L1"].Style.Font.Bold = true;
        //                ws.Cells["A1:L1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

        //                AddDataToSheet(ws, dt3, title, true);

        //                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //                Response.AddHeader("content-disposition", "attachment;filename=" + csvfilename + ".xlsx");
        //                Response.BinaryWrite(pck.GetAsByteArray());
        //                Response.End();
        //            }
        //        }
        //        else if (decryptedReportName == "Over 30 days Standing Report")
        //        {
        //            csvfilename = "Over 30 days Standing Report";
        //            Query = "select ScaffRef,WONumber,Location,BuildingNo,FloorLevel,Bearing,Replace(Replace(Description,Char(13),' '),char(10),' ') as Description,AssetNo,DateTagged,DaysSinceLastInsp,convert(decimal(18, 2),((tonnage)/1000)) as Tonnage,DesignType,MaterialType,HireDaysToDate,Length,Width,Height,Status from VW_TblRegister where clientid ='" + databaseCon.Decrypt(id) + "' and SiteLocId='" + siteid + "' and ('All'='" + Ownername + "' or ScaffoldOwner ='" + Ownername + "') and HireDaysToDate>30 order by DateTagged,ScaffRef";
        //            DataTable dt4 = databaseCon.GetData(Query);
        //            using (ExcelPackage pck = new ExcelPackage())
        //            {
        //                string title = $"Site Location: {siteName}                                  Report Name: {csvfilename}                                      Printed On: {currentDate}";

        //                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet1");
        //                ws.Cells["A1:L1"].Merge = true;
        //                ws.Cells["A1:L1"].Value = title;
        //                ws.Cells["A1:L1"].Style.Font.Bold = true;
        //                ws.Cells["A1:L1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

        //                AddDataToSheet(ws, dt4, title, true);

        //                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //                Response.AddHeader("content-disposition", "attachment;filename=" + csvfilename + ".xlsx");
        //                Response.BinaryWrite(pck.GetAsByteArray());
        //                Response.End();
        //            }
        //        }
        //        else if (decryptedReportName == "ALL Current Standing Scaffolds Report")
        //        {
        //            csvfilename = "ALL Current Standing Scaffolds Report";
        //            Query = "select ScaffRef,WONumber,Location,BuildingNo,FloorLevel,Bearing,Replace(Replace(Description,Char(13),' '),char(10),' ') as Description,AssetNo,DateTagged,DaysSinceLastInsp,convert(decimal(18, 2),((tonnage)/1000)) as Tonnage,DesignType,MaterialType,HireDaysToDate,Length,Width,Height,Status from VW_TblRegister where clientid ='" + databaseCon.Decrypt(id) + "' and SiteLocId='" + siteid + "' and ('All'='" + Ownername + "' or ScaffoldOwner ='" + Ownername + "') and HireDaysToDate is not null order by DateTagged,ScaffRef";
        //            DataTable dt5 = databaseCon.GetData(Query);
        //            using (ExcelPackage pck = new ExcelPackage())
        //            {
        //                string title = $"Site Location: {siteName}                                  Report Name: {csvfilename}                                      Printed On: {currentDate}";

        //                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet1");
        //                ws.Cells["A1:L1"].Merge = true;
        //                ws.Cells["A1:L1"].Value = title;
        //                ws.Cells["A1:L1"].Style.Font.Bold = true;
        //                ws.Cells["A1:L1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //                ws.Cells[ws.Dimension.Address].AutoFitColumns();
        //                AddDataToSheet(ws, dt5, title, true);

        //                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //                Response.AddHeader("content-disposition", "attachment;filename=" + csvfilename + ".xlsx");
        //                Response.BinaryWrite(pck.GetAsByteArray());
        //                Response.End();
        //            }
        //        }

        //        //DataTable dt6 = databaseCon.GetData(Query);

        //        //Response.ContentType = "Application/x-msexcel";
        //        //Response.AddHeader("content-disposition", "attachment;filename=" + csvfilename + ".csv");
        //        //Response.Write(csvfile(dt6));
        //        //Response.End();
        //    }
        //}


        public void ExportToCsv_Click(string UID, string id, string site, string Owner, string availablereportname, string btn, string SiteLocId)
        {
           
            
            // Decrypt Owner for Special Character like C&I
            string Ownername = databaseCon.Decrypt(Owner).Replace("'", "''").Trim();
            var siteid = databaseCon.Decrypt(site);
            var Clientid = databaseCon.Decrypt(id);
            // var SiteLocid = SiteLocId;

            string decryptedReportName = databaseCon.Decrypt(Request.QueryString["availablereportname"]);
            string siteName = "SELECT SiteLocation FROM TblSiteLocation WHERE ClientID =" + databaseCon.Decrypt(id) + "AND SiteLocId = '" + siteid + "'";
            DataTable dt0 = databaseCon.GetData(siteName);
            siteName = dt0.Rows[0]["SiteLocation"].ToString();
            string currentDate = DateTime.Now.ToString("dd-MMM-yyyy");
            string Query = "EXEC Pro_ExportToCsv @decryptedReportName = '" + decryptedReportName + "', @clientID = '" + Clientid + "', @siteid = '" + siteid + "', @Ownername = '" + Ownername + "', @Index = '" + "First" + "', @Days = '" + "0" + "'";

            DataTable dt = databaseCon.GetData(Query);
            using (ExcelPackage pck = new ExcelPackage())
            {
                string title = $"Site Location: {siteName}                                  Report Name: {decryptedReportName}                                      Printed On: {currentDate}";

                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet1");
                ws.Cells["A1:L1"].Merge = true;
                ws.Cells["A1:L1"].Value = title;
                ws.Cells["A1:L1"].Style.Font.Bold = true;
                ws.Cells["A1:L1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                AddDataToSheet(ws, dt, title, true);

                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=" + decryptedReportName + ".xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();
            }
        }

        //private string Csvfile(DataTable dt)
        //{
        //    StringBuilder sb = new StringBuilder();

        //    IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().Select(column => column.ColumnName);
        //    sb.AppendLine(string.Join(",", columnNames));

        //    foreach (DataRow row in dt.Rows)
        //    {
        //        IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
        //        sb.AppendLine(string.Join(",", fields));
        //    }

        //    return sb.ToString();
        //}
        public void ExportToCsvWeekly_Click(string UID, string id, string site, string Owner, string availablereportname, string btn)
        {
            var csvfilename = "Last 7 Day Activity Report";
            string Query1 = "";
            string Query2 = "";
            string Ownername = databaseCon.Decrypt(Owner).Replace("'", "''").Trim();
            var Clientid = databaseCon.Decrypt(id);
            var siteid = databaseCon.Decrypt(site);
            string decryptedReportName = databaseCon.Decrypt(Request.QueryString["availablereportname"]);

            Query1 = "EXEC Pro_ExportToCsv @decryptedReportName = '" + decryptedReportName + "', @clientID = '" + Clientid + "', @siteid = '" + siteid + "', @Ownername = '" + Ownername + "', @Index = '" + "First" + "', @Days = '" + "0" + "'";

           Query2 = "EXEC Pro_ExportToCsv @decryptedReportName = '" + decryptedReportName + "', @clientID = '" + Clientid + "', @siteid = '" + siteid + "', @Ownername = '" + Ownername + "', @Index = '" + "Second" + "', @Days = '" + "0" + "'";


            DataTable dt1 = databaseCon.GetData(Query1);
            DataTable dt2 = databaseCon.GetData(Query2);
            string siteNameQuery = "SELECT SiteLocation FROM TblSiteLocation WHERE ClientID =" + databaseCon.Decrypt(id) + " AND SiteLocId = '" + siteid + "'";
            DataTable dt3 = databaseCon.GetData(siteNameQuery);
            string siteName = dt3.Rows[0]["SiteLocation"].ToString();

            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet1 = excelPackage.Workbook.Worksheets.Add("Built in last 7 days");
                ExcelWorksheet worksheet2 = excelPackage.Workbook.Worksheets.Add("Dismantled in last 7 days");
                string currentDate = DateTime.Now.ToString("dd/MM/yyyy");
                string title1 = $"Site Location: {siteName}                                  ReportName: Built in last 7 days                                     Printed On: {currentDate}";
                string title2 = $"Site Location: {siteName}                                  ReportName: Dismantled in last 7 days                                     Printed On: {currentDate}";
                worksheet1.Cells["A1:L1"].Merge = true;
                worksheet1.Cells["A1:L1"].Value = title1;
                worksheet1.Cells["A1:L1"].Style.Font.Bold = true;
                worksheet1.Cells["A1:L1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                worksheet2.Cells["A1:L1"].Merge = true;
                worksheet2.Cells["A1:L1"].Value = title2;
                worksheet2.Cells["A1:L1"].Style.Font.Bold = true;
                worksheet2.Cells["A1:L1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                AddDataToSheet(worksheet1, dt1, title1, true);
                AddDataToSheet(worksheet2, dt2, title2, true);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=" + csvfilename + ".xlsx");
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.End();
            }
        }

        public void ExportToCsvDateofDismantleReport_Click(string UID, string id, string site, string Owner, string availablereportname, string btn)
        {
            var csvfilename = "";
            // string Query1 = "";
            string Ownername = databaseCon.Decrypt(Owner).Replace("'", "''").Trim();
            var siteid = databaseCon.Decrypt(site);
            var Id = databaseCon.Decrypt(id);
            string decryptedReportName = databaseCon.Decrypt(Request.QueryString["availablereportname"]);
            csvfilename = "Date_of_Dismantle_Entered_By";

            string Query = "EXEC Pro_ExportToCsv @decryptedReportName = '" + csvfilename + "', @clientID = '" + Id + "', @siteid = '" + siteid + "', @Ownername = '" + Ownername + "', @Index = '" + "First" + "', @Days = '" + "0" + "'";

            //    Query1 = $@"
            //SELECT 
            //    ScaffRef AS [Scaf Ref #],
            //    ClientScaffRef AS [Client Scaf Ref],  
            //    WONumber AS [WO No.], 
            //    SiteLocation AS [Site], 
            //    Location, 
            //    BuildingNo AS [Building/Module No.], 
            //    FloorLevel AS [Floor Level], 
            //    Bearing,
            //    Description AS [Reason For Access.],
            //    AssetNo AS [Asset # /Func. Loc], 
            //    DateTagged AS [Date Built], 
            //    CASE 
            //        WHEN DateOfDismantle IS NULL OR DateOfDismantle = '1900-01-01' 
            //        THEN ''
            //        ELSE CONVERT(varchar, DateOfDismantle, 121)
            //    END AS [Date Dismantled], 
            //    CASE 
            //        WHEN DateOfDismantle IS NULL OR DateOfDismantle = '1900-01-01' 
            //        THEN '' 
            //        ELSE ISNULL(UpdatedBy, ISNULL(CreatedBy, '')) 
            //    END AS [Date Dismantled Entered by]
            //FROM 
            //    dbo.VW_TblRegister
            //WHERE 
            //    ClientID = '{Id}' AND 
            //    SiteLocId = '{siteid}' AND 
            //    ('All' = 'All' OR ScaffoldOwner = 'All') AND
            //    isnull(DateOfDismantle,'1900-01-01')!='1900-01-01'
            //ORDER BY 
            //    ScaffRef";

            DataTable dt1 = databaseCon.GetData(Query);

            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet1 = excelPackage.Workbook.Worksheets.Add("Date of Dismantle Entered By");

                // Add a title row with merged cells
                // string siteLocation = dt1.Rows.Count > 0 ? dt1.Rows[0]["Site"].ToString() : "";
                string siteName = "SELECT SiteLocation FROM TblSiteLocation WHERE ClientID =" + Id + "AND SiteLocId = '" + siteid + "'";
                DataTable dt2 = databaseCon.GetData(siteName);
                siteName = dt2.Rows[0]["SiteLocation"].ToString();
                string currentDate = DateTime.Now.ToString("dd/MM/yyyy");
                string title = $"Site Location: {siteName}                                  ReportName: 'Date of Dismantle' Entered By                                      Printed On: {currentDate}";
                worksheet1.Cells["A1:L1"].Merge = true;
                worksheet1.Cells["A1:L1"].Value = title;
                worksheet1.Cells["A1:L1"].Style.Font.Bold = true;
                worksheet1.Cells["A1:L1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                // Add the data to the sheet
                AddDataToSheet(worksheet1, dt1, title, true);

                // Set response for file download
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=" + csvfilename + ".xlsx");
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.End();
            }
        }

        public void ExportToCsvBuildDismantledByReport_Click(string UID, string id, string site, string Owner, string availablereportname, string btn)
        {
            var csvfilename = "";
           // string Query1 = "";
            string Ownername = databaseCon.Decrypt(Owner).Replace("'", "''").Trim();
            var siteid = databaseCon.Decrypt(site);
            var Id = databaseCon.Decrypt(id);
            string decryptedReportName = databaseCon.Decrypt(Request.QueryString["availablereportname"]);
            //csvfilename = "Date_of_Dismantle_Entered_By";
            csvfilename = "Build_&_Dismantled_By_Report";

            string Query = "EXEC Pro_ExportToCsv @decryptedReportName = '" + csvfilename + "', @clientID = '" + Id + "', @siteid = '" + siteid + "', @Ownername = '" + Ownername + "', @Index = '" + "First" + "', @Days = '" + "0" + "'";

            //    Query1 = $@"
            //SELECT 
            //    ScaffRef AS [Scaf Ref #],
            //    ClientScaffRef AS [Client Scaf Ref],  
            //    WONumber AS [WO No.], 
            //    SiteLocation AS [Site], 
            //    Location, 
            //    BuildingNo AS [Building/Module No.], 
            //    FloorLevel AS [Floor Level], 
            //    Bearing,
            //    Description AS [Reason For Access.],
            //    AssetNo AS [Asset # /Func. Loc], 
            //    DateTagged AS [Date Built], 
            //    CASE 
            //        WHEN DateOfDismantle IS NULL OR DateOfDismantle = '1900-01-01' 
            //        THEN ''
            //        ELSE CONVERT(varchar, DateOfDismantle, 121)
            //    END AS [Date Dismantled], 
            //    CASE 
            //        WHEN DateOfDismantle IS NULL OR DateOfDismantle = '1900-01-01' 
            //        THEN '' 
            //        ELSE ISNULL(UpdatedBy, ISNULL(CreatedBy, '')) 
            //    END AS [Date Dismantled Entered by]
            //FROM 
            //    dbo.VW_TblRegister
            //WHERE 
            //    ClientID = '{Id}' AND 
            //    SiteLocId = '{siteid}' AND 
            //    ('All' = 'All' OR ScaffoldOwner = 'All') AND
            //    isnull(DateOfDismantle,'1900-01-01')!='1900-01-01'
            //ORDER BY 
            //    ScaffRef";

            DataTable dt1 = databaseCon.GetData(Query);

            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet1 = excelPackage.Workbook.Worksheets.Add("Build & Dismantled By Report");

                // Add a title row with merged cells
                // string siteLocation = dt1.Rows.Count > 0 ? dt1.Rows[0]["Site"].ToString() : "";
                string siteName = "SELECT SiteLocation FROM TblSiteLocation WHERE ClientID =" + Id + "AND SiteLocId = '" + siteid + "'";
                DataTable dt2 = databaseCon.GetData(siteName);
                siteName = dt2.Rows[0]["SiteLocation"].ToString();
                string currentDate = DateTime.Now.ToString("dd/MM/yyyy");
                string title = $"Site Location: {siteName}                                  ReportName: Build & Dismantled By Report                                      Printed On: {currentDate}";
                worksheet1.Cells["A1:L1"].Merge = true;
                worksheet1.Cells["A1:L1"].Value = title;
                worksheet1.Cells["A1:L1"].Style.Font.Bold = true;
                worksheet1.Cells["A1:L1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                // Add the data to the sheet
                AddDataToSheet(worksheet1, dt1, title, true);

                // Set response for file download
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=" + csvfilename + ".xlsx");
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.End();
            }
        }



        //public void ExportToCsvJobsRequiringIsolation_Click(string UID, string id, string site, string Owner, string availablereportname, string btn)
        //{
        //    var csvfilename = "";
        //    string Query1 = "";
        //    string Query2 = "";
        //    //var Ownername = Owner;
        //    //Added on 24-04-2024 for Decrypt Owner for Speacial Character like C&I
        //    string Ownername = databaseCon.Decrypt(Owner).Replace("'", "''").Trim();
        //    var siteid = site;

        //    csvfilename = "Jobs Requiring Isolation Report.";

        //    Query1 = "SELECT ScaffRef as 'Scaff Ref #', CASE WHEN ChkRequiresIsolation = 1 THEN 'Yes' ELSE 'No' END as 'Requires Isolation', Location, BuildingNo as 'Building / Module No', FloorLevel as 'Floor Level', Bearing, Description as 'Reason for Access.(Provide Detailed Description)', AssetNo as 'Asset # / Func. Loc', FORMAT(DateTagged, 'dd-MMM-yyyy') AS 'Date Built',  FORMAT(DateOFDismantle, 'dd-MMM-yyyy') AS 'Date OF Dismantle', DaysSinceLastInsp as 'Days Since Last Insp', CONVERT(decimal(18, 2), (tonnage / 1000)) as Tonnage, DesignType as 'Design Type', MaterialType as 'Material Type', HireDaysToDate as 'Standing Days to Date', Length, Width, Height as 'Height (TWP)', Status from VW_TblRegister where clientid ='" + databaseCon.Decrypt(id) + "' and SiteLocId='" + databaseCon.Decrypt(siteid) + "' and ('All'='" + Ownername + "' or ScaffoldOwner ='" + Ownername + "') and ChkRequiresIsolation=1 and (DateOfDismantle='1900-01-01' or DateOfDismantle='' or DateOfDismantle is null) order by ScaffRef,DateTagged";


        //    DataTable dt1 = databaseCon.GetData(Query1);

        //    using (ExcelPackage excelPackage = new ExcelPackage())
        //    {

        //        ExcelWorksheet worksheet1 = excelPackage.Workbook.Worksheets.Add("Jobs Requiring Isolation Report");

        //        AddDataToSheet(worksheet1, dt1, "Jobs Requiring Isolation Report", true);

        //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        Response.AddHeader("content-disposition", "attachment;filename=" + csvfilename + ".xlsx");
        //        Response.BinaryWrite(excelPackage.GetAsByteArray());
        //        Response.End();
        //    }
        //}

        public void ExportToCsvJobsRequiringIsolation_Click(string UID, string id, string site, string Owner, string availablereportname, string btn)
        {
            var csvfilename = "";
            // string Query1 = "";
            string Ownername = databaseCon.Decrypt(Owner).Replace("'", "''").Trim();
            var siteid = databaseCon.Decrypt(site);
            var Id = databaseCon.Decrypt(id);
            string decryptedReportName = databaseCon.Decrypt(Request.QueryString["availablereportname"]);
            csvfilename = "Jobs Requiring Isolation Report";

            string Query = "EXEC Pro_ExportToCsv @decryptedReportName = '" + csvfilename + "', @clientID = '" + Id + "', @siteid = '" + siteid + "', @Ownername = '" + Ownername + "', @Index = '" + "First" + "', @Days = '" + "0" + "'";

            DataTable dt1 = databaseCon.GetData(Query);

            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet1 = excelPackage.Workbook.Worksheets.Add("Jobs Requiring Isolation Report");

                // Add a title row with merged cells
                // string siteLocation = dt1.Rows.Count > 0 ? dt1.Rows[0]["Site"].ToString() : "";
                string siteName = "SELECT SiteLocation FROM TblSiteLocation WHERE ClientID =" + Id + "AND SiteLocId = '" + siteid + "'";
                DataTable dt2 = databaseCon.GetData(siteName);
                siteName = dt2.Rows[0]["SiteLocation"].ToString();
                string currentDate = DateTime.Now.ToString("dd/MM/yyyy");
                string title = $"Site Location: {siteName}                                  ReportName: 'Jobs Requiring Isolation Report'                                    Printed On: {currentDate}";
                worksheet1.Cells["A1:L1"].Merge = true;
                worksheet1.Cells["A1:L1"].Value = title;
                worksheet1.Cells["A1:L1"].Style.Font.Bold = true;
                worksheet1.Cells["A1:L1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                // Add the data to the sheet
                AddDataToSheet(worksheet1, dt1, title, true);

                // Set response for file download
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=" + csvfilename + ".xlsx");
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.End();
            }
        }


        //public void ExportToCsvOtyAndTonnage_Click(string UID, string id, string site, string Owner, string availablereportname, string btn)
        //{
        //    var csvfilename = "";
        //    string Query1 = "";
        //    string Ownername = databaseCon.Decrypt(Owner).Replace("'", "''").Trim();
        //    var siteid = databaseCon.Decrypt(site);
        //    var Id = databaseCon.Decrypt(id);
        //    csvfilename = "Qty And Tonnage By Workgroup (Standing Jobs)";
        //    string decryptedReportName = databaseCon.Decrypt(Request.QueryString["availablereportname"]);
        //    //Query1 = "EXEC Pro_QtyAndTonnageStandingReport '" + Id + "','" + siteid + "'";
        //    string Query = "EXEC Pro_ExportToCsv @decryptedReportName = '" + decryptedReportName + "', @clientID = '" + Id + "', @siteid = '" + siteid + "', @Ownername = '" + Ownername + "', @Index = '" + "First" + "', @Days = '" + "0" + "'";


        //    DataTable dt1 = databaseCon.GetData(Query1);

        //    using (ExcelPackage excelPackage = new ExcelPackage())
        //    {
        //        ExcelWorksheet worksheet1 = excelPackage.Workbook.Worksheets.Add("Qty And Tonnage By Workgroup (Standing Jobs)");

        //        // Add a title row with merged cells
        //        // string siteLocation = dt1.Rows.Count > 0 ? dt1.Rows[0]["Site"].ToString() : "";
        //        string siteName = "SELECT SiteLocation FROM TblSiteLocation WHERE ClientID =" + Id + "AND SiteLocId = '" + siteid + "'";
        //        DataTable dt2 = databaseCon.GetData(siteName);
        //        siteName = dt2.Rows[0]["SiteLocation"].ToString();
        //        string currentDate = DateTime.Now.ToString("dd/MM/yyyy");
        //        string title = $"Site Location: {siteName}                                  ReportName: Qty And Tonnage By Workgroup (Standing Jobs)                                     Printed On: {currentDate}";
        //        worksheet1.Cells["A1:L1"].Merge = true;
        //        worksheet1.Cells["A1:L1"].Value = title;
        //        worksheet1.Cells["A1:L1"].Style.Font.Bold = true;
        //        worksheet1.Cells["A1:L1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

        //        // Add the data to the sheet
        //        AddDataToSheet(worksheet1, dt1, title, true);

        //        // Set response for file download
        //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        Response.AddHeader("content-disposition", "attachment;filename=" + csvfilename + ".xlsx");
        //        Response.BinaryWrite(excelPackage.GetAsByteArray());
        //        Response.End();
        //    }
        //}

        public void ExportToCsvOtyAndTonnage_Click(string UID, string id, string site, string Owner, string availablereportname, string btn)
        {
            var csvfilename = "";
            string Query1 = "";
            string Query2 = "";
            string Ownername = databaseCon.Decrypt(Owner).Replace("'", "''").Trim();
            var siteid = databaseCon.Decrypt(site);
            var Id = databaseCon.Decrypt(id);
            csvfilename = "Qty And Tonnage By Workgroup (Standing Jobs)";


            Query1 = "EXEC Pro_ExportToCsv @decryptedReportName = '" + csvfilename + "', @clientID = '" + Id + "', @siteid = '" + siteid + "', @Ownername = '" + Ownername + "', @Index = '" + "First" + "', @Days = '" + "0" + "'";

            Query2 = "EXEC Pro_ExportToCsv @decryptedReportName = '" + csvfilename + "', @clientID = '" + Id + "', @siteid = '" + siteid + "', @Ownername = '" + Ownername + "', @Index = '" + "Second" + "', @Days = '" + "0" + "'";

            DataTable dt1 = databaseCon.GetData(Query1);
            DataTable dt2 = databaseCon.GetData(Query2);

            string siteName = "SELECT SiteLocation FROM TblSiteLocation WHERE ClientID =" + Id + "AND SiteLocId = '" + siteid + "'";
            DataTable dt3 = databaseCon.GetData(siteName);
            siteName = dt3.Rows[0]["SiteLocation"].ToString();

            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet1 = excelPackage.Workbook.Worksheets.Add("Qty And Tonnage By Workgroup");
                ExcelWorksheet worksheet2 = excelPackage.Workbook.Worksheets.Add("Site Tonnage Summary");
                string currentDate = DateTime.Now.ToString("dd/MM/yyyy");
                string title1 = $"Site Location: {siteName}                                  ReportName: {csvfilename}                                     Printed On: {currentDate}";
                string title2 = $"Site Location: {siteName}                                  ReportName: Site Tonnage Summary                                     Printed On: {currentDate}";
                worksheet1.Cells["A1:L1"].Merge = true;
                worksheet1.Cells["A1:L1"].Value = title1;
                worksheet1.Cells["A1:L1"].Style.Font.Bold = true;
                worksheet1.Cells["A1:L1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet2.Cells["A1:L1"].Merge = true;
                worksheet2.Cells["A1:L1"].Value = title1;
                worksheet2.Cells["A1:L1"].Style.Font.Bold = true;
                worksheet2.Cells["A1:L1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                // Add the data to the sheet
                AddDataToSheet(worksheet1, dt1, title1, true);
                AddDataToSheet(worksheet2, dt2, title2, true);

                // Set response for file download
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=" + csvfilename + ".xlsx");
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.End();
            }
        }
        public void ExportToCsvnStandingReportsDays_Click(string UID, string id, string site, string Owner, string availablereportname, string btn, string Days)
        {
            var csvfilename = "Last " + Days + " Day Activity Report";
            string Query1 = "";
            string Query2 = "";
            string Ownername = databaseCon.Decrypt(Owner).Replace("'", "''").Trim();
            var siteid = databaseCon.Decrypt(site);
            var Id = databaseCon.Decrypt(id);
           
            string decryptedReportName = databaseCon.Decrypt(Request.QueryString["availablereportname"]);
           
             Query1 = "EXEC Pro_ExportToCsv @decryptedReportName = '" + decryptedReportName + "', @clientID = '" + Id + "', @siteid = '" + siteid + "', @Ownername = '" + Ownername + "', @Index = '" + "First" + "', @Days = '" + Days + "'";
             Query2 = "EXEC Pro_ExportToCsv @decryptedReportName = '" + decryptedReportName + "', @clientID = '" + Id + "', @siteid = '" + siteid + "', @Ownername = '" + Ownername + "', @Index = '" + "Second" + "', @Days = '" + Days + "'";

            //Query1 = "SELECT ScaffRef as 'Scaff Ref #', WONumber as 'WO #', Location, BuildingNo as 'Building / Module No', FloorLevel as 'Floor Level', Bearing, Description as 'Reason For Access', AssetNo as 'Asset # / Func. Loc', FORMAT(DateTagged, 'dd-MMM-yyyy') AS 'Date Built',  FORMAT(DateOFDismantle, 'dd-MMM-yyyy') AS 'Date OF Dismantle', DaysSinceLastInsp as 'Days Since Last Insp', CONVERT(decimal(18, 2), (tonnage / 1000)) as Tonnage, DesignType as 'Design Type', MaterialType as 'Material Type', HireDaysToDate as 'Hire Days To Date', Length, Width, Height as 'Height (TWP)', Status from VW_TblRegister where clientid ='" + databaseCon.Decrypt(id) + "' and SiteLocId='" + databaseCon.Decrypt(siteid) + "' and ('All'='" + Ownername + "' or ScaffoldOwner ='" + Ownername + "') and isnull(DateTagged,'1900-01-01') > convert(date,DATEADD(day, -" + Days + ", GETDATE())) order by DateTagged,ScaffRef";

            //Query2 = "SELECT ScaffRef as 'Scaff Ref #', WONumber as 'WO #', Location, BuildingNo as 'Building / Module No', FloorLevel as 'Floor Level', Bearing, Description as 'Reason For Access', AssetNo as 'Asset # / Func. Loc', FORMAT(DateTagged, 'dd-MMM-yyyy') AS 'Date Built',  FORMAT(DateOFDismantle, 'dd-MMM-yyyy') AS 'Date OF Dismantle', DaysSinceLastInsp as 'Days Since Last Insp', CONVERT(decimal(18, 2), (tonnage / 1000)) as Tonnage, DesignType as 'Design Type', MaterialType as 'Material Type', HireDaysToDate as 'Hire Days To Date', Length, Width, Height as 'Height (TWP)', Status from VW_TblRegister where clientid ='" + databaseCon.Decrypt(id) + "' and SiteLocId='" + databaseCon.Decrypt(siteid) + "' and ('All'='" + Ownername + "' or ScaffoldOwner ='" + Ownername + "') and isnull(DateOfDismantle,'1900-01-01') > convert(date,DATEADD(day,-" + Days + ", GETDATE())) order by DateTagged,ScaffRef";

            DataTable dt1 = databaseCon.GetData(Query1);
            DataTable dt2 = databaseCon.GetData(Query2);
            string siteNameQuery = "SELECT SiteLocation FROM TblSiteLocation WHERE ClientID =" + Id + " AND SiteLocId = '" + siteid + "'";
            DataTable dt3 = databaseCon.GetData(siteNameQuery);
            string siteName = dt3.Rows[0]["SiteLocation"].ToString();

            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                string currentDate = DateTime.Now.ToString("dd/MM/yyyy");
                ExcelWorksheet worksheet1 = excelPackage.Workbook.Worksheets.Add("Built in last " + Days + " days ");
                ExcelWorksheet worksheet2 = excelPackage.Workbook.Worksheets.Add("Dismantled in last " + Days + " days  ");
                string title1 = $"Site Location: {siteName}                                  ReportName: Built in last " + Days + " days                                     Printed On: "+currentDate+"";
                string title2 = $"Site Location: {siteName}                                  ReportName: Dismantled in last " + Days + " days                                     Printed On: "+currentDate+"";
                worksheet1.Cells["A1:L1"].Merge = true;
                worksheet1.Cells["A1:L1"].Value = title1;
                worksheet1.Cells["A1:L1"].Style.Font.Bold = true;
                worksheet1.Cells["A1:L1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                AddDataToSheet(worksheet1, dt1, title1, true);
                worksheet2.Cells["A1:L1"].Merge = true;
                worksheet2.Cells["A1:L1"].Value = title2;
                worksheet2.Cells["A1:L1"].Style.Font.Bold = true;
                worksheet2.Cells["A1:L1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; 
                AddDataToSheet(worksheet2, dt2, title2, true);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=" + csvfilename + ".xlsx");
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.End();
            }
        }

        void AddDataToSheet(ExcelWorksheet worksheet, DataTable dt, string sheetName, bool includeHeadings)
        {
            worksheet.Cells["A1"].Value = sheetName;
            if (includeHeadings)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    worksheet.Cells[2, i + 1].Value = dt.Columns[i].ColumnName;
                    worksheet.Cells[2, i + 1].Style.Font.Bold = true;
                }
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string cellValue = dt.Rows[i][j].ToString();
                    if (dt.Columns[j].ColumnName == "Date Built" ||
                        dt.Columns[j].ColumnName == "Date Dismantled" ||
                        dt.Columns[j].ColumnName == "Date Dismantled Entered by" ||
                        dt.Columns[j].ColumnName == "DateTagged")
                    {
                        DateTime dateValue;
                        if (DateTime.TryParse(cellValue, out dateValue))
                        {
                            if (dateValue != DateTime.Parse("1900-01-01"))
                            {
                                cellValue = dateValue.ToString("dd-MMM-yyyy");
                            }
                            else
                            {
                                cellValue = "";
                            }
                        }
                    }

                    worksheet.Cells[i + (includeHeadings ? 3 : 2), j + 1].Value = cellValue;
                }
            }
            worksheet.Cells.AutoFitColumns();
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
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
                            if (column.ColumnName == "DateTagged")
                            {
                                sbldr.Append(Convert.ToDateTime(row[column]).ToString("dd-MMM-yyyy") + ',');
                            }
                            else
                            {
                                sbldr.Append(row[column].ToString() + ',');
                            }
                        }
                    }
                    sbldr.Append("\r\n");
                }
            }
            return sbldr.ToString();
        }

        public class BoxAvailableViewModel
        {
            public Nullable<int> ReportID { get; set; }
            public string ReportName { get; set; }
            public string ReportDesc { get; set; }
            public Nullable<int> SiteLocID { get; set; }
            public string SiteLocation { get; set; }
            public string ScaffoldOwner { get; set; }
        }
    }
}