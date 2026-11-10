using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;
using System.Text;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace Scaffold.Controllers
{
    public class RequestInventoryController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: RequestInventory
        public ActionResult RequestInventory()
        {
            string qry = "select Right ('000000' + convert (varchar, [scaffref]),6) as ScaffRref,* from vw_tblregister where RegisterId=" + databaseCon.Decrypt(Request.QueryString["RID"]) + "and Sitelocid=" + databaseCon.Decrypt(Request.QueryString["Siteid"]);
            DataTable dt = databaseCon.GetData(qry);
            if (dt.Rows.Count > 0)
            {
                // txtsite.Text = dt.Rows[0]["SiteLocation"].ToString();
                //  txtjob.Text = dt.Rows[0]["ScaffRref"].ToString();
                ViewBag.SiteName = dt.Rows[0]["SiteLocation"].ToString();
                ViewBag.ScaffRref = dt.Rows[0]["ScaffRref"].ToString();
            }
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
                // var getSiteLocationData = dbContext.Database.SqlQuery<SiteLocationViewModel>("select distinct SiteLocID,SiteLocation from tblSiteLocation where SiteLocation <> '' and SiteLocation is not null and ClientId = '" + databaseCon.Decrypt(id) + "' and MaintainInventory = 1 order by SiteLocation").ToList();
                //ADDED DisplayOrder As To Get 'ALL' on top on 12-feb-2022
                //var getSiteLocationData = dbContext.Database.SqlQuery<SiteLocationViewModel>("select * from (select 0 as SiteLocID,'All' as SiteLocation union select distinct SiteLocID,SiteLocation from tblSiteLocation where SiteLocation <>'' and SiteLocation is not null and  ClientId = '" + databaseCon.Decrypt(id) + "' and MaintainInventory = 1 ) as sitelisting order by SiteLocation").ToList();
                var getSiteLocationData = dbContext.Database.SqlQuery<SiteLocationViewModel>("select * from (select 0 as SiteLocID,'All' as SiteLocation,0 as DisplayOrder union select distinct SiteLocID,SiteLocation,1 as DisplayOrder from tblSiteLocation where SiteLocation <>'' and SiteLocation is not null and  ClientId = '" + databaseCon.Decrypt(id) + "' and MaintainInventory = 1 ) as sitelisting order by DisplayOrder,SiteLocation").ToList();
                return Json(getSiteLocationData, JsonRequestBehavior.AllowGet);
            }
        }

        //public ActionResult GetRequestInventoryData(string id, string FromId, string ToId, string ddl1from, string ddl1to, string JobSiteid, string ddlfrom, string rdodepottosite, string rdositetojob, string RID, string rdoThirdPartytoDepot)
        //{

        //    int fromid;
        //    string addquery = "";
        //    //if (Request.QueryString["FromId"] != "" && Request.QueryString["FromId"] != null)
        //    //{
        //    //    addquery = " and FromId=" + databaseCon.Decrypt(Request.QueryString["FromId"]) + " and ToId=" + databaseCon.Decrypt(Request.QueryString["ToId"]);
        //    //}
        //    //if (ddl1from.SelectedValue != "" && ddl1from.SelectedValue != "All")
        //    //{
        //    //    addquery = " and FromId=" + ddl1from.SelectedValue + " and [From]='Job'";
        //    //}

        //    //if (ddl1to.SelectedValue != "" && ddl1to.SelectedValue != "All")
        //    //{
        //    //    addquery = addquery + " and ToId=" + ddl1to.SelectedValue + " and [To]='Site'";
        //    //}
        //    //if (Request.QueryString["Siteid"] != "" && Request.QueryString["Siteid"] != null)
        //    //{
        //    //    addquery = " and ToId=" + databaseCon.Decrypt(Request.QueryString["Siteid"]) + " and [To]='Site' and FromId='" + databaseCon.Decrypt(Request.QueryString["RID"]) + "'";
        //    //}
        //    //if (ddlfrom.SelectedValue != "" && ddlfrom.SelectedValue != "All")
        //    //{
        //    //    addquery = " and FromId=" + ddlfrom.SelectedValue + " and [From]='Site'";
        //    //    fromid = Convert.ToInt32(ddlfrom.SelectedValue);
        //    //}


        //    if (FromId != "" && FromId != null)
        //    {
        //        addquery = " and FromId=" + databaseCon.Decrypt(FromId) + " and ToId=" + databaseCon.Decrypt(ToId);
        //    }
        //    if (ddl1from != "" && ddl1from != "All" && ddl1from != null)
        //    {
        //        addquery = " and FromId=" + databaseCon.Decrypt(ddl1from) + " and [From]='Job'";
        //    }

        //    if (ddl1to != "" && ddl1to != "All" && ddl1to != null)
        //    {
        //        addquery = addquery + " and ToId=" + databaseCon.Decrypt(ddl1to) + " and [To]='Site'";
        //    }
        //    if (JobSiteid != "" && JobSiteid != null)
        //    {
        //        addquery = " and ToId=" + databaseCon.Decrypt(JobSiteid) + " and [To]='Site' and FromId='" + databaseCon.Decrypt(RID) + "'";
        //    }
        //    if (ddlfrom != "" && ddlfrom != "All" && ddlfrom != null)
        //    {
        //        addquery = " and FromId=" + ddlfrom + " and [From]='Site'";
        //        fromid = Convert.ToInt32(databaseCon.Decrypt(ddlfrom));
        //    }


        //    string query;
        //    //  if (rdodepottosite.Checked == true)
        //    if (rdodepottosite == "True")
        //    {
        //        addquery = addquery + " and ToId='" + databaseCon.Decrypt(id) + "' and [To]='Depot'";

        //        dbContext.Database.CommandTimeout = 15 * 60;

        //        //var RequestInventoryData = dbContext.Database.SqlQuery<RequestInventoryViewModel>("select transId,case when RequestStatus='Confirm' then 'Confirmed' else RequestStatus End as RequestStatus, transtype, FromId,Supplier,TransDesc,case when DeliveryStatus='Confirm' then 'Confirmed' else DeliveryStatus End as DeliveryStatus, case when [From]='Site' then tblSiteLocation.SiteLocation when [From]='Job' then VW_TblRegister.Description else '' end as FromName,case when[To] = 'Site' then tblSiteLocation.SiteLocation when[To] = 'Depot' then[To] else '' end as ToName , ToId,[To],FromPerson,ToPerson ,FromTime,ToTime ,PickDate,(Select sum(isnull(quantity,0)) From tblTransactionHistRequest where tblTransaction.TransId = tblTransactionHistRequest.TransId) as NoofItems, (Select Sum(isnull(Quantity * Itemweight, 0))/1000 From tblTransactionHistRequest join tblInventory on tblInventory.InventoryID = tblTransactionHistRequest.InventoryId where tblTransaction.TransId = tblTransactionHistRequest.TransId) as TotalWeight,ReceiptConfirmation,LastEmailSentOn,Comment from tblTransaction left join tblSiteLocation on tblTransaction.FromId = tblSiteLocation.SiteLocID and[From] = 'Site' or (tblTransaction.ToId = tblSiteLocation.SiteLocID and[To] = 'Site') left join tblClient on tblClient.ClientId = tblTransaction.ToId and[To] = 'Depot' left join VW_TblRegister on tblTransaction.FromId = VW_TblRegister.RegisterId and[From] = 'Job' left join tblDeliveryDocket on tblTransaction.DeliveryId=tblDeliveryDocket.DeliveryID and tblTransaction.ClientId=tblDeliveryDocket.clientId where 1=1 and tbltransaction.ClientId='" + databaseCon.Decrypt(id) + "'and [To]='Depot'" + addquery + "Order by transId desc,DeliveryStatus").ToList();
        //        string RequestInventoryDataquesrt = "select transId,case when RequestStatus='Confirm' then 'Confirmed' else RequestStatus End as RequestStatus, transtype, FromId,Supplier,TransDesc,case when DeliveryStatus='Confirm' then 'Confirmed' else DeliveryStatus End as DeliveryStatus, case when [From]='Site' then tblSiteLocation.SiteLocation when [From]='Job' then VW_TblRegister.Description else '' end as FromName,case when[To] = 'Site' then tblSiteLocation.SiteLocation when[To] = 'Depot' then[To] else '' end as ToName , ToId,[To],FromPerson,ToPerson ,FromTime,RequiredDate,ToTime ,PickDate,(Select sum(isnull(quantity,0)) From tblTransactionHistRequest where tblTransaction.TransId = tblTransactionHistRequest.TransId) as NoofItems, (Select Sum(isnull(Quantity * Itemweight, 0))/1000 From tblTransactionHistRequest join tblInventory on tblInventory.InventoryID = tblTransactionHistRequest.InventoryId where tblTransaction.TransId = tblTransactionHistRequest.TransId) as TotalWeight,ReceiptConfirmation,LastEmailSentOn,Comment from tblTransaction left join tblSiteLocation on tblTransaction.FromId = tblSiteLocation.SiteLocID and[From] = 'Site' or (tblTransaction.ToId = tblSiteLocation.SiteLocID and[To] = 'Site') left join tblClient on tblClient.ClientId = tblTransaction.ToId and[To] = 'Depot' left join VW_TblRegister on tblTransaction.FromId = VW_TblRegister.RegisterId and[From] = 'Job' left join tblDeliveryDocket on tblTransaction.DeliveryId=tblDeliveryDocket.DeliveryID and tblTransaction.ClientId=tblDeliveryDocket.clientId where 1=1 and tbltransaction.ClientId='" + databaseCon.Decrypt(id) + "'and [To]='Depot'" + addquery + "Order by transId desc,DeliveryStatus";
        //        var RequestInventoryData = dbContext.Database.SqlQuery<RequestInventoryViewModel>("select transId,case when RequestStatus='Confirm' then 'Confirmed' else RequestStatus End as RequestStatus, transtype, FromId,Supplier,TransDesc,case when DeliveryStatus='Confirm' then 'Confirmed' else DeliveryStatus End as DeliveryStatus, case when [From]='Site' then tblSiteLocation.SiteLocation when [From]='Job' then VW_TblRegister.Description else '' end as FromName,case when[To] = 'Site' then tblSiteLocation.SiteLocation when[To] = 'Depot' then[To] else '' end as ToName , ToId,[To],FromPerson,ToPerson ,FromTime,RequiredDate,ToTime ,PickDate,(Select sum(isnull(quantity,0)) From tblTransactionHistRequest where tblTransaction.TransId = tblTransactionHistRequest.TransId) as NoofItems, (Select Sum(isnull(Quantity * Itemweight, 0))/1000 From tblTransactionHistRequest join tblInventory on tblInventory.InventoryID = tblTransactionHistRequest.InventoryId where tblTransaction.TransId = tblTransactionHistRequest.TransId) as TotalWeight,ReceiptConfirmation,LastEmailSentOn,Comment from tblTransaction left join tblSiteLocation on tblTransaction.FromId = tblSiteLocation.SiteLocID and[From] = 'Site' or (tblTransaction.ToId = tblSiteLocation.SiteLocID and[To] = 'Site') left join tblClient on tblClient.ClientId = tblTransaction.ToId and[To] = 'Depot' left join VW_TblRegister on tblTransaction.FromId = VW_TblRegister.RegisterId and[From] = 'Job' left join tblDeliveryDocket on tblTransaction.DeliveryId=tblDeliveryDocket.DeliveryID and tblTransaction.ClientId=tblDeliveryDocket.clientId where 1=1 and tbltransaction.ClientId='" + databaseCon.Decrypt(id) + "'and [To]='Depot'" + addquery + "Order by transId desc,DeliveryStatus").ToList();
        //        return Json(RequestInventoryData, JsonRequestBehavior.AllowGet);


        //        //  gridRequest.Style.Add("Display", "block");
        //        // addquery = addquery + " and ToId='" + databaseCon.Decrypt(id) + "' and [To]='Depot'";
        //        // query = "select transId,case when RequestStatus='Confirm' then 'Confirmed' else RequestStatus End as RequestStatus, transtype, FromId,TransDesc,case when DeliveryStatus='Confirm' then 'Confirmed' else DeliveryStatus End as DeliveryStatus, case when [From]='Site' then tblSiteLocation.SiteLocation when [From]='Job' then VW_TblRegister.Description else '' end as FromName,case when[To] = 'Site' then tblSiteLocation.SiteLocation when[To] = 'Depot' then[To] else '' end as ToName , ToId,[To],FromPerson,ToPerson ,FromTime,ToTime ,(Select sum(isnull(quantity,0)) From tblTransactionHistRequest where tblTransaction.TransId = tblTransactionHistRequest.TransId) as NoofItems, (Select Sum(isnull(Quantity * Itemweight, 0))/1000 From tblTransactionHistRequest join tblInventory on tblInventory.InventoryID = tblTransactionHistRequest.InventoryId where tblTransaction.TransId = tblTransactionHistRequest.TransId) as TotalWeight,ReceiptConfirmation from tblTransaction left join tblSiteLocation on tblTransaction.FromId = tblSiteLocation.SiteLocID and[From] = 'Site' or (tblTransaction.ToId = tblSiteLocation.SiteLocID and[To] = 'Site') left join tblClient on tblClient.ClientId = tblTransaction.ToId and[To] = 'Depot' left join VW_TblRegister on tblTransaction.FromId = VW_TblRegister.RegisterId and[From] = 'Job' left join tblDeliveryDocket on tblTransaction.DeliveryId=tblDeliveryDocket.DeliveryID and tblTransaction.ClientId=tblDeliveryDocket.clientId where 1=1 and tbltransaction.ClientId='" + databaseCon.Decrypt(id) + "'and [To]='Depot'" + addquery + "Order by transId desc, DeliveryStatus";
        //        //DataTable dt = databaseCon.GetData(query);
        //        //if (dt.Rows.Count < 1)
        //        //{
        //        //    //  ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "hide", "<script>filterhide();</script>", false);
        //        //}
        //        // gridRequest.DataSource = dt;
        //    }
        //    //  else if (rdositetojob.Checked == true)
        //    else if (rdositetojob == "True")
        //    {
        //        dbContext.Database.CommandTimeout = 15 * 60;
        //        //var RequestInventoryData = dbContext.Database.SqlQuery<RequestInventoryViewModel>("select transId,case when RequestStatus='Confirm' then 'Confirmed' else RequestStatus End as RequestStatus, transtype, FromId,Supplier,TransDesc,case when DeliveryStatus='Confirm' then 'Confirmed' else DeliveryStatus End as DeliveryStatus, case when Description is not null and Description!='' then Right('000000'+Convert(varchar,ScaffRef),6)+' - '+Description else Right('000000'+Convert(varchar,ScaffRef),6) end  as FromName,case when[To] = 'Site' then tblSiteLocation.SiteLocation when[To] = 'Depot' then[To] else '' end as ToName , ToId,[To],FromPerson,ToPerson ,FromTime,TOTime ,PickDate,(Select sum(isnull(quantity,0)) From tblTransactionHistRequest where tblTransaction.TransId = tblTransactionHistRequest.TransId) as NoofItems, (Select Sum(isnull(Quantity * Itemweight, 0))/1000 From tblTransactionHistRequest join tblInventory on tblInventory.InventoryID = tblTransactionHistRequest.InventoryId where tblTransaction.TransId = tblTransactionHistRequest.TransId) as TotalWeight,Null as ReceiptConfirmation from tblTransaction left join tblSiteLocation on tblTransaction.FromId = tblSiteLocation.SiteLocID and[From] = 'Site' or (tblTransaction.ToId = tblSiteLocation.SiteLocID and[To] = 'Site') left join tblClient on tblClient.ClientId = tblTransaction.ToId and[To] = 'Depot' left join VW_TblRegister on tblTransaction.FromId = VW_TblRegister.RegisterId and[From] = 'Job' left join tblDeliveryDocket on tblTransaction.DeliveryId = tblDeliveryDocket.DeliveryID and tblTransaction.ClientId = tblDeliveryDocket.clientId where 1=1 and tbltransaction.ClientId='" + databaseCon.Decrypt(id) + "'and [To]<>'Depot'" + addquery + "Order by transId desc, DeliveryStatus").ToList();
        //        // added comment column
        //        string fdfjdsf = "select transId,case when RequestStatus='Confirm' then 'Confirmed' else RequestStatus End as RequestStatus, transtype, FromId,Supplier,TransDesc,case when DeliveryStatus='Confirm' then 'Confirmed' else DeliveryStatus End as DeliveryStatus, case when Description is not null and Description!='' then Right('000000'+Convert(varchar,ScaffRef),6)+' - '+Description else Right('000000'+Convert(varchar,ScaffRef),6) end  as FromName,case when[To] = 'Site' then tblSiteLocation.SiteLocation when[To] = 'Depot' then[To] else '' end as ToName , ToId,[To],FromPerson,ToPerson ,FromTime,RequiredDate,TOTime ,PickDate,Comment,(Select sum(isnull(quantity,0)) From tblTransactionHistRequest where tblTransaction.TransId = tblTransactionHistRequest.TransId) as NoofItems, (Select Sum(isnull(Quantity * Itemweight, 0))/1000 From tblTransactionHistRequest join tblInventory on tblInventory.InventoryID = tblTransactionHistRequest.InventoryId where tblTransaction.TransId = tblTransactionHistRequest.TransId) as TotalWeight,Null as ReceiptConfirmation from tblTransaction left join tblSiteLocation on tblTransaction.FromId = tblSiteLocation.SiteLocID and[From] = 'Site' or (tblTransaction.ToId = tblSiteLocation.SiteLocID and[To] = 'Site') left join tblClient on tblClient.ClientId = tblTransaction.ToId and[To] = 'Depot' left join VW_TblRegister on tblTransaction.FromId = VW_TblRegister.RegisterId and[From] = 'Job' left join tblDeliveryDocket on tblTransaction.DeliveryId = tblDeliveryDocket.DeliveryID and tblTransaction.ClientId = tblDeliveryDocket.clientId where 1=1 and tbltransaction.ClientId='" + databaseCon.Decrypt(id) + "'and [To]<>'Depot'" + addquery + "Order by transId desc, DeliveryStatus";
        //        var RequestInventoryData = dbContext.Database.SqlQuery<RequestInventoryViewModel>("select transId,case when RequestStatus='Confirm' then 'Confirmed' else RequestStatus End as RequestStatus, transtype, FromId,Supplier,TransDesc,case when DeliveryStatus='Confirm' then 'Confirmed' else DeliveryStatus End as DeliveryStatus, case when Description is not null and Description!='' then Right('000000'+Convert(varchar,ScaffRef),6)+' - '+Description else Right('000000'+Convert(varchar,ScaffRef),6) end  as FromName,case when[To] = 'Site' then tblSiteLocation.SiteLocation when[To] = 'Depot' then[To] else '' end as ToName , ToId,[To],FromPerson,ToPerson ,FromTime,RequiredDate,TOTime ,PickDate,Comment,(Select sum(isnull(quantity,0)) From tblTransactionHistRequest where tblTransaction.TransId = tblTransactionHistRequest.TransId) as NoofItems, (Select Sum(isnull(Quantity * Itemweight, 0))/1000 From tblTransactionHistRequest join tblInventory on tblInventory.InventoryID = tblTransactionHistRequest.InventoryId where tblTransaction.TransId = tblTransactionHistRequest.TransId) as TotalWeight,Null as ReceiptConfirmation from tblTransaction left join tblSiteLocation on tblTransaction.FromId = tblSiteLocation.SiteLocID and[From] = 'Site' or (tblTransaction.ToId = tblSiteLocation.SiteLocID and[To] = 'Site') left join tblClient on tblClient.ClientId = tblTransaction.ToId and[To] = 'Depot' left join VW_TblRegister on tblTransaction.FromId = VW_TblRegister.RegisterId and[From] = 'Job' left join tblDeliveryDocket on tblTransaction.DeliveryId = tblDeliveryDocket.DeliveryID and tblTransaction.ClientId = tblDeliveryDocket.clientId where 1=1 and tbltransaction.ClientId='" + databaseCon.Decrypt(id) + "'and [To]<>'Depot'" + addquery + "Order by transId desc, DeliveryStatus").ToList();


        //        ////  gridRequest.Style.Add("Display", "block");


        //        //query = "select transId,case when RequestStatus='Confirm' then 'Confirmed' else RequestStatus End as RequestStatus, transtype, FromId,TransDesc,case when DeliveryStatus='Confirm' then 'Confirmed' else DeliveryStatus End as DeliveryStatus, case when Description is not null and Description!='' then Right('000000'+Convert(varchar,ScaffRef),6)+' - '+Description else Right('000000'+Convert(varchar,ScaffRef),6) end  as FromName,case when[To] = 'Site' then tblSiteLocation.SiteLocation when[To] = 'Depot' then[To] else '' end as ToName , ToId,[To],FromPerson,ToPerson ,FromTime,TOTime ,(Select sum(isnull(quantity,0)) From tblTransactionHistRequest where tblTransaction.TransId = tblTransactionHistRequest.TransId) as NoofItems, (Select Sum(isnull(Quantity * Itemweight, 0))/1000 From tblTransactionHistRequest join tblInventory on tblInventory.InventoryID = tblTransactionHistRequest.InventoryId where tblTransaction.TransId = tblTransactionHistRequest.TransId) as TotalWeight,Null as ReceiptConfirmation from tblTransaction left join tblSiteLocation on tblTransaction.FromId = tblSiteLocation.SiteLocID and[From] = 'Site' or (tblTransaction.ToId = tblSiteLocation.SiteLocID and[To] = 'Site') left join tblClient on tblClient.ClientId = tblTransaction.ToId and[To] = 'Depot' left join VW_TblRegister on tblTransaction.FromId = VW_TblRegister.RegisterId and[From] = 'Job' where 1=1 and tbltransaction.ClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "'and [To]<>'Depot'" + addquery + "Order by transId desc, DeliveryStatus";
        //        //   DataTable dt = databaseCon.GetData(query);
        //        //  if (dt.Rows.Count < 1)
        //        //  {
        //        //    //  ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "hide", "<script>filterhide();</script>", false);
        //        //  }
        //        // gridRequest.DataSource = dt;

        //        return Json(RequestInventoryData, JsonRequestBehavior.AllowGet);
        //    }
        //    else if (rdoThirdPartytoDepot == "True")
        //    {
        //        addquery = addquery + " and ToId='" + databaseCon.Decrypt(id) + "' and [To]='ThirdParty'";
        //        //var RequestInventoryData = dbContext.Database.SqlQuery<RequestInventoryViewModel>("select transId,case when RequestStatus='Confirm' then 'Confirmed' else RequestStatus End as RequestStatus, transtype, FromId,Supplier,TransDesc,case when DeliveryStatus = 'Confirm' then 'Confirmed' else DeliveryStatus End as DeliveryStatus,[From] as FromName,[To] as ToName,ToId,[To], FromPerson, ToPerson, FromTime, ToTime,PickDate,(Select sum(isnull(quantity, 0)) From tblTransactionHistRequest where tblTransaction.TransId = tblTransactionHistRequest.TransId) as NoofItems,(Select Sum(isnull(Quantity * Itemweight, 0)) / 1000 From tblTransactionHistRequest join tblInventory on tblInventory.InventoryID = tblTransactionHistRequest.InventoryId where tblTransaction.TransId = tblTransactionHistRequest.TransId) as TotalWeight,ReceiptConfirmation,LastEmailSentOn,Comment from tblTransaction left join tblDeliveryDocket on tblTransaction.DeliveryId = tblDeliveryDocket.DeliveryID and tblTransaction.ClientId = tblDeliveryDocket.clientId where 1 = 1 and tbltransaction.ClientId = '" + databaseCon.Decrypt(id) + "' and [To] = 'ThirdParty' " + addquery + " Order by transId desc, DeliveryStatus").ToList();
        //        dbContext.Database.CommandTimeout = 15 * 60;
        //        string sgkjgs = "select transId,case when RequestStatus='Confirm' then 'Confirmed' else RequestStatus End as RequestStatus, transtype, FromId,Supplier,TransDesc,case when DeliveryStatus = 'Confirm' then 'Confirmed' else DeliveryStatus End as DeliveryStatus,[From] as FromName,[To] as ToName,ToId,[To], FromPerson, ToPerson, FromTime,RequiredDate, ToTime,PickDate,(Select sum(isnull(quantity, 0)) From tblTransactionHistRequest where tblTransaction.TransId = tblTransactionHistRequest.TransId) as NoofItems,(Select Sum(isnull(Quantity * Itemweight, 0)) / 1000 From tblTransactionHistRequest join tblInventory on tblInventory.InventoryID = tblTransactionHistRequest.InventoryId where tblTransaction.TransId = tblTransactionHistRequest.TransId) as TotalWeight,ReceiptConfirmation,LastEmailSentOn,Comment from tblTransaction left join tblDeliveryDocket on tblTransaction.DeliveryId = tblDeliveryDocket.DeliveryID and tblTransaction.ClientId = tblDeliveryDocket.clientId where 1 = 1 and tbltransaction.ClientId = '" + databaseCon.Decrypt(id) + "' and [To] = 'ThirdParty' " + addquery + " Order by transId desc, DeliveryStatus";
        //        var RequestInventoryData = dbContext.Database.SqlQuery<RequestInventoryViewModel>("select transId,case when RequestStatus='Confirm' then 'Confirmed' else RequestStatus End as RequestStatus, transtype, FromId,Supplier,TransDesc,case when DeliveryStatus = 'Confirm' then 'Confirmed' else DeliveryStatus End as DeliveryStatus,[From] as FromName,[To] as ToName,ToId,[To], FromPerson, ToPerson, FromTime,RequiredDate, ToTime,PickDate,(Select sum(isnull(quantity, 0)) From tblTransactionHistRequest where tblTransaction.TransId = tblTransactionHistRequest.TransId) as NoofItems,(Select Sum(isnull(Quantity * Itemweight, 0)) / 1000 From tblTransactionHistRequest join tblInventory on tblInventory.InventoryID = tblTransactionHistRequest.InventoryId where tblTransaction.TransId = tblTransactionHistRequest.TransId) as TotalWeight,ReceiptConfirmation,LastEmailSentOn,Comment from tblTransaction left join tblDeliveryDocket on tblTransaction.DeliveryId = tblDeliveryDocket.DeliveryID and tblTransaction.ClientId = tblDeliveryDocket.clientId where 1 = 1 and tbltransaction.ClientId = '" + databaseCon.Decrypt(id) + "' and [To] = 'ThirdParty' " + addquery + " Order by transId desc, DeliveryStatus").ToList();

        //        return Json(RequestInventoryData, JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(new { result = "Failed", Message = "No Record Found." });
        //}
        [HttpPost]
        public JsonResult GetRequestInventoryData(JqueryDatatableParam param, string id, string ddlfrom,
     string rdodepottosite, string rdositetojob, string rdoThirdPartytoDepot)
        {
            try
            {
                dbContext.Database.CommandTimeout = 15 * 60;

                int iDisplayLength = param.length > 0 ? param.length : 10;
                int iDisplayStart = param.start;
                string sSearch = (param.search != null && param.search.value != null) ? param.search.value : "";

                int sortBy = 0;
                string sortDir = "asc";
                if (param.order != null && param.order.Count > 0)
                {
                    sortBy = param.order[0].column;
                    sortDir = string.Equals(param.order[0].dir, "desc", StringComparison.OrdinalIgnoreCase) ? "desc" : "asc";
                }

                string query = "EXEC pro_RequestInventory_serverside "
                    + "@rdodepottosite=" + (string.Equals(rdodepottosite, "true", StringComparison.OrdinalIgnoreCase) ? "1" : "0") + ","
                    + "@rdositetojob=" + (string.Equals(rdositetojob, "true", StringComparison.OrdinalIgnoreCase) ? "1" : "0") + ","
                    + "@rdoThirdPartytoDepot=" + (string.Equals(rdoThirdPartytoDepot, "true", StringComparison.OrdinalIgnoreCase) ? "1" : "0") + ","
                    + "@id=" + (string.IsNullOrEmpty(id) ? "NULL" : databaseCon.Decrypt(id)) + ","
                    + "@ddlfrom=" + (string.IsNullOrEmpty(ddlfrom) || ddlfrom == "All" ? "NULL" : "N'" + ddlfrom.Replace("'", "''") + "'") + ","
                    + "@DisplayLength=" + iDisplayLength + ","
                    + "@Displaystart=" + iDisplayStart + ","
                    + "@search=N'" + sSearch.Replace("'", "''") + "',"
                    + "@sortBy=" + sortBy + ","
                    + "@sortDir='" + sortDir + "'";

                var list = dbContext.Database.SqlQuery<RequestInventoryViewModel>(query).ToList();

                int totalCount = (list != null && list.Count > 0) ? list[0].TotalCount : 0;

                return Json(new
                {
                    draw = param.draw,
                    recordsTotal = totalCount,
                    recordsFiltered = totalCount,
                    data = list
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    draw = param != null ? param.draw : 0,
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    data = new List<RequestInventoryViewModel>(),
                    error = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }


        //  protected void btndeleteyes_Click(object sender, EventArgs e)
        public ActionResult btndeleteyes_Click(string ClientID, string transID)
        {
            string query = "";
            DataTable dt;
            // string querydelblob = "select DocumentName from tbldeliverydocketdocuments where TransID = '" + ViewState["designfordelete"] + "' and ClientID = '" + databaseCon.Decrypt(Request.QueryString["id"]) + "'";
            string querydelblob = "select DocumentName from tbldeliverydocketdocuments where TransID = '" + transID + "' and ClientID = '" + databaseCon.Decrypt(ClientID) + "'";
            DataTable dtblob = databaseCon.GetData(querydelblob);
            if (dtblob.Rows.Count > 0)
            {
                for (int i = 0; i < dtblob.Rows.Count; i++)
                {
                    string filename = dtblob.Rows[i]["DocumentName"].ToString();
                    databaseCon.deletefileonblob(filename, "siteimage");    // Need To write this Query
                }
            }
            // query = "exec Pro_InventoryRequestDelete'" + ViewState["designfordelete"] + "','" + databaseCon.Decrypt(Request.QueryString["id"]) + "'";
            query = "exec Pro_InventoryRequestDelete'" + transID + "','" + databaseCon.Decrypt(ClientID) + "'";
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

        //Added on 06-06-2023 to Add the Edit Functionality On Request Inventory List Page To Enable Editing For Supplier,Comment and Description Columns
        public ActionResult btnEdityes_Click(string ClientID, string transID,string Supplier,string TransDesc,string Comment)
        {
            string Result =""; 
            string Message ="";
            string query4 = "update TblTransaction set Supplier = '"+ Supplier.Replace("'","''").Trim() + "', TransDesc = '"+ TransDesc.Replace("'", "''").Trim() + "',Comment = '" + Comment.Replace("'", "''").Trim() + "' where ClientId = " + databaseCon.Decrypt(ClientID) + " and TransId = " + transID + "";
            int i = databaseCon.RunCmd(query4);
            if (i>0)
            {

                Result = "Success";
                Message = "Data Save Successfully";
            
            }
            else {
                Result = "Failed";
                Message = "Syntax Error.Contact Administrator";
            }
            var response = new { Result = Result, Message = Message };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public void ExportToReqCsvButton_Click(string id, string FromId, string ToId, string ddl1from, string ddl1to, string JobSiteid, string ddlfrom, string rdodepottosite, string rdositetojob, string RID, string rdoThirdPartytoDepot)
        {
            int fromid;
            string addquery = "";
            string query = "";
            if (FromId != "" && FromId != "null")
            {
                addquery = " and FromId=" + databaseCon.Decrypt(FromId) + " and ToId=" + databaseCon.Decrypt(ToId);
            }
            if (ddl1from != "" && ddl1from != "All" && ddl1from != "null")
            {
                addquery = " and FromId=" + databaseCon.Decrypt(ddl1from) + " and [From]='Job'";
            }

            if (ddl1to != "" && ddl1to != "All" && ddl1to != "null")
            {
                addquery = addquery + " and ToId=" + databaseCon.Decrypt(ddl1to) + " and [To]='Site'";
            }
            if (JobSiteid != "" && JobSiteid != "null")
            {
                addquery = " and ToId=" + databaseCon.Decrypt(JobSiteid) + " and [To]='Site' and FromId='" + databaseCon.Decrypt(RID) + "'";
            }
            if (ddlfrom != "" && ddlfrom != "All" && ddlfrom != "null")
            {
                addquery = " and FromId=" + ddlfrom + " and [From]='Site'";
                fromid = Convert.ToInt32(databaseCon.Decrypt(ddlfrom));
            }



            if (rdodepottosite == "true")
            {
                addquery = addquery + " and ToId='" + databaseCon.Decrypt(id) + "' and [To]='Depot'";
                //query = "select transId as TransId,Replace(convert(nvarchar,convert(datetime,ToTime,120),106),' ','-')  as 'Delivered Date',Supplier,TransDesc as 'Description', case when [From]='Site' then tblSiteLocation.SiteLocation when [From]='Job' then VW_TblRegister.Description else '' end as 'From',case when[To] = 'Site' then tblSiteLocation.SiteLocation when[To] = 'Depot' then[To] else '' end as 'To',case when RequestStatus='Confirm' then 'Confirmed' else RequestStatus End as 'Request Status', case when DeliveryStatus='Confirm' then 'Confirmed' else DeliveryStatus End as 'Delivery Status' ,FromPerson as Requester,ToPerson as Deliverer,Replace(convert(nvarchar,convert(datetime,FromTime,120),106),' ','-') as 'Requested Date',(Select sum(isnull(quantity,0)) From tblTransactionHistRequest where  tblTransaction.TransId = tblTransactionHistRequest.TransId) as '(Requested) No. of Items', (Convert(Decimal(18, 2), (Select Sum(isnull(Quantity * Itemweight, 0)) / 1000 From tblTransactionHistRequest join tblInventory on tblInventory.InventoryID = tblTransactionHistRequest.InventoryId where tblTransaction.TransId = tblTransactionHistRequest.TransId))) as 'TotalWeight(Tonnes)',LastEmailSentOn,Comment from tblTransaction left join tblSiteLocation on tblTransaction.FromId = tblSiteLocation.SiteLocID and[From] = 'Site' or (tblTransaction.ToId = tblSiteLocation.SiteLocID and[To] = 'Site') left join tblClient on tblClient.ClientId = tblTransaction.ToId and[To] = 'Depot' left join VW_TblRegister on tblTransaction.FromId = VW_TblRegister.RegisterId and[From] = 'Job' left join tblDeliveryDocket on tblTransaction.DeliveryId=tblDeliveryDocket.DeliveryID and tblTransaction.ClientId=tblDeliveryDocket.clientId where 1=1 and tbltransaction.ClientId='" + databaseCon.Decrypt(id) + "'and [To]='Depot'" + addquery + "Order by transId desc,DeliveryStatus";

                query = "select transId as TransId,Replace(convert(nvarchar,convert(datetime,ToTime,120),106),' ','-')  as 'Delivered Date',Supplier,TransDesc as 'Description', case when [From]='Site' then tblSiteLocation.SiteLocation when [From]='Job' then VW_TblRegister.Description else '' end as 'From',case when[To] = 'Site' then tblSiteLocation.SiteLocation when[To] = 'Depot' then[To] else '' end as 'To',case when RequestStatus='Confirm' then 'Confirmed' else RequestStatus End as 'Request Status', case when DeliveryStatus='Confirm' then 'Confirmed' else DeliveryStatus End as 'Delivery Status' ,FromPerson as Requester,ToPerson as Deliverer,Replace(convert(nvarchar,convert(datetime,FromTime,120),106),' ','-') as 'Requested Date',Replace(convert(nvarchar,convert(datetime,RequiredDate,120),106),' ','-') as 'RequiredDate',(Select sum(isnull(quantity,0)) From tblTransactionHistRequest where  tblTransaction.TransId = tblTransactionHistRequest.TransId) as '(Requested) No. of Items', (Convert(Decimal(18, 2), (Select Sum(isnull(Quantity * Itemweight, 0)) / 1000 From tblTransactionHistRequest join tblInventory on tblInventory.InventoryID = tblTransactionHistRequest.InventoryId where tblTransaction.TransId = tblTransactionHistRequest.TransId))) as 'TotalWeight(Tonnes)',LastEmailSentOn,Comment from tblTransaction left join tblSiteLocation on tblTransaction.FromId = tblSiteLocation.SiteLocID and[From] = 'Site' or (tblTransaction.ToId = tblSiteLocation.SiteLocID and[To] = 'Site') left join tblClient on tblClient.ClientId = tblTransaction.ToId and[To] = 'Depot' left join VW_TblRegister on tblTransaction.FromId = VW_TblRegister.RegisterId and[From] = 'Job' left join tblDeliveryDocket on tblTransaction.DeliveryId=tblDeliveryDocket.DeliveryID and tblTransaction.ClientId=tblDeliveryDocket.clientId where 1=1 and tbltransaction.ClientId='" + databaseCon.Decrypt(id) + "'and [To]='Depot'" + addquery + "Order by transId desc,DeliveryStatus";

            }
            else if (rdositetojob == "true")
            {
                //query = "select transId,case when RequestStatus='Confirm' then 'Confirmed' else RequestStatus End as RequestStatus, transtype, FromId,Supplier,TransDesc,case when DeliveryStatus='Confirm' then 'Confirmed' else DeliveryStatus End as DeliveryStatus, case when Description is not null and Description!='' then Right('000000'+Convert(varchar,ScaffRef),6)+' - '+Description else Right('000000'+Convert(varchar,ScaffRef),6) end  as FromName,case when[To] = 'Site' then tblSiteLocation.SiteLocation when[To] = 'Depot' then[To] else '' end as ToName , ToId,[To],FromPerson,ToPerson ,FromTime,TOTime ,(Select sum(isnull(quantity,0)) From tblTransactionHistRequest where tblTransaction.TransId = tblTransactionHistRequest.TransId) as NoofItems, (Select Sum(isnull(Quantity * Itemweight, 0))/1000 From tblTransactionHistRequest join tblInventory on tblInventory.InventoryID = tblTransactionHistRequest.InventoryId where tblTransaction.TransId = tblTransactionHistRequest.TransId) as TotalWeight,Null as ReceiptConfirmation from tblTransaction left join tblSiteLocation on tblTransaction.FromId = tblSiteLocation.SiteLocID and[From] = 'Site' or (tblTransaction.ToId = tblSiteLocation.SiteLocID and[To] = 'Site') left join tblClient on tblClient.ClientId = tblTransaction.ToId and[To] = 'Depot' left join VW_TblRegister on tblTransaction.FromId = VW_TblRegister.RegisterId and[From] = 'Job' where 1=1 and tbltransaction.ClientId='" + databaseCon.Decrypt(id) + "'and [To]<>'Depot'" + addquery + "Order by transId desc, DeliveryStatus";

                query = "select transId,case when RequestStatus='Confirm' then 'Confirmed' else RequestStatus End as RequestStatus, transtype, FromId,Supplier,TransDesc,case when DeliveryStatus='Confirm' then 'Confirmed' else DeliveryStatus End as DeliveryStatus, case when Description is not null and Description!='' then Right('000000'+Convert(varchar,ScaffRef),6)+' - '+Description else Right('000000'+Convert(varchar,ScaffRef),6) end  as FromName,case when[To] = 'Site' then tblSiteLocation.SiteLocation when[To] = 'Depot' then[To] else '' end as ToName , ToId,[To],FromPerson,ToPerson ,FromTime,RequiredDate,TOTime ,(Select sum(isnull(quantity,0)) From tblTransactionHistRequest where tblTransaction.TransId = tblTransactionHistRequest.TransId) as NoofItems, (Select Sum(isnull(Quantity * Itemweight, 0))/1000 From tblTransactionHistRequest join tblInventory on tblInventory.InventoryID = tblTransactionHistRequest.InventoryId where tblTransaction.TransId = tblTransactionHistRequest.TransId) as TotalWeight,Null as ReceiptConfirmation from tblTransaction left join tblSiteLocation on tblTransaction.FromId = tblSiteLocation.SiteLocID and[From] = 'Site' or (tblTransaction.ToId = tblSiteLocation.SiteLocID and[To] = 'Site') left join tblClient on tblClient.ClientId = tblTransaction.ToId and[To] = 'Depot' left join VW_TblRegister on tblTransaction.FromId = VW_TblRegister.RegisterId and[From] = 'Job' where 1=1 and tbltransaction.ClientId='" + databaseCon.Decrypt(id) + "'and [To]<>'Depot'" + addquery + "Order by transId desc, DeliveryStatus";
            }
            else if (rdoThirdPartytoDepot == "true")
            {
                addquery = addquery + " and ToId='" + databaseCon.Decrypt(id) + "' and [To]='ThirdParty'";
                // query = "select transId,case when RequestStatus='Confirm' then 'Confirmed' else RequestStatus End as RequestStatus, transtype, FromId,Supplier,TransDesc,case when DeliveryStatus = 'Confirm' then 'Confirmed' else DeliveryStatus End as DeliveryStatus,[From] as FromName,[To] as ToName,ToId,[To], FromPerson, ToPerson, FromTime, ToTime,(Select sum(isnull(quantity, 0)) From tblTransactionHistRequest where tblTransaction.TransId = tblTransactionHistRequest.TransId) as NoofItems,(Select Sum(isnull(Quantity * Itemweight, 0)) / 1000 From tblTransactionHistRequest join tblInventory on tblInventory.InventoryID = tblTransactionHistRequest.InventoryId where tblTransaction.TransId = tblTransactionHistRequest.TransId) as TotalWeight,ReceiptConfirmation,LastEmailSentOn from tblTransaction left join tblDeliveryDocket on tblTransaction.DeliveryId = tblDeliveryDocket.DeliveryID and tblTransaction.ClientId = tblDeliveryDocket.clientId where 1 = 1 and tbltransaction.ClientId = '" + databaseCon.Decrypt(id) + "' and [To] = 'ThirdParty' " + addquery + " Order by transId desc, DeliveryStatus";

                //query = "select transId, ToTime as DeliveredDate, Supplier,TransDesc as Description,[From] as FromName,[To] as ToName,case when RequestStatus = 'Confirm' then 'Confirmed' else RequestStatus End as RequestStatus,case when DeliveryStatus = 'Confirm' then 'Confirmed' else DeliveryStatus End as DeliveryStatus, FromPerson as Requestor, ToPerson as Deliverer, FromTime as RequestedDate, (Select sum(isnull(quantity, 0)) From tblTransactionHistRequest where tblTransaction.TransId = tblTransactionHistRequest.TransId) as RequestedNoofItems,(Select Sum(isnull(Quantity* Itemweight, 0)) / 1000 From tblTransactionHistRequest join tblInventory on tblInventory.InventoryID = tblTransactionHistRequest.InventoryId where tblTransaction.TransId = tblTransactionHistRequest.TransId) as TotalWeightTonnes,LastEmailSentOn,Comment from tblTransaction left join tblDeliveryDocket on tblTransaction.DeliveryId = tblDeliveryDocket.DeliveryID and tblTransaction.ClientId = tblDeliveryDocket.clientId where 1 = 1 and tbltransaction.ClientId = '" + databaseCon.Decrypt(id) + "' and [To] = 'ThirdParty' " + addquery + " Order by transId desc, DeliveryStatus";

                query = "select transId, ToTime as DeliveredDate, Supplier,TransDesc as Description,[From] as FromName,[To] as ToName,case when RequestStatus = 'Confirm' then 'Confirmed' else RequestStatus End as RequestStatus,case when DeliveryStatus = 'Confirm' then 'Confirmed' else DeliveryStatus End as DeliveryStatus, FromPerson as Requestor, ToPerson as Deliverer, FromTime as RequestedDate,RequiredDate as RequiredDate , (Select sum(isnull(quantity, 0)) From tblTransactionHistRequest where tblTransaction.TransId = tblTransactionHistRequest.TransId) as RequestedNoofItems,(Select Sum(isnull(Quantity* Itemweight, 0)) / 1000 From tblTransactionHistRequest join tblInventory on tblInventory.InventoryID = tblTransactionHistRequest.InventoryId where tblTransaction.TransId = tblTransactionHistRequest.TransId) as TotalWeightTonnes,LastEmailSentOn,Comment from tblTransaction left join tblDeliveryDocket on tblTransaction.DeliveryId = tblDeliveryDocket.DeliveryID and tblTransaction.ClientId = tblDeliveryDocket.clientId where 1 = 1 and tbltransaction.ClientId = '" + databaseCon.Decrypt(id) + "' and [To] = 'ThirdParty' " + addquery + " Order by transId desc, DeliveryStatus";

            }
            DataTable dt = databaseCon.GetData(query);

            Response.ContentType = "Application/x-msexcel";
            Response.AddHeader("content-disposition", "attachment;filename='RequestInventoryList'.csv");
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
                        //sbldr.Append(row[column].ToString().Replace(",", ";") + ',');
                    }
                    sbldr.Append("\r\n");
                }
            }
            return sbldr.ToString();
        }

        public ActionResult btnEmailClick(string id, string UID, string UserRole, string TransId, string DomainName, string TransID1, string StatusTrans, string fromid, string toid, string fromvalue, string tovalue, string fromtocheck, string DelStatus, string chckEmail, string EmailCheckTo)
        {

            var Status = "";
            var Message = "";
            var txtFile = "";
            var txttempfile = "";
            var txtcc = "";
            var txtClientEmail = "";
            var txtsubject = "";
            var msg = "";
            var txtEmailMSg = "";
            var FromId = "";
            var RequestStatus = "";
            var ToId = "";
            var DeliveryStatus = "";
            string Emaillink = "";
            var SiteName = "";
            var Requester = "";
            string transactionId = databaseCon.Decrypt(TransId);

            DataTable dtclientinfo;

            if (chckEmail != "1")
            {
                //Added On 20-01-2024 in case of Navigation from Site Request To Request Page we are getting correct value in TransID and In case of Checking Check Box and clicking on Email Button We are getting Correct Value in TransID1 
                transactionId = databaseCon.Decrypt(TransID1);
                //Ended On 20-01-2024 

                if ((databaseCon.Decrypt(EmailCheckTo) == "ThirdParty" || databaseCon.Decrypt(EmailCheckTo) == "") && databaseCon.Decrypt(tovalue) != "Depot")
                {
                    dtclientinfo = databaseCon.GetData("select ClientInventoryMailingList as InventoryMailingList,SiteLocation from tblClient join tblSiteLocation on tblSiteLocation.ClientId=tblClient.ClientId where tblClient.ClientId = '" + databaseCon.Decrypt(id) + "' ");
                }
                else
                {
                    //Added on (11-01-2024) by S.S We have handeled null in case the InventoryMailingList of Site is blank then we should used InventoryMailingList column from Client table.
                    //dtclientinfo = databaseCon.GetData("select InventoryMailingList,EmailID,SiteLocation from tblSiteLocation join tblClient on tblSiteLocation.ClientId=tblClient.ClientId where SiteLocID='" + databaseCon.Decrypt(fromid) + "'");
                    dtclientinfo = databaseCon.GetData("select COALESCE(NULLIF(InventoryMailingList, ''), ClientInventoryMailingList) AS InventoryMailingList,EmailID,SiteLocation from tblSiteLocation join tblClient on tblSiteLocation.ClientId=tblClient.ClientId where SiteLocID='" + databaseCon.Decrypt(fromid) + "'");
                }

            }
            else
            {
                DataTable dt = databaseCon.GetData("select FromId,RequestStatus,DeliveryStatus,ToId from TblTransaction where TransId='" + databaseCon.Decrypt(TransId) + "'");
                if (dt.Rows.Count > 0)
                {

                    FromId = Convert.ToString(dt.Rows[0]["FromId"]);
                    RequestStatus = Convert.ToString(dt.Rows[0]["RequestStatus"]);
                    DeliveryStatus = Convert.ToString(dt.Rows[0]["DeliveryStatus"]);
                    if (DeliveryStatus == "" || DeliveryStatus == null)
                    {
                        DeliveryStatus = "NotConfirmed";
                    }
                    ToId = Convert.ToString(dt.Rows[0]["ToId"]);
                }
                if ((databaseCon.Decrypt(EmailCheckTo) == "ThirdParty" || databaseCon.Decrypt(EmailCheckTo) == "") && databaseCon.Decrypt(tovalue) != "Depot")
                {
                    dtclientinfo = databaseCon.GetData("select ClientInventoryMailingList as InventoryMailingList,SiteLocation from tblClient join tblSiteLocation on tblSiteLocation.ClientId=tblClient.ClientId where tblClient.ClientId = '" + databaseCon.Decrypt(id) + "' ");
                }
                else
                {
                    //Added on (11-01-2024) by S.S We have handeled null in case the InventoryMailingList of Site is blank then we should used InventoryMailingList column from Client table.
                    //dtclientinfo = databaseCon.GetData("select InventoryMailingList,EmailID,SiteLocation from tblSiteLocation join tblClient on tblSiteLocation.ClientId=tblClient.ClientId where SiteLocID='" + FromId + "'");
                    dtclientinfo = databaseCon.GetData("select COALESCE(NULLIF(InventoryMailingList, ''), ClientInventoryMailingList) AS InventoryMailingList,EmailID,SiteLocation from tblSiteLocation join tblClient on tblSiteLocation.ClientId=tblClient.ClientId where SiteLocID='" + FromId + "'");

                }

            }

            //Added On 20-01-2024 To Add Required Date And Requested Date on Email Pop-Up
            var query2 = "SELECT isnull(fromTime,'01 - Jan - 1900') as RequestedDate, isnull(RequiredDate,'01 - Jan - 1900') as RequiredSDate FROM tbltransaction WHERE transId = @transactionId";

            var parameters2 = new SqlParameter("@transactionId", transactionId);
            var EmailDate = dbContext.Database.SqlQuery<CommonInventoryColumnViewMoidel>(query2, parameters2).FirstOrDefault();
            string RequestedDate = (EmailDate.RequestedDate.HasValue ? EmailDate.RequestedDate.Value.ToString("dd-MMM-yyyy") : "N/A");
            string RequiredDate = (EmailDate.RequiredSDate.HasValue ? EmailDate.RequiredSDate.Value.ToString("dd-MMM-yyyy") : "N/A");
            //Ended On 20-01-2024 

            if (dtclientinfo.Rows.Count > 0)
            {
                SiteName = Convert.ToString(dtclientinfo.Rows[0]["SiteLocation"]);
                txtClientEmail = Convert.ToString(dtclientinfo.Rows[0]["InventoryMailingList"]);
                // txtcc = Convert.ToString(dtclientinfo.Rows[0]["EmailID"]);
            }

            DataTable dtemailinfo = databaseCon.GetData("select Subject,Body from [tblEmailContent] where Purpose = 'Inventory' and clientid='" + databaseCon.Decrypt(id) + "' ");
            if (dtemailinfo.Rows.Count > 0)
            {
                txtsubject = Convert.ToString(dtemailinfo.Rows[0]["Subject"]);
                msg = Convert.ToString(dtemailinfo.Rows[0]["Body"]);

                DataTable dtRequsterInfo = databaseCon.GetData("select EmailId,firstname+lastName as Name from tblUser where UserId='" + databaseCon.Decrypt(UID) + "'");
                if (dtRequsterInfo.Rows.Count > 0)
                {
                    Requester = Convert.ToString(dtRequsterInfo.Rows[0]["Name"]);
                }
            }

            if (chckEmail != "1")
            {
                //Emaillink = DomainName + "/" + "DeliveryInventory/DeliveryInventory?UID=" + UID.Trim().Replace(" ", "+") + "&UserRole=" + UserRole.Trim().Replace(" ", "+") + "&id=" + id.Trim().Replace(" ", "+") + "&checkid=" + TransID1.Trim().Replace(" ", "+") + "&FromId=" + fromid.Trim().Replace(" ", "+") + "&ToId=" + toid.Trim().Replace(" ", "+") + "&fromtocheck=" + fromtocheck.Trim().Replace(" ", "+") + "&from=" + fromvalue.Trim().Replace(" ", "+") + "&To=" + tovalue.Trim().Replace(" ", "+") + "&DelStatus=" + DelStatus.Trim().Replace(" ", "+");
                Emaillink = DomainName + "/" + "PreDeliveryInventory/PreDeliveryInventory?id=" + id.Trim().Replace(" ", "+") + "&checkid=" + TransID1.Trim().Replace(" ", "+") + "&FromId=" + fromid.Trim().Replace(" ", "+") + "&ToId=" + toid.Trim().Replace(" ", "+") + "&fromtocheck=" + fromtocheck.Trim().Replace(" ", "+") + "&from=" + fromvalue.Trim().Replace(" ", "+") + "&To=" + tovalue.Trim().Replace(" ", "+") + "&DelStatus=" + DelStatus.Trim().Replace(" ", "+");
            }
            else
            {
                //Emaillink = DomainName + "/" + "DeliveryInventory/DeliveryInventory?UID=" + UID.Trim().Replace(" ", "+") + "&UserRole=" + UserRole.Trim().Replace(" ", "+") + "&id=" + id.Trim().Replace(" ", "+") + "&checkid=" + TransId.Trim().Replace(" ", "+") + "&FromId=" + databaseCon.Encrypt(FromId).Trim().Replace(" ", "+") + "&ToId=" + databaseCon.Encrypt(ToId).Trim().Replace(" ", "+") + "&fromtocheck=" + databaseCon.Encrypt(fromtocheck).Trim().Replace(" ", "+") + "&from=" + databaseCon.Encrypt(fromvalue).Trim().Replace(" ", "+") + "&To=" + databaseCon.Encrypt(tovalue).Trim().Replace(" ", "+") + "&DelStatus=" + databaseCon.Encrypt(DeliveryStatus).Trim().Replace(" ", "+");

                Emaillink = DomainName + "/" + "PreDeliveryInventory/PreDeliveryInventory?id=" + id.Trim().Replace(" ", "+") + "&checkid=" + TransId.Trim().Replace(" ", "+") + "&FromId=" + databaseCon.Encrypt(FromId).Trim().Replace(" ", "+") + "&ToId=" + databaseCon.Encrypt(ToId).Trim().Replace(" ", "+") + "&fromtocheck=" + databaseCon.Encrypt(fromtocheck).Trim().Replace(" ", "+") + "&from=" + databaseCon.Encrypt(fromvalue).Trim().Replace(" ", "+") + "&To=" + databaseCon.Encrypt(tovalue).Trim().Replace(" ", "+") + "&DelStatus=" + databaseCon.Encrypt(DeliveryStatus).Trim().Replace(" ", "+");
            }

            if ((databaseCon.Decrypt(EmailCheckTo) == "ThirdParty" || databaseCon.Decrypt(EmailCheckTo) == "") && databaseCon.Decrypt(tovalue) != "Depot")
            {
                //Commented on (22-01-2024) We need to add space between SuperAdmin and Message.
                //txtEmailMSg = msg + Environment.NewLine + Environment.NewLine + "Link: " + Emaillink + Environment.NewLine + Environment.NewLine + "From: Depot " + Environment.NewLine + Environment.NewLine + "Requestor: " + Requester.Trim();
                //Ended on (22-01-2024).

                //Added on (22-01-2024) We need to add space between SuperAdmin and Message.
                txtEmailMSg = msg + Environment.NewLine + Environment.NewLine + "Link: " + Emaillink + Environment.NewLine + Environment.NewLine + "From: Depot " + Environment.NewLine + Environment.NewLine + "Requestor: " + Requester.Trim() + ".  ";

                if (RequestedDate != "01-Jan-1900")
                {
                    txtEmailMSg += msg + Environment.NewLine + Environment.NewLine +
                                      "Requested Date: " + (EmailDate.RequestedDate.HasValue ? EmailDate.RequestedDate.Value.ToString("dd-MMM-yyyy") : "N/A");
                }
                if (RequiredDate != "01-Jan-1900")
                {

                    txtEmailMSg += 
                                  Environment.NewLine + Environment.NewLine +
                                  "Required Date: " + (EmailDate.RequiredSDate.HasValue ? EmailDate.RequiredSDate.Value.ToString("dd-MMM-yyyy") : "N/A");
                }
                Status = "Success";
            }
            else
            {
                //Commented on (22-01-2024) We need to add space between SuperAdmin and Message.
                //txtEmailMSg = msg + Environment.NewLine + Environment.NewLine + "Link: " + Emaillink + Environment.NewLine + Environment.NewLine + "Site: " + SiteName + Environment.NewLine + Environment.NewLine + "Requestor: " + Requester.Trim();
                //Ended on (22-01-2024)

                //Added On (22-01-2024) We need to add space between SuperAdmin and Message.
                txtEmailMSg = msg + Environment.NewLine + Environment.NewLine + "Link: " + Emaillink + Environment.NewLine + Environment.NewLine + "Site: " + SiteName + Environment.NewLine + Environment.NewLine + "Requestor: " + Requester.Trim() + ".  ";

                if (RequestedDate != "01-Jan-1900")
                {
                    txtEmailMSg += msg + Environment.NewLine + Environment.NewLine +
                                      "Requested Date: " + (EmailDate.RequestedDate.HasValue ? EmailDate.RequestedDate.Value.ToString("dd-MMM-yyyy") : "N/A");
                }
                if (RequiredDate != "01-Jan-1900")
                {
               
                    txtEmailMSg += 
                                  Environment.NewLine + Environment.NewLine +
                                  "Required Date: " + (EmailDate.RequiredSDate.HasValue ? EmailDate.RequiredSDate.Value.ToString("dd-MMM-yyyy") : "N/A");
                }
                
                Status = "Success";
            }
            // txtEmailMSg += msg + Environment.NewLine + Environment.NewLine + "RequestedDate: " + Format(EmailDate.RequestedDate, 'dd-MMM-yyyy') + Environment.NewLine + Environment.NewLine + "RequiredDate: " + EmailDate.RequiredDate;

            //txtEmailMSg += msg + Environment.NewLine + Environment.NewLine +
            //   "RequestedDate: " + (EmailDate.RequestedDate.HasValue ? EmailDate.RequestedDate.Value.ToString("dd-MMM-yyyy") : "N/A") +
            //   Environment.NewLine + Environment.NewLine +
            //   "RequiredDate: " + (EmailDate.RequiredSDate.HasValue ? EmailDate.RequiredSDate.Value.ToString("dd-MMM-yyyy") : "N/A");

            Status = "Success";

            return Json(new { Result = Status, Message = Message, txtFile = txtFile, txttempfile = txttempfile, txtcc = txtcc, txtClientEmail = txtClientEmail, txtsubject = txtsubject, txtEmailMSg = txtEmailMSg }, JsonRequestBehavior.AllowGet);

        }


        public ActionResult btnSend_Click(string txtClientEmail, string txtcc, string txtsubject, string txtEmailMSg, string txtFile, string EmailTransID , string txtEmailAdditionalText)
        {
            var Result = "";
            var Message = "";
            txtEmailMSg = txtEmailAdditionalText + Environment.NewLine + Environment.NewLine + txtEmailMSg;

            if (databaseCon.SendEmail(txtClientEmail, txtcc, txtsubject, txtEmailMSg, txtFile))
            {
                int i = databaseCon.RunCmd("update TblTransaction set LastEmailSentOn= '" + DateTime.Now + "' where TransId =" + databaseCon.Decrypt(EmailTransID) + " ");
                Result = "Success";
                Message = "Email send Successfully.";

            }
            else
            {
                Result = "Failed";
                Message = "Try Again. ";
            }
            if (System.IO.File.Exists(txtFile))
            {
                try
                {
                    System.IO.File.Delete(txtFile);
                }
                catch { }
            }
            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
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