using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scaffold.Models
{
    public class FloorLevelddlViewModel
    {
        public Nullable<int> SiteLocID {get;set;}
        public string SiteLocation { get; set; }
        public string area { get; set; }
        public Nullable<int> AreaID { get; set; }

        public Nullable<int> buildingid { get; set; }
        public string buildingno { get; set; }
    }
}