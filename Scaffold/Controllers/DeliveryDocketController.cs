using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure.Storage.Blob;
using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;

namespace Scaffold.Controllers
{
    public class DeliveryDocketController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();

     
        // GET: DeliveryDocket
        public ActionResult DeliveryDocket(string id,string checkid)

        {
            DeliveryDocketViewModel deliveryDocketViewModel = new DeliveryDocketViewModel();
            int Clientid = 0;
            {
                Clientid = Convert.ToInt32(databaseCon.Decrypt(Request.QueryString["id"]));
                string query1 = "select Name from tblClient where ClientId=" + Clientid;
                DataTable dt1 = databaseCon.GetData(query1);
                if (dt1.Rows.Count > 0)
                {
                    //lblclientname.Text = dt1.Rows[0]["Name"].ToString();
                    ViewBag.ClientName = dt1.Rows[0]["Name"].ToString();
                    deliveryDocketViewModel.ClientName= dt1.Rows[0]["Name"].ToString();
                }
            }

            string query = "select DelDocId,Comment from tblTransaction join tblDeliveryDocket on tblTransaction.DeliveryId=tblDeliveryDocket.DeliveryID and tblTransaction.ClientId=tblDeliveryDocket.ClientId where TransId=" + databaseCon.Decrypt(checkid) + "and tblDeliveryDocket.ClientId=" + databaseCon.Decrypt(id);
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
             int   DocketNo = Convert.ToInt32(dt.Rows[0]["DelDocId"]);
                deliveryDocketViewModel.TransferNumber = DocketNo;
                deliveryDocketViewModel.Comment = dt.Rows[0]["Comment"].ToString();
                }

            if (Request.QueryString["ToId"] != null && Request.QueryString["ToId"] != "")
            {
                int ToId = Convert.ToInt32(databaseCon.Decrypt(Request.QueryString["ToId"]));
                string query1 = "select SiteLocation,Address from tblSiteLocation where SiteLocID=" + ToId;
                DataTable dt1 = databaseCon.GetData(query1);
                if (dt1.Rows.Count > 0)
                {
                    // lblsitename.Text = Convert.ToString(dt1.Rows[0]["SiteLocation"]);
                    // lblsiteaddress.Text = Convert.ToString(dt1.Rows[0]["Address"]);
                    ViewBag.SiteName = Convert.ToString(dt1.Rows[0]["SiteLocation"]);
                    ViewBag.SiteAddress = Convert.ToString(dt1.Rows[0]["Address"]);

                    deliveryDocketViewModel.SiteLocation = dt1.Rows[0]["SiteLocation"].ToString();
                    deliveryDocketViewModel.Address = dt1.Rows[0]["Address"].ToString();
                    

                    }

            }
            // Added on 20Feb2025
            ViewBag.ClientID = Convert.ToString(Clientid);

            string ClientQuery = "select ClientImage from tblClient where ClientId = '" + Clientid + "'";
            DataTable clientdt = databaseCon.GetData(ClientQuery);

            if (clientdt.Rows.Count > 0)
            {
                if (clientdt.Rows[0]["ClientImage"].ToString() != "")
                {
                    var uri = new Uri(databaseCon.clientimageLink);
                    string imgname = clientdt.Rows[0]["ClientImage"].ToString();
                    if (clientdt.Rows[0]["ClientImage"].ToString() == "")
                    {
                        //ViewBag.ClientImage = "/img/logo1.png";
                        ViewBag.ClientImage = databaseCon.logoimage;
                    }
                    else
                    {
                        ViewBag.ClientImage = "/images/clientlogo/" + clientdt.Rows[0]["ClientImage"].ToString();
                    }
                }
                else
                {
                    // imgLogo.Attributes.Add("src", "images/logo.png");
                    //ViewBag.ClientImage = "/img/logo1.png";
                    ViewBag.ClientImage = databaseCon.logoimage;
                }
            }

            //string qury = "select ClientPosition,ReceiptConfirmation,ReceivedByPosition,replace(CONVERT(varchar(11),SiteReceivedDate,106),' ','-') as SiteReceivedDate,TransportCompName from tblDeliveryDocket where DelDocId=" + databaseCon.Decrypt(Request.QueryString["DDId"]);
            //DataTable dat1 = databaseCon.GetData(qury);
            //if(dat1.Rows.Count>0)
            //{
            //    //txtTransport2.Text = dat1.Rows[0]["TransportCompName"].ToString();
            //    //txtposition.SelectedValue = dat1.Rows[0]["ReceivedByPosition"].ToString();
            //    //txtAcceptedposi.SelectedValue = dat1.Rows[0]["ClientPosition"].ToString();
            //    ViewBag.TransportCompName = dat1.Rows[0]["TransportCompName"].ToString();
            //    ViewBag.ReceivedByPosition = dat1.Rows[0]["ReceivedByPosition"].ToString();
            //    ViewBag.ClientPosition = dat1.Rows[0]["ClientPosition"].ToString();

