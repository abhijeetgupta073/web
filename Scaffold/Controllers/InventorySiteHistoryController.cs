using Scaffold.App_Code;
using Scaffold.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Scaffold_DataAccess;
using Microsoft.Ajax.Utilities;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace Scaffold.Controllers
{
    public class InventorySiteHistoryController : Controller
    {
        // GET: InventorySiteHistory
        ScaffoldEntities dbcontext = new ScaffoldEntities();
        public ActionResult InventorySiteHistory()
        {
            return View();
        }

        public ActionResult SiteDropdown(string ClientID, string UID)
        {
            var SiteOwnerName = "";
            string query1 = "select SiteOwnerName from tblUser where ClientId = '" + databaseCon.Decrypt(ClientID) + "'  and UserId='" + databaseCon.Decrypt(UID) + "'";
            DataTable dt1 = databaseCon.GetData(query1);
            if (dt1.Rows.Count > 0)
            {
                SiteOwnerName = dt1.Rows[0]["SiteOwnerName"].ToString();
            }
            if (SiteOwnerName != "")
            {
                // Escape single quote by replacing ' with ''
                SiteOwnerName = SiteOwnerName.Replace("'", "''");
                var getSiteLocationData = dbcontext.Database.SqlQuery<SiteLocationViewModel>("select distinct SiteLocation,SiteLocID from tblSiteLocation where SiteLocation <>'' and SiteLocation is not null and ClientId = '" + databaseCon.Decrypt(ClientID) + "'  and OwnerName='" + SiteOwnerName + "' and MaintainInventory = 1 and MaintainInventoryAtJob=1  order by SiteLocation").ToList();
                return Json(getSiteLocationData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //var GetData = dbcontext.Database.SqlQuery<SiteLocationViewModel>("select * from (select 0 as SiteLocID,'All' as SiteLocation union select distinct SiteLocID,SiteLocation from tblSiteLocation where SiteLocation <>'' and SiteLocation is not null and  ClientId = '" + databaseCon.Decrypt(ClientID) + "' and MaintainInventory = 1 ) as sitelisting order by SiteLocation").ToList();
                var GetData = dbcontext.Database.SqlQuery<SiteLocationViewModel>("select * from (  select distinct SiteLocID,SiteLocation from tblSiteLocation where SiteLocation <> '' and SiteLocation is not null and ClientId = '" + databaseCon.Decrypt(ClientID) + "' and MaintainInventory = 1) as sitelisting order by SiteLocation").ToList();
                return Json(GetData, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult MaterialDropDown(string SiteID,string ClientID)
        {
            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select distinct MaterialType from Vw_SiteInventory  join tblinventory on Vw_SiteInventory.InventoryId=tblInventory.InventoryID where SiteId='" + SiteID + "' Order by MaterialType").ToList();
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ddlMattype_SelectedIndexChanged(string SiteID, string ddlMattype,string ddlSubCategory)
        {
            //string query = "select '' as ItemCode,'All' as ItemCodeItemDesc Union select distinct ItemCode,ItemCode+'  -  '+ItemDesc as ItemCodeItemDesc from Vw_SiteInventory  join tblinventory on Vw_SiteInventory.InventoryId=tblInventory.InventoryID where SiteId=" + SiteID + " and MaterialType='" + ddlMattype.Replace("'", "''") + "' Order by ItemCode";
           //string query = "select '' as ItemCode,'All' as ItemCodeItemDesc,0 as InventoryId Union select distinct ItemCode,ItemCode+'  -  '+ItemDesc as ItemCodeItemDesc,tblInventory.InventoryID from Vw_SiteInventory  join tblinventory on Vw_SiteInventory.InventoryId=tblInventory.InventoryID where SiteId=" + SiteID + " and MaterialType='" + ddlMattype.Replace("'", "''") + "' Order by ItemCode";
            
            //Commented on 29-06-2023 (Added Subcategory filter in inventory Item)
            //string query = "select '' as ItemCode,'' as ItemCodeItemDesc,0 as InventoryId Union select distinct ItemCode,ItemCode+'  -  '+ItemDesc as ItemCodeItemDesc,tblInventory.InventoryID from Vw_SiteInventory  join tblinventory on Vw_SiteInventory.InventoryId=tblInventory.InventoryID where SiteId=" + SiteID + " and MaterialType='" + ddlMattype.Replace("'", "''") + "' Order by ItemCode";

            string query = "select '' as ItemCode,'' as ItemCodeItemDesc,0 as InventoryId Union select distinct ItemCode,ItemCode+'  -  '+ItemDesc as ItemCodeItemDesc,tblInventory.InventoryID from Vw_SiteInventory  join tblinventory on Vw_SiteInventory.InventoryId=tblInventory.InventoryID join tblsubcategory on tblinventory.subcategoryid = tblsubcategory.subcategoryid where SiteId= '" + SiteID + "' and MaterialType = '" + ddlMattype + "' and (tblsubcategory.SubCategoryID = '" + ddlSubCategory + "' or 'All' = '" + ddlSubCategory + "') Order by ItemCode";
            dbcontext.Database.CommandTimeout = 15 * 60;
            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>(query).ToList();
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ddlitemcode_SelectedIndexChanged(string SiteID, string ddlMattype, string ddlitemcode)
        {
            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select distinct ItemDesc from Vw_SiteInventory  join tblinventory on Vw_SiteInventory.InventoryId=tblInventory.InventoryID where SiteId=" + SiteID + " and MaterialType='" + ddlMattype.Replace("'", "''") + "' and ItemCode='" + ddlitemcode.Replace("'", "''") + "' Order by ItemDesc").ToList();
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSweatReportData(string ClientID, string SiteID, string ddlMattype, string FromDate, string EndDate, string ddlitemcode, string ddlitemdesc,string ReportType,string ddlSubCategory)
        {

            if (ddlMattype == "")
            {
                ddlMattype = "All";
            }
            if (FromDate == "")
            {
                FromDate = "All";
            }
            if (EndDate == "")
            {
                EndDate = "All";
            }
            if (ddlitemcode == "" || ddlitemcode == "undefined")
            {
                ddlitemcode = "0";
            }
            if (ddlitemdesc == "")
            {
                ddlitemdesc = "All";
            }
            if (ddlSubCategory == "" || ddlSubCategory == "undefined" || ddlSubCategory == "All")
            {
                ddlSubCategory = "0";
            }
            if (ReportType == "S") 
            {
                FromDate = DateTime.Now.ToShortDateString();
                EndDate = DateTime.Now.ToShortDateString();
            }
            string query = " exec Pro_InventorySiteHistory '" + databaseCon.Decrypt(ClientID) + "' ," + SiteID + " , '" + ddlMattype + "', '" + FromDate + "', '" + EndDate + "','" + ddlitemcode + "', '" + ReportType + "','" + ddlSubCategory + "'";
            //procedure renamed on 24july23 by pooja
            //string query = " exec Pro_SweatReportDashBoard '" + databaseCon.Decrypt(ClientID) + "' ," + SiteID + " , '" + ddlMattype + "', '" + FromDate + "', '" + EndDate + "','" + ddlitemcode + "', '" + ReportType + "','" + ddlSubCategory + "'";
            dbcontext.Database.CommandTimeout = 15 * 60;
            var GetData = dbcontext.Database.SqlQuery<GetSweatReportDataModel>(query).ToList();
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }

        public void ExportToCsvFile(string ClientID, string SiteID, string ddlMattype, string FromDate, string EndDate, string ddlitemcode, string ddlitemdesc,string ReportType, string ddlSubCategory)
        {
            string Material = databaseCon.Decrypt(ddlMattype).ToString();
            if (Material == "")
            {
                Material = "All";
            }
            if (FromDate == "")
            {
                FromDate = "All";
            }
            if (EndDate == "")
            {
                EndDate = "All";
            }
            if (ddlitemcode == "" || ddlitemcode == "undefined")
            {
                ddlitemcode = "0";
            }
            if (ddlitemdesc == "")
            {
                ddlitemdesc = "All";
            }
            if (ddlSubCategory == "" || ddlSubCategory == "undefined" || ddlSubCategory == "All")
            {
                ddlSubCategory = "0";
            }
            if (ReportType == "S")
            {
                FromDate = DateTime.Now.ToShortDateString();
                EndDate = DateTime.Now.ToShortDateString();
            }
            string allcategory = " exec Pro_InventorySiteHistory '" + databaseCon.Decrypt(ClientID) + "' ," + SiteID + " , '" + Material + "', '" + FromDate + "', '" + EndDate + "','" + ddlitemcode + "', '" + ReportType + "','" + ddlSubCategory + "'";
            //procedure renamed on 24july23 by pooja
            //string allcategory = " exec Pro_SweatReportDashBoard '" + databaseCon.Decrypt(ClientID) + "' ," + SiteID + " , '" + ddlMattype + "', '" + FromDate + "', '" + EndDate + "','" + ddlitemcode + "', '" + ReportType + "','" + ddlSubCategory + "'";
            dbcontext.Database.CommandTimeout = 2000 * 60;
            DataTable dt = databaseCon.GetData(allcategory);

            Response.ContentType = "Application/x-msexcel";
            Response.AddHeader("content-disposition", "attachment;filename='Inventory Site History Summary Report ' - " + DateTime.Now.Date.ToString("dd'-'MM'-'yyyy") + ".csv");
            Response.Write(csvfile(dt));
            Response.End();
        }
        public void ExportToCsvFileDetail(string ClientID, string SiteID, string ddlMattype, string FromDate, string EndDate, string ddlitemcode, string ddlitemdesc, string ReportType, string ddlSubCategory)
        {
            string Material = databaseCon.Decrypt(ddlMattype).ToString();
            if (Material == "")
            {
                Material = "All";
            }
            if (FromDate == "")
            {
                FromDate = "All";
            }
            if (EndDate == "")
            {
                EndDate = "All";
            }
            if (ddlitemcode == "" || ddlitemcode == "undefined")
            {
                ddlitemcode = "0";
            }
            if (ddlitemdesc == "")
            {
                ddlitemdesc = "All";
            }
            if (ddlSubCategory == "" || ddlSubCategory == "undefined" || ddlSubCategory == "All")
            {
                ddlSubCategory = "0";
            }
            string allcategory = " exec Pro_InventorySiteHistory '" + databaseCon.Decrypt(ClientID) + "' ," + SiteID + " , '" + Material + "', '" + FromDate + "', '" + EndDate + "','" + ddlitemcode + "', '" + ReportType + "','" + ddlSubCategory + "'";
            //procedure renamed on 24july23 by pooja
            //string allcategory = " exec Pro_SweatReportDashBoard '" + databaseCon.Decrypt(ClientID) + "' ," + SiteID + " , '" + ddlMattype + "', '" + FromDate + "', '" + EndDate + "','" + ddlitemcode + "', '" + ReportType + "','" + ddlSubCategory + "'";
            dbcontext.Database.CommandTimeout = 200000 * 60;
            DataTable dt = databaseCon.GetData(allcategory);

            Response.ContentType = "Application/x-msexcel";
            Response.AddHeader("content-disposition", "attachment;filename='Inventory Site History Detail Report ' - " + DateTime.Now.Date.ToString("dd'-'MM'-'yyyy") + ".csv");
            Response.Write(csvfile(dt));
            Response.End();
        }
        public class GetSweatReportDataModel
        {
            //Commented LastFriday on 17-08-2023 renamed as Today
            //public DateTime LastFriday { get; set; }
            public DateTime Today { get; set; }
            public DateTime weekdays { get; set; }

            //public DateTime Today { get; set; }
            public decimal? OnSiteQuantity { get; set; }
            public int? UsedQty { get; set; }
            public decimal? Utilation { get; set; }
            public string ItemCode { get; set; }
            public decimal? Available { get; set; }
            public string ItemDesc { get; set; }
            public string Code { get; set; }
            public string Description { get; set; }
            public string subcategory { get; set; }
            public int QuantityWithJob { get; set; }
            public decimal? OnSite { get; set; }

            //public int? CurrentUtilisation { get; set; }

            //Commented ToDateUtilisation on 17-08-2023 renamed as CurrentUtilisation
            //public decimal? ToDateUtilisation { get; set; }
            public decimal? CurrentUtilisation { get; set; }
        }

        private string csvfile(DataTable dtTable)
        {
            StringBuilder sbldr = new StringBuilder();
            try
            {
                if (dtTable.Columns.Count != 0)

                {
                    //foreach (DataColumn col in dtTable.Columns)
                    //{
                    //    sbldr.Append(col.ColumnName + ',');
                    //}
                    //sbldr.Append("\r\n");
                    //foreach (DataRow row in dtTable.Rows)
                    //{
                    //    foreach (DataColumn column in dtTable.Columns)
                    //    {
                    //        if (row[column].ToString().Contains(','))
                    //        {
                    //            sbldr.Append("\"" + row[column].ToString() + "\"" + ',');
                    //        }
                    //        else
                    //        {
                    //            sbldr.Append(row[column].ToString() + ',');
                    //        }
                    //        //sbldr.Append(row[column].ToString().Replace(",", ";") + ',');
                    //    }
                    //    sbldr.Append("\r\n");
                    //}

                    foreach (DataColumn col in dtTable.Columns)
                    {
                        sbldr.Append(col.ColumnName + ',');
                    }
                    sbldr.Append("\r\n");

                    // Write rows
                    foreach (DataRow row in dtTable.Rows)
                    {
                        foreach (DataColumn column in dtTable.Columns)
                        {
                            if (column.DataType == typeof(DateTime)) // Check if the column is of type DateTime
                            {
                                DateTime dateValue = (DateTime)row[column];
                                string formattedDate = dateValue.ToString("dd-MMM-yyyy"); // Apply desired date format
                                sbldr.Append("\"" + formattedDate + "\"" + ',');
                            }
                            else if (row[column].ToString().Contains(','))
                            {
                                sbldr.Append("\"" + row[column].ToString() + "\"" + ',');
                            }
                            else
                            {
                                sbldr.Append(row[column].ToString() + ',');
                            }
                        }
                        sbldr.Append("\r\n");
                    }

                }

            }
            catch
            {

            }
            return sbldr.ToString();
        }

        public ActionResult SubCategoryDropDown(string id,string ddlMattype)
        {
            if (ddlMattype == "")
            {
                ddlMattype = "All";
            }
            var getSubCategoryData = dbcontext.Database.SqlQuery<SubCategoryViewModel>("select distinct SubCategoryID,SubCategory,Category from tblSubCategory  where clientid=" + databaseCon.Decrypt(id) + " and (Category = '" + ddlMattype + "' or 'All' = '" + ddlMattype + "') order by SubCategory").ToList();
            return Json(getSubCategoryData, JsonRequestBehavior.AllowGet);

        }
    }
}