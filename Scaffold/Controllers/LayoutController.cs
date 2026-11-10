using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DocumentFormat.OpenXml.Office2010.Excel;
using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;
using System.IO;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Security.Policy;
using Microsoft.Ajax.Utilities;




namespace Scaffold.Controllers
{
    public class LayoutController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: Layout
        public ActionResult Index(string id, string UID, string UserRole)
        {
            ViewBag.id = databaseCon.Decrypt(id);
            ViewBag.UID = databaseCon.Decrypt(UID);
            ViewBag.UserRole = databaseCon.Decrypt(UserRole);
            return View();

        }
        public class GetHideSideMenuBarViewModel
        {
            public bool Li_OffSystem { get; set; }
            public string inventoryPlans { get; set; }
            public string CycloneManagement { get; set; }
            public bool ManHoursManagement { get; set; }
            public string FloorLevel { get; set; }
            public string ScafMapGps { get; set; }
            public string MaintainInventoryAtJob { get; set; }
            public bool HireInventory { get; set; }
            public bool LongTermStructure { get; set; }
            public bool ComparisonDashboard { get; set; }
            public Nullable<bool> HideMainDepotInventory { get; set; }


        }

        public ActionResult GetHideSideMenuBar(string id, string UID, string UserRole)
        {
            GetHideSideMenuBarViewModel getHideSideMenuBarViewModel = new GetHideSideMenuBarViewModel();
            // Line Add On 04 July 2020 For OffSysytemInspection Hide And Show.
            DataTable dtOffSystem1 = databaseCon.GetData("select distinct SiteLocID,SiteLocation from tblSiteLocation where SiteLocation <>'' and SiteLocation is not null and OffSystemInspection=1 and  ClientId='" + databaseCon.Decrypt(id) + "' order by SiteLocation");
            if (dtOffSystem1.Rows.Count > 0)
            {
                //Li_OffSystem.Visible = true;
                getHideSideMenuBarViewModel.Li_OffSystem = true;
            }
            else
            {
                // Li_OffSystem.Visible = false;
                getHideSideMenuBarViewModel.Li_OffSystem = false;
            }
            DataTable inventoryPlans = databaseCon.GetData("select Plans from tblclient where clientid='" + databaseCon.Decrypt(id) + "'");
            if (inventoryPlans.Rows.Count > 0)
            {

                if (inventoryPlans.Rows[0]["Plans"].ToString() == "Plan1")
                {
                    getHideSideMenuBarViewModel.inventoryPlans = "Plan1";
                    // LiSetup_InventoryManagement.Visible = false;
                    // LiManHrs.Visible = false;
                }
                else if (inventoryPlans.Rows[0]["Plans"].ToString() == "Plan2")
                {
                    getHideSideMenuBarViewModel.inventoryPlans = "Plan2";
                    // LiSetup_InventoryPlan1.Visible = false;
                }
            }
            // Changes On 18 July 2019 For Cyclone Dashboard Hide And Show Base On Maintain Cyclone Checked Unchecked On Site Label

            DataTable MaintainCyclone = databaseCon.GetData("select COUNT(*) as CycloneManagement from tblSiteLocation where clientid='" + databaseCon.Decrypt(id) + "' and CycloneManagement=1");

            if (MaintainCyclone.Rows[0]["CycloneManagement"].ToString() == "0")
            {
                //  LiCyloneDashBoard.Visible = false;
                getHideSideMenuBarViewModel.CycloneManagement = "0";
            }

            // Line Add On 27 July 2019 For Man Hours Management Dash Board Hide And Show 
            DataTable dtManHoursManagement = databaseCon.GetData("select isnull(ManHoursManagement,0) as ManHoursManagement from tblClient where clientid='" + databaseCon.Decrypt(id) + "'");

            if ((dtManHoursManagement.Rows[0]["ManHoursManagement"]).ToString() != "True")
            {
                // LiManHrs.Visible = false;
                getHideSideMenuBarViewModel.ManHoursManagement = false;
            }
            else
            {
                getHideSideMenuBarViewModel.ManHoursManagement = true;
            }
            DataTable dtMainDepot = databaseCon.GetData("select isnull(HideMainDepotInventory,0) as HideMainDepotInventory from tblClient where clientid='" + databaseCon.Decrypt(id) + "'");

            if ((dtMainDepot.Rows[0]["HideMainDepotInventory"]).ToString() != "True")
            {
                // LiManHrs.Visible = false;
                getHideSideMenuBarViewModel.HideMainDepotInventory = false;
            }
            else
            {
                getHideSideMenuBarViewModel.HideMainDepotInventory = true;
            }

            // Line Add On 06 Jan 2020 if No Any Site On FloorMap Checked Then FloorMap Menu Hide otherwise show. 
            DataTable dtFloorMap = databaseCon.GetData("select Count(*) as FloorLevel from tblSiteLocation where FloorLevel=1 and clientid='" + databaseCon.Decrypt(id) + "'");

            if (dtFloorMap.Rows[0]["FloorLevel"].ToString() == "0")
            {
                //  LiFloorMap.Visible = false;
                getHideSideMenuBarViewModel.FloorLevel = "0";

            }
            // Line Add On 06 Jan 2020 if No Any Site On Gps Checked Then ScapMap Menu Hide otherwise show.  
            DataTable dtScafMap = databaseCon.GetData("select Count(*) as ScafMapGps from tblSiteLocation where Gps=1 and clientid='" + databaseCon.Decrypt(id) + "'");

            if (dtScafMap.Rows[0]["ScafMapGps"].ToString() == "0")
            {
                //  LiMap.Visible = false;
                getHideSideMenuBarViewModel.ScafMapGps = "0";
            }


            // Query Change on 14 Sep 2019  Dashboard Menu Hide Show Accoding to MaintainInventoryAtJob Checked Or Unchecked.

            // DataTable MaintainInventoryAtJob = databaseCon.GetData("select COUNT(*) as MaintainInventoryAtJob from tblSiteLocation where clientid='" + HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["id"])) + "' and MaintainInventoryAtJob=1");
            DataTable MaintainInventoryAtJob = databaseCon.GetData("select COUNT(*) as MaintainInventoryAtJob from tblSiteLocation where clientid='" + databaseCon.Decrypt(id) + "' and MaintainInventoryAtJob=1");
            if (MaintainInventoryAtJob.Rows[0]["MaintainInventoryAtJob"].ToString() == "0")
            {
                getHideSideMenuBarViewModel.MaintainInventoryAtJob = "0";
                //LiDashBoard.Visible = false;
                //LiDashBoards.Visible = false;
                //LiItemUtilization.Visible = false;
                //LiWeeklyDashboard.Visible = false;
                //LiWeeklySummaryReport.Visible = false;
                //LiModularVsTubeFit.Visible = false;
                //LiHeadingReport.Visible = false;
                //LiSetup_InventoryManagement.Visible = false;

            }
            // Line Add On 30 March 2020 For HireInventory Hide And Show. 
            DataTable dtHireInventory = databaseCon.GetData("select isnull(HireInventory,0) as HireInventory from tblClient where clientid='" + databaseCon.Decrypt(id) + "'");

            if ((dtHireInventory.Rows[0]["HireInventory"]).ToString() != "True")
            {
                // LiSubCategories.Visible = false;
                getHideSideMenuBarViewModel.HireInventory = false;
            }
            else
            {
                getHideSideMenuBarViewModel.HireInventory = true;
            }

            DataTable dtLongTerm = databaseCon.GetData("select isnull(LongTermStructure,0) as LongTermStructure from tblClient where clientid='" + databaseCon.Decrypt(id) + "'");

            if ((dtLongTerm.Rows[0]["LongTermStructure"]).ToString() != "True")
            {
                getHideSideMenuBarViewModel.LongTermStructure = false;
            }
            else
            {
                getHideSideMenuBarViewModel.LongTermStructure = true;
            }

            DataTable dtComparison = databaseCon.GetData("select isnull(ComparisonDashboard,0) as ComparisonDashboard from tblClient where clientid='" + databaseCon.Decrypt(id) + "'");

            if ((dtComparison.Rows[0]["ComparisonDashboard"]).ToString() != "True")
            {
                getHideSideMenuBarViewModel.ComparisonDashboard = false;
            }
            else
            {
                getHideSideMenuBarViewModel.ComparisonDashboard = true;
            }


            return Json(getHideSideMenuBarViewModel, JsonRequestBehavior.AllowGet);
        }


