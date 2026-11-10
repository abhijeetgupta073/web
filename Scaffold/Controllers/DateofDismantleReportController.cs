using iTextSharp.text.pdf;
using iTextSharp.text;
using Scaffold.App_Code;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Data;


namespace Scaffold.Controllers
{
    public class Date_of_DismantleReportController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        //    public void DateofDismantleReport(string UID, string id, string site, string Owner, string availablereportname, string btn, string ClientID, string SiteLocId)
        //    {
        //        string ReportName = "Report:'Date of Dismantle' Entered By:";
        //        string Ownername = databaseCon.Decrypt(Owner).Replace("'", "''").Trim();
        //        iTextSharp.text.Document docs = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, -110f, -110f, 50f, 50f);
        //        docs.SetPageSize(new Rectangle(1000, 900));
        //        iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(docs, Response.OutputStream);
        //        Response.ContentType = "application/pdf";
        //        HeaderFooterst evnts = new HeaderFooterst
        //        {
        //            Id = id,
        //            Report = 1,
        //            site = site,
        //            availablereportname = availablereportname
        //        };
        //        writer.PageEvent = evnts;
        //        docs.Open();
        //        var Heading781 = FontFactory.GetFont("Arial", 10f, Font.BOLD, BaseColor.BLACK);
        //        var Heading782 = FontFactory.GetFont("Arial", 20f, Font.BOLD, BaseColor.BLACK);
        //        var DataFont = FontFactory.GetFont("Arial", 9f, Font.NORMAL, BaseColor.BLACK);
        //        PdfPTable headertable1 = new PdfPTable(13);
        //        headertable1.SetWidths(new int[] { 5, 5, 6, 6, 6, 9, 6, 6, 10, 10, 6, 9, 12 });
        //        headertable1.HeaderRows = 4;
        //        PdfPCell cell = new PdfPCell(new Phrase(ReportName, Heading782))
        //        {
        //            Colspan = 13,
        //            PaddingBottom = 8f,
        //            Border = 0,
        //            PaddingTop = 5f,
        //            HorizontalAlignment = Element.ALIGN_LEFT
        //        };
        //        headertable1.AddCell(cell);
        //        string[] headers = { "ScaffRef", "Client Scaff Ref", "WO No", "Site", "Location", "Building/Module", "Floor Level", "Bearing", "Reason For Access", "Asset#/Func.Loc", "Date Built", "Date Dismantled", "Date Dismantled Entered by" };
        //        foreach (var header in headers)
        //        {
        //            cell = new PdfPCell(new Phrase(header, Heading781))
        //            {
        //                BackgroundColor = BaseColor.LIGHT_GRAY,
        //                HorizontalAlignment = Element.ALIGN_CENTER,
        //                PaddingTop = 10f,
        //                PaddingBottom = 5f,
        //                Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER
        //            };
        //            headertable1.AddCell(cell);
        //        }
        //        string Query = $@"
        //            SELECT 
        //                ScaffRef,
        //                ClientScaffRef AS [Client Scaff Ref],  
        //                WONumber AS [WO No], 
        //                SiteLocation AS [Site], 
        //                Location, 
        //                BuildingNo AS [Building/Module], 
        //                FloorLevel AS [Floor Level], 
        //                Bearing,
        //                Description AS [Reason For Access],
        //                AssetNo AS [Asset#/Func.Loc], 
        //                DateTagged AS [Date Built], 
        //                CASE 
        //            WHEN DateOfDismantle IS NULL OR DateOfDismantle = '1900-01-01' 
        //            THEN ''
        //            ELSE CONVERT(varchar, DateOfDismantle, 121)
        //        END AS [Date Dismantled], 
        //        CASE 
        //            WHEN DateOfDismantle IS NULL OR DateOfDismantle = '1900-01-01' 
        //            THEN '' 
        //            ELSE ISNULL(UpdatedBy, ISNULL(CreatedBy, '')) 
        //        END AS [Date Dismantled Entered by]
        //    FROM 
        //                dbo.VW_TblRegister 
        //            WHERE 
        //                ClientID = '{ClientID}' AND 
        //                SiteLocId = '{SiteLocId}' AND 
        //                ('All' = 'All' OR ScaffoldOwner = 'All') 
        //            ORDER BY 
        //                DateTagged, 
        //                ScaffRef";
        //        DataTable dt = databaseCon.GetData(Query);

