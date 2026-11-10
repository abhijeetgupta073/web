using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scaffold.Models
{
    public class HandOverFormViewModel
    {
        public string CurrentDate { get; set; }
        public int HandOverTime { get; set; }
        public Nullable<System.DateTime> HandOverDate { get; set; }
        public string OwnerName { get; set; }
        public string SiteLocation { get; set; }
        public string BuildingNo { get; set; }
        public string Location { get; set; }
        public string FloorLevel { get; set; }
        public string ScaffoldTag { get; set; }
        public Nullable<double> Height { get; set; }
        public Nullable<double> Width { get; set; }
        public Nullable<double> Length { get; set; }
        public string DesignDrawingNo { get; set; }
        public string ReasonForAccess { get; set; }
        public string NumberOfPlatform { get; set; }
        public string EngineeredDesign { get; set; }
        public string HandOverDesc { get; set; }
        public Nullable<bool> IsConfirmed { get; set; }
        public string HandOverImage { get; set; }
        public string DutyRating { get; set; }
        public string NoOfWorkingDecks { get; set; }
        public string HeightOFTopWorkingPlateform { get; set; }
        public string HasEncapsulated { get; set; }
        public string HandOverLat { get; set; }
        public string HandOverLong { get; set; }
        public string HandOverAddress { get; set; }
        public string HandOverAudio { get; set; }
        public string HandOverVedio { get; set; }
        public string HandOverComments { get; set; }
        public string HandOverAcknowledgment { get; set; }
        public string CertificateNo { get; set; }
        public string ContractorRepName { get; set; }
        public string ContractorRepLicence { get; set; }
        public string ContractorRepPosition { get; set; }
        public string ContractorSign { get; set; }
        public Nullable<int> CRId { get; set; }
        public string RepresentativeName { get; set; }
        public string RepresentativeSign { get; set; }
        public string PhotoAngle { get; set; }
    }
}