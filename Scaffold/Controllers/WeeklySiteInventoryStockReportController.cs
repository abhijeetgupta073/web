using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Scaffold.App_Code;


namespace Scaffold.Controllers
{
    public class WeeklySiteInventoryStockReportController : Controller
    {
        float Totalquantity = 0;
        float WeightTotal = 0;
        double TotalWeeklyHireValues = 0;
        // GET: WeeklySiteInventoryStockReport
        public ActionResult Index()
        {
            return View();
        }

        public void WeeklySiteInventoryStockReport(string UID, string UserRole, string ClientId, string SiteId,  string materialType, string RID)
        {
            int id = Convert.ToInt32(databaseCon.Decrypt(UID));
            int SiteID = Convert.ToInt32(databaseCon.Decrypt(SiteId));
            int ClientID = Convert.ToInt32(databaseCon.Decrypt(ClientId));
            string URole = databaseCon.Decrypt(UserRole);
            string MaterialType = databaseCon.Decrypt(materialType);
            string query = "";

            var date1 = DateTime.Now;
            var date2 = DateTime.Today.AddDays(-6);

            string StartDate = date2.ToString("MM-dd-yyyy");
            string EndDate = date1.ToString("MM-dd-yyyy");


            //  dateRange = DateTime.Today.AddDays(1).AddSeconds(-1);
            //For weekly Calculations
            //For daily bases
            //query = "Select SiteId,MaterialType,ItemCode,ItemDesc,Sum(OrgSiteQuantity)OrgSiteQuantity,Sum(QuantityToBeSent)QuantityToBeSent,Sum(AvailableQuantity)AvailableQuantity,ItemWeight ,Sum(TotalWeight)TotalWeight,Sum(QuantityReturned)QuantityReturned,Sum(QuantityReturnedToDepot)QuantityReturnedToDepot,Sum(flag)flag,Sum(SiteDamagedQuantityReturnToDepot)SiteDamagedQuantityReturnToDepot,Sum(JobQuantityDamaged)JobQuantityDamaged,Sum(OnSite)OnSite ,Min(HireMeasurement) as HireMeasurement,Min(DailyHireRate) DailyHireRate,sum(TotalDailyHire) as TotalDailyHire from(select Case when '0' = '" + SiteID + "' then 0 else SiteId End SiteId, MaterialType, ItemCode, ItemDesc, OrgSiteQuantity, isnull(QuantityToBeSent, 0) as QuantityToBeSent, convert(int, isnull(AvailableQuantity, 0)) as AvailableQuantity, ItemWeight, convert(decimal(18, 2), (isnull(OrgSiteQuantity, 0) - isnull(QuantityReturnedToDepot, 0)) * ItemWeight) as TotalWeight, convert(int, isnull(QuantityReturned, 0)) as QuantityReturned, QuantityReturnedToDepot, flag, SiteDamagedQuantityReturnToDepot, JobQuantityDamaged, convert(int, (isnull(OrgSiteQuantity, 0) - isnull(QuantityReturnedToDepot, 0))) as OnSite ,HireMeasurement,CASE WHEN HireMeasurement ='Tonnage' THEN convert(decimal(18, 4),((ItemWeight)/1000)*(isnull(HireRate,0))) WHEN HireMeasurement = 'Each' THEN convert(decimal(18, 4),(isnull(HireRate, 0))) END as DailyHireRate,CASE WHEN HireMeasurement = 'Tonnage' THEN convert(decimal(18, 4),((QuantityTobesent * ItemWeight) / 1000) * (isnull(HireRate, 0))) WHEN HireMeasurement = 'Each' THEN convert(decimal(18, 4),(QuantityTobesent) * (isnull(HireRate, 0)))END as TotalDailyHire from Vw_SiteInventory left join tblInventory on tblInventory.InventoryID = Vw_SiteInventory.InventoryID  left join tblSubCategory on tblInventory.SubCategoryID = tblSubCategory.SubCategoryID where tblInventory.ClientID =  " + ClientID + " and (SiteId = '" + SiteID + "' or '0' = '" + SiteID + "') and SendQuantity is not null) AllSites Group by SiteId, MaterialType, ItemCode, ItemDesc, ItemWeight order by MaterialType,ItemCode";
            // query = "exec Pro_WeeklyHireRate '" + ClientID + "','" + SiteID + "','"+ ParPDF + "'";   //18-jan-2022
            //query = "exec Proc_InvoiceCostReportSurgedate '" + StartDate + "', '" + EndDate + "', " + ClientID + "," + SiteID + ",'weeklyperday'";
             query = " exec [Proc_InvoiceCostReportSurgedate_DownloadReports] '" + StartDate + "' , '" + EndDate + "' ,  " + ClientID + " ," + SiteID + " ,'weeklyperday'," + RID + " ,'" + MaterialType + "' ";
            // Site ID 0 for All Site Inventory Data Get
            DataTable dt = databaseCon.GetData(query, true, 800);

            iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.LETTER, -110f, -110f, 50f, 50f);
            doc.SetPageSize(new Rectangle(990, 600));

            iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, Response.OutputStream);

            Response.ContentType = ("application/pdf");
            HeaderFooter2 evnt = new HeaderFooter2();
            writer.PageEvent = evnt;
            evnt.ClientID = ClientID;
            evnt.SiteID = Convert.ToInt32(SiteID);

            doc.Open();
            var Heading786 = FontFactory.GetFont("Arial", 10f, Font.BOLD, BaseColor.BLACK);
            var Heading787 = FontFactory.GetFont("Arial", 20f, Font.BOLD, BaseColor.BLACK);
            var DataFont = FontFactory.GetFont("Arial", 10f, Font.NORMAL, BaseColor.BLACK);
            Phrase phrase;
            PdfPCell cell;

            PdfPTable headertable1 = new PdfPTable(5);
            headertable1.SetWidths(new int[] { 10, 9, 25, 9, 9 });

            PdfPTable headertable2 = new PdfPTable(12);
              //headertable2.SetWidths(new int[] { 10, 9, 20, 7, 7, 9, 9, 10, 9, 10, 11 }); //20-Jan-2022
            headertable2.SetWidths(new int[] { 9, 7, 17, 7, 7, 7, 8, 9, 10, 9, 10, 11 });

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Material Type"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Item Code"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Item Desc"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    phrase = new Phrase();
                    string ReportDate11 = String.Format("{0:MM/dd/yyyy}", dt.Rows[i]["Report Date"]);
                    phrase.Add(new Chunk(ReportDate11, DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    phrase = new Phrase();
                    //string OrgSiteqty = String.Format("{0:#,##0}", dt.Rows[i]["OrgSiteQuantity"]); // Change on 24 March 2020
                    string OnSiteQty = String.Format("{0:#,##0}", dt.Rows[i]["On Site"]);
                    phrase.Add(new Chunk(OnSiteQty, DataFont));
                    //phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["OrgSiteQuantity"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    phrase = new Phrase();
                    string QtyToSend = String.Format("{0:#,##0}", dt.Rows[i]["Qty With Job"]);
                    phrase.Add(new Chunk(QtyToSend, DataFont));
                    //phrase.Add(new Chunk(Convert.ToInt32(dt.Rows[i]["QuantityToBeSent"]).ToString(), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);


                    phrase = new Phrase();
                    // string onSite = String.Format("{0:#,##0}", dt.Rows[i]["OnSite"]);
                    // phrase.Add(new Chunk(onSite, DataFont));
                    string AvailableQuantityQty = String.Format("{0:#,##0}", dt.Rows[i]["Available Quantity"]);
                    phrase.Add(new Chunk(AvailableQuantityQty, DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);
                    Totalquantity += Convert.ToInt32(dt.Rows[i]["On Site"]);

                    phrase = new Phrase();
                    string itemweight = String.Format("{0:#,##0.00}", dt.Rows[i]["Item Weight"]);
                    phrase.Add(new Chunk(itemweight, DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    phrase = new Phrase();
                    string Totalweight = String.Format("{0:#,##0.00}", dt.Rows[i]["Total Weight"]);
                    phrase.Add(new Chunk(Totalweight, DataFont));
                    //phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["TotalWeight"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Hire Measurement"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    phrase = new Phrase();
                    string WeeklyItemHire = String.Format("${0:#,##0.0000}", dt.Rows[i]["Daily Hire Rate"]);
                    phrase.Add(new Chunk(WeeklyItemHire, DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    phrase = new Phrase();
                    string TotalWeeklyHire = String.Format("${0:#,##0.0000}", dt.Rows[i]["Total Daily Hire"]);
                    phrase.Add(new Chunk(TotalWeeklyHire, DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Total Weight"])))
                    {
                        WeightTotal += Convert.ToInt32(dt.Rows[i]["Total Weight"]);
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Total Daily Hire"])))
                    {
                        TotalWeeklyHireValues += Convert.ToDouble(dt.Rows[i]["Total Daily Hire"]);
                    }

                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 12;
                cell.Border = 0;
                cell.PaddingBottom = 6F;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

            }
            phrase = new Phrase();
            phrase.Add(new Chunk("", DataFont));
            cell = new PdfPCell(phrase);
            cell.Colspan = 7;
            cell.PaddingBottom = 6F;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable2.AddCell(cell);

            //phrase = new Phrase();
            //string Totalqty = string.Format("{0:#,##0}", Totalquantity);
            //phrase.Add(new Chunk(Totalqty.ToString(), DataFont));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.Border = 3;
            //cell.PaddingBottom = 6F;
            //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //cell.BackgroundColor = BaseColor.GRAY;
            //headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", DataFont));
            cell = new PdfPCell(phrase);
            cell.Colspan = 2;
            cell.PaddingBottom = 6F;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable2.AddCell(cell);

            //phrase = new Phrase();

            //decimal Total = Convert.ToDecimal(WeightTotal / 1000);
            //string TotalWe = string.Format("{0:#,##0.00}", Total);
            //phrase.Add(new Chunk(TotalWe.ToString(), DataFont));
            //phrase.Add(new Chunk(" Tonnes", DataFont));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.Border = 3;
            //cell.PaddingBottom = 6F;
            //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //cell.BackgroundColor = BaseColor.GRAY;
            //headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", DataFont));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
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
            cell.Border = PdfPCell.RIGHT_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            //phrase.Add(new Chunk(string.Format("${0:#,##0.0000}", TotalWeeklyHireValues), DataFont));
            phrase.Add(new Chunk(string.Format("${0:#,##0.00}", TotalWeeklyHireValues), DataFont));//Changed on 01/04/2020
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.Border = 3;
            cell.PaddingBottom = 6F;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.BackgroundColor = BaseColor.GRAY;
            headertable2.AddCell(cell);


            doc.Add(headertable1);
            doc.Add(headertable2);
            doc.Close();

        }


        public class HeaderFooter2 : iTextSharp.text.pdf.PdfPageEventHelper
        {
            public int ClientID { get; set; }
            public int SiteID { get; set; }
            List<PdfTemplate> pageNumberTemplates = new List<PdfTemplate>();// page header

            public override void OnStartPage(PdfWriter writer, Document doc)
            {
                string queryheader = "select distinct ClientImage,Case when '0'='" + SiteID + "' then 'All' Else SiteLocation End as SiteLocation from tblClient join tblSiteLocation on tblclient.clientId = tblsitelocation.clientId where tblClient.ClientId  = '" + ClientID + "' and (tblsitelocation.sitelocid='" + SiteID + "' or '0'='" + SiteID + "')";
                DataTable dtheader = databaseCon.GetData(queryheader);
                var Heading786 = FontFactory.GetFont("Arial", 10f, Font.BOLD, BaseColor.BLACK);
                var Heading787 = FontFactory.GetFont("Arial", 20f, Font.BOLD, BaseColor.BLACK);
                var DataFont = FontFactory.GetFont("Arial", 10f, Font.NORMAL, BaseColor.BLACK);
                Phrase phrase;
                PdfPCell cell;

                PdfPTable headertable1 = new PdfPTable(5);
                headertable1.SetWidths(new int[] { 10, 9, 25, 9, 9 });

                //   PdfPTable headertable2 = new PdfPTable(11);
                PdfPTable headertable2 = new PdfPTable(12);

                //  headertable2.SetWidths(new int[] { 10, 9, 20, 7, 7, 9, 9, 10, 9, 10, 11 });   20-jan-2022
                headertable2.SetWidths(new int[] { 9, 7, 17, 7, 7, 7, 8, 9, 10, 9, 10, 11 });



                if (!string.IsNullOrEmpty(Convert.ToString(dtheader.Rows[0]["ClientImage"])))
                {
                    var uri = new Uri(databaseCon.clientimageLink);
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
                cell.Colspan = 2;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell.PaddingLeft = 20f;
                cell.PaddingTop = -10f;
                headertable1.AddCell(cell);


                phrase = new Phrase();
                phrase.Add(new Chunk("Weekly Site Stock Take", Heading787));
                //phrase.Add(new Chunk(Convert.ToString(dtheader.Rows[0]["SiteName"].ToString()), Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 3;
                //cell.PaddingBottom = 6f;
                //cell.PaddingLeft = 100f;
                cell.PaddingTop = -10f;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                headertable1.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading787));
                //phrase.Add(new Chunk(Convert.ToString(dtheader.Rows[0]["SiteName"].ToString()), Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 7;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Site:", Heading786));
                //phrase.Add(new Chunk(Convert.ToString(dtheader.Rows[0]["SiteName"].ToString()), Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 2;
                cell.PaddingBottom = 6f;
                //cell.PaddingLeft = 100f;
                //cell.PaddingTop = 30f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                //phrase.Add(new Chunk("Site:", Heading787));
                phrase.Add(new Chunk(Convert.ToString(dtheader.Rows[0]["SiteLocation"].ToString()), DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 3;
                cell.PaddingBottom = 6f;
                //cell.PaddingLeft = 100f;
                //cell.PaddingTop = 30f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk(" ", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 12;
                cell.PaddingBottom = 0f;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 7;
                cell.PaddingBottom = 6f;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Week Ending Date:", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 2;
                //cell.PaddingBottom = 6f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("" + DateTime.Now.ToString("dd-MMM-yyy") + "", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 3;
                cell.PaddingBottom = 6f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
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
                phrase.Add(new Chunk("Material Type", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingBottom = 6f;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;

                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Item Code", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.PaddingBottom = 6f;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Description", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Report Date", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                //phrase.Add(new Chunk("Qty in Yard", Heading786)); // Change On 24 March 2020
                phrase.Add(new Chunk("On Site", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                // phrase.Add(new Chunk("Quantity Out of Yard", Heading786));
                phrase.Add(new Chunk("Qty With Job", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                //phrase.Add(new Chunk("On Site", Heading786)); // Change On 24 March 2020
                phrase.Add(new Chunk("Avaliable", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Item Weight (kg)", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Total Weight (kg)", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("UOM", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Weekly Hire Rate", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Total Weekly Hire Cost", Heading786));
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


        // Weekly Site Stock Report Btn Export To CSV Click Code 

        public void btnWeeklyReportExportToCsv_Click(string ClientID, string SiteID ,String WeeklySummaryDetail , string materialType, string RID)
        {
            int clientId = Convert.ToInt32(databaseCon.Decrypt(ClientID));
            int siteId = Convert.ToInt32(databaseCon.Decrypt(SiteID));
            string MaterialType = databaseCon.Decrypt(materialType);

            var date1 = DateTime.Now.Date;
            var date2 = DateTime.Today.AddDays(-6);

            string ReportStartDate = date2.ToString("MM-dd-yyyy");
            string ReportEndDate = date1.ToString("MM-dd-yyyy");

            //  if (ddlSite.SelectedValue != "")
            if (!siteId.Equals("null"))
            {

                //string query = " Select MaterialType, ItemCode,ItemDesc as Description,Sum(OrgSiteQuantity)OnSite,Sum(QuantityToBeSent)QtyWithJob,Sum(AvailableQuantity)Available,ItemWeight ,Sum(TotalWeight)TotalWeight,Min(HireMeasurement) as UOM,Min(DailyHireRate) WeeklyHireRate,sum(TotalDailyHire) as TotalWeeklyHireCost from(select Case when '0' = '" + siteId + "' then 0 else SiteId End SiteId, MaterialType, ItemCode, ItemDesc, OrgSiteQuantity, isnull(QuantityToBeSent, 0) as QuantityToBeSent, convert(int, isnull(AvailableQuantity, 0)) as AvailableQuantity, ItemWeight, convert(decimal(18, 2), (isnull(OrgSiteQuantity, 0) - isnull(QuantityReturnedToDepot, 0)) * ItemWeight) as TotalWeight, convert(int, isnull(QuantityReturned, 0)) as QuantityReturned, QuantityReturnedToDepot, flag, SiteDamagedQuantityReturnToDepot, JobQuantityDamaged, convert(int, (isnull(OrgSiteQuantity, 0) - isnull(QuantityReturnedToDepot, 0))) as OnSite ,HireMeasurement,CASE WHEN HireMeasurement ='Tonnage' THEN convert(decimal(18, 4),((ItemWeight)/1000)*(isnull(HireRate,0))) WHEN HireMeasurement = 'Each' THEN convert(decimal(18, 4),(isnull(HireRate, 0))) END as DailyHireRate,CASE WHEN HireMeasurement = 'Tonnage' THEN convert(decimal(18, 4),((QuantityTobesent * ItemWeight) / 1000) * (isnull(HireRate, 0))) WHEN HireMeasurement = 'Each' THEN convert(decimal(18, 4),(QuantityTobesent) * (isnull(HireRate, 0)))END as TotalDailyHire from Vw_SiteInventory left join tblInventory on tblInventory.InventoryID = Vw_SiteInventory.InventoryID  left join tblSubCategory on tblInventory.SubCategoryID = tblSubCategory.SubCategoryID where tblInventory.ClientID =  " + clientId + " and (SiteId = '" + siteId + "' or '0' = '" + siteId + "') and SendQuantity is not null) AllSites Group by SiteId, MaterialType, ItemCode, ItemDesc, ItemWeight order by MaterialType,ItemCode";
                //  string query = "exec Pro_WeeklyHireRate '" + clientId + "','" + siteId + "','"+ ParExcel + "'"; //18-jan-2022
                //string query = "exec Proc_InvoiceCostReportSurgedate '" + StartDate + "', '" + EndDate + "', " + clientId + "," + siteId + " ,'" + WeeklySummaryDetail + "' , '" + RID + "' , '" + MaterialType + "' ";
                string query = " exec [Proc_InvoiceCostReportSurgedate_DownloadReports] '" + ReportStartDate + "' , '" + ReportEndDate + "' ,  " + clientId + " ," + siteId + " ,'" + WeeklySummaryDetail + "'," + RID + " ,'" + MaterialType + "' ";

                DataTable dt1 = databaseCon.GetData(query, true, 800);

                //For download File
                Response.ContentType = "Application/x-msexcel";
                Response.AddHeader("content-disposition", "attachment;filename=WeeklyCostReport.csv");
                Response.Write(csvfile(dt1));
                Response.End();

            }
            else
            {
                //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                //art1.Style.Add("display", "block");
                //alertlabel.Text = "<strong>Alert ! </strong> Please Select the Site.";
            }

        }
        public string btnWhrGenerateReport_Click(string UID, string UserRole, string ClientId, string SiteId, string MaterialType)
        {
            var message = "success";

            try 
            { 

            //int id = Convert.ToInt32(databaseCon.Decrypt(UID));
            int SiteID = Convert.ToInt32(SiteId);
            int ClientID = Convert.ToInt32(databaseCon.Decrypt(ClientId));
            var date1 = DateTime.Now.Date;
            var date2 = DateTime.Today.AddDays(-6);

            string StartDate = date2.ToString("MM-dd-yyyy");
            string EndDate = date1.ToString("MM-dd-yyyy");
            DateTime DateStartDate = Convert.ToDateTime(StartDate);
            DateTime DateEndDate = Convert.ToDateTime(EndDate);
            DateTime Datecurrdate = Convert.ToDateTime(StartDate);
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
                            string querysitedatadelete = "Delete from [TblGenerateInvoiceCostReportSiteData] where SiteId='" + SiteId + "'and ActualSiteStartDate= '" + DateStartDate + "'and ActualSiteEndDate=  '" + DateEndDate + "' ";
                            databaseCon.RunCmd(querysitedatadelete);
                    }
                    while (Datecurrdate <= DateEndDate)
                    {
                        //string execquery = " exec Proc_InvoiceCostReportSurgedate_GenerateReport '" + StartDate + "' , '" + EndDate + "',  " + ClientID + " ," + SiteID + "  ";
                        if (Datecurrdate.AddDays(9) > DateEndDate)
                        {
                            execquery = " exec Proc_InvoiceCostReportSurgedate_GenerateReport '" + Datecurrdate + "' , '" + DateEndDate + "',  " + ClientID + " ," + SiteID + ",'Generate'," + 0 + "," + "'All'" + ",'" + DateStartDate + "','" + DateEndDate + "'  ";
                                Datecurrdate = DateEndDate;
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
                return message;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return message;
            }
        }
        public ActionResult WHRGenerateLastReport(string UID, string UserRole, string ClientId, string SiteId)
        {
            var GeneratedValue = "";
            //int id = Convert.ToInt32(databaseCon.Decrypt(UID));
            int SiteID = Convert.ToInt32(SiteId);
            int ClientID = Convert.ToInt32(databaseCon.Decrypt(ClientId));
            var date1 = DateTime.Now;
            var date2 = DateTime.Today.AddDays(-6);
            string StartDate = date2.ToString("MM-dd-yyyy");
            string EndDate = date1.ToString("MM-dd-yyyy");

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