using Scaffold_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scaffold.Controllers
{
    public class UserAuditController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: UserAudit
        public ActionResult UserAudit(string UID, string UserRole, string id)
        {
            return View();
        }

        public ActionResult GetUserAuditData(string id, string fromDate, string toDate)
        {

            string query = "";

            DateTime Date = Convert.ToDateTime(toDate).AddDays(1);
            toDate = Date.ToString();
            if (fromDate == null || toDate == null)
            {
                //query = "select case when UserId is null then 'No' else 'Yes' end as ValidUser , IPAddress, AL.Country as Country, UTCLoginTime, SessionId, case when LoginAttemptSuccess=0 then 'No' else 'Yes' end as LoginAttemptSuccess, Username, AL.LoginFailureReason as LoginFailureReason, Name from TblAuditLogin AL left join tblClient C on AL.ClientID = C.ClientId where UTCLoginTime >= '" + fromDate.SelectedDate + "' and UTCLoginTime < '" + Date + "'";
                //Changed qyery on 01/April/2020
                query = "select Name,AU.Role,AU.Status,AU.IPAddress, case when AttemptSuccess=0 then 'No' else 'Yes' end as AttemptSuccess, AU.UserId,U.FirstName,U.LastName,U.EmailId as Username,AU.CreatedBy,AU.CreatedOn,AtteptReason from TblAuditUser AU left join tblClient C on AU.ClientID = C.ClientId  left join tblUser U on AU.UserID=U.userId where AU.CreatedOn >= '" + fromDate + "' and AU.CreatedOn  < '" + Date + "' order by AuditUserId asc";
            }
            else
            {
                //query = "select case when UserId is null then 'No' else 'Yes' end as ValidUser , IPAddress, AL.Country as Country, UTCLoginTime, SessionId, case when LoginAttemptSuccess = 0 then 'No' else 'Yes' end as LoginAttemptSuccess, Username, AL.LoginFailureReason as LoginFailureReason, Name from TblAuditLogin AL left join tblClient C on AL.ClientID = C.ClientId where UTCLoginTime >= '" + ViewState["SelectedfromDate"] + "' and UTCLoginTime < '" + ViewState["SelectedtoDate"] + "'";
                //Changed qyery on 01/April/2020
                query = "select Name,AU.Role,AU.Status,AU.IPAddress, case when AttemptSuccess=0 then 'No' else 'Yes' end as AttemptSuccess, AU.UserId,U.FirstName,U.LastName,U.EmailId as Username,AU.CreatedBy,AU.CreatedOn,AtteptReason from TblAuditUser AU left join tblClient C on AU.ClientID = C.ClientId  left join tblUser U on AU.UserID=U.userId where AU.CreatedOn >=  '" + fromDate + "' and AU.CreatedOn  < '" + toDate + "' order by AuditUserId asc";
            }

            // DataTable dt = databaseCon.GetData(query);
            //if (dt.Rows.Count < 1)
            //{
            //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "hide", "<script>filterhide();</script>", false);
            //}
            //LoginAuditID.DataSource = dt;

            var getData = dbContext.Database.SqlQuery<UserAuditViewModel>(query).ToList();
            return Json(getData, JsonRequestBehavior.AllowGet);
        }

    }

    public class UserAuditViewModel
    {
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        public string AttemptSuccess { get; set; }
        public string IPAddress { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<DateTime> CreatedOn { get; set; }
        public string AtteptReason { get; set; }
    }

}