using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Scaffold.App_Code;
using ClosedXML.Excel;
using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;
//using static Scaffold.Controllers.OfflineRegisterController;

namespace Scaffold.Controllers
{
    public class SiteInventoryInvoiceSurgeCostController : Controller
    {
        double TotalInvoiceValues = 0;
        double TotalCalculatedValues = 0;
        //float Totalquantity = 0;
        //float WeightTotal = 0;
        // GET: SiteInventoryReport
        public ActionResult Index()
        {
           return View();
        }
        public void SiteInventoryInvoiceSurgeCostAllocationReport(string UID, string UserRole, string ClientId, string SiteId, string rdpReportStartDate, string rdpReportEndDate, string SummaryDetails, string materialType, string RID)
        {
            int id = Convert.ToInt32(databaseCon.Decrypt(UID));
            int SiteID = Convert.ToInt32(databaseCon.Decrypt(SiteId));
            int rID = Convert.ToInt32(databaseCon.Decrypt(RID));
            int ClientID = Convert.ToInt32(databaseCon.Decrypt(ClientId));
            string StartDate = databaseCon.Decrypt(rdpReportStartDate);
            string EndDate = databaseCon.Decrypt(rdpReportEndDate);
            string URole = databaseCon.Decrypt(UserRole);
            string MaterialType = databaseCon.Decrypt(materialType);
            DataTable dt;
            String execquery = "";
            //string query = "select * from [GenerateInvoiceCostReport] where SiteLocId='" + SiteID + "'and [ReportStartDate] ='" + StartDate + "'and [ReportEndDate]= '" + EndDate + "' ";
            string query1 = "select count('x') as cnt from [TblGenerateInvoiceCostReport] where SiteLocId='" + SiteID + "'and  [ActualStartDate] ='" + StartDate + "'and [ActualEndDate]= '" + EndDate + "' ";
            DataTable dt0 = databaseCon.GetData(query1, true, 3200);

            var rwcount = Convert.ToInt32(dt0.Rows[0]["cnt"].ToString());
            if (rwcount != 0)
            {
                //string execquery = " exec [Proc_InvoiceCostReportSurgedate] '" + StartDate + "' , '" + EndDate + "',  " + ClientID + " ," + SiteID + " ,'InvoicedPDF' ";//changed by pooja 12dec2022
                execquery = " exec [Proc_InvoiceCostReportSurgedate_DownloadReports] '" + StartDate + "' , '" + EndDate + "',  " + ClientID + " ," + SiteID + " ,'InvoicedPDF'," + rID + " ,'" + MaterialType + "' ";
                dt = databaseCon.GetData(execquery, true, 3200);
                //ViewData["Success"] = "Data Generated successfully.";


                iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.LETTER, -110f, -110f, 50f, 50f);
                doc.SetPageSize(new Rectangle(990, 600));

                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, Response.OutputStream);
                Response.ContentType = ("application/pdf");
                //Response.AddHeader("Content-Disposition", "attachment; filename=SiteInventoryInvoiceCostAllocationReport.pdf");
                Response.AddHeader("Content-Disposition", " filename=SiteInventoryInvoiceCostAllocationReport.pdf");
                HeaderFooter2 evnt = new HeaderFooter2();
                writer.PageEvent = evnt;
                evnt.ID = id;
                evnt.SiteID = SiteID;
                evnt.ClientID = ClientID;
                evnt.StartDate = StartDate;
                evnt.EndDate = EndDate;

                doc.Open();
                var Heading786 = FontFactory.GetFont("Arial", 10f, Font.BOLD, BaseColor.BLACK);
                //Heading786.Color = Color.GRAY;
                var Heading787 = FontFactory.GetFont("Arial", 25f, Font.BOLD, BaseColor.BLACK);
                var Heading788 = FontFactory.GetFont("Arial", 30f, Font.BOLD, BaseColor.BLACK);
                var DataFont = FontFactory.GetFont("Arial", 10f, Font.NORMAL, BaseColor.BLACK);
                Phrase phrase;
                PdfPCell cell;

                PdfPTable headertable1 = new PdfPTable(5);
                headertable1.SetWidths(new int[] { 10, 9, 25, 9, 9 });

                //Table Start

