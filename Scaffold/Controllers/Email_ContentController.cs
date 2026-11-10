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
    public class Email_ContentController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: Email_Content
        public ActionResult Email_Content(string UID, string UserRole, string id)
        {
            return View();
        }

        public ActionResult GetEmailContentData(string id) 
        {
            string query = "";
            query = "exec [Pro_TblEmailContent_GET] ''," + databaseCon.Decrypt(id);
            var getData = dbContext.Database.SqlQuery<EmailContentViewModel>(query).ToList();
            return Json(getData, JsonRequestBehavior.AllowGet);
        }


        public ActionResult EditEmailContentData(string emailContentId, string purpose, string subject, string body, string id, string UID)
        {

            string query = "";
            DataTable dt;
            string Result = "";
            string Message = "";
            if (emailContentId == "0")
            {
                query = "exec Pro_TblEmailContent_SAVE '0' ,'" + purpose.Replace("'", "''").Trim() + "','" + subject.Replace("'", "''").Trim() + "','" + body.Replace("'", "''").Trim() + "','" + databaseCon.Decrypt(id) + "','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "'";

                dt = databaseCon.GetData(query);

                if (dt.Rows.Count != 0)
                {
                    if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                    {
                        Result = "Success";
                        Message = dt.Rows[0][1].ToString();
                    }
                    else if (Convert.ToInt32(dt.Rows[0][0]) == -1)
                    {
                        Result = "Failed";
                        Message = dt.Rows[0][1].ToString();
                    }
                    else
                    {
                        Result = "Failed";
                        Message = dt.Rows[0][1].ToString();
                    }
                }
                else
                {
                    Result = "Failed";
                    Message = "Syntax Error.Contact administrator ";
                }
            }
            else
            {
                query = "exec Pro_TblEmailContent_SAVE '" + emailContentId + "','" + purpose.Replace("'", "''").Trim() + "','" + subject.Replace("'", "''").Trim() + "','" + body.Replace("'", "''").Trim() + "','" + databaseCon.Decrypt(id) + "','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "'";
                dt = databaseCon.GetData(query);
                if (dt.Rows.Count != 0)
                {
                    if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                    {
                        Result = "Success";
                        Message = dt.Rows[0][1].ToString() + "";
                    }
                    else if (Convert.ToInt32(dt.Rows[0][0]) == -1)
                    {
                        Result = "Failed";
                        Message = dt.Rows[0][1].ToString();

                    }
                    else
                    {
                        Result = "Failed";
                        Message = dt.Rows[0][1].ToString();
                    }
                }
                else
                {
                    Result = "Failed";
                    Message = "Syntax Error.Contact administrator ";
                }

            }
            var response = new { Result = Result, Message = Message };
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DeleteDocument(string EmailContentId)
        {
            string Result = "";
            string Message = "";
            string query = "";
            DataTable dt;
            query = "exec Pro_TblEmailContent_DEL '" + EmailContentId + "'";

            dt = databaseCon.GetData(query);
            if (dt.Rows.Count != 0)
            {
                if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                {
                    Result = "Success";
                    Message = dt.Rows[0][1].ToString();
                }
                else if (Convert.ToInt32(dt.Rows[0][0]) == -1)
                {
                    Result = "Failed";
                    Message = dt.Rows[0][1].ToString();

                }
                else
                {
                    Result = "Failed";
                    Message = dt.Rows[0][1].ToString();
                }
            }
            else
            {
                Result = "Failed";
                Message = "Syntax Error. Contact administrator ";
            }
            var response = new { Result = Result, Message = Message };
            return Json(response, JsonRequestBehavior.AllowGet);

        }

    }

    public class EmailContentViewModel 
    {
        public Nullable<int> ID { get; set; }
        public string Purpose { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

    }
}