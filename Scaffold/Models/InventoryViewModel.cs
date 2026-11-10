using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scaffold.Models
{
    public class InventoryViewModel
    {
        public Nullable<int> ClientID { get; set; }
        public int InventoryID { get; set; }
        public string MaterialType { get; set; }
        public string ItemCode { get; set; }
        public string ItemDesc { get; set; }
        public Nullable<decimal> ItemWeight { get; set; }
        public int OrgYardQuantity { get; set; }
        public int QuantityToBeSent { get; set; }
        public decimal QuantityReturned { get; set; }
        public decimal SiteQuantityDamaged { get; set; }
        public decimal JobQuantityDamaged { get; set; }
        public decimal TotalDamagedQuentity { get; set; }
        public Nullable<int> AvailableQuantity { get; set; }
        public decimal OrgYardAvailableWithoutDamage { get; set; }
        public Nullable<decimal> TotalWeight { get; set; }
        public string Comments { get; set; }
        public Nullable<decimal> LabourErect { get; set; }
        public Nullable<decimal> LabourDismantle { get; set; }
        public Nullable<int> SubCategoryId { get; set; }
        public string SubCategory { get; set; }
        public int? DisplaySortOrder { get; set; }
    }
}