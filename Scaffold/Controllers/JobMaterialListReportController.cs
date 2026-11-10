using iTextSharp.text;
using iTextSharp.text.pdf;
using Scaffold.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scaffold.Controllers
{
    public class JobMaterialListReportController : Controller
    {
        float Totalquantity = 0;
        float WeightTotal = 0;
        // GET: JobMaterialListReport
        public ActionResult Index()
        {
            return View();
        }

        //public void JobMaterialListReport(int id, int SiteId, int RID)
            public void JobMaterialListReport(string UID, string UserRole, string id,string RID, string checkid,string SiteId)
        {
           // transId = databaseCon.Decrypt(transId);
           // int id = 0, SiteId = 153,  RID = 1;
            string query = "";
            string queryheader = "select ClientImage,SiteLocation,right('000000' + convert(varchar,tblRegister.ScaffRef),6) as Scaffref,Description from tblClient join tblSiteLocation on tblclient.clientId = tblsitelocation.clientId join tblRegister on tblClient.ClientID=tblregister.clientId where tblClient.ClientId  = '" + databaseCon.Decrypt(id) + "' and tblsitelocation.sitelocid='" + databaseCon.Decrypt(Request.QueryString["SiteId"]) + "' and tblregister.registerId='" + databaseCon.Decrypt(Request.QueryString["RID"]) + "'";
            DataTable dtheader = databaseCon.GetData(queryheader);


            query = "select tblTransactionHistDelivery.TransDeliId,tblTransactionHistRequest.FromId,tblTransactionHistRequest.ToId,TransId, tblTransactionHistRequest.TransReqId,MaterialType,tblTransactionHistRequest.InventoryId,ItemCode,Left(ItemDesc,30) as ItemDesc,cast(Isnull(AvailableQuantity,0) as int) as AvailableQuantity , isnull (tblTransactionHistRequest.Quantity,0) as Quantity,Isnull (tblTransactionHistDelivery.Quantity,0) as AllocatedQuantity, ItemWeight  , Isnull(tblTransactionHistDelivery.Quantity*ItemWeight,0) as TotalWeight  from tblTransactionHistRequest left join VW_SiteInventory on tblTransactionHistRequest.InventoryId = VW_SiteInventory.InventoryId left join tblInventory on tblInventory.InventoryID = tblTransactionHistRequest.InventoryId left join tblTransactionHistDelivery on tblTransactionHistRequest.TransReqId = tblTransactionHistDelivery.RequestId where TransId in ('" + databaseCon.Decrypt(Request.QueryString["checkid"]) + "') and SiteID = '" + databaseCon.Decrypt(Request.QueryString["SiteId"]) + "' order by MaterialType ";
            DataTable dt = databaseCon.GetData(query);

            iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, -50f, -50f, 20f, 20f);   //Page Size set top ,left,margin
                                                                                                                              // doc.SetPageSize(iTextSharp.text.PageSize.A4);

            iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, Response.OutputStream);
            Response.ContentType = ("application/pdf");
            HeaderFooter2 evnt = new HeaderFooter2();
            writer.PageEvent = evnt;
            doc.Open();
            var Heading786 = FontFactory.GetFont("Arial", 10f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            //Heading786.BaseColor = BaseColor.GRAY;
            var Heading787 = FontFactory.GetFont("Arial", 20f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            var DataFont = FontFactory.GetFont("Arial", 10f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            Phrase phrase;
            PdfPCell cell;

            PdfPTable headertable1 = new PdfPTable(5);
            headertable1.SetWidths(new int[] { 10, 9, 25, 9, 9 });

            //Table Start

            PdfPTable headertable2 = new PdfPTable(8);
            headertable2.SetWidths(new int[] { 10, 11, 25, 9, 9, 9, 8, 8 });

            //phrase = new Phrase();
            //phrase.Add(new Chunk("", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 6;
            //cell.PaddingBottom = 6f;
            //cell.Border = 0;
            //cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //headertable2.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk("", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 6;
            //cell.PaddingBottom = 6f;
            //cell.Border = 0;
            //cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //headertable2.AddCell(cell);


            //if (!string.IsNullOrEmpty(Convert.ToString(dtheader.Rows[0]["ClientImage"])))
            //{
            //    var uri = new Uri(databaseCon.clientimageLink);
            //    // var container = new CloudBlobContainer(uri);
            //    //var blob = container.GetBlockBlobReference(dtheader.Rows[0]["ClientImage"].ToString());
            //    var noimage = Server.MapPath("Images/Logo2.png");
            //    //var exists = blob.Exists();
            //    if (Convert.ToString(dtheader.Rows[0]["ClientImage"]) == "")
            //    {
            //        string nophoto = Server.MapPath("images/Logo2.png");
            //        cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
            //        cell.FixedHeight = 150f;
            //        cell.Colspan = 2;
            //        cell.PaddingLeft = 10f;
            //        cell.PaddingTop = 20;
            //    }
            //    else
            //    {
            //        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(Server.MapPath("images/clientlogo/" + dtheader.Rows[0]["ClientImage"].ToString()));

            //        img.ScaleAbsolute(180f, 133f);
            //        cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img), true);
            //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //        cell.FixedHeight = 150f;
            //        cell.Colspan = 2;
            //        cell.PaddingLeft = 10f;
            //        cell.PaddingTop = 20f;

            //    }
            //}
            //else
            //{
            //    string nophoto = Server.MapPath("images/Logo2.png");
            //    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
            //    cell.FixedHeight = 150f;
            //    cell.PaddingLeft = 10f;
            //    cell.Colspan = 2;
            //    cell.PaddingTop = 20f;
            //}
            //cell.Border = 0;
            //cell.Colspan = 2;
            //cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //cell.PaddingLeft = 20f;
            //cell.PaddingTop = 30f;
            //headertable2.AddCell(cell);


            //phrase = new Phrase();
            //phrase.Add(new Chunk("Job Delivery List", Heading787));
            ////phrase.Add(new Chunk(Convert.ToString(dtheader.Rows[0]["SiteName"].ToString()), Heading787));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 6;
            //cell.PaddingBottom = 6f;
            //cell.PaddingLeft = 100f;
            //cell.PaddingTop = 30f;
            //cell.Border = 0;
            //cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //headertable2.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk("", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 8;
            //cell.PaddingBottom = 6f;
            //cell.Border = 0;
            ////cell.PaddingTop = -50f;
            //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //headertable2.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk("Site", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            ////cell.PaddingTop = -50f;
            //cell.PaddingBottom = 6f;
            //cell.Border = 0;
            //cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //headertable2.AddCell(cell);

            //if (dtheader.Rows.Count > 0)
            //{

            //    phrase = new Phrase();
            //    phrase.Add(new Chunk(Convert.ToString(dtheader.Rows[0]["SiteLocation"].ToString()), DataFont));
            //    cell = new PdfPCell(phrase);
            //    //cell.PaddingBottom = 5f;
            //    //cell.PaddingTop = 8f;
            //    cell.HorizontalAlignment = Element.ALIGN_CENTER;

            //    cell.Colspan = 2;
            //    //cell.PaddingTop = -50f;
            //    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            //    headertable2.AddCell(cell);


            //}
            //else
            //{
            //    phrase = new Phrase();
            //    phrase.Add(new Chunk("", DataFont));
            //    cell = new PdfPCell(phrase);
            //    cell.Colspan = 2;
            //    cell.Border = 3;
            //    cell.PaddingBottom = 6F;
            //    //cell.PaddingTop = -50f;
            //    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            //    cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //    headertable2.AddCell(cell);
            //}

            //phrase = new Phrase();
            //phrase.Add(new Chunk("", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            ////cell.PaddingTop = -50f;
            //cell.PaddingBottom = 6f;
            //cell.Border = 0;
            //cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //headertable2.AddCell(cell);



            //phrase = new Phrase();
            //phrase.Add(new Chunk("Scaf Ref. No:", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 2;
            //cell.PaddingBottom = 6f;
            //cell.Border = 0;
            ////cell.PaddingTop = -50f;
            //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //headertable2.AddCell(cell);

            //if (dtheader.Rows.Count > 0)
            //{

            //    phrase = new Phrase();
            //    phrase.Add(new Chunk(Convert.ToString(dtheader.Rows[0]["Scaffref"].ToString()), DataFont));
            //    cell = new PdfPCell(phrase);
            //    //cell.PaddingBottom = 15f;
            //    cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //    cell.Border = 1;
            //    cell.Colspan = 2;
            //    //cell.PaddingTop = -50f;
            //    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            //    headertable2.AddCell(cell);



            //}
            //else
            //{
            //    phrase = new Phrase();
            //    phrase.Add(new Chunk("", DataFont));
            //    cell = new PdfPCell(phrase);
            //    cell.Colspan = 2;
            //    cell.Border = 3;
            //    cell.PaddingBottom = 6F;
            //    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            //    cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //    headertable2.AddCell(cell);
            //}

            //phrase = new Phrase();
            //phrase.Add(new Chunk("", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 8;
            ////cell.PaddingBottom = 6f;
            //cell.Border = 0;
            ////cell.PaddingTop = -50f;
            //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //headertable2.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk("", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 8;
            ////cell.PaddingBottom = 6f;
            //cell.Border = 0;
            ////cell.PaddingTop = -50f;
            //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //headertable2.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk("Description", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.PaddingBottom = 6f;
            //cell.Border = 0;
            ////cell.PaddingTop = -50f;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //headertable2.AddCell(cell);

            //if (dtheader.Rows.Count > 0)
            //{

            //    phrase = new Phrase();
            //    phrase.Add(new Chunk(Convert.ToString(dtheader.Rows[0]["Description"].ToString()), DataFont));
            //    cell = new PdfPCell(phrase);
            //    //cell.PaddingBottom = 15f;
            //    cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //    cell.Border = 1;
            //    cell.Colspan = 7;
            //    //cell.PaddingTop = -50f;
            //    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            //    headertable2.AddCell(cell);



            //}
            //else
            //{
            //    phrase = new Phrase();
            //    phrase.Add(new Chunk("", DataFont));
            //    cell = new PdfPCell(phrase);
            //    cell.Colspan = 7;
            //    cell.Border = 3;
            //    cell.PaddingBottom = 6F;
            //    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            //    cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //    headertable2.AddCell(cell);
            //}

            //phrase = new Phrase();
            //phrase.Add(new Chunk("", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 8;
            ////cell.PaddingBottom = 6f;
            //cell.Border = 0;
            ////cell.PaddingTop = -50f;
            //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //headertable2.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk("", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 8;
            ////cell.PaddingBottom = 6f;
            //cell.Border = 0;
            ////cell.PaddingTop = -50f;
            //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //headertable2.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk("TransId", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.Border = 1;
            //cell.PaddingBottom = 6f;
            //cell.BackgroundBaseColor = BaseColor.LIGHT_GRAY;
            //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;

            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //headertable2.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk("Item", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.PaddingBottom = 6f;
            //cell.Border = 1;
            //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            //cell.BackgroundBaseColor = BaseColor.LIGHT_GRAY;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //headertable2.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk("Item Description", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.Border = 1;
            //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            //cell.BackgroundBaseColor = BaseColor.LIGHT_GRAY;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //headertable2.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk("Available Quantity", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.Border = 1;
            //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            //cell.BackgroundBaseColor = BaseColor.LIGHT_GRAY;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //headertable2.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk("Requested Quantity", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.Border = 1;
            //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            //cell.BackgroundBaseColor = BaseColor.LIGHT_GRAY;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //headertable2.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk("Allocated Quantity", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.Border = 1;
            //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            //cell.BackgroundBaseColor = BaseColor.LIGHT_GRAY;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //headertable2.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk("Item Weight (kg)", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.Border = 1;
            //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            //cell.BackgroundBaseColor = BaseColor.LIGHT_GRAY;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //headertable2.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk("Total Weight (kg)", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.Border = 1;
            //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            //cell.BackgroundBaseColor = BaseColor.LIGHT_GRAY;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //headertable2.AddCell(cell);
            //Table End

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["TransId"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);



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
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["ItemDesc"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    phrase = new Phrase();
                    string Avaiqty = String.Format("{0:#,##0}", dt.Rows[i]["AvailableQuantity"]);
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
                    string qty = String.Format("{0:#,##0}", dt.Rows[i]["Quantity"]);
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
                    string Allocatedqty = String.Format("{0:#,##0}", dt.Rows[i]["AllocatedQuantity"]);
                    phrase.Add(new Chunk(Allocatedqty, DataFont));
                    //phrase.Add(new Chunk(Convert.ToInt32(dt.Rows[i]["AllocatedQuantity"]).ToString(), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);
                    Totalquantity += Convert.ToInt32(dt.Rows[i]["AllocatedQuantity"]);
                    //cell.BackgroundBaseColor = BaseColor.LIGHT_GRAY;

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
                    string Totalweight = String.Format("{0:#,##0.00}", dt.Rows[i]["TotalWeight"]);
                    phrase.Add(new Chunk(Totalweight, DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
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
                cell.Colspan = 8;
                cell.Border = 0;
                cell.PaddingBottom = 6F;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

            }
            phrase = new Phrase();
            phrase.Add(new Chunk("", DataFont));
            cell = new PdfPCell(phrase);
            cell.Colspan = 4;
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
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            headertable2.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk("", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 8;
            //cell.PaddingBottom = 6f;
            //cell.Border = 0;
            //cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //headertable2.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk("", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 8;
            //cell.PaddingBottom = 6f;
            //cell.Border = 0;
            //cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //headertable2.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk("Printed:", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.PaddingBottom = 6f;
            //cell.Border = 0;
            //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //headertable2.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk("" + DateTime.Now.ToString("dd-MMM-yyy") + "", DataFont));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 7;
            //cell.PaddingBottom = 6f;
            //cell.Border = 0;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //headertable2.AddCell(cell);

            doc.Add(headertable2);
            doc.Close();

        }
    }

        public class HeaderFooter2 : iTextSharp.text.pdf.PdfPageEventHelper
        {

            List<PdfTemplate> pageNumberTemplates = new List<PdfTemplate>();// page header

            public override void OnStartPage(PdfWriter writer, Document doc)
            {
                int id, SiteId, RID;
                id = Convert.ToInt32(databaseCon.Decrypt(HttpContext.Current.Request.QueryString["Id"]));
                SiteId = Convert.ToInt32(databaseCon.Decrypt(HttpContext.Current.Request.QueryString["SiteId"]));
                RID = Convert.ToInt32(databaseCon.Decrypt(HttpContext.Current.Request.QueryString["RID"]));
                base.OnStartPage(writer, doc);
                //string queryheader = "select ClientImage from tblClient where tblClient.ClientId  = '" + id + "'";
                //DataTable dtheader = databaseCon.GetData(queryheader);

                string queryheader = "select ClientImage,SiteLocation,right('000000' + convert(varchar,tblRegister.ScaffRef),6) as Scaffref,Description from tblClient join tblSiteLocation on tblclient.clientId = tblsitelocation.clientId join tblRegister on tblClient.ClientID=tblregister.clientId where tblClient.ClientId  = '" + id + "' and tblsitelocation.sitelocid='" + SiteId + "' and tblregister.registerId='" + RID + "'";
                DataTable dtheader = databaseCon.GetData(queryheader);

                string ImagePath = System.Web.HttpContext.Current.Server.MapPath("~/Images/logo2.png");// image size
               // Rectangle pageSize = doc.PageSize;
                PdfPTable headerTbl = new PdfPTable(2);
                headerTbl.SetWidths(new int[] { 50, 50 });

                PdfPTable headertable2 = new PdfPTable(8);
                headertable2.SetWidths(new int[] { 10, 11, 25, 9, 9, 9, 8, 8 });

                var contentfont = FontFactory.GetFont("Arial", 8f, BaseColor.BLACK);
                var Headings = FontFactory.GetFont("Arial", 9f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                var Head = FontFactory.GetFont("Arial", 11f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                var BackOrderheader = FontFactory.GetFont("Arial", 20f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                var Heading786 = FontFactory.GetFont("Arial", 10f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                var DataFont = FontFactory.GetFont("Arial", 10f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

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
                phrase.Add(new Chunk("Job Delivery List", BackOrderheader));//top haddings
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.PaddingTop = -10f;
                //cell.PaddingLeft = 5f;
                cell.Border = 0;
                headerTbl.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("", BackOrderheader));//top haddings
                cell = new PdfPCell(phrase);
                cell.Colspan = 2;
                //cell.PaddingTop = 20f;
                //cell.PaddingLeft = 5f;
                cell.Border = 0;
                headerTbl.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk(" ", BackOrderheader));//top haddings
                cell = new PdfPCell(phrase);
                cell.Colspan = 2;
                //cell.PaddingTop = 20f;
                //cell.PaddingLeft = 5f;
                cell.Border = 0;
                headerTbl.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Site", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                //cell.PaddingTop = -50f;
                cell.PaddingBottom = 6f;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable2.AddCell(cell);

                if (dtheader.Rows.Count > 0)
                {

                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dtheader.Rows[0]["SiteLocation"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 5f;
                    //cell.PaddingTop = 8f;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;

                    cell.Colspan = 2;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);


                }
                else
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("", DataFont));
                    cell = new PdfPCell(phrase);
                    cell.Colspan = 2;
                    cell.Border = 3;
                    cell.PaddingBottom = 6F;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    headertable2.AddCell(cell);
                }

                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                //cell.PaddingTop = -50f;
                cell.PaddingBottom = 6f;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable2.AddCell(cell);



                phrase = new Phrase();
                phrase.Add(new Chunk("Scaf Ref. No:", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 2;
                cell.PaddingBottom = 6f;
                cell.Border = 0;
                //cell.PaddingTop = -50f;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                headertable2.AddCell(cell);

                if (dtheader.Rows.Count > 0)
                {

                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dtheader.Rows[0]["Scaffref"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Border = 1;
                    cell.Colspan = 2;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);



                }
                else
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("", DataFont));
                    cell = new PdfPCell(phrase);
                    cell.Colspan = 2;
                    cell.Border = 3;
                    cell.PaddingBottom = 6F;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    headertable2.AddCell(cell);
                }

                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 8;
                //cell.PaddingBottom = 6f;
                cell.Border = 0;
                //cell.PaddingTop = -50f;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk(" ", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 8;
                //cell.PaddingBottom = 6f;
                cell.Border = 0;
                //cell.PaddingTop = -50f;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Reason for Access:", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 2;
                cell.PaddingBottom = 6f;
                cell.Border = 0;
                //cell.PaddingTop = -50f;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                if (dtheader.Rows.Count > 0)
                {

                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dtheader.Rows[0]["Description"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 6;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);



                }
                else
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("", DataFont));
                    cell = new PdfPCell(phrase);
                    cell.Colspan = 6;
                    cell.Border = 3;
                    cell.PaddingBottom = 6F;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    headertable2.AddCell(cell);
                }

                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 8;
                //cell.PaddingBottom = 6f;
                cell.Border = 0;
                //cell.PaddingTop = -50f;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk(" ", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 8;
                //cell.PaddingBottom = 6f;
                cell.Border = 0;
                //cell.PaddingTop = -50f;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("TransId", Heading786));
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
                phrase.Add(new Chunk("Requested Quantity", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Allocated Quantity", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

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


                doc.Add(headerTbl);
                doc.Add(headertable2);


            }
            public override void OnEndPage(PdfWriter writer, Document document)
            {

                Paragraph footerdate = new Paragraph(("Printed: " + DateTime.Now.ToString("dd-MMM-yyy")), FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK));
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
}