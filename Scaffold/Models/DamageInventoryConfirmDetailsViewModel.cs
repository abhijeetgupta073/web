using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scaffold.Models
{
    public class DamageInventoryConfirmDetailsViewModel
    {
        public int DIDetailID { get; set; }
        public string MaterialType { get; set; }
        public string ItemCode { get; set; }
        public string ItemDesc { get; set; }
        public double ItemWeight { get; set; }
        public string Supplier { get; set; }
        public string SupplierDocketNo { get; set; }
        public string Reason { get; set; }
        public string DateActioned { get; set; }
        public string AvaQty { get; set; }
        public Decimal DamagedQuantity { get; set; }
        public double TotalItemWeight { get; set; }

        public Nullable<int> DDDocumentID { get; set; }
        public string DocumentName { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> DateOfUpload { get; set; }
        public string DocumentPdfPath { get; set; }
    }
}