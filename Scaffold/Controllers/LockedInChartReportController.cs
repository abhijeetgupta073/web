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
    public class LockedInChartReportController : Controller
    {
        // GET: LockedInChartReport
        public ActionResult Index()
        {
            return View();
        }

      public  void showLockedInChartReport(string id,string siteId)
        {

             siteId = databaseCon.Decrypt(siteId);
             string ClientID= databaseCon.Decrypt(id);

            if (siteId == "0")
            {
                siteId = "All";
            }
            

            //string query = "select Right ('000000' + convert (varchar, [ScaffRef]),6) as ScaffRef,Description,Location,BuildingNo,AssetNo,FloorLevel,SiteLocation,DesignType, MaterialType from VW_TblRegister where clientid='" + databaseCon.Decrypt(id) + "' and DateTagged <> '01/01/1900' and DateTagged is not null and('" + siteId + "' = 'All'  or SiteLocId = '" + siteId + "') and LockedIn = '1' Order by Location,BuildingNo";
            //DataTable dt = databaseCon.GetData(query);

            string query = "select Right ('000000' + convert (varchar, [ScaffRef]),6) as ScaffRef,Description,Location,BuildingNo,AssetNo,FloorLevel,SiteLocation,DesignType, MaterialType from VW_TblRegister where clientid='" + databaseCon.Decrypt(id) + "' and DateTagged <> '01/01/1900' and DateTagged is not null and('" + siteId + "' = 'All'  or SiteLocId = '" + siteId + "') and  LockedIn = '1' Order by Location,BuildingNo";
            DataTable dt = databaseCon.GetData(query);

            iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, -50f, -50f, 30f, 30f);
            iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, Response.OutputStream);
            Response.ContentType = ("application/Pdf");
            HeaderFooter2 evnt = new HeaderFooter2();
            evnt.id = ClientID;
            evnt.SiteId = siteId;
            writer.PageEvent = evnt;
            document.Open();


            var heading1 = FontFactory.GetFont("Arial", 18f, Font.NORMAL, BaseColor.BLACK);
            var heading2 = FontFactory.GetFont("tahoma", 10f, Font.NORMAL, BaseColor.BLACK);
            var font = FontFactory.GetFont("Arial", 10f, Font.BOLD, BaseColor.BLACK);
            var heading3 = FontFactory.GetFont("Arial", 18f, Font.BOLD, BaseColor.BLACK);
            Phrase phrase;
            PdfPCell cell;

            PdfPTable headertable1 = new PdfPTable(8);
            headertable1.SetWidths(new int[] { 6, 16, 15, 10, 6, 7, 9, 9 });


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                phrase = new Phrase();
                phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["ScaffRef"]), heading2));
                cell = new PdfPCell(phrase);
                cell.PaddingBottom = 15f;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 1;
                cell.Colspan = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                headertable1.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Description"]), heading2));
                cell = new PdfPCell(phrase);
                cell.PaddingBottom = 15f;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 1;
                cell.Colspan = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                headertable1.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Location"]), heading2));
                cell = new PdfPCell(phrase);
                cell.PaddingBottom = 15f;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 1;
                cell.Colspan = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                headertable1.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["BuildingNo"]), heading2));
                cell = new PdfPCell(phrase);
                cell.PaddingBottom = 15f;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 1;
                cell.Colspan = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                headertable1.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["AssetNo"]), heading2));
                cell = new PdfPCell(phrase);
                cell.PaddingBottom = 15f;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 1;
                cell.Colspan = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                headertable1.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["FloorLevel"]), heading2));
                cell = new PdfPCell(phrase);
                cell.PaddingBottom = 15f;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 1;
                cell.Colspan = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                headertable1.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["DesignType"]), heading2));
                cell = new PdfPCell(phrase);
                cell.PaddingBottom = 15f;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 1;
                cell.Colspan = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                headertable1.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["MaterialType"]), heading2));
                cell = new PdfPCell(phrase);
                cell.PaddingBottom = 15f;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 1;
                cell.Colspan = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                headertable1.AddCell(cell);
            }

            document.Add(headertable1);
            document.Close();
        }
        public class HeaderFooter2 : iTextSharp.text.pdf.PdfPageEventHelper
        {
           public string id { get; set; }
           public string SiteId { get; set; }
            public override void OnStartPage(PdfWriter writer, Document document)
            {
               // string SiteId, id;
                //SiteId = Convert.ToInt32(databaseCon.Decrypt(HttpContext.Current.Request.QueryString["siteId"]));

                //id = Convert.ToInt32(databaseCon.Decrypt(HttpContext.Current.Request.QueryString["id"]));

                string query = "select SiteLocation from VW_TblRegister where clientid='" + id + "' and DateTagged <> '01/01/1900' and DateTagged is not null and('" + SiteId + "' = 'All'  or SiteLocId = '" + SiteId + "') and LockedIn = 1";
                DataTable dt = databaseCon.GetData(query);

                var heading1 = FontFactory.GetFont("Arial", 18f, Font.NORMAL, BaseColor.BLACK);
                var heading2 = FontFactory.GetFont("tahoma", 10f, Font.NORMAL, BaseColor.BLACK);
                var font = FontFactory.GetFont("Arial", 10f, Font.BOLD, BaseColor.BLACK);
                var heading3 = FontFactory.GetFont("tahoma", 18f, Font.BOLD, BaseColor.BLACK);
                Phrase phrase;
                PdfPCell cell;

                PdfPTable headertable1 = new PdfPTable(8);
                headertable1.SetWidths(new int[] { 6, 16, 15, 10, 6, 7, 9, 9 });

                phrase = new Phrase();
                phrase.Add(new Chunk("Locked In Scaffolds for ", heading1));
                if(dt.Rows.Count>0) {
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[0]["SiteLocation"]), heading3));
                }else {
                    phrase.Add(new Chunk("", heading3));
                }
                
                cell = new PdfPCell(phrase);
                cell.Border = 0;
                cell.Colspan = 8;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.PaddingLeft = 0f;
                cell.PaddingTop = 0f;
                headertable1.AddCell(cell);

                //phrase = new Phrase();

                //cell = new PdfPCell(phrase);
                //cell.Border = 0;
                //cell.Colspan = 3;
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell.PaddingLeft = 0f;
                //cell.PaddingTop = 0f;
                //headertable1.AddCell(cell);


                phrase = new Phrase();
                phrase.Add(new Chunk("", heading2));
                cell = new PdfPCell(phrase);
                cell.Colspan = 8;
                cell.PaddingBottom = 6f;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable1.AddCell(cell);



                phrase = new Phrase();
                phrase.Add(new Chunk("", heading2));
                cell = new PdfPCell(phrase);
                cell.Colspan = 8;
                cell.PaddingBottom = 6f;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable1.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Scaf Ref #", font));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingBottom = 6f;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);


                phrase = new Phrase();
                phrase.Add(new Chunk("Reason for Access", font));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingBottom = 6f;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);


                phrase = new Phrase();
                phrase.Add(new Chunk("Location", font));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingBottom = 6f;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);


                phrase = new Phrase();
                phrase.Add(new Chunk("Building / Module No.", font));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingBottom = 6f;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);


                phrase = new Phrase();
                phrase.Add(new Chunk("Asset # / Func. Loc ", font));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingBottom = 6f;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Floor Level", font));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 3;
                cell.PaddingBottom = 6f;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Design Type", font));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 3;
                cell.PaddingBottom = 6f;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Material Type", font));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 3;
                cell.PaddingBottom = 6f;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
                document.Add(headertable1);


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
    }
}