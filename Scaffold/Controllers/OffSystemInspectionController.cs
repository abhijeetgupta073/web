using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.Services.Description;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNet.Identity;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;
using static Scaffold.Controllers.OfflineOffSystemInspectionController;

namespace Scaffold.Controllers
{
    public class OffSystemInspectionController : Controller
    {
        ScaffoldEntities dbcontext = new ScaffoldEntities();
        // GET: OffSystemInspection
        public ActionResult OffSystemInspection()
        {
            string query = "select distinct UserId from tblUser where IsInspector='true' and userid= '" + databaseCon.Decrypt(Request.QueryString["UID"]) + "' and clientid='" + databaseCon.Decrypt(Request.QueryString["id"]) + "'";
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["UserId"].ToString() != "0")
                {
                    //  ddlddlInspectedByadd.SelectedValue = dt.Rows[0]["UserId"].ToString();
                    ViewBag.ddlddlInspectedByadd = dt.Rows[0]["UserId"].ToString();
                }
            }

            return View();
        }
        public ActionResult BindallDropdown(string id)
        {

            var ddlsites = dbcontext.Database.SqlQuery<DropDownViewModel>("select distinct SiteLocID,SiteLocation from tblSiteLocation where SiteLocation <>'' and SiteLocation is not null and OffSystemInspection=1 and  ClientId='" + databaseCon.Decrypt(id) + "' order by SiteLocation").ToList();

            var ddlInspectedBy = dbcontext.Database.SqlQuery<DropDownViewModel>("select distinct firstName+ ' '+LastName as name,UserId,LicenceNo from tblUser where IsInspector='true' and Status='Active' and clientid='" + databaseCon.Decrypt(id) + "' Order by name");

            return Json(new { ddlsites = ddlsites, ddlInspectedBy = ddlInspectedBy }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult dropdownlocation(string id, string SiteID)
        {
            string query;
            DataTable dt;
            query = "select distinct area,areaid from tblArea where ClientID='" + databaseCon.Decrypt(id) + "' ";
            if (SiteID != "" && SiteID != null)
            {
                query = query + "  and(SiteLocId = '" + SiteID + "')";
            }
            query = query + " order by area";

            var getData = dbcontext.Database.SqlQuery<DropDownViewModel>(query);
            return Json(getData, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult dropdownBuildiing(string id, string SiteID, string LocationID)
        //    {
        //    string query = "";
        //    if (LocationID == "All")
        //        {
        //        query = "select buildingid, buildingno  from tblbuilding where BuildingStatus = 'Active' and ClientID = '" + id + "'  and Areaid in (select AreaID from tblarea where AreaStatus = 'Active' and SiteLocId = '" + SiteID + "') order by buildingno";
        //        }
        //    else
        //        {
        //        query = "select buildingid, buildingno  from tblbuilding where BuildingStatus = 'Active' and ClientID = '" + id + "'  and Areaid= '" + LocationID + "' order by buildingno";
        //        }

        //    var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>(query);
        //    return Json(GetData, JsonRequestBehavior.AllowGet);
        //    }

        public ActionResult dropdownBuilding(string id, string SiteID, string LocationID)
        {

            string query = "";
            if (LocationID == "")
            {
                //query = "select distinct buildingid, buildingno  from tblbuilding where ClientID = '" + databaseCon.Decrypt(id) + "' and Areaid in (select AreaID from tblarea where SiteLocId = '" + SiteID + "') order by buildingno";
                query = "select distinct concat(buildingno, ' (', area, ')') as buildingno,buildingid from (select distinct tblbuilding.buildingid, tblbuilding.buildingno, tblarea.area from tblbuilding join tblarea on tblbuilding.ClientID= tblarea.clientid and tblbuilding.areaid= tblarea.areaid and tblbuilding.areaid in (select AreaID from tblarea where SiteLocId = '" + SiteID + "') and tblbuilding.ClientID = '" + databaseCon.Decrypt(id) + "')subqueryforconcat order by buildingno";

            }
            else
            {
                //query = "select distinct buildingid, buildingno  from tblbuilding where ClientID = '" + databaseCon.Decrypt(id) + "'  and Areaid= '" + LocationID + "' order by buildingno";
                query = "select distinct concat(buildingno, ' (', area, ')') as buildingno,buildingid from (select distinct tblbuilding.buildingid, tblbuilding.buildingno, tblarea.area from tblbuilding join tblarea on tblbuilding.ClientID= tblarea.clientid and tblbuilding.areaid= tblarea.areaid and tblbuilding.areaid= '" + LocationID + "' and tblbuilding.ClientID = '" + databaseCon.Decrypt(id) + "')subqueryforconcat order by buildingno";
            }

            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>(query);
            return Json(GetData, JsonRequestBehavior.AllowGet);

        }
        public ActionResult dropdownSubOwner(string id, string SiteID, string LocationID, string FloorLevelId)
        {
            string query;
            DataTable dt;
            query = "select distinct ScaffoldOwner from tblRegister where ClientId = '" + databaseCon.Decrypt(id) + "' and ScaffoldOwner is not null and ScaffoldOwner!= ''" + "";
            if (SiteID != "" && SiteID != null)
            {
                query = query + "  and(SiteLocId = '" + SiteID + "')";
            }
            if (LocationID != "" && LocationID != null)
            {
                query = query + "  and(LocationId = '" + LocationID + "')";
            }
            if (FloorLevelId != "" && FloorLevelId != null)
            {
                query = query + "  and(FloorLevelId = '" + FloorLevelId + "')";
            }
            query = query + "Order by ScaffoldOwner";

            //dt = databaseCon.GetData(query);

            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>(query);

            return Json(GetData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult dropdownFloorLevel(string id, string SiteID, string LocationID, string[] BuildingId)
        {
            string buildingValues = BuildingId != null && BuildingId.Length > 0
                ? string.Join("','", BuildingId)
                : "";

            string query = "";
            query = "select distinct concat(floorlevel,' (',buildingno,case when buildingno is not null then '-' else '' end,location,')') as floorlevel, floorlevelid  from vw_tblregister where ClientID = '" + databaseCon.Decrypt(id) + "'";

            if (!string.IsNullOrEmpty(buildingValues))
            {
                query += " and BuildingId IN ('" + buildingValues + "')";
            }

            if (!string.IsNullOrEmpty(LocationID))
            {
                query += " and LocationId = '" + LocationID + "'";
            }

            if (!string.IsNullOrEmpty(SiteID))
            {
                query += " and SiteLocId = '" + SiteID + "'";
            }

            query += " Order by FloorLevel";

            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>(query);
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetOffSystemInspectionData(string id, string SiteID, string LocationID, string ddlSubOwner, string hdnTimeOffSet, string[] ddlBuilding, string FloorLevel, string WorkGroup, string Bearing)
        {
            string buildingValues = ddlBuilding != null ? string.Join(",", ddlBuilding) : "";
            string query = "exec [Pro_TBLREGISTER_GET] '','" + databaseCon.Decrypt(id);

            //if (ddlSitesLoc.SelectedValue != "")
            if (SiteID != "" && SiteID != null)
            {
                query = query + "','" + SiteID + "";

            }
            else
            {
                query = query + "','";

            }

            //if (ddlLocaSite.SelectedValue != "")
            if (LocationID != "" && LocationID != null)
            {
                query = query + "','" + LocationID + "";
            }

            else
            {
                query = query + "','";
            }



            if (ddlSubOwner != "" && ddlSubOwner != null)
            {
                query = query + "','','" + ddlSubOwner + "";
            }
            else
            {
                // query = query + "','";
                query = query + "','','";
            }

            //query = query + "','','1' ";
            //query = query + "','1'," + Convert.ToDouble((hdnTimeOffSet)) * (-1) + "";

            query = query + "','1'," + Convert.ToDouble((hdnTimeOffSet)) * (-1) + ",'";

            if (WorkGroup != "" && WorkGroup != null)
            {
                query = query + WorkGroup + "','','";
            }

            else
            {
                query = query + "','','";
            }

            if (buildingValues != "" && buildingValues != null)
            {
                query = query + "','" + buildingValues + "";
            }

            else
            {
                query = query + "','";
            }

            if (FloorLevel != "" && FloorLevel != null)
            {
                query = query + "','" + FloorLevel + "";
            }

            else
            {
                query = query + "','";
            }
            if (Bearing != "" && Bearing != null)
            {
                query = query + "','" + Bearing + "'";
            }

            else
            {
                query = query + "',''";
            }

            // Add this Line On 01 Aug 2020
            DataTable dt = databaseCon.GetData(query);

            var GetData = dbcontext.Database.SqlQuery<Pro_TBLREGISTER_GETViewModel>(query).ToList();
            //Commented on 26-07-2023 to avoid large data problem.
            //return Json(GetData, JsonRequestBehavior.AllowGet);
            var jsonResult = Json(GetData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        //public ActionResult GetOffSystemInspectionData(string id, string SiteID, string LocationID, string ddlSubOwner, string hdnTimeOffSet, string ddlBuilding, string FloorLevel, string ddlWorkgroup)
        //{

        //    string query = "exec [Pro_TBLREGISTER_GET] '','" + databaseCon.Decrypt(id);

        //    //if (ddlSitesLoc.SelectedValue != "")
        //    if (SiteID != "" && SiteID != null)
        //    {
        //        query = query + "','" + SiteID + "";

        //    }
        //    else
        //    {
        //        query = query + "','";

        //    }

        //    //if (ddlLocaSite.SelectedValue != "")
        //    if (LocationID != "" && LocationID != null)
        //    {
        //        query = query + "','" + LocationID + "";
        //    }

        //    else
        //    {
        //        query = query + "','";
        //    }



        //    if (ddlSubOwner != "" && ddlSubOwner != null)
        //    {
        //        query = query + "','','" + ddlSubOwner + "";
        //    }
        //    else
        //    {
        //        // query = query + "','";
        //        query = query + "','','";
        //    }

        //    //query = query + "','','1' ";
        //    //query = query + "','1'," + Convert.ToDouble((hdnTimeOffSet)) * (-1) + "";

        //    //query = query + "','1'," + Convert.ToDouble((hdnTimeOffSet)) * (-1) + ",'','";//Commented on 02-August-2022
        //    query = query + "','1'," + Convert.ToDouble((hdnTimeOffSet)) * (-1) + "";//Added on 02-August-2022
        //    //Added on 02-August-2022 start here
        //    if (ddlWorkgroup != "" && ddlWorkgroup != null)
        //    {
        //        query = query + ",'" + ddlWorkgroup + "','";
        //    }
        //    else
        //    {
        //        query = query + ",'','";
        //    }
        //    //Added on 02-August-2022 end here
        //    if (ddlBuilding != "" && ddlBuilding != null)
        //    {
        //        query = query + "','" + ddlBuilding + "";
        //    }

        //    else
        //    {
        //        query = query + "','";
        //    }

        //    if (FloorLevel != "" && FloorLevel != null)
        //    {
        //        query = query + "','" + FloorLevel + "'";
        //    }

        //    else
        //    {
        //        query = query + "',''";
        //    }

        //    // Add this Line On 01 Aug 2020
        //    DataTable dt = databaseCon.GetData(query);

        //    var GetData = dbcontext.Database.SqlQuery<Pro_TBLREGISTER_GETViewModel>(query).ToList();
        //    return Json(GetData, JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        public ActionResult btnSaveInspect_Click(List<OffSystemInspectionSaveViewModel> model, string Siteid, string ClientID)
        {
            bool OnlineInsp = true;
            bool complete = true;
            var Result = "";
            var Message = "";
            var LicenceNo = "";
            try
            {

                foreach (OffSystemInspectionSaveViewModel item in model)
                {

                    var Array = item;

                    var InspectionComments = Array.InspectionComments == null ? "" : Array.InspectionComments.Replace("'", "''").Trim();

                    string inspbyQuery = "select UserID, firstName+ ' '+LastName as name,LicenceNo from tblUser where IsInspector='true' and UserId='" + Array.InspectedBy + "' order by name";
                    DataTable inspDt = databaseCon.GetData(inspbyQuery);
                    if (inspDt.Rows.Count > 0)
                    {
                        LicenceNo = inspDt.Rows[0]["LicenceNo"].ToString();
                    }
                    string query = "exec Pro_TBLINSPECTION_SAVE   '0','" + Array.InspectionDate + "','" + Array.InspectedBy + "','" + "" + "','" + Array.RegisterID + "','" + databaseCon.Decrypt(Array.UID) + "','" + DateTime.Now + "','" + OnlineInsp + "','" + complete + "','','','','','','','','" + InspectionComments + "','true','" + Array.StatusConfirmed + "','" + Array.InspectionDate + "','" + LicenceNo + "','" + "" + "','" + databaseCon.Decrypt(Array.ClientID) + "'," + "0" + ",'" + databaseCon.Decrypt(Array.UID) + "'," + 0 + "," + 0 + "," + "1" + "";

                    DataTable Confirmdt = databaseCon.GetData(query);
                    if (Confirmdt.Rows.Count != 0)
                    {
                        if (Convert.ToInt32(Confirmdt.Rows[0][0]) > 0)
                        {
                            //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                            //art1.Style.Add("display", "block");
                            //alertlabel.Text = "<strong>Success ! </strong>" + Confirmdt.Rows[0][1].ToString();

                            Result = "Success";
                            Message = Confirmdt.Rows[0][1].ToString();
                        }
                        else if (Convert.ToInt32(Confirmdt.Rows[0][0]) == -1)
                        {
                            //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                            //art1.Style.Add("display", "block");
                            //alertlabel.Text = "<strong>Alert ! </strong>" + Confirmdt.Rows[0][1].ToString();

                            Result = "Failed";
                            Message = Confirmdt.Rows[0][1].ToString();
                        }
                        else
                        {
                            //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                            //art1.Style.Add("display", "block");
                            //alertlabel.Text = "<strong>Alert ! </strong>" + Confirmdt.Rows[0][1].ToString();

                            Result = "Failed";
                            Message = Confirmdt.Rows[0][1].ToString();
                        }


                    }
                    else
                    {
                        //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator ";
                        Result = "Failed";
                        Message = "Syntax Error. Contact administrator .";
                    }


                }


            }
            catch (Exception ex)
            {
                Result = "Failed";
                Message = ex.Message;
            }


            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult dropdownWorkGroup(string id, string SiteID, string LocationID, string FloorLevelId, string BuildingId)
        {
            string query;
            DataTable dt;
            query = "select Distinct WorkGroup,WorkGroupID  from tblWorkGroup where ClientID = '" + databaseCon.Decrypt(id) + "' and WorkGroup is not null and WorkGroup !=''" + "";
            if (SiteID != "" && SiteID != null)
            {
                query = query + "  and(SiteLocId = '" + SiteID + "')";
            }
            query = query + "Order by WorkGroup";

            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>(query);

            return Json(GetData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult dropdownBearing(string id, string SiteID, string LocationID, string FloorLevelId, string[] BuildingId, string Workgroup)
        {
            string query;
            string buildingValues = BuildingId != null && BuildingId.Length > 0
              ? string.Join("','", BuildingId)
              : "";
            DataTable dt;
            query = "select Distinct Bearing  from tblRegister where ClientID = '" + databaseCon.Decrypt(id) + "' and Bearing is not null and Bearing !=''" + "";
            if (SiteID != "" && SiteID != null)
            {
                query = query + "  and(SiteLocId = '" + SiteID + "')";
            }
            if (LocationID != "" && LocationID != null)
            {
                query = query + "  and(LocationId = '" + LocationID + "')";
            }
            if (FloorLevelId != "" && FloorLevelId != null)
            {
                query = query + "  and(FloorLevelId = '" + FloorLevelId + "')";
            }
            if (buildingValues != "" && buildingValues != null)
            {
                query = query + "  and(BuildingId = '" + buildingValues + "')";
            }
            if (Workgroup != "" && Workgroup != null)
            {
                query = query + "  and(Workgroup = '" + Workgroup + "')";
            }
            query = query + "Order by Bearing";

            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>(query);

            return Json(GetData, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult GoOfflineClick(string id, string UserId, string Siteid)
        //{

        //    string checkqueryForthisClienthaveAnyRegisterOnOffline = "select  SiteLocation , count('*')  as NoOfJobOffline from VW_TblRegister where IsOfflineCurrentForInspection = 1 and ClientID =" + databaseCon.Decrypt(id) + " group by SiteLocation having count('*') >1 ";
        //    var VarCheckqueryForthisClienthaveAnyRegisterOnOffline = databaseCon.GetData(checkqueryForthisClienthaveAnyRegisterOnOffline);

        //    //if ((int.Parse(VarCheckqueryForthisClienthaveAnyRegisterOnOffline.Rows[0][1].ToString()) <= 0))


        //    string checkquery = "select  count('x') from tblregister where IsOfflineCurrentForInspection =1 and clientid  = " + databaseCon.Decrypt(id) + " and SiteLocId = " + databaseCon.Decrypt(Siteid) + " and DateOfDismantle = '1900-01-01'  and DateTagged != '1900-01-01'";

        //    var CheckOfflinereg = databaseCon.GetData(checkquery);

        //    var co = CheckOfflinereg.Rows[0][0].ToString();
        //    if (int.Parse(co) <= 0)
        //    {
        //        if (VarCheckqueryForthisClienthaveAnyRegisterOnOffline.Rows.Count <= 0)
        //        {
        //            string Userinfor = "select  LoginId from tblUser where UserId = " + databaseCon.Decrypt(UserId) + "";
        //            DataTable Confirmdt = databaseCon.GetData(Userinfor);

        //            var loginuserid = Confirmdt.Rows[0][0].ToString();

        //            string query = "update tblRegister set IsOfflineCurrentForInspection = 1 ,GoOfflineOn = GETDATE(), GoOfflineBy = '" + loginuserid + "' where  SiteLocId = " + databaseCon.Decrypt(Siteid) + " and ClientID =  " + databaseCon.Decrypt(id) + " and DateOfDismantle = '1900-01-01'  and DateTagged != '1900-01-01'";
        //            string goOffline = "insert into tblTrackOffline (ClientId, SiteId, Action, GoOfflineOn, GoOfflineBy) values(" + databaseCon.Decrypt(id) + "," + databaseCon.Decrypt(Siteid) + ", 'Offline', GetDate(),'" + loginuserid + "')";
        //            var OfflineEntry = databaseCon.GetData(goOffline);
        //            var res = databaseCon.RunCmd(query);
        //            res = Convert.ToInt32(res);
        //            return Json(new { Result = "Success", Message = "Your data uploaded successfully for offline mode " }, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json(new { Result = "FailedToGoOffline", Message = "Data already in offline Mode for " + VarCheckqueryForthisClienthaveAnyRegisterOnOffline.Rows[0][0].ToString() + " Site Location" }, JsonRequestBehavior.AllowGet);
        //        }
        //    }

        //    else
        //    {
        //        return Json(new { Result = "Failed", Message = "Data already in offline" }, JsonRequestBehavior.AllowGet);

        //    }

        //}
        //public ActionResult GoOfflineClick(string id, string UserId, string Siteid, List<string> RegisterIds)
        //{

        //    string checkqueryForthisClienthaveAnyRegisterOnOffline = "select  SiteLocation , count('*')  as NoOfJobOffline from VW_TblRegister where IsOfflineCurrentForInspection = 1 and ClientID =" + databaseCon.Decrypt(id) + " group by SiteLocation having count('*') >1 ";
        //    var VarCheckqueryForthisClienthaveAnyRegisterOnOffline = databaseCon.GetData(checkqueryForthisClienthaveAnyRegisterOnOffline);

        //    //if ((int.Parse(VarCheckqueryForthisClienthaveAnyRegisterOnOffline.Rows[0][1].ToString()) <= 0))


        //    string checkquery = "select  count('x') from tblregister where IsOfflineCurrentForInspection =1 and clientid  = " + databaseCon.Decrypt(id) + " and SiteLocId = " + databaseCon.Decrypt(Siteid) + " and DateOfDismantle = '1900-01-01'  and DateTagged != '1900-01-01'";

        //    var CheckOfflinereg = databaseCon.GetData(checkquery);

        //    var co = CheckOfflinereg.Rows[0][0].ToString();
        //    if (int.Parse(co) <= 0)
        //    {
        //        if (VarCheckqueryForthisClienthaveAnyRegisterOnOffline.Rows.Count <= 0)
        //        {
        //            string Userinfor = "select  LoginId from tblUser where UserId = " + databaseCon.Decrypt(UserId) + "";
        //            DataTable Confirmdt = databaseCon.GetData(Userinfor);

        //            var loginuserid = Confirmdt.Rows[0][0].ToString();
        //            string registerIds = string.Join(",", RegisterIds);
        //            //string query = "update tblRegister set IsOfflineCurrentForInspection = 1 ,GoOfflineOn = GETDATE(), GoOfflineBy = '" + loginuserid + "' where  SiteLocId = " + databaseCon.Decrypt(Siteid) + " and ClientID =  " + databaseCon.Decrypt(id) + " and DateOfDismantle = '1900-01-01'  and DateTagged != '1900-01-01'";

        //            string query = "update tblRegister set IsOfflineCurrentForInspection = 1 ,GoOfflineOn = GETDATE(), GoOfflineBy = '" + loginuserid + "' where REGISTERId IN (" + registerIds + ") and SiteLocId = " + databaseCon.Decrypt(Siteid) + " and ClientID =  " + databaseCon.Decrypt(id) + " and DateOfDismantle = '1900-01-01'  and DateTagged != '1900-01-01'";

        //            string goOffline = "insert into tblTrackOffline (ClientId, SiteId, Action, GoOfflineOn, GoOfflineBy) values(" + databaseCon.Decrypt(id) + "," + databaseCon.Decrypt(Siteid) + ", 'Offline', GetDate(),'" + loginuserid + "')";
        //            var OfflineEntry = databaseCon.GetData(goOffline);
        //            var res = databaseCon.RunCmd(query);
        //            res = Convert.ToInt32(res);
        //            return Json(new { Result = "Success", Message = "Your data uploaded successfully for offline mode " }, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json(new { Result = "FailedToGoOffline", Message = "Data already in offline Mode for " + VarCheckqueryForthisClienthaveAnyRegisterOnOffline.Rows[0][0].ToString() + " Site Location" }, JsonRequestBehavior.AllowGet);
        //        }
        //    }

        //    else
        //    {
        //        return Json(new { Result = "Failed", Message = "Data already in offline" }, JsonRequestBehavior.AllowGet);

        //    }

        //}

        //public ActionResult GoOfflineClick(string id, string UserId, string Siteid, string RegisterIds)
        //{
        //    string checkqueryForthisClienthaveAnyRegisterOnOffline =
        //        "select SiteLocation , count('*') as NoOfJobOffline " +
        //        "from VW_TblRegister where IsOfflineCurrentForInspection = 1 " +
        //        "and ClientID =" + databaseCon.Decrypt(id) +
        //        " group by SiteLocation having count('*') >1 ";

        //    var VarCheckqueryForthisClienthaveAnyRegisterOnOffline =
        //        databaseCon.GetData(checkqueryForthisClienthaveAnyRegisterOnOffline);

        //    string checkquery =
        //        "select count('x') from tblregister " +
        //        "where IsOfflineCurrentForInspection =1 " +
        //        "and clientid = " + databaseCon.Decrypt(id) +
        //        " and SiteLocId = " + databaseCon.Decrypt(Siteid) +
        //        " and DateOfDismantle = '1900-01-01' " +
        //        "and DateTagged != '1900-01-01'";

        //    var CheckOfflinereg = databaseCon.GetData(checkquery);

        //    var co = CheckOfflinereg.Rows[0][0].ToString();

        //    if (int.Parse(co) <= 0)
        //    {
        //        if (VarCheckqueryForthisClienthaveAnyRegisterOnOffline.Rows.Count <= 0)
        //        {
        //            string Userinfor =
        //                "select LoginId from tblUser where UserId = " +
        //                databaseCon.Decrypt(UserId);

        //            DataTable Confirmdt = databaseCon.GetData(Userinfor);

        //            var loginuserid = Confirmdt.Rows[0][0].ToString();

        //            string query =
        //                "update tblRegister " +
        //                "set IsOfflineCurrentForInspection = 1, " +
        //                "GoOfflineOn = GETDATE(), " +
        //                "GoOfflineBy = '" + loginuserid + "' " +
        //                "where REGISTERId IN (" + RegisterIds + ") " +
        //                "and SiteLocId = " + databaseCon.Decrypt(Siteid) +
        //                " and ClientID = " + databaseCon.Decrypt(id) +
        //                " and DateOfDismantle = '1900-01-01' " +
        //                "and DateTagged != '1900-01-01'";

        //            string goOffline =
        //                "insert into tblTrackOffline " +
        //                "(ClientId, SiteId, Action, GoOfflineOn, GoOfflineBy) " +
        //                "values(" + databaseCon.Decrypt(id) + "," +
        //                databaseCon.Decrypt(Siteid) +
        //                ", 'Offline', GetDate(),'" + loginuserid + "')";

        //            databaseCon.GetData(goOffline);

        //            var res = databaseCon.RunCmd(query);

        //            return Json(new
        //            {
        //                Result = "Success",
        //                Message = "Your data uploaded successfully for offline mode "
        //            }, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json(new
        //            {
        //                Result = "FailedToGoOffline",
        //                Message = "Data already in offline Mode for " +
        //                VarCheckqueryForthisClienthaveAnyRegisterOnOffline.Rows[0][0].ToString() +
        //                " Site Location"
        //            }, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    else
        //    {
        //        return Json(new
        //        {
        //            Result = "Failed",
        //            Message = "Data already in offline"
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        public ActionResult GoOfflineClick(string id, string UserId, string Siteid, string RegisterIds)
{
    try
    {
        // Prevent empty RegisterIds issue
        if (string.IsNullOrWhiteSpace(RegisterIds))
        {
            return Json(new
            {
                Result = "Failed",
                Message = "No jobs selected for offline."
            }, JsonRequestBehavior.AllowGet);
        }

        string checkqueryForthisClienthaveAnyRegisterOnOffline =
            "select SiteLocation , count('*') as NoOfJobOffline " +
            "from VW_TblRegister where IsOfflineCurrentForInspection = 1 " +
            "and ClientID =" + databaseCon.Decrypt(id) +
            " group by SiteLocation having count('*') >1 ";

        var VarCheckqueryForthisClienthaveAnyRegisterOnOffline =
            databaseCon.GetData(checkqueryForthisClienthaveAnyRegisterOnOffline);

        // Check only selected RegisterIds instead of all site jobs
        string checkquery =
            "select count('x') from tblregister " +
            "where IsOfflineCurrentForInspection = 1 " +
            "and REGISTERId IN (" + RegisterIds + ") " +
            "and clientid = " + databaseCon.Decrypt(id) +
            " and SiteLocId = " + databaseCon.Decrypt(Siteid) +
            " and DateOfDismantle = '1900-01-01' " +
            "and DateTagged != '1900-01-01'";

        var CheckOfflinereg = databaseCon.GetData(checkquery);

        var co = CheckOfflinereg.Rows[0][0].ToString();

        if (int.Parse(co) <= 0)
        {
            if (VarCheckqueryForthisClienthaveAnyRegisterOnOffline.Rows.Count <= 0)
            {
                string Userinfor =
                    "select LoginId from tblUser where UserId = " +
                    databaseCon.Decrypt(UserId);

                DataTable Confirmdt = databaseCon.GetData(Userinfor);

                var loginuserid = Confirmdt.Rows[0][0].ToString();

                string query =
                    "update tblRegister " +
                    "set IsOfflineCurrentForInspection = 1, " +
                    "GoOfflineOn = GETDATE(), " +
                    "GoOfflineBy = '" + loginuserid + "' " +
                    "where REGISTERId IN (" + RegisterIds + ") " +
                    "and SiteLocId = " + databaseCon.Decrypt(Siteid) +
                    " and ClientID = " + databaseCon.Decrypt(id) +
                    " and DateOfDismantle = '1900-01-01' " +
                    "and DateTagged != '1900-01-01'";

                string goOffline =
                    "insert into tblTrackOffline " +
                    "(ClientId, SiteId, Action, GoOfflineOn, GoOfflineBy) " +
                    "values(" + databaseCon.Decrypt(id) + "," +
                    databaseCon.Decrypt(Siteid) +
                    ", 'Offline', GetDate(),'" + loginuserid + "')";

                databaseCon.GetData(goOffline);

                var res = databaseCon.RunCmd(query);

                return Json(new
                {
                    Result = "Success",
                    Message = "Your data uploaded successfully for offline mode"
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    Result = "FailedToGoOffline",
                    Message = "Data already in offline Mode for " +
                    VarCheckqueryForthisClienthaveAnyRegisterOnOffline.Rows[0][0].ToString() +
                    " Site Location"
                }, JsonRequestBehavior.AllowGet);
            }
        }
        else
        {
            return Json(new
            {
                Result = "Failed",
                Message = "Selected jobs are already in offline"
            }, JsonRequestBehavior.AllowGet);
        }
    }
    catch (Exception ex)
    {
        return Json(new
        {
            Result = "Error",
            Message = ex.Message
        }, JsonRequestBehavior.AllowGet);
    }
}
        public ActionResult CheckIsInspectionDataForOffline(string id, string Siteid, int userTimeZone = 0)
        {
            var Result = "";
            var Message = "";

            string checkqueryForSiteDataisOffline = "select count('x') from tblregister where IsOfflineCurrentForInspection =1 and clientid = " + databaseCon.Decrypt(id) + " and SiteLocId = " + Siteid;
            var CheckOfflinereg = databaseCon.GetData(checkqueryForSiteDataisOffline);
            var co = CheckOfflinereg.Rows[0][0].ToString();

            if (int.Parse(co) <= 0)
            {
                return Json(new { Checked = false });
            }
            else
            {
                string FinddataQuery = "select distinct userid, GoOfflineBy, GoOfflineOn from tblregister left join tbluser on GoOfflineBy=EmailID where IsOfflineCurrentForInspection =1 and tblregister.clientid = " + databaseCon.Decrypt(id) + " and SiteLocId = " + Siteid;
                var FinddataUpdatedBy = databaseCon.GetData(FinddataQuery);
                var userId = FinddataUpdatedBy.Rows[0][0].ToString();
                var offlineBy = FinddataUpdatedBy.Rows[0][1].ToString();
                var offlineon = FinddataUpdatedBy.Rows[0][2].ToString();

                DateTime offlineonDateTime = Convert.ToDateTime(offlineon);

                DateTime localTime;

                // If running on local machine, don't subtract timezone
                if (Request.Url.Host.Contains("localhost"))
                {
                    localTime = offlineonDateTime;
                }
                else
                {
                    localTime = offlineonDateTime.AddMinutes(-userTimeZone);
                }

                string formattedOfflineon = localTime.ToString("dd-MMM-yyyy h:mm:ss tt");

                Result = "Failed";
                Message = "You cannot save the inspection because the site data is on offline mode by " + offlineBy + " since " + formattedOfflineon;

                TempData["offlineBy"] = offlineBy;
                TempData["userId"] = userId;

                return Json(new { Checked = true, Result = Result, Message = Message });
            }
        }

        [HttpPost]
        public ActionResult UpdateOfflineInspectionFlag(string LoginUserId, string clientId, string Siteid)
        {
            string CheckIsDatasillInOfflineMode = "select  RegisterId from tblRegister where ClientID =  " + databaseCon.Decrypt(clientId) + " and SiteLocId = " + databaseCon.Decrypt(Siteid) + " and IsOfflineCurrentForInspection = 1";
            DataTable CheckIsDatasillInOfflineModeData = databaseCon.GetData(CheckIsDatasillInOfflineMode);

            if (CheckIsDatasillInOfflineModeData.Rows.Count > 0)
            {

                try
                {
                    string Userinfor = "select LoginId from tblUser where UserId = " + databaseCon.Decrypt(LoginUserId) + "";

                    DataTable LoginuserInfo = databaseCon.GetData(Userinfor);

                    //string updateRegisterquery = "update tblRegister set IsOfflineCurrentForInspection = 0 ,OfflineCleared = 1, OfflineClearedOn = GETDATE(), OfflineClearedBy = '" + LoginuserInfo.Rows[0][0].ToString() + "' where ClientID =  " + databaseCon.Decrypt(clientId) + " and SiteLocId = " + databaseCon.Decrypt(Siteid)+" and DateOfDismantle = '1900-01-01' and DateTagged != '1900-01-01'";
                    //chaned on 12-11-2024 
                    string updateRegisterquery = "update tblRegister set IsOfflineCurrentForInspection = 0 ,OfflineCleared = 1, OfflineClearedOn = GETDATE(), OfflineClearedBy = '" + LoginuserInfo.Rows[0][0].ToString() + "' where ClientID =  " + databaseCon.Decrypt(clientId) + " and SiteLocId = " + databaseCon.Decrypt(Siteid) + " and IsOfflineCurrentForInspection = 1";

                    var res = databaseCon.RunCmd(updateRegisterquery);

                    string cleared = "insert into tblTrackOffline (ClientId, SiteId, Action, ClearedOn, ClearedBy) values(" + databaseCon.Decrypt(clientId) + "," + databaseCon.Decrypt(Siteid) + ", 'Cleared', GetDate(),'" + LoginuserInfo.Rows[0][0].ToString() + "')";
                    var clearedEntry = databaseCon.GetData(cleared);

                    return Json(new { Result = "Success", Message = "The Site inspections done on offline (If any) will no more be allowed to be loaded. You can now work online." });
                }
                catch (Exception ex)
                {
                    return Json(new { Result = "Failed", Message = ex.Message });
                }
            }
            else
            {
                //string FinddataQuery = "select  distinct GoOfflineBy ,GoOfflineOn from tblregister where IsOfflineCurrentForInspection =1 and clientid  = " + databaseCon.Decrypt(clientId) + " and SiteLocId = " + databaseCon.Decrypt(Siteid) + "";
                //var FinddataUpdatedBy = databaseCon.GetData(FinddataQuery);

                //var offlineBy = FinddataUpdatedBy.Rows[0][0].ToString();
                //var offlineon = FinddataUpdatedBy.Rows[0][1].ToString();

                //DateTime offlineonDateTime = DateTime.Parse(offlineon);
                //string formattedOfflineon = offlineonDateTime.ToString("dd-MMM-yyyy h:mm:ss tt");
                //return Json(new { Result = "AlreadyMoved", Message = "This inspection session has been voided due to time lapse.  Voided by Scafpac Admin: " + formattedOfflineon + "." });
                return Json(new { Result = "Failed", Message = "No data found for cleared records." });


            }
        }


        public ActionResult CheckThisUserIsInOfflinemode(string LoginUserId, string clientId, string Siteid)
        {

            //string UserVerifyedQuery = "select distinct GoOfflineBy from tblregister where  IsOfflineCurrentForInspection = 1 and clientid = " + databaseCon.Decrypt(clientId) + "   and SiteLocId = " + databaseCon.Decrypt(Siteid) + " and GoOfflineBy = (select emailid from tbluser where userid = " + databaseCon.Decrypt(LoginUserId) + ")";
            //we used this query for  is this same user which done ofline data of difrent ?

            string UserVerifyedQuery = "SELECT CASE WHEN EXISTS (SELECT 'x' FROM tblregister WHERE IsOfflineCurrentForInspection = 1 AND clientid = " + databaseCon.Decrypt(clientId) + " AND SiteLocId = " + Siteid + " AND ISNULL(GoOfflineBy, '') = (SELECT emailid FROM tbluser WHERE userid = " + databaseCon.Decrypt(LoginUserId) + ")) THEN 1 WHEN EXISTS (SELECT 'x' FROM tblregister WHERE IsOfflineCurrentForInspection = 1 AND clientid = " + databaseCon.Decrypt(clientId) + " AND SiteLocId = " + Siteid + " AND ISNULL(GoOfflineBy, '') != (SELECT emailid FROM tbluser WHERE userid = " + databaseCon.Decrypt(LoginUserId) + ")) THEN 0 ELSE 1 END";

            //string UserVerifyedQuery = "SELECT CASE WHEN EXISTS (SELECT 'x' FROM tblregister WHERE IsOfflineCurrentForInspection = 1 AND clientid = " + databaseCon.Decrypt(clientId) + " AND SiteLocId = " + Siteid + " AND ISNULL(GoOfflineBy, '') = (SELECT emailid FROM tbluser WHERE userid = " + databaseCon.Decrypt(LoginUserId) + ")) THEN 1 WHEN EXISTS (SELECT 'x' FROM tblregister WHERE IsOfflineCurrentForInspection = 1 AND clientid = " + databaseCon.Decrypt(clientId) + " AND SiteLocId = " + Siteid + " AND ISNULL(GoOfflineBy, '') != (SELECT emailid FROM tbluser WHERE userid = " + databaseCon.Decrypt(LoginUserId) + ")) THEN 0 ELSE 1 end ,EmailId,concat(firstname,' ',Lastname)UserName from tbluser where userid = " + databaseCon.Decrypt(LoginUserId) + "";

            var isuer = databaseCon.GetData(UserVerifyedQuery);


            string finduser = "select  distinct userid,concat(firstname,' ',Lastname)UserName,GoOfflineBy ,GoOfflineOn from tblregister left join tbluser on GoOfflineBy=EmailID where IsOfflineCurrentForInspection =1 and tblregister.clientid  = " + databaseCon.Decrypt(clientId) + " and SiteLocId = " + Siteid + "";
            var FinddataUpdatedBy = databaseCon.GetData(finduser);

            //var userId = FinddataUpdatedBy.Rows[0][0].ToString();
            //var offlineBy = FinddataUpdatedBy.Rows[0][1].ToString();
            //var Username = FinddataUpdatedBy.Rows[0][1].ToString();
            var userId = "";
            var userName = "";
            var offlineBy = "";

            if (FinddataUpdatedBy.Rows.Count > 0)
            {
                userId = FinddataUpdatedBy.Rows[0]["userid"].ToString();
                userName = FinddataUpdatedBy.Rows[0]["UserName"].ToString();
                offlineBy = FinddataUpdatedBy.Rows[0]["GoOfflineBy"].ToString();


            }

            return Json(new { VerifyedUser = isuer.Rows[0][0], UserName = userName }, JsonRequestBehavior.AllowGet);
        }
    }
}