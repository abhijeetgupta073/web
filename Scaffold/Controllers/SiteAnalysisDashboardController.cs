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
    public class SiteAnalysisDashboardController : Controller
    {
        ScaffoldEntities dbcontext = new ScaffoldEntities();
        // GET: SiteAnalysisDashboard
        public ActionResult SiteAnalysisDashboard()
        {
            return View();
        }

        public ActionResult BindallDropdown(string id, string UID)
        {
            var SiteOwnerName = "";
            string query1 = "select SiteOwnerName from tblUser where ClientId = '" + id + "'  and UserId='" + databaseCon.Decrypt(UID) + "'";
            DataTable dt1 = databaseCon.GetData(query1);
            if (dt1.Rows.Count > 0)
            {
                SiteOwnerName = dt1.Rows[0]["SiteOwnerName"].ToString();
            }
            if (SiteOwnerName != "")
            {
                // Escape single quote by replacing ' with ''
                SiteOwnerName = SiteOwnerName.Replace("'", "''");
                var getSiteLocationData = dbcontext.Database.SqlQuery<SiteLocationViewModel>("select distinct SiteLocation,SiteLocID from tblSiteLocation where SiteLocation <>'' and SiteLocation is not null and ClientId = '" + id + "'  and OwnerName='" + SiteOwnerName + "' and MaintainInventory = 1 and MaintainInventoryAtJob=1  order by SiteLocation").ToList();
                return Json(getSiteLocationData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select SiteLocID, SiteLocation from tblsitelocation where ClientId = '" + id + "' Order by SiteLocation").ToList();
                //var getSiteLocationData = dbContext.Database.SqlQuery<SiteLocationViewModel>("select distinct SiteLocation,SiteLocID from tblSiteLocation where SiteLocation <>'' and SiteLocation is not null and  ClientId='" + databaseCon.Decrypt(id) + "' and MaintainInventory = 1 and MaintainInventoryAtJob=1  order by SiteLocation").ToList();
                var GetData = dbcontext.Database.SqlQuery<SiteLocationViewModel>("select * from (select 0 as SiteLocID,'All' as SiteLocation union select distinct SiteLocID,SiteLocation from tblSiteLocation where SiteLocation <>'' and SiteLocation is not null and  ClientId = '" + id + "' and MaintainInventory = 1 ) as sitelisting order by SiteLocation").ToList();
                return Json(GetData, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ddlJobDashboard(string id, string SiteId)
        {

            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select distinct Location as area,LocationId as areaid from VW_TblRegister where (Tonnage is not null or ManHrsDismantle is not null) and (SiteLocId='" + SiteId + "' or 'All'='" + SiteId + "') and ClientID='" + id + "' Order by Location").ToList();

            return Json(GetData, JsonRequestBehavior.AllowGet);

        }

        /// Site Structure Analysis Design type Dashboard Chart 1 Start

        List<ScaffoldAllDashBoardViewModel> SiteAnaysisDashboardChart1 = new List<ScaffoldAllDashBoardViewModel> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod(EnableSession = true)]
        //public List<Scaffold> SiteAnalysisByDType(string id, string jobid, string site)
        public ActionResult SiteAnalysisByDType(string id, string jobid, string site)
        {
            string qry = "select case when tblRegister.DesignType='' or tblRegister.DesignType is null then 'Design Type not assigned to job' else tblRegister.DesignType end as DesignType ,count(RegisterId) as RegisterIdCount,tblDesign.DifficultyRating as difficultyRating from tblRegister join tblDesign on tblRegister.DesignType=tblDesign.DesignType and tblDesign.ClientID= tblRegister.ClientID  where DateTagged is not null and (DateOfDismantle is null or DateOfDismantle = '01/01/1900') and tblDesign.ClientID=" + id + " and ( SiteLocId='" + site + "' or 'All'='" + site + "') and ('" + jobid + "' = 'All'  or LocationId = '" + jobid + "')   group by tblRegister.DesignType,tblDesign.DifficultyRating order by RegisterIdCount desc";

            DataSet ds = null;
            ds = databaseCon.GetDataSet(qry);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ScaffoldAllDashBoardViewModel pro = new ScaffoldAllDashBoardViewModel();
                    if (!(item["DesignType"] is DBNull) && item["DesignType"].ToString() != "")
                    {
                        pro.designtype = item["DesignType"].ToString();
                    }
                    if (!(item["RegisterIdCount"] is DBNull) && item["RegisterIdCount"].ToString() != "")
                    {
                        pro.rquantity = Convert.ToInt32(item["RegisterIdCount"].ToString());
                    }
                    if (!(item["difficultyRating"] is DBNull) && item["difficultyRating"].ToString() != "")
                    {
                        pro.Chart1difficultyrating = item["difficultyRating"].ToString();
                    }
                    SiteAnaysisDashboardChart1.Add(pro);
                }
            }
            // return SiteAnaysisDashboardChart1;
            return Json(SiteAnaysisDashboardChart1, JsonRequestBehavior.AllowGet);
        }
        /// Site Structure Analysis Design type Dashboard Chart 1 End

        /// Site Structure Analysis by Height Dashboard Chart 2 Start
        List<ScaffoldAllDashBoardViewModel> SiteAlanysisDashboardChat2 = new List<ScaffoldAllDashBoardViewModel> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod(EnableSession = true)]
        //public List<Scaffold> SiteAnalysisByHeight(string id, string jobid, string site)
        public ActionResult SiteAnalysisByHeight(string id, string jobid, string site)
        {
            string qry = "select count(RegisterId) as RCount,'<=1m' as Height  from tblRegister where Height<= 1 and DateTagged is not null and (DateOfDismantle is null or DateOfDismantle = '01/01/1900') and ClientID =" + id + " and ( SiteLocId='" + site + "' or 'All'='" + site + "') and ('" + jobid + "' = 'All'  or LocationId = '" + jobid + "') union select count(RegisterId) as registerid,'>1<=2' as cnt from tblRegister where Height > 1 and Height<= 2 and DateTagged is not null and (DateOfDismantle is null or DateOfDismantle = '01/01/1900') and ClientID = " + id + " and ( SiteLocId='" + site + "' or 'All'='" + site + "') and ('" + jobid + "' = 'All'  or LocationId = '" + jobid + "') union select count(RegisterId) as registerid,'>2<=4' as cnt from tblRegister where Height > 2 and Height<= 4 and DateTagged is not null and (DateOfDismantle is null or DateOfDismantle = '01/01/1900') and ClientID = " + id + " and ( SiteLocId='" + site + "' or 'All'='" + site + "') and ('" + jobid + "' = 'All'  or LocationId = '" + jobid + "') union select count(RegisterId) as registerid,'>4<=6' as cnt  from tblRegister where Height > 4 and Height<= 6 and DateTagged is not null and (DateOfDismantle is null or DateOfDismantle = '01/01/1900') and ClientID = " + id + " and ( SiteLocId='" + site + "' or 'All'='" + site + "') and ('" + jobid + "' = 'All'  or LocationId = '" + jobid + "') union select count(RegisterId) as registerid,'>6m' as cnt  from tblRegister where Height > 6 and DateTagged is not null and (DateOfDismantle is null or DateOfDismantle = '01/01/1900') and ClientID = " + id + " and ( SiteLocId='" + site + "' or 'All'='" + site + "') and ('" + jobid + "' = 'All'  or LocationId = '" + jobid + "') order by Height";
            // updated by pooja on 29may23 as '>4<=6' was written as '>5<=6' and similarly its condition 
            //string qry = "select count(RegisterId) as RCount,'<=1m' as Height  from tblRegister where Height<= 1 and DateTagged is not null and (DateOfDismantle is null or DateOfDismantle = '01/01/1900') and ClientID =" + id + " and ( SiteLocId='" + site + "' or 'All'='" + site + "') and ('" + jobid + "' = 'All'  or LocationId = '" + jobid + "') union select count(RegisterId) as registerid,'>1<=2' as cnt from tblRegister where Height > 1 and Height<= 2 and DateTagged is not null and (DateOfDismantle is null or DateOfDismantle = '01/01/1900') and ClientID = " + id + " and ( SiteLocId='" + site + "' or 'All'='" + site + "') and ('" + jobid + "' = 'All'  or LocationId = '" + jobid + "') union select count(RegisterId) as registerid,'>2<=4' as cnt from tblRegister where Height > 2 and Height<= 4 and DateTagged is not null and (DateOfDismantle is null or DateOfDismantle = '01/01/1900') and ClientID = " + id + " and ( SiteLocId='" + site + "' or 'All'='" + site + "') and ('" + jobid + "' = 'All'  or LocationId = '" + jobid + "') union select count(RegisterId) as registerid,'>5<=6' as cnt  from tblRegister where Height > 5 and Height<= 6 and DateTagged is not null and (DateOfDismantle is null or DateOfDismantle = '01/01/1900') and ClientID = " + id + " and ( SiteLocId='" + site + "' or 'All'='" + site + "') and ('" + jobid + "' = 'All'  or LocationId = '" + jobid + "') union select count(RegisterId) as registerid,'>6m' as cnt  from tblRegister where Height > 6 and DateTagged is not null and (DateOfDismantle is null or DateOfDismantle = '01/01/1900') and ClientID = " + id + " and ( SiteLocId='" + site + "' or 'All'='" + site + "') and ('" + jobid + "' = 'All'  or LocationId = '" + jobid + "') order by Height";

            DataSet ds = null;
            ds = databaseCon.GetDataSet(qry);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ScaffoldAllDashBoardViewModel pro = new ScaffoldAllDashBoardViewModel();
                    if (!(item["RCount"] is DBNull) && item["RCount"].ToString() != "")
                    {
                        pro.RCount = Convert.ToInt32(item["RCount"].ToString());
                    }
                    if (!(item["Height"] is DBNull) && item["Height"].ToString() != "")
                    {
                        pro.Height = item["Height"].ToString();
                    }
                    SiteAlanysisDashboardChat2.Add(pro);
                }
            }
            // return SiteAlanysisDashboardChat2;
            return Json(SiteAlanysisDashboardChat2, JsonRequestBehavior.AllowGet);
        }
        /// Site Structure Analysis by Height Dashboard Chart 2 End

        /// Site Structure Analysis by DifficultyRating Dashboard Chart 3 Start

        List<ScaffoldAllDashBoardViewModel> SiteAlanysisDashboardChat3 = new List<ScaffoldAllDashBoardViewModel> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod(EnableSession = true)]
        //public List<Scaffold> SiteAnalysisByDRating(string id, string jobid, string site)
        public ActionResult SiteAnalysisByDRating(string id, string jobid, string site)
        {
            //change on 15 Nov
            string qry = "select count(RegisterId)RCount,DifficultyRating from tbldesign join tblRegister on tblDesign.DesignType=tblRegister.DesignType and  tblDesign.clientId=tblRegister.clientId where DateTagged is not null and (DateOfDismantle is null or DateOfDismantle = '01/01/1900') and tblRegister.ClientID=" + id + " and ( tblRegister.SiteLocId='" + site + "' or 'All'='" + site + "') and ('" + jobid + "' = 'All'  or LocationId = '" + jobid + "') and tblRegister.DesignType!='' and tblRegister.DesignType is not null group by DifficultyRating order by DifficultyRating asc";

            DataSet ds = null;
            ds = databaseCon.GetDataSet(qry);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ScaffoldAllDashBoardViewModel pro = new ScaffoldAllDashBoardViewModel();
                    if (!(item["RCount"] is DBNull) && item["RCount"].ToString() != "")
                    {
                        pro.DRCount = Convert.ToInt32(item["RCount"].ToString());
                    }
                    if (!(item["DifficultyRating"] is DBNull) && item["DifficultyRating"].ToString() != "")
                    {
                        pro.DRating = item["DifficultyRating"].ToString();
                    }
                    SiteAlanysisDashboardChat3.Add(pro);
                }
            }
            //return SiteAlanysisDashboardChat3;
            return Json(SiteAlanysisDashboardChat3, JsonRequestBehavior.AllowGet);
        }
        /// Site Structure Analysis by DifficultyRating Dashboard Chart 3 End
    }
}