using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Scaffold.Controllers
{
    public class ReturnInventoryController : Controller
    {
        // GET: ReturnInventory
        ScaffoldEntities dbContext = new ScaffoldEntities();
        public ActionResult ReturnInventory()
        {
            return View();
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
                var getSiteLocationData = dbContext.Database.SqlQuery<SiteLocationViewModel>("select distinct SiteLocID,SiteLocation from tblSiteLocation where SiteLocation <> '' and SiteLocation is not null and ClientId = '" + databaseCon.Decrypt(id) + "' and OwnerName='" + SiteOwnerName + "' and MaintainInventory = 1 order by SiteLocation").ToList();
                return Json(getSiteLocationData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //var getSiteLocationData = dbContext.Database.SqlQuery<SiteLocationViewModel>("select distinct SiteLocID,SiteLocation from tblSiteLocation where SiteLocation <> '' and SiteLocation is not null and ClientId = '" + databaseCon.Decrypt(id) + "' and MaintainInventory = 1 order by SiteLocation").ToList();
                //return Json(getSiteLocationData, JsonRequestBehavior.AllowGet);
                var getSiteLocationData = dbContext.Database.SqlQuery<SiteLocationViewModel>("select * from (select 0 as SiteLocID,'All' as SiteLocation union select distinct SiteLocID,SiteLocation from tblSiteLocation where SiteLocation <> '' and SiteLocation is not null and ClientId = '" + databaseCon.Decrypt(id) + "' and MaintainInventory = 1) as sitelisting order by SiteLocation").ToList();
                return Json(getSiteLocationData, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult SitetoJobDropDown(string id, string UID)
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
                //var getSiteLocationData = dbContext.Database.SqlQuery<SiteLocationViewModel>("select distinct SiteLocation,SiteLocID from tblSiteLocation where SiteLocation <>'' and SiteLocation is not null and  ClientId='" + databaseCon.Decrypt(id) + "' and MaintainInventory = 1 and MaintainInventoryAtJob=1  order by SiteLocation").ToList();
                var getSiteLocationData = dbContext.Database.SqlQuery<SiteLocationViewModel>("select * from (select 0 as SiteLocID,'All' as SiteLocation union select distinct SiteLocID,SiteLocation from tblSiteLocation where SiteLocation <>'' and SiteLocation is not null and  ClientId = '" + databaseCon.Decrypt(id) + "' and MaintainInventory = 1 ) as sitelisting order by SiteLocation").ToList();
                return Json(getSiteLocationData, JsonRequestBehavior.AllowGet);
            }
        }

        public class JobRegisterViewModel
        {
            public Nullable<int> RegisterId { get; set; }
            public string Description { get; set; }
            public string ScaffRef { get; set; }
        }
        public ActionResult GetJobDropDownBind(string id, string JobsiteID)
        {
            var getJobData = dbContext.Database.SqlQuery<JobRegisterViewModel>("select RegisterId,Description,RIGHT('000000'+CONVERT(varchar,ScaffRef),6) as ScaffRef from tblRegister where SiteLocID='" + JobsiteID + "'and ClientId='" + databaseCon.Decrypt(id) + "' order by RegisterId").ToList();
            return Json(getJobData, JsonRequestBehavior.AllowGet);

        }

        //public ActionResult GetReturnInventoryData(string id, string ddlfrom, string ddl1to, string ddl1from, string rdodepottosite, string rdositetojob, string rdomaindepottodepot)
        //{

        //    //var RequestInventoryData = dbContext.Database.SqlQuery<ReturnInventoryViewModel>("select FromId,ToId,ReturnInvId,SiteLocation as FromName,[To] as ToName,ReturnDoneBy,Replace(convert(nvarchar,convert(datetime,ReturnDoneOn,120),106),' ','-') as ReturnDoneOn,ReturnStatus from tblReturnInventory join tblSiteLocation on tblReturnInventory.FromId=tblSiteLocation.SiteLocID where [From]='Site'and tblReturnInventory.ClientId=153 order by ReturnInvId Desc").ToList();
        //    //  return Json(RequestInventoryData, JsonRequestBehavior.AllowGet);

        //    string addquery = "";
        //    // if (ddlfrom.SelectedValue != "" && ddlfrom.SelectedValue != null)
        //    if (ddlfrom != "" && ddlfrom != null)
        //    {
        //        //  addquery = "and FromId=" + ddlfrom.SelectedValue;
        //        addquery = "and FromId=" + ddlfrom;
        //    }

        //    // if (ddl1from.SelectedValue != "" && ddl1from.SelectedValue != null)
        //    if (ddl1from != "" && ddl1from != null)
        //    {
        //        // addquery = "and FromId=" + ddl1from.SelectedValue; 
        //        addquery = " and FromId=" + ddl1from;
        //    }
        //    // if (ddl1to.SelectedValue != "" && ddl1to.SelectedValue != null)
        //    if (ddl1to != "" && ddl1to != null)
        //    {
        //        // addquery = addquery + "and ToId=" + ddl1to.SelectedValue;
        //        addquery = addquery + " and ToId=" + ddl1to;
        //    }
        //    string addqury = addquery + " and tblReturnInventory.ClientId=" + databaseCon.Decrypt(id);
        //    // if (rdodepottosite.Checked == true)

        //    if (rdomaindepottodepot == "True")
        //    {


        //        var GetData = dbContext.Database.SqlQuery<ReturnInventoryViewModel>("select FromId,ToId,ReturnInvId,[From] FromName,[To] as ToName,ReturnDoneBy,Replace(convert(nvarchar,convert(datetime,ReturnDoneOn,120),106),' ','-') as ReturnDoneOn, ReturnStatus, ReturnDescription, (Select sum(isnull(quantity, 0)) From tblReturnInventoryDetails where tblReturnInventory.ReturnInvId = tblReturnInventoryDetails.ReturnInvId) as NoofItems, Convert(Decimal(18, 2), (Select Sum(isnull(Quantity * Itemweight, 0)) / 1000 From tblReturnInventoryDetails join tblInventory on tblInventory.InventoryID =tblReturnInventoryDetails.InventoryId where tblReturnInventory.ReturnInvId = tblReturnInventoryDetails.ReturnInvId)) as TotalWeight from tblReturnInventory where [From] = 'Depot' and tblReturnInventory.ClientId = " + databaseCon.Decrypt(id) + " order by ReturnInvId Desc").ToList();
        //        return Json(GetData, JsonRequestBehavior.AllowGet);

        //    }

        //    else if (rdodepottosite == "True")
        //    {

        //        //var GetData = dbContext.Database.SqlQuery<ReturnInventoryViewModel>("select FromId,ToId,ReturnInvId,SiteLocation as FromName,[To] as ToName,ReturnDoneBy,Replace(convert(nvarchar,convert(datetime,ReturnDoneOn,120),106),' ','-') as ReturnDoneOn,ReturnStatus from tblReturnInventory join tblSiteLocation on tblReturnInventory.FromId=tblSiteLocation.SiteLocID where [From]='Site'" + addqury + " order by ReturnInvId Desc").ToList();
        //        //  return Json(GetData, JsonRequestBehavior.AllowGet);

        //        var GetData = dbContext.Database.SqlQuery<ReturnInventoryViewModel>("select FromId,ToId,ReturnInvId,SiteLocation as FromName,[To] as ToName,ReturnDoneBy,Replace(convert(nvarchar,convert(datetime,ReturnDoneOn,120),106),' ','-') as ReturnDoneOn,ReturnStatus, ReturnDescription,(Select sum(isnull(quantity,0)) From tblReturnInventoryDetails where tblReturnInventory.ReturnInvId = tblReturnInventoryDetails.ReturnInvId) as NoofItems,Convert(Decimal(18, 2), (Select Sum(isnull(Quantity * Itemweight, 0)) / 1000 From tblReturnInventoryDetails join tblInventory on tblInventory.InventoryID = tblReturnInventoryDetails.InventoryId where tblReturnInventory.ReturnInvId = tblReturnInventoryDetails.ReturnInvId)) as TotalWeight from tblReturnInventory join tblSiteLocation on tblReturnInventory.FromId=tblSiteLocation.SiteLocID where [From]='Site'" + addqury + " order by ReturnInvId Desc").ToList();
        //        return Json(GetData, JsonRequestBehavior.AllowGet);

        //    }
        //    else
        //    {
        //        //var GetData = dbContext.Database.SqlQuery<ReturnInventoryViewModel>("select FromId,ToId,ReturnInvId,case when Description is not null and Description!=''then Right('000000'+Convert(varchar,ScaffRef),6)+' - '+Description else Right('000000'+Convert(varchar,ScaffRef),6) end  as FromName,SiteLocation as ToName,ReturnDoneBy,Replace(convert(nvarchar,convert(datetime,ReturnDoneOn,120),106),' ','-')as ReturnDoneOn,ReturnStatus from tblReturnInventory join tblSiteLocation on tblReturnInventory.ToId = tblSiteLocation.SiteLocID  join tblRegister on tblReturnInventory.FromId = tblRegister.RegisterId where[To] = 'Site'" + addqury + " order by ReturnInvId Desc").ToList();
        //        var GetData = dbContext.Database.SqlQuery<ReturnInventoryViewModel>("select FromId,ToId,ReturnInvId,case when Description is not null and Description!=''then Right('000000'+Convert(varchar,ScaffRef),6)+' - '+Description else Right('000000'+Convert(varchar,ScaffRef),6) end  as FromName,SiteLocation as ToName,ReturnDoneBy,Replace(convert(nvarchar,convert(datetime,ReturnDoneOn,120),106),' ','-')as ReturnDoneOn,ReturnStatus,ReturnDescription,(Select sum(isnull(quantity,0))From tblReturnInventoryDetails where tblReturnInventory.ReturnInvId = tblReturnInventoryDetails.ReturnInvId) as NoofItems,Convert(Decimal(18, 2), (Select Sum(isnull(Quantity * Itemweight, 0)) / 1000 From tblReturnInventoryDetails join tblInventory on tblInventory.InventoryID = tblReturnInventoryDetails.InventoryId where tblReturnInventory.ReturnInvId = tblReturnInventoryDetails.ReturnInvId)) as TotalWeight from tblReturnInventory join tblSiteLocation on tblReturnInventory.ToId = tblSiteLocation.SiteLocID  join tblRegister on tblReturnInventory.FromId = tblRegister.RegisterId where[To] = 'Site'" + addqury + " order by ReturnInvId Desc").ToList();

        //        return Json(GetData, JsonRequestBehavior.AllowGet);
        //    }


        //}

        // using System.Data.SqlClient;   <-- controller ke top par add karna hai (agar pehle se nahi hai)

        // using System.Data.SqlClient;   <-- controller ke top par add karna hai (agar pehle se nahi hai)

        // using System.Data.SqlClient;   <-- controller ke top par add karna hai (agar pehle se nahi hai)

        public ActionResult GetReturnInventoryData(JqueryDatatableParam param, string id, string ddlfrom, string ddl1to, string ddl1from, string rdodepottosite, string rdositetojob, string rdomaindepottodepot)
        {
            try
            {
                dbContext.Database.CommandTimeout = 15 * 60;

                int iDisplayLength = param.length > 0 ? param.length : 10;
                int iDisplayStart = param.start;
                string sSearch = (param.search != null && param.search.value != null) ? param.search.value : "";
                int iSortCol_0 = (param.order != null && param.order.Count > 0) ? param.order[0].column : 0;
                string sSortDir_0 = (param.order != null && param.order.Count > 0 && !string.IsNullOrEmpty(param.order[0].dir)) ? param.order[0].dir : "asc";
                bool bDepotToDepot = false, bDepotToSite = false, bSiteToJob = false;
                bool.TryParse(rdomaindepottodepot, out bDepotToDepot);
                bool.TryParse(rdodepottosite, out bDepotToSite);
                bool.TryParse(rdositetojob, out bSiteToJob);

                int clientId = string.IsNullOrEmpty(id) ? 0 : Convert.ToInt32(databaseCon.Decrypt(id));
                string sSearchSafe = sSearch.Replace("'", "''");
                string ddl1fromSafe = (ddl1from ?? "").Replace("'", "''");
                string ddl1toSafe = (ddl1to ?? "").Replace("'", "''");
                string ddlfromSafe = (ddlfrom ?? "").Replace("'", "''");
                string sSortDirSafe = sSortDir_0.ToLower() == "desc" ? "desc" : "asc";

                string query = "EXEC pro_ReturnInventory_serverside "
                    + "@rdomaindepottodepot = " + (bDepotToDepot ? 1 : 0) + ", "
                    + "@rdodepottosite = " + (bDepotToSite ? 1 : 0) + ", "
                    + "@rdositetojob = " + (bSiteToJob ? 1 : 0) + ", "
                    + "@ddl1from = '" + ddl1fromSafe + "', "
                    + "@ddl1to = '" + ddl1toSafe + "', "
                    + "@ddlfrom = '" + ddlfromSafe + "', "
                    + "@id = " + clientId + ", "
                    + "@DisplayLength = " + iDisplayLength + ", "
                    + "@Displaystart = " + iDisplayStart + ", "
                    + "@search = " + (string.IsNullOrEmpty(sSearch) ? "NULL" : "'" + sSearchSafe + "'") + ", "
                    + "@sortBy = " + iSortCol_0 + ", "
                    + "@sortDir = '" + sSortDirSafe + "'";

                var list = dbContext.Database.SqlQuery<ReturnInventoryViewModel>(query).ToList();
                int totalRecords = 0;
                int totalDisplayRecords = 0;
                if (list != null && list.Count > 0)
                {
                    totalRecords = list[0].TotalCount;
                    totalDisplayRecords = list[0].TotalCount;
                }

                return Json(new
                {
                    draw = param.draw,
                    recordsTotal = totalRecords,
                    recordsFiltered = totalDisplayRecords,
                    data = list
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    draw = param.draw,
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    data = new List<ReturnInventoryViewModel>(),
                    message = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public void ExportToReturnCsvButton_Click(string id, string ddlfrom, string ddl1to, string ddl1from, string rdodepottosite, string rdositetojob, string rdomaindepottodepot)
        {
            string addquery = "";
            string query = "";

            if (ddlfrom != "" && ddlfrom != null && ddlfrom != "undefined")
            {

                addquery = "and FromId=" + ddlfrom;
            }

            if (ddl1from != "" && ddl1from != null && ddl1from != "undefined")
            {

                addquery = "and FromId=" + ddl1from;
            }

            if (ddl1to != "" && ddl1to != null && ddl1to != "undefined")
            {

                addquery = addquery + "and ToId=" + ddl1to;
            }
            string addqury = addquery + "and tblReturnInventory.ClientId=" + databaseCon.Decrypt(id);

            if (rdodepottosite == "true")
            {


                //addquery = addquery + " and ToId='" + databaseCon.Decrypt(id) + "' and [To]='Depot'";
                //query = "select FromId,ToId,ReturnInvId,SiteLocation as FromName,[To] as ToName,ReturnDoneBy,Replace(convert(nvarchar,convert(datetime,ReturnDoneOn,120),106),' ','-') as ReturnDoneOn,ReturnStatus from tblReturnInventory join tblSiteLocation on tblReturnInventory.FromId = tblSiteLocation.SiteLocID where [From]='Site'" + addqury + " order by ReturnInvId Desc" ;

                addquery = addquery + " and ToId='" + databaseCon.Decrypt(id) + "' and [To]='Depot'";
                query = "select ReturnInvId as TransID,ReturnDescription as Description,SiteLocation as 'From',[To] as 'To',ReturnStatus,ReturnDoneBy,Replace(convert(nvarchar,convert(datetime,ReturnDoneOn,120),106),' ','-') as ReturnDoneOn, (Select sum(isnull(quantity,0)) From tblReturnInventoryDetails where tblReturnInventory.ReturnInvId = tblReturnInventoryDetails.ReturnInvId) as '(Requested) No. of Items',Convert(Decimal(18, 2), (Select Sum(isnull(Quantity * Itemweight, 0)) / 1000 From tblReturnInventoryDetails join tblInventory on tblInventory.InventoryID = tblReturnInventoryDetails.InventoryId where tblReturnInventory.ReturnInvId = tblReturnInventoryDetails.ReturnInvId)) as 'TotalWeight(Tonnes)' from tblReturnInventory join tblSiteLocation on tblReturnInventory.FromId=tblSiteLocation.SiteLocID where [From]='Site'" + addqury + " order by ReturnInvId Desc";
            }
            else if (rdositetojob == "true")
            {


                //query = "select FromId,ToId,ReturnInvId,case when Description is not null and Description!=''then Right('000000'+Convert(varchar,ScaffRef),6)+' - '+Description else Right('000000'+Convert(varchar,ScaffRef),6) end  as FromName,SiteLocation as ToName,ReturnDoneBy,Replace(convert(nvarchar,convert(datetime,ReturnDoneOn,120),106),' ','-')as ReturnDoneOn,ReturnStatus from tblReturnInventory join tblSiteLocation on tblReturnInventory.ToId = tblSiteLocation.SiteLocID  join tblRegister on tblReturnInventory.FromId = tblRegister.RegisterId where[To] = 'Site'" + addqury + " order by ReturnInvId Desc";

                query = "select ReturnInvId as TransID,ReturnDescription as Description,case when Description is not null and Description!=''then Right('000000'+Convert(varchar,ScaffRef),6)+' - '+ Replace(Replace(Description,Char(13),' '),char(10),' ') else Right('000000'+Convert(varchar,ScaffRef),6) end  as 'From',SiteLocation as 'To',ReturnStatus,ReturnDoneBy,Replace(convert(nvarchar,convert(datetime,ReturnDoneOn,120),106),' ','-')as ReturnDoneOn,ReturnDescription as Description,(Select sum(isnull(quantity,0))From tblReturnInventoryDetails where tblReturnInventory.ReturnInvId = tblReturnInventoryDetails.ReturnInvId) as '(Requested) No. of Items',Convert(Decimal(18, 2), (Select Sum(isnull(Quantity * Itemweight, 0)) / 1000 From tblReturnInventoryDetails join tblInventory on tblInventory.InventoryID = tblReturnInventoryDetails.InventoryId where tblReturnInventory.ReturnInvId = tblReturnInventoryDetails.ReturnInvId)) as 'TotalWeight(Tonnes)' from tblReturnInventory join tblSiteLocation on tblReturnInventory.ToId = tblSiteLocation.SiteLocID  join tblRegister on tblReturnInventory.FromId = tblRegister.RegisterId where[To] = 'Site'" + addqury + " order by ReturnInvId Desc";


            }
            else
            {


                query = "select ReturnInvId as TransID,ReturnDescription as Description,[From] FromName,[To] as ToName,ReturnStatus,ReturnDoneBy,Replace(convert(nvarchar,convert(datetime,ReturnDoneOn,120),106),' ','-') as ReturnDoneOn, (Select sum(isnull(quantity, 0)) From tblReturnInventoryDetails where tblReturnInventory.ReturnInvId = tblReturnInventoryDetails.ReturnInvId) as NoofItems, Convert(Decimal(18, 2), (Select Sum(isnull(Quantity * Itemweight, 0)) / 1000 From tblReturnInventoryDetails join tblInventory on tblInventory.InventoryID =tblReturnInventoryDetails.InventoryId where tblReturnInventory.ReturnInvId = tblReturnInventoryDetails.ReturnInvId)) as TotalWeight  from tblReturnInventory where [From] = 'Depot' and tblReturnInventory.ClientId = " + databaseCon.Decrypt(id) + " order by ReturnInvId Desc";


            }

            DataTable dt = databaseCon.GetData(query);

            Response.ContentType = "Application/x-msexcel";
            Response.AddHeader("content-disposition", "attachment;filename='ReturnInventoryList'.csv");
            Response.Write(csvfile(dt));
            Response.End();

        }

        private string csvfile(DataTable dtTable)
        {
            StringBuilder sbldr = new StringBuilder();
            if (dtTable.Columns.Count != 0)
            {
                foreach (DataColumn col in dtTable.Columns)
                {
                    sbldr.Append(col.ColumnName + ',');
                }
                sbldr.Append("\r\n");
                foreach (DataRow row in dtTable.Rows)
                {
                    foreach (DataColumn column in dtTable.Columns)
                    {
                        if (row[column].ToString().Contains(','))
                        {
                            sbldr.Append("\"" + row[column].ToString() + "\"" + ',');
                        }
                        else
                        {
                            sbldr.Append(row[column].ToString() + ',');
                        }

                    }
                    sbldr.Append("\r\n");
                }
            }
            return sbldr.ToString();
        }

        public ActionResult btndeleteyes_Click(string ClientId, string ReturnInvId)
        {
            string query = "";
            DataTable dt;
            query = "exec Pro_InventoryReturnDelete'" + ReturnInvId + "','" + databaseCon.Decrypt(ClientId) + "'";
            dt = databaseCon.GetData(query);
            if (dt.Rows.Count != 0)
            {
                if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                {
                    return Json(new { Result = "Success", Message = dt.Rows[0][1].ToString() });
                }
                else if (Convert.ToInt32(dt.Rows[0][0]) == -1)
                {
                    return Json(new { Result = "Failed", Message = dt.Rows[0][1].ToString() });
                }
                else
                {
                    return Json(new { Result = "Failed", Message = dt.Rows[0][1].ToString() });
                }
            }
            else
            {
                return Json(new { Result = "Failed", Message = "Syntax Error. Contact administrator." });
            }


        }

        public ActionResult PipeInventoryHide(string ClientID)
        {
            // string query = "Select * from tblpipelineRequest where Clientid='" + databaseCon.Decrypt(ClientID) + "'";
            string query = "select DISTINCT  PipeLineInvHide from tblSiteLocation where Clientid='" + databaseCon.Decrypt(ClientID) + "' and PipeLineInvHide is not null and PipeLineInvHide != 0";
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                return Json(new { result = "success", Message = "Available Quantity" });
            }
            else
            {
                return Json(new { result = "Failed", Message = "Not Quantity" });
            }
        }
    }
}