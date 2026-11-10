using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scaffold.Models
{
    public class ScaffoldMaterialListPhase2ViewModel
    {
        public  Nullable<int> FromId { get; set; }
        public Nullable<int> ToId { get; set; }
        public string MaterialType { get;set;}
        public string ItemCode { get; set; }
        public string ItemDesc { get; set; }
        public Nullable<int> InventoryId { get; set; }
        public Nullable<int> DeliveryAvailableQuantity { get; set; }
        public Nullable<int> ReturnALLQuantity { get; set; }
        public Nullable<int> DamagedALLQuantity { get; set; }
        public Nullable<int> AvailableQuantity { get; set; }
        public Nullable<double> ItemWeight { get; set; }
        public Nullable<double> TotalWeight { get; set; }
        public Nullable<int> SubCategoryID { get; set; }
        public string HireMeasurement { get; set; }
        public Nullable<decimal> DailyHireRate { get; set; }
        public Nullable<decimal> TotalDailyHire { get; set; }
        public Nullable<int> Quantity { get; set; }
    }
}