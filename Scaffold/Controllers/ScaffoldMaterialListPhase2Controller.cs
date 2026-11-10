using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Markup;
using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;

namespace Scaffold.Controllers
{
    public class ScaffoldMaterialListPhase2Controller : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        string ddlMaterialType = "";
        // GET: ScaffoldMaterialListPhase2
        public ActionResult ScaffoldMaterialListPhase2(String id, string RID,string ddlMaterialType)
        {
            string addquery = "";

            int ClientID = Convert.ToInt32(databaseCon.Decrypt(id));
            int RId = Convert.ToInt32(databaseCon.Decrypt(RID));
            string query;
            DataTable dt;
            // query = "exec Pro_TBLREGISTER_GET '" + databaseCon.Decrypt(Request.QueryString["RID"]) + "','" + databaseCon.Decrypt(Request.QueryString["id"]) + "','',''";
            query = "select SiteLocId from tblRegister where ClientID='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and RegisterID='" + databaseCon.Decrypt(Request.QueryString["RID"]) + "'";
            dt = databaseCon.GetData(query);

            //var Site1 = "";
            var Site1 = "0";

            if (dt.Rows.Count != 0)
            {
                string Siteid = Convert.ToString(dt.Rows[0]["SiteLocID"]);
                ViewBag.SiteID = databaseCon.Encrypt(Siteid);

                Site1 = Siteid;
            }
            query = "exec Pro_UpdateRegisterForSiteMaterial '" + databaseCon.Decrypt(Request.QueryString["RID"]) + "'";
            //dt = databaseCon.GetData(query);
            var result = dbContext.Database.ExecuteSqlCommand(query);

            DataTable dtHireInventory = databaseCon.GetData("select isnull(HireInventory,0) as HireInventory from tblClient where clientid='" + (databaseCon.Decrypt(Request.QueryString["id"])) + "'");
            if (dtHireInventory.Rows.Count > 0)
            {
                ViewBag.HireInventory = dtHireInventory.Rows[0]["HireInventory"];
            }
            if (ddlMaterialType == "" || ddlMaterialType == null)
            {
                ddlMaterialType = "All";

            }

            //  query = "Select convert(decimal(18, 2),SUM(TotalDailyHire))TotalDailyHireValue from ( select DeliveryAll.FromId ,DeliveryAll.ToId,MaterialType,ItemCode,ItemDesc , DeliveryAll.InventoryId,cast(DeliveryAll.DeliveryAvailableQuantity as int) as DeliveryAvailableQuantity,isnull(cast(ReturnAll.ReturnALLQuantity as int), 0) as ReturnALLQuantity,isnull(cast(DamagedAll.DamagedAllQuantity as int), 0) as DamagedALLQuantity,cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0) - ISNULL(DamagedAll.DamagedAllQuantity, 0)) as int)as AvailableQuantity,DeliveryAll.ItemWeight,cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0) - ISNULL(DamagedAll.DamagedAllQuantity, 0)) as int) * ItemWeight as TotalWeight,DeliveryAll.SubCategoryID  ,HireMeasurement, CASE WHEN HireMeasurement = 'Tonnage' THEN convert(decimal(18, 4),((ItemWeight) / 1000) * (isnull(HireRate, 0) / 7.0)) WHEN HireMeasurement = 'Each' THEN convert(decimal(18, 4),(isnull(HireRate, 0) / 7.0)) END as DailyHireRate,CASE WHEN HireMeasurement = 'Tonnage' THEN convert(decimal(18, 4),(((cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0) - ISNULL(DamagedAll.DamagedAllQuantity, 0)) as int)) * ItemWeight) / 1000) * (isnull(HireRate, 0) / 7.0)) WHEN HireMeasurement = 'Each' THEN convert(decimal(18, 4),(cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0) - ISNULL(DamagedAll.DamagedAllQuantity, 0)) as int)) * (isnull(HireRate, 0) / 7.0))END as TotalDailyHire from(select FromId, ToId, MaterialType, ItemCode, ItemDesc, ItemWeight, tblTransactionHistDelivery.InventoryId, tblInventory.SubCategoryID, isnull(Sum(Quantity), 0) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Site' and[to] = 'Job' and TransType = 'Send' and ToId = '" + databaseCon.Decrypt(Request.QueryString["RID"]) + "' group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc, ItemWeight, SubCategoryID)DeliveryAll left join(select FromId, ToId, tblReturnInventoryDetails.InventoryId, isnull(Sum(Quantity), 0) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site' and FromId = '" + databaseCon.Decrypt(Request.QueryString["RID"]) + "' group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.InventoryId = ReturnAll.InventoryId left join(select FromId, ToId, tblDamagedInventoryDetails.InventoryId, ISNULL(Sum(DamagedQuantity), 0) as DamagedAllQuantity from tblDamagedInventoryDetails  join tblDamagedInventory on tblDamagedInventoryDetails.DamagedInvId = tblDamagedInventory.DamagedInvId where [FromName] = 'Job' and[toName] = 'Site' and FromId = '" + databaseCon.Decrypt(Request.QueryString["RID"]) + "' group by[FromName],[ToName], FromId, ToId, tblDamagedInventoryDetails.InventoryId) DamagedAll On DeliveryAll.FromId = DamagedAll.ToId and DeliveryAll.InventoryId = DamagedAll.InventoryId left join tblSubCategory on tblSubCategory.SubCategoryID = DeliveryAll.SubCategoryID where 1 = 1 )AllJobsHire ";

            var EndDate = DateTime.Now.Date.ToString("MM-dd-yyyy");
            //commented by pooja 14dec2022
            //query = "exec Proc_InvoiceCostReportSurgedate '" + EndDate + "', '" + EndDate + "', " + ClientID + "," + Site1 + ",'dJHC'," + RId + ",'"+ ddlMaterialType + "'";
            query = "exec Proc_InvoiceCostReportSurgedate_GenerateReport '" + EndDate + "', '" + EndDate + "', " + ClientID + "," + Site1 + ",'dJHC'," + RId + ",'" + ddlMaterialType + "','" + EndDate + "', '" + EndDate + "'";


            dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                var TotalDailyHireValue = Convert.ToString(dt.Rows[0]["TotalDailyHireValue"]);
                if (TotalDailyHireValue == "")
                {
                    // txtTotalDailyHireCost.Text = "";
                    ViewBag.TotalDailyHireCost = "";
                }
                else
                {
                    ViewBag.TotalDailyHireCost = ("$" + TotalDailyHireValue);
                    //txtTotalDailyHireCost.Text = ("$" + TotalDailyHireValue);
                }
            }


