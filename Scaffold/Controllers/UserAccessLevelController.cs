using Scaffold.App_Code;
using Scaffold_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scaffold.Controllers
{
    public class UserAccessLevelController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: UserAccessLevel
        public ActionResult UserAccessLevel(string UID, string UserRole, string id)
        {
            return View();
        }

        public ActionResult GetUserAccessLevelData(string id)
        {
            //Added CLientId condition to get record on the basis of different client as we do in INPEX (03-06-2023)
            //string query = "select Id,Pagename, ButtonName,buttonType ,HideFromRole,(select STUFF((SELECT distinct ',' + abc.RoleName FROM (select RoleName from splitstring(HideFromRole,',') join tblUserRole on Item=RoleNumber)abc FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)') ,1,1,'')) as Role from tblHideShow order by Pagename";
            string query = "select Id,Pagename, ButtonName,buttonType ,HideFromRole,(select STUFF((SELECT distinct ',' + abc.RoleName FROM (select RoleName from splitstring(HideFromRole,',') join tblUserRole on Item=RoleNumber)abc FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)') ,1,1,'')) as Role from tblHideShow where  ClientID = '" + databaseCon.Decrypt(id) + "' order by Pagename,ButtonName";
            var getData = dbContext.Database.SqlQuery<UserAccesslevelViewModel>(query).ToList();
            return Json(getData, JsonRequestBehavior.AllowGet);

            //DataTable dt = databaseCon.GetData(query);
            //GridUserAccessLevel.DataSource = dt;

        }

        public ActionResult ddlUserAccessLevel()
        {
            string query = "select RoleName,RoleNumber from tblUserRole where RoleName !='SuperAdmin' and RoleName!='Resource'";
            var getUserData = dbContext.Database.SqlQuery<UserddlViewModel>(query).ToList();
            return Json(getUserData, JsonRequestBehavior.AllowGet);
            // DataTable dt = databaseCon.GetData(query);
        }

        public ActionResult EditUserAccessData(string PageNameId, string HideFromRole, string id)
        {

            //if (e.CommandName == RadGrid.EditCommandName)
            //{
            //    GridUserAccessLevel.ShowHeader = false;
            //}
            //else
            //{
            //    GridUserAccessLevel.ShowHeader = true;
            //}
            //if (e.CommandName == RadGrid.EditCommandName)
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Open", "ActiveMultiSelect()", true);
            //}
            //if (e.CommandName == RadGrid.UpdateCommandName)
            //{

            //GridEditableItem item = (GridEditableItem)e.Item;
            //ListBox listhideRole = (ListBox)item.FindControl("listhideRole");
            //string Id = item.GetDataKeyValue("Id").ToString();
            string Result = "";
            string Message = "";
            //foreach (string ss in hideFromRole)
            //{

            //        GetRoleId = GetRoleId + "," + ss + ",";
            //        GetRoleId = GetRoleId.TrimEnd(',');
            //        GetRoleId = GetRoleId.TrimStart(',');

            //}
            if (HideFromRole == "" || HideFromRole == null)
            {
                string query = "Update tblHideShow set HideFromRole='" + HideFromRole + "' where Id='" + PageNameId + "' and ClientID = '" + databaseCon.Decrypt(id) + "'";
                int getdata = databaseCon.RunCmd(query);
                if (getdata > 0)
                {
                    Result = "Success";
                    Message = "Data Saved successfully.";
                }
                else
                {
                    Result = "Failed";
                    Message = "Syntax Error. Contact administrator ";
                }
            }
            
            //if(HideFromRole != "")
            if (HideFromRole != "" && HideFromRole != null)
            {

                var HideFromRoleValue = HideFromRole.Substring(0, HideFromRole.Length - 1);
                string query = "Update tblHideShow set HideFromRole='" + HideFromRoleValue + "' where Id='" + PageNameId + "' and ClientID = '" + databaseCon.Decrypt(id) + "'";
                int getdata = databaseCon.RunCmd(query);
                if (getdata > 0)
                {
                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Success ! </strong>Data Saved successfully.";
                    //GridUserAccessLevel.Rebind();
                    Result = "Success";
                    Message = "Data Saved successfully.";
                }
                else
                {
                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator ";
                    Result = "Failed";
                    Message = "Syntax Error. Contact administrator ";
                }
            }
            var response = new { Result = Result, Message = Message };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

    }

    public class UserAccesslevelViewModel
    {
        public Nullable<int> Id { get; set; }
        public string Pagename { get; set; }
        public string ButtonName { get; set; }
        public string buttonType { get; set; }
        public string HideFromRole { get; set; }
        public string Role { get; set; }
    }

    public class UserddlViewModel
    {
        public Nullable<int> RoleNumber { get; set; }
        public string RoleName { get; set; }
    }
}