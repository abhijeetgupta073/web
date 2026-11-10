using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.WindowsAzure.Storage.Blob;
using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;

namespace Scaffold.Controllers
{
    public class DayDocketController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: DayDocket
        public ActionResult DayDocket()
        {

            return View();
        }

        public class DayDocketModel
        {
            public Nullable<int> DayDocketID { get; set; }
            public string comments { get; set; }
            public string SiteRef { get; set; }
            public string ClientSign { get; set; }
            public Nullable<int> SiteLocID { get; set; }
            public Nullable<System.DateTime> EntryDate { get; set; }
            public string sitelocation { get; set; }
            public Nullable<decimal> NoOfHrs { get; set; }
        }

        public ActionResult ShowDayDocketData(string id)
        {
           string query = "select ISNULL(tblDayDocket.DayDocketID, 0) as DayDocketID,tblDayDocket.comments, Right('000000'+Convert(varchar,DayDocketId),6)  as SiteRef,tblDayDocket.ClientSign  ,ManHrsQuery.EntryDate , ManHrsQuery.SiteLocID ,ManHrsQuery.sitelocation, ManHrsQuery.NoOfHrs from (select EntryDate, tblSiteLocation.SiteLocID , sitelocation, SUM(AdditionalHrs) as NoOfHrs from tblManHrs inner join tblSiteLocation on tblSiteLocation.SiteLocID = tblManHrs.SiteLocId where ClientId = '" + databaseCon.Decrypt(id) + "' and AdditionalHrs > 0  and IncludedInDayDocket = 1 group by tblSiteLocation.SiteLocId, sitelocation, EntryDate, IncludedInDayDocket)ManHrsQuery left join tblDayDocket on ManHrsQuery.EntryDate = tblDayDocket.EntryDate and ManHrsQuery.SiteLocID = tblDayDocket.SiteLocId";

            var getData = dbContext.Database.SqlQuery<DayDocketModel>(query);
            return Json(getData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DayDocketEntryRedirect(string DayDocketID, string SiteLocID, string NoOfHrs, string EntryDate)
        {
            int DayDocketInst=0;
            if (DayDocketID == "0")
            {
                string query = "Insert into tblDayDocket (SiteLocId,EntryDate,NoOfHrs,CreatedBy,CreatedOn) values ('" + SiteLocID + "','" + EntryDate + "','" + NoOfHrs + "',(select EmailID from tblUser where UserId='" + databaseCon.Decrypt(Request.QueryString["UID"]) + "'),'" + DateTime.Now + "')";

                DayDocketInst = databaseCon.RunCmdID(query);

            }
            else
            {
                DayDocketInst = 0;
            }
            return Json(new { DayDocketInst = DayDocketInst }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewPdfDocument(string id, string DayDocketID, string SiteLocID, string EntryDate)
        {
            var Status = "";
            var Message = "";

            //GridDataItem gdi = (GridDataItem)e.Item;
            //string DayID = Convert.ToString(gdi.GetDataKeyValue("DayDocketID"));
            //string DocketId = DayID;
            //string SiteLocID = Convert.ToString(gdi.GetDataKeyValue("SiteLocID"));
            //string siteIDPDF = SiteLocID;
            //string EntryDate = Convert.ToString(gdi.GetDataKeyValue("EntryDate"));
            //string EnterDatePDf = EntryDate;
           
          var GetData = ShowData(id, SiteLocID, EntryDate, DayDocketID);
          var arrData = GetData.Split('~');
          var txttempfile = arrData[0];
          var txtFile = arrData[1];
          if (txttempfile != "" && txtFile != "")
          {

            string path = databaseCon.registerdocumentlink + txttempfile.ToString();
            var uri = new Uri(databaseCon.registerdocumentlink);
            var container = new CloudBlobContainer(uri);
            var blob = container.GetBlockBlobReference(txttempfile.ToString());
            var exists = blob.Exists();
            string extension = Path.GetExtension(".pdf");
            //string extension = "";
            if (exists == false)
            {
            }
            else
            {
                databaseCon.deletefileonblob(txttempfile, "registerdocument");
            }
            if (extension.Replace(".", "") == "pdf")
            {
                extension = "application/pdf";
            }
            databaseCon.savefileonblob(txttempfile, "Inspection-Document", txtFile, extension.Replace(".", ""));
          // ScriptManager.RegisterStartupScript(this, GetType(), "newtab", "window.open('" + path + "')", true);
            Status = "Success";
            Message = path;

            if (System.IO.File.Exists(txtFile))
            {
                try
                {
                    System.IO.File.Delete(txtFile);
                }
                catch
                {

                }
            }
         }else {
                Status = "Failed";
                Message = "Pdf Documents Not Found.";
          }

          return Json(new { Result = Status, Message = Message }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult btnEmailClick(string id, string UID, string UserRole, string DayDocketID, string SiteLocID, string EntryDate, string DomainName, string ClientSign)
        {

            var Status = "";
            var Message = "";
            var txtFile = "";
            var txttempfile = "";
            var txtcc = "";
            var txtClientEmail = "";
            var txtsubject = "";
            var msg = "";
            var txtEmailMSg = "";
            string Emaillink = "";

            DataTable dtclientinfo = databaseCon.GetData(" select DistributionList,EmailID from  tblClient join tblSiteLocation on tblClient.ClientId=tblSiteLocation.ClientId where tblClient.clientId='" + databaseCon.Decrypt(id) + "' and SiteLocId='" + SiteLocID + "'");
            if (dtclientinfo.Rows.Count > 0)
            {
              if (ClientSign != "")
               { 
                var GetData = ShowData(id, SiteLocID, EntryDate, DayDocketID);
                var arrData = GetData.Split('~');
                txttempfile = arrData[0];
                txtFile = arrData[1];
                if (txttempfile != "" && txtFile != "")
                {
                var uri = new Uri(databaseCon.registerdocumentlink);
                var container = new CloudBlobContainer(uri);
                var blob = container.GetBlockBlobReference(txttempfile.ToString());

                var exists = blob.Exists();
                if (exists == false)
                {
                }
                //else
                //{
                //    databaseCon.deletefileonblob(txttempfile, "registerdocument");

                //}
                string extension = Path.GetExtension(".pdf");
                if (extension.Replace(".", "") == "pdf")
                {
                    extension = "application/pdf";
                }

                databaseCon.savefileonblob(txttempfile, "registerdocument", txtFile, extension.Replace(".", ""));
                    //if (File.Exists(txtFile))
                    //{
                    //    try
                    //    {
                    //        //File.Delete(txtFile.Text);
                    //    }
                    //    catch
                    //    {

                    //    }
                    //}
                }
                else
                {
                    Status = "Failed";
                    Message = "Pdf Document Not Found.";
                }
              }
              else
               {
                    string path = Server.MapPath("~/Report/DayDocket_" + DayDocketID + ".pdf");
                    txttempfile = "No file attached as DayDocket not yet signed by Client.";
                    txtFile = path;

                   //Emaillink = DomainName + "/" + "DayDocketForm/DayDocketForm?UID=" + UID.Trim().Replace(" ", "+") + "&UserRole=" + UserRole.Trim().Replace(" ", "+") + "&id=" + id.Trim().Replace(" ", "+") + "&DayDock=" + DayDocketID.Trim().Replace(" ", "+") + "&SiteID=" + SiteLocID.Trim().Replace(" ", "+") + "&EntryDate=" + EntryDate.Trim().Replace(" ", "+");

                    Status = "Success";
                    Message = path;
               }
                txtClientEmail = Convert.ToString(dtclientinfo.Rows[0]["DistributionList"]);
                txtcc= Convert.ToString(dtclientinfo.Rows[0]["EmailID"]);

            }
            DataTable dtemailinfo = databaseCon.GetData("select Subject,Body from [tblEmailContent] where Purpose = 'Day Docket' and clientid='" + databaseCon.Decrypt(id) + "'");
            if (dtemailinfo.Rows.Count > 0)
            {
                txtsubject = Convert.ToString(dtemailinfo.Rows[0]["Subject"]);
                msg = Convert.ToString(dtemailinfo.Rows[0]["Body"]);
            }

            //string Emaillink = DomainName + "/" + "DayDocketForm?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&DayDock=" + DocketId + "&SiteID=" + SiteLocID + "&EntryDate=" + Convert.ToDateTime(EntryDate).ToString();
           // string Emaillink = DomainName + "/" + "DayDocketForm/DayDocketForm?UID=" + UID.Trim().Replace(" ", "+") + "&UserRole=" + UserRole.Trim().Replace(" ", "+") + "&id=" + id.Trim().Replace(" ", "+") + "&DayDock=" + DayDocketID.Trim().Replace(" ", "+") + "&SiteID=" + SiteLocID.Trim().Replace(" ", "+") + "&EntryDate=" + EntryDate.Trim().Replace(" ", "+");
            txtEmailMSg = msg + Environment.NewLine + Emaillink;


            Status = "Success";
            Message = "";

            return Json(new { Result = Status, Message = Message, txtFile = txtFile, txttempfile = txttempfile, txtcc = txtcc, txtClientEmail = txtClientEmail, txtsubject = txtsubject, txtEmailMSg = txtEmailMSg }, JsonRequestBehavior.AllowGet);
    }

        public ActionResult btnSend_Click(string txtClientEmail, string txtcc, string txtsubject, string txtEmailMSg, string txtFile, string DayDocketID, string txttempfile)
        {

            var Result = "";
            var Message = "";
            // emailConfirm.Style.Add("display", "none");
            if (databaseCon.SendEmail(txtClientEmail, txtcc, txtsubject, txtEmailMSg, txtFile))
            {
                databaseCon.deletefileonblob(txttempfile, "registerdocument");
                //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                //art1.Style.Add("display", "block");
                //alertlabel.Text = "<strong>Success ! </strong>Email send Successfully.";
                //GridInspectionList.Rebind();
                Result = "Success";
                Message = "Email send Successfully.";

            }
            else
            {
                //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                //art1.Style.Add("display", "block");
                //alertlabel.Text = "<strong>Alert ! </strong>Try Again. ";
                Result = "Failed";
                Message = "Try Again. ";
            }
            if (System.IO.File.Exists(txtFile))
            {
                try
                {
                    System.IO.File.Delete(txtFile);
                }
                catch { }
            }
            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }

        public int btnCancel_Click(string txttempfile, string txtFile)
        {
            databaseCon.deletefileonblob(txttempfile, "registerdocument");
            if (System.IO.File.Exists(txtFile))
            {
                try
                {
                    System.IO.File.Delete(txtFile);
                }
                catch
                {

                }
            }
            return 1;
        }


        // void ShowData()
        public string ShowData(string id, string siteIDPDF, string EnterDatePDf, string DocketId)
        {
            var txttempfile = "";
            var txtFile = "";

            int TotalWeight = 0;
            string query = "select Name,ClientImage, Address , Zip , Case when CellPhone!='' then 'Ph:'+CellPhone else '' end as CellPhone,Case when State!='' and City!=''  then City+' ,'+State else case when City!='' then City else state end  end as CityState from tblClient where ClientId='" + databaseCon.Decrypt(id) + "'";
            DataTable dt = databaseCon.GetData(query);

            string queryHeader = "select RIght('000000'+Convert(varchar,ScaffRef),6) as Scaffref,Description,firstName+' '+LastName as Resource,TypeofVariation,VariationDesc,isnull(AdditionalHrs,0) as AdditionalHrs,isnull(Category,'') as Category from tblManHrs join vw_tblRegister on tblManHrs.RegisterId=vw_tblRegister.RegisterId join tblUser On tblManHrs.UserId=tblUser.UserId left join tblUserCategory on tblUser.CategoryId=tblUserCategory.UserCatId where  IncludedInDayDocket=1 and tblManHrs.sitelocId='" + siteIDPDF + "' and entrydate='" + EnterDatePDf + "'";
            DataTable dtHeader = databaseCon.GetData(queryHeader);

            String query1 = "select Right('000000'+Convert(varchar,DayDocketId),6) as SiteRef , Convert(date,EntryDate) as EntryDate , tblDayDocket.SiteLocId ,SiteLocation,Comments ,CompanyRep,ClientRep,Case when Convert(varchar,Convert(date,ClientSignDate))='1900-01-01'then null else ClientSignDate End as ClientSignDate, SupervisorSign,SupervisorSignDate,ClientSign  from tblDayDocket join tblSiteLocation on tblDayDocket.SiteLocId=tblSiteLocation.SiteLocId where  DayDocketID='" + DocketId + "'";
            DataTable HeaderContent = databaseCon.GetData(query1);

            string path = Server.MapPath("~/Report/DayDocket_" + DocketId + ".pdf");

            txttempfile = "DayDocket_" + DocketId + ".pdf";
            txtFile = path;



            //    PageSize Set Top


            iTextSharp.text.Document doc = new iTextSharp.text.Document(PageSize.A4, -50f, -50f, 20f, 20f);   //Page Size set top ,left,margin
                                                                                                              // doc.SetPageSize(iTextSharp.text.PageSize.A4);

            //PdfWriter writer = PdfWriter.GetInstance(doc, Response.OutputStream);


            Response.ContentType = ("application/pdf");
            var contentfontCheck = FontFactory.GetFont((Server.MapPath("~/Content/fonts/wingding.ttf")), BaseFont.CP1252, true, 14);

            var contentfontUncheck = FontFactory.GetFont((Server.MapPath("~/Content/fonts/wingding.ttf")), BaseFont.CP1252, true, 14);
            PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));
            doc.Open();

            var Heading786 = FontFactory.GetFont("Arial", 10f, Font.BOLD, BaseColor.BLACK);
            var Heading787 = FontFactory.GetFont("Arial", 25f, Font.BOLD, BaseColor.BLACK);
            var Heading788 = FontFactory.GetFont("Arial", 16f, Font.BOLD, BaseColor.BLUE);
            var DataFont = FontFactory.GetFont("Arial", 10f, Font.NORMAL, BaseColor.BLACK);
            Phrase pharse;
            PdfPCell cell;

            PdfPTable headertable1 = new PdfPTable(5);
            headertable1.SetWidths(new int[] { 12, 12, 12, 12, 12 });

            //       Table Start

            PdfPTable headertable2 = new PdfPTable(7);
            headertable2.SetWidths(new int[] { 9, 20, 10, 9, 9, 9, 9 });


            if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["ClientImage"])))
            {
                var uri = new Uri(databaseCon.clientimageLink);
                // var container = new CloudBlobContainer(uri);
                //var blob = container.GetBlockBlobReference(dtheader.Rows[0]["ClientImage"].ToString());
                var noimage = Server.MapPath("~/images/Logo2.png");
                //var exists = blob.Exists();
                if (Convert.ToString(dt.Rows[0]["ClientImage"]) == "")
                {
                    string nophoto = Server.MapPath("~/images/Logo2.png");
                    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.FixedHeight = 50f;
                    cell.PaddingTop = -10f;
                    cell.Colspan = 1;
                    //cell.PaddingLeft = 10f;
                    //cell.PaddingTop = 20;
                }
                else
                {
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images/clientlogo/" + dt.Rows[0]["ClientImage"].ToString()));

                    img.ScaleAbsolute(180f, 133f);
                    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img), true);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.FixedHeight = 50f;
                    cell.PaddingTop = -10f;
                    cell.Colspan = 1;
                    //cell.PaddingLeft = 10f;
                    //cell.PaddingTop = 20f;

                }
            }
            else
            {
                string nophoto = Server.MapPath("~/images/Logo2.png");
                cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.FixedHeight = 50f;
                cell.PaddingTop = -10f;
                cell.Colspan = 1;
                //cell.PaddingTop = 20f;
            }
            cell.Border = 0;
            cell.Colspan = 1;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.FixedHeight = 50f;
            cell.PaddingTop = -10f;
            headertable1.AddCell(cell);



