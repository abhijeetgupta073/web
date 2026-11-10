using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure.Storage.Blob;
using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;

namespace Scaffold.Controllers
{
    public class InspectionListController : Controller
    {
        ScaffoldEntities dbcontext = new ScaffoldEntities();
        // GET: InspectionList
        public ActionResult InspectionList()
        {
            string query = "select UserId from tblUser where IsInspector='true' and userid= '" + databaseCon.Decrypt(Request.QueryString["UID"]) + "' and clientid='" + databaseCon.Decrypt(Request.QueryString["id"]) + "'";
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["UserId"].ToString() != "0")
                {
                    ViewBag.UserId = dt.Rows[0]["UserId"].ToString();
                }
            }

            DataTable dt1 = databaseCon.GetData("select isnull(tblSiteLocation.OffSystemInspection ,0) as OffSystemInspection   from tblSiteLocation join tblRegister on tblSiteLocation.SiteLocID=tblRegister.SiteLocId where RegisterId='" + databaseCon.Decrypt(Request.QueryString["RID"]) + "'");
            if (dt1.Rows[0]["OffSystemInspection"].ToString() == "True")
            {
                 ViewBag.OffSystemInspection = true;
                //LinkButton LinkButton1 = (LinkButton)item.FindControl("btnDeplank");
                //LinkButton LinkButton2 = (LinkButton)item.FindControl("btnadddocs");
                //LinkButton1.Visible = false;
                //LinkButton2.Visible = false;
            }

            DataTable dt2 = databaseCon.GetData("select isnull(tblSiteLocation.CycloneManagement ,0) as CycloneManagement   from tblSiteLocation join tblRegister on tblSiteLocation.SiteLocID=tblRegister.SiteLocId where RegisterId='" + databaseCon.Decrypt(Request.QueryString["RID"]) + "'");
            if (dt2.Rows[0]["CycloneManagement"].ToString() == "False")
            {
                ViewBag.CycloneManagement = false;
            }

