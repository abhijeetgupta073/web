using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scaffold.Models
{
    public class SubCategoryViewModel
    {
        public int SubCategoryID { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string ClientID { get; set; }
        public string HireMeasurement { get; set; }
        public Nullable<decimal> HireRate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string UpdatedBY { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
    }
}