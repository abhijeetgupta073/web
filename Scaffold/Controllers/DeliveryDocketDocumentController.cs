using Scaffold.App_Code;
using Scaffold_DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scaffold.Models;
using System.Data;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Scaffold.Controllers
{
    public class DeliveryDocketDocumentController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: DeliveryDocketDocument
        public ActionResult DeliveryDocketDocument(string TranId)
        {
            ViewBag.transId = TranId;

            var transid = Request.QueryString["TranId"];
            var ReturnInv = Request.QueryString["RInvId"];
            string query = "";
            if (Request.QueryString["TranId"] != null)
            {
                if (databaseCon.Decrypt(Request.QueryString["ThirdParty"]) == "true")
                {
                    query = "select ToTime,'Depot' as [From],'ThirdParty' as [To] from tblTransaction where TransId= '" + databaseCon.Decrypt(Request.QueryString["TranId"]) + "'";
                    DataTable dt = databaseCon.GetData(query);
                    if (dt.Rows.Count != 0)
                    {
                        if (dt.Rows[0]["ToTime"] != DBNull.Value)
                        {
                            ViewBag.lblclient = "Delivery from <strong>" + dt.Rows[0]["To"] + "</strong> to <strong>" + dt.Rows[0]["From"] + "</strong> on Delivery date <strong>" + Convert.ToDateTime(dt.Rows[0]["ToTime"]).ToString("dd-MMM-yyyy") + "</strong>";
                        }
                        else
                        {
                            ViewBag.lblclient = "Delivery from <strong>" + dt.Rows[0]["To"] + "</strong> to <strong>" + dt.Rows[0]["From"] + "</strong> on Delivery date ";
                        }
                    }
                }
                else
                {
                    query = "select ToTime, (select SiteLocation from tblSiteLocation where SiteLocId=FromId) as [From],'Depot' as [To] from tblTransaction where TransId= '" + databaseCon.Decrypt(Request.QueryString["TranId"]) + "'";
                    DataTable dt = databaseCon.GetData(query);
                    if (dt.Rows.Count != 0)
                    {
                        if (dt.Rows[0]["ToTime"] != DBNull.Value)
                        {
                            ViewBag.lblclient = "Delivery from <strong>" + dt.Rows[0]["To"] + "</strong> to <strong>" + dt.Rows[0]["From"] + "</strong> on Delivery date <strong>" + Convert.ToDateTime(dt.Rows[0]["ToTime"]).ToString("dd-MMM-yyyy") + "</strong>";
                        }
                        else
                        {
                            ViewBag.lblclient = "Delivery from <strong>" + dt.Rows[0]["To"] + "</strong> to <strong>" + dt.Rows[0]["From"] + "</strong> on Delivery date ";
                        }
                    }

                }
               
            }
            else if (Request.QueryString["RInvId"] != null)
            {
                if (Request.QueryString["Depot"] != null)
                {
                    query = "select [From] as FromName,[To] as ToName,ReturnInvId,CONVERT(varchar(11),Convert(Datetime,ReturnDoneOn),106) as ReturnDoneOn from tblReturnInventory  where [From]='Site'and tblReturnInventory.ClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and ReturnInvId='" + databaseCon.Decrypt(Request.QueryString["RInvId"]) + "'";
                    DataTable dt = databaseCon.GetData(query);
                    if (dt.Rows.Count != 0)
                    {
                        ViewBag.lblclient = "Delivery from <strong>" + dt.Rows[0]["FromName"] + "</strong> to <strong>" + dt.Rows[0]["ToName"] + "</strong> on Delivery date <strong>" + Convert.ToDateTime(dt.Rows[0]["ReturnDoneOn"]).ToString("dd-MMM-yyyy") + "</strong>";
                    }
                }
                else if (Request.QueryString["Site"] != null)
                {
                    //query = "select SiteLocation as FromName,[To] as ToName,ReturnInvId,CONVERT(varchar(11),ReturnDoneOn,106) as ReturnDoneOn from tblReturnInventory join tblSiteLocation on tblReturnInventory.FromId=tblSiteLocation.SiteLocID where [From]='Site'and tblReturnInventory.ClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and ReturnInvId='" + databaseCon.Decrypt(Request.QueryString["RInvId"]) + "'";
                    //change ReturnDoneon column format on 13-Oct-2020
                    query = "select SiteLocation as FromName,[To] as ToName,ReturnInvId,CONVERT(varchar(11),Convert(Datetime,ReturnDoneOn),106) as ReturnDoneOn from tblReturnInventory join tblSiteLocation on tblReturnInventory.FromId=tblSiteLocation.SiteLocID where [From]='Site'and tblReturnInventory.ClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and ReturnInvId='" + databaseCon.Decrypt(Request.QueryString["RInvId"]) + "'";
                    DataTable dt = databaseCon.GetData(query);
                    if (dt.Rows.Count != 0)
                    {

                       ViewBag.lblclient = "Delivery from <strong>" + dt.Rows[0]["FromName"] + "</strong> to <strong>" + dt.Rows[0]["ToName"] + "</strong> on Delivery date <strong>" + Convert.ToDateTime(dt.Rows[0]["ReturnDoneOn"]).ToString("dd-MMM-yyyy") + "</strong>";
                    }
                }
                else if (Request.QueryString["Job"] != null)
                {
                    //query = "select case when Description is not null and Description!=''then Right('000000'+Convert(varchar,ScaffRef),6)+' - '+Description else Right('000000'+Convert(varchar,ScaffRef),6) end  as FromName,SiteLocation as ToName,CONVERT(varchar(11),ReturnDoneOn,106) as ReturnDoneOn from tblReturnInventory join tblSiteLocation on tblReturnInventory.ToId = tblSiteLocation.SiteLocID  join tblRegister on tblReturnInventory.FromId = tblRegister.RegisterId where[To] = 'Site'and tblReturnInventory.ClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and ReturnInvId='" + databaseCon.Decrypt(Request.QueryString["RInvId"]) + "'";
                    //change ReturnDoneon column format on 13-Oct-2020
                    query = "select case when Description is not null and Description!=''then Right('000000'+Convert(varchar,ScaffRef),6)+' - '+Description else Right('000000'+Convert(varchar,ScaffRef),6) end  as FromName,SiteLocation as ToName,CONVERT(varchar(11),Convert(Datetime,ReturnDoneOn),106) as ReturnDoneOn from tblReturnInventory join tblSiteLocation on tblReturnInventory.ToId = tblSiteLocation.SiteLocID  join tblRegister on tblReturnInventory.FromId = tblRegister.RegisterId where[To] = 'Site'and tblReturnInventory.ClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and ReturnInvId='" + databaseCon.Decrypt(Request.QueryString["RInvId"]) + "'";
                    DataTable dt = databaseCon.GetData(query);
                    if (dt.Rows.Count != 0)
                    {

                        ViewBag.lblclient= "Delivery from <strong>" + dt.Rows[0]["FromName"] + "</strong> to <strong>" + dt.Rows[0]["ToName"] + "</strong> on Delivery date <strong>" + Convert.ToDateTime(dt.Rows[0]["ReturnDoneOn"]).ToString("dd-MMM-yyyy") + "</strong>";

                    }
                }

            }

            return View();
        }

        public ActionResult GetdeliveryDocketDoc(string id, string transId, string RInvId)
        {
            if (databaseCon.Decrypt(transId) != null && databaseCon.Decrypt(transId) != "")
            {
                var getData = dbContext.Database.SqlQuery<DeliveryDocketDocumentViewModel>("select DDDocumentId,DocumentName,Description,DateOfUpload,TransID from tblDeliveryDocketDocuments where clientid ='" + databaseCon.Decrypt(id) + "' and TransID='" + databaseCon.Decrypt(transId) + "' and [Type]='Delivery Docket Document' order by DocumentName").ToList();
                return Json(getData, JsonRequestBehavior.AllowGet);
            } else
            {
                var getData = dbContext.Database.SqlQuery<DeliveryDocketDocumentViewModel>("select DDDocumentId,DocumentName,Description,DateOfUpload,TransID from tblDeliveryDocketDocuments where clientid = '" + databaseCon.Decrypt(id) + "' and TransID='" + databaseCon.Decrypt(RInvId) + "' and [Type]='Return Inventory Document' order by DocumentName").ToList();
                return Json(getData, JsonRequestBehavior.AllowGet);
            }

          

            // //var GetData = dbContext.Database.SqlQuery<>("");
            // var GetData = "1";
            // string query = "";
            // if (databaseCon.Decrypt(transId) != null || databaseCon.Decrypt(transId) != "")
            // {
            //     query = "select DDDocumentId,DocumentName,Description,DateOfUpload,TransID from tblDeliveryDocketDocuments where clientid ='" + databaseCon.Decrypt(id) + "' and TransID='" + databaseCon.Decrypt(transId) + "' and [Type]='Delivery Docket Document' order by DocumentName";
            // }
            //// else if (Request.QueryString["RInvId"] != null)
            // else    if (databaseCon.Decrypt(RInvId) != null || databaseCon.Decrypt(RInvId) != "")
            //     {
            //   //  query = "select DDDocumentId,DocumentName,Description,DateOfUpload,TransID from tblDeliveryDocketDocuments where clientid = " + databaseCon.Decrypt(Request.QueryString["id"]) + " and TransID='" + databaseCon.Decrypt(Request.QueryString["RInvId"]) + "' and [Type]='Return Inventory Document' order by DocumentName";
            //     query = "select DDDocumentId,DocumentName,Description,DateOfUpload,TransID from tblDeliveryDocketDocuments where clientid = '" + databaseCon.Decrypt(id) + "' and TransID='" + databaseCon.Decrypt(RInvId) + "' and [Type]='Return Inventory Document' order by DocumentName";
            // }

            ////  var GetData = dbContext.Database.SqlQuery<DeliveryDocketDocumentViewModel>(query);
            // //DataTable dt = databaseCon.GetData(query);
            // //if (dt.Rows.Count < 1)
            // //{
            // //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "hide", "<script>filterhide();</script>", false);
            // //}
            // //docsgrid.DataSource = dt;

            // return Json(GetData, JsonRequestBehavior.AllowGet);
        }


        public ActionResult DeliveryLableData()
        {

            string query = "";
            if (Request.QueryString["TranId"] != null)
            {
                query = "select ToTime, (select SiteLocation from tblSiteLocation where SiteLocId=FromId) as [From],'Depot' as [To] from tblTransaction where TransId= '" + databaseCon.Decrypt(Request.QueryString["TranId"]) + "'";
                DataTable dt = databaseCon.GetData(query);
                if (dt.Rows.Count != 0)
                {
                    if (dt.Rows[0]["ToTime"] != DBNull.Value)
                    {
                        //lblclient.Text = "Delivery from <strong>" + dt.Rows[0]["To"] + "</strong> to <strong>" + dt.Rows[0]["From"] + "</strong> on Delivery date <strong>" + Convert.ToDateTime(dt.Rows[0]["ToTime"]).ToString("dd-MMM-yyyy") + "</strong>";
                    }
                    else
                    {
                       // lblclient.Text = "Delivery from <strong>" + dt.Rows[0]["To"] + "</strong> to <strong>" + dt.Rows[0]["From"] + "</strong> on Delivery date ";
                    }
                }
            }
            else if (Request.QueryString["RInvId"] != null)
            {
                if (Request.QueryString["Site"] != null)
                {
                    query = "select SiteLocation as FromName,[To] as ToName,ReturnInvId,CONVERT(varchar(11),ReturnDoneOn,106) as ReturnDoneOn from tblReturnInventory join tblSiteLocation on tblReturnInventory.FromId=tblSiteLocation.SiteLocID where [From]='Site'and tblReturnInventory.ClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and ReturnInvId='" + databaseCon.Decrypt(Request.QueryString["RInvId"]) + "'";
                    DataTable dt = databaseCon.GetData(query);
                    if (dt.Rows.Count != 0)
                    {

                      //  lblclient.Text = "Delivery from <strong>" + dt.Rows[0]["FromName"] + "</strong> to <strong>" + dt.Rows[0]["ToName"] + "</strong> on Delivery date <strong>" + Convert.ToDateTime(dt.Rows[0]["ReturnDoneOn"]).ToString("dd-MMM-yyyy") + "</strong>";

                    }
                }
                else if (Request.QueryString["Job"] != null)
                {
                    query = "select case when Description is not null and Description!=''then Right('000000'+Convert(varchar,ScaffRef),6)+' - '+Description else Right('000000'+Convert(varchar,ScaffRef),6) end  as FromName,SiteLocation as ToName,CONVERT(varchar(11),ReturnDoneOn,106) as ReturnDoneOn from tblReturnInventory join tblSiteLocation on tblReturnInventory.ToId = tblSiteLocation.SiteLocID  join tblRegister on tblReturnInventory.FromId = tblRegister.RegisterId where[To] = 'Site'and tblReturnInventory.ClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and ReturnInvId='" + databaseCon.Decrypt(Request.QueryString["RInvId"]) + "'";
                    DataTable dt = databaseCon.GetData(query);
                    if (dt.Rows.Count != 0)
                    {

                       // lblclient.Text = "Delivery from <strong>" + dt.Rows[0]["FromName"] + "</strong> to <strong>" + dt.Rows[0]["ToName"] + "</strong> on Delivery date <strong>" + Convert.ToDateTime(dt.Rows[0]["ReturnDoneOn"]).ToString("dd-MMM-yyyy") + "</strong>";

                    }
                }
            }
            return Json("res", JsonRequestBehavior.AllowGet);
            }


        public int DeleteDocument(string id, string DocumentId) {
            //confmodal.Style.Add("display", "none");

            int Result = 0;
            string query1 = "select DDDocumentId,DocumentName,Description,DateOfUpload from tblDeliveryDocketDocuments where DDDocumentId='" + DocumentId + "'";
            DataTable dt1 = databaseCon.GetData(query1);
            if (dt1.Rows.Count != 0)
            {
                var DocumentName = dt1.Rows[0]["DocumentName"].ToString();

                string query = "delete from  tblDeliveryDocketDocuments where DDDocumentId ='" + DocumentId + "'";
                int i = databaseCon.RunCmd(query);
                if (i > 0)
                {
                    //art1.Style.Add("display", "block");
                    //art1.Attributes["class"] = "w3-section w3-card-12 j-close fade in  j-alertBox-Success";
                    //alertlabel.Text = "<strong>Success ! </strong>Record Deleted Successfully.";
                    //docsgrid.Rebind();
                    // string filename = ViewState["docname"].ToString();
                    databaseCon.deletefileonblob(DocumentName, "siteimage");
                    Result = 1;
                }
            }


            return Result;
        }

        public ActionResult EditDeliveryDocketDoc(string id)
        {
            // var getMaterialType = dbContext.Database.SqlQuery<InventoryViewModel>("select distinct MaterialType from tblInventory  where clientid = " + id + " order by MaterialType").ToList();
           // var uid = databaseCon.Decrypt(id);

            var getData = dbContext.Database.SqlQuery<DeliveryDocketDocumentViewModel>("select DDDocumentId,Description,TransID from tblDeliveryDocketDocuments where DDDocumentId ='" + databaseCon.Decrypt(id));
            return Json(getData, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ViewDocument(string id, string DocumentId)
        {
            // ViewState["id"] = gdi.GetDataKeyValue("DDDocumentId").ToString();
            string query = "select DDDocumentId,DocumentName,Description,DateOfUpload from tblDeliveryDocketDocuments where DDDocumentId='" + DocumentId + "'";
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count != 0)
            {
                string path = databaseCon.siteimagelink + dt.Rows[0]["DocumentName"];
                var uri = new Uri(databaseCon.siteimagelink);
                var container = new CloudBlobContainer(uri);
                var blob = container.GetBlockBlobReference(dt.Rows[0]["DocumentName"].ToString());
                var exists = blob.Exists();
                if (exists == false)
                {
                    //art1.Style.Add("display", "block");
                    //art1.Attributes["class"] = "w3-section w3-card-12 j-close fade in  j-alertBox-Failed";
                    //alertlabel.Text = "<strong>Alert ! </strong> File does not exists.";
                    return Json(new { result = "Failed", Message = " File does not exists." });
                }
                else
                {
                    return Json(new { result = "Success", Message = path });
                   // Response.Write("<script> window.open('" + path + "','_blank'); </script>");
                }

            }
            return Json(new { result = "Failed", Message = " File does not exists." });
        }

        public ActionResult btnEditSave(string id, string UID, string hdnDDDocumentID, string txtdescription, string hdnDocumentName)
        {
            var Result = "";
            if (txtdescription != "")
            {
                string query = "update tblDeliveryDocketDocuments set Description = '" + txtdescription.Replace("'", "''").Trim() + "',UpdatedOn='" + DateTime.Now + "',UpdatedBy=(select Emailid from tbluser where UserId='" + databaseCon.Decrypt(UID) + "')  where  DocumentName = '" + hdnDocumentName + "' and DDDocumentId='" + hdnDDDocumentID + "'";
                int i = databaseCon.RunCmd(query);
                if (i > 0)
                {
                    Result = "Success";
                }
                else
                {
                    Result = "Failed";
                }
            }

            return Json(Result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult btnsave_Click(string UID, string ClientID, string TransId,string txtdesc,string RInvId, string UserRole, string image,string fileUpload,string Site,string Job)
        {
            string Result = "";
            string Message = "";
            //if (Request.Files.Count > 0)
            //{
            //    try
            //    {
            //        //  Get all files from Request object  
            //        HttpFileCollectionBase files = Request.Files;
            //        for (int k = 0; k < 1; k++)
            //        {

            //            HttpPostedFileBase file = files[k];
            //            string filename;
            //            string filePath;

            //            // Checking for Internet Explorer  
            //            if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
            //            {
            //                string[] testfiles = file.FileName.Split(new char[] { '\\' });
            //                filename = testfiles[testfiles.Length - 1];
            //            }
            //            else
            //            {
            //                filename = file.FileName;
            //            }

            //            var UserFileName = filename;
            //            // loginUserId = DataBaseCon.Decrypt(loginUserId);
            //            // Get the complete folder path and store the file inside it.  
            //            filePath = Path.Combine(Server.MapPath("~/Files/"), filename + ".xlsx");
            //            file.SaveAs(filePath);

            //            var FileExtension = System.IO.Path.GetExtension(filename).ToLower();
            //        }
            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //}
            ////  return Json("res", JsonRequestBehavior.AllowGet);


            //Adddocs.Style.Add("display", "none");
            bool capturing = false;
            bool uploading = false;
            int count = 0;
           
            // string imageData = tempimg.Value;
            //string name = nameget.Value;

            // var FileExtension = System.IO.Path.GetExtension(filename).ToLower();
            //  if (fileuplod.HasFile == true)
            if (Request.Files.Count > 0)
            {
                //  if (FileExtension != "")
                //  {
                //  foreach (HttpPostedFile postedFile in fileuplod.PostedFiles)
                //  {
                HttpFileCollectionBase files = Request.Files;
                for (int k = 0; k < 1; k++)
                {

                    HttpPostedFileBase file = files[k];
                    string filename;
                    string filePath;


                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        filename = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        filename = file.FileName;
                    }

                    var UserFileName = filename;
                    var FileExtension = System.IO.Path.GetExtension(filename).ToLower();
                    var extension = System.IO.Path.GetExtension(filename).ToLower();
                    //FileExtension = postedFile.FileName;
                    var ext = FileExtension.Split('.');
                   // string extension = Path.GetExtension(fileuplod.PostedFile.FileName);
                    extension = ext[1];
                    if (extension.ToLower() == "jpg" || extension.ToLower() == "jpeg" || extension.ToLower() == "png" || extension.ToLower() == "ico" || extension.ToLower() == "gif" || extension.ToLower() == "bmp" || extension.ToLower() == "pdf" || extension.ToLower() == "xlsx" || extension.ToLower() == "xls" || extension.ToLower() == "docx" || extension.ToLower() == "doc")
                    {
                        //string fileName = Path.GetFileName(postedFile.FileName);
                        string fileName = "";
                        // if (Request.QueryString["TranId"] != null)
                        if (TransId != "null" && TransId != "")
                        {
                            // fileName = databaseCon.Decrypt(Request.QueryString["TranId"]) + "_" + DateTime.Now.Ticks + "." + extension;

                             fileName = databaseCon.Decrypt(TransId) + "_" + DateTime.Now.Ticks + "." + extension;
                        }
                        // else if (Request.QueryString["RInvId"] != null)
                        else if (RInvId != "null" && RInvId != "")
                        {
                            fileName = databaseCon.Decrypt(RInvId) + "_" + DateTime.Now.Ticks + "." + extension;
                        }
                        //postedFile.SaveAs(Server.MapPath("~/images/sites") + @"/" + fileName);

                       string path = Path.Combine(Server.MapPath("~/images/sites"), filename);
                        file.SaveAs(path);

                        //string path = "/images/sites/";
                        //path = Server.MapPath(path);

                        string fpath = path;
                        if (extension.Replace(".", "") == "pdf")
                        {
                            extension = "application/pdf";
                        }

                        databaseCon.savefileonblob(fileName, "Site-images", fpath, extension.Replace(".", ""));
                        //  File.Delete(fpath);
                        System.IO.File.Delete(fpath);

                        string query = "";
                        // if (Request.QueryString["TranId"] != null)
                        if (TransId != "null" && TransId != "")
                        {
                            //  query = "insert into tblDeliveryDocketDocuments(DocumentName,Description,DateofUpload,Clientid,transid,Createdby,Createdon,[Type])values('" + fileName + "', '" + txtdesc.Replace("'", "''").Trim() + "', '" + DateTime.Now + "', '" + databaseCon.Decrypt(Request.QueryString["id"]) + "','" + databaseCon.Decrypt(Request.QueryString["TranId"]) + "', (select Emailid from tbluser where UserId ='" + databaseCon.Decrypt(Request.QueryString["UID"]) + "'),'" + DateTime.Now + "','Delivery Docket Document')";
                            query = "insert into tblDeliveryDocketDocuments(DocumentName,Description,DateofUpload,Clientid,transid,Createdby,Createdon,[Type])values('" + fileName + "', '" + txtdesc.Replace("'", "''").Trim() + "', '" + DateTime.Now + "', '" + databaseCon.Decrypt(ClientID) + "','" + databaseCon.Decrypt(TransId) + "', (select Emailid from tbluser where UserId ='" + databaseCon.Decrypt(UID) + "'),'" + DateTime.Now + "','Delivery Docket Document')";
                        }
                        //else if (Request.QueryString["RInvId"] != null)
                        if (RInvId != "null" && RInvId != "")
                        {
                            // query = "insert into tblDeliveryDocketDocuments(DocumentName,Description,DateofUpload,Clientid,TransId,Createdby,Createdon,[Type])values('" + fileName + "', '" + txtdesc.Replace("'", "''").Trim() + "', '" + DateTime.Now + "', '" + databaseCon.Decrypt(Request.QueryString["id"]) + "','" + databaseCon.Decrypt(Request.QueryString["RInvId"]) + "', (select Emailid from tbluser where UserId ='" + databaseCon.Decrypt(Request.QueryString["UID"]) + "'),'" + DateTime.Now + "','Return Inventory Document')";
                            query = "insert into tblDeliveryDocketDocuments(DocumentName,Description,DateofUpload,Clientid,TransId,Createdby,Createdon,[Type])values('" + fileName + "', '" + txtdesc.Replace("'", "''").Trim() + "', '" + DateTime.Now + "', '" + databaseCon.Decrypt(ClientID) + "','" + databaseCon.Decrypt(RInvId) + "', (select Emailid from tbluser where UserId ='" + databaseCon.Decrypt(UID) + "'),'" + DateTime.Now + "','Return Inventory Document')";
                        }

                        count = databaseCon.RunCmd(query);
                    }
                }
                if (count > 0)
                {
                    uploading = true;
                }
                else
                {
                    // art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                    // art1.Style.Add("display", "block");
                    // alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator. ";
                }

            }
            // }

     //       if (hdncapture.Value != "" || imageData != "")
                if (image != "" || image != "")
                {
                var FileExtension = "";
              // var hd = hdncapture.Value;
                string path = "~/uploads/";
                    path = Server.MapPath(path);
                //string name1 = nameget.Value;
                string name = "";
                string FileName = "";
                    if (name != "")
                    {
                        var extname = name.Split('.');
                        string extensionname = extname[1];
                        if (extensionname.ToLower() == "xlsx" || extensionname.ToLower() == "xls" || extensionname.ToLower() == "docx" || extensionname.ToLower() == "doc" || extensionname.ToLower() == "pdf" || extensionname.ToLower() == "jpg" || extensionname.ToLower() == "jpeg" || extensionname.ToLower() == "png" || extensionname.ToLower() == "ico" || extensionname.ToLower() == "gif" || extensionname.ToLower() == "bmp")
                        {
                            if (TransId != "null" && TransId != "")
                            {
                                FileName = "Files" + "_" + databaseCon.Decrypt(TransId) + "_" + DateTime.Now.Ticks + "." + extensionname;
                            }
                            else if (RInvId != "null" && RInvId != "")
                            {
                                FileName = "Files" + "_" + databaseCon.Decrypt(RInvId) + "_" + DateTime.Now.Ticks + "." + extensionname;
                            }
                        }
                        else
                        {
                        //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Failed to upload File! </strong>Valid File Extensions Are .xlsx, .xls, .docx, .doc, .pdf, .jpg, .jpeg, .png, .ico, .gif, .bmp.";
                        Result = "Warning";
                        Message = "Failed to upload File! Valid File Extensions Are .xlsx, .xls, .docx, .doc, .pdf, .jpg, .jpeg, .png, .ico, .gif, .bmp.";
                        }
                    }
                    else
                    {
                   
                        if (TransId != "null" && TransId != "")
                        {
                            FileName = "CaptureImg" + "_" + databaseCon.Decrypt(TransId) + "_" + DateTime.Now.Ticks + ".jpg";
                        }
                        else if (RInvId != "null" && RInvId != "")
                        {
                            FileName = "CaptureImg" + "_" + databaseCon.Decrypt(RInvId) + "_" + DateTime.Now.Ticks + ".jpg";
                        }
                    }
                    if (FileName != "")
                    {
                        string newpath = path + FileName;
                      //  byte[] bytes = Convert.FromBase64String(imageData.Split(',')[1]);
                    byte[] bytes = Convert.FromBase64String(image.Split(',')[1]);
                    var uri = new Uri(databaseCon.siteimagelink);
                        var container = new CloudBlobContainer(uri);
                        var blob = container.GetBlockBlobReference(FileName);

                        var exists = blob.Exists();
                        if (exists == false)
                        {
                            using (var imageFile = new FileStream(newpath, FileMode.Create))
                            {
                                imageFile.Write(bytes, 0, bytes.Length);
                                imageFile.Flush();
                            }
                            if (System.IO.File.Exists(newpath))
                            {

                                string fpath = path + FileName;
                                FileExtension = FileName;
                                var ext = FileExtension.Split('.');
                                string extension = ext[1];
                                if (extension.Replace(".", "") == "pdf")
                                {
                                    extension = "application/pdf";
                                }

                                databaseCon.savefileonblob(FileName, "Site-images", fpath, extension.Replace(".", ""));
                            //databaseCon.savefileonblob(FileName, "Site-images", fpath, "image/jpeg");
                            System.IO.File.Delete(fpath);
                            }
                        }
                        else
                        {
                            databaseCon.deletefileonblob(FileName, "siteimage");
                            using (var imageFile = new FileStream(newpath, FileMode.Create))
                            {
                                imageFile.Write(bytes, 0, bytes.Length);
                                imageFile.Flush();
                            }
                            if (System.IO.File.Exists(newpath))
                            {
                                string fpath = path + FileName;
                                FileExtension = FileName;
                                var ext = FileExtension.Split('.');
                                string extension = ext[1];
                                if (extension.Replace(".", "") == "pdf")
                                {
                                    extension = "application/pdf";
                                }
                                databaseCon.savefileonblob(FileName, "Site-images", fpath, extension.Replace(".", ""));
                            System.IO.File.Delete(fpath);
                            }
                        }
                        string query = "";
                    //if (databaseCon.Decrypt(TransId) != null && databaseCon.Decrypt(TransId) != "" && databaseCon.Decrypt(TransId) != "0")
                    if(TransId !="null" && TransId!="")
                    {
                            query = "insert into tblDeliveryDocketDocuments(DocumentName,Description,DateofUpload,Clientid,transid,Createdby,Createdon,[Type])values('" + FileName + "', '" + txtdesc.Replace("'", "''").Trim() + "', '" + DateTime.Now + "', '" + databaseCon.Decrypt(ClientID) + "','" + databaseCon.Decrypt(TransId) + "', (select Emailid from tbluser where UserId ='" + databaseCon.Decrypt(UID) + "'),'" + DateTime.Now + "','Delivery Docket Document')";

                        }
                        else if (RInvId != "null" && RInvId != "")
                        {
                            query = "insert into tblDeliveryDocketDocuments(DocumentName,Description,DateofUpload,Clientid,TransId,Createdby,Createdon,[Type])values('" + FileName + "', '" + txtdesc.Replace("'", "''").Trim() + "', '" + DateTime.Now + "', '" + databaseCon.Decrypt(ClientID) + "','" + databaseCon.Decrypt(RInvId) + "', (select Emailid from tbluser where UserId ='" + databaseCon.Decrypt(UID) + "'),'" + DateTime.Now + "','Return Inventory Document')";
                        }
                        count = databaseCon.RunCmd(query);
                        if (count > 0)
                        {
                            capturing = true;
                        Result = "Success";
                        Message = "Successfully Data Uploaded.";
                    }
                        else
                        {
                        //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator. ";
                        Result = "Warning";
                        Message = "Error. Contact administrator.";
                        }

                    }

                }
            //if (capturing != false || uploading != false)
            //{
            //    Session["Insert"] = "Insert";
            //    if (TransId != "null" && TransId != "")
            //    {
            //       // Response.Redirect("DeliveryDocketDocument.aspx?UID=" + UID + "&UserRole=" + UserRole + "&id=" + ClientID + "&TranId=" + TransId, false);
            //    }
            //    else if (RInvId != "null" && RInvId != "")
            //    {
            //        if (Site != null)
            //        {
            //            Response.Redirect("DeliveryDocketDocument.aspx?UID=" + UID + "&UserRole=" + UserRole + "&id=" + ClientID + "&RInvId=" + TransId + "&Site=" + databaseCon.Encrypt("1"), false);
            //        }
            //        else if (Job != null)
            //        {
            //            Response.Redirect("DeliveryDocketDocument.aspx?UID=" + UID + "&UserRole=" + UserRole + "&id=" + ClientID + "&RInvId=" + RInvId + "&Job=" + databaseCon.Encrypt("2"), false);
            //        }

            //    }
            //}

            var response = new { Result = Result, Message = Message };

            return Json(response, JsonRequestBehavior.AllowGet);
        }
           



        //protected void btnsave_Click(object sender, EventArgs e)
        //{
        //    Adddocs.Style.Add("display", "none");
        //    bool capturing = false;
        //    bool uploading = false;
        //    int count = 0;
        //    string imageData = tempimg.Value;
        //    //string name = nameget.Value;

        //    string FileExtension = fileuplod.FileName;
        //    if (fileuplod.HasFile == true)
        //    {
        //        if (FileExtension != "")
        //        {
        //            foreach (HttpPostedFile postedFile in fileuplod.PostedFiles)
        //            {
        //                FileExtension = postedFile.FileName;
        //                var ext = FileExtension.Split('.');
        //                string extension = Path.GetExtension(fileuplod.PostedFile.FileName);
        //                extension = ext[1];
        //                if (extension.ToLower() == "jpg" || extension.ToLower() == "jpeg" || extension.ToLower() == "png" || extension.ToLower() == "ico" || extension.ToLower() == "gif" || extension.ToLower() == "bmp" || extension.ToLower() == "pdf" || extension.ToLower() == "xlsx" || extension.ToLower() == "xls" || extension.ToLower() == "docx" || extension.ToLower() == "doc")
        //                {
        //                    //string fileName = Path.GetFileName(postedFile.FileName);
        //                    string fileName = "";
        //                    if (Request.QueryString["TranId"] != null)
        //                    {
        //                        fileName = databaseCon.Decrypt(Request.QueryString["TranId"]) + "_" + DateTime.Now.Ticks + "." + extension;
        //                    }
        //                    else if (Request.QueryString["RInvId"] != null)
        //                    {
        //                        fileName = databaseCon.Decrypt(Request.QueryString["RInvId"]) + "_" + DateTime.Now.Ticks + "." + extension;
        //                    }
        //                    postedFile.SaveAs(Server.MapPath("~/images/sites") + @"/" + fileName);
        //                    string path = "/images/sites/";
        //                    path = Server.MapPath(path);

        //                    string fpath = path + fileName;
        //                    if (extension.Replace(".", "") == "pdf")
        //                    {
        //                        extension = "application/pdf";
        //                    }

        //                    databaseCon.savefileonblob(fileName, "Site-images", fpath, extension.Replace(".", ""));
        //                    File.Delete(fpath);

        //                    string query = "";
        //                    if (Request.QueryString["TranId"] != null)
        //                    {
        //                        query = "insert into tblDeliveryDocketDocuments(DocumentName,Description,DateofUpload,Clientid,transid,Createdby,Createdon,[Type])values('" + fileName + "', '" + txtdesc.Text.Replace("'", "''").Trim() + "', '" + DateTime.Now + "', '" + databaseCon.Decrypt(Request.QueryString["id"]) + "','" + databaseCon.Decrypt(Request.QueryString["TranId"]) + "', (select Emailid from tbluser where UserId ='" + databaseCon.Decrypt(Request.QueryString["UID"]) + "'),'" + DateTime.Now + "','Delivery Docket Document')";
        //                    }
        //                    else if (Request.QueryString["RInvId"] != null)
        //                    {
        //                        query = "insert into tblDeliveryDocketDocuments(DocumentName,Description,DateofUpload,Clientid,TransId,Createdby,Createdon,[Type])values('" + fileName + "', '" + txtdesc.Text.Replace("'", "''").Trim() + "', '" + DateTime.Now + "', '" + databaseCon.Decrypt(Request.QueryString["id"]) + "','" + databaseCon.Decrypt(Request.QueryString["RInvId"]) + "', (select Emailid from tbluser where UserId ='" + databaseCon.Decrypt(Request.QueryString["UID"]) + "'),'" + DateTime.Now + "','Return Inventory Document')";
        //                    }

        //                    count = databaseCon.RunCmd(query);
        //                }
        //            }
        //            if (count > 0)
        //            {
        //                uploading = true;
        //            }
        //            else
        //            {
        //                art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
        //                art1.Style.Add("display", "block");
        //                alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator. ";
        //            }

        //        }
        //    }
        //    if (hdncapture.Value != "" || imageData != "")
        //    {
        //        hdncapture.Value = "";
        //        string path = "~/uploads/";
        //        path = Server.MapPath(path);
        //        string name = nameget.Value;
        //        string FileName = "";
        //        if (name != "")
        //        {
        //            var extname = name.Split('.');
        //            string extensionname = extname[1];
        //            if (extensionname.ToLower() == "xlsx" || extensionname.ToLower() == "xls" || extensionname.ToLower() == "docx" || extensionname.ToLower() == "doc" || extensionname.ToLower() == "pdf" || extensionname.ToLower() == "jpg" || extensionname.ToLower() == "jpeg" || extensionname.ToLower() == "png" || extensionname.ToLower() == "ico" || extensionname.ToLower() == "gif" || extensionname.ToLower() == "bmp")
        //            {
        //                if (Request.QueryString["TranId"] != null)
        //                {
        //                    FileName = "Files" + "_" + databaseCon.Decrypt(Request.QueryString["TranId"]) + "_" + DateTime.Now.Ticks + "." + extensionname;
        //                }
        //                else if (Request.QueryString["RInvId"] != null)
        //                {
        //                    FileName = "Files" + "_" + databaseCon.Decrypt(Request.QueryString["RInvId"]) + "_" + DateTime.Now.Ticks + "." + extensionname;
        //                }
        //            }
        //            else
        //            {
        //                art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
        //                art1.Style.Add("display", "block");
        //                alertlabel.Text = "<strong>Failed to upload File! </strong>Valid File Extensions Are .xlsx, .xls, .docx, .doc, .pdf, .jpg, .jpeg, .png, .ico, .gif, .bmp.";
        //            }
        //        }
        //        else
        //        {
        //            if (Request.QueryString["TranId"] != null)
        //            {
        //                FileName = "CaptureImg" + "_" + databaseCon.Decrypt(Request.QueryString["TranId"]) + "_" + DateTime.Now.Ticks + ".jpg";
        //            }
        //            else if (Request.QueryString["RInvId"] != null)
        //            {
        //                FileName = "CaptureImg" + "_" + databaseCon.Decrypt(Request.QueryString["RInvId"]) + "_" + DateTime.Now.Ticks + ".jpg";
        //            }
        //        }
        //        if (FileName != "")
        //        {
        //            string newpath = path + FileName;
        //            byte[] bytes = Convert.FromBase64String(imageData.Split(',')[1]);
        //            var uri = new Uri(databaseCon.siteimagelink);
        //            var container = new CloudBlobContainer(uri);
        //            var blob = container.GetBlockBlobReference(FileName);

        //            var exists = blob.Exists();
        //            if (exists == false)
        //            {
        //                using (var imageFile = new FileStream(newpath, FileMode.Create))
        //                {
        //                    imageFile.Write(bytes, 0, bytes.Length);
        //                    imageFile.Flush();
        //                }
        //                if (File.Exists(newpath))
        //                {

        //                    string fpath = path + FileName;
        //                    FileExtension = FileName;
        //                    var ext = FileExtension.Split('.');
        //                    string extension = ext[1];
        //                    if (extension.Replace(".", "") == "pdf")
        //                    {
        //                        extension = "application/pdf";
        //                    }

        //                    databaseCon.savefileonblob(FileName, "Site-images", fpath, extension.Replace(".", ""));
        //                    //databaseCon.savefileonblob(FileName, "Site-images", fpath, "image/jpeg");
        //                    File.Delete(fpath);
        //                }
        //            }
        //            else
        //            {
        //                databaseCon.deletefileonblob(FileName, "siteimage");
        //                using (var imageFile = new FileStream(newpath, FileMode.Create))
        //                {
        //                    imageFile.Write(bytes, 0, bytes.Length);
        //                    imageFile.Flush();
        //                }
        //                if (File.Exists(newpath))
        //                {
        //                    string fpath = path + FileName;
        //                    FileExtension = FileName;
        //                    var ext = FileExtension.Split('.');
        //                    string extension = ext[1];
        //                    if (extension.Replace(".", "") == "pdf")
        //                    {
        //                        extension = "application/pdf";
        //                    }
        //                    databaseCon.savefileonblob(FileName, "Site-images", fpath, extension.Replace(".", ""));
        //                    File.Delete(fpath);
        //                }
        //            }
        //            string query = "";
        //            if (Request.QueryString["TranId"] != null)
        //            {
        //                query = "insert into tblDeliveryDocketDocuments(DocumentName,Description,DateofUpload,Clientid,transid,Createdby,Createdon,[Type])values('" + FileName + "', '" + txtdesc.Text.Replace("'", "''").Trim() + "', '" + DateTime.Now + "', '" + databaseCon.Decrypt(Request.QueryString["id"]) + "','" + databaseCon.Decrypt(Request.QueryString["TranId"]) + "', (select Emailid from tbluser where UserId ='" + databaseCon.Decrypt(Request.QueryString["UID"]) + "'),'" + DateTime.Now + "','Delivery Docket Document')";

        //            }
        //            else if (Request.QueryString["RInvId"] != null)
        //            {
        //                query = "insert into tblDeliveryDocketDocuments(DocumentName,Description,DateofUpload,Clientid,TransId,Createdby,Createdon,[Type])values('" + FileName + "', '" + txtdesc.Text.Replace("'", "''").Trim() + "', '" + DateTime.Now + "', '" + databaseCon.Decrypt(Request.QueryString["id"]) + "','" + databaseCon.Decrypt(Request.QueryString["RInvId"]) + "', (select Emailid from tbluser where UserId ='" + databaseCon.Decrypt(Request.QueryString["UID"]) + "'),'" + DateTime.Now + "','Return Inventory Document')";
        //            }
        //            count = databaseCon.RunCmd(query);
        //            if (count > 0)
        //            {
        //                capturing = true;
        //            }
        //            else
        //            {
        //                art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
        //                art1.Style.Add("display", "block");
        //                alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator. ";
        //            }

        //        }

        //    }
        //    if (capturing != false || uploading != false)
        //    {
        //        Session["Insert"] = "Insert";
        //        if (Request.QueryString["TranId"] != null)
        //        {
        //            Response.Redirect("DeliveryDocketDocument.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&TranId=" + Request.QueryString["TranId"], false);
        //        }
        //        else if (Request.QueryString["RInvId"] != null)
        //        {
        //            if (Request.QueryString["Site"] != null)
        //            {
        //                Response.Redirect("DeliveryDocketDocument.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RInvId=" + Request.QueryString["RInvId"] + "&Site=" + databaseCon.Encrypt("1"), false);
        //            }
        //            else if (Request.QueryString["Job"] != null)
        //            {
        //                Response.Redirect("DeliveryDocketDocument.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RInvId=" + Request.QueryString["RInvId"] + "&Job=" + databaseCon.Encrypt("2"), false);
        //            }

        //        }
        //    }
        //}



    }
}