using Scaffold.App_Code;
using Scaffold_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scaffold.Controllers
{
    public class SubCategoriesController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: SubCategories
        public ActionResult SubCategories(string UID, string UserRole, string id)
        {
            return View();
        }

        public ActionResult GetSubCategoryData(string id)
        {
            string query = "";
            //query = "select * from tblSubCategory where ClientID='" + databaseCon.Decrypt(id) + "'";
            query = "select SubCategoryID, Category, SubCategory, HireMeasurement, HireRate from tblSubCategory where ClientID='" + databaseCon.Decrypt(id) + "'";
            var getData = dbContext.Database.SqlQuery<SubCategoryViewModel>(query).ToList();
            return Json(getData, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ddlSubCategory(string id)
        {
            string query = "";
            //query = "select distinct Category from tblsubCategory where ClientId = '" + databaseCon.Decrypt(id) + "' and Category is not null and Category!= '' Order by Category;";
            
            query = "select distinct MaterialType from tblMaterialType where ClientID = '" + databaseCon.Decrypt(id) + "' Order by MaterialType;";
            var getData = dbContext.Database.SqlQuery<string>(query).ToList();
            return Json(getData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveSubCategory(string subcategoryId, string Category, string SubCategory, string HireMeasurement, string hireRate, string id, string UID)
        {

            string query = "";
            DataTable dt;
            string Result = "";
            string Message = "";
            int i;
            if (hireRate != null)
            {
                var Array = hireRate.Split('$');
                if (Array.Length > 1)
                {
                    hireRate = Array[1];
                }
                else
                {
                    hireRate = Array[0];
                }

            }


            //query = "select Count(*) as SubCategory  from tblSubCategory where ClientId='" + databaseCon.Decrypt(id) + "' and SubCategory='" + SubCategory.Replace("'", "''").Trim() + "'";
            //26-03-2025 by ss added Category check for edit and add 
            // query = "select Count(*) as SubCategory  from tblSubCategory where ClientId='" + databaseCon.Decrypt(id) + "' and SubCategory='" + SubCategory.Replace("'", "''").Trim() + "'and Category = '" + Category.Replace("'", "''").Trim() + "'";
            query = "select  Count(1) as SubCategory from tblSubCategory where ClientId='" + databaseCon.Decrypt(id) + "' and SubCategory='" + SubCategory.Replace("'", "''").Trim() + "' and Category = '" + Category.Replace("'", "''").Trim() + "'";
            dt = databaseCon.GetData(query);
            if (subcategoryId == "0")
            {

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["SubCategory"].ToString() == "0")
                    {
                        query = "insert into tblSubCategory (Category,SubCategory,HireMeasurement,HireRate,ClientId,CreatedBy,CreatedOn) values" +
                     "('" + Category.Replace("'", "''").Trim() + "','" + SubCategory.Replace("'", "''").Trim() + "','" + HireMeasurement.Replace("'", "''").Trim() + "','" + (hireRate == "" ? null : hireRate) + "','" + databaseCon.Decrypt(id) +
                     "',(select EmailId from tblUser where UserId='" + databaseCon.Decrypt(UID) + "'),'" + DateTime.Now + "')";

                        i = databaseCon.RunCmd(query);
                        if (i > 0)
                        {
                            //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                            //art1.Style.Add("display", "block");
                            //alertlabel.Text = "<strong>Success ! </strong> Record Save Successfully ";
                            Result = "Success";
                            Message = "Record Save Successfully";

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
                    else
                    {
                        //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Alert ! </strong>Record Already Exist For " + SubCategory.Text;
                        Result = "Failed";
                        Message = "Record Already Exist For " + SubCategory;
                    }
                }


            }
            else
            {
                //string SubID = item.GetDataKeyValue("SubCategoryID").ToString();
                string query1 = "";
                //query = "select Count(*) as SubCategory from tblSubCategory where ClientId='" + databaseCon.Decrypt(id) + "' and SubCategory='" + SubCategory.Replace("'", "''").Trim() + "' and SubCategoryID!='" + subcategoryId.Replace("'", "''").Trim() + "'";

                //26-03-2025 by ss added Category check for edit and add 

              //  query = "select Count(*) as SubCategory from tblSubCategory where ClientId='" + databaseCon.Decrypt(id) + "' and SubCategory='" + SubCategory.Replace("'", "''").Trim() + "' and SubCategoryID!='" + subcategoryId.Replace("'", "''").Trim() + "'  and Category = '" + Category.Replace("'", "''").Trim() + "'";
                query = "select Count(1) as SubCategory from tblSubCategory where ClientId='" + databaseCon.Decrypt(id) + "' and SubCategory='" + SubCategory.Replace("'", "''").Trim() + "' and SubCategoryID!='" + subcategoryId.Replace("'", "''").Trim() + "'  and Category = '" + Category.Replace("'", "''").Trim() + "'";
                dt = databaseCon.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["SubCategory"].ToString() == "0")
                    {
                        query1 = "update tblSubCategory set Category='" + Category.Replace("'", "''").Trim() + "', SubCategory='" + SubCategory.Replace("'", "''").Trim() + "',  HireMeasurement='" + HireMeasurement.Replace("'", "''").Trim() + "',HireRate='" + (hireRate == "" ? null : hireRate) + "',UpdatedBY =(select EmailId from tblUser where UserId='" + databaseCon.Decrypt(UID) + "'),UpdatedOn ='" + DateTime.Now + "' where  ClientId='" + databaseCon.Decrypt(id) + "' and SubCategoryID='" + subcategoryId.Replace("'", "''").Trim() + "'";
                        i = databaseCon.RunCmd(query1);
                        if (i > 0)
                        {
                            //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                            //art1.Style.Add("display", "block");
                            //alertlabel.Text = "<strong>Success ! </strong> Record Save Successfully ";
                            Result = "Success";
                            Message = "Record Save Successfully";
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
                    else
                    {
                        //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Alert ! </strong>Record Already Exsist For "+ SubCategory.Text;
                        Result = "Failed";
                        Message = "Record Already Exsist For " + SubCategory;
                    }
                }
            }
            var response = new { Result = Result, Message = Message };
            return Json(response, JsonRequestBehavior.AllowGet);

        }


        public JsonResult DeleteDocument(string SubCategoryId, string id)
        {
            string query = "";
            int i;
            string Result = "";
            string Message = "";
           // string SubCategoryID = ViewState["SubCategoryID"].ToString();

            //query = "select COUNT(*)as InventoryCount from tblInventory  where ClientID='" + databaseCon.Decrypt(id) + "' and SubCategoryID='" + SubCategoryId + "'";
            query = "select Count(1) as InventoryCount from tblInventory  where ClientID='" + databaseCon.Decrypt(id) + "' and SubCategoryID='" + SubCategoryId + "'";
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["InventoryCount"].ToString() == "0")
                {
                    query = "delete from tblSubCategory where  ClientId='" + databaseCon.Decrypt(id) + "' and SubCategoryID='" + SubCategoryId.Replace("'", "''").Trim() + "' ";

                    i = databaseCon.RunCmd(query);
                    if (i > 0)

                    {
                        //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Success ! </strong> Record Deleted Successfully ";
                        //gridSubCategory.Rebind();
                        Result = "Success";
                        Message = "Record Deleted Successfully ";
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
                else
                {
                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! This SubCategory is already in use you cannot delete it</strong> ";
                    Result = "Failed";
                    Message = "This SubCategory is already in use you cannot delete it";
                }
            }

           // divdelete.Style.Add("display", "none");

            var response = new { Result = Result, Message = Message };
            return Json(response, JsonRequestBehavior.AllowGet);
        }


    }

    public class SubCategoryViewModel 
    {
        public Nullable<int> SubCategoryID { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string HireMeasurement { get; set; }
        public Nullable<Decimal> HireRate { get; set; }

    }

}