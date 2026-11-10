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
    public class ApprovedforDismantleReportController : Controller
    {
        // GET: ApprovedforDismantleReport
        public ActionResult Index()
        {
            return View();
        }

        // UID=5l%20kQV7YGkdMC9G2r0xGzQ==&id=iEKccYo70rw3HmhXUsJ7OQ==&Owner=qBGby234tfhxhnQnatRL5Q==&btn=prv


        public void ApprovedforDismantleReportPdf(string UID, string id, string site, string Owner, string availablereportname, string btn)
        {
            string Query = "";
            string ReportName = "";
            var siteid = site;
            var siteids = databaseCon.Decrypt(siteid);
            var Id = databaseCon.Decrypt(id);
            //Added on 24-04-2024 for Decrypt Owner for Speacial Character like C&I
            string Ownername = databaseCon.Decrypt(Owner).Replace("'", "''").Trim();
            string decryptedReportName = databaseCon.Decrypt(Request.QueryString["availablereportname"]);
            iTextSharp.text.Document docs = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, -110f, -110f, 50f, 50f);
            //iTextSharp.text.Document docs = new iTextSharp.text.Document(iTextSharp.text.PageSize.LETTER, -110f, -110f, 50f, 50f);
            docs.SetPageSize(new Rectangle(1000, 900));

            iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(docs, Response.OutputStream);
            Response.ContentType = ("application/pdf");
            HeaderFooter2 evnts = new HeaderFooter2();
            writer.PageEvent = evnts;
            evnts.Id = id;
            evnts.Report = 1;
            evnts.site = siteid;
            evnts.availablereportname = availablereportname;

            docs.Open();
            var Heading781 = FontFactory.GetFont("Arial", 10f, Font.BOLD, BaseColor.BLACK);
            var Heading782 = FontFactory.GetFont("Arial", 20f, Font.BOLD, BaseColor.BLACK);
            var DataFont = FontFactory.GetFont("Arial", 9f, Font.NORMAL, BaseColor.BLACK);
            Phrase phrase;
            PdfPCell cell;
            PdfPTable headertable1 = new PdfPTable(12);
            headertable1.SetWidths(new int[] { 8, 9, 9, 12, 9, 9, 9, 8, 9, 9, 7, 5 });
            headertable1.HeaderRows = 4;
            ReportName = "Approved for Dismantle Report";
            //evnts.Report = 1;
            //Query = "select * from VW_TblRegister where clientid ='" + databaseCon.Decrypt(id) + "' and SiteLocId='" + databaseCon.Decrypt(siteid) + "' and ('All'='" + Ownername + "' or ScaffoldOwner ='" + Ownername + "') and ChkRequiresIsolation=1 and (DateOfDismantle='1900-01-01' or DateOfDismantle='' or DateOfDismantle is null) order by ScaffRef,DateTagged";
            Query = "EXEC Pro_ExportToCsv @decryptedReportName = '" + decryptedReportName + "', @clientID = '" + Id + "', @siteid = '" + siteids + "', @Ownername = '" + Ownername + "', @Index = '" + "First" + "', @Days = '" + "0" + "'";

            evnts.Report = 2;
            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading781));
            cell = new PdfPCell(phrase);
            cell.Colspan = 20;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(ReportName, Heading782));
            cell = new PdfPCell(phrase);
            cell.Colspan = 20;
            cell.PaddingBottom = 2f;
            cell.Border = 0;
            cell.PaddingTop = 5f;
            cell.PaddingBottom = 2f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading781));
            cell = new PdfPCell(phrase);
            //  cell.Colspan = 16;
            //cell.Colspan = 17;
            // Added New Column Status on (11-08-2023)
            cell.Colspan = 20;
            //cell.PaddingBottom = 6f;
            cell.Border = 0;
            //cell.PaddingTop = -50f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Scaf Ref #", Heading781));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.Border = 1;
            cell.PaddingTop = 10f;
            // cell.Width = 100f;
            //cell.Left = 400f;
            //cell.PaddingRight = -100f;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Site", Heading781));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.Border = 1;
            cell.PaddingTop = 10f;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Work OrderNo.", Heading781));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.Border = 1;
            cell.PaddingTop = 10f;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Reason For Access", Heading781));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.Border = 1;
            cell.PaddingTop = 10f;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Date Erected", Heading781));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.Border = 1;
            cell.PaddingTop = 10f;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Dis. Appr.Date", Heading781));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.Border = 1;
            cell.PaddingTop = 10f;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Dismantle Hours", Heading781));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.Border = 1;
            cell.PaddingTop = 10f;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Location", Heading781));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.Border = 1;
            cell.PaddingTop = 10f;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            //Commented on 14-09-2023 Renamed as Date Built
            //phrase.Add(new Chunk("Date Tagged", Heading786));
            phrase.Add(new Chunk("Building / Module No.", Heading781));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.Border = 1;
            cell.PaddingTop = 10f;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);



            phrase = new Phrase();
            phrase.Add(new Chunk("Asset # / Func.Loc", Heading781));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.Border = 1;
            cell.PaddingTop = 10f;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);



            phrase = new Phrase();
            phrase.Add(new Chunk("SAP Number", Heading781));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.Border = 1;
            cell.PaddingTop = 10f;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Status", Heading781));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.Border = 1;
            cell.PaddingTop = 10f;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);



            DataTable dt = databaseCon.GetData(Query);
            if (dt.Rows.Count > 0)
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["ScaffRef"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.PaddingTop = 2f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);

                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Site"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);


                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Building/Module No."].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);


                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Reason For Access"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);


                    phrase = new Phrase();
                    string dateonly = String.Format("{0:dd-MMM-yyyy}", dt.Rows[i]["DateTagged"]);
                    phrase.Add(new Chunk(dateonly, DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);


                    //phrase = new Phrase();
                    //phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Dis.Appr.Date"].ToString()), DataFont));
                    //cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 15f;
                    //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //cell.Border = 1;
                    //cell.Colspan = 1;
                    //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    //headertable1.AddCell(cell);

                    phrase = new Phrase();
                    string dateonlyDisAppr = String.Format("{0:dd-MMM-yyyy}", dt.Rows[i]["Dis.Appr.Date"]);
                    phrase.Add(new Chunk(dateonlyDisAppr, DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);


                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Dismantle Hours"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);




                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Location"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);


                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Building/Module No."].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);


                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Asset/Func.Loc"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);


                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Outage Code"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);



                    // Added New Column Status on (11-08-2023)
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Status"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);
                }
                docs.Add(headertable1);
                headertable1.DeleteBodyRows();
                for (int m = headertable1.Rows.Count - 1; m >= 0; m--)
                {
                    headertable1.Rows.RemoveAt(m);
                }
                docs.NewPage();
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("No Data Found", DataFont));
                cell = new PdfPCell(phrase);
                cell.PaddingBottom = 15f;
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = 1;
                cell.Colspan = 19;
                cell.PaddingTop = 2f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                headertable1.AddCell(cell);
                docs.Add(headertable1);
                for (int m = headertable1.Rows.Count - 1; m >= 0; m--)
                {
                    headertable1.Rows.RemoveAt(m);
                }
                docs.NewPage();
            }
            docs.Close();


        }

        public class HeaderFooter2 : PdfPageEventHelper
        {
            public string Id { get; set; }
            public string site { get; set; }
            public string availablereportname { get; set; }
            public int Report { get; set; }
            List<PdfTemplate> pageNumberTemplates = new List<PdfTemplate>();// page header


            public override void OnStartPage(iTextSharp.text.pdf.PdfWriter writer, Document docs)
            {
                string ImagePath = System.Web.HttpContext.Current.Server.MapPath("~/Images/logo2.png");// image size
                Rectangle pageSize = docs.PageSize;
                // PdfPTable headertables = new PdfPTable(16);
                //PdfPTable headertables = new PdfPTable(17);
                // Added New Column Status on (11-08-2023)
                //PdfPTable headertables = new PdfPTable(20);
                PdfPTable headertables = new PdfPTable(12);

                //headertables.SetWidths(new int[] { 6, 7, 7, 7, 7, 12, 6, 7, 4, 5, 8, 9, 5, 5, 4, 4 });
                //added this code on 16/08/2023
                //headertables.SetWidths(new int[] { 4, 5, 6, 6, 5, 6, 8, 6, 5, 5, 6, 6, 6, 10, 10, 6, 5, 4, 5, 6 });
                headertables.SetWidths(new int[] { 4, 6, 6, 8, 6, 6, 10, 6, 5, 5, 6, 7 });


                var contentfont = FontFactory.GetFont("Arial", 8f, BaseColor.BLACK);
                var Headings = FontFactory.GetFont("Arial", 9f, Font.BOLD, BaseColor.BLACK);
                var Head = FontFactory.GetFont("Arial", 18f, Font.BOLD, BaseColor.BLACK);
                var BackOrderheader = FontFactory.GetFont("Arial", 20f, Font.BOLD, BaseColor.BLACK);
                var Heading787 = FontFactory.GetFont("Arial", 20f, Font.BOLD, BaseColor.BLACK);
                var Heading786 = FontFactory.GetFont("Arial", 10f, Font.BOLD, BaseColor.BLACK);
                var DataFont = FontFactory.GetFont("Arial", 13f, Font.NORMAL, BaseColor.BLACK);
                Phrase phrase;
                PdfPCell cell;

                //int id = Convert.ToInt32(HttpUtility.UrlEncode(databaseCon.Decrypt(HttpContext.Current.Request.QueryString["id"])));
                //string namesite = Convert.ToString(HttpUtility.UrlEncode(databaseCon.Decrypt(HttpContext.Current.Request.QueryString["site"])));
                //string AvailableReporid = Convert.ToString(HttpUtility.UrlEncode(databaseCon.Decrypt(HttpContext.Current.Request.QueryString["availablereportname"])));

                int id = Convert.ToInt32(databaseCon.Decrypt(Id));
                int ReportCount = Report;
                string namesite = Convert.ToString(databaseCon.Decrypt(site));
                string AvailableReporid = Convert.ToString(databaseCon.Decrypt(availablereportname));

                string date = (("Printed: " + DateTime.Now.ToString("dd-MMM-yyy")));

                string queryimage = "select ClientImage from tblclient where clientid = '" + id + "'";
                DataTable dtheaders = databaseCon.GetData(queryimage);

                string sitename = "select SiteLocID,SiteLocation from tblSiteLocation where SiteLocID = '" + namesite + "'";
                DataTable sitidname = databaseCon.GetData(sitename);

                if (ReportCount == 1)
                {
                    //footerDate.HorizontalAlignment = Element.ALIGN_CENTER;
                    //cell = new PdfPCell(footerdate);
                    //cell.Border = 0;

                    phrase = new Phrase();
                    phrase.Add(new Chunk(date, DataFont));
                    cell = new PdfPCell(phrase);
                    // cell.Colspan = 16;
                    // Added New Column Status on (11-08-2023)
                    //cell.Colspan = 17;
                    cell.Colspan = 20;
                    cell.Border = 0;
                    cell.PaddingTop = -30f;
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    headertables.AddCell(cell);


                    if (!string.IsNullOrEmpty(Convert.ToString(dtheaders.Rows[0]["ClientImage"])))
                    {
                        var uri = new Uri(databaseCon.clientimageLink);
                        // var container = new CloudBlobContainer(uri);
                        //var blob = container.GetBlockBlobReference(dtheader.Rows[0]["ClientImage"].ToString());
                        var noimage = System.Web.HttpContext.Current.Server.MapPath("~/Images/Logo2.png");
                        //var exists = blob.Exists();
                        if (Convert.ToString(dtheaders.Rows[0]["ClientImage"]) == "")
                        {
                            string nophoto = System.Web.HttpContext.Current.Server.MapPath("~/images/Logo2.png");
                            cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
                            cell.FixedHeight = 100f;
                            // cell.Width = 100f;
                            cell.Colspan = 6;
                            //cell.PaddingLeft = 10f;
                            cell.PaddingTop = -50f;
                        }
                        else
                        {
                            iTextSharp.text.Image imglogo = iTextSharp.text.Image.GetInstance(System.Web.HttpContext.Current.Server.MapPath("~/images/clientlogo/" + dtheaders.Rows[0]["ClientImage"].ToString()));

                            imglogo.ScaleAbsolute(180f, 133f);
                            cell = new PdfPCell(iTextSharp.text.Image.GetInstance(imglogo), true);
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.FixedHeight = 100f;
                            // cell.Width = 100f;
                            cell.Colspan = 6;
                            //cell.PaddingLeft = 10f;
                            cell.PaddingTop = -50f;

                        }
                    }


                    else
                    {
                        string nophoto = System.Web.HttpContext.Current.Server.MapPath("~/images/Logo2.png");
                        cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
                        cell.FixedHeight = 100f;
                        //  cell.Width = 100f;
                        //cell.PaddingLeft = 10f;
                        cell.Colspan = 6;
                        cell.PaddingTop = -50f;
                    }
                    cell.Border = 0;
                    cell.FixedHeight = 45f;
                    ////  cell.Colspan = 6;
                    cell.Colspan = 3;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    ////cell.PaddingLeft = 20f;
                    cell.PaddingTop = -30f;
                    headertables.AddCell(cell);

                    phrase = new Phrase();
                    phrase.Add(new Chunk("", DataFont));
                    cell = new PdfPCell(phrase);
                    cell.FixedHeight = 30f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    cell.Colspan = 3;
                    headertables.AddCell(cell);

                    //phrase = new Phrase();
                    //phrase.Add(new Chunk("Site :", Heading787));
                    //cell = new PdfPCell(phrase);
                    //cell.Border = 0;
                    //cell.FixedHeight = 30f;
                    //cell.PaddingRight = 10f;
                    //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //cell.Colspan = 3;
                    //headertables.AddCell(cell);


                    if (sitidname.Rows.Count > 0)
                    {
                        phrase = new Phrase();
                        phrase.Add(new Chunk(Convert.ToString("Site : " + sitidname.Rows[0]["SiteLocation"].ToString()), Heading787));
                        cell = new PdfPCell(phrase);
                        //cell.PaddingBottom = 15f;
                        cell.PaddingRight = 5f;
                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cell.Border = 0;
                        cell.FixedHeight = 30f;
                        cell.Colspan = 10;
                        //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertables.AddCell(cell);
                    }

                    else
                    {

                        phrase = new Phrase();
                        phrase.Add(new Chunk(" ", Heading787));
                        cell = new PdfPCell(phrase);
                        //cell.PaddingBottom = 15f;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = 1;
                        cell.FixedHeight = 30f;
                        cell.Colspan = 5;
                        //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertables.AddCell(cell);
                        headertables.HeaderRows = 1;
                    }


                    docs.NewPage();
                    docs.Add(headertables);
                }

                headertables.DeleteBodyRows();
                //headertables.AddCell(cell);
            }
            public override void OnEndPage(iTextSharp.text.pdf.PdfWriter writer, Document document)
            {
                //Paragraph footerdate = new Paragraph(("Printed: " + DateTime.Now.ToString("dd-MMM-yyy")), FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
                //PdfPCell cell;

                //PdfPTable footerDate = new PdfPTable(1);
                //footerDate.TotalWidth = 400;
                //footerDate.HorizontalAlignment = Element.ALIGN_CENTER;
                //cell = new PdfPCell(footerdate);
                //cell.Border = 0;
                //cell.PaddingTop = 50;
                //footerDate.AddCell(cell);
                //footerDate.WriteSelectedRows(0, -1, 17, 70, writer.DirectContent);
            }
        }


    }
}