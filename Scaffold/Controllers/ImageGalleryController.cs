using Microsoft.WindowsAzure.Storage.Blob;
using Scaffold.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing.Imaging;
using Org.BouncyCastle.Ocsp;
using System.Net;
using System.Data.SqlClient;
using System.Configuration;

namespace Scaffold.Controllers
{
    public class ImageGalleryController : Controller
    {
        SqlConnection con = null;
        // GET: ImageGallery
        public ActionResult ImageGallery()
        {
            return View();
        }


        //public ActionResult btnSave_Click(string UID, string id, string DIDetailID, string txtdesc, string RID, string UserRole, string tempimg, string hdndocument, string FileUpload1, string hdncapture, string nameget, string OriginalDocumentName)
        //{
        //    string Result = "";
        //    string Message = "";
        //    DataTable dt = null;
        //    bool capturing = false;
        //    bool uploading = false;
        //    string imageData = tempimg;
        //    //string imageData = tempimg.Value;

        //    var tempImage = imageData;
        //    string NewFileName = "";

        //    //string newpath2 = "";
        //    //string QueryU = "update tblRegisterGallery set";
        //    //string FileExtension = FileUpload1.FileName;
        //    string FileExtension = System.IO.Path.GetExtension(FileUpload1).ToLower();
        //    //if (FileUpload1.HasFiles == true)
        //    if (FileUpload1 != "")
        //    {
        //        if (FileExtension != "")
        //        {
        //            var ext = FileExtension.Split('.');
        //            var extension = ext[1];
        //            if (extension.ToLower() == "jpg" || extension.ToLower() == "jpeg" || extension.ToLower() == "png" || extension.ToLower() == "ico" || extension.ToLower() == "gif" || extension.ToLower() == "bmp")
        //            {
        //                HttpFileCollectionBase PostedFiles = Request.Files;

        //                HttpFileCollectionBase files = Request.Files;
        //                //foreach (HttpPostedFile postedFile in FileUpload1.PostedFiles)
        //                //{
        //                // for (int k = 0; k < 1; k++)
        //                for (int k = 0; k < files.Count; k++)
        //                {

        //                    HttpPostedFileBase postedFile = files[k];
        //                    string txtSiteName = "";
        //                    //string fileName = Path.GetFileName(postedFile.FileName);
        //                    string fileName = txtSiteName.Replace("'", "''").Replace(" ", "").Trim() + "_" + databaseCon.Decrypt(RID) + "_" + DateTime.Now.Ticks + "." + extension;
        //                    postedFile.SaveAs(Server.MapPath("~/images/sites") + @"/" + fileName);
        //                    string path = "/images/sites/";
        //                    path = Server.MapPath(path);

        //                    //Added On (01-11-2022) start here
        //                    string FileName = path + fileName ;

        //                    string newPath = FileName;

        //                    Image img = Image.FromFile(newPath);

        //                    NewFileName = txtSiteName.Replace("'", "''").Replace(" ", "").Trim() + "_" + databaseCon.Decrypt(RID) + "_" + DateTime.Now.Ticks + "." + extension;

        //                    if (img.PropertyIdList.Contains(0x0112))
        //                    {

        //                        PropertyItem propOrientation = img.GetPropertyItem(0x0112);
        //                        short orientation = BitConverter.ToInt16(propOrientation.Value, 0);
        //                        if (orientation == 6)
        //                        {
        //                            img.RotateFlip(RotateFlipType.Rotate90FlipNone);
        //                        }
        //                        else if (orientation == 8)
        //                        {
        //                            img.RotateFlip(RotateFlipType.Rotate270FlipNone);
        //                        }
        //                        else if (orientation == 3)
        //                        {
        //                            img.RotateFlip(RotateFlipType.Rotate180FlipNone);
        //                        }
        //                        else
        //                        {
        //                            // Do nothing
        //                        }

        //                        tempImage = imageData;
        //                        //string filename = NewFileName;
        //                        var newWidth = (int)(img.Width * 0.5);
        //                        var newHeight = (int)(img.Height * 0.5);
        //                        var thumbnailImg = new Bitmap(newWidth, newHeight);
        //                        var thumbGraph = Graphics.FromImage(thumbnailImg);
        //                        thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
        //                        thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
        //                        thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //                        var imageRectangle = new System.Drawing.Rectangle(0, 0, newWidth, newHeight);

        //                        thumbGraph.DrawImage(img, imageRectangle);

        //                        byte[] bte = GetCompressedBitmap(thumbnailImg, 75);
        //                        MemoryStream ms2 = new MemoryStream(bte);
        //                        System.Drawing.Image img12 = System.Drawing.Image.FromStream(ms2);

