using Scaffold.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scaffold_DataAccess;
using Scaffold.Models;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using QRCoder;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Configuration;
using static Scaffold.Controllers.RegisterListController;
using System.Web.Services.Description;
using DocumentFormat.OpenXml.Office2010.Excel;
using static Scaffold.Controllers.OfflineRegisterController;
using System.Web.Helpers;
namespace Scaffold.Controllers
{
    public class RegisterController : Controller
    {
        ScaffoldEntities dbcontext = new ScaffoldEntities();

        SqlCommand cmd = null;
        SqlConnection con = null;
        // GET: Rigister
        public ActionResult Register(string id, string UID, string UserRole)
        {
            ViewBag.id = databaseCon.Decrypt(id);
            ViewBag.UID = databaseCon.Decrypt(UID);
            ViewBag.UserRole = databaseCon.Decrypt(UserRole);

            string query = "exec Pro_TBLCLIENT_GET '" + databaseCon.Decrypt(id) + "'";
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count != 0)
            {
                //  ddlInspectionTimeFrame.SelectedValue = Convert.ToString(dt.Rows[0]["InspTimeFrame"]);
                //  ddlChecklistInsp.SelectedValue = Convert.ToString(dt.Rows[0]["ChecklistId"]);

                ViewBag.ddlInspectionTimeFrame = Convert.ToString(dt.Rows[0]["InspTimeFrame"]);
                ViewBag.ddlChecklistInsp = Convert.ToString(dt.Rows[0]["ChecklistId"]);
                ViewBag.NoOfScafRefGenerate = dt.Rows[0]["NoOfScafRefGenerate"].ToString();
            }
            query = "select isnull(OfflineEnabled,0) as OfflineEnabled from tblClient where clientid='" + databaseCon.Decrypt(id) + "'";
            dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["OfflineEnabled"].ToString() == "True")
                {
                    ViewBag.OfflineEnabled = true;

                }
            }

            return View();
        }
        /// Register get ClientRatio Start

        public ActionResult ClientErectRatio(string ClientID)
        {
            var ClientRatio = "";
            string query = "select ManHrsRatio from tblClient where clientid='" + ClientID + "'";
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                ClientRatio = Convert.ToString(dt.Rows[0]["ManHrsRatio"]);
            }
            return Json(ClientRatio, JsonRequestBehavior.AllowGet);
        }
        /// Register get ClientRatio End
        public ActionResult BindallDropdown(string id)
        {

            var ddlsites = dbcontext.Database.SqlQuery<DropDownViewModel>("select distinct SiteLocID,SiteLocation from tblSiteLocation where SiteLocationStatus='Active' and SiteLocation <>'' and SiteLocation is not null and  ClientId='" + id + "' Order by SiteLocation").ToList();

            var ddlScaffoldDesign = dbcontext.Database.SqlQuery<DropDownViewModel>("select distinct(DesignType) as DesignType from tblDesign where DesignStatus = 'Active' and DesignType <> '' and DesignType is not null and ClientId = '" + id + "' order by DesignType");

            var ddlDutyrating = dbcontext.Database.SqlQuery<DropDownViewModel>("select distinct(DutyRating) as DutyRating from tblDutyRating where DutyRatingStatus='Active' and DutyRating <>'' and DutyRating is not null and  ClientId='" + id + "' order by DutyRating");

            var ddlMaterialType = dbcontext.Database.SqlQuery<DropDownViewModel>("select distinct(MaterialType) as MaterialType from tblMaterialType where MaterialStatus='Active' and MaterialType <>'' and MaterialType is not null and  ClientId='" + id + "' order by MaterialType");

            var ddlsupervisor = dbcontext.Database.SqlQuery<DropDownViewModel>("select UserId,firstName+' '+LastName as Supervisor from tblUser join tblUserCategory on  tblUser.ClientId=tblUserCategory.ClientId and tblUser.CategoryId=tblUserCategory.UserCatId  where Category='Supervisor' and tbluser.ClientId='" + id + "' order by Supervisor");

            var ddlsignbysprvsr = dbcontext.Database.SqlQuery<DropDownViewModel>("select UserId,firstName+' '+LastName as Supervisor from tblUser join tblUserCategory on  tblUser.ClientId=tblUserCategory.ClientId and tblUser.CategoryId=tblUserCategory.UserCatId  where Category='Supervisor' and tbluser.ClientId='" + id + "' order by Supervisor");

            var ddlChecklistInsp = dbcontext.Database.SqlQuery<DropDownViewModel>("select distinct tblCheckList.checklistID, checklistdescription from tblchecklist join tblcheckListContent on tblchecklist.ChecklistId = tblcheckListContent.ChecklistId and  tblCheckList.ClientId = tblchecklistcontent.clientid where tblCheckList.clientid = '" + id + "' and[status] = 'Active' order by checklistdescription");

            var dropdownScaffoldOwnerRep = dbcontext.Database.SqlQuery<DropDownViewModel>("select distinct ScaffoldOwner from tblRegister where ClientId='" + id + "' and  ScaffoldOwner is not null and ScaffoldOwner!= '' Order by ScaffoldOwner");

            var dropdowndiscipline = dbcontext.Database.SqlQuery<DropDownViewModel>("select distinct discipline from  tblRegister where ClientId = '" + id + "' and discipline is not null and discipline!= '' Order by discipline");

            var dropdownWorkGroup = dbcontext.Database.SqlQuery<DropDownViewModel>("select distinct W.WorkGroup,W.WorkGroupID,S.Sitelocation as DropdownSitelocation from tblWorkGroup W left join tblSiteLocation S on W.SiteLocId=S.SiteLocId where W.ClientId='" + id + "' and W.WorkGroup is not null and W.WorkGroup!='' and Status=1  order by W.WorkGroup").ToList();


            var dropdownSubGroup = dbcontext.Database.SqlQuery<DropDownViewModel>("select distinct SubGroupId, SubGroupName as SubGroup from TblSubGroups where ClientId = '" + id + "' and SubGroupName is not null and SubGroupName!= '' and Status=1 Order by SubGroupName");

            var dropdownRequestedby = dbcontext.Database.SqlQuery<DropDownViewModel>("select distinct RequestById, RequestBy from TblRequestBy where ClientId = '" + id + "' and RequestBy is not null and RequestBy!= '' and Status=1 Order by RequestBy");

            return Json(new { ddlsites = ddlsites, ddlScaffoldDesign = ddlScaffoldDesign, ddlDutyrating = ddlDutyrating, ddlMaterialType = ddlMaterialType, ddlsupervisor = ddlsupervisor, ddlsignbysprvsr = ddlsignbysprvsr, ddlChecklistInsp = ddlChecklistInsp, dropdownScaffoldOwnerRep = dropdownScaffoldOwnerRep, dropdowndiscipline = dropdowndiscipline, dropdownWorkGroup = dropdownWorkGroup, dropdownSubGroup = dropdownSubGroup, dropdownRequestedby = dropdownRequestedby }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult getworkgroupdropdown(string id, string sitelocid)
        {
            var dropdownWorkGroup = dbcontext.Database.SqlQuery<DropDownViewModel>("select distinct W.WorkGroup,W.WorkGroupID,S.Sitelocation as DropdownSitelocation from tblWorkGroup W  join tblSiteLocation S on W.SiteLocId=S.SiteLocId where W.ClientId='" + id + "' and W.SiteLocId='" + sitelocid + "' and W.WorkGroup is not null and W.WorkGroup!='' and Status=1  order  by W.WorkGroup");
            return Json(new { dropdownWorkGroup = dropdownWorkGroup }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getSubgroupdropdown(string id, string sitelocid, string WorkGroupid)
        {
            var dropdownSubGroup = dbcontext.Database.SqlQuery<DropDownViewModel>(
                "select distinct SubGroupId, SubGroupName as SubGroup from TblSubGroups " +
                "where ClientId='" + id + "' and SiteLocId='" + sitelocid + "' " +
                "and SubGroupName is not null and SubGroupName!='' and Status=1 " +
                (!string.IsNullOrEmpty(WorkGroupid) && WorkGroupid != "0" ? " and WorkGroupId='" + WorkGroupid + "' " : "") +
                " Order by SubGroupName").ToList();

            return Json(new { dropdownSubGroup = dropdownSubGroup }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getRequestedbydropdown(string id, string sitelocid)
        {
            var dropdownRequestedby = dbcontext.Database.SqlQuery<DropDownViewModel>("select distinct RequestById, RequestBy from TblRequestBy where ClientId = '" + id + "' and SiteLocId = '" + sitelocid + "' and RequestBy is not null and RequestBy!= '' and Status=1 Order by RequestBy");
            return Json(new { dropdownRequestedby = dropdownRequestedby }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LocationDropdownBind(string id, string SiteID)
        {
            //var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select area,areaid  from tblarea where AreaStatus='Active' and  Area <>'' and Area is not null and ClientID='" + id + "' and siteLocId='" + SiteID + "' order by area");

            //Changed done on 28/Oct/2020 (displaying Not Assigned Location at top)
            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select area,areaid ,0 as displayorder from tblarea where AreaStatus='Active' and  Area <>'' and Area is not null and Area ='Not Assigned Location' and ClientID='" + id + "' and siteLocId='" + SiteID + "' UNION select area, areaid, 1 as displayorder from tblarea where AreaStatus = 'Active' and  Area <> '' and Area is not null and Area != 'Not Assigned Location' and ClientID ='" + id + "' and siteLocId = '" + SiteID + "' order by displayorder, area");
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BuildingDropdownBind(string id, string LocationID)
        {
            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select buildingid,buildingno  from tblbuilding where BuildingStatus='Active' and ClientID='" + databaseCon.Decrypt(id) + "' and Areaid='" + LocationID + "' order by buildingno");
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AssetsDropdownBind(string id, string BuildingID)
        {
            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select Assetid,Assetno  from tblasset where AssetStatus='Active' and ClientID ='" + databaseCon.Decrypt(id) + "' and buildingid='" + BuildingID + "' order by Assetno");
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FloorLevelDropdown(string id, string SiteID, string LocationID, string BuildingID)
        {
            //string queryFloor = "";
            //string getFloor = "";
            //string getFloor1 = "";
            //DataTable dtFloor;
            //DataTable dtgetFloor;
            //DataTable dtgetFloor1;

            string query = "select  isnull (GPS,0) as Gps,isnull(FloorLevel,0) as FloorLevel from tblSiteLocation  where SiteLocID='" + SiteID + "'";
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {

                if (dt.Rows[0]["Gps"].ToString() == "True")
                {

                    //  getFloor = "select FloorLevelId,FloorLevel,LocationId from tblFloorlevel where ClientId ='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and FloorLevelStatus='Active' and  sitelocid='" + ddlsites.SelectedValue + "' and  LocationId='" + ddlLocation.SelectedValue + "' and BuildingID = '" + ddbuilding.SelectedValue + "' and Category = 'GPS' order by SortOrder ASC"; 

                    query = "select FloorLevelId,FloorLevel,LocationId from tblFloorlevel where ClientId ='" + id + "' and FloorLevelStatus='Active' and  sitelocid='" + SiteID + "' and  LocationId='" + LocationID + "' and BuildingID = '" + BuildingID + "' and Category = 'GPS' order by SortOrder ASC";
                    // Check Floors For Building

                    dt = databaseCon.GetData(query);

                    if (dt.Rows.Count > 0)
                    {
                        query = "select FloorLevelId,FloorLevel,LocationId from tblFloorlevel where ClientId ='" + id + "' and FloorLevelStatus='Active' and  sitelocid='" + SiteID + "' and  LocationId='" + LocationID + "' and BuildingID = '" + BuildingID + "' and Category = 'GPS' order by SortOrder ASC";
                    }
                    else
                    {
                        query = "select FloorLevelId,FloorLevel,LocationId from tblFloorlevel where ClientId ='" + id + "' and FloorLevelStatus='Active' and  sitelocid='" + SiteID + "' and  LocationId='" + LocationID + "' and (BuildingID = '0') and Category = 'GPS' order by SortOrder ASC"; // Check Floors For Location

                        dt = databaseCon.GetData(query);

                        if (dt.Rows.Count > 0)
                        {
                            query = "select FloorLevelId,FloorLevel,LocationId from tblFloorlevel where ClientId ='" + id + "' and FloorLevelStatus='Active' and  sitelocid='" + SiteID + "' and  LocationId='" + LocationID + "' and (BuildingID = '0') and Category = 'GPS' order by SortOrder ASC";
                        }
                        else
                        {
                            query = "select FloorLevelId,FloorLevel from tblFloorlevel where ClientId ='" + id + "' and FloorLevelStatus='Active' and  sitelocid='" + SiteID + "' and  LocationId = '0' and BuildingID = '0' and Category = 'GPS' order by SortOrder ASC";
                            // Check Floors For Sites only
                        }

                    }
                }
                else if (dt.Rows[0]["FloorLevel"].ToString() == "True")
                {

                    query = "select FloorLevel,tblSiteImages.FloorLevelId from tblSiteImages join tblfloorLevel on tblSiteImages.FloorLevelId = tblFloorLevel.FloorLevelId  where tblSiteImages.ClientId ='" + id + "' and tblSiteImages.SiteId ='" + SiteID + "' and tblSiteImages.LocID = '" + LocationID + "' and tblSiteImages.BuildingID = '" + BuildingID + "'";
                }
            }

            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>(query);
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ddlLocation_SelectedIndexChanged(string id, string SiteID, string LocationID, string RID)
        {

            RegisterCommonViewModel obj = new RegisterCommonViewModel();
            string flag = "False";
            string LocationId = "";
            string query1 = "select XPosition,YPosition,Sitelocid,LocationId,BuildingID,FloorLevelId,Lat,Long,AssetId,AssetDesc from tblregister where clientID = '" + databaseCon.Decrypt(id) + "'  and RegisterId='" + databaseCon.Decrypt(RID) + "'";
            DataTable dt = databaseCon.GetData(query1);
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToString(dt.Rows[0]["XPosition"]) != "" && Convert.ToString(dt.Rows[0]["YPosition"]) != "")
                {
                    obj.Sitelocid = Convert.ToInt32(dt.Rows[0]["Sitelocid"]);
                    obj.LocationId = Convert.ToInt32(dt.Rows[0]["LocationId"]);
                    obj.BuildingID = Convert.ToInt32(dt.Rows[0]["BuildingID"]);
                    obj.FloorLevelID = Convert.ToInt32(dt.Rows[0]["FloorLevelId"]);
                    //  SiteLocationID = Convert.ToString(dt.Rows[0]["Sitelocid"]);
                    LocationId = Convert.ToString(dt.Rows[0]["LocationId"]);
                    if (dt.Rows[0]["AssetId"].ToString() != "")
                    {
                        obj.AssetId = Convert.ToInt32(dt.Rows[0]["AssetId"]);
                    }
                    obj.AssetDesc = dt.Rows[0]["AssetDesc"].ToString();
                    if (LocationID != Convert.ToString(dt.Rows[0]["LocationId"]))
                    {
                        flag = "True";
                    }
                    else
                    {
                        flag = "False";
                    }
                }
                else
                {
                    flag = "False";
                }
            }
            else
            {
                flag = "False";
            }

            return Json(new { Result = flag, LocationData = obj }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ddbuilding_SelectedIndexChanged(string id, string SiteID, string LocationID, string BuildingID, string RID)
        {
            RegisterCommonViewModel obj = new RegisterCommonViewModel();
            string flag = "False";
            string query1 = "select XPosition,YPosition,Sitelocid,LocationId,BuildingID,FloorLevelId,Lat,Long,AssetId,AssetDesc from tblregister where clientID = '" + databaseCon.Decrypt(id) + "'  and RegisterId='" + databaseCon.Decrypt(RID) + "'";
            DataTable dt = databaseCon.GetData(query1);
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToString(dt.Rows[0]["XPosition"]) != "" && Convert.ToString(dt.Rows[0]["YPosition"]) != "")
                {
                    obj.Sitelocid = Convert.ToInt32(dt.Rows[0]["Sitelocid"]);
                    obj.LocationId = Convert.ToInt32(dt.Rows[0]["LocationId"]);
                    obj.BuildingID = Convert.ToInt32(dt.Rows[0]["BuildingID"]);
                    obj.FloorLevelID = Convert.ToInt32(dt.Rows[0]["FloorLevelId"]);
                    BuildingID = Convert.ToString(dt.Rows[0]["BuildingID"]);
                    if (dt.Rows[0]["AssetId"].ToString() != "")
                    {
                        obj.AssetId = Convert.ToInt32(dt.Rows[0]["AssetId"]);
                    }
                    obj.AssetDesc = dt.Rows[0]["AssetDesc"].ToString();
                    if (BuildingID == Convert.ToString(dt.Rows[0]["BuildingID"]))
                    {
                        flag = "True";
                    }
                    else
                    {
                        flag = "False";
                    }
                }
            }
            else
            {
                flag = "False";
            }

            return Json(new { Result = flag, BuildingData = obj }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ddassets_SelectedIndexChanged(string id, string assestID)
        {
            string query;
            var txtassetsdesc = "";
            query = "select AssetDesc  from tblAsset where  ClientID='" + databaseCon.Decrypt(id) + "' and assetid='" + assestID + "'";
            DataTable dtassetdesc = databaseCon.GetData(query);

            if (dtassetdesc.Rows.Count != 0)
            {
                txtassetsdesc = Convert.ToString(dtassetdesc.Rows[0]["AssetDesc"]);

            }
            return Json(txtassetsdesc, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DropDownList1_SelectedIndexChanged(string id, string RID)
        {
            var XPosition = "";
            var YPosition = "";
            string query = "select SiteLocId,FloorLevelId,XPosition,YPosition from tblregister where clientID = '" + databaseCon.Decrypt(id) + "'  and RegisterId='" + databaseCon.Decrypt(RID) + "'";
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                XPosition = dt.Rows[0]["XPosition"].ToString();
                YPosition = dt.Rows[0]["YPosition"].ToString();
            }
            return Json(XPosition + "_" + YPosition, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSiteSelectIndexChangeData(string id, string SiteID, string RID, string ddlsites)
        {
            string queryFloor = "";
            DataTable dtFloor;
            string MapState = "";
            string query;
            var XPosition = "";
            var YPosition = "";

            if (ddlsites != "")
            {
                SiteID = ddlsites;
            }

            var SiteLocationID = "";
            var LocationId = "";

            RegisterCommonViewModel obj = new RegisterCommonViewModel();

            string query1 = "select XPosition,YPosition,Sitelocid,LocationId,BuildingID,FloorLevelId,Lat,Long,AssetId,AssetDesc from tblregister where clientID = '" + databaseCon.Decrypt(id) + "'  and RegisterId='" + databaseCon.Decrypt(RID) + "'";
            DataTable dt = databaseCon.GetData(query1);
            if (dt.Rows.Count > 0)
            {

                obj.XPosition = dt.Rows[0]["XPosition"].ToString();
                obj.YPosition = dt.Rows[0]["YPosition"].ToString();
                obj.Lat = dt.Rows[0]["Lat"].ToString();
                obj.Long = dt.Rows[0]["Long"].ToString();
                if (Convert.ToString(dt.Rows[0]["XPosition"]) != "" && Convert.ToString(dt.Rows[0]["YPosition"]) != "")
                {
                    obj.Sitelocid = Convert.ToInt32(dt.Rows[0]["Sitelocid"]);
                    obj.LocationId = Convert.ToInt32(dt.Rows[0]["LocationId"]);
                    if (dt.Rows[0]["BuildingID"].ToString() != "")
                    {
                        obj.BuildingID = Convert.ToInt32(dt.Rows[0]["BuildingID"]);
                    }
                    if (dt.Rows[0]["AssetId"].ToString() != "")
                    {
                        obj.AssetId = Convert.ToInt32(dt.Rows[0]["AssetId"]);
                    }
                    obj.AssetDesc = dt.Rows[0]["AssetDesc"].ToString();
                    obj.FloorLevelID = Convert.ToInt32(dt.Rows[0]["FloorLevelId"]);
                    SiteLocationID = Convert.ToString(dt.Rows[0]["Sitelocid"]);
                    LocationId = Convert.ToString(dt.Rows[0]["LocationId"]);
                }
                else if (Convert.ToString(dt.Rows[0]["Lat"]) != "" && Convert.ToString(dt.Rows[0]["Long"]) != "")
                {
                    obj.Sitelocid = Convert.ToInt32(dt.Rows[0]["Sitelocid"]);
                    obj.LocationId = Convert.ToInt32(dt.Rows[0]["LocationId"]);
                    if (dt.Rows[0]["BuildingID"].ToString() != "")
                    {
                        obj.BuildingID = Convert.ToInt32(dt.Rows[0]["BuildingID"]);
                    }
                    if (dt.Rows[0]["AssetId"].ToString() != "")
                    {
                        obj.AssetId = Convert.ToInt32(dt.Rows[0]["AssetId"]);
                    }
                    obj.AssetDesc = dt.Rows[0]["AssetDesc"].ToString();
                    obj.FloorLevelID = Convert.ToInt32(dt.Rows[0]["FloorLevelId"]);
                    SiteLocationID = Convert.ToString(dt.Rows[0]["Sitelocid"]);
                    LocationId = Convert.ToString(dt.Rows[0]["LocationId"]);
                }
                else
                {
                    obj.Sitelocid = Convert.ToInt32(dt.Rows[0]["Sitelocid"]);
                    if (dt.Rows[0]["LocationId"].ToString() != "")
                    {
                        obj.LocationId = Convert.ToInt32(dt.Rows[0]["LocationId"]);
                    }

                    //SiteLocationID = Convert.ToString(dt.Rows[0]["Sitelocid"]);
                    //LocationId = Convert.ToString(dt.Rows[0]["LocationId"]);

                    //  obj.Sitelocid = Convert.ToInt32(ddlsites);
                    // obj.LocationId = Convert.ToInt32(dt.Rows[0]["LocationId"]);
                    SiteLocationID = ddlsites;
                    // LocationId = Convert.ToString(dt.Rows[0]["LocationId"]);
                }
            }

            //string updatesite = "Select sum(Counts) as TotalRecord from (select Count(*)as Counts from TblSiteMaterial where registerId='" + databaseCon.Decrypt(RID) + "' Union select Count(*)as Counts from TblTransaction where [To]='Site' and fromId='" + databaseCon.Decrypt(RID) + "' ) Siteselect";
            string updatesite = "Select sum(Counts) as TotalRecord from (select Count(1) as Counts from TblSiteMaterial where registerId='" + databaseCon.Decrypt(RID) + "' Union select Count(1) as Counts from TblTransaction where [To]='Site' and fromId='" + databaseCon.Decrypt(RID) + "' ) Siteselect";
            DataTable dtsite = databaseCon.GetData(updatesite);
            if (dtsite.Rows.Count > 0)
            {
                obj.TotalRecord = Convert.ToInt32(dtsite.Rows[0]["TotalRecord"]);
                if (Convert.ToInt32(dtsite.Rows[0]["TotalRecord"]) > 0)
                {
                    if (ddlsites != "")
                    {
                        SiteID = SiteLocationID;
                    }

                }
            }

            if (SiteLocationID != "")
            {
                SiteID = SiteLocationID;
            }
            string querySite = "select isnull(GPS,0) as GPS,isnull(FloorLevel,0) as FloorLevel,isnull(EnabledGPSSitePin,0) as EnabledGPSSitePin ,isnull(EnabledCaptureGps,0) as EnabledCaptureGps ,isnull(SkipMaterial,0) as SkipMaterial,Address,city,state,OwnerName,Zip,isnull(MaintainInventory,0) as MaintainInventory, isnull(MaintainInventoryAtJob,0) as MaintainInventoryAtJob from tblSiteLocation where clientID = '" + databaseCon.Decrypt(id) + "'  and SiteLocID='" + SiteID + "'";
            DataTable dt1 = databaseCon.GetData(querySite);
            if (dt1.Rows.Count > 0)
            {
                obj.GPS = Convert.ToBoolean(dt1.Rows[0]["GPS"]);
                obj.FloorLevel = Convert.ToBoolean(dt1.Rows[0]["FloorLevel"]);
                obj.EnabledGPSSitePin = Convert.ToBoolean(dt1.Rows[0]["EnabledGPSSitePin"]);
                obj.EnabledCaptureGps = Convert.ToBoolean(dt1.Rows[0]["EnabledCaptureGps"]);
                obj.MaintainInventory = Convert.ToBoolean(dt1.Rows[0]["MaintainInventory"]);
                obj.MaintainInventoryAtJob = Convert.ToBoolean(dt1.Rows[0]["MaintainInventoryAtJob"]);
                obj.SkipMaterial = Convert.ToBoolean(dt1.Rows[0]["SkipMaterial"]);
                obj.Address = dt1.Rows[0]["Address"].ToString();
                obj.city = dt1.Rows[0]["city"].ToString();
                obj.state = dt1.Rows[0]["state"].ToString();
                obj.OwnerName = dt1.Rows[0]["OwnerName"].ToString();
                obj.Zip = dt1.Rows[0]["Zip"].ToString();

                if (dt1.Rows[0]["GPS"].ToString() == "True")
                {
                    queryFloor = "select FloorLevelId,FloorLevel from tblFloorlevel where ClientId ='" + databaseCon.Decrypt(id) + "' and FloorLevelStatus='Active' and  sitelocid='" + SiteID + "' and Category = 'GPS' order by SortOrder ASC";
                }
                if (dt1.Rows[0]["FloorLevel"].ToString() == "True")
                {

                    // queryFloor = "select FloorLevel from tblSiteImages where ClientId='" + databaseCon.Decrypt(id) + "' and SiteId='" + SiteID + "'";
                    queryFloor = "select FloorLevelId from tblSiteImages where ClientId='" + databaseCon.Decrypt(id) + "' and SiteId='" + SiteID + "'";
                }

            }

            var GetFloorLebelData = dbcontext.Database.SqlQuery<DropDownViewModel>(queryFloor);
            //var GetLocationData = dbcontext.Database.SqlQuery<DropDownViewModel>("select area,areaid  from tblarea where AreaStatus='Active' and  Area <>'' and Area is not null and ClientID='" + databaseCon.Decrypt(id) + "' and siteLocId='" + SiteID + "' order by area");

            // return Json(new { GetAllSiteData= obj,  GetFloorLebelData = GetFloorLebelData, GetLocationData = GetLocationData}, JsonRequestBehavior.AllowGet);

            return Json(new { GetAllSiteData = obj }, JsonRequestBehavior.AllowGet);
        }

        // public ActionResult ShowregisterDetails(string RID, string id)
        //List<Pro_TBLREGISTER_GETViewModel> GetData = new List<Pro_TBLREGISTER_GETViewModel> { };
        public ActionResult ShowregisterDetails(string RID, string id, string hdnTimeOffSet)
        {
            //string query = "exec Pro_TBLREGISTER_GET '" + databaseCon.Decrypt(Request.QueryString["RID"]) + "','" + databaseCon.Decrypt(Request.QueryString["id"]) + "','',''";
            //var GetData = dbcontext.Database.SqlQuery<Pro_TBLREGISTER_GETViewModel>("exec Pro_TBLREGISTER_GET '" + databaseCon.Decrypt(RID) + "', '" + databaseCon.Decrypt(id) + "', '', ''").FirstOrDefault();

            //  var GetData = dbcontext.Database.SqlQuery<Pro_TBLREGISTER_GETViewModel>("exec Pro_TBLREGISTER_GET '" + databaseCon.Decrypt(RID) + "', '" + databaseCon.Decrypt(id) + "', '', '','','',''," + Convert.ToDouble((hdnTimeOffSet)) * (-1) + "").FirstOrDefault();

            var QueryGetRegisterData = "exec Pro_TBLREGISTER_GET '" + databaseCon.Decrypt(RID) + "', '" + databaseCon.Decrypt(id) + "', '', '','','',''," + Convert.ToDouble((hdnTimeOffSet)) * (-1) + ",'','','',''";
            var GetData = dbcontext.Database.SqlQuery<Pro_TBLREGISTER_GETViewModel>(QueryGetRegisterData).FirstOrDefault();

            //DataTable inventoryPlans = databaseCon.GetData("select Plans from tblclient where clientid='" + databaseCon.Decrypt(id) + "'");//Commented on 07-August-2021
            string queryplan = "select Plans from tblclient where clientid='" + databaseCon.Decrypt(id) + "'";
            DataTable inventoryPlans = databaseCon.GetData(queryplan);
            if (inventoryPlans.Rows.Count > 0)
            {
                GetData.Plans = inventoryPlans.Rows[0]["Plans"].ToString();
            }

            //string queryjob = "select Count(*) as InventoryCount  from tblTransaction where ClientId='" + databaseCon.Decrypt(id) + "' and [From]='Job' and FromId='" + databaseCon.Decrypt(RID) + "' ";
            string queryjob = "select Count(1) as InventoryCount  from tblTransaction where ClientId='" + databaseCon.Decrypt(id) + "' and [From]='Job' and FromId='" + databaseCon.Decrypt(RID) + "' ";
            DataTable dataTable = databaseCon.GetData(queryjob);
            if (dataTable.Rows.Count > 0)
            {
                GetData.InventoryCount = Convert.ToInt32(dataTable.Rows[0]["InventoryCount"]);

                string querydismantle = "select sum(cast((DeliveryAll.DeliveryAvailableQuantity - (Isnull(ReturnAll.ReturnALLQuantity,0) +Isnull(DamagedAll.DamageAllQuantity,0))) as int)) as AvailableQuantity  from (select FromId, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Site' and[to] = 'Job' and TransType = 'Send' and ToId = '" + databaseCon.Decrypt(RID) + "' group by FromId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc, ItemWeight)DeliveryAll left join(select ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site' and FromId = '" + databaseCon.Decrypt(RID) + "' group by FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.InventoryId = ReturnAll.InventoryId  left join (select FromId,ToId, tblDamagedInventoryDetails.InventoryId, Sum(DamagedQuantity)as DamageAllQuantity from tblDamagedInventoryDetails  join tblDamagedInventory on tblDamagedInventoryDetails.DamagedInvId = tblDamagedInventory.DamagedInvId   where [FromName] = 'Job' and[toname] = 'Site' and FromId = '" + databaseCon.Decrypt(RID) + "' group by FromId, ToId, tblDamagedInventoryDetails.InventoryId)DamagedAll on DeliveryAll.FromId =DamagedAll.ToId and DamagedAll.InventoryId= DeliveryAll.InventoryId";

                DataTable dtdismaltle = databaseCon.GetData(querydismantle);
                if (dtdismaltle.Rows.Count > 0)
                {
                    var AvlQuantity = dtdismaltle.Rows[0]["AvailableQuantity"].ToString() == "" ? "0" : dtdismaltle.Rows[0]["AvailableQuantity"].ToString();
                    GetData.AvailableQuantity = Convert.ToInt32(AvlQuantity);
                }
            }
            string queryCopiedFromRID = "select CopiedFromJobRID from tblRegister where RegisterId = '" + databaseCon.Decrypt(RID) + "' and CopiedFromJobRID is not null";
            DataTable copiedFromRID = databaseCon.GetData(queryCopiedFromRID);
            if (copiedFromRID.Rows.Count > 0)
            {
                string queryCopiedRID = "select Right('000000'+Convert(varchar,ScaffRef),6) as ShowCopiedRegisterID from tblRegister where RegisterId = '" + copiedFromRID.Rows[0]["CopiedFromJobRID"].ToString() + "'";
                DataTable copiedRID = databaseCon.GetData(queryCopiedRID);
                GetData.ShowCopiedRegisterID = copiedRID.Rows[0]["ShowCopiedRegisterID"].ToString();
            }
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult btnSave_Click(Pro_RegisterSaveViewModel model)
        {
            var Status = "";
            var Msg = "";
            var RegisterID = "";
            bool requeststatus = false;

            string query;
            DataTable registerdt;
            model.OnlineApplied = true;
            string CrewSize = model.CrewSize;

            int NoOfScafref = model.NoOfScafref;

            bool FloorLevelFlag = false;

            string query1 = "select isnull(FloorLevel,0) as FloorLevel from tblSiteLocation where clientID = '" + databaseCon.Decrypt(model.ClientID) + "'  and SiteLocID='" + model.SiteLocId + "'";
            DataTable dt1 = databaseCon.GetData(query1);
            if (dt1.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt1.Rows[0]["FloorLevel"]) == true)
                {
                    if (model.BuildingID == null)
                    {
                        FloorLevelFlag = true;
                    }
                }
            }


            if (CrewSize == "")
            {
                CrewSize = "null";
            }
            else
            {
                CrewSize = model.CrewSize;
            }

            int workingDocks = 0;
            if (model.NoOfWorkingDecks == "N/A")
            {
                workingDocks = 0;
            }
            else
            {
                workingDocks = Convert.ToInt32(model.NoOfWorkingDecks);
            }

            int noOfPlatform = 0;
            if (model.NumberOfPlatform == "")
            {
                noOfPlatform = 0;
            }
            else
            {
                noOfPlatform = Convert.ToInt32(model.NumberOfPlatform);
            }

            //if (Convert.ToInt32(databaseCon.Decrypt(Request.QueryString["RID"])) > 0)

            //Added On 30-08-2023 To add a check that if material's are requested from job then you can not save blank values in date tagged.
            var RegisterId = databaseCon.Decrypt(model.RegisterId);
            if (model.DateTagged.ToString() == "")
            {
                //string query2 = "select * from tblTransactionHistRequest where [From] = 'Job' and [Fromid] = '" + RegisterId + "' and RequestStatus = 'Confirm'";
                string query2 = "select 'X' as Exists from tblTransactionHistRequest where [From] = 'Job' and [Fromid] = '" + RegisterId + "' and RequestStatus = 'Confirm'";
                DataTable dt2 = databaseCon.GetData(query2);
                if (dt2.Rows.Count > 0)
                {
                    requeststatus = true;
                }
            }
            //Added On 30-08-2023 To add a check that if material's are requested from job then you can not save blank values in date tagged.

            if (RegisterId == null || RegisterId == "")
            {
                RegisterId = "0";
            }

            //Added on 19-12-2023 to add Hidden time off Set to get Current Date According to Client System Date Time. 
            var hdntimeoffset = Convert.ToDouble((model.hdnTimeOffSet)) * (-1);
            var CurrentDate = Convert.ToDateTime(DateTime.Now).AddMinutes(hdntimeoffset);

            if (Convert.ToInt32(RegisterId) > 0)
            {
                string angle = model.PhotoAngle.ToString();

                //ViewState["PhotoAngle"] = imgangle.Value;

                if (FloorLevelFlag != true)
                {
                    //Added on 19-12-2023 to restrict user from saving Date Tagged greater than current date.
                    if (model.DateTagged < CurrentDate || (model.DateTagged) == null)
                    {
                        if (model.DateTagged <= model.DateOfDis || (model.DateOfDis).ToString() == "")
                        {
                            string valuesupervisou = "";
                            if (Session["Valuesuper"] != null)
                            {
                                valuesupervisou = Session["Valuesuper"].ToString();
                            }
                            if (workingDocks >= noOfPlatform)
                            {
                                //Added On 30-08-2023 To add a check that if date tagged was blank then it can be re-saved as blank but once the date built is filled it can not be saved as blank.
                                if (requeststatus != true)
                                {
                                    // if ((model.EngineeredDesign == "YES" && (model.DesignDrawingNo != "" || model.DesignDrawingNo != null)) || (model.EngineeredDesign != "YES" && (model.DesignDrawingNo == "" || model.DesignDrawingNo == null)))
                                    if ((model.EngineeredDesign == "YES" && (model.DesignDrawingNo != null)) || (model.EngineeredDesign != "YES" && (model.DesignDrawingNo == null)))
                                    {
                                        // query = "exec Pro_TBLREGISTER_SAVE '" + databaseCon.Decrypt(Request.QueryString["RID"]) + "','" + txtdesc.Text.Replace("'", "''").Trim() + "','" + databaseCon.Decrypt(Request.QueryString["id"]) + "','" + txtInspectedBy.Text.Replace("'", "''").Trim() + "','" + ddlScaffoldDesign.Text.Replace("'", "''").Trim() + "','" + ddlDutyrating.Text.Replace("'", "''").Trim() + "','" + ddlMaterialType.Text.Replace("'", "''").Trim() + "','" + rdpDateTagged.SelectedDate + "','" + rdpdatedis.SelectedDate + "','','" + databaseCon.Decrypt(Request.QueryString["UID"]) + "','" + DateTime.Now + "','" + txtclientReference.Text.Replace("'", "''").Trim() + "','" + ddlInspectionTimeFrame.SelectedValue + "','" + ddlsites.SelectedValue.Replace("'", "''") + "','" + angle + "'," + (ddbuilding.SelectedValue == "" ? "null" : "'" + ddbuilding.SelectedValue.Replace("'", "''").Trim() + "'") + "," + (ddassets.SelectedValue == "" ? "null" : "'" + ddassets.SelectedValue.Replace("'", "''").Trim() + "'") + ",'" + txtassetsdesc.Text.Replace("'", "''").Trim() + "'," + (txtlength.Text == "" ? "null" : "'" + txtlength.Text + "'") + "," + (txtheight.Text == "" ? "null" : "'" + txtheight.Text + "'") + "," + (txtwidth.Text == "" ? "null" : "'" + txtwidth.Text + "'") + ",'" + DropDownList2.SelectedValue.Replace("'", "''").Trim() + "','" + DropDownList1.Text.Replace("'", "''").Trim() + "','" + ddlLocation.SelectedValue.Replace("'", "''").Trim() + "','" + chkOnline.Checked + "'," + (ddlChecklistInsp.SelectedValue == "" ? "null" : "'" + ddlChecklistInsp.SelectedValue.Replace("'", "''").Trim() + "'") + ",'" + ddlPlanningStatus.SelectedValue.Replace("'", "''").Trim() + "','" + hdnlatitude.Value + "','" + hdnlongitude.Value + "','" + ddlLockedIn.SelectedValue + "','" + txtplnstandDur.Text + "'," + (txtCrewSize.Text == "" ? "null" : "'" + txtCrewSize.Text.Replace("'", "''").Trim() + "'") + "," + (txtManhrserect.Text == "" ? "null" : "'" + txtManhrserect.Text.Replace("'", "''").Trim() + "'") + "," + (txtmanDisman.Text == "" ? "null" : "'" + txtmanDisman.Text.Replace("'", "''").Trim() + "'") + "," + (ddlsupervisor.SelectedValue == "" ? "null" : "'" + ddlsupervisor.SelectedValue.Replace("'", "''").Trim() + "'") + ",'" + txtCrew.Text.Replace("'", "''").Trim() + "','" + txtPermit.Text.Replace("'", "''").Trim() + "','" + txtcomments.Text.Replace("'", "''").Trim() + "','" + txtSapNumber.Text.Replace("'", "''").Trim() + "','" + txtScaffoldOwner.Text.Replace("'", "''").Trim() + "','" + rddateDisAppro.SelectedDate + "','" + txtdiappronotes.Text.Replace("'", "''").Trim() + "','" + txtBearing.Text.Replace("'", "''").Trim() + "'," + (valuesupervisou == "" ? "null" : "'" + valuesupervisou + "'") + "," + checkboxMaintainInventory.Checked + ",'" + txtDesignDrawingNo.Text.Replace("'", "''").Trim() + "','" + txtNoOfPlatform.Text.Replace("'", "''").Trim() + "','" + ddlEngineeredDesign.SelectedValue.Replace("'", "''") + "','" + txtWOnumb.Text.Replace("'", "''").Trim() + "','" + txtScaffType.Text.Replace("'", "''").Trim() + "','" + txtdiscipline.Text.Replace("'", "''").Trim() + "','" + ddlContractor.Text.Replace("'", "''").Trim() + "','" + ddlContractorRep.Text.Replace("'", "''").Trim() + "'";

                                        query = "exec Pro_TBLREGISTER_SAVE '" + databaseCon.Decrypt(model.RegisterId) + "'," + (model.Description == null ? "null" : "'" + model.Description.Replace("'", "''").Trim() + "'") + ",'" + databaseCon.Decrypt(model.ClientID) + "'," + (model.InspectedBy == null ? "null" : "'" + model.InspectedBy.Replace("'", "''").Trim() + "'") + "," + (model.DesignType == null ? "null" : "'" + model.DesignType.Replace("'", "''").Trim() + "'") + "," + (model.DutyRating == null ? "null" : "'" + model.DutyRating.Replace("'", "''").Trim() + "'") + "," + (model.MaterialType == null ? "null" : "'" + model.MaterialType.Replace("'", "''").Trim() + "'") + ",'" + model.DateTagged + "','" + model.DateOfDis + "','','" + databaseCon.Decrypt(model.DoneBy) + "','" + DateTime.Now + "'," + (model.ClientRef == null ? "null" : "'" + model.ClientRef.Replace("'", "''").Trim() + "'") + ",'" + model.InspTimeFrame + "','" + model.SiteLocId + "','" + angle + "'," + (model.BuildingID == null ? "null" : "'" + model.BuildingID + "'") + "," + (model.AssetId == null ? "null" : "'" + model.AssetId + "'") + "," + (model.AssetDesc == null ? "null" : "'" + model.AssetDesc.Replace("'", "''").Trim() + "'") + "," + (model.Length == null ? "null" : "'" + model.Length + "'") + "," + (model.Height == null ? "null" : "'" + model.Height + "'") + "," + (model.Width == null ? "null" : "'" + model.Width + "'") + ",'" + model.NoOfWorkingDecks + "','" + model.FloorLevel + "','" + model.LocationId + "','" + model.OnlineApplied + "'," + (model.CheckListId == null ? "null" : "'" + model.CheckListId + "'") + ",'" + model.PlanningStatus + "','" + model.Lat + "','" + model.Long + "','" + model.LockedIn + "','" + model.PlannedStandingDuration + "'," + (model.CrewSize == null ? "null" : "'" + model.CrewSize + "'") + "," + (model.ManHrsErect == null ? "null" : "'" + model.ManHrsErect + "'") + "," + (model.ManHrsDismantle == null ? "null" : "'" + model.ManHrsDismantle + "'") + "," + (model.SupervisorId == null ? "null" : "'" + model.SupervisorId + "'") + "," + (model.Crew == null ? "null" : "'" + model.Crew.Replace("'", "''").Trim() + "'") + "," + (model.PermitNo == null ? "null" : "'" + model.PermitNo.Replace("'", "''").Trim() + "'") + "," + (model.Comments == null ? "null" : "'" + model.Comments.Replace("'", "''").Trim() + "'") + "," + (model.SAPNumber == null ? "null" : "'" + model.SAPNumber.Replace("'", "''").Trim() + "'") + ",'" + model.ScaffoldOwner + "','" + model.DismantleApprovedDate + "'," + (model.DismantleApprovedNotes == null ? "null" : "'" + model.DismantleApprovedNotes.Replace("'", "''").Trim() + "'") + ",'" + (model.Bearing == null ? "" : model.Bearing.Replace("'", "''").Trim()) + "'," + (model.LockedInSignedOff == null ? "null" : "'" + model.LockedInSignedOff + "'") + "," + model.MaintainInventory + ",'" + model.DesignDrawingNo + "','" + model.NumberOfPlatform + "','" + model.EngineeredDesign + "'," + (model.WONumber == null ? "null" : "'" + model.WONumber.Replace("'", "''").Trim() + "'") + ",'" + (model.ScafType == null ? "" : model.ScafType.Replace("'", "''").Trim()) + "'," + (model.discipline == null ? "null" : "'" + model.discipline.Replace("'", "''").Trim() + "'") + "," + (model.ShiftAllocation == null ? "null" : "'" + model.ShiftAllocation.Replace("'", "''").Trim() + "'") + "," + (model.txtClientScaff == null ? "null" : "'" + model.txtClientScaff.Replace("'", "''").Trim() + "'") + "," + (model.ddlWorkGroups == null ? "null" : "'" + model.ddlWorkGroups.Replace("'", "''").Trim() + "'") + "," + (model.ddlSubGroups == null ? "null" : "'" + model.ddlSubGroups.Replace("'", "''").Trim() + "'") + "," + (model.txtRequestedBy == null ? "null" : "'" + model.txtRequestedBy.Replace("'", "''").Trim() + "'") + "," + model.checkLTStructure + "," + model.chkNoInventory + ",'" + model.PipelineStartDate + "'," + (model.PipeLineNote == null ? "null" : "'" + model.PipeLineNote.Replace("'", "''").Trim() + "'") + "," + model.chkReqis + "," + (model.DismantledBy == null ? "null" : "'" + model.DismantledBy.Replace("'", "''").Trim() + "'") + "," + (model.BuiltBy == null ? "null" : "'" + model.BuiltBy.Replace("'", "''").Trim() + "'") + "," + model.BuildDuringShutdown + "," + model.IsMasterJob + ",'" + model.RTIONotificationNumber + "'";

                                        registerdt = databaseCon.GetData(query);
                                        if (registerdt.Rows.Count != 0)
                                        {
                                            if (Convert.ToInt32(registerdt.Rows[0][0]) > 0)
                                            {
                                                string RID = registerdt.Rows[0][0].ToString();
                                                // UpdateImagePath(RID);
                                                //ManHrsRatioAndManHrsdismantle();
                                                //ShowregisterDetails();
                                                // Session["Insert"] = "Insert";
                                                //  Session["Message"] = Convert.ToString(registerdt.Rows[0][1]);
                                                // Session["Valuesuper"] = null;
                                                //Response.Redirect("Register.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RID=" + Request.QueryString["RID"], false);
                                                Status = "Success";
                                                Msg = "Data save successfully";
                                                RegisterID = RID;
                                            }
                                            else
                                            {
                                                //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                                                //art1.Style.Add("display", "block");
                                                //alertlabel.Text = "<strong>Alert ! </strong>" + registerdt.Rows[0][1].ToString();
                                                Status = "Failed";
                                                Msg = registerdt.Rows[0][1].ToString();
                                            }

                                        }

                                        else
                                        {
                                            //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                                            //art1.Style.Add("display", "block");
                                            //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator ";
                                            Status = "Failed";
                                            Msg = "Syntax Error. Contact administrator";
                                        }
                                    }
                                    else
                                    {
                                        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                                        //art1.Style.Add("display", "block");
                                        //alertlabel.Text = "<strong>Alert ! </strong>Design Drawing Number is applicable when Engineered Design is Yes.";
                                        Status = "Failed";
                                        Msg = "Design Drawing Number is applicable when Engineered Design is Yes.";
                                    }
                                }

                                //Added On 30-08-2023 To add a check that if date tagged was blank then it can be re-saved as blank but once the date built is filled it can not be saved as blank.
                                else
                                {
                                    Status = "Failed";
                                    Msg = "You Can Not Leave Date Built Blank Once You Have Requested Material From Job.";
                                }
                                //Add ended On 30-08-2023 To add a check that if date tagged was blank then it can be re-saved as blank but once the date built is filled it can not be saved as blank.
                            }
                            else
                            {

                                //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                                //art1.Style.Add("display", "block");
                                //alertlabel.Text = "<strong>Alert ! </strong>Max Number of Decks loaded at one time Values Canot be Grater then Working Decks.";
                                Status = "Failed";
                                Msg = "Max Number of Decks loaded at one time Values Canot be Grater then Working Decks.";
                            }
                        }
                        else
                        {
                            //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                            //art1.Style.Add("display", "block");
                            //alertlabel.Text = "<strong>Alert ! </strong> Date Dismantle cannot be less than Date Tagged";
                            Status = "Failed";
                            //Commented on 14-09-2023 Renamed as Date Built
                            //Msg = "Date Dismantle cannot be less than Date Tagged";
                            Msg = "Date Dismantle cannot be less than Date Built";
                        }
                    }
                    else
                    {
                        Status = "Failed";
                        Msg = "Date Built cannot be greater than current date";
                    }
                }
                else
                {
                    //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong>Please Select Building / Module No. then click on save.";
                    Status = "Failed";
                    Msg = "Please Select Building / Module No. then click on save.";
                }
            }

            else
            {
                string angle = model.PhotoAngle.ToString();
                //if (imgangle.Value == "0")
                //    angle = "0";
                //else
                //    angle = imgangle.Value;
                //start-7-mar-2019
                string valuesupervisou = "";
                if (Session["Valuesuper"] != null)
                {
                    valuesupervisou = Session["Valuesuper"].ToString();
                }
                if (FloorLevelFlag != true)
                {

                    //Added on 19-12-2023 to restrict user from saving Date Tagged greater than current date.
                    if (model.DateTagged < CurrentDate || (model.DateTagged) == null)
                    {
                        if (model.DateTagged <= model.DateOfDis || (model.DateOfDis).ToString() == "")
                        {
                            if (workingDocks >= noOfPlatform)
                            {
                                //  if ((model.EngineeredDesign == "YES" && (model.DesignDrawingNo != "" || model.DesignDrawingNo != null)) || (model.EngineeredDesign != "YES" && model.DesignDrawingNo == "" || (model.DesignDrawingNo == null)))
                                if ((model.EngineeredDesign == "YES" && (model.DesignDrawingNo != null)) || (model.EngineeredDesign != "YES" && (model.DesignDrawingNo == null)))
                                {

                                    //  query = "exec Pro_TBLREGISTER_SAVE '" + databaseCon.Decrypt(Request.QueryString["RID"]) + "','" + txtdesc.Text.Replace("'", "''").Trim() + "','" + databaseCon.Decrypt(Request.QueryString["id"]) + "','" + txtInspectedBy.Text.Replace("'", "''").Trim() + "','" + ddlScaffoldDesign.Text.Replace("'", "''").Trim() + "','" + ddlDutyrating.Text.Replace("'", "''").Trim() + "','" + ddlMaterialType.Text.Replace("'", "''").Trim() + "','" + rdpDateTagged.SelectedDate + "','" + rdpdatedis.SelectedDate + "','','" + databaseCon.Decrypt(Request.QueryString["UID"]) + "','" + DateTime.Now + "','" + txtclientReference.Text.Replace("'", "''").Trim() + "','" + ddlInspectionTimeFrame.SelectedValue + "','" + ddlsites.SelectedValue.Replace("'", "''").Trim() + "','" + angle + "'," + (ddbuilding.SelectedValue == "" ? "null" : "'" + ddbuilding.SelectedValue.Replace("'", "''").Trim() + "'") + "," + (ddassets.SelectedValue == "" ? "null" : "'" + ddassets.SelectedValue.Replace("'", "''").Trim() + "'") + ",'" + txtassetsdesc.Text.Replace("'", "''").Trim() + "'," + (txtlength.Text == "" ? "null" : "'" + txtlength.Text + "'") + "," + (txtheight.Text == "" ? "null" : "'" + txtheight.Text + "'") + "," + (txtwidth.Text == "" ? "null" : "'" + txtwidth.Text + "'") + ",'" + DropDownList2.SelectedValue + "','" + DropDownList1.SelectedValue + "','" + ddlLocation.SelectedValue.Replace("'", "''").Trim() + "','" + chkOnline.Checked + "', " + (ddlChecklistInsp.SelectedValue == "" ? "null" : "'" + ddlChecklistInsp.SelectedValue.Replace("'", "''").Trim() + "'") + ",'" + ddlPlanningStatus.SelectedValue.Replace("'", "''").Trim() + "','" + lbllat.Text + "','" + lbllong.Text + "','" + ddlLockedIn.SelectedValue + "','" + txtplnstandDur.Text.Replace("'", "''").Trim() + "'," + (txtCrewSize.Text == "" ? "null" : "'" + txtCrewSize.Text.Replace("'", "''").Trim() + "'") + "," + (txtManhrserect.Text == "" ? "null" : "'" + txtManhrserect.Text + "'") + "," + (txtmanDisman.Text == "" ? "null" : "'" + txtmanDisman.Text.Replace("'", "''").Trim() + "'") + "," + (ddlsupervisor.SelectedValue == "" ? "null" : "'" + ddlsupervisor.SelectedValue.Replace("'", "''").Trim() + "'") + ",'" + txtCrew.Text.Replace("'", "''").Trim() + "','" + txtPermit.Text.Replace("'", "''").Trim() + "','" + txtcomments.Text.Replace("'", "''").Trim() + "','" + txtSapNumber.Text.Replace("'", "''").Trim() + "','" + txtScaffoldOwner.Text.Replace("'", "''").Trim() + "','" + rddateDisAppro.SelectedDate + "','" + txtdiappronotes.Text.Replace("'", "''").Trim() + "','" + txtBearing.Text.Replace("'", "''").Trim() + "'," + (valuesupervisou == "" ? "null" : "'" + valuesupervisou + "'") + "," + checkboxMaintainInventory.Checked + ",'" + txtDesignDrawingNo.Text.Replace("'", "''").Trim() + "','" + txtNoOfPlatform.Text.Replace("'", "''").Trim() + "','" + ddlEngineeredDesign.SelectedValue.Replace("'", "''").Trim() + "','" + txtWOnumb.Text.Replace("'", "''").Trim() + "','" + txtScaffType.Text.Replace("'", "''").Trim() + "','" + txtdiscipline.Text.Replace("'", "''").Trim() + "','" + ddlContractor.Text.Replace("'", "''").Trim() + "','" + ddlContractorRep.Text.Replace("'", "''").Trim() + "'";

                                    query = "exec Pro_TBLREGISTER_SAVE '" + databaseCon.Decrypt(model.RegisterId) + "'," + (model.Description == null ? "null" : "'" + model.Description.Replace("'", "''").Trim() + "'") + ",'" + databaseCon.Decrypt(model.ClientID) + "'," + (model.InspectedBy == null ? "null" : "'" + model.InspectedBy.Replace("'", "''").Trim() + "'") + "," + (model.DesignType == null ? "null" : "'" + model.DesignType.Replace("'", "''").Trim() + "'") + "," + (model.DutyRating == null ? "null" : "'" + model.DutyRating.Replace("'", "''").Trim() + "'") + "," + (model.MaterialType == null ? "null" : "'" + model.MaterialType.Replace("'", "''").Trim() + "'") + ",'" + model.DateTagged + "','" + model.DateOfDis + "','','" + databaseCon.Decrypt(model.DoneBy) + "','" + DateTime.Now + "'," + (model.ClientRef == null ? "null" : "'" + model.ClientRef.Replace("'", "''").Trim() + "'") + ",'" + model.InspTimeFrame + "','" + model.SiteLocId + "','" + angle + "'," + (model.BuildingID == null ? "null" : "'" + model.BuildingID + "'") + "," + (model.AssetId == null ? "null" : "'" + model.AssetId + "'") + "," + (model.AssetDesc == null ? "null" : "'" + model.AssetDesc.Replace("'", "''").Trim() + "'") + "," + (model.Length == null ? "null" : "'" + model.Length + "'") + "," + (model.Height == null ? "null" : "'" + model.Height + "'") + "," + (model.Width == null ? "null" : "'" + model.Width + "'") + ",'" + model.NoOfWorkingDecks + "','" + model.FloorLevel + "','" + model.LocationId + "','" + model.OnlineApplied + "'," + (model.CheckListId == null ? "null" : "'" + model.CheckListId + "'") + ",'" + model.PlanningStatus + "','" + model.Lat + "','" + model.Long + "','" + model.LockedIn + "','" + model.PlannedStandingDuration + "'," + (model.CrewSize == null ? "null" : "'" + model.CrewSize + "'") + "," + (model.ManHrsErect == null ? "null" : "'" + model.ManHrsErect + "'") + "," + (model.ManHrsDismantle == null ? "null" : "'" + model.ManHrsDismantle + "'") + "," + (model.SupervisorId == null ? "null" : "'" + model.SupervisorId + "'") + "," + (model.Crew == null ? "null" : "'" + model.Crew.Replace("'", "''").Trim() + "'") + "," + (model.PermitNo == null ? "null" : "'" + model.PermitNo.Replace("'", "''").Trim() + "'") + "," + (model.Comments == null ? "null" : "'" + model.Comments.Replace("'", "''").Trim() + "'") + "," + (model.SAPNumber == null ? "null" : "'" + model.SAPNumber.Replace("'", "''").Trim() + "'") + ",'" + model.ScaffoldOwner + "','" + model.DismantleApprovedDate + "'," + (model.DismantleApprovedNotes == null ? "null" : "'" + model.DismantleApprovedNotes.Replace("'", "''").Trim() + "'") + ",'" + (model.Bearing == null ? "" : model.Bearing.Replace("'", "''").Trim()) + "'," + (model.LockedInSignedOff == null ? "null" : "'" + model.LockedInSignedOff + "'") + "," + model.MaintainInventory + ",'" + model.DesignDrawingNo + "','" + model.NumberOfPlatform + "','" + model.EngineeredDesign + "'," + (model.WONumber == null ? "null" : "'" + model.WONumber.Replace("'", "''").Trim() + "'") + ",'" + (model.ScafType == null ? "" : model.ScafType.Replace("'", "''").Trim()) + "'," + (model.discipline == null ? "null" : "'" + model.discipline.Replace("'", "''").Trim() + "'") + "," + (model.ShiftAllocation == null ? "null" : "'" + model.ShiftAllocation.Replace("'", "''").Trim() + "'") + "," + (model.txtClientScaff == null ? "null" : "'" + model.txtClientScaff.Replace("'", "''").Trim() + "'") + "," + (model.ddlWorkGroups == null ? "null" : "'" + model.ddlWorkGroups.Replace("'", "''").Trim() + "'") + "," + (model.ddlSubGroups == null ? "null" : "'" + model.ddlSubGroups.Replace("'", "''").Trim() + "'") + "," + (model.txtRequestedBy == null ? "null" : "'" + model.txtRequestedBy.Replace("'", "''").Trim() + "'") + "," + model.checkLTStructure + "," + model.chkNoInventory + ",'" + model.PipelineStartDate + "'," + (model.PipeLineNote == null ? "null" : "'" + model.PipeLineNote.Replace("'", "''").Trim() + "'") + "," + model.chkReqis + "," + (model.DismantledBy == null ? "null" : "'" + model.DismantledBy.Replace("'", "''").Trim() + "'") + "," + (model.BuiltBy == null ? "null" : "'" + model.BuiltBy.Replace("'", "''").Trim() + "'") + "," + model.BuildDuringShutdown + "," + model.IsMasterJob + ",'" + model.RTIONotificationNumber + "'";
                                    for (int i = 1; i <= NoOfScafref; i++)
                                    {
                                        registerdt = databaseCon.GetData(query);

                                        if (registerdt.Rows.Count != 0)
                                        {
                                            if (Convert.ToInt32(registerdt.Rows[0][0]) > 0)
                                            {
                                                string RID = registerdt.Rows[0][0].ToString();
                                                // UpdateImagePath(RID);
                                                //ManHrsRatioAndManHrsdismantle();
                                                //Session["Insert"] = "Insert";
                                                //Session["Valuesuper"] = null;
                                                //Session["Message"] = Convert.ToString(registerdt.Rows[0][1]);
                                                //Response.Redirect("Register.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RID=" + databaseCon.Encrypt(Convert.ToInt32(registerdt.Rows[0][0]).ToString()), false);

                                                Status = "Success";
                                                Msg = "Data save successfully";
                                                RegisterID = RID;
                                            }
                                            else
                                            {

                                                //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                                                //art1.Style.Add("display", "block");
                                                //alertlabel.Text = "<strong>Alert ! </strong>" + registerdt.Rows[0][1].ToString();
                                                Status = "Failed";
                                                Msg = registerdt.Rows[0][1].ToString();
                                            }
                                        }
                                        else
                                        {
                                            //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                                            //art1.Style.Add("display", "block");
                                            //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator ";
                                            Status = "Failed";
                                            Msg = "Syntax Error. Contact administrator";
                                        }

                                    }
                                }
                                else
                                {
                                    //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                                    //art1.Style.Add("display", "block");
                                    //alertlabel.Text = "<strong>Alert ! </strong>Design Drawing Number is applicable when Engineered Design is Yes.";
                                    Status = "Failed";
                                    Msg = "Design Drawing Number is applicable when Engineered Design is Yes.";
                                }
                            }
                            else
                            {

                                //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                                //art1.Style.Add("display", "block");
                                //alertlabel.Text = "<strong>Alert ! </strong>Max Number of Decks loaded at one time Values Canot be Grater then Working Decks.";
                                Status = "Failed";
                                Msg = "Alert ! Max Number of Decks loaded at one time Values Canot be Grater then Working Decks.";
                            }
                        }
                        else
                        {
                            Status = "Failed";
                            Msg = "Date Dismantle cannot be less than Date Built";
                        }
                    }
                    else
                    {
                        Status = "Failed";
                        Msg = "Date Built cannot be greater than current date";
                    }
                }
                else
                {
                    //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong>Please Select Building / Module No. then click on save.";
                    Status = "Failed";
                    Msg = "Please Select Building / Module No. then click on save.";
                }
            }

            return Json(new { Result = Status, Message = Msg, RegisterID = RegisterID }, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult UpdateImagePath(string id, string RID, string txtOwnername, string radUpdogImg, string imgname, string tempimg, string basetempimg)
        //{

        //    try
        //    {
        //        int RegisterID = Convert.ToInt32(RID);
        //        if (RegisterID > 0)
        //        {
        //            string extension = "";
        //            string FileName = "";
        //            string path = "/images/sites/";
        //            path = Server.MapPath(path);
        //            //string Newpath = "/images/sites/";
        //            string newpath2 = "";
        //            string QueryU = "update tblRegister set";
        //            string NewFileName = "";
        //            //if (radUpdogImg.UploadedFiles.Count > 0)
        //            if (radUpdogImg != "")
        //            {
        //                //string dotscount = hdfield.Value;
        //                //if (Convert.ToInt32(dotscount) <= 1)
        //                //{
        //                // extension = radUpdogImg.UploadedFiles[0].GetExtension();
        //                extension = System.IO.Path.GetExtension(radUpdogImg);
        //                if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".ico" || extension == ".gif" || extension == ".bmp" || extension == ".JPG" || extension == ".JPEG" || extension == ".PNG" || extension == ".ICO" || extension == ".GIF" || extension == ".BMP")
        //                {

        //                    FileName = txtOwnername.Replace("'", "''").Replace(" ", "").Trim() + "_" + RegisterID + "_" + DateTime.Now.Ticks + extension;

        //                    newpath2 = path + FileName;
        //                    // ShowregisterDetails();
        //                    //  if (ViewState["imgname"].ToString() != "")

        //                    HttpFileCollectionBase files = Request.Files;

        //                    for (int k = 0; k < 1; k++)
        //                    {

        //                        HttpPostedFileBase postedFile = files[k];

        //                        if (imgname != "")
        //                        {
        //                            // string clientimgget = ViewState["imgname"].ToString();
        //                            string clientimgget = imgname;
        //                            string imgfile = path + clientimgget;
        //                            System.IO.File.Delete(imgfile);
        //                            //File.Delete(ViewState["imgname"].ToString());
        //                            //uploadlogo.SaveAs(newpath2);
        //                            // radUpdogImg.UploadedFiles[0].SaveAs(newpath2);
        //                            postedFile.SaveAs(newpath2);
        //                            Image img = Image.FromFile(newpath2);

        //                            NewFileName = txtOwnername.Replace("'", "''").Replace(" ", "").Trim() + "_" + RegisterID + "_" + DateTime.Now.Ticks + extension;
        //                            if (img.PropertyIdList.Contains(0x0112))
        //                            {

        //                                PropertyItem propOrientation = img.GetPropertyItem(0x0112);
        //                                short orientation = BitConverter.ToInt16(propOrientation.Value, 0);
        //                                if (orientation == 6)
        //                                {
        //                                    img.RotateFlip(RotateFlipType.Rotate90FlipNone);
        //                                }
        //                                else if (orientation == 8)
        //                                {
        //                                    img.RotateFlip(RotateFlipType.Rotate270FlipNone);
        //                                }
        //                                else if (orientation == 3)
        //                                {
        //                                    img.RotateFlip(RotateFlipType.Rotate180FlipNone);
        //                                }
        //                                else
        //                                {
        //                                    // Do nothing
        //                                }

        //                                var newWidth = (int)(img.Width * 0.5);
        //                                var newHeight = (int)(img.Height * 0.5);
        //                                var thumbnailImg = new Bitmap(newWidth, newHeight);
        //                                var thumbGraph = Graphics.FromImage(thumbnailImg);
        //                                thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
        //                                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
        //                                thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //                                var imageRectangle = new System.Drawing.Rectangle(0, 0, newWidth, newHeight);

        //                                thumbGraph.DrawImage(img, imageRectangle);

        //                                byte[] bte = GetCompressedBitmap(thumbnailImg, 75);
        //                                MemoryStream ms2 = new MemoryStream(bte);
        //                                System.Drawing.Image img12 = System.Drawing.Image.FromStream(ms2);

        //                                thumbnailImg.Save(path + NewFileName, img12.RawFormat);
        //                            }

        //                            else
        //                            {
        //                                var newWidth = (int)(img.Width * 0.5);
        //                                var newHeight = (int)(img.Height * 0.5);
        //                                var thumbnailImg = new Bitmap(newWidth, newHeight);
        //                                var thumbGraph = Graphics.FromImage(thumbnailImg);
        //                                thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
        //                                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
        //                                thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //                                var imageRectangle = new System.Drawing.Rectangle(0, 0, newWidth, newHeight);

        //                                thumbGraph.DrawImage(img, imageRectangle);

        //                                byte[] bte = GetCompressedBitmap(thumbnailImg, 50);
        //                                MemoryStream ms2 = new MemoryStream(bte);
        //                                System.Drawing.Image img12 = System.Drawing.Image.FromStream(ms2);

        //                                thumbnailImg.Save(path + NewFileName, img12.RawFormat);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (!Directory.Exists(path))
        //                            {
        //                                Directory.CreateDirectory(path);
        //                            }
        //                            //uploadlogo.SaveAs(newpath2);

        //                            // radUpdogImg.UploadedFiles[0].SaveAs(newpath2);
        //                            postedFile.SaveAs(newpath2);

        //                            Image img = Image.FromFile(newpath2);

        //                            NewFileName = txtOwnername.Replace("'", "''").Replace(" ", "").Trim() + "_" + RegisterID + "_" + DateTime.Now.Ticks + extension;

        //                            if (img.PropertyIdList.Contains(0x0112))
        //                            {

        //                                PropertyItem propOrientation = img.GetPropertyItem(0x0112);
        //                                short orientation = BitConverter.ToInt16(propOrientation.Value, 0);
        //                                if (orientation == 6)
        //                                {
        //                                    img.RotateFlip(RotateFlipType.Rotate90FlipNone);
        //                                }
        //                                else if (orientation == 8)
        //                                {
        //                                    img.RotateFlip(RotateFlipType.Rotate270FlipNone);
        //                                }
        //                                else if (orientation == 3)
        //                                {
        //                                    img.RotateFlip(RotateFlipType.Rotate180FlipNone);
        //                                }
        //                                else
        //                                {
        //                                    // Do nothing
        //                                }

        //                                var newWidth = (int)(img.Width * 0.5);
        //                                var newHeight = (int)(img.Height * 0.5);
        //                                var thumbnailImg = new Bitmap(newWidth, newHeight);
        //                                var thumbGraph = Graphics.FromImage(thumbnailImg);
        //                                thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
        //                                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
        //                                thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //                                var imageRectangle = new System.Drawing.Rectangle(0, 0, newWidth, newHeight);

        //                                thumbGraph.DrawImage(img, imageRectangle);

        //                                byte[] bte = GetCompressedBitmap(thumbnailImg, 75);
        //                                MemoryStream ms2 = new MemoryStream(bte);
        //                                System.Drawing.Image img12 = System.Drawing.Image.FromStream(ms2);

        //                                thumbnailImg.Save(path + NewFileName, img12.RawFormat);
        //                            }

        //                            else
        //                            {
        //                                var newWidth = (int)(img.Width * 0.5);
        //                                var newHeight = (int)(img.Height * 0.5);
        //                                var thumbnailImg = new Bitmap(newWidth, newHeight);
        //                                var thumbGraph = Graphics.FromImage(thumbnailImg);
        //                                thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
        //                                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
        //                                thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //                                var imageRectangle = new System.Drawing.Rectangle(0, 0, newWidth, newHeight);

        //                                thumbGraph.DrawImage(img, imageRectangle);

        //                                byte[] bte = GetCompressedBitmap(thumbnailImg, 50);
        //                                MemoryStream ms2 = new MemoryStream(bte);
        //                                System.Drawing.Image img12 = System.Drawing.Image.FromStream(ms2);

        //                                thumbnailImg.Save(path + NewFileName, img12.RawFormat);
        //                            }



        //                        }
        //                        // if (ViewState["imgname"].ToString() != "")
        //                        if (imgname != "")
        //                        {
        //                            // databaseCon.deletefileonblob(ViewState["imgname"].ToString(), "siteimage");
        //                        }
        //                        //QueryU += " Image='" + FileName + "' where RegisterID=" + RegisterID;
        //                        //QueryU += " Image='" + NewFileName + "' ,OriginalBaseImage ='" + basetempimg + "' where RegisterID=" + RegisterID;
        //                        QueryU += " Image='" + NewFileName.Replace("'", "''") +
        //  "', OriginalBaseImage='" + basetempimg +
        //  "' where RegisterID=" + RegisterID;


        //                        int count = databaseCon.RunCmd(QueryU);
        //                        string fpath = path + NewFileName;
        //                        //databaseCon.savefileonblob(FileName, "Site-images", fpath, "image/png");
        //                        // File.Delete(fpath);
        //                        // RotatedImageSave(RID);
        //                    }
        //                }
        //                // }

        //            }
        //            //else if (tempimg.Value != null && tempimg.Value != "")
        //            else if (tempimg != null && tempimg != "")
        //            {
        //                // string path = "~/uploads/";
        //                //path = Server.MapPath(path);
        //                string ImageData = tempimg;
        //                FileName = txtOwnername.Replace("'", "''").Replace(" ", "").Trim() + "_" + RegisterID + "_" + DateTime.Now.Ticks + ".jpg";
        //                // FileName = "SketchInsp" + "_" + DateTime.Now.Ticks + "_" + Request.QueryString["InspID"] + ".png";
        //                string SketchnewPath = path + FileName;

        //                byte[] bytes = Convert.FromBase64String(ImageData.Split(',')[1]);


        //                if (System.IO.File.Exists(SketchnewPath))
        //                {
        //                    string fpath = path + FileName;
        //                    //databaseCon.savefileonblob(FileName, "Site-images", fpath, "image/png");
        //                    System.IO.File.Delete(fpath);

        //                    using (var imageFile = new FileStream(SketchnewPath, FileMode.Create))
        //                    {
        //                        imageFile.Write(bytes, 0, bytes.Length);
        //                        imageFile.Flush();

        //                    }
        //                }
        //                else
        //                {
        //                    if (!Directory.Exists(path))
        //                    {
        //                        Directory.CreateDirectory(path);
        //                    }

        //                    using (var imageFile = new FileStream(SketchnewPath, FileMode.Create))
        //                    {
        //                        imageFile.Write(bytes, 0, bytes.Length);
        //                        imageFile.Flush();
        //                    }
        //                }

        //                // QueryU += " Image='" + FileName + "' where RegisterID=" + RegisterID;
        //                QueryU += " Image='" + FileName + "', OriginalBaseImage ='" + tempimg + "' where RegisterID=" + RegisterID;
        //                int count = databaseCon.RunCmd(QueryU);
        //                // RotatedImageSave(RID);
        //            }

        //            else
        //            {
        //                //  RotatedImageSave(RID);
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return Json(new { Result = "Success", Message = "Image Save Successfully ." }, JsonRequestBehavior.AllowGet);
        //}




        public ActionResult UpdateImagePath(string id, string RID, string txtOwnername, string radUpdogImg, string imgname, string tempimg, string basetempimg)
        {

            try
            {
                int RegisterID = Convert.ToInt32(RID);
                string path = Server.MapPath("/images/sites/");
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                string NewFileName = "";
                string QueryU = "";

                // Fix 1: Null-safe handling for OwnerName to avoid 'Object reference' error
                // Also removing quotes for the physical file name to avoid OS issues
                string safeOwnerNameForFile = (txtOwnername ?? "").Replace("'", "").Replace(" ", "").Trim();

                if (!string.IsNullOrEmpty(radUpdogImg))
                {
                    HttpFileCollectionBase files = Request.Files;
                    if (files.Count > 0)
                    {
                        HttpPostedFileBase postedFile = files[0];
                        string extension = Path.GetExtension(radUpdogImg).ToLower();

                        // Delete old image if exists
                        if (!string.IsNullOrEmpty(imgname))
                        {
                            string oldPath = Path.Combine(path, imgname);
                            if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                        }

                        // Create Unique File Name
                        NewFileName = safeOwnerNameForFile + "_" + RegisterID + "_" + DateTime.Now.Ticks + extension;
                        string finalSavePath = Path.Combine(path, NewFileName);

                        using (Stream fs = postedFile.InputStream)
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                fs.CopyTo(ms);
                                ms.Position = 0;

                                try
                                {
                                    using (Bitmap tempBmp = new Bitmap(ms))
                                    {
                                        // Orientation Fix
                                        if (tempBmp.PropertyIdList.Contains(0x0112))
                                        {
                                            PropertyItem prop = tempBmp.GetPropertyItem(0x0112);
                                            int val = BitConverter.ToInt16(prop.Value, 0);
                                            if (val == 6) tempBmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                                            else if (val == 8) tempBmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
                                            else if (val == 3) tempBmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
                                        }

                                        int newWidth = (int)(tempBmp.Width * 0.5);
                                        int newHeight = (int)(tempBmp.Height * 0.5);

                                        using (Bitmap thumbnailImg = new Bitmap(newWidth, newHeight))
                                        using (Graphics thumbGraph = Graphics.FromImage(thumbnailImg))
                                        {
                                            thumbGraph.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                            thumbGraph.DrawImage(tempBmp, 0, 0, newWidth, newHeight);

                                            // Fix 2: Force save as Jpeg to avoid "RawFormat" or extension mismatch issues
                                            thumbnailImg.Save(finalSavePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                                        }
                                    }
                                }
                                catch
                                {
                                    // Recovery logic: if resize fails, save original file
                                    ms.Position = 0;
                                    using (FileStream file = new FileStream(finalSavePath, FileMode.Create, FileAccess.Write))
                                    {
                                        ms.WriteTo(file);
                                    }
                                }
                            }
                        }

                        // Fix 3: Escape single quotes for SQL Update
                        string dbFileName = NewFileName.Replace("'", "''");
                        QueryU = "UPDATE tblRegister SET Image='" + dbFileName + "', OriginalBaseImage='" + basetempimg + "' WHERE RegisterID=" + RegisterID;
                        databaseCon.RunCmd(QueryU);
                    }
                }
                else if (!string.IsNullOrEmpty(tempimg))
                {
                    NewFileName = safeOwnerNameForFile + "_" + RegisterID + "_" + DateTime.Now.Ticks + ".jpg";
                    string pureBase64 = tempimg.Contains(",") ? tempimg.Split(',')[1] : tempimg;
                    byte[] imageBytes = Convert.FromBase64String(pureBase64);

                    System.IO.File.WriteAllBytes(Path.Combine(path, NewFileName), imageBytes);

                    // Fix 3: Escape single quotes for SQL Update
                    string dbFileName = NewFileName.Replace("'", "''");
                    QueryU = "UPDATE tblRegister SET Image='" + dbFileName + "', OriginalBaseImage='" + tempimg + "' WHERE RegisterID=" + RegisterID;
                    databaseCon.RunCmd(QueryU);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Result = "Success", Message = "Image Saved Successfully." }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult QRCodeGenerate(string id, string RID, string scafRef)
        {
            // string SketchnewPath = "";
            // string scafRef = txtscaffref.Text;
            string clintid = databaseCon.Decrypt(id);
            string code = "QR" + clintid + "_" + scafRef;



            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            // QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q); // Comment On 2 May 2020

            QRCodeData qrCodeData = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);

            System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
            imgBarCode.Width = 150;
            imgBarCode.Attributes.Add("class", "img-responsive");
            imgBarCode.ID = "imageQRCode";
            using (Bitmap bitMap = qrCode.GetGraphic(20))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] byteImage = ms.ToArray();
                    imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                }
                // QRCodeImage.Controls.Add(imgBarCode);

                string regQuery = "select QRCodeImage from tblRegister where registerid ='" + databaseCon.Decrypt(Request.QueryString["RID"]) + "'";
                DataTable datatable = databaseCon.GetData(regQuery);
                string QrCode = (datatable.Rows[0]["QRCodeImage"]).ToString();
                if (QrCode != code)
                {
                    string query = "update tblregister set QRCodeImage='" + code + "' where registerID='" + databaseCon.Decrypt(Request.QueryString["RID"]) + "'";
                    int i = databaseCon.RunCmd(query);
                }
                return Json(imgBarCode, JsonRequestBehavior.AllowGet);
            }
        }


        // For Floor Label Marks In Job Save Start
        public int FloorLabelMarksSave(string id, string RID, string XPosition, string YPosition)
        // public int FloorLabelMarksSave(string id)
        {


            int updt = 0;
            string query = "";
            if ((XPosition == null || XPosition == "") && (YPosition == null || YPosition == ""))
            {
                XPosition = "null";
                YPosition = "null";
                query = "Update tblregister set XPosition= " + XPosition + " ,YPosition= " + YPosition + " where ClientID='" + id + "' and RegisterId='" + RID + "'";
                updt = databaseCon.RunCmdID(query);

            }

            else
            {
                query = "Update tblregister set XPosition='" + XPosition + "',YPosition='" + YPosition + "' where ClientID='" + id + "' and RegisterId='" + RID + "'";
                updt = databaseCon.RunCmdID(query);
            }

            return updt;
        }

        // For Floor Label Marks In Job Save End

        // For Floor Label Marks In Job  Get Start

        public class FloorLabelMarksGetModel
        {
            public string SiteFloorLevelImage { get; set; }
            public string XPosition { get; set; }
            public string YPosition { get; set; }
        }
        // public List<Scaffold> FloorLabelMarksGet(string id, string siteID, string FloorLabel, string RID)
        public ActionResult FloorLabelMarksGet(string id, string siteID, string FloorLabelID, string RID, string LocationId, string BuildingId)
        {
            var SiteFloorImages = "";
            // string query1 = "select ImageName from  tblSiteImages where ClientID='" + id + "' and SiteID='" + siteID + "'  and FloorLevel='" + FloorLabel + "'";

            string queryFloor = "";
            string BuildingID;
            if (BuildingId != "")
            {
                BuildingID = BuildingId;
            }
            else
            {
                BuildingID = "0";
            }

            string getFloor = "select ImageName from  tblSiteImages where ClientID='" + id + "' and SiteID='" + siteID + "'  and FloorLevelId='" + FloorLabelID + "' and LocId='" + LocationId + "' and BuildingId='" + BuildingID + "'";// Check Images For Building

            DataTable dtgetFloor = databaseCon.GetData(getFloor);
            if (dtgetFloor.Rows.Count > 0)
            {
                queryFloor = "select ImageName from  tblSiteImages where ClientID='" + id + "' and SiteID='" + siteID + "'  and FloorLevelId='" + FloorLabelID + "' and LocId='" + LocationId + "' and BuildingId='" + BuildingID + "'";
            }
            else
            {
                string getFloor1 = "select ImageName from  tblSiteImages where ClientID='" + id + "' and SiteID='" + siteID + "'  and FloorLevelId='" + FloorLabelID + "' and LocId='" + LocationId + "' and (BuildingID = '0' or BuildingID is Null)"; // Check Images For Location

                DataTable dtgetFloor1 = databaseCon.GetData(getFloor1);

                if (dtgetFloor1.Rows.Count > 0)
                {
                    queryFloor = "select ImageName from  tblSiteImages where ClientID='" + id + "' and SiteID='" + siteID + "'  and FloorLevelId='" + FloorLabelID + "' and LocId='" + LocationId + "' and (BuildingID = '0' or BuildingID is Null)";
                }
                else
                {
                    queryFloor = "select ImageName from  tblSiteImages where ClientID='" + id + "' and SiteID='" + siteID + "'  and FloorLevelId='" + FloorLabelID + "' and (LocID = '0' or LocID is Null) and (BuildingID = '0' or BuildingID is Null)";// Check Images For Sites only              
                }

            }

            DataTable dt = databaseCon.GetData(queryFloor);
            if (dt.Rows.Count > 0)
            {
                SiteFloorImages = dt.Rows[0]["ImageName"].ToString();

            }

            var GetData = dbcontext.Database.SqlQuery<FloorLabelMarksGetModel>("select XPosition,YPosition from tblregister where ClientID='" + id + "' and SiteLocID='" + siteID + "' and RegisterId='" + RID + "'").FirstOrDefault();
            if (GetData != null)
            {
                GetData.SiteFloorLevelImage = SiteFloorImages;
            }
            return Json(GetData, JsonRequestBehavior.AllowGet);

        }


        // For Floor Label Marks In Job  Get End

        public ActionResult GetSiteLatOrLong(string clientID, string SiteID)
        {
            var SiteLat = "";
            var SiteLong = "";
            string SItequeryforLatAndLong = "select SiteLat,SiteLong from tblSiteLocation where SiteLocId= '" + SiteID + "' and ClientID='" + clientID + "'";
            DataTable dtSiteLatLong = databaseCon.GetData(SItequeryforLatAndLong);
            if (dtSiteLatLong.Rows.Count > 0)
            {
                SiteLat = dtSiteLatLong.Rows[0]["SiteLat"].ToString();
                SiteLong = dtSiteLatLong.Rows[0]["SiteLong"].ToString();
            }

            return Json(SiteLat + "_" + SiteLong, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLockDownSignOffData(string id, string RID)
        {

            string query = "select DesignType,MaterialType,ManHrsErect,ManHrsDismantle,HierarchySignedOff  from tblRegister where RegisterId='" + databaseCon.Decrypt(RID) + "'";
            var GetData = dbcontext.Database.SqlQuery<Pro_TBLREGISTER_GETViewModel>("select DesignType,MaterialType,ManHrsErect,ManHrsDismantle,HierarchySignedOff  from tblRegister where RegisterId='" + databaseCon.Decrypt(RID) + "'").FirstOrDefault();

            return Json(GetData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult btnPre_Click(string id, string RID, string UID, string UserRole)
        {
            string query;
            DataTable dt;
            string RegId = "";
            int ScafRef = 0;
            var SiteOwnerName = "";
            var ScaffoldOwner = "";
            var SiteName = "";
            var LocationName = "";
            var Result = "";
            var Message = "";

            //  string query1 = "select case when SiteOwnerName is null or SiteOwnerName='' then 'All' Else SiteOwnerName end as SiteOwnerName, case when ScaffoldOwner is null or ScaffoldOwner='' then 'All' Else ScaffoldOwner end  ScaffoldOwner from tblUser where ClientId='" + HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["id"])) + "' and UserId='" + HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["UID"])) + "'";

            string query1 = "select case when SiteOwnerName is null or SiteOwnerName='' then 'All' Else SiteOwnerName end as SiteOwnerName, case when ScaffoldOwner is null or ScaffoldOwner='' then 'All' Else ScaffoldOwner end  ScaffoldOwner, case when Site is null or Site='' then 'All' Else Site end as Site, case when Location is null or Location='' then 'All' Else Location end as Location from tblUser where ClientId='" + databaseCon.Decrypt(id) + "' and UserId='" + databaseCon.Decrypt(UID) + "'";
            DataTable dt1 = databaseCon.GetData(query1);
            if (dt1.Rows.Count > 0)
            {

                SiteOwnerName = dt1.Rows[0]["SiteOwnerName"].ToString();
                ScaffoldOwner = dt1.Rows[0]["ScaffoldOwner"].ToString();
                SiteName = dt1.Rows[0]["Site"].ToString();
                LocationName = dt1.Rows[0]["Location"].ToString();
            }

            // query = "select registerid,ScaffRef from tblregister join tblsiteLocation on tblRegister.SiteLocId=tblSiteLocation.SiteLocID where SiteLocationStatus='Active' and tblregister.ClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and  registerid='" + databaseCon.Decrypt(Request.QueryString["RID"]) + "'";
            query = "select registerid,ScaffRef from tblregister join tblsiteLocation on tblRegister.SiteLocId=tblSiteLocation.SiteLocID where SiteLocationStatus='Active' and tblregister.ClientId='" + databaseCon.Decrypt(id) + "' and  registerid='" + databaseCon.Decrypt(RID) + "'";
            dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                RegId = dt.Rows[0]["registerid"].ToString();
                ScafRef = Convert.ToInt32(dt.Rows[0]["ScaffRef"]);
                //ScafRef -= 1;
                // query = "select Top 1 ScaffRef,registerid from tblRegister join tblsiteLocation on tblRegister.SiteLocId=tblSiteLocation.SiteLocID    where SiteLocationStatus='Active' and tblregister.ClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and ScaffRef<'" + ScafRef + "'";
                query = "select Top 1 ScaffRef,registerid from tblRegister join tblsiteLocation on tblRegister.SiteLocId=tblSiteLocation.SiteLocID    where SiteLocationStatus='Active' and tblregister.ClientId='" + databaseCon.Decrypt(id) + "' and ScaffRef<'" + ScafRef + "'";
                if ((SiteOwnerName != "" && SiteOwnerName != "All") || (ScaffoldOwner != "" && ScaffoldOwner != "All") || (SiteName != "" && SiteName != "All") || (LocationName != "" && LocationName != "All"))
                {
                    query = query + "and (OwnerName = '" + SiteOwnerName.Replace("'", "''") + "' OR 'All' = '" + SiteOwnerName.Replace("'", "''") + "') and(ScaffoldOwner = '" + ScaffoldOwner.Replace("'", "''") + "' OR 'All' = '" + ScaffoldOwner.Replace("'", "''") + "') and (tblRegister.SiteLocID = '" + SiteName.Replace("'", "''") + "' OR 'All' = '" + SiteName.Replace("'", "''") + "') and (LocationId = '" + LocationName.Replace("'", "''") + "' OR 'All' = '" + LocationName.Replace("'", "''") + "')";
                }

                query = query + "order by ScaffRef desc";
                dt = databaseCon.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    RegId = dt.Rows[0]["registerid"].ToString();
                    ScafRef = Convert.ToInt32(dt.Rows[0]["ScaffRef"]);

                    var ii = databaseCon.Encrypt(RegId);
                    var Des = databaseCon.Decrypt(ii);

                    //Response.Redirect("Register.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&Datetagged=" + Request.QueryString["Datetagged"] + "&RID=" + databaseCon.Encrypt(RegId), false);
                    Result = "Success";
                    //Message = "/Register/Register?UID='"+UID +"'&UserRole='"+UserRole+"'&id='"+id+ "'&RID='" + databaseCon.Encrypt(RegId) + "";
                    Message = databaseCon.Encrypt(RegId);
                }
                else
                {
                    //QRCodeGenerate();
                    //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong> This is the first job. Please click Next Button.";
                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp", "<script>hideAlert();</script>", false);
                    Result = "Failed";
                    Message = "This is the first job. Please click Next Button.";
                }
            }
            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult btnNext_Click(string id, string RID, string UID, string UserRole)
        {
            string query;
            DataTable dt;
            string RegId = "";
            int ScafRef = 0;
            var SiteOwnerName = "";
            var ScaffoldOwner = "";
            var SiteName = "";
            var LocationName = "";
            var Result = "";
            var Message = "";

            //  string query1 = "select case when SiteOwnerName is null or SiteOwnerName='' then 'All' Else SiteOwnerName end as SiteOwnerName, case when ScaffoldOwner is null or ScaffoldOwner='' then 'All' Else ScaffoldOwner end  ScaffoldOwner from tblUser where ClientId='" + HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["id"])) + "' and UserId='" + HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["UID"])) + "'";
            string query1 = "select case when SiteOwnerName is null or SiteOwnerName='' then 'All' Else SiteOwnerName end as SiteOwnerName, case when ScaffoldOwner is null or ScaffoldOwner='' then 'All' Else ScaffoldOwner end  ScaffoldOwner, case when Site is null or Site='' then 'All' Else Site end as Site, case when Location is null or Location='' then 'All' Else Location end as Location from tblUser where ClientId='" + databaseCon.Decrypt(id) + "' and UserId='" + databaseCon.Decrypt(UID) + "'";
            DataTable dt1 = databaseCon.GetData(query1);
            if (dt1.Rows.Count > 0)
            {

                SiteOwnerName = dt1.Rows[0]["SiteOwnerName"].ToString();
                ScaffoldOwner = dt1.Rows[0]["ScaffoldOwner"].ToString();
                SiteName = dt1.Rows[0]["Site"].ToString();
                LocationName = dt1.Rows[0]["Location"].ToString();
            }
            // query = "select registerid,ScaffRef from tblregister join tblsiteLocation on tblRegister.SiteLocId=tblSiteLocation.SiteLocID where SiteLocationStatus='Active' and tblregister.ClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and  registerid='" + databaseCon.Decrypt(Request.QueryString["RID"]) + "'";
            query = "select registerid,ScaffRef from tblregister join tblsiteLocation on tblRegister.SiteLocId=tblSiteLocation.SiteLocID where SiteLocationStatus='Active' and tblregister.ClientId='" + databaseCon.Decrypt(id) + "' and  registerid='" + databaseCon.Decrypt(RID) + "'";
            dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                RegId = dt.Rows[0]["registerid"].ToString();
                ScafRef = Convert.ToInt32(dt.Rows[0]["ScaffRef"]);
                //ScafRef += 1;
                // query = "select Top 1 ScaffRef,registerid from tblRegister join tblsiteLocation on tblRegister.SiteLocId=tblSiteLocation.SiteLocID    where SiteLocationStatus='Active' and tblregister.ClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and ScaffRef>'" + ScafRef + "'";
                query = "select Top 1 ScaffRef,registerid from tblRegister join tblsiteLocation on tblRegister.SiteLocId=tblSiteLocation.SiteLocID    where SiteLocationStatus='Active' and tblregister.ClientId='" + databaseCon.Decrypt(id) + "' and ScaffRef>'" + ScafRef + "'";
                if ((SiteOwnerName != "" && SiteOwnerName != "All") || (ScaffoldOwner != "" && ScaffoldOwner != "All") || (SiteName != "" && SiteName != "All") || (LocationName != "" && LocationName != "All"))
                {
                    //query = query + "and (OwnerName = '" + SiteOwnerName.Replace("'", "''") + "' OR 'All' = '" + SiteOwnerName.Replace("'", "''") + "') and(ScaffoldOwner = '" + ScaffoldOwner.Replace("'", "''") + "' OR 'All' = '" + ScaffoldOwner.Replace("'", "''") + "')";
                    query = query + "and (OwnerName = '" + SiteOwnerName.Replace("'", "''") + "' OR 'All' = '" + SiteOwnerName.Replace("'", "''") + "') and(ScaffoldOwner = '" + ScaffoldOwner.Replace("'", "''") + "' OR 'All' = '" + ScaffoldOwner.Replace("'", "''") + "') and (tblRegister.SiteLocID = '" + SiteName.Replace("'", "''") + "' OR 'All' = '" + SiteName.Replace("'", "''") + "') and (LocationId = '" + LocationName.Replace("'", "''") + "' OR 'All' = '" + LocationName.Replace("'", "''") + "')";
                }
                dt = databaseCon.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    RegId = dt.Rows[0]["registerid"].ToString();
                    ScafRef = Convert.ToInt32(dt.Rows[0]["ScaffRef"]);
                    // Response.Redirect("Register.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&Datetagged=" + Request.QueryString["Datetagged"] + "&RID=" + databaseCon.Encrypt(RegId), false);
                    Result = "Success";
                    // Message = "/Register/Register?UID='" + UID + " '&UserRole=' " + UserRole + " '&id=' " + id + " '&RID=' " + databaseCon.Encrypt(RegId) + "";
                    Message = databaseCon.Encrypt(RegId);
                }

                else
                {
                    //QRCodeGenerate();
                    //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong> This is the last job. Please click Previous Button.";
                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp", "<script>hideAlert();</script>", false);
                    Result = "Failed";
                    Message = "This is the last job. Please click Previous Button.";
                }
            }
            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult btnSiteFloorLevelChangeYes_Click(string RID)
        {
            string query = "update tblregister set XPosition=null,YPosition=null where registerID='" + databaseCon.Decrypt(RID) + "'";
            int i = databaseCon.RunCmd(query);
            return Json(new { Result = "Success", Message = "Update Success fully." }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult btnSiteFloorLevelChangeNo_Click(string RID)
        {
            var FloorLevelID = "";
            string query1 = "select FloorLevelId from tblregister where RegisterId='" + databaseCon.Decrypt(RID) + "'";
            DataTable dt = databaseCon.GetData(query1);
            if (dt.Rows.Count > 0)
            {
                FloorLevelID = dt.Rows[0]["FloorLevelId"].ToString();
            }
            return Json(FloorLevelID, JsonRequestBehavior.AllowGet);
        }
        public ActionResult btndeleteyes_Click(string id, string RID, string ImageName)
        {
            string query;
            var Result = "";
            var Message = "";
            string filePath = Server.MapPath("/images/sites/" + ImageName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);

            }
            //query = "update tblRegister set Image='',PhotoAngle='0' where registerid='" + databaseCon.Decrypt(RID) + "'";
            query = "update tblRegister set Image='',OriginalBaseImage ='',PhotoAngle='0' where registerid='" + databaseCon.Decrypt(RID) + "'";
            int result = databaseCon.RunCmd(query);
            if (result > 0)
            {
                //ShowregisterDetails();
                //Session["DeleteImagemsg"] = "DeleteImagemsg";
                //Response.Redirect("Register.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RID=" + Request.QueryString["RID"], false);

                Result = "Success";
                Message = "Image Deleted Successfully.";
            }
            return Json(new { Result = "Success", Message = "Image Deleted Successfully." });
            // divdelete.Style.Add("display", "none");
        }

        //Added on 06-12-2023 We are now creating Drop Dow of Shift Allocation Dynamically.
        public ActionResult ShiftAllocationDropdownBind(string id)
        {
            var GetData = dbcontext.Database.SqlQuery<ShiftAllocationDropdownBindViewModel>("Select distinct isnull (ShiftAllocation,'') as ShiftAllocation from tblShiftAllocation where Clientid = '" + id + "' and Status != 'InActive'");
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }
        private static byte[] GetCompressedBitmap(Bitmap bmp, long quality)
        {
            byte[] image;
            using (MemoryStream output = new MemoryStream())
            {
                EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                ImageCodecInfo imageCodec = ImageCodecInfo.GetImageEncoders().FirstOrDefault(o => o.FormatID == ImageFormat.Jpeg.Guid);
                EncoderParameters parameters = new EncoderParameters(1);
                parameters.Param[0] = qualityParam;
                bmp.Save(output, imageCodec, parameters);
                image = output.ToArray();
            }
            return image;
        }

        // Tab hide show View Model

        public class TabHideShowViewModel
        {
            public Nullable<int> CycloneCount { get; set; }
            public string Plans { get; set; }
            public string CycloneManagement { get; set; }
            public string MaintainInventory { get; set; }
            public Nullable<int> userRole { get; set; }
            public string NoInventory { get; set; }

            public string PipeLineInvHide { get; set; }
        }

        // Tab hide show Code For All Job section Page

        public ActionResult TabHideShow(string ClientID, string RegisterID, string userRole)
        {
            TabHideShowViewModel tabHideShowViewModel = new TabHideShowViewModel();
            tabHideShowViewModel.userRole = Convert.ToInt32(databaseCon.Decrypt(userRole));

           // DataTable dt = databaseCon.GetData("select Count(*) as CycloneCount from tblRegister join tblSiteLocation on tblSiteLocation.SiteLocID=tblRegister.SiteLocId  where tblSiteLocation.ClientID='" + databaseCon.Decrypt(ClientID) + "' and CycloneManagement=1  and RegisterId='" + databaseCon.Decrypt(RegisterID) + "'");
            DataTable dt = databaseCon.GetData("select Count(1) as CycloneCount from tblRegister join tblSiteLocation on tblSiteLocation.SiteLocID=tblRegister.SiteLocId  where tblSiteLocation.ClientID='" + databaseCon.Decrypt(ClientID) + "' and CycloneManagement=1  and RegisterId='" + databaseCon.Decrypt(RegisterID) + "'");
            if (dt.Rows[0]["CycloneCount"].ToString() == "0")
            {
                // liCyclone.Visible = false;

                tabHideShowViewModel.CycloneCount = Convert.ToInt32(dt.Rows[0]["CycloneCount"] == null ? 0 : dt.Rows[0]["CycloneCount"]);
            }

            DataTable inventoryPlans = databaseCon.GetData("select Plans from tblclient where clientid='" + databaseCon.Decrypt(ClientID) + "'");
            if (inventoryPlans.Rows.Count > 0)
            {
                tabHideShowViewModel.Plans = inventoryPlans.Rows[0]["Plans"].ToString();
                //string query = "select MaintainInventory,SiteLocId from tblRegister where RegisterId='" + databaseCon.Decrypt(RegisterID) + "'";
                //added by pooja on 11 jan25 as maintaininventory is takenup from tblsitelocation not tblregister
                string query = "select tblsitelocation.MaintainInventory,tblsitelocation.SiteLocId from tblRegister join tblsitelocation on tblRegister.sitelocid=tblsitelocation.sitelocid where  RegisterId='" + databaseCon.Decrypt(RegisterID) + "'";
                DataTable dt1 = databaseCon.GetData(query);
                if (dt1.Rows.Count > 0)
                {
                    var siteLocationID = dt1.Rows[0]["SiteLocId"].ToString();
                    string Sitequery = "select CycloneManagement from tblSiteLocation where SiteLocID='" + siteLocationID + "'";
                    DataTable Sitedt = databaseCon.GetData(Sitequery);
                    string NoInventoryquery = "select isnull(NoInventory,0) as NoInventory from tblRegister where SiteLocID='" + siteLocationID + "'and RegisterId='" + databaseCon.Decrypt(RegisterID) + "'";
                    DataTable NoInventorydt = databaseCon.GetData(NoInventoryquery);
                    string NoInventoryquery13 = "select isnull(PipeLineInvHide,0) as PipeLineInvHide from tblSiteLocation where SiteLocID='" + siteLocationID + "'";
                    DataTable NoInventorydt12 = databaseCon.GetData(NoInventoryquery13);


                    tabHideShowViewModel.MaintainInventory = dt1.Rows[0]["MaintainInventory"].ToString();
                    tabHideShowViewModel.CycloneManagement = Sitedt.Rows[0]["CycloneManagement"].ToString();
                    tabHideShowViewModel.NoInventory = NoInventorydt.Rows[0]["NoInventory"].ToString();
                    tabHideShowViewModel.PipeLineInvHide = NoInventorydt12.Rows[0]["PipeLineInvHide"].ToString();


                }

            }

            return Json(tabHideShowViewModel, JsonRequestBehavior.AllowGet);
        }

        //Added on 06-12-2023 We are now creating Drop Dow of Shift Allocation Dynamically.
        public class ShiftAllocationDropdownBindViewModel
        {
            public string ShiftAllocation { get; set; }
        }

        public ActionResult btnCopyHistorical(string id, string UID, string UserRole, string OldFloorid, string OldSiteId, string OldLocId, string OldRegisterid, string CheckInvnt, string NewRegistId, string NewLocationId, string NewFloorLevel, string NewSiteLocId, int SelectLiveJobOrPipelineJob, string startdate)
        {
            var Result = "";
            var Message = "";
            //string NewFileName;
            string txtSiteName = "";
            var NewRegId = "";

            string query1 = "exec CopyHistoricalJob '" + databaseCon.Decrypt(id) + "','" + OldSiteId + "','" + databaseCon.Decrypt(UID) + "','" + OldRegisterid + "','" + OldLocId + "','" + OldFloorid + "','" + CheckInvnt + "','" + databaseCon.Decrypt(NewRegistId) + "','" + NewSiteLocId + "','" + NewLocationId + "','" + NewFloorLevel + "' ," + SelectLiveJobOrPipelineJob + ",'" + startdate + "'";
            dbcontext.Database.CommandTimeout = 15 * 60;
            DataTable dt1 = databaseCon.GetData(query1);

            if (dt1.Rows.Count != 0)
            {
                if (Convert.ToInt32(dt1.Rows[0][0]) == -1)
                {
                    Result = "Failed";
                    Message = dt1.Rows[0][1].ToString().Replace("|", "<br>");
                }
                else if (Convert.ToInt32(dt1.Rows[0][0]) == -2)
                {
                    Result = "Insufficient";
                    Message = dt1.Rows[0][1].ToString().Replace("|", "<br>");

                }
                else if (Convert.ToInt32(dt1.Rows[0][0]) != -1 && Convert.ToInt32(dt1.Rows[0][0]) != 0)
                {
                    if (dt1.Rows[0][0].ToString() != "" || dt1.Rows[0][0].ToString() != null)
                    {
                        NewRegId = dt1.Rows[0][0].ToString();
                    }
                    else
                    {
                        NewRegId = NewRegistId;
                    }
                    string DocQuery = "select DocumentId, documentType, DocumentId,tblDocuments.ClientID,DocumentName,Description,DateOfUpload,tblDocuments.DocumentTypeId,RegisterID,OriginalDocumentName from tbldocuments join tblDocumentType on tblDocuments.DocumentTypeId = tblDocumentType.DocumentTypeId  where RegisterID = " + OldRegisterid + " and tblDocuments.clientid = " + databaseCon.Decrypt(id) + " order by dateofupload desc";
                    string GalleryQyery = "select ID ,RegisterId,Description ,FileName as DocumentName,Type,OriginalDocumentName from tblRegisterGallery where RegisterId =  " + OldRegisterid + "";

                    var GetData = dbcontext.Database.SqlQuery<GetDocumentTypeData>(DocQuery).ToList();
                    var GetGalleryData = dbcontext.Database.SqlQuery<DrawingPDFViewModel>(GalleryQyery).ToList();

                    foreach (var item in GetData)
                    {
                        int documentid = item.DocumentId;
                        string txtdesc = item.Description;
                        var sourceFilename = item.DocumentName;
                        string extension = System.IO.Path.GetExtension(sourceFilename).ToLower();
                        string NewFileName = databaseCon.Decrypt(id) + "_" + "Job_" + NewRegId + "_" + DateTime.Now.Ticks + extension;

                        var nreFile = databaseCon.CopyAndRenameFileOnBlob(sourceFilename, NewFileName, "Inspection-Document");

                        string UploaddocumentQuery = "insert into tblDocuments(RegisterID,DocumentName,Description,DateOfUpload,DocumentTypeId,ClientID,CreatedBy,CreatedOn,OriginalDocumentName,SiteLocID) Select " + NewRegId + ",'" + NewFileName + "',Description,DateOfUpload,DocumentTypeId,ClientID,'Admin@gmail.com',GETDATE(),OriginalDocumentName,SiteLocID from tblDocuments where RegisterID = " + OldRegisterid + " and DocumentId = " + documentid + " ";
                        databaseCon.RunCmd(UploaddocumentQuery);


                    }
                    foreach (var items in GetGalleryData)
                    {
                        int Galleryid = items.ID;
                        string txtdesc = items.Description;
                        var sourceFilename = items.DocumentName;
                        string extension = System.IO.Path.GetExtension(sourceFilename).ToLower();

                        //Added on (21-03-2024) we are having this naming convention for Image Gallery and Document Tab. Becuase we get special character in site name so we are not using it anymore now thus passed a blank string.
                        string NewFileName = txtSiteName.Replace("'", "''").Replace(" ", "").Trim() + "_" + NewRegId + "_" + DateTime.Now.Ticks + extension;

                        var nreFile = databaseCon.CopyAndRenameFileOnBlob(sourceFilename, NewFileName, "Site-images");

                        string UploaddatraQuery = "INSERT INTO tblRegisterGallery(RegisterId, Description, FileName, CreatedBy, CreatedOn, Type, OriginalDocumentName) SELECT " + NewRegId + ", Description, '" + NewFileName + "', 'Admin@gmail.com', GETDATE(), Type, OriginalDocumentName FROM tblRegisterGallery WHERE RegisterId = " + OldRegisterid + " and ID = " + Galleryid + "";
                        databaseCon.RunCmd(UploaddatraQuery);


                    }

                    Result = "Success";
                    Message = dt1.Rows[0][1].ToString();
                }
            }
            else
            {
                Result = "Failed";
                Message = "Historical Copy Process Failed. Please Contact Admin";
            }
            return Json(new { Result = Result, Message = Message, NewRegId = NewRegId }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadImage(string registerId)
        {
            string decryptedId = databaseCon.Decrypt(registerId);
            string ImageName = "";
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);



            string query = "SELECT image FROM tblregister WHERE registerid = '" + decryptedId + "'";
            DataTable imagename = databaseCon.GetData(query);
            if (imagename.Rows.Count > 0)
            {

                ImageName = imagename.Rows[0]["image"].ToString();
            }
            var FilePath = Server.MapPath("~/images/sites/" + ImageName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(FilePath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, ImageName);

        }

        public ActionResult CheckIsPipeLineJob(string id, string UID, string RegisterId)
        {
            //string query = "SELECT CASE WHEN EXISTS ( SELECT 1 FROM tblPipelineRequest WHERE ClientId = " + databaseCon.Decrypt(id) + " AND UserId = " + databaseCon.Decrypt(UID) + " AND JobId =" +RegisterId + ") THEN CAST(1 AS BIT)  ELSE CAST(0 AS BIT) END AS Result";
            string query = "SELECT CASE WHEN EXISTS (SELECT 1 FROM tblTransactionHistRequest WHERE FromId = " + RegisterId + ") OR EXISTS (SELECT 1 FROM tblOptimizedInventory WHERE RegisterId = " + RegisterId + ") THEN CAST(0 AS BIT)  ELSE CAST(1 AS BIT) END AS Result";

            DataTable dt1 = databaseCon.GetData(query);

            var IsPipeLineJob = dt1.Rows[0][0];
            return Json(new { Result = IsPipeLineJob });
        }


        public ActionResult GetVariationHours(string RegisterId)
        {
            string registerId = databaseCon.Decrypt(RegisterId);
            string sqlQuery = @"
       SELECT 
            WorkCarriedOutVariation, 
            RegisterId, 
            WorkCarriedOut, 
            VariationInHrs, 
            coalesce(updatedby,createdby) as createdby,
            coalesce(updatedOn,createdOn) as createdOn,
            UpdatedBy, 
            UpdatedOn,
            WorkCarriedOutDate
          FROM TblWorkCarriedOutVariation
    WHERE RegisterId = @registerId
    ORDER BY WorkCarriedOutDate  DESC";

            var parameters = new SqlParameter("@RegisterId", registerId);
            var getData = dbcontext.Database.SqlQuery<TblWorkCarriedOutVariation>(sqlQuery, parameters).ToList();
            return Json(getData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateVariationHours(int WorkCarriedOutVariationId, string WorkCarriedOut, decimal VariationInHrs, string UID, DateTime WorkCarriedOutDate)
        {
            string userId = databaseCon.Decrypt(UID);
            try
            {
                string emailQuery = @"
           SELECT TOP 1 EmailID 
            FROM tblUser 
            WHERE  UserId = @userId";
                var email = dbcontext.Database.SqlQuery<string>(emailQuery,
                            new SqlParameter("@UserId", userId)).FirstOrDefault();
                if (string.IsNullOrEmpty(email))
                {
                    return Json(new { success = false, message = "User email not found." });
                }
                string sqlQuery = @"
                UPDATE TblWorkCarriedOutVariation
                SET WorkCarriedOut = @WorkCarriedOut,
                    VariationInHrs = @VariationInHrs,
                WorkCarriedOutDate = @WorkCarriedOutDate,
                    UpdatedOn = GetDate(),
                     UpdatedBy = @UpdatedBy
                WHERE WorkCarriedOutVariation = @WorkCarriedOutVariationId";

                var parameters = new SqlParameter[]
                {
                new SqlParameter("@WorkCarriedOutVariationId", WorkCarriedOutVariationId),
                new SqlParameter("@WorkCarriedOut", WorkCarriedOut),
                new SqlParameter("@VariationInHrs", VariationInHrs),
            new SqlParameter("@WorkCarriedOutDate", WorkCarriedOutDate),
                new SqlParameter("@UpdatedOn", DateTime.Now),
                new SqlParameter("@UpdatedBy", email)
                };

                int rowsAffected = dbcontext.Database.ExecuteSqlCommand(sqlQuery, parameters);

                if (rowsAffected > 0)
                {
                    return Json(new { success = true, message = "Data updated successfully" });
                }
                else
                {
                    return Json(new { success = false, message = "Record not found or no changes made" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        public JsonResult DeleteVariationHours(int WorkCarriedOutVariationId)
        {
            try
            {
                string sqlQuery = @"
                DELETE FROM TblWorkCarriedOutVariation
                WHERE WorkCarriedOutVariation = @WorkCarriedOutVariationId";

                var parameter = new SqlParameter("@WorkCarriedOutVariationId", WorkCarriedOutVariationId);

                int rowsAffected = dbcontext.Database.ExecuteSqlCommand(sqlQuery, parameter);

                if (rowsAffected > 0)
                {
                    return Json(new { success = true, message = "Data deleted successfully" });
                }
                else
                {
                    return Json(new { success = false, message = "Record not found" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        public ActionResult AddVariationHours(string WorkCarriedOut, decimal VariationInHrs, string RegisterId, string id, string UID, string WorkCarriedOutDate)
        {
            string registerId = databaseCon.Decrypt(RegisterId);
            string userId = databaseCon.Decrypt(UID);

            try
            {
                string emailQuery = @"
           SELECT TOP 1 EmailID 
           FROM tblUser 
           WHERE UserId = @userId";
                var email = dbcontext.Database.SqlQuery<string>(emailQuery,
                            new SqlParameter("@UserId", userId)).FirstOrDefault();
                if (string.IsNullOrEmpty(email))
                {
                    return Json(new { success = false, message = "User email not found." });
                }

                string sqlQuery = @"
            INSERT INTO TblWorkCarriedOutVariation (WorkCarriedOut, VariationInHrs, RegisterId, CreatedOn, CreatedBy, WorkCarriedOutDate)
            OUTPUT INSERTED.WorkCarriedOutVariation, INSERTED.WorkCarriedOut, INSERTED.VariationInHrs, INSERTED.RegisterId, INSERTED.CreatedOn, INSERTED.CreatedBy, INSERTED.WorkCarriedOutDate
            VALUES (@WorkCarriedOut, @VariationInHrs, @RegisterId, GETDATE(), @CreatedBy, @WorkCarriedOutDate);";

                var result = dbcontext.Database.SqlQuery<TblWorkCarriedOutVariation>(sqlQuery,
                    new SqlParameter("@WorkCarriedOut", WorkCarriedOut),
                    new SqlParameter("@VariationInHrs", VariationInHrs),
                    new SqlParameter("@RegisterId", registerId),
                    new SqlParameter("@CreatedBy", email),
                    new SqlParameter("@WorkCarriedOutDate", WorkCarriedOutDate)).FirstOrDefault();

                return Json(new
                {
                    success = result != null,
                    id = result?.WorkCarriedOutVariation,
                    workCarriedOut = result?.WorkCarriedOut,
                    variationInHrs = result?.VariationInHrs,
                    registerId = result?.RegisterId,
                    createdOn = result?.CreatedOn,
                    createdBy = result?.CreatedBy,
                    workCarriedOutDate = result?.WorkCarriedOutDate
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        public ActionResult btnofflineconfirmYes_Click(string id, string UID, string UserRole, string Siteid, string RegisterId)
        {
            string query;
            var Result = "";
            var Message = "";
            string Link = "";
            try

            {
                // DataTable GetDt = databaseCon.GetData("select * from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'");
                DataTable GetDt = databaseCon.GetData("select EmailID from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'");

                string qry = "select RegisterID from tblRegister where IsOfflineCurrent=1 and  ClientId='" + databaseCon.Decrypt(id) + "'  and RegisterID='" + RegisterId + "'";
                DataTable dt = databaseCon.GetData(qry);
                if (dt.Rows.Count < 1)
                {
                    query = "update tblRegister set IsOfflineCurrent=1 ,GoOfflineJobBy = '" + GetDt.Rows[0]["EmailID"].ToString() + "',GoOfflineJobOn = GETDATE()  where ClientId='" + databaseCon.Decrypt(id) + "'  and RegisterID='" + databaseCon.Decrypt(RegisterId) + "'";
                    int Registerdt = databaseCon.RunCmd(query);
                    if (Registerdt != 0)
                    {
                        Result = "Failed";
                        Message = "";
                    }
                    else
                    {

                        Result = "Failed";
                        Message = "Syntax Error. Contact administrator.";
                    }
                }
                if (GetDt.Rows.Count > 0)
                {

                    string domainName = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);


                    Link = domainName + "/OfflineRegisterList.html?UID=" + UID + "&UserRole=" + UserRole + "&id=" + id + "&Siteid=" + databaseCon.Encrypt(Siteid) + "&RID=" + RegisterId;
                    Console.WriteLine(Link);
                    string strText = "Here is the <a href='" + Link + "'>Link</a> to access Offline Register.";

                    Result = "Success";

                    Message = "Please stay online. The link is now opening in a new tab.";

                }

            }
            catch (Exception ex)
            {
                Result = "Failed";
                Message = ex.Message;
            }
            return Json(new { Result = Result, Message = Message, Link = Link });
        }

        [HttpPost]
        public ActionResult UpdateOfflineInspectionFlag(string LoginUserId, string clientId, string Siteid, string RID)
        {
            string CheckIsDatasillInOfflineMode = "select  RegisterId from tblRegister where ClientID =  " + databaseCon.Decrypt(clientId) + " and SiteLocId = " + databaseCon.Decrypt(Siteid) + " and IsOfflineCurrent = 1 and registerid = '" + databaseCon.Decrypt(RID) + "'";
            DataTable CheckIsDatasillInOfflineModeData = databaseCon.GetData(CheckIsDatasillInOfflineMode);

            if (CheckIsDatasillInOfflineModeData.Rows.Count > 0)
            {

                try
                {
                    string Userinfor = "select LoginId from tblUser where UserId = " + databaseCon.Decrypt(LoginUserId) + "";

                    DataTable LoginuserInfo = databaseCon.GetData(Userinfor);
                    string updateRegisterquery = "update tblRegister set IsOfflineCurrent = 0 ,OfflineJobCleared = 1, OfflineJobClearedOn = GETDATE(),OfflineJobClearedBy = '" + LoginuserInfo.Rows[0][0].ToString() + "' where ClientID =  " + databaseCon.Decrypt(clientId) + " and SiteLocId = " + databaseCon.Decrypt(Siteid) + " and IsOfflineCurrent = 1 and registerid = '" + databaseCon.Decrypt(RID) + "'";

                    var res = databaseCon.RunCmd(updateRegisterquery);

                    string cleared = "insert into tblTrackOfflineJob (RegisterId,ClientId, SiteId, Action, OfflineJobClearedOn, OfflineJobClearedBy) values('" + databaseCon.Decrypt(RID) + "'," + databaseCon.Decrypt(clientId) + "," + databaseCon.Decrypt(Siteid) + ", 'Cleared', GetDate(),'" + LoginuserInfo.Rows[0][0].ToString() + "')";
                    var clearedEntry = databaseCon.GetData(cleared);

                    return Json(new { Result = "Success", Message = "The Job Modified offline (If any) will no more be allowed to be loaded. You can now work online." });
                }
                catch (Exception ex)
                {
                    return Json(new { Result = "Failed", Message = ex.Message });
                }
            }
            else
            {

                return Json(new { Result = "Failed", Message = "This job is not available in offline mode." });


            }
        }

        public ActionResult CheckuserUserisOfflineMode(string id, string UID, string Siteid, string RegisterID)
        {
            string CheckCompletedJob = "select  'x'  cnt  from vw_tblregister  where Status='Completed' and RegisterID= '" + databaseCon.Decrypt(RegisterID) + "'";

            var CompletedJob = databaseCon.GetData(CheckCompletedJob);

            string CheckuserIsOfflineForInspection = "select  top 1 ScaffRef, userid,concat(firstname,' ',Lastname)UserName,GoOfflineBy,GoOfflineOn from tbluser  join tblregister on GoOfflineBy = EmailID where IsOfflineCurrentForInspection =1 and userid= '" + databaseCon.Decrypt(UID) + "'";

            var ValueForCheckuserIsOfflineForInspection = databaseCon.GetData(CheckuserIsOfflineForInspection);


            string CheckJobInventoryHasData = "select   'x' cnt  from tblTransactionHistDelivery  where  ToId= '" + databaseCon.Decrypt(RegisterID) + "' ";



            //added on 4feb2025 , as admin has a single entry in tbluser, multiple clients start getting alerts
            //string UserIsInspector = "select  'x' cnt from tbluser where IsInspector=1 and status='Active' and ((clientid='" + databaseCon.Decrypt(id) + "' and userid= '" + databaseCon.Decrypt(UID) + "') or role='SuperAdmin')";
            //added on 7feb2025
            string UserIsInspector = "select  'x' cnt from tbluser where IsInspector=1 and status='Active' and userid= '" + databaseCon.Decrypt(UID) + "' and (clientid='" + databaseCon.Decrypt(id) + "' or role='SuperAdmin')";

            var UserIsInspectorcount = databaseCon.GetData(UserIsInspector);

            var Inventorycount = databaseCon.GetData(CheckJobInventoryHasData);

            string UserScaffoldInspectionList = "select distinct  checklistid from tblRegister where clientid='" + databaseCon.Decrypt(id) + "' and registerid='" + databaseCon.Decrypt(RegisterID) + "' and  (checklistid is null or checklistid='')";


            var ScaffoldInspectionList = databaseCon.GetData(UserScaffoldInspectionList);

            if (CompletedJob.Rows.Count == 0)
            {
                if (ValueForCheckuserIsOfflineForInspection.Rows.Count == 0)
                {
                    if (Inventorycount.Rows.Count == 0)
                    {
                        if (UserIsInspectorcount.Rows.Count > 0)
                        {
                            if (ScaffoldInspectionList.Rows.Count == 0)
                            {
                                //string UserVerifyedQuery = "SELECT CASE WHEN EXISTS (SELECT 1 FROM tblregister r WHERE r.IsOfflineCurrent = 1 AND r.clientid = '" + databaseCon.Decrypt(id) + "' AND r.SiteLocId = '" + Siteid + "' AND r.GoOfflineJobBy = (SELECT emailid FROM tbluser WHERE userid = '" + databaseCon.Decrypt(UID) + "')) OR EXISTS (SELECT 1 FROM tblregister r WHERE r.IsOfflineCurrent = 1 AND r.clientid = '" + databaseCon.Decrypt(id) + "' AND r.SiteLocId = '" + Siteid + "' AND r.GoOfflineJobBy != (SELECT emailid FROM tbluser WHERE userid = '" + databaseCon.Decrypt(UID) + "')AND r.registerid in ('"+ databaseCon.Decrypt(RegisterID)+ "')) THEN 0 ELSE 1 END AS CanGoOffline";
                                // update on  3 jan  2025 by ss 
                                //string UserVerifyedQuery = "SELECT CASE WHEN EXISTS (SELECT 1 FROM tblregister r WHERE r.IsOfflineCurrent = 1 AND r.clientid = '" + databaseCon.Decrypt(id) + "' AND r.SiteLocId = '" + Siteid + "' AND r.GoOfflineJobBy = (SELECT emailid FROM tbluser WHERE userid = '" + databaseCon.Decrypt(UID) + "')) OR EXISTS (SELECT 1 FROM tblregister r WHERE r.IsOfflineCurrent = 1 AND r.clientid = '" + databaseCon.Decrypt(id) + "' AND r.SiteLocId = '" + Siteid + "' AND r.GoOfflineJobBy != (SELECT emailid FROM tbluser WHERE userid = '" + databaseCon.Decrypt(UID) + "')AND r.registerid in ('" + databaseCon.Decrypt(RegisterID)+ "')) THEN 0  WHEN EXISTS (SELECT 1 FROM tblregister r WHERE r.IsOfflineCurrent = 1 AND r.clientid = '" + databaseCon.Decrypt(id) + "' AND r.GoOfflineJobBy = (SELECT emailid FROM tbluser WHERE userid = '" + databaseCon.Decrypt(UID) + "'))  then 0 ELSE 1 END AS CanGoOffline";
                                // updated  on  04 jan 2024 by ss

                                //string UserVerifyedQuery = "SELECT case when '" + databaseCon.Decrypt(RegisterID) + "'=(SELECT RegisterId FROM tblregister r WHERE r.IsOfflineCurrent = 1 AND r.clientid ='" + databaseCon.Decrypt(id) + "' AND r.SiteLocId ='" + Siteid + "' AND r.GoOfflineJobBy = (SELECT emailid FROM tbluser WHERE userid = '" + databaseCon.Decrypt(UID) + "')) then 1 WHEN EXISTS (SELECT 1 FROM tblregister r WHERE r.IsOfflineCurrent = 1 AND r.clientid ='" + databaseCon.Decrypt(id) + "' AND r.SiteLocId = '" + Siteid + "' AND r.GoOfflineJobBy = (SELECT emailid FROM tbluser WHERE userid = '" + databaseCon.Decrypt(UID) + "')) OR EXISTS (SELECT 1 FROM tblregister r WHERE r.IsOfflineCurrent = 1 AND r.clientid ='" + databaseCon.Decrypt(id) + "' AND r.SiteLocId ='" + Siteid + "' AND r.GoOfflineJobBy != (SELECT emailid FROM tbluser WHERE userid = '" + databaseCon.Decrypt(UID) + "')AND r.registerid in ('" + databaseCon.Decrypt(RegisterID) + "')) THEN 0  WHEN EXISTS (SELECT 1 FROM tblregister r WHERE r.IsOfflineCurrent = 1 AND r.clientid ='" + databaseCon.Decrypt(id) + "' AND r.GoOfflineJobBy = (SELECT emailid FROM tbluser WHERE userid = '" + databaseCon.Decrypt(UID) + "'))  then 0 ELSE 1 END AS CanGoOffline ";
                                string UserVerifyedQuery = "exec Pro_CheckUserIsValidForOfflineRegister   '" + databaseCon.Decrypt(RegisterID) + "','" + databaseCon.Decrypt(UID) + "'";


                                var isuer = databaseCon.GetData(UserVerifyedQuery);
                                //string finduser = "select  ScaffRef, userid,concat(firstname,' ',Lastname)UserName,GoOfflineJobBy,GoOfflineJobOn from tbluser  join tblregister on GoOfflineJobBy = EmailID where IsOfflineCurrent =1 and userid= '" + databaseCon.Decrypt(UID) + "'";

                                //string finduser = "select  top 1 ScaffRef, userid,concat(firstname,' ',Lastname)UserName,GoOfflineJobBy,GoOfflineJobOn from tbluser  join tblregister on GoOfflineJobBy = EmailID where IsOfflineCurrent =1 and userid= '" + databaseCon.Decrypt(UID) + "'";

                                //string finduser = "select  top 1 ScaffRef, userid,concat(firstname,' ',Lastname)UserName,GoOfflineJobBy,GoOfflineJobOn from tbluser  join tblregister on GoOfflineJobBy = EmailID where IsOfflineCurrent =1 ";

                                // updated by ss on 13 jan 
                                //string finduser = "select  top 1 ScaffRef, userid,concat(firstname,' ',Lastname)UserName,GoOfflineJobBy,GoOfflineJobOn,SiteLocation from tbluser  join tblregister on GoOfflineJobBy = EmailID join tblSiteLocation on tblSiteLocation.SiteLocID=tblregister.SiteLocId where IsOfflineCurrent =1";
                                string finduser = "select top 1 Right('000000'+Convert(varchar,ScaffRef),6) as ScaffRef, userid, UserName, GoOfflineJobBy, GoOfflineJobOn, SiteLocation, displayorder, msg from (select ScaffRef, userid, concat(firstname,' ',Lastname) UserName, GoOfflineJobBy, GoOfflineJobOn, SiteLocation, 0 displayorder, 'You are already offline with another Scaf Ref # ' + Right('000000'+Convert(varchar,ScaffRef),6) as msg from tbluser join tblregister on GoOfflineJobBy = EmailID join tblSiteLocation on tblSiteLocation.SiteLocID=tblregister.SiteLocId where IsOfflineCurrent =1 and userid = '" + databaseCon.Decrypt(UID) + "' and registerid != '" + databaseCon.Decrypt(RegisterID) + "' UNION select ScaffRef, userid, concat(firstname,' ',Lastname) UserName, GoOfflineJobBy, GoOfflineJobOn, SiteLocation, 1 displayorder, 'Another User '+ concat(firstname,' ',Lastname)+' is already offline with this Job' as msg from tbluser join tblregister on GoOfflineJobBy = EmailID join tblSiteLocation on tblSiteLocation.SiteLocID=tblregister.SiteLocId where IsOfflineCurrent =1 and userid != '" + databaseCon.Decrypt(UID) + "' and registerid = '" + databaseCon.Decrypt(RegisterID) + "') as CheckOfflineJob order by displayorder, scaffref";




                                var FinddataUpdatedBy = databaseCon.GetData(finduser);
                                var userId = "";
                                var userName = "";
                                var offlineBy = "";
                                var scacffref = "";
                                var Message = "";
                                if (FinddataUpdatedBy.Rows.Count > 0)
                                {
                                    //userId = FinddataUpdatedBy.Rows[0]["userid"].ToString();
                                    //userName = FinddataUpdatedBy.Rows[0]["UserName"].ToString();
                                    //offlineBy = FinddataUpdatedBy.Rows[0]["GoOfflineJobBy"].ToString();
                                    //scacffref = FinddataUpdatedBy.Rows[0]["ScaffRef"].ToString();
                                    // SiteLocation = FinddataUpdatedBy.Rows[0]["SiteLocation"].ToString();
                                    Message = FinddataUpdatedBy.Rows[0]["msg"].ToString();
                                }
                                //return Json(new { VerifyedUser = isuer.Rows[0][0], UserName = userName, scacffref = scacffref, SiteLocation = SiteLocation }, JsonRequestBehavior.AllowGet);
                                return Json(new { VerifyedUser = isuer.Rows[0][0], Message = Message }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { ScaffoldInspectionList = true, VerifyedUser = 0, UserName = false, scacffref = false });
                            }
                        }
                        else
                        {
                            return Json(new { UserIsInspector = true, VerifyedUser = 0, UserName = false, scacffref = false });
                        }
                    }
                    else
                    {
                        return Json(new { JobHasInventory = true, VerifyedUser = 0, UserName = false, scacffref = false });
                    }
                }
                else
                {
                    return Json(new { UserIsOnOfflineInspectionAlso = true, VerifyedUser = 0, UserName = false, scacffref = false });
                }


            }
            else
            {
                //Status = "Failed";
                //Msg = "User is not allowed to take Completed Jobs Offline.";
                return Json(new { JobIsCompleted = true, VerifyedUser = 0, UserName = false, scacffref = false });
            }
        }

        public JsonResult GetUserEmailByUID(string UID)
        {
            try
            {
                string query = "SELECT FirstName, LastName FROM tblUser WHERE UserId = " + databaseCon.Decrypt(UID);

                var dt = databaseCon.GetData(query);

                string fullName = "";

                if (dt != null && dt.Rows.Count > 0)
                {
                    string firstName = dt.Rows[0]["FirstName"].ToString();
                    string lastName = dt.Rows[0]["LastName"].ToString();

                    fullName = firstName + " " + lastName;
                }

                return Json(new { name = fullName }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { name = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CloseOutJob(string id, string RID, string UID, string DismantleDate, string hdnTimeOffSet)
        {
            var Result = "";
            var Message = "";
            try
            {
                string clientId = databaseCon.Decrypt(id);
                string registerId = databaseCon.Decrypt(RID);
                string userId = databaseCon.Decrypt(UID);
                var hdntimeoffset = Convert.ToDouble(hdnTimeOffSet) * (-1);
                var CurrentDateWithOffSet = Convert.ToDateTime(DateTime.Now).AddMinutes(hdntimeoffset);
                DateTime parsedDismantleDate;
                if (!DateTime.TryParse(DismantleDate, out parsedDismantleDate))
                {
                    return Json(new { Result = "Failed", Message = "Invalid Dismantle Date." }, JsonRequestBehavior.AllowGet);
                }
                if (parsedDismantleDate.Date > CurrentDateWithOffSet)
                {
                    return Json(new { Result = "Failed", Message = "Future date is not allowed for Dismantle Date." }, JsonRequestBehavior.AllowGet);
                }
                string dismantledBy = "";
                string userQuery = "SELECT FirstName, LastName FROM tblUser WHERE UserId = " + userId;
                DataTable dtUser = databaseCon.GetData(userQuery);
                if (dtUser.Rows.Count > 0)
                {
                    dismantledBy = dtUser.Rows[0]["FirstName"].ToString() + " " + dtUser.Rows[0]["LastName"].ToString();
                }
                string closeOutQuery = "EXEC Pro_CloseOutJob '" + registerId + "','" +
                            clientId + "','" +
                            parsedDismantleDate.ToString("yyyy-MM-dd") + "','" +
                            dismantledBy.Replace("'", "''") + "','" +
                            userId + "','" + CurrentDateWithOffSet.ToString("yyyy-MM-dd") + "'";
                DataTable dt = databaseCon.GetData(closeOutQuery);
                if (dt.Rows.Count > 0 && dt.Rows[0]["Result"].ToString() == "Success")
                {
                    Result = "Success";
                    Message = dt.Rows[0]["Message"].ToString();
                }
                else
                {
                    Result = "Failed";
                    Message = dt.Rows.Count > 0 ? dt.Rows[0]["Message"].ToString() : "Unable to Close Out Job.";
                }
            }
            catch (Exception ex)
            {
                Result = "Failed";
                Message = "Error: " + ex.Message;
            }
            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckJobInventoryExists(string id, string RID)
        {
            bool hasInventory = false;
            try
            {
                string clientId = databaseCon.Decrypt(id);
                string registerId = databaseCon.Decrypt(RID);
                if (string.IsNullOrEmpty(registerId) || registerId == "0")
                {
                    return Json(new { HasInventory = false }, JsonRequestBehavior.AllowGet);
                }

                string query = "SELECT CASE WHEN EXISTS (SELECT 1 FROM tblTransaction WHERE ClientId='" + clientId + "' AND [From]='Job' AND FromId='" + registerId + "') OR EXISTS ( SELECT 1 FROM tblTransactionHistRequest WHERE [From]='Job' AND [Fromid]='" + registerId + "' AND RequestStatus='Confirm') THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS HasInventory";

                DataTable dt = databaseCon.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    hasInventory = Convert.ToBoolean(dt.Rows[0]["HasInventory"]);
                }
            }
            catch (Exception)
            {
                hasInventory = false;
            }

            return Json(new { HasInventory = hasInventory }, JsonRequestBehavior.AllowGet);
        }
        public class TblWorkCarriedOutVariation
        {
            public int WorkCarriedOutVariation { get; set; }
            public int RegisterId { get; set; }
            public string WorkCarriedOut { get; set; }
            public decimal VariationInHrs { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedOn { get; set; }
            public string UpdatedBy { get; set; }
            public DateTime? UpdatedOn { get; set; }
            public DateTime? WorkCarriedOutDate { get; set; }
        }

    }
}
