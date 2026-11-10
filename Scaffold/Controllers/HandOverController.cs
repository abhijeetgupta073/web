using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Vml.Wordprocessing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.WindowsAzure.Storage.Blob;
using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;

namespace Scaffold.Controllers
{
    public class HandOverController : Controller
    {
        ScaffoldEntities dbcontext = new ScaffoldEntities();
        // GET: HandOver
        public ActionResult HandOver()
        {
            return View();
        }
        public ActionResult ShowHandOverListDetails(string id, string RID, string hdnTimeOffSet)
        {
            //string query = "exec Pro_TblHAndOver_GET '', '" + databaseCon.Decrypt(RID) + "'";
            var GetData = dbcontext.Database.SqlQuery<Pro_TblHAndOver_GET>("exec Pro_TblHAndOver_GET '', '" + databaseCon.Decrypt(RID) + "'").ToList();
            foreach (var item in GetData)
            {
                var HandOverDate = item.HandOverDate;
                if (HandOverDate != null)
                {
                    item.HandOverDate = Convert.ToDateTime(HandOverDate).AddMinutes(Convert.ToDouble(hdnTimeOffSet));
                }
            }

            return Json(GetData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult btndeleteyes_Click(string hdnHandOverID)
        {
            var Result = "";
            var Message = "";
            string query;
            DataTable dtinsp;
            query = "exec  Pro_TBLHandOver_DEL'" + hdnHandOverID + "'";
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

        //Added on 26Feb2025
        public ActionResult btnUnconfirmyes_Click(string hdnHandOverID)
        {
            var Result = "";
            var Message = "";
            string query;
            query = "update tblHandOver set IsConfirmed = 0 where IsConfirmed = 1 and HandOverId = '" + hdnHandOverID + "'";
            int dtUnconfirm = databaseCon.RunCmd(query);
            if (dtUnconfirm != 0)
            {

                if (Convert.ToInt32(dtUnconfirm) > 0)
                {
                    Result = "Success";
                    Message = "Record unconfirmed successfully";
                }
                else
                {
                    Result = "Failed";
                    Message = "Failed to unconfirm record";
                }
            }
            else
            {
                Result = "Failed";
                Message = "Syntax Error. Contact administrator";
            }

            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ViewPdfDocument(string id, string RID, string HandOverID)
        {
            var Status = "";
            var Message = "";

            //GridDataItem gdi = (GridDataItem)e.Item;
            //string InspID = Convert.ToString(gdi.GetDataKeyValue("InspID"));
            //insppdfid = InspID;
            //string HandOverID = Convert.ToString(gdi.GetDataKeyValue("Id"));
            //Handoverpdfid = HandOverID;
            var GetData = showreport(id, RID, HandOverID);
            var arrData = GetData.Split('~');
            var txttempfile = arrData[0];
            var txtFile = arrData[1];
            if (txttempfile != "" && txtFile != "")
            {
                string path = databaseCon.registerdocumentlink + txttempfile.ToString();
                var uri = new Uri(databaseCon.registerdocumentlink);
                var container = new CloudBlobContainer(uri);
                var blob = container.GetBlockBlobReference(txttempfile.ToString());
                var exists = blob.Exists();
                if (exists == false)
                {
                }
                else
                {
                    databaseCon.deletefileonblob(txttempfile, "registerdocument");
                }
                string extension = Path.GetExtension(".pdf");
                if (extension.Replace(".", "") == "pdf")
                {
                    extension = "application/pdf";
                }

                databaseCon.savefileonblob(txttempfile, "Inspection-Document", txtFile, extension.Replace(".", ""));
                // ScriptManager.RegisterStartupScript(this, GetType(), "newtab", "window.open('" + path + "')", true);
                Status = "Success";
                Message = path;
                if (System.IO.File.Exists(txtFile))
                {
                    try
                    {
                        System.IO.File.Delete(txtFile);
                    }
                    catch
                    {

                    }
                }
            }
            else
            {
                Status = "Failed";
                Message = "Pdf Documents Not Found.";
            }
            return Json(new { Result = Status, Message = Message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult btnEmailClick(string id, string UID, string UserRole, string HandOverID, string RID, string DomainName)
        {

            var Status = "";
            var Message = "";
            var txtFile = "";
            var txttempfile = "";
            var txtcc = "";
            var txtClientEmail = "";
            var txtsubject = "";
            var msg = "";
            var txtEmailMSg = "";


            DataTable dtclientinfo = databaseCon.GetData(" select DistributionList,EmailID from VW_TblRegister join tblSiteLocation on tblSiteLocation.SiteLocID=VW_TblRegister.SiteLocId join tblClient on tblClient.ClientId = VW_TblRegister.ClientID where RegisterId=" + databaseCon.Decrypt(RID) + "");
            if (dtclientinfo.Rows.Count > 0)
            {
                //Pdf is generated from the handover table Data goes to the table after confirm save. Pdf will only be attached on confirmed case.  Change On 31 July 2019

                DataTable dt = databaseCon.GetData("select isnull(IsConfirmed,0) as IsConfirmed  from tblHandOver where HandOverId = '" + HandOverID + "'");
                if (Convert.ToBoolean(dt.Rows[0]["IsConfirmed"]) == true)
                {
                    var GetData = showreport(id, RID, HandOverID);
                    var arrData = GetData.Split('~');
                    txttempfile = arrData[0];
                    txtFile = arrData[1];
                    if (txttempfile != "" && txtFile != "")
                    {
                        var uri = new Uri(databaseCon.registerdocumentlink);
                        var container = new CloudBlobContainer(uri);
                        var blob = container.GetBlockBlobReference(txttempfile.ToString());
                        var exists = blob.Exists();
                        if (exists == false)
                        {
                        }
                        //else
                        //{
                        //    //databaseCon.deletefileonblob(txttempfile.Text, "registerdocument");

                        //}
                        string extension = Path.GetExtension(".pdf");
                        if (extension.Replace(".", "") == "pdf")
                        {
                            extension = "application/pdf";
                        }

                        databaseCon.savefileonblob(txttempfile, "Inspection-Document", txtFile, extension.Replace(".", ""));
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
                    }
                    else
                    {
                        Status = "Failed";
                        Message = "Pdf Document Not Found.";
                    }

                }
                else
                {
                    string path = Server.MapPath("~/Report/HandOver_" + HandOverID + ".pdf");
                    txttempfile = "No file attached as Hand Over not yet confirmed.";
                    txtFile = path;

                    Status = "Success";
                    Message = path;
                }
                //txtClientEmail.Text = Convert.ToString(dtclientinfo.Rows[0]["DistributionList"]);
                //txtcc.Text = Convert.ToString(dtclientinfo.Rows[0]["EmailID"]);

                // To send Email On cc Change On 12 Dec 2019
                //commented on 08 july 2025 for client do not want to company email anywhere
                //txtcc= Convert.ToString(dtclientinfo.Rows[0]["DistributionList"]);
                //txtClientEmail = Convert.ToString(dtclientinfo.Rows[0]["EmailID"]);

                txtClientEmail = Convert.ToString(dtclientinfo.Rows[0]["DistributionList"]);


            }
            DataTable dtemailinfo = databaseCon.GetData("select Subject,Body from [tblEmailContent] where Purpose = 'Hand Over' and clientid='" + databaseCon.Decrypt(id) + "'");
            if (dtemailinfo.Rows.Count > 0)
            {
                txtsubject = Convert.ToString(dtemailinfo.Rows[0]["Subject"]);
                msg = Convert.ToString(dtemailinfo.Rows[0]["Body"]);
            }

            // string Emaillink = DomainName + "/"+"HandOverForm?UID="+Request.QueryString["UID"].Trim().Replace(" ", "+")+"&UserRole="+Request.QueryString["UserRole"].Trim().Replace(" ", "+")+"&id="+Request.QueryString["id"].Trim().Replace(" ", "+")+"&RID="+Request.QueryString["RID"].Trim().Replace(" ", "+")+"&InspID="+databaseCon.Encrypt(InspID).Trim().Replace(" ", "+")+"&HandOverId="+databaseCon.Encrypt(HandOverID).Trim().Replace(" ", "+")+"&FormPage="+databaseCon.Encrypt("Email").Trim().Replace(" ", "+");
            // string Emaillink = DomainName + "/" + "HandOverForm?UID=" + Request.QueryString["UID"].Trim().Replace(" ", "+") + "&UserRole=" + Request.QueryString["UserRole"].Trim().Replace(" ", "+") + "&id=" + Request.QueryString["id"].Trim().Replace(" ", "+") + "&RID=" + Request.QueryString["RID"].Trim().Replace(" ", "+") + "&HandOverId=" + databaseCon.Encrypt(HandOverID).Trim().Replace(" ", "+") + "&FormPage=" + databaseCon.Encrypt("Email").Trim().Replace(" ", "+");

            string Emaillink = DomainName + "/" + "HandOverForm/HandOverForm?UID=" + UID.Trim().Replace(" ", "+") + "&UserRole=" + UserRole.Trim().Replace(" ", "+") + "&id=" + id.Trim().Replace(" ", "+") + "&RID=" + RID.Trim().Replace(" ", "+") + "&HandOverId=" + databaseCon.Encrypt(HandOverID).Trim().Replace(" ", "+") + "&FormPage=" + databaseCon.Encrypt("Email").Trim().Replace(" ", "+");

            txtEmailMSg = msg + Environment.NewLine + Emaillink;
            // emailConfirm.Style.Add("display", "block");

            Status = "Success";
            Message = "";

            return Json(new { Result = Status, Message = Message, txtFile = txtFile, txttempfile = txttempfile, txtcc = txtcc, txtClientEmail = txtClientEmail, txtsubject = txtsubject, txtEmailMSg = txtEmailMSg }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult btnSend_Click(string txtClientEmail, string txtcc, string txtsubject, string txtEmailMSg, string txtFile, string HandOverID, string txttempfile)
        {

            var Result = "";
            var Message = "";
            // emailConfirm.Style.Add("display", "none");
            if (databaseCon.SendEmail(txtClientEmail, txtcc, txtsubject, txtEmailMSg, txtFile))
            {
                databaseCon.deletefileonblob(txttempfile, "registerdocument");
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

        public int btnCancel_Click(string txttempfile, string txtFile)
        {
            databaseCon.deletefileonblob(txttempfile, "registerdocument");
            if (System.IO.File.Exists(txtFile))
            {
                try
                {
                    System.IO.File.Delete(txtFile);
                }
                catch
                {

                }
            }
            return 1;
        }


        public ActionResult btnadddocs_Click(string id, string UID, string UserRole, string RID, string hdnTimeOffSet, string DomainName)
        {
            var Status = "";
            var Message = "";
            String query;

            // New Query Add on 31 August 2019 For Multiple Handover create one tome

            query = "select isnull(Min(InspectionID),0) as InspId from tblInspection where InspStatus='Confirmed' and RegisterID='" + databaseCon.Decrypt(RID) + "'";
            DataTable dt = databaseCon.GetData(query);
            if (Convert.ToInt32(dt.Rows[0]["InspId"]) == 0)
            {
                Status = "Failed";
                Message = "An inspection has to be confirmed before creating hand over certificate .";
                //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                //art1.Style.Add("display", "block");
                //alertlabel.Text = "<strong>Alert ! </strong> An inspection has to be confirmed before creating hand over certificate ";
            }
            else
            {
                //  string query1 = "Insert into tblHandOver (InspId,RegisterId,CreatedBy,CreatedOn,HandOverDate,CertificateNo)values ('" + Convert.ToInt32(dt.Rows[0]["InspId"]) + "','" + databaseCon.Decrypt(Request.QueryString["RID"]) + "',(select EmailID from tblUser where UserId='" + databaseCon.Decrypt(Request.QueryString["UID"]) + "'),'" + DateTime.Now + "','" + DateTime.Now + "',(select isnull(Max(CertificateNo),0)+1 as CertificateNo from tblHandOver join tblRegister on tblHandOver.RegisterId=tblRegister.registerid where tblregister.clientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "'))";
                // Query Without Inspection ID

                //string query1 = "Insert into tblHandOver (RegisterId,CreatedBy,CreatedOn,HandOverDate,CertificateNo)values ('" + databaseCon.Decrypt(Request.QueryString["RID"]) + "',(select EmailID from tblUser where UserId='" + databaseCon.Decrypt(Request.QueryString["UID"]) + "'),'" + DateTime.Now + "','" + DateTime.Now + "',(select isnull(Max(CertificateNo),0)+1 as CertificateNo from tblHandOver join tblRegister on tblHandOver.RegisterId=tblRegister.registerid where tblregister.clientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "'))";

                // Query Change On 27 Jan 2020 Add HandoverTimeZone and Save Handover To Utc 
                string query1 = "Insert into tblHandOver (RegisterId,CreatedBy,CreatedOn,HandOverDate,HandoverTimeZone,CertificateNo)values ('" + databaseCon.Decrypt(RID) + "',(select EmailID from tblUser where UserId='" + databaseCon.Decrypt(UID) + "'),'" + DateTime.Now + "','" + DateTime.Now + "'," + Convert.ToDouble(hdnTimeOffSet) * (-1) + ",(select isnull(Max(CertificateNo),0)+1 as CertificateNo from tblHandOver join tblRegister on tblHandOver.RegisterId=tblRegister.registerid where tblregister.clientId='" + databaseCon.Decrypt(id) + "'))";
                //get inserted HandOverId
                int dtInsp = databaseCon.RunCmdID(query1);
                var HandOverID = dtInsp.ToString();
                // ViewState["HandOverId"] = dtInsp;
                // Response.Redirect(DomainName + "/" + "HandOverForm?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RID=" + Request.QueryString["RID"] + "&HandOverId=" + databaseCon.Encrypt(ViewState["HandOverId"].ToString()) + "&InspId=" + databaseCon.Encrypt(dt.Rows[0]["InspId"].ToString()) + "&FormPage=" + "");
                // Response.Redirect(DomainName + "/" + "HandOverForm?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RID=" + Request.QueryString["RID"] + "&HandOverId=" + databaseCon.Encrypt(ViewState["HandOverId"].ToString()) + "&FormPage=" + "");

                // string Path = DomainName + "/" + "HandOverForm/HandOverForm?UID=" + UID.Trim().Replace(" ", "+") + "&UserRole=" + UserRole.Trim().Replace(" ", "+") + "&id=" + id.Trim().Replace(" ", "+") + "&RID=" + RID.Trim().Replace(" ", "+") + "&HandOverId=" + databaseCon.Encrypt(HandOverID).Trim().Replace(" ", "+");
                string Path = DomainName + "/" + "HandOverForm/HandOverForm?UID=" + UID + "&UserRole=" + UserRole + "&id=" + id + "&RID=" + RID + "&HandOverId=" + databaseCon.Encrypt(HandOverID);

                Status = "Success";
                Message = Path;

                // Adddocs.Style.Add("display", "block");
                //  art1.Style.Add("display", "none");
            }

            // Comment On 31 July 2019 one Handover Can create one time

            //query = "select isnull(Min(InspectionID),0) as InspId from tblInspection where InspStatus='Confirmed' and RegisterID='" + databaseCon.Decrypt(Request.QueryString["RID"]) + "'";
            //DataTable dt = databaseCon.GetData(query);
            //if (Convert.ToInt32(dt.Rows[0]["InspId"]) == 0)
            //{
            //    art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
            //    art1.Style.Add("display", "block");
            //    alertlabel.Text = "<strong>Alert ! </strong> An inspection has to be confirmed before creating hand over certificate ";
            //}
            //else
            //{
            //    DataTable Seldt;

            //    query = "select Isnull(HandOverID,0) as HandOverID from tblHandover Where InspID='" + Convert.ToInt32(dt.Rows[0]["InspId"]) + "' and RegisterID='" + databaseCon.Decrypt(Request.QueryString["RID"]) + "'";
            //    DataTable HandOverdt = databaseCon.GetData(query);
            //    //if (Convert.ToInt32(HandOverdt.Rows[0]["HandOverID"]) == 0)
            //    if (HandOverdt.Rows.Count == 0)
            //    {
            //        //UID=dt.Columns[0]["EmailID"];
            //        string query1 = "Insert into tblHandOver (InspId,RegisterId,CreatedBy,CreatedOn,HandOverDate,CertificateNo)values ('" + Convert.ToInt32(dt.Rows[0]["InspId"]) + "','" + databaseCon.Decrypt(Request.QueryString["RID"]) + "',(select EmailID from tblUser where UserId='" + databaseCon.Decrypt(Request.QueryString["UID"]) + "'),'" + DateTime.Now + "','" + DateTime.Now + "',(select isnull(Max(CertificateNo),0)+1 as CertificateNo from tblHandOver join tblRegister on tblHandOver.RegisterId=tblRegister.registerid where tblregister.clientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "'))";
            //        //get inserted HandOverId
            //        int dtInsp = databaseCon.RunCmd(query1);
            //        String SelectQuery = "select HandOverID from tblHandover Where InspID='" + Convert.ToInt32(dt.Rows[0]["InspId"]) + "' and RegisterID='" + databaseCon.Decrypt(Request.QueryString["RID"]) + "'";
            //        Seldt = databaseCon.GetData(SelectQuery);
            //        ViewState["HandOverId"] = Convert.ToInt32(Seldt.Rows[0]["HandOverID"]);
            //        Response.Redirect(DomainName + "/" + "HandOverForm?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RID=" + Request.QueryString["RID"] + "&HandOverId=" + databaseCon.Encrypt(ViewState["HandOverId"].ToString()) + "&InspId=" + databaseCon.Encrypt(dt.Rows[0]["InspId"].ToString()) + "&FormPage=" + "");
            //        Adddocs.Style.Add("display", "block");
            //        art1.Style.Add("display", "none");
            //    }
            //    else if (HandOverdt.Rows.Count >= 1)
            //    {
            //        art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
            //        art1.Style.Add("display", "block");
            //        alertlabel.Text = "<strong>Alert ! </strong> There can only be one hand over certificate . You can view the hand over certificate. ";
            //    }
            //}

            return Json(new { Result = Status, Message = Message }, JsonRequestBehavior.AllowGet);
        }


        // void showreport()
        public string showreport(string id, string RID, string HandOverID)
        {
            var txttempfile = "";
            var txtFile = "";
            string queryClientLogo = "select Clientimage,Address from tblclient where clientid = '" + databaseCon.Decrypt(id) + "'";

            //String handOverQuery = "Select REPLACE(CONVERT(VARCHAR(11),getdate(),106), ' ','-') as CurrentDate,REPLACE(CONVERT(VARCHAR(11),HandOverDate,106), ' ','-') as HandOverDate,  VW_tblRegister.OwnerName,SiteLocation,BuildingNo,FloorLevel ,Right('000000' + Convert(varchar, ScaffRef), 6)  as ScaffoldTag  ,VW_tblRegister.Height,HandOverDesc,IsConfirmed , HandOverImage ,  DutyRating , NoOfWorkingDecks , '' as HeightOFTopWorkingPlateform , HasEncapsulated ,HandOverLat, HandOverLong,HandOverAddress , HandOverAudio  ,HandOverVedio , HandOverComments , HandOverAcknowledgment,(select Right('000000'+Convert(varchar,CertificateNo),6)as CertificateNo from tblHandOver where RegisterId = '" + databaseCon.Decrypt(Request.QueryString["RID"]) + "') as CertificateNo,FirstName + ' ' + LastName as ContractorRepresentative ,tblUser.LicenceNo,tblUser.[Role] as ContractorPosition  , ContractorSign,tblHandOver.CRId ,Position, RepresentativeName, RepresentativeSign,HandoverMapImage from VW_tblRegister join tblInspection on VW_tblRegister.RegisterId = tblInspection.RegisterID left join tblUser on tblInspection.InspectedById = tblUser.UserId join tblClient on VW_TblRegister.ClientID = tblClient.ClientId left join tblHandOver on tblHandOver.InspId = tblInspection.InspectionID and tblHandOver.RegisterID = VW_TblRegister.RegisterId left join tblClientRepresentation on tblClientRepresentation.CRId=tblHandOver.CRId  where  VW_tblRegister.RegisterID = '" + databaseCon.Decrypt(Request.QueryString["RID"]) + "' and InspId='" + insppdfid + "' and HandOverId='" + Handoverpdfid + "'";

            // Query For Old Handover ConfirmID=1 and ConfirmedDate not equal to null

            String handOverQuery = "";

            string query = "select Count(*) as HandOverIdCount from tblHandOver where IsConfirmed=1  and HandOverId='" + HandOverID + "' and ConfirmedDate is not null";
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows[0]["HandOverIdCount"].ToString() == "0")
            {
                //  handOverQuery = "Select REPLACE(CONVERT(VARCHAR(11),getdate(),106), ' ','-') as CurrentDate,REPLACE(CONVERT(VARCHAR(11),HandOverDate,106), ' ','-') as HandOverDate,HandOverDate as HandOverDateTime,  VW_tblRegister.OwnerName,SiteLocation,BuildingNo,FloorLevel ,Right('000000' + Convert(varchar, ScaffRef), 6)  as ScaffoldTag  ,VW_tblRegister.Height,VW_tblRegister.Width,VW_tblRegister.Length,VW_tblRegister.DesignDrawingNo,VW_tblRegister.Description as ReasonForAccess,VW_TblRegister.NumberOfPlatform,VW_TblRegister.EngineeredDesign,HandOverDesc,IsConfirmed , HandOverImage ,  DutyRating , NoOfWorkingDecks , '' as HeightOFTopWorkingPlateform , HasEncapsulated ,HandOverLat, HandOverLong,HandOverAddress , HandOverAudio  ,HandOverVedio , HandOverComments , HandOverAcknowledgment,(select Right('000000'+Convert(varchar,CertificateNo),6)as CertificateNo from tblHandOver where RegisterId = '" + databaseCon.Decrypt(Request.QueryString["RID"]) + "' and  HandOverId='" + Handoverpdfid + "') as CertificateNo,FirstName + ' ' + LastName as ContractorRepresentative ,tblUser.LicenceNo,tblUser.[Role] as ContractorPosition  , ContractorSign,tblHandOver.CRId ,Position, RepresentativeName, RepresentativeSign,HandoverMapImage from VW_tblRegister join tblInspection on VW_tblRegister.RegisterId = tblInspection.RegisterID left join tblUser on tblInspection.InspectedById = tblUser.UserId join tblClient on VW_TblRegister.ClientID = tblClient.ClientId left join tblHandOver on tblHandOver.InspId = tblInspection.InspectionID and tblHandOver.RegisterID = VW_TblRegister.RegisterId left join tblClientRepresentation on tblClientRepresentation.CRId=tblHandOver.CRId  where  VW_tblRegister.RegisterID = '" + databaseCon.Decrypt(Request.QueryString["RID"]) + "' and InspId='" + insppdfid + "' and HandOverId='" + Handoverpdfid + "'";

                // Query Change on 5 Aug Without Inspection Id
                // handOverQuery = "Select REPLACE(CONVERT(VARCHAR(11),getdate(),106), ' ','-') as CurrentDate,REPLACE(CONVERT(VARCHAR(11),HandOverDate,106), ' ','-') as HandOverDate,HandOverDate as HandOverDateTime,  VW_tblRegister.OwnerName,SiteLocation,BuildingNo,FloorLevel ,Right('000000' + Convert(varchar, ScaffRef), 6)  as ScaffoldTag  ,VW_tblRegister.Height,VW_tblRegister.Width,VW_tblRegister.Length,VW_tblRegister.DesignDrawingNo,VW_tblRegister.Description as ReasonForAccess,VW_TblRegister.NumberOfPlatform,VW_TblRegister.EngineeredDesign,HandOverDesc,IsConfirmed , HandOverImage ,  DutyRating , NoOfWorkingDecks ,  HasEncapsulated ,HandOverLat, HandOverLong,HandOverAddress , HandOverAudio  ,HandOverVedio , HandOverComments , HandOverAcknowledgment,(select Right('000000'+Convert(varchar,CertificateNo),6)as CertificateNo from tblHandOver where RegisterId ='" + databaseCon.Decrypt(Request.QueryString["RID"]) + "'  and  HandOverId='" + Handoverpdfid + "') as CertificateNo, ContractorRepName ,ContractorRepLicence,ContractorRepPosition , ContractorSign,tblHandOver.CRId ,Position, RepresentativeName, RepresentativeSign,HandoverMapImage from VW_tblRegister join tblHandOver on tblHandOver.RegisterID = VW_TblRegister.RegisterId left join tblClientRepresentation on tblClientRepresentation.CRId = tblHandOver.CRId  where VW_tblRegister.RegisterID = '" + databaseCon.Decrypt(Request.QueryString["RID"]) + "'  and HandOverId = '" + Handoverpdfid + "'";

                // Query Change On 27 Jan 2020
                //handOverQuery = "Select REPLACE(CONVERT(VARCHAR(11),getdate(),106), ' ','-') as CurrentDate,Isnull(Convert(int,LEFT(Replace(convert(Time,DateAdd(minute, Isnull(HandoverTimezone,0), HandOverDate)),':',''),4)),0) as HandOverTime,REPLACE(CONVERT(VARCHAR(11), DateAdd(minute, Isnull(HandoverTimezone,0), HandOverDate), 106), ' ', '-') as HandOverDate,DateAdd(minute, Isnull(HandoverTimezone,0), HandOverDate) as HandOverDateTime,VW_tblRegister.OwnerName,SiteLocation,BuildingNo,FloorLevel,Right('000000' + Convert(varchar, ScaffRef), 6) as ScaffoldTag  ,VW_tblRegister.Height,VW_tblRegister.Width,VW_tblRegister.Length,VW_tblRegister.DesignDrawingNo,VW_tblRegister.Description as ReasonForAccess,VW_TblRegister.NumberOfPlatform,VW_TblRegister.EngineeredDesign,HandOverDesc,IsConfirmed , HandOverImage ,tblHandover.PhotoAngle,  DutyRating , NoOfWorkingDecks ,  HasEncapsulated ,HandOverLat, HandOverLong,HandOverAddress , HandOverAudio  ,HandOverVedio , HandOverComments , HandOverAcknowledgment,(select Right('000000' + Convert(varchar, CertificateNo), 6)as CertificateNo from tblHandOver where RegisterId = '" + databaseCon.Decrypt(RID) + "'  and HandOverId = '" + HandOverID + "') as CertificateNo,InspectedBy as ContractorRepName ,ContractorRepLicence,ContractorRepPosition , ContractorSign,tblHandOver.CRId ,Position, RepresentativeName, RepresentativeSign,HandoverMapImage from VW_tblRegister join tblHandOver on tblHandOver.RegisterID = VW_TblRegister.RegisterId left join tblClientRepresentation on tblClientRepresentation.CRId = tblHandOver.CRId where VW_tblRegister.RegisterID ='" + databaseCon.Decrypt(RID) + "'  and HandOverId = '" + HandOverID + "'";
                handOverQuery = "Select REPLACE(CONVERT(VARCHAR(11),getdate(),106), ' ','-') as CurrentDate,Isnull(Convert(int,LEFT(Replace(convert(Time,DateAdd(minute, Isnull(HandoverTimezone,0), HandOverDate)),':',''),4)),0) as HandOverTime,REPLACE(CONVERT(VARCHAR(11), DateAdd(minute, Isnull(HandoverTimezone,0), HandOverDate), 106), ' ', '-') as HandOverDate,DateAdd(minute, Isnull(HandoverTimezone,0), HandOverDate) as HandOverDateTime,VW_tblRegister.OwnerName,SiteLocation,BuildingNo,VW_tblRegister.Location,FloorLevel,Right('000000' + Convert(varchar, ScaffRef), 6) as ScaffoldTag  ,VW_tblRegister.Height,VW_tblRegister.Width,VW_tblRegister.Length,VW_tblRegister.DesignDrawingNo,VW_tblRegister.Description as ReasonForAccess,VW_TblRegister.NumberOfPlatform,VW_TblRegister.EngineeredDesign,HandOverDesc,IsConfirmed , HandOverImage ,tblHandover.PhotoAngle,  DutyRating , NoOfWorkingDecks ,  HasEncapsulated ,HandOverLat, HandOverLong,HandOverAddress , HandOverAudio  ,HandOverVedio , HandOverComments , HandOverAcknowledgment,(select Right('000000' + Convert(varchar, CertificateNo), 6)as CertificateNo from tblHandOver where RegisterId = '" + databaseCon.Decrypt(RID) + "'  and HandOverId = '" + HandOverID + "') as CertificateNo,InspectedBy as ContractorRepName ,ContractorRepLicence,ContractorRepPosition , ContractorSign,tblHandOver.CRId ,Position, RepresentativeName, RepresentativeSign,HandoverMapImage from VW_tblRegister join tblHandOver on tblHandOver.RegisterID = VW_TblRegister.RegisterId left join tblClientRepresentation on tblClientRepresentation.CRId = tblHandOver.CRId where VW_tblRegister.RegisterID ='" + databaseCon.Decrypt(RID) + "'  and HandOverId = '" + HandOverID + "'";
            }
            else
            {
                //handOverQuery = "Select REPLACE(CONVERT(VARCHAR(11),getdate(),106), ' ','-') as CurrentDate,REPLACE(CONVERT(VARCHAR(11),HandOverDate,106), ' ','-') as HandOverDate,HandOverDate as HandOverDateTime,  siteOwnerFromJob as OwnerName,siteLocationFromJob as SiteLocation,BuildingNoFromJob as BuildingNo,FloorLevelFromJob as FloorLevel ,ScaffrefNoFromJob as ScaffoldTag  ,HeightFromJob as Height,WidthFromJob as Width,LengthFromJob as Length,DesignDrawingNoFromJob as DesignDrawingNo,ReasonForAccessFromJob as ReasonForAccess,MaximumDecksFromJob as NumberOfPlatform,EngineeredDesignFromJob as EngineeredDesign,HandOverDesc,IsConfirmed , HandOverImage , DutyRatingFromJob as DutyRating ,   NoOfworkingDecksFromJob as NoOfWorkingDecks ,HasEncapsulated ,HandOverLat, HandOverLong,HandOverAddress , HandOverAudio  ,HandOverVedio , HandOverComments , HandOverAcknowledgment,(select Right('000000'+Convert(varchar,CertificateNo),6)as CertificateNo from tblHandOver where RegisterId = '" + databaseCon.Decrypt(Request.QueryString["RID"]) + "' and HandOverId='" + Handoverpdfid + "' ) as CertificateNo,FirstName + ' ' + LastName as ContractorRepresentative ,tblUser.LicenceNo,tblUser.[Role] as ContractorPosition  , ContractorSign,tblHandOver.CRId ,Position, RepresentativeName, RepresentativeSign,HandoverMapImage from VW_tblRegister join tblInspection on VW_tblRegister.RegisterId = tblInspection.RegisterID left join tblUser on tblInspection.InspectedById = tblUser.UserId join tblClient on VW_TblRegister.ClientID = tblClient.ClientId left join tblHandOver on tblHandOver.InspId = tblInspection.InspectionID and tblHandOver.RegisterID = VW_TblRegister.RegisterId left join tblClientRepresentation on tblClientRepresentation.CRId=tblHandOver.CRId  where  VW_tblRegister.RegisterID = '" + databaseCon.Decrypt(Request.QueryString["RID"]) + "' and InspId='" + insppdfid + "' and HandOverId='" + Handoverpdfid + "'";

                // added on 2-Aug-2019
                //  handOverQuery = "Select REPLACE(CONVERT(VARCHAR(11),getdate(),106), ' ','-') as CurrentDate,REPLACE(CONVERT(VARCHAR(11),HandOverDate,106), ' ','-') as HandOverDate,HandOverDate as HandOverDateTime,  siteOwnerFromJob as OwnerName,siteLocationFromJob as SiteLocation,BuildingNoFromJob as BuildingNo,FloorLevelFromJob as FloorLevel ,ScaffrefNoFromJob as ScaffoldTag  ,HeightFromJob as Height,WidthFromJob as Width,LengthFromJob as Length,DesignDrawingNoFromJob as DesignDrawingNo,ReasonForAccessFromJob as ReasonForAccess,MaximumDecksFromJob as NumberOfPlatform,EngineeredDesignFromJob as EngineeredDesign,HandOverDesc,IsConfirmed , HandOverImage , DutyRatingFromJob as DutyRating ,   NoOfworkingDecksFromJob as NoOfWorkingDecks ,HasEncapsulated ,HandOverLat, HandOverLong,HandOverAddress , HandOverAudio  ,HandOverVedio , HandOverComments , HandOverAcknowledgment,(select Right('000000'+Convert(varchar,CertificateNo),6)as CertificateNo from tblHandOver where RegisterId = '" + databaseCon.Decrypt(Request.QueryString["RID"]) + "' and HandOverId='" + Handoverpdfid + "' ) as CertificateNo, ContractorRepName ,ContractorRepLicence,ContractorRepPosition , ContractorSign,tblHandOver.CRId ,Position, RepresentativeName, RepresentativeSign,HandoverMapImage from VW_tblRegister join tblInspection on VW_tblRegister.RegisterId = tblInspection.RegisterID left join tblUser on tblInspection.InspectedById = tblUser.UserId join tblClient on VW_TblRegister.ClientID = tblClient.ClientId left join tblHandOver on tblHandOver.InspId = tblInspection.InspectionID and tblHandOver.RegisterID = VW_TblRegister.RegisterId left join tblClientRepresentation on tblClientRepresentation.CRId=tblHandOver.CRId  where  VW_tblRegister.RegisterID = '" + databaseCon.Decrypt(Request.QueryString["RID"]) + "' and InspId='" + insppdfid + "' and HandOverId='" + Handoverpdfid + "'";

                // Query Change on 5 Aug Without Inspection Id
                // handOverQuery = "Select REPLACE(CONVERT(VARCHAR(11),getdate(),106), ' ','-') as CurrentDate,REPLACE(CONVERT(VARCHAR(11),HandOverDate,106), ' ','-') as HandOverDate,HandOverDate as HandOverDateTime,  siteOwnerFromJob as OwnerName,siteLocationFromJob as SiteLocation,BuildingNoFromJob as BuildingNo,FloorLevelFromJob as FloorLevel ,ScaffrefNoFromJob as ScaffoldTag  ,case when LengthFromJob!='' then CONVERT(DECIMAL(6, 2),HeightFromJob) else NULL End AS Height,case when LengthFromJob!='' then CONVERT(DECIMAL(6, 2), WidthFromJob) else NULL End as Width,case when LengthFromJob!= '' then CONVERT(DECIMAL(6, 2), LengthFromJob) else NULL End as Length,DesignDrawingNoFromJob as DesignDrawingNo,ReasonForAccessFromJob as ReasonForAccess,MaximumDecksFromJob as NumberOfPlatform,EngineeredDesignFromJob as EngineeredDesign,HandOverDesc,IsConfirmed , HandOverImage , DutyRatingFromJob as DutyRating ,   NoOfworkingDecksFromJob as NoOfWorkingDecks ,HasEncapsulated ,HandOverLat, HandOverLong,HandOverAddress , HandOverAudio  ,HandOverVedio , HandOverComments , HandOverAcknowledgment,(select Right('000000'+Convert(varchar,CertificateNo),6)as CertificateNo from tblHandOver where RegisterId = '" + databaseCon.Decrypt(Request.QueryString["RID"]) + "' and HandOverId='" + Handoverpdfid + "' ) as CertificateNo, ContractorRepName ,ContractorRepLicence,ContractorRepPosition , ContractorSign,tblHandOver.CRId ,Position, RepresentativeName, RepresentativeSign,HandoverMapImage from VW_tblRegister   join tblClient on VW_TblRegister.ClientID = tblClient.ClientId left join tblHandOver on  tblHandOver.RegisterID = VW_TblRegister.RegisterId left join tblClientRepresentation on tblClientRepresentation.CRId=tblHandOver.CRId  where  VW_tblRegister.RegisterID = '" + databaseCon.Decrypt(Request.QueryString["RID"]) + "' and HandOverId='" + Handoverpdfid + "'";

                // Query Change On 27 Jan 2020
                //    handOverQuery = "Select REPLACE(CONVERT(VARCHAR(11),getdate(),106), ' ','-') as CurrentDate,Isnull(Convert(int,LEFT(Replace(convert(Time,DateAdd(minute, Isnull(HandoverTimezone,0), HandOverDate)),':',''),4)),0) as HandOverTime,REPLACE(CONVERT(VARCHAR(11), DateAdd(minute, Isnull(HandoverTimezone,0), HandOverDate), 106), ' ', '-') as HandOverDate,DateAdd(minute, Isnull(HandoverTimezone,0), HandOverDate) as HandOverDateTime,siteOwnerFromJob as OwnerName,siteLocationFromJob as SiteLocation,BuildingNoFromJob as BuildingNo,FloorLevelFromJob as FloorLevel ,ScaffrefNoFromJob as ScaffoldTag  ,case when HeightFromJob!= '' then CONVERT(DECIMAL(6, 2), HeightFromJob) else NULL End AS Height,case when WidthFromJob!= '' then CONVERT(DECIMAL(6, 2), WidthFromJob) else NULL End as Width,case when LengthFromJob!= '' then CONVERT(DECIMAL(6, 2), LengthFromJob) else NULL End as Length,DesignDrawingNoFromJob as DesignDrawingNo,ReasonForAccessFromJob as ReasonForAccess,MaximumDecksFromJob as NumberOfPlatform,EngineeredDesignFromJob as EngineeredDesign,HandOverDesc,IsConfirmed , HandOverImage, tblHandover.PhotoAngle, DutyRatingFromJob as DutyRating ,   NoOfworkingDecksFromJob as NoOfWorkingDecks ,HasEncapsulated ,HandOverLat, HandOverLong,HandOverAddress , HandOverAudio  ,HandOverVedio , HandOverComments , HandOverAcknowledgment,(select Right('000000' + Convert(varchar, CertificateNo), 6) as CertificateNo from tblHandOver where RegisterId = '" + databaseCon.Decrypt(RID) + "' and HandOverId =  '" + HandOverID + "' ) as CertificateNo, ContractorRepName ,ContractorRepLicence,ContractorRepPosition , ContractorSign,tblHandOver.CRId ,Position, RepresentativeName, RepresentativeSign,HandoverMapImage from VW_tblRegister join tblClient on VW_TblRegister.ClientID = tblClient.ClientId left join tblHandOver on  tblHandOver.RegisterID = VW_TblRegister.RegisterId left join tblClientRepresentation on tblClientRepresentation.CRId = tblHandOver.CRId where VW_tblRegister.RegisterID ='" + databaseCon.Decrypt(RID) + "' and HandOverId = '" + HandOverID + "'";

                //}
                //query change on
                //handOverQuery = "Select REPLACE(CONVERT(VARCHAR(11),getdate(),106), ' ','-') as CurrentDate,Isnull(Convert(int,LEFT(Replace(convert(Time,DateAdd(minute, Isnull(HandoverTimezone,0), HandOverDate)),':',''),4)),0) as HandOverTime,REPLACE(CONVERT(VARCHAR(11), DateAdd(minute, Isnull(HandoverTimezone,0), HandOverDate), 106), ' ', '-') as HandOverDate,DateAdd(minute, Isnull(HandoverTimezone,0), HandOverDate) as HandOverDateTime,siteOwnerFromJob as OwnerName,siteLocationFromJob as SiteLocation,BuildingNoFromJob as BuildingNo,FloorLevelFromJob as FloorLevel ,ScaffrefNoFromJob as ScaffoldTag  ,case when HeightFromJob!= '' then CONVERT(DECIMAL(6, 2), HeightFromJob) else NULL End AS Height,case when WidthFromJob!= '' then CONVERT(DECIMAL(6, 2), WidthFromJob) else NULL End as Width,case when LengthFromJob!= '' then CONVERT(DECIMAL(6, 2), LengthFromJob) else NULL End as Length,DesignDrawingNoFromJob as DesignDrawingNo,ReasonForAccessFromJob as ReasonForAccess,MaximumDecksFromJob as NumberOfPlatform,EngineeredDesignFromJob as EngineeredDesign,HandOverDesc,IsConfirmed , HandOverImage, tblHandover.PhotoAngle, DutyRatingFromJob as DutyRating ,   NoOfworkingDecksFromJob as NoOfWorkingDecks ,HasEncapsulated ,HandOverLat, HandOverLong,HandOverAddress , HandOverAudio  ,HandOverVedio , HandOverComments , HandOverAcknowledgment,(select Right('000000' + Convert(varchar, CertificateNo), 6) as CertificateNo from tblHandOver where RegisterId = '" + databaseCon.Decrypt(RID) + "' and HandOverId =  '" + HandOverID + "' ) as CertificateNo, InspectedBy as ContractorRepName ,ContractorRepLicence,ContractorRepPosition , ContractorSign,tblHandOver.CRId ,Position, RepresentativeName, RepresentativeSign,HandoverMapImage from VW_tblRegister join tblClient on VW_TblRegister.ClientID = tblClient.ClientId left join tblHandOver on  tblHandOver.RegisterID = VW_TblRegister.RegisterId left join tblClientRepresentation on tblClientRepresentation.CRId = tblHandOver.CRId where VW_tblRegister.RegisterID ='" + databaseCon.Decrypt(RID) + "' and HandOverId = '" + HandOverID + "'"; // Commented on 17-May-2022
                //handOverQuery = "Select REPLACE(CONVERT(VARCHAR(11),getdate(),106), ' ','-') as CurrentDate,Isnull(Convert(int,LEFT(Replace(convert(Time,DateAdd(minute, Isnull(HandoverTimezone,0), HandOverDate)),':',''),4)),0) as HandOverTime,REPLACE(CONVERT(VARCHAR(11), DateAdd(minute, Isnull(HandoverTimezone,0), HandOverDate), 106), ' ', '-') as HandOverDate,DateAdd(minute, Isnull(HandoverTimezone,0), HandOverDate) as HandOverDateTime,siteOwnerFromJob as OwnerName,siteLocationFromJob as SiteLocation,BuildingNoFromJob as BuildingNo,FloorLevelFromJob as FloorLevel ,ScaffrefNoFromJob as ScaffoldTag  ,case when HeightFromJob!= '' then CONVERT(DECIMAL(6, 2), HeightFromJob) else NULL End AS Height,case when WidthFromJob!= '' then CONVERT(DECIMAL(6, 2), WidthFromJob) else NULL End as Width,case when LengthFromJob!= '' then CONVERT(DECIMAL(6, 2), LengthFromJob) else NULL End as Length,DesignDrawingNoFromJob as DesignDrawingNo,ReasonForAccessFromJob as ReasonForAccess,MaximumDecksFromJob as NumberOfPlatform,EngineeredDesignFromJob as EngineeredDesign,HandOverDesc,IsConfirmed , HandOverImage, tblHandover.PhotoAngle, DutyRatingFromJob as DutyRating ,   NoOfworkingDecksFromJob as NoOfWorkingDecks ,HasEncapsulated ,HandOverLat, HandOverLong,HandOverAddress , HandOverAudio  ,HandOverVedio , HandOverComments , HandOverAcknowledgment,(select Right('000000' + Convert(varchar, CertificateNo), 6) as CertificateNo from tblHandOver where RegisterId = '" + databaseCon.Decrypt(RID) + "' and HandOverId =  '" + HandOverID + "' ) as CertificateNo, (select firstName+' '+LastName  from tblUser where IsInspector = 'true' and Status='Active' and UserId = tblHandOver.ContractorRepName ) as ContractorRepName ,ContractorRepLicence,ContractorRepPosition , ContractorSign,tblHandOver.CRId ,Position, RepresentativeName, RepresentativeSign,HandoverMapImage from VW_tblRegister join tblClient on VW_TblRegister.ClientID = tblClient.ClientId left join tblHandOver on  tblHandOver.RegisterID = VW_TblRegister.RegisterId left join tblClientRepresentation on tblClientRepresentation.CRId = tblHandOver.CRId where VW_tblRegister.RegisterID ='" + databaseCon.Decrypt(RID) + "' and HandOverId = '" + HandOverID + "'";  //Added on 17-May-2022

                //Commented on 06-06-2023 The 'Yes' for the Engineered drawing is not carrying over to the Hand Over cert
                /*handOverQuery = "Select REPLACE(CONVERT(VARCHAR(11),getdate(),106), ' ','-') as CurrentDate,Isnull(Convert(int,LEFT(Replace(convert(Time,DateAdd(minute, Isnull(HandoverTimezone,0), HandOverDate)),':',''),4)),0) as HandOverTime,REPLACE(CONVERT(VARCHAR(11), DateAdd(minute, Isnull(HandoverTimezone,0), HandOverDate), 106), ' ', '-') as HandOverDate,DateAdd(minute, Isnull(HandoverTimezone,0), HandOverDate) as HandOverDateTime,siteOwnerFromJob as OwnerName,siteLocationFromJob as SiteLocation,BuildingNoFromJob as BuildingNo,VW_tblRegister.Location as Location,FloorLevelFromJob as FloorLevel ,ScaffrefNoFromJob as ScaffoldTag  ,case when HeightFromJob!= '' then CONVERT(DECIMAL(6, 2), HeightFromJob) else NULL End AS Height,case when WidthFromJob!= '' then CONVERT(DECIMAL(6, 2), WidthFromJob) else NULL End as Width,case when LengthFromJob!= '' then CONVERT(DECIMAL(6, 2), LengthFromJob) else NULL End as Length,DesignDrawingNoFromJob as DesignDrawingNo,ReasonForAccessFromJob as ReasonForAccess,MaximumDecksFromJob as NumberOfPlatform,EngineeredDesignFromJob as EngineeredDesign,HandOverDesc,IsConfirmed , HandOverImage, tblHandover.PhotoAngle, DutyRatingFromJob as DutyRating ,   NoOfworkingDecksFromJob as NoOfWorkingDecks ,HasEncapsulated ,HandOverLat, HandOverLong,HandOverAddress , HandOverAudio  ,HandOverVedio , HandOverComments , HandOverAcknowledgment,(select Right('000000' + Convert(varchar, CertificateNo), 6) as CertificateNo from tblHandOver where RegisterId = '" + databaseCon.Decrypt(RID) + "' and HandOverId =  '" + HandOverID + "' ) as CertificateNo, (select firstName+' '+LastName  from tblUser where IsInspector = 'true' and Status='Active' and UserId = tblHandOver.ContractorRepName ) as ContractorRepName ,ContractorRepLicence,ContractorRepPosition , ContractorSign,tblHandOver.CRId ,Position, RepresentativeName, RepresentativeSign,HandoverMapImage from VW_tblRegister join tblClient on VW_TblRegister.ClientID = tblClient.ClientId left join tblHandOver on  tblHandOver.RegisterID = VW_TblRegister.RegisterId left join tblClientRepresentation on tblClientRepresentation.CRId = tblHandOver.CRId where VW_tblRegister.RegisterID ='" + databaseCon.Decrypt(RID) + "' and HandOverId = '" + HandOverID + "'";*/  //Added on 11-JAN-2022

                //Added on 06-06-2023 To Get 'Yes' for the Engineered drawing that was not carrying over to the Hand Over cert because of we are saving the data in the column of tblregister with name EngineeredDesign but we are trying to pick the data from column of tblhandover with name EngineeredDesignFromJob in which we are not saving value as the Checkboxes are disabled on handover form because we are only taking the values from tblregister and we are not allowing user to update it on Handover Form.
                handOverQuery = "Select REPLACE(CONVERT(VARCHAR(11),getdate(),106), ' ','-') as CurrentDate,Isnull(Convert(int,LEFT(Replace(convert(Time,DateAdd(minute, Isnull(HandoverTimezone,0), HandOverDate)),':',''),4)),0) as HandOverTime,REPLACE(CONVERT(VARCHAR(11), DateAdd(minute, Isnull(HandoverTimezone,0), HandOverDate), 106), ' ', '-') as HandOverDate,DateAdd(minute, Isnull(HandoverTimezone,0), HandOverDate) as HandOverDateTime,siteOwnerFromJob as OwnerName,siteLocationFromJob as SiteLocation,BuildingNoFromJob as BuildingNo,VW_tblRegister.Location as Location,FloorLevelFromJob as FloorLevel ,ScaffrefNoFromJob as ScaffoldTag  ,case when HeightFromJob!= '' then CONVERT(DECIMAL(6, 2), HeightFromJob) else NULL End AS Height,case when WidthFromJob!= '' then CONVERT(DECIMAL(6, 2), WidthFromJob) else NULL End as Width,case when LengthFromJob!= '' then CONVERT(DECIMAL(6, 2), LengthFromJob) else NULL End as Length,DesignDrawingNoFromJob as DesignDrawingNo,ReasonForAccessFromJob as ReasonForAccess,MaximumDecksFromJob as NumberOfPlatform,EngineeredDesign,HandOverDesc,IsConfirmed , HandOverImage, tblHandover.PhotoAngle, DutyRatingFromJob as DutyRating ,   NoOfworkingDecksFromJob as NoOfWorkingDecks ,HasEncapsulated ,HandOverLat, HandOverLong,HandOverAddress , HandOverAudio  ,HandOverVedio , HandOverComments , HandOverAcknowledgment,(select Right('000000' + Convert(varchar, CertificateNo), 6) as CertificateNo from tblHandOver where RegisterId = '" + databaseCon.Decrypt(RID) + "' and HandOverId =  '" + HandOverID + "' ) as CertificateNo, (select firstName+' '+LastName  from tblUser where IsInspector = 'true' and Status='Active' and UserId = tblHandOver.ContractorRepName ) as ContractorRepName ,ContractorRepLicence,ContractorRepPosition , ContractorSign,tblHandOver.CRId ,Position, RepresentativeName, RepresentativeSign,HandoverMapImage from VW_tblRegister join tblClient on VW_TblRegister.ClientID = tblClient.ClientId left join tblHandOver on  tblHandOver.RegisterID = VW_TblRegister.RegisterId left join tblClientRepresentation on tblClientRepresentation.CRId = tblHandOver.CRId where VW_tblRegister.RegisterID ='" + databaseCon.Decrypt(RID) + "' and HandOverId = '" + HandOverID + "'";  //Added on 11-JAN-2022   
            }
            DataTable clientLogodt = databaseCon.GetData(queryClientLogo);
            DataTable handoverdt = databaseCon.GetData(handOverQuery);
            string path = Server.MapPath("~/Report/HandOver_" + handoverdt.Rows[0]["HandOverDate"] + "_" + HandOverID + ".pdf");
            // string path = Server.MapPath("~/Report/HandOver_" + handoverdt.Rows[0]["HandOverDate"] + "_" + HandOverID + ".pdf");  // Line Comment On 12-Sep-2020 
            //string path = Server.MapPath("~/Report/");

            //txttempfile.Text = "HandOver_" + handoverdt.Rows[0]["HandOverDate"] + "_" + HandOverID + ".pdf";
            //txtFile.Text = path;

            txttempfile = "HandOver_" + handoverdt.Rows[0]["HandOverDate"] + "_" + HandOverID + ".pdf";
            txtFile = path;
            iTextSharp.text.Document doc = new iTextSharp.text.Document(PageSize.A4, 0, 0, 10f, 0);
            /*iTextSharp.text.Document doc = new iTextSharp.text.Document(PageSize.A4, -50f, -50f, 20f, 20f);  */ //Page Size set top ,left,margin
                                                                                                                  //doc.SetPageSize(iTextSharp.text.PageSize.A4);

            // PdfWriter writer = PdfWriter.GetInstance(doc, Response.OutputStream);
            Response.ContentType = ("application/pdf");
            var contentfontCheck = FontFactory.GetFont((Server.MapPath("~/Content/fonts/wingding.ttf")), BaseFont.CP1252, true, 14);

            var contentfontUncheck = FontFactory.GetFont((Server.MapPath("~/Content/fonts/wingding.ttf")), BaseFont.CP1252, true, 14);
            PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));
            doc.Open();
            var Heading787 = FontFactory.GetFont("Arial", 20f, iTextSharp.text.Font.BOLD, BaseColor.BLUE);
            var Heading786 = FontFactory.GetFont("Arial", 10f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            var Heading788 = FontFactory.GetFont("Arial", 12f, iTextSharp.text.Font.BOLD, BaseColor.BLUE);
            var DataFont = FontFactory.GetFont("Arial", 10f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            Phrase phrase;
            PdfPCell cell;

            PdfPTable headertable1 = new PdfPTable(5);
            headertable1.SetWidths(new int[] { 30, 9, 10, 9, 9 });


            PdfPTable headertable2 = new PdfPTable(6);
            headertable2.SetWidths(new int[] { 10, 9, 25, 9, 9, 9 });

            PdfPTable headertable3 = new PdfPTable(5);
            headertable3.SetWidths(new int[] { 30, 9, 10, 9, 9 });


            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 2;
            cell.Border = 0;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 2;
            cell.Border = 0;
            headertable2.AddCell(cell);

            if (!string.IsNullOrEmpty(Convert.ToString(handoverdt.Rows[0]["IsConfirmed"]).ToString()))
            {
                string Unchecked = System.Web.HttpContext.Current.Server.MapPath("~/images/lock.png");
                cell = new PdfPCell(iTextSharp.text.Image.GetInstance(Unchecked), true);
                cell.FixedHeight = 20f;
                cell.PaddingTop = -10f;

                cell.Border = 0;
                cell.Colspan = 2;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                headertable2.AddCell(cell);
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 2;
                cell.FixedHeight = 20f;
                cell.PaddingTop = -10f;

                //cell.PaddingLeft = 100f;
                //cell.PaddingTop = 30f;
                cell.Border = 0;
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);
            }

            //Table Start








            if (!string.IsNullOrEmpty(Convert.ToString(clientLogodt.Rows[0]["ClientImage"])))
            {
                // var uri = new Uri(databaseCon.clientimageLink);
                // var container = new CloudBlobContainer(uri);
                // var blob = container.GetBlockBlobReference(clientLogodt.Rows[0]["ClientImage"].ToString());
                var noimage = Server.MapPath("~/Images/Logo2.png");
                // var exists = blob.Exists();
                if (clientLogodt.Rows[0]["ClientImage"].ToString() == "")
                {
                    string nophoto = Server.MapPath("~/images/Logo2.png");
                    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.FixedHeight = 50f;
                    cell.PaddingTop = -10f;
                    cell.Colspan = 2;
                    //cell.PaddingLeft = 10f;
                    //cell.PaddingTop = -35;
                }
                else
                {
                    try
                    {
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images/clientlogo/" + clientLogodt.Rows[0]["ClientImage"].ToString()));

                        img.ScaleAbsolute(180f, 133f);
                        cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img), true);
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.FixedHeight = 50f;
                        cell.PaddingTop = -10f;
                        //cell.FixedHeight = 200f;
                        cell.Colspan = 2;
                        //cell.PaddingLeft = 10f;
                        //cell.PaddingTop = -35f;
                    }
                    catch (Exception ex)
                    {
                        string nophoto = Server.MapPath("~/images/noimg.jpg");
                        cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.FixedHeight = 50f;
                        cell.PaddingTop = -10f;
                        cell.Colspan = 2;
                        //cell.PaddingLeft = 10f;
                        //cell.PaddingTop = -35;
                    }
                }
            }
            else
            {
                string nophoto = Server.MapPath("~/images/Logo2.png");
                cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.FixedHeight = 50f;
                cell.PaddingTop = -10f;
                cell.Colspan = 2;
                //cell.PaddingTop = 20f;
            }
            cell.Border = 0;
            cell.Colspan = 2;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.FixedHeight = 50f;
            cell.PaddingTop = -10f;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Scaffold Hand Over", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 2;
            //cell.PaddingBottom = 6f;
            //cell.PaddingLeft = 100f;
            cell.PaddingTop = 10f;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            headertable2.AddCell(cell);


            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading786));
            // Comment On 27 Jan 2020
            // phrase.Add(new Chunk("Date: ", Heading786));
            // phrase.Add(new Chunk("" + DateTime.Now.ToString("dd-MMM-yyy") + "", DataFont));
            //phrase.Add(new Chunk(" " + DateTime.Now.ToString("hh:mm:ss") + "", DataFont));
            cell = new PdfPCell(phrase);
            cell.Colspan = 2;
            //cell.PaddingBottom = 6f;
            //cell.PaddingLeft = 100f;
            cell.PaddingTop = 10f;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable2.AddCell(cell);




            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            //cell.PaddingBottom = 6f;
            //cell.PaddingLeft = 100f;
            //cell.PaddingTop = 30f;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);



            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            //cell.PaddingBottom = 6f;
            //cell.PaddingLeft = 100f;
            //cell.PaddingTop = 30f;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            // for address

            phrase = new Phrase();
            phrase.Add(new Chunk("Company Address -", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.FixedHeight = 20f;
            cell.Border = 0;
            cell.BorderColor = BaseColor.GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable1.AddCell(cell);

            if (clientLogodt.Rows.Count > 0)
            {
                for (int i = 0; i < clientLogodt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(clientLogodt.Rows[i]["Address"].ToString()), DataFont));
                    //phrase.Add(new Chunk("" + DateTime.Now.ToString("dd-MMM-yyy") + "", DataFont));
                    cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 5f;
                    //cell.PaddingTop = 8f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderColor = BaseColor.GRAY;
                    cell.Colspan = 4;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);

                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                //cell.Border = 0;
                cell.PaddingBottom = 6F;
                cell.BorderColor = BaseColor.GRAY;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
            }

            // for space 
            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            //cell.BackgroundColor = new iTextSharp.text.Color(220, 220, 220);
            cell.FixedHeight = 10f;
            cell.Border = 0;
            cell.BorderColor = BaseColor.WHITE;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable1.AddCell(cell);


            phrase = new Phrase();
            phrase.Add(new Chunk("Scaffold Hand Over Date", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.FixedHeight = 20f;
            cell.Border = 0;
            cell.BorderColor = BaseColor.GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable1.AddCell(cell);

            if (handoverdt.Rows.Count > 0)
            {
                for (int i = 0; i < handoverdt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    // phrase.Add(new Chunk(Convert.ToString(handoverdt.Rows[i]["HandOverDateTime"].ToString()), DataFont));
                    //phrase.Add(new Chunk("" + DateTime.Now.ToString("dd-MMM-yyy") + "", DataFont));

                    if (Convert.ToInt32(handoverdt.Rows[i]["HandOverTime"]) == 0)
                    {
                        phrase.Add(new Chunk(string.Format("{0:dd-MMM-yyyy}", handoverdt.Rows[i]["HandOverDateTime"]), DataFont));
                    }
                    else
                    {
                        phrase.Add(new Chunk(string.Format("{0:dd-MMM-yyyy hh:mm tt}", handoverdt.Rows[i]["HandOverDateTime"]), DataFont));
                    }
                    cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 5f;
                    //cell.PaddingTop = 8f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderColor = BaseColor.GRAY;
                    cell.Colspan = 4;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);

                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                //cell.Border = 0;
                cell.PaddingBottom = 6F;
                cell.BorderColor = BaseColor.GRAY;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
            }

            phrase = new Phrase();
            phrase.Add(new Chunk("Scaffold Hand Over Certificate #", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.Border = 0;
            cell.FixedHeight = 20f;
            cell.BorderColor = BaseColor.GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable1.AddCell(cell);


            if (handoverdt.Rows.Count > 0)
            {
                for (int i = 0; i < handoverdt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(handoverdt.Rows[i]["CertificateNo"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.BorderColor = BaseColor.GRAY;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 4;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);

                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.BorderColor = BaseColor.GRAY;
                cell.PaddingBottom = 6F;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
            }


            phrase = new Phrase();
            phrase.Add(new Chunk("Scaffold Owner (Client Name)", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.Border = 0;
            cell.FixedHeight = 20f;
            cell.BorderColor = BaseColor.GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable1.AddCell(cell);


            if (handoverdt.Rows.Count > 0)
            {
                for (int i = 0; i < handoverdt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(handoverdt.Rows[i]["OwnerName"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.BorderColor = BaseColor.GRAY;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 4;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);

                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.BorderColor = BaseColor.GRAY;
                cell.PaddingBottom = 6F;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
            }

            phrase = new Phrase();
            phrase.Add(new Chunk("Site", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.Border = 0;
            cell.FixedHeight = 20f;
            cell.BorderColor = BaseColor.GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable1.AddCell(cell);


            if (handoverdt.Rows.Count > 0)
            {
                for (int i = 0; i < handoverdt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(handoverdt.Rows[i]["SiteLocation"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.BorderColor = BaseColor.GRAY;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 4;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);

                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.BorderColor = BaseColor.GRAY;
                cell.PaddingBottom = 6F;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
            }

            phrase = new Phrase();
            phrase.Add(new Chunk("Building No.", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.Border = 0;
            cell.FixedHeight = 20f;
            cell.BorderColor = BaseColor.GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable1.AddCell(cell);


            if (handoverdt.Rows.Count > 0)
            {
                for (int i = 0; i < handoverdt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(handoverdt.Rows[i]["BuildingNo"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.BorderColor = BaseColor.GRAY;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 4;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);

                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.BorderColor = BaseColor.GRAY;
                cell.PaddingBottom = 6F;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
            }
            //add on 11 jan by s.sourabh
            phrase = new Phrase();
            phrase.Add(new Chunk("Location", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.Border = 0;
            cell.FixedHeight = 20f;
            cell.BorderColor = BaseColor.GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable1.AddCell(cell);


            if (handoverdt.Rows.Count > 0)
            {
                for (int i = 0; i < handoverdt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(handoverdt.Rows[i]["Location"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.BorderColor = BaseColor.GRAY;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 4;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);

                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.BorderColor = BaseColor.GRAY;
                cell.PaddingBottom = 6F;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
            }
            //end
            phrase = new Phrase();
            phrase.Add(new Chunk("Floor Level", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.Border = 0;
            cell.FixedHeight = 20f;
            cell.BorderColor = BaseColor.GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable1.AddCell(cell);


            if (handoverdt.Rows.Count > 0)
            {
                for (int i = 0; i < handoverdt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(handoverdt.Rows[i]["FloorLevel"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.BorderColor = BaseColor.GRAY;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 4;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);

                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.BorderColor = BaseColor.GRAY;
                cell.PaddingBottom = 6F;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
            }

            phrase = new Phrase();
            phrase.Add(new Chunk("Scaffold Tag #", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.Border = 0;
            cell.FixedHeight = 20f;
            cell.BorderColor = BaseColor.GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable1.AddCell(cell);


            if (handoverdt.Rows.Count > 0)
            {
                for (int i = 0; i < handoverdt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(handoverdt.Rows[i]["ScaffoldTag"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.BorderColor = BaseColor.GRAY;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 4;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);

                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.BorderColor = BaseColor.GRAY;
                cell.PaddingBottom = 6F;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
            }

            phrase = new Phrase();
            phrase.Add(new Chunk("Scaffold/Section being Handed Over. Description", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.Border = 0;
            cell.FixedHeight = 30f;
            cell.BorderColor = BaseColor.GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable1.AddCell(cell);


            if (handoverdt.Rows.Count > 0)
            {
                for (int i = 0; i < handoverdt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(handoverdt.Rows[i]["HandOverDesc"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.BorderColor = BaseColor.GRAY;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 4;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);

                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.BorderColor = BaseColor.GRAY;
                cell.PaddingBottom = 6F;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
            }

            phrase = new Phrase();
            phrase.Add(new Chunk("Photo of Scaffold/Section to be handed over", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.Border = 0;
            cell.FixedHeight = 20f;
            cell.BorderColor = BaseColor.GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable1.AddCell(cell);


            if (!string.IsNullOrEmpty(Convert.ToString(handoverdt.Rows[0]["HandOverImage"])))
            {
                var uri = new Uri(databaseCon.siteimagelink);
                var container = new CloudBlobContainer(uri);
                var blob = container.GetBlockBlobReference(handoverdt.Rows[0]["HandOverImage"].ToString());
                var noimage = Server.MapPath("~/images/noimg.jpg");
                var exists = blob.Exists();
                if (exists == false)
                {
                    string nophoto = Server.MapPath("~/images/noimg.jpg");
                    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(noimage), true);
                    //cell.FixedHeight = 150f;
                    cell.PaddingLeft = 10f;
                    cell.PaddingBottom = 10f;
                    cell.PaddingRight = 150f;

                    cell.PaddingTop = -20f;
                    cell.BorderColor = BaseColor.LIGHT_GRAY;
                    cell.Colspan = 4;
                }
                else
                {
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(databaseCon.siteimagelink + handoverdt.Rows[0]["HandOverImage"].ToString());

                    //added on 28 - July - 2022 start here
                    WebClient wc = new WebClient();
                    byte[] bytes = wc.DownloadData(databaseCon.siteimagelink + handoverdt.Rows[0]["HandOverImage"].ToString());
                    MemoryStream ms = new MemoryStream(bytes);
                    System.Drawing.Image img1 = System.Drawing.Image.FromStream(ms);
                    //added by 28-July-2022 end here 

                    // by baans 14/02/2020
                    var imagephotoangle = "";

                    if (handoverdt.Rows[0]["PhotoAngle"].ToString() != "")
                    {

                        imagephotoangle = handoverdt.Rows[0]["PhotoAngle"].ToString();
                        if (imagephotoangle == "-90")
                        {
                            imagephotoangle = "-270";
                        }
                        else if (imagephotoangle == "-270")
                        {
                            imagephotoangle = "-90";
                        }

                        else if (imagephotoangle == "90")
                        {
                            imagephotoangle = "270";
                        }

                        else if (imagephotoangle == "270")
                        {
                            imagephotoangle = "90";
                        }
                        else
                        {
                            imagephotoangle = handoverdt.Rows[0]["PhotoAngle"].ToString();
                        }

                    }
                    else
                    {
                        imagephotoangle = "0";
                    }
                    img.ScaleAbsolute(180f, 133f);
                    img.Rotation = (float)Math.PI / 2;

                    var imagesangle = Convert.ToString(imagephotoangle);
                    img.RotationDegrees = Convert.ToSingle(imagephotoangle);
                    //img.setRotationDegrees(90);
                    // end


                    img.ScaleAbsolute(180f, 133f);
                    // Added on 28-July-2022 start here
                    if (img1.PropertyIdList.Contains(0x0112))
                    {
                        PropertyItem propOrientation = img1.GetPropertyItem(0x0112);
                        short orientation = BitConverter.ToInt16(propOrientation.Value, 0);
                        if (orientation == 6)
                        {
                            img1.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        }
                        else if (orientation == 8)
                        {
                            img1.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        }
                        else if (orientation == 3)
                        {
                            img1.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        }
                        else
                        {
                            // Do nothing
                        }

                        cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img1, BaseColor.WHITE), true);
                    }
                    else
                    {
                        cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img), true);
                    }
                    // Added on 28-July-2022 end here 
                    /*cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img), true);*///Commented on 28-July-2022
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderColor = BaseColor.GRAY;
                    //cell.FixedHeight = 200f;
                    cell.PaddingLeft = 1f;
                    cell.PaddingBottom = 1f;
                    cell.PaddingRight = 100f;

                    cell.PaddingTop = 1f;
                    cell.Colspan = 4;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                }
            }
            else
            {
                string nophoto = Server.MapPath("~/images/noimg.jpg");
                cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
                //cell.FixedHeight = 150f;
                //cell.PaddingLeft = 10f;
                cell.Colspan = 4;
                cell.PaddingLeft = 10f;
                cell.PaddingBottom = 10f;
                cell.PaddingRight = 150f;

                cell.PaddingTop = 10f;
                cell.BorderColor = BaseColor.GRAY;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                //cell.PaddingTop = 20f;
            }
            cell.Border = 0;
            cell.Colspan = 4;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //cell.FixedHeight = 150f;
            cell.BorderColor = BaseColor.GRAY;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable1.AddCell(cell);


            phrase = new Phrase();
            phrase.Add(new Chunk("Scaffold Duty Rating", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.Border = 0;
            cell.FixedHeight = 20f;
            cell.BorderColor = BaseColor.GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable1.AddCell(cell);


            if (handoverdt.Rows.Count > 0)
            {
                for (int i = 0; i < handoverdt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(handoverdt.Rows[i]["DutyRating"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.BorderColor = BaseColor.GRAY;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 4;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);

                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.BorderColor = BaseColor.GRAY;
                cell.PaddingBottom = 6F;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
            }

            phrase = new Phrase();
            phrase.Add(new Chunk("Number of Working Decks", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.Border = 0;
            cell.FixedHeight = 20f;
            cell.BorderColor = BaseColor.GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable1.AddCell(cell);


            if (handoverdt.Rows.Count > 0)
            {
                for (int i = 0; i < handoverdt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(handoverdt.Rows[i]["NoOfWorkingDecks"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.BorderColor = BaseColor.GRAY;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 4;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);

                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.BorderColor = BaseColor.GRAY;
                cell.PaddingBottom = 6F;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
            }

            // for Maximun Number of Platform 29 July 2019
            phrase = new Phrase();
            phrase.Add(new Chunk("Max Number of Decks Loaded at one time", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.Border = 0;
            cell.FixedHeight = 20f;
            cell.BorderColor = BaseColor.GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable1.AddCell(cell);


            if (handoverdt.Rows.Count > 0)
            {
                for (int i = 0; i < handoverdt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(handoverdt.Rows[i]["NumberOfPlatform"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.BorderColor = BaseColor.GRAY;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 4;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);

                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.BorderColor = BaseColor.GRAY;
                cell.PaddingBottom = 6F;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
            }

            phrase = new Phrase();
            phrase.Add(new Chunk("Has this Scaffold been designed to be Encapsulated?", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.Border = 0;
            cell.FixedHeight = 30f;
            cell.BorderColor = BaseColor.GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable1.AddCell(cell);


            if (handoverdt.Rows.Count > 0)
            {
                for (int i = 0; i < handoverdt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(handoverdt.Rows[i]["HasEncapsulated"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.BorderColor = BaseColor.GRAY;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 4;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);

                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.BorderColor = BaseColor.GRAY;
                cell.PaddingBottom = 6F;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
            }

            // for Engineered Design
            phrase = new Phrase();
            phrase.Add(new Chunk("Does this scaffold have an Engineered Design?", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.Border = 0;
            cell.FixedHeight = 30f;
            cell.BorderColor = BaseColor.GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable1.AddCell(cell);


            if (handoverdt.Rows.Count > 0)
            {
                for (int i = 0; i < handoverdt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(handoverdt.Rows[i]["EngineeredDesign"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.BorderColor = BaseColor.GRAY;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 4;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);

                }

            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.BorderColor = BaseColor.GRAY;
                cell.PaddingBottom = 6F;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
            }

            // for Engineered Design
            phrase = new Phrase();
            phrase.Add(new Chunk("Design Drawing No:", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.Border = 0;
            cell.FixedHeight = 20f;
            cell.BorderColor = BaseColor.GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable1.AddCell(cell);


            if (handoverdt.Rows.Count > 0)
            {
                for (int i = 0; i < handoverdt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(handoverdt.Rows[i]["DesignDrawingNo"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.BorderColor = BaseColor.GRAY;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 4;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);

                }

            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.BorderColor = BaseColor.GRAY;
                cell.PaddingBottom = 6F;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
            }


            //phrase = new Phrase();
            //phrase.Add(new Chunk("Hand Over Location", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.BackgroundColor = new iTextSharp.text.Color(220, 220, 220);
            //cell.Border = 0;
            //cell.FixedHeight = 20f;
            //cell.BorderColor = Color.GRAY;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            //headertable1.AddCell(cell);


            //if (!string.IsNullOrEmpty(Convert.ToString(handoverdt.Rows[0]["HandoverMapImage"])))
            //{
            //    var uri = new Uri(databaseCon.siteimagelink);
            //    var container = new CloudBlobContainer(uri);
            //    var blob = container.GetBlockBlobReference(handoverdt.Rows[0]["HandoverMapImage"].ToString());
            //    var noimage = Server.MapPath("images/noimg.jpg");
            //    var exists = blob.Exists();
            //    if (exists == false)
            //    {
            //        string nophoto = Server.MapPath("images/noimg.jpg");
            //        cell = new PdfPCell(iTextSharp.text.Image.GetInstance(noimage), true);
            //        cell.FixedHeight = 140f;
            //        cell.PaddingLeft = 10f;
            //        cell.PaddingBottom = 10f;
            //        //cell.PaddingRight = 150f;

            //        cell.PaddingTop = 10f;
            //        cell.BorderColor = Color.LIGHT_GRAY;
            //        cell.Colspan = 4;
            //    }
            //    else
            //    {
            //        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(databaseCon.siteimagelink + handoverdt.Rows[0]["HandoverMapImage"].ToString());
            //        img.ScaleAbsolute(180f, 130f);

            //        cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img), true);
            //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //        cell.BorderColor = Color.GRAY;
            //        cell.FixedHeight = 140f;
            //        cell.PaddingLeft = 1f;
            //        cell.PaddingBottom = 1f;
            //        //cell.PaddingRight = 100f;

            //        cell.PaddingTop = 1f;
            //        cell.Colspan = 4;
            //        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            //    }
            //}
            //else
            //{
            //    string nophoto = Server.MapPath("images/noimg.jpg");
            //    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
            //    cell.FixedHeight = 140f;
            //    //cell.PaddingLeft = 10f;
            //    cell.Colspan = 4;
            //    cell.PaddingLeft = 10f;
            //    cell.PaddingBottom = 10f;
            //    //cell.PaddingRight = 150f;

            //    cell.PaddingTop = 10f;
            //    cell.BorderColor = Color.GRAY;
            //    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            //    cell.PaddingTop = 20f;
            //}
            //cell.Border = 0;
            //cell.Colspan = 4;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //cell.FixedHeight = 140f;
            //cell.BorderColor = Color.GRAY;
            //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            //headertable1.AddCell(cell);


            //phrase = new Phrase();
            //phrase.Add(new Chunk("Audio Notes", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.BackgroundColor = new iTextSharp.text.Color(220, 220, 220);
            //cell.Border = 0;
            //cell.FixedHeight = 20f;
            //cell.BorderColor = Color.GRAY;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            //headertable1.AddCell(cell);


            //if (handoverdt.Rows.Count > 0)
            //{
            //    for (int i = 0; i < handoverdt.Rows.Count; i++)
            //    {

            //        if (!string.IsNullOrEmpty(Convert.ToString(handoverdt.Rows[i]["HandOverAudio"].ToString())))
            //        {
            //            phrase = new Phrase();
            //            phrase.Add(new Chunk("Yes", Heading786));
            //            cell = new PdfPCell(phrase);
            //        }
            //        else
            //        {
            //            phrase = new Phrase();
            //            phrase.Add(new Chunk("No", Heading786));
            //            cell = new PdfPCell(phrase);
            //        }
            //        cell.Border = 0;
            //        cell.BorderColor = Color.GRAY;
            //        cell.FixedHeight = 1f;
            //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            //        cell.Colspan = 4;
            //        headertable1.AddCell(cell);


            //    }
            //}
            //else
            //{
            //    phrase = new Phrase();
            //    phrase.Add(new Chunk("", DataFont));
            //    cell = new PdfPCell(phrase);
            //    cell.Colspan = 4;
            //    //cell.Border = 0;
            //    cell.PaddingBottom = 6F;
            //    //cell.PaddingTop = -50f;
            //    cell.BorderColor = Color.GRAY;
            //    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            //    cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //    headertable1.AddCell(cell);
            //}

            //phrase = new Phrase();
            //phrase.Add(new Chunk("Video", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.BackgroundColor = new iTextSharp.text.Color(220, 220, 220);
            //cell.Border = 0;
            //cell.FixedHeight = 20f;
            //cell.BorderColor = Color.GRAY;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            //headertable1.AddCell(cell);


            //if (handoverdt.Rows.Count > 0)
            //{
            //    for (int i = 0; i < handoverdt.Rows.Count; i++)
            //    {

            //        if (!string.IsNullOrEmpty(Convert.ToString(handoverdt.Rows[i]["HandOverVedio"].ToString())))
            //        {
            //            phrase = new Phrase();
            //            phrase.Add(new Chunk("Yes", Heading786));
            //            cell = new PdfPCell(phrase);
            //        }
            //        else
            //        {
            //            phrase = new Phrase();
            //            phrase.Add(new Chunk("No", Heading786));
            //            cell = new PdfPCell(phrase);
            //        }
            //        cell.Border = 0;
            //        cell.BorderColor = Color.GRAY;
            //        cell.FixedHeight = 1f;
            //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            //        cell.Colspan = 4;
            //        headertable1.AddCell(cell);


            //    }
            //}
            //else
            //{
            //    phrase = new Phrase();
            //    phrase.Add(new Chunk("", DataFont));
            //    cell = new PdfPCell(phrase);
            //    cell.Colspan = 4;
            //    //cell.Border = 0;
            //    cell.PaddingBottom = 6F;
            //    //cell.PaddingTop = -50f;
            //    cell.BorderColor = Color.GRAY;
            //    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            //    cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //    headertable1.AddCell(cell);
            //}

            // for Reason for Access 29 July 2019
            phrase = new Phrase();
            phrase.Add(new Chunk("Reason for Access", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.Border = 0;
            cell.FixedHeight = 20f;
            cell.BorderColor = BaseColor.GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable1.AddCell(cell);


            if (handoverdt.Rows.Count > 0)
            {
                for (int i = 0; i < handoverdt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(handoverdt.Rows[i]["ReasonForAccess"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.BorderColor = BaseColor.GRAY;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 4;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);

                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.BorderColor = BaseColor.GRAY;
                cell.PaddingBottom = 6F;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
            }

            // for Length 29 July 2019
            phrase = new Phrase();
            phrase.Add(new Chunk("Length (m)", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.Border = 0;
            cell.FixedHeight = 20f;
            cell.BorderColor = BaseColor.GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable1.AddCell(cell);


            if (handoverdt.Rows.Count > 0)
            {
                for (int i = 0; i < handoverdt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    //phrase.Add(new Chunk(Convert.ToString(handoverdt.Rows[i]["Length"].ToString())), DataFont));
                    phrase.Add(new Chunk(string.Format("{0:0.00}", handoverdt.Rows[i]["Length"]), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.BorderColor = BaseColor.GRAY;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 4;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);

                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.BorderColor = BaseColor.GRAY;
                cell.PaddingBottom = 6F;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
            }

            // for Width Access 29 July 2019
            phrase = new Phrase();
            phrase.Add(new Chunk("Width (m)", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.Border = 0;
            cell.FixedHeight = 20f;
            cell.BorderColor = BaseColor.GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable1.AddCell(cell);


            if (handoverdt.Rows.Count > 0)
            {
                for (int i = 0; i < handoverdt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    //phrase.Add(new Chunk(Convert.ToString(handoverdt.Rows[i]["Width"].ToString()), DataFont));
                    phrase.Add(new Chunk(string.Format("{0:0.00}", handoverdt.Rows[i]["Width"]), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.BorderColor = BaseColor.GRAY;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 4;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);

                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.BorderColor = BaseColor.GRAY;
                cell.PaddingBottom = 6F;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
            }

            // for Height 29 July 2019
            phrase = new Phrase();
            phrase.Add(new Chunk("Height TWP (m)", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.Border = 0;
            cell.FixedHeight = 20f;
            cell.BorderColor = BaseColor.GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable1.AddCell(cell);


            if (handoverdt.Rows.Count > 0)
            {
                for (int i = 0; i < handoverdt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    //phrase.Add(new Chunk(Convert.ToString(handoverdt.Rows[i]["Height"].ToString()), DataFont));
                    phrase.Add(new Chunk(string.Format("{0:0.00}", handoverdt.Rows[i]["Height"]), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.BorderColor = BaseColor.GRAY;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 4;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);

                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.BorderColor = BaseColor.GRAY;
                cell.PaddingBottom = 6F;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
            }

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading788));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.FixedHeight = 10f;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading788));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.FixedHeight = 10f;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading788));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.FixedHeight = 10f;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading788));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.FixedHeight = 10f;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading788));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.FixedHeight = 10f;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading788));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.FixedHeight = 10f;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading788));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.FixedHeight = 10f;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading788));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.FixedHeight = 10f;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading788));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.FixedHeight = 10f;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading788));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.FixedHeight = 10f;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading788));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.FixedHeight = 10f;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading788));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.FixedHeight = 10f;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading788));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.FixedHeight = 10f;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading788));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.FixedHeight = 10f;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading788));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.FixedHeight = 10f;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);


            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading788));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.FixedHeight = 10f;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading788));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.FixedHeight = 10f;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading788));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.FixedHeight = 10f;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading788));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.FixedHeight = 10f;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading788));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.FixedHeight = 10f;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading788));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.FixedHeight = 10f;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading788));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.FixedHeight = 10f;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading788));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.FixedHeight = 10f;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Hand Over & Sign Off", Heading788));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            //cell.FixedHeight = 20f;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable3.AddCell(cell);


            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading788));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.FixedHeight = 10f;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading788));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            //cell.FixedHeight = 20f;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Comments", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.Border = 0;
            cell.FixedHeight = 20f;
            cell.BorderColor = BaseColor.GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable3.AddCell(cell);


            if (handoverdt.Rows.Count > 0)
            {
                for (int i = 0; i < handoverdt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(handoverdt.Rows[i]["HandOverComments"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.BorderColor = BaseColor.GRAY;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 4;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable3.AddCell(cell);

                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.BorderColor = BaseColor.GRAY;
                cell.PaddingBottom = 6F;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }

            phrase = new Phrase();
            phrase.Add(new Chunk("Acknowledgement", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.Border = 0;
            cell.FixedHeight = 20f;
            cell.BorderColor = BaseColor.GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable3.AddCell(cell);


            if (handoverdt.Rows.Count > 0)
            {
                for (int i = 0; i < handoverdt.Rows.Count; i++)
                {
                    string var = "";
                    if (!String.IsNullOrEmpty(handoverdt.Rows[i]["HandOverAcknowledgment"].ToString()) == false || handoverdt.Rows[i]["HandOverAcknowledgment"].ToString() == "false")
                    {
                        var = "false";
                    }
                    else
                    {
                        var = "true";
                    }

                    if (var == "true")
                    //if (Convert.ToBoolean(handoverdt.Rows[i]["HandOverAcknowledgment"]) == true)
                    {
                        phrase = new Phrase();
                        //phrase.Add(new Chunk("Scaffolding, as described above, has now been completed and complies with the following Standards AS/NZ 1576:1 to 6 and Scaffold Guidelines AS/NZ 4576:1995 . It is structurally sound and should only be used and loaded in accordance with the Duty Rating. (The rating is for a load evenly distributed across a single bay).", Heading786));
                        phrase.Add(new Chunk("The structure as described above, has now been completed and complies with Standards AS/NZS 1576 ALL PARTS, Scaffold Guidelines AS/NZS 4576 and any other applicable standards, guidelines or regulations.  It is structurally sound and ready for use.", Heading786));
                        phrase.Add(Environment.NewLine);
                        phrase.Add(Environment.NewLine);
                        phrase.Add(new Chunk("The detailed requirements of the regulations with regard to guard-rails, working platforms, toe-boards, bracing and ties have been complied with.", Heading786));
                        phrase.Add(Environment.NewLine);
                        phrase.Add(Environment.NewLine);
                        phrase.Add(new Chunk("This structure has been inspected and tagged on completion of build. It must be inspected at minimum intervals of 30 days or at site interval requirements, whichever is the least number of days, since last inspection. It must also be inspected after any event likely to jeopardise the structures integrity , after part dismantle or any modification. Particulars of future inspections to be recorded in the inspection report.", Heading786));
                        phrase.Add(Environment.NewLine);
                        phrase.Add(Environment.NewLine);
                        phrase.Add(new Chunk("NO UNAUTHORIZED MODIFICATIONS OR ALTERATIONS ARE TO BE MADE TO THE STRUCTURE.", Heading786));
                        phrase.Add(Environment.NewLine);
                        phrase.Add(Environment.NewLine);
                        phrase.Add(new Chunk("\xFE " + "", contentfontCheck));
                        //phrase.Add(new Chunk("  "));

                        phrase.Add(new Chunk(" I Agree", Heading786));


                        cell = new PdfPCell(phrase);
                        cell.Colspan = 4;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        cell.BorderColor = BaseColor.GRAY;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(176, 255, 175);
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        headertable3.AddCell(cell);
                    }
                    else
                    {
                        phrase = new Phrase();
                        //phrase.Add(new Chunk("Scaffolding, as described above, has now been completed and complies with the following Standards AS/NZ 1576:1 to 6 and Scaffold Guidelines AS/NZ 4576:1995 . It is structurally sound and should only be used and loaded in accordance with the Duty Rating. (The rating is for a load evenly distributed across a single bay).", Heading786));
                        phrase.Add(new Chunk("The structure as described above, has now been completed and complies with Standards AS/NZS 1576 ALL PARTS, Scaffold Guidelines AS/NZS 4576 and any other applicable standards, guidelines or regulations.  It is structurally sound and ready for use.", Heading786));
                        phrase.Add(Environment.NewLine);
                        phrase.Add(Environment.NewLine);
                        phrase.Add(new Chunk("The detailed requirements of the regulations with regard to gaurd-rails, working platforms, toe-boards, bracing and ties have been complied with.", Heading786));
                        phrase.Add(Environment.NewLine);
                        phrase.Add(new Chunk("This structure has been inspected and tagged on completion of build. It must be inspected at minimum intervals of 30 days or at site interval requirements, whichever is the least number of days, since last inspection. It must also be inspected after any event likely to jeopardise the structures integrity , after part dismantle or any modification. Particulars of future inspections to be recorded in the inspection report.", Heading786));
                        phrase.Add(Environment.NewLine);
                        phrase.Add(Environment.NewLine);
                        phrase.Add(new Chunk("NO UNAUTHORIZED MODIFICATIONS OR ALTERATIONS ARE TO BE MADE TO THE STRUCTURE.", Heading786));
                        phrase.Add(Environment.NewLine);
                        phrase.Add(Environment.NewLine);
                        phrase.Add(new Chunk("\x6F" + " ", contentfontUncheck));
                        phrase.Add(new Chunk("I Agree", Heading786));
                        cell = new PdfPCell(phrase);
                        cell.Colspan = 4;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        cell.BorderColor = BaseColor.GRAY;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        headertable3.AddCell(cell);
                    }

                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.BorderColor = BaseColor.GRAY;
                cell.PaddingBottom = 6F;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }

            phrase = new Phrase();
            phrase.Add(new Chunk("Scaffold Contractor Representative", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.Border = 0;
            cell.FixedHeight = 20f;
            cell.BorderColor = BaseColor.GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable3.AddCell(cell);


            if (handoverdt.Rows.Count > 0)
            {
                for (int i = 0; i < handoverdt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(handoverdt.Rows[i]["ContractorRepName"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.BorderColor = BaseColor.GRAY;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 4;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable3.AddCell(cell);

                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.BorderColor = BaseColor.GRAY;
                cell.PaddingBottom = 6F;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }

            phrase = new Phrase();
            phrase.Add(new Chunk("Licence to Perform High Risk Work #", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.Border = 0;
            cell.FixedHeight = 20f;
            cell.BorderColor = BaseColor.GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable3.AddCell(cell);


            if (handoverdt.Rows.Count > 0)
            {
                for (int i = 0; i < handoverdt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(handoverdt.Rows[i]["ContractorRepLicence"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.BorderColor = BaseColor.GRAY;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 4;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable3.AddCell(cell);

                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.BorderColor = BaseColor.GRAY;
                cell.PaddingBottom = 6F;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }

            phrase = new Phrase();
            phrase.Add(new Chunk("Scaffold Contractor Representative Position", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.Border = 0;
            cell.FixedHeight = 30f;
            cell.BorderColor = BaseColor.GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable3.AddCell(cell);


            if (handoverdt.Rows.Count > 0)
            {
                for (int i = 0; i < handoverdt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(handoverdt.Rows[i]["ContractorRepPosition"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.BorderColor = BaseColor.GRAY;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 4;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable3.AddCell(cell);

                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.BorderColor = BaseColor.GRAY;
                cell.PaddingBottom = 6F;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }

            phrase = new Phrase();
            phrase.Add(new Chunk("Scaffold Contractor Representative Signature", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.Border = 0;
            cell.FixedHeight = 20f;
            cell.BorderColor = BaseColor.GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable3.AddCell(cell);


            if (!string.IsNullOrEmpty(Convert.ToString(handoverdt.Rows[0]["ContractorSign"])))
            {
                var uri = new Uri(databaseCon.siteimagelink);
                var container = new CloudBlobContainer(uri);
                var blob = container.GetBlockBlobReference(handoverdt.Rows[0]["ContractorSign"].ToString());
                //var noimage = Server.MapPath("images/noimg.jpg");
                var exists = blob.Exists();
                if (exists == false)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("", Heading786));
                    cell = new PdfPCell(phrase);
                    cell.Colspan = 4;
                    cell.FixedHeight = 150f;
                    cell.BorderColor = BaseColor.GRAY;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;

                }
                else
                {
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(databaseCon.siteimagelink + handoverdt.Rows[0]["ContractorSign"].ToString());
                    img.ScaleAbsolute(180f, 133f);

                    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img), true);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderColor = BaseColor.GRAY;
                    cell.PaddingLeft = 1f;
                    cell.PaddingBottom = 1f;
                    cell.PaddingRight = 100f;

                    cell.PaddingTop = 1f;
                    cell.Colspan = 4;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.FixedHeight = 150f;
                cell.BorderColor = BaseColor.GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;

            }
            cell.Border = 0;
            cell.Colspan = 4;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.FixedHeight = 150f;
            cell.BorderColor = BaseColor.GRAY;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Scaffold Owner (Client) Representative", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.Border = 0;
            cell.FixedHeight = 20f;
            cell.BorderColor = BaseColor.GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable3.AddCell(cell);


            if (handoverdt.Rows.Count > 0)
            {
                for (int i = 0; i < handoverdt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(handoverdt.Rows[i]["RepresentativeName"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.BorderColor = BaseColor.GRAY;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 4;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable3.AddCell(cell);

                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.BorderColor = BaseColor.GRAY;
                cell.PaddingBottom = 6F;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }

            phrase = new Phrase();
            phrase.Add(new Chunk("Scaffold Owner (Client) Representative Position", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.Border = 0;
            cell.FixedHeight = 20f;
            cell.BorderColor = BaseColor.GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable3.AddCell(cell);


            if (handoverdt.Rows.Count > 0)
            {
                for (int i = 0; i < handoverdt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(handoverdt.Rows[i]["Position"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.BorderColor = BaseColor.GRAY;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 4;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable3.AddCell(cell);

                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.BorderColor = BaseColor.GRAY;
                cell.PaddingBottom = 6F;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }

            phrase = new Phrase();
            phrase.Add(new Chunk("Scaffold Owner (Client) Rep To Sign Here", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.Border = 0;
            cell.FixedHeight = 20f;
            cell.BorderColor = BaseColor.GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable3.AddCell(cell);


            if (!string.IsNullOrEmpty(Convert.ToString(handoverdt.Rows[0]["RepresentativeSign"])))
            {
                var uri = new Uri(databaseCon.siteimagelink);
                var container = new CloudBlobContainer(uri);
                var blob = container.GetBlockBlobReference(handoverdt.Rows[0]["RepresentativeSign"].ToString());
                //var noimage = Server.MapPath("images/noimg.jpg");
                var exists = blob.Exists();
                if (exists == false)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("", Heading786));
                    cell = new PdfPCell(phrase);
                    cell.Colspan = 4;
                    cell.FixedHeight = 150f;
                    cell.BorderColor = BaseColor.GRAY;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;

                }
                else
                {
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(databaseCon.siteimagelink + handoverdt.Rows[0]["RepresentativeSign"].ToString());
                    img.ScaleAbsolute(180f, 133f);

                    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img), true);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderColor = BaseColor.GRAY;
                    cell.PaddingLeft = 1f;
                    cell.PaddingBottom = 1f;
                    cell.PaddingRight = 100f;

                    cell.PaddingTop = 1f;
                    cell.Colspan = 4;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.FixedHeight = 150f;
                cell.BorderColor = BaseColor.GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;

            }
            cell.Border = 0;
            cell.Colspan = 4;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.FixedHeight = 150f;
            cell.BorderColor = BaseColor.GRAY;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable3.AddCell(cell);

            doc.Add(headertable2);

            doc.Add(headertable1);
            //doc.NewPage();
            doc.Add(headertable3);
            doc.Close();

            return txttempfile + "~" + txtFile;
        }
    }
}