        //        if (dt.Rows.Count > 0)
        //        {
        //            foreach (DataRow row in dt.Rows)
        //            {
        //                foreach (var header in headers)
        //                {
        //                    string columnName = header.Replace(" (provide Detailed Description)", "").Trim();
        //                    string cellText = Convert.ToString(row[columnName]);
        //                    if (columnName == "Date Built" || columnName == "Date Dismantled")
        //                    {
        //                        if (DateTime.TryParse(cellText, out DateTime dateValue))
        //                        {
        //                            if (dateValue != DateTime.Parse("1900-01-01"))
        //                            {
        //                                cellText = dateValue.ToString("dd-MMM-yyyy");
        //                            }
        //                            else
        //                            {
        //                                cellText = "";
        //                            }
        //                        }
        //                        else
        //                        {
        //                            cellText = "";
        //                        }
        //                    }
        //                    cell = new PdfPCell(new Phrase(cellText, DataFont))
        //                    {
        //                        PaddingBottom = 15f,
        //                        HorizontalAlignment = Element.ALIGN_CENTER,
        //                        Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER,
        //                        PaddingTop = 2f
        //                    };
        //                    headertable1.AddCell(cell);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            cell = new PdfPCell(new Phrase("No data available", DataFont))
        //            {
        //                Colspan = headers.Length,
        //                HorizontalAlignment = Element.ALIGN_CENTER,
        //                PaddingTop = 10f,
        //                PaddingBottom = 10f,
        //                Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER
        //            };
        //            headertable1.AddCell(cell);
        //        }

        //        docs.Add(headertable1);
        //        docs.Close();
        //        Response.End();

        //}

        public void Date_of_DismantleReport(string UID, string id, string site, string Owner, string availablereportname, string btn)
        {
            string ReportName = "'Date of Dismantle' Entered By";
            string Ownername = databaseCon.Decrypt(Owner).Replace("'", "''").Trim();
            var csvfilename = "";


            var siteid = databaseCon.Decrypt(site);
            var Id = databaseCon.Decrypt(id);
            string decryptedReportName = databaseCon.Decrypt(Request.QueryString["availablereportname"]);
            csvfilename = "Date_of_Dismantle_Entered_By";
            iTextSharp.text.Document docs = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, -110f, -110f, 50f, 50f);
            docs.SetPageSize(new Rectangle(1000, 900));
            iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(docs, Response.OutputStream);
            Response.ContentType = "application/pdf";
            HeaderFooterst evnts = new HeaderFooterst
            {
                Id = id,
                Report = 1,
                site = site,
                availablereportname = availablereportname
            };
            writer.PageEvent = evnts;
            docs.Open();
            var Heading781 = FontFactory.GetFont("Arial", 10f, Font.BOLD, BaseColor.BLACK);
            var Heading782 = FontFactory.GetFont("Arial", 20f, Font.BOLD, BaseColor.BLACK);
            var DataFont = FontFactory.GetFont("Arial", 9f, Font.NORMAL, BaseColor.BLACK);
            PdfPTable headertable1 = new PdfPTable(13);
            headertable1.SetWidths(new int[] { 6, 5, 6, 6, 6, 9, 6, 9, 10, 10, 6, 9, 12 });
            headertable1.HeaderRows = 1;
            PdfPCell cell = new PdfPCell(new Phrase(ReportName, Heading782))
            {
                Colspan = 13,
                PaddingBottom = 8f,
                Border = 0,
                PaddingTop = 5f,
                HorizontalAlignment = Element.ALIGN_LEFT
            };
            headertable1.AddCell(cell);
            string[] headers = { "Scaf Ref #", "Client Scaf Ref", "WO No.", "Site", "Location", "Building/Module No.", "Floor Level", "Bearing", "Reason For Access.", "Asset # /Func. Loc", "Date Built", "Date Dismantled", "Date Dismantled Entered by" };
            foreach (var header in headers)
            {
                cell = new PdfPCell(new Phrase(header, Heading781))
                {
                    BackgroundColor = BaseColor.LIGHT_GRAY,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    PaddingTop = 10f,
                    PaddingBottom = 5f,
                    Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER
                };
                headertable1.AddCell(cell);
            }



