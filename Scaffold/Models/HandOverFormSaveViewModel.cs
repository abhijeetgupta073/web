using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scaffold.Models
{
    public class HandOverFormSaveViewModel
    {
       public string id { get; set; }
       public string UID { get; set; }
       public string HandOverId { get; set; }
       public string hdnlatitude { get; set; }
       public string hdnlongitude { get; set; }
       public string hdnAddress { get; set; } 
       public string sketch_data { get; set; }
       public string sketch_data2 { get; set; }
       public string tempimg { get; set; }
       public string tempAud { get; set; }
       public string tempVideo { get; set; }
       public string map_image { get; set; }
       public string imgangle { get; set; }
       public string rdpInspectiondate { get; set; }
       public string hdnTimeOffSet { get; set; }
       public string txtsiteManager { get; set; }
       public string ddlsiteManager { get; set; }
       public string lblOwnerName { get; set; }
       public string comment { get; set; }
       public string handovrDesc { get; set; }
       public Boolean confirmcheck { get; set; }
       public string EncapsulateValue { get; set; }
       public string ddlContractorRepresentative { get; set; }
       public string lbllicence { get; set; }
       public string lblrepresentPost { get; set; }

        public string sitelbl { get; set; }
        public string BuildingNolbl { get; set; }
        public string lblfloor { get; set; }
        public string lblReasonforAccess { get; set; }
        public string lblDutyRating { get; set; }
        public string lblWrokingDeck { get; set; }
        public string lblNumberOfPlatform { get; set; }
        public string lbldesigndNo { get; set; }
        public string lblLength { get; set; }
        public string lblWidth { get; set; }
        public string lblHeight { get; set; }
        public string scaffreflbl { get; set; }
        public string EngineeredDesign { get; set; }


    }
}