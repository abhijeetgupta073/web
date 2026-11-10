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
    public class SiteInventoryController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: SiteInventory
        public ActionResult SiteInventory(string id, string UID, string UserRole)
        {
            ViewBag.id = databaseCon.Decrypt(id);
            ViewBag.UID = databaseCon.Decrypt(UID);
            ViewBag.UserRole = databaseCon.Decrypt(UserRole);
            // Add Code On 27/March/2020.
            DataTable dtHireInventory = databaseCon.GetData("select isnull(HireInventory,0) as HireInventory from tblClient where clientid='" + (databaseCon.Decrypt(id)) + "'");

            if ((dtHireInventory.Rows[0]["HireInventory"]).ToString() != "True")
            {
                ViewBag.HireInventory = dtHireInventory.Rows[0]["HireInventory"].ToString();
                //divWeeklyReport.Visible = false;
                //divInvoiceCost.Visible = false;
                //divSAP.Visible = false; // Add code On 02/April/2020 
            }
            return View();
        }
        public ActionResult GetSiteInventoryData(string ClientID, string siteID, string MaterialType)
        {
            dbContext.Database.CommandTimeout = 15 * 60;
            string query = "exec Pro_SiteInventory_Get " + databaseCon.Decrypt(ClientID) + "," + siteID + ",'" + MaterialType.Replace("'", "''").Trim() + "'";
            var SiteInventoryData = dbContext.Database.SqlQuery<Pro_SiteInventoryGetViewModel>(query).ToList();
            return Json(SiteInventoryData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTotalTotalTonnage(String ClientID, string siteID, string MaterialType)
        {
            var TotalItemsValues = "";
            var TotalTonnageValues = "";
            string query = "select CONVERT(int,(Sum(OrgSiteQuantity-QuantityReturnedToDepot))) as TotalItems,convert(decimal(18,2),Sum((isnull(OrgSiteQuantity,0)-isnull(QuantityReturnedToDepot,0))*ItemWeight)/1000) as TotalTonnage from vw_siteInventory join tblInventory on  vw_SiteInventory.InventoryId=tblInventory.inventoryId where ClientID=" + databaseCon.Decrypt(ClientID) + " and (SiteId='" + siteID + "' or '0'='" + siteID + "') and ('All' = '" + MaterialType.Replace("'", "''").Trim() + "' or MaterialType = '" + MaterialType.Replace("'", "''").Trim() + "')";
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                TotalItemsValues = dt.Rows[0]["TotalItems"].ToString();
                TotalTonnageValues = dt.Rows[0]["TotalTonnage"].ToString();
            }
            var Result = "TotalItemsValues" + TotalItemsValues + "|" + "TotalTonnageValues" + TotalTonnageValues;
            return Json(new { TotalItemsValues = TotalItemsValues, TotalTonnageValues = TotalTonnageValues });
        }

        //add by s.sourabh 19 oct 2022
        public ActionResult DdlMaterialType(string id, string siteID)
        {
            if (siteID == "0" || siteID == "")
            {
                siteID = "All";
            }
            var getMaterialType = dbContext.Database.SqlQuery<InventoryViewModel>("select distinct MaterialType from tblinventory join tblTransactionHistDelivery on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId  where tblinventory.MaterialType <> '' and tblinventory.MaterialType is not null and tblTransactionHistDelivery.[To] = 'site' and tblTransactionHistDelivery.DeliveryStatus = 'Confirm' and ( 'All' = '" + siteID + "' or tblTransactionHistDelivery.ToId = '" + siteID + "') and tblinventory.ClientId = '" + databaseCon.Decrypt(id) + "' order by MaterialType").ToList();
            //var getMaterialType = dbContext.Database.SqlQuery<InventoryViewModel>("select distinct MaterialType from tblInventory  where clientid = " + databaseCon.Decrypt(id) + " order by MaterialType").ToList();
            return Json(getMaterialType, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GenerateLastGenerated(string UID, string UserRole, string ClientId, string SiteId, string rdpReportStartDate, string rdpReportEndDate)
        {
            var GeneratedValue = "";
            int id = Convert.ToInt32(databaseCon.Decrypt(UID));
            int SiteID = Convert.ToInt32(SiteId);
            int ClientID = Convert.ToInt32(databaseCon.Decrypt(ClientId));
            //string StartDate = databaseCon.Decrypt(rdpReportStartDate);
            //string EndDate = databaseCon.Decrypt(rdpReportEndDate);
            string StartDate = rdpReportStartDate;
            string EndDate = rdpReportEndDate;

            var Result = "";

            string URole = databaseCon.Decrypt(UserRole);
            if (SiteId != "")
            {
                //  if ((rdpReportStartDate.SelectedDate != null) && (rdpReportEndDate.SelectedDate != null))
                if ((StartDate != null) && (EndDate != null))
                {

                    //string query = "select isnull(cast(format(max(ReportCreatedOn),'dd-MMM-yyyy') as nvarchar(20)),'Not Yet Generated') as maxcreatedon from [GenerateInvoiceCostReport] where SiteLocId='" + SiteId + "'and  [ReportStartDate] ='" + StartDate + "'and [ReportEndDate]= '" + EndDate + "' ";
                    string query = "select isnull(cast(format(max(ReportCreatedOn),'dd-MMM-yyyy') as nvarchar(20)),'Not Yet Generated') as maxcreatedon from [TblGenerateInvoiceCostReport] where SiteLocId='" + SiteId + "'and  [ActualStartDate] ='" + StartDate + "'and [ActualEndDate]= '" + EndDate + "' ";
                    DataTable dt = databaseCon.GetData(query);

                    if (dt.Rows.Count > 0)
                    {
                        GeneratedValue = dt.Rows[0]["maxcreatedon"].ToString();

                    }
                }
            }
             Result = "GeneratedValue" + GeneratedValue;
            return Json(new { GeneratedValue = GeneratedValue });
        }
        public ActionResult SiteDropDown(string id, string UID)
        {
            var SiteOwnerName = "";
            string query1 = "select SiteOwnerName from tblUser where ClientId='" + databaseCon.Decrypt(id) + "' and UserId='" + databaseCon.Decrypt(UID) + "'";
            DataTable dt1 = databaseCon.GetData(query1);
            if (dt1.Rows.Count > 0)
            {
                SiteOwnerName = dt1.Rows[0]["SiteOwnerName"].ToString();
            }
            if (SiteOwnerName != "")
            {
                // Escape single quote by replacing ' with ''
                SiteOwnerName = SiteOwnerName.Replace("'", "''");
                var getSiteLocationData = dbContext.Database.SqlQuery<SiteLocationViewModel>("select distinct SiteLocation,SiteLocID from tblSiteLocation where SiteLocation <>'' and SiteLocation is not null and  ClientId='" + databaseCon.Decrypt(id) + "' and OwnerName='" + SiteOwnerName + "' and MaintainInventory = 1 and MaintainInventoryAtJob=1  order by SiteLocation").ToList();
                return Json(getSiteLocationData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // var getSiteLocationData = dbContext.Database.SqlQuery<SiteLocationViewModel>("select distinct SiteLocID,SiteLocation from tblSiteLocation where SiteLocation <> '' and SiteLocation is not null and ClientId = '" + databaseCon.Decrypt(id) + "' and MaintainInventory = 1 order by SiteLocation").ToList();
                var getSiteLocationData = dbContext.Database.SqlQuery<SiteLocationViewModel>("select * from (select 0 as SiteLocID,'All' as SiteLocation union select distinct SiteLocID,SiteLocation from tblSiteLocation where SiteLocation <>'' and SiteLocation is not null and  ClientId = '" + databaseCon.Decrypt(id) + "' and MaintainInventory = 1 ) as sitelisting order by SiteLocation").ToList();

                return Json(getSiteLocationData, JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult PipeInventoryHide(string ClientID)
        {
            // string query = "Select * from tblpipelineRequest where Clientid='" + databaseCon.Decrypt(ClientID) + "'";
            string query = "select DISTINCT  PipeLineInvHide from tblSiteLocation where Clientid='" + databaseCon.Decrypt(ClientID) + "' and PipeLineInvHide is not null and PipeLineInvHide != 0";
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                return Json(new { result = "success", Message = "Available Quantity" });
            }
            else
            {
                return Json(new { result = "Failed", Message = "Not Quantity" });
            }
        }

    }
}