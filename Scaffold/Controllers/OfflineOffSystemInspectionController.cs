using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace Scaffold.Controllers
{
    public class OfflineOffSystemInspectionController : Controller
    {
        // GET: OfflineOffSystemInspection

        ScaffoldEntities dbcontext = new ScaffoldEntities();

        public ActionResult OfflineOffSystemInspection()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CheckOfflineClearedDate(string LoginUserId, string clientId, string Siteid)
        {
            string FinddataQuery = "select  isnull(max( OfflineClearedOn) ,'1900-01-01') as OfflineClearedOn from tblregister where OfflineCleared =1 and clientid  = " + databaseCon.Decrypt(clientId) + " and SiteLocId = " + databaseCon.Decrypt(Siteid) + "and DateOfDismantle = '1900-01-01'  and DateTagged != '1900-01-01'";
            var FinddataUpdatedBy = databaseCon.GetData(FinddataQuery);

            var OfflineClearedOn = FinddataUpdatedBy.Rows[0][0].ToString();

            DateTime offlineonDateTime = DateTime.Parse(OfflineClearedOn);
            string formattedOfflineon = offlineonDateTime.ToString("dd-MMM-yyyy h:mm:ss tt");
            return Json(new { formattedOfflineon = formattedOfflineon });
        }
        public async Task<JsonResult> GetAllDataFromServerDB(int clientId ,string Siteid)
        {
            try
            {
                dbcontext.Database.CommandTimeout = 180;

                var tblSiteLocationData = await dbcontext.Database.SqlQuery<tblSiteLocation>(
                    "SELECT distinct SiteLocID,SiteLocation,ClientId FROM tblSiteLocation WHERE SiteLocationStatus='Active' and OffSystemInspection = 1 and  ClientID = @clientId and SiteLocID = @SiteLocID",
                    new SqlParameter("@clientId", clientId),
                    new SqlParameter("@SiteLocID", databaseCon.Decrypt(Siteid))
                ).ToListAsync();
                // var tblSiteLocationData = await dbcontext.Database.SqlQuery<tblSiteLocation>(
                //    "SELECT distinct SiteLocID,SiteLocation,ClientId FROM tblSiteLocation WHERE SiteLocationStatus='Active' and OffSystemInspection = 1 and  ClientID = @clientId",
                //    new SqlParameter("@clientId", clientId)
                //).ToListAsync();

                var tblAreaData = await dbcontext.Database.SqlQuery<tblArea>(
                    "SELECT distinct Area,AreaId,AreaStatus,ClientID,SiteLocId FROM tblArea WHERE AreaStatus='Active' and ClientID = @clientId and SiteLocId = @SiteLocId",
                    new SqlParameter("@clientId", clientId),
                    new SqlParameter("@SiteLocID", databaseCon.Decrypt(Siteid))

                ).ToListAsync();

                var tblBuildingData = await dbcontext.Database.SqlQuery<tblBuilding>(
                    "SELECT distinct BuildingNo,BuildingId,AreaId,BuildingStatus,ClientID FROM tblBuilding WHERE BuildingStatus='Active' and  ClientID = @clientId",
                    new SqlParameter("@clientId", clientId)
                ).ToListAsync();

                //var tblFloorLevelData = await dbcontext.Database.SqlQuery<TblFloorLevelModel>(
                //    "SELECT * FROM TblFloorLevel WHERE ClientID = @clientId and SiteLocId = @SiteLocId",

                var tblFloorLevelData = await dbcontext.Database.SqlQuery<TblFloorLevelModel>(
                    "SELECT distinct VW_TblRegister.FloorLevelId,VW_TblRegister.floorlevel,VW_TblRegister.BuildingId,VW_TblRegister.LocationId,Category,VW_TblRegister.SiteLocId,VW_TblRegister.ClientID FROM VW_TblRegister join TblFloorLevel on VW_TblRegister.FloorLevelId = TblFloorLevel.FloorLevelId where FloorLevelStatus = 'Active' and  VW_TblRegister.ClientID = @clientId and VW_TblRegister.SiteLocId = @SiteLocId",
                    new SqlParameter("@clientId", clientId),
                    new SqlParameter("@SiteLocID", databaseCon.Decrypt(Siteid))

                ).ToListAsync();

                //var tblRegisterData = await dbcontext.Database.SqlQuery<offlineregisterviewModel>(
                //    "SELECT * FROM tblRegister WHERE ClientID = @clientId and SiteLocId = @Siteid and IsOfflineCurrentForInspection != 0 and DateOfDismantle = '1900-01-01'  and DateTagged != '1900-01-01'",
                //    new SqlParameter("@clientId", clientId ),
                //    new SqlParameter("@Siteid", databaseCon.Decrypt( Siteid))
                //).ToListAsync();
                var offset = 0;
                var tblRegisterData = await dbcontext.Database.SqlQuery<offlineregisterviewModel>(
    "EXEC Pro_OfflineTBLREGISTER_GET @clientId, @siteId, @offset",
    new SqlParameter("@clientId", clientId),
    new SqlParameter("@siteId", databaseCon.Decrypt(Siteid)),
    new SqlParameter("@offset", offset)
).ToListAsync();
                var Tbluserdata = await dbcontext.Database.SqlQuery<tbluser>(
                    "select distinct firstName+ ' '+LastName as Name,UserId,LicenceNo,IsInspector from tblUser where IsInspector='true' and Status='Active' and clientid=@clientId Order by name",
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


        [HttpPost]
        public ActionResult UploadData(List<InspectionModel> offlineDataArray ,string LoginUserId, string clientId ,string Siteid)
        {

            string CheckIsDatasillInOfflineMode = "select  RegisterId from tblRegister where ClientID =  "+databaseCon.Decrypt(clientId)+" and SiteLocId = " + databaseCon.Decrypt(Siteid) + " and IsOfflineCurrentForInspection = 1  and GoOfflineBy = (select LoginId from tblUser where UserId = " + databaseCon.Decrypt(LoginUserId) + ") and DateOfDismantle = '1900-01-01'  and DateTagged != '1900-01-01'";
            DataTable CheckIsDatasillInOfflineModeData = databaseCon.GetData(CheckIsDatasillInOfflineMode);

            if (CheckIsDatasillInOfflineModeData.Rows.Count > 0)
            {

                try
                {
                    if (offlineDataArray == null || !offlineDataArray.Any())
                    {
                        return Json(new { Result = "Failed", Message = "No data to upload" });
                    }
                    string Userinfor = "select LoginId from tblUser where UserId = " + databaseCon.Decrypt(LoginUserId) + "";

                    DataTable LoginuserInfo = databaseCon.GetData(Userinfor);

                    foreach (var item in offlineDataArray)
                    {

                        DateTime defaultDate = new DateTime(1, 1, 1);

                        var InspectionDate = item.InspectionDate;

                        var InspectedBy = item.InspectedBy;
                        var Comments = item.Comments;
                        var RegisterID = item.RegisterID;
                        var CreatedBy = LoginuserInfo.Rows[0][0].ToString();
                        //DateTime? createdOn = item.CreatedOn == defaultDate ? (DateTime?)null : item.CreatedOn;
                        var createdOn = DateTime.Now;
                        var OnlineInsp = item.OnlineInsp ? 1 : 0;
                        var CompleteInsp = item.CompleteInsp ? 1 : 0;
                        var InspComments = item.InspComments;
                        var Acknowledgment = item.Acknowledgment;
                        var InspStatus = item.InspStatus;
                        var AckDate = item.AckDate;
                        var LicenceNo = item.LicenceNo;
                        var InspectedById = item.InspectedById;
                        var IsOffline = item.IsOffline ? 1 : 0;
                        var UserId = item.UserId;
                        var OffSystemInspection = item.OffSystemInspection ? 1 : 0;


                        string query = "EXEC Pro_InsertIntoOfflineInspection  '" + InspectionDate + "','" + InspectedBy + "','" + Comments + "'," + RegisterID + " ,'" + CreatedBy + "','" + createdOn + "' ," + OnlineInsp + "," + CompleteInsp + ",'" + InspComments + "' ," + Acknowledgment + " , '" + InspStatus + "','" + AckDate + "', '" + LicenceNo + "'," + InspectedById + " ," + IsOffline + " ," + UserId + " , " + OffSystemInspection + "";

                        var Confirmdt = databaseCon.GetData(query);

                        //string Userinfor = "select  LoginId from tblUser where UserId = " + databaseCon.Decrypt(LoginUserId) + "";


                        // DataTable LofinuserInfo = databaseCon.GetData(Userinfor);


                        //string updateRegisterquery = "update tblRegister set IsOfflineCurrentForInspection = 0 ,GoOfflineOn = GETDATE(), GoOfflineBy = '" + LofinuserInfo.Rows[0][0].ToString() +"' where ClientID =  " + databaseCon.Decrypt(clientId) + " and SiteLocId = "+databaseCon.Decrypt(Siteid) +"";
                        //var res = databaseCon.RunCmd(updateRegisterquery);

                    }
                    return Json(new { Result = "Success", Message = "Data uploaded successfully" });
                }
                catch (Exception ex)
                {
                    return Json(new { Result = "Failed", Message = ex.Message });
                }
            }
            else
            {
                string FinddataQuery = "select  max(OfflineClearedBy) as OfflineClearedBy ,max(OfflineClearedOn) as OfflineClearedOn from tblregister where OfflineCleared =1 and clientid  = " + databaseCon.Decrypt(clientId) + " and SiteLocId = " + databaseCon.Decrypt(Siteid) + " and DateOfDismantle = '1900-01-01'  and DateTagged != '1900-01-01'";
                var FinddataUpdatedBy = databaseCon.GetData(FinddataQuery);

                var offlineBy = FinddataUpdatedBy.Rows[0][0].ToString();
                var offlineon = FinddataUpdatedBy.Rows[0][1].ToString();

                DateTime offlineonDateTime = DateTime.Parse(offlineon);
                string formattedOfflineon = offlineonDateTime.ToString("dd-MMM-yyyy h:mm:ss tt");
                return Json(new
                {
                    Result = "Failed",
                    Message = "ON LAST SESSION YOU DID NOT UPLOAD SAVED OFFLINE INSPECTIONS TO THE CLOUD! LAST SESSION DATA NOW CANCELLED. \n\nFor a copy of the list of inspections: Contact Scafpac Admin at: admin@scafpac.com.au"
                });
            }
        }
        

        [HttpPost]
        public ActionResult UpdateOfflineInspectionFlag(string LoginUserId, string clientId, string Siteid)
        {
            string CheckIsDatasillInOfflineMode = "select  RegisterId from tblRegister where ClientID =  " + databaseCon.Decrypt(clientId) + " and SiteLocId = " + databaseCon.Decrypt(Siteid) + " and IsOfflineCurrentForInspection = 1 and DateOfDismantle = '1900-01-01'  and DateTagged != '1900-01-01'";
            DataTable CheckIsDatasillInOfflineModeData = databaseCon.GetData(CheckIsDatasillInOfflineMode);

            if (CheckIsDatasillInOfflineModeData.Rows.Count > 0)
            {

                try
                {
                    string Userinfor = "select LoginId from tblUser where UserId = " + databaseCon.Decrypt(LoginUserId) + "";

                    DataTable LoginuserInfo = databaseCon.GetData(Userinfor);

                    string updateRegisterquery = "update tblRegister set IsOfflineCurrentForInspection = 0  where  DateOfDismantle = '1900-01-01'  and DateTagged != '1900-01-01'and ClientID =  " + databaseCon.Decrypt(clientId) + " and SiteLocId = " + databaseCon.Decrypt(Siteid) + " and GoOfflineBy = '" + LoginuserInfo.Rows[0][0].ToString() + "'";

                    var res = databaseCon.RunCmd(updateRegisterquery);

                    string maxTrackOfflineId = "select max(TrackOfflineId) as TrackOfflineId from tblTrackOffline where ClientId = " + databaseCon.Decrypt(clientId) + " and SiteId = " + databaseCon.Decrypt(Siteid) + "";
                    DataTable TrackOfflineId = databaseCon.GetData(maxTrackOfflineId);

                    string goOnlineline = "Update tblTrackOffline set Action ='Online', GoOnlineOn = GetDate(), GoOnlineBy = '" + LoginuserInfo.Rows[0][0].ToString() + "' where ClientID =  " + databaseCon.Decrypt(clientId) + " and SiteId = " + databaseCon.Decrypt(Siteid) + " and GoOnlineOn IS NULL and GoOnlineBy IS NULL and TrackOfflineId = " + TrackOfflineId.Rows[0][0] + "";
                    var OfflineEntry = databaseCon.GetData(goOnlineline);
                    return Json(new { Result = "Success", Message = "Inspection flag updated successfully" });
                }
                catch (Exception ex)
                {
                    return Json(new { Result = "Failed", Message = ex.Message });
                }
            }
            else
            {
                string FinddataQuery = "select max(OfflineClearedBy) as OfflineClearedBy ,max(OfflineClearedOn) as OfflineClearedOn from tblregister where OfflineCleared =1 and clientid  = " + databaseCon.Decrypt(clientId) + " and SiteLocId = " + databaseCon.Decrypt(Siteid) + "and DateOfDismantle = '1900-01-01'  and DateTagged != '1900-01-01'";
                var FinddataUpdatedBy = databaseCon.GetData(FinddataQuery);

                var offlineBy = FinddataUpdatedBy.Rows[0][0].ToString();
                var offlineon = FinddataUpdatedBy.Rows[0][1].ToString();

                DateTime offlineonDateTime = DateTime.Parse(offlineon);
                string formattedOfflineon = offlineonDateTime.ToString("dd-MMM-yyyy h:mm:ss tt");
                //return Json(new { Result = "AlreadyMoved", Message = "This inspection session has been voided due to time lapse.  Voided by Scafpac Admin: "+ formattedOfflineon + "." });
                return Json(new { Result = "AlreadyMoved", Message = "ON LAST SESSION YOU DID NOT UPLOAD SAVED OFFLINE INSPECTIONS TO THE CLOUD! LAST SESSION DATA NOW CANCELLED. \n\nFor a copy of the list of inspections: Contact Scafpac Admin at: admin@scafpac.com.au ." });

            }
        }
        public string GetIPAddress()
        {
            string ipAddress = "";
            var currentRequest = System.Web.HttpContext.Current.Request;

            // Check HTTP_X_FORWARDED_FOR for proxy usage
            ipAddress = currentRequest.ServerVariables["HTTP_X_FORWARDED_FOR"];

            // Fallback to REMOTE_ADDR if HTTP_X_FORWARDED_FOR is null or empty
            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = currentRequest.ServerVariables["REMOTE_ADDR"];
            }

            // Check if the IP is local (localhost IPv6 address "::1" or IPv4 "127.0.0.1")
            if (ipAddress == "::1" || ipAddress == "127.0.0.1")
            {
                // Determine whether the app is running locally or on a server
                string host = currentRequest.Url.Host;

                if (host == "localhost" || host.Contains("127.0.0.1"))
                {
                    // Running locally, so set a dummy external IP for testing
                    ipAddress = "115.110.119.138"; // Replace with a test IP if needed
                }
                else
                {
                    // Running on a server, use the server-assigned IP
                    ipAddress = currentRequest.ServerVariables["REMOTE_ADDR"];
                }
            }

            return ipAddress;
        }

        [HttpPost]
        public ActionResult BackupOldInspectionData(List<InspectionModel> offlineDataArray, string LoginUserId, string clientId, string Siteid)
        {

            try
            {
                string Userinfor = "select LoginId from tblUser where UserId = " + databaseCon.Decrypt(LoginUserId) + "";

                DataTable LoginuserInfo = databaseCon.GetData(Userinfor);
                var CreatedOn = DateTime.Now;
                var LoginUerInfo = LoginuserInfo.Rows[0][0].ToString();
                string ipAddress = GetIPAddress();

                foreach (var item in offlineDataArray)
                {

                    DateTime defaultDate = new DateTime(1, 1, 1);

                    var InspectionDate = item.InspectionDate;

                    var InspectedBy = item.InspectedBy;
                    var Comments = item.Comments;
                    var RegisterID = item.RegisterID;
                    //DateTime? createdOn = item.CreatedOn == defaultDate ? (DateTime?)null : item.CreatedOn;
                    //var createdOn = DateTime.Now;
                    var OnlineInsp = item.OnlineInsp ? 1 : 0;
                    var CompleteInsp = item.CompleteInsp ? 1 : 0;
                    var InspComments = item.InspComments;
                    var Acknowledgment = item.Acknowledgment;
                    var InspStatus = item.InspStatus;
                    var AckDate = item.AckDate;
                    var LicenceNo = item.LicenceNo;
                    var InspectedById = item.InspectedById;
                    var IsOffline = item.IsOffline ? 1 : 0;
                    var UserId = item.UserId;
                    var OffSystemInspection = item.OffSystemInspection ? 1 : 0;
                    var OfflineInspStatus = "";
                    if (item.InspStatus == "Confirmed")
                    {
                        OfflineInspStatus = "Green";
                    }
                    else
                    {
                        OfflineInspStatus = item.InspStatus;
                    }

                    string query = "EXEC [Pro_InsertIntoOfflineInspection_NotUploadedTocloud]  '" + InspectionDate + "','" + InspectedBy + "','" + Comments + "'," + RegisterID + " ,'" + InspectedBy + "','" + InspectionDate + "' ," + OnlineInsp + "," + CompleteInsp + ",'" + InspComments + "' ," + Acknowledgment + " , '" + InspStatus + "','" + AckDate + "', '" + LicenceNo + "'," + InspectedById + " ," + IsOffline + " ," + UserId + " , " + OffSystemInspection + ",'" + OfflineInspStatus + "','" + ipAddress + "','" + LoginUerInfo + "','" + CreatedOn + "'";

                    var Confirmdt = databaseCon.GetData(query);
                    

                }

                return Json(new { Result = "Success", Message = "Offline inspection data successfully backed up." });

            }

            catch (Exception ex)
            {
                return Json(new { Result = "Failed", Message = $"An error occurred: {ex.Message}" });
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