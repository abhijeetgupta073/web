using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.WindowsAzure.Storage.Blob;
using Scaffold.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scaffold.Controllers
{
    public class DamagedReportController : Controller
    {
        float Totalquantity = 0;
        float WeightTotal = 0;
        // GET: DamageReport
        // public void DamagedReport(string UID, string UserRole, string id, string fromtocheck, string DIDetailID, string DamagedInvId, string FromId, string ToId, string FromName, string RInvId)
        public void DamagedReport()
        {
             string UID = Request.QueryString["UID"];
             string UserRole = Request.QueryString["UserRole"];
             string id = Request.QueryString["id"];
             string fromtocheck = Request.QueryString["fromtocheck"];
             string DIDetailID = Request.QueryString["DIDetailID"]; 
             string DamagedInvId = Request.QueryString["DamagedInvId"];
             string FromId = Request.QueryString["FromId"];
             string ToId = Request.QueryString["ToId"];
             string FromName = Request.QueryString["FromName"];
             string RInvId = Request.QueryString["RInvId"];

            //int DIDetailID;
            //DIDetailID = Convert.ToInt32(databaseCon.Decrypt(Request.QueryString["DIDetailID"]));
            //showreport(DIDetailID);

         var returnFile = showreport(DIDetailID, fromtocheck, id, DamagedInvId, FromId, ToId, FromName, RInvId);

            var hdntxttempfile = returnFile[0];  // Create Pdf Name 
            var hdntxtFile = returnFile[1];     // // Create Pdf Full Path 

            var hdntemppdffile = "";
            var hdnpdffile = "";

            var uri = new Uri(databaseCon.registerdocumentlink);
            var container = new CloudBlobContainer(uri);
            // var blob = container.GetBlockBlobReference(hdntxttempfile.Value.ToString());
            var blob = container.GetBlockBlobReference(hdntxttempfile.ToString());
            var exists = blob.Exists();
            if (exists == false)
            {
                // databaseCon.deletefileonblob(hdntxttempfile.Value, "registerdocument");
                databaseCon.deletefileonblob(hdntxttempfile, "registerdocument");
            }
            else
            {
                // databaseCon.deletefileonblob(hdntxttempfile.Value, "registerdocument");
            }
            string extension = Path.GetExtension(".pdf");
            if (extension.Replace(".", "") == "pdf")
            {
                extension = "application/pdf";
            }

            // databaseCon.savefileonblob(hdntxttempfile.Value, "registerdocument", hdntxtFile.Value, extension.Replace(".", ""));
            databaseCon.savefileonblob(hdntxttempfile, "registerdocument", hdntxtFile, extension.Replace(".", ""));
            // if (File.Exists(hdntxtFile.Value))
            if (System.IO.File.Exists(hdntxtFile))
            {
                try
                {
                    // File.Delete(hdntxtFile.Value);
                    System.IO.File.Delete(hdntxtFile);
                }
                catch
                {

                }
            }
            // var pdfDocument = databaseCon.siteimagelink + hdntxttempfile.Value;
            var pdfDocument = databaseCon.siteimagelink + hdntxttempfile;
            List<string> lstFiles = new List<string>();
            lstFiles.Add(pdfDocument);

            // string query = "select DDDocumentId,DocumentName,Description,DateOfUpload from tblDeliveryDocketDocuments where transid='" + DIDetailID + "' and type='Damaged All Type Document'";
            string query = "select DDDocumentId,DocumentName,Description,DateOfUpload from tblDeliveryDocketDocuments where transid='" + databaseCon.Decrypt(DIDetailID) + "' and type='Damaged All Type Document'";
            DataTable dt = databaseCon.GetData(query);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string path = databaseCon.siteimagelink + dt.Rows[i]["DocumentName"];
                var uri1 = new Uri(databaseCon.siteimagelink);
                var container1 = new CloudBlobContainer(uri1);
                var blob1 = container1.GetBlockBlobReference(dt.Rows[i]["DocumentName"].ToString());
                var exists1 = blob1.Exists();

                if (exists1 == true)
                {
                    lstFiles.Add(path);
                }

            }

            string pdfpath = Server.MapPath("~/Report/FinalDamageReport_" + databaseCon.Decrypt(DIDetailID) + ".pdf");
            // hdntemppdffile.Value = "FinalDamageReport_" + DIDetailID + ".pdf";
            // hdnpdffile.Value = pdfpath;

             hdntemppdffile = "FinalDamageReport_" + databaseCon.Decrypt(DIDetailID) + ".pdf";
             hdnpdffile = pdfpath;
            MergePages(pdfpath, lstFiles);

            var uripdf = new Uri(databaseCon.registerdocumentlink);
            var containerpdf = new CloudBlobContainer(uripdf);
            // var blobpdf = containerpdf.GetBlockBlobReference(hdntemppdffile.Value.ToString());
            var blobpdf = containerpdf.GetBlockBlobReference(hdntemppdffile.ToString());
            var existspdf = blobpdf.Exists();
            if (existspdf == false)
            {
                // databaseCon.deletefileonblob(hdntemppdffile.Value, "registerdocument");
                databaseCon.deletefileonblob(hdntemppdffile, "registerdocument");
            }
            else
            {

            }
            string extensionpdf = Path.GetExtension(".pdf");
            if (extensionpdf.Replace(".", "") == "pdf")
            {
                extensionpdf = "application/pdf";
            }

            //databaseCon.savefileonblob(hdntemppdffile.Value, "registerdocument", hdnpdffile.Value, extensionpdf.Replace(".", ""));
            //if (File.Exists(hdnpdffile.Value))
            //{
            //    File.Delete(hdnpdffile.Value);
            //}

            databaseCon.savefileonblob(hdntemppdffile, "registerdocument", hdnpdffile, extensionpdf.Replace(".", ""));
            if (System.IO.File.Exists(hdnpdffile))
            {
                System.IO.File.Delete(hdnpdffile);
            }

            // var file = databaseCon.siteimagelink + hdntemppdffile.Value;
            var file = databaseCon.siteimagelink + hdntemppdffile;
            var uri2 = new Uri(databaseCon.siteimagelink);
            var container2 = new CloudBlobContainer(uri2);

            // Response.Write("<script> window.open('" + file + "','_blank'); </script>");
            Response.Redirect(file);
            
        }

        public string[] showreport(string DIDetailID, string fromtocheck, string id, string DamagedInvId, string FromId, string ToId, string FromName, string RInvId)
        //public void showreport()
        {
            DataTable dt;
            int AvailableQuantity = 0;
            //Site Avaliable Quantity Check
            if (databaseCon.Decrypt(fromtocheck) == "1")
            {
                string querysite;
                querysite = "select cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(DamagedAll.DamagedAllQuantity, 0) - Isnull(SentToJobQuantity, 0) - isnull(ReturnAllQuantity, 0) + Isnull(ReturnFromJobQuantity,0)) as int) as AvailableQuantity,DIDetailID from(select FromId, ToId, MaterialType, ItemCode, ItemDesc, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Depot' and[to] = 'Site' and TransType = 'Send' group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc) DeliveryAll left join (select FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Site' and[to] = 'Depot' group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.ToId = ReturnAll.FromId  and DeliveryAll.InventoryId = ReturnAll.InventoryId left join (select FromId, ToId, InventoryId, Sum(DamagedQuantity) as DamagedAllQuantity from tbldamagedInventoryDetails  join tbldamagedInventory on tbldamagedInventoryDetails.DamagedINvId = tbldamagedInventory.DamagedINvId where [FromName] = 'Site' and[ToName] = 'Depot' group by[FromName],[ToName], FromId, ToId, InventoryId )DamagedAll On DeliveryAll.FromId = DamagedAll.ToId and DeliveryAll.ToId = DamagedAll.FromId  and DeliveryAll.InventoryId = DamagedAll.InventoryId left join (select tblDamagedInventoryDetails.DIDetailID, tblDamagedInventoryDetails.DamagedInvId, FromId, ToId,Reason, tblDamagedInventoryDetails.InventoryId, Sum(DamagedQuantity) as DamagedQuantity from tblDamagedInventoryDetails  join tblDamagedInventory on tblDamagedInventoryDetails.DamagedInvId = tblDamagedInventory.DamagedInvId where [FromName] = 'Site' and[toName] = 'Depot' and tblDamagedInventoryDetails.DamagedInvId = '" + databaseCon.Decrypt(DamagedInvId) + "'  group by Reason,[FromName],[ToName], FromId, ToId, tblDamagedInventoryDetails.InventoryId, tblDamagedInventoryDetails.DamagedInvId, tblDamagedInventoryDetails.DIDetailID )DamagedQuery On DeliveryAll.FromId = DamagedQuery.ToId and DeliveryAll.ToId = DamagedQuery.FromId  and DeliveryAll.InventoryId = DamagedQuery.InventoryId Left join (select tblReturnInventoryDetails.RIDetailID, tblReturnInventoryDetails.ReturnInvId, FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Site' and[to] = 'Depot' and tblReturnInventoryDetails.ReturnInvId = '" + databaseCon.Decrypt(RInvId) + "' group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId, tblReturnInventoryDetails.ReturnInvId, tblReturnInventoryDetails.RIDetailID )ReturnQuery On DeliveryAll.FromId = ReturnQuery.ToId and DeliveryAll.ToId = ReturnQuery.FromId  and DeliveryAll.InventoryId = ReturnQuery.InventoryId left join (select FromId, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as SentToJobQuantity from tblTransactionHistDelivery where [From] = 'Site' and[to] = 'Job' and TransType = 'Send' group by[From],[To], FromId, tblTransactionHistDelivery.InventoryId )SentToJob On  DeliveryAll.ToId = SentToJob.FromId  and DeliveryAll.InventoryId = SentToJob.InventoryId Left join (select ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnFromJobQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site' group by[From],[To], ToId, tblReturnInventoryDetails.InventoryId )ReturnFromJob On  DeliveryAll.ToId = ReturnFromJob.ToId  and DeliveryAll.InventoryId = ReturnFromJob.InventoryId Left join (select ToId, tbldamagedInventoryDetails.InventoryId, Sum(DamagedQuantity) as DamagedFromJobQuantity from tbldamagedInventoryDetails  join tbldamagedInventory on tbldamagedInventoryDetails.DamagedINvId = tbldamagedInventory.DamagedINvId where [FromName] = 'Job' and[toName] = 'Site'group by[FromName],[ToNAME], ToId, InventoryId )DamagedFromJob On  DeliveryAll.ToId = DamagedFromJob.ToId  and DeliveryAll.InventoryId = DamagedFromJob.InventoryId where DeliveryAll.FromId = '" + databaseCon.Decrypt(id) + "' and DeliveryAll.ToId = '" + databaseCon.Decrypt(FromId) + "'and DIDetailID='" + databaseCon.Decrypt(DIDetailID) + "' order by MaterialType,ItemCode";
                dt = databaseCon.GetData(querysite);
                if (dt.Rows.Count > 0)
                {
                    AvailableQuantity = Convert.ToInt32(dt.Rows[0]["AvailableQuantity"]);
                }

            }
            //Job Avaliable Quantity Check
            else if (databaseCon.Decrypt(fromtocheck) == "2")
            {

                string queryjob = "select cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(DamagedAll.DamagedAllQuantity, 0) - Isnull(SentToJobQuantity, 0) + Isnull(DamagedFromJobQuantity, 0) - isnull(ReturnAllQuantity, 0)) as int) as AvailableQuantity,DIDetailID from(select FromId, ToId, MaterialType, ItemCode, ItemDesc, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Site' and[to] = 'Job' and TransType = 'Send' group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc) DeliveryAll left join(select FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site' group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.ToId = ReturnAll.FromId  and DeliveryAll.InventoryId = ReturnAll.InventoryId left join (select FromId, ToId, InventoryId, Sum(DamagedQuantity) as DamagedAllQuantity from tbldamagedInventoryDetails  join tbldamagedInventory on tbldamagedInventoryDetails.damagedINvId = tbldamagedInventory.damagedINvId where [FromName] = 'Job' and[ToName] = 'Site' group by[FromName],[ToName], FromId, ToId, InventoryId )DamagedAll On DeliveryAll.FromId = DamagedAll.ToId and DeliveryAll.ToId = DamagedAll.FromId  and DeliveryAll.InventoryId = DamagedAll.InventoryId left join (select tblDamagedInventoryDetails.DIDetailID, tblDamagedInventoryDetails.DamagedInvId, FromId, ToId, Reason, tblDamagedInventoryDetails.InventoryId, Sum(DamagedQuantity) as DamagedQuantity from tblDamagedInventoryDetails  join tblDamagedInventory on tblDamagedInventoryDetails.DamagedInvId = tblDamagedInventory.DamagedInvId where [FromName] = 'Job' and[toName] = 'Site' and tblDamagedInventoryDetails.DamagedInvId =  '" + databaseCon.Decrypt(DamagedInvId) + "'  group by Reason,[FromName],[ToName], FromId, ToId, tblDamagedInventoryDetails.InventoryId, tblDamagedInventoryDetails.DamagedInvId, tblDamagedInventoryDetails.DIDetailID )DamagedQuery On DeliveryAll.FromId = DamagedQuery.ToId and DeliveryAll.ToId = DamagedQuery.FromId  and DeliveryAll.InventoryId = DamagedQuery.InventoryId Left join (select tblReturnInventoryDetails.RIDetailID, tblReturnInventoryDetails.ReturnInvId, FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Site' and[to] = 'Job' and tblReturnInventoryDetails.ReturnInvId = '" + databaseCon.Decrypt(RInvId) + "' group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId, tblReturnInventoryDetails.ReturnInvId, tblReturnInventoryDetails.RIDetailID)ReturnQuery On DeliveryAll.FromId = ReturnQuery.ToId and DeliveryAll.ToId = ReturnQuery.FromId  and DeliveryAll.InventoryId = ReturnQuery.InventoryId left join (select FromId, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as SentToJobQuantity from tblTransactionHistDelivery where [From] = 'Site' and[to] = 'Job' and TransType = 'Send' group by[From],[To], FromId, tblTransactionHistDelivery.InventoryId )SentToJob On  DeliveryAll.ToId = SentToJob.FromId  and DeliveryAll.InventoryId = SentToJob.InventoryId Left join(select ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnFromJobQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site' group by[From],[To], ToId, tblReturnInventoryDetails.InventoryId )ReturnFromJob On  DeliveryAll.ToId = ReturnFromJob.ToId  and DeliveryAll.InventoryId = ReturnFromJob.InventoryId Left join (select ToId, tbldamagedInventoryDetails.InventoryId, Sum(DamagedQuantity) as DamagedFromJobQuantity from tbldamagedInventoryDetails  join tbldamagedInventory on tbldamagedInventoryDetails.damagedINvId = tbldamagedInventory.damagedINvId where [FromName] = 'Job' and[toName] = 'Site' group by [FromName],[ToNAME], ToId, InventoryId )DamagedFromJob On  DeliveryAll.ToId = DamagedFromJob.ToId  and DeliveryAll.InventoryId = DamagedFromJob.InventoryId where DeliveryAll.FromId ='" + databaseCon.Decrypt(ToId) + "' and DeliveryAll.ToId='" + databaseCon.Decrypt(FromId) + "'and DIDetailID='" + databaseCon.Decrypt(DIDetailID) + "'order by MaterialType,ItemCode";

                dt = databaseCon.GetData(queryjob);
                if (dt.Rows.Count > 0)
                {
                    AvailableQuantity = Convert.ToInt32(dt.Rows[0]["AvailableQuantity"]);
                }
            }

            //Depot Avaliable Quantity Check
            else if (databaseCon.Decrypt(fromtocheck) == "3")
            {
                //string querydepot = "select AvailableQuantity,DIDetailID,isnull(DamagedQuantity,0) as DamagedQuantity ,Reason, VW_DepotInventory.InventoryId from VW_DepotInventory left join(select  Reason,DIDetailID,clientId, tblDamagedInventoryDetails.InventoryId, Sum(isnull(DamagedQuantity, 0)) as DamagedQuantity from tblDamagedInventoryDetails join tblDamagedInventory on tblDamagedInventoryDetails.DamagedInvId = tblDamagedInventory.DamagedInvId where [FromName] = 'Depot' and[toName] = 'Depot' and clientId = '" + databaseCon.Decrypt(id) + "' and tblDamagedInventoryDetails.DamagedInvId='" + databaseCon.Decrypt(DamagedInvId) + "' group by[FromName],[ToName], tblDamagedInventoryDetails.InventoryId, clientId,DIDetailID,Reason) DamageQuery on VW_DepotInventory.InventoryID = DamageQuery.InventoryID and VW_DepotInventory.ClientID = DamageQuery.clientId   where VW_DepotInventory.ClientID = '" + databaseCon.Decrypt(id) + "' and DIDetailID='" + databaseCon.Decrypt(DIDetailID) + "' order by MaterialType,ItemCode";
                string querydepot = "select AvailableQuantity,DIDetailID,isnull(DamagedQuantity,0) as DamagedQuantity ,Reason, VW_DepotInventory.InventoryId from VW_DepotInventory left join(select  Reason,DIDetailID,clientId, tblDamagedInventoryDetails.InventoryId, Sum(isnull(DamagedQuantity, 0)) as DamagedQuantity from tblDamagedInventoryDetails join tblDamagedInventory on tblDamagedInventoryDetails.DamagedInvId = tblDamagedInventory.DamagedInvId where [FromName] = 'Depot' and[toName] = 'ThirdParty' and clientId = '" + databaseCon.Decrypt(id) + "' and tblDamagedInventoryDetails.DamagedInvId='" + databaseCon.Decrypt(DamagedInvId) + "' group by[FromName],[ToName], tblDamagedInventoryDetails.InventoryId, clientId,DIDetailID,Reason) DamageQuery on VW_DepotInventory.InventoryID = DamageQuery.InventoryID and VW_DepotInventory.ClientID = DamageQuery.clientId   where VW_DepotInventory.ClientID = '" + databaseCon.Decrypt(id) + "' and DIDetailID='" + databaseCon.Decrypt(DIDetailID) + "' order by MaterialType,ItemCode";
                dt = databaseCon.GetData(querydepot);
                if (dt.Rows.Count > 0)
                {
                    AvailableQuantity = Convert.ToInt32(dt.Rows[0]["AvailableQuantity"]);
                }

            }
            string query = "Select MaterialType,ItemCode,ItemDesc,ItemWeight,Supplier,SupplierDocketNo,Reason,Replace(convert(nvarchar,convert(datetime,DateActioned,120),106),' ','-') as DateActioned,'" + AvailableQuantity + "' as AvaQty,DamagedQuantity,(DamagedQuantity*ItemWeight) as TotalItemWeight from tblDamagedInventoryDetails join tblInventory on tblDamagedInventoryDetails.InventoryId=tblInventory.InventoryID where DIDetailID='" + databaseCon.Decrypt(DIDetailID) + "'";
            dt = databaseCon.GetData(query);

            string queryimage = "select DDDocumentID,DocumentName,Description from tblDeliveryDocketDocuments where Type='Damaged Inventory Document' and TransID='" + databaseCon.Decrypt(DIDetailID) + "'";
            DataTable dtimage = databaseCon.GetData(queryimage);

            string path = Server.MapPath("~/Report/DamageReport_" + databaseCon.Decrypt(DIDetailID) + ".pdf");

            //hdntxttempfile.Value = "DamageReport_" + DIDetailID + ".pdf";
            //hdntxtFile.Value = path;

            string hdntxttempfile = "DamageReport_" + databaseCon.Decrypt(DIDetailID) + ".pdf";
            string hdntxtFile = path;

            // File.Delete(path);
            Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, -50f, -50f, 20f, 20f);   //Page Size set top ,left,margin
                                                                                                                              // doc.SetPageSize(iTextSharp.text.PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));
            // iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, Response.OutputStream);
            // Response.ContentType = ("application/pdf");

       
            HeaderFooter2 evnt = new HeaderFooter2();
            writer.PageEvent = evnt;
            evnt.id = Convert.ToInt32(databaseCon.Decrypt(id));
            evnt.DIDetailID = Convert.ToInt32(databaseCon.Decrypt(DIDetailID));
            evnt.FromId = Convert.ToInt32(databaseCon.Decrypt(FromId));
            evnt.FromName = databaseCon.Decrypt(FromName);

            doc.Open();
            var Heading786 = FontFactory.GetFont("Arial", 10f, Font.BOLD, BaseColor.BLACK);
            //Heading786.BaseColor = BaseColor.GRAY;
            var Heading787 = FontFactory.GetFont("Arial", 20f, Font.BOLD, BaseColor.BLACK);
            var DataFont = FontFactory.GetFont("Arial", 10f, Font.NORMAL, BaseColor.BLACK);
            Phrase phrase;
            PdfPCell cell;

            PdfPTable headertable1 = new PdfPTable(5);
            headertable1.SetWidths(new int[] { 10, 9, 25, 9, 9 });

            //Table Start
            PdfPTable headerTbl = new PdfPTable(7);
            headerTbl.SetWidths(new int[] { 10, 11, 25, 1, 1, 17, 17 });
            PdfPTable headertable2 = new PdfPTable(9);
            headertable2.SetWidths(new int[] { 10, 11, 15, 9, 9, 16, 10, 9, 8 });

            PdfPTable headertableImage = new PdfPTable(2);
            headertableImage.SetWidths(new int[] { 20, 20 });

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading786));//top haddings
            cell = new PdfPCell(phrase);
            cell.Colspan = 9;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //cell.PaddingTop = -10f;
            //cell.PaddingLeft = 5f;
            cell.Border = 0;
            headerTbl.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading786));//top haddings
            cell = new PdfPCell(phrase);
            cell.Colspan = 3;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.PaddingTop = -10f;
            //cell.PaddingLeft = 5f;
            cell.Border = 0;
            headerTbl.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Damage Report # : ", Heading786));//top haddings
            cell = new PdfPCell(phrase);
            cell.Colspan = 3;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.Border = 0;
            headerTbl.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(databaseCon.Decrypt(DIDetailID).ToString(), DataFont));//top haddings
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            headerTbl.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading786));//top haddings
            cell = new PdfPCell(phrase);
            cell.Colspan = 7;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.Border = 0;
            headerTbl.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.Border = 0;
            headerTbl.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Total Weight (kg) : ", Heading786));//top haddings
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.Border = 0;
            headerTbl.AddCell(cell);

            phrase = new Phrase();
            string Totalweight = String.Format("{0:#,##0.00}", dt.Rows[0]["TotalItemWeight"]);
            phrase.Add(new Chunk(Totalweight, DataFont));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            headerTbl.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 7;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.Border = 0;
            headerTbl.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.Border = 0;
            headerTbl.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Date Actioned : ", Heading786));//top haddings
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.Border = 0;
            headerTbl.AddCell(cell);

            phrase = new Phrase();
            // phrase.Add(new Chunk("", DataFont));//top haddings
            phrase.Add(new Chunk(dt.Rows[0]["DateActioned"].ToString(), DataFont));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            headerTbl.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading786));//top haddings
            cell = new PdfPCell(phrase);
            cell.Colspan = 7;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.Border = 0;
            headerTbl.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading786));//top haddings
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.Border = 0;
            headerTbl.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Site Ref. #: ", Heading786));//top haddings
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.Border = 0;
            headerTbl.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(dt.Rows[0]["SupplierDocketNo"].ToString(), DataFont));//top haddings
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            headerTbl.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading786));//top haddings
            cell = new PdfPCell(phrase);
            cell.Colspan = 11;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.Border = 0;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Material Type", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.Border = 1;
            cell.PaddingBottom = 6f;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Item", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 1;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Item Description", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.Border = 1;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Available Quantity", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.Border = 1;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Damaged Quantity", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.Border = 1;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Reason for Damage ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.Border = 1;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Supplier", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.Border = 1;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk("Date Actioned", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.Border = 1;
            //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            //cell.BackgroundBaseColor = BaseColor.LIGHT_GRAY;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //headertable2.AddCell(cell);


            phrase = new Phrase();
            phrase.Add(new Chunk("Item Weight (kg)", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.Border = 1;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Total Weight (kg)", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.Border = 1;
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
                    string Avaiqty = String.Format("{0:#,##0}", dt.Rows[i]["AvaQty"]);
                    phrase.Add(new Chunk(Avaiqty, DataFont));
                    //phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["AvailableQuantity"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    phrase = new Phrase();
                    string qty = String.Format("{0:#,##0}", dt.Rows[i]["DamagedQuantity"]);
                    phrase.Add(new Chunk(qty, DataFont));
                    //phrase.Add(new Chunk(Convert.ToInt32(dt.Rows[i]["Quantity"]).ToString(), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Reason"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Supplier"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    //phrase = new Phrase();
                    //phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["DateActioned"].ToString()), DataFont));
                    //cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 15f;
                    //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //cell.Border = 1;
                    //cell.Colspan = 1;
                    //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    //headertable2.AddCell(cell);

                    phrase = new Phrase();
                    string itemweight = String.Format("{0:#,##0.00}", dt.Rows[i]["ItemWeight"]);
                    phrase.Add(new Chunk(itemweight, DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    phrase = new Phrase();
                    string Totalweight1 = String.Format("{0:#,##0.00}", dt.Rows[i]["TotalItemWeight"]);
                    phrase.Add(new Chunk(Totalweight1, DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["TotalItemWeight"])))
                    {
                        WeightTotal += Convert.ToInt32(dt.Rows[i]["TotalItemWeight"]);
                    }

                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 10;
                cell.Border = 0;
                cell.PaddingBottom = 6F;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

            }
            phrase = new Phrase();
            phrase.Add(new Chunk("", DataFont));
            cell = new PdfPCell(phrase);
            cell.Colspan = 7;
            cell.PaddingBottom = 6F;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Total:", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.Border = 3;
            cell.PaddingBottom = 6F;
            cell.Border = PdfPCell.RIGHT_BORDER;
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
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", DataFont));
            cell = new PdfPCell(phrase);
            cell.Colspan = 2;
            cell.Border = 0;
            headertableImage.AddCell(cell);
            List<string> listImages = new List<string>();
            int tempcount = 0;
            if (dtimage.Rows.Count > 0)
            {
                for (int j = 0; j < dtimage.Rows.Count; j++)
                {
                    var uri = new Uri(databaseCon.siteimagelink);
                    var container = new CloudBlobContainer(uri);
                    var blob = container.GetBlockBlobReference(dtimage.Rows[j]["DocumentName"].ToString());
                    string file = databaseCon.siteimagelink + dtimage.Rows[j]["DocumentName"].ToString();
                    listImages.Add(file);
                    tempcount++;
                }
            }

            for (int i = 0; i < listImages.Count; i++)
            {
                if (listImages.Count % 2 != 0 && i == (tempcount - 1))
                {
                    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(listImages[i]), true);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 1;
                    cell.Border = 0;
                    cell.FixedHeight = 200f;
                    // cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertableImage.AddCell(cell);

                    phrase = new Phrase();
                    phrase.Add(new Chunk("", DataFont));
                    cell = new PdfPCell(phrase);
                    cell.Colspan = 1;
                    cell.Border = 0;
                    headertableImage.AddCell(cell);

                }
                else
                {
                    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(listImages[i]), true);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 1;
                    cell.Border = 0;
                    cell.FixedHeight = 200f;
                    // cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertableImage.AddCell(cell);
                }

            }
            doc.Add(headerTbl);
            doc.Add(headertable2);
            doc.Add(headertableImage);
            doc.Close();


            //var PathName = new { hdntxttempfile = hdntxttempfile, hdntxtFile = hdntxtFile }; 
            return new string[] { hdntxttempfile = hdntxttempfile, hdntxtFile = hdntxtFile };
        }


        public partial class HeaderFooter2 : PdfPageEventHelper
        {
            public int id { get; set; }
            public int DIDetailID { get; set; }
            public int FromId { get; set; }
            public string FromName { get; set; }

            List<PdfTemplate> pageNumberTemplates = new List<PdfTemplate>();// page header
            public override void OnStartPage(iTextSharp.text.pdf.PdfWriter writer, Document doc)
            {
                //int id, DIDetailID, FromId;
                //id = Convert.ToInt32(databaseCon.Decrypt(HttpContext.Current.Request.QueryString["Id"]));
                //DIDetailID = Convert.ToInt32(databaseCon.Decrypt(HttpContext.Current.Request.QueryString["DIDetailID"]));
                //FromId = Convert.ToInt32(databaseCon.Decrypt(HttpContext.Current.Request.QueryString["FromId"]));
                //string FromName = Convert.ToString(databaseCon.Decrypt(HttpContext.Current.Request.QueryString["FromName"]));
                base.OnStartPage(writer, doc);

                //string queryheader1 = "select SiteLocation from tblSiteLocation where ClientId = '"+ id + "' and SiteLocID = '"+ FromId + "'";
                //DataTable dtheader1 = databaseCon.GetData(queryheader1);

                string queryheader = "Select Supplier,SupplierDocketNo,Reason,DateActioned,ClientImage,ItemWeight,DamagedQuantity,(DamagedQuantity*ItemWeight) as TotalItemWeight from tblDamagedInventoryDetails join tblInventory on tblDamagedInventoryDetails.InventoryId=tblInventory.InventoryID join tblclient on tblInventory.clientid=tblclient.clientid where DIDetailID='" + DIDetailID + "' and tblinventory.clientid= '" + id + "'";
                DataTable dtheader = databaseCon.GetData(queryheader);

                string ImagePath = System.Web.HttpContext.Current.Server.MapPath("~/Images/logo2.png");// image size
                Rectangle pageSize = doc.PageSize;
                PdfPTable headerTbl = new PdfPTable(7);
                headerTbl.SetWidths(new int[] { 10, 11, 25, 1, 1, 17, 17 });

                PdfPTable headertable2 = new PdfPTable(9);
                headertable2.SetWidths(new int[] { 10, 11, 15, 9, 9, 16, 10, 9, 8 });

                var contentfont = FontFactory.GetFont("Arial", 8f, BaseColor.BLACK);
                var Headings = FontFactory.GetFont("Arial", 9f, Font.BOLD, BaseColor.BLACK);
                var Head = FontFactory.GetFont("Arial", 16f, Font.BOLD, BaseColor.BLACK);
                var BackOrderheader = FontFactory.GetFont("Arial", 20f, Font.BOLD, BaseColor.BLACK);
                var Heading786 = FontFactory.GetFont("Arial", 10f, Font.BOLD, BaseColor.BLACK);
                var DataFont = FontFactory.GetFont("Arial", 10f, Font.NORMAL, BaseColor.BLACK);

                Phrase phrase;
                PdfPCell cell;

                if (!string.IsNullOrEmpty(Convert.ToString(dtheader.Rows[0]["ClientImage"])))
                {
                    var uri = new Uri(databaseCon.clientimageLink);
                    // var container = new CloudBlobContainer(uri);
                    //var blob = container.GetBlockBlobReference(dtheader.Rows[0]["ClientImage"].ToString());
                    var noimage = System.Web.HttpContext.Current.Server.MapPath("~/Images/Logo2.png");
                    //var exists = blob.Exists();
                    if (Convert.ToString(dtheader.Rows[0]["ClientImage"]) == "")
                    {
                        string nophoto = System.Web.HttpContext.Current.Server.MapPath("~/images/Logo2.png");
                        cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
                        cell.FixedHeight = 50f;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Colspan = 1;
                        //cell.PaddingLeft = -45f;
                        cell.PaddingTop = -10;
                    }
                    else
                    {
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(System.Web.HttpContext.Current.Server.MapPath("~/images/clientlogo/" + dtheader.Rows[0]["ClientImage"].ToString()));

                        img.ScaleAbsolute(180f, 133f);
                        cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img), true);
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.FixedHeight = 50f;
                        cell.Colspan = 1;
                        //cell.PaddingLeft = -45f;
                        cell.PaddingTop = -10f;

                    }
                }
                else
                {
                    string nophoto = System.Web.HttpContext.Current.Server.MapPath("~/images/Logo2.png");
                    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
                    cell.FixedHeight = 50f;
                    //cell.PaddingLeft = -45f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 1;
                    cell.PaddingTop = -10f;
                }
                cell.Border = 0;
                cell.Colspan = 1;
                cell.FixedHeight = 50f;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell.PaddingLeft = -45f;
                cell.PaddingTop = -10f;
                headerTbl.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Damage Stock Report", BackOrderheader));//top haddings
                cell = new PdfPCell(phrase);
                cell.Colspan = 6;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.PaddingTop = -10f;
                //cell.PaddingLeft = 5f;
                cell.Border = 0;
                headerTbl.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("From : ", Head));
                phrase.Add(new Chunk(FromName, Head));//top haddings
                cell = new PdfPCell(phrase);
                cell.Colspan = 7;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.PaddingTop = -20f;
                //cell.PaddingLeft = 5f;
                cell.Border = 0;
                headerTbl.AddCell(cell);



                doc.Add(headerTbl);
                doc.Add(headertable2);
            }
            public override void OnEndPage(PdfWriter writer, Document document)
            {
                Paragraph footerdate = new Paragraph(("Printed: " + DateTime.Now.ToString("dd-MMM-yyy")), FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
                PdfPCell cell;

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

        public void MergePages(string outFile, List<string> fileNames)
        {
            // step 1: creation of a document-object
            Document document = new Document();

            using (FileStream newFileStream = new FileStream(outFile, FileMode.Create))
            {
                // step 2: we create a writer that listens to the document
                PdfCopy writer = new PdfCopy(document, newFileStream);
                if (writer == null)
                {
                    return;
                }

                // step 3: we open the document
                document.Open();

                foreach (string fileName in fileNames)
                {
                    // we create a reader for a certain document
                    PdfReader reader = new PdfReader(fileName);
                    reader.ConsolidateNamedDestinations();

                    // step 4: we add content
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        PdfImportedPage page = writer.GetImportedPage(reader, i);
                        writer.AddPage(page);
                    }

                    PRAcroForm form = reader.AcroForm;
                    if (form != null)
                    {
                        // writer.CopyAcroForm(reader);
                        writer.AddDocument(reader);
                        
                    }
                  reader.Close();
                }
                writer.Close();
                document.Close();
            }

        }
    }

  

}