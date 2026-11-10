using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scaffold.App_Code;

namespace Scaffold.Controllers
{
    public class ChangePasswordController : Controller
    {
        // GET: ChangePassword
        public ActionResult ChangePassword()
        {
            string query = "";
            query = "select EmailID from tbluser where EmailID= '" + databaseCon.Decrypt(Request.QueryString["ID"]) + "'";
          DataTable  dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                // txtUserName.Text = dt.Rows[0]["EmailID"].ToString();
                ViewBag.txtUserName = dt.Rows[0]["EmailID"].ToString();
            }
            return View();
        }
        public ActionResult btnLogin_Click(string txtnewpassword,String ID)
        {
            var Res = "";
            var Message = "";
            // if (txtnewpassword.Text.Length > 15 || txtnewpassword.Text.Length < 5)
            if (txtnewpassword.Length > 15 || txtnewpassword.Length < 5)
            {
                // ScriptManager.RegisterStartupScript(this, this.GetType(), "Open", "document.getElementById('passwordMsg').style.display='block';", true);
                Res = "Failed";
                Message = "Only 5 to 15 characters allowed for Password.";
            }
            else
            {
                // int result = databaseCon.RunCmd("Update tbluser set Password='" + databaseCon.Encrypt(txtnewpassword.Text.Replace("'", "''").Trim()) + "',UpdatedBy='" + databaseCon.Decrypt(Request.QueryString["ID"]) + "', UpdatedOn='" + DateTime.Now + "' where EmailID='" + databaseCon.Decrypt(Request.QueryString["ID"]) + "'");
                int result = databaseCon.RunCmd("Update tbluser set Password='" + databaseCon.Encrypt(txtnewpassword.Replace("'", "''").Trim()) + "',UpdatedBy='" + databaseCon.Decrypt(ID) + "', UpdatedOn='" + DateTime.Now + "' where EmailID='" + databaseCon.Decrypt(ID) + "'");
                if (result > 0)
                {
                    // ScriptManager.RegisterStartupScript(this, this.GetType(), "Open", "document.getElementById('modalAgency').style.display='block';", true);
                    Res = "Success";
                    Message = "Your password has been updated successfully";
                }
            }
            return Json(new {Result= Res,Message=Message }, JsonRequestBehavior.AllowGet);
        }
    }
}