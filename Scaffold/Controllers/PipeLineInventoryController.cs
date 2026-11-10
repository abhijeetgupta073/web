
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
    public class PipeLineInventoryController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: PipeLineInventory
        public ActionResult PipeLineInventory(string id, string UID, string UserRole)
        {
            string query1 = "select SiteOwnerName from tblUser where ClientId='" + databaseCon.Decrypt(id) + "' and UserId='" + databaseCon.Decrypt(UID) + "'";
            return View();
        }
        public ActionResult GetPipeLineInventoryData(string ClientID, string SiteId, string JobID, string PipelineDate)
        {
            string addquery = "";
            //string query = "";
            if ((SiteId != "" && SiteId != "All") && SiteId != null)
            {
                addquery = " and SiteId='" + SiteId + "'";

            }
            if (JobID != "" && JobID != "All" && JobID != null)
            {
                addquery = addquery + " and JobID=" + JobID;
            }
            if (PipelineDate != "" && PipelineDate != "All" && PipelineDate != null)
            {
                addquery = addquery + " and PipelineStartDate='" + PipelineDate + "'";
            }
            //    var PipeLineInventoryData = dbContext.Database.SqlQuery<PipelineInventoryViewModel>("select RIGHT('000000'+CONVERT(varchar,ScaffRef),6) as  ScaffRef,SiteLocation,Vw_ThirdPartyInventory.MaterialType,Vw_ThirdPartyInventory.ItemCode,Vw_ThirdPartyInventory.ItemDesc, PipelineStartDate,PipeLineNote,Quantity, ItemWeight, (Quantity * ItemWeight) as TotalWeight  from tblPipelineRequest join Vw_ThirdPartyInventory on tblPipelineRequest.InventoryID = Vw_ThirdPartyInventory.InventoryId join tblSiteLocation on tblSiteLocation.SiteLocID = tblPipelineRequest.siteId Join tblRegister on tblRegister.RegisterId = tblPipelineRequest.JobId where 1 = 1 and Vw_ThirdPartyInventory.clientid = '" + databaseCon.Decrypt(ClientID) + "' " + addquery + " order by MaterialType, ItemCode").ToList();

            string query = "select SiteLocation, Vw_ThirdPartyInventory.MaterialType , Vw_ThirdPartyInventory.ItemCode, Vw_ThirdPartyInventory.ItemDesc, sum(Quantity) as Quantity, ItemWeight, (sum(Quantity) * ItemWeight) as TotalWeight from tblPipelineRequest join Vw_ThirdPartyInventory on tblPipelineRequest.InventoryID = Vw_ThirdPartyInventory.InventoryId join tblSiteLocation on tblSiteLocation.SiteLocID = tblPipelineRequest.siteId Join tblRegister on tblRegister.RegisterId = tblPipelineRequest.JobId where 1 = 1 and (DateTagged = '01-01-1900' or DateTagged is null) and Vw_ThirdPartyInventory.clientid = '" + databaseCon.Decrypt(ClientID) + "' " + addquery + "  group by Vw_ThirdPartyInventory.ItemCode, tblSiteLocation.SiteLocation, Vw_ThirdPartyInventory.MaterialType, Vw_ThirdPartyInventory.ItemDesc, Vw_ThirdPartyInventory.ItemWeight ,isnull(DisplaySortOrder,9999999) order by MaterialType, isnull(DisplaySortOrder,9999999), ItemCode";

            var PipeLineInventoryData = dbContext.Database.SqlQuery<PipelineInventoryViewModel>(query).ToList();


            return Json(PipeLineInventoryData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SiteDropDown(string id, string UID)
        {
            var SiteOwnerName = "";
            string query1 = "select SiteOwnerName from tblUser where ClientId='" + databaseCon.Decrypt(id) + "' and UserId='" + databaseCon.Decrypt(UID) + "'";
            DataTable dt1 = databaseCon.GetData(query1);
            if (dt1.Rows.Count > 0)
            {
                SiteOwnerName = dt1.Rows[0]["SiteOwnerName"].ToString();
            }
            if (SiteOwnerName != "")
            {
                // Escape single quote by replacing ' with ''
                SiteOwnerName = SiteOwnerName.Replace("'", "''");
                var getSiteLocationData = dbContext.Database.SqlQuery<SiteLocationViewModel>("select distinct SiteLocation,SiteLocID from tblSiteLocation where SiteLocation <>'' and SiteLocation is not null and  ClientId='" + databaseCon.Decrypt(id) + "' and OwnerName='" + SiteOwnerName + "' and MaintainInventory = 1 and MaintainInventoryAtJob=1  order by SiteLocation").ToList();
                return Json(getSiteLocationData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var getSiteLocationData = dbContext.Database.SqlQuery<SiteLocationViewModel>("select * from (select 0 as SiteLocID,'All' as SiteLocation union select distinct SiteLocID,SiteLocation from tblSiteLocation where SiteLocation <>'' and SiteLocation is not null and  ClientId = '" + databaseCon.Decrypt(id) + "' and MaintainInventory = 1 ) as sitelisting order by SiteLocation").ToList();
                return Json(getSiteLocationData, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetJobDropDownBind(string id, string SiteId)
        {
            //var getJobData = dbContext.Database.SqlQuery<PipelineInventoryViewModel>("select Distinct Registerid,Scaffref from tblRegister where ClientId='" + databaseCon.Decrypt(id) + "' and SiteLocId='" + SiteId + "' and PipelineStartDate is not null order by Scaffref").ToList();

            //var getJobData = dbContext.Database.SqlQuery<PipelineInventoryViewModel>("select Distinct RegisterId, RIGHT('000000'+CONVERT(varchar,ScaffRef),6) as ScaffRef from tblRegister, tblPipelineRequest where tblRegister.registerid = tblPipelineRequest.jobid and  tblPipelineRequest.ClientId = '" + databaseCon.Decrypt(id) + "' and SiteLocId = '" + SiteId + "' order by ScaffRef").ToList();

            var getJobData = dbContext.Database.SqlQuery<PipelineInventoryViewModel>("select Distinct RegisterId, RIGHT('000000'+CONVERT(varchar,ScaffRef),6) as ScaffRef from tblRegister, tblPipelineRequest where tblRegister.registerid = tblPipelineRequest.jobid and (DateTagged = '01-01-1900' or DateTagged is null) and tblPipelineRequest.ClientId = '" + databaseCon.Decrypt(id) + "' and SiteLocId = '" + SiteId + "' order by ScaffRef").ToList();


            return Json(getJobData, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetMaterialDropDown(string id, string SiteId, string JobId, string UserRole)
        {
            var getMaterialData = dbContext.Database.SqlQuery<PipelineInventoryViewModel>("select Distinct MaterialType from tblPipelineRequest where ClientId='" + databaseCon.Decrypt(id) + "' and siteId='" + SiteId + "' order by MaterialType").ToList();
            return Json(getMaterialData, JsonRequestBehavior.AllowGet);

        }

        public ActionResult AddNewRequest(string id, string UID, string SiteId, string JobId, string UserRole)
        {
            string query3 = "exec Pro_PipelineInventoryAddRequest '" + databaseCon.Decrypt(UID) + "','" + databaseCon.Decrypt(id) + "','" + SiteId + "','" + JobId + "'";
            int execpro = databaseCon.RunCmdID(query3);
            return Json(query3, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTotalTotalTonnage(string ClientID, string SiteId, string JobId)
        {
            var TotalItemsValues = "";
            var TotalTonnageValues = "";
            var NoOfJobs = "";

            //(13/06/2022)
            //string query = "select ISNULL(FORMAT(CONVERT(int,(Sum(Quantity))),'N0'),0) as TotalItems,convert(decimal(18,2),ISNULL(Sum(Quantity * ItemWeight)/1000,0)) as TotalTonnage from tblPipelineRequest join Vw_ThirdPartyInventory on tblPipelineRequest.InventoryID = Vw_ThirdPartyInventory.InventoryId where tblPipelineRequest.ClientID = " + databaseCon.Decrypt(ClientID) + " and (0='" + (SiteId) + "' or Siteid='" + (SiteId) + "') and (0='" + (JobId) + "' or JobId='" + (JobId) + "')";

            string query = "select ISNULL(FORMAT(CONVERT(int,(Sum(Quantity))),'N0'),0) as TotalItems,convert(decimal(18,2),ISNULL(Sum(Quantity * ItemWeight)/1000,0)) as TotalTonnage from tblPipelineRequest join Vw_ThirdPartyInventory on tblPipelineRequest.InventoryID = Vw_ThirdPartyInventory.InventoryId Join tblRegister on tblRegister.RegisterId = tblPipelineRequest.JobId where tblPipelineRequest.ClientID = " + databaseCon.Decrypt(ClientID) + " and (DateTagged = '01-01-1900' or DateTagged is null) and (0='" + (SiteId) + "' or Siteid='" + (SiteId) + "') and (0='" + (JobId) + "' or JobId='" + (JobId) + "')";


            DataTable dt = databaseCon.GetData(query);
            //Added on 24-Feb-2022 start here

            string query1 = "Select Count(registerId) As PipelineRegisterCount  from Vw_tblRegister where (PipelineStartDate != '1900/01/01' or PipelineStartDate != null or PipelineStartDate != '') and (DateTagged = '01-01-1900' or DateTagged is null) and (ClientID = " + databaseCon.Decrypt(ClientID) + ")";
            DataTable dt1 = databaseCon.GetData(query1);

            if (dt1.Rows.Count > 0) {
                NoOfJobs = dt1.Rows[0]["PipelineRegisterCount"].ToString();
            }
            //Added on 24-Feb-2022 end here
            if (dt.Rows.Count > 0)
            {
                TotalItemsValues = dt.Rows[0]["TotalItems"].ToString();
                TotalTonnageValues = dt.Rows[0]["TotalTonnage"].ToString();
            }

            //var Result = "TotalItemsValues" + TotalItemsValues + "|" + "TotalTonnageValues" + TotalTonnageValues;
            var Result = "TotalItemsValues" + TotalItemsValues + "|" + "TotalTonnageValues" + TotalTonnageValues + "|" + "NoOfJobs" + NoOfJobs;
            return Json(new { TotalItemsValues = TotalItemsValues, TotalTonnageValues = TotalTonnageValues, NoOfJobs = NoOfJobs });
        }

        public ActionResult GetPipelineDateData(string id, string SiteId, string JobID)
        {
            string addquery = "";
            if ((SiteId != "" && SiteId != "All") && SiteId != null)
            {
                addquery = " and SiteLocId ='" + SiteId + "'";
            }
            if (JobID != "" && JobID != "All" && JobID != null)
            {
                addquery = addquery + " and JobID=" + JobID;
            }


            var getPipelineDate = dbContext.Database.SqlQuery<PipelineInventoryViewModel>("select Distinct PipelineStartDate from tblRegister where PipelineStartDate is not null and (DateTagged = '01-01-1900' or DateTagged is null) and ClientID='" + databaseCon.Decrypt(id) + "' " + addquery + " order by PipelineStartDate").ToList();
            return Json(getPipelineDate, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetPipelineStartDate(string ClientID, string SiteId, string JobId)
        {
            var PipelineDateValues = "";
            var pipelineNotesValues = "";
            var PipelineDesc = "";

            string query = "Select distinct case when  Convert(varchar,PipelineStartDate,103)='01/01/1900' then ''else Convert(varchar, PipelineStartDate, 103) end as PipelineStartDate ,PipeLineNote,PipeLineDesc  from tblRegister Join tblPipelineRequest on tblRegister.RegisterId = tblPipelineRequest.jobid where  SiteLocId = '" + SiteId + "' and RegisterId = '" + JobId + "'";
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                PipelineDateValues = dt.Rows[0]["PipelineStartDate"].ToString();
                pipelineNotesValues = dt.Rows[0]["PipeLineNote"].ToString();
                PipelineDesc = dt.Rows[0]["PipeLineDesc"].ToString();
            }

            var Result = "TotalItemsValues" + PipelineDateValues + "|" + "PipelineDateValues" + PipelineDateValues + "|" + "PipelineDesc" + PipelineDesc;
            return Json(new { TotalItemsValues = PipelineDateValues, PipelineDateValues = pipelineNotesValues, PipelineDesc = PipelineDesc });
        }

    }
}


public class PipelineInventoryViewModel
{
    public string ScaffRef { get; set; }
    public string MaterialType { get; set; }
    public string ItemCode { get; set; }
    public string SiteLocation { get; set; }
    public string ItemDesc { get; set; }
    public Nullable<double> ItemWeight { get; set; }
    public Nullable<double> TotalWeight { get; set; }
    public string PipeLineNote { get; set; }
    public Nullable<System.DateTime> PipelineStartDate { get; set; }
    public Nullable<int> Quantity { get; set; }
    public Nullable<int> JobId { get; set; }
    public string Description { get; set; }
    public Nullable<int> RegisterId { get; set; }
}