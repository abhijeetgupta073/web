using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure.Storage.Blob;
using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;

namespace Scaffold.Controllers
{
    public class DayDocketFormController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: DayDocketForm
        public ActionResult DayDocketForm()
        {
            string ClientQuery = "select ClientImage from tblClient where ClientId = '" + databaseCon.Decrypt(Request.QueryString["id"]) + "'";
            DataTable clientdt = databaseCon.GetData(ClientQuery);

            if (clientdt.Rows.Count > 0)
            {
                if (clientdt.Rows[0]["ClientImage"].ToString() != "")
                {
                    //  var uri = new Uri(databaseCon.clientimageLink);
                    string imgname = clientdt.Rows[0]["ClientImage"].ToString();
                    //   var container = new CloudBlobContainer(uri);
                    //   var blob = container.GetBlockBlobReference(clientdt.Rows[0]["ClientImage"].ToString());
                    //string a = databaseCon.siteimagelink + "Item1.png";
                    // imga.Attributes.Add("src", databaseCon.siteimagelink + "Item1.png");
                    //   var exists = blob.Exists();
                    if (clientdt.Rows[0]["ClientImage"].ToString() == "")
                    {
                       // imgLogo.Attributes.Add("src", "/img/logo1.png");
                        //ViewBag.ClientImage = "/img/logo1.png";
                        ViewBag.ClientImage = databaseCon.logoimage;

                    }
                    else
                    {
                        // string a = databaseCon.clientimageLink + clientdt.Rows[0]["ClientImage"].ToString();
                       // imgLogo.Attributes.Add("src", "/images/clientlogo/" + clientdt.Rows[0]["ClientImage"].ToString());
                        ViewBag.ClientImage = "/images/clientlogo/" + clientdt.Rows[0]["ClientImage"].ToString();
                    }
                }
                else
                {
                    // imgLogo.Attributes.Add("src", "/img/logo1.png");
                    //ViewBag.ClientImage = "/img/logo1.png";
                    ViewBag.ClientImage = databaseCon.logoimage;
                }
            }

            return View();
        }

