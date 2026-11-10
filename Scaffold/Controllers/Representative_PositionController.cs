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
    public class Representative_PositionController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: Representative_Position
        public ActionResult Representative_Position(string UID, string UserRole, string id)
        {
            return View();
        }

        public ActionResult GetRepresentattivePositionData(string id)
        {
            string query = "";
            query = "exec [Pro_TblClientRepresentation_GET] ''," + databaseCon.Decrypt(id);
            var getData = dbContext.Database.SqlQuery<RepresentativePositionViewModel>(query).ToList();
            return Json(getData, JsonRequestBehavior.AllowGet);

        }

        public ActionResult RepresentativeSaveData(string RepresentativeId, string Position, string Status, string id, string UID)
        {

            string query = "";
            DataTable dt;
            string Result = "";
            string Message = "";
            if (RepresentativeId == "0")
            {
                query = "exec Pro_TblClientRepresentation_SAVE '0','" + databaseCon.Decrypt(id) + "','" + Position.Replace("'", "''").Trim() + "','" + Status.Replace("'", "''").Trim() + "','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "'";
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
                query = "exec Pro_TblClientRepresentation_SAVE '" + RepresentativeId + "','" + databaseCon.Decrypt(id) + "','" + Position.Replace("'", "''").Trim() + "','" + Status.Replace("'", "''").Trim() + "','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "'";

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


        public JsonResult DeleteDocument(string RepresantiveId)
        {
            string query = "";
            string Result = "";
            string Message = "";
            DataTable dt;
            query = "exec Pro_TblClientRepresentation_DEL '" + RepresantiveId + "'";
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

    public class RepresentativePositionViewModel 
    {
        public Nullable<int> ID { get; set; }
        public string Position { get; set; }
        public string Status { get; set; }
       
    }

}