using iTextSharp.text.pdf;
using iTextSharp.text;
using Scaffold.App_Code;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Data;
using System.Security.Cryptography.Xml;


namespace Scaffold.Controllers
{
    public class Qty_And_Tonnage_By_Workgroup_Standing_JobsController : Controller
    {
        // GET: Qty_And_Tonnage_By_Workgroup_Standing_Jobs
        public ActionResult Index()
        {
            return View();
        }
        public void Qty_And_Tonnage_By_Workgroup_Standing_Jobs(string UID, string id, string site, string Owner, string availablereportname, string btn)
        {
            string ReportName = "Qty & Tonnage By Workgroup, (Standing Jobs)";
            string ReportName1 = "Site Tonnage Summary";
            var csvfilename = "";
            var siteid = databaseCon.Decrypt(site);
            var Id = databaseCon.Decrypt(id);
            csvfilename = "Qty And Tonnage By Workgroup (Standing Jobs)";
            string Ownername = databaseCon.Decrypt(Owner).Replace("'", "''").Trim();
            iTextSharp.text.Document docs = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, -110f, -110f, 50f, 50f);
            docs.SetPageSize(new Rectangle(1000, 900));
            iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(docs, Response.OutputStream);
            Response.ContentType = "application/pdf";
            HeaderFooterst evnts = new HeaderFooterst
            {
                Id = id,
                Report = 1,
                site = site,
                availablereportname = ReportName
            };

          
            writer.PageEvent = evnts;
            docs.Open();
            var Heading781 = FontFactory.GetFont("Arial", 10f, Font.BOLD, BaseColor.BLACK);
            var Heading782 = FontFactory.GetFont("Arial", 20f, Font.BOLD, BaseColor.BLACK);
            var DataFont = FontFactory.GetFont("Arial", 9f, Font.NORMAL, BaseColor.BLACK);
            PdfPTable headertable1 = new PdfPTable(3);
            headertable1.SetWidths(new int[] { 30,30,40 });
            headertable1.HeaderRows = 1;
            PdfPCell cell = new PdfPCell(new Phrase(ReportName, Heading782))
            {
                Colspan = 3,
                PaddingBottom = 8f,
                Border = 0,
                PaddingTop = 5f,
                HorizontalAlignment = Element.ALIGN_LEFT
            };
            headertable1.AddCell(cell);
            string[] headers = { "Work Group", "Qty of Scaffolds by Work Group", "Tonnage by Work Group" };
            foreach (var header in headers)
            {
                cell = new PdfPCell(new Phrase(header, Heading781))
                {
                    BackgroundColor = BaseColor.LIGHT_GRAY,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    PaddingTop = 5f,
                    PaddingBottom = 7f,
                    Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER
                };
                headertable1.AddCell(cell);
            }


            PdfPTable headertable2 = new PdfPTable(3);
            headertable2.SetWidths(new int[] { 30, 30, 40 });
            headertable2.HeaderRows = 1;

            PdfPCell cell2 = new PdfPCell(new Phrase(ReportName1, Heading782))
            {
                Colspan = 3,
                PaddingBottom = 8f,
                Border = 0,
                PaddingTop = 5f,
                HorizontalAlignment = Element.ALIGN_LEFT
            };
            headertable2.AddCell(cell2);

            string[] headers2 = { "On Site", "Standing", "Available" };
            foreach (var header in headers2)
            {
                cell = new PdfPCell(new Phrase(header, Heading781))
                {
                    BackgroundColor = BaseColor.LIGHT_GRAY,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    PaddingTop = 5f,
                    PaddingBottom = 7f,
                    Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER
                };
                headertable2.AddCell(cell);
            }

            //string Query = "EXEC Pro_QtyAndTonnageStandingReport '"+ Id +"','"+ siteid + "'";

            string Query = "EXEC Pro_ExportToCsv @decryptedReportName = '" + csvfilename + "', @clientID = '" + Id + "', @siteid = '" + siteid + "', @Ownername = '" + Ownername + "', @Index = '" + "First" + "', @Days = '" + "0" + "'";

            DataTable dt = databaseCon.GetData(Query);

            //string Query1 = "select Convert(decimal(18,2),[OnSite]/1000) [On Site], Convert(decimal(18, 2),[OutOfSite]/1000) [Standing], Convert(decimal(18, 2),[InSite]/1000) [Available] from(select  Sum(AvailableQuantity * ItemWeight) as 'InSite',Sum((isnull(QuantityToBeSent, 0)) * ItemWeight) as  'OutOfSite',Sum(((isnull(OrgSiteQuantity,0)-isnull(QuantityReturnedToDepot,0)-Isnull(JobQuantityDamaged,0)-Isnull(SiteDamagedQuantityReturnToDepot,0))* ItemWeight)) as 'OnSite' from vw_SiteInventory join tblInventory On tblInventory.InventoryId = vw_SiteInventory.InventoryId where SiteId ='" + siteid + "' and ClientID = '" + Id + "' ) as vw_inv";

