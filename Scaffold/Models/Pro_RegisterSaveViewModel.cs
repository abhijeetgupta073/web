using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scaffold.Models
{
    public class Pro_RegisterSaveViewModel
    {
        //public int RegisterId { get; set; }
        public string RegisterId { get; set; }
        public string ClientID { get; set; }
        public string DoneBy { get; set; }

        public string Description { get; set; }

        public string InspectedBy { get; set; }
        public string DesignType { get; set; }
        public string DutyRating { get; set; }
        public string MaterialType { get; set; }

        //public string DateTagged { get; set; }
        //public string DateOfDis { get; set; }
        public Nullable<System.DateTime> DateTagged { get; set; }
        public Nullable<System.DateTime> DateOfDis { get; set; }

        public string ExpectedStartDate { get; set; }
        //public string DoneBy { get; set; }
        public string DoneOn { get; set; }
        public string ClientRef { get; set; }

        public string InspTimeFrame { get; set; }
        public int SiteLocId { get; set; }
        public int PhotoAngle { get; set; }

        //public int BuildingID { get; set; }
        //public int AssetId { get; set; }
        public string BuildingID { get; set; }
        public string AssetId { get; set; }

        public string AssetDesc { get; set; }

        //public float Length { get; set; }
        //public float Height { get; set; }
        //public float Width { get; set; }
        public string Length { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }

        public string NoOfWorkingDecks { get; set; }
        public string FloorLevel { get; set; }
        public int LocationId { get; set; }
        public bool OnlineApplied { get; set; }

        //public int CheckListId { get; set; }
        public string CheckListId { get; set; }

        public string PlanningStatus { get; set; }

        public string Lat { get; set; }
        public string Long { get; set; }
        //public decimal Lat { get; set; }
        //public decimal Long { get; set; }

        public string LockedIn { get; set; }
        public int PlannedStandingDuration { get; set; }

        //public int CrewSize { get; set; }
        public string CrewSize { get; set; }

        //public decimal ManHrsErect { get; set; }
        //public decimal ManHrsDismantle { get; set; }
        //public int SupervisorId { get; set; }
        public string ManHrsErect { get; set; }
        public string ManHrsDismantle { get; set; }
        public string SupervisorId { get; set; }

        public string Crew { get; set; }
        public string PermitNo { get; set; }
        public string Comments { get; set; }

        public string SAPNumber { get; set; }
        public string ScaffoldOwner { get; set; }
        public string DismantleApprovedDate { get; set; }
        public string DismantleApprovedNotes { get; set; }
        public string Bearing { get; set; }

        //public int LockedInSignedOff { get; set; }
        public string LockedInSignedOff { get; set; }

        public bool MaintainInventory { get; set; }
        public string DesignDrawingNo { get; set; }
        public string NumberOfPlatform { get; set; }
        public string EngineeredDesign { get; set; }
        public string WONumber { get; set; }
        public string ScafType { get; set; }
        public string discipline { get; set; }
        public string ShiftAllocation { get; set; }
        public string txtClientScaff { get; set; }
        public string ddlWorkGroups { get; set; }
        public string ddlSubGroups { get; set; }
        public string txtRequestedBy { get; set; }
        public string checkLTStructure { get; set; }
        public string chkNoInventory { get; set; }
        public int NoOfScafref { get; set; }
        public string PipelineStartDate { get; set; }
        public string PipeLineNote { get; set; }
        public string hdnTimeOffSet { get; set; }

        public bool chkReqis { get; set; }
        public string DismantledBy { get; set; }
        public string BuiltBy { get; set; }
        public string BuildDuringShutdown { get; set; }
        public bool IsMasterJob { get; set; }

        public string RTIONotificationNumber { get; set; }
    }
}