using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scaffold.Models
{
    public class CopyHistoricalJobMaterialList
    {
        public  Nullable<int> JobId { get; set; }
        public string Location { get; set; }
        public string BuildingNo { get; set; }
        public string AssetNo { get; set; }
        public string Assetdesc { get; set; }
        public string Scaffref { get; set; }
        public string OptimizeStatus { get; set; }
    }
}