           string Query1 = "EXEC Pro_ExportToCsv @decryptedReportName = '" + csvfilename + "', @clientID = '" + Id + "', @siteid = '" + siteid + "', @Ownername = '" + Ownername + "', @Index = '" + "Second" + "', @Days = '" + "0" + "'";
            DataTable dt1 = databaseCon.GetData(Query1);

            if (dt.Rows.Count > 1)
            {
                if (dt.Rows.Count > 5)
                {
                    headertable1.HeaderRows = 2;
                }
                int rowIndex = 0;
                foreach (DataRow row in dt.Rows)
                {
                    bool isLastRow = rowIndex == dt.Rows.Count - 1;

                    // Changed to single loop through columns
                    for (int colIndex = 0; colIndex < headers.Length; colIndex++)
                    {
                        string header = headers[colIndex];
                        string columnName = header.Replace(" (provide Detailed Description)", "").Trim();
                        string cellText = Convert.ToString(row[columnName]);

                       // PdfPCell cell;

                        if (isLastRow)
                        {
                            cell = new PdfPCell(new Phrase(cellText, DataFont))
                            {
                                PaddingBottom = 7f,
                                PaddingTop = 5f,
                                //Border = PdfPCell.TOP_BORDER | PdfPCell.RIGHT_BORDER
                                Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER |
                                        PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER
                            };

                            if (colIndex == 0) // First column
                                {
                                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.RIGHT_BORDER;
                                }
                            else // Other columns
                                {
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                                }
                            }
                        else
                            {
                        cell = new PdfPCell(new Phrase(cellText, DataFont))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER,
                                PaddingBottom = 7f,
                                PaddingTop = 5f,
                                Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER |
                                        PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER
                        };
                        }

                        headertable1.AddCell(cell);
                    }
                    rowIndex++; // Moved outside column loop
                }
            }
            else
            {
                cell = new PdfPCell(new Phrase("No data available", DataFont))
                {
                    Colspan = headers.Length,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    PaddingBottom = 7f,
                    PaddingTop = 5f,
                    Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER
                };
                headertable1.AddCell(cell);


            }

            docs.Add(headertable1);

            docs.Add(new Paragraph(" ", Heading781));
            docs.Add(new Paragraph(" ", Heading781));
            docs.Add(new Paragraph(" ", Heading781));

            if (dt1.Rows.Count > 0)
            {
                foreach (DataRow row in dt1.Rows)
                {
                    for (int colIndex = 0; colIndex < headers2.Length; colIndex++)
                    {
                        string columnName = headers2[colIndex];
                        string cellText = Convert.ToString(row[columnName]);

                        cell = new PdfPCell(new Phrase(cellText, DataFont))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            PaddingBottom = 7f,
                            PaddingTop = 5f,
                            Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER
                        };

                        headertable2.AddCell(cell);
                    }
                }
            }
            else
            {
                PdfPCell noDataCell = new PdfPCell(new Phrase("No data available", DataFont))
                {
                    Colspan = headers2.Length,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    PaddingTop = 5f,
                    PaddingBottom = 7f,
                    Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER
                };
                headertable2.AddCell(noDataCell);
            }

            docs.Add(headertable2);
            docs.Close();
            Response.End();
        }




        public class HeaderFooterst : PdfPageEventHelper
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
                PdfPTable headertables = new PdfPTable(19);

                //headertables.SetWidths(new int[] { 6, 7, 7, 7, 7, 12, 6, 7, 4, 5, 8, 9, 5, 5, 4, 4 });
                //added this code on 16/08/2023
                //headertables.SetWidths(new int[] { 4, 5, 6, 6, 5, 6, 8, 6, 5, 5, 6, 6, 6, 10, 10, 6, 5, 4, 5, 6 });
                headertables.SetWidths(new int[] { 4, 5, 6, 6, 5, 6, 8, 6, 5, 5, 6, 6, 10, 10, 6, 5, 4, 5, 6 });


                var contentfont = FontFactory.GetFont("Arial", 8f, BaseColor.BLACK);
                var Headings = FontFactory.GetFont("Arial", 9f, Font.BOLD, BaseColor.BLACK);
                var Head = FontFactory.GetFont("Arial", 18f, Font.BOLD, BaseColor.BLACK);
                var BackOrderheader = FontFactory.GetFont("Arial", 20f, Font.BOLD, BaseColor.BLACK);
                var Heading787 = FontFactory.GetFont("Arial", 20f, Font.BOLD, BaseColor.BLACK);
                var Heading786 = FontFactory.GetFont("Arial", 10f, Font.BOLD, BaseColor.BLACK);
                var DataFont = FontFactory.GetFont("Arial", 13f, Font.NORMAL, BaseColor.BLACK);
                Phrase phrase;
                PdfPCell cell;


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
                    //  cell.Colspan = 6;
                    cell.Colspan = 9;
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