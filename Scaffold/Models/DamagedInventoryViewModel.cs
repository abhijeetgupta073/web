using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scaffold.Models
{
    public class DamagedInventoryViewModel
    {

      public Nullable <int> FromId { get; set; }
      public Nullable <int> ToId { get; set; }
      public Nullable<int> DamagedInvId { get; set; }
      public string FromName { get; set; }
      public string ToName { get; set; } 
      public string Name { get; set; } 
      public string CreatedBy { get; set; }
      public string CreatedOn { get; set; }

        public Nullable<int> DIDetailID { get; set; }
        public string MaterialType { get; set; }
        public string ItemCode { get; set; }
        public string ItemDesc { get; set; }
        public string Reason { get; set; }
        public Nullable<Decimal> DamagedQuantity { get; set; }



    }
}