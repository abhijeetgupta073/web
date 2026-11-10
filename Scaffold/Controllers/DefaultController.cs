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
    public class DefaultController : Controller
    {
        ScaffoldEntities dbcontext = new ScaffoldEntities();
        // GET: Default
        public ActionResult Default()
        {
            int clientId;
           
            if (!string.IsNullOrEmpty(HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["id"]))) && Request.QueryString["id"] != null)
            {
                clientId = Convert.ToInt32(HttpUtility.UrlEncode(databaseCon.Decrypt(Request.QueryString["id"])));
            }
            else
            {
                string query = "select distinct(Name+' '+'('+status+')') as name,ClientId from tblclient where ClientId!='1' Order by name";
                DataTable dt = databaseCon.GetData(query);
                clientId = Convert.ToInt32(dt.Rows[0]["ClientId"]);

            }

           // hdnClientID.Value = Convert.ToString(clientId);

            string ClientQuery = "select ClientImage from tblClient where ClientId = '" + clientId + "'";
            DataTable clientdt = databaseCon.GetData(ClientQuery);

            if (clientdt.Rows.Count > 0)
            {
                if (clientdt.Rows[0]["ClientImage"].ToString() != "")
                {
                    var uri = new Uri(databaseCon.clientimageLink);
                    string imgname = clientdt.Rows[0]["ClientImage"].ToString();
                    if (clientdt.Rows[0]["ClientImage"].ToString() == "")
                    {
                        // imgLogo.Attributes.Add("src", "/images/logo.png");
                        //ViewBag.ClientImage = "/images/logo.png";
                        ViewBag.ClientImage = databaseCon.logoimage;
                    }
                    else
                    {
                        //imgLogo.Attributes.Add("src", (("/images/clientlogo/") + clientdt.Rows[0]["ClientImage"].ToString()));
                        ViewBag.ClientImage = ("/images/clientlogo/") + clientdt.Rows[0]["ClientImage"].ToString();
                    }
                }
                else
                {
                    //imgLogo.Attributes.Add("src", "images/logo.png");
                    //ViewBag.ClientImage = "/images/logo.png";
                    ViewBag.ClientImage = databaseCon.logoimage;
                }
            }
            
            //Total Number of Scaffolds Erected to Date:
            string query1 = "select COUNT(*) as result from VW_TblRegister where ClientId='" + clientId + "' and (DateTagged<>'01/01/1900' and DateTagged is not null)";
            DataTable dt1 = databaseCon.GetData(query1);

            if (dt1.Rows.Count > 0)
            {
               // ErrectedScaffolds.Text = dt1.Rows[0]["result"].ToString();
               ViewBag.ErrectedScaffolds= dt1.Rows[0]["result"].ToString();
            }

            //Total Number of Scaffolds Standing:
            string query2 = "select COUNT(*) as result from VW_TblRegister where ClientId='" + clientId + "' and (DateTagged<>'01/01/1900' and DateTagged is not null) and (DateOfDismantle='01/01/1900' or DateOfDismantle is null)";
            DataTable dt2 = databaseCon.GetData(query2);

            if (dt2.Rows.Count > 0)
            {
                //TextBox1.Text = dt2.Rows[0]["result"].ToString();
                ViewBag.TextBox1 = dt2.Rows[0]["result"].ToString();
            }

            //Total Tonnage Erected to Date:
            string query3 = "select cast(sum(isnull(Tonnage,0))/1000 as decimal(18,2)) as ErrectedTonnage from VW_TblRegister where ClientId='" + clientId + "' and (DateTagged<>'01/01/1900' and DateTagged is not null)";
            DataTable dt3 = databaseCon.GetData(query3);

            if (dt3.Rows.Count > 0)
            {
                //TextBox2.Text = dt3.Rows[0]["ErrectedTonnage"].ToString();
                ViewBag.TextBox2 = dt3.Rows[0]["ErrectedTonnage"].ToString();
            }

            //Total Tonnage Standing:
            string query4 = "select cast(sum(isnull(Tonnage,0))/1000 as decimal(18,2)) as StandingTonnage from VW_TblRegister where ClientId='" + clientId + "' and (DateTagged<>'01/01/1900' and DateTagged is not null) and (DateOfDismantle='01/01/1900' or DateOfDismantle is null)";
            DataTable dt4 = databaseCon.GetData(query4);

            if (dt4.Rows.Count > 0)
            {
                // TextBox3.Text = dt4.Rows[0]["StandingTonnage"].ToString();
                ViewBag.TextBox3 = dt4.Rows[0]["StandingTonnage"].ToString();
            }

            //Average Tonnage Per Scaffold:
            string query5 = "select case when sum(isnull(Tonnage,0))=0 then 0 else cast(sum(isnull(Tonnage,0))/(1000*COUNT(*)) as decimal(18,2)) End as AverageTonnage from VW_TblRegister where ClientId='" + clientId + "' and ( DateTagged<>'01/01/1900' and DateTagged is not null)";
            DataTable dt5 = databaseCon.GetData(query5);

            if (dt5.Rows.Count > 0)
            {
                //TextBox4.Text = dt5.Rows[0]["AverageTonnage"].ToString();
                ViewBag.TextBox4 = dt5.Rows[0]["AverageTonnage"].ToString();
            }

            //Average Hire Days Per Scaffold (to Date):
            string query6 = "select convert(decimal(18),cast(Convert(decimal(18,2),SUM(HireDays))/Convert(decimal(18,2),AVG(Counts)) as float) )as AvgHireDays from(select SUM(isnull(HireDaysToDate, 0)) as HireDays, COUNT(*) as Counts from VW_TblRegister where ClientId = '" + clientId + "' and(DateTagged <> '01/01/1900' or DateTagged is not null)  union all select Sum(isnull(TotalHireDays, 0)) as HireDays, COUNT(*) as Counts from VW_TblRegister where ClientId = '" + clientId + "' and(DateTagged <> '01/01/1900' and DateTagged is not null))AverageDays";
            DataTable dt6 = databaseCon.GetData(query6);

           // TextBox5.Text = dt6.Rows[0]["AvgHireDays"].ToString() + " Days";
            ViewBag.TextBox5 = dt6.Rows[0]["AvgHireDays"].ToString() + " Days";
            //added on 04-Mar-2022
            string qrystandingtonnage = "select case when sum(isnull(Tonnage,0))=0 then 0 else cast(sum(isnull(Tonnage,0))/(1000*COUNT(*)) as decimal(18,2)) End as AverageStandingTonnage from VW_TblRegister where ClientId = '" + clientId + "' and(DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null)";
            DataTable standingtonnage = databaseCon.GetData(qrystandingtonnage);
            ViewBag.TextBoxStandingTon = standingtonnage.Rows[0]["AverageStandingTonnage"].ToString();

            string qrystandingaverage = "select convert(decimal(18),cast(Convert(decimal(18,2),SUM(HireDays))/Convert(decimal(18,2),AVG(Counts)) as float) )as AvgStandingHireDays from(select SUM(isnull(HireDaysToDate, 0)) as HireDays, COUNT(*) as Counts from VW_TblRegister where ClientId = '" + clientId + "' and(DateTagged <> '01/01/1900' and DateTagged is not null) and (DateOfDismantle = '01/01/1900' or DateOfDismantle is null) union all select Sum(isnull(TotalHireDays, 0)) as HireDays, COUNT(*) as Counts from VW_TblRegister where ClientId = '" + clientId + "' and(DateTagged <> '01/01/1900' and DateTagged is not null) and (DateOfDismantle = '01/01/1900' or DateOfDismantle is null))AverageStandingDays";
            DataTable standingaverage = databaseCon.GetData(qrystandingaverage);
            ViewBag.TextBoxStandingAver = standingaverage.Rows[0]["AvgStandingHireDays"].ToString() + " Days";

            return View();
        }
      
        public ActionResult ddlSiteDashboard(string ClientID)
        {
            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select SiteLocID,SiteLocation from dbo.tblSiteLocation where ClientId='" + ClientID + "' order by SiteLocation").ToList();
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }
        List<ScaffoldAllDashBoardViewModel> Dashboard1ddlTonnagebyjob = new List<ScaffoldAllDashBoardViewModel> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod(EnableSession = true)]
        //public List<Scaffold> ddlTonnagejobDashboard(string Siteid, string ClientID)
        public ActionResult ddlTonnagejobDashboard(string Siteid, string ClientID)
        {
            string query = "select Right ('000000' + convert (varchar, [scaffref]),6) as ScaffRef, RegisterId from VW_tblRegister where ClientID = '" + ClientID + "' and('" + Siteid + "' = 'all' or SiteLocId = '" + Siteid + "') and tonnage >0.00  and (DateTagged<>'01/01/1900' and DateTagged is not null) and (DateOfDismantle='01/01/1900' or DateOfDismantle is null) order by RegisterId";

            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ScaffoldAllDashBoardViewModel pro = new ScaffoldAllDashBoardViewModel();
                    if (!(item["ScaffRef"] is DBNull) && item["ScaffRef"].ToString() != "")
                    {
                        pro.ScaffRef = item["ScaffRef"].ToString();
                    }
                    if (!(item["RegisterId"] is DBNull) && item["RegisterId"].ToString() != "")
                    {
                        pro.Registerid = item["RegisterId"].ToString();
                    }

                    Dashboard1ddlTonnagebyjob.Add(pro);
                }
            }
            // return Dashboard1ddlTonnagebyjob;
            return Json(Dashboard1ddlTonnagebyjob, JsonRequestBehavior.AllowGet);
        }

        List<ScaffoldAllDashBoardViewModel> Dashboard1ddlHiredays = new List<ScaffoldAllDashBoardViewModel> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod(EnableSession = true)]
        // public List<Scaffold> ddlHireDaysDashboard(string Siteid, string ClientID)
        public ActionResult ddlHireDaysDashboard(string Siteid, string ClientID)
        {
            string query = "select Right ('000000' + convert(varchar, [scaffref]), 6) as ScaffRef, RegisterId from VW_tblRegister where ClientID = '" + ClientID + "' and('" + Siteid + "' = 'all' or SiteLocId = '" + Siteid + "') and (DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) and HireDaysToDate > 0 order by RegisterId";

            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ScaffoldAllDashBoardViewModel pro = new ScaffoldAllDashBoardViewModel();
                    if (!(item["ScaffRef"] is DBNull) && item["ScaffRef"].ToString() != "")
                    {
                        pro.ScaffRef = item["ScaffRef"].ToString();
                    }
                    if (!(item["RegisterId"] is DBNull) && item["RegisterId"].ToString() != "")
                    {
                        pro.Registerid = item["RegisterId"].ToString();
                    }

                    Dashboard1ddlHiredays.Add(pro);
                }
            }
            // return Dashboard1ddlHiredays;
            return Json(Dashboard1ddlHiredays, JsonRequestBehavior.AllowGet);
        }
        List<ScaffoldAllDashBoardViewModel> ScaffoldAllsites = new List<ScaffoldAllDashBoardViewModel> { };
      //  [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
      //  [WebMethod(EnableSession = true)]
      //  public List<Scaffold> GetAllSites(string ClientID)
         public ActionResult GetAllSites(string ClientID)
        {
            //Change By Vishal 20/11/2018
            string query = "select MaterialType,convert(decimal(18,2),TonnageStanding) as TonnageStanding,TotalTonnage,TonnageStanding/TotalTonnage as Pct from ( select MaterialType,Sum(totalWeight)/1000 as TonnageStanding from vw_jobInventory where ToId in (select RegisterID from tblRegister where ClientId=" + ClientID + " and(DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null)) Group by MaterialType)BasedOnMaterial,(select Sum(totalWeight)/1000 as TotalTonnage from vw_jobInventory where ToId in (select RegisterID from tblRegister where ClientId=" + ClientID + " and(DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null)))ForTotal";
            // string query = "select Case when MaterialType!='' and MaterialType is not null then MaterialType else 'Material Type not assigned to Job' end as MaterialType,convert(decimal(18,2),Sum(isnull(Tonnage,0))/1000) TonnageStanding ,case when totalTonnageStanding >0 then cast(round(convert(decimal(18, 2), Sum(isnull(Tonnage, 0)) / 1000) / totalTonnageStanding, 2) as numeric(36, 2)) else 0 end as Pct,totalTonnageStanding from VW_TblRegister, (select  convert(decimal(18, 2), Sum(isnull(Tonnage, 0)) / 1000) totalTonnageStanding from VW_TblRegister where ClientID = '" + ClientID + "' and(DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null))temp where ClientID = '" + ClientID + "' and(DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) group by Case when MaterialType!='' and MaterialType is not null then MaterialType else 'Material Type not assigned to Job' end ,totalTonnageStanding order by Sum(isnull(Tonnage, 0)) / 1000 desc, Case when MaterialType!='' and MaterialType is not null then MaterialType else 'Material Type not assigned to Job' end";

            //select MaterialType,convert(decimal(18,2),Sum(isnull(Tonnage,0))/1000) TonnageStanding from VW_TblRegister where ClientID='" + ClientID + "' and (DateTagged<>'01/01/1900' and DateTagged is not null) and (DateOfDismantle='01/01/1900' or DateOfDismantle is null) group by MaterialType order by Sum(isnull(Tonnage,0))/1000 desc , MaterialType";
            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ScaffoldAllDashBoardViewModel pro = new ScaffoldAllDashBoardViewModel();
                    if (!(item["MaterialType"] is DBNull) && item["MaterialType"].ToString() != "")
                    {
                        pro.MaterialType = item["MaterialType"].ToString();
                    }
                    if (!(item["TonnageStanding"] is DBNull) && item["TonnageStanding"].ToString() != "")
                    {
                        pro.TonnageStanding = Convert.ToDecimal(item["TonnageStanding"]);
                    }
                    if (!(item["Pct"] is DBNull) && item["Pct"].ToString() != "")
                    {
                        pro.PctAllSite1 = Convert.ToDecimal(item["Pct"]);
                    }

                    ScaffoldAllsites.Add(pro);
                }
            }
            // return ScaffoldAllsites;
            return Json(ScaffoldAllsites,JsonRequestBehavior.AllowGet);
          
        }


        List<ScaffoldAllDashBoardViewModel> ScaffoldBysites = new List<ScaffoldAllDashBoardViewModel> { };
        // [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        // [WebMethod(EnableSession = true)]
        // public List<Scaffold> GetBySites(string Siteid, string ClientID)
        public ActionResult GetBySites(string Siteid, string ClientID)
        {
            //Change By Vishal 20/11/2018
            string query = "select MaterialType,convert(decimal(18,2),TonnageStanding) as TonnageStanding,TotalTonnage,TonnageStanding/TotalTonnage as Pct from ( select MaterialType,Sum(totalWeight)/1000 as TonnageStanding from vw_jobInventory where ToId in (select RegisterID from tblRegister where ClientId=" + ClientID + " and('" + Siteid + "' = 'all' or SiteLocId = '" + Siteid + "') and(DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null)) Group by MaterialType)BasedOnMaterial,(select Sum(totalWeight)/1000 as TotalTonnage from vw_jobInventory where ToId in (select RegisterID from tblRegister where ClientId=" + ClientID + " and('" + Siteid + "' = 'all' or SiteLocId = '" + Siteid + "') and(DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null)))ForTotal";

            //string query= "select Case when MaterialType!='' and MaterialType is not null then MaterialType else 'Material Type not assigned to Job' end as MaterialType,convert(decimal(18,2),Sum(isnull(Tonnage,0))/1000) TonnageStanding ,case when totalTonnageStanding>0 then cast(round(convert(decimal(18,2),Sum(isnull(Tonnage,0))/1000)/totalTonnageStanding,2) as numeric(36,2)) else 0 end as Pct,totalTonnageStanding from VW_TblRegister, (select  convert(decimal(18, 2), Sum(isnull(Tonnage, 0)) / 1000) totalTonnageStanding from VW_TblRegister where ClientID = '" + ClientID + "' and('" + Siteid + "' = 'all' or SiteLocId = '" + Siteid + "')  and(DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) )temp where ClientID = '" + ClientID + "' and('" + Siteid + "' = 'all' or SiteLocId = '" + Siteid + "')  and(DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) group by Case when MaterialType!='' and MaterialType is not null then MaterialType else 'Material Type not assigned to Job' end,totalTonnageStanding order by Sum(isnull(Tonnage, 0)) / 1000 desc, Case when MaterialType!='' and MaterialType is not null then MaterialType else 'Material Type not assigned to Job' end";

            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ScaffoldAllDashBoardViewModel pro = new ScaffoldAllDashBoardViewModel();
                    if (!(item["MaterialType"] is DBNull) && item["MaterialType"].ToString() != "")
                    {
                        pro.MaterialType = item["MaterialType"].ToString();
                    }
                    if (!(item["TonnageStanding"] is DBNull) && item["TonnageStanding"].ToString() != "")
                    {
                        pro.TonnageStanding = Convert.ToDecimal(item["TonnageStanding"]);
                    }
                    if (!(item["Pct"] is DBNull) && item["Pct"].ToString() != "")
                    {
                        pro.TonnagebySitePct = Convert.ToDecimal(item["Pct"]);
                    }
                    ScaffoldBysites.Add(pro);
                }
            }
            // return ScaffoldBysites;
            return Json(ScaffoldBysites, JsonRequestBehavior.AllowGet);
        }
       
        List<ScaffoldAllDashBoardViewModel> TonnageAllSites = new List<ScaffoldAllDashBoardViewModel> { };
       // [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
       // [WebMethod(EnableSession = true)]
        public ActionResult GetTonnageAllSites(string ClientID)
        {
            string query = "select SiteLocation, convert(decimal(18, 2), Sum(isnull(Tonnage, 0)) / 1000) TonnageStanding ,case when totalTonnageStanding>0 then cast(round(convert(decimal(18, 2), Sum(isnull(Tonnage, 0)) / 1000) / totalTonnageStanding, 2) as numeric(36, 2)) else 0 end as Pct,totalTonnageStanding from VW_TblRegister, (select  convert(decimal(18, 2), Sum(isnull(Tonnage, 0)) / 1000) totalTonnageStanding from VW_TblRegister where ClientID = '" + ClientID + "' and(DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null)  )temp where ClientID = '" + ClientID + "' and(DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) group by SiteLocation,totalTonnageStanding order by Sum(isnull(Tonnage, 0)) / 1000 desc, SiteLocation";

            //select SiteLocation,convert(decimal(18,2),Sum(isnull(Tonnage,0))/1000) TonnageStanding from VW_TblRegister where ClientID='" + ClientID + "' and (DateTagged<>'01/01/1900' and DateTagged is not null) and (DateOfDismantle='01/01/1900' or DateOfDismantle is null) group by SiteLocation order by Sum(isnull(Tonnage,0))/1000 desc, SiteLocation";
            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ScaffoldAllDashBoardViewModel pro = new ScaffoldAllDashBoardViewModel();
                    if (!(item["SiteLocation"] is DBNull) && item["SiteLocation"].ToString() != "")
                    {
                        pro.MaterialType = item["SiteLocation"].ToString();
                    }
                    if (!(item["TonnageStanding"] is DBNull) && item["TonnageStanding"].ToString() != "")
                    {
                        pro.TonnageStanding = Convert.ToDecimal(item["TonnageStanding"]);
                    }
                    if (!(item["Pct"] is DBNull) && item["Pct"].ToString() != "")
                    {
                        pro.TonnageAllsitePct = Convert.ToDecimal(item["Pct"]);
                    }
                    if (!(item["totalTonnageStanding"] is DBNull) && item["totalTonnageStanding"].ToString() != "")
                    {
                        pro.TotalTonnageAllsite = Convert.ToDecimal(item["totalTonnageStanding"]);
                    }
                    TonnageAllSites.Add(pro);
                }
            }
            // return TonnageAllSites;
            return Json(TonnageAllSites, JsonRequestBehavior.AllowGet);
        }
       
         List<ScaffoldAllDashBoardViewModel> TonnageByJob = new List<ScaffoldAllDashBoardViewModel> { };
        // [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        // [WebMethod(EnableSession = true)]
        // public List<Scaffold> GetTonnageByJob(string Siteid, string Registerid, string ClientID)
        public ActionResult GetTonnageByJob(string Siteid, string Registerid, string ClientID)
        {
            string query;
            query = "select Right ('000000' + convert (varchar, [scaffref]),6) as Description,convert(decimal(18,2),Sum(isnull(Tonnage,0))/1000) TonnageStanding ,case when totalTonnageStanding>0 then cast(round(convert(decimal(18,2),Sum(isnull(Tonnage,0))/1000)/totalTonnageStanding,2) as numeric(36,2)) else 0 end as Pct,totalTonnageStanding from VW_TblRegister, (select  convert(decimal(18,2),Sum(isnull(Tonnage,0))/1000) totalTonnageStanding from VW_TblRegister  where ClientID = '" + ClientID + "'  and ('" + Siteid + "'='all'  or SiteLocId='" + Siteid + "' ) and ('" + Registerid + "'='all'  or RegisterId='" + Registerid + "') and (DateTagged<>'01/01/1900' and DateTagged is not null) and (DateOfDismantle='01/01/1900' or DateOfDismantle is null)  )temp where ClientID = '" + ClientID + "'  and ('" + Siteid + "'='all'  or SiteLocId='" + Siteid + "' ) and ('" + Registerid + "'='all'  or RegisterId='" + Registerid + "') and (DateTagged<>'01/01/1900' and DateTagged is not null) and (DateOfDismantle='01/01/1900' or DateOfDismantle is null) group by scaffref,totalTonnageStanding order by Sum(isnull(Tonnage,0))/1000 desc,scaffref";

            //select top 20 Right ('000000' + convert (varchar, [scaffref]),6) as Description,convert(decimal(18,2),Sum(isnull(Tonnage,0))/1000) TonnageStanding from VW_TblRegister where ClientID = '" + ClientID + "'  and ('" + Siteid + "'='all'  or SiteLocId='" + Siteid + "' ) and ('" + Registerid + "'='all'  or RegisterId='" + Registerid + "') and (DateTagged<>'01/01/1900' and DateTagged is not null) and (DateOfDismantle='01/01/1900' or DateOfDismantle is null) group by scaffref order by Sum(isnull(Tonnage,0))/1000 desc,scaffref";


            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ScaffoldAllDashBoardViewModel pro = new ScaffoldAllDashBoardViewModel();
                    if (!(item["Description"] is DBNull) && item["Description"].ToString() != "")
                    {
                        pro.Description = item["Description"].ToString();
                    }
                    if (!(item["TonnageStanding"] is DBNull) && item["TonnageStanding"].ToString() != "")
                    {
                        pro.TonnageStanding = Convert.ToDecimal(item["TonnageStanding"]);
                    }
                    if (!(item["Pct"] is DBNull) && item["Pct"].ToString() != "")
                    {
                        pro.TonnageByjobPct = Convert.ToDecimal(item["Pct"]);
                    }
                    if (!(item["totalTonnageStanding"] is DBNull) && item["totalTonnageStanding"].ToString() != "")
                    {
                        pro.TotalTonnageByjob = Convert.ToDecimal(item["totalTonnageStanding"]);
                    }

                    TonnageByJob.Add(pro);
                }
            }
            // return TonnageByJob;
            //  return Json(TonnageByJob, JsonRequestBehavior.AllowGet);
            var jsonResult = Json(TonnageByJob, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue; // Setting the maximum JSON length to maximum possible value
            return jsonResult;
        }
       
        List<ScaffoldAllDashBoardViewModel> Hireperjob = new List<ScaffoldAllDashBoardViewModel> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod(EnableSession = true)]
        //public List<Scaffold> GetHireperjob(string Siteid, string Registerid, string ClientID)
        public ActionResult GetHireperjob(string Siteid, string Registerid, string ClientID)
        {
            string query;
            query = "select Right ('000000' + convert (varchar, [scaffref]),6) as Description,Sum(isnull(HireDaysToDate,0)) as HireDays,Convert(decimal(18,0),Round(Convert(decimal(18, 2),TotalDays)/Convert(decimal(18, 2),TotalRegister),0)) as AvgHireDays from VW_TblRegister, (select  Sum(isnull(HireDaysToDate, 0)) as totalDays, COUNT(*) as TotalRegister  from VW_TblRegister  where ClientID = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and('" + Registerid + "' = 'all'  or RegisterId = '" + Registerid + "') and(DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null)) as temp   where ClientID = '" + ClientID + "' and('" + Siteid + "' = 'all'  or SiteLocId = '" + Siteid + "') and('" + Registerid + "' = 'all'  or RegisterId = '" + Registerid + "') and(DateTagged <> '01/01/1900' and DateTagged is not null) and(DateOfDismantle = '01/01/1900' or DateOfDismantle is null) group by scaffref ,TotalDays,TotalRegister order by Sum(isnull(HireDaysToDate, 0)) desc, scaffref";


            //select top 20 Right ('000000' + convert (varchar, [scaffref]),6) as Description,Sum(isnull(HireDaysToDate,0)) as HireDays from VW_TblRegister where ClientID = '" + ClientID + "' and ('" + Siteid + "'='all'  or SiteLocId='" + Siteid + "' ) and ('" + Registerid + "'='all'  or RegisterId='" + Registerid + "') and (DateTagged<>'01/01/1900' and DateTagged is not null) and (DateOfDismantle='01/01/1900' or DateOfDismantle is null) group by scaffref order by Sum(isnull(HireDaysToDate,0)) desc, scaffref";

            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ScaffoldAllDashBoardViewModel pro = new ScaffoldAllDashBoardViewModel();
                    if (!(item["Description"] is DBNull) && item["Description"].ToString() != "")
                    {
                        pro.Description = item["Description"].ToString();
                    }
                    if (!(item["HireDays"] is DBNull) && item["HireDays"].ToString() != "")
                    {
                        pro.HireDays = Convert.ToDecimal(item["HireDays"]);
                    }
                    if (!(item["AvgHireDays"] is DBNull) && item["AvgHireDays"].ToString() != "")
                    {
                        pro.AvgHireDays = item["AvgHireDays"].ToString();
                    }
                    Hireperjob.Add(pro);
                }
            }
            //  return Hireperjob;
            //return Json(Hireperjob, JsonRequestBehavior.AllowGet);
            var jsonResult = Json(Hireperjob, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue; // Setting the maximum JSON length to maximum possible value
            return jsonResult;
        }
       
    }
}