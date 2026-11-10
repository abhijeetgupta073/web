using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scaffold.App_Code;
using Scaffold_DataAccess;

namespace Scaffold.Controllers
{
    public class TermsNConditionController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: TermsNCondition
        public ActionResult TermsNCondition()
        {
            return View();
        }
        public class tblTermNConditionViewModel
        {
            public string MainPoint { get; set; }
            public string SubPoint { get; set; }
            public string Text { get; set; }
        }
        public ActionResult bindtermsandcondition(string UID)
        {
            // REFACTORED: Replaced SELECT * with explicit columns
            // Original: "Select MainPoint,SubPoint,Text from tblTermNCondition where clientId=..."
            // Already optimized - no SELECT * used here

            var getData = dbContext.Database.SqlQuery<tblTermNConditionViewModel>(
                "Select MainPoint, SubPoint, Text " +
                "from tblTermNCondition " +
                "where clientId = (select ClientId from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "') " +
                "and Category = 'Terms' " +
                "order by TermOrder"
            ).ToList();

            return Json(getData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult bindPolicyandcondition(string UID)
        {
            // REFACTORED: Replaced SELECT * with explicit columns
            // The subqueries in the original used SELECT * which retrieves all columns
            // We need to explicitly list only the required columns: TermOrder, MainPoint, SubPoint, Text, Category, ClientId

            var getData = dbContext.Database.SqlQuery<tblTermNConditionViewModel>(
                "select case when v1.mainpoint = v2.mainpoint then '' else isnull(v1.mainpoint, '') end as MainPoint, " +
                "isnull(v1.subpoint, '') as Subpoint, " +
                "isnull(v1.text, '') as Text, " +
                "v1.TermOrder " +
                "from " +
                "(select TermOrder, MainPoint, SubPoint, Text, Category, ClientId " +
                "from tblTermNCondition " +
                "where Category = 'Policy' " +
                "and clientId = (select ClientId from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "')) v1 " +
                "left join " +
                "(select TermOrder, MainPoint, SubPoint, Text, Category, ClientId " +
                "from tblTermNCondition " +
                "where Category = 'Policy' " +
                "and clientId = (select ClientId from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "')) v2 " +
                "on v1.TermOrder = v2.TermOrder + 1 " +
                "order by v1.TermOrder"
            ).ToList();

            return Json(getData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Accept_Click(string ChkAgree, string UID, string UserRole)
        {
            string query1 = "update tbluser set Accept='" + ChkAgree + "' where UserId='" + databaseCon.Decrypt(UID) + "'";
            int result = databaseCon.RunCmd(query1);
            string clientid = Request.QueryString["id"].ToString();
            int id = Convert.ToInt32(databaseCon.Decrypt(clientid));

            // REFACTORED: Query already uses explicit column (HideMainDepotInventory)
            DataTable dtMainDepot = databaseCon.GetData(
                "select isnull(HideMainDepotInventory, 0) as HideMainDepotInventory " +
                "from tblClient " +
                "where clientid = '" + id + "'"
            );

            HideMainDepotInventoryViewModel GetHideMainDepotInventory = new HideMainDepotInventoryViewModel();
            if ((dtMainDepot.Rows[0]["HideMainDepotInventory"]).ToString() != "True")
            {
                GetHideMainDepotInventory.HideMainDepotInventory = false;
            }
            else
            {
                GetHideMainDepotInventory.HideMainDepotInventory = true;
            }

            var TempURL = databaseCon.RedirectingCommonLink(id, UID, UserRole, GetHideMainDepotInventory.HideMainDepotInventory);

            Response.Redirect("/" + TempURL + "?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"]);
            return Json(TempURL);
        }
    }
}