            return View();
        }
        public ActionResult BindallDropdown(string id)
        {
        
            var ddlInspectedBy = dbcontext.Database.SqlQuery<DropDownViewModel>("select firstName+ ' '+LastName as name,UserId,LicenceNo from tblUser where IsInspector='true' and Status='Active' and clientid='" + databaseCon.Decrypt(id) + "' Order by name");

            var ddlChecklistInsp = dbcontext.Database.SqlQuery<DropDownViewModel>("select distinct tblCheckList.checklistID, checklistdescription from tblchecklist join tblcheckListContent on tblchecklist.ChecklistId = tblcheckListContent.ChecklistId and  tblCheckList.ClientId = tblchecklistcontent.clientid where tblCheckList.clientid = '" + databaseCon.Decrypt(id) + "' and[status] = 'Active' order by checklistdescription");
            
            return Json(new { ddlInspectedBy = ddlInspectedBy, ddlChecklistInsp = ddlChecklistInsp}, JsonRequestBehavior.AllowGet);

        }
        public ActionResult ShowInspectionListDetails(string id, string RID,string hdnTimeOffSet,string HistInspList)
        {

            //var GetData = dbcontext.Database.SqlQuery<Pro_TBLINSPECTION_GETModelView>("exec Pro_TBLINSPECTION_GET '','" + databaseCon.Decrypt(RID) + "'").ToList();
            var GetData = dbcontext.Database.SqlQuery<Pro_TBLINSPECTION_GETModelView>("exec Pro_TBLINSPECTION_GET '','" + databaseCon.Decrypt(RID) + "','" + HistInspList + "'").ToList();
            foreach (var item in GetData)
            {
                var InspectionDate = item.InspectionDate;
                if (InspectionDate != null)
                    {
                    item.InspectionDate = Convert.ToDateTime(InspectionDate).AddMinutes(Convert.ToDouble(hdnTimeOffSet));
                    }
                var ScheduledOrAckDate = item.ScheduledOrAckDate;
                if (ScheduledOrAckDate != null)
                    {
                    item.ScheduledOrAckDate = Convert.ToDateTime(ScheduledOrAckDate).AddMinutes(Convert.ToDouble(hdnTimeOffSet));
                    }
                }

            return Json(GetData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult btndeplanksave_Click(string id,string UID,string RID,string txtcomm, string txtdeplanl,string ddlInspecdeplank,string dateInspection,string hdnTimeOffSet)
        {
            var Result = "";
            var Message = "";
            bool chkOnline = true;
            bool complete = true;
            string inspbyQuery = "select UserID, firstName+ ' '+LastName as name,LicenceNo from tblUser where IsInspector='true' and UserId='" + ddlInspecdeplank+ "' order by name";
            DataTable inspDt = databaseCon.GetData(inspbyQuery);
            var AcknowladgeDateOffSet = Convert.ToDouble(hdnTimeOffSet);
            DateTime ScheduleInspectionDate = Convert.ToDateTime(dateInspection).AddMinutes(AcknowladgeDateOffSet);

            // Change On 24 Jan 2020 Add InspectionTimeZone To Diffrent Between TimeZone  And Inspection Date Save To Utc Time Zone  And Acknowladgement Date= Inspection Date  and  AcknowladgementTimeZone=InspectionTimeZone
            string query = "exec Pro_TBLINSPECTION_SAVE   '0','" + ScheduleInspectionDate + "','" + inspDt.Rows[0]["UserID"].ToString() + "','','" + databaseCon.Decrypt(RID) + "','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "','" + chkOnline + "','" + complete + "','','','','','','','','" + txtcomm.Replace("'", "''").Trim() + "','','" + txtdeplanl+ "','" + ScheduleInspectionDate + "','" + inspDt.Rows[0]["LicenceNo"].ToString() + "','','" + databaseCon.Decrypt(id) + "'," + "0" + ",'" + databaseCon.Decrypt(UID) + "'," + ((AcknowladgeDateOffSet) * (-1)) + "," + ((AcknowladgeDateOffSet) * (-1)) + "";


            // Change On 14 Jan 2020 update Acknowladgement Date= Inspection Date
            // string query = "exec Pro_TBLINSPECTION_SAVE   '0','" + dateInspection.SelectedDate + "','" + inspDt.Rows[0]["UserID"].ToString() + "','" + txtComments.Text.Replace("'", "''").Trim() + "','" + databaseCon.Decrypt(Request.QueryString["RID"]) + "','" + databaseCon.Decrypt(Request.QueryString["UID"]) + "','" + DateTime.Now + "','" + chkOnline.Checked + "','" + complete + "','','','','','','','','" + txtcomm.Text.Replace("'", "''").Trim() + "','','" + txtdeplanl.Text + "','" + dateInspection.SelectedDate + "','" + inspDt.Rows[0]["LicenceNo"].ToString() + "','" + ddlChecklistInsp.SelectedValue + "','" + databaseCon.Decrypt(Request.QueryString["id"]) + "'," + "0" + ",'" + databaseCon.Decrypt(Request.QueryString["UID"]) + "'";

            // 26 Nov 2019
            //   string query = "exec Pro_TBLINSPECTION_SAVE   '0','" + dateInspection.SelectedDate + "','" + inspDt.Rows[0]["UserID"].ToString() + "','" + txtComments.Text.Replace("'", "''").Trim() + "','" + databaseCon.Decrypt(Request.QueryString["RID"]) + "','" + databaseCon.Decrypt(Request.QueryString["UID"]) + "','" + DateTime.Now + "','" + chkOnline.Checked + "','" + complete + "','','','','','','','','" + txtcomm.Text.Replace("'", "''").Trim() + "','','" + txtdeplanl.Text + "','','" + inspDt.Rows[0]["LicenceNo"].ToString() + "','" + ddlChecklistInsp.SelectedValue + "','" + databaseCon.Decrypt(Request.QueryString["id"]) + "'," + "0" + ",'" + databaseCon.Decrypt(Request.QueryString["UID"]) + "'";

            // string query = "exec Pro_TBLINSPECTION_SAVE   '0','" + dateInspection.SelectedDate + "','" + inspDt.Rows[0]["UserID"].ToString() + "','" + txtComments.Text.Replace("'", "''").Trim() + "','" + databaseCon.Decrypt(Request.QueryString["RID"]) + "','" + databaseCon.Decrypt(Request.QueryString["UID"]) + "','" + DateTime.Now + "','" + chkOnline.Checked + "','" + complete + "','','','','','','','','" + txtcomm.Text.Replace("'", "''").Trim() + "','','" + txtdeplanl.Text + "','','" + inspDt.Rows[0]["LicenceNo"].ToString() + "','" + ddlChecklistInsp.SelectedValue + "','" + databaseCon.Decrypt(Request.QueryString["id"]) + "'";


            //}
            DataTable Deplankdt = databaseCon.GetData(query);
            if (Deplankdt.Rows.Count != 0)
            {
                if (Convert.ToInt32(Deplankdt.Rows[0][0]) > 0)
                {
                    Result = "Success";
                    Message = Deplankdt.Rows[0][1].ToString();
                }
                else
                {
                    Result = "Failed";
                    Message = Deplankdt.Rows[0][1].ToString();
                }
            }
            else
            {
                Result = "Failed";
                Message = "Syntax Error. Contact administrator";
            }
            return Json(new { Result=Result,Message=Message}, JsonRequestBehavior.AllowGet);
        }

       public ActionResult btnsave_Click(string id, string UID, string RID, string ddlddlInspectedByadd,string hdnTimeOffSet,string ddlInspStatuspop,string ddlChecklistInsp,string rdpInspectiondate)
        {
            var Result = "";
            var Message = "";
            var Data = "";
            string query;
            DataTable Inspectiondt;
            bool chkOnline = true;
            bool complete =  false;
            string inspbyQuery = "select UserID, firstName+ ' '+LastName as name,LicenceNo from tblUser where IsInspector='true' and UserId='" + ddlddlInspectedByadd + "'";
            DataTable inspDt = databaseCon.GetData(inspbyQuery);

            var AcknowladgeDateOffSet = Convert.ToDouble(hdnTimeOffSet);
            DateTime ScheduleInspectionDate = Convert.ToDateTime(rdpInspectiondate).AddMinutes(AcknowladgeDateOffSet);

            DateTime date = Convert.ToDateTime(rdpInspectiondate);




            //  query = "exec Pro_TBLINSPECTION_SAVE   '0','" + rdpInspectiondate.SelectedDate + "','" + inspDt.Rows[0]["UserID"].ToString() + "','" + txtComments.Text.Replace("'", "''").Trim() + "','" + databaseCon.Decrypt(Request.QueryString["RID"]) + "','" + databaseCon.Decrypt(Request.QueryString["UID"]) + "','" + DateTime.Now + "','" + chkOnline.Checked + "','" + complete + "','','','','','','','','','','" + ddlInspStatuspop.SelectedValue + "','','" + inspDt.Rows[0]["LicenceNo"].ToString() + "','" + ddlChecklistInsp.SelectedValue + "','" + databaseCon.Decrypt(Request.QueryString["id"]) + "'";



            //  Changes done on 26 - Nov - 2019
            //  query = "exec Pro_TBLINSPECTION_SAVE   '0','" + rdpInspectiondate.SelectedDate + "','" +
            //     inspDt.Rows[0]["UserID"].ToString() + "','" + txtComments.Text.Replace("'", "''").Trim() + "','" + databaseCon.Decrypt(Request.QueryString["RID"]) + "','" + databaseCon.Decrypt(Request.QueryString["UID"]) + "','" + DateTime.Now + "','" + chkOnline.Checked + "','" + complete + "','','','','','','','','','','" + ddlInspStatuspop.SelectedValue + "','','" + inspDt.Rows[0]["LicenceNo"].ToString() + "','" + ddlChecklistInsp.SelectedValue + "','" + databaseCon.Decrypt(Request.QueryString["id"]) + "'," + "0" + ",'" + databaseCon.Decrypt(Request.QueryString["UID"]) + "'";

            // Change On 24 Jan 2020 Add InspectionTimeZone To Diffrent Between TimeZone  And Inspection Date Save To Utc Time Zone
            query = "exec Pro_TBLINSPECTION_SAVE   '0','" + ScheduleInspectionDate + "','" +
                      inspDt.Rows[0]["UserID"].ToString() + "','','" + databaseCon.Decrypt(RID) + "','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "','" + chkOnline+ "','" + complete + "','','','','','','','','','','" + ddlInspStatuspop + "','','" + inspDt.Rows[0]["LicenceNo"].ToString() + "','" + ddlChecklistInsp + "','" + databaseCon.Decrypt(id) + "'," + "0" + ",'" + databaseCon.Decrypt(UID) + "'," + ((AcknowladgeDateOffSet) * (-1)) + ",0";

            //}
            Inspectiondt = databaseCon.GetData(query);
            if (Inspectiondt.Rows.Count != 0)
            {
                if (Convert.ToInt32(Inspectiondt.Rows[0][0]) > 0)
                {
                    //string ddlChecklistvalue = ddlChecklistInsp.SelectedValue;
                    //ShowregisterDetails();
                    //UpdateImagePath();

                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Success ! </strong>" + Inspectiondt.Rows[0][1].ToString();


                    //Response.Redirect(DomainName + "/" + "InspectionForm?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RID=" + Request.QueryString["RID"] + "&CheckListID=" + databaseCon.Encrypt(ddlChecklistvalue) + "&InspID=" + databaseCon.Encrypt(Convert.ToInt32(Inspectiondt.Rows[0][0]).ToString()) + "&FormPage=" + "");

                    Result = "Success";
                    Message = Inspectiondt.Rows[0][1].ToString();
                   Data=databaseCon.Encrypt(ddlChecklistInsp)+"_"+ databaseCon.Encrypt((Inspectiondt.Rows[0][0]).ToString());
                }
                else
                {
                    Result = "Failed";
                    Message = Inspectiondt.Rows[0][1].ToString();
                }
            }
            else
            {
                Result = "Failed";
                Message = "Syntax Error. Contact administrator";
            }

            return Json(new { Result = Result, Message = Message, Data=Data }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult btndeleteyes_Click(string hdnInspectionID)
        {
            var Result = "";
            var Message = "";
            string query;
            DataTable dtinsp;
            query = "exec  Pro_TBLInspection_DEL'" + hdnInspectionID + "'";
            dtinsp = databaseCon.GetData(query);
            if (dtinsp.Rows.Count != 0)
            {

                if (Convert.ToInt32(dtinsp.Rows[0][0]) > 0)
                {
                    Result = "Success";
                    Message = dtinsp.Rows[0][1].ToString();
                }
                else
                {
                    Result = "Failed";
                    Message = dtinsp.Rows[0][1].ToString();
                }
            }
            else
            {
                Result = "Failed";
                Message = "Syntax Error. Contact administrator";
            }

            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetCheckListID(string InspectionID) {
            var Result = "";
            var Message = "";
            DataTable dt = databaseCon.GetData("select distinct ChecklistId from tblinspectionList where InspectionID='" + InspectionID + "'");
            if(dt.Rows.Count>0)
            {
                var ChecklistId = dt.Rows[0]["ChecklistId"].ToString();
                Result = "Success";
                Message =databaseCon.Encrypt(ChecklistId)+"_"+ databaseCon.Encrypt(InspectionID);
            }
            else {
                Result = "Failed";
                Message = "No Record Found.";
            }
            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewPdfDocument(string id,string InspID)
        {
            var Status = "";
            var Message = "";
            try
            {
                DataTable dt = databaseCon.GetData("select InspFileName from tblInspection where CompleteInsp=1 and (InspStatus='Confirmed' Or InspStatus='Out of Service') and InspectionID='" + InspID + "'");
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["InspFileName"].ToString() != "")
                    {
                        string path = databaseCon.registerdocumentlink + dt.Rows[0]["InspFileName"].ToString();
                        // ScriptManager.RegisterStartupScript(this, GetType(), "newtab", "window.open('" + path + "')", true);
                        Status = "Success";
                        Message = path;

                    }
                    if (dt.Rows[0]["InspFileName"].ToString() == "")
                    {
                        var Result = databaseCon.InspectionPdfReportCreate(Convert.ToInt32(databaseCon.Decrypt(id)), Convert.ToInt32(InspID));

                        var NewResult = Result.Split('|');
                        var FileName = NewResult[0];
                        var PdfFileFullPath = NewResult[1];


                        string path = databaseCon.registerdocumentlink + FileName;
                        var uri = new Uri(databaseCon.registerdocumentlink);
                        var container = new CloudBlobContainer(uri);
                        var blob = container.GetBlockBlobReference(FileName);
                        var exists = blob.Exists();
                        if (exists == false)
                        {
                        }
                        else
                        {
                            databaseCon.deletefileonblob(FileName, "registerdocument");
                        }
                        string extension = Path.GetExtension(".pdf");
                        if (extension.Replace(".", "") == "pdf")
                        {
                            extension = "application/pdf";
                        }

                        databaseCon.savefileonblob(FileName, "Inspection-Document", PdfFileFullPath, extension.Replace(".", ""));
                        // ScriptManager.RegisterStartupScript(this, GetType(), "newtab", "window.open('" + path + "')", true);
                        Status = "Success";
                        Message = path;
                        if (System.IO.File.Exists(PdfFileFullPath))
                        {
                            try
                            {
                                System.IO.File.Delete(PdfFileFullPath);
                            }
                            catch
                            {

                            }
                        }

                    }

                }
                else
                {

                    var Result = databaseCon.InspectionPdfReportCreate(Convert.ToInt32(databaseCon.Decrypt(id)), Convert.ToInt32(InspID));
                    var NewResult = Result.Split('|');
                    var FileName = NewResult[0];
                    var PdfFileFullPath = NewResult[1];


                    string path = databaseCon.registerdocumentlink + FileName;
                    var uri = new Uri(databaseCon.registerdocumentlink);
                    var container = new CloudBlobContainer(uri);
                    var blob = container.GetBlockBlobReference(FileName);
                    var exists = blob.Exists();
                    if (exists == false)
                    {
                    }
                    else
                    {
                        databaseCon.deletefileonblob(FileName, "registerdocument");
                    }
                    string extension = Path.GetExtension(".pdf");
                    if (extension.Replace(".", "") == "pdf")
                    {
                        extension = "application/pdf";
                    }

                    databaseCon.savefileonblob(FileName, "Inspection-Document", PdfFileFullPath, extension.Replace(".", ""));
                    // ScriptManager.RegisterStartupScript(this, GetType(), "newtab", "window.open('" + path + "')", true);
                    Status = "Success";
                    Message = path;
                    if (System.IO.File.Exists(PdfFileFullPath))
                    {
                        try
                        {
                            System.IO.File.Delete(PdfFileFullPath);
                        }
                        catch
                        {

                        }
                    }

                }
            }catch(Exception ex) {
                Status = "Failed";
                Message = ex.Message;
            }

            return Json(new { Result = Status, Message = Message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult btnEmailClick(string id,string UID,string UserRole,string InspID,string RID,string DomainName) {

            var Status = "";
            var Message = "";
            var txtFile = "";
            var txttempfile = "";
            var txtcc = "";
            var txtClientEmail = "";
            var txtsubject = "";
            var msg = "";
            var txtEmailMSg = "";
            //if (e.CommandName == "Email")
            //{
            //    GridDataItem gdi = (GridDataItem)e.Item;
            //    string InspID = Convert.ToString(gdi.GetDataKeyValue("InspectionID"));
            //    InspIDPDF = InspID;

            var pdfReport = "";
                var pdfmsg = "Please see below link for Scaffold Inspection Pdf Report.";

                DataTable dtInsp = databaseCon.GetData("select InspFileName from tblInspection where CompleteInsp=1 and (InspStatus='Confirmed' Or InspStatus='Out of Service') and InspectionID='" + InspID + "'");
                if (dtInsp.Rows.Count > 0)   // If Inspection Is Confirmed And Out Of Services
                {
                    if (dtInsp.Rows[0]["InspFileName"].ToString() != "")
                    {
                        string path = databaseCon.registerdocumentlink + dtInsp.Rows[0]["InspFileName"].ToString();
                    // string path1 = Server.MapPath("~/Report/" + dtInsp.Rows[0]["InspFileName"].ToString());
                    // txtFile.Text = path;
                    // txttempfile.Text = dtInsp.Rows[0]["InspFileName"].ToString();  // Set File Name 

                     txtFile = path;
                     txttempfile = dtInsp.Rows[0]["InspFileName"].ToString();  // Set File Name 
                    pdfReport = path;
                    }
                    if (dtInsp.Rows[0]["InspFileName"].ToString() == "")
                    {
                        // No Flie Name In Database And Inspection Status Confirmed
                        //  txtFile.Text = "";
                        //   txttempfile.Text = "File does not exists.";

                        // if Old  Inspection Is Before(14 Dec 2019)(Complete OR Out Of Service) Then Create Pdf And Show  Not Save Data on  DataBase Of Blob Directory Every Time Create New Pdf 

                        var Result = databaseCon.InspectionPdfReportCreate(Convert.ToInt32(databaseCon.Decrypt(id)), Convert.ToInt32(InspID));
                        var NewResult = Result.Split('|');
                        var FileName = NewResult[0];
                        var PdfFileFullPath = NewResult[1];


                        string path = databaseCon.registerdocumentlink + FileName;
                        var uri = new Uri(databaseCon.registerdocumentlink);
                        var container = new CloudBlobContainer(uri);
                        var blob = container.GetBlockBlobReference(FileName);
                        var exists = blob.Exists();
                        if (exists == false)
                        {
                        }
                        else
                        {
                            databaseCon.deletefileonblob(FileName, "registerdocument");
                        }
                        string extension = Path.GetExtension(".pdf");
                        if (extension.Replace(".", "") == "pdf")
                        {
                            extension = "application/pdf";
                        }

                        databaseCon.savefileonblob(FileName, "Inspection-Document", PdfFileFullPath, extension.Replace(".", ""));
                        // ScriptManager.RegisterStartupScript(this, GetType(), "newtab", "window.open('" + path + "')", true);
                        if (System.IO.File.Exists(PdfFileFullPath))
                        {
                            try
                            {
                            System.IO.File.Delete(PdfFileFullPath);
                            }
                            catch
                            {

                            }
                        }

                        pdfReport = path;

                        // Code Comment On 23 Dec 2019

                        //showreport();
                        //string path = databaseCon.registerdocumentlink + txttempfile.Text.ToString();
                        //var uri = new Uri(databaseCon.registerdocumentlink);
                        //var container = new CloudBlobContainer(uri);
                        //var blob = container.GetBlockBlobReference(txttempfile.Text.ToString());
                        //var exists = blob.Exists();
                        //if (exists == false)
                        //{
                        //}
                        //else
                        //{
                        //    databaseCon.deletefileonblob(txttempfile.Text, "registerdocument");
                        //}
                        //string extension = Path.GetExtension(".pdf");
                        //if (extension.Replace(".", "") == "pdf")
                        //{
                        //    extension = "application/pdf";
                        //}

                        //databaseCon.savefileonblob(txttempfile.Text, "Inspection-Document", txtFile.Text, extension.Replace(".", ""));

                        //if (File.Exists(txtFile.Text))
                        //{
                        //    try
                        //    {
                        //        File.Delete(txtFile.Text);
                        //    }
                        //    catch
                        //    {

                        //    }
                        //}

                        // pdfReport = path;

                    }

                    DataTable dtclientinfo = databaseCon.GetData(" select DistributionList,EmailID from VW_TblRegister join tblSiteLocation on tblSiteLocation.SiteLocID=VW_TblRegister.SiteLocId join tblClient on tblClient.ClientId = VW_TblRegister.ClientID where RegisterId=" + databaseCon.Decrypt(RID) + "");

                    if (dtclientinfo.Rows.Count > 0)
                    {
                    //txtClientEmail.Text = Convert.ToString(dtclientinfo.Rows[0]["DistributionList"]);
                    //txtcc.Text = Convert.ToString(dtclientinfo.Rows[0]["EmailID"]);

                    // To send Email On cc Change On 12 Dec 2019

                    //  txtcc.Text = Convert.ToString(dtclientinfo.Rows[0]["DistributionList"]);
                    //  txtClientEmail.Text = Convert.ToString(dtclientinfo.Rows[0]["EmailID"]);
                   
                    //commented on 08 july 2025 for client do not want to company email anywhere
                    //txtcc= Convert.ToString(dtclientinfo.Rows[0]["DistributionList"]);
                    //txtClientEmail = Convert.ToString(dtclientinfo.Rows[0]["EmailID"]);

                    txtClientEmail = Convert.ToString(dtclientinfo.Rows[0]["DistributionList"]);
                   
                }
                    DataTable dtemailinfo = databaseCon.GetData("select Subject,Body from [tblEmailContent] where Purpose = 'Inspection' and clientid='" + databaseCon.Decrypt(id) + "' ");
                    if (dtemailinfo.Rows.Count > 0)
                    {
                   txtsubject = Convert.ToString(dtemailinfo.Rows[0]["Subject"]);
                    msg = Convert.ToString(dtemailinfo.Rows[0]["Body"]);
                }

                //  ViewState["InspectionID"] = gdi.GetDataKeyValue("InspectionID");

                    DataTable dt = databaseCon.GetData("select distinct ChecklistId from tblinspectionList where InspectionID='" + InspID + "'");


                // string Emaillink = DomainName + "/" + "InspectionForm?UID=" + Request.QueryString["UID"].Trim().Replace(" ", "+") + "&UserRole=" + Request.QueryString["UserRole"].Trim().Replace(" ", "+") + "&id=" + Request.QueryString["id"].Trim().Replace(" ", "+") + "&RID=" + Request.QueryString["RID"].Trim().Replace(" ", "+") + "&CheckListID=" + databaseCon.Encrypt(dt.Rows[0]["CheckListID"].ToString()).Trim().Replace(" ", "+") + "&InspID=" + databaseCon.Encrypt(InspID).Trim().Replace(" ", "+") + "&FormPage=" + databaseCon.Encrypt("Email").Trim().Replace(" ", "+");

                string Emaillink = DomainName + "/" + "InspectionForm/InspectionForm?UID=" + UID.Trim().Replace(" ", "+") + "&UserRole=" + UserRole.Trim().Replace(" ", "+") + "&id=" + id.Trim().Replace(" ", "+") + "&RID=" +RID.Trim().Replace(" ", "+") + "&CheckListID=" + databaseCon.Encrypt(dt.Rows[0]["CheckListID"].ToString()).Trim().Replace(" ", "+") + "&InspID=" + databaseCon.Encrypt(InspID).Trim().Replace(" ", "+") + "&FormPage=" + databaseCon.Encrypt("Email").Trim().Replace(" ", "+");


                // txtEmailMSg.Text = pdfmsg + Environment.NewLine + pdfReport + Environment.NewLine + Environment.NewLine + msg + Environment.NewLine + Emaillink;

                txtEmailMSg = pdfmsg + Environment.NewLine + pdfReport + Environment.NewLine + Environment.NewLine + msg + Environment.NewLine + Emaillink;
                // emailConfirm.Style.Add("display", "block");
                Status = "Success";

            }
                else
                {
                //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                //art1.Style.Add("display", "block");
                //alertlabel.Text = "<strong>Alert ! </strong>An Inspection must be verified and Confirmed before e-mailing.";
                Status = "Failed";
                Message = "An Inspection must be verified and Confirmed before e-mailing.";
                }
            return Json(new {Result=Status,Message= Message ,txtFile= txtFile, txttempfile= txttempfile, txtcc= txtcc, txtClientEmail= txtClientEmail, txtsubject= txtsubject, txtEmailMSg=txtEmailMSg }, JsonRequestBehavior.AllowGet);
            
        }

        public ActionResult btnSend_Click(string txtClientEmail,string txtcc,string txtsubject,string txtEmailMSg,string txtFile,string InspectionID) {
            var Result = "";
            var Message = "";
           // emailConfirm.Style.Add("display", "none");
            if (databaseCon.SendEmail(txtClientEmail, txtcc, txtsubject, txtEmailMSg, txtFile))
            {
                int i = databaseCon.RunCmd("update tblInspection set LastEmailSentOn= '" + DateTime.Now + "' where InspectionID ="+ InspectionID + " ");
                //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                //art1.Style.Add("display", "block");
                //alertlabel.Text = "<strong>Success ! </strong>Email send Successfully.";
                //GridInspectionList.Rebind();
                Result = "Success";
                Message = "Email send Successfully.";

            }
            else
            {
                //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                //art1.Style.Add("display", "block");
                //alertlabel.Text = "<strong>Alert ! </strong>Try Again. ";
                Result = "Failed";
                Message = "Try Again. ";
            }
            if (System.IO.File.Exists(txtFile))
            {
                try
                {
                    System.IO.File.Delete(txtFile);
                }
                catch { }
            }
            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }
        
     }
}