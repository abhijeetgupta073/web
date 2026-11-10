using Microsoft.WindowsAzure.Storage.Blob;
using Scaffold.App_Code;
using Scaffold_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scaffold.Controllers
{
    public class ScafpacDocumentationController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: Documentation
        public ActionResult ScafpacDocumentation()
        {
            return View();
        }

        public ActionResult SiteDropDown(string id, string UID)
        {
            var SiteOwnerName = "";
            string query1 = "select SiteOwnerName from tblUser where ClientId='" + databaseCon.Decrypt(id) + "' and UserId='" + databaseCon.Decrypt(UID) + "'";
            DataTable dt1 = databaseCon.GetData(query1);
            if (dt1.Rows.Count > 0)
            {
                SiteOwnerName = dt1.Rows[0]["SiteOwnerName"].ToString();
            }
            if (SiteOwnerName != "")
            {
                // Escape single quote by replacing ' with ''
                SiteOwnerName = SiteOwnerName.Replace("'", "''");
                var getSiteLocationData = dbContext.Database.SqlQuery<GetDocumentsTypeDataV2>("select distinct SiteLocation,SiteLocID from tblSiteLocation where SiteLocation <>'' and SiteLocation is not null and  ClientId='" + databaseCon.Decrypt(id) + "' and OwnerName='" + SiteOwnerName + "' and MaintainInventory = 1 and MaintainInventoryAtJob=1  order by SiteLocation").ToList();
                return Json(getSiteLocationData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var getSiteLocationData = dbContext.Database.SqlQuery<GetDocumentsTypeDataV2>("select * from (select 0 as SiteLocID,'All' as SiteLocation union select distinct SiteLocID,SiteLocation from tblSiteLocation where SiteLocation <>'' and SiteLocation is not null and  ClientId = '" + databaseCon.Decrypt(id) + "' and MaintainInventory = 1 ) as sitelisting order by SiteLocation").ToList();
                return Json(getSiteLocationData, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult txtdoctype_TextChanged(string ClientID, string UID, string txtDoctype)
        {
            string Result = "";
            string Message = "";
            string doctype = txtDoctype.Replace("'", "''").Trim();
            if (!string.IsNullOrEmpty(doctype))
            {
                string query = "select documentType from tbldocumenttype where documentType='" + doctype + "' and clientid ='1'";
                DataTable dt = databaseCon.GetData(query);
                if (dt.Rows.Count <= 0)
                {
                    query = " insert into tbldocumenttype(documentType,CreatedBy,CreatedOn,clientid,DocumentCategoryLocation) values('" + doctype.Replace("'", "").Trim() + "',(select Emailid from tbluser where UserId='" + databaseCon.Decrypt(UID) + "'),'" + DateTime.Now + "','1','Site')";
                    int i = databaseCon.RunCmd(query);
                    if (i > 0)
                    {
                        Result = "Success";
                        Message = "Record Saved Successfully";
                    }
                }
                else
                {
                    Result = "Warning";
                    Message = "Docuemnt type already exists...";
                }
            }
            var response = new { Result = Result, Message = Message };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ddlDocumentType(string ClientID, string SiteID)
        {
       
            var GetddlData = dbContext.Database.SqlQuery<DocumentDDlTypeViewModel>("select distinct tblDocumentType.DocumentTypeId, documentType,tblDocumentType.CreatedOn  from  tbldocuments right join tblDocumentType on tblDocuments.DocumentTypeId = tblDocumentType.DocumentTypeId  where tblDocumentType.ClientId ='1'   order by tblDocumentType.CreatedOn desc").ToList();

            return Json(GetddlData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult btnsave_Click(string ClientID, string UID, string SiteID, string ddldoctype, string txtdesc, string browerFile, string OriginalDocumentName)
        {
            string Result = "";
            string Message = "";
            string filename = "C0000000";
            string targetfolder = "";
            string orgfilename = "";
            int docfile2;
            string NewFileName;
            string tempdocfile;


            string docid = databaseCon.getStringCell("select DocumentTypeId from tblDocumentType where tblDocumentType.Clientid = 1 and DocumentType='" + ddldoctype + "'");
          
            if (Request.Files.Count > 0)
            {
               
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

                    //string extension = Path.GetExtension(fileuplod.PostedFile.FileName);
                    string extension = System.IO.Path.GetExtension(filename).ToLower();

                    var ext = extension.Split('.');
                    var Tempextension = ext[1];
                    if (Tempextension.ToLower() == "jpg" || Tempextension.ToLower() == "jpeg" || Tempextension.ToLower() == "png" || Tempextension.ToLower() == "ico" || Tempextension.ToLower() == "gif" || Tempextension.ToLower() == "bmp")
                    {


                        string docfile = ClientID + "_" + "Site_0_" + DateTime.Now.Ticks + extension;
                        tempdocfile =ClientID + "_" + "Site_0_" + 1 + DateTime.Now.Ticks + extension;

                        file.SaveAs(Server.MapPath("/uploads/") + @"/" + docfile);

                        string path = "/uploads/";

                        path = Server.MapPath(path);

                        string FileName = path + docfile;

                        string newPath = FileName;

                        Image img = Image.FromFile(newPath);

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
                            else
                            {
                                // Do nothing
                            }

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
                            MemoryStream ms2 = new MemoryStream(bte);
                            System.Drawing.Image img12 = System.Drawing.Image.FromStream(ms2);

                            thumbnailImg.Save(path + NewFileName, img12.RawFormat);
                        }

                        else
                        {
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
                            MemoryStream ms2 = new MemoryStream(bte);
                            System.Drawing.Image img12 = System.Drawing.Image.FromStream(ms2);

                            thumbnailImg.Save(path + NewFileName, img12.RawFormat);
                        }

                        string fpath = path + NewFileName;

                       
                        string query = "insert into tbldocuments(ClientID,DocumentName,Description,DateOfUpload,DocumentTypeId,CreatedBy,CreatedOn,OriginalDocumentName)values('1','" + NewFileName + "','" + txtdesc.Replace("'", "''").Trim() + "','" + DateTime.Now + "','" + docid + "', (select Emailid from tbluser where UserId ='" + databaseCon.Decrypt(UID) + "'),'" + DateTime.Now + "','" + OriginalDocumentName.Replace("'", "''").Trim() + "')";
                        int i = databaseCon.RunCmd(query);
                        if (i > 0)
                        {
                            //string path = targetfolder;
                            if (extension.Replace(".", "") == "pdf")
                            {
                                extension = "application/pdf";
                            }

                            databaseCon.savefileonblob(NewFileName, "Site-images", fpath, extension.Replace(".", ""));

                            System.IO.File.Delete(fpath);
                         
                            Result = "Success";
                            Message = "Record Saved Successfully";
                        }
                        else
                        {
                        
                            Result = "Failed";
                            Message = "Select any file and document type";
                        }
                      
                    }
                    else
                    {
                       
                        string docfile = "1_" + "Site_0_" + DateTime.Now.Ticks + extension;

                        //targetfolder = "/uploads/Documents/" + docfile;
                        // fileuplod.SaveAs(Server.MapPath(targetfolder));
                        targetfolder = Path.Combine(Server.MapPath("~/uploads/"), docfile);
                        file.SaveAs(targetfolder);

                        string query = "insert into tbldocuments(ClientID,DocumentName,Description,DateOfUpload,DocumentTypeId,CreatedBy,CreatedOn,OriginalDocumentName)values('1','" + docfile + "','" + txtdesc.Replace("'", "''").Trim() + "','" + DateTime.Now + "','" + docid + "', (select Emailid from tbluser where UserId ='" + databaseCon.Decrypt(UID) + "'),'" + DateTime.Now + "','" + OriginalDocumentName.Replace("'", "''").Trim() + "')";
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
                            databaseCon.savefileonblob(docfile, "Site-images", path, extension.Replace(".", ""));

                            System.IO.File.Delete(path);
                          
                            Result = "Success";
                            Message = "Record Saved Successfully";
                        }
                        else
                        {
                           
                            Result = "Failed";
                            Message = "Select any file and document type";
                        }
                    }
                  
                }

            }

            var response = new { Result = Result, Message = Message };
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDocumentTypeData(string ClientID, string SiteID, string CategoryID)
        {
          

            //string query = "select documentType, DocumentId,tblDocuments.ClientID,DocumentName,Description,DateOfUpload,tblDocuments.DocumentTypeId,RegisterID,SiteLocID,OriginalDocumentName from tbldocuments join tblDocumentType on tblDocuments.DocumentTypeId = tblDocumentType.DocumentTypeId  where   tblDocuments.ClientId = 1 order by dateofupload desc";
            string query = "select documentType, DocumentId,tblDocuments.ClientID,DocumentName,Description,DateOfUpload,tblDocuments.DocumentTypeId,RegisterID,SiteLocID,OriginalDocumentName from tbldocuments join tblDocumentType on tblDocuments.DocumentTypeId = tblDocumentType.DocumentTypeId  where   tblDocuments.ClientId = 1 order by  OriginalDocumentName asc,dateofupload desc";

            var GetData = dbContext.Database.SqlQuery<GetDocumentsTypeData>(query).ToList();
            foreach (var item in GetData)
            {
                var DocumentPdfPath = item.DocumentName;
                if (DocumentPdfPath != "")
                {
                    string path = databaseCon.siteimagelink + DocumentPdfPath;
                    var uri = new Uri(databaseCon.siteimagelink);
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

        public JsonResult DeleteDocument(string DocumentId)
        {
            string Result = "";
            string query = "delete from  tbldocuments where DocumentId =" + DocumentId;
            int i = databaseCon.RunCmd(query);
            if (i > 0)
            {
                Result = "Success";
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult btnEditSave(string id, string UID, string hdnDocumentID, string txtdescription, string hdnDocumentName)
        {
            var Result = "";
            if (txtdescription != "")
            {
              
                string query = "update tbldocuments set Description = '" + txtdescription.Replace("'", "''").Trim() + "',UpdatedOn='" + DateTime.Now + "',UpdatedBy=(select Emailid from tbluser where UserId='" + databaseCon.Decrypt(UID) + "')  where  DocumentId='" + hdnDocumentID + "'";
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

public class GetDocumentsTypeDataV2
{
    public Nullable<int> SiteLocID { get; set; }
    public string SiteLocation { get; set; }
    public string documentType { get; set; }
    public int DocumentId { get; set; }
    public string DocumentName { get; set; }
    public string Description { get; set; }
    public DateTime DateOfUpload { get; set; }
    public string DocumentURL { get; set; }
    public string OriginalDocumentName { get; set; }
}