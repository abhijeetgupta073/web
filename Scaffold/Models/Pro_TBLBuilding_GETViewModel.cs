using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scaffold.Models
{
    public class Pro_TBLBuilding_GETViewModel
    {
        public int ID { get; set; }
        public string BuildingNo { get; set; }
        public string BuildingStatus { get; set; }
        public Nullable<int> ClientId { get; set; }
        public Nullable<int> AreaId { get; set; }
    }
}