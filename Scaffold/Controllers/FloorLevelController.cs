using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scaffold.Controllers
{
    public class FloorLevelController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: FloorLevel
        public ActionResult FloorLevel(string UID, string UserRole, string id)
        {
            return View();
        }

        public ActionResult GetFloorLevelData(string id, string SiteID, string ddlLocaSite, string ddlBuilding, string CategoryValue)
        {

            string query = "";
           
             query = "exec [Pro_TBLFLOORLEVEL_GET] ''," + databaseCon.Decrypt(id) + ",'" + SiteID + "','" + CategoryValue + "','" + ddlLocaSite + "','" + ddlBuilding + "'";
            var getData = dbContext.Database.SqlQuery<FloorLevelViewModel>(query).ToList();
            return Json(getData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ddlSiteDropdown(string id)
        {
            string query = "select distinct SiteLocID,SiteLocation from tblSiteLocation where SiteLocation <>'' and SiteLocation is not null and  ClientId='" + HttpUtility.UrlEncode(databaseCon.Decrypt(id)) + "' order by SiteLocation";

            DataTable dt = databaseCon.GetData(query);

            var ddlSiteData = dbContext.Database.SqlQuery<FloorLevelddlViewModel>(query).ToList();
            return Json(ddlSiteData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult dropdownlocation(string id, string ddlSitesLoc)
        {
            string query;
            
            query = "select area,AreaID from tblArea where ClientID='" + databaseCon.Decrypt(id) + "' ";
            if (!String.IsNullOrEmpty(ddlSitesLoc))
            {
                query = query + "  and(SiteLocId = '" + ddlSitesLoc + "')";
            }
            query = query + " order by area";
            var ddlLocationData = dbContext.Database.SqlQuery<FloorLevelddlViewModel>(query).ToList();
            return Json(ddlLocationData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ddlSites(string ddlSitesLoc)
        {
           // dropdownlocation();
            string SiteId = ddlSitesLoc;
            string txtboxCategory = "";
            string query = "select Case When  isnull(GPS,0)=1 Then 'GPS' when isnull(FloorLevel,0)=1 Then 'Floor Level' Else '' End as Category from tblSiteLocation where SiteLocId=" + SiteId;
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count != 0)
            {
                string val = dt.Rows[0]["Category"].ToString();
                txtboxCategory = val;
            }
            else
            {
                txtboxCategory = " ";
            }
            return Json(txtboxCategory, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ddlBuildingLoc(string id, string ddlLocationId)
        {
            string query;
            query = "select buildingid,buildingno  from tblbuilding where BuildingStatus='Active' and ClientID='" + databaseCon.Decrypt(id) + "' and Areaid='" + ddlLocationId + "' order by buildingno";
            DataTable dtbuilding = databaseCon.GetData(query);
            var ddlBuildingLoc = dbContext.Database.SqlQuery<FloorLevelddlViewModel>(query).ToList();
            return Json(ddlBuildingLoc, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveFloorLevelData(string FloorTypeId, string txtFloorLevel, string NumericTextSortOrder,  string ddlStatuss, string txtboxCategory, string id, string UID, string ddlSitesLoc, string ddlLocaSite, string ddlBuilding)
        {

            string query = "";
            DataTable dt;
            string Result = "";
            string Message = "";
            //art1.Style.Add("display", "none");


            //if (e.CommandName == RadGrid.InitInsertCommandName || e.CommandName == RadGrid.EditCommandName)
            //{

            //    gridFloorType.ShowHeader = false;
            //}
            //else
            //{
            //    gridFloorType.ShowHeader = true;
            //}

            //if (e.CommandName == RadGrid.PerformInsertCommandName)
            //{
            //    GridEditableItem item = (GridEditableItem)e.Item;
            //    RadTextBox txtCategory = (RadTextBox)item.FindControl("txtCategory");
            //    RadTextBox txtSiteLocation = (RadTextBox)item.FindControl("txtSiteLocation");
            //    RadTextBox txtFloorLevel = (RadTextBox)item.FindControl("txtFloorLevel");
            //    RadComboBox ddlStatuss = (RadComboBox)item.FindControl("ddlStatuss");
            //    RadNumericTextBox NumericTextSortOrder = (RadNumericTextBox)item.FindControl("NumericTextSortOrder");

                if (FloorTypeId == "0")
                {

                if (txtboxCategory == "Floor Level" && ddlLocaSite != "0" && ddlBuilding != "0")
                {
                    //string category= ddlCategory.SelectedValue;
                    //query = "exec [Pro_TBLFLOORLEVEL_SAVE] '0','" + txtboxCategory.Text + "'," + ddlSitesLoc.SelectedValue + "," + databaseCon.Decrypt(Request.QueryString["id"]) + ",'" + txtFloorLevel.Text.Replace("'", "''").Trim() + "','" + ddlStatuss.Text.Replace("'", "''").Trim() + "'," + databaseCon.Decrypt(Request.QueryString["UID"]) + ",'" + DateTime.Now + "'," + NumericTextSortOrder.Text;

                    query = "exec [Pro_TBLFLOORLEVEL_SAVE] '0','" + txtboxCategory + "'," + ddlSitesLoc + "," + databaseCon.Decrypt(id) + ",'" + txtFloorLevel.Replace("'", "''").Trim() + "','" + ddlStatuss.Replace("'", "''").Trim() + "'," + databaseCon.Decrypt(UID) + ",'" + DateTime.Now + "'," + NumericTextSortOrder + "," + ddlLocaSite + ",'" + ddlBuilding + "'";
                    dt = databaseCon.GetData(query);
                    if (dt.Rows.Count != 0)
                    {
                        if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                        {
                            //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                            //art1.Style.Add("display", "block");
                            //alertlabel.Text = "<strong>Success ! </strong>" + dt.Rows[0][1].ToString();
                            //gridFloorType.Rebind();
                            Result = "Success";
                            Message = dt.Rows[0][1].ToString();
                        }
                        else if (Convert.ToInt32(dt.Rows[0][0]) == -1)
                        {
                            //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                            //art1.Style.Add("display", "block");
                            //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                            Result = "Failed";
                            Message = dt.Rows[0][1].ToString();
                        }
                        else
                        {
                            //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                            //art1.Style.Add("display", "block");
                            //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                            Result = "Failed";
                            Message = dt.Rows[0][1].ToString();
                        }
                    }
                    else
                    {
                        //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator ";
                        Result = "Failed";
                        Message = "Syntax Error. Contact administrator";
                    }
                }
                else if (txtboxCategory == "GPS") {
                    query = "exec [Pro_TBLFLOORLEVEL_SAVE] '0','" + txtboxCategory + "'," + ddlSitesLoc + "," + databaseCon.Decrypt(id) + ",'" + txtFloorLevel.Replace("'", "''").Trim() + "','" + ddlStatuss.Replace("'", "''").Trim() + "'," + databaseCon.Decrypt(UID) + ",'" + DateTime.Now + "'," + NumericTextSortOrder + "," + ddlLocaSite + ",'" + ddlBuilding + "'";
                    dt = databaseCon.GetData(query);
                    if (dt.Rows.Count != 0)
                    {
                        if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                        {
                            //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                            //art1.Style.Add("display", "block");
                            //alertlabel.Text = "<strong>Success ! </strong>" + dt.Rows[0][1].ToString();
                            //gridFloorType.Rebind();
                            Result = "Success";
                            Message = dt.Rows[0][1].ToString();
                        }
                        else if (Convert.ToInt32(dt.Rows[0][0]) == -1)
                        {
                            //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                            //art1.Style.Add("display", "block");
                            //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                            Result = "Failed";
                            Message = dt.Rows[0][1].ToString();
                        }
                        else
                        {
                            //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                            //art1.Style.Add("display", "block");
                            //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                            Result = "Failed";
                            Message = dt.Rows[0][1].ToString();
                        }
                    }
                    else
                    {
                        //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator ";
                        Result = "Failed";
                        Message = "Syntax Error. Contact administrator";
                    }
                }
                else {
                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong>Please Select Location & Building Then Click On Add New Floor Level.";
                    Result = "Failed";
                    Message = "Please Select Location & Building Then Click On Add New Floor Level.";
                }
                }
                else
                {
                //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                //art1.Style.Add("display", "block");
                //alertlabel.Text = "<strong>Alert ! </strong>Please Select Site Then Click On Add New Floor Level.";
                Result = "Failed";
                Message = "Please Select Site Then Click On Add New Floor Level.";
            }
          //  }

            //if (e.CommandName == RadGrid.UpdateCommandName)
            //{
            if(FloorTypeId != "0")
            { 
                //GridEditableItem item = (GridEditableItem)e.Item;
                //RadTextBox txtCategory = (RadTextBox)item.FindControl("txtCategory");
                //RadTextBox txtSiteLocation = (RadTextBox)item.FindControl("txtSiteLocation");
                //RadTextBox txtFloorLevel = (RadTextBox)item.FindControl("txtFloorLevel");
                //RadComboBox ddlStatuss = (RadComboBox)item.FindControl("ddlStatuss");
                //RadNumericTextBox NumericTextSortOrder = (RadNumericTextBox)item.FindControl("NumericTextSortOrder");
                //string FloorTypeId = item.GetDataKeyValue("FloorLevelId").ToString();
                //string SiteLocID = txtSiteLocation.Text; 
                //string SiteId = ddlSitesLoc.SelectedValue;
                //string SiteId= item.GetDataKeyValue("SiteLocId").ToString();


                //query = "exec [Pro_TBLFLOORLEVEL_SAVE] '" + FloorTypeId + "','" + txtboxCategory.Text+ "'," + ddlSitesLoc.SelectedValue + "," + databaseCon.Decrypt(Request.QueryString["id"]) + ",'" + txtFloorLevel.Text.Replace("'", "''").Trim() + "','" + ddlStatuss.Text.Replace("'", "''").Trim() + "'," + databaseCon.Decrypt(Request.QueryString["UID"]) + ",'" + DateTime.Now + "'," + NumericTextSortOrder.Text;
                query = "exec [Pro_TBLFLOORLEVEL_SAVE] '" + FloorTypeId + "','" + txtboxCategory + "'," + ddlSitesLoc + "," + databaseCon.Decrypt(id) + ",'" + txtFloorLevel.Replace("'", "''").Trim() + "','" + ddlStatuss.Replace("'", "''").Trim() + "'," + databaseCon.Decrypt(UID) + ",'" + DateTime.Now + "'," + NumericTextSortOrder + "," + ddlLocaSite + ",'" + ddlBuilding + "'";

                dt = databaseCon.GetData(query);
                if (dt.Rows.Count != 0)
                {
                    if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                    {
                        //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Success ! </strong>" + dt.Rows[0][1].ToString();
                        //gridFloorType.Rebind();
                        Result = "Success";
                        Message = dt.Rows[0][1].ToString();
                    }
                    else if (Convert.ToInt32(dt.Rows[0][0]) == -1)
                    {
                        //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                        Result = "Failed";
                        Message = dt.Rows[0][1].ToString();
                    }
                    else
                    {
                        //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                        Result = "Failed";
                        Message = dt.Rows[0][1].ToString();
                    }
                }
                else
                {
                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator ";
                    Result = "Failed";
                    Message = "Syntax Error. Contact administrator ";
                }
            }
            //if (e.CommandName == "ExportToCsv")
            //{
            //    gridFloorType.ExportSettings.FileName = "FloorList";
            //    gridFloorType.AllowFilteringByColumn = false;
            //    gridFloorType.ExportSettings.IgnorePaging = true;

            //    gridFloorType.MasterTableView.GetColumn("UniqueCategory").Visible = true;
            //    gridFloorType.MasterTableView.GetColumn("UniqueSiteLocation").Visible = true;

            //    gridFloorType.MasterTableView.ExportToCSV();

            //    //gridFloorType.MasterTableView.Items[0].Visible = true;
            //    //gridFloorType.MasterTableView.Items[2].Visible = false;
            //    //foreach (GridDataItem item in gridFloorType.MasterTableView.Items)
            //    //    item.Visible = item.Selected;

            //    //gridFloorType.ExportSettings.ExportOnlyData =false;  

            //}

            //if (e.CommandName == RadGrid.DeleteCommandName)
            //{
            //    GridEditableItem item = (GridEditableItem)e.Item;
            //    // string FloorTypeId = item.GetDataKeyValue("FloorLevelId").ToString();
            //    ViewState["Floorfordelete"] = Convert.ToInt32(item.GetDataKeyValue("FloorLevelId"));
            //    divdelete.Style.Add("display", "block");
            //}
            var response = new { Result = Result, Message = Message };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteDocument(string FloorLevelId)
        {
            string query = "";
            DataTable dt;
            string Result = "";
            string Message = "";
            query = "exec [Pro_TBLFLOORLEVEL_DEL] '" + FloorLevelId + "'";

            dt = databaseCon.GetData(query);
            if (dt.Rows.Count != 0)
            {
                if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                {
                    Result = "Success";
                    Message = dt.Rows[0][1].ToString();
                }
                else if (Convert.ToInt32(dt.Rows[0][0]) == -1)
                {
                    Result = "Failed";
                    Message = dt.Rows[0][1].ToString();
                }
                else
                {
                    Result = "Failed";
                    Message = dt.Rows[0][1].ToString();
                }
            }
            else
            {
                Result = "Failed";
                Message = "Syntax Error. Contact administrator ";
            }
           
            var response = new { Result = Result, Message = Message };
            return Json(response, JsonRequestBehavior.AllowGet);
        }


    }

    public class FloorLevelViewModel 
    {
        public Nullable<int> FloorLevelId { get; set; }
        public Nullable<int> ClientID { get; set; }
        public Nullable<int> SiteLocId { get; set; }
        public string SiteLocation { get; set; }
        public string Category { get; set; }
        public string FloorLevel { get; set; }
        public string FloorLevelStatus { get; set; }
        public Nullable<int> SortOrder { get; set; }

    }

}