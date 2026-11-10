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
    public class UserCategoryController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: UserCategory
        public ActionResult UserCategory(string UID, string UserRole, string id)
        {
            return View();
        }

        public ActionResult GetUserCategoryData(string id)
        {
            string query = "";
            query = "exec [Pro_TblUserCategory_GET] ''," + databaseCon.Decrypt(id);
            var getData = dbContext.Database.SqlQuery<UserCategoryViewModel>(query).ToList();
            return Json(getData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UserCategorySaveData(string UserCategoryId, string Category, string DirectIndirect, string CategoryStatus, string id, string UID)
        {
                string query = "";
                DataTable dt;
                string Result = "";
                string Message = "";
                if (UserCategoryId == "0")
                {
                query = "exec Pro_TblUserCategory_SAVE '0','" + databaseCon.Decrypt(id) + "','" + Category.Replace("'", "''").Trim() + "','" + DirectIndirect.Replace("'", "''").Trim() + "','" + CategoryStatus.Replace("'", "''").Trim() + "','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "'";

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
                        Message = "Syntax Error.Contact administrator ";
                    }
                }
                else
                {
                query = "exec Pro_TblUserCategory_SAVE '" + UserCategoryId + "','" + databaseCon.Decrypt(id) + "','" + Category.Replace("'", "''").Trim() + "','" + DirectIndirect.Replace("'", "''").Trim() + "','" + CategoryStatus.Replace("'", "''").Trim() + "','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "'";
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
                        {   Result = "Failed";
                            Message = dt.Rows[0][1].ToString();
                        }
                    }
                    else
                    {   
                        Result = "Failed";
                        Message = "Syntax Error.Contact administrator ";
                    }

                }
                var response = new { Result = Result, Message = Message };
                return Json(response, JsonRequestBehavior.AllowGet);

            }


        public JsonResult DeleteDocument(string UserCategoryId)
        {
            string query = "";
            string Result = "";
            string Message = "";
            DataTable dt;
            query = "exec Pro_TblUserCategory_DEL '" + UserCategoryId + "'";
            dt = databaseCon.GetData(query);
            if (dt.Rows.Count != 0)
            {
                if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                {
                    Result = "Success";
                    Message = dt.Rows[0][1].ToString();
                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Success ! </strong>" + dt.Rows[0][1].ToString();
                    //gridMaterialType.Rebind();
                }
                else if (Convert.ToInt32(dt.Rows[0][0]) == -1)
                {
                    Result = "Failed";
                    Message = dt.Rows[0][1].ToString();
                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();

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

    public class UserCategoryViewModel
    {
        public Nullable<int> ID { get; set; }
        public Nullable<int> ClientId { get; set; }
        public string Category { get; set; }
        public string DirectIndirect { get; set; }
        public string CategoryStatus { get; set; }
    }

}