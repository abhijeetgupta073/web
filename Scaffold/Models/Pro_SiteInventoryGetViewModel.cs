using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scaffold.Models
{
    public class Pro_SiteInventoryGetViewModel
    {
        public Nullable<int> SiteId { get; set; }
        public string MaterialType { get; set; }
        public string ItemCode { get; set; }
        public string ItemDesc { get; set; }
        public Nullable<int> OrgSiteQuantity { get; set; }
        public Nullable<decimal> QuantityToBeSent { get; set; }
        public Nullable<int> AvailableQuantity { get; set; }
        public Nullable<double> ItemWeight { get; set; }
        public Nullable<decimal> TotalWeight { get; set; }
        public Nullable<int> QuantityReturned { get; set; }
        public Nullable<decimal> QuantityReturnedToDepot { get; set; }
        public Nullable<int> flag { get; set; }
        public Nullable<decimal> SiteDamagedQuantityReturnToDepot { get; set; }
        public Nullable<decimal> JobQuantityDamaged { get; set; }
        public Nullable<int> OnSite { get; set; }
        public string HireMeasurement { get; set; }
        public Nullable<decimal> DailyHireRate { get; set; }
        public Nullable<decimal> TotalDailyHire { get; set; }
    }
}