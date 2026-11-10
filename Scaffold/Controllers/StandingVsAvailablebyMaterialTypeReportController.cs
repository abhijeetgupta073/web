using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Scaffold.App_Code;
using System.Configuration;

namespace Scaffold.Controllers
{
    public class StandingVsAvailablebyMaterialTypeReportController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowMaterialReport(int? clientid, int? Sitelocid, string Type, string MaterialType = "ALL")
        {
            if (!clientid.HasValue || !Sitelocid.HasValue)
            {
                return new HttpStatusCodeResult(400, "Missing required parameters");
            }

            DataTable dt = GetMaterialData(clientid.Value, Sitelocid.Value, Type, MaterialType);
            iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4.Rotate(), 30f, 30f, 30f, 30f);
            iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, Response.OutputStream);
            Response.ContentType = ("application/pdf");

            MaterialReportHeader evnt = new MaterialReportHeader();
            writer.PageEvent = evnt;
            evnt.MaterialType = MaterialType;
            evnt.GraphType = Type;

            document.Open();

            var heading1 = FontFactory.GetFont("Arial", 18f, Font.NORMAL, BaseColor.BLACK);
            var heading2 = FontFactory.GetFont("tahoma", 10f, Font.NORMAL, BaseColor.BLACK);
            var font = FontFactory.GetFont("Arial", 10f, Font.BOLD, BaseColor.BLACK);
            var heading3 = FontFactory.GetFont("Arial", 18f, Font.BOLD, BaseColor.BLACK);
            var headerFont = FontFactory.GetFont("Arial", 10f, Font.BOLD, BaseColor.WHITE);

            Phrase phrase;
            PdfPCell cell;

            PdfPTable dataTable = new PdfPTable(6);
            dataTable.SetWidths(new int[] { 15, 15, 30, 15, 12, 13 });
            dataTable.WidthPercentage = 100;

            decimal totalTonnage = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                phrase = new Phrase();
                phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["MaterialType"]), heading2));
                cell = new PdfPCell(phrase);
                cell.PaddingBottom = 10f;
                cell.PaddingTop = 5f;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.BOX;
                dataTable.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["ItemCode"]), heading2));
                cell = new PdfPCell(phrase);
                cell.PaddingBottom = 10f;
                cell.PaddingTop = 5f;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.BOX;
                dataTable.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["ItemDesc"]), heading2));
                cell = new PdfPCell(phrase);
                cell.PaddingBottom = 10f;
                cell.PaddingTop = 5f;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                dataTable.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["AvailableQuantity"]), heading2));
                cell = new PdfPCell(phrase);
                cell.PaddingBottom = 10f;
                cell.PaddingTop = 5f;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.BOX;
                dataTable.AddCell(cell);

                phrase = new Phrase();
                decimal itemWeight = Convert.ToDecimal(dt.Rows[i]["ItemWeight"]);
                phrase.Add(new Chunk(itemWeight.ToString("N2"), heading2));
                cell = new PdfPCell(phrase);
                cell.PaddingBottom = 10f;
                cell.PaddingTop = 5f;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.BOX;
                dataTable.AddCell(cell);

                phrase = new Phrase();
                string tonnageColumn = (Type == "StandingInfo") ? "TonnageStanding" : "TonnageAvailable";
                decimal tonnage = Convert.ToDecimal(dt.Rows[i][tonnageColumn]);
                totalTonnage += tonnage;
                phrase.Add(new Chunk(tonnage.ToString("N2"), heading2));
                cell = new PdfPCell(phrase);
                cell.PaddingBottom = 10f;
                cell.PaddingTop = 5f;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.BOX;
                dataTable.AddCell(cell);
            }

            phrase = new Phrase();
            phrase.Add(new Chunk("Total Tonnage:", font));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.PaddingBottom = 10f;
            cell.PaddingTop = 5f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.Border = PdfPCell.BOX;
            dataTable.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk(totalTonnage.ToString("N2"), font));
            cell = new PdfPCell(phrase);
            cell.PaddingBottom = 10f;
            cell.PaddingTop = 5f;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.Border = PdfPCell.BOX;
            dataTable.AddCell(cell);

            document.Add(dataTable);
            document.Close();

            return new EmptyResult();
        }

        // Method to get data from stored procedure
        private DataTable GetMaterialData(int clientId, int siteLocId, string type, string MaterialType = "ALL")
        {
            DataTable dt = new DataTable();

            string con = null;
            try
            {
                con = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString;
                using (SqlConnection sqlCon = new SqlConnection(con))
                {
                    using (SqlCommand cmd = new SqlCommand("pro_StandingVsAvailablebyMatType", sqlCon))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@clientid", clientId);
                        cmd.Parameters.AddWithValue("@Sitelocid", siteLocId);
                        cmd.Parameters.AddWithValue("@Type", type);
                        cmd.Parameters.AddWithValue("@MaterialType", MaterialType);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error
                throw new Exception("Error fetching material data: " + ex.Message);
            }

            return dt;
        }

        // Header and Footer class
        public class MaterialReportHeader : iTextSharp.text.pdf.PdfPageEventHelper
        {
            public string MaterialType { get; set; }
            public string GraphType { get; set; }

            public override void OnStartPage(PdfWriter writer, Document document)
            {
                var heading1 = FontFactory.GetFont("Arial", 18f, Font.NORMAL, BaseColor.BLACK);
                var heading2 = FontFactory.GetFont("tahoma", 10f, Font.NORMAL, BaseColor.BLACK);
                var font = FontFactory.GetFont("Arial", 10f, Font.BOLD, BaseColor.BLACK);
                var heading3 = FontFactory.GetFont("tahoma", 18f, Font.BOLD, BaseColor.BLACK);

                Phrase phrase;
                PdfPCell cell;

                PdfPTable headerTable = new PdfPTable(6);
                headerTable.SetWidths(new int[] { 15, 15, 30, 15, 12, 13 });
                headerTable.WidthPercentage = 100;

                // Title Row
                phrase = new Phrase();
                string titlePrefix = GraphType == "Standing" ? "Standing Material Details for " : "Available Material Details for ";
                phrase.Add(new Chunk(titlePrefix, heading1));
                phrase.Add(new Chunk(MaterialType, heading3));
                cell = new PdfPCell(phrase);
                cell.Border = 0;
                cell.Colspan = 6;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.PaddingBottom = 10f;
                headerTable.AddCell(cell);

                // Empty Row
                phrase = new Phrase();
                phrase.Add(new Chunk(" ", heading2));
                cell = new PdfPCell(phrase);
                cell.Colspan = 6;
                cell.PaddingBottom = 10f;
                cell.Border = 0;
                headerTable.AddCell(cell);

                // Column Headers
                // Material Type Header
                phrase = new Phrase();
                phrase.Add(new Chunk("Material Type", font));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.PaddingBottom = 8f;
                cell.PaddingTop = 5f;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.BOTTOM_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headerTable.AddCell(cell);

                // Item Code Header
                phrase = new Phrase();
                phrase.Add(new Chunk("Item Code", font));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.PaddingBottom = 8f;
                cell.PaddingTop = 5f;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.BOTTOM_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headerTable.AddCell(cell);

                // Item Description Header
                phrase = new Phrase();
                phrase.Add(new Chunk("Item Description", font));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.PaddingBottom = 8f;
                cell.PaddingTop = 5f;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.BOTTOM_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headerTable.AddCell(cell);

                // Available Quantity Header
                phrase = new Phrase();
                phrase.Add(new Chunk("Available Quantity", font));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.PaddingBottom = 8f;
                cell.PaddingTop = 5f;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.BOTTOM_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headerTable.AddCell(cell);

                // Item Weight Header
                phrase = new Phrase();
                phrase.Add(new Chunk("Item Weight (kg)", font));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.PaddingBottom = 8f;
                cell.PaddingTop = 5f;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.BOTTOM_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headerTable.AddCell(cell);

                // Tonnage Header
                phrase = new Phrase();
                phrase.Add(new Chunk("Tonnage", font));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.PaddingBottom = 8f;
                cell.PaddingTop = 5f;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.BOTTOM_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headerTable.AddCell(cell);

                document.Add(headerTable);
            }

            //public override void OnEndPage(PdfWriter writer, Document document)
            //{
            //    Paragraph footerdate = new Paragraph(
            //        "Printed: " + DateTime.Now.ToString("dd-MMM-yyyy HH:mm"),
            //        FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)
            //    );

            //    PdfPCell cell;
            //    PdfPTable footerTable = new PdfPTable(1);
            //    footerTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            //    footerTable.HorizontalAlignment = Element.ALIGN_RIGHT;

            //    cell = new PdfPCell(footerdate);
            //    cell.Border = 0;
            //    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //    footerTable.AddCell(cell);
            //    footerTable.WriteSelectedRows(
            //        0, -1,
            //        document.RightMargin,
            //        document.BottomMargin + 20,
            //        writer.DirectContent
            //    );
            //}

            public override void OnEndPage(PdfWriter writer, Document document)
            {

                Paragraph footerdate = new Paragraph(
                    "Printed: " + DateTime.Now.ToString("dd-MMM-yyyy"),
                    FontFactory.GetFont(FontFactory.TIMES, 9, Font.BOLD, BaseColor.BLACK)
                );

                PdfPTable footerTable = new PdfPTable(1);
                footerTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;

                PdfPCell cell = new PdfPCell(footerdate)
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    PaddingTop = 0,
                    PaddingBottom = 0
                };

                footerTable.AddCell(cell);

                float xPos = document.LeftMargin;
                float yPos = document.BottomMargin - 10; 

                footerTable.WriteSelectedRows(0, -1, xPos, yPos, writer.DirectContent);
            }
        }
    }
}