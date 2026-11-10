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
    public class TermsAndConditionTypeController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: TermsAndConditionType
        public ActionResult TermsAndConditionType(string UID, string UserRole, string id)
        {
            return View();
        }

        public ActionResult GetTermsAndConditionData(string radioValue, string id)
        {
            //string query = "";
            //DataTable dt;
            //if (rdoTerms.Checked == true)
            //{
            //    TermsAndCondition.Columns[4].Visible = false;
            //    rdoTerms.Text = "Terms";
            //    query = "exec [Pro_TBLTermsNCondition_GET] ''," + databaseCon.Decrypt(Request.QueryString["id"]) + ",'" + rdoTerms.Text + "'";
            //}
            //if (rdoPolicy.Checked == true)
            //{
            //    rdoPolicy.Text = "Policy";
            //    TermsAndCondition.Columns[4].Visible = true;
            //    query = "exec [Pro_TBLTermsNCondition_GET] ''," + databaseCon.Decrypt(Request.QueryString["id"]) + ",'" + rdoPolicy.Text + "'";
            //    rdoTerms.Checked = false;
            //}
            //dt = databaseCon.GetData(query);
            //TermsAndCondition.DataSource = dt;

            string query = "";
            DataTable dt;
            if (radioValue == "Terms")
            {
                //TermsAndCondition.Columns[4].Visible = false;
               /// rdoTerms.Text = "Terms";
                query = "exec [Pro_TBLTermsNCondition_GET] ''," + databaseCon.Decrypt(id) + ",'" + radioValue + "'";
            }
            if (radioValue == "Policy")
            {
               // rdoPolicy.Text = "Policy";
               // TermsAndCondition.Columns[4].Visible = true;
                query = "exec [Pro_TBLTermsNCondition_GET] ''," + databaseCon.Decrypt(id) + ",'" + radioValue + "'";
               // rdoTerms.Checked = false;
            }
            //dt = databaseCon.GetData(query);
            //TermsAndCondition.DataSource = dt;
            var getData = dbContext.Database.SqlQuery<TermAndConditionViewModel>(query).ToList();
            var response = new { getData = getData, radioValue = radioValue };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveTermAndCondition(string termsAndConditionId, string Order, string mainPoint, string text, string subPoint,string chkradioValue, string id, string UID)
        {
            string query = "";
            string Result = "";
            string Message = "";
            DataTable dt;
            if (termsAndConditionId == "0")
            {
                //GridEditableItem item = (GridEditableItem)e.Item;
                //RadTextBox txtMainPoint = (RadTextBox)item.FindControl("txtMainPoint");
                //RadTextBox txtSubpoint = (RadTextBox)item.FindControl("txtSubpoint");
                //RadNumericTextBox txtorder = (RadNumericTextBox)item.FindControl("txtorder");
                //RadTextBox txttext = (RadTextBox)item.FindControl("txttext");
                if (chkradioValue == "Terms")
                {
                    query = "exec Pro_TBLTermsNCondition_SAVE '0','" + mainPoint.Replace("'", "''").Trim() + "','" + subPoint.Replace("'", "''").Trim() + "','" + Order.Replace("'", "''").Trim() + "','" + databaseCon.Decrypt(id) + "','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "','" + text.Replace("'", "''").Trim() + "','Terms'";
                }
                if (chkradioValue == "Policy")
                {
                    query = "exec Pro_TBLTermsNCondition_SAVE '0','" + mainPoint.Replace("'", "''").Trim() + "','" + subPoint.Replace("'", "''").Trim() + "','" + Order.Replace("'", "''").Trim() + "','" + databaseCon.Decrypt(id) + "','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "','" + text.Replace("'", "''").Trim() + "','Policy'";
                }
                dt = databaseCon.GetData(query);
                if (dt.Rows.Count != 0)
                {
                    if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                    {
                        //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Success ! </strong>" + dt.Rows[0][1].ToString();
                        //TermsAndCondition.Rebind();
                        Result = "Success";
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
            else 
            {
                //GridEditableItem item = (GridEditableItem)e.Item;
                //RadTextBox txtMainPoint = (RadTextBox)item.FindControl("txtMainPoint");
                //RadTextBox txtSubpoint = (RadTextBox)item.FindControl("txtSubpoint");
                //RadNumericTextBox txtorder = (RadNumericTextBox)item.FindControl("txtorder");
                //RadTextBox txttext = (RadTextBox)item.FindControl("txttext");
               // string id = item.GetDataKeyValue("Id").ToString();
                if (chkradioValue == "Terms")
                {
                    query = "exec Pro_TBLTermsNCondition_SAVE '" + termsAndConditionId + "','" + mainPoint.Replace("'", "''").Trim() + "','" + subPoint.Replace("'", "''").Trim() + "','" + Order.Replace("'", "''").Trim() + "','" + databaseCon.Decrypt(id) + "','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "','" + text.Replace("'", "''").Trim() + "','Terms'";
                }
                if (chkradioValue == "Policy")
                {
                    query = "exec Pro_TBLTermsNCondition_SAVE '" + termsAndConditionId + "','" + mainPoint.Replace("'", "''").Trim() + "','" + subPoint.Replace("'", "''").Trim() + "','" + Order.Replace("'", "''").Trim() + "','" + databaseCon.Decrypt(id) + "','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "','" + text.Replace("'", "''").Trim() + "','Policy'";
                }

                dt = databaseCon.GetData(query);
                if (dt.Rows.Count != 0)
                {
                    if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                    {
                        //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Success ! </strong>" + dt.Rows[0][1].ToString();
                        //TermsAndCondition.Rebind();
                        Result = "Success";
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
                    Message = "Syntax Error.Contact administrator";
                }
            }
            var response = new { Result = Result, Message = Message };
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DeleteDocument(string TermsAndConditionId)
        {
            string Result = "";
            string Message = "";
            string query = "";
            DataTable dt;
            query = "exec Pro_TBLTermsNCondition_DEL '" + TermsAndConditionId + "'";

            dt = databaseCon.GetData(query);
            if (dt.Rows.Count != 0)
            {
                if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                {
                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Success ! </strong>" + dt.Rows[0][1].ToString();
                    //TermsAndCondition.Rebind();
                    Result = "Success";
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
            var response = new { Result = Result, Message = Message };
            return Json(response, JsonRequestBehavior.AllowGet);

        }


    }

    public class TermAndConditionViewModel 
    {
        public Nullable<int> id { get; set; }
        public string MainPoint { get; set; }
        public string SubPoint { get; set; }
        public Nullable<int> TermOrder { get; set; }
        public string Text { get; set; }
    }

}