using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scaffold.App_Code;
using Newtonsoft.Json;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.Ajax.Utilities;
using DocumentFormat.OpenXml.Spreadsheet;
using Scaffold_DataAccess;
using System.Web.Services.Description;
using System.Data.SqlClient;

namespace Scaffold.Controllers
{
    public class OfflineRegisterController : Controller
    {
        // GET: OfflineRegister
        public ActionResult Index()
        {
            return View();
        }
        public class Scaffold
        {
            public decimal ManHrsRatio;
            // public string scaffoldowner;

        }

        public class ddlsites
        {

            public int SiteLocID;
            public string SiteLocation;
            public string OwnerName;
            public string Address;
            public string city;
            public string Zip;
            public string state;
            public bool MaintainInventoryAtJob;
            public string FloorLevel;
            public string GPS;
            public bool CycloneManagement;
            public bool PipeLineInvHide;


            public List<ddlLocation> GetLocationList;
        }

        public class ddlScaffoldDesign
        {
            public string DesignType; public int ClientID;
        }

        //Add code on 27-Jan-2020.
        public class ddlFloorLevel
        {
            public string Category;
            public string FloorLevel;
            public int ClientID;
            public int SiteLocId;
            public string FloorLevelStatus;
            public string SortOrder;
            // Add on 13 Aug 2020
            public int FloorLevelId;
            public int LocationID;
            public int BuildingID;

        }
        public class ddlDutyrating
        {
            public string DutyRating;
            public int ClientID;
        }

        public class ddlMaterialType
        {
            public string MaterialType;
            public int ClientID;

        }

        public class ChecklistDropdown
        {
            public int checklistID;
            public string checklistdescription;
            public int ClientID;

        }
        public class SupervisorDropdown
        {
            public int UserID;
            public string Supervisor;
            public int ClientID;

        }
        public class ScaffoldOwnerDropdown
        {
            public string ScaffoldOwner;
        }
        public class SubOwnerRepDropdown
        {
            public string subOwnerRep;
        }
        public class WorkGroupsDropdown
        {
            public string WorkGroups;
            public string ClientID { get; set; }
            public string SiteLocID { get; set; }
            public string WorkGroupID { get; set; }
        }
        public class SubGroupsDropdown
        {
            public string SubGroupId { get; set; }
            public string SubGroupName { get; set; }
            public string ClientID { get; set; }
            public string SiteLocID { get; set; }
            public string WorkGroupId { get; set; }
        }
        public class RequestedByDropdown
        {
            public string RequestBy;
            public string ClientID { get; set; }
            public string SiteLocID { get; set; }
            public string RequestById { get; set; }
        }
        public class RegisterListdropdown
        {
            public int REGISTERId { get; set; }
            public string ShowRegisterID { get; set; }
            public string SiteLocation { get; set; }
            public Nullable<int> SiteLocID { get; set; }
            public string SiteLocationStatus { get; set; }
            public string Description { get; set; }
            public Nullable<int> LocationId { get; set; }
            public string Location { get; set; }
            public string LocationStatus { get; set; }
            public Nullable<int> ClientID { get; set; }
            public string InspectedBy { get; set; }
            public string DesignType { get; set; }
            public string DutyRating { get; set; }
            public string MaterialType { get; set; }
            public string DateTagged { get; set; }
            public string DateTaggedTXT { get; set; }
            public Nullable<System.DateTime> DateOFDismantle { get; set; }
            public Nullable<int> HireDaysToDate { get; set; }
            public Nullable<int> TotalHireDays { get; set; }
            public string RecentInspDate { get; set; }
            public string DaysSinceLastInsp { get; set; }
            public string Status { get; set; }
            public Nullable<System.DateTime> ExpectedStartDate { get; set; }
            public string Image { get; set; }
            public string ClientRef { get; set; }
            public string InspTimeframe { get; set; }
            public string OwnerName { get; set; }
            public string NoOfItems { get; set; }
            public Nullable<decimal> Tonnage { get; set; }
            public string PhotoAngle { get; set; }
            public Nullable<int> BuildingID { get; set; }
            public string BuildingNo { get; set; }
            public string BuildingStatus { get; set; }
            public Nullable<int> AssetId { get; set; }
            public string AssetNo { get; set; }
            public string AssetDesc { get; set; }
            public string AssetStatus { get; set; }
            public Nullable<double> Length { get; set; }
            public Nullable<double> Height { get; set; }
            public Nullable<double> Width { get; set; }
            public string NoOfWorkingDecks { get; set; }
            public string FloorLevel { get; set; }
            public Nullable<bool> OnlineApplied { get; set; }
            public Nullable<int> CheckListId { get; set; }
            public string checklistdescription { get; set; }
            public string PlanningStatus { get; set; }
            public string WONumber { get; set; }
            public string ClientScaffref { get; set; }
            public string ScafType { get; set; }
            public string Lat { get; set; }
            public string Long { get; set; }
            public Nullable<bool> LockedIn { get; set; }
            public string OptimizeStatus { get; set; }
            public Nullable<int> PlannedStandingDuration { get; set; }
            public Nullable<int> CrewSize { get; set; }
            public Nullable<decimal> ManHrsErect { get; set; }
            public Nullable<decimal> ManHrsDismantle { get; set; }
            public Nullable<decimal> TotalLabourErect { get; set; }
            public Nullable<decimal> TotalLabourDismantle { get; set; }
            public Nullable<decimal> TotalErectCount { get; set; }
            public Nullable<decimal> TotalDismantleCount { get; set; }
            public Nullable<decimal> TotalLabourErectEdit { get; set; }
            public Nullable<decimal> TotalLabourDismantleEdit { get; set; }
            public Nullable<decimal> TotalErectCountEdit { get; set; }
            public Nullable<decimal> TotalDismantleCountEdit { get; set; }
            public Nullable<bool> UseEnteredValue { get; set; }
            public string CycloneAction { get; set; }
            public string Supervisor { get; set; }
            public string Crew { get; set; }
            public string PermitNo { get; set; }
            public string PriorityRiskRating { get; set; }
            public Nullable<int> SupervisorId { get; set; }
            public string Comments { get; set; }
            public string CreatedBy { get; set; }
            public Nullable<System.DateTime> CreatedOn { get; set; }
            public string UpdatedBy { get; set; }
            public Nullable<System.DateTime> UpdatedOn { get; set; }
            public string SAPNumber { get; set; }
            public string ScaffoldOwner { get; set; }
            public Nullable<System.DateTime> DismantleApprovedDate { get; set; }
            public string DismantleApprovedNotes { get; set; }
            public string Bearing { get; set; }
            public string CycloneComment { get; set; }
            public string HierarchyLevel { get; set; }
            public string HierarchyComment1 { get; set; }
            public string HierarchyComment2 { get; set; }
            public string HierarchyComment3 { get; set; }
            public string HierarchyComment4 { get; set; }
            public string HierarchyComment5 { get; set; }
            public Nullable<int> HierarchySignedOff { get; set; }
            public Nullable<int> LockedInSignedOff { get; set; }
            public string LockedInSignedOffName { get; set; }
            public string QRCodeImage { get; set; }
            public Nullable<bool> MaintainInventory { get; set; }
            public string DesignDrawingNo { get; set; }
            public string NumberOfPlatform { get; set; }
            public string EngineeredDesign { get; set; }
            public string discipline { get; set; }
            public string ShiftAllocation { get; set; }

            // Extra Column Add
            public string Plans { get; set; }
            public int InventoryCount { get; set; }
            public int AvailableQuantity { get; set; }

            public string Contractor { get; set; }
            public string ContractorRep { get; set; }
            public Nullable<int> FloorLevelId { get; set; }
            public string WorkGroup { get; set; }
            public string SubGroup { get; set; }
            public string RequestedBy { get; set; }
            public Nullable<bool> WithInRangeOfOutBoard { get; set; }
            public Nullable<bool> LongTermStructure { get; set; }
            public Nullable<bool> IsOfflineCurrent { get; set; }
            public Nullable<bool> NoInventory { get; set; }
            public Nullable<System.DateTime> PipelineStartDate { get; set; }
            public string PipeLineNote { get; set; }

            public Nullable<bool> ChkRequiresIsolation { get; set; }
            public string GoOfflineJobBy { get; set; }
            public string GoOfflineJobOn { get; set; }
            public Nullable<bool> OfflineJobCleared { get; set; }
            public Nullable<System.DateTime> OfflineJobClearedOn { get; set; }
            public string OfflineJobClearedBy { get; set; }


        }
        // by  shubham m ,neeraj ,pooja

        public class TblClient
        {
            public int? ClientId { get; set; }
            public bool? InspectionCheckListColor { get; set; }
            public decimal? ManHrsRatio { get; set; }
        }
        public class InspectionListdropdown
        {
            public int InspectionID { get; set; }

            public int UserId { get; set; }
            public string InspectionDate { get; set; }
            public string InspectionTime { get; set; }
            public int? InspectedById { get; set; }
            public string InspectedBy { get; set; }
            public string InspFileName { get; set; }
            public string Comments { get; set; }
            public int RegisterID { get; set; }
            public bool? OnlineInsp { get; set; }
            public bool? CompleteInsp { get; set; }
            public decimal? Lat { get; set; }
            public decimal? Long { get; set; }
            public string GPSAddress { get; set; }
            public byte[] InspImage { get; set; }
            public byte[] InspAudio { get; set; }
            public byte[] InspVideo { get; set; }
            public byte[] InspSign { get; set; }
            public string InspComments { get; set; }
            public bool? Acknowledgment { get; set; }
            public string InspStatus { get; set; }
            public string AckDate { get; set; }
            public string AckDateTime { get; set; }
            public DateTime? LastEmailSentOn { get; set; }
            public string ScheduledOrAckDate { get; set; }
            public TimeSpan? ScheduledOrAckTime { get; set; }
            public bool? OffSystemInspection { get; set; }
            public string LicenceNo { get; set; }

            public string Inspimage { get; set; }
            public string Inspimage2 { get; set; }
        }


        public class TblUserList
        {

            public int UserId;

            public string firstName { get; set; }
            public string FullName { get; set; }
            public string LastName { get; set; }
            //public string Notes { get; set; } 
            public string LicenceNo { get; set; }
            //public bool? IsInspector { get; set; }
            //public DateTime? BirthDate { get; set; }  
            //public string DirectOrIndirect { get; set; }
            //public int? CategoryId { get; set; }          
            //public string Country { get; set; }
            //public bool? Accept { get; set; }  
            //public string LicencePic { get; set; }  
            //public string LicencePicUpload { get; set; }
            //public string SessionId { get; set; }
            //public string PhotoAngle { get; set; }
            //public string SiteOwnerName { get; set; }
            //public string ScaffoldOwner { get; set; }
            //public string IPAddress { get; set; }
            //public bool? IPAuthentication { get; set; }
            //public bool? EmailAuthentication { get; set; }
        }


        public class TblcheckListsContent
        {
            public string CheckListContent { get; set; }

            public int checklistID { get; set; }
            public int? ClientID { get; set; }
            public int? ContentID { get; set; }
            public Nullable<int> checklistItem { get; set; }
            public Nullable<int> subChildItem { get; set; }
            public bool? IsNA { get; set; }
        }

        public class OfflineInspectionList
        {
            public int? InspectionListID { get; set; }
            public int? InspectionID { get; set; }
            public int? CheckListID { get; set; }
            public int? ContentID { get; set; }
            public Nullable<int> ClientID { get; set; }
            public bool? Y { get; set; }
            public bool? N { get; set; }
            public bool? NA { get; set; }
            public string ActionRequired { get; set; }
            public string CreatedBy { get; set; }
            public DateTime? CreatedOn { get; set; }
            public string UpdatedBy { get; set; }
            public DateTime? UpdatedOn { get; set; }
            public int? RegisterId { get; set; }
        }

        //public class RegisterGallerydropdown
        //{
        //    public int RegisterId { get; set; }
        //    public int ID { get; set; }
        //    public string Description;
        //    public string FileName;
        //    public string Type;
        //    public string Images;
        //}

        public class InventoryRequest
        {

            public int InventoryId { get; set; }
            public int REGISTERId { get; set; }
            public string MaterialType { get; set; }
            public string ItemCode { get; set; }
            public string ItemDesc { get; set; }
            public int AvailableQuantity { get; set; }
            public int Quantity { get; set; }

            public string PipeLineDesc { get; set; }
            public double ItemWeight { get; set; }
            public double TotalWeight { get; set; }
            public bool insertedOffline { get; set; }

        }

        public class InventoryListdropdown
        {
            public int REGISTERId { get; set; }
            public string MaterialType { get; set; }
            public string ItemCode { get; set; }
            public string ItemDesc { get; set; }

            public int Quantity { get; set; }
        }
        public class dropdown
        {
            public List<ddlsites> GetSiteList;
            public List<ddlScaffoldDesign> GetDesignList;

            public List<ddlFloorLevel> GetFloorLevelList;

            public List<ddlDutyrating> GetDutyRating;
            public List<ddlMaterialType> GetMaterialType;
            public List<ChecklistDropdown> GetCheckListData;
            public List<ddlLocation> GetAllLocationListData;
            public List<ddbuilding> GetAllBuildingListData;
            public List<ddassets> GetAllAssetListData;
            public List<SupervisorDropdown> GetSupervisorData;
            public List<ScaffoldOwnerDropdown> GetScaffoldOwnerData;
            public List<SubOwnerRepDropdown> GetSubOwnerRepData;
            public List<WorkGroupsDropdown> GetWorkGroupsData;
            public List<SubGroupsDropdown> GetSubGroupsData;
            public List<RequestedByDropdown> GetRequestedByData;
            public List<RegisterListdropdown> GetRegisterListData;
            public List<InspectionListdropdown> GetInspectionListData;
            public List<TblUserList> GetUserListData;
            public List<TblcheckListsContent> GetcheckListContentData;
            public List<OfflineInspectionList> GetInspectionListDetailsData;
            //public List<RegisterGallerydropdown> GetRegisterGalleryData;
            public List<TblClient> GetClientData;
            public List<InventoryListdropdown> GetInventoryListdropdowns;
            public List<InventoryRequest> GetInventoryRequests;
            //public string GetClientId;
        }

        public class ddlLocation
        {
            //public string areaid;
            //public string area;
            //public string SiteLocId;
            public string areaid { get; set; }
            public string area { get; set; }
            public string SiteLocId { get; set; }
        }

        public class ddbuilding
        {
            //public string BuildingID;
            //public string BuildingNo;
            //public string Area;
            //public string areaid;
            public string BuildingID { get; set; }
            public string BuildingNo { get; set; }
            public string Area { get; set; }
            public string areaid { get; set; }
        }

        public class ddassets
        {
            //public string Assetid;
            //public string Assetno;
            //public string BuildNo;
            //public string assetsdesc;
            //public string tblAssetList;
            //public string BuildingID;

            public int Assetid { get; set; }
            public string Assetno { get; set; }
            public string BuildNo { get; set; }
            public string assetsdesc { get; set; }
            public string tblAssetList { get; set; }
            public int BuildingID { get; set; }
            public int ClientID { get; set; }

        }

        public class RegisterDetails
        {
            public string txtSiteName { get; set; }
            public string txtSiteLocation { get; set; }
            public string txtdesc { get; set; }
            public string txtInspectedBy { get; set; }
            public string txtRecentinspectiondate { get; set; }
            public string txtDaylastinspetion { get; set; }
            public string txtstatus { get; set; }
            public string txtPlannedStatus { get; set; }
            public string txtHireDays { get; set; }
            public string txtTotalhireday { get; set; }
            public string txtdatetagged { get; set; }
            public string txtDatedismantle { get; set; }
            public string txtNoOfItem { get; set; }
            public string txttonnage { get; set; }
            public string txtoptstatus { get; set; }
            public string txtscaffref { get; set; }
            public string txtclientReference { get; set; }
            public string txtWOnumb { get; set; }
            public string rdpDateTagged { get; set; }
            public string rdpdatedis { get; set; }
            public string txtassetsdesc { get; set; }
            public string txtlength { get; set; }
            public string txtwidth { get; set; }
            public string txtheight { get; set; }
            public string DropDownList1 { get; set; }
            public string DropDownList2 { get; set; }
            public string txtScaffType { get; set; }
            public string ddlLockedIn { get; set; }
            public string txtplnstandDur { get; set; }
            public string txtCrewSize { get; set; }
            public string txtManhrserect { get; set; }
            public string txtmanDisman { get; set; }
            public string ddlInspectionTimeFrame { get; set; }
            public string txtSapNumber { get; set; }
            public string txtCrew { get; set; }
            public string txtPermit { get; set; }
            public string txtcomment { get; set; }
            public string ddlscaffoldowner { get; set; }
            public string rddateDisAppro { get; set; }
            public string txtdiappronotes { get; set; }
            public string txtbearning { get; set; }
            public string ddlLocksignedOffBy { get; set; }
            public string ddlSubownerRep { get; set; }
            public string txtDesignDrawingNo { get; set; }
            public string txtNoOfPlatform { get; set; }
            public string ddlEngineeredDesign { get; set; }
            public string checkboxMaintainInventory { get; set; }

            //public List<ddbuilding> GetBuildingList { get; set; }
            //public List<ddassets> GetAssetList { get; set; }
            public List<ddbuilding> GetBuildingList;
            public List<ddassets> GetAssetList;

            public string ddlsitesselected { get; set; }
            public string ddlbuildingselected { get; set; }
            //public List<RegisterDetails> GetSelectIndexChanged { get; set; }
            //public List<ddlLocation> GetLocationList { get; set; }
            public List<RegisterDetails> GetSelectIndexChanged;
            public List<ddlLocation> GetLocationList;
            public string txtsiteAddress { get; set; }
            public string txtsiteCity { get; set; }
            public string txtOwnername { get; set; }
            public string txtZip { get; set; }
            public string ddlLocationselected { get; set; }
            public string ddlDesignSelected { get; set; }
            public string ddlDutyratingSelected { get; set; }
            public string ddlMaterialTypeSelected { get; set; }
            public string ddassetsSelected { get; set; }
            public string ddlChecklistInspSelected { get; set; }
            public string ddlPlanningStatusSelected { get; set; }
            public string ddlSupervisorSelected { get; set; }

            public string txtClientScaff { get; set; }
            public string ddlWorkGroups { get; set; }
            public string ddlSubGroups { get; set; }
            public string ddlShiftAllocation { get; set; }
            public string txtRequestedBy { get; set; }
            public string checkboxLongTermStructure { get; set; }
            public string PhotoAngle { get; set; }
            public string image { get; set; }
            public string txtRegisterId { get; set; }
            public string chkNoInventory { get; set; }
            public int NoOfScafref { get; set; }
            public string PipelineStartDate { get; set; }
            public string PipeLineNote { get; set; }
            public string hdnTimeOffSet { get; set; }

            public bool chkReqis { get; set; }
            public string DismantledBy { get; set; }
            public string BuildDuringShutdown { get; set; }
        }

        public class OffRegisterDetails
        {
            public int RegisterID;
            public string txtSiteName { get; set; }
            public string txtSiteLocation { get; set; }
            public string txtdesc { get; set; }
            public string txtInspectedBy { get; set; }
            public string txtRecentinspectiondate { get; set; }
            public string txtDaylastinspetion { get; set; }
            public string txtstatus { get; set; }
            public string txtPlannedStatus { get; set; }
            public string txtHireDays { get; set; }
            public string txtTotalhireday { get; set; }
            public string txtdatetagged { get; set; }
            public string txtDatedismantle { get; set; }
            public string txtNoOfItem { get; set; }
            public string txttonnage { get; set; }
            public string txtoptstatus { get; set; }
            public string txtscaffref { get; set; }
            public string txtclientReference { get; set; }
            public string txtWOnumb { get; set; }
            public string rdpDateTagged { get; set; }
            public string rdpdatedis { get; set; }
            public string txtassetsdesc { get; set; }
            public string txtlength { get; set; }
            public string txtwidth { get; set; }
            public string txtheight { get; set; }
            public string DropDownList1 { get; set; }
            public string DropDownList2 { get; set; }
            public string txtScaffType { get; set; }
            public string ddlLockedIn { get; set; }
            public string txtplnstandDur { get; set; }
            public string txtCrewSize { get; set; }
            public string txtManhrserect { get; set; }
            public string txtmanDisman { get; set; }
            public string ddlInspectionTimeFrame { get; set; }
            public string txtSapNumber { get; set; }
            public string txtCrew { get; set; }
            public string txtPermit { get; set; }
            public string txtcomment { get; set; }
            public string ddlscaffoldowner { get; set; }
            public string rddateDisAppro { get; set; }
            public string txtdiappronotes { get; set; }
            public string txtbearning { get; set; }
            public string ddlLocksignedOffBy { get; set; }
            public string ddlSubownerRep { get; set; }
            public string txtDesignDrawingNo { get; set; }
            public string txtNoOfPlatform { get; set; }
            public string ddlEngineeredDesign { get; set; }
            public string checkboxMaintainInventory { get; set; }

            //public List<ddbuilding> GetBuildingList;
            //public List<ddassets> GetAssetList;

            public string ddlsitesselected { get; set; }
            public string ddlbuildingselected { get; set; }

            //public List<RegisterDetails> GetSelectIndexChanged;
            //public List<ddlLocation> GetLocationList;
            public string txtsiteAddress { get; set; }
            public string txtsiteCity { get; set; }
            public string txtOwnername { get; set; }
            public string txtZip { get; set; }
            public string ddlLocationselected { get; set; }
            public string ddlDesignSelected { get; set; }
            public string ddlDutyratingSelected { get; set; }
            public string ddlMaterialTypeSelected { get; set; }
            public string ddassetsSelected { get; set; }
            public string ddlChecklistInspSelected { get; set; }
            public string ddlPlanningStatusSelected { get; set; }
            public string ddlSupervisorSelected { get; set; }

            public string txtClientScaff { get; set; }
            public string ddlWorkGroups { get; set; }
            public string ddlSubGroups { get; set; }
            public string ddlShiftAllocation { get; set; }
            public string txtRequestedBy { get; set; }
            public string checkboxLongTermStructure { get; set; }
            public string PhotoAngle { get; set; }
            public string image { get; set; }
        }

        List<RegisterDetails> GetRegisterDetails = new List<RegisterDetails> { };
        // [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        // [WebMethod]
        // public List<RegisterDetails> ShowregisterDetails(string RID, string id)
        public ActionResult ShowregisterDetails(string RID, string id)
        {
            string query = "exec Pro_TBLREGISTER_GET '" + RID + "','" + databaseCon.Decrypt(id) + "','','','','','','','','','','','',''";
            DataSet ds = null;

            ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    RegisterDetails GetDetails = new RegisterDetails();
                    if (!(item["SiteLocation"] is DBNull) && item["SiteLocation"].ToString() != "")
                    {
                        GetDetails.txtSiteName = item["SiteLocation"].ToString();
                    }

                    if (!(item["Description"] is DBNull) && item["Description"].ToString() != "")
                    {
                        GetDetails.txtdesc = item["Description"].ToString();
                    }

                    if (!(item["Location"] is DBNull) && item["Location"].ToString() != "")
                    {
                        GetDetails.txtSiteLocation = item["Location"].ToString();
                    }

                    if (!(item["InspectedBy"] is DBNull) && item["InspectedBy"].ToString() != "")
                    {
                        GetDetails.txtInspectedBy = item["InspectedBy"].ToString();
                    }

                    if (!(item["RecentInspDate"] is DBNull) && Convert.ToString(item["RecentInspDate"]) != "01/01/1900 12:00:00 AM")
                    {
                        GetDetails.txtRecentinspectiondate = Convert.ToDateTime(item["RecentInspDate"].ToString()).ToString("dd-MMM-yyyy");
                    }

                    if (!(item["DaysSinceLastInsp"] is DBNull) && item["DaysSinceLastInsp"].ToString() != "")
                    {
                        GetDetails.txtDaylastinspetion = item["DaysSinceLastInsp"].ToString();
                    }

                    if (!(item["Status"] is DBNull) && item["Status"].ToString() != "")
                    {
                        GetDetails.txtstatus = item["Status"].ToString();
                    }

                    if (!(item["PlanningStatus"] is DBNull) && item["PlanningStatus"].ToString() != "")
                    {
                        GetDetails.txtPlannedStatus = item["PlanningStatus"].ToString();
                    }

                    if (!(item["HireDaysToDate"] is DBNull) && item["HireDaysToDate"].ToString() != "")
                    {
                        GetDetails.txtHireDays = item["HireDaysToDate"].ToString();
                    }

                    if (!(item["TotalHireDays"] is DBNull) && item["TotalHireDays"].ToString() != "")
                    {
                        GetDetails.txtTotalhireday = item["TotalHireDays"].ToString();
                    }

                    if (!(item["DateTagged"] is DBNull) && item["DateTagged"].ToString() != "01/01/1900 12:00:00 AM")
                    {
                        GetDetails.rdpDateTagged = Convert.ToDateTime(item["DateTagged"].ToString()).ToString("dd-MMM-yyyy");
                        GetDetails.txtdatetagged = Convert.ToDateTime(item["DateTagged"].ToString()).ToString("dd-MMM-yyyy");
                    }

                    if (!(item["DateOFDismantle"] is DBNull) && item["DateOFDismantle"].ToString() != "01/01/1900 12:00:00 AM")
                    {
                        GetDetails.txtDatedismantle = Convert.ToDateTime(item["DateOFDismantle"]).ToString("dd-MMM-yyyy");
                        GetDetails.rdpdatedis = Convert.ToDateTime(item["DateOFDismantle"]).ToString("dd-MMM-yyyy");
                    }

                    if (!(item["NoOfItems"] is DBNull) && item["NoOfItems"].ToString() != "")
                    {
                        GetDetails.txtNoOfItem = item["NoOfItems"].ToString();
                    }

                    if (!(item["Tonnage"] is DBNull) && item["Tonnage"].ToString() != "")
                    {
                        GetDetails.txttonnage = item["Tonnage"].ToString();
                    }

                    if (!(item["OptimizeStatus"] is DBNull) && item["OptimizeStatus"].ToString() != "")
                    {
                        GetDetails.txtoptstatus = item["OptimizeStatus"].ToString();
                    }

                    if (!(item["ShowRegisterID"] is DBNull) && item["ShowRegisterID"].ToString() != "")
                    {
                        GetDetails.txtscaffref = item["ShowRegisterID"].ToString();
                    }

                    if (!(item["AssetDesc"] is DBNull) && item["AssetDesc"].ToString() != "")
                    {
                        GetDetails.txtassetsdesc = item["AssetDesc"].ToString();
                    }

                    if (!(item["Length"] is DBNull) && item["Length"].ToString() != "")
                    {
                        GetDetails.txtlength = item["Length"].ToString();
                    }
                    if (!(item["Width"] is DBNull) && item["Width"].ToString() != "")
                    {
                        GetDetails.txtwidth = item["Width"].ToString();
                    }

                    if (!(item["Height"] is DBNull) && item["Height"].ToString() != "")
                    {
                        GetDetails.txtheight = item["Height"].ToString();
                    }

                    if (!(item["FloorLevel"] is DBNull) && item["FloorLevel"].ToString() != "")
                    {
                        GetDetails.DropDownList1 = item["FloorLevel"].ToString();
                    }

                    if (!(item["NoOfWorkingDecks"] is DBNull) && item["NoOfWorkingDecks"].ToString() != "")
                    {
                        GetDetails.DropDownList2 = item["NoOfWorkingDecks"].ToString();
                    }

                    if (!(item["ScafType"] is DBNull) && item["ScafType"].ToString() != "")
                    {
                        GetDetails.txtScaffType = item["ScafType"].ToString();
                    }

                    if (!(item["LockedIn"] is DBNull) && item["LockedIn"].ToString() != "")
                    {
                        GetDetails.ddlLockedIn = item["LockedIn"].ToString().ToUpper() == "TRUE" ? "1" : "0";
                    }

                    if (!(item["PlannedStandingDuration"] is DBNull) && item["PlannedStandingDuration"].ToString() != "")
                    {
                        GetDetails.txtplnstandDur = item["PlannedStandingDuration"].ToString();
                    }

                    if (!(item["CrewSize"] is DBNull) && item["CrewSize"].ToString() != "")
                    {
                        GetDetails.txtCrewSize = item["CrewSize"].ToString();
                    }
                    if (!(item["SAPNumber"] is DBNull) && item["SAPNumber"].ToString() != "")
                    {
                        GetDetails.txtSapNumber = item["SAPNumber"].ToString();
                    }
                    if (!(item["Crew"] is DBNull) && item["Crew"].ToString() != "")
                    {
                        GetDetails.txtCrew = item["Crew"].ToString();
                    }
                    if (!(item["PermitNo"] is DBNull) && item["PermitNo"].ToString() != "")
                    {
                        GetDetails.txtPermit = item["PermitNo"].ToString();
                    }
                    if (!(item["Comments"] is DBNull) && item["Comments"].ToString() != "")
                    {
                        GetDetails.txtcomment = item["Comments"].ToString();
                    }

                    if (!(item["ManHrsErect"] is DBNull) && item["ManHrsErect"].ToString() != "")
                    {
                        GetDetails.txtManhrserect = item["ManHrsErect"].ToString();
                    }

                    if (!(item["ManHrsDismantle"] is DBNull) && item["ManHrsDismantle"].ToString() != "")
                    {
                        GetDetails.txtmanDisman = item["ManHrsDismantle"].ToString();
                    }

                    if (!(item["DesignType"] is DBNull) && item["DesignType"].ToString() != "")
                    {
                        GetDetails.ddlDesignSelected = item["DesignType"].ToString();
                    }

                    if (!(item["DutyRating"] is DBNull) && item["DutyRating"].ToString() != "")
                    {
                        GetDetails.ddlDutyratingSelected = item["DutyRating"].ToString();
                    }

                    if (!(item["MaterialType"] is DBNull) && item["MaterialType"].ToString() != "")
                    {
                        GetDetails.ddlMaterialTypeSelected = item["MaterialType"].ToString();
                    }

                    if (!(item["InspTimeFrame"] is DBNull) && item["InspTimeFrame"].ToString() != "")
                    {
                        GetDetails.ddlInspectionTimeFrame = item["InspTimeFrame"].ToString();
                    }

                    if (!(item["CheckListId"] is DBNull) && item["CheckListId"].ToString() != "")
                    {
                        GetDetails.ddlChecklistInspSelected = item["CheckListId"].ToString();
                    }
                    if (!(item["PlanningStatus"] is DBNull) && item["PlanningStatus"].ToString() != "")
                    {
                        GetDetails.ddlPlanningStatusSelected = item["PlanningStatus"].ToString();
                    }
                    if (!(item["ClientRef"] is DBNull) && item["ClientRef"].ToString() != "")
                    {
                        GetDetails.txtclientReference = item["ClientRef"].ToString();
                    }

                    if (!(item["WONumber"] is DBNull) && item["WONumber"].ToString() != "")
                    {
                        GetDetails.txtWOnumb = item["WONumber"].ToString();
                    }




                    if (!(item["SiteLocID"] is DBNull) && item["SiteLocID"].ToString() != "")
                    {
                        GetDetails.ddlsitesselected = item["SiteLocID"].ToString();

                        //GetDetails.GetSelectIndexChanged = ddlsites_SelectedIndexChanged("", id, Convert.ToString(GetDetails.ddlsitesselected), RID);
                    }

                    if (!(item["LocationId"] is DBNull) && item["LocationId"].ToString() != "")
                    {
                        GetDetails.ddlLocationselected = item["LocationId"].ToString();

                        //GetDetails.GetBuildingList = ddlLocation_SelectedIndexChanged(id, Convert.ToString(GetDetails.ddlLocationselected));
                    }

                    if (!(item["BuildingID"] is DBNull) && item["BuildingID"].ToString() != "")
                    {
                        GetDetails.ddlbuildingselected = item["BuildingID"].ToString();

                        GetDetails.GetAssetList = ddbuilding_SelectedIndexChanged(id, Convert.ToString(GetDetails.ddlbuildingselected));
                    }

                    if (!(item["AssetID"] is DBNull) && item["AssetID"].ToString() != "")
                    {
                        GetDetails.ddassetsSelected = item["AssetID"].ToString();
                        GetDetails.txtassetsdesc = ddassets_SelectedIndexChanged(id, item["AssetID"].ToString());
                    }

                    GetRegisterDetails.Add(GetDetails);
                }

            }

