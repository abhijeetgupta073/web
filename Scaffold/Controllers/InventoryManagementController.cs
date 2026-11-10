using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.InkML;
using iTextSharp.text.pdf.qrcode;
using OfficeOpenXml;
using OfficeOpenXml.Packaging.Ionic.Zlib;
using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;

namespace Scaffold.Controllers
{
    public class InventoryManagementController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();

        SqlCommand cmd = null;
        SqlConnection con = null;
        string FlagQuantity;
        string FlagSubCategory = "";
        // GET: cc
        public ActionResult InventoryManagement(string id, string UID, string UserRole)
        {
            ViewBag.id = databaseCon.Decrypt(id);
            ViewBag.UID = databaseCon.Decrypt(UID);
            ViewBag.UserRole = databaseCon.Decrypt(UserRole);

            string Hirequery = "select isnull(HireInventory,0) as HireInventory from tblClient where clientid='" + (databaseCon.Decrypt(Request.QueryString["id"])) + "'";
            DataTable dtHireInventory = databaseCon.GetData(Hirequery);
            if (dtHireInventory.Rows.Count > 0)
            {
                for (int i = 0; i < dtHireInventory.Rows.Count; i++)
                {
                    if (dtHireInventory.Rows[i]["HireInventory"].ToString() == "True")
                    {
                        // hdnhire.Value = "true";
                        ViewBag.hdnhire = "true";
                    }
                    else
                    {
                        // hdnhire.Value = "false";
                        ViewBag.hdnhire = "false";
                    }
                }
            }

            return View();
        }

        public ActionResult GetInventoryData(String id, string MaterialType)
        {

            // string q = "exec Pro_TBLINVENTORY_GET '','" + databaseCon.Decrypt(id) + "'";
            //var InventoryData = dbContext.Database.SqlQuery<InventoryViewModel>(q).ToList();

            var InventoryData = dbContext.Database.SqlQuery<InventoryViewModel>("exec Pro_TBLINVENTORY_GET '','" + databaseCon.Decrypt(id) + "','" + MaterialType + "'").ToList();
            return Json(InventoryData, JsonRequestBehavior.AllowGet);
        }


        public int InvManagementSaveData(String id, string UID, string Invid, string Mattype, string itemcode, int Quantity, decimal itemweight, string itemdesc, string comments, string lerect, string ldismantle, string subcategory, string DisplaySortOrder)

        {

            string displaySortOrderPart = string.IsNullOrEmpty(DisplaySortOrder) ? "null" : Convert.ToInt32(DisplaySortOrder).ToString();


            string query = "exec Pro_TBLINVENTORY_SAVE '" + Invid + "','" + Mattype + "','" + itemcode + "','" + itemdesc.Replace("'", "''") + "','" + itemweight + "','" + databaseCon.Decrypt(id) + "','" + UID + "','" + DateTime.Now + "','" + Quantity + "','" + comments + "'," + (lerect == "" ? "null" : "'" + lerect + "'") + "," + (ldismantle == "" ? "null" : "'" + ldismantle + "'") + "," + (subcategory == "0" ? "null" : "'" + subcategory + "'," + displaySortOrderPart + "");
            string checkSubCategory = subcategory;
            if (checkSubCategory == "0")
            {
                subcategory = "";
                string Hirequery = "select isnull(HireInventory,0) as HireInventory from tblClient where clientid='" + (databaseCon.Decrypt(id)) + "'";
                DataTable dtHireInventory = databaseCon.GetData(Hirequery);
                if (dtHireInventory.Rows.Count > 0)
                {
                    for (int i = 0; i < dtHireInventory.Rows.Count; i++)
                    {
                        if (dtHireInventory.Rows[i]["HireInventory"].ToString() == "True")
                        {
                            var Result = 0;

                            return Result;

                        }
                    }
                }

            }
            //dbContext.Database.CommandTimeout = 15 * 60;
            //string query = "exec Pro_TBLINVENTORY_SAVE '" + Invid + "','" + Mattype + "','" + itemcode + "','" + itemdesc.Replace("'", "''") + "','" + itemweight + "','" + databaseCon.Decrypt(id) + "','" + UID + "','" + DateTime.Now + "','" + Quantity + "','" + comments + "'," + (lerect == "" ? "null" : "'" + lerect + "'") + "," + (ldismantle == "" ? "null" : "'" + ldismantle + "'") + ",'" + (subcategory == "" ? "null" : "" + subcategory + "") + "'";

            //var result = dbContext.Database.ExecuteSqlCommand(query);

            //return result;

            dbContext.Database.CommandTimeout = 15 * 60;
            string query1 = "exec Pro_TBLINVENTORY_SAVE '" + Invid + "','" + Mattype + "','" + itemcode + "','" + itemdesc.Replace("'", "''") + "','" + itemweight + "','" + databaseCon.Decrypt(id) + "','" + UID + "','" + DateTime.Now + "','" + Quantity + "','" + comments + "'," + (string.IsNullOrEmpty(lerect) ? "null" : "'" + lerect + "'") + "," + (string.IsNullOrEmpty(ldismantle) ? "null" : "'" + ldismantle + "'") + "," + (string.IsNullOrEmpty(subcategory) ? "null" : "'" + subcategory + "'") + "," + displaySortOrderPart;

            var result = dbContext.Database.ExecuteSqlCommand(query1);
            return result;


        }


