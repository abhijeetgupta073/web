using Scaffold.App_Code;
using Scaffold_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure.Storage.Blob;
using iTextSharp.text.pdf.codec;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.Ajax.Utilities;

namespace Scaffold.Controllers
{
    public class DocumentController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: Document
        public ActionResult Document(string UID, string UserRole, string id, string Datetagged, string RID)
        {
            return View();
        }

        public ActionResult txtdoctype_TextChanged(string ClientID, string UID, string txtDoctype)
        {
            string Result = "";
            string Message = "";
            string doctype = txtDoctype.Replace("'", "''").Trim();
            if (!string.IsNullOrEmpty(doctype))
            {
                string query = "select documentType from tbldocumenttype where documentType='" + doctype + "' and clientid ='" + databaseCon.Decrypt(ClientID) + "' and DocumentCategoryLocation='Register' ";
                DataTable dt = databaseCon.GetData(query);
                if (dt.Rows.Count <= 0)
                {
                    query = " insert into tbldocumenttype(documentType,CreatedBy,CreatedOn,clientid,DocumentCategoryLocation) values('" + doctype.Replace("'", "''").Trim() + "',(select Emailid from tbluser where UserId='" + databaseCon.Decrypt(UID) + "'),'" + DateTime.Now + "','" + databaseCon.Decrypt(ClientID) + "','Register')";
                    int i = databaseCon.RunCmd(query);
                    if (i > 0)
                    {
                        Result = "Success";
                        Message = "Record Saved Successfully";
                        //art1.Style.Add("display", "block");
                        //art1.Attributes["class"] = "w3-section w3-card-12 j-close fade in j-alertBox-Success";
                        //alertlabel.Text = " <strong>Success ! </strong>Record Saved Successfully";
                        //txtdoctype.Text = "";
                        //modaltextbox.Style.Add("display", "none");
                        //bindddl();
                        //ddldoctype.Text = doctype;
                        // string io = ddldoctype.DataValueField;
                    }
                }
                else
                {
                    Result = "Warning";
                    Message = "Docuemnt type already exists...";
                    //art1.Style.Add("display", "block");
                    //art1.Attributes["class"] = "w3-section w3-card-12 j-close fade in j-alertBox-Failed";
                    //alertlabel.Text = "<strong>Alert ! </strong> Docuemnt type already exists...";
                }
            }

            var response = new { Result = Result, Message = Message };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ddlDocumentType(string ClientID)
        {
            var GetddlData = dbContext.Database.SqlQuery<DocumentDDlTypeViewModel>("select DocumentTypeId, documentType from tbldocumenttype where clientid ='" + databaseCon.Decrypt(ClientID) + "'  and DocumentCategoryLocation = 'Register' order by CreatedOn desc").ToList();

            return Json(GetddlData, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DeleteDocument(string DocumentId)
        {

            string Result = "";

            string query = "delete from  tbldocuments where DocumentId =" + DocumentId;
            int i = databaseCon.RunCmd(query);
            if (i > 0)
            {
                Result = "Success";
                //var data = dbContext.tblDocuments.Where(_ => _.DocumentId == DocId).FirstOrDefault();
                //databaseCon.deletefileonblob(data.DocumentName, "registerdocument");
            }

            return Json(Result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult btnEditSave(string id, string UID, string hdnDDDocumentID, string txtdescription, string hdnDocumentName)
        {
            var Result = "";
            if (txtdescription != "")
            {
                //string query = "update tbldocuments set Description = '" + txtdescription.Replace("'", "''").Trim() + "',UpdatedOn='" + DateTime.Now + "',UpdatedBy=(select Emailid from tbluser where UserId='" + databaseCon.Decrypt(UID) + "')  where  DocumentName = '" + hdnDocumentName + "' and DocumentId='" + hdnDDDocumentID + "'";
                string query = "update tbldocuments set Description = '" + txtdescription.Replace("'", "''").Trim() + "',UpdatedOn='" + DateTime.Now + "',UpdatedBy=(select Emailid from tbluser where UserId='" + databaseCon.Decrypt(UID) + "')  where   DocumentId='" + hdnDDDocumentID + "'";
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


        public ActionResult GetDocumentTypeData(string ClientID, string RID)
        {
            string query = "select documentType, DocumentId,tblDocuments.ClientID,DocumentName,Description,DateOfUpload,tblDocuments.DocumentTypeId,RegisterID,OriginalDocumentName from tbldocuments join tblDocumentType on tblDocuments.DocumentTypeId = tblDocumentType.DocumentTypeId  where RegisterID = " + databaseCon.Decrypt(RID) + " and tblDocuments.clientid = " + databaseCon.Decrypt(ClientID) + " order by dateofupload desc";

            var GetData = dbContext.Database.SqlQuery<GetDocumentTypeData>(query).ToList();
            foreach (var item in GetData)
            {
                var DocumentPdfPath = item.DocumentName;
                if (DocumentPdfPath != "")
                {
                    string path = databaseCon.registerdocumentlink + DocumentPdfPath;
                    var uri = new Uri(databaseCon.registerdocumentlink);
                    var container = new CloudBlobContainer(uri);
                    var blob = container.GetBlockBlobReference(DocumentPdfPath);
                    var exists = blob.Exists();
                    if (exists == false)
                    {
                        item.DocumentURL = "File does not exists.";
                    }
                    else
                    {
                        item.DocumentURL = path;
                    }
                }
            }

            return Json(GetData, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult btnsave_Click(string ClientID, string UID, string RID, string ddldoctype, string txtdesc, string browerFile, string OriginalDocumentName)
        //{
        //    string Result = "";
        //    string Message = "";
        //    string filename = "C0000000";
        //    string targetfolder = "";
        //    string orgfilename = "";
        //    int docfile2;
        //    string NewFileName;
        //    string tempdocfile;

        //    string docid = databaseCon.getStringCell("select DocumentTypeId from tblDocumentType where DocumentType='" + ddldoctype + "' and clientid = '" + databaseCon.Decrypt(ClientID) + "'and DocumentCategoryLocation='Register'");
        //    //if (fileuplod.HasFile)
        //    //if (fileuplod.HasFile)
        //    //{
        //    if (Request.Files.Count > 0)
        //    {
        //        //  if (FileExtension != "")
        //        //  {
        //        //  foreach (HttpPostedFile postedFile in fileuplod.PostedFiles)
        //        //  {
        //        HttpFileCollectionBase files = Request.Files;
        //        for (int k = 0; k < 1; k++)
        //        {

        //            HttpPostedFileBase file = files[k];
        //            //string filename;
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

        //            string extension = System.IO.Path.GetExtension(filename).ToLower();

        //            //Commented on (01-11-2022)  Because we dont to handel Other Document differently in scaffol Type start here 
        //            //string extension = Path.GetExtension(fileuplod.PostedFile.FileName);
        //            //if (ddldoctype.ToUpper() == "Other".ToUpper())
        //            //{
        //            //    //int file = Request.QueryString["RID"].ToString().Length;
        //            //    //filename = filename.Remove(filename.Length - file, file);
        //            //    //filename = filename + Request.QueryString["RID"];
        //            //    //orgfilename = filename + "_" + fileuplod.FileName.Replace(" ", "_").Trim() + DateTime.Now.Date.ToString("MM-dd-yy");
        //            //    //orgfilename = orgfilename.Replace(extension, "_") + extension;
        //            //    //targetfolder = "/uploads/Documents/" + orgfilename;
        //            //    //fileuplod.SaveAs(Server.MapPath(targetfolder));
        //            //    var RegisterId = databaseCon.Decrypt(RID);

        //            //    int file1 = RegisterId.ToString().Length;
        //            //    filename = filename.Remove(filename.Length - file1, file1);
        //            //    //filename = filename + RID;
        //            //    // filename = filename;
        //            //    // orgfilename = filename + "_" + filename.Replace(" ", "_").Trim() + DateTime.Now.Date.ToString("MM-dd-yy");
        //            //    orgfilename = filename.Replace(" ", "_").Trim() + DateTime.Now.Date.ToString("MM-dd-yy");
        //            //    orgfilename = orgfilename.Replace(extension, "_") + extension;
        //            //    // string targetfolder1 = "/uploads/" + orgfilename;
        //            //    string targetfolder1 = Path.Combine(Server.MapPath("~/uploads/"), orgfilename);
        //            //    file.SaveAs(targetfolder1);

        //            //    NewFileName = "SnapImg" + "_" + DateTime.Now.Ticks + "_" + databaseCon.Decrypt(RID) + ".jpg";

        //            //    //string query = "insert into tbldocuments(ClientID,RegisterID,DocumentName,Description,DateOfUpload,DocumentTypeId,CreatedBy,CreatedOn)values(" + Request.QueryString["id"] + ",'" + Request.QueryString["RID"] + "','" + orgfilename + "','" + txtdesc.Text.Replace("'", "''").Trim() + "','" + DateTime.Now + "','" + ddldoctype.SelectedValue + "','" + Request.QueryString["UID"] + "','" + DateTime.Now + "')";
        //            //    string query = "insert into tbldocuments(ClientID,RegisterID,DocumentName,Description,DateOfUpload,DocumentTypeId,CreatedBy,CreatedOn,OriginalDocumentName)values(" + databaseCon.Decrypt(ClientID) + ",'" + databaseCon.Decrypt(RID) + "','" + NewFileName + "','" + txtdesc.Replace("'", "''").Trim() + "','" + DateTime.Now + "','" + docid + "',(select Emailid from tbluser where UserId='" + databaseCon.Decrypt(UID) + "'),'" + DateTime.Now + "','" + OriginalDocumentName.Replace("'", "''").Trim() + "')";
        //            //    int i = databaseCon.RunCmd(query);
        //            //    if (i > 0)
        //            //    {
        //            //        //string path = Server.MapPath(targetfolder);
        //            //        string path = targetfolder1;
        //            //        if (extension.Replace(".", "") == "pdf")
        //            //        {
        //            //            extension = "application/pdf";
        //            //        }

        //            //        file.SaveAs(Server.MapPath("/uploads/") + @"/" + orgfilename);
        //            //        string path1 = "/uploads/";
        //            //        path1 = Server.MapPath(path1);

        //            //        string FileName = path1 + orgfilename;

        //            //        string newPath = FileName;

        //            //        Image img = Image.FromFile(newPath);

        //            //        if (extension.Replace(".", "") == "pdf")
        //            //        {
        //            //            extension = "application/pdf";
        //            //        }

        //            //        if (img.PropertyIdList.Contains(0x0112))
        //            //        {

        //            //            PropertyItem propOrientation = img.GetPropertyItem(0x0112);
        //            //            short orientation = BitConverter.ToInt16(propOrientation.Value, 0);
        //            //            if (orientation == 6)
        //            //            {
        //            //                img.RotateFlip(RotateFlipType.Rotate90FlipNone);
        //            //            }
        //            //            else if (orientation == 8)
        //            //            {
        //            //                img.RotateFlip(RotateFlipType.Rotate270FlipNone);
        //            //            }
        //            //            else if (orientation == 3)
        //            //            {
        //            //                img.RotateFlip(RotateFlipType.Rotate180FlipNone);
        //            //            }
        //            //            else
        //            //            {
        //            //                // Do nothing
        //            //            }

        //            //            //tempImage = filename;
        //            //            //string filename = NewFileName;
        //            //            var newWidth = (int)(img.Width * 0.5);
        //            //            var newHeight = (int)(img.Height * 0.5);
        //            //            var thumbnailImg = new Bitmap(newWidth, newHeight);
        //            //            var thumbGraph = Graphics.FromImage(thumbnailImg);
        //            //            thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
        //            //            thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
        //            //            thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //            //            var imageRectangle = new System.Drawing.Rectangle(0, 0, newWidth, newHeight);

        //            //            thumbGraph.DrawImage(img, imageRectangle);

        //            //            byte[] bte = GetCompressedBitmap(thumbnailImg, 75);
        //            //            MemoryStream ms2 = new MemoryStream(bte);
        //            //            System.Drawing.Image img12 = System.Drawing.Image.FromStream(ms2);

        //            //            thumbnailImg.Save(path1 + NewFileName, img12.RawFormat);
        //            //        }

        //            //        else
        //            //        {
        //            //            //tempImage = imageData;
        //            //            //string filename = NewFileName;
        //            //            var newWidth = (int)(img.Width * 0.5);
        //            //            var newHeight = (int)(img.Height * 0.5);
        //            //            var thumbnailImg = new Bitmap(newWidth, newHeight);
        //            //            var thumbGraph = Graphics.FromImage(thumbnailImg);
        //            //            thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
        //            //            thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
        //            //            thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //            //            var imageRectangle = new System.Drawing.Rectangle(0, 0, newWidth, newHeight);

        //            //            thumbGraph.DrawImage(img, imageRectangle);

        //            //            byte[] bte = GetCompressedBitmap(thumbnailImg, 50);
        //            //            MemoryStream ms2 = new MemoryStream(bte);
        //            //            System.Drawing.Image img12 = System.Drawing.Image.FromStream(ms2);

        //            //            thumbnailImg.Save(path1 + NewFileName, img12.RawFormat);
        //            //        }

        //            //        string fpath = path1 + NewFileName;

        //            //        databaseCon.savefileonblob(NewFileName, "Inspection-Document", fpath, extension.Replace(".", ""));

        //            //        //System.IO.File.Delete(path);
        //            //        //txtdesc.Text = "";
        //            //        //docsgrid.Rebind();
        //            //        //art1.Style.Add("display", "block");
        //            //        //art1.Attributes["class"] = "w3-section w3-card-12 j-close fade in j-alertBox-Success";
        //            //        //alertlabel.Text = "<strong>Success ! </strong> Record Saved Successfully";
        //            //        Result = "Success";
        //            //        Message = "Record Saved Successfully";
        //            //    }
        //            //    else
        //            //    {
        //            //        //ViewState["doctype"] = ddldoctype.SelectedValue;
        //            //        //art1.Style.Add("display", "block");
        //            //        //art1.Attributes["class"] = "w3-section w3-card-12 j-close fade in j-alertBox-Failed";
        //            //        //alertlabel.Text = "<strong>Alert ! </strong> Select any file and document type";
        //            //        Result = "Failed";
        //            //        Message = "Select any file and document type";
        //            //    }
        //            //}
        //            //else
        //            //{

        //            //End Here (01-11-2022)

        //            var ext = extension.Split('.');
        //            var Tempextension = ext[1];

        //            if (Tempextension.ToLower() == "jpg" || Tempextension.ToLower() == "jpeg" || Tempextension.ToLower() == "png" || Tempextension.ToLower() == "ico" || Tempextension.ToLower() == "gif" || Tempextension.ToLower() == "bmp")
        //            {

        //                //string docfile = databaseCon.Decrypt(ClientID) + "_" + ddldoctype + "_";
        //                //string query2 = "select Max(Cast (Left(REVERSE(SUBSTRING(REVERSE((DocumentName)),0,CHARINDEX('_',REVERSE( (DocumentName))))),CHARINDEX('.',REVERSE(SUBSTRING(REVERSE((DocumentName)),0,CHARINDEX('_',REVERSE( (DocumentName))))))-1) as int ))  as number from tbldocuments where DocumentName like '%" + docfile + "%'order by number";
        //                //DataTable dt = databaseCon.GetData(query2);
        //                //if (dt.Rows.Count >= 1 && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["number"])) && dt.Rows[0]["number"] != "" && dt.Rows[0]["number"] != "null" && dt.Rows[0]["number"] != null)
        //                //{
        //                //    //string[] temp = Convert.ToString(dt.Rows[0]["number"]).Split('.');
        //                //    //docfile2 = Convert.ToInt32(temp[0]);
        //                //    docfile2 = Convert.ToInt32(dt.Rows[0]["number"]) + 1;
        //                //    docfile = docfile + docfile2 + extension;
        //                //    tempdocfile = databaseCon.Decrypt(ClientID) + "_" + ddldoctype + "_" + docfile2 + DateTime.Now.Ticks + extension;
        //                //}
        //                //else
        //                //{
        //                //    docfile = docfile + 1 + extension;
        //                //    tempdocfile = databaseCon.Decrypt(ClientID) + "_" + ddldoctype + "_" + 1 + DateTime.Now.Ticks + extension;
        //                //}

        //                //commented on 07july23 To remove doctype from filename and add Registerid instead
        //                //string docfile = databaseCon.Decrypt(ClientID) + "_" + ddldoctype + "_" + DateTime.Now.Ticks + extension;
        //                //tempdocfile = databaseCon.Decrypt(ClientID) + "_" + ddldoctype + "_" + 1 + DateTime.Now.Ticks + extension;

        //                string docfile = databaseCon.Decrypt(ClientID) + "_" + "Job_" + databaseCon.Decrypt(RID) + "_" + DateTime.Now.Ticks + extension;
        //                tempdocfile = databaseCon.Decrypt(ClientID) + "_" + "Job_" + databaseCon.Decrypt(RID) + "_" + 1 + DateTime.Now.Ticks + extension;

        //                file.SaveAs(Server.MapPath("/uploads/") + @"/" + docfile);
        //                string path = "/uploads/";
        //                path = Server.MapPath(path);

        //                string FileName = path + docfile;

        //                string newPath = FileName;

        //                Image img = Image.FromFile(newPath);

        //                NewFileName = tempdocfile ;

        //                if (extension.Replace(".", "") == "pdf")
        //                {
        //                    extension = "application/pdf";
        //                }

        //                if (img.PropertyIdList.Contains(0x0112))
        //                {

        //                    PropertyItem propOrientation = img.GetPropertyItem(0x0112);
        //                    short orientation = BitConverter.ToInt16(propOrientation.Value, 0);
        //                    if (orientation == 6)
        //                    {
        //                        img.RotateFlip(RotateFlipType.Rotate90FlipNone);
        //                    }
        //                    else if (orientation == 8)
        //                    {
        //                        img.RotateFlip(RotateFlipType.Rotate270FlipNone);
        //                    }
        //                    else if (orientation == 3)
        //                    {
        //                        img.RotateFlip(RotateFlipType.Rotate180FlipNone);
        //                    }
        //                    else
        //                    {
        //                        // Do nothing
        //                    }

        //                    var newWidth = (int)(img.Width * 0.5);
        //                    var newHeight = (int)(img.Height * 0.5);
        //                    var thumbnailImg = new Bitmap(newWidth, newHeight);
        //                    var thumbGraph = Graphics.FromImage(thumbnailImg);
        //                    thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
        //                    thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
        //                    thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //                    var imageRectangle = new System.Drawing.Rectangle(0, 0, newWidth, newHeight);

        //                    thumbGraph.DrawImage(img, imageRectangle);

        //                    byte[] bte = GetCompressedBitmap(thumbnailImg, 75);
        //                    MemoryStream ms2 = new MemoryStream(bte);
        //                    System.Drawing.Image img12 = System.Drawing.Image.FromStream(ms2);

        //                    thumbnailImg.Save(path + NewFileName, img12.RawFormat);
        //                }

        //                else
        //                {
        //                    var newWidth = (int)(img.Width * 0.5);
        //                    var newHeight = (int)(img.Height * 0.5);
        //                    var thumbnailImg = new Bitmap(newWidth, newHeight);
        //                    var thumbGraph = Graphics.FromImage(thumbnailImg);
        //                    thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
        //                    thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
        //                    thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //                    var imageRectangle = new System.Drawing.Rectangle(0, 0, newWidth, newHeight);

        //                    thumbGraph.DrawImage(img, imageRectangle);

        //                    byte[] bte = GetCompressedBitmap(thumbnailImg, 50);
        //                    MemoryStream ms2 = new MemoryStream(bte);
        //                    System.Drawing.Image img12 = System.Drawing.Image.FromStream(ms2);

        //                    thumbnailImg.Save(path + NewFileName, img12.RawFormat);
        //                }

        //                string fpath = path + NewFileName;

        //                //string query = "insert into tbldocuments(ClientID,RegisterID,DocumentName,Description,DateOfUpload,DocumentTypeId,CreatedBy,CreatedOn)values(" + Request.QueryString["id"] + ",'" + Request.QueryString["RID"] + "','" + docfile + "','" + txtdesc.Text.Replace("'", "''").Trim() + "','" + DateTime.Now + "','" + ddldoctype.SelectedValue + "','" + Request.QueryString["UID"] + "','" + DateTime.Now + "')";
        //                string query = "insert into tbldocuments(ClientID,RegisterID,DocumentName,Description,DateOfUpload,DocumentTypeId,CreatedBy,CreatedOn,OriginalDocumentName)values(" + databaseCon.Decrypt(ClientID) + ",'" + databaseCon.Decrypt(RID) + "','" + NewFileName + "','" + txtdesc.Replace("'", "''").Trim() + "','" + DateTime.Now + "','" + docid + "', (select Emailid from tbluser where UserId ='" + databaseCon.Decrypt(UID) + "'),'" + DateTime.Now + "','" + OriginalDocumentName.Replace("'", "''").Trim() + "')";
        //                int i = databaseCon.RunCmd(query);
        //                if (i > 0)
        //                {
        //                    //string path = targetfolder;
        //                    if (extension.Replace(".", "") == "pdf")
        //                    {
        //                        extension = "application/pdf";
        //                    }
        //                    var a = extension.Replace(".", "");

        //                    databaseCon.savefileonblob(NewFileName, "Inspection-Document", fpath, extension.Replace(".", ""));

        //                    System.IO.File.Delete(fpath);
        //                    //txtdesc.Text = "";
        //                    //docsgrid.Rebind();
        //                    //art1.Style.Add("display", "block");
        //                    //art1.Attributes["class"] = "w3-section w3-card-12 j-close fade in j-alertBox-Success";
        //                    //alertlabel.Text = "<strong>Success ! </strong> Record Saved Successfully";
        //                    // btnsave.Enabled = true;
        //                    Result = "Success";
        //                    Message = "Record Saved Successfully";
        //                }
        //                else
        //                {
        //                    //ViewState["doctype"] = ddldoctype.SelectedValue;
        //                    //art1.Style.Add("display", "block");
        //                    //art1.Attributes["class"] = "w3-section w3-card-12 j-close fade in j-alertBox-Failed";
        //                    //alertlabel.Text = "<strong>Alert ! </strong> Select any file and document type";
        //                    Result = "Failed";
        //                    Message = "Select any file and document type";
        //                }
        //                //}
        //                //Adddocs.Style.Add("display", "none");
        //                //ddldoctype.ClearSelection();
        //            }

        //            else
        //            {
        //                //commented as we decided to use timetick for all files
        //                // string docfile = databaseCon.Decrypt(ClientID) + "_" + ddldoctype + "_";

        //                //string query2 = "select Max(Cast (Left(REVERSE(SUBSTRING(REVERSE((DocumentName)),0,CHARINDEX('_',REVERSE( (DocumentName))))),CHARINDEX('.',REVERSE(SUBSTRING(REVERSE((DocumentName)),0,CHARINDEX('_',REVERSE( (DocumentName))))))-1) as int ))  as number from tbldocuments where DocumentName like '%" + docfile + "%'order by number";
        //                //DataTable dt = databaseCon.GetData(query2);
        //                //if (dt.Rows.Count >= 1 && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["number"])) && dt.Rows[0]["number"] != "" && dt.Rows[0]["number"] != "null" && dt.Rows[0]["number"] != null)
        //                //{
        //                //    //string[] temp = Convert.ToString(dt.Rows[0]["number"]).Split('.');
        //                //    //docfile2 = Convert.ToInt32(temp[0]);
        //                //    docfile2 = Convert.ToInt32(dt.Rows[0]["number"]) + 1;
        //                //    docfile = docfile + docfile2 + extension;

        //                //}
        //                //else
        //                //{
        //                //    docfile = docfile + 1 + extension;

        //                //}

        //                //commented on 07july23 To remove doctype from filename and add Registerid instead
        //                //string docfile = databaseCon.Decrypt(ClientID) + "_" + ddldoctype + "_"  + DateTime.Now.Ticks + extension;


        //                string docfile = databaseCon.Decrypt(ClientID) + "_" + "Job_" + databaseCon.Decrypt(RID) + "_" + DateTime.Now.Ticks + extension;

        //                //targetfolder = "/uploads/Documents/" + docfile;
        //                // fileuplod.SaveAs(Server.MapPath(targetfolder));
        //                targetfolder = Path.Combine(Server.MapPath("~/uploads/"), docfile);
        //                file.SaveAs(targetfolder);

        //                //string query = "insert into tbldocuments(ClientID,RegisterID,DocumentName,Description,DateOfUpload,DocumentTypeId,CreatedBy,CreatedOn)values(" + Request.QueryString["id"] + ",'" + Request.QueryString["RID"] + "','" + docfile + "','" + txtdesc.Text.Replace("'", "''").Trim() + "','" + DateTime.Now + "','" + ddldoctype.SelectedValue + "','" + Request.QueryString["UID"] + "','" + DateTime.Now + "')";
        //                string query = "insert into tbldocuments(ClientID,RegisterID,DocumentName,Description,DateOfUpload,DocumentTypeId,CreatedBy,CreatedOn,OriginalDocumentName)values(" + databaseCon.Decrypt(ClientID) + ",'" + databaseCon.Decrypt(RID) + "','" + docfile + "','" + txtdesc.Replace("'", "''").Trim() + "','" + DateTime.Now + "','" + docid + "', (select Emailid from tbluser where UserId ='" + databaseCon.Decrypt(UID) + "'),'" + DateTime.Now + "','" + OriginalDocumentName.Replace("'", "''").Trim() + "')";
        //                int i = databaseCon.RunCmd(query);
        //                if (i > 0)
        //                {

        //                    // string path = Server.MapPath(targetfolder);
        //                    string path = targetfolder;
        //                    if (extension.Replace(".", "") == "pdf")
        //                    {
        //                        extension = "application/pdf";
        //                    }
        //                    var a = extension.Replace(".", "");
        //                    databaseCon.savefileonblob(docfile, "Inspection-Document", path, extension.Replace(".", ""));

        //                    System.IO.File.Delete(path);
        //                    //txtdesc.Text = "";
        //                    //docsgrid.Rebind();
        //                    //art1.Style.Add("display", "block");
        //                    //art1.Attributes["class"] = "w3-section w3-card-12 j-close fade in j-alertBox-Success";
        //                    //alertlabel.Text = "<strong>Success ! </strong> Record Saved Successfully";
        //                    // btnsave.Enabled = true;
        //                    Result = "Success";
        //                    Message = "Record Saved Successfully";
        //                }
        //                else
        //                {
        //                    //ViewState["doctype"] = ddldoctype.SelectedValue;
        //                    //art1.Style.Add("display", "block");
        //                    //art1.Attributes["class"] = "w3-section w3-card-12 j-close fade in j-alertBox-Failed";
        //                    //alertlabel.Text = "<strong>Alert ! </strong> Select any file and document type";
        //                    Result = "Failed";
        //                    Message = "Select any file and document type";
        //                }
        //            }

        //        }
        //    }
        //    var response = new { Result = Result, Message = Message };
        //    return Json(response, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult btnsave_Click(string ClientID, string UID, string RID, string ddldoctype, string txtdesc, string browerFile, string OriginalDocumentName)
        {
            string Result = "";
            string Message = "";
            string filename = "C0000000";
            string targetfolder = "";
            string orgfilename = "";
            int docfile2;
            string NewFileName;
            string tempdocfile;

            string docid = databaseCon.getStringCell("select DocumentTypeId from tblDocumentType where DocumentType='" + ddldoctype + "' and clientid = '" + databaseCon.Decrypt(ClientID) + "'and DocumentCategoryLocation='Register'");
            //if (fileuplod.HasFile)
            //if (fileuplod.HasFile)
            //{
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
                    //string filename;
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

                    string extension = System.IO.Path.GetExtension(filename).ToLower();

                    //Commented on (01-11-2022)  Because we dont to handel Other Document differently in scaffol Type start here 
                    //string extension = Path.GetExtension(fileuplod.PostedFile.FileName);
                    //if (ddldoctype.ToUpper() == "Other".ToUpper())
                    //{
                    //    //int file = Request.QueryString["RID"].ToString().Length;
                    //    //filename = filename.Remove(filename.Length - file, file);
                    //    //filename = filename + Request.QueryString["RID"];
                    //    //orgfilename = filename + "_" + fileuplod.FileName.Replace(" ", "_").Trim() + DateTime.Now.Date.ToString("MM-dd-yy");
                    //    //orgfilename = orgfilename.Replace(extension, "_") + extension;
                    //    //targetfolder = "/uploads/Documents/" + orgfilename;
                    //    //fileuplod.SaveAs(Server.MapPath(targetfolder));
                    //    var RegisterId = databaseCon.Decrypt(RID);

                    //    int file1 = RegisterId.ToString().Length;
                    //    filename = filename.Remove(filename.Length - file1, file1);
                    //    //filename = filename + RID;
                    //    // filename = filename;
                    //    // orgfilename = filename + "_" + filename.Replace(" ", "_").Trim() + DateTime.Now.Date.ToString("MM-dd-yy");
                    //    orgfilename = filename.Replace(" ", "_").Trim() + DateTime.Now.Date.ToString("MM-dd-yy");
                    //    orgfilename = orgfilename.Replace(extension, "_") + extension;
                    //    // string targetfolder1 = "/uploads/" + orgfilename;
                    //    string targetfolder1 = Path.Combine(Server.MapPath("~/uploads/"), orgfilename);
                    //    file.SaveAs(targetfolder1);

                    //    NewFileName = "SnapImg" + "_" + DateTime.Now.Ticks + "_" + databaseCon.Decrypt(RID) + ".jpg";

                    //    //string query = "insert into tbldocuments(ClientID,RegisterID,DocumentName,Description,DateOfUpload,DocumentTypeId,CreatedBy,CreatedOn)values(" + Request.QueryString["id"] + ",'" + Request.QueryString["RID"] + "','" + orgfilename + "','" + txtdesc.Text.Replace("'", "''").Trim() + "','" + DateTime.Now + "','" + ddldoctype.SelectedValue + "','" + Request.QueryString["UID"] + "','" + DateTime.Now + "')";
                    //    string query = "insert into tbldocuments(ClientID,RegisterID,DocumentName,Description,DateOfUpload,DocumentTypeId,CreatedBy,CreatedOn,OriginalDocumentName)values(" + databaseCon.Decrypt(ClientID) + ",'" + databaseCon.Decrypt(RID) + "','" + NewFileName + "','" + txtdesc.Replace("'", "''").Trim() + "','" + DateTime.Now + "','" + docid + "',(select Emailid from tbluser where UserId='" + databaseCon.Decrypt(UID) + "'),'" + DateTime.Now + "','" + OriginalDocumentName.Replace("'", "''").Trim() + "')";
                    //    int i = databaseCon.RunCmd(query);
                    //    if (i > 0)
                    //    {
                    //        //string path = Server.MapPath(targetfolder);
                    //        string path = targetfolder1;
                    //        if (extension.Replace(".", "") == "pdf")
                    //        {
                    //            extension = "application/pdf";
                    //        }

                    //        file.SaveAs(Server.MapPath("/uploads/") + @"/" + orgfilename);
                    //        string path1 = "/uploads/";
                    //        path1 = Server.MapPath(path1);

                    //        string FileName = path1 + orgfilename;

                    //        string newPath = FileName;

                    //        Image img = Image.FromFile(newPath);

                    //        if (extension.Replace(".", "") == "pdf")
                    //        {
                    //            extension = "application/pdf";
                    //        }

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

                    //            //tempImage = filename;
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

                    //            thumbnailImg.Save(path1 + NewFileName, img12.RawFormat);
                    //        }

                    //        else
                    //        {
                    //            //tempImage = imageData;
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

                    //            thumbnailImg.Save(path1 + NewFileName, img12.RawFormat);
                    //        }

                    //        string fpath = path1 + NewFileName;

                    //        databaseCon.savefileonblob(NewFileName, "Inspection-Document", fpath, extension.Replace(".", ""));

                    //        //System.IO.File.Delete(path);
                    //        //txtdesc.Text = "";
                    //        //docsgrid.Rebind();
                    //        //art1.Style.Add("display", "block");
                    //        //art1.Attributes["class"] = "w3-section w3-card-12 j-close fade in j-alertBox-Success";
                    //        //alertlabel.Text = "<strong>Success ! </strong> Record Saved Successfully";
                    //        Result = "Success";
                    //        Message = "Record Saved Successfully";
                    //    }
                    //    else
                    //    {
                    //        //ViewState["doctype"] = ddldoctype.SelectedValue;
                    //        //art1.Style.Add("display", "block");
                    //        //art1.Attributes["class"] = "w3-section w3-card-12 j-close fade in j-alertBox-Failed";
                    //        //alertlabel.Text = "<strong>Alert ! </strong> Select any file and document type";
                    //        Result = "Failed";
                    //        Message = "Select any file and document type";
                    //    }
                    //}
                    //else
                    //{

                    //End Here (01-11-2022)

                    var ext = extension.Split('.');
                    var Tempextension = ext[1];

                    if (Tempextension.ToLower() == "jpg" || Tempextension.ToLower() == "jpeg" || Tempextension.ToLower() == "png" || Tempextension.ToLower() == "ico" || Tempextension.ToLower() == "gif" || Tempextension.ToLower() == "bmp")
                    {

                        //string docfile = databaseCon.Decrypt(ClientID) + "_" + ddldoctype + "_";
                        //string query2 = "select Max(Cast (Left(REVERSE(SUBSTRING(REVERSE((DocumentName)),0,CHARINDEX('_',REVERSE( (DocumentName))))),CHARINDEX('.',REVERSE(SUBSTRING(REVERSE((DocumentName)),0,CHARINDEX('_',REVERSE( (DocumentName))))))-1) as int ))  as number from tbldocuments where DocumentName like '%" + docfile + "%'order by number";
                        //DataTable dt = databaseCon.GetData(query2);
                        //if (dt.Rows.Count >= 1 && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["number"])) && dt.Rows[0]["number"] != "" && dt.Rows[0]["number"] != "null" && dt.Rows[0]["number"] != null)
                        //{
                        //    //string[] temp = Convert.ToString(dt.Rows[0]["number"]).Split('.');
                        //    //docfile2 = Convert.ToInt32(temp[0]);
                        //    docfile2 = Convert.ToInt32(dt.Rows[0]["number"]) + 1;
                        //    docfile = docfile + docfile2 + extension;
                        //    tempdocfile = databaseCon.Decrypt(ClientID) + "_" + ddldoctype + "_" + docfile2 + DateTime.Now.Ticks + extension;
                        //}
                        //else
                        //{
                        //    docfile = docfile + 1 + extension;
                        //    tempdocfile = databaseCon.Decrypt(ClientID) + "_" + ddldoctype + "_" + 1 + DateTime.Now.Ticks + extension;
                        //}

                        //commented on 07july23 To remove doctype from filename and add Registerid instead
                        //string docfile = databaseCon.Decrypt(ClientID) + "_" + ddldoctype + "_" + DateTime.Now.Ticks + extension;
                        //tempdocfile = databaseCon.Decrypt(ClientID) + "_" + ddldoctype + "_" + 1 + DateTime.Now.Ticks + extension;

                        string docfile = databaseCon.Decrypt(ClientID) + "_" + "Job_" + databaseCon.Decrypt(RID) + "_" + DateTime.Now.Ticks + extension;
                        tempdocfile = databaseCon.Decrypt(ClientID) + "_" + "Job_" + databaseCon.Decrypt(RID) + "_" + 1 + DateTime.Now.Ticks + extension;

                        file.SaveAs(Server.MapPath("/uploads/") + @"/" + docfile);
                        string path = "/uploads/";
                        path = Server.MapPath(path);

                        string FileName = path + docfile;

                        string newPath = FileName;
                        ////commented on  07 march 2025   for  dispose file strime 

                        //Image img = Image.FromFile(newPath);

                        //NewFileName = tempdocfile ;

                        //if (extension.Replace(".", "") == "pdf")
                        //{
                        //    extension = "application/pdf";
                        //}

                        //if (img.PropertyIdList.Contains(0x0112))
                        //{

                        //    PropertyItem propOrientation = img.GetPropertyItem(0x0112);
                        //    short orientation = BitConverter.ToInt16(propOrientation.Value, 0);
                        //    if (orientation == 6)
                        //    {
                        //        img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        //    }
                        //    else if (orientation == 8)
                        //    {
                        //        img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        //    }
                        //    else if (orientation == 3)
                        //    {
                        //        img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        //    }
                        //    else
                        //    {
                        //        // Do nothing
                        //    }

                        //    var newWidth = (int)(img.Width * 0.5);
                        //    var newHeight = (int)(img.Height * 0.5);
                        //    var thumbnailImg = new Bitmap(newWidth, newHeight);
                        //    var thumbGraph = Graphics.FromImage(thumbnailImg);
                        //    thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                        //    thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                        //    thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        //    var imageRectangle = new System.Drawing.Rectangle(0, 0, newWidth, newHeight);

                        //    thumbGraph.DrawImage(img, imageRectangle);

                        //    byte[] bte = GetCompressedBitmap(thumbnailImg, 75);
                        //    MemoryStream ms2 = new MemoryStream(bte);
                        //    System.Drawing.Image img12 = System.Drawing.Image.FromStream(ms2);

                        //    thumbnailImg.Save(path + NewFileName, img12.RawFormat);
                        //}

                        //else
                        //{
                        //    var newWidth = (int)(img.Width * 0.5);
                        //    var newHeight = (int)(img.Height * 0.5);
                        //    var thumbnailImg = new Bitmap(newWidth, newHeight);
                        //    var thumbGraph = Graphics.FromImage(thumbnailImg);
                        //    thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                        //    thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                        //    thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        //    var imageRectangle = new System.Drawing.Rectangle(0, 0, newWidth, newHeight);

                        //    thumbGraph.DrawImage(img, imageRectangle);

                        //    byte[] bte = GetCompressedBitmap(thumbnailImg, 50);
                        //    MemoryStream ms2 = new MemoryStream(bte);
                        //    System.Drawing.Image img12 = System.Drawing.Image.FromStream(ms2);

                        //    thumbnailImg.Save(path + NewFileName, img12.RawFormat);
                        //}
                        using (Image img = Image.FromFile(newPath))
                        {
                            NewFileName = tempdocfile;

                            if (extension.Replace(".", "") == "pdf")
                            {
                                extension = "application/pdf";
                            }

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
                            }

                            int newWidth = (int)(img.Width * 0.5);
                            int newHeight = (int)(img.Height * 0.5);

                            using (Bitmap thumbnailImg = new Bitmap(newWidth, newHeight))
                            {
                                using (Graphics thumbGraph = Graphics.FromImage(thumbnailImg))
                                {
                                    thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                                    thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                                    thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                    var imageRectangle = new System.Drawing.Rectangle(0, 0, newWidth, newHeight);
                                    thumbGraph.DrawImage(img, imageRectangle);
                                }

                                byte[] bte = GetCompressedBitmap(thumbnailImg, 75);
                                using (MemoryStream ms2 = new MemoryStream(bte))
                                {
                                    using (System.Drawing.Image img12 = System.Drawing.Image.FromStream(ms2))
                                    {
                                        thumbnailImg.Save(path + NewFileName, img12.RawFormat);
                                    }
                                }
                            }
                        }
                        string fpath = path + NewFileName;

                        //string query = "insert into tbldocuments(ClientID,RegisterID,DocumentName,Description,DateOfUpload,DocumentTypeId,CreatedBy,CreatedOn)values(" + Request.QueryString["id"] + ",'" + Request.QueryString["RID"] + "','" + docfile + "','" + txtdesc.Text.Replace("'", "''").Trim() + "','" + DateTime.Now + "','" + ddldoctype.SelectedValue + "','" + Request.QueryString["UID"] + "','" + DateTime.Now + "')";
                        string query = "insert into tbldocuments(ClientID,RegisterID,DocumentName,Description,DateOfUpload,DocumentTypeId,CreatedBy,CreatedOn,OriginalDocumentName)values(" + databaseCon.Decrypt(ClientID) + ",'" + databaseCon.Decrypt(RID) + "','" + NewFileName + "','" + txtdesc.Replace("'", "''").Trim() + "','" + DateTime.Now + "','" + docid + "', (select Emailid from tbluser where UserId ='" + databaseCon.Decrypt(UID) + "'),'" + DateTime.Now + "','" + OriginalDocumentName.Replace("'", "''").Trim() + "')";
                        int i = databaseCon.RunCmd(query);
                        if (i > 0)
                        {
                            //string path = targetfolder;
                            if (extension.Replace(".", "") == "pdf")
                            {
                                extension = "application/pdf";
                            }
                            var a = extension.Replace(".", "");

                            databaseCon.savefileonblob(NewFileName, "Inspection-Document", fpath, extension.Replace(".", ""));




                            System.IO.File.Delete(fpath);
                            System.IO.File.Delete(newPath);
                            //txtdesc.Text = "";
                            //docsgrid.Rebind();
                            //art1.Style.Add("display", "block");
                            //art1.Attributes["class"] = "w3-section w3-card-12 j-close fade in j-alertBox-Success";
                            //alertlabel.Text = "<strong>Success ! </strong> Record Saved Successfully";
                            // btnsave.Enabled = true;
                            Result = "Success";
                            Message = "Record Saved Successfully";
                        }
                        else
                        {
                            //ViewState["doctype"] = ddldoctype.SelectedValue;
                            //art1.Style.Add("display", "block");
                            //art1.Attributes["class"] = "w3-section w3-card-12 j-close fade in j-alertBox-Failed";
                            //alertlabel.Text = "<strong>Alert ! </strong> Select any file and document type";
                            Result = "Failed";
                            Message = "Select any file and document type";
                        }
                        //}
                        //Adddocs.Style.Add("display", "none");
                        //ddldoctype.ClearSelection();
                    }

                    else
                    {
                        //commented as we decided to use timetick for all files
                        // string docfile = databaseCon.Decrypt(ClientID) + "_" + ddldoctype + "_";

                        //string query2 = "select Max(Cast (Left(REVERSE(SUBSTRING(REVERSE((DocumentName)),0,CHARINDEX('_',REVERSE( (DocumentName))))),CHARINDEX('.',REVERSE(SUBSTRING(REVERSE((DocumentName)),0,CHARINDEX('_',REVERSE( (DocumentName))))))-1) as int ))  as number from tbldocuments where DocumentName like '%" + docfile + "%'order by number";
                        //DataTable dt = databaseCon.GetData(query2);
                        //if (dt.Rows.Count >= 1 && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["number"])) && dt.Rows[0]["number"] != "" && dt.Rows[0]["number"] != "null" && dt.Rows[0]["number"] != null)
                        //{
                        //    //string[] temp = Convert.ToString(dt.Rows[0]["number"]).Split('.');
                        //    //docfile2 = Convert.ToInt32(temp[0]);
                        //    docfile2 = Convert.ToInt32(dt.Rows[0]["number"]) + 1;
                        //    docfile = docfile + docfile2 + extension;

                        //}
                        //else
                        //{
                        //    docfile = docfile + 1 + extension;

                        //}

                        //commented on 07july23 To remove doctype from filename and add Registerid instead
                        //string docfile = databaseCon.Decrypt(ClientID) + "_" + ddldoctype + "_"  + DateTime.Now.Ticks + extension;


                        string docfile = databaseCon.Decrypt(ClientID) + "_" + "Job_" + databaseCon.Decrypt(RID) + "_" + DateTime.Now.Ticks + extension;

                        //targetfolder = "/uploads/Documents/" + docfile;
                        // fileuplod.SaveAs(Server.MapPath(targetfolder));
                        targetfolder = Path.Combine(Server.MapPath("~/uploads/"), docfile);
                        file.SaveAs(targetfolder);

                        //string query = "insert into tbldocuments(ClientID,RegisterID,DocumentName,Description,DateOfUpload,DocumentTypeId,CreatedBy,CreatedOn)values(" + Request.QueryString["id"] + ",'" + Request.QueryString["RID"] + "','" + docfile + "','" + txtdesc.Text.Replace("'", "''").Trim() + "','" + DateTime.Now + "','" + ddldoctype.SelectedValue + "','" + Request.QueryString["UID"] + "','" + DateTime.Now + "')";
                        string query = "insert into tbldocuments(ClientID,RegisterID,DocumentName,Description,DateOfUpload,DocumentTypeId,CreatedBy,CreatedOn,OriginalDocumentName)values(" + databaseCon.Decrypt(ClientID) + ",'" + databaseCon.Decrypt(RID) + "','" + docfile + "','" + txtdesc.Replace("'", "''").Trim() + "','" + DateTime.Now + "','" + docid + "', (select Emailid from tbluser where UserId ='" + databaseCon.Decrypt(UID) + "'),'" + DateTime.Now + "','" + OriginalDocumentName.Replace("'", "''").Trim() + "')";
                        int i = databaseCon.RunCmd(query);
                        if (i > 0)
                        {

                            // string path = Server.MapPath(targetfolder);
                            string path = targetfolder;
                            if (extension.Replace(".", "") == "pdf")
                            {
                                extension = "application/pdf";
                            }
                            var a = extension.Replace(".", "");
                            databaseCon.savefileonblob(docfile, "Inspection-Document", path, extension.Replace(".", ""));

                            System.IO.File.Delete(path);
                            //txtdesc.Text = "";
                            //docsgrid.Rebind();
                            //art1.Style.Add("display", "block");
                            //art1.Attributes["class"] = "w3-section w3-card-12 j-close fade in j-alertBox-Success";
                            //alertlabel.Text = "<strong>Success ! </strong> Record Saved Successfully";
                            // btnsave.Enabled = true;
                            Result = "Success";
                            Message = "Record Saved Successfully";
                        }
                        else
                        {
                            //ViewState["doctype"] = ddldoctype.SelectedValue;
                            //art1.Style.Add("display", "block");
                            //art1.Attributes["class"] = "w3-section w3-card-12 j-close fade in j-alertBox-Failed";
                            //alertlabel.Text = "<strong>Alert ! </strong> Select any file and document type";
                            Result = "Failed";
                            Message = "Select any file and document type";
                        }
                    }

                }
            }
            var response = new { Result = Result, Message = Message };
            return Json(response, JsonRequestBehavior.AllowGet);
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

    public class DocumentDDlTypeViewModel
    {
        public int DocumentTypeId { get; set; }
        public string documentType { get; set; }
    }

    public class GetDocumentTypeData
    {
        public string documentType { get; set; }
        public int DocumentId { get; set; }
        public string DocumentName { get; set; }
        public string Description { get; set; }
        public DateTime DateOfUpload { get; set; }
        public string DocumentURL { get; set; }

        public string OriginalDocumentName { get; set; }
    }

}