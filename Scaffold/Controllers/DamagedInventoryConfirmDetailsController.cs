using Microsoft.WindowsAzure.Storage.Blob;
using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scaffold.Controllers
{
    public class DamagedInventoryConfirmDetailsController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: DamagedInventoryConfirmDetails
        public ActionResult DamagedInventoryConfirmDetails(string UID, string UserRole, string fromtocheck, string id, string FromId, string ToId, string DamagedInvId, string page, string DIDetailID, string FromName)
        {
            return View();
        }

        public ActionResult DamagedConfDetailsData(string id, string RInvId, string DamagedInvId, string DIDetailID, string FromId, string UserRole, string ToId, string fromtocheck)
        {

            DataTable dt;
            int AvailableQuantity = 0;
           var fromtoCheck = databaseCon.Decrypt(fromtocheck);
            string hdnAvailableQty;
            //Site Avaliable Quantity Check
            if (fromtoCheck == "1")
            {
                string querysite;
                querysite = "select cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(DamagedAll.DamagedAllQuantity, 0) - Isnull(SentToJobQuantity, 0) - isnull(ReturnAllQuantity, 0) + Isnull(ReturnFromJobQuantity,0)) as int) as AvailableQuantity,DIDetailID from(select FromId, ToId, MaterialType, ItemCode, ItemDesc, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Depot' and[to] = 'Site' and TransType = 'Send' group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc) DeliveryAll left join (select FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Site' and[to] = 'Depot' group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.ToId = ReturnAll.FromId  and DeliveryAll.InventoryId = ReturnAll.InventoryId left join (select FromId, ToId, InventoryId, Sum(DamagedQuantity) as DamagedAllQuantity from tbldamagedInventoryDetails  join tbldamagedInventory on tbldamagedInventoryDetails.DamagedINvId = tbldamagedInventory.DamagedINvId where [FromName] = 'Site' and[ToName] = 'Depot' group by[FromName],[ToName], FromId, ToId, InventoryId )DamagedAll On DeliveryAll.FromId = DamagedAll.ToId and DeliveryAll.ToId = DamagedAll.FromId  and DeliveryAll.InventoryId = DamagedAll.InventoryId left join (select tblDamagedInventoryDetails.DIDetailID, tblDamagedInventoryDetails.DamagedInvId, FromId, ToId,Reason, tblDamagedInventoryDetails.InventoryId, Sum(DamagedQuantity) as DamagedQuantity from tblDamagedInventoryDetails  join tblDamagedInventory on tblDamagedInventoryDetails.DamagedInvId = tblDamagedInventory.DamagedInvId where [FromName] = 'Site' and[toName] = 'Depot' and tblDamagedInventoryDetails.DamagedInvId = '" + databaseCon.Decrypt(DamagedInvId) + "'  group by Reason,[FromName],[ToName], FromId, ToId, tblDamagedInventoryDetails.InventoryId, tblDamagedInventoryDetails.DamagedInvId, tblDamagedInventoryDetails.DIDetailID )DamagedQuery On DeliveryAll.FromId = DamagedQuery.ToId and DeliveryAll.ToId = DamagedQuery.FromId  and DeliveryAll.InventoryId = DamagedQuery.InventoryId Left join (select tblReturnInventoryDetails.RIDetailID, tblReturnInventoryDetails.ReturnInvId, FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Site' and[to] = 'Depot' and tblReturnInventoryDetails.ReturnInvId = '" + databaseCon.Decrypt(RInvId) + "' group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId, tblReturnInventoryDetails.ReturnInvId, tblReturnInventoryDetails.RIDetailID )ReturnQuery On DeliveryAll.FromId = ReturnQuery.ToId and DeliveryAll.ToId = ReturnQuery.FromId  and DeliveryAll.InventoryId = ReturnQuery.InventoryId left join (select FromId, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as SentToJobQuantity from tblTransactionHistDelivery where [From] = 'Site' and[to] = 'Job' and TransType = 'Send' group by[From],[To], FromId, tblTransactionHistDelivery.InventoryId )SentToJob On  DeliveryAll.ToId = SentToJob.FromId  and DeliveryAll.InventoryId = SentToJob.InventoryId Left join (select ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnFromJobQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site' group by[From],[To], ToId, tblReturnInventoryDetails.InventoryId )ReturnFromJob On  DeliveryAll.ToId = ReturnFromJob.ToId  and DeliveryAll.InventoryId = ReturnFromJob.InventoryId Left join (select ToId, tbldamagedInventoryDetails.InventoryId, Sum(DamagedQuantity) as DamagedFromJobQuantity from tbldamagedInventoryDetails  join tbldamagedInventory on tbldamagedInventoryDetails.DamagedINvId = tbldamagedInventory.DamagedINvId where [FromName] = 'Job' and[toName] = 'Site'group by[FromName],[ToNAME], ToId, InventoryId )DamagedFromJob On  DeliveryAll.ToId = DamagedFromJob.ToId  and DeliveryAll.InventoryId = DamagedFromJob.InventoryId where DeliveryAll.FromId = '" + databaseCon.Decrypt(id) + "' and DeliveryAll.ToId = '" + databaseCon.Decrypt(FromId) + "'and DIDetailID='" + databaseCon.Decrypt(DIDetailID) + "' order by MaterialType,ItemCode";
                dt = databaseCon.GetData(querysite);
                if (dt.Rows.Count > 0)
                {
                    AvailableQuantity = Convert.ToInt32(dt.Rows[0]["AvailableQuantity"]);
                    // hdnAvailableQty.Value = Convert.ToString(AvailableQuantity);
                    hdnAvailableQty = Convert.ToString(AvailableQuantity);
                }

            }
            //Job Avaliable Quantity Check
            // else if (databaseCon.Decrypt(Request.QueryString["fromtocheck"]) == "2")
            else if (fromtoCheck == "2")
            {

                string queryjob = "select cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(DamagedAll.DamagedAllQuantity, 0) - Isnull(SentToJobQuantity, 0) + Isnull(DamagedFromJobQuantity, 0) - isnull(ReturnAllQuantity, 0)) as int) as AvailableQuantity,DIDetailID from(select FromId, ToId, MaterialType, ItemCode, ItemDesc, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Site' and[to] = 'Job' and TransType = 'Send' group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc) DeliveryAll left join(select FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site' group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.ToId = ReturnAll.FromId  and DeliveryAll.InventoryId = ReturnAll.InventoryId left join (select FromId, ToId, InventoryId, Sum(DamagedQuantity) as DamagedAllQuantity from tbldamagedInventoryDetails  join tbldamagedInventory on tbldamagedInventoryDetails.damagedINvId = tbldamagedInventory.damagedINvId where [FromName] = 'Job' and[ToName] = 'Site' group by[FromName],[ToName], FromId, ToId, InventoryId )DamagedAll On DeliveryAll.FromId = DamagedAll.ToId and DeliveryAll.ToId = DamagedAll.FromId  and DeliveryAll.InventoryId = DamagedAll.InventoryId left join (select tblDamagedInventoryDetails.DIDetailID, tblDamagedInventoryDetails.DamagedInvId, FromId, ToId, Reason, tblDamagedInventoryDetails.InventoryId, Sum(DamagedQuantity) as DamagedQuantity from tblDamagedInventoryDetails  join tblDamagedInventory on tblDamagedInventoryDetails.DamagedInvId = tblDamagedInventory.DamagedInvId where [FromName] = 'Job' and[toName] = 'Site' and tblDamagedInventoryDetails.DamagedInvId =  '" + databaseCon.Decrypt(DamagedInvId) + "'  group by Reason,[FromName],[ToName], FromId, ToId, tblDamagedInventoryDetails.InventoryId, tblDamagedInventoryDetails.DamagedInvId, tblDamagedInventoryDetails.DIDetailID )DamagedQuery On DeliveryAll.FromId = DamagedQuery.ToId and DeliveryAll.ToId = DamagedQuery.FromId  and DeliveryAll.InventoryId = DamagedQuery.InventoryId Left join (select tblReturnInventoryDetails.RIDetailID, tblReturnInventoryDetails.ReturnInvId, FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Site' and[to] = 'Job' and tblReturnInventoryDetails.ReturnInvId = '" + databaseCon.Decrypt(RInvId) + "' group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId, tblReturnInventoryDetails.ReturnInvId, tblReturnInventoryDetails.RIDetailID)ReturnQuery On DeliveryAll.FromId = ReturnQuery.ToId and DeliveryAll.ToId = ReturnQuery.FromId  and DeliveryAll.InventoryId = ReturnQuery.InventoryId left join (select FromId, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as SentToJobQuantity from tblTransactionHistDelivery where [From] = 'Site' and[to] = 'Job' and TransType = 'Send' group by[From],[To], FromId, tblTransactionHistDelivery.InventoryId )SentToJob On  DeliveryAll.ToId = SentToJob.FromId  and DeliveryAll.InventoryId = SentToJob.InventoryId Left join(select ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnFromJobQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site' group by[From],[To], ToId, tblReturnInventoryDetails.InventoryId )ReturnFromJob On  DeliveryAll.ToId = ReturnFromJob.ToId  and DeliveryAll.InventoryId = ReturnFromJob.InventoryId Left join (select ToId, tbldamagedInventoryDetails.InventoryId, Sum(DamagedQuantity) as DamagedFromJobQuantity from tbldamagedInventoryDetails  join tbldamagedInventory on tbldamagedInventoryDetails.damagedINvId = tbldamagedInventory.damagedINvId where [FromName] = 'Job' and[toName] = 'Site' group by [FromName],[ToNAME], ToId, InventoryId )DamagedFromJob On  DeliveryAll.ToId = DamagedFromJob.ToId  and DeliveryAll.InventoryId = DamagedFromJob.InventoryId where DeliveryAll.FromId ='" + databaseCon.Decrypt(ToId) + "' and DeliveryAll.ToId='" + databaseCon.Decrypt(FromId) + "'and DIDetailID='" + databaseCon.Decrypt(DIDetailID) + "'order by MaterialType,ItemCode";

                dt = databaseCon.GetData(queryjob);
                if (dt.Rows.Count > 0)
                {
                    AvailableQuantity = Convert.ToInt32(dt.Rows[0]["AvailableQuantity"]);
                    // hdnAvailableQty.Value = Convert.ToString(AvailableQuantity);
                    hdnAvailableQty = Convert.ToString(AvailableQuantity);
                }
            }

            //Depot Avaliable Quantity Check
            else if (fromtoCheck == "3")
            {
                //string querydepot = "select AvailableQuantity,DIDetailID,isnull(DamagedQuantity,0) as DamagedQuantity ,Reason, VW_DepotInventory.InventoryId from VW_DepotInventory left join(select  Reason,DIDetailID,clientId, tblDamagedInventoryDetails.InventoryId, Sum(isnull(DamagedQuantity, 0)) as DamagedQuantity from tblDamagedInventoryDetails join tblDamagedInventory on tblDamagedInventoryDetails.DamagedInvId = tblDamagedInventory.DamagedInvId where [FromName] = 'Depot' and[toName] = 'Depot' and clientId = '" + databaseCon.Decrypt(id) + "' and tblDamagedInventoryDetails.DamagedInvId='"+ databaseCon.Decrypt(DamagedInvId) +"' group by[FromName],[ToName], tblDamagedInventoryDetails.InventoryId, clientId,DIDetailID,Reason) DamageQuery on VW_DepotInventory.InventoryID = DamageQuery.InventoryID and VW_DepotInventory.ClientID = DamageQuery.clientId   where VW_DepotInventory.ClientID = '" + databaseCon.Decrypt(id) + "' and DIDetailID='" + databaseCon.Decrypt(DIDetailID) + "' order by MaterialType,ItemCode";
                string querydepot = "select AvailableQuantity,DIDetailID,isnull(DamagedQuantity,0) as DamagedQuantity ,Reason, VW_DepotInventory.InventoryId from VW_DepotInventory left join(select  Reason,DIDetailID,clientId, tblDamagedInventoryDetails.InventoryId, Sum(isnull(DamagedQuantity, 0)) as DamagedQuantity from tblDamagedInventoryDetails join tblDamagedInventory on tblDamagedInventoryDetails.DamagedInvId = tblDamagedInventory.DamagedInvId where [FromName] = 'Depot' and[toName] = 'ThirdParty' and clientId = '" + databaseCon.Decrypt(id) + "' and tblDamagedInventoryDetails.DamagedInvId='" + databaseCon.Decrypt(DamagedInvId) + "' group by[FromName],[ToName], tblDamagedInventoryDetails.InventoryId, clientId,DIDetailID,Reason) DamageQuery on VW_DepotInventory.InventoryID = DamageQuery.InventoryID and VW_DepotInventory.ClientID = DamageQuery.clientId   where VW_DepotInventory.ClientID = '" + databaseCon.Decrypt(id) + "' and DIDetailID='" + databaseCon.Decrypt(DIDetailID) + "' order by MaterialType,ItemCode";
                dt = databaseCon.GetData(querydepot);
                if (dt.Rows.Count > 0)
                {
                    AvailableQuantity = Convert.ToInt32(dt.Rows[0]["AvailableQuantity"]);
                    // hdnAvailableQty.Value = Convert.ToString(AvailableQuantity);
                    hdnAvailableQty = Convert.ToString(AvailableQuantity);
                }

            }


           // string querfinal = "Select DIDetailID,MaterialType, ItemCode, ItemDesc,ItemWeight ,Supplier, SupplierDocketNo, Reason, Replace(convert(nvarchar,convert(datetime,DateActioned,120),106),' ','-') as DateActioned,'420' as AvaQty, DamagedQuantity, (DamagedQuantity*ItemWeight) as TotalItemWeight from tblDamagedInventoryDetails join tblInventory on tblDamagedInventoryDetails.InventoryId = tblInventory.InventoryID where DIDetailID='" + databaseCon.Decrypt(DIDetailID) + "'";
            // dt = databaseCon.GetData(query);
            var getData = dbContext.Database.SqlQuery<DamageInventoryConfirmDetailsViewModel>("Select DIDetailID,MaterialType, ItemCode, ItemDesc,ItemWeight ,Supplier, SupplierDocketNo, Reason, Replace(convert(nvarchar,convert(datetime,DateActioned,120),106),' ','-') as DateActioned,'"+ AvailableQuantity + "' as AvaQty, DamagedQuantity, (DamagedQuantity*ItemWeight) as TotalItemWeight from tblDamagedInventoryDetails join tblInventory on tblDamagedInventoryDetails.InventoryId = tblInventory.InventoryID where DIDetailID='" + databaseCon.Decrypt(DIDetailID) + "'").ToList();
            // DamagedConfGrid.DataSource = dt;

            return Json(getData, JsonRequestBehavior.AllowGet);
        }


        public ActionResult BatchEditCommand(string DIDDetail, string Supplier, string SupplierDocketNo, string DateActioned)
        {
            string Result = "";
            string Message = "";

            //if (e.Commands.Count > 0)
            //{
            //    {
            //        foreach (GridBatchEditingCommand command in e.Commands)
            //        {
            //            if ((command.Type == GridBatchEditingCommandType.Update))
            //            {
            //                Hashtable newValues = command.NewValues;
            //                string Supplier = newValues["Supplier"].ToString();
            //                string SupplierDocketNo = newValues["SupplierDocketNo"].ToString();
            //                string DateActioned = newValues["DateActioned"].ToString();
            //                string queryupdate = "update tblDamagedInventoryDetails set Supplier='" + Supplier.Replace("'", "''").Trim() + "', SupplierDocketNo='" + SupplierDocketNo.Replace("'", "''").Trim() + "',DateActioned=" + (DateActioned == "" ? "null" : "'" + DateActioned + "'") + " where DIDetailID=" + databaseCon.Decrypt(Request.QueryString["DIDetailID"]) + "";
            //                int qupdate = databaseCon.RunCmdID(queryupdate);

            //                if (qupdate > 0)
            //                {
            //                    art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
            //                    art1.Style.Add("display", "block");
            //                    alertlabel.Text = "<strong>Success ! </strong>Data Saved Successfully.";
            //                    DamagedConfGrid.Rebind();
            //                }

            //            }
            //        }
            //    }
            //}


            string queryupdate = "update tblDamagedInventoryDetails set Supplier='" + Supplier.Replace("'", "''").Trim() + "', SupplierDocketNo='" + SupplierDocketNo.Replace("'", "''").Trim() + "',DateActioned=" + (DateActioned == "" ? "null" : "'" + DateActioned + "'") + " where DIDetailID=" + DIDDetail + "";
            int qupdate = databaseCon.RunCmdID(queryupdate);

            if (qupdate > 0)
            {
                //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                //art1.Style.Add("display", "block");
                //alertlabel.Text = "<strong>Success ! </strong>Data Saved Successfully.";
                //DamagedConfGrid.Rebind();

                Result = "Success";
                Message = "Data Saved Successfully.";

            }
            var response = new { Result = Result, Message = Message };
            return Json(response, JsonRequestBehavior.AllowGet);

        }



        public ActionResult btnSave_Click(string UID, string id, string DIDetailID, string txtdesc, string RID, string UserRole, string tempimg, string hdndocument, string FileUpload1, string hdncapture, string nameget)
        {
            string Result = "";
            string Message = "";
            // if (hdndocument.Value == "Image")
            if (hdndocument == "Image")
            {
                // fileName = "DamageImage" + "_" + databaseCon.Decrypt(Request.QueryString["DIDetailID"]) + "_" + DateTime.Now.Ticks + "." + extension;


                DataTable dt = null;
                bool capturing = false;
                bool uploading = false;
                // string imageData = tempimg.Value;
                string imageData = tempimg;
                //string newpath2 = "";
                //string QueryU = "update tblRegisterGallery set";
                // string FileExtension = FileUpload1.FileName;
                //string FileExtension = FileUpload1;
                var FileExtension = System.IO.Path.GetExtension(FileUpload1).ToLower();
                // if (FileUpload1.HasFiles == true)
                if (FileUpload1 != "")
                {
                    if (FileExtension != "")
                    {
                        var ext = FileExtension.Split('.');
                        var extension = ext[1];
                        if (extension.ToLower() == "jpg" || extension.ToLower() == "jpeg" || extension.ToLower() == "png" || extension.ToLower() == "ico" || extension.ToLower() == "gif" || extension.ToLower() == "bmp")
                        {
                            HttpFileCollectionBase PostedFiles = Request.Files;

                            HttpFileCollectionBase files = Request.Files;
                            for (int k = 0; k < 1; k++)
                            {


                                HttpPostedFileBase postedFile = files[k];

                            //}
                            //  foreach (HttpPostedFile postedFile in PostedFiles)
                            //{
                                //string fileName = Path.GetFileName(postedFile.FileName);
                                // string fileName = "DamageImage" + "_" + databaseCon.Decrypt(Request.QueryString["DIDetailID"]) + "_" + DateTime.Now.Ticks + "." + extension;
                                string fileName = "DamageImage" + "_" + databaseCon.Decrypt(DIDetailID) + "_" + DateTime.Now.Ticks + "." + extension;
                                postedFile.SaveAs(Server.MapPath("~/images/sites") + @"/" + fileName);
                                string path = "~/images/sites/";
                                path = Server.MapPath(path);

                                string fpath = path + fileName;

                                databaseCon.savefileonblob(fileName, "img", fpath, "image/png");
                                System.IO.File.Delete(fpath);
                                string query1 = "insert into tblDeliveryDocketDocuments(DocumentName,Description,DateofUpload,Clientid,transid,Createdby,Createdon,[Type])values('" + fileName + "', '" + txtdesc.Replace("'", "''").Trim() + "', '" + DateTime.Now + "', '" + databaseCon.Decrypt(id) + "','" + databaseCon.Decrypt(DIDetailID) + "', (select Emailid from tbluser where UserId ='" + databaseCon.Decrypt(UID) + "'),'" + DateTime.Now + "','Damaged Inventory Document')";
                                int count = databaseCon.RunCmd(query1);
                                if (count > 0)
                                {
                                    capturing = true;
                                }
                                else
                                {
                                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                                    //art1.Style.Add("display", "block");
                                    //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator ";
                                    Result = "Failed";
                                    Message = "Syntax Error. Contact administrator";
                                }
                            }
                        }

                        else
                        {
                            //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                            //art1.Style.Add("display", "block");
                            //alertlabel.Text = "<strong>Alert ! </strong>Valid File Extensions are jpg, jpeg, png, gif, bmp.";
                            Result = "Failed";
                            Message = "Valid File Extensions are jpg, jpeg, png, gif, bmp.";
                        }
                    }

                }
                // if (hdncapture.Value != "" || imageData != "")
                if (hdncapture != "" || imageData != "")
                {
                    //hdncapture.Value = "";
                    //string name = nameget.Value;
                    hdncapture = "";
                    string name = nameget;
                    string path = "~/uploads/";
                    path = Server.MapPath(path);
                    string FileName = "";
                    if (name != "")
                    {
                        var extname = name.Split('.');
                        string extensionname = extname[1];
                        if (extensionname.ToLower() == "jpg" || extensionname.ToLower() == "jpeg" || extensionname.ToLower() == "png" || extensionname.ToLower() == "ico" || extensionname.ToLower() == "gif" || extensionname.ToLower() == "bmp")
                        {
                            FileName = "CaptureImg" + "_" + databaseCon.Decrypt(RID) + "_" + DateTime.Now.Ticks + ".jpg";

                        }
                        else
                        {
                            //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                            //art1.Style.Add("display", "block");
                            //alertlabel.Text = "<strong>Failed to upload File! </strong>Valid File Extensions Are .jpg, .jpeg, .png, .ico, .gif, .bmp.";
                            //txtdesc.Text = "";
                            Result = "Failed";
                            Message = ">Failed to upload File! Valid File Extensions Are .jpg, .jpeg, .png, .ico, .gif, .bmp.";
                            txtdesc = "";
                        }
                    }
                    else
                    {
                        FileName = "CaptureImg" + "_" + databaseCon.Decrypt(RID) + "_" + DateTime.Now.Ticks + ".jpg";
                    }
                    if (FileName != "")
                    {
                        string newpath = path + FileName;
                        byte[] bytes = Convert.FromBase64String(imageData.Split(',')[1]);
                        var uri = new Uri(databaseCon.siteimagelink);
                        var container = new CloudBlobContainer(uri);
                        var blob = container.GetBlockBlobReference(FileName);
                        var exists = blob.Exists();
                        if (exists == false)
                        {
                            using (var imageFile = new FileStream(newpath, FileMode.Create))
                            {
                                imageFile.Write(bytes, 0, bytes.Length);
                                imageFile.Flush();
                            }
                            if (System.IO.File.Exists(newpath))
                            {
                                string fpath = path + FileName;
                                databaseCon.savefileonblob(FileName, "img", fpath, "image/jpeg");
                                System.IO.File.Delete(fpath);
                            }
                        }
                        else
                        {
                            databaseCon.deletefileonblob(FileName, "img");
                            using (var imageFile = new FileStream(newpath, FileMode.Create))
                            {
                                imageFile.Write(bytes, 0, bytes.Length);
                                imageFile.Flush();
                            }
                            if (System.IO.File.Exists(newpath))
                            {
                                string fpath = path + FileName;
                                databaseCon.savefileonblob(FileName, "img", fpath, "image/jpeg");
                                System.IO.File.Delete(fpath);
                            }
                        }

                        string query1 = "insert into tblDeliveryDocketDocuments(DocumentName,Description,DateofUpload,Clientid,transid,Createdby,Createdon,[Type])values('" + FileName + "', '" + txtdesc.Replace("'", "''").Trim() + "', '" + DateTime.Now + "', '" + databaseCon.Decrypt(id) + "','" + databaseCon.Decrypt(DIDetailID) + "', (select Emailid from tbluser where UserId ='" + databaseCon.Decrypt(UID) + "'),'" + DateTime.Now + "','Damaged Inventory Document')";
                        int count = databaseCon.RunCmd(query1);
                        if (count > 0)
                        {
                            capturing = true;
                        }
                        else
                        {
                            //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                            //art1.Style.Add("display", "block");
                            //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator. ";
                            Result = "Failed";
                            Message = "Syntax Error. Contact administrator.";
                        }

                    }
                }
                if (capturing != false || uploading != false)
                {
                    //Session["Insert"] = "Insert";
                    //Response.Redirect("DamagedInventoryConfirmDetails.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&fromtocheck=" + Request.QueryString["fromtocheck"] + "&id=" + Request.QueryString["id"] + "&DIDetailID=" + Request.QueryString["DIDetailID"] + "&DamagedInvId=" + Request.QueryString["DamagedInvId"] + "&FromId=" + Request.QueryString["FromId"] + "&ToId=" + Request.QueryString["ToId"] + "&FromName=" + Request.QueryString["FromName"] + "&page=" + Request.QueryString["page"]);

                }
            }
            // For Document 
            // if (hdndocument.Value == "AllTypedoc")
            if (hdndocument == "AllTypedoc")
            {

                DataTable dt = null;
                bool capturing = false;
                bool uploading = false;
                // string imageData = tempimg.Value;
                string imageData = tempimg;

                //string newpath2 = "";
                //string QueryU = "update tblRegisterGallery set";
                // string FileExtension = FileUpload1.FileName;
                var FileExtension = System.IO.Path.GetExtension(FileUpload1).ToLower();
                // if (FileUpload1.HasFiles == true)
                if (FileUpload1 != "")
                {
                    if (FileExtension != "")
                    {
                        var ext = FileExtension.Split('.');
                        var extensionname = ext[1];
                        if (extensionname.ToLower() == "pdf")

                        // if (extensionname.ToLower() == "xlsx" || extensionname.ToLower() == "xls" || extensionname.ToLower() == "docx" || extensionname.ToLower() == "doc" || extensionname.ToLower() == "pdf" || extensionname.ToLower() == "jpg" || extensionname.ToLower() == "jpeg" || extensionname.ToLower() == "png" || extensionname.ToLower() == "ico" || extensionname.ToLower() == "gif" || extensionname.ToLower() == "bmp")
                        {
                            //HttpFileCollectionBase PostedFiles = Request.Files;
                            //foreach (HttpPostedFile postedFile in PostedFiles)
                            //// foreach (HttpPostedFile postedFile in FileUpload1.PostedFiles)
                            //{

                            HttpFileCollectionBase PostedFiles = Request.Files;

                            HttpFileCollectionBase files = Request.Files;
                            for (int k = 0; k < 1; k++)
                            {


                                HttpPostedFileBase postedFile = files[k];

                                //string fileName = Path.GetFileName(postedFile.FileName);
                                string fileName = "DamageAllDoc" + "_" + databaseCon.Decrypt(DIDetailID) + "_" + DateTime.Now.Ticks + ".pdf".ToLower();
                                postedFile.SaveAs(Server.MapPath("~/images/sites") + @"/" + fileName);
                                string path = "/images/sites/";
                                path = Server.MapPath(path);

                                string fpath = path + fileName;
                                if (extensionname.Replace(".", "") == "pdf".ToLower())
                                {
                                    extensionname = "application/pdf";
                                }
                                databaseCon.savefileonblob(fileName, "img", fpath, extensionname.Replace(".", ""));
                                //databaseCon.savefileonblob(fileName, "img", fpath, "image/png");
                                System.IO.File.Delete(fpath);
                                string query1 = "insert into tblDeliveryDocketDocuments(DocumentName,Description,DateofUpload,Clientid,transid,Createdby,Createdon,[Type])values('" + fileName + "', '" + txtdesc.Replace("'", "''").Trim() + "', '" + DateTime.Now + "', '" + databaseCon.Decrypt(id) + "','" + databaseCon.Decrypt(DIDetailID) + "', (select Emailid from tbluser where UserId ='" + databaseCon.Decrypt(UID) + "'),'" + DateTime.Now + "','Damaged All Type Document')";
                                int count = databaseCon.RunCmd(query1);
                                if (count > 0)
                                {
                                    capturing = true;
                                }
                                else
                                {
                                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                                    //art1.Style.Add("display", "block");
                                    //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator. ";
                                    Result = "Failed";
                                    Message = "Syntax Error. Contact administrator.";
                                }
                            }
                        }

                        else
                        {
                            //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                            //art1.Style.Add("display", "block");
                            ////  alertlabel.Text = "<strong>Alert ! </strong>Valid File Extensions are Are .xlsx, .xls, .docx, .doc, .pdf, .jpg, .jpeg, .png, .ico, .gif, .bmp";
                            //alertlabel.Text = "<strong>Alert ! </strong>Valid File Extensions is .pdf.";
                            Result = "Failed";
                            Message = "Valid File Extensions are Are .xlsx, .xls, .docx, .doc, .pdf, .jpg, .jpeg, .png, .ico, .gif, .bmp";
                            //alertlabel.Text = "<strong>Alert ! </strong>Valid File Extensions is .pdf.";
                        }
                    }

                }
                //  if (hdncapture.Value != "" || imageData != "")
                if (hdncapture != "" || imageData != "")
                {
                    //hdncapture.Value = "";
                    //string name = nameget.Value;
                    hdncapture = "";
                    string name = nameget;
                    string path = "~/uploads/";
                    path = Server.MapPath(path);
                    string FileName = "";
                    if (name != "")
                    {
                        var extname = name.Split('.');
                        string extensionname = extname[1];
                        if (extensionname.ToLower() == "pdf")
                        //  if(extensionname.ToLower() == "xlsx" || extensionname.ToLower() == "xls" || extensionname.ToLower() == "docx" || extensionname.ToLower() == "doc" || extensionname.ToLower() == "pdf" || extensionname.ToLower() == "jpg" || extensionname.ToLower() == "jpeg" || extensionname.ToLower() == "png" || extensionname.ToLower() == "ico" || extensionname.ToLower() == "gif" || extensionname.ToLower() == "bmp")
                        {

                            FileName = "CaptureImg" + "_" + databaseCon.Decrypt(DIDetailID) + "_" + DateTime.Now.Ticks + ".pdf".ToLower();

                        }
                        else
                        {
                            //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                            //art1.Style.Add("display", "block");
                            //// alertlabel.Text = "<strong>Failed to upload File! </strong>Valid File Extensions Are Are .xlsx, .xls, .docx, .doc, .pdf, .jpg, .jpeg, .png, .ico, .gif, .bmp";
                            //alertlabel.Text = "<strong>Failed to upload File! </strong>Valid File Extensions is .pdf.";
                            //txtdesc.Text = "";
                            Result = "Failed";
                            Message = "Failed to upload File! Valid File Extensions Are Are .xlsx, .xls, .docx, .doc, .pdf, .jpg, .jpeg, .png, .ico, .gif, .bmp";
                            txtdesc = "";
                        }
                    }
                    else
                    {
                        FileName = "CaptureImg" + "_" + databaseCon.Decrypt(DIDetailID) + "_" + DateTime.Now.Ticks + ".pdf".ToLower();
                    }
                    if (FileName != "")
                    {
                        string newpath = path + FileName;
                        byte[] bytes = Convert.FromBase64String(imageData.Split(',')[1]);
                        var uri = new Uri(databaseCon.siteimagelink);
                        var container = new CloudBlobContainer(uri);
                        var blob = container.GetBlockBlobReference(FileName);
                        var exists = blob.Exists();
                        if (exists == false)
                        {
                            using (var imageFile = new FileStream(newpath, FileMode.Create))
                            {
                                imageFile.Write(bytes, 0, bytes.Length);
                                imageFile.Flush();
                            }
                            if (System.IO.File.Exists(newpath))
                            {
                                string fpath = path + FileName;
                                FileExtension = FileName;
                                var ext = FileExtension.Split('.');
                                string extensionname = ext[1];
                                if (extensionname.Replace(".", "") == "pdf".ToLower())
                                {
                                    extensionname = "application/pdf";
                                }
                                databaseCon.savefileonblob(FileName, "img", fpath, extensionname.Replace(".", ""));
                                System.IO.File.Delete(fpath);
                            }
                        }
                        else
                        {
                            databaseCon.deletefileonblob(FileName, "img");
                            using (var imageFile = new FileStream(newpath, FileMode.Create))
                            {
                                imageFile.Write(bytes, 0, bytes.Length);
                                imageFile.Flush();
                            }
                            if (System.IO.File.Exists(newpath))
                            {
                                string fpath = path + FileName;
                                FileExtension = FileName;
                                var ext = FileExtension.Split('.');
                                string extensionname = ext[1];
                                if (extensionname.Replace(".", "") == "pdf".ToLower())
                                {
                                    extensionname = "application/pdf";
                                }
                                databaseCon.savefileonblob(FileName, "img", fpath, extensionname.Replace(".", ""));
                                //databaseCon.savefileonblob(FileName, "img", fpath, "image/jpeg");
                                System.IO.File.Delete(fpath);
                            }
                        }

                        string query1 = "insert into tblDeliveryDocketDocuments(DocumentName,Description,DateofUpload,Clientid,transid,Createdby,Createdon,[Type])values('" + FileName + "', '" + txtdesc.Replace("'", "''").Trim() + "', '" + DateTime.Now + "', '" + databaseCon.Decrypt(id) + "','" + databaseCon.Decrypt(DIDetailID) + "', (select Emailid from tbluser where UserId ='" + databaseCon.Decrypt(UID) + "'),'" + DateTime.Now + "','Damaged All Type Document')";
                        int count = databaseCon.RunCmd(query1);
                        if (count > 0)
                        {
                            capturing = true;
                        }
                        else
                        {
                            //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                            //art1.Style.Add("display", "block");
                            //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator. ";
                            Result = "Failed";
                            Message = "Syntax Error. Contact administrator.";
                        }

                    }
                }
                if (capturing != false || uploading != false)
                {
                    Session["Insert"] = "Insert";
                    //Response.Redirect("DamagedInventoryConfirmDetails.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&fromtocheck=" + Request.QueryString["fromtocheck"] + "&id=" + Request.QueryString["id"] + "&DIDetailID=" + Request.QueryString["DIDetailID"] + "&DamagedInvId=" + Request.QueryString["DamagedInvId"] + "&FromId=" + Request.QueryString["FromId"] + "&ToId=" + Request.QueryString["ToId"] + "&FromName=" + Request.QueryString["FromName"] + "&page=" + Request.QueryString["page"]);
                    Result = "Success";
                    Message = "Success";
                }

            }
            Result = "Success";
            Message = "Success";
            var response = new { Result = Result, Message = Message };
            return Json(response, JsonRequestBehavior.AllowGet);
        }



        public ActionResult showtImage(string DIDetailID)
        {
            string query = "select DDDocumentID,DocumentName,Description from tblDeliveryDocketDocuments where Type='Damaged Inventory Document' and TransID=" + databaseCon.Decrypt(DIDetailID);
            DataTable dt = databaseCon.GetData(query);
            string html = "<table id='image' style='width:100%'>";
            if (dt.Rows.Count > 0)
            {
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    string imgchk = dt.Rows[j]["DocumentName"].ToString();
                    if (imgchk != "" && imgchk != "NULL" && imgchk != null)
                    {
                        var uri = new Uri(databaseCon.siteimagelink);
                        var container = new CloudBlobContainer(uri);
                        var blob = container.GetBlockBlobReference(dt.Rows[j]["DocumentName"].ToString());
                        string file = databaseCon.siteimagelink + dt.Rows[j]["DocumentName"].ToString();
                        html += "<div class='col-lg-4 col-md-6 col-sm-12 col-xs-12' style='margin-top: 5px;'>";
                        html += "<img id='ppdl" + j + "' runnat='server' class='img-container' src='" + file + "' style='width: 300px;object-fit:contain; height: 250px' />";
                        html += "<div class='row' style='margin-top: -18px;text-align:left'>";
                        html += "<div class='col-lg-12 col-md-12 col-sm-6 col-xs-12'>";
                        html += "<a id='descr" + j + "' style='color:black;font-size:16px;font-weight:600;margin-left:32px'>" + dt.Rows[j]["Description"] + "</a>";
                        html += "</div>";
                        html += "</div>";
                        html += "</div>";

                    }

                }
                html += "</table>";
            }

            return Json(html, JsonRequestBehavior.AllowGet);


            //StringBuilder html = new StringBuilder();
            //html.Append("<table id='image' style='width:100%'>");
            //if (dt.Rows.Count > 0)
            //{
            //    for (int j = 0; j < dt.Rows.Count; j++)
            //    {
            //        string imgchk = dt.Rows[j]["DocumentName"].ToString();
            //        if (imgchk != "" && imgchk != "NULL" && imgchk != null)
            //        {
            //            var uri = new Uri(databaseCon.siteimagelink);
            //            var container = new CloudBlobContainer(uri);
            //            var blob = container.GetBlockBlobReference(dt.Rows[j]["DocumentName"].ToString());
            //            string file = databaseCon.siteimagelink + dt.Rows[j]["DocumentName"].ToString();
            //            html.Append("<div class='col-lg-4 col-md-6 col-sm-12 col-xs-12' style='margin-top: 5px;'>");
            //            html.Append("<img id='ppdl" + j + "' runnat='server' class='img-container' src='" + file + "' style='width: 300px;object-fit:contain; height: 250px' />");
            //            html.Append("<div class='row' style='margin-top: -18px;text-align:left'>");
            //            html.Append("<div class='col-lg-12 col-md-12 col-sm-6 col-xs-12'>");
            //            html.Append("<a id='descr" + j + "' style='color:black;font-size:16px;font-weight:600;margin-left:32px'>" + dt.Rows[j]["Description"] + "</a>");
            //            html.Append("</div>");
            //            html.Append("</div>");
            //            html.Append("</div>");
            //        }

            //    }
            //    html.Append("</table>");
            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp", "<script>margin();</script>", false);
            //}
            //placeholderforimage.Controls.Add(new Literal { Text = html.ToString() });


        }

        public ActionResult GetUplodadPdfData(string DIDetailID)
         {

            var GetData = dbContext.Database.SqlQuery<DamageInventoryConfirmDetailsViewModel>("select DDDocumentID,DocumentName,DocumentName as DocumentPdfPath,Description,DateOfUpload from tblDeliveryDocketDocuments where Type='Damaged All Type Document' and TransID=" + databaseCon.Decrypt(DIDetailID)).ToList();

            foreach (var item in GetData)
            {
                var DocumentPdfPath = item.DocumentName;
                if (DocumentPdfPath != "")
                {
                    string path = databaseCon.siteimagelink + DocumentPdfPath;
                    var uri = new Uri(databaseCon.siteimagelink);
                    var container = new CloudBlobContainer(uri);
                    var blob = container.GetBlockBlobReference(DocumentPdfPath);
                    var exists = blob.Exists();
                    if (exists == false)
                    {
                        item.DocumentPdfPath = "File does not exists.";
                    }
                    else
                    {
                        item.DocumentPdfPath = path;
                    }
                }
            }

            return Json(GetData, JsonRequestBehavior.AllowGet);

        }
    
        public ActionResult btndeleteyes_Click(string DIDDetailID, string hdnimgname)
        {
            string Result = "";
            string Message = "";
            //if (hdnimgname.Value != "")
            if (hdnimgname != "")
            {
                //string query = "delete from tblDeliveryDocketDocuments where DocumentName='" + hdnimgname.Value + "' and TransID=" + databaseCon.Decrypt(Request.QueryString["DIDetailID"]);
                string query = "delete from tblDeliveryDocketDocuments where DocumentName='" + hdnimgname + "' and TransID='"+ DIDDetailID + "'";
                int dt = databaseCon.RunCmdID(query);
                // databaseCon.deletefileonblob(hdnimgname.Value, "img");
                databaseCon.deletefileonblob(hdnimgname, "img");
                if (dt > 0)
                {
                    // hdnimgname.Value = "";
                    //hdnimgname = "";
                    //Session["Delete"] = "Delete";
                    //Response.Redirect("DamagedInventoryConfirmDetails.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&fromtocheck=" + Request.QueryString["fromtocheck"] + "&id=" + Request.QueryString["id"] + "&DIDetailID=" + Request.QueryString["DIDetailID"] + "&DamagedInvId=" + Request.QueryString["DamagedInvId"] + "&FromId=" + Request.QueryString["FromId"] + "&ToId=" + Request.QueryString["ToId"] + "&FromName=" + Request.QueryString["FromName"] + "&page=" + Request.QueryString["page"]);
                    Result = "Success";
                    Message = "";
                }else {
                    Result = "Failed";
                }
                
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult btneditSave(string id,string UID, string hdnDDDocumentID,string txtdescription,string hdnDocumentName) 
        {
            var Result = "";
            if(txtdescription!="") {
                string query = "update tblDeliveryDocketDocuments set Description = '" + txtdescription.Replace("'", "''").Trim() + "',UpdatedOn='" + DateTime.Now + "',UpdatedBy=(select Emailid from tbluser where UserId='" + databaseCon.Decrypt(UID) + "')  where  DocumentName = '" + hdnDocumentName + "' and DDDocumentId='" + hdnDDDocumentID + "'";
                int i = databaseCon.RunCmd(query);
                if (i > 0)
                {
                    Result = "Success";
                }
                else
                {
                    Result = "Failed";
                }
            }

            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        public int btnyes_Click(string id,string hdnDDDocumentID, string hdnDocumentName)
         {
            int Result = 0;
            string query = "delete from  tblDeliveryDocketDocuments where DDDocumentId ='"+ hdnDDDocumentID + "'";
            int i = databaseCon.RunCmd(query);
            if (i > 0)
            {
               databaseCon.deletefileonblob(hdnDocumentName, "siteimage");
                Result = 1;
            }
            return Result;
        }
    }
}