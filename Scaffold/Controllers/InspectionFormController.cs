using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Windows.Media.Imaging;
using Microsoft.WindowsAzure.Storage.Blob;
using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;

namespace Scaffold.Controllers
{
    public class InspectionFormController : Controller
    {
        ScaffoldEntities dbcontext = new ScaffoldEntities();
        // GET: InspectionForm
        public ActionResult InspectionForm()
        {

            string ClientQuery = "select ClientImage from tblClient where ClientId = '" + databaseCon.Decrypt(Request.QueryString["id"]) + "'";
            DataTable clientdt = databaseCon.GetData(ClientQuery);

            if (clientdt.Rows.Count > 0)
            {
                if (clientdt.Rows[0]["ClientImage"].ToString() != "")
                {
                    if (clientdt.Rows[0]["ClientImage"].ToString() == "")
                    {
                        //  imgLogo.Attributes.Add("src", "/img/logo1.png");
                        // ViewBag.ClientImage = "/img/logo1.png";
                        ViewBag.ClientImage = databaseCon.logoimage;
                    }
                    else
                    {
                        //imgLogo.Attributes.Add("src", "/images/clientlogo/" + clientdt.Rows[0]["ClientImage"].ToString());
                        ViewBag.ClientImage = "/images/clientlogo/" + clientdt.Rows[0]["ClientImage"].ToString();
                    }
                }
                else
                {
                    // imgLogo.Attributes.Add("src", "/img/logo1.png");
                    // ViewBag.ClientImage = "/img/logo1.png";
                    ViewBag.ClientImage = databaseCon.logoimage;
                }
            }
            return View();
        }