            string Query = "EXEC Pro_ExportToCsv @decryptedReportName = '" + csvfilename + "', @clientID = '" + Id + "', @siteid = '" + siteid + "', @Ownername = '" + Ownername + "', @Index = '" + "First" + "', @Days = '" + "0" + "'";

            //    string Query = $@"
            // SELECT 
            //    ScaffRef AS [Scaf Ref #],
            //    ClientScaffRef AS [Client Scaf Ref],  
            //    WONumber AS [WO No.], 
            //    SiteLocation AS [Site], 
            //    Location, 
            //    BuildingNo AS [Building/Module No.], 
            //    FloorLevel AS [Floor Level], 
            //    Bearing,
            //    Description AS [Reason For Access.],
            //    AssetNo AS [Asset # /Func. Loc], 
            //    DateTagged AS [Date Built], 
            //    CASE 
            //        WHEN DateOfDismantle IS NULL OR DateOfDismantle = '1900-01-01' 
            //        THEN ''
            //        ELSE CONVERT(varchar, DateOfDismantle, 121)
            //    END AS [Date Dismantled], 
            //    CASE 
            //        WHEN DateOfDismantle IS NULL OR DateOfDismantle = '1900-01-01' 
            //        THEN '' 
            //        ELSE ISNULL(UpdatedBy, ISNULL(CreatedBy, '')) 
            //    END AS [Date Dismantled Entered by]
            //FROM 
            //    dbo.VW_TblRegister 
            //WHERE 
            //    ClientID = '{databaseCon.Decrypt(id)}' AND SiteLocId = '{databaseCon.Decrypt(site)}' AND
            //    ('All' = 'All' OR ScaffoldOwner = 'All') AND
            //    isnull(DateOfDismantle,'1900-01-01')!='1900-01-01'
            //ORDER BY 

            //    ScaffRef";

