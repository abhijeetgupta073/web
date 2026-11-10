using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Spreadsheet;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using System.Windows.Controls.Primitives;
using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;
using static Scaffold.Controllers.OfflineOffSystemInspectionController;
using DocumentFormat.OpenXml.Vml.Office;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Ajax.Utilities;
using System.Security.Policy;

namespace Scaffold.Controllers
{
    public class RegisterListController : Controller
    {

        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: RegisterList
        public ActionResult RegisterList()
        {
            string query = "";
            DataTable dt;

           // query = "select * from tblPageDropdownFilter  where ClientID='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and UserId='" + databaseCon.Decrypt(Request.QueryString["UID"]) + "'";
            query = "select 'X' as DummyColumn from tblPageDropdownFilter  where ClientID='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and UserId='" + databaseCon.Decrypt(Request.QueryString["UID"]) + "'";
            dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                ViewBag.SiteID = dt.Rows[0]["SiteID"].ToString();
                ViewBag.LocationID = dt.Rows[0]["LocationID"].ToString();
                ViewBag.Disciplines = dt.Rows[0]["Disciplines"].ToString();
                ViewBag.WorkGroup = dt.Rows[0]["WorkGroup"].ToString();
                ViewBag.SubGroup = dt.Rows[0]["SubGroup"].ToString();
                ViewBag.BuildingName = dt.Rows[0]["BuildingName"].ToString();
            }
            query = "select UserId from tblUser where IsInspector='true' and userid= '" + databaseCon.Decrypt(Request.QueryString["UID"]) + "' and clientid='" + databaseCon.Decrypt(Request.QueryString["id"]) + "'";
            dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["UserId"].ToString() != "0")
                {
                    ViewBag.UserId = dt.Rows[0]["UserId"].ToString();
                }
            }
            query = "select isnull(OfflineEnabled,0) as OfflineEnabled from tblClient where clientid='" + (databaseCon.Decrypt(Request.QueryString["id"])) + "'";
            dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["OfflineEnabled"].ToString() == "True")
                {
                    ViewBag.OfflineEnabled = true;
                    //divOffline.Visible = false;
                    //divOfflineInspection.Visible = false;
                    //divOfflineUploadData.Visible = false;
                }
            }
            query = "select isnull(OfflineInspEnable,0) as OfflineInspEnable from tblClient where clientid='" + (databaseCon.Decrypt(Request.QueryString["id"])) + "'";
            dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["OfflineInspEnable"].ToString() == "True")
                {
                    ViewBag.OfflineInspEnable = true;
                }
            }

            return View();
        }
        public ActionResult BindallDropdown(string id)
        {

            var ddlInspectedBy = dbContext.Database.SqlQuery<DropDownViewModel>("select firstName+ ' '+LastName as name,UserId,LicenceNo from tblUser where IsInspector='true' and Status='Active' and clientid='" + databaseCon.Decrypt(id) + "' Order by name");

            var ddlChecklistInsp = dbContext.Database.SqlQuery<DropDownViewModel>("select distinct tblCheckList.checklistID, checklistdescription from tblchecklist join tblcheckListContent on tblchecklist.ChecklistId = tblcheckListContent.ChecklistId and  tblCheckList.ClientId = tblchecklistcontent.clientid where tblCheckList.clientid = '" + databaseCon.Decrypt(id) + "' and[status] = 'Active' order by checklistdescription");

            return Json(new { ddlInspectedBy = ddlInspectedBy, ddlChecklistInsp = ddlChecklistInsp }, JsonRequestBehavior.AllowGet);

        }
        //public ActionResult SiteDropDown(string id,string UID)
        //{
        //    string query;
        //    DataTable dt;
        //    var SiteOwnerName = "";
        //    var ScaffoldOwner = "";
        //   // ViewState["SiteOwnerName"] = "All";
        //   // ViewState["ScaffoldOwner"] = "All";
        //    string query1 = "select case when SiteOwnerName is null or SiteOwnerName='' then 'All' Else SiteOwnerName end as SiteOwnerName, case when ScaffoldOwner is null or ScaffoldOwner='' then 'All' Else ScaffoldOwner end  ScaffoldOwner from tblUser where ClientId='" +databaseCon.Decrypt(id) + "' and UserId='" +databaseCon.Decrypt(UID) + "'";
        //    DataTable dt1 = databaseCon.GetData(query1);
        //    if (dt1.Rows.Count > 0)
        //    {

        //        SiteOwnerName = dt1.Rows[0]["SiteOwnerName"].ToString();
        //        ScaffoldOwner = dt1.Rows[0]["ScaffoldOwner"].ToString();
        //       // ViewState["SiteOwnerName"] = SiteOwnerName;
        //       // ViewState["ScaffoldOwner"] = ScaffoldOwner;
        //    }

        //    if ((SiteOwnerName != "" && SiteOwnerName != "All") || (ScaffoldOwner != "" && ScaffoldOwner != "All"))
        //    {
        //        query = "select distinct tblSiteLocation.SiteLocID, SiteLocation from tblSiteLocation join(Select distinct SiteLocId from Vw_tblRegister where (OwnerName = '" + SiteOwnerName.Replace("'", "''") + "' OR 'All' = '" + SiteOwnerName.Replace("'", "''") + "') and(ScaffoldOwner = '" + ScaffoldOwner.Replace("'", "''") + "' OR 'All' = '" + ScaffoldOwner.Replace("'", "''") + "') and ClientId = '" +databaseCon.Decrypt(id) + "') FromRegister On tblSiteLocation.SiteLocID = FromRegister.SiteLocId where ClientId = '" +databaseCon.Decrypt(id) + "' order by SiteLocation";
        //    }
        //    else
        //    {
        //        query = "select distinct SiteLocID,SiteLocation from tblSiteLocation where SiteLocation <>'' and SiteLocation is not null and  ClientId='" + databaseCon.Decrypt(id) + "' order by SiteLocation";
        //    }

        //  var getData = dbContext.Database.SqlQuery<DropDownViewModel>(query).ToList();
        //    return Json(getData, JsonRequestBehavior.AllowGet);

        //}
        public ActionResult SiteDropDown(string id, string UID)
        {
            string query;
            DataTable dt;
            var SiteOwnerName = "";
            var ScaffoldOwner = "";
            var Site = "";

            string query1 = "select Site, case when SiteOwnerName is null or SiteOwnerName='' then 'All' Else SiteOwnerName end as SiteOwnerName, case when ScaffoldOwner is null or ScaffoldOwner='' then 'All' Else ScaffoldOwner end  ScaffoldOwner from tblUser where ClientId='" + databaseCon.Decrypt(id) + "' and UserId='" + databaseCon.Decrypt(UID) + "'";
            DataTable dt1 = databaseCon.GetData(query1);
            if (dt1.Rows.Count > 0)
            {

                SiteOwnerName = dt1.Rows[0]["SiteOwnerName"].ToString();
                ScaffoldOwner = dt1.Rows[0]["ScaffoldOwner"].ToString();
                Site = dt1.Rows[0]["Site"].ToString();

            }
            if (!Site.IsNullOrWhiteSpace())
            {
                query = "select  SiteLocID,SiteLocation from tblSiteLocation where SiteLocID = " + Site + " and ClientId='" + databaseCon.Decrypt(id) + "'";
            }
            else if ((SiteOwnerName != "" && SiteOwnerName != "All") || (ScaffoldOwner != "" && ScaffoldOwner != "All"))
            {
                query = "select distinct tblSiteLocation.SiteLocID, SiteLocation from tblSiteLocation join(Select distinct SiteLocId from Vw_tblRegister where (OwnerName = '" + SiteOwnerName.Replace("'", "''") + "' OR 'All' = '" + SiteOwnerName.Replace("'", "''") + "') and(ScaffoldOwner = '" + ScaffoldOwner.Replace("'", "''") + "' OR 'All' = '" + ScaffoldOwner.Replace("'", "''") + "') and ClientId = '" + databaseCon.Decrypt(id) + "') FromRegister On tblSiteLocation.SiteLocID = FromRegister.SiteLocId where ClientId = '" + databaseCon.Decrypt(id) + "' order by SiteLocation";
            }
            else
            {
                query = "select distinct SiteLocID,SiteLocation from tblSiteLocation where SiteLocation <>'' and SiteLocation is not null and  ClientId='" + databaseCon.Decrypt(id) + "' order by SiteLocation";
            }

            var getData = dbContext.Database.SqlQuery<DropDownViewModel>(query).ToList();
            return Json(getData, JsonRequestBehavior.AllowGet);

        }
        public ActionResult LocationDroupdown(string id, string UID, string Siteid)
        {
            string query;
            DataTable dt;
            var SiteOwnerName = "";
            var ScaffoldOwner = "";
            var Location = "";
            var SiteIdFromUser = "";
            string query1 = "select Site, Location ,case when SiteOwnerName is null or SiteOwnerName='' then 'All' Else SiteOwnerName end as SiteOwnerName, case when ScaffoldOwner is null or ScaffoldOwner='' then 'All' Else ScaffoldOwner end  ScaffoldOwner from tblUser where ClientId='" + databaseCon.Decrypt(id) + "' and UserId='" + databaseCon.Decrypt(UID) + "'";
            DataTable dt1 = databaseCon.GetData(query1);
            if (dt1.Rows.Count > 0)
            {
                SiteOwnerName = dt1.Rows[0]["SiteOwnerName"].ToString();
                ScaffoldOwner = dt1.Rows[0]["ScaffoldOwner"].ToString();
                Location = dt1.Rows[0]["Location"].ToString();
                SiteIdFromUser = dt1.Rows[0]["Site"].ToString();
            }

            //query = "select area,AreaID from tblArea where ClientID='" + databaseCon.Decrypt(id) + "' ";
            query = "select AreaID,area+' ('+(Select SiteLocation from tblSiteLocation where ClientID=Area.ClientID and SiteLocId = Area.SiteLocId)+')' as Area from tblArea Area  where ClientID = '" + databaseCon.Decrypt(id) + "' ";

            // if (!String.IsNullOrEmpty(ddlSitesLoc.SelectedValue))
            if (Siteid != "")
            {
                query = query + "  and (SiteLocId = '" + Siteid + "')";
            }
            if (SiteIdFromUser != "")
            {
                query = query + "  and (SiteLocId = '" + SiteIdFromUser + "')";
            }
            if (!Location.IsNullOrWhiteSpace())
            {
                query = query + "  and (AreaId = '" + Location + "')";

            }
            else if ((SiteOwnerName != "" && SiteOwnerName != "All") || (ScaffoldOwner != "" && ScaffoldOwner != "All"))
            {
                query = query + " and AreaId in (Select distinct LocationId from Vw_tblRegister where (OwnerNAme = '" + SiteOwnerName.Replace("'", "''") + "' OR 'All' = '" + SiteOwnerName.Replace("'", "''") + "') and(ScaffoldOwner = '" + ScaffoldOwner.Replace("'", "''") + "' OR 'All' = '" + ScaffoldOwner.Replace("'", "''") + "') and ClientId = '" + databaseCon.Decrypt(id) + "')";
            }
            query = query + " order by area";

            var getData = dbContext.Database.SqlQuery<DropDownViewModel>(query).ToList();
            return Json(getData, JsonRequestBehavior.AllowGet);

        }


        // Add Filter Condation on 15/9/2020 End
        public ActionResult UpdateFilterTable(string id, string UID, string UserRole, string PageName, string SiteID, string LocationID, string WorkGroup, string SubGroup)
        {
            string query = "";
            DataTable dt;
            if (databaseCon.Decrypt(UserRole) != "10")
            {
                //query = "select Count(*) as TotalEntry from tblPageDropdownFilter  where ClientID='" + databaseCon.Decrypt(id) + "' and UserId='" + databaseCon.Decrypt(UID) + "'";
                query = "select Count(1) as TotalEntry from tblPageDropdownFilter where ClientID='" + databaseCon.Decrypt(id) + "' and UserId='" + databaseCon.Decrypt(UID) + "'";
                dt = databaseCon.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["TotalEntry"].ToString() == "0")
                    {
                        query = "insert  into tblPageDropdownFilter (ClientID,UserId,PageName,SiteID,LocationID,WorkGroup,SubGroup) values('" + databaseCon.Decrypt(id) + "','" + databaseCon.Decrypt(UID) + "','" + PageName + "','" + SiteID + "','" + LocationID + "','" + WorkGroup.Replace("'", "''") + "','" + SubGroup.Replace("'", "''") + "')";
                    }
                    else
                    {
                        query = "update tblPageDropdownFilter set SiteID='" + SiteID + "', LocationID='" + LocationID + "',WorkGroup='" + WorkGroup.Replace("'", "''") + "',SubGroup='" + SubGroup.Replace("'", "''") + "' where ClientID='" + databaseCon.Decrypt(id) + "' and UserId='" + databaseCon.Decrypt(UID) + "'";
                    }
                    int i = databaseCon.RunCmd(query);
                }
            }

            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        // Add Filter Condation on 15/9/2020 Start
        //public ActionResult dropdownWorkGoups(string id, string Siteid, string LocationID)
        //{
        //    string query;
        //    DataTable dt;

        //    query = "select distinct WorkGroup,WorkGroupID,(select SiteLocation from tblSiteLocation where SiteLocId=tblRegister.SiteLocId) as DropdownSitelocation from tblRegister where ClientId='" + databaseCon.Decrypt(id) + "'";
        //    if (Siteid != "")
        //    {
        //        query = query + "  and(SiteLocId = '" + Siteid + "')";
        //    }
        //    if (LocationID != "")
        //    {
        //        query = query + "  and(LocationId = '" + LocationID + "')";
        //    }
        //    query = query + "and  WorkGroup is not null and WorkGroup!= ''  Order by WorkGroup";

        //    dt = databaseCon.GetData(query);

        //    var getData = dbContext.Database.SqlQuery<DropDownViewModel>(query);
        //    return Json(getData, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult dropdownWorkGoups(string id, string UID, string Siteid, string LocationID,string BuildingID)
        {
            string query;
            DataTable dt;
            var SiteOwnerName = "";
            var ScaffoldOwner = "";
            var Site = "";
            var Location = "";

            // Get user restrictions
            string query1 = "select Site, Location, case when SiteOwnerName is null or SiteOwnerName='' then 'All' Else SiteOwnerName end as SiteOwnerName, case when ScaffoldOwner is null or ScaffoldOwner='' then 'All' Else ScaffoldOwner end ScaffoldOwner from tblUser where ClientId='" + databaseCon.Decrypt(id) + "' and UserId='" + databaseCon.Decrypt(UID) + "'";
            DataTable dt1 = databaseCon.GetData(query1);
            if (dt1.Rows.Count > 0)
            {
                SiteOwnerName = dt1.Rows[0]["SiteOwnerName"].ToString();
                ScaffoldOwner = dt1.Rows[0]["ScaffoldOwner"].ToString();
                Site = dt1.Rows[0]["Site"].ToString();
                Location = dt1.Rows[0]["Location"].ToString();
            }

            query = "select distinct WorkGroup, WorkGroupID, (select SiteLocation from tblSiteLocation where SiteLocId=Vw_tblRegister.SiteLocId) as DropdownSitelocation from Vw_tblRegister where ClientId='" + databaseCon.Decrypt(id) + "'";

            // Apply user-specific Site restriction
            if (!Site.IsNullOrWhiteSpace())
            {
                query = query + " and (SiteLocId = '" + Site + "')";
            }
            else if (Siteid != "")
            {
                query = query + " and (SiteLocId = '" + Siteid + "')";
            }

            // Apply user-specific Location restriction
            if (!Location.IsNullOrWhiteSpace())
            {
                query = query + " and (LocationId = '" + Location + "')";
            }
            else if (LocationID != "")
            {
                query = query + " and (LocationId = '" + LocationID + "')";
            }

            // Apply SiteOwnerName and ScaffoldOwner filters
            if ((SiteOwnerName != "" && SiteOwnerName != "All") || (ScaffoldOwner != "" && ScaffoldOwner != "All"))
            {
                query = query + " and ((OwnerName = '" + SiteOwnerName.Replace("'", "''") + "' OR 'All' = '" + SiteOwnerName.Replace("'", "''") + "') and (ScaffoldOwner = '" + ScaffoldOwner.Replace("'", "''") + "' OR 'All' = '" + ScaffoldOwner.Replace("'", "''") + "'))";
            }
            if (!BuildingID.IsNullOrWhiteSpace())
            {
                query = query + " and (BuildingID = '" + BuildingID + "')";
            }
            else if (BuildingID != "")
            {
                query = query + " and (BuildingID = '" + BuildingID + "')";
            }
            query = query + " and WorkGroup is not null and WorkGroup != '' Order by WorkGroup";

            var getData = dbContext.Database.SqlQuery<DropDownViewModel>(query).ToList();
            return Json(getData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult dropdownBuilding(string id, string Siteid, string LocationID)
        {
            string query;
            DataTable dt;

            query = "select distinct BuildingID,(select BuildingNo from tblBuilding where BuildingID = tblRegister.BuildingID) as DropdownBuilding, (select SiteLocation from tblSiteLocation where SiteLocId=tblRegister.SiteLocId) as DropdownSitelocation from tblRegister where ClientId = '" + databaseCon.Decrypt(id) + "'";

            if (Siteid != "")
            {
                query = query + " and(SiteLocId = '" + Siteid + "')";
            }
            if (LocationID != "")
            {
                query = query + " and(LocationId = '" + LocationID + "')";
            }

            query = query + " and (select BuildingNo from tblBuilding where BuildingID = tblRegister.BuildingID) is not null and (select BuildingNo from tblBuilding where BuildingID = tblRegister.BuildingID) <> '' Order by DropdownBuilding";

            var getData = dbContext.Database.SqlQuery<DropDownViewModel>(query).ToList();
            return Json(getData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult dropdownSubGroups(string id, string Siteid, string LocationID, string WorkGroup)
        {
            string query;
            DataTable dt;

            query = "select distinct SubGroup,SubGroupId,(select SiteLocation from tblSiteLocation where SiteLocId=tblRegister.SiteLocId) as DropdownSitelocation from tblRegister where ClientId = '" + databaseCon.Decrypt(id) + "'";
            if (Siteid != "")
            {
                query = query + "  and(SiteLocId = '" + Siteid + "')";
            }
            if (LocationID != "")
            {
                query = query + "  and(LocationId = '" + LocationID + "')";
            }
            if (WorkGroup != "")
            {
                query = query + "  and(WorkGroupid = '" + WorkGroup + "')";
            }
            query = query + "and  SubGroup is not null and SubGroup!= ''  Order by SubGroup";

            dt = databaseCon.GetData(query);
            var getData = dbContext.Database.SqlQuery<DropDownViewModel>(query);
            return Json(getData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult dropdownSupervisor(string id, string Siteid, string LocationID, string WorkGroup, string SubGroup, string BuildingId)
        {
            string query;
            DataTable dt;

            query = "select distinct UserId,firstName+' '+LastName as Supervisor from tblUser join tblUserCategory on  tblUser.ClientId=tblUserCategory.ClientId and tblUser.CategoryId=tblUserCategory.UserCatId join tblRegister on tblRegister.SupervisorId=tblUser.UserId  where Category='Supervisor' and tbluser.ClientId = '" + databaseCon.Decrypt(id) + "'";
            if (Siteid != "")
            {
                query = query + "  and(SiteLocId = '" + Siteid + "')";
            }
            if (LocationID != "")
            {
                query = query + "  and(LocationId = '" + LocationID + "')";
            }
            if (WorkGroup != "")
            {
                query = query + "  and (WorkGroupId = '" + WorkGroup.Replace("'", "''") + "')";
            }
            if (SubGroup != "")
            {
                query = query + "  and (SubGroupId = '" + SubGroup.Replace("'", "''") + "')";
            }
            if (BuildingId != "")
            {
                query = query + "  and(BuildingId = '" + BuildingId + "')";
            }
            query = query + "and  SupervisorId is not null and SupervisorId!= ''  order by Supervisor";

            dt = databaseCon.GetData(query);
            var getData = dbContext.Database.SqlQuery<DropDownViewModel>(query);
            return Json(getData, JsonRequestBehavior.AllowGet);
        }
        // public ActionResult GetRegisterListData(string id, string UID, string showCompleted, string showCurrent, string showBuilt1, string showAll, string ddlSitesLoc, string ddlLocaSite, string checkHideCompleted,string hdnTimeOffSet)
        public ActionResult GetRegisterListData(string id, string UID, string showCompleted, string showCurrent, string showBuilt1, string showAll, string ddlSitesLoc, string ddlLocaSite, string checkHideCompleted, string hdnTimeOffSet, string WorkGroup, string SubGroup, string Supervisor,string BuildingID, string IsMasterJob)
        {

            // var GetData = dbContext.Database.SqlQuery<tblRegister>("select * from tblRegister where ClientID='" + databaseCon.Decrypt(id) + "'").ToList();
            //  return Json(GetData, JsonRequestBehavior.AllowGet);

            var SiteOwnerName = "";
            var ScaffoldOwner = "";
            var Site = "";
            var Location = "";
            string query1 = "select Site ,Location ,SiteOwnerName,ScaffoldOwner from tblUser where ClientId='" + databaseCon.Decrypt(id) + "' and UserId='" + databaseCon.Decrypt(UID) + "'";
            DataTable dt1 = databaseCon.GetData(query1);
            if (dt1.Rows.Count > 0)
            {

                SiteOwnerName = dt1.Rows[0]["SiteOwnerName"].ToString();
                ScaffoldOwner = dt1.Rows[0]["ScaffoldOwner"].ToString();
                Site = dt1.Rows[0]["Site"].ToString();
                Location = dt1.Rows[0]["Location"].ToString();
                //string dbSite = dt1.Rows[0]["Site"].ToString();
                //string dbLocation = dt1.Rows[0]["Location"].ToString();

                if (!string.IsNullOrWhiteSpace(Site))
                {
                    ddlSitesLoc = Site;
                }

                if (!string.IsNullOrWhiteSpace(Location))
                {
                    ddlLocaSite = Location;
                }
            }

            string query = "";
            DataTable dt;
            // query = "exec Pro_GetRegisterReport '" + HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["id"]));
            query = "exec Pro_GetRegisterReport '" + databaseCon.Decrypt(id);
            // if (Convert.ToBoolean(ViewState["showCompleted"]) == true)
            if (showCompleted == "True")
            {
                // query = query + "','Completed','" + SiteOwnerName.Replace("'", "''") + "','" + ScaffoldOwner.Replace("'", "''") + "'";
                query = query + "','Completed','" + SiteOwnerName.Replace("'", "''") + "','" + ScaffoldOwner.Replace("'", "''") + "'," + Convert.ToDouble((hdnTimeOffSet)) * (-1) + ",'" + ddlSitesLoc + "','" + ddlLocaSite + "','" + WorkGroup + "','" + SubGroup + "','" + Supervisor + "','" + BuildingID + "'";
            }
            // else if (Convert.ToBoolean(ViewState["showCurrent"]) == true)
            else if (showCurrent == "True")
            {
                // query = query + "','Current','" + SiteOwnerName.Replace("'", "''") + "','" + ScaffoldOwner.Replace("'", "''") + "'";
                query = query + "','Current','" + SiteOwnerName.Replace("'", "''") + "','" + ScaffoldOwner.Replace("'", "''") + "'," + Convert.ToDouble((hdnTimeOffSet)) * (-1) + ",'" + ddlSitesLoc + "','" + ddlLocaSite + "','" + WorkGroup + "','" + SubGroup + "','" + Supervisor + "','" + BuildingID + "'";
            }
            // else if (Convert.ToBoolean(ViewState["showBuilt1"]) == true)
            else if (showBuilt1 == "True")
            {
                //  query = query + "','Built','" + SiteOwnerName.Replace("'", "''") + "','" + ScaffoldOwner.Replace("'", "''") + "'";
                query = query + "','Built','" + SiteOwnerName.Replace("'", "''") + "','" + ScaffoldOwner.Replace("'", "''") + "'," + Convert.ToDouble((hdnTimeOffSet)) * (-1) + ",'" + ddlSitesLoc + "','" + ddlLocaSite + "','" + WorkGroup + "','" + SubGroup + "','" + Supervisor + "','" + BuildingID + "'";

            }
            // else if (Convert.ToBoolean(ViewState["showAll"]) == true)
            else if (showAll == "True")
            {
                // query = query + "','all','" + SiteOwnerName.Replace("'", "''") + "','" + ScaffoldOwner.Replace("'", "''") + "'";
                query = query + "','all','" + SiteOwnerName.Replace("'", "''") + "','" + ScaffoldOwner.Replace("'", "''") + "'," + Convert.ToDouble((hdnTimeOffSet)) * (-1) + ",'" + ddlSitesLoc + "','" + ddlLocaSite + "','" + WorkGroup + "','" + SubGroup + "','" + Supervisor + "','" + BuildingID + "'";

            }
            else if (IsMasterJob == "True")
            {
                query = query + "','MasterJob','" + SiteOwnerName.Replace("'", "''") + "','" + ScaffoldOwner.Replace("'", "''") + "'," + Convert.ToDouble(hdnTimeOffSet) * (-1);
            }

            else
            {
                //ViewState["showAll"] = false;
                //ViewState["showBuilt1"] = false;
                //ViewState["showCurrent"] = false;
                //ViewState["showCompleted"] = false;



                query = "exec [Pro_TBLREGISTER_GET] '','" + databaseCon.Decrypt(id);

                // if (ddlSitesLoc.SelectedValue != "")
                if (ddlSitesLoc != "")
                {
                    query = query + "','" + ddlSitesLoc + "";

                }
                else
                {
                    query = query + "','";

                }

                // if (ddlLocaSite.SelectedValue != "")
                if (ddlLocaSite != "")
                {
                    query = query + "','" + ddlLocaSite + "";
                }
                else
                {
                    query = query + "','";
                }
                if (SiteOwnerName != "")
                {
                    query = query + "','" + SiteOwnerName.Replace("'", "''") + "";
                }
                else
                {
                    query = query + "','";
                }
                if (ScaffoldOwner != "")
                {
                    query = query + "','" + ScaffoldOwner.Replace("'", "''") + "";
                }
                else
                {
                    query = query + "','";
                }

                // Add Code on 13-Feb-2020.
                //if (checkHideCompleted.Checked == true)
                if (checkHideCompleted == "True")
                {
                    query = query + "','1";

                }
                else
                {
                    query = query + "','";
                }
                // End Code on 13-Feb-2020.
                query = query + "','" + Convert.ToDouble((hdnTimeOffSet)) * (-1) + "";  // Add this Line On 01 Aug 2020
                if (WorkGroup != "")
                {
                    query = query + "','" + WorkGroup.Replace("'", "''") + "";
                }
                else
                {
                    query = query + "','";
                }
                if (SubGroup != "")
                {
                    query = query + "','" + SubGroup.Replace("'", "''") + "";
                }
                else
                {
                    query = query + "','";
                }
                if (Supervisor != "")
                {
                    query = query + "','" + Supervisor.Replace("'", "''") + "";
                }
                else
                {
                    query = query + "','";
                }
                if (BuildingID != "")
                {
                    query = query + "','" + BuildingID + "";
                }
                else
                {
                    query = query + "','";
                }
                query = query + "','',''";


                //dt = databaseCon.GetData(query);
                //if (dt.Rows.Count < 1)
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "hide", "<script>filterhide();</script>", false);
                //}
                //GridRegiterList.DataSource = dt;
            }
            dt = databaseCon.GetData(query);
            if (dt.Rows.Count < 1)
            {
                //  ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "hide", "<script>filterhide();</script>", false);
            }
            // GridRegiterList.DataSource = dt;

            // var GetData = dbContext.Database.SqlQuery<tblRegister>("select * from tblRegister where ClientID='" + databaseCon.Decrypt(id) + "'").ToList();

            dbContext.Database.CommandTimeout = 15 * 60;
            var GetData = dbContext.Database.SqlQuery<Pro_TBLREGISTER_GETViewModel>(query).ToList();
            // return Json(GetData, JsonRequestBehavior.AllowGet);
            var jsonResult = Json(GetData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult btnQRCodeClick_Click(string id, string UID, string UserRole, string tempimg, string hdnfrom)
        {
            string file = "";
            string scafref = "";
            string RedirectPath = "";
            // if (tempimg.Value != "")
            if (tempimg != "")
            {
                //file = tempimg.Value;
                file = tempimg;
                int index = file.IndexOf("_");
                scafref = file.Substring(index + 1);
                string query = "select ClientID, QRCodeImage,Right ('000000' + convert (varchar, [scaffref]),6) as ScaffRref,registerid from tblRegister where ClientID='" + databaseCon.Decrypt(id) + "' and scaffref='" + scafref + "' and QRCodeImage='" + file + "'";
                DataTable dt = databaseCon.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    string RegisterID = dt.Rows[0]["registerid"].ToString();
                    String Registerid = databaseCon.Encrypt(RegisterID);
                    // if (hdnfrom.Value == "1")
                    if (hdnfrom == "1")
                    {

                        // Response.Redirect("Register.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RID=" + databaseCon.Encrypt(RegisterID), false);
                        //RedirectPath = "/Register/Register?UID='" + UID + " '&UserRole=' " + UserRole + " '&id=' " + id + " '&RID=' " + databaseCon.Encrypt(RegisterID) + "";
                        RedirectPath = $"/Register/Register?UID={UID}&UserRole={UserRole}&id={id}&RID={Registerid}";

                    }
                    //if (hdnfrom.Value == "2")
                    if (hdnfrom == "2")
                    {
                        //  Response.Redirect("InspectionList.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RID=" + databaseCon.Encrypt(RegisterID), false);
                        RedirectPath = $"/InspectionList/InspectionList?UID={UID}&UserRole={UserRole}&id={id}&RID={Registerid}";

                    }
                    return Json(new { Result = "Success", Message = RedirectPath });
                }
                else
                {
                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong>Please check the QR Code. No matching job found for this QR Code. ";
                    return Json(new { Result = "Failed", Message = "Please check the QR Code.No matching job found for this QR Code." });
                }

            }
            else
            {
                //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                //art1.Style.Add("display", "block");
                //alertlabel.Text = "<strong>Alert ! </strong>Please check the QR Code. No matching job found for this QR Code. ";
                return Json(new { Result = "Failed", Message = "Please check the QR Code. No matching job found for this QR Code." });
            }

        }

        public ActionResult btndeleteyes_Click(string id, string RegisterID)
        {
            string query;
            DataTable dt;
            query = "exec Pro_TBLREGISTER_DEL '" + RegisterID + "'";
            dt = databaseCon.GetData(query);
            if (dt.Rows.Count != 0)
            {
                if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                {
                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Success ! </strong>" + dt.Rows[0][1].ToString();
                    //GridRegiterList.Rebind();
                    return Json(new { Result = "Success", Message = dt.Rows[0][1].ToString() });
                }
                else if (Convert.ToInt32(dt.Rows[0][0]) == -1)
                {
                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                    return Json(new { Result = "Failed", Message = dt.Rows[0][1].ToString() });
                }
                else
                {
                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                    return Json(new { Result = "Failed", Message = dt.Rows[0][1].ToString() });
                }
            }
            else
            {
                //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                //art1.Style.Add("display", "block");
                //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator ";
                return Json(new { Result = "Failed", Message = "Syntax Error. Contact administrator." });
            }
            //divdelete.Style.Add("display", "none");
        }
        //public ActionResult btnofflineconfirm_Click(string id, string UID, string UserRole, string GetOfflinestatus)
        //{
        //    var Result = "";
        //    var Message = "";
        //    try
        //    {
        //        DataTable GetDt = databaseCon.GetData("select * from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'");
        //        if (GetDt.Rows.Count > 0)
        //        {
        //            string domainName = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);

        //            //  string Link = domainName + "/OfflineRegister.html?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + databaseCon.Decrypt(Request.QueryString["id"]) + "&offlinestatus=" + GetOfflinestatus + "";
        //            string Link = domainName + "/OfflineRegister.html?UID=" + UID + "&UserRole=" + UserRole + "&id=" + id + "&offlinestatus=" + GetOfflinestatus + "";
        //            string strText = "Here is the <a href='" + Link + "'>Link</a> to access Offline Register.";
        //            // databaseCon.SendEmail(GetDt.Rows[0]["EmailID"].ToString(), "Test@gmail.com", "Offline Register", strText, "");
        //            databaseCon.SendEmail(GetDt.Rows[0]["EmailID"].ToString(), "", "Offline Register", strText, "");
        //            Result = "Success";
        //            Message = "Please check your email and click on the link before disconnecting your internet connection .";

        //        }
        //        else
        //        {
        //            Result = "Failed";
        //            Message = "Syntax Error. Contact administrator.";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Result = "Failed";
        //        Message = ex.Message;
        //    }
        //    return Json(new { Result = Result, Message = Message });
        //}

        public ActionResult btnofflineconfirm_Click(string id, string UID, string UserRole, string GetOfflinestatus, string Siteid, List<OffLineInspectionRegisterListViewModel> model)
        {
            string query;
            //DataTable Registerdt;
            var Result = "";
            var Message = "";
            string Link = "";
            try

            {
                //DataTable GetDt = databaseCon.GetData("select * from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'");
                DataTable GetDt = databaseCon.GetData("select RegisterID from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'");

                foreach (OffLineInspectionRegisterListViewModel item in model)
                {

                    var Array = item;
                    var RegisterID = Array.RegisterID;

                    //  Check for If Register already  Created those Register in Offline Mode  Then  No Register Created

                    string qry = "select RegisterID from tblRegister where IsOfflineCurrent=1 and  ClientId='" + databaseCon.Decrypt(id) + "'  and RegisterID='" + RegisterID + "'";
                    DataTable dt = databaseCon.GetData(qry);
                    if (dt.Rows.Count < 1)
                    {
                        query = "update tblRegister set IsOfflineCurrent=1 ,GoOfflineJobBy = '" + GetDt.Rows[0]["EmailID"].ToString() + "',GoOfflineJobOn = GETDATE()  where ClientId='" + databaseCon.Decrypt(id) + "'  and RegisterID='" + RegisterID + "'";
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
                }

                if (GetDt.Rows.Count > 0)
                {

                    string domainName = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);

                    //string Link = domainName + "/OfflineRegister.html?UID=" + UID + "&UserRole=" + UserRole + "&id=" + id + "&offlinestatus=" + GetOfflinestatus + "";
                    Link = domainName + "/OfflineRegisterList.html?UID=" + UID + "&UserRole=" + UserRole + "&id=" + id + "&offlinestatus=" + GetOfflinestatus + "&Siteid=" + databaseCon.Encrypt(Siteid);
                    Console.WriteLine(Link);
                    string strText = "Here is the <a href='" + Link + "'>Link</a> to access Offline Register.";
                    // databaseCon.SendEmail(GetDt.Rows[0]["EmailID"].ToString(), "Test@gmail.com", "Offline Register", strText, "");
                    //databaseCon.SendEmail(GetDt.Rows[0]["EmailID"].ToString(), "", "Offline Register", strText, "");
                    Result = "Success";
                    //Message = "Please check your email and click on the link before disconnecting your internet connection .";
                    Message = "Please stay online. The link is now opening in a new tab.";

                }
                //else
                //{
                //    Result = "Failed";
                //    Message = "Syntax Error. Contact administrator.";
                //}
            }
            catch (Exception ex)
            {
                Result = "Failed";
                Message = ex.Message;
            }
            return Json(new { Result = Result, Message = Message, Link = Link });
        }

        public ActionResult btnOfflineLink_Click(string id, string UID, string UserRole, string GetOfflinestatus, string RegisterID)
        {
            var Result = "";
            var Message = "";
            //try
            //{
            string qrycheck = "select RegisterID from tblRegister where IsOfflineCurrent=1 and ClientId='" + databaseCon.Decrypt(id) + "'";
            DataTable dt1 = databaseCon.GetData(qrycheck);
            if (dt1.Rows.Count > 0)
            {
                if (RegisterID != "")
                {
                    Result = "Success";
                    Message = "Adddocs";
                }
                else
                {
                    //DataTable GetDt = databaseCon.GetData("select * from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'");
                    DataTable GetDt = databaseCon.GetData("select count(1) as UserCount from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'");
                    if (GetDt.Rows.Count > 0)
                    {
                        string domainName = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);

                        string Link = domainName + "/OfflineRegister.html?UID=" + UID + "&UserRole=" + UserRole + "&id=" + id + "&offlinestatus=" + GetOfflinestatus + "";
                        string strText = "Here is the <a href='" + Link + "'>Link</a> to access Offline Register.";
                        // databaseCon.SendEmail(GetDt.Rows[0]["EmailID"].ToString(), "Test@gmail.com", "Offline Register", strText, "");
                        //databaseCon.SendEmail(GetDt.Rows[0]["EmailID"].ToString(), "", "Offline Register", strText, "");
                        Result = "Success";
                        Message = "Please check your email and click on the link before disconnecting your internet connection .";

                    }
                    else
                    {
                        Result = "Failed";
                        Message = "Syntax Error. Contact administrator.";
                    }

                }
            }
            else
            {
                if (RegisterID != "")
                {
                    Result = "Success";
                    Message = "Adddocs";
                }
                else
                {

                    Result = "Failed";
                    Message = "Please select Job then click Offline Register button .";
                }
            }
            //}
            //catch (Exception ex)
            //{
            //    Result = "Failed";
            //    Message = ex.Message;
            //}
            return Json(new { Result = Result, Message = Message });
        }

        public ActionResult btnOfflinedocs_Click(string id, string UserRole, string UID, string RegisterID, string hdnOfflineStatus)
        {
            var Result = "";
            var Message = "";
            string qrycheck = "select InspectionID from tblInspection where IsOffline=1 and  UserId='" + databaseCon.Decrypt(UID) + "'";
            DataTable dt1 = databaseCon.GetData(qrycheck);
            if (dt1.Rows.Count > 0)
            {
                if (RegisterID != "")
                {
                    //Adddocs.Style.Add("display", "block");
                    //art1.Style.Add("display", "none");
                    Result = "Success";
                    Message = "Adddocs";
                }
                else
                {        // Redirect Page To Offline Inspection


                    //DataTable GetDt = databaseCon.GetData("select * from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'");
                    DataTable GetDt = databaseCon.GetData("select Count(1) as UserCount from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'");
                    if (GetDt.Rows.Count > 0)
                    {
                        string GetOfflinestatus = hdnOfflineStatus;

                        string domainName = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
                        // string domainName = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);

                        // string Link = domainName + "/Offline.html?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RID=" + Request.QueryString["RID"] + "&CheckListID=" + Request.QueryString["CheckListID"] + "&InspID=" + Request.QueryString["InspID"] + "&InspectionID=" + databaseCon.Decrypt(Request.QueryString["InspID"]) + "&offlinestatus=" + GetOfflinestatus + "";

                        // string Link = domainName + "/OfflineInspectionList.html?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&offlinestatus=" + GetOfflinestatus + "";
                        string Link = domainName + "/OfflineInspectionList.html?UID=" + UID + "&UserRole=" + UserRole + "&id=" + id + "&offlinestatus=" + GetOfflinestatus + "";

                        string strText = "Here is the <a href='" + Link + "'>Link</a> to access Offline Inspection.";
                        // databaseCon.SendEmail(GetDt.Rows[0]["EmailID"].ToString(), "Test@gmail.com", "Offline Inspection", strText, "");
                        //databaseCon.SendEmail(GetDt.Rows[0]["EmailID"].ToString(), "", "Offline Inspection", strText, "");

                        //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Success ! </strong>The link to go offline for the selected inspection has been send to your email. </br>Please check your email and click on the link before disconnecting your internet connection .";
                        Result = "Success";
                        Message = "The link to go offline for the selected inspection has been sent to your email. </br>Please check your email and click on the link before disconnecting your internet connection .";
                    }
                }

            }
            else
            {
                if (RegisterID != "")
                {

                    //Adddocs.Style.Add("display", "block");
                    //art1.Style.Add("display", "none");
                    Result = "Success";
                    Message = "Adddocs";
                }
                else
                {
                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong>Please select Job then click Offline inspection button . ";
                    Result = "Failed";
                    Message = "Please select Job then click Offline inspection button .";
                }
            }
            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckuserUserisOfflineMode(string id, string UID, string Siteid, string RegisterID)
        {

            //string UserVerifyedQuery = "SELECT CASE WHEN EXISTS (SELECT 1 FROM tblregister r WHERE r.IsOfflineCurrent = 1 AND r.clientid = '" + databaseCon.Decrypt(id) + "' AND r.SiteLocId = '" + Siteid + "' AND r.GoOfflineJobBy = (SELECT emailid FROM tbluser WHERE userid = '" + databaseCon.Decrypt(UID) + "')) OR EXISTS (SELECT 1 FROM tblregister r WHERE r.IsOfflineCurrent = 1 AND r.clientid = '" + databaseCon.Decrypt(id) + "' AND r.SiteLocId = '" + Siteid + "' AND r.GoOfflineJobBy != (SELECT emailid FROM tbluser WHERE userid = '" + databaseCon.Decrypt(UID) + "')AND r.registerid in ('"+ databaseCon.Decrypt(RegisterID)+ "')) THEN 0 ELSE 1 END AS CanGoOffline";
            // update on  3 jan  2025 by ss 
            //string UserVerifyedQuery = "SELECT CASE WHEN EXISTS (SELECT 1 FROM tblregister r WHERE r.IsOfflineCurrent = 1 AND r.clientid = '" + databaseCon.Decrypt(id) + "' AND r.SiteLocId = '" + Siteid + "' AND r.GoOfflineJobBy = (SELECT emailid FROM tbluser WHERE userid = '" + databaseCon.Decrypt(UID) + "')) OR EXISTS (SELECT 1 FROM tblregister r WHERE r.IsOfflineCurrent = 1 AND r.clientid = '" + databaseCon.Decrypt(id) + "' AND r.SiteLocId = '" + Siteid + "' AND r.GoOfflineJobBy != (SELECT emailid FROM tbluser WHERE userid = '" + databaseCon.Decrypt(UID) + "')AND r.registerid in ('" + databaseCon.Decrypt(RegisterID)+ "')) THEN 0  WHEN EXISTS (SELECT 1 FROM tblregister r WHERE r.IsOfflineCurrent = 1 AND r.clientid = '" + databaseCon.Decrypt(id) + "' AND r.GoOfflineJobBy = (SELECT emailid FROM tbluser WHERE userid = '" + databaseCon.Decrypt(UID) + "'))  then 0 ELSE 1 END AS CanGoOffline";
            // updated  on  04 jan 2024 by ss

            //string UserVerifyedQuery = "SELECT case when '" + databaseCon.Decrypt(RegisterID) + "'=(SELECT RegisterId FROM tblregister r WHERE r.IsOfflineCurrent = 1 AND r.clientid ='" + databaseCon.Decrypt(id) + "' AND r.SiteLocId ='" + Siteid + "' AND r.GoOfflineJobBy = (SELECT emailid FROM tbluser WHERE userid = '" + databaseCon.Decrypt(UID) + "')) then 1 WHEN EXISTS (SELECT 1 FROM tblregister r WHERE r.IsOfflineCurrent = 1 AND r.clientid ='" + databaseCon.Decrypt(id) + "' AND r.SiteLocId = '" + Siteid + "' AND r.GoOfflineJobBy = (SELECT emailid FROM tbluser WHERE userid = '" + databaseCon.Decrypt(UID) + "')) OR EXISTS (SELECT 1 FROM tblregister r WHERE r.IsOfflineCurrent = 1 AND r.clientid ='" + databaseCon.Decrypt(id) + "' AND r.SiteLocId ='" + Siteid + "' AND r.GoOfflineJobBy != (SELECT emailid FROM tbluser WHERE userid = '" + databaseCon.Decrypt(UID) + "')AND r.registerid in ('" + databaseCon.Decrypt(RegisterID) + "')) THEN 0  WHEN EXISTS (SELECT 1 FROM tblregister r WHERE r.IsOfflineCurrent = 1 AND r.clientid ='" + databaseCon.Decrypt(id) + "' AND r.GoOfflineJobBy = (SELECT emailid FROM tbluser WHERE userid = '" + databaseCon.Decrypt(UID) + "'))  then 0 ELSE 1 END AS CanGoOffline ";
            string UserVerifyedQuery = "exec Pro_CheckUserIsValidForOfflineRegister   '" + databaseCon.Decrypt(RegisterID) + "','" + databaseCon.Decrypt(UID) + "'";


            var isuer = databaseCon.GetData(UserVerifyedQuery);
            //string finduser = "select  ScaffRef, userid,concat(firstname,' ',Lastname)UserName,GoOfflineJobBy,GoOfflineJobOn from tbluser  join tblregister on GoOfflineJobBy = EmailID where IsOfflineCurrent =1 and userid= '" + databaseCon.Decrypt(UID) + "'";
            string finduser = "select  top 1 ScaffRef, userid,concat(firstname,' ',Lastname)UserName,GoOfflineJobBy,GoOfflineJobOn from tbluser  join tblregister on GoOfflineJobBy = EmailID where IsOfflineCurrent =1 and userid= '" + databaseCon.Decrypt(UID) + "'";


            var FinddataUpdatedBy = databaseCon.GetData(finduser);
            var userId = "";
            var userName = "";
            var offlineBy = "";
            var scacffref = "";
            if (FinddataUpdatedBy.Rows.Count > 0)
            {
                userId = FinddataUpdatedBy.Rows[0]["userid"].ToString();
                userName = FinddataUpdatedBy.Rows[0]["UserName"].ToString();
                offlineBy = FinddataUpdatedBy.Rows[0]["GoOfflineJobBy"].ToString();
                scacffref = FinddataUpdatedBy.Rows[0]["ScaffRef"].ToString();
            }
            return Json(new { VerifyedUser = isuer.Rows[0][0], UserName = userName, scacffref = scacffref }, JsonRequestBehavior.AllowGet);
        }
        public class OffLineInspectionRegisterListViewModel
        {
            public string RegisterID { get; set; }
        }
        public ActionResult btnsave_Click(string id, string UID, string UserRole, string ddlddlInspectedByadd, string hdnOfflineStatus, string rdpInspectiondate, string hdnTimeOffSet, string ddlChecklistInsp, string ddlInspStatuspop, List<OffLineInspectionRegisterListViewModel> model)
        {
            string query;
            DataTable Inspectiondt;
            var Result = "";
            var Message = "";
            var chkOnline = false;

            // bool complete = chkOnline.Checked == true ? false : true;
            bool complete = chkOnline;
            string inspbyQuery = "select UserID, firstName+ ' '+LastName as name,LicenceNo from tblUser where IsInspector='true' and UserId='" + ddlddlInspectedByadd + "'";
            DataTable inspDt = databaseCon.GetData(inspbyQuery);
            DateTime date = Convert.ToDateTime(rdpInspectiondate);
            var AcknowladgeDateOffSet = Convert.ToDouble(hdnTimeOffSet);
            DateTime ScheduleInspectionDate = Convert.ToDateTime(rdpInspectiondate).AddMinutes(AcknowladgeDateOffSet);
            string str = "";
            //foreach (GridDataItem item in GridRegiterList.MasterTableView.Items)
            //{
            //    CheckBox chk = (CheckBox)item.FindControl("chck_Inspection");

            //    if (chk != null && chk.Checked)
            //    {

            try
            {
                foreach (OffLineInspectionRegisterListViewModel item in model)
                {
                    var Array = item;
                    var RegisterID = Array.RegisterID;

                    // var RegisterID = item.GetDataKeyValue("RegisterID").ToString();

                    //query = "exec Pro_TBLINSPECTION_SAVE   '0','" + rdpInspectiondate.SelectedDate + "','" + inspDt.Rows[0]["UserID"].ToString() + "','" + txtComments.Text.Replace("'", "''").Trim() + "','" + RegisterID + "','" + databaseCon.Decrypt(Request.QueryString["UID"]) + "','" + DateTime.Now + "','" + chkOnline.Checked + "','" + complete + "','','','','','','','','','','" + ddlInspStatuspop.SelectedValue + "','','" + inspDt.Rows[0]["LicenceNo"].ToString() + "','" + ddlChecklistInsp.SelectedValue + "','" + databaseCon.Decrypt(Request.QueryString["id"]) + "'";

                    // Line Change On 02 Jan 2020 Check for If Inspection already  Created those Register in Offline Mode  Then  No Inspection Created

                    string qry = "select InspectionID from tblInspection where IsOffline=1 and  UserId='" + databaseCon.Decrypt(UID) + "'  and RegisterID='" + RegisterID + "'";
                    DataTable dt = databaseCon.GetData(qry);
                    if (dt.Rows.Count < 1)
                    {
                        query = "exec Pro_TBLINSPECTION_SAVE   '0','" + ScheduleInspectionDate + "','" + inspDt.Rows[0]["UserID"].ToString() + "','','" + RegisterID + "','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "','" + chkOnline + "','" + complete + "','','','','','','','','','','" + ddlInspStatuspop + "','','" + inspDt.Rows[0]["LicenceNo"].ToString() + "','" + ddlChecklistInsp + "','" + databaseCon.Decrypt(id) + "'," + "1" + ",'" + databaseCon.Decrypt(UID) + "'," + ((AcknowladgeDateOffSet) * (-1)) + ",0";

                        // Change On 24 Jan 2020 Add InspectionTimeZone To Diffrent Between TimeZone  And Inspection Date Save To Utc Time Zone

                        // query = "exec Pro_TBLINSPECTION_SAVE   '0','" + ScheduleInspectionDate + "','" + inspDt.Rows[0]["UserID"].ToString() + "','" + txtComments.Text.Replace("'", "''").Trim() + "','" + RegisterID + "','" + databaseCon.Decrypt(Request.QueryString["UID"]) + "','" + DateTime.Now + "','" + chkOnline.Checked + "','" + complete + "','','','','','','','','','','" + ddlInspStatuspop.SelectedValue + "','','" + inspDt.Rows[0]["LicenceNo"].ToString() + "','" + ddlChecklistInsp.SelectedValue + "','" + databaseCon.Decrypt(Request.QueryString["id"]) + "'," + "1" + ",'" + databaseCon.Decrypt(Request.QueryString["UID"]) + "'," + ((AcknowladgeDateOffSet) * (-1)) + ",0";


                        Inspectiondt = databaseCon.GetData(query);
                        if (Inspectiondt.Rows.Count != 0)
                        {
                            if (Convert.ToInt32(Inspectiondt.Rows[0][0]) > 0)
                            {
                                string ddlChecklistvalue = ddlChecklistInsp;
                                //  ShowregisterDetails();
                                //  UpdateImagePath();

                                //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                                //art1.Style.Add("display", "block");
                                //alertlabel.Text = "<strong>Success ! </strong>" + Inspectiondt.Rows[0][1].ToString();

                                Result = "Failed";
                                Message = Inspectiondt.Rows[0][1].ToString();
                            }
                            else if (Convert.ToInt32(Inspectiondt.Rows[0][0]) == -1)
                            {
                                Result = "Failed";
                                Message = Inspectiondt.Rows[0][1].ToString();
                            }
                            else
                            {
                                Result = "Failed";
                                Message = Inspectiondt.Rows[0][1].ToString();
                            }
                        }
                        else
                        {
                            //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                            //art1.Style.Add("display", "block");
                            //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator ";

                            Result = "Failed";
                            Message = "Syntax Error. Contact administrator.";
                        }
                    }
                }

                // Redirect Page To Offline Inspection

                //DataTable GetDt = databaseCon.GetData("select * from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'");
                DataTable GetDt = databaseCon.GetData("select Count(1) as UserCount from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'");
                if (GetDt.Rows.Count > 0)
                {
                    string GetOfflinestatus = hdnOfflineStatus;


                    string domainName = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);

                    // string Link = domainName + "/Offline.html?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RID=" + Request.QueryString["RID"] + "&CheckListID=" + Request.QueryString["CheckListID"] + "&InspID=" + Request.QueryString["InspID"] + "&InspectionID=" + databaseCon.Decrypt(Request.QueryString["InspID"]) + "&offlinestatus=" + GetOfflinestatus + "";

                    // string Link = domainName + "/OfflineInspectionList.html?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&offlinestatus=" + GetOfflinestatus + "";

                    string Link = domainName + "/OfflineInspectionList.html?UID=" + UID + "&UserRole=" + UserRole + "&id=" + id + "&offlinestatus=" + GetOfflinestatus + "";

                    string strText = "Here is the <a href='" + Link + "'>Link</a> to access Offline Inspection.";
                    // databaseCon.SendEmail(GetDt.Rows[0]["EmailID"].ToString(), "Test@gmail.com", "Offline Inspection", strText, "");
                    //databaseCon.SendEmail(GetDt.Rows[0]["EmailID"].ToString(), "", "Offline Inspection", strText, "");

                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Success ! </strong>The link to go offline for the selected inspection has been send to your email. </br>Please check your email and click on the link before disconnecting your internet connection .";

                    Result = "Success";
                    Message = "The link to go offline for the selected inspection has been sent to your email. </br>Please check your email and click on the link before disconnecting your internet connection .";

                }
            }
            catch (Exception ex)
            {
                Result = "Failed";
                Message = ex.Message;
            }

            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult btnCopyHistorical(string id, string UID, string UserRole, string OldFloorid, string OldSiteId, string OldLocId, string OldRegisterid, string CheckInvnt, string NewRegistId, string NewLocationId, string NewFloorLevel, string NewSiteLocId, int SelectLiveJobOrPipelineJob, string startdate)
        {
            var Result = "";
            var Message = "";
            //string NewFileName;
            string txtSiteName = "";
            var NewRegId = "";

            string query1 = "exec CopyHistoricalJob '" + databaseCon.Decrypt(id) + "','" + OldSiteId + "','" + databaseCon.Decrypt(UID) + "','" + OldRegisterid + "','" + OldLocId + "','" + OldFloorid + "','" + CheckInvnt + "','" + databaseCon.Decrypt(NewRegistId) + "','" + NewSiteLocId + "','" + NewLocationId + "','" + NewFloorLevel + "' ," + SelectLiveJobOrPipelineJob + ",'" + startdate + "'";
            dbContext.Database.CommandTimeout = 15 * 60;
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

                    var GetData = dbContext.Database.SqlQuery<GetDocumentTypeData>(DocQuery).ToList();
                    var GetGalleryData = dbContext.Database.SqlQuery<DrawingPDFViewModel>(GalleryQyery).ToList();

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


        public ActionResult checkIsOfflineCurrent(string id)
        {
            string Result = "";
            string Message = "";
            bool IsOffline = false;
            List<string> RegisterID = new List<string>();

            try
            {
                string qry = "SELECT RegisterID FROM tblRegister WHERE IsOfflineCurrent = 1 AND ClientId = '" + databaseCon.Decrypt(id) + "'";
                DataTable dt = databaseCon.GetData(qry);

                if (dt.Rows.Count > 0)
                {
                    Result = "Success";
                    Message = "Data is in Offline mode.";
                    IsOffline = true;
                    foreach (DataRow row in dt.Rows)
                    {
                        RegisterID.Add(row["RegisterID"].ToString());
                    }
                }
                else
                {
                    Result = "Failed";
                    Message = "Data is in Online mode.";
                    IsOffline = false;
                }
            }
            catch (Exception ex)
            {
                Result = "Failed";
                Message = ex.Message;
            }

            return Json(new { Result = Result, Message = Message, IsOffline, RegisterID });
        }

        public ActionResult btnSaveSearchHistory(string UID, string checkHideCompleted, string LocationID, string Siteid, string ddldisciplines, string ddlWorkGroup, string ddlSubGroup, string ddlcampaignCode, string ddlBuilding, string DropDownWONumber, string ddlSubDisciplines, string ddlSupervisor)
        {
            var Result = "";
            var Message = "";

            string query1 = "select id from tblRegistersearch_history where UserId = '" + databaseCon.Decrypt(UID) + "'";
            DataTable dt1 = databaseCon.GetData(query1);
            //int id = Convert.ToInt32(dt1.Rows[0]["id"].ToString());
            if (dt1.Rows.Count != 0)
            {
                string updatequery = " update tblRegistersearch_history set showCompleted = '" + checkHideCompleted + "',LocationId = '" + LocationID + "' ,SiteLocId = '" + Siteid + "' ,discipline = '" + ddldisciplines + "',SubDiscipline = '" + ddlSubDisciplines + "',WorkGroup = '" + ddlWorkGroup + "',SubGroup = '" + ddlSubGroup + "',ClientScaffref = '" + ddlcampaignCode + "',BuildingID = '" + ddlBuilding + "',WOnumber = '" + DropDownWONumber + "',Supervisor = '" + ddlSupervisor + "',UpdatedBy = (select Emailid from tbluser where UserId ='" + databaseCon.Decrypt(UID) + "') ,UpdatedOn = '" + DateTime.Now + "' where UserId = '" + databaseCon.Decrypt(UID) + "'";

                //DataTable dt2 = databaseCon.GetData(updatequery);
                int dt2 = databaseCon.RunCmd(updatequery);
            }
            else
            {
                string insertquery = "insert into tblRegistersearch_history (UserId,showCompleted,LocationId,SiteLocId,discipline,SubDiscipline,WorkGroup,SubGroup,ClientScaffref,BuildingID,WONumber,Supervisor,Createdby,Createdon)values('" + databaseCon.Decrypt(UID) + "', '" + checkHideCompleted + "', '" + LocationID + "','" + Siteid + "','" + ddldisciplines + "','" + ddlSubDisciplines + "','" + ddlWorkGroup + "','" + ddlSubGroup + "','" + ddlcampaignCode + "','" + ddlBuilding + "','" + DropDownWONumber + "','" + ddlSupervisor + "'," + "(select Emailid from tbluser where UserId ='" + databaseCon.Decrypt(UID) + "'),'" + DateTime.Now + "')";
                int dt = databaseCon.RunCmd(insertquery);
            }
            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSearchHistory(string UID)
        {
            string query = "select showCompleted,LocationId,SiteLocId,discipline,WorkGroup,SubGroup,BuildingID,ClientScaffref,BuildingID,WONumber,Supervisor,SubDiscipline from tblRegistersearch_history where Userid = '" + databaseCon.Decrypt(UID) + "'";

            var GetData = dbContext.Database.SqlQuery<Pro_TBLREGISTERSEARCH_GETViewModel>(query).ToList();

            var jsonResult = Json(GetData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        //public List<ColumnSortModel> BuildColumnOrder(string id)
        //{
        //    List<ColumnSortModel> finalColumns = new List<ColumnSortModel>();

        //    string clientId = databaseCon.Decrypt(id);

        //    string query = "SELECT ColumnName, ItemOrder FROM tblRegisterListSorting WHERE ClientId = '" + clientId + "' ORDER BY ItemOrder";
        //    DataTable dt = databaseCon.GetData(query);

        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        finalColumns.Add(new ColumnSortModel
        //        {
        //            ColumnName = dt.Rows[i]["ColumnName"].ToString(),
        //            ItemOrder = Convert.ToInt32(dt.Rows[i]["ItemOrder"])
        //        });
        //    }

        //    return finalColumns;
        //}

        //public class ColumnSortModel
        //{
        //    public string ColumnName { get; set; }
        //    public int ItemOrder { get; set; }
        //}

        //public ActionResult GetRegisterListSortingData(string id)
        //{
        //    var sortedColumns = BuildColumnOrder(id);
        //    return Json(new { status = 1, columns = sortedColumns }, JsonRequestBehavior.AllowGet);
        //}
        public List<ColumnSortModel> BuildColumnOrderFromDb(string id)
        {
            List<ColumnSortModel> finalColumns = new List<ColumnSortModel>();

            try
            {
                var clientid = databaseCon.Decrypt(id);
                string query = "SELECT ColumnName, DisplayColumnName, ItemOrder, IsVisible,ColumnId, IsNA FROM tblRegisterListSorting WHERE ClientID = '" + clientid + "' AND IsNA = 0 ORDER BY ItemOrder";
                finalColumns = dbContext.Database.SqlQuery<ColumnSortModel>(query).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching column order: " + ex.Message);
            }

            return finalColumns;
        }

        public class ColumnSortModel
        {
            public string ColumnName { get; set; }
            public string DisplayColumnName { get; set; }
            public string ColumnId { get; set; }
            public int ItemOrder { get; set; }
            public bool IsVisible { get; set; }
            public bool IsNA { get; set; }
        }

        public ActionResult GetRegisterListSortingData(string id)
        {
            var sortedColumns = BuildColumnOrderFromDb(id);

            return Json(new
            {
                status = 1,
                columns = sortedColumns
            }, JsonRequestBehavior.AllowGet);
        }

    }
}