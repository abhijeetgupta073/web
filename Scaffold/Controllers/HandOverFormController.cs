using System;
using System.Collections.Generic;
using System.Data;
//using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
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
    public class HandOverFormController : Controller
    {
        ScaffoldEntities dbcontext = new ScaffoldEntities();
        // GET: HandOverForm
        public ActionResult HandOverForm()
        {
            if (Session["UserEmail"] == null)
            {
                //Response.Redirect("~/Login.aspx");
                ViewBag.IsLogin = "";
            }
            else
            {
                ViewBag.IsLogin = Session["UserEmail"];
            }
            string ClientQuery = "select ClientImage,Address from tblClient where ClientId = '" + databaseCon.Decrypt(Request.QueryString["id"]) + "'";
            DataTable clientdt = databaseCon.GetData(ClientQuery);

            if (clientdt.Rows.Count > 0)
            {
                if (clientdt.Rows[0]["ClientImage"].ToString() != "")
                {
                    string imgname = clientdt.Rows[0]["ClientImage"].ToString();
                    if (clientdt.Rows[0]["ClientImage"].ToString() == "")
                    {
                        //imgLogo.Attributes.Add("src", "/img/logo1.png");
                        //ViewBag.imgLogo = "/img/logo1.png";
                        ViewBag.imgLogo =  databaseCon.logoimage;
                    }
                    else
                    {
                        // string a = databaseCon.clientimageLink + clientdt.Rows[0]["ClientImage"].ToString();
                       // imgLogo.Attributes.Add("src", "/images/clientlogo/" + clientdt.Rows[0]["ClientImage"].ToString());
                        ViewBag.imgLogo = "/images/clientlogo/" + clientdt.Rows[0]["ClientImage"].ToString();
                    }

                }
                else
                {
                    // imgLogo.Attributes.Add("src", "/img/logo1.png");
                    // ViewBag.imgLogo = "/img/logo1.png";
                    ViewBag.imgLogo = databaseCon.logoimage;
                }
                //CompanyAdd.InnerText = clientdt.Rows[0]["Address"].ToString();
                ViewBag.CompanyAdd= clientdt.Rows[0]["Address"].ToString();
            }
            return View();
        }
        //public ActionResult checkuserrole(string UserRole)
        //{
        //    string roleNumber = "";
        //    string query1 = "select * from tbluserRole where RoleNumber='" + databaseCon.Decrypt(UserRole) + "'";
        //    DataTable dt = databaseCon.GetData(query1);
        //    if (dt.Rows.Count > 0)
        //    {
        //        roleNumber = dt.Rows[0]["RoleNumber"].ToString();
        //    }
        //    return Json(roleNumber, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult BindallDropdown(string id)
        {
            var ddlsiteManager = dbcontext.Database.SqlQuery<DropDownViewModel>("select Position,CRId from tblClientRepresentation where ClientId = '" + databaseCon.Decrypt(id) + "' order by Position");

            var ddlContractorRepresentative = dbcontext.Database.SqlQuery<DropDownViewModel>("select UserId,firstName+' '+LastName as name from tblUser where IsInspector = 'true' and Status='Active' and ClientId = '" + databaseCon.Decrypt(id) + "' Order by name ");

            return Json(new { ddlsiteManager = ddlsiteManager, ddlContractorRepresentative = ddlContractorRepresentative}, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetHandOverFormData(string id, string RID, string HandOverId,string hdnTimeOffSet) {

            DataTable dt;
            // Changes On 29 July 2019
            //  String handOverQuery = "Select REPLACE(CONVERT(VARCHAR(11),getdate(),106), ' ','-') as CurrentDate,HandOverDate, VW_tblRegister.OwnerName,SiteLocation,BuildingNo, FloorLevel ,Right('000000' + Convert(varchar, ScaffRef), 6)  as ScaffoldTag  ,VW_tblRegister.Height,VW_tblRegister.Width,VW_tblRegister.Length,VW_tblRegister.DesignDrawingNo,VW_tblRegister.Description as ReasonForAccess,VW_TblRegister.NumberOfPlatform,VW_TblRegister.EngineeredDesign,HandOverDesc,IsConfirmed , HandOverImage ,  DutyRating , NoOfWorkingDecks , '' as HeightOFTopWorkingPlateform , HasEncapsulated ,HandOverLat, HandOverLong,HandOverAddress , HandOverAudio  ,HandOverVedio , HandOverComments , HandOverAcknowledgment,(select Right('000000'+Convert(varchar,CertificateNo),6)as CertificateNo from tblHandOver where RegisterId = '" + databaseCon.Decrypt(Request.QueryString["RID"]) + "' and HandOverId='" + databaseCon.Decrypt(Request.QueryString["HandOverId"]) + "') as CertificateNo,FirstName + ' ' + LastName as ContractorRepresentative ,tblUser.LicenceNo,tblUser.[Role] as ContractorPosition  , ContractorSign,CRId , RepresentativeName, RepresentativeSign from VW_tblRegister join tblInspection on VW_tblRegister.RegisterId = tblInspection.RegisterID left join tblUser on tblInspection.InspectedById = tblUser.UserId join tblClient on VW_TblRegister.ClientID = tblClient.ClientId left join tblHandOver on tblHandOver.InspId = tblInspection.InspectionID and tblHandOver.RegisterID = VW_TblRegister.RegisterId   where  VW_tblRegister.RegisterID = '" + databaseCon.Decrypt(Request.QueryString["RID"]) + "' and InspId='" + databaseCon.Decrypt(Request.QueryString["InspId"]) + "' and HandOverId='" + databaseCon.Decrypt(Request.QueryString["HandOverId"]) + "'";

            // added on 2-Aug three column in tblhandover
            //   String handOverQuery = "Select REPLACE(CONVERT(VARCHAR(11),getdate(),106), ' ','-') as CurrentDate,HandOverDate, VW_tblRegister.OwnerName,SiteLocation,BuildingNo, FloorLevel ,Right('000000' + Convert(varchar, ScaffRef), 6)  as ScaffoldTag  ,VW_tblRegister.Height,VW_tblRegister.Width,VW_tblRegister.Length,VW_tblRegister.DesignDrawingNo,VW_tblRegister.Description as ReasonForAccess,VW_TblRegister.NumberOfPlatform,VW_TblRegister.EngineeredDesign,HandOverDesc,IsConfirmed , HandOverImage ,  DutyRating , NoOfWorkingDecks , '' as HeightOFTopWorkingPlateform , HasEncapsulated ,HandOverLat, HandOverLong,HandOverAddress , HandOverAudio  ,HandOverVedio , HandOverComments , HandOverAcknowledgment,(select Right('000000'+Convert(varchar,CertificateNo),6)as CertificateNo from tblHandOver where RegisterId = '" + databaseCon.Decrypt(Request.QueryString["RID"]) + "' and HandOverId='" + databaseCon.Decrypt(Request.QueryString["HandOverId"]) + "') as CertificateNo,ContractorRepName ,ContractorRepLicence,ContractorRepPosition , ContractorSign,CRId , RepresentativeName, RepresentativeSign from VW_tblRegister join tblInspection on VW_tblRegister.RegisterId = tblInspection.RegisterID left join tblUser on tblInspection.InspectedById = tblUser.UserId join tblClient on VW_TblRegister.ClientID = tblClient.ClientId left join tblHandOver on tblHandOver.InspId = tblInspection.InspectionID and tblHandOver.RegisterID = VW_TblRegister.RegisterId   where  VW_tblRegister.RegisterID = '" + databaseCon.Decrypt(Request.QueryString["RID"]) + "' and InspId='" + databaseCon.Decrypt(Request.QueryString["InspId"]) + "' and HandOverId='" + databaseCon.Decrypt(Request.QueryString["HandOverId"]) + "'";

            //Query Comment On 27 Jan 2020
            //  String handOverQuery = "Select REPLACE(CONVERT(VARCHAR(11), getdate(), 106), ' ', '-') as CurrentDate,HandOverDate, VW_tblRegister.OwnerName,SiteLocation,BuildingNo, FloorLevel ,Right('000000' + Convert(varchar, ScaffRef), 6) as ScaffoldTag  ,VW_tblRegister.Height,VW_tblRegister.Width,VW_tblRegister.Length,VW_tblRegister.DesignDrawingNo,VW_tblRegister.Description as ReasonForAccess,VW_TblRegister.NumberOfPlatform,VW_TblRegister.EngineeredDesign,HandOverDesc,IsConfirmed , HandOverImage ,  DutyRating , NoOfWorkingDecks , '' as HeightOFTopWorkingPlateform , HasEncapsulated ,HandOverLat, HandOverLong,HandOverAddress , HandOverAudio  ,HandOverVedio , HandOverComments , HandOverAcknowledgment,(select Right('000000' + Convert(varchar, CertificateNo), 6) as CertificateNo from tblHandOver where RegisterId ='" + databaseCon.Decrypt(Request.QueryString["RID"]) + "' and HandOverId = '" + databaseCon.Decrypt(Request.QueryString["HandOverId"]) + "') as CertificateNo,ContractorRepName ,ContractorRepLicence,ContractorRepPosition , ContractorSign,CRId , RepresentativeName, RepresentativeSign,tblHandOver.PhotoAngle from VW_tblRegister join tblHandOver on VW_tblRegister.RegisterId = tblHandOver.RegisterID left join tblUser on tblHandOver.HandOverId = tblUser.UserId join tblClient on VW_TblRegister.ClientID = tblClient.ClientId    where VW_tblRegister.RegisterID ='" + databaseCon.Decrypt(Request.QueryString["RID"]) + "'  and HandOverId = '" + databaseCon.Decrypt(Request.QueryString["HandOverId"]) + "'";


            String handOverQuery = "Select REPLACE(CONVERT(VARCHAR(11), getdate(), 106), ' ', '-') as CurrentDate,Isnull(Convert(int,LEFT(Replace(convert(Time,DateAdd(minute, Isnull(HandoverTimezone,0), HandOverDate)),':',''),4)),0) as HandOverTime,DateAdd(minute, Isnull(HandoverTimezone,0), HandOverDate) as HandOverDate, VW_tblRegister.OwnerName,SiteLocation,BuildingNo,VW_tblRegister.Location, FloorLevel ,Right('000000' + Convert(varchar, ScaffRef), 6)as ScaffoldTag  ,VW_tblRegister.Height,VW_tblRegister.Width,VW_tblRegister.Length,VW_tblRegister.DesignDrawingNo,VW_tblRegister.Description as ReasonForAccess,VW_TblRegister.NumberOfPlatform,VW_TblRegister.EngineeredDesign,HandOverDesc,IsConfirmed , HandOverImage ,  DutyRating , NoOfWorkingDecks , '' as HeightOFTopWorkingPlateform , HasEncapsulated ,HandOverLat, HandOverLong,HandOverAddress , HandOverAudio  ,HandOverVedio , HandOverComments , HandOverAcknowledgment,(select Right('000000' + Convert(varchar, CertificateNo), 6) as CertificateNo from tblHandOver where RegisterId = '" + databaseCon.Decrypt(RID) + "' and HandOverId ='" + databaseCon.Decrypt(HandOverId) + "') as CertificateNo,ContractorRepName ,ContractorRepLicence,ContractorRepPosition , ContractorSign,CRId , RepresentativeName, RepresentativeSign,tblHandOver.PhotoAngle from VW_tblRegister join tblHandOver on VW_tblRegister.RegisterId = tblHandOver.RegisterID left join tblUser on tblHandOver.HandOverId = tblUser.UserId join tblClient on VW_TblRegister.ClientID = tblClient.ClientId where VW_tblRegister.RegisterID ='" + databaseCon.Decrypt(RID) + "'  and HandOverId ='" + databaseCon.Decrypt(HandOverId) + "'";


            dt = databaseCon.GetData(handOverQuery);
            var GetData = dbcontext.Database.SqlQuery<HandOverFormViewModel>(handOverQuery).FirstOrDefault();
            if(GetData!=null) {
                var HandOverDate = GetData.HandOverDate;
                 if (HandOverDate != null)
                {
                    GetData.HandOverDate = Convert.ToDateTime(HandOverDate).AddMinutes(Convert.ToDouble(hdnTimeOffSet));
                }
                

                //HandOver Image Code Starts Here 
                if (dt.Rows[0]["HandOverImage"].ToString() != "")
                {
                    var uri = new Uri(databaseCon.siteimagelink);
                    var container = new CloudBlobContainer(uri);
                    var blob = container.GetBlockBlobReference(dt.Rows[0]["HandOverImage"].ToString());
                    var exists = blob.Exists();
                    if (exists == true)
                    {
                        string HandOverUrl = databaseCon.siteimagelink + dt.Rows[0]["HandOverImage"].ToString();
                        GetData.HandOverImage = HandOverUrl;
                    }
                }
                //HandOver Image Code Ends Here

                //HandOver Audio Code Starts Here 
                if (dt.Rows[0]["HandOverAudio"].ToString() != "")
                {
                    var uri = new Uri(databaseCon.siteimagelink);
                    var container = new CloudBlobContainer(uri);
                    var blob = container.GetBlockBlobReference(dt.Rows[0]["HandOverAudio"].ToString());
                    var exists = blob.Exists();
                    if (exists == true)
                    {
                       var HandOverAudioUrl = databaseCon.siteimagelink + dt.Rows[0]["HandOverAudio"].ToString();
                        GetData.HandOverAudio = HandOverAudioUrl;
                    }
                }
                //HandOver Audio Code Ends Here

                //HandOver Video Code Starts Here
                if (dt.Rows[0]["HandOverVedio"].ToString() != "")
                {
                    var uri = new Uri(databaseCon.siteimagelink);
                    var container = new CloudBlobContainer(uri);
                    var blob = container.GetBlockBlobReference(dt.Rows[0]["HandOverVedio"].ToString());
                    var exists = blob.Exists();
                    if (exists == true)
                    {
                       var HandOverVideoUrl= databaseCon.siteimagelink + dt.Rows[0]["HandOverVedio"].ToString();
                        GetData.HandOverVedio = HandOverVideoUrl;
                    }
                  
                }
                //HandOver Video Code Ends Here


                //HandOver Signature 1 Code Starts Here
                if (dt.Rows[0]["ContractorSign"].ToString() != "")
                {
                    var uri = new Uri(databaseCon.siteimagelink);
                    var container = new CloudBlobContainer(uri);
                    var blob = container.GetBlockBlobReference(dt.Rows[0]["ContractorSign"].ToString());
                    var exists = blob.Exists();
                    if (exists == true)
                    {
                        string ContractorSignUrl = databaseCon.siteimagelink + dt.Rows[0]["ContractorSign"].ToString();
                        GetData.ContractorSign = ContractorSignUrl;
                    }
                   
                }
                //HandOver Signature 1 Code Ends Here

                //HandOver Signature 2 Code Starts Here
                if (dt.Rows[0]["RepresentativeSign"].ToString() != "")
                {
                    var uri = new Uri(databaseCon.siteimagelink);
                    var container = new CloudBlobContainer(uri);
                    var blob = container.GetBlockBlobReference(dt.Rows[0]["RepresentativeSign"].ToString());
                    var exists = blob.Exists();
                    if (exists == true)
                    {
                        string RepresentativeSignUrl = databaseCon.siteimagelink + dt.Rows[0]["RepresentativeSign"].ToString();
                        GetData.RepresentativeSign = RepresentativeSignUrl;
                    }
                  
                }
                //HandOver Signature 2 Code Ends Here


            }

            return Json(GetData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult HandOverUpload(HandOverFormSaveViewModel model)
        {
            var Result = "";
            var Message = "";
            try
            {
                string comment = "";
                //string newhdnlatitude = hdnlatitude.Value;
                //string newhdnlongitude = hdnlongitude.Value;
                //string newhdnAddress = hdnAddress.Value;
                //string sketchData1 = sketch_data.Value;
                //string sketchData2 = sketch_data2.Value;
                //string imageData = tempimg.Value;
                //string AudioData = tempAud.Value;
                //string videoData = tempVideo.Value;
                //string map_imageData = map_image.Value;
                //string PhotoAngle = imgangle.Value;

                string newhdnlatitude = model.hdnlatitude;
                string newhdnlongitude = model.hdnlongitude;
                string newhdnAddress = model.hdnAddress==null ? "" : model.hdnAddress;
                string sketchData1 = model.sketch_data;
                string sketchData2 = model.sketch_data2;
               // string imageData = model.tempimg;
                string AudioData = model.tempAud;
                string videoData = model.tempVideo;
                string map_imageData = model.map_image;
                string PhotoAngle = model.imgangle;

                // DateTime HandOverDate = Convert.ToDateTime(rdpInspectiondate.SelectedDate);
                DateTime HandOverDate = Convert.ToDateTime(model.rdpInspectiondate).AddMinutes(Convert.ToDouble(model.hdnTimeOffSet));
                string HandOverDesc = "";
                string EncapsulateValue = "";
                string ClientRepName = model.txtsiteManager;
                string ClientRepPost = model.ddlsiteManager;

                string OwnerName = model.lblOwnerName==null ? "" : model.lblOwnerName;
                comment = model.comment==null ? "" : model.comment;
                HandOverDesc = model.handovrDesc==null ? "" : model.handovrDesc;
                Boolean Acknowledge = model.confirmcheck;
                EncapsulateValue = model.EncapsulateValue;

                //if (Request.Form["comment"] != "")
                //{
                //    comment = Request.Form["comment"];
                //}
                //else
                //{
                //    comment = "";
                //}

                //if (Request.Form["handovrDesc"] != "")
                //{
                //    HandOverDesc = Request.Form["handovrDesc"];
                //}
                //else
                //{
                //    HandOverDesc = "";
                //}

                //if (confirmcheck.Checked == true)
                //{
                //    Acknowledge = "true";
                //}
                //else
                //{
                //    Acknowledge = "false";
                //}


                //if (Request.Form["checkEncapsulateY"] != "" && Request.Form["checkEncapsulateY"] != null)
                //{
                //    EncapsulateValue = "YES";
                //}
                //else if (Request.Form["checkEncapsulateN"] != "" && Request.Form["checkEncapsulateN"] != null)
                //{
                //    EncapsulateValue = "NO";
                //}
                //else if (Request.Form["checkEncapsulateNA"] != "" && Request.Form["checkEncapsulateNA"] != null)
                //{
                //    EncapsulateValue = "NA";
                //}



                // Query Change On 27 Jan 2020 Add HandoverTimeZone and Save Handover To Utc 

                //  string handOverUpdate = "update tblHandOver set HandOverLat='" + newhdnlatitude + "',HandOverLong='" + newhdnlongitude + "',HandOverAddress='" + newhdnAddress.Replace("'", "''").Trim() + "',HandOverComments='" + comment.Replace("'", "''").Trim() + "',HandOverDate='" + HandOverDate + "',HandOverDesc='" + HandOverDesc.Replace("'", "''").Trim() + "',HasEncapsulated='" + EncapsulateValue + "',HandOverAcknowledgment='" + Acknowledge + "',OwnerName='" + OwnerName + "',RepresentativeName='" + ClientRepName + "',CRId='" + ClientRepPost + "',ContractorRepName='" + ddlContractorRepresentative.Text.Replace("'", "''").Trim() + "' ,ContractorRepLicence='" + lbllicence.Text.Replace("'", "''").Trim() + "', ContractorRepPosition='" + lblrepresentPost.Text.Replace("'", "''").Trim() + "',UpdatedOn='" + DateTime.Now + "',UpdatedBy=(Select EmailID from tblUser where UserId='" + databaseCon.Decrypt(Request.QueryString["UID"]) + "'),PhotoAngle='" + PhotoAngle + "',HandoverTimeZone='" + Convert.ToDouble((hdnTimeOffSet.Value)) * (-1) + "' where  HandOverId='" + databaseCon.Decrypt(Request.QueryString["HandOverId"]) + "'";

                // Query Change On 27 Jan 2020 For Mvc Application 
                string handOverUpdate = "update tblHandOver set HandOverLat='" + newhdnlatitude + "',HandOverLong='" + newhdnlongitude + "',HandOverAddress='" + newhdnAddress.Replace("'", "''").Trim() + "',HandOverComments='" + comment.Replace("'", "''").Trim() + "',HandOverDate='" + HandOverDate + "',HandOverDesc='" + HandOverDesc.Replace("'", "''").Trim() + "',HasEncapsulated='" + EncapsulateValue + "',HandOverAcknowledgment='" + Acknowledge + "',OwnerName='" + OwnerName.Replace("'", "''").Trim() + "',RepresentativeName='" + ClientRepName + "',CRId='" + ClientRepPost + "',ContractorRepName='" + model.ddlContractorRepresentative + "' ,ContractorRepLicence='" + model.lbllicence + "', ContractorRepPosition='" + model.lblrepresentPost + "',UpdatedOn='" + DateTime.Now + "',UpdatedBy=(Select EmailID from tblUser where UserId='" + databaseCon.Decrypt(model.UID) + "'),PhotoAngle='" + PhotoAngle + "',HandoverTimeZone='" + Convert.ToDouble((model.hdnTimeOffSet)) * (-1) + "' where  HandOverId='" + databaseCon.Decrypt(model.HandOverId) + "'";


                int HandOverCount = databaseCon.RunCmd(handOverUpdate);
                if(HandOverCount>0) {
                    var HandOverId = model.HandOverId;

                    if (map_imageData != "" && map_imageData != null)
                    {
                        MapImageUpload(map_imageData, HandOverId);
                    }

                    if (sketchData1 != "" && sketchData1 != null)
                    {
                        SignatureUpload1(sketchData1, HandOverId);
                    }

                    if (sketchData2 != "" && sketchData2 != null)
                    {
                        SignatureUpload2(sketchData2, HandOverId);
                    }

                    //if (imageData != "" && imageData != null)
                    //{
                    //    imageUpload(imageData, HandOverId);
                    //}

                    if (AudioData != "" && AudioData != null)
                    {
                        AudioUpload(AudioData, HandOverId);
                    }

                    if (videoData != "" && videoData != null)
                    {
                        VideoUpload(videoData, HandOverId);
                    }

                    Result = "Success";
                    Message = "Data Save Successfully.";
                }else {
                    Result = "Failed";
                    Message = "Some thing error.";
                }
                

            }catch(Exception ex) {
                Result = "Failed";
                Message =ex.Message;
            }
            // saveContinueModal.Style.Add("display", "Block");
            return Json(new { Result=Result,Message=Message}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UploadImagesSignature(string HandOverId, string tempimg)
        {

            string imageData = tempimg;

            if (imageData != "" && imageData != null)
            {
                imageUpload(imageData, HandOverId);
            }

            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        //Map Image Upload Code Starts Here
        protected void MapImageUpload(string map_imageData, string HandOverId)
        {
            string path = "~/uploads/";
            path = Server.MapPath(path);
            string FileName = "MapHandOver" + "_" + DateTime.Now.Ticks + "_" + databaseCon.Decrypt(HandOverId) + ".jpg";
            string newPath = path + FileName;

            byte[] bytes = Convert.FromBase64String(map_imageData.Split(',')[1]);

            var uri = new Uri(databaseCon.siteimagelink);
            var container = new CloudBlobContainer(uri);
            var blob = container.GetBlockBlobReference(FileName);
            var exists = blob.Exists();
            if (exists == false)
            {
                using (var imageFile = new FileStream(newPath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }


                if (System.IO.File.Exists(newPath))
                {
                    string fpath = path + FileName;
                    databaseCon.savefileonblob(FileName, "Site-images", fpath, "image/jpg");
                    System.IO.File.Delete(fpath);
                }
            }
            else
            {
                databaseCon.deletefileonblob(FileName, "siteimage");

                using (var imageFile = new FileStream(newPath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }


                if (System.IO.File.Exists(newPath))
                {
                    string fpath = path + FileName;
                    databaseCon.savefileonblob(FileName, "Site-images", fpath, "image/jpg");
                    System.IO.File.Delete(fpath);
                }
            }

            string querystr = "update tblHandOver set HandoverMapImage='" + FileName + "' where HandOverId ='" + databaseCon.Decrypt(HandOverId) + "'";
            int count = databaseCon.RunCmd(querystr);
        }
        // Map Image Upload Code Ends Here

        //Contractor Signature Upload Code Starts Here
        protected void SignatureUpload1(string sketchData1,string HandOverId)
        {
            string path = "~/uploads/";
            path = Server.MapPath(path);
            string FileName = "Sketch1HandOver" + "_" + DateTime.Now.Ticks + "_" + databaseCon.Decrypt(HandOverId) + ".png";
            string SketchnewPath = path + FileName;


            byte[] bytes = Convert.FromBase64String(sketchData1.Split(',')[1]);

            var uri = new Uri(databaseCon.siteimagelink);
            var container = new CloudBlobContainer(uri);
            var blob = container.GetBlockBlobReference(FileName);
            var exists = blob.Exists();
            if (exists == false)
            {
                using (var imageFile = new FileStream(SketchnewPath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }

                if (System.IO.File.Exists(SketchnewPath))
                {
                    string fpath = path + FileName;
                    databaseCon.savefileonblob(FileName, "Site-images", fpath, "image/png");
                    System.IO.File.Delete(fpath);
                }
            }
            else
            {
                databaseCon.deletefileonblob(FileName, "siteimage");

                using (var imageFile = new FileStream(SketchnewPath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }

                if (System.IO.File.Exists(SketchnewPath))
                {
                    string fpath = path + FileName;
                    databaseCon.savefileonblob(FileName, "Site-images", fpath, "image/png");
                    System.IO.File.Delete(fpath);
                }
            }


            string querystr = "update tblHandOver set ContractorSign='" + FileName + "' where HandOverId ='" + databaseCon.Decrypt(HandOverId) + "'";
            int count = databaseCon.RunCmd(querystr);
        }
        //Contractor Signature Upload Code Ends Here


        //Representative Signature Upload Code Starts Here
        protected void SignatureUpload2(string sketchData2,string HandOverId)
        {
            string path = "~/uploads/";
            path = Server.MapPath(path);
            string FileName = "Sketch2HandOver" + DateTime.Now.Ticks + databaseCon.Decrypt(HandOverId) + ".png";
            string SketchnewPath = path + FileName;


            byte[] bytes = Convert.FromBase64String(sketchData2.Split(',')[1]);

            var uri = new Uri(databaseCon.siteimagelink);
            var container = new CloudBlobContainer(uri);
            var blob = container.GetBlockBlobReference(FileName);
            var exists = blob.Exists();
            if (exists == false)
            {
                using (var imageFile = new FileStream(SketchnewPath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }

                if (System.IO.File.Exists(SketchnewPath))
                {
                    string fpath = path + FileName;
                    databaseCon.savefileonblob(FileName, "Site-images", fpath, "image/png");
                    System.IO.File.Delete(fpath);
                }
            }
            else
            {
                databaseCon.deletefileonblob(FileName, "siteimage");

                using (var imageFile = new FileStream(SketchnewPath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }

                if (System.IO.File.Exists(SketchnewPath))
                {
                    string fpath = path + FileName;
                    databaseCon.savefileonblob(FileName, "Site-images", fpath, "image/png");
                    System.IO.File.Delete(fpath);
                }
            }


            string querystr = "update tblHandOver set RepresentativeSign='" + FileName + "' where HandOverId ='" + databaseCon.Decrypt(HandOverId) + "'";
            int count = databaseCon.RunCmd(querystr);
        }
        //Representative Signature Upload Code Ends Here

        //Image Upload Code Starts Here
        //protected void imageUpload(string imageData, string HandOverId)
        //{
        //    string path = "~/uploads/";
        //    path = Server.MapPath(path);
        //    string FileName = "SnapHandOver" + "_" + DateTime.Now.Ticks + "_" + databaseCon.Decrypt(HandOverId) + ".jpg";
        //    string newPath = path + FileName;

        //    var tempImage = imageData;
        //    string NewFileName = "";

        //    byte[] bytes = Convert.FromBase64String(imageData.Split(',')[1]);

        //    var uri = new Uri(databaseCon.siteimagelink);
        //    var container = new CloudBlobContainer(uri);
        //    var blob = container.GetBlockBlobReference(FileName);
        //    var exists = blob.Exists();
        //    if (exists == false)
        //    {
        //        using (var imageFile = new FileStream(newPath, FileMode.Create))
        //        {
        //            imageFile.Write(bytes, 0, bytes.Length);
        //            imageFile.Flush();
        //        }
        //        // Added on 01-September-2022  Start here 
        //        Image img = Image.FromFile(newPath);
        //        if (img.PropertyIdList.Contains(0x0112))
        //        {

        //            PropertyItem propOrientation = img.GetPropertyItem(0x0112);
        //            short orientation = BitConverter.ToInt16(propOrientation.Value, 0);
        //            if (orientation == 6)
        //            {
        //                img.RotateFlip(RotateFlipType.Rotate90FlipNone);
        //            }
        //            else if (orientation == 8)
        //            {
        //                img.RotateFlip(RotateFlipType.Rotate270FlipNone);
        //            }
        //            else if (orientation == 3)
        //            {
        //                img.RotateFlip(RotateFlipType.Rotate180FlipNone);
        //            }
        //            else
        //            {
        //                // Do nothing
        //            }
        //            //else if (orientation == 8)
        //            //{
        //            //    img.RotateFlip(RotateFlipType.Rotate270FlipNone);
        //            //}
        //            //else
        //            //{
        //            //    // Do nothing
        //            //}
        //            NewFileName = "SnapHandOver" + "_" + DateTime.Now.Ticks + "_" + databaseCon.Decrypt(HandOverId) + ".jpg";

        //            tempImage = imageData;
        //            //string filename = NewFileName;
        //            var newWidth = (int)(img.Width * 0.5);
        //            var newHeight = (int)(img.Height * 0.5);
        //            var thumbnailImg = new Bitmap(newWidth, newHeight);
        //            var thumbGraph = Graphics.FromImage(thumbnailImg);
        //            thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
        //            thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
        //            thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //            var imageRectangle = new System.Drawing.Rectangle(0, 0, newWidth, newHeight);

        //            thumbGraph.DrawImage(img, imageRectangle);

        //            byte[] bte = GetCompressedBitmap(thumbnailImg, 75);
        //            MemoryStream ms2 = new MemoryStream(bte);
        //            System.Drawing.Image img12 = System.Drawing.Image.FromStream(ms2);

        //            thumbnailImg.Save(path + NewFileName, img12.RawFormat);

        //        }
        //        else
        //        {
        //            NewFileName = "SnapHandOver" + "_" + DateTime.Now.Ticks + "_" + databaseCon.Decrypt(HandOverId) + ".jpg";

        //            tempImage = imageData;
        //            //string filename = NewFileName;
        //            var newWidth = (int)(img.Width * 0.5);
        //            var newHeight = (int)(img.Height * 0.5);
        //            var thumbnailImg = new Bitmap(newWidth, newHeight);
        //            var thumbGraph = Graphics.FromImage(thumbnailImg);
        //            thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
        //            thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
        //            thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //            var imageRectangle = new System.Drawing.Rectangle(0, 0, newWidth, newHeight);

        //            thumbGraph.DrawImage(img, imageRectangle);

        //            byte[] bte = GetCompressedBitmap(thumbnailImg, 50);
        //            MemoryStream ms2 = new MemoryStream(bte);
        //            System.Drawing.Image img12 = System.Drawing.Image.FromStream(ms2);

        //            thumbnailImg.Save(path + NewFileName, img12.RawFormat);
        //        }
        //        // Added on 01-September-2022  End here 


        //        if (System.IO.File.Exists(newPath))
        //        {
        //            string fpath = path + NewFileName;
        //            databaseCon.savefileonblob(NewFileName, "Site-images", fpath, "image/jpeg");
        //            System.IO.File.Delete(fpath);

        //        }
        //    }
        //    else
        //    {
        //        databaseCon.deletefileonblob(FileName, "siteimage");

        //        using (var imageFile = new FileStream(newPath, FileMode.Create))
        //        {
        //            imageFile.Write(bytes, 0, bytes.Length);
        //            imageFile.Flush();
        //        }


        //        if (System.IO.File.Exists(newPath))
        //        {
        //            string fpath = path + FileName;
        //            databaseCon.savefileonblob(FileName, "Site-images", fpath, "image/jpeg");
        //            System.IO.File.Delete(fpath);

        //        }
        //    }

        //    string querystr = "update tblHandOver set HandOverImage='" + NewFileName + "' where HandOverId ='" + databaseCon.Decrypt(HandOverId) + "'";//Added on 01-Sep-2022
        //    //string querystr = "update tblHandOver set HandOverImage='" + FileName + "' where HandOverId ='" + databaseCon.Decrypt(HandOverId) + "'"; //Commented on 01-Sep-2022
        //    int count = databaseCon.RunCmd(querystr);
        //    // Delete original file - now it should work without errors
        //    if (System.IO.File.Exists(newPath))
        //    {
        //        System.IO.File.Delete(newPath);
        //    }
        //}
        //Image Upload Code Ends Here

        //Audio Upload Code Starts Here

        protected void imageUpload(string imageData, string HandOverId)
        {
            string path = "~/uploads/";
            path = Server.MapPath(path);
            string FileName = "SnapHandOver" + "_" + DateTime.Now.Ticks + "_" + databaseCon.Decrypt(HandOverId) + ".jpg";
            string newPath = path + FileName;

            var tempImage = imageData;
            string NewFileName = "";

            try
            {


                byte[] bytes = Convert.FromBase64String(imageData.Split(',')[1]);

                var uri = new Uri(databaseCon.siteimagelink);
                var container = new CloudBlobContainer(uri);
                var blob = container.GetBlockBlobReference(FileName);
                var exists = blob.Exists();
                if (exists == false)
                {
                    using (var imageFile = new FileStream(newPath, FileMode.Create))
                    {
                        imageFile.Write(bytes, 0, bytes.Length);
                        imageFile.Flush();
                    }
                    // Added on 01-September-2022  Start here 
                    using (Image img = Image.FromFile(newPath))
                    {
                        if (img.PropertyIdList.Contains(0x0112))
                        {

                            PropertyItem propOrientation = img.GetPropertyItem(0x0112);
                            short orientation = BitConverter.ToInt16(propOrientation.Value, 0);
                            if (orientation == 6)
                            {
                                img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            }
                            else if (orientation == 8)
                            {
                                img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            }
                            else if (orientation == 3)
                            {
                                img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            }
                            else
                            {
                                // Do nothing
                            }
                            //else if (orientation == 8)
                            //{
                            //    img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            //}
                            //else
                            //{
                            //    // Do nothing
                            //}
                            NewFileName = "SnapHandOver" + "_" + DateTime.Now.Ticks + "_" + databaseCon.Decrypt(HandOverId) + ".jpg";

                            tempImage = imageData;
                            //string filename = NewFileName;
                            var newWidth = (int)(img.Width * 0.5);
                            var newHeight = (int)(img.Height * 0.5);
                            var thumbnailImg = new Bitmap(newWidth, newHeight);
                            var thumbGraph = Graphics.FromImage(thumbnailImg);
                            thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                            thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                            thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            var imageRectangle = new System.Drawing.Rectangle(0, 0, newWidth, newHeight);

                            thumbGraph.DrawImage(img, imageRectangle);

                            byte[] bte = GetCompressedBitmap(thumbnailImg, 75);
                            using (MemoryStream ms2 = new MemoryStream(bte))
                            {
                                using (System.Drawing.Image img12 = System.Drawing.Image.FromStream(ms2))
                                {
                                    thumbnailImg.Save(path + NewFileName, img12.RawFormat);

                                }

                            }


                        }
                        else
                        {
                            NewFileName = "SnapHandOver" + "_" + DateTime.Now.Ticks + "_" + databaseCon.Decrypt(HandOverId) + ".jpg";

                            tempImage = imageData;
                            //string filename = NewFileName;
                            var newWidth = (int)(img.Width * 0.5);
                            var newHeight = (int)(img.Height * 0.5);
                            var thumbnailImg = new Bitmap(newWidth, newHeight);
                            var thumbGraph = Graphics.FromImage(thumbnailImg);
                            thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                            thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                            thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            var imageRectangle = new System.Drawing.Rectangle(0, 0, newWidth, newHeight);

                            thumbGraph.DrawImage(img, imageRectangle);

                            byte[] bte = GetCompressedBitmap(thumbnailImg, 50);
                            using (MemoryStream ms2 = new MemoryStream(bte))
                            {
                                using (System.Drawing.Image img12 = System.Drawing.Image.FromStream(ms2))
                                {
                                    thumbnailImg.Save(path + NewFileName, img12.RawFormat);

                                }

                            }

                        }
                        // Added on 01-September-2022  End here 
                        if (System.IO.File.Exists(newPath))
                        {
                            string fpath = path + NewFileName;
                            databaseCon.savefileonblob(NewFileName, "Site-images", fpath, "image/jpeg");
                            System.IO.File.Delete(fpath);

                        }

                    }


                }
                else
                {
                    databaseCon.deletefileonblob(FileName, "siteimage");

                    using (var imageFile = new FileStream(newPath, FileMode.Create))
                    {
                        imageFile.Write(bytes, 0, bytes.Length);
                        imageFile.Flush();
                    }


                    if (System.IO.File.Exists(newPath))
                    {
                        string fpath = path + FileName;
                        databaseCon.savefileonblob(FileName, "Site-images", fpath, "image/jpeg");
                        System.IO.File.Delete(fpath);
                        System.IO.File.Delete(newPath);
                    }
                }

                string querystr = "update tblHandOver set HandOverImage='" + NewFileName + "' where HandOverId ='" + databaseCon.Decrypt(HandOverId) + "'";
                //Added on 01-Sep-2022
             //string querystr = "update tblHandOver set HandOverImage='" + FileName + "' where HandOverId ='" + databaseCon.Decrypt(HandOverId) + "'";//Commented on 01-Sep-2022
                int count = databaseCon.RunCmd(querystr);
            }
            catch (Exception ex) { }
            finally
            {
                if (System.IO.File.Exists(newPath))
                {
                    System.IO.File.Delete(newPath);
                }
            }
        }
        //Image Upload Code Ends Here

        //Audio Upload Code Starts Here

        protected void AudioUpload(string AudioData,string HandOverId)
        {
            string path = "~/uploads/";
            path = Server.MapPath(path);
            string FileName = "AudioHandOver" + "_" + DateTime.Now.Ticks + "_" + databaseCon.Decrypt(HandOverId) + ".mp3";
            string AudionewPath = path + FileName;

            byte[] dummyAudioData = Convert.FromBase64String(AudioData.Split(',')[1]);

            var uri = new Uri(databaseCon.siteimagelink);
            var container = new CloudBlobContainer(uri);
            var blob = container.GetBlockBlobReference(FileName);
            var exists = blob.Exists();
            if (exists == false)
            {
                using (var AudioFile = new FileStream(AudionewPath, FileMode.Create))
                {
                    AudioFile.Write(dummyAudioData, 0, dummyAudioData.Length);
                    AudioFile.Flush();
                }

                if (System.IO.File.Exists(AudionewPath))
                {
                    string fpath = path + FileName;
                    databaseCon.savefileonblob(FileName, "Site-images", fpath, "audio/mp3");
                    System.IO.File.Delete(fpath);
                }
            }
            else
            {
                databaseCon.deletefileonblob(FileName, "siteimage");
                using (var AudioFile = new FileStream(AudionewPath, FileMode.Create))
                {
                    AudioFile.Write(dummyAudioData, 0, dummyAudioData.Length);
                    AudioFile.Flush();
                }

                if (System.IO.File.Exists(AudionewPath))
                {
                    string fpath = path + FileName;
                    databaseCon.savefileonblob(FileName, "Site-images", fpath, "audio/mp3");
                    System.IO.File.Delete(fpath);
                }
            }

            string querystr = "update tblHandOver set HandOverAudio='" + FileName + "' where HandOverId ='" + databaseCon.Decrypt(HandOverId) + "'";
            int count = databaseCon.RunCmd(querystr);
        }
        //Audio Upload Code Ends Here

        //Video Upload Code Starts Here
        protected void VideoUpload(string videoData,string HandOverId)
        {
            string path = "~/uploads/";
            path = Server.MapPath(path);
            string FileName = "VideoHandOver" + "_" + DateTime.Now.Ticks + "_" + databaseCon.Decrypt(HandOverId) + ".mov";
            string VideonewPath = path + FileName;

            byte[] dummyVideoData = Convert.FromBase64String(videoData.Split(',')[1]);

            var uri = new Uri(databaseCon.siteimagelink);
            var container = new CloudBlobContainer(uri);
            var blob = container.GetBlockBlobReference(FileName);
            var exists = blob.Exists();
            if (exists == false)
            {
                using (var VideoFile = new FileStream(VideonewPath, FileMode.Create))
                {
                    VideoFile.Write(dummyVideoData, 0, dummyVideoData.Length);
                    VideoFile.Flush();
                }

                if (System.IO.File.Exists(VideonewPath))
                {
                    string fpath = path + FileName;
                    databaseCon.savefileonblob(FileName, "Site-images", fpath, "video/mov");
                    System.IO.File.Delete(fpath);
                }
            }
            else
            {
                databaseCon.deletefileonblob(FileName, "siteimage");
                using (var VideoFile = new FileStream(VideonewPath, FileMode.Create))
                {
                    VideoFile.Write(dummyVideoData, 0, dummyVideoData.Length);
                    VideoFile.Flush();
                }

                if (System.IO.File.Exists(VideonewPath))
                {
                    string fpath = path + FileName;
                    databaseCon.savefileonblob(FileName, "Site-images", fpath, "video/mov");
                    System.IO.File.Delete(fpath);
                }
            }

            string querystr = "update tblHandOver set HandOverVedio='" + FileName + "' where HandOverId ='" + databaseCon.Decrypt(HandOverId) + "'";
            int count = databaseCon.RunCmd(querystr);
        }
        //Video Upload Code Ends Here


        public ActionResult btnConfirm_ClickOk(HandOverFormSaveViewModel model)
        {
            var Result = "";
            var Message = "";
            var EngineeredDesign = "";
            //if (Request.Form["checkBoxDesignY"] != "" && Request.Form["checkBoxDesignY"] != null)
            //{
            //    EngineeredDesign = "YES";
            //}
            //else if (Request.Form["checkBoxDesignN"] != "" && Request.Form["checkBoxDesignN"] != null)
            //{
            //    EngineeredDesign = "NO";
            //}
            //else if (Request.Form["checkBoxDesignNA"] != "" && Request.Form["checkBoxDesignNA"] != null)
            //{
            //    EngineeredDesign = "NA";
            //}


            //  string confirmQuery = "update tblHandOver set IsConfirmed = 1 where InspId='" + databaseCon.Decrypt(Request.QueryString["InspId"]) + "' and HandOverId='" + databaseCon.Decrypt(Request.QueryString["HandOverId"]) + "'";

            //  string confirmQuery = "update tblHandOver set IsConfirmed = 1,SiteOwnerFromJob='" + lblOwnerName.Text + "', siteLocationFromJob='" + sitelbl.Text + "', BuildingNoFromJob='" + BuildingNolbl.Text + "',FloorLevelFromJob='" + lblfloor.Text + "', ReasonForAccessFromJob='" + lblReasonforAccess.Text + "', DutyRatingFromJob='" + lblDutyRating.Text + "', NoOfworkingDecksFromJob='" + lblWrokingDeck.Text + "', MaximumDecksFromJob='" + lblNumberOfPlatform.Text + "',DesignDrawingNoFromJob='" + lbldesigndNo.InnerText + "',LengthFromJob='" + lblLength.Text + "',WidthFromJob='" + lblWidth.Text + "',HeightFromJob='" + lblHeight.Text + "',ScaffrefNoFromJob='"+ scaffreflbl.Text + "',EngineeredDesignFromJob='" + EngineeredDesign + "',ConfirmedDate='"+DateTime.Now+"' where InspId='" + databaseCon.Decrypt(Request.QueryString["InspId"]) + "' and HandOverId='" + databaseCon.Decrypt(Request.QueryString["HandOverId"]) + "'";

            //string confirmQuery = "update tblHandOver set IsConfirmed = 1,SiteOwnerFromJob='" + lblOwnerName.Text.Replace("'", "''").Trim() + "', siteLocationFromJob='" + sitelbl.Text.Replace("'", "''").Trim() + "', BuildingNoFromJob='" + BuildingNolbl.Text.Replace("'", "''").Trim() + "',FloorLevelFromJob='" + lblfloor.Text.Replace("'", "''").Trim() + "', ReasonForAccessFromJob='" + lblReasonforAccess.Text.Replace("'", "''").Trim() + "', DutyRatingFromJob='" + lblDutyRating.Text.Replace("'", "''").Trim() + "', NoOfworkingDecksFromJob='" + lblWrokingDeck.Text.Replace("'", "''").Trim() + "', MaximumDecksFromJob='" + lblNumberOfPlatform.Text.Replace("'", "''").Trim() + "',DesignDrawingNoFromJob='" + lbldesigndNo.InnerText.Replace("'", "''").Trim() + "',LengthFromJob='" + lblLength.Text.Replace("'", "''").Trim() + "',WidthFromJob='" + lblWidth.Text.Replace("'", "''").Trim() + "',HeightFromJob='" + lblHeight.Text.Replace("'", "''").Trim() + "',ScaffrefNoFromJob='" + scaffreflbl.Text.Replace("'", "''").Trim() + "',EngineeredDesignFromJob='" + EngineeredDesign.Replace("'", "''").Trim() + "' ,ConfirmedDate='" + DateTime.Now + "' where  HandOverId='" + databaseCon.Decrypt(Request.QueryString["HandOverId"]) + "'";

            // Query Change On 27 Jan 2020 Add HandoverConfirmTimeZone and Save ConfirmedDate To Utc 
            // string confirmQuery = "update tblHandOver set IsConfirmed = 1,SiteOwnerFromJob='" + lblOwnerName.Text.Replace("'", "''").Trim() + "', siteLocationFromJob='" + sitelbl.Text.Replace("'", "''").Trim() + "', BuildingNoFromJob='" + BuildingNolbl.Text.Replace("'", "''").Trim() + "',FloorLevelFromJob='" + lblfloor.Text.Replace("'", "''").Trim() + "', ReasonForAccessFromJob='" + lblReasonforAccess.Text.Replace("'", "''").Trim() + "', DutyRatingFromJob='" + lblDutyRating.Text.Replace("'", "''").Trim() + "', NoOfworkingDecksFromJob='" + lblWrokingDeck.Text.Replace("'", "''").Trim() + "', MaximumDecksFromJob='" + lblNumberOfPlatform.Text.Replace("'", "''").Trim() + "',DesignDrawingNoFromJob='" + lbldesigndNo.InnerText.Replace("'", "''").Trim() + "',LengthFromJob='" + lblLength.Text.Replace("'", "''").Trim() + "',WidthFromJob='" + lblWidth.Text.Replace("'", "''").Trim() + "',HeightFromJob='" + lblHeight.Text.Replace("'", "''").Trim() + "',ScaffrefNoFromJob='" + scaffreflbl.Text.Replace("'", "''").Trim() + "',EngineeredDesignFromJob='" + EngineeredDesign.Replace("'", "''").Trim() + "' ,ConfirmedDate='" + DateTime.Now + "',HandoverConfirmTimeZone='" + Convert.ToDouble((hdnTimeOffSet.Value)) * (-1) + "' where  HandOverId='" + databaseCon.Decrypt(Request.QueryString["HandOverId"]) + "'";

            var lblOwnerName = model.lblOwnerName == null ? "" : model.lblOwnerName;
            var sitelbl = model.sitelbl == null ? "" : model.sitelbl;
            var BuildingNolbl = model.BuildingNolbl == null ? "" : model.BuildingNolbl;
            var lblfloor = model.lblfloor == null ? "" : model.lblfloor;
            var lblReasonforAccess = model.lblReasonforAccess == null ? "" : model.lblReasonforAccess;
            var lblDutyRating = model.lblDutyRating == null ? "" : model.lblDutyRating;
            var lblWrokingDeck = model.lblWrokingDeck == null ? "" : model.lblWrokingDeck;
            var lblNumberOfPlatform = model.lblNumberOfPlatform == null ? "" : model.lblNumberOfPlatform;
            var lbldesigndNo = model.lbldesigndNo == null ? "" : model.lbldesigndNo;

            string confirmQuery = "update tblHandOver set IsConfirmed = 1,SiteOwnerFromJob='" + lblOwnerName.Replace("'", "''").Trim() + "', siteLocationFromJob='" + sitelbl.Replace("'", "''").Trim() + "', BuildingNoFromJob='" + BuildingNolbl.Replace("'", "''").Trim() + "',FloorLevelFromJob='" + lblfloor.Replace("'", "''").Trim() + "', ReasonForAccessFromJob='" + lblReasonforAccess.Replace("'", "''").Trim() + "', DutyRatingFromJob='" + lblDutyRating.Replace("'", "''").Trim() + "', NoOfworkingDecksFromJob='" + lblWrokingDeck.Replace("'", "''").Trim() + "', MaximumDecksFromJob='" + lblNumberOfPlatform.Replace("'", "''").Trim() + "',DesignDrawingNoFromJob='" + lbldesigndNo.Replace("'", "''").Trim() + "',LengthFromJob='" + model.lblLength + "',WidthFromJob='" + model.lblWidth + "',HeightFromJob='" + model.lblHeight + "',ScaffrefNoFromJob='" + model.scaffreflbl + "',EngineeredDesignFromJob='" + EngineeredDesign + "' ,ConfirmedDate='" + DateTime.Now + "',HandoverConfirmTimeZone='" + Convert.ToDouble((model.hdnTimeOffSet)) * (-1) + "' where  HandOverId='" + databaseCon.Decrypt(model.HandOverId) + "'";

            int confirmCount = databaseCon.RunCmd(confirmQuery);
            if (confirmCount > 0)
            {
                Result = "Success";
                Message = "Data Save Successfully.";
            
            }
            else {
                Result = "Failed";
                Message = "Something went wrong Please try again.";
            }
            return Json(new {Result=Result,Message=Message },JsonRequestBehavior.AllowGet);
        }
       public ActionResult ddlContractorRepresentative_SelectedIndexChanged(string ddlContractorRepresentative)
        {
            DataTable dt;
            var lbllicence = "";
            var lblrepresentPost = "";
            string Query = "select LicenceNo,Role from tblUser where UserId='" + ddlContractorRepresentative + "'";
            dt = databaseCon.GetData(Query);

            if (dt.Rows.Count > 0)
            {
                lbllicence = dt.Rows[0]["LicenceNo"].ToString();
                lblrepresentPost= dt.Rows[0]["Role"].ToString();

            }
            var GetData = lbllicence + "_" + lblrepresentPost;
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
    }
}