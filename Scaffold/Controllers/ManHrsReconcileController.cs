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
    public class ManHrsReconcileController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        DataTable WorkedHrdt;
        // GET: SiteManHrs
        public ActionResult ManHrsReconcile()
        {
            
            return View();
        }

        public ActionResult showData(string id, string UID, DateTime EntryDate)
        {
            //int id = 0;
            //int UID = 0;

            //getTableData.Value = "";

            //if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            //{
            //    id = Convert.ToInt32(databaseCon.Decrypt(Request.QueryString["id"]));
            //}
            //if (Request.QueryString["UID"] != null && Request.QueryString["UID"] != "")
            //{
            //    UID = Convert.ToInt32(databaseCon.Decrypt(Request.QueryString["UID"]));
            //}

            string siteQuery = "select distinct tblSiteManHrs.SiteLocID,SiteLocation from tblSiteManHrs join tblSiteLocation on tblSiteManHrs.SiteLocId=tblSiteLocation.SiteLocID where EntryDate='" + Convert.ToDateTime(EntryDate).ToString("yyyy-MM-dd") + "' and ClientId='" + databaseCon.Decrypt(id) + "'";

            DataTable sitedt = databaseCon.GetData(siteQuery);
            string ColsiteId = "";
            ////string RowsiteId = "";
            //StringBuilder html = new StringBuilder();
            string html = "";
            string getRowCount = "";

            if (sitedt.Rows.Count > 0)
            {
                //html.Append("<table class='display cell-border nowrap' id='example' style='width:100%;color: #000;' border = '1'>");
                //html.Append("<thead>");
                //html.Append("<tr>");
                html = "<table class='display cell-border nowrap' id='example' style='width:100%;color: #000;' border = '1'>";
                html += "<thead>";
                html += "<tr>";
                int ColumnCount = 0;
                int colHeaderCount = 0; //For Taking Header On all Dynamic Columns
                for (int i = 0; i < sitedt.Rows.Count; i++)
                {
                    ColsiteId = sitedt.Rows[i]["SiteLocID"].ToString();

                    string Columnquery = "exec PRO_ManHrs '" + Convert.ToDateTime(EntryDate).ToString("yyyy-MM-dd") + "','" + databaseCon.Decrypt(id) + "','" + ColsiteId + "'";
                    DataTable columndt = databaseCon.GetData(Columnquery);

                    if (columndt.Columns.Count > 0)
                    {
                        int j = 2;
                        if (ColumnCount == 0)
                        {
                            j = 2;
                            //string WorkedHrQuery = "select tblSiteManHrs.UserId,tblUser.firstName +' '+tblUser.LastName as Resource,SUM(Isnull(NoOfWorkedHrs,0)) as [NoOfHoursWorked (For All Sites)] from tblUser left join tblSiteManHrs on tblUser.UserId=tblSiteManHrs.UserId where tblSiteManHrs.EntryDate='" + getEntryDate + "' group by tblSiteManHrs.UserId,firstName,LastName order by tblSiteManHrs.UserId";

                            //Commented on 24-05-2023 It was giving records twice 
                            //string WorkedHrQuery = "select tblUser.UserId,tblUser.firstName +' '+tblUser.LastName as Resource,SUM(Isnull(NoOfWorkedHrs,0)) as [Total Site Hours for All Sites], isnull(Category,'') as Category , sum(isnull(ErectionHrs,0)+isnull(ModificationHrs,0)+isnull(DismantleHrs,0)+isnull(AdditionalHrs,0)) as AllocatedHrs from tblUser left join tblSiteManHrs on tblUser.UserId = tblSiteManHrs.UserId and tblSiteManHrs.EntryDate ='" + Convert.ToDateTime(EntryDate).ToString("yyyy-MM-dd") + "' left join tblUserCategory on tblUser.CategoryId = tblUserCategory.UserCatId  left join  tblManHrs on tblSiteManHrs.UserId = tblManHrs.UserId and tblManHrs.EntryDate = '" + Convert.ToDateTime(EntryDate).ToString("yyyy-MM-dd") + "' and tblSiteManHrs.SiteLocId = tblManHrs.SiteLocId   where tblUser.ClientId = '" + databaseCon.Decrypt(id) + "' and Role!='SuperAdmin'  group by tblUser.UserId,firstName,LastName,Category order by tblUser.UserId";

                            string WorkedHrQuery = "select tblUser.UserId,tblUser.firstName +' '+tblUser.LastName as Resource,sum(Isnull(NoOfWorkedHrs,0))as [Total Site Hours for All Sites], isnull(Category, '') as Category, sum(isnull(AllocatedHrs,0)) as AllocatedHrs from tblUser left join tblUserCategory on tblUser.CategoryId = tblUserCategory.UserCatId left join (select userid, sum(Isnull(NoOfWorkedHrs, 0)) NoOfWorkedHrs from tblSiteManHrs where tblSiteManHrs.EntryDate = '" + Convert.ToDateTime(EntryDate).ToString("yyyy-MM-dd") + "' group by userid) tblSiteManHrs on tblUser.UserId = tblSiteManHrs.UserId left join(select userid, sum(isnull(ErectionHrs,0)+isnull(ModificationHrs, 0) + isnull(DismantleHrs, 0) + isnull(AdditionalHrs, 0)) as AllocatedHrs from tblManHrs where tblManHrs.EntryDate = '" + Convert.ToDateTime(EntryDate).ToString("yyyy-MM-dd") + "' group by userid) tblManHrs on tblSiteManHrs.UserId = tblManHrs.UserId where tblUser.ClientId = '" + databaseCon.Decrypt(id) + "' and Role!= 'SuperAdmin' group by tblUser.UserId,firstName,LastName,Category order by tblUser.UserId";


                            //"select tblUser.UserId,tblUser.firstName +' '+tblUser.LastName as Resource,SUM(Isnull(NoOfWorkedHrs,0)) as [Total Site Hours for All Sites], isnull(Category,'') as Category from tblUser left join tblSiteManHrs on tblUser.UserId=tblSiteManHrs.UserId and tblSiteManHrs.EntryDate='" + getEntryDate + "' left join tblUserCategory on tblUser.CategoryId=tblUserCategory.UserCatId where tblUser.ClientId='" + id + "' group by tblUser.UserId,firstName,LastName,Category order by tblUser.UserId";

                            WorkedHrdt = databaseCon.GetData(WorkedHrQuery);

                            if (WorkedHrdt.Rows.Count > 0)
                            {
                                if (colHeaderCount == 0 && i == 0)
                                {
                                   // html.Append("<td colspan='5' class='tblmainhdr'>For All Sites</td>");
                                    html += "<td colspan='4' class='tblmainhdr'>For All Sites</td>";
                                }
                                else
                                {
                                    //html.Append("<th class='tblheaders no-sort'></th>");
                                    //html.Append("<th class='tblheaders'>" + WorkedHrdt.Columns["Resource"].ToString() + "</th>");
                                    //html.Append("<th class='tblheaders'>" + WorkedHrdt.Columns["Category"].ToString() + "</th>");
                                    //html.Append("<th class='tblheaders no-sort'>" + WorkedHrdt.Columns["Total Site Hours for All Sites"].ToString() + "</th>");
                                    //html.Append("<th class='tblheaders no-sort'>Allocated Hrs</th>");
                                    html+="<th class='tblheaders no-sort'></th>";
                                    html += "<th class='tblheaders'>" + WorkedHrdt.Columns["Resource"].ToString() + "</th>";
                                    html += "<th class='tblheaders'>" + WorkedHrdt.Columns["Category"].ToString() + "</th>";

                                    //Commented on 24-05-2023 Client wanted to hide the column because it was creating a bit confusion for him.
                                    //html += "<th class='tblheaders no-sort'>" + WorkedHrdt.Columns["Total Site Hours for All Sites"].ToString() + "</th>";
                                    html += "<th class='tblheaders no-sort' style='display:none;'>" + WorkedHrdt.Columns["Total Site Hours for All Sites"].ToString() + "</th>";
                                    html += "<th class='tblheaders no-sort'>Allocated Hrs</th>";
                                }
                            }

                        }
                        else
                        {
                            j = 2;
                        }

                        if (colHeaderCount == 0)
                        {
                          //html.Append("<td class='tblmainhdr linecolor' colspan='" + ((columndt.Columns.Count) - j) + "'>" + sitedt.Rows[i]["SiteLocation"].ToString() + "</td>");
                          html+="<td class='tblmainhdr linecolor' colspan='" + ((columndt.Columns.Count) - j) + "'>" + sitedt.Rows[i]["SiteLocation"].ToString() + "</td>";
                        }
                        else
                        {
                            for (; j < columndt.Columns.Count; j++)
                            {
                                //html.Append("<th class='tblheaders linecolor'>" + columndt.Columns[j].ToString() + "</th>");
                                html+="<th class='tblheaders linecolor'>" + columndt.Columns[j].ToString() + "</th>";
                            }
                        }

                    }

                    ColumnCount++;

                    if (i == (sitedt.Rows.Count) - 1 && colHeaderCount == 0)
                    {
                        i = -1;
                        colHeaderCount++;
                        //html.Append("</tr>");
                        html+="</tr>";

                        ColumnCount = 0;
                    }
                }


                //html.Append("</thead>");

                //html.Append("<tbody>");
                html+="</thead>";

                html+="<tbody>";

                //Looping Starts For Dynamic Columns Data

                int RowCount = 0;

                string Countquery = "exec PRO_ManHrs '" + Convert.ToDateTime(EntryDate) + "','" + databaseCon.Decrypt(id) + "','" + sitedt.Rows[0]["SiteLocID"] + "'";
                DataTable Countdt = databaseCon.GetData(Countquery);

                if (Countdt.Rows.Count > 0)
                {
                    // getRowCount.Value = Countdt.Rows.Count.ToString();
                    //ViewBag.getRowCount= Countdt.Rows.Count.ToString();
                    getRowCount = Countdt.Rows.Count.ToString();
                    //K-loop is used for Looping Through Each row
                    for (int k = 0; k < Countdt.Rows.Count; k++)
                    {
                        //html.Append("<tr id='manhrRow" + k + "'>");
                        html+="<tr id='manhrRow" + k + "'>";
                        //M-Loop is used for Looping each row data for each site

                        for (int m = 0; m < sitedt.Rows.Count; m++)
                        {
                            string tempId = sitedt.Rows[m]["SiteLocID"].ToString();

                            string tempquery = "exec PRO_ManHrs '" + Convert.ToDateTime(EntryDate).ToString("yyyy-MM-dd") + "','" + databaseCon.Decrypt(id) + "','" + tempId + "'";
                            DataTable tempdt = databaseCon.GetData(tempquery);

                            int GetActualHrs = (tempdt.Columns.Count) - 1;

                            if (tempdt.Rows.Count > 0)
                            {
                                int l = 2;
                                if (m == 0)
                                {
                                    l = 2;

                                    if (WorkedHrdt.Rows.Count > 0)
                                    {
                                        //html.Append("<td class='tblcols '><input type='hidden' id='UserId" + k + "' value='" + WorkedHrdt.Rows[k]["UserId"].ToString() + "' /><i style='cursor:pointer;' class='editUser fa fa-pencil' aria-hidden='true'></i></td>");
                                        //html.Append("<td>" + WorkedHrdt.Rows[k]["Resource"].ToString() + "</td>");
                                        //html.Append("<td>" + WorkedHrdt.Rows[k]["Category"].ToString() + "</td>");
                                        //html.Append("<td class='tblcols'><label class='ExpectedHrsTotal" + k + "'>" + WorkedHrdt.Rows[k]["Total Site Hours for All Sites"].ToString() + "</label></td>");
                                        //html.Append("<td class='tblcols'><label class='getActualTotal ActualHrsTotal" + k + "'>" + WorkedHrdt.Rows[k]["AllocatedHrs"].ToString() + "</label></td>");
                                        html+="<td class='tblcols '><input type='hidden' id='UserId" + k + "' value='" + WorkedHrdt.Rows[k]["UserId"].ToString() + "' /><i style='cursor:pointer;' class='editUser fa fa-pencil' aria-hidden='true'></i></td>";
                                        html += "<td>" + WorkedHrdt.Rows[k]["Resource"].ToString() + "</td>";
                                        html += "<td>" + WorkedHrdt.Rows[k]["Category"].ToString() + "</td>";
                                        //Commented on 24-05-2023 Client wanted to hide the column because it was creating a bit confusion for him
                                        //html += "<td class='tblcols'><label class='ExpectedHrsTotal" + k + "'>" + WorkedHrdt.Rows[k]["Total Site Hours for All Sites"].ToString() + "</label></td>";

                                        html += "<td class='tblcols' style='display:none;'><label class='ExpectedHrsTotal" + k + "'>" + WorkedHrdt.Rows[k]["Total Site Hours for All Sites"].ToString() + "</label></td>";                                        
                                        html += "<td class='tblcols'><label class='getActualTotal ActualHrsTotal" + k + "'>" + WorkedHrdt.Rows[k]["AllocatedHrs"].ToString() + "</label></td>";
                                    }
                                }
                                else
                                {
                                    l = 2;
                                }

                                for (; l < tempdt.Columns.Count; l++)
                                {
                                    string colVal = tempdt.Rows[k][l].ToString();
                                    if (l == (tempdt.Columns.Count) - 1)
                                    {
                                        //html.Append("<td class='tblcols totalcolor '><label class='siteTotal" + k + "'>" + tempdt.Rows[k][l].ToString() + "</label></td>");
                                        html+="<td class='tblcols totalcolor '><label class='siteTotal" + k + "'>" + tempdt.Rows[k][l].ToString() + "</label></td>";
                                    }
                                    else
                                    {
                                        //html.Append("<td class='tblcols linecolor'>" + tempdt.Rows[k][l].ToString() + "</td>");
                                        html+="<td class='tblcols linecolor'>" + tempdt.Rows[k][l].ToString() + "</td>";
                                    }
                                }
                            }

                        }

                        //html.Append("</tr>");
                        html+="</tr>";
                        // }

                        RowCount++;
                    }
                }

                //html.Append("</tbody>");
                //html.Append("</table>");
                html+="</tbody>";
                html+="</table>";

                // btnExport.Style.Add("display", "block");
            }
            else
            {
                //html.Append("<div class='No records'>Please Go to Site Man Hours and assign resources for the selected date.</div>");
                html+="<div class='No records'>Please Go to Site Man Hours and assign resources for the selected date.</div>";

                //btnExport.Style.Add("display", "none");
            }

            //lbltabledata.InnerText = "";

            //lbltabledata.InnerText = html.ToString();

            string lbltabledata = "";

            lbltabledata = html.ToString();


            //PlaceHolder1.Controls.Add(new Literal { Text = html.ToString() });
            return Json(new { html = html, getRowCount = getRowCount, lbltabledata  = lbltabledata }, JsonRequestBehavior.AllowGet);
        }

    }

}