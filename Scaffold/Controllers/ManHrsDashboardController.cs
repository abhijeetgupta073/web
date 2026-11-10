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
    public class ManHrsDashboardController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: ManHrsDashboard
        public ActionResult ManHrsDashboard()
        {
            string ClientQuery = "select  weekStartDay,ClientImage from tblClient where ClientId = '" + databaseCon.Decrypt(Request.QueryString["id"]) + "'";
            //string ClientQuery = "select  weekStartDay,ClientImage from tblClient where ClientId = '" + hdnClientID.Value+ "'";
            DataTable clientdt = databaseCon.GetData(ClientQuery);

            if (clientdt.Rows.Count > 0)
            {
                ViewBag.hdnweekday = clientdt.Rows[0]["weekStartDay"].ToString();
                if (clientdt.Rows[0]["ClientImage"].ToString() != "")
                {
                    var uri = new Uri(databaseCon.clientimageLink);
                    string imgname = clientdt.Rows[0]["ClientImage"].ToString();
                    // var container = new CloudBlobContainer(uri);
                    // var blob = container.GetBlockBlobReference(clientdt.Rows[0]["ClientImage"].ToString());
                    //string a = databaseCon.siteimagelink + "Item1.png";
                    // imga.Attributes.Add("src", databaseCon.siteimagelink + "Item1.png");
                    // var exists = blob.Exists();
                    if (clientdt.Rows[0]["ClientImage"].ToString() == "")
                    {
                        // imgLogo.Attributes.Add("src", "/images/logo.png");
                        // imgLogo1.Attributes.Add("src", "/images/logo.png");
                        //ViewBag.ClientImage = "/images/logo.png";
                        //ViewBag.ClientImage1 = "/images/logo.png";
                        ViewBag.ClientImage = databaseCon.logoimage;
                        ViewBag.ClientImage1 = databaseCon.logoimage;

                    }
                    else
                    {
                        //  string a = databaseCon.clientimageLink + clientdt.Rows[0]["ClientImage"].ToString();
                        //imgLogo.Attributes.Add("src", databaseCon.clientimageLink + clientdt.Rows[0]["ClientImage"].ToString());

                        //imgLogo.Attributes.Add("src", (("/images/clientlogo/") + clientdt.Rows[0]["ClientImage"].ToString()));
                        //imgLogo1.Attributes.Add("src", (("/images/clientlogo/") + clientdt.Rows[0]["ClientImage"].ToString()));
                        ViewBag.ClientImage = "/images/clientlogo/" + clientdt.Rows[0]["ClientImage"].ToString();
                        ViewBag.ClientImage1 = "/images/clientlogo/" + clientdt.Rows[0]["ClientImage"].ToString();
                    }
                }
                else
                {
                    //imgLogo.Attributes.Add("src", "images/logo.png");
                    //imgLogo1.Attributes.Add("src", "images/logo.png");
                    //ViewBag.ClientImage = "/images/logo.png";
                    //ViewBag.ClientImage1 = "/images/logo.png";
                    ViewBag.ClientImage = databaseCon.logoimage;
                    ViewBag.ClientImage1 = databaseCon.logoimage;
                }
            }

            return View();
        }

        public ActionResult SiteDropDownBind(string id)
        {
            var GetData = dbContext.Database.SqlQuery<DropDownViewModel>("select tblManHrs.SiteLocID , SiteLocation from tblManHrs join tblSiteLocation on tblManHrs.SiteLocId=tblSiteLocation.SiteLocID where ClientId='" + id + "' Group by tblManHrs.SiteLocID, SiteLocation").ToList();
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ddlJobDashboard(string id)
        {
            var GetData = dbContext.Database.SqlQuery<CommonPageViewModel>("select distinct Description,Right('000000'+Convert(varchar,Scaffref),6) as Scaffref from tblregister where Description <> '' and ClientID='" + id +"'").ToList();
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ddlCategoryBind(string id)
        {
            var GetData = dbContext.Database.SqlQuery<CommonPageViewModel>("select distinct TypeofVariation from tblManHrs where TypeofVariation <> '' and TypeofVariation is not null").ToList();
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }
        public class CommonPageViewModel
        {
            public string Description { get; set; }
            public string Scaffref { get; set; }
            public string TypeofVariation { get; set; }
        }


        List<ScaffoldAllDashBoardViewModel> ManHrsAll = new List<ScaffoldAllDashBoardViewModel> { };
       // [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
       // [WebMethod(EnableSession = true)]
        //public List<Scaffold> GetAllManHrs(string ClientID, string fromdate, string todate)
        public ActionResult GetAllManHrs(string ClientID, string fromdate, string todate)
        {
            string query1;
            query1 = "select Sum(Isnull(AdditionalHrs, 0)) as AdditionalHrs, Sum(Isnull(ModificationHrs, 0)) as ModificationHrs, Sum(Isnull(DismantleHrs, 0)) as DismantleHrs, Sum(Isnull(ErectionHrs, 0)) as ErectionHrs, Sum(Isnull(ModificationHrs, 0) + Isnull(ErectionHrs, 0) + Isnull(DismantleHrs, 0) + Isnull(AdditionalHrs, 0)) as Total  from tblManHrs join tblRegister on tblManHrs.RegisterId = tblRegister.RegisterId  where tblregister.ClientID='" + ClientID + "'and tblManHrs.EntryDate >= '" + fromdate + "' and tblManHrs.EntryDate <= '" + todate + "'";
            DataSet ds = null;
            ds = databaseCon.GetDataSet(query1);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ScaffoldAllDashBoardViewModel pro = new ScaffoldAllDashBoardViewModel();

                    if (!(item["AdditionalHrs"] is DBNull) && item["AdditionalHrs"].ToString() != "")
                    {
                        pro.AllAdditionalHrs = Convert.ToDecimal(item["AdditionalHrs"].ToString());
                    }
                    if (!(item["ModificationHrs"] is DBNull) && item["ModificationHrs"].ToString() != "")
                    {
                        pro.AllModificationHrs = Convert.ToDecimal(item["ModificationHrs"]);
                    }
                    if (!(item["DismantleHrs"] is DBNull) && item["DismantleHrs"].ToString() != "")
                    {
                        pro.AllDismantleHrs = Convert.ToDecimal(item["DismantleHrs"]);
                    }
                    if (!(item["ErectionHrs"] is DBNull) && item["ErectionHrs"].ToString() != "")
                    {
                        pro.AllErectionHrs = Convert.ToDecimal(item["ErectionHrs"]);
                    }
                    if (!(item["Total"] is DBNull) && item["Total"].ToString() != "")
                    {
                        pro.total = Convert.ToDecimal(item["Total"]);
                    }

                    ManHrsAll.Add(pro);
                }
            }
            // return ManHrsAll;
            return Json(ManHrsAll, JsonRequestBehavior.AllowGet);
        }

        List<ScaffoldAllDashBoardViewModel> ManHrsBySite = new List<ScaffoldAllDashBoardViewModel> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod(EnableSession = true)]
        //public List<Scaffold> GetBySitesManHrs(string id, string ClientID, string fromdate, string todate)
        public ActionResult GetBySitesManHrs(string id, string ClientID, string fromdate, string todate)
        {
            string query1;
            if (id == "All")
            {
                query1 = "select tblManHrs.SiteLocID , SiteLocation,Sum(Isnull(ErectionHrs,0)) as ErectionHrs,Sum(Isnull(ModificationHrs,0)) as ModificationHrs,Sum(Isnull(DismantleHrs,0)) as DismantleHrs,Sum(Isnull(AdditionalHrs,0)) as AdditionalHrs from tblManHrs join tblSiteLocation on tblManHrs.SiteLocId=tblSiteLocation.SiteLocID where ClientId='" + ClientID + "' and EntryDate >= '" + fromdate + "' and EntryDate <= '" + todate + "'Group by tblManHrs.SiteLocID, SiteLocation";
            }
            else
            {
                query1 = "select tblManHrs.SiteLocID , SiteLocation,Sum(Isnull(ErectionHrs,0)) as ErectionHrs,Sum(Isnull(ModificationHrs,0)) as ModificationHrs,Sum(Isnull(DismantleHrs,0)) as DismantleHrs,Sum(Isnull(AdditionalHrs,0)) as AdditionalHrs from tblManHrs join tblSiteLocation on tblManHrs.SiteLocId=tblSiteLocation.SiteLocID where ClientId='" + ClientID + "' and EntryDate >= '" + fromdate + "' and EntryDate <= '" + todate + "' and tblManHrs.SiteLocID='" + id + "' Group by tblManHrs.SiteLocID, SiteLocation";
            }

            DataSet ds = null;
            ds = databaseCon.GetDataSet(query1);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ScaffoldAllDashBoardViewModel pro = new ScaffoldAllDashBoardViewModel();
                    if (!(item["SiteLocation"] is DBNull) && item["SiteLocation"].ToString() != "")
                    {
                        pro.SiteLocation = item["SiteLocation"].ToString();
                    }
                    if (!(item["ErectionHrs"] is DBNull) && item["ErectionHrs"].ToString() != "")
                    {
                        pro.ErectionHrs = Convert.ToDecimal(item["ErectionHrs"]);
                    }
                    if (!(item["ModificationHrs"] is DBNull) && item["ModificationHrs"].ToString() != "")
                    {
                        pro.ModificationHrs = Convert.ToDecimal(item["ModificationHrs"]);
                    }
                    if (!(item["DismantleHrs"] is DBNull) && item["DismantleHrs"].ToString() != "")
                    {
                        pro.DismantleHrs = Convert.ToDecimal(item["DismantleHrs"]);
                    }
                    if (!(item["AdditionalHrs"] is DBNull) && item["AdditionalHrs"].ToString() != "")
                    {
                        pro.AdditionalHrs = Convert.ToDecimal(item["AdditionalHrs"]);
                    }

                    ManHrsBySite.Add(pro);
                }
            }
            //return ManHrsBySite;
            return Json(ManHrsBySite, JsonRequestBehavior.AllowGet);
        }

        List<ScaffoldAllDashBoardViewModel> ManHrsByJob = new List<ScaffoldAllDashBoardViewModel> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod(EnableSession = true)]
       // public List<Scaffold> GetByJobManHrs(string id, string ClientID, string fromdate, string todate)
        public ActionResult GetByJobManHrs(string id, string ClientID, string fromdate, string todate)
        {
            string query1;
            if (id == "All")
            {
                query1 = "select tblManHrs.RegisterId,Right('000000'+Convert(varchar,Scaffref),6) as Scaffref , Description,Sum(Isnull(ErectionHrs,0)) as ErectionHrs,Sum(Isnull(ModificationHrs,0)) as ModificationHrs,Sum(Isnull(DismantleHrs,0)) as DismantleHrs,Sum(Isnull(AdditionalHrs,0)) as AdditionalHrs from tblManHrs join tblRegister on tblManHrs.RegisterId = tblRegister.RegisterId where  ClientId = '" + ClientID + "' and EntryDate >= '" + fromdate + "' and EntryDate <= '" + todate + "' Group by tblManHrs.RegisterId,Scaffref, Description";
            }
            else
            {
                query1 = "select tblManHrs.RegisterId,Right('000000'+Convert(varchar,Scaffref),6) as Scaffref , Description,Sum(Isnull(ErectionHrs,0)) as ErectionHrs,Sum(Isnull(ModificationHrs,0)) as ModificationHrs,Sum(Isnull(DismantleHrs,0)) as DismantleHrs,Sum(Isnull(AdditionalHrs,0)) as AdditionalHrs from tblManHrs join tblRegister on tblManHrs.RegisterId = tblRegister.RegisterId where ClientId = '" + ClientID + "' and EntryDate >= '" + fromdate + "' and EntryDate <= '" + todate + "' and Scaffref='" + id + "' Group by tblManHrs.RegisterId,Scaffref, Description";
            }

            DataSet ds = null;
            ds = databaseCon.GetDataSet(query1);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ScaffoldAllDashBoardViewModel pro = new ScaffoldAllDashBoardViewModel();
                    if (!(item["Scaffref"] is DBNull) && item["Scaffref"].ToString() != "")
                    {
                        pro.Scaffref = Convert.ToString(item["Scaffref"]);
                    }
                    if (!(item["ErectionHrs"] is DBNull) && item["ErectionHrs"].ToString() != "")
                    {
                        pro.ErectionHrs = Convert.ToDecimal(item["ErectionHrs"]);
                    }
                    if (!(item["ModificationHrs"] is DBNull) && item["ModificationHrs"].ToString() != "")
                    {
                        pro.ModificationHrs = Convert.ToDecimal(item["ModificationHrs"]);
                    }
                    if (!(item["DismantleHrs"] is DBNull) && item["DismantleHrs"].ToString() != "")
                    {
                        pro.DismantleHrs = Convert.ToDecimal(item["DismantleHrs"]);
                    }
                    if (!(item["AdditionalHrs"] is DBNull) && item["AdditionalHrs"].ToString() != "")
                    {
                        pro.AdditionalHrs = Convert.ToDecimal(item["AdditionalHrs"]);
                    }

                    ManHrsByJob.Add(pro);
                }
            }
            //return ManHrsByJob;
            return Json(ManHrsByJob, JsonRequestBehavior.AllowGet);
        }

        List<ScaffoldAllDashBoardViewModel> ManHrsByCategory = new List<ScaffoldAllDashBoardViewModel> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod(EnableSession = true)]
        // public List<Scaffold> GetByCategoryManHrs(string id, string ClientID, string fromdate, string todate)
        public ActionResult GetByCategoryManHrs(string id, string ClientID, string fromdate, string todate)
        {
            string query1;
            if (id == "All")
            {
                query1 = "select TypeofVariation, Sum(Isnull(AdditionalHrs, 0)) as AdditionalHrs from tblManHrs join tblRegister on tblManHrs.RegisterId = tblRegister.RegisterId where ClientId = '" + ClientID + "' and EntryDate >= convert(varchar(11),'" + fromdate + "',106) and EntryDate <= convert(varchar(11),'" + todate + "',106) and TypeofVariation is not null Group by TypeofVariation";
            }
            else
            {
                query1 = "select TypeofVariation, Sum(Isnull(AdditionalHrs, 0)) as AdditionalHrs from tblManHrs join tblRegister on tblManHrs.RegisterId = tblRegister.RegisterId where ClientId = '" + ClientID + "' and EntryDate >= convert(varchar(11),'" + fromdate + "',106) and EntryDate <= convert(varchar(11),'" + todate + "',106) and TypeofVariation is not null and TypeofVariation='" + id + "'  Group by TypeofVariation";
            }

            DataSet ds = null;
            ds = databaseCon.GetDataSet(query1);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ScaffoldAllDashBoardViewModel pro = new ScaffoldAllDashBoardViewModel();
                    if (!(item["TypeofVariation"] is DBNull) && item["TypeofVariation"].ToString() != "")
                    {
                        pro.TypeofVariation = Convert.ToString(item["TypeofVariation"]);
                    }
                    if (!(item["AdditionalHrs"] is DBNull) && item["AdditionalHrs"].ToString() != "")
                    {
                        pro.AdditionalHrs = Convert.ToDecimal(item["AdditionalHrs"]);
                    }

                    ManHrsByCategory.Add(pro);
                }
            }
            //return ManHrsByCategory;
            return Json(ManHrsByCategory, JsonRequestBehavior.AllowGet);
        }

        List<ScaffoldAllDashBoardViewModel> ManHrsByLine = new List<ScaffoldAllDashBoardViewModel> { };
        // [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        // [WebMethod(EnableSession = true)]
        //public List<Scaffold> GetByLineManHrs(string ClientID, string fromdate, string todate)
        public ActionResult GetByLineManHrs(string ClientID, string fromdate, string todate)
        {
            string query1;

            // Commented this peice because it was calculating the NoOfWorkedHrs Wrongly
            //query1 = "select CONVERT(varchar(11),tblManHrs.EntryDate,106) as EntryDate ,sum(isnull(AdditionalHrs,0) + isnull(ErectionHrs,0) + isnull(ModificationHrs,0) + isnull(DismantleHrs,0))as SpentHours,(sum(isnull(NoOfWorkedHrs,0))) as NoOfHrs from tblSiteManHrs join tblUser On tblSiteManHrs.UserId = tblUser.UserId join tblSiteLocation On tblSiteManHrs.SiteLocId = tblSiteLocation.SiteLocID left join tblManHrs on (tblSiteManHrs.UserId = tblManHrs.UserId and tblSiteManHrs.EntryDate = tblManHrs.EntryDate  and tblSiteManHrs.SiteLocId = tblManHrs.SiteLocId ) join tblRegister on   tblRegister.RegisterId = tblManHrs.RegisterId where tblUser.ClientID='" + ClientID + "' and tblManHrs.EntryDate >= '" + fromdate + "' and tblManHrs.EntryDate <= '" + todate + "' group by tblmanhrs.EntryDate";

            query1 = "select CONVERT(varchar(11),tblManHrs.EntryDate,106) as EntryDate ,sum(isnull(AdditionalHrs,0) + isnull(ErectionHrs,0) + isnull(ModificationHrs,0) + isnull(DismantleHrs,0))as SpentHours, /*(sum(isnull(NoOfWorkedHrs,0))) as NoOfHrs*/ /*Commented this peice because it was calculating the NoOfWorkedHrs Wrongly*/ ((sum(isnull(tblRegister.ManHrsDismantle,0))) + (sum(isnull(tblRegister.ManHrsErect,0)))) as NoOfHrs from tblSiteManHrs join tblUser On tblSiteManHrs.UserId = tblUser.UserId join tblSiteLocation On tblSiteManHrs.SiteLocId = tblSiteLocation.SiteLocID left join tblManHrs on (tblSiteManHrs.UserId = tblManHrs.UserId and tblSiteManHrs.EntryDate = tblManHrs.EntryDate  and tblSiteManHrs.SiteLocId = tblManHrs.SiteLocId ) join tblRegister on   tblRegister.RegisterId = tblManHrs.RegisterId where tblUser.ClientID='" + ClientID + "' and tblManHrs.EntryDate >= '" + fromdate + "' and tblManHrs.EntryDate <= '" + todate + "' group by tblmanhrs.EntryDate";


            DataSet ds = null;
            ds = databaseCon.GetDataSet(query1);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ScaffoldAllDashBoardViewModel pro = new ScaffoldAllDashBoardViewModel();

                    if (!(item["EntryDate"] is DBNull) && item["EntryDate"].ToString() != "")
                    {
                        pro.EntryDate = (item["EntryDate"].ToString());
                    }
                    if (!(item["SpentHours"] is DBNull) && item["SpentHours"].ToString() != "")
                    {
                        pro.SpentHours = Convert.ToDecimal(item["SpentHours"]);
                    }
                    if (!(item["NoOfHrs"] is DBNull) && item["NoOfHrs"].ToString() != "")
                    {
                        pro.NoOfHrs = Convert.ToDecimal(item["NoOfHrs"]);
                    }

                    ManHrsByLine.Add(pro);
                }
            }
           // return ManHrsByLine;
            return Json(ManHrsByLine, JsonRequestBehavior.AllowGet);
        }

    }

}