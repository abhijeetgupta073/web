using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scaffold.Models
{
    public class ClientRepresentationViewModel
    {
        public int CRId { get; set; }
        public Nullable<int> ClientId { get; set; }
        public string Position { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public string status { get; set; }
    }
}