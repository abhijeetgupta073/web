using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Scaffold.App_Code;

namespace Scaffold.Controllers
{
    public class UploadSAPReportController : Controller
    {
        double TotalSAPValues = 0;
        // GET: UploadSAPReport
        public ActionResult Index()
        {
            return View();
        }
        public void showreport(string id, string SiteID, string ReportStartDate1, string ReportEndDate1, string SiteName ,string RID, string materialType)
        {


            int Clientid = Convert.ToInt32(databaseCon.Decrypt(id));
            int SiteId = Convert.ToInt32(databaseCon.Decrypt(SiteID));
            DateTime ReportStartDate = Convert.ToDateTime(databaseCon.Decrypt(ReportStartDate1));
            DateTime ReportEndDate = Convert.ToDateTime(databaseCon.Decrypt(ReportEndDate1));
            string Date1 = ReportStartDate.ToString("MMM-yyyy").Replace("-", " ");
            string MaterialType = databaseCon.Decrypt(materialType);

            //string query = "Declare @ReportStartDate datetime, @ReportEndDate Datetime Set  @ReportStartDate='" + databaseCon.Decrypt(Request.QueryString["ReportStartDate1"]) + "' Set @ReportEndDate='" + databaseCon.Decrypt(Request.QueryString["ReportEndDate1"]) + "' Select ClientId, RegisterID, SiteLocation as [Site], 'WACO' As TaxInvoiceNo,'OLE Scaffold' as Desc1,'101378' as VendorNO,'G1' as TaxCode,Convert(varchar,(Convert(Decimal(18, 2),(Convert(Decimal(18, 2),Convert(Decimal(18, 2), Sum(Isnull(DailyHireCost, 0))) * Max(DaysCalc))*10)/100) + Convert(Decimal(18, 2),Convert(Decimal(18, 2), Sum(Isnull(DailyHireCost, 0))) * Max(DaysCalc)))) as 'Amount(incl of GST)','' As Desc2,'Scaffold hire' As Desc3, '' as 'Cost Centre',WONumber as 'Work Order','' as WBS, Case when WONumber like '39%'  then '30800001' else '5051016' end  AS 'Expense A/C',Convert(Decimal(18, 2),Convert(Decimal(18, 2), Sum(Isnull(DailyHireCost, 0))) * Max(DaysCalc)) as '$Invoiced' from ( Select Vw_AllJobInventoryWithRate.RegisterId,InventoryId,Vw_AllJobInventoryWithRate.MaterialType,ItemCode,ItemDesc,Quantity,ItemWeight,TotalWeight,subCategoryId,SubCategory,HireMeasurement,EnteredHireRate,DailyHireRate,WeeklyHireRate , ClientId, vw_tblRegister.SiteLocId,vw_tblRegister.SiteLocation,Location, AssetNo, WONumber, ClientRef , [Description] , Tonnage, DateTagged, DateOfDismantle,Comments, ScaffRef, ClientScaffRef, DateDiff(d,(Case When DateTagged>@ReportStartDate then Datetagged else @ReportStartDate End),(Case When DateOfDismantle<@ReportEndDate then DateOfDismantle else @ReportEndDate End))+1 as DaysCalc ,Case When DateTagged IS not null then DateDiff(d,Datetagged,(Case When DateOfDismantle<@ReportEndDate then DateOfDismantle else @ReportEndDate End))+1 Else 0 end as TotalDays ,Case when Isnull(DailyHireRate,0)>0 then Quantity * Isnull(DailyHireRate,0) Else NULL End as DailyHireCost from Vw_AllJobInventoryWithRate join vw_tblRegister on vw_tblRegister.RegisterId=Vw_AllJobInventoryWithRate.RegisterId where clientId='" + id + "' and(Vw_tblRegister.SiteLocId= '" + SiteID + "' or '0'='" + SiteID + "')and ( (Isnull(DateTagged,@ReportStartDate)<=@ReportEndDate) and Isnull(DateOfDismantle,@ReportEndDate)>=@ReportStartDate)) AllRegisterWithHireData group by AllRegisterWithHireData.RegisterId, ClientId, SiteLocation, Location, AssetNo,WONumber, ClientRef,[Description],DateTagged, DateOfDismantle, Comments, ScaffRef, ClientScaffRef ";

            // Changed Query on 26-May-2020
            //string query = "Declare @ReportStartDate datetime, @ReportEndDate Datetime Set  @ReportStartDate='" + databaseCon.Decrypt(ReportStartDate1) + "' Set @ReportEndDate='" + databaseCon.Decrypt(ReportEndDate1) + "' Select ClientId, RegisterID, SiteLocation as [Site], 'WACO' As TaxInvoiceNo,upper(left(sitelocation,3))+' Scaffold' as Desc1,'101378' as VendorNO,'G1' as TaxCode,Convert(varchar,(Convert(Decimal(18, 2),(Convert(Decimal(18, 2),Convert(Decimal(18, 2), Sum(Isnull(DailyHireCost, 0))) * Max(DaysCalc))*10)/100) + Convert(Decimal(18, 2),Convert(Decimal(18, 2), Sum(Isnull(DailyHireCost, 0))) * Max(DaysCalc)))) as 'Amount(incl of GST)','' As Desc2,'Scaffold hire' As Desc3, '' as 'Cost Centre',WONumber as 'Work Order','' as WBS, Case when WONumber like '39%'  then '30800001' else '5051016' end  AS 'Expense A/C',Convert(Decimal(18, 2),Convert(Decimal(18, 2), Sum(Isnull(DailyHireCost, 0))) * Max(DaysCalc)) as '$Invoiced' from ( Select Vw_AllJobInventoryWithRate.RegisterId,InventoryId,Vw_AllJobInventoryWithRate.MaterialType,ItemCode,ItemDesc,Quantity,ItemWeight,TotalWeight,subCategoryId,SubCategory,HireMeasurement,EnteredHireRate,DailyHireRate,WeeklyHireRate , ClientId, vw_tblRegister.SiteLocId,vw_tblRegister.SiteLocation,Location, AssetNo, WONumber, ClientRef , [Description] , Tonnage, DateTagged, DateOfDismantle,Comments, ScaffRef, ClientScaffRef, DateDiff(d,(Case When DateTagged>@ReportStartDate then Datetagged else @ReportStartDate End),(Case When DateOfDismantle<@ReportEndDate then DateOfDismantle else @ReportEndDate End))+1 as DaysCalc ,Case When DateTagged IS not null then DateDiff(d,Datetagged,(Case When DateOfDismantle<@ReportEndDate then DateOfDismantle else @ReportEndDate End))+1 Else 0 end as TotalDays ,Case when Isnull(DailyHireRate,0)>0 then Quantity * Isnull(DailyHireRate,0) Else NULL End as DailyHireCost from Vw_AllJobInventoryWithRate join vw_tblRegister on vw_tblRegister.RegisterId=Vw_AllJobInventoryWithRate.RegisterId where clientId='" + databaseCon.Decrypt(id) + "' and(Vw_tblRegister.SiteLocId= '" + databaseCon.Decrypt(SiteID) + "' or '0'='" + databaseCon.Decrypt(SiteID) + "')and ( (Isnull(DateTagged,@ReportStartDate)<=@ReportEndDate) and Isnull(DateOfDismantle,@ReportEndDate)>=@ReportStartDate)) AllRegisterWithHireData group by AllRegisterWithHireData.RegisterId, ClientId, SiteLocation, Location, AssetNo,WONumber, ClientRef,[Description],DateTagged, DateOfDismantle, Comments, ScaffRef, ClientScaffRef ";

            //string query = " exec Proc_InvoiceCostReportSurgedate '" + ReportStartDate + "' , '" + ReportEndDate + "',  " + Clientid + " ," + SiteId + " ,'SAPPDF' ";

            string query = " exec Proc_InvoiceCostReportSurgedate_DownloadReports '" + ReportStartDate + "' , '" + ReportEndDate + "',  " + Clientid + " ," + SiteId + " ,'SAPPDF'," + RID + ", '" + MaterialType + "'";
            DataTable dt = databaseCon.GetData(query, true, 800);

            //Changing the pdf orientation potrait to landscape on 18 March 2020
            iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.LETTER, -110f, -110f, 50f, 50f);
            doc.SetPageSize(new Rectangle(990, 600));