                PdfPTable headertable2 = new PdfPTable(15);
                headertable2.SetWidths(new int[] { 9, 8, 10, 9, 8, 8, 9, 10, 12, 10, 10, 8, 8, 8, 9 });

                string queryheader = "select ClientImage from tblClient where ClientId= '" + ClientID + "'";
                DataTable dtheader = databaseCon.GetData(queryheader);

                //start
                if (!string.IsNullOrEmpty(Convert.ToString(dtheader.Rows[0]["ClientImage"])))
                {
                    var uri = new Uri(databaseCon.clientimageLink);
                    var noimage = System.Web.HttpContext.Current.Server.MapPath("~/Images/Logo2.png");
                    if (Convert.ToString(dtheader.Rows[0]["ClientImage"]) == "")
                    {
                        string nophoto = System.Web.HttpContext.Current.Server.MapPath("~/images/Logo2.png");
                        cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
                        cell.FixedHeight = 50f;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Colspan = 2;
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
                        cell.PaddingTop = -10f;

                    }
                }
                else
                {
                    string nophoto = System.Web.HttpContext.Current.Server.MapPath("~/images/Logo2.png");
                    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
                    cell.FixedHeight = 50f;
                    cell.PaddingTop = -10f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 2;

                }
                cell.Border = 0;
                cell.Colspan = 2;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell.PaddingLeft = 20f;
                cell.FixedHeight = 50f;
                cell.PaddingTop = -10f;
                headertable2.AddCell(cell);


