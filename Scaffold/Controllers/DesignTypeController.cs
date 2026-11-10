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
    public class DesignTypeController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: DesignType
        public ActionResult DesignType(string UID, string UserRole, string id)
        {
            ViewBag.id = id;
            return View();
        }

        public ActionResult GetDesignTypeData(string id) 
        {
            string query = "";
            query = "exec [Pro_TBLDESIGN_GET] ''," + databaseCon.Decrypt(id);
            var getData = dbContext.Database.SqlQuery<DesignTypeViewMode>(query).ToList();
            return Json(getData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteDesignType(string DesignTypeId)
        {

            string query = "";
            DataTable dt;
            var Result = "";
            var Message = "";
            query = "exec Pro_TBLDESIGN_DEL'" + DesignTypeId + "'";
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
                Message = "Syntax Error. Contact administrator ";
            }

            return Json(new {Result= Result, Message = Message }, JsonRequestBehavior.AllowGet);
        
        }


        public ActionResult DesignTypeSaveData(string desigyTypeId, string DesignType, string DifficultyRating, string Status, string id, string UID)
        {

            string query = "";
            DataTable dt;
            string Result = "";
            string Message = "";
            if (desigyTypeId == "0")
            {
                query = "exec Pro_TBLDESIGN_SAVE '0','" + DesignType.Replace("'", "''").Trim() + "','" + databaseCon.Decrypt(id) + "','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "','" + Status.Replace("'", "''").Trim() + "','" + DifficultyRating.Replace("'", "''").Trim() + "'";
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
                query = "exec Pro_TBLDESIGN_SAVE '" + desigyTypeId + "','" + DesignType.Replace("'", "''").Trim() + "','" + databaseCon.Decrypt(id) + "','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "','" + Status.Replace("'", "''").Trim() + "','" + DifficultyRating.Replace("'", "''").Trim() + "'";
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


    }


    public class DesignTypeViewMode 
    {
        public Nullable<int> DesignId { get; set; }
        public string DesignType { get; set; }
        public Nullable<int> ClientID { get; set; }
        public string DesignStatus { get; set; }
        public Nullable<int> DifficultyRating { get; set; }
    }

}