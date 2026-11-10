using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scaffold.App_Code;


namespace Scaffold.Controllers
{
    public class CycloneController : Controller
    {
        // GET: Cyclone
        public ActionResult Cyclone()
        {
            return View();
        }

       public ActionResult btnsave_Click(string RID,string txtTotalDeBoardEdit,string chkuseentervalue,string ddlCycloneAction,string ddlPriorityRiskRating,string txtTotalReInstateEdit,string txtreInstateEdit,string txtDeBoardItemEdit,string txtcomments,string ddlRangeOfOutBoard)
        {
            var Result = "";
            var Message = "";
            //  if (txtTotalDeBoardEdit.Text != "" && chkuseentervalue.Checked == true || txtTotalDeBoardEdit.Text == "" && chkuseentervalue.Checked == false)
            if (txtTotalDeBoardEdit != "" && chkuseentervalue == "True" || txtTotalDeBoardEdit == "" && chkuseentervalue == "False")
            {
                //  string query = "exec Pro_TblRegisterCyclone_SAVE '" + databaseCon.Decrypt(RID) + "'," + (ddlCycloneAction.SelectedValue == "" ? "null" : "'" + ddlCycloneAction.SelectedValue + "'") + ",'" + ddlPriorityRiskRating.SelectedValue + "'," + (txtTotalReInstateEdit.Text == "" ? "null" : "'" + txtTotalReInstateEdit.Text + "'") + "," + (txtTotalDeBoardEdit.Text == "" ? "null" : "'" + txtTotalDeBoardEdit.Text + "'") + "," + (txtreInstateEdit.Text == "" ? "null" : "'" + txtreInstateEdit.Text + "'") + "," + (txtDeBoardItemEdit.Text == "" ? "null" : "'" + txtDeBoardItemEdit.Text + "'") + ",'" + chkuseentervalue.Checked + "','" + txtcomments.Text.Replace("'", "''").Trim() + "'";

                string query = "exec Pro_TblRegisterCyclone_SAVE '" + databaseCon.Decrypt(RID) + "'," + (ddlCycloneAction == "" ? "null" : "'" + ddlCycloneAction + "'") + ",'" + ddlPriorityRiskRating + "'," + (txtTotalReInstateEdit == "" ? "null" : "'" + txtTotalReInstateEdit + "'") + "," + (txtTotalDeBoardEdit == "" ? "null" : "'" + txtTotalDeBoardEdit + "'") + "," + (txtreInstateEdit == "" ? "null" : "'" + txtreInstateEdit + "'") + "," + (txtDeBoardItemEdit == "" ? "null" : "'" + txtDeBoardItemEdit + "'") + ",'" + chkuseentervalue + "','" + txtcomments.Replace("'", "''").Trim() + "',"+ ddlRangeOfOutBoard + "";
                DataTable dt = databaseCon.GetData(query);
                if (dt.Rows.Count != 0)
                {
                    if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                    {
                        int RegisterID = Convert.ToInt32(dt.Rows[0][0]);
                       // ShowregisterDetails();
                        //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Success ! </strong>" + dt.Rows[0][1];

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
                    Message = "Syntax Error. Contact administrator ";
                }
            }
            else
            {
                if (txtTotalDeBoardEdit == "" && chkuseentervalue== "True")
                {
                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong>You have checked the Box for <strong>Use Entered Value (For De-Plank Only)</strong>. Please fill <strong>Entered Value </br>Preparation Hrs</strong>.";
                    Result = "Failed";
                    Message = "You have checked the Box for <strong>Use Entered Value (For De-Plank Only)</strong>. Please fill <strong>Entered Value </br>Preparation Hrs";
                }
                else
                {
                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong>You have Entered Preparation Hrs Please select the <strong>Use Entered Value (For De-Plank Only)</strong>.";
                    Result = "Failed";
                    Message = "You have Entered Preparation Hrs Please select the <strong>Use Entered Value (For De-Plank Only).";
                }
            }
            return Json(new { Result = Result, Message = Message });
        }
    }
}