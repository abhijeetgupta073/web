using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scaffold.Models
{
    public class Pro_TBLAsset_GETViewModel
    {
        public int ID { get; set; }
        public string AssetNo { get; set; }
        public string AssetDesc { get; set; }
        public string AssetStatus { get; set; }
        public Nullable<int> ClientId { get; set; }
        public Nullable<int> BuildingId { get; set; }
    }
}