using iTextSharp.text;
using iTextSharp.text.pdf;
using Scaffold.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scaffold.Controllers
{
    public class ReturnDocketController : Controller
    {
        float Totalquantity = 0;
        float WeightTotal = 0;
        // GET: ReturnDocket
        public ActionResult Index()
        {
            return View();
        }

      
            public void ReturnDocket(string UID, string UserRole, string id, string RInvId, string FromName, string ToName)
        {
            int ReturnInvID = Convert.ToInt32(databaseCon.Decrypt(RInvId));
            int ClientID = Convert.ToInt32(databaseCon.Decrypt(id));
            FromName = Convert.ToString(databaseCon.Decrypt(FromName));
            ToName = Convert.ToString(databaseCon.Decrypt(ToName));

            //string ReturnInvId = "'select ReturnInvId from tblReturnInventoryDetails'";
            //DataTable dt1 = databaseCon.GetData(ReturnInvId);

            string query = "";
            query = "select MaterialType,ItemCode,ItemDesc,Quantity as ReturnQuantity, ItemWeight  , Isnull(tblReturnInventoryDetails.Quantity*ItemWeight,0) as TotalWeight from tblReturnInventoryDetails join tblInventory on tblReturnInventoryDetails.InventoryId = tblInventory.InventoryID where ReturnInvId = '" + ReturnInvID + "'  order by MaterialType,ISNULL(DisplaySortOrder,9999999),ItemCode,ItemDesc";
            DataTable dt = databaseCon.GetData(query);


            iTextSharp.text.Document doc = new iTextSharp.text.Document(PageSize.A4, -50f, -50f, 20f, 20f);   //Page Size set top ,left,margin
            iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, Response.OutputStream);
            //Document doc = new Document(PageSize.A4, -50f, -50f, 20f, 20f);   //Page Size set top ,left,margin
            //doc.SetPageSize(iTextSharp.text.PageSize.A4);

            //PdfWriter writer = PdfWriter.GetInstance(doc, Response.OutputStream);
            Response.ContentType = ("application/pdf");
            HeaderFooter2 evnt = new HeaderFooter2();
            writer.PageEvent = evnt;
            evnt.ReturnInvID = ReturnInvID;
            evnt.ID = ClientID;
            evnt.FromName = FromName;
            evnt.ToName = ToName;

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

            PdfPTable headertable3 = new PdfPTable(6);
            headertable3.SetWidths(new int[] { 15, 15, 20, 8, 9, 9 });

            PdfPTable headertable2 = new PdfPTable(4);
            headertable2.SetWidths(new int[] { 20, 20, 20, 20 });
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
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable3.AddCell(cell);


                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["ItemCode"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable3.AddCell(cell);

                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["ItemDesc"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable3.AddCell(cell);

                    phrase = new Phrase();
                    string OrgSiteqty = String.Format("{0:#,##0}", dt.Rows[i]["ReturnQuantity"]);
                    phrase.Add(new Chunk(OrgSiteqty, DataFont));
                    //phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["OrgSiteQuantity"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable3.AddCell(cell);
                    Totalquantity += Convert.ToInt32(dt.Rows[i]["ReturnQuantity"]);

                    phrase = new Phrase();
                    string itemweight = String.Format("{0:#,##0.00}", dt.Rows[i]["ItemWeight"]);
                    phrase.Add(new Chunk(itemweight, DataFont));
                    // phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["ItemWeight"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable3.AddCell(cell);

                    phrase = new Phrase();
                    string Totalweight = String.Format("{0:#,##0.00}", dt.Rows[i]["TotalWeight"]);
                    phrase.Add(new Chunk(Totalweight, DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable3.AddCell(cell);

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
            cell.Colspan = 2;
            cell.PaddingBottom = 6F;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Total:", DataFont));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.Border = 3;
            cell.PaddingBottom = 6F;
            cell.Border = PdfPCell.RIGHT_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable3.AddCell(cell);

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
            headertable3.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", DataFont));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6F;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable3.AddCell(cell);

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
            headertable3.AddCell(cell);



            doc.Add(headertable1);
            doc.Add(headertable2);
            doc.Add(headertable3);
            doc.Close();

        }


        public class HeaderFooter2 : iTextSharp.text.pdf.PdfPageEventHelper
        {
            // string UID, string UserRole, string id, string RInvId, string FromName, string ToName

            public int ID { get; set; }
            public int ReturnInvID { get; set; }
            public string FromName { get; set; }
            public string ToName { get; set; }

            List<PdfTemplate> pageNumberTemplates = new List<PdfTemplate>();// page header

            public override void OnStartPage(PdfWriter writer, Document doc)
            {
                //int ID = Convert.ToInt32(databaseCon.Decrypt(HttpContext.Current.Request.QueryString["Id"]));
                //int SiteID = Convert.ToInt32(databaseCon.Decrypt(HttpContext.Current.Request.QueryString["Site"]));
                //int ReturnInvID = Convert.ToInt32(databaseCon.Decrypt(HttpContext.Current.Request.QueryString["RInvId"]));
                //string FromName = (databaseCon.Decrypt(HttpContext.Current.Request.QueryString["FromName"]));
                //string ToName = (databaseCon.Decrypt(HttpContext.Current.Request.QueryString["ToName"]));

               
                string queryheader = "select ClientImage from tblClient where ClientId  = '" + ID + "'";
                DataTable dtheader = databaseCon.GetData(queryheader);


                //string query1 = "select ReturnInvId,[From],[To], Convert(date,ReturnDoneon) as ReturnDate from tblReturnInventory  where ReturnInvId = '" + ReturnInvID + "'"; //15-apr-2019-baans-end

                string query1 = "select ReturnInvId,[From],[To], Convert(date,ReturnDoneon) as ReturnDate,ReturnStatus  from tblReturnInventory  where ReturnInvId = '" + ReturnInvID + "'";//15-apr-2019-baans-changes
                DataTable dt1 = databaseCon.GetData(query1);

                var Heading786 = FontFactory.GetFont("Arial", 10f, Font.BOLD, BaseColor.BLACK);
                //Heading786.BaseColor = BaseColor.GRAY;
                var Heading787 = FontFactory.GetFont("Arial", 20f, Font.BOLD, BaseColor.BLACK);
                var DataFont = FontFactory.GetFont("Arial", 10f, Font.NORMAL, BaseColor.BLACK);
                Phrase phrase;
                PdfPCell cell;

                PdfPTable headertable1 = new PdfPTable(5);
                headertable1.SetWidths(new int[] { 10, 9, 25, 9, 9 });

                //Table Start

                PdfPTable headertable3 = new PdfPTable(6);
                headertable3.SetWidths(new int[] { 15, 15, 20, 8, 9, 9 });

                PdfPTable headertable2 = new PdfPTable(4);
                headertable2.SetWidths(new int[] { 20, 20, 20, 20 });



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
                        //cell.PaddingLeft = 10f;
                        cell.PaddingTop = -10f;
                }
                else
                {
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(System.Web.HttpContext.Current.Server.MapPath("~/images/clientlogo/" + dtheader.Rows[0]["ClientImage"].ToString()));

                    img.ScaleAbsolute(180f, 133f);
                    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img), true);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.FixedHeight = 50f;
                    cell.Colspan = 2;
                    //cell.PaddingLeft = 10f;
                    cell.PaddingTop = -10f;

                }
            }
                else
                {
                    string nophoto = System.Web.HttpContext.Current.Server.MapPath("~/images/Logo2.png");
                    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
                    cell.FixedHeight = 50f;
                    //cell.PaddingLeft = 10f;
                    cell.Colspan = 2;
                    cell.PaddingTop = -10f;
                }
                cell.Border = 0;
                cell.Colspan = 1; //15-apr-baans-change
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell.PaddingLeft = 20f;
                cell.PaddingTop = -10f;
                headertable2.AddCell(cell);


                phrase = new Phrase();
                phrase.Add(new Chunk("Return Docket", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 2;
                cell.PaddingTop = -10f;
                cell.Border = 0;
                cell.PaddingLeft = 80; //15-apr-baans-change
                                       //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                headertable2.AddCell(cell);

                //15-apr-2019-baans-start
                if (Convert.ToString(dt1.Rows[0]["ReturnStatus"]) != "")
                {
                    string Returnimg = System.Web.HttpContext.Current.Server.MapPath("~/images/lock.png");
                    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(Returnimg), true);
                    cell.FixedHeight = 10f;
                    cell.Colspan = 1;
                    cell.PaddingTop = -8f;
                    cell.Border = 0;
                    cell.PaddingLeft = 75;
                }
                else
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("", Heading787));
                    cell = new PdfPCell(phrase);
                    cell.Colspan = 1;
                    cell.PaddingTop = -8f;
                    cell.Border = 0;
                    cell.PaddingLeft = 75;
                }
                headertable2.AddCell(cell);//15-apr-2019-baans-end

                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 2;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Trans ID:", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk(Convert.ToString(dt1.Rows[0]["ReturnInvId"].ToString()), DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.PaddingBottom = 6f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 2;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Date:", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                //cell.PaddingBottom = 6f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable2.AddCell(cell);

                phrase = new Phrase();

                string dateTime = String.Format("{0:d-MMM-yyyy}", dt1.Rows[0]["ReturnDate"]);
                phrase.Add(new Phrase(dateTime, DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.PaddingBottom = 6f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 2;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("From:", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.PaddingBottom = 6f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk(FromName, DataFont));
                //  phrase.Add(new Chunk(Convert.ToString(dt1.Rows[0]["From"].ToString()), DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.PaddingBottom = 6f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable2.AddCell(cell);


                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 2;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("To:", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.PaddingBottom = 6f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                // phrase.Add(new Chunk(Convert.ToString(dt1.Rows[0]["To"].ToString()), DataFont));
                phrase.Add(new Chunk(ToName, DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.PaddingBottom = 6f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk(" ", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 12;
                cell.PaddingBottom = 6f;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 2;
                cell.PaddingBottom = 6f;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                headertable2.AddCell(cell);

                //phrase = new Phrase();
                //phrase.Add(new Chunk(" ", Heading786));
                //cell = new PdfPCell(phrase);
                //cell.Colspan = 12;
                ////cell.PaddingBottom = 6f;
                //cell.Border = 0;
                //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Material Type", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingBottom = 6f;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;

                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Item Code", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.PaddingBottom = 6f;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Item Description", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Return Quantity", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);


                phrase = new Phrase();
                phrase.Add(new Chunk("Item Weight (kg)", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Total Weight (kg)", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable3.AddCell(cell);

                doc.Add(headertable1);
                doc.Add(headertable2);
                doc.Add(headertable3);

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

  
}