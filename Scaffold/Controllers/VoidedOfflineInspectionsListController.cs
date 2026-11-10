using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;
using System.Data;

namespace Scaffold.Controllers
{

    public class VoidedOfflineInspectionsListController : Controller
    {

        ScaffoldEntities dbcontext = new ScaffoldEntities();
        // GET: VoidedOfflineInspectionsList
        public ActionResult VoidedOfflineInspectionsList()

        {
            string query = "select distinct UserId from tblUser where IsInspector='true' and userid= '" + databaseCon.Decrypt(Request.QueryString["UID"]) + "' and clientid='" + databaseCon.Decrypt(Request.QueryString["id"]) + "'";
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["UserId"].ToString() != "0")
                {
                    //  ddlddlInspectedByadd.SelectedValue = dt.Rows[0]["UserId"].ToString();
                    ViewBag.ddlddlInspectedByadd = dt.Rows[0]["UserId"].ToString();
                }
            }

            return View();
        }
        public ActionResult BindallDropdown(string id)
        {

            var ddlCreatedBy = dbcontext.Database.SqlQuery<DropDownViewModel>("select distinct concat(FirstName,' ',LastName) CreatedBy  from VW_TblRegister join [TblInspection_NotUploadedTocloud] OI on VW_TblRegister.RegisterId = OI.RegisterId join tbluser on TblUser.emailid = OI.CreatedBy where((tbluser.clientid = VW_TblRegister.ClientId) or(role = 'superadmin'))and VW_TblRegister.ClientId  = '" + databaseCon.Decrypt(id) + "' order by concat(FirstName, ' ', LastName)").ToList();

            var ddlCreatedOn = dbcontext.Database.SqlQuery<DropDownViewModel>("select distinct convert(nvarchar(20),convert(date,OI.createdon)) CreatedOn from  TblInspection_NotUploadedTocloud OI  join tblregister on OI.registerid=tblregister.registerid  where tblregister.ClientId='" + databaseCon.Decrypt(id) + "' Order by convert(nvarchar(20),convert(date,OI.createdon)) desc").ToList();


            return Json(new { ddlCreatedBy = ddlCreatedBy, ddlCreatedOn = ddlCreatedOn }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetVoidedOfflineInspectionsListData(string id , string CreatedBy , String CreatedOn)
        {

            string query = "exec [Pro_GetFromOfflineInspection_NotUploadedTocloud] " + databaseCon.Decrypt(id)+",'"+ CreatedBy + "','" + CreatedOn + "' ";

            DataTable dt = databaseCon.GetData(query);

            var GetData = dbcontext.Database.SqlQuery<Pro_TBLREGISTER_GETViewModel>(query).ToList();
            //Commented on 26-07-2023 to avoid large data problem.
            //return Json(GetData, JsonRequestBehavior.AllowGet);
            var jsonResult = Json(GetData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public ActionResult dropdownCreatedOn(string id, string CreatedBy)
        {
            string query;
      
            query = "select distinct convert(nvarchar(20),convert(date,OI.createdon)) CreatedOn from  TblInspection_NotUploadedTocloud OI  join tblregister on OI.registerid=tblregister.registerid  join tbluser on TblUser.emailid=OI.CreatedBy  where ( ( tbluser.clientid=  tblregister.ClientId) or ( role='superadmin') )and tblregister.ClientId='" + databaseCon.Decrypt(id) + "'";
     
            if (CreatedBy != "" && CreatedBy != null)
            {
                query = query + "  and('All'='" + CreatedBy + "' or concat(FirstName,' ',LastName) = '" + CreatedBy + "')";
            }
            query = query + " Order by convert(nvarchar(20),convert(date,OI.createdon)) desc; ";

            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>(query);

            return Json(GetData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckIsInspectionDataForOffline(string id, string Siteid)
        {

            var Result = "";
            var Message = "";
            string checkqueryForSiteDataisOffline = "select  count('x') from tblregister where IsOfflineCurrentForInspection =1 and clientid  = " + databaseCon.Decrypt(id) + "";

            var CheckOfflinereg = databaseCon.GetData(checkqueryForSiteDataisOffline);


            var co = CheckOfflinereg.Rows[0][0].ToString();

            if (int.Parse(co) <= 0)
            {

                return Json(new { Checked = false });
            }
            else
            {
                string FinddataQuery = "select  distinct userid,GoOfflineBy ,GoOfflineOn from tblregister left join tbluser on GoOfflineBy=EmailID where IsOfflineCurrentForInspection =1 and tblregister.clientid  = " + databaseCon.Decrypt(id) + " and SiteLocId = " + Siteid + "";
                var FinddataUpdatedBy = databaseCon.GetData(FinddataQuery);

                var userId = FinddataUpdatedBy.Rows[0][0].ToString();
                var offlineBy = FinddataUpdatedBy.Rows[0][1].ToString();
                var offlineon = FinddataUpdatedBy.Rows[0][2].ToString();

                DateTime offlineonDateTime = DateTime.Parse(offlineon);
                string formattedOfflineon = offlineonDateTime.ToString("dd-MMM-yyyy h:mm:ss tt");

                Result = "Failed";
                Message = "You cannot save the inspection because the site data is on offline mode by " + offlineBy + " since " + formattedOfflineon + "";
                TempData["offlineBy"] = offlineBy.ToString();
                TempData["userId"] = userId.ToString();

                return Json(new { Checked = true, Result = Result, Message = Message });

            }


        }


    }
}