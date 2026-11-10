using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scaffold.Controllers
{
    public class ActualPlanningWeekController : Controller
    {
        ScaffoldEntities dbcontext = new ScaffoldEntities();
        // GET: ActualPlanningWeek
        public ActionResult ActualPlanningWeek()
        {
            return View();
        }

        public ActionResult BindWeekStartDayDropdown(string id)
        {

            var WeekStartDayDropdown = dbcontext.Database.SqlQuery<ClientViewModel>("select WeekStartDay from tblClient where ClientId = '" + databaseCon.Decrypt(id) + "'").ToList();

            return Json(WeekStartDayDropdown, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GenerateActualPlanningWeek(string id, string Fromdate, string UID)
        {
            id = databaseCon.Decrypt(id);
            string LoginUserQuery = "select LoginId from tblUser where UserId = '"+ databaseCon.Decrypt(UID) + "'";
            DataTable LoginUserInfo = databaseCon.GetData(LoginUserQuery);
            string generateQuery = $"exec Pro_GenerateActualPlanningWeek @Fromdate='{Fromdate}',@ClientId='{id}',@GeneratedBy='{LoginUserInfo.Rows[0][0].ToString()}'";
            var generate = databaseCon.GetData(generateQuery);
            return Json(new { Result = "Success", Message = "Actual Planning week generated successfully." });
        }

        public ActionResult DeleteActualPlanningWeek(string id)
        {
            id = databaseCon.Decrypt(id);
            string DeleteWeeksQuery = "delete from tblYearWeekDate2024 where ClientId = '" + id + "'";
            var delete = databaseCon.GetData(DeleteWeeksQuery);
            return Json(new { Result = "Success", Message = "Actual Planning week deleted successfully." });
        }

        public ActionResult ActualPlanningWeekData(string id)
        {

            id = databaseCon.Decrypt(id);
            string query = "select WeekNo, WeekStartDate, WeekEndDate, CreatedBy from tblYearWeekDate2024 where ClientId = '"+id+"'";
            var getData = dbcontext.Database.SqlQuery<ActualPlanningWeekViewModel>(query).ToList();
            return Json(getData, JsonRequestBehavior.AllowGet);
        }
    }

    public class ActualPlanningWeekViewModel
    {
        public int WeekNo { get; set; }
        public DateTime WeekStartDate { get; set; }
        public DateTime? WeekEndDate { get; set; }
        public string CreatedBy { get; set; }

    }
}