        public ActionResult ddlInspectedBy_OnChange(string id, string InspectedByID)
        {
            var LicenceNo = "";
            string userQuery = "select firstName+' '+LastName as name,UserId,LicenceNo from tblUser where IsInspector='true' and UserId='" + InspectedByID + "'";
            DataTable userDt = databaseCon.GetData(userQuery);
            if (userDt.Rows.Count > 0)
            {
                LicenceNo = userDt.Rows[0]["LicenceNo"].ToString();
            }
            return Json(LicenceNo, JsonRequestBehavior.AllowGet);
        }
        public ActionResult showData(string id, string UID, string RID, string InspID, string CheckListID, string hdnTimeOffSet)
        {
            string InspectionStatus = "";
            string inspstcode = "";

            string html = "";

            // Query Change Add  Join On ViewTblInspection Query on 24 Jan 2020
            string InspectQuery = " select REPLACE(CONVERT(VARCHAR(11), getdate(), 106), ' ', '-') as CurrentDate, InspectionTime,InspectionDate,InspectedById,CompleteInsp,InspStatus,vw_TblInspection.InspectedBy,vw_TblInspection.CreatedBy,vw_TblInspection.CreatedOn,OnlineInsp,Acknowledgment,CompleteInsp,vw_TblInspection.Lat,vw_TblInspection.Long,GPSAddress,Inspimage,Inspimage2,vw_TblInspection.PhotoAngle,PhotoAngle1,InspAudio,Inspvedio,InspSign,InspComments,InspStatus,MapImage,case when AckDate = '1900-01-01' then null else AckDate end as AckDate,AcknowledgeTimeZone,AckDateTime,LicenceNo,Right('000000' + Convert(varchar, ScaffRef), 6) as ScaffRef,DesignType,VW_TblRegister.SiteLocation as SiteName,tblSiteLocation.OwnerName as OwnerName from vw_TblInspection  join VW_TblRegister on vw_TblInspection.RegisterID = VW_TblRegister.RegisterId join tblSiteLocation on tblSiteLocation.SiteLocID = VW_TblRegister.SiteLocId where InspectionID = '" + databaseCon.Decrypt(InspID) + "'";

            DataTable InspectDt = databaseCon.GetData(InspectQuery);

            var InspectionHeaderData = dbcontext.Database.SqlQuery<Vw_TblInspectionViewModel>(InspectQuery).FirstOrDefault();
            var InspectionDate = InspectionHeaderData.InspectionDate;
            var AckDate = InspectionHeaderData.AckDate;
            if (InspectionDate != null)
            {
                InspectionHeaderData.InspectionDate = Convert.ToDateTime(InspectionDate).AddMinutes(Convert.ToDouble(hdnTimeOffSet));
            }
            if (AckDate != null)
            {
                InspectionHeaderData.AckDate = Convert.ToDateTime(AckDate).AddMinutes(Convert.ToDouble(hdnTimeOffSet));
            }


            if (InspectDt.Rows.Count > 0)
            {
                if (InspectDt.Rows[0]["InspStatus"].ToString() != "" && InspectDt.Rows[0]["CompleteInsp"].ToString() != "")
                {
                    InspectionStatus = InspectDt.Rows[0]["InspStatus"].ToString();
                    inspstcode = InspectDt.Rows[0]["CompleteInsp"].ToString();
                }
                if (InspectDt.Rows[0]["Inspimage"].ToString() != "")
                {
                    var uri = new Uri(databaseCon.siteimagelink);
                    var container = new CloudBlobContainer(uri);
                    var blob = container.GetBlockBlobReference(InspectDt.Rows[0]["Inspimage"].ToString());
                    var exists = blob.Exists();
                    if (exists == true)
                    {
                        string ImageUrl = databaseCon.siteimagelink + InspectDt.Rows[0]["Inspimage"].ToString();
                        InspectionHeaderData.Inspimage = ImageUrl;

                    }
                }
                if (InspectDt.Rows[0]["Inspimage2"].ToString() != "")
                {
                    var uri1 = new Uri(databaseCon.siteimagelink);
                    var container1 = new CloudBlobContainer(uri1);
                    var blob1 = container1.GetBlockBlobReference(InspectDt.Rows[0]["Inspimage2"].ToString());
                    var exists1 = blob1.Exists();
                    if (exists1 == true)
                    {
                        string ImageUrl = databaseCon.siteimagelink + InspectDt.Rows[0]["Inspimage2"].ToString();
                        InspectionHeaderData.Inspimage2 = ImageUrl;

                    }
                }


                //Inspection Sign Code Starts Here
                if (InspectDt.Rows[0]["InspSign"].ToString() != "")
                {
                    var uri = new Uri(databaseCon.siteimagelink);
                    var container = new CloudBlobContainer(uri);
                    var blob = container.GetBlockBlobReference(InspectDt.Rows[0]["InspSign"].ToString());
                    var exists = blob.Exists();
                    if (exists == true)
                    {
                        string SignUrl = databaseCon.siteimagelink + InspectDt.Rows[0]["InspSign"].ToString();
                        InspectionHeaderData.InspSign = SignUrl;
                    }
                }
                //Inspection Sign Code Ends Here


                //  if (InspectionStatus == "Confirmed" && inspstcode == "True")  // Comment On 13 Dec 2019
                if (((InspectionStatus == "Confirmed") || (InspectionStatus == "Out of Service")) && inspstcode == "True")
                {


                    //Added code on 10-Dec-2019 for InspectionCheckListColor Tick.
                    string queryInspection = "Select isnull(InspectionCheckListColor,0) as InspectionCheckListColor  from tblClient where ClientId='" + databaseCon.Decrypt(id) + "' ";
                    DataTable InspectionDt = databaseCon.GetData(queryInspection);

                    //Inspection Question List Table Starts Here
                    //Baans Comment Dynamic HTML table code starts here
                    // string query2 = "SELECT a.contentId,ChecklistItem,CheckListContent,Case when checklistItem IS null and subChildItem IS null then 1 else 0 end as Header FROM tblChecklistContent   a left join tblInspectionList On tblInspectionList.ContentID = a.ContentID where Case when checklistItem IS null and subChildItem IS null then 1 else 0 end = 1 and a.ClientID = '" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and a.checklistID = '" + databaseCon.Decrypt(Request.QueryString["CheckListID"]) + "' and tblInspectionList.InspectionID='" + databaseCon.Decrypt(Request.QueryString["InspID"]) + "' order by itemOrder ";

                    //Query Change On 20 Jan 2020 Add Order By In Checklist Header  Query

                    string query2 = "SELECT a.contentId,ChecklistItem,CheckListContent,Case when checklistItem IS null and subChildItem IS null then 1 else 0 end as Header,itemOrder FROM tblChecklistContent   a left join tblInspectionList On tblInspectionList.ContentID = a.ContentID where Case when checklistItem IS null and subChildItem IS null then 1 else 0 end = 1 and a.ClientID = '" + databaseCon.Decrypt(id) + "' and a.checklistID = '" + databaseCon.Decrypt(CheckListID) + "' and tblInspectionList.InspectionID='" + databaseCon.Decrypt(InspID) + "' order by itemOrder";

                    DataTable dt2 = databaseCon.GetData(query2);

                    // StringBuilder html = new StringBuilder();


                    for (int k = 0; k < dt2.Rows.Count; k++)
                    {
                        string HeaderId = dt2.Rows[k]["contentId"].ToString();

                        string HeaderName = dt2.Rows[k]["CheckListContent"].ToString();

                        html += "<div class='row' style='margin-top: 20px;'><div class='col-lg-12 col-md-12 col-sm-12 col-xs-12'> <span class='InspectionListTitle'>" + HeaderName + "</span> </div></div>";

                        //Baans change For removing Level3
                        // string query3 = "SELECT InspectionListID,InspectionID,a.contentId,ChecklistItem,SubChildItem, case when checklistItem is null and subChildItem is null then CONVERT(varchar,itemorder)+'.0' Else case when checklistItem is not null and subChildItem is not null then Convert(varchar,(select Convert(varchar, itemOrder) from tblChecklistContent where ContentID = a.checklistItem) )+'.' + (select Convert(varchar, itemOrder) from tblChecklistContent where ContentID = a.subChildItem)+'.' + CONVERT(varchar, itemorder) Else Convert(varchar, (select Convert(varchar, itemOrder) from tblChecklistContent where ContentID = a.checklistItem) )+'.' + CONVERT(varchar, itemorder) End End as itemorder, CheckListContent,a.ChecklistID,ISNULL( Y,'False') as Y,ISNULL( N,'False') as N,ISNULL( NA,'False') as NA,ActionRequired,IsNA FROM tblChecklistContent   a left join tblInspectionList On  tblInspectionList.ContentID=a.ContentID where SubChildItem is null and ChecklistItem = '" + HeaderId + "' and  a.ClientID='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and a.checklistID='" + databaseCon.Decrypt(Request.QueryString["CheckListID"]) + "' and tblInspectionList.InspectionID='" + databaseCon.Decrypt(Request.QueryString["InspID"]) + "' order by itemOrder";

                        //Query Change On 20 Jan 2020 Add Order By In itemOrder Checklist Question Query 

                        string query3 = "SELECT InspectionListID,InspectionID,a.contentId,ChecklistItem,SubChildItem,case when checklistItem is null and subChildItem is null then Right('00' + CONVERT(varchar, itemorder), 2)+'.0' Else case when checklistItem is not null and subChildItem is not null then Convert(varchar, (select Right('00' + Convert(varchar, itemOrder), 2) from tblChecklistContent where ContentID = a.checklistItem) )+'.' + (select Right('00' + Convert(varchar, itemOrder), 2) from tblChecklistContent where ContentID = a.subChildItem)+'.' + Right('000' + CONVERT(varchar, itemorder), 3) Else Right('00' + Convert(varchar, (select Convert(varchar, itemOrder) from tblChecklistContent where ContentID = a.checklistItem)),2)+'.' + Right('000' + CONVERT(varchar, itemorder), 3) End End as itemorder,CheckListContent,a.ChecklistID,ISNULL(Y, 'False') as Y,ISNULL(N, 'False') as N,ISNULL(NA, 'False') as NA,ActionRequired,IsNA FROM tblChecklistContent a left join tblInspectionList On  tblInspectionList.ContentID = a.ContentID where SubChildItem is null and ChecklistItem = '" + HeaderId + "' and  a.ClientID='" + databaseCon.Decrypt(id) + "' and a.checklistID='" + databaseCon.Decrypt(CheckListID) + "' and tblInspectionList.InspectionID='" + databaseCon.Decrypt(InspID) + "' order by itemOrder";

                        DataTable dt3 = databaseCon.GetData(query3);

                        if (dt3.Rows.Count > 0)
                        {
                            html += ("<table style='width:100%;' border = '1'>");

                            for (int i = 0; i < dt3.Rows.Count; i++)
                            {
                                string QuestName = dt3.Rows[i]["CheckListContent"].ToString();
                                string contentId = dt3.Rows[i]["contentId"].ToString();

                                string checkY = dt3.Rows[i]["Y"].ToString();
                                string checkN = dt3.Rows[i]["N"].ToString();
                                string checkNA = dt3.Rows[i]["NA"].ToString();

                                string IsNa = dt3.Rows[i]["IsNa"].ToString();

                                html += ("<tr><th style='width:40%;'>" + QuestName + "</th>");

                                html += ("<td style='width:60%;'><div class='row InspectionForm'> <div class='col-lg-1 col-md-1 col-sm-1 col-xs-1'> </div><div class='col-lg-2 col-md-2 col-sm-2 col-xs-2'>");

                                //Baans Comment to take checkbox in a condition
                                //Yes checkbox
                                if (checkY == "True")
                                {
                                    html += ("<input runat='server' type='checkbox' checked='checked' disabled value='YES' name='check" + contentId + "' id='chckY~" + contentId + "'>");

                                    //Added code on 10-Dec-2019 for InspectionCheckListColor Tick
                                    if (InspectionDt.Rows[0]["InspectionCheckListColor"].ToString() == "True")
                                    {
                                        html += ("<label class='chckyes1' for='chckY~" + contentId + "'></label> ");

                                    }
                                    else
                                    {
                                        html += ("<label class='chckyes' for='chckY~" + contentId + "'></label> ");

                                    }
                                    //html+=("<label class='chckyes' for='chckY~" + contentId + "'></label> ");

                                }
                                else
                                {
                                    html += ("<input runat='server' type='checkbox' value='YES' disabled name='check" + contentId + "'  id='chckY~" + contentId + "'>");
                                    //Added code on 10-Dec-2019 for InspectionCheckListColor Tick
                                    if (InspectionDt.Rows[0]["InspectionCheckListColor"].ToString() == "True")
                                    {
                                        html += ("<label class='chckyes1' for='chckY~" + contentId + "'></label> ");

                                    }
                                    else
                                    {
                                        html += ("<label class='chckyes' for='chckY~" + contentId + "'></label> ");

                                    }
                                    //html+=("<label class='chckyes' for='chckY~" + contentId + "'></label> ");
                                }


                                html += ("</div> <div class='col-lg-2 col-md-2 col-sm-2 col-xs-2'>");
                                //No checkbox
                                if (checkN == "True")
                                {
                                    html += ("<input runat='server' type='checkbox' checked='checked' disabled value='NO' name='check" + contentId + "' id='chckN~" + contentId + "'>");
                                    //Added code on 10-Dec-2019 for InspectionCheckListColor Tick
                                    if (InspectionDt.Rows[0]["InspectionCheckListColor"].ToString() == "True")
                                    {
                                        html += ("<label class='chckNo1' for='chckN~" + contentId + "'></label> ");
                                    }
                                    else
                                    {
                                        html += ("<label class='chckNo' for='chckN~" + contentId + "'></label> ");
                                    }
                                    // html+=("<label class='chckNo' for='chckN~" + contentId + "'></label> ");
                                }
                                else
                                {
                                    html += ("<input runat='server' type='checkbox' disabled value='NO' name='check" + contentId + "' id='chckN~" + contentId + "'>");
                                    //Added code on 10-Dec-2019 for InspectionCheckListColor Tick
                                    if (InspectionDt.Rows[0]["InspectionCheckListColor"].ToString() == "True")
                                    {
                                        html += ("<label class='chckNo1' for='chckN~" + contentId + "'></label> ");
                                    }
                                    else
                                    {
                                        html += ("<label class='chckNo' for='chckN~" + contentId + "'></label> ");
                                    }
                                    // html+=("<label class='chckNo' for='chckN~" + contentId + "'></label> ");
                                }


                                html += ("</div> <div class='col-lg-2 col-md-2 col-sm-2 col-xs-2'>");
                                //NA checkbox

                                if (IsNa == "True")
                                {
                                    if (checkNA == "True")
                                    {
                                        html += ("<input runat='server' type='checkbox' disabled checked='checked' value='NA' name='check" + contentId + "' id='chckNA~" + contentId + "'>");
                                        //Added code on 10-Dec-2019 for InspectionCheckListColor Tick
                                        if (InspectionDt.Rows[0]["InspectionCheckListColor"].ToString() == "True")
                                        {
                                            html += ("<label class='chckNA1' for='chckNA~" + contentId + "'></label> ");
                                        }
                                        else
                                        {
                                            html += ("<label class='chckNA' for='chckNA~" + contentId + "'></label> ");
                                        }
                                        //html+=("<label class='chckNA' for='chckNA~" + contentId + "'></label> ");
                                    }
                                    else
                                    {
                                        html += ("<input runat='server' type='checkbox' disabled value='NA' name='check" + contentId + "' id='chckNA~" + contentId + "'>");
                                        //Added code on 10-Dec-2019 for InspectionCheckListColor Tick
                                        if (InspectionDt.Rows[0]["InspectionCheckListColor"].ToString() == "True")
                                        {
                                            html += ("<label class='chckNA1' for='chckNA~" + contentId + "'></label> ");
                                        }
                                        else
                                        {
                                            html += ("<label class='chckNA' for='chckNA~" + contentId + "'></label> ");
                                        }
                                        //html+=("<label class='chckNA' for='chckNA~" + contentId + "'></label> ");
                                    }

                                }

                                html += ("</div> <div class='col-lg-4 col-md-4 col-sm-4 col-xs-4'>");
                                //Action Textbox
                                if (dt3.Rows[i]["Y"].ToString() != "")
                                {
                                    html += ("<input runat='server' type='text' value='" + dt3.Rows[i]["ActionRequired"].ToString() + "' name='Action" + contentId + "' style='width: 100%;' id='Action~" + contentId + "' readonly>");
                                }
                                else
                                {
                                    html += ("<input runat='server' type='text' name='Action" + contentId + "' style='width: 100%;' id='Action~" + contentId + "' readonly>");
                                }


                                html += ("</div> </div></td></tr>");
                            }

                            html += ("</table>");
                        }

                    }

                    // PlaceHolder1.Controls.Add(new Literal { Text = html.ToString() });

                }
                else
                {

                    //Added code on 9-Dec-2019 for InspectionCheckListColor Tick.
                    string queryInspection = "Select isnull(InspectionCheckListColor,0) as InspectionCheckListColor  from tblClient where ClientId='" + databaseCon.Decrypt(id) + "' ";
                    DataTable InspectionDt = databaseCon.GetData(queryInspection);

                    //Inspection Question List Table Starts Here
                    //Baans Comment Dynamic HTML table code starts here
                    // string query2 = "SELECT a.contentId,ChecklistItem,CheckListContent,Case when checklistItem IS null and subChildItem IS null then 1 else 0 end as Header FROM tblChecklistContent   a left join tblInspectionList On tblInspectionList.ContentID = a.ContentID where Case when checklistItem IS null and subChildItem IS null then 1 else 0 end = 1 and a.ClientID = '" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and a.checklistID = '" + databaseCon.Decrypt(Request.QueryString["CheckListID"]) + "' and tblInspectionList.InspectionID='" + databaseCon.Decrypt(Request.QueryString["InspID"]) + "' order by itemorder";

                    //Query Change On 20 Jan 2020 Add Order By In Checklist Header  Query

                    string query2 = "SELECT a.contentId,ChecklistItem,CheckListContent,Case when checklistItem IS null and subChildItem IS null then 1 else 0 end as Header,itemOrder FROM tblChecklistContent   a left join tblInspectionList On tblInspectionList.ContentID = a.ContentID where Case when checklistItem IS null and subChildItem IS null then 1 else 0 end = 1 and a.ClientID = '" + databaseCon.Decrypt(id) + "' and a.checklistID = '" + databaseCon.Decrypt(CheckListID) + "' and tblInspectionList.InspectionID='" + databaseCon.Decrypt(InspID) + "' order by itemOrder";


                    DataTable dt2 = databaseCon.GetData(query2);

                    //StringBuilder html = new StringBuilder();
                    // string html = "";

                    for (int k = 0; k < dt2.Rows.Count; k++)
                    {
                        string HeaderId = dt2.Rows[k]["contentId"].ToString();

                        string HeaderName = dt2.Rows[k]["CheckListContent"].ToString();

                        html += ("<div class='row' style='margin-top: 20px;'><div class='col-lg-12 col-md-12 col-sm-12 col-xs-12'> <span class='InspectionListTitle'>" + HeaderName + "</span> </div></div>");


                        //Baans change For removing Level3
                        //  string query3 = "SELECT InspectionListID,InspectionID,a.contentId,ChecklistItem,SubChildItem, case when checklistItem is null and subChildItem is null then CONVERT(varchar,itemorder)+'.0' Else case when checklistItem is not null and subChildItem is not null then Convert(varchar,(select Convert(varchar, itemOrder) from tblChecklistContent where ContentID = a.checklistItem) )+'.' + (select Convert(varchar, itemOrder) from tblChecklistContent where ContentID = a.subChildItem)+'.' + CONVERT(varchar, itemorder) Else Convert(varchar, (select Convert(varchar, itemOrder) from tblChecklistContent where ContentID = a.checklistItem) )+'.' + CONVERT(varchar, itemorder) End End as itemorder, CheckListContent,a.ChecklistID,ISNULL( Y,'False') as Y,ISNULL( N,'False') as N,ISNULL( NA,'False') as NA,ActionRequired,IsNA FROM tblChecklistContent   a left join tblInspectionList On  tblInspectionList.ContentID=a.ContentID where SubChildItem is null and ChecklistItem = '" + HeaderId + "' and  a.ClientID='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and a.checklistID='" + databaseCon.Decrypt(Request.QueryString["CheckListID"]) + "' and tblInspectionList.InspectionID='" + databaseCon.Decrypt(Request.QueryString["InspID"]) + "' order by itemOrder";


                        //Query Change On 20 Jan 2020 Add Order By In itemOrder Checklist Question Query 

                        string query3 = "SELECT InspectionListID,InspectionID,a.contentId,ChecklistItem,SubChildItem,case when checklistItem is null and subChildItem is null then Right('00' + CONVERT(varchar, itemorder), 2)+'.0' Else case when checklistItem is not null and subChildItem is not null then Convert(varchar, (select Right('00' + Convert(varchar, itemOrder), 2) from tblChecklistContent where ContentID = a.checklistItem) )+'.' + (select Right('00' + Convert(varchar, itemOrder), 2) from tblChecklistContent where ContentID = a.subChildItem)+'.' + Right('000' + CONVERT(varchar, itemorder), 3) Else Right('00' + Convert(varchar, (select Convert(varchar, itemOrder) from tblChecklistContent where ContentID = a.checklistItem)),2)+'.' + Right('000' + CONVERT(varchar, itemorder), 3) End End as itemorder,CheckListContent,a.ChecklistID,ISNULL(Y, 'False') as Y,ISNULL(N, 'False') as N,ISNULL(NA, 'False') as NA,ActionRequired,IsNA FROM tblChecklistContent a left join tblInspectionList On  tblInspectionList.ContentID = a.ContentID where SubChildItem is null and ChecklistItem = '" + HeaderId + "' and  a.ClientID='" + databaseCon.Decrypt(id) + "' and a.checklistID='" + databaseCon.Decrypt(CheckListID) + "' and tblInspectionList.InspectionID='" + databaseCon.Decrypt(InspID) + "' order by itemOrder";

                        DataTable dt3 = databaseCon.GetData(query3);

                        if (dt3.Rows.Count > 0)
                        {
                            html += ("<table style='width:100%;' border = '1'>");

                            for (int i = 0; i < dt3.Rows.Count; i++)
                            {
                                string QuestName = dt3.Rows[i]["CheckListContent"].ToString();
                                string contentId = dt3.Rows[i]["contentId"].ToString();

                                string checkY = dt3.Rows[i]["Y"].ToString();
                                string checkN = dt3.Rows[i]["N"].ToString();
                                string checkNA = dt3.Rows[i]["NA"].ToString();

                                string IsNa = dt3.Rows[i]["IsNa"].ToString();

                                html += ("<tr><th style='width:40%;'>" + QuestName + "</th>");

                                html += ("<td style='width:60%;'><div class='row InspectionForm'> <div class='col-lg-1 col-md-1 col-sm-1 col-xs-1'> </div><div class='col-lg-2 col-md-2 col-sm-2 col-xs-2'>");

                                //Baans Comment to take checkbox in a condition
                                //Yes checkbox
                                if (checkY == "True")
                                {
                                    html += ("<input class='checkboxYes' type='checkbox' checked='checked' value='YES' name='check" + contentId + "' id='chckY~" + contentId + "'>");
                                    //Added code on 9-Dec-2019 for InspectionCheckListColor Tick
                                    if (InspectionDt.Rows[0]["InspectionCheckListColor"].ToString() == "True")
                                    {
                                        html += ("<label class='chckyes1' for='chckY~" + contentId + "'></label> ");

                                    }
                                    else
                                    {
                                        html += ("<label class='chckyes' for='chckY~" + contentId + "'></label> ");

                                    }
                                    // html+=("<label class='chckyes' for='chckY~" + contentId + "'></label> ");
                                }
                                else
                                {
                                    html += ("<input class='checkboxYes' type='checkbox' value='YES' name='check" + contentId + "' id='chckY~" + contentId + "'>");
                                    //Added code on 9-Dec-2019 for InspectionCheckListColor Tick
                                    if (InspectionDt.Rows[0]["InspectionCheckListColor"].ToString() == "True")
                                    {
                                        html += ("<label class='chckyes1' for='chckY~" + contentId + "'></label> ");
                                    }
                                    else
                                    {
                                        html += ("<label class='chckyes' for='chckY~" + contentId + "'></label> ");
                                    }
                                    // html+=("<label class='chckyes' for='chckY~" + contentId + "'></label> ");

                                }


                                html += ("</div> <div class='col-lg-2 col-md-2 col-sm-2 col-xs-2'>");
                                //No checkbox
                                if (checkN == "True")
                                {
                                    html += ("<input class='checkboxNo' type='checkbox' checked='checked' value='NO' name='check" + contentId + "' id='chckN~" + contentId + "'>");
                                    //Added code on 9-Dec-2019 for InspectionCheckListColor Tick
                                    if (InspectionDt.Rows[0]["InspectionCheckListColor"].ToString() == "True")
                                    {
                                        html += ("<label class='chckNo1' for='chckN~" + contentId + "'></label> ");
                                    }
                                    else
                                    {
                                        html += ("<label class='chckNo' for='chckN~" + contentId + "'></label> ");
                                    }
                                    // html+=("<label class='chckNo' for='chckN~" + contentId + "'></label> ");
                                }
                                else
                                {
                                    html += ("<input class='checkboxNo' type='checkbox' value='NO' name='check" + contentId + "' id='chckN~" + contentId + "'>");
                                    //Added code on 9-Dec-2019 for InspectionCheckListColor Tick
                                    if (InspectionDt.Rows[0]["InspectionCheckListColor"].ToString() == "True")
                                    {
                                        html += ("<label class='chckNo1' for='chckN~" + contentId + "'></label> ");
                                    }
                                    else
                                    {
                                        html += ("<label class='chckNo' for='chckN~" + contentId + "'></label> ");
                                    }
                                    // html+=("<label class='chckNo' for='chckN~" + contentId + "'></label> ");
                                }


                                html += ("</div> <div class='col-lg-2 col-md-2 col-sm-2 col-xs-2'>");
                                //NA checkbox

                                if (IsNa == "True")
                                {
                                    if (checkNA == "True")
                                    {
                                        html += ("<input class='checkboxNA' type='checkbox' checked='checked' value='NA' name='check" + contentId + "' id='chckNA~" + contentId + "'>");
                                        //Added code on 9-Dec-2019 for InspectionCheckListColor Tick
                                        if (InspectionDt.Rows[0]["InspectionCheckListColor"].ToString() == "True")
                                        {
                                            html += ("<label class='chckNA1' for='chckNA~" + contentId + "'></label> ");
                                        }
                                        else
                                        {
                                            html += ("<label class='chckNA' for='chckNA~" + contentId + "'></label> ");
                                        }

                                        // html+=("<label class='chckNA' for='chckNA~" + contentId + "'></label> ");
                                    }
                                    else
                                    {
                                        html += ("<input class='checkboxNA' type='checkbox' value='NA' name='check" + contentId + "' id='chckNA~" + contentId + "'>");
                                        //Added code on 9-Dec-2019 for InspectionCheckListColor Tick
                                        if (InspectionDt.Rows[0]["InspectionCheckListColor"].ToString() == "True")
                                        {
                                            html += ("<label class='chckNA1' for='chckNA~" + contentId + "'></label> ");
                                        }
                                        else
                                        {
                                            html += ("<label class='chckNA' for='chckNA~" + contentId + "'></label> ");
                                        }
                                        // html+=("<label class='chckNA' for='chckNA~" + contentId + "'></label> ");
                                    }

                                }

                                html += ("</div> <div class='col-lg-4 col-md-4 col-sm-4 col-xs-4'>");
                                //Action Textbox
                                if (dt3.Rows[i]["Y"].ToString() != "")
                                {
                                    html += ("<input class='inputTypeActionRequired' type='text' value='" + dt3.Rows[i]["ActionRequired"].ToString() + "' name='Action" + contentId + "' style='width: 100%;' id='Action~" + contentId + "'>");
                                }
                                else
                                {
                                    html += ("<input class='inputTypeActionRequired' type='text' name='Action" + contentId + "' style='width: 100%;' id='Action~" + contentId + "'>");
                                }


                                html += ("</div> </div></td></tr>");
                            }

                            html += ("</table>");
                        }

                    }

                    // PlaceHolder1.Controls.Add(new Literal { Text = html.ToString() });
                    //Inspection Question List Table Ends Here

                }

            }
            return Json(new { InspectionHeaderData = InspectionHeaderData, InspectionFormData = html }, JsonRequestBehavior.AllowGet);
        }

