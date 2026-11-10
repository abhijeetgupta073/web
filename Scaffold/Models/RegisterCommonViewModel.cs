using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scaffold.Models
{
    public class RegisterCommonViewModel
    {
        public Nullable<bool> GPS { get; set; }
        public Nullable<bool> FloorLevel { get; set; }
        public Nullable<bool> EnabledGPSSitePin { get; set; }
        public Nullable<bool> EnabledCaptureGps { get; set; }
        public string XPosition {get;set;}
        public string YPosition {get;set;}
        public Nullable<int> Sitelocid { get; set; }
        public Nullable<int> LocationId { get; set; }
        public Nullable<int> TotalRecord { get; set; }
        public string Address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string OwnerName { get; set; }
        public string Zip { get; set; }
        public Nullable<bool> MaintainInventory { get; set; }
        public Nullable<bool> MaintainInventoryAtJob { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public Nullable<int> BuildingID { get; set; }
        public Nullable<int> FloorLevelID { get; set; }
        public Nullable<int> AssetId { get; set; }
        public string AssetDesc { get; set; }
        public Nullable<bool> SkipMaterial { get; set; }
        


        }
}