using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scaffold.Models
{
    public class ClientViewModel
    {
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string EmailID { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public string Status { get; set; }
        public string InspTimeFrame { get; set; }
        public string ClientImage { get; set; }
        public string CellPhone { get; set; }
        public Nullable<int> ChecklistId { get; set; }
        public Nullable<decimal> StandardWorkingHrs { get; set; }
        public string Country { get; set; }
        public string Plans { get; set; }
        public string WeekStartDay { get; set; }
        public Nullable<int> PlannedStandingDuration { get; set; }
        public Nullable<decimal> ManHrsRatio { get; set; }
        public Nullable<bool> IPAuthentication { get; set; }
        public Nullable<bool> ManHoursManagement { get; set; }
        public Nullable<bool> IsMainClient { get; set; }
        public Nullable<bool> InspectionCheckListColor { get; set; }
        public Nullable<bool> OfflineEnabled { get; set; }
        public Nullable<bool> HireInventory { get; set; }
    }
}