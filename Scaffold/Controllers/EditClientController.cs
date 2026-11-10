using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;

namespace Scaffold.Controllers
{
    public class EditClientController : Controller
    {
        ScaffoldEntities dbcontext = new ScaffoldEntities();
        // GET: EditClient
        public ActionResult EditClient(string AddSite, string AddLocation, string AddBuilding, string AddAsset)
        {

            ViewBag.AddSite = AddSite;
            ViewBag.AddLocation = AddLocation;
            ViewBag.AddBuilding = AddBuilding;
            ViewBag.AddAsset = AddAsset;

            // Check Hire Inventory Checkbox Add on 13 March 2020
            var InventoryChecked = "";
            string HireInventoryquery = "select COUNT(*) as HireInventory from tblInventory where ClientId = '" + HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["id"])) + "'";
            DataTable dt = databaseCon.GetData(HireInventoryquery);
            if (dt.Rows.Count > 0)
            {
                InventoryChecked = dt.Rows[0]["HireInventory"].ToString();
                ViewBag.InventoryChecked = dt.Rows[0]["HireInventory"].ToString();
            }

            string qry = "select isnull(HireInventory,0) as HireInventory from tblClient where ClientId = '" + HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["id"])) + "'";
            DataTable cdt = databaseCon.GetData(qry);
            if (cdt.Rows.Count > 0)
            {
                ViewBag.HireInventory = cdt.Rows[0]["HireInventory"].ToString();
                //if (cdt.Rows[0]["HireInventory"].ToString() == "True")
                //{
                //    if (InventoryChecked != "0")
                //    {
                //       // chkHireInventory.Enabled = false;
                //       // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "HireInventory", "<script>HireInventoryMessage();</script>", false);

                //    }
                //}
                //else
                //{
                //    if (InventoryChecked != "0")
                //    {
                //       // chkHireInventory.Enabled = false;
                //       // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "HireInventory1", "<script>HireInventoryOldDataMessage();</script>", false);
                //    }
                //}
            }
            // Man Hours Management Disable If Any Site Chaked Man Hours CheckBox.

            string mainHourQry = "select Count(ManHoursManagement) as ManHoursManagement  from tblSiteLocation where  ClientId = '" + HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["id"])) + "' and ManHoursManagement='1'";
            DataTable mainHourdt = databaseCon.GetData(mainHourQry);
            if (mainHourdt.Rows[0]["ManHoursManagement"].ToString() != "0")
            {
                // checkboxManHoursManagement.Enabled = false;
                ViewBag.checkboxManHoursManagement = mainHourdt.Rows[0]["ManHoursManagement"].ToString();
            }

            string querymanhours = "select isnull(ManHoursManagement,0) as ManHoursManagement  from tblClient where ClientId = '" + HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["id"])) + "'";
            DataTable dataTable = databaseCon.GetData(querymanhours);

            if (dataTable.Rows.Count > 0)
            {
                if (dataTable.Rows[0]["ManHoursManagement"].ToString() == "False")
                {
                    // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ManHourCheckBoxHide", "<script>hideManHour();</script>", false);
                    ViewBag.ManHoursManagement = false;
                }
                else
                {
                    // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ManHourCheckBoxHideByDefault", "<script> ByDefaultClient();</script>", false);
                    ViewBag.ManHoursManagement = true;

                }

            }

            // Line Add On 06 Jan 2020 if No Any Site On FloorMap Checked Then Add Floor Image Button Hide otherwise show. 
            DataTable dtFloorMap = databaseCon.GetData("select Count(*) as FloorLevel from tblSiteLocation where FloorLevel=1 and clientid='" + HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["id"])) + "'");

            if (dtFloorMap.Rows[0]["FloorLevel"].ToString() == "0")
            {
                //btnFloorLevelImage.Visible = false;
                ViewBag.btnFloorLevelImage = dtFloorMap.Rows[0]["FloorLevel"].ToString();
            }
            string query = "select PlannedStandingDuration from tblRegister where clientid='" + HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["id"])) + "'";
            DataTable dtstanding = databaseCon.GetData(query);
            if (dtstanding.Rows.Count > 0)
            {
                // txtplnstandDur.Enabled = false;
                ViewBag.PlannedStandingDuration = true;
            }


            return View();
        }

        public ActionResult BindallDropdown(string id)
        {

            var StateDropdown = dbcontext.Database.SqlQuery<DropDownViewModel>("select distinct(state) from tblstate where Country=(select Country From tblClient where ClientId=" + databaseCon.Decrypt(id) + ") order by state").ToList();

            var CountryDropdown = dbcontext.Database.SqlQuery<DropDownViewModel>("select distinct(Country) from tblstate");

            return Json(new { StateDropdown = StateDropdown, CountryDropdown = CountryDropdown }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ShowClientDetails(string id)
        {
            //string query;
            //query = "exec Pro_TBLCLIENT_GET " + databaseCon.Decrypt(id);
            //DataTable dt = databaseCon.GetData(query);

            var GetData = dbcontext.Database.SqlQuery<Pro_TBLCLIENT_GET_ViewModel>("exec Pro_TBLCLIENT_GET " + databaseCon.Decrypt(id)).FirstOrDefault();
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult savepopAddnewcompany_Click(string UID, string txtCompanyName, string ddlpopCountry, string ddlweekstartday)
        {
            var Result = "";
            var Message = "";
            var ClientID = "";
            var chkIPAuthentication = false;
            //  string query = "exec Pro_TBLCLIENT_SAVE 0,'" + txtCompanyName.Text.Replace("'", "''").Trim() + "','','','','','','','" + HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["UID"])) + "','" + DateTime.Now + "','Active','','8.00','" + ddlpopCountry.SelectedValue + "','Plan1','" + ddlweekstartday.SelectedValue + "','30','0.4','" + chkIPAuthentication.Checked + "','False','False'";

            // By Change On 05 Sep 2019 Change Plain="Plan1"  To  Plain="Plan2"
            //  string query = "exec Pro_TBLCLIENT_SAVE 0,'" + txtCompanyName.Text.Replace("'", "''").Trim() + "','','','','','','','" + HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["UID"])) + "','" + DateTime.Now + "','Active','','8.00','" + ddlpopCountry.SelectedValue + "','Plan2','" + ddlweekstartday.SelectedValue + "','30','0.4','" + chkIPAuthentication + "','False','False','False'";

            //string query = "exec Pro_TBLCLIENT_SAVE 0,'" + txtCompanyName.Replace("'", "''").Trim() + "','','','','','','','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "','Active','','8.00','" + ddlpopCountry + "','Plan2','" + ddlweekstartday + "','30','0.4','" + chkIPAuthentication + "','False','False','False','False','False',''";//Commented on 08-june-2021
            string query = "exec Pro_TBLCLIENT_SAVE 0,'" + txtCompanyName.Replace("'", "''").Trim() + "','','','','','','','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "','Active','','8.00','" + ddlpopCountry + "','Plan2','" + ddlweekstartday + "','30','0.4','" + chkIPAuthentication + "','False','False','False','False','False','','false','false','false','false'";// Added on 08-June-2021

            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count != 0)
            {
                if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                {
                    int id = Convert.ToInt32(dt.Rows[0][0]);
                    //Session["Insert"] = "Insert";
                    //Session["Message"] = Convert.ToString(dt.Rows[0][1]);
                    //Response.Redirect("EditClient.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + HttpUtility.UrlEncode(databaseCon.Encrypt(id.ToString())));
                    Result = "Success";
                    Message = dt.Rows[0][1].ToString();
                    ClientID = databaseCon.Encrypt(id.ToString());
                }
                else
                {
                    //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                    Result = "Failed";
                    Message = dt.Rows[0][1].ToString();
                }
            }
            else
            {
                //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                //art1.Style.Add("display", "block");
                //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator ";

                Result = "Failed";
                Message = "Syntax Error. Contact administrator ";
            }
            return Json(new { Result = Result, Message = Message, ClientID = ClientID }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult checkActualPlanningWeek(string id)
        {
            id = databaseCon.Decrypt(id);
            string checkActualPlanningWeekQuery = "select ClientId from tblYearWeekDate2024 where ClientId = '" + id + "'";
            var check = databaseCon.GetData(checkActualPlanningWeekQuery);
            string result = "Success";
            string message = "Week Start Day is changed.";
            if (check != null && check.Rows.Count > 0)
            {
                result = "Failed";
                message = "You have record for existing Week Start Day, first delete from Actual Planning week then change Week Start Day.";
            }
            return Json(new { Result = result, Message = message });
        }

        public ActionResult btnSave_Click(string ClientID, string UID, string plan, string ddlCountry, string txtclientName, string txtAddress, string txtcity, string ddlstate, string txtzip, string txtpostcodeuk, string txtUK, string txtContact, string txtemail, string ddlClientstatus, string ddlInspectionTimeFrame, string txtStandardWorkingHrs, string ddlweekstartday, string txtplnstandDur, string txtManHrsRatio, string chkIPAuthentication, string checkboxManHoursManagement, string isMainClient, string checkOfflineEnabled, string chkHireInventory, string ddlLongTermStructure, string txtClientInventoryMailingList, string chkComparisonDashboard, string chkOfflineInspection, string chkNOGenerate, string chkMainDepotInventory)
        {
            var ResultMsg = "";
            var Message = "";
            var EncriptClientID = "";
            DataTable clientdt;
            string query;

            // if (Convert.ToInt32(HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["id"]))) > 0)
            if (Convert.ToInt32(databaseCon.Decrypt(ClientID)) > 0)
            {

                // if ((isMainClient.Checked == true && lstSubClient.SelectedValue != "") || (isMainClient.Checked != true && lstSubClient.SelectedValue == ""))
                //    {
                //string deletequery = "delete from TblMainClientToClient where MainClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "'";
                //int deleteID = databaseCon.RunCmd(deletequery);
                //foreach (ListItem s in lstSubClient.Items)
                //{
                //    if (s.Selected)
                //    {
                //        string SubClientquery = "insert into TblMainClientToClient (MainClientId,SubClientId,CreatedBy,CreatedOn)values('" + databaseCon.Decrypt(Request.QueryString["id"]) + "'," + s.Value + ",(select EmailId from tblUser where UserId='" + databaseCon.Decrypt(Request.QueryString["UID"]) + "'),'" + DateTime.Now + "')";
                //        int id = databaseCon.RunCmdID(SubClientquery);
                //    }
                //}

                var FlagUpdateUserStatus = false;

                //Step 1 Select Status Of This Client
                var GetData = dbcontext.Database.SqlQuery<Pro_TBLCLIENT_GET_ViewModel>("select Status from tblClient where clientid='" + databaseCon.Decrypt(ClientID) + "'").FirstOrDefault();

                // string Query = "select Status from tblClient where clientid='" + databaseCon.Decrypt(ClientID) + "'";
                string chkStatus = GetData.Status;

                // step 2-- Chek if Status Return from Step 1 is != to Status on the text box
                if (chkStatus != ddlClientstatus)
                {
                    FlagUpdateUserStatus = true;
                }

                if (ddlCountry == "Australia")
                {
                    //string queryaus = "exec Pro_TBLCLIENT_SAVE '" + HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["id"])) + "','" + txtclientName.Text.Replace("'", "''").Trim() + "','" + txtAddress.Text.Replace("'", "''").Trim() + "','" + txtcity.Text.Replace("'", "''").Trim() + "','" + ddlstate.SelectedValue + "','" + txtzip.Text.Replace("'", "''").Trim() + "'," + (string.IsNullOrEmpty(txtContact.Text.Trim()) ? "null" : "'" + txtContact.Text.Trim() + "'") + ",'" + txtemail.Text.Trim() + "','" + HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["UID"])) + "','" + DateTime.Now + "','" + ddlClientstatus.SelectedValue + "','" + ddlInspectionTimeFrame.SelectedValue + "'," + (txtStandardWorkingHrs.Text == "" ? "0" : "'" + txtStandardWorkingHrs.Text + "'") + ",'" + ddlCountry.SelectedValue + "','" + plan.ToString() + "','" + ddlweekstartday.SelectedValue + "'," + (txtplnstandDur.Text == "" ? "null" : "'" + txtplnstandDur.Text + "'") + "," + (txtManHrsRatio.Text == "" ? "null" : "'" + txtManHrsRatio.Text + "'") + ",'" + chkIPAuthentication.Checked + "','" + checkboxManHoursManagement.Checked + "','" + isMainClient.Checked + "','" + checkOfflineEnabled.Checked + "'";
                    //Commented on 07-june-2021
                    //string queryaus = "exec Pro_TBLCLIENT_SAVE '" + databaseCon.Decrypt(ClientID) + "','" + txtclientName.Replace("'", "''").Trim() + "','" + txtAddress.Replace("'", "''").Trim() + "','" + txtcity.Replace("'", "''").Trim() + "','" + ddlstate + "','" + txtzip.Replace("'", "''").Trim() + "'," + (string.IsNullOrEmpty(txtContact.Trim()) ? "null" : "'" + txtContact.Trim() + "'") + ",'" + txtemail.Trim() + "','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "','" + ddlClientstatus + "','" + ddlInspectionTimeFrame + "'," + (txtStandardWorkingHrs == "" ? "0" : "'" + txtStandardWorkingHrs + "'") + ",'" + ddlCountry + "','" + plan.ToString() + "','" + ddlweekstartday + "'," + (txtplnstandDur == "" ? "null" : "'" + txtplnstandDur + "'") + "," + (txtManHrsRatio == "" ? "null" : "'" + txtManHrsRatio + "'") + ",'" + chkIPAuthentication + "','" + checkboxManHoursManagement + "','" + isMainClient + "','" + checkOfflineEnabled + "','"+ chkHireInventory + "','" + ddlLongTermStructure + "','" + txtClientInventoryMailingList + "'";
                    //added on 07-June-2021
                    //string queryaus = "exec Pro_TBLCLIENT_SAVE '" + databaseCon.Decrypt(ClientID) + "','" + txtclientName.Replace("'", "''").Trim() + "','" + txtAddress.Replace("'", "''").Trim() + "','" + txtcity.Replace("'", "''").Trim() + "','" + ddlstate + "','" + txtzip.Replace("'", "''").Trim() + "'," + (string.IsNullOrEmpty(txtContact.Trim()) ? "null" : "'" + txtContact.Trim() + "'") + ",'" + txtemail.Trim() + "','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "','" + ddlClientstatus + "','" + ddlInspectionTimeFrame + "'," + (txtStandardWorkingHrs == "" ? "0" : "'" + txtStandardWorkingHrs + "'") + ",'" + ddlCountry + "','" + plan.ToString() + "','" + ddlweekstartday + "'," + (txtplnstandDur == "" ? "null" : "'" + txtplnstandDur + "'") + "," + (txtManHrsRatio == "" ? "null" : "'" + txtManHrsRatio + "'") + ",'" + chkIPAuthentication + "','" + checkboxManHoursManagement + "','" + isMainClient + "','" + checkOfflineEnabled + "','" + chkHireInventory + "','" + ddlLongTermStructure + "','" + txtClientInventoryMailingList + "','" + chkComparisonDashboard + "'";
                    string queryaus = "exec Pro_TBLCLIENT_SAVE '" + databaseCon.Decrypt(ClientID) + "','" + txtclientName.Replace("'", "''").Trim() + "','" + txtAddress.Replace("'", "''").Trim() + "','" + txtcity.Replace("'", "''").Trim() + "','" + ddlstate + "','" + txtzip.Replace("'", "''").Trim() + "'," + (string.IsNullOrEmpty(txtContact.Trim()) ? "null" : "'" + txtContact.Trim() + "'") + ",'" + txtemail.Trim() + "','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "','" + ddlClientstatus + "','" + ddlInspectionTimeFrame + "'," + (txtStandardWorkingHrs == "" ? "0" : "'" + txtStandardWorkingHrs + "'") + ",'" + ddlCountry + "','" + plan.ToString() + "','" + ddlweekstartday + "'," + (txtplnstandDur == "" ? "null" : "'" + txtplnstandDur + "'") + "," + (txtManHrsRatio == "" ? "null" : "'" + txtManHrsRatio + "'") + ",'" + chkIPAuthentication + "','" + checkboxManHoursManagement + "','" + isMainClient + "','" + checkOfflineEnabled + "','" + chkHireInventory + "','" + ddlLongTermStructure + "','" + txtClientInventoryMailingList + "','" + chkComparisonDashboard + "','" + chkOfflineInspection + "','" + chkNOGenerate + "','" + chkMainDepotInventory + "'";
                    clientdt = databaseCon.GetData(queryaus);
                    if (clientdt.Rows.Count != 0)
                    {
                        if (Convert.ToInt32(clientdt.Rows[0][0]) > 0)
                        {
                            int id = Convert.ToInt32(clientdt.Rows[0][0]);
                            //UpdateImagePath(id);
                            //ShowClientDetails();
                            //if (ddlClientstatus.SelectedValue == "InActive")

                            if (FlagUpdateUserStatus == true)
                            {
                                if (ddlClientstatus == "InActive")
                                {
                                    string queryupdate = "Update tbluser set status='InActive' where clientid='" + databaseCon.Decrypt(ClientID) + "'";
                                    int result = databaseCon.RunCmd(queryupdate);
                                }
                                // else if (ddlClientstatus.SelectedValue == "Active")
                                else if (ddlClientstatus == "Active")
                                {
                                    string queryupdate = "Update tbluser set status='Active' where clientid='" + databaseCon.Decrypt(ClientID) + "'";
                                    int result = databaseCon.RunCmd(queryupdate);
                                }
                            }
                            //Session["Insert"] = "update";
                            //Session["Message"] = clientdt.Rows[0][1].ToString();
                            //Response.Redirect("EditClient.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"]);
                            ResultMsg = "Success";
                            Message = clientdt.Rows[0][1].ToString();
                            EncriptClientID = databaseCon.Encrypt(id.ToString());
                        }
                        else
                        {
                            //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                            //art1.Style.Add("display", "block");
                            //alertlabel.Text = "<strong>Alert ! </strong>" + clientdt.Rows[0][1].ToString();
                            ResultMsg = "Failed";
                            Message = clientdt.Rows[0][1].ToString();
                        }
                    }
                    else
                    {
                        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator ";
                        ResultMsg = "Failed";
                        Message = "Syntax Error. Contact administrator .";
                    }

                }
                else if (ddlCountry == "United Kingdom")
                {
                    //string queryUk = "exec Pro_TBLCLIENT_SAVE '" + databaseCon.Decrypt(ClientID) + "','" + txtclientName.Replace("'", "''").Trim() + "','" + txtAddress.Replace("'", "''").Trim() + "','" + txtcity.Replace("'", "''").Trim() + "','" + ddlstate + "','" + txtpostcodeuk.Replace("'", "''").Trim() + "'," + (string.IsNullOrEmpty(txtUK.Trim()) ? "null" : "'" + txtUK.Trim() + "'") + ",'" + txtemail.Trim() + "','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "','" + ddlClientstatus + "','" + ddlInspectionTimeFrame + "'," + (txtStandardWorkingHrs == "" ? "0" : "'" + txtStandardWorkingHrs + "'") + ",'" + ddlCountry + "','" + plan + "','" + ddlweekstartday + "'," + (txtplnstandDur == "" ? "null" : "'" + txtplnstandDur + "'") + "," + (txtManHrsRatio == "" ? "null" : "'" + txtManHrsRatio + "'") + ",'" + chkIPAuthentication + "','" + checkboxManHoursManagement + "','" + isMainClient + "','" + checkOfflineEnabled + "','" + chkHireInventory + "','" + ddlLongTermStructure + "','" + txtClientInventoryMailingList + "','" + chkComparisonDashboard + "'";
                    string queryUk = "exec Pro_TBLCLIENT_SAVE '" + databaseCon.Decrypt(ClientID) + "','" + txtclientName.Replace("'", "''").Trim() + "','" + txtAddress.Replace("'", "''").Trim() + "','" + txtcity.Replace("'", "''").Trim() + "','" + ddlstate + "','" + txtpostcodeuk.Replace("'", "''").Trim() + "'," + (string.IsNullOrEmpty(txtUK.Trim()) ? "null" : "'" + txtUK.Trim() + "'") + ",'" + txtemail.Trim() + "','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "','" + ddlClientstatus + "','" + ddlInspectionTimeFrame + "'," + (txtStandardWorkingHrs == "" ? "0" : "'" + txtStandardWorkingHrs + "'") + ",'" + ddlCountry + "','" + plan + "','" + ddlweekstartday + "'," + (txtplnstandDur == "" ? "null" : "'" + txtplnstandDur + "'") + "," + (txtManHrsRatio == "" ? "null" : "'" + txtManHrsRatio + "'") + ",'" + chkIPAuthentication + "','" + checkboxManHoursManagement + "','" + isMainClient + "','" + checkOfflineEnabled + "','" + chkHireInventory + "','" + ddlLongTermStructure + "','" + txtClientInventoryMailingList + "','" + chkComparisonDashboard + "','" + chkOfflineInspection + "','" + chkNOGenerate + "','" + chkMainDepotInventory + "'";
                    clientdt = databaseCon.GetData(queryUk);
                    if (clientdt.Rows.Count != 0)
                    {
                        if (Convert.ToInt32(clientdt.Rows[0][0]) > 0)
                        {
                            int id = Convert.ToInt32(clientdt.Rows[0][0]);
                            //UpdateImagePath(id);
                            //ShowClientDetails();
                            // if (ddlClientstatus.SelectedValue == "InActive")
                            if (FlagUpdateUserStatus == true)
                            {
                                if (ddlClientstatus == "InActive")
                                {
                                    string queryupdate = "Update tbluser set status='InActive' where clientid='" + databaseCon.Decrypt(ClientID) + "'";
                                    int result = databaseCon.RunCmd(queryupdate);
                                }
                                //  else if (ddlClientstatus.SelectedValue == "Active")
                                else if (ddlClientstatus == "Active")
                                {
                                    string queryupdate = "Update tbluser set status='Active' where clientid='" + databaseCon.Decrypt(ClientID) + "'";
                                    int result = databaseCon.RunCmd(queryupdate);
                                }
                            }
                            //Session["Insert"] = "update";
                            //Session["Message"] = clientdt.Rows[0][1].ToString();
                            //Response.Redirect("EditClient.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"]);
                            ResultMsg = "Success";
                            Message = clientdt.Rows[0][1].ToString();
                            EncriptClientID = databaseCon.Encrypt(id.ToString());
                        }
                        else
                        {
                            //    art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                            //    art1.Style.Add("display", "block");
                            //    alertlabel.Text = "<strong>Alert ! </strong>" + clientdt.Rows[0][1].ToString();
                            ResultMsg = "Failed";
                            Message = clientdt.Rows[0][1].ToString();
                        }
                    }
                    else
                    {
                        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator ";
                        ResultMsg = "Failed";
                        Message = "Syntax Error. Contact administrator .";
                    }
                    // }
                    // Else Condition For Is Main Client Checked
                    //    }
                    //    else
                    //    {
                    //    art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                    //    art1.Style.Add("display", "block");
                    //    alertlabel.Text = "<strong>Alert ! </strong>Subclient selection is necessary if Client is selected as Main Client.";
                    //}
                }
            }
            else
            {
                //if ((isMainClient.Checked == true && lstSubClient.SelectedValue != "") || (isMainClient.Checked != true && lstSubClient.SelectedValue == ""))
                //{
                //    string deletequery = "delete from TblMainClientToClient where MainClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "'";
                //    int deleteID = databaseCon.RunCmd(deletequery);
                //    foreach (ListItem s in lstSubClient.Items)
                //    {
                //        if (s.Selected)
                //        {
                //            string SubClientquery = "insert into TblMainClientToClient (MainClientId,SubClientId,CreatedBy,CreatedOn)values('" + databaseCon.Decrypt(Request.QueryString["id"]) + "'," + s.Value + ",(select EmailId from tblUser where UserId='" + databaseCon.Decrypt(Request.QueryString["UID"]) + "'),'" + DateTime.Now + "')";
                //            int id = databaseCon.RunCmdID(SubClientquery);

                //        }
                //    }

                //query = "exec Pro_TBLCLIENT_SAVE '" + HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["id"])) + "','" + txtclientName.Replace("'", "''").Trim() + "','" + txtAddress.Replace("'", "''").Trim() + "','" + txtcity.Replace("'", "''").Trim() + "','" + ddlstate + "','" + txtzip.Replace("'", "''").Trim() + "', " + (String.IsNullOrEmpty(txtContact.Trim()) ? "null" : "'" + txtContact.Trim() + "'") + ",'" + txtemail.Trim() + "','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "','" + ddlClientstatus + "','" + ddlInspectionTimeFrame + "'," + (txtStandardWorkingHrs == "" ? "0" : "'" + txtStandardWorkingHrs + "'") + ",'" + ddlCountry + "','" + plan.ToString() + "','" + ddlweekstartday + "'," + (txtplnstandDur == "" ? "null" : "'" + txtplnstandDur + "'") + "," + (txtManHrsRatio == "" ? "null" : "'" + txtManHrsRatio + "'") + ",'" + chkIPAuthentication + "','" + checkboxManHoursManagement + "','" + isMainClient + "','" + checkOfflineEnabled + "','" + chkHireInventory + "','" + ddlLongTermStructure + "','" + txtClientInventoryMailingList + "','" + chkComparisonDashboard + "'";

                query = "exec Pro_TBLCLIENT_SAVE '" + HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["id"])) + "','" + txtclientName.Replace("'", "''").Trim() + "','" + txtAddress.Replace("'", "''").Trim() + "','" + txtcity.Replace("'", "''").Trim() + "','" + ddlstate + "','" + txtzip.Replace("'", "''").Trim() + "', " + (String.IsNullOrEmpty(txtContact.Trim()) ? "null" : "'" + txtContact.Trim() + "'") + ",'" + txtemail.Trim() + "','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "','" + ddlClientstatus + "','" + ddlInspectionTimeFrame + "'," + (txtStandardWorkingHrs == "" ? "0" : "'" + txtStandardWorkingHrs + "'") + ",'" + ddlCountry + "','" + plan.ToString() + "','" + ddlweekstartday + "'," + (txtplnstandDur == "" ? "null" : "'" + txtplnstandDur + "'") + "," + (txtManHrsRatio == "" ? "null" : "'" + txtManHrsRatio + "'") + ",'" + chkIPAuthentication + "','" + checkboxManHoursManagement + "','" + isMainClient + "','" + checkOfflineEnabled + "','" + chkHireInventory + "','" + ddlLongTermStructure + "','" + txtClientInventoryMailingList + "','" + chkComparisonDashboard + "','" + chkOfflineInspection + "','" + chkNOGenerate + "','" + chkMainDepotInventory + "'";
                clientdt = databaseCon.GetData(query);

                if (clientdt.Rows.Count != 0)
                {
                    if (Convert.ToInt32(clientdt.Rows[0][0]) > 0)
                    {
                        int id = Convert.ToInt32(clientdt.Rows[0][0]);
                        //UpdateImagePath(id);
                        //ShowClientDetails();
                        //Session["Insert"] = "Insert";
                        //Session["Message"] = Convert.ToString(clientdt.Rows[0][1]);

                        //Response.Redirect("EditClient.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + HttpUtility.UrlEncode(databaseCon.Encrypt(id.ToString())));
                        ResultMsg = "Success";
                        Message = clientdt.Rows[0][1].ToString();
                        EncriptClientID = databaseCon.Encrypt(id.ToString());

                    }
                    else
                    {
                        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Alert ! </strong>" + clientdt.Rows[0][1].ToString();
                        ResultMsg = "Failed";
                        Message = clientdt.Rows[0][1].ToString();
                    }

                }
                else
                {
                    //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator ";
                    ResultMsg = "Failed";
                    Message = "Syntax Error. Contact administrator .";
                }

                // Else Condition For Is Main Client Checked
                //}
                //else
                //{
                //    art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                //    art1.Style.Add("display", "block");
                //    alertlabel.Text = "<strong>Alert ! </strong>Subclient selection is necessary if Client is selected as Main Client.";
                //}
            }
            return Json(new { Result = ResultMsg, Message = Message, EncriptClientID = EncriptClientID }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateImagePath(string id, string txtclientName, string imagename, string imgname, string radUpdogImg)
        {
            if (Convert.ToInt32(databaseCon.Decrypt(id)) > 0)
            {
                string extension = "";
                string FileName = "";
                string path = "/images/clientlogo/";
                path = Server.MapPath(path);
                string newpath2 = "";
                string QueryU = "update tblClient set";
                //  if (radUpdogImg.UploadedFiles.Count > 0)
                if (radUpdogImg != "")
                {
                    //extension = radUpdogImg.UploadedFiles[0].GetExtension();
                    extension = System.IO.Path.GetExtension(radUpdogImg).ToLower();
                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png" || extension.ToLower() == ".ico" || extension.ToLower() == ".gif" || extension.ToLower() == ".bmp")
                    {
                        // FileName = ddlsites.SelectedValue.ToString() + "_" + txtsiteAddress.Text.Replace("'", "''").Trim() + "_" + txtsiteCity.Text.Replace("'", "''").Trim() + "_" + id + "_" + RID + extension;
                        FileName = txtclientName.Replace("'", "''").Replace(" ", "").Replace("&", "").Trim() + "_" + databaseCon.Decrypt(id) + "_" + DateTime.Now.Ticks + extension;
                        newpath2 = path + FileName;


                        HttpFileCollectionBase files = Request.Files;

                        for (int k = 0; k < 1; k++)
                        {

                            HttpPostedFileBase postedFile = files[k];


                            // if (ViewState["imgname"].ToString() != "")
                            if (imgname != "")
                            {
                                // string clientimgget = ViewState["imgname"].ToString();
                                string clientimgget = imgname;
                                string imgfile = path + clientimgget;
                                System.IO.File.Delete(imgfile);
                                //File.Delete(ViewState["imgname"].ToString());
                                //uploadlogo.SaveAs(newpath2);
                                // radUpdogImg.UploadedFiles[0].SaveAs(newpath2);
                                postedFile.SaveAs(newpath2);
                            }
                            else
                            {
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                //uploadlogo.SaveAs(newpath2);
                                // radUpdogImg.UploadedFiles[0].SaveAs(newpath2);
                                postedFile.SaveAs(newpath2);
                            }
                            QueryU += " ClientImage='" + FileName + "' where ClientId=" + databaseCon.Decrypt(id);
                            int count = databaseCon.RunCmd(QueryU);
                            string fpath = path + FileName;
                            //databaseCon.savefileonblob(FileName, "Client-img", fpath, "image/png");
                            //File.Delete(fpath);
                        }
                    }

                }
                // else if (imagename.Value != null && imagename.Value != "")
                else if (imagename != null && imagename != "")
                {
                    string Imgedata = imagename;
                    FileName = txtclientName.Replace("'", "''").Replace(" ", "").Replace("&", "").Trim() + "_" + databaseCon.Decrypt(id) + "_" + DateTime.Now.Ticks + ".jpg";
                    string sketchnewPath = path + FileName;
                    byte[] bytes = Convert.FromBase64String(Imgedata.Split(',')[1]);
                    if (System.IO.File.Exists(sketchnewPath))
                    {
                        string fpath = path + FileName;
                        System.IO.File.Delete(fpath);
                    }
                    using (var imageFile = new FileStream(sketchnewPath, FileMode.Create))
                    {
                        imageFile.Write(bytes, 0, bytes.Length);
                        imageFile.Flush();
                    }
                    QueryU += " ClientImage='" + FileName + "' where ClientId=" + databaseCon.Decrypt(id);
                    int count = databaseCon.RunCmd(QueryU);
                }

            }
            return Json(new { Result = "Success", Message = "Image Save Successfully ." }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult btndeleteImageYes_Click(string id, string imgname)
        {
            var Result = "";
            var Message = "";
            string query;
            // string filePath = Server.MapPath("/images/clientlogo/" + ViewState["imagedelete"]);
            string filePath = Server.MapPath("/images/clientlogo/" + imgname);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);

            }
            query = "update tblClient set ClientImage='' where ClientID='" + databaseCon.Decrypt(id) + "'";
            int result = databaseCon.RunCmd(query);
            if (result > 0)
            {
                //ShowClientDetails();
                //art1.Style.Add("display", "block");
                //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Success";
                //alertlabel.Text = "<strong> Info ! </strong> Image Deleted Successfully.";

                Result = "Success";
                Message = "Image Deleted Successfully.";

            }
            else
            {
                Result = "Failed";
                Message = "Image Not Deleted.";
            }
            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSiteData(string id)
        {

            var GetData = dbcontext.Database.SqlQuery<Pro_TBLSITELOCATION_GETViewModel>("exec [Pro_TBLSITELOCATION_GET] ''," + databaseCon.Decrypt(id)).ToList();
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SiteSave(string id, string UID, string txtsitelocation, string txtaddress, string txtcity, string ddlstates, string txtzip, string txtukpost, string ddlStatuss, string txtOwnerName, string txtdistriList, string txtlat, string txtlong, string txtManHrsAvai, string checkboxMaintainInventory, string checkboxCycloneManagement, string chkboxManHoursManagement, string chkboxEnabledGPSSitePin, string checkboxMaintainInventoryAtJob, string chkboxEnabledCaptureGps, string btngps, string btnfloorlevel, string sitelocdid, string chkOffSystemInspection, string txtMailingList, string chkboxSkipMaterial, string chkPipelineInv, string showpopup)
        {

            var query = "";
            DataTable dt;
            var ResultVal = "";
            var MessageVal = "";
            var postcode = "";

            if (txtzip != "")
            {
                postcode = txtzip;
            }

            if (txtukpost != "")
            {
                postcode = txtukpost;
            }

            //GridEditableItem item = (GridEditableItem)e.Item;
            //RadTextBox txtsitelocation = (RadTextBox)item.FindControl("txtsitelocation");
            //RadTextBox txtaddress = (RadTextBox)item.FindControl("txtaddress");
            //RadTextBox txtcity = (RadTextBox)item.FindControl("txtcity");
            //RadComboBox ddlstates = (RadComboBox)item.FindControl("ddlstates");
            //RadMaskedTextBox txtzip = (RadMaskedTextBox)item.FindControl("txtzip");
            //RadTextBox txtukzip = (RadTextBox)item.FindControl("txtukpost");
            //RadComboBox ddlStatuss = (RadComboBox)item.FindControl("ddlStatuss");
            //RadTextBox txtOwnerName = (RadTextBox)item.FindControl("txtOwnerName");
            //RadTextBox txtdistriList = (RadTextBox)item.FindControl("txtdistriList");
            //RadNumericTextBox txtlat = (RadNumericTextBox)item.FindControl("txtlat");
            //RadNumericTextBox txtlong = (RadNumericTextBox)item.FindControl("txtlong");
            //RadNumericTextBox txtManHrsAvai = (RadNumericTextBox)item.FindControl("txtManHrsAvai");
            //CheckBox checkboxMaintainInventory = (CheckBox)item.FindControl("checkboxMaintainInventory");
            //CheckBox checkboxCycloneManagement = (CheckBox)item.FindControl("checkboxCycloneManagement");
            //CheckBox chkboxManHoursManagement = (CheckBox)item.FindControl("chkboxManHoursManagement");
            //CheckBox chkboxEnabledGPSSitePin = (CheckBox)item.FindControl("chkboxEnabledGPSSitePin");
            //CheckBox checkboxMaintainInventoryAtJob = (CheckBox)item.FindControl("checkboxMaintainInventoryAtJob");
            //CheckBox chkboxEnabledCaptureGps = (CheckBox)item.FindControl("chkboxEnabledCaptureGps");
            //RadioButton btngps = (RadioButton)item.FindControl("btngps");
            //RadioButton btnfloorlevel = (RadioButton)item.FindControl("btnfloorlevel");
            ////Add on 24-Feb-2020.
            //RadNumericTextBox txtPI = (RadNumericTextBox)item.FindControl("txtPI");
            //RadNumericTextBox txtMaintenance = (RadNumericTextBox)item.FindControl("txtMaintenance");



            //if (e.Item.OwnerTableView.Name == "SiteMaster")
            //{
            //    // For Inpex Only
            //    // Show Checkbox For Man Hours Management,Maintain Inventory And Cyclone Managment Checkbox only for Super Admin All Role Hide.
            //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp10", "<script>hideCheckBox();</script>", false);
            //    if (HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["UserRole"])) != "10")
            //    {
            //        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp15", "<script>hidedivGpsSitePin();</script>", false);
            //    }


            //    //// Show Checkbox For Man Hours Management,Maintain Inventory And Cyclone Managment Checkbox only for Super Admin All Role Hide.
            //    //if (HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["UserRole"])) != "10")
            //    //{
            //    //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp10", "<script>hideCheckBox();</script>", false);
            //    //}

            //    string querymanhours = "select isnull(ManHoursManagement,0) as ManHoursManagement  from tblClient where ClientId = '" + HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["id"])) + "'";
            //    DataTable dataTable = databaseCon.GetData(querymanhours);

            //    if (e.CommandName == RadGrid.InitInsertCommandName)
            //    {
            //        if (dataTable.Rows[0]["ManHoursManagement"].ToString() == "False")
            //        {
            //            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ManHourCheckBoxHide", "<script>hideManHour();</script>", false);
            //        }
            //        else
            //        {
            //            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ManHourCheckBoxHideByDefault", "<script> ByDefaultClient();</script>", false);

            //        }

            //    }
            //    if (e.CommandName == "Edit")
            //    {

            //        if (dataTable.Rows[0]["ManHoursManagement"].ToString() == "False")
            //        {
            //            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ManHourCheckBoxHide", "<script>hideManHour();</script>", false);
            //        }

            //    }


            if (sitelocdid == "")
            {

                //  if (e.CommandName == RadGrid.PerformInsertCommandName)
                //  {
                //GridEditableItem item = (GridEditableItem)e.Item;
                //RadTextBox txtsitelocation = (RadTextBox)item.FindControl("txtsitelocation");
                //RadTextBox txtaddress = (RadTextBox)item.FindControl("txtaddress");
                //RadTextBox txtcity = (RadTextBox)item.FindControl("txtcity");
                //RadComboBox ddlstates = (RadComboBox)item.FindControl("ddlstates");
                //RadMaskedTextBox txtzip = (RadMaskedTextBox)item.FindControl("txtzip");
                //RadTextBox txtukzip = (RadTextBox)item.FindControl("txtukpost");
                //RadComboBox ddlStatuss = (RadComboBox)item.FindControl("ddlStatuss");
                //RadTextBox txtOwnerName = (RadTextBox)item.FindControl("txtOwnerName");
                //RadTextBox txtdistriList = (RadTextBox)item.FindControl("txtdistriList");
                //RadNumericTextBox txtlat = (RadNumericTextBox)item.FindControl("txtlat");
                //RadNumericTextBox txtlong = (RadNumericTextBox)item.FindControl("txtlong");
                //RadNumericTextBox txtManHrsAvai = (RadNumericTextBox)item.FindControl("txtManHrsAvai");
                //CheckBox checkboxMaintainInventory = (CheckBox)item.FindControl("checkboxMaintainInventory");
                //CheckBox checkboxCycloneManagement = (CheckBox)item.FindControl("checkboxCycloneManagement");
                //CheckBox chkboxManHoursManagement = (CheckBox)item.FindControl("chkboxManHoursManagement");
                //CheckBox chkboxEnabledGPSSitePin = (CheckBox)item.FindControl("chkboxEnabledGPSSitePin");
                //CheckBox checkboxMaintainInventoryAtJob = (CheckBox)item.FindControl("checkboxMaintainInventoryAtJob");
                //CheckBox chkboxEnabledCaptureGps = (CheckBox)item.FindControl("chkboxEnabledCaptureGps");
                //RadioButton btngps = (RadioButton)item.FindControl("btngps");
                //RadioButton btnfloorlevel = (RadioButton)item.FindControl("btnfloorlevel");
                ////Add on 24-Feb-2020.
                //RadNumericTextBox txtPI = (RadNumericTextBox)item.FindControl("txtPI");
                //RadNumericTextBox txtMaintenance = (RadNumericTextBox)item.FindControl("txtMaintenance");


                string Planquery = "select Plans from tblClient where ClientId = '" + databaseCon.Decrypt(id) + "'";
                DataTable plandt = databaseCon.GetData(Planquery);
                if (plandt.Rows[0]["Plans"].ToString() == "Plan1")
                {
                    //checkboxCycloneManagement.Checked = false;
                    //checkboxMaintainInventory.Checked = false;
                    //chkboxManHoursManagement.Checked = false;
                    //checkboxMaintainInventoryAtJob.Checked = false;

                    checkboxCycloneManagement = "False";
                    checkboxMaintainInventory = "False";
                    chkboxManHoursManagement = "False";
                    checkboxMaintainInventoryAtJob = "False";
                }




                // if (((checkboxMaintainInventory.Checked == true) && (checkboxMaintainInventoryAtJob.Checked == true)) || ((checkboxMaintainInventory.Checked == true) && (checkboxMaintainInventoryAtJob.Checked == false)) || ((checkboxMaintainInventory.Checked == false) && (checkboxMaintainInventoryAtJob.Checked == false)))
                if (((checkboxMaintainInventory == "True") && (checkboxMaintainInventoryAtJob == "True")) || ((checkboxMaintainInventory == "True") && (checkboxMaintainInventoryAtJob == "False")) || ((checkboxMaintainInventory == "False") && (checkboxMaintainInventoryAtJob == "False")))
                {
                    // query = "exec Pro_TBLSITELOCATION_SAVE '0','" + txtsitelocation.Text.Replace("'", "''").Trim() + "','" + txtaddress.Text.Replace("'", "''").Trim() + "','" + txtcity.Text.Replace("'", "''").Trim() + "','" + ddlstates.SelectedValue.Replace("'", "''").Trim() + "','" + postcode + "','" + HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["id"])) + "','" + HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["UID"])) + "','" + DateTime.Now + "','" + ddlStatuss.SelectedValue.Replace("'", "''").Trim() + "','" + txtOwnerName.Text.Replace("'", "''").Trim() + "','" + txtdistriList.Text.Replace("'", "''").Trim() + "','','" + txtlat.Text.Replace("'", "''").Trim() + "','" + txtlong.Text.Replace("'", "''").Trim() + "'," + (txtManHrsAvai.Text == "" ? "null" : "'" + txtManHrsAvai.Text + "'") + "," + checkboxMaintainInventory.Checked + "," + checkboxCycloneManagement.Checked + "," + chkboxManHoursManagement.Checked + "," + checkboxMaintainInventoryAtJob.Checked + "," + btngps.Checked + "," + btnfloorlevel.Checked + "," + chkboxEnabledGPSSitePin.Checked + "," + (txtPI.Text == "" ? "null" : "'" + txtPI.Text + "'") + "," + (txtMaintenance.Text == "" ? "null" : "'" + txtMaintenance.Text + "'") + "," + chkboxEnabledCaptureGps.Checked + "";


                    query = "exec Pro_TBLSITELOCATION_SAVE '0','" + txtsitelocation.Replace("'", "''").Trim() + "','" + txtaddress.Replace("'", "''").Trim() + "','" + txtcity.Replace("'", "''").Trim() + "','" + ddlstates + "','" + postcode + "','" + databaseCon.Decrypt(id) + "','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "','" + ddlStatuss.Replace("'", "''").Trim() + "'," + (txtOwnerName == "" ? "null" : "'" + txtOwnerName.Replace("'", "''").Trim() + "'") + ",'" + txtdistriList.Replace("'", "''").Trim() + "','','" + txtlat.Replace("'", "''").Trim() + "','" + txtlong.Replace("'", "''").Trim() + "'," + (txtManHrsAvai == "" ? "null" : "'" + txtManHrsAvai + "'") + "," + checkboxMaintainInventory + "," + checkboxCycloneManagement + "," + chkboxManHoursManagement + "," + checkboxMaintainInventoryAtJob + "," + btngps + "," + btnfloorlevel + "," + chkboxEnabledGPSSitePin + "," + chkboxEnabledCaptureGps + "," + chkOffSystemInspection + ",'" + txtMailingList.Replace("'", "''").Trim() + "'," + chkboxSkipMaterial + "," + chkPipelineInv + "";
                    dt = databaseCon.GetData(query);
                    if (dt.Rows.Count != 0)
                    {
                        if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                        {
                            //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Success";
                            //art1.Style.Add("display", "block");
                            //alertlabel.Text = "<strong>Success ! </strong>" + dt.Rows[0][1].ToString();
                            //gridsitelocation.Rebind();

                            ResultVal = "Success";
                            MessageVal = dt.Rows[0][1].ToString();
                        }
                        else if (Convert.ToInt32(dt.Rows[0][0]) == -1)
                        {
                            //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                            //art1.Style.Add("display", "block");
                            //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();

                            ResultVal = "Failed";
                            MessageVal = dt.Rows[0][1].ToString();

                        }
                        else
                        {
                            //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                            //art1.Style.Add("display", "block");
                            //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                            ResultVal = "Failed";
                            MessageVal = dt.Rows[0][1].ToString();
                        }
                    }
                    else
                    {
                        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator ";
                        ResultVal = "Failed";
                        MessageVal = "Syntax Error. Contact administrator. ";
                    }
                }
                else
                {
                    //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong> Maintain Inventory At Site selection is necessary if Maintain Inventory At Job is selected .";
                    ResultVal = "Failed";
                    MessageVal = "Maintain Inventory At Site selection is necessary if Maintain Inventory At Job is selected .";
                }
            }
            else
            {
                // if (e.CommandName == RadGrid.UpdateCommandName)
                // {
                //GridEditableItem item = (GridEditableItem)e.Item;
                //RadTextBox txtsitelocation = (RadTextBox)item.FindControl("txtsitelocation");
                //RadTextBox txtaddress = (RadTextBox)item.FindControl("txtaddress");
                //RadTextBox txtcity = (RadTextBox)item.FindControl("txtcity");
                //RadComboBox ddlstates = (RadComboBox)item.FindControl("ddlstates");
                //RadMaskedTextBox txtzip = (RadMaskedTextBox)item.FindControl("txtzip");
                //RadTextBox txtukzip = (RadTextBox)item.FindControl("txtukpost");
                //RadComboBox ddlStatuss = (RadComboBox)item.FindControl("ddlStatuss");
                //string sitelocdid = item.GetDataKeyValue("ID").ToString();
                //RadTextBox txtOwnerName = (RadTextBox)item.FindControl("txtOwnerName");
                //RadTextBox txtdistriList = (RadTextBox)item.FindControl("txtdistriList");
                //RadNumericTextBox txtlat = (RadNumericTextBox)item.FindControl("txtlat");
                //RadNumericTextBox txtlong = (RadNumericTextBox)item.FindControl("txtlong");
                //RadNumericTextBox txtManHrsAvai = (RadNumericTextBox)item.FindControl("txtManHrsAvai");
                //CheckBox checkboxMaintainInventory = (CheckBox)item.FindControl("checkboxMaintainInventory");
                //CheckBox checkboxCycloneManagement = (CheckBox)item.FindControl("checkboxCycloneManagement");
                //CheckBox chkboxManHoursManagement = (CheckBox)item.FindControl("chkboxManHoursManagement");
                //CheckBox chkboxEnabledGPSSitePin = (CheckBox)item.FindControl("chkboxEnabledGPSSitePin");
                //CheckBox checkboxMaintainInventoryAtJob = (CheckBox)item.FindControl("checkboxMaintainInventoryAtJob");
                //CheckBox chkboxEnabledCaptureGps = (CheckBox)item.FindControl("chkboxEnabledCaptureGps");
                //RadioButton btngps = (RadioButton)item.FindControl("btngps");
                //RadioButton btnfloorlevel = (RadioButton)item.FindControl("btnfloorlevel");
                ////Add on 24-Feb-2020.
                //RadNumericTextBox txtPI = (RadNumericTextBox)item.FindControl("txtPI");
                //RadNumericTextBox txtMaintenance = (RadNumericTextBox)item.FindControl("txtMaintenance");


                //string Planquery = "select Plans from tblClient where ClientId = '" + HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["id"])) + "'";
                //DataTable plandt = databaseCon.GetData(Planquery);
                //if (plandt.Rows[0]["Plans"].ToString() == "Plan1")
                //{
                //    checkboxCycloneManagement.Checked = false;
                //    checkboxMaintainInventory.Checked = false;
                //    chkboxManHoursManagement.Checked = false;
                //    checkboxMaintainInventoryAtJob.Checked = false;
                //}

                //if (txtzip.Text != "")
                //{
                //    postcode = txtzip.Text;
                //}

                //else if (txtukzip.Text != "")
                //{
                //    postcode = txtukzip.Text;
                //}

                // Add Check For Gps And Floor Change Condation
                string Gps = "";
                string FloorLevel = "";
                var RegisterIDCount = 0;
                var Message = "";

                string SiteQuery = "select tblSiteLocation.GPS,tblSiteLocation.FloorLevel,COUNT(RegisterId) as RegisterId from tblSiteLocation left join tblRegister on tblSiteLocation.SiteLocID = tblRegister.SiteLocID  where tblSiteLocation.ClientId = '" + databaseCon.Decrypt(id) + "' and tblSiteLocation.SiteLocId='" + sitelocdid + "' group by  tblSiteLocation.GPS,tblSiteLocation.FloorLevel";
                DataTable datatable1 = databaseCon.GetData(SiteQuery);
                if (datatable1.Rows.Count > 0)
                {
                    if (datatable1.Rows[0]["GPS"].ToString() == "")
                    {
                        Gps = "False";
                    }
                    else
                    {
                        Gps = datatable1.Rows[0]["GPS"].ToString();
                    }
                    if (datatable1.Rows[0]["FloorLevel"].ToString() == "")
                    {
                        FloorLevel = "False";
                    }
                    else
                    {
                        FloorLevel = datatable1.Rows[0]["FloorLevel"].ToString();
                    }

                    RegisterIDCount = Convert.ToInt32(datatable1.Rows[0]["RegisterId"]);
                    if (Gps == "True")
                    {
                        Message = "Gps";
                    }
                    if (FloorLevel == "True")
                    {
                        Message = "FloorLevel";
                    }
                }

                bool flag = false;
                if ((RegisterIDCount == 0))
                {
                    flag = true;
                }
                var checkBoxGps = btngps;
                var checkBoxFloo = btnfloorlevel;
                if ((RegisterIDCount > 0 && Gps == btngps && FloorLevel == btnfloorlevel))
                {
                    flag = true;
                }

                if (flag == true)
                {

                    //  if (((checkboxMaintainInventory.Checked == true) && (checkboxMaintainInventoryAtJob.Checked == true)) || ((checkboxMaintainInventory.Checked == true) && (checkboxMaintainInventoryAtJob.Checked == false)) || ((checkboxMaintainInventory.Checked == false) && (checkboxMaintainInventoryAtJob.Checked == false)))
                    if (((checkboxMaintainInventory == "True") && (checkboxMaintainInventoryAtJob == "True")) || ((checkboxMaintainInventory == "True") && (checkboxMaintainInventoryAtJob == "False")) || ((checkboxMaintainInventory == "False") && (checkboxMaintainInventoryAtJob == "False")))
                    {
                        // For Site Inventory Checked
                        // For Checked Site Query

                        string querysite = "select COUNT(*) as SiteInventoryCount from tblTransaction where ClientId='" + databaseCon.Decrypt(id) + "' and [From]='Site' and FromId='" + sitelocdid + "' ";
                        DataTable dtsite = databaseCon.GetData(querysite);

                        //  if (((dtsite.Rows[0]["SiteInventoryCount"].ToString() == "0") && (checkboxMaintainInventory.Checked == true)) || ((dtsite.Rows[0]["SiteInventoryCount"].ToString() == "0") && (checkboxMaintainInventory.Checked == false)) || ((dtsite.Rows[0]["SiteInventoryCount"].ToString() != "0") && (checkboxMaintainInventory.Checked == true)))

                        if (((dtsite.Rows[0]["SiteInventoryCount"].ToString() == "0") && (checkboxMaintainInventory == "True")) || ((dtsite.Rows[0]["SiteInventoryCount"].ToString() == "0") && (checkboxMaintainInventory == "False")) || ((dtsite.Rows[0]["SiteInventoryCount"].ToString() != "0") && (checkboxMaintainInventory == "True")))
                        {
                            // for Job Onventory Check
                            // For Checked Job Inventory

                            string queryjob = "select COUNT(*) as JobInventoryCount from tblTransaction where ClientId='" + databaseCon.Decrypt(id) + "' and [From]='Job' and ToId='" + sitelocdid + "' ";
                            DataTable dtJob = databaseCon.GetData(queryjob);

                            // if (((dtJob.Rows[0]["JobInventoryCount"].ToString() == "0") && (checkboxMaintainInventoryAtJob.Checked == true)) || ((dtJob.Rows[0]["JobInventoryCount"].ToString() == "0") && (checkboxMaintainInventoryAtJob.Checked == false)) || ((dtJob.Rows[0]["JobInventoryCount"].ToString() != "0") && (checkboxMaintainInventoryAtJob.Checked == true)))
                            if (((dtJob.Rows[0]["JobInventoryCount"].ToString() == "0") && (checkboxMaintainInventoryAtJob == "True")) || ((dtJob.Rows[0]["JobInventoryCount"].ToString() == "0") && (checkboxMaintainInventoryAtJob == "False")) || ((dtJob.Rows[0]["JobInventoryCount"].ToString() != "0") && (checkboxMaintainInventoryAtJob == "True")))
                            {

                                //query = "exec Pro_TBLSITELOCATION_SAVE  '" + sitelocdid + "','" + txtsitelocation.Text.Replace("'", "''").Trim() + "','" + txtaddress.Text.Replace("'", "''").Trim() + "','" + txtcity.Text.Replace("'", "''").Trim() + "','" + ddlstates.SelectedValue.Replace("'", "''").Trim() + "','" + postcode + "','" + HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["id"])) + "','" + HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["UID"])) + "','" + DateTime.Now + "','" + ddlStatuss.SelectedValue.Replace("'", "''").Trim() + "','" + txtOwnerName.Text.Replace("'", "''").Trim() + "','" + txtdistriList.Text.Replace("'", "''").Trim() + "','','" + txtlat.Text.Replace("'", "''").Trim() + "','" + txtlong.Text.Replace("'", "''").Trim() + "'," + (txtManHrsAvai.Text == "" ? "null" : "'" + txtManHrsAvai.Text + "'") + "," + checkboxMaintainInventory.Checked + "," + checkboxCycloneManagement.Checked + "," + chkboxManHoursManagement.Checked + "," + checkboxMaintainInventoryAtJob.Checked + "," + btngps.Checked + "," + btnfloorlevel.Checked + "," + chkboxEnabledGPSSitePin.Checked + "";

                                // change on 24-FEB-2020.

                                //query = "exec Pro_TBLSITELOCATION_SAVE  '" + sitelocdid + "','" + txtsitelocation.Text.Replace("'", "''").Trim() + "','" + txtaddress.Text.Replace("'", "''").Trim() + "','" + txtcity.Text.Replace("'", "''").Trim() + "','" + ddlstates.SelectedValue.Replace("'", "''").Trim() + "','" + postcode + "','" + HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["id"])) + "','" + HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["UID"])) + "','" + DateTime.Now + "','" + ddlStatuss.SelectedValue.Replace("'", "''").Trim() + "','" + txtOwnerName.Text.Replace("'", "''").Trim() + "','" + txtdistriList.Text.Replace("'", "''").Trim() + "','','" + txtlat.Text.Replace("'", "''").Trim() + "','" + txtlong.Text.Replace("'", "''").Trim() + "'," + (txtManHrsAvai.Text == "" ? "null" : "'" + txtManHrsAvai.Text + "'") + "," + checkboxMaintainInventory.Checked + "," + checkboxCycloneManagement.Checked + "," + chkboxManHoursManagement.Checked + "," + checkboxMaintainInventoryAtJob.Checked + "," + btngps.Checked + "," + btnfloorlevel.Checked + "," + chkboxEnabledGPSSitePin.Checked + "," + (txtPI.Text == "" ? "null" : "'" + txtPI.Text + "'") + "," + (txtMaintenance.Text == "" ? "null" : "'" + txtMaintenance.Text + "'") + "," + chkboxEnabledCaptureGps.Checked + "";

                                // Changes On 22 June 2020
                                query = "exec Pro_TBLSITELOCATION_SAVE  '" + sitelocdid + "','" + txtsitelocation.Replace("'", "''").Trim() + "','" + txtaddress.Replace("'", "''").Trim() + "','" + txtcity.Replace("'", "''").Trim() + "','" + ddlstates + "','" + postcode + "','" + databaseCon.Decrypt(id) + "','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "','" + ddlStatuss.Replace("'", "''").Trim() + "'," + (txtOwnerName == "" ? "null" : "'" + txtOwnerName.Replace("'", "''").Trim() + "'") + ",'" + txtdistriList.Replace("'", "''").Trim() + "','','" + txtlat.Replace("'", "''").Trim() + "','" + txtlong.Replace("'", "''").Trim() + "'," + (txtManHrsAvai == "" ? "null" : "'" + txtManHrsAvai + "'") + "," + checkboxMaintainInventory + "," + checkboxCycloneManagement + "," + chkboxManHoursManagement + "," + checkboxMaintainInventoryAtJob + "," + btngps + "," + btnfloorlevel + "," + chkboxEnabledGPSSitePin + "," + chkboxEnabledCaptureGps + "," + chkOffSystemInspection + ",'" + txtMailingList.Replace("'", "''").Trim() + "'," + chkboxSkipMaterial + "," + chkPipelineInv + "," + showpopup + "";

                                dt = databaseCon.GetData(query);
                                if (dt.Rows.Count != 0)
                                {
                                    if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                                    {
                                        // Add Code on 04 Sep 2019 For Update All Register Maintain inventory Base On Site Selection
                                        string qry = "update tblRegister set MaintainInventory='" + checkboxMaintainInventoryAtJob + "' where SiteLocId='" + sitelocdid + "'";
                                        int result = databaseCon.RunCmd(qry);

                                        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Success";
                                        //art1.Style.Add("display", "block");
                                        //alertlabel.Text = "<strong>Success ! </strong>" + dt.Rows[0][1].ToString();
                                        //gridsitelocation.Rebind();

                                        ResultVal = "Success";
                                        MessageVal = dt.Rows[0][1].ToString();
                                    }
                                    else if (Convert.ToInt32(dt.Rows[0][0]) == -1)
                                    {
                                        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                                        //art1.Style.Add("display", "block");
                                        //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();

                                        ResultVal = "Failed";
                                        MessageVal = dt.Rows[0][1].ToString();
                                    }
                                    else
                                    {
                                        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                                        //art1.Style.Add("display", "block");
                                        //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();

                                        ResultVal = "Failed";
                                        MessageVal = dt.Rows[0][1].ToString();
                                    }
                                }
                                else
                                {
                                    //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                                    //art1.Style.Add("display", "block");
                                    //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator ";

                                    ResultVal = "Failed";
                                    MessageVal = "Syntax Error. Contact administrator .";
                                }
                            }
                            else
                            {
                                //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                                //art1.Style.Add("display", "block");
                                //alertlabel.Text = "<strong>Alert ! </strong> Material has already been loaded on Job Inventory so you cannot uncheck Maintain Inventory.";
                                ResultVal = "Failed";
                                MessageVal = "Material has already been loaded on Job Inventory so you cannot uncheck Maintain Inventory.";
                            }
                        }
                        else
                        {
                            //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                            //art1.Style.Add("display", "block");
                            //alertlabel.Text = "<strong>Alert ! </strong> Material has already been loaded on Site Inventory so you cannot uncheck Maintain Inventory.";
                            ResultVal = "Failed";
                            MessageVal = "Material has already been loaded on Site Inventory so you cannot uncheck Maintain Inventory.";
                        }

                    }
                    else
                    {
                        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Alert ! </strong>  Maintain Inventory At Site selection is necessary if Maintain Inventory At Job is selected .";

                        ResultVal = "Failed";
                        MessageVal = "Maintain Inventory At Site selection is necessary if Maintain Inventory At Job is selected .";
                    }
                }
                else
                {
                    //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong> There are jobs that have been assigned " + Message + " so you cannot change it .";
                    ResultVal = "Failed";
                    MessageVal = "There are jobs that have been assigned " + Message + " so you cannot change it.";

                }

            }

            return Json(new { Result = ResultVal, Message = MessageVal }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetLocationData(string id, string sitelocaID)
        {

            var GetData = dbcontext.Database.SqlQuery<Pro_TBLAREA_GETViewModel>("exec [Pro_TBLAREA_GET] ''," + databaseCon.Decrypt(id) + "," + sitelocaID + "").ToList();
            return Json(GetData, JsonRequestBehavior.AllowGet);

        }

        public ActionResult LocationSave(string id, string UID, string SiteID, string txtarea, string ddlStatuss, string LocationID)
        {
            var query = "";
            DataTable dt;
            var Result = "";
            var Message = "";
            if (LocationID == "")
            {
                //GridEditableItem item = (GridEditableItem)e.Item;
                //GridDataItem parentitem = e.Item.OwnerTableView.ParentItem;
                //SiteID = parentitem.OwnerTableView.DataKeyValues[parentitem.ItemIndex]["ID"].ToString();
                //RadTextBox txtarea = (RadTextBox)item.FindControl("txtarea");
                //RadComboBox ddlStatuss = (RadComboBox)item.FindControl("ddlStatuss");


                //   query = "exec Pro_TBLAREA_SAVE '0','" + txtarea.Text.Replace("'", "''").Trim() + "','" + HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["id"])) + "','" + HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["UID"])) + "','" + DateTime.Now + "','" + ddlStatuss.SelectedValue.Replace("'", "''").Trim() + "','" + SiteID + "'";
                query = "exec Pro_TBLAREA_SAVE '0','" + txtarea.Replace("'", "''").Trim() + "','" + databaseCon.Decrypt(id) + "','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "','" + ddlStatuss.Replace("'", "''").Trim() + "','" + SiteID + "'";
                dt = databaseCon.GetData(query);
                if (dt.Rows.Count != 0)
                {
                    if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                    {
                        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Success";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Success ! </strong>" + dt.Rows[0][1].ToString();
                        //e.Item.OwnerTableView.Rebind();
                        //gridsitelocation.MasterTableView.Rebind();

                        Result = "Success";
                        Message = dt.Rows[0][1].ToString();
                    }
                    else if (Convert.ToInt32(dt.Rows[0][0]) == -1)
                    {
                        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                        Result = "Failed";
                        Message = dt.Rows[0][1].ToString();
                    }
                    else
                    {
                        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                        Result = "Failed";
                        Message = dt.Rows[0][1].ToString();
                    }

                }
                else
                {
                    //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator ";
                    Result = "Failed";
                    Message = "Syntax Error. Contact administrator .";
                }
            }
            else
            {
                //GridEditableItem item = (GridEditableItem)e.Item;
                //GridDataItem parentitem = e.Item.OwnerTableView.ParentItem;
                //string areaid = item.GetDataKeyValue("ID").ToString();
                //SiteID = parentitem.OwnerTableView.DataKeyValues[parentitem.ItemIndex]["ID"].ToString();
                //RadTextBox txtarea = (RadTextBox)item.FindControl("txtarea");
                //RadComboBox ddlStatuss = (RadComboBox)item.FindControl("ddlStatuss");

                //query = "exec Pro_TBLAREA_SAVE '" + areaid + "','" + txtarea.Text.Replace("'", "''").Trim() + "','" + HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["id"])) + "','" + HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["UID"])) + "','" + DateTime.Now + "','" + ddlStatuss.SelectedValue.Replace("'", "''").Trim() + "','" + SiteID + "'";

                query = "exec Pro_TBLAREA_SAVE '" + LocationID + "','" + txtarea.Replace("'", "''").Trim() + "','" + databaseCon.Decrypt(id) + "','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "','" + ddlStatuss.Replace("'", "''").Trim() + "','" + SiteID + "'";
                dt = databaseCon.GetData(query);
                if (dt.Rows.Count != 0)
                {
                    if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                    {
                        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Success";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Success ! </strong>" + dt.Rows[0][1].ToString();
                        //e.Item.OwnerTableView.Rebind();
                        //gridsitelocation.MasterTableView.Rebind();
                        Result = "Success";
                        Message = dt.Rows[0][1].ToString();

                    }
                    else if (Convert.ToInt32(dt.Rows[0][0]) == -1)
                    {
                        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                        Result = "Failed";
                        Message = dt.Rows[0][1].ToString();
                    }
                    else
                    {
                        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                        Result = "Failed";
                        Message = dt.Rows[0][1].ToString();
                    }

                }
                else
                {
                    //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator ";
                    Result = "Failed";
                    Message = "Syntax Error. Contact administrator .";
                }

            }
            return Json(new { Result = Result, Message = Message });
        }

        public ActionResult GetBuildingData(string id, string AreaLocaID)
        {

            var GetData = dbcontext.Database.SqlQuery<Pro_TBLBuilding_GETViewModel>("exec[Pro_TBLBuilding_GET] '', " + databaseCon.Decrypt(id) + ", " + AreaLocaID + "").ToList();
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BuildingSave(string id, string UID, string AreaID, string txtBuilding, string ddlStatuss1, string buildingid)
        {
            var query = "";
            DataTable dt;
            var Result = "";
            var Message = "";

            if (buildingid == "")
            {

                //GridEditableItem item = (GridEditableItem)e.Item;
                //GridDataItem parentitem1 = e.Item.OwnerTableView.ParentItem;
                //AreaID = parentitem1.OwnerTableView.DataKeyValues[parentitem1.ItemIndex]["ID"].ToString();
                //RadTextBox txtBuilding = (RadTextBox)item.FindControl("txtBuilding");
                //RadComboBox ddlStatuss1 = (RadComboBox)item.FindControl("ddlStatuss1");
                // query = "exec Pro_TBLBuilding_SAVE '0','" + txtBuilding.Text.Replace("'", "''").Trim() + "','" + ddlStatuss1.SelectedValue.Replace("'", "''").Trim() + "','" + HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["id"])) + "','" + AreaID + "','" + HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["UID"])) + "','" + DateTime.Now + "'";
                query = "exec Pro_TBLBuilding_SAVE '0','" + txtBuilding.Replace("'", "''").Trim() + "','" + ddlStatuss1.Replace("'", "''").Trim() + "','" + databaseCon.Decrypt(id) + "','" + AreaID + "','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "'";
                dt = databaseCon.GetData(query);
                if (dt.Rows.Count != 0)
                {
                    if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                    {
                        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Success";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Success ! </strong>" + dt.Rows[0][1].ToString();
                        //e.Item.OwnerTableView.Rebind();
                        //gridsitelocation.MasterTableView.Rebind();

                        Result = "Success";
                        Message = dt.Rows[0][1].ToString();
                    }
                    else if (Convert.ToInt32(dt.Rows[0][0]) == -1)
                    {
                        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();

                        Result = "Failed";
                        Message = dt.Rows[0][1].ToString();
                    }
                    else
                    {
                        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();

                        Result = "Failed";
                        //Message = dt.Rows[0][1].ToString();
                        Message = "Location cannot be blank ";
                    }

                }
                else
                {
                    //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator ";

                    Result = "Failed";
                    Message = "Syntax Error. Contact administrator ";
                }
            }
            else
            {
                //GridEditableItem item = (GridEditableItem)e.Item;
                //GridDataItem parentitem1 = e.Item.OwnerTableView.ParentItem;
                //string buildingid = item.GetDataKeyValue("ID").ToString();
                //AreaID = parentitem1.OwnerTableView.DataKeyValues[parentitem1.ItemIndex]["ID"].ToString();
                //RadTextBox txtBuilding = (RadTextBox)item.FindControl("txtBuilding");
                //RadComboBox ddlStatuss1 = (RadComboBox)item.FindControl("ddlStatuss1");

                // query = "exec Pro_TBLBuilding_SAVE  '" + buildingid + "','" + txtBuilding.Text.Replace("'", "''").Trim() + "','" + ddlStatuss1.SelectedValue.Replace("'", "''").Trim() + "','" + HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["id"])) + "','" + AreaID + "','" + HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["UID"])) + "','" + DateTime.Now + "'";
                query = "exec Pro_TBLBuilding_SAVE  '" + buildingid + "','" + txtBuilding.Replace("'", "''").Trim() + "','" + ddlStatuss1.Replace("'", "''").Trim() + "','" + databaseCon.Decrypt(id) + "','" + AreaID + "','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "'";
                dt = databaseCon.GetData(query);
                if (dt.Rows.Count != 0)
                {
                    if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                    {
                        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Success";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Success ! </strong>" + dt.Rows[0][1].ToString();
                        //e.Item.OwnerTableView.Rebind();
                        //gridsitelocation.MasterTableView.Rebind();
                        Result = "Success";
                        Message = dt.Rows[0][1].ToString();

                    }
                    else if (Convert.ToInt32(dt.Rows[0][0]) == -1)
                    {
                        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                        Result = "Failed";
                        Message = dt.Rows[0][1].ToString();
                    }
                    else
                    {
                        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                        Result = "Failed";
                        //Message = dt.Rows[0][1].ToString();
                        Message = "Location cannot be blank on register";
                    }

                }
                else
                {
                    //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator ";

                    Result = "Failed";
                    Message = "Syntax Error. Contact administrator ";
                }

            }
            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetAssetData(string id, string BuildingID)
        {

            var GetData = dbcontext.Database.SqlQuery<Pro_TBLAsset_GETViewModel>("exec [Pro_TBLAsset_GET] ''," + databaseCon.Decrypt(id) + "," + BuildingID + "").ToList();
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AssetSave(string id, string UID, string BuildingID, string txtAssetNo, string txtAssetDesc, string ddlStatuss2, string Assestid)
        {
            var query = "";
            DataTable dt;
            var Result = "";
            var Message = "";
            if (Assestid == "")
            {

                //GridEditableItem item = (GridEditableItem)e.Item;
                //GridDataItem parentitem1 = e.Item.OwnerTableView.ParentItem;
                //BuildingID = parentitem1.OwnerTableView.DataKeyValues[parentitem1.ItemIndex]["ID"].ToString();
                //RadTextBox txtAssetNo = (RadTextBox)item.FindControl("txtAssetNo");
                //RadTextBox txtAssetDesc = (RadTextBox)item.FindControl("txtAssetDesc");
                //RadComboBox ddlStatuss2 = (RadComboBox)item.FindControl("ddlStatuss2");

                // query = "exec Pro_TBLAsset_SAVE '0','" + txtAssetNo.Text.Replace("'", "''").Trim() + "','" + txtAssetDesc.Text.Replace("'", "''").Trim() + "','" + ddlStatuss2.SelectedValue.Replace("'", "''").Trim() + "','" + HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["id"])) + "','" + BuildingID + "','" + HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["UID"])) + "','" + DateTime.Now + "'";
                query = "exec Pro_TBLAsset_SAVE '0','" + txtAssetNo.Replace("'", "''").Trim() + "','" + txtAssetDesc.Replace("'", "''").Trim() + "','" + ddlStatuss2.Replace("'", "''").Trim() + "','" + databaseCon.Decrypt(id) + "','" + BuildingID + "','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "'";
                dt = databaseCon.GetData(query);
                if (dt.Rows.Count != 0)
                {
                    if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                    {
                        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Success";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Success ! </strong>" + dt.Rows[0][1].ToString();
                        //e.Item.OwnerTableView.Rebind();
                        //gridsitelocation.MasterTableView.Rebind();

                        Result = "Success";
                        Message = dt.Rows[0][1].ToString();
                    }
                    else if (Convert.ToInt32(dt.Rows[0][0]) == -1)
                    {
                        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();

                        Result = "Failed";
                        Message = dt.Rows[0][1].ToString();
                    }
                    else
                    {
                        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();

                        Result = "Failed";
                        //Message = dt.Rows[0][1].ToString();
                        //Message = "Building cannot be blank ";
                        Message = "Building cannot be blank on register";
                    }

                }
                else
                {
                    //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator ";

                    Result = "Failed";
                    Message = "Syntax Error. Contact administrator ";
                }
            }
            else
            {

                //GridEditableItem item = (GridEditableItem)e.Item;
                //GridDataItem parentitem1 = e.Item.OwnerTableView.ParentItem;
                //string Assestid = item.GetDataKeyValue("ID").ToString();
                //BuildingID = parentitem1.OwnerTableView.DataKeyValues[parentitem1.ItemIndex]["ID"].ToString();
                //RadTextBox txtAssetNo = (RadTextBox)item.FindControl("txtAssetNo");
                //RadTextBox txtAssetDesc = (RadTextBox)item.FindControl("txtAssetDesc");
                //RadComboBox ddlStatuss2 = (RadComboBox)item.FindControl("ddlStatuss2");

                // query = "exec Pro_TBLAsset_SAVE  '" + Assestid + "','" + txtAssetNo.Text.Replace("'", "''").Trim() + "','" + txtAssetDesc.Text.Replace("'", "''").Trim() + "','" + ddlStatuss2.SelectedValue.Replace("'", "''").Trim() + "','" + HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["id"])) + "','" + BuildingID + "','" + HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["UID"])) + "','" + DateTime.Now + "'";
                query = "exec Pro_TBLAsset_SAVE  '" + Assestid + "','" + txtAssetNo.Replace("'", "''").Trim() + "','" + txtAssetDesc.Replace("'", "''").Trim() + "','" + ddlStatuss2.Replace("'", "''").Trim() + "','" + databaseCon.Decrypt(id) + "','" + BuildingID + "','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "'";
                dt = databaseCon.GetData(query);
                if (dt.Rows.Count != 0)
                {
                    if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                    {
                        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Success";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Success ! </strong>" + dt.Rows[0][1].ToString();
                        //e.Item.OwnerTableView.Rebind();
                        //gridsitelocation.MasterTableView.Rebind();
                        Result = "Success";
                        Message = dt.Rows[0][1].ToString();
                    }
                    else if (Convert.ToInt32(dt.Rows[0][0]) == -1)
                    {
                        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                        Result = "Failed";
                        Message = dt.Rows[0][1].ToString();
                    }
                    else
                    {
                        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                        Result = "Failed";
                        //Message = dt.Rows[0][1].ToString();
                        Message = "Building cannot be blank on register";
                    }

                }
                else
                {
                    //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator ";

                    Result = "Failed";
                    Message = "Syntax Error. Contact administrator ";
                }

            }
            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult btndeleteyes_Click(string id, string deleteID, string DeleteFlag)
        {
            DataTable dt;
            string query;
            var Result = "";
            var Message = "";
            if (DeleteFlag == "SiteMaster")
            {
                // query = "exec Pro_TBLSITELOCATION_DEL '" + ViewState["sitelociddelete"] + "'";
                query = "exec Pro_TBLSITELOCATION_DEL '" + deleteID + "'";
                dt = databaseCon.GetData(query);
                if (dt.Rows.Count != 0)
                {
                    if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                    {

                        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Success";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Success ! </strong>" + dt.Rows[0][1].ToString();
                        //gridsitelocation.Rebind();

                        Result = "Success";
                        Message = dt.Rows[0][1].ToString();

                    }
                    else if (Convert.ToInt32(dt.Rows[0][0]) == -1)
                    {
                        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                        Result = "Failed";
                        Message = dt.Rows[0][1].ToString();
                    }
                    else
                    {
                        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                        Result = "Success";
                        Message = dt.Rows[0][1].ToString();
                    }

                }
                else
                {
                    //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator ";
                    Result = "Success";
                    Message = "Syntax Error. Contact administrator .";
                }
                //divdelete.Style.Add("display", "none");
                //gridsitelocation.Rebind();
            }

            else if (DeleteFlag == "LocationMaster")
            {
                // query = "exec Pro_TBLAREA_DEL '" + ViewState["Areafordelete"] + "'";
                query = "exec Pro_TBLAREA_DEL '" + deleteID + "'";
                dt = databaseCon.GetData(query);
                if (dt.Rows.Count != 0)
                {
                    if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                    {
                        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Success";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Success ! </strong>" + dt.Rows[0][1].ToString();
                        //gridsitelocation.Rebind();
                        Result = "Success";
                        Message = dt.Rows[0][1].ToString();
                    }
                    else if (Convert.ToInt32(dt.Rows[0][0]) == -1)
                    {
                        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                        Result = "Failed";
                        Message = dt.Rows[0][1].ToString();
                    }
                    else
                    {
                        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                        Result = "Failed";
                        Message = dt.Rows[0][1].ToString();
                    }

                }
                else
                {
                    //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator ";
                    Result = "Failed";
                    Message = "Syntax Error. Contact administrator";
                }
                //divdelete.Style.Add("display", "none");
                //gridsitelocation.Rebind();
            }
            else if (DeleteFlag == "BuildingMaster")
            {
                // query = "exec Pro_TBLBuilding_DEL '" + ViewState["Buildingfordelete"] + "'";
                query = "exec Pro_TBLBuilding_DEL '" + deleteID + "'";
                dt = databaseCon.GetData(query);
                if (dt.Rows.Count != 0)
                {
                    if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                    {
                        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Success";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Success ! </strong>" + dt.Rows[0][1].ToString();
                        //gridsitelocation.Rebind();
                        Result = "Success";
                        Message = dt.Rows[0][1].ToString();
                    }
                    else if (Convert.ToInt32(dt.Rows[0][0]) == -1)
                    {
                        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                        Result = "Failed";
                        Message = dt.Rows[0][1].ToString();

                    }
                    else
                    {
                        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                        Result = "Failed";
                        Message = dt.Rows[0][1].ToString();
                    }

                }
                else
                {
                    //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator ";
                    Result = "Success";
                    Message = "Syntax Error. Contact administrator";
                }
                //divdelete.Style.Add("display", "none");
                //gridsitelocation.Rebind();
            }
            else if (DeleteFlag == "AssestMaster")
            {
                //query = "exec Pro_TBLAsset_DEL '" + ViewState["Assetfordelete"] + "'";
                query = "exec Pro_TBLAsset_DEL '" + deleteID + "'";
                dt = databaseCon.GetData(query);
                if (dt.Rows.Count != 0)
                {
                    if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                    {
                        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Success";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Success ! </strong>" + dt.Rows[0][1].ToString();
                        //gridsitelocation.Rebind();
                        Result = "Success";
                        Message = dt.Rows[0][1].ToString();
                    }
                    else if (Convert.ToInt32(dt.Rows[0][0]) == -1)
                    {
                        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                        Result = "Failed";
                        Message = dt.Rows[0][1].ToString();
                    }
                    else
                    {
                        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                        Result = "Failed";
                        Message = dt.Rows[0][1].ToString();
                    }

                }
                else
                {
                    //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator ";
                    Result = "Failed";
                    Message = "Syntax Error. Contact administrator";
                }
                //divdelete.Style.Add("display", "none");
                //gridsitelocation.Rebind();
            }
            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PipeInventory(string SiteID, string id)
        {
            string query = "Select Clientid, SiteId from tblpipelineRequest where Clientid='" + databaseCon.Decrypt(id) + "' And SiteId = '" + SiteID + "'";
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                return Json(new { result = "Failed", Message = "Pipeline Inventory Exists For This Site Hence Disabled" });
            }
            else
            {
                return Json(new { result = "success", Message = "Check Enable Pipeline Inventory if You Wish To Enter Pipeline Inventory" });
            }
        }
        public ActionResult DisbSkipMat(string SiteID)
        {
            string qry = "select DISTINCT  NoInventory from tblRegister where SiteLocId =" + SiteID + " and NoInventory is not null and NoInventory != 0";
            var GetData = dbcontext.Database.SqlQuery<Pro_TBLREGISTER_GETViewModel>(qry).FirstOrDefault();

            return Json(GetData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult btnCheckChangeUser(string id, string txtOwnerName, string sitelocdid)
        {
            string qryCurrentOwnerName = "select  OwnerName from tblSiteLocation   where SiteLocID ='" + sitelocdid + "'";
            DataTable dtCurrentOwnerName = databaseCon.GetData(qryCurrentOwnerName);
            if (dtCurrentOwnerName.Rows.Count > 0)
            {
                var CurrentOwnerName = dtCurrentOwnerName.Rows[0]["OwnerName"].ToString();

                string qryCurrentUsers = "select string_AGG(CONCAT(firstName, ' ', LastName), ',') CurrentUser from tblUser where SiteOwnerName = '" + CurrentOwnerName.Replace("'", "''") + "' and ClientID = '" + databaseCon.Decrypt(id) + "'";
                DataTable dtCurrentUsers = databaseCon.GetData(qryCurrentUsers);
                if (dtCurrentUsers.Rows.Count > 0)
                {
                    var CurrentUsers = dtCurrentUsers.Rows[0]["CurrentUser"].ToString();

                    if (CurrentOwnerName != txtOwnerName && CurrentUsers != null && CurrentUsers != "")
                    {
                        return Json(new
                        {
                            Result = "Success",
                            Message = "[" + CurrentUsers + "] is already using the Owner Name [" + CurrentOwnerName + "]. Changing here will also update in the given user. Do you still want to Update?"
                        });

                    }
                    else
                    {
                        return Json(new { Result = "Failed", Message = "call save directly" });

                    }
                }
                else
                {
                    return Json(new { Result = "Failed", Message = "call save directly" });

                }

            }
            else
            {
                return Json(new { Result = "Failed", Message = "call save directly" });

            }

        }

        public JsonResult DeleteInactiveSiteDocuments(string clientId, int siteLocId)
        {
            clientId = databaseCon.Decrypt(clientId);

            int deleted = 0;
            List<string> errors = new List<string>();

            string docQuery = "SELECT d.DocumentId,d.DocumentName,d.RegisterID,d.SiteLocID FROM tblDocuments d LEFT JOIN tblRegister r ON d.RegisterID = r.RegisterId WHERE d.ClientID = " + clientId + " AND((d.RegisterID IS NOT NULL AND r.SiteLocId = " + siteLocId + ") OR(d.RegisterID IS NULL AND d.SiteLocID = " + siteLocId + ") OR R.SiteLocId = " + siteLocId + ") AND ISNULL(d.DocumentName,'')<> '' AND ISNULL(d.RegisterID,'')<> ''";

            //string docQuery = "SELECT d.DocumentId,d.DocumentName,d.RegisterID,d.SiteLocID FROM tblDocuments d LEFT JOIN tblRegister r ON d.RegisterID = r.RegisterId WHERE d.ClientID = " + clientId + " AND((d.RegisterID IS NOT NULL AND r.SiteLocId = " + siteLocId + ") OR(d.RegisterID IS NULL AND d.SiteLocID = " + siteLocId + ")) AND ISNULL(d.DocumentName,'')<> '' AND ISNULL(d.RegisterID,'')<> ''";


            DataTable dtDocuments = databaseCon.GetData(docQuery);

            foreach (DataRow dr in dtDocuments.Rows)
            {
                try
                {
                    int documentId = Convert.ToInt32(dr["DocumentId"]);
                    string fileName = Convert.ToString(dr["DocumentName"]);

                    if (string.IsNullOrWhiteSpace(fileName))
                    {
                        errors.Add("Document Id " + documentId + " : File Name is empty.");
                    }

                    if (!databaseCon.BlobFileExists(fileName, "registerdocument"))
                    {
                        errors.Add(fileName + " does not exist in Blob Storage.");

                        databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments "
                            + "(TableName, ColumnName, FileName, ClientID, SiteLocID, RegisterID, BlobContainer, CheckedOn, QueryName, QueryText, ErrorMessage, Reason, ErrorOn) "
                            + "VALUES ('tblDocuments', 'DocumentName', '" + (((fileName) ?? "").Replace("'", "''")) + "', "
                            + clientId + ", " + siteLocId + ", NULL, 'registerdocument', GETDATE(), "
                            + "'DeleteInactiveSiteDocuments-DocumentName', '" + docQuery.Replace("'", "''") + "', '', "
                            + "'File does not exist in Blob Storage.', GETDATE())");
                    }
                    else
                    {
                        databaseCon.deletefileonblob(fileName, "registerdocument");

                        if (databaseCon.BlobFileExists(fileName, "registerdocument"))
                        {
                            errors.Add(fileName + " could not be deleted from Blob Storage.");
                        }
                        else
                        {
                            databaseCon.RunCmd("DELETE d FROM tblDocuments d LEFT JOIN tblRegister r ON d.RegisterID=r.RegisterId WHERE d.DocumentId="
                                + documentId + " AND d.ClientID=" + clientId
                                + " AND ((d.RegisterID IS NOT NULL AND r.SiteLocId=" + siteLocId + ") "
                                + "OR (d.RegisterID IS NULL AND d.SiteLocID=" + siteLocId + "))");

                            deleted++;
                        }
                    }
                }

                catch (Exception ex)
                {
                    errors.Add("DocumentName: " + dr["DocumentName"] + " => " + ex.Message);
                }
            }

            string inspQuery = "SELECT i.InspectionID, i.InspFileName, i.InspectionDate FROM tblInspection i INNER JOIN tblRegister r ON i.RegisterID = r.RegisterId WHERE r.ClientID = " + clientId + " AND r.SiteLocId = " + siteLocId;

            DataTable dtInspection = databaseCon.GetData(inspQuery);

            foreach (DataRow dr in dtInspection.Rows)
            {
                try
                {
                    int inspectionId = Convert.ToInt32(dr["InspectionID"]);
                    string fileName = Convert.ToString(dr["InspFileName"]);
                    bool isDeleted = false;

                    DateTime inspectionDate = Convert.ToDateTime(dr["InspectionDate"]);
                    string generatedPdf = "Inspection_" + inspectionDate.ToString("dd-MMM-yyyy") + "_" + inspectionId + ".pdf";

                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        if (databaseCon.BlobFileExists(fileName, "registerdocument"))
                        {
                            databaseCon.deletefileonblob(fileName, "registerdocument");

                            if (!databaseCon.BlobFileExists(fileName, "registerdocument"))
                            {
                                isDeleted = true;
                            }
                            else
                            {
                                errors.Add(fileName + " could not be deleted from Blob Storage.");
                            }
                        }
                        else
                        {
                            errors.Add(fileName + " does not exist in Blob Storage.");

                            databaseCon.RunCmd(
                                "INSERT INTO tblMissingBlobDocuments " +
                                "(TableName, ColumnName, FileName, ClientID, SiteLocID, RegisterID, BlobContainer, CheckedOn, QueryName, QueryText, ErrorMessage, Reason, ErrorOn) " +
                                "VALUES ('tblInspection', 'InspFileName', '" + (fileName ?? "").Replace("'", "''") + "', " +
                                clientId + ", " + siteLocId + ", NULL, 'registerdocument', GETDATE(), " +
                                "'DeleteInactiveSiteDocuments-InspFileName', '" + inspQuery.Replace("'", "''") + "', '', " +
                                "'File does not exist in Blob Storage.', GETDATE())");
                        }
                    }

                    if (string.IsNullOrWhiteSpace(fileName) ||
                        !string.Equals(fileName, generatedPdf, StringComparison.OrdinalIgnoreCase))
                    {
                        if (databaseCon.BlobFileExists(generatedPdf, "registerdocument"))
                        {
                            databaseCon.deletefileonblob(generatedPdf, "registerdocument");
                        }
                    }

                    if (isDeleted)
                    {
                        databaseCon.RunCmd("UPDATE i " + "SET InspFileName = NULL " + "FROM tblInspection i " + "INNER JOIN tblRegister r ON i.RegisterID = r.RegisterId " + "WHERE i.InspectionID = " + inspectionId + " AND r.ClientID = " + clientId + " AND r.SiteLocId = " + siteLocId);

                        deleted++;
                    }
                }
                catch (Exception ex)
                {
                    errors.Add("InspFileName: " + Convert.ToString(dr["InspFileName"]) + " => " + ex.Message);
                }
            }

            string DayDocket = "SELECT h.DayDocketID FROM tblDayDocket h INNER JOIN tblRegister r ON h.SiteLocId=r.SiteLocId WHERE r.ClientID=" + clientId + " AND h.SiteLocId=" + siteLocId;

            DataTable dtDayDocket = databaseCon.GetData(DayDocket);

            foreach (DataRow dr in dtDayDocket.Rows)
            {
                string pdfFileName = null;

                try
                {
                    int DayDocketId = Convert.ToInt32(dr["DayDocketID"]);
                    pdfFileName = "DayDocket_" + DayDocketId + ".pdf";

                    if (!databaseCon.BlobFileExists(pdfFileName, "registerdocument"))
                    {
                        errors.Add(pdfFileName + " DayDocket PDF not found in Blob.");
                        databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments "
            + "(TableName, ColumnName, FileName, ClientID, SiteLocID, RegisterID, BlobContainer, CheckedOn, QueryName, QueryText, ErrorMessage, Reason, ErrorOn) "
            + "VALUES ('tblDayDocket', 'DayDocketPdf', '" + (((pdfFileName) ?? "").Replace("'", "''")) + "', " + clientId + ", " + siteLocId + ", NULL, 'registerdocument', GETDATE(), 'DeleteInactiveSiteDocuments-DayDocketPdf', '" + DayDocket.Replace("'", "''") + "', '', 'DayDocket PDF not found in Blob.', GETDATE())");
                    }
                    else
                    {
                        databaseCon.deletefileonblob(pdfFileName, "registerdocument");

                        if (databaseCon.BlobFileExists(pdfFileName, "registerdocument"))
                        {
                            errors.Add(pdfFileName + " could not be deleted from Blob Storage.");
                        }
                        else
                        {
                            deleted++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    errors.Add(ex.Message);
                }
            }

            string handOverQuery = "SELECT h.HandOverId,h.HandOverDate FROM tblHandOver h INNER JOIN tblRegister r ON h.RegisterId=r.RegisterId WHERE r.ClientID=" + clientId + " AND r.SiteLocId=" + siteLocId;

            DataTable dtHandOver = databaseCon.GetData(handOverQuery);

            foreach (DataRow dr in dtHandOver.Rows)
            {
                try
                {
                    int handOverId = Convert.ToInt32(dr["HandOverId"]);
                    DateTime handOverDate = Convert.ToDateTime(dr["HandOverDate"]);

                    string pdfFileName = "HandOver_" + handOverDate.ToString("dd-MMM-yyyy") + "_" + handOverId + ".pdf";

                    // Delete only if the file exists in Blob
                    if (databaseCon.BlobFileExists(pdfFileName, "registerdocument"))
                    {
                        databaseCon.deletefileonblob(pdfFileName, "registerdocument");

                        if (databaseCon.BlobFileExists(pdfFileName, "registerdocument"))
                        {
                            errors.Add(pdfFileName + " could not be deleted from Blob Storage.");
                        }
                        else
                        {
                            deleted++;
                        }
                    }

                    // If the generated PDF does not exist in Blob,
                    // do nothing (no error, no tblMissingBlobDocuments entry).
                }
                catch (Exception ex)
                {
                    errors.Add(ex.Message);
                }
            }

            if (errors.Count > 0)
            {
                return Json(new
                {
                    Result = "Warning",
                    Deleted = deleted,
                    Message = string.Join("\n", errors)
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                Result = "Success",
                Deleted = deleted,
                Message = deleted + " documents deleted successfully from Blob and Database."
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteSiteImages(string clientId, int siteLocId)
        {
            clientId = databaseCon.Decrypt(clientId);
            int deleted = 0;
            int failed = 0;
            List<string> errors = new List<string>();

            string handOverQuery = "SELECT h.HandOverId, h.HandOverImage, h.ContractorSign, h.RepresentativeSign, h.HandoverMapImage FROM tblHandOver h INNER JOIN tblRegister r ON h.RegisterId=r.RegisterId WHERE r.ClientID=" + clientId + " AND r.SiteLocId=" + siteLocId + " AND (ISNULL(h.HandOverImage,'')<>'' OR ISNULL(h.ContractorSign,'')<>'' OR ISNULL(h.RepresentativeSign,'')<>'' OR ISNULL(h.HandoverMapImage,'')<>'')";
            DataTable dtHandOver = databaseCon.GetData(handOverQuery);

            foreach (DataRow dr in dtHandOver.Rows)
            {
                int handOverId = Convert.ToInt32(dr["HandOverId"]);

                try
                {
                    string fileName = dr["HandOverImage"].ToString().Trim();

                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        if (!databaseCon.BlobFileExists(fileName, "siteimage"))
                        {
                            errors.Add(fileName + " does not exist in Blob Storage.");
                            databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments "
            + "(TableName, ColumnName, FileName, ClientID, SiteLocID, RegisterID, BlobContainer, CheckedOn, QueryName, QueryText, ErrorMessage, Reason, ErrorOn) "
            + "VALUES ('tblHandOver', 'HandOverImage', '" + (((fileName) ?? "").Replace("'", "''")) + "', " + clientId + ", " + siteLocId + ", " + "NULL" + ", 'siteimage', GETDATE(), 'DeleteSiteImages-HandOverImage', '" + handOverQuery.Replace("'", "''") + "', '" + (((null) ?? "").Replace("'", "''")) + "', 'File does not exist in Blob Storage.', GETDATE())");
                        }
                        else
                        {
                            databaseCon.deletefileonblob(fileName, "siteimage");

                            if (databaseCon.BlobFileExists(fileName, "siteimage"))
                            {
                                errors.Add(fileName + " could not be deleted from Blob Storage.");
                            }
                            else
                            {
                                databaseCon.RunCmd("UPDATE h SET HandOverImage=NULL FROM tblHandOver h INNER JOIN tblRegister r ON h.RegisterId=r.RegisterId WHERE h.HandOverId=" + handOverId + " AND r.ClientID=" + clientId + " AND r.SiteLocId=" + siteLocId);
                                deleted++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    failed++;
                    errors.Add("HandOverImage: " + dr["HandOverImage"] + " => " + ex.Message);
                }

                try
                {
                    string fileName = dr["ContractorSign"].ToString().Trim();

                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        if (!databaseCon.BlobFileExists(fileName, "siteimage"))
                        {
                            errors.Add(fileName + " does not exist in Blob Storage.");
                            databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments "
            + "(TableName, ColumnName, FileName, ClientID, SiteLocID, RegisterID, BlobContainer, CheckedOn, QueryName, QueryText, ErrorMessage, Reason, ErrorOn) "
            + "VALUES ('tblHandOver', 'ContractorSign', '" + (((fileName) ?? "").Replace("'", "''")) + "', " + clientId + ", " + siteLocId + ", " + "NULL" + ", 'siteimage', GETDATE(), 'DeleteSiteImages-ContractorSign', '" + handOverQuery.Replace("'", "''") + "', '" + (((null) ?? "").Replace("'", "''")) + "', 'File does not exist in Blob Storage.', GETDATE())");
                        }
                        else
                        {
                            databaseCon.deletefileonblob(fileName, "siteimage");

                            if (databaseCon.BlobFileExists(fileName, "siteimage"))
                            {
                                errors.Add(fileName + " could not be deleted from Blob Storage.");
                            }
                            else
                            {
                                databaseCon.RunCmd("UPDATE h SET ContractorSign=NULL FROM tblHandOver h INNER JOIN tblRegister r ON h.RegisterId=r.RegisterId WHERE h.HandOverId=" + handOverId + " AND r.ClientID=" + clientId + " AND r.SiteLocId=" + siteLocId);
                                deleted++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    failed++;
                    errors.Add("ContractorSign: " + dr["ContractorSign"] + " => " + ex.Message);
                }

                try
                {
                    string fileName = dr["RepresentativeSign"].ToString().Trim();

                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        if (!databaseCon.BlobFileExists(fileName, "siteimage"))
                        {
                            errors.Add(fileName + " does not exist in Blob Storage.");
                            databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments "
            + "(TableName, ColumnName, FileName, ClientID, SiteLocID, RegisterID, BlobContainer, CheckedOn, QueryName, QueryText, ErrorMessage, Reason, ErrorOn) "
            + "VALUES ('tblHandOver', 'RepresentativeSign', '" + (((fileName) ?? "").Replace("'", "''")) + "', " + clientId + ", " + siteLocId + ", " + "NULL" + ", 'siteimage', GETDATE(), 'DeleteSiteImages-RepresentativeSign', '" + handOverQuery.Replace("'", "''") + "', '" + (((null) ?? "").Replace("'", "''")) + "', 'File does not exist in Blob Storage.', GETDATE())");
                        }
                        else
                        {
                            databaseCon.deletefileonblob(fileName, "siteimage");

                            if (databaseCon.BlobFileExists(fileName, "siteimage"))
                            {
                                errors.Add(fileName + " could not be deleted from Blob Storage.");
                            }
                            else
                            {
                                databaseCon.RunCmd("UPDATE h SET RepresentativeSign=NULL FROM tblHandOver h INNER JOIN tblRegister r ON h.RegisterId=r.RegisterId WHERE h.HandOverId=" + handOverId + " AND r.ClientID=" + clientId + " AND r.SiteLocId=" + siteLocId);
                                deleted++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    failed++;
                    errors.Add("RepresentativeSign: " + dr["RepresentativeSign"] + " => " + ex.Message);
                }

                try
                {
                    string fileName = dr["HandoverMapImage"].ToString().Trim();

                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        if (!databaseCon.BlobFileExists(fileName, "siteimage"))
                        {
                            errors.Add(fileName + " does not exist in Blob Storage.");
                            databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments "
            + "(TableName, ColumnName, FileName, ClientID, SiteLocID, RegisterID, BlobContainer, CheckedOn, QueryName, QueryText, ErrorMessage, Reason, ErrorOn) "
            + "VALUES ('tblHandOver', 'HandoverMapImage', '" + (((fileName) ?? "").Replace("'", "''")) + "', " + clientId + ", " + siteLocId + ", " + "NULL" + ", 'siteimage', GETDATE(), 'DeleteSiteImages-HandoverMapImage', '" + handOverQuery.Replace("'", "''") + "', '" + (((null) ?? "").Replace("'", "''")) + "', 'File does not exist in Blob Storage.', GETDATE())");
                        }
                        else
                        {
                            databaseCon.deletefileonblob(fileName, "siteimage");

                            if (databaseCon.BlobFileExists(fileName, "siteimage"))
                            {
                                errors.Add(fileName + " could not be deleted from Blob Storage.");
                            }
                            else
                            {
                                databaseCon.RunCmd("UPDATE h SET HandoverMapImage=NULL FROM tblHandOver h INNER JOIN tblRegister r ON h.RegisterId=r.RegisterId WHERE h.HandOverId=" + handOverId + " AND r.ClientID=" + clientId + " AND r.SiteLocId=" + siteLocId);
                                deleted++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    failed++;
                    errors.Add("HandoverMapImage: " + dr["HandoverMapImage"] + " => " + ex.Message);
                }
            }

            string galleryQuery = "SELECT g.RegisterId, g.FileName FROM tblRegisterGallery g INNER JOIN tblRegister r ON g.RegisterId = r.RegisterId WHERE r.ClientID=" + clientId + " AND r.SiteLocId=" + siteLocId + " AND ISNULL(g.FileName,'')<>''";
            DataTable dtGallery = databaseCon.GetData(galleryQuery);

            foreach (DataRow dr in dtGallery.Rows)
            {
                int registerId = 0;
                try
                {
                    registerId = Convert.ToInt32(dr["RegisterId"]);
                    string fileName = dr["FileName"] == DBNull.Value ? "" : dr["FileName"].ToString().Trim();

                    if (fileName.Contains("/"))
                    {
                        fileName = fileName.Substring(fileName.LastIndexOf('/') + 1);
                    }

                    if (string.IsNullOrWhiteSpace(fileName))
                    {
                        errors.Add("Register Id " + registerId + " : File Name is empty.");
                    }
                    else if (!databaseCon.BlobFileExists(fileName, "siteimage"))
                    {
                        errors.Add(fileName + " does not exist in Blob Storage.");
                        databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments "
            + "(TableName, ColumnName, FileName, ClientID, SiteLocID, RegisterID, BlobContainer, CheckedOn, QueryName, QueryText, ErrorMessage, Reason, ErrorOn) "
            + "VALUES ('tblRegisterGallery', 'FileName', '" + (((fileName) ?? "").Replace("'", "''")) + "', " + clientId + ", " + siteLocId + ", " + registerId.ToString() + ", 'siteimage', GETDATE(), 'DeleteSiteImages-Gallery', '" + galleryQuery.Replace("'", "''") + "', '" + (((null) ?? "").Replace("'", "''")) + "', 'File does not exist in Blob Storage.', GETDATE())");
                    }
                    else
                    {
                        databaseCon.deletefileonblob(fileName, "siteimage");

                        if (databaseCon.BlobFileExists(fileName, "siteimage"))
                        {
                            errors.Add(fileName + " could not be deleted from Blob Storage.");
                        }
                        else
                        {
                            databaseCon.RunCmd("DELETE g FROM tblRegisterGallery g INNER JOIN tblRegister r ON g.RegisterId=r.RegisterId WHERE g.RegisterId=" + registerId + " AND r.ClientID=" + clientId + " AND r.SiteLocId=" + siteLocId);
                            deleted++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    failed++;
                    errors.Add("Gallery: " + dr["FileName"] + " => " + ex.Message);
                }
            }

            string inspQuery = "SELECT i.InspectionID, i.Inspimage, i.Inspimage2, i.InspSign, i.InspFileName FROM tblInspection i INNER JOIN tblRegister r ON i.RegisterID=r.RegisterId WHERE r.ClientID=" + clientId + " AND r.SiteLocId=" + siteLocId + " AND (ISNULL(i.Inspimage,'')<>'' OR ISNULL(i.Inspimage2,'')<>'' OR ISNULL(i.InspSign,'')<>'' OR ISNULL(i.InspFileName,'')<>'')";
            DataTable dtInspection = databaseCon.GetData(inspQuery);

            foreach (DataRow dr in dtInspection.Rows)
            {
                int inspectionId = Convert.ToInt32(dr["InspectionID"]);

                try
                {
                    string fileName = dr["Inspimage"].ToString().Trim();

                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        if (!databaseCon.BlobFileExists(fileName, "siteimage"))
                        {
                            errors.Add(fileName + " does not exist in Blob Storage.");
                            databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments "
            + "(TableName, ColumnName, FileName, ClientID, SiteLocID, RegisterID, BlobContainer, CheckedOn, QueryName, QueryText, ErrorMessage, Reason, ErrorOn) "
            + "VALUES ('tblInspection', 'Inspimage', '" + (((fileName) ?? "").Replace("'", "''")) + "', " + clientId + ", " + siteLocId + ", " + "NULL" + ", 'siteimage', GETDATE(), 'DeleteSiteImages-Inspimage', '" + inspQuery.Replace("'", "''") + "', '" + (((null) ?? "").Replace("'", "''")) + "', 'File does not exist in Blob Storage.', GETDATE())");
                        }
                        else
                        {
                            databaseCon.deletefileonblob(fileName, "siteimage");

                            if (databaseCon.BlobFileExists(fileName, "siteimage"))
                            {
                                errors.Add(fileName + " could not be deleted from Blob Storage.");
                            }
                            else
                            {
                                databaseCon.RunCmd("UPDATE i SET Inspimage=NULL FROM tblInspection i INNER JOIN tblRegister r ON i.RegisterID=r.RegisterId WHERE i.InspectionID=" + inspectionId + " AND r.ClientID=" + clientId + " AND r.SiteLocId=" + siteLocId);
                                deleted++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    failed++;
                    errors.Add("Inspimage: " + dr["Inspimage"] + " => " + ex.Message);
                }

                try
                {
                    string fileName = dr["Inspimage2"].ToString().Trim();

                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        if (!databaseCon.BlobFileExists(fileName, "siteimage"))
                        {
                            errors.Add(fileName + " does not exist in Blob Storage.");
                            databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments "
            + "(TableName, ColumnName, FileName, ClientID, SiteLocID, RegisterID, BlobContainer, CheckedOn, QueryName, QueryText, ErrorMessage, Reason, ErrorOn) "
            + "VALUES ('tblInspection', 'Inspimage2', '" + (((fileName) ?? "").Replace("'", "''")) + "', " + clientId + ", " + siteLocId + ", " + "NULL" + ", 'siteimage', GETDATE(), 'DeleteSiteImages-Inspimage2', '" + inspQuery.Replace("'", "''") + "', '" + (((null) ?? "").Replace("'", "''")) + "', 'File does not exist in Blob Storage.', GETDATE())");
                        }
                        else
                        {
                            databaseCon.deletefileonblob(fileName, "siteimage");

                            if (databaseCon.BlobFileExists(fileName, "siteimage"))
                            {
                                errors.Add(fileName + " could not be deleted from Blob Storage.");
                            }
                            else
                            {
                                databaseCon.RunCmd("UPDATE i SET Inspimage2=NULL FROM tblInspection i INNER JOIN tblRegister r ON i.RegisterID=r.RegisterId WHERE i.InspectionID=" + inspectionId + " AND r.ClientID=" + clientId + " AND r.SiteLocId=" + siteLocId);
                                deleted++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    failed++;
                    errors.Add("Inspimage2: " + dr["Inspimage2"] + " => " + ex.Message);
                }

                try
                {
                    string fileName = dr["InspSign"].ToString().Trim();

                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        if (!databaseCon.BlobFileExists(fileName, "siteimage"))
                        {
                            errors.Add(fileName + " does not exist in Blob Storage.");
                            databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments "
            + "(TableName, ColumnName, FileName, ClientID, SiteLocID, RegisterID, BlobContainer, CheckedOn, QueryName, QueryText, ErrorMessage, Reason, ErrorOn) "
            + "VALUES ('tblInspection', 'InspSign', '" + (((fileName) ?? "").Replace("'", "''")) + "', " + clientId + ", " + siteLocId + ", " + "NULL" + ", 'siteimage', GETDATE(), 'DeleteSiteImages-InspSign', '" + inspQuery.Replace("'", "''") + "', '" + (((null) ?? "").Replace("'", "''")) + "', 'File does not exist in Blob Storage.', GETDATE())");
                        }
                        else
                        {
                            databaseCon.deletefileonblob(fileName, "siteimage");

                            if (databaseCon.BlobFileExists(fileName, "siteimage"))
                            {
                                errors.Add(fileName + " could not be deleted from Blob Storage.");
                            }
                            else
                            {
                                databaseCon.RunCmd("UPDATE i SET InspSign=NULL FROM tblInspection i INNER JOIN tblRegister r ON i.RegisterID=r.RegisterId WHERE i.InspectionID=" + inspectionId + " AND r.ClientID=" + clientId + " AND r.SiteLocId=" + siteLocId);
                                deleted++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    failed++;
                    errors.Add("InspSign: " + dr["InspSign"] + " => " + ex.Message);
                }

                //    try
                //    {
                //        string fileName = dr["InspFileName"].ToString().Trim();

                //        if (!string.IsNullOrWhiteSpace(fileName))
                //        {
                //            if (!databaseCon.BlobFileExists(fileName, "siteimage"))
                //            {
                //                errors.Add(fileName + " does not exist in Blob Storage.");
                //                databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments "
                //+ "(TableName, ColumnName, FileName, ClientID, SiteLocID, RegisterID, BlobContainer, CheckedOn, QueryName, QueryText, ErrorMessage, Reason, ErrorOn) "
                //+ "VALUES ('tblInspection', 'InspFileName', '" + (((fileName) ?? "").Replace("'", "''")) + "', " + clientId + ", " + siteLocId + ", " + "NULL" + ", 'siteimage', GETDATE(), 'DeleteSiteImages-InspFileName', '" + inspQuery.Replace("'", "''") + "', '" + (((null) ?? "").Replace("'", "''")) + "', 'File does not exist in Blob Storage.', GETDATE())");
                //            }
                //            else
                //            {
                //                databaseCon.deletefileonblob(fileName, "siteimage");

                //                if (databaseCon.BlobFileExists(fileName, "siteimage"))
                //                {
                //                    errors.Add(fileName + " could not be deleted from Blob Storage.");
                //                }
                //                else
                //                {
                //                    databaseCon.RunCmd("UPDATE i SET InspFileName=NULL FROM tblInspection i INNER JOIN tblRegister r ON i.RegisterID=r.RegisterId WHERE i.InspectionID=" + inspectionId + " AND r.ClientID=" + clientId + " AND r.SiteLocId=" + siteLocId);
                //                    deleted++;
                //                }
                //            }
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        failed++;
                //        errors.Add("InspFileName: " + dr["InspFileName"] + " => " + ex.Message);
                //    }
            }

            string dayDocketQuery = "SELECT d.DayDocketId, d.SupervisorSign, d.ClientSign FROM tblDayDocket d WHERE d.SiteLocId=" + siteLocId + " AND (ISNULL(d.SupervisorSign,'')<>'' OR ISNULL(d.ClientSign,'')<>'')";
            DataTable dtDayDocket = databaseCon.GetData(dayDocketQuery);

            foreach (DataRow dr in dtDayDocket.Rows)
            {
                int dayDocketId = Convert.ToInt32(dr["DayDocketId"]);

                try
                {
                    string fileName = dr["SupervisorSign"].ToString().Trim();

                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        if (!databaseCon.BlobFileExists(fileName, "siteimage"))
                        {
                            errors.Add(fileName + " does not exist in Blob Storage.");
                            databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments "
            + "(TableName, ColumnName, FileName, ClientID, SiteLocID, RegisterID, BlobContainer, CheckedOn, QueryName, QueryText, ErrorMessage, Reason, ErrorOn) "
            + "VALUES ('tblDayDocket', 'SupervisorSign', '" + (((fileName) ?? "").Replace("'", "''")) + "', " + clientId + ", " + siteLocId + ", " + "NULL" + ", 'siteimage', GETDATE(), 'DeleteSiteImages-DayDocket-SupervisorSign', '" + dayDocketQuery.Replace("'", "''") + "', '" + (((null) ?? "").Replace("'", "''")) + "', 'File does not exist in Blob Storage.', GETDATE())");
                        }
                        else
                        {
                            databaseCon.deletefileonblob(fileName, "siteimage");

                            if (databaseCon.BlobFileExists(fileName, "siteimage"))
                            {
                                errors.Add(fileName + " could not be deleted from Blob Storage.");
                            }
                            else
                            {
                                databaseCon.RunCmd("UPDATE tblDayDocket SET SupervisorSign=NULL WHERE DayDocketId=" + dayDocketId + " AND SiteLocId=" + siteLocId);
                                deleted++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    failed++;
                    errors.Add("DayDocket-SupervisorSign: " + dr["SupervisorSign"] + " => " + ex.Message);
                }

                try
                {
                    string fileName = dr["ClientSign"].ToString().Trim();

                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        if (!databaseCon.BlobFileExists(fileName, "siteimage"))
                        {
                            errors.Add(fileName + " does not exist in Blob Storage.");
                            databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments "
            + "(TableName, ColumnName, FileName, ClientID, SiteLocID, RegisterID, BlobContainer, CheckedOn, QueryName, QueryText, ErrorMessage, Reason, ErrorOn) "
            + "VALUES ('tblDayDocket', 'ClientSign', '" + (((fileName) ?? "").Replace("'", "''")) + "', " + clientId + ", " + siteLocId + ", " + "NULL" + ", 'siteimage', GETDATE(), 'DeleteSiteImages-DayDocket-ClientSign', '" + dayDocketQuery.Replace("'", "''") + "', '" + (((null) ?? "").Replace("'", "''")) + "', 'File does not exist in Blob Storage.', GETDATE())");
                        }
                        else
                        {
                            databaseCon.deletefileonblob(fileName, "siteimage");

                            if (databaseCon.BlobFileExists(fileName, "siteimage"))
                            {
                                errors.Add(fileName + " could not be deleted from Blob Storage.");
                            }
                            else
                            {
                                databaseCon.RunCmd("UPDATE tblDayDocket SET ClientSign=NULL WHERE DayDocketId=" + dayDocketId + " AND SiteLocId=" + siteLocId);
                                deleted++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    failed++;
                    errors.Add("DayDocket-ClientSign: " + dr["ClientSign"] + " => " + ex.Message);
                }
            }

            string docQuery = "SELECT d.DocumentId,d.DocumentName,d.ClientID,d.SiteLocID,d.RegisterID FROM tblDocuments d LEFT JOIN tblRegister r ON d.RegisterID=r.RegisterId WHERE d.ClientID=" + clientId + " AND ((d.RegisterID IS NOT NULL AND r.SiteLocId=" + siteLocId + ") OR (d.RegisterID IS NULL AND d.SiteLocID=" + siteLocId + ")) AND ISNULL(d.DocumentName,'')<>'' AND ISNULL(d.SiteLocID,'')<>''";
            DataTable dtDocuments = databaseCon.GetData(docQuery);

            foreach (DataRow dr in dtDocuments.Rows)
            {
                int documentId = 0;
                try
                {
                    documentId = Convert.ToInt32(dr["DocumentId"]);
                    string fileName = Convert.ToString(dr["DocumentName"]);

                    if (string.IsNullOrWhiteSpace(fileName))
                    {
                        errors.Add("Document Id " + documentId + " : File Name is empty.");
                    }

                    else if (!databaseCon.BlobFileExists(fileName, "siteimage"))
                    {
                        errors.Add(fileName + " does not exist in Blob Storage.");
                        databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments "
            + "(TableName, ColumnName, FileName, ClientID, SiteLocID, RegisterID, BlobContainer, CheckedOn, QueryName, QueryText, ErrorMessage, Reason, ErrorOn) "
            + "VALUES ('tblDocuments', 'DocumentName', '" + (((fileName) ?? "").Replace("'", "''")) + "', " + clientId + ", " + siteLocId + ", " + "NULL" + ", 'siteimage', GETDATE(), 'DeleteSiteImages-Documents', '" + docQuery.Replace("'", "''") + "', '" + (((null) ?? "").Replace("'", "''")) + "', 'File does not exist in Blob Storage.', GETDATE())");
                    }
                    else
                    {
                        databaseCon.deletefileonblob(fileName, "siteimage");

                        if (databaseCon.BlobFileExists(fileName, "siteimage"))
                        {
                            errors.Add(fileName + " could not be deleted from Blob Storage.");
                        }
                        else
                        {
                            databaseCon.RunCmd("DELETE d FROM tblDocuments d LEFT JOIN tblRegister r ON d.RegisterID=r.RegisterId WHERE d.DocumentId=" + documentId + " AND d.ClientID=" + clientId + " AND ((d.RegisterID IS NOT NULL AND r.SiteLocId=" + siteLocId + ") OR (d.RegisterID IS NULL AND d.SiteLocID=" + siteLocId + "))");
                            deleted++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    failed++;
                    errors.Add("DocumentName: " + dr["DocumentName"] + " => " + ex.Message);
                }
            }

            string deliveryDocketQuery = "SELECT dd.DelDocId, dd.DepotPersonSign, dd.TransportDriverSign, dd.SitePersonSign, dd.AcceptorSign, dd.DepotPic, dd.SitePic FROM tblTransaction t INNER JOIN tblDeliveryDocket dd ON t.DeliveryId=dd.DeliveryID AND t.ClientId = dd.ClientId WHERE t.ClientID=" + clientId + " AND ((t.[From]='Site' AND t.FromId=" + siteLocId + ") OR (t.[To]='Site' AND t.ToId=" + siteLocId + ")) AND (ISNULL(dd.DepotPersonSign,'')<>'' OR ISNULL(dd.TransportDriverSign,'')<>'' OR ISNULL(dd.SitePersonSign,'')<>'' OR ISNULL(dd.AcceptorSign,'')<>'' OR ISNULL(dd.DepotPic,'')<>'' OR ISNULL(dd.SitePic,'')<>'') Union SELECT dd.DelDocId, dd.DepotPersonSign, dd.TransportDriverSign, dd.SitePersonSign, dd.AcceptorSign, dd.DepotPic, dd.SitePic FROM tblTransaction t INNER JOIN tblDeliveryDocket dd ON t.DeliveryId = dd.DeliveryID and t.ClientId = dd.ClientId WHERE t.ClientID = " + clientId + " AND(t.[From] = 'Depot' AND t.[To] = 'ThirdParty') AND(ISNULL(dd.DepotPersonSign, '') <> '' OR ISNULL(dd.TransportDriverSign, '') <> '' OR ISNULL(dd.SitePersonSign, '') <> '' OR ISNULL(dd.AcceptorSign, '') <> '' OR ISNULL(dd.DepotPic, '') <> '' OR ISNULL(dd.SitePic, '') <> '')";
            DataTable dtDeliveryDocket = databaseCon.GetData(deliveryDocketQuery);

            foreach (DataRow dr in dtDeliveryDocket.Rows)
            {
                int delDocId = Convert.ToInt32(dr["DelDocId"]);

                try
                {
                    string fileName = dr["DepotPersonSign"].ToString().Trim();

                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        if (!databaseCon.BlobFileExists(fileName, "siteimage"))
                        {
                            errors.Add(fileName + " does not exist in Blob Storage.");
                            databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments "
            + "(TableName, ColumnName, FileName, ClientID, SiteLocID, RegisterID, BlobContainer, CheckedOn, QueryName, QueryText, ErrorMessage, Reason, ErrorOn) "
            + "VALUES ('tblDeliveryDocket', 'DepotPersonSign', '" + (((fileName) ?? "").Replace("'", "''")) + "', " + clientId + ", " + siteLocId + ", " + "NULL" + ", 'siteimage', GETDATE(), 'DeleteSiteImages-DeliveryDocket-DepotPersonSign', '" + deliveryDocketQuery.Replace("'", "''") + "', '" + (((null) ?? "").Replace("'", "''")) + "', 'File does not exist in Blob Storage.', GETDATE())");
                        }
                        else
                        {
                            databaseCon.deletefileonblob(fileName, "siteimage");

                            if (databaseCon.BlobFileExists(fileName, "siteimage"))
                            {
                                errors.Add(fileName + " could not be deleted from Blob Storage.");
                            }
                            else
                            {
                                databaseCon.RunCmd("UPDATE dd SET DepotPersonSign=NULL FROM tblDeliveryDocket dd INNER JOIN tblTransaction t ON dd.DeliveryID=t.DeliveryId and t.ClientId = dd.ClientId WHERE dd.DelDocId=" + delDocId + " AND t.ClientID=" + clientId + " AND (((t.[From]='Site' AND t.FromId=" + siteLocId + ") OR (t.[To]='Site' AND t.ToId=" + siteLocId + ")) or (t.[From] = 'Depot' and t.[To] = 'ThirdParty'))");
                                deleted++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    failed++;
                    errors.Add("DepotPersonSign: " + dr["DepotPersonSign"] + " => " + ex.Message);
                }

                try
                {
                    string fileName = dr["TransportDriverSign"].ToString().Trim();

                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        if (!databaseCon.BlobFileExists(fileName, "siteimage"))
                        {
                            errors.Add(fileName + " does not exist in Blob Storage.");
                            databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments "
            + "(TableName, ColumnName, FileName, ClientID, SiteLocID, RegisterID, BlobContainer, CheckedOn, QueryName, QueryText, ErrorMessage, Reason, ErrorOn) "
            + "VALUES ('tblDeliveryDocket', 'TransportDriverSign', '" + (((fileName) ?? "").Replace("'", "''")) + "', " + clientId + ", " + siteLocId + ", " + "NULL" + ", 'siteimage', GETDATE(), 'DeleteSiteImages-DeliveryDocket-TransportDriverSign', '" + deliveryDocketQuery.Replace("'", "''") + "', '" + (((null) ?? "").Replace("'", "''")) + "', 'File does not exist in Blob Storage.', GETDATE())");
                        }
                        else
                        {
                            databaseCon.deletefileonblob(fileName, "siteimage");

                            if (databaseCon.BlobFileExists(fileName, "siteimage"))
                            {
                                errors.Add(fileName + " could not be deleted from Blob Storage.");
                            }
                            else
                            {
                                databaseCon.RunCmd("UPDATE dd SET TransportDriverSign=NULL FROM tblDeliveryDocket dd INNER JOIN tblTransaction t ON dd.DeliveryID=t.DeliveryId and t.ClientId = dd.ClientId WHERE dd.DelDocId=" + delDocId + " AND t.ClientID=" + clientId + " AND (((t.[From]='Site' AND t.FromId=" + siteLocId + ") OR (t.[To]='Site' AND t.ToId=" + siteLocId + ")) or (t.[From] = 'Depot' and t.[To] = 'ThirdParty'))");
                                deleted++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    failed++;
                    errors.Add("TransportDriverSign: " + dr["TransportDriverSign"] + " => " + ex.Message);
                }

                try
                {
                    string fileName = dr["SitePersonSign"].ToString().Trim();

                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        if (!databaseCon.BlobFileExists(fileName, "siteimage"))
                        {
                            errors.Add(fileName + " does not exist in Blob Storage.");
                            databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments "
            + "(TableName, ColumnName, FileName, ClientID, SiteLocID, RegisterID, BlobContainer, CheckedOn, QueryName, QueryText, ErrorMessage, Reason, ErrorOn) "
            + "VALUES ('tblDeliveryDocket', 'SitePersonSign', '" + (((fileName) ?? "").Replace("'", "''")) + "', " + clientId + ", " + siteLocId + ", " + "NULL" + ", 'siteimage', GETDATE(), 'DeleteSiteImages-DeliveryDocket-SitePersonSign', '" + deliveryDocketQuery.Replace("'", "''") + "', '" + (((null) ?? "").Replace("'", "''")) + "', 'File does not exist in Blob Storage.', GETDATE())");
                        }
                        else
                        {
                            databaseCon.deletefileonblob(fileName, "siteimage");

                            if (databaseCon.BlobFileExists(fileName, "siteimage"))
                            {
                                errors.Add(fileName + " could not be deleted from Blob Storage.");
                            }
                            else
                            {
                                databaseCon.RunCmd("UPDATE dd SET SitePersonSign=NULL FROM tblDeliveryDocket dd INNER JOIN tblTransaction t ON dd.DeliveryID=t.DeliveryId and t.ClientId = dd.ClientId WHERE dd.DelDocId=" + delDocId + " AND t.ClientID=" + clientId + " AND (((t.[From]='Site' AND t.FromId=" + siteLocId + ") OR (t.[To]='Site' AND t.ToId=" + siteLocId + ")) or (t.[From] = 'Depot' and t.[To] = 'ThirdParty'))");
                                deleted++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    failed++;
                    errors.Add("SitePersonSign: " + dr["SitePersonSign"] + " => " + ex.Message);
                }

                try
                {
                    string fileName = dr["AcceptorSign"].ToString().Trim();

                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        if (!databaseCon.BlobFileExists(fileName, "siteimage"))
                        {
                            errors.Add(fileName + " does not exist in Blob Storage.");
                            databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments "
            + "(TableName, ColumnName, FileName, ClientID, SiteLocID, RegisterID, BlobContainer, CheckedOn, QueryName, QueryText, ErrorMessage, Reason, ErrorOn) "
            + "VALUES ('tblDeliveryDocket', 'AcceptorSign', '" + (((fileName) ?? "").Replace("'", "''")) + "', " + clientId + ", " + siteLocId + ", " + "NULL" + ", 'siteimage', GETDATE(), 'DeleteSiteImages-DeliveryDocket-AcceptorSign', '" + deliveryDocketQuery.Replace("'", "''") + "', '" + (((null) ?? "").Replace("'", "''")) + "', 'File does not exist in Blob Storage.', GETDATE())");
                        }
                        else
                        {
                            databaseCon.deletefileonblob(fileName, "siteimage");

                            if (databaseCon.BlobFileExists(fileName, "siteimage"))
                            {
                                errors.Add(fileName + " could not be deleted from Blob Storage.");
                            }
                            else
                            {
                                databaseCon.RunCmd("UPDATE dd SET AcceptorSign=NULL FROM tblDeliveryDocket dd INNER JOIN tblTransaction t ON dd.DeliveryID=t.DeliveryId and t.ClientId = dd.ClientId WHERE dd.DelDocId=" + delDocId + " AND t.ClientID=" + clientId + " AND (((t.[From]='Site' AND t.FromId=" + siteLocId + ") OR (t.[To]='Site' AND t.ToId=" + siteLocId + ")) or (t.[From] = 'Depot' and t.[To] = 'ThirdParty'))");
                                deleted++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    failed++;
                    errors.Add("AcceptorSign: " + dr["AcceptorSign"] + " => " + ex.Message);
                }

                try
                {
                    string fileName = dr["DepotPic"].ToString().Trim();

                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        if (!databaseCon.BlobFileExists(fileName, "siteimage"))
                        {
                            errors.Add(fileName + " does not exist in Blob Storage.");
                            databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments "
            + "(TableName, ColumnName, FileName, ClientID, SiteLocID, RegisterID, BlobContainer, CheckedOn, QueryName, QueryText, ErrorMessage, Reason, ErrorOn) "
            + "VALUES ('tblDeliveryDocket', 'DepotPic', '" + (((fileName) ?? "").Replace("'", "''")) + "', " + clientId + ", " + siteLocId + ", " + "NULL" + ", 'siteimage', GETDATE(), 'DeleteSiteImages-DeliveryDocket-DepotPic', '" + deliveryDocketQuery.Replace("'", "''") + "', '" + (((null) ?? "").Replace("'", "''")) + "', 'File does not exist in Blob Storage.', GETDATE())");
                        }
                        else
                        {
                            databaseCon.deletefileonblob(fileName, "siteimage");

                            if (databaseCon.BlobFileExists(fileName, "siteimage"))
                            {
                                errors.Add(fileName + " could not be deleted from Blob Storage.");
                            }
                            else
                            {
                                databaseCon.RunCmd("UPDATE dd SET DepotPic=NULL FROM tblDeliveryDocket dd INNER JOIN tblTransaction t ON dd.DeliveryID=t.DeliveryId and t.ClientId = dd.ClientId WHERE dd.DelDocId=" + delDocId + " AND t.ClientID=" + clientId + " AND (((t.[From]='Site' AND t.FromId=" + siteLocId + ") OR (t.[To]='Site' AND t.ToId=" + siteLocId + ")) or (t.[From] = 'Depot' and t.[To] = 'ThirdParty'))");
                                deleted++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    failed++;
                    errors.Add("DepotPic: " + dr["DepotPic"] + " => " + ex.Message);
                }

                try
                {
                    string fileName = dr["SitePic"].ToString().Trim();

                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        if (!databaseCon.BlobFileExists(fileName, "siteimage"))
                        {
                            errors.Add(fileName + " does not exist in Blob Storage.");
                            databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments "
            + "(TableName, ColumnName, FileName, ClientID, SiteLocID, RegisterID, BlobContainer, CheckedOn, QueryName, QueryText, ErrorMessage, Reason, ErrorOn) "
            + "VALUES ('tblDeliveryDocket', 'SitePic', '" + (((fileName) ?? "").Replace("'", "''")) + "', " + clientId + ", " + siteLocId + ", " + "NULL" + ", 'siteimage', GETDATE(), 'DeleteSiteImages-DeliveryDocket-SitePic', '" + deliveryDocketQuery.Replace("'", "''") + "', '" + (((null) ?? "").Replace("'", "''")) + "', 'File does not exist in Blob Storage.', GETDATE())");
                        }
                        else
                        {
                            databaseCon.deletefileonblob(fileName, "siteimage");

                            if (databaseCon.BlobFileExists(fileName, "siteimage"))
                            {
                                errors.Add(fileName + " could not be deleted from Blob Storage.");
                            }
                            else
                            {
                                databaseCon.RunCmd("UPDATE dd SET SitePic=NULL FROM tblDeliveryDocket dd INNER JOIN tblTransaction t ON dd.DeliveryID=t.DeliveryId and t.ClientId = dd.ClientId WHERE dd.DelDocId=" + delDocId + " AND t.ClientID=" + clientId + " AND (((t.[From]='Site' AND t.FromId=" + siteLocId + ") OR (t.[To]='Site' AND t.ToId=" + siteLocId + ")) or (t.[From] = 'Depot' and t.[To] = 'ThirdParty'))");
                                deleted++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    failed++;
                    errors.Add("SitePic: " + dr["SitePic"] + " => " + ex.Message);
                }
            }

            string deliveryDocumentQuery = "SELECT ddd.DDDocumentId, ddd.DocumentName FROM tblTransaction t INNER JOIN tblDeliveryDocketDocuments ddd ON t.TransId=ddd.TransID AND t.ClientId = ddd.ClientId WHERE t.ClientID=" + clientId + " AND ((t.[From]='Site' AND t.FromId=" + siteLocId + ") OR (t.[To]='Site' AND t.ToId=" + siteLocId + ")) AND ISNULL(ddd.DocumentName,'')<>'' UNION SELECT ddd.DDDocumentId, ddd.DocumentName FROM tblTransaction t INNER JOIN tblDeliveryDocketDocuments ddd ON t.TransId = ddd.TransID AND t.ClientId = ddd.ClientId WHERE t.ClientID = " + clientId + " AND(t.[From] = 'Depot' AND t.[To] = 'ThirdParty') AND ISNULL(ddd.DocumentName,'')<> ''";
            DataTable dtDeliveryDocument = databaseCon.GetData(deliveryDocumentQuery);

            foreach (DataRow dr in dtDeliveryDocument.Rows)
            {
                int ddDocumentId = Convert.ToInt32(dr["DDDocumentId"]);

                try
                {
                    string fileName = dr["DocumentName"].ToString().Trim();

                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        if (!databaseCon.BlobFileExists(fileName, "siteimage"))
                        {
                            errors.Add(fileName + " does not exist in Blob Storage.");
                            databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments "
            + "(TableName, ColumnName, FileName, ClientID, SiteLocID, RegisterID, BlobContainer, CheckedOn, QueryName, QueryText, ErrorMessage, Reason, ErrorOn) "
            + "VALUES ('tblDeliveryDocketDocuments', 'DocumentName', '" + (((fileName) ?? "").Replace("'", "''")) + "', " + clientId + ", " + siteLocId + ", " + "NULL" + ", 'siteimage', GETDATE(), 'DeleteSiteImages-DeliveryDocketDocuments', '" + deliveryDocumentQuery.Replace("'", "''") + "', '" + (((null) ?? "").Replace("'", "''")) + "', 'File does not exist in Blob Storage.', GETDATE())");
                        }
                        else
                        {
                            databaseCon.deletefileonblob(fileName, "siteimage");

                            if (databaseCon.BlobFileExists(fileName, "siteimage"))
                            {
                                errors.Add(fileName + " could not be deleted from Blob Storage.");
                            }
                            else
                            {
                                databaseCon.RunCmd("UPDATE ddd SET DocumentName=NULL FROM tblDeliveryDocketDocuments ddd INNER JOIN tblTransaction t ON ddd.TransID=t.TransId AND t.ClientId = ddd.ClientId WHERE ddd.DDDocumentId=" + ddDocumentId + " AND t.ClientID=" + clientId + " AND (((t.[From]='Site' AND t.FromId=" + siteLocId + ") OR (t.[To]='Site' AND t.ToId=" + siteLocId + ")) or (t.[From] = 'Depot' and t.[To] = 'ThirdParty'))");
                                deleted++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    failed++;
                    errors.Add("DocumentName: " + dr["DocumentName"] + " => " + ex.Message);
                }
            }

            //string damageQuery = "SELECT did.DIDetailID FROM tblDamagedInventory di INNER JOIN tblDamagedInventoryDetails did ON di.DamagedInvId = did.DamagedInvId WHERE ((di.[FromName]='Site' AND di.FromId=" + siteLocId + ") OR (di.[ToName]='Site' AND di.ToId=" + siteLocId + ")) AND di.ClientID=" + clientId + " union SELECT did.DIDetailID FROM tblDamagedInventory di INNER JOIN tblDamagedInventoryDetails did ON di.DamagedInvId = did.DamagedInvId WHERE (di.[FromName]='Depot' and di.[ToName]='ThirdParty') AND di.ClientID=" + clientId;
            //DataTable dtDamage = databaseCon.GetData(damageQuery);

            //foreach (DataRow drs in dtDamage.Rows)
            //{
            //    int diDetailId = Convert.ToInt32(drs["DIDetailID"]);
            //    string finalDamageReport = "FinalDamageReport_" + diDetailId + ".pdf";

            //    try
            //    {
            //        if (!databaseCon.BlobFileExists(finalDamageReport, "siteimage"))
            //        {
            //            errors.Add(finalDamageReport + " does not exist in Blob Storage.");
            //            databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments "
            //+ "(TableName, ColumnName, FileName, ClientID, SiteLocID, RegisterID, BlobContainer, CheckedOn, QueryName, QueryText, ErrorMessage, Reason, ErrorOn) "
            //+ "VALUES ('tblDamagedInventoryDetails', 'FinalDamageReport', '" + (((finalDamageReport) ?? "").Replace("'", "''")) + "', " + clientId + ", " + siteLocId + ", " + "NULL" + ", 'siteimage', GETDATE(), 'DeleteSiteImages-DamagedInventory', '" + damageQuery.Replace("'", "''") + "', '" + (((null) ?? "").Replace("'", "''")) + "', 'File does not exist in Blob Storage.', GETDATE())");
            //        }
            //        else
            //        {
            //            databaseCon.deletefileonblob(finalDamageReport, "siteimage");

            //            if (databaseCon.BlobFileExists(finalDamageReport, "siteimage"))
            //            {
            //                errors.Add(finalDamageReport + " could not be deleted from Blob Storage.");
            //            }
            //            else
            //            {
            //                deleted++;
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        failed++;
            //        errors.Add(finalDamageReport + " => " + ex.Message);
            //    }
            //}

            string damageQuery = "SELECT did.DIDetailID FROM tblDamagedInventory di INNER JOIN tblDamagedInventoryDetails did ON di.DamagedInvId = did.DamagedInvId WHERE ((di.[FromName]='Site' AND di.FromId=" + siteLocId + ") OR (di.[ToName]='Site' AND di.ToId=" + siteLocId + ")) AND di.ClientID=" + clientId + " UNION SELECT did.DIDetailID FROM tblDamagedInventory di INNER JOIN tblDamagedInventoryDetails did ON di.DamagedInvId = did.DamagedInvId WHERE (di.[FromName]='Depot' AND di.[ToName]='ThirdParty') AND di.ClientID=" + clientId;

            DataTable dtDamage = databaseCon.GetData(damageQuery);

            foreach (DataRow drs in dtDamage.Rows)
            {
                int diDetailId = Convert.ToInt32(drs["DIDetailID"]);
                string finalDamageReport = "FinalDamageReport_" + diDetailId + ".pdf";

                try
                {
                    // Delete only if the file exists in Blob
                    if (databaseCon.BlobFileExists(finalDamageReport, "siteimage"))
                    {
                        databaseCon.deletefileonblob(finalDamageReport, "siteimage");

                        if (databaseCon.BlobFileExists(finalDamageReport, "siteimage"))
                        {
                            errors.Add(finalDamageReport + " could not be deleted from Blob Storage.");
                        }
                        else
                        {
                            deleted++;
                        }
                    }

                    // If the generated file does not exist in Blob,
                    // do nothing (no error, no tblMissingBlobDocuments entry).
                }
                catch (Exception ex)
                {
                    failed++;
                    errors.Add(finalDamageReport + " => " + ex.Message);
                }
            }

            if (errors.Count > 0)
            {
                return Json(new
                {
                    Result = "Warning",
                    Deleted = deleted,
                    Message = string.Join("\n", errors)
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                Result = "Success",
                Deleted = deleted,
                Message = deleted + " Images deleted successfully from Blob and Database."
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetDeleteJobStatus(int jobId)
        {
            DataTable dt = databaseCon.GetData("SELECT Status, Message FROM tblDeleteJobStatus WHERE JobId=" + jobId);
            if (dt.Rows.Count == 0)
                return Json(new { Result = "NotFound" }, JsonRequestBehavior.AllowGet);

            return Json(new
            {
                Result = dt.Rows[0]["Status"].ToString(),
                Message = dt.Rows[0]["Message"].ToString()
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteClientData(string clientId, string userId)
        {
            try
            {
                clientId = databaseCon.Decrypt(clientId);
                userId = databaseCon.Decrypt(userId);

                // ---------------------------------------------------------------
                // Validate decrypted values BEFORE using them anywhere.
                // ---------------------------------------------------------------
                int clientIdInt, userIdInt;
                if (!int.TryParse(clientId, out clientIdInt) || clientIdInt <= 0)
                {
                    return Json(new
                    {
                        Result = "Failed",
                        Message = "Invalid or corrupted clientId after decryption. Please reload the page and try again."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (!int.TryParse(userId, out userIdInt) || userIdInt <= 0)
                {
                    return Json(new
                    {
                        Result = "Failed",
                        Message = "Invalid or corrupted userId after decryption. Please reload the page and try again."
                    }, JsonRequestBehavior.AllowGet);
                }

                clientId = clientIdInt.ToString();
                userId = userIdInt.ToString();

                List<object> pending = new List<object>();

                // ---------------------------------------------------------------
                // tblDocuments -> container "registerdocument"
                // ---------------------------------------------------------------
                string docQuery = "SELECT DocumentId,DocumentName FROM tblDocuments  WHERE ClientID = " + clientId + "  AND ISNULL(DocumentName,'')<> ''";
                try
                {
                    DataTable dtDocuments = databaseCon.GetData(docQuery);
                    foreach (DataRow dr in dtDocuments.Rows)
                    {
                        string fileName = dr["DocumentName"] == DBNull.Value ? "" : dr["DocumentName"].ToString().Trim();
                        if (!string.IsNullOrWhiteSpace(fileName))
                        {
                            pending.Add(new { Table = "tblDocuments", Id = dr["DocumentId"], Column = "DocumentName", FileName = fileName, Container = "registerdocument" });
                        }
                    }
                }
                catch (Exception ex)
                {
                    databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments (ClientID,TableName,QueryName,QueryText,ErrorMessage,ErrorOn) VALUES (" + clientId + ",'tblDocuments','tblDocuments Query','" + docQuery.Replace("'", "''") + "','" + ex.ToString().Replace("'", "''") + "',GETDATE())");
                }

                // ---------------------------------------------------------------
                // tblInspection.InspFileName -> container "registerdocument"
                // ---------------------------------------------------------------
                string inspDocQuery = "SELECT i.InspectionID, i.InspFileName, i.InspectionDate FROM tblInspection i INNER JOIN tblRegister r ON i.RegisterID = r.RegisterId WHERE r.ClientID = " + clientId + "";
                try
                {
                    DataTable dtInspDoc = databaseCon.GetData(inspDocQuery);
                    foreach (DataRow dr in dtInspDoc.Rows)
                    {
                        string fileName = dr["InspFileName"] == DBNull.Value ? "" : dr["InspFileName"].ToString().Trim();
                        if (!string.IsNullOrWhiteSpace(fileName))
                        {
                            pending.Add(new { Table = "tblInspection", Id = dr["InspectionID"], Column = "InspFileName", FileName = fileName, Container = "registerdocument" });
                        }
                    }
                }
                catch (Exception ex)
                {
                    databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments (ClientID,TableName,QueryName,QueryText,ErrorMessage,ErrorOn) VALUES (" + clientId + ",'tblInspection','Inspection Document Query','" + inspDocQuery.Replace("'", "''") + "','" + ex.ToString().Replace("'", "''") + "',GETDATE())");
                }

                // ---------------------------------------------------------------
                // tblHandOver -> container "siteimage"
                // ---------------------------------------------------------------
                string handOverQuerySite = "SELECT h.HandOverId, h.HandOverImage, h.ContractorSign, h.RepresentativeSign, h.HandoverMapImage FROM tblHandOver h INNER JOIN tblRegister r ON h.RegisterId=r.RegisterId WHERE r.ClientID=" + clientId + " AND (ISNULL(h.HandOverImage,'')<>'' OR ISNULL(h.ContractorSign,'')<>'' OR ISNULL(h.RepresentativeSign,'')<>'' OR ISNULL(h.HandoverMapImage,'')<>'')";
                try
                {
                    DataTable dtHandOversite = databaseCon.GetData(handOverQuerySite);
                    foreach (DataRow dr in dtHandOversite.Rows)
                    {
                        string[] cols = { "HandOverImage", "ContractorSign", "RepresentativeSign", "HandoverMapImage" };
                        foreach (string col in cols)
                        {
                            string fileName = dr[col] == DBNull.Value ? "" : dr[col].ToString().Trim();
                            if (!string.IsNullOrWhiteSpace(fileName))
                            {
                                pending.Add(new { Table = "tblHandOver", Id = dr["HandOverId"], Column = col, FileName = fileName, Container = "siteimage" });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments (ClientID,TableName,QueryName,QueryText,ErrorMessage,ErrorOn) VALUES (" + clientId + ",'tblHandOver','HandOver SiteImage Query','" + handOverQuerySite.Replace("'", "''") + "','" + ex.ToString().Replace("'", "''") + "',GETDATE())");
                }

                // ---------------------------------------------------------------
                // tblRegisterGallery -> container "siteimage"
                // ---------------------------------------------------------------
                string galleryQuery = "SELECT g.RegisterId, g.FileName FROM tblRegisterGallery g INNER JOIN tblRegister r ON g.RegisterId = r.RegisterId WHERE r.ClientID = " + clientId + " AND ISNULL(g.FileName,'') <> ''";
                try
                {
                    DataTable dtGallery = databaseCon.GetData(galleryQuery);
                    foreach (DataRow dr in dtGallery.Rows)
                    {
                        string fileName = dr["FileName"] == DBNull.Value ? "" : dr["FileName"].ToString().Trim();
                        if (fileName.Contains("/"))
                        {
                            fileName = fileName.Substring(fileName.LastIndexOf('/') + 1);
                        }
                        if (!string.IsNullOrWhiteSpace(fileName))
                        {
                            pending.Add(new { Table = "tblRegisterGallery", Id = dr["RegisterId"], Column = "FileName", FileName = fileName, Container = "siteimage" });
                        }
                    }
                }
                catch (Exception ex)
                {
                    databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments (ClientID,TableName,QueryName,QueryText,ErrorMessage,ErrorOn) VALUES (" + clientId + ",'tblRegisterGallery','Register Gallery Query','" + galleryQuery.Replace("'", "''") + "','" + ex.ToString().Replace("'", "''") + "',GETDATE())");
                }

                // ---------------------------------------------------------------
                // tblInspection - Inspimage / Inspimage2 / InspSign -> container "siteimage"
                // ---------------------------------------------------------------
                string inspImgQuery = "SELECT i.InspectionID, i.Inspimage, i.Inspimage2, i.InspSign FROM tblInspection i INNER JOIN tblRegister r ON i.RegisterID = r.RegisterId WHERE r.ClientID = " + clientId + " AND (ISNULL(i.Inspimage,'') <> '' OR ISNULL(i.Inspimage2,'') <> '' OR ISNULL(i.InspSign,'') <> '')";
                try
                {
                    DataTable dtInspImg = databaseCon.GetData(inspImgQuery);
                    foreach (DataRow dr in dtInspImg.Rows)
                    {
                        string[] cols = { "Inspimage", "Inspimage2", "InspSign" };
                        foreach (string col in cols)
                        {
                            string fileName = dr[col] == DBNull.Value ? "" : dr[col].ToString().Trim();
                            if (!string.IsNullOrWhiteSpace(fileName))
                            {
                                pending.Add(new { Table = "tblInspection", Id = dr["InspectionID"], Column = col, FileName = fileName, Container = "siteimage" });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments (ClientID,TableName,QueryName,QueryText,ErrorMessage,ErrorOn) VALUES (" + clientId + ",'tblInspection','Inspection Image Query','" + inspImgQuery.Replace("'", "''") + "','" + ex.ToString().Replace("'", "''") + "',GETDATE())");
                }

                // ---------------------------------------------------------------
                // tblDayDocket -> container "siteimage" (scoped via tblSiteLocation)
                // ---------------------------------------------------------------
                string dayDocketQuerysite = "SELECT d.DayDocketId, d.SupervisorSign, d.ClientSign FROM tblDayDocket d WHERE d.Sitelocid IN (SELECT SiteLocId FROM tblSiteLocation WHERE ClientId = " + clientId + ") AND (ISNULL(d.SupervisorSign,'') <> '' OR ISNULL(d.ClientSign,'') <> '')";
                try
                {
                    DataTable dtDayDocketsite = databaseCon.GetData(dayDocketQuerysite);
                    foreach (DataRow dr in dtDayDocketsite.Rows)
                    {
                        string[] cols = { "SupervisorSign", "ClientSign" };
                        foreach (string col in cols)
                        {
                            string fileName = dr[col] == DBNull.Value ? "" : dr[col].ToString().Trim();
                            if (!string.IsNullOrWhiteSpace(fileName))
                            {
                                pending.Add(new { Table = "tblDayDocket", Id = dr["DayDocketId"], Column = col, FileName = fileName, Container = "siteimage" });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments (ClientID,TableName,QueryName,QueryText,ErrorMessage,ErrorOn) VALUES (" + clientId + ",'tblDayDocket','DayDocket SiteImage Query','" + dayDocketQuerysite.Replace("'", "''") + "','" + ex.ToString().Replace("'", "''") + "',GETDATE())");
                }

                // ---------------------------------------------------------------
                // tblDeliveryDocket -> container "siteimage" (via tblTransaction.ClientId)
                // ---------------------------------------------------------------
                string deliveryDocketQuery = "SELECT dd.DelDocId, dd.DepotPersonSign, dd.TransportDriverSign, dd.SitePersonSign, dd.AcceptorSign, dd.DepotPic, dd.SitePic FROM tblTransaction t INNER JOIN tblDeliveryDocket dd ON t.DeliveryId=dd.DeliveryID AND t.ClientId=dd.ClientId WHERE t.ClientID=" + clientId + " AND (ISNULL(dd.DepotPersonSign,'')<>'' OR ISNULL(dd.TransportDriverSign,'')<>'' OR ISNULL(dd.SitePersonSign,'')<>'' OR ISNULL(dd.AcceptorSign,'')<>'' OR ISNULL(dd.DepotPic,'')<>'' OR ISNULL(dd.SitePic,'')<>'') UNION SELECT dd.DelDocId, dd.DepotPersonSign, dd.TransportDriverSign, dd.SitePersonSign, dd.AcceptorSign, dd.DepotPic, dd.SitePic FROM tblTransaction t INNER JOIN tblDeliveryDocket dd ON t.DeliveryId=dd.DeliveryID AND t.ClientId=dd.ClientId WHERE t.ClientID=" + clientId + " AND (t.[From]='Depot' AND t.[To]='ThirdParty') AND (ISNULL(dd.DepotPersonSign,'')<>'' OR ISNULL(dd.TransportDriverSign,'')<>'' OR ISNULL(dd.SitePersonSign,'')<>'' OR ISNULL(dd.AcceptorSign,'')<>'' OR ISNULL(dd.DepotPic,'')<>'' OR ISNULL(dd.SitePic,'')<>'')";
                try
                {
                    DataTable dtDeliveryDocket = databaseCon.GetData(deliveryDocketQuery);
                    foreach (DataRow dr in dtDeliveryDocket.Rows)
                    {
                        string[] cols = { "DepotPersonSign", "TransportDriverSign", "SitePersonSign", "AcceptorSign", "DepotPic", "SitePic" };
                        foreach (string col in cols)
                        {
                            string fileName = dr[col] == DBNull.Value ? "" : dr[col].ToString().Trim();
                            if (!string.IsNullOrWhiteSpace(fileName))
                            {
                                pending.Add(new { Table = "tblDeliveryDocket", Id = dr["DelDocId"], Column = col, FileName = fileName, Container = "siteimage" });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments (ClientID,TableName,QueryName,QueryText,ErrorMessage,ErrorOn) VALUES (" + clientId + ",'tblDeliveryDocket','Delivery Docket Query','" + deliveryDocketQuery.Replace("'", "''") + "','" + ex.ToString().Replace("'", "''") + "',GETDATE())");
                }

                // ---------------------------------------------------------------
                // tblDocuments -> container "siteimage" (via tblDocuments.DocumentId)
                // ---------------------------------------------------------------
                string docQuerysite = "SELECT DocumentId,DocumentName FROM tblDocuments  WHERE ClientID = " + clientId + "  AND ISNULL(DocumentName,'')<> ''";
                try
                {
                    DataTable dtDocumentssite = databaseCon.GetData(docQuerysite);
                    foreach (DataRow dr in dtDocumentssite.Rows)
                    {
                        string fileName = dr["DocumentName"] == DBNull.Value ? "" : dr["DocumentName"].ToString().Trim();
                        if (!string.IsNullOrWhiteSpace(fileName))
                        {
                            pending.Add(new { Table = "tblDocuments", Id = dr["DocumentId"], Column = "DocumentName", FileName = fileName, Container = "siteimage" });
                        }
                    }
                }
                catch (Exception ex)
                {
                    databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments (ClientID,TableName,QueryName,QueryText,ErrorMessage,ErrorOn) VALUES (" + clientId + ",'tblDocuments','Document SiteImage Query','" + docQuerysite.Replace("'", "''") + "','" + ex.ToString().Replace("'", "''") + "',GETDATE())");
                }

                // ---------------------------------------------------------------
                // tblDeliveryDocketDocuments -> container "siteimage" (via tblTransaction.ClientId)
                // ---------------------------------------------------------------
                string deliveryDocumentQuerysite = "SELECT ddd.DDDocumentId, ddd.DocumentName FROM tblTransaction t INNER JOIN tblDeliveryDocketDocuments ddd ON t.TransId=ddd.TransID AND t.ClientId=ddd.ClientId WHERE t.ClientID=" + clientId + " AND ISNULL(ddd.DocumentName,'')<>'' UNION SELECT ddd.DDDocumentId, ddd.DocumentName FROM tblTransaction t INNER JOIN tblDeliveryDocketDocuments ddd ON t.TransId=ddd.TransID AND t.ClientId=ddd.ClientId WHERE t.ClientID=" + clientId + " AND (t.[From]='Depot' AND t.[To]='ThirdParty') AND ISNULL(ddd.DocumentName,'')<>''";
                try
                {
                    DataTable dtDeliveryDocumentsite = databaseCon.GetData(deliveryDocumentQuerysite);
                    foreach (DataRow dr in dtDeliveryDocumentsite.Rows)
                    {
                        string fileName = dr["DocumentName"] == DBNull.Value ? "" : dr["DocumentName"].ToString().Trim();
                        if (!string.IsNullOrWhiteSpace(fileName))
                        {
                            pending.Add(new { Table = "tblDeliveryDocketDocuments", Id = dr["DDDocumentId"], Column = "DocumentName", FileName = fileName, Container = "siteimage" });
                        }
                    }
                }
                catch (Exception ex)
                {
                    databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments (ClientID,TableName,QueryName,QueryText,ErrorMessage,ErrorOn) VALUES (" + clientId + ",'tblDeliveryDocketDocuments','Delivery Docket Documents Query','" + deliveryDocumentQuerysite.Replace("'", "''") + "','" + ex.ToString().Replace("'", "''") + "',GETDATE())");
                }

                // ---------------------------------------------------------------
                // tblDamagedInventory -> FinalDamageReport_{DIDetailID}.pdf -> container "siteimage"
                // ---------------------------------------------------------------
                //string damageQuery = "SELECT did.DIDetailID FROM tblDamagedInventory di INNER JOIN tblDamagedInventoryDetails did ON di.DamagedInvId = did.DamagedInvId WHERE di.ClientID = " + clientId + " AND ((di.FromName = 'Site' AND di.FromId IN (SELECT SiteLocId FROM tblSiteLocation WHERE ClientID = " + clientId + ")) OR (di.ToName = 'Site' AND di.ToId IN (SELECT SiteLocId FROM tblSiteLocation WHERE ClientID = " + clientId + ")) OR (di.FromName = 'Depot' AND di.ToName = 'ThirdParty'))";
                //try
                //{
                //    DataTable dtDamage = databaseCon.GetData(damageQuery);
                //    foreach (DataRow dr in dtDamage.Rows)
                //    {
                //        int diDetailId = Convert.ToInt32(dr["DIDetailID"]);
                //        string pdfFileName = "FinalDamageReport_" + diDetailId + ".pdf";
                //        pending.Add(new { Table = "tblDamagedInventoryDetails", Id = diDetailId, Column = "FinalDamageReport", FileName = pdfFileName, Container = "siteimage" });
                //    }
                //}
                //catch (Exception ex)
                //{
                //    databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments (ClientID,TableName,QueryName,QueryText,ErrorMessage,ErrorOn) VALUES (" + clientId + ",'tblDamagedInventoryDetails','Damaged Inventory Query','" + damageQuery.Replace("'", "''") + "','" + ex.ToString().Replace("'", "''") + "',GETDATE())");
                //}

                if (pending.Count > 0)
                {
                    return Json(new
                    {
                        Result = "BlobPending",
                        Count = pending.Count,
                        Files = pending.Select(x => ((dynamic)x).FileName.ToString()).Distinct().ToList()
                    }, JsonRequestBehavior.AllowGet);
                }

                int cId = clientIdInt;
                int uId = userIdInt;
                string connString = dbcontext.Database.Connection.ConnectionString;

                string insertJobQuery = "INSERT INTO tblDeleteJobStatus (ClientId, Status, StartedOn) VALUES (" + cId + ", 'Running', GETDATE())";
                int jobId = databaseCon.RunCmdID(insertJobQuery);

                Task.Run(() =>
                {
                    try
                    {
                        DataSet ds = new DataSet();
                        using (SqlConnection conn = new SqlConnection(connString))
                        using (SqlCommand cmd = new SqlCommand("EXEC dbo.Pro_DeleteClientData @ClientId=" + cId + ", @userId=" + uId + ", @RunDelete=1", conn))
                        {
                            cmd.CommandTimeout = 900; conn.Open(); new SqlDataAdapter(cmd).Fill(ds);
                        }
                        if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                        {
                            using (SqlConnection statusConn = new SqlConnection(connString))
                            using (SqlCommand statusCmd = new SqlCommand("UPDATE tblDeleteJobStatus SET Status='Failed', Message=@msg, CompletedOn=GETDATE() WHERE JobId=@jobId", statusConn))
                            {
                                statusCmd.Parameters.AddWithValue("@msg", "Delete procedure did not return a result set for ClientId " + cId);
                                statusCmd.Parameters.AddWithValue("@jobId", jobId);
                                statusConn.Open();
                                statusCmd.ExecuteNonQuery();
                            }
                            return;
                        }

                        string status = ds.Tables[0].Rows[0]["Status"].ToString();
                        string errorMsg = ds.Tables[0].Rows[0]["ErrorMessage"] == DBNull.Value
                            ? null
                            : ds.Tables[0].Rows[0]["ErrorMessage"].ToString();

                        if (status == "ERROR")
                        {
                            using (SqlConnection statusConn = new SqlConnection(connString))
                            using (SqlCommand statusCmd = new SqlCommand("UPDATE tblDeleteJobStatus SET Status='Failed', Message=@msg, CompletedOn=GETDATE() WHERE JobId=@jobId", statusConn))
                            {
                                statusCmd.Parameters.AddWithValue("@msg", "Client deletion failed: " + errorMsg);
                                statusCmd.Parameters.AddWithValue("@jobId", jobId);
                                statusConn.Open();
                                statusCmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            using (SqlConnection statusConn = new SqlConnection(connString))
                            using (SqlCommand statusCmd = new SqlCommand("UPDATE tblDeleteJobStatus SET Status='Success', Message=@msg, CompletedOn=GETDATE() WHERE JobId=@jobId", statusConn))
                            {
                                statusCmd.Parameters.AddWithValue("@msg", "ClientId " + cId + " and all related data deleted successfully.");
                                statusCmd.Parameters.AddWithValue("@jobId", jobId);
                                statusConn.Open();
                                statusCmd.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            using (SqlConnection statusConn = new SqlConnection(connString))
                            using (SqlCommand statusCmd = new SqlCommand("UPDATE tblDeleteJobStatus SET Status='Failed', Message=@msg, CompletedOn=GETDATE() WHERE JobId=@jobId", statusConn))
                            {
                                statusCmd.Parameters.AddWithValue("@msg", ex.ToString());
                                statusCmd.Parameters.AddWithValue("@jobId", jobId);
                                statusConn.Open();
                                statusCmd.ExecuteNonQuery();
                            }
                        }
                        catch { }
                    }
                });

                return Json(new
                {
                    Result = "Started",
                    JobId = jobId,
                    Message = "Client deletion started in background."
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = "Failed",
                    Message = ex.ToString()
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult CheckMissingSiteDocumentsAndImages(string clientId, int siteLocId)
        {
            clientId = databaseCon.Decrypt(clientId);
            int inserted = 0;
            int failed = 0;
            List<string> errors = new List<string>();

            string docQuery = "SELECT d.DocumentId,d.DocumentName,d.RegisterID,d.SiteLocID FROM tblDocuments d LEFT JOIN tblRegister r ON d.RegisterID = r.RegisterId WHERE d.ClientID = " + clientId + " AND((d.RegisterID IS NOT NULL AND r.SiteLocId = " + siteLocId + ") OR(d.RegisterID IS NULL AND d.SiteLocID = " + siteLocId + ") OR R.SiteLocId = " + siteLocId + ") AND ISNULL(d.DocumentName,'')<> '' AND ISNULL(d.RegisterID,'')<> ''";

            DataTable dtDocuments = databaseCon.GetData(docQuery);

            foreach (DataRow dr in dtDocuments.Rows)
            {
                try
                {
                    int documentId = Convert.ToInt32(dr["DocumentId"]);
                    string fileName = Convert.ToString(dr["DocumentName"]);
                    string registerId = dr["RegisterID"] == DBNull.Value ? "NULL" : dr["RegisterID"].ToString();
                    string docSiteLocId = dr["SiteLocID"] == DBNull.Value ? siteLocId.ToString() : dr["SiteLocID"].ToString();

                    if (string.IsNullOrWhiteSpace(fileName))
                    {
                        errors.Add("Document Id " + documentId + " : File Name is empty.");
                    }

                    if (!databaseCon.BlobFileExists(fileName, "registerdocument"))
                    {
                        databaseCon.RunCmd(
                            "INSERT INTO tblMissingBlobDocuments(TableId,TableName,ColumnName,FileName,ClientID,SiteLocID,RegisterID,BlobContainer,CheckedOn) VALUES(" +
                            documentId + ",'tblDocuments','DocumentName','" +
                            fileName.Replace("'", "''") + "'," +
                            clientId + "," + docSiteLocId + "," + registerId +
                            ",'registerdocument',GETDATE())");

                        inserted++;
                    }
                }

                catch (Exception ex)
                {
                    failed++;
                    errors.Add("DocumentName: " + dr["DocumentName"] + " => " + ex.Message);
                }
            }


            string inspQuery = "SELECT i.InspectionID, i.InspFileName, i.InspectionDate, i.RegisterID FROM tblInspection i INNER JOIN tblRegister r ON i.RegisterID = r.RegisterId WHERE r.ClientID = " + clientId + " AND r.SiteLocId = " + siteLocId;

            DataTable dtInspection = databaseCon.GetData(inspQuery);
            foreach (DataRow dr in dtInspection.Rows)
            {
                try
                {
                    int inspectionId = Convert.ToInt32(dr["InspectionID"]);
                    string fileName = Convert.ToString(dr["InspFileName"]);
                    string registerId = dr["RegisterID"] == DBNull.Value ? "NULL" : dr["RegisterID"].ToString();

                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        if (!databaseCon.BlobFileExists(fileName, "registerdocument"))
                        {
                            databaseCon.RunCmd(
                                "INSERT INTO tblMissingBlobDocuments(TableId,TableName,ColumnName,FileName,ClientID,SiteLocID,RegisterID,BlobContainer,CheckedOn) " +
                                "VALUES(" + inspectionId + ",'tblInspection','InspFileName','" + fileName.Replace("'", "''") + "'," +
                                clientId + "," + siteLocId + "," + registerId + ",'registerdocument',GETDATE())");

                            inserted++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    failed++;
                    errors.Add("InspFileName: " + Convert.ToString(dr["InspFileName"]) + " => " + ex.Message);
                }
            }

            string DayDocket = "SELECT h.DayDocketID FROM tblDayDocket h INNER JOIN tblRegister r ON h.SiteLocId=r.SiteLocId WHERE r.ClientID=" + clientId + " AND h.SiteLocId=" + siteLocId;

            DataTable dtDayDocket = databaseCon.GetData(DayDocket);

            foreach (DataRow dr in dtDayDocket.Rows)
            {
                try
                {
                    int DayDocketId = Convert.ToInt32(dr["DayDocketID"]);
                    string pdfFileName = "DayDocket_" + DayDocketId + ".pdf";

                    if (!databaseCon.BlobFileExists(pdfFileName, "registerdocument"))
                    {
                        databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments(TableId,TableName,ColumnName,FileName,ClientID,SiteLocID,RegisterID,BlobContainer,CheckedOn) VALUES(" + DayDocketId + ",'tblDayDocket','DayDocketPdf','" + pdfFileName.Replace("'", "''") + "'," + clientId + "," + siteLocId + ",NULL,'registerdocument',GETDATE())");
                        inserted++;
                    }
                }
                catch (Exception ex)
                {
                    failed++;
                    errors.Add(ex.Message);
                }
            }



            string handOverImageQuery = "SELECT h.HandOverId, h.HandOverImage, h.ContractorSign, h.RepresentativeSign, h.HandoverMapImage, h.RegisterId FROM tblHandOver h INNER JOIN tblRegister r ON h.RegisterId=r.RegisterId WHERE r.ClientID=" + clientId + " AND r.SiteLocId=" + siteLocId + " AND (ISNULL(h.HandOverImage,'')<>'' OR ISNULL(h.ContractorSign,'')<>'' OR ISNULL(h.RepresentativeSign,'')<>'' OR ISNULL(h.HandoverMapImage,'')<>'')";

            DataTable dtHandOverImage = databaseCon.GetData(handOverImageQuery);

            foreach (DataRow dr in dtHandOverImage.Rows)
            {
                int handOverId = Convert.ToInt32(dr["HandOverId"]);
                string registerId = dr["RegisterId"] == DBNull.Value ? "NULL" : dr["RegisterId"].ToString();

                try
                {
                    string fileName = dr["HandOverImage"].ToString().Trim();

                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        if (!databaseCon.BlobFileExists(fileName, "siteimage"))
                        {
                            databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments(TableId,TableName,ColumnName,FileName,ClientID,SiteLocID,RegisterID,BlobContainer,CheckedOn) VALUES(" + handOverId + ",'tblHandOver','HandOverImage','" + fileName.Replace("'", "''") + "'," + clientId + "," + siteLocId + "," + registerId + ",'siteimage',GETDATE())");
                            inserted++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    failed++;
                    errors.Add("HandOverImage: " + dr["HandOverImage"] + " => " + ex.Message);
                }

                try
                {
                    string fileName = dr["ContractorSign"].ToString().Trim();

                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        if (!databaseCon.BlobFileExists(fileName, "siteimage"))
                        {
                            databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments(TableId,TableName,ColumnName,FileName,ClientID,SiteLocID,RegisterID,BlobContainer,CheckedOn) VALUES(" + handOverId + ",'tblHandOver','ContractorSign','" + fileName.Replace("'", "''") + "'," + clientId + "," + siteLocId + "," + registerId + ",'siteimage',GETDATE())");
                            inserted++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    failed++;
                    errors.Add("ContractorSign: " + dr["ContractorSign"] + " => " + ex.Message);
                }

                try
                {
                    string fileName = dr["RepresentativeSign"].ToString().Trim();

                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        if (!databaseCon.BlobFileExists(fileName, "siteimage"))
                        {
                            databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments(TableId,TableName,ColumnName,FileName,ClientID,SiteLocID,RegisterID,BlobContainer,CheckedOn) VALUES(" + handOverId + ",'tblHandOver','RepresentativeSign','" + fileName.Replace("'", "''") + "'," + clientId + "," + siteLocId + "," + registerId + ",'siteimage',GETDATE())");
                            inserted++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    failed++;
                    errors.Add("RepresentativeSign: " + dr["RepresentativeSign"] + " => " + ex.Message);
                }

                try
                {
                    string fileName = dr["HandoverMapImage"].ToString().Trim();

                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        if (!databaseCon.BlobFileExists(fileName, "siteimage"))
                        {
                            databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments(TableId,TableName,ColumnName,FileName,ClientID,SiteLocID,RegisterID,BlobContainer,CheckedOn) VALUES(" + handOverId + ",'tblHandOver','HandoverMapImage','" + fileName.Replace("'", "''") + "'," + clientId + "," + siteLocId + "," + registerId + ",'siteimage',GETDATE())");
                            inserted++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    failed++;
                    errors.Add("HandoverMapImage: " + dr["HandoverMapImage"] + " => " + ex.Message);
                }
            }

            string galleryQuery = "SELECT g.RegisterId, g.FileName FROM tblRegisterGallery g INNER JOIN tblRegister r ON g.RegisterId = r.RegisterId WHERE r.ClientID=" + clientId + " AND r.SiteLocId=" + siteLocId + " AND ISNULL(g.FileName,'')<>''";

            DataTable dtGallery = databaseCon.GetData(galleryQuery);

            foreach (DataRow dr in dtGallery.Rows)
            {
                try
                {
                    int registerId = Convert.ToInt32(dr["RegisterId"]);
                    string fileName = dr["FileName"] == DBNull.Value ? "" : dr["FileName"].ToString().Trim();

                    if (fileName.Contains("/"))
                    {
                        fileName = fileName.Substring(fileName.LastIndexOf('/') + 1);
                    }

                    if (string.IsNullOrWhiteSpace(fileName))
                    {
                        errors.Add("Register Id " + registerId + " : File Name is empty.");
                    }
                    else
                    {
                        if (!databaseCon.BlobFileExists(fileName, "siteimage"))
                        {
                            databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments(TableId,TableName,ColumnName,FileName,ClientID,SiteLocID,RegisterID,BlobContainer,CheckedOn) VALUES(" + registerId + ",'tblRegisterGallery','FileName','" + fileName.Replace("'", "''") + "'," + clientId + "," + siteLocId + "," + registerId + ",'siteimage',GETDATE())");
                            inserted++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    failed++;
                    errors.Add("Gallery: " + dr["FileName"] + " => " + ex.Message);
                }
            }

            string inspImageQuery = "SELECT i.InspectionID, i.Inspimage, i.Inspimage2, i.InspSign, i.InspFileName, i.RegisterID FROM tblInspection i INNER JOIN tblRegister r ON i.RegisterID=r.RegisterId WHERE r.ClientID=" + clientId + " AND r.SiteLocId=" + siteLocId + " AND (ISNULL(i.Inspimage,'')<>'' OR ISNULL(i.Inspimage2,'')<>'' OR ISNULL(i.InspSign,'')<>'' OR ISNULL(i.InspFileName,'')<>'')";

            DataTable dtInspectionImage = databaseCon.GetData(inspImageQuery);

            foreach (DataRow dr in dtInspectionImage.Rows)
            {
                int inspectionId = Convert.ToInt32(dr["InspectionID"]);
                string registerId = dr["RegisterID"] == DBNull.Value ? "NULL" : dr["RegisterID"].ToString();

                try
                {
                    string fileName = dr["Inspimage"].ToString().Trim();

                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        if (!databaseCon.BlobFileExists(fileName, "siteimage"))
                        {
                            databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments(TableId,TableName,ColumnName,FileName,ClientID,SiteLocID,RegisterID,BlobContainer,CheckedOn) VALUES(" + inspectionId + ",'tblInspection','Inspimage','" + fileName.Replace("'", "''") + "'," + clientId + "," + siteLocId + "," + registerId + ",'siteimage',GETDATE())");
                            inserted++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    failed++;
                    errors.Add("Inspimage: " + dr["Inspimage"] + " => " + ex.Message);
                }

                try
                {
                    string fileName = dr["Inspimage2"].ToString().Trim();

                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        if (!databaseCon.BlobFileExists(fileName, "siteimage"))
                        {
                            databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments(TableId,TableName,ColumnName,FileName,ClientID,SiteLocID,RegisterID,BlobContainer,CheckedOn) VALUES(" + inspectionId + ",'tblInspection','Inspimage2','" + fileName.Replace("'", "''") + "'," + clientId + "," + siteLocId + "," + registerId + ",'siteimage',GETDATE())");
                            inserted++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    failed++;
                    errors.Add("Inspimage2: " + dr["Inspimage2"] + " => " + ex.Message);
                }

                try
                {
                    string fileName = dr["InspSign"].ToString().Trim();

                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        if (!databaseCon.BlobFileExists(fileName, "siteimage"))
                        {
                            databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments(TableId,TableName,ColumnName,FileName,ClientID,SiteLocID,RegisterID,BlobContainer,CheckedOn) VALUES(" + inspectionId + ",'tblInspection','InspSign','" + fileName.Replace("'", "''") + "'," + clientId + "," + siteLocId + "," + registerId + ",'siteimage',GETDATE())");
                            inserted++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    failed++;
                    errors.Add("InspSign: " + dr["InspSign"] + " => " + ex.Message);
                }


            }

            string dayDocketQuery = "SELECT d.DayDocketId, d.SupervisorSign, d.ClientSign FROM tblDayDocket d WHERE d.SiteLocId=" + siteLocId + " AND (ISNULL(d.SupervisorSign,'')<>'' OR ISNULL(d.ClientSign,'')<>'')";

            DataTable dtDayDocketImage = databaseCon.GetData(dayDocketQuery);

            foreach (DataRow dr in dtDayDocketImage.Rows)
            {
                int dayDocketId = Convert.ToInt32(dr["DayDocketId"]);

                try
                {
                    string fileName = dr["SupervisorSign"].ToString().Trim();

                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        if (!databaseCon.BlobFileExists(fileName, "siteimage"))
                        {
                            databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments(TableId,TableName,ColumnName,FileName,ClientID,SiteLocID,RegisterID,BlobContainer,CheckedOn) VALUES(" + dayDocketId + ",'tblDayDocket','SupervisorSign','" + fileName.Replace("'", "''") + "'," + clientId + "," + siteLocId + ",NULL,'siteimage',GETDATE())");
                            inserted++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    failed++;
                    errors.Add("DayDocket-SupervisorSign: " + dr["SupervisorSign"] + " => " + ex.Message);
                }

                try
                {
                    string fileName = dr["ClientSign"].ToString().Trim();

                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        if (!databaseCon.BlobFileExists(fileName, "siteimage"))
                        {
                            databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments(TableId,TableName,ColumnName,FileName,ClientID,SiteLocID,RegisterID,BlobContainer,CheckedOn) VALUES(" + dayDocketId + ",'tblDayDocket','ClientSign','" + fileName.Replace("'", "''") + "'," + clientId + "," + siteLocId + ",NULL,'siteimage',GETDATE())");
                            inserted++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    failed++;
                    errors.Add("DayDocket-ClientSign: " + dr["ClientSign"] + " => " + ex.Message);
                }
            }

            string docImageQuery = "SELECT d.DocumentId,d.DocumentName,d.ClientID,d.SiteLocID,d.RegisterID FROM tblDocuments d LEFT JOIN tblRegister r ON d.RegisterID=r.RegisterId WHERE d.ClientID=" + clientId + " AND ((d.RegisterID IS NOT NULL AND r.SiteLocId=" + siteLocId + ") OR (d.RegisterID IS NULL AND d.SiteLocID=" + siteLocId + ")) AND ISNULL(d.DocumentName,'')<>'' AND ISNULL(d.SiteLocID,'')<>''";

            DataTable dtDocumentsImage = databaseCon.GetData(docImageQuery);

            foreach (DataRow dr in dtDocumentsImage.Rows)
            {
                try
                {
                    int documentId = Convert.ToInt32(dr["DocumentId"]);
                    string fileName = Convert.ToString(dr["DocumentName"]);
                    string registerId = dr["RegisterID"] == DBNull.Value ? "NULL" : dr["RegisterID"].ToString();
                    string docSiteLocId = dr["SiteLocID"] == DBNull.Value ? siteLocId.ToString() : dr["SiteLocID"].ToString();

                    if (string.IsNullOrWhiteSpace(fileName))
                    {
                        errors.Add("Document Id " + documentId + " : File Name is empty.");
                    }

                    else
                    {
                        if (!databaseCon.BlobFileExists(fileName, "siteimage"))
                        {
                            databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments(TableId,TableName,ColumnName,FileName,ClientID,SiteLocID,RegisterID,BlobContainer,CheckedOn) VALUES(" + documentId + ",'tblDocuments','DocumentName','" + fileName.Replace("'", "''") + "'," + clientId + "," + docSiteLocId + "," + registerId + ",'siteimage',GETDATE())");
                            inserted++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    failed++;
                    errors.Add("DocumentName: " + dr["DocumentName"] + " => " + ex.Message);
                }
            }

            string deliveryDocketQuery = "SELECT dd.DelDocId, dd.DepotPersonSign, dd.TransportDriverSign, dd.SitePersonSign, dd.AcceptorSign, dd.DepotPic, dd.SitePic FROM tblTransaction t INNER JOIN tblDeliveryDocket dd ON t.DeliveryId=dd.DeliveryID AND t.ClientId = dd.ClientId WHERE t.ClientID=" + clientId + " AND ((t.[From]='Site' AND t.FromId=" + siteLocId + ") OR (t.[To]='Site' AND t.ToId=" + siteLocId + ")) AND (ISNULL(dd.DepotPersonSign,'')<>'' OR ISNULL(dd.TransportDriverSign,'')<>'' OR ISNULL(dd.SitePersonSign,'')<>'' OR ISNULL(dd.AcceptorSign,'')<>'' OR ISNULL(dd.DepotPic,'')<>'' OR ISNULL(dd.SitePic,'')<>'') Union SELECT dd.DelDocId, dd.DepotPersonSign, dd.TransportDriverSign, dd.SitePersonSign, dd.AcceptorSign, dd.DepotPic, dd.SitePic FROM tblTransaction t INNER JOIN tblDeliveryDocket dd ON t.DeliveryId = dd.DeliveryID and t.ClientId = dd.ClientId WHERE t.ClientID = " + clientId + " AND(t.[From] = 'Depot' AND t.[To] = 'ThirdParty') AND(ISNULL(dd.DepotPersonSign, '') <> '' OR ISNULL(dd.TransportDriverSign, '') <> '' OR ISNULL(dd.SitePersonSign, '') <> '' OR ISNULL(dd.AcceptorSign, '') <> '' OR ISNULL(dd.DepotPic, '') <> '' OR ISNULL(dd.SitePic, '') <> '')";

            DataTable dtDeliveryDocket = databaseCon.GetData(deliveryDocketQuery);

            foreach (DataRow dr in dtDeliveryDocket.Rows)
            {
                int delDocId = Convert.ToInt32(dr["DelDocId"]);

                try
                {
                    string fileName = dr["DepotPersonSign"].ToString().Trim();

                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        if (!databaseCon.BlobFileExists(fileName, "siteimage"))
                        {
                            databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments(TableId,TableName,ColumnName,FileName,ClientID,SiteLocID,RegisterID,BlobContainer,CheckedOn) VALUES(" + delDocId + ",'tblDeliveryDocket','DepotPersonSign','" + fileName.Replace("'", "''") + "'," + clientId + "," + siteLocId + ",NULL,'siteimage',GETDATE())");
                            inserted++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    failed++;
                    errors.Add("DepotPersonSign: " + dr["DepotPersonSign"] + " => " + ex.Message);
                }

                try
                {
                    string fileName = dr["TransportDriverSign"].ToString().Trim();

                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        if (!databaseCon.BlobFileExists(fileName, "siteimage"))
                        {
                            databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments(TableId,TableName,ColumnName,FileName,ClientID,SiteLocID,RegisterID,BlobContainer,CheckedOn) VALUES(" + delDocId + ",'tblDeliveryDocket','TransportDriverSign','" + fileName.Replace("'", "''") + "'," + clientId + "," + siteLocId + ",NULL,'siteimage',GETDATE())");
                            inserted++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    failed++;
                    errors.Add("TransportDriverSign: " + dr["TransportDriverSign"] + " => " + ex.Message);
                }

                try
                {
                    string fileName = dr["SitePersonSign"].ToString().Trim();

                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        if (!databaseCon.BlobFileExists(fileName, "siteimage"))
                        {
                            databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments(TableId,TableName,ColumnName,FileName,ClientID,SiteLocID,RegisterID,BlobContainer,CheckedOn) VALUES(" + delDocId + ",'tblDeliveryDocket','SitePersonSign','" + fileName.Replace("'", "''") + "'," + clientId + "," + siteLocId + ",NULL,'siteimage',GETDATE())");
                            inserted++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    failed++;
                    errors.Add("SitePersonSign: " + dr["SitePersonSign"] + " => " + ex.Message);
                }

                try
                {
                    string fileName = dr["AcceptorSign"].ToString().Trim();

                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        if (!databaseCon.BlobFileExists(fileName, "siteimage"))
                        {
                            databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments(TableId,TableName,ColumnName,FileName,ClientID,SiteLocID,RegisterID,BlobContainer,CheckedOn) VALUES(" + delDocId + ",'tblDeliveryDocket','AcceptorSign','" + fileName.Replace("'", "''") + "'," + clientId + "," + siteLocId + ",NULL,'siteimage',GETDATE())");
                            inserted++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    failed++;
                    errors.Add("AcceptorSign: " + dr["AcceptorSign"] + " => " + ex.Message);
                }

                try
                {
                    string fileName = dr["DepotPic"].ToString().Trim();

                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        if (!databaseCon.BlobFileExists(fileName, "siteimage"))
                        {
                            databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments(TableId,TableName,ColumnName,FileName,ClientID,SiteLocID,RegisterID,BlobContainer,CheckedOn) VALUES(" + delDocId + ",'tblDeliveryDocket','DepotPic','" + fileName.Replace("'", "''") + "'," + clientId + "," + siteLocId + ",NULL,'siteimage',GETDATE())");
                            inserted++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    failed++;
                    errors.Add("DepotPic: " + dr["DepotPic"] + " => " + ex.Message);
                }

                try
                {
                    string fileName = dr["SitePic"].ToString().Trim();

                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        if (!databaseCon.BlobFileExists(fileName, "siteimage"))
                        {
                            databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments(TableId,TableName,ColumnName,FileName,ClientID,SiteLocID,RegisterID,BlobContainer,CheckedOn) VALUES(" + delDocId + ",'tblDeliveryDocket','SitePic','" + fileName.Replace("'", "''") + "'," + clientId + "," + siteLocId + ",NULL,'siteimage',GETDATE())");
                            inserted++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    failed++;
                    errors.Add("SitePic: " + dr["SitePic"] + " => " + ex.Message);
                }
            }

            string deliveryDocumentQuery = "SELECT ddd.DDDocumentId, ddd.DocumentName FROM tblTransaction t INNER JOIN tblDeliveryDocketDocuments ddd ON t.TransId=ddd.TransID AND t.ClientId = ddd.ClientId WHERE t.ClientID=" + clientId + " AND ((t.[From]='Site' AND t.FromId=" + siteLocId + ") OR (t.[To]='Site' AND t.ToId=" + siteLocId + ")) AND ISNULL(ddd.DocumentName,'')<>'' UNION SELECT ddd.DDDocumentId, ddd.DocumentName FROM tblTransaction t INNER JOIN tblDeliveryDocketDocuments ddd ON t.TransId = ddd.TransID AND t.ClientId = ddd.ClientId WHERE t.ClientID = " + clientId + " AND(t.[From] = 'Depot' AND t.[To] = 'ThirdParty') AND ISNULL(ddd.DocumentName,'')<> ''";

            DataTable dtDeliveryDocument = databaseCon.GetData(deliveryDocumentQuery);

            foreach (DataRow dr in dtDeliveryDocument.Rows)
            {
                int ddDocumentId = Convert.ToInt32(dr["DDDocumentId"]);

                try
                {
                    string fileName = dr["DocumentName"].ToString().Trim();

                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        if (!databaseCon.BlobFileExists(fileName, "siteimage"))
                        {
                            databaseCon.RunCmd("INSERT INTO tblMissingBlobDocuments(TableId,TableName,ColumnName,FileName,ClientID,SiteLocID,RegisterID,BlobContainer,CheckedOn) VALUES(" + ddDocumentId + ",'tblDeliveryDocketDocuments','DocumentName','" + fileName.Replace("'", "''") + "'," + clientId + "," + siteLocId + ",NULL,'siteimage',GETDATE())");
                            inserted++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    failed++;
                    errors.Add("DocumentName: " + dr["DocumentName"] + " => " + ex.Message);
                }
            }

            if (errors.Count > 0)
            {
                return Json(new
                {
                    Result = "Warning",
                    Inserted = inserted,
                    Failed = failed,
                    Message = string.Join("\\n", errors)
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                Result = "Success",
                Inserted = inserted,
                Failed = failed,
                Message = inserted + " missing blob records inserted successfully."
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetClientDetails(string clientId, string userId)
        {
            clientId = databaseCon.Decrypt(clientId);
            userId = databaseCon.Decrypt(userId);
            string clientName = "";
            string userRole = "";
            DataSet dsClient = databaseCon.GetDataSet("SELECT Name FROM tblClient WHERE ClientID=" + clientId);
            if (dsClient.Tables.Count > 0 && dsClient.Tables[0].Rows.Count > 0)
            {
                clientName = Convert.ToString(dsClient.Tables[0].Rows[0]["Name"]);
            }
            DataSet dsUser = databaseCon.GetDataSet("SELECT Role FROM tblUser WHERE UserID=" + userId);
            if (dsUser.Tables.Count > 0 && dsUser.Tables[0].Rows.Count > 0)
            {
                userRole = Convert.ToString(dsUser.Tables[0].Rows[0]["Role"]);
            }
            return Json(new
            {
                ClientName = clientName,
                UserRole = userRole
            }, JsonRequestBehavior.AllowGet);
        }

    }
}
