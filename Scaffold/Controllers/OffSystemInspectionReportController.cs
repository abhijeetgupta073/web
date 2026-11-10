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
    public class OffSystemInspectionReportController : Controller
    {
        // GET: OffSystemInspectionReport
        public ActionResult Index()
        {
            return View();
        }


        public void showreport(string id, string SiteId, string LocId, string subowner, string InspectionDate, string hdnTimeOffSet, string BuildingId, string FloorLevel, string Workgroup, string Bearing)
        {
            string query = "";

            query = "exec [Pro_TBLREGISTER_GET] '','" + databaseCon.Decrypt(id);

            if (SiteId != "")
            {
                query = query + "','" + SiteId + "";

            }
            else
            {
                query = query + "','";

            }

            if (LocId != "")
            {
                query = query + "','" + LocId + "";
            }

            else
            {
                query = query + "','";
            }

            if (subowner != "")
            {
                query = query + "','','" + subowner + "";
            }
            else
            {
                // query = query + "','";
                query = query + "','','";
            }
            //query = query + "','','1' ";


            //query = query + "','1'," + Convert.ToDouble((hdnTimeOffSet)) * (-1) + "";

            query = query + "','1'," + Convert.ToDouble((hdnTimeOffSet)) * (-1) + "";

            if (Workgroup != "")
            {
                query = query + ",'" + Workgroup + "','','";
            }
            else
            {
                query = query + ",'','','";
            }
            if (BuildingId != "")
            {
                query = query + "','" + BuildingId + "";
            }

            else
            {
                query = query + "','";
            }
            if (FloorLevel != "" && FloorLevel != null)
            {
                query = query + "','" + FloorLevel + "";
            }

            else
            {
                query = query + "','";
            }
            if (Bearing != "" && Bearing != null)
            {
                query = query + "','" + Bearing + "'";
            }

            else
            {
                query = query + "',''";
            }
            DataTable dt = databaseCon.GetData(query);

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
            evnt.Site = SiteId;
            evnt.InspectionDate = InspectionDate;

            writer.PageEvent = evnt;
            doc.Open();
            var Heading786 = FontFactory.GetFont("Arial", 10f, Font.BOLD, BaseColor.BLACK);
            //Heading786.Color = Color.GRAY;
            var Heading787 = FontFactory.GetFont("Arial", 20f, Font.BOLD, BaseColor.BLACK);
            var DataFont = FontFactory.GetFont("Arial", 10f, Font.NORMAL, BaseColor.BLACK);
            Phrase phrase;
            PdfPCell cell;

            PdfPTable headertable1 = new PdfPTable(5);
            headertable1.SetWidths(new int[] { 10, 9, 25, 9, 9 });

            //Table Start      
            // PdfPTable headertable2 = new PdfPTable(10);
            // headertable2.SetWidths(new int[] { 5, 12, 6, 9, 9, 6, 8, 4, 4, 19 });
            //Table End

            //Added on 13-09-2023 because added one column ClientRef 
            //PdfPTable headertable2 = new PdfPTable(11);
            PdfPTable headertable2 = new PdfPTable(12);

            //headertable2.SetWidths(new int[] { 5, 5, 12, 6, 9, 9, 6, 8, 4, 4, 15 });
            //Added on 13-09-2023 because added one column ClientRef 
            headertable2.SetWidths(new int[] { 5, 5, 10, 6, 9, 9, 6, 8, 4, 4, 12, 5 });

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //Added to filter only Jobs That will not have status != NotStarted  and != "Pipeline"
                    if (dt.Rows[i]["Status"].ToString() != "NotStarted" && dt.Rows[i]["Status"].ToString() != "Pipeline")
                    {
                        phrase = new Phrase();
                        phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Status"].ToString()), DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = 1;
                        cell.Colspan = 1;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable2.AddCell(cell);

                        phrase = new Phrase();
                        phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["ShowRegisterID"].ToString()), DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = 1;
                        cell.Colspan = 1;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable2.AddCell(cell);

                        phrase = new Phrase();
                        phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["SiteLocation"].ToString()), DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = 1;
                        cell.Colspan = 1;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable2.AddCell(cell);

                        phrase = new Phrase();
                        phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Location"].ToString()), DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = 1;
                        cell.Colspan = 1;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable2.AddCell(cell);

                        phrase = new Phrase();
                        phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["BuildingNo"].ToString()), DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = 1;
                        cell.Colspan = 1;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable2.AddCell(cell);

                        phrase = new Phrase();
                        phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["AssetNo"]).ToString(), DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = 1;
                        cell.Colspan = 1;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable2.AddCell(cell);

                        phrase = new Phrase();
                        phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["FloorLevel"]).ToString(), DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = 1;
                        cell.Colspan = 1;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable2.AddCell(cell);

                        


                        phrase = new Phrase();
                        //phrase.Add(new Chunk(Convert.ToDateTime(dt.Rows[i]["RecentInspDate"]).ToString(), DataFont));
                        string InspDate = String.Format("{0:dd-MMM-yyyy}", dt.Rows[i]["RecentInspDate"]);
                        phrase.Add(new Chunk(InspDate, DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = 1;
                        cell.Colspan = 1;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable2.AddCell(cell);

                        //phrase = new Phrase();
                        //string Totalweight = String.Format("{0:#,##0.00}", dt.Rows[i]["TotalWeight"]);
                        //phrase.Add(new Chunk(Totalweight, DataFont));
                        ////phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["TotalWeight"].ToString()), DataFont));
                        //cell = new PdfPCell(phrase);
                        //cell.PaddingBottom = 15f;
                        //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        //cell.Border = 1;
                        //cell.Colspan = 1;
                        //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        //headertable2.AddCell(cell);

                        phrase = new Phrase();
                        phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["Bearing"]).ToString(), DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = 1;
                        cell.Colspan = 1;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable2.AddCell(cell);


                        phrase = new Phrase();
                        phrase.Add(new Chunk());
                        //phrase.Add(new Chunk(WeeklyItemHire, DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = 1;
                        cell.Colspan = 1;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable2.AddCell(cell);

                        phrase = new Phrase();
                        phrase.Add(new Chunk());
                        // phrase.Add(new Chunk(TotalWeeklyHire, DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = 1;
                        cell.Colspan = 1;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable2.AddCell(cell);


                        //Added on 13-09-2023 added one column ClientRef 
                        phrase = new Phrase();
                        phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["ClientScaffref"]).ToString(), DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = 1;
                        cell.Colspan = 1;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable2.AddCell(cell);

                    }
                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 13;
                cell.Border = 0;
                cell.PaddingBottom = 6F;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

            }
            //phrase = new Phrase();
            //phrase.Add(new Chunk("", DataFont));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 4;
            //cell.PaddingBottom = 6F;
            //cell.Border = 0;
            //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //headertable2.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk("Total:", DataFont));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.Border = 3;
            //cell.PaddingBottom = 6F;
            //cell.Border = PdfPCell.RIGHT_BORDER;
            //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //headertable2.AddCell(cell);

            //phrase = new Phrase();
            //string Totalqty=string.Format("{0:#,##0}",Totalquantity);
            //phrase.Add(new Chunk(Totalqty.ToString(), DataFont));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.Border = 3;
            //cell.PaddingBottom = 6F;
            //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //cell.BackgroundColor = Color.LIGHT_GRAY;
            //headertable2.AddCell(cell);      

            doc.Add(headertable1);
            doc.Add(headertable2);
            doc.Close();
        }
        public class HeaderFooter2 : iTextSharp.text.pdf.PdfPageEventHelper
        {
            public string id { get; set; }
            public string Site { get; set; }
            public string InspectionDate { get; set; }
            List<PdfTemplate> pageNumberTemplates = new List<PdfTemplate>();// page header

            public override void OnStartPage(PdfWriter writer, Document doc)
            {
                //int ID = Convert.ToInt32(databaseCon.Decrypt(System.Web.HttpContext.Current.Request.QueryString["Id"]));
                //int SiteID = Convert.ToInt32(databaseCon.Decrypt(HttpContext.Current.Request.QueryString["Site"]));
                //DateTime ReportInspectDate = Convert.ToDateTime(databaseCon.Decrypt(HttpContext.Current.Request.QueryString["InspectionDate"]));

                int ID = Convert.ToInt32(databaseCon.Decrypt(id));
                int SiteID = Convert.ToInt32(Site);
                DateTime ReportInspectDate = Convert.ToDateTime(InspectionDate);

                string queryheader = "select distinct ClientImage,SiteLocID,SiteLocation from tblSiteLocation  join tblClient on tblclient.clientId = tblsitelocation.clientId where SiteLocation <>'' and SiteLocation is not null and OffSystemInspection=1 and  tblclient.ClientId='" + ID + "' and tblsitelocation.sitelocid='" + SiteID + "' order by SiteLocation";
                DataTable dtheader = databaseCon.GetData(queryheader);
                var Heading786 = FontFactory.GetFont("Arial", 10f, Font.BOLD, BaseColor.BLACK);
                var Heading787 = FontFactory.GetFont("Arial", 20f, Font.BOLD, BaseColor.BLACK);
                var DataFont = FontFactory.GetFont("Arial", 10f, Font.NORMAL, BaseColor.BLACK);
                Phrase phrase;
                PdfPCell cell;

                PdfPTable headertable1 = new PdfPTable(5);
                headertable1.SetWidths(new int[] { 10, 9, 25, 9, 9 });

                //PdfPTable headertable2 = new PdfPTable(10);
                //headertable2.SetWidths(new int[] { 5, 12, 6, 9, 9, 6, 8, 4, 4, 19 });

                //Added on 13-09-2023 added one column ClientRef 
                //PdfPTable headertable2 = new PdfPTable(11);
                PdfPTable headertable2 = new PdfPTable(12);

                //Added on 13-09-2023 added one column ClientRef 
                //headertable2.SetWidths(new int[] { 5,5, 12, 6, 9, 9, 6, 8, 4, 4, 15 });
                headertable2.SetWidths(new int[] { 5, 5, 10, 6, 9, 9, 6, 8, 4, 4, 12, 5 });


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
                phrase.Add(new Chunk("Off System Inspection List", Heading787));
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
                cell = new PdfPCell(phrase);
                //Added on 13-09-2023 because added one column ClientRef 
                //cell.Colspan = 5;
                cell.Colspan = 6;
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
                cell.Colspan = 4;
                cell.PaddingBottom = 6f;
                //cell.PaddingLeft = 100f;
                //cell.PaddingTop = 30f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading786));
                cell = new PdfPCell(phrase);
                //Added on 13-09-2023 because added one column ClientRef 
                //cell.Colspan = 11;
                cell.Colspan = 12;
                cell.PaddingBottom = 6f;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading786));
                cell = new PdfPCell(phrase);
                //Added on 13-09-2023 because added one column ClientRef 
                //cell.Colspan = 5;
                cell.Colspan = 6;
                cell.PaddingBottom = 6f;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Inspection Date:", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 2;
                //cell.PaddingBottom = 6f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk(String.Format("{0:dd-MMM-yyyy}", ReportInspectDate)));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.PaddingBottom = 6f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk(" ", Heading786));
                cell = new PdfPCell(phrase);
                //Added on 13-09-2023 because added one column ClientRef 
                //cell.Colspan = 11;
                cell.Colspan = 12;
                //cell.PaddingBottom = 6f;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk(" ", Heading786));
                cell = new PdfPCell(phrase);
                //Added on 13-09-2023 because added one column ClientRef 
                //cell.Colspan = 11;
                cell.Colspan = 12;
                //cell.PaddingBottom = 6f;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Status", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingBottom = 6f;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;

                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Scaf Ref #", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.PaddingBottom = 6f;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;

                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Site", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.PaddingBottom = 6f;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Location", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Building / Module No.", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Asset # / Func. Loc", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Floor Level", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

             

                phrase = new Phrase();
                phrase.Add(new Chunk("Recent Insp Date", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Bearing", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Out Of Service", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Update All", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);



                phrase = new Phrase();
                phrase.Add(new Chunk("ClientScaffref", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                //Added on 13-09-2023 added one column ClientRef 
                phrase = new Phrase();
                phrase.Add(new Chunk("ClientScaffref", Heading786));
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
    }
}