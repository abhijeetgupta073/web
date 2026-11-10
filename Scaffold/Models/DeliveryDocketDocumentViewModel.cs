using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scaffold.Models
{
    public class DeliveryDocketDocumentViewModel
    {
      public Nullable<int> DDDocumentId { get; set; }
      public string  DocumentName { get; set; }
      public string  Description { get; set; }
      public Nullable<System.DateTime> DateOfUpload  { get; set; }
      public Nullable<int>  TransID { get; set; }
    }
}