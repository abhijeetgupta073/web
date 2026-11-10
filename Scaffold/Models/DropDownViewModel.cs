using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scaffold.Models
{
    public class DropDownViewModel
    {
        public int SiteLocID { get; set; }
        public int InventoryId { get; set; }
        public string SiteLocation { get; set; }
        public int areaid { get; set; }
        public string area { get; set; }
        public int displayorder { get; set; } // add on 28/Oct/2020
        public int UserId { get; set; }
        public string name { get; set; }
        public string DesignType { get; set; }
        public string DutyRating { get; set; }
        public string MaterialType { get; set; }
        public string Supervisor { get; set; }
        public Nullable<int> FloorLevelId { get; set; }
        public string FloorLevel { get; set; }
        public Nullable<int> checklistID { get; set; }
        public string checklistdescription { get; set; }
        public string ScaffoldOwner { get; set; }
        public string discipline { get; set; }
        public Nullable<int> buildingid { get; set; }
        public string buildingno { get; set; }
        public Nullable<int> Assetid { get; set; }
        public string Assetno { get; set; }
        public string Position { get; set; }
        public Nullable<int> CRId { get; set; }
        public Nullable<int> RegisterId { get; set; }
        public string ScaffRef { get; set; }
        public string ItemCode { get; set; }
        public string ItemDesc { get; set; }
        public string ItemCodeItemDesc { get; set; }
        public string Supplier { get; set; }
        public string Contractor { get; set; }
        public string ContractorRep { get; set; }
        public string RoleName { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public string Category { get; set; }
        public string Country { get; set; }
        public string state { get; set; }
        public int WorkGroupID { get; set; }
        public string WorkGroup { get; set; }
        public string SubGroup { get; set; }
        //public int SubGroupID { get; set; }
        public string DrawingName { get; set; }
        public string OwnerName { get; set; }

        public string Bearing { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string DropdownSitelocation { get; set; }
        public int SubGroupId { get; set; }
        public string RequestBy { get; set; }
        public int RequestById { get; set; }
        public string DropdownBuilding { get; set; }
    }
}