            // iTextSharp.text.Document doc = new iTextSharp.text.Document(PageSize.A4, -50f, -50f, 20f, 20f);   //Page Size set top ,left,margin
            iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, Response.OutputStream);

            //Document doc = new Document(PageSize.A4, -50f, -50f, 20f, 20f);   //Page Size set top ,left,margin
            //doc.SetPageSize(iTextSharp.text.PageSize.A4);

            //PdfWriter writer = PdfWriter.GetInstance(doc, Response.OutputStream);
            Response.ContentType = ("application/pdf");
            HeaderFooter2 evnt = new HeaderFooter2();

            evnt.id = id;
            evnt.ddlSiteID = SiteID;
            evnt.SiteName = SiteName;
            evnt.ReportStartDate1 = ReportStartDate1;
            evnt.ReportEndDate1 = ReportEndDate1;

            writer.PageEvent = evnt;
            doc.Open();
            var Heading786 = FontFactory.GetFont("Arial", 10f, Font.BOLD, BaseColor.BLACK);
            //Heading786.Color = Color.GRAY;
            var Heading787 = FontFactory.GetFont("Arial", 20f, Font.BOLD, BaseColor.BLACK);
            var Heading788 = FontFactory.GetFont("Arial", 30f, Font.BOLD, BaseColor.BLACK);
            var DataFont = FontFactory.GetFont("Arial", 10f, Font.NORMAL, BaseColor.BLACK);
            Phrase phrase;
            PdfPCell cell;