            //return GetRegisterDetails;
            return Json(GetRegisterDetails, JsonRequestBehavior.AllowGet);
        }

        List<OffRegisterDetails> GetListRegister = new List<OffRegisterDetails> { };
        [HttpPost]
        public ActionResult GetAllRegisterDetails(string ClientID, string UID)
        {

            string query = "select RegisterId,Description,Zip,OwnerName,SiteLocation,Location,ClientID,InspectedBy,DesignType,DutyRating,MaterialType,DateTagged,DateOfDismantle,Image,ScaffRef,ClientRef,InspTimeFrame,SiteLocId,PhotoAngle,BuildingID,AssetId,BuildingNo,AssetNo,AssetDesc,Length,Height,Width,NoOfWorkingDecks,FloorLevelId,LocationId,OnlineApplied,CheckListId,PlanningStatus,Lat,Long,LockedIn,PlannedStandingDuration,CrewSize,ManHrsErect,ManHrsDismantle,SupervisorId,Crew,PermitNo,Comments,SAPNumber,DismantleApprovedNotes,Bearing,LockedInSignedOff,MaintainInventory,RecentInspDate,DaysSinceLastInsp,Status,DesignDrawingNo,NumberOfPlatform,EngineeredDesign,WONumber,HireDaysToDate,TotalHireDays,ScafType,OptimizeStatus,PlanningStatus,SupervisorId,discipline,NoOfItems,CONVERT(DECIMAL(10,2),Tonnage/1000) as Tonnage,ScaffoldOwner,DismantleApprovedDate,DismantleApprovedNotes,ShiftAllocation,ClientScaffref,WorkGroup,SubGroup,RequestedBy,Right('000000'+Convert(varchar,ScaffRef),6) as ShowRegisterID,LongTermStructure,OriginalBaseImage  from vw_tblregister where ClientID ='" + ClientID + "' and IsOfflineCurrent= 1";

            DataSet ds = null;

            ds = databaseCon.GetDataSet(query);

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    OffRegisterDetails ListRegister = new OffRegisterDetails();

                    if (!(item["RegisterId"] is DBNull) && item["RegisterId"].ToString() != "")
                    {
                        ListRegister.RegisterID = Convert.ToInt32(item["RegisterId"].ToString());
                    }

                    if (!(item["SiteLocation"] is DBNull) && item["SiteLocation"].ToString() != "")
                    {
                        ListRegister.txtSiteName = item["SiteLocation"].ToString();
                    }
                    if (!(item["Zip"] is DBNull) && item["Zip"].ToString() != "")
                    {
                        ListRegister.txtZip = item["Zip"].ToString();
                    }
                    if (!(item["OwnerName"] is DBNull) && item["OwnerName"].ToString() != "")
                    {
                        ListRegister.txtOwnername = item["OwnerName"].ToString();
                    }
                    if (!(item["Location"] is DBNull) && item["Location"].ToString() != "")
                    {
                        ListRegister.txtSiteLocation = item["Location"].ToString();
                    }

                    if (!(item["Description"] is DBNull) && item["Description"].ToString() != "")
                    {
                        ListRegister.txtdesc = item["Description"].ToString();
                    }
                    if (!(item["InspectedBy"] is DBNull) && item["InspectedBy"].ToString() != "")
                    {
                        ListRegister.txtInspectedBy = item["InspectedBy"].ToString();
                    }

                    if (!(item["RecentInspDate"] is DBNull) && Convert.ToString(item["RecentInspDate"]) != "01/01/1900 12:00:00 AM")
                    {
                        ListRegister.txtRecentinspectiondate = Convert.ToDateTime(item["RecentInspDate"].ToString()).ToString("dd-MMM-yyyy");
                    }

                    if (!(item["DaysSinceLastInsp"] is DBNull) && item["DaysSinceLastInsp"].ToString() != "")
                    {
                        ListRegister.txtDaylastinspetion = item["DaysSinceLastInsp"].ToString();
                    }

                    if (!(item["Status"] is DBNull) && item["Status"].ToString() != "")
                    {
                        ListRegister.txtstatus = item["Status"].ToString();
                    }

                    if (!(item["PlanningStatus"] is DBNull) && item["PlanningStatus"].ToString() != "")
                    {
                        ListRegister.txtPlannedStatus = item["PlanningStatus"].ToString();
                    }

                    if (!(item["HireDaysToDate"] is DBNull) && item["HireDaysToDate"].ToString() != "")
                    {
                        ListRegister.txtHireDays = item["HireDaysToDate"].ToString();
                    }

                    if (!(item["TotalHireDays"] is DBNull) && item["TotalHireDays"].ToString() != "")
                    {
                        ListRegister.txtTotalhireday = item["TotalHireDays"].ToString();
                    }

                    if (!(item["DateTagged"] is DBNull) && item["DateTagged"].ToString() != "01/01/1900 12:00:00 AM")
                    {
                        ListRegister.rdpDateTagged = Convert.ToDateTime(item["DateTagged"].ToString()).ToString("dd-MMM-yyyy");
                        ListRegister.txtdatetagged = Convert.ToDateTime(item["DateTagged"].ToString()).ToString("dd-MMM-yyyy");
                    }

                    if (!(item["DateOfDismantle"] is DBNull) && item["DateOfDismantle"].ToString() != "01/01/1900 12:00:00 AM")
                    {
                        ListRegister.txtDatedismantle = Convert.ToDateTime(item["DateOfDismantle"]).ToString("dd-MMM-yyyy");
                        ListRegister.rdpdatedis = Convert.ToDateTime(item["DateOfDismantle"]).ToString("dd-MMM-yyyy");
                    }

                    if (!(item["NoOfItems"] is DBNull) && item["NoOfItems"].ToString() != "")
                    {
                        ListRegister.txtNoOfItem = item["NoOfItems"].ToString();
                    }

                    if (!(item["Tonnage"] is DBNull) && item["Tonnage"].ToString() != "")
                    {
                        ListRegister.txttonnage = item["Tonnage"].ToString();
                    }

                    if (!(item["OptimizeStatus"] is DBNull) && item["OptimizeStatus"].ToString() != "")
                    {
                        ListRegister.txtoptstatus = item["OptimizeStatus"].ToString();
                    }

                    if (!(item["ShowRegisterID"] is DBNull) && item["ShowRegisterID"].ToString() != "")
                    {
                        ListRegister.txtscaffref = item["ShowRegisterID"].ToString();
                    }

                    //if (!(item["ScaffRef"] is DBNull) && item["ScaffRef"].ToString() != "")
                    //{
                    //    ListRegister.txtscaffref = item["ScaffRef"].ToString();
                    //}

                    if (!(item["AssetDesc"] is DBNull) && item["AssetDesc"].ToString() != "")
                    {
                        ListRegister.txtassetsdesc = item["AssetDesc"].ToString();
                    }

                    if (!(item["Length"] is DBNull) && item["Length"].ToString() != "")
                    {
                        ListRegister.txtlength = item["Length"].ToString();
                    }
                    if (!(item["Width"] is DBNull) && item["Width"].ToString() != "")
                    {
                        ListRegister.txtwidth = item["Width"].ToString();
                    }

                    if (!(item["Height"] is DBNull) && item["Height"].ToString() != "")
                    {
                        ListRegister.txtheight = item["Height"].ToString();
                    }

                    //if (!(item["FloorLevel"] is DBNull) && item["FloorLevel"].ToString() != "")
                    //{
                    //    ListRegister.DropDownList1 = item["FloorLevel"].ToString();
                    //}
                    if (!(item["FloorLevelId"] is DBNull) && item["FloorLevelId"].ToString() != "")
                    {
                        ListRegister.DropDownList1 = item["FloorLevelId"].ToString();
                    }

                    if (!(item["NoOfWorkingDecks"] is DBNull) && item["NoOfWorkingDecks"].ToString() != "")
                    {
                        ListRegister.DropDownList2 = item["NoOfWorkingDecks"].ToString();
                    }

                    if (!(item["ScafType"] is DBNull) && item["ScafType"].ToString() != "")
                    {
                        ListRegister.txtScaffType = item["ScafType"].ToString();
                    }

                    if (!(item["LockedIn"] is DBNull) && item["LockedIn"].ToString() != "")
                    {
                        ListRegister.ddlLockedIn = item["LockedIn"].ToString().ToUpper() == "TRUE" ? "1" : "0";
                    }

                    if (!(item["PlannedStandingDuration"] is DBNull) && item["PlannedStandingDuration"].ToString() != "")
                    {
                        ListRegister.txtplnstandDur = item["PlannedStandingDuration"].ToString();
                    }

                    if (!(item["CrewSize"] is DBNull) && item["CrewSize"].ToString() != "")
                    {
                        ListRegister.txtCrewSize = item["CrewSize"].ToString();
                    }
                    if (!(item["SAPNumber"] is DBNull) && item["SAPNumber"].ToString() != "")
                    {
                        ListRegister.txtSapNumber = item["SAPNumber"].ToString();
                    }
                    if (!(item["Crew"] is DBNull) && item["Crew"].ToString() != "")
                    {
                        ListRegister.txtCrew = item["Crew"].ToString();
                    }
                    if (!(item["PermitNo"] is DBNull) && item["PermitNo"].ToString() != "")
                    {
                        ListRegister.txtPermit = item["PermitNo"].ToString();
                    }
                    if (!(item["Comments"] is DBNull) && item["Comments"].ToString() != "")
                    {
                        ListRegister.txtcomment = item["Comments"].ToString();
                    }

                    if (!(item["ManHrsErect"] is DBNull) && item["ManHrsErect"].ToString() != "")
                    {
                        ListRegister.txtManhrserect = item["ManHrsErect"].ToString();
                    }

                    if (!(item["ManHrsDismantle"] is DBNull) && item["ManHrsDismantle"].ToString() != "")
                    {
                        ListRegister.txtmanDisman = item["ManHrsDismantle"].ToString();
                    }

                    if (!(item["DesignType"] is DBNull) && item["DesignType"].ToString() != "")
                    {
                        ListRegister.ddlDesignSelected = item["DesignType"].ToString();
                    }

                    if (!(item["DutyRating"] is DBNull) && item["DutyRating"].ToString() != "")
                    {
                        ListRegister.ddlDutyratingSelected = item["DutyRating"].ToString();
                    }

                    if (!(item["MaterialType"] is DBNull) && item["MaterialType"].ToString() != "")
                    {
                        ListRegister.ddlMaterialTypeSelected = item["MaterialType"].ToString();
                    }

                    if (!(item["InspTimeFrame"] is DBNull) && item["InspTimeFrame"].ToString() != "")
                    {
                        ListRegister.ddlInspectionTimeFrame = item["InspTimeFrame"].ToString();
                    }

                    if (!(item["CheckListId"] is DBNull) && item["CheckListId"].ToString() != "")
                    {
                        ListRegister.ddlChecklistInspSelected = item["CheckListId"].ToString();
                    }
                    if (!(item["PlanningStatus"] is DBNull) && item["PlanningStatus"].ToString() != "")
                    {
                        ListRegister.ddlPlanningStatusSelected = item["PlanningStatus"].ToString();
                    }
                    if (!(item["ScaffoldOwner"] is DBNull) && item["ScaffoldOwner"].ToString() != "")
                    {
                        ListRegister.ddlscaffoldowner = item["ScaffoldOwner"].ToString();
                    }

                    if (!(item["DismantleApprovedDate"] is DBNull) && item["DismantleApprovedDate"].ToString() != "")
                    {
                        ListRegister.rddateDisAppro = Convert.ToDateTime(item["DismantleApprovedDate"]).ToString("dd-MMM-yyyy");
                    }

                    if (!(item["DismantleApprovedNotes"] is DBNull) && item["DismantleApprovedNotes"].ToString() != "")
                    {
                        ListRegister.txtdiappronotes = item["DismantleApprovedNotes"].ToString();
                    }

                    if (!(item["ClientRef"] is DBNull) && item["ClientRef"].ToString() != "")
                    {
                        ListRegister.txtclientReference = item["ClientRef"].ToString();
                    }

                    if (!(item["WONumber"] is DBNull) && item["WONumber"].ToString() != "")
                    {
                        ListRegister.txtWOnumb = item["WONumber"].ToString();
                    }

                    if (!(item["SiteLocId"] is DBNull) && item["SiteLocId"].ToString() != "")
                    {
                        ListRegister.ddlsitesselected = item["SiteLocId"].ToString();

                    }
                    if (!(item["SupervisorId"] is DBNull) && item["SupervisorId"].ToString() != "")
                    {
                        ListRegister.ddlSupervisorSelected = item["SupervisorId"].ToString();
                    }

                    if (!(item["ShiftAllocation"] is DBNull) && item["ShiftAllocation"].ToString() != "")
                    {
                        ListRegister.ddlShiftAllocation = item["ShiftAllocation"].ToString();
                    }

                    if (!(item["WorkGroup"] is DBNull) && item["WorkGroup"].ToString() != "")
                    {
                        ListRegister.ddlWorkGroups = item["WorkGroup"].ToString();
                    }

                    if (!(item["SubGroup"] is DBNull) && item["SubGroup"].ToString() != "")
                    {
                        ListRegister.ddlSubGroups = item["SubGroup"].ToString();
                    }


                    if (!(item["Bearing"] is DBNull) && item["Bearing"].ToString() != "")
                    {
                        ListRegister.txtbearning = item["Bearing"].ToString();
                    }

                    if (!(item["LockedInSignedOff"] is DBNull) && item["LockedInSignedOff"].ToString() != "")
                    {
                        ListRegister.ddlLocksignedOffBy = item["LockedInSignedOff"].ToString();
                    }

                    if (!(item["discipline"] is DBNull) && item["discipline"].ToString() != "")
                    {
                        ListRegister.ddlSubownerRep = item["discipline"].ToString();
                    }

                    if (!(item["EngineeredDesign"] is DBNull) && item["EngineeredDesign"].ToString() != "")
                    {
                        ListRegister.ddlEngineeredDesign = item["EngineeredDesign"].ToString();
                    }

                    if (!(item["DesignDrawingNo"] is DBNull) && item["DesignDrawingNo"].ToString() != "")
                    {
                        ListRegister.txtDesignDrawingNo = item["DesignDrawingNo"].ToString();
                    }

                    if (!(item["NumberOfPlatform"] is DBNull) && item["NumberOfPlatform"].ToString() != "")
                    {
                        ListRegister.txtNoOfPlatform = item["NumberOfPlatform"].ToString();
                    }

                    if (!(item["MaintainInventory"] is DBNull) && item["MaintainInventory"].ToString() != "")
                    {
                        ListRegister.checkboxMaintainInventory = item["MaintainInventory"].ToString().ToUpper() == "TRUE" ? "true" : "false";
                    }
                    if (!(item["ClientScaffref"] is DBNull) && item["ClientScaffref"].ToString() != "")
                    {
                        ListRegister.txtClientScaff = item["ClientScaffref"].ToString();
                    }



                    if (!(item["LocationId"] is DBNull) && item["LocationId"].ToString() != "")
                    {
                        ListRegister.ddlLocationselected = item["LocationId"].ToString();

                    }

                    //if (!(item["BuildingID"] is DBNull) && item["BuildingID"].ToString() != "")
                    //{
                    //    ListRegister.ddlbuildingselected = item["BuildingID"].ToString();

                    //}

                    //if (!(item["AssetId"] is DBNull) && item["AssetId"].ToString() != "")
                    //{
                    //    ListRegister.ddassetsSelected = item["AssetId"].ToString();
                    //}
                    if (!(item["BuildingNo"] is DBNull) && item["BuildingNo"].ToString() != "")
                    {
                        ListRegister.ddlbuildingselected = item["BuildingNo"].ToString();

                    }

                    if (!(item["Assetno"] is DBNull) && item["Assetno"].ToString() != "")
                    {
                        ListRegister.ddassetsSelected = item["Assetno"].ToString();
                    }

                    if (!(item["PhotoAngle"] is DBNull) && item["PhotoAngle"].ToString() != "")
                    {
                        ListRegister.PhotoAngle = item["PhotoAngle"].ToString();
                    }

                    //if (!(item["Image"] is DBNull) && item["Image"].ToString() != "")
                    //{
                    //    ListRegister.image = item["Image"].ToString();
                    //}
                    if (!(item["OriginalBaseImage"] is DBNull) && item["OriginalBaseImage"].ToString() != "")
                    {
                        ListRegister.image = item["OriginalBaseImage"].ToString();
                    }

                    if (!(item["RequestedBy"] is DBNull) && item["RequestedBy"].ToString() != "")
                    {
                        ListRegister.txtRequestedBy = item["RequestedBy"].ToString();
                    }

                    if (!(item["LongTermStructure"] is DBNull) && item["LongTermStructure"].ToString() != "")
                    {
                        ListRegister.checkboxLongTermStructure = item["LongTermStructure"].ToString().ToUpper() == "TRUE" ? "true" : "false";
                    }

                    GetListRegister.Add(ListRegister);
                }
            }

            //return Json(GetListRegister, JsonRequestBehavior.AllowGet);
            var jsonResult = Json(GetListRegister, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }


        List<dropdown> GetAllDropDown = new List<dropdown> { };
        // [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        // [WebMethod]
        // public List<dropdown> BindallDropdown(string id)
        public ActionResult BindallDropdown(string id, string Siteid, string RID)
        {
            dropdown GetDropdownList = new dropdown();
            GetDropdownList.GetSiteList = GetAllSites(id);
            GetDropdownList.GetDesignList = GetAllScaffoldDesign(id);

            GetDropdownList.GetFloorLevelList = GetAllScaffoldFloorLevel(id);

            GetDropdownList.GetDutyRating = GetAllDutyRatingList(id);
            GetDropdownList.GetMaterialType = GetAllMaterialTypeList(id);
            GetDropdownList.GetCheckListData = GetAllCheckList(id, RID);
            GetDropdownList.GetAllLocationListData = GetLocationList;
            GetDropdownList.GetAllBuildingListData = GetBuildingList;
            GetDropdownList.GetAllAssetListData = GetAssetList;
            GetDropdownList.GetSupervisorData = GetSupivisorList(id);
            GetDropdownList.GetScaffoldOwnerData = GetScaffoldOwnerList(id);
            GetDropdownList.GetSubOwnerRepData = GetSubOwnerRepDropdown(id);
            GetDropdownList.GetWorkGroupsData = GetWorkGroupsDropdown(id, Siteid);
            GetDropdownList.GetSubGroupsData = GetSubGroupsDropdown(id, Siteid);
            GetDropdownList.GetRequestedByData = GetRequestedByDropdown(id, Siteid);
            GetDropdownList.GetRegisterListData = GetRegisterListDetails(id, Siteid, RID);
            //GetDropdownList.GetInspectionListData = GetInspectionListDetails(id, Siteid);
            //GetDropdownList.GetInspectionListData = FetchInspectionList(id, Siteid);
            GetDropdownList.GetUserListData = GetUserListDataDetails(id, RID);
            GetDropdownList.GetcheckListContentData = GetcheckListContentDataDetails(id, RID);
            //GetDropdownList.GetInspectionListDetailsData = GetOfflineInspectionListDetailsData(id);
            //GetDropdownList.GetRegisterGalleryData = GetRegisterGallerytData(id);
            GetDropdownList.GetClientData = GetClientDetails(id);
            GetDropdownList.GetInventoryRequests = ShowInventoryRequest(id, RID);
            GetDropdownList.GetInventoryListdropdowns = GettblJobInventoryListData(id, Siteid);

            //GetDropdownList.GetClientId = databaseCon.Decrypt(id).ToString();
            GetAllDropDown.Add(GetDropdownList);
            var jsonResult = Json(GetAllDropDown, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }


        List<ddlsites> GetAllSitesList = new List<ddlsites> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod]
        //public List<ddlsites> GetAllSites(string id)

        //{
        //    //string Sitequery = "select distinct SiteLocID,SiteLocation from tblSiteLocation where SiteLocationStatus='Active' and SiteLocation <>'' and SiteLocation is not null and  ClientId='" + databaseCon.Decrypt(id) + "'";
        //    string Sitequery = "select * from tblSiteLocation where SiteLocationStatus='Active' and SiteLocation <>'' and SiteLocation is not null and  ClientId='" + id + "'";
        //    //// Add On 24 Jan 2020 For Only 
        //    //string Sitequery = "select distinct SiteLocID,SiteLocation from tblSiteLocation where SiteLocationStatus='Active' and SiteLocation <>'' and SiteLocation is not null and  ClientId='" + id + "' and GPS=1";
        //    DataSet ds = null;

        //    ds = databaseCon.GetDataSet(Sitequery);
        //    if (ds.Tables.Count > 0)
        //    {
        //        foreach (DataRow item in ds.Tables[0].Rows)
        //        {
        //            ddlsites SitesList = new ddlsites();
        //            if (!(item["SiteLocID"] is DBNull) && item["SiteLocID"].ToString() != "")
        //            {
        //                SitesList.SiteLocID = Convert.ToInt32(item["SiteLocID"].ToString());

        //                GetAllLocationList(id, item["SiteLocID"].ToString());

        //                //string SItequery = "select Address,city,state,OwnerName,Zip,MaintainInventoryAtJob from tblSiteLocation where SiteLocId= '" + Convert.ToInt32(item["SiteLocID"].ToString()) + "' and ClientID='" + id + "'";

        //                string SItequery = "select Address,city,state,OwnerName,Zip,MaintainInventoryAtJob,FloorLevel,GPS,CycloneManagement,PipeLineInvHide from tblSiteLocation where SiteLocId= '" + Convert.ToInt32(item["SiteLocID"].ToString()) + "' and ClientID='" + id + "'";

        //                DataSet newds = databaseCon.GetDataSet(SItequery);
        //                if (newds.Tables.Count > 0)
        //                {
        //                    foreach (DataRow Newitem in newds.Tables[0].Rows)
        //                    {
        //                        if (!(Newitem["OwnerName"] is DBNull) && Newitem["OwnerName"].ToString() != "")
        //                        {
        //                            SitesList.OwnerName = Newitem["OwnerName"].ToString().Replace("'", "''").Trim();
        //                        }

        //                        if (!(Newitem["Address"] is DBNull) && Newitem["Address"].ToString() != "")
        //                        {
        //                            SitesList.Address = Newitem["Address"].ToString().Replace("'", "''").Trim();
        //                        }

        //                        if (!(Newitem["city"] is DBNull) && Newitem["city"].ToString() != "")
        //                        {
        //                            SitesList.city = Newitem["city"].ToString().Replace("'", "''").Trim();
        //                        }

        //                        if (!(Newitem["Zip"] is DBNull) && Newitem["Zip"].ToString() != "")
        //                        {
        //                            SitesList.Zip = Newitem["Zip"].ToString();
        //                        }
        //                        // ADd on 19 Nov 2019 
        //                        if (!(Newitem["MaintainInventoryAtJob"] is DBNull) && Newitem["MaintainInventoryAtJob"].ToString() != "")
        //                        {
        //                            SitesList.MaintainInventoryAtJob = Convert.ToBoolean(Newitem["MaintainInventoryAtJob"]);
        //                        }

        //                        // Add on 27 JAN 2020.
        //                        if (!(Newitem["FloorLevel"] is DBNull) && Newitem["FloorLevel"].ToString() != "")
        //                        {
        //                            SitesList.FloorLevel = Newitem["FloorLevel"].ToString().Replace("'", "''").Trim();
        //                        }

        //                        if (!(Newitem["GPS"] is DBNull) && Newitem["GPS"].ToString() != "")
        //                        {
        //                            SitesList.GPS = Newitem["GPS"].ToString().Replace("'", "''").Trim();
        //                        }

        //                        // Add on 19 DEC 2024
        //                        if (!(item["CycloneManagement"] is DBNull) && Newitem["CycloneManagement"].ToString() != "")
        //                        {
        //                            SitesList.CycloneManagement = Convert.ToBoolean(Newitem["CycloneManagement"]);
        //                        }
        //                        if (!(item["PipeLineInvHide"] is DBNull) && Newitem["PipeLineInvHide"].ToString() != "")
        //                        {
        //                            SitesList.PipeLineInvHide = Convert.ToBoolean(Newitem["PipeLineInvHide"]);
        //                        }
        //                    }
        //                }

        //            }

        //            if (!(item["SiteLocation"] is DBNull) && item["SiteLocation"].ToString() != "")
        //            {
        //                SitesList.SiteLocation = item["SiteLocation"].ToString().Replace("'", "''").Trim();
        //            }

        //            GetAllSitesList.Add(SitesList);
        //        }
        //    }

        //    return GetAllSitesList;

        //}
        public List<ddlsites> GetAllSites(string id)
        {
            //string Sitequery = "select distinct SiteLocID,SiteLocation from tblSiteLocation where SiteLocationStatus='Active' and SiteLocation <>'' and SiteLocation is not null and  ClientId='" + databaseCon.Decrypt(id) + "'";
            string Sitequery = "select SiteLocID, SiteLocation from tblSiteLocation where SiteLocationStatus='Active' and SiteLocation <>'' and SiteLocation is not null and ClientId='" + id + "'";
            //// Add On 24 Jan 2020 For Only 
            //string Sitequery = "select distinct SiteLocID,SiteLocation from tblSiteLocation where SiteLocationStatus='Active' and SiteLocation <>'' and SiteLocation is not null and  ClientId='" + id + "' and GPS=1";
            DataSet ds = null;

            ds = databaseCon.GetDataSet(Sitequery);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ddlsites SitesList = new ddlsites();
                    if (!(item["SiteLocID"] is DBNull) && item["SiteLocID"].ToString() != "")
                    {
                        SitesList.SiteLocID = Convert.ToInt32(item["SiteLocID"].ToString());

                        GetAllLocationList(id, item["SiteLocID"].ToString());

                        //string SItequery = "select Address,city,state,OwnerName,Zip,MaintainInventoryAtJob from tblSiteLocation where SiteLocId= '" + Convert.ToInt32(item["SiteLocID"].ToString()) + "' and ClientID='" + id + "'";

                        string SItequery = "select Address,city,state,OwnerName,Zip,MaintainInventoryAtJob,FloorLevel,GPS,CycloneManagement,PipeLineInvHide from tblSiteLocation where SiteLocId= '" + Convert.ToInt32(item["SiteLocID"].ToString()) + "' and ClientID='" + id + "'";

                        DataSet newds = databaseCon.GetDataSet(SItequery);
                        if (newds.Tables.Count > 0)
                        {
                            foreach (DataRow Newitem in newds.Tables[0].Rows)
                            {
                                if (!(Newitem["OwnerName"] is DBNull) && Newitem["OwnerName"].ToString() != "")
                                {
                                    SitesList.OwnerName = Newitem["OwnerName"].ToString().Replace("'", "''").Trim();
                                }

                                if (!(Newitem["Address"] is DBNull) && Newitem["Address"].ToString() != "")
                                {
                                    SitesList.Address = Newitem["Address"].ToString().Replace("'", "''").Trim();
                                }

                                if (!(Newitem["city"] is DBNull) && Newitem["city"].ToString() != "")
                                {
                                    SitesList.city = Newitem["city"].ToString().Replace("'", "''").Trim();
                                }

                                if (!(Newitem["Zip"] is DBNull) && Newitem["Zip"].ToString() != "")
                                {
                                    SitesList.Zip = Newitem["Zip"].ToString();
                                }

                                // ADd on 19 Nov 2019 
                                if (!(Newitem["MaintainInventoryAtJob"] is DBNull) && Newitem["MaintainInventoryAtJob"].ToString() != "")
                                {
                                    SitesList.MaintainInventoryAtJob = Convert.ToBoolean(Newitem["MaintainInventoryAtJob"]);
                                }

                                // Add on 27 JAN 2020.
                                if (!(Newitem["FloorLevel"] is DBNull) && Newitem["FloorLevel"].ToString() != "")
                                {
                                    SitesList.FloorLevel = Newitem["FloorLevel"].ToString().Replace("'", "''").Trim();
                                }

                                if (!(Newitem["GPS"] is DBNull) && Newitem["GPS"].ToString() != "")
                                {
                                    SitesList.GPS = Newitem["GPS"].ToString().Replace("'", "''").Trim();
                                }

                                // Add on 19 DEC 2024
                                if (!(Newitem["CycloneManagement"] is DBNull) && Newitem["CycloneManagement"].ToString() != "")
                                {
                                    SitesList.CycloneManagement = Convert.ToBoolean(Newitem["CycloneManagement"]);
                                }

                                if (!(Newitem["PipeLineInvHide"] is DBNull) && Newitem["PipeLineInvHide"].ToString() != "")
                                {
                                    SitesList.PipeLineInvHide = Convert.ToBoolean(Newitem["PipeLineInvHide"]);
                                }
                            }
                        }
                    }

                    if (!(item["SiteLocation"] is DBNull) && item["SiteLocation"].ToString() != "")
                    {
                        SitesList.SiteLocation = item["SiteLocation"].ToString().Replace("'", "''").Trim();
                    }

                    GetAllSitesList.Add(SitesList);
                }
            }

            return GetAllSitesList;
        }
        List<ddlScaffoldDesign> GetAllDesign = new List<ddlScaffoldDesign> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod]
        public List<ddlScaffoldDesign> GetAllScaffoldDesign(string id)
        {
            //string Sitequery = "select distinct(DesignType) from tblDesign where DesignStatus = 'Active' and DesignType <> '' and DesignType is not null and ClientId = '" + databaseCon.Decrypt(id) + "'";
            string Sitequery = "select distinct DesignType ,ClientId from tblDesign where DesignStatus = 'Active' and DesignType <> '' and DesignType is not null and ClientId = '" + id + "'";
            DataSet ds = null;

            ds = databaseCon.GetDataSet(Sitequery);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ddlScaffoldDesign DesignList = new ddlScaffoldDesign();
                    if (!(item["DesignType"] is DBNull) && item["DesignType"].ToString() != "")
                    {
                        DesignList.DesignType = item["DesignType"].ToString().Replace("'", "''").Trim();
                    }
                    if (!(item["ClientID"] is DBNull))
                    {
                        DesignList.ClientID = (int)(item["ClientId"] as int?);
                    }
                    GetAllDesign.Add(DesignList);
                }
            }

            return GetAllDesign;

        }

        // add code on 27-JAN-2020.( For Floor Level)
        List<ddlFloorLevel> GetAllFloorLevel = new List<ddlFloorLevel> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod]
        public List<ddlFloorLevel> GetAllScaffoldFloorLevel(string id)
        {
            // string Floorquery = "Select Category,FloorLevel,ClientID,SiteLocId,FloorLevelStatus, SortOrder from(Select Case When TblFloorLevel.Category = 'GPS' then 1 when TblFloorLevel.Category = 'Floor Level' then(Select Count(*) from tblSiteImages Where TblSiteImages.FloorLevel = TBlFloorLEvel.FloorLevel and  TBlFloorLEvel.SiteLocID = TblSiteImages.SiteId) Else 0 End as FloorExists, TblFloorLevel.Category, TblFloorLevel.FloorLevel, TblFloorLevel.ClientID, TblFloorLevel.SiteLocId, TblFloorLevel.FloorLevelStatus, TblFloorLevel.SortOrder from TblFloorLevel join tblSitelocation On TBlFloorLEvel.SiteLocID = TblSiteLocation.SiteLocId and TblFloorLevel.Category = Case when Isnull(TblSiteLocation.FloorLevel, 0) = 1 then  'Floor Level' else 'GPS' End and FloorLevelStatus = 'Active' where TblFloorLevel.ClientID = '" + id + "'" + " ) FloorLevelData Where FloorExists > 0 ";

            //string Floorquery = "Select Category,FloorLevel,ClientID,SiteLocId,FloorLevelStatus, SortOrder,LocationID,BuildingID,FloorLevelId from (Select Case When TblFloorLevel.Category = 'GPS' then 1 when TblFloorLevel.Category = 'Floor Level' then(Select Count(*) from tblSiteImages Where TblSiteImages.FloorLevelid = TBlFloorLEvel.FloorLevelid and  TBlFloorLEvel.SiteLocID = TblSiteImages.SiteId) Else 0 End as FloorExists, TblFloorLevel.Category, TblFloorLevel.FloorLevel, TblFloorLevel.ClientID, TblFloorLevel.SiteLocId,TblFloorLevel.LocationID,TblFloorLevel.BuildingID,TblFloorLevel.FloorLevelId, TblFloorLevel.FloorLevelStatus, TblFloorLevel.SortOrder from TblFloorLevel join tblSitelocation On TBlFloorLEvel.SiteLocID = TblSiteLocation.SiteLocId and TblFloorLevel.Category = Case when Isnull(TblSiteLocation.FloorLevel, 0) = 1 then  'Floor Level' else 'GPS' End and FloorLevelStatus = 'Active' where TblFloorLevel.ClientID = '" + id + "'" + " ) FloorLevelData Where FloorExists > 0 ";
            string Floorquery = "Select Category,FloorLevel,ClientID,SiteLocId,FloorLevelStatus, SortOrder,LocationID,BuildingID,FloorLevelId from (Select Case When TblFloorLevel.Category = 'GPS' then 1 when TblFloorLevel.Category = 'Floor Level' then(Select Count(1) from tblSiteImages Where TblSiteImages.FloorLevelid = TBlFloorLEvel.FloorLevelid and  TBlFloorLEvel.SiteLocID = TblSiteImages.SiteId) Else 0 End as FloorExists, TblFloorLevel.Category, TblFloorLevel.FloorLevel, TblFloorLevel.ClientID, TblFloorLevel.SiteLocId,TblFloorLevel.LocationID,TblFloorLevel.BuildingID,TblFloorLevel.FloorLevelId, TblFloorLevel.FloorLevelStatus, TblFloorLevel.SortOrder from TblFloorLevel join tblSitelocation On TBlFloorLEvel.SiteLocID = TblSiteLocation.SiteLocId and TblFloorLevel.Category = Case when Isnull(TblSiteLocation.FloorLevel, 0) = 1 then  'Floor Level' else 'GPS' End and FloorLevelStatus = 'Active' where TblFloorLevel.ClientID = '" + id + "'" + " ) FloorLevelData Where FloorExists > 0 ";

            DataSet ds = null;

            ds = databaseCon.GetDataSet(Floorquery);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ddlFloorLevel FloorLevelList = new ddlFloorLevel();
                    if (!(item["Category"] is DBNull) && item["Category"].ToString() != "")
                    {
                        FloorLevelList.Category = item["Category"].ToString().Replace("'", "''").Trim();
                    }

                    if (!(item["FloorLevel"] is DBNull) && item["FloorLevel"].ToString() != "")
                    {
                        FloorLevelList.FloorLevel = item["FloorLevel"].ToString().Replace("'", "''").Trim();
                    }

                    if (!(item["ClientID"] is DBNull) && item["ClientID"].ToString() != "")
                    {
                        FloorLevelList.ClientID = Convert.ToInt32(item["ClientID"].ToString());
                    }

                    if (!(item["SiteLocId"] is DBNull) && item["SiteLocId"].ToString() != "")
                    {
                        FloorLevelList.SiteLocId = Convert.ToInt32(item["SiteLocId"].ToString());
                    }

                    if (!(item["FloorLevelStatus"] is DBNull) && item["FloorLevelStatus"].ToString() != "")
                    {
                        FloorLevelList.FloorLevelStatus = item["FloorLevelStatus"].ToString().Replace("'", "''").Trim();
                    }

                    if (!(item["SortOrder"] is DBNull) && item["SortOrder"].ToString() != "")
                    {
                        FloorLevelList.SortOrder = item["SortOrder"].ToString();
                    }
                    if (!(item["FloorLevelId"] is DBNull) && item["FloorLevelId"].ToString() != "")
                    {
                        FloorLevelList.FloorLevelId = Convert.ToInt32(item["FloorLevelId"].ToString());
                    }
                    if (!(item["LocationID"] is DBNull) && item["LocationID"].ToString() != "")
                    {
                        FloorLevelList.LocationID = Convert.ToInt32(item["LocationID"].ToString());
                    }
                    if (!(item["BuildingID"] is DBNull) && item["BuildingID"].ToString() != "")
                    {
                        FloorLevelList.BuildingID = Convert.ToInt32(item["BuildingID"].ToString());
                    }

                    GetAllFloorLevel.Add(FloorLevelList);
                }
            }

            return GetAllFloorLevel;
        }


        List<ddlDutyrating> GetDutyRating = new List<ddlDutyrating> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod]
        public List<ddlDutyrating> GetAllDutyRatingList(string id)
        {
            //string query = "select distinct(DutyRating) from tblDutyRating where DutyRatingStatus='Active' and DutyRating <>'' and DutyRating is not null and  ClientId='" + databaseCon.Decrypt(id) + "'";
            string query = "select distinct DutyRating ,ClientId from tblDutyRating where DutyRatingStatus='Active' and DutyRating <>'' and DutyRating is not null and  ClientId='" + id + "'";
            DataSet ds = null;

            ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ddlDutyrating RatingList = new ddlDutyrating();
                    if (!(item["DutyRating"] is DBNull) && item["DutyRating"].ToString() != "")
                    {
                        RatingList.DutyRating = item["DutyRating"].ToString().Replace("'", "''").Trim();
                    }
                    if (!(item["ClientID"] is DBNull))
                    {
                        RatingList.ClientID = (int)(item["ClientId"] as int?);
                    }

                    GetDutyRating.Add(RatingList);
                }
            }

            return GetDutyRating;
        }


        List<ddlMaterialType> GetMaterialType = new List<ddlMaterialType> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod]
        public List<ddlMaterialType> GetAllMaterialTypeList(string id)
        {
            //string query = "select distinct(MaterialType) from tblMaterialType where MaterialStatus='Active' and MaterialType <>'' and MaterialType is not null and  ClientId='" + databaseCon.Decrypt(id) + "'";
            string query = "select distinct MaterialType ,ClientId from tblMaterialType where MaterialStatus='Active' and MaterialType <>'' and MaterialType is not null and  ClientId='" + id + "'";
            DataSet ds = null;

            ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ddlMaterialType MaterialList = new ddlMaterialType();
                    if (!(item["MaterialType"] is DBNull) && item["MaterialType"].ToString() != "")
                    {
                        MaterialList.MaterialType = item["MaterialType"].ToString().Replace("'", "''").Trim();
                    }
                    if (!(item["ClientID"] is DBNull))
                    {
                        MaterialList.ClientID = (int)(item["ClientId"] as int?);
                    }

                    GetMaterialType.Add(MaterialList);
                }
            }

            return GetMaterialType;
        }

        List<ChecklistDropdown> GetCheckList = new List<ChecklistDropdown> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod]
        public List<ChecklistDropdown> GetAllCheckList(string id, string RID)
        {
            //string Query = "select distinct tblCheckList.checklistID,checklistdescription from tblchecklist join tblcheckListContent on tblchecklist.ChecklistId = tblcheckListContent.ChecklistId and  tblCheckList.ClientId=tblchecklistcontent.clientid where tblCheckList.clientid='" + databaseCon.Decrypt(id) + "'";
            //string Query = "select distinct tblCheckList.checklistID,checklistdescription from tblchecklist join tblcheckListContent on tblchecklist.ChecklistId = tblcheckListContent.ChecklistId and  tblCheckList.ClientId=tblchecklistcontent.clientid where tblCheckList.clientid='" + id + "' AND tblCheckList.Status = 'Active' ";
            string Query = "select tblRegister.checklistID, checklistdescription from tblchecklist join tblRegister on  tblRegister.checklistID = tblchecklist.checklistID where tblRegister.Registerid = '" + databaseCon.Decrypt(RID) + "' and tblchecklist.status = 'Active'";
            DataSet ds = null;
            ds = databaseCon.GetDataSet(Query);

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ChecklistDropdown checkListData = new ChecklistDropdown();
                    if (!(item["checklistID"] is DBNull) && item["checklistID"].ToString() != "")
                    {
                        checkListData.checklistID = Convert.ToInt32(item["checklistID"].ToString());
                    }

                    if (!(item["checklistdescription"] is DBNull) && item["checklistdescription"].ToString() != "")
                    {
                        checkListData.checklistdescription = item["checklistdescription"].ToString().Replace("'", "''").Trim();
                    }

                    GetCheckList.Add(checkListData);
                }
            }

            return GetCheckList;
        }



        List<ddlLocation> GetLocationList = new List<ddlLocation> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod]
        public List<ddlLocation> GetAllLocationList(string id, string siteLocId)
        {
            string Query = "select area,areaid  from tblarea where AreaStatus='Active' and  Area <>'' and Area is not null and ClientID='" + id + "' and siteLocId='" + siteLocId + "'";
            DataSet ds = null;
            ds = databaseCon.GetDataSet(Query);

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ddlLocation LocationListData = new ddlLocation();
                    if (!(item["areaid"] is DBNull) && item["areaid"].ToString() != "")
                    {
                        LocationListData.areaid = item["areaid"].ToString();

                        GetAllBuildingList(id, item["areaid"].ToString(), item["area"].ToString());
                    }

                    if (!(item["area"] is DBNull) && item["area"].ToString() != "")
                    {
                        LocationListData.area = item["area"].ToString();
                    }

                    LocationListData.SiteLocId = siteLocId;

                    GetLocationList.Add(LocationListData);
                }
            }

            return GetLocationList;
        }


        List<ddbuilding> GetBuildingList = new List<ddbuilding> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod]
        public List<ddbuilding> GetAllBuildingList(string id, string AreaId, string Area)
        {
            string Query = "select buildingid,buildingno  from tblbuilding where BuildingStatus='Active' and ClientID='" + id + "' and Areaid='" + AreaId + "'";
            DataSet ds = null;
            ds = databaseCon.GetDataSet(Query);

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ddbuilding BuildingListData = new ddbuilding();
                    if (!(item["buildingid"] is DBNull) && item["buildingid"].ToString() != "")
                    {
                        BuildingListData.BuildingID = item["buildingid"].ToString();
                        GetAllAssetsList(id, item["buildingid"].ToString(), item["buildingno"].ToString());
                    }

                    if (!(item["buildingno"] is DBNull) && item["buildingno"].ToString() != "")
                    {
                        BuildingListData.BuildingNo = item["buildingno"].ToString();
                    }

                    BuildingListData.Area = Area;
                    BuildingListData.areaid = AreaId;

                    GetBuildingList.Add(BuildingListData);
                }
            }

            return GetBuildingList;
        }

        List<ddassets> GetAssetList = new List<ddassets> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod]
        public List<ddassets> GetAllAssetsList(string id, string BuildId, string BuildNo)
        {
            string Query = "select Assetid,Assetno  from tblasset where AssetStatus='Active' and ClientID ='" + id + "' and buildingid='" + BuildId + "'";
            DataSet ds = null;
            ds = databaseCon.GetDataSet(Query);

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ddassets AssetListData = new ddassets();
                    if (!(item["Assetid"] is DBNull) && item["Assetid"].ToString() != "")
                    {
                        AssetListData.Assetid = Convert.ToInt32(item["Assetid"].ToString());

                        string query;
                        query = "select AssetDesc  from tblAsset where  ClientID='" + id + "' and assetid='" + item["Assetid"].ToString() + "'";
                        DataTable dtassetdesc = databaseCon.GetData(query);

                        if (dtassetdesc.Rows.Count != 0)
                        {
                            AssetListData.assetsdesc = Convert.ToString(dtassetdesc.Rows[0]["AssetDesc"]);

                        }
                        else
                        {
                            AssetListData.assetsdesc = "";

                        }
                    }

                    if (!(item["Assetno"] is DBNull) && item["Assetno"].ToString() != "")
                    {
                        AssetListData.Assetno = item["Assetno"].ToString();
                    }

                    AssetListData.BuildNo = BuildNo;
                    AssetListData.BuildingID = Convert.ToInt32(BuildId);
                    AssetListData.ClientID = Convert.ToInt32(id);


                    GetAssetList.Add(AssetListData);
                }
            }

            return GetAssetList;
        }

        List<SupervisorDropdown> GetSupivisor = new List<SupervisorDropdown> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod]
        public List<SupervisorDropdown> GetSupivisorList(string id)
        {
            //string Query = "select distinct tblCheckList.checklistID,checklistdescription from tblchecklist join tblcheckListContent on tblchecklist.ChecklistId = tblcheckListContent.ChecklistId and  tblCheckList.ClientId=tblchecklistcontent.clientid where tblCheckList.clientid='" + databaseCon.Decrypt(id) + "'";
            string Query = "select UserId, firstName+' ' + LastName as Supervisor,tbluser.ClientId from tblUser join tblUserCategory on tblUser.ClientId = tblUserCategory.ClientId and tblUser.CategoryId = tblUserCategory.UserCatId  where Category = 'Supervisor' and tbluser.ClientId = '" + id + "' order by Supervisor";
            DataSet ds = null;
            ds = databaseCon.GetDataSet(Query);

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    SupervisorDropdown supervisorData = new SupervisorDropdown();
                    if (!(item["UserId"] is DBNull) && item["UserId"].ToString() != "")
                    {
                        supervisorData.UserID = Convert.ToInt32(item["UserId"].ToString());
                    }

                    if (!(item["Supervisor"] is DBNull) && item["Supervisor"].ToString() != "")
                    {
                        supervisorData.Supervisor = item["Supervisor"].ToString().Replace("'", "''").Trim();
                    }
                    if (!(item["ClientID"] is DBNull))
                    {
                        supervisorData.ClientID = (int)(item["ClientId"] as int?);
                    }

                    GetSupivisor.Add(supervisorData);
                }
            }

            return GetSupivisor;
        }

        List<ScaffoldOwnerDropdown> GetScaffoldOwner = new List<ScaffoldOwnerDropdown> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod]
        public List<ScaffoldOwnerDropdown> GetScaffoldOwnerList(string id)
        {
            string Query = "select distinct ScaffoldOwner from tblRegister where ClientId='" + id + "' and ScaffoldOwner is not null and ScaffoldOwner!='' order by ScaffoldOwner";

            DataSet ds = null;
            ds = databaseCon.GetDataSet(Query);

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ScaffoldOwnerDropdown ScaffoldOwnerData = new ScaffoldOwnerDropdown();
                    if (!(item["ScaffoldOwner"] is DBNull) && item["ScaffoldOwner"].ToString() != "")
                    {
                        ScaffoldOwnerData.ScaffoldOwner = item["ScaffoldOwner"].ToString().Replace("'", "''");
                    }

                    GetScaffoldOwner.Add(ScaffoldOwnerData);
                }
            }

            return GetScaffoldOwner;
        }

        // Add On 16 Nov 2019

        List<SubOwnerRepDropdown> ddlSubownerRepdropdown = new List<SubOwnerRepDropdown> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod]
        public List<SubOwnerRepDropdown> GetSubOwnerRepDropdown(string ClientID)
        {
            string query = "select distinct discipline from  tblRegister where ClientId = '" + ClientID + "' and discipline is not null and discipline!= '' Order by discipline";

            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    SubOwnerRepDropdown pro = new SubOwnerRepDropdown();
                    if (!(item["discipline"] is DBNull) && item["discipline"].ToString() != "")
                    {
                        pro.subOwnerRep = item["discipline"].ToString();
                    }

                    ddlSubownerRepdropdown.Add(pro);
                }
            }
            return ddlSubownerRepdropdown;
        }

        // Add 13 Aug 2020

        List<WorkGroupsDropdown> ddlWorkGroupsdropdown = new List<WorkGroupsDropdown> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod]
        public List<WorkGroupsDropdown> GetWorkGroupsDropdown(string ClientID,string Siteid)
        {
            string query = "select distinct WorkGroup,SiteLocId ,WorkGroupID,ClientID from  tblWorkGroup where ClientId = '" + ClientID + "' and  SiteLocId = '" + databaseCon.Decrypt(Siteid) + "' and WorkGroup is not null and WorkGroup!= '' and Status=1 Order by WorkGroup";

            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    WorkGroupsDropdown pro = new WorkGroupsDropdown();
                    if (!(item["WorkGroup"] is DBNull) && item["WorkGroup"].ToString() != "")
                    {
                        pro.WorkGroups = item["WorkGroup"].ToString();
                    }
                    if (!(item["SiteLocId"] is DBNull) && item["SiteLocId"].ToString() != "")
                    {
                        pro.SiteLocID = item["SiteLocId"].ToString();
                    }
                    if (!(item["ClientID"] is DBNull) && item["ClientID"].ToString() != "")
                    {
                        pro.ClientID = item["ClientID"].ToString();
                    }
                    if (!(item["WorkGroupID"] is DBNull) && item["WorkGroupID"].ToString() != "")
                    {
                        pro.WorkGroupID = item["WorkGroupID"].ToString();
                    }
                    ddlWorkGroupsdropdown.Add(pro);
                }
            }
            return ddlWorkGroupsdropdown;
        }
        List<SubGroupsDropdown> ddlSubGroupsdropdown = new List<SubGroupsDropdown> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod]
        public List<SubGroupsDropdown> GetSubGroupsDropdown(string ClientID, string Siteid)
        {
            string query = "select distinct SubGroupId,WorkGroupId,SubGroupName,SiteLocId ,ClientID from  TblSubGroups where ClientId = '" + ClientID + "' and  SiteLocId = '" + databaseCon.Decrypt(Siteid) + "' and SubGroupName is not null and SubGroupName!= '' and Status=1 Order by SubGroupName";

            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    SubGroupsDropdown pro = new SubGroupsDropdown();
                    if (!(item["SubGroupId"] is DBNull) && item["SubGroupId"].ToString() != "")
                    {
                        pro.SubGroupId = item["SubGroupId"].ToString();
                    }
                    if (!(item["WorkGroupId"] is DBNull) && item["WorkGroupId"].ToString() != "")
                    {
                        pro.WorkGroupId = item["WorkGroupId"].ToString();
                    }
                    if (!(item["SubGroupName"] is DBNull) && item["SubGroupName"].ToString() != "")
                    {
                        pro.SubGroupName = item["SubGroupName"].ToString();
                    }
                    if (!(item["SiteLocId"] is DBNull) && item["SiteLocId"].ToString() != "")
                    {
                        pro.SiteLocID = item["SiteLocId"].ToString();
                    }
                    if (!(item["ClientID"] is DBNull) && item["ClientID"].ToString() != "")
                    {
                        pro.ClientID = item["ClientID"].ToString();
                    }

                    ddlSubGroupsdropdown.Add(pro);
                }
            }
            return ddlSubGroupsdropdown;
        }
        List<RequestedByDropdown> ddlRequestedBydropdown = new List<RequestedByDropdown> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod]
        public List<RequestedByDropdown> GetRequestedByDropdown(string ClientID, string Siteid)
        {
            string query = "select distinct RequestById,RequestBy,SiteLocId,ClientID from  TblRequestBy where ClientId = '" + ClientID + "' and  SiteLocId = '" + databaseCon.Decrypt(Siteid) + "' and RequestBy is not null and RequestBy!= '' Order by RequestBy";

            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    RequestedByDropdown pro = new RequestedByDropdown();
                    if (!(item["RequestById"] is DBNull) && item["RequestById"].ToString() != "")
                    {
                        pro.RequestById = item["RequestById"].ToString();
                    }
                    if (!(item["RequestBy"] is DBNull) && item["RequestBy"].ToString() != "")
                    {
                        pro.RequestBy = item["RequestBy"].ToString();
                    }
                    if (!(item["SiteLocId"] is DBNull) && item["SiteLocId"].ToString() != "")
                    {
                        pro.SiteLocID = item["SiteLocId"].ToString();
                    }
                    if (!(item["ClientID"] is DBNull) && item["ClientID"].ToString() != "")
                    {
                        pro.ClientID = item["ClientID"].ToString();
                    }
                    ddlRequestedBydropdown.Add(pro);
                }
            }
            return ddlRequestedBydropdown;
        }



        List<RegisterListdropdown> ddlRegisterListdropdown = new List<RegisterListdropdown> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod]
        public List<RegisterListdropdown> GetRegisterListDetails(string ClientID, string Siteid, string RID)
        {
            string query = $"exec Pro_TBLREGISTER_GET_FOR_OFFLINE '{databaseCon.Decrypt(RID)}', '{ClientID}', '{databaseCon.Decrypt(Siteid)}', '', '', '', '1', '0', '', '', '', ''";

            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);

            List<RegisterListdropdown> ddlRegisterListdropdown = new List<RegisterListdropdown>();

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    RegisterListdropdown pro = new RegisterListdropdown();
                    if (!(item["Description"] is DBNull) && !string.IsNullOrEmpty(item["Description"].ToString()))
                    {
                        pro.Description = item["Description"].ToString();
                    }

                    if (!(item["REGISTERId"] is DBNull) && !string.IsNullOrEmpty(item["REGISTERId"].ToString()))
                    {
                        pro.REGISTERId = Convert.ToInt32(item["REGISTERId"]);
                    }
                    if (!(item["ShowRegisterID"] is DBNull) && !string.IsNullOrEmpty(item["ShowRegisterID"].ToString()))
                    {
                        pro.ShowRegisterID = item["ShowRegisterID"].ToString();
                    }
                    if (!(item["SiteLocation"] is DBNull) && !string.IsNullOrEmpty(item["SiteLocation"].ToString()))
                    {
                        pro.SiteLocation = item["SiteLocation"].ToString();
                    }

                    if (!(item["SiteLocID"] is DBNull))
                    {
                        pro.SiteLocID = item["SiteLocID"] as int?;
                    }

                    if (!(item["SiteLocationStatus"] is DBNull) && !string.IsNullOrEmpty(item["SiteLocationStatus"].ToString()))
                    {
                        pro.SiteLocationStatus = item["SiteLocationStatus"].ToString();
                    }

                    if (!(item["Description"] is DBNull) && !string.IsNullOrEmpty(item["Description"].ToString()))
                    {
                        pro.Description = item["Description"].ToString();
                    }
                    if (!(item["LocationId"] is DBNull))
                    {
                        pro.LocationId = item["LocationId"] as int?;
                    }
                    if (!(item["Location"] is DBNull) && !string.IsNullOrEmpty(item["Location"].ToString()))
                    {
                        pro.Location = item["Location"].ToString();
                    }
                    if (!(item["LocationStatus"] is DBNull) && !string.IsNullOrEmpty(item["LocationStatus"].ToString()))
                    {
                        pro.LocationStatus = item["LocationStatus"].ToString();
                    }
                    if (!(item["ClientID"] is DBNull))
                    {
                        pro.ClientID = item["ClientID"] as int?;
                    }
                    if (!(item["InspectedBy"] is DBNull) && !string.IsNullOrEmpty(item["InspectedBy"].ToString()))
                    {
                        pro.InspectedBy = item["InspectedBy"].ToString();
                    }

                    if (!(item["DesignType"] is DBNull) && !string.IsNullOrEmpty(item["DesignType"].ToString()))
                    {
                        pro.DesignType = item["DesignType"].ToString();
                    }

                    if (!(item["DutyRating"] is DBNull) && !string.IsNullOrEmpty(item["DutyRating"].ToString()))
                    {
                        pro.DutyRating = item["DutyRating"].ToString();
                    }

                    if (!(item["MaterialType"] is DBNull) && !string.IsNullOrEmpty(item["MaterialType"].ToString()))
                    {
                        pro.MaterialType = item["MaterialType"].ToString();
                    }
                    DateTime parsedDate;
                    if (item["DateTagged"] != DBNull.Value &&
                        !string.IsNullOrWhiteSpace(item["DateTagged"].ToString()) &&
                        DateTime.TryParse(item["DateTagged"].ToString(), out parsedDate))
                    {
                        pro.DateTagged = parsedDate.ToString("dd-MMM-yyyy");
                    }
                    else
                    {
                        pro.DateTagged = "";
                    }




                    if (!(item["DateTaggedTXT"] is DBNull) && !string.IsNullOrEmpty(item["DateTaggedTXT"].ToString()))
                    {
                        pro.DateTaggedTXT = item["DateTaggedTXT"].ToString();
                    }

                    if (!(item["DateOFDismantle"] is DBNull))
                    {
                        pro.DateOFDismantle = Convert.ToDateTime(item["DateOFDismantle"]);
                    }

                    if (!(item["HireDaysToDate"] is DBNull))
                    {
                        pro.HireDaysToDate = item["HireDaysToDate"] as int?;
                    }

                    if (!(item["TotalHireDays"] is DBNull))
                    {
                        pro.TotalHireDays = item["TotalHireDays"] as int?;
                    }

                    if (!(item["RecentInspDate"] is DBNull) && !string.IsNullOrEmpty(item["RecentInspDate"].ToString()))
                    {
                        pro.RecentInspDate = item["RecentInspDate"].ToString();
                    }

                    if (!(item["DaysSinceLastInsp"] is DBNull) && !string.IsNullOrEmpty(item["DaysSinceLastInsp"].ToString()))
                    {
                        pro.DaysSinceLastInsp = item["DaysSinceLastInsp"].ToString();
                    }

                    if (!(item["Status"] is DBNull) && !string.IsNullOrEmpty(item["Status"].ToString()))
                    {
                        pro.Status = item["Status"].ToString();
                    }

                    if (!(item["ExpectedStartDate"] is DBNull))
                    {
                        pro.ExpectedStartDate = Convert.ToDateTime(item["ExpectedStartDate"]);
                    }

                    if (!(item["Image"] is DBNull) && !string.IsNullOrEmpty(item["Image"].ToString()))
                    {
                        pro.Image = item["Image"].ToString();
                    }

                    if (!(item["ClientRef"] is DBNull) && !string.IsNullOrEmpty(item["ClientRef"].ToString()))
                    {
                        pro.ClientRef = item["ClientRef"].ToString();
                    }

                    if (!(item["InspTimeframe"] is DBNull) && !string.IsNullOrEmpty(item["InspTimeframe"].ToString()))
                    {
                        pro.InspTimeframe = item["InspTimeframe"].ToString();
                    }

                    if (!(item["OwnerName"] is DBNull) && !string.IsNullOrEmpty(item["OwnerName"].ToString()))
                    {
                        pro.OwnerName = item["OwnerName"].ToString();
                    }

                    if (!(item["NoOfItems"] is DBNull) && !string.IsNullOrEmpty(item["NoOfItems"].ToString()))
                    {
                        pro.NoOfItems = item["NoOfItems"].ToString();
                    }

                    if (!(item["Tonnage"] is DBNull))
                    {
                        pro.Tonnage = item["Tonnage"] as decimal?;
                    }

                    if (!(item["PhotoAngle"] is DBNull) && !string.IsNullOrEmpty(item["PhotoAngle"].ToString()))
                    {
                        pro.PhotoAngle = item["PhotoAngle"].ToString();
                    }

                    if (!(item["BuildingID"] is DBNull))
                    {
                        pro.BuildingID = item["BuildingID"] as int?;
                    }

                    if (!(item["BuildingNo"] is DBNull) && !string.IsNullOrEmpty(item["BuildingNo"].ToString()))
                    {
                        pro.BuildingNo = item["BuildingNo"].ToString();
                    }

                    if (!(item["BuildingStatus"] is DBNull) && !string.IsNullOrEmpty(item["BuildingStatus"].ToString()))
                    {
                        pro.BuildingStatus = item["BuildingStatus"].ToString();
                    }

                    if (!(item["AssetId"] is DBNull))
                    {
                        pro.AssetId = item["AssetId"] as int?;
                    }

                    if (!(item["AssetNo"] is DBNull) && !string.IsNullOrEmpty(item["AssetNo"].ToString()))
                    {
                        pro.AssetNo = item["AssetNo"].ToString();
                    }

                    if (!(item["AssetDesc"] is DBNull) && !string.IsNullOrEmpty(item["AssetDesc"].ToString()))
                    {
                        pro.AssetDesc = item["AssetDesc"].ToString();
                    }

                    if (!(item["AssetStatus"] is DBNull) && !string.IsNullOrEmpty(item["AssetStatus"].ToString()))
                    {
                        pro.AssetStatus = item["AssetStatus"].ToString();
                    }

                    if (!(item["Length"] is DBNull))
                    {
                        pro.Length = item["Length"] as double?;
                    }

                    if (!(item["Height"] is DBNull))
                    {
                        pro.Height = item["Height"] as double?;
                    }

                    if (!(item["Width"] is DBNull))
                    {
                        pro.Width = item["Width"] as double?;
                    }

                    if (!(item["NoOfWorkingDecks"] is DBNull) && !string.IsNullOrEmpty(item["NoOfWorkingDecks"].ToString()))
                    {
                        pro.NoOfWorkingDecks = item["NoOfWorkingDecks"].ToString();
                    }

                    if (!(item["FloorLevel"] is DBNull) && !string.IsNullOrEmpty(item["FloorLevel"].ToString()))
                    {
                        pro.FloorLevel = item["FloorLevel"].ToString();
                    }

                    if (!(item["OnlineApplied"] is DBNull))
                    {
                        pro.OnlineApplied = item["OnlineApplied"] as bool?;
                    }

                    if (!(item["CheckListId"] is DBNull))
                    {
                        pro.CheckListId = item["CheckListId"] as int?;
                    }

                    if (!(item["checklistdescription"] is DBNull) && !string.IsNullOrEmpty(item["checklistdescription"].ToString()))
                    {
                        pro.checklistdescription = item["checklistdescription"].ToString();
                    }

                    if (!(item["PlanningStatus"] is DBNull) && !string.IsNullOrEmpty(item["PlanningStatus"].ToString()))
                    {
                        pro.PlanningStatus = item["PlanningStatus"].ToString();
                    }

                    if (!(item["WONumber"] is DBNull) && !string.IsNullOrEmpty(item["WONumber"].ToString()))
                    {
                        pro.WONumber = item["WONumber"].ToString();
                    }

                    if (!(item["ClientScaffref"] is DBNull) && !string.IsNullOrEmpty(item["ClientScaffref"].ToString()))
                    {
                        pro.ClientScaffref = item["ClientScaffref"].ToString();
                    }

                    if (!(item["ScafType"] is DBNull) && !string.IsNullOrEmpty(item["ScafType"].ToString()))
                    {
                        pro.ScafType = item["ScafType"].ToString();
                    }

                    if (!(item["Lat"] is DBNull) && !string.IsNullOrEmpty(item["Lat"].ToString()))
                    {
                        pro.Lat = item["Lat"].ToString();
                    }

                    if (!(item["Long"] is DBNull) && !string.IsNullOrEmpty(item["Long"].ToString()))
                    {
                        pro.Long = item["Long"].ToString();
                    }

                    if (!(item["LockedIn"] is DBNull))
                    {
                        pro.LockedIn = item["LockedIn"] as bool?;
                    }

                    if (!(item["OptimizeStatus"] is DBNull) && !string.IsNullOrEmpty(item["OptimizeStatus"].ToString()))
                    {
                        pro.OptimizeStatus = item["OptimizeStatus"].ToString();
                    }

                    if (!(item["PlannedStandingDuration"] is DBNull))
                    {
                        pro.PlannedStandingDuration = item["PlannedStandingDuration"] as int?;
                    }

                    if (!(item["CrewSize"] is DBNull))
                    {
                        pro.CrewSize = item["CrewSize"] as int?;
                    }

                    if (!(item["ManHrsErect"] is DBNull))
                    {
                        pro.ManHrsErect = item["ManHrsErect"] as decimal?;
                    }

                    if (!(item["ManHrsDismantle"] is DBNull))
                    {
                        pro.ManHrsDismantle = item["ManHrsDismantle"] as decimal?;
                    }

                    if (!(item["TotalLabourErect"] is DBNull))
                    {
                        pro.TotalLabourErect = item["TotalLabourErect"] as decimal?;
                    }

                    if (!(item["TotalLabourDismantle"] is DBNull))
                    {
                        pro.TotalLabourDismantle = item["TotalLabourDismantle"] as decimal?;
                    }

                    if (!(item["TotalErectCount"] is DBNull))
                    {
                        pro.TotalErectCount = item["TotalErectCount"] as decimal?;
                    }

                    if (!(item["TotalDismantleCount"] is DBNull))
                    {
                        pro.TotalDismantleCount = item["TotalDismantleCount"] as decimal?;
                    }

                    if (!(item["TotalLabourErectEdit"] is DBNull))
                    {
                        pro.TotalLabourErectEdit = item["TotalLabourErectEdit"] as decimal?;
                    }

                    if (!(item["TotalLabourDismantleEdit"] is DBNull))
                    {
                        pro.TotalLabourDismantleEdit = item["TotalLabourDismantleEdit"] as decimal?;
                    }

                    if (!(item["TotalErectCountEdit"] is DBNull))
                    {
                        pro.TotalErectCountEdit = item["TotalErectCountEdit"] as decimal?;
                    }

                    if (!(item["TotalDismantleCountEdit"] is DBNull))
                    {
                        pro.TotalDismantleCountEdit = item["TotalDismantleCountEdit"] as decimal?;
                    }

                    if (!(item["UseEnteredValue"] is DBNull))
                    {
                        pro.UseEnteredValue = item["UseEnteredValue"] as bool?;
                    }

                    if (!(item["CycloneAction"] is DBNull) && !string.IsNullOrEmpty(item["CycloneAction"].ToString()))
                    {
                        pro.CycloneAction = item["CycloneAction"].ToString();
                    }

                    if (!(item["Supervisor"] is DBNull) && !string.IsNullOrEmpty(item["Supervisor"].ToString()))
                    {
                        pro.Supervisor = item["Supervisor"].ToString();
                    }

                    if (!(item["Crew"] is DBNull) && !string.IsNullOrEmpty(item["Crew"].ToString()))
                    {
                        pro.Crew = item["Crew"].ToString();
                    }

                    if (!(item["PermitNo"] is DBNull) && !string.IsNullOrEmpty(item["PermitNo"].ToString()))
                    {
                        pro.PermitNo = item["PermitNo"].ToString();
                    }

                    if (!(item["PriorityRiskRating"] is DBNull) && !string.IsNullOrEmpty(item["PriorityRiskRating"].ToString()))
                    {
                        pro.PriorityRiskRating = item["PriorityRiskRating"].ToString();
                    }

                    if (!(item["SupervisorId"] is DBNull))
                    {
                        pro.SupervisorId = item["SupervisorId"] as int?;
                    }

                    if (!(item["Comments"] is DBNull) && !string.IsNullOrEmpty(item["Comments"].ToString()))
                    {
                        pro.Comments = item["Comments"].ToString();
                    }

                    if (!(item["CreatedBy"] is DBNull) && !string.IsNullOrEmpty(item["CreatedBy"].ToString()))
                    {
                        pro.CreatedBy = item["CreatedBy"].ToString();
                    }

                    if (!(item["CreatedOn"] is DBNull))
                    {
                        pro.CreatedOn = Convert.ToDateTime(item["CreatedOn"]);
                    }

                    if (!(item["UpdatedBy"] is DBNull) && !string.IsNullOrEmpty(item["UpdatedBy"].ToString()))
                    {
                        pro.UpdatedBy = item["UpdatedBy"].ToString();
                    }

                    if (!(item["UpdatedOn"] is DBNull))
                    {
                        pro.UpdatedOn = Convert.ToDateTime(item["UpdatedOn"]);
                    }

                    if (!(item["SAPNumber"] is DBNull) && !string.IsNullOrEmpty(item["SAPNumber"].ToString()))
                    {
                        pro.SAPNumber = item["SAPNumber"].ToString();
                    }

                    if (!(item["ScaffoldOwner"] is DBNull) && !string.IsNullOrEmpty(item["ScaffoldOwner"].ToString()))
                    {
                        pro.ScaffoldOwner = item["ScaffoldOwner"].ToString();
                    }

                    if (!(item["DismantleApprovedDate"] is DBNull))
                    {
                        pro.DismantleApprovedDate = Convert.ToDateTime(item["DismantleApprovedDate"]);
                    }

                    if (!(item["DismantleApprovedNotes"] is DBNull) && !string.IsNullOrEmpty(item["DismantleApprovedNotes"].ToString()))
                    {
                        pro.DismantleApprovedNotes = item["DismantleApprovedNotes"].ToString();
                    }

                    if (!(item["Bearing"] is DBNull) && !string.IsNullOrEmpty(item["Bearing"].ToString()))
                    {
                        pro.Bearing = item["Bearing"].ToString();
                    }

                    if (!(item["CycloneComment"] is DBNull) && !string.IsNullOrEmpty(item["CycloneComment"].ToString()))
                    {
                        pro.CycloneComment = item["CycloneComment"].ToString();
                    }

                    if (!(item["HierarchyLevel"] is DBNull) && !string.IsNullOrEmpty(item["HierarchyLevel"].ToString()))
                    {
                        pro.HierarchyLevel = item["HierarchyLevel"].ToString();
                    }

                    if (!(item["HierarchyComment1"] is DBNull) && !string.IsNullOrEmpty(item["HierarchyComment1"].ToString()))
                    {
                        pro.HierarchyComment1 = item["HierarchyComment1"].ToString();
                    }

                    if (!(item["HierarchyComment2"] is DBNull) && !string.IsNullOrEmpty(item["HierarchyComment2"].ToString()))
                    {
                        pro.HierarchyComment2 = item["HierarchyComment2"].ToString();
                    }

                    if (!(item["HierarchyComment3"] is DBNull) && !string.IsNullOrEmpty(item["HierarchyComment3"].ToString()))
                    {
                        pro.HierarchyComment3 = item["HierarchyComment3"].ToString();
                    }

                    if (!(item["HierarchyComment4"] is DBNull) && !string.IsNullOrEmpty(item["HierarchyComment4"].ToString()))
                    {
                        pro.HierarchyComment4 = item["HierarchyComment4"].ToString();
                    }

                    if (!(item["HierarchyComment5"] is DBNull) && !string.IsNullOrEmpty(item["HierarchyComment5"].ToString()))
                    {
                        pro.HierarchyComment5 = item["HierarchyComment5"].ToString();
                    }

                    if (!(item["HierarchySignedOff"] is DBNull))
                    {
                        pro.HierarchySignedOff = item["HierarchySignedOff"] as int?;
                    }

                    if (!(item["LockedInSignedOff"] is DBNull))
                    {
                        pro.LockedInSignedOff = item["LockedInSignedOff"] as int?;
                    }

                    if (!(item["LockedInSignedOffName"] is DBNull) && !string.IsNullOrEmpty(item["LockedInSignedOffName"].ToString()))
                    {
                        pro.LockedInSignedOffName = item["LockedInSignedOffName"].ToString();
                    }

                    if (!(item["QRCodeImage"] is DBNull) && !string.IsNullOrEmpty(item["QRCodeImage"].ToString()))
                    {
                        pro.QRCodeImage = item["QRCodeImage"].ToString();
                    }

                    if (!(item["MaintainInventory"] is DBNull))
                    {
                        pro.MaintainInventory = item["MaintainInventory"] as bool?;
                    }

                    if (!(item["DesignDrawingNo"] is DBNull) && !string.IsNullOrEmpty(item["DesignDrawingNo"].ToString()))
                    {
                        pro.DesignDrawingNo = item["DesignDrawingNo"].ToString();
                    }

                    if (!(item["NumberOfPlatform"] is DBNull) && !string.IsNullOrEmpty(item["NumberOfPlatform"].ToString()))
                    {
                        pro.NumberOfPlatform = item["NumberOfPlatform"].ToString();
                    }

                    if (!(item["EngineeredDesign"] is DBNull) && !string.IsNullOrEmpty(item["EngineeredDesign"].ToString()))
                    {
                        pro.EngineeredDesign = item["EngineeredDesign"].ToString();
                    }

                    if (!(item["discipline"] is DBNull) && !string.IsNullOrEmpty(item["discipline"].ToString()))
                    {
                        pro.discipline = item["discipline"].ToString();
                    }

                    if (!(item["ShiftAllocation"] is DBNull) && !string.IsNullOrEmpty(item["ShiftAllocation"].ToString()))
                    {
                        pro.ShiftAllocation = item["ShiftAllocation"].ToString();
                    }

                    //// Extra Column Add
                    //if (!(item["Plans"] is DBNull) && !string.IsNullOrEmpty(item["Plans"].ToString()))
                    //{
                    //    pro.Plans = item["Plans"].ToString();
                    //}

                    //if (!(item["InventoryCount"] is DBNull))
                    //{
                    //    pro.InventoryCount = Convert.ToInt32(item["InventoryCount"]);
                    //}

                    //if (!(item["AvailableQuantity"] is DBNull))
                    //{
                    //    pro.AvailableQuantity = Convert.ToInt32(item["AvailableQuantity"]);
                    //}

                    //if (!(item["Contractor"] is DBNull) && !string.IsNullOrEmpty(item["Contractor"].ToString()))
                    //{
                    //    pro.Contractor = item["Contractor"].ToString();
                    //}

                    //if (!(item["ContractorRep"] is DBNull) && !string.IsNullOrEmpty(item["ContractorRep"].ToString()))
                    //{
                    //    pro.ContractorRep = item["ContractorRep"].ToString();
                    //}

                    if (!(item["FloorLevelId"] is DBNull))
                    {
                        pro.FloorLevelId = item["FloorLevelId"] as int?;
                    }
                    if (!(item["AssetId"] is DBNull))
                    {
                        pro.AssetId = item["AssetId"] as int?;
                    }

                    if (!(item["WorkGroupid"] is DBNull) && !string.IsNullOrEmpty(item["WorkGroupid"].ToString()))
                    {
                        pro.WorkGroup = item["WorkGroupid"].ToString();
                    }
                    if (!(item["SubGroupID"] is DBNull) && !string.IsNullOrEmpty(item["SubGroupID"].ToString()))
                    {
                        pro.SubGroup = item["SubGroupID"].ToString();
                    }

                    if (!(item["RequestedById"] is DBNull) && !string.IsNullOrEmpty(item["RequestedById"].ToString()))
                    {
                        pro.RequestedBy = item["RequestedById"].ToString();
                    }

                    if (!(item["WithInRangeOfOutBoard"] is DBNull))
                    {
                        pro.WithInRangeOfOutBoard = item["WithInRangeOfOutBoard"] as bool?;
                    }

                    if (!(item["LongTermStructure"] is DBNull))
                    {
                        pro.LongTermStructure = item["LongTermStructure"] as bool?;
                    }

                    if (!(item["IsOfflineCurrent"] is DBNull))
                    {
                        pro.IsOfflineCurrent = item["IsOfflineCurrent"] as bool?;
                    }

                    if (!(item["NoInventory"] is DBNull))
                    {
                        pro.NoInventory = item["NoInventory"] as bool?;
                    }
                    if (!(item["PipelineStartDate"] is DBNull))
                    {
                        pro.PipelineStartDate = Convert.ToDateTime(item["PipelineStartDate"]);
                    }

                    if (!(item["PipeLineNote"] is DBNull) && !string.IsNullOrEmpty(item["PipeLineNote"].ToString()))
                    {
                        pro.PipeLineNote = item["PipeLineNote"].ToString();
                    }

                    if (!(item["ChkRequiresIsolation"] is DBNull))
                    {
                        pro.ChkRequiresIsolation = item["ChkRequiresIsolation"] as bool?;
                    }

                    if (!(item["GoOfflineJobOn"] is DBNull) && !string.IsNullOrEmpty(item["GoOfflineJobOn"].ToString()))
                    {
                        pro.GoOfflineJobOn = item["GoOfflineJobOn"].ToString();
                    }

                    ddlRegisterListdropdown.Add(pro);
                }
            }
            return ddlRegisterListdropdown;
        }
        public List<InventoryRequest> ShowInventoryRequest(string id, string RID)
        {
            //string query = " select  PipeLineDesc,JobId as REGISTERId,Vw_ThirdPartyInventory.InventoryId, Vw_ThirdPartyInventory.MaterialType, Vw_ThirdPartyInventory.ItemCode,Vw_ThirdPartyInventory.ItemDesc,isnull(tblPipelineRequest.Quantity,0) Quantity, cast(AvailableQuantity as int) as AvailableQuantity, '' as CheckQty from(select * from Vw_ThirdPartyInventory where ClientID = '" + id + "' ) Vw_ThirdPartyInventory Left join(select * from tblPipelineRequest ) tblPipelineRequest on Vw_ThirdPartyInventory.InventoryID = tblPipelineRequest.InventoryId order by case when Quantity is null then 1 else 0 end,Vw_ThirdPartyInventory.MaterialType, Vw_ThirdPartyInventory.ItemCode";

            //string query = " select  '' as PipeLineDesc,0 as REGISTERId,Vw_ThirdPartyInventory.InventoryId, Vw_ThirdPartyInventory.MaterialType, Vw_ThirdPartyInventory.ItemCode,Vw_ThirdPartyInventory.ItemDesc,0 as  Quantity, cast(AvailableQuantity as int) as AvailableQuantity, isnull(ItemWeight,0)ItemWeight,0 as TotalWeight from Vw_ThirdPartyInventory where ClientID = '" + id + "' order by  Vw_ThirdPartyInventory.MaterialType, Vw_ThirdPartyInventory.ItemCode,Vw_ThirdPartyInventory.ItemDesc";


            //string query = "select distinct * from ( select  case when jobid is not null then 1 else 0 end as insertedOffline, coalesce(PipeLineDesc,'') as  PipeLineDesc,coalesce(jobid,0) as REGISTERId,Vw_ThirdPartyInventory.InventoryId, Vw_ThirdPartyInventory.MaterialType, Vw_ThirdPartyInventory.ItemCode,Vw_ThirdPartyInventory.ItemDesc,coalesce(Quantity,0) as  Quantity, cast(AvailableQuantity as int) as AvailableQuantity, isnull(ItemWeight,0)ItemWeight,coalesce(Quantity,0)*isnull(ItemWeight,0) as TotalWeight from Vw_ThirdPartyInventory  left join tblPipelineRequest on Vw_ThirdPartyInventory.InventoryId=tblPipelineRequest.InventoryId and Vw_ThirdPartyInventory.ClientID=tblPipelineRequest.ClientID where Vw_ThirdPartyInventory.ClientID = '" + id + "' UNION ALL select 0 as insertedOffline, '' as  PipeLineDesc,0 as REGISTERId,Vw_ThirdPartyInventory.InventoryId, Vw_ThirdPartyInventory.MaterialType, Vw_ThirdPartyInventory.ItemCode,Vw_ThirdPartyInventory.ItemDesc,0 as  Quantity, cast(AvailableQuantity as int) as AvailableQuantity, isnull(ItemWeight,0)ItemWeight,0 as TotalWeight  from Vw_ThirdPartyInventory where  Vw_ThirdPartyInventory.ClientID = '" + id + "' )a order by  MaterialType, ItemCode,ItemDesc";
            //string query = "select distinct * from ( select  case when jobid is not null then 1 else 0 end as insertedOffline, coalesce(PipeLineDesc,'') as  PipeLineDesc,coalesce(jobid,0) as REGISTERId,Vw_ThirdPartyInventory.InventoryId, Vw_ThirdPartyInventory.MaterialType, Vw_ThirdPartyInventory.ItemCode,Vw_ThirdPartyInventory.ItemDesc,coalesce(Quantity,0) as  Quantity, cast(AvailableQuantity as int) as AvailableQuantity, isnull(ItemWeight,0)ItemWeight,coalesce(Quantity,0)*isnull(ItemWeight,0) as TotalWeight from Vw_ThirdPartyInventory  left join (select * from tblPipelineRequest where  jobid = '" + databaseCon.Decrypt(RID) + "')tblPipelineRequest on Vw_ThirdPartyInventory.InventoryId=tblPipelineRequest.InventoryId and Vw_ThirdPartyInventory.ClientID=tblPipelineRequest.ClientID where Vw_ThirdPartyInventory.ClientID = '" + id + "' UNION ALL select 0 as insertedOffline, '' as  PipeLineDesc,0 as REGISTERId,Vw_ThirdPartyInventory.InventoryId, Vw_ThirdPartyInventory.MaterialType, Vw_ThirdPartyInventory.ItemCode,Vw_ThirdPartyInventory.ItemDesc,0 as  Quantity, cast(AvailableQuantity as int) as AvailableQuantity, isnull(ItemWeight,0)ItemWeight,0 as TotalWeight  from Vw_ThirdPartyInventory where  Vw_ThirdPartyInventory.ClientID = '" + id + "' )a order by  MaterialType, ItemCode,ItemDesc";

            string query = "select distinct insertedOffline, PipeLineDesc, REGISTERId, InventoryId, MaterialType, ItemCode, ItemDesc, Quantity, AvailableQuantity, ItemWeight, TotalWeight from ( " +
               "select case when jobid is not null then 1 else 0 end as insertedOffline, coalesce(PipeLineDesc,'') as PipeLineDesc, coalesce(jobid,0) as REGISTERId, Vw_ThirdPartyInventory.InventoryId, Vw_ThirdPartyInventory.MaterialType, Vw_ThirdPartyInventory.ItemCode, Vw_ThirdPartyInventory.ItemDesc, coalesce(Quantity,0) as Quantity, cast(AvailableQuantity as int) as AvailableQuantity, isnull(ItemWeight,0) ItemWeight, coalesce(Quantity,0)*isnull(ItemWeight,0) as TotalWeight " +
               "from Vw_ThirdPartyInventory " +
               "left join tblPipelineRequest on Vw_ThirdPartyInventory.InventoryId = tblPipelineRequest.InventoryId and Vw_ThirdPartyInventory.ClientID = tblPipelineRequest.ClientID and tblPipelineRequest.jobid = '" + databaseCon.Decrypt(RID) + "' " +
               "where Vw_ThirdPartyInventory.ClientID = '" + id + "' " +
               "UNION ALL " +
               "select 0 as insertedOffline, '' as PipeLineDesc, 0 as REGISTERId, Vw_ThirdPartyInventory.InventoryId, Vw_ThirdPartyInventory.MaterialType, Vw_ThirdPartyInventory.ItemCode, Vw_ThirdPartyInventory.ItemDesc, 0 as Quantity, cast(AvailableQuantity as int) as AvailableQuantity, isnull(ItemWeight,0) ItemWeight, 0 as TotalWeight " +
               "from Vw_ThirdPartyInventory where Vw_ThirdPartyInventory.ClientID = '" + id + "' " +
               ") a order by MaterialType, ItemCode, ItemDesc";
            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);

            //string queryzero = "select *  from Vw_ThirdPartyInventory where  Vw_ThirdPartyInventory.ClientID = '" + id + "' order by  MaterialType, ItemCode,ItemDesc";
            //DataSet dszero = null;
            //dszero = databaseCon.GetDataSet(queryzero);


            //string queryreg = "select * from tblPipelineRequest where  jobid = '" + databaseCon.Decrypt(RID) + "' order by  MaterialType, ItemCode,ItemDesc";
            //DataSet dsqueryreg = null;
            //dsqueryreg = databaseCon.GetDataSet(queryreg);


            //string query1 = "insert into TblTrackUploadToCloud(ProcName,Registerid,userid,Status,createdBy,CreatedOn,Query,Response)values('TblPipelineRequest-All Records', '" + databaseCon.Decrypt(RID) + "',0,'Active','','" + DateTime.Now + "','" + query.Replace("'", "''").Trim() + "','" + ds.Tables[0].Rows.Count + "'),('TblPipelineRequest-Registerid Records', '" + databaseCon.Decrypt(RID) + "',0,'Active','','" + DateTime.Now + "','" + queryreg.Replace("'", "''").Trim() + "','" + dsqueryreg.Tables[0].Rows.Count + "'),('TblPipelineRequest-new entries Records', '" + databaseCon.Decrypt(RID) + "',0,'Active','','" + DateTime.Now + "','" + queryzero.Replace("'", "''").Trim() + "','" + dszero.Tables[0].Rows.Count + "')";
            //databaseCon.RunCmd(query1);



            List<InventoryRequest> ddlInventoryRequest = new List<InventoryRequest>();

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    InventoryRequest pro = new InventoryRequest();
                    if (!(item["insertedOffline"] is DBNull) && !string.IsNullOrEmpty(item["insertedOffline"].ToString()))
                    {
                        pro.insertedOffline = Convert.ToBoolean(item["insertedOffline"]);
                    }

                    if (!(item["REGISTERId"] is DBNull) && !string.IsNullOrEmpty(item["REGISTERId"].ToString()))
                    {
                        pro.REGISTERId = Convert.ToInt32(item["REGISTERId"]);
                    }
                    if (!(item["InventoryId"] is DBNull) && !string.IsNullOrEmpty(item["InventoryId"].ToString()))
                    {
                        pro.InventoryId = Convert.ToInt32(item["InventoryId"]);
                    }
                    if (!(item["MaterialType"] is DBNull) && !string.IsNullOrEmpty(item["MaterialType"].ToString()))
                    {
                        pro.MaterialType = item["MaterialType"].ToString();
                    }
                    if (!(item["ItemCode"] is DBNull) && !string.IsNullOrEmpty(item["ItemCode"].ToString()))
                    {
                        pro.ItemCode = item["ItemCode"].ToString();
                    }
                    if (!(item["ItemDesc"] is DBNull) && !string.IsNullOrEmpty(item["ItemDesc"].ToString()))
                    {
                        pro.ItemDesc = item["ItemDesc"].ToString();
                    }

                    if (!(item["Quantity"] is DBNull) && !string.IsNullOrEmpty(item["Quantity"].ToString()))
                    {
                        pro.Quantity = Convert.ToInt32(item["Quantity"]);
                    }
                    if (!(item["AvailableQuantity"] is DBNull) && !string.IsNullOrEmpty(item["AvailableQuantity"].ToString()))
                    {
                        pro.AvailableQuantity = Convert.ToInt32(item["AvailableQuantity"]);
                    }
                    if (!(item["ItemWeight"] is DBNull))
                    {
                        pro.ItemWeight = Convert.ToDouble(item["ItemWeight"]);
                    }
                    if (!(item["TotalWeight"] is DBNull))
                    {
                        pro.TotalWeight = Convert.ToDouble(item["TotalWeight"]);
                    }
                    if (!(item["PipeLineDesc"] is DBNull) && !string.IsNullOrEmpty(item["PipeLineDesc"].ToString()))
                    {
                        pro.PipeLineDesc = item["PipeLineDesc"].ToString();
                    }

                    ddlInventoryRequest.Add(pro);
                }

            }
            return ddlInventoryRequest;
            // Return the data as JSON
            // return Json(ddlInventoryRequest, JsonRequestBehavior.AllowGet);

        }
        public List<InventoryListdropdown> GettblJobInventoryListData(string id, string sitelocid)
        {

            var EndDate = DateTime.Now.Date.ToString("MM-dd-yyyy");
            //string query = "exec Proc_OfflineInvoiceCostReportSurgedate_GenerateReport '" + EndDate + "', '" + EndDate + "', " + databaseCon.Decrypt(id) + ",'" + sitelocid + "','JHC','" + RID + "','All'";
            string addquery = "";
            string query;
            DataTable dt;


            //if (ddlMaterialType != "" && ddlMaterialType != "All")
            //{
            //    addquery = " and Vw_ThirdPartyInventory.MaterialType='" + ddlMaterialType + "' ";

            // }
            query = "select  JobId  as REGISTERId , tblPipelineRequest.MaterialType,ItemCode,ItemDesc, isnull(Quantity,0)Quantity  from tblPipelineRequest join tblregister on tblPipelineRequest.jobid=tblregister.RegisterId where isofflinecurrent=1 and  tblPipelineRequest.clientid = '" + id + "'  and siteid = '" + databaseCon.Decrypt(sitelocid) + "'";

            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);

            List<InventoryListdropdown> ddlInventoryListdropdown = new List<InventoryListdropdown>();

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    InventoryListdropdown pro = new InventoryListdropdown();
                    if (!(item["REGISTERId"] is DBNull) && !string.IsNullOrEmpty(item["REGISTERId"].ToString()))
                    {
                        pro.REGISTERId = Convert.ToInt32(item["REGISTERId"]);
                    }

                    if (!(item["MaterialType"] is DBNull) && !string.IsNullOrEmpty(item["MaterialType"].ToString()))
                    {
                        pro.MaterialType = item["MaterialType"].ToString();
                    }
                    if (!(item["ItemCode"] is DBNull) && !string.IsNullOrEmpty(item["ItemCode"].ToString()))
                    {
                        pro.ItemCode = item["ItemCode"].ToString();
                    }
                    if (!(item["ItemDesc"] is DBNull) && !string.IsNullOrEmpty(item["ItemDesc"].ToString()))
                    {
                        pro.ItemDesc = item["ItemDesc"].ToString();
                    }

                    if (!(item["Quantity"] is DBNull) && !string.IsNullOrEmpty(item["Quantity"].ToString()))
                    {
                        pro.Quantity = Convert.ToInt32(item["Quantity"]);
                    }



                    ddlInventoryListdropdown.Add(pro);
                }
            }

            // Return the data as JSON
            //return Json(ddlInventoryListdropdown, JsonRequestBehavior.AllowGet);
            return ddlInventoryListdropdown;


        }
        //List<RegisterDetails> ddlsiteIndexchanged = new List<RegisterDetails> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod]
        //public List<RegisterDetails> ddlsites_SelectedIndexChanged(string sender, string id, string siteLocId,string RID)
        //{
        //    RegisterDetails sitechange = new RegisterDetails();
        //    if (sender != "")
        //    {
        //        string updatesite = "Select sum(Counts) as TotalRecord from (select Count(*)as Counts from TblSiteMaterial where registerId='" + RID + "' Union select Count(*)as Counts from TblTransaction where [To]='Site' and fromId='" + RID + "' ) Siteselect";
        //        DataTable dtsite = databaseCon.GetData(updatesite);
        //        if (Convert.ToInt32(dtsite.Rows[0]["TotalRecord"]) > 0)
        //        {
        //            string querysite = "select Sitelocid,LocationId from tblregister where ClientId ='" + databaseCon.Decrypt(id) + "' and RegisterId = '" + databaseCon.Decrypt(id) + "'";
        //            DataTable dtgetsite = databaseCon.GetData(querysite);
        //            if (dtgetsite.Rows.Count > 0)
        //            {
        //                sitechange.ddlsitesselected = dtgetsite.Rows[0]["Sitelocid"].ToString();
        //                sitechange.ddlLocationselected = dtgetsite.Rows[0]["LocationId"].ToString();
        //            }

        //        }
        //    }

        //    {
        //        string SItequery = "select Address,city,state,OwnerName,Zip from tblSiteLocation where SiteLocId= '" + siteLocId + "' and ClientID='" + databaseCon.Decrypt(id) + "'";
        //        DataTable dtSiteDetails = databaseCon.GetData(SItequery);
        //        if (dtSiteDetails.Rows.Count != 0)
        //        {
        //            sitechange.txtsiteAddress = Convert.ToString(dtSiteDetails.Rows[0]["Address"]);
        //            sitechange.txtsiteCity = Convert.ToString(dtSiteDetails.Rows[0]["City"]);
        //            sitechange.txtOwnername = Convert.ToString(dtSiteDetails.Rows[0]["OwnerName"]);
        //            sitechange.txtZip = Convert.ToString(dtSiteDetails.Rows[0]["Zip"]);
        //        }
        //        else
        //        {
        //            sitechange.txtsiteAddress = "";
        //            sitechange.txtsiteCity = "";
        //            sitechange.txtOwnername = "";
        //            sitechange.txtZip = "";
        //        }

        //        sitechange.GetLocationList = GetAllLocationList(id, siteLocId);
        //    }

        //    ddlsiteIndexchanged.Add(sitechange);

        //    return ddlsiteIndexchanged;
        //}


        List<RegisterDetails> ddlBuildingSelectChange = new List<RegisterDetails> { };
        List<ddassets> GetAllAssetList = new List<ddassets> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod]
        public List<ddassets> ddbuilding_SelectedIndexChanged(string id, string BuildId)
        {
            RegisterDetails locationChange = new RegisterDetails();
            string SItequery = "select Assetid,Assetno  from tblasset where AssetStatus='Active' and ClientID ='" + databaseCon.Decrypt(id) + "' and buildingid='" + BuildId + "'";

            DataSet ds = null;
            ds = databaseCon.GetDataSet(SItequery);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ddassets AssetList = new ddassets();
                    if (!(item["Assetid"] is DBNull) && item["Assetid"].ToString() != "")
                    {
                        AssetList.Assetid = Convert.ToInt32(item["Assetid"].ToString());
                    }

                    if (!(item["Assetno"] is DBNull) && item["Assetno"].ToString() != "")
                    {
                        AssetList.Assetno = item["Assetno"].ToString();
                    }

                    GetAllAssetList.Add(AssetList);
                }
                //ddassets_SelectedIndexChanged(id, assetId);
            }

            return GetAllAssetList;

        }

        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod]
        public string ddassets_SelectedIndexChanged(string id, string assetId)
        {
            string txtassetsdesc;
            string SItequery = "select AssetDesc  from tblAsset where  ClientID='" + databaseCon.Decrypt(id) + "' and assetid='" + assetId + "'";
            DataTable dtassetdesc = databaseCon.GetData(SItequery);

            if (dtassetdesc.Rows.Count != 0)
            {
                txtassetsdesc = Convert.ToString(dtassetdesc.Rows[0]["AssetDesc"]);
            }
            else
            {
                txtassetsdesc = "";
            }

            return txtassetsdesc;

        }

        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod]
        public string GetDecyptedId(string Id)
        {
            string GetID = databaseCon.Decrypt(Id) + "_" + databaseCon.Encrypt("0");

            return GetID;
        }
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod]
        //public string GetDecryptClientID(string id)
        //{
        //    return databaseCon.Decrypt(id);
        //}
        public Dictionary<string, string> DecryptClientIDs(List<string> ids)
        {
            var decryptedIds = new Dictionary<string, string>();
            foreach (var id in ids)
            {
                decryptedIds[id] = databaseCon.Decrypt(id);
            }
            return decryptedIds;
        }

        [HttpPost]
        public JsonResult GetDecryptClientIDs(List<string> ids)
        {
            var decryptedIds = DecryptClientIDs(ids);
            return Json(decryptedIds);
        }

        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod]
        //public string UpdateRegisterDetails(RegisterDetails model, string id, string RID, string UID, string image, string PhotoAngle, string GetOfflineStatus)

        //public string UpdateRegisterDetails(RegisterDetails model, string id, string RID, string UID, string image, string PhotoAngle, string GetOfflineStatus)
        //{
        //    string query;
        //    DataTable registerdt;
        //    bool chkOnline = true;
        //    string lbllat = "";
        //    string lbllong = "";
        //    string Message = "";
        //    string DesignSelected = "";
        //    string ddlsupervisor = "";
        //    string txtCrew = "";
        //    string txtPermit = "";

        //    DataTable GetDt = databaseCon.GetData("select * from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'");
        //    if (GetDt.Rows.Count > 0)
        //    {
        //        query = "select distinct(DesignType) from tblDesign where ClientId = '" + id + "' and DesignType = '" + model.ddlDesignSelected.Replace("'", "''").Trim() + "'";
        //        DataTable dtDesign = databaseCon.GetData(query);
        //        if (dtDesign.Rows.Count == 0)
        //        {
        //            string DesignQuery = "Insert into tblDesign(DesignType,ClientID,CreatedOn,CreatedBy,DesignStatus) Values('" + model.ddlDesignSelected.Replace("'", "''").Trim() + "','" + id + "','" + DateTime.Now + "','" + GetDt.Rows[0]["EmailID"].ToString() + "','Active')";
        //            int count = databaseCon.RunCmd(DesignQuery);
        //        }

        //        query = "select distinct(DutyRating) from tblDutyRating where ClientId = '" + id + "' and DutyRating = '" + model.ddlDutyratingSelected.Replace("'", "''").Trim() + "'";
        //        DataTable dtRating = databaseCon.GetData(query);
        //        if (dtRating.Rows.Count == 0)
        //        {
        //            string RatingQuery = "Insert into tblDutyRating(DutyRating,ClientID,CreatedOn,CreatedBy,DutyRatingStatus) Values('" + model.ddlDutyratingSelected.Replace("'", "''").Trim() + "','" + id + "','" + DateTime.Now + "','" + GetDt.Rows[0]["EmailID"].ToString() + "','Active')";
        //            int count = databaseCon.RunCmd(RatingQuery);
        //        }

        //        query = "select distinct(MaterialType) from tblMaterialType where ClientId = '" + id + "' and MaterialType = '" + model.ddlMaterialTypeSelected.Replace("'", "''").Trim() + "'";
        //        DataTable dtMaterial = databaseCon.GetData(query);
        //        if (dtMaterial.Rows.Count == 0)
        //        {
        //            string MaterialQuery = "Insert into tblMaterialType(MaterialType,ClientID,CreatedOn,CreatedBy,MaterialStatus) Values('" + model.ddlMaterialTypeSelected.Replace("'", "''").Trim() + "','" + id + "','" + DateTime.Now + "','" + GetDt.Rows[0]["EmailID"].ToString() + "','Active')";
        //            int count = databaseCon.RunCmd(MaterialQuery);
        //        }
        //    }

        //    //  Query Change On 10 Oct 2019 Add To New Record Funclity

        //    if (model.ddlsitesselected != "")
        //    {
        //        string Locquery = "select * from tblarea where ClientId = '" + id + "' and SiteLocId = '" + model.ddlsitesselected + "' and Area = '" + model.ddlLocationselected.Replace("'", "''").Trim() + "'";
        //        DataTable dtLoc = databaseCon.GetData(Locquery);
        //        if (dtLoc.Rows.Count > 0)
        //        {
        //            model.ddlLocationselected = dtLoc.Rows[0]["AreaID"].ToString();
        //        }

        //        if (model.ddlbuildingselected != "")
        //        {
        //            string Buildquery = "select BuildingID from tblBuilding where ClientId = '" + id + "' and AreaId = '" + model.ddlLocationselected + "' and BuildingNo = '" + model.ddlbuildingselected.Replace("'", "''").Trim() + "'";
        //            DataTable dtbuild = databaseCon.GetData(Buildquery);
        //            if (dtbuild.Rows.Count == 0)
        //            {
        //                string BuldingQuery = "Insert into tblBuilding(BuildingNo,ClientID,AreaId,CreatedOn,CreatedBy,BuildingStatus) Values('" + model.ddlbuildingselected.Replace("'", "''").Trim() + "','" + id + "','" + model.ddlLocationselected + "','" + DateTime.Now + "','" + GetDt.Rows[0]["EmailID"].ToString() + "','Active')";
        //                int BuildingID = databaseCon.RunCmdID(BuldingQuery);

        //                model.ddlbuildingselected = BuildingID.ToString();
        //            }
        //            if (dtbuild.Rows.Count > 0)
        //            {
        //                model.ddlbuildingselected = dtbuild.Rows[0]["BuildingID"].ToString();
        //            }

        //        }

        //        if (model.ddassetsSelected != "")
        //        {
        //            string Assetquery = "select AssetID from tblAsset where ClientId = '" + id + "' and BuildingId = '" + model.ddlbuildingselected + "' and AssetNo = '" + model.ddassetsSelected.Replace("'", "''").Trim() + "'";
        //            DataTable dtasset = databaseCon.GetData(Assetquery);
        //            if (dtasset.Rows.Count == 0)
        //            {
        //                string AssQuery = "Insert into tblAsset(AssetNo,AssetDesc,ClientID,BuildingId,CreatedOn,CreatedBy,AssetStatus) Values('" + model.ddassetsSelected.Replace("'", "''").Trim() + "','" + model.txtassetsdesc.Replace("'", "''").Trim() + "','" + id + "','" + model.ddlbuildingselected + "','" + DateTime.Now + "','" + GetDt.Rows[0]["EmailID"].ToString() + "','Active')";
        //                int AssetID = databaseCon.RunCmdID(AssQuery);

        //                model.ddlbuildingselected = AssetID.ToString();
        //            }
        //            if (dtasset.Rows.Count > 0)
        //            {
        //                model.ddassetsSelected = dtasset.Rows[0]["AssetID"].ToString();
        //            }
        //        }

        //    }

        //    // Old Query Comment on 10 Oct  2019

        //    //if (model.ddlsitesselected != "")
        //    //{
        //    //    string Locquery = "select * from tblarea where ClientId = '" + id + "' and SiteLocId = '" + model.ddlsitesselected + "' and Area = '" + model.ddlLocationselected.Replace("'", "''").Trim() + "'";
        //    //    DataTable dtLoc = databaseCon.GetData(Locquery);
        //    //    if (dtLoc.Rows.Count > 0)
        //    //    {
        //    //        model.ddlLocationselected = dtLoc.Rows[0]["AreaID"].ToString();
        //    //    }

        //    //    if (model.ddlbuildingselected != "")
        //    //    {
        //    //      

        //    //string Buildquery = "select BuildingID from tblBuilding where ClientId = '" + id + "' and AreaId = '" + model.ddlLocationselected + "' and BuildingNo = '" + model.ddlbuildingselected.Replace("'", "''").Trim() + "'";
        //    //DataTable dtbuild = databaseCon.GetData(Buildquery);
        //    //if (dtbuild.Rows.Count > 0)
        //    //{
        //    //    model.ddlbuildingselected = dtbuild.Rows[0]["BuildingID"].ToString();
        //    //}
        //    //    if (model.ddassetsSelected != "")
        //    //    {
        //    //        string Assetquery = "select AssetID from tblAsset where ClientId = '" + id + "' and BuildingId = '" + model.ddlbuildingselected + "' and AssetNo = '" + model.ddassetsSelected.Replace("'", "''").Trim() + "'";
        //    //        DataTable dtasset = databaseCon.GetData(Assetquery);
        //    //        if (dtasset.Rows.Count > 0)
        //    //        {
        //    //            model.ddassetsSelected = dtasset.Rows[0]["AssetID"].ToString();
        //    //        }
        //    //    }

        //    //}

        //    if (Convert.ToInt32(RID) > 0)
        //    {
        //        string angle = "0";
        //        //if (imgangle.Value == "0")
        //        //{
        //        //    angle = "0";
        //        //}

        //        //if (rdpDateTagged.SelectedDate < rdpdatedis.SelectedDate || (rdpdatedis.SelectedDate).ToString() == "")

        //        string query1 = "exec Pro_TBLREGISTER_SAVE '" + RID + "','" + model.txtdesc.Replace("'", "''").Trim() + "','" + id + "','" + model.txtInspectedBy + "','" + model.ddlDesignSelected + "','" + model.ddlDutyratingSelected + "','" + model.ddlMaterialTypeSelected + "','" + model.rdpDateTagged + "','" + model.rdpdatedis + "','','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "','" + model.txtclientReference + "','" + model.ddlInspectionTimeFrame + "','" + model.ddlsitesselected + "','" + angle + "'," + (model.ddlbuildingselected == "" ? null : "'" + model.ddlbuildingselected + "'") + "," + (model.ddassetsSelected == "" ? null : "'" + model.ddassetsSelected + "'") + ",'" + model.txtassetsdesc + "'," + (model.txtlength == "" ? "null" : "'" + model.txtlength + "'") + "," + (model.txtheight == "" ? "null" : "'" + model.txtheight + "'") + "," + (model.txtwidth == "" ? "null" : "'" + model.txtwidth + "'") + ",'" + model.DropDownList2 + "','" + model.DropDownList1 + "','" + model.ddlLocationselected + "','" + chkOnline + "'," + (model.ddlChecklistInspSelected == "" ? "null" : "'" + model.ddlChecklistInspSelected + "'") + ",'" + model.ddlPlanningStatusSelected + "','" + lbllat + "','" + lbllong + "','" + model.ddlLockedIn + "','" + model.txtplnstandDur + "'," + (model.txtCrewSize == "" ? "null" : "'" + model.txtCrewSize + "'") + "," + (model.txtManhrserect == "" ? "null" : "" + model.txtManhrserect + "") + "," + (model.txtmanDisman == "" ? "null" : "" + model.txtmanDisman + "") + "," + (model.ddlSupervisorSelected == null ? null : "'" + model.ddlSupervisorSelected + "'") + ",'" + model.txtCrew.Replace("'", "''").Trim() + "','" + model.txtPermit.Replace("'", "''").Trim() + "','" + model.txtcomment.Replace("'", "''").Trim() + "','" + model.txtSapNumber.Replace("'", "''").Trim() + "','" + model.ddlscaffoldowner.Replace("'", "''").Trim() + "','" + model.rddateDisAppro.Replace("'", "''").Trim() + "','" + model.txtdiappronotes.Replace("'", "''").Trim() + "','" + model.txtbearning.Replace("'", "''").Trim() + "','" + model.ddlLocksignedOffBy.Replace("'", "''").Trim() + "'," + model.checkboxMaintainInventory + ",'" + model.txtDesignDrawingNo.Replace("'", "''").Trim() + "','" + model.txtNoOfPlatform.Replace("'", "''").Trim() + "','" + model.ddlEngineeredDesign.Replace("'", "''").Trim() + "','" + model.txtWOnumb.Replace("'", "''").Trim() + "','" + model.txtScaffType.Replace("'", "''").Trim() + "','" + model.ddlSubownerRep.Replace("'", "''").Trim() + "'";


        //        //  query = "exec Pro_TBLREGISTER_SAVE '" + databaseCon.Decrypt(Request.QueryString["RID"]) + "','" + txtdesc.Text.Replace("'", "''").Trim() + "','" + databaseCon.Decrypt(Request.QueryString["id"]) + "','" + txtInspectedBy.Text.Replace("'", "''").Trim() + "','" + ddlScaffoldDesign.Text.Replace("'", "''").Trim() + "','" + ddlDutyrating.Text.Replace("'", "''").Trim() + "','" + ddlMaterialType.Text.Replace("'", "''").Trim() + "','" + rdpDateTagged.SelectedDate + "','" + rdpdatedis.SelectedDate + "','','" + databaseCon.Decrypt(Request.QueryString["UID"]) + "','" + DateTime.Now + "','" + txtclientReference.Text.Replace("'", "''").Trim() + "','" + ddlInspectionTimeFrame.SelectedValue + "','" + ddlsites.SelectedValue.Replace("'", "''") + "','" + angle + "'," + (ddbuilding.SelectedValue == "" ? "null" : "'" + ddbuilding.SelectedValue.Replace("'", "''").Trim() + "'") + "," + (ddassets.SelectedValue == "" ? "null" : "'" + ddassets.SelectedValue.Replace("'", "''").Trim() + "'") + ",'" + txtassetsdesc.Text.Replace("'", "''").Trim() + "'," + (txtlength.Text == "" ? "null" : "'" + txtlength.Text + "'") + "," + (txtheight.Text == "" ? "null" : "'" + txtheight.Text + "'") + "," + (txtwidth.Text == "" ? "null" : "'" + txtwidth.Text + "'") + ",'" + DropDownList2.SelectedValue.Replace("'", "''").Trim() + "','" + DropDownList1.Text.Replace("'", "''").Trim() + "','" + ddlLocation.SelectedValue.Replace("'", "''").Trim() + "','" + chkOnline.Checked + "'," + (ddlChecklistInsp.SelectedValue == "" ? "null" : "'" + ddlChecklistInsp.SelectedValue.Replace("'", "''").Trim() + "'") + ",'" + ddlPlanningStatus.SelectedValue.Replace("'", "''").Trim() + "','" + hdnlatitude.Value + "','" + hdnlongitude.Value + "','" + ddlLockedIn.SelectedValue + "','" + txtplnstandDur.Text + "'," + (txtCrewSize.Text == "" ? "null" : "'" + txtCrewSize.Text.Replace("'", "''").Trim() + "'") + "," + (txtManhrserect.Text == "" ? "null" : "'" + txtManhrserect.Text.Replace("'", "''").Trim() + "'") + "," + (txtmanDisman.Text == "" ? "null" : "'" + txtmanDisman.Text.Replace("'", "''").Trim() + "'") + "," + (ddlsupervisor.SelectedValue == "" ? "null" : "'" + ddlsupervisor.SelectedValue.Replace("'", "''").Trim() + "'") + ",'" + txtCrew.Text.Replace("'", "''").Trim() + "','" + txtPermit.Text.Replace("'", "''").Trim() + "','" + txtcomments.Text.Replace("'", "''").Trim() + "','" + txtSapNumber.Text.Replace("'", "''").Trim() + "','" + txtScaffoldOwner.Text.Replace("'", "''").Trim() + "','" + rddateDisAppro.SelectedDate + "','" + txtdiappronotes.Text.Replace("'", "''").Trim() + "','" + txtBearing.Text.Replace("'", "''").Trim() + "'," + (valuesupervisou == "" ? "null" : "'" + valuesupervisou + "'") + "," + checkboxMaintainInventory.Checked + ",'" + txtDesignDrawingNo.Text.Replace("'", "''").Trim() + "','" + txtNoOfPlatform.Text.Replace("'", "''").Trim() + "','" + ddlEngineeredDesign.SelectedValue.Replace("'", "''") + "','" + txtWOnumb.Text.Replace("'", "''").Trim() + "','" + txtScaffType.Text.Replace("'", "''").Trim() + "','" + txtdiscipline.Text.Replace("'", "''").Trim() + "'";

        //        registerdt = databaseCon.GetData(query1);
        //        if (registerdt.Rows.Count != 0)
        //        {
        //            int NewRID = Convert.ToInt32(registerdt.Rows[0][0]);
        //            // Message = NewRID + "_Success";
        //            Message = NewRID + "_Success_" + "" + GetOfflineStatus + "";
        //        }

        //        else
        //        {
        //            Message = "null_Failed";
        //        }
        //    }
        //    else
        //    {
        //        if (PhotoAngle == "")
        //        {
        //            PhotoAngle = "0";
        //        }

        //        //string angle = "0";
        //        //if (imgangle.Value == "0")
        //        //    angle = "0";
        //        //else
        //        //    angle = imgangle.Value;



        //        //  string query2 = "exec Pro_TBLREGISTER_SAVE '" + RID + "','" + model.txtdesc.Replace("'", "''").Trim() + "','" + id + "','" + model.txtInspectedBy + "','" + model.ddlDesignSelected.Replace("'", "''").Trim() + "','" + model.ddlDutyratingSelected.Replace("'", "''").Trim() + "','" + model.ddlMaterialTypeSelected.Replace("'", "''").Trim() + "','" + model.rdpDateTagged + "','" + model.rdpdatedis + "','','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "','" + model.txtclientReference.Replace("'", "''").Trim() + "','" + model.ddlInspectionTimeFrame + "','" + model.ddlsitesselected + "','" + PhotoAngle + "'," + (model.ddlbuildingselected == "" ? "null" : "'" + model.ddlbuildingselected + "'") + "," + (model.ddassetsSelected == "" ? "null" : "'" + model.ddassetsSelected + "'") + "," + (model.txtassetsdesc == "" ? "null" : "'" + model.txtassetsdesc.Replace("'", "''").Trim() + "'") + "," + (model.txtlength == "" ? null : "'" + model.txtlength + "'") + "," + (model.txtheight == "" ? null : "'" + model.txtheight + "'") + "," + (model.txtwidth == "" ? null : "'" + model.txtwidth + "'") + ",'" + model.DropDownList2 + "','" + model.DropDownList1 + "','" + model.ddlLocationselected + "','" + chkOnline + "', " + (model.ddlChecklistInspSelected == "" ? null : "'" + model.ddlChecklistInspSelected + "'") + ",'" + model.ddlPlanningStatusSelected + "','" + lbllat + "','" + lbllong + "','" + model.ddlLockedIn + "','" + model.txtplnstandDur + "'," + (model.txtCrewSize == "" ? null : "'" + model.txtCrewSize + "'") + "," + (model.txtManhrserect == "" ? null : "'" + model.txtManhrserect + "'") + "," + (model.txtmanDisman == "" ? null : "'" + model.txtmanDisman + "'") + "," + (model.ddlSupervisorSelected == "" ? "null" : "'" + model.ddlSupervisorSelected + "'") + ",'" + model.txtCrew.Replace("'", "''").Trim() + "','" + model.txtPermit.Replace("'", "''").Trim() + "','" + model.txtcomment.Replace("'", "''").Trim() + "','" + model.txtSapNumber.Replace("'", "''").Trim() + "','"+model.ddlscaffoldowner.Replace("'","''").Trim()+"','" + model.rddateDisAppro.Replace("'", "''").Trim() + "','" + model.txtdiappronotes.Replace("'", "''").Trim() + "','" + model.txtbearning.Replace("'", "''").Trim() + "','" + model.ddlLocksignedOffBy.Replace("'", "''").Trim() + "'";

        //        var Bulding = model.ddlbuildingselected;
        //        var Site = model.ddassetsSelected;
        //        var AssDesc = model.txtassetsdesc.Replace("'", "''").Trim();

        //        string query2 = "exec Pro_TBLREGISTER_SAVE '" + RID + "','" + model.txtdesc.Replace("'", "''").Trim() + "','" + id + "','" + model.txtInspectedBy + "','" + model.ddlDesignSelected.Replace("'", "''").Trim() + "','" + model.ddlDutyratingSelected.Replace("'", "''").Trim() + "','" + model.ddlMaterialTypeSelected.Replace("'", "''").Trim() + "','" + model.rdpDateTagged + "','" + model.rdpdatedis + "','','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "','" + model.txtclientReference.Replace("'", "''").Trim() + "','" + model.ddlInspectionTimeFrame + "','" + model.ddlsitesselected + "','" + PhotoAngle + "'," + (model.ddlbuildingselected == "" ? "null" : "'" + model.ddlbuildingselected + "'") + "," + (model.ddassetsSelected == "" ? "null" : "'" + model.ddassetsSelected + "'") + "," + (model.txtassetsdesc == "" ? "null" : "'" + model.txtassetsdesc.Replace("'", "''").Trim() + "'") + "," + (model.txtlength == "" ? null : "'" + model.txtlength + "'") + "," + (model.txtheight == "" ? null : "'" + model.txtheight + "'") + "," + (model.txtwidth == "" ? null : "'" + model.txtwidth + "'") + ",'" + model.DropDownList2 + "','" + model.DropDownList1 + "','" + model.ddlLocationselected + "','" + chkOnline + "', " + (model.ddlChecklistInspSelected == "" ? null : "'" + model.ddlChecklistInspSelected + "'") + ",'" + model.ddlPlanningStatusSelected + "','" + lbllat + "','" + lbllong + "','" + model.ddlLockedIn + "','" + model.txtplnstandDur + "'," + (model.txtCrewSize == "" ? null : "'" + model.txtCrewSize + "'") + "," + (model.txtManhrserect == "" ? null : "'" + model.txtManhrserect + "'") + "," + (model.txtmanDisman == "" ? null : "'" + model.txtmanDisman + "'") + "," + (model.ddlSupervisorSelected == "" ? "null" : "'" + model.ddlSupervisorSelected + "'") + ",'" + model.txtCrew.Replace("'", "''").Trim() + "','" + model.txtPermit.Replace("'", "''").Trim() + "','" + model.txtcomment.Replace("'", "''").Trim() + "','" + model.txtSapNumber.Replace("'", "''").Trim() + "','" + model.ddlscaffoldowner.Replace("'", "''").Trim() + "','" + model.rddateDisAppro.Replace("'", "''").Trim() + "','" + model.txtdiappronotes.Replace("'", "''").Trim() + "','" + model.txtbearning.Replace("'", "''").Trim() + "','" + model.ddlLocksignedOffBy.Replace("'", "''").Trim() + "'," + model.checkboxMaintainInventory + ",'" + model.txtDesignDrawingNo.Replace("'", "''").Trim() + "','" + model.txtNoOfPlatform.Replace("'", "''").Trim() + "','" + model.ddlEngineeredDesign.Replace("'", "''").Trim() + "','" + model.txtWOnumb.Replace("'", "''").Trim() + "','" + model.txtScaffType.Replace("'", "''").Trim() + "','" + model.ddlSubownerRep.Replace("'", "''").Trim() + "'";

        //        // query = "exec Pro_TBLREGISTER_SAVE '" + databaseCon.Decrypt(Request.QueryString["RID"]) + "','" + txtdesc.Text.Replace("'", "''").Trim() + "','" + databaseCon.Decrypt(Request.QueryString["id"]) + "','" + txtInspectedBy.Text.Replace("'", "''").Trim() + "','" + ddlScaffoldDesign.Text.Replace("'", "''").Trim() + "','" + ddlDutyrating.Text.Replace("'", "''").Trim() + "','" + ddlMaterialType.Text.Replace("'", "''").Trim() + "','" + rdpDateTagged.SelectedDate + "','" + rdpdatedis.SelectedDate + "','','" + databaseCon.Decrypt(Request.QueryString["UID"]) + "','" + DateTime.Now + "','" + txtclientReference.Text.Replace("'", "''").Trim() + "','" + ddlInspectionTimeFrame.SelectedValue + "','" + ddlsites.SelectedValue.Replace("'", "''") + "','" + angle + "'," + (ddbuilding.SelectedValue == "" ? "null" : "'" + ddbuilding.SelectedValue.Replace("'", "''").Trim() + "'") + "," + (ddassets.SelectedValue == "" ? "null" : "'" + ddassets.SelectedValue.Replace("'", "''").Trim() + "'") + ",'" + txtassetsdesc.Text.Replace("'", "''").Trim() + "'," + (txtlength.Text == "" ? "null" : "'" + txtlength.Text + "'") + "," + (txtheight.Text == "" ? "null" : "'" + txtheight.Text + "'") + "," + (txtwidth.Text == "" ? "null" : "'" + txtwidth.Text + "'") + ",'" + DropDownList2.SelectedValue.Replace("'", "''").Trim() + "','" + DropDownList1.Text.Replace("'", "''").Trim() + "','" + ddlLocation.SelectedValue.Replace("'", "''").Trim() + "','" + chkOnline.Checked + "'," + (ddlChecklistInsp.SelectedValue == "" ? "null" : "'" + ddlChecklistInsp.SelectedValue.Replace("'", "''").Trim() + "'") + ",'" + ddlPlanningStatus.SelectedValue.Replace("'", "''").Trim() + "','" + hdnlatitude.Value + "','" + hdnlongitude.Value + "','" + ddlLockedIn.SelectedValue + "','" + txtplnstandDur.Text + "'," + (txtCrewSize.Text == "" ? "null" : "'" + txtCrewSize.Text.Replace("'", "''").Trim() + "'") + "," + (txtManhrserect.Text == "" ? "null" : "'" + txtManhrserect.Text.Replace("'", "''").Trim() + "'") + "," + (txtmanDisman.Text == "" ? "null" : "'" + txtmanDisman.Text.Replace("'", "''").Trim() + "'") + "," + (ddlsupervisor.SelectedValue == "" ? "null" : "'" + ddlsupervisor.SelectedValue.Replace("'", "''").Trim() + "'") + ",'" + txtCrew.Text.Replace("'", "''").Trim() + "','" + txtPermit.Text.Replace("'", "''").Trim() + "','" + txtcomments.Text.Replace("'", "''").Trim() + "','" + txtSapNumber.Text.Replace("'", "''").Trim() + "','" + txtScaffoldOwner.Text.Replace("'", "''").Trim() + "','" + rddateDisAppro.SelectedDate + "','" + txtdiappronotes.Text.Replace("'", "''").Trim() + "','" + txtBearing.Text.Replace("'", "''").Trim() + "'," + (valuesupervisou == "" ? "null" : "'" + valuesupervisou + "'") + "," + checkboxMaintainInventory.Checked + ",'" + txtDesignDrawingNo.Text.Replace("'", "''").Trim() + "','" + txtNoOfPlatform.Text.Replace("'", "''").Trim() + "','" + ddlEngineeredDesign.SelectedValue.Replace("'", "''") + "','" + txtWOnumb.Text.Replace("'", "''").Trim() + "','" + txtScaffType.Text.Replace("'", "''").Trim() + "','" + txtdiscipline.Text.Replace("'", "''").Trim() + "'";

        //        DataTable dt = databaseCon.GetData(query2);
        //        if (dt.Rows.Count != 0)
        //        {
        //            if (Convert.ToInt32(dt.Rows[0][0]) > 0)
        //            {
        //                int newRID = Convert.ToInt32(dt.Rows[0][0]);
        //                string ownername = "";
        //                string queryowner = "select OwnerName from tblsitelocation where SiteLocID='" + model.ddlsitesselected + "'";
        //                DataTable dtsiteowner = databaseCon.GetData(queryowner);
        //                if (dtsiteowner.Rows.Count > 0)
        //                {
        //                    ownername = dtsiteowner.Rows[0]["OwnerName"].ToString();
        //                }

        //                GetimageList(image, newRID, ownername);
        //                // Message = newRID + "_Success";
        //                Message = newRID + "_Success_" + "" + GetOfflineStatus + "";
        //            }
        //            else
        //            {

        //                Message = "null_Failed";
        //            }
        //        }

        //    }

        //    return Message;

        //}

        //  public ActionResult UpdateRegisterDetails(List<RegisterDetails> modelList, string id, string RID, string UID, string image, string PhotoAngle, string GetOfflineStatus)
        public ActionResult UpdateRegisterDetails(List<RegisterDetails> modelList, string id, string RID, string UID, string GetOfflineStatus)
        {
            string query;
            DataTable registerdt;
            bool chkOnline = true;
            string lbllat = "";
            string lbllong = "";
            string Message = "";
            string DesignSelected = "";
            string ddlsupervisor = "";
            string txtCrew = "";
            string txtPermit = "";
            bool RegisterStatusFlag = false;

            var UserEmail = "";
            string userqry = "select EmailID from tbluser where UserId = '" + databaseCon.Decrypt(UID) + "'";
            DataTable dtuser = databaseCon.GetData(userqry);
            if (dtuser.Rows.Count > 0)
            {
                UserEmail = dtuser.Rows[0]["EmailID"].ToString();
            }

            foreach (RegisterDetails model in modelList)
            {


               // DataTable GetDt = databaseCon.GetData("select * from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'");
                DataTable GetDt = databaseCon.GetData("select EmailID from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'");
                if (GetDt.Rows.Count > 0)
                {
                    query = "select distinct(DesignType) from tblDesign where ClientId = '" + id + "' and DesignType = " + (model.ddlDesignSelected == null ? "null" : "'" + model.ddlDesignSelected.Replace("'", "''").Trim() + "'") + "";
                    DataTable dtDesign = databaseCon.GetData(query);
                    if (dtDesign.Rows.Count == 0)
                    {
                        string DesignQuery = "Insert into tblDesign(DesignType,ClientID,CreatedOn,CreatedBy,DesignStatus) Values(" + (model.ddlDesignSelected == null ? "null" : "'" + model.ddlDesignSelected.Replace("'", "''").Trim() + "'") + ",'" + id + "','" + DateTime.Now + "','" + GetDt.Rows[0]["EmailID"].ToString() + "','Active')";
                        int count = databaseCon.RunCmd(DesignQuery);
                    }

                    query = "select distinct(DutyRating) from tblDutyRating where ClientId = '" + id + "' and DutyRating = " + (model.ddlDutyratingSelected == null ? "null" : "'" + model.ddlDutyratingSelected.Replace("'", "''").Trim() + "'") + "";
                    DataTable dtRating = databaseCon.GetData(query);
                    if (dtRating.Rows.Count == 0)
                    {
                        string RatingQuery = "Insert into tblDutyRating(DutyRating,ClientID,CreatedOn,CreatedBy,DutyRatingStatus) Values(" + (model.ddlDutyratingSelected == null ? "null" : "'" + model.ddlDutyratingSelected.Replace("'", "''").Trim() + "'") + ",'" + id + "','" + DateTime.Now + "','" + GetDt.Rows[0]["EmailID"].ToString() + "','Active')";
                        int count = databaseCon.RunCmd(RatingQuery);
                    }

                    query = "select distinct(MaterialType) from tblMaterialType where ClientId = '" + id + "' and MaterialType =" + (model.ddlMaterialTypeSelected == null ? "null" : "'" + model.ddlMaterialTypeSelected.Replace("'", "''").Trim() + "'") + "";
                    DataTable dtMaterial = databaseCon.GetData(query);
                    if (dtMaterial.Rows.Count == 0)
                    {
                        string MaterialQuery = "Insert into tblMaterialType(MaterialType,ClientID,CreatedOn,CreatedBy,MaterialStatus) Values(" + (model.ddlMaterialTypeSelected == null ? "null" : "'" + model.ddlMaterialTypeSelected.Replace("'", "''").Trim() + "'") + ",'" + id + "','" + DateTime.Now + "','" + GetDt.Rows[0]["EmailID"].ToString() + "','Active')";
                        int count = databaseCon.RunCmd(MaterialQuery);
                    }
                }

                //  Query Change On 10 Oct 2019 Add To New Record Funclity

                //if (model.ddlsitesselected != "")
                if (model.ddlsitesselected != "" && model.ddlsitesselected != null)
                {
                    //string Locquery = "select * from tblarea where ClientId = '" + id + "' and SiteLocId = '" + model.ddlsitesselected + "' and Area =" + (model.ddlLocationselected == null ? "null" : "'" + model.ddlLocationselected.Replace("'", "''").Trim() + "'") + "";
                    string Locquery = "select 'X' as AreaID from tblarea where ClientId = '" + id + "' and SiteLocId = '" + model.ddlsitesselected + "' and Area =" + (model.ddlLocationselected == null ? "null" : "'" + model.ddlLocationselected.Replace("'", "''").Trim() + "'") + "";
                    DataTable dtLoc = databaseCon.GetData(Locquery);
                    if (dtLoc.Rows.Count > 0)
                    {
                        model.ddlLocationselected = dtLoc.Rows[0]["AreaID"].ToString();
                    }

                    //if (model.ddlbuildingselected != "")
                    if (model.ddlbuildingselected != "" && model.ddlbuildingselected != null)
                    {
                        string Buildquery = "select BuildingID from tblBuilding where ClientId = '" + id + "' and AreaId = '" + model.ddlLocationselected + "' and BuildingNo =" + (model.ddlbuildingselected == null ? "null" : "'" + model.ddlbuildingselected.Replace("'", "''").Trim() + "'") + " ";
                        DataTable dtbuild = databaseCon.GetData(Buildquery);
                        if (dtbuild.Rows.Count == 0)
                        {
                            string BuldingQuery = "Insert into tblBuilding(BuildingNo,ClientID,AreaId,CreatedOn,CreatedBy,BuildingStatus) Values(" + (model.ddlbuildingselected == null ? "null" : "'" + model.ddlbuildingselected.Replace("'", "''").Trim() + "'") + ",'" + id + "','" + model.ddlLocationselected + "','" + DateTime.Now + "','" + GetDt.Rows[0]["EmailID"].ToString() + "','Active')";
                            int BuildingID = databaseCon.RunCmdID(BuldingQuery);

                            model.ddlbuildingselected = BuildingID.ToString();
                        }
                        if (dtbuild.Rows.Count > 0)
                        {
                            model.ddlbuildingselected = dtbuild.Rows[0]["BuildingID"].ToString();
                        }

                    }

                    //if (model.ddassetsSelected != "")
                    if (model.ddassetsSelected != "" && model.ddassetsSelected != null)
                    {
                        string Assetquery = "select AssetID from tblAsset where ClientId = '" + id + "' and BuildingId = '" + model.ddlbuildingselected + "' and AssetNo =" + (model.ddassetsSelected == null ? "null" : "'" + model.ddassetsSelected.Replace("'", "''").Trim() + "'") + "";
                        DataTable dtasset = databaseCon.GetData(Assetquery);
                        if (dtasset.Rows.Count == 0)
                        {
                            string AssQuery = "Insert into tblAsset(AssetNo,AssetDesc,ClientID,BuildingId,CreatedOn,CreatedBy,AssetStatus) Values(" + (model.ddassetsSelected == null ? "null" : "'" + model.ddassetsSelected.Replace("'", "''").Trim() + "'") + "," + (model.txtassetsdesc == null ? "null" : "'" + model.txtassetsdesc.Replace("'", "''").Trim() + "'") + ",'" + id + "','" + model.ddlbuildingselected + "','" + DateTime.Now + "','" + GetDt.Rows[0]["EmailID"].ToString() + "','Active')";
                            int AssetID = databaseCon.RunCmdID(AssQuery);

                            model.ddlbuildingselected = AssetID.ToString();
                        }
                        if (dtasset.Rows.Count > 0)
                        {
                            model.ddassetsSelected = dtasset.Rows[0]["AssetID"].ToString();
                        }
                    }

                }


                //if (Convert.ToInt32(RID) > 0)
                if (Convert.ToInt32(model.txtRegisterId) > 0)
                {
                    string angle = "0";
                    if (model.PhotoAngle == "" || model.PhotoAngle == null)
                    {
                        angle = "0";
                    }


                    string query1 = "exec Pro_TBLREGISTER_SAVE '" + model.txtRegisterId + "'," + (model.txtdesc == null ? "null" : "'" + model.txtdesc.Replace("'", "''").Trim() + "'") + " ,'" + id + "','" + model.txtInspectedBy + "'," + (model.ddlDesignSelected == null ? "null" : "'" + model.ddlDesignSelected.Replace("'", "''").Trim() + "'") + "," + (model.ddlDutyratingSelected == null ? "null" : "'" + model.ddlDutyratingSelected.Replace("'", "''").Trim() + "'") + "," + (model.ddlMaterialTypeSelected == null ? "null" : "'" + model.ddlMaterialTypeSelected.Replace("'", "''").Trim() + "'") + ",'" + model.rdpDateTagged + "','" + model.rdpdatedis + "','','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "'," + (model.txtclientReference == null ? "null" : "'" + model.txtclientReference.Replace("'", "''").Trim() + "'") + ",'" + model.ddlInspectionTimeFrame + "','" + model.ddlsitesselected + "','" + angle + "'," + (model.ddlbuildingselected == null ? "null" : "'" + model.ddlbuildingselected + "'") + "," + (model.ddassetsSelected == null ? "null" : "'" + model.ddassetsSelected + "'") + "," + (model.txtassetsdesc == null ? "null" : "'" + model.txtassetsdesc.Replace("'", "''").Trim() + "'") + "," + (model.txtlength == "" ? "null" : "'" + model.txtlength + "'") + "," + (model.txtheight == "" ? "null" : "'" + model.txtheight + "'") + "," + (model.txtwidth == "" ? "null" : "'" + model.txtwidth + "'") + ",'" + model.DropDownList2 + "','" + model.DropDownList1 + "','" + model.ddlLocationselected + "','" + chkOnline + "'," + (model.ddlChecklistInspSelected == "" ? "null" : "'" + model.ddlChecklistInspSelected + "'") + ",'" + model.ddlPlanningStatusSelected + "','" + lbllat + "','" + lbllong + "','" + model.ddlLockedIn + "','" + model.txtplnstandDur + "'," + (model.txtCrewSize == "" ? "null" : "'" + model.txtCrewSize + "'") + "," + (model.txtManhrserect == null ? "null" : "" + model.txtManhrserect + "") + "," + (model.txtmanDisman == null ? "null" : "" + model.txtmanDisman + "") + "," + (model.ddlSupervisorSelected == null ? "null" : "'" + model.ddlSupervisorSelected + "'") + "," + (model.txtCrew == null ? "null" : "'" + model.txtCrew.Replace("'", "''").Trim() + "'") + "," + (model.txtPermit == null ? "null" : "'" + model.txtPermit.Replace("'", "''").Trim() + "'") + "," + (model.txtcomment == null ? "null" : "'" + model.txtcomment.Replace("'", "''").Trim() + "'") + "," + (model.txtSapNumber == null ? "null" : "'" + model.txtSapNumber.Replace("'", "''").Trim() + "'") + ", " + (model.ddlscaffoldowner == null ? "null" : "'" + model.ddlscaffoldowner.Replace("'", "''").Trim() + "'") + "," + (model.rddateDisAppro == null ? "null" : "'" + model.rddateDisAppro.Replace("'", "''").Trim() + "'") + "," + (model.txtdiappronotes == null ? "null" : "'" + model.txtdiappronotes.Replace("'", "''").Trim() + "'") + "," + (model.txtbearning == null ? "null" : "'" + model.txtbearning.Replace("'", "''").Trim() + "'") + "," + (model.ddlLocksignedOffBy == null ? "null" : "'" + model.ddlLocksignedOffBy.Replace("'", "''").Trim() + "'") + "," + model.checkboxMaintainInventory + "," + (model.txtDesignDrawingNo == null ? "null" : "'" + model.txtDesignDrawingNo.Replace("'", "''").Trim() + "'") + "," + (model.txtNoOfPlatform == null ? "null" : "'" + model.txtNoOfPlatform.Replace("'", "''").Trim() + "'") + "," + (model.ddlEngineeredDesign == null ? "null" : "'" + model.ddlEngineeredDesign.Replace("'", "''").Trim() + "'") + "," + (model.txtWOnumb == null ? "null" : "'" + model.txtWOnumb.Replace("'", "''").Trim() + "'") + "," + (model.txtScaffType == null ? "null" : "'" + model.txtScaffType.Replace("'", "''").Trim() + "'") + "," + (model.ddlSubownerRep == null ? "null" : "'" + model.ddlSubownerRep.Replace("'", "''").Trim() + "'") + "," + (model.ddlShiftAllocation == null ? "null" : "'" + model.ddlShiftAllocation.Replace("'", "''").Trim() + "'") + "," + (model.txtClientScaff == null ? "null" : "'" + model.txtClientScaff.Replace("'", "''").Trim() + "'") + "," + (model.ddlWorkGroups == null ? "null" : "'" + model.ddlWorkGroups.Replace("'", "''").Trim() + "'") + "," + (model.ddlSubGroups == null ? "null" : "'" + model.ddlSubGroups.Replace("'", "''").Trim() + "'") + "," + (model.txtRequestedBy == null ? "null" : "'" + model.txtRequestedBy.Replace("'", "''").Trim() + "'") + "," + model.checkboxLongTermStructure + "," + model.chkNoInventory + ",'" + model.PipelineStartDate + "'," + (model.PipeLineNote == null ? "null" : "'" + model.PipeLineNote.Replace("'", "''").Trim() + "'") + "," + model.chkReqis + "," + (model.DismantledBy == null ? "null" : "'" + model.DismantledBy.Replace("'", "''").Trim() + "'") + "," + model.BuildDuringShutdown + "";


                    registerdt = databaseCon.GetData(query1);
                    if (registerdt.Rows.Count != 0)
                    {
                        RegisterStatusFlag = true;

                        int NewRID = Convert.ToInt32(registerdt.Rows[0][0]);
                        string ownername = "";
                        string queryowner = "select OwnerName from tblsitelocation where SiteLocID='" + model.ddlsitesselected + "'";
                        DataTable dtsiteowner = databaseCon.GetData(queryowner);
                        if (dtsiteowner.Rows.Count > 0)
                        {
                            ownername = dtsiteowner.Rows[0]["OwnerName"].ToString();
                        }
                        var uploadImage = model.image;
                        if (uploadImage != "" && uploadImage != null)
                        {
                            var checkImageformat = uploadImage.Split(':')[0];
                            if (checkImageformat == "data")
                            {
                                GetimageList(model.image, NewRID, ownername);
                            }
                        }


                        Message = NewRID + "_Success_" + "" + GetOfflineStatus + "";
                    }

                    else
                    {
                        Message = "null_Failed";
                    }
                }
                else
                {
                    var PhotoAngle = "0";
                    if (model.PhotoAngle == "" || model.PhotoAngle == null)
                    {
                        PhotoAngle = "0";
                    }


                    var Bulding = model.ddlbuildingselected;
                    var Site = model.ddassetsSelected;
                    var AssDesc = (model.txtassetsdesc == null ? "null" : "'" + model.txtassetsdesc.Replace("'", "''").Trim() + "'");

                    //string query2 = "exec Pro_TBLREGISTER_SAVE '" + RID + "','" + model.txtdesc.Replace("'", "''").Trim() + "','" + id + "','" + model.txtInspectedBy + "','" + model.ddlDesignSelected.Replace("'", "''").Trim() + "','" + model.ddlDutyratingSelected.Replace("'", "''").Trim() + "','" + model.ddlMaterialTypeSelected.Replace("'", "''").Trim() + "','" + model.rdpDateTagged + "','" + model.rdpdatedis + "','','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "','" + model.txtclientReference.Replace("'", "''").Trim() + "','" + model.ddlInspectionTimeFrame + "','" + model.ddlsitesselected + "','" + PhotoAngle + "'," + (model.ddlbuildingselected == "" ? "null" : "'" + model.ddlbuildingselected + "'") + "," + (model.ddassetsSelected == "" ? "null" : "'" + model.ddassetsSelected + "'") + "," + (model.txtassetsdesc == "" ? "null" : "'" + model.txtassetsdesc.Replace("'", "''").Trim() + "'") + "," + (model.txtlength == "" ? null : "'" + model.txtlength + "'") + "," + (model.txtheight == "" ? null : "'" + model.txtheight + "'") + "," + (model.txtwidth == "" ? null : "'" + model.txtwidth + "'") + ",'" + model.DropDownList2 + "','" + model.DropDownList1 + "','" + model.ddlLocationselected + "','" + chkOnline + "', " + (model.ddlChecklistInspSelected == "" ? null : "'" + model.ddlChecklistInspSelected + "'") + ",'" + model.ddlPlanningStatusSelected + "','" + lbllat + "','" + lbllong + "','" + model.ddlLockedIn + "','" + model.txtplnstandDur + "'," + (model.txtCrewSize == "" ? null : "'" + model.txtCrewSize + "'") + "," + (model.txtManhrserect == "" ? null : "'" + model.txtManhrserect + "'") + "," + (model.txtmanDisman == "" ? null : "'" + model.txtmanDisman + "'") + "," + (model.ddlSupervisorSelected == "" ? "null" : "'" + model.ddlSupervisorSelected + "'") + ",'" + model.txtCrew.Replace("'", "''").Trim() + "','" + model.txtPermit.Replace("'", "''").Trim() + "','" + model.txtcomment.Replace("'", "''").Trim() + "','" + model.txtSapNumber.Replace("'", "''").Trim() + "','" + model.ddlscaffoldowner.Replace("'", "''").Trim() + "','" + model.rddateDisAppro.Replace("'", "''").Trim() + "','" + model.txtdiappronotes.Replace("'", "''").Trim() + "','" + model.txtbearning.Replace("'", "''").Trim() + "','" + model.ddlLocksignedOffBy.Replace("'", "''").Trim() + "'," + model.checkboxMaintainInventory + ",'" + model.txtDesignDrawingNo.Replace("'", "''").Trim() + "','" + model.txtNoOfPlatform.Replace("'", "''").Trim() + "','" + model.ddlEngineeredDesign.Replace("'", "''").Trim() + "','" + model.txtWOnumb.Replace("'", "''").Trim() + "','" + model.txtScaffType.Replace("'", "''").Trim() + "','" + model.ddlSubownerRep.Replace("'", "''").Trim() + "'";

                    // Query Change On 01 Dec 2020

                    string query2 = "exec Pro_TBLREGISTER_SAVE '" + model.txtRegisterId + "'," + (model.txtdesc == null ? "null" : "'" + model.txtdesc.Replace("'", "''").Trim() + "'") + " ,'" + id + "','" + model.txtInspectedBy + "'," + (model.ddlDesignSelected == null ? "null" : "'" + model.ddlDesignSelected.Replace("'", "''").Trim() + "'") + "," + (model.ddlDutyratingSelected == null ? "null" : "'" + model.ddlDutyratingSelected.Replace("'", "''").Trim() + "'") + "," + (model.ddlMaterialTypeSelected == null ? "null" : "'" + model.ddlMaterialTypeSelected.Replace("'", "''").Trim() + "'") + ",'" + model.rdpDateTagged + "','" + model.rdpdatedis + "','','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "'," + (model.txtclientReference == null ? "null" : "'" + model.txtclientReference.Replace("'", "''").Trim() + "'") + ",'" + model.ddlInspectionTimeFrame + "','" + model.ddlsitesselected + "','" + PhotoAngle + "'," + (model.ddlbuildingselected == null ? "null" : "'" + model.ddlbuildingselected + "'") + "," + (model.ddassetsSelected == null ? "null" : "'" + model.ddassetsSelected + "'") + "," + (model.txtassetsdesc == null ? "null" : "'" + model.txtassetsdesc.Replace("'", "''").Trim() + "'") + "," + (model.txtlength == "" ? "null" : "'" + model.txtlength + "'") + "," + (model.txtheight == "" ? "null" : "'" + model.txtheight + "'") + "," + (model.txtwidth == "" ? "null" : "'" + model.txtwidth + "'") + ",'" + model.DropDownList2 + "','" + model.DropDownList1 + "','" + model.ddlLocationselected + "','" + chkOnline + "'," + (model.ddlChecklistInspSelected == "" ? "null" : "'" + model.ddlChecklistInspSelected + "'") + ",'" + model.ddlPlanningStatusSelected + "','" + lbllat + "','" + lbllong + "','" + model.ddlLockedIn + "','" + model.txtplnstandDur + "'," + (model.txtCrewSize == "" ? "null" : "'" + model.txtCrewSize + "'") + "," + (model.txtManhrserect == null ? "null" : "" + model.txtManhrserect + "") + "," + (model.txtmanDisman == null ? "null" : "" + model.txtmanDisman + "") + "," + (model.ddlSupervisorSelected == null ? "null" : "'" + model.ddlSupervisorSelected + "'") + "," + (model.txtCrew == null ? "null" : "'" + model.txtCrew.Replace("'", "''").Trim() + "'") + "," + (model.txtPermit == null ? "null" : "'" + model.txtPermit.Replace("'", "''").Trim() + "'") + "," + (model.txtcomment == null ? "null" : "'" + model.txtcomment.Replace("'", "''").Trim() + "'") + "," + (model.txtSapNumber == null ? "null" : "'" + model.txtSapNumber.Replace("'", "''").Trim() + "'") + ", " + (model.ddlscaffoldowner == null ? "null" : "'" + model.ddlscaffoldowner.Replace("'", "''").Trim() + "'") + "," + (model.rddateDisAppro == null ? "null" : "'" + model.rddateDisAppro.Replace("'", "''").Trim() + "'") + "," + (model.txtdiappronotes == null ? "null" : "'" + model.txtdiappronotes.Replace("'", "''").Trim() + "'") + "," + (model.txtbearning == null ? "null" : "'" + model.txtbearning.Replace("'", "''").Trim() + "'") + "," + (model.ddlLocksignedOffBy == null ? "null" : "'" + model.ddlLocksignedOffBy.Replace("'", "''").Trim() + "'") + "," + model.checkboxMaintainInventory + "," + (model.txtDesignDrawingNo == null ? "null" : "'" + model.txtDesignDrawingNo.Replace("'", "''").Trim() + "'") + "," + (model.txtNoOfPlatform == null ? "null" : "'" + model.txtNoOfPlatform.Replace("'", "''").Trim() + "'") + "," + (model.ddlEngineeredDesign == null ? "null" : "'" + model.ddlEngineeredDesign.Replace("'", "''").Trim() + "'") + "," + (model.txtWOnumb == null ? "null" : "'" + model.txtWOnumb.Replace("'", "''").Trim() + "'") + "," + (model.txtScaffType == null ? "null" : "'" + model.txtScaffType.Replace("'", "''").Trim() + "'") + "," + (model.ddlSubownerRep == null ? "null" : "'" + model.ddlSubownerRep.Replace("'", "''").Trim() + "'") + "," + (model.ddlShiftAllocation == null ? "null" : "'" + model.ddlShiftAllocation.Replace("'", "''").Trim() + "'") + "," + (model.txtClientScaff == null ? "null" : "'" + model.txtClientScaff.Replace("'", "''").Trim() + "'") + "," + (model.ddlWorkGroups == null ? "null" : "'" + model.ddlWorkGroups.Replace("'", "''").Trim() + "'") + "," + (model.ddlSubGroups == null ? "null" : "'" + model.ddlSubGroups.Replace("'", "''").Trim() + "'") + "," + (model.txtRequestedBy == null ? "null" : "'" + model.txtRequestedBy.Replace("'", "''").Trim() + "'") + "," + model.checkboxLongTermStructure + "";

                    DataTable dt = databaseCon.GetData(query2);
                    if (dt.Rows.Count != 0)
                    {
                        if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                        {
                            int newRID = Convert.ToInt32(dt.Rows[0][0]);
                            string ownername = "";
                            string queryowner = "select OwnerName from tblsitelocation where SiteLocID='" + model.ddlsitesselected + "'";
                            DataTable dtsiteowner = databaseCon.GetData(queryowner);
                            if (dtsiteowner.Rows.Count > 0)
                            {
                                ownername = dtsiteowner.Rows[0]["OwnerName"].ToString();
                            }

                            GetimageList(model.image, newRID, ownername);
                            // Message = newRID + "_Success";
                            Message = newRID + "_Success_" + "" + GetOfflineStatus + "";
                        }
                        else
                        {

                            Message = "null_Failed";
                        }
                    }

                }

                // updating IsOfflineCurrent Flag back to 0. 
                if (RegisterStatusFlag == true)
                {
                    string updateqry = "update tblRegister set IsOfflineCurrent= 0,UpdatedBy='" + UserEmail + "', UpdatedOn='" + DateTime.Now + "' where RegisterID= '" + model.txtRegisterId + "'";

                    int result = databaseCon.RunCmd(updateqry);
                }

            }

            // updating those Register which is deleted in Offline & set IsOfflineCurrent Flag back to 0.
            if (RegisterStatusFlag == true)
            {
                string updateqry = "select Count(RegisterId) as ExistRegisterId from tblregister where IsOfflineCurrent= 1 and ClientId='" + id + "'";
                DataTable dtupdate = databaseCon.GetData(updateqry);
                if (dtupdate.Rows.Count > 0)
                {
                    if (dtupdate.Rows[0]["ExistRegisterId"].ToString() != "0")
                    {
                        string updateqry1 = "update tblRegister set IsOfflineCurrent= 0,UpdatedBy='" + UserEmail + "', UpdatedOn='" + DateTime.Now + "' where IsOfflineCurrent= 1 and ClientId='" + id + "'";

                        int result = databaseCon.RunCmd(updateqry1);
                    }
                }

            }

            // return Message;
            return Json(Message, JsonRequestBehavior.AllowGet);
        }

        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod]
        // public string UpdateLocationList(List<ddlLocation> LocationModel, string id, string UID)
        public ActionResult UpdateLocationList(List<ddlLocation> LocationModel, string id, string UID)
        {
            //DataTable GetDt = databaseCon.GetData("select * from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'");
            DataTable GetDt = databaseCon.GetData("select EmailID from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'");
            if (GetDt.Rows.Count > 0)
            {
                //  foreach (var item in LocationModel)
                foreach (ddlLocation item in LocationModel)
                {
                    string SiteLocId = item.SiteLocId;
                    string Area = item.area.Replace("'", "''").Trim();
                    //string query = "select * from tblarea where ClientId = '" + id + "' and area = '" + Area + "' and SiteLocId = '" + SiteLocId + "'";
                    string query = "select AreaID from tblarea where ClientId = '" + id + "' and area = '" + Area + "' and SiteLocId = '" + SiteLocId + "'";
                    DataTable dtDesign = databaseCon.GetData(query);
                    if (dtDesign.Rows.Count == 0)
                    {
                        string DesignQuery = "Insert into tblarea(area,ClientID,CreatedOn,CreatedBy,AreaStatus,SiteLocId) Values('" + Area + "','" + id + "','" + DateTime.Now + "','" + GetDt.Rows[0]["EmailID"].ToString() + "','Active','" + SiteLocId + "')";
                        int count = databaseCon.RunCmd(DesignQuery);
                    }
                }
            }

            return Json("Success", JsonRequestBehavior.AllowGet);
            // return "Success";
        }

        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod]
        // public string UpdateBuildingList(List<ddbuilding> BuildingModel, string id, string UID)
        public ActionResult UpdateBuildingList(List<ddbuilding> BuildingModel, string id, string UID)
        {
            //DataTable GetDt = databaseCon.GetData("select * from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'");
            DataTable GetDt = databaseCon.GetData("select EmailID from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'");
            if (GetDt.Rows.Count > 0)
            {
                //foreach (var item in BuildingModel)
                foreach (ddbuilding item in BuildingModel)
                {
                    string BuildNo = item.BuildingNo.Replace("'", "''").Trim();
                    string Area = item.Area.Replace("'", "''").Trim();

                    string Newquery = "select AreaID from tblarea where ClientId = '" + id + "' and area = '" + Area + "'";
                    DataTable Newdt = databaseCon.GetData(Newquery);
                    if (Newdt.Rows.Count > 0)
                    {
                        string AreaId = Newdt.Rows[0]["AreaID"].ToString();
                        //string query = "select * from tblBuilding where ClientId = '" + id + "' and BuildingNo = '" + BuildNo + "' and AreaId = '" + AreaId + "'";
                        string query = "select 'X' as ExistsFlag from tblBuilding where ClientId = '" + id + "' and BuildingNo = '" + BuildNo + "' and AreaId = '" + AreaId + "'";
                        DataTable dtbuilding = databaseCon.GetData(query);
                        if (dtbuilding.Rows.Count == 0)
                        {
                            string InsertBuildQuery = "Insert into tblBuilding(BuildingNo,ClientId,CreatedOn,CreatedBy,BuildingStatus,AreaId) Values('" + BuildNo
                                + "','" + id + "','" + DateTime.Now + "','" + GetDt.Rows[0]["EmailID"].ToString() + "','Active','" + AreaId + "')";
                            int count = databaseCon.RunCmd(InsertBuildQuery);
                        }
                    }
                }
            }

            return Json("Success", JsonRequestBehavior.AllowGet);
            // return "Success";
        }

        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod]
        // public string UpdateAssetList(List<ddassets> AssetModel, string id, string UID)
        public ActionResult UpdateAssetList(List<ddassets> AssetModel, string id, string UID)
        {
            //DataTable GetDt = databaseCon.GetData("select * from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'");
            DataTable GetDt = databaseCon.GetData("select EmailID from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'");
            if (GetDt.Rows.Count > 0)
            {
                // foreach (var item in AssetModel)
                foreach (ddassets item in AssetModel)
                {
                    string AssetNo = item.Assetno.Replace("'", "''").Trim();
                    string BuildNo = item.BuildNo.Replace("'", "''").Trim();
                    string AssetDesc = item.assetsdesc.Replace("'", "''").Trim();

                    string Newquery = "select BuildingID from tblBuilding where ClientId = '" + id + "' and BuildingNo = '" + BuildNo + "'";
                    DataTable Newdt = databaseCon.GetData(Newquery);
                    if (Newdt.Rows.Count > 0)
                    {
                        string BuildId = Newdt.Rows[0]["BuildingID"].ToString();
                        //string query = "select * from tblasset where ClientId = '" + id + "' and AssetNo = '" + AssetNo + "' and BuildingId = '" + BuildId + "'";
                        string query = "select 'X' as ExistsFlag from tblasset where ClientId = '" + id + "' and AssetNo = '" + AssetNo + "' and BuildingId = '" + BuildId + "'";
                        DataTable dtAssets = databaseCon.GetData(query);
                        if (dtAssets.Rows.Count == 0)
                        {
                            string InsertBuildQuery = "Insert into tblasset(AssetNo,AssetDesc,ClientId,CreatedOn,CreatedBy,AssetStatus,BuildingId) Values('" + AssetNo + "','" + AssetDesc + "','" + id + "','" + DateTime.Now + "','" + GetDt.Rows[0]["EmailID"].ToString() + "','Active','" + BuildId + "')";
                            int count = databaseCon.RunCmd(InsertBuildQuery);
                        }
                    }
                }
            }

            return Json("Success", JsonRequestBehavior.AllowGet);
            // return "Success";
        }

        List<Scaffold> ClientRatio = new List<Scaffold> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod]
        public List<Scaffold> ClientErectRatio(string id)
        {
            string query = "select ManHrsRatio from tblClient where clientid='" + id + "'";
            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    Scaffold pro1 = new Scaffold();
                    if (!(item["ManHrsRatio"] is DBNull) && item["ManHrsRatio"].ToString() != "")
                    {
                        pro1.ManHrsRatio = Convert.ToDecimal(item["ManHrsRatio"].ToString());
                    }
                    ClientRatio.Add(pro1);
                }
            }
            return ClientRatio;
        }


        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod]
        public void GetimageList(string image, int RID, string Ownername)
        {

            string imageData = image;
            string path = "~/images/sites/";
            path = Server.MapPath(path);
            string FileName = Ownername + "_" + RID + "_" + DateTime.Now.Ticks + "_" + ".jpg";
            string SketchnewPath = path + FileName;
            if (imageData != "" && imageData != null)
            {


                byte[] bytes = Convert.FromBase64String(imageData.Split(',')[1]);

                if (System.IO.File.Exists(SketchnewPath))
                {
                    string fpath = path + FileName;
                    //databaseCon.savefileonblob(FileName, "Site-images", fpath, "image/png");
                    System.IO.File.Delete(fpath);

                    using (var imageFile = new FileStream(SketchnewPath, FileMode.Create))
                    {
                        imageFile.Write(bytes, 0, bytes.Length);
                        imageFile.Flush();
                    }
                }
                else
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    using (var imageFile = new FileStream(SketchnewPath, FileMode.Create))
                    {
                        imageFile.Write(bytes, 0, bytes.Length);
                        imageFile.Flush();
                    }
                }
                //string QueryU = "update tblregister set image='" + FileName + "' where registerId ='" + RID + "'";
                string QueryU = "update tblregister set image='" + FileName + "', OriginalBaseImage ='" + imageData + "' where registerId ='" + RID + "'";
                int count = databaseCon.RunCmd(QueryU);
            }
        }

        //shubham gupta


        List<InspectionListdropdown> ddlInspectionListdropdown = new List<InspectionListdropdown> { };

        //        public JsonResult GetInspectionListDetails(string id, string Siteid)
        //        {
        //            var inspectionList = FetchInspectionList(id, Siteid);
        //            return Json(inspectionList, JsonRequestBehavior.AllowGet);
        //        }
        //        public List<InspectionListdropdown> FetchInspectionList(string id, string Siteid)
        //        {
        //            //string query = $"exec Pro_OFFLINETBLINSPECTION_GET '{id}', '{databaseCon.Decrypt(Siteid)}'";
        //            string query = $"EXEC Pro_OFFLINETBLINSPECTION_GET '{id}', '{databaseCon.Decrypt(Siteid)}'";

        //            DataSet ds = databaseCon.GetDataSet(query);

        //            List<InspectionListdropdown> ddlInspectionListdropdown = new List<InspectionListdropdown>();

        //            if (ds.Tables.Count > 0)
        //            {
        //                foreach (DataRow item in ds.Tables[0].Rows)
        //                {
        //                    InspectionListdropdown pro = new InspectionListdropdown();

        //                    if (!(item["InspectionID"] is DBNull) && !string.IsNullOrEmpty(item["InspectionID"].ToString()))
        //                    {
        //                        pro.InspectionID = Convert.ToInt32(item["InspectionID"]);
        //                    }

        //                    //if (!(item["UserId"] is DBNull) && !string.IsNullOrEmpty(item["UserId"].ToString()))
        //                    //{
        //                    //    pro.UserId = Convert.ToInt32(item["UserId"]);
        //                    //}

        //                    if (!(item["InspectionDate"] is DBNull))
        //                    {
        //                        pro.InspectionDate = Convert.ToDateTime(item["InspectionDate"]);
        //                    }

        //                    //if (!(item["InspectionTime"] is DBNull))
        //                    //            //{
        //                    //            //    pro.InspectionTime = Convert.ToDateTime(item["InspectionTime"]);
        //                    //            //}

        //                    //if (!(item["InspectionTime"] is DBNull))
        //                    //{
        //                    //    int timestamp = Convert.ToInt32(item["InspectionTime"]);
        //                    //    pro.InspectionTime = DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;
        //                    //}
        //                    DateTime parsedDate;
        //                    //if (item["InspectionTime"] != DBNull.Value &&
        //                    //    !string.IsNullOrWhiteSpace(item["InspectionTime"].ToString()) &&
        //                    //    DateTime.TryParse(item["InspectionTime"].ToString(), out parsedDate))
        //                    //{
        //                    //    pro.InspectionTime = parsedDate.ToString("dd-MMM-yyyy");
        //                    //}
        //                    //else
        //                    //{
        //                    //    pro.InspectionTime = "";
        //                    //}
        //                    if (item["InspectionTime"] != DBNull.Value &&
        //    !string.IsNullOrWhiteSpace(item["InspectionTime"].ToString()) &&
        //    DateTime.TryParse(item["InspectionTime"].ToString(), out parsedDate))
        //                    {
        //                        pro.InspectionTime = parsedDate.ToString("dd-MMM-yyyy HH:mm");
        //                    }

        //                    if (!(item["InspectedBy"] is DBNull) && !string.IsNullOrEmpty(item["InspectedBy"].ToString()))
        //                    {
        //                        pro.InspectedBy = item["InspectedBy"].ToString();
        //                    }

        //                    if (!(item["InspFileName"] is DBNull) && !string.IsNullOrEmpty(item["InspFileName"].ToString()))
        //                    {
        //                        pro.InspFileName = item["InspFileName"].ToString();
        //                    }

        //                    if (!(item["Comments"] is DBNull) && !string.IsNullOrEmpty(item["Comments"].ToString()))
        //                    {
        //                        pro.Comments = item["Comments"].ToString();
        //                    }

        //                    if (!(item["RegisterID"] is DBNull) && !string.IsNullOrEmpty(item["RegisterID"].ToString()))
        //                    {
        //                        pro.RegisterID = Convert.ToInt32(item["RegisterID"]);
        //                    }

        //                    if (!(item["OnlineInsp"] is DBNull))
        //                    {
        //                        pro.OnlineInsp = item["OnlineInsp"] as bool?;
        //                    }

        //                    if (!(item["CompleteInsp"] is DBNull))
        //                    {
        //                        pro.CompleteInsp = item["CompleteInsp"] as bool?;
        //                    }

        //                    if (!(item["Lat"] is DBNull))
        //                    {
        //                        pro.Lat = item["Lat"] as decimal?;
        //                    }

        //                    if (!(item["Long"] is DBNull))
        //                    {
        //                        pro.Long = item["Long"] as decimal?;
        //                    }

        //                    if (!(item["GPSAddress"] is DBNull) && !string.IsNullOrEmpty(item["GPSAddress"].ToString()))
        //                    {
        //                        pro.GPSAddress = item["GPSAddress"].ToString();
        //                    }

        //                    if (!(item["LicenceNo"] is DBNull) && !string.IsNullOrEmpty(item["LicenceNo"].ToString()))
        //                    {
        //                        pro.LicenceNo = item["LicenceNo"].ToString();
        //                    }

        //                    if (!(item["InspComments"] is DBNull) && !string.IsNullOrEmpty(item["InspComments"].ToString()))
        //                    {
        //                        pro.InspComments = item["InspComments"].ToString();
        //                    }

        //                    if (!(item["Acknowledgment"] is DBNull))
        //                    {
        //                        pro.Acknowledgment = item["Acknowledgment"] as bool?;
        //                    }

        //                    if (!(item["InspStatus"] is DBNull) && !string.IsNullOrEmpty(item["InspStatus"].ToString()))
        //                    {
        //                        pro.InspStatus = item["InspStatus"].ToString();
        //                    }

        //                    //if (!(item["AckDate"] is DBNull))
        //                    //{
        //                    //    pro.AckDate = Convert.ToDateTime(item["AckDate"]);
        //                    //}
        //                    //if (item["AckDate"] != DBNull.Value &&
        //                    //   !string.IsNullOrWhiteSpace(item["AckDate"].ToString()) &&
        //                    //   DateTime.TryParse(item["AckDate"].ToString(), out parsedDate))
        //                    //{
        //                    //    pro.AckDate = parsedDate.ToString("dd-MMM-yyyy");
        //                    //}
        //                    //else
        //                    //{
        //                    //    pro.AckDateTime = "";
        //                    //}
        //                    if (item["AckDate"] != DBNull.Value &&
        // !string.IsNullOrWhiteSpace(item["AckDate"].ToString()) &&
        // DateTime.TryParse(item["AckDate"].ToString(), out parsedDate))
        //                    {
        //                        pro.AckDate = parsedDate.ToString("dd-MMM-yyyy HH:mm");
        //                    }

        //                    //if (!(item["AckDateTime"] is DBNull))
        //                    //{
        //                    //    int timestamp = Convert.ToInt32(item["AckDateTime"]);
        //                    //    pro.AckDateTime = DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;
        //                    //}

        //                    //if (item["AckDateTime"] != DBNull.Value &&
        //                    //    !string.IsNullOrWhiteSpace(item["AckDateTime"].ToString()) &&
        //                    //    DateTime.TryParse(item["AckDateTime"].ToString(), out parsedDate))
        //                    //{
        //                    //    pro.AckDateTime = parsedDate.ToString("dd-MMM-yyyy");
        //                    //}
        //                    //else
        //                    //{
        //                    //    pro.AckDateTime = "";
        //                    //}
        //                    if (item["AckDateTime"] != DBNull.Value &&
        //!string.IsNullOrWhiteSpace(item["AckDateTime"].ToString()) &&
        //DateTime.TryParse(item["AckDateTime"].ToString(), out parsedDate))
        //                    {
        //                        pro.AckDateTime = parsedDate.ToString("dd-MMM-yyyy HH:mm");
        //                    }



        //                    if (!(item["LastEmailSentOn"] is DBNull))
        //                    {
        //                        pro.LastEmailSentOn = Convert.ToDateTime(item["LastEmailSentOn"]);
        //                    }

        //                    //if (!(item["ScheduledOrAckDate"] is DBNull))
        //                    //{
        //                    //    pro.ScheduledOrAckDate = Convert.ToDateTime(item["ScheduledOrAckDate"]);
        //                    //}

        //                    //if (item["ScheduledOrAckDate"] != DBNull.Value &&
        //                    //    !string.IsNullOrWhiteSpace(item["ScheduledOrAckDate"].ToString()) &&
        //                    //    DateTime.TryParse(item["ScheduledOrAckDate"].ToString(), out parsedDate))
        //                    //{
        //                    //    pro.ScheduledOrAckDate = parsedDate.ToString("dd-MMM-yyyy");
        //                    //}
        //                    //else
        //                    //{
        //                    //    pro.ScheduledOrAckDate = "";
        //                    //}
        //                    if (item["ScheduledOrAckDate"] != DBNull.Value &&
        //!string.IsNullOrWhiteSpace(item["ScheduledOrAckDate"].ToString()) &&
        //DateTime.TryParse(item["ScheduledOrAckDate"].ToString(), out parsedDate))
        //                    {
        //                        pro.ScheduledOrAckDate = parsedDate.ToString("dd-MMM-yyyy HH:mm");
        //                    }

        //                    //     if (!(item["ScheduledOrAckTime"] is DBNull))
        //                    //{
        //                    //    int timestamp = Convert.ToDateTime(item["ScheduledOrAckTime"]);
        //                    //    pro.ScheduledOrAckTime = DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;
        //                    //}

        //                    if (!(item["OffSystemInspection"] is DBNull))
        //                    {
        //                        pro.OffSystemInspection = item["OffSystemInspection"] as bool?;
        //                    }

        //                    if (!(item["InspSign"] is DBNull))
        //                    {
        //                        pro.InspSign = item["InspSign"] as byte[];
        //                    }
        //                    if (!(item["Inspimage"] is DBNull) && !string.IsNullOrEmpty(item["Inspimage"].ToString()))
        //                    {
        //                        pro.Inspimage = item["Inspimage"].ToString();
        //                    }
        //                    if (!(item["Inspimage2"] is DBNull) && !string.IsNullOrEmpty(item["Inspimage2"].ToString()))
        //                    {
        //                        pro.Inspimage2 = item["Inspimage2"].ToString();
        //                    }


        //                    ddlInspectionListdropdown.Add(pro);
        //                }
        //            }

        //            return ddlInspectionListdropdown;
        //        }



        List<TblUserList> ddlTblUserList = new List<TblUserList> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod]
        public List<TblUserList> GetUserListDataDetails(string id, string RID)
        {
            //string query = "select distinct SubGroup from  tblRegister where ClientId = '" + ClientID + "' and SubGroup is not null and SubGroup!= '' Order by SubGroup";
            //string query = "select firstName,LastName,UserId,LicenceNo from tblUser where IsInspector='true' and Status='Active' and clientid='" + id + "' Order by firstName";
            string query = "select firstName,LastName,UserId,LicenceNo from tblUser join tblRegister on tblUser.LoginId = tblRegister.GoOfflineJobBy where tblRegister.RegisterId = '" + databaseCon.Decrypt(RID) + "' and tblUser.IsInspector = 'true' and tblUser.Status = 'Active'";

            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    TblUserList pro = new TblUserList();

                    if (!(item["firstName"] is DBNull) && !(item["LastName"] is DBNull))
                    {
                        pro.FullName = $"{item["firstName"]} {item["LastName"]}".Trim();
                    }

                    if (!(item["firstName"] is DBNull) && item["firstName"].ToString() != "")
                    {
                        pro.firstName = item["firstName"].ToString();
                    }
                    if (!(item["LastName"] is DBNull) && item["LastName"].ToString() != "")
                    {
                        pro.LastName = item["LastName"].ToString();
                    }

                    if (!(item["UserId"] is DBNull) && !string.IsNullOrEmpty(item["UserId"].ToString()))
                    {
                        pro.UserId = Convert.ToInt32(item["UserId"]);
                    }
                    if (!(item["LicenceNo"] is DBNull) && !string.IsNullOrEmpty(item["LicenceNo"].ToString()))
                    {
                        pro.LicenceNo = item["LicenceNo"].ToString();
                    }

                    ddlTblUserList.Add(pro);
                }
            }
            return ddlTblUserList;
        }



        //Shubham Add 2

        List<TblcheckListsContent> ddlcheckList = new List<TblcheckListsContent> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod]
        public List<TblcheckListsContent> GetcheckListContentDataDetails(string id, string RID)
        {
            //string query = "select distinct SubGroup from  tblRegister where ClientId = '" + ClientID + "' and SubGroup is not null and SubGroup!= '' Order by SubGroup";
            //string query = "select * from tblChecklistContent where ClientId = '" + id + "'";  
            string query = "select tblChecklistContent.checklistID, tblChecklistContent.CheckListContent, tblChecklistContent.ClientID, tblChecklistContent.ContentID, tblChecklistContent.checklistItem, tblChecklistContent.subChildItem, tblChecklistContent.IsNA from tblchecklist join tblRegister on tblRegister.checklistID = tblchecklist.checklistID join tblChecklistContent on tblChecklistContent.CheckListID = tblRegister.checklistID where tblRegister.Registerid = '" + databaseCon.Decrypt(RID) + "' and tblchecklist.status = 'Active'";

            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    TblcheckListsContent pro = new TblcheckListsContent();



                    if (!(item["checklistID"] is DBNull) && !string.IsNullOrEmpty(item["checklistID"].ToString()))
                    {
                        pro.checklistID = Convert.ToInt32(item["checklistID"]);
                    }

                    if (!(item["CheckListContent"] is DBNull) && !string.IsNullOrEmpty(item["CheckListContent"].ToString()))
                    {
                        pro.CheckListContent = item["CheckListContent"].ToString();
                    }
                    if (!(item["ClientID"] is DBNull))
                    {
                        pro.ClientID = item["ClientID"] as int?;
                    }
                    if (!(item["ContentID"] is DBNull) && !string.IsNullOrEmpty(item["ContentID"].ToString()))
                    {
                        pro.ContentID = Convert.ToInt32(item["ContentID"]);
                    }

                    if (!(item["checklistItem"] is DBNull) && item["checklistItem"].ToString() != "")
                    {
                        pro.checklistItem = item["checklistItem"] as int?;
                    }
                    if (!(item["subChildItem"] is DBNull) && item["subChildItem"].ToString() != "")
                    {
                        pro.subChildItem = item["subChildItem"] as int?;
                    }
                    if (!(item["IsNA"] is DBNull) && item["IsNA"].ToString() != "")
                    {
                        pro.IsNA = Convert.ToBoolean(item["IsNA"]);
                    }

                    ddlcheckList.Add(pro);
                }
            }
            return ddlcheckList;
        }


        List<OfflineInspectionList> ddlOfflineInspectionList = new List<OfflineInspectionList> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod]

        //public List<OfflineInspectionList> GetOfflineInspectionListDetailsData(string id)
        //{
        //    //string query = "select distinct SubGroup from  tblRegister where ClientId = '" + ClientID + "' and SubGroup is not null and SubGroup!= '' Order by SubGroup";
        //    string query = "select * from tblinspectionList where ClientID = " + id + " ";

        //    DataSet ds = null;

        //    ds = databaseCon.GetDataSet(query);

        //    if (ds.Tables.Count > 0)
        //    {
        //        foreach (DataRow item in ds.Tables[0].Rows)
        //        {
        //            OfflineInspectionList pro = new OfflineInspectionList();


        //            if (!(item["InspectionListID"] is DBNull) && !string.IsNullOrEmpty(item["InspectionListID"].ToString()))
        //            {
        //                pro.InspectionListID = Convert.ToInt32(item["InspectionListID"]);
        //            }
        //            if (!(item["InspectionID"] is DBNull) && !string.IsNullOrEmpty(item["InspectionID"].ToString()))
        //            {
        //                pro.InspectionID = Convert.ToInt32(item["InspectionID"]);
        //            }

        //            if (!(item["CheckListID"] is DBNull) && !string.IsNullOrEmpty(item["CheckListID"].ToString()))
        //            {
        //                pro.CheckListID = Convert.ToInt32(item["CheckListID"]);
        //            }
        //            if (!(item["ContentID"] is DBNull) && !string.IsNullOrEmpty(item["ContentID"].ToString()))
        //            {
        //                pro.ContentID = Convert.ToInt32(item["ContentID"]);
        //            }
        //            if (!(item["ClientID"] is DBNull))
        //            {
        //                pro.ClientID = item["ClientID"] as int?;
        //            }
        //            //}
        //            //if (!(item["Y"] is DBNull))
        //            //{
        //            //    pro.Y = item["Y"] as bool?;
        //            //}
        //            //if (!(item["N"] is DBNull))
        //            //{
        //            //    pro.N = item["N"] as bool?;
        //            //}

        //            if (item["Y"] != DBNull.Value)
        //            {
        //                pro.Y = Convert.ToBoolean(item["Y"]);
        //            }
        //            else
        //            {
        //                pro.Y = null; // Explicitly set to null if needed.
        //            }

        //            if (item["N"] != DBNull.Value)
        //            {
        //                pro.N = Convert.ToBoolean(item["N"]);
        //            }
        //            else
        //            {
        //                pro.N = null; // Explicitly set to null if needed.
        //            }

        //            if (item["NA"] != DBNull.Value)
        //            {
        //                pro.NA = Convert.ToBoolean(item["NA"]);
        //            }
        //            else
        //            {
        //                pro.NA = null; // Explicitly set to null if DBNull
        //            }
        //            //if (!(item["NA"] is DBNull))
        //            //{
        //            //    pro.NA = item["NA"] as bool?;
        //            //}

        //            //if (!(item["ActionRequired"] is DBNull) && !string.IsNullOrEmpty(item["ActionRequired"].ToString()))
        //            //{
        //            //    pro.ActionRequired = item["ActionRequired"].ToString();
        //            //}
        //            if (!(item["CreatedBy"] is DBNull) && !string.IsNullOrEmpty(item["CreatedBy"].ToString()))
        //            {
        //                pro.CreatedBy = item["CreatedBy"].ToString();
        //            }
        //            if (!(item["CreatedOn"] is DBNull))
        //            {
        //                pro.CreatedOn = Convert.ToDateTime(item["CreatedOn"]);
        //            }
        //            if (!(item["UpdatedOn"] is DBNull))
        //            {
        //                pro.UpdatedOn = Convert.ToDateTime(item["UpdatedOn"]);
        //            }
        //            if (!(item["RegisterId"] is DBNull) && item["RegisterId"].ToString() != "")
        //            {
        //                pro.RegisterId = Convert.ToInt32(item["RegisterId"].ToString());
        //            }
        //            if (!(item["ActionRequired"] is DBNull) && !string.IsNullOrEmpty(item["ActionRequired"].ToString()))
        //            {
        //                pro.ActionRequired = item["ActionRequired"].ToString();
        //            }

        //            ddlOfflineInspectionList.Add(pro);
        //        }
        //    }
        //    return ddlOfflineInspectionList;
        //}


        //neeraj Offline

        //List<RegisterGallerydropdown> ddlRegisterGallerydropdown = new List<RegisterGallerydropdown> { };
        ////[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        ////[WebMethod]
        //public List<RegisterGallerydropdown> GetRegisterGallerytData(string RID)
        //{
        //    //string query = "select distinct Image from  tblRegister where ClientId = '33' and Image is not null and Image!= '' Order by Image";
        //    string query = "select  ID,RegisterId,FileName,Description,Type from tblRegisterGallery where Type='ImageGallery'";
        //    DataSet ds = null;
        //    ds = databaseCon.GetDataSet(query);
        //    if (ds.Tables.Count > 0)
        //    {
        //        foreach (DataRow item in ds.Tables[0].Rows)
        //        {
        //            RegisterGallerydropdown pro = new RegisterGallerydropdown();
        //            //if (!(item["Images"] is DBNull) && item["Images"].ToString() != "")
        //            //{
        //            //    pro.Images = item["Images"].ToString();
        //            //}
        //            if (!(item["RegisterId"] is DBNull) && item["RegisterId"].ToString() != "")
        //            {
        //                pro.RegisterId = Convert.ToInt32(item["RegisterId"].ToString());
        //            }
        //            if (!(item["ID"] is DBNull) && item["ID"].ToString() != "")
        //            {
        //                pro.ID = Convert.ToInt32(item["ID"].ToString());
        //            }
        //            if (!(item["Description"] is DBNull) && !string.IsNullOrEmpty(item["Description"].ToString()))
        //            {
        //                pro.Description = item["Description"].ToString();
        //            }
        //            if (!(item["Type"] is DBNull) && !string.IsNullOrEmpty(item["Type"].ToString()))
        //            {
        //                pro.Type = item["Type"].ToString();
        //            }
        //            if (!(item["FileName"] is DBNull) && !string.IsNullOrEmpty(item["FileName"].ToString()))
        //            {
        //                pro.FileName = item["FileName"].ToString();
        //            }
        //            ddlRegisterGallerydropdown.Add(pro);
        //        }
        //    }
        //    return ddlRegisterGallerydropdown;
        //}


        public class TableData
        {
            public string TableName { get; set; }
            public List<Dictionary<string, object>> Data { get; set; }

        }

        [HttpPost]
        public ActionResult UploadToCloud(string jsonData)
        {
            var dataJson = JsonConvert.DeserializeObject<LargeDataModel>(jsonData);
            List<TableData> data = dataJson.data;
            string UserId = dataJson.UserId;
            string ClientID = dataJson.ClientID;
            string SiteId = dataJson.SiteId;
            string UserRole = dataJson.UserRole;
            string RID = dataJson.RID;
            bool isInspectionProcessed = false;
            try
            {
                foreach (var tableData in data)
                {
                    switch (tableData.TableName)
                    {
                        case "tblRegisterDetails":
                            ProcessTableData(tableData.Data, (record) => ProcessRegisterDetails(record, UserId, RID), UserId, RID);

                            string QueryForFinalRunProc = "exec Pro_TBLOfflineREGISTER_SAVE " + UserId + "," + ClientID + "";

                            var Messege = databaseCon.GetData(QueryForFinalRunProc);
                            break;
                        case "tblRegisterGallery":
                            ProcessTableData(tableData.Data, (record) => ProcessRegisterGalleryData(record, UserId, RID), UserId, RID);
                            break;
                        case "tblPipelineRequest":
                            ProcessTableData(tableData.Data, (record) => tblPipelineRequestData(record, UserId, ClientID, SiteId, RID), UserId, RID);

                            break;
                        // we had to change  the  inspection table case  because of  de-plank  
                        case "tblInspectionListDetails":
                            ProcessTableData(tableData.Data, (record) => ProcesstblInspectionListData(record, UserId, ClientID, SiteId, RID), UserId, RID);
                            break;

                        case "tblInspectionDetails":
                            ProcessTableData(tableData.Data, (record) => ProcesstbltblInspectionData(record, UserId, ClientID, SiteId, RID), UserId, RID);
                            isInspectionProcessed = true;
                            break;

                            //case "tblMaterialType":
                            //    ProcessTableData(tableData.Data, ProcessMaterialType, Userid);
                            //    break;
                            //case "tblDutyRating":
                            //    ProcessTableData(tableData.Data, ProcessDutyRating , Userid);
                            //    break;
                            //case "tblAssetList":
                            //    ProcessTableData(tableData.Data, ProcessAssetList, Userid);
                            //    break;

                    }
                }
                if (isInspectionProcessed)
                {
                    string QueryForFinalInspRunProc = "exec Pro_OfflineInspectionSave " + UserId + "," + ClientID + "";
                    var FinalMessage = databaseCon.GetData(QueryForFinalInspRunProc);
                }

                //string domainName = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);

                ////string Link = domainName + "/OfflineRegister.html?UID=" + UID + "&UserRole=" + UserRole + "&id=" + id + "&offlinestatus=" + GetOfflinestatus + "";
                //var Link = domainName + "RegisterList/RegisterList?UID=" + databaseCon.Encrypt(UserId) + "&UserRole=" + databaseCon.Encrypt(UserRole) + "&id=" + databaseCon.Encrypt(ClientID) + "";
                return Json(new { success = true, message = "Data uploaded successfully.", UID = databaseCon.Encrypt(UserId), UserRole = databaseCon.Encrypt(UserRole), id = databaseCon.Encrypt(ClientID), RID = databaseCon.Encrypt(RID) });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
            finally
            {
                UpdateOfflineInspectionFlag(UserId, ClientID, SiteId, UserRole, RID);
            }

        }
        //Split method for upload to cloud

        public class LargeDataModel
        {
            public List<TableData> data { get; set; }
            public string UserId { get; set; }
            public string ClientID { get; set; }
            public string SiteId { get; set; }
            public string UserRole { get; set; }
            public string RID { get; set; }
        }

        [HttpPost]
        //public ActionResult UploadToCloud(string jsonData)
        //{
        //    var mydata = JsonConvert.DeserializeObject<LargeDataModel>(jsonData);
        //    List<TableData> data = mydata.data;
        //    string UserId = mydata.UserId;
        //    string ClientID = mydata.ClientID;
        //    string SiteId = mydata.SiteId;
        //    string UserRole = mydata.UserRole;
        //    string RID = mydata.RID;

        //    var Result = string.Empty;
        //    var Message = string.Empty;
        //    //SiteId = databaseCon.Decrypt(SiteId);
        //    //bool isInspectionProcessed = false;
        //    //bool isInspectionProcessedDetails = false;
        //    try
        //    {
        //        foreach (var tableData in data)
        //        {
        //            switch (tableData.TableName)
        //            {
        //                case "tblRegisterDetails":
        //                    ProcessTableData(tableData.Data, (record) => ProcessRegisterDetails(record, UserId, RID), UserId, RID);

        //                    string QueryForFinalRunProc = "exec Pro_TBLOfflineREGISTER_SAVE " + UserId + "," + ClientID + "";
        //                    DataTable FinalResponse = databaseCon.GetData(QueryForFinalRunProc);

        //                    if (FinalResponse != null && FinalResponse.Rows.Count > 0)
        //                    {
        //                        Result = Convert.ToString(FinalResponse.Rows[0]["Result"]);
        //                        Message = Convert.ToString(FinalResponse.Rows[0]["Message"]);
        //                    }
        //                    else
        //                    {
        //                        Console.WriteLine("No data returned from the procedure.");
        //                    }
        //                    break;
        //                case "tblRegisterGallery":
        //                    ProcessTableData(tableData.Data, (record) => ProcessRegisterGalleryData(record, UserId, RID), UserId, RID);
        //                    Result = "Success";
        //                    Message = "Image Gallery uploaded successfully.";
        //                    break;
        //                case "tblPipelineRequest":
        //                    ProcessTableData(tableData.Data, (record) => tblPipelineRequestData(record, UserId, ClientID, SiteId, RID), UserId, RID);
        //                    Result = "Success";
        //                    Message = "Job Inventory uploaded successfully.";
        //                    break;

        //                // we had to change  the  inspection table case  because of  de-plank 
        //                case "tblInspectionListDetails":
        //                    ProcessTableData(tableData.Data, (record) => ProcesstblInspectionListData(record, UserId, ClientID, SiteId, RID), UserId, RID);
        //                    //isInspectionProcessedDetails = true;
        //                    //string QueryForFinalInspRunProc = "exec Pro_OfflineInspectionSave " + UserId + "," + ClientID + "";
        //                    //DataTable InspectionResponse = databaseCon.GetData(QueryForFinalInspRunProc);
        //                    //Result = Convert.ToString(InspectionResponse.Rows[0]["Result"]);
        //                    //Message = Convert.ToString(InspectionResponse.Rows[0]["Message"]);

        //                    break;
        //                case "tblInspectionDetails":
        //                    ProcessTableData(tableData.Data, (record) => ProcesstbltblInspectionData(record, UserId, ClientID, SiteId, RID), UserId, RID);
        //                    //isInspectionProcessed = true;

        //                    //ProcessTableData(tableData.Data, (record) => ProcesstblInspectionListData(record, UserId, ClientID, SiteId), UserId);
        //                    ////isInspectionProcessed = true;
        //                    string QueryForFinalInspRunProc = "exec Pro_OfflineInspectionSave " + UserId + "," + ClientID + "";
        //                    DataTable InspectionResponse = databaseCon.GetData(QueryForFinalInspRunProc);
        //                    Result = Convert.ToString(InspectionResponse.Rows[0]["Result"]);
        //                    Message = Convert.ToString(InspectionResponse.Rows[0]["Message"]);
        //                    break;



        //                    //case "tblMaterialType":
        //                    //    ProcessTableData(tableData.Data, ProcessMaterialType, Userid);
        //                    //    break;
        //                    //case "tblDutyRating":
        //                    //    ProcessTableData(tableData.Data, ProcessDutyRating , Userid);
        //                    //    break;
        //                    //case "tblAssetList":
        //                    //    ProcessTableData(tableData.Data, ProcessAssetList, Userid);
        //                    //    break;

        //            }
        //        }
        //        //if (isInspectionProcessed && isInspectionProcessedDetails)
        //        //{
        //        //    //string QueryForFinalInspRunProc = "exec Pro_OfflineInspectionSave " + UserId + "," + ClientID + "";
        //        //    //var FinalMessage = databaseCon.GetData(QueryForFinalInspRunProc);
        //        //}

        //        //string domainName = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);

        //        ////string Link = domainName + "/OfflineRegister.html?UID=" + UID + "&UserRole=" + UserRole + "&id=" + id + "&offlinestatus=" + GetOfflinestatus + "";
        //        //var Link = domainName + "RegisterList/RegisterList?UID=" + databaseCon.Encrypt(UserId) + "&UserRole=" + databaseCon.Encrypt(UserRole) + "&id=" + databaseCon.Encrypt(ClientID) + "";
        //        //return Json(new { success = true, message = "Data uploaded successfully.", UID = databaseCon.Encrypt(UserId), UserRole = databaseCon.Encrypt(UserRole), id = databaseCon.Encrypt(ClientID), RID = databaseCon.Encrypt(RID) });
        //        return Json(new { success = true, Result = Result, Message = Message });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, message = ex.Message });
        //    }
        //    //finally
        //    //{
        //    //    UpdateOfflineInspectionFlag(UserId, ClientID, SiteId, UserRole, RID);
        //    //}

        //}
        private void ProcesstbltblInspectionData(Dictionary<string, object> record, string UserId, string ClientID, string SiteId, string RID)
        {
            try
            {
                string QueryForStep1 = "INSERT INTO TblTrackUploadToCloud(ProcName,Registerid, userid, createdBy, CreatedOn, Query, Response) values('Pre-TblTempOfflineInspection_UploadedToCloud','" + RID + "'," + (UserId == null ? "null" : "'" + UserId.ToString() + "'") + ",'" + UserId + "',GETDATE(),'we are inti.. before  variable" + UserId + "','making query for user: " + UserId + "')";
                databaseCon.RunCmd(QueryForStep1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //DataTable Inspectiondt;
            var Inspectiondt = 0;
            // Extract the values from the dictionary, with null checks
            string AckDateTime = record.ContainsKey("AckDateTime") ? record["AckDateTime"]?.ToString() : null;
            //string InspectionDate = record.ContainsKey("InspectionDate") ? record["InspectionDate"]?.ToString() : null;
            string InspectionDate = record.ContainsKey("InspectionDate") ? record["InspectionDate"]?.ToString() : null;
            string Acknowledgment = record.ContainsKey("Acknowledgment") ? record["Acknowledgment"]?.ToString() : null;
            string InspComments = record.ContainsKey("InspComments") ? record["InspComments"]?.ToString() : null;
            string InspSign = record.ContainsKey("InspSign") ? record["InspSign"]?.ToString() : null;
            string InspStatus = record.ContainsKey("InspStatus") ? record["InspStatus"]?.ToString() : null;
            string InspectedBy = record.ContainsKey("InspectedBy") ? record["InspectedBy"]?.ToString() : null;
            string InspectedById = record.ContainsKey("InspectedById") ? record["InspectedById"]?.ToString() : null;
            string InspectionID = record.ContainsKey("InspectionID") ? record["InspectionID"]?.ToString() : null;
            string Inspimage = record.ContainsKey("Inspimage") ? record["Inspimage"]?.ToString() : null;
            string Inspimage2 = record.ContainsKey("Inspimage2") ? record["Inspimage2"]?.ToString() : null;
            string LicenceNo = record.ContainsKey("LicenceNo") ? record["LicenceNo"]?.ToString() : null;
            string RegisterID = record.ContainsKey("RegisterID") ? record["RegisterID"]?.ToString() : null;
            string ScheduledOrAckDate = record.ContainsKey("ScheduledOrAckDate") ? record["ScheduledOrAckDate"]?.ToString() : null;
            string ddlChecklistInsp = record.ContainsKey("ddlChecklistInsp") ? record["ddlChecklistInsp"]?.ToString() : null;
            string insertedOffline = record.ContainsKey("insertedOffline") ? record["insertedOffline"]?.ToString() : null;
            string clientId = ClientID;
            string siteId = SiteId;
            string UID = UserId;
            string path = Server.MapPath("/images/sites/");
            string InspimageFileName = SaveInspImageToBlob(Inspimage, RegisterID, InspectionID, path);
            string Inspimage2FileName = SaveInspImageToBlob(Inspimage2, RegisterID, InspectionID, path);
            string InspSignFileName = SaveInspSignImageToBlob(InspSign, RegisterID, InspectionID, path);
            try
            {
                string QueryForStep1 = "INSERT INTO TblTrackUploadToCloud(ProcName,Registerid, userid, createdBy, CreatedOn, Query, Response) values('Pre-TblTempOfflineInspection_UploadedToCloud'," + (RegisterID == null ? "null" : "'" + RegisterID.Replace("'", "''").Trim() + "'") + "," + (UserId == null ? "null" : "'" + UserId.ToString() + "'") + ",'" + UserId + "',GETDATE(),'we are inti.. the value for " + UserId + "','making query for user: " + UserId + "')";
                databaseCon.RunCmd(QueryForStep1);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

            //string query = "INSERT INTO TblTempOfflineInspection_UploadedToCloud( RegisterID, InspectedById, AckDateTime,InspectedBy, LicenceNo, InsertedOffline, InspStatus,ScheduledOrAckDate, ddlChecklistInsp, Inspimage2,Inspimage, InspComments, InspSign, Acknowledgment,Clientid,Userid,InspectionID,InspectionDate) VALUES(" + (RegisterID == null ? "null" : "'" + RegisterID.Replace("'", "''").Trim() + "'") + ", " + (InspectedById == null ? "null" : "'" + InspectedById.Replace("'", "''").Trim() + "'") + ", " + (AckDateTime == null ? "null" : "'" + AckDateTime.Replace("'", "''").Trim() + "'") + ", " + (InspectedBy == null ? "null" : "'" + InspectedBy.Replace("'", "''").Trim() + "'") + ", " + (LicenceNo == null ? "null" : "'" + LicenceNo.Replace("'", "''").Trim() + "'") + ", " + (insertedOffline == null ? "null" : "'" + insertedOffline.Replace("'", "''").Trim() + "'") + ", " + (InspStatus == null ? "null" : "'" + InspStatus.Replace("'", "''").Trim() + "'") + ", " + (ScheduledOrAckDate == null ? "null" : "'" + ScheduledOrAckDate.Replace("'", "''").Trim() + "'") + ", " + (ddlChecklistInsp == null ? "null" : "'" + ddlChecklistInsp.Replace("'", "''").Trim() + "'") + ", " + (Inspimage2FileName == null ? "null" : "'" + Inspimage2FileName.Replace("'", "''").Trim() + "'") + ", " + (InspimageFileName == null ? "null" : "'" + InspimageFileName.Replace("'", "''").Trim() + "'") + ", " + (InspComments == null ? "null" : "'" + InspComments.Replace("'", "''").Trim() + "'") + ", " + (InspSignFileName == null ? "null" : "'" + InspSignFileName.Replace("'", "''").Trim() + "'") + ", " + (Acknowledgment == null ? "null" : "'" + Acknowledgment.Replace("'", "''").Trim() + "'") + ", " + (clientId == null ? "null" : "'" + clientId.Replace("'", "''").Trim() + "'") + ", " + (UID == null ? "null" : "'" + UID.Replace("'", "''").Trim() + "'") + ", " + (InspectionID == null ? "null" : "'" + InspectionID.Replace("'", "''").Trim() + "'") + ", " + (InspectionDate == null ? "null" : "'" + InspectionDate.Replace("'", "''").Trim() + "'") + ")";
            string query = "INSERT INTO TblTempOfflineInspection_UploadedToCloud( RegisterID, InspectedById, AckDateTime,InspectedBy, LicenceNo, InsertedOffline, InspStatus,ScheduledOrAckDate, ddlChecklistInsp, Inspimage2,Inspimage, InspComments, InspSign, Acknowledgment,Clientid,Userid,InspectionID,InspectionDate) VALUES(" + (RegisterID == null ? "null" : "'" + RegisterID.Replace("'", "''").Trim() + "'") + ", " + (InspectedById == null ? "null" : "'" + InspectedById.Replace("'", "''").Trim() + "'") + ", " + (AckDateTime == null ? "null" : "'" + AckDateTime.Replace("'", "''").Trim() + "'") + ", " + (InspectedBy == null ? "null" : "'" + InspectedBy.Replace("'", "''").Trim() + "'") + ", " + (LicenceNo == null ? "null" : "'" + LicenceNo.Replace("'", "''").Trim() + "'") + ", " + (insertedOffline == null ? "null" : "'" + insertedOffline.Replace("'", "''").Trim() + "'") + ", " + (InspStatus == null ? "null" : "'" + InspStatus.Replace("'", "''").Trim() + "'") + ", " + (ScheduledOrAckDate == null ? "null" : "'" + ScheduledOrAckDate.Replace("'", "''").Trim() + "'") + ", " + (ddlChecklistInsp == null ? "null" : "'" + ddlChecklistInsp.Replace("'", "''").Trim() + "'") + ", " + (Inspimage2FileName == null ? "null" : "'" + Inspimage2FileName.Replace("'", "''").Trim() + "'") + ", " + (InspimageFileName == null ? "null" : "'" + InspimageFileName.Replace("'", "''").Trim() + "'") + ", " + (InspComments == null ? "null" : "'" + InspComments.Replace("'", "''").Trim() + "'") + ", " + (InspSignFileName == null ? "null" : "'" + InspSignFileName.Replace("'", "''").Trim() + "'") + ", " + (Acknowledgment == null ? "null" : "'" + Acknowledgment.Replace("'", "''").Trim() + "'") + ", " + (clientId == null ? "null" : "'" + clientId.Replace("'", "''").Trim() + "'") + ", " + (UID == null ? "null" : "'" + UID.Replace("'", "''").Trim() + "'") + ", " + (InspectionID == null ? "null" : "'" + InspectionID.Replace("'", "''").Trim() + "'") + ", " + (InspectionDate == null ? "null" : "'" + InspectionDate.Replace(" at ", " ").Trim() + "'") + ")";

            string trackUploadQuery = "INSERT INTO TblTrackUploadToCloud(ProcName,Registerid, userid, createdBy, CreatedOn, Query, Response) values('Pre-TblTempOfflineInspection_UploadedToCloud'," + (RegisterID == null ? "null" : "'" + RegisterID.Replace("'", "''").Trim() + "'") + "," + (UserId == null ? "null" : "'" + UserId.ToString() + "'") + ",'" + UserId + "',GETDATE()," + (query == null ? "null" : "'" + query.Replace("'", "''").Trim() + "'") + ",'Pre-Insert in TempTable after making query STEP 2')";


            try
            {
                var trackUploadPreInsert = databaseCon.RunCmd(trackUploadQuery);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            try
            {

                // Run the main query
                Inspectiondt = databaseCon.RunCmd(query);
                string trackUpload = "";
                if (Inspectiondt == 1)
                {
                    trackUpload = "INSERT INTO TblTrackUploadToCloud(ProcName,Registerid, userid, createdBy, CreatedOn, Query, Response) values('TblTempOfflineInspection_UploadedToCloud'," + (RegisterID == null ? "null" : "'" + RegisterID.Replace("'", "''").Trim() + "'") + "," + (UserId == null ? "null" : "'" + UserId.ToString() + "'") + ",'" + UserId + "',GETDATE()," + (query == null ? "null" : "'" + query.Replace("'", "''").Trim() + "'") + ",'Success insert in TempTable STEP 3')";
                }
                else
                {
                    trackUpload = "INSERT INTO TblTrackUploadToCloud(ProcName,Registerid, userid, createdBy, CreatedOn, Query, Response) values('TblTempOfflineInspection_UploadedToCloud'," + (RegisterID == null ? "null" : "'" + RegisterID.Replace("'", "''").Trim() + "'") + "," + (UserId == null ? "null" : "'" + UserId.ToString() + "'") + ",'" + UserId + "',GETDATE()," + (query == null ? "null" : "'" + query.Replace("'", "''").Trim() + "'") + ",'Failed insert in TempTable STEP 4')";
                }

                var trackUploadInsert = databaseCon.RunCmd(trackUpload);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);

                string trackUploadError = "INSERT INTO TblTrackUploadToCloud(ProcName,Registerid, userid, createdBy, CreatedOn, Query, Response) values('TblTempOfflineInspection_UploadedToCloud'," + (RegisterID == null ? "null" : "'" + RegisterID.Replace("'", "''").Trim() + "'") + "," + (UserId == null ? "null" : "'" + UserId.ToString() + "'") + ",'" + UserId + "',GETDATE()," + (query == null ? "null" : "'" + query.Replace("'", "''").Trim() + "'") + ",'" + ex.Message.Replace("'", "''") + "')";


                var trackUploadInsertError = databaseCon.RunCmd(trackUploadError);
            }


            //Inspectiondt = databaseCon.GetData(query);
        }
        private string SaveInspSignImageToBlob(string base64Image, string RegisterID, string InspectionID, string path)
        {
            if (string.IsNullOrEmpty(base64Image)) return null;

            //string extension = ".png";
            //string FileName = $"_{RegisterID}_{DateTime.Now.Ticks}{extension}";
            string FileName = "SketchInsp" + "_" + DateTime.Now.Ticks + "_" + InspectionID + ".png";

            string fullPath = Path.Combine(path, FileName);

            byte[] imageBytes = Convert.FromBase64String(base64Image.Split(',')[1]);
            System.IO.File.WriteAllBytes(fullPath, imageBytes);

            using (Image img = Image.FromFile(fullPath))
            {
                if (img.PropertyIdList.Contains(0x0112))
                    RotateImageIfNeeded(img);

                string NewFileName = "SketchInsp" + "_" + DateTime.Now.Ticks + "_" + InspectionID + ".png";
                SaveResizedImage(img, Path.Combine(path, NewFileName), 0.5);
                databaseCon.savefileonblob(NewFileName, "Site-images", Path.Combine(path, NewFileName), "image/jpeg");
                System.IO.File.Delete(Path.Combine(path, NewFileName));
                return NewFileName;
            }
        }

        private string SaveInspImageToBlob(string base64Image, string RegisterID, string InspectionID, string path)
        {
            if (string.IsNullOrEmpty(base64Image)) return null;

            //string extension = ".jpg";
            //string FileName = $"_{RegisterID}_{DateTime.Now.Ticks}{extension}";
            string FileName = "SnapInsp" + "_" + DateTime.Now.Ticks + "_" + InspectionID + ".jpg";
            string fullPath = Path.Combine(path, FileName);

            byte[] imageBytes = Convert.FromBase64String(base64Image.Split(',')[1]);
            System.IO.File.WriteAllBytes(fullPath, imageBytes);

            using (Image img = Image.FromFile(fullPath))
            {
                if (img.PropertyIdList.Contains(0x0112))
                    RotateImageIfNeeded(img);

                string NewFileName = "SnapInsp" + "_" + DateTime.Now.Ticks + "_" + InspectionID + ".jpg";
                SaveResizedImage(img, Path.Combine(path, NewFileName), 0.5);
                databaseCon.savefileonblob(NewFileName, "Site-images", Path.Combine(path, NewFileName), "image/jpeg");
                System.IO.File.Delete(Path.Combine(path, NewFileName));
                return NewFileName;
            }
        }

        private void ProcesstblInspectionListData(Dictionary<string, object> record, string UserId, string ClientID, string SiteId, string RID)
        {
            try
            {
                string QueryForStep1 = "INSERT INTO TblTrackUploadToCloud(ProcName,Registerid, userid, createdBy, CreatedOn, Query, Response) values('Pre-TblTempOfflineInspectionList_UploadedToCloud','" + RID + "'," + (UserId == null ? "null" : "'" + UserId.ToString() + "'") + ",'" + UserId + "',GETDATE(),'we are inti.. before  variable" + UserId + "','making query for user: " + UserId + "')";
                databaseCon.RunCmd(QueryForStep1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //DataTable InspectionListdt;
            var InspectionListdt = 0;
            // Extract the values from the dictionary, with null checks
            string N = record.ContainsKey("N") ? record["N"]?.ToString() : null;
            string NA = record.ContainsKey("NA") ? record["NA"]?.ToString() : null;
            string RegisterId = record.ContainsKey("RegisterId") ? record["RegisterId"]?.ToString() : null;
            string Y = record.ContainsKey("Y") ? record["Y"]?.ToString() : null;
            string ContentID = record.ContainsKey("ContentID") ? record["ContentID"]?.ToString() : null;
            string InspectionID = record.ContainsKey("InspectionID") ? record["InspectionID"]?.ToString() : null;
            string Status = record.ContainsKey("Status") ? record["Status"]?.ToString() : null;
            string CheckListID = record.ContainsKey("CheckListID") ? record["CheckListID"]?.ToString() : null;
            string ActionRequired = record.ContainsKey("ActionRequired") ? record["ActionRequired"]?.ToString() : null;
            try
            {
                string QueryForStep1 = "INSERT INTO TblTrackUploadToCloud(ProcName,Registerid, userid, createdBy, CreatedOn, Query, Response) values('Pre-TblTempOfflineInspectionList_UploadedToCloud'," + (RID == null ? "null" : "'" + RID.Replace("'", "''").Trim() + "'") + "," + (UserId == null ? "null" : "'" + UserId.ToString() + "'") + ",'" + UserId + "',GETDATE(),'we are inti.. the value for " + UserId + "','making query for user: " + UserId + "')";
                databaseCon.RunCmd(QueryForStep1);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            string query = "INSERT INTO TblTempOfflineInspectionList_UploadedToCloud(ActionRequired, CheckListID, ContentID, InspectionID,N, Y, NA, Status) VALUES(" + (ActionRequired == null ? "null" : "'" + ActionRequired.Replace("'", "''").Trim() + "'") + ", " + (CheckListID == null ? "null" : "'" + CheckListID.Replace("'", "''").Trim() + "'") + ", " + (ContentID == null ? "null" : "'" + ContentID.Replace("'", "''").Trim() + "'") + ", " + (InspectionID == null ? "null" : "'" + InspectionID.Replace("'", "''").Trim() + "'") + ", " + (N == null ? "null" : "'" + N.Replace("'", "''").Trim() + "'") + ", " + (Y == null ? "null" : "'" + Y.Replace("'", "''").Trim() + "'") + ", " + (NA == null ? "null" : "'" + NA.Replace("'", "''").Trim() + "'") + ", " + (Status == null ? "null" : "'" + Status.Replace("'", "''").Trim() + "'") + ")";
            string trackUploadQuery = "INSERT INTO TblTrackUploadToCloud(ProcName,Registerid, userid, createdBy, CreatedOn, Query, Response) values('Pre-TblTempOfflineInspectionList_UploadedToCloud'," + (RID == null ? "null" : "'" + RID.Replace("'", "''").Trim() + "'") + "," + (UserId == null ? "null" : "'" + UserId.ToString() + "'") + ",'" + UserId + "',GETDATE()," + (query == null ? "null" : "'" + query.Replace("'", "''").Trim() + "'") + ",'Pre-Insert in TempTable after making query STEP 2')";


            try
            {
                var trackUploadPreInsert = databaseCon.RunCmd(trackUploadQuery);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            try
            {

                // Run the main query
                InspectionListdt = databaseCon.RunCmd(query);
                string trackUpload = "";
                if (InspectionListdt == 1)
                {
                    trackUpload = "INSERT INTO TblTrackUploadToCloud(ProcName,Registerid, userid, createdBy, CreatedOn, Query, Response) values('TblTempOfflineInspectionList_UploadedToCloud'," + (RID == null ? "null" : "'" + RID.Replace("'", "''").Trim() + "'") + "," + (UserId == null ? "null" : "'" + UserId.ToString() + "'") + ",'" + UserId + "',GETDATE()," + (query == null ? "null" : "'" + query.Replace("'", "''").Trim() + "'") + ",'Success insert in TempTable STEP 3')";
                }
                else
                {
                    trackUpload = "INSERT INTO TblTrackUploadToCloud(ProcName,Registerid, userid, createdBy, CreatedOn, Query, Response) values('TblTempOfflineInspectionList_UploadedToCloud'," + (RID == null ? "null" : "'" + RID.Replace("'", "''").Trim() + "'") + "," + (UserId == null ? "null" : "'" + UserId.ToString() + "'") + ",'" + UserId + "',GETDATE()," + (query == null ? "null" : "'" + query.Replace("'", "''").Trim() + "'") + ",'Failed insert in TempTable STEP 4')";
                }

                var trackUploadInsert = databaseCon.RunCmd(trackUpload);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);

                string trackUploadError = "INSERT INTO TblTrackUploadToCloud(ProcName,Registerid, userid, createdBy, CreatedOn, Query, Response) values('TblTempOfflineInspectionList_UploadedToCloud'," + (RID == null ? "null" : "'" + RID.Replace("'", "''").Trim() + "'") + "," + (UserId == null ? "null" : "'" + UserId.ToString() + "'") + ",'" + UserId + "',GETDATE()," + (query == null ? "null" : "'" + query.Replace("'", "''").Trim() + "'") + ",'" + ex.Message.Replace("'", "''") + "')";


                var trackUploadInsertError = databaseCon.RunCmd(trackUploadError);
            }


            //InspectionListdt = databaseCon.GetData(query);
        }

        private void ProcessTableData(List<Dictionary<string, object>> tableData, Action<Dictionary<string, object>> processFunction, String UserId, string RID)
        {
            foreach (var record in tableData)
            {
                processFunction(record);
            }
        }
        private void tblPipelineRequestData(Dictionary<string, object> record, string UserId, string ClientID, string SiteId, string RID)
        {
            try
            {
                // Extract the values from the dictionary, with null checks
                string PipeLineDesc = record.ContainsKey("PipeLineDesc") ? record["PipeLineDesc"]?.ToString() : null;
                string InventoryId = record.ContainsKey("InventoryId") ? record["InventoryId"]?.ToString() : null;
                string REGISTERId = record.ContainsKey("REGISTERId") ? record["REGISTERId"]?.ToString() : null;
                string MaterialType = record.ContainsKey("MaterialType") ? record["MaterialType"]?.ToString() : null;
                string ItemCode = record.ContainsKey("ItemCode") ? record["ItemCode"]?.ToString() : null;
                string ItemDesc = record.ContainsKey("ItemDesc") ? record["ItemDesc"]?.ToString() : null;
                string Quantity = record.ContainsKey("Quantity") ? record["Quantity"]?.ToString() : null;
                string clientId = ClientID;
                string siteId = SiteId;
                string UID = UserId;

                var result = btnSaveInventory_Click(UID, PipeLineDesc, InventoryId, REGISTERId, MaterialType, ItemCode, ItemDesc, Quantity, clientId, siteId);
                //if (result is JsonResult jsonResult)
                //{
                //    var data = jsonResult.Data;
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        public ActionResult btnSaveInventory_Click(string UID, string PipeLineDesc, string InventoryId, string REGISTERId, string MaterialType, string ItemCode, string ItemDesc, string Quantity, string clientId, string siteId)
        {
            try
            {
                try
                {
                    string QueryForStep1 = "INSERT INTO TblTrackUploadToCloud(ProcName,Registerid, userid, createdBy, CreatedOn, Query, Response) values('Pre-tblPipelineRequest','" + REGISTERId + "'," + (UID == null ? "null" : "'" + UID.ToString() + "'") + ",'" + UID + "',GETDATE(),'we are inti.. before  variable" + UID + "','making query for user: " + UID + "')";
                    databaseCon.RunCmd(QueryForStep1);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                string qryCreatedBy = "Select distinct emailid from tbluser where UserId= '" + UID + "'";
                DataTable dt = databaseCon.GetData(qryCreatedBy);
                string createdBy = dt.Rows[0]["emailid"].ToString();

                // Build the SQL query with the new file name and other data
                //string query = "if not exists(select 'x' from tblPipelineRequest where JobId='" + REGISTERId + "' and InventoryId='" + InventoryId + "' and userid='" + UID + "' and siteid='" + siteId + "' ) begin insert into tblPipelineRequest(UserId,ClientId,InventoryId,siteId,JobId,Quantity,CreatedBy,CreatedOn,ItemCode,ItemDesc,MaterialType,PipeLineDesc)values('" + UID + "', '" + clientId + "', '" + InventoryId + "', '" + siteId + "', '" + REGISTERId + "', '" + Quantity + "', '" + createdBy + "', '" + DateTime.Now + "', '" + ItemCode.Replace("'", "''") + "', '" + ItemDesc.Replace("'", "''") + "', '" + MaterialType.Replace("'", "''") + "', '" + PipeLineDesc.Replace("'", "''") + "') end else begin update tblPipelineRequest set Quantity='" + Quantity + "',PipeLineDesc='" + PipeLineDesc.Replace("'", "''") + "',UpdatedBy=  '" + createdBy + "', updatedon='" + DateTime.Now + "' where JobId='" + REGISTERId + "' and InventoryId='" + InventoryId + "' and userid='" + UID + "' and siteid='" + siteId + "' end";


                //string deletequery = "delete from tblPipelineRequest  where JobId='" + REGISTERId + "'  and userid='" + UID + "' and siteid='" + siteId + "'";
                //int result1 = databaseCon.RunCmd(deletequery);
                try
                {
                    string QueryForStep1 = "INSERT INTO TblTrackUploadToCloud(ProcName,Registerid, userid, createdBy, CreatedOn, Query, Response) values('Pre-tblPipelineRequest'," + (REGISTERId == null ? "null" : "'" + REGISTERId.Replace("'", "''").Trim() + "'") + "," + (UID == null ? "null" : "'" + UID.ToString() + "'") + ",'" + UID + "',GETDATE(),'we are inti.. the value for " + UID + "','making query for user: " + UID + "')";
                    databaseCon.RunCmd(QueryForStep1);
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }

                string query = "  insert into tblPipelineRequest(UserId,ClientId,InventoryId,siteId,JobId,Quantity,CreatedBy,CreatedOn,ItemCode,ItemDesc,MaterialType,PipeLineDesc)values('" + UID + "', '" + clientId + "', '" + InventoryId + "', '" + siteId + "', '" + REGISTERId + "', '" + Quantity + "', '" + createdBy + "', '" + DateTime.Now + "', '" + ItemCode.Replace("'", "''") + "', '" + ItemDesc.Replace("'", "''") + "', '" + MaterialType.Replace("'", "''") + "', '" + PipeLineDesc.Replace("'", "''") + "')  ";
                // Execute the query
                int result = databaseCon.RunCmd(query);
                string trackUpload = "";
                if (result > 0)
                {
                    trackUpload = "INSERT INTO TblTrackUploadToCloud(ProcName,Registerid, userid, createdBy, CreatedOn, Query, Response) values('tblPipelineRequest'," + (REGISTERId == null ? "null" : "'" + REGISTERId.Replace("'", "''").Trim() + "'") + "," + (UID == null ? "null" : "'" + UID.ToString() + "'") + ",'" + UID + "',GETDATE()," + (query == null ? "null" : "'" + query.Replace("'", "''").Trim() + "'") + ",'Success insert in Pipeline STEP 1')";
                    int trackUploadPipeline = databaseCon.RunCmd(trackUpload);
                    return Json(new { success = true, message = "Data saved successfully." });
                }
                else
                {
                    trackUpload = "INSERT INTO TblTrackUploadToCloud(ProcName,Registerid, userid, createdBy, CreatedOn, Query, Response) values('tblPipelineRequest'," + (REGISTERId == null ? "null" : "'" + REGISTERId.Replace("'", "''").Trim() + "'") + "," + (UID == null ? "null" : "'" + UID.ToString() + "'") + ",'" + UID + "',GETDATE()," + (query == null ? "null" : "'" + query.Replace("'", "''").Trim() + "'") + ",'Failed insert in Pipeline STEP 2')";
                    int trackUploadPipeline = databaseCon.RunCmd(trackUpload);
                    return Json(new { success = false, message = "Failed to save data." });
                }



            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }
        public ActionResult DeletePipelineData(string LoginUserId, string clientId, string Siteid, string UserRole, string RID)
        {
            //commented as userid check not required for inventory, any user can see or delete inventory added by another user.
            //string deletequery = "delete from tblPipelineRequest  where JobId='" + RID + "'  and userid='" + LoginUserId + "' and siteid='" + Siteid + "'";
            string deletequery = "delete from tblPipelineRequest  where JobId='" + RID + "'  and siteid='" + Siteid + "'";
            int result = databaseCon.RunCmd(deletequery);

            if (result > 0)
            {
                return Json(new { Success = true, message = "Data saved successfully." });
            }
            else
            {
                return Json(new { Success = false, message = "Failed to save data." });
            }

        }
        //private void ProcessRegisterDetails(Dictionary<string, object> record , string UserId) {
        //    DataTable registerdt;
        //    //Basic Info
        //    string registerId = record.ContainsKey("REGISTERId") ? record["REGISTERId"]?.ToString() : null;
        //    string clientId = record.ContainsKey("ClientID") ? record["ClientID"]?.ToString() : null;
        //    string TxtOwanerName = record.ContainsKey("OwnerName") ? record["OwnerName"]?.ToString() : null;

        //    string FloorLevelId = record.ContainsKey("FloorLevelId") ? record["FloorLevelId"]?.ToString() : null;
        //    string FloorLevel = record.ContainsKey("FloorLevel") ? record["FloorLevel"]?.ToString() : null;
        //    string locationId = record.ContainsKey("LocationId") ? record["LocationId"]?.ToString() : null;
        //    string location = record.ContainsKey("Location") ? record["Location"]?.ToString() : null;
        //    string siteLocId = record.ContainsKey("SiteLocID") ? record["SiteLocID"]?.ToString() : null;
        //    string SiteLocation = record.ContainsKey("SiteLocation") ? record["SiteLocation"]?.ToString() : null;
        //    string buildingId = record.ContainsKey("BuildingID") ? record["BuildingID"]?.ToString() : null;
        //    string buildingNo = record.ContainsKey("BuildingNo") ? record["BuildingNo"]?.ToString() : null;
        //    string AssetNo = record.ContainsKey("AssetNo") ? record["AssetNo"]?.ToString() : null;
        //    string AssetId = record.ContainsKey("AssetId") ? record["AssetId"]?.ToString() : null;
        //    var AssetDesc = "";
        //    var parts = AssetNo.Split('-');
        //    if (parts.Length > 1)
        //    {
        //        AssetDesc = parts[1].Trim();  
        //    }
        //    string bearing = record.ContainsKey("Bearing") ? record["Bearing"]?.ToString() : null;
        //    string ddlWorkGroups = record.ContainsKey("WorkGroup") ? record["WorkGroup"]?.ToString() : null;
        //    string ddlSubGroups = record.ContainsKey("SubGroup") ? record["SubGroup"]?.ToString() : null;
        //    string RequestedBy = record.ContainsKey("RequestedBy") ? record["RequestedBy"]?.ToString() : null;
        //   //details section var  
        //    string dateTagged = record.ContainsKey("DateTagged") ? record["DateTagged"]?.ToString() : null;
        //    string dateOfDis = record.ContainsKey("DateOfDis") ? record["DateOfDis"]?.ToString() : null;
        //    string designType = record.ContainsKey("DesignType") ? record["DesignType"]?.ToString() : null;
        //    string dutyRating = record.ContainsKey("DutyRating") ? record["DutyRating"]?.ToString() : null;
        //    string materialType = record.ContainsKey("MaterialType") ? record["MaterialType"]?.ToString() : null;
        //    string description = record.ContainsKey("Description") ? record["Description"]?.ToString() : null;
        //    string inspTimeFrame = record.ContainsKey("InspTimeFrame") ? record["InspTimeFrame"]?.ToString() : null;
        //    var LongTermStructure = record.ContainsKey("LongTermStructure") ? record["LongTermStructure"]?.ToString() : null;
        //    if(LongTermStructure == "True")
        //    {
        //        LongTermStructure = "1";
        //    }
        //    var ChkRequiresIsolation = record.ContainsKey("ChkRequiresIsolation") ? record["ChkRequiresIsolation"]?.ToString() : null;
        //    if (ChkRequiresIsolation == "True")
        //    {
        //        ChkRequiresIsolation = "1";
        //    }
        //    string NoInventory = record.ContainsKey("NoInventory") ? record["NoInventory"]?.ToString() : null;
        //    if (NoInventory == "True")
        //    {
        //        NoInventory = "1";
        //    }
        //    // More Info Section
        //    string woNumber = record.ContainsKey("WONumber") ? record["WONumber"]?.ToString() : null;
        //    string length = record.ContainsKey("Length") ? record["Length"]?.ToString() : null;
        //    string height = record.ContainsKey("Height") ? record["Height"]?.ToString() : null;
        //    string width = record.ContainsKey("Width") ? record["Width"]?.ToString() : null;
        //    string noOfWorkingDecks = record.ContainsKey("NoOfWorkingDecks") ? record["NoOfWorkingDecks"]?.ToString() : null;
        //    string manHrsErect = record.ContainsKey("ManHrsErect") && !string.IsNullOrWhiteSpace(record["ManHrsErect"]?.ToString())? record["ManHrsErect"]?.ToString(): null;
        //    string manHrsDismantle = record.ContainsKey("ManHrsDismantle") && !string.IsNullOrWhiteSpace(record["ManHrsDismantle"]?.ToString())? record["ManHrsDismantle"]?.ToString(): null;

        //    string PlannedStandingDuration = record.ContainsKey("PlannedStandingDuration") ? record["PlannedStandingDuration"]?.ToString() : null;
        //    string PlanningStatus = record.ContainsKey("PlanningStatus") ? record["PlanningStatus"]?.ToString() : null;
        //    string EngineeredDesign = record.ContainsKey("EngineeredDesign") ? record["EngineeredDesign"]?.ToString() : null;
        //    string DesignDrawingNo = record.ContainsKey("DesignDrawingNo") ? record["DesignDrawingNo"]?.ToString() : null;
        //    string SupervisorId = record.ContainsKey("Supervisor") ? record["Supervisor"]?.ToString() : null;
        //    string Crew = record.ContainsKey("Crew") ? record["Crew"]?.ToString() : null;
        //    string PermitNo = record.ContainsKey("PermitNo") ? record["PermitNo"]?.ToString() : null;
        //    string CrewSize = record.ContainsKey("CrewSize") ? record["CrewSize"]?.ToString() : null;
        //    string comments = record.ContainsKey("Comments") ? record["Comments"]?.ToString() : null;
        //    string CycloneAction = record.ContainsKey("CycloneAction") ? record["CycloneAction"]?.ToString() : null;
        //    string PriorityRiskRating = record.ContainsKey("PriorityRiskRating") ? record["PriorityRiskRating"]?.ToString() : null;
        //    string TotalLabourDismantleEdit = record.ContainsKey("TotalLabourDismantleEdit") ? record["TotalLabourDismantleEdit"]?.ToString() : null;
        //    string WithInRangeOfOutBoard = record.ContainsKey("WithInRangeOfOutBoard") ? record["WithInRangeOfOutBoard"]?.ToString() : null;
        //    string UseEnteredValue = record.ContainsKey("UseEnteredValue") ? record["UseEnteredValue"]?.ToString() : null;
        //    string NoOfItems = record.ContainsKey("NoOfItems") ? record["NoOfItems"]?.ToString() : null;
        //    string Tonnage = record.ContainsKey("Tonnage") ? record["Tonnage"]?.ToString() : null;


        //    string PhoToAngle = record.ContainsKey("PhotoAngle") ? record["PhotoAngle"]?.ToString() : null;
        //    string radUpdogImg = record.ContainsKey("radUpdogImg") ? record["radUpdogImg"]?.ToString() : null;
        //    string NumberOfPlatform = record.ContainsKey("NumberOfPlatform") ? record["NumberOfPlatform"]?.ToString() : null;
        //    string basetempimg = record.ContainsKey("basetempimg") ? record["basetempimg"]?.ToString() : null;
        //    string tempimg = record.ContainsKey("tempimg") ? record["tempimg"]?.ToString() : null;

        //    string Image = record.ContainsKey("Image") ? record["Image"]?.ToString() : null;

        //    string query = "INSERT INTO TblTempOfflineRegister_UploadedToCloud(RegisterId, ClientID, SiteLocId, LocationId, LocationName, BuildingID, BuildingNo, AssetId, AssetName, AssetDesc, FloorLevelId, FloorLevel, Bearing, WorkGroup, SubGroup, RequestedBy, DateTagged, DateOfDis, DesignType, DutyRating, MaterialType, Description, InspTimeFrame, NoInventory, LongTermStructure, ChkRequiresIsolation, WONumber, Length, Height, Width, NoOfWorkingDecks, ManHrsErect, ManHrsDismantle, PlannedStandingDuration, PlanningStatus, EngineeredDesign, DesignDrawingNo, NumberOfPlatform, SupervisorId, Crew, PermitNo, CrewSize, Comments, PhotoAngle, UserID, CycloneAction, PriorityRiskRating, UseEnteredValue, WithInRangeOfOutBoard, OriginalBaseImage, Image, NoOfItems, Tonnage) VALUES(" + (registerId == null ? "null" : "'" + registerId.Replace("'", "''").Trim() + "'") + ", " + (clientId == null ? "null" : "'" + clientId.Replace("'", "''").Trim() + "'") + ", " + (siteLocId == null ? "null" : "'" + siteLocId.Replace("'", "''").Trim() + "'") + ", NULL, " + (location == null ? "null" : "'" + location.Replace("'", "''").Trim() + "'") + ", NULL, " + (buildingNo == null ? "null" : "'" + buildingNo.Replace("'", "''").Trim() + "'") + ", NULL, " + (AssetNo == null ? "null" : "'" + AssetNo.Replace("'", "''").Trim() + "'") + ", " + (AssetDesc == null ? "null" : "'" + AssetDesc.Replace("'", "''").Trim() + "'") + ", NULL, " + (FloorLevel == null ? "null" : "'" + FloorLevel.Replace("'", "''").Trim() + "'") + ", " + (bearing == null ? "null" : "'" + bearing.Replace("'", "''").Trim() + "'") + ", " + (ddlWorkGroups == null ? "null" : "'" + ddlWorkGroups.Replace("'", "''").Trim() + "'") + ", " + (ddlSubGroups == null ? "null" : "'" + ddlSubGroups.Replace("'", "''").Trim() + "'") + ", " + (RequestedBy == null ? "null" : "'" + RequestedBy.Replace("'", "''").Trim() + "'") + ", " + (dateTagged == null ? "null" : "'" + dateTagged.Replace("'", "''").Trim() + "'") + ", " + (dateOfDis == null ? "null" : "'" + dateOfDis.Replace("'", "''").Trim() + "'") + ", " + (designType == null ? "null" : "'" + designType.Replace("'", "''").Trim() + "'") + ", " + (dutyRating == null ? "null" : "'" + dutyRating.Replace("'", "''").Trim() + "'") + ", " + (materialType == null ? "null" : "'" + materialType.Replace("'", "''").Trim() + "'") + ", " + (description == null ? "null" : "'" + description.Replace("'", "''").Trim() + "'") + ", " + (inspTimeFrame == null ? "null" : "'" + inspTimeFrame.Replace("'", "''").Trim() + "'") + ", " + (NoInventory == null ? "null" : NoInventory.ToString()) + ", " + (LongTermStructure == null ? "null" : LongTermStructure.ToString()) + ", " + (ChkRequiresIsolation == null ? "null" : ChkRequiresIsolation.ToString()) + ", " + (woNumber == null ? "null" : "'" + woNumber.Replace("'", "''").Trim() + "'") + ", " + (length == null ? "null" : length.ToString()) + ", " + (height == null ? "null" : height.ToString()) + ", " + (width == null ? "null" : width.ToString()) + ", " + (noOfWorkingDecks == null ? "null" : "'" + noOfWorkingDecks.Replace("'", "''").Trim() + "'") + ", " + (manHrsErect == null ? "null" : manHrsErect.ToString()) + ", " + (manHrsDismantle == null ? "null" : manHrsDismantle.ToString()) + ", " + (PlannedStandingDuration == null ? "null" : PlannedStandingDuration.ToString()) + ", " + (PlanningStatus == null ? "null" : "'" + PlanningStatus.Replace("'", "''").Trim() + "'") + ", " + (EngineeredDesign == null ? "null" : "'" + EngineeredDesign.Replace("'", "''").Trim() + "'") + ", " + (DesignDrawingNo == null ? "null" : "'" + DesignDrawingNo.Replace("'", "''").Trim() + "'") + ", " + (NumberOfPlatform == null ? "null" : "'" + NumberOfPlatform.Replace("'", "''").Trim() + "'") + ", " + (SupervisorId == null ? "null" : SupervisorId.ToString()) + ", " + (Crew == null ? "null" : "'" + Crew.Replace("'", "''").Trim() + "'") + ", " + (PermitNo == null ? "null" : "'" + PermitNo.Replace("'", "''").Trim() + "'") + ", " + (CrewSize == null ? "null" : CrewSize.ToString()) + ", " + (comments == null ? "null" : "'" + comments.Replace("'", "''").Trim() + "'") + ", " + (PhoToAngle == null ? "null" : "'" + PhoToAngle.Replace("'", "''").Trim() + "'") + ", " + (UserId == null ? "null" : UserId.ToString()) + ", " + (CycloneAction == null ? "null" : "'" + CycloneAction.Replace("'", "''").Trim() + "'") + ", " + (PriorityRiskRating == null ? "null" : "'" + PriorityRiskRating.Replace("'", "''").Trim() + "'") + ", " + (UseEnteredValue == null ? "null" : UseEnteredValue.ToString()) + ", " + (WithInRangeOfOutBoard == null ? "null" : WithInRangeOfOutBoard.ToString()) + ", " + (Image == null ? "null" : "'" + Image.Replace("'", "''").Trim() + "'") + ", " + (Image == null ? "null" : "'" + Image.Replace("'", "''").Trim() + "'") + ", " + (NoOfItems == null ? "null" : "'" + NoOfItems.Replace("'", "''").Trim() + "'") + ", " + (Tonnage == null ? "null" : Tonnage.ToString()) + ");";



        //    registerdt = databaseCon.GetData(query);

        //    //RegisterController.UpdateImagePath(string id, string RID, string txtOwnername, string radUpdogImg, string imgname, string tempimg, string basetempimg)
        //    UpdateImagePath(clientId, registerId, TxtOwanerName, radUpdogImg, "test image.png", tempimg, basetempimg);


        //}
        private void ProcessRegisterDetails(Dictionary<string, object> record, string UserId, string RID)
        {
            try
            {
                string QueryForStep1 = "INSERT INTO TblTrackUploadToCloud(ProcName,Registerid, userid, createdBy, CreatedOn, Query, Response) values('Pre-TblTempOfflineRegister_UploadedToCloud','" + RID + "'," + (UserId == null ? "null" : "'" + UserId.ToString() + "'") + ",'" + UserId + "',GETDATE(),'we are inti.. before  variable" + UserId + "','making query for user: " + UserId + "')";
                databaseCon.RunCmd(QueryForStep1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            //DataTable registerdt;
            var registerdt = 0;
            //Basic Info
            string registerId = record.ContainsKey("REGISTERId") ? record["REGISTERId"]?.ToString() : null;
            string clientId = record.ContainsKey("ClientID") ? record["ClientID"]?.ToString() : null;
            string TxtOwanerName = record.ContainsKey("OwnerName") ? record["OwnerName"]?.ToString() : null;

            string FloorLevelId = record.ContainsKey("FloorLevelId") ? record["FloorLevelId"]?.ToString() : null;
            string FloorLevel = record.ContainsKey("FloorLevel") ? record["FloorLevel"]?.ToString() : null;
            string locationId = record.ContainsKey("LocationId") ? record["LocationId"]?.ToString() : null;
            string location = record.ContainsKey("Location") ? record["Location"]?.ToString() : null;
            string siteLocId = record.ContainsKey("SiteLocID") ? record["SiteLocID"]?.ToString() : null;
            string SiteLocation = record.ContainsKey("SiteLocation") ? record["SiteLocation"]?.ToString() : null;
            string buildingId = record.ContainsKey("BuildingID") ? record["BuildingID"]?.ToString() : null;
            //string buildingNo = record.ContainsKey("BuildingNo") ? record["BuildingNo"]?.ToString() : null;
            string buildingNo = record.ContainsKey("BuildingNo") && !string.IsNullOrWhiteSpace(record["BuildingNo"]?.ToString()) ? record["BuildingNo"].ToString() : null;

            //string AssetNo = record.ContainsKey("AssetNo") ? record["AssetNo"]?.ToString() : null;
            string AssetNo = record.ContainsKey("AssetNo") && !string.IsNullOrWhiteSpace(record["AssetNo"]?.ToString()) ? record["AssetNo"].ToString() : null;

            string AssetId = record.ContainsKey("AssetId") ? record["AssetId"]?.ToString() : null;
            var AssetDesc = "";
            if (!AssetNo.IsNullOrWhiteSpace())
            {
                var parts = AssetNo.Split('~');
                if (parts.Length > 1)
                {
                    AssetNo = parts[0].Trim();
                    AssetDesc = parts[1].Trim();
                }
            }
            string bearing = record.ContainsKey("Bearing") && !string.IsNullOrWhiteSpace(record["Bearing"]?.ToString()) ? record["Bearing"].ToString() : null;
            string ddlWorkGroups = record.ContainsKey("WorkGroup") && !string.IsNullOrWhiteSpace(record["WorkGroup"]?.ToString()) ? record["WorkGroup"].ToString() : null;
            string ddlSubGroups = record.ContainsKey("SubGroup") && !string.IsNullOrWhiteSpace(record["SubGroup"]?.ToString()) ? record["SubGroup"].ToString() : null;
            //string RequestedBy = record.ContainsKey("RequestedBy") ? record["RequestedBy"]?.ToString() : null;
            string RequestedBy = record.ContainsKey("RequestedBy") && !string.IsNullOrWhiteSpace(record["RequestedBy"]?.ToString()) ? record["RequestedBy"].ToString() : null;
            //details section var  
            //string dateTagged = record.ContainsKey("DateTagged") && !string.IsNullOrWhiteSpace(record["DateTagged"]?.ToString()) ? record["DateTagged"].ToString() : null;
            //string dateOfDis = record.ContainsKey("DateOfDis") && !string.IsNullOrWhiteSpace(record["DateOfDis"]?.ToString()) ? record["DateOfDis"].ToString() : null;
            string dateTagged = record.ContainsKey("DateTagged") && !string.IsNullOrWhiteSpace(record["DateTagged"]?.ToString()) ? record["DateTagged"].ToString() : "1900-01-01";
            string dateOfDis = record.ContainsKey("DateOfDis") && !string.IsNullOrWhiteSpace(record["DateOfDis"]?.ToString()) ? record["DateOfDis"].ToString() : "1900-01-01";
            string designType = record.ContainsKey("DesignType") && !string.IsNullOrWhiteSpace(record["DesignType"]?.ToString()) ? record["DesignType"].ToString() : null;
            string dutyRating = record.ContainsKey("DutyRating") && !string.IsNullOrWhiteSpace(record["DutyRating"]?.ToString()) ? record["DutyRating"].ToString() : null;
            string materialType = record.ContainsKey("MaterialType") && !string.IsNullOrWhiteSpace(record["MaterialType"]?.ToString()) ? record["MaterialType"].ToString() : null;
            string description = record.ContainsKey("Description") && !string.IsNullOrWhiteSpace(record["Description"]?.ToString()) ? record["Description"].ToString() : null;
            string inspTimeFrame = record.ContainsKey("InspTimeframe") && !string.IsNullOrWhiteSpace(record["InspTimeframe"]?.ToString()) ? record["InspTimeframe"].ToString() : null;
            var LongTermStructure = record.ContainsKey("LongTermStructure") && !string.IsNullOrWhiteSpace(record["LongTermStructure"]?.ToString()) ? record["LongTermStructure"].ToString() : null;
            if (LongTermStructure == "True")
            {
                LongTermStructure = "1";
            }
            else
            {
                LongTermStructure = "0";

            }
            var ChkRequiresIsolation = record.ContainsKey("ChkRequiresIsolation") ? record["ChkRequiresIsolation"]?.ToString() : null;
            if (ChkRequiresIsolation == "True")
            {
                ChkRequiresIsolation = "1";
            }
            else
            {
                ChkRequiresIsolation = "0";
            }
            string NoInventory = record.ContainsKey("NoInventory") ? record["NoInventory"]?.ToString() : null;
            if (NoInventory == "True")
            {
                NoInventory = "1";
            }
            else
            {
                NoInventory = "0";
            }
            // More Info Section
            string woNumber = record.ContainsKey("WONumber") && !string.IsNullOrWhiteSpace(record["WONumber"]?.ToString()) ? record["WONumber"].ToString() : null;
            string length = record.ContainsKey("Length") && !string.IsNullOrWhiteSpace(record["Length"]?.ToString()) ? record["Length"].ToString() : null;
            string height = record.ContainsKey("Height") && !string.IsNullOrWhiteSpace(record["Height"]?.ToString()) ? record["Height"].ToString() : null;
            string width = record.ContainsKey("Width") && !string.IsNullOrWhiteSpace(record["Width"]?.ToString()) ? record["Width"].ToString() : null;
            string noOfWorkingDecks = record.ContainsKey("NoOfWorkingDecks") ? record["NoOfWorkingDecks"]?.ToString() : null;
            string manHrsErect = record.ContainsKey("ManHrsErect") && !string.IsNullOrWhiteSpace(record["ManHrsErect"]?.ToString()) ? record["ManHrsErect"]?.ToString() : null;
            string manHrsDismantle = record.ContainsKey("ManHrsDismantle") && !string.IsNullOrWhiteSpace(record["ManHrsDismantle"]?.ToString()) ? record["ManHrsDismantle"]?.ToString() : null;

            string PlannedStandingDuration = record.ContainsKey("PlannedStandingDuration") ? record["PlannedStandingDuration"]?.ToString() : null;
            string PlanningStatus = record.ContainsKey("PlanningStatus") && !string.IsNullOrWhiteSpace(record["PlanningStatus"]?.ToString()) ? record["PlanningStatus"].ToString() : null;
            string EngineeredDesign = record.ContainsKey("EngineeredDesign") && !string.IsNullOrWhiteSpace(record["EngineeredDesign"]?.ToString()) ? record["EngineeredDesign"].ToString() : null;
            string DesignDrawingNo = record.ContainsKey("DesignDrawingNo") && !string.IsNullOrWhiteSpace(record["DesignDrawingNo"]?.ToString()) ? record["DesignDrawingNo"].ToString() : null;
            string SupervisorId = record.ContainsKey("SupervisorId") && !string.IsNullOrWhiteSpace(record["SupervisorId"]?.ToString()) ? record["SupervisorId"].ToString() : null;
            string Crew = record.ContainsKey("Crew") && !string.IsNullOrWhiteSpace(record["Crew"]?.ToString()) ? record["Crew"].ToString() : null;
            string PermitNo = record.ContainsKey("PermitNo") && !string.IsNullOrWhiteSpace(record["PermitNo"]?.ToString()) ? record["PermitNo"].ToString() : null;
            string CrewSize = record.ContainsKey("CrewSize") && !string.IsNullOrWhiteSpace(record["CrewSize"]?.ToString()) ? record["CrewSize"].ToString() : null;

            string comments = record.ContainsKey("Comments") && !string.IsNullOrWhiteSpace(record["Comments"]?.ToString()) ? record["Comments"].ToString() : null;
            string CycloneAction = record.ContainsKey("CycloneAction") && !string.IsNullOrWhiteSpace(record["CycloneAction"]?.ToString()) ? record["CycloneAction"].ToString() : null;
            string PriorityRiskRating = record.ContainsKey("PriorityRiskRating") && !string.IsNullOrWhiteSpace(record["PriorityRiskRating"]?.ToString()) ? record["PriorityRiskRating"].ToString() : null;
            string TotalLabourDismantleEdit = record.ContainsKey("TotalLabourDismantleEdit") && !string.IsNullOrWhiteSpace(record["TotalLabourDismantleEdit"]?.ToString()) ? record["TotalLabourDismantleEdit"].ToString() : null;
            string WithInRangeOfOutBoard = record.ContainsKey("WithInRangeOfOutBoard") && !string.IsNullOrWhiteSpace(record["WithInRangeOfOutBoard"]?.ToString()) ? record["WithInRangeOfOutBoard"].ToString() : null;
            if (WithInRangeOfOutBoard == "True" || WithInRangeOfOutBoard == "1")
            {
                WithInRangeOfOutBoard = "1";
            }
            else
            {
                WithInRangeOfOutBoard = "0";
            }
            string UseEnteredValue = record.ContainsKey("UseEnteredValue") ? record["UseEnteredValue"]?.ToString() : null;
            if (UseEnteredValue == "True")
            {
                UseEnteredValue = "1";
            }
            else
            {
                UseEnteredValue = "0";
            }
            string NoOfItems = record.ContainsKey("NoOfItems") ? record["NoOfItems"]?.ToString() : null;
            string Tonnage = record.ContainsKey("Tonnage") ? record["Tonnage"]?.ToString() : null;


            string PhoToAngle = record.ContainsKey("PhotoAngle") ? record["PhotoAngle"]?.ToString() : null;
            string radUpdogImg = record.ContainsKey("radUpdogImg") ? record["radUpdogImg"]?.ToString() : null;
            string NumberOfPlatform = record.ContainsKey("NumberOfPlatform") ? record["NumberOfPlatform"]?.ToString() : null;
            string basetempimg = record.ContainsKey("basetempimg") ? record["basetempimg"]?.ToString() : null;
            string tempimg = record.ContainsKey("tempimg") ? record["tempimg"]?.ToString() : null;

            string Image = record.ContainsKey("Image") ? record["Image"]?.ToString() : null;
            string path = Server.MapPath("/images/sites/");
            string ImageFileName = SavejobImageToBlob(Image, TxtOwanerName, registerId, path);


            try
            {
                string QueryForStep1 = "INSERT INTO TblTrackUploadToCloud(ProcName,Registerid, userid, createdBy, CreatedOn, Query, Response) values('Pre-TblTempOfflineRegister_UploadedToCloud'," + (registerId == null ? "null" : "'" + registerId.Replace("'", "''").Trim() + "'") + "," + (UserId == null ? "null" : "'" + UserId.ToString() + "'") + ",'" + UserId + "',GETDATE(),'we are inti.. the value for " + UserId + "','making query for user: " + UserId + "')";
                databaseCon.RunCmd(QueryForStep1);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }


            string query = "INSERT INTO TblTempOfflineRegister_UploadedToCloud(RegisterId, ClientID, SiteLocId, LocationId, LocationName, BuildingID, BuildingNo, AssetId, AssetName, AssetDesc, FloorLevelId, FloorLevel, Bearing, WorkGroup, SubGroup, RequestedBy, DateTagged, DateOfDis, DesignType, DutyRating, MaterialType, Description, InspTimeFrame, NoInventory, LongTermStructure, ChkRequiresIsolation, WONumber, Length, Height, Width, NoOfWorkingDecks, ManHrsErect, ManHrsDismantle, PlannedStandingDuration, PlanningStatus, EngineeredDesign, DesignDrawingNo, NumberOfPlatform, SupervisorId, Crew, PermitNo, CrewSize, Comments, PhotoAngle, UserID, CycloneAction, PriorityRiskRating, TotalLabourDismantleEdit, UseEnteredValue, WithInRangeOfOutBoard, OriginalBaseImage, Image) VALUES(" + (registerId == null ? "null" : "'" + registerId.Replace("'", "''").Trim() + "'") + ", " + (clientId == null ? "null" : "'" + clientId.Replace("'", "''").Trim() + "'") + ", " + (siteLocId == null ? "null" : "'" + siteLocId.Replace("'", "''").Trim() + "'") + ", NULL, " + (location == null ? "null" : "'" + location.Replace("'", "''").Trim() + "'") + ", NULL, " + (buildingNo == null ? "null" : "'" + buildingNo.Replace("'", "''").Trim() + "'") + ", NULL, " + (AssetNo == null ? "null" : "'" + AssetNo.Replace("'", "''").Trim() + "'") + ", " + (AssetDesc == null || AssetDesc == "" ? "null" : "'" + AssetDesc.Replace("'", "''").Trim() + "'") + ", NULL, " + (FloorLevel == null ? "null" : "'" + FloorLevel.Replace("'", "''").Trim() + "'") + ", " + (bearing == null ? "null" : "'" + bearing.Replace("'", "''").Trim() + "'") + ", " + (ddlWorkGroups == null ? "null" : "'" + ddlWorkGroups.Replace("'", "''").Trim() + "'") + ", " + (ddlSubGroups == null ? "null" : "'" + ddlSubGroups.Replace("'", "''").Trim() + "'") + ", " + (RequestedBy == null ? "null" : "'" + RequestedBy.Replace("'", "''").Trim() + "'") + ", " + (dateTagged == null ? "null" : "'" + dateTagged.Replace("'", "''").Trim() + "'") + ", " + (dateOfDis == null ? "null" : "'" + dateOfDis.Replace("'", "''").Trim() + "'") + ", " + (designType == null ? "null" : "'" + designType.Replace("'", "''").Trim() + "'") + ", " + (dutyRating == null ? "null" : "'" + dutyRating.Replace("'", "''").Trim() + "'") + ", " + (materialType == null ? "null" : "'" + materialType.Replace("'", "''").Trim() + "'") + ", " + (description == null ? "null" : "'" + description.Replace("'", "''").Trim() + "'") + ", " + (inspTimeFrame == null ? "null" : "'" + inspTimeFrame.Replace("'", "''").Trim() + "'") + ", " + (NoInventory == null ? "null" : NoInventory.ToString()) + ", " + (LongTermStructure == null ? "null" : LongTermStructure.ToString()) + ", " + (ChkRequiresIsolation == null ? "null" : ChkRequiresIsolation.ToString()) + ", " + (woNumber == null ? "null" : "'" + woNumber.Replace("'", "''").Trim() + "'") + ", " + (length == null ? "null" : length.ToString()) + ", " + (height == null ? "null" : height.ToString()) + ", " + (width == null ? "null" : width.ToString()) + ", " + (noOfWorkingDecks == null ? "null" : "'" + noOfWorkingDecks.Replace("'", "''").Trim() + "'") + ", " + (manHrsErect == null ? "null" : manHrsErect.ToString()) + ", " + (manHrsDismantle == null ? "null" : manHrsDismantle.ToString()) + ", " + (PlannedStandingDuration == null ? "null" : PlannedStandingDuration.ToString()) + ", " + (PlanningStatus == null ? "null" : "'" + PlanningStatus.Replace("'", "''").Trim() + "'") + ", " + (EngineeredDesign == null ? "null" : "'" + EngineeredDesign.Replace("'", "''").Trim() + "'") + ", " + (DesignDrawingNo == null ? "null" : "'" + DesignDrawingNo.Replace("'", "''").Trim() + "'") + ", " + (NumberOfPlatform == null ? "null" : "'" + NumberOfPlatform.Replace("'", "''").Trim() + "'") + ", " + (SupervisorId == null ? "null" : SupervisorId.ToString()) + ", " + (Crew == null ? "null" : "'" + Crew.Replace("'", "''").Trim() + "'") + ", " + (PermitNo == null ? "null" : "'" + PermitNo.Replace("'", "''").Trim() + "'") + ", " + (CrewSize == null ? "null" : CrewSize.ToString()) + ", " + (comments == null ? "null" : "'" + comments.Replace("'", "''").Trim() + "'") + ", " + (PhoToAngle == null ? "null" : "'" + PhoToAngle.Replace("'", "''").Trim() + "'") + ", " + (UserId == null ? "null" : UserId.ToString()) + ", " + (CycloneAction == null ? "null" : "'" + CycloneAction.Replace("'", "''").Trim() + "'") + ", " + (PriorityRiskRating == null ? "null" : "'" + PriorityRiskRating.Replace("'", "''").Trim() + "'") + " ," + (TotalLabourDismantleEdit == null ? "null" : TotalLabourDismantleEdit.ToString()) + ", " + (UseEnteredValue == null ? "null" : UseEnteredValue.ToString()) + ", " + (WithInRangeOfOutBoard == null ? "null" : WithInRangeOfOutBoard.ToString()) + ", " + (ImageFileName == null ? "null" : "'" + ImageFileName.Replace("'", "''").Trim() + "'") + ", " + (ImageFileName == null ? "null" : "'" + ImageFileName.Replace("'", "''").Trim() + "'") + ")";

            string trackUploadQuery = "INSERT INTO TblTrackUploadToCloud(ProcName,Registerid, userid, createdBy, CreatedOn, Query, Response) values('Pre-TblTempOfflineRegister_UploadedToCloud'," + (registerId == null ? "null" : "'" + registerId.Replace("'", "''").Trim() + "'") + "," + (UserId == null ? "null" : "'" + UserId.ToString() + "'") + ",'" + UserId + "',GETDATE()," + (query == null ? "null" : "'" + query.Replace("'", "''").Trim() + "'") + ",'Pre-Insert in TempTable after making query STEP 2')";


            try
            {
                var trackUploadPreInsert = databaseCon.RunCmd(trackUploadQuery);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            try
            {

                // Run the main query
                registerdt = databaseCon.RunCmd(query);
                string trackUpload = "";
                if (registerdt == 1)
                {
                    trackUpload = "INSERT INTO TblTrackUploadToCloud(ProcName,Registerid, userid, createdBy, CreatedOn, Query, Response) values('TblTempOfflineRegister_UploadedToCloud'," + (registerId == null ? "null" : "'" + registerId.Replace("'", "''").Trim() + "'") + "," + (UserId == null ? "null" : "'" + UserId.ToString() + "'") + ",'" + UserId + "',GETDATE()," + (query == null ? "null" : "'" + query.Replace("'", "''").Trim() + "'") + ",'Success insert in TempTable STEP 3')";
                }
                else
                {
                    trackUpload = "INSERT INTO TblTrackUploadToCloud(ProcName,Registerid, userid, createdBy, CreatedOn, Query, Response) values('TblTempOfflineRegister_UploadedToCloud'," + (registerId == null ? "null" : "'" + registerId.Replace("'", "''").Trim() + "'") + "," + (UserId == null ? "null" : "'" + UserId.ToString() + "'") + ",'" + UserId + "',GETDATE()," + (query == null ? "null" : "'" + query.Replace("'", "''").Trim() + "'") + ",'Failed insert in TempTable STEP 4')";
                }

                var trackUploadInsert = databaseCon.RunCmd(trackUpload);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);

                string trackUploadError = "INSERT INTO TblTrackUploadToCloud(ProcName,Registerid, userid, createdBy, CreatedOn, Query, Response) values('TblTempOfflineRegister_UploadedToCloud'," + (registerId == null ? "null" : "'" + registerId.Replace("'", "''").Trim() + "'") + "," + (UserId == null ? "null" : "'" + UserId.ToString() + "'") + ",'" + UserId + "',GETDATE()," + (query == null ? "null" : "'" + query.Replace("'", "''").Trim() + "'") + ",'" + ex.Message.Replace("'", "''") + "')";


                var trackUploadInsertError = databaseCon.RunCmd(trackUploadError);
            }

            //RegisterController.UpdateImagePath(string id, string RID, string txtOwnername, string radUpdogImg, string imgname, string tempimg, string basetempimg)
            //UpdateImagePath(clientId, registerId, TxtOwanerName, radUpdogImg, "test image.png", tempimg, basetempimg);


        }

        private string SavejobImageToBlob(string base64Image, string TxtOwanerName, string RegisterID, string path)
        {
            if (string.IsNullOrEmpty(base64Image)) return null;

            //string extension = ".jpg";
            //string FileName = $"_{RegisterID}_{DateTime.Now.Ticks}{extension}";
            string FileName = $"{TxtOwanerName}_{RegisterID}_{DateTime.Now.Ticks}.jpg";
            string fullPath = Path.Combine(path, FileName);

            byte[] imageBytes = Convert.FromBase64String(base64Image.Split(',')[1]);
            System.IO.File.WriteAllBytes(fullPath, imageBytes);

            using (Image img = Image.FromFile(fullPath))
            {
                if (img.PropertyIdList.Contains(0x0112))
                    RotateImageIfNeeded(img);

                string NewFileName = $"{TxtOwanerName}_{RegisterID}_{DateTime.Now.Ticks}.jpg";
                SaveResizedImage(img, Path.Combine(path, NewFileName), 0.5);
                databaseCon.savefileonblob(NewFileName, "Site-images", Path.Combine(path, NewFileName), "image/jpeg");
                //System.IO.File.Delete(Path.Combine(path, NewFileName));
                return NewFileName;
            }
        }

        private void ProcessRegisterGalleryData(Dictionary<string, object> record, string UserId, string RID)
        {
            try
            {
                // Extract the values from the dictionary, with null checks
                string Description = record.ContainsKey("Description") ? record["Description"]?.ToString() : null;
                string FileName = record.ContainsKey("FileName") ? record["FileName"]?.ToString() : null;
                string ID = record.ContainsKey("ID") ? record["ID"]?.ToString() : null;
                string Images = record.ContainsKey("Images") ? record["Images"]?.ToString() : null;
                string RegisterId = record.ContainsKey("RegisterId") ? record["RegisterId"]?.ToString() : null;
                string Type = record.ContainsKey("Type") ? record["Type"]?.ToString() : null;
                string UID = UserId;
                //string RID = RegisterId;
                string txtdesc = Description;
                string FileUpload1 = FileName;
                string OriginalDocumentName = FileName;
                var result = btnSaveimagegellery_Click(UID, txtdesc, RID, FileUpload1, OriginalDocumentName, Images);
                //if (result is JsonResult jsonResult)
                //{
                //    var data = jsonResult.Data;
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        public ActionResult btnSaveimagegellery_Click(string UID, string txtdesc, string RID, string FileUpload1, string OriginalDocumentName, string Images)
        {
            try
            {
                string extension = ".jpg";
                string FileName = "";
                string path = Server.MapPath("/images/sites/");
                string newpath2 = "";
                //string QueryU = "insert tblRegisterGallery SET";
                string NewFileName = "";

                if (!string.IsNullOrEmpty(Images))
                {
                    //FileName = $"{txtdesc.Replace("'", "''").Replace(" ", "").Trim()}_{RID}_{DateTime.Now.Ticks}{extension}";
                    FileName = $"_{RID}_{DateTime.Now.Ticks}{extension}";
                    newpath2 = Path.Combine(path, FileName);
                    byte[] imageBytes = Convert.FromBase64String(Images.Split(',')[1]);
                    System.IO.File.WriteAllBytes(newpath2, imageBytes);

                    if (!string.IsNullOrEmpty(FileUpload1))
                    {
                        string imgfile = Path.Combine(path, FileUpload1);
                        if (System.IO.File.Exists(imgfile))
                            System.IO.File.Delete(imgfile);
                    }
                    using (Image img = Image.FromFile(newpath2))
                    {
                        //NewFileName = $"{txtdesc.Replace("'", "''").Replace(" ", "").Trim()}_{RID}_{DateTime.Now.Ticks}{extension}";
                        NewFileName = $"_{RID}_{DateTime.Now.Ticks}{extension}";

                        if (img.PropertyIdList.Contains(0x0112))
                            RotateImageIfNeeded(img);

                        SaveResizedImage(img, Path.Combine(path, NewFileName), 0.5);
                    }
                    string fpath = Path.Combine(path, NewFileName);
                    databaseCon.savefileonblob(NewFileName, "Site-images", fpath, "image/jpeg");
                    System.IO.File.Delete(fpath);

                    // Build the SQL query with the new file name and other data
                    string query = "exec Pro_TBLREGISTERGALLERY_SAVE '0','" + RID + "','" + txtdesc.Replace("'", "''").Trim() + "','" + NewFileName + "','" + UID + "','" + DateTime.Now + "','ImageGallery','" + OriginalDocumentName + "'";
                    string trackUploadQuery = "INSERT INTO TblTrackUploadToCloud(ProcName,Registerid, userid, createdBy, CreatedOn, Query, Response) values('Pre-Pro_TBLREGISTERGALLERY_SAVE'," + (RID == null ? "null" : "'" + RID.Replace("'", "''").Trim() + "'") + "," + (UID == null ? "null" : "'" + UID.ToString() + "'") + ",'" + UID + "',GETDATE()," + (query == null ? "null" : "'" + query.Replace("'", "''").Trim() + "'") + ",'Pre-Insert in TempTable after making query STEP 1')";


                    try
                    {
                        var trackUploadPreInsert = databaseCon.RunCmd(trackUploadQuery);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("An error occurred: " + ex.Message);
                    }
                    // Execute the query
                    int result = databaseCon.RunCmd(query);
                    string trackUpload = "";

                    if (result > 0)
                    {
                        trackUpload = "INSERT INTO TblTrackUploadToCloud(ProcName,Registerid, userid, createdBy, CreatedOn, Query, Response) values('Pro_TBLREGISTERGALLERY_SAVE'," + (RID == null ? "null" : "'" + RID.Replace("'", "''").Trim() + "'") + "," + (UID == null ? "null" : "'" + UID.ToString() + "'") + ",'" + UID + "',GETDATE()," + (query == null ? "null" : "'" + query.Replace("'", "''").Trim() + "'") + ",'Success exec proc STEP 2')";
                        int trackUploadImage = databaseCon.RunCmd(trackUpload);
                        return Json(new { success = true, message = "Data saved successfully." });
                    }
                    else
                    {
                        trackUpload = "INSERT INTO TblTrackUploadToCloud(ProcName,Registerid, userid, createdBy, CreatedOn, Query, Response) values('Pro_TBLREGISTERGALLERY_SAVE'," + (RID == null ? "null" : "'" + RID.Replace("'", "''").Trim() + "'") + "," + (UID == null ? "null" : "'" + UID.ToString() + "'") + ",'" + UID + "',GETDATE()," + (query == null ? "null" : "'" + query.Replace("'", "''").Trim() + "'") + ",'Failed exec proc STEP 3')";
                        int trackUploadImage = databaseCon.RunCmd(trackUpload);
                        return Json(new { success = false, message = "Failed to save data." });
                    }
                }

                return Json(new { success = false });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }

        private void ProcessMaterialType(Dictionary<string, object> record)
        {
            string materialType = record.ContainsKey("MaterialType") && record["MaterialType"] != null
                ? record["MaterialType"].ToString().Replace("'", "''").Trim()
                : null;

            string emailId = record.ContainsKey("EmailID") && record["EmailID"] != null
                ? record["EmailID"].ToString().Trim()
                : "Unknown";

            string clientId = record.ContainsKey("ClientID") && record["ClientID"] != null
                ? record["ClientID"].ToString().Trim()
                : null;

            string query = "SELECT DISTINCT(MaterialType) FROM tblMaterialType " +
                           "WHERE ClientId = '" + clientId + "' " +
                           "AND MaterialType = " + (materialType == null ? "NULL" : "'" + materialType + "'") + "";

            DataTable dtMaterial = databaseCon.GetData(query);

            if (dtMaterial.Rows.Count == 0)
            {
                string insertQuery = "INSERT INTO tblMaterialType (MaterialType, ClientID, CreatedOn, CreatedBy, MaterialStatus) " +
                                     "VALUES (" +
                                     (materialType == null ? "NULL" : "'" + materialType + "'") + ", " +
                                     "'" + clientId + "', " +
                                     "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                     "'" + emailId + "', " +
                                     "'Active')";

                int count = databaseCon.RunCmd(insertQuery);
            }
        }

        private void ProcessDutyRating(Dictionary<string, object> record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record), "Record dictionary cannot be null.");
            }

            string clientId = record.ContainsKey("ClientID") ? record["ClientID"]?.ToString() : null;
            string dutyRating = record.ContainsKey("DutyRating") ? record["DutyRating"]?.ToString() : null;

            string createdBy = record.ContainsKey("EmailID") && record["EmailID"] != null
                ? record["EmailID"].ToString().Trim()
                : "Unknown";
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentException("ClientID is a required field.");
            }

            string query = "SELECT DISTINCT DutyRating FROM tblDutyRating WHERE ClientId = '" + clientId + "' AND DutyRating = " + (dutyRating == null ? "null" : "'" + dutyRating.Replace("'", "''").Trim() + "'");
            DataTable dtRating = databaseCon.GetData(query);

            if (dtRating.Rows.Count == 0)
            {
                string ratingQuery = "INSERT INTO tblDutyRating (DutyRating, ClientID, CreatedOn, CreatedBy, DutyRatingStatus) VALUES (" +
                                     (dutyRating == null ? "null" : "'" + dutyRating.Replace("'", "''").Trim() + "'") + ", " +
                                     "'" + clientId + "', " +
                                     "'" + DateTime.Now + "', " +
                                     "'" + createdBy.ToString().Replace("'", "''") + "', 'Active')";

                int count = databaseCon.RunCmd(ratingQuery);
                if (count > 0)
                {
                    Console.WriteLine("Duty rating inserted successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to insert duty rating.");
                }
            }
            else
            {
                Console.WriteLine("Duty rating already exists.");
            }
        }

        private void ProcessAssetList(Dictionary<string, object> record)
        {
            string buildingId = record.ContainsKey("BuildingId") && record["BuildingId"] != null
                ? record["BuildingId"].ToString().Trim()
                : null;
            string ClientId = record.ContainsKey("ClientID") && record["ClientID"] != null
                ? record["ClientID"].ToString().Trim()
                : null;

            string assetNo = record.ContainsKey("AssetNo") && record["AssetNo"] != null
                ? record["AssetNo"].ToString().Replace("'", "''").Trim()
                : null;

            string assetDesc = record.ContainsKey("AssetDesc") && record["AssetDesc"] != null
                ? record["AssetDesc"].ToString().Replace("'", "''").Trim()
                : null;

            string createdBy = record.ContainsKey("EmailID") && record["EmailID"] != null
                ? record["EmailID"].ToString().Trim()
                : "Unknown";

            string Assetquery = "select AssetID from tblAsset where ClientId = '" + ClientId + "' and BuildingId = '" + (buildingId == null ? "null" : buildingId) + "' and AssetNo = " + (assetNo == null ? "null" : "'" + assetNo + "'") + "";

            DataTable dtasset = databaseCon.GetData(Assetquery);

            if (dtasset.Rows.Count == 0)
            {
                string AssQuery = "Insert into tblAsset(AssetNo, AssetDesc, ClientID, BuildingId, CreatedOn, CreatedBy, AssetStatus) Values(" +
                                  (assetNo == null ? "null" : "'" + assetNo + "'") + "," +
                                  (assetDesc == null ? "null" : "'" + assetDesc + "'") + "," +
                                  "'" + ClientId + "'," +
                                  (buildingId == null ? "null" : "'" + buildingId + "'") + "," +
                                  "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                  "'" + createdBy + "'," +
                                  "'Active')";

                int AssetID = databaseCon.RunCmdID(AssQuery);

            }
        }
        //   public ActionResult UpdateImagePath(
        //string id,
        //string RID,
        //string txtOwnername,
        //string radUpdogImg,
        //string imgname,
        //string tempimg,
        //string basetempimg)
        //   {
        //       try
        //       {
        //           int RegisterID = Convert.ToInt32(RID);
        //           if (RegisterID > 0)
        //           {
        //               string extension = ".jpg"; // Default extension for base64 images
        //               string FileName = "";
        //               string path = Server.MapPath("/images/sites/");
        //               string newpath2 = "";
        //               string QueryU = "UPDATE tblRegister SET";
        //               string NewFileName = "";

        //               // Process base64 image if provided
        //               if (!string.IsNullOrEmpty(basetempimg))
        //               {
        //                   // Generate the filename
        //                   FileName = $"{txtOwnername.Replace("'", "''").Replace(" ", "").Trim()}_{RegisterID}_{DateTime.Now.Ticks}{extension}";
        //                   newpath2 = Path.Combine(path, FileName);

        //                   // Convert base64 to byte array and save
        //                   byte[] imageBytes = Convert.FromBase64String(basetempimg.Split(',')[1]);
        //                   System.IO.File.WriteAllBytes(newpath2, imageBytes);

        //                   // Optional: Delete existing image if `imgname` is provided
        //                   if (!string.IsNullOrEmpty(imgname))
        //                   {
        //                       string imgfile = Path.Combine(path, imgname);
        //                       if (System.IO.File.Exists(imgfile))
        //                           System.IO.File.Delete(imgfile);
        //                   }

        //                   // Handle rotation and resizing if required
        //                   using (Image img = Image.FromFile(newpath2))
        //                   {
        //                       NewFileName = $"{txtOwnername.Replace("'", "''").Replace(" ", "").Trim()}_{RegisterID}_{DateTime.Now.Ticks}{extension}";

        //                       if (img.PropertyIdList.Contains(0x0112)) // Check for EXIF orientation
        //                           RotateImageIfNeeded(img);

        //                       SaveResizedImage(img, Path.Combine(path, NewFileName), 0.5);
        //                   }

        //                   // Update query with the new image and base image
        //                   QueryU += $" Image='{NewFileName}', OriginalBaseImage='{basetempimg}' WHERE RegisterID={RegisterID}";
        //                   databaseCon.RunCmd(QueryU);
        //               }
        //           }

        //           return Json(new { Result = "Success", Message = "Image saved successfully." }, JsonRequestBehavior.AllowGet);
        //       }
        //       catch (Exception ex)
        //       {
        //           return Json(new { Result = "Error", Message = ex.Message }, JsonRequestBehavior.AllowGet);
        //       }
        //   }




        private bool IsValidImageExtension(string extension)
        {
            string[] validExtensions = { ".jpg", ".jpeg", ".png", ".ico", ".gif", ".bmp" };
            return validExtensions.Contains(extension.ToLower());
        }

        private void RotateImageIfNeeded(Image img)
        {
            PropertyItem propOrientation = img.GetPropertyItem(0x0112);
            short orientation = BitConverter.ToInt16(propOrientation.Value, 0);

            switch (orientation)
            {
                case 6:
                    img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
                case 8:
                    img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
                case 3:
                    img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;
            }
        }

        //private void SaveResizedImage(Image img, string filePath, double scale)
        //{
        //    int newWidth = (int)(img.Width * scale);
        //    int newHeight = (int)(img.Height * scale);

        //    using (Bitmap thumbnailImg = new Bitmap(newWidth, newHeight))
        //    {
        //        using (Graphics thumbGraph = Graphics.FromImage(thumbnailImg))
        //        {
        //            thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
        //            thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
        //            thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

        //            thumbGraph.DrawImage(img, new Rectangle(0, 0, newWidth, newHeight));
        //        }

        //        thumbnailImg.Save(filePath, img.RawFormat);
        //    }
        //}
        private void SaveResizedImage(Image img, string filePath, double scale)
        {
            int newWidth = (int)(img.Width * scale);
            int newHeight = (int)(img.Height * scale);

            using (Bitmap thumbnailImg = new Bitmap(newWidth, newHeight))
            {
                using (Graphics thumbGraph = Graphics.FromImage(thumbnailImg))
                {
                    thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                    thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                    thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

                    thumbGraph.DrawImage(img, new Rectangle(0, 0, newWidth, newHeight));
                }

                try
                {
                    thumbnailImg.Save(filePath, img.RawFormat);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving image: {ex.Message}");
                }
                finally
                {
                    img.Dispose();
                }
            }
        }


        //private void SaveBase64Image(string filePath, byte[] imageBytes)
        //{
        //    try
        //    {
        //        string directoryPath = Path.GetDirectoryName(filePath);
        //        if (!Directory.Exists(directoryPath))
        //            Directory.CreateDirectory(directoryPath);

        //        using (FileStream imageFile = new FileStream(filePath, FileMode.Create))
        //        {
        //            imageFile.Write(imageBytes, 0, imageBytes.Length);
        //            imageFile.Flush();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log or handle the exception as needed
        //        Console.WriteLine($"Error saving Base64 image: {ex.Message}");
        //    }
        //}


        //private static byte[] GetCompressedBitmap(Bitmap bmp, long quality)
        //{
        //    byte[] image;
        //    using (MemoryStream output = new MemoryStream())
        //    {
        //        EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
        //        ImageCodecInfo imageCodec = ImageCodecInfo.GetImageEncoders().FirstOrDefault(o => o.FormatID == ImageFormat.Jpeg.Guid);
        //        EncoderParameters parameters = new EncoderParameters(1);
        //        parameters.Param[0] = qualityParam;
        //        bmp.Save(output, imageCodec, parameters);
        //        image = output.ToArray();
        //    }
        //    return image;
        //}
        //private static byte[] GetCompressedBitmap(Bitmap bmp, long quality)
        //{
        //    byte[] image;
        //    try
        //    {
        //        using (MemoryStream output = new MemoryStream())
        //        {
        //            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
        //            ImageCodecInfo imageCodec = ImageCodecInfo.GetImageEncoders().FirstOrDefault(o => o.FormatID == ImageFormat.Jpeg.Guid);
        //            if (imageCodec == null)
        //                throw new InvalidOperationException("JPEG codec not found.");

        //            EncoderParameters parameters = new EncoderParameters(1);
        //            parameters.Param[0] = qualityParam;
        //            bmp.Save(output, imageCodec, parameters);
        //            image = output.ToArray();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log or handle the exception as needed
        //        Console.WriteLine($"Error compressing bitmap: {ex.Message}");
        //        throw;
        //    }
        //    return image;
        //}

        public ActionResult UpdateOfflineInspectionFlag(string LoginUserId, string clientId, string Siteid, string UserRole, string RID)
        {
            string Userinfor = "select LoginId from tblUser where UserId = " + LoginUserId + "";
            DataTable LoginuserInfo = databaseCon.GetData(Userinfor);

            string CheckIsDatasillInOfflineMode = "select  RegisterId from tblRegister where ClientID =  " + clientId + " and SiteLocId = " + Siteid + " and IsOfflineCurrent = 1  and  GoOfflineJobBy = '" + LoginuserInfo.Rows[0][0].ToString() + "'";
            DataTable CheckIsDatasillInOfflineModeData = databaseCon.GetData(CheckIsDatasillInOfflineMode);

            if (CheckIsDatasillInOfflineModeData.Rows.Count > 0)
            {

                try
                {
                    string updateRegisterquery = "update tblRegister set IsOfflineCurrent = 0  where  IsOfflineCurrent = 1  and ClientID =  " + clientId + " and SiteLocId = " + Siteid + " and GoOfflineJobBy = '" + LoginuserInfo.Rows[0][0].ToString() + "'";

                    var res = databaseCon.RunCmd(updateRegisterquery);

                    return Json(new { Result = "Success", Message = "Data uploaded successfully.", UID = databaseCon.Encrypt(LoginUserId), UserRole = databaseCon.Encrypt(UserRole), id = databaseCon.Encrypt(clientId), RID = databaseCon.Encrypt(RID) });

                }
                catch (Exception ex)
                {
                    return Json(new { Result = "Failed", Message = ex.Message });
                }
            }
            else
            {

                return Json(new { Result = "AlreadyMoved", Message = "No any data in offline" });

            }
        }


        //shubham added 

        List<TblClient> ddlClientList = new List<TblClient> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod]
        public List<TblClient> GetClientDetails(string id)
        {
            //string query = "select distinct SubGroup from  tblRegister where ClientId = '" + ClientID + "' and SubGroup is not null and SubGroup!= '' Order by SubGroup";
            string query = "Select isnull(InspectionCheckListColor,0) as InspectionCheckListColor,ClientId,isnull(ManHrsRatio,0) as ManHrsRatio from tblClient where ClientId = '" + id + "'";

            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    TblClient pro = new TblClient();




                    if (!(item["ClientId"] is DBNull))
                    {
                        pro.ClientId = item["ClientId"] as int?;
                    }

                    if (!(item["InspectionCheckListColor"] is DBNull))
                    {
                        pro.InspectionCheckListColor = item["InspectionCheckListColor"] as bool?;
                    }

                    if (!(item["ManHrsRatio"] is DBNull))
                    {
                        pro.ManHrsRatio = item["ManHrsRatio"] as decimal?;
                    }

                    ddlClientList.Add(pro);
                }
            }
            return ddlClientList;
        }



        [HttpPost]
        public ActionResult checkOfflineJobClearedDate(string id, string UID, string SiteId)
        {
            //string FinddataQuery = "SELECT RegisterId, isnull(max(OfflineJobClearedOn),'1900-01-01') as OfflineJobClearedOn FROM tblRegister WHERE isofflineCurrent = 1 AND ClientId = '" + databaseCon.Decrypt(id) + "' group by RegisterId";
            string FinddataQuery = "SELECT tblRegister.RegisterId, isnull(max(tblRegister.OfflineJobClearedOn),'1900-01-01') as OfflineJobClearedOn FROM tblRegister join tblUser on tblRegister.GoOfflineJobBy = tblUser.LoginId WHERE tblRegister.isofflineCurrent = 1 and tblRegister.ClientId = '" + databaseCon.Decrypt(id) + "' and tblUser.UserId = '" + databaseCon.Decrypt(UID) + "' and tblRegister.SiteLocId = '" + databaseCon.Decrypt(SiteId) + "' group by tblRegister.RegisterId";
            var FinddataUpdatedBy = databaseCon.GetData(FinddataQuery);

            var RegisterId = FinddataUpdatedBy.Rows[0]["RegisterId"].ToString();
            var OfflineJobClearedOn = FinddataUpdatedBy.Rows[0]["OfflineJobClearedOn"].ToString();

            DateTime offlineJobOnDateTime = DateTime.Parse(OfflineJobClearedOn);
            string formattedOfflineJobOn = offlineJobOnDateTime.ToString("dd-MMM-yyyy h:mm:ss tt");
            return Json(new { RegisterId = RegisterId, formattedOfflineJobOn = formattedOfflineJobOn });
        }


        [HttpPost]
        public JsonResult SaveIndexedDBData(IndexedDBDataModel data, string UserId, string NonZeroRegisterCount, string ZeroRegisterCount, string RID, string upload)
        {
            try
            {
                if (upload == "0")
                {
                    foreach (var table in data.Tables)
                    {
                        string QuerStep1 = "INSERT INTO TblTrackUploadToCloud(ProcName, Registerid,userid,CreatedOn, Response) values('" + table.TableName + "','" + RID + "','" + UserId + "',GETDATE(),'" + table.EntryCount + "')";
                        databaseCon.RunCmd(QuerStep1);


                    }
                    string NonZeroRegisterCountquery = "INSERT INTO TblTrackUploadToCloud(ProcName,Registerid,userid,CreatedOn, Response) values('NonZeroRegistertblPipelineRequest','" + RID + "','" + UserId + "',GETDATE(),'" + NonZeroRegisterCount + "')";
                    databaseCon.RunCmd(NonZeroRegisterCountquery);
                    string ZeroRegisterCountQuery = "INSERT INTO TblTrackUploadToCloud(ProcName,Registerid,userid,CreatedOn, Response) values('ZeroRegistertblPipelineRequest','" + RID + "','" + UserId + "',GETDATE(),'" + ZeroRegisterCount + "')";
                    databaseCon.RunCmd(ZeroRegisterCountQuery);
                }
                if (upload == "1")
                {
                    foreach (var table in data.Tables)
                    {
                        string QuerStep1 = "INSERT INTO TblTrackUploadToCloud(ProcName,Status, Registerid,userid,CreatedOn, Response) values('" + table.TableName + "','Uploaded Table','" + RID + "','" + UserId + "',GETDATE(),'" + table.EntryCount + "')";
                        databaseCon.RunCmd(QuerStep1);


                    }
                    string NonZeroRegisterCountquery = "INSERT INTO TblTrackUploadToCloud(ProcName,Status,Registerid,userid,CreatedOn, Response) values('NonZeroRegistertblPipelineRequest','Uploaded Table','" + RID + "','" + UserId + "',GETDATE(),'" + NonZeroRegisterCount + "')";
                    databaseCon.RunCmd(NonZeroRegisterCountquery);
                    string ZeroRegisterCountQuery = "INSERT INTO TblTrackUploadToCloud(ProcName,Status,Registerid,userid,CreatedOn, Response) values('ZeroRegistertblPipelineRequest','Uploaded Table','" + RID + "','" + UserId + "',GETDATE(),'" + ZeroRegisterCount + "')";
                    databaseCon.RunCmd(ZeroRegisterCountQuery);
                }

                return Json(new { success = true, message = "Data inserted successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        // Model for IndexedDB Data
        public class IndexedDBDataModel
        {
            public List<TablesData> Tables { get; set; }
        }

        public class TablesData
        {
            public string TableName { get; set; }
            public int EntryCount { get; set; }
        }


    }
}





