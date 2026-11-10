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
    public class SiteInventoryReportController : Controller
    {
        float Totalquantity = 0;
        float WeightTotal = 0;
        // GET: SiteInventoryReport
        public ActionResult Index()
        {
            return View();
        }

        //  public void SiteInventoryReport(string id, string SiteId)

        public void SiteInventoryReport(string UID, string UserRole, string ClientId, string SiteId,string MaterialType)
        {
            int id = Convert.ToInt32(databaseCon.Decrypt(UID));
            int SiteID = Convert.ToInt32(databaseCon.Decrypt(SiteId));
            int ClientID = Convert.ToInt32(databaseCon.Decrypt(ClientId));
            string URole = databaseCon.Decrypt(UserRole);
            string query = "";

            //Change Query On 25 June 2019

            // query = "select SiteId,MaterialType,ItemCode,ItemDesc,OrgSiteQuantity,isnull(QuantityToBeSent,0) as QuantityToBeSent,convert(int,isnull(AvailableQuantity,0)) as AvailableQuantity, ItemWeight , round(AvailableQuantity*ItemWeight,2) as TotalWeight, convert(int,isnull(QuantityReturned,0)) as QuantityReturned,isnull(QuantityReturnedToDepot,0) as QuantityReturnedToDepot,SiteDamagedQuantityReturnToDepot,JobQuantityDamaged from Vw_SiteInventory left join tblInventory on tblInventory.InventoryID=Vw_SiteInventory.InventoryID where ClientID=" + databaseCon.Decrypt(Request.QueryString["id"]) + " and SiteId=" + SiteId + "order by MaterialType,ItemCode";

            // Query Change On 11 Feb 2020 Add Condition For All Site Inventory Data

            // query = "select SiteId,MaterialType,ItemCode,ItemDesc,OrgSiteQuantity,isnull(QuantityToBeSent,0) as QuantityToBeSent,convert(int,isnull(AvailableQuantity,0)) as AvailableQuantity, ItemWeight ,convert(int,isnull(OrgSiteQuantity,0)-isnull(QuantityReturnedToDepot,0))as OnSite, round((isnull(OrgSiteQuantity,0)-isnull(QuantityReturnedToDepot,0))*ItemWeight,2) as TotalWeight, convert(int,isnull(QuantityReturned,0)) as QuantityReturned,isnull(QuantityReturnedToDepot,0) as QuantityReturnedToDepot,SiteDamagedQuantityReturnToDepot,JobQuantityDamaged from Vw_SiteInventory left join tblInventory on tblInventory.InventoryID=Vw_SiteInventory.InventoryID where ClientID=" + databaseCon.Decrypt(Request.QueryString["id"]) + " and SiteId=" + SiteId + "order by MaterialType,ItemCode";

            // Query Change On 15 Feb 2020 Add Condition For All Site Inventory Data

            // query = "select SiteId,MaterialType,ItemCode,ItemDesc,OrgSiteQuantity,isnull(QuantityToBeSent,0) as QuantityToBeSent,convert(int,isnull(AvailableQuantity,0)) as AvailableQuantity, ItemWeight ,convert(int,isnull(OrgSiteQuantity,0)-isnull(QuantityReturnedToDepot,0))as OnSite, round((isnull(OrgSiteQuantity,0)-isnull(QuantityReturnedToDepot,0))*ItemWeight,2) as TotalWeight, convert(int,isnull(QuantityReturned,0)) as QuantityReturned,isnull(QuantityReturnedToDepot,0) as QuantityReturnedToDepot,SiteDamagedQuantityReturnToDepot,JobQuantityDamaged from Vw_SiteInventory left join tblInventory on tblInventory.InventoryID=Vw_SiteInventory.InventoryID where ClientID=" + databaseCon.Decrypt(Request.QueryString["id"]) + " and (SiteId = '" + SiteId + "' or '0' = '" + SiteId + "') order by MaterialType,ItemCode";
            // Site ID 0 for All Site Inventory Data Get

            //query = "Select SiteId,MaterialType, ItemCode,ItemDesc,Sum(OrgSiteQuantity)OrgSiteQuantity,Sum(QuantityToBeSent)QuantityToBeSent,Sum(AvailableQuantity)AvailableQuantity,ItemWeight ,Sum(TotalWeight)TotalWeight,Sum(QuantityReturned)QuantityReturned,Sum(QuantityReturnedToDepot)QuantityReturnedToDepot,Sum(flag)flag,Sum(SiteDamagedQuantityReturnToDepot)SiteDamagedQuantityReturnToDepot,Sum(JobQuantityDamaged)JobQuantityDamaged,Sum(OnSite)OnSite from(select Case when '0' = '" + SiteID + "' then 0 else SiteId End SiteId, MaterialType, ItemCode, ItemDesc, OrgSiteQuantity, isnull(QuantityToBeSent, 0) as QuantityToBeSent, convert(int, isnull(AvailableQuantity, 0)) as AvailableQuantity, ItemWeight, convert(decimal(18, 2), (isnull(OrgSiteQuantity, 0) - isnull(QuantityReturnedToDepot, 0)) * ItemWeight) as TotalWeight, convert(int, isnull(QuantityReturned, 0)) as QuantityReturned, QuantityReturnedToDepot, flag, SiteDamagedQuantityReturnToDepot, JobQuantityDamaged, convert(int, (isnull(OrgSiteQuantity, 0) - isnull(QuantityReturnedToDepot, 0))) as OnSite from Vw_SiteInventory left join tblInventory on tblInventory.InventoryID = Vw_SiteInventory.InventoryID where ClientID =" + ClientID + " and(SiteId = '" + SiteID + "' or '0' = '" + SiteID + "')   and ('All' = '"+ MaterialType.Replace("'", "''").Trim() + "' or tblInventory.MaterialType ='"+ MaterialType.Replace("'", "''").Trim() + "' ) and SendQuantity is not null) AllSites Group by SiteId, MaterialType, ItemCode, ItemDesc, ItemWeight order by MaterialType,ItemCode";
            query = "Select SiteId,MaterialType, ItemCode,ItemDesc,Sum(OrgSiteQuantity)OrgSiteQuantity,Sum(QuantityToBeSent)QuantityToBeSent,Sum(AvailableQuantity)AvailableQuantity,ItemWeight ,Sum(TotalWeight)TotalWeight,Sum(QuantityReturned)QuantityReturned,Sum(QuantityReturnedToDepot)QuantityReturnedToDepot,Sum(flag)flag,Sum(SiteDamagedQuantityReturnToDepot)SiteDamagedQuantityReturnToDepot,Sum(JobQuantityDamaged)JobQuantityDamaged,Sum(OnSite)OnSite ,ISNULL(DisplaySortOrder, 9999999) AS DisplaySortOrder from(select Case when '0' = '" + SiteID + "' then 0 else SiteId End SiteId, MaterialType, ItemCode, ItemDesc, OrgSiteQuantity, isnull(QuantityToBeSent, 0) as QuantityToBeSent, convert(int, isnull(AvailableQuantity, 0)) as AvailableQuantity, ItemWeight, convert(decimal(18, 2), (isnull(OrgSiteQuantity, 0) - isnull(QuantityReturnedToDepot, 0)) * ItemWeight) as TotalWeight, convert(int, isnull(QuantityReturned, 0)) as QuantityReturned, QuantityReturnedToDepot, flag, SiteDamagedQuantityReturnToDepot, JobQuantityDamaged, convert(int, (isnull(OrgSiteQuantity, 0) - isnull(QuantityReturnedToDepot, 0))) as OnSite ,tblInventory.DisplaySortOrder from Vw_SiteInventory left join tblInventory on tblInventory.InventoryID = Vw_SiteInventory.InventoryID where ClientID =" + ClientID + " and(SiteId = '" + SiteID + "' or '0' = '" + SiteID + "')   and ('All' = '"+ MaterialType.Replace("'", "''").Trim() + "' or tblInventory.MaterialType ='"+ MaterialType.Replace("'", "''").Trim() + "' ) and SendQuantity is not null) AllSites Group by SiteId, MaterialType, ItemCode, ItemDesc, ItemWeight ,DisplaySortOrder order by MaterialType,ISNULL(DisplaySortOrder, 9999999),ItemCode";
            // Site ID 0 for All Site Inventory Data Get
            DataTable dt = databaseCon.GetData(query);

            //Changing the pdf orientation potrait to landscape on 18 March 2020
            iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.LETTER, -110f, -110f, 50f, 50f);
            doc.SetPageSize(new Rectangle(990, 600));

            //iTextSharp.text.Document doc = new iTextSharp.text.Document(PageSize.A4, -50f, -50f, 20f, 20f);   //Page Size set top ,left,margin
            iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, Response.OutputStream);
            //Document doc = new Document(PageSize.A4, -50f, -50f, 20f, 20f);   //Page Size set top ,left,margin
            //doc.SetPageSize(iTextSharp.text.PageSize.A4);

            //PdfWriter writer = PdfWriter.GetInstance(doc, Response.OutputStream);
            Response.ContentType = ("application/pdf");
            HeaderFooter2 evnt = new HeaderFooter2();
            writer.PageEvent = evnt;
            evnt.ClientID = ClientID;
            evnt.SiteID = SiteID;

            doc.Open();
            var Heading786 = FontFactory.GetFont("Arial", 10f, Font.BOLD, BaseColor.BLACK);
            //Heading786.Color = BaseColor.GRAY;
            var Heading787 = FontFactory.GetFont("Arial", 20f, Font.BOLD, BaseColor.BLACK);
            var DataFont = FontFactory.GetFont("Arial", 10f, Font.NORMAL, BaseColor.BLACK);
            Phrase phrase;
            PdfPCell cell;

            PdfPTable headertable1 = new PdfPTable(5);
            headertable1.SetWidths(new int[] { 10, 9, 25, 9, 9 });

            //Table Start

            //PdfPTable headertable2 = new PdfPTable(12);
            //  headertable2.SetWidths(new int[] { 13, 9, 21, 9, 9, 9, 9, 10, 9, 9,9,9 });

            PdfPTable headertable2 = new PdfPTable(11);
            headertable2.SetWidths(new int[] { 10, 9, 20, 7, 7, 9, 10, 10, 10, 9, 9 });
            //Table End

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["MaterialType"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["ItemCode"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["ItemDesc"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    phrase = new Phrase();
                    string OrgSiteqty = String.Format("{0:#,##0}", dt.Rows[i]["OrgSiteQuantity"]);
                    phrase.Add(new Chunk(OrgSiteqty, DataFont));
                    //phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["OrgSiteQuantity"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    phrase = new Phrase();
                    string QtyToSend = String.Format("{0:#,##0}", dt.Rows[i]["QuantityToBeSent"]);
                    phrase.Add(new Chunk(QtyToSend, DataFont));
                    //phrase.Add(new Chunk(Convert.ToInt32(dt.Rows[i]["QuantityToBeSent"]).ToString(), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    //phrase = new Phrase();
                    //string qtyReturn = String.Format("{0:#,##0}", dt.Rows[i]["QuantityReturned"]);
                    //phrase.Add(new Chunk(qtyReturn, DataFont));
                    ////phrase.Add(new Chunk(Convert.ToInt32(dt.Rows[i]["QuantityReturned"]).ToString(), DataFont));
                    //cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 15f;
                    //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //cell.Border = 1;
                    //cell.Colspan = 1;
                    //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    //headertable2.AddCell(cell);

                    phrase = new Phrase();
                    string qtyReturndeport = String.Format("{0:#,##0}", dt.Rows[i]["QuantityReturnedToDepot"]);
                    phrase.Add(new Chunk(qtyReturndeport, DataFont));
                    //phrase.Add(new Chunk(Convert.ToInt32(dt.Rows[i]["QuantityReturned"]).ToString(), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    phrase = new Phrase();
                    string qtyDamagedJob = String.Format("{0:#,##0}", dt.Rows[i]["JobQuantityDamaged"]);
                    phrase.Add(new Chunk(qtyDamagedJob, DataFont));
                    //phrase.Add(new Chunk(Convert.ToInt32(dt.Rows[i]["QuantityReturned"]).ToString(), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    phrase = new Phrase();
                    string qtyDamageddeport = String.Format("{0:#,##0}", dt.Rows[i]["SiteDamagedQuantityReturnToDepot"]);
                    phrase.Add(new Chunk(qtyDamageddeport, DataFont));
                    //phrase.Add(new Chunk(Convert.ToInt32(dt.Rows[i]["QuantityReturned"]).ToString(), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    // Change ON 25 June 2019
                    //phrase = new Phrase();
                    //string Allocatedqty = String.Format("{0:#,##0}", dt.Rows[i]["AvailableQuantity"]);
                    //phrase.Add(new Chunk(Allocatedqty, DataFont));
                    ////phrase.Add(new Chunk(Convert.ToInt32(dt.Rows[i]["AvailableQuantity"]).ToString(), DataFont));
                    //cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 15f;
                    //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //cell.Border = 1;
                    //cell.Colspan = 1;
                    //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    //headertable2.AddCell(cell);
                    //Totalquantity += Convert.ToInt32(dt.Rows[i]["AvailableQuantity"]);
                    ////cell.BackgroundColor = BaseColor.LIGHT_GRAY;

                    phrase = new Phrase();
                    string onSite = String.Format("{0:#,##0}", dt.Rows[i]["OnSite"]);
                    phrase.Add(new Chunk(onSite, DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);
                    Totalquantity += Convert.ToInt32(dt.Rows[i]["OnSite"]);

                    phrase = new Phrase();
                    string itemweight = String.Format("{0:#,##0.00}", dt.Rows[i]["ItemWeight"]);
                    phrase.Add(new Chunk(itemweight, DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    phrase = new Phrase();
                    string Totalweight = String.Format("{0:#,##0.00}", dt.Rows[i]["TotalWeight"]);
                    phrase.Add(new Chunk(Totalweight, DataFont));
                    //phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["TotalWeight"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["TotalWeight"])))
                    {
                        WeightTotal += Convert.ToInt32(dt.Rows[i]["TotalWeight"]);
                    }

                }

            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 11;
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
            string Totalqty = string.Format("{0:#,##0}", Totalquantity);
            phrase.Add(new Chunk(Totalqty.ToString(), DataFont));
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
            cell.Colspan = 1;
            cell.PaddingBottom = 6F;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable2.AddCell(cell);

            phrase = new Phrase();

            decimal Total = Convert.ToDecimal(WeightTotal / 1000);
            string TotalWe = string.Format("{0:#,##0.00}", Total);
            phrase.Add(new Chunk(TotalWe.ToString(), DataFont));
            phrase.Add(new Chunk(" Tonnes", DataFont));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.Border = 3;
            cell.PaddingBottom = 6F;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            headertable2.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk("", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 10;
            //cell.PaddingBottom = 6f;
            //cell.Border = 0;
            //cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //headertable2.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk("", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 10;
            //cell.PaddingBottom = 6f;
            //cell.Border = 0;
            //cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //headertable2.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk("Printed:", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.PaddingBottom = 6f;
            //cell.Border = 0;
            //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //headertable2.AddCell(cell);

            //phrase = new Phrase();
            //phrase.Add(new Chunk("" + DateTime.Now.ToString("dd-MMM-yyy") + "", DataFont));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 9;
            //cell.PaddingBottom = 6f;
            //cell.Border = 0;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //headertable2.AddCell(cell);

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
                
                // string queryheader = "select ClientImage,SiteLocation from tblClient join tblSiteLocation on tblclient.clientId = tblsitelocation.clientId where tblClient.ClientId  = '" + ID + "' and tblsitelocation.sitelocid='" + SiteID + "'";
                // Query Change On 11 Feb 2020 Add Condition For All Site Inventory Data
                string queryheader = "select distinct ClientImage,Case when '0'='" + SiteID + "' then 'All' Else SiteLocation End as SiteLocation from tblClient join tblSiteLocation on tblclient.clientId = tblsitelocation.clientId where tblClient.ClientId  = '" + ClientID + "' and (tblsitelocation.sitelocid='" + SiteID + "' or '0'='" + SiteID + "')";
                DataTable dtheader = databaseCon.GetData(queryheader);
                var Heading786 = FontFactory.GetFont("Arial", 10f, Font.BOLD, BaseColor.BLACK);
                //Heading786.BaseColor = BaseColor.GRAY;
                var Heading787 = FontFactory.GetFont("Arial", 20f, Font.BOLD, BaseColor.BLACK);
                var DataFont = FontFactory.GetFont("Arial", 10f, Font.NORMAL, BaseColor.BLACK);
                Phrase phrase;
                PdfPCell cell;

                PdfPTable headertable1 = new PdfPTable(5);
                headertable1.SetWidths(new int[] { 10, 9, 25, 9, 9 });

                //Table Start

                //PdfPTable headertable2 = new PdfPTable(12);
                //headertable2.SetWidths(new int[] { 13, 9, 21, 9, 9, 9, 9, 10, 9, 9,9,9 });

                PdfPTable headertable2 = new PdfPTable(11);
                headertable2.SetWidths(new int[] { 10, 9, 20, 7, 7, 9, 10, 10, 10, 9, 9 });

                //phrase = new Phrase();
                //phrase.Add(new Chunk("", Heading786));
                //cell = new PdfPCell(phrase);
                //cell.Colspan = 5;
                //cell.PaddingBottom = 6f;
                //cell.Border = 0;
                //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                //headertable1.AddCell(cell);

                //phrase = new Phrase();
                //phrase.Add(new Chunk("", Heading786));
                //cell = new PdfPCell(phrase);
                //cell.Colspan = 5;
                //cell.PaddingBottom = 6f;
                //cell.Border = 0;
                //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                //headertable1.AddCell(cell);


                if (!string.IsNullOrEmpty(Convert.ToString(dtheader.Rows[0]["ClientImage"])))
                {
                    var uri = new Uri(databaseCon.clientimageLink);
                    // var container = new CloudBlobContainer(uri);
                    //var blob = container.GetBlockBlobReference(dtheader.Rows[0]["ClientImage"].ToString());

                    //var noimage = System.Web.HttpContext.Current.Server.MapPath("Images/Logo2.png");
                    var noimage = System.Web.HttpContext.Current.Server.MapPath("~/Images/Logo2.png");

                    // string photo = System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/uncheckbox.png");
                    // cell = new PdfPCell(iTextSharp.text.Image.GetInstance(photo), true);

                    //var exists = blob.Exists();
                    if (Convert.ToString(dtheader.Rows[0]["ClientImage"]) == "")
                    {
                        //string nophoto = System.Web.HttpContext.Current.Server.MapPath("images/Logo2.png");
                        string nophoto = System.Web.HttpContext.Current.Server.MapPath("~/images/Logo2.png");
                        cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
                        cell.FixedHeight = 50f;
                        cell.Colspan = 2;
                        //cell.PaddingLeft = 10f;
                        cell.PaddingTop = -10f;
                    }
                    else
                    {
                        // iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(System.Web.HttpContext.Current.Server.MapPath("images/clientlogo/" + dtheader.Rows[0]["ClientImage"].ToString()));
                        
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
                    //string nophoto = System.Web.HttpContext.Current.Server.MapPath("images/Logo2.png");
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
                phrase.Add(new Chunk("Stock in Yard Report", Heading787));
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
                cell.Colspan = 6;
                //cell.PaddingBottom = 6f;
                //cell.PaddingLeft = 100f;
                //cell.PaddingTop = 30f;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Site:", Heading786));
                //phrase.Add(new Chunk(Convert.ToString(dtheader.Rows[0]["SiteName"].ToString()), Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
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
                cell.Colspan = 5;
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
                cell.Colspan = 6;
                cell.PaddingBottom = 6f;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Date:", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                //cell.PaddingBottom = 6f;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("" + DateTime.Now.ToString("dd-MMM-yyy") + "", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 5;
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
                phrase.Add(new Chunk("", Heading786));
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
                phrase.Add(new Chunk("Qty in Yard", Heading786));
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

                //phrase = new Phrase();
                //phrase.Add(new Chunk("Return Quantity To Job", Heading786));
                //cell = new PdfPCell(phrase);
                //cell.Colspan = 1;
                //cell.Border = 1;
                //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                //headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Return Qty To Depot", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Damaged Qty From Job", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);


                phrase = new Phrase();
                phrase.Add(new Chunk("Damaged Qty To Depot", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                // Change ON 25 June 2019
                //phrase = new Phrase();
                //phrase.Add(new Chunk("Available Qty", Heading786));
                //cell = new PdfPCell(phrase);
                //cell.Colspan = 1;
                //cell.Border = 1;
                //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                //headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("On Site", Heading786));
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