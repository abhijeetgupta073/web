using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scaffold.Models
{
    public class OffSystemInspectionSaveViewModel
    {
        public string ClientID { get; set; }
        public string UID { get; set; }
        public string RegisterID { get; set; }
        public string StatusConfirmed { get; set; }
        public string InspectedBy { get; set; }
        public string InspectionComments { get; set; }
        public Nullable<System.DateTime> InspectionDate { get; set; }
        public double TimeOffSet { get; set; }
    }
}