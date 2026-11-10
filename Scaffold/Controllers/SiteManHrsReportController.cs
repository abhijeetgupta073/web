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
    public class SiteManHrsReportController : Controller
    {
        // GET: SiteManHrsReport
        public ActionResult Index()
        {
            return View();
        }

        public void showreport(string id, string SiteId, string EntryDate)
        {
            string queryheader = "select ClientImage from tblClient where tblClient.ClientId  = '" + databaseCon.Decrypt(id) + "'";
            DataTable dtheader = databaseCon.GetData(queryheader);


            string qury = "select SiteLocation as [Site],firstName+' '+LastName as [Resource], Category,Isnull(AllocatedHrs,0) as AllSitesHours,NoOfWorkedHrs as ThisSiteHours, EntryDate from tblSiteManHrs join tblsiteLocation on tblSiteLocation.SiteLocID=tblSiteManHrs.SiteLocId join tblUser on tblSiteManHrs.UserId=tblUser.UserId  left join tblUserCategory on tblUser.UserId=tblUserCategory.UserCatId left join (select UserId, Sum(Isnull(NoOfWorkedHrs, 0)) as AllocatedHrs from tblSiteManHrs where EntryDate = '" + EntryDate + "' and SiteLocId !='" + SiteId + "' group by UserId)AllocatedQuery on tblSiteManHrs.UserId=AllocatedQuery.UserId where tblSiteManHrs.SiteLocId='" + SiteId + "' and EntryDate='" + EntryDate + "'";

            DataTable dt = databaseCon.GetData(qury);

            //Document doc = new Document(PageSize.A4, -50f, -50f, 20f, 20f);   //Page Size set top ,left,margin
            // doc.SetPageSize(iTextSharp.text.PageSize.A4);

            //PdfWriter writer = PdfWriter.GetInstance(doc, Response.OutputStream);


            iTextSharp.text.Document doc = new iTextSharp.text.Document(PageSize.A4, -50f, -50f, 20f, 20f);   //Page Size set top ,left,margin
            iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, Response.OutputStream);
            //Document doc = new Document(PageSize.A4, -50f, -50f, 20f, 20f);   //Page Size set top ,left,margin
            //doc.SetPageSize(iTextSharp.text.PageSize.A4);
            Response.ContentType = ("application/pdf");
            HeaderFooter2 evnt = new HeaderFooter2();
            evnt.id = id;
            writer.PageEvent = evnt;
            doc.Open();
            var BackOrderheader = FontFactory.GetFont("Arial", 10f, Font.BOLD, BaseColor.BLACK);
            //BackOrderheader.Color = Color.GRAY;
            var Heading787 = FontFactory.GetFont("Arial", 20f, Font.BOLD, BaseColor.BLACK);
            var DataFont = FontFactory.GetFont("Arial", 10f, Font.NORMAL, BaseColor.BLACK);
            Phrase phrase;
            PdfPCell cell;

            PdfPTable headertable1 = new PdfPTable(5);
            headertable1.SetWidths(new int[] { 10, 9, 25, 9, 9 });

            //Table Start

            PdfPTable headerTbl = new PdfPTable(6);
            headerTbl.SetWidths(new int[] { 15, 15, 15, 9, 9, 9 });

            //phrase = new Phrase();
            //phrase.Add(new Chunk("", BackOrderheader));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 6;
            //cell.PaddingBottom = 6f;
            //cell.Border = 0;
            //cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //headerTbl.AddCell(cell);




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
            //        cell.PaddingLeft = -50f;
            //        //cell.PaddingTop = 20;
            //    }
            //    else
            //    {
            //        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(Server.MapPath("images/clientlogo/" + dtheader.Rows[0]["ClientImage"].ToString()));

            //        img.ScaleAbsolute(180f, 133f);
            //        cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img), true);
            //        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //        cell.FixedHeight = 100f;
            //        cell.Colspan = 2;
            //        cell.PaddingLeft = -50f;
            //        //cell.PaddingTop = 20f;

            //    }
            //}
            //else
            //{
            //    string nophoto = Server.MapPath("images/Logo2.png");
            //    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
            //    cell.FixedHeight = 100f;
            //    cell.Colspan = 2;
            //    //cell.PaddingTop = 20f;
            //    cell.PaddingLeft = -50f;

            //}
            //cell.Border = 0;
            //cell.Colspan = 2;
            //cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //cell.PaddingLeft = -50f;
            ////cell.PaddingTop = 30f;
            //headerTbl.AddCell(cell);


            //phrase = new Phrase();
            //phrase.Add(new Chunk("Daily Site Man Hours", Heading787));
            ////phrase.Add(new Chunk(Convert.ToString(dtheader.Rows[0]["SiteName"].ToString()), Heading787));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 4;
            //cell.PaddingBottom = 6f;
            //cell.PaddingLeft = 100f;
            //cell.PaddingTop = 30f;
            //cell.Border = 0;
            //cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //headerTbl.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk("", BackOrderheader));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 6;
            //cell.PaddingBottom = 6f;
            //cell.Border = 0;
            //cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //headerTbl.AddCell(cell);
            //phrase = new Phrase();
            //phrase.Add(new Chunk("", BackOrderheader));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 6;
            //cell.PaddingBottom = 6f;
            //cell.Border = 0;
            //cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //headerTbl.AddCell(cell);
            //phrase = new Phrase();
            //phrase.Add(new Chunk("", BackOrderheader));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 6;
            //cell.PaddingBottom = 6f;
            //cell.Border = 0;
            //cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //headerTbl.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk("Site", BackOrderheader));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.Border = 1;
            //cell.PaddingBottom = 6f;
            //cell.BackgroundColor = Color.LIGHT_GRAY;
            //cell.Border = PdfCell.TOP_BORDER | PdfCell.LEFT_BORDER | PdfCell.RIGHT_BORDER | PdfCell.ALIGN_BOTTOM;

            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //headerTbl.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk("Resource", BackOrderheader));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.PaddingBottom = 6f;
            //cell.Border = 1;
            //cell.Border = PdfCell.TOP_BORDER | PdfCell.LEFT_BORDER | PdfCell.RIGHT_BORDER | PdfCell.ALIGN_BOTTOM;
            //cell.BackgroundColor = Color.LIGHT_GRAY;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //headerTbl.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk("Category", BackOrderheader));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.Border = 1;
            //cell.Border = PdfCell.TOP_BORDER | PdfCell.LEFT_BORDER | PdfCell.RIGHT_BORDER | PdfCell.ALIGN_BOTTOM;
            //cell.BackgroundColor = Color.LIGHT_GRAY;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //headerTbl.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk("All Site Hours", BackOrderheader));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.Border = 1;
            //cell.Border = PdfCell.TOP_BORDER | PdfCell.LEFT_BORDER | PdfCell.RIGHT_BORDER | PdfCell.ALIGN_BOTTOM;
            //cell.BackgroundColor = Color.LIGHT_GRAY;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //headerTbl.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk("This Site Hours", BackOrderheader));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.Border = 1;
            //cell.Border = PdfCell.TOP_BORDER | PdfCell.LEFT_BORDER | PdfCell.RIGHT_BORDER | PdfCell.ALIGN_BOTTOM;
            //cell.BackgroundColor = Color.LIGHT_GRAY;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //headerTbl.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk("Entry Date", BackOrderheader));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.Border = 1;
            //cell.Border = PdfCell.TOP_BORDER | PdfCell.LEFT_BORDER | PdfCell.RIGHT_BORDER | PdfCell.ALIGN_BOTTOM;
            //cell.BackgroundColor = Color.LIGHT_GRAY;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //headerTbl.AddCell(cell);

            //Table End

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Site"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headerTbl.AddCell(cell);



                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Resource"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headerTbl.AddCell(cell);

                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Category"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headerTbl.AddCell(cell);

                    phrase = new Phrase();
                    //string Avaiqty = String.Format("{0:#,##0}", dt.Rows[i]["AvailableQuantity"]);
                    //phrase.Add(new Chunk(Avaiqty, DataFont));
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["AllSitesHours"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headerTbl.AddCell(cell);

                    phrase = new Phrase();
                    string Thissitehrs = String.Format("{0:#.#0}", dt.Rows[i]["ThisSiteHours"]);
                    phrase.Add(new Chunk(Thissitehrs, DataFont));
                    //phrase.Add(new Chunk(Convert.ToInt32(dt.Rows[i]["ThisSiteHours"]).ToString(), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headerTbl.AddCell(cell);

                    phrase = new Phrase();
                    //string Allocatedqty = String.Format("{0:#,##0}", dt.Rows[i]["AllocatedQuantity"]);
                    //phrase.Add(new Chunk(Allocatedqty, DataFont));
                    //phrase.Add(new Chunk("" + DateTime.Now.ToString("dd-MMM-yyy") + "", DataFont));
                    phrase.Add(new Chunk(Convert.ToDateTime(dt.Rows[i]["EntryDate"]).ToString("dd-MMM-yyy"), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headerTbl.AddCell(cell);
                    //Totalquantity += Convert.ToInt32(dt.Rows[i]["AllocatedQuantity"]);
                    //cell.BackgroundColor = Color.LIGHT_GRAY;
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
                headerTbl.AddCell(cell);

            }



            //phrase = new Phrase();
            //phrase.Add(new Chunk("Printed: ", BackOrderheader));
            //phrase.Add(new Chunk("" + DateTime.Now.ToString("dd-MMM-yyy") + "", DataFont));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.PaddingBottom = 6f;
            //cell.Border = 0;
            //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //headerTbl.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk("", DataFont));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 5;
            //cell.PaddingBottom = 6f;
            //cell.Border = 0;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //headerTbl.AddCell(cell);

            doc.Add(headerTbl);
            doc.Close();

        }

        public class HeaderFooter2 : iTextSharp.text.pdf.PdfPageEventHelper
        {
            public string id { get; set; }
            List<PdfTemplate> pageNumberTemplates = new List<PdfTemplate>();// page header

            public override void OnStartPage(PdfWriter writer, Document doc)
            {
                int ClientID;
                ClientID = Convert.ToInt32(databaseCon.Decrypt(id));
                //SiteId = Convert.ToInt32(databaseCon.Decrypt(HttpContext.Current.Request.QueryString["SiteId"]));
                //RID = Convert.ToInt32(databaseCon.Decrypt(HttpContext.Current.Request.QueryString["RID"]));
                base.OnStartPage(writer, doc);
                string queryheader = "select ClientImage from tblClient where tblClient.ClientId  = '" + ClientID + "'";
                //string queryheader = "select ClientImage from tblClient where tblClient.ClientId  = '153'";
                DataTable dtheader = databaseCon.GetData(queryheader);



                string ImagePath = System.Web.HttpContext.Current.Server.MapPath("~/Images/logo2.png");// image size
                Rectangle pageSize = doc.PageSize;
                PdfPTable headerTbl = new PdfPTable(6);
                headerTbl.SetWidths(new int[] { 15, 15, 15, 9, 9, 9 });

                var contentfont = FontFactory.GetFont("Arial", 8f, BaseColor.BLACK);
                var Headings = FontFactory.GetFont("Arial", 9f, Font.BOLD, BaseColor.BLACK);
                var Head = FontFactory.GetFont("Arial", 11f, Font.BOLD, BaseColor.BLACK);
                var BackOrderheader = FontFactory.GetFont("Arial", 20f, Font.BOLD, BaseColor.BLACK);
                var Heading787 = FontFactory.GetFont("Arial", 10f, Font.BOLD, BaseColor.BLACK);
                //var Heading787 = FontFactory.GetFont("Arial", 20f, Font.BOLD, Color.BLACK);
                Phrase phrase;
                PdfPCell cell;


                if (!string.IsNullOrEmpty(Convert.ToString(dtheader.Rows[0]["ClientImage"])))
                {
                    var uri = new Uri(databaseCon.clientimageLink);
                    // var container = new CloudBlobContainer(uri);
                    //var blob = container.GetBlockBlobReference(dtheader.Rows[0]["ClientImage"].ToString());
                    var noimage = System.Web.HttpContext.Current.Server.MapPath("/Images/Logo2.png");
                    //var exists = blob.Exists();
                    if (Convert.ToString(dtheader.Rows[0]["ClientImage"]) == "")
                    {
                        string nophoto = System.Web.HttpContext.Current.Server.MapPath("/images/Logo2.png");
                        cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
                        cell.FixedHeight = 50f;
                        cell.Colspan = 1;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        //cell.PaddingLeft = -45f;
                        cell.PaddingTop = -10;
                    }
                    else
                    {
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(System.Web.HttpContext.Current.Server.MapPath("/images/clientlogo/" + dtheader.Rows[0]["ClientImage"].ToString()));

                        img.ScaleAbsolute(180f, 133f);
                        cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img), true);
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.FixedHeight = 50f;
                        cell.PaddingTop = -10f;
                        cell.Colspan = 1;



                    }
                }
                else
                {
                    string nophoto = System.Web.HttpContext.Current.Server.MapPath("/images/Logo2.png");
                    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
                    cell.FixedHeight = 50f;
                    //cell.PaddingLeft = -45f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 1;
                    cell.PaddingTop = -10f;
                }
                cell.Border = 0;
                cell.Colspan = 1;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.FixedHeight = 50f;
                cell.PaddingTop = -10f;
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
                phrase.Add(new Chunk("Daily Site Man Hours", BackOrderheader));//top haddings
                cell = new PdfPCell(phrase);
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.FixedHeight = 50f;
                cell.Colspan = 3;
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
                phrase.Add(new Chunk("", BackOrderheader));
                cell = new PdfPCell(phrase);
                cell.Colspan = 6;
                cell.PaddingBottom = 6f;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headerTbl.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Site", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingBottom = 6f;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;

                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headerTbl.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Resource", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.PaddingBottom = 6f;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headerTbl.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Category", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headerTbl.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("All Site Hours", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headerTbl.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("This Site Hours", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headerTbl.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Entry Date", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;

                headerTbl.AddCell(cell);

                doc.Add(headerTbl);
            }
            public override void OnEndPage(iTextSharp.text.pdf.PdfWriter writer, Document document)
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