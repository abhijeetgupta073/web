using System;
using System.Collections.Generic;
using System.Text;

namespace Scaffold.Models
{
    public class JqueryDatatableParam
    {
        public string sEcho { get; set; }
        public string sSearch { get; set; }
        public int iDisplayLength { get; set; }
        public int iDisplayStart { get; set; }
        public int iColumns { get; set; }
        public int iSortCol_0 { get; set; }
        public string sSortDir_0 { get; set; }
        public int iSortingCols { get; set; }
        public string sColumns { get; set; }
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public SearchStructure search { get; set; }
        public List<OrderStructure> order { get; set; }
    }
    public class SearchStructure
    {
        public string value { get; set; }
        public bool regex { get; set; }
    }
    public class OrderStructure
    {
        public string dir { get; set; }
        public int column { get; set; }
    }
}
