using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;
using System.Data;

namespace Scaffold.Controllers
{
    public class ComparisonDashboardController : Controller
    {
        ScaffoldEntities dbcontext = new ScaffoldEntities();
        // GET: ComparisonDashboard
        public ActionResult ComparisonDashboard()
        {
            return View();
        }

        /*New Code Nitin*/
        

        public ActionResult GetPageImagePath(string Page1base64, string Page2base64)
            {
            var Msg = "";
            if (!string.IsNullOrEmpty(Page1base64) && !string.IsNullOrEmpty(Page2base64))
                {
                Session["Page1base64"] = Page1base64;
                Session["Page2base64"] = Page2base64;
                Msg = "Success";
                }
            else
                {
                Msg = "Error";
                }

            return Json(Msg, JsonRequestBehavior.AllowGet);
            }

        /**/



        public ActionResult ddlsitedropdownbindquery(string id)
            {
            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select SiteLocID,SiteLocation from dbo.tblSiteLocation where ClientId='" + id + "' order by SiteLocation").ToList();
            return Json(GetData, JsonRequestBehavior.AllowGet);
            }

        List<ComparisonViewModel> ComparisondataDetails = new List<ComparisonViewModel> { };
        public ActionResult WeekEndingData(string Siteid, string ClientID ,DateTime WeekEndingDate)
            {

            //String query = "exec ComparisonDashboardSummary'" + WeekEndingDate + ","+Siteid + "," + ClientID +"'";
            //String query = "exec Pro_ComparisonDashboardSummary '" + WeekEndingDate + "'," + Siteid + "," + ClientID + "";
            String query = "exec Pro_ComparisonDashboardSummary '" + WeekEndingDate + "','" + Siteid + "'," + ClientID + "";

            DataSet ds = null;
              ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
                {
                foreach (DataRow item in ds.Tables[0].Rows)
                    {
                    ComparisonViewModel pro = new ComparisonViewModel();
                    if (!(item["Erectedscaffold"] is DBNull) && item["Erectedscaffold"].ToString() != "")
                        {
                        pro.ErectedDate1 = item["Erectedscaffold"].ToString();
                        }
                    if (!(item["StandingScaffold"] is DBNull) && item["StandingScaffold"].ToString() != "")
                        {
                        pro.ScaffoldsStanding1 = item["StandingScaffold"].ToString();
                        }
                    if (!(item["ErectScaffold"] is DBNull) && item["ErectScaffold"].ToString() != "")
                        {
                        pro.ErectedDate2 = item["ErectScaffold"].ToString();
                        }
                    if (!(item["DismantledScaffolds"] is DBNull) && item["DismantledScaffolds"].ToString() != "")
                        {
                        pro.DismantledScaffolds1 = item["DismantledScaffolds"].ToString();
                        }
                    if (!(item["TotalManHoursSpent"] is DBNull) && item["TotalManHoursSpent"].ToString() != "")
                        {
                        pro.ManHrs = item["TotalManHoursSpent"].ToString();
                        }
                    if (!(item["ErrectedTonnage"] is DBNull) && item["ErrectedTonnage"].ToString() != "")
                        {
                        pro.TonnageErected1 = item["ErrectedTonnage"].ToString();
                        }
                    if (!(item["StandingTonnage"] is DBNull) && item["StandingTonnage"].ToString() != "")
                        {
                        pro.TonnageStanding1 = item["StandingTonnage"].ToString();
                        }
                    if (!(item["AverageTonnage"] is DBNull) && item["AverageTonnage"].ToString() != "")
                        {
                        pro.AverageTonnage1 = item["AverageTonnage"].ToString();
                        }
                    if (!(item["AvgHireDays"] is DBNull) && item["AvgHireDays"].ToString() != "")
                        {
                        pro.HireDays1 = item["AvgHireDays"].ToString();
                        }
                    ComparisondataDetails.Add(pro);
                    }
                }
            return Json(ComparisondataDetails, JsonRequestBehavior.AllowGet);
            }

