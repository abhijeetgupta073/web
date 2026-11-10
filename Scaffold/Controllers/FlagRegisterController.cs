using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scaffold.Controllers
{
    public class FlagRegisterController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: FlagRegister
        public ActionResult FlagRegister(string UID, string UserRole, string id)
        {
            return View();
        }

        public ActionResult GetFlagRegisterData(string id, String TrueOrFalse)
        {
            var setTrueOrfalse = "0";
            if (TrueOrFalse == null) { setTrueOrfalse = "0"; }
            else if (TrueOrFalse == "0") { setTrueOrfalse = "0"; }
            else { setTrueOrfalse = "1"; }

           //string query = "select VW_TblRegister.RegisterId, Right('000000'+Convert(varchar,scaffref),6) as Scaffref,Location,BuildingNo,AssetNo,VW_TblRegister.FloorLevel,[Description],Bearing, Case when[Image]!= '' and[Image] is not null then 'Y' else 'N' end as Photo, Case when ManHrsErect <> 0 and ManHrsErect is not null then 'Y' else 'N' end as ManHrsErect, Case when ManHrsDismantle <> 0 and ManHrsDismantle is not null then 'Y' else 'N' end as ManHrsDismantle, case when Plans = 'Plan1' then(case when Plan1MaterialCount > 0 then 'Y' else 'N' end) when Plans = 'Plan2' then(case when Plan2MaterialCount > 0 then 'Y' else 'N' end) else NULL end as JobInventoryList , case when Plans = 'Plan1' then 'N' when Plans = 'Plan2' then(case when Plan2FinalInvCount > 0 then 'Y' else 'N' end) else NULL end as FinalMaterialList, Case when[LEngth]<> 0 and[LEngth] is not null then 'Y' else 'N' end as [Length], Case when Width <> 0 and Width is not null then 'Y' else 'N' end as Width, Case when Height <> 0 and Height is not null then 'Y' else 'N' end as Height, Case when CycloneAction != '' and CycloneAction is not null then 'Y' else 'N' end as CycloneAction, Case when UseEnteredValue = 0 then Case when isnull(TotalLabourDismantle,0)> 0  then 'Y' else 'N' end when UseEnteredValue = 1 then Case when isnull(TotalLabourDismantleEdit,0)> 0 then 'Y' else 'N' end else 'N' End as TotalDeBoardHrs, Case when UseEnteredValue = 0 then Case when isnull(TotalLabourErect,0)> 0  then 'Y' else 'N' end when UseEnteredValue = 1 then Case when isnull(TotalLabourErectEdit,0)> 0 then 'Y' else 'N' end else 'N' End as TotalReinstateHrs,Case when Lat != ''And Long != '' and Lat is not null and Long is not null then 'Y' else 'N' end as RegisterPin,Case when SiteLat != ''And SiteLong != '' and SiteLat is not null and SiteLong is not null then 'Y' else 'N' end as SitePin,tblSiteLocation.SiteLocation, datetagged,dateofdismantle,discipline,EngineeredDesign,ClientRef from VW_TblRegister join tblClient on VW_TblRegister.clientID = tblClient.clientId left join (select registerID,Count(*) as Plan1MaterialCount from tblSiteMaterial group by registerID)Plan1MaterialList on  Plan1MaterialList.registerID = VW_TblRegister.RegisterId left join (select ToId as registerID, Count(*) as Plan2MaterialCount from tblTransactionHistDelivery where [From] = 'Site' and[To] = 'Job' group by ToId)Plan2MaterialList On Plan2MaterialList.registerID = VW_TblRegister.RegisterId left join (select registerID, Count(*) as Plan2FinalInvCount from tblOptimizedInventory group by registerID)Plan2FinalInventory on  Plan2FinalInventory.registerID = VW_TblRegister.RegisterId left join tblSiteLocation on tblSiteLocation.SiteLocID=VW_TblRegister.SiteLocId where tblClient.clientID = '" + databaseCon.Decrypt(id)+ "' and (datetagged is not null and datetagged!='01/01/1900') and  ((dateofdismantle='01/01/1900' or dateofdismantle is null)  or 1= "+ setTrueOrfalse + ")";
            string query = "select VW_TblRegister.RegisterId, Right('000000'+Convert(varchar,scaffref),6) as Scaffref,Workgroup,Location,BuildingNo,AssetNo,VW_TblRegister.FloorLevel,[Description],Bearing, Case when[Image]!= '' and[Image] is not null then 'Y' else 'N' end as Photo, Case when ManHrsErect <> 0 and ManHrsErect is not null then 'Y' else 'N' end as ManHrsErect, Case when ManHrsDismantle <> 0 and ManHrsDismantle is not null then 'Y' else 'N' end as ManHrsDismantle, case when Plans = 'Plan1' then(case when Plan1MaterialCount > 0 then 'Y' else 'N' end) when Plans = 'Plan2' then(case when Plan2MaterialCount > 0 then 'Y' else 'N' end) else NULL end as JobInventoryList , case when Plans = 'Plan1' then 'N' when Plans = 'Plan2' then(case when Plan2FinalInvCount > 0 then 'Y' else 'N' end) else NULL end as FinalMaterialList, Case when[LEngth]<> 0 and[LEngth] is not null then 'Y' else 'N' end as [Length], Case when Width <> 0 and Width is not null then 'Y' else 'N' end as Width, Case when Height <> 0 and Height is not null then 'Y' else 'N' end as Height, Case when CycloneAction != '' and CycloneAction is not null then 'Y' else 'N' end as CycloneAction, Case when UseEnteredValue = 0 then Case when isnull(TotalLabourDismantle,0)> 0  then 'Y' else 'N' end when UseEnteredValue = 1 then Case when isnull(TotalLabourDismantleEdit,0)> 0 then 'Y' else 'N' end else 'N' End as TotalDeBoardHrs, Case when UseEnteredValue = 0 then Case when isnull(TotalLabourErect,0)> 0  then 'Y' else 'N' end when UseEnteredValue = 1 then Case when isnull(TotalLabourErectEdit,0)> 0 then 'Y' else 'N' end else 'N' End as TotalReinstateHrs,Case when Lat != ''And Long != '' and Lat is not null and Long is not null then 'Y' else 'N' end as RegisterPin,Case when SiteLat != ''And SiteLong != '' and SiteLat is not null and SiteLong is not null then 'Y' else 'N' end as SitePin,tblSiteLocation.SiteLocation, datetagged,dateofdismantle,discipline,EngineeredDesign,ClientRef from VW_TblRegister join tblClient on VW_TblRegister.clientID = tblClient.clientId left join (select registerID,Count(*) as Plan1MaterialCount from tblSiteMaterial group by registerID)Plan1MaterialList on  Plan1MaterialList.registerID = VW_TblRegister.RegisterId left join (select ToId as registerID, Count(*) as Plan2MaterialCount from tblTransactionHistDelivery where [From] = 'Site' and[To] = 'Job' group by ToId)Plan2MaterialList On Plan2MaterialList.registerID = VW_TblRegister.RegisterId left join (select registerID, Count(*) as Plan2FinalInvCount from tblOptimizedInventory group by registerID)Plan2FinalInventory on  Plan2FinalInventory.registerID = VW_TblRegister.RegisterId left join tblSiteLocation on tblSiteLocation.SiteLocID=VW_TblRegister.SiteLocId where tblClient.clientID = '" + databaseCon.Decrypt(id) + "' and (datetagged is not null and datetagged!='01/01/1900') and  ((dateofdismantle='01/01/1900' or dateofdismantle is null)  or 1= " + setTrueOrfalse + ")";
            //DataTable dt = databaseCon.GetData(query);
            //GridFlagRegister.DataSource = dt;
            // "+ databaseCon.Decrypt(id) +"
            var getData = dbContext.Database.SqlQuery<FlagRegisterViewModel>(query).ToList();

            //return Json(getData, JsonRequestBehavior.AllowGet);
			var jsonResult = Json(getData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }

    }
}