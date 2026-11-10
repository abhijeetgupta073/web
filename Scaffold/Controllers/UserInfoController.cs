using Microsoft.Ajax.Utilities;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scaffold.Controllers
{
    public class UserInfoController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();

        // GET: UserInfo
        public ActionResult UserInfo()
        {

            string query1 = "select Country from tblClient where clientid='" + databaseCon.Decrypt(Request.QueryString["id"]) + "'";
            DataTable dt1 = databaseCon.GetData(query1);

            ViewBag.Country = Convert.ToString(dt1.Rows[0]["Country"]);
            return View();
        }

        public ActionResult ShowClientDetails(string id, string UserID)
        {
            string query;
            query = "exec Pro_TBLUSER_GET '" + databaseCon.Decrypt(id) + "', '" + databaseCon.Decrypt(UserID) + "'";
            var getClientUserdata = dbContext.Database.SqlQuery<Pro_TBLUSER_GETViewModel>("exec Pro_TBLUSER_GET '" + databaseCon.Decrypt(id) + "', '" + databaseCon.Decrypt(UserID) + "'").FirstOrDefault();
            getClientUserdata.Password = databaseCon.Decrypt(getClientUserdata.Password);
            var Country = getClientUserdata.Country;


            // if (dt.Rows[0]["Country"].ToString() == "")
            if (Country == "")
            {
                string queryclient = "select country from tblclient where clientid='" + databaseCon.Decrypt(id) + "'";
                DataTable dtclient = databaseCon.GetData(queryclient);
                //ddlCountry.SelectedValue = dtclient.Rows[0]["Country"].ToString();
                getClientUserdata.Country = dtclient.Rows[0]["Country"].ToString();

            }
            //else
            //{
            //    ddlCountry.SelectedValue = dt.Rows[0]["Country"].ToString();
            //}

            return Json(getClientUserdata, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ddlUserRole()
        {

            // For Inpex Only No need To show RoleName ='Client View' and RoleName ='Inspection Audit View' in Dropdown. Changes Date 19 Sep 2019

            // string query = "select RoleName from tbluserRole where RoleName !='SuperAdmin' and RoleName!='Client View' and RoleName!='Inspection Audit View' order by RoleName";

            // For Test And Inpex Only need To show RoleName ='Client View' and RoleName='Inspection Audit View'  in Dropdown. Changes Date 19 Sep 2019
            string query = "select RoleName from tbluserRole where RoleName !='SuperAdmin' order by RoleName";

            var getddlUserRole = dbContext.Database.SqlQuery<DropDownViewModel>(query).ToList();
            return Json(getddlUserRole, JsonRequestBehavior.AllowGet);
        }

        public ActionResult countryDropDown()
        {
            string query = "select distinct(Country) from tblState";
            var getddlCountry = dbContext.Database.SqlQuery<DropDownViewModel>(query).ToList();
            return Json(getddlCountry, JsonRequestBehavior.AllowGet);
            //DataTable dtCountry = databaseCon.GetData(query);
            //ddlCountry.DataValueField = "Country";
            //ddlCountry.DataTextField = "Country";
            //ddlCountry.DataSource = dtCountry;
            //ddlCountry.DataBind();
            //ddlCountry.Items.Insert(0, new ListItem("", ""));
        }

        //public ActionResult ddlState()
        //{
        //    string query;
        //    //if (!string.IsNullOrEmpty(ddlCountry.SelectedValue))
        //    //{
        //    //    query = "select distinct(State) as State from tblState where Country='" + ddlCountry.SelectedValue + "' order by State";
        //    //DataTable dtState = databaseCon.GetData(query);
        //    //ddlstate.DataValueField = "State";
        //    //ddlstate.DataTextField = "State";
        //    //ddlstate.DataSource = dtState;
        //    //ddlstate.DataBind();
        //    //ddlstate.Items.Insert(0, new ListItem("", ""));

        //    //}
        //    //else
        //    //{
        //    query = "select distinct (State) as state from tblState ";
        //    var getddlState = dbContext.Database.SqlQuery<DropDownViewModel>(query).ToList();
        //    return Json(getddlState, JsonRequestBehavior.AllowGet);
        //    //DataTable dtCountry = databaseCon.GetData(query);
        //    //ddlstate.DataValueField = "State";
        //    //ddlstate.DataTextField = "State";
        //    //ddlstate.DataSource = dtCountry;
        //    //ddlstate.DataBind();
        //    //ddlstate.Items.Insert(0, new ListItem("", ""));
        //    // }
        //}

        public ActionResult ddlState(string country)
        {
            string query = "";
            if (!string.IsNullOrEmpty(country))
            {
                query = "select distinct(State) as state from tblState where Country='"
                        + country + "' order by State";
            }
            else
            {
                query = "select distinct(State) as state from tblState order by State";
            }
            var getddlState = dbContext.Database.SqlQuery<DropDownViewModel>(query).ToList();
            return Json(getddlState, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ddlCategory(string id)
        {
            string query;
            //DataTable dt;
            query = "select UserCatId as CategoryId,Category from tblusercategory where ClientId = '" + databaseCon.Decrypt(id) + "' order by Category";
            var getddlCategory = dbContext.Database.SqlQuery<DropDownViewModel>(query).ToList();
            return Json(getddlCategory, JsonRequestBehavior.AllowGet);
            //dt = databaseCon.GetData(query);
            //ddlCategory.DataValueField = "CategoryId";
            //ddlCategory.DataTextField = "Category";
            //ddlCategory.DataSource = dt;
            ////ddlCategory.Items.Insert(0, new ListItem("", ""));
            //ddlCategory.DataBind();
        }

        public ActionResult dropdownOwner(string id)
        {
            ///*DropDown For Owner Name*/
            string query = "select Distinct OwnerName from tblSiteLocation where ClientId = '" + databaseCon.Decrypt(id) + "' and OwnerName!= '' and OwnerName is not null";
            var getddlOwner = dbContext.Database.SqlQuery<DropDownViewModel>(query).ToList();
            return Json(getddlOwner, JsonRequestBehavior.AllowGet);
            //DataTable dt = databaseCon.GetData(query);
            //ddlOwnerName.DataTextField = "OwnerName";
            //ddlOwnerName.DataValueField = "OwnerName";
            //ddlOwnerName.DataSource = dt;
            //ddlOwnerName.DataBind();
            //ddlOwnerName.Items.Insert(0, new ListItem("", ""));

        }

        public ActionResult dropdownSite(string id)
        {
            string query = "select SiteLocID, SiteLocation from tblSiteLocation where ClientId = '" + databaseCon.Decrypt(id) + "' and SiteLocation!= '' and SiteLocation is not null order by SiteLocation";
            var getddlOwner = dbContext.Database.SqlQuery<DropDownViewModel>(query).ToList();
            return Json(getddlOwner, JsonRequestBehavior.AllowGet);


        }

        public ActionResult ddlSubOwnerDropdown(string id, string ddlOwnerName)
        {
            string query = "";
            if (ddlOwnerName != "")
            {
                ddlOwnerName = ddlOwnerName.Replace("'", "''");
                query = "select Distinct ScaffoldOwner from Vw_tblRegister where ClientId = '" + databaseCon.Decrypt(id) + "' and ScaffoldOwner != '' and ScaffoldOwner is not null and OwnerName ='" + ddlOwnerName + "'";
            }
            else
            {
                query = "select Distinct ScaffoldOwner from Vw_tblRegister where ClientId = '" + databaseCon.Decrypt(id) + "' and ScaffoldOwner != '' and ScaffoldOwner is not null ";
            }
            var getddlSubOwner = dbContext.Database.SqlQuery<DropDownViewModel>(query).ToList();
            return Json(getddlSubOwner, JsonRequestBehavior.AllowGet);
            //DataTable dt = databaseCon.GetData(query);
            //ddlSubOwner.DataTextField = "ScaffoldOwner";
            //ddlSubOwner.DataValueField = "ScaffoldOwner";
            //ddlSubOwner.DataSource = dt;
            //ddlSubOwner.DataBind();
            //ddlSubOwner.Items.Insert(0, new ListItem("", ""));
        }

        public ActionResult ddlLocation(string id, string ddlSite)
        {
            string query = "";
            if (ddlSite != "")
            {
                query = "select AreaID,Area from tblArea where ClientId = '" + databaseCon.Decrypt(id) + "' and Area != '' and Area is not null and SiteLocId ='" + ddlSite + "' order by Area ";
            }
            else
            {
                query = "select AreaID, Area from tblArea where ClientId = '" + databaseCon.Decrypt(id) + "' and Area != '' and Area is not null  order  by Area";
            }
            var getddlSubOwner = dbContext.Database.SqlQuery<tblArea>(query).ToList();
            return Json(getddlSubOwner, JsonRequestBehavior.AllowGet);

        }

        public ActionResult btndeleteyes_Click(string UserID, string deleteimageupload)
        {
            string Result = "";
            string Message = "";
            string query;
            //string filePathupload = Server.MapPath("/images/" + ViewState["deleteimageupload"]);
            //if (System.IO.File.Exists(filePathupload))
            //{
            //    System.IO.File.Delete(filePathupload);
            //}
            //if (ViewState["deleteimageCapture"] != null && ViewState["deleteimageCapture"].ToString() != "")
            //{
            //    databaseCon.deletefileonblob(ViewState["deleteimageCapture"].ToString(), "siteimage");
            //}

            string filePathupload = Server.MapPath("~/images/UserLicencePic/" + deleteimageupload);
            if (System.IO.File.Exists(filePathupload))
            {
                System.IO.File.Delete(filePathupload);
            }
            if (deleteimageupload != null && deleteimageupload != "")
            {
                databaseCon.deletefileonblob(deleteimageupload, "siteimage");
            }

            query = "update tblUser set LicencePicUpload='', PhotoAngle='0' where UserId='" + databaseCon.Decrypt(UserID) + "'";
            int result = databaseCon.RunCmd(query);
            if (result > 0)
            {
                Result = "Success";
                Message = "Image delete Successfully";
                //Session["DeleteImagemsg"] = "DeleteImagemsg";
                //if (Request.QueryString["RID"] != null && Request.QueryString["RID"] != "")
                //{
                //    Response.Redirect("UserInfo.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&UserID=" + Request.QueryString["UserID"] + "&RID=" + Request.QueryString["RID"], false);
                //}
                //else
                //{
                //    Response.Redirect("UserInfo.aspx.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&UserID=" + Request.QueryString["UserID"], false);
                //}

            }
            var response = new { Result = Result, Message = Message };
            return Json(response, JsonRequestBehavior.AllowGet);
            // divdelete.Style.Add("display", "none");
        }


        public ActionResult btnSave_Click(string UserID, string firstName, string lastName, string userRole, string status, string mobileNo, string emailAddress, bool chkInspector, string licenceNo, string birthDate, string category, string password, string address, string city, string state, string ddlCountry, string postCode, string ownerName, string subOwner, string Site, string Location, string notes, string id, string UID, string ddlDirectIndirect, string imgangle, string image, string txtUK, string radUpdogImg, string hdnpassword, string txtIPAddress, string ChkIpAddress, string ChkEmailAuthantication)
        {

            // GetIPAddress();//Add code on 22/april/2020      
            //bool CheckLicenceNo = true;

            var IPaddress = Session["IPaddress"];

            string query;
            DataTable dt;
            string Message = "";
            string Result = "";
            string returnUserID = "";
            //  string imageData = tempimg.Value;
            // string UID = databaseCon.Decrypt(Request.QueryString["UID"]);
            if (hdnpassword == "")
            {
                hdnpassword = "scaffold123";
            }

            if (hdnpassword.Length > 15 || hdnpassword.Length < 5)
            {
                //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                //art1.Style.Add("display", "block");
                //alertlabel.Text = "<strong>Alert ! </strong> Only 5 to 15 characters allowed for Password.";
                Result = "Failed";
                Message = "Only 5 to 15 characters allowed for Password.";
            }
            else
            {
                // if ((chkInspector.Checked == true && txtlicenceNo.Text != "") || chkInspector.Checked == false)
                if ((chkInspector == true && licenceNo != "") || chkInspector == false)
                {

                    if (Convert.ToInt32(UserID) > 0)
                    {
                        if (ddlCountry == "Australia")
                        {

                            query = "exec pro_tbluser_save '" + UserID + "','" + firstName.Replace("'", "''").Trim() + "','" + lastName.Replace("'", "''").Trim() + "','" + address.Replace("'", "''").Trim() + "','" + city.Replace("'", "''").Trim() + "'," + (state == "" ? "null" : "'" + state + "'") + "," + (String.IsNullOrEmpty(postCode.Trim()) ? "null" : "'" + postCode.Trim() + "'") + "," + (String.IsNullOrEmpty(mobileNo.Trim()) ? "null" : "'" + mobileNo.Trim() + "'") + ",'" + emailAddress.Trim() + "','" + status + "','" + databaseCon.Decrypt(id) + "','" + userRole + "','" + emailAddress.Trim() + "','" + databaseCon.Encrypt(password.Replace("'", "''").Trim()) + "','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "','" + notes.Replace("'", "''").Trim() + "','" + licenceNo.Replace("'", "''").Trim() + "','" + chkInspector + "','" + birthDate + "','" + ddlDirectIndirect + "','" + category + "','" + ddlCountry + "','" + imgangle + "','" + ownerName.Replace("'", "''") + "','" + subOwner.Replace("'", "''") + "', " + (Site == "" ? "null" : "'" + Site + "'") + ", " + (Location == "" ? "null" : "'" + Location + "'") + ",'" + txtIPAddress + "','" + ChkIpAddress + "','" + ChkEmailAuthantication + "'";
                            dt = databaseCon.GetData(query);
                            if (dt.Rows.Count != 0)
                            {
                                int Results = Convert.ToInt32(dt.Rows[0][0]);
                                if (Results > 0)
                                {
                                    //Add code on 22/april/2020
                                    query = " insert into TblAuditUser (UserId,ClientID,Role,Status,IPAddress,AttemptSuccess,CreatedBy,CreatedOn,AtteptReason) values ('" + UserID + "'," + databaseCon.Decrypt(id) + ",'" + userRole + "','" + status + "','" + IPaddress + "'," + "1" + ",(select Emailid from tbluser where UserId='" + databaseCon.Decrypt(UID) + "'),'" + DateTime.Now + "','" + Convert.ToString(dt.Rows[0][1]) + "')";

                                    int result = databaseCon.RunCmd(query);
                                    //UpdateImagePath(Convert.ToInt32(dt.Rows[0][0]));
                                    //if (imageData != "")
                                    //{
                                    imageUpload(image, radUpdogImg, Convert.ToInt32(dt.Rows[0][0]), firstName);
                                    // }
                                    //Session["Insert"] = "Insert";
                                    //Session["Message"] = Convert.ToString(dt.Rows[0][1]);
                                    //Response.Redirect("UserInfo.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&UserID=" + Request.QueryString["UserID"], false);

                                    Result = "Success";
                                    Message = Convert.ToString(dt.Rows[0][1]);
                                    returnUserID = Convert.ToString(dt.Rows[0][0]);
                                }
                                else if (Results == -1)
                                {
                                    //Add code on 22/april/2020
                                    query = " insert into TblAuditUser (UserId,ClientID,Role,Status,IPAddress,AttemptSuccess,CreatedBy,CreatedOn,AtteptReason) values ('" + UserID + "'," + databaseCon.Decrypt(id) + ",'" + userRole + "','" + status + "','" + IPaddress + "'," + "0" + ",(select Emailid from tbluser where UserId='" + databaseCon.Decrypt(UID) + "'),'" + DateTime.Now + "','" + Convert.ToString(dt.Rows[0][1]) + "')";

                                    int result = databaseCon.RunCmd(query);
                                    //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                                    //art1.Style.Add("display", "block");
                                    //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                                    Result = "Failed";
                                    Message = dt.Rows[0][1].ToString();
                                }
                                else
                                {
                                    query = " insert into TblAuditUser (UserId,ClientID,Role,Status,IPAddress,AttemptSuccess,CreatedBy,CreatedOn,AtteptReason) values ('" + UserID + "'," + databaseCon.Decrypt(id) + ",'" + userRole + "','" + status + "','" + IPaddress + "'," + "0" + ",(select Emailid from tbluser where UserId='" + databaseCon.Decrypt(UID) + "'),'" + DateTime.Now + "','" + Convert.ToString(dt.Rows[0][1]) + "')";

                                    int result = databaseCon.RunCmd(query);
                                    //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                                    //art1.Style.Add("display", "block");
                                    //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                                    Result = "Failed";
                                    Message = dt.Rows[0][1].ToString();
                                }
                            }
                            else
                            {
                                //Add code on 22/april/2020
                                query = " insert into TblAuditUser (UserId,ClientID,Role,Status,IPAddress,AttemptSuccess,CreatedBy,CreatedOn,AtteptReason) values ('" + UserID + "'," + databaseCon.Decrypt(id) + ",'" + userRole + "','" + status + "','" + IPaddress + "'," + "0" + ",(select Emailid from tbluser where UserId='" + databaseCon.Decrypt(UID) + "'),'" + DateTime.Now + "','Syntax Error.Contact administrator.')";
                                int result = databaseCon.RunCmd(query);

                                //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                                //art1.Style.Add("display", "block");
                                //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator ";
                                Result = "Failed";
                                Message = "Syntax Error.Contact administrator ";
                            }
                        }

                        else if (ddlCountry == "United Kingdom")
                        {


                            query = "exec pro_tbluser_save '" + UserID + "','" + firstName.Replace("'", "''").Trim() + "','" + lastName.Replace("'", "''").Trim() + "','" + address.Replace("'", "''").Trim() + "','" + city.Replace("'", "''").Trim() + "'," + (state == "" ? "null" : "'" + state + "'") + "," + (String.IsNullOrEmpty(postCode.Trim()) ? "null" : "'" + postCode.Trim() + "'") + "," + (String.IsNullOrEmpty(txtUK.Trim()) ? "null" : "'" + txtUK.Trim() + "'") + ",'" + emailAddress.Trim() + "','" + status + "','" + databaseCon.Decrypt(id) + "','" + userRole + "','" + emailAddress.Trim() + "','" + databaseCon.Encrypt(password.Replace("'", "''").Trim()) + "','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "','" + notes.Replace("'", "''").Trim() + "','" + licenceNo.Replace("'", "''").Trim() + "','" + chkInspector + "','" + birthDate + "','" + ddlDirectIndirect + "','" + category + "','" + ddlCountry + "','" + imgangle + "','" + ownerName.Replace("'", "''") + "','" + subOwner.Replace("'", "''") + "', " + (Site == "" ? "null" : "'" + Site + "'") + ", " + (Location == "" ? "null" : "'" + Location + "'") + ",'" + txtIPAddress + "','" + ChkIpAddress + "','" + ChkEmailAuthantication + "'";


                            dt = databaseCon.GetData(query);
                            if (dt.Rows.Count != 0)
                            {
                                int Results = Convert.ToInt32(dt.Rows[0][0]);
                                returnUserID = Convert.ToString(dt.Rows[0][0]);
                                if (Results > 0)
                                {
                                    //Add code on 22/april/2020
                                    query = " insert into TblAuditUser (UserId,ClientID,Role,Status,IPAddress,AttemptSuccess,CreatedBy,CreatedOn,AtteptReason) values ('" + UserID + "'," + databaseCon.Decrypt(id) + ",'" + userRole + "','" + status + "','" + IPaddress + "'," + "1" + ",(select Emailid from tbluser where UserId='" + databaseCon.Decrypt(UID) + "'),'" + DateTime.Now + "','" + dt.Rows[0][1].ToString() + "')";

                                    int result = databaseCon.RunCmd(query);
                                    Result = "Success";
                                    Message = Convert.ToString(dt.Rows[0][1]);
                                    //UpdateImagePath(Convert.ToInt32(dt.Rows[0][0]));
                                    //if (imageData != "")
                                    //{
                                    imageUpload(image, radUpdogImg, Convert.ToInt32(dt.Rows[0][0]), firstName);
                                    //}
                                    Session["Insert"] = "Insert";
                                    //Session["Message"] = Convert.ToString(dt.Rows[0][1]);
                                    // Response.Redirect("UserInfo.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&UserID=" + Request.QueryString["UserID"], false);
                                }
                                else if (Results == -1)
                                {
                                    //Add code on 22/april/2020
                                    query = " insert into TblAuditUser (UserId,ClientID,Role,Status,IPAddress,AttemptSuccess,CreatedBy,CreatedOn,AtteptReason) values ('" + UserID + "'," + databaseCon.Decrypt(id) + ",'" + userRole + "','" + status + "','" + IPaddress + "'," + "0" + ",(select Emailid from tbluser where UserId='" + databaseCon.Decrypt(UID) + "'),'" + DateTime.Now + "','" + dt.Rows[0][1].ToString() + "')";

                                    int result = databaseCon.RunCmd(query);
                                    //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                                    //art1.Style.Add("display", "block");
                                    //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                                    Result = "Failed";
                                    Message = dt.Rows[0][1].ToString();
                                }
                                else
                                {
                                    query = " insert into TblAuditUser (UserId,ClientID,Role,Status,IPAddress,AttemptSuccess,CreatedBy,CreatedOn,AtteptReason) values ('" + UserID + "'," + databaseCon.Decrypt(id) + ",'" + userRole + "','" + status + "','" + IPaddress + "'," + "0" + ",(select Emailid from tbluser where UserId='" + databaseCon.Decrypt(UID) + "'),'" + DateTime.Now + "','" + dt.Rows[0][1].ToString() + "')";

                                    int result = databaseCon.RunCmd(query);
                                    //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                                    //art1.Style.Add("display", "block");
                                    //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                                    Result = "Failed";
                                    Message = dt.Rows[0][1].ToString();
                                    returnUserID = dt.Rows[0][0].ToString();
                                }
                            }
                            else
                            {
                                //Add code on 22/april/2020
                                query = " insert into TblAuditUser (UserId,ClientID,Role,Status,IPAddress,AttemptSuccess,CreatedBy,CreatedOn,AtteptReason) values ('" + UserID + "'," + databaseCon.Decrypt(id) + ",'" + userRole + "','" + status + "','" + IPaddress + "'," + "0" + ",(select Emailid from tbluser where UserId='" + databaseCon.Decrypt(UID) + "'),'" + DateTime.Now + "','Syntax Error. Contact administrator.')";
                                int results = databaseCon.RunCmd(query);

                                //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                                //art1.Style.Add("display", "block");
                                //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator ";
                                Result = "Failed";
                                Message = "Syntax Error. Contact administrator ";
                            }
                        }
                    }
                    else
                    {
                        //if (txtZip.Text != "")
                        //{
                        //    postcode = txtZip.Text;
                        //}
                        //else if (txtpostcodeuk.Text != "")
                        //{
                        //    postcode = txtpostcodeuk.Text;
                        //}

                        //if (txtContact.Text != "")
                        //{
                        //    cellphone = txtContact.Text;
                        //}
                        //else if (txtUK.Text != "")
                        //{
                        //    cellphone = txtUK.Text;
                        //}


                        //query = "exec pro_tbluser_save '" + databaseCon.Decrypt(UserID) + "','" + firstName.Replace("'", "''").Trim() + "','" + lastName.Replace("'", "''").Trim() + "','" + address.Replace("'", "''").Trim() + "','" + city.Replace("'", "''").Trim() + "'," + (state == "" ? "null" : "'" + state + "'") + ",'" + postCode + "','" + mobileNo + "','" + emailAddress.Trim() + "','" + status + "','" + databaseCon.Decrypt(id) + "','" + userRole + "','" + emailAddress.Trim() + "','" + databaseCon.Encrypt(password.Replace("'", "''").Trim()) + "','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "','" + notes.Replace("'", "''").Trim() + "','" + licenceNo.Replace("'", "''").Trim() + "','" + chkInspector + "','" + birthDate + "','" + ddlDirectIndirect + "','" + category + "','" + ddlCountry + "','" + imgangle + "','" + ownerName.Replace("'", "''") + "','" + subOwner.Replace("'", "''") + "'";

                        query = "exec pro_tbluser_save '" + UserID + "','" + firstName.Replace("'", "''").Trim() + "','" + lastName.Replace("'", "''").Trim() + "','" + address.Replace("'", "''").Trim() + "','" + city.Replace("'", "''").Trim() + "'," + (state == "" ? "null" : "'" + state + "'") + ",'" + postCode + "','" + mobileNo + "','" + emailAddress.Trim() + "','" + status + "','" + databaseCon.Decrypt(id) + "','" + userRole + "','" + emailAddress.Trim() + "','" + databaseCon.Encrypt(password.Replace("'", "''").Trim()) + "','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "','" + notes.Replace("'", "''").Trim() + "','" + licenceNo.Replace("'", "''").Trim() + "','" + chkInspector + "','" + birthDate + "','" + ddlDirectIndirect + "','" + category + "','" + ddlCountry + "','" + imgangle + "','" + ownerName.Replace("'", "''") + "','" + subOwner.Replace("'", "''") + "', " + (Site == "" ? "null" : "'" + Site + "'") + ", " + (Location == "" ? "null" : "'" + Location + "'") + ",'" + txtIPAddress + "','" + ChkIpAddress + "','" + ChkEmailAuthantication + "'";

                        // query = "";
                        dt = databaseCon.GetData(query);
                        if (dt.Rows.Count != 0)
                        {
                            int Results = Convert.ToInt32(dt.Rows[0][0]);
                            returnUserID = Convert.ToString(dt.Rows[0][0]);
                            if (Results > 0)
                            {
                                //Session["Insert"] = "Insert";
                                //Session["Message"] = Convert.ToString(dt.Rows[0][1]);
                                Result = "Success";
                                Message = Convert.ToString(dt.Rows[0][1]);
                                if (Request.QueryString["RID"] != null || Request.QueryString["from"] != null)
                                {
                                    //Add code on 22/april/2020
                                    query = " insert into TblAuditUser (UserId,ClientID,Role,Status,IPAddress,AttemptSuccess,CreatedBy,CreatedOn,AtteptReason) values ('" + UserID + "'," + databaseCon.Decrypt(id) + ",'" + userRole + "','" + status + "','" + IPaddress + "'," + "1" + ",(select Emailid from tbluser where UserId='" + databaseCon.Decrypt(UID) + "'),'" + DateTime.Now + "','" + dt.Rows[0][1].ToString() + "')";

                                    int result = databaseCon.RunCmd(query);
                                    //UpdateImagePath(Convert.ToInt32(dt.Rows[0][0]));
                                    //if (imageData != "")
                                    //{
                                    imageUpload(image, radUpdogImg, Convert.ToInt32(dt.Rows[0][0]), firstName);
                                    //}
                                    // Response.Redirect("UserInfo.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&UserID=" + databaseCon.Encrypt(dt.Rows[0][0].ToString()) + "&RID=" + Request.QueryString["RID"] + "&from=" + Request.QueryString["from"], false);

                                }
                                else
                                {
                                    //UpdateImagePath(Convert.ToInt32(dt.Rows[0][0]));
                                    //if (imageData != "")
                                    //{
                                    imageUpload(image, radUpdogImg, Convert.ToInt32(dt.Rows[0][0]), firstName);
                                    //}
                                    // Response.Redirect("UserInfo.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&UserID=" + databaseCon.Encrypt(Convert.ToInt32(dt.Rows[0][0]).ToString()), false);
                                }
                            }
                            else if (Results == -1)
                            {
                                //Add code on 22/april/2020
                                query = " insert into TblAuditUser (UserId,ClientID,Role,Status,IPAddress,AttemptSuccess,CreatedBy,CreatedOn,AtteptReason) values ('" + UserID + "'," + databaseCon.Decrypt(id) + ",'" + userRole + "','" + status + "','" + IPaddress + "'," + "0" + ",(select Emailid from tbluser where UserId='" + databaseCon.Decrypt(UID) + "'),'" + DateTime.Now + "','" + dt.Rows[0][1].ToString() + "')";

                                int result = databaseCon.RunCmd(query);
                                //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                                //art1.Style.Add("display", "block");
                                //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                                Result = "Failed";
                                Message = dt.Rows[0][1].ToString();
                            }
                            else
                            {
                                query = " insert into TblAuditUser (UserId,ClientID,Role,Status,IPAddress,AttemptSuccess,CreatedBy,CreatedOn,AtteptReason) values ('" + UserID + "'," + databaseCon.Decrypt(id) + ",'" + userRole + "','" + status + "','" + IPaddress + "'," + "0" + ",(select Emailid from tbluser where UserId='" + databaseCon.Decrypt(UID) + "'),'" + DateTime.Now + "','" + dt.Rows[0][1].ToString() + "')";
                                int result = databaseCon.RunCmd(query);
                                //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                                //art1.Style.Add("display", "block");
                                //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                                Result = "Failed";
                                Message = dt.Rows[0][1].ToString();
                            }
                        }
                        else
                        {
                            //Add code on 22/april/2020
                            query = " insert into TblAuditUser (UserId,ClientID,Role,Status,IPAddress,AttemptSuccess,CreatedBy,CreatedOn,AtteptReason) values ('" + UserID + "'," + databaseCon.Decrypt(id) + ",'" + userRole + "','" + status + "','" + IPaddress + "'," + "0" + ",(select Emailid from tbluser where UserId='" + databaseCon.Decrypt(UID) + "'),'" + DateTime.Now + "','Syntax Error. Contact administrator.')";
                            int result = databaseCon.RunCmd(query);

                            //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                            //art1.Style.Add("display", "block");
                            //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator ";
                            Result = "Failed";
                            Message = "Syntax Error. Contact administrator ";
                        }
                    }
                }
                else
                {
                    query = " insert into TblAuditUser (UserId,ClientID,Role,Status,IPAddress,AttemptSuccess,CreatedBy,CreatedOn,AtteptReason) values ('" + UserID + "'," + databaseCon.Decrypt(id) + ",'" + userRole + "','" + status + "','" + IPaddress + "'," + "0" + ",(select Emailid from tbluser where UserId='" + databaseCon.Decrypt(UID) + "'),'" + DateTime.Now + "','Licence No. must be entered for Inspectors.')";
                    int result = databaseCon.RunCmd(query);
                    //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong> Licence No. must be entered for Inspectors.";
                    Result = "Failed";
                    Message = "Licence No. must be entered for Inspectors.";
                }
            }
            var response = new { Result = Result, Message = Message, returnUserID = databaseCon.Encrypt(returnUserID) };
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        public void imageUpload(string imageData, string radUpdogImg, int UserId, string firstName)
        {
            //if (radUpdogImg != null)
            //{
            if (UserId > 0)
            {
                //string ImageData = imgload.Value;
                string extension = "";
                string FileName = "";
                string path = "/images/UserLicencePic/";
                path = Server.MapPath(path);
                string newpath2 = "";
                string QueryU = "update tblUser set";
                string FileExtension = "";
                //if (radUpdogImg.UploadedFiles.Count > 0)
                //{
                if (radUpdogImg != "")
                {
                    //string dotscount = hdfield.Value;
                    extension = System.IO.Path.GetExtension(radUpdogImg).ToLower();
                    if (extension != "")
                    {
                        var ext = FileExtension.Split('.');
                        //var extension = ext[1];
                        if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png" || extension.ToLower() == ".ico" || extension.ToLower() == ".gif" || extension.ToLower() == ".bmp")
                        {
                            //HttpFileCollectionBase PostedFiles = Request.Files;

                            HttpFileCollectionBase files = Request.Files;
                            //foreach (HttpPostedFile postedFile in FileUpload1.PostedFiles)
                            //{
                            for (int k = 0; k < 1; k++)
                            {

                                HttpPostedFileBase postedFile = files[k];

                                //string fileName = Path.GetFileName(postedFile.FileName);
                                FileName = firstName.Replace("'", "''").Replace(" ", "").Trim() + "_" + UserId + "_" + DateTime.Now.Ticks + extension;
                                newpath2 = path + FileName;

                                //byte[] bytess;
                                //if (imageData != "")
                                //{
                                //    bytess = Convert.FromBase64String(imageData.Split(',')[1]);
                                //}
                                //else
                                //{
                                //    bytess = Convert.FromBase64String(imageData.Split(',')[1]);
                                //}

                                if (FileName != "")
                                {
                                    string Filenameget = FileName;
                                    string filenamepath = path + Filenameget;


                                    System.IO.File.Delete(filenamepath);
                                    postedFile.SaveAs(Server.MapPath("~/images/UserLicencePic") + @"/" + FileName);
                                }
                                else
                                {
                                    if (!Directory.Exists(path))
                                    {
                                        Directory.CreateDirectory(path);
                                    }
                                    //radUpdogImg.UploadedFiles[0].SaveAs(newpath2);
                                    //postedFile.SaveAs(Server.MapPath("~/images/sites") + @"/" + newpath2);
                                    postedFile.SaveAs(Server.MapPath("~/images/sites") + @"/" + FileName);
                                }


                                QueryU += " LicencePicUpload='" + FileName + "' where UserId=" + UserId;
                                int count = databaseCon.RunCmd(QueryU);
                                string fpath = path + FileName;
                            }
                            // ShowClientDetails();
                        }
                    }
                }
                else if (imageData != "" || imageData != "")
                {

                    FileName = firstName.Replace("'", "''").Replace(" ", "").Trim() + "_" + UserId + "_" + DateTime.Now.Ticks + ".png";
                    string SketchnewPath = path + FileName;
                    byte[] bytes;
                    if (imageData != "")
                    {
                        bytes = Convert.FromBase64String(imageData.Split(',')[1]);
                    }
                    else
                    {
                        bytes = Convert.FromBase64String(imageData.Split(',')[1]);
                    }
                    //ShowClientDetails();
                    if (FileName != "")
                    {
                        string Filenamegetcap = FileName;
                        string SketchnewPath1 = path + Filenamegetcap;
                        System.IO.File.Delete(SketchnewPath1);
                        //string fpath = path + FileName;

                    }
                    using (var imageFile = new FileStream(SketchnewPath, FileMode.Create))
                    {
                        imageFile.Write(bytes, 0, bytes.Length);
                        imageFile.Flush();
                    }
                    QueryU += " LicencePicUpload='" + FileName + "' where UserId=" + UserId;
                    int count = databaseCon.RunCmd(QueryU);
                }
            }
            //Photoblock.Style.Add("display", "none");
            //}
        }
        public class tblArea
        {
            public int AreaID { get; set; }
            public string Area { get; set; }

        }
    }
}