using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scaffold.Models
{
    public class DeliveryInventoryViewModel
    {
        public Nullable<int> TransDeliId { get; set; }
        public Nullable<int> FromId { get; set; }
        public Nullable<int> ToId { get; set; }
        public Nullable<int> TransId { get; set; }
        public Nullable<int> TransReqId { get; set; }
        public string MaterialType { get; set; }
        public Nullable<int> InventoryId { get; set; }
        public string ItemCode { get; set; }
        public string ItemDesc { get; set; }
        public Nullable<int> AvailableQuantity { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<int> AllocatedQuantity { get; set; }
    }
}