        //  public ActionResult btnConfirmok_Click(string id, string InspID, string UID, string hdnTimeOffSet, string rdpInspectiondate, string InspectionStatus, string UserRole, string RID) //Commented Because We Are Not Using This


        public ActionResult btnConfirmok_Click(string id, string InspID, string UID, string hdnTimeOffSet, string rdpInspectiondate, string InspectionStatus)
        {
            var Status = "";
            var Message = "";

            try
            {

                //DateTime AcknowladgeDate = Convert.ToDateTime(hdnCurrentDate.Value);
                //var AcknowladgeDateOffSet = hdnTimeOffSet.Value;
                DateTime AcknowladgeDate = Convert.ToDateTime(rdpInspectiondate);
                var AcknowladgeDateOffSet = hdnTimeOffSet;

                var UserEmail = "";
                string userqry = "select EmailID from tbluser where UserId = '" + databaseCon.Decrypt(UID) + "'";
                DataTable dt = databaseCon.GetData(userqry);
                if (dt.Rows.Count > 0)
                {
                    UserEmail = dt.Rows[0]["EmailID"].ToString();
                }
                if (rdpInspectiondate != null && rdpInspectiondate != "")
                {
                    AcknowladgeDate = Convert.ToDateTime(rdpInspectiondate);
                }

                string inspstatusCheck = InspectionStatus;

                // string inspstatusCheck = "Confirmed";
                string InspectionStatusModal = "";
                if (inspstatusCheck != "")
                {

                    string querystr = "";
                    if (inspstatusCheck == "Confirmed")
                    {
                        InspectionStatusModal = "Confirmed";
                        //  querystr = "update tblinspection set  UpdatedBy='" + UserEmail + "', UpdatedOn='" + DateTime.Now + "', CompleteInsp='1',Acknowledgment='true',AckDate='" + rdpInspectiondate.SelectedDate + "',InspStatus='" + inspstatusCheck + "',IsOffline=0 where InspectionID ='" + databaseCon.Decrypt(Request.QueryString["InspID"]) + "'";
                        // Comment On 23 Jan 2020
                        // querystr = "update tblinspection set  UpdatedBy='" + UserEmail + "', UpdatedOn='" + DateTime.Now + "', CompleteInsp='1',Acknowledgment='true',AckDate='" + AcknowladgeDate + "',InspStatus='" + inspstatusCheck + "',IsOffline=0 where InspectionID ='" + databaseCon.Decrypt(Request.QueryString["InspID"]) + "'";

                        // Add TimeOffset In Acknoladgement Date And Save Utc Time 
                        querystr = "update tblinspection set  UpdatedBy='" + UserEmail + "', UpdatedOn='" + DateTime.Now + "', CompleteInsp='1',Acknowledgment='true',AckDate='" + AcknowladgeDate.AddMinutes(Convert.ToDouble(AcknowladgeDateOffSet)) + "',InspStatus='" + inspstatusCheck + "',IsOffline=0,AcknowledgeTimeZone=" + Convert.ToDouble((AcknowladgeDateOffSet)) * (-1) + " where InspectionID ='" + databaseCon.Decrypt(InspID) + "'";
                    }
                    else
                    {
                        InspectionStatusModal = "Out of Service";
                        //  querystr = "update tblinspection set  UpdatedBy='" + UserEmail + "', UpdatedOn='" + DateTime.Now + "', CompleteInsp='1',Acknowledgment='true',AckDate='" + rdpInspectiondate.SelectedDate + "',InspStatus='" + inspstatusCheck + "',IsOffline=0 where InspectionID ='" + databaseCon.Decrypt(Request.QueryString["InspID"]) + "'";

                        // Comment On 23 Jan 2020
                        // querystr = "update tblinspection set  UpdatedBy='" + UserEmail + "', UpdatedOn='" + DateTime.Now + "', CompleteInsp='1',Acknowledgment='true',AckDate='" + AcknowladgeDate + "',InspStatus='" + inspstatusCheck + "',IsOffline=0 where InspectionID ='" + databaseCon.Decrypt(Request.QueryString["InspID"]) + "'";

                        // Add TimeOffset In Acknoladgement Date And Save Utc Time 
                        querystr = "update tblinspection set  UpdatedBy='" + UserEmail + "', UpdatedOn='" + DateTime.Now + "', CompleteInsp='1',Acknowledgment='true',AckDate='" + AcknowladgeDate.AddMinutes(Convert.ToDouble(AcknowladgeDateOffSet)) + "',InspStatus='" + inspstatusCheck + "',IsOffline=0,AcknowledgeTimeZone=" + Convert.ToDouble((AcknowladgeDateOffSet)) * (-1) + " where InspectionID ='" + databaseCon.Decrypt(InspID) + "'";

                    }

                    int count = databaseCon.RunCmd(querystr);
                    if (count > 0)
                    {
                        // Upload(null, null);

                        // lblInspectionstatusmodal.InnerText = InspectionStatusModal;
                        //Response.Redirect("InspectionForm?UID=" + UID + "&UserRole=" + UserRole + "&id=" + id + "&RID=" + RID + "&CheckListID=" + Checklistid + "&InspID=" + InspectId +"&status=confirmed");
                        // confirmModal.Style.Add("display", "none");

                        // confirmContinueModal.Style.Add("display", "block");


                        // New Changes Add On 04 Dec 2019  If Inspection Status Is Confirm Redirect To Inspection List


                        //InspIDPDF = databaseCon.Decrypt(InspID);

                        var Result = databaseCon.InspectionPdfReportCreate(Convert.ToInt32(databaseCon.Decrypt(id)), Convert.ToInt32(databaseCon.Decrypt(InspID)));

                        var NewResult = Result.Split('|');
                        var FileName = NewResult[0];
                        var PdfFileFullPath = NewResult[1];


                        string path = databaseCon.registerdocumentlink + FileName;
                        var uri = new Uri(databaseCon.registerdocumentlink);
                        var container = new CloudBlobContainer(uri);
                        var blob = container.GetBlockBlobReference(FileName);
                        var exists = blob.Exists();
                        if (exists == false)
                        {
                        }
                        else
                        {
                            databaseCon.deletefileonblob(FileName, "registerdocument");
                        }
                        string extension = Path.GetExtension(".pdf");
                        if (extension.Replace(".", "") == "pdf")
                        {
                            extension = "application/pdf";
                        }

                        databaseCon.savefileonblob(FileName, "Inspection-Document", PdfFileFullPath, extension.Replace(".", ""));
                        if (System.IO.File.Exists(PdfFileFullPath))
                        {
                            try
                            {
                                System.IO.File.Delete(PdfFileFullPath);
                            }
                            catch
                            {

                            }
                        }



                        string query = "update tblinspection set UpdatedBy='" + UserEmail + "', UpdatedOn='" + DateTime.Now + "', InspFileName='" + FileName + "' where InspectionID ='" + databaseCon.Decrypt(InspID) + "'";
                        int result = databaseCon.RunCmd(query);
                        if (result > 0)
                        {
                            Status = "Success";
                            Message = "Data save Successfully .";
                        }
                        else
                        {
                            Status = "Failed";
                            Message = "Something Want wrong.";
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Status = "Failed";
                Message = ex.Message;
            }

            return Json(new { Result = Status, Message = Message }, JsonRequestBehavior.AllowGet);
            // divdelete.Style.Add("display", "none");
        }




        public ActionResult Upload(string id, string UID, string RID, string InspID, string CheckListID, string hdnlatitude, string hdnlongitude, string hdnAddress, string sketch_data, string tempAud, string tempVideo, string map_image, string imgangle, string imgangle1, string ddlInspectedBy, string lblInspectionLicence, string confirmcheck, string txtcomment, List<InspectionFormQuestionDataViewModel> InsFormModel)
        {
            var Result = "";
            var Message = "";
            string query;

            string Acknowledge = "";
            string comment = txtcomment;
            string newhdnlatitude = hdnlatitude;
            string newhdnlongitude = hdnlongitude;
            string newhdnAddress = hdnAddress;
            string sketchData = sketch_data;
            // string imageData = tempimg;
            string AudioData = tempAud;
            string videoData = tempVideo;
            string map_imageData = map_image;
            string PhotoAngle = imgangle;
            string PhotoAngle1 = imgangle1;
            //if (Request.Form["comment"] != "")
            //{
            //    // comment = Request.Form["comment"];
            //    comment = "Test";
            //}
            if (confirmcheck == "True")
            {
                Acknowledge = "true";
            }
            else
            {
                Acknowledge = "false";
            }


            if (map_imageData != "" && map_imageData != null)
            {
                if (map_imageData != "")
                {
                    MapImageUpload(map_imageData, InspID);
                }
            }

            if (sketchData != "" && sketchData != null)
            {
                if (sketchData != "")
                {
                    SignatureUpload(sketchData, InspID);
                }
            }

            //if (imageData != "" && imageData != null)
            //{
            //    imageUpload(imageData, InspID);
            //}

            if (AudioData != "" && AudioData != null)
            {
                AudioUpload(AudioData, InspID);
            }

            if (Request.Form["tempVideo"] != "" && Request.Form["tempVideo"] != null)
            {
                VideoUpload(videoData, InspID);
            }

            var UserEmail = "";
            string userqry = "select EmailID from tbluser where UserId = '" + databaseCon.Decrypt(UID) + "'";
            DataTable dt = databaseCon.GetData(userqry);
            if (dt.Rows.Count > 0)
            {
                UserEmail = dt.Rows[0]["EmailID"].ToString();
            }

            string SignoffQuery = "update tblinspection set  UpdatedBy='" + UserEmail + "', UpdatedOn='" + DateTime.Now + "', Lat='" + newhdnlatitude + "',Long='" + newhdnlongitude + "',GPSAddress='" + newhdnAddress.Replace("'", "''").Trim() + "', InspComments='" + comment.Replace("'", "''").Trim() + "',InspectedBy=(select firstName+' '+LastName as name from tblUser where IsInspector='true' and UserId='" + ddlInspectedBy + "'),InspectedById='" + ddlInspectedBy + "',LicenceNo='" + lblInspectionLicence + "',PhotoAngle='" + PhotoAngle + "',PhotoAngle1='" + PhotoAngle1 + "' where InspectionID='" + databaseCon.Decrypt(InspID) + "'";

            int signCount = databaseCon.RunCmd(SignoffQuery);

            string Newquery = "SELECT InspectionListID,InspectionID,RegisterId,a.contentId,ChecklistItem,SubChildItem, case when checklistItem is null and subChildItem is null then CONVERT(varchar,itemorder)+'.0' Else case when checklistItem is not null and subChildItem is not null then Convert(varchar,(select Convert(varchar, itemOrder) from tblChecklistContent where ContentID = a.checklistItem) )+'.' + (select Convert(varchar, itemOrder) from tblChecklistContent where ContentID = a.subChildItem)+'.' + CONVERT(varchar, itemorder) Else Convert(varchar, (select Convert(varchar, itemOrder) from tblChecklistContent where ContentID = a.checklistItem) )+'.' + CONVERT(varchar, itemorder) End End as itemorder, CheckListContent,a.ChecklistID,ISNULL( Y,'False') as Y,ISNULL( N,'False') as N,ISNULL( NA,'False') as NA,ActionRequired,IsNA FROM tblChecklistContent   a left join tblInspectionList On  tblInspectionList.ContentID=a.ContentID where SubChildItem is null and ChecklistItem is not null and a.ClientID='" + databaseCon.Decrypt(id) + "' and a.checklistID='" + databaseCon.Decrypt(CheckListID) + "' and tblInspectionList.InspectionID='" + databaseCon.Decrypt(InspID) + "' order by itemorder";

            DataTable Newdt = databaseCon.GetData(Newquery);

            if (Newdt.Rows.Count > 0)
            {
                for (int j = 0; j < Newdt.Rows.Count; j++)
                {

                    string ContentID = Newdt.Rows[j]["ContentID"].ToString();
                    string InspectionID = Newdt.Rows[j]["InspectionID"].ToString();
                    string RegisterId = Newdt.Rows[j]["RegisterId"].ToString();



                    foreach (InspectionFormQuestionDataViewModel item in InsFormModel)
                    {
                        var Array = item;
                        var chkID = Array.ContentID;
                        if (chkID == ContentID)
                        {
                            var Y = Array.ChkBoxYes;
                            var N = Array.ChkBoxNo;
                            var NA = Array.ChkBoxNA;
                            var Action = Array.Action;

                            //This Query will Reflect all ContentIds For Given InspectionId//

                            query = "update tblInspectionList set UpdatedBy='" + UserEmail + "', UpdatedOn='" + DateTime.Now + "',Y='" + Y + "',N='" + N + "',NA='" + NA + "',ActionRequired='" + Action + "' where ContentID='" + ContentID + "' and InspectionID='" + InspectionID + "'";

                            //Baans Changes 04-Dec-2019 start//

                            int result = databaseCon.RunCmd(query);

                            if (result > 0)
                            {
                                //art1.Style.Add("display", "block");
                                //art1.Attributes["class"] = "alert alert-success";
                                //alertlabel.Text = "<strong> Success ! </strong>Data saved successfully";
                                Result = "Success";
                                Message = "Data saved successfully .";
                            }
                            else
                            {
                                Result = "Failed";
                                Message = "something went wrong .";
                            }

                        }

                    }

                    //  RotatedImageSave(InspID);

                    //string checkValue = Request.Form["check" + ContentID];
                    //string Action = "";
                    //if (Request.Form["Action" + ContentID] != "")
                    //{
                    //    Action = Request.Form["Action" + ContentID];
                    //}
                    //else
                    //{
                    //    Action = "";
                    //}
                    //int Y = 0;
                    //int N = 0;
                    //int NA = 0;

                    //if (checkValue == "YES")
                    //{
                    //    Y = 1;
                    //    N = 0;
                    //    NA = 0;
                    //}
                    //else if (checkValue == "NO")
                    //{
                    //    Y = 0;
                    //    N = 1;
                    //    NA = 0;
                    //}
                    //else if (checkValue == "NA")
                    //{
                    //    Y = 0;
                    //    N = 0;
                    //    NA = 1;
                    //}

                    //Baans Changes 04-Dec-2019 start//

                    //This Query will Reflect all ContentIds Without Checking RegisterId Or  InspectionId//
                    // query = "update tblInspectionList set UpdatedOn='" + DateTime.Now + "',Y='" + Y + "',N='" + N + "',NA='" + NA + "',ActionRequired='" + Action + "' where ContentID='" + ContentID + "'";

                    //This Query will Reflect all ContentIds For Given RegisterId//
                    // query = "update tblInspectionList set UpdatedOn='" + DateTime.Now + "',Y='" + Y + "',N='" + N + "',NA='" + NA + "',ActionRequired='" + Action + "' where ContentID='" + ContentID + "' and RegisterId='"+ RegisterId + "'";

                    ////This Query will Reflect all ContentIds For Given InspectionId//

                    //query = "update tblInspectionList set UpdatedBy='" + UserEmail + "', UpdatedOn='" + DateTime.Now + "',Y='" + Y + "',N='" + N + "',NA='" + NA + "',ActionRequired='" + Action + "' where ContentID='" + ContentID + "' and InspectionID='" + InspectionID + "'";

                    ////Baans Changes 04-Dec-2019 start//

                    //int result = databaseCon.RunCmd(query);

                    //if (result > 0)
                    //{
                    //    //art1.Style.Add("display", "block");
                    //    //art1.Attributes["class"] = "alert alert-success";
                    //    //alertlabel.Text = "<strong> Success ! </strong>Data saved successfully";
                    //    Result = "Success";
                    //    Message = "Data saved successfully .";
                    //}else {
                    //  Result = "Failed";
                    //  Message = "something went wrong .";
                    //}
                }
            }
            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
            //showData();
            // saveContinueModal.Style.Add("display", "block");
        }

        public ActionResult UploadImagesSignature(string InspID, string tempimg, string tempimg2)
        {

            string imageData = tempimg;

            if (imageData != "" && imageData != null)
            {
                imageUpload(imageData, InspID);
            }


            return Json("Success", JsonRequestBehavior.AllowGet);
        }
        public ActionResult UploadImagesSignature1(string InspID, string tempimg2)
        {

            string imageData2 = tempimg2;

            if (imageData2 != "" && imageData2 != null)
            {
                imageUpload1(imageData2, InspID);
            }
            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        protected void imageUpload(string imageData, string InspID)
        {
            string originalPath = null;
            string compressedPath = null;

            try
            {
                string path = Server.MapPath("~/uploads/Temp/");
                string timestamp = DateTime.Now.Ticks.ToString();
                string decryptedInspID = databaseCon.Decrypt(InspID);

                string originalFileName = $"SnapInsp_{timestamp}_{decryptedInspID}_original.jpg";
                string compressedFileName = $"SnapInsp_{timestamp}_{decryptedInspID}_compressed.jpg";

                originalPath = Path.Combine(path, originalFileName);
                compressedPath = Path.Combine(path, compressedFileName);

                // Convert Base64 string to byte array
                byte[] bytes = Convert.FromBase64String(imageData.Split(',')[1]);

                // Save original image
                using (var imageFile = new FileStream(originalPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }

                // Load image and fix orientation
                using (Image img = Image.FromFile(originalPath))
                {
                    FixImageOrientation(img);

                    // Compress image
                    int newWidth = (int)(img.Width * 0.5);
                    int newHeight = (int)(img.Height * 0.5);

                    using (var thumbnailImg = new Bitmap(newWidth, newHeight))
                    {
                        using (var thumbGraph = Graphics.FromImage(thumbnailImg))
                        {
                            thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                            thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                            thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            thumbGraph.DrawImage(img, new Rectangle(0, 0, newWidth, newHeight));
                        }

                        // Get compressed byte array
                        byte[] compressedBytes = GetCompressedBitmap(thumbnailImg, 50);

                        // Save compressed image
                        using (MemoryStream ms = new MemoryStream(compressedBytes))
                        {
                            using (Image compressedImg = Image.FromStream(ms))
                            {
                                compressedImg.Save(compressedPath, compressedImg.RawFormat);
                            }
                        }
                    }
                } // Image is disposed here, releasing file lock

                // Upload compressed image to Blob Storage
                databaseCon.savefileonblob(compressedFileName, "Site-images", compressedPath, "image/jpeg");

                // Update database with the compressed image file name
                string query = $"UPDATE tblinspection SET Inspimage = '{compressedFileName}', SafariPhotoAngle = '0' WHERE InspectionID = '{decryptedInspID}'";
                databaseCon.RunCmd(query);
            }
            catch (Exception ex)
            {
                // Log error (implement your own logging method)
                Console.WriteLine("Error in imageUpload: " + ex.Message);
                // You might want to throw the exception or handle it based on your requirements
            }
            finally
            {
                // Guaranteed cleanup - delete files whether success or failure
                CleanupTempFile(originalPath, "original image");
                CleanupTempFile(compressedPath, "compressed image");
            }
        }

        protected void imageUpload1(string imageData2, string InspID)
        {
            string originalPath = null;
            string compressedPath = null;

            try
            {
                string path = Server.MapPath("~/uploads/Temp/");
                string timestamp = DateTime.Now.Ticks.ToString();
                string decryptedInspID = databaseCon.Decrypt(InspID);

                string originalFileName = $"SnapInsp1_{timestamp}_{decryptedInspID}_original.jpg";
                string compressedFileName = $"SnapInsp1_{timestamp}_{decryptedInspID}_compressed.jpg";

                originalPath = Path.Combine(path, originalFileName);
                compressedPath = Path.Combine(path, compressedFileName);

                byte[] bytes1 = Convert.FromBase64String(imageData2.Split(',')[1]);

                using (var imageFile = new FileStream(originalPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    imageFile.Write(bytes1, 0, bytes1.Length);
                    imageFile.Flush();
                }

                using (Image img = Image.FromFile(originalPath))
                {
                    FixImageOrientation(img);

                    int newWidth = (int)(img.Width * 0.5);
                    int newHeight = (int)(img.Height * 0.5);

                    using (var thumbnailImg = new Bitmap(newWidth, newHeight))
                    using (var thumbGraph = Graphics.FromImage(thumbnailImg))
                    {
                        thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                        thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                        thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        thumbGraph.DrawImage(img, new Rectangle(0, 0, newWidth, newHeight));

                        byte[] compressedBytes = GetCompressedBitmap(thumbnailImg, 50);
                        using (MemoryStream ms = new MemoryStream(compressedBytes))
                        using (Image compressedImg = Image.FromStream(ms))
                        {
                            compressedImg.Save(compressedPath, compressedImg.RawFormat);
                        }
                    }
                }

                // Upload to blob storage
                databaseCon.savefileonblob(compressedFileName, "Site-images", compressedPath, "image/jpeg");

                string querystr = $"UPDATE tblinspection SET Inspimage2 = '{compressedFileName}', SafariPhotoAngle1 = '0' WHERE InspectionID = '{decryptedInspID}'";
                databaseCon.RunCmd(querystr);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in imageUpload1: " + ex.Message);
            }
            finally
            {
                // Guaranteed cleanup - delete files whether success or failure
                CleanupTempFile(originalPath, "original image 1");
                CleanupTempFile(compressedPath, "compressed image 1");
            }
        }

        protected void SignatureUpload(string sketchData, string InspID)
        {
            string tempFilePath = null;

            try
            {
                string path = "~/uploads/";
                path = Server.MapPath(path);
                string FileName = "SketchInsp" + "_" + DateTime.Now.Ticks + "_" + databaseCon.Decrypt(InspID) + ".png";
                tempFilePath = path + FileName;

                byte[] bytes = Convert.FromBase64String(sketchData.Split(',')[1]);

                var uri = new Uri(databaseCon.siteimagelink);
                var container = new CloudBlobContainer(uri);
                var blob = container.GetBlockBlobReference(FileName);
                var exists = blob.Exists();

                if (exists)
                {
                    databaseCon.deletefileonblob(FileName, "siteimage");
                }

                using (var imageFile = new FileStream(tempFilePath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }

                // Upload to blob storage
                databaseCon.savefileonblob(FileName, "Site-images", tempFilePath, "image/png");

                // Update database
                string querystr = "update tblinspection set InspSign='" + FileName + "' where InspectionID ='" + databaseCon.Decrypt(InspID) + "'";
                int count = databaseCon.RunCmd(querystr);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in SignatureUpload: " + ex.Message);
            }
            finally
            {
                // Guaranteed cleanup
                CleanupTempFile(tempFilePath, "signature file");
            }
        }

        protected void MapImageUpload(string map_imageData, string InspID)
        {
            string tempFilePath = null;

            try
            {
                string path = "~/uploads/";
                path = Server.MapPath(path);
                string FileName = "MapInsp" + "_" + DateTime.Now.Ticks + "_" + databaseCon.Decrypt(InspID) + ".jpg";
                tempFilePath = path + FileName;

                byte[] bytes = Convert.FromBase64String(map_imageData.Split(',')[1]);

                var uri = new Uri(databaseCon.siteimagelink);
                var container = new CloudBlobContainer(uri);
                var blob = container.GetBlockBlobReference(FileName);
                var exists = blob.Exists();

                if (exists)
                {
                    databaseCon.deletefileonblob(FileName, "siteimage");
                }

                using (var imageFile = new FileStream(tempFilePath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }

                // Upload to blob storage
                databaseCon.savefileonblob(FileName, "Site-images", tempFilePath, "image/jpg");

                // Update database
                string querystr = "update tblinspection set MapImage='" + FileName + "' where InspectionID ='" + databaseCon.Decrypt(InspID) + "'";
                int count = databaseCon.RunCmd(querystr);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in MapImageUpload: " + ex.Message);
            }
            finally
            {
                // Guaranteed cleanup
                CleanupTempFile(tempFilePath, "map image file");
            }
        }

        protected void AudioUpload(string AudioData, string InspID)
        {
            string tempFilePath = null;

            try
            {
                string path = "~/uploads/";
                path = Server.MapPath(path);
                string FileName = "AudioInsp" + "_" + DateTime.Now.Ticks + "_" + databaseCon.Decrypt(InspID) + ".mp3";
                tempFilePath = path + FileName;

                byte[] dummyAudioData = Convert.FromBase64String(AudioData.Split(',')[1]);

                var uri = new Uri(databaseCon.siteimagelink);
                var container = new CloudBlobContainer(uri);
                var blob = container.GetBlockBlobReference(FileName);
                var exists = blob.Exists();

                if (exists)
                {
                    databaseCon.deletefileonblob(FileName, "siteimage");
                }

                using (var AudioFile = new FileStream(tempFilePath, FileMode.Create))
                {
                    AudioFile.Write(dummyAudioData, 0, dummyAudioData.Length);
                    AudioFile.Flush();
                }

                // Upload to blob storage
                databaseCon.savefileonblob(FileName, "Site-images", tempFilePath, "audio/mp3");

                // Update database
                string querystr = "update tblinspection set InspAudio='" + FileName + "' where InspectionID ='" + databaseCon.Decrypt(InspID) + "'";
                int count = databaseCon.RunCmd(querystr);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in AudioUpload: " + ex.Message);
            }
            finally
            {
                // Guaranteed cleanup
                CleanupTempFile(tempFilePath, "audio file");
            }
        }

        protected void VideoUpload(string videoData, string InspID)
        {
            string tempFilePath = null;

            try
            {
                string path = "~/uploads/";
                path = Server.MapPath(path);
                string FileName = "VideoInsp" + "_" + DateTime.Now.Ticks + "_" + databaseCon.Decrypt(InspID) + ".mov";
                tempFilePath = path + FileName;

                byte[] dummyVideoData = Convert.FromBase64String(videoData.Split(',')[1]);

                var uri = new Uri(databaseCon.siteimagelink);
                var container = new CloudBlobContainer(uri);
                var blob = container.GetBlockBlobReference(FileName);
                var exists = blob.Exists();

                if (exists)
                {
                    databaseCon.deletefileonblob(FileName, "siteimage");
                }

                using (var VideoFile = new FileStream(tempFilePath, FileMode.Create))
                {
                    VideoFile.Write(dummyVideoData, 0, dummyVideoData.Length);
                    VideoFile.Flush();
                }

                // Upload to blob storage
                databaseCon.savefileonblob(FileName, "Site-images", tempFilePath, "video/mov");

                // Update database
                string querystr = "update tblinspection set Inspvedio='" + FileName + "' where InspectionID ='" + databaseCon.Decrypt(InspID) + "'";
                int count = databaseCon.RunCmd(querystr);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in VideoUpload: " + ex.Message);
            }
            finally
            {
                // Guaranteed cleanup
                CleanupTempFile(tempFilePath, "video file");
            }
        }

        /// <summary>
        /// Helper method to safely delete temporary files with proper error handling
        /// </summary>
        /// <param name="filePath">Path to the file to delete</param>
        /// <param name="fileType">Description of file type for logging</param>
        private void CleanupTempFile(string filePath, string fileType)
        {
            if (string.IsNullOrEmpty(filePath))
                return;

            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    Console.WriteLine($"Successfully deleted temp {fileType}: {Path.GetFileName(filePath)}");
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Access denied when deleting temp {fileType} '{Path.GetFileName(filePath)}': {ex.Message}");
                // You might want to implement a retry mechanism or add to a cleanup queue
            }
            catch (IOException ex)
            {
                Console.WriteLine($"IO error when deleting temp {fileType} '{Path.GetFileName(filePath)}': {ex.Message}");
                // File might be in use, consider retry logic
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error when deleting temp {fileType} '{Path.GetFileName(filePath)}': {ex.Message}");
            }
        }

        /// <summary>
        /// Alternative cleanup method with retry logic for stubborn files
        /// </summary>
        /// <param name="filePath">Path to the file to delete</param>
        /// <param name="fileType">Description of file type for logging</param>
        /// <param name="maxRetries">Maximum number of retry attempts</param>
        private void CleanupTempFileWithRetry(string filePath, string fileType, int maxRetries = 3)
        {
            if (string.IsNullOrEmpty(filePath))
                return;

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                        Console.WriteLine($"Successfully deleted temp {fileType}: {Path.GetFileName(filePath)} on attempt {attempt}");
                        return; // Success, exit retry loop
                    }
                    else
                    {
                        return; // File doesn't exist, nothing to delete
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Attempt {attempt} failed to delete temp {fileType} '{Path.GetFileName(filePath)}': {ex.Message}");

                    if (attempt < maxRetries)
                    {
                        // Wait before retry (exponential backoff)
                        System.Threading.Thread.Sleep(100 * attempt);
                    }
                    else
                    {
                        Console.WriteLine($"Failed to delete temp {fileType} '{Path.GetFileName(filePath)}' after {maxRetries} attempts");
                        // You might want to add this file to a cleanup queue for later processing
                    }
                }
            }
        }



        private static byte[] GetCompressedBitmap(Bitmap bmp, long quality)
        {
            byte[] image;
            using (MemoryStream output = new MemoryStream())
            {
                EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                ImageCodecInfo imageCodec = ImageCodecInfo.GetImageEncoders().FirstOrDefault(o => o.FormatID == ImageFormat.Jpeg.Guid);
                EncoderParameters parameters = new EncoderParameters(1);
                parameters.Param[0] = qualityParam;
                bmp.Save(output, imageCodec, parameters);
                image = output.ToArray();
            }
            return image;
        }
        private void FixImageOrientation(Image img)
        {
            if (img.PropertyIdList.Contains(0x0112)) // Check if EXIF data exists
            {
                PropertyItem prop = img.GetPropertyItem(0x0112);
                int orientation = BitConverter.ToInt16(prop.Value, 0);

                switch (orientation)
                {
                    case 6:
                        img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    case 8:
                        img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                    case 3:
                        img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                }
            }
        }
        //protected void SignatureUpload(string sketchData, string InspID)
        //{
        //    string path = "~/uploads/";
        //    path = Server.MapPath(path);
        //    string FileName = "SketchInsp" + "_" + DateTime.Now.Ticks + "_" + databaseCon.Decrypt(InspID) + ".png";
        //    string SketchnewPath = path + FileName;


        //    byte[] bytes = Convert.FromBase64String(sketchData.Split(',')[1]);

        //    var uri = new Uri(databaseCon.siteimagelink);
        //    var container = new CloudBlobContainer(uri);
        //    var blob = container.GetBlockBlobReference(FileName);
        //    var exists = blob.Exists();
        //    if (exists == false)
        //    {
        //        using (var imageFile = new FileStream(SketchnewPath, FileMode.Create))
        //        {
        //            imageFile.Write(bytes, 0, bytes.Length);
        //            imageFile.Flush();
        //        }

        //        if (System.IO.File.Exists(SketchnewPath))
        //        {
        //            string fpath = path + FileName;
        //            databaseCon.savefileonblob(FileName, "Site-images", fpath, "image/png");
        //            System.IO.File.Delete(fpath);
        //        }
        //    }
        //    else
        //    {
        //        databaseCon.deletefileonblob(FileName, "siteimage");

        //        using (var imageFile = new FileStream(SketchnewPath, FileMode.Create))
        //        {
        //            imageFile.Write(bytes, 0, bytes.Length);
        //            imageFile.Flush();
        //        }

        //        if (System.IO.File.Exists(SketchnewPath))
        //        {
        //            string fpath = path + FileName;
        //            databaseCon.savefileonblob(FileName, "Site-images", fpath, "image/png");
        //            System.IO.File.Delete(fpath);
        //        }
        //    }


        //    string querystr = "update tblinspection set InspSign='" + FileName + "' where InspectionID ='" + databaseCon.Decrypt(InspID) + "'";
        //    int count = databaseCon.RunCmd(querystr);
        //}


        //protected void MapImageUpload(string map_imageData, string InspID)
        //{
        //    string path = "~/uploads/";
        //    path = Server.MapPath(path);
        //    string FileName = "MapInsp" + "_" + DateTime.Now.Ticks + "_" + databaseCon.Decrypt(InspID) + ".jpg";
        //    string newPath = path + FileName;

        //    byte[] bytes = Convert.FromBase64String(map_imageData.Split(',')[1]);

        //    var uri = new Uri(databaseCon.siteimagelink);
        //    var container = new CloudBlobContainer(uri);
        //    var blob = container.GetBlockBlobReference(FileName);
        //    var exists = blob.Exists();
        //    if (exists == false)
        //    {
        //        using (var imageFile = new FileStream(newPath, FileMode.Create))
        //        {
        //            imageFile.Write(bytes, 0, bytes.Length);
        //            imageFile.Flush();
        //        }


        //        if (System.IO.File.Exists(newPath))
        //        {
        //            string fpath = path + FileName;
        //            databaseCon.savefileonblob(FileName, "Site-images", fpath, "image/jpg");
        //            System.IO.File.Delete(fpath);
        //        }
        //    }
        //    else
        //    {
        //        databaseCon.deletefileonblob(FileName, "siteimage");

        //        using (var imageFile = new FileStream(newPath, FileMode.Create))
        //        {
        //            imageFile.Write(bytes, 0, bytes.Length);
        //            imageFile.Flush();
        //        }


        //        if (System.IO.File.Exists(newPath))
        //        {
        //            string fpath = path + FileName;
        //            databaseCon.savefileonblob(FileName, "Site-images", fpath, "image/jpg");
        //            System.IO.File.Delete(fpath);
        //        }
        //    }

        //    string querystr = "update tblinspection set MapImage='" + FileName + "' where InspectionID ='" + databaseCon.Decrypt(InspID) + "'";
        //    int count = databaseCon.RunCmd(querystr);
        //}


        //protected void imageUpload(string imageData, string InspID)
        //{
        //    try
        //    {
        //        string path = Server.MapPath("~/uploads/Temp/");
        //        string timestamp = DateTime.Now.Ticks.ToString();
        //        string decryptedInspID = databaseCon.Decrypt(InspID);

        //        string originalFileName = $"SnapInsp_{timestamp}_{decryptedInspID}_original.jpg";
        //        string compressedFileName = $"SnapInsp_{timestamp}_{decryptedInspID}_compressed.jpg";

        //        string originalPath = Path.Combine(path, originalFileName);
        //        string compressedPath = Path.Combine(path, compressedFileName);

        //        // Convert Base64 string to byte array
        //        byte[] bytes = Convert.FromBase64String(imageData.Split(',')[1]);

        //        // Save original image
        //        using (var imageFile = new FileStream(originalPath, FileMode.Create, FileAccess.Write, FileShare.None))
        //        {
        //            imageFile.Write(bytes, 0, bytes.Length);
        //            imageFile.Flush();
        //        }

        //        // Load image and fix orientation
        //        using (Image img = Image.FromFile(originalPath))
        //        {
        //            FixImageOrientation(img); // Fixes rotation issues if needed

        //            // Compress image
        //            int newWidth = (int)(img.Width * 0.5);
        //            int newHeight = (int)(img.Height * 0.5);

        //            using (var thumbnailImg = new Bitmap(newWidth, newHeight))
        //            {
        //                using (var thumbGraph = Graphics.FromImage(thumbnailImg))
        //                {
        //                    thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
        //                    thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
        //                    thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //                    thumbGraph.DrawImage(img, new Rectangle(0, 0, newWidth, newHeight));
        //                }

        //                // Get compressed byte array
        //                byte[] compressedBytes = GetCompressedBitmap(thumbnailImg, 50);

        //                // Save compressed image
        //                using (MemoryStream ms = new MemoryStream(compressedBytes))
        //                {
        //                    using (Image compressedImg = Image.FromStream(ms))
        //                    {
        //                        compressedImg.Save(compressedPath, compressedImg.RawFormat);
        //                    }
        //                }
        //            }
        //        } // Image is disposed here, releasing file lock

        //        // Upload compressed image to Blob Storage
        //        databaseCon.savefileonblob(compressedFileName, "Site-images", compressedPath, "image/jpeg");

        //        // Delete local files after upload
        //        if (System.IO.File.Exists(originalPath))
        //            System.IO.File.Delete(originalPath);
        //        if (System.IO.File.Exists(compressedPath))
        //            System.IO.File.Delete(compressedPath);

        //        // Update database with the compressed image file name
        //        string query = $"UPDATE tblinspection SET Inspimage = '{compressedFileName}', SafariPhotoAngle = '0' WHERE InspectionID = '{decryptedInspID}'";
        //        databaseCon.RunCmd(query);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log error (implement your own logging method)
        //        Console.WriteLine("Error in imageUpload: " + ex.Message);
        //    }
        //}

        //protected void imageUpload1(string imageData2, string InspID)
        //{
        //    string path = Server.MapPath("~/uploads/Temp/");
        //    string timestamp = DateTime.Now.Ticks.ToString();
        //    string decryptedInspID = databaseCon.Decrypt(InspID);

        //    string originalFileName = $"SnapInsp1_{timestamp}_{decryptedInspID}_original.jpg";
        //    string compressedFileName = $"SnapInsp1_{timestamp}_{decryptedInspID}_compressed.jpg";

        //    string originalPath = Path.Combine(path, originalFileName);
        //    string compressedPath = Path.Combine(path, compressedFileName);

        //    byte[] bytes1 = Convert.FromBase64String(imageData2.Split(',')[1]);

        //    using (var imageFile = new FileStream(originalPath, FileMode.Create, FileAccess.Write, FileShare.None))
        //    {
        //        imageFile.Write(bytes1, 0, bytes1.Length);
        //        imageFile.Flush();
        //    }

        //    using (Image img = Image.FromFile(originalPath))
        //    {
        //        if (img.PropertyIdList.Contains(0x0112))
        //        {
        //            PropertyItem propOrientation = img.GetPropertyItem(0x0112);
        //            short orientation = BitConverter.ToInt16(propOrientation.Value, 0);
        //            switch (orientation)
        //            {
        //                case 6:
        //                    img.RotateFlip(RotateFlipType.Rotate90FlipNone);
        //                    break;
        //                case 8:
        //                    img.RotateFlip(RotateFlipType.Rotate270FlipNone);
        //                    break;
        //                case 3:
        //                    img.RotateFlip(RotateFlipType.Rotate180FlipNone);
        //                    break;
        //            }
        //        }

        //        int newWidth = (int)(img.Width * 0.5);
        //        int newHeight = (int)(img.Height * 0.5);

        //        using (var thumbnailImg = new Bitmap(newWidth, newHeight))
        //        using (var thumbGraph = Graphics.FromImage(thumbnailImg))
        //        {
        //            thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
        //            thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
        //            thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //            thumbGraph.DrawImage(img, new Rectangle(0, 0, newWidth, newHeight));

        //            byte[] compressedBytes = GetCompressedBitmap(thumbnailImg, 50);
        //            using (MemoryStream ms = new MemoryStream(compressedBytes))
        //            using (Image compressedImg = Image.FromStream(ms))
        //            {
        //                compressedImg.Save(compressedPath, compressedImg.RawFormat);
        //            }
        //        }
        //    }

        //    // Upload to blob storage
        //    databaseCon.savefileonblob(compressedFileName, "Site-images", compressedPath, "image/jpeg");

        //    if (System.IO.File.Exists(originalPath))
        //        System.IO.File.Delete(originalPath);

        //    if (System.IO.File.Exists(compressedPath))
        //        System.IO.File.Delete(compressedPath);

        //    string querystr = $"UPDATE tblinspection SET Inspimage2 = '{compressedFileName}', SafariPhotoAngle1 = '0' WHERE InspectionID = '{decryptedInspID}'";
        //    databaseCon.RunCmd(querystr);
        //}
        //// update method on  05 march 2025  for the delete temp image from local after upload image on server 
        //protected void AudioUpload(string AudioData, string InspID)
        //{
        //    string path = "~/uploads/";
        //    path = Server.MapPath(path);
        //    string FileName = "AudioInsp" + "_" + DateTime.Now.Ticks + "_" + databaseCon.Decrypt(InspID) + ".mp3";
        //    string AudionewPath = path + FileName;

        //    byte[] dummyAudioData = Convert.FromBase64String(AudioData.Split(',')[1]);

        //    var uri = new Uri(databaseCon.siteimagelink);
        //    var container = new CloudBlobContainer(uri);
        //    var blob = container.GetBlockBlobReference(FileName);
        //    var exists = blob.Exists();
        //    if (exists == false)
        //    {
        //        using (var AudioFile = new FileStream(AudionewPath, FileMode.Create))
        //        {
        //            AudioFile.Write(dummyAudioData, 0, dummyAudioData.Length);
        //            AudioFile.Flush();
        //        }

        //        if (System.IO.File.Exists(AudionewPath))
        //        {
        //            string fpath = path + FileName;
        //            databaseCon.savefileonblob(FileName, "Site-images", fpath, "audio/mp3");
        //            System.IO.File.Delete(fpath);
        //        }
        //    }
        //    else
        //    {
        //        databaseCon.deletefileonblob(FileName, "siteimage");
        //        using (var AudioFile = new FileStream(AudionewPath, FileMode.Create))
        //        {
        //            AudioFile.Write(dummyAudioData, 0, dummyAudioData.Length);
        //            AudioFile.Flush();
        //        }

        //        if (System.IO.File.Exists(AudionewPath))
        //        {
        //            string fpath = path + FileName;
        //            databaseCon.savefileonblob(FileName, "Site-images", fpath, "audio/mp3");
        //            System.IO.File.Delete(fpath);
        //        }
        //    }

        //    string querystr = "update tblinspection set InspAudio='" + FileName + "' where InspectionID ='" + databaseCon.Decrypt(InspID) + "'";
        //    int count = databaseCon.RunCmd(querystr);
        //}

        //protected void VideoUpload(string videoData, string InspID)
        //{
        //    string path = "~/uploads/";
        //    path = Server.MapPath(path);
        //    string FileName = "VideoInsp" + "_" + DateTime.Now.Ticks + "_" + databaseCon.Decrypt(InspID) + ".mov";
        //    string VideonewPath = path + FileName;

        //    byte[] dummyVideoData = Convert.FromBase64String(videoData.Split(',')[1]);

        //    var uri = new Uri(databaseCon.siteimagelink);
        //    var container = new CloudBlobContainer(uri);
        //    var blob = container.GetBlockBlobReference(FileName);
        //    var exists = blob.Exists();
        //    if (exists == false)
        //    {
        //        using (var VideoFile = new FileStream(VideonewPath, FileMode.Create))
        //        {
        //            VideoFile.Write(dummyVideoData, 0, dummyVideoData.Length);
        //            VideoFile.Flush();
        //        }

        //        if (System.IO.File.Exists(VideonewPath))
        //        {
        //            string fpath = path + FileName;
        //            databaseCon.savefileonblob(FileName, "Site-images", fpath, "video/mov");
        //            System.IO.File.Delete(fpath);
        //        }
        //    }
        //    else
        //    {
        //        databaseCon.deletefileonblob(FileName, "siteimage");
        //        using (var VideoFile = new FileStream(VideonewPath, FileMode.Create))
        //        {
        //            VideoFile.Write(dummyVideoData, 0, dummyVideoData.Length);
        //            VideoFile.Flush();
        //        }

        //        if (System.IO.File.Exists(VideonewPath))
        //        {
        //            string fpath = path + FileName;
        //            databaseCon.savefileonblob(FileName, "Site-images", fpath, "video/mov");
        //            System.IO.File.Delete(fpath);
        //        }
        //    }

        //    string querystr = "update tblinspection set Inspvedio='" + FileName + "' where InspectionID ='" + databaseCon.Decrypt(InspID) + "'";
        //    int count = databaseCon.RunCmd(querystr);
        //}







        //public void RotatedImageSave(string InspID)
        //{
        //    var PhotoAngle = "0";
        //    //string query = "select RegisterId, Image, PhotoAngle from tblRegister where RegisterId ='" + RID + "'";
        //    string query = "select Inspimage, PhotoAngle from tblinspection where InspectionID ='" + databaseCon.Decrypt(InspID) + "'";

        //     DataTable dt = databaseCon.GetData(query);
        //    if (dt.Rows.Count > 0)
        //    {

        //        var ImageName = dt.Rows[0]["Inspimage"].ToString();

        //        //if (imgangle.Value == "0")
        //        //{
        //        //    PhotoAngle = dt.Rows[0]["PhotoAngle"].ToString();
        //        //}
        //        //else
        //        //{
        //        //    PhotoAngle = imgangle.Value;
        //        //}
        //       // PhotoAngle = ViewState["PhotoAngle"].ToString();
        //        PhotoAngle = dt.Rows[0]["PhotoAngle"].ToString(); 
        //        if (ImageName != "")
        //        {
        //            if (PhotoAngle != "0")
        //            {
        //               // string ImagePath = Server.MapPath("/images/sites/" + ImageName);
        //                string ImagePath = databaseCon.siteimagelink + dt.Rows[0]["Inspimage"].ToString();
        //                //create an image object from the image in that path
        //                System.Drawing.Image img = System.Drawing.Image.FromFile(ImagePath);

        //                //Rotate the image in memory


        //                if (PhotoAngle == "-90" || PhotoAngle == "270")
        //                {
        //                    img.RotateFlip(RotateFlipType.Rotate270FlipNone);
        //                }
        //                else if (PhotoAngle == "-180" || PhotoAngle == "180")
        //                {
        //                    img.RotateFlip(RotateFlipType.Rotate180FlipNone);
        //                }
        //                else if (PhotoAngle == "-270" || PhotoAngle == "90")
        //                {
        //                    img.RotateFlip(RotateFlipType.Rotate90FlipNone);
        //                }

        //                // Delete the file so the new image can be saved
        //                //if (File.Exists(ImagePath))
        //                //{
        //                //    System.IO.File.Delete(ImagePath);
        //                //}

        //                //save the image out to the file
        //                img.Save(ImagePath);

        //                //release image file
        //                img.Dispose();
        //                //string UpdateQuery = "update tblRegister set PhotoAngle='"+ PhotoAngle + "' where RegisterId ='" + RID + "'";
        //                string UpdateQuery = "update tblinspection set PhotoAngle='0' where InspectionID ='" + databaseCon.Decrypt(InspID) + "'";
        //                int i = databaseCon.RunCmd(UpdateQuery);
        //            }
        //        }
        //    }
        //}

        //protected void imageUpload(string imageData,  string InspID)
        //{
        //    //string path = "~/uploads/";
        //    string path = "~/uploads/Temp/";
        //    path = Server.MapPath(path);
        //    string FileName = "SnapInsp" + "_" + DateTime.Now.Ticks + "_" + databaseCon.Decrypt(InspID) + ".jpg";

        //    string newPath = path + FileName;
        //    var PhotoAngle = "0";

        //    var tempImage = imageData;
        //    string NewFileName = "";

        //    byte[] bytes = Convert.FromBase64String(imageData.Split(',')[1]);

        //    var uri = new Uri(databaseCon.siteimagelink);
        //    var container = new CloudBlobContainer(uri);
        //    var blob = container.GetBlockBlobReference(FileName);

        //    var exists = blob.Exists();
        //    if (exists == false)
        //    {
        //        using (var imageFile = new FileStream(newPath, FileMode.Create))
        //        {
        //            imageFile.Write(bytes, 0, bytes.Length);

        //            imageFile.Flush();
        //        }

        //        // Code Added on 11 Aug 2020
        //        //  var FileP = "https://inpexdocumenttest.blob.core.windows.net/siteimage/SnapInsp_637317997199338642_7174.jpg";
        //        //  var bitmapImage = new BitmapImage(); 
        //        //  bitmapImage.UriSource = new Uri("https://inpexdocumenttest.blob.core.windows.net/siteimage/SnapInsp_637317997199338642_7174.jpg", UriKind.RelativeOrAbsolute);
        //        // imgProf.Source = bitmapImage;
        //        Image img = Image.FromFile(newPath);
        //        // Image img1 = Image.FromHbitmap(bitmapImage.UriSource);

        //        // BitmapImage image = new BitmapImage(new Uri("https://inpexdocumenttest.blob.core.windows.net/siteimage/SnapInsp_637317997199338642_7174.jpg", UriKind.Relative));


        //        if (img.PropertyIdList.Contains(0x0112))
        //        {

        //            PropertyItem propOrientation = img.GetPropertyItem(0x0112);
        //            short orientation = BitConverter.ToInt16(propOrientation.Value, 0);
        //            if (orientation == 6)
        //            {
        //                img.RotateFlip(RotateFlipType.Rotate90FlipNone);
        //            }
        //            else if (orientation == 8)
        //            {
        //                img.RotateFlip(RotateFlipType.Rotate270FlipNone);
        //            }
        //            else if (orientation == 3)
        //            {
        //                img.RotateFlip(RotateFlipType.Rotate180FlipNone);
        //            }
        //            else
        //            {
        //                // Do nothing
        //            }
        //            //else if (orientation == 8)
        //            //{
        //            //    img.RotateFlip(RotateFlipType.Rotate270FlipNone);
        //            //}
        //            //else
        //            //{
        //            //    // Do nothing
        //            //}
        //            NewFileName = "SnapInsp" + "_" + DateTime.Now.Ticks + "_" + databaseCon.Decrypt(InspID) + ".jpg";

        //            tempImage = imageData;
        //            //string filename = NewFileName;
        //            var newWidth = (int)(img.Width * 0.5);
        //            var newHeight = (int)(img.Height * 0.5);
        //            var thumbnailImg = new Bitmap(newWidth, newHeight);
        //            var thumbGraph = Graphics.FromImage(thumbnailImg);
        //            thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
        //            thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
        //            thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //            var imageRectangle = new System.Drawing.Rectangle(0, 0, newWidth, newHeight);

        //            thumbGraph.DrawImage(img, imageRectangle);

        //            byte[] bte = GetCompressedBitmap(thumbnailImg, 75);
        //            MemoryStream ms2 = new MemoryStream(bte);
        //            System.Drawing.Image img12 = System.Drawing.Image.FromStream(ms2);

        //            thumbnailImg.Save(path + NewFileName, img12.RawFormat);

        //        }
        //        else
        //        {
        //            NewFileName = "SnapInsp" + "_" + DateTime.Now.Ticks + "_" + databaseCon.Decrypt(InspID) + ".jpg";

        //            tempImage = imageData;
        //            //string filename = NewFileName;
        //            var newWidth = (int)(img.Width * 0.5);
        //            var newHeight = (int)(img.Height * 0.5);
        //            var thumbnailImg = new Bitmap(newWidth, newHeight);
        //            var thumbGraph = Graphics.FromImage(thumbnailImg);
        //            thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
        //            thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
        //            thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //            var imageRectangle = new System.Drawing.Rectangle(0, 0, newWidth, newHeight);

        //            thumbGraph.DrawImage(img, imageRectangle);

        //            byte[] bte = GetCompressedBitmap(thumbnailImg, 50);
        //            MemoryStream ms2 = new MemoryStream(bte);
        //            System.Drawing.Image img12 = System.Drawing.Image.FromStream(ms2);

        //            thumbnailImg.Save(path + NewFileName, img12.RawFormat);
        //        }
        //        //private const int exifOrientationID = 0x112; //274

        //        //public static void ExifRotate(this Image img)
        //        //{
        //        //    if (!img.PropertyIdList.Contains(exifOrientationID))
        //        //        return;

        //        //    var prop = img.GetPropertyItem(exifOrientationID);
        //        //    int val = BitConverter.ToUInt16(prop.Value, 0);
        //        //    var rot = RotateFlipType.RotateNoneFlipNone;

        //        //    if (val == 3 || val == 4)
        //        //        rot = RotateFlipType.Rotate180FlipNone;
        //        //    else if (val == 5 || val == 6)
        //        //        rot = RotateFlipType.Rotate90FlipNone;
        //        //    else if (val == 7 || val == 8)
        //        //        rot = RotateFlipType.Rotate270FlipNone;

        //        //    if (val == 2 || val == 4 || val == 5 || val == 7)
        //        //        rot |= RotateFlipType.RotateNoneFlipX;

        //        //    if (rot != RotateFlipType.RotateNoneFlipNone)
        //        //        img.RotateFlip(rot);
        //        //}


        //        // Code Ended on 11 Aug 2020
        //        //if (System.IO.File.Exists(tempFilePath))
        //        //{
        //        //    System.IO.File.Copy(tempFilePath, newPath);
        //        //    System.IO.File.Delete(tempFilePath);
        //        //}



        //        if (System.IO.File.Exists(newPath))
        //        {
        //            string fpath = path + NewFileName;
        //            databaseCon.savefileonblob(NewFileName, "Site-images", fpath, "image/jpeg");
        //            // System.IO.File.Delete(fpath);

        //        }
        //    }
        //    else
        //    {
        //        databaseCon.deletefileonblob(FileName, "siteimage");

        //        using (var imageFile = new FileStream(newPath, FileMode.Create))
        //        {
        //            imageFile.Write(bytes, 0, bytes.Length);
        //            imageFile.Flush();
        //        }


        //        if (System.IO.File.Exists(newPath))
        //        {
        //            string fpath = path + FileName;
        //            databaseCon.savefileonblob(FileName, "Site-images", fpath, "image/jpeg");
        //            System.IO.File.Delete(fpath);
        //        }
        //    }


        //    // string querystr = "update tblinspection set Inspimage='" + FileName + "' where InspectionID ='" + databaseCon.Decrypt(InspID) + "'";
        //    string querystr = "update tblinspection set Inspimage ='" + NewFileName + "' ,SafariPhotoAngle='" + PhotoAngle + "'  where InspectionID ='" + databaseCon.Decrypt(InspID) + "'";
        //    // string querystr = "update tblinspection set Inspimage2='" + FileName + "',SafariPhotoAngle='" + PhotoAngle + "'  where InspectionID ='" + databaseCon.Decrypt(InspID) + "'";

        //    int count = databaseCon.RunCmd(querystr);
        //}

        //protected void imageUpload1(string imageData2, string InspID)
        //{
        //    string path = "~/uploads/Temp/";
        //    path = Server.MapPath(path);
        //    string FileName1 = "SnapInsp1" + "_" + DateTime.Now.Ticks + "_" + databaseCon.Decrypt(InspID) + ".jpg";

        //    string newPath1 = path + FileName1;
        //    var PhotoAngle1 = "0";
        //    string NewFileName = "";

        //    var tempImage = imageData2;

        //    byte[] bytes1 = Convert.FromBase64String(imageData2.Split(',')[1]);
        //    var uri1 = new Uri(databaseCon.siteimagelink);
        //    var container1 = new CloudBlobContainer(uri1);
        //    var blob1 = container1.GetBlockBlobReference(FileName1);
        //    var exists1 = blob1.Exists();
        //    if (exists1 == false)
        //    {
        //        using (var imageFile = new FileStream(newPath1, FileMode.Create))
        //        {
        //            imageFile.Write(bytes1, 0, bytes1.Length);
        //            imageFile.Flush();
        //        }
        //        Image img = Image.FromFile(newPath1);

        //        if (img.PropertyIdList.Contains(0x0112))
        //        {
        //            PropertyItem propOrientation = img.GetPropertyItem(0x0112);
        //            short orientation = BitConverter.ToInt16(propOrientation.Value, 0);
        //            if (orientation == 6)
        //            {
        //                img.RotateFlip(RotateFlipType.Rotate90FlipNone);
        //            }
        //            else if (orientation == 8)
        //            {
        //                img.RotateFlip(RotateFlipType.Rotate270FlipNone);
        //            }
        //            else if (orientation == 3)
        //            {
        //                img.RotateFlip(RotateFlipType.Rotate180FlipNone);
        //            }
        //            else
        //            {
        //                // Do nothing
        //            }

        //            NewFileName = "SnapInsp" + "_" + DateTime.Now.Ticks + "_" + databaseCon.Decrypt(InspID) + ".jpg";

        //            tempImage = imageData2;
        //            //string filename = NewFileName;
        //            var newWidth = (int)(img.Width * 0.5);
        //            var newHeight = (int)(img.Height * 0.5);
        //            var thumbnailImg = new Bitmap(newWidth, newHeight);
        //            var thumbGraph = Graphics.FromImage(thumbnailImg);
        //            thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
        //            thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
        //            thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //            var imageRectangle = new System.Drawing.Rectangle(0, 0, newWidth, newHeight);

        //            thumbGraph.DrawImage(img, imageRectangle);

        //            byte[] bte = GetCompressedBitmap(thumbnailImg, 75);
        //            MemoryStream ms2 = new MemoryStream(bte);
        //            System.Drawing.Image img12 = System.Drawing.Image.FromStream(ms2);

        //            thumbnailImg.Save(path + NewFileName, img12.RawFormat);


        //        }
        //        else 
        //        {
        //            NewFileName = "SnapInsp" + "_" + DateTime.Now.Ticks + "_" + databaseCon.Decrypt(InspID) + ".jpg";

        //            tempImage = imageData2;
        //            //string filename = NewFileName;
        //            var newWidth = (int)(img.Width * 0.5);
        //            var newHeight = (int)(img.Height * 0.5);
        //            var thumbnailImg = new Bitmap(newWidth, newHeight);
        //            var thumbGraph = Graphics.FromImage(thumbnailImg);
        //            thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
        //            thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
        //            thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //            var imageRectangle = new System.Drawing.Rectangle(0, 0, newWidth, newHeight);

        //            thumbGraph.DrawImage(img, imageRectangle);

        //            byte[] bte = GetCompressedBitmap(thumbnailImg, 50);
        //            MemoryStream ms2 = new MemoryStream(bte);
        //            System.Drawing.Image img12 = System.Drawing.Image.FromStream(ms2);

        //            thumbnailImg.Save(path + NewFileName, img12.RawFormat);
        //        }
        //        if (System.IO.File.Exists(newPath1))
        //        {
        //            string fpath = path + NewFileName;
        //            databaseCon.savefileonblob(NewFileName, "Site-images", fpath, "image/jpeg");
        //        }
        //    }
        //    else
        //    {
        //        databaseCon.deletefileonblob(FileName1, "siteimage");

        //        using (var imageFile = new FileStream(newPath1, FileMode.Create))
        //        {
        //            imageFile.Write(bytes1, 0, bytes1.Length);
        //            imageFile.Flush();
        //        }
        //        if (System.IO.File.Exists(newPath1))
        //        {
        //            string fpath = path + FileName1;
        //            databaseCon.savefileonblob(FileName1, "Site-images", fpath, "image/jpeg");
        //            System.IO.File.Delete(fpath);
        //        }
        //    }

        //    string querystr = "update tblinspection set Inspimage2 ='" + NewFileName + "',SafariPhotoAngle1='" + PhotoAngle1 + "'  where InspectionID ='" + databaseCon.Decrypt(InspID) + "'";

        //    int count = databaseCon.RunCmd(querystr);
        //}
        // update method on  05 march 2025  for the delete temp image from local after upload image on server 

    }
}


