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
    public class ReturnInventoryDetailsController : Controller
    {
        // GET: ReturnInventoryDetails
        ScaffoldEntities dbContext = new ScaffoldEntities();
        public ActionResult ReturnInventoryDetails(string id,string RInvId,string FromId)
        {
            if (databaseCon.Decrypt(Request.QueryString["RInvId"]) != "0" && Request.QueryString["RInvId"] != null)
            {
                string query2 = "select CONVERT(VARCHAR(11),getdate(),106) AS Returndate,ReturnDescription from tblReturnInventory where ReturnInvId=" + databaseCon.Decrypt(Request.QueryString["RInvId"]);
                DataTable dt2 = databaseCon.GetData(query2);
               // txtreturndate.SelectedDate = Convert.ToDateTime(dt2.Rows[0]["Returndate"]);
               // txtdesc.InnerText = dt2.Rows[0]["ReturnDescription"].ToString();
               if(dt2.Rows.Count>0) {
                    ViewBag.ReturnDate = Convert.ToDateTime(dt2.Rows[0]["Returndate"]);
                    ViewBag.ReturnDescription = dt2.Rows[0]["ReturnDescription"].ToString();
                }
              
            }


            if (databaseCon.Decrypt(Request.QueryString["fromtocheck"]) == "1")
            // On Site to depot Radio button Check
            {
                string query5 = "select distinct SiteLocID,SiteLocation from tblSiteLocation where SiteLocation <>'' and SiteLocation is not null and  ClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "'and SiteLocId=" + databaseCon.Decrypt(Request.QueryString["FromId"]);
                DataTable dt5 = databaseCon.GetData(query5);

               // txtto.Text = "Depot";
               // txtfrom.Text = dt5.Rows[0]["SiteLocation"].ToString();
               if(dt5.Rows.Count>0) {
                    ViewBag.To = "Depot";
                    ViewBag.From = dt5.Rows[0]["SiteLocation"].ToString();
                }
              
            }

          

            else  if (databaseCon.Decrypt(Request.QueryString["fromtocheck"]) == "2")
            //Job to Site Radio button Check
                {
               // chkdisplay.Style.Add("display", "block");
                string query5 = "select distinct SiteLocID,SiteLocation from tblSiteLocation where SiteLocation <>'' and SiteLocation is not null and  ClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "'and SiteLocId=" + databaseCon.Decrypt(Request.QueryString["ToId"]);
                DataTable dt5 = databaseCon.GetData(query5);
                if (dt5.Rows.Count > 0)
                {
                   // txtto.Text = dt5.Rows[0]["SiteLocation"].ToString();
                    ViewBag.To = dt5.Rows[0]["SiteLocation"].ToString();
                }
                string query7 = "select RIGHT('000000'+CONVERT(varchar,ScaffRef),6) as ScaffRef,Description,optimizestatus from tblregister where RegisterId=" + databaseCon.Decrypt(Request.QueryString["FromId"]);
                DataTable dt7 = databaseCon.GetData(query7);
                if (dt7.Rows.Count > 0)
                {
                  //  txtfrom.Text = dt7.Rows[0]["ScaffRef"].ToString();
                   
                    ViewBag.From = dt7.Rows[0]["ScaffRef"].ToString();
                    ViewBag.OptimizeStatus= dt7.Rows[0]["optimizestatus"].ToString();
                }
            }

            else 
            // On Main Depot to Third Party Radio button Check
                {
                ViewBag.To = "Third Party";
                ViewBag.From = "Depot";
                }

            string rtrnstus = "select ReturnStatus from tblReturnInventory where ReturnInvId =" + databaseCon.Decrypt(RInvId);
            DataTable dtr = databaseCon.GetData(rtrnstus);
            if (dtr.Rows.Count > 0)
            {
                if (dtr.Rows[0]["ReturnStatus"].ToString() != null && dtr.Rows[0]["ReturnStatus"].ToString() != "")
                {
                    //lockimage.Style.Add("display", "block");
                    //hdntextdisable.Value = "true";
                    //btnconfrminvtry.Style.Add("display", "none");
                    ViewBag.ReturnStatus = dtr.Rows[0]["ReturnStatus"].ToString();
                }
            }
            return View();
        }

        public ActionResult MaterialDropDown(string id)
        {
            var getMaterialType = dbContext.Database.SqlQuery<InventoryViewModel>("select distinct MaterialType from tblinventory where MaterialType <>'' and MaterialType is not null and  ClientId='" + databaseCon.Decrypt(id) + "' order by MaterialType").ToList();
            return Json(getMaterialType, JsonRequestBehavior.AllowGet);

        }

        //public List<returnq> reqgriddata(int id, int ReqInvid, string FromID, int ToId, string Page, string Material)
        public ActionResult reqgriddata(int id, int ReqInvid, string FromID, int ToId, string Page, string Material)
        {
            string addquery = "";
            string query = "";
            if (Material != "" && Material != "All")
            {
                addquery = " and MaterialType='" + Material.Replace("'", "''").Trim() + "'";

            }
            if (Page == "3")
                {
                //string query1 = "select RIDetailID,DeliveryAll.FromId ,DeliveryAll.ToId,MaterialType,ItemCode,ItemDesc, DeliveryAll.InventoryId,cast(DeliveryAll.DeliveryAvailableQuantity as int) as DeliveryAvailableQuantity, Cast(Isnull(DamagedAll.DamagedAllQuantity, 0) as int) as DamagedALLQuantity, cast(isnull(ReturnQuantity, 0) as int) as ReturnQuantity, Cast(Isnull(ReturnAll.ReturnAllQuantity, 0) as int) as ReturnALLQuantity, cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(DamagedAll.DamagedAllQuantity, 0) - Isnull(SentToSiteQuantity, 0) - isnull(ReturnAllQuantity, 0) + Isnull(ReturnFromJobQuantity, 0)) as int) as AvailableQuantity from (select FromId, ToId, MaterialType, ItemCode, ItemDesc, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'ThirdParty' and[to] = 'Depot' and TransType = 'Send' group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc) DeliveryAll left join(select FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnAllQuantity from tblReturnInventoryDetails join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Depot' and[to] = 'ThirdParty' group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.ToId = ReturnAll.FromId and DeliveryAll.InventoryId = ReturnAll.InventoryId left join(select FromId, ToId, InventoryId, Sum(DamagedQuantity) as DamagedAllQuantity from tbldamagedInventoryDetails join tbldamagedInventory on tbldamagedInventoryDetails.damagedINvId = tbldamagedInventory.damagedINvId where [FromName] = 'Depot' and[ToName] = 'ThirdParty' group by[FromName],[ToName], FromId, ToId, InventoryId)DamagedAll On DeliveryAll.FromId = DamagedAll.ToId and DeliveryAll.ToId = DamagedAll.FromId and DeliveryAll.InventoryId = DamagedAll.InventoryId Left join(select tblReturnInventoryDetails.RIDetailID, tblReturnInventoryDetails.ReturnInvId, FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Depot' and[to] = 'ThirdParty' and tblReturnInventoryDetails.ReturnInvId = '" + ReqInvid + "' group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId, tblReturnInventoryDetails.ReturnInvId, tblReturnInventoryDetails.RIDetailID)ReturnQuery On DeliveryAll.FromId = ReturnQuery.ToId and DeliveryAll.ToId = ReturnQuery.FromId  and DeliveryAll.InventoryId = ReturnQuery.InventoryId left join(select FromId, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as SentToSiteQuantity from tblTransactionHistDelivery where [From] = 'Depot' and[to] = 'Site' and TransType = 'Send' group by[From],[To], FromId, tblTransactionHistDelivery.InventoryId)SentToSite On  DeliveryAll.ToId = SentToSite.FromId  and DeliveryAll.InventoryId = SentToSite.InventoryId Left join(select ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnFromJobQuantity from tblReturnInventoryDetails join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Site' and[to] = 'Depot' group by[From],[To], ToId, tblReturnInventoryDetails.InventoryId)ReturnFromJob On  DeliveryAll.ToId = ReturnFromJob.ToId  and DeliveryAll.InventoryId = ReturnFromJob.InventoryId Left join(select ToId, tbldamagedInventoryDetails.InventoryId, Sum(DamagedQuantity) as DamagedFromJobQuantity from tbldamagedInventoryDetails join tbldamagedInventory on tbldamagedInventoryDetails.damagedINvId = tbldamagedInventory.damagedINvId where [FromName] = 'Site' and[toName] = 'Depot' group by[FromName],[ToNAME], ToId, InventoryId)DamagedFromJob On  DeliveryAll.ToId = DamagedFromJob.ToId  and DeliveryAll.InventoryId = DamagedFromJob.InventoryId where DeliveryAll.FromId = '" + id + "' and DeliveryAll.ToId = '" + FromID + "'" + addquery + "order by case when ReturnQuantity is null then 1 else 0 end, MaterialType,ItemCode,ItemDesc";
                string query1 = "select RIDetailID,DeliveryAll.FromId ,DeliveryAll.ToId,MaterialType,ItemCode,ItemDesc, DeliveryAll.InventoryId,cast(DeliveryAll.DeliveryAvailableQuantity as int) as DeliveryAvailableQuantity, Cast(Isnull(DamagedAll.DamagedAllQuantity, 0) as int) as DamagedALLQuantity, cast(isnull(ReturnQuantity, 0) as int) as ReturnQuantity, Cast(Isnull(ReturnAll.ReturnAllQuantity, 0) as int) as ReturnALLQuantity, cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(DamagedAll.DamagedAllQuantity, 0) - Isnull(SentToSiteQuantity, 0) - isnull(ReturnAllQuantity, 0) + Isnull(ReturnFromJobQuantity, 0)) as int) as AvailableQuantity from (select FromId, ToId, MaterialType, ItemCode, ItemDesc,DisplaySortOrder ,tblTransactionHistDelivery.InventoryId, Sum(Quantity) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'ThirdParty' and[to] = 'Depot' and TransType = 'Send' group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc,DisplaySortOrder) DeliveryAll left join(select FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnAllQuantity from tblReturnInventoryDetails join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Depot' and[to] = 'ThirdParty' group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.ToId = ReturnAll.FromId and DeliveryAll.InventoryId = ReturnAll.InventoryId left join(select FromId, ToId, InventoryId, Sum(DamagedQuantity) as DamagedAllQuantity from tbldamagedInventoryDetails join tbldamagedInventory on tbldamagedInventoryDetails.damagedINvId = tbldamagedInventory.damagedINvId where [FromName] = 'Depot' and[ToName] = 'ThirdParty' group by[FromName],[ToName], FromId, ToId, InventoryId)DamagedAll On DeliveryAll.FromId = DamagedAll.ToId and DeliveryAll.ToId = DamagedAll.FromId and DeliveryAll.InventoryId = DamagedAll.InventoryId Left join(select tblReturnInventoryDetails.RIDetailID, tblReturnInventoryDetails.ReturnInvId, FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Depot' and[to] = 'ThirdParty' and tblReturnInventoryDetails.ReturnInvId = '" + ReqInvid + "' group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId, tblReturnInventoryDetails.ReturnInvId, tblReturnInventoryDetails.RIDetailID)ReturnQuery On DeliveryAll.FromId = ReturnQuery.ToId and DeliveryAll.ToId = ReturnQuery.FromId  and DeliveryAll.InventoryId = ReturnQuery.InventoryId left join(select FromId, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as SentToSiteQuantity from tblTransactionHistDelivery where [From] = 'Depot' and[to] = 'Site' and TransType = 'Send' group by[From],[To], FromId, tblTransactionHistDelivery.InventoryId)SentToSite On  DeliveryAll.ToId = SentToSite.FromId  and DeliveryAll.InventoryId = SentToSite.InventoryId Left join(select ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnFromJobQuantity from tblReturnInventoryDetails join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Site' and[to] = 'Depot' group by[From],[To], ToId, tblReturnInventoryDetails.InventoryId)ReturnFromJob On  DeliveryAll.ToId = ReturnFromJob.ToId  and DeliveryAll.InventoryId = ReturnFromJob.InventoryId Left join(select ToId, tbldamagedInventoryDetails.InventoryId, Sum(DamagedQuantity) as DamagedFromJobQuantity from tbldamagedInventoryDetails join tbldamagedInventory on tbldamagedInventoryDetails.damagedINvId = tbldamagedInventory.damagedINvId where [FromName] = 'Site' and[toName] = 'Depot' group by[FromName],[ToNAME], ToId, InventoryId)DamagedFromJob On  DeliveryAll.ToId = DamagedFromJob.ToId  and DeliveryAll.InventoryId = DamagedFromJob.InventoryId where DeliveryAll.FromId = '" + id + "' and DeliveryAll.ToId = '" + FromID + "'" + addquery + "order by case when ReturnQuantity is null then 1 else 0 end, MaterialType,isnull(DisplaySortOrder,9999999),ItemCode,ItemDesc";
                var getMaterialType = dbContext.Database.SqlQuery<ReturnInventoryDetailsViewModel>(query1).ToList();

                return Json(getMaterialType, JsonRequestBehavior.AllowGet);
                
                }
            else  if (Page == "1")
            {
                string query2 = "select RIDetailID,DeliveryAll.FromId ,DeliveryAll.ToId,MaterialType,ItemCode,ItemDesc, DeliveryAll.InventoryId,cast(DeliveryAll.DeliveryAvailableQuantity as int)  as DeliveryAvailableQuantity,Cast(Isnull(DamagedAll.DamagedAllQuantity,0) as int) as DamagedALLQuantity, cast(isnull(ReturnQuantity,0) as int) as ReturnQuantity ,Cast(Isnull(ReturnAll.ReturnAllQuantity,0) as int) as ReturnALLQuantity,cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(DamagedAll.DamagedAllQuantity, 0) - Isnull(SentToJobQuantity, 0) - isnull(ReturnAllQuantity, 0) + Isnull(ReturnFromJobQuantity,0)) as int) as AvailableQuantity from (select FromId, ToId, MaterialType, ItemCode, ItemDesc, DisplaySortOrder,tblTransactionHistDelivery.InventoryId, Sum(Quantity) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Depot' and[to] = 'Site' and TransType = 'Send' group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc,DisplaySortOrder) DeliveryAll left join(select FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Site' and[to] = 'Depot' group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.ToId = ReturnAll.FromId  and DeliveryAll.InventoryId = ReturnAll.InventoryId left join (select FromId, ToId, InventoryId, Sum(DamagedQuantity) as DamagedAllQuantity from tbldamagedInventoryDetails  join tbldamagedInventory on tbldamagedInventoryDetails.damagedINvId = tbldamagedInventory.damagedINvId where [FromName] = 'Site' and[ToName] = 'Depot' group by[FromName],[ToName], FromId, ToId, InventoryId )DamagedAll On DeliveryAll.FromId = DamagedAll.ToId and DeliveryAll.ToId = DamagedAll.FromId  and DeliveryAll.InventoryId = DamagedAll.InventoryId Left join (select tblReturnInventoryDetails.RIDetailID, tblReturnInventoryDetails.ReturnInvId, FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Site' and[to] = 'Depot' and tblReturnInventoryDetails.ReturnInvId = '" + ReqInvid + "' group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId, tblReturnInventoryDetails.ReturnInvId, tblReturnInventoryDetails.RIDetailID )ReturnQuery On DeliveryAll.FromId = ReturnQuery.ToId and DeliveryAll.ToId = ReturnQuery.FromId  and DeliveryAll.InventoryId = ReturnQuery.InventoryId left join (select FromId, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as SentToJobQuantity from tblTransactionHistDelivery where [From] = 'Site' and[to] = 'Job' and TransType = 'Send' group by[From],[To], FromId, tblTransactionHistDelivery.InventoryId )SentToJob On  DeliveryAll.ToId = SentToJob.FromId  and DeliveryAll.InventoryId = SentToJob.InventoryId Left join(select ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnFromJobQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site' group by[From],[To], ToId, tblReturnInventoryDetails.InventoryId )ReturnFromJob On  DeliveryAll.ToId = ReturnFromJob.ToId  and DeliveryAll.InventoryId = ReturnFromJob.InventoryId Left join (select ToId, tbldamagedInventoryDetails.InventoryId, Sum(DamagedQuantity) as DamagedFromJobQuantity from tbldamagedInventoryDetails  join tbldamagedInventory on tbldamagedInventoryDetails.damagedINvId = tbldamagedInventory.damagedINvId where [FromName] = 'Job' and[toName] = 'Site'group by[FromName],[ToNAME], ToId, InventoryId )DamagedFromJob On  DeliveryAll.ToId = DamagedFromJob.ToId  and DeliveryAll.InventoryId = DamagedFromJob.InventoryId where DeliveryAll.FromId ='" + id + "' and DeliveryAll.ToId ='" + FromID + "'" + addquery + " order by case when ReturnQuantity is null then 1 else 0 end, MaterialType,isnull(DisplaySortOrder,9999999),ItemCode,ItemDesc";

                var getMaterialType = dbContext.Database.SqlQuery<ReturnInventoryDetailsViewModel>(query2).ToList();

                return Json(getMaterialType, JsonRequestBehavior.AllowGet);

             //   query = "select RIDetailID,DeliveryAll.FromId ,DeliveryAll.ToId,MaterialType,ItemCode,ItemDesc, DeliveryAll.InventoryId,cast(DeliveryAll.DeliveryAvailableQuantity as int)  as DeliveryAvailableQuantity,Cast(Isnull(DamagedAll.DamagedAllQuantity,0) as int) as DamagedALLQuantity, cast(isnull(ReturnQuantity,0) as int) as ReturnQuantity ,Cast(Isnull(ReturnAll.ReturnAllQuantity,0) as int) as ReturnALLQuantity,cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(DamagedAll.DamagedAllQuantity, 0) - Isnull(SentToJobQuantity, 0) - isnull(ReturnAllQuantity, 0) + Isnull(ReturnFromJobQuantity,0)) as int) as AvailableQuantity from (select FromId, ToId, MaterialType, ItemCode, ItemDesc, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Depot' and[to] = 'Site' and TransType = 'Send' group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc) DeliveryAll left join(select FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Site' and[to] = 'Depot' group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.ToId = ReturnAll.FromId  and DeliveryAll.InventoryId = ReturnAll.InventoryId left join (select FromId, ToId, InventoryId, Sum(DamagedQuantity) as DamagedAllQuantity from tbldamagedInventoryDetails  join tbldamagedInventory on tbldamagedInventoryDetails.damagedINvId = tbldamagedInventory.damagedINvId where [FromName] = 'Site' and[ToName] = 'Depot' group by[FromName],[ToName], FromId, ToId, InventoryId )DamagedAll On DeliveryAll.FromId = DamagedAll.ToId and DeliveryAll.ToId = DamagedAll.FromId  and DeliveryAll.InventoryId = DamagedAll.InventoryId Left join (select tblReturnInventoryDetails.RIDetailID, tblReturnInventoryDetails.ReturnInvId, FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Site' and[to] = 'Depot' and tblReturnInventoryDetails.ReturnInvId = '" + ReqInvid + "' group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId, tblReturnInventoryDetails.ReturnInvId, tblReturnInventoryDetails.RIDetailID )ReturnQuery On DeliveryAll.FromId = ReturnQuery.ToId and DeliveryAll.ToId = ReturnQuery.FromId  and DeliveryAll.InventoryId = ReturnQuery.InventoryId left join (select FromId, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as SentToJobQuantity from tblTransactionHistDelivery where [From] = 'Site' and[to] = 'Job' and TransType = 'Send' group by[From],[To], FromId, tblTransactionHistDelivery.InventoryId )SentToJob On  DeliveryAll.ToId = SentToJob.FromId  and DeliveryAll.InventoryId = SentToJob.InventoryId Left join(select ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnFromJobQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site' group by[From],[To], ToId, tblReturnInventoryDetails.InventoryId )ReturnFromJob On  DeliveryAll.ToId = ReturnFromJob.ToId  and DeliveryAll.InventoryId = ReturnFromJob.InventoryId Left join (select ToId, tbldamagedInventoryDetails.InventoryId, Sum(DamagedQuantity) as DamagedFromJobQuantity from tbldamagedInventoryDetails  join tbldamagedInventory on tbldamagedInventoryDetails.damagedINvId = tbldamagedInventory.damagedINvId where [FromName] = 'Job' and[toName] = 'Site'group by[FromName],[ToNAME], ToId, InventoryId )DamagedFromJob On  DeliveryAll.ToId = DamagedFromJob.ToId  and DeliveryAll.InventoryId = DamagedFromJob.InventoryId where DeliveryAll.FromId ='" + id + "' and DeliveryAll.ToId ='" + FromID + "'" + addquery + " order by ReturnQuantity desc, MaterialType,ItemCode";
            }
            else
            {
                string query3 = "select RIDetailID,DeliveryAll.FromId ,DeliveryAll.ToId,MaterialType,ItemCode,ItemDesc, DeliveryAll.InventoryId,cast(DeliveryAll.DeliveryAvailableQuantity as int)  as DeliveryAvailableQuantity,Cast(Isnull(DamagedAll.DamagedAllQuantity,0) as int) as DamagedALLQuantity, cast(isnull(ReturnQuantity,0) as int) as ReturnQuantity ,Cast(Isnull(ReturnAll.ReturnAllQuantity,0) as int) as ReturnALLQuantity,cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(DamagedAll.DamagedAllQuantity, 0) - Isnull(SentToJobQuantity, 0) + Isnull(DamagedFromJobQuantity, 0) - isnull(ReturnAllQuantity, 0)) as int) as AvailableQuantity from (select FromId, ToId, MaterialType, ItemCode, ItemDesc,DisplaySortOrder, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Site' and[to] = 'Job' and TransType = 'Send' group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc,DisplaySortOrder) DeliveryAll left join(select FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site' group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.ToId = ReturnAll.FromId  and DeliveryAll.InventoryId = ReturnAll.InventoryId left join (select FromId, ToId, InventoryId, Sum(DamagedQuantity) as DamagedAllQuantity from tbldamagedInventoryDetails  join tbldamagedInventory on tbldamagedInventoryDetails.damagedINvId = tbldamagedInventory.damagedINvId where [FromName] = 'Job' and[ToName] = 'Site' group by[FromName],[ToName], FromId, ToId, InventoryId )DamagedAll On DeliveryAll.FromId = DamagedAll.ToId and DeliveryAll.ToId = DamagedAll.FromId  and DeliveryAll.InventoryId = DamagedAll.InventoryId Left join (select tblReturnInventoryDetails.RIDetailID, tblReturnInventoryDetails.ReturnInvId, FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site' and tblReturnInventoryDetails.ReturnInvId = '" + ReqInvid + "' group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId, tblReturnInventoryDetails.ReturnInvId, tblReturnInventoryDetails.RIDetailID )ReturnQuery On DeliveryAll.FromId = ReturnQuery.ToId and DeliveryAll.ToId = ReturnQuery.FromId  and DeliveryAll.InventoryId = ReturnQuery.InventoryId left join (select FromId, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as SentToJobQuantity from tblTransactionHistDelivery where [From] = 'Site' and[to] = 'Job' and TransType = 'Send' group by[From],[To], FromId, tblTransactionHistDelivery.InventoryId )SentToJob On  DeliveryAll.ToId = SentToJob.FromId  and DeliveryAll.InventoryId = SentToJob.InventoryId Left join(select ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnFromJobQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site' group by[From],[To], ToId, tblReturnInventoryDetails.InventoryId )ReturnFromJob On  DeliveryAll.ToId = ReturnFromJob.ToId  and DeliveryAll.InventoryId = ReturnFromJob.InventoryId Left join (select ToId, tbldamagedInventoryDetails.InventoryId, Sum(DamagedQuantity) as DamagedFromJobQuantity from tbldamagedInventoryDetails  join tbldamagedInventory on tbldamagedInventoryDetails.damagedINvId = tbldamagedInventory.damagedINvId where [FromName] = 'Job' and[toName] = 'Site' group by[FromName],[ToNAME], ToId, InventoryId )DamagedFromJob On  DeliveryAll.ToId = DamagedFromJob.ToId  and DeliveryAll.InventoryId = DamagedFromJob.InventoryId where DeliveryAll.FromId= '" + ToId + "' and DeliveryAll.ToId='" + FromID + "'" + addquery + "order by case when ReturnQuantity is null then 1 else 0 end, MaterialType,isnull(DisplaySortOrder,9999999),ItemCode,ItemDesc";
                var getMaterialType = dbContext.Database.SqlQuery<ReturnInventoryDetailsViewModel>(query3).ToList();

                return Json(getMaterialType, JsonRequestBehavior.AllowGet);

              //  query = "select RIDetailID,DeliveryAll.FromId ,DeliveryAll.ToId,MaterialType,ItemCode,ItemDesc, DeliveryAll.InventoryId,cast(DeliveryAll.DeliveryAvailableQuantity as int)  as DeliveryAvailableQuantity,Cast(Isnull(DamagedAll.DamagedAllQuantity,0) as int) as DamagedALLQuantity, cast(isnull(ReturnQuantity,0) as int) as ReturnQuantity ,Cast(Isnull(ReturnAll.ReturnAllQuantity,0) as int) as ReturnALLQuantity,cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(DamagedAll.DamagedAllQuantity, 0) - Isnull(SentToJobQuantity, 0) + Isnull(DamagedFromJobQuantity, 0) - isnull(ReturnAllQuantity, 0)) as int) as AvailableQuantity from (select FromId, ToId, MaterialType, ItemCode, ItemDesc, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Site' and[to] = 'Job' and TransType = 'Send' group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc) DeliveryAll left join(select FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site' group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.ToId = ReturnAll.FromId  and DeliveryAll.InventoryId = ReturnAll.InventoryId left join (select FromId, ToId, InventoryId, Sum(DamagedQuantity) as DamagedAllQuantity from tbldamagedInventoryDetails  join tbldamagedInventory on tbldamagedInventoryDetails.damagedINvId = tbldamagedInventory.damagedINvId where [FromName] = 'Job' and[ToName] = 'Site' group by[FromName],[ToName], FromId, ToId, InventoryId )DamagedAll On DeliveryAll.FromId = DamagedAll.ToId and DeliveryAll.ToId = DamagedAll.FromId  and DeliveryAll.InventoryId = DamagedAll.InventoryId Left join (select tblReturnInventoryDetails.RIDetailID, tblReturnInventoryDetails.ReturnInvId, FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site' and tblReturnInventoryDetails.ReturnInvId = '" + ReqInvid + "' group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId, tblReturnInventoryDetails.ReturnInvId, tblReturnInventoryDetails.RIDetailID )ReturnQuery On DeliveryAll.FromId = ReturnQuery.ToId and DeliveryAll.ToId = ReturnQuery.FromId  and DeliveryAll.InventoryId = ReturnQuery.InventoryId left join (select FromId, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as SentToJobQuantity from tblTransactionHistDelivery where [From] = 'Site' and[to] = 'Job' and TransType = 'Send' group by[From],[To], FromId, tblTransactionHistDelivery.InventoryId )SentToJob On  DeliveryAll.ToId = SentToJob.FromId  and DeliveryAll.InventoryId = SentToJob.InventoryId Left join(select ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnFromJobQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site' group by[From],[To], ToId, tblReturnInventoryDetails.InventoryId )ReturnFromJob On  DeliveryAll.ToId = ReturnFromJob.ToId  and DeliveryAll.InventoryId = ReturnFromJob.InventoryId Left join (select ToId, tbldamagedInventoryDetails.InventoryId, Sum(DamagedQuantity) as DamagedFromJobQuantity from tbldamagedInventoryDetails  join tbldamagedInventory on tbldamagedInventoryDetails.damagedINvId = tbldamagedInventory.damagedINvId where [FromName] = 'Job' and[toName] = 'Site' group by[FromName],[ToNAME], ToId, InventoryId )DamagedFromJob On  DeliveryAll.ToId = DamagedFromJob.ToId  and DeliveryAll.InventoryId = DamagedFromJob.InventoryId where DeliveryAll.FromId= '" + ToId + "' and DeliveryAll.ToId='" + FromID + "'" + addquery + "order by ReturnQuantity desc, MaterialType,ItemCode";
            }


            //DataSet ds = null;
            //ds = databaseCon.GetDataSet(query);
            //if (ds.Tables.Count > 0)
            //{
            //    foreach (DataRow item in ds.Tables[0].Rows)
            //    {
            //        returnq pro = new returnq();
            //        if (!(item["RIDetailID"] is DBNull) && item["RIDetailID"].ToString() != "")
            //        {
            //            pro.RIDetailID = Convert.ToInt32(item["RIDetailID"].ToString());
            //        }
            //        if (!(item["InventoryID"] is DBNull) && item["InventoryID"].ToString() != "")
            //        {
            //            pro.InventoryID = Convert.ToInt32(item["InventoryID"].ToString());
            //        }
            //        if (!(item["MaterialType"] is DBNull) && item["MaterialType"].ToString() != "")
            //        {
            //            pro.MateralType = item["MaterialType"].ToString();
            //        }
            //        if (!(item["ItemCode"] is DBNull) && item["ItemCode"].ToString() != "")
            //        {
            //            pro.ItemCode = Convert.ToString(item["ItemCode"].ToString());
            //        }
            //        if (!(item["ItemDesc"] is DBNull) && item["ItemDesc"].ToString() != "")
            //        {
            //            pro.ItemDesc = Convert.ToString(item["ItemDesc"].ToString());
            //        }
            //        if (!(item["AvailableQuantity"] is DBNull) && item["AvailableQuantity"].ToString() != "")
            //        {
            //            pro.AvailableQuantity = Convert.ToInt32(item["AvailableQuantity"].ToString());
            //        }
            //        if (!(item["ReturnQuantity"] is DBNull) && item["ReturnQuantity"].ToString() != "")
            //        {
            //            pro.ReturnQuantity = Convert.ToInt32(item["ReturnQuantity"].ToString());
            //        }

            //        Reqgridata.Add(pro);
            //    }
            //}
            //return Reqgridata;
        }


        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod(EnableSession = true)]

        public int ReturnInsertion(int id, string from, int fromid, int TO, int ToId, int UID, string RInvId, string Page, int RowRInvId, int inv, int tempval, int Quantity, string txtdesc)

        {
            int insert = 0;
            int returnid = 0;

            if (RInvId == "0" || RInvId == null)      // The value of ReturnId is 0
            {

                if (from == "1")
                {
                    if (Quantity > 0)
                    {
                        //string query = "insert into tblreturninventory([From],fromId,[To],ToId,ReturnDoneBy,ReturnDoneOn,ClientId,ReturnDescription) values('Site','" + fromid + "','Depot','" + id + "',(select EmailId from tblUser where UserId='" + UID + "'),'" + DateTime.Now + "','" + id + "' )";

                        //Commented on (13-11-2023) We have added Createdby And CreatedOn which was not passed earlier.
                        //string query = "insert into tblreturninventory([From],fromId,[To],ToId,ReturnDoneBy,ReturnDoneOn,ClientId,ReturnDescription) values('Site','" + fromid + "','Depot','" + id + "',(select EmailId from tblUser where UserId='" + UID + "'),'" + DateTime.Now + "','" + id + "','" + txtdesc.Replace("'", "''").Trim() + "' )";

                        //Added on (13-11-2023) We have added Createdby And CreatedOn which was not passed earlier.
                        string query = "insert into tblreturninventory([From],fromId,[To],ToId,ReturnDoneBy,ReturnDoneOn,ClientId,ReturnDescription,CreatedBy,CreatedOn) values('Site','" + fromid + "','Depot','" + id + "',(select EmailId from tblUser where UserId='" + UID + "'),'" + DateTime.Now + "','" + id + "','" + txtdesc.Replace("'", "''").Trim() + "',(select EmailId from tblUser where UserId='" + UID + "'),'" + DateTime.Now + "')";

                        returnid = databaseCon.RunCmdID(query);
                    }

                }
                else if (from == "3")
                {
                    if (Quantity > 0)
                    {

                        //Commented on (13-11-2023) We have added Createdby And CreatedOn which was not passed earlier.     
                        //string query = "insert into tblreturninventory([From],fromId,[To],ToId,ReturnDoneBy,ReturnDoneOn,ClientId,ReturnDescription) values('Depot','" + fromid + "','ThirdParty','" + id + "',(select EmailId from tblUser where UserId='" + UID + "'),'" + DateTime.Now + "','" + id + "','" + txtdesc.Replace("'", "''").Trim() + "' )";

                        //Added on (13-11-2023) We have added Createdby And CreatedOn which was not passed earlier. 
                        string query = "insert into tblreturninventory([From],fromId,[To],ToId,ReturnDoneBy,ReturnDoneOn,ClientId,ReturnDescription,CreatedBy,CreatedOn) values('Depot','" + fromid + "','ThirdParty','" + id + "',(select EmailId from tblUser where UserId='" + UID + "'),'" + DateTime.Now + "','" + id + "','" + txtdesc.Replace("'", "''").Trim() + "',(select EmailId from tblUser where UserId='" + UID + "'),'" + DateTime.Now + "' )";
                        returnid = databaseCon.RunCmdID(query);

                    }

                }
                else
                {
                    if (Quantity > 0)
                    {
                        //string query = "insert into tblreturninventory([From],fromId,[To],ToId,ReturnDoneBy,ReturnDoneOn,ClientId) values('Job','" + fromid + "','Site','" + ToId + "',(select EmailId from tblUser where UserId='" + UID + "'),'" + DateTime.Now + "','" + id + "' )";

                        //Commented on (13-11-2023) We have added Createdby And CreatedOn which was not passed earlier.  
                        //string query = "insert into tblreturninventory([From],fromId,[To],ToId,ReturnDoneBy,ReturnDoneOn,ClientId,ReturnDescription) values('Job','" + fromid + "','Site','" + ToId + "',(select EmailId from tblUser where UserId='" + UID + "'),'" + DateTime.Now + "','" + id + "','" + txtdesc.Replace("'", "''").Trim() + "' )";

                        //Added on (13-11-2023) We have added Createdby And CreatedOn which was not passed earlier.  
                        string query = "insert into tblreturninventory([From],fromId,[To],ToId,ReturnDoneBy,ReturnDoneOn,ClientId,ReturnDescription,CreatedBy,CreatedOn) values('Job','" + fromid + "','Site','" + ToId + "',(select EmailId from tblUser where UserId='" + UID + "'),'" + DateTime.Now + "','" + id + "','" + txtdesc.Replace("'", "''").Trim() + "',(select EmailId from tblUser where UserId='" + UID + "'),'" + DateTime.Now + "' )";
                        returnid = databaseCon.RunCmdID(query);
                    }

                }
                if (returnid > 0)
                {
                    if (Quantity > 0)
                    {
                        string query2 = "insert into tblReturnInventoryDetails(ReturnInvId,InventoryId,Quantity,CreatedBy,CreatedOn) values('" + returnid + "','" + inv + "','" + Quantity + "',(select EmailId from tblUser where UserId='" + UID + "') ,'" + DateTime.Now + "')";
                        insert = databaseCon.RunCmdID(query2);
                    }
                }
                return returnid;
            }
            else
            {
                int insertD = 0;
                if (tempval == 0)
                {
                    if (RowRInvId == 0)
                    {
                        if (Quantity > 0)
                        {
                            string querye = "insert into tblReturnInventoryDetails(ReturnInvId,InventoryId,Quantity,CreatedBy,CreatedOn)values('" + RInvId + "','" + inv + "', '" + Quantity + "', (select EmailId from tblUser where UserId='" + UID + "') ,'" + DateTime.Now + "')";
                            insertD = databaseCon.RunCmdID(querye);
                        }

                    }
                    else
                    {
                        string queryupdate = "update tblReturnInventoryDetails set Quantity=" + Quantity + " ,UpdatedBy=(select EmailId from tblUser where UserId = '" + UID + "'),UpdatedOn = '" + DateTime.Now + "' where ReturnInvId=" + RInvId + " and InventoryId=" + inv;
                        int qupdate = databaseCon.RunCmdID(queryupdate);
                    }
                }
                else
                {
                    string queryupdate = "update tblReturnInventoryDetails set Quantity=" + Quantity + ",UpdatedBy=(select EmailId from tblUser where UserId = '" + UID + "'),UpdatedOn = '" + DateTime.Now + "' where ReturnInvId=" + RInvId + " and InventoryId=" + inv;
                    int qupdate = databaseCon.RunCmdID(queryupdate);
                }
                return insertD;
            }
        }

        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod(EnableSession = true)]

        public int ReturnAllInsertion(int fromid, int ToId, int RInvId, bool returnall, int UID, int id, string txtdesc)
        {
            int insertret = 0;

            if (returnall == true)
            {
                if (RInvId == 0)
                {
                    // string query = "insert into tblreturninventory([From],fromId,[To],ToId,ReturnDoneBy,ReturnDoneOn,ClientId) values('Job','" + fromid + "','Site','" + ToId + "',(select EmailId from tblUser where UserId='" + UID + "'),'" + DateTime.Now + "','" + id + "' )";

                    //Commented on (13-11-2023) We have added Createdby And CreatedOn which was not passed earlier.  
                    //string query = "insert into tblreturninventory([From],fromId,[To],ToId,ReturnDoneBy,ReturnDoneOn,ClientId,ReturnDescription) values('Job','" + fromid + "','Site','" + ToId + "',(select EmailId from tblUser where UserId='" + UID + "'),'" + DateTime.Now + "','" + id + "','" + txtdesc.Replace("'", "''").Trim() + "' )";

                    //Added on (13-11-2023) We have added Createdby And CreatedOn which was not passed earlier.  
                    string query = "insert into tblreturninventory([From],fromId,[To],ToId,ReturnDoneBy,ReturnDoneOn,ClientId,ReturnDescription,CreatedBy,CreatedOn) values('Job','" + fromid + "','Site','" + ToId + "',(select EmailId from tblUser where UserId='" + UID + "'),'" + DateTime.Now + "','" + id + "','" + txtdesc.Replace("'", "''").Trim() + "',(select EmailId from tblUser where UserId='" + UID + "'),'" + DateTime.Now + "' )";

                    insertret = databaseCon.RunCmdID(query);

                    if (insertret > 0)
                    {
                        string queryreturnall = "insert into tblreturninventorydetails (ReturnInvId,InventoryId, Quantity,CreatedBy, CreatedOn) select '" + insertret + "', DeliveryAll.InventoryId ,case when  '" + insertret + "' = '0' then cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(DamagedAll.DamagedAllQuantity, 0) - Isnull(SentToJobQuantity, 0) + Isnull(DamagedFromJobQuantity, 0) - isnull(ReturnAllQuantity, 0)) as int) else cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(DamagedAll.DamagedAllQuantity, 0) - Isnull(SentToJobQuantity, 0) + Isnull(DamagedFromJobQuantity, 0) - isnull(ReturnAllQuantity, 0)) as int) + isnull(ReturnQuantity,0) end as AvailableQuantity, (select EmailId from tblUser where UserId = '" + UID + "'), GETDATE()    from(select FromId, ToId, MaterialType, ItemCode, ItemDesc, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Site' and[to] = 'Job' and TransType = 'Send' group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc) DeliveryAll left join(select FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site' group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.ToId = ReturnAll.FromId  and DeliveryAll.InventoryId = ReturnAll.InventoryId left join(select FromId, ToId, InventoryId, Sum(DamagedQuantity) as DamagedAllQuantity from tbldamagedInventoryDetails  join tbldamagedInventory on tbldamagedInventoryDetails.damagedINvId = tbldamagedInventory.damagedINvId where [FromName] = 'Job' and[ToName] = 'Site' group by[FromName],[ToName], FromId, ToId, InventoryId)DamagedAll On DeliveryAll.FromId = DamagedAll.ToId and DeliveryAll.ToId = DamagedAll.FromId  and DeliveryAll.InventoryId = DamagedAll.InventoryId Left join(select tblReturnInventoryDetails.RIDetailID, tblReturnInventoryDetails.ReturnInvId, FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site' and tblReturnInventoryDetails.ReturnInvId = '" + insertret + "' group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId, tblReturnInventoryDetails.ReturnInvId, tblReturnInventoryDetails.RIDetailID)ReturnQuery On DeliveryAll.FromId = ReturnQuery.ToId and DeliveryAll.ToId = ReturnQuery.FromId  and DeliveryAll.InventoryId = ReturnQuery.InventoryId left join(select FromId, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as SentToJobQuantity from tblTransactionHistDelivery where [From] = 'Site' and[to] = 'Job' and TransType = 'Send' group by[From],[To], FromId, tblTransactionHistDelivery.InventoryId)SentToJob On  DeliveryAll.ToId = SentToJob.FromId  and DeliveryAll.InventoryId = SentToJob.InventoryId Left join(select ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnFromJobQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and [to] = 'Site' group by[From],[To], ToId, tblReturnInventoryDetails.InventoryId)ReturnFromJob On  DeliveryAll.ToId = ReturnFromJob.ToId  and DeliveryAll.InventoryId = ReturnFromJob.InventoryId Left join(select ToId, tbldamagedInventoryDetails.InventoryId, Sum(DamagedQuantity) as DamagedFromJobQuantity from tbldamagedInventoryDetails  join tbldamagedInventory on tbldamagedInventoryDetails.damagedINvId = tbldamagedInventory.damagedINvId where [FromName] = 'Job' and[toName] = 'Site' group by[FromName],[ToNAME], ToId, InventoryId)DamagedFromJob On  DeliveryAll.ToId = DamagedFromJob.ToId  and DeliveryAll.InventoryId = DamagedFromJob.InventoryId where DeliveryAll.FromId = '" + ToId + "' and DeliveryAll.ToId = '" + fromid + "' order by  MaterialType, ItemCode";
                        int insertreturn = databaseCon.RunCmd(queryreturnall);
                    }
                }
                else
                {
                    string queryreturnall = "Merge tblReturnInventoryDetails using (select '" + RInvId + "' as REturnInvId,DeliveryAll.InventoryId,cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(DamagedAll.DamagedAllQuantity, 0) - Isnull(SentToJobQuantity, 0) + Isnull(DamagedFromJobQuantity, 0) - isnull(ReturnAllQuantity, 0)) as int) as AvailableQuantity,(Select EmailID From tblUser where UserId='" + UID + "') as CreatedBy, GETDATE() as CreatedOn   from (select FromId, ToId, MaterialType, ItemCode, ItemDesc, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Site' and[to] = 'Job' and TransType = 'Send' group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc) DeliveryAll left join(select FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site' group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.ToId = ReturnAll.FromId  and DeliveryAll.InventoryId = ReturnAll.InventoryId left join (select FromId, ToId, InventoryId, Sum(DamagedQuantity) as DamagedAllQuantity from tbldamagedInventoryDetails  join tbldamagedInventory on tbldamagedInventoryDetails.damagedINvId = tbldamagedInventory.damagedINvId where [FromName] = 'Job' and[ToName] = 'Site' group by[FromName],[ToName], FromId, ToId, InventoryId )DamagedAll On DeliveryAll.FromId = DamagedAll.ToId and DeliveryAll.ToId = DamagedAll.FromId  and DeliveryAll.InventoryId = DamagedAll.InventoryId Left join (select tblReturnInventoryDetails.RIDetailID, tblReturnInventoryDetails.ReturnInvId, FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site' and tblReturnInventoryDetails.ReturnInvId = '" + RInvId + "' group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId, tblReturnInventoryDetails.ReturnInvId, tblReturnInventoryDetails.RIDetailID )ReturnQuery On DeliveryAll.FromId = ReturnQuery.ToId and DeliveryAll.ToId = ReturnQuery.FromId  and DeliveryAll.InventoryId = ReturnQuery.InventoryId left join (select FromId, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as SentToJobQuantity from tblTransactionHistDelivery where [From] = 'Site' and[to] = 'Job' and TransType = 'Send' group by[From],[To], FromId, tblTransactionHistDelivery.InventoryId )SentToJob On  DeliveryAll.ToId = SentToJob.FromId  and DeliveryAll.InventoryId = SentToJob.InventoryId Left join(select ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnFromJobQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site' group by[From],[To], ToId, tblReturnInventoryDetails.InventoryId )ReturnFromJob On  DeliveryAll.ToId = ReturnFromJob.ToId  and DeliveryAll.InventoryId = ReturnFromJob.InventoryId Left join (select ToId, tbldamagedInventoryDetails.InventoryId, Sum(DamagedQuantity) as DamagedFromJobQuantity from tbldamagedInventoryDetails  join tbldamagedInventory on tbldamagedInventoryDetails.damagedINvId = tbldamagedInventory.damagedINvId where [FromName] = 'Job' and[toName] = 'Site' group by[FromName],[ToNAME], ToId, InventoryId )DamagedFromJob On  DeliveryAll.ToId = DamagedFromJob.ToId  and DeliveryAll.InventoryId = DamagedFromJob.InventoryId where DeliveryAll.FromId='" + ToId + "' and DeliveryAll.ToId='" + fromid + "' )AllDEtails on AllDetails.REturnInvID = tblReturnInventoryDetails.REturnInvId and AllDetails.InventoryId = tblReturnInventoryDetails.InventoryId When Matched Then UPDATE SET Quantity = AvailableQuantity + Quantity WHEN NOT MATCHED BY TARGET THEN INSERT(ReturnInvID, InventoryID, Quantity) VALUES(ReturnInvId, InventoryId, AvailableQuantity); ";
                    int insertreturn = databaseCon.RunCmd(queryreturnall);
                }
            }
            return insertret;
        }

        public int btnconfrminvtry_Click(string id,string hdnRInvId,string UID) {

            var result = 0;
            string Query = "update tblReturnInventory set ReturnStatus ='Confirm' ,UpdatedBy=(select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'),UpdatedOn = '" + DateTime.Now + "', ReturnDoneOn = '" + DateTime.Now + "' where ReturnInvId =" + hdnRInvId;
            int i = databaseCon.RunCmd(Query);
            if (i > 0)
            {
                // lockimage.Style.Add("display", "block");
                // hdntextdisable.Value = "true";
                // btnconfrminvtry.Style.Add("display", "none");
                //art1.Attributes["class"] = "j-close w3-section w3-card-12 fade in j-alertBox-Success";
                //art1.Style.Add("display", "block");
                //alertlabel.Text = "<strong>Success ! </strong>Return confirmation saved successfully.";
                result =1;
            }
            return result;
        }


        public ActionResult GetTotalTotalTonnage(string RetId)
        {
            var TotalItemsValues = "";
            var TotalTonnageValues = "";
            string query = "select ISNULL(FORMAT(CONVERT(int, (Sum(Quantity))),'N0'),0)  as TotalItems,convert(decimal(18, 2), ISNULL(Sum(Quantity * ItemWeight) / 1000, 0)) as TotalTonnage from tblReturnInventoryDetails join Vw_ThirdPartyInventory on tblReturnInventoryDetails.InventoryID = Vw_ThirdPartyInventory.InventoryId where ReturnInvId = '" + RetId + "'";
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                TotalItemsValues = dt.Rows[0]["TotalItems"].ToString();
                TotalTonnageValues = dt.Rows[0]["TotalTonnage"].ToString();
            }
            var Result = "TotalItemsValues" + TotalItemsValues + "|" + "TotalTonnageValues" + TotalTonnageValues;
            return Json(new { TotalItemsValues = TotalItemsValues, TotalTonnageValues = TotalTonnageValues });
        }

        public void updateDesc(string RInvId, string Desccription)
        {
            if (RInvId != "")
            {
                string updescqry = "update tblReturnInventory set ReturnDescription='" + Desccription.Replace("'", "''").Trim() + "' where ReturnInvId='" + RInvId + "'";
                int i = databaseCon.RunCmd(updescqry);
            }
            else
            {

            }
        }
    }
}


public class DocketCommonViewModel
{
    public Nullable<int> InventoryId { get; set; }
    public Nullable<int> ReturnQuantity { get; set; }
}