using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scaffold.Models
{
    public class ReturnInventoryDetailsViewModel
    {
      public Nullable<int> RIDetailID { get; set; }
      public Nullable<int> FromId  { get; set; }
      public Nullable<int> ToId { get; set; }
      public string  MaterialType  { get; set; }
      public string ItemCode { get; set; }
      public string ItemDesc  { get; set; }  
      public Nullable<int> InventoryId { get; set; }
      public Nullable<int> DeliveryAvailableQuantity { get; set; }
      public Nullable<int> DamagedALLQuantity { get; set; }
      public Nullable<int> ReturnQuantity { get; set; }
      public Nullable<int> ReturnALLQuantity { get; set; }
      public Nullable<int> AvailableQuantity { get; set; }
    }
}