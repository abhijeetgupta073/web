using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scaffold.Models
{
    public class DayDocketViewModel
    {

       public string SiteRef { get; set; }
       public string EntryDate { get; set; }
       public string Comments { get; set; }
        public Nullable<int> SiteLocId { get; set; }
        public string SiteLocation { get; set; }
        public string CompanyRep { get; set; }
        public string ClientRep { get; set; }
        public string SupervisorSign { get; set; }
        public string ClientSign { get; set; }
        public Nullable<System.DateTime> ClientSignDate { get; set; }
        public Nullable<System.DateTime> SupervisorSignDate { get; set; }
        public Nullable<System.DateTime> ReceivedDate { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string CellPhone { get; set; }
        public string ZIP { get; set; }
    }
}