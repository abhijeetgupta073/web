using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;

namespace Scaffold.Controllers
{
    public class DeliveryInventoryController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: DeliveryInventory
        public ActionResult DeliveryInventory()
        {
                       return View();
        }

        public ActionResult DeliveryInventorycommentData(string ReqId)
        {
            CommonInventoryColumnViewMoidel CommonInventoryColumn = new CommonInventoryColumnViewMoidel();
            string query = "";
            DataTable dt;
            string Comment = "";
            query = "select  Comment ,Isnull(Supplier,'') as Supplier,fromperson,CONVERT(VARCHAR(11),fromtime,106) AS fromtime from tblTransaction where TransId=" + ReqId;
            dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
                {
                Comment = dt.Rows[0]["Comment"].ToString();
                }
            else
                {
                // return Json(new { result = "Failed", Message = "Please fill in Comment to Request then click on Confirm Request." });

                }
            //CommonInventoryColumn.Comment = Comment;

            return Json(Comment, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MaterialDropDown(string id)
        {
            var getMaterialType = dbContext.Database.SqlQuery<InventoryViewModel>("select distinct MaterialType from tblinventory where MaterialType <>'' and MaterialType is not null and  ClientId='" + databaseCon.Decrypt(id) + "' order by MaterialType").ToList();
            return Json(getMaterialType, JsonRequestBehavior.AllowGet);

        }

        //List<ScaffoldInventory> Deliverydata = new List<ScaffoldInventory> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod(EnableSession = true)]
       // public List<ScaffoldInventory> getDeliverydata(string check)
        public ActionResult getDeliverydata(int check,string fromcheck)
        {
            if (fromcheck == "1")
            {
                dbContext.Database.CommandTimeout = 15 * 60;

                string query = "select tblTransactionHistDelivery.TransDeliId,tblTransactionHistRequest.FromId,tblTransactionHistRequest.ToId,TransId,tblTransactionHistRequest.TransReqId,MaterialType,tblTransactionHistRequest.InventoryId,ItemCode,ItemDesc,cast(AvailableQuantity as int) as AvailableQuantity ,tblTransactionHistRequest.Quantity,tblTransactionHistDelivery.Quantity as AllocatedQuantity from tblTransactionHistRequest join VW_depotInventory on tblTransactionHistRequest.InventoryId=VW_depotInventory.InventoryId  left join tblTransactionHistDelivery on  tblTransactionHistRequest.TransReqId=tblTransactionHistDelivery.RequestId where TransId in (" + check + ") and (tblTransactionHistRequest.Quantity ! = 0 ) order by case when tblTransactionHistDelivery.Quantity is null then 1 else 0 end ,MaterialType,ISNULL(DisplaySortOrder,9999999),ItemCode";
                var getMaterialType = dbContext.Database.SqlQuery<DeliveryInventoryViewModel>(query).ToList();
                return Json(getMaterialType, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string query = "select tblTransactionHistDelivery.TransDeliId,tblTransactionHistRequest.FromId,tblTransactionHistRequest.ToId,TransId,tblTransactionHistRequest.TransReqId,MaterialType,tblTransactionHistRequest.InventoryId,ItemCode,ItemDesc,cast(AvailableQuantity as int) as AvailableQuantity ,tblTransactionHistRequest.Quantity,tblTransactionHistDelivery.Quantity as AllocatedQuantity from tblTransactionHistRequest join Vw_ThirdPartyInventory on tblTransactionHistRequest.InventoryId=Vw_ThirdPartyInventory.InventoryId  left join tblTransactionHistDelivery on  tblTransactionHistRequest.TransReqId=tblTransactionHistDelivery.RequestId where TransId in (" + check + ") and (tblTransactionHistRequest.Quantity ! = 0 ) order by case when tblTransactionHistDelivery.Quantity is null then 1 else 0 end ,MaterialType,ISNULL(DisplaySortOrder,9999999),ItemCode";
                var getMaterialType = dbContext.Database.SqlQuery<DeliveryInventoryViewModel>(query).ToList();
                return Json(getMaterialType, JsonRequestBehavior.AllowGet);

            }

            //string getquery = "select tblTransactionHistDelivery.TransDeliId,tblTransactionHistRequest.FromId,tblTransactionHistRequest.ToId,TransId,tblTransactionHistRequest.TransReqId,MaterialType,tblTransactionHistRequest.InventoryId,ItemCode,ItemDesc,cast(AvailableQuantity as int) as AvailableQuantity ,tblTransactionHistRequest.Quantity,tblTransactionHistDelivery.Quantity as AllocatedQuantity from tblTransactionHistRequest join VW_depotInventory on tblTransactionHistRequest.InventoryId=VW_depotInventory.InventoryId  left join tblTransactionHistDelivery on  tblTransactionHistRequest.TransReqId=tblTransactionHistDelivery.RequestId where TransId in (" + check + ") order by case when tblTransactionHistDelivery.Quantity is null then 1 else 0 end ,MaterialType,ItemCode";
            //DataSet ds = null;
            //ds = databaseCon.GetDataSet(getquery);
            //return Json("Res", JsonRequestBehavior.AllowGet);

            //if (ds.Tables.Count > 0)
            //{
            //    foreach (DataRow item in ds.Tables[0].Rows)
            //    {
            //        ScaffoldInventory inv = new ScaffoldInventory();
            //        if (!(item["TransId"] is DBNull) && item["TransId"].ToString() != "")
            //        {
            //            inv.TransId = Convert.ToInt32(item["TransId"].ToString());
            //        }
            //        if (!(item["TransReqId"] is DBNull) && item["TransReqId"].ToString() != "")
            //        {
            //            inv.TransReqId = Convert.ToInt32(item["TransReqId"].ToString());
            //        }
            //        if (!(item["TransDeliId"] is DBNull) && item["TransDeliId"].ToString() != "")
            //        {
            //            inv.TransDeliId = Convert.ToInt32(item["TransDeliId"].ToString());
            //        }
            //        if (!(item["InventoryId"] is DBNull) && item["InventoryId"].ToString() != "")
            //        {
            //            inv.InventoryId = Convert.ToInt32(item["InventoryId"].ToString());
            //        }
            //        if (!(item["MaterialType"] is DBNull) && item["MaterialType"].ToString() != "")
            //        {
            //            inv.MaterialType = item["MaterialType"].ToString();
            //        }
            //        if (!(item["ItemCode"] is DBNull) && item["ItemCode"].ToString() != "")
            //        {
            //            inv.ItemCode = item["ItemCode"].ToString();
            //        }
            //        if (!(item["ItemDesc"] is DBNull) && item["ItemDesc"].ToString() != "")
            //        {
            //            inv.ItemDesc = item["ItemDesc"].ToString();
            //        }
            //        if (!(item["AvailableQuantity"] is DBNull) && item["AvailableQuantity"].ToString() != "")
            //        {
            //            inv.AvailableQuantity = Convert.ToInt32(item["AvailableQuantity"].ToString());
            //        }
            //        if (!(item["Quantity"] is DBNull) && item["Quantity"].ToString() != "")
            //        {
            //            inv.ReqQty = Convert.ToInt32(item["Quantity"].ToString());
            //        }
            //        if (!(item["AllocatedQuantity"] is DBNull) && item["AllocatedQuantity"].ToString() != "")
            //        {
            //            inv.AllocatedQuantity = Convert.ToInt32(item["AllocatedQuantity"].ToString());
            //        }
            //        Deliverydata.Add(inv);
            //    }
            //}
            //return Deliverydata;
        }

        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod(EnableSession = true)]

        public int InsertDeliveryData(int AllocatedQty, int InvId, int UID, string From, string FromId, string To, string ToId, int Id, string CheckID, int TransReqId, int DeliId, int TransDeliId)
        {
            int insert = 0;
            if (DeliId == 0)
            {
                if (TransDeliId == 0)
                {
                    if (AllocatedQty > 0)
                    {
                        //string queryInsert = "insert into tblTransactionHistDelivery([From],FromId,[To],ToId,InventoryId,Quantity,TransType,RequestId,CreatedBy,CreatedOn) values('" + To + "','" + ToId + "','" + From + "','" + FromId + "','" + InvId + "','" + AllocatedQty + "','Allocate','" + TransReqId + "' ,(select EmailId from tblUser where UserId='" + UID + "') ,'" + DateTime.Now + "')";

                        string queryInsert = "insert into tblTransactionHistDelivery([From],FromId,[To],ToId,InventoryId,Quantity,TransType,RequestId,CreatedBy,CreatedOn) select '" + To + "','" + ToId + "','" + From + "','" + FromId + "','" + InvId + "','" + AllocatedQty + "','Allocate','" + TransReqId + "' ,(select EmailId from tblUser where UserId = '" + UID + "') ,'" + DateTime.Now + "' where NOT EXISTS(SELECT  1 FROM    tblTransactionHistDelivery where InventoryId = '" + InvId + "'and Quantity = '" + AllocatedQty + "'and RequestId = '" + TransReqId + "' and[To] = '" + From + "' and[From]  = '" + To + "' and FRomId = '" + ToId + "' and CreatedOn > DateAdd(minute, -1, '" + DateTime.Now + "') and CreatedOn <= '" + DateTime.Now + "' and ToId  = '" + FromId + "' and CreatedBy = (select EmailId from tblUser where UserId = '" + UID + "'))";
                        insert = databaseCon.RunCmdID(queryInsert);
                    }

                }
                else
                {
                    string queryUpdate = "Update tblTransactionHistDelivery set Quantity='" + AllocatedQty + "'where TransDeliId='" + TransDeliId + "'";
                    int updt = databaseCon.RunCmdID(queryUpdate);
                }

            }
            else
            {
                string queryUpdate = "Update tblTransactionHistDelivery set Quantity='" + AllocatedQty + "'where TransDeliId='" + DeliId + "'";
                int updt = databaseCon.RunCmdID(queryUpdate);

            }
            return insert;
        }



        public ActionResult btnconfirmDelvry_Click(string hdncheckId)
        {
            string query = "select 'x' as TotalDeliveryCount from tblTransactionHistDelivery join tblTransactionHistRequest on tblTransactionHistDelivery.RequestId=tblTransactionHistRequest.TransReqId join tblTransaction on tblTransactionHistRequest.TransId=tblTransaction.TransId where tblTransaction.TransId in(" + hdncheckId + ")";
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
               // confmodal.Style.Add("display", "block");
                return Json(new { result = "Success", Message = "Record Found." });
            }
            else
            {
                //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                //art1.Style.Add("display", "block");
                //alertlabel.Text = "<strong>Alert ! </strong>Please fill in Quantity to Deliver then click on Confirm Delivery.";
                return Json(new { result = "Failed", Message = "Please fill in Quantity to Deliver then click on Confirm Delivery." });
            }
        }

        public ActionResult btnconfirmyes_Click(string id, string UID, string DelStatus, string hdncheckId, string UserRole, string fromid, string fromtocheck)
        {
            if (databaseCon.Decrypt(DelStatus) == "NotConfirmed" || databaseCon.Decrypt(DelStatus) == "")
            {
                //string query1 = "exec Pro_DeliveryDocket '" + hdncheckId.Value + "','" + databaseCon.Decrypt(Request.QueryString["id"]) + "','" + databaseCon.Decrypt(Request.QueryString["UID"]) + "'";
                string query1 = "exec Pro_DeliveryDocket '" + databaseCon.Decrypt(hdncheckId) + "','" + databaseCon.Decrypt(id) + "','" + databaseCon.Decrypt(UID) + "'";
                DataTable dt1 = databaseCon.GetData(query1);
                if (dt1.Rows.Count > 0)
                {
                    int DocketNo = Convert.ToInt32(dt1.Rows[0][0]);

                    //Response.Redirect("DeliveryDocket.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&ToId=" + Request.QueryString["fromid"] + "&checkid=" + databaseCon.Encrypt(hdncheckId.Value) + "&DDId=" + databaseCon.Encrypt(DocketNo.ToString()));

                    //  return RedirectToAction("DeliveryDocket", "DeliveryDocket", new { id = id, UID = UID, UserRole = UserRole, ToId = fromid, checkid = databaseCon.Encrypt(hdncheckId), DDId = databaseCon.Encrypt(DocketNo.ToString()) });
                    return Json(new { result = "Success", Message = "DeliveryDocket", DocketNo = databaseCon.Encrypt(DocketNo.ToString()) });
                }
                else
                {
                    return Json(new { result = "Failed", Message = "Some thing Wants Wrongs." });
                }


            }
            else
            {
                //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                //art1.Style.Add("display", "block");
                //alertlabel.Text = "<strong>Success ! </strong>Delivery Already Confirmed.";
                return Json(new { result = "Failed", Message = "Delivery Already Confirmed." });
            }
        }
        public ActionResult GetTotalTotalTonnage(string ClientID, string ReqId)
        {
            var TotalItemsValues = "";
            var TotalTonnageValues = "";
            var From = "";
            var To = "";
            //string query = "select ISNULL(FORMAT(CONVERT(int,(Sum(Quantity))),'N0'),0) as TotalItems,convert(decimal(18,2),ISNULL(Sum(Quantity * ItemWeight) / 1000, 0)) as TotalTonnage from tblTransactionHistDelivery join Vw_ThirdPartyInventory on tblTransactionHistDelivery.InventoryID = Vw_ThirdPartyInventory.InventoryId where tblTransactionHistDelivery.ToId = " + databaseCon.Decrypt(ClientID) + " and TransId = '" + ReqId + "'";

            string query = "select ISNULL(FORMAT(CONVERT(int,(Sum(Quantity))),'N0'),0) as TotalItems,convert(decimal(18,2),ISNULL(Sum(Quantity * ItemWeight) / 1000, 0)) as TotalTonnage from tblTransactionHistDelivery join Vw_ThirdPartyInventory on tblTransactionHistDelivery.InventoryID = Vw_ThirdPartyInventory.InventoryId where tblTransactionHistDelivery.FromId = " + databaseCon.Decrypt(ClientID) + " and RequestId in (select transreqid from tblTransactionHistRequest where transid =" + ReqId + ")";
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                TotalItemsValues = dt.Rows[0]["TotalItems"].ToString();
                TotalTonnageValues = dt.Rows[0]["TotalTonnage"].ToString();
            }
            var Result = "TotalItemsValues" + TotalItemsValues + "|" + "TotalTonnageValues" + TotalTonnageValues;
            return Json(new { TotalItemsValues = TotalItemsValues, TotalTonnageValues = TotalTonnageValues });
        }