        // All Page Common Function 
        public ActionResult GetHideShowControl(string pageName, string UserRole, string id)
        {
            dbContext.Database.CommandTimeout = 15 * 60;
            var GetData = dbContext.Database.SqlQuery<Pro_tblShowControl_GET_ViewModel>("exec Pro_tblShowControl_GET '" + pageName + "','" + databaseCon.Decrypt(UserRole) + "','" + databaseCon.Decrypt(id) + "'").ToList();

            return Json(GetData, JsonRequestBehavior.AllowGet);
        }


        // Get Sessionid for user Start

        //public ActionResult ForceLogOut(string id, string session)
        public ActionResult ForceLogOut(string id, string hdnoffset)
        {
            var LoginDateTime = "";
            if (Session["LoginDateTime"] == null)
            {
                LoginDateTime = "";
            }
            else
            {
                LoginDateTime = Session["LoginDateTime"].ToString();
            }

            var hdnTimeOffSetForLogout = hdnoffset;
            DateTime LogoutDateTime = DateTime.Now;
            string session = System.Web.HttpContext.Current.Session.SessionID;
            var SessionStatus = "";
            string query = "Select sessionid from tblUser where UserId='" + databaseCon.Decrypt(id) + "'";

            //this.Session["DemoEmail"]  = this.Session["DemoEmail"].ToString();
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                if (session == Convert.ToString(dt.Rows[0]["sessionid"]))
                {
                    // return SessionStatus = "Active";
                    SessionStatus = "Active";
                }
                else
                {
                    string queryCheckLogin = "Select sessionid,UTCLoginTime from TblAuditLogin where UserId='" + databaseCon.Decrypt(id) + "' and LoginAttemptSuccess = '1' order by AuditLoginId desc";
                    DataTable dtCheckLogin = databaseCon.GetData(queryCheckLogin);

                    if (dtCheckLogin.Rows.Count > 0)
                    {
                        if (LoginDateTime == Convert.ToString(dtCheckLogin.Rows[0]["UTCLoginTime"]))
                        {
                            string query1 = "update TblAuditLogin set UTCLogoutTime = '" + LogoutDateTime + "' ,LogoutTimeZone = '" + Convert.ToDouble((hdnTimeOffSetForLogout)) * (-1) + "'  where SessionId = '" + Convert.ToString(dt.Rows[0]["sessionid"]) + "' and UserId ='" + databaseCon.Decrypt(id) + "' and LoginAttemptSuccess = '1' and UTCLoginTime = '" + LoginDateTime + "'";

                            int result = databaseCon.RunCmd(query1);
                        }
                        else
                        {

                        }
                    }



                    //// return SessionStatus = "InActive";
                    SessionStatus = "InActive";
                    if (Session["Uid"] != null && Session["UserRole"] != null)
                    {
                        Session["Uid"] = null;
                        Session["UserRole"] = null;
                        Session["ClientID"] = null;
                        Session["EmailClientMsg"] = null;
                        Session["LoginDateTime"] = null;
                    }
                }
            }
            //return SessionStatus = "InActive";
            return Json(SessionStatus, JsonRequestBehavior.AllowGet);
        }

        public ActionResult btnSaveChangePassword_Click(string txtConfirmNewPassword, string txtNewPassword, string txtCurrentPassWord)
        {
            var Result = "";
            var Message = "";
            if (Session["UserEmail"] == null)
            {
                //Response.Redirect("~/Login.aspx");
                Result = "SessionOut";
                Message = "Session Out";
            }
            else
            {
                if (txtConfirmNewPassword == txtNewPassword)
                {
                    string query = "select EmailID,Password from tblUser where EmailID='" + Session["UserEmail"].ToString() + "' and Password='" + databaseCon.Encrypt(txtCurrentPassWord.Replace("'", "''").Trim()) + "'";
                    DataTable dt = databaseCon.GetData(query);
                    if (dt.Rows.Count <= 0)
                    {
                        //Show Error Msg--wrong password
                        //art1.Style.Add("display", "block");
                        //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                        //alertlabel.Text = "<strong> Info ! </strong> Current Password is not valid please enter valid Password.";
                        //ChangePasWorkModel.Style.Add("display", "block");
                        Result = "Failed";
                        Message = "Current Password is not valid please enter valid Password.";
                    }
                    else
                    {
                        if (txtNewPassword.Length > 15 || txtNewPassword.Length < 5)
                        {
                            //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                            //art1.Style.Add("display", "block");
                            //alertlabel.Text = "<strong>Alert ! </strong> Only 5 to 15 characters allowed for Password.";
                            Result = "Failed";
                            Message = "Only 5 to 15 characters allowed for Password.";
                        }
                        else
                        {
                            query = "update tblUser set Password='" + databaseCon.Encrypt(txtNewPassword.Replace("'", "'").Trim()) + "' where EmailID='" + Session["UserEmail"].ToString() + "'";
                            int Count = databaseCon.RunCmd(query);
                            if (Count > 0)
                            {
                                //art1.Style.Add("display", "block");
                                //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                                //alertlabel.Text = "<strong> Info ! </strong>Password Changed Successfully.";
                                //ChangePasWorkModel.Style.Add("display", "none");
                                Result = "Success";
                                Message = "Password Changed Successfully.";
                            }
                        }
                    }
                }
                else
                {
                    //Show Error Msg --confirm pass word does not match
                    //art1.Style.Add("display", "block");
                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                    //alertlabel.Text = "<strong> Info ! </strong>New Password is not match with Confirm Password.";
                    //ChangePasWorkModel.Style.Add("display", "block");
                    Result = "Failed";
                    Message = "New Password is not match with Confirm Password.";
                }
            }
            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DropdownBindClient(string UID, string UserRole)
        {
            var UserId = databaseCon.Decrypt(UID);
            var RoleNumber = databaseCon.Decrypt(UserRole);
            string query = "";
            object getData = "";

            if (Session["UserEmail"] != null)
            {
                var Fullname = "";
                string decryptedClientId = databaseCon.Decrypt(Session["ClientID"].ToString());

                if (RoleNumber == "80")
                {
                    query = "SELECT Firstname AS Fullname FROM tblUser WHERE ClientId = '" + decryptedClientId + "' AND EmailID = '" + Session["UserEmail"].ToString() + "'";

                    getData = dbContext.Database.SqlQuery<ClientViewModel>("select distinct(Name+' '+'('+tblclient.Status+')') as name,TblClient.ClientId ,UserId as Uid,Role as UserRole,FirstName as Fullname from tblclient left join tblUser on tblclient.ClientId = tblUser.ClientId  where tblUser.status = 'Active' and tblUser.EmailID='" + Session["UserEmail"].ToString() +
                        "' Order by name").ToList();
                }
                else
                {
                    query = "select Firstname as Fullname from tblUser where EmailID='" + Session["UserEmail"].ToString() + "'";
                    //string query = "SELECT Firstname AS Fullname FROM tblUser WHERE ClientId = '" + decryptedClientId + "' AND EmailID = '" + Session["UserEmail"].ToString() + "'";
                    //DataTable dt = databaseCon.GetData(query);


                    getData = dbContext.Database.SqlQuery<ClientViewModel>("select distinct(Name+' '+'('+status+')') as name,ClientId from tblclient where ClientId!='1' Order by name").ToList();

                }
                DataTable dt = databaseCon.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    //ViewBag.Fullname = dt.Rows[0]["Fullname"].ToString();
                    Fullname = dt.Rows[0]["Fullname"].ToString();

                }
                return Json(new { getData = getData, Fullname = Fullname }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // return RedirectToAction("Login", "Login");
                return Json(new { Result = "SessionOut", Message = "Session Out" }, JsonRequestBehavior.AllowGet);
            }


        }

        public ActionResult LogOutClick(string UID, string hdnoffset)
        {
            //var LoginDateTime = Session["LoginDateTime"].ToString(); 
            var LoginDateTime = "";
            if (Session["LoginDateTime"] == null)
            {
                LoginDateTime = "";
            }
            else
            {
                LoginDateTime = Session["LoginDateTime"].ToString();
            }

            DateTime LogoutDateTime = DateTime.Now;
            var hdnTimeOffSetForLogout = hdnoffset;
            string session = System.Web.HttpContext.Current.Session.SessionID;
            string query = "Select sessionid from tblUser where UserId='" + databaseCon.Decrypt(UID) + "'";
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                string SID = Convert.ToString(dt.Rows[0]["sessionid"]);
                string queryCheckLogin = "Select sessionid,UTCLoginTime from TblAuditLogin where UserId='" + databaseCon.Decrypt(UID) + "' and LoginAttemptSuccess = '1' order by AuditLoginId desc";
                DataTable dtCheckLogin = databaseCon.GetData(queryCheckLogin);

                if (dtCheckLogin.Rows.Count > 0)
                {
                    if (LoginDateTime == Convert.ToString(dtCheckLogin.Rows[0]["UTCLoginTime"]))
                    {
                        query = "update TblAuditLogin set UTCLogoutTime = '" + LogoutDateTime + "' ,LogoutTimeZone = '" + Convert.ToDouble((hdnTimeOffSetForLogout)) * (-1) + "'  where SessionId = '" + System.Web.HttpContext.Current.Session.SessionID + "' and UserId ='" + databaseCon.Decrypt(UID) + "' and LoginAttemptSuccess = '1' and UTCLoginTime = '" + LoginDateTime + "'";

                        int result = databaseCon.RunCmd(query);

                    }
                    else
                    {

                    }
                }

            }

            if (Session["Uid"] != null && Session["UserRole"] != null)
            {
                Session["Uid"] = null;
                Session["UserRole"] = null;
                Session["ClientID"] = null;
                Session["EmailClientMsg"] = null;
                Session["LoginDateTime"] = null;
            }
            return Json(new { Result = "SessionOut", Message = "Session Out" }, JsonRequestBehavior.AllowGet);
        }

        // public ActionResult ddlClient_SelectedIndexChanged(string ClientID)
        public ActionResult ddlClient_SelectedIndexChanged()
        {
            string ClientID = databaseCon.Decrypt(Request.QueryString["id"]);
            string UserRole = Request.QueryString["UserRole"];
            string UserId = Request.QueryString["Uid"];
            string userEmailId = Session["UserEmail"].ToString();
            DataTable dt;


            if (ClientID != "")
            {
                // Session["clientSelectId"] = ddlClient.SelectedValue;
                Session["clientSelectId"] = ClientID;
                Session["ClientID"] = databaseCon.Encrypt(ClientID);
            }
            else
            {
                Session["clientSelectId"] = null;
                Session["ClientID"] = null;
            }

            if (Convert.ToString((databaseCon.Decrypt(Request.QueryString["UserRole"]))) == "10")
            {
                // Response.Redirect("DashboardHere.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + HttpUtility.UrlEncode(databaseCon.Encrypt(ddlClient.SelectedValue)));

                return RedirectToAction("DashboardHere", "DashboardHere", new { UID = Request.QueryString["UID"], UserRole = Request.QueryString["UserRole"], id = databaseCon.Encrypt(ClientID) });
            }
            //else
            //{
            //    // Response.Redirect("RegisterList.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + HttpUtility.UrlEncode(databaseCon.Encrypt(ddlClient.SelectedValue)));
            //    var query = "select UserId,UserRole where emailId='" + userEmailId + "' and  ClientId='" + ClientID + "'";
            //    dt = databaseCon.GetData(query);

            //    return RedirectToAction("RegisterList", "RegisterList", new { UID = Request.QueryString["UID"], UserRole = Request.QueryString["UserRole"], id = databaseCon.Encrypt(ClientID)});
            //}
            else
            {
                var query = "select UserId,RoleNumber as UserRole from TblUser join TblUserRole on TblUser.Role = TblUserRole.RoleName where emailId='" + userEmailId + "' and  ClientId='" + ClientID + "'";

                //// Assuming databaseCon is an instance of your database connection class
                dt = databaseCon.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    // Retrieve UserId from the first row (assuming there's only one user for the given email and client)
                    UserId = dt.Rows[0]["UserId"].ToString();
                    UserRole = dt.Rows[0]["UserRole"].ToString();
                    UserRole = databaseCon.Encrypt(UserRole);
                    Session["Uid"] = UserId;
                    Session["UserRole"] = UserRole;
                }

                //return RedirectToAction("RegisterList", "RegisterList", new { UID = Request.QueryString["UID"], UserRole = Request.QueryString["UserRole"], id = databaseCon.Encrypt(ClientID) });
                return RedirectToAction("RegisterList", "RegisterList", new { UID = databaseCon.Encrypt(UserId), UserRole = UserRole, id = databaseCon.Encrypt(ClientID) });
            }





        }

        public ActionResult GoToJob(string id, string txtSacff, string UID)
        {
            var Result = "";
            var Message = "";
            string query;
            DataTable dt;
            string RegId = "";
            var ScafRef = txtSacff;
            var Location = "";
            var Site = "";
            var SiteOwnerName = "";
            var ScaffoldOwner = "";

            string query1 = "select Site, Location ,case when SiteOwnerName is null or SiteOwnerName='' then 'All' Else SiteOwnerName end as SiteOwnerName, case when ScaffoldOwner is null or ScaffoldOwner='' then 'All' Else ScaffoldOwner end  ScaffoldOwner from tblUser where ClientId='" + id + "' and UserId='" + databaseCon.Decrypt(UID) + "'";
            DataTable dt1 = databaseCon.GetData(query1);
            DataTable dt2;
            if (dt1.Rows.Count > 0)
            {
                Location = dt1.Rows[0]["Location"].ToString();
                Site = dt1.Rows[0]["Site"].ToString();
                SiteOwnerName = dt1.Rows[0]["Site"].ToString();
                ScaffoldOwner = dt1.Rows[0]["Site"].ToString();
            }

            if (!Site.IsNullOrWhiteSpace() && !Location.IsNullOrWhiteSpace())
            {
                string query2 = "select Scaffref ,RegisterId from tblregister  where SiteLocId  = " + Site + " and  LocationId  = " + Location + " ";
                dt2 = databaseCon.GetData(query2);

                DataRow[] dataRows = dt2.Select("Scaffref = '" + ScafRef + "'");
                if (dataRows.Length > 0)
                {
                    RegId = dataRows[0]["RegisterId"].ToString();
                    Result = "Success";
                    Message = RegId;
                }
                else
                {
                    Result = "Failed";
                    Message = "You do not have access to Scaf Ref " + ScafRef + ". Please contact administrator.";

                }
            }
            else
            {
                //query = "select ClientID,RegisterId,ScaffRef from tblRegister  where ClientId='" + ddlClient.SelectedValue + "' and  ScaffRef='" + ScafRef + "'";
                query = "select ClientID,RegisterId,ScaffRef from tblRegister  where ClientId='" + id + "' and  ScaffRef='" + ScafRef + "'";
                dt = databaseCon.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    RegId = dt.Rows[0]["registerid"].ToString();
                    // Response.Redirect("Register.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + databaseCon.Encrypt(ddlClient.SelectedValue) + "&Datetagged=" + Request.QueryString["Datetagged"] + "&RID=" + databaseCon.Encrypt(RegId), false);
                    Result = "Success";
                    Message = RegId;
                }
                else
                {
                    //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong> There is no Job with Scaf Ref " + ScafRef;
                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp", "<script>hideAlert();</script>", false);
                    Result = "Failed";
                    Message = "There is no Job with Scaf Ref " + ScafRef;
                }
            }


            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GoToInspection(string id, string txtSacff, string UID)
        {
            var Result = "";
            var Message = "";
            string query;
            DataTable dt;
            string RegId = "";
            var ScafRef = txtSacff;
            var Location = "";
            var Site = "";
            var SiteOwnerName = "";
            var ScaffoldOwner = "";

            string query1 = "select Site, Location ,case when SiteOwnerName is null or SiteOwnerName='' then 'All' Else SiteOwnerName end as SiteOwnerName, case when ScaffoldOwner is null or ScaffoldOwner='' then 'All' Else ScaffoldOwner end  ScaffoldOwner from tblUser where ClientId='" + id + "' and UserId='" + databaseCon.Decrypt(UID) + "'";
            DataTable dt1 = databaseCon.GetData(query1);
            DataTable dt2;
            if (dt1.Rows.Count > 0)
            {
                Location = dt1.Rows[0]["Location"].ToString();
                Site = dt1.Rows[0]["Site"].ToString();
                SiteOwnerName = dt1.Rows[0]["Site"].ToString();
                ScaffoldOwner = dt1.Rows[0]["Site"].ToString();
            }
            if (!Site.IsNullOrWhiteSpace() && !Location.IsNullOrWhiteSpace())
            {
                string query2 = "select Scaffref ,RegisterId from tblregister  where SiteLocId  = " + Site + " and  LocationId  = " + Location + " ";
                dt2 = databaseCon.GetData(query2);

                DataRow[] dataRows = dt2.Select("Scaffref = '" + ScafRef + "'");
                if (dataRows.Length > 0)
                {
                    RegId = dataRows[0]["RegisterId"].ToString();
                    Result = "Success";
                    Message = RegId;
                }
                else
                {
                    Result = "Failed";
                    Message = "You do not have access to Scaf Ref " + ScafRef + ". Please contact administrator.";

                }
            }
            else
            {
                // query = "select ClientID,RegisterId,ScaffRef from tblRegister  where ClientId='" + ddlClient.SelectedValue + "' and  ScaffRef='" + ScafRef + "'";
                query = "select ClientID,RegisterId,ScaffRef from tblRegister  where ClientId='" + id + "' and  ScaffRef='" + ScafRef + "'";
                dt = databaseCon.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    RegId = dt.Rows[0]["registerid"].ToString();
                    // ScafRef = Convert.ToInt32(dt.Rows[0]["ScaffRef"]);
                    //  Response.Redirect("InspectionList.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + databaseCon.Encrypt(ddlClient.SelectedValue) + "&Datetagged=" + Request.QueryString["Datetagged"] + "&RID=" + databaseCon.Encrypt(RegId), false);
                    Result = "Success";
                    Message = RegId;
                }
                else
                {
                    Result = "Failed";
                    Message = "There is no Job with Scaf Ref " + ScafRef;
                }
            }

            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Encrypt(string[] encrypt)
        {
            for (int i = 0; i < encrypt.Length; i++)
            {
                encrypt[i] = databaseCon.Encrypt(encrypt[i]);
            }

            return Json(encrypt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Decrypt(string[] decrypt)
        {
            for (int i = 0; i < decrypt.Length; i++)
            {
                decrypt[i] = databaseCon.Decrypt(decrypt[i]);
            }

            return Json(decrypt, JsonRequestBehavior.AllowGet);
        }
      

    }
}