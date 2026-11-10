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
    public class DamagedInventoryDetailsController : Controller
    {
        // GET: DamagedInventoryDetails
        ScaffoldEntities dbContext = new ScaffoldEntities();
        public ActionResult DamagedInventoryDetails()
        {
            if (databaseCon.Decrypt(Request.QueryString["fromtocheck"]) == "1")
            // On Site to depot Radio button Check
            {
                string query = "select distinct SiteLocID,SiteLocation from tblSiteLocation where SiteLocation <>'' and SiteLocation is not null and  ClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "'and SiteLocId=" + databaseCon.Decrypt(Request.QueryString["FromId"]);
                DataTable dt = databaseCon.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    ViewBag.To = "Depot";
                    ViewBag.From = dt.Rows[0]["SiteLocation"].ToString();
                }

            }

            else if (databaseCon.Decrypt(Request.QueryString["fromtocheck"]) == "2")
            //Job to Site Radio button Check
            {
                string query5 = "select distinct SiteLocID,SiteLocation from tblSiteLocation where SiteLocation <>'' and SiteLocation is not null and  ClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "'and SiteLocId=" + databaseCon.Decrypt(Request.QueryString["ToId"]);
                DataTable dt5 = databaseCon.GetData(query5);
                if (dt5.Rows.Count > 0)
                {
                    ViewBag.To = dt5.Rows[0]["SiteLocation"].ToString();

                }
                string query7 = "select RIGHT('000000'+CONVERT(varchar,ScaffRef),6) as ScaffRef,Description from tblregister where RegisterId=" + databaseCon.Decrypt(Request.QueryString["FromId"]);
                DataTable dt7 = databaseCon.GetData(query7);
                if (dt7.Rows.Count > 0)
                {
                    ViewBag.From = dt7.Rows[0]["ScaffRef"].ToString();
                }
            }
            else if (databaseCon.Decrypt(Request.QueryString["fromtocheck"]) == "3")
            {
                ViewBag.To = "ThirdParty";
                // ViewBag.To = "Depot";
                ViewBag.From = "Depot";
            }

            if (databaseCon.Decrypt(Request.QueryString["DamagedInvId"]) != "0" && Request.QueryString["DamagedInvId"] != null)
            {
                string querydata = "select DamagedDescription from tblDamagedInventory where DamagedInvId=" + databaseCon.Decrypt(Request.QueryString["DamagedInvId"]);
                DataTable dt1 = databaseCon.GetData(querydata);
                if (dt1.Rows.Count > 0)
                {
                    ViewBag.DamagedDescription = dt1.Rows[0]["DamagedDescription"].ToString();
                }

            }
            return View();
        }

        public ActionResult MaterialDropDown(string id)
        {
            var getMaterialType = dbContext.Database.SqlQuery<InventoryViewModel>("select distinct MaterialType from tblinventory where MaterialType <>'' and MaterialType is not null and  ClientId='" + databaseCon.Decrypt(id) + "' order by MaterialType").ToList();
            return Json(getMaterialType, JsonRequestBehavior.AllowGet);

        }

        public ActionResult getDamageData(string id, string toid, string fromid, int rInvId, int dInvId, int fromtocheck, string Material)
        {
            string query = "";
            string addquery = "";
            if (Material != "" && Material != "All")
            {
                addquery = " and MaterialType='" + Material + "'";
            }
            // On Site to depot Radio button Check
            if (fromtocheck == 1)
            {
                //string query1 = "select RIDetailID, DIDetailID, DeliveryAll.FromId ,DeliveryAll.ToId,MaterialType,ItemCode,ItemDesc, DeliveryAll.InventoryId,Reason,cast(DeliveryAll.DeliveryAvailableQuantity as int) as DeliveryAvailableQuantity,Cast(Isnull(DamagedAll.DamagedAllQuantity, 0) as int) as DamagedALLQuantity, cast(isnull(ReturnQuantity, 0) as int) as ReturnQuantity , cast(isnull(DamagedQuantity, 0) as int) as DamagedQuantity ,Cast(Isnull(ReturnAll.ReturnAllQuantity, 0) as int) as ReturnALLQuantity,cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(DamagedAll.DamagedAllQuantity, 0) - Isnull(SentToJobQuantity, 0) - isnull(ReturnAllQuantity, 0) + Isnull(ReturnFromJobQuantity,0)) as int) as AvailableQuantity from(select FromId, ToId, MaterialType, ItemCode, ItemDesc, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Depot' and[to] = 'Site' and TransType = 'Send' group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc) DeliveryAll left join (select FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Site' and[to] = 'Depot' group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.ToId = ReturnAll.FromId  and DeliveryAll.InventoryId = ReturnAll.InventoryId left join (select FromId, ToId, InventoryId, Sum(DamagedQuantity) as DamagedAllQuantity from tbldamagedInventoryDetails  join tbldamagedInventory on tbldamagedInventoryDetails.DamagedINvId = tbldamagedInventory.DamagedINvId where [FromName] = 'Site' and[ToName] = 'Depot' group by[FromName],[ToName], FromId, ToId, InventoryId )DamagedAll On DeliveryAll.FromId = DamagedAll.ToId and DeliveryAll.ToId = DamagedAll.FromId  and DeliveryAll.InventoryId = DamagedAll.InventoryId left join (select tblDamagedInventoryDetails.DIDetailID, tblDamagedInventoryDetails.DamagedInvId, FromId, ToId,Reason, tblDamagedInventoryDetails.InventoryId, Sum(DamagedQuantity) as DamagedQuantity from tblDamagedInventoryDetails  join tblDamagedInventory on tblDamagedInventoryDetails.DamagedInvId = tblDamagedInventory.DamagedInvId where [FromName] = 'Site' and[toName] = 'Depot' and tblDamagedInventoryDetails.DamagedInvId ='" + dInvId + "'  group by Reason,[FromName],[ToName], FromId, ToId, tblDamagedInventoryDetails.InventoryId, tblDamagedInventoryDetails.DamagedInvId, tblDamagedInventoryDetails.DIDetailID )DamagedQuery On DeliveryAll.FromId = DamagedQuery.ToId and DeliveryAll.ToId = DamagedQuery.FromId  and DeliveryAll.InventoryId = DamagedQuery.InventoryId Left join (select tblReturnInventoryDetails.RIDetailID, tblReturnInventoryDetails.ReturnInvId, FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Site' and[to] = 'Depot' and tblReturnInventoryDetails.ReturnInvId = '" + rInvId + "' group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId, tblReturnInventoryDetails.ReturnInvId, tblReturnInventoryDetails.RIDetailID )ReturnQuery On DeliveryAll.FromId = ReturnQuery.ToId and DeliveryAll.ToId = ReturnQuery.FromId  and DeliveryAll.InventoryId = ReturnQuery.InventoryId left join (select FromId, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as SentToJobQuantity from tblTransactionHistDelivery where [From] = 'Site' and[to] = 'Job' and TransType = 'Send' group by[From],[To], FromId, tblTransactionHistDelivery.InventoryId )SentToJob On  DeliveryAll.ToId = SentToJob.FromId  and DeliveryAll.InventoryId = SentToJob.InventoryId Left join (select ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnFromJobQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site' group by[From],[To], ToId, tblReturnInventoryDetails.InventoryId )ReturnFromJob On  DeliveryAll.ToId = ReturnFromJob.ToId  and DeliveryAll.InventoryId = ReturnFromJob.InventoryId Left join (select ToId, tbldamagedInventoryDetails.InventoryId, Sum(DamagedQuantity) as DamagedFromJobQuantity from tbldamagedInventoryDetails  join tbldamagedInventory on tbldamagedInventoryDetails.DamagedINvId = tbldamagedInventory.DamagedINvId where [FromName] = 'Job' and[toName] = 'Site'group by[FromName],[ToNAME], ToId, InventoryId )DamagedFromJob On  DeliveryAll.ToId = DamagedFromJob.ToId  and DeliveryAll.InventoryId = DamagedFromJob.InventoryId where DeliveryAll.FromId = '" + id + "' and DeliveryAll.ToId = '" + fromid + "' " + addquery + " order by DamagedQuantity desc, MaterialType,ItemCode";
                string query1 = "select RIDetailID, DIDetailID, DeliveryAll.FromId ,DeliveryAll.ToId,MaterialType,ItemCode,ItemDesc, DeliveryAll.InventoryId,Reason,cast(DeliveryAll.DeliveryAvailableQuantity as int) as DeliveryAvailableQuantity,Cast(Isnull(DamagedAll.DamagedAllQuantity, 0) as int) as DamagedALLQuantity, cast(isnull(ReturnQuantity, 0) as int) as ReturnQuantity , cast(isnull(DamagedQuantity, 0) as int) as DamagedQuantity ,Cast(Isnull(ReturnAll.ReturnAllQuantity, 0) as int) as ReturnALLQuantity,cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(DamagedAll.DamagedAllQuantity, 0) - Isnull(SentToJobQuantity, 0) - isnull(ReturnAllQuantity, 0) + Isnull(ReturnFromJobQuantity,0)) as int) as AvailableQuantity from(select FromId, ToId, MaterialType, ItemCode, ItemDesc, DisplaySortOrder,tblTransactionHistDelivery.InventoryId, Sum(Quantity) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Depot' and[to] = 'Site' and TransType = 'Send' group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc,DisplaySortOrder) DeliveryAll left join (select FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Site' and[to] = 'Depot' group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.ToId = ReturnAll.FromId  and DeliveryAll.InventoryId = ReturnAll.InventoryId left join (select FromId, ToId, InventoryId, Sum(DamagedQuantity) as DamagedAllQuantity from tbldamagedInventoryDetails  join tbldamagedInventory on tbldamagedInventoryDetails.DamagedINvId = tbldamagedInventory.DamagedINvId where [FromName] = 'Site' and[ToName] = 'Depot' group by[FromName],[ToName], FromId, ToId, InventoryId )DamagedAll On DeliveryAll.FromId = DamagedAll.ToId and DeliveryAll.ToId = DamagedAll.FromId  and DeliveryAll.InventoryId = DamagedAll.InventoryId left join (select tblDamagedInventoryDetails.DIDetailID, tblDamagedInventoryDetails.DamagedInvId, FromId, ToId,Reason, tblDamagedInventoryDetails.InventoryId, Sum(DamagedQuantity) as DamagedQuantity from tblDamagedInventoryDetails  join tblDamagedInventory on tblDamagedInventoryDetails.DamagedInvId = tblDamagedInventory.DamagedInvId where [FromName] = 'Site' and[toName] = 'Depot' and tblDamagedInventoryDetails.DamagedInvId ='" + dInvId + "'  group by Reason,[FromName],[ToName], FromId, ToId, tblDamagedInventoryDetails.InventoryId, tblDamagedInventoryDetails.DamagedInvId, tblDamagedInventoryDetails.DIDetailID )DamagedQuery On DeliveryAll.FromId = DamagedQuery.ToId and DeliveryAll.ToId = DamagedQuery.FromId  and DeliveryAll.InventoryId = DamagedQuery.InventoryId Left join (select tblReturnInventoryDetails.RIDetailID, tblReturnInventoryDetails.ReturnInvId, FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Site' and[to] = 'Depot' and tblReturnInventoryDetails.ReturnInvId = '" + rInvId + "' group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId, tblReturnInventoryDetails.ReturnInvId, tblReturnInventoryDetails.RIDetailID )ReturnQuery On DeliveryAll.FromId = ReturnQuery.ToId and DeliveryAll.ToId = ReturnQuery.FromId  and DeliveryAll.InventoryId = ReturnQuery.InventoryId left join (select FromId, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as SentToJobQuantity from tblTransactionHistDelivery where [From] = 'Site' and[to] = 'Job' and TransType = 'Send' group by[From],[To], FromId, tblTransactionHistDelivery.InventoryId )SentToJob On  DeliveryAll.ToId = SentToJob.FromId  and DeliveryAll.InventoryId = SentToJob.InventoryId Left join (select ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnFromJobQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site' group by[From],[To], ToId, tblReturnInventoryDetails.InventoryId )ReturnFromJob On  DeliveryAll.ToId = ReturnFromJob.ToId  and DeliveryAll.InventoryId = ReturnFromJob.InventoryId Left join (select ToId, tbldamagedInventoryDetails.InventoryId, Sum(DamagedQuantity) as DamagedFromJobQuantity from tbldamagedInventoryDetails  join tbldamagedInventory on tbldamagedInventoryDetails.DamagedINvId = tbldamagedInventory.DamagedINvId where [FromName] = 'Job' and[toName] = 'Site'group by[FromName],[ToNAME], ToId, InventoryId )DamagedFromJob On  DeliveryAll.ToId = DamagedFromJob.ToId  and DeliveryAll.InventoryId = DamagedFromJob.InventoryId where DeliveryAll.FromId = '" + id + "' and DeliveryAll.ToId = '" + fromid + "' " + addquery + " order by DamagedQuantity desc, MaterialType,isnull(DisplaySortOrder,9999999),ItemCode";
                var getData = dbContext.Database.SqlQuery<DamagedInventoryDetailsViewModel>(query1).ToList();

                return Json(getData, JsonRequestBehavior.AllowGet);


            }
            //Job to Site Radio button Check
            else if (fromtocheck == 2)
            {
                string query2 = "select DIDetailID,DeliveryAll.FromId ,DeliveryAll.ToId,MaterialType,ItemCode,ItemDesc,Reason, DeliveryAll.InventoryId,cast(DeliveryAll.DeliveryAvailableQuantity as int) as DeliveryAvailableQuantity,Cast(Isnull(DamagedAll.DamagedAllQuantity, 0) as int) as DamagedALLQuantity, cast(isnull(ReturnQuantity, 0) as int) as ReturnQuantity , cast(isnull(DamagedQuantity, 0) as int) as DamagedQuantity ,Cast(Isnull(ReturnAll.ReturnAllQuantity, 0) as int) as ReturnALLQuantity,cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(DamagedAll.DamagedAllQuantity, 0) - Isnull(SentToJobQuantity, 0) + Isnull(DamagedFromJobQuantity, 0) - isnull(ReturnAllQuantity, 0)) as int) as AvailableQuantity from(select FromId, ToId, MaterialType, ItemCode, ItemDesc,DisplaySortOrder, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Site' and[to] = 'Job' and TransType = 'Send' group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc,DisplaySortOrder) DeliveryAll left join(select FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site' group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.ToId = ReturnAll.FromId  and DeliveryAll.InventoryId = ReturnAll.InventoryId left join (select FromId, ToId, InventoryId, Sum(DamagedQuantity) as DamagedAllQuantity from tbldamagedInventoryDetails  join tbldamagedInventory on tbldamagedInventoryDetails.damagedINvId = tbldamagedInventory.damagedINvId where [FromName] = 'Job' and[ToName] = 'Site' group by[FromName],[ToName], FromId, ToId, InventoryId )DamagedAll On DeliveryAll.FromId = DamagedAll.ToId and DeliveryAll.ToId = DamagedAll.FromId  and DeliveryAll.InventoryId = DamagedAll.InventoryId left join (select tblDamagedInventoryDetails.DIDetailID, tblDamagedInventoryDetails.DamagedInvId, FromId, ToId, Reason, tblDamagedInventoryDetails.InventoryId, Sum(DamagedQuantity) as DamagedQuantity from tblDamagedInventoryDetails  join tblDamagedInventory on tblDamagedInventoryDetails.DamagedInvId = tblDamagedInventory.DamagedInvId where [FromName] = 'Job' and[toName] = 'Site' and tblDamagedInventoryDetails.DamagedInvId =" + dInvId + "  group by Reason,[FromName],[ToName], FromId, ToId, tblDamagedInventoryDetails.InventoryId, tblDamagedInventoryDetails.DamagedInvId, tblDamagedInventoryDetails.DIDetailID )DamagedQuery On DeliveryAll.FromId = DamagedQuery.ToId and DeliveryAll.ToId = DamagedQuery.FromId  and DeliveryAll.InventoryId = DamagedQuery.InventoryId Left join (select tblReturnInventoryDetails.RIDetailID, tblReturnInventoryDetails.ReturnInvId, FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Site' and[to] = 'Job' and tblReturnInventoryDetails.ReturnInvId =" + rInvId + " group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId, tblReturnInventoryDetails.ReturnInvId, tblReturnInventoryDetails.RIDetailID)ReturnQuery On DeliveryAll.FromId = ReturnQuery.ToId and DeliveryAll.ToId = ReturnQuery.FromId  and DeliveryAll.InventoryId = ReturnQuery.InventoryId left join (select FromId, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as SentToJobQuantity from tblTransactionHistDelivery where [From] = 'Site' and[to] = 'Job' and TransType = 'Send' group by[From],[To], FromId, tblTransactionHistDelivery.InventoryId )SentToJob On  DeliveryAll.ToId = SentToJob.FromId  and DeliveryAll.InventoryId = SentToJob.InventoryId Left join(select ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnFromJobQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site' group by[From],[To], ToId, tblReturnInventoryDetails.InventoryId )ReturnFromJob On  DeliveryAll.ToId = ReturnFromJob.ToId  and DeliveryAll.InventoryId = ReturnFromJob.InventoryId Left join (select ToId, tbldamagedInventoryDetails.InventoryId, Sum(DamagedQuantity) as DamagedFromJobQuantity from tbldamagedInventoryDetails  join tbldamagedInventory on tbldamagedInventoryDetails.damagedINvId = tbldamagedInventory.damagedINvId where [FromName] = 'Job' and[toName] = 'Site' group by [FromName],[ToNAME], ToId, InventoryId )DamagedFromJob On  DeliveryAll.ToId = DamagedFromJob.ToId  and DeliveryAll.InventoryId = DamagedFromJob.InventoryId where DeliveryAll.FromId ='" + toid + "' and DeliveryAll.ToId='" + fromid + "' " + addquery + "order by DamagedQuantity desc, MaterialType,isnull(DisplaySortOrder,9999999),ItemCode";
                var getData = dbContext.Database.SqlQuery<DamagedInventoryDetailsViewModel>(query2).ToList();
                return Json(getData, JsonRequestBehavior.AllowGet);

            }
            //Depot to ThirdParty
            else if (fromtocheck == 3)
            {
                //var getData = dbContext.Database.SqlQuery<DamagedInventoryDetailsViewModel>("select DIDetailID,VW_DepotInventory.InventoryId,MaterialType,ItemCode,ItemDesc,Cast(isnull(AvailableQuantity,0) as int) as AvailableQuantity ,Cast(isnull(DamagedQuantity,0) as int) as DamagedQuantity ,Reason from VW_DepotInventory left join(select  Reason,DIDetailID,clientId, tblDamagedInventoryDetails.InventoryId, Sum(isnull(DamagedQuantity, 0)) as DamagedQuantity from tblDamagedInventoryDetails join tblDamagedInventory on tblDamagedInventoryDetails.DamagedInvId = tblDamagedInventory.DamagedInvId where [FromName] = 'Depot' and[toName] = 'Depot' and clientId = " + id + " and tblDamagedInventoryDetails.DamagedInvId=" + dInvId + " group by[FromName],[ToName], tblDamagedInventoryDetails.InventoryId, clientId,DIDetailID,Reason) DamageQuery on VW_DepotInventory.InventoryID = DamageQuery.InventoryID and VW_DepotInventory.ClientID = DamageQuery.clientId   where VW_DepotInventory.ClientID = " + id + " " + addquery + " order by DamagedQuantity desc, MaterialType,ItemCode").ToList();
                //string query3 = "select DIDetailID,VW_DepotInventory.InventoryId,MaterialType,ItemCode,ItemDesc,Cast(isnull(AvailableQuantity,0) as int) as AvailableQuantity ,Cast(isnull(DamagedQuantity,0) as int) as DamagedQuantity ,Reason from VW_DepotInventory left join(select  Reason,DIDetailID,clientId, tblDamagedInventoryDetails.InventoryId, Sum(isnull(DamagedQuantity, 0)) as DamagedQuantity from tblDamagedInventoryDetails join tblDamagedInventory on tblDamagedInventoryDetails.DamagedInvId = tblDamagedInventory.DamagedInvId where [FromName] = 'Depot' and[toName] = 'ThirdParty' and clientId = " + id + " and tblDamagedInventoryDetails.DamagedInvId=" + dInvId + " group by[FromName],[ToName], tblDamagedInventoryDetails.InventoryId, clientId,DIDetailID,Reason) DamageQuery on VW_DepotInventory.InventoryID = DamageQuery.InventoryID and VW_DepotInventory.ClientID = DamageQuery.clientId   where VW_DepotInventory.ClientID = " + id + " " + addquery + " order by DamagedQuantity desc, MaterialType,ItemCode";
                string query3 = "select DIDetailID,VW_DepotInventory.InventoryId,MaterialType,ItemCode,ItemDesc,Cast(isnull(AvailableQuantity,0) as int) as AvailableQuantity ,Cast(isnull(DamagedQuantity,0) as int) as DamagedQuantity ,Reason from VW_DepotInventory left join(select  Reason,DIDetailID,clientId, tblDamagedInventoryDetails.InventoryId, Sum(isnull(DamagedQuantity, 0)) as DamagedQuantity from tblDamagedInventoryDetails join tblDamagedInventory on tblDamagedInventoryDetails.DamagedInvId = tblDamagedInventory.DamagedInvId where [FromName] = 'Depot' and[toName] = 'ThirdParty' and clientId = " + id + " and tblDamagedInventoryDetails.DamagedInvId=" + dInvId + " group by[FromName],[ToName], tblDamagedInventoryDetails.InventoryId, clientId,DIDetailID,Reason) DamageQuery on VW_DepotInventory.InventoryID = DamageQuery.InventoryID and VW_DepotInventory.ClientID = DamageQuery.clientId   where VW_DepotInventory.ClientID = " + id + " " + addquery + " order by DamagedQuantity desc, MaterialType,isnull(DisplaySortOrder,9999999),ItemCode";

                var getData = dbContext.Database.SqlQuery<DamagedInventoryDetailsViewModel>(query3).ToList();

                return Json(getData, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(new { result = "Failed", Message = "Record Not Found." });
            }

        }


        public int insertDamage(int dmgQty, int InvID, string id, int UID, string toid, string fromid, int fromtocheck, int DamagedInvId, int DIDetailID, string reason, int avlQty, string txtdesc)
        {
            int result = 0;
            string query = "";
            int result1 = 0;
            if (DamagedInvId == 0)
            {
                //Insert perform in tblDamagedInventory from Site to Job
                if (fromtocheck == 1)
                {
                    query = "insert into tblDamagedInventory([FromName],fromId,[ToName],ToId,CreatedBy,CreatedOn,ClientId,DamagedDescription) values('Site','" + fromid + "','Depot','" + id + "',(select EmailId from tblUser where UserId='" + UID + "'),'" + DateTime.Now + "','" + id + "','" + txtdesc.Replace("'", "''").Trim() + "')";
                }
                else if (fromtocheck == 2)
                //Insert per+form in tblDamagedInventory from Job to Site
                {
                    query = "insert into tblDamagedInventory([FromName],fromId,[ToName],ToId,CreatedBy,CreatedOn,ClientId,DamagedDescription) values('Job','" + fromid + "','Site','" + toid + "',(select EmailId from tblUser where UserId='" + UID + "'),'" + DateTime.Now + "','" + id + "','" + txtdesc.Replace("'", "''").Trim() + "')";
                }
                //Insert perform in tblDamagedInventory from Depot to ThirdParty
                else if (fromtocheck == 3)
                {
                    //query = "insert into tblDamagedInventory([FromName],fromId,[ToName],ToId,CreatedBy,CreatedOn,ClientId,DamagedDescription) values('Depot','" + fromid + "','Depot','" + toid + "',(select EmailId from tblUser where UserId='" + UID + "'),'" + DateTime.Now + "','" + id + "','" + txtdesc.Replace("'", "''").Trim() + "')";
                    query = "insert into tblDamagedInventory([FromName],fromId,[ToName],ToId,CreatedBy,CreatedOn,ClientId,DamagedDescription) values('Depot','" + fromid + "','ThirdParty','" + toid + "',(select EmailId from tblUser where UserId='" + UID + "'),'" + DateTime.Now + "','" + id + "','" + txtdesc.Replace("'", "''").Trim() + "')";
                }
                int result2 = databaseCon.RunCmdID(query);
                if ((avlQty + dmgQty) >= dmgQty)
                {
                    if (result2 > 0)
                    {
                        if (dmgQty > 0)
                        {
                            query = "insert into tblDamagedInventoryDetails(DamagedInvId,InventoryId,DamagedQuantity,CreatedBy,CreatedOn,[Date],Reason) values('" + result2 + "','" + InvID + "','" + dmgQty + "',(select EmailId from tblUser where UserId='" + UID + "') ,'" + DateTime.Now + "','" + DateTime.Now + "','" + reason.Replace("'", "''").Trim() + "')";
                            result1 = databaseCon.RunCmdID(query);
                        }
                    }
                }
                return result2;
            }
            else
            {
                if (avlQty + dmgQty >= dmgQty)
                {
                    if (DIDetailID > 0)
                    {
                        query = "update tblDamagedInventoryDetails set DamagedQuantity=" + dmgQty + ", Reason='" + reason.Replace("'", "''").Trim() + "' ,UpdatedBy=(select EmailId from tblUser where UserId = '" + UID + "'),UpdatedOn = '" + DateTime.Now + "' where DamagedInvId=" + DamagedInvId + " and InventoryId=" + InvID;
                        result = databaseCon.RunCmdID(query);
                    }
                    else
                    {
                        if (dmgQty > 0)
                        {
                            query = "insert into tblDamagedInventoryDetails(DamagedInvId,InventoryId,DamagedQuantity,CreatedBy,CreatedOn,[Date],Reason) values('" + DamagedInvId + "','" + InvID + "','" + dmgQty + "',(select EmailId from tblUser where UserId='" + UID + "') ,'" + DateTime.Now + "','" + DateTime.Now + "','" + reason.Replace("'", "''").Trim() + "')";
                            result1 = databaseCon.RunCmdID(query);
                        }

                    }
                }
                return result1;
            }
        }

        public ActionResult GetTotalTonnage(string ClientID, string dmgInvId)
        {
            var TotalItemsValues = "";
            var TotalTonnageValues = "";
            string query = "select ISNULL(FORMAT(CONVERT(int,(Sum(DamagedQuantity))),'N0'),0) as TotalItems,convert(decimal(18,2),ISNULL(Sum(DamagedQuantity * ItemWeight) / 1000, 0)) as TotalTonnage from tblDamagedInventoryDetails join Vw_ThirdPartyInventory on tblDamagedInventoryDetails.InventoryID = Vw_ThirdPartyInventory.InventoryId join tblDamagedInventory on tblDamagedInventoryDetails.DamagedInvId = tblDamagedInventory.DamagedInvId   where tblDamagedInventory.Clientid = " + databaseCon.Decrypt(ClientID) + " and tblDamagedInventoryDetails.DamagedInvId = '" + dmgInvId + "'";
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                TotalItemsValues = dt.Rows[0]["TotalItems"].ToString();
                TotalTonnageValues = dt.Rows[0]["TotalTonnage"].ToString();
            }
            var Result = "TotalItemsValues" + TotalItemsValues + "|" + "TotalTonnageValues" + TotalTonnageValues;
            return Json(new { TotalItemsValues = TotalItemsValues, TotalTonnageValues = TotalTonnageValues });
        }




        public void btnrefresh_Click(string ClientID, string dmgInvId, string txtdesc)
        {
            string updescqry = "update tblDamagedInventory Set DamagedDescription = '" + txtdesc + "' where DamagedInvId = " + dmgInvId + "";
            int i = databaseCon.RunCmd(updescqry);
        }
    }
}