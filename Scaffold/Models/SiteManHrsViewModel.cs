using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scaffold.Models
{
    public class SiteManHrsViewModel
    {
            public string ClientID { get; set; }
            public Nullable<int> SiteManHrsId { get; set; }
            public string SiteLocId { get; set; }
            public string SiteLocation { get; set; }
            public string EntryDate { get; set; }
            public int UserId { get; set; }
            public string name { get; set; }

            public Nullable<decimal> NoOfWorkedHrs { get; set; }
            public string IsPresent { get; set; }
            public Nullable<decimal> AllocatedHrs { get; set; }
            public string Category { get; set; }
            public string UID { get; set; }

    }
}