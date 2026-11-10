using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scaffold.Controllers
{
    public class RequestedByController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        public ActionResult RequestedBy()
        {
            return View();
        }

        public ActionResult GetRequestByData(string id, string SiteID)
        {
            string query = "";
            query = "select RequestById,ClientId,SiteLocId,RequestBy,Status from TblRequestBy w where ClientID='" + databaseCon.Decrypt(id) + "' ";

            if (!String.IsNullOrEmpty(SiteID))
            {
                query = query + " and(SiteLocId = '" + SiteID + "')";
            }
            query = query + "order by RequestBy";
            var getData = dbContext.Database.SqlQuery<RequestByViewModel>(query).ToList();
            return Json(getData, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ddlSiteDropdown(string id)
        {
            string query = "select distinct SiteLocID,SiteLocation from tblSiteLocation where SiteLocation <>'' and SiteLocation is not null and  ClientId='" + HttpUtility.UrlEncode(databaseCon.Decrypt(id)) + "' order by SiteLocation";

            DataTable dt = databaseCon.GetData(query);

            var ddlSiteData = dbContext.Database.SqlQuery<FloorLevelddlViewModel>(query).ToList();
            return Json(ddlSiteData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveRequestedByData(int RequestedByID, string RequestedBy, string ddlStatus, string id, string UID, string ddlSitesLoc)
        {
            string Result = "";
            string Message = "";
            try
            {
                int clientId = Convert.ToInt32(databaseCon.Decrypt(id));
                int userId = Convert.ToInt32(databaseCon.Decrypt(UID));
                int siteLocId = Convert.ToInt32(ddlSitesLoc);
                bool status = ddlStatus == "1";
                string emailQuery = "SELECT EmailId FROM tblUser WHERE UserID=" + userId;
                DataTable dtEmail = databaseCon.GetData(emailQuery);
                string userEmail = dtEmail.Rows.Count > 0 ? dtEmail.Rows[0]["EmailId"].ToString() : "";

                if (string.IsNullOrEmpty(userEmail))
                {
                    return Json(new { Result = "Failed", Message = "User email not found." });
                }

                string duplicateQuery = "SELECT COUNT(*) FROM TblRequestBy WHERE ClientID=" + clientId + " AND SiteLocID=" + siteLocId + " AND RequestBy='" + RequestedBy.Replace("'", "''") + "'";

                if (RequestedByID > 0)
                {
                    duplicateQuery += " AND RequestById!=" + RequestedByID;
                }

                DataTable dtCheck = databaseCon.GetData(duplicateQuery);
                int exists = dtCheck.Rows.Count > 0 ? Convert.ToInt32(dtCheck.Rows[0][0]) : 0;

                if (exists > 0)
                {
                    return Json(new { Result = "Failed", Message = "Requested By already exists for this Site & Client." });
                }

                if (RequestedByID == 0)
                {
                    string insertQuery = "INSERT INTO TblRequestBy (ClientID,SiteLocID,RequestBy,Status,CreatedBy,CreatedOn) VALUES (" + clientId + "," + siteLocId + ",'" + RequestedBy.Replace("'", "''") + "'," + (status ? 1 : 0) + ",'" + userEmail.Replace("'", "''") + "',GETDATE())";
                    try
                    {
                        databaseCon.GetData(insertQuery);
                        Result = "Success";
                        Message = "Requested By added successfully.";
                    }
                    catch
                    {
                        Result = "Failed";
                        Message = "Failed to insert Requested By.";
                    }

                    return Json(new { Result = Result, Message = Message });
                }
                else
                {
                    string oldDataQuery = "SELECT RequestBy, Status FROM TblRequestBy WHERE RequestById=" + RequestedByID;
                    DataTable dtOld = databaseCon.GetData(oldDataQuery);

                    if (dtOld.Rows.Count == 0)
                    {
                        return Json(new { Result = "Failed", Message = "Requested By not found." });
                    }

                    string oldName = dtOld.Rows[0]["RequestBy"].ToString();
                    bool oldStatus = Convert.ToBoolean(dtOld.Rows[0]["Status"]);

                    bool isNameChanged = oldName != RequestedBy;
                    bool isStatusChanged = oldStatus != status;
                    if (isNameChanged || isStatusChanged)
                    {
                        string usageQuery = "SELECT COUNT(*) FROM tblRegister WHERE RequestedById=" + RequestedByID + " AND ClientID=" + clientId + " AND SiteLocID=" + siteLocId;
                        DataTable dtUsage = databaseCon.GetData(usageQuery);
                        int inUse = dtUsage.Rows.Count > 0 ? Convert.ToInt32(dtUsage.Rows[0][0]) : 0;

                        if (inUse > 0)
                        {
                            return Json(new { Result = "Failed", Message = "Cannot modify. Requested By is being used in Register." });
                        }
                    }
                    string updateQuery = "UPDATE TblRequestBy SET RequestBy='" + RequestedBy.Replace("'", "''") + "',Status=" + (status ? 1 : 0) + ",UpdatedBy='" + userEmail.Replace("'", "''") + "',UpdatedOn=GETDATE() WHERE RequestById=" + RequestedByID;

                    try
                    {
                        databaseCon.GetData(updateQuery);
                        return Json(new { Result = "Success", Message = "Requested By updated successfully." });
                    }
                    catch
                    {
                        return Json(new { Result = "Failed", Message = "Failed to update Requested By." });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Failed", Message = "Error: " + ex.Message });
            }
        }

        public ActionResult DeleteRequestedBy(string RequestedById, string SitelocId, string ClientId)
        {
            string Result = "";
            string Message = "";
            try
            {
                int RequestById = Convert.ToInt32(RequestedById);
                int SiteLocId = Convert.ToInt32(SitelocId);
                int Id = Convert.ToInt32(databaseCon.Decrypt(ClientId));
                string usageQuery = "SELECT COUNT(*) FROM tblRegister WHERE RequestedById=" + RequestById + " AND ClientID=" + Id + " AND SiteLocID=" + SiteLocId;
                DataTable dtUsage = databaseCon.GetData(usageQuery);
                int inUse = dtUsage.Rows.Count > 0 ? Convert.ToInt32(dtUsage.Rows[0][0]) : 0;

                if (inUse > 0)
                {
                    return Json(new { Result = "Failed", Message = "Cannot delete. Requested By is being used in Register." }, JsonRequestBehavior.AllowGet);
                }
                string deleteQuery = "DELETE FROM TblRequestBy WHERE RequestById=" + RequestById;

                try
                {
                    databaseCon.GetData(deleteQuery);
                    Result = "Success";
                    Message = "Requested By deleted successfully.";
                }
                catch (Exception ex)
                {
                    Result = "Failed";
                    Message = "Failed to delete Requested By. " + ex.Message;
                }
            }
            catch (Exception ex)
            {
                Result = "Failed";
                Message = "Error: " + ex.Message;
            }

            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }

    }
    public class RequestByViewModel
    {
        public int RequestById { get; set; }        
        public int ClientId { get; set; }
        public int SiteLocId { get; set; }
        public string RequestBy { get; set; }   
        public bool Status { get; set; }    
    }

}