        public ActionResult MaterialDropDown(string id)
        {
            //var getMaterialType = dbContext.Database.SqlQuery<InventoryViewModel>("select distinct MaterialType from tblInventory  where clientid = " + databaseCon.Decrypt(id) + " order by MaterialType").ToList();
            var getMaterialType = dbContext.Database.SqlQuery<InventoryViewModel>("select distinct MaterialType from tblMaterialType  where clientid = " + databaseCon.Decrypt(id) + " order by MaterialType").ToList();//Added on 22-April-2022
            return Json(getMaterialType, JsonRequestBehavior.AllowGet);

        }

        public ActionResult SubCategoryDropDown(string id, string mattype)
        {
            var getSubCategoryData = dbContext.Database.SqlQuery<SubCategoryViewModel>("select distinct SubCategoryID,SubCategory from tblSubCategory  where clientid = " + databaseCon.Decrypt(id) + " and Category = '" + mattype + "' order by SubCategory").ToList();
            return Json(getSubCategoryData, JsonRequestBehavior.AllowGet);


        }


        public ActionResult EditData(string id, int Invid)
        {
            //var getInventoryChangedData = dbContext.Database.SqlQuery<InventoryChangedViewModel>("select * from tblInventoryChange  where InventoryId=" + Invid + "and ClientId=" + databaseCon.Decrypt(id) + "").ToList();
            var getInventoryChangedData = dbContext.Database.SqlQuery<InventoryChangedViewModel>("select InvChangeId,	ClientId,	InventoryId	,OldQuantity,	NewQuantity	,ChangeComments	,CreatedBy,	CreatedOn	,ChangeType	,OldValue,	NewValue from tblInventoryChange  where InventoryId=" + Invid + "and ClientId=" + databaseCon.Decrypt(id) + "").ToList();
            return Json(getInventoryChangedData, JsonRequestBehavior.AllowGet);
        }

        public int InvManagementDeleteData(string id, int Invid)
        {
            dbContext.Database.CommandTimeout = 15 * 60;
            var result = dbContext.Database.ExecuteSqlCommand("exec Pro_TBLInventory_DEL '" + Invid + "','" + databaseCon.Decrypt(id) + "'");
            return result;
        }


