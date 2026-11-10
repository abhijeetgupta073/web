using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scaffold.Models
{
    public class Pro_TBLAREA_GETViewModel
    {
        public int ID { get; set; }
        public string Area { get; set; }
        public Nullable<int> ClientID { get; set; }
        public string AreaStatus { get; set; }
    }
}