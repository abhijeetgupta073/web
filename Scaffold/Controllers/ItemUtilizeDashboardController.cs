using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;

namespace Scaffold.Controllers
{
    public class ItemUtilizeDashboardController : Controller
    {
        ScaffoldEntities dbcontext = new ScaffoldEntities();
        // GET: ItemUtilizeDashboard
        public ActionResult ItemUtilizeDashboard()
        {
            return View();
        }

        public ActionResult SiteDropdownBind(string id)
        {
            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("Select SiteLocId,SiteLocation from tblsitelocation where ClientId='" + databaseCon.Decrypt(id) + "' Order by SiteLocation").ToList();
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ddlSite_SelectedIndexChanged(string SiteID)
        {
            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select distinct MaterialType from Vw_SiteInventory  join tblinventory on Vw_SiteInventory.InventoryId=tblInventory.InventoryID where SiteId='" + SiteID + "' Order by MaterialType").ToList();
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ddlMattype_SelectedIndexChanged(string SiteID, string ddlMattype)
        {
            //var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select '' as ItemCode,'All' as ItemCodeItemDesc Union select distinct ItemCode,ItemCode+'  -  '+ItemDesc as ItemCodeItemDesc from Vw_SiteInventory  join tblinventory on Vw_SiteInventory.InventoryId=tblInventory.InventoryID where SiteId=" + SiteID + " and MaterialType='" + ddlMattype.Replace("'", "''") + "' Order by ItemCode").ToList();
            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select '' as InventoryId,'All' as ItemCodeItemDesc Union select distinct tblinventory.InventoryId as InventoryId,ItemCode+'  -  '+ItemDesc as ItemCodeItemDesc from Vw_SiteInventory  join tblinventory on Vw_SiteInventory.InventoryId=tblInventory.InventoryID where SiteId=" + SiteID + " and MaterialType='" + ddlMattype.Replace("'", "''") + "'Order by InventoryId").ToList();
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ddlitemcode_SelectedIndexChanged(string SiteID, string ddlMattype, string InventoryID)
        {
            //var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select distinct ItemDesc from Vw_SiteInventory  join tblinventory on Vw_SiteInventory.InventoryId=tblInventory.InventoryID where SiteId=" + SiteID + " and MaterialType='" + ddlMattype.Replace("'", "''") + "' and ItemCode='" + ddlitemcode.Replace("'", "''") + "' Order by ItemDesc").ToList();
            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select distinct ItemDesc from Vw_SiteInventory  join tblinventory on Vw_SiteInventory.InventoryId=tblInventory.InventoryID where SiteId=" + SiteID + " and MaterialType='" + ddlMattype.Replace("'", "''") + "' and tblinventory.InventoryID='" + InventoryID.Replace("'", "''") + "' Order by ItemDesc").ToList();
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ddlitemdesc_SelectedIndexChanged(string ddlMattype, string InventoryID, string ddlSite)
        {

            var txtqtyonsite = "";
            var txtqtystanding = "";
            var txtNotstarted = "";
            var txtqtyAvaliable = "";
            var txtqtyDamaged = "";

            // Query Add On 03 Sep 2019 Calculate Values On Site.
            //string OnSitequery = "select isnull(cast(sum(isnull(OrgSiteQuantity,0)-isnull(QuantityReturnedToDepot,0)-Isnull(JobQuantityDamaged,0)-Isnull(SiteDamagedQuantityReturnToDepot,0))as int),0) as OnSiteAvailableQuantity from Vw_SiteInventory where InventoryID in(Select InventoryID from tblInventory where (MaterialType= case when '" + ddlMattype.Replace("'", "''") + "' ='' then MaterialType else '" + ddlMattype.Replace("'", "''") + "' end) and (ItemDesc=case when '" + ddlitemdesc.Replace("'", "''") + "' ='' then ItemDesc else '" + ddlitemdesc.Replace("'", "''") + "' end) ) and SiteId=" + ddlSite.Replace("'", "''");
            //DataTable OnSiteDt = databaseCon.GetData(OnSitequery);

            //if (OnSiteDt.Rows.Count > 0)
            //{
            //    // txtqtyonsite.Text = OnSiteDt.Rows[0]["OnSiteAvailableQuantity"].ToString();
            //    txtqtyonsite = OnSiteDt.Rows[0]["OnSiteAvailableQuantity"].ToString();
            //}

            //// Change Query On 01 July 2019

            //string query = "select isnull(cast(sum(AvailableQuantity)as int),0) as AvailableQuantity ,cast(Sum(isnull(JobQuantityDamaged,0)+Isnull(SiteDamagedQuantityReturnToDepot,0))as int) as DamageQuantity from Vw_SiteInventory  where InventoryID in(Select InventoryID from tblInventory where (MaterialType= case when '" + ddlMattype.Replace("'", "''") + "' ='' then MaterialType else '" + ddlMattype.Replace("'", "''") + "' end) and (ItemDesc=case when '" + ddlitemdesc.Replace("'", "''") + "' ='' then ItemDesc else '" + ddlitemdesc.Replace("'", "''") + "' end) ) and SiteId=" + ddlSite.Replace("'", "''");
            //DataTable dt = databaseCon.GetData(query);

            //if (dt.Rows.Count > 0)
            //{
            //    //txtqtyAvaliable.Text = dt.Rows[0]["AvailableQuantity"].ToString();
            //    //txtqtyDamaged.Text = dt.Rows[0]["DamageQuantity"].ToString();
            //    txtqtyAvaliable = dt.Rows[0]["AvailableQuantity"].ToString();
            //    txtqtyDamaged = dt.Rows[0]["DamageQuantity"].ToString();
            //}

            //// Query Change On 04 Feb 2020 join Condition on Vw_Register
            //string query1 = "select isnull(cast(sum(AvailableQuantity) as int), 0) as AvailableQuantity from(select DeliveryAll.FromId, DeliveryAll.ToId, cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0)) as int) * ItemWeight as TotalWeight, Right('000000' + convert(varchar, [scaffref]), 6) as ScaffRef, Location, BuildingNo, AssetNo, cast((DeliveryAll.DeliveryAvailableQuantity - (Isnull(ReturnAll.ReturnALLQuantity, 0)+isnull(DamageAll.JobQuantityDamaged,0))) as int) as AvailableQuantity  from(select FromId, ToId, MaterialType, ItemCode, ItemDesc, ItemWeight, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Site' and[to] = 'Job' and TransType = 'Send'  group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc, ItemWeight)DeliveryAll join VW_TblRegister on VW_TblRegister.SiteLocId = DeliveryAll.FromId and VW_TblRegister.RegisterId = DeliveryAll.ToId and DateTagged is not null and DateTagged!= '01/01/1900' and DateOfDismantle is null left join(select FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site'  group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.ToId = ReturnAll.FromId and DeliveryAll.InventoryId = ReturnAll.InventoryId Left join (select[FromName],FromId ,[ToName],ToId, tblDamagedInventoryDetails.InventoryId, isnull(Sum(DamagedQuantity),0) as JobQuantityDamaged from tblDamagedInventoryDetails join tblDamagedInventory on tblDamagedInventoryDetails.DamagedInvId = tblDamagedInventory.DamagedInvId where[FromName] ='job' and[ToName] = 'Site' group by[FromName],[ToName], ToId, FromId, tblDamagedInventoryDetails.InventoryId)DamageAll on DeliveryAll.FromId=DamageAll.ToId and DeliveryAll.ToId=DamageAll.FromId and DeliveryAll.InventoryId = DamageAll.InventoryId where 1 = 1 and DeliveryAll.FromId = '" + ddlSite.Replace("'", "''") + "' and (DeliveryAll.MaterialType= case when '" + ddlMattype.Replace("'", "''") + "' ='' then DeliveryAll.MaterialType else '" + ddlMattype.Replace("'", "''") + "' end) and (DeliveryAll.ItemDesc=case when '" + ddlitemdesc.Replace("'", "''") + "' ='' then DeliveryAll.ItemDesc else '" + ddlitemdesc.Replace("'", "''") + "' end)) QueryAll  ";



            //// Query Change On 06 Sep 2019 Damage Sub
            //// string query1 = "select isnull(cast(sum(AvailableQuantity) as int), 0) as AvailableQuantity from(select DeliveryAll.FromId, DeliveryAll.ToId, cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0)) as int) * ItemWeight as TotalWeight, Right('000000' + convert(varchar, [scaffref]), 6) as ScaffRef, Location, BuildingNo, AssetNo, cast((DeliveryAll.DeliveryAvailableQuantity - (Isnull(ReturnAll.ReturnALLQuantity, 0)+isnull(DamageAll.JobQuantityDamaged,0))) as int) as AvailableQuantity  from(select FromId, ToId, MaterialType, ItemCode, ItemDesc, ItemWeight, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Site' and[to] = 'Job' and TransType = 'Send'  group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc, ItemWeight)DeliveryAll join VW_TblRegister on VW_TblRegister.SiteLocId = DeliveryAll.FromId and VW_TblRegister.RegisterId = DeliveryAll.ToId left join(select FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site'  group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.ToId = ReturnAll.FromId and DeliveryAll.InventoryId = ReturnAll.InventoryId Left join (select[FromName],FromId ,[ToName],ToId, tblDamagedInventoryDetails.InventoryId, isnull(Sum(DamagedQuantity),0) as JobQuantityDamaged from tblDamagedInventoryDetails join tblDamagedInventory on tblDamagedInventoryDetails.DamagedInvId = tblDamagedInventory.DamagedInvId where[FromName] ='job' and[ToName] = 'Site' group by[FromName],[ToName], ToId, FromId, tblDamagedInventoryDetails.InventoryId)DamageAll on DeliveryAll.FromId=DamageAll.ToId and DeliveryAll.ToId=DamageAll.FromId and DeliveryAll.InventoryId = DamageAll.InventoryId where 1 = 1 and DeliveryAll.FromId = '" + ddlSite.SelectedValue.Replace("'", "''") + "' and (DeliveryAll.MaterialType= case when '" + ddlMattype.SelectedValue.Replace("'", "''") + "' ='' then DeliveryAll.MaterialType else '" + ddlMattype.SelectedValue.Replace("'", "''") + "' end) and (DeliveryAll.ItemDesc=case when '" + ddlitemdesc.SelectedValue.Replace("'", "''") + "' ='' then DeliveryAll.ItemDesc else '" + ddlitemdesc.SelectedValue.Replace("'", "''") + "' end)) QueryAll join VW_TblRegister on VW_TblRegister.RegisterId = QueryAll.ToId where DateTagged is not null and DateTagged!= '01/01/1900' and DateOfDismantle is null";

            //// string query1 = "select isnull(cast(sum(AvailableQuantity) as int), 0) as AvailableQuantity from(select DeliveryAll.FromId, DeliveryAll.ToId, cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0)) as int) * ItemWeight as TotalWeight, Right('000000' + convert(varchar, [scaffref]), 6) as ScaffRef, Location, BuildingNo, AssetNo, cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0)) as int) as AvailableQuantity  from(select FromId, ToId, MaterialType, ItemCode, ItemDesc, ItemWeight, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Site' and[to] = 'Job' and TransType = 'Send'  group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc, ItemWeight)DeliveryAll join VW_TblRegister on VW_TblRegister.SiteLocId = DeliveryAll.FromId and VW_TblRegister.RegisterId = DeliveryAll.ToId left join(select FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site'  group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.ToId = ReturnAll.FromId and DeliveryAll.InventoryId = ReturnAll.InventoryId where 1 = 1 and DeliveryAll.FromId = '" + ddlSite.SelectedValue.Replace("'", "''") + "' and (DeliveryAll.MaterialType= case when '" + ddlMattype.SelectedValue.Replace("'", "''") + "' ='' then DeliveryAll.MaterialType else '" + ddlMattype.SelectedValue.Replace("'", "''") + "' end) and (DeliveryAll.ItemDesc=case when '" + ddlitemdesc.SelectedValue.Replace("'", "''") + "' ='' then DeliveryAll.ItemDesc else '" + ddlitemdesc.SelectedValue.Replace("'", "''") + "' end)) QueryAll join VW_TblRegister on VW_TblRegister.RegisterId = QueryAll.ToId where DateTagged is not null and DateTagged!= '01/01/1900' and DateOfDismantle is null";


            ////  DeliveryAll.MaterialType = '" + ddlMattype.SelectedValue + "' and DeliveryAll.ItemDesc = '" + ddlitemdesc.SelectedValue.Replace("'", "''") + "') QueryAll join VW_TblRegister on VW_TblRegister.RegisterId = QueryAll.ToId where DateTagged is not null and DateTagged!= '01/01/1900' and DateOfDismantle is null";

            //// Change Query On 01 July 2019

            ////  string query1 = "select isnull(cast(sum(AvailableQuantity) as int), 0) as AvailableQuantity from(select DeliveryAll.FromId, DeliveryAll.ToId, cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0)) as int) * ItemWeight as TotalWeight, Right('000000' + convert(varchar, [scaffref]), 6) as ScaffRef, Location, BuildingNo, AssetNo, cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0)) as int) as AvailableQuantity  from(select FromId, ToId, MaterialType, ItemCode, ItemDesc, ItemWeight, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Site' and[to] = 'Job' and TransType = 'Send'  group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc, ItemWeight)DeliveryAll join VW_TblRegister on VW_TblRegister.SiteLocId = DeliveryAll.FromId and VW_TblRegister.RegisterId = DeliveryAll.ToId left join(select FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site'  group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.ToId = ReturnAll.FromId and DeliveryAll.InventoryId = ReturnAll.InventoryId where 1 = 1 and DeliveryAll.FromId = '" + ddlSite.SelectedValue + "' and DeliveryAll.MaterialType = '" + ddlMattype.SelectedValue + "' and DeliveryAll.ItemDesc = '" + ddlitemdesc.SelectedValue.Replace("'", "''") + "') QueryAll join VW_TblRegister on VW_TblRegister.RegisterId = QueryAll.ToId where DateTagged is not null and DateTagged!= '01/01/1900' and DateOfDismantle is null";

            ////string query1 = "select isnull(cast(sum(AvailableQuantity)as int),0) as AvailableQuantity from Vw_SiteInventory join VW_TblRegister on VW_TblRegister.SiteLocId=Vw_SiteInventory.SiteId where InventoryID in(Select InventoryID from tblInventory where MaterialType='" + ddlMattype.SelectedValue + "' and ItemDesc='" + ddlitemdesc.SelectedValue.Replace("'", "''") + "')  and SiteId=" + ddlSite.SelectedValue + "and DateTagged !='1900-01-01' and DateTagged is not null and DateOfDismantle !='1900-01-01' and DateOfDismantle is not null";


            //DataTable dt1 = databaseCon.GetData(query1);
            //if (dt1.Rows.Count > 0)
            //{
            //    //  txtqtystanding.Text = dt1.Rows[0]["AvailableQuantity"].ToString();
            //    txtqtystanding = dt1.Rows[0]["AvailableQuantity"].ToString();
            //}
            //string query2 = "select isnull(cast(sum(AvailableQuantity) as int), 0) as NotStarted from(select DeliveryAll.FromId, DeliveryAll.ToId, cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0)) as int) * ItemWeight as TotalWeight, Right('000000' + convert(varchar, [scaffref]), 6) as ScaffRef, Location, BuildingNo, AssetNo, cast((DeliveryAll.DeliveryAvailableQuantity -(Isnull(ReturnAll.ReturnALLQuantity, 0)+isnull(DamageAll.JobQuantityDamaged,0))) as int) as AvailableQuantity from VW_TblRegister left join (select FromId, ToId, MaterialType, ItemCode, ItemDesc, ItemWeight, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Site' and[to] = 'Job' and TransType = 'Send' group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc, ItemWeight)DeliveryAll on VW_TblRegister.SiteLocId = DeliveryAll.FromId and VW_TblRegister.RegisterId = DeliveryAll.ToId left join (select FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site'  group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.ToId = ReturnAll.FromId and DeliveryAll.InventoryId = ReturnAll.InventoryId Left join (select[FromName],FromId ,[ToName],ToId, tblDamagedInventoryDetails.InventoryId, isnull(Sum(DamagedQuantity),0) as JobQuantityDamaged from tblDamagedInventoryDetails join tblDamagedInventory on tblDamagedInventoryDetails.DamagedInvId = tblDamagedInventory.DamagedInvId where[FromName] ='job' and[ToName] = 'Site' group by[FromName],[ToName], ToId, FromId, tblDamagedInventoryDetails.InventoryId)DamageAll on DeliveryAll.FromId=DamageAll.ToId and DeliveryAll.ToId=DamageAll.FromId and DeliveryAll.InventoryId = DamageAll.InventoryId where 1 = 1 and DeliveryAll.FromId = '" + ddlSite.Replace("'", "''") + "' and (DeliveryAll.MaterialType= case when '" + ddlMattype.Replace("'", "''") + "' ='' then DeliveryAll.MaterialType else '" + ddlMattype.Replace("'", "''") + "' end) and (DeliveryAll.ItemDesc=case when '" + ddlitemdesc.Replace("'", "''") + "' ='' then DeliveryAll.ItemDesc else '" + ddlitemdesc.Replace("'", "''") + "' end)) QueryAll join VW_TblRegister on VW_TblRegister.RegisterId = QueryAll.ToId where DateTagged is null";

            //// Query Change On 06 Sep 2019 

            //// string query2 = "select isnull(cast(sum(AvailableQuantity) as int), 0) as NotStarted from(select DeliveryAll.FromId, DeliveryAll.ToId, cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0)) as int) * ItemWeight as TotalWeight, Right('000000' + convert(varchar, [scaffref]), 6) as ScaffRef, Location, BuildingNo, AssetNo, cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0)) as int) as AvailableQuantity from VW_TblRegister left join (select FromId, ToId, MaterialType, ItemCode, ItemDesc, ItemWeight, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Site' and[to] = 'Job' and TransType = 'Send' group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc, ItemWeight)DeliveryAll on VW_TblRegister.SiteLocId = DeliveryAll.FromId and VW_TblRegister.RegisterId = DeliveryAll.ToId left join (select FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site'  group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.ToId = ReturnAll.FromId and DeliveryAll.InventoryId = ReturnAll.InventoryId where 1 = 1 and DeliveryAll.FromId = '" + ddlSite.SelectedValue.Replace("'", "''") + "' and (DeliveryAll.MaterialType= case when '" + ddlMattype.SelectedValue.Replace("'", "''") + "' ='' then DeliveryAll.MaterialType else '" + ddlMattype.SelectedValue.Replace("'", "''") + "' end) and (DeliveryAll.ItemDesc=case when '" + ddlitemdesc.SelectedValue.Replace("'", "''") + "' ='' then DeliveryAll.ItemDesc else '" + ddlitemdesc.SelectedValue.Replace("'", "''") + "' end)) QueryAll join VW_TblRegister on VW_TblRegister.RegisterId = QueryAll.ToId where DateTagged is null";

            //// Change Query On 01 July 2019

            ////  string query2 = "select isnull(cast(sum(AvailableQuantity) as int), 0) as NotStarted from(select DeliveryAll.FromId, DeliveryAll.ToId, cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0)) as int) * ItemWeight as TotalWeight, Right('000000' + convert(varchar, [scaffref]), 6) as ScaffRef, Location, BuildingNo, AssetNo, cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0)) as int) as AvailableQuantity from VW_TblRegister left join (select FromId, ToId, MaterialType, ItemCode, ItemDesc, ItemWeight, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Site' and[to] = 'Job' and TransType = 'Send' group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc, ItemWeight)DeliveryAll on VW_TblRegister.SiteLocId = DeliveryAll.FromId and VW_TblRegister.RegisterId = DeliveryAll.ToId left join (select FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site'  group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.ToId = ReturnAll.FromId and DeliveryAll.InventoryId = ReturnAll.InventoryId where 1 = 1 and DeliveryAll.FromId = '" + ddlSite.SelectedValue + "' and DeliveryAll.MaterialType = 'AT-PAC' and DeliveryAll.ItemDesc = '" + ddlitemdesc.SelectedValue.Replace("'", "''") + "') QueryAll join VW_TblRegister on VW_TblRegister.RegisterId = QueryAll.ToId where DateTagged is null";

            ////string query2 = "select isnull(cast(sum(AvailableQuantity) as int), 0) as NotStarted from(select DeliveryAll.FromId, DeliveryAll.ToId, cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0)) as int) * ItemWeight as TotalWeight, Right('000000' + convert(varchar, [scaffref]), 6) as ScaffRef, Location, BuildingNo, AssetNo, cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0)) as int) as AvailableQuantity  from(select FromId, ToId, MaterialType, ItemCode, ItemDesc, ItemWeight, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Site' and[to] = 'Job' and TransType = 'Send'  group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc, ItemWeight)DeliveryAll join VW_TblRegister on VW_TblRegister.SiteLocId = DeliveryAll.FromId and VW_TblRegister.RegisterId = DeliveryAll.ToId left join(select FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site'  group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.ToId = ReturnAll.FromId and DeliveryAll.InventoryId = ReturnAll.InventoryId where 1 = 1 and DeliveryAll.FromId = '" + ddlSite.SelectedValue + "' and DeliveryAll.MaterialType = '" + ddlMattype.SelectedValue + "' and DeliveryAll.ItemDesc = '" + ddlitemdesc.SelectedValue.Replace("'", "''") + "') QueryAll join VW_TblRegister on VW_TblRegister.RegisterId = QueryAll.ToId where DateTagged is null";
            //DataTable dt2 = databaseCon.GetData(query2);
            try
            {
                string qry = "EXEC Pro_ddlitemdesc_SelectedIndexChanged " + ddlSite + ", '" + ddlMattype + "', " + InventoryID;
                dbcontext.Database.CommandTimeout = 15 * 60;
                DataTable dt2 = databaseCon.GetData(qry);
                if (dt2.Rows.Count > 0)
                {
                    txtqtyonsite = dt2.Rows[0]["txtqtyonsite"].ToString();
                    txtqtystanding = dt2.Rows[0]["txtqtystanding"].ToString();
                    txtNotstarted = dt2.Rows[0]["txtNotstarted"].ToString();
                    txtqtyAvaliable = dt2.Rows[0]["txtqtyAvaliable"].ToString();
                    txtqtyDamaged = dt2.Rows[0]["txtqtyDamaged"].ToString();
                }
            }
            catch (Exception ex)
            {
                return Json(new { error = true, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { txtqtyonsite = txtqtyonsite, txtqtystanding = txtqtystanding, txtNotstarted = txtNotstarted, txtqtyAvaliable = txtqtyAvaliable, txtqtyDamaged = txtqtyDamaged }, JsonRequestBehavior.AllowGet);
        }

        public class ItemUtilizeDashboardJobDetails
        {
            public Nullable<int> FromId { get; set; }
            public Nullable<int> ToId { get; set; }
            public Nullable<Double> TotalWeight { get; set; }
            public string ScaffRef { get; set; }
            public string Location { get; set; }
            public string BuildingNo { get; set; }
            public Nullable<int> AvailableQuantity { get; set; }
            public string AssetNo { get; set; }

        }

        //public void ExportToCsvButton_Click(string ddlSite, string ddlMattype, string ddlitemdesc)
        //{
        //    string qry = "";

        //    ddlSite = databaseCon.Decrypt(ddlSite);
        //    ddlMattype = databaseCon.Decrypt(ddlMattype);
        //    ddlitemdesc = databaseCon.Decrypt(ddlitemdesc);
        //    if (ddlSite != "")
        //    {
        //        if (ddlMattype != "")
        //        {
        //            if (ddlitemdesc != "")
        //            {
        //                qry = "select ScaffRef,MaterialType,ItemCode, ItemDesc,ItemWeight, Location,ScaffoldOwner as Discipline, BuildingNo, AssetNo, Sum(AvailableQuantity) as Quantity from(select DeliveryAll.FromId, DeliveryAll.ToId, DeliveryAll.MaterialType, ItemCode, ItemDesc, cast((DeliveryAll.DeliveryAvailableQuantity -Isnull(ReturnAll.ReturnALLQuantity, 0)) as int) * ItemWeight as TotalWeight, Right('000000' + convert(varchar,[scaffref]), 6) as ScaffRef,ItemWeight, Location,ScaffoldOwner, BuildingNo, AssetNo, cast((DeliveryAll.DeliveryAvailableQuantity - (Isnull(ReturnAll.ReturnALLQuantity, 0) + isnull(DamageAll.JobQuantityDamaged, 0))) as int) as AvailableQuantity from(  select FromId, ToId, MaterialType, ItemCode, ItemDesc, ItemWeight, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as DeliveryAvailableQuantity from tblTransactionHistDelivery    join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId  where [From] = 'Site' and[to] = 'Job' and TransType = 'Send' group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc, ItemWeight )DeliveryAll join VW_TblRegister on VW_TblRegister.SiteLocId = DeliveryAll.FromId and VW_TblRegister.RegisterId = DeliveryAll.ToId left join(select FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnAllQuantity  from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId =   tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site'  group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.toid = ReturnAll.fromid and DeliveryAll.InventoryId = ReturnAll.InventoryId Left join(select[FromName], FromId,[ToName], ToId, tblDamagedInventoryDetails.InventoryId,  isnull(Sum(DamagedQuantity), 0) as JobQuantityDamaged from tblDamagedInventoryDetails  join tblDamagedInventory on tblDamagedInventoryDetails.DamagedInvId = tblDamagedInventory.DamagedInvId   where[FromName] = 'job' and[ToName] = 'Site' group by[FromName],[ToName], ToId, FromId,   tblDamagedInventoryDetails.InventoryId)DamageAll on DeliveryAll.FromId = DamageAll.ToId and DeliveryAll.ToId = DamageAll.FromId and DeliveryAll.InventoryId = DamageAll.InventoryId where 1 = 1 and DeliveryAll.FromId =  " + ddlSite.Replace("'", "''") + " and DeliveryAll.MaterialType = '" + ddlMattype.Replace("'", "''") + "' and DeliveryAll.ItemDesc = '" + ddlitemdesc.Replace("'", "''") + "' ) QueryAll where AvailableQuantity > 0 group by ScaffRef,MaterialType,ItemWeight, Location,ScaffoldOwner, BuildingNo, AssetNo,ItemCode, ItemDesc";
        //            }
        //            else
        //            {
        //                qry = "select ScaffRef,MaterialType,ItemCode, ItemDesc,ItemWeight, Location,ScaffoldOwner as Discipline, BuildingNo, AssetNo, Sum(AvailableQuantity) as Quantity  from(select DeliveryAll.FromId, DeliveryAll.ToId, DeliveryAll.MaterialType, ItemCode, ItemDesc, cast((DeliveryAll.DeliveryAvailableQuantity -Isnull(ReturnAll.ReturnALLQuantity, 0)) as int) * ItemWeight as TotalWeight, Right('000000' + convert(varchar,[scaffref]), 6) as ScaffRef,ItemWeight, Location,ScaffoldOwner, BuildingNo, AssetNo, cast((DeliveryAll.DeliveryAvailableQuantity - (Isnull(ReturnAll.ReturnALLQuantity, 0) + isnull(DamageAll.JobQuantityDamaged, 0))) as int) as AvailableQuantity from(  select FromId, ToId, MaterialType, ItemCode, ItemDesc, ItemWeight, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as DeliveryAvailableQuantity from tblTransactionHistDelivery    join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId  where [From] = 'Site' and[to] = 'Job' and TransType = 'Send' group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc, ItemWeight )DeliveryAll join VW_TblRegister on VW_TblRegister.SiteLocId = DeliveryAll.FromId and VW_TblRegister.RegisterId = DeliveryAll.ToId left join(select FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnAllQuantity  from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId =   tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site'  group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.toid = ReturnAll.fromid and DeliveryAll.InventoryId = ReturnAll.InventoryId Left join(select[FromName], FromId,[ToName], ToId, tblDamagedInventoryDetails.InventoryId,  isnull(Sum(DamagedQuantity), 0) as JobQuantityDamaged from tblDamagedInventoryDetails  join tblDamagedInventory on tblDamagedInventoryDetails.DamagedInvId = tblDamagedInventory.DamagedInvId   where[FromName] = 'job' and[ToName] = 'Site' group by[FromName],[ToName], ToId, FromId,   tblDamagedInventoryDetails.InventoryId)DamageAll on DeliveryAll.FromId = DamageAll.ToId and DeliveryAll.ToId = DamageAll.FromId and DeliveryAll.InventoryId = DamageAll.InventoryId where 1 = 1 and DeliveryAll.FromId =  " + ddlSite.Replace("'", "''") + " and DeliveryAll.MaterialType = '" + ddlMattype.Replace("'", "''") + "') QueryAll where AvailableQuantity > 0 group by ScaffRef,MaterialType,ItemWeight, Location,ScaffoldOwner, BuildingNo, AssetNo,ItemCode, ItemDesc";
        //            }
        //        }
        //        else
        //        {
        //            qry = "select ScaffRef,MaterialType,ItemCode, ItemDesc,ItemWeight, Location,ScaffoldOwner as Discipline, BuildingNo, AssetNo, Sum(AvailableQuantity) as Quantity  from(select DeliveryAll.FromId, DeliveryAll.ToId, DeliveryAll.MaterialType, ItemCode, ItemDesc, cast((DeliveryAll.DeliveryAvailableQuantity -Isnull(ReturnAll.ReturnALLQuantity, 0)) as int) * ItemWeight as TotalWeight, Right('000000' + convert(varchar,[scaffref]), 6) as ScaffRef,ItemWeight, Location,ScaffoldOwner, BuildingNo, AssetNo, cast((DeliveryAll.DeliveryAvailableQuantity - (Isnull(ReturnAll.ReturnALLQuantity, 0) + isnull(DamageAll.JobQuantityDamaged, 0))) as int) as AvailableQuantity from(  select FromId, ToId, MaterialType, ItemCode, ItemDesc, ItemWeight, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as DeliveryAvailableQuantity from tblTransactionHistDelivery    join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId  where [From] = 'Site' and[to] = 'Job' and TransType = 'Send' group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc, ItemWeight )DeliveryAll join VW_TblRegister on VW_TblRegister.SiteLocId = DeliveryAll.FromId and VW_TblRegister.RegisterId = DeliveryAll.ToId left join(select FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnAllQuantity  from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId =   tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site'  group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.toid = ReturnAll.fromid and DeliveryAll.InventoryId = ReturnAll.InventoryId Left join(select[FromName], FromId,[ToName], ToId, tblDamagedInventoryDetails.InventoryId,  isnull(Sum(DamagedQuantity), 0) as JobQuantityDamaged from tblDamagedInventoryDetails  join tblDamagedInventory on tblDamagedInventoryDetails.DamagedInvId = tblDamagedInventory.DamagedInvId   where[FromName] = 'job' and[ToName] = 'Site' group by[FromName],[ToName], ToId, FromId,   tblDamagedInventoryDetails.InventoryId)DamageAll on DeliveryAll.FromId = DamageAll.ToId and DeliveryAll.ToId = DamageAll.FromId and DeliveryAll.InventoryId = DamageAll.InventoryId where 1 = 1 and DeliveryAll.FromId =  " + ddlSite.Replace("'", "''") + ") QueryAll where AvailableQuantity > 0 group by ScaffRef,MaterialType,ItemWeight, Location,ScaffoldOwner, BuildingNo, AssetNo,ItemCode, ItemDesc";

        //        }
        //    }
        //    DataTable dt1 = databaseCon.GetData(qry);
        //    Response.ContentType = "Application/x-msexcel";
        //    Response.AddHeader("content-disposition", "attachment;filename=Export Standing Job Items.csv");
        //    Response.Write(csvfile(dt1));
        //    Response.End();

        //}
        //Added By Abhijeet gupta On 03-08-2024 for inhance performance
        public void ExportToCsvButton_Click(string ddlSite, string ddlMattype, string InventoryID, string id)
        {
            string decryptedSite = databaseCon.Decrypt(ddlSite);
            string decryptedMattype = databaseCon.Decrypt(ddlMattype);
            string InventoryId = databaseCon.Decrypt(InventoryID);
            string decryptedId = databaseCon.Decrypt(id);
            string qry = "EXEC Pro_GetInventoryStatus " + decryptedId + ", " + decryptedSite + ", '" + decryptedMattype + "', " + InventoryId + ", 'Standing'";
            dbcontext.Database.CommandTimeout = 15 * 60;
            DataTable dt1 = databaseCon.GetData(qry);
            Response.ContentType = "Application/x-msexcel";
            Response.AddHeader("content-disposition", "attachment;filename=Export Standing Job Items.csv");
            Response.Write(csvfile(dt1));
            Response.End();
        }
        public void ExportToCsvButton_Click12(string ddlSite, string ddlMattype, string InventoryID,string id)
        {
            ddlSite = databaseCon.Decrypt(ddlSite);
            ddlMattype = databaseCon.Decrypt(ddlMattype);
            InventoryID = databaseCon.Decrypt(InventoryID);
            string decryptedId = databaseCon.Decrypt(id);
            string qry = "EXEC Pro_GetInventoryStatus " + decryptedId + ", " + ddlSite + ", '" + ddlMattype + "', " + InventoryID + ", 'Dismantled'";
            //if (ddlSite != "")
            //{
            //    if (ddlMattype != "")
            //    {
            //        if (ddlitemdesc != "")
            //        {

            //            qry = "select Right('000000' + convert(varchar,[scaffref]), 6) as ScaffRef,tblInventory.MaterialType,ItemCode,ItemDesc,ItemWeight,Location,ScaffoldOwner as Discipline,BuildingNo,AssetNo, optimizedQuantity as Quantity from tblOptimizedInventory join tblInventory on tblOptimizedInventory.inventoryid = tblInventory.InventoryID Join VW_TblRegister on tblOptimizedInventory.RegisterId = VW_TblRegister.RegisterId where  SiteLocId =  " + ddlSite.Replace("'", "''") + " and tblInventory.MaterialType = '" + ddlMattype.Replace("'", "''") + "' and ItemDesc = '" + ddlitemdesc.Replace("'", "''") + "'  And optimizedQuantity > 0 and IsNull(DateOfDismantle,'1900-01-01') != '1900-01-01' and Status = 'Completed' Order by ScaffRef, Location, BuildingNo, AssetNo,ItemCode, ItemDesc";
            //        }
            //        else
            //        {
            //            qry = "select Right('000000' + convert(varchar,[scaffref]), 6) as ScaffRef,tblInventory.MaterialType,ItemCode,ItemDesc,ItemWeight,Location,ScaffoldOwner as Discipline,BuildingNo,AssetNo, optimizedQuantity as Quantity from tblOptimizedInventory join tblInventory on tblOptimizedInventory.inventoryid = tblInventory.InventoryID Join VW_TblRegister on tblOptimizedInventory.RegisterId = VW_TblRegister.RegisterId where  SiteLocId =  " + ddlSite.Replace("'", "''") + " and tblInventory.MaterialType = '" + ddlMattype.Replace("'", "''") + "'  And optimizedQuantity > 0 and IsNull(DateOfDismantle,'1900-01-01') != '1900-01-01' and Status = 'Completed' Order by ScaffRef, Location, BuildingNo, AssetNo,ItemCode, ItemDesc";

            //        }
            //    }
            //    else
            //    {
            //        qry = "select Right('000000' + convert(varchar,[scaffref]), 6) as ScaffRef,tblInventory.MaterialType,ItemCode,ItemDesc,ItemWeight,Location,ScaffoldOwner as Discipline,BuildingNo,AssetNo, optimizedQuantity as Quantity from tblOptimizedInventory join tblInventory on tblOptimizedInventory.inventoryid = tblInventory.InventoryID Join VW_TblRegister on tblOptimizedInventory.RegisterId = VW_TblRegister.RegisterId where  SiteLocId =  " + ddlSite.Replace("'", "'") + "  And optimizedQuantity > 0 and IsNull(DateOfDismantle,'1900-01-01') != '1900-01-01' and Status = 'Completed' Order by ScaffRef, Location, BuildingNo, AssetNo,ItemCode, ItemDesc";

            //    }
            //}
            dbcontext.Database.CommandTimeout = 15 * 60;
            DataTable dt1 = databaseCon.GetData(qry);
            Response.ContentType = "Application/x-msexcel";
            Response.AddHeader("content-disposition", "attachment;filename= Export Dismantled Job Items .csv");
            Response.Write(csvfile(dt1));
            Response.End();

        }
        public ActionResult GetJobListData(string ddlSite, string ddlMattype, string InventoryID, string id)
        {
            //string qry = "";
            //if (ddlSite != "")
            //{
            //    if (ddlMattype != "")
            //    {
            //        if (ddlitemdesc != "")
            //        {
            //            qry = "select FromId, ToId, Sum(TotalWeight) / 1000 as TotalWeight, ScaffRef, Location, BuildingNo, AssetNo, Sum(AvailableQuantity) as AvailableQuantity  from (select DeliveryAll.FromId, DeliveryAll.ToId, cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0)) as int) * ItemWeight as TotalWeight, Right('000000' + convert(varchar, [scaffref]), 6) as ScaffRef, Location, BuildingNo, AssetNo, cast((DeliveryAll.DeliveryAvailableQuantity - (Isnull(ReturnAll.ReturnALLQuantity, 0)+isnull(DamageAll.JobQuantityDamaged,0))) as int) as AvailableQuantity  from(select FromId, ToId, MaterialType, ItemCode, ItemDesc, ItemWeight, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Site' and[to] = 'Job' and TransType = 'Send'  group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc, ItemWeight )DeliveryAll join VW_TblRegister on VW_TblRegister.SiteLocId = DeliveryAll.FromId and VW_TblRegister.RegisterId = DeliveryAll.ToId left join( select FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site'  group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.toid = ReturnAll.fromid and DeliveryAll.InventoryId = ReturnAll.InventoryId Left join (select[FromName],FromId ,[ToName],ToId, tblDamagedInventoryDetails.InventoryId, isnull(Sum(DamagedQuantity),0) as JobQuantityDamaged from tblDamagedInventoryDetails join tblDamagedInventory on tblDamagedInventoryDetails.DamagedInvId = tblDamagedInventory.DamagedInvId where[FromName] ='job' and[ToName] = 'Site' group by[FromName],[ToName], ToId, FromId, tblDamagedInventoryDetails.InventoryId)DamageAll on DeliveryAll.FromId=DamageAll.ToId and DeliveryAll.ToId=DamageAll.FromId and DeliveryAll.InventoryId = DamageAll.InventoryId where 1 = 1 and DeliveryAll.FromId = " + ddlSite.Replace("'", "''") + " and DeliveryAll.MaterialType='" + ddlMattype.Replace("'", "''") + "' and DeliveryAll.ItemDesc='" + ddlitemdesc.Replace("'", "''") + "' ) QueryAll where AvailableQuantity>0 group by FromId, ToId, ScaffRef, Location, BuildingNo, AssetNo";

            //            // qry = "select FromId, ToId, Sum(TotalWeight) / 1000 as TotalWeight, ScaffRef, Location, BuildingNo, AssetNo, Sum(AvailableQuantity) as AvailableQuantity  from (select DeliveryAll.FromId, DeliveryAll.ToId, cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0)) as int) * ItemWeight as TotalWeight, Right('000000' + convert(varchar, [scaffref]), 6) as ScaffRef, Location, BuildingNo, AssetNo, cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0)) as int) as AvailableQuantity  from(select FromId, ToId, MaterialType, ItemCode, ItemDesc, ItemWeight, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Site' and[to] = 'Job' and TransType = 'Send'  group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc, ItemWeight )DeliveryAll join VW_TblRegister on VW_TblRegister.SiteLocId = DeliveryAll.FromId and VW_TblRegister.RegisterId = DeliveryAll.ToId left join( select FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site'  group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.toid = ReturnAll.fromid and DeliveryAll.InventoryId = ReturnAll.InventoryId where 1 = 1 and DeliveryAll.FromId = " + ddlSite.SelectedValue.Replace("'", "''") + " and DeliveryAll.MaterialType='" + ddlMattype.SelectedValue.Replace("'", "''") + "' and DeliveryAll.ItemDesc='" + ddlitemdesc.SelectedValue.Replace("'", "''") + "' ) QueryAll where AvailableQuantity>0 group by FromId, ToId, ScaffRef, Location, BuildingNo, AssetNo";

            //        }
            //        else
            //        {
            //            qry = "select FromId, ToId, Sum(TotalWeight) / 1000 as TotalWeight, ScaffRef, Location, BuildingNo, AssetNo, Sum(AvailableQuantity) as AvailableQuantity  from (select DeliveryAll.FromId, DeliveryAll.ToId, cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0)) as int) * ItemWeight as TotalWeight, Right('000000' + convert(varchar, [scaffref]), 6) as ScaffRef, Location, BuildingNo, AssetNo, cast((DeliveryAll.DeliveryAvailableQuantity - (Isnull(ReturnAll.ReturnALLQuantity, 0)+isnull(DamageAll.JobQuantityDamaged,0))) as int) as AvailableQuantity  from(select FromId, ToId, MaterialType, ItemCode, ItemDesc, ItemWeight, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Site' and[to] = 'Job' and TransType = 'Send'  group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc, ItemWeight )DeliveryAll join VW_TblRegister on VW_TblRegister.SiteLocId = DeliveryAll.FromId and VW_TblRegister.RegisterId = DeliveryAll.ToId left join( select FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site'  group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.toid = ReturnAll.fromid and DeliveryAll.InventoryId = ReturnAll.InventoryId Left join (select[FromName],FromId ,[ToName],ToId, tblDamagedInventoryDetails.InventoryId, isnull(Sum(DamagedQuantity),0) as JobQuantityDamaged from tblDamagedInventoryDetails join tblDamagedInventory on tblDamagedInventoryDetails.DamagedInvId = tblDamagedInventory.DamagedInvId where[FromName] ='job' and[ToName] = 'Site' group by[FromName],[ToName], ToId, FromId, tblDamagedInventoryDetails.InventoryId)DamageAll on DeliveryAll.FromId=DamageAll.ToId and DeliveryAll.ToId=DamageAll.FromId and DeliveryAll.InventoryId = DamageAll.InventoryId where 1 = 1 and DeliveryAll.FromId = " + ddlSite.Replace("'", "''") + " and DeliveryAll.MaterialType='" + ddlMattype.Replace("'", "''") + "') QueryAll where AvailableQuantity>0 group by FromId, ToId, ScaffRef, Location, BuildingNo, AssetNo";

            //            // qry = "select FromId, ToId, Sum(TotalWeight) / 1000 as TotalWeight, ScaffRef, Location, BuildingNo, AssetNo, Sum(AvailableQuantity) as AvailableQuantity  from (select DeliveryAll.FromId, DeliveryAll.ToId, cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0)) as int) * ItemWeight as TotalWeight, Right('000000' + convert(varchar, [scaffref]), 6) as ScaffRef, Location, BuildingNo, AssetNo, cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0)) as int) as AvailableQuantity  from(select FromId, ToId, MaterialType, ItemCode, ItemDesc, ItemWeight, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Site' and[to] = 'Job' and TransType = 'Send'  group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc, ItemWeight )DeliveryAll join VW_TblRegister on VW_TblRegister.SiteLocId = DeliveryAll.FromId and VW_TblRegister.RegisterId = DeliveryAll.ToId left join( select FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site'  group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.toid = ReturnAll.fromid and DeliveryAll.InventoryId = ReturnAll.InventoryId where 1 = 1 and DeliveryAll.FromId = " + ddlSite.SelectedValue.Replace("'", "''") + " and DeliveryAll.MaterialType='" + ddlMattype.SelectedValue.Replace("'", "''") + "') QueryAll where AvailableQuantity>0 group by FromId, ToId, ScaffRef, Location, BuildingNo, AssetNo";
            //        }
            //    }
            //    else
            //    {
            //        qry = "select FromId, ToId, Sum(TotalWeight) / 1000 as TotalWeight, ScaffRef, Location, BuildingNo, AssetNo, Sum(AvailableQuantity) as AvailableQuantity  from (select DeliveryAll.FromId, DeliveryAll.ToId, cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0)) as int) * ItemWeight as TotalWeight, Right('000000' + convert(varchar, [scaffref]), 6) as ScaffRef, Location, BuildingNo, AssetNo, cast((DeliveryAll.DeliveryAvailableQuantity - (Isnull(ReturnAll.ReturnALLQuantity, 0)+isnull(DamageAll.JobQuantityDamaged,0))) as int) as AvailableQuantity  from(select FromId, ToId, MaterialType, ItemCode, ItemDesc, ItemWeight, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Site' and[to] = 'Job' and TransType = 'Send'  group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc, ItemWeight )DeliveryAll join VW_TblRegister on VW_TblRegister.SiteLocId = DeliveryAll.FromId and VW_TblRegister.RegisterId = DeliveryAll.ToId left join( select FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site'  group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.toid = ReturnAll.fromid and DeliveryAll.InventoryId = ReturnAll.InventoryId Left join (select[FromName],FromId ,[ToName],ToId, tblDamagedInventoryDetails.InventoryId, isnull(Sum(DamagedQuantity),0) as JobQuantityDamaged from tblDamagedInventoryDetails join tblDamagedInventory on tblDamagedInventoryDetails.DamagedInvId = tblDamagedInventory.DamagedInvId where[FromName] ='job' and[ToName] = 'Site' group by[FromName],[ToName], ToId, FromId, tblDamagedInventoryDetails.InventoryId)DamageAll on DeliveryAll.FromId=DamageAll.ToId and DeliveryAll.ToId=DamageAll.FromId and DeliveryAll.InventoryId = DamageAll.InventoryId where 1 = 1 and DeliveryAll.FromId = " + ddlSite.Replace("'", "''") + ") QueryAll where AvailableQuantity>0 group by FromId, ToId, ScaffRef, Location, BuildingNo, AssetNo";

            //        // qry = "select FromId, ToId, Sum(TotalWeight) / 1000 as TotalWeight, ScaffRef, Location, BuildingNo, AssetNo, Sum(AvailableQuantity) as AvailableQuantity  from (select DeliveryAll.FromId, DeliveryAll.ToId, cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0)) as int) * ItemWeight as TotalWeight, Right('000000' + convert(varchar, [scaffref]), 6) as ScaffRef, Location, BuildingNo, AssetNo, cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0)) as int) as AvailableQuantity  from(select FromId, ToId, MaterialType, ItemCode, ItemDesc, ItemWeight, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Site' and[to] = 'Job' and TransType = 'Send'  group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc, ItemWeight )DeliveryAll join VW_TblRegister on VW_TblRegister.SiteLocId = DeliveryAll.FromId and VW_TblRegister.RegisterId = DeliveryAll.ToId left join( select FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site'  group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.toid = ReturnAll.fromid and DeliveryAll.InventoryId = ReturnAll.InventoryId where 1 = 1 and DeliveryAll.FromId = " + ddlSite.SelectedValue.Replace("'", "''") + ") QueryAll where AvailableQuantity>0 group by FromId, ToId, ScaffRef, Location, BuildingNo, AssetNo";

            //    }
            //}
            ddlSite = databaseCon.Decrypt(ddlSite);
            ddlMattype = databaseCon.Decrypt(ddlMattype);
            InventoryID = databaseCon.Decrypt(InventoryID);
            string decryptedId = databaseCon.Decrypt(id);
            string qry = "EXEC Pro_GetInventoryStatus " + decryptedId + ", " + ddlSite + ", '" + ddlMattype + "', " + InventoryID + ", 'GetJobData'";
            dbcontext.Database.CommandTimeout = 15 * 60;
            DataTable dt = databaseCon.GetData(qry);
            //MatDetail.DataSource = dt;
            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp", "<script>Chart1('" + ddlSite.SelectedValue.Replace("'", "''") + "');Chart2('" + ddlSite.SelectedValue.Replace("'", "''") + "');abc();</script>", false);
            var GetData = dbcontext.Database.SqlQuery<ItemUtilizeDashboardJobDetails>(qry).ToList();
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }




        /// Item Utilization Dashboard On Site Material Break-Down (Tonnes) Chart 1 Start
        List<ScaffoldAllDashBoardViewModel> ItemUtilizeDashboardChart1 = new List<ScaffoldAllDashBoardViewModel> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod(EnableSession = true)]
        //public List<Scaffold> ItemUtilizeFirst(string Siteid)
        public ActionResult ItemUtilizeFirst(string Siteid)
        {
            string query = "select MaterialType, Convert(decimal(18, 2), Sum(PerMaterialWeight) / 1000) as Tonnage ,case when TotalWeight > 0 then Convert(decimal(18, 2), Sum(PerMaterialWeight) / TotalWeight) else 0 end as Pct from(select vw_SiteInventory.InventoryId, MaterialTYpe,Sum((isnull(OrgSiteQuantity, 0) - isnull(QuantityReturnedToDepot, 0)-Isnull(JobQuantityDamaged,0)-Isnull(SiteDamagedQuantityReturnToDepot,0)) * ItemWeight) as PerMaterialWeight from vw_SiteInventory join tblInventory On tblInventory.InventoryId = vw_SiteInventory.InventoryId where SiteId = '" + Siteid + "' and(OrgSiteQuantity - QuantityReturnedToDepot) > 0 group by vw_SiteInventory.InventoryId, MaterialTYpe)ByMaterialType , (select Sum((isnull(OrgSiteQuantity, 0) - isnull(QuantityReturnedToDepot, 0)-Isnull(JobQuantityDamaged,0)-Isnull(SiteDamagedQuantityReturnToDepot,0)) * ItemWeight) as TotalWeight from vw_SiteInventory join tblInventory On tblInventory.InventoryId = vw_SiteInventory.InventoryId where SiteId ='" + Siteid + "' )SumTotal group by MaterialType, SumTotal.TotalWeight order by SumTotal.TotalWeight";

            // Query Change On 3 Sep 2019 Graphs Make On Avaliable Qty.But After Change Query Graphs Make On Site Qty. 

            //  string query = "select MaterialType, Convert(decimal(18, 2), Sum(PerMaterialWeight) / 1000) as Tonnage ,case when TotalWeight > 0 then  Convert(decimal(18, 2), Sum(PerMaterialWeight) / TotalWeight) else 0 end as Pct from(select vw_SiteInventory.InventoryId, MaterialTYpe, Sum(AvailableQuantity * ItemWeight) as PerMaterialWeight from vw_SiteInventory join tblInventory On tblInventory.InventoryId = vw_SiteInventory.InventoryId where SiteId = '" + Siteid + "' and (OrgSiteQuantity - QuantityReturnedToDepot)>0 group by vw_SiteInventory.InventoryId, MaterialTYpe)ByMaterialType , (select Sum(AvailableQuantity * ItemWeight) as TotalWeight from vw_SiteInventory join tblInventory On tblInventory.InventoryId = vw_SiteInventory.InventoryId where SiteId = '" + Siteid + "' )SumTotal group by MaterialType, SumTotal.TotalWeight order by SumTotal.TotalWeight";

            //string query = "select MaterialType, Tonnage, TotalTonnage, cast(convert(decimal(18, 2), Sum(isnull(Tonnage, 0))) / TotalTonnage as numeric(36, 2)) as Pct from(select MaterialType, Convert(Decimal(18, 2), Sum(cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0)) as int) * ItemWeight) / 1000) as Tonnage  from(select FromId, ToId, MaterialType, ItemCode, ItemDesc, ItemWeight, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Site' and[to] = 'Job' and TransType = 'Send'  group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc, ItemWeight)DeliveryAll left join(select FromId, ToId, tblReturnInventoryDetails.InventoryId, Sum(Quantity) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site'  group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.InventoryId = ReturnAll.InventoryId  where 1 = 1 and DeliveryAll.FromId =  '" + Siteid + "' group by DeliveryAll.FromId, MaterialType)AllMatType , (select Convert(Decimal(18, 2), Sum(cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0)) as int) * ItemWeight) / 1000) as TotalTonnage  from(select FromId, ToId, MaterialType, ItemCode, ItemDesc, ItemWeight, tblTransactionHistDelivery.InventoryId, Sum(Quantity) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Site' and[to] = 'Job' and TransType = 'Send'  group by[From],[To], FromId, ToId, MaterialType, ItemCode, ItemDesc, ItemWeight, tblTransactionHistDelivery.InventoryId)DeliveryAll left join(select FromId, ToId, InventoryId, Sum(Quantity) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site'  group by[From],[To], FromId, ToId, InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and ReturnAll.InventoryId = DeliveryAll.InventoryId where 1 = 1 and DeliveryAll.FromId = '" + Siteid + "' group by DeliveryAll.FromId) SumTotal group by AllMatType.MaterialType,AllMatType.Tonnage, SumTotal.TotalTonnage order by MaterialType";

            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ScaffoldAllDashBoardViewModel pro = new ScaffoldAllDashBoardViewModel();
                    if (!(item["MaterialType"] is DBNull) && item["MaterialType"].ToString() != "")
                    {
                        pro.Material = item["MaterialType"].ToString();
                    }
                    if (!(item["Tonnage"] is DBNull) && item["Tonnage"].ToString() != "")
                    {
                        pro.TotalTonnes = Convert.ToDecimal(item["Tonnage"].ToString());
                    }
                    if (!(item["Pct"] is DBNull) && item["Pct"].ToString() != "")
                    {
                        pro.PctTonnage = Convert.ToDecimal(item["Pct"]);
                    }
                    ItemUtilizeDashboardChart1.Add(pro);
                }
            }
            // return ItemUtilizeDashboardChart1;
            return Json(ItemUtilizeDashboardChart1, JsonRequestBehavior.AllowGet);
        }
       
        List<ScaffoldAllDashBoardViewModel> ItemUtilizeDashboardChart2 = new List<ScaffoldAllDashBoardViewModel> { };
     
        public ActionResult ItemUtilizeSecond(string Siteid)
        {

            //  string query = "select Convert(decimal(18, 2),[InSite]/1000)-Convert(decimal(18, 2),[OutOfSite]/1000) [InSite] , Convert(decimal(18, 2),[OutOfSite]/1000) [OutOfSite],Convert(decimal(18,2),[OnSite]/1000) [OnSite],Convert(decimal(18, 2), ([InSite]/([InSite] + [OutOfSite]+[OnSite])))InSitepct, Convert(decimal(18, 2),([OutOfSite] / ([InSite] + [OutOfSite]+[OnSite]))) OutOfSitepct,Convert(decimal(18, 2),([OnSite] / ([InSite] + [OutOfSite]+[OnSite]))) OnSitepct from(select  Sum(AvailableQuantity * ItemWeight) as 'InSite',Sum((isnull(QuantityToBeSent, 0)) * ItemWeight) as  'OutOfSite',Sum(((isnull(OrgSiteQuantity,0)-isnull(QuantityReturnedToDepot,0))* ItemWeight)) as 'OnSite' from vw_SiteInventory join tblInventory On tblInventory.InventoryId = vw_SiteInventory.InventoryId where SiteId ='" + Siteid + "' ) as vw_inv";

            //Commented on 11-04-2023 To Remove (Isnull(JobQuantityDamaged,0)-Isnull(SiteDamagedQuantityReturnToDepot,0)) as client want to exclude.
            //string query = "select Convert(decimal(18, 2),[InSite]/1000) [InSite] , Convert(decimal(18, 2),[OutOfSite]/1000) [OutOfSite],Convert(decimal(18,2),[OnSite]/1000) [OnSite],Convert(decimal(18, 2), ([InSite]/([InSite] + [OutOfSite]+[OnSite])))InSitepct, Convert(decimal(18, 2),([OutOfSite] / ([InSite] + [OutOfSite]+[OnSite]))) OutOfSitepct,Convert(decimal(18, 2),([OnSite] / ([InSite] + [OutOfSite]+[OnSite]))) OnSitepct from(select  Sum(AvailableQuantity * ItemWeight) as 'InSite',Sum((isnull(QuantityToBeSent, 0)) * ItemWeight) as  'OutOfSite',Sum(((isnull(OrgSiteQuantity,0)-isnull(QuantityReturnedToDepot,0)-Isnull(JobQuantityDamaged,0)-Isnull(SiteDamagedQuantityReturnToDepot,0))* ItemWeight)) as 'OnSite' from vw_SiteInventory join tblInventory On tblInventory.InventoryId = vw_SiteInventory.InventoryId where SiteId ='" + Siteid + "' ) as vw_inv";

           // string query = "select Convert(decimal(18, 2),[InSite]/1000) [InSite] , Convert(decimal(18, 2),[OutOfSite]/1000) [OutOfSite],Convert(decimal(18,2),[OnSite]/1000) [OnSite],Convert(decimal(18, 2), ([InSite]/([InSite] + [OutOfSite]+[OnSite])))InSitepct, Convert(decimal(18, 2),([OutOfSite] / ([InSite] + [OutOfSite]+[OnSite]))) OutOfSitepct,Convert(decimal(18, 2),([OnSite] / ([InSite] + [OutOfSite]+[OnSite]))) OnSitepct from(select  Sum(AvailableQuantity * ItemWeight) as 'InSite',Sum((isnull(QuantityToBeSent, 0)) * ItemWeight) as  'OutOfSite',Sum(((isnull(OrgSiteQuantity,0)-isnull(QuantityReturnedToDepot,0))* ItemWeight)) as 'OnSite' from vw_SiteInventory join tblInventory On tblInventory.InventoryId = vw_SiteInventory.InventoryId where SiteId ='" + Siteid + "' ) as vw_inv";

            //added on 27feb2026 by pooja to handle Divide by zero error encountered
            string query = "select Convert(decimal(18, 2),[InSite]/1000) [InSite] , Convert(decimal(18, 2),[OutOfSite]/1000) [OutOfSite],Convert(decimal(18,2),[OnSite]/1000) [OnSite],Convert(decimal(18, 2), ([InSite]/NULLIF(([InSite] + [OutOfSite] + [OnSite]), 0)))InSitepct, Convert(decimal(18, 2),([OutOfSite] / NULLIF(([InSite] + [OutOfSite] + [OnSite]), 0))) OutOfSitepct,Convert(decimal(18, 2),([OnSite] / NULLIF(([InSite] + [OutOfSite] + [OnSite]), 0))) OnSitepct from(select  Sum(AvailableQuantity * ItemWeight) as 'InSite',Sum((isnull(QuantityToBeSent, 0)) * ItemWeight) as  'OutOfSite',Sum(((isnull(OrgSiteQuantity,0)-isnull(QuantityReturnedToDepot,0))* ItemWeight)) as 'OnSite' from vw_SiteInventory join tblInventory On tblInventory.InventoryId = vw_SiteInventory.InventoryId where SiteId ='" + Siteid + "' ) as vw_inv";

            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ScaffoldAllDashBoardViewModel pro = new ScaffoldAllDashBoardViewModel();
                    if (!(item["InSitePct"] is DBNull) && item["InSitePct"].ToString() != "")
                    {
                        pro.Material1 = Convert.ToDecimal(item["InSitePct"].ToString());
                    }
                    if (!(item["InSite"] is DBNull) && item["InSite"].ToString() != "")
                    {
                        pro.TotalTonnes1 = Convert.ToDecimal(item["InSite"].ToString());

                    }
                    pro.TypeList1 = "Available";
                    if (!(item["OutOfSitePct"] is DBNull) && item["OutOfSitePct"].ToString() != "")
                    {
                        pro.Material2 = Convert.ToDecimal(item["OutOfSitePct"].ToString());
                    }
                    if (!(item["OutOfSite"] is DBNull) && item["OutOfSite"].ToString() != "")
                    {
                        pro.TotalTonnes2 = Convert.ToDecimal(item["OutOfSite"].ToString());

                    }
                    pro.TypeList2 = "Standing";

                    if (!(item["OnSitePct"] is DBNull) && item["OnSitePct"].ToString() != "")
                    {
                        pro.Material3 = Convert.ToDecimal(item["OnSitePct"].ToString());
                    }
                    if (!(item["OnSite"] is DBNull) && item["OnSite"].ToString() != "")
                    {
                        pro.TotalTonnes3 = Convert.ToDecimal(item["OnSite"].ToString());

                    }
                    pro.TypeList3 = "On Site";
                    ItemUtilizeDashboardChart2.Add(pro);
                }
            }
      
            // return ItemUtilizeDashboardChart2;
            return Json(ItemUtilizeDashboardChart2, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetScaffoldItemBreakdown(string SiteId, string Type, string ClientId)
        {
            List<object> list = new List<object>();

            string query = "exec Pro_SiteInventory_Get "+ databaseCon.Decrypt(ClientId) + "," + SiteId + ",'All'";

            DataSet ds = databaseCon.GetDataSet(query);

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    decimal quantity = 0;
                    decimal itemWeight = Convert.ToDecimal(dr["ItemWeight"]);

                    if (Type == "Available")
                    {
                        quantity = Convert.ToDecimal(dr["AvailableQuantity"]);
                    }
                    else if (Type == "Standing")
                    {
                        quantity = Convert.ToDecimal(dr["QuantityToBeSent"]);
                    }
                    else if (Type == "On Site")
                    {
                        quantity = Convert.ToDecimal(dr["OnSite"]);
                    }
                    if (quantity == 0)
                    {
                        continue;
                    }
                    //if (quantity <= 0) continue;

                    decimal totalWeight = quantity * itemWeight;

                    list.Add(new
                    {
                        MaterialType = dr["MaterialType"].ToString(),
                        ItemCode = dr["ItemCode"].ToString(),
                        ItemDesc = dr["ItemDesc"].ToString(),
                        Quantity = quantity,
                        ItemWeight = itemWeight,
                        TotalWeight = Math.Round(totalWeight, 2)
                    });
                }
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // Item Utilization Dashboard On Scaffold Material ON Site Vs Available Vs Standing Chart 2 Start

        private string csvfile(DataTable dtTable)
        {
            StringBuilder sbldr = new StringBuilder();

            if (dtTable.Columns.Count != 0)
            {
                // Header
                for (int i = 0; i < dtTable.Columns.Count; i++)
                {
                    sbldr.Append(dtTable.Columns[i].ColumnName);
                    if (i < dtTable.Columns.Count - 1)
                        sbldr.Append(',');
                }
                sbldr.Append("\r\n");

                foreach (DataRow row in dtTable.Rows)
                {
                    for (int i = 0; i < dtTable.Columns.Count; i++)
                    {
                        string columnName = dtTable.Columns[i].ColumnName;
                        string value = row[i].ToString();

                        // 🔹 Remove leading zeros (only if numeric)
                        bool isNumeric = !string.IsNullOrEmpty(value) && value.All(char.IsDigit);
                        if (isNumeric)
                        {
                            value = int.Parse(value).ToString();
                        }

                        // 🔹 Special columns (keep your existing logic)
                        if (columnName == "ItemDesc" || columnName == "ItemWeight" || columnName == "Quantity")
                        {
                            if (value.Contains("\""))
                            {
                                value = value.Replace("\"", "\"\"");
                            }

                            if (value.Contains(",") || value.Contains("\""))
                            {
                                value = "\"" + value + "\"";
                            }

                            sbldr.Append(value);
                        }
                        else
                        {
                            // 🔹 For other columns

                            if (isNumeric)
                            {
                                // Keep numbers as numbers (no quotes)
                                sbldr.Append(value);
                            }
                            else
                            {
                                // Escape quotes
                                if (value.Contains("\""))
                                {
                                    value = value.Replace("\"", "\"\"");
                                }

                                // Wrap text in quotes
                                sbldr.Append("\"" + value + "\"");
                            }
                        }

                        if (i < dtTable.Columns.Count - 1)
                            sbldr.Append(',');
                    }

                    sbldr.Append("\r\n");
                }
            }

            return sbldr.ToString();
        }

    }
}