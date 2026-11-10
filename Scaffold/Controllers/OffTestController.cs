using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scaffold.Controllers
{
    public class OffTestController : Controller
    {
        ScaffoldEntities dbcontext = new ScaffoldEntities();
        // GET: OffTest
        public ActionResult OffTest()
        {
            string query = "select distinct UserId from tblUser where IsInspector='true' and userid= '" + databaseCon.Decrypt(Request.QueryString["UID"]) + "' and clientid='" + databaseCon.Decrypt(Request.QueryString["id"]) + "'";
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["UserId"].ToString() != "0")
                {
                    ViewBag.ddlddlInspectedByadd = dt.Rows[0]["UserId"].ToString();
                }
            }
            return View();
        }

        public ActionResult GetOffSystemInspectionData(string id, string SiteID, string LocationID, string ddlSubOwner, string hdnTimeOffSet, string[] ddlBuilding, string FloorLevel, string WorkGroup, string Bearing)
        {
            string buildingValues = ddlBuilding != null ? string.Join(",", ddlBuilding) : "";
            string query = "exec [Pro_TBLREGISTER_GET] '','" + databaseCon.Decrypt(id);

            if (SiteID != "" && SiteID != null)
            {
                query = query + "','" + SiteID + "";

            }
            else
            {
                query = query + "','";

            }

            if (LocationID != "" && LocationID != null)
            {
                query = query + "','" + LocationID + "";
            }

            else
            {
                query = query + "','";
            }

            DataTable dt = databaseCon.GetData(query);

            var GetData = dbcontext.Database.SqlQuery<Pro_TBLREGISTER_GETViewModel>(query).ToList();
            var jsonResult = Json(GetData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult GoOfflineClick(string id, string UserId, string Siteid)
        {

            string checkqueryForthisClienthaveAnyRegisterOnOffline = "select  SiteLocation , count('*')  as NoOfJobOffline from VW_TblRegister where IsOfflineCurrentForInspection = 1 and ClientID =" + databaseCon.Decrypt(id) + " group by SiteLocation having count('*') >1 ";
            var VarCheckqueryForthisClienthaveAnyRegisterOnOffline = databaseCon.GetData(checkqueryForthisClienthaveAnyRegisterOnOffline);

            string checkquery = "select  count('x') from tblregister where IsOfflineCurrentForInspection =1 and clientid  = " + databaseCon.Decrypt(id) + " and SiteLocId = " + databaseCon.Decrypt(Siteid) + " and DateOfDismantle = '1900-01-01'  and DateTagged != '1900-01-01'";

            var CheckOfflinereg = databaseCon.GetData(checkquery);

            var co = CheckOfflinereg.Rows[0][0].ToString();
            if (int.Parse(co) <= 0)
            {
                if (VarCheckqueryForthisClienthaveAnyRegisterOnOffline.Rows.Count <= 0)
                {
                    string Userinfor = "select  LoginId from tblUser where UserId = " + databaseCon.Decrypt(UserId) + "";
                    DataTable Confirmdt = databaseCon.GetData(Userinfor);

                    var loginuserid = Confirmdt.Rows[0][0].ToString();

                    string query = "update tblRegister set IsOfflineCurrentForInspection = 1 ,GoOfflineOn = GETDATE(), GoOfflineBy = '" + loginuserid + "' where  SiteLocId = " + databaseCon.Decrypt(Siteid) + " and ClientID =  " + databaseCon.Decrypt(id) + " and DateOfDismantle = '1900-01-01'  and DateTagged != '1900-01-01'";
                    var res = databaseCon.RunCmd(query);
                    res = Convert.ToInt32(res);
                    return Json(new { Result = "Success", Message = "Your data uploaded successfully for offline mode " }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = "FailedToGoOffline", Message = "Data already in offline Mode for " + VarCheckqueryForthisClienthaveAnyRegisterOnOffline.Rows[0][0].ToString() + " Site Location" }, JsonRequestBehavior.AllowGet);
                }
            }

            else
            {
                return Json(new { Result = "Failed", Message = "Data already in offline" }, JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult CheckOffTestDataForOffline(string id, string Siteid)
        {

            var Result = "";
            var Message = "";
            string checkqueryForSiteDataisOffline = "select  count('x') from tblregister where IsOfflineCurrentForInspection =1 and clientid  = " + databaseCon.Decrypt(id) + " and SiteLocId = " + Siteid + "";

            var CheckOfflinereg = databaseCon.GetData(checkqueryForSiteDataisOffline);


            var co = CheckOfflinereg.Rows[0][0].ToString();

            if (int.Parse(co) <= 0)
            {

                return Json(new { Checked = false });
            }
            else
            {
                string FinddataQuery = "select  distinct GoOfflineBy ,GoOfflineOn from tblregister where IsOfflineCurrentForInspection =1 and clientid  = " + databaseCon.Decrypt(id) + " and SiteLocId = " + Siteid + "";
                var FinddataUpdatedBy = databaseCon.GetData(FinddataQuery);

                var offlineBy = FinddataUpdatedBy.Rows[0][0].ToString();
                var offlineon = FinddataUpdatedBy.Rows[0][1].ToString();

                DateTime offlineonDateTime = DateTime.Parse(offlineon);
                string formattedOfflineon = offlineonDateTime.ToString("dd-MMM-yyyy h:mm:ss tt");

                Result = "Failed";
                Message = "You cannot save the inspection because the site data is on offline mode by " + offlineBy + " since " + formattedOfflineon + "";


                return Json(new { Checked = true, Result = Result, Message = Message });

            }
        }



    }
}