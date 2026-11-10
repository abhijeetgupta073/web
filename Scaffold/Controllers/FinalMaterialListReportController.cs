using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Scaffold.App_Code;

namespace Scaffold.Controllers
{
    public class FinalMaterialListReportController : Controller
    {
        // GET: FinalMaterialListReport
        float Totalquantity = 0;
        float WeightTotal = 0;
        public ActionResult Index()
        {
            //int id, RID;
            //RID = Convert.ToInt32(databaseCon.Decrypt(Request.QueryString["RID"]));
            //id = Convert.ToInt32(databaseCon.Decrypt(Request.QueryString["Id"]));
            //showreport(RID, id);
            return View();
        }

       public  void showreport(string RegisterID, string ClientID)
        {
            int id, RID;
            RID = Convert.ToInt32(databaseCon.Decrypt(RegisterID));
            id = Convert.ToInt32(databaseCon.Decrypt(ClientID));

            string query = "";
            string queryheader = "select tblClient.ClientImage,Plans,right('000000' + convert(varchar,tblRegister.ScaffRef),6) as ScaffRef,[Description] from tblClient, tblRegister where tblClient.ClientId = tblRegister.ClientID and tblRegister.registerid = '" + RID + "'";
            DataTable dtheader = databaseCon.GetData(queryheader);

            query = "select optimizedQuantity,MaterialType,ItemCode,ItemDesc,ItemWeight,CAST((optimizedQuantity*ItemWeight) as decimal(18,2)) as TotalWeight from tblOptimizedInventory join tblInventory on tblOptimizedInventory.inventoryid=tblInventory.InventoryID where Registerid=" + RID + "and tblOptimizedInventory.Clientid=" + id + "order by MaterialType,isnull(DisplaySortOrder,9999999),ItemCode";

            DataTable dt = databaseCon.GetData(query);


            iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, -50f, -50f, 20f, 20f);   //Page Size set top ,left,margin
                                                                                                                              // doc.SetPageSize(iTextSharp.text.PageSize.A4);

            iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, Response.OutputStream);
            Response.ContentType = ("application/pdf");
            HeaderFooter2 evnt = new HeaderFooter2();
            writer.PageEvent = evnt;
            evnt.RID = RID;
            doc.Open();
            var Heading786 = FontFactory.GetFont("Arial", 10f, Font.BOLD, BaseColor.BLACK);
            //Heading786.Color = Color.GRAY;
            var Heading787 = FontFactory.GetFont("Arial", 25f, Font.BOLD, BaseColor.BLACK);
            var DataFont = FontFactory.GetFont("Arial", 10f, Font.NORMAL, BaseColor.BLACK);
            Phrase phrase;
            PdfPCell cell;

            PdfPTable headertable1 = new PdfPTable(5);
            headertable1.SetWidths(new int[] { 10, 9, 25, 9, 9 });

            //Table Start

            PdfPTable headertable2 = new PdfPTable(6);
            headertable2.SetWidths(new int[] { 10, 9, 20, 9, 9, 7 });

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
            //        cell.FixedHeight = 100f;
            //        cell.Colspan = 2;
            //        cell.PaddingLeft = 10f;
            //        //cell.PaddingTop = 10;
            //    }
            //    else
            //    {
            //        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(Server.MapPath("images/clientlogo/" + dtheader.Rows[0]["ClientImage"].ToString()));

            //        img.ScaleAbsolute(180f, 133f);
            //        cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img), true);
            //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //        cell.FixedHeight = 100f;
            //        cell.Colspan = 2;
            //        cell.PaddingLeft = 10f;
            //        // cell.PaddingTop = 10f;

            //    }
            //}
            //else
            //{
            //    string nophoto = Server.MapPath("images/Logo2.png");
            //    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
            //    cell.FixedHeight = 100f;
            //    cell.PaddingLeft = 10f;
            //    cell.Colspan = 2;
            //    //cell.PaddingTop = 10f;
            //}
            //cell.Border = 0;
            //cell.Colspan = 2;
            //cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //cell.PaddingLeft = 20f;
            ////cell.PaddingTop = 10f;
            //headertable2.AddCell(cell);


            //phrase = new Phrase();
            //phrase.Add(new Chunk("Material Finalised", Heading787));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 4;
            //cell.PaddingBottom = 6f;
            //cell.PaddingLeft = 100f;
            //cell.PaddingTop = 10f;
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