            pharse = new Phrase();
            pharse.Add(new Chunk("EXTRA VARIATION", Heading788));
            cell = new PdfPCell(pharse);
            cell.Colspan = 3;
            //cell.PaddingBottom = 3f;
            //cell.PaddingLeft = 80f;
            //cell.PaddingTop = 1f;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            headertable1.AddCell(cell);

            if (dt.Rows.Count > 0)
            {

                pharse = new Phrase();
                pharse.Add(new Chunk(Convert.ToString(dt.Rows[0]["Address"].ToString()), DataFont));
                pharse.Add(Environment.NewLine);
                pharse.Add(new Chunk(Convert.ToString(dt.Rows[0]["CityState"].ToString()), DataFont));
                pharse.Add(Environment.NewLine);
                pharse.Add(new Chunk(Convert.ToString(dt.Rows[0]["CellPhone"].ToString()), DataFont));
                pharse.Add(Environment.NewLine);
                pharse.Add(new Chunk(Convert.ToString(dt.Rows[0]["ZIP"].ToString()), DataFont));
                cell = new PdfPCell(pharse);
                cell.Colspan = 2;
                //cell.PaddingBottom = 3f;
                //cell.PaddingLeft = 100f;
                //cell.PaddingTop = 10f;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                headertable1.AddCell(cell);



            }
            else
            {

                pharse = new Phrase();
                pharse.Add(new Chunk("", DataFont));
                cell = new PdfPCell(pharse);
                cell.Colspan = 2;
                //cell.PaddingBottom = 3f;
                //cell.PaddingLeft = 100f;
                //cell.PaddingTop = 10f;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                headertable1.AddCell(cell);
            }


