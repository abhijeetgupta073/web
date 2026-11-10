using iTextSharp.text;
using iTextSharp.text.pdf;
using Scaffold.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scaffold.Controllers
{
    public class DailyJobInventoryHireReportController : Controller
    {
        float Totalquantity = 0;
        float WeightTotal = 0;
        float DailyRate = 0;
        double TotalHire = 0;
        // GET: DailyJobInventoryHireReport
        public void DailyJobInventoryHireReport(string UID, string RID, string id)
        {

            string query = "";
            string queryheader = "select tblClient.ClientImage,right('000000' + convert(varchar,tblRegister.ScaffRef),6) as ScaffRef,tblRegister.Description,Plans from tblClient, tblRegister where tblClient.ClientId = tblRegister.ClientID and tblRegister.registerid = '" + databaseCon.Decrypt(RID) + "'";
            DataTable dtheader = databaseCon.GetData(queryheader);

            //query = "select DeliveryAll.FromId ,DeliveryAll.ToId,MaterialType,ItemCode,ItemDesc , DeliveryAll.InventoryId,cast(DeliveryAll.DeliveryAvailableQuantity as int) as DeliveryAvailableQuantity,isnull(cast(ReturnAll.ReturnALLQuantity as int), 0) as ReturnALLQuantity,isnull(cast(DamagedAll.DamagedAllQuantity as int), 0) as DamagedALLQuantity,cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0) - ISNULL(DamagedAll.DamagedAllQuantity, 0)) as int) as AvailableQuantity,DeliveryAll.ItemWeight,cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0) - ISNULL(DamagedAll.DamagedAllQuantity, 0)) as int) * ItemWeight as TotalWeight,DeliveryAll.SubCategoryID  ,HireMeasurement, CASE WHEN HireMeasurement = 'Tonnage' THEN convert(decimal(18, 4),((ItemWeight) / 1000) * (isnull(HireRate, 0) / 7.0)) WHEN HireMeasurement = 'Each' THEN convert(decimal(18, 4),(isnull(HireRate, 0) / 7.0)) END as DailyHireRate,CASE WHEN HireMeasurement = 'Tonnage' THEN convert(decimal(18, 4),(((cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0) - ISNULL(DamagedAll.DamagedAllQuantity, 0)) as int)) * ItemWeight) / 1000) * (isnull(HireRate, 0) / 7.0)) WHEN HireMeasurement = 'Each' THEN convert(decimal(18, 4),(cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0) - ISNULL(DamagedAll.DamagedAllQuantity, 0)) as int)) * (isnull(HireRate, 0) / 7.0))END as TotalDailyHire from(select FromId, ToId, MaterialType, ItemCode, ItemDesc, ItemWeight, tblTransactionHistDelivery.InventoryId, tblInventory.SubCategoryID, isnull(Sum(Quantity), 0) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Site' and[to] = 'Job' and TransType = 'Send' and ToId = '" + databaseCon.Decrypt(RID) + "' group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc, ItemWeight, SubCategoryID)DeliveryAll left join(select FromId, ToId, tblReturnInventoryDetails.InventoryId, isnull(Sum(Quantity), 0) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site' and FromId = '" + databaseCon.Decrypt(RID) + "' group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.InventoryId = ReturnAll.InventoryId left join(select FromId, ToId, tblDamagedInventoryDetails.InventoryId, ISNULL(Sum(DamagedQuantity), 0) as DamagedAllQuantity from tblDamagedInventoryDetails  join tblDamagedInventory on tblDamagedInventoryDetails.DamagedInvId = tblDamagedInventory.DamagedInvId where [FromName] = 'Job' and[toName] = 'Site' and FromId = '" + databaseCon.Decrypt(RID) + "' group by[FromName],[ToName], FromId, ToId, tblDamagedInventoryDetails.InventoryId) DamagedAll On DeliveryAll.FromId = DamagedAll.ToId and DeliveryAll.InventoryId = DamagedAll.InventoryId left join tblSubCategory on tblSubCategory.SubCategoryID = DeliveryAll.SubCategoryID where 1 = 1order by MaterialType,ItemCode";
            query = "select DeliveryAll.FromId ,DeliveryAll.ToId,MaterialType,ItemCode,ItemDesc , DeliveryAll.InventoryId,cast(DeliveryAll.DeliveryAvailableQuantity as int) as DeliveryAvailableQuantity,isnull(cast(ReturnAll.ReturnALLQuantity as int), 0) as ReturnALLQuantity,isnull(cast(DamagedAll.DamagedAllQuantity as int), 0) as DamagedALLQuantity,cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0) - ISNULL(DamagedAll.DamagedAllQuantity, 0)) as int) as AvailableQuantity,DeliveryAll.ItemWeight,cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0) - ISNULL(DamagedAll.DamagedAllQuantity, 0)) as int) * ItemWeight as TotalWeight,DeliveryAll.SubCategoryID  ,HireMeasurement, CASE WHEN HireMeasurement = 'Tonnage' THEN convert(decimal(18, 4),((ItemWeight) / 1000) * (isnull(HireRate, 0) / 7.0)) WHEN HireMeasurement = 'Each' THEN convert(decimal(18, 4),(isnull(HireRate, 0) / 7.0)) END as DailyHireRate,CASE WHEN HireMeasurement = 'Tonnage' THEN convert(decimal(18, 4),(((cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0) - ISNULL(DamagedAll.DamagedAllQuantity, 0)) as int)) * ItemWeight) / 1000) * (isnull(HireRate, 0) / 7.0)) WHEN HireMeasurement = 'Each' THEN convert(decimal(18, 4),(cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0) - ISNULL(DamagedAll.DamagedAllQuantity, 0)) as int)) * (isnull(HireRate, 0) / 7.0))END as TotalDailyHire from(select FromId, ToId, MaterialType, ItemCode, ItemDesc, ItemWeight, tblTransactionHistDelivery.InventoryId, tblInventory.SubCategoryID, isnull(Sum(Quantity), 0) as DeliveryAvailableQuantity ,DisplaySortOrder from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Site' and[to] = 'Job' and TransType = 'Send' and ToId = '" + databaseCon.Decrypt(RID) + "' group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc, ItemWeight, SubCategoryID ,DisplaySortOrder)DeliveryAll left join(select FromId, ToId, tblReturnInventoryDetails.InventoryId, isnull(Sum(Quantity), 0) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site' and FromId = '" + databaseCon.Decrypt(RID) + "' group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.InventoryId = ReturnAll.InventoryId left join(select FromId, ToId, tblDamagedInventoryDetails.InventoryId, ISNULL(Sum(DamagedQuantity), 0) as DamagedAllQuantity from tblDamagedInventoryDetails  join tblDamagedInventory on tblDamagedInventoryDetails.DamagedInvId = tblDamagedInventory.DamagedInvId where [FromName] = 'Job' and[toName] = 'Site' and FromId = '" + databaseCon.Decrypt(RID) + "' group by[FromName],[ToName], FromId, ToId, tblDamagedInventoryDetails.InventoryId) DamagedAll On DeliveryAll.FromId = DamagedAll.ToId and DeliveryAll.InventoryId = DamagedAll.InventoryId left join tblSubCategory on tblSubCategory.SubCategoryID = DeliveryAll.SubCategoryID where 1 = 1order by MaterialType,isnull(DeliveryAll.DisplaySortOrder,9999999),ItemCode";
            DataTable dt = databaseCon.GetData(query);




            //if (dt.Rows.Count > 0)
            //{
            //iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, -50f, -50f, 20f, 20f);   //Page Size set top ,left,margin
            // doc.SetPageSize(iTextSharp.text.PageSize.A4);

            //Changing the pdf orientation potrait to landscape on 18 March 2020
            iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.LETTER, -110f, -110f, 50f, 50f);
            doc.SetPageSize(new Rectangle(990, 600));

            iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, Response.OutputStream);
            Response.ContentType = ("application/pdf");
            HeaderFooter2 evnt = new HeaderFooter2();
            evnt.RID = RID;
            writer.PageEvent = evnt;
            doc.Open();
            var Heading786 = FontFactory.GetFont("Arial", 10f, Font.BOLD, BaseColor.BLACK);
            //Heading786.BaseColor = BaseColor.GRAY;
            var Heading787 = FontFactory.GetFont("Arial", 25f, Font.BOLD, BaseColor.BLACK);
            var DataFont = FontFactory.GetFont("Arial", 10f, Font.NORMAL, BaseColor.BLACK);
            Phrase phrase;
            PdfPCell cell;

            PdfPTable headertable1 = new PdfPTable(5);
            headertable1.SetWidths(new int[] { 10, 9, 25, 9, 9 });

            //Table Start

            //PdfPTable headertable2 = new PdfPTable(12);
            //headertable2.SetWidths(new int[] { 10, 9, 20, 9, 9,9,6, 9,10,10,10,12});

            PdfPTable headertable2 = new PdfPTable(10);
            headertable2.SetWidths(new int[] { 10, 14, 20, 9, 8, 9, 10, 10, 11, 13 }); //10, 9, 20, 9, 6, 9, 10, 10, 10, 12

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
                    string Deliveryqty = String.Format("{0:#,##0}", dt.Rows[i]["DeliveryAvailableQuantity"]);
                    phrase.Add(new Chunk(Deliveryqty, DataFont));
                    //phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["DeliveryAvailableQuantity"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    //phrase = new Phrase();
                    //string ReturnAllqty = String.Format("{0:#,##0}", dt.Rows[i]["ReturnALLQuantity"]);
                    //phrase.Add(new Chunk(ReturnAllqty, DataFont));
                    ////phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["ReturnALLQuantity"].ToString()), DataFont));
                    //cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 15f;
                    //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //cell.Border = 1;
                    //cell.Colspan = 1;
                    //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    //headertable2.AddCell(cell);

                    //phrase = new Phrase();
                    //string DamagedAllqty = String.Format("{0:#,##0}", dt.Rows[i]["DamagedALLQuantity"]);
                    //phrase.Add(new Chunk(DamagedAllqty, DataFont));
                    //cell = new PdfPCell(phrase);
                    //cell.PaddingBottom = 15f;
                    //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //cell.Border = 1;
                    //cell.Colspan = 1;
                    //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    //headertable2.AddCell(cell);

                    phrase = new Phrase();
                    string qty = String.Format("{0:#,##0}", dt.Rows[i]["AvailableQuantity"]);
                    phrase.Add(new Chunk(qty, DataFont));
                    //phrase.Add(new Chunk(Convert.ToInt32(dt.Rows[i]["Quantity"]).ToString(), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);
                    Totalquantity += Convert.ToInt32(dt.Rows[i]["AvailableQuantity"]);
                    //cell.BackgroundBaseColor = BaseColor.LIGHT_GRAY;

                    phrase = new Phrase();
                    string itemweight = String.Format("{0:#,##0.00}", dt.Rows[i]["ItemWeight"]);
                    phrase.Add(new Chunk(itemweight, DataFont));
                    //phrase.Add(new Chunk(Convert.ToString(dt.Rows[i]["ItemWeight"].ToString()), DataFont));
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

                    //Adding 3 new columns on 16 March 2020
                    phrase = new Phrase();
                    string HireMeasurement = String.Format("{0:#,##0.00}", dt.Rows[i]["HireMeasurement"]);
                    phrase.Add(new Chunk(HireMeasurement, DataFont));
                    cell = new PdfPCell(phrase);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    phrase = new Phrase();
                    string DailyHireRate = String.Format("${0:#,##0.0000}", dt.Rows[i]["DailyHireRate"]);//changed on 26/March/2020
                                                                                                         // string DailyHireRate = String.Format("${0:# ###.0000}", dt.Rows[i]["DailyHireRate"]);
                    phrase.Add(new Chunk(DailyHireRate, DataFont));
                    cell.PaddingBottom = 15f;
                    cell = new PdfPCell(phrase);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    phrase = new Phrase();
                    string TotalDailyHire = String.Format("${0:#,##0.0000}", dt.Rows[i]["TotalDailyHire"]);
                    phrase.Add(new Chunk(TotalDailyHire, DataFont));
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
                    //Adding calculation for DailyHireRate and TotalDailyHire on 16 March 2020
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["DailyHireRate"])))
                    {
                        DailyRate += Convert.ToInt32(dt.Rows[i]["DailyHireRate"]);
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["TotalDailyHire"])))
                    {
                        TotalHire += Convert.ToDouble(dt.Rows[i]["TotalDailyHire"]);
                    }

                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 10;
                cell.Border = 0;
                cell.PaddingBottom = 6F;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

            }
            phrase = new Phrase();
            phrase.Add(new Chunk("", DataFont));
            cell = new PdfPCell(phrase);
            cell.Colspan = 3;
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

            phrase = new Phrase();
            phrase.Add(new Chunk("", DataFont));
            cell = new PdfPCell(phrase);
            cell.Colspan = 2;
            cell.PaddingBottom = 6F;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            // phrase.Add(new Chunk(string.Format("{0:$# ###.0000}", TotalHire).ToString(), DataFont));;
            //phrase.Add(new Chunk(string.Format("${0:#,##0.0000}", TotalHire).ToString(), DataFont));
            phrase.Add(new Chunk(string.Format("${0:#,##0.00}", TotalHire).ToString(), DataFont));// changed on 01/04/2020
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.Border = 3;
            cell.PaddingBottom = 6F;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            headertable2.AddCell(cell);

            doc.Add(headertable2);
            doc.Close();

        }

        public class HeaderFooter2 : iTextSharp.text.pdf.PdfPageEventHelper
        {
            public string RID { get; set; }
            List<PdfTemplate> pageNumberTemplates = new List<PdfTemplate>();// page header

            public override void OnStartPage(PdfWriter writer, Document doc)
            {
               // int RID;
               // RID = Convert.ToInt32(databaseCon.Decrypt(HttpContext.Current.Request.QueryString["RID"]));
                //SiteId = Convert.ToInt32(databaseCon.Decrypt(HttpContext.Current.Request.QueryString["SiteId"]));
                //RID = Convert.ToInt32(databaseCon.Decrypt(HttpContext.Current.Request.QueryString["RID"]));
                base.OnStartPage(writer, doc);
                string queryheader = "select tblClient.ClientImage,right('000000' + convert(varchar,tblRegister.ScaffRef),6) as ScaffRef,tblRegister.Description,Plans from tblClient, tblRegister where tblClient.ClientId = tblRegister.ClientID and tblRegister.registerid = '" + databaseCon.Decrypt(RID) + "'";
                DataTable dtheader = databaseCon.GetData(queryheader);



                string ImagePath = System.Web.HttpContext.Current.Server.MapPath("~/Images/logo2.png");// image size
                Rectangle pageSize = doc.PageSize;
                //PdfPTable headertable2 = new PdfPTable(12);
                //headertable2.SetWidths(new int[] { 10, 9, 20, 9, 9, 9 , 6, 9 ,10,10,10,12});

                PdfPTable headertable2 = new PdfPTable(10);
                headertable2.SetWidths(new int[] { 10, 14, 20, 9, 8, 9, 10, 10, 11, 13 });

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
                    // var container = new CloudBlobContainer(uri);
                    //var blob = container.GetBlockBlobReference(dtheader.Rows[0]["ClientImage"].ToString());
                    var noimage = System.Web.HttpContext.Current.Server.MapPath("~/Images/Logo2.png");
                    //var exists = blob.Exists();
                    if (Convert.ToString(dtheader.Rows[0]["ClientImage"]) == "")
                    {
                        string nophoto = System.Web.HttpContext.Current.Server.MapPath("~/images/Logo2.png");
                        cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
                        cell.FixedHeight = 50f;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
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
                        cell.PaddingTop = -10f;
                        // cell.PaddingTop = 10f;

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
                    //cell.PaddingTop = 10f;
                }
                cell.Border = 0;
                cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell.PaddingLeft = 20f;
                cell.FixedHeight = 50f;
                cell.PaddingTop = -10f;
                headertable2.AddCell(cell);


                phrase = new Phrase();
                phrase.Add(new Chunk("Job Inventory Hire Report", Heading787));
                cell = new PdfPCell(phrase);
                cell.Colspan = 7;
                //cell.PaddingBottom = 6f;
                //cell.PaddingLeft = 100f;
                cell.PaddingTop = -10f;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                headertable2.AddCell(cell);


                phrase = new Phrase();
                phrase.Add(new Chunk("Scaf Ref #", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                //cell.PaddingTop = -50f;
                cell.PaddingBottom = 6f;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable2.AddCell(cell);

                if (dtheader.Rows.Count > 0)
                {
                    for (int i = 0; i < dtheader.Rows.Count; i++)
                    {
                        phrase = new Phrase();
                        phrase.Add(new Chunk(Convert.ToString(dtheader.Rows[i]["ScaffRef"].ToString()), DataFont));
                        cell = new PdfPCell(phrase);
                        //cell.PaddingBottom = 5f;
                        //cell.PaddingTop = 8f;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;

                        cell.Colspan = 1;
                        //cell.PaddingTop = -50f;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable2.AddCell(cell);

                    }
                }
                else
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("", DataFont));
                    cell = new PdfPCell(phrase);
                    cell.Colspan = 1;
                    cell.Border = 3;
                    cell.PaddingBottom = 6F;
                    //cell.PaddingTop = -50f;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    headertable2.AddCell(cell);
                }

                //phrase = new Phrase();
                //phrase.Add(new Chunk("", Heading786));
                //cell = new PdfPCell(phrase);
                //cell.Colspan = 2 ;
                //cell.PaddingBottom = 6f;
                //cell.Border = 0;
                ////cell.PaddingTop = -50f;
                //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Reason for Access: ", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.PaddingBottom = 6f;
                cell.Border = 0;
                //cell.PaddingTop = -50f;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                headertable2.AddCell(cell);


                //phrase = new Phrase();
                //phrase.Add(new Chunk("", Heading786));
                //cell = new PdfPCell(phrase);
                //cell.Colspan = 1;
                //cell.PaddingBottom = 6f;
                //cell.Border = 0;
                ////cell.PaddingTop = -50f;
                //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //headertable2.AddCell(cell);

                if (dtheader.Rows.Count > 0)
                {
                    for (int i = 0; i < dtheader.Rows.Count; i++)
                    {
                        phrase = new Phrase();
                        phrase.Add(new Chunk(Convert.ToString(dtheader.Rows[i]["Description"].ToString()), DataFont));
                        cell = new PdfPCell(phrase);
                        //cell.PaddingBottom = 15f;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.Border = 1;
                        cell.Colspan = 4;
                        //cell.PaddingTop = -50f;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable2.AddCell(cell);


                    }
                }
                else
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("", DataFont));
                    cell = new PdfPCell(phrase);
                    cell.Colspan = 4;
                    cell.Border = 3;
                    cell.PaddingBottom = 6F;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    headertable2.AddCell(cell);
                }


                phrase = new Phrase();
                phrase.Add(new Chunk(" ", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 10;
                cell.PaddingBottom = 6f;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 10;
                cell.PaddingBottom = 6f;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
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
                phrase.Add(new Chunk("Delivered Qty", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                //phrase = new Phrase();
                //phrase.Add(new Chunk("Return Qty", Heading786));
                //cell = new PdfPCell(phrase);
                //cell.Colspan = 1;
                //cell.Border = 1;
                //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                //cell.BackgroundBaseColor = BaseColor.LIGHT_GRAY;
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                //headertable2.AddCell(cell);

                //phrase = new Phrase();
                //phrase.Add(new Chunk("Damage Qty", Heading786));
                //cell = new PdfPCell(phrase);
                //cell.Colspan = 1;
                //cell.Border = 1;
                //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                //cell.BackgroundBaseColor = BaseColor.LIGHT_GRAY;
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                //headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("On Job", Heading786));
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

                //Adding new code on 16 March 2020
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
                phrase.Add(new Chunk("Daily Hire Rate", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Total Daily Hire", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 1;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);
                headertable2.AddCell(cell);

                doc.Add(headertable2);
            }
            public override void OnEndPage(PdfWriter writer, Document document)
            {
                //var fontBaseColor = new iTextSharp.text.BaseColor(114, 163, 118);
                //Paragraph footerdate = new Paragraph(databaseCon.ToTimeZoneTime(DateTime.Now.ToUniversalTime()).ToLongDateString(), FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.TIMES_ROMAN, fontBaseColor));
                //PdfPCell cell;
                //PdfPTable footerDate = new PdfPTable(1);
                //footerDate.TotalWidth = 400;
                //footerDate.HorizontalAlignment = Element.ALIGN_CENTER;
                //cell = new PdfPCell(footerdate);
                //cell.Border = 0;
                //cell.PaddingTop = 30;
                //footerDate.AddCell(cell);
                //footerDate.WriteSelectedRows(0, -1, 700, 70, writer.DirectContent);

                //int TotalPage = writer.PageNumber;
                //int Currentpage = writer.CurrentPageNumber;
                //string text = "";
                //Paragraph footer = new Paragraph(text, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL));
                Paragraph footerdate = new Paragraph(("Printed: " + DateTime.Now.ToString("dd-MMM-yyy")), FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK));
                PdfPCell cell;
                //if (writer.PageNumber >= 2)
                //{
                //    PdfPTable footerTbl = new PdfPTable(1);
                //    footerTbl.TotalWidth = 400;// 1350;
                //    footerTbl.HorizontalAlignment = Element.ALIGN_CENTER;
                //    cell = new PdfPCell(footer);
                //    cell.Border = 0;
                //    cell.PaddingLeft = 10;
                //    cell.PaddingTop = 50;
                //    footerTbl.AddCell(cell);
                //    footerTbl.WriteSelectedRows(0, -1, 20, 70, writer.DirectContent);
                //}
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