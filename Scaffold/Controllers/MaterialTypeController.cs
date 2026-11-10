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
    public class MaterialTypeController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: MaterialType
        public ActionResult MaterialType(string UID, string UserRole, string id)
        {
            return View();
        }

        public ActionResult GetMaterialTypeData(string id)
        {
            string query = "";
            query = "exec [Pro_TBLMATERIALTYPE_GET] ''," + databaseCon.Decrypt(id);
            var getData = dbContext.Database.SqlQuery<MaterialTypeViewModel>(query).ToList();
            return Json(getData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MaterialTypeSaveData(string MaterialId, string MaterialType, string ReorderPct, string Status, string id, string UID)
        {

            string query = "";
            DataTable dt;
            string Result = "";
            string Message = "";
            if (MaterialId == "0")
            {
                query = "exec Pro_TBLMATERIALTYPE_SAVE '0','" + MaterialType.Replace("'", "''").Trim() + "','" + databaseCon.Decrypt(id) + "','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "','" + Status.Replace("'", "''").Trim() + "'," + (ReorderPct == "" ? "null" : "'" + ReorderPct + "'") + "";
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
                        //gridesign.Rebind();
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
                    Message = "Syntax Error.Contact administrator ";
                }
            }
            else
            {
                query = "exec Pro_TBLMATERIALTYPE_SAVE '" + MaterialId + "','" + MaterialType.Replace("'", "''").Trim() + "','" + databaseCon.Decrypt(id) + "','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "','" + Status.Replace("'", "''").Trim() + "'," + (ReorderPct == "" ? "null" : "'" + ReorderPct + "'") + "";
                dt = databaseCon.GetData(query);
                if (dt.Rows.Count != 0)
                {
                    if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                    {
                        //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Success ! </strong>" + dt.Rows[0][1].ToString();
                        //gridesign.Rebind();
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
                    Message = "Syntax Error.Contact administrator ";
                }

            }
            var response = new { Result = Result, Message = Message };
            return Json(response, JsonRequestBehavior.AllowGet);

        }


        public JsonResult DeleteDocument(string MaterialId)
        {
            string query = "";
            string Result = "";
            string Message = "";
            DataTable dt;
            query = "exec Pro_TBLMATERIALTYPE_DEL '" + MaterialId + "'";
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

    public class MaterialTypeViewModel 
    {
        public Nullable<int> MatTypeId { get; set; }
        public string MaterialType { get; set; }
        public Nullable<int> ClientID { get; set; }
        public string MaterialStatus { get; set; }
        public Nullable<Decimal> ReOrderPct { get; set; }
    }

}