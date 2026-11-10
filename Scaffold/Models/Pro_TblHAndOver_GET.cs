using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scaffold.Models
{
    public class Pro_TblHAndOver_GET
    {
        public int ID { get; set; }
        public Nullable<System.DateTime> HandOverDate { get; set; }
        public int HandOverTime { get; set; }
        public string HandOverDesc { get; set; }
        public string CertificateNo { get; set; }
        public string HandOverComments { get; set; }
        public Nullable<bool> IsConfirmed { get; set; }
        public string ContractorSign { get; set; }
    }
}