using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scaffold.Models
{
    public class ManHrsViewModel
    {

       public int ManHrsId { get; set; }
       public Nullable<int> SiteManHrsId { get; set; }
       public Nullable<int> SiteLocId { get; set; }
       public string SiteLocation { get; set; }
       public int RegisterID { get; set; }
       public string Description { get; set; }
       public Nullable<int> UserId { get; set; }
       public Nullable<System.DateTime> EntryDate { get; set; }
       // public string EntryDate { get; set; }
        public string name { get; set; }
       public Nullable<decimal> AdditionalHrs { get; set; }
       public string TypeofVariation { get; set; }
       public string VariationDesc { get; set; }
       public string IncludedInDayDocket { get; set; }
       public string VariationComments { get; set; }
       public Nullable<decimal> ErectionHrs { get; set; }
       public Nullable<decimal> ModificationHrs { get; set; }
       public Nullable<decimal> DismantleHrs { get; set; }
       public decimal NoOfHrs { get; set; }
       public decimal AllocatedHrs { get; set; }
       public string Category { get; set; }
       public string UID { get; set; }


    }
}