using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Scaffold.Controllers
{
    public class WorkGroupsController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: WorkGroups
        public ActionResult WorkGroups(string UID, string UserRole, string id)
        {
            return View();
        }
        public ActionResult ddlSiteDropdown(string id)
        {
            string query = "select distinct SiteLocID,SiteLocation from tblSiteLocation where SiteLocation <>'' and SiteLocation is not null and  ClientId='" + HttpUtility.UrlEncode(databaseCon.Decrypt(id)) + "' order by SiteLocation";

            DataTable dt = databaseCon.GetData(query);

            var ddlSiteData = dbContext.Database.SqlQuery<FloorLevelddlViewModel>(query).ToList();
            return Json(ddlSiteData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetWorkGroupData(string id, string SiteID)
        {
            string query = "";
            query = "select WorkGroupID,SiteLocID,ClientID,WorkGroup,Status from tblWorkGroup w where ClientID='" + databaseCon.Decrypt(id) + "' ";

            if (!String.IsNullOrEmpty(SiteID))
            {
                query = query + " and(SiteLocId = '" + SiteID + "')";
            }

            query = query + " order by WorkGroup";
            var getData = dbContext.Database.SqlQuery<WorkGroupsViewModel>(query).ToList();
            return Json(getData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSubGroupData(string id, string SiteID, string WorkGroupid)
        {
            string query = "";
            query = "select SubGroupId,WorkGroupID,SiteLocID,ClientID,SubGroupName,Status from TblSubGroups w where ClientID='" + databaseCon.Decrypt(id) + "' ";

            if (!String.IsNullOrEmpty(SiteID))
            {
                query = query + " and(SiteLocId = '" + SiteID + "')";
            }
            if (!String.IsNullOrEmpty(WorkGroupid))
            {
                query = query + " and(WorkGroupId = '" + WorkGroupid + "')";
            }
            query = query + " order by SubGroupName";
            var getData = dbContext.Database.SqlQuery<SubGroupsViewModel>(query).ToList();
            return Json(getData, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveWorkGroupData(int WorkGroupID, string WorkGroups, string ddlStatuss, string id, string UID, string ddlSitesLoc)
        {
            string Result = "";
            string Message = "";
            try
            {
                int clientId = Convert.ToInt32(databaseCon.Decrypt(id));
                int userId = Convert.ToInt32(databaseCon.Decrypt(UID));
                int siteLocId = Convert.ToInt32(ddlSitesLoc);
                bool status = ddlStatuss == "1";
                string emailQuery = "SELECT EmailId FROM tblUser WHERE UserID=" + userId;
                DataTable dtEmail = databaseCon.GetData(emailQuery);
                string userEmail = dtEmail.Rows.Count > 0 ? dtEmail.Rows[0]["EmailId"].ToString() : "";

                if (string.IsNullOrEmpty(userEmail))
                {
                    return Json(new { Result = "Failed", Message = "User email not found." });
                }

                string duplicateQuery = "SELECT COUNT(*) FROM tblWorkGroup WHERE ClientID=" + clientId + " AND SiteLocID=" + siteLocId + " AND WorkGroup='" + WorkGroups.Replace("'", "''") + "'";

                if (WorkGroupID > 0)
                {
                    duplicateQuery += " AND WorkGroupID!=" + WorkGroupID;
                }

                DataTable dtCheck = databaseCon.GetData(duplicateQuery);
                int exists = dtCheck.Rows.Count > 0 ? Convert.ToInt32(dtCheck.Rows[0][0]) : 0;

                if (exists > 0)
                {
                    return Json(new { Result = "Failed", Message = "Work Group already exists for this Site & Client." });
                }

                if (WorkGroupID == 0)
                {
                    string insertQuery = "INSERT INTO tblWorkGroup (ClientID,SiteLocID,WorkGroup,Status,CreatedBy,CreatedOn) VALUES (" + clientId + "," + siteLocId + ",'" + WorkGroups.Replace("'", "''") + "'," + (status ? 1 : 0) + ",'" + userEmail.Replace("'", "''") + "',GETDATE())";
                    try
                    {
                        databaseCon.GetData(insertQuery);
                        Result = "Success";
                        Message = "Work Group added successfully.";
                    }
                    catch
                    {
                        Result = "Failed";
                        Message = "Failed to insert Work Group.";
                    }

                    return Json(new { Result = Result, Message = Message });
                }
                else
                {
                    string oldDataQuery = "SELECT WorkGroup, Status FROM tblWorkGroup WHERE WorkGroupID=" + WorkGroupID;
                    DataTable dtOld = databaseCon.GetData(oldDataQuery);

                    if (dtOld.Rows.Count == 0)
                    {
                        return Json(new { Result = "Failed", Message = "Work Group not found." });
                    }

                    string oldName = dtOld.Rows[0]["WorkGroup"].ToString();
                    bool oldStatus = Convert.ToBoolean(dtOld.Rows[0]["Status"]);

                    bool isNameChanged = oldName != WorkGroups;
                    bool isStatusChanged = oldStatus != status;
                    if (isNameChanged || isStatusChanged)
                    {
                        string usageQuery = "SELECT COUNT(*) FROM tblRegister WHERE WorkGroupID=" + WorkGroupID + " AND ClientID=" + clientId + " AND SiteLocID=" + siteLocId;
                        DataTable dtUsage = databaseCon.GetData(usageQuery);
                        int inUse = dtUsage.Rows.Count > 0 ? Convert.ToInt32(dtUsage.Rows[0][0]) : 0;

                        if (inUse > 0)
                        {
                            return Json(new { Result = "Failed", Message = "Cannot modify. Work Group is being used in Register." });
                        }
                    }
                    string updateQuery = "UPDATE tblWorkGroup SET WorkGroup='" + WorkGroups.Replace("'", "''") + "',Status=" + (status ? 1 : 0) + ",UpdatedBy='" + userEmail.Replace("'", "''") + "',UpdatedOn=GETDATE() WHERE WorkGroupID=" + WorkGroupID;

                    try
                    {
                        databaseCon.GetData(updateQuery);
                        return Json(new { Result = "Success", Message = "Work Group updated successfully." });
                    }
                    catch
                    {
                        return Json(new { Result = "Failed", Message = "Failed to update Work Group." });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Failed", Message = "Error: " + ex.Message });
            }
        }

        public ActionResult DeleteWorkGroup(string WorkgroupId, string SitelocId, string ClientId)
        {
            string Result = "";
            string Message = "";

            try
            {
                int workGroupID = Convert.ToInt32(WorkgroupId);
                       int SiteLocId = Convert.ToInt32(SitelocId);
                        int Id = Convert.ToInt32(databaseCon.Decrypt(ClientId));

                string usageQuery = "SELECT COUNT(*) FROM tblRegister WHERE WorkGroupID=" + workGroupID + " AND ClientID=" + Id + " AND SiteLocID=" + SiteLocId;

                DataTable dtUsage = databaseCon.GetData(usageQuery);
                int inUse = Convert.ToInt32(dtUsage.Rows[0][0]);

                if (inUse > 0)
                {
                    return Json(new
                    {
                        Result = "Failed",
                        Message = "Cannot delete. Work Group is being used in Register."
                    }, JsonRequestBehavior.AllowGet);
                }

                string subGroupQuery = "SELECT COUNT(*) FROM TblSubGroups WHERE WorkGroupID = " + workGroupID;

                DataTable dtSubGroup = databaseCon.GetData(subGroupQuery);
                int subGroupCount = Convert.ToInt32(dtSubGroup.Rows[0][0]);

                if (subGroupCount > 0)
                {
                    return Json(new
                    {
                        Result = "Failed",
                        Message = "Cannot delete. Sub Groups exist under this Work Group."
                    }, JsonRequestBehavior.AllowGet);
                }

                string deleteQuery = "DELETE FROM tblWorkGroup WHERE WorkGroupID = " + workGroupID;

                databaseCon.GetData(deleteQuery);

                Result = "Success";
                Message = "Work Group deleted successfully.";
            }
            catch (Exception ex)
            {
                Result = "Failed";
                Message = "Error: " + ex.Message;
            }

            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveSubGroupData(int SubGroupID, string SubGroupName, int WorkGroupID, string Status, string id, string UID, string ddlSitesLoc)
        {
            string Result = "";
            string Message = "";
            try
            {
                int clientId = Convert.ToInt32(databaseCon.Decrypt(id));
                int userId = Convert.ToInt32(databaseCon.Decrypt(UID));
                int siteLocId = Convert.ToInt32(ddlSitesLoc);
                bool status = Status == "1";
                string emailQuery = "SELECT EmailId FROM tblUser WHERE UserID=" + userId;
                DataTable dtEmail = databaseCon.GetData(emailQuery);
                string userEmail = dtEmail.Rows.Count > 0 ? dtEmail.Rows[0]["EmailId"].ToString() : "";

                if (string.IsNullOrEmpty(userEmail))
                {
                    return Json(new { Result = "Failed", Message = "User email not found." });
                }
                string duplicateQuery = "SELECT COUNT(*) FROM TblSubGroups WHERE ClientID=" + clientId + " AND SiteLocID=" + siteLocId + " AND WorkGroupID=" + WorkGroupID + " AND SubGroupName='" + SubGroupName.Replace("'", "''") + "'";

                if (SubGroupID > 0)
                {
                    duplicateQuery += " AND SubGroupID!=" + SubGroupID;
                }

                DataTable dtCheck = databaseCon.GetData(duplicateQuery);
                int exists = dtCheck.Rows.Count > 0 ? Convert.ToInt32(dtCheck.Rows[0][0]) : 0;

                if (exists > 0)
                {
                    return Json(new { Result = "Failed", Message = "Sub Group already exists for this Work Group & Site & Client." });
                }

                if (SubGroupID == 0)
                {
                    string insertQuery = "INSERT INTO TblSubGroups (ClientID,SiteLocID,WorkGroupID,SubGroupName,Status,CreatedBy,CreatedOn) VALUES (" + clientId + "," + siteLocId + "," + WorkGroupID + ",'" + SubGroupName.Replace("'", "''") + "'," + (status ? 1 : 0) + ",'" + userEmail.Replace("'", "''") + "',GETDATE())";
                    try
                    {
                        databaseCon.GetData(insertQuery);
                        Result = "Success";
                        Message = "Sub Group added successfully.";
                    }
                    catch
                    {
                        Result = "Failed";
                        Message = "Failed to insert Sub Group.";
                    }

                    return Json(new { Result = Result, Message = Message });
                }
                else
                {
                    string oldDataQuery = "SELECT SubGroupName, Status FROM TblSubGroups WHERE SubGroupID=" + SubGroupID;
                    DataTable dtOld = databaseCon.GetData(oldDataQuery);

                    if (dtOld.Rows.Count == 0)
                    {
                        return Json(new { Result = "Failed", Message = "Sub Group not found." });
                    }

                    string oldName = dtOld.Rows[0]["SubGroupName"].ToString();
                    bool oldStatus = Convert.ToBoolean(dtOld.Rows[0]["Status"]);

                    bool isNameChanged = oldName != SubGroupName;
                    bool isStatusChanged = oldStatus != status;
                    if (isNameChanged || isStatusChanged)
                    {
                        string usageQuery = "SELECT COUNT(*) FROM tblRegister WHERE SubGroupID=" + SubGroupID + " AND ClientID=" + clientId + " AND SiteLocID=" + siteLocId;
                        DataTable dtUsage = databaseCon.GetData(usageQuery);
                        int inUse = dtUsage.Rows.Count > 0 ? Convert.ToInt32(dtUsage.Rows[0][0]) : 0;

                        if (inUse > 0)
                        {
                            return Json(new { Result = "Failed", Message = "Cannot modify. Sub Group is being used in Register." });
                        }
                    }
                    string updateQuery = "UPDATE TblSubGroups SET SubGroupName='" + SubGroupName.Replace("'", "''") + "',Status=" + (status ? 1 : 0) + ",UpdatedBy='" + userEmail.Replace("'", "''") + "',UpdatedOn=GETDATE() WHERE SubGroupID=" + SubGroupID;

                    try
                    {
                        databaseCon.GetData(updateQuery);
                        return Json(new { Result = "Success", Message = "Sub Group updated successfully." });
                    }
                    catch
                    {
                        return Json(new { Result = "Failed", Message = "Failed to update Sub Group." });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Failed", Message = "Error: " + ex.Message });
            }
        }
        [HttpPost]

        public ActionResult DeleteSubGroup(string SubGroupId, string SitelocId, string ClientId)
        {
            string Result = "";
            string Message = "";
            try
            {
                int subGroupID = Convert.ToInt32(SubGroupId);
                int SiteLocId = Convert.ToInt32(SitelocId);
                int Id = Convert.ToInt32(databaseCon.Decrypt(ClientId));
                string usageQuery = "SELECT COUNT(*) FROM tblRegister WHERE SubGroupId='" + subGroupID + "' AND ClientID=" + Id + " AND SiteLocID=" + SiteLocId;
                DataTable dtUsage = databaseCon.GetData(usageQuery);
                int inUse = dtUsage.Rows.Count > 0 ? Convert.ToInt32(dtUsage.Rows[0][0]) : 0;

                if (inUse > 0)
                {
                    return Json(new { Result = "Failed", Message = "Cannot delete. Sub Group is being used in Register." }, JsonRequestBehavior.AllowGet);
                }
                string deleteQuery = "DELETE FROM TblSubGroups WHERE SubGroupId=" + subGroupID + " AND ClientId=" + Id + " AND SiteLocId=" + SiteLocId;

                try
                {
                    databaseCon.GetData(deleteQuery);
                    Result = "Success";
                    Message = "Sub Group deleted successfully.";
                }
                catch (Exception ex)
                {
                    Result = "Failed";
                    Message = "Failed to delete Sub Group. " + ex.Message;
                }
            }
            catch (Exception ex)
            {
                Result = "Failed";
                Message = "Error: " + ex.Message;
            }

            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ExportWorkGroupAndSubGroupCSV(int siteLocId, string id)
        {
            try
            {
                var clientId = databaseCon.Decrypt(id);

                string query = "SELECT SG.SubGroupName, WG.WorkGroup, SG.Status, SG.SiteLocId, SG.ClientId, SG.WorkGroupId FROM TblSubGroups SG INNER JOIN TblWorkGroup WG ON SG.WorkGroupId = WG.WorkGroupId WHERE SG.SiteLocId=@SiteLocId AND SG.ClientId=@ClientId";
                var subGroupsData = dbContext.Database.SqlQuery<SubGroupExportModel>(query,
                    new SqlParameter("@SiteLocId", siteLocId),
                    new SqlParameter("@ClientId", clientId)).ToList();

                string workGroupsQuery = "SELECT WorkGroupId, WorkGroup, Status, SiteLocId, ClientId FROM TblWorkGroup WHERE SiteLocId=@SiteLocId AND ClientId=@ClientId";
                var workGroupsData = dbContext.Database.SqlQuery<WorkGroupExportModel>(workGroupsQuery,
                    new SqlParameter("@SiteLocId", siteLocId),
                    new SqlParameter("@ClientId", clientId)).ToList();

                var csv = new StringBuilder();
                csv.AppendLine("WorkGroups");
                csv.AppendLine("WorkGroup,Work Group Status");
                foreach (var item in workGroupsData)
                {
                    var statusText = item.Status ? "Active" : "Inactive";
                    csv.AppendLine($"{item.WorkGroup},{statusText}");
                }

                csv.AppendLine();
                csv.AppendLine();

                csv.AppendLine("SubGroups with WorkGroup");
                csv.AppendLine("WorkGroup,SubGroup,Sub Group Status");
                foreach (var item in subGroupsData)
                {
                    var statusText = item.Status ? "Active" : "Inactive";
                    csv.AppendLine($"{item.WorkGroup},{item.SubGroupName},{statusText}");
                }

                var fileName = "WorkGroup_SubGroup_Export.csv";
                return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", fileName);
            }
            catch (Exception ex)
            {
                return Content("Error: " + ex.Message);
            }
        }
    }
    public class SubGroupExportModel
    {
        public string SubGroupName { get; set; }
        public int WorkGroupId { get; set; }
        public string WorkGroup { get; set; }
        public bool Status { get; set; }
        public int SiteLocId { get; set; }
        public int ClientId { get; set; }
    }

    public class WorkGroupExportModel
    {
        public int WorkGroupId { get; set; }
        public string WorkGroup { get; set; }
        public bool Status { get; set; }
        public int SiteLocId { get; set; }
        public int ClientId { get; set; }
    }
    public class WorkGroupsViewModel
    {
        public int? WorkGroupID { get; set; }
        public int ClientID { get; set; }
        public int SiteLocID { get; set; }
        public string WorkGroup { get; set; }
        public bool Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

    }
    public class SubGroupsViewModel
    {
        public int? SubGroupId { get; set; }
        public int WorkGroupID { get; set; }
        public int ClientID { get; set; }
        public int SiteLocID { get; set; }
        public string SubGroupName { get; set; }
        public bool Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

    }
}