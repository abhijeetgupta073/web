using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scaffold.Models
{
    public class InspectionFormQuestionDataViewModel
    {
        public string ContentID { get; set; }
        public Nullable<int> ChkBoxYes { get; set; }
        public Nullable<int> ChkBoxNo { get; set; }
        public Nullable<int> ChkBoxNA { get; set; }
        public string Action { get; set; }
    }
}