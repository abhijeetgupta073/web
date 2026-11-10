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
    public class ManHrsController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: SiteManHrs
        public ActionResult ManHrs()
        {
            
            
            return View();
        }

        public ActionResult dropdownSiteLocation(string id, string UID)
        {
            string query = "select distinct SiteLocID,SiteLocation from tblSiteLocation where SiteLocation <>'' and SiteLocation is not null and  ClientId='" + databaseCon.Decrypt(id) + "' and isnull(ManHoursManagement,0)= 1 order by SiteLocation";

            var getData = dbContext.Database.SqlQuery<DropDownViewModel>(query).ToList();
            return Json(getData, JsonRequestBehavior.AllowGet);

        }

        public class JobRegisterViewModel
        {
            public Nullable<int> RegisterId { get; set; }
            public string Description { get; set; }
            public string ScaffRef { get; set; }
            public int joborder { get; set; }
            public Nullable<int> UserId { get; set; }
            public string resource { get; set; }
        }

        public ActionResult dropdownResource(string id)
        {
            var getJobData = dbContext.Database.SqlQuery<JobRegisterViewModel>("select UserId,firstName+' '+LastName as resource from tblUser where ClientId='" + databaseCon.Decrypt(id) + "'").ToList();
            return Json(getJobData, JsonRequestBehavior.AllowGet);

        }

        public ActionResult SiteJobDropdownBind(string id, string SiteID)
        {
            var getJobData = dbContext.Database.SqlQuery<JobRegisterViewModel>("select RegisterId,Right('000000'+Convert(varchar,ScaffRef),6) as ScaffRef,Description,case when Status='Completed' then 2 when Status='NotStarted' then 1 else 0 end as joborder from vw_tblRegister where ClientID='" + databaseCon.Decrypt(id) + "' and siteLocId='" + SiteID + "' order by joborder").ToList();
            return Json(getJobData, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetManHrsData(string id, string SiteID, string RegisterId, string EntryDate, string ResourceId, string NewEntryDate)
        {
            string query = "";
            if (SiteID != "" && RegisterId != "" && EntryDate != null)
            {

                query = "select isnull(ManHrsId,0) as ManHrsId,SiteManHrsId,tblSiteManHrs.SiteLocId,SiteLocation,ISNULL(tblRegister.RegisterID,0) as RegisterID,case when Description is not null and Description!=''then Right('000000'+Convert(varchar,ScaffRef),6)+'-'+Description else Right('000000'+Convert(varchar,ScaffRef),6) end as Description,  tblSiteManHrs.UserId ,tblSiteManHrs.EntryDate,firstName+' '+LastName as name,AdditionalHrs,TypeofVariation,VariationDesc,case when IncludedInDayDocket=0 then 'No' when IncludedInDayDocket=1 then  'Yes' else '' end as IncludedInDayDocket,VariationComments,ErectionHrs,ModificationHrs,DismantleHrs , isnull(NoOfWorkedHrs,0) as NoOfHrs,Isnull(AllocatedHrs,0) as AllocatedHrs, isnull(Category,'') as Category from tblSiteManHrs join tblUser On tblSiteManHrs.UserId=tblUser.UserId join tblSiteLocation On tblSiteManHrs.SiteLocId=tblSiteLocation.SiteLocID left join tblManHrs on (tblSiteManHrs.UserId = tblManHrs.UserId and tblSiteManHrs.EntryDate = tblManHrs.EntryDate  and tblSiteManHrs.SiteLocId=tblManHrs.SiteLocId and tblManHrs.RegisterId='" + RegisterId + "') Left join tblRegister on tblRegister.RegisterId='" + RegisterId + "' Left join (select UserId,Isnull(Sum(Isnull(ModificationHrs,0)+Isnull(ErectionHrs,0)+Isnull(DismantleHrs,0)+Isnull(AdditionalHrs,0)),0) as AllocatedHrs From TblManHrs Where SiteLocID='" + SiteID + "' and RegisterId!='" + RegisterId + "' and tblManHrs.EntryDate = '" + Convert.ToDateTime(EntryDate).ToString("yyyy-MM-dd") + "'  Group by UserId) AllocatedQuery on tblUser.UserId=AllocatedQuery.UserId left join tblUserCategory on tblUser.CategoryId=tblUserCategory.UserCatId where tblSiteManHrs.EntryDate = '" + Convert.ToDateTime(EntryDate).ToString("yyyy-MM-dd") + "' and tblSiteManHrs.SiteLocId='" + SiteID + "' and tblUser.Role !='Superadmin'";
                
                //dt = databaseCon.GetData(query);

                //if (dt.Rows.Count > 0)
                //{
                //    getgridcount.Value = dt.Rows.Count.ToString();
                //}

                //ManHrsGrid.DataSource = dt;
            }
            else if (ResourceId != "" && NewEntryDate != null)
            {
                query = "select isnull(ManHrsId,0) as ManHrsId,SiteManHrsId,tblSiteManHrs.SiteLocId,SiteLocation,ISNULL(tblManHrs.RegisterID,0) as RegisterID,case when Description is not null and Description!=''then Right('000000'+Convert(varchar,ScaffRef),6)+'-'+Description else Right('000000'+Convert(varchar,ScaffRef),6) end as Description,  tblSiteManHrs.UserId ,tblSiteManHrs.EntryDate,firstName+' '+LastName as name,AdditionalHrs,TypeofVariation,VariationDesc,case when IncludedInDayDocket=0 then 'No' when IncludedInDayDocket=1 then  'Yes' else '' end as IncludedInDayDocket,VariationComments,ErectionHrs,ModificationHrs,DismantleHrs , isnull(NoOfWorkedHrs,0) as NoOfHrs,AllocatedHrs,isnull(Category,'') as Category  from tblSiteManHrs join tblUser On tblSiteManHrs.UserId = tblUser.UserId  join tblSiteLocation On tblSiteManHrs.SiteLocId = tblSiteLocation.SiteLocID left join tblUserCategory on tblUser.CategoryId=tblUserCategory.UserCatId left join tblManHrs on (tblSiteManHrs.UserId = tblManHrs.UserId and tblSiteManHrs.EntryDate = tblManHrs.EntryDate  and tblSiteManHrs.SiteLocId = tblManHrs.SiteLocId ) join tblRegister on tblRegister.RegisterId = tblManHrs.RegisterId  Left join (select UserId,Sum (Isnull(ModificationHrs,0)+Isnull(ErectionHrs,0)+Isnull(DismantleHrs,0)+Isnull(AdditionalHrs,0)) as AllocatedHrs From TblManHrs  Where   tblManHrs.EntryDate = '" + Convert.ToDateTime(NewEntryDate).ToString("yyyy-MM-dd") + "'  Group by UserId) AllocatedQuery on tblUser.UserId=AllocatedQuery.UserId where tblSiteManHrs.EntryDate = '" + Convert.ToDateTime(NewEntryDate).ToString("yyyy-MM-dd") + "' and tblSiteManHrs.UserId = '" + ResourceId + "' UNION select 0 as ManHrsId,SiteManHrsID as SiteManHrsID,tblManHrs.SiteLocId,SiteLocation,ISNULL(tblManHrs.RegisterID, 0) as RegisterID,case when Description is not null and Description!= ''then Right('000000' + Convert(varchar, ScaffRef), 6)+'-' + Description else Right('000000' + Convert(varchar, ScaffRef), 6) end as Description,  tblUser.UserId ,tblManHrs.EntryDate,firstName + ' ' + LastName as Name,0 as AdditionalHrs,'' asTypeofVariation,'' as VariationDesc,'' as IncludedInDayDocket,'' as VariationComments,0 as ErectionHrs,0 as ModificationHrs,0 as DismantleHrs , isnull(NoOfWorkedHrs, 0) as NoOfHrs ,0 as AllocatedHrs ,isnull(Category,'') as Category  from(select distinct SiteLocId, RegisterID, EntryDate from tblManHrs where tblManHrs.EntryDate = '" + Convert.ToDateTime(NewEntryDate).ToString("yyyy-MM-dd") + "' and tblManHrs.userid <> '" + ResourceId + "') as tblManHrs left join tblSiteManHrs on tblSiteManHrs.SiteLocId = tblManHrs.SiteLocId and tblSiteManHrs.UserId = '" + ResourceId + "'   and tblSiteManHrs.EntryDate='" + Convert.ToDateTime(NewEntryDate).ToString("yyyy-MM-dd") + "' join tblUser On tblUser.UserId = '" + ResourceId + "' join tblSiteLocation On tblManHrs.SiteLocId = tblSiteLocation.SiteLocID  join tblRegister on tblRegister.RegisterId = tblManHrs.RegisterId left join tblUserCategory on tblUser.CategoryId=tblUserCategory.UserCatId  where tblSiteLocation.ClientId = '" + databaseCon.Decrypt(id) + "' and tblRegister.RegisterId not in(Select RegisterId from tblManHrs  where EntryDate = '" + Convert.ToDateTime(NewEntryDate).ToString("yyyy-MM-dd") + "' and UserId = '" + ResourceId + "')";

                //dt = databaseCon.GetData(query);

                //if (dt.Rows.Count > 0)
                //{
                //    getgridcount.Value = dt.Rows.Count.ToString();
                //}

                //ManHrsGrid.DataSource = dt;
            }

            var getData = dbContext.Database.SqlQuery<ManHrsViewModel>(query);
            return Json(getData, JsonRequestBehavior.AllowGet);

        }


        public ActionResult btnSaveManHrs_Click(List<ManHrsViewModel> model)
        {
            string query = "";
            var Result = "";
            var Message = "";

            string DayDocketCheck = "";
            string ErectionHrs = "";
            string ModificationHrs = "";
            string DismantleHrs = "";
            string AdditionalHrs = "";
            string VariationDesc = "";
            string VariationComments = ""; 
            try
            {
                foreach (ManHrsViewModel item in model)
                {

                    var Array = item;

                    if (Array.IncludedInDayDocket == "Yes")
                    {
                        DayDocketCheck = "1";
                    }
                    else if (Array.IncludedInDayDocket == "No")
                    {
                        DayDocketCheck = "0";
                    }
                    else
                    {
                        DayDocketCheck = "null";
                    }

                    if (Array.ErectionHrs.ToString() != "")
                    {

                        ErectionHrs = Array.ErectionHrs.ToString();
                        if (Array.ErectionHrs.ToString() == "0")
                        {
                            ErectionHrs = "null";
                        }
                    }
                    else
                    {
                        ErectionHrs = "null";
                    }

                    if (Array.ModificationHrs.ToString() != "")
                    {

                        ModificationHrs = Array.ModificationHrs.ToString();
                        if (Array.ModificationHrs.ToString() == "0")
                        {
                            ModificationHrs = "null";
                        }
                    }
                    else
                    {
                        ModificationHrs = "null";
                    }

                    if (Array.AdditionalHrs.ToString() != "" )
                    {
                        AdditionalHrs = Array.AdditionalHrs.ToString();
                        if (Array.AdditionalHrs.ToString() == "0")
                        {
                            AdditionalHrs = "null";
                        }
                    }
                    else
                    {
                        AdditionalHrs = "null";
                    }



                    if (Array.DismantleHrs.ToString() != "" )
                    {
                        DismantleHrs = Array.DismantleHrs.ToString();
                        if (Array.DismantleHrs.ToString() == "0")
                        {
                            DismantleHrs = "null";
                        }
                    }
                    else
                    {
                        DismantleHrs = "null";
                    }


                    if (Convert.ToString(Array.VariationDesc) != null && Convert.ToString(Array.VariationDesc) != "")
                    {
                        VariationDesc = Convert.ToString(Array.VariationDesc).Replace("'", "''").Trim();
                    }
                    else
                    {
                        VariationDesc = "";
                    }

                    if (Convert.ToString(Array.VariationComments) != null && Convert.ToString(Array.VariationComments) != "")
                    {
                        VariationComments = Convert.ToString(Array.VariationComments).Replace("'", "''").Trim();
                    }
                    else
                    {
                        VariationComments = "";
                    }

                    query = "exec Pro_TBLMANHRS_SAVE '" + Array.ManHrsId + "','" + Array.SiteLocId + "','" + Array.RegisterID + "','" + Array.EntryDate + "','" + Array.UserId + "'," + Array.NoOfHrs + ",'" + databaseCon.Decrypt(Array.UID) + "','" + DateTime.Now + "'," + AdditionalHrs + ",'" + Array.TypeofVariation + "','" + VariationDesc + "'," + DayDocketCheck + ",'" + VariationComments + "'," + ErectionHrs + "," + ModificationHrs + "," + DismantleHrs + "";
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

                    

            }
            catch (Exception ex)
            {
                Result = "Failed";
                Message = ex.Message;
            }


            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult btndeleteyes_Click(string id, string ManHrsId)
        {
            string query;
            DataTable dt;
            query = "exec Pro_TBLManHrs_DEL '" + ManHrsId + "'";
            dt = databaseCon.GetData(query);
            if (dt.Rows.Count != 0)
            {
                if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                {
                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Success ! </strong>" + dt.Rows[0][1].ToString();
                    //ManHrsGrid.Rebind();
                    return Json(new { Result = "Success", Message = dt.Rows[0][1].ToString() });
                }
                else if (Convert.ToInt32(dt.Rows[0][0]) == -1)
                {
                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                    return Json(new { Result = "Failed", Message = dt.Rows[0][1].ToString() });
                }
                else
                {
                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                    return Json(new { Result = "Failed", Message = dt.Rows[0][1].ToString() });
                }
            }
            else
            {
                //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                //art1.Style.Add("display", "block");
                //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator ";
                return Json(new { Result = "Failed", Message = "Syntax Error. Contact administrator." });
            }
            //divdelete.Style.Add("display", "none");
        }

    }

}