using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scaffold.Controllers
{
    public class InvoicedSurgeReportController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: SubCategories
        public ActionResult InvoicedSurgeReport(string UID, string UserRole, string id)
        {
            return View();
        }

        public ActionResult GetSubCategoryData(string id)
        {
            string query = "";
            string query2 = "";
           // query = "select * from tblInvoicedSurgeRate where ClientId = " + databaseCon.Decrypt(id) + "";
            query = "select SurgeRate_ID,TonnageHireRate,TonnageSurgeRate,SurgeAppliedTonnage,AgreedHire from tblInvoicedSurgeRate where ClientId = " + databaseCon.Decrypt(id) + "";
            query2 = "select distinct(Name+' '+'('+status+')') as name from tblclient where ClientId = " + databaseCon.Decrypt(id) + " Order by name";
            var getData = dbContext.Database.SqlQuery<SubCategoryViewModel1>(query).ToList();
            var getData2 = dbContext.Database.SqlQuery<ClientViewModel>(query2).ToList();
            return Json(new { getData = getData, getData2 = getData2 }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult SaveSurgeData(string hireRate, string SurgeRate, string SurgeAppliedTonnage,string Clientid)
        {
            string query = "";
            string Result = "";
            string Message = "";

            query = "update tblInvoicedSurgeRate set TonnageHireRate = " + hireRate + ", TonnageSurgeRate = " + SurgeRate + ", SurgeAppliedTonnage = " + SurgeAppliedTonnage + ", ClientId = " + databaseCon.Decrypt(Clientid) + "";

            DataTable dt = databaseCon.GetData(query);


            Result = "Success";
            Message = "Record Saved Successfully ";

            var response = new { Result = Result, Message = Message };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

    }

    public class SubCategoryViewModel1
    {
        public Nullable<int> SurgeRate_ID { get; set; }
        public Nullable<Decimal> TonnageHireRate { get; set; }
        public Nullable<Decimal> TonnageSurgeRate { get; set; }
        public Nullable<Decimal> SurgeAppliedTonnage { get; set; }

        public Nullable<Decimal> AgreedHire { get; set; }

    }

}