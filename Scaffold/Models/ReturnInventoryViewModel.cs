using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scaffold.Models
{
    public class ReturnInventoryViewModel
    {
      public Nullable<int> FromId { get; set; }
      public Nullable<int> ToId  { get; set; }
      public Nullable<int> ReturnInvId { get; set; }
      public string FromName   { get; set; }
      public string ToName { get; set; }
      public string ReturnDoneBy  { get; set; }
        // public Nullable<System.DateTime> ReturnDoneOn { get; set; }
        public string ReturnDoneOn { get; set; }
        public string ReturnStatus { get; set; }
        public string ReturnDescription { get; set; }
        public Nullable<decimal> NoofItems { get; set; }
        public Nullable<decimal> TotalWeight { get; set; }
        public int TotalCount { get; set; }
    }
}