        List<ComparisonViewModel> ManHrsTonnage = new List<ComparisonViewModel> { };

        
        public ActionResult ManHrsDashboard(string Siteid, string ClientID, string ALL, DateTime WeekEndingDate)
            {
            //string query = "exec Pro_ComparisonDashboardWeek @WeekEndingDate  = '11/30/2020' ,  @week  = '5' ,   @SiteLocId = 'All' , @LocationId  = 'All',@ClientId  = '30'";
            // string query = "exec Pro_ComparisonDashboardWeek '" + WeekEndingDate + "'," + Siteid + ",'ALL' ," + ClientID + "";
            // string query = "exec Pro_ComparisonDashboardWeek @WeekEndingDate  = '" + WeekEndingDate + "',   @SiteLocId  = " + Siteid + ", @LocationId  = 'All',@ClientId   =" + ClientID;
            string query = "exec Pro_ComparisonDashboardWeek @WeekEndingDate  = '" + WeekEndingDate + "',   @SiteLocId  = '" + Siteid + "', @LocationId  = 'All',@ClientId   =" + ClientID;
            // string query = "exec Pro_ComparisonDashboardWeek '05/22/2021 12:00:00 AM',5,103,'ALL',38";
            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
                {
                foreach (DataRow item in ds.Tables[0].Rows)
                    {
                    ComparisonViewModel pro = new ComparisonViewModel();
                    if (!(item["WeekNo"] is DBNull) && item["WeekNo"].ToString() != "")
                        {
                        pro.WeekNum = item["WeekNo"].ToString();
                        }
                    if (!(item["TotManHrsErect"] is DBNull) && item["TotManHrsErect"].ToString() != "")
                        {
                        pro.TotManHrsErect = Convert.ToDecimal(item["TotManHrsErect"].ToString());
                        }
                    if (!(item["TotManHrsDismantle"] is DBNull) && item["TotManHrsDismantle"].ToString() != "")
                        {
                        pro.TotManHrsDismantle = Convert.ToDecimal(item["TotManHrsDismantle"].ToString());
                        }
                    if (!(item["TotErectedTonnage"] is DBNull) && item["TotErectedTonnage"].ToString() != "")
                        {
                        pro.TotErectedTonnage = Convert.ToDecimal(item["TotErectedTonnage"].ToString());
                        }
                    if (!(item["TotErectedRegister"] is DBNull) && item["TotErectedRegister"].ToString() != "")
                        {
                        pro.TotErectedRegister = Convert.ToDecimal(item["TotErectedRegister"].ToString());
                        }
                    if (!(item["TotDismantleRegister"] is DBNull) && item["TotDismantleRegister"].ToString() != "")
                        {
                        pro.TotDismantleRegister = Convert.ToDecimal(item["TotDismantleRegister"].ToString());
                        }
                    if (!(item["TotDismantleTonnage"] is DBNull) && item["TotDismantleTonnage"].ToString() != "")
                        {
                        pro.TotDismantleTonnage =Convert.ToDecimal(item["TotDismantleTonnage"].ToString());
                        }
                    if (!(item["weekStartDate"] is DBNull) && item["weekStartDate"].ToString() != "")
                        {
                        pro.WeekStartDate = Convert.ToDateTime(item["weekStartDate"]).ToString("dd-MMM-yyyy");
                        }
                    if (!(item["weekEndDate"] is DBNull) && item["weekEndDate"].ToString() != "")
                        {
                        pro.WeekEndDate = Convert.ToDateTime(item["weekEndDate"]).ToString("dd-MMM-yyyy");
                        }
                    if (!(item["LinearTonnageErected"] is DBNull) && item["LinearTonnageErected"].ToString() != "")
                        {
                        pro.LinearTonnage = item["LinearTonnageErected"].ToString();
                        }
                    if (!(item["LinearManHrsErect"] is DBNull) && item["LinearManHrsErect"].ToString() != "")
                        {
                        pro.LinerManHrs = item["LinearManHrsErect"].ToString();
                        }

                    ManHrsTonnage.Add(pro);
                    }
                }
            return Json(ManHrsTonnage, JsonRequestBehavior.AllowGet);
            }

