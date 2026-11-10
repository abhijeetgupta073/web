using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scaffold.Models
{
    public class AllDamageReportViewModel
    {
        public string MaterialType { get; set; }
        public string ItemCode { get; set; }
        public string ItemDesc { get; set; }
        public double ItemWeight { get; set; }
        public string FromName { get; set; }
        public string ToName { get; set; }
        public string Reason { get; set; }
        public string Name { get; set; }
        public string DamageEnterBy { get; set; }
        public string DamageEnterOn { get; set; }
        public int InventoryId { get; set; }
        public decimal QuantityDamaged { get; set; }
        public double TotalWeight { get; set; }
    }
}