            return View();
        }
        public ActionResult MaterialDropDown(string id)
        {
            var getMaterialType = dbContext.Database.SqlQuery<InventoryViewModel>("select distinct MaterialType from tblInventory  where clientid = " + databaseCon.Decrypt(id) + " order by MaterialType").ToList();
            return Json(getMaterialType, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetInventoryData(string id, string RID, string ddlMaterialType, string Siteid)
        {


            string addquery = "";
            string query;
            DataTable dt;

            int ClientID = Convert.ToInt32(databaseCon.Decrypt(id));
            int RId = Convert.ToInt32(databaseCon.Decrypt(RID));
            int SiteID = Convert.ToInt32(databaseCon.Decrypt(Siteid));

            var EndDate = DateTime.Now.Date.ToString("MM-dd-yyyy");
            //if (ddlMaterialType != "" && ddlMaterialType != "All")
            //{
            //    addquery = " and MaterialType='" + ddlMaterialType + "' ";

            //}
            // Query Change on 04 May 2020
            // query = "select DeliveryAll.FromId ,DeliveryAll.ToId,MaterialType,ItemCode,ItemDesc , DeliveryAll.InventoryId,cast(DeliveryAll.DeliveryAvailableQuantity as int) as DeliveryAvailableQuantity,isnull(cast(ReturnAll.ReturnALLQuantity as int), 0) as ReturnALLQuantity,isnull(cast(DamagedAll.DamagedAllQuantity as int), 0) as DamagedALLQuantity,cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0) - ISNULL(DamagedAll.DamagedAllQuantity, 0)) as int) as AvailableQuantity,DeliveryAll.ItemWeight,cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0) - ISNULL(DamagedAll.DamagedAllQuantity, 0)) as int) * ItemWeight as TotalWeight,DeliveryAll.SubCategoryID  ,HireMeasurement, CASE WHEN HireMeasurement = 'Tonnage' THEN convert(decimal(18, 4),((ItemWeight) / 1000) * (isnull(HireRate, 0) / 7.0)) WHEN HireMeasurement = 'Each' THEN convert(decimal(18, 4),(isnull(HireRate, 0) / 7.0)) END as DailyHireRate,CASE WHEN HireMeasurement = 'Tonnage' THEN convert(decimal(18, 4),(((cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0) - ISNULL(DamagedAll.DamagedAllQuantity, 0)) as int)) * ItemWeight) / 1000) * (isnull(HireRate, 0) / 7.0)) WHEN HireMeasurement = 'Each' THEN convert(decimal(18, 4),(cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0) - ISNULL(DamagedAll.DamagedAllQuantity, 0)) as int)) * (isnull(HireRate, 0) / 7.0))END as TotalDailyHire from(select FromId, ToId, MaterialType, ItemCode, ItemDesc, ItemWeight, tblTransactionHistDelivery.InventoryId, tblInventory.SubCategoryID, isnull(Sum(Quantity), 0) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Site' and[to] = 'Job' and TransType = 'Send' and ToId = '" + databaseCon.Decrypt(RID) + "' group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc, ItemWeight, SubCategoryID)DeliveryAll left join(select FromId, ToId, tblReturnInventoryDetails.InventoryId, isnull(Sum(Quantity), 0) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site' and FromId = '" + databaseCon.Decrypt(RID) + "' group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.InventoryId = ReturnAll.InventoryId left join(select FromId, ToId, tblDamagedInventoryDetails.InventoryId, ISNULL(Sum(DamagedQuantity), 0) as DamagedAllQuantity from tblDamagedInventoryDetails  join tblDamagedInventory on tblDamagedInventoryDetails.DamagedInvId = tblDamagedInventory.DamagedInvId where [FromName] = 'Job' and[toName] = 'Site' and FromId = '" + databaseCon.Decrypt(RID) + "' group by[FromName],[ToName], FromId, ToId, tblDamagedInventoryDetails.InventoryId) DamagedAll On DeliveryAll.FromId = DamagedAll.ToId and DeliveryAll.InventoryId = DamagedAll.InventoryId left join tblSubCategory on tblSubCategory.SubCategoryID = DeliveryAll.SubCategoryID where 1 = 1 " + addquery + " order by MaterialType,ItemCode";

            //commented by pooja 14dec2022
            //query = "exec Proc_InvoiceCostReportSurgedate '" + EndDate + "', '" + EndDate + "', " + ClientID + "," + SiteID + ",'JHC'," + RId + ",'" + ddlMaterialType + "'  ";

            query = "exec Proc_InvoiceCostReportSurgedate_GenerateReport '" + EndDate + "', '" + EndDate + "', " + ClientID + "," + SiteID + ",'JHC'," + RId + ",'" + ddlMaterialType.Replace("'", "''").Trim() + "','" + EndDate + "', '" + EndDate + "'";




            //Changes made on query on 16 March 2020
            //  query = "select DeliveryAll.FromId ,DeliveryAll.ToId,MaterialType,ItemCode,ItemDesc , DeliveryAll.InventoryId,cast(DeliveryAll.DeliveryAvailableQuantity as int) as DeliveryAvailableQuantity,isnull(cast(ReturnAll.ReturnALLQuantity as int), 0) as ReturnALLQuantity,isnull(cast(DamagedAll.DamagedAllQuantity as int), 0) as DamagedALLQuantity,cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0) - ISNULL(DamagedAll.DamagedAllQuantity, 0)) as int) as AvailableQuantity,DeliveryAll.ItemWeight,cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0) - ISNULL(DamagedAll.DamagedAllQuantity, 0)) as int) * ItemWeight as TotalWeight,DeliveryAll.SubCategoryID  ,HireMeasurement, CASE WHEN HireMeasurement = 'Tonnage' THEN convert(decimal(18, 4),((ItemWeight) / 1000) * (isnull(HireRate, 0) / 7.0)) WHEN HireMeasurement = 'Each' THEN convert(decimal(18, 4),(isnull(HireRate, 0) / 7.0)) END as DailyHireRate,CASE WHEN HireMeasurement = 'Tonnage' THEN convert(decimal(18, 4),(((cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0) - ISNULL(DamagedAll.DamagedAllQuantity, 0)) as int)) * ItemWeight) / 1000) * (isnull(HireRate, 0) / 7.0)) WHEN HireMeasurement = 'Each' THEN convert(decimal(18, 4),(cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0) - ISNULL(DamagedAll.DamagedAllQuantity, 0)) as int)) * (isnull(HireRate, 0) / 7.0))END as TotalDailyHire from(select FromId, ToId, MaterialType, ItemCode, ItemDesc, ItemWeight, tblTransactionHistDelivery.InventoryId, tblInventory.SubCategoryID, isnull(Sum(Quantity), 0) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Site' and[to] = 'Job' and TransType = 'Send' and ToId = '" + databaseCon.Decrypt(Request.QueryString["RID"]) + "' group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc, ItemWeight, SubCategoryID)DeliveryAll left join(select FromId, ToId, tblReturnInventoryDetails.InventoryId, isnull(Sum(Quantity), 0) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site' and FromId = '" + databaseCon.Decrypt(Request.QueryString["RID"]) + "' group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.InventoryId = ReturnAll.InventoryId left join(select FromId, ToId, tblDamagedInventoryDetails.InventoryId, ISNULL(Sum(DamagedQuantity), 0) as DamagedAllQuantity from tblDamagedInventoryDetails  join tblDamagedInventory on tblDamagedInventoryDetails.DamagedInvId = tblDamagedInventory.DamagedInvId where [FromName] = 'Job' and[toName] = 'Site' and FromId = '" + databaseCon.Decrypt(Request.QueryString["RID"]) + "' group by[FromName],[ToName], FromId, ToId, tblDamagedInventoryDetails.InventoryId) DamagedAll On DeliveryAll.FromId = DamagedAll.ToId and DeliveryAll.InventoryId = DamagedAll.InventoryId left join tblSubCategory on tblSubCategory.SubCategoryID = DeliveryAll.SubCategoryID where 1 = 1 " + addquery + " order by MaterialType,ItemCode";


            // query = "select DeliveryAll.FromId ,DeliveryAll.ToId,MaterialType,ItemCode,ItemDesc , DeliveryAll.InventoryId,cast(DeliveryAll.DeliveryAvailableQuantity as int) as DeliveryAvailableQuantity,isnull(cast(ReturnAll.ReturnALLQuantity as int),0) as ReturnALLQuantity,isnull(cast(DamagedAll.DamagedAllQuantity as int),0) as DamagedALLQuantity,cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity,0) - ISNULL(DamagedAll.DamagedAllQuantity,0)) as int) as AvailableQuantity,DeliveryAll.ItemWeight,cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity,0) - ISNULL(DamagedAll.DamagedAllQuantity,0)) as int)*ItemWeight as TotalWeight  from (select FromId, ToId, MaterialType, ItemCode, ItemDesc, ItemWeight, tblTransactionHistDelivery.InventoryId, isnull(Sum(Quantity),0) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Site' and[to] = 'Job' and TransType = 'Send' and ToId = '" + databaseCon.Decrypt(Request.QueryString["RID"]) + "' group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc, ItemWeight)DeliveryAll left join(select FromId, ToId, tblReturnInventoryDetails.InventoryId, isnull(Sum(Quantity),0) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site' and FromId = '" + databaseCon.Decrypt(Request.QueryString["RID"]) + "' group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.InventoryId = ReturnAll.InventoryId left join (select FromId, ToId, tblDamagedInventoryDetails.InventoryId, ISNULL(Sum(DamagedQuantity), 0) as DamagedAllQuantity from tblDamagedInventoryDetails  join tblDamagedInventory on tblDamagedInventoryDetails.DamagedInvId = tblDamagedInventory.DamagedInvId where [FromName] = 'Job' and[toName] = 'Site' and FromId ='" + databaseCon.Decrypt(Request.QueryString["RID"]) + "' group by[FromName],[ToName], FromId, ToId, tblDamagedInventoryDetails.InventoryId) DamagedAll On DeliveryAll.FromId = DamagedAll.ToId and DeliveryAll.InventoryId = DamagedAll.InventoryId where 1=1" + addquery + "order by MaterialType,ItemCode";


            //query = "select tblTransactionHistDelivery.InventoryID,MaterialType , ItemCode , ItemDesc , Sum(tblTransactionHistDelivery.Quantity-isnull(ReturnQty.Quantity,0)) as Quantity1 , ItemWeight , Sum(tblTransactionHistDelivery.Quantity-isnull(ReturnQty.Quantity,0))*ItemWeight as TotalWeight from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId join (Select Quantity,FromId,InventoryId from tblReturnInventoryDetails join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId=tblReturnInventory.ReturnInvId)ReturnQty on ReturnQty.FromId=tblTransactionHistDelivery.ToId where [From]='site' and [to]='job' and ToId='" + Request.QueryString["RID"] + "'" + addquery + "group by tblTransactionHistDelivery.InventoryID,MaterialType,ItemCode,ItemDesc,ItemWeight";
            // dt = databaseCon.GetData(query);
            //  rowcount.Value = dt.Rows.Count.ToString();
            //  RadgridPhase2.DataSource = dt;

            dbContext.Database.CommandTimeout = 15 * 60;

            var GetData = dbContext.Database.SqlQuery<ScaffoldMaterialListPhase2ViewModel>(query).ToList();
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult btnoptimizeyes_Click(string id,string RID,string ddlMaterialType,string UID)
        {
            var Result = "";
            var Message = "";
            string addqury = "";
            if (ddlMaterialType != "" && ddlMaterialType != "All")
            {
                addqury = " and MaterialType='" + ddlMaterialType.Replace("'", "''").Trim() + "' ";

            }
            string checkquery = "select Sum(cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity,0)) as int)) as TotAvailableQuantity  from (select FromId, ToId, MaterialType, ItemCode, ItemDesc, ItemWeight, tblTransactionHistDelivery.InventoryId, isnull(Sum(Quantity),0) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Site' and[to] = 'Job' and TransType = 'Send' and ToId =  '" + databaseCon.Decrypt(RID) + "' group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc, ItemWeight)DeliveryAll left join(select FromId, ToId, tblReturnInventoryDetails.InventoryId, isnull(Sum(Quantity),0) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site' and FromId = '" + databaseCon.Decrypt(RID) + "' group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.InventoryId = ReturnAll.InventoryId";
            DataTable dtcheck = databaseCon.GetData(checkquery);
            if (dtcheck.Rows.Count > 0)
            {
                int totalava = Convert.ToInt32(dtcheck.Rows[0]["TotAvailableQuantity"]);
                if (totalava > 0)
                {
                    //Commented on 18-12-2023 added created by and Created On.
                    //string qury = "insert into tblOptimizedInventory(InventoryId,OptimizedQuantity,ClientId,RegisterId)select DeliveryAll.InventoryId,cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity,0)- ISNULL(DamagedAll.DamagedAllQuantity,0)) as int) as AvailableQuantity," + databaseCon.Decrypt(id) + "," + databaseCon.Decrypt(RID) + "  from (select FromId, ToId, MaterialType, ItemCode, ItemDesc, ItemWeight, tblTransactionHistDelivery.InventoryId, isnull(Sum(Quantity),0) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Site' and[to] = 'Job' and TransType = 'Send' and ToId = '" + databaseCon.Decrypt(RID) + "' group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc, ItemWeight)DeliveryAll left join(select FromId, ToId, tblReturnInventoryDetails.InventoryId, isnull(Sum(Quantity),0) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site' and FromId = '" + databaseCon.Decrypt(RID) + "' group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.InventoryId = ReturnAll.InventoryId left join (select FromId, ToId, tblDamagedInventoryDetails.InventoryId, ISNULL(Sum(DamagedQuantity), 0) as DamagedAllQuantity from tblDamagedInventoryDetails  join tblDamagedInventory on tblDamagedInventoryDetails.DamagedInvId = tblDamagedInventory.DamagedInvId where [FromName] = 'Job' and[toName] = 'Site' and FromId = '" + databaseCon.Decrypt(RID) + "' group by[FromName],[ToName], FromId, ToId, tblDamagedInventoryDetails.InventoryId) DamagedAll On DeliveryAll.FromId = DamagedAll.ToId and DeliveryAll.InventoryId = DamagedAll.InventoryId where 1=1  and cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity,0)) as int)>0 " + addqury + " order by MaterialType,ItemCode";

                    //Added on 18-12-2023 added Created by and Created On.
                    string qury = "insert into tblOptimizedInventory(InventoryId,OptimizedQuantity,ClientId,RegisterId,Createdby,CreatedOn)select DeliveryAll.InventoryId,cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity,0)- ISNULL(DamagedAll.DamagedAllQuantity,0)) as int) as AvailableQuantity," + databaseCon.Decrypt(id) + "," + databaseCon.Decrypt(RID) + ",(select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'),GETDATE()  from (select FromId, ToId, MaterialType, ItemCode, ItemDesc, ItemWeight, tblTransactionHistDelivery.InventoryId, isnull(Sum(Quantity),0) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Site' and[to] = 'Job' and TransType = 'Send' and ToId = '" + databaseCon.Decrypt(RID) + "' group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc, ItemWeight)DeliveryAll left join(select FromId, ToId, tblReturnInventoryDetails.InventoryId, isnull(Sum(Quantity),0) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site' and FromId = '" + databaseCon.Decrypt(RID) + "' group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.InventoryId = ReturnAll.InventoryId left join (select FromId, ToId, tblDamagedInventoryDetails.InventoryId, ISNULL(Sum(DamagedQuantity), 0) as DamagedAllQuantity from tblDamagedInventoryDetails  join tblDamagedInventory on tblDamagedInventoryDetails.DamagedInvId = tblDamagedInventory.DamagedInvId where [FromName] = 'Job' and[toName] = 'Site' and FromId = '" + databaseCon.Decrypt(RID) + "' group by[FromName],[ToName], FromId, ToId, tblDamagedInventoryDetails.InventoryId) DamagedAll On DeliveryAll.FromId = DamagedAll.ToId and DeliveryAll.InventoryId = DamagedAll.InventoryId where 1=1  and cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity,0)) as int)>0 " + addqury + " order by MaterialType,ItemCode";
                    int freezing = databaseCon.RunCmdID(qury);
                    if (freezing > 0)
                    {
                        string query = "update tblregister set optimizestatus='Green' where registerid=" + databaseCon.Decrypt(RID);
                        int updatestatus = databaseCon.RunCmdID(query);
                        //Session["Insert"] = "Insert";
                        //Response.Redirect("FinalMaterialList.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RID=" + Request.QueryString["RID"]);
                        Result = "Success";
                        Message = "Record Insert Successfully.";
                    }
                    else
                    {
                        //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Alert ! </strong>There is no data to finalise.";
                        //freezepopup.Style.Add("display", "none");
                        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp", "<script>hideAlert();</script>", false);
                        Result = "Failed";
                        Message = "There is no data to finalise.";
                    }
                }
                else
                {
                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong>There is no any Material present on job to finalise.";
                    //freezepopup.Style.Add("display", "none");

                    Result = "Failed";
                    Message = "There is no any Material present on job to finalise.";
                }
            }

            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTextTotalData (string id,string RID,string Siteid, string ddlMaterialType)
        {
            var EndDate = DateTime.Now.Date.ToString("MM-dd-yyyy");
            var TotalDailyHireCost = ""; 
            var query = "exec Proc_InvoiceCostReportSurgedate_GenerateReport '" + EndDate + "', '" + EndDate + "', " + databaseCon.Decrypt(id) + "," + databaseCon.Decrypt(Siteid) + ",'dJHC'," +databaseCon.Decrypt(RID) + ",'" + ddlMaterialType + "','" + EndDate + "', '" + EndDate + "'";


            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                var TotalDailyHireValue = Convert.ToString(dt.Rows[0]["TotalDailyHireValue"]);
                if (TotalDailyHireValue != "")
                {
                    TotalDailyHireCost = ("$" + TotalDailyHireValue);
                }
            }
            return Json(TotalDailyHireCost, JsonRequestBehavior.AllowGet);
        }

    }
}