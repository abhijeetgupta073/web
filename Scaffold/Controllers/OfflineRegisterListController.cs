using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Scaffold.Controllers
{
    public class OfflineRegisterListController : Controller
    {

        ScaffoldEntities dbcontext = new ScaffoldEntities();
        public ActionResult OfflineRegisterList()
        {
            return View();
        }
        public string GetDecryptClientID(string id)
        {
            return (id);
        }
        public async Task<JsonResult> GetAllDataFromServerDB(int clientId, string Siteid)
        {
            try
            {
                dbcontext.Database.CommandTimeout = 180;

                var tblSiteLocationData = await dbcontext.Database.SqlQuery<tblSiteLocation>(
                    "SELECT SiteLocID,SiteLocation,ClientId FROM tblSiteLocation WHERE OffSystemInspection = 1 and  ClientID = @clientId and SiteLocID = @SiteLocID",
                    new SqlParameter("@clientId", clientId),
                    new SqlParameter("@SiteLocID", databaseCon.Decrypt(Siteid))
                ).ToListAsync();

                var tblAreaData = await dbcontext.Database.SqlQuery<tblArea>(
                    "SELECT * FROM tblArea WHERE ClientID = @clientId and SiteLocId = @SiteLocId",
                    new SqlParameter("@clientId", clientId),
                    new SqlParameter("@SiteLocID", databaseCon.Decrypt(Siteid))

                ).ToListAsync();

                var tblBuildingData = await dbcontext.Database.SqlQuery<tblBuilding>(
                    "SELECT * FROM tblBuilding WHERE ClientID = @clientId",
                    new SqlParameter("@clientId", clientId)
                ).ToListAsync();

                var tblFloorLevelData = await dbcontext.Database.SqlQuery<TblFloorLevelModel>(
                    "SELECT * FROM TblFloorLevel WHERE ClientID = @clientId and SiteLocId = @SiteLocId",
                    new SqlParameter("@clientId", clientId),
                    new SqlParameter("@SiteLocID", databaseCon.Decrypt(Siteid))

                ).ToListAsync();

                var tblRegisterData = await dbcontext.Database.SqlQuery<offlineregisterviewModel>(
                    "SELECT * FROM tblRegister WHERE ClientID = @clientId and SiteLocId = @Siteid and IsOfflineCurrent != 0 and DateOfDismantle = '1900-01-01'  and DateTagged != '1900-01-01'",
                    new SqlParameter("@clientId", clientId),
                    new SqlParameter("@Siteid", databaseCon.Decrypt(Siteid))
                ).ToListAsync();
                var Tbluserdata = await dbcontext.Database.SqlQuery<tbluser>(
                    "select distinct firstName+ ' '+LastName as Name,UserId,LicenceNo from tblUser where IsInspector='true' and Status='Active' and clientid=@clientId Order by name",
                    new SqlParameter("@clientId", clientId)
                ).ToListAsync();

                return Json(new
                {
                    tblsitelocationdata = tblSiteLocationData,
                    tblAreadata = tblAreaData,
                    tblbuildingdata = tblBuildingData,
                    tblfloorleveldata = tblFloorLevelData,
                    tblregisterdata = tblRegisterData,
                    Tbluserdata = Tbluserdata
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.WriteLine("Error: " + ex.Message);
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }


        }


        public class TblFloorLevelModel
        {
            public int FloorLevelId { get; set; }
            public string Category { get; set; }
            public string FloorLevel { get; set; }
            public int? ClientID { get; set; }
            public int? SiteLocId { get; set; }
            //public string FloorLevelStatus { get; set; }
            //public int? SortOrder { get; set; }
            //public string CreatedBy { get; set; }
            //public DateTime? CreatedOn { get; set; }
            //public string UpdatedBy { get; set; }
            //public DateTime? UpdatedOn { get; set; }
            public int? LocationID { get; set; }
            public int? BuildingID { get; set; }
        }
        public class tblSiteLocation
        {
            public int SiteLocID { get; set; }
            public string SiteLocation { get; set; }
            public int ClientId { get; set; }
            //public string CreatedBy { get; set; }
            //public DateTime? CreatedOn { get; set; }
            //public string UpdatedBy { get; set; }
            //public DateTime? UpdatedOn { get; set; }
            //public string Address { get; set; }
            //public string City { get; set; }
            //public string State { get; set; }
            //public string Zip { get; set; }
            //public string SiteLocationStatus { get; set; }
            //public string OwnerName { get; set; }
            //public string DistributionList { get; set; }
            //public string Country { get; set; }
            //public string SiteLat { get; set; }
            //public string SiteLong { get; set; }
            //public decimal? ManHoursAvailable { get; set; }
            //public bool MaintainInventory { get; set; }
            //public bool CycloneManagement { get; set; }
            //public bool ManHoursManagement { get; set; }
            //public bool MaintainInventoryAtJob { get; set; }
            //public bool GPS { get; set; }
            //public bool FloorLevel { get; set; }
            //public bool EnabledGPSSitePin { get; set; }
            //public bool EnabledCaptureGps { get; set; }
            //public bool OffSystemInspection { get; set; }
            //public string InventoryMailingList { get; set; }
            //public bool SkipMaterial { get; set; }
            //public bool PipeLineInvHide { get; set; }
        }

        public class tblArea
        {
            public int AreaID { get; set; }

            public string Area { get; set; }

            public int? ClientID { get; set; }



            public string AreaStatus { get; set; }

            public int? SiteLocId { get; set; }
        }

        public class tblBuilding
        {
            public int BuildingID { get; set; }

            public string BuildingNo { get; set; }

            public string BuildingStatus { get; set; }

            public int ClientId { get; set; }

            public int AreaId { get; set; }




        }
        public class tbluser
        {
            public int UserId { get; set; }

            public string Name { get; set; }

            public string LicenceNo { get; set; }






        }
        public class InspectionModel
        {
            public int InspectionID { get; set; }
            public DateTime InspectionDate { get; set; }
            public string InspectedBy { get; set; }
            public string Comments { get; set; }
            public int RegisterID { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedOn { get; set; }
            public string UpdatedBy { get; set; }
            public DateTime UpdatedOn { get; set; }
            public bool OnlineInsp { get; set; }
            public bool CompleteInsp { get; set; }
            public string InspComments { get; set; }
            public bool Acknowledgment { get; set; }
            public string InspStatus { get; set; }
            public DateTime? AckDate { get; set; }
            public string LicenceNo { get; set; }
            public int InspectedById { get; set; }
            public bool IsOffline { get; set; }
            public int UserId { get; set; }
            public bool OffSystemInspection { get; set; }
        }

    }
}