        //                        thumbnailImg.Save(path + NewFileName, img12.RawFormat);

        //                    }

        //                    else
        //                    {
        //                        tempImage = imageData;
        //                        //string filename = NewFileName;
        //                        var newWidth = (int)(img.Width * 0.5);
        //                        var newHeight = (int)(img.Height * 0.5);
        //                        var thumbnailImg = new Bitmap(newWidth, newHeight);
        //                        var thumbGraph = Graphics.FromImage(thumbnailImg);
        //                        thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
        //                        thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
        //                        thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //                        var imageRectangle = new System.Drawing.Rectangle(0, 0, newWidth, newHeight);

        //                        thumbGraph.DrawImage(img, imageRectangle);

        //                        byte[] bte = GetCompressedBitmap(thumbnailImg, 50);
        //                        MemoryStream ms2 = new MemoryStream(bte);
        //                        System.Drawing.Image img12 = System.Drawing.Image.FromStream(ms2);

        //                        thumbnailImg.Save(path + NewFileName, img12.RawFormat);
        //                    }

        //                    string fpath = path + NewFileName;

        //                    databaseCon.savefileonblob(NewFileName, "Site-images", fpath, "image/png");
        //                    System.IO.File.Delete(fpath);

        //                    //Added On (01-11-2022) End here

        //                    string query = "exec Pro_TBLREGISTERGALLERY_SAVE '0','" + databaseCon.Decrypt(RID) + "','" + txtdesc.Replace("'", "''").Trim() + "','" + NewFileName + "','" + HttpUtility.UrlEncode(databaseCon.Decrypt(UID)) + "','" + DateTime.Now + "','ImageGallery','" + OriginalDocumentName + "'";
        //                    dt = databaseCon.GetData(query);
        //                }
        //                if (dt.Rows.Count != 0)
        //                {
        //                    if (Convert.ToInt32(dt.Rows[0][0]) > 0)
        //                    {
        //                        uploading = true;

        //                    }
        //                    else if (Convert.ToInt32(dt.Rows[0][0]) == -1)
        //                    {
        //                        //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
        //                        //art1.Style.Add("display", "block");
        //                        //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();

        //                    }
        //                    else
        //                    {
        //                        //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
        //                        //art1.Style.Add("display", "block");
        //                        //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
        //                    }
        //                }
        //                else
        //                {
        //                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
        //                    //art1.Style.Add("display", "block");
        //                    //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator "
        //                    Result = "Failed";
        //                    Message = "Syntax Error. Contact administrator";
        //                }
        //                Result = "Success";
        //                Message = "Data Saved Successfully";
        //            }
        //            else
        //            {
        //                //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
        //                //art1.Style.Add("display", "block");
        //                //alertlabel.Text = "<strong>Alert ! </strong>Valid File Extensions are jpg, jpeg, png, gif, bmp.";
        //                Result = "Failed";
        //                Message = "Valid File Extensions are jpg, jpeg, png, gif, bmp.";
        //            }
        //        }

        //    }
        //    //if (hdncapture.Value != "" || imageData != "")
        //    if (hdncapture != "" || imageData != "")
        //    {
        //        //hdncapture.Value = "";
        //        //string name = nameget.Value;
        //        hdncapture = "";
        //        string name = nameget;
        //        string path = "~/uploads/";
        //        path = Server.MapPath(path);
        //        string FileName = "";
        //        if (name != "")
        //        {
        //            var extname = name.Split('.');
        //            string extensionname = extname[1];
        //            if (extensionname.ToLower() == "jpg" || extensionname.ToLower() == "jpeg" || extensionname.ToLower() == "png" || extensionname.ToLower() == "ico" || extensionname.ToLower() == "gif" || extensionname.ToLower() == "bmp")
        //            {
        //                FileName = "CaptureImg" + "_" + databaseCon.Decrypt(RID) + "_" + DateTime.Now.Ticks + ".jpg";

        //            }
        //            else
        //            {
        //                //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
        //                //art1.Style.Add("display", "block");
        //                //alertlabel.Text = "<strong>Failed to upload File! </strong>Valid File Extensions Are .jpg, .jpeg, .png, .ico, .gif, .bmp";
        //                //txtdesc.Text = "";
        //                Result = "Failed";
        //                Message = ">Failed to upload File! Valid File Extensions Are .jpg, .jpeg, .png, .ico, .gif, .bmp.";
        //                txtdesc = "";
        //            }
        //        }
        //        else
        //        {
        //            FileName = "CaptureImg" + "_" + databaseCon.Decrypt(Request.QueryString["RID"]) + "_" + DateTime.Now.Ticks + ".jpg";
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
        //                if (System.IO.File.Exists(newpath))
        //                {
        //                    string newPath = path + FileName;

