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
    public class StandingReportController : Controller
    {
        // GET: StandingReport
        public ActionResult Index()
        {
            return View();
        }

        public void StandingReportPdf(string UID, string id, string site, string Owner, string availablereportname, string btn)
        {
            //string Query = "";
            var siteid = site;
            var siteId = databaseCon.Decrypt(site);
            var Clientid = databaseCon.Decrypt(id);
            string decryptedReportName = databaseCon.Decrypt(Request.QueryString["availablereportname"]);
            //Added on 24-04-2024 for Decrypt Owner for Speacial Character like C&I
            string Ownername = databaseCon.Decrypt(Owner).Replace("'", "''").Trim();
            //if (databaseCon.Decrypt(Request.QueryString["availablereportname"]) == "Over 90 days Standing Report")
            //if (databaseCon.Decrypt(Request.QueryString["availablereportname"]) == "Over 90 days less than 180 days Standing Report")
            //{
            //    //Query = "select * from VW_TblRegister where ClientID = '" + id + "' and SiteLocId ='" + siteid + "'  and DateTagged>DATEADD(d,-90,Convert(date,getdate()))and DateOFDismantle is null";
            //    Query = "select convert(decimal(18, 2),((tonnage)/1000)) as tonnage, * from VW_TblRegister where clientid ='" + databaseCon.Decrypt(id) + "' and SiteLocId ='" + databaseCon.Decrypt(siteid) + "' and ('All'='" + Ownername + "' or ScaffoldOwner ='" + Ownername + "') and HireDaysToDate>90 and HireDaysToDate<=180 order by DateTagged,ScaffRef";
            //}

            //if (databaseCon.Decrypt(Request.QueryString["availablereportname"]) == "Over 180 days Standing Report")
            //{
            //    //Query = "select * from VW_TblRegister where ClientID = '" + id + "' and SiteLocId ='" + siteid + "'  and DateTagged>DATEADD(d,-180,Convert(date,getdate()))and DateOFDismantle is null";

            //    Query = "select convert(decimal(18, 2),((tonnage)/1000)) as tonnage, * from VW_TblRegister where clientid ='" + databaseCon.Decrypt(id) + "' and SiteLocId='" + databaseCon.Decrypt(siteid) + "' and ('All'='" + Ownername + "' or ScaffoldOwner ='" + Ownername + "') and HireDaysToDate>180 order by DateTagged,ScaffRef";

            //}

            //if (databaseCon.Decrypt(Request.QueryString["availablereportname"]) == "Over 30 days less than 90 days Standing Report")
            //{
            //    Query = "select convert(decimal(18, 2),((tonnage)/1000)) as tonnage, * from VW_TblRegister where clientid ='" + databaseCon.Decrypt(id) + "' and SiteLocId='" + databaseCon.Decrypt(siteid) + "' and ('All'='" + Ownername + "' or ScaffoldOwner ='" + Ownername + "') and HireDaysToDate>30 and HireDaysToDate<=90 order by DateTagged,ScaffRef";

            //}

            //if (databaseCon.Decrypt(Request.QueryString["availablereportname"]) == "Over 30 days Standing Report")
            //{
            //    Query = "select convert(decimal(18, 2),((tonnage)/1000)) as tonnage, * from VW_TblRegister where clientid ='" + databaseCon.Decrypt(id) + "' and SiteLocId='" + databaseCon.Decrypt(siteid) + "' and ('All'='" + Ownername + "' or ScaffoldOwner ='" + Ownername + "') and HireDaysToDate>30 order by DateTagged,ScaffRef";

            //}

            //if (databaseCon.Decrypt(Request.QueryString["availablereportname"]) == "ALL Current Standing Scaffolds Report")
            //{
            //    Query = "select convert(decimal(18, 2),((tonnage)/1000)) as tonnage, * from VW_TblRegister where clientid ='" + databaseCon.Decrypt(id) + "' and SiteLocId='" + databaseCon.Decrypt(siteid) + "' and ('All'='" + Ownername + "' or ScaffoldOwner ='" + Ownername + "') and HireDaysToDate is not null order by DateTagged,ScaffRef";

            //}

            string Query = "EXEC Pro_ExportToCsv @decryptedReportName = '" + decryptedReportName + "', @clientID = '" + Clientid + "', @siteid = '" + siteId + "', @Ownername = '" + Ownername + "', @Index = '" + "First" + "', @Days = '" + "0" + "'";



            DataTable dt = databaseCon.GetData(Query);

            iTextSharp.text.Document docs = new iTextSharp.text.Document(iTextSharp.text.PageSize.LETTER, -110f, -110f, 50f, 50f);
            docs.SetPageSize(new Rectangle(990, 600));

            iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(docs, Response.OutputStream);
            Response.ContentType = ("application/pdf");
            HeaderFooterst evnts = new HeaderFooterst();
            writer.PageEvent = evnts;
            evnts.Id = id;
            evnts.site = siteid;
            evnts.availablereportname = availablereportname;
            docs.Open();
            var Heading781 = FontFactory.GetFont("Arial", 10f, Font.BOLD, BaseColor.BLACK);
            var Heading782 = FontFactory.GetFont("Arial", 25f, Font.BOLD, BaseColor.BLACK);
            var DataFont = FontFactory.GetFont("Arial", 9f, Font.NORMAL, BaseColor.BLACK);
            Phrase phrase;
            PdfPCell cell;
            //PdfPTable headertable1 = new PdfPTable(16); //8-Nov-2021 Added New Column
            //added this code on 16/08/2023
            PdfPTable headertable1 = new PdfPTable(18);

            // headertable1.SetWidths(new int[] { 6, 7, 7, 7, 7, 12, 6, 7, 4, 5, 8, 9, 5, 5, 4, 4 });
            //added this code on 16/08/2023
            headertable1.SetWidths(new int[] { 4, 5, 6, 7, 7, 5, 9, 6, 7, 5, 5, 8, 8, 6, 5, 4, 4, 4 });

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["ScaffRef"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.PaddingTop = 2f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);

                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["WONumber"].ToString()), DataFont));
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
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);


                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["BuildingNo"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);


                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["FloorLevel"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);


                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Bearing"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);


                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Description"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);


                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["AssetNo"].ToString()), DataFont));
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
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);


                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["DaysSinceLastInsp"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);


                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Tonnage"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);


                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["DesignType"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);


                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["MaterialType"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);


                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["HireDaysToDate"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);

                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Length"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);

                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Width"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);

                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Height"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);

                    //added this code on 16/08/2023
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Status"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);
                }
            }

            docs.Add(headertable1);
            docs.Close();
        }

        public class HeaderFooterst : PdfPageEventHelper
        {
            public string Id { get; set; }
            public string site { get; set; }
            public string availablereportname { get; set; }
            List<PdfTemplate> pageNumberTemplates = new List<PdfTemplate>();// page header

            public override void OnStartPage(iTextSharp.text.pdf.PdfWriter writer, Document docs)
            {
                string ImagePath = System.Web.HttpContext.Current.Server.MapPath("~/Images/logo2.png");// image size
                Rectangle pageSize = docs.PageSize;
                // PdfPTable headertables = new PdfPTable(16);
                //PdfPTable headertables = new PdfPTable(17);

                PdfPTable headertables = new PdfPTable(18);

                //headertables.SetWidths(new int[] { 6, 7, 7, 7, 7, 12, 6, 7, 4, 5, 8, 9, 5, 5, 4, 4 });
                //added this code on 16/08/2023
                headertables.SetWidths(new int[] { 4, 5, 6, 7, 7, 5, 9, 6, 7, 5, 5, 8, 8, 6, 5, 4, 4, 4 });


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
                string namesite = Convert.ToString(databaseCon.Decrypt(site));
                string AvailableReporid = Convert.ToString(databaseCon.Decrypt(availablereportname));

                string date = (("Printed: " + DateTime.Now.ToString("dd-MMM-yyy")));

                string queryimage = "select ClientImage from tblclient where clientid = '" + id + "'";
                DataTable dtheaders = databaseCon.GetData(queryimage);

                string sitename = "select SiteLocID,SiteLocation from tblSiteLocation where SiteLocID = '" + namesite + "'";
                DataTable sitidname = databaseCon.GetData(sitename);






                //footerDate.HorizontalAlignment = Element.ALIGN_CENTER;
                //cell = new PdfPCell(footerdate);
                //cell.Border = 0;

                phrase = new Phrase();
                phrase.Add(new Chunk(date, DataFont));
                cell = new PdfPCell(phrase);
                // cell.Colspan = 16;
                // cell.Colspan = 17;
                //added this code on 16/08/2023
                cell.Colspan = 18;
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
                       // iTextSharp.text.Image imglogo = iTextSharp.text.Image.GetInstance(System.Web.HttpContext.Current.Server.MapPath("~/images/clientlogo/" + dtheaders.Rows[0]["ClientImage"].ToString()));
                        iTextSharp.text.Image imglogo = iTextSharp.text.Image.GetInstance(System.Web.HttpContext.Current.Server.MapPath
                            ("~/images/clientlogo/"+ dtheaders.Rows[0]["ClientImage"].ToString()));

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
                //  cell.Colspan = 6;
                cell.Colspan = 7;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell.PaddingLeft = 20f;
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

                phrase = new Phrase();
                phrase.Add(new Chunk("Site :", Heading787));
                cell = new PdfPCell(phrase);
                cell.Border = 0;
                cell.FixedHeight = 30f;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //cell.Colspan = 2;
                //added this code on 16/08/2023
                cell.Colspan = 3;
                headertables.AddCell(cell);

                if (sitidname.Rows.Count > 0)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(sitidname.Rows[0]["SiteLocation"].ToString()), Heading787));
                    cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.FixedHeight = 30f;
                    cell.Colspan = 5;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
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
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertables.AddCell(cell);
                }



                if (AvailableReporid == "Over 90 days less than 180 days Standing Report")
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("Over 90 Days less than 180 Days Standing Report", Heading787));
                    cell = new PdfPCell(phrase);
                    cell.Colspan = 14;
                    cell.PaddingBottom = 10f;
                    cell.Border = 0;
                    cell.PaddingTop = 5f;
                    cell.PaddingBottom = 13f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    headertables.AddCell(cell);
                }
                if (AvailableReporid == "Over 180 days Standing Report")
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("Over 180 Days Standing Report", Heading787));
                    cell = new PdfPCell(phrase);
                    cell.Colspan = 14;
                    cell.PaddingBottom = 10f;
                    cell.Border = 0;
                    cell.PaddingTop = 5f;
                    cell.PaddingBottom = 13f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    headertables.AddCell(cell);

                }
                if (AvailableReporid == "Over 30 days less than 90 days Standing Report")
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("Over 30 Days less than 90 Days Standing Report", Heading787));
                    cell = new PdfPCell(phrase);
                    cell.Colspan = 14;
                    cell.PaddingBottom = 10f;
                    cell.Border = 0;
                    cell.PaddingTop = 5f;
                    cell.PaddingBottom = 13f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    headertables.AddCell(cell);

                }
                if (AvailableReporid == "Over 30 days Standing Report")
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("Over 30 Days Standing Report", Heading787));
                    cell = new PdfPCell(phrase);
                    cell.Colspan = 14;
                    cell.PaddingBottom = 10f;
                    cell.Border = 0;
                    cell.PaddingTop = 5f;
                    cell.PaddingBottom = 13f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    headertables.AddCell(cell);

                }

                if (AvailableReporid == "ALL Current Standing Scaffolds Report")
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("ALL Current Standing Scaffolds Report", Heading787));
                    cell = new PdfPCell(phrase);
                    cell.Colspan = 14;
                    cell.PaddingBottom = 10f;
                    cell.Border = 0;
                    cell.PaddingTop = 5f;
                    cell.PaddingBottom = 13f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    headertables.AddCell(cell);

                }

                phrase = new Phrase();
                phrase.Add(new Chunk(" ", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 2;
                cell.PaddingBottom = 6f;
                cell.Border = 0;
                //cell.PaddingTop = -50f;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                headertables.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk(" ", Heading786));
                cell = new PdfPCell(phrase);
                //  cell.Colspan = 16;
                // cell.Colspan = 17;
                //added this code on 16/08/2023
                cell.Colspan = 18;
                cell.PaddingBottom = 6f;
                cell.Border = 0;
                //cell.PaddingTop = -50f;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                headertables.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Scaf Ref #", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingTop = 10f;
                // cell.Width = 100f;
                //cell.Left = 400f;
                //cell.PaddingRight = -100f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertables.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Wo Number", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingTop = 10f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertables.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Location", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingTop = 10f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertables.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Building / Module No.", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingTop = 10f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertables.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Floor Level", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingTop = 10f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertables.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Bearing", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingTop = 10f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertables.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Reason for Access. (Provide Detailed Description)", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingTop = 10f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertables.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Asset # / Func. Loc", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingTop = 10f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertables.AddCell(cell);

                phrase = new Phrase();
                //Commented on 14-09-2023 Renamed as Date Built
                //phrase.Add(new Chunk("Date Tagged", Heading786));
                phrase.Add(new Chunk("Date Built", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingTop = 10f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertables.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Days Since Last Insp.", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingTop = 10f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertables.AddCell(cell);



                phrase = new Phrase();
                phrase.Add(new Chunk("Tonnage", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingTop = 10f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertables.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Design Type", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingTop = 10f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertables.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Material Type", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingTop = 10f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertables.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Standing Days to Date", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingTop = 10f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertables.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Length", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingTop = 10f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertables.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Width", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingTop = 10f;

                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertables.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Height(TWP)", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingTop = 10f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertables.AddCell(cell);
                //added this code on 11/08/2023
                phrase = new Phrase();
                phrase.Add(new Chunk("Status", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingTop = 10f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertables.AddCell(cell);

                headertables.AddCell(cell);
                docs.Add(headertables);

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