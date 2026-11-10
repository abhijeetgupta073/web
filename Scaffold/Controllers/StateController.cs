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
    public class StateController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: State
        public ActionResult State(string UID, string UserRole, string id)
        {
            return View();
        }

        public ActionResult GetStateData(string ddlCountry)
        {
            string query = "";
            query = "exec [Pro_TBLSTATE_GET] '','" + ddlCountry + "'";
            var getData = dbContext.Database.SqlQuery<CountryViewModel>(query).ToList();
            return Json(getData, JsonRequestBehavior.AllowGet);

        }

        public JsonResult countryDropDown()
        {
            string query = "select distinct(Country) from tblState where Country !=''";
            var getData = dbContext.Database.SqlQuery<string>(query).ToList();
            return Json(getData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveStateCountry(string StateId, string state, string ddlCountry, string id, string UID)
        {
            string query = "";
            DataTable dt;
            string Result = "";
            string Message = "";
         
            if (StateId == "0")
            {
                query = "exec Pro_TBLSTATE_SAVE '0','" + state.Replace("'", "''").Trim() + "','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "','" + ddlCountry + "'";
                dt = databaseCon.GetData(query);
                if (dt.Rows.Count != 0)
                {
                    if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                    {
                        //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Success ! </strong>" + dt.Rows[0][1].ToString();
                        //gridState.Rebind();
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
            else
            {
                query = "exec Pro_TBLSTATE_SAVE '" + StateId + "','" + state.Replace("'", "''").Trim() + "','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "','" + ddlCountry + "'";
                dt = databaseCon.GetData(query);
                if (dt.Rows.Count != 0)
                {
                    if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                    {
                        //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Success ! </strong>" + dt.Rows[0][1].ToString();
                        //gridState.Rebind();
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
            
            var response = new { Result = Result, Message = Message };
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        public JsonResult DeleteDocument(string StateId)
        {
            string query = "";
            DataTable dt;
            string Result = "";
            string Message = "";
            query = "exec Pro_TBLSTATE_DEL '" + StateId + "'";
            dt = databaseCon.GetData(query);
            if (dt.Rows.Count != 0)
            {
                if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                {
                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Success ! </strong>" + dt.Rows[0][1].ToString();
                    //gridState.Rebind();
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

            var response = new { Result = Result, Message = Message };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

    }

    public class CountryViewModel 
    {
        public Nullable<int> StateId { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }

}