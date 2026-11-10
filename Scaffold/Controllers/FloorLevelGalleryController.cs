using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;

namespace Scaffold.Controllers
{
    public class FloorLevelGalleryController : Controller
    {
        ScaffoldEntities dbcontext = new ScaffoldEntities();
        // GET: FloorLevelGallery
        public ActionResult FloorLevelGallery()
        {
            return View();
        }

        public ActionResult SiteDropDownBind(string id)
        {
            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select SiteLocID,SiteLocation from tblSiteLocation where ClientId='" + databaseCon.Decrypt(id) + "' and FloorLevel=1");
            return Json(GetData, JsonRequestBehavior.AllowGet);

        }

        public ActionResult dropdownlocation(string id, string ddlSites)
        {
            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select area,areaid from tblArea where ClientID='" + databaseCon.Decrypt(id) + "'and SiteLocId = '" + ddlSites + "' order by area");
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult dropdownbuilding(string id, string ddlLocaSite)
        {
            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select buildingid,buildingno  from tblbuilding where BuildingStatus='Active' and ClientID='" + databaseCon.Decrypt(id) + "' and Areaid='" + ddlLocaSite + "' order by buildingno");
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult dropdownDrawing(string id)
        {
            string query;
            //query = "select distinct DrawingName  from tblSiteImages where ClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and SiteId='" + ddlSites.SelectedValue + "' and (LocID='" + ddlLocaSite.SelectedValue + "' or LocID is Null) and (BuildingID='" + ddlBuilding.SelectedValue + "' or BuildingID is Null) ";
            query = "select distinct DrawingName  from tblSiteImages where ClientId='" + databaseCon.Decrypt(id) + "' and DrawingName is not Null and FloorlevelId != '0'";
            DataTable dtSelectFile = databaseCon.GetData(query);
            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>(query);
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DropdownFloorLevelBind(string id,string ddlSites, string ddlLocaSite, string ddlBuilding,string siteName) {
            string query = "";
           if(ddlLocaSite!="") {
              if(ddlBuilding!="" || ddlBuilding != null) {
                    query = "select FloorLevelId,FloorLevel from tblFloorlevel where ClientId ='" + databaseCon.Decrypt(id) + "' and FloorLevelStatus='Active' and sitelocid = '" + ddlSites + "' and LocationID = '" + ddlLocaSite + "' and BuildingID= '" + ddlBuilding + "' and Category = 'Floor Level' and FloorLevelId not in (select FloorLevelId from tblSiteImages where SiteId='" + ddlSites + "' and LocID='" + ddlLocaSite + "' and BuildingID= '" + ddlBuilding + "' ) order by SortOrder ASC";
                }
                else {
                    query = "select 0 as FloorLevelId,'" + siteName + " Landing Image' as FloorLevel ";
                }
           }else{
               query = "select 0 as FloorLevelId,'" + siteName + " Landing Image' as FloorLevel ";
            }
            //var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select FloorLevelId,FloorLevel from tblFloorlevel where ClientId ='" + databaseCon.Decrypt(id) + "' and FloorLevelStatus='Active' and sitelocid = '" + ddlSites + "' and LocationID = '" + ddlLocaSite + "' and BuildingID= '" + ddlBuilding + "' and Category = 'Floor Level' and FloorLevelId not in (select FloorLevelId from tblSiteImages where SiteId='" + ddlSites + "' and LocID='" + ddlLocaSite + "' and BuildingID= '" + ddlBuilding + "' ) order by SortOrder ASC");
            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>(query);
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult ddlSites_SelectedIndexChanged(string id,string ddlSites)
        {
            var queryFloor = "";
            if (ddlSites == "0")
            {
                //  queryFloor = "select 0 as FloorLevelId,'Floors Landing Image' as FloorLevel where FloorLevel not in (select FloorLevel from tblSiteImages where ClientId ='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and SiteId='" + ddlSites.SelectedValue + "')";
                // queryFloor = "select 0 as FloorLevelId,'Floors Landing Image' as FloorLevel";

            }
            else
            {
                // Floor DropDown List Bind
                //Changed Query on 27-May-2020
                //queryFloor = "select FloorLevelId,FloorLevel from tblFloorlevel where ClientId ='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and FloorLevelStatus='Active' and sitelocid = '" + ddlSites.SelectedValue + "' and (LocationID='" + ddlLocaSite.SelectedValue + "' or LocationID is Null) and Category = 'Floor Level' and FloorLevelId not in (select FloorLevelId from tblSiteImages where SiteId='" + ddlSites.SelectedValue + "' and (LocID='" + ddlLocaSite.SelectedValue + "' or LocID is Null)) order by SortOrder ASC";

                //queryFloor = "select FloorLevelId,FloorLevel,SortOrder from tblFloorlevel where ClientId ='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and FloorLevelStatus='Active' and sitelocid = '" + ddlSites.SelectedValue + "' and (LocationID='" + ddlLocaSite.SelectedValue + "' or LocationID is Null) and Category = 'Floor Level' and FloorLevelId not in (select FloorLevelId from tblSiteImages where SiteId='" + ddlSites.SelectedValue + "' and (LocID='" + ddlLocaSite.SelectedValue + "' or LocID is Null))  union all select 0 as FloorLevelId,'" + siteName + " Landing Image' as FloorLevel, 0 as SortOrder  order by SortOrder ASC";
                var siteName = ddlSites;
                queryFloor = "select 0 as FloorLevelId,'" + siteName + " Landing Image' as FloorLevel ";
            }

            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>(queryFloor);
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }
     

        public ActionResult showthumnail(string id, string ddlSites, string ddlLocaSite, string ddlBuilding)
        {
            //string query = "select ImageName,SiteImageId,FloorLevel,SiteId,ClientId,Description from tblSiteImages where ClientId = '" + databaseCon.Decrypt3(Request.QueryString["id"]) + "' and SiteId = '" + ddlSites.SelectedValue + "' and (LocID ='" + ddlLocaSite.SelectedValue + "' or LocID IS Null) and (BuildingID ='" + ddlBuilding.SelectedValue + "' or BuildingID IS Null)";
            string query = "";
            //var siteName = ddlSites.SelectedItem.Text;
            //if (ddlSites.SelectedValue != "" && ddlLocaSite.SelectedValue == "0" && ddlBuilding.SelectedValue == "0")
            
            if ((ddlSites == "" || ddlSites!="") && (ddlLocaSite == "0" || ddlLocaSite == "") && (ddlBuilding == "0" || ddlBuilding == ""))
            {
            if(ddlSites=="") {
                    ddlSites = "0";
            }
               
                //query = "select ImageName,SiteImageId,isnull(FloorLevelId,0) as FloorLevelId,'" + siteName + " Landing Image' as FloorLevel,SiteId,ClientId,Description from TblSiteImages where ClientId= '" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and (SiteId = '" + ddlSites.SelectedValue + "' and SiteId != '0') and isnull(LocId,0)='0' and isnull(BuildingId,0)='0' and isnull(FloorLevelId,0) = '0' ";
                query = "select ImageName,SiteImageId,isnull(FloorLevelId,0) as FloorLevelId,SiteLocation+' Landing Image' as FloorLevel,TblSiteImages.SiteId,TblSiteImages.ClientId,Description from TblSiteImages Left join TblSiteLocation on TblSiteImages.SiteId = TblSiteLocation.SiteLocId where TblSiteImages.ClientId = '" + databaseCon.Decrypt(id) + "' and((SiteId = '" + ddlSites + "'  and SiteId != '0') or(SiteId != '0' and '" + ddlSites + "'  = '0')) and isnull(LocId,0)= '0' and isnull(BuildingId,0)= '0' and isnull(FloorLevelId,0) = 0 ";

            }
            else
            {
                query = "select ImageName,SiteImageId,tblSiteImages.FloorLevelId,tblfloorLevel.FloorLevel,tblSiteImages.SiteId,tblSiteImages.ClientId,Description from tblSiteImages join tblfloorLevel on tblSiteImages.FloorLevelId = tblFloorLevel.FloorLevelId where tblSiteImages.ClientId = '" + databaseCon.Decrypt(id) + "' and tblSiteImages.SiteId = '" + ddlSites + "' and(LocID = '" + ddlLocaSite + "' or LocID IS Null) and(tblSiteImages.BuildingID = '" + ddlBuilding + "' or tblSiteImages.BuildingID IS Null)";
            }

            DataTable dt = databaseCon.GetData(query);
            // StringBuilder html = new StringBuilder();
            // html.Append("<table id='image' style='width:100%'>");
            string html = "<table id='image' style='width:100%'>";
            if (dt.Rows.Count > 0)
            {
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    string imgchk = dt.Rows[j]["ImageName"].ToString();
                    if (imgchk != "" && imgchk != "NULL" && imgchk != null)
                    {
                        //var uri = new Uri(databaseCon.siteimagelink);
                        //var container = new CloudBlobContainer(uri);
                        //var blob = container.GetBlockBlobReference(dt.Rows[j]["ImageName"].ToString());
                        //string file = databaseCon.siteimagelink + dt.Rows[j]["ImageName"].ToString();

                        string path = "/images/SiteFloorlevelimg/";
                        //  path = Server.MapPath(path);
                        string fpath = path + imgchk;
                        var SiteImageID = dt.Rows[j]["SiteImageId"];

                        //html.Append("<div class='col-lg-4 col-md-6 col-sm-12 col-xs-12' style='margin-top: 5px;'>");
                        //html.Append("<img id='ppdl" + SiteImageID + "' runnat='server' class='img-container' src='" + fpath + "' style='width: 300px;object-fit:contain; height: 250px' />");
                        //html.Append("<div class='row' style='margin-top: -18px;text-align:left'>");
                        //html.Append("<div class='col-lg-12 col-md-12 col-sm-6 col-xs-12'>");
                        //html.Append("<a id='descrs" + j + "' style='color:black;font-size:16px;font-weight:600;margin-left:32px;margin-right: 36px';>" + dt.Rows[j]["FloorLevel"] + "</a>");
                        //html.Append("<a id='description" + j + "' style='color:black;font-size:16px;font-weight:600;margin-left:32px;margin-right: 36px;display:none';>" + dt.Rows[j]["Description"] + "</a>");
                        //html.Append("<a id='siteflrimgid" + j + "' style='color:black;display:none;font-size:16px;font-weight:600;margin-left:32px'>" + dt.Rows[j]["SiteImageId"] + "</a>");
                        ////html.Append("<a id='siteflrimgid1" + j + "' style='color:black;font-size:16px; display:none; font-weight:600;margin-left:32px'>" + dt.Rows[j]["SiteImageId"] + "</a>");
                        ////html.Append("<button class='w3-btn nln-bg w3-hover-dark-grey' id = 'btndelete1' onclick='displayRadioValue()'" + dt.Rows[j]["FloorLevel"] + " >Delete</button>");
                        //html.Append("</div>");
                        //html.Append("</div>");
                        //html.Append("</div>");

                        html+=("<div class='col-lg-4 col-md-6 col-sm-12 col-xs-12' style='margin-top: 5px;'>");
                        html+=("<img id='ppdl" + SiteImageID + "' runnat='server' class='img-container' src='" + fpath + "' style='width: 300px;object-fit:contain; height: 250px' />");
                        html+=("<div class='row' style='margin-top: -18px;text-align:left'>");
                        html+=("<div class='col-lg-12 col-md-12 col-sm-6 col-xs-12'>");
                        html+=("<a id='descrs" + j + "' style='color:black;font-size:16px;font-weight:600;margin-left:32px;margin-right: 36px';>" + dt.Rows[j]["FloorLevel"] + "</a>");
                        html+=("<a id='description" + j + "' style='color:black;font-size:16px;font-weight:600;margin-left:32px;margin-right: 36px;display:none';>" + dt.Rows[j]["Description"] + "</a>");
                        html+=("<a id='siteflrimgid" + j + "' style='color:black;display:none;font-size:16px;font-weight:600;margin-left:32px'>" + dt.Rows[j]["SiteImageId"] + "</a>");
                        //html.Append("<a id='siteflrimgid1" + j + "' style='color:black;font-size:16px; display:none; font-weight:600;margin-left:32px'>" + dt.Rows[j]["SiteImageId"] + "</a>");
                        //html.Append("<button class='w3-btn nln-bg w3-hover-dark-grey' id = 'btndelete1' onclick='displayRadioValue()'" + dt.Rows[j]["FloorLevel"] + " >Delete</button>");
                        html+=("</div>");
                        html+=("</div>");
                        html+=("</div>");
                    }

                }
                html += "</table>";
            }
            else
            {
            }
            //placeholderforimage.Controls.Add(new Literal { Text = html.ToString() });
            return Json(html, JsonRequestBehavior.AllowGet);
        }


        public ActionResult btnnSave_Click(string id, string RID, string UID, string ddlSites, string ddlLocaSite, string ddlFloorLabel, string ddlBuilding, string txtdescs, string txtdrawing, string hdndrawing, string FileUploads, string FloorLevelLandingName, string ddlFileSelect)
        {
            string Result = "";
            string Message = "";
            int i = 0;
            //string FileExtension = "";
            var ExistImageFile = "";

            string FileExtension = System.IO.Path.GetExtension(FileUploads).ToLower();
            if (ddlFileSelect != "" && ddlFileSelect != null)
            {
                string query1 = "select ImageName from tblSiteImages where ClientId = '" + databaseCon.Decrypt(id) + "' and DrawingName ='" + ddlFileSelect + "'";

                DataTable dt1 = databaseCon.GetData(query1);
                if(dt1.Rows.Count>0) {
                    ExistImageFile = dt1.Rows[0]["ImageName"].ToString();

                    var tt= dt1.Rows[0]["ImageName"].ToString();
                }

               

                //string getflrlevel = "select Count(*) As FloorCount from tblSiteImages where ClientId = '" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and ( SiteId = '" + ddlSites.SelectedValue + "' or SiteId is Null) and ( LocID = '" + ddlLocaSite.SelectedValue + "' or LocID is Null)  and ( BuildingID = '" + ddlBuilding.SelectedValue + "' or BuildingID is Null) and FloorLevel = '" + ddlFloorLabel.SelectedValue + "'";
                string getflrlevel = "select Count(*) As FloorCount from tblSiteImages where ClientId = '" + databaseCon.Decrypt(id) + "' and ( SiteId = '" + ddlSites + "' or SiteId is Null) and ( LocID = '" + ddlLocaSite + "' or LocID is Null)  and ( BuildingID = '" + ddlBuilding + "' or BuildingID is Null) and FloorLevelId = '" + ddlFloorLabel + "'";
                DataTable dt = databaseCon.GetData(getflrlevel);

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["FloorCount"].ToString() == "0")
                    {

                        //string query = "insert into tblSiteImages (ClientId,SiteId,FloorLevel,ImageName,Description,CreatedBy,CreatedOn,LocID,BuildingID,DrawingName) values ('" + databaseCon.Decrypt(Request.QueryString["id"]) + "','" + ddlSites.SelectedValue + "','" + ddlFloorLabel.SelectedValue + "','" + ExistImageFile + "','" + txtdescs.Text + "',(select EmailId from tblUser where UserId='" + databaseCon.Decrypt(Request.QueryString["UID"]) + "'),'" + DateTime.Now + "','" + ddlLocaSite.SelectedValue + "','" + ddlBuilding.SelectedValue + "','" + txtdrawing.Text + "')";
                        string query = "insert into tblSiteImages (ClientId,SiteId,FloorLevelId,ImageName,Description,CreatedBy,CreatedOn,LocID,BuildingID,DrawingName) values ('" + databaseCon.Decrypt(id) + "','" + ddlSites + "','" + ddlFloorLabel + "','" + ExistImageFile + "','" + txtdescs + "',(select EmailId from tblUser where UserId='" + databaseCon.Decrypt(UID) + "'),'" + DateTime.Now + "','" + ddlLocaSite + "','" + ddlBuilding + "','" + txtdrawing + "')";

                        i = databaseCon.RunCmd(query);


                        if (i != 0)
                        {
                            //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Success";
                            //art1.Style.Add("display", "block");
                            //alertlabel.Text = "<strong>Success! </strong>Image Saved Successfully";
                            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp1", "<script>hideAlert();</script>", false);
                            //showthumnail();
                            Result = "Success";
                            Message = "Image Saved Successfully";
                        }
                    }
                    else
                    {
                        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Alert! </strong>Floor Level Image already exists .";
                        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp", "<script>hideAlert();</script>", false);
                        //showthumnail();//Add
                        Result = "Failed";
                        Message = "Floor Level Image already exists.";
                    }
                    // txtdrawing.Text = "";
                }
            }

            else
            {
                // FileExtension = FileUploads.FileName;
                FileExtension = FileUploads;
                //string FileExtension = FileUploads.FileName;        
                //if (FileUploads.HasFiles == true)
                if (FileUploads != "")
                {
                    if (FileExtension != "")
                    {
                        //var ext = FileExtension.Split('.'); 
                        var ext1 = Path.GetExtension(FileExtension);
                        var ext = ext1.Split('.');
                        var extension = ext[1];

                        if (extension.ToLower() == "jpg" || extension.ToLower() == "jpeg" || extension.ToLower() == "png" || extension.ToLower() == "ico" || extension.ToLower() == "gif" || extension.ToLower() == "bmp")
                        {
                            HttpFileCollectionBase files = Request.Files;
                            //foreach (HttpPostedFile postedFile in FileUploads.PostedFiles)
                            //{
                            for (int k = 0; k < 1; k++)
                            {
                                HttpPostedFileBase postedFile = files[k];

                                 if (ddlBuilding != "" && ddlSites != "" && ddlLocaSite != "")
                               // if (ddlBuilding != "" || ddlSites != "" || ddlLocaSite != "")
                                {
                                    string fileName = ddlSites.Replace("'", "''").Replace(" ", "").Trim() + "_" + ddlLocaSite.Replace("'", "''").Replace(" ", "").Trim() + "_" + ddlBuilding.Replace("'", "''").Replace(" ", "").Trim() + "_" + databaseCon.Decrypt(RID) + "_" + DateTime.Now.Ticks + "." + extension;
                                    postedFile.SaveAs(Server.MapPath("~/images/SiteFloorlevelimg") + @"/" + fileName);
                                    string path = "/images/SiteFloorlevelimg/";
                                    path = Server.MapPath(path);

                                    string fpath = path + fileName;

                                    string querydrawingchck = "select Count(*) As DrawingCount  from tblSiteImages where ClientId='" + databaseCon.Decrypt(id) + "' and SiteId='" + ddlSites + "' and (LocID='" + ddlLocaSite + "' or LocID is Null) and (BuildingID='" + ddlBuilding + "' or BuildingID is Null) and DrawingName= '" + hdndrawing + "'";
                                    DataTable dt1 = databaseCon.GetData(querydrawingchck);
                                    if (dt1.Rows.Count > 0)
                                    {
                                        if (dt1.Rows[0]["DrawingCount"].ToString() == "0")
                                        {
                                            //string getflrlevel = "select Count(*) As FloorCount from tblSiteImages where ClientId = '" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and SiteId = '" + ddlSites.SelectedValue + "' and LocID = '" + ddlLocaSite.SelectedValue + "' and BuildingID = '" + ddlBuilding.SelectedValue + "' and FloorLevel = '" + ddlFloorLabel.SelectedValue + "'";
                                            string getflrlevel = "select Count(*) As FloorCount from tblSiteImages where ClientId = '" + databaseCon.Decrypt(id) + "' and SiteId = '" + ddlSites + "' and LocID = '" + ddlLocaSite + "' and BuildingID = '" + ddlBuilding + "' and FloorLevelId = '" + ddlFloorLabel + "'";
                                            DataTable dt = databaseCon.GetData(getflrlevel);

                                            if (dt.Rows.Count > 0)
                                            {
                                                if (dt.Rows[0]["FloorCount"].ToString() == "0")
                                                {
                                                  if(txtdescs=="undefined") {
                                                        txtdescs = "";
                                                  }
                                                    string query = "insert into tblSiteImages (ClientId,SiteId,FloorLevelId,ImageName,Description,CreatedBy,CreatedOn,LocId,BuildingID,DrawingName)values ('" + databaseCon.Decrypt(id) + "','" + ddlSites + "'," + (ddlFloorLabel=="0" ? "null" : ddlFloorLabel) + ",'" + fileName + "','" + txtdescs + "',(select EmailId from tblUser where UserId='" + databaseCon.Decrypt(UID) + "'),'" + DateTime.Now + "','" + ddlLocaSite + "','" + ddlBuilding + "','" + txtdrawing + "')";
                                                    i = databaseCon.RunCmd(query);


                                                    if (i != 0)
                                                    {
                                                        Result = "Success";
                                                        Message = "Image Saved Successfully";
                                                    }


                                                }
                                                else
                                                {
                                                    Result = "Failed";
                                                    Message = "Floor Level Image already exists for this Drawing Name.";
                                                }
                                               
                                            }
                                        }
                                        else
                                        {
                                            Result = "Failed";
                                            Message = "Floor Level Image already exists for this Drawing Name.";
                                        }
                                    }
                                }

                                // Commented Code For Location FloorImage Save functinality.

                                //else if (ddlSites.SelectedValue != "0" && ddlLocaSite.SelectedValue != "0")
                                //{

                                //    string fileName = ddlSites.Text.Replace("'", "''").Replace(" ", "").Trim() + "_" + ddlLocaSite.Text.Replace("'", "''").Replace(" ", "").Trim() + "_" + databaseCon.Decrypt(Request.QueryString["RID"]) + "_" + DateTime.Now.Ticks + "." + extension;
                                //    postedFile.SaveAs(Server.MapPath("~/images/SiteFloorlevelimg") + @"/" + fileName);
                                //    string path = "/images/SiteFloorlevelimg/";
                                //    path = Server.MapPath(path);

                                //    string fpath = path + fileName;

                                //  string querydrawingchck = "select Count(*) As DrawingCount  from tblSiteImages where ClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and SiteId='" + ddlSites.SelectedValue + "' and (LocID='" + ddlLocaSite.SelectedValue + "' or LocID is Null) and (BuildingID='" + ddlBuilding.SelectedValue + "' or BuildingID is Null) and DrawingName= '" + hdndrawing.Value + "'";
                                //    DataTable dt1 = databaseCon.GetData(querydrawingchck);
                                //  if (dt1.Rows.Count > 0)
                                //  {
                                //     if (dt1.Rows[0]["DrawingCount"].ToString() == "0")
                                //     {
                                //            string getflrlevel = "select Count(*) As FloorCount from tblSiteImages where ClientId = '" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and SiteId = '" + ddlSites.SelectedValue + "' and LocID = '" + ddlLocaSite.SelectedValue + "' and FloorLevelId = '" + ddlFloorLabel.SelectedValue + "'";
                                //            DataTable dt = databaseCon.GetData(getflrlevel);

                                //    if (dt.Rows.Count > 0)
                                //    {
                                //        if (dt.Rows[0]["FloorCount"].ToString() == "0")
                                //        {
                                //                string query = "insert into tblSiteImages (ClientId,SiteId,FloorLevelId,ImageName,Description,CreatedBy,CreatedOn,LocID,BuildingID,DrawingName) values ('" + databaseCon.Decrypt(Request.QueryString["id"]) + "','" + ddlSites.SelectedValue + "','" + ddlFloorLabel.SelectedValue + "','" + fileName + "','" + txtdescs.Text + "',(select EmailId from tblUser where UserId='" + databaseCon.Decrypt(Request.QueryString["UID"]) + "'),'" + DateTime.Now + "','" + ddlLocaSite.SelectedValue + "','" + ddlBuilding.SelectedValue + "','" + hdndrawing.Value + "')";
                                //                    i = databaseCon.RunCmd(query);


                                //            if (i != 0)
                                //            {
                                //                art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Success";
                                //                art1.Style.Add("display", "block");
                                //                alertlabel.Text = "<strong>Success! </strong>Image Saved Successfully";
                                //                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp1", "<script>hideAlert();</script>", false);

                                //                showthumnail();
                                //            }


                                //        }
                                //        else
                                //        {
                                //            art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                                //            art1.Style.Add("display", "block");
                                //            alertlabel.Text = "<strong>Alert! </strong>Floor Level Image already exists .";
                                //            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp", "<script>hideAlert();</script>", false);
                                //            showthumnail();//Add
                                //        }
                                //        txtdescs.Text = "";
                                //        ddlFloorLabel.ClearSelection();
                                //    }
                                //}
                                //     else
                                //        {
                                //            art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                                //            art1.Style.Add("display", "block");
                                //            alertlabel.Text = "<strong>Alert! </strong>Floor Level Image already exists for this Drawing Name .";
                                //            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp", "<script>hideAlert();</script>", false);
                                //            showthumnail();
                                //        }
                                //    }
                                //}

                                else if (!String.IsNullOrEmpty(ddlSites))
                                {

                                    string fileName = ddlSites.Replace("'", "''").Replace(" ", "").Trim() + "_" + databaseCon.Decrypt(RID) + "_" + DateTime.Now.Ticks + "." + extension;
                                    postedFile.SaveAs(Server.MapPath("~/images/SiteFloorlevelimg") + @"/" + fileName);
                                    string path = "/images/SiteFloorlevelimg/";
                                    path = Server.MapPath(path);

                                    string fpath = path + fileName;

                                    // databaseCon.savefileonblob(fileName, "Site-images", fpath, "image/png");
                                    // File.Delete(fpath);
                                    string querydrawingchck = "select Count(*) As DrawingCount  from tblSiteImages where ClientId='" + databaseCon.Decrypt(id) + "' and SiteId='" + ddlSites + "' and (LocID='" + ddlLocaSite + "' or LocID is Null) and (BuildingID='" + ddlBuilding + "' or BuildingID is Null) and DrawingName= '" + hdndrawing + "'";
                                    DataTable dt1 = databaseCon.GetData(querydrawingchck);
                                    if (dt1.Rows.Count > 0)
                                    {
                                        if (dt1.Rows[0]["DrawingCount"].ToString() == "0")
                                        {

                                            //string getflrlevel = "select Count(*) As FloorCount from tblSiteImages where ClientId = '" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and SiteId = '" + ddlSites.SelectedValue + "' and FloorLevel = '" + ddlFloorLabel.SelectedValue + "'";
                                            string getflrlevel = "select Count(*) As FloorCount from tblSiteImages where ClientId = '" + databaseCon.Decrypt(id) + "' and SiteId = '" + ddlSites + "' and isnull(FloorLevelId,0) = '" + ddlFloorLabel + "'";
                                            DataTable dt = databaseCon.GetData(getflrlevel);
                                            if (dt.Rows.Count > 0)
                                            {
                                                if (dt.Rows[0]["FloorCount"].ToString() == "0")
                                                {
                                                    //string query = "insert into tblSiteImages (ClientId,SiteId,FloorLevel,ImageName,Description,CreatedBy,CreatedOn) values ('" + databaseCon.Decrypt(Request.QueryString["id"]) + "','" + ddlSites.SelectedValue + "','" + ddlFloorLabel.SelectedValue + "','" + fileName + "','" + txtdescs.Text + "',(select EmailId from tblUser where UserId='" + databaseCon.Decrypt(Request.QueryString["UID"]) + "'),'" + DateTime.Now + "')";

                                                    //string query = "insert into tblSiteImages (ClientId,SiteId,FloorLevel,ImageName,Description,CreatedBy,CreatedOn,LocId,BuildingID,DrawingName)values ('" + databaseCon.Decrypt(Request.QueryString["id"]) + "','" + ddlSites.SelectedValue + "','" + ddlFloorLabel.SelectedValue + "','" + fileName + "','" + txtdescs.Text + "',(select EmailId from tblUser where UserId='" + databaseCon.Decrypt(Request.QueryString["UID"]) + "'),'" + DateTime.Now + "','" + ddlLocaSite.SelectedValue + "','" + ddlBuilding.SelectedValue + "','" + hdndrawing.Value + "')";
                                                  
                                                    if (txtdescs == "undifined")
                                                    {
                                                        txtdescs = "";
                                                    }
                                                   

                                                    string query = "insert into tblSiteImages (ClientId,SiteId,FloorLevelId,ImageName,Description,CreatedBy,CreatedOn,LocId,BuildingID,DrawingName)values ('" + databaseCon.Decrypt(id) + "','" + ddlSites + "'," + (ddlFloorLabel == "0" ? "null" : ddlFloorLabel) + ",'" + fileName + "','" + txtdescs + "',(select EmailId from tblUser where UserId='" + databaseCon.Decrypt(UID) + "'),'" + DateTime.Now + "','" + ddlLocaSite + "','" + ddlBuilding + "','" + txtdrawing + "')";

                                                    i = databaseCon.RunCmd(query);


                                                    if (i != 0)
                                                    {
                                                        Result = "Success";
                                                        Message = "Image Saved Successfully";
                                                    }
                                                }
                                                else
                                                {
                                                   
                                                    Result = "Failed";
                                                    Message = "Floor Level Image already exists.";
                                                }
                                               
                                            }
                                        }
                                        else
                                        {
                                           Result = "Failed";
                                            Message = "Floor Level Image already exists for this Drawing Name.";
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                           
                            Result = "Failed";
                            Message = "Valid File Extensions are jpg, jpeg, png, gif, bmp.";
                        }
                    }
                }
                else
                {
                    Result = "Failed";
                    Message = "Please Choose File.";
                }
            }
           return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult btndeletedyes_Click(string id, string ddlSites, string hdnimgnames, string ddlLocaSite, string ddlBuilding, string hdnImageId)
        {
            string Result = "";
            string Message = "";
            string query;
            DataTable dt;

            query = "select FloorLevelId,ImageName from tblSiteImages where SiteImageId = '" + hdnImageId + "'";
            dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                //var FloorLevel= dt.Rows[0]["FloorLevel"].ToString();
                var FloorLevel = dt.Rows[0]["FloorLevelId"].ToString();
                string imagename = dt.Rows[0]["ImageName"].ToString();

                //query = "select COUNT(*) as FloorImagePointCount from tblRegister where ClientId = '" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and SiteLocId='" + ddlSites.SelectedValue + "' and LocationID = '" + ddlLocaSite.SelectedValue + "' and ( BuildingID = '" + ddlBuilding.SelectedValue + "' or BuildingID is Null )  and FloorLevel = '" + FloorLevel + "'";
                query = "select COUNT(*) as FloorImagePointCount from tblRegister where ClientId = '" + databaseCon.Decrypt(id) + "' and SiteLocId='" + ddlSites + "' and LocationID = '" + ddlLocaSite + "' and ( BuildingID = '" + ddlBuilding + "' or BuildingID is Null )  and FloorLevelId = '" + FloorLevel + "'";
                dt = databaseCon.GetData(query);

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["FloorImagePointCount"].ToString() == "0")
                    {
                        //string dltimg = "delete from tblSiteImages where ClientId = '" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and SiteId = '" + ddlSites.SelectedValue + "' and ImageName = '" + hdnimgnames.Value + "'";
                        string dltimg = "delete from tblSiteImages where ClientId = '" + databaseCon.Decrypt(id) + "' and SiteImageId = '" + hdnImageId + "' and ImageName = '" + imagename + "'";
                        int dtimg = databaseCon.RunCmdID(dltimg);

                        if (dtimg != 0)
                        {

                            string dltcheckimg = "select Count(*) as ImageFileCount  from tblSiteImages where  ImageName= '" + imagename + "' and SiteImageId!= '" + hdnImageId + "'";
                            DataTable dtCount = databaseCon.GetData(dltcheckimg);

                            if (dtCount.Rows.Count > 0)
                            {
                                if (dtCount.Rows[0]["ImageFileCount"].ToString() == "0")
                                {
                                    //databaseCon.deletefileonblob(hdnimgnames.Value, "siteimage");
                                    string path = "/images/SiteFloorlevelimg/";
                                    path = Server.MapPath(path);
                                    System.IO.File.Delete(path + imagename);
                                    //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Success";
                                    //art1.Style.Add("display", "block");
                                    //alertlabel.Text = "<strong>Success! </strong>Image Deleted Successfully";
                                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp5", "<script>hideAlert();</script>", false);
                                    //showthumnail();
                                    Result = "Success";
                                    Message = "Image Deleted Successfully";
                                }
                                else
                                {
                                    //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Success";
                                    //art1.Style.Add("display", "block");
                                    //alertlabel.Text = "<strong>Success! </strong>Image Deleted Successfully";
                                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp5", "<script>hideAlert();</script>", false);
                                    //showthumnail();
                                    Result = "Success";
                                    Message = "Image Deleted Successfully";
                                }
                            }
                        }
                       // divdeleted.Style.Add("display", "none");
                    }
                    else
                    {
                        //showthumnail();
                        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Alert! </strong>This image is in use on a Job you cannot Delete .";
                        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp7", "<script>hideAlert();</script>", false);
                        //divdeleted.Style.Add("display", "none");
                        Result = "Failed";
                        Message = "This image is in use on a Job you cannot Delete.";
                    }
                }
            }
            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult shownewthumbnail(string id,string ddlFileSelect)
        {
           // var ImageName = ddlFileSelect.SelectedValue;
            string query = "select ImageName from tblSiteImages where ClientId = '" + databaseCon.Decrypt(id) + "' and DrawingName ='" + ddlFileSelect + "'";

            DataTable dt = databaseCon.GetData(query);
            // StringBuilder html = new StringBuilder();
            // html.Append("<table id='image1' style='width:100%'>");

            string html = "";
            html+="<table id='image1' style='width:100%'>";
            if (dt.Rows.Count > 0)
            {

                //for (int j = 0; j < dt.Rows.Count; j++)
                //{
                string imgchk = dt.Rows[0]["ImageName"].ToString();
               // ViewState["imgname"] = imgchk;
                if (imgchk != "" && imgchk != "NULL" && imgchk != null)
                {
                    string path = "/images/SiteFloorlevelimg/";
                    //  path = Server.MapPath(path);
                    string fpath = path + imgchk;


                    //html.Append("<div class='col-lg-4 col-md-6 col-sm-12 col-xs-12' style='margin-top: 0px;margin-left: 50px;'>");
                    //html.Append("<img id='ppdl1" + 0 + "' runnat='server' class='img-container' src='" + fpath + "' style='width: 300px;object-fit:contain; height: 250px' />");
                    ////html.Append("<div class='row' style='margin-top: -18px;text-align:left'>");
                    //html.Append("<div class='col-lg-12 col-md-12 col-sm-6 col-xs-12'>");
                    //html.Append("</div>");
                    //html.Append("</div>");


                    html+="<div class='col-lg-4 col-md-6 col-sm-12 col-xs-12' style='margin-top: 0px;margin-left: 50px;'>";
                    html +="<img id='ppdl1" + 0 + "' runnat='server' class='img-container' src='" + fpath + "' style='width: 300px;object-fit:contain; height: 250px' />";
                    html +="<div class='col-lg-12 col-md-12 col-sm-6 col-xs-12'>";
                    html+="</div>";
                    html+="</div>";
                }

                //}
                html+="</table>";
            }
            else
            {
            }

            // placeholderfordrawingimage.Controls.Add(new Literal { Text = html.ToString() });
            // txtdrawing.Text = ImageName;

            return Json(html,JsonRequestBehavior.AllowGet);
        }


    }
}