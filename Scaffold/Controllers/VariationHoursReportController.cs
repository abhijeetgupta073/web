
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scaffold.App_Code;
using Scaffold.Controllers;
using Scaffold_DataAccess;

namespace Scaffold.Controllers
{
    public class VariationHoursReportController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: VariationHoursReport
        public ActionResult VariationHoursReport(string UID, string UserRole, string id)
        {
            return View();
        }
        public ActionResult GetVariationHoursReport(string Fromdate, string ToDate, string id, string Siteid)
        {
            id = databaseCon.Decrypt(id);
            string query = $"exec Pro_WorkCarriedOutVariation @Fromdate='{Fromdate}', @ToDate='{ToDate}',@ClientId='{id}',@SiteId='{Siteid}'";
            var getData = dbContext.Database.SqlQuery<WorkCarriedOutVariationViewModel>(query).ToList();
            return Json(getData, JsonRequestBehavior.AllowGet);
        }



    }
    public class WorkCarriedOutVariationViewModel
    {
        public string ScaffRef { get; set; }
        public DateTime BuiltDate { get; set; }
        public DateTime? DateOfDismantle { get; set; }
        public Nullable<int> HireDaysToDate { get; set; }
        public DateTime? Date { get; set; }
        public string JobDescription { get; set; }
        public string WorkCarriedOut { get; set; }
        public Nullable<decimal> VariationInHrs { get; set; }
        public Nullable<decimal> VariationHrsToDate { get; set; }
        public Nullable<decimal> TotalVariationHrs { get; set; }
        public string EnteredBy { get; set; }

    }
}