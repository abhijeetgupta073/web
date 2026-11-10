using iTextSharp.text;
using iTextSharp.text.pdf;

using Scaffold.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Scaffold.Controllers
{
    public class PDFWithPagination : PdfPageEventHelper
    {
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            PdfContentByte cb = writer.DirectContent;
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Paragraph footerdate = new Paragraph(("Printed: " + DateTime.Now.ToString("dd-MMM-yyy")), FontFactory.GetFont(FontFactory.TIMES, 10,iTextSharp.text.BaseColor.BLACK));
            PdfPCell cell;
           
            PdfPTable footerDate = new PdfPTable(1);
            footerDate.TotalWidth = 400;
            footerDate.HorizontalAlignment = Element.ALIGN_CENTER;
            cell = new PdfPCell(footerdate);
            cell.Border = 0;
            cell.PaddingTop = 50;
            footerDate.AddCell(cell);
            footerDate.WriteSelectedRows(0, -1, 17, 70, writer.DirectContent);
            cb.BeginText();
            cb.SetFontAndSize(bf, 10);

            // Adjust position
            float x = document.LeftMargin + 585; // Move slightly right
            float y = document.BottomMargin - 10; // Move slightly up

            string pageText = "Page " + writer.PageNumber;
            cb.SetTextMatrix(x, y);
            cb.ShowText(pageText);
            cb.EndText();

            System.Diagnostics.Debug.WriteLine("OnEndPage triggered for page " + writer.PageNumber);
        }
    }
    public class DeliveryDocketReportController : Controller
    {
        // GET: DeliveryDocketReport
        public ActionResult Index()
        {
            return View();
            //  DeliveryDocketReport();
        }

        public void DeliveryDocketReport()
        {
            int DocketNo = 0;
            string query = "select DelDocId,Comment from tblTransaction join tblDeliveryDocket on tblTransaction.DeliveryId=tblDeliveryDocket.DeliveryID and tblTransaction.ClientId=tblDeliveryDocket.ClientId where TransId=" + databaseCon.Decrypt(Request.QueryString["checkid"]) + "and tblDeliveryDocket.ClientId=" + databaseCon.Decrypt(Request.QueryString["id"]);
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                DocketNo = Convert.ToInt32(dt.Rows[0]["DelDocId"]);
            }

            string queryheader = "select ClientImage,Name from tblClient where ClientId ='" + databaseCon.Decrypt(Request.QueryString["id"]) + "'";
            DataTable dtheader = databaseCon.GetData(queryheader);

            string querydata = "select SiteLocation, Address from tblSiteLocation where ClientId ='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and SiteLocID='" + databaseCon.Decrypt(Request.QueryString["ToId"]) + "'";
            DataTable dtheaderdata = databaseCon.GetData(querydata);

            string querydt = "select DepotPic,DepotPersonSign,TransportDriverSign,SitePic,SitePersonSign,AcceptorSign,Replace(Convert(varchar,PickDate,106),' ','-') as PickDate,DelDocId,Case when SiteReceivedDate='1900-01-01' then null else  Replace(Convert(varchar,SiteReceivedDate,106),' ','-') end as SiteReceivedDate,DepotPickedBy, TransportBy, TransportCompName,SiteReceivedBy,ReceivedByPosition, AcceptedBy, ClientPosition,isnull(ReceiptConfirmation,0) as ReceiptConfirmation from tbldeliverydocket where DelDocId =" + DocketNo + " ";
            DataTable dtdata = databaseCon.GetData(querydt);

            //string querytable = " select tblTransactionHistDelivery.TransDeliId,TransId,tblInventory.MaterialType as MaterialType,tblInventory.ItemCode as ItemCode,Left(tblInventory.ItemDesc,30) as ItemDesc,convert(decimal(18,2),tblTransactionHistDelivery.Quantity) as Quantity,convert(decimal(18,2),tblInventory.ItemWeight) as ItemWeight,convert(decimal(18,2),(tblInventory.ItemWeight * tblTransactionHistDelivery.Quantity)) as TotalWeight from tblTransactionHistRequest join VW_depotInventory on tblTransactionHistRequest.InventoryId = VW_depotInventory.InventoryId  left join tblTransactionHistDelivery on tblTransactionHistRequest.TransReqId = tblTransactionHistDelivery.RequestId join tblInventory on tblTransactionHistDelivery.InventoryId = tblInventory.InventoryID where TransId in (" + databaseCon.Decrypt(Request.QueryString["checkid"]) + ")order by tblInventory.MaterialType,tblInventory.ItemCode";
            // remove  item desc limit on 21 feb 2025 by ss
            string querytable = " select tblTransactionHistDelivery.TransDeliId,TransId,tblInventory.MaterialType as MaterialType,tblInventory.ItemCode as ItemCode,tblInventory.ItemDesc as ItemDesc,convert(decimal(18,2),tblTransactionHistDelivery.Quantity) as Quantity,convert(decimal(18,2),tblInventory.ItemWeight) as ItemWeight,convert(decimal(18,2),(tblInventory.ItemWeight * tblTransactionHistDelivery.Quantity)) as TotalWeight from tblTransactionHistRequest join VW_depotInventory on tblTransactionHistRequest.InventoryId = VW_depotInventory.InventoryId  left join tblTransactionHistDelivery on tblTransactionHistRequest.TransReqId = tblTransactionHistDelivery.RequestId join tblInventory on tblTransactionHistDelivery.InventoryId = tblInventory.InventoryID where TransId in (" + databaseCon.Decrypt(Request.QueryString["checkid"]) + ")order by tblInventory.MaterialType,ISNULL(tblInventory.DisplaySortOrder,9999999),tblInventory.ItemCode";
            DataTable dtTable = databaseCon.GetData(querytable);

            string querytotalsum = "select Sum(tblTransactionHistDelivery.Quantity) as TotalQuantity ,round(SUM((tblInventory.ItemWeight * tblTransactionHistDelivery.Quantity))/1000,2) as SumOfTotalWeight from tblTransactionHistRequest join VW_depotInventory on tblTransactionHistRequest.InventoryId = VW_depotInventory.InventoryId  left join tblTransactionHistDelivery on tblTransactionHistRequest.TransReqId = tblTransactionHistDelivery.RequestId join tblInventory on tblTransactionHistDelivery.InventoryId = tblInventory.InventoryID where TransId in (" + databaseCon.Decrypt(Request.QueryString["checkid"]) + ")";
            DataTable dttotal = databaseCon.GetData(querytotalsum);

            var ThirdPartyRequest = databaseCon.Decrypt(Request.QueryString["ThirdParty"]);

            //iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, -50f, -50f, 20f, 20f);

            iTextSharp.text.Document doc = new iTextSharp.text.Document(PageSize.A4, -50f, -50f, 20f, 20f);   //Page Size set top ,left,margin
                                                                                                              //doc.SetPageSize(iTextSharp.text.PageSize.A4);


            iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, Response.OutputStream);
            writer.PageEvent = new PDFWithPagination();

            Response.ContentType = ("application/pdf");

            var contentfontCheck = FontFactory.GetFont((Server.MapPath("~/Content/fonts/wingding.ttf")), BaseFont.CP1252, true, 14);

            var contentfontUncheck = FontFactory.GetFont((Server.MapPath("~/Content/fonts/wingding.ttf")), BaseFont.CP1252, true, 14);
            doc.Open();
            var Heading786 = FontFactory.GetFont("Arial", 25f, Font.BOLD, BaseColor.GRAY);
            var Heading787 = FontFactory.GetFont("Arial", 10f, Font.BOLD, BaseColor.BLACK);
            var Heading788 = FontFactory.GetFont("Arial", 20f, Font.BOLD, BaseColor.BLACK);
            var DataFont = FontFactory.GetFont("Arial", 10f, Font.NORMAL, BaseColor.BLACK);
            Phrase phrase;
            PdfPCell cell;

            PdfPTable headertable1 = new PdfPTable(5);
            headertable1.SetWidths(new int[] { 10, 9, 10, 10, 9 });

            PdfPTable headertable3 = new PdfPTable(6);
            headertable3.SetWidths(new int[] { 13, 7, 25, 9, 7, 9 });

            PdfPTable headertable4 = new PdfPTable(6);
            headertable4.SetWidths(new int[] { 13, 7, 25, 9, 7, 9 });
            //Table Start

            PdfPTable headertable2 = new PdfPTable(6);
            headertable2.SetWidths(new int[] { 13, 7, 25, 9, 7, 9 });
            headertable2.HeaderRows = 2;

            if (!string.IsNullOrEmpty(Convert.ToString(dtheader.Rows[0]["ClientImage"])))
            {
                var uri = new Uri(databaseCon.clientimageLink);
                //var container = new CloudBlobContainer(uri);
                //var blob = container.GetBlockBlobReference(dtheader.Rows[0]["ClientImage"].ToString());
                var noimage = Server.MapPath("~/Images/Logo2.png");
                //var exists = blob.Exists();
                //if (exists == false)
                if (Convert.ToString(dtheader.Rows[0]["ClientImage"]) == "")
                {
                    string nophoto = Server.MapPath("~/images/Logo2.png");
                    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
                    cell.FixedHeight = 100f;
                    cell.Colspan = 1;
                    //cell.PaddingLeft = 10f;
                    //cell.PaddingTop = 20;
                }
                else
                {
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images/clientlogo/" + dtheader.Rows[0]["ClientImage"].ToString()));

                    img.ScaleAbsolute(180f, 133f);
                    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img), true);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.FixedHeight = 100f;
                    cell.Colspan = 2;
                    //cell.PaddingLeft = 10f;
                    //cell.PaddingTop = 20f;

                }
            }
            else
            {
                string nophoto = Server.MapPath("~/images/Logo2.png");
                cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
                cell.FixedHeight = 100f;
                //cell.PaddingLeft = 10f;
                cell.Colspan = 2;
                //cell.PaddingTop = 20f;
            }
            cell.Border = 0;
            cell.Colspan = 2;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //cell.PaddingLeft = 20f;
            //cell.PaddingTop = 30f;
            cell.FixedHeight = 100f;
            headertable1.AddCell(cell);



            phrase = new Phrase();
            phrase.Add(new Chunk("DELIVERY DOCKET", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 3;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable1.AddCell(cell);

            if (Convert.ToBoolean(dtdata.Rows[0]["ReceiptConfirmation"]) == true)
            {



                phrase = new Phrase();
                phrase.Add(new Chunk("Delivery Confirmed", Heading788));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.Border = 0;
                cell.PaddingRight = -50f;
                //cell.PaddingTop = -40f;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                headertable1.AddCell(cell);
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk(" ", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                //cell.FixedHeight = 40f;
                //cell.PaddingBottom = -30f;
                //cell.PaddingLeft = 100f;
                //cell.PaddingTop = 30f;
                cell.Border = 0;
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
            }

            if (Convert.ToBoolean(dtdata.Rows[0]["ReceiptConfirmation"]) == true)
            {

                string Unchecked = System.Web.HttpContext.Current.Server.MapPath("~/images/lock.png");
                cell = new PdfPCell(iTextSharp.text.Image.GetInstance(Unchecked), true);
                //cell.PaddingBottom = -30f;
                //cell.PaddingTop = 30f;
                cell.FixedHeight = 40f;
                //cell.PaddingTop = -40f;
                cell.Border = 0;
                cell.Colspan = 1;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                headertable1.AddCell(cell);
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.FixedHeight = 40f;
                //cell.PaddingBottom = -30f;
                //cell.PaddingLeft = 100f;
                //cell.PaddingTop = 30f;
                cell.Border = 0;
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
            }

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Client:", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.Border = 0;

            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable3.AddCell(cell);

            if (dtheader.Rows.Count > 0)
            {

                phrase = new Phrase();
                phrase.Add(new Chunk(Convert.ToString(dtheader.Rows[0]["Name"].ToString()), DataFont));
                cell = new PdfPCell(phrase);
                //cell.PaddingBottom = 5f;
                //cell.PaddingTop = 8f;
                // cell.PaddingLeft = -50f;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;

                cell.Colspan = 2;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
                headertable3.AddCell(cell);


            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 2;
                cell.Border = 0;
                //cell.PaddingBottom = 6F;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }



            //phrase = new Phrase();
            //phrase.Add(new Chunk("", Heading787));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.Border = 0;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("   Date of Arriving at Site:", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 2;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable3.AddCell(cell);

            if (dtdata.Rows.Count > 0)
            {

                phrase = new Phrase();
                phrase.Add(new Chunk(Convert.ToString(dtdata.Rows[0]["SiteReceivedDate"].ToString()), DataFont));
                cell = new PdfPCell(phrase);
                //cell.PaddingBottom = 5f;
                //cell.PaddingTop = 8f;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;

                cell.Colspan = 1;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                headertable3.AddCell(cell);


            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 0;
                //cell.PaddingBottom = 6F;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }

            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.Border = 0;
            cell.PaddingTop = -8f;
            //cell.PaddingBottom = 0f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable3.AddCell(cell);


            if (ThirdPartyRequest == "true")
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("From:", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("Site Name:", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }
            if (dtheaderdata.Rows.Count > 0)
            {
                if (ThirdPartyRequest == "true")
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("Third Party", DataFont));
                    cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 5f;
                    //cell.PaddingTop = 8f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;

                    cell.Colspan = 2;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable3.AddCell(cell);
                }
                else
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dtheaderdata.Rows[0]["SiteLocation"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 5f;
                    //cell.PaddingTop = 8f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;

                    cell.Colspan = 2;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable3.AddCell(cell);
                }

            }
            else
            {
                if (ThirdPartyRequest == "true")
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("Third Party", DataFont));
                    cell = new PdfPCell(phrase);
                    cell.Colspan = 2;
                    cell.Border = 0;
                    //cell.PaddingBottom = 6F;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    headertable3.AddCell(cell);
                }
                else
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("", DataFont));
                    cell = new PdfPCell(phrase);
                    cell.Colspan = 2;
                    cell.Border = 0;
                    //cell.PaddingBottom = 6F;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    headertable3.AddCell(cell);
                }
            }

            //phrase = new Phrase();
            //phrase.Add(new Chunk("", Heading787));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.Border = 0;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("   Transfer Number:", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 2;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable3.AddCell(cell);

            if (dtdata.Rows.Count > 0)
            {

                phrase = new Phrase();
                phrase.Add(new Chunk(Convert.ToString(dtdata.Rows[0]["DelDocId"].ToString()), DataFont));
                cell = new PdfPCell(phrase);
                //cell.PaddingBottom = 5f;
                //cell.PaddingTop = 8f;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;

                cell.Colspan = 1;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                headertable3.AddCell(cell);


            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                //cell.Border = 0;
                //cell.PaddingBottom = 6F;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }



            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.Border = 0;
            cell.PaddingTop = -8f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable3.AddCell(cell);

            if (ThirdPartyRequest == "true")
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("To:", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("Site Address:", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }
            if (dtheaderdata.Rows.Count > 0)
            {

                if (ThirdPartyRequest == "true")
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("Depot", DataFont));
                    cell = new PdfPCell(phrase);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;

                    cell.Colspan = 2;
                    cell.Border = PdfPCell.RIGHT_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER;
                    headertable3.AddCell(cell);
                }
                else
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dtheaderdata.Rows[0]["Address"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 5f;
                    //cell.PaddingTop = 8f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;

                    cell.Colspan = 2;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.RIGHT_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER;
                    headertable3.AddCell(cell);
                }

            }
            else
            {

                if (ThirdPartyRequest == "true")
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("Depot", DataFont));
                    cell = new PdfPCell(phrase);
                    cell.Colspan = 2;
                    cell.Border = PdfPCell.RIGHT_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    headertable3.AddCell(cell);
                }
                else
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("", DataFont));
                    cell = new PdfPCell(phrase);
                    cell.Colspan = 2;
                    cell.Border = PdfPCell.RIGHT_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    headertable3.AddCell(cell);
                }
            }

            //phrase = new Phrase();
            //phrase.Add(new Chunk("", Heading787));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.Border = 0;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("   Total Item:", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 2;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            string Totalitem = String.Format("{0:#,##0}", dttotal.Rows[0]["TotalQuantity"]);
            phrase.Add(new Chunk(Totalitem, DataFont));
            //phrase.Add(new Chunk(Convert.ToString(dttotal.Rows[0]["TotalQuantity"].ToString()), DataFont));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.Border = 0;
            cell.PaddingTop = -8f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable3.AddCell(cell);
            if (ThirdPartyRequest == "true")
            {
                phrase = new Phrase();
                //phrase.Add(new Chunk("Date of Leaving Third Party:", Heading787));
                phrase.Add(new Chunk("Delivered Date:", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("Delivered Date:", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }
            if (dtdata.Rows.Count > 0)
            {

                phrase = new Phrase();
                phrase.Add(new Chunk(Convert.ToString(dtdata.Rows[0]["Pickdate"].ToString()), DataFont));
                cell = new PdfPCell(phrase);
                //cell.PaddingBottom = 5f;
                //cell.PaddingTop = 8f;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;

                cell.Colspan = 2;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
                headertable3.AddCell(cell);


            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 2;
                //cell.Border = 0;
                //cell.PaddingBottom = 6F;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }

            //phrase = new Phrase();
            //phrase.Add(new Chunk("", Heading787));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.Border = 0;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Total Weight (Tonnes)", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 2;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            string Totalwei = String.Format("{0:#,##0.00}", dttotal.Rows[0]["SumOfTotalWeight"]);
            phrase.Add(new Chunk(Totalwei, DataFont));
            //phrase.Add(new Chunk(Convert.ToString(dttotal.Rows[0]["SumOfTotalWeight"].ToString()), DataFont));
            //phrase.Add(new Chunk(" Tonnes", DataFont));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.Border = 0;
            cell.PaddingTop = -8f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Request Comment:", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.Border = 0;

            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable3.AddCell(cell);

            if (dt.Rows.Count > 0)
            {

                phrase = new Phrase();
                phrase.Add(new Chunk(Convert.ToString(dt.Rows[0]["Comment"].ToString()), DataFont));
                cell = new PdfPCell(phrase);
                //cell.PaddingBottom = 5f;
                //cell.PaddingTop = 8f;
                // cell.PaddingLeft = -50f;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;

                cell.Colspan = 5;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
                headertable3.AddCell(cell);


            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 5;
                cell.Border = 0;
                //cell.PaddingBottom = 6F;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);
            }

            // added by ss on  21 feb  2025   create  one more table  for show heading   data 


            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.Border = 0;
            cell.PaddingTop = -9f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Material Type", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.Border = 1;
            cell.PaddingBottom = 6f;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Item Code", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 1;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Description", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.Border = 1;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Item Weight (kg)", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.Border = 1;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Quantity", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.Border = 1;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Total Weight (kg)", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.Border = 1;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);


            if (dtTable.Rows.Count > 0)
            {
                //int rowcount = 10;
                string ItemDescription = "";
                //doc.Add(headertable1);
                //doc.Add(headertable2);
                //headertable2 = new PdfPTable(6);
                //05-ap-2021 headertable2.SetWidths(new int[] { 10, 10, 31, 9, 7, 9 });
                //headertable2.SetWidths(new int[] { 13, 7, 25, 9, 7, 9 });

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    //if (rowcount >= 32)
                    //{

                        //doc.NewPage();
                        



                        //phrase = new Phrase();
                        //phrase.Add(new Chunk("Material Type", Heading787));
                        //cell = new PdfPCell(phrase);
                        //cell.Colspan = 1;
                        //cell.Border = 1;
                        //cell.PaddingBottom = 6f;
                        //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                        //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        //headertable2.AddCell(cell);

                        //phrase = new Phrase();
                        //phrase.Add(new Chunk("Item Code", Heading787));
                        //cell = new PdfPCell(phrase);
                        //cell.Colspan = 1;
                        //cell.PaddingBottom = 6f;
                        //cell.Border = 1;
                        //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                        //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        //headertable2.AddCell(cell);

                        //phrase = new Phrase();
                        //phrase.Add(new Chunk("Description", Heading787));
                        //cell = new PdfPCell(phrase);
                        //cell.Colspan = 1;
                        //cell.Border = 1;
                        //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                        //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        ////cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        //headertable2.AddCell(cell);

                        //phrase = new Phrase();
                        //phrase.Add(new Chunk("Item Weight (kg)", Heading787));
                        //cell = new PdfPCell(phrase);
                        //cell.Colspan = 1;
                        //cell.Border = 1;
                        //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                        //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        //headertable2.AddCell(cell);

                        //phrase = new Phrase();
                        //phrase.Add(new Chunk("Quantity", Heading787));
                        //cell = new PdfPCell(phrase);
                        //cell.Colspan = 1;
                        //cell.Border = 1;
                        //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                        //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        //headertable2.AddCell(cell);

                        //phrase = new Phrase();
                        //phrase.Add(new Chunk("Total Weight (kg)", Heading787));
                        //cell = new PdfPCell(phrase);
                        //cell.Colspan = 1;
                        //cell.Border = 1;
                        //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                        //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        //headertable2.AddCell(cell);

                    //    rowcount = 0;
                    //}
                    //else
                    //{ // 2Nov2019 comment Else Condition bcz data rows overlapping on row pagebreak

                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dtTable.Rows[i]["MaterialType"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 7f;
                    cell.PaddingTop = 7f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);



                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dtTable.Rows[i]["ItemCode"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 7f;
                    cell.PaddingTop = 7f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);


                    phrase = new Phrase();
                    //paragraph. = 2 ;
                    // phrase.Add(new Chunk("0123456789012345678901234567890123456789012345", DataFont));
                    ItemDescription = Convert.ToString(dtTable.Rows[i]["ItemDesc"]).ToString();


                    //if (ItemDescription.Length > 39)
                    //{
                    //    rowcount++;
                    //    // phrase.Leading = 40;
                    //    phrase.Add(new Chunk(ItemDescription.PadRight(160), DataFont));
                    //}
                    //else
                    //{
                    //    phrase.Add(new Chunk(ItemDescription, DataFont));
                    //}
                    phrase.Add(new Chunk(ItemDescription, DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 7f;
                    cell.PaddingTop = 7f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    phrase = new Phrase();
                    string itemweight = String.Format("{0:#,#0.00}", dtTable.Rows[i]["ItemWeight"]);
                    phrase.Add(new Chunk(itemweight, DataFont));
                    //phrase.Add(new Chunk(Convert.ToString(dtTable.Rows[i]["ItemWeight"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 7f;
                    cell.PaddingTop = 7f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    phrase = new Phrase();
                    string quanti = String.Format("{0:#,##0}", dtTable.Rows[i]["Quantity"]);
                    phrase.Add(new Chunk(quanti, DataFont));
                    //phrase.Add(new Chunk(Convert.ToInt32(dtTable.Rows[i]["Quantity"]).ToString(), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 7f;
                    cell.PaddingTop = 7f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);
                    //cell.BackgroundColor = Color.LIGHT_GRAY;

                    phrase = new Phrase();
                    string Totalweight = String.Format("{0:#,##0.00}", dtTable.Rows[i]["TotalWeight"]);
                    phrase.Add(new Chunk(Totalweight, DataFont));
                    //phrase.Add(new Chunk(Convert.ToString(dtTable.Rows[i]["TotalWeight"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 7f;
                    cell.PaddingTop = 7f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    //rowcount++;
                    //}

                }
                ////int rowcounts = 1;
                //for (int i = 1; i < (33 - rowcount); i++)
                //{
                //    phrase = new Phrase();
                //    phrase.Add(new Chunk("", DataFont));
                //    cell = new PdfPCell(phrase);
                //    //cell.PaddingBottom =11f;
                //    //cell.PaddingTop = 11f;
                //    cell.FixedHeight = 25.5f;
                //    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                //    cell.Border = 1;
                //    cell.Colspan = 6;
                //    cell.Border = 0;
                //    headertable2.AddCell(cell);


                //    //rowcounts++;
                //}

            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 6;
                cell.Border = 0;
                cell.PaddingBottom = 6F;

                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

            }

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);


            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);


            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("cont.", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable2.AddCell(cell);

            // from here create one more table for saprate  other   details of docate  table 4
            phrase = new Phrase();
            phrase.Add(new Chunk("Sign-Off's-Main Depot", Heading788));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable4.AddCell(cell);


            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.Border = 0;
            cell.PaddingTop = -8f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable4.AddCell(cell);

            if (!string.IsNullOrEmpty(Convert.ToString(dtdata.Rows[0]["DepotPic"])))
            {
                var uri = new Uri(databaseCon.siteimagelink);
                var container = new CloudBlobContainer(uri);
                var blob = container.GetBlockBlobReference(dtdata.Rows[0]["DepotPic"].ToString());
                var noimage = Server.MapPath("~/images/noimg.jpg");
                var exists = blob.Exists();
                if (exists == false)
                {
                    string nophoto = Server.MapPath("~/images/noimg.jpg");
                    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(noimage), true);
                    //cell.Width = 550f;
                    cell.FixedHeight = 270f;
                    cell.Border = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                    cell.Colspan = 2;
                }
                else
                {

                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(databaseCon.siteimagelink + dtdata.Rows[0]["DepotPic"].ToString());
                    img.ScaleAbsolute(180f, 133f);
                    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img), true);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Border = 1;
                    cell.FixedHeight = 270f;
                    cell.Colspan = 6;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                }
            }
            else
            {
                string nophoto = Server.MapPath("~/images/noimg.jpg");
                cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
                cell.Colspan = 2;
                cell.FixedHeight = 270f;
                cell.Border = 1;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            }
            cell.Border = 1;
            cell.FixedHeight = 270f;
            cell.Colspan = 3;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable4.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading787));
            cell = new PdfPCell(phrase);
            cell.FixedHeight = 270f;
            cell.Colspan = 4;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable4.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.Border = 0;
            cell.PaddingTop = -8f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable4.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.Border = 0;
            cell.PaddingTop = -8f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable4.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Picked By:", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 2;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable4.AddCell(cell);

            if (dtdata.Rows.Count > 0)
            {

                phrase = new Phrase();
                phrase.Add(new Chunk(Convert.ToString(dtdata.Rows[0]["DepotPickedBy"].ToString()), DataFont));
                cell = new PdfPCell(phrase);
                //cell.PaddingBottom = 5f;
                //cell.PaddingTop = 8f;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;

                cell.Colspan = 1;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                headertable4.AddCell(cell);


            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 0;
                //cell.PaddingBottom = 6F;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable4.AddCell(cell);
            }

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 3;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable4.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Signature", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable4.AddCell(cell);


            if (!string.IsNullOrEmpty(Convert.ToString(dtdata.Rows[0]["DepotPersonSign"])))
            {
                var uri = new Uri(databaseCon.siteimagelink);
                var container = new CloudBlobContainer(uri);
                var blob = container.GetBlockBlobReference(dtdata.Rows[0]["DepotPersonSign"].ToString());
                var noimage = Server.MapPath("~/images/noimg.jpg");
                var exists = blob.Exists();
                if (exists == false)
                {
                    //string nophoto = Server.MapPath("images/noimg.jpg");
                    //cell = new PdfPCell(iTextSharp.text.Image.GetInstance(noimage), true);
                    //cell.FixedHeight = 150f;
                    //cell.BorderColor = Color.LIGHT_GRAY;
                    //cell.Colspan = 4;
                    phrase = new Phrase();
                    phrase.Add(new Chunk("", Heading787));
                    cell = new PdfPCell(phrase);
                    cell.Colspan = 4;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.PaddingLeft = 10f;
                    cell.PaddingBottom = 10f;
                    cell.PaddingRight = 150f;

                    cell.PaddingTop = 10f;
                    cell.BorderColor = BaseColor.GRAY;
                    cell.Border = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    //cell.PaddingTop = 20f;
                }
                else
                {
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(databaseCon.siteimagelink + dtdata.Rows[0]["DepotPersonSign"].ToString());
                    img.ScaleAbsolute(180f, 133f);

                    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img), true);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderColor = BaseColor.GRAY;
                    //cell.FixedHeight = 200f;
                    cell.PaddingLeft = 1f;
                    cell.PaddingBottom = 1f;
                    cell.PaddingRight = 100f;

                    cell.PaddingTop = 1f;
                    cell.Colspan = 4;
                    cell.Border = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.PaddingLeft = 10f;
                cell.PaddingBottom = 10f;
                cell.PaddingRight = 150f;

                cell.PaddingTop = 10f;
                cell.BorderColor = BaseColor.GRAY;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                //cell.PaddingTop = 20f;
            }

            cell.Colspan = 4;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.FixedHeight = 150f;
            cell.Border = 1;
            cell.BorderColor = BaseColor.GRAY;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable4.AddCell(cell);



            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 2;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable4.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.Border = 0;
            cell.PaddingTop = -8f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable4.AddCell(cell);

            phrase = new Phrase();
            //phrase.Add(new Chunk("Transport By(Name):", Heading787));//  rename as per client request on  21 feb 2025 by ss
            phrase.Add(new Chunk("Driver Name :", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 2;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable4.AddCell(cell);

            if (dtdata.Rows.Count > 0)
            {

                phrase = new Phrase();
                phrase.Add(new Chunk(Convert.ToString(dtdata.Rows[0]["TransportBy"].ToString()), DataFont));
                cell = new PdfPCell(phrase);
                //cell.PaddingBottom = 5f;
                //cell.PaddingTop = 8f;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;

                cell.Colspan = 1;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                headertable4.AddCell(cell);


            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 0;
                //cell.PaddingBottom = 6F;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable4.AddCell(cell);
            }

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 3;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable4.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.Border = 0;
            cell.PaddingTop = -8f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable4.AddCell(cell);

            phrase = new Phrase();
            //phrase.Add(new Chunk("Transport CompName:", Heading787));// commented as per client request 
            phrase.Add(new Chunk("Transport Company :", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 2;
            //cell.FixedHeight = 13f;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable4.AddCell(cell);

            if (dtdata.Rows.Count > 0)
            {

                phrase = new Phrase();
                phrase.Add(new Chunk(Convert.ToString(dtdata.Rows[0]["TransportCompName"].ToString()), DataFont));
                cell = new PdfPCell(phrase);
                //cell.PaddingBottom = 5f;
                //cell.PaddingTop = 8f;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell.FixedHeight = 13f;
                cell.Colspan = 1;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                headertable4.AddCell(cell);


            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.FixedHeight = 13f;
                cell.Border = 0;
                //cell.PaddingBottom = 6F;
                //cell.PaddingTop = -50f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable4.AddCell(cell);
            }



            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 3;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable4.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable4.AddCell(cell);



            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.Border = 0;
            cell.PaddingTop = -8f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable4.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Signature Of Driver ", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable4.AddCell(cell);

            if (!string.IsNullOrEmpty(Convert.ToString(dtdata.Rows[0]["TransportDriverSign"])))
            {
                var uri = new Uri(databaseCon.siteimagelink);
                var container = new CloudBlobContainer(uri);
                var blob = container.GetBlockBlobReference(dtdata.Rows[0]["TransportDriverSign"].ToString());
                var noimage = Server.MapPath("~/images/noimg.jpg");
                var exists = blob.Exists();
                if (exists == false)
                {
                    //string nophoto = Server.MapPath("images/noimg.jpg");
                    //cell = new PdfPCell(iTextSharp.text.Image.GetInstance(noimage), true);
                    //cell.FixedHeight = 150f;
                    //cell.BorderColor = Color.LIGHT_GRAY;
                    //cell.Colspan = 4;
                    phrase = new Phrase();
                    phrase.Add(new Chunk("", Heading787));
                    cell = new PdfPCell(phrase);
                    cell.Colspan = 2;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.PaddingLeft = 10f;
                    cell.PaddingBottom = 10f;
                    cell.PaddingRight = 150f;
                    cell.Border = 1;
                    cell.PaddingTop = 10f;
                    cell.BorderColor = BaseColor.GRAY;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    //cell.PaddingTop = 20f;
                }
                else
                {
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(databaseCon.siteimagelink + dtdata.Rows[0]["TransportDriverSign"].ToString());
                    img.ScaleAbsolute(180f, 133f);

                    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img), true);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderColor = BaseColor.GRAY;
                    //cell.FixedHeight = 200f;
                    cell.PaddingLeft = 1f;
                    cell.PaddingBottom = 1f;
                    cell.PaddingRight = 100f;
                    cell.Border = 1;
                    cell.PaddingTop = 1f;
                    cell.Colspan = 4;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 2;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.PaddingLeft = 10f;
                cell.PaddingBottom = 10f;
                cell.PaddingRight = 150f;
                cell.Border = 1;
                cell.PaddingTop = 10f;
                cell.BorderColor = BaseColor.GRAY;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                //cell.PaddingTop = 20f;
            }
            cell.Border = 1;
            cell.Colspan = 4;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.FixedHeight = 150f;
            cell.BorderColor = BaseColor.GRAY;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable4.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 2;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable4.AddCell(cell);



            if (Convert.ToBoolean(dtdata.Rows[0]["ReceiptConfirmation"]) == true)
            {

                for (int i = 1; i < (5); i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("", DataFont));
                    cell = new PdfPCell(phrase);
                    //cell.PaddingBottom =11f;
                    //cell.PaddingTop = 11f;
                    cell.FixedHeight = 25.5f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 6;
                    cell.Border = 0;
                    headertable4.AddCell(cell);


                    //rowcounts++;
                }


                phrase = new Phrase();
                phrase.Add(new Chunk("Sign-off's-Site", Heading788));
                cell = new PdfPCell(phrase);
                cell.Colspan = 6;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable4.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 6;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable4.AddCell(cell);

                if (!string.IsNullOrEmpty(Convert.ToString(dtdata.Rows[0]["SitePic"])))
                {
                    var uri = new Uri(databaseCon.siteimagelink);
                    var container = new CloudBlobContainer(uri);
                    var blob = container.GetBlockBlobReference(dtdata.Rows[0]["SitePic"].ToString());
                    var noimage = Server.MapPath("~/images/noimg.jpg");
                    var exists = blob.Exists();
                    if (exists == false)
                    {
                        string nophoto = Server.MapPath("~/images/noimg.jpg");
                        cell = new PdfPCell(iTextSharp.text.Image.GetInstance(noimage), true);
                        cell.FixedHeight = 270f;
                        cell.Border = 1;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                        cell.Colspan = 2;
                    }
                    else
                    {
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(databaseCon.siteimagelink + dtdata.Rows[0]["SitePic"].ToString());
                        img.ScaleAbsolute(180f, 133f);

                        cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img), true);
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.Border = 1;
                        cell.FixedHeight = 270f;
                        cell.Colspan = 2;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    }
                }
                else
                {
                    string nophoto = Server.MapPath("~/images/noimg.jpg");
                    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
                    cell.FixedHeight = 270f;
                    cell.Colspan = 2;
                    cell.Border = 1;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                }
                cell.Border = 1;
                cell.FixedHeight = 270f;
                cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                headertable4.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.FixedHeight = 270f;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable4.AddCell(cell);


                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 6;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable4.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("For Scaffold Company", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 2;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable4.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable4.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Date Received On Site:", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 2;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable4.AddCell(cell);

                if (dtdata.Rows.Count > 0)
                {

                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dtdata.Rows[0]["SiteReceivedDate"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 5f;
                    //cell.PaddingTop = 8f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 1;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable4.AddCell(cell);


                }
                else
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("", DataFont));
                    cell = new PdfPCell(phrase);
                    cell.Colspan = 1;
                    cell.Border = 0;
                    //cell.PaddingBottom = 6F;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    headertable4.AddCell(cell);
                }

                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 3;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable4.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 6;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable4.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Received By(Name):", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 2;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable4.AddCell(cell);

                if (dtdata.Rows.Count > 0)
                {

                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dtdata.Rows[0]["SiteReceivedBy"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 5f;
                    //cell.PaddingTop = 8f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;

                    cell.Colspan = 1;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable4.AddCell(cell);


                }
                else
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("", DataFont));
                    cell = new PdfPCell(phrase);
                    cell.Colspan = 1;
                    cell.Border = 0;
                    //cell.PaddingBottom = 6F;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    headertable4.AddCell(cell);
                }

                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 3;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable4.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 6;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable4.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Position:", Heading787));
                cell = new PdfPCell(phrase);
                //cell.FixedHeight = 13f;
                cell.Colspan = 2;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable4.AddCell(cell);


                if (dtdata.Rows.Count > 0)
                {

                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dtdata.Rows[0]["ReceivedByPosition"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 5f;
                    //cell.PaddingTop = 8f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //cell.FixedHeight = 13f;
                    cell.Colspan = 1;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable4.AddCell(cell);


                }
                else
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("", DataFont));
                    cell = new PdfPCell(phrase);
                    cell.Colspan = 1;
                    cell.Border = 0;
                    cell.FixedHeight = 13f;
                    //cell.PaddingBottom = 6F;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    headertable4.AddCell(cell);
                }

                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 3;
                cell.FixedHeight = 13f;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable4.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Signature", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 6;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable4.AddCell(cell);

                if (!string.IsNullOrEmpty(Convert.ToString(dtdata.Rows[0]["SitePersonSign"])))
                {
                    var uri = new Uri(databaseCon.siteimagelink);
                    var container = new CloudBlobContainer(uri);
                    var blob = container.GetBlockBlobReference(dtdata.Rows[0]["SitePersonSign"].ToString());
                    var noimage = Server.MapPath("~/images/noimg.jpg");
                    var exists = blob.Exists();
                    if (exists == false)
                    {
                        //string nophoto = Server.MapPath("images/noimg.jpg");
                        //cell = new PdfPCell(iTextSharp.text.Image.GetInstance(noimage), true);
                        //cell.FixedHeight = 150f;
                        //cell.BorderColor = Color.LIGHT_GRAY;
                        //cell.Colspan = 4;
                        phrase = new Phrase();
                        phrase.Add(new Chunk("", Heading787));
                        cell = new PdfPCell(phrase);
                        cell.Colspan = 2;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.PaddingLeft = 10f;
                        cell.PaddingBottom = 10f;
                        cell.PaddingRight = 150f;
                        cell.Border = 1;
                        cell.PaddingTop = 10f;
                        cell.BorderColor = BaseColor.GRAY;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        //cell.PaddingTop = 20f;
                    }
                    else
                    {
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(databaseCon.siteimagelink + dtdata.Rows[0]["SitePersonSign"].ToString());
                        img.ScaleAbsolute(180f, 133f);

                        cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img), true);
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.BorderColor = BaseColor.GRAY;
                        //cell.FixedHeight = 200f;
                        cell.PaddingLeft = 1f;
                        cell.PaddingBottom = 1f;
                        cell.PaddingRight = 100f;
                        cell.Border = 1;
                        cell.PaddingTop = 1f;
                        cell.Colspan = 4;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    }
                }
                else
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("", Heading787));
                    cell = new PdfPCell(phrase);
                    cell.Colspan = 2;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.PaddingLeft = 10f;
                    cell.PaddingBottom = 10f;
                    cell.PaddingRight = 150f;
                    cell.Border = 1;
                    cell.PaddingTop = 10f;
                    cell.BorderColor = BaseColor.GRAY;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    //cell.PaddingTop = 20f;
                }
                cell.Border = 1;
                cell.Colspan = 4;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.FixedHeight = 150f;
                cell.BorderColor = BaseColor.GRAY;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                headertable4.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 2;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable4.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 6;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable4.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("For Client", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 6;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable4.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Accepted By(Name):", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 2;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable4.AddCell(cell);

                if (dtdata.Rows.Count > 0)
                {

                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dtdata.Rows[0]["AcceptedBy"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 5f;
                    //cell.PaddingTop = 8f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;

                    cell.Colspan = 1;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable4.AddCell(cell);


                }
                else
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("", DataFont));
                    cell = new PdfPCell(phrase);
                    cell.Colspan = 1;
                    cell.Border = 0;
                    //cell.PaddingBottom = 6F;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    headertable4.AddCell(cell);
                }

                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 3;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable4.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 6;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable4.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Position:", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 2;
                //cell.FixedHeight = 13f;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable4.AddCell(cell);

                if (dtdata.Rows.Count > 0)
                {

                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dtdata.Rows[0]["ClientPosition"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    //cell.FixedHeight = 5f;
                    //cell.FixedHeight = 13f;
                    //cell.PaddingBottom = 5f;
                    //cell.PaddingTop = 8f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable4.AddCell(cell);


                }
                else
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("", DataFont));
                    cell = new PdfPCell(phrase);
                    cell.Colspan = 1;
                    cell.FixedHeight = 13f;
                    cell.Border = 1;
                    //cell.PaddingBottom = 6F;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    headertable4.AddCell(cell);
                }

                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 3;
                cell.FixedHeight = 13f;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable4.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Signature", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 6;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable4.AddCell(cell);

                if (!string.IsNullOrEmpty(Convert.ToString(dtdata.Rows[0]["AcceptorSign"])))
                {
                    var uri = new Uri(databaseCon.siteimagelink);
                    var container = new CloudBlobContainer(uri);
                    var blob = container.GetBlockBlobReference(dtdata.Rows[0]["AcceptorSign"].ToString());
                    var noimage = Server.MapPath("~/images/noimg.jpg");
                    var exists = blob.Exists();
                    if (exists == false)
                    {
                        //string nophoto = Server.MapPath("images/noimg.jpg");
                        //cell = new PdfPCell(iTextSharp.text.Image.GetInstance(noimage), true);
                        //cell.FixedHeight = 150f;
                        //cell.BorderColor = Color.LIGHT_GRAY;
                        //cell.Colspan = 4;
                        phrase = new Phrase();
                        phrase.Add(new Chunk("", Heading787));
                        cell = new PdfPCell(phrase);
                        cell.Colspan = 2;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.PaddingLeft = 10f;
                        cell.PaddingBottom = 10f;
                        cell.PaddingRight = 150f;
                        cell.Border = 1;
                        cell.PaddingTop = 10f;
                        cell.BorderColor = BaseColor.GRAY;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        //cell.PaddingTop = 20f;
                    }
                    else
                    {
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(databaseCon.siteimagelink + dtdata.Rows[0]["AcceptorSign"].ToString());
                        img.ScaleAbsolute(180f, 133f);

                        cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img), true);
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.BorderColor = BaseColor.GRAY;
                        //cell.FixedHeight = 200f;
                        cell.PaddingLeft = 1f;
                        cell.PaddingBottom = 1f;
                        cell.PaddingRight = 100f;
                        cell.Border = 1;
                        cell.PaddingTop = 1f;
                        cell.Colspan = 4;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    }
                }
                else
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("", Heading787));
                    cell = new PdfPCell(phrase);
                    cell.Colspan = 2;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.PaddingLeft = 10f;
                    cell.PaddingBottom = 10f;
                    cell.PaddingRight = 150f;
                    cell.Border = 1;
                    cell.PaddingTop = 10f;
                    cell.BorderColor = BaseColor.GRAY;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    //cell.PaddingTop = 20f;
                }
                cell.Border = 1;
                cell.Colspan = 4;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.FixedHeight = 150f;
                cell.BorderColor = BaseColor.GRAY;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                headertable4.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 2;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable4.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Receipt Confirmation", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 2;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable4.AddCell(cell);

                if (string.IsNullOrEmpty(Convert.ToString(dtdata.Rows[0]["ReceiptConfirmation"])))
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("\x6F", contentfontUncheck));
                    cell = new PdfPCell(phrase);
                }
                else
                {
                    if (Convert.ToBoolean(dtdata.Rows[0]["ReceiptConfirmation"]) == true)
                    {
                        phrase = new Phrase();
                        phrase.Add(new Chunk("\xFE", contentfontCheck));
                        cell = new PdfPCell(phrase);
                        cell.PaddingLeft = -30f;
                    }
                    else
                    {
                        phrase = new Phrase();
                        phrase.Add(new Chunk("\x6F", contentfontUncheck));
                        cell = new PdfPCell(phrase);
                        cell.PaddingLeft = -30f;
                    }
                }
                cell.Border = 0;
                cell.BorderColor = BaseColor.GRAY;
                //cell.FixedHeight = 1f;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                cell.Colspan = 1;
                headertable4.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 3;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable4.AddCell(cell);
            }

            doc.Add(headertable1);
            doc.Add(headertable3);
            doc.Add(headertable2);
            doc.NewPage();
            doc.Add(headertable4);
            doc.Close();

        }


    }
}

