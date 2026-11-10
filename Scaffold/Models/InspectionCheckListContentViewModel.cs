using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scaffold.Models
{
    public class InspectionCheckListContentViewModel
    {
        public int ContentID { get; set; }
        public int ClientID { get; set; }
        public int CheckListID { get; set; }
        public string CheckListContent { get; set; }
        public Nullable<int> checklistItem { get; set; }
        public Nullable<int> subChildItem { get; set; }
        public Nullable<int> itemOrder { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string Createdby { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<int> CopyFromCheckListItem { get; set; }
        public Nullable<int> CopyFromSubChilditem { get; set; }
        public Nullable<int> CopyFromitemOrder { get; set; }
        public Nullable<int> CopyFromChecklistId { get; set; }
        public Nullable<bool> IsNA { get; set; }
        public Nullable<int> CopyFromContentID { get; set; }
    }
}