        public ActionResult GetWhereToWhereFrom(string ClientID, string ReqId, string FromId, string ToId, string from, string To, string fromtocheck )
        {
            string query = "";
            DataTable dt;
            string fromName = "";
            string ToName = "";

            query = "select  case when  tbltransaction.[From] ='job' then case when isnull(Description,'')= '' then Convert(varchar, RIGHT('000000' + CONVERT(varchar, ScaffRef), 6)) else Convert(varchar, RIGHT('000000' + CONVERT(varchar, ScaffRef), 6)) + ' - ' + Substring(Description, 1, 80) end   when  tbltransaction.[From] ='Depot' then 'Depot' when  tbltransaction.[From] ='ThirdParty' then 'ThirdParty'   when  tbltransaction.[From] ='Site' then tblSiteLocationFrom.sitelocation end as 'To',case when  tbltransaction.[To] ='job' then case when isnull(Description,'')= '' then Convert(varchar, RIGHT('000000' + CONVERT(varchar, ScaffRef), 6)) else Convert(varchar, RIGHT('000000' + CONVERT(varchar, ScaffRef), 6)) + ' - ' + Substring(Description, 1, 80) end   when  tbltransaction.[To] ='Depot' then 'Depot' when  tbltransaction.[To] ='ThirdParty' then 'ThirdParty'   when  tbltransaction.[To] ='Site' then tblSiteLocationTo.sitelocation end as 'From' from tbltransaction  left join tblregister on tblregister.registerid= tbltransaction.fromid left join tblSiteLocation tblSiteLocationTo on tblSiteLocationTo.sitelocid=tbltransaction.Toid left join tblSiteLocation tblSiteLocationFrom on tblSiteLocationFrom.sitelocid=tbltransaction.Fromid where transid = " + databaseCon.Decrypt(ReqId) + "and tblTransaction.ClientId = " + databaseCon.Decrypt(ClientID)+ "";
            dt = databaseCon.GetData(query);
            ToName = dt.Rows[0]["To"].ToString();
            fromName = dt.Rows[0]["From"].ToString();
            var Result = "ToName" + ToName + "|" + "fromName" + fromName;
            return Json(new { ToName = ToName, fromName = fromName });
        }
    }
}