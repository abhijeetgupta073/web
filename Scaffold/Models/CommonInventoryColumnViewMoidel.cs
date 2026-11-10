using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scaffold.Models
{
    public class CommonInventoryColumnViewMoidel
    {
        public string ClientID { get; set; }
        public string SiteID  { get; set; }
        public string pageID { get; set; }
        public string RequestStatus { get; set; }
        public string UserID { get; set; }
        public string FromName { get; set; }
        public string ToName { get; set; }
        public string ToNameId { get; set; }
        public string RequestId { get; set; }
        public string FromNameID { get; set; }
        public string RequestedPerson { get; set; }
        public Nullable<System.DateTime> RequestedDate  { get; set; }
        public Nullable<System.DateTime> RequiredSDate { get; set; }
        public string Description { get; set; }
        public string FromtocheckID { get; set; }
        public string Supplier { get; set; }
        public string Comment { get; set; }
        public string fromtime { get; set; }
        public string RequiredDate { get; set; }
        }
}