        public ActionResult TableData(string SiteId, string EntryDate)
        {
            int sumtotalHrs = 0;
            //string query = "select EntryDate as DateOfWorks ,TypeofVariation as DescOfWork,COUNT(TypeofVariation) as NoOfMen , SUM(AdditionalHrs) as NoOfHrs ,COUNT(TypeofVariation)*SUM(AdditionalHrs) as TotalHrs from tblManHrs where DayDocketId='" + Request.QueryString["DayDock"] + "' Group by EntryDate ,TypeofVariation";
            // string query = "select EntryDate as DateOfWorks ,TypeofVariation as DescOfWork,COUNT(TypeofVariation) as NoOfMen , SUM(AdditionalHrs) as NoOfHrs ,COUNT(TypeofVariation)*SUM(AdditionalHrs) as TotalHrs from tblManHrs where sitelocId='" + Request.QueryString["SiteId"] + "' and entrydate='" + Request.QueryString["EntryDate"] + "'  Group by EntryDate ,TypeofVariation";

            string query = "select RIght('000000'+Convert(varchar,ScaffRef),6) as Scaffref,Description,firstName+' '+LastName as Resource,TypeofVariation,VariationDesc,isnull(AdditionalHrs,0) as AdditionalHrs, isnull(Category,'') as Category from tblManHrs join vw_tblRegister on tblManHrs.RegisterId=vw_tblRegister.RegisterId join tblUser On tblManHrs.UserId=tblUser.UserId left join tblUserCategory on tblUser.CategoryId=tblUserCategory.UserCatId  where  IncludedInDayDocket=1 and  tblManHrs.sitelocId='" + SiteId + "' and entrydate='" + EntryDate + "'";

            DataTable dt = databaseCon.GetData(query);
            //StringBuilder html = new StringBuilder();
            string html = "";

            //string HeaderId = dt.Rows[k]["contentId"].ToString();

            //string HeaderName = dt.Rows[k]["CheckListContent"].ToString();

            //html.Append("<div class='row' style='margin-top: 20px;'><div class='col-lg-3 col-md-4 col-sm-4 col-xs-6'> <span class='InspectionListTitle'>" +  + "</span> </div></div>");
            //if (dt.Rows.Count > 0)
            //{

            //html.Append("<table style='width:100%;' border = '1'>");
            //html.Append("<thead>");
            //html.Append("<th>Scaf Ref #</th>");
            //html.Append("<th>Reason for Access</th>");
            //html.Append("<th>Resource</th>");
            //html.Append("<th>Category</th>");
            //html.Append("<th>Type of Variation</th>");
            //html.Append("<th>Variation Desc</th>");
            //html.Append("<th>Additional Hrs</th>");
            //html.Append("</thead>");
            //html.Append("</tbody>");

            html = "<table style='width:100%;' border = '1'>";
            html += "<thead>";
            html += "<th>Scaf Ref #</th>";
            html += "<th>Reason for Access</th>";
            html += "<th>Resource</th>";
            html += "<th>Category</th>";
            html += "<th>Type of Variation</th>";
            html += "<th>Variation Desc</th>";
            html += "<th>Additional Hrs</th>";
            html += "</thead>";
            html += "</tbody>";
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                string Scaffref = String.Format("{0:dd-MMM-yyyy}", dt.Rows[i]["Scaffref"]);
                String Description = Convert.ToString(dt.Rows[i]["Description"].ToString());
                string Resource = dt.Rows[i]["Resource"].ToString();
                string Category = dt.Rows[i]["Category"].ToString();
                string TypeofVariation = dt.Rows[i]["TypeofVariation"].ToString();
                string VariationDesc = dt.Rows[i]["VariationDesc"].ToString();
                string AdditionalHrs = dt.Rows[i]["AdditionalHrs"].ToString();
                //html.Append("<tr><td>" + Scaffref + "</td>");
                //html.Append("<td>" + Description + "</td>");
                //html.Append("<td>" + Resource + "</td>");
                //html.Append("<td>" + Category + "</td>");
                //html.Append("<td>" + TypeofVariation + "</td>");
                //html.Append("<td>" + VariationDesc + "</td>");
                //html.Append("<td>" + AdditionalHrs + "</td></tr>");

                html += "<tr><td>" + Scaffref + "</td>";
                html += "<td>" + Description + "</td>";
                html += "<td>" + Resource + "</td>";
                html += "<td>" + Category + "</td>";
                html += "<td>" + TypeofVariation + "</td>";
                html += "<td>" + VariationDesc + "</td>";
                html += "<td>" + AdditionalHrs + "</td></tr>";

                sumtotalHrs = sumtotalHrs + Convert.ToInt32(dt.Rows[i]["AdditionalHrs"]);

            }
            //html.Append("<tr><td colspan='6' style='text-align:right'><b>TOTAL HOURS WORKED<b/></td>");
            //html.Append("<td>" + String.Format("{0:0.00}", sumtotalHrs) + "</td>");
            //html.Append("</td></tr>");
            //html.Append("</tbody></table>");

            html += "<tr><td colspan='6' style='text-align:right'><b>TOTAL HOURS WORKED<b/></td>";
            html += "<td>" + String.Format("{0:0.00}", sumtotalHrs) + "</td>";
            html += "</td></tr>";
            html += "</tbody></table>";

