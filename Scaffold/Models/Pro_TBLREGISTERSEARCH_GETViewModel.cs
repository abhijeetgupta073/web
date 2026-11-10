using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scaffold.Models
{
    public class Pro_TBLREGISTERSEARCH_GETViewModel
    {
        public string WorkGroup { get; set; }

        public string SubGroup { get; set; }

        public Nullable<int> BuildingID { get; set; }

        public string ClientScaffref { get; set; }

        public Nullable<int> LocationId { get; set; }

        public Nullable<int> SiteLocId { get; set; }

        public Nullable<bool> showCompleted { get; set; }

        public string discipline { get; set; }
        public string WONumber { get; set; }
        public string SubDiscipline { get; set; }
        public string Supervisor { get; set; }
    }
}