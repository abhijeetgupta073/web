using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;

namespace Scaffold.Controllers
{
    public class DamagedInventoryController : Controller
    {
        // GET: DamagedInventory
        ScaffoldEntities dbContext = new ScaffoldEntities();
        public ActionResult DamagedInventory()
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
                var getSiteLocationData = dbContext.Database.SqlQuery<SiteLocationViewModel>("select distinct SiteLocation,SiteLocID from tblSiteLocation where SiteLocation <>'' and SiteLocation is not null and  ClientId='" + databaseCon.Decrypt(id) + "' and OwnerName='" + SiteOwnerName + "' and MaintainInventory = 1 and MaintainInventoryAtJob=1  order by SiteLocation").ToList();
                return Json(getSiteLocationData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var getSiteLocationData = dbContext.Database.SqlQuery<SiteLocationViewModel>("select distinct SiteLocID,SiteLocation from tblSiteLocation where SiteLocation <> '' and SiteLocation is not null and ClientId = '" + databaseCon.Decrypt(id) + "' and MaintainInventory = 1 order by SiteLocation").ToList();
                //var getSiteLocationData = dbContext.Database.SqlQuery<SiteLocationViewModel>("select * from (select 0 as SiteLocID,'All' as SiteLocation union select distinct SiteLocID,SiteLocation from tblSiteLocation where SiteLocation <>'' and SiteLocation is not null and  ClientId = '" + databaseCon.Decrypt(id) + "' and MaintainInventory = 1 ) as sitelisting order by SiteLocation").ToList();
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
                // var getSiteLocationData = dbContext.Database.SqlQuery<SiteLocationViewModel>("select distinct SiteLocation,SiteLocID from tblSiteLocation where SiteLocation <>'' and SiteLocation is not null and  ClientId='" + databaseCon.Decrypt(id) + "' and MaintainInventory = 1 and MaintainInventoryAtJob=1  order by SiteLocation").ToList();
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
        //public ActionResult GetJobDropDownBind(string id, string JobsiteID)
        //{
        //    var getJobData = dbContext.Database.SqlQuery<JobRegisterViewModel>("select RegisterId,Description,RIGHT('000000'+CONVERT(varchar,ScaffRef),6) as ScaffRef from tblRegister where SiteLocID='" + JobsiteID + "'and ClientId='" + databaseCon.Decrypt(id) + "' order by RegisterId").ToList();
        //    return Json(getJobData, JsonRequestBehavior.AllowGet);

        //}
        public ActionResult GetJobDropDownBind(string id, string JobsiteID)
        {
            string query = "";

            if (JobsiteID == "All" || string.IsNullOrEmpty(JobsiteID))
            {
                query = "select RegisterId,Description,RIGHT('000000'+CONVERT(varchar,ScaffRef),6) as ScaffRef " +
                        "from tblRegister where ClientId=" + databaseCon.Decrypt(id) +
                        " order by RegisterId";
            }
            else
            {
                query = "select RegisterId,Description,RIGHT('000000'+CONVERT(varchar,ScaffRef),6) as ScaffRef " +
                        "from tblRegister where SiteLocID=" + JobsiteID +
                        " and ClientId=" + databaseCon.Decrypt(id) +
                        " order by RegisterId";
            }

            var getJobData = dbContext.Database.SqlQuery<JobRegisterViewModel>(query).ToList();

            return Json(getJobData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDamageInventoryData(string id, string ddlfrom, string ddl1to, string ddl1from, string rdodepottosite, string rdositetojob, string rdodepottodepot)
        {

            string addquery = "";
            //if (ddlfrom != "" && ddlfrom != null)
            //{
            //    addquery = "and FromId=" + ddlfrom;
            //}

            //if (ddl1from != "" && ddl1from != null)
            //{
            //    addquery = "and FromId=" + ddl1from;
            //}
            //if (ddl1to != "" && ddl1to != null)
            //{
            //    addquery = addquery + "and ToId=" + ddl1to;
            //}
            if (!string.IsNullOrEmpty(ddlfrom) && ddlfrom != "All")
            {
                addquery += " and FromId=" + ddlfrom;
            }

            if (!string.IsNullOrEmpty(ddl1from) && ddl1from != "All")
            {
                addquery += " and FromId=" + ddl1from;
            }

            if (!string.IsNullOrEmpty(ddl1to) && ddl1to != "All")
            {
                addquery += " and ToId=" + ddl1to;
            }

            string addqury = addquery + "and tblDamagedInventory.ClientId=" + databaseCon.Decrypt(id);
            if (rdodepottosite == "True")
            {
                if (Request.QueryString["FromId"] != null && Request.QueryString["ToId"] != null)
                {
                    addquery = "and FromId=" + databaseCon.Decrypt(Request.QueryString["FromId"]);
                }

                // string query = "select FromId,ToId,DamagedInvId,SiteLocation as FromName,[ToName],tblDamagedInventory.CreatedBy,Replace(convert(nvarchar,convert(datetime,tblDamagedInventory.CreatedOn,120),106),' ','-') as CreatedOn from tblDamagedInventory join tblSiteLocation on tblDamagedInventory.FromId=tblSiteLocation.SiteLocID where [FromName]='Site'" + addqury + " order by DamagedInvId desc ";

                // Change On 19-Feb-2019 Showing Name In Place Of CreatedBy 

                // Comment On 20 April 2020

                string query = "select FromId,ToId,DamagedInvId,SiteLocation as FromName,[ToName],(firstname +' ' + LastName) as Name,tblDamagedInventory.CreatedBy,Replace(convert(nvarchar,tblDamagedInventory.CreatedOn,106),' ','-') as CreatedOn from tblDamagedInventory join tbluser on tblDamagedInventory.CreatedBy=tbluser.EmailID join tblSiteLocation on tblDamagedInventory.FromId=tblSiteLocation.SiteLocID where [FromName]='Site'" + addqury + " order by DamagedInvId desc ";


                //DataTable dt = databaseCon.GetData(query);
                //if (dt.Rows.Count < 1)
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "hide", "<script>filterhide();</script>", false);
                //}
                //GridDamagedInventory.DataSource = dt;


                var GetData = dbContext.Database.SqlQuery<DamagedInventoryViewModel>("select FromId,ToId,DamagedInvId,SiteLocation as FromName,[ToName],(firstname +' ' + LastName) as Name,tblDamagedInventory.CreatedBy,Replace(convert(nvarchar,tblDamagedInventory.CreatedOn,106),' ','-') as CreatedOn from tblDamagedInventory join tbluser on tblDamagedInventory.CreatedBy=tbluser.EmailID join tblSiteLocation on tblDamagedInventory.FromId=tblSiteLocation.SiteLocID where [FromName]='Site'" + addqury + " order by DamagedInvId desc ").ToList();
                return Json(GetData, JsonRequestBehavior.AllowGet);

            }
            else if (rdositetojob == "True")
            {
                if (Request.QueryString["FromId"] != null && Request.QueryString["ToId"] != null)
                {
                    addquery = "and FromId=" + databaseCon.Decrypt(Request.QueryString["FromId"]) + "and ToId=" + databaseCon.Decrypt(Request.QueryString["ToId"]);
                }
                // string query = "select FromId,ToId,DamagedInvId,case when Description is not null and Description!=''then Right('000000'+Convert(varchar,ScaffRef),6)+' - '+Description else Right('000000'+Convert(varchar,ScaffRef),6) end  as FromName,SiteLocation as ToName,tblDamagedInventory.CreatedBy,Replace(convert(nvarchar,convert(datetime,tblDamagedInventory.CreatedOn,120),106),' ','-')as CreatedOn from tblDamagedInventory join tblSiteLocation on tblDamagedInventory.ToId = tblSiteLocation.SiteLocID  join tblRegister on tblDamagedInventory.FromId = tblRegister.RegisterId where[ToName] = 'Site' " + addqury + " order by DamagedInvId desc ";

                // Change On 19-Feb-2019 Showing Name In Place Of CreatedBy 



                //string query = "select FromId,ToId,DamagedInvId,case when Description is not null and Description!=''then Right('000000'+Convert(varchar,ScaffRef),6)+' - '+Description else Right('000000'+Convert(varchar,ScaffRef),6) end  as FromName,SiteLocation as ToName,(firstname +' ' + LastName) as Name,tblDamagedInventory.CreatedBy, Replace(convert(nvarchar,tblDamagedInventory.CreatedOn,106),' ','-') as CreatedOn from tblDamagedInventory join tblSiteLocation on tblDamagedInventory.ToId = tblSiteLocation.SiteLocID  join tblRegister on tblDamagedInventory.FromId = tblRegister.RegisterId join tblUser on tblUser.EmailID=tblDamagedInventory.CreatedBy where[ToName] = 'Site' " + addqury + " order by DamagedInvId desc ";

                //DataTable dt = databaseCon.GetData(query);
                //if (dt.Rows.Count < 1)
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "hide", "<script>filterhide();</script>", false);
                //}
                //GridDamagedInventory.DataSource = dt;


                var GetData = dbContext.Database.SqlQuery<DamagedInventoryViewModel>("select FromId,ToId,DamagedInvId,case when Description is not null and Description!=''then Right('000000'+Convert(varchar,ScaffRef),6)+' - '+Description else Right('000000'+Convert(varchar,ScaffRef),6) end  as FromName,SiteLocation as ToName,(firstname +' ' + LastName) as Name,tblDamagedInventory.CreatedBy, Replace(convert(nvarchar,tblDamagedInventory.CreatedOn,106),' ','-') as CreatedOn from tblDamagedInventory join tblSiteLocation on tblDamagedInventory.ToId = tblSiteLocation.SiteLocID  join tblRegister on tblDamagedInventory.FromId = tblRegister.RegisterId join tblUser on tblUser.EmailID=tblDamagedInventory.CreatedBy where[ToName] = 'Site' " + addqury + " order by DamagedInvId desc ").ToList();
                return Json(GetData, JsonRequestBehavior.AllowGet);
            }
            else if (rdodepottodepot == "True")
            {


                //  string query = "select FromId,ToId,DamagedInvId , FromName,[ToName],tblDamagedInventory.CreatedBy,Replace(convert(nvarchar,convert(datetime,tblDamagedInventory.CreatedOn,120),106),' ','-') as CreatedOn from tblDamagedInventory  where [FromName]='Depot'and tblDamagedInventory.ClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' order by DamagedInvId desc ";

                // Change On 19-Feb-2019 Showing Name In Place Of CreatedBy 



                string query = "select FromId,ToId,DamagedInvId , FromName,[ToName],(firstname +' ' + LastName) as Name,tblDamagedInventory.CreatedBy,Replace(convert(nvarchar,tblDamagedInventory.CreatedOn,106),' ','-') CreatedOn from  tblDamagedInventory join tbluser on tblDamagedInventory.CreatedBy= tbluser.EmailID  where [FromName]='Depot'and tblDamagedInventory.ClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' order by DamagedInvId desc ";
                //DataTable dt = databaseCon.GetData(query);
                //if (dt.Rows.Count < 1)
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "hide", "<script>filterhide();</script>", false);
                //}
                //GridDamagedInventory.DataSource = dt;


                var GetData = dbContext.Database.SqlQuery<DamagedInventoryViewModel>(query).ToList();
                return Json(GetData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("No Dropdown Selected", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DamageInventoryChildGridData(string id, string DamagedInvId)
        {

            var GetData = dbContext.Database.SqlQuery<DamagedInventoryViewModel>("select DIDetailID,Fromid,ToId, Fromname,tblDamagedInventory.DamagedInvId,MaterialType,ItemCode,ItemDesc,DamagedQuantity ,Reason from tblDamagedInventory join tblDamagedInventoryDetails on tblDamagedInventory.DamagedinvID=tblDamagedInventoryDetails.DamagedInvId  join tblInventory on tblDamagedInventoryDetails.InventoryId=tblInventory.InventoryID where tblDamagedInventory.DamagedinvID=" + DamagedInvId + "").ToList();

            return Json(GetData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllDamageData(string id, string rdodepottosite, string rdositetojob, string rdodepottodepot)
        {
            string query = "";
            DataTable dt;
            
            if (rdodepottodepot== "True")
            {
                query = "select FromId,ToId from tblDamagedInventory where clientId='" + databaseCon.Decrypt(id) + "'";
                dt = databaseCon.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    return Json(new { result = "Success", Message = databaseCon.Encrypt("1") });
                }
                else
                {
                    return Json(new { result = "Failed", Message = "There are no damage item in Depot." });
                }

            }
            else if (rdodepottosite == "True")
            {
                query = "select FromId,ToId from tblDamagedInventory where clientId='" + databaseCon.Decrypt(id) + "' and FromName='Site' and ToName='Depot'";
                dt = databaseCon.GetData(query);
                if (dt.Rows.Count > 0)
                {
                  return Json(new { result = "Success", Message = databaseCon.Encrypt("2") });
                }
                else
                {
                     return Json(new { result = "Failed", Message = "There are no damage item in Site." });
                }

            }
            else if (rdositetojob == "True")
            {
                query = "select FromId,ToId from tblDamagedInventory where clientId='" + databaseCon.Decrypt(id) + "' and FromName='Job' and ToName='Site' ";
                dt = databaseCon.GetData(query);
                if (dt.Rows.Count > 0)
                {
                  return Json(new { result = "Success", Message = databaseCon.Encrypt("3") });
                }
                else
                {
                     return Json(new { result = "Failed", Message = "There are no damage item in Job." });
                }
            }
            else {
                return Json(new { result = "Failed", Message = "Please Select Any One." });
            }

          
        }

        public ActionResult PipeInventoryHide(string ClientID)
        {
            //  string query = "Select * from tblpipelineRequest where Clientid='" + databaseCon.Decrypt(ClientID) + "'";
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