            //PlaceHolder1.Controls.Add(new Literal { Text = html.ToString() });
            return Json(new { html = html }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult showData(string SiteId, string EntryDate, string DayDock, string id)
        {
            //DataTable dt;
            // lockimg.Style.Add("display", "none");

            // btnSave.Style.Add("display", "block");
            //backtoDayDocket.Style.Add("display", "block");
            DayDocketViewModel deliveryDocketViewModel = new DayDocketViewModel();
            string query11 = "select SupervisorSign , ClientSign from tblDayDocket where DayDocketID='" + DayDock + "'";
            DataTable dt1 = databaseCon.GetData(query11);
            if (dt1.Rows.Count > 0)
            {
                if (dt1.Rows[0]["SupervisorSign"].ToString() != "")
                {
                    var uri = new Uri(databaseCon.siteimagelink);
                    var container = new CloudBlobContainer(uri);
                    var blob = container.GetBlockBlobReference(dt1.Rows[0]["SupervisorSign"].ToString());
                    var exists = blob.Exists();
                    if (exists == true)
                    {
                        
                        //imga.Attributes.Add("src", databaseCon.siteimagelink + InspectDt.Rows[0]["Inspimage"].ToString());
                        //signimga.Attributes.Add("src", databaseCon.siteimagelink + dt1.Rows[0]["SupervisorSign"].ToString());
                        string signUrl = databaseCon.siteimagelink + dt1.Rows[0]["SupervisorSign"].ToString();
                        deliveryDocketViewModel.SupervisorSign = signUrl;
                        // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp5", "<script>RemoveSignatureBlock();</script>", false);

                        //printcanvas.Style.Add("display", "none");
                        //hdnSignImageBlock2.Style.Add("display", "block");
                    }
                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp12", "<script>HideSignatureBlock();</script>", false);
                }
                //HandOver Signature 1 Code Ends Here

                //HandOver Signature 2 Code Starts Here
                if (dt1.Rows[0]["ClientSign"].ToString() != "")
                {
                    var uri = new Uri(databaseCon.siteimagelink);
                    var container = new CloudBlobContainer(uri);
                    var blob = container.GetBlockBlobReference(dt1.Rows[0]["ClientSign"].ToString());
                    var exists = blob.Exists();
                    if (exists == true)
                    {
                       
                        //imga.Attributes.Add("src", databaseCon.siteimagelink + InspectDt.Rows[0]["Inspimage"].ToString());
                       // signimg2.Attributes.Add("src", databaseCon.siteimagelink + dt1.Rows[0]["ClientSign"].ToString());
                        string signUrl1 = databaseCon.siteimagelink + dt1.Rows[0]["ClientSign"].ToString();
                        deliveryDocketViewModel.ClientSign = signUrl1;
                        //Canvas1.Style.Add("display", "none");
                        // hdnSignImageBlock1.Style.Add("display", "block");
                    }
                    ///UpdATE TBLmaNhRS after client Confirmation
                    string queryupdate = " update tblManHrs set DayDocketId='" + DayDock + "' where EntryDate =Convert(date,'" + EntryDate + "') and SiteLocId = '" + SiteId + "'";
                    int UpdateID = databaseCon.RunCmd(queryupdate);


                }
            }
            //HandOver Signature 2 Code Ends Here


            string query = "select Name,Address , City, State, CellPhone , ZIP from tblClient where ClientId='" + databaseCon.Decrypt(id) + "'";
            DataTable dt = databaseCon.GetData(query);
            String query1 = "select Right('000000'+Convert(varchar,DayDocketId),6) as SiteRef , Convert(date,EntryDate) as EntryDate , tblDayDocket.Comments,tblDayDocket.SiteLocId ,SiteLocation ,CompanyRep,ClientRep,Case when Convert(varchar,Convert(date,ClientSignDate))='1900-01-01'then '' else ClientSignDate End as ClientSignDate, SupervisorSign,SupervisorSignDate,ClientSign,ClientSignDate  from tblDayDocket join tblSiteLocation on tblDayDocket.SiteLocId=tblSiteLocation.SiteLocId where DayDocketId ='" + DayDock + "'";
            DataTable dt2 = databaseCon.GetData(query1);
            if (dt2.Rows.Count > 0)
            {
                //lblDate1.Text = String.Format("{0:dd-MMM-yyyy}", dt2.Rows[0]["EntryDate"]);

                //lblSiteRef.Text = Convert.ToString(dt2.Rows[0]["SiteRef"].ToString());
                //lblSiteLocation.Text = Convert.ToString(dt2.Rows[0]["SiteLocation"].ToString());
                //lblAddress.Text = Convert.ToString(dt.Rows[0]["Address"].ToString());
                deliveryDocketViewModel.EntryDate = String.Format("{0:dd-MMM-yyyy}", dt2.Rows[0]["EntryDate"]);

                deliveryDocketViewModel.SiteRef = Convert.ToString(dt2.Rows[0]["SiteRef"].ToString());
                deliveryDocketViewModel.SiteLocation = Convert.ToString(dt2.Rows[0]["SiteLocation"].ToString());
                deliveryDocketViewModel.Address = Convert.ToString(dt.Rows[0]["Address"].ToString());
                if (dt.Rows[0]["City"].ToString() != "")
                {
                    deliveryDocketViewModel.City = Convert.ToString(dt.Rows[0]["City"].ToString());
                    //lblCity.Text= Convert.ToString(dt.Rows[0]["City"].ToString());
                    //lblComma.Visible = true;
                }
                //else
                //{
                //    lblComma.Visible = false;
                //}
                //txtcomment.Text = Convert.ToString(dt2.Rows[0]["Comments"].ToString());
                //lblState.Text = Convert.ToString(dt.Rows[0]["State"].ToString());
                deliveryDocketViewModel.Comments = Convert.ToString(dt2.Rows[0]["Comments"].ToString());
                deliveryDocketViewModel.State = Convert.ToString(dt.Rows[0]["State"].ToString());
                if (dt.Rows[0]["CellPhone"].ToString() != "")
                {
                    deliveryDocketViewModel.CellPhone = String.Format("{0:(##) ####-####}", dt.Rows[0]["CellPhone"]);
                    //lblCellPhone.Text = String.Format("{0:(##) ####-####}", dt.Rows[0]["CellPhone"]);
                    //lblPh.Visible = true;

                }
                //else
                //{
                //    lblPh.Visible = false;
                //}

                //lblzip.Text = Convert.ToString(dt.Rows[0]["Zip"].ToString());
                //lblClient.Text = Convert.ToString(dt.Rows[0]["Name"].ToString());
                //txtsupervisor.Text = Convert.ToString(dt2.Rows[0]["CompanyRep"].ToString());
                //txtclient.Text = Convert.ToString(dt2.Rows[0]["ClientRep"].ToString());

                deliveryDocketViewModel.ZIP = Convert.ToString(dt.Rows[0]["Zip"].ToString());
                deliveryDocketViewModel.Name = Convert.ToString(dt.Rows[0]["Name"].ToString());
                deliveryDocketViewModel.CompanyRep = Convert.ToString(dt2.Rows[0]["CompanyRep"].ToString());
                deliveryDocketViewModel.ClientRep= Convert.ToString(dt2.Rows[0]["ClientRep"].ToString());
                if (dt2.Rows[0]["ClientSignDate"] != DBNull.Value && Convert.ToString(dt2.Rows[0]["ClientSignDate"]) != "01/01/1900 12:00:00 AM")
                {
                    //datepicketID.SelectedDate = Convert.ToDateTime(dt2.Rows[0]["ClientSignDate"]);
                    deliveryDocketViewModel.ClientSignDate= Convert.ToDateTime(dt2.Rows[0]["ClientSignDate"]);
                }

                //HandOver Signature 1 Code Starts Here

            }
            return Json(new { GetAllData = deliveryDocketViewModel }, JsonRequestBehavior.AllowGet);
        }

        public int btnSave_Click(string DayDock, string sketchData, string sketchData1, string txtsupervisor, string txtclient, string ClientSignDate, string txtcomment)
        {
            //string sketchData = sketch_data.Value;
            //string sketchData1 = sketch_data1.Value;
            //  string query = "update tblDayDocket set DateOfWorks='"+ +"'";
            if (sketchData != "" && sketchData != null)
            {
                SignatureUpload1(sketchData, DayDock);
            }

            if (sketchData1 != "" && sketchData1 != null)
            {
                SignatureUpload2(sketchData1, DayDock);

            }



            string query = "update tblDayDocket set CompanyRep='" + txtsupervisor.Replace("'", "''").Trim() + "' ,ClientRep='" + txtclient.Replace("'", "''").Trim() + "',ClientSignDate ='" + ClientSignDate + "' ,Comments='" + txtcomment.Replace("'", "''") + "'  where DayDocketID ='" + DayDock + "'";
            int count = databaseCon.RunCmd(query);
            // TableData();
            //showData();

            //if (sketchData1.ToString() != "")
            //{
            //    Response.Redirect("DayDocket?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"]);
            //}
            //else
            //{
            //    art1.Style.Add("display", "block");
            //    art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
            //    //art1.Attributes["class"] = "alert alert-success";
            //    alertlabel.Text = "<strong> Success ! </strong>Data Saved Successfully";
            //}
            //Response.Redirect("DayDocketForm?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&DayDock=" + Request.QueryString["DayDock"] + "&SiteID=" + Request.QueryString["SiteId"] + "&EntryDate=" + Request.QueryString["EntryDate"]);
            return 1;
        }

        protected void SignatureUpload1(string sketchData1, string DayDock)
        {
            string path = "~/uploads/";
            path = Server.MapPath(path);
            string FileName = "Sketch1SupervisorSign" + "_" + DateTime.Now.Ticks + "_" + DayDock + ".png";
            string SketchnewPath = path + FileName;


            byte[] bytes = Convert.FromBase64String(sketchData1.Split(',')[1]);

            var uri = new Uri(databaseCon.siteimagelink);
            var container = new CloudBlobContainer(uri);
            var blob = container.GetBlockBlobReference(FileName);
            var exists = blob.Exists();
            if (exists == false)
            {
                using (var imageFile = new FileStream(SketchnewPath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }

                if (System.IO.File.Exists(SketchnewPath))
                {
                    string fpath = path + FileName;
                    databaseCon.savefileonblob(FileName, "Site-images", fpath, "image/png");
                    System.IO.File.Delete(fpath);
                }
            }
            else
            {
                databaseCon.deletefileonblob(FileName, "siteimage");

                using (var imageFile = new FileStream(SketchnewPath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }

                if (System.IO.File.Exists(SketchnewPath))
                {
                    string fpath = path + FileName;
                    databaseCon.savefileonblob(FileName, "Site-images", fpath, "image/png");
                    System.IO.File.Delete(fpath);
                }
            }


            string querystr = "update tblDayDocket set SupervisorSign='" + FileName + "' where DayDocketID ='" + DayDock + "'";
            int count = databaseCon.RunCmd(querystr);
        }


        protected void SignatureUpload2(string sketchData2, string DayDock)
        {
            string path = "~/uploads/";
            path = Server.MapPath(path);
            string FileName = "Sketch2ClientSign" + DateTime.Now.Ticks + DayDock + ".png";
            string SketchnewPath = path + FileName;


            byte[] bytes = Convert.FromBase64String(sketchData2.Split(',')[1]);

            var uri = new Uri(databaseCon.siteimagelink);
            var container = new CloudBlobContainer(uri);
            var blob = container.GetBlockBlobReference(FileName);
            var exists = blob.Exists();
            if (exists == false)
            {
                using (var imageFile = new FileStream(SketchnewPath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }

                if (System.IO.File.Exists(SketchnewPath))
                {
                    string fpath = path + FileName;
                    databaseCon.savefileonblob(FileName, "Site-images", fpath, "image/png");
                    System.IO.File.Delete(fpath);
                }
            }
            else
            {
                databaseCon.deletefileonblob(FileName, "siteimage");

                using (var imageFile = new FileStream(SketchnewPath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }

                if (System.IO.File.Exists(SketchnewPath))
                {
                    string fpath = path + FileName;
                    databaseCon.savefileonblob(FileName, "Site-images", fpath, "image/png");
                    System.IO.File.Delete(fpath);
                }
            }


            string querystr = "update tblDayDocket set ClientSign='" + FileName + "' where DayDocketID ='" + DayDock + "'";
            int count = databaseCon.RunCmd(querystr);

        }
    }

}