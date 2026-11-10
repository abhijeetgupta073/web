using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scaffold.Models
{
    public class SiteLocationViewModel
    {
        public int SiteLocID { get; set; }
        public string SiteLocation { get; set; }
        public Nullable<int> ClientId { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string SiteLocationStatus { get; set; }
        public string OwnerName { get; set; }
        public string DistributionList { get; set; }
        public string Country { get; set; }
        public string SiteLat { get; set; }
        public string SiteLong { get; set; }
        public Nullable<decimal> ManHoursAvailable { get; set; }
        public Nullable<bool> MaintainInventory { get; set; }
        public Nullable<bool> CycloneManagement { get; set; }
        public Nullable<bool> ManHoursManagement { get; set; }
        public Nullable<bool> MaintainInventoryAtJob { get; set; }
        public Nullable<bool> GPS { get; set; }
        public Nullable<bool> FloorLevel { get; set; }
        public Nullable<bool> EnabledGPSSitePin { get; set; }
        public Nullable<bool> OffSystemInspection { get; set; }
    }
}