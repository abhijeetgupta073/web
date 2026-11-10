using Scaffold.App_Code;
using Scaffold_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scaffold.Controllers
{
    public class LoginAuditController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: LoginAudit
        public ActionResult LoginAudit(string UID, string UserRole, string id)
        {
            return View();
        }

        public ActionResult GetLoginAuditData(string fromDate, string toDate,string hdnTimeOffSet)
        {

            //fromDate.SelectedDate = databaseCon.ToTimeZoneTime(DateTime.Now).AddDays(-2);
            //toDate.SelectedDate = databaseCon.ToTimeZoneTime(DateTime.Now);

            string query = "";
            // DateTime Date = Convert.ToDateTime(toDate.SelectedDate).AddDays(1);
            DateTime Date = Convert.ToDateTime(toDate).AddDays(1);
            toDate = Date.ToString();
            if (fromDate == null || toDate == null)
            {
                // query = "select case when UserId is null then 'No' else 'Yes' end as ValidUser , IPAddress, AL.Country as Country, UTCLoginTime, SessionId, case when LoginAttemptSuccess=0 then 'No' else 'Yes' end as LoginAttemptSuccess, Username, AL.LoginFailureReason as LoginFailureReason, Name,LoginTimezone, DateAdd(minute,isnull(LoginTimezone,0),UTCLoginTime)as GeographicLoginTime, DateAdd(HOUR,8,UTCLoginTime)as PerthTime from TblAuditLogin AL left join tblClient C on AL.ClientID = C.ClientId where UTCLoginTime >= '" + fromDate + "' and UTCLoginTime < '" + Date + "'";

                //  query = "select case when UserId is null then 'No' else 'Yes' end as ValidUser , IPAddress, AL.Country as Country, UTCLoginTime, SessionId, case when LoginAttemptSuccess=0 then 'No' else 'Yes' end as LoginAttemptSuccess, Username, AL.LoginFailureReason as LoginFailureReason, Name,LoginTimezone, DateAdd(minute,isnull(LoginTimezone,0),UTCLoginTime)as GeographicLoginTime, DateAdd(HOUR,8,UTCLoginTime)as PerthTime,regionName,AL.city,AL.zip,latitute,longitude,timezone  from TblAuditLogin AL left join tblClient C on AL.ClientID = C.ClientId where UTCLoginTime >= '" + fromDate + "' and UTCLoginTime < '" + Date + "' order by AuditLoginId asc";

                query = "select case when UserId is null then 'No' else 'Yes' end as ValidUser , IPAddress, AL.Country as Country, UTCLoginTime, SessionId, case when LoginAttemptSuccess=0 then 'No' else 'Yes' end as LoginAttemptSuccess, Username, AL.LoginFailureReason as LoginFailureReason, Name,LoginTimezone, DateAdd(minute,isnull(LoginTimezone,0),UTCLoginTime)as GeographicLoginTime, DateAdd(HOUR,8,UTCLoginTime)as PerthTime,DateAdd(HOUR, 8, UTCLogoutTime) as PerthLogoutTime,CONVERT(varchar, DATEADD(MINUTE,DATEDIFF(MINUTE, UTCLoginTime,UTCLogoutTime),0),108) as PerthTotalSessionTime,regionName,AL.city,AL.zip,latitute,longitude,timezone  from TblAuditLogin AL left join tblClient C on AL.ClientID = C.ClientId where DateAdd(minute, LoginTimezone, UTCLoginTime) >= '" + fromDate + "' and DateAdd(minute, LoginTimezone, UTCLoginTime) < '" + toDate + "' order by AuditLoginId asc";
            }
            else
            {

                // query = "select case when UserId is null then 'No' else 'Yes' end as ValidUser , IPAddress, AL.Country as Country, UTCLoginTime, SessionId, case when LoginAttemptSuccess = 0 then 'No' else 'Yes' end as LoginAttemptSuccess, Username, AL.LoginFailureReason as LoginFailureReason, Name,LoginTimezone, DateAdd(minute,isnull(LoginTimezone,0),UTCLoginTime)as GeographicLoginTime, DateAdd(HOUR,8,UTCLoginTime)as PerthTime from TblAuditLogin AL left join tblClient C on AL.ClientID = C.ClientId where UTCLoginTime >= '" + fromDate + "' and UTCLoginTime < '" + toDate + "'";

                //  query = "select case when UserId is null then 'No' else 'Yes' end as ValidUser , IPAddress, AL.Country as Country, UTCLoginTime, SessionId, case when LoginAttemptSuccess = 0 then 'No' else 'Yes' end as LoginAttemptSuccess, Username, AL.LoginFailureReason as LoginFailureReason, Name,LoginTimezone, DateAdd(minute,isnull(LoginTimezone,0),UTCLoginTime)as GeographicLoginTime, DateAdd(HOUR,8,UTCLoginTime)as PerthTime,regionName,AL.city,AL.zip,latitute,longitude,timezone from TblAuditLogin AL left join tblClient C on AL.ClientID = C.ClientId where UTCLoginTime >= '" + fromDate + "' and UTCLoginTime < '" + toDate + "' order by AuditLoginId asc";
               
                 query = "select case when UserId is null then 'No' else 'Yes' end as ValidUser , IPAddress, AL.Country as Country,UTCLoginTime, SessionId, case when LoginAttemptSuccess = 0 then 'No' else 'Yes' end as LoginAttemptSuccess,Username, AL.LoginFailureReason as LoginFailureReason, Name,LoginTimezone, DateAdd(minute, isnull(LoginTimezone, 0),UTCLoginTime) as GeographicLoginTime, DateAdd(HOUR, 8, UTCLoginTime) as PerthTime,DateAdd(Hour, 8, UTCLogoutTime) as PerthLogoutTime,CONVERT(varchar, DATEADD(MINUTE,DATEDIFF(MINUTE, UTCLoginTime,UTCLogoutTime),0),108) as PerthTotalSessionTime,regionName,AL.city,AL.zip,latitute,longitude,timezone from TblAuditLogin AL left join tblClient C on AL.ClientID = C.ClientId where DateAdd(minute, LoginTimezone, UTCLoginTime) >= '" + fromDate + "' and DateAdd(minute, LoginTimezone, UTCLoginTime) < '" + toDate + "' order by AuditLoginId asc";
            }

           // DataTable dt = databaseCon.GetData(query);
            //if (dt.Rows.Count < 1)
            //{
            //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "hide", "<script>filterhide();</script>", false);
            //}
            //LoginAuditID.DataSource = dt;

            var getData = dbContext.Database.SqlQuery<LoginAuditViewModel>(query).ToList();
            if (getData != null)
            {

                foreach (var item in getData)
                {
                    var UTCLoginTime = item.UTCLoginTime;
                    if (UTCLoginTime != null)
                    {
                        item.UTCLoginTime = Convert.ToDateTime(UTCLoginTime).AddMinutes(Convert.ToDouble(hdnTimeOffSet));
                    }
                    var GeographicLoginTime = item.GeographicLoginTime;
                    if (GeographicLoginTime != null)
                    {
                        item.GeographicLoginTime = Convert.ToDateTime(GeographicLoginTime).AddMinutes(Convert.ToDouble(hdnTimeOffSet));
                    }
                    var PerthTime = item.PerthTime;
                    if (PerthTime != null)
                    {
                        item.PerthTime = Convert.ToDateTime(PerthTime).AddMinutes(Convert.ToDouble(hdnTimeOffSet));
                    }
                    var PerthLogoutTime = item.PerthLogoutTime;
                    if (PerthLogoutTime != null)
                        {
                        item.PerthLogoutTime = Convert.ToDateTime(PerthLogoutTime).AddMinutes(Convert.ToDouble(hdnTimeOffSet));
                        }
                    }
            }
            return Json(getData, JsonRequestBehavior.AllowGet);
        }

    }

    public class LoginAuditViewModel 
    {
    public string ValidUser { get; set; }
        public string IPAddress { get; set; }
        public string Country { get; set; }
        public Nullable<DateTime> UTCLoginTime { get; set; }
        public string SessionId { get; set; }
        public string LoginAttemptSuccess { get; set; }
        public string Username { get; set; }
        public string LoginFailureReason { get; set; }
        public string Name { get; set; }
        public Nullable<Decimal> LoginTimezone { get; set; }
        public Nullable<DateTime> GeographicLoginTime { get; set; }
        public Nullable<DateTime> PerthTime { get; set; }
        public string regionName { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
        public string latitute { get; set; }
        public string longitude { get; set; }
        public string timezone { get; set; }
        public Nullable<DateTime> UTCLogoutTime { get; set; }
        public Nullable<DateTime> PerthLogoutTime { get; set; }
        public Nullable<Decimal> LogoutTimeZone { get; set; }
        public string PerthTotalSessionTime { get; set; }


        }


}