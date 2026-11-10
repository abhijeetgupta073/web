using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Scaffold_DataAccess;

namespace Scaffold.Models
{
    public class InventoryChangedViewModel
    {
        public int InvChangeId { get; set; }
        public Nullable<int> ClientId { get; set; }
        public Nullable<int> InventoryId { get; set; }
        public Nullable<int> OldQuantity { get; set; }
        public Nullable<int> NewQuantity { get; set; }
        public string ChangeComments { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }

        public virtual tblClient tblClient { get; set; }
        public virtual tblInventory tblInventory { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string ChangeType { get; set; }
        }
}