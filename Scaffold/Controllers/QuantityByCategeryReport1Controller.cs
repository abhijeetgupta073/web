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
    public class QuantityByCategeryReport1Controller : Controller
    {
        // GET: QuantityByCategeryReport1
        public ActionResult Index()
        {
            return View();
        }
        public void showQuantityReport(string id, string CycloneAction, string Site, string Location, string ddlRiskRating)
        {

            //string CycloneActionName = databaseCon.Decrypt(Request.QueryString["CycloneAction"]);
            //string siteid = databaseCon.Decrypt(Request.QueryString["Site"]);
            //string jobselect = databaseCon.Decrypt(Request.QueryString["Location"]);

            string CycloneActionName = CycloneAction;
            string siteid = databaseCon.Decrypt(Request.QueryString["Site"]);
            string jobselect = databaseCon.Decrypt(Request.QueryString["Location"]);
            string SubOwner = databaseCon.Decrypt(Request.QueryString["SubOwner"]);

            if (siteid == "0")
            {
                siteid = "All";
            }
            if (jobselect == "0")
            {
                jobselect = "All";
            }

            // add on 24 Feb 2020.
            if (SubOwner == "0")
            {
                SubOwner = "All";
            }
            if (SubOwner == "")
            {
                SubOwner = "All";
            }
            // showQuantityReport(CycloneActionName, siteid, jobselect);
            var query = "";

            string ConditionAddNoAction = "";
            if ((CycloneActionName != "De-Planked") && (CycloneActionName != "Tied-Down"))
            {

                if (CycloneActionName == "No Action")
                {
                    ConditionAddNoAction = "and (CycloneAction='" + CycloneActionName + "' or CycloneAction is null) ";
                }
                else
                {
                    ConditionAddNoAction = "and CycloneAction='" + CycloneActionName + "'";
                }
                // string query = "select Right ('000000' + convert (varchar, [ScaffRef]),6) as ScaffRef,Description,Location,BuildingNo,AssetNo,FloorLevel,DesignType,MaterialType, TotalLabourDismantleEdit,PriorityRiskRating from VW_TblRegister where clientid='" + databaseCon.Decrypt(id) + "' and DateOfDismantle is NULL and(DateTagged <> '01/01/1900' and DateTagged is not null) and('" + siteid + "' = 'All'  or SiteLocId = '" + siteid + "') and ('" + jobselect + "' = 'All'  or LocationId = '" + jobselect + "') " + ConditionAddNoAction + " Order by Location,BuildingNo";

                // Commented On 26-07-2023 
                //string query1 = "select Right ('000000' + convert (varchar, [ScaffRef]),6) as ScaffRef,Description,Location,BuildingNo,AssetNo,FloorLevel,DesignType,MaterialType, TotalLabourDismantleEdit,PriorityRiskRating from VW_TblRegister where clientid='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and DateOfDismantle is NULL and(DateTagged <> '01/01/1900' and DateTagged is not null and status !='De-Planked' and status!='Tie-Down') and('" + siteid + "' = 'All'  or SiteLocId = '" + siteid + "') and ('" + jobselect + "' = 'All'  or LocationId = '" + jobselect + "') and  ('" + SubOwner.Replace("'", "''") + "' = 'All'  or Case when ScaffoldOwner = 'P&I' then 'P&I' else 'Maintenance' End = '" + SubOwner.Replace("'", "''") + "') " + ConditionAddNoAction + " Order by Location,BuildingNo";

                // Added on 26-07-2023
                string query1 = "select Right ('000000' + convert (varchar, [ScaffRef]),6) as ScaffRef,Description,Location,BuildingNo,AssetNo,FloorLevel,DesignType,MaterialType,case when CycloneAction = 'Full Strip' then ManHrsDismantle When CycloneAction <> 'Full Strip' and UseEnteredValue = 1 then TotalLabourDismantleEdit else TotalLabourDismantle End TotalLabourDismantleEdit,PriorityRiskRating from VW_TblRegister where clientid='" + databaseCon.Decrypt(id) + "' and DateOfDismantle is NULL and(DateTagged <> '01/01/1900' and DateTagged is not null and status !='De-Planked' and status!='Tie-Down') and('" + siteid + "' = 'All'  or SiteLocId = '" + siteid + "') and('" + ddlRiskRating + "' = 'All'  or PriorityRiskRating = '" + ddlRiskRating + "') and ('" + jobselect + "' = 'All'  or LocationId = '" + jobselect + "') " + ConditionAddNoAction + " Order by Location,BuildingNo";

                query = query1;
            }
            else
            {
                if (CycloneActionName == "De-Planked")
                {
                    ConditionAddNoAction = "De-Planked";

                    ConditionAddNoAction = "and Status='" + ConditionAddNoAction + "'";
                }

                if (CycloneActionName == "Tied-Down")
                {
                    ConditionAddNoAction = "Tied-Down";
                    ConditionAddNoAction = "and Status='" + ConditionAddNoAction + "'";
                }
                //string query2 = "select Right ('000000' + convert (varchar, [ScaffRef]),6) as ScaffRef,Description,Location,BuildingNo,AssetNo,FloorLevel,DesignType,MaterialType, TotalLabourDismantleEdit,PriorityRiskRating from VW_TblRegister where clientid='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and DateOfDismantle is NULL and(DateTagged <> '01/01/1900' and DateTagged is not null) and('" + siteid + "' = 'All'  or SiteLocId = '" + siteid + "') and ('" + jobselect + "' = 'All'  or LocationId = '" + jobselect + "') and  ('" + SubOwner.Replace("'", "''") + "' = 'All'  or Case when ScaffoldOwner = 'C&I' then 'C&I' else 'Maintenance' End = '" + SubOwner.Replace("'", "''") + "') " + ConditionAddNoAction + " and(Status = 'De-Planked' or Status = 'Tie-Down') and(CycloneAction = 'De-Plank Only' or CycloneAction = 'Tie-Down') Order by Location,BuildingNo";
                //query = query2;
                string query2 = "select Right ('000000' + convert (varchar, [ScaffRef]),6) as ScaffRef,Description,Location,BuildingNo,AssetNo,FloorLevel,DesignType,MaterialType, TotalLabourDismantleEdit,PriorityRiskRating from VW_TblRegister where clientid='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and DateOfDismantle is NULL and(DateTagged <> '01/01/1900' and DateTagged is not null) and('" + siteid + "' = 'All'  or SiteLocId = '" + siteid + "') and('" + ddlRiskRating + "' = 'All'  or PriorityRiskRating = '" + ddlRiskRating + "') and ('" + jobselect + "' = 'All'  or LocationId = '" + jobselect + "')" + ConditionAddNoAction + " Order by Location,BuildingNo";
                query = query2;
            }
            DataTable dt = databaseCon.GetData(query);

            iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, -50f, -50f, 30f, 30f);
            iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, Response.OutputStream);
            Response.ContentType = ("application/Pdf");
            HeaderFooter2 evnt = new HeaderFooter2();
            writer.PageEvent = evnt;
            evnt.CycloneAction = CycloneAction;
            document.Open();


            var heading1 = FontFactory.GetFont("Arial", 18f, Font.NORMAL, BaseColor.BLACK);
            var heading2 = FontFactory.GetFont("tahoma", 10f, Font.NORMAL, BaseColor.BLACK);
            var font = FontFactory.GetFont("Arial", 10f, Font.BOLD, BaseColor.BLACK);
            var heading3 = FontFactory.GetFont("Arial", 18f, Font.BOLD, BaseColor.BLACK);
            Phrase phrase;
            PdfPCell cell;

            PdfPTable headertable1 = new PdfPTable(10);
            headertable1.SetWidths(new int[] { 8, 20, 15, 12, 9, 10, 10, 10, 10, 10 });


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

                phrase = new Phrase();
                phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["TotalLabourDismantleEdit"]), heading2));
                cell = new PdfPCell(phrase);
                cell.PaddingBottom = 15f;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 1;
                cell.Colspan = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                headertable1.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["PriorityRiskRating"]), heading2));
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
            public string CycloneAction { get; set; }
            public override void OnStartPage(PdfWriter writer, Document document)
            {
                //string CycloneAction;
                // CycloneAction = databaseCon.Decrypt(HttpContext.Current.Request.QueryString["CycloneAction"]);
                //CycloneAction = databaseCon.Decrypt(CycloneAction);

                var heading1 = FontFactory.GetFont("Arial", 18f, Font.NORMAL, BaseColor.BLACK);
                var heading2 = FontFactory.GetFont("tahoma", 10f, Font.NORMAL, BaseColor.BLACK);
                var font = FontFactory.GetFont("Arial", 10f, Font.BOLD, BaseColor.BLACK);
                var heading3 = FontFactory.GetFont("tahoma", 18f, Font.BOLD, BaseColor.BLACK);
                Phrase phrase;
                PdfPCell cell;

                PdfPTable headertable1 = new PdfPTable(10);
                headertable1.SetWidths(new int[] { 8, 20, 15, 12, 9, 10, 10, 10, 10, 10 });

                phrase = new Phrase();
                phrase.Add(new Chunk("Scaffolds with cyclone action set as ", heading1));
                phrase.Add(new Chunk(CycloneAction, heading3));
                cell = new PdfPCell(phrase);
                cell.Border = 0;
                cell.Colspan = 10;
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
                phrase.Add(new Chunk(" ", heading2));
                cell = new PdfPCell(phrase);
                cell.Colspan = 10;
                cell.PaddingBottom = 6f;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable1.AddCell(cell);



                phrase = new Phrase();
                phrase.Add(new Chunk("", heading2));
                cell = new PdfPCell(phrase);
                cell.Colspan = 10;
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

                phrase = new Phrase();
                phrase.Add(new Chunk("Prep Hours", font));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 3;
                cell.PaddingBottom = 6f;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Risk Rating", font));
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