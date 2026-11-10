using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scaffold.Models
{
    public class Pro_TBLINSPECTION_GETModelView
    {
        public int InspectionID { get; set; }
        public Nullable<System.DateTime> InspectionDate { get; set; }
        public int InspectionTime { get; set; }
        public Nullable<int> InspectedById { get; set; }
        public string InspectedBy { get; set; }
        public string InspFileName { get; set; }
        public string Comments { get; set; }
        public Nullable<int> RegisterID { get; set; }
        public Nullable<bool> OnlineInsp { get; set; }
        public Nullable<bool> CompleteInsp { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public string GPSAddress { get; set; }
        public string Inspimage { get; set; }
        public string InspAudio { get; set; }
        public string Inspvedio { get; set; }
        public string InspSign { get; set; }
        public string InspComments { get; set; }
        public string Acknowledgment { get; set; }
        public string InspStatus { get; set; }
        public Nullable<System.DateTime> AckDate { get; set; }
        public int AckDateTime { get; set; }
        public Nullable<System.DateTime> LastEmailSentOn { get; set; }
        public Nullable<System.DateTime> ScheduledOrAckDate { get; set; }
        public int ScheduledOrAckTime { get; set; }
        public Nullable<bool> OffSystemInspection { get;set;}
    }
}