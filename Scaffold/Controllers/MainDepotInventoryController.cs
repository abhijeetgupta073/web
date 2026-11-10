using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scaffold.Controllers
{
    public class MainDepotInventoryController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: MainDepotInventory
        public ActionResult MainDepotInventory(string id, string UID, string UserRole)
        {
            ViewBag.id = databaseCon.Decrypt(id);
            ViewBag.UID = databaseCon.Decrypt(UID);
            ViewBag.UserRole = databaseCon.Decrypt(UserRole);

            DataTable dtHireInventory = databaseCon.GetData("select isnull(HireInventory,0) as HireInventory from tblClient where clientid='" + (databaseCon.Decrypt(id)) + "'");

            if ((dtHireInventory.Rows[0]["HireInventory"]).ToString() != "True")
            {
                ViewBag.HireInventory = dtHireInventory.Rows[0]["HireInventory"].ToString();
            }
            return View();
        }

        public ActionResult GetDepotInventoryData(string ClientID, string MaterialType)
        {
            //var DepotInventoryData = dbContext.Database.SqlQuery<DepotInventoryViewModel>("select ClientId,MaterialType,ItemCode,ItemDesc,OrgYardQuantity,QuantityToBeSent,AvailableQuantity,ItemWeight,TotalWeight,QuantityReturned,TotalDamagedQuentity,OrgYardAvailableWithDamage from [VW_DepotInventory] where ClientID = "  + databaseCon.Decrypt(ClientID)+ " order by MaterialType ").ToList();
            //string Query1 = "select ClientId,MaterialType,ItemCode,ItemDesc,OrgYardQuantity,QuantityToBeSent,AvailableQuantity,ItemWeight,TotalWeight,QuantityReturned,TotalDamagedQuentity,OrgYardAvailableWithDamage from [VW_DepotInventory] where ClientID = " + databaseCon.Decrypt(ClientID) + " order by MaterialType ,  isnull(DisplaySortOrder,9999999) ";

            string Query1 = "select ClientId,MaterialType,ItemCode,ItemDesc,OrgYardQuantity,QuantityToBeSent,AvailableQuantity,ItemWeight,TotalWeight,QuantityReturned,TotalDamagedQuentity,OrgYardAvailableWithDamage from [VW_DepotInventory] where ClientID = " + databaseCon.Decrypt(ClientID) + " and  ('All' = '" + MaterialType.Replace("'", "''").Trim() + "' or MaterialType = '" + MaterialType.Replace("'", "''").Trim() + "')order by MaterialType ,  isnull(DisplaySortOrder,9999999) ";
            dbContext.Database.CommandTimeout = 15 * 60;
            var DepotInventoryData = dbContext.Database.SqlQuery<DepotInventoryViewModel>(Query1).ToList();
            return Json(DepotInventoryData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTotalTotalTonnage(string ClientID, string MaterialType)
        {
            var TotalItemsValues = "";
            var TotalTonnageValues = "";

            string query = "select FORMAT(CONVERT(int,(Sum(AvailableQuantity))),'N0') as TotalItems,convert(decimal(18,2),Sum(TotalWeight)/1000) as TotalTonnage from VW_DepotInventory  where ClientID = " + databaseCon.Decrypt(ClientID) + " and ('All' = '" + MaterialType.Replace("'", "''").Trim() + "' or MaterialType = '" + MaterialType.Replace("'", "''").Trim() + "')";
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                TotalItemsValues = dt.Rows[0]["TotalItems"].ToString();
                TotalTonnageValues = Convert.ToDecimal(dt.Rows[0]["TotalTonnage"]).ToString("N2");
            }

            var Result = "TotalItemsValues" + TotalItemsValues + "|" + "TotalTonnageValues" + TotalTonnageValues;
            return Json(new { TotalItemsValues = TotalItemsValues, TotalTonnageValues = TotalTonnageValues });
        }

        public class DepotInventoryViewModel
        {
            public Nullable<int> ClientId { get; set; }
            public string MaterialType { get; set; }
            public string ItemCode { get; set; }
            public string ItemDesc { get; set; }
            public Nullable<decimal> OrgYardQuantity { get; set; }
            public Nullable<int> QuantityToBeSent { get; set; }
            public Nullable<decimal> AvailableQuantity { get; set; }
            public Nullable<double> ItemWeight { get; set; }
            public Nullable<double> TotalWeight { get; set; }
            public Nullable<decimal> QuantityReturned { get; set; }
            public Nullable<decimal> QuantityReturnedToDepot { get; set; }
            public Nullable<decimal> OrgYardAvailableWithDamage { get; set; }
            public Nullable<int> OnSite { get; set; }
            public string HireMeasurement { get; set; }
            public Nullable<decimal> TotalDamagedQuentity { get; set; }
        }

        //public ActionResult GetTotalTotalTonnage(String ClientID, string siteID)
        //    {
        //    var TotalItemsValues = "";
        //    var TotalTonnageValues = "";

        //    string query = "select CONVERT(int,(Sum(OrgSiteQuantity-QuantityReturnedToDepot))) as TotalItems,convert(decimal(18,2),Sum((isnull(OrgSiteQuantity,0)-isnull(QuantityReturnedToDepot,0))*ItemWeight)/1000) as TotalTonnage from vw_siteInventory join tblInventory on  vw_SiteInventory.InventoryId=tblInventory.inventoryId where ClientID=" + databaseCon.Decrypt(ClientID) + " and (SiteId='" + siteID + "' or '0'='" + siteID + "')";
        //    DataTable dt = databaseCon.GetData(query);
        //    if (dt.Rows.Count > 0)
        //        {
        //        TotalItemsValues = dt.Rows[0]["TotalItems"].ToString();
        //        TotalTonnageValues = dt.Rows[0]["TotalTonnage"].ToString();
        //        }

        //    var Result = "TotalItemsValues" + TotalItemsValues + "|" + "TotalTonnageValues" + TotalTonnageValues;
        //    return Json(new { TotalItemsValues = TotalItemsValues, TotalTonnageValues = TotalTonnageValues });
        //    }

        //public ActionResult SiteDropDown(string id)
        //    {
        //    var getSiteLocationData = dbContext.Database.SqlQuery<SiteLocationViewModel>("select distinct SiteLocID,SiteLocation from tblSiteLocation where SiteLocation <> '' and SiteLocation is not null and ClientId = '" + databaseCon.Decrypt(id) + "' and MaintainInventory = 1 order by SiteLocation").ToList();
        //    return Json(getSiteLocationData, JsonRequestBehavior.AllowGet);

        //    }
        public ActionResult PipeInventoryHide(string ClientID)
        {
            //   string query = "Select * from tblpipelineRequest where Clientid='" + databaseCon.Decrypt(ClientID) + "'";
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
        public ActionResult DdlMaterialType(string id)
        {
            var getMaterialType = dbContext.Database.SqlQuery<InventoryViewModel>("select distinct MaterialType from tblinventory join tblTransactionHistDelivery on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId  where tblinventory.MaterialType <> '' and tblinventory.MaterialType is not null and tblTransactionHistDelivery.[To] = 'Depot' and tblTransactionHistDelivery.DeliveryStatus = 'Confirm' and tblinventory.ClientId = '" + databaseCon.Decrypt(id) + "' order by MaterialType").ToList();

            return Json(getMaterialType, JsonRequestBehavior.AllowGet);
        }
    }
}