                phrase = new Phrase();
                phrase.Add(new Chunk("Invoice Surge Cost Allocation", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 7;
                //cell.PaddingBottom = 6f;
                //cell.PaddingLeft = 100f;
                cell.PaddingTop = -10f;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 5;
                //cell.PaddingBottom = 6f;
                //cell.PaddingLeft = 100f;
                cell.PaddingTop = -10f;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                headertable2.AddCell(cell);


                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 15;
                cell.PaddingBottom = 6f;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable2.AddCell(cell);


                phrase.Add(new Chunk("", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 2;
                cell.PaddingBottom = 6f;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable2.AddCell(cell);

                // add code on 23/March/2020.
                phrase = new Phrase();
                phrase.Add(new Chunk("Invoice Value:", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 2;
                cell.PaddingBottom = 6f;
                cell.Border = 0;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                if (dt.Rows.Count > 0)
                {
                    phrase = new Phrase();
                    //phrase.Add(new Chunk(string.Format("${0:#,##0.0000}",dt.Rows[0]["TotalInvoicedHireCost"]), DataFont));
                    //phrase.Add(new Chunk(string.Format("${0:#,##0.00}", dt.Rows[0]["TotalInvoicedHireCost"]), DataFont));  // Changed code on 24/March/2020.
                    phrase.Add(new Chunk(string.Format("${0:#,##0.00}", dt.Rows[0]["TotalCalculatedHireCost"]), DataFont));  // Changed code on 23/April/2020.
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 2;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                }


                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.PaddingBottom = 6f;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Report Start Date :", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 2;
                cell.PaddingBottom = 6f;
                cell.Border = 0;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk(string.Format("{0:dd-MM-yyyy}", StartDate), DataFont));
                cell = new PdfPCell(phrase);
                cell.PaddingBottom = 15f;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 1;
                cell.Colspan = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.PaddingBottom = 6f;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Report End Date :", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 2;
                cell.PaddingBottom = 6f;
                cell.Border = 0;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk(string.Format("{0:dd-MMM-yyyy}", EndDate), DataFont));
                cell = new PdfPCell(phrase);
                cell.PaddingBottom = 15f;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 1;
                cell.Colspan = 2;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk(" ", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 15;
                cell.PaddingBottom = 0f;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Site", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingBottom = 6f;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;

                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Area", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.PaddingBottom = 6f;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Equipment", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("WO Number", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Daily Hire Rate", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                //phrase = new Phrase();
                //phrase.Add(new Chunk("$ Invoiced", Heading786));
                //cell = new PdfPCell(phrase);
                //cell.Colspan = 1;
                //cell.Border = 1;
                //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                //headertable2.AddCell(cell);


                phrase = new Phrase();
                // phrase.Add(new Chunk("$ Calculated", Heading786));
                phrase.Add(new Chunk("$ Invoiced", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Requested By", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Activity", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                //Commented on 14-09-2023 Renamed as Date Built
                //phrase.Add(new Chunk("Date Tagged", Heading786));
                phrase.Add(new Chunk("Date Built", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                //Add Code on 22/04/2020.


                phrase = new Phrase();
                phrase.Add(new Chunk("Date Of Dism.", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Hire Days", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Total Days", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);


                phrase = new Phrase();
                phrase.Add(new Chunk("Comments", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Scaf Ref #", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);


                phrase = new Phrase();
                phrase.Add(new Chunk("Client Ref #", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                //end

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
                        headertable2.AddCell(cell);

                        phrase = new Phrase();
                        phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Area"].ToString()), DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = 1;
                        cell.Colspan = 1;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable2.AddCell(cell);

                        phrase = new Phrase();
                        phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Equipment"].ToString()), DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = 1;
                        cell.Colspan = 1;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable2.AddCell(cell);

                        phrase = new Phrase();
                        phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["WONumber"].ToString()), DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = 1;
                        cell.Colspan = 1;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable2.AddCell(cell);

                        phrase = new Phrase();
                        //string DaysCalcqty = String.Format("${0:#,##0.0000}", dt.Rows[i]["DailyHireCost"]);
                        string DaysCalcqty = String.Format("${0:#,##0.00}", dt.Rows[i]["DailyHireCost"]);//Changed On 24/March/2020.
                        phrase.Add(new Chunk(DaysCalcqty, DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = 1;
                        cell.Colspan = 1;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable2.AddCell(cell);

                        //phrase = new Phrase();
                        ////string qty = String.Format("${0:#,##0.0000}", dt.Rows[i]["InvoicedHireCost"]);
                        //string qty = String.Format("${0:#,##0.00}", dt.Rows[i]["InvoicedHireCost"]);//Changed On 24/March/2020.
                        //phrase.Add(new Chunk(qty, DataFont));
                        //cell = new PdfPCell(phrase);
                        //cell.PaddingBottom = 15f;
                        //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        //cell.Border = 1;
                        //cell.Colspan = 1;
                        //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        //headertable2.AddCell(cell);

                        phrase = new Phrase();
                        //string Deliveryqty = String.Format("${0:#,##0.0000}", dt.Rows[i]["CalculatedHireCost"]);
                        string Deliveryqty = String.Format("${0:#,##0.00}", dt.Rows[i]["CalculatedHireCost"]);//Changed On 24/March/2020.
                        phrase.Add(new Chunk(Deliveryqty, DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = 1;
                        cell.Colspan = 1;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable2.AddCell(cell);


                        phrase = new Phrase();
                        phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["RequestedBy"].ToString()), DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = 1;
                        cell.Colspan = 1;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable2.AddCell(cell);

                        phrase = new Phrase();
                        phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Activity"].ToString()), DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = 1;
                        cell.Colspan = 1;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable2.AddCell(cell);

                        phrase = new Phrase();
                        //phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["DateTagged"].ToString()), DataFont));
                        phrase.Add(new Chunk(dt.Rows[i]["DateTagged"] == DBNull.Value ? "" : Convert.ToDateTime(dt.Rows[i]["DateTagged"]).ToString("dd-MMM-yy"), DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = 1;
                        cell.Colspan = 1;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable2.AddCell(cell);



                        phrase = new Phrase();
                        //phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["DateOfDismantle"].ToString()), DataFont));
                        phrase.Add(new Chunk(dt.Rows[i]["DateOfDismantle"] == DBNull.Value ? "" : Convert.ToDateTime(dt.Rows[i]["DateOfDismantle"]).ToString("dd-MMM-yy"), DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = 1;
                        cell.Colspan = 1;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable2.AddCell(cell);

                        phrase = new Phrase();
                        phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["DaysCalc"].ToString()), DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = 1;
                        cell.Colspan = 1;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable2.AddCell(cell);

                        phrase = new Phrase();
                        phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["TotalDays"].ToString()), DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = 1;
                        cell.Colspan = 1;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable2.AddCell(cell);

                        phrase = new Phrase();
                        phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Comments"].ToString()), DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = 1;
                        cell.Colspan = 1;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable2.AddCell(cell);

                        phrase = new Phrase();
                        phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["ScaffRef"].ToString()), DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = 1;
                        cell.Colspan = 1;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable2.AddCell(cell);

                        phrase = new Phrase();
                        phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["ClientRef"].ToString()), DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = 1;
                        cell.Colspan = 1;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable2.AddCell(cell);


                        //if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["InvoicedHireCost"])))
                        //{
                        //    TotalInvoiceValues += Convert.ToDouble(dt.Rows[i]["InvoicedHireCost"]);
                        //}

                        if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["CalculatedHireCost"])))
                        {
                            TotalCalculatedValues += Convert.ToDouble(dt.Rows[i]["CalculatedHireCost"]);
                        }


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
                    phrase.Add(new Chunk("Total:", Heading786));
                    cell = new PdfPCell(phrase);
                    cell.Colspan = 1;
                    cell.Border = 3;
                    cell.PaddingBottom = 6F;
                    cell.Border = PdfPCell.RIGHT_BORDER;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    headertable2.AddCell(cell);

                    //phrase = new Phrase();
                    //// phrase.Add(new Chunk(string.Format("${0:#,##0.0000}", TotalInvoiceValues), DataFont));
                    //phrase.Add(new Chunk(string.Format("${0:#,##0.00}", TotalInvoiceValues), DataFont));//Changed On 24/March/2020.
                    //cell = new PdfPCell(phrase);
                    //cell.Colspan = 1;
                    //cell.Border = 3;
                    //cell.PaddingBottom = 6F;
                    //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    //headertable2.AddCell(cell);

                    phrase = new Phrase();
                    // phrase.Add(new Chunk(string.Format("${0:#,##0.0000}", TotalCalculatedValues), DataFont));
                    phrase.Add(new Chunk(string.Format("${0:#,##0.00}", TotalCalculatedValues), DataFont));//Changed On 24/March/2020.
                    cell = new PdfPCell(phrase);
                    cell.Colspan = 1;
                    cell.Border = 3;
                    cell.PaddingBottom = 6F;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    headertable2.AddCell(cell);
                }
                else
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(" ", Heading788));
                    cell = new PdfPCell(phrase);
                    cell.FixedHeight = 15;
                    cell.Colspan = 15;
                    cell.Border = 3;
                    cell.PaddingBottom = 6F;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    headertable2.AddCell(cell);

                    phrase = new Phrase();
                    phrase.Add(new Chunk("No Data Found For The Given Date Range", Heading788));
                    cell = new PdfPCell(phrase);
                    cell.Colspan = 15;
                    cell.FixedHeight = 50;
                    cell.Border = 3;
                    cell.PaddingBottom = 6F;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    headertable2.AddCell(cell);

                }



                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 9;
                cell.PaddingBottom = 6F;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                headertable2.AddCell(cell);

                doc.Add(headertable2);
                doc.Close();

            }



            else
            {
                // ViewData["Failed"] = "Data Already Generated For The Given Time Period.";
            }

        }

        public class HeaderFooter2 : iTextSharp.text.pdf.PdfPageEventHelper
        {
            public int ID { get; set; }
            public int SiteID { get; set; }
            public int ClientID = 0;
            public string StartDate { get; set; }
            public string EndDate { get; set; }

            List<PdfTemplate> pageNumberTemplates = new List<PdfTemplate>();// page header

            public override void OnStartPage(PdfWriter writer, Document doc)
            {
                //String summary = "S";


                //string query = " exec Proc_InvoiceCostReportSurgedate '" + StartDate + "' , '" + EndDate + "',  " + ClientID + " ," + SiteID + " ,'" + summary + "' ";

                //DataTable dt = databaseCon.GetData(query);
                //DateTime ReportStartDate = Convert.ToDateTime(databaseCon.Decrypt(HttpContext.Current.Request.QueryString["ReportStartDate"]));
                //DateTime ReportEndDate = Convert.ToDateTime(databaseCon.Decrypt(HttpContext.Current.Request.QueryString["ReportEndDate"]));
                base.OnStartPage(writer, doc);



                string ImagePath = System.Web.HttpContext.Current.Server.MapPath("~/Images/logo2.png");// image size
                Rectangle pageSize = doc.PageSize;

                PdfPTable headertable2 = new PdfPTable(15);
                headertable2.SetWidths(new int[] { 9, 8, 10, 9, 8, 8, 9, 10, 12, 10, 10, 8, 8, 8, 9 });

                var contentfont = FontFactory.GetFont("Arial", 8f, BaseColor.BLACK);
                var Headings = FontFactory.GetFont("Arial", 9f, Font.BOLD, BaseColor.BLACK);
                var Head = FontFactory.GetFont("Arial", 11f, Font.BOLD, BaseColor.BLACK);
                var BackOrderheader = FontFactory.GetFont("Arial", 20f, Font.BOLD, BaseColor.BLACK);
                var Heading787 = FontFactory.GetFont("Arial", 20f, Font.BOLD, BaseColor.BLACK);
                var Heading786 = FontFactory.GetFont("Arial", 10f, Font.BOLD, BaseColor.BLACK);
                var DataFont = FontFactory.GetFont("Arial", 10f, Font.NORMAL, BaseColor.BLACK);

               

                doc.Add(headertable2);

            }
            public override void OnEndPage(PdfWriter writer, Document document)
            {

                Paragraph footerdate = new Paragraph(("Printed: " + DateTime.Now.ToString("dd-MMM-yyy")), FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
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
        // InvoiceReport Site Stock Report Btn Export To CSV Click Code 



        
        public ActionResult btnInvoiceSurgeReportExportToCsv_Click(string UID, string UserRole, string ClientId, string SiteId, string ReportStartDate, string ReportEndDate, string SummaryDetails , string MaterialType ,string RID )
        {
            string path = @"~/uploads/";
            path = Server.MapPath(path);
            string fpath = path + "InvoiceCostReport.csv";
            StreamWriter sw = new StreamWriter(fpath, false);
            //Added on (14-12-2022)
            RID = "0";
            int rID = Convert.ToInt32(databaseCon.Decrypt(RID));
            int clientId = Convert.ToInt32(databaseCon.Decrypt(ClientId));
            int siteId = Convert.ToInt32(SiteId);
            DataTable dt;
            string StrDate = ReportStartDate;
            string EndDate = ReportEndDate;
            var result = "";
            var message = "";
            
            //if (SummaryDetails == "s")
            //{
            //if (ddlSite.SelectedValue != "")
            if (SiteId != "")
            {
                //  if ((rdpReportStartDate.SelectedDate != null) && (rdpReportEndDate.SelectedDate != null))
                if ((StrDate != null) && (EndDate != null))
                {
                    //string query = "select * from [GenerateInvoiceCostReport] where SiteLocId='" + SiteId + "'and  [ReportStartDate] ='" + ReportStartDate + "'and [ReportEndDate]= '" + ReportEndDate + "' ";
                    string query = "select  count('x') as cnt from [TblGenerateInvoiceCostReport] where SiteLocId='" + SiteId + "'and  [ActualStartDate] ='" + ReportStartDate + "'and [ActualEndDate]= '" + ReportEndDate + "' ";
                    DataTable dt0 = databaseCon.GetData(query, true, 3200);
                   

                    var rwcount = Convert.ToInt32(dt0.Rows[0]["cnt"].ToString());
                    if (rwcount != 0)
                    {
                        //string execquery = " exec [Proc_InvoiceCostReportSurgedate] '" + ReportStartDate + "' , '" + ReportEndDate + "' ,  " + clientId + " ," + siteId + " ,'" + SummaryDetails + "';changed by pooja 12dec2022
                        string execquery = " exec [Proc_InvoiceCostReportSurgedate_DownloadReports] '" + ReportStartDate + "' , '" + ReportEndDate + "' ,  " + clientId + " ," + siteId + " ,'" + SummaryDetails + "'," + rID + " ,'" + MaterialType + "' ";


                        //code for timeout start
                         dt = databaseCon.GetData(execquery, true, 3200);
                    }
                    else { 
                       return Json(new { result= "failed", message="Data Not Generated !!! Kindly click on Generate Button." }, JsonRequestBehavior.AllowGet);
                        //return Json("Data Not Generated !!! Kindly click on Generate Button.", JsonRequestBehavior.AllowGet);

                    }
                    //Response.ClearContent();
                    //Response.AddHeader("content-disposition", "attachment;filename=MyCsvFile.csv");
                    //Response.ContentType = "application/text";

                    //var csvFile = new StringBuilder();
                    //csvFile.AppendLine("Site,Area,Equipment,[WO Number],[Daily Hire Rate],$Invoiced,[Requested By],Activity,[Date Tagged],[Date Of Dismantle],[Hire Days],[Total Days],Comments,[Scaf Ref #],[Client Ref #]");

                    int iColCount = dt.Columns.Count;
                    for (int i = 0; i < iColCount; i++)
                    {

                        sw.Write(dt.Columns[i].ToString());
                        //var Tempcolumn = dt.Columns[i].ToString();
                        if (i < iColCount - 1)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.Write(sw.NewLine);

                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    for (int i = 0; i < iColCount; i++)
                    //    {
                    //        if (!Convert.IsDBNull(dr[i]))
                    //        {
                    //          //  var Tempcolumn1 = dr[i].ToString();
                    //            sw.Write("\""+dr[i].ToString().Replace("\n", " ")+"\"");
                    //        }
                    //        if (i < iColCount - 1)

                    //        {
                    //            sw.Write(",");
                    //        }
                    //    }
                    //    sw.Write(sw.NewLine);
                    //}
                    foreach (DataRow dr in dt.Rows)
                    {
                        for (int i = 0; i < iColCount; i++)
                        {
                            if (!Convert.IsDBNull(dr[i]))
                            {
                                string value = dr[i].ToString();

                                // Remove new lines safely
                                value = value.Replace("\r", " ").Replace("\n", " ");

                                // Escape double quotes (VERY IMPORTANT)
                                value = value.Replace("\"", "\"\"");

                                // Wrap every field in quotes (safest approach)
                                sw.Write("\"" + value + "\"");
                            }

                            if (i < iColCount - 1)
                            {
                                sw.Write(",");
                            }
                        }

                        sw.Write(sw.NewLine);
                    }

                    sw.Close();

                    //Response.Write(csvFile.ToString());
                    //Response.Flush();
                    //Response.End();

                }
                else
                {
                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong> Please Select Report Start Date and Report End Date.";
                }
            }
            else
            {
                //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                //art1.Style.Add("display", "block");
                //alertlabel.Text = "<strong>Alert ! </strong> Please Select the Site.";
            }
            if (SummaryDetails == "S")
            {
              return  File(fpath, "application/octet-stream", "InvoiceSummaryCostReport.csv");

            }
            else
            {
                return File(fpath, "application/octet-stream", "InvoiceDetailedCostReport.csv");
            }

        }


        //}

        //created by pooja 12dec2022
        public string btnGenerateReport_Click(string UID, string UserRole, string ClientId, string SiteId, string rdpReportStartDate, string rdpReportEndDate, string MaterialType)
        {
            //var message = "success";
            var Message = "";
            var Result = "";
            try
            {
                //int id = Convert.ToInt32(databaseCon.Decrypt(UID));
                int SiteID = Convert.ToInt32(SiteId);
                int ClientID = Convert.ToInt32(databaseCon.Decrypt(ClientId));
                //string StartDate = databaseCon.Decrypt(rdpReportStartDate);
                //string EndDate = databaseCon.Decrypt(rdpReportEndDate);
                string StartDate = rdpReportStartDate;
                string EndDate = rdpReportEndDate;
                DateTime DateStartDate = Convert.ToDateTime(StartDate);
                DateTime DateEndDate = Convert.ToDateTime(EndDate);
                DateTime Datecurrdate = Convert.ToDateTime(StartDate);
                //Baans
                //DateTime Dateenddate = Convert.ToDateTime(EndDate);
                string execquery = "";
                //string URole = databaseCon.Decrypt(UserRole);
                if (SiteId != "")
                {
                    //  if ((rdpReportStartDate.SelectedDate != null) && (rdpReportEndDate.SelectedDate != null))
                    if ((StartDate != null) && (EndDate != null))
                    {

                        string query1 = "select count('x') as cnt from [TblGenerateInvoiceCostReport] where SiteLocId='" + SiteId + "'and  [ActualStartDate] ='" + DateStartDate + "'and [ActualEndDate]= '" + DateEndDate + "' ";
                        DataTable dt0 = databaseCon.GetData(query1, true, 1600);

                        var rwcount = Convert.ToInt32(dt0.Rows[0]["cnt"].ToString());
                        if (rwcount != 0)
                        {
                            string querydelete = "Delete from [TblGenerateInvoiceCostReport] where SiteLocId='" + SiteId + "'and  [ActualStartDate] ='" + DateStartDate + "'and [ActualEndDate]= '" + DateEndDate + "' ";
                            databaseCon.RunCmd(querydelete);
                            string querysitedatadelete = "Delete from [TblGenerateInvoiceCostReportSiteData] where SiteId='" + SiteId + "'and ActualSiteStartDate= '" + DateStartDate + "'and ActualSiteEndDate= '" + DateEndDate + "' ";
                            databaseCon.RunCmd(querysitedatadelete);
                        }
                        while (Datecurrdate <= DateEndDate )
                        {
                            //string execquery = " exec Proc_InvoiceCostReportSurgedate_GenerateReport '" + StartDate + "' , '" + EndDate + "',  " + ClientID + " ," + SiteID + "  ";
                            if (Datecurrdate.AddDays(9) > DateEndDate)
                            {
                                execquery = " exec Proc_InvoiceCostReportSurgedate_GenerateReport '" + Datecurrdate + "' , '" + DateEndDate + "',  " + ClientID + " ," + SiteID + ",'Generate'," + 0 + "," + "'All'" + ",'" + DateStartDate + "','" + DateEndDate + "'";
                                //databaseCon.RunCmd(execquery);
                                Datecurrdate = DateEndDate;
                                Datecurrdate = Datecurrdate.AddDays(1);
                            }
                            else
                            {
                                execquery = " exec Proc_InvoiceCostReportSurgedate_GenerateReport '" + Datecurrdate + "' , '" + Datecurrdate.AddDays(9) + "',  " + ClientID + " ," + SiteID + ",'Generate'," + 0 + "," + "'All'" + ",'" + DateStartDate + "','" + DateEndDate + "'";
                                //databaseCon.RunCmd(execquery);
                                Datecurrdate = Datecurrdate.AddDays(10);
                            }
                            databaseCon.RunCmd(execquery);
                        }

                        execquery = " select count('x') from tblGenerateInvoiceCostReport where ActualStartDate = '" + DateStartDate + "' and ActualEndDate='" + DateEndDate + "' and SiteLocId='" + SiteId + "'";

                        DataTable dt = databaseCon.GetData(execquery, true, 1600);

                        if (dt.Rows.Count != 0)
                        {
                            if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                            {

                                Result = "success";
                                //Message = dt.Rows[0][1].ToString();
                                Message = "Report Generated Successfully";

                            }
                            else
                            {

                                Result = "Failed";
                                Message = "Report Generation Failed";
                            }
                        }
                        return Result;
                    }

                }
                return Result;
            }
            catch (Exception ex)
            {

                //message = ex.Message;
                return Result;
            }
        }
        
            private string csvfile(DataTable dtTable)
        {
            StringBuilder sbldr = new StringBuilder();
            if (dtTable.Columns.Count != 0)
            {
                foreach (DataColumn col in dtTable.Columns)
                {
                    sbldr.Append(col.ColumnName + ',');
                }
                sbldr.Append("\r\n");
                foreach (DataRow row in dtTable.Rows)
                {
                    foreach (DataColumn column in dtTable.Columns)
                    {
                        if (row[column].ToString().Contains(','))
                        {
                            sbldr.Append("\"" + row[column].ToString() + "\"" + ',');
                        }
                        //Add code on 17/April/2020
                        else if (row[column].ToString().Contains('\n'))
                        {
                            sbldr.Append(row[column].ToString().Replace("\n", " ") + ',');
                        }
                        else
                        {
                            sbldr.Append(row[column].ToString() + ',');
                        }
                        //sbldr.Append(row[column].ToString().Replace(",", ";") + ',');
                    }
                    sbldr.Append("\r\n");
                }

            }
            return sbldr.ToString();
        }
        public ActionResult downloadforcsv(String SummaryDetails)
        {

            string path = @"~/uploads/";
            path = Server.MapPath(path);
            string fpath = path + "InvoiceCostReport.csv";
            if (SummaryDetails == "S")
            {
                
                return File(fpath, "text/csv", "InvoiceSummaryCostReport.csv");
            }
            else
            {
                return File(fpath, "text/csv", "InvoiceDetailedCostReport.csv");
            }
            

        }

    }
}