        //                    Image img = Image.FromFile(newPath);

        //                    NewFileName = "SnapImg" + "_" + DateTime.Now.Ticks + "_" + databaseCon.Decrypt(RID) + ".jpg";

        //                    tempImage = imageData;
        //                    //string filename = NewFileName;
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


        //                    string fpath = path + NewFileName;
        //                    databaseCon.savefileonblob(NewFileName, "Site-images", fpath, "image/jpeg");
        //                    //System.IO.File.Delete(fpath);
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
        //                if (System.IO.File.Exists(newpath))
        //                {
        //                    string fpath = path + FileName;
        //                    databaseCon.savefileonblob(FileName, "Site-images", fpath, "image/jpeg");
        //                    //System.IO.File.Delete(fpath);
        //                }
        //            }
        //            string query1 = "exec Pro_TBLREGISTERGALLERY_SAVE '0','" + databaseCon.Decrypt(RID) + "','" + txtdesc.Replace("'", "''").Trim() + "','" + NewFileName + "','" + HttpUtility.UrlEncode(databaseCon.Decrypt(UID)) + "','" + DateTime.Now + "','ImageGallery','" + OriginalDocumentName + "'";
        //            DataTable dt1 = databaseCon.GetData(query1);
        //            if (dt1.Rows.Count != 0)
        //            {
        //                if (Convert.ToInt32(dt1.Rows[0][0]) > 0)
        //                {

        //                    capturing = true;
        //                }
        //                else if (Convert.ToInt32(dt1.Rows[0][0]) == -1)
        //                {
        //                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
        //                    //art1.Style.Add("display", "block");
        //                    //alertlabel.Text = "<strong>Alert ! </strong>" + dt1.Rows[0][1].ToString();

        //                }
        //                else
        //                {
        //                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
        //                    //art1.Style.Add("display", "block");
        //                    //alertlabel.Text = "<strong>Alert ! </strong>" + dt1.Rows[0][1].ToString();
        //                }
        //            }
        //            else
        //            {
        //                //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
        //                //art1.Style.Add("display", "block");
        //                //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator ";
        //                Result = "Failed";
        //                Message = "Syntax Error. Contact administrator";
        //            }