        public ActionResult UploadInventoryData(string UID, string ClientID, bool IsSort)
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);

            bool HireInventory = false;

            string Hirequery = "select isnull(HireInventory,0) as HireInventory from tblClient where clientid='" + (databaseCon.Decrypt(ClientID)) + "'";
            DataTable dtHireInventory = databaseCon.GetData(Hirequery);
            if (dtHireInventory.Rows.Count > 0)
            {
                if (dtHireInventory.Rows[0]["HireInventory"].ToString() == "True")
                {
                    HireInventory = true;
                }

            }

            databaseCon.RunCmd("delete from tblTempInventoryDataUpload where UserId = " + databaseCon.Decrypt(UID));

            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int k = 0; k < 1; k++)
                    {

                        HttpPostedFileBase file = files[k];
                        string filename;
                        string filePath;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            filename = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            filename = file.FileName;
                        }

                        var UserFileName = filename;
                        // loginUserId = DataBaseCon.Decrypt(loginUserId);
                        // Get the complete folder path and store the file inside it.  
                        filePath = Path.Combine(Server.MapPath("~/Files/"), filename + ".xlsx");
                        file.SaveAs(filePath);

                        var FileExtension = System.IO.Path.GetExtension(filename).ToLower();
                        if (FileExtension == ".xlsx")
                        {
                            try
                            {
                                string Filename = filename;

                                //string filePath = Server.MapPath(FileUploadSelectfile.PostedFile.FileName);
                                //filePath = Server.MapPath("~/Files/") + filename;

                                //FileUploadSelectfile.SaveAs(filePath);

                                FileInfo workBook = new FileInfo(filePath);
                                using (ExcelPackage xlPackage = new ExcelPackage(workBook))
                                {
                                    //Fetch the worksheet to insert the data
                                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[1]; //ContactImportTemplate
                                    int totalRows = worksheet.Dimension.End.Row;

                                    int totalCols = worksheet.Dimension.End.Column;
                                    DataTable dtExcelData = new DataTable(worksheet.Name);
                                    int validcount = 0;
                                    int errorexist = 0;
                                    int Subdata = 0;

                                    for (int n = 1; n <= totalCols; n++)
                                    {
                                        string ColName = worksheet.Cells[1, n].Value.ToString().Trim();
                                        dtExcelData.Columns.Add(ColName);
                                        if (n == 1)
                                        {
                                            if (ColName.ToLower() != "MaterialType".ToLower())
                                            {
                                                errorexist = 1;
                                            }

                                        }
                                        if (n == 2)
                                        {
                                            if (ColName.ToLower() != "ItemCode".ToLower())
                                            {
                                                errorexist = 1;
                                            }

                                        }
                                        validcount = validcount + 1;

                                        if (ColName == "Quantity")
                                        {
                                            FlagQuantity = "yes";
                                        }
                                        // Add code on 12-March-2020.
                                        if (ColName == "SubCategory")
                                        {
                                            FlagSubCategory = "yes";
                                        }

                                    }

                                    if (FlagQuantity == "yes")
                                    {
                                        // Add code on 12-March-2020.
                                        if ((HireInventory == true) && FlagSubCategory == "yes")
                                        {
                                            //if (validcount != 8) // change on 07 March 2020.(Added 1 more New Column in Excel)
                                            //{
                                            //    errorexist = 1;
                                            //}
                                            if (validcount != 9) // change on 16-04-2025 (Added 1 more New Column in Excel DisplaySortorder)
                                            {
                                                errorexist = 1;
                                            }


                                        }
                                        else if ((HireInventory == true) && FlagSubCategory == "")
                                        {
                                            errorexist = 1;
                                        }
                                        else if ((HireInventory == false) && (FlagSubCategory == "" || FlagSubCategory == "yes"))
                                        {
                                            //if (validcount != 7)
                                            //{
                                            //    errorexist = 1;
                                            //}
                                            //change on 16 - 04 - 2025(Added 1 more New Column in Excel DisplaySortorder)
                                            if (validcount != 8)
                                            {
                                                errorexist = 1;
                                            }
                                        }
                                        // if (validcount != 7) // change on 07 March 2020.(Added 1 more New Column in Excel)
                                        //if (validcount != 8)
                                        //{
                                        //    errorexist = 1;
                                        //}
                                    }

                                    else
                                    {
                                        // else if (validcount != 6)  // change on 07 March 2020
                                        if ((HireInventory == true) && FlagSubCategory == "yes")
                                        {
                                            //if (validcount != 7)z
                                            //{
                                            //    errorexist = 1;
                                            //}
                                            //change on 16 - 04 - 2025(Added 1 more New Column in Excel DisplaySortorder)
                                            if (validcount != 8)
                                            {
                                                errorexist = 1;
                                            }
                                        }

                                        else if ((HireInventory == true) && FlagSubCategory == "")
                                        {
                                            errorexist = 1;
                                        }

                                        else if ((HireInventory == false) && (FlagSubCategory == "" || FlagSubCategory == "yes"))
                                        {
                                            //if (validcount != 6)
                                            //{
                                            //    errorexist = 1;
                                            //}
                                            //change on 16 - 04 - 2025(Added 1 more New Column in Excel DisplaySortorder)
                                            if (validcount != 7)
                                            {
                                                errorexist = 1;
                                            }
                                        }

                                    }
                                    // End code on 12-March-2020.

                                    if (errorexist == 1)
                                    {
                                        //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                                        //art1.Style.Add("display", "block");
                                        //alertlabel.Text = "<strong>Alert ! </strong>Columns do not match the columns of the Template.";
                                        return Json(new { Result = "Failed", Message = "Columns do not match the columns of the Template." });
                                    }
                                    else
                                    {
                                        dtExcelData.Columns.Add("Clientid", typeof(System.Int32));
                                        dtExcelData.Columns.Add("UserId", typeof(System.Int32));

                                        DataRow dr = null;
                                        for (int i = 2; i <= totalRows; i++)
                                        {
                                            dr = dtExcelData.NewRow();
                                            bool flagdataexist = false;
                                            for (int j = 1; j <= totalCols; j++)
                                            {
                                                if (worksheet.Cells[i, j].Value != null)
                                                {
                                                    dr[j - 1] = worksheet.Cells[i, j].Value.ToString().Trim();
                                                    flagdataexist = true;
                                                }
                                                else
                                                {
                                                    dr[j - 1] = null;
                                                }
                                            }

                                            if (flagdataexist == true)
                                            {
                                                dtExcelData.Rows.Add(dr);
                                            }
                                            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                                            {
                                                foreach (DataRow row in dtExcelData.Rows)
                                                {
                                                    //row["Clientid"] = databaseCon.Decrypt(Request.QueryString["Id"]);
                                                    //row["UserId"] = databaseCon.Decrypt(Request.QueryString["UID"]);
                                                    row["Clientid"] = databaseCon.Decrypt(ClientID);
                                                    row["UserId"] = databaseCon.Decrypt(UID);
                                                }
                                                sqlBulkCopy.DestinationTableName = "tblTempInventoryDataUpload";
                                                // sqlBulkCopy.ColumnMappings.Add("InventoryID", "InventoryID");
                                                sqlBulkCopy.ColumnMappings.Add("MaterialType", "MaterialType");
                                                sqlBulkCopy.ColumnMappings.Add("ItemCode", "ItemCode");
                                                sqlBulkCopy.ColumnMappings.Add("ItemDesc", "ItemDesc");
                                                //sqlBulkCopy.ColumnMappings.Add("OrgYardQuantity", "OrgYardQuantity");
                                                sqlBulkCopy.ColumnMappings.Add("ItemWeight", "ItemWeight");
                                                sqlBulkCopy.ColumnMappings.Add("clientid", "clientid");
                                                sqlBulkCopy.ColumnMappings.Add("UserId", "UserId");
                                                sqlBulkCopy.ColumnMappings.Add("LabourErect", "LabourErect");
                                                sqlBulkCopy.ColumnMappings.Add("LabourDismantle", "LabourDismantle");
                                                sqlBulkCopy.ColumnMappings.Add("DisplaySortOrder", "DisplaySortOrder");


                                                if (FlagQuantity == "yes")
                                                {
                                                    sqlBulkCopy.ColumnMappings.Add("Quantity", "OrgYardQuantity");
                                                }
                                                // Add code on 12-March-2020.
                                                if (FlagSubCategory == "yes" && FlagSubCategory != "")
                                                {
                                                    sqlBulkCopy.ColumnMappings.Add("SubCategory", "SubCategory");// add code on 07 March 2020.
                                                }

                                                con.Open();
                                                sqlBulkCopy.WriteToServer(dtExcelData);
                                                con.Close();
                                                dtExcelData.Clear();
                                            }
                                        }

                                        //    DataTable dt = databaseCon.GetData("Pro_ImportInventory " + databaseCon.Decrypt(Request.QueryString["Id"]) + "," + databaseCon.Decrypt(Request.QueryString["UID"]));

                                        //   DataTable errordt = databaseCon.GetData("select * from tblTempInventoryDataUpload where Comments is not null and Comments!='' and UserId='" + databaseCon.Decrypt(Request.QueryString["UID"]) + "'");

                                        if (IsSort)
                                        {
                                            string query = "  select  count('X')  as InventoryStatus FROM tblTempInventoryDataUpload WHERE OrgYardQuantity > 0 and  clientid = " + databaseCon.Decrypt(ClientID) + " and UserId = " + databaseCon.Decrypt(UID) + "";
                                            DataTable dt1 = databaseCon.GetData(query);
                                            var InventoryStatus = dt1.Rows[0]["InventoryStatus"];
                                            if (Convert.ToInt32(InventoryStatus) > 0)
                                            {
                                                return Json(new { Result = "Failed", Message = "quantityZero" });
                                            }

                                        }

                                        string x = "Pro_ImportInventory " + databaseCon.Decrypt(ClientID) + "," + databaseCon.Decrypt(UID) + "," + IsSort;

                                        DataTable dt = databaseCon.GetData(x);

                                        //DataTable errordt = databaseCon.GetData("select * from tblTempInventoryDataUpload where Comments is not null and Comments!='' and UserId='" + databaseCon.Decrypt(UID) + "'");
                                        DataTable errordt = databaseCon.GetData("select 'X' from tblTempInventoryDataUpload where Comments is not null and Comments!='' and UserId='" + databaseCon.Decrypt(UID) + "'");

                                        if (errordt.Rows.Count > 0)
                                        {
                                            return Json(new { Result = "Failed", Message = "ErrorGrid" });
                                            //   hdnerror.Value = "true";

                                            //  btndelete.Visible = false;
                                        }
                                        else
                                        {
                                            //  hdnerror.Value = "false";


                                            if (dt.Rows.Count != 0)
                                            {
                                                if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                                                {
                                                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                                                    //art1.Style.Add("display", "block");
                                                    //alertlabel.Text = "<strong>Success ! </strong>" + dt.Rows[0][1].ToString();
                                                    //gridinventory.Rebind();
                                                    return Json(new { Result = "Success", Message = dt.Rows[0][1].ToString() });
                                                }
                                                else if (Convert.ToInt32(dt.Rows[0][0]) == -1)
                                                {
                                                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                                                    //art1.Style.Add("display", "block");
                                                    //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                                                    return Json(new { Result = "Failed", Message = dt.Rows[0][1].ToString() });
                                                }
                                                else if (Convert.ToInt32(dt.Rows[0][0]) == -2)
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
                                                return Json(new { Result = "Failed", Message = "Syntax Error. Contact administrator" });
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                return Json(new { Result = "Failed", Message = ex.Message });
                            }
                        }
                        else
                        {
                            return Json(new { Result = "Failed", Message = "File must be xlsx." });
                        }
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { Result = "Failed", Message = "Error occurred. Error details: " + ex.Message });
                }
            }
            else
            {
                return Json(new { Result = "Failed", Message = "No files selected." });
            }

            return RedirectToAction("Login", "Login");

        }

        public class ErrorData
        {
            public string InventoryID;
            public string Comments;
            public string MaterialType;
            public string ItemCode;
            public string ItemDesc;
            public string ItemWeight;
            public string OrgYardQuantity;
            public string LabourErect;
            public string LabourDismantle;
            public string SubCategory;// add on 09-March-2020

        }

        List<ErrorData> ErrData = new List<ErrorData> { };
        // [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        // [WebMethod(EnableSession = true)]
        public ActionResult geterrorData(string id, string UID)
        {
            //string query = "select InventoryId, MaterialType, ItemCode,ItemDesc,ItemWeight,OrgYardQuantity,LabourErect, LabourDismantle, Comments from tblTempInventoryDataUpload where Comments is not null and clientid='" + id + "' and UserId=" + UID;

            // Change Query on 09-March-2020
            string query = "select InventoryId, Comments,MaterialType, ItemCode,ItemDesc,ItemWeight,OrgYardQuantity,LabourErect, LabourDismantle, SubCategory from tblTempInventoryDataUpload where Comments is not null and clientid='" + databaseCon.Decrypt(id) + "' and UserId=" + databaseCon.Decrypt(UID);
            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ErrorData inv = new ErrorData();
                    if (!(item["InventoryId"] is DBNull) && item["InventoryId"].ToString() != "")
                    {
                        inv.InventoryID = item["InventoryId"].ToString();
                    }
                    if (!(item["Comments"] is DBNull) && item["Comments"].ToString() != "")
                    {
                        inv.Comments = item["Comments"].ToString();
                    }
                    if (!(item["MaterialType"] is DBNull) && item["MaterialType"].ToString() != "")
                    {
                        inv.MaterialType = item["MaterialType"].ToString();
                    }

                    if (!(item["ItemCode"] is DBNull) && item["ItemCode"].ToString() != "")
                    {
                        inv.ItemCode = item["ItemCode"].ToString();
                    }
                    if (!(item["ItemDesc"] is DBNull) && item["ItemDesc"].ToString() != "")
                    {
                        inv.ItemDesc = item["ItemDesc"].ToString();
                    }
                    if (!(item["LabourErect"] is DBNull) && item["LabourErect"].ToString() != "")
                    {
                        inv.LabourErect = item["LabourErect"].ToString();
                    }
                    if (!(item["LabourDismantle"] is DBNull) && item["LabourDismantle"].ToString() != "")
                    {
                        inv.LabourDismantle = item["LabourDismantle"].ToString();
                    }
                    if (!(item["ItemWeight"] is DBNull) && item["ItemWeight"].ToString() != "")
                    {
                        inv.ItemWeight = item["ItemWeight"].ToString();
                    }
                    if (!(item["OrgYardQuantity"] is DBNull) && item["OrgYardQuantity"].ToString() != "")
                    {
                        inv.OrgYardQuantity = item["OrgYardQuantity"].ToString();
                    }

                    if (!(item["SubCategory"] is DBNull) && item["SubCategory"].ToString() != "")// add on 09-March-2020
                    {
                        inv.SubCategory = item["SubCategory"].ToString();
                    }
                    ErrData.Add(inv);
                }
            }
            //  return ErrData;
            return Json(ErrData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PipeInventoryHide(string id)
        {
            // string query = "Select * from tblpipelineRequest where Clientid='" + databaseCon.Decrypt(id) + "'";
            string query = "select DISTINCT  PipeLineInvHide from tblSiteLocation where Clientid='" + databaseCon.Decrypt(id) + "' and PipeLineInvHide is not null and PipeLineInvHide != 0";
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                return Json(new { result = "success", Message = "Available Quantity" });
            }
            else
            {
                return Json(new { result = "Failed", Message = "Not Quantity" });
            }
        }
        public ActionResult GetTempleteFile(string TempleteFile, string UID, string ClientID)
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);

            bool HireInventory = false;

            string Hirequery = "select isnull(HireInventory,0) as HireInventory from tblClient where clientid='" + (databaseCon.Decrypt(ClientID)) + "'";
            DataTable dtHireInventory = databaseCon.GetData(Hirequery);
            if (dtHireInventory.Rows.Count > 0)
            {
                for (int i = 0; i < dtHireInventory.Rows.Count; i++)
                {
                    if (dtHireInventory.Rows[i]["HireInventory"].ToString() == "True")
                    {
                        HireInventory = true;

                    }
                }
            }
            if (HireInventory == true)
            {
                TempleteFile = "Template_InventoryManagement_With_SubCategory.xlsx";
            }
            else
            {
                TempleteFile = "Template_InventoryManagement.xlsx";
            }
            var FilePath = Server.MapPath("~/uploads/Template/" + TempleteFile);

            byte[] fileBytes;
            using (var package = new ExcelPackage(new FileInfo(FilePath)))
            {
                foreach (var worksheet in package.Workbook.Worksheets)
                {
                    if (worksheet.Dimension != null)
                    {
                        int colStart = worksheet.Dimension.Start.Column;
                        int colEnd = worksheet.Dimension.End.Column;
                        int rowStart = worksheet.Dimension.Start.Row;
                        int rowEnd = worksheet.Dimension.End.Row;

                        for (int col = colStart; col <= colEnd; col++)
                        {
                            string headerText = worksheet.Cells[1, col].Text;
                            if (!string.IsNullOrEmpty(headerText))
                            {
                                int length = headerText.Length;
                                worksheet.Column(col).Width = length + 5;
                            }
                        }
                    }
                }
                fileBytes = package.GetAsByteArray();
            }

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, TempleteFile);

        }
        public ActionResult DdlMaterialType(string id)
        {
            //Old Query 03-09-2025 
            //var getMaterialType = dbContext.Database.SqlQuery<InventoryViewModel>("select distinct MaterialType from tblinventory join tblTransactionHistDelivery on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId  where tblinventory.MaterialType <> '' and tblinventory.MaterialType is not null and tblTransactionHistDelivery.[To] = 'site' and tblTransactionHistDelivery.DeliveryStatus = 'Confirm' and tblinventory.ClientId = '" + databaseCon.Decrypt(id) + "' order by MaterialType").ToList();

            var getMaterialType = dbContext.Database.SqlQuery<InventoryViewModel>("select distinct MaterialType from tblinventory where ClientID = '" + databaseCon.Decrypt(id) + "' and MaterialType <> '' and MaterialType is not null order by MaterialType").ToList();

            return Json(getMaterialType, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTotalTotalTonnage(string ClientID, string MaterialType)
        {
            var TotalItemsValues = "";
            var TotalTonnageValues = "";

            string query = "select FORMAT(CONVERT(bigint,(Sum(AvailableQuantity))),'N0') as TotalItems,convert(decimal(18,2),Sum(TotalWeight)/1000) as TotalTonnage from Vw_ThirdPartyInventory  where ClientID = " + databaseCon.Decrypt(ClientID) + " and ('All' = '" + MaterialType.Replace("'", "''").Trim() + "' or MaterialType = '" + MaterialType.Replace("'", "''").Trim() + "')";
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                TotalItemsValues = dt.Rows[0]["TotalItems"].ToString();
                //TotalTonnageValues = Convert.ToDecimal(dt.Rows[0]["TotalTonnage"]).ToString("N2");
                TotalTonnageValues = dt.Rows[0]["TotalTonnage"] == DBNull.Value ||
                    String.IsNullOrEmpty(dt.Rows[0]["TotalTonnage"]?.ToString())
                    ? "0.00"
                    : Convert.ToDecimal(dt.Rows[0]["TotalTonnage"]).ToString("N2");
            }

            var Result = "TotalItemsValues" + TotalItemsValues + "|" + "TotalTonnageValues" + TotalTonnageValues;
            return Json(new { TotalItemsValues = TotalItemsValues, TotalTonnageValues = TotalTonnageValues });
        }
    }
}