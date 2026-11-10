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
using System.Net;
using System.Data.SqlClient;
using System.Configuration;

namespace Scaffold.Controllers
{
    public class DrawingsController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        SqlConnection con = null;
        // GET: Drawings
        public ActionResult Drawings()
        {
            return View();
        }

        public ActionResult btnSave_Click(string UID, string id, string DIDetailID, string txtdesc, string RID, string tempimg, string hdndocument, string FileUpload1, string hdncapture, string nameget, string OriginalDocumentName)
        {
            string Result = "";
            string Message = "";
            DataTable dt = null;
            bool ExtensionMsg = false;
            bool uploading = false;
            string imageData = tempimg;

            var tempImage = imageData;
            string NewFileName = "";
            //string imageData = tempimg.Value;

            // string FileExtension = FileUpload1.FileName;
            string FileExtension = System.IO.Path.GetExtension(FileUpload1).ToLower();

            if (FileUpload1 != "")
            {
                if (FileExtension != "")
                {
                    var ext = FileExtension.Split('.');
                    var extension = ext[1];
                    if (extension.ToLower() == "jpg" || extension.ToLower() == "png" || extension.ToLower() == "gif" || extension.ToLower() == "ico" || extension.ToLower() == "bmp" || extension.ToLower() == "jpeg" || extension.ToLower() == "jpe" || extension.ToLower() == "tiff" || extension.ToLower() == "tif")
                    {
                        HttpFileCollectionBase PostedFiles = Request.Files;

                        HttpFileCollectionBase files = Request.Files;

                        //foreach (HttpPostedFile postedFile in FileUpload1.PostedFiles)
                        //{
                        // for (int k = 0; k < 1; k++)
                        for (int k = 0; k < files.Count; k++)
                        {
                            HttpPostedFileBase postedFile = files[k];
                            string txtSiteName = "";
                            string fileName = txtSiteName.Replace("'", "''").Replace(" ", "").Trim() + "_" + databaseCon.Decrypt(RID) + "_" + DateTime.Now.Ticks + "." + extension;
                            postedFile.SaveAs(Server.MapPath("~/images/sites") + @"/" + fileName);
                            string path = "/images/sites/";
                            path = Server.MapPath(path);

                            //Added On (01-11-2022) start here

                            string FileName = path + fileName;

                            string newPath = FileName;

                            Image img;
                            try
                            {
                                img = Image.FromFile(newPath);
                            }
                            catch (OutOfMemoryException)
                            {

                                System.IO.File.Delete(newPath);
                                Result = "Failed";
                                Message = "Uploaded file is corrupt or not a valid image.";
                                continue;
                            }

                            //NewFileName = txtSiteName.Replace("'", "''").Replace(" ", "").Trim() + "_" + databaseCon.Decrypt(RID) + "_" + DateTime.Now.Ticks + "." + extension;

                            if (extension == "pdf")
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
                                MemoryStream ms2 = new MemoryStream(bte);
                                System.Drawing.Image img12 = System.Drawing.Image.FromStream(ms2);
                                img.Dispose();
                                thumbnailImg.Save(path + fileName, img12.RawFormat);
                            }
                            else
                            {
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
                                MemoryStream ms2 = new MemoryStream(bte);
                                System.Drawing.Image img12 = System.Drawing.Image.FromStream(ms2);
                                img.Dispose();
                                thumbnailImg.Save(path + fileName, img12.RawFormat);
                            }

                            string fpath = path + fileName;

                            databaseCon.savefileonblob(fileName, "Site-images", fpath, extension);
                            System.IO.File.Delete(fpath);

                            //Added On (01-11-2022) End here

                            string query = "exec Pro_TBLREGISTERGALLERY_SAVE '0','" + databaseCon.Decrypt(RID) + "','" + txtdesc.Replace("'", "''").Trim() + "','" + fileName + "','" + HttpUtility.UrlEncode(databaseCon.Decrypt(UID)) + "','" + DateTime.Now + "','Drawings','" + OriginalDocumentName + "'";
                            dt = databaseCon.GetData(query);
                        }
                        if (dt != null && dt.Rows.Count != 0)
                        {
                            if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                            {
                                uploading = true;
                            }
                            else if (Convert.ToInt32(dt.Rows[0][0]) == -1)
                            {
                                Result = "Failed";
                                Message = dt.Rows[0][1].ToString();
                                //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                                //art1.Style.Add("display", "block");
                                //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();

                            }
                            else
                            {
                                Result = "Failed";
                                Message = dt.Rows[0][1].ToString();
                                //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                                //art1.Style.Add("display", "block");
                                //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                            }
                        }
                        else if (dt == null)
                        {
                            // All files were corrupt/invalid; Result and Message already set in the catch block above.
                        }
                        else
                        {
                            Result = "Failed";
                            Message = "Syntax Error. Contact administrator ";
                            //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                            //art1.Style.Add("display", "block");
                            //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator ";
                        }
                    }

                    else if (extension.ToLower() == "pdf")
                    {
                        HttpFileCollectionBase PostedFiles = Request.Files;

                        HttpFileCollectionBase files = Request.Files;

                        //foreach (HttpPostedFile postedFile in FileUpload1.PostedFiles)
                        //{
                        // for (int k = 0; k < 1; k++)
                        for (int k = 0; k < files.Count; k++)
                        {
                            HttpPostedFileBase postedFile = files[k];
                            string txtSiteName = "";
                            string fileName = txtSiteName.Replace("'", "''").Replace(" ", "").Trim() + "_" + databaseCon.Decrypt(RID) + "_" + DateTime.Now.Ticks + "." + extension;
                            postedFile.SaveAs(Server.MapPath("~/images/sites") + @"/" + fileName);
                            string path = "/images/sites/";
                            path = Server.MapPath(path);

                            string fpath = path + fileName;
                            if (extension == "pdf")
                            {
                                extension = "application/pdf";
                            }
                            databaseCon.savefileonblob(fileName, "Site-images", fpath, extension);
                            System.IO.File.Delete(fpath);

                            string query = "exec Pro_TBLREGISTERGALLERY_SAVE '0','" + databaseCon.Decrypt(RID) + "','" + txtdesc.Replace("'", "''").Trim() + "','" + fileName + "','" + HttpUtility.UrlEncode(databaseCon.Decrypt(UID)) + "','" + DateTime.Now + "','Drawings','" + OriginalDocumentName + "'";
                            dt = databaseCon.GetData(query);
                        }
                        if (dt.Rows.Count != 0)
                        {
                            if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                            {
                                uploading = true;
                            }
                            else if (Convert.ToInt32(dt.Rows[0][0]) == -1)
                            {
                                Result = "Failed";
                                Message = dt.Rows[0][1].ToString();
                                //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                                //art1.Style.Add("display", "block");
                                //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();

                            }
                            else
                            {
                                Result = "Failed";
                                Message = dt.Rows[0][1].ToString();
                                //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                                //art1.Style.Add("display", "block");
                                //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                            }
                        }
                        else
                        {
                            Result = "Failed";
                            Message = "Syntax Error. Contact administrator ";
                            //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                            //art1.Style.Add("display", "block");
                            //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator ";
                        }
                    }

                    else
                    {
                        ExtensionMsg = true;
                    }
                }

            }
            if (ExtensionMsg != false)
            {
                Result = "Failed";
                //Message = "Valid File Extensions are jpg, jpe, jpeg, png, gif, bmp.,pdf,tif,tiff";
                //commented on 24-08-2023
                Message = "Valid File Extensions are jpg, jpe, jpeg, png, gif, bmp,pdf,tif,tiff";
                //Session["ExtensionMsg"] = "ExtensionMsg";
                //Response.Redirect("Drawings.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&Datetagged" + Request.QueryString["Datetagged"] + "&RID=" + Request.QueryString["RID"]);
            }
            if (uploading != false)
            {
                Result = "Success";
                Message = "Image Saved Successfully .";
                //Session["Insert"] = "Insert";
                //Response.Redirect("Drawings.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&Datetagged" + Request.QueryString["Datetagged"] + "&RID=" + Request.QueryString["RID"]);
            }
            var response = new { Result = Result, Message = Message };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult showtImage(string RID)
        {
            //string query = "select FileName,ID,RegisterID,Description from tblRegisterGallery where Type='Drawings' and Substring(FileName, CHARINDEX('.', FileName)+1,LEN(FileName))!= 'pdf' And RegisterId=" + databaseCon.Decrypt(RID);
            string query = "select FileName,ID,RegisterID,Description from tblRegisterGallery where Type='Drawings' and (Substring(FileName, CHARINDEX('.', FileName)+1,LEN(FileName))!= 'pdf' and Substring(FileName, CHARINDEX('.', FileName)+1,LEN(FileName))!= 'tiff' and Substring(FileName, CHARINDEX('.', FileName)+1,LEN(FileName))!= 'tif') And RegisterId=" + databaseCon.Decrypt(RID);
            DataTable dt = databaseCon.GetData(query);
            string html = "<table id='image' style='width:100%'>";
            if (dt.Rows.Count > 0)
            {
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    string imgchk = dt.Rows[j]["FileName"].ToString();
                    if (imgchk != "" && imgchk != "NULL" && imgchk != null)
                    {
                        var uri = new Uri(databaseCon.siteimagelink);
                        var container = new CloudBlobContainer(uri);
                        var blob = container.GetBlockBlobReference(dt.Rows[j]["FileName"].ToString());
                        string file = databaseCon.siteimagelink + dt.Rows[j]["FileName"].ToString();
                        html += "<div class='col-lg-4 col-md-6 col-sm-12 col-xs-12' style='margin-top: 5px;'>";
                        html += "<img id='ppdl" + j + "' runnat='server' class='img-container' src='" + file + "' style='width: 300px;object-fit:contain; height: 250px' />";
                        html += "<div class='row' style='margin-top: -18px;text-align:left'>";
                        html += "<div class='col-lg-12 col-md-12 col-sm-6 col-xs-12'>";
                        html += "<a id='descr" + j + "' style='color:black;font-size:16px;font-weight:600;margin-left:32px'>" + dt.Rows[j]["Description"] + "</a>";
                        html += "</div>";
                        html += "</div>";
                        html += "</div>";
                    }
                }
                html += "</table>";
            }
            return Json(html, JsonRequestBehavior.AllowGet);

        }


        public ActionResult btndeleteyes_Click(string RID, string hdnimgname)
        {
            string Result = "";
            string Message = "";
            if (hdnimgname != "")
            {
                string query = "delete from tblRegisterGallery where FileName='" + hdnimgname + "' and RegisterId=" + databaseCon.Decrypt(RID);
                int dt = databaseCon.RunCmdID(query);
                databaseCon.deletefileonblob(hdnimgname, "siteimage");
                if (dt > 0)
                {
                    hdnimgname = "";
                    //Session["Delete"] = "Delete";
                    //Response.Redirect("Drawings.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&Datetagged" + Request.QueryString["Datetagged"] + "&RID=" + Request.QueryString["RID"]);
                    Result = "Success";
                    Message = "Record Deleted Successfully.";
                }
                else
                {
                    Result = "Failed";
                    Message = "Failed";
                }
            }
            var response = new { Result = Result, Message = Message };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDrawingData(string RID)
        {

            //var GetData = dbContext.Database.SqlQuery<DrawingPDFViewModel>("select FileName as DocumentName,ID,RegisterID,Description from tblRegisterGallery where Type='Drawings' and Substring(FileName, CHARINDEX('.', FileName)+1,LEN(FileName))= 'pdf' And RegisterId= '" + databaseCon.Decrypt(RID)+"'").ToList();
            var GetData = dbContext.Database.SqlQuery<DrawingPDFViewModel>("select FileName as DocumentName,ID,RegisterID,Description,OriginalDocumentName,CreatedOn from tblRegisterGallery where Type='Drawings' and (Substring(FileName, CHARINDEX('.', FileName)+1,LEN(FileName))= 'pdf'   or Substring(FileName, CHARINDEX('.', FileName)+1,LEN(FileName))= 'tiff' or Substring(FileName, CHARINDEX('.', FileName)+1,LEN(FileName))= 'tif')  And RegisterId= '" + databaseCon.Decrypt(RID) + "'").ToList();

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

        public ActionResult DownloadImage(string registerId, string fileName)
        {
            string decryptedId = databaseCon.Decrypt(registerId);
            string ImageName = "";
            string Type = "Drawings";

            con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);

            string query = "SELECT FileName FROM tblRegisterGallery WHERE registerid = '"
                            + decryptedId + "' AND FileName = '" + fileName + "' AND Type = '" + Type + "'";

            DataTable imagename = databaseCon.GetData(query);

            if (imagename.Rows.Count > 0)
            {
                ImageName = imagename.Rows[0]["FileName"].ToString();
            }

            string fpath = databaseCon.siteimagelink + ImageName;

            using (WebClient client = new WebClient())
            {
                byte[] fileBytes = client.DownloadData(fpath);

                return File(
                    fileBytes,
                    System.Net.Mime.MediaTypeNames.Application.Octet,
                    ImageName
                );
            }
        }

        public JsonResult DeleteDrawing(string DocumentId, string RID)
        {

            string Result = "";

            // string query = "delete from  tbldocuments where DocumentId =" + DocumentId;
            string query = "delete from tblRegisterGallery where ID='" + DocumentId + "' and RegisterId=" + databaseCon.Decrypt(RID);
            int i = databaseCon.RunCmd(query);
            if (i > 0)
            {
                Result = "Success";
                //var data = dbContext.tblDocuments.Where(_ => _.DocumentId == DocId).FirstOrDefault();
                //databaseCon.deletefileonblob(data.DocumentName, "registerdocument");
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

    public class DrawingPDFViewModel
    {
        public string DocumentName { get; set; }
        public int ID { get; set; }
        public int RegisterID { get; set; }
        public string Description { get; set; }
        public string DocumentURL { get; set; }
        public string OriginalDocumentName { get; set; }
        public DateTime CreatedOn { get; set; }
    }

}