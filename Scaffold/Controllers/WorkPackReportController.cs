using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
//using System.Reflection.Metadata;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Services.Description;
using iTextSharp.text;

using iTextSharp.text.pdf;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Scaffold.App_Code;



namespace Scaffold.Controllers
{
    public class WorkPackReportController : Controller
    {
        // GET: WorkPack
        //ScaffoldEntities dbcontext = new ScaffoldEntities();
        float Totalquantity = 0;
        float WeightTotal = 0;
        string Message;

        public static string registerdocument = ConfigurationManager.AppSettings["registerdocument"].ToString();
        public static string siteimage = ConfigurationManager.AppSettings["siteimage"].ToString();

        public ActionResult Index(string SessionName, string SessionValue)
        {
            Session[SessionName] = SessionValue;
            return View();
        }
        public static System.Drawing.Image Base64ToImage(string base64String)
        {
            // Convert base 64 string to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            // Convert byte[] to Image
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);

                return image;
            }
        }
        //public ActionResult showreport(string RegisterID, string ClientID, string hdnTimeOffSet, string SiteID)
        //    {
        //    string finalReportPathName = "";

        //    int id, RID;
        //    RID = Convert.ToInt32(databaseCon.Decrypt(RegisterID));
        //    id = Convert.ToInt32(databaseCon.Decrypt(ClientID));

        //    finalReportPathName = CreateWorkPackReport(RegisterID, ClientID, hdnTimeOffSet, SiteID);


        //    //string query5 = "select documentType, DocumentId, tblDocuments.ClientID,DocumentName,Description,DateOfUpload,tblDocuments.DocumentTypeId,RegisterID,OriginalDocumentName from tbldocuments join tblDocumentType on tblDocuments.DocumentTypeId = tblDocumentType.DocumentTypeId  where RegisterID = " + RID + " and tblDocuments.clientid = " + id + " order by dateofupload desc";
        //    //string query5 = "select documentType, DocumentId, tblDocuments.ClientID,DocumentName,Description,DateOfUpload,tblDocuments.DocumentTypeId,RegisterID,OriginalDocumentName from tbldocuments join tblDocumentType on tblDocuments.DocumentTypeId = tblDocumentType.DocumentTypeId where RegisterID = " + RID + " and tblDocuments.clientid = " + id + " and Substring(DocumentName, CHARINDEX('.', DocumentName)+1,LEN(DocumentName)) = 'pdf'";
        //    string qryforpdf = "select documentType, DocumentId, tblDocuments.ClientID,DocumentName,Description,DateOfUpload,tblDocuments.DocumentTypeId,RegisterID,OriginalDocumentName from tbldocuments join tblDocumentType on tblDocuments.DocumentTypeId = tblDocumentType.DocumentTypeId where RegisterID = " + RID + " and tblDocuments.clientid = " + id + " and (RIGHT(OriginalDocumentName, 4)) = '.pdf'";
        //    DataTable additionaldoc = databaseCon.GetData(qryforpdf);

        //    //string query6 = "select RIGHT(FileName,3) as documentType, FileName as DocumentName,ID,RegisterID,Description,CreatedOn  from tblRegisterGallery where Type = 'Drawings' and Substring(FileName, CHARINDEX('.', FileName)+1,LEN(FileName))= 'pdf' And RegisterId =" + (RID);
        //    //string qryforengineerpdf = "select RIGHT(FileName,3) as documentType, FileName as DocumentName,ID,RegisterID,Description,CreatedOn  from tblRegisterGallery where (RIGHT(FileName,4))='.pdf' And RegisterId =" + (RID);
        //    string qryforengineerpdf = "select RIGHT(FileName,3) as documentType, FileName as DocumentName,ID,RegisterID,Description,CreatedOn  from tblRegisterGallery where (RIGHT(FileName,4))='.pdf' and (Substring(FileName, CHARINDEX('.', FileName)+1,LEN(FileName))!= 'tiff' and Substring(FileName, CHARINDEX('.', FileName)+1,LEN(FileName))!= 'tif') And RegisterId =" + (RID); //Added on 12-July-2022
        //    DataTable pdfdoc = databaseCon.GetData(qryforengineerpdf);

        //    string ExternalPdfName = "";
        //    string path = "";
        //    var pdfpath = "";
        //    var myList = new List<string>();
        //    var PdfName = "";

        //    //PdfName = "WorkPackReport_" + RID + ".pdf";
        //    PdfName = finalReportPathName;
        //    myList.Add((PdfName));

        //    if (additionaldoc.Rows.Count > 0)
        //        {
        //        for (int i = 0; i < additionaldoc.Rows.Count; i++)
        //            {

        //            // code for link
        //            var DocumentPdfPath = (additionaldoc.Rows[i]["DocumentName"].ToString());

        //            ExternalPdfName = DocumentPdfPath;
        //            if (DocumentPdfPath != "")
        //                {
        //                path = databaseCon.siteimagelink;
        //                pdfpath = databaseCon.registerdocumentlink + DocumentPdfPath;
        //                var uri = new Uri(databaseCon.siteimagelink);
        //                var container = new CloudBlobContainer(uri);
        //                var blob = container.GetBlockBlobReference(DocumentPdfPath);
        //                var exists = blob.Exists();
        //                byte[] ResultInfo;
        //                using (var ms = new MemoryStream())
        //                    {
        //                    if (blob.Exists())
        //                        {
        //                        blob.DownloadToStream(ms);
        //                        }
        //                    ResultInfo = ms.ToArray();
        //                    }

        //                File(ResultInfo, System.Net.Mime.MediaTypeNames.Application.Pdf, "NewFile12.pdf");

        //                }

        //            // Add items to the list
        //            myList.Add(ExternalPdfName); // Convert to array

        //            DownloadFileFromBlob(DocumentPdfPath, registerdocument);
        //            }
        //        }

        //    if (pdfdoc.Rows.Count > 0)
        //        {
        //        for (int i = 0; i < pdfdoc.Rows.Count; i++)
        //            {

        //            // code for link
        //            var DocumentPdfPath = (pdfdoc.Rows[i]["DocumentName"].ToString());

        //            ExternalPdfName = DocumentPdfPath;
        //            if (DocumentPdfPath != "")
        //                {
        //                path = databaseCon.siteimagelink;
        //                pdfpath = databaseCon.registerdocumentlink + DocumentPdfPath;
        //                var uri = new Uri(databaseCon.siteimagelink);
        //                var container = new CloudBlobContainer(uri);
        //                var blob = container.GetBlockBlobReference(DocumentPdfPath);
        //                var exists = blob.Exists();
        //                byte[] ResultInfo;
        //                using (var ms = new MemoryStream())
        //                    {
        //                    if (blob.Exists())
        //                        {
        //                        blob.DownloadToStream(ms);
        //                        }
        //                    ResultInfo = ms.ToArray();
        //                    }

        //                File(ResultInfo, System.Net.Mime.MediaTypeNames.Application.Pdf, "NewFile12.pdf");

        //                }

        //            // Add items to the list
        //            myList.Add(ExternalPdfName); // Convert to array

        //            DownloadFileFromBlob(DocumentPdfPath, siteimage);
        //            }
        //        }


        //    var myArray = myList.ToArray();

        //    if (myArray.Length > 1)
        //        {
        //        finalReportPathName = MergePDF(myArray, RID);
        //        }

        //    if (additionaldoc.Rows.Count > 0)
        //        {
        //        for (int i = 0; i < additionaldoc.Rows.Count; i++)
        //            {
        //            var DocumentPdfPath = (additionaldoc.Rows[i]["DocumentName"].ToString());

        //            var outputPdfPath = Path.Combine(Server.MapPath("~/uploads/TempUpload/" + DocumentPdfPath));

        //            if (System.IO.File.Exists(outputPdfPath))
        //                {

        //                System.IO.File.Delete(outputPdfPath);
        //                }

        //            }
        //        }

        //    if (pdfdoc.Rows.Count > 0)
        //        {
        //        for (int i = 0; i < pdfdoc.Rows.Count; i++)
        //            {
        //            var DocumentPdfPath = (pdfdoc.Rows[i]["DocumentName"].ToString());

        //            var outputPdfPath = Path.Combine(Server.MapPath("~/uploads/TempUpload/" + DocumentPdfPath));

        //            if (System.IO.File.Exists(outputPdfPath))
        //                {

        //                System.IO.File.Delete(outputPdfPath);
        //                }

        //            }
        //        }

        //    //  var GetOriginalServerPath = System.Web.HttpContext.Current.Server.MapPath("~/uploads/TempUpload/" + finalReportPathName);

        //    //var GetOriginalServerPath = databaseCon.uploadpathDocument + finalReportPathName;
        //    var GetOriginalServerPath = finalReportPathName;

        //    return Json(GetOriginalServerPath, JsonRequestBehavior.AllowGet);

        //    }

        // commented on 01 march 2024 for  delete pdf after download or open  from the system 
        //public ActionResult showreport(string RegisterID, string ClientID, string hdnTimeOffSet, string SiteID)
        //{
        //    string finalReportPathName = "";
        //    try
        //    {
        //        int id, RID;
        //        RID = Convert.ToInt32(databaseCon.Decrypt(RegisterID));
        //        id = Convert.ToInt32(databaseCon.Decrypt(ClientID));

        //        finalReportPathName = CreateWorkPackReport(RegisterID, ClientID, hdnTimeOffSet, SiteID);

        //        if (finalReportPathName != "Fail")
        //        {
        //            string qryforpdf = "select documentType, DocumentId, tblDocuments.ClientID,DocumentName,Description,DateOfUpload,tblDocuments.DocumentTypeId,RegisterID,OriginalDocumentName from tbldocuments join tblDocumentType on tblDocuments.DocumentTypeId = tblDocumentType.DocumentTypeId where RegisterID = " + RID + " and tblDocuments.clientid = " + id + " and (RIGHT(OriginalDocumentName, 4)) = '.pdf'";
        //            DataTable additionaldoc = databaseCon.GetData(qryforpdf);

        //            string qryforengineerpdf = "select RIGHT(FileName,3) as documentType, FileName as DocumentName,ID,RegisterID,Description,CreatedOn  from tblRegisterGallery where (RIGHT(FileName,4))='.pdf' And RegisterId =" + (RID);
        //            DataTable pdfdoc = databaseCon.GetData(qryforengineerpdf);

        //            string ExternalPdfName = "";
        //            string path = "";
        //            var pdfpath = "";
        //            var myList = new List<string>();
        //            var PdfName = "";

        //            PdfName = finalReportPathName;
        //            myList.Add((PdfName));

        //            if (additionaldoc.Rows.Count > 0)
        //            {
        //                for (int i = 0; i < additionaldoc.Rows.Count; i++)
        //                {

        //                    // code for link
        //                    var DocumentPdfPath = (additionaldoc.Rows[i]["DocumentName"].ToString());

        //                    ExternalPdfName = DocumentPdfPath;
        //                    if (DocumentPdfPath != "")
        //                    {
        //                        path = databaseCon.siteimagelink;
        //                        pdfpath = databaseCon.registerdocumentlink + DocumentPdfPath;
        //                        var uri = new Uri(databaseCon.siteimagelink);
        //                        var container = new CloudBlobContainer(uri);
        //                        var blob = container.GetBlockBlobReference(DocumentPdfPath);
        //                        var exists = blob.Exists();
        //                        byte[] ResultInfo;
        //                        using (var ms = new MemoryStream())
        //                        {
        //                            if (blob.Exists())
        //                            {
        //                                blob.DownloadToStream(ms);
        //                            }
        //                            ResultInfo = ms.ToArray();
        //                        }

        //                        File(ResultInfo, System.Net.Mime.MediaTypeNames.Application.Pdf, "NewFile12.pdf");

        //                    }

        //                    // Add items to the list
        //                    myList.Add(ExternalPdfName); // Convert to array

        //                    DownloadFileFromBlob(DocumentPdfPath, registerdocument);
        //                }
        //            }

        //            if (pdfdoc.Rows.Count > 0)
        //            {
        //                for (int i = 0; i < pdfdoc.Rows.Count; i++)
        //                {

        //                    // code for link
        //                    var DocumentPdfPath = (pdfdoc.Rows[i]["DocumentName"].ToString());

        //                    ExternalPdfName = DocumentPdfPath;
        //                    if (DocumentPdfPath != "")
        //                    {
        //                        path = databaseCon.siteimagelink;
        //                        pdfpath = databaseCon.registerdocumentlink + DocumentPdfPath;
        //                        var uri = new Uri(databaseCon.siteimagelink);
        //                        var container = new CloudBlobContainer(uri);
        //                        var blob = container.GetBlockBlobReference(DocumentPdfPath);
        //                        var exists = blob.Exists();
        //                        byte[] ResultInfo;
        //                        using (var ms = new MemoryStream())
        //                        {
        //                            if (blob.Exists())
        //                            {
        //                                blob.DownloadToStream(ms);
        //                            }
        //                            ResultInfo = ms.ToArray();
        //                        }

        //                        File(ResultInfo, System.Net.Mime.MediaTypeNames.Application.Pdf, "NewFile12.pdf");

        //                    }

        //                    // Add items to the list
        //                    myList.Add(ExternalPdfName); // Convert to array

        //                    DownloadFileFromBlob(DocumentPdfPath, siteimage);
        //                }
        //            }

        //            var myArray = myList.ToArray();

        //            if (myArray.Length > 1)
        //            {
        //                finalReportPathName = MergePDF(myArray, RID);
        //            }

        //            if (additionaldoc.Rows.Count > 0)
        //            {
        //                for (int i = 0; i < additionaldoc.Rows.Count; i++)
        //                {
        //                    var DocumentPdfPath = (additionaldoc.Rows[i]["DocumentName"].ToString());

        //                    var outputPdfPath = Path.Combine(Server.MapPath("~/uploads/TempUpload/" + DocumentPdfPath));

        //                    if (System.IO.File.Exists(outputPdfPath))
        //                    {

        //                        System.IO.File.Delete(outputPdfPath);
        //                    }

        //                }
        //            }

        //            if (pdfdoc.Rows.Count > 0)
        //            {
        //                for (int i = 0; i < pdfdoc.Rows.Count; i++)
        //                {
        //                    var DocumentPdfPath = (pdfdoc.Rows[i]["DocumentName"].ToString());

        //                    var outputPdfPath = Path.Combine(Server.MapPath("~/uploads/TempUpload/" + DocumentPdfPath));

        //                    if (System.IO.File.Exists(outputPdfPath))
        //                    {

        //                        System.IO.File.Delete(outputPdfPath);
        //                    }

        //                }
        //            }

        //            if (finalReportPathName != PdfName)
        //            {
        //                if (System.IO.File.Exists(Path.Combine(Server.MapPath("~/uploads/TempUpload/" + PdfName))))
        //                {

        //                    System.IO.File.Delete(Path.Combine(Server.MapPath("~/uploads/TempUpload/" + PdfName)));
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Message += "\n" + ex.Message;
        //        finalReportPathName = "Fail";
        //    }

        //    var GetOriginalServerPath = finalReportPathName;

        //    var getResponse = new { Result = GetOriginalServerPath, Message = Message };
        //    return Json(getResponse, JsonRequestBehavior.AllowGet);

        //}

        public ActionResult showreport(string RegisterID, string ClientID, string hdnTimeOffSet, string SiteID)
        {
            string finalReportPathName = "";
            string Message = "";
            try
            {
                int id, RID;
                RID = Convert.ToInt32(databaseCon.Decrypt(RegisterID));
                id = Convert.ToInt32(databaseCon.Decrypt(ClientID));

                finalReportPathName = CreateWorkPackReport(RegisterID, ClientID, hdnTimeOffSet, SiteID);

                if (finalReportPathName != "Fail")
                {
                    string qryforpdf = "select documentType, DocumentId, tblDocuments.ClientID,DocumentName,Description,DateOfUpload,tblDocuments.DocumentTypeId,RegisterID,OriginalDocumentName from tbldocuments join tblDocumentType on tblDocuments.DocumentTypeId = tblDocumentType.DocumentTypeId where RegisterID = " + RID + " and tblDocuments.clientid = " + id + " and (RIGHT(OriginalDocumentName, 4)) = '.pdf'";
                    DataTable additionaldoc = databaseCon.GetData(qryforpdf);

                    string qryforengineerpdf = "select RIGHT(FileName,3) as documentType, FileName as DocumentName,ID,RegisterID,Description,CreatedOn  from tblRegisterGallery where (RIGHT(FileName,4))='.pdf' And RegisterId =" + (RID);
                    DataTable pdfdoc = databaseCon.GetData(qryforengineerpdf);

                    string ExternalPdfName = "";
                    string path = "";
                    var pdfpath = "";
                    var myList = new List<string>();
                    var PdfName = "";

                    PdfName = finalReportPathName;
                    myList.Add(PdfName);

                    // Defensive: Only process if rows exist
                    if (additionaldoc != null && additionaldoc.Rows.Count > 0)
                    {
                        for (int i = 0; i < additionaldoc.Rows.Count; i++)
                        {
                            var DocumentPdfPath = additionaldoc.Rows[i]["DocumentName"].ToString();
                            ExternalPdfName = DocumentPdfPath;
                            if (!string.IsNullOrEmpty(DocumentPdfPath))
                            {
                                path = databaseCon.siteimagelink;
                                pdfpath = databaseCon.registerdocumentlink + DocumentPdfPath;
                                var uri = new Uri(databaseCon.siteimagelink);
                                var container = new CloudBlobContainer(uri);
                                var blob = container.GetBlockBlobReference(DocumentPdfPath);
                                // Defensive: Check if blob exists before downloading
                                if (blob.Exists())
                                {
                                    byte[] ResultInfo;
                                    using (var ms = new MemoryStream())
                                    {
                                        blob.DownloadToStream(ms);
                                        ResultInfo = ms.ToArray();
                                    }
                                    // The File() here does nothing; you may want to remove it unless you intend to return a file at this point
                                }
                            }
                            myList.Add(ExternalPdfName);
                            DownloadFileFromBlob(DocumentPdfPath, registerdocument);
                        }
                    }

                    if (pdfdoc != null && pdfdoc.Rows.Count > 0)
                    {
                        for (int i = 0; i < pdfdoc.Rows.Count; i++)
                        {
                            var DocumentPdfPath = pdfdoc.Rows[i]["DocumentName"].ToString();
                            ExternalPdfName = DocumentPdfPath;
                            if (!string.IsNullOrEmpty(DocumentPdfPath))
                            {
                                path = databaseCon.siteimagelink;
                                pdfpath = databaseCon.registerdocumentlink + DocumentPdfPath;
                                var uri = new Uri(databaseCon.siteimagelink);
                                var container = new CloudBlobContainer(uri);
                                var blob = container.GetBlockBlobReference(DocumentPdfPath);
                                if (blob.Exists())
                                {
                                    byte[] ResultInfo;
                                    using (var ms = new MemoryStream())
                                    {
                                        blob.DownloadToStream(ms);
                                        ResultInfo = ms.ToArray();
                                    }
                                    // The File() here does nothing; you may want to remove it unless you intend to return a file at this point
                                }
                            }
                            myList.Add(ExternalPdfName);
                            DownloadFileFromBlob(DocumentPdfPath, siteimage);
                        }
                    }

                    var myArray = myList.ToArray();

                    if (myArray.Length > 1)
                    {
                        finalReportPathName = MergePDF(myArray, RID);
                    }

                    // Delete temp files if they exist
                    if (additionaldoc != null && additionaldoc.Rows.Count > 0)
                    {
                        for (int i = 0; i < additionaldoc.Rows.Count; i++)
                        {
                            var DocumentPdfPath = additionaldoc.Rows[i]["DocumentName"].ToString();
                            var outputPdfPath = Path.Combine(Server.MapPath("~/uploads/TempUpload/" + DocumentPdfPath));
                            if (System.IO.File.Exists(outputPdfPath))
                            {
                                System.IO.File.Delete(outputPdfPath);
                            }
                        }
                    }

                    if (pdfdoc != null && pdfdoc.Rows.Count > 0)
                    {
                        for (int i = 0; i < pdfdoc.Rows.Count; i++)
                        {
                            var DocumentPdfPath = pdfdoc.Rows[i]["DocumentName"].ToString();
                            var outputPdfPath = Path.Combine(Server.MapPath("~/uploads/TempUpload/" + DocumentPdfPath));
                            if (System.IO.File.Exists(outputPdfPath))
                            {
                                System.IO.File.Delete(outputPdfPath);
                            }
                        }
                    }

                    if (finalReportPathName != PdfName)
                    {
                        var pdfPath = Path.Combine(Server.MapPath("~/uploads/TempUpload/" + PdfName));
                        if (System.IO.File.Exists(pdfPath))
                        {
                            System.IO.File.Delete(pdfPath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message += "\n" + ex.Message;
                finalReportPathName = "Fail";
            }

            var GetOriginalServerPath = finalReportPathName;
            var getResponse = new { Result = GetOriginalServerPath, Message = Message };
            return Json(getResponse, JsonRequestBehavior.AllowGet);
        }



        public string CreateWorkPackReport(string RegisterID, string ClientID, string hdnTimeOffSet, string SiteID)
        {
            int id, RID;
            RID = Convert.ToInt32(databaseCon.Decrypt(RegisterID));
            id = Convert.ToInt32(databaseCon.Decrypt(ClientID));
            string query10 = "";
            string query11 = "";
            string query = "";


            string base64Data = "";


            string getimageurl = Session["img"].ToString();
            string getimageurl2 = Session["img2"].ToString();

            if (getimageurl == "data:,")
            {
                getimageurl = null;
            }
            if (getimageurl2 == "data:,")
            {
                getimageurl2 = null;
            }

            Session["img"] = null;
            Session["img2"] = null;

            //Create .NET Image object, please make sure it is not iTextSharp Image object
            System.Drawing.Image MapImage = null;
            System.Drawing.Image FloorImage = null;


            if ((!string.IsNullOrEmpty(getimageurl)) && (getimageurl != "undefined"))
            {
                base64Data = Regex.Match(getimageurl, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
                MapImage = Base64ToImage(base64Data);
            }
            if ((!string.IsNullOrEmpty(getimageurl2)) && (getimageurl2 != "undefined"))
            {
                base64Data = Regex.Match(getimageurl2, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
                FloorImage = Base64ToImage(base64Data);
            }


            string queryheader = "select tblClient.ClientImage,Plans,right('000000' + convert(varchar,tblRegister.ScaffRef),6) as ScaffRef,[Description] from tblClient, tblRegister where tblClient.ClientId = tblRegister.ClientID and tblRegister.registerid = '" + RID + "'";
            DataTable dtheader = databaseCon.GetData(queryheader);



            query = "select optimizedQuantity,MaterialType,ItemCode,ItemDesc,ItemWeight,CAST((optimizedQuantity*ItemWeight) as decimal(18,2)) as TotalWeight from tblOptimizedInventory join tblInventory on tblOptimizedInventory.inventoryid=tblInventory.InventoryID where Registerid=" + RID + "and tblOptimizedInventory.Clientid=" + id + "order by MaterialType,ItemCode";
            DataTable dt10 = databaseCon.GetData(query);


            query11 = "select DeliveryAll.FromId ,DeliveryAll.ToId,MaterialType,ItemCode,ItemDesc , DeliveryAll.InventoryId,cast(DeliveryAll.DeliveryAvailableQuantity as int) as DeliveryAvailableQuantity,isnull(cast(ReturnAll.ReturnALLQuantity as int), 0) as ReturnALLQuantity,isnull(cast(DamagedAll.DamagedAllQuantity as int), 0) as DamagedALLQuantity,cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0) - ISNULL(DamagedAll.DamagedAllQuantity, 0)) as int) as optimizedQuantity,DeliveryAll.ItemWeight,cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0) - ISNULL(DamagedAll.DamagedAllQuantity, 0)) as int) * ItemWeight as TotalWeight  from(select FromId, ToId, MaterialType, ItemCode, ItemDesc, ItemWeight, tblTransactionHistDelivery.InventoryId, isnull(Sum(Quantity), 0) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Site' and[to] = 'Job' and TransType = 'Send' and ToId =" + RID + " group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc, ItemWeight)DeliveryAll left join(select FromId, ToId, tblReturnInventoryDetails.InventoryId, isnull(Sum(Quantity), 0) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site' and FromId =" + RID + " group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.InventoryId = ReturnAll.InventoryId left join(select FromId, ToId, tblDamagedInventoryDetails.InventoryId, ISNULL(Sum(DamagedQuantity), 0) as DamagedAllQuantity from tblDamagedInventoryDetails  join tblDamagedInventory on tblDamagedInventoryDetails.DamagedInvId = tblDamagedInventory.DamagedInvId where [FromName] = 'Job' and[toName] = 'Site' and FromId =" + RID + " group by[FromName],[ToName], FromId, ToId, tblDamagedInventoryDetails.InventoryId) DamagedAll On DeliveryAll.FromId = DamagedAll.ToId and DeliveryAll.InventoryId = DamagedAll.InventoryId where 1 = 1order by MaterialType,ItemCode";
            DataTable dt11 = databaseCon.GetData(query11);


            if (dt10.Rows.Count > 0)
            {
                query10 = "select optimizedQuantity,MaterialType,ItemCode,ItemDesc,ItemWeight,CAST((optimizedQuantity*ItemWeight) as decimal(18,2)) as TotalWeight from tblOptimizedInventory join tblInventory on tblOptimizedInventory.inventoryid=tblInventory.InventoryID where Registerid=" + RID + "and tblOptimizedInventory.Clientid=" + id + "order by MaterialType,ItemCode";

            }
            else if (dt11.Rows.Count > 0)
            {

                query10 = "select DeliveryAll.FromId ,DeliveryAll.ToId,MaterialType,ItemCode,ItemDesc , DeliveryAll.InventoryId,cast(DeliveryAll.DeliveryAvailableQuantity as int) as DeliveryAvailableQuantity,isnull(cast(ReturnAll.ReturnALLQuantity as int), 0) as ReturnALLQuantity,isnull(cast(DamagedAll.DamagedAllQuantity as int), 0) as DamagedALLQuantity,cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0) - ISNULL(DamagedAll.DamagedAllQuantity, 0)) as int) as optimizedQuantity,DeliveryAll.ItemWeight,cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0) - ISNULL(DamagedAll.DamagedAllQuantity, 0)) as int) * ItemWeight as TotalWeight  from(select FromId, ToId, MaterialType, ItemCode, ItemDesc, ItemWeight, tblTransactionHistDelivery.InventoryId, isnull(Sum(Quantity), 0) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Site' and[to] = 'Job' and TransType = 'Send' and ToId =" + RID + " group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc, ItemWeight)DeliveryAll left join(select FromId, ToId, tblReturnInventoryDetails.InventoryId, isnull(Sum(Quantity), 0) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site' and FromId =" + RID + " group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.InventoryId = ReturnAll.InventoryId left join(select FromId, ToId, tblDamagedInventoryDetails.InventoryId, ISNULL(Sum(DamagedQuantity), 0) as DamagedAllQuantity from tblDamagedInventoryDetails  join tblDamagedInventory on tblDamagedInventoryDetails.DamagedInvId = tblDamagedInventory.DamagedInvId where [FromName] = 'Job' and[toName] = 'Site' and FromId =" + RID + " group by[FromName],[ToName], FromId, ToId, tblDamagedInventoryDetails.InventoryId) DamagedAll On DeliveryAll.FromId = DamagedAll.ToId and DeliveryAll.InventoryId = DamagedAll.InventoryId where 1 = 1order by MaterialType,ItemCode";

            }

            else
            {
                //query10 = " select Vw_ThirdPartyInventory.MaterialType,Vw_ThirdPartyInventory.ItemCode,Vw_ThirdPartyInventory.ItemDesc, Quantity as optimizedQuantity ,ItemWeight,(Quantity * ItemWeight) as TotalWeight  from(select * from tblPipelineRequest where JobId = " + RID + ") tblPipelineRequest Left join(select * from Vw_ThirdPartyInventory where clientid =" + id + ") Vw_ThirdPartyInventory on tblPipelineRequest.InventoryID = Vw_ThirdPartyInventory.InventoryId where 1 = 1  and Quantity!= 0 order by MaterialType,ItemCode";

                query10 = " select Vw_ThirdPartyInventory.MaterialType,Vw_ThirdPartyInventory.ItemCode,Vw_ThirdPartyInventory.ItemDesc,tblPipelineRequest.Quantity AS optimizedQuantity,Vw_ThirdPartyInventory.ItemWeight,(tblPipelineRequest.Quantity * Vw_ThirdPartyInventory.ItemWeight) AS TotalWeight FROM (SELECT InventoryID,Quantity FROM tblPipelineRequest WHERE JobId = " + RID + ") tblPipelineRequest LEFT JOIN(SELECT InventoryId,MaterialType,ItemCode,ItemDesc,ItemWeight,ClientId FROM Vw_ThirdPartyInventory WHERE ClientId = =" + id + ") Vw_ThirdPartyInventory ON tblPipelineRequest.InventoryID = Vw_ThirdPartyInventory.InventoryId WHERE tblPipelineRequest.Quantity != 0 ORDER BY Vw_ThirdPartyInventory.MaterialType,Vw_ThirdPartyInventory.ItemCode ";
            }
            DataTable dt = databaseCon.GetData(query10);

            string QueryGetRegisterData = "exec Pro_TBLREGISTER_GET '" + RID + "', '" + id + "', '', '','','',''," + Convert.ToDouble((hdnTimeOffSet)) * (-1) + ",'','','','','',''";
            DataTable dt1 = databaseCon.GetData(QueryGetRegisterData);


            //String query2 = "select FileName,ID,RegisterID,Description from tblRegisterGallery where Type='Drawings' and Substring(FileName, CHARINDEX('.', FileName)+1,LEN(FileName))!= 'pdf' And RegisterId=" + (RID);
            //String imgqry = "select FileName,ID,RegisterID,Description from tblRegisterGallery where ((RIGHT(FileName,4)) !='.pdf')  And RegisterId=" + (RID);
            String imgqry = "select FileName,ID,RegisterID,Description from tblRegisterGallery where ((RIGHT(FileName,4)) !='.pdf') and (Substring(FileName, CHARINDEX('.', FileName)+1,LEN(FileName))!= 'tiff' and Substring(FileName, CHARINDEX('.', FileName)+1,LEN(FileName))!= 'tif') And RegisterId=" + (RID);//Added on 12-July-2022
            DataTable imagedt = databaseCon.GetData(imgqry);

            string imgqryreg = "Select Image,RegisterId,Description from tblregister where Clientid=" + id + "and  RegisterId=" + (RID);
            DataTable gpsphotodt = databaseCon.GetData(imgqryreg);

            //string query5 = "select documentType, DocumentId, tblDocuments.ClientID,DocumentName,Description,DateOfUpload,tblDocuments.DocumentTypeId,RegisterID,OriginalDocumentName from tbldocuments join tblDocumentType on tblDocuments.DocumentTypeId = tblDocumentType.DocumentTypeId  where RegisterID = " + RID + " and tblDocuments.clientid = " + id + " order by dateofupload desc";
            //string query5 = "select documentType, DocumentId, tblDocuments.ClientID,DocumentName,Description,DateOfUpload,tblDocuments.DocumentTypeId,RegisterID,OriginalDocumentName from tbldocuments join tblDocumentType on tblDocuments.DocumentTypeId = tblDocumentType.DocumentTypeId  where RegisterID = " + RID + " and tblDocuments.clientid = " + id + " and(Substring(DocumentName, CHARINDEX('.', DocumentName) + 1, LEN(DocumentName)) like '%doc%' or Substring(DocumentName, CHARINDEX('.', DocumentName) + 1, LEN(DocumentName)) like '%xls%') order by dateofupload desc";
            //string qryforexcelanddoc = "select documentType, DocumentId, tblDocuments.ClientID,DocumentName,Description,DateOfUpload,tblDocuments.DocumentTypeId,RegisterID,OriginalDocumentName from tbldocuments join tblDocumentType on tblDocuments.DocumentTypeId = tblDocumentType.DocumentTypeId  where RegisterID = " + RID + " and tblDocuments.clientid = " + id + " and ((RIGHT(OriginalDocumentName,4)) = '.doc' or (RIGHT(OriginalDocumentName,4)) = 'xlsx' or  (RIGHT(OriginalDocumentName,4)) = '.xls' or (RIGHT(OriginalDocumentName,5))='.tiff' or (RIGHT(OriginalDocumentName,5))='.tif') order by dateofupload desc";
            //above code line comented oon  24-08-2023  by s.sourabh  because not  was adding docx file so we add the ext  
            string qryforexcelanddoc = "select documentType, DocumentId, tblDocuments.ClientID,DocumentName,Description,DateOfUpload,tblDocuments.DocumentTypeId,RegisterID,OriginalDocumentName from tbldocuments join tblDocumentType on tblDocuments.DocumentTypeId = tblDocumentType.DocumentTypeId  where RegisterID = " + RID + " and tblDocuments.clientid = " + id + " and ((RIGHT(OriginalDocumentName,4)) = '.doc' or (RIGHT(OriginalDocumentName,5)) = '.docx' or (RIGHT(OriginalDocumentName,4)) = 'xlsx' or  (RIGHT(OriginalDocumentName,4)) = '.xls' or (RIGHT(OriginalDocumentName,5))='.tiff' or (RIGHT(OriginalDocumentName,5))='.tif') order by dateofupload desc";

            DataTable additionaldoc = databaseCon.GetData(qryforexcelanddoc);

            //Added on 12-July-2022 start here
            string drawingtiff = ("select  case when (OriginalDocumentName is null or OriginalDocumentName='') then FileName else  OriginalDocumentName end  as DocumentName,FileName,ID,RegisterID,Description,Type,CreatedOn,OriginalDocumentName from tblRegisterGallery where Type='Drawings' and (Substring(FileName, CHARINDEX('.', FileName)+1,LEN(FileName))= 'tiff' or Substring(FileName, CHARINDEX('.', FileName)+1,LEN(FileName))= 'tif') And RegisterId= '" + RID + "'");
            DataTable drawingtifflsit = databaseCon.GetData(drawingtiff);

            //Added on 12-July-2022 end here



            //string qryforpdfdoc = "select RIGHT(FileName,3) as documentType, FileName as DocumentName,ID,RegisterID,Description,CreatedOn  from tblRegisterGallery where (RIGHT(FileName,4) = '.pdf') And RegisterId =" + (RID);
            //string qryforpdfdoc = "select RIGHT(FileName,3) as documentType, FileName as DocumentName,ID,RegisterID,Description,CreatedOn  from tblRegisterGallery where (RIGHT(FileName,4) = '.pdf') And RegisterId =" + (RID);
            string qryforpdfdoc = "select RIGHT(FileName,3) as documentType, FileName as DocumentName,ID,RegisterID,Description,CreatedOn  from tblRegisterGallery where (RIGHT(FileName,4) = '.pdf') and (Substring(FileName, CHARINDEX('.', FileName)+1,LEN(FileName))!= 'tiff' and Substring(FileName, CHARINDEX('.', FileName)+1,LEN(FileName))!= 'tif') And RegisterId =" + (RID);
            DataTable pdfdoc = databaseCon.GetData(qryforpdfdoc);

            //string query7 = "select documentType, DocumentId, tblDocuments.ClientID,DocumentName,Description,DateOfUpload,tblDocuments.DocumentTypeId,RegisterID,OriginalDocumentName from tbldocuments join tblDocumentType on tblDocuments.DocumentTypeId = tblDocumentType.DocumentTypeId  where RegisterID = " + RID + " and tblDocuments.clientid = " + id + " and  (Substring(DocumentName, CHARINDEX('.', DocumentName)+1,LEN(DocumentName)) like '%jpg%' or Substring(DocumentName, CHARINDEX('.', DocumentName)+1,LEN(DocumentName)) like '%png%' or  Substring(DocumentName, CHARINDEX('.', DocumentName)+1,LEN(DocumentName)) like '%gif%' or Substring(DocumentName, CHARINDEX('.', DocumentName)+1,LEN(DocumentName)) like '%jpeg%' ) order by dateofupload desc";
            string qryforimgdoc = "select documentType, DocumentId, tblDocuments.ClientID,DocumentName,Description,DateOfUpload,tblDocuments.DocumentTypeId,RegisterID,OriginalDocumentName from tbldocuments join tblDocumentType on tblDocuments.DocumentTypeId = tblDocumentType.DocumentTypeId  where RegisterID = " + RID + " and tblDocuments.clientid = " + id + " and  ((RIGHT(OriginalDocumentName,4))='.png' or (RIGHT(OriginalDocumentName,4))='.jpg' or (RIGHT(OriginalDocumentName,4))='.gif' or (RIGHT(OriginalDocumentName,4))='jpeg' ) order by dateofupload desc";
            DataTable imagedt7 = databaseCon.GetData(qryforimgdoc);

            //string query8 = "select documentType, DocumentId, tblDocuments.ClientID,DocumentName,Description,DateOfUpload,tblDocuments.DocumentTypeId,RegisterID,OriginalDocumentName from tbldocuments join tblDocumentType on tblDocuments.DocumentTypeId = tblDocumentType.DocumentTypeId  where RegisterID = " + RID + " and tblDocuments.clientid = " + id + " and  (Substring(DocumentName, CHARINDEX('.', DocumentName)+1,LEN(DocumentName)) like '%jpg%' or Substring(DocumentName, CHARINDEX('.', DocumentName)+1,LEN(DocumentName)) like '%png%' or Substring(DocumentName, CHARINDEX('.', DocumentName)+1,LEN(DocumentName)) like '%pdf%') order by dateofupload desc";
            string qryforimgandpdftbale = "select documentType, DocumentId, tblDocuments.ClientID,DocumentName,Description,DateOfUpload,tblDocuments.DocumentTypeId,RegisterID,OriginalDocumentName from tbldocuments join tblDocumentType on tblDocuments.DocumentTypeId = tblDocumentType.DocumentTypeId  where RegisterID = " + RID + " and tblDocuments.clientid = " + id + " and  ((RIGHT(OriginalDocumentName,4))='.png' or (RIGHT(OriginalDocumentName,4))='.jpg' or (RIGHT(OriginalDocumentName,4))='jpeg' or (RIGHT(OriginalDocumentName,4))='.gif' or (RIGHT(OriginalDocumentName,4))='.pdf'  ) order by dateofupload desc";
            DataTable pdfdocimg = databaseCon.GetData(qryforimgandpdftbale);

            string qryforgpscondition = "select GPS from tblSiteLocation where ClientId = " + id + "and SiteLocID = " + SiteID;
            DataTable dtgpscondition = databaseCon.GetData(qryforgpscondition);




            string PdfName = "";

            iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, -50f, -50f, 20f, 20f);   //Page Size set top ,left,margin
                                                                                                                              // doc.SetPageSize(iTextSharp.text.PageSize.A4);



            PdfName = "WorkPackReport_" + RID + "" + DateTime.Now.Ticks + ".pdf";

            String path1 = Server.MapPath("~/uploads/TempUpload/" + PdfName);
            string rootFolder = Server.MapPath("~/PDFFiles/");
            string authorsFile = PdfName;
            var DeleteFilePath = Path.Combine(Server.MapPath("~/uploads/TempUpload/"), authorsFile);
            if (System.IO.File.Exists(DeleteFilePath))
            {

                System.IO.File.Delete(DeleteFilePath);
            }
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(path1, FileMode.Create));





            //            iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, Response.OutputStream);
            Response.ContentType = ("application/pdf");
            HeaderFooter2 evnt = new HeaderFooter2();
            writer.PageEvent = evnt;
            evnt.RID = RID;
            doc.Open();
            var Heading786 = FontFactory.GetFont("Arial", 10f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            //Heading786.Color = Color.GRAY;
            var Heading787 = FontFactory.GetFont("Arial", 15f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            var DataFont = FontFactory.GetFont("Arial", 10f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            Phrase phrase;
            PdfPCell cell;

            PdfPTable headertable1 = new PdfPTable(5);
            headertable1.SetWidths(new int[] { 10, 9, 25, 9, 9 });

            //Table Start

            PdfPTable headertable2 = new PdfPTable(6);
            headertable2.HeaderRows = 6;
            headertable2.SetWidths(new int[] { 10, 9, 20, 9, 9, 9 });

            PdfPTable headertable3 = new PdfPTable(4);
            headertable3.SetWidths(new int[] { 10, 9, 10, 9 });


            PdfPTable headertable4 = new PdfPTable(2);
            headertable4.SetWidths(new int[] { 50, 50 });

            PdfPTable headertable5 = new PdfPTable(1);
            headertable5.SetWidths(new int[] { 100 });
            //Added on 07-Jan-2022 start here 
            PdfPTable headertable6 = new PdfPTable(4);
            headertable6.SetWidths(new int[] { 36, 15, 35, 14 });

            PdfPTable headertable7 = new PdfPTable(4);
            headertable7.SetWidths(new int[] { 25, 25, 25, 25 });

            PdfPTable headertable8 = new PdfPTable(4);
            headertable8.SetWidths(new int[] { 36, 15, 35, 14 });

            PdfPTable headertable9 = new PdfPTable(2);
            headertable9.SetWidths(new int[] { 50, 50 });

            PdfPTable headertable12 = new PdfPTable(1);
            headertable12.SetWidths(new int[] { 50 });

            PdfPTable headertable13 = new PdfPTable(1);
            headertable13.SetWidths(new int[] { 50 });

            PdfPTable headertable10 = new PdfPTable(4);
            headertable10.SetWidths(new int[] { 36, 15, 35, 14 });

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable3.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk("", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.PaddingBottom = 6f;
            //cell.Border = 0;
            ////cell.PaddingTop = -50f;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //headertable3.AddCell(cell);
            //image start here 
            //if (imagedt.Rows.Count > 0)
            //{
            //    for (int j = 0; j < imagedt.Rows.Count; j++)
            //    {
            //        if (!string.IsNullOrEmpty(Convert.ToString(imagedt.Rows[j]["FileName"])))
            //        {
            //            var uri = new Uri(databaseCon.siteimagelink);
            //            var container = new CloudBlobContainer(uri);
            //            var blob = container.GetBlockBlobReference(imagedt.Rows[0]["FileName"].ToString());
            //            var noimage = System.Web.HttpContext.Current.Server.MapPath("~/images/noimg.jpg");

            //            if (Convert.ToString(imagedt.Rows[0]["FileName"]) == "")
            //            {
            //                string nophoto = System.Web.HttpContext.Current.Server.MapPath("~/images/noimg.jpg");
            //                cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
            //                cell.FixedHeight = 50f;
            //                cell.Colspan = 1;
            //                //cell.PaddingBottom = 15f;
            //                cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //                //cell.PaddingRight = 35f;
            //                cell.PaddingTop = -10;
            //            }

            //            else
            //            {

            //                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(databaseCon.siteimagelink + imagedt.Rows[j]["FileName"].ToString());
            //                //iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance("https://inpexdocumenttest.blob.core.windows.net/siteimage/_609_637789830565711469.png");
            //                img.ScaleAbsolute(180f, 150f);
            //                cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img), true);
            //                cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //                cell.FixedHeight = 100f;
            //                cell.Colspan = 1;
            //                //cell.PaddingBottom = 15f;
            //                //cell.PaddingRight = 35f;
            //                cell.PaddingTop = 10;
            //                cell.BorderColor = BaseColor.WHITE;
            //                headertable3.AddCell(cell);

            //            }

            //        }

            //        else
            //        {
            //            phrase = new Phrase();
            //            phrase.Add(new Chunk("", DataFont));
            //            cell = new PdfPCell(phrase);
            //            cell.Colspan = 1;
            //            cell.Border = 3;
            //            cell.PaddingBottom = 6F;
            //            //cell.PaddingTop = -50f;
            //            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            //            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //            cell.PaddingTop = 10;
            //            headertable3.AddCell(cell);
            //        }


            //    }
            //}

            //else
            //{
            //    string nophoto = System.Web.HttpContext.Current.Server.MapPath("~/images/noimg.jpg");
            //    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
            //    cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //    cell.FixedHeight = 100f;
            //    cell.Colspan = 1;
            //    //cell.PaddingBottom = 15f;
            //    //cell.PaddingRight = 35f;
            //    cell.PaddingTop = 10;
            //    cell.BorderColor = BaseColor.WHITE;
            //    headertable3.AddCell(cell);
            //}

            //clinet image start here
            if (!string.IsNullOrEmpty(Convert.ToString(dtheader.Rows[0]["ClientImage"])))
            {
                var uri = new Uri(databaseCon.clientimageLink);
                // var container = new CloudBlobContainer(uri);
                //var blob = container.GetBlockBlobReference(dtheader.Rows[0]["ClientImage"].ToString());
                var noimage = System.Web.HttpContext.Current.Server.MapPath("~/Images/Logo2.png");
                //var exists = blob.Exists();
                if (Convert.ToString(dtheader.Rows[0]["ClientImage"]) == "")
                {
                    //string nophoto = System.Web.HttpContext.Current.Server.MapPath("~/images/Logo2.png");
                    //cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
                    cell = new PdfPCell();
                    cell.FixedHeight = 50f;
                    cell.Colspan = 1;
                    //cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //cell.PaddingRight = 35f;
                    cell.PaddingTop = -10;
                    cell.BorderColor = BaseColor.WHITE;
                    headertable3.AddCell(cell);
                }
                else
                {
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(System.Web.HttpContext.Current.Server.MapPath("~/images/clientlogo/" + dtheader.Rows[0]["ClientImage"].ToString()));

                    img.ScaleAbsolute(180f, 133f);
                    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img), true);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.FixedHeight = 50f;
                    cell.Colspan = 1;
                    //cell.PaddingBottom = 15f;
                    //cell.PaddingRight = 35f;
                    cell.PaddingTop = -10;
                    cell.BorderColor = BaseColor.WHITE;
                    headertable3.AddCell(cell);

                }
            }
            else
            {
                string nophoto = System.Web.HttpContext.Current.Server.MapPath("~/images/Logo2.png");
                cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
                cell.FixedHeight = 50f;
                //cell.PaddingLeft = 10f;
                //cell.PaddingBottom = 15f;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Colspan = 1;
                cell.PaddingTop = -10;
                cell.BorderColor = BaseColor.WHITE;
                headertable3.AddCell(cell);
            }
            //clinet image end here
            // image end here 



            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Work Pac Report - Scafpac", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 2;
            cell.PaddingTop = 20;
            cell.PaddingBottom = 20;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 4;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 4;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            headertable3.AddCell(cell);


            phrase = new Phrase();
            phrase.Add(new Chunk("General Job Information", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 4;
            cell.PaddingTop = 10;
            cell.PaddingBottom = 10;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 4;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            headertable3.AddCell(cell);

            //scaf ref start here 

            phrase = new Phrase();
            phrase.Add(new Chunk("Scaf Ref #", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            //cell.PaddingTop = -50f;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable3.AddCell(cell);

            if (dtheader.Rows.Count > 0)
            {
                for (int i = 0; i < dtheader.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dtheader.Rows[i]["ScaffRef"].ToString()), Heading787));
                    cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 5f;
                    //cell.PaddingTop = 8f;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;

                    cell.Colspan = 1;
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
                cell.Colspan = 1;
                cell.Border = 3;
                cell.PaddingBottom = 6F;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }

            phrase = new Phrase();
            phrase.Add(new Chunk(" Owner Ref ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable3.AddCell(cell);

            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt1.Rows[i]["ClientRef"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
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
                cell.Colspan = 1;
                cell.Border = 3;
                cell.PaddingBottom = 6F;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }


            //phrase = new Phrase();
            //phrase.Add(new Chunk(" ", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.PaddingBottom = 6f;
            //cell.Border = 0;
            ////cell.PaddingTop = -50f;
            //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //headertable3.AddCell(cell);

            // scaf ref end here 


            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" WO Number ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable3.AddCell(cell);

            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt1.Rows[i]["WONumber"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
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
                cell.Colspan = 1;
                cell.Border = 3;
                cell.PaddingBottom = 6F;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }


            phrase = new Phrase();
            phrase.Add(new Chunk(" SAP Number ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable3.AddCell(cell);

            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt1.Rows[i]["SAPNumber"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
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
                cell.Colspan = 1;
                cell.Border = 3;
                cell.PaddingBottom = 6F;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }



            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Site", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            //cell.PaddingTop = -50f;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable3.AddCell(cell);

            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt1.Rows[i]["SiteLocation"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 5f;
                    //cell.PaddingTop = 8f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;

                    cell.Colspan = 3;
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
                cell.Colspan = 3;
                cell.Border = 3;
                cell.PaddingBottom = 6F;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }


            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Location", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            //cell.PaddingTop = -50f;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable3.AddCell(cell);

            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt1.Rows[i]["Location"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 5f;
                    //cell.PaddingTop = 8f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;

                    cell.Colspan = 3;
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
                cell.Colspan = 3;
                cell.Border = 3;
                cell.PaddingBottom = 6F;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }

            //phrase = new Phrase();
            //phrase.Add(new Chunk("", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.PaddingBottom = 6f;
            //cell.Border = 0;
            ////cell.PaddingTop = -50f;
            //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //headertable3.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk(" SAP Number ", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.PaddingBottom = 6f;
            //cell.Border = 0;
            ////cell.PaddingTop = -50f;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //headertable3.AddCell(cell);

            //if (dt1.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dt1.Rows.Count; i++)
            //    {
            //        phrase = new Phrase();
            //        phrase.Add(new Chunk(Convert.ToString(dt1.Rows[i]["SAPNumber"].ToString()), DataFont));
            //        cell = new PdfPCell(phrase);
            //        //cell.PaddingBottom = 15f;
            //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //        cell.Border = 1;
            //        cell.Colspan = 1;
            //        //cell.PaddingTop = -50f;
            //        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            //        headertable3.AddCell(cell);


            //    }
            //}
            //else
            //{
            //    phrase = new Phrase();
            //    phrase.Add(new Chunk("", DataFont));
            //    cell = new PdfPCell(phrase);
            //    cell.Colspan = 1;
            //    cell.Border = 3;
            //    cell.PaddingBottom = 6F;
            //    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            //    cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //    headertable3.AddCell(cell);
            //}
            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Building / Module #", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            //cell.PaddingTop = -50f;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable3.AddCell(cell);

            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt1.Rows[i]["BuildingNo"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 5f;
                    cell.PaddingTop = 8f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;

                    cell.Colspan = 1;
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
                cell.Colspan = 1;
                cell.Border = 3;
                cell.PaddingBottom = 6F;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }

            //phrase = new Phrase();
            //phrase.Add(new Chunk("", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.PaddingBottom = 6f;
            //cell.Border = 0;
            ////cell.PaddingTop = -50f;
            //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" Asset # / Func.Loc ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable3.AddCell(cell);

            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt1.Rows[i]["AssetNo"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
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
                cell.Colspan = 1;
                cell.Border = 3;
                cell.PaddingBottom = 6F;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }
            //phrase = new Phrase();
            //phrase.Add(new Chunk(" Planner Group ", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.PaddingBottom = 6f;
            //cell.Border = 0;
            ////cell.PaddingTop = -50f;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //headertable3.AddCell(cell);

            //if (dt1.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dt1.Rows.Count; i++)
            //    {
            //        phrase = new Phrase();
            //        phrase.Add(new Chunk(Convert.ToString(dt1.Rows[i]["ClientRef"].ToString()), DataFont));
            //        cell = new PdfPCell(phrase);
            //        //cell.PaddingBottom = 15f;
            //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //        cell.Border = 1;
            //        cell.Colspan = 1;
            //        //cell.PaddingTop = -50f;
            //        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            //        headertable3.AddCell(cell);


            //    }
            //}
            //else
            //{
            //    phrase = new Phrase();
            //    phrase.Add(new Chunk("", DataFont));
            //    cell = new PdfPCell(phrase);
            //    cell.Colspan = 1;
            //    cell.Border = 3;
            //    cell.PaddingBottom = 6F;
            //    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            //    cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //    headertable3.AddCell(cell);
            //}

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Floor Level", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            //cell.PaddingTop = -50f;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable3.AddCell(cell);

            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt1.Rows[i]["FloorLevel"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 5f;
                    //cell.PaddingTop = 8f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;

                    cell.Colspan = 1;
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
                cell.Colspan = 1;
                cell.Border = 3;
                cell.PaddingBottom = 6F;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }

            //phrase = new Phrase();
            //phrase.Add(new Chunk("", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.PaddingBottom = 6f;
            //cell.Border = 0;
            ////cell.PaddingTop = -50f;
            //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" Bearing ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable3.AddCell(cell);

            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt1.Rows[i]["Bearing"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
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
                cell.Colspan = 1;
                cell.Border = 3;
                cell.PaddingBottom = 6F;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Design Type ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable3.AddCell(cell);
            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt1.Rows[i]["DesignType"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
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
                cell.Colspan = 1;
                cell.Border = 3;
                cell.PaddingBottom = 6F;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }

            phrase = new Phrase();
            phrase.Add(new Chunk(" Duty Rating ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable3.AddCell(cell);

            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt1.Rows[i]["DutyRating"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
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
                cell.Colspan = 1;
                cell.Border = 3;
                cell.PaddingBottom = 6F;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Material Type ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable3.AddCell(cell);

            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt1.Rows[i]["MaterialType"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
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
                cell.Colspan = 3;
                cell.Border = 1;
                cell.PaddingBottom = 6F;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }

            phrase = new Phrase();
            phrase.Add(new Chunk(" # Working Decks  ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable3.AddCell(cell);

            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt1.Rows[i]["NoOfWorkingDecks"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
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
                cell.Colspan = 3;
                cell.Border = 1;
                cell.PaddingBottom = 6F;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }


            //phrase = new Phrase();
            //phrase.Add(new Chunk("", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 6;
            //cell.PaddingBottom = 6f;
            //cell.Border = 0;
            ////cell.PaddingTop = -50f;
            //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //headertable3.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk(" ", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 6;
            //cell.PaddingBottom = 6f;
            //cell.Border = 0;
            ////cell.PaddingTop = -50f;
            //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Items ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable3.AddCell(cell);

            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt1.Rows[i]["NoOfItems"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
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
                cell.Colspan = 3;
                cell.Border = 1;
                cell.PaddingBottom = 6F;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }

            phrase = new Phrase();
            phrase.Add(new Chunk(" Tonnage  ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable3.AddCell(cell);

            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt1.Rows[i]["Tonnage"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
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
                cell.Colspan = 3;
                cell.Border = 1;
                cell.PaddingBottom = 6F;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }


            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Engineered Design?", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            //cell.PaddingTop = -50f;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable3.AddCell(cell);

            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt1.Rows[i]["EngineeredDesign"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 5f;
                    //cell.PaddingTop = 8f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;

                    cell.Colspan = 1;
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
                cell.Colspan = 1;
                cell.Border = 3;
                cell.PaddingBottom = 6F;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }

            //phrase = new Phrase();
            //phrase.Add(new Chunk("", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.PaddingBottom = 6f;
            //cell.Border = 0;
            ////cell.PaddingTop = -50f;
            //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" Design Drawing # ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable3.AddCell(cell);

            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt1.Rows[i]["DesignDrawingNo"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
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
                cell.Colspan = 1;
                cell.Border = 3;
                cell.PaddingBottom = 6F;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Max # of Decks at one time ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable3.AddCell(cell);

            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt1.Rows[i]["NumberOfPlatform"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
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
                cell.Colspan = 1;
                cell.Border = 3;
                cell.PaddingBottom = 6F;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }

            phrase = new Phrase();
            phrase.Add(new Chunk(" Height TWP (m) ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable3.AddCell(cell);

            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt1.Rows[i]["Height"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
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
                cell.Colspan = 1;
                cell.Border = 3;
                cell.PaddingBottom = 6F;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 4;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 4;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Length (m)", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable3.AddCell(cell);

            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt1.Rows[i]["Length"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 5f;
                    //cell.PaddingTop = 8f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;

                    cell.Colspan = 1;
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
                cell.Colspan = 1;
                cell.Border = 3;
                cell.PaddingBottom = 6F;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }

            //phrase = new Phrase();
            //phrase.Add(new Chunk("", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.PaddingBottom = 6f;
            //cell.Border = 0;
            ////cell.PaddingTop = -50f;
            //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" Width (m) ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable3.AddCell(cell);

            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt1.Rows[i]["Width"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable3.AddCell(cell);


                }
            }

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 4;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Inspection Time Frame", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            //cell.PaddingTop = -50f;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable3.AddCell(cell);

            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt1.Rows[i]["InspTimeFrame"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 5f;
                    //cell.PaddingTop = 8f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;

                    cell.Colspan = 1;
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
                cell.Colspan = 1;
                cell.Border = 3;
                cell.PaddingBottom = 6F;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }

            //phrase = new Phrase();
            //phrase.Add(new Chunk("", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.PaddingBottom = 6f;
            //cell.Border = 0;
            ////cell.PaddingTop = -50f;
            //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" Planned Standing Days ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable3.AddCell(cell);

            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt1.Rows[i]["PlannedStandingDuration"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
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
                cell.Colspan = 1;
                cell.Border = 3;
                cell.PaddingBottom = 6F;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Reason For Access", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable3.AddCell(cell);
            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt1.Rows[i]["Description"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 3;
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
                cell.Colspan = 3;
                cell.Border = 3;
                cell.PaddingBottom = 6F;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }

            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Comments", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable3.AddCell(cell);
            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt1.Rows[i]["Comments"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 3;
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
                cell.Colspan = 3;
                cell.Border = 3;
                cell.PaddingBottom = 6F;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }

            //end of code

            if (!string.IsNullOrEmpty(Convert.ToString(dtheader.Rows[0]["ClientImage"])))
            {
                var uri = new Uri(databaseCon.clientimageLink);
                // var container = new CloudBlobContainer(uri);
                //var blob = container.GetBlockBlobReference(dtheader.Rows[0]["ClientImage"].ToString());
                var noimage = System.Web.HttpContext.Current.Server.MapPath("~/Images/Logo2.png");
                //var exists = blob.Exists();
                if (Convert.ToString(dtheader.Rows[0]["ClientImage"]) == "")
                {
                    //string nophoto = System.Web.HttpContext.Current.Server.MapPath("~/images/Logo2.png");
                    //cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
                    cell = new PdfPCell();
                    cell.FixedHeight = 50f;
                    cell.Colspan = 2;
                    //cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //cell.PaddingRight = 35f;
                    cell.PaddingTop = -10;
                }
                else
                {
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(System.Web.HttpContext.Current.Server.MapPath("~/images/clientlogo/" + dtheader.Rows[0]["ClientImage"].ToString()));

                    img.ScaleAbsolute(180f, 133f);
                    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img), true);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.FixedHeight = 50f;
                    cell.Colspan = 2;
                    //cell.PaddingBottom = 15f;
                    //cell.PaddingRight = 35f;
                    cell.PaddingTop = -10;

                }
            }
            else
            {
                string nophoto = System.Web.HttpContext.Current.Server.MapPath("~/images/Logo2.png");
                cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
                cell.FixedHeight = 50f;
                //cell.PaddingLeft = 10f;
                //cell.PaddingBottom = 15f;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Colspan = 2;
                cell.PaddingTop = -10;
            }
            cell.Border = 0;
            cell.Colspan = 2;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.FixedHeight = 50f;
            //cell.PaddingBottom = 15f;
            //cell.PaddingLeft = 20f;
            cell.PaddingTop = -10;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingTop = 10;
            cell.Border = 0;
            //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable2.AddCell(cell);

            if (dt10.Rows.Count > 0)
            {

                phrase = new Phrase();
                phrase.Add(new Chunk("Job Material List (Finalised)", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 3;
                //cell.PaddingTop = 6f;
                //cell.PaddingBottom = 6f;
                //cell.PaddingLeft = 100f;
                cell.PaddingTop = 10;
                cell.PaddingBottom = 10;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable2.AddCell(cell);

            }
            else if (dt11.Rows.Count > 0)
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("Job Material List", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 3;
                //cell.PaddingTop = 6f;
                //cell.PaddingBottom = 6f;
                //cell.PaddingLeft = 100f;
                cell.PaddingTop = 10;
                cell.PaddingBottom = 10;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable2.AddCell(cell);

            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("Job Material List (Pipeline)", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 3;
                //cell.PaddingTop = 6f;
                //cell.PaddingBottom = 6f;
                //cell.PaddingLeft = 100f;
                cell.PaddingTop = 10;
                cell.PaddingBottom = 10;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable2.AddCell(cell);

            }

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            headertable2.AddCell(cell);



            //start table
            phrase = new Phrase();
            phrase.Add(new Chunk("Scaf Ref #", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            //cell.PaddingTop = -50f;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            headertable2.AddCell(cell);

            if (dtheader.Rows.Count > 0)
            {
                for (int i = 0; i < dtheader.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dtheader.Rows[i]["ScaffRef"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 5f;
                    //cell.PaddingTop = 8f;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;

                    cell.Colspan = 1;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 3;
                cell.PaddingBottom = 6F;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);
            }

            //phrase = new Phrase();
            //phrase.Add(new Chunk("", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.PaddingBottom = 6f;
            //cell.Border = 0;
            ////cell.PaddingTop = -50f;
            //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Reason for Access: ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable2.AddCell(cell);

            if (dtheader.Rows.Count > 0)
            {
                for (int i = 0; i < dtheader.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dtheader.Rows[i]["Description"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 4;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);


                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.Border = 3;
                cell.PaddingBottom = 6F;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);
            }

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable2.AddCell(cell);


            phrase = new Phrase();
            phrase.Add(new Chunk("Material Type", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.Border = 1;
            cell.PaddingTop = 10f;
            cell.PaddingBottom = 6f;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;

            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Item Code", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.PaddingTop = 10f;
            cell.Border = 1;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Description", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingTop = 10f;
            cell.Border = 1;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Quantity", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.Border = 1;
            cell.PaddingTop = 10f;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);


            phrase = new Phrase();
            phrase.Add(new Chunk("Item Weight", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.Border = 1;
            cell.PaddingTop = 10f;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);


            phrase = new Phrase();
            phrase.Add(new Chunk("Total Weight", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.Border = 1;
            cell.PaddingTop = 10f;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["MaterialType"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.PaddingTop = 2f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);



                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["ItemCode"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["ItemDesc"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    phrase = new Phrase();
                    string qty = String.Format("{0:#,##0}", dt.Rows[i]["optimizedQuantity"]);
                    phrase.Add(new Chunk(qty, DataFont));
                    //phrase.Add(new Chunk(Convert.ToInt32(dt.Rows[i]["Quantity"]).ToString(), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);
                    Totalquantity += Convert.ToInt32(dt.Rows[i]["optimizedQuantity"]);
                    //cell.BackgroundColor = Color.LIGHT_GRAY;

                    phrase = new Phrase();
                    string itemweight = String.Format("{0:#,##0.00}", dt.Rows[i]["ItemWeight"]);
                    phrase.Add(new Chunk(itemweight, DataFont));
                    //phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["ItemWeight"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    phrase = new Phrase();
                    string Totalweight = String.Format("{0:#,##0.00}", dt.Rows[i]["TotalWeight"]);
                    phrase.Add(new Chunk(Totalweight, DataFont));
                    //phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["TotalWeight"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["TotalWeight"])))
                    {
                        WeightTotal += Convert.ToInt32(dt.Rows[i]["TotalWeight"]);
                    }

                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 6;
                cell.Border = 0;
                cell.PaddingBottom = 6F;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

            }
            phrase = new Phrase();
            phrase.Add(new Chunk("", DataFont));
            cell = new PdfPCell(phrase);
            cell.Colspan = 2;
            cell.PaddingBottom = 6F;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Total:", DataFont));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.Border = 3;
            cell.PaddingBottom = 6F;
            cell.Border = PdfPCell.RIGHT_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            string Totalqty = string.Format("{0:#,##0}", Totalquantity);
            phrase.Add(new Chunk(Totalqty.ToString(), DataFont));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.Border = 3;
            cell.PaddingBottom = 6F;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", DataFont));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6F;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            decimal Total = Convert.ToDecimal(WeightTotal / 1000);
            string TotalWe = string.Format("{0:#,##0.00}", Total);
            phrase.Add(new Chunk(TotalWe.ToString(), DataFont));
            phrase.Add(new Chunk(" Tonnes", DataFont));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.Border = 3;
            cell.PaddingBottom = 6F;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            headertable2.AddCell(cell);

            //code for drawing start here
            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 2;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            headertable4.AddCell(cell);


            //phrase = new Phrase();
            //phrase.Add(new Chunk("Engineered Drawing", Heading787));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            ////cell.PaddingTop = 6f;
            ////cell.PaddingBottom = 6f;
            ////cell.PaddingLeft = 100f;
            //cell.PaddingTop = 10;
            //cell.PaddingBottom = 10;
            //cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            //headertable4.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 2;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            headertable4.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 2;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            headertable4.AddCell(cell);

            if (imagedt.Rows.Count > 0)
            {
                for (int j = 0; j < imagedt.Rows.Count; j++)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(imagedt.Rows[j]["FileName"])))
                    {
                        var uri = new Uri(databaseCon.siteimagelink);
                        var container = new CloudBlobContainer(uri);
                        var blob = container.GetBlockBlobReference(imagedt.Rows[0]["FileName"].ToString());
                        var noimage = System.Web.HttpContext.Current.Server.MapPath("~/images/noimg.jpg");

                        if (Convert.ToString(imagedt.Rows[0]["FileName"]) == "")
                        {
                            //string nophoto = System.Web.HttpContext.Current.Server.MapPath("~/images/noimg.jpg");
                            //cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
                            cell = new PdfPCell();
                            cell.FixedHeight = 50f;
                            cell.Colspan = 1;
                            //cell.PaddingBottom = 15f;
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            //cell.PaddingRight = 35f;
                            cell.PaddingTop = -10;
                        }

                        else
                        {

                            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(databaseCon.siteimagelink + imagedt.Rows[j]["FileName"].ToString());

                            WebClient wc = new WebClient();
                            byte[] bytes = wc.DownloadData(databaseCon.siteimagelink + imagedt.Rows[j]["FileName"].ToString());
                            MemoryStream ms = new MemoryStream(bytes);
                            System.Drawing.Image img1 = System.Drawing.Image.FromStream(ms);
                            //iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance("https://inpexdocumenttest.blob.core.windows.net/siteimage/_609_637789830565711469.png");
                            img.ScaleAbsolute(180f, 150f);
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
                                byte[] bte = CompressedImageSave(img1);
                                MemoryStream ms2 = new MemoryStream(bte);
                                System.Drawing.Image img12 = System.Drawing.Image.FromStream(ms2);
                                cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img12, BaseColor.WHITE), true);
                                //cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img1, BaseColor.WHITE), true);
                            }
                            else
                            {
                                cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img), true);
                            }
                            //cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img), true);
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            //cell.FixedHeight = 100f;
                            cell.Colspan = 1;
                            cell.Padding = 10;
                            cell.Border = 0;
                            cell.PaddingTop = 10;
                            cell.BorderColor = BaseColor.WHITE;
                            headertable4.AddCell(cell);

                        }

                    }

                    else
                    {
                        phrase = new Phrase();
                        phrase.Add(new Chunk("", DataFont));
                        cell = new PdfPCell(phrase);
                        cell.Colspan = 1;
                        cell.Border = 3;
                        cell.PaddingBottom = 6F;
                        //cell.PaddingTop = -50f;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.PaddingTop = 10;
                        headertable4.AddCell(cell);
                    }


                }
            }

            else
            {
                //string nophoto = System.Web.HttpContext.Current.Server.MapPath("~/images/noimg.jpg");
                //cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.FixedHeight = 100f;
                cell.Colspan = 1;
                //cell.PaddingBottom = 15f;
                //cell.PaddingRight = 35f;
                cell.PaddingTop = 10;
                cell.BorderColor = BaseColor.WHITE;
                headertable4.AddCell(cell);
            }

            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingTop = 10;
            cell.Border = 0;
            //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable4.AddCell(cell);

            //code for drawing end here
            //gps location

            //phrase = new Phrase();
            //phrase.Add(new Chunk(" ", Heading787));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.PaddingTop = 10;
            //cell.Border = 0;
            ////cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //headertable5.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk(" ", Heading787));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.PaddingTop = 10;
            //cell.Border = 0;
            ////cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //headertable5.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk(" ", Heading787));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.PaddingTop = 10;
            //cell.Border = 0;
            ////cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //headertable5.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingTop = 10;
            cell.Border = 0;
            //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable5.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Image", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingTop = 10;
            cell.PaddingBottom = 10;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable5.AddCell(cell);


            if (Convert.ToString(gpsphotodt.Rows[0]["image"]) != "")
            //if (gpsphotodt.Rows.Count > 0)
            {
                //for (int j = 0; j < gpsphotodt.Rows.Count; j++)
                //{
                if (!string.IsNullOrEmpty(Convert.ToString(gpsphotodt.Rows[0]["image"])))
                {
                    var uri = new Uri(databaseCon.siteimagelink);
                    var container = new CloudBlobContainer(uri);
                    var blob = container.GetBlockBlobReference(gpsphotodt.Rows[0]["image"].ToString());
                    var noimage = System.Web.HttpContext.Current.Server.MapPath("~/images/noimg.jpg");

                    if (Convert.ToString(gpsphotodt.Rows[0]["image"]) == "")
                    {
                        //string nophoto = System.Web.HttpContext.Current.Server.MapPath("~/images/noimg.jpg");
                        //cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
                        cell = new PdfPCell();
                        cell.FixedHeight = 50f;
                        cell.Colspan = 1;
                        //cell.PaddingBottom = 15f;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        //cell.PaddingRight = 35f;
                        cell.PaddingTop = -10;
                    }

                    else
                    {
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(System.Web.HttpContext.Current.Server.MapPath("/images/sites/" + gpsphotodt.Rows[0]["image"].ToString()));


                        WebClient wc = new WebClient();
                        byte[] bytes = wc.DownloadData(System.Web.HttpContext.Current.Server.MapPath("/images/sites/" + gpsphotodt.Rows[0]["image"].ToString()));
                        MemoryStream ms = new MemoryStream(bytes);
                        System.Drawing.Image img1 = System.Drawing.Image.FromStream(ms);


                        // iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance("https://inpexdocumenttest.blob.core.windows.net/siteimage/Test1_607_637793178658161472.jpg");
                        //img.ScaleAbsolute(180f, 150f);
                        //img.SetAbsolutePosition(0, 0); // set the position to bottom left corner of pdf
                        img.SetAbsolutePosition(30, 30); // set the position to bottom left corner of pdf
                        var imgheight = img.Height;
                        var imgwitdth = img.Width;
                        if (imgheight < imgwitdth)
                        {
                            img.ScaleAbsolute(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height - 70F); // set the height and width of image to PDF page size
                        }
                        else
                        {
                            img.ScaleAbsolute(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height - 100F); // set the height and width of image to PDF page size
                        }

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

                            byte[] bte = CompressedImageSave(img1);
                            MemoryStream ms2 = new MemoryStream(bte);
                            System.Drawing.Image img12 = System.Drawing.Image.FromStream(ms2);
                            cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img12, BaseColor.WHITE), true);
                            //cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img1, BaseColor.WHITE), true);
                        }
                        else
                        {
                            cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img), true);
                        }
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.FixedHeight = 700f;
                        //cell.FixedHeight = iTextSharp.text.PageSize.A4.Height - 20F;
                        //cell.TotalWidth = 400f;
                        cell.Colspan = 1;
                        //cell.PaddingBottom = 15f;
                        //cell.PaddingRight = 35f;
                        cell.PaddingTop = 10;
                        cell.BorderColor = BaseColor.WHITE;
                        headertable5.AddCell(cell);

                    }

                }


                else
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("", DataFont));
                    cell = new PdfPCell(phrase);
                    cell.Colspan = 1;
                    cell.Border = 3;
                    cell.PaddingBottom = 6F;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.PaddingTop = 10;

                }


                //}
            }


            else
            {
                //string nophoto = System.Web.HttpContext.Current.Server.MapPath("~/images/noimg.jpg");
                //cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
                cell = new PdfPCell();
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.FixedHeight = 100f;
                cell.Colspan = 1;
                //cell.PaddingBottom = 15f;
                //cell.PaddingRight = 35f;
                cell.PaddingTop = 10;
                cell.BorderColor = BaseColor.WHITE;
                headertable5.AddCell(cell);
            }

            if (Convert.ToString(dtgpscondition.Rows[0]["GPS"]) == "True")
            {



                phrase = new Phrase();
                phrase.Add(new Chunk("Scaffold GPS", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.PaddingTop = 10;
                cell.PaddingBottom = 10;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                headertable12.AddCell(cell);


                if (Convert.ToString(dt1.Rows[0]["Lat"]) != "" && Convert.ToString(dt1.Rows[0]["Long"]) != "")
                //if (gpsimagedt.Rows.Count > 0)
                {
                    //for (int j = 0; j < gpsimagedt.Rows.Count; j++)
                    //{
                    //if (!string.IsNullOrEmpty(Convert.ToString(gpsimagedt.Rows[j]["GPSCapture"])))
                    //{
                    //var uri = new Uri(databaseCon.siteimagelink);
                    //var container = new CloudBlobContainer(uri);
                    //var blob = container.GetBlockBlobReference(gpsimagedt.Rows[0]["GPSCapture"].ToString());
                    ////var noimage = System.Web.HttpContext.Current.Server.MapPath("~/images/noimg.jpg");
                    //var noimage = System.Web.HttpContext.Current.Server.MapPath("~/images/nogps.png");

                    //if (Convert.ToString(gpsimagedt.Rows[0]["GPSCapture"]) == "")
                    //{
                    //    string nophoto = System.Web.HttpContext.Current.Server.MapPath("~/images/nogps.png");
                    //    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
                    //    cell.FixedHeight = 300f;
                    //    cell.Colspan = 1;
                    //    //cell.PaddingBottom = 15f;
                    //    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //    //cell.PaddingRight = 35f;
                    //    cell.PaddingTop = -10;
                    //}

                    //else
                    //{

                    iTextSharp.text.Image GoogleMapImage = iTextSharp.text.Image.GetInstance(MapImage, System.Drawing.Imaging.ImageFormat.Jpeg);
                    //img.Alignment = Element.ALIGN_LEFT;
                    GoogleMapImage.ScaleToFit(500f, 800f);

                    phrase = new Phrase();
                    phrase.Add(new Chunk(GoogleMapImage, 0, 0, true));
                    cell = new PdfPCell(phrase);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.PaddingTop = 40f;
                    //cell.PaddingLeft = -70f;
                    cell.Border = 0;
                    cell.Colspan = 1;
                    headertable12.AddCell(cell);


                    // }

                    //}


                    //else
                    //{
                    //    phrase = new Phrase();
                    //    phrase.Add(new Chunk("", DataFont));
                    //    cell = new PdfPCell(phrase);
                    //    cell.Colspan = 1;
                    //    cell.Border = 3;
                    //    cell.PaddingBottom = 6F;
                    //    //cell.PaddingTop = -50f;
                    //    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    //    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //    cell.PaddingTop = 10;

                    //}


                    //}

                }



                else
                {
                    //string nophoto = System.Web.HttpContext.Current.Server.MapPath("~/images/noimg.jpg");
                    string nophoto = System.Web.HttpContext.Current.Server.MapPath("~/images/nogps.png");
                    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.FixedHeight = 300f;
                    cell.Colspan = 1;
                    //cell.PaddingBottom = 15f;
                    //cell.PaddingRight = 35f;
                    cell.PaddingTop = 10;
                    cell.BorderColor = BaseColor.WHITE;
                    headertable12.AddCell(cell);


                }


                phrase = new Phrase();
                phrase.Add(new Chunk(" ", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.PaddingTop = 10;
                cell.PaddingBottom = 10;
                cell.Border = 0;
                //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                headertable12.AddCell(cell);


                //Lat Long start here

                phrase = new Phrase();
                phrase.Add(new Chunk("Latitude", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.PaddingTop = 10;
                cell.PaddingBottom = 10;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                headertable7.AddCell(cell);



                if (dt1.Rows.Count > 0)
                {
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        phrase = new Phrase();
                        phrase.Add(new Chunk(Convert.ToString(dt1.Rows[i]["Lat"].ToString()), DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingTop = 10;
                        cell.PaddingBottom = 10;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;

                        cell.Colspan = 1;
                        //cell.PaddingTop = -50f;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable7.AddCell(cell);

                    }
                }
                else
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("", DataFont));
                    cell = new PdfPCell(phrase);
                    cell.Colspan = 1;
                    cell.Border = 3;
                    cell.PaddingBottom = 6F;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    headertable7.AddCell(cell);
                }


                phrase = new Phrase();
                phrase.Add(new Chunk("Longitude", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.PaddingTop = 10;
                cell.PaddingBottom = 10;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                headertable7.AddCell(cell);

                if (dt1.Rows.Count > 0)
                {
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        phrase = new Phrase();
                        phrase.Add(new Chunk(Convert.ToString(dt1.Rows[i]["Long"].ToString()), DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingTop = 10;
                        cell.PaddingBottom = 10;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;

                        cell.Colspan = 1;
                        //cell.PaddingTop = -50f;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable7.AddCell(cell);

                    }
                }
                else
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("", DataFont));
                    cell = new PdfPCell(phrase);
                    cell.Colspan = 1;
                    cell.Border = 3;
                    cell.PaddingBottom = 6F;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    headertable7.AddCell(cell);
                }

                phrase = new Phrase();
                phrase.Add(new Chunk(" ", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.Border = 0;
                headertable7.AddCell(cell);

                //Lat end here

                phrase = new Phrase();
                phrase.Add(new Chunk(" ", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.PaddingTop = 10;
                cell.Border = 0;
                //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                headertable5.AddCell(cell);

            }
            else
            {
                //floor image start here 
                phrase = new Phrase();
                phrase.Add(new Chunk("Floor Image", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.PaddingTop = 10;
                cell.PaddingBottom = 10;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                headertable13.AddCell(cell);


                //if (Convert.ToString(dt1.Rows[0]["Lat"]) != "" && Convert.ToString(dt1.Rows[0]["Long"]) != "")
                //{

                //Floor image start here
                iTextSharp.text.Image FloorMapImage = iTextSharp.text.Image.GetInstance(FloorImage, System.Drawing.Imaging.ImageFormat.Jpeg);
                //img.Alignment = Element.ALIGN_LEFT;
                FloorMapImage.ScaleToFit(500f, 800f);
                //FloorMapImage.SetAbsolutePosition(0, 0); // set the position to bottom left corner of pdf
                //FloorMapImage.ScaleAbsolute(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height); // set the height and width of image to PDF page size




                phrase = new Phrase();
                phrase.Add(new Chunk(FloorMapImage, 0, 0, true));
                cell = new PdfPCell(phrase);
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.PaddingTop = 40f;
                //cell.PaddingLeft = -70f;
                cell.Border = 0;
                cell.Colspan = 1;
                headertable13.AddCell(cell);

            }

            //}



            //else
            //{
            //    //string nophoto = System.Web.HttpContext.Current.Server.MapPath("~/images/noimg.jpg");
            //    string nophoto = System.Web.HttpContext.Current.Server.MapPath("~/images/nogps.png");
            //    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
            //    cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //    cell.FixedHeight = 300f;
            //    cell.Colspan = 1;
            //    //cell.PaddingBottom = 15f;
            //    //cell.PaddingRight = 35f;
            //    cell.PaddingTop = 10;
            //    cell.BorderColor = BaseColor.WHITE;
            //    headertable13.AddCell(cell);


            //}


            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingTop = 10;
            cell.PaddingBottom = 10;
            cell.Border = 0;
            //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable13.AddCell(cell);
            //floor image code end here

            //Code for additonal Document start here 
            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 2;
            cell.Border = 0;
            //cell.PaddingTop = 10;
            //cell.PaddingBottom = 10;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable6.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 2;
            cell.Border = 0;
            //cell.PaddingTop = 10;
            //cell.PaddingBottom = 10;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable6.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Engineered Drawing & Additional Document  Links", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 4;
            cell.PaddingTop = 10;
            cell.PaddingBottom = 10;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable6.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 4;
            cell.Border = 0;
            headertable6.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Document Name", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingTop = 10;
            cell.PaddingBottom = 10;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable6.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" Type", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingTop = 10;
            cell.PaddingBottom = 10;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable6.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Description", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingTop = 10;
            cell.PaddingBottom = 10;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable6.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Date of Upload", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingTop = 10;
            cell.PaddingBottom = 10;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable6.AddCell(cell);


            //Loop start here 

            if (additionaldoc.Rows.Count > 0)
            {
                for (int i = 0; i < additionaldoc.Rows.Count; i++)
                {
                    var pdfpath = "";
                    //code for link
                    var DocumentPdfPath = (additionaldoc.Rows[i]["DocumentName"].ToString());
                    if (DocumentPdfPath != "")
                    {
                        string path = databaseCon.registerdocumentlink + DocumentPdfPath;
                        pdfpath = databaseCon.registerdocumentlink + DocumentPdfPath;
                        var uri = new Uri(databaseCon.registerdocumentlink);
                        var container = new CloudBlobContainer(uri);
                        var blob = container.GetBlockBlobReference(DocumentPdfPath);
                        var exists = blob.Exists();
                        if (exists == false)
                        {

                        }
                        else
                        {

                        }
                    }

                    phrase = new Phrase();
                    var anchor = new Chunk(Convert.ToString(additionaldoc.Rows[i]["OriginalDocumentName"].ToString()))
                    {
                        Font = new iTextSharp.text.Font(
                        iTextSharp.text.Font.FontFamily.HELVETICA, 10,
                        iTextSharp.text.Font.NORMAL,
                        BaseColor.BLUE

                        )
                    };
                    anchor.SetAnchor(pdfpath);


                    phrase.Add(anchor);

                    //phrase = new Phrase();
                    //phrase.Add(new Chunk(Convert.ToString(additionaldoc.Rows[i]["OriginalDocumentName"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.PaddingTop = 2f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable6.AddCell(cell);



                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(additionaldoc.Rows[i]["documentType"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable6.AddCell(cell);

                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(additionaldoc.Rows[i]["Description"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable6.AddCell(cell);


                    phrase = new Phrase();
                    phrase.Add(new Chunk(String.Format("{0:dd-MM-yyyy}", (additionaldoc.Rows[i]["DateOfUpload"])), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable6.AddCell(cell);



                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.Border = 0;
                cell.PaddingBottom = 6F;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable6.AddCell(cell);

            }

            //tiff code start here 12-July-2022
            if (drawingtifflsit.Rows.Count > 0)
            {
                for (int i = 0; i < drawingtifflsit.Rows.Count; i++)
                {
                    var pdfpath = "";
                    //code for link
                    var DocumentPdfPath = (drawingtifflsit.Rows[i]["FileName"].ToString());
                    if (DocumentPdfPath != "")
                    {
                        string path = databaseCon.siteimagelink + DocumentPdfPath;
                        pdfpath = databaseCon.siteimagelink + DocumentPdfPath;
                        var uri = new Uri(databaseCon.siteimagelink);
                        var container = new CloudBlobContainer(uri);
                        var blob = container.GetBlockBlobReference(DocumentPdfPath);
                        var exists = blob.Exists();
                        if (exists == false)
                        {

                        }
                        else
                        {

                        }
                    }

                    phrase = new Phrase();
                    var anchor = new Chunk(Convert.ToString(drawingtifflsit.Rows[i]["DocumentName"].ToString()))
                    {
                        Font = new iTextSharp.text.Font(
                        iTextSharp.text.Font.FontFamily.HELVETICA, 10,
                        iTextSharp.text.Font.NORMAL,
                        BaseColor.BLUE

                        )
                    };
                    anchor.SetAnchor(pdfpath);


                    phrase.Add(anchor);

                    //phrase = new Phrase();
                    //phrase.Add(new Chunk(Convert.ToString(additionaldoc.Rows[i]["OriginalDocumentName"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.PaddingTop = 2f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable10.AddCell(cell);



                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(drawingtifflsit.Rows[i]["Type"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable10.AddCell(cell);

                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(drawingtifflsit.Rows[i]["Description"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable10.AddCell(cell);


                    phrase = new Phrase();
                    phrase.Add(new Chunk(String.Format("{0:dd-MM-yyyy}", (drawingtifflsit.Rows[i]["CreatedOn"])), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable10.AddCell(cell);



                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.Border = 0;
                cell.PaddingBottom = 6F;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable10.AddCell(cell);

            }
            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 4;
            cell.Border = 0;
            headertable10.AddCell(cell);//Added on 13-July-2022
            //tiff code end here 12-July-2022

            //Added on 24-Feb-2022
            if (pdfdocimg.Rows.Count > 0)
            {
                for (int i = 0; i < pdfdocimg.Rows.Count; i++)
                {

                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(pdfdocimg.Rows[i]["OriginalDocumentName"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.PaddingTop = 2f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable6.AddCell(cell);



                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(pdfdocimg.Rows[i]["documentType"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable6.AddCell(cell);

                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(pdfdocimg.Rows[i]["Description"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable6.AddCell(cell);


                    phrase = new Phrase();
                    phrase.Add(new Chunk(String.Format("{0:dd-MM-yyyy}", (pdfdocimg.Rows[i]["DateOfUpload"])), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable6.AddCell(cell);



                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.Border = 0;
                cell.PaddingBottom = 6F;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable6.AddCell(cell);

            }

            //end 24-Feb-2022


            //phrase = new Phrase();
            //phrase.Add(new Chunk(" ", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 4;
            ////cell.PaddingTop = 10;
            ////cell.PaddingBottom = 10;
            ////cell.HorizontalAlignment = Element.ALIGN_LEFT;
            ////cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            ////cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            //headertable6.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 4;
            cell.Border = 0;
            headertable6.AddCell(cell);//Commented on 13-July-2022


            //code added for add doc today


            // Loop end here

            //Code for addditonal document end here  

            //code for additional doc pdf start here 
            //phrase = new Phrase();
            //phrase.Add(new Chunk("pdf additional doc ", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 2;
            //cell.PaddingTop = 10;
            //cell.PaddingBottom = 10;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            //headertable8.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" Engineered Document Name", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingTop = 10;
            cell.PaddingBottom = 10;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable8.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" Type", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingTop = 10;
            cell.PaddingBottom = 10;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable8.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Description", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingTop = 10;
            cell.PaddingBottom = 10;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable8.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Date of Upload", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingTop = 10;
            cell.PaddingBottom = 10;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable8.AddCell(cell);

            //loop start here 

            if (pdfdoc.Rows.Count > 0)
            {
                for (int i = 0; i < pdfdoc.Rows.Count; i++)
                {

                    //phrase = new Phrase();
                    //phrase.Add(new Chunk(Convert.ToString(pdfdoc.Rows[i]["DocumentName"].ToString()), DataFont));
                    //cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 15f;
                    //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //cell.Border = 1;
                    //cell.Colspan = 1;
                    //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    //headertable8.AddCell(cell);

                    //var pdfpath = "";
                    ////code for link
                    //var DocumentPdfPath = (pdfdoc.Rows[i]["DocumentName"].ToString());
                    //if (DocumentPdfPath != "")
                    //{
                    //    string path = databaseCon.siteimagelink + DocumentPdfPath;
                    //    pdfpath = databaseCon.siteimagelink + DocumentPdfPath;
                    //    var uri = new Uri(databaseCon.siteimagelink);
                    //    var container = new CloudBlobContainer(uri);
                    //    var blob = container.GetBlockBlobReference(DocumentPdfPath);
                    //    var exists = blob.Exists();
                    //    if (exists == false)
                    //    {

                    //    }
                    //    else
                    //    {

                    //    }
                    //}

                    //phrase = new Phrase();
                    //var anchor = new Chunk(Convert.ToString(pdfdoc.Rows[i]["DocumentName"].ToString()))
                    //{
                    //    Font = new Font(
                    //    Font.FontFamily.HELVETICA, 10,
                    //    Font.NORMAL,
                    //    BaseColor.BLUE

                    //    )
                    //};
                    //anchor.SetAnchor(pdfpath);


                    //phrase.Add(anchor);
                    //cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 15f;
                    //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //cell.Border = 1;
                    //cell.Colspan = 1;
                    //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    //headertable8.AddCell(cell);

                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(pdfdoc.Rows[i]["DocumentName"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable8.AddCell(cell);

                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(pdfdoc.Rows[i]["documentType"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable8.AddCell(cell);



                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(pdfdoc.Rows[i]["Description"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable8.AddCell(cell);


                    phrase = new Phrase();
                    phrase.Add(new Chunk(String.Format("{0:dd-MM-yyyy}", (pdfdoc.Rows[i]["CreatedOn"])), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable8.AddCell(cell);

                    //var pdfpath = "";
                    // code for link
                    //var DocumentPdfPath = (pdfdoc.Rows[i]["DocumentName"].ToString());
                    //if (DocumentPdfPath != "")
                    //{
                    //    string path = databaseCon.siteimagelink + DocumentPdfPath;
                    //    pdfpath = databaseCon.siteimagelink + DocumentPdfPath;
                    //    var uri = new Uri(databaseCon.siteimagelink);
                    //    var container = new CloudBlobContainer(uri);
                    //    var blob = container.GetBlockBlobReference(DocumentPdfPath);
                    //    var exists = blob.Exists();
                    //    if (exists == false)
                    //    {

                    //    }
                    //    else
                    //    {

                    //    }
                    //}

                    //phrase = new Phrase();
                    //var anchor = new Chunk(Convert.ToString(pdfdoc.Rows[i]["Description"].ToString()))
                    //{
                    //    Font = new Font(
                    //    Font.FontFamily.HELVETICA, 10,
                    //    Font.NORMAL,
                    //    BaseColor.BLUE

                    //    )
                    //};
                    //anchor.SetAnchor(pdfpath);


                    //phrase.Add(anchor);
                    //cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 15f;
                    //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //cell.Border = 1;
                    //cell.Colspan = 1;
                    //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    //headertable8.AddCell(cell);


                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 2;
                cell.Border = 0;
                cell.PaddingBottom = 6F;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable8.AddCell(cell);

            }

            //Loop end here 

            //code for additional doc pdf end here 


            //code for document image start hee

            //code for drawing start here
            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 2;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            headertable9.AddCell(cell);


            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 2;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            headertable9.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 2;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            headertable9.AddCell(cell);

            if (imagedt7.Rows.Count > 0)
            {
                for (int j = 0; j < imagedt7.Rows.Count; j++)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(imagedt7.Rows[j]["DocumentName"])))
                    {
                        var uri = new Uri(databaseCon.siteimagelink);
                        var container = new CloudBlobContainer(uri);
                        var blob = container.GetBlockBlobReference(imagedt7.Rows[0]["DocumentName"].ToString());
                        var noimage = System.Web.HttpContext.Current.Server.MapPath("~/images/noimg.jpg");

                        if (Convert.ToString(imagedt7.Rows[0]["DocumentName"]) == "")
                        {
                            //string nophoto = System.Web.HttpContext.Current.Server.MapPath("~/images/noimg.jpg");
                            //cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
                            cell = new PdfPCell();
                            cell.FixedHeight = 50f;
                            cell.Colspan = 1;
                            //cell.PaddingBottom = 15f;
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            //cell.PaddingRight = 35f;
                            cell.PaddingTop = -10;
                        }

                        else
                        {

                            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(databaseCon.registerdocumentlink + imagedt7.Rows[j]["DocumentName"].ToString());

                            WebClient wc = new WebClient();
                            byte[] bytes = wc.DownloadData(databaseCon.registerdocumentlink + imagedt7.Rows[j]["DocumentName"].ToString());
                            MemoryStream ms = new MemoryStream(bytes);
                            System.Drawing.Image img1 = System.Drawing.Image.FromStream(ms);

                            //iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance("https://inpexdocumenttest.blob.core.windows.net/siteimage/_609_637789830565711469.png");
                            img.ScaleAbsolute(180f, 150f);
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
                                byte[] bte = CompressedImageSave(img1);
                                MemoryStream ms2 = new MemoryStream(bte);
                                System.Drawing.Image img12 = System.Drawing.Image.FromStream(ms2);
                                cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img12, BaseColor.WHITE), true);//Commented on 08-November-2022
                                //cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img1, BaseColor.WHITE), true);
                            }
                            else
                            {
                                cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img), true);
                            }
                            //cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img), true);
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            //cell.FixedHeight = 100f;
                            cell.Colspan = 1;
                            cell.Padding = 10;
                            cell.Border = 0;
                            cell.PaddingTop = 10;
                            cell.BorderColor = BaseColor.WHITE;
                            headertable9.AddCell(cell);

                        }

                    }

                    else
                    {
                        phrase = new Phrase();
                        phrase.Add(new Chunk("", DataFont));
                        cell = new PdfPCell(phrase);
                        cell.Colspan = 1;
                        cell.Border = 3;
                        cell.PaddingBottom = 6F;
                        //cell.PaddingTop = -50f;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.PaddingTop = 10;
                        headertable9.AddCell(cell);
                    }


                }
            }

            else
            {
                //string nophoto = System.Web.HttpContext.Current.Server.MapPath("~/images/noimg.jpg");
                //cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.FixedHeight = 100f;
                cell.Colspan = 1;
                //cell.PaddingBottom = 15f;
                //cell.PaddingRight = 35f;
                cell.PaddingTop = 10;
                cell.BorderColor = BaseColor.WHITE;
                headertable9.AddCell(cell);
            }

            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingTop = 10;
            cell.Border = 0;
            //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable9.AddCell(cell);

            //code for documnet image end here

            //end here 

            doc.Add(headertable3);
            doc.NewPage();
            doc.Add(headertable5);
            doc.NewPage();
            doc.Add(headertable12);
            doc.Add(headertable7);
            //doc.NewPage();
            doc.Add(headertable13);
            doc.NewPage();
            doc.Add(headertable2);
            doc.NewPage();
            doc.Add(headertable6);
            //doc.Add(headertable10);//Commented on 16-July-2022
            doc.Add(headertable8);
            doc.Add(headertable10);//Added on 16-July-2022
            doc.Add(headertable4);
            doc.Add(headertable9);
            //doc.NewPage();
            //doc.Add(headertable8);

            doc.Close();

            return PdfName;

        }
        //private string MergePDF(string[] myArray, int RID)
        //    {
        //    string[] fileArray = new string[myArray.Length + 1];
        //    for (int i = 0; i <= myArray.Length; i++)
        //        {
        //        if (i < myArray.Length)
        //            {
        //            fileArray[i] = Server.MapPath("~/uploads/TempUpload/" + myArray[i]);
        //            }
        //        else
        //            {
        //            fileArray[i] = "";
        //            }
        //        }

        //    PdfReader reader = null;
        //    Document sourceDocument = null;
        //    PdfCopy pdfCopyProvider = null;
        //    PdfImportedPage importedPage;
        //    //string outputPdfPath = Server.MapPath("~/uploads/TempUpload/FinalWorkPackReport4.pdf");
        //    string PdfName = "";
        //    PdfName = "FinalWorkPackReport1_" + RID + ".pdf";
        //    //var outputPdfPath = Path.Combine(Server.MapPath("~/uploads/TempUpload/FinalWorkPackReport3.pdf"));
        //    var outputPdfPath = Path.Combine(Server.MapPath("~/uploads/TempUpload/" + PdfName));

        //    if (System.IO.File.Exists(outputPdfPath))
        //        {

        //        System.IO.File.Delete(outputPdfPath);
        //        }

        //    sourceDocument = new Document();
        //    pdfCopyProvider = new PdfCopy(sourceDocument, new System.IO.FileStream(outputPdfPath, System.IO.FileMode.Create));

        //    //output file Open  
        //    sourceDocument.Open();

        //    //files list wise Loop  
        //    for (int f = 0; f < fileArray.Length - 1; f++)
        //        {
        //        int pages = TotalPageCount(fileArray[f]);

        //        reader = new PdfReader(fileArray[f]);
        //        //Add pages in new file  
        //        for (int i = 1; i <= pages; i++)
        //            {
        //            importedPage = pdfCopyProvider.GetImportedPage(reader, i);
        //            pdfCopyProvider.AddPage(importedPage);
        //            }

        //        reader.Close();
        //        }
        //    //save the output file  
        //    sourceDocument.Close();

        //    return PdfName;

        //    }
        private static int TotalPageCount(string file)
        {
            using (StreamReader sr = new StreamReader(System.IO.File.OpenRead(file)))
            {
                Regex regex = new Regex(@"/Type\s*/Page[^s]");
                MatchCollection matches = regex.Matches(sr.ReadToEnd());

                return matches.Count;
            }
        }
        public void DownloadFileFromBlob(string fileName, string ContainerName)
        {
            //string folderName =”DatabaseBackupFiles”;
            CloudStorageAccount storageaccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudBlobClient blobclient = storageaccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobclient.GetContainerReference(ContainerName);
            CloudBlockBlob blockblob = container.GetBlockBlobReference(fileName);
            string directoryPath = Server.MapPath("~/uploads/TempUpload").ToString();
            string filePath = directoryPath + "\\" + fileName;
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            blockblob.DownloadToFile(filePath, FileMode.OpenOrCreate);
        }
        public static byte[] CompressedImageSave(System.Drawing.Image img)
        {
            // Image img = Image.FromFile(newPath);

            var savepath = "";
            string path = "~/uploads/Temp/";
            var NewFileName = "SnapInsp" + "_" + DateTime.Now.Ticks + ".jpg";

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
            //MemoryStream ms2 = new MemoryStream(bte);
            //System.Drawing.Image img12 = System.Drawing.Image.FromStream(ms2);

            //thumbnailImg.Save(path + NewFileName, img12.RawFormat);

            return bte;
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
        public class HeaderFooter2 : iTextSharp.text.pdf.PdfPageEventHelper
        {
            public int id { get; set; }
            public int RID { get; set; }
            // List<PdfTemplate> pageNumberTemplates = new List<PdfTemplate>();// page header

            public override void OnStartPage(PdfWriter writer, Document doc)
            {
                // int RID;
                // RID = Convert.ToInt32(databaseCon.Decrypt(HttpContext.Current.Request.QueryString["RID"]));


                base.OnStartPage(writer, doc);
                string queryheader = "select tblClient.ClientImage,Plans,right('000000' + convert(varchar,tblRegister.ScaffRef),6) as ScaffRef,left(Description,90) as Description from tblClient, tblRegister where tblClient.ClientId = tblRegister.ClientID and tblRegister.registerid = '" + RID + "'";
                DataTable dtheader = databaseCon.GetData(queryheader);


                string ImagePath = System.Web.HttpContext.Current.Server.MapPath("~/Images/logo2.png");// image size
                iTextSharp.text.Rectangle pageSize = doc.PageSize;
                PdfPTable headertable2 = new PdfPTable(6);
                headertable2.SetWidths(new int[] { 10, 9, 10, 9, 9, 7 });

                var contentfont = FontFactory.GetFont("Arial", 8f, BaseColor.BLACK);
                var Headings = FontFactory.GetFont("Arial", 9f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                var Head = FontFactory.GetFont("Arial", 11f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                var BackOrderheader = FontFactory.GetFont("Arial", 20f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                var Heading787 = FontFactory.GetFont("Arial", 20f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                var Heading786 = FontFactory.GetFont("Arial", 10f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                var DataFont = FontFactory.GetFont("Arial", 10f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                Phrase phrase;
                PdfPCell cell;




                //headertable2.AddCell(cell);

                //doc.Add(headertable2);
            }

            public override void OnEndPage(PdfWriter writer, Document document)
            {
                //var fontcolor = new iTextSharp.text.Color(114, 163, 118);
                //Paragraph footerdate = new Paragraph(databaseCon.ToTimeZoneTime(DateTime.Now.ToUniversalTime()).ToLongDateString(), FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.TIMES_ROMAN, fontcolor));
                //PdfPCell cell;
                //PdfPTable footerDate = new PdfPTable(1);
                //footerDate.TotalWidth = 400;
                //footerDate.HorizontalAlignment = Element.ALIGN_CENTER;
                //cell = new PdfPCell(footerdate);
                //cell.Border = 0;
                //cell.PaddingTop = 30;
                //footerDate.AddCell(cell);
                //footerDate.WriteSelectedRows(0, -1, 700, 70, writer.DirectContent);

                //int TotalPage = writer.PageNumber;
                //int Currentpage = writer.CurrentPageNumber;
                //string text = "";
                //Paragraph footer = new Paragraph(text, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL));
                Paragraph footerdate = new Paragraph(("Printed: " + DateTime.Now.ToString("dd-MMM-yyy")), FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK));
                PdfPCell cell;
                //if (writer.PageNumber >= 2)
                //{
                //    PdfPTable footerTbl = new PdfPTable(1);
                //    footerTbl.TotalWidth = 400;// 1350;
                //    footerTbl.HorizontalAlignment = Element.ALIGN_CENTER;
                //    cell = new PdfPCell(footer);
                //    cell.Border = 0;
                //    cell.PaddingLeft = 10;
                //    cell.PaddingTop = 50;
                //    footerTbl.AddCell(cell);
                //    footerTbl.WriteSelectedRows(0, -1, 20, 70, writer.DirectContent);
                //}
                PdfPTable footerDate = new PdfPTable(1);
                footerDate.TotalWidth = 400;
                footerDate.HorizontalAlignment = Element.ALIGN_CENTER;
                cell = new PdfPCell(footerdate);
                cell.Border = 0;
                cell.PaddingTop = 50;
                footerDate.AddCell(cell);
                footerDate.WriteSelectedRows(0, -1, 17, 70, writer.DirectContent);

            }
        }
        //public ActionResult DeleteLastFile(string FileName)
        //{
        //    var Response = "";
        //    try
        //    {
        //        if (System.IO.File.Exists(Path.Combine(Server.MapPath("~/uploads/TempUpload/" + FileName))))
        //        {

        //            System.IO.File.Delete(Path.Combine(Server.MapPath("~/uploads/TempUpload/" + FileName)));
        //            Response = "File Deleted";
        //        }
        //        else
        //        {
        //            Response = "File Not Exists";
        //        }

        //    }

        //    catch (Exception ex)
        //    {
        //        Response = ex.Message;
        //    }
        //    return Json(Response, JsonRequestBehavior.AllowGet);
        //}

        // Example of improved MergePDF method with proper cleanup
        //private string MergePDF(string[] myArray, int RID)
        //{
        //    string[] fileArray = new string[myArray.Length + 1];
        //    for (int i = 0; i <= myArray.Length; i++)
        //    {
        //        if (i < myArray.Length)
        //        {
        //            fileArray[i] = Server.MapPath("~/uploads/TempUpload/" + myArray[i]);
        //        }
        //        else
        //        {
        //            fileArray[i] = "";
        //        }
        //    }

        //    PdfReader reader = null;
        //    Document sourceDocument = null;
        //    PdfCopy pdfCopyProvider = null;
        //    PdfImportedPage importedPage;
        //    string PdfName = "FinalWorkPackReport1_" + RID + ".pdf";
        //    var outputPdfPath = Path.Combine(Server.MapPath("~/uploads/TempUpload/" + PdfName));

        //    // List to track temporary files for cleanup
        //    List<string> tempFilesToDelete = new List<string>();

        //    try
        //    {
        //        // Delete existing output file if it exists
        //        if (System.IO.File.Exists(outputPdfPath))
        //        {
        //            System.IO.File.Delete(outputPdfPath);
        //        }

        //        sourceDocument = new Document();
        //        pdfCopyProvider = new PdfCopy(sourceDocument, new System.IO.FileStream(outputPdfPath, System.IO.FileMode.Create));

        //        // Output file open  
        //        sourceDocument.Open();

        //        // Files list wise loop  
        //        for (int f = 0; f < fileArray.Length - 1; f++)
        //        {
        //            if (!string.IsNullOrEmpty(fileArray[f]) && System.IO.File.Exists(fileArray[f]))
        //            {
        //                // Add to cleanup list
        //                tempFilesToDelete.Add(fileArray[f]);

        //                int pages = TotalPageCount(fileArray[f]);
        //                reader = new PdfReader(fileArray[f]);

        //                // Add pages in new file  
        //                for (int i = 1; i <= pages; i++)
        //                {
        //                    importedPage = pdfCopyProvider.GetImportedPage(reader, i);
        //                    pdfCopyProvider.AddPage(importedPage);
        //                }

        //                reader.Close();
        //                reader = null; // Explicitly set to null after closing
        //            }
        //        }

        //        // Save the output file  
        //        sourceDocument.Close();

        //        return PdfName;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception
        //        System.Diagnostics.Debug.WriteLine($"Error in MergePDF: {ex.Message}");
        //        throw; // Re-throw to maintain original behavior
        //    }
        //    finally
        //    {
        //        // Cleanup resources
        //        try
        //        {
        //            reader?.Close();
        //            sourceDocument?.Close();
        //            pdfCopyProvider?.Close();
        //        }
        //        catch (Exception cleanupEx)
        //        {
        //            System.Diagnostics.Debug.WriteLine($"Error during resource cleanup: {cleanupEx.Message}");
        //        }

        //        // Delete temporary files
        //        foreach (string tempFile in tempFilesToDelete)
        //        {
        //            try
        //            {
        //                if (System.IO.File.Exists(tempFile))
        //                {
        //                    System.IO.File.Delete(tempFile);
        //                    System.Diagnostics.Debug.WriteLine($"Deleted temp file: {tempFile}");
        //                }
        //            }
        //            catch (Exception deleteEx)
        //            {
        //                System.Diagnostics.Debug.WriteLine($"Failed to delete temp file {tempFile}: {deleteEx.Message}");
        //            }
        //        }
        //    }
        //}

        private string MergePDF(string[] myArray, int RID)
        {
            string[] fileArray = new string[myArray.Length + 1];
            for (int i = 0; i <= myArray.Length; i++)
            {
                if (i < myArray.Length)
                {
                    fileArray[i] = Server.MapPath("~/uploads/TempUpload/" + myArray[i]);
                }
                else
                {
                    fileArray[i] = "";
                }
            }
            PdfReader reader = null;
            Document sourceDocument = null;
            PdfCopy pdfCopyProvider = null;
            PdfImportedPage importedPage;

            string PdfName = "FinalWorkPackReport1_" + RID + ".pdf";

            var outputPdfPath = Path.Combine(Server.MapPath("~/uploads/TempUpload/" + PdfName));
            List<string> tempFilesToDelete = new List<string>();

            try
            {

                if (System.IO.File.Exists(outputPdfPath))
                {
                    System.IO.File.Delete(outputPdfPath);
                }

                sourceDocument = new Document();

                pdfCopyProvider = new PdfCopy(
                    sourceDocument,
                    new System.IO.FileStream(outputPdfPath, System.IO.FileMode.Create)
                );

                sourceDocument.Open();

                for (int f = 0; f < fileArray.Length - 1; f++)
                {
                    if (!string.IsNullOrEmpty(fileArray[f]) &&
                        System.IO.File.Exists(fileArray[f]))
                    {
                        // Add to cleanup list
                        tempFilesToDelete.Add(fileArray[f]);

                        reader = new PdfReader(fileArray[f]);

                        int pages = reader.NumberOfPages;

                        // Add pages in new file
                        for (int i = 1; i <= pages; i++)
                        {
                            reader.ConsolidateNamedDestinations();

                            importedPage = pdfCopyProvider.GetImportedPage(reader, i);

                            pdfCopyProvider.AddPage(importedPage);
                        }

                        reader.Close();
                        reader.Dispose();
                        reader = null;
                    }
                }

                sourceDocument.Close();
                pdfCopyProvider.Close();

                return PdfName;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"Error in MergePDF: {ex.Message}"
                );

                throw;
            }
            finally
            {
                try
                {
                    reader?.Close();
                    reader?.Dispose();

                    sourceDocument?.Close();
                    pdfCopyProvider?.Close();
                }
                catch (Exception cleanupEx)
                {
                    System.Diagnostics.Debug.WriteLine(
                        $"Error during resource cleanup: {cleanupEx.Message}"
                    );
                }

                foreach (string tempFile in tempFilesToDelete)
                {
                    try
                    {
                        if (System.IO.File.Exists(tempFile))
                        {
                            System.IO.File.Delete(tempFile);

                            System.Diagnostics.Debug.WriteLine(
                                $"Deleted temp file: {tempFile}"
                            );
                        }
                    }
                    catch (Exception deleteEx)
                    {
                        System.Diagnostics.Debug.WriteLine(
                            $"Failed to delete temp file {tempFile}: {deleteEx.Message}"
                        );
                    }
                }
            }
        }


        // Generic method for safe file deletion
        private void SafeDeleteFile(string filePath)
        {
            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    System.Diagnostics.Debug.WriteLine($"Successfully deleted: {filePath}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to delete file {filePath}: {ex.Message}");
                // Don't throw - we don't want cleanup failures to break the main operation
            }
        }

        // Improved method with automatic cleanup after blob upload
        public void UploadToBlobWithCleanup(string localFilePath, string blobFileName, string containerName)
        {
            try
            {
                // Your blob upload logic here
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(containerName);
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobFileName);

                blockBlob.UploadFromFile(localFilePath);

                System.Diagnostics.Debug.WriteLine($"Successfully uploaded {localFilePath} to blob storage");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error uploading to blob: {ex.Message}");
                throw; // Re-throw to maintain original behavior
            }
            finally
            {
                // Always attempt to clean up the local file
                SafeDeleteFile(localFilePath);
            }
        }

        // Improved DeleteLastFile method with better error handling
        public ActionResult DeleteLastFile(string FileName)
        {
            var response = "";
            var filePath = Path.Combine(Server.MapPath("~/uploads/TempUpload/" + FileName));

            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    response = "File Deleted Successfully";
                }
                else
                {
                    response = "File Not Found";
                }
            }
            catch (UnauthorizedAccessException)
            {
                response = "Access denied - file may be in use";
            }
            catch (DirectoryNotFoundException)
            {
                response = "Directory not found";
            }
            catch (IOException ioEx)
            {
                response = $"IO Error: {ioEx.Message}";
            }
            catch (Exception ex)
            {
                response = $"Error: {ex.Message}";
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }


    }
}

