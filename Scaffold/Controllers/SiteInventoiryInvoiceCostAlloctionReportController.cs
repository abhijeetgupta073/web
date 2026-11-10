using iTextSharp.text;
using iTextSharp.text.pdf;
using Scaffold.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Scaffold.Controllers
{
    public class SiteInventoiryInvoiceCostAlloctionReportController : Controller
    {
        double TotalInvoiceValues = 0;
        double TotalCalculatedValues = 0;
        // GET: SiteInventoiryInvoiceCostAlloctionReport
        public ActionResult Index()
        {
            return View();
        }

         public void SiteInventoryInvoiceCostAllocationReport(string UID, string UserRole, string ClientId, string SiteId, string ReportStrDt, string ReportEndDt)
        {
            int id = Convert.ToInt32(databaseCon.Decrypt(UID));
            int SiteID = Convert.ToInt32(databaseCon.Decrypt(SiteId));
            int ClientID = Convert.ToInt32(databaseCon.Decrypt(ClientId));
            string StartDate = databaseCon.Decrypt(ReportStrDt);
            string EndDate = databaseCon.Decrypt(ReportEndDt);

            string URole = databaseCon.Decrypt(UserRole);

            // string query = " Select ClientId, RegisterID, SiteLocation as [Site],Location as Area,AssetNo as Equipment,WONumber as WONumber, Max(DaysCalc)DaysCalc,Convert(Decimal(18, 4), Sum(Isnull(DailyHireCost, 0))) as DailyHireCost,Convert(Decimal(18, 4), Sum(Isnull(DailyHireCost, 0)) * Max(DaysCalc)) as CalculatedHireCost,Convert(Decimal(18, 4), Sum(Isnull(DailyHireCost, 0)) * Max(DaysCalc) * 0.77) as InvoicedHireCost ,ClientRef as RequestedBy ,[Description] as Activity ,REPLACE(CONVERT(VARCHAR(11),DateTagged,106), ' ','-') as DateTagged,REPLACE(CONVERT(VARCHAR(11),DateOfDismantle,106), ' ','-') as DateOfDismantle,Comments, ScaffRef,ClientScaffRef as ClientRef from(Select Vw_JobInventoryWithSubCategoryRate.* , ClientId, vw_tblRegister.RegisterId,SiteLocation, Location, AssetNo, WONumber, ClientRef , [Description] , Tonnage, DateTagged, DateOfDismantle,Comments, ScaffRef, ClientScaffRef, DateDiff(d,(Case When DateTagged>='" + databaseCon.Decrypt(Request.QueryString["ReportStartDate"]) + "' and DateTagged<='" + databaseCon.Decrypt(Request.QueryString["ReportEndDate"]) + "' then Datetagged else '"+ databaseCon.Decrypt(Request.QueryString["ReportStartDate"]) + "' End),'" + databaseCon.Decrypt(Request.QueryString["ReportEndDate"]) + "')+1 as DaysCalc ,Case when Isnull(DailyHireRate,0)>0 then AvailableQuantity*Isnull(DailyHireRate,0) Else NULL End as DailyHireCost from Vw_JobInventoryWithSubCategoryRate join vw_tblRegister on vw_tblRegister.RegisterId=Vw_JobInventoryWithSubCategoryRate.ToId where clientId='" +id+"' and(Vw_JobInventoryWithSubCategoryRate.FromId= '"+SiteID+"' or '0'='"+SiteID+"')) AllRegisterWithHireData group by RegisterId, ClientId, SiteLocation, Location, AssetNo, WONumber, ClientRef,[Description],DateTagged, DateOfDismantle, Comments, ScaffRef, ClientScaffRef ";
            // Changed on 24-Mar-2020.
            //string query = "Select ClientId, RegisterID, SiteLocation as [Site],Location as Area,AssetNo as Equipment,WONumber as WONumber, Max(DaysCalc)DaysCalc,Convert(Decimal(18, 4), Sum(Isnull(DailyHireCost, 0))) as DailyHireCost,Convert(Decimal(18, 4), Sum(Isnull(DailyHireCost, 0)) * Max(DaysCalc)) as CalculatedHireCost,Convert(Decimal(18, 4), Sum(Isnull(DailyHireCost, 0)) * Max(DaysCalc) * 0.77) as InvoicedHireCost ,ClientRef as RequestedBy ,[Description] as Activity ,REPLACE(CONVERT(VARCHAR(11),DateTagged,106), ' ','-') as DateTagged,REPLACE(CONVERT(VARCHAR(11),DateOfDismantle,106), ' ','-') as DateOfDismantle,Comments,ScaffRef,ClientScaffRef as ClientRef from ( Select Vw_AllJobInventoryWithRate.RegisterId,InventoryId,Vw_AllJobInventoryWithRate.MaterialType,ItemCode,ItemDesc,Quantity,ItemWeight,TotalWeight,subCategoryId,SubCategory,HireMeasurement,EnteredHireRate,DailyHireRate,WeeklyHireRate ,ClientId, vw_tblRegister.SiteLocId,vw_tblRegister.SiteLocation,Location, AssetNo, WONumber, ClientRef , [Description] , Tonnage,DateTagged, DateOfDismantle,Comments, ScaffRef, ClientScaffRef, DateDiff(d,(Case When DateTagged>='" + databaseCon.Decrypt(Request.QueryString["ReportStartDate"]) + "' and DateTagged<='" + databaseCon.Decrypt(Request.QueryString["ReportEndDate"]) + "' then Datetagged else '" + databaseCon.Decrypt(Request.QueryString["ReportStartDate"]) + "' End),'" + databaseCon.Decrypt(Request.QueryString["ReportEndDate"]) + "')+1 as DaysCalc ,Case when Isnull(DailyHireRate,0)>0 then Quantity * Isnull(DailyHireRate,0) Else NULL End as DailyHireCost from Vw_AllJobInventoryWithRate join vw_tblRegister on vw_tblRegister.RegisterId=Vw_AllJobInventoryWithRate.RegisterId where clientId='" + id + "'  and(Vw_tblRegister.SiteLocId= '" + SiteID + "' or '0'='" + SiteID + "')) AllRegisterWithHireData group by AllRegisterWithHireData.RegisterId, ClientId, SiteLocation, Location, AssetNo, WONumber, ClientRef,[Description],DateTagged, DateOfDismantle, Comments, ScaffRef, ClientScaffRef";
            // changed query on 25-Mar-2020.
            //string query = "Declare @ReportStartDate datetime, @ReportEndDate Datetime Set @ReportStartDate = '" + databaseCon.Decrypt(Request.QueryString["ReportStartDate"]) + "' Set @ReportEndDate = '" + databaseCon.Decrypt(Request.QueryString["ReportEndDate"]) + "' Select ClientId, RegisterID, SiteLocation as [Site],Location as Area,AssetNo as Equipment,WONumber as WONumber, Max(DaysCalc)DaysCalc,Convert(Decimal(18, 4), Sum(Isnull(DailyHireCost, 0))) as DailyHireCost,Convert(Decimal(18, 4), Sum(Isnull(DailyHireCost, 0)) * Max(DaysCalc)) as CalculatedHireCost,Convert(Decimal(18, 4), Sum(Isnull(DailyHireCost, 0)) * Max(DaysCalc) * 0.77) as InvoicedHireCost ,ClientRef as RequestedBy ,[Description] as Activity ,REPLACE(CONVERT(VARCHAR(11),DateTagged,106),' ','-') as DateTagged,REPLACE(CONVERT(VARCHAR(11),DateOfDismantle,106),' ','-') as DateOfDismantle,Comments,ScaffRef,ClientScaffRef as ClientRef from(Select Vw_AllJobInventoryWithRate.RegisterId, InventoryId, Vw_AllJobInventoryWithRate.MaterialType, ItemCode, ItemDesc, Quantity, ItemWeight, TotalWeight, subCategoryId, SubCategory, HireMeasurement, EnteredHireRate, DailyHireRate, WeeklyHireRate , ClientId, vw_tblRegister.SiteLocId, vw_tblRegister.SiteLocation, Location, AssetNo, WONumber, ClientRef , [Description] , Tonnage, DateTagged, DateOfDismantle, Comments, ScaffRef, ClientScaffRef, DateDiff(d,(Case When DateTagged>@ReportStartDate then Datetagged else @ReportStartDate End),(Case When DateOfDismantle<@ReportEndDate then DateOfDismantle else @ReportEndDate End))+1 as DaysCalc,Case When DateTagged IS not null then DateDiff(d,Datetagged,(Case When DateOfDismantle < @ReportEndDate then DateOfDismantle else @ReportEndDate End))+1 Else 0 end as TotalDays , Case when Isnull(DailyHireRate,0)>0 then Quantity * Isnull(DailyHireRate,0) Else NULL End as DailyHireCost from Vw_AllJobInventoryWithRate join vw_tblRegister on vw_tblRegister.RegisterId=Vw_AllJobInventoryWithRate.RegisterId where clientId='" + id + "' and(Vw_tblRegister.SiteLocId= '" + SiteID + "' or '0'='" + SiteID + "') and( (isnull(DateTagged, @ReportStartDate)<=@ReportEndDate) and Isnull(DateOfDismantle, @ReportEndDate)>=@ReportStartDate)) AllRegisterWithHireData group by AllRegisterWithHireData.RegisterId, ClientId, SiteLocation, Location, AssetNo, WONumber, ClientRef, [Description], DateTagged, DateOfDismantle, Comments, ScaffRef, ClientScaffRef ";
            string query = "Declare @ReportStartDate datetime, @ReportEndDate Datetime Set  @ReportStartDate='" + StartDate + "' Set @ReportEndDate='" + EndDate + "' Select ClientId, RegisterID, SiteLocation as [Site],Location as Area,AssetNo as Equipment,WONumber as WONumber, Max(DaysCalc)DaysCalc,Max(TotalDays)TotalDays,Convert(Decimal(18, 2), Sum(Isnull(DailyHireCost, 0))) as DailyHireCost,Convert(Decimal(18, 2),Convert(Decimal(18, 2), Sum(Isnull(DailyHireCost, 0))) * Max(DaysCalc)) as CalculatedHireCost,Convert(Decimal(18, 2),Convert(Decimal(18, 2), Sum(Isnull(DailyHireCost, 0))) * Max(DaysCalc) * 0.77) as InvoicedHireCost ,ClientRef as RequestedBy ,[Description] as Activity ,REPLACE(CONVERT(VARCHAR(11),DateTagged,106),' ','-') as DateTagged,REPLACE(CONVERT(VARCHAR(11),DateOfDismantle,106),' ','-') as DateOfDismantle,Comments, ScaffRef,ClientScaffRef as ClientRef from ( Select Vw_AllJobInventoryWithRate.RegisterId,InventoryId,Vw_AllJobInventoryWithRate.MaterialType,ItemCode,ItemDesc,Quantity,ItemWeight,TotalWeight,subCategoryId,SubCategory,HireMeasurement,EnteredHireRate,DailyHireRate,WeeklyHireRate , ClientId, vw_tblRegister.SiteLocId,vw_tblRegister.SiteLocation,Location, AssetNo, WONumber, ClientRef , [Description] , Tonnage, DateTagged, DateOfDismantle,Comments, ScaffRef, ClientScaffRef, DateDiff(d,(Case When DateTagged>@ReportStartDate then Datetagged else @ReportStartDate End),(Case When DateOfDismantle<@ReportEndDate then DateOfDismantle else @ReportEndDate End))+1 as DaysCalc ,Case When DateTagged IS not null then DateDiff(d,Datetagged,(Case When DateOfDismantle<@ReportEndDate then DateOfDismantle else @ReportEndDate End))+1 Else 0 end as TotalDays ,Case when Isnull(DailyHireRate,0)>0 then Quantity * Isnull(DailyHireRate,0) Else NULL End as DailyHireCost from Vw_AllJobInventoryWithRate join vw_tblRegister on vw_tblRegister.RegisterId=Vw_AllJobInventoryWithRate.RegisterId where clientId='" + ClientID + "' and(Vw_tblRegister.SiteLocId= '" + SiteID + "' or '0'='" + SiteID + "')and ( (Isnull(DateTagged,@ReportStartDate)<=@ReportEndDate) and Isnull(DateOfDismantle,@ReportEndDate)>=@ReportStartDate)) AllRegisterWithHireData group by AllRegisterWithHireData.RegisterId, ClientId, SiteLocation, Location, AssetNo,WONumber, ClientRef,[Description],DateTagged, DateOfDismantle, Comments, ScaffRef, ClientScaffRef ";

            DataTable dt = databaseCon.GetData(query);

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
            var DataFont = FontFactory.GetFont("Arial", 10f, Font.NORMAL, BaseColor.BLACK);
            Phrase phrase;
            PdfPCell cell;

            PdfPTable headertable1 = new PdfPTable(5);
            headertable1.SetWidths(new int[] { 10, 9, 25, 9, 9 });

            //Table Start

            PdfPTable headertable2 = new PdfPTable(15);
            headertable2.SetWidths(new int[] { 9, 8, 10, 9, 8, 8, 9, 10, 12, 10, 10, 8, 8, 8, 9 });

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
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["DateTagged"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);



                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["DateOfDismantle"].ToString()), DataFont));
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


                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["InvoicedHireCost"])))
                    {
                        TotalInvoiceValues += Convert.ToDouble(dt.Rows[i]["InvoicedHireCost"]);
                    }

                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["CalculatedHireCost"])))
                    {
                        TotalCalculatedValues += Convert.ToDouble(dt.Rows[i]["CalculatedHireCost"]);
                    }


                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 14;
                cell.Border = 0;
                cell.PaddingBottom = 6F;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

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
                //int ClientID, SiteID;
                //ClientID = Convert.ToInt32(databaseCon.Decrypt(HttpContext.Current.Request.QueryString["id"]));
                //SiteID = Convert.ToInt32(databaseCon.Decrypt(HttpContext.Current.Request.QueryString["Site"]));


                // add code 23/March/2020.
                //string query = "select Convert(Decimal(18, 4), Sum(Isnull(InvoicedHireCost,0)))TotalInvoicedHireCost from (Select ClientId, RegisterID, SiteLocation as [Site],Location as Area,AssetNo as Equipment,WONumber as WONumber,Max(DaysCalc)DaysCalc,Convert(Decimal(18, 4), Sum(Isnull(DailyHireCost, 0))) as DailyHireCost,Convert(Decimal(18, 4), Sum(Isnull(DailyHireCost, 0)) * Max(DaysCalc)) as CalculatedHireCost,Convert(Decimal(18, 4), Sum(Isnull(DailyHireCost, 0)) * Max(DaysCalc) * 0.77) as InvoicedHireCost,ClientRef as RequestedBy ,[Description] as Activity ,DateTagged, DateOfDismantle,Comments, ScaffRef,ClientScaffRef as ClientRef from(Select Vw_JobInventoryWithSubCategoryRate.* , ClientId, vw_tblRegister.RegisterId,SiteLocation, Location, AssetNo, WONumber, ClientRef , [Description] , Tonnage, DateTagged, DateOfDismantle,Comments, ScaffRef, ClientScaffRef, DateDiff(d,(Case When DateTagged>'" + databaseCon.Decrypt(HttpContext.Current.Request.QueryString["ReportStartDate"]) + "' and DateTagged<='" + databaseCon.Decrypt(HttpContext.Current.Request.QueryString["ReportEndDate"]) + "' then Datetagged else '" + databaseCon.Decrypt(HttpContext.Current.Request.QueryString["ReportStartDate"]) + "' End),'" + databaseCon.Decrypt(HttpContext.Current.Request.QueryString["ReportEndDate"]) + "')+1 as DaysCalc,Case when Isnull(DailyHireRate,0)>0 then AvailableQuantity*Isnull(DailyHireRate,0) Else NULL End as DailyHireCost from Vw_JobInventoryWithSubCategoryRate join vw_tblRegister on vw_tblRegister.RegisterId=Vw_JobInventoryWithSubCategoryRate.ToId where clientId='" + ClientID+"' and(Vw_JobInventoryWithSubCategoryRate.FromId='"+ SiteID+"' or '0'='"+SiteID+"')) AllRegisterWithHireData group by RegisterId, ClientId, SiteLocation, Location, AssetNo, WONumber, ClientRef,[Description],DateTagged, DateOfDismantle, Comments, ScaffRef, ClientScaffRef )AllTotalRegisters";
                //Changed Query on 24/March/2020
                // string query = "select Convert(Decimal(18, 4), Sum(Isnull(InvoicedHireCost,0)))TotalInvoicedHireCost from (Select ClientId, RegisterID, SiteLocation as [Site],Location as Area,AssetNo as Equipment,WONumber as WONumber, Max(DaysCalc)DaysCalc,Convert(Decimal(18, 4), Sum(Isnull(DailyHireCost, 0))) as DailyHireCost,Convert(Decimal(18, 4), Sum(Isnull(DailyHireCost, 0)) * Max(DaysCalc)) as CalculatedHireCost,Convert(Decimal(18, 4), Sum(Isnull(DailyHireCost, 0)) * Max(DaysCalc) * 0.77) as InvoicedHireCost ,ClientRef as RequestedBy ,[Description] as Activity ,REPLACE(CONVERT(VARCHAR(11),DateTagged,106), ' ','-') as DateTagged,REPLACE(CONVERT(VARCHAR(11),DateOfDismantle,106), ' ','-') as DateOfDismantle,Comments, ScaffRef,ClientScaffRef as ClientRef from(Select Vw_AllJobInventoryWithRate.RegisterId, InventoryId, Vw_AllJobInventoryWithRate.MaterialType, ItemCode, ItemDesc, Quantity, ItemWeight, TotalWeight, subCategoryId, SubCategory,HireMeasurement, EnteredHireRate, DailyHireRate, WeeklyHireRate ,ClientId, vw_tblRegister.SiteLocId, vw_tblRegister.SiteLocation, Location, AssetNo, WONumber, ClientRef , [Description] , Tonnage,DateTagged, DateOfDismantle, Comments, ScaffRef, ClientScaffRef, DateDiff(d,(Case When DateTagged>= '" + databaseCon.Decrypt(HttpContext.Current.Request.QueryString["ReportStartDate"]) + "' and DateTagged<='" + databaseCon.Decrypt(HttpContext.Current.Request.QueryString["ReportEndDate"]) + "' then Datetagged else '" + databaseCon.Decrypt(HttpContext.Current.Request.QueryString["ReportStartDate"]) + "' End),'" + databaseCon.Decrypt(HttpContext.Current.Request.QueryString["ReportEndDate"]) + "')+1 as DaysCalc ,Case when Isnull(DailyHireRate,0)>0 then Quantity* Isnull(DailyHireRate,0) Else NULL End as DailyHireCost from Vw_AllJobInventoryWithRate join vw_tblRegister on vw_tblRegister.RegisterId=Vw_AllJobInventoryWithRate.RegisterId where  clientId='" + ClientID + "' and(Vw_tblRegister.SiteLocId='" + SiteID + "' or '0'='" + SiteID + "')) AllRegisterWithHireData group by AllRegisterWithHireData.RegisterId, ClientId, SiteLocation, Location, AssetNo, WONumber, ClientRef, [Description], DateTagged, DateOfDismantle, Comments, ScaffRef, ClientScaffRef)AllTotalRegisters";
                // changed query on 25-Mar-2020.
                //string query = "Declare @ReportStartDate datetime, @ReportEndDate Datetime Set  @ReportStartDate='" + databaseCon.Decrypt(HttpContext.Current.Request.QueryString["ReportStartDate"]) + "' Set @ReportEndDate='" + databaseCon.Decrypt(HttpContext.Current.Request.QueryString["ReportEndDate"]) + "' select Convert(Decimal(18, 4), Sum(Isnull(InvoicedHireCost,0)))TotalInvoicedHireCost from (Select ClientId, RegisterID, SiteLocation as [Site],Location as Area,AssetNo as Equipment,WONumber as WONumber, Max(DaysCalc)DaysCalc,Convert(Decimal(18, 4), Sum(Isnull(DailyHireCost, 0))) as DailyHireCost,Convert(Decimal(18, 4), Sum(Isnull(DailyHireCost, 0)) * Max(DaysCalc)) as CalculatedHireCost,Convert(Decimal(18, 4), Sum(Isnull(DailyHireCost, 0)) * Max(DaysCalc) * 0.77) as InvoicedHireCost ,ClientRef as RequestedBy ,[Description] as Activity ,REPLACE(CONVERT(VARCHAR(11),DateTagged,106),' ','-') as DateTagged,REPLACE(CONVERT(VARCHAR(11),DateOfDismantle,106),' ','-') as DateOfDismantle,Comments,ScaffRef,ClientScaffRef as ClientRef from ( Select Vw_AllJobInventoryWithRate.RegisterId,InventoryId,Vw_AllJobInventoryWithRate.MaterialType,ItemCode,ItemDesc,Quantity,ItemWeight,TotalWeight,subCategoryId,SubCategory,HireMeasurement,EnteredHireRate,DailyHireRate,WeeklyHireRate , ClientId, vw_tblRegister.SiteLocId,vw_tblRegister.SiteLocation,Location, AssetNo, WONumber,ClientRef , [Description] , Tonnage, DateTagged, DateOfDismantle,Comments, ScaffRef, ClientScaffRef, DateDiff(d,(Case When DateTagged>@ReportStartDate then Datetagged else @ReportStartDate End),(Case When DateOfDismantle<@ReportEndDate then DateOfDismantle else @ReportEndDate End))+1 as DaysCalc ,Case when Isnull(DailyHireRate,0)>0 then Quantity * Isnull(DailyHireRate,0) Else NULL End as DailyHireCost from Vw_AllJobInventoryWithRate join vw_tblRegister on vw_tblRegister.RegisterId=Vw_AllJobInventoryWithRate.RegisterId where clientId='" + ClientID + "' and(Vw_tblRegister.SiteLocId= '" + SiteID + "' or '0'='" + SiteID + "')and ( (isnull(DateTagged,@ReportStartDate)<=@ReportEndDate) and Isnull(DateOfDismantle,@ReportEndDate)>=@ReportStartDate)) AllRegisterWithHireData group by AllRegisterWithHireData.RegisterId, ClientId, SiteLocation, Location, AssetNo,WONumber, ClientRef,[Description],DateTagged, DateOfDismantle, Comments, ScaffRef, ClientScaffRef )AllTotalRegisters";
               

              //  string query = "Declare @ReportStartDate datetime, @ReportEndDate Datetime Set @ReportStartDate = '" + StartDate + "' Set @ReportEndDate = '" + EndDate + "' select Convert(Decimal(18, 2), Sum(Isnull(InvoicedHireCost, 0)))TotalInvoicedHireCost from (Select ClientId, RegisterID, SiteLocation as [Site], Location as Area, AssetNo as Equipment, WONumber as WONumber, Max(DaysCalc)DaysCalc, Max(TotalDays)TotalDays ,Convert(Decimal(18, 2), Sum(Isnull(DailyHireCost, 0))) as DailyHireCost,Convert(Decimal(18, 2),Convert(Decimal(18, 2), Sum(Isnull(DailyHireCost, 0))) * Max(DaysCalc)) as CalculatedHireCost,Convert(Decimal(18, 2),Convert(Decimal(18, 2), Sum(Isnull(DailyHireCost, 0))) * Max(DaysCalc) * 0.77) as InvoicedHireCost ,ClientRef as RequestedBy,[Description] as Activity, REPLACE(CONVERT(VARCHAR(11), DateTagged, 106), ' ', '-') as DateTagged, REPLACE(CONVERT(VARCHAR(11), DateOfDismantle, 106), ' ', '-') as DateOfDismantle, Comments, ScaffRef, ClientScaffRef as ClientRef from(Select Vw_AllJobInventoryWithRate.RegisterId, InventoryId, Vw_AllJobInventoryWithRate.MaterialType, ItemCode, ItemDesc, Quantity, ItemWeight, TotalWeight, subCategoryId, SubCategory, HireMeasurement, EnteredHireRate, DailyHireRate, WeeklyHireRate, ClientId, vw_tblRegister.SiteLocId, vw_tblRegister.SiteLocation, Location, AssetNo, WONumber, ClientRef, [Description], Tonnage, DateTagged, DateOfDismantle, Comments, ScaffRef, ClientScaffRef, DateDiff(d, (Case When DateTagged > @ReportStartDate then Datetagged else @ReportStartDate End), (Case When DateOfDismantle < @ReportEndDate then DateOfDismantle else @ReportEndDate End))+1 as DaysCalc ,Case When DateTagged IS not null then DateDiff(d, Datetagged,(Case When DateOfDismantle < @ReportEndDate then DateOfDismantle else @ReportEndDate End))+1 Else 0 end as TotalDays ,Case when Isnull(DailyHireRate, 0) > 0 then Quantity *Isnull(DailyHireRate, 0) Else NULL End as DailyHireCost from Vw_AllJobInventoryWithRate join vw_tblRegister on vw_tblRegister.RegisterId = Vw_AllJobInventoryWithRate.RegisterId where clientId = '" + ClientID + "' and(Vw_tblRegister.SiteLocId = '" + SiteID + "' or '0' = '" + SiteID + "')and((isnull(DateTagged, @ReportStartDate) <= @ReportEndDate) and Isnull(DateOfDismantle, @ReportEndDate) >= @ReportStartDate)) AllRegisterWithHireData group by AllRegisterWithHireData.RegisterId, ClientId, SiteLocation, Location, AssetNo,WONumber, ClientRef,[Description],DateTagged, DateOfDismantle, Comments, ScaffRef, ClientScaffRef )AllTotalRegisters ";

                string query = "Declare @ReportStartDate datetime, @ReportEndDate Datetime Set @ReportStartDate = '" + StartDate + "' Set @ReportEndDate = '" + EndDate + "' select Convert(Decimal(18, 2), Sum(Isnull(InvoicedHireCost, 0)))TotalInvoicedHireCost,Convert(Decimal(18, 2), Sum(Isnull(CalculatedHireCost, 0)))TotalCalculatedHireCost from (Select ClientId, RegisterID, SiteLocation as [Site], Location as Area, AssetNo as Equipment, WONumber as WONumber, Max(DaysCalc)DaysCalc, Max(TotalDays)TotalDays ,Convert(Decimal(18, 2), Sum(Isnull(DailyHireCost, 0))) as DailyHireCost,Convert(Decimal(18, 2),Convert(Decimal(18, 2), Sum(Isnull(DailyHireCost, 0))) * Max(DaysCalc)) as CalculatedHireCost,Convert(Decimal(18, 2),Convert(Decimal(18, 2), Sum(Isnull(DailyHireCost, 0))) * Max(DaysCalc) * 0.77) as InvoicedHireCost ,ClientRef as RequestedBy,[Description] as Activity, REPLACE(CONVERT(VARCHAR(11), DateTagged, 106), ' ', '-') as DateTagged, REPLACE(CONVERT(VARCHAR(11), DateOfDismantle, 106), ' ', '-') as DateOfDismantle, Comments, ScaffRef, ClientScaffRef as ClientRef from(Select Vw_AllJobInventoryWithRate.RegisterId, InventoryId, Vw_AllJobInventoryWithRate.MaterialType, ItemCode, ItemDesc, Quantity, ItemWeight, TotalWeight, subCategoryId, SubCategory, HireMeasurement, EnteredHireRate, DailyHireRate, WeeklyHireRate, ClientId, vw_tblRegister.SiteLocId, vw_tblRegister.SiteLocation, Location, AssetNo, WONumber, ClientRef, [Description], Tonnage, DateTagged, DateOfDismantle, Comments, ScaffRef, ClientScaffRef, DateDiff(d, (Case When DateTagged > @ReportStartDate then Datetagged else @ReportStartDate End), (Case When DateOfDismantle < @ReportEndDate then DateOfDismantle else @ReportEndDate End))+1 as DaysCalc ,Case When DateTagged IS not null then DateDiff(d, Datetagged,(Case When DateOfDismantle < @ReportEndDate then DateOfDismantle else @ReportEndDate End))+1 Else 0 end as TotalDays ,Case when Isnull(DailyHireRate, 0) > 0 then Quantity *Isnull(DailyHireRate, 0) Else NULL End as DailyHireCost from Vw_AllJobInventoryWithRate join vw_tblRegister on vw_tblRegister.RegisterId = Vw_AllJobInventoryWithRate.RegisterId where clientId = '" + ClientID + "' and(Vw_tblRegister.SiteLocId = '" + SiteID + "' or '0' = '" + SiteID + "')and((isnull(DateTagged, @ReportStartDate) <= @ReportEndDate) and Isnull(DateOfDismantle, @ReportEndDate) >= @ReportStartDate)) AllRegisterWithHireData group by AllRegisterWithHireData.RegisterId, ClientId, SiteLocation, Location, AssetNo,WONumber, ClientRef,[Description],DateTagged, DateOfDismantle, Comments, ScaffRef, ClientScaffRef )AllTotalRegisters ";

                DataTable dt = databaseCon.GetData(query);



                //DateTime ReportStartDate = Convert.ToDateTime(databaseCon.Decrypt(HttpContext.Current.Request.QueryString["ReportStartDate"]));
                //DateTime ReportEndDate = Convert.ToDateTime(databaseCon.Decrypt(HttpContext.Current.Request.QueryString["ReportEndDate"]));
                base.OnStartPage(writer, doc);
                string queryheader = "select ClientImage from tblClient where ClientId= '" + ClientID + "'";
                DataTable dtheader = databaseCon.GetData(queryheader);


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

                Phrase phrase;
                PdfPCell cell;

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
                phrase.Add(new Chunk("Invoice Cost Allocation", Heading787));
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
        public void btnInvoiceReportExportToCsv_Click(string ClientID, string SiteID, string rdpReportStartDate, string rdpReportEndDate)
        {
            int clientId = Convert.ToInt32(databaseCon.Decrypt(ClientID));
            int siteId = Convert.ToInt32(SiteID);
            string StrDate = rdpReportStartDate;
            string EndDate = rdpReportEndDate;
            // if (ddlSite.SelectedValue != "")
            if (SiteID != "")
            {
                //  if ((rdpReportStartDate.SelectedDate != null) && (rdpReportEndDate.SelectedDate != null))
                if ((StrDate != null) && (EndDate != null))
                {
                    // Asp Code Change On 03 April 2020

                    //  string query = "Declare @ReportStartDate datetime, @ReportEndDate Datetime Set  @ReportStartDate='" + rdpReportStartDate.SelectedDate + "' Set @ReportEndDate = '" + rdpReportEndDate.SelectedDate + "'  Select  SiteLocation as [Site],Location as Area,AssetNo as Equipment,WONumber as WONumber,Convert(Decimal(18, 2), Sum(Isnull(DailyHireCost, 0))) as DailyHireRate,Convert(Decimal(18, 2),Convert(Decimal(18, 2), Sum(Isnull(DailyHireCost, 0))) * Max(DaysCalc) * 0.77) as '$Invoiced', Convert(Decimal(18, 2),Convert(Decimal(18, 2), Sum(Isnull(DailyHireCost, 0))) * Max(DaysCalc)) as '$Calculated', ClientRef as RequestedBy ,[Description] as Activity ,REPLACE(CONVERT(VARCHAR(11),DateTagged,106),' ','-') as DateTagged,REPLACE(CONVERT(VARCHAR(11),DateOfDismantle,106),' ','-') as DateOfDismantle,Max(TotalDays)TotalDays,Comments, ScaffRef,ClientScaffRef as ClientRef from ( Select Vw_AllJobInventoryWithRate.RegisterId,InventoryId,Vw_AllJobInventoryWithRate.MaterialType,ItemCode,ItemDesc,Quantity,ItemWeight,TotalWeight,subCategoryId,SubCategory,HireMeasurement,EnteredHireRate,DailyHireRate,WeeklyHireRate , ClientId, vw_tblRegister.SiteLocId,vw_tblRegister.SiteLocation,Location, AssetNo, WONumber, ClientRef , [Description] , Tonnage, DateTagged, DateOfDismantle,Comments, ScaffRef, ClientScaffRef, DateDiff(d,(Case When DateTagged>@ReportStartDate then Datetagged else @ReportStartDate End),(Case When DateOfDismantle<@ReportEndDate then DateOfDismantle else @ReportEndDate End))+1 as DaysCalc ,Case When DateTagged IS not null then DateDiff(d,Datetagged,(Case When DateOfDismantle<@ReportEndDate then DateOfDismantle else @ReportEndDate End))+1 Else 0 end as TotalDays ,Case when Isnull(DailyHireRate,0)>0 then Quantity * Isnull(DailyHireRate,0) Else NULL End as DailyHireCost from Vw_AllJobInventoryWithRate join vw_tblRegister on vw_tblRegister.RegisterId=Vw_AllJobInventoryWithRate.RegisterId where clientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and(Vw_tblRegister.SiteLocId= '" + ddlSite.SelectedValue + "' or  '0'='" + ddlSite.SelectedValue + "')and ( (Isnull(DateTagged,@ReportStartDate)<=@ReportEndDate) and Isnull(DateOfDismantle,@ReportEndDate)>=@ReportStartDate)) AllRegisterWithHireData group by AllRegisterWithHireData.RegisterId, SiteLocation, Location, AssetNo,WONumber, ClientRef,[Description],DateTagged, DateOfDismantle, Comments, ScaffRef, ClientScaffRef ";


                   // string query = "Declare @ReportStartDate datetime, @ReportEndDate Datetime Set  @ReportStartDate='" + StrDate + "' Set @ReportEndDate = '" + EndDate + "'  Select  SiteLocation as [Site],Location as Area,AssetNo as Equipment,WONumber as WONumber,Convert(Decimal(18, 2), Sum(Isnull(DailyHireCost, 0))) as DailyHireRate,Convert(Decimal(18, 2),Convert(Decimal(18, 2), Sum(Isnull(DailyHireCost, 0))) * Max(DaysCalc) * 0.77) as '$Invoiced', Convert(Decimal(18, 2),Convert(Decimal(18, 2), Sum(Isnull(DailyHireCost, 0))) * Max(DaysCalc)) as '$Calculated', ClientRef as RequestedBy ,[Description] as Activity ,REPLACE(CONVERT(VARCHAR(11),DateTagged,106),' ','-') as DateTagged,REPLACE(CONVERT(VARCHAR(11),DateOfDismantle,106),' ','-') as DateOfDismantle,Max(TotalDays)TotalDays,Comments, ScaffRef,ClientScaffRef as ClientRef from ( Select Vw_AllJobInventoryWithRate.RegisterId,InventoryId,Vw_AllJobInventoryWithRate.MaterialType,ItemCode,ItemDesc,Quantity,ItemWeight,TotalWeight,subCategoryId,SubCategory,HireMeasurement,EnteredHireRate,DailyHireRate,WeeklyHireRate , ClientId, vw_tblRegister.SiteLocId,vw_tblRegister.SiteLocation,Location, AssetNo, WONumber, ClientRef , [Description] , Tonnage, DateTagged, DateOfDismantle,Comments, ScaffRef, ClientScaffRef, DateDiff(d,(Case When DateTagged>@ReportStartDate then Datetagged else @ReportStartDate End),(Case When DateOfDismantle<@ReportEndDate then DateOfDismantle else @ReportEndDate End))+1 as DaysCalc ,Case When DateTagged IS not null then DateDiff(d,Datetagged,(Case When DateOfDismantle<@ReportEndDate then DateOfDismantle else @ReportEndDate End))+1 Else 0 end as TotalDays ,Case when Isnull(DailyHireRate,0)>0 then Quantity * Isnull(DailyHireRate,0) Else NULL End as DailyHireCost from Vw_AllJobInventoryWithRate join vw_tblRegister on vw_tblRegister.RegisterId=Vw_AllJobInventoryWithRate.RegisterId where clientId='" + clientId + "' and(Vw_tblRegister.SiteLocId= '" + siteId + "' or  '0'='" + siteId + "')and ( (Isnull(DateTagged,@ReportStartDate)<=@ReportEndDate) and Isnull(DateOfDismantle,@ReportEndDate)>=@ReportStartDate)) AllRegisterWithHireData group by AllRegisterWithHireData.RegisterId, SiteLocation, Location, AssetNo,WONumber, ClientRef,[Description],DateTagged, DateOfDismantle, Comments, ScaffRef, ClientScaffRef ";

                    // Changed Query on 23/April/2020.
                    string query = "Declare @ReportStartDate datetime, @ReportEndDate Datetime Set  @ReportStartDate='" + StrDate + "' Set @ReportEndDate = '" + EndDate + "'  Select  SiteLocation as [Site],Location as Area,AssetNo as Equipment,WONumber as WONumber,Convert(Decimal(18, 2), Sum(Isnull(DailyHireCost, 0))) as DailyHireRate, Convert(Decimal(18, 2),Convert(Decimal(18, 2), Sum(Isnull(DailyHireCost, 0))) * Max(DaysCalc)) as '$Invoiced', ClientRef as RequestedBy ,[Description] as Activity ,REPLACE(CONVERT(VARCHAR(11),DateTagged,106),' ','-') as DateTagged,REPLACE(CONVERT(VARCHAR(11),DateOfDismantle,106),' ','-') as DateOfDismantle,Max(DaysCalc)HireDays,Max(TotalDays)TotalDays,Comments, ScaffRef,ClientScaffRef as ClientRef from ( Select Vw_AllJobInventoryWithRate.RegisterId,InventoryId,Vw_AllJobInventoryWithRate.MaterialType,ItemCode,ItemDesc,Quantity,ItemWeight,TotalWeight,subCategoryId,SubCategory,HireMeasurement,EnteredHireRate,DailyHireRate,WeeklyHireRate , ClientId, vw_tblRegister.SiteLocId,vw_tblRegister.SiteLocation,Location, AssetNo, WONumber, ClientRef ,  [Description] , Tonnage, DateTagged, DateOfDismantle,Comments, ScaffRef, ClientScaffRef, DateDiff(d,(Case When DateTagged>@ReportStartDate then Datetagged else @ReportStartDate End),(Case When DateOfDismantle<@ReportEndDate then DateOfDismantle else @ReportEndDate End))+1 as DaysCalc ,Case When DateTagged IS not null then DateDiff(d,Datetagged,(Case When DateOfDismantle<@ReportEndDate then DateOfDismantle else @ReportEndDate End))+1 Else 0 end as TotalDays ,Case when Isnull(DailyHireRate,0)>0 then Quantity * Isnull(DailyHireRate,0) Else NULL End as DailyHireCost from Vw_AllJobInventoryWithRate join vw_tblRegister on vw_tblRegister.RegisterId=Vw_AllJobInventoryWithRate.RegisterId where clientId='" + clientId + "' and(Vw_tblRegister.SiteLocId= '" + siteId + "' or  '0'='" + siteId + "')and ( (Isnull(DateTagged,@ReportStartDate)<=@ReportEndDate) and Isnull(DateOfDismantle,@ReportEndDate)>=@ReportStartDate)) AllRegisterWithHireData group by AllRegisterWithHireData.RegisterId, SiteLocation, Location, AssetNo,WONumber, ClientRef,[Description],DateTagged, DateOfDismantle, Comments, ScaffRef, ClientScaffRef ";

                    DataTable dt = databaseCon.GetData(query);

                    //For download File
                    Response.ContentType = "Application/x-msexcel";
                    Response.AddHeader("content-disposition", "attachment;filename=InvoiceCostReport.csv");
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

       

    }
}