            //    if (dat1.Rows[0]["SiteReceivedDate"].ToString() != "" && dat1.Rows[0]["SiteReceivedDate"].ToString() != "1900-01-01" && dat1.Rows[0]["SiteReceivedDate"].ToString() != "01-Jan-1900" && dat1.Rows[0]["SiteReceivedDate"] != DBNull.Value)
            //    {
            //        // lbldate.Text = dat1.Rows[0]["SiteReceivedDate"].ToString();
            //        ViewBag.SiteReceivedDate= dat1.Rows[0]["SiteReceivedDate"].ToString();
            //    }
            //}


            //string query2 = "select ReceiptConfirmation,TransportDriverSign from tblDeliveryDocket where DelDocId=" + databaseCon.Decrypt(Request.QueryString["DDId"]);
            //DataTable dt2 = databaseCon.GetData(query2);
            //if()
            //if (dt2.Rows[0]["ReceiptConfirmation"].ToString() != "NULL" && dt2.Rows[0]["ReceiptConfirmation"].ToString() != "")
            //{
            //    Confirmation.Style.Add("display", "block");
            //    btnSubmit.Style.Add("display", "none");
            //    TopSubmit.Style.Add("display", "none");
            //    confirmcheck.Checked = true;
            //    datePicker1.Enabled = false;
            //}
            //if (dt2.Rows[0]["TransportDriverSign"].ToString() != null && dt2.Rows[0]["TransportDriverSign"].ToString() != "")
            //{
            //    mainsite.Style.Add("display", "block");
            //}

            //string ClientQuery = "select ClientImage from tblClient where ClientId = '" + id + "'";
            //DataTable clientdt = databaseCon.GetData(ClientQuery);

            //if (clientdt.Rows.Count > 0)
            //{
            //    if (clientdt.Rows[0]["ClientImage"].ToString() != "")
            //    {
            //        //var uri = new Uri(databaseCon.clientimageLink);
            //        string imgname = clientdt.Rows[0]["ClientImage"].ToString();
            //        //var container = new CloudBlobContainer(uri);
            //        //var blob = container.GetBlockBlobReference(clientdt.Rows[0]["ClientImage"].ToString());
            //        //string a = databaseCon.siteimagelink + "Item1.png";
            //        // imga.Attributes.Add("src", databaseCon.siteimagelink + "Item1.png");
            //        //var exists = blob.Exists();
            //        if (clientdt.Rows[0]["ClientImage"].ToString() == "")
            //        {
            //            imageScaffold.Attributes.Add("src", "/images/logo.png");

            //        }
            //        else
            //        {
            //            //string a = databaseCon.clientimageLink + clientdt.Rows[0]["ClientImage"].ToString();
            //            //imageScaffold.Attributes.Add("src", databaseCon.clientimageLink + clientdt.Rows[0]["ClientImage"].ToString());
            //            imageScaffold.Attributes.Add("src", "/images/clientlogo/" + clientdt.Rows[0]["ClientImage"].ToString());
            //        }
            //    }
            //    else
            //    {
            //        imageScaffold.Attributes.Add("src", "images/logo.png");
            //    }
            //}