        List<ComparisonViewModel> TonneTackingDetails = new List<ComparisonViewModel> { };
        public ActionResult TonneTacking(string Siteid, string ClientID, string ALL, DateTime WeekEndingDate)
            {
            //string query = "exec Pro_ComparisonAverageDashboard @WeekEndingDate  = '" + WeekEndingDate + "',   @SiteLocId  = " + Siteid + ", @LocationId  = 'All',@ClientId   =" + ClientID;
            string query = "exec Pro_ComparisonAverageDashboard @WeekEndingDate  = '" + WeekEndingDate + "',   @SiteLocId  = '" + Siteid + "', @LocationId  = 'All',@ClientId   =" + ClientID;
            // string query = "exec Pro_ComparisonDashboardWeek '05/22/2021 12:00:00 AM',5,103,'ALL',38";
            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
                {
                foreach (DataRow item in ds.Tables[0].Rows)
                    {
                    ComparisonViewModel pro = new ComparisonViewModel();
                    if (!(item["WeekNo"] is DBNull) && item["WeekNo"].ToString() != "")
                        {
                        pro.WeekNumb = item["WeekNo"].ToString();
                        }
                    if (!(item["TotErectedTonnage"] is DBNull) && item["TotErectedTonnage"].ToString() != "")
                        {
                        pro.TotErectedTonnage = Convert.ToDecimal(item["TotErectedTonnage"].ToString());
                        }
                    if (!(item["TotManHrsErect"] is DBNull) && item["TotManHrsErect"].ToString() != "")
                        {
                        pro.TotManHrsErect = Convert.ToDecimal(item["TotManHrsErect"].ToString());
                        }
                    if (!(item["AverageErectHoursTonne"] is DBNull) && item["AverageErectHoursTonne"].ToString() != "")
                        {
                        pro.AverageErectHoursTonne = Convert.ToDecimal(item["AverageErectHoursTonne"].ToString());
                        }
                    if (!(item["TotDismantleTonnage"] is DBNull) && item["TotDismantleTonnage"].ToString() != "")
                        {
                        pro.TotDismantleTonnage = Convert.ToDecimal(item["TotDismantleTonnage"].ToString());
                        }
                    if (!(item["TotManHrsDismantle"] is DBNull) && item["TotManHrsDismantle"].ToString() != "")
                        {
                        pro.TotManHrsDismantle = Convert.ToDecimal(item["TotManHrsDismantle"].ToString());
                        }
                    if (!(item["AverageDismantleHoursTonne"] is DBNull) && item["AverageDismantleHoursTonne"].ToString() != "")
                        {
                        pro.AverageDismantleHoursTonne = Convert.ToDecimal(item["AverageDismantleHoursTonne"].ToString());
                        }
                    if (!(item["TotalManHours"] is DBNull) && item["TotalManHours"].ToString() != "")
                        {
                        pro.TotalManHours = Convert.ToDecimal(item["TotalManHours"].ToString());
                        }
                    if (!(item["TotalTonnageMoved"] is DBNull) && item["TotalTonnageMoved"].ToString() != "")
                        {
                        pro.TotalTonnageMoved = Convert.ToDecimal(item["TotalTonnageMoved"].ToString());
                        }
                    if (!(item["AverageTotalHoursTonne"] is DBNull) && item["AverageTotalHoursTonne"].ToString() != "")
                        {
                        pro.AverageTotalHoursTonne = Convert.ToDecimal(item["AverageTotalHoursTonne"].ToString());
                        }
                    if (!(item["weekStartDate"] is DBNull) && item["weekStartDate"].ToString() != "")
                        {
                        pro.WeekStartDate = Convert.ToDateTime(item["weekStartDate"]).ToString("dd-MMM-yyyy");
                        }
                    if (!(item["weekEndDate"] is DBNull) && item["weekEndDate"].ToString() != "")
                        {
                        pro.WeekEndDate = Convert.ToDateTime(item["weekEndDate"]).ToString("dd-MMM-yyyy");
                        }
                    TonneTackingDetails.Add(pro);
                    }
                }
            return Json(TonneTackingDetails, JsonRequestBehavior.AllowGet);
            }

        }
    }