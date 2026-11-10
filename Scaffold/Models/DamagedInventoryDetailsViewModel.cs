using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scaffold.Models
{
    public class DamagedInventoryDetailsViewModel
    {
        public Nullable<int> DIDetailID { get; set; }
        public Nullable<int> InventoryId { get; set; }
        public string MaterialType { get; set; }
        public string ItemCode { get; set; }
        public string ItemDesc { get; set; }
        public Nullable<int> AvailableQuantity { get; set; }
        public Nullable<int> DamagedQuantity { get; set; }
        public string Reason { get; set; }

   }
}