            return View(deliveryDocketViewModel);
        }

        public ActionResult GetClientRepresentationDropDownBind(string ClientID)
        {
            string qry = "select distinct CRId,Position from tblClientRepresentation where ClientId='" + databaseCon.Decrypt(ClientID) + "' order by Position";
            DataTable dt = databaseCon.GetData(qry);

            var GetData = dbContext.Database.SqlQuery<ClientRepresentationViewModel>("select distinct CRId,Position from tblClientRepresentation where ClientId='" + databaseCon.Decrypt(ClientID) + "' order by Position").ToList();

            return Json(GetData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDropDownBind(string ClientID)
        {
            var GetData = dbContext.Database.SqlQuery<UserViewModel>("select(firstName + ' ' + LastName) as FullName from tbluser where CategoryId = (select categoryId from tblUserCategory where Category = 'Yard Person' and ClientId = '" + databaseCon.Decrypt(ClientID) + "') and ClientId = '" + databaseCon.Decrypt(ClientID) + "' order by firstName").ToList();

            return Json(GetData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDeliveryData(string ClientID,string checkid)
        {
            string query = "select tblTransactionHistDelivery.TransDeliId,TransId,tblInventory.MaterialType as MaterialType,tblInventory.ItemCode as ItemCode,tblInventory.ItemDesc as ItemDesc,tblTransactionHistDelivery.Quantity as Quantity,convert(decimal(18,2),isnull(tblInventory.ItemWeight,0)) as ItemWeight,convert(decimal(18,2),isnull(tblInventory.ItemWeight*tblTransactionHistDelivery.Quantity,0)) as TotalWeight from tblTransactionHistRequest join VW_depotInventory on tblTransactionHistRequest.InventoryId=VW_depotInventory.InventoryId  left join tblTransactionHistDelivery on  tblTransactionHistRequest.TransReqId=tblTransactionHistDelivery.RequestId join tblInventory on tblTransactionHistDelivery.InventoryId=tblInventory.InventoryID where TransId in (" + databaseCon.Decrypt(checkid) + ")order by tblInventory.MaterialType,ISNULL(tblInventory.DisplaySortOrder,9999999),tblInventory.ItemCode";
            var DeliveryDocketData = dbContext.Database.SqlQuery<DeliveryDocketViewModel>(query).ToList();

            return Json(DeliveryDocketData, JsonRequestBehavior.AllowGet);

            // string query1 = "select tblTransactionHistDelivery.TransDeliId,TransId,tblInventory.MaterialType as MaterialType,tblInventory.ItemCode as ItemCode,tblInventory.ItemDesc as ItemDesc,tblTransactionHistDelivery.Quantity as Quantity,convert(decimal(18,2),isnull(tblInventory.ItemWeight,0)) as ItemWeight,convert(decimal(18,2),isnull(tblInventory.ItemWeight*tblTransactionHistDelivery.Quantity,0)) as TotalWeight from tblTransactionHistRequest join VW_depotInventory on tblTransactionHistRequest.InventoryId=VW_depotInventory.InventoryId  left join tblTransactionHistDelivery on  tblTransactionHistRequest.TransReqId=tblTransactionHistDelivery.RequestId join tblInventory on tblTransactionHistDelivery.InventoryId=tblInventory.InventoryID where TransId in (" + databaseCon.Decrypt(Request.QueryString["checkid"]) + ")order by tblInventory.MaterialType,tblInventory.ItemCode";

            //  DataTable dt = databaseCon.GetData(query1);
        }
        public ActionResult GetDeliveryDocketData(string ClientID, string checkid)
         {
            int DocketNo = 0;
            string query = "select DelDocId from tblTransaction join tblDeliveryDocket on tblTransaction.DeliveryId=tblDeliveryDocket.DeliveryID and tblTransaction.ClientId=tblDeliveryDocket.ClientId where TransId=" + databaseCon.Decrypt(checkid) + "and tblDeliveryDocket.ClientId=" + databaseCon.Decrypt(ClientID);
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                DocketNo = Convert.ToInt32(dt.Rows[0]["DelDocId"]);
            }
            //var DeliveryDocketData = dbContext.Database.SqlQuery<DeliveryDocketViewModel>("select * from tblDeliveryDocket where DelDocId='"+ DocketNo + "'").FirstOrDefault();

            var DeliveryDocketData = dbContext.Database.SqlQuery<DeliveryDocketViewModel>(
    @"SELECT DelDocId,
             DeliveryID,
             DepotPic,
             DepotVedio,
             DepotPickedBy,
             DepotPersonSign,
             TransportBy,
             TransportDriverSign,
             SitePic,
             SiteReceivedBy,
             SitePersonSign,
             AcceptedBy,
             AcceptorSign,
             ReceiptConfirmation,
             CreatedBy,
             CreatedOn,
             UpdatedBy,
             UpdatedOn,
             PickDate,
             ReceivedDate,
             TransportCompName,
             SiteReceivedDate,
             ReceivedByPosition,
             ClientPosition,
             PickedBy,
             ClientId
      FROM tblDeliveryDocket
      WHERE DelDocId = @DocketNo",
    new System.Data.SqlClient.SqlParameter("@DocketNo", DocketNo)
).FirstOrDefault();

            var DepotPersonSign = DeliveryDocketData.DepotPersonSign;
            var TransportDriverSign = DeliveryDocketData.TransportDriverSign;
            var SitePersonSign = DeliveryDocketData.SitePersonSign;
            var AcceptorSign = DeliveryDocketData.AcceptorSign;
            var DepotPic = DeliveryDocketData.DepotPic;
            var SitePic = DeliveryDocketData.SitePic;
           // var PickDate = DeliveryDocketData.PickDate;

            var DepotPersonSignUrl = "";
            var TransportDriverSignUrl = "";
            var SitePersonSignUrl = "";
            var AcceptorSignUrl = "";
            var DepotPicUrl = "";
            var SitePicUrl = "";
           // DateTime newPickDate ;

            if (DepotPersonSign != "" && DepotPersonSign!=null)
            {
                var uri = new Uri(databaseCon.siteimagelink);
                var container = new CloudBlobContainer(uri);
                //var blob = container.GetBlockBlobReference(dt.Rows[0]["DepotPersonSign"].ToString());
                var blob = container.GetBlockBlobReference(DepotPersonSign.ToString());
                var exists = blob.Exists();
                if (exists == true)
                {
                    DepotPersonSignUrl = databaseCon.siteimagelink + DepotPersonSign.ToString();
                    ////imga.Attributes.Add("src", databaseCon.siteimagelink + InspectDt.Rows[0]["Inspimage"].ToString());
                    //signimg1.Attributes.Add("src", databaseCon.siteimagelink + DepotPersonSign.ToString());

                    //// ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp5", "<script>RemoveSignatureBlock();</script>", false);

                    //printcanvas.Style.Add("display", "none");
                    //hdnSignImageBlock.Style.Add("display", "block");

                }
            }
         
            if (TransportDriverSign != "" && TransportDriverSign!=null)
            {
                var uri = new Uri(databaseCon.siteimagelink);
                var container = new CloudBlobContainer(uri);
                var blob = container.GetBlockBlobReference(TransportDriverSign.ToString());
                var exists = blob.Exists();
                if (exists == true)
                {
                    TransportDriverSignUrl = databaseCon.siteimagelink + TransportDriverSign.ToString();
                    //imga.Attributes.Add("src", databaseCon.siteimagelink + InspectDt.Rows[0]["Inspimage"].ToString());
                   // signimg2.Attributes.Add("src", databaseCon.siteimagelink + dt.Rows[0]["TransportDriverSign"].ToString());

                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp5", "<script>RemoveSignatureBlock();</script>", false);

                  //  Canvas1.Style.Add("display", "none");
                  //  hdnSignImageBlock1.Style.Add("display", "block");
                }
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp12", "<script>HideSignatureBlock();</script>", false);
            }

            if (SitePersonSign != "" && SitePersonSign != null)
            {
                var uri = new Uri(databaseCon.siteimagelink);
                var container = new CloudBlobContainer(uri);
                var blob = container.GetBlockBlobReference(SitePersonSign.ToString());
                var exists = blob.Exists();
                if (exists == true)
                {
                    SitePersonSignUrl = databaseCon.siteimagelink + SitePersonSign.ToString();
                    //imga.Attributes.Add("src", databaseCon.siteimagelink + InspectDt.Rows[0]["Inspimage"].ToString());
                   // signimg3.Attributes.Add("src", databaseCon.siteimagelink + dt.Rows[0]["SitePersonSign"].ToString());

                    // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp5", "<script>RemoveSignatureBlock();</script>", false);

                   // Canvas2.Style.Add("display", "none");
                  //  hdnSignImageBlock2.Style.Add("display", "block");
                }
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp12", "<script>HideSignatureBlock();</script>", false);
            }
          
            if (AcceptorSign != "" && AcceptorSign!=null)
            {
                var uri = new Uri(databaseCon.siteimagelink);
                var container = new CloudBlobContainer(uri);
                var blob = container.GetBlockBlobReference(AcceptorSign.ToString());
                var exists = blob.Exists();
                if (exists == true)
                {
                    AcceptorSignUrl = databaseCon.siteimagelink + AcceptorSign.ToString();
                    //imga.Attributes.Add("src", databaseCon.siteimagelink + InspectDt.Rows[0]["Inspimage"].ToString());
                   // signimg4.Attributes.Add("src", databaseCon.siteimagelink + dt.Rows[0]["AcceptorSign"].ToString());

                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp5", "<script>RemoveSignatureBlock();</script>", false);

                   // Canvas3.Style.Add("display", "none");
                   // hdnSignImageBlock3.Style.Add("display", "block");
                }
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp12", "<script>HideSignatureBlock();</script>", false);
            }
            if (DepotPic != "" && DepotPic!=null)
            {
                var uri = new Uri(databaseCon.siteimagelink);
                var container = new CloudBlobContainer(uri);
                var blob = container.GetBlockBlobReference(DepotPic.ToString());
                var exists = blob.Exists();
                if (exists == true)
                {
                    DepotPicUrl = databaseCon.siteimagelink + DepotPic.ToString();
                    //imagetruck.Attributes.Add("src", databaseCon.siteimagelink + dt.Rows[0]["DepotPic"].ToString());
                    //imagetruck.Attributes.Add("src", databaseCon.siteimagelink + dt.Rows[0]["DepotPic"].ToString());
                    ////ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp", "<script>SafariCheck(" + 1 + ",null,null);</script>", false);

                    //Photoblock.Style.Add("display", "none");
                    //imagetruck.Style.Add("display", "block");
                }
                //  getCamera.Style.Add("display", "none");
            }
            if (SitePic != "" && SitePic!=null)
            {
                var uri = new Uri(databaseCon.siteimagelink);
                var container = new CloudBlobContainer(uri);
                var blob = container.GetBlockBlobReference(SitePic.ToString());
                var exists = blob.Exists();
                if (exists == true)
                {
                    SitePicUrl = databaseCon.siteimagelink + SitePic.ToString();
                 //   image.Attributes.Add("src", databaseCon.siteimagelink + dt.Rows[0]["SitePic"].ToString());
                 //   image.Attributes.Add("src", databaseCon.siteimagelink + dt.Rows[0]["SitePic"].ToString());
                 ////   ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp", "<script>SafariCheck(" + 1 + ",null,null);</script>", false);

                 //   photoblock2.Style.Add("display", "none");
                 //   image.Style.Add("display", "block");
                }
                //    getCamera.Style.Add("display", "none");
            }

            //if (PickDate != null && Convert.ToString(PickDate) != "01/01/1900 12:00:00 AM")
            //{
            //    // datePicker1.SelectedDate = Convert.ToDateTime(dt.Rows[0]["PickDate"]);
            //    newPickDate = Convert.ToDateTime(PickDate);
            //}
            //else
            //{
            //    string qury = "select convert(Varchar,getdate(),106) as Date";
            //    DataTable dt2 = databaseCon.GetData(qury);
            //    // datePicker1.SelectedDate = Convert.ToDateTime(dt2.Rows[0]["Date"].ToString());
            //    newPickDate= Convert.ToDateTime(dt2.Rows[0]["Date"].ToString());
            //}

            DeliveryDocketData.DepotPersonSign = DepotPersonSignUrl;
            DeliveryDocketData.TransportDriverSign = TransportDriverSignUrl;
            DeliveryDocketData.SitePersonSign = SitePersonSignUrl;
            DeliveryDocketData.AcceptorSign = AcceptorSignUrl;
            DeliveryDocketData.DepotPic = DepotPicUrl;
            DeliveryDocketData.SitePic = SitePicUrl;
            

            return Json(DeliveryDocketData, JsonRequestBehavior.AllowGet);
        }

        public int btnConfirmok_Click(string ClientID, string checkid, string txtAccepted, string txtAcceptedposi, string confirmcheck ) {
            int result = 0;
            var DDId = "";
            string query1 = "select DelDocId from tblTransaction join tblDeliveryDocket on tblTransaction.DeliveryId=tblDeliveryDocket.DeliveryID and tblTransaction.ClientId=tblDeliveryDocket.ClientId where TransId=" + databaseCon.Decrypt(checkid) + "and tblDeliveryDocket.ClientId=" + databaseCon.Decrypt(ClientID);
            DataTable dt = databaseCon.GetData(query1);
            if (dt.Rows.Count > 0)
            {
                DDId = dt.Rows[0]["DelDocId"].ToString();
            }

            string confirmQuery = "update tblDeliveryDocket set ReceiptConfirmation = 1, AcceptedBy = '" + txtAccepted.Replace("'", "''").Trim() + "', ClientPosition = '" + txtAcceptedposi+ "' where DelDocId='" + DDId + "'";
            int confirmCount = databaseCon.RunCmd(confirmQuery);
            if (confirmCount > 0)
            {
                result = 1;
            }
            return result;
        }

        public int DelieveryDocketUpload(string ClientID,string checkid, string imageData,string imageData1,string sketchData,string sketchData1,string sketchData2,string sketchData3, string datePicker1, string SiteReceivedDate,string txtReceived,string txtposition,string txtpickedby,string txtTransport1,string txtTransport2)
        {
            bool flag = false;

            //if (confirmcheck.Checked == true)
            //{
            //    confirmModal.Style.Add("display", "block");
            //}


            string Date = "";
            //if (Datepicker.SelectedDate != null)
            //{
            //    Date = Convert.ToString(Datepicker.SelectedDate);
            //    hdnfld.Value = "";
            //}
            //else if (hdnfld.Value != "")
            //{
            //    Date = Convert.ToString(hdnfld.Value);
            //    hdnfld.Value = "";
            //}


            var DDId = "";
            string query1 = "select DelDocId from tblTransaction join tblDeliveryDocket on tblTransaction.DeliveryId=tblDeliveryDocket.DeliveryID and tblTransaction.ClientId=tblDeliveryDocket.ClientId where TransId=" + databaseCon.Decrypt(checkid) + "and tblDeliveryDocket.ClientId=" + databaseCon.Decrypt(ClientID);
            DataTable dt = databaseCon.GetData(query1);
            if (dt.Rows.Count > 0)
            {
                DDId =dt.Rows[0]["DelDocId"].ToString();
            }


            // string query = "update tblDeliveryDocket set PickDate = '" + datePicker1.SelectedDate + "',SiteReceivedBy='" + txtReceived.Text.Replace("'", "''").Trim() + "',ReceivedByPosition='" + txtposition.SelectedValue + "', SiteReceivedDate = '" + Date + "',DepotPickedBy='" + txtpickedby.SelectedValue + "', TransportBy = '" + txtTransport1.Text.Replace("'", "''").Trim() + "', TransportCompName='" + txtTransport2.Text.Replace("'", "''").Trim() + "' where DelDocId='" + databaseCon.Decrypt(Request.QueryString["DDId"]) + "'";

            string query = "update tblDeliveryDocket set PickDate = '" + datePicker1+ "',SiteReceivedBy='" + txtReceived.Replace("'", "''").Trim() + "',ReceivedByPosition='" + txtposition + "', SiteReceivedDate = '" + SiteReceivedDate + "',DepotPickedBy='" + txtpickedby + "', TransportBy = '" + txtTransport1.Replace("'", "''").Trim() + "', TransportCompName='" + txtTransport2.Replace("'", "''").Trim() + "' where DelDocId='" + DDId + "'";
            int i = databaseCon.RunCmdID(query);
            if (i > 0)
            {
                string querydate = "select replace( convert(varchar, SiteReceivedDate,106),' ','-') as SiteReceivedDate  from tblDeliveryDocket where  DelDocId='" + databaseCon.Decrypt(Request.QueryString["DDId"]) + "'";
                //DataTable dtdate = databaseCon.GetData(querydate);
                //if (dtdate.Rows[0]["SiteReceivedDate"].ToString() != "" && dtdate.Rows[0]["SiteReceivedDate"].ToString() != "1900-01-01" && dtdate.Rows[0]["SiteReceivedDate"].ToString() != "01-Jan-1900" && dtdate.Rows[0]["SiteReceivedDate"] != DBNull.Value)
                //{
                //   // lbldate.Text = dtdate.Rows[0]["SiteReceivedDate"].ToString();
                //}
                //Session["Insert"] = "Insert";
            }


            if (imageData != "" && imageData != null)
            {
                imageUpload(imageData, DDId);
                flag = true;
            }

            if (imageData1 != "" && imageData1 != null)
            {
                imageUpload1(imageData1, DDId);
                flag = true;
            }

            if (sketchData != "" && sketchData != null)
            {
                SignatureUpload3(sketchData, DDId);
                flag = true;
            }
            if (sketchData1 != "" && sketchData1 != null)
            {
                SignatureUpload4(sketchData1, DDId);
                flag = true;

            }

            if (sketchData2 != "" && sketchData2 != null)
            {

                SignatureUpload1(sketchData2, DDId);
                flag = true;
            }
            if (sketchData3 != "" && sketchData3 != null)
            {
                SignatureUpload2(sketchData3, DDId);
                flag = true;
            }

         
            return 1;
        }


        protected void imageUpload(string imageData,string DDId)
        {
        
            string path = "~/uploads/";
            path = Server.MapPath(path);
            // string FileName = "DepotPic" + "_" + DateTime.Now.Ticks + "_" + databaseCon.Decrypt(Request.QueryString["DDId"]) + ".jpg";
            string FileName = "DepotPic" + "_" + DateTime.Now.Ticks + "_" + DDId + ".jpg";
            string newPath = path + FileName;

            byte[] bytes = Convert.FromBase64String(imageData.Split(',')[1]);

            var uri = new Uri(databaseCon.siteimagelink);
            var container = new CloudBlobContainer(uri);
            var blob = container.GetBlockBlobReference(FileName);
            var exists = blob.Exists();
            if (exists == false)
            {
                using (var imageFile = new FileStream(newPath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }

               // if (File.Exists(newPath))
               if(System.IO.File.Exists(newPath))
                {
                    string fpath = path + FileName;
                    databaseCon.savefileonblob(FileName, "Site-images", fpath, "image/jpeg");
                    System.IO.File.Delete(fpath);
                }
            }
            else
            {
                databaseCon.deletefileonblob(FileName, "siteimage");

                using (var imageFile = new FileStream(newPath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }


                if (System.IO.File.Exists(newPath))
                {
                    string fpath = path + FileName;
                    databaseCon.savefileonblob(FileName, "Site-images", fpath, "image/jpeg");
                    System.IO.File.Delete(fpath);
                }
            }


            // string querystr = "update tblDeliveryDocket set DepotPic = '" + FileName + "' where DelDocId='" + databaseCon.Decrypt(Request.QueryString["DDId"]) + "'";
            string querystr = "update tblDeliveryDocket set DepotPic = '" + FileName + "' where DelDocId='" + DDId + "'";
            int count = databaseCon.RunCmd(querystr);
        }

        protected void imageUpload1(string imageData,string DDId)
        {
            string path = "~/uploads/";
            path = Server.MapPath(path);
            string FileName = "SitePic" + "_" + DateTime.Now.Ticks + "_" + DDId + ".jpg";
            string newPath = path + FileName;

            byte[] bytes = Convert.FromBase64String(imageData.Split(',')[1]);

            var uri = new Uri(databaseCon.siteimagelink);
            var container = new CloudBlobContainer(uri);
            var blob = container.GetBlockBlobReference(FileName);
            var exists = blob.Exists();
            if (exists == false)
            {
                using (var imageFile = new FileStream(newPath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }

                if (System.IO.File.Exists(newPath))
                {
                    string fpath = path + FileName;
                    databaseCon.savefileonblob(FileName, "Site-images", fpath, "image/jpeg");
                    System.IO.File.Delete(fpath);
                }
            }
            else
            {
                databaseCon.deletefileonblob(FileName, "siteimage");

                using (var imageFile = new FileStream(newPath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }


                if (System.IO.File.Exists(newPath))
                {
                    string fpath = path + FileName;
                    databaseCon.savefileonblob(FileName, "Site-images", fpath, "image/jpeg");
                    System.IO.File.Delete(fpath);
                }
            }

            //string querystr = "update tblDelieveryDocket set SitePic='" + FileName + "' where DDId ='" + DDId + "'";
            //   string querystr = "insert into tblDeliveryDocket(SitePic,DelDocId) values ('" + FileName + "','" + DDId + "')";
            string querystr = "update tblDeliveryDocket set SitePic = '" + FileName + "' where DelDocId='" + DDId + "'";
            int count = databaseCon.RunCmd(querystr);
        }



        protected void SignatureUpload1(string sketchData2,string DDId)
        {
            string path = "~/uploads/";
            path = Server.MapPath(path);
            string FileName = "SitePersonSign" + DateTime.Now.Ticks + DDId + ".png";
            string SketchnewPath = path + FileName;


            byte[] bytes = Convert.FromBase64String(sketchData2.Split(',')[1]);

            var uri = new Uri(databaseCon.siteimagelink);
            var container = new CloudBlobContainer(uri);
            var blob = container.GetBlockBlobReference(FileName);
            var exists = blob.Exists();
            if (exists == false)
            {
                using (var imageFile = new FileStream(SketchnewPath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }

                if (System.IO.File.Exists(SketchnewPath))
                {
                    string fpath = path + FileName;
                    databaseCon.savefileonblob(FileName, "Site-images", fpath, "image/png");
                    System.IO.File.Delete(fpath);
                }
            }
            else
            {
                databaseCon.deletefileonblob(FileName, "siteimage");

                using (var imageFile = new FileStream(SketchnewPath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }

                if (System.IO.File.Exists(SketchnewPath))
                {
                    string fpath = path + FileName;
                    databaseCon.savefileonblob(FileName, "Site-images", fpath, "image/png");
                    System.IO.File.Delete(fpath);
                }
            }


            //string querystr = "update tblDeliveryDocket set DepotPersonSign='" + FileName + "' where DDId ='" + DDId + "'";
            //    string querystr = "insert into tblDeliveryDocket(DepotPersonSign,DeliveryID) values ('"+FileName +"','"+DDId+"')";
            string querystr = "update tblDeliveryDocket set SitePersonSign = '" + FileName + "' where DelDocId='" + DDId + "'";
            int count = databaseCon.RunCmd(querystr);
        }


        protected void SignatureUpload2(string sketchData2,string DDId)
        {
            string path = "~/uploads/";
            path = Server.MapPath(path);
            string FileName = "AcceptorSign" + DateTime.Now.Ticks + DDId + ".png";
            string SketchnewPath = path + FileName;


            byte[] bytes = Convert.FromBase64String(sketchData2.Split(',')[1]);

            var uri = new Uri(databaseCon.siteimagelink);
            var container = new CloudBlobContainer(uri);
            var blob = container.GetBlockBlobReference(FileName);
            var exists = blob.Exists();
            if (exists == false)
            {
                using (var imageFile = new FileStream(SketchnewPath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }

                if (System.IO.File.Exists(SketchnewPath))
                {
                    string fpath = path + FileName;
                    databaseCon.savefileonblob(FileName, "Site-images", fpath, "image/png");
                    System.IO.File.Delete(fpath);
                }
            }
            else
            {
                databaseCon.deletefileonblob(FileName, "siteimage");

                using (var imageFile = new FileStream(SketchnewPath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }

                if (System.IO.File.Exists(SketchnewPath))
                {
                    string fpath = path + FileName;
                    databaseCon.savefileonblob(FileName, "Site-images", fpath, "image/png");
                    System.IO.File.Delete(fpath);
                }
            }


            // string querystr = "update tblDeliveryDocket set TransportDriverSign='" + FileName + "' where DDId ='" + DDId + "'";
            //string querystr = "insert into tblDeliveryDocket(TransportDriverSign,DeliveryID) values ('" + FileName + "','" + DDId + "')";
            string querystr = "update tblDeliveryDocket set AcceptorSign = '" + FileName + "' where DelDocId='" + DDId + "'";
            int count = databaseCon.RunCmd(querystr);
        }



        protected void SignatureUpload3(string sketchData2,string DDId)
        {
            string path = "~/uploads/";
            path = Server.MapPath(path);
            string FileName = "DepotPersonSign" + DateTime.Now.Ticks + DDId + ".png";
            string SketchnewPath = path + FileName;


            byte[] bytes = Convert.FromBase64String(sketchData2.Split(',')[1]);

            var uri = new Uri(databaseCon.siteimagelink);
            var container = new CloudBlobContainer(uri);
            var blob = container.GetBlockBlobReference(FileName);
            var exists = blob.Exists();
            if (exists == false)
            {
                using (var imageFile = new FileStream(SketchnewPath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }

                if (System.IO.File.Exists(SketchnewPath))
                {
                    string fpath = path + FileName;
                    databaseCon.savefileonblob(FileName, "Site-images", fpath, "image/png");
                    System.IO.File.Delete(fpath);
                }
            }
            else
            {
                databaseCon.deletefileonblob(FileName, "siteimage");

                using (var imageFile = new FileStream(SketchnewPath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }

                if (System.IO.File.Exists(SketchnewPath))
                {
                    string fpath = path + FileName;
                    databaseCon.savefileonblob(FileName, "Site-images", fpath, "image/png");
                    System.IO.File.Delete(fpath);
                }
            }

            string querystr = "update tblDeliveryDocket set DepotPersonSign = '" + FileName + "' where DelDocId='" + DDId + "'";
            int count = databaseCon.RunCmd(querystr);
        }


        protected void SignatureUpload4(string sketchData2,string DDId)
        {
            string path = "~/uploads/";
            path = Server.MapPath(path);
            string FileName = "TransportDriverSign" + DateTime.Now.Ticks + DDId + ".png";
            string SketchnewPath = path + FileName;


            byte[] bytes = Convert.FromBase64String(sketchData2.Split(',')[1]);

            var uri = new Uri(databaseCon.siteimagelink);
            var container = new CloudBlobContainer(uri);
            var blob = container.GetBlockBlobReference(FileName);
            var exists = blob.Exists();
            if (exists == false)
            {
                using (var imageFile = new FileStream(SketchnewPath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }

                if (System.IO.File.Exists(SketchnewPath))
                {
                    string fpath = path + FileName;
                    databaseCon.savefileonblob(FileName, "Site-images", fpath, "image/png");
                    System.IO.File.Delete(fpath);
                }
            }
            else
            {
                databaseCon.deletefileonblob(FileName, "siteimage");

                using (var imageFile = new FileStream(SketchnewPath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }

                if (System.IO.File.Exists(SketchnewPath))
                {
                    string fpath = path + FileName;
                    databaseCon.savefileonblob(FileName, "Site-images", fpath, "image/png");
                    System.IO.File.Delete(fpath);
                }
            }


            string querystr = "update tblDeliveryDocket set TransportDriverSign = '" + FileName + "' where DelDocId='" + DDId + "'";
            int count = databaseCon.RunCmd(querystr);
        }

        public JsonResult CheckTransactionSiteStatus(string transID)
        {
            try
            {
                string query = "SELECT SiteLocationStatus FROM tblTransaction  JOIN tblSiteLocation  ON tblTransaction.FromId = tblSiteLocation.SiteLocID WHERE tblTransaction.TransId = '" + transID + "'";
                DataTable dt = databaseCon.GetData(query);
                bool IsInactive = false;
                if (dt.Rows.Count > 0)
                {
                    IsInactive = dt.Rows[0]["SiteLocationStatus"].ToString() == "InActive";
                }
                return Json(new { IsInactive = IsInactive }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { IsInactive = false }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}