            PdfPTable headertable1 = new PdfPTable(5);
            headertable1.SetWidths(new int[] { 10, 9, 25, 9, 9 });

            //Table Start      
            PdfPTable headertable2 = new PdfPTable(12);
            headertable2.SetWidths(new int[] { 8, 6, 11, 6, 4, 10, 4, 16, 6, 6, 4, 8 });
            //Table End


            //phrase = new Phrase();
            //phrase.Add(new Chunk("", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 12;
            ////cell.PaddingBottom = 6f;
            //cell.Border = 0;
            //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Reference", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Invoice Date", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Doc. Header Text", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Vendor No.", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Tax Code", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Amount - FI", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Assignment", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Item Text", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Cost Centre", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Work Order", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("WBS", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("GL Account", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk("", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 12;
            //cell.Border = 0;
            //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("(XBLNR)", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("(BLDAT)", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("(BKTXT)", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("(LIFNR)", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("(MWSKZ)", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("(WRBTR)", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("(ZUONR)", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("(SGTXT)", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("(KOSTL)", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("(AUFNR)", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("(PROJK)", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("(HKONT)", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk("", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 12;
            //cell.Border = 0;
            //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Len: 16 char", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Len: 8 dats", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Len: 25 char", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Len: 10 char", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Len: 2 char", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Len: 13 curr", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Len: 18 char", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Len: 50 char", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Len: 10 char", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Len: 12 char", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Len: 24 char", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Len: 10 char", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.PaddingBottom = 6f;
            cell.Border = 0;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(" ", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 12;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable2.AddCell(cell);



            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["TaxInvoiceNo"].ToString()) + " " + Date1, DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    phrase = new Phrase();
                    //phrase.Add(new Chunk(string.Format("{dd-MM-yyyy}", ReportEndDate), DataFont));
                    phrase.Add(new Chunk(string.Format("{0:dd-MMM-yy}", ReportEndDate),DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Desc1"].ToString()) + " " + Date1, DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["VendorNO"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["TaxCode"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString("$" + dt.Rows[i]["Amount(incl of GST)"]).ToString(), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Desc2"]).ToString(), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    phrase = new Phrase();
                    string Date2 = String.Format("{0:dd.MM.yyyy}", ReportStartDate);
                    string Date3 = String.Format("{0:dd.MM.yyyy}", ReportEndDate);
                    string DateFormat = Date2 + " - " + Date3;
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Desc3"]).ToString() + " " + DateFormat, DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Cost Centre"]).ToString(), DataFont));
                    //string InspDate = String.Format("{0:dd-MMM-yyyy}", dt.Rows[i]["RecentInspDate"]);
                    //phrase.Add(new Chunk(InspDate, DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Work Order"]).ToString(), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["WBS"]).ToString(), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Expense A/C"]).ToString(), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Amount(incl of GST)"])))
                    {
                        TotalSAPValues += Convert.ToDouble(dt.Rows[i]["Amount(incl of GST)"]);
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
                phrase.Add(new Chunk("Total:", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 3;
                cell.PaddingBottom = 6F;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                //phrase.Add(new Chunk(string.Format("${0:#,##0.0000}", TotalWeeklyHireValues), DataFont));
                phrase.Add(new Chunk(string.Format("${0:#,##0.00}", TotalSAPValues), DataFont));//Changed on 01/04/2020
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 3;
                cell.PaddingBottom = 6F;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                headertable2.AddCell(cell);
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("No Data Found For The Given Date Range", Heading788));
                cell = new PdfPCell(phrase);
                cell.Colspan = 12;
                cell.FixedHeight = 50;
                cell.Border = 3;
                cell.PaddingBottom = 6F;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable2.AddCell(cell);


            }
            

            phrase = new Phrase();
            phrase.Add(new Chunk("", DataFont));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.PaddingBottom = 6F;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable2.AddCell(cell);

            doc.Add(headertable1);
            doc.Add(headertable2);
            doc.Close();

        }
        public class HeaderFooter2 : iTextSharp.text.pdf.PdfPageEventHelper
        {
            public string id { get; set; }
            public string ddlSiteID { get; set; }
            public string SiteName { get; set; }
            public string ReportStartDate1 { get; set; }
            public string ReportEndDate1 { get; set; }

            List<PdfTemplate> pageNumberTemplates = new List<PdfTemplate>();// page header

            public override void OnStartPage(PdfWriter writer, Document doc)
            {
                int ClientID, SiteID;
                string Sites;
                //ClientID = Convert.ToInt32(databaseCon.Decrypt(HttpContext.Current.Request.QueryString["id"]));
                //SiteID = Convert.ToInt32(databaseCon.Decrypt(HttpContext.Current.Request.QueryString["Site"]));
                //Sites = Convert.ToString(databaseCon.Decrypt(HttpContext.Current.Request.QueryString["Sites"]));

                //DateTime ReportStartDate = Convert.ToDateTime(databaseCon.Decrypt(HttpContext.Current.Request.QueryString["ReportStartDate1"]));
                //DateTime ReportEndDate = Convert.ToDateTime(databaseCon.Decrypt(HttpContext.Current.Request.QueryString["ReportEndDate1"]));

                ClientID = Convert.ToInt32(databaseCon.Decrypt(id));
                SiteID = Convert.ToInt32(databaseCon.Decrypt(ddlSiteID));
                Sites = Convert.ToString(databaseCon.Decrypt(SiteName));

                DateTime ReportStartDate = Convert.ToDateTime(databaseCon.Decrypt(ReportStartDate1));
                DateTime ReportEndDate = Convert.ToDateTime(databaseCon.Decrypt(ReportEndDate1));

                string queryheader = "select ClientImage from tblClient where ClientId= '" + ClientID + "'";
                DataTable dtheader = databaseCon.GetData(queryheader);
                var Heading786 = FontFactory.GetFont("Arial", 10f, Font.BOLD, BaseColor.BLACK);
                var Heading787 = FontFactory.GetFont("Arial", 20f, Font.BOLD, BaseColor.BLACK);
                var DataFont = FontFactory.GetFont("Arial", 10f, Font.NORMAL, BaseColor.BLACK);
                Phrase phrase;
                PdfPCell cell;

                PdfPTable headertable1 = new PdfPTable(5);
                headertable1.SetWidths(new int[] { 10, 9, 25, 9, 9 });

                PdfPTable headertable2 = new PdfPTable(12);
                headertable2.SetWidths(new int[] { 8, 6, 11, 6, 4, 10, 4, 16, 6, 6, 4, 8 });


                if (!string.IsNullOrEmpty(Convert.ToString(dtheader.Rows[0]["ClientImage"])))
                {
                    var uri = new Uri(databaseCon.clientimageLink);
                    var noimage = System.Web.HttpContext.Current.Server.MapPath("/Images/Logo2.png");
                    //var exists = blob.Exists();
                    if (Convert.ToString(dtheader.Rows[0]["ClientImage"]) == "")
                    {
                        string nophoto = System.Web.HttpContext.Current.Server.MapPath("/images/Logo2.png");
                        cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
                        cell.FixedHeight = 50f;
                        cell.Colspan = 2;
                        //cell.PaddingLeft = 10f;
                        cell.PaddingTop = -10f;
                    }
                    else
                    {
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(System.Web.HttpContext.Current.Server.MapPath("/images/clientlogo/" + dtheader.Rows[0]["ClientImage"].ToString()));

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
                    string nophoto = System.Web.HttpContext.Current.Server.MapPath("/images/Logo2.png");
                    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
                    cell.FixedHeight = 50f;
                    //cell.PaddingLeft = 10f;
                    cell.Colspan = 2;
                    cell.PaddingTop = -10f;
                }
                cell.Border = 0;
                cell.Colspan = 2;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell.PaddingLeft = 20f;
                cell.PaddingTop = -10f;
                headertable1.AddCell(cell);


                phrase = new Phrase();
                phrase.Add(new Chunk("SAP CSV Report List", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 5;
                //cell.PaddingBottom = 6f;
                //cell.PaddingLeft = 100f;
                cell.PaddingTop = -10f;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                headertable1.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk(" ", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 12;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Site:", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.PaddingBottom = 6f;
                cell.Border = 0;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk(Sites, DataFont));
                cell = new PdfPCell(phrase);
                cell.PaddingBottom = 15f;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 1;
                cell.Colspan = 2;
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
                phrase.Add(new Chunk("Report Start Date :", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 3;
                cell.PaddingBottom = 6f;
                cell.Border = 0;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk(string.Format("{0:dd-MMM-yyyy}", ReportStartDate), DataFont));
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
                phrase.Add(new Chunk(string.Format("{0:dd-MMM-yyyy}", ReportEndDate), DataFont));
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
                cell.Colspan = 12;
                //cell.PaddingBottom = 6f;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 12;
                //cell.PaddingBottom = 6f;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Tax Invoice No.", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingBottom = 6f;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;

                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Invoice Date", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.PaddingBottom = 6f;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Desc1", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Vendor No.", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Tax Code", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Amount(incl of GST)", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Desc2", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Desc3", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Cost Centre", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Work Order", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("WBS", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Expense A/C", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);


                doc.Add(headertable1);
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
        public void btnSAPCSVReportExportToCsv_Click(string id, string SiteID, string rdpReportStartDate1, string rdpReportEndDate1, string SiteName, string materialType, string RID )
        {
            string MaterialType = databaseCon.Decrypt(materialType);

            if (SiteID != "")
            {
                if ((rdpReportStartDate1 != null) && (rdpReportEndDate1 != null))
                {

                    int Clientid = Convert.ToInt32(databaseCon.Decrypt(id));
                    //int SiteId = Convert.ToInt32(databaseCon.Decrypt(SiteID));
                    DateTime StartDate = Convert.ToDateTime(rdpReportStartDate1);
                    DateTime EndDate = Convert.ToDateTime(rdpReportEndDate1);
                    //string Date1 = StartDate.ToString("MMM-yyyy").Replace("-", " ");

                    //string Date2 = StartDate.ToString("dd-MM-yyyy");
                    //string Date3 = EndDate.ToString("dd-MM-yyyy");
                    //string Date4 = StartDate.ToString("dd.MM.yyyy");
                    //string Date5 = EndDate.ToString("dd.MM.yyyy");
                    ////string DateFormat = Date2 + " - " + Date3;
                    //string DateFormat = Date4 + " - " + Date5;

                    //string query = "select 'Reference' As 'Tax Invoice No.','Invoice Date' as 'Invoice Date','Doc. Header Text' as Desc1,'Vendor No.' as 'Vendor No.','Tax Code' as 'Tax Code','Amount - FI' as 'Amount(incl of GST)','Assignment' as Desc2,'Item Text' as Desc3,'Cost Centre' as 'Cost Centre','Work Order' as 'Work Order','WBS' as 'WBS','GL Account' as 'Expense A/C' Union All select '(XBLNR)' As 'Tax Invoice No.','(BLDAT)' as 'Invoice Date','(BKTXT)' as Desc1,'(LIFNR)' as 'Vendor No.','(MWSKZ)' as 'Tax Code','(WRBTR)' as 'Amount(incl of GST)','(ZUONR)' as Desc2,'(SGTXT)' as Desc3,'(KOSTL)' as 'Cost Centre','(AUFNR)' as 'Work Order','(PROJK)' as 'WBS','(HKONT)' as 'Expense A/C' Union All select 'Len: 16 char' As 'Tax Invoice No.','Len: 8 dats' as 'Invoice Date','Len: 25 char' as Desc1,'Len: 10 char' as 'Vendor No.','Len: 2 char' as 'Tax Code','Len: 13 curr' as 'Amount(incl of GST)','Len: 18 char' as Desc2,'Len: 50 char' as Desc3,'Len: 10 char' as 'Cost Centre','Len: 12 char' as 'Work Order','Len: 24 char' as 'WBS','Len: 10 char' as 'Expense A/C' Union All Select Left('WACO" + " " + Date1 + "'+ replicate(Char(32),16), 16) As 'Tax Invoice No.','" + EndDate.ToString("dd-MM-yyyy") + "' as 'Invoice Date',Left('OLE Scaffold" + " " + Date1 + "'+ replicate(Char(32),25), 25) as Desc1,Left('101378'+replicate(Char(32),10), 10) as 'Vendor No.',Left('G1'+replicate(Char(32),2), 2) as 'Tax Code',Left(Convert(varchar,(Convert(Decimal(18, 2),(Convert(Decimal(18, 2),Convert(Decimal(18, 2), Sum(Isnull(DailyHireCost, 0))) * Max(DaysCalc))*10)/100) + Convert(Decimal(18, 2),Convert(Decimal(18, 2), Sum(Isnull(DailyHireCost, 0))) * Max(DaysCalc)))) + replicate(Char(32),13), 13) as 'Amount(incl of GST)',Left(''+replicate(Char(32),18), 18) As Desc2,Left('Scaffold hire" + " " + DateFormat + "' + replicate(Char(32),50), 50) As Desc3,  Left(''+replicate(Char(32),10), 10) as 'Cost Centre', Left( WONumber +replicate(Char(32),12), 12) as 'Work Order',Left(''+replicate(Char(32),24), 24) as WBS, Case when WONumber like '39%'  then Left('30800001'+replicate(Char(32),10), 10) else Left('5051016'+replicate(Char(32),10), 10) end  AS 'Expense A/C'  from ( Select Vw_AllJobInventoryWithRate.RegisterId,InventoryId,Vw_AllJobInventoryWithRate.MaterialType,ItemCode,ItemDesc,Quantity,ItemWeight,TotalWeight,subCategoryId,SubCategory,HireMeasurement,EnteredHireRate,DailyHireRate,WeeklyHireRate , ClientId, vw_tblRegister.SiteLocId,vw_tblRegister.SiteLocation,Location, AssetNo, WONumber, ClientRef , [Description] , Tonnage, DateTagged, DateOfDismantle,Comments, ScaffRef, ClientScaffRef, DateDiff(d,(Case When DateTagged>'" + rdpReportStartDate1.SelectedDate + "' then Datetagged else '" + rdpReportStartDate1.SelectedDate + "' End),(Case When DateOfDismantle<'" + rdpReportEndDate1.SelectedDate + "' then DateOfDismantle else '" + rdpReportEndDate1.SelectedDate + "' End))+1 as DaysCalc ,Case When DateTagged IS not null then DateDiff(d,Datetagged,(Case When DateOfDismantle<'" + rdpReportEndDate1.SelectedDate + "' then DateOfDismantle else '" + rdpReportEndDate1.SelectedDate + "' End))+1 Else 0 end as TotalDays ,Case when Isnull(DailyHireRate,0)>0 then Quantity * Isnull(DailyHireRate,0) Else NULL End as DailyHireCost from Vw_AllJobInventoryWithRate join vw_tblRegister on vw_tblRegister.RegisterId=Vw_AllJobInventoryWithRate.RegisterId where clientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and(Vw_tblRegister.SiteLocId= '" + ddlSite.SelectedValue + "' or '0'='" + ddlSite.SelectedValue + "')and ( (Isnull(DateTagged,'" + rdpReportStartDate1.SelectedDate + "')<='" + rdpReportEndDate1.SelectedDate + "') and Isnull(DateOfDismantle,'" + rdpReportEndDate1.SelectedDate + "')>='" + rdpReportStartDate1.SelectedDate + "')) AllRegisterWithHireData group by AllRegisterWithHireData.RegisterId, ClientId, SiteLocation, Location, AssetNo,WONumber, ClientRef,[Description],DateTagged, DateOfDismantle, Comments, ScaffRef, ClientScaffRef ";

                    // Changed Query on 26-May-2020
                    // string query = "select 'Reference' As 'Tax Invoice No.','Invoice Date' as 'Invoice Date','Doc. Header Text' as Desc1,'Vendor No.' as 'Vendor No.','Tax Code' as 'Tax Code','Amount - FI' as 'Amount(incl of GST)','Assignment' as Desc2,'Item Text' as Desc3,'Cost Centre' as 'Cost Centre','Work Order' as 'Work Order','WBS' as 'WBS','GL Account' as 'Expense A/C' Union All select '(XBLNR)' As 'Tax Invoice No.','(BLDAT)' as 'Invoice Date','(BKTXT)' as Desc1,'(LIFNR)' as 'Vendor No.','(MWSKZ)' as 'Tax Code','(WRBTR)' as 'Amount(incl of GST)','(ZUONR)' as Desc2,'(SGTXT)' as Desc3,'(KOSTL)' as 'Cost Centre','(AUFNR)' as 'Work Order','(PROJK)' as 'WBS','(HKONT)' as 'Expense A/C' Union All select 'Len: 16 char' As 'Tax Invoice No.','Len: 8 dats' as 'Invoice Date','Len: 25 char' as Desc1,'Len: 10 char' as 'Vendor No.','Len: 2 char' as 'Tax Code','Len: 13 curr' as 'Amount(incl of GST)','Len: 18 char' as Desc2,'Len: 50 char' as Desc3,'Len: 10 char' as 'Cost Centre','Len: 12 char' as 'Work Order','Len: 24 char' as 'WBS','Len: 10 char' as 'Expense A/C' Union All Select Left('WACO" + " " + Date1 + "'+ replicate(Char(32),16), 16) As 'Tax Invoice No.','" + EndDate.ToString("dd-MM-yyyy") + "' as 'Invoice Date',Left(upper(left(sitelocation,3))+' Scaffold' +" + "' " + Date1 + "'+ replicate(Char(32),25), 25) as Desc1,Left('101378'+replicate(Char(32),10), 10) as 'Vendor No.',Left('G1'+replicate(Char(32),2), 2) as 'Tax Code',Left(Convert(varchar,(Convert(Decimal(18, 2),(Convert(Decimal(18, 2),Convert(Decimal(18, 2), Sum(Isnull(DailyHireCost, 0))) * Max(DaysCalc))*10)/100) + Convert(Decimal(18, 2),Convert(Decimal(18, 2), Sum(Isnull(DailyHireCost, 0))) * Max(DaysCalc)))) + replicate(Char(32),13), 13) as 'Amount(incl of GST)',Left(''+replicate(Char(32),18), 18) As Desc2,Left('Scaffold hire" + " " + DateFormat + "' + replicate(Char(32),50), 50) As Desc3,  Left(''+replicate(Char(32),10), 10) as 'Cost Centre', Left( WONumber +replicate(Char(32),12), 12) as 'Work Order',Left(''+replicate(Char(32),24), 24) as WBS, Case when WONumber like '39%'  then Left('30800001'+replicate(Char(32),10), 10) else Left('5051016'+replicate(Char(32),10), 10) end  AS 'Expense A/C'  from ( Select Vw_AllJobInventoryWithRate.RegisterId,InventoryId,Vw_AllJobInventoryWithRate.MaterialType,ItemCode,ItemDesc,Quantity,ItemWeight,TotalWeight,subCategoryId,SubCategory,HireMeasurement,EnteredHireRate,DailyHireRate,WeeklyHireRate , ClientId, vw_tblRegister.SiteLocId,vw_tblRegister.SiteLocation,Location, AssetNo, WONumber, ClientRef , [Description] , Tonnage, DateTagged, DateOfDismantle,Comments, ScaffRef, ClientScaffRef, DateDiff(d,(Case When DateTagged>'" + rdpReportStartDate1 + "' then Datetagged else '" + rdpReportStartDate1+ "' End),(Case When DateOfDismantle<'" + rdpReportEndDate1 + "' then DateOfDismantle else '" + rdpReportEndDate1 + "' End))+1 as DaysCalc ,Case When DateTagged IS not null then DateDiff(d,Datetagged,(Case When DateOfDismantle<'" + rdpReportEndDate1 + "' then DateOfDismantle else '" + rdpReportEndDate1 + "' End))+1 Else 0 end as TotalDays ,Case when Isnull(DailyHireRate,0)>0 then Quantity * Isnull(DailyHireRate,0) Else NULL End as DailyHireCost from Vw_AllJobInventoryWithRate join vw_tblRegister on vw_tblRegister.RegisterId=Vw_AllJobInventoryWithRate.RegisterId where clientId='" + databaseCon.Decrypt(id) + "' and(Vw_tblRegister.SiteLocId= '" + SiteID + "' or '0'='" + SiteID + "')and ( (Isnull(DateTagged,'" + rdpReportStartDate1 + "')<='" + rdpReportEndDate1 + "') and Isnull(DateOfDismantle,'" + rdpReportEndDate1 + "')>='" + rdpReportStartDate1 + "')) AllRegisterWithHireData group by AllRegisterWithHireData.RegisterId, ClientId, SiteLocation, Location, AssetNo,WONumber, ClientRef,[Description],DateTagged, DateOfDismantle, Comments, ScaffRef, ClientScaffRef ";
                    //string query = " exec Proc_InvoiceCostReportSurgedate '" + StartDate + "' , '" + EndDate + "',  " + Clientid + " ," + SiteID + " ,'" + MaterialType + "'," + RID + ",'CSV' ";

                    string query = " exec Proc_InvoiceCostReportSurgedate_DownloadReports '" + StartDate + "' , '" + EndDate + "',  " + Clientid + " ," + SiteID + " ,'CSV'," + RID + ", '" + MaterialType + "'";
                    DataTable dt = databaseCon.GetData(query, true, 800);

                    //Add on 30-May-2020
                    // string Site = ddlSite.SelectedItem.Text;
                    string Site = SiteName;
                    string FileName = "";
                    if (Site != "All")
                    {
                        FileName = "_" + Site.Substring(0, Site.Length >= 3 ? 3 : Site.Length).ToUpper() + " CSV";
                    }
                    else
                    {
                        FileName = " CSV";
                    }
                    //For download File
                    Response.ContentType = "Application/x-msexcel";
                    Response.AddHeader("content-disposition", "attachment;filename=SAP Upload" + FileName + ".csv");
                    Response.Write(csvfile(dt));
                    Response.End();

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

        }
        //added by s.sourabh 17 dec 2022
        public void btnSapGenerateReport_Click(string UID, string UserRole, string ClientId, string SiteId, string rdpReportStartDate, string rdpReportEndDate ,string MaterialType)
        {
            int id = Convert.ToInt32(databaseCon.Decrypt(UID));
            int SiteID = Convert.ToInt32(SiteId);
            int ClientID = Convert.ToInt32(databaseCon.Decrypt(ClientId));
            //string StartDate = databaseCon.Decrypt(rdpReportStartDate);
            //string EndDate = databaseCon.Decrypt(rdpReportEndDate);
            string StartDate = rdpReportStartDate;
            string EndDate = rdpReportEndDate;
            DateTime DateStartDate = Convert.ToDateTime(StartDate);
            DateTime DateEndDate = Convert.ToDateTime(EndDate);
            DateTime Datecurrdate = Convert.ToDateTime(StartDate);
            DateTime Dateenddate = Convert.ToDateTime(EndDate);
            string execquery = "";
            string URole = databaseCon.Decrypt(UserRole);
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
                        string querysitedatadelete = "Delete from [TblGenerateInvoiceCostReportSiteData] where SiteId='" + SiteId + "'and ActualSiteStartDate='" + DateStartDate + "'and ActualSiteEndDate= '" + DateEndDate + "' ";
                        databaseCon.RunCmd(querysitedatadelete);
                    }
                    while (Datecurrdate <= Dateenddate)
                    {
                        //string execquery = " exec Proc_InvoiceCostReportSurgedate_GenerateReport '" + StartDate + "' , '" + EndDate + "',  " + ClientID + " ," + SiteID + "  ";
                        if (Datecurrdate.AddDays(9) > Dateenddate)
                        {
                            execquery = " exec Proc_InvoiceCostReportSurgedate_GenerateReport '" + Datecurrdate + "' , '" + Dateenddate + "',  " + ClientID + " ," + SiteID + ",'Generate'," + 0 + "," + "'All'" + ",'" + DateStartDate + "','" + DateEndDate + "'  ";
                            
                        }
                        else
                        {
                            execquery = " exec Proc_InvoiceCostReportSurgedate_GenerateReport '" + Datecurrdate + "' , '" + Datecurrdate.AddDays(9) + "',  " + ClientID + " ," + SiteID + ",'Generate'," + 0 + "," + "'All'" + ",'" + DateStartDate + "','" + DateEndDate + "'  ";
                        }



                        // DataTable dt = databaseCon.GetData(execquery, true, 1600);
                        databaseCon.RunCmd(execquery);
                        Datecurrdate = Datecurrdate.AddDays(10);

                    }
                }

            }
        }
        public ActionResult SapGenerateLastReport(string UID, string UserRole, string ClientId, string SiteId, string rdpReportStartDate, string rdpReportEndDate)
        {
            var GeneratedValue = "";
            int id = Convert.ToInt32(databaseCon.Decrypt(UID));
            int SiteID = Convert.ToInt32(SiteId);
            int ClientID = Convert.ToInt32(databaseCon.Decrypt(ClientId));
            //string StartDate = databaseCon.Decrypt(rdpReportStartDate);
            //string EndDate = databaseCon.Decrypt(rdpReportEndDate);
            string StartDate = rdpReportStartDate;
            string EndDate = rdpReportEndDate;

            var Result = "";

            string URole = databaseCon.Decrypt(UserRole);
            if (SiteId != "")
            {
                //  if ((rdpReportStartDate.SelectedDate != null) && (rdpReportEndDate.SelectedDate != null))
                if ((StartDate != null) && (EndDate != null))
                {

                    //string query = "select isnull(cast(format(max(ReportCreatedOn),'dd-MMM-yyyy') as nvarchar(20)),'Not Yet Generated') as maxcreatedon from [GenerateInvoiceCostReport] where SiteLocId='" + SiteId + "'and  [ReportStartDate] ='" + StartDate + "'and [ReportEndDate]= '" + EndDate + "' ";
                    string query = "select isnull(cast(format(max(ReportCreatedOn),'dd-MMM-yyyy') as nvarchar(20)),'Not Yet Generated') as maxcreatedon from [TblGenerateInvoiceCostReport] where SiteLocId='" + SiteId + "'and  [ActualStartDate] ='" + StartDate + "'and [ActualEndDate]= '" + EndDate + "' ";
                    DataTable dt = databaseCon.GetData(query);

                    if (dt.Rows.Count > 0)
                    {
                        GeneratedValue = dt.Rows[0]["maxcreatedon"].ToString();

                    }
                }
            }
            Result = "GeneratedValue" + GeneratedValue;
            return Json(new { GeneratedValue = GeneratedValue });
        }
        //end 
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
    }
}