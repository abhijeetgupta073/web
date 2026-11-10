using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scaffold.Models
{
    public class FlagRegisterViewModel
    {
        public int  RegisterId { get; set; }
        public string Scaffref { get; set; }
        public string Location { get; set; }
        public string BuildingNo { get; set; }
        public string AssetNo { get; set; }
        public string FloorLevel { get; set; }
        public string Description { get; set; }
        public string Bearing { get; set; }
        public string Photo { get; set; }
        public string ManHrsErect { get; set; }
        public string ManHrsDismantle { get; set; }
        public string JobInventoryList { get; set; }
        public string FinalMaterialList { get; set; }
        public string Length { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string CycloneAction { get; set; }
        public string  TotalDeBoardHrs { get; set;}
        public string TotalReinstateHrs { get; set; }
        public DateTime datetagged { get; set; }
        public Nullable<DateTime> DateOfDismantle{ get; set; }
        public string discipline { get; set; }
        public string EngineeredDesign { get; set; }
        public string ClientRef { get; set; }
        public string RegisterPin { get; set; }
        public string SitePin { get; set; }
        public string SiteLocation { get; set; }
        public string WorkGroup { get; set; }
        

    }
}