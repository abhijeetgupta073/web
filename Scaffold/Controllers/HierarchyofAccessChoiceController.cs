using Scaffold.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scaffold_DataAccess;
using Scaffold.Models;
using System.Data;

namespace Scaffold.Controllers
{
    public class HierarchyofAccessChoiceController : Controller
    {
        ScaffoldEntities dbcontext = new ScaffoldEntities();
        // GET: HierarchyofAccessChoice
        public ActionResult HierarchyofAccessChoice(string id, string UID, string UserRole)
        {
            ViewBag.id = databaseCon.Decrypt(id);
            ViewBag.UID = databaseCon.Decrypt(UID);
            ViewBag.UserRole = databaseCon.Decrypt(UserRole);

            //string query = "select UserId from tblUser where IsInspector='true' and userid= '" + databaseCon.Decrypt(Request.QueryString["UID"]) + "' and clientid='" + databaseCon.Decrypt(Request.QueryString["id"]) + "'";
            string query = "select UserId,firstName+' '+LastName as Supervisor from tblUser join tblUserCategory on  tblUser.ClientId=tblUserCategory.ClientId and tblUser.CategoryId=tblUserCategory.UserCatId  where Category='Supervisor' and  userid= '" + databaseCon.Decrypt(Request.QueryString["UID"]) + "' and tbluser.clientid='" + databaseCon.Decrypt(Request.QueryString["id"]) + "'";
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["UserId"].ToString() != "0")
                {
                    ViewBag.ddlsupervisor = dt.Rows[0]["UserId"].ToString();
                }
            }
            return View();
        }

        public ActionResult ddldropdownbind(string id)
        {
            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select UserId,firstName+' '+LastName as Supervisor from tblUser join tblUserCategory on  tblUser.ClientId=tblUserCategory.ClientId and tblUser.CategoryId=tblUserCategory.UserCatId  where Category='Supervisor' and tbluser.ClientId='" + databaseCon.Decrypt(id) + "' order by Supervisor").ToList();
           
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getAccesschoice(string id,string RID) {
            string HierarchyLevel = "";
            string query = "select HierarchyLevel from tblRegister where  HierarchyLevel is not null and RegisterID = '" + databaseCon.Decrypt(RID) + "'";
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                HierarchyLevel = dt.Rows[0]["HierarchyLevel"].ToString();
            }
                return Json(HierarchyLevel, JsonRequestBehavior.AllowGet);
        }
        public ActionResult btnsave_Click(string AccessValue, string txt1, string txt2, string txt3, string txt4,string txt5, string ddlSupervisor,string RID )
        {
            var status = "";
            string queryupdate = "update tblRegister set HierarchyLevel='" + AccessValue + "', HierarchyComment1='" + txt1.Replace("'", "''").Trim() + "', HierarchyComment2='" + txt2.Replace("'", "''").Trim() + "',HierarchyComment3='" + txt3.Replace("'", "''").Trim() + "', HierarchyComment4='" + txt4.Replace("'", "''").Trim() + "', HierarchyComment5='" + txt5.Replace("'", "''").Trim() + "',	HierarchySignedOff='" + ddlSupervisor + "' where RegisterID='" + databaseCon.Decrypt(RID) + "'";
            int upresult = databaseCon.RunCmd(queryupdate);
            if (upresult > 0)
            {
                status = "Update";
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        public ActionResult btnyesCon_Click(string RID)
        {
            var status = "";
            string delete = "update tblRegister set  HierarchyLevel=null, HierarchyComment1=null, HierarchyComment2=null,HierarchyComment3=null, HierarchyComment4=null, HierarchyComment5=null,	HierarchySignedOff=null where RegisterID='" + databaseCon.Decrypt(RID) + "'";
            int dt = databaseCon.RunCmd(delete);
            if (dt > 0)
            {
                status = "Delete";
            }

            return Json(status, JsonRequestBehavior.AllowGet);
        }

    }
}