using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scaffold.Models
{
    public class offlineregisterviewModel
    {
        //    public int RegisterId { get; set; }

        //    public string Description { get; set; }

        //    public int? ClientID { get; set; }

        //    public string InspectedBy { get; set; }

        //    public string DesignType { get; set; }

        //    public string DutyRating { get; set; }

        //    public string MaterialType { get; set; }

        //    public DateTime? DateTagged { get; set; }

        //    public DateTime? DateOfDismantle { get; set; }

        //    public DateTime? ExpectedStartDate { get; set; }

        //    public string Image { get; set; }

        //    public string CreatedBy { get; set; }

        //    public DateTime? CreatedOn { get; set; }

        //    public string UpdatedBy { get; set; }

        //    public DateTime? UpdatedOn { get; set; }

        //    public string ClientRef { get; set; }

        //    public string NoOfItems { get; set; }

        //    public decimal? Tonnage { get; set; }

        //    public string InspTimeFrame { get; set; }

        //    public DateTime? RecentInspDate { get; set; }

        //    public int? SiteLocId { get; set; }

        //    public int? ScaffRef { get; set; }

        //    public string PhotoAngle { get; set; }

        //    public int? BuildingID { get; set; }

        //    public int? AssetId { get; set; }

        //    public string AssetDesc { get; set; }



        //    public string NoOfWorkingDecks { get; set; }

        //    public string FloorLevelOld { get; set; }

        //    public int? LocationId { get; set; }

        //    public bool? OnlineApplied { get; set; }

        //    public int? CheckListId { get; set; }


        //    public string PlanningStatus { get; set; }


        //    public decimal? ManHoursAllowed { get; set; }


        //    public string WONumber { get; set; }


        //    public string ClientScaffref { get; set; }


        //    public string ScafType { get; set; }

        //    public string Lat { get; set; }

        //    public string Long { get; set; }

        //    public bool? LockedIn { get; set; }


        //    public string OptimizeStatus { get; set; }

        //    public int? PlannedStandingDuration { get; set; }

        //    public int? CrewSize { get; set; }


        //    public decimal? ManHrsErect { get; set; }


        //    public decimal? ManHrsDismantle { get; set; }


        //    public decimal? TotalLabourErect { get; set; }


        //    public decimal? TotalLabourDismantle { get; set; }


        //    public decimal? TotalErectCount { get; set; }


        //    public decimal? TotalDismantleCount { get; set; }


        //    public string CycloneAction { get; set; }

        //    public int? SupervisorId { get; set; }


        //    public string Crew { get; set; }


        //    public string PermitNo { get; set; }


        //    public string PriorityRiskRating { get; set; }


        //    public decimal? TotalLabourErectEdit { get; set; }


        //    public decimal? TotalLabourDismantleEdit { get; set; }


        //    public decimal? TotalErectCountEdit { get; set; }


        //    public decimal? TotalDismantleCountEdit { get; set; }

        //    public bool? UseEnteredValue { get; set; }

        //    public string Comments { get; set; }


        //    public string SAPNumber { get; set; }


        //    public string ScaffoldOwner { get; set; }

        //    public DateTime? DismantleApprovedDate { get; set; }

        //    public string DismantleApprovedNotes { get; set; }

        //    public string Bearing { get; set; }

        //    public string CycloneComment { get; set; }


        //    public string HierarchyLevel { get; set; }

        //    public string HierarchyComment1 { get; set; }

        //    public string HierarchyComment2 { get; set; }

        //    public string HierarchyComment3 { get; set; }

        //    public string HierarchyComment4 { get; set; }

        //    public string HierarchyComment5 { get; set; }

        //    public int? HierarchySignedOff { get; set; }

        //    public int? LockedInSignedOff { get; set; }

        //    public string QRCodeImage { get; set; }

        //    public bool? MaintainInventory { get; set; }


        //    public string DesignDrawingNo { get; set; }


        //    public string NumberOfPlatform { get; set; }


        //    public string EngineeredDesign { get; set; }

        //    public string Discipline { get; set; }

        //    public string XPosition { get; set; }

        //    public string YPosition { get; set; }


        //    public string ShiftAllocation { get; set; }

        //    public int? FloorLevelId { get; set; }


        //    public string WorkGroup { get; set; }


        //    public string SubGroup { get; set; }


        //    public string RequestedBy { get; set; }

        //    public bool? WithInRangeOfOutBoard { get; set; }

        //    public bool? LongTermStructure { get; set; }

        //    public bool? IsOfflineCurrent { get; set; }

        //    public string OriginalBaseImage { get; set; }

        //    public bool? NoInventory { get; set; }

        //    public DateTime? PipelineStartDate { get; set; }

        //    public string PipeLineNote { get; set; }

        //    public bool? ChkRequiresIsolation { get; set; }
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
        public Nullable<System.DateTime> DateTagged { get; set; }
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
        public string GoOfflineBy { get; set; }
        public string GoOfflineOn { get; set; }
        public Nullable<bool> OfflineCleared { get; set; }
        public Nullable<System.DateTime> OfflineClearedOn { get; set; }
        public string OfflineClearedBy { get; set; }



    }
}



