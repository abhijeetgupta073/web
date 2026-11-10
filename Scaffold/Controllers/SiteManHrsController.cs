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
    public class SiteManHrsController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: SiteManHrs
        public ActionResult SiteManHrs()
        {
            ViewBag.ExportToCSV = "false";
            string query = "select StandardWorkingHrs from tblClient where ClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "'";
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                ViewBag.txtstandardworkinghour = dt.Rows[0]["StandardWorkingHrs"].ToString();
            }
            
            return View();
        }

        public ActionResult dropdownSiteLocation(string id, string UID)
        {
            string query = "select distinct SiteLocID,SiteLocation from tblSiteLocation where SiteLocation <>'' and SiteLocation is not null and  ClientId='" + databaseCon.Decrypt(id) + "' and isnull(ManHoursManagement,0)= 1 order by SiteLocation";

            var getData = dbContext.Database.SqlQuery<DropDownViewModel>(query).ToList();
            return Json(getData, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetSiteManHrsData(string id, string SiteID, string siteName, string EntryDate,string chckExportToCsv)
        {
          string query = "";
            //string selectedsite = "";
            //// DateTime Date;
            //selectedsite = ddlSite.SelectedItem.Text.Replace("'", "''").Trim();
          if (chckExportToCsv == "false")
          {
                //if (ddlSite.SelectedValue != "" && DatePicker.SelectedDate != null)
            if (SiteID != "" && EntryDate != null)
            {

                    query = "select isnull(SiteManHrsId,0) as SiteManHrsId,'" + SiteID + "' as SiteLocId,'" + siteName + "' as SiteLocation,'" + Convert.ToDateTime(EntryDate).ToString("dd-MMM-yyyy") + "' as EntryDate,tblUser.UserId,firstName+' '+LastName as name, isnull(NoOfWorkedHrs,0) as NoOfWorkedHrs ,Case when NoOfWorkedHrs>0 then 'Yes' Else 'No' End as IsPresent , isnull(AllocatedHrs,0) as AllocatedHrs,isnull(Category,'') as Category from tblUser left join tblSiteManHrs on (tblUser.UserId = tblSiteManHrs.UserId and tblSiteManHrs.SiteLocId = '" + SiteID + "' and tblSiteManHrs.EntryDate = '" + Convert.ToDateTime(EntryDate).ToString("dd-MMM-yyyy") + "')   Left join (select UserId, Sum(Isnull(NoOfWorkedHrs, 0)) as AllocatedHrs from tblSiteManHrs where EntryDate = '" + Convert.ToDateTime(EntryDate).ToString("dd-MMM-yyyy") + "' and SiteLocId !='" + SiteID + "' group by UserId)AllocatedQuery On tblUser.UserId = AllocatedQuery.UserID left join tblUserCategory on tblUser.CategoryId=tblUserCategory.UserCatId where tblUser.Status='Active' and tblUser.ClientId = '" + databaseCon.Decrypt(id) + "' and Role !='Superadmin' order by IsPresent desc";

                    DataTable dt = databaseCon.GetData(query);
                    if (dt.Rows.Count > 0)
                    {
                       // getgridcount.Value = dt.Rows.Count.ToString();
                    }
                    //SiteManHrs1.DataSource = dt;
                    //SiteManHrs1.Style.Add("display", "block");
                }
            }
            else
            {
                 query = "select isnull(SiteManHrsId,0) as SiteManHrsId,'" + SiteID + "' as SiteLocId,'" + siteName + "' as SiteLocation,'" + Convert.ToDateTime(EntryDate).ToString("dd-MMM-yyyy") + "' as EntryDate,tblUser.UserId,firstName+' '+LastName as name, isnull(NoOfWorkedHrs,0) as NoOfWorkedHrs ,Case when NoOfWorkedHrs>0 then 'Yes' Else 'No' End as IsPresent , isnull(AllocatedHrs,0) as AllocatedHrs,isnull(Category,'') as Category from tblUser left join tblSiteManHrs on (tblUser.UserId = tblSiteManHrs.UserId and tblSiteManHrs.SiteLocId = '" + SiteID + "' and tblSiteManHrs.EntryDate = '" + Convert.ToDateTime(EntryDate).ToString("dd-MMM-yyyy") + "')   Left join (select UserId, Sum(Isnull(NoOfWorkedHrs, 0)) as AllocatedHrs from tblSiteManHrs where EntryDate = '" + Convert.ToDateTime(EntryDate).ToString("dd-MMM-yyyy") + "' and SiteLocId !='" + SiteID + "' group by UserId)AllocatedQuery On tblUser.UserId = AllocatedQuery.UserID left join tblUserCategory on tblUser.CategoryId=tblUserCategory.UserCatId where tblUser.Status='Active' and tblUser.ClientId = '" + databaseCon.Decrypt(id) + "' and Role !='Superadmin' and NoOfWorkedHrs>0 order by IsPresent desc";

                //DataTable dt1 = databaseCon.GetData(qury);
                //SiteManHrs1.DataSource = dt1;
            }

            var getData = dbContext.Database.SqlQuery<SiteManHrsViewModel>(query);
            return Json(getData, JsonRequestBehavior.AllowGet);

        }

        public ActionResult btnSaveSiteManHrs_Click(List<SiteManHrsViewModel> model)
        {
            var Result = "";
            var Message = "";

            try
            {
                foreach (SiteManHrsViewModel item in model)
                {

                    var Array = item;

                    if (Array.SiteManHrsId.ToString() == "0")
                    {
                        string query = "exec Pro_TBLSITEMANHRS_SAVE '0','" + Array.SiteLocId.Trim().Replace("'", "''") + "','" + Array.EntryDate + "','" + Array.UserId + "','" + Array.NoOfWorkedHrs + "','" + databaseCon.Decrypt(Array.UID) + "','" + DateTime.Now + "'";
                        DataTable dt = databaseCon.GetData(query);

                        if (dt.Rows.Count != 0)
                        {
                            if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                            {
                                //art1.Attributes["class"] = "w3-section w3-card-12 j-close fadeOut j-alertBox-Success";
                                //art1.Style.Add("display", "block");
                                //alertlabel.Text = "<strong>Success ! </strong>" + dt.Rows[0][1].ToString();
                                //// SiteManHrs1.Rebind();
                                Result = "Success";
                                Message = dt.Rows[0][1].ToString();
                            }
                            else if (Convert.ToInt32(dt.Rows[0][0]) == -1)
                            {
                                //art1.Attributes["class"] = "w3-section w3-card-12  j-close fadeOut j-alertBox-Failed";
                                //art1.Style.Add("display", "block");
                                //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                                Result = "Failed";
                                Message = dt.Rows[0][1].ToString();

                            }
                            else
                            {
                                //art1.Attributes["class"] = "w3-section w3-card-12  j-close fadeOut j-alertBox-Failed";
                                //art1.Style.Add("display", "block");
                                //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                                Result = "Failed";
                                Message = dt.Rows[0][1].ToString();
                            }
                        }
                        else
                        {
                            //art1.Attributes["class"] = "w3-section w3-card-12  j-close fadeOut j-alertBox-Failed";
                            //art1.Style.Add("display", "block");
                            //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator ";
                            Result = "Failed";
                            Message = "Syntax Error. Contact administrator ";
                        }
                    }

                    else
                    {
                        string query = "exec Pro_TBLSITEMANHRS_SAVE '" + Array.SiteManHrsId + "','" + Array.SiteLocId.Trim().Replace("'", "''") + "','" + Array.EntryDate + "','" + Array.UserId + "','" + Array.NoOfWorkedHrs + "','" + databaseCon.Decrypt(Array.UID) + "','" + DateTime.Now + "'";

                        DataTable dt = databaseCon.GetData(query);

                        if (dt.Rows.Count != 0)
                        {
                            if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                            {
                                //art1.Attributes["class"] = "w3-section w3-card-12  j-close fadeOut j-alertBox-Success";
                                //art1.Style.Add("display", "block");
                                //alertlabel.Text = "<strong>Success ! </strong>" + dt.Rows[0][1].ToString();
                                ////SiteManHrs1.Rebind();
                                Result = "Success";
                                Message = dt.Rows[0][1].ToString();
                            }
                            else if (Convert.ToInt32(dt.Rows[0][0]) == -1)
                            {
                                //art1.Attributes["class"] = "w3-section w3-card-12  j-close fadeOut j-alertBox-Failed";
                                //art1.Style.Add("display", "block");
                                //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                                Result = "Failed";
                                Message = dt.Rows[0][1].ToString();

                            }
                            else
                            {
                                //art1.Attributes["class"] = "w3-section w3-card-12  j-close fadeOut  j-alertBox-Failed";
                                //art1.Style.Add("display", "block");
                                //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                                Result = "Failed";
                                Message = dt.Rows[0][1].ToString();
                            }
                        }
                        else
                        {
                            //art1.Attributes["class"] = "w3-section w3-card-12  j-close fadeOut j-alertBox-Failed";
                            //art1.Style.Add("display", "block");
                            //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator ";
                            Result = "Failed";
                            Message = "Syntax Error. Contact administrator ";
                        }

                    }



                }


            }
            catch (Exception ex)
            {
                Result = "Failed";
                Message = ex.Message;
            }


            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }
    }

}