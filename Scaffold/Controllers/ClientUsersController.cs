using Scaffold.App_Code;
using Scaffold_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;


namespace Scaffold.Controllers
{
    public class ClientUsersController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: ClientUsers
        public ActionResult ClientUsers(string UID, string UserRole, string id)
        {
            return View();
        }

        public ActionResult GetClientUserData(string id)
        {
            string query;
            query = "exec Pro_TBLUSER_GET  '" + databaseCon.Decrypt(id) + "',''";

            var getData = dbContext.Database.SqlQuery<ClientUserViewModel>(query).ToList();
            return Json(getData, JsonRequestBehavior.AllowGet);
            //if (dt.Rows.Count < 1)
            //{
            //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "hide", "<script>filterhide();</script>", false);
            //}
            //GridClientUserList.DataSource = dt;
        }

        public ActionResult EncrptyClientUserID(string clientUserId)
        {
            var encptyClientUserID = databaseCon.Encrypt(clientUserId);
            return Json(encptyClientUserID, JsonRequestBehavior.AllowGet);
        }



        public ActionResult btnConfirmDeleteYes_Click(string UserID)
        {
            string query;
            DataTable dt;
            var Result = "";
            var Message = "";
            query = "exec Pro_TBLUSER_DEL '" + UserID + "'";
            dt = databaseCon.GetData(query);
            if (dt.Rows.Count != 0)
            {
                if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                {

                    //Session["Save"] = dt.Rows[0][1].ToString();
                    //Response.Redirect("ClientUsers.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"]);
                    Result = "Success";
                    Message = dt.Rows[0][1].ToString();

                }
                else if (Convert.ToInt32(dt.Rows[0][0]) == -1)
                {
                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                    Result = "Failed";
                    Message = dt.Rows[0][1].ToString();
                }
                else
                {
                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                    Result = "Failed";
                    Message = dt.Rows[0][1].ToString();
                }
            }
            else
            {
                //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                //art1.Style.Add("display", "block");
                //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator ";
                Result = "Failed";
                Message = "Syntax Error. Contact administrator .";
            }
            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
            // PopupDelete.Style.Add("display", "none");
        }
    }
    public class ClientUserViewModel 
    {
    public Nullable<int> UserId { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string Category { get; set; }
        public string CellPhone { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string Role { get; set; }
        public string EmailID { get; set; }
        public string Status { get; set; }
        public string Password { get; set; }
    }

}