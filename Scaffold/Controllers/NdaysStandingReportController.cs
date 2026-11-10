
using iTextSharp.text.pdf;
using iTextSharp.text;
using Scaffold.App_Code;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Data;
using OfficeOpenXml;
using System.Linq;
using System.Text;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace Scaffold.Controllers
{
    public class NdaysStandingReportController : Controller
    {
        // GET: TwentyEightDaysStandingReport
        public ActionResult Index()
        {
            return View();
        }

        public void NdaysStandingReport(string UID, string id, string site, string Owner, string availablereportname, string btn, string Days)
        {
            string Query = "";
            string ReportName = "";
            var siteid = site;
            var csvfilename = "";


            var siteId = databaseCon.Decrypt(site);
            var Id = databaseCon.Decrypt(id);

            csvfilename = "Activity Report - User Set Days";
            //Added on 24-04-2024 for Decrypt Owner for Speacial Character like C&I
            string Ownername = databaseCon.Decrypt(Owner).Replace("'", "''").Trim();



            iTextSharp.text.Document docs = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, -110f, -110f, 50f, 50f);
            //iTextSharp.text.Document docs = new iTextSharp.text.Document(iTextSharp.text.PageSize.LETTER, -110f, -110f, 50f, 50f);
            docs.SetPageSize(new Rectangle(1000, 900));

            iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(docs, Response.OutputStream);
            Response.ContentType = ("application/pdf");
            HeaderFooterst evnts = new HeaderFooterst();
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
            //PdfPTable headertable1 = new PdfPTable(16); //8-Nov-2021 Added New Column
            // Added New Column Status on (11-08-2023)
            //PdfPTable headertable1 = new PdfPTable(17);
            //PdfPTable headertable1 = new PdfPTable(20);
            PdfPTable headertable1 = new PdfPTable(19);

            // headertable1.SetWidths(new int[] { 6, 7, 7, 7, 7, 12, 6, 7, 4, 5, 8, 9, 5, 5, 4, 4 });
            //added this code on 16/08/2023
            //headertable1.SetWidths(new int[] { 4, 5, 6, 6, 5, 6, 8, 6, 5, 5, 6, 6, 6, 10, 10, 6, 5, 4, 5, 6 });
            headertable1.SetWidths(new int[] { 4, 5, 6, 6, 5, 6, 8, 6, 5, 5, 6, 6, 10, 10, 6, 5, 4, 5, 6 });

            //We only need Date Dismantle and Date Built Data
            //for (int Report = 1; Report <= 3; Report++)
            for (int Report = 1; Report <= 2; Report++)
            {
                headertable1.HeaderRows = 4;
                if (Report == 1)
                {
                    //ReportName = "Built in last 28 Days";
                    ReportName = "Built in last " + Days + " Days";
                    //Query = "select convert(decimal(18, 2),((tonnage)/1000)) as tonnage, * from VW_TblRegister where clientid ='" + databaseCon.Decrypt(id) + "' and SiteLocId='" + databaseCon.Decrypt(siteid) + "' and ('All'='" + Ownername + "' or ScaffoldOwner ='" + Ownername + "') and isnull(DateTagged,'1900-01-01') > convert(date,DATEADD(day, -" + Days + ", GETDATE())) order by DateTagged,ScaffRef";

                    Query = "EXEC Pro_ExportToCsv @decryptedReportName = '" + csvfilename + "', @clientID = '" + Id + "', @siteid = '" + siteId + "', @Ownername = '" + Ownername + "', @Index = '" + "First" + "', @Days = '" + Days + "'";
                    // Query = "select convert(decimal(18, 2),((tonnage)/1000)) as tonnage, * from VW_TblRegister " +
                    //"where clientid ='" + databaseCon.Decrypt(id) + "' " +
                    //"and SiteLocId='" + databaseCon.Decrypt(site) + "' " +
                    //"and ('All'='" + Owner + "' or ScaffoldOwner ='" + Owner + "') " +
                    //"and isnull(DateTagged,'1900-01-01') > convert(date,DATEADD(day, -CONVERT(int, " + Days + "), GETDATE())) " +
                    //"order by DateTagged, ScaffRef";
                    // evnts.Report = 2;

                }
                else if (Report == 2)
                {
                    evnts.Report = 2;
                    ReportName = "Dismantled in last " + Days + " Days";
                    //Query = "select convert(decimal(18, 2),((tonnage)/1000)) as tonnage, * from VW_TblRegister where clientid ='" + databaseCon.Decrypt(id) + "' and SiteLocId='" + databaseCon.Decrypt(siteid) + "' and ('All'='" + Ownername + "' or ScaffoldOwner ='" + Ownername + "') and isnull(DateOfDismantle,'1900-01-01') > convert(date,DATEADD(day, -" + Days + ", GETDATE())) order by DateTagged,ScaffRef";

                    Query = "EXEC Pro_ExportToCsv @decryptedReportName = '" + csvfilename + "', @clientID = '" + Id + "', @siteid = '" + siteId + "', @Ownername = '" + Ownername + "', @Index = '" + "Second" + "', @Days = '" + Days + "'";
                }
                else
                {
                    ReportName = "Out Of Service in last " + Days + " days";

                    evnts.Report = 3;

                    //Query = "SELECT ScaffRef, WONumber, Location, BuildingNo, FloorLevel, Bearing, Description, AssetNo, DateTagged, DateOFDismantle, RecentInspDate, DaysSinceLastInsp, CONVERT(decimal(18, 2), (tonnage / 1000)) as tonnage, DesignType, MaterialType, HireDaysToDate, Length, Width, Height, Status FROM VW_TblRegister WHERE clientid = '" + databaseCon.Decrypt(id) + "' AND SiteLocId = '" + databaseCon.Decrypt(siteid) + "' AND ('All' = '" + Ownername + "' OR ScaffoldOwner = '" + Ownername + "') AND ((ISDATE(isnull(RecentInspDate,'1900-01-01')) = 1 AND CONVERT(date, isnull(RecentInspDate,'1900-01-01')) > DATEADD(day,-" + Days + ", GETDATE())) AND (isnull(RecentInspDate,'') != 'Not Yet Inspected' OR isnull(RecentInspDate,'') != 'Not Yet Tagged')) AND Status = 'Out Of Service' ORDER BY DateTagged, ScaffRef";

                    Query = "EXEC Pro_ExportToCsv @decryptedReportName = '" + csvfilename + "', @clientID = '" + Id + "', @siteid = '" + siteId + "', @Ownername = '" + Ownername + "', @Index = '" + "Third" + "', @Days = '" + Days + "'";


                }

                phrase = new Phrase();
                phrase.Add(new Chunk(" ", Heading781));
                cell = new PdfPCell(phrase);
                cell.Colspan = 20;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                headertable1.AddCell(cell);

                //if (Report != 1)
                //{
                //    docs.Add(headertable1);
                //    headertable1.DeleteBodyRows();
                //    for (int m = headertable1.Rows.Count - 1; m >= 0; m--)
                //    {
                //        headertable1.Rows.RemoveAt(m);
                //    }
                //    docs.NewPage();

                //}

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
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable1.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Wo #", Heading781));
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
                phrase.Add(new Chunk("Building / Module No.", Heading781));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingTop = 10f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Floor Level", Heading781));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingTop = 10f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Bearing", Heading781));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingTop = 10f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Reason for Access.", Heading781));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingTop = 10f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Asset # / Func. Loc", Heading781));
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
                phrase.Add(new Chunk("Date Built", Heading781));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingTop = 10f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable1.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Date Dis", Heading781));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingTop = 10f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable1.AddCell(cell);

                //phrase = new Phrase();
                //phrase.Add(new Chunk("Date Out of Service", Heading781));
                //cell = new PdfPCell(phrase);
                //cell.Colspan = 1;
                //cell.Border = 1;
                //cell.PaddingTop = 10f;
                //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                ////cell.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                //headertable1.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Days Since Last Insp.", Heading781));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingTop = 10f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable1.AddCell(cell);



                phrase = new Phrase();
                phrase.Add(new Chunk("Tonnage", Heading781));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingTop = 10f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable1.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Design Type", Heading781));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingTop = 10f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Material Type", Heading781));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingTop = 10f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Standing Days to Date", Heading781));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingTop = 10f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable1.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Length", Heading781));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingTop = 10f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable1.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Width", Heading781));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingTop = 10f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable1.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Height (TWP)", Heading781));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingTop = 10f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable1.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Status", Heading781));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingTop = 10f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable1.AddCell(cell);


                DataTable dt = databaseCon.GetData(Query);
                if (dt.Rows.Count > 0)
                {

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        phrase = new Phrase();
                        //phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["ScaffRef"].ToString()), DataFont));
                        phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Scaff Ref #"].ToString()), DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.Border = 1;
                        cell.Colspan = 1;
                        cell.PaddingTop = 2f;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable1.AddCell(cell);

                        phrase = new Phrase();
                        //phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["WONumber"].ToString()), DataFont));
                        phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["WO #"].ToString()), DataFont));
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
                        // phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["BuildingNo"].ToString()), DataFont));
                        phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Building / Module No"].ToString()), DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = 1;
                        cell.Colspan = 1;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable1.AddCell(cell);


                        phrase = new Phrase();
                        //phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["FloorLevel"].ToString()), DataFont));
                        phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Floor Level"].ToString()), DataFont));
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
                        //phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Description"].ToString()), DataFont));
                        phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Reason For Access"].ToString()), DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = 1;
                        cell.Colspan = 1;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable1.AddCell(cell);


                        phrase = new Phrase();
                        //phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["AssetNo"].ToString()), DataFont));
                        phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Asset # / Func. Loc"].ToString()), DataFont));

                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = 1;
                        cell.Colspan = 1;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable1.AddCell(cell);


                        phrase = new Phrase();
                        // string dateonly = String.Format("{0:dd-MMM-yyyy}", dt.Rows[i]["DateTagged"]);
                        phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Date Built"].ToString()), DataFont));
                        //phrase.Add(new Chunk(dateonly, DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.Border = 1;
                        cell.Colspan = 1;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable1.AddCell(cell);

                        phrase = new Phrase();
                        //string dateonlyDateOfDismantle = String.Format("{0:dd-MMM-yyyy}", dt.Rows[i]["DateOfDismantle"]);
                        //phrase.Add(new Chunk(dateonlyDateOfDismantle, DataFont));
                        phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Date OF Dismantle"].ToString()), DataFont));

                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.Border = 1;
                        cell.Colspan = 1;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable1.AddCell(cell);

                        //phrase = new Phrase();
                        //string dateonlyRecentInspDate = String.Format("{0:dd-MMM-yyyy}", dt.Rows[i]["RecentInspDate"]);
                        //phrase.Add(new Chunk(dateonlyRecentInspDate, DataFont));
                        //cell = new PdfPCell(phrase);
                        //cell.PaddingBottom = 15f;
                        ////cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        //cell.Border = 1;
                        //cell.Colspan = 1;
                        //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        //headertable1.AddCell(cell);


                        phrase = new Phrase();
                        //  phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["DaysSinceLastInsp"].ToString()), DataFont));
                        phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Days Since Last Insp"].ToString()), DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.Border = 1;
                        cell.Colspan = 1;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable1.AddCell(cell);


                        phrase = new Phrase();
                        phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Tonnage"].ToString()), DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.Border = 1;
                        cell.Colspan = 1;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable1.AddCell(cell);


                        phrase = new Phrase();
                        // phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["DesignType"].ToString()), DataFont));
                        phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Design Type"].ToString()), DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = 1;
                        cell.Colspan = 1;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable1.AddCell(cell);


                        phrase = new Phrase();
                        // phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["MaterialType"].ToString()), DataFont));
                        phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Material Type"].ToString()), DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = 1;
                        cell.Colspan = 1;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable1.AddCell(cell);


                        phrase = new Phrase();
                        //phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["HireDaysToDate"].ToString()), DataFont));
                        phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Hire Days To Date"].ToString()), DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.Border = 1;
                        cell.Colspan = 1;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable1.AddCell(cell);

                        phrase = new Phrase();
                        phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Length"].ToString()), DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.Border = 1;
                        cell.Colspan = 1;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable1.AddCell(cell);

                        phrase = new Phrase();
                        phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Width"].ToString()), DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.Border = 1;
                        cell.Colspan = 1;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable1.AddCell(cell);

                        phrase = new Phrase();
                        // phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Height"].ToString()), DataFont));
                        phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Height (TWP)"].ToString()), DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
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
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
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
            }

            docs.Close();


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
