using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scaffold.Models
{
    public class RequestInventoryViewModel
    {

        public Nullable<int> transId { get; set; }
        public string RequestStatus { get; set; }
        public string transtype { get; set; }
        public Nullable<int> FromId { get; set; }
        public string TransDesc { get; set; }
        public string DeliveryStatus { get; set; }
        public string FromName { get; set; }
        public string ToName { get; set; }
        public Nullable<int> ToId { get; set; }
        public string To { get; set; }
        public string FromPerson { get; set; }
        public string ToPerson { get; set; }
        public Nullable<System.DateTime> FromTime { get; set; }
        public Nullable<System.DateTime> RequiredDate { get; set; }
        public Nullable<System.DateTime> ToTime { get; set; }
        public Nullable<System.DateTime> PickDate { get; set; }
        public Nullable<int> NoofItems { get; set; }
        public Nullable<Double> TotalWeight { get; set; }
        public Nullable<Boolean> ReceiptConfirmation { get; set; }
        public string Supplier { get; set; }
        public string Comment { get; set; }
        public Nullable<System.DateTime> LastEmailSentOn { get; set; }

        public int TotalCount { get; set; }
    }
}