            //phrase = new Phrase();
            //phrase.Add(new Chunk("", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 6;
            //cell.PaddingBottom = 6f;
            //cell.Border = 0;
            //cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //headertable2.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk("Material Type", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.Border = 1;
            //cell.PaddingBottom = 6f;
            //cell.BackgroundColor = Color.LIGHT_GRAY;
            //cell.Border = PdfCell.TOP_BORDER | PdfCell.LEFT_BORDER | PdfCell.RIGHT_BORDER | PdfCell.ALIGN_BOTTOM;

            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //headertable2.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk("Item Code", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.PaddingBottom = 6f;
            //cell.Border = 1;
            //cell.Border = PdfCell.TOP_BORDER | PdfCell.LEFT_BORDER | PdfCell.RIGHT_BORDER | PdfCell.ALIGN_BOTTOM;
            //cell.BackgroundColor = Color.LIGHT_GRAY;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //headertable2.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk("Description", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.Border = 1;
            //cell.Border = PdfCell.TOP_BORDER | PdfCell.LEFT_BORDER | PdfCell.RIGHT_BORDER | PdfCell.ALIGN_BOTTOM;
            //cell.BackgroundColor = Color.LIGHT_GRAY;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //headertable2.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk("Quantity", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.Border = 1;
            //cell.Border = PdfCell.TOP_BORDER | PdfCell.LEFT_BORDER | PdfCell.RIGHT_BORDER | PdfCell.ALIGN_BOTTOM;
            //cell.BackgroundColor = Color.LIGHT_GRAY;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //headertable2.AddCell(cell);


            //phrase = new Phrase();
            //phrase.Add(new Chunk("Item Weight", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.Border = 1;
            //cell.Border = PdfCell.TOP_BORDER | PdfCell.LEFT_BORDER | PdfCell.RIGHT_BORDER | PdfCell.ALIGN_BOTTOM;
            //cell.BackgroundColor = Color.LIGHT_GRAY;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //headertable2.AddCell(cell);


            //phrase = new Phrase();
            //phrase.Add(new Chunk("Total Weight", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.Border = 1;
            //cell.Border = PdfCell.TOP_BORDER | PdfCell.LEFT_BORDER | PdfCell.RIGHT_BORDER | PdfCell.ALIGN_BOTTOM;
            //cell.BackgroundColor = Color.LIGHT_GRAY;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //headertable2.AddCell(cell);
            //Table End

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
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            headertable2.AddCell(cell);

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
            //cell.Colspan = 5;
            //cell.PaddingBottom = 6f;
            //cell.Border = 0;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //headertable2.AddCell(cell);

            doc.Add(headertable2);
            doc.Close();

        }
        public class HeaderFooter2 : iTextSharp.text.pdf.PdfPageEventHelper
        {
            public int ClientID { get; set; }
            public int RID { get; set; }
            List<PdfTemplate> pageNumberTemplates = new List<PdfTemplate>();// page header

            public override void OnStartPage(PdfWriter writer, Document doc)
            {
               // int RID;
               // RID = Convert.ToInt32(databaseCon.Decrypt(HttpContext.Current.Request.QueryString["RID"]));
               
                base.OnStartPage(writer, doc);
                string queryheader = "select tblClient.ClientImage,Plans,right('000000' + convert(varchar,tblRegister.ScaffRef),6) as ScaffRef,left(Description,90) as Description from tblClient, tblRegister where tblClient.ClientId = tblRegister.ClientID and tblRegister.registerid = '" + RID + "'";
                DataTable dtheader = databaseCon.GetData(queryheader);



                string ImagePath = System.Web.HttpContext.Current.Server.MapPath("~/Images/logo2.png");// image size
                Rectangle pageSize = doc.PageSize;
                PdfPTable headertable2 = new PdfPTable(6);
                headertable2.SetWidths(new int[] { 10, 9, 20, 9, 9, 7 });

                var contentfont = FontFactory.GetFont("Arial", 8f, BaseColor.BLACK);
                var Headings = FontFactory.GetFont("Arial", 9f, Font.BOLD, BaseColor.BLACK);
                var Head = FontFactory.GetFont("Arial", 11f, Font.BOLD, BaseColor.BLACK);
                var BackOrderheader = FontFactory.GetFont("Arial", 20f, Font.BOLD, BaseColor.BLACK);
                var Heading787 = FontFactory.GetFont("Arial", 20f, Font.BOLD, BaseColor.BLACK);
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
                phrase.Add(new Chunk("", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.PaddingTop = -10;
                cell.Border = 0;
                //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                headertable2.AddCell(cell);


                phrase = new Phrase();
                phrase.Add(new Chunk("Material Finalised", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 3;
                //cell.PaddingBottom = 6f;
                //cell.PaddingLeft = 100f;
                cell.PaddingTop = -10;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                headertable2.AddCell(cell);

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

                headertable2.AddCell(cell);

                doc.Add(headertable2);
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
    }
}