            DataTable dt = databaseCon.GetData(Query);

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows.Count > 5)
                {
                    headertable1.HeaderRows = 2;
                }
                foreach (DataRow row in dt.Rows)
                {
                    foreach (var header in headers)
                    {
                        string columnName = header.Replace(" (provide Detailed Description)", "").Trim();
                        string cellText = Convert.ToString(row[columnName]);
                        if (columnName == "Date Built" || columnName == "Date Dismantled")
                        {
                            try
                            {
                                var dateValue = DateTime.Parse(cellText);
                                if (dateValue != DateTime.Parse("1900-01-01"))
                                {
                                    cellText = dateValue.ToString("dd-MMM-yyyy");
                                }
                                else
                                {
                                    cellText = "";
                                }
                            }
                            catch
                            {
                                cellText = "";
                            }
                        }
                        cell = new PdfPCell(new Phrase(cellText, DataFont))
                        {
                            PaddingBottom = 15f,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER,
                            PaddingTop = 2f
                        };
                        headertable1.AddCell(cell);
                    }

                }
            }
            else
            {
                cell = new PdfPCell(new Phrase("No data available", DataFont))
                {
                    Colspan = headers.Length,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    PaddingBottom = 15f,
                    PaddingTop = 2f,
                    Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER
                };
                headertable1.AddCell(cell);


            }

            docs.Add(headertable1);
            docs.Close();
            Response.End();
        }


        public void Build_Dismantled_By_ReportReport(string UID, string id, string site, string Owner, string availablereportname, string btn)
        {
            string ReportName = "Build & Dismantled By Report";
            string Ownername = databaseCon.Decrypt(Owner).Replace("'", "''").Trim();
            var csvfilename = "";


            var siteid = databaseCon.Decrypt(site);
            var Id = databaseCon.Decrypt(id);
            string decryptedReportName = databaseCon.Decrypt(Request.QueryString["availablereportname"]);
            csvfilename = "Build_&_Dismantled_By_Report";
            iTextSharp.text.Document docs = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, -110f, -110f, 50f, 50f);
            docs.SetPageSize(new Rectangle(1000, 900));
            iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(docs, Response.OutputStream);
            Response.ContentType = "application/pdf";
            HeaderFooterst evnts = new HeaderFooterst
            {
                Id = id,
                Report = 1,
                site = site,
                availablereportname = availablereportname
            };
            writer.PageEvent = evnts;
            docs.Open();
            var Heading781 = FontFactory.GetFont("Arial", 10f, Font.BOLD, BaseColor.BLACK);
            var Heading782 = FontFactory.GetFont("Arial", 20f, Font.BOLD, BaseColor.BLACK);
            var DataFont = FontFactory.GetFont("Arial", 9f, Font.NORMAL, BaseColor.BLACK);
            PdfPTable headertable1 = new PdfPTable(14);
            headertable1.SetWidths(new int[] { 5, 5, 6, 6, 6, 9, 6, 9, 9, 9, 6,9, 6, 9 });
            headertable1.HeaderRows = 1;
            PdfPCell cell = new PdfPCell(new Phrase(ReportName, Heading782))
            {
                Colspan = 14,
                PaddingBottom = 8f,
                Border = 0,
                PaddingTop = 5f,
                HorizontalAlignment = Element.ALIGN_LEFT
            };
            headertable1.AddCell(cell);
            string[] headers = { "Scaf Ref #", "Client Scaf Ref", "WO No.", "Site", "Location", "Building/Module No.", "Floor Level", "Bearing", "Reason For Access.", "Asset # /Func. Loc", "Date Built","Built By", "Date Dismantled", "Dismantled By" };
            foreach (var header in headers)
            {
                cell = new PdfPCell(new Phrase(header, Heading781))
                {
                    BackgroundColor = BaseColor.LIGHT_GRAY,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    PaddingTop = 10f,
                    PaddingBottom = 5f,
                    Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER
                };
                headertable1.AddCell(cell);
            }

          

            string Query = "EXEC Pro_ExportToCsv @decryptedReportName = '" + csvfilename + "', @clientID = '" + Id + "', @siteid = '" + siteid + "', @Ownername = '" + Ownername + "', @Index = '" + "First" + "', @Days = '" + "0" + "'";

            //    string Query = $@"
            // SELECT 
            //    ScaffRef AS [Scaf Ref #],
            //    ClientScaffRef AS [Client Scaf Ref],  
            //    WONumber AS [WO No.], 
            //    SiteLocation AS [Site], 
            //    Location, 
            //    BuildingNo AS [Building/Module No.], 
            //    FloorLevel AS [Floor Level], 
            //    Bearing,
            //    Description AS [Reason For Access.],
            //    AssetNo AS [Asset # /Func. Loc], 
            //    DateTagged AS [Date Built], 
            //    CASE 
            //        WHEN DateOfDismantle IS NULL OR DateOfDismantle = '1900-01-01' 
            //        THEN ''
            //        ELSE CONVERT(varchar, DateOfDismantle, 121)
            //    END AS [Date Dismantled], 
            //    CASE 
            //        WHEN DateOfDismantle IS NULL OR DateOfDismantle = '1900-01-01' 
            //        THEN '' 
            //        ELSE ISNULL(UpdatedBy, ISNULL(CreatedBy, '')) 
            //    END AS [Date Dismantled Entered by]
            //FROM 
            //    dbo.VW_TblRegister 
            //WHERE 
            //    ClientID = '{databaseCon.Decrypt(id)}' AND SiteLocId = '{databaseCon.Decrypt(site)}' AND
            //    ('All' = 'All' OR ScaffoldOwner = 'All') AND
            //    isnull(DateOfDismantle,'1900-01-01')!='1900-01-01'
            //ORDER BY 

            //    ScaffRef";

            DataTable dt = databaseCon.GetData(Query);

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows.Count > 5)
                {
                    headertable1.HeaderRows = 2;
                }
                foreach (DataRow row in dt.Rows)
                {
                    foreach (var header in headers)
                    {
                        string columnName = header.Replace(" (provide Detailed Description)", "").Trim();
                        string cellText = Convert.ToString(row[columnName]);
                        if (columnName == "Date Built" || columnName == "Date Dismantled")
                        {
                            try
                            {
                                var dateValue = DateTime.Parse(cellText);
                                if (dateValue != DateTime.Parse("1900-01-01"))
                                {
                                    cellText = dateValue.ToString("dd-MMM-yyyy");
                                }
                                else
                                {
                                    cellText = "";
                                }
                            }
                            catch
                            {
                                cellText = "";
                            }
                        }
                        cell = new PdfPCell(new Phrase(cellText, DataFont))
                        {
                            PaddingBottom = 15f,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER,
                            PaddingTop = 2f
                        };
                        headertable1.AddCell(cell);
                    }

                }
            }
            else
            {
                cell = new PdfPCell(new Phrase("No data available", DataFont))
                {
                    Colspan = headers.Length,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    PaddingBottom = 15f,
                    PaddingTop = 2f,
                    Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER
                };
                headertable1.AddCell(cell);


            }

            docs.Add(headertable1);
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


                    //if (AvailableReporid == "Reports Having Date Built Filled last 7 days")

                    //phrase = new Phrase();
                    //phrase.Add(new Chunk(" ", Heading787));
                    //cell = new PdfPCell(phrase);
                    //cell.Colspan = 20;
                    ////cell.PaddingBottom = 5f;
                    //cell.Border = 0;
                    ////cell.PaddingTop = 5f;
                    ////cell.PaddingBottom = 5f;
                    //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //headertables.AddCell(cell);

                    //phrase = new Phrase();
                    //phrase.Add(new Chunk("Built in last 7 Days", Heading787));
                    //cell = new PdfPCell(phrase);
                    //cell.Colspan = 20;
                    //cell.PaddingBottom = 2f;
                    //cell.Border = 0;
                    //cell.PaddingTop = 5f;
                    //cell.PaddingBottom = 2f;
                    //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //headertables.AddCell(cell);

                    //// if (AvailableReporid == "Reports Having Date Dismantle Filled last 7 days")
                    ////else if (ReportCount == 2)
                    ////{
                    ////    phrase = new Phrase();
                    ////    phrase.Add(new Chunk("Reports Having Date Dismantle Filled last 7 days", Heading787));
                    ////    cell = new PdfPCell(phrase);
                    ////    cell.Colspan = 14;
                    ////    cell.PaddingBottom = 10f;
                    ////    cell.Border = 0;
                    ////    cell.PaddingTop = 4f;
                    ////    cell.PaddingBottom = 11f;
                    ////    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    ////    headertables.AddCell(cell);

                    ////    }
                    ////    //if (AvailableReporid == "Reports Having Out Of Service Filled last 7 days")
                    ////   else
                    ////    {
                    ////    phrase = new Phrase();
                    ////    phrase.Add(new Chunk("Reports Having Out Of Service Filled last 7 days", Heading787));
                    ////    cell = new PdfPCell(phrase);
                    ////    cell.Colspan = 14;
                    ////    cell.PaddingBottom = 10f;
                    ////    cell.Border = 0;
                    ////    cell.PaddingTop = 5f;
                    ////    cell.PaddingBottom = 13f;
                    ////    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    ////    headertables.AddCell(cell);

                    ////}

                    ////phrase = new Phrase();
                    ////phrase.Add(new Chunk(" ", Heading786));
                    ////cell = new PdfPCell(phrase);
                    ////cell.Colspan = 2;
                    ////cell.PaddingBottom = 6f;
                    ////cell.Border = 0;
                    //////cell.PaddingTop = -50f;
                    ////cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    ////headertables.AddCell(cell);

                    //phrase = new Phrase();
                    //phrase.Add(new Chunk(" ", Heading786));
                    //cell = new PdfPCell(phrase);
                    ////  cell.Colspan = 16;
                    ////cell.Colspan = 17;
                    //// Added New Column Status on (11-08-2023)
                    //cell.Colspan = 20;
                    //cell.PaddingBottom = 6f;
                    //cell.Border = 0;
                    ////cell.PaddingTop = -50f;
                    //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //headertables.AddCell(cell);

                    //phrase = new Phrase();
                    //phrase.Add(new Chunk("Scaf Ref #", Heading786));
                    //cell = new PdfPCell(phrase);
                    //cell.Colspan = 1;
                    //cell.Border = 1;
                    //cell.PaddingTop = 10f;
                    //// cell.Width = 100f;
                    ////cell.Left = 400f;
                    ////cell.PaddingRight = -100f;
                    //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                    //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    ////cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //headertables.AddCell(cell);

                    //phrase = new Phrase();
                    //phrase.Add(new Chunk("Wo #", Heading786));
                    //cell = new PdfPCell(phrase);
                    //cell.Colspan = 1;
                    //cell.Border = 1;
                    //cell.PaddingTop = 10f;
                    //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                    //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //headertables.AddCell(cell);

                    //phrase = new Phrase();
                    //phrase.Add(new Chunk("Location", Heading786));
                    //cell = new PdfPCell(phrase);
                    //cell.Colspan = 1;
                    //cell.Border = 1;
                    //cell.PaddingTop = 10f;
                    //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                    //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //headertables.AddCell(cell);

                    //phrase = new Phrase();
                    //phrase.Add(new Chunk("Building / Module No.", Heading786));
                    //cell = new PdfPCell(phrase);
                    //cell.Colspan = 1;
                    //cell.Border = 1;
                    //cell.PaddingTop = 10f;
                    //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                    //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //headertables.AddCell(cell);

                    //phrase = new Phrase();
                    //phrase.Add(new Chunk("Floor Level", Heading786));
                    //cell = new PdfPCell(phrase);
                    //cell.Colspan = 1;
                    //cell.Border = 1;
                    //cell.PaddingTop = 10f;
                    //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                    //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //headertables.AddCell(cell);

                    //phrase = new Phrase();
                    //phrase.Add(new Chunk("Bearing", Heading786));
                    //cell = new PdfPCell(phrase);
                    //cell.Colspan = 1;
                    //cell.Border = 1;
                    //cell.PaddingTop = 10f;
                    //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                    //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //headertables.AddCell(cell);

                    //phrase = new Phrase();
                    //phrase.Add(new Chunk("Reason for Access.", Heading786));
                    //cell = new PdfPCell(phrase);
                    //cell.Colspan = 1;
                    //cell.Border = 1;
                    //cell.PaddingTop = 10f;
                    //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                    //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //headertables.AddCell(cell);

                    //phrase = new Phrase();
                    //phrase.Add(new Chunk("Asset # / Func. Loc", Heading786));
                    //cell = new PdfPCell(phrase);
                    //cell.Colspan = 1;
                    //cell.Border = 1;
                    //cell.PaddingTop = 10f;
                    //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                    //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //headertables.AddCell(cell);

                    //phrase = new Phrase();
                    ////Commented on 14-09-2023 Renamed as Date Built
                    ////phrase.Add(new Chunk("Date Tagged", Heading786));
                    //phrase.Add(new Chunk("Date Built", Heading786));
                    //cell = new PdfPCell(phrase);
                    //cell.Colspan = 1;
                    //cell.Border = 1;
                    //cell.PaddingTop = 10f;
                    //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                    //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    ////cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //headertables.AddCell(cell);

                    //phrase = new Phrase();
                    //phrase.Add(new Chunk("Date Dis", Heading786));
                    //cell = new PdfPCell(phrase);
                    //cell.Colspan = 1;
                    //cell.Border = 1;
                    //cell.PaddingTop = 10f;
                    //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                    //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    ////cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //headertables.AddCell(cell);

                    ////phrase = new Phrase();
                    ////phrase.Add(new Chunk("Date Out of Service", Heading786));
                    ////cell = new PdfPCell(phrase);
                    ////cell.Colspan = 1;
                    ////cell.Border = 1;
                    ////cell.PaddingTop = 10f;
                    ////cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                    ////cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    //////cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    ////cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    ////headertables.AddCell(cell);

                    //phrase = new Phrase();
                    //phrase.Add(new Chunk("Days Since Last Insp.", Heading786));
                    //cell = new PdfPCell(phrase);
                    //cell.Colspan = 1;
                    //cell.Border = 1;
                    //cell.PaddingTop = 10f;
                    //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                    //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    ////cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //headertables.AddCell(cell);



                    //phrase = new Phrase();
                    //phrase.Add(new Chunk("Tonnage", Heading786));
                    //cell = new PdfPCell(phrase);
                    //cell.Colspan = 1;
                    //cell.Border = 1;
                    //cell.PaddingTop = 10f;
                    //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                    //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    ////cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //headertables.AddCell(cell);

                    //phrase = new Phrase();
                    //phrase.Add(new Chunk("Design Type", Heading786));
                    //cell = new PdfPCell(phrase);
                    //cell.Colspan = 1;
                    //cell.Border = 1;
                    //cell.PaddingTop = 10f;
                    //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                    //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //headertables.AddCell(cell);

                    //phrase = new Phrase();
                    //phrase.Add(new Chunk("Material Type", Heading786));
                    //cell = new PdfPCell(phrase);
                    //cell.Colspan = 1;
                    //cell.Border = 1;
                    //cell.PaddingTop = 10f;
                    //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                    //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //headertables.AddCell(cell);

                    //phrase = new Phrase();
                    //phrase.Add(new Chunk("Standing Days to Date", Heading786));
                    //cell = new PdfPCell(phrase);
                    //cell.Colspan = 1;
                    //cell.Border = 1;
                    //cell.PaddingTop = 10f;
                    //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                    //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    ////cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //headertables.AddCell(cell);

                    //phrase = new Phrase();
                    //phrase.Add(new Chunk("Length", Heading786));
                    //cell = new PdfPCell(phrase);
                    //cell.Colspan = 1;
                    //cell.Border = 1;
                    //cell.PaddingTop = 10f;
                    //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                    //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    ////cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //headertables.AddCell(cell);

                    //phrase = new Phrase();
                    //phrase.Add(new Chunk("Width", Heading786));
                    //cell = new PdfPCell(phrase);
                    //cell.Colspan = 1;
                    //cell.Border = 1;
                    //cell.PaddingTop = 10f;
                    //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                    //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    ////cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //headertables.AddCell(cell);

                    //phrase = new Phrase();
                    //phrase.Add(new Chunk("Height(TWP)", Heading786));
                    //cell = new PdfPCell(phrase);
                    //cell.Colspan = 1;
                    //cell.Border = 1;
                    //cell.PaddingTop = 10f;
                    //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                    //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    ////cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //headertables.AddCell(cell);

                    //phrase = new Phrase();
                    //phrase.Add(new Chunk("Status", Heading786));
                    //cell = new PdfPCell(phrase);
                    //cell.Colspan = 1;
                    //cell.Border = 1;
                    //cell.PaddingTop = 10f;
                    //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                    //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    ////cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //headertables.AddCell(cell);
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