        //        }
        //    }
        //    if (capturing != false || uploading != false)
        //    {
        //        //Session["Insert"] = "Insert";
        //        //Response.Redirect("ImageGallery.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&Datetagged" + Request.QueryString["Datetagged"] + "&RID=" + Request.QueryString["RID"]);
        //    }
        //    var response = new { Result = Result, Message = Message };
        //    return Json(response, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult btnSave_Click(string UID, string id, string DIDetailID, string txtdesc, string RID, string UserRole, string tempimg, string hdndocument, string FileUpload1, string hdncapture, string nameget, string OriginalDocumentName)
        {
            string Result = "";
            string Message = "";
            DataTable dt = null;
            bool capturing = false;
            bool uploading = false;
            string imageData = tempimg;

            var tempImage = imageData;
            string NewFileName = "";
            string newPath = "";


            string FileExtension = System.IO.Path.GetExtension(FileUpload1).ToLower();
            if (FileUpload1 != "")
            {
                try
                {
                    if (FileExtension != "")
                    {
                        var ext = FileExtension.Split('.');
                        var extension = ext[1];
                        if (extension.ToLower() == "jpg" || extension.ToLower() == "jpeg" || extension.ToLower() == "png" || extension.ToLower() == "ico" || extension.ToLower() == "gif" || extension.ToLower() == "bmp")
                        {
                            HttpFileCollectionBase PostedFiles = Request.Files;

                            HttpFileCollectionBase files = Request.Files;

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

                                newPath = FileName;

                                using (Image img = Image.FromFile(newPath))
                                {
                                    NewFileName = txtSiteName.Replace("'", "''").Replace(" ", "").Trim() + "_" + databaseCon.Decrypt(RID) + "_" + DateTime.Now.Ticks + "." + extension;

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

                                    string fpath = path + NewFileName;

                                    databaseCon.savefileonblob(NewFileName, "Site-images", fpath, "image/png");
                                    System.IO.File.Delete(fpath);

                                    //Added On (01-11-2022) End here

                                    string query = "exec Pro_TBLREGISTERGALLERY_SAVE '0','" + databaseCon.Decrypt(RID) + "','" + txtdesc.Replace("'", "''").Trim() + "','" + NewFileName + "','" + HttpUtility.UrlEncode(databaseCon.Decrypt(UID)) + "','" + DateTime.Now + "','ImageGallery','" + OriginalDocumentName + "'";
                                    dt = databaseCon.GetData(query);
                                }


                            }
                            if (dt.Rows.Count != 0)
                            {
                                if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                                {
                                    uploading = true;

                                }
                                else if (Convert.ToInt32(dt.Rows[0][0]) == -1)
                                {


                                }
                                else
                                {

                                }
                            }
                            else
                            {

                                Result = "Failed";
                                Message = "Syntax Error. Contact administrator";
                            }
                            Result = "Success";
                            Message = "Data Saved Successfully";
                        }
                        else
                        {

                            Result = "Failed";
                            Message = "Valid File Extensions are jpg, jpeg, png, gif, bmp.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    Message = ex.Message;

                }
                finally
                {
                    try
                    {
                        if (System.IO.File.Exists(newPath))
                        {
                            System.IO.File.Delete(newPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        Message = ex.Message;
                    }
                }


            }
            if (hdncapture != "" || imageData != "")
            {
                try
                {
                    hdncapture = "";
                    string name = nameget;
                    string path = "~/uploads/";
                    path = Server.MapPath(path);
                    string FileName = "";
                    string newpath = "";
                    if (name != "")
                    {
                        var extname = name.Split('.');
                        string extensionname = extname[1];
                        if (extensionname.ToLower() == "jpg" || extensionname.ToLower() == "jpeg" || extensionname.ToLower() == "png" || extensionname.ToLower() == "ico" || extensionname.ToLower() == "gif" || extensionname.ToLower() == "bmp")
                        {
                            FileName = "CaptureImg" + "_" + databaseCon.Decrypt(RID) + "_" + DateTime.Now.Ticks + ".jpg";

                        }
                        else
                        {
                            Result = "Failed";
                            Message = ">Failed to upload File! Valid File Extensions Are .jpg, .jpeg, .png, .ico, .gif, .bmp.";
                            txtdesc = "";
                        }
                    }
                    else
                    {
                        FileName = "CaptureImg" + "_" + databaseCon.Decrypt(Request.QueryString["RID"]) + "_" + DateTime.Now.Ticks + ".jpg";
                    }
                    if (FileName != "")
                    {
                        newpath = path + FileName;
                        byte[] bytes = Convert.FromBase64String(imageData.Split(',')[1]);
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
                                newPath = path + FileName;

                                using (Image img = Image.FromFile(newPath))
                                {

                                    NewFileName = "SnapImg" + "_" + DateTime.Now.Ticks + "_" + databaseCon.Decrypt(RID) + ".jpg";

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

                                    string fpath = path + NewFileName;
                                    databaseCon.savefileonblob(NewFileName, "Site-images", fpath, "image/jpeg");
                                    System.IO.File.Delete(fpath);
                                }
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
                                databaseCon.savefileonblob(FileName, "Site-images", fpath, "image/jpeg");
                                System.IO.File.Delete(fpath);
                            }
                        }
                        string query1 = "exec Pro_TBLREGISTERGALLERY_SAVE '0','" + databaseCon.Decrypt(RID) + "','" + txtdesc.Replace("'", "''").Trim() + "','" + NewFileName + "','" + HttpUtility.UrlEncode(databaseCon.Decrypt(UID)) + "','" + DateTime.Now + "','ImageGallery','" + OriginalDocumentName + "'";
                        DataTable dt1 = databaseCon.GetData(query1);
                        if (dt1.Rows.Count != 0)
                        {
                            if (Convert.ToInt32(dt1.Rows[0][0]) > 0)
                            {

                                capturing = true;
                            }
                            else if (Convert.ToInt32(dt1.Rows[0][0]) == -1)
                            {

                            }
                            else
                            {

                            }
                        }
                        else
                        {

                            Result = "Failed";
                            Message = "Syntax Error. Contact administrator";
                        }

                    }
                }
                catch (Exception ex)
                {
                    Message = ex.Message;
                }
                finally
                {
                    try
                    {
                        if (System.IO.File.Exists(newPath))
                        {
                            System.IO.File.Delete(newPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        Message = ex.Message;
                    }
                }
            }

            var response = new { Result = Result, Message = Message };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult showtImage(string RID)
        {
            string query = "select FileName,Description from tblRegisterGallery where Type='ImageGallery' and RegisterId=" + databaseCon.Decrypt(RID);
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
                    //Response.Redirect("ImageGallery.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&Datetagged" + Request.QueryString["Datetagged"] + "&RID=" + Request.QueryString["RID"]);
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
        public ActionResult DownloadImage(string registerId, string fileName)
        {
            string decryptedId = databaseCon.Decrypt(registerId);
            string ImageName = "";
            string Type = "ImageGallery";

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
    }
}