            pharse = new Phrase();
            pharse.Add(new Chunk("", Heading786));
            cell = new PdfPCell(pharse);
            cell.Colspan = 3;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);




            pharse = new Phrase();
            pharse.Add(new Chunk("DATE :", Heading786));
            pharse.Add(Environment.NewLine);
            cell = new PdfPCell(pharse);
            cell.Colspan = 1;
            //cell.PaddingBottom = 3f;
            //cell.PaddingRight = 50f;
            //cell.PaddingTop = 10f;
            cell.Border = 0;

            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            if (HeaderContent.Rows.Count > 0)
            {
                pharse = new Phrase();
                string date = String.Format("{0:dd-MMM-yyyy}", HeaderContent.Rows[0]["EntryDate"]);
                pharse.Add(new Chunk(date, DataFont));
                cell = new PdfPCell(pharse);
                cell.Colspan = 1;
                //cell.PaddingBottom = 3f;
                //cell.PaddingLeft = -10f;
                //cell.PaddingTop = 10f;
                cell.Border = PdfPCell.ALIGN_BOTTOM;
                cell.Border = PdfPCell.ALIGN_RIGHT;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
            }
            else
            {
                pharse = new Phrase();
                pharse.Add(new Chunk("", DataFont));
                cell = new PdfPCell(pharse);
                cell.Colspan = 1;
                //cell.PaddingBottom = 3f;
                //cell.PaddingLeft = -10f;
                //cell.PaddingTop = 10f;
                cell.Border = PdfPCell.ALIGN_BOTTOM;
                cell.Border = PdfPCell.ALIGN_RIGHT;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
            }

            pharse = new Phrase();
            pharse.Add(new Chunk("", Heading786));
            cell = new PdfPCell(pharse);
            cell.Colspan = 5;
            cell.Border = 0;
            headertable1.AddCell(cell);

            pharse = new Phrase();
            pharse.Add(new Chunk("", Heading786));
            cell = new PdfPCell(pharse);
            cell.Colspan = 5;
            cell.Border = 0;
            headertable1.AddCell(cell);


            pharse = new Phrase();
            pharse.Add(new Chunk("", Heading786));
            //    pharse.Add(Environment.NewLine);
            cell = new PdfPCell(pharse);
            cell.Colspan = 3;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);


            pharse = new Phrase();
            pharse.Add(new Chunk("ORDER NO :", Heading786));
            cell = new PdfPCell(pharse);
            //cell.PaddingLeft = 13f;
            cell.Colspan = 1;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            if (HeaderContent.Rows.Count > 0)
            {

                pharse = new Phrase();
                pharse.Add(new Chunk(Convert.ToString(HeaderContent.Rows[0]["SiteRef"].ToString()), DataFont));
                cell = new PdfPCell(pharse);
                cell.Colspan = 1;
                cell.Border = PdfPCell.ALIGN_BOTTOM;
                cell.Border = PdfPCell.ALIGN_RIGHT;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
            }
            else
            {
                pharse = new Phrase();
                pharse.Add(new Chunk("", DataFont));
                cell = new PdfPCell(pharse);
                cell.Colspan = 1;
                cell.Border = PdfPCell.ALIGN_BOTTOM;
                cell.Border = PdfPCell.ALIGN_RIGHT;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);

            }


            pharse = new Phrase();
            pharse.Add(new Chunk("", Heading786));
            cell = new PdfPCell(pharse);
            cell.Colspan = 5;
            cell.Border = 0;
            headertable1.AddCell(cell);

            pharse = new Phrase();
            pharse.Add(new Chunk("", Heading786));
            pharse.Add(Environment.NewLine);
            cell = new PdfPCell(pharse);
            cell.Colspan = 5;
            cell.Border = 0;
            headertable1.AddCell(cell);

            pharse = new Phrase();
            pharse.Add(new Chunk("BUILDER/CLIENT :", Heading786));
            cell = new PdfPCell(pharse);
            cell.Colspan = 1;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            if (dt.Rows.Count > 0)
            {
                pharse = new Phrase();
                pharse.Add(new Chunk(Convert.ToString(dt.Rows[0]["Name"].ToString()), DataFont));
                cell = new PdfPCell(pharse);
                cell.Colspan = 2;
                cell.Border = PdfPCell.ALIGN_BOTTOM;
                cell.Border = PdfPCell.ALIGN_RIGHT;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
            }
            else
            {

                pharse = new Phrase();
                pharse.Add(new Chunk("", DataFont));
                cell = new PdfPCell(pharse);
                cell.Colspan = 2;
                cell.Border = PdfPCell.ALIGN_BOTTOM;
                cell.Border = PdfPCell.ALIGN_RIGHT;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
            }


            pharse = new Phrase();
            pharse.Add(new Chunk("SITE :", Heading786));
            cell = new PdfPCell(pharse);
            cell.Colspan = 1;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            if (HeaderContent.Rows.Count > 0)
            {
                pharse = new Phrase();
                pharse.Add(new Chunk(Convert.ToString(HeaderContent.Rows[0]["SiteLocation"].ToString()), DataFont));
                // pharse.Add(Environment.NewLine);
                cell = new PdfPCell(pharse);
                cell.Colspan = 1;
                cell.Border = PdfPCell.ALIGN_BOTTOM;
                cell.Border = PdfPCell.ALIGN_RIGHT;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
            }
            else
            {
                pharse = new Phrase();
                pharse.Add(new Chunk("", DataFont));
                // pharse.Add(Environment.NewLine);
                cell = new PdfPCell(pharse);
                cell.Colspan = 1;
                cell.Border = PdfPCell.ALIGN_BOTTOM;
                cell.Border = PdfPCell.ALIGN_RIGHT;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
            }

            pharse = new Phrase();
            pharse.Add(new Chunk("", Heading786));
            cell = new PdfPCell(pharse);
            cell.Colspan = 5;
            cell.Border = 0;
            headertable1.AddCell(cell);

            pharse = new Phrase();
            pharse.Add(new Chunk("", Heading786));
            cell = new PdfPCell(pharse);
            cell.Colspan = 5;
            cell.Border = 0;
            headertable1.AddCell(cell);

            pharse = new Phrase();
            pharse.Add(new Chunk("", Heading786));
            cell = new PdfPCell(pharse);
            cell.Colspan = 5;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            pharse = new Phrase();
            pharse.Add(new Chunk("", Heading786));
            pharse.Add(Environment.NewLine);
            cell = new PdfPCell(pharse);
            cell.Colspan = 5;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            pharse = new Phrase();
            pharse.Add(new Chunk("COMMENTS :", Heading786));
            cell = new PdfPCell(pharse);
            cell.Colspan = 1;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            //pharse = new Phrase();
            //pharse.Add(new Chunk("", Heading786));
            //cell = new PdfPCell(pharse);
            //cell.Colspan = 4;
            //cell.Border = PdfCell.TOP_BORDER | PdfCell.LEFT_BORDER | PdfCell.RIGHT_BORDER | PdfCell.ALIGN_BOTTOM;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //headertable1.AddCell(cell);

            if (HeaderContent.Rows.Count > 0)
            {
                pharse = new Phrase();
                pharse.Add(new Chunk(Convert.ToString(HeaderContent.Rows[0]["Comments"].ToString()), DataFont));
                // pharse.Add(Environment.NewLine);
                cell = new PdfPCell(pharse);
                cell.Colspan = 4;
                //  cell.Border = PdfPCell.ALIGN_BOTTOM;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
            }
            else
            {
                pharse = new Phrase();
                pharse.Add(new Chunk("", DataFont));
                // pharse.Add(Environment.NewLine);
                cell = new PdfPCell(pharse);
                cell.Colspan = 4;
                // cell.Border = PdfPCell.ALIGN_BOTTOM;
                //cell.Border = PdfPCell.ALIGN_RIGHT;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
            }


            // pharse = new Phrase();
            // pharse.Add(new Chunk("", DataFont));
            // cell = new PdfPCell(pharse);
            // cell.Colspan = 4;
            //// cell.Border = 3;
            // //cell.PaddingBottom = 6F;
            // cell.Border = PdfCell.TOP_BORDER | PdfCell.LEFT_BORDER | PdfCell.BOTTOM_BORDER | PdfCell.RIGHT_BORDER;
            // cell.HorizontalAlignment = Element.ALIGN_LEFT;
            // // cell.BackgroundColor = Color.LIGHT_GRAY;
            // headertable2.AddCell(cell);


            pharse = new Phrase();
            pharse.Add(new Chunk("", Heading786));
            pharse.Add(Environment.NewLine);
            cell = new PdfPCell(pharse);
            cell.Colspan = 5;
            cell.Border = PdfPCell.ALIGN_BOTTOM;
            cell.Border = PdfPCell.ALIGN_RIGHT;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);


            pharse = new Phrase();
            pharse.Add(new Chunk("Scaf Ref #", Heading786));
            cell = new PdfPCell(pharse);
            cell.Colspan = 1;
            cell.Border = 1;
            //  cell.PaddingBottom = 6f;
            // cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            // cell.Border = PdfPCell.ALIGN_RIGHT;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            pharse = new Phrase();
            pharse.Add(new Chunk("Reason for Access", Heading786));
            cell = new PdfPCell(pharse);
            cell.Colspan = 1;
            // cell.PaddingBottom = 6f;
            cell.Border = 1;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            //  cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            pharse = new Phrase();
            pharse.Add(new Chunk("Resource", Heading786));
            cell = new PdfPCell(pharse);
            cell.Colspan = 1;
            cell.Border = 1;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            //  cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            pharse = new Phrase();
            pharse.Add(new Chunk("Category", Heading786));
            cell = new PdfPCell(pharse);
            cell.Colspan = 1;
            cell.Border = 1;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            //  cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            pharse = new Phrase();
            pharse.Add(new Chunk("Type of Variation", Heading786));
            cell = new PdfPCell(pharse);
            cell.Colspan = 1;
            cell.Border = 1;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            // cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            pharse = new Phrase();
            pharse.Add(new Chunk("Variation Desc", Heading786));
            cell = new PdfPCell(pharse);
            cell.Colspan = 1;
            cell.Border = 1;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            //     cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            pharse = new Phrase();
            pharse.Add(new Chunk("Additional Hrs", Heading786));
            cell = new PdfPCell(pharse);
            cell.Colspan = 1;
            cell.Border = 1;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.ALIGN_BOTTOM;
            //     cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);
            //Table End




            if (dtHeader.Rows.Count > 0)
            {
                for (int i = 0; i < dtHeader.Rows.Count; i++)
                {
                    pharse = new Phrase();
                    //pharse.Add(new Chunk(Convert.ToString(dtHeader.Rows[i]["DateOfWork"]).ToString(),DataFont));
                    //  string dateTime = String.Format("{0:MM/dd/yyyy}", dtHeader.Rows[i]["scaffref"]);
                    pharse.Add(new Chunk(Convert.ToString(dtHeader.Rows[i]["scaffref"].ToString()), DataFont));
                    //   pharse.Add(new Chunk( dateTime , DataFont));
                    cell = new PdfPCell(pharse);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    pharse = new Phrase();
                    pharse.Add(new Chunk(Convert.ToString(dtHeader.Rows[i]["Description"].ToString()), DataFont));
                    cell = new PdfPCell(pharse);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    pharse = new Phrase();
                    pharse.Add(new Chunk(Convert.ToString(dtHeader.Rows[i]["Resource"].ToString()), DataFont));
                    cell = new PdfPCell(pharse);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    pharse = new Phrase();
                    pharse.Add(new Chunk(Convert.ToString(dtHeader.Rows[i]["Category"].ToString()), DataFont));
                    cell = new PdfPCell(pharse);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    pharse = new Phrase();
                    pharse.Add(new Chunk(Convert.ToString(dtHeader.Rows[i]["TypeofVariation"]).ToString(), DataFont));
                    cell = new PdfPCell(pharse);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    pharse = new Phrase();
                    pharse.Add(new Chunk(Convert.ToString(dtHeader.Rows[i]["VariationDesc"].ToString()), DataFont));
                    cell = new PdfPCell(pharse);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);

                    pharse = new Phrase();
                    pharse.Add(new Chunk(Convert.ToInt32(dtHeader.Rows[i]["AdditionalHrs"]).ToString(), DataFont));
                    cell = new PdfPCell(pharse);
                    cell.PaddingBottom = 15f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 1;
                    cell.Colspan = 1;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable2.AddCell(cell);
                    TotalWeight += Convert.ToInt32(dtHeader.Rows[i]["AdditionalHrs"]);
                    //cell.BackgroundColor = Color.LIGHT_GRAY;
                }
            }
            else
            {
                pharse = new Phrase();
                pharse.Add(new Chunk("", DataFont));
                cell = new PdfPCell(pharse);
                cell.Colspan = 7;
                cell.Border = 0;
                cell.PaddingBottom = 6F;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);
            }


            pharse = new Phrase();
            pharse.Add(new Chunk("TOTAL HOURS WORKED", DataFont));
            cell = new PdfPCell(pharse);
            cell.Colspan = 6;
            cell.Border = 3;
            cell.PaddingBottom = 6F;
            cell.Border = PdfPCell.RIGHT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable2.AddCell(cell);

            pharse = new Phrase();
            decimal Total = Convert.ToDecimal(TotalWeight.ToString());
            pharse.Add(new Chunk(Total.ToString(), DataFont));
            cell = new PdfPCell(pharse);
            cell.Colspan = 1;
            cell.Border = 3;
            cell.PaddingBottom = 6F;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            headertable2.AddCell(cell);

            // space of table


            pharse = new Phrase();
            pharse.Add(new Chunk("", Heading786));
            pharse.Add(Environment.NewLine);
            cell = new PdfPCell(pharse);
            cell.Colspan = 7;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            pharse = new Phrase();
            pharse.Add(new Chunk("", Heading786));
            pharse.Add(Environment.NewLine);
            cell = new PdfPCell(pharse);
            cell.Colspan = 7;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            pharse = new Phrase();
            pharse.Add(new Chunk("", Heading786));
            pharse.Add(Environment.NewLine);
            cell = new PdfPCell(pharse);
            cell.Colspan = 7;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            pharse = new Phrase();
            pharse.Add(new Chunk(""));
            pharse.Add(Environment.NewLine);
            cell = new PdfPCell(pharse);
            cell.Colspan = 7;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            pharse = new Phrase();
            pharse.Add(new Chunk(""));
            pharse.Add(Environment.NewLine);
            cell = new PdfPCell(pharse);
            cell.Colspan = 7;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            // end of space



            pharse = new Phrase();
            pharse.Add(new Chunk("Company Rep:", Heading786));
            cell = new PdfPCell(pharse);
            cell.Colspan = 1;
            cell.Border = -1;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);


            if (HeaderContent.Rows.Count > 0)
            {
                pharse = new Phrase();
                // pharse.Add(new Chunk(Convert.ToInt32(HeaderContent.Rows[0]["CompanyRep"]).ToString(), DataFont));
                pharse.Add(new Chunk(Convert.ToString(HeaderContent.Rows[0]["CompanyRep"].ToString()), DataFont));
                cell = new PdfPCell(pharse);
                cell.Colspan = 1;
                cell.FixedHeight = 0;
                cell.Border = 0;
                //cell.Border = PdfPCell.ALIGN_BOTTOM;
                //cell.Border = PdfPCell.ALIGN_RIGHT;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);
            }
            else
            {
                pharse = new Phrase();
                pharse.Add(new Chunk("", Heading786));
                cell = new PdfPCell(pharse);
                cell.Colspan = 1;
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);
            }

            pharse = new Phrase();
            pharse.Add(new Chunk("", Heading786));
            cell = new PdfPCell(pharse);
            cell.Colspan = 1;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            pharse = new Phrase();
            pharse.Add(new Chunk("Signature :", Heading786));
            cell = new PdfPCell(pharse);
            cell.Colspan = 1;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);



            if (!string.IsNullOrEmpty(Convert.ToString(HeaderContent.Rows[0]["SupervisorSign"])))
            {
                var uri = new Uri(databaseCon.siteimagelink);
                var container = new CloudBlobContainer(uri);
                var blob = container.GetBlockBlobReference(HeaderContent.Rows[0]["SupervisorSign"].ToString());
                //var noimage = Server.MapPath("images/noimg.jpg");
                var exists = blob.Exists();
                if (exists == false)
                {

                    pharse = new Phrase();
                    pharse.Add(new Chunk("", Heading786));
                    // pharse.Add(Environment.NewLine);
                    cell = new PdfPCell(pharse);
                    cell.Colspan = 3;
                    cell.FixedHeight = 70f;
                    cell.PaddingRight = 100f;
                    cell.PaddingBottom = 100f;
                    cell.Border = 0;
                    // cell.Border = PdfPCell.ALIGN_BOTTOM;
                    // cell.Border = PdfPCell.ALIGN_RIGHT;
                    //cell.Border = PdfCell.TOP_BORDER | PdfCell.LEFT_BORDER | PdfCell.BOTTOM_BORDER | PdfCell.RIGHT_BORDER;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    headertable2.AddCell(cell);
                }
                else
                {
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(databaseCon.siteimagelink + HeaderContent.Rows[0]["SupervisorSign"].ToString());
                    img.ScaleAbsolute(180f, 133f);

                    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img), true);
                    cell.Colspan = 3;
                    cell.FixedHeight = 70f;
                    cell.PaddingRight = 50f;
                    cell.Border = 0;
                    // cell.Border = PdfPCell.ALIGN_BOTTOM;
                    // cell.Border = PdfPCell.ALIGN_RIGHT;
                    //cell.Border = PdfCell.TOP_BORDER | PdfCell.LEFT_BORDER | PdfCell.BOTTOM_BORDER | PdfCell.RIGHT_BORDER;
                    //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    headertable2.AddCell(cell);



                }
            }
            else
            {

                pharse = new Phrase();
                pharse.Add(new Chunk("", Heading786));
                // pharse.Add(Environment.NewLine);
                cell = new PdfPCell(pharse);
                cell.Colspan = 3;
                cell.FixedHeight = 70f;
                cell.Border = 0;
                // cell.Border = PdfPCell.ALIGN_BOTTOM;
                // cell.Border = PdfPCell.ALIGN_RIGHT;
                //cell.Border = PdfCell.TOP_BORDER | PdfCell.LEFT_BORDER | PdfCell.BOTTOM_BORDER | PdfCell.RIGHT_BORDER;
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);
            }



            pharse = new Phrase();
            pharse.Add(new Chunk(""));
            pharse.Add(Environment.NewLine);
            cell = new PdfPCell(pharse);
            cell.Colspan = 3;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            headertable2.AddCell(cell);


            pharse = new Phrase();
            pharse.Add(new Chunk(""));
            pharse.Add(Environment.NewLine);
            cell = new PdfPCell(pharse);
            cell.Colspan = 7;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            headertable2.AddCell(cell);


            pharse = new Phrase();
            pharse.Add(new Chunk("Client Rep :", Heading786));
            cell = new PdfPCell(pharse);
            cell.Colspan = 1;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            if (HeaderContent.Rows.Count > 0)
            {
                pharse = new Phrase();
                pharse.Add(new Chunk(Convert.ToString(HeaderContent.Rows[0]["ClientRep"]).ToString(), DataFont));
                cell = new PdfPCell(pharse);
                cell.Colspan = 1;
                cell.Border = 0;
                //cell.Border = PdfPCell.ALIGN_RIGHT;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);
            }
            else
            {
                pharse = new Phrase();
                pharse.Add(new Chunk("", Heading786));
                cell = new PdfPCell(pharse);
                cell.Colspan = 1;
                cell.Border = 0;
                //cell.Border = PdfPCell.ALIGN_BOTTOM;
                //cell.Border = PdfPCell.ALIGN_RIGHT;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);
            }

            pharse = new Phrase();
            pharse.Add(new Chunk("", Heading786));
            cell = new PdfPCell(pharse);
            cell.Colspan = 1;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            pharse = new Phrase();
            pharse.Add(new Chunk("Signature :", Heading786));
            cell = new PdfPCell(pharse);
            cell.Colspan = 1;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);


            if (!string.IsNullOrEmpty(Convert.ToString(HeaderContent.Rows[0]["ClientSign"])))
            {
                var uri = new Uri(databaseCon.siteimagelink);
                var container = new CloudBlobContainer(uri);
                var blob = container.GetBlockBlobReference(HeaderContent.Rows[0]["ClientSign"].ToString());
                //var noimage = Server.MapPath("images/noimg.jpg");
                var exists = blob.Exists();
                if (exists == false)
                {

                    pharse = new Phrase();
                    pharse.Add(new Chunk("", Heading786));
                    // pharse.Add(Environment.NewLine);
                    cell = new PdfPCell(pharse);
                    cell.Colspan = 3;
                    cell.FixedHeight = 70f;
                    cell.PaddingRight = 100f;
                    cell.PaddingBottom = 100f;
                    cell.Border = 0;
                    // cell.Border = PdfPCell.ALIGN_BOTTOM;
                    // cell.Border = PdfPCell.ALIGN_RIGHT;
                    //cell.Border = PdfCell.TOP_BORDER | PdfCell.LEFT_BORDER | PdfCell.BOTTOM_BORDER | PdfCell.RIGHT_BORDER;
                    //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    headertable2.AddCell(cell);
                }
                else
                {
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(databaseCon.siteimagelink + HeaderContent.Rows[0]["ClientSign"].ToString());
                    img.ScaleAbsolute(180f, 133f);

                    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img), true);
                    cell.Colspan = 3;
                    cell.FixedHeight = 70f;
                    cell.PaddingRight = 50f;
                    cell.Border = 0;
                    // cell.Border = PdfPCell.ALIGN_BOTTOM;
                    // cell.Border = PdfPCell.ALIGN_RIGHT;
                    //cell.Border = PdfCell.TOP_BORDER | PdfCell.LEFT_BORDER | PdfCell.BOTTOM_BORDER | PdfCell.RIGHT_BORDER;
                    //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    headertable2.AddCell(cell);



                }
            }
            else
            {

                pharse = new Phrase();
                pharse.Add(new Chunk("", Heading786));
                // pharse.Add(Environment.NewLine);
                cell = new PdfPCell(pharse);
                cell.Colspan = 3;
                cell.FixedHeight = 70f;
                cell.Border = 0;
                // cell.Border = PdfPCell.ALIGN_BOTTOM;
                // cell.Border = PdfPCell.ALIGN_RIGHT;
                //cell.Border = PdfCell.TOP_BORDER | PdfCell.LEFT_BORDER | PdfCell.BOTTOM_BORDER | PdfCell.RIGHT_BORDER;
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);
            }



            pharse = new Phrase();
            pharse.Add(new Chunk(""));
            pharse.Add(Environment.NewLine);
            cell = new PdfPCell(pharse);
            cell.Colspan = 3;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            headertable2.AddCell(cell);


            pharse = new Phrase();
            pharse.Add(new Chunk(""));
            pharse.Add(Environment.NewLine);
            cell = new PdfPCell(pharse);
            cell.Colspan = 7;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            headertable2.AddCell(cell);

            pharse = new Phrase();
            pharse.Add(new Chunk("Date :", Heading786));
            cell = new PdfPCell(pharse);
            cell.Colspan = 1;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            if (HeaderContent.Rows.Count > 0)
            {
                pharse = new Phrase();
                string dateTime = String.Format("{0:dd-MMM-yyyy}", HeaderContent.Rows[0]["ClientSignDate"]);
                pharse.Add(new Chunk(dateTime, DataFont));
                cell = new PdfPCell(pharse);
                cell.Colspan = 1;
                cell.Border = PdfPCell.ALIGN_BOTTOM;
                cell.Border = PdfPCell.ALIGN_RIGHT;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);
            }
            else
            {
                pharse = new Phrase();
                pharse.Add(new Chunk("", Heading786));
                cell = new PdfPCell(pharse);
                cell.Colspan = 1;
                cell.Border = PdfPCell.ALIGN_BOTTOM;
                cell.Border = PdfPCell.ALIGN_RIGHT;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable2.AddCell(cell);
            }


            pharse = new Phrase();
            pharse.Add(new Chunk("", Heading786));
            cell = new PdfPCell(pharse);
            cell.Colspan = 5;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            pharse = new Phrase();
            pharse.Add(new Chunk(""));
            pharse.Add(Environment.NewLine);
            cell = new PdfPCell(pharse);
            cell.Colspan = 7;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);




            pharse = new Phrase();
            pharse.Add(new Chunk("The Client hereby accepts the hours worked including travel if applicable when worked on behalf of the Client and the Client agrees to pay the hours signed for at the prescribed rate. ", DataFont));
            cell = new PdfPCell(pharse);
            cell.Colspan = 7;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            headertable2.AddCell(cell);


            doc.Add(headertable1);
            doc.Add(headertable2);
            doc.Close();

            return txttempfile + "~" + txtFile;
        }

    }

}