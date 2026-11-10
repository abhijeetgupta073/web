using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure.Storage.Blob;
using OfficeOpenXml;
using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;

namespace Scaffold.Controllers
{
    public class InspectionCheckListContentController : Controller
    {
        ScaffoldEntities dbcontext = new ScaffoldEntities();
        SqlCommand cmd = null;
        SqlConnection con = null;
        // GET: InspectionCheckListContent
        public ActionResult InspectionCheckListContent()
        {
            string queryChecked = "Select isnull(InspectionCheckListColor,0) as InspectionCheckListColor from tblClient where clientID='" + databaseCon.Decrypt(Request.QueryString["id"])
                 + "'";
            DataTable dt = databaseCon.GetData(queryChecked);
            if (dt.Rows[0]["InspectionCheckListColor"].ToString() == "True")
            {
                //RadioBtnColorBlack.Checked = true;
                //RadioBtnColorGreenRed.Checked = false;
                ViewBag.InspectionCheckListColor = true;
            }
            if (dt.Rows[0]["InspectionCheckListColor"].ToString() == "False")
            {
                //RadioBtnColorGreenRed.Checked = true;
                //RadioBtnColorBlack.Checked = false;
                ViewBag.InspectionCheckListColor = false;
            }


            string query = "select checklistID from tblClient where clientID='" + databaseCon.Decrypt(Request.QueryString["id"]) + "'";
            DataTable dt1 = databaseCon.GetData(query);
            if (dt1.Rows.Count > 0)
            {
                if (dt1.Rows[0]["checklistID"].ToString() != "" || dt1.Rows[0]["ChecklistId"] == null)
                {
                    // ddlCheckList.SelectedValue = dt.Rows[0]["checklistID"].ToString();
                    ViewBag.ddlCheckListSelectedValues = dt1.Rows[0]["checklistID"].ToString();
                }
                if (Request.QueryString["ChkID"] != null)
                {
                    // ddlCheckList.SelectedValue = databaseCon.Decrypt(Request.QueryString["ChkID"]);
                    ViewBag.ddlCheckListSelectedValues = databaseCon.Decrypt(Request.QueryString["ChkID"]);
                }
            }
            return View();
        }

        //public  InspectionCheckListContent()
        //{
        //  con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
        //}

        public ActionResult RadioButtonBind(string id, string ddlCheckList)
        {
            var CheckListStatus = "";
            string query = "select status as Status from tblchecklist where clientID='" + databaseCon.Decrypt(id) + "' and  checklistID='" + ddlCheckList + "'";
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                CheckListStatus = dt.Rows[0]["Status"].ToString();
                //  if (dt.Rows[0]["Status"].ToString() == "Active")
                //  {
                //      //RadioButtonActive.Checked = true;
                //      //RadioButtonInActive.Checked = false;
                //     
                //}
                //  if (dt.Rows[0]["Status"].ToString() == "InActive")
                //  {
                //      //RadioButtonInActive.Checked = true;
                //      //RadioButtonActive.Checked = false;
                //     
                //  }
            }
            return Json(CheckListStatus, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DropdownBind(string id)
        {
            var ddlCheckList = dbcontext.Database.SqlQuery<DropDownViewModel>("select checklistID,checklistdescription from tblchecklist where clientID='" + databaseCon.Decrypt(id) + "' order by checklistdescription").ToList();
            return Json(ddlCheckList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInspectionCheckListData(string id, string ddlCheckList)
        {

            //RowcountLevel1 = 0;
            //query = "select * from tblChecklistContent where checklistItem is null and ClientID='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and CheckListID='" + ddlCheckList.SelectedValue + "' order by ItemOrder asc";
            //dt = databaseCon.GetData(query);
            //RowcountLevel1 = dt.Rows.Count - 1;
            //GridInspectionCheckList.DataSource = dt;

            //var GetData = dbcontext.Database.SqlQuery<InspectionCheckListContentViewModel>("select * from tblChecklistContent where checklistItem is null and ClientID='" + databaseCon.Decrypt(id) + "' and CheckListID='" + ddlCheckList + "' order by ItemOrder asc").ToList();
            var GetData = dbcontext.Database.SqlQuery<InspectionCheckListContentViewModel>("select ContentID,ClientID,CheckListID,CheckListContent,checklistItem,subChildItem,ItemOrder,CopyFromCheckListItem,CopyFromSubChilditem,CopyFromItemOrder,CopyFromChecklistId,IsNA,CopyFromContentID from tblChecklistContent where checklistItem is null and ClientID='" + databaseCon.Decrypt(id) + "' and CheckListID='" + ddlCheckList + "' order by ItemOrder asc").ToList();
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInspectionCheckListContentData(string id, string ContentID)
        {
            //RowcountLevel2 = 0;
            //GridDataItem gdi = e.DetailTableView.ParentItem;
            //var DetaailContentID = gdi.GetDataKeyValue("ContentID");
            //query = "select ContentID,CheckListContent,CheckListID, checklistItem,subChildItem,ClientID,ItemOrder,IsNA from tblChecklistContent where checklistItem='" + DetaailContentID + "' and subChildItem is null and ClientID='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' order by ItemOrder";
            //dt = databaseCon.GetData(query);
            //RowcountLevel2 = dt.Rows.Count - 1;
            //e.DetailTableView.DataSource = dt;

            var GetData = dbcontext.Database.SqlQuery<InspectionCheckListContentViewModel>("select ContentID,CheckListContent,CheckListID, checklistItem,subChildItem,ClientID,itemOrder,IsNA from tblChecklistContent where checklistItem='" + ContentID + "' and subChildItem is null and ClientID='" + databaseCon.Decrypt(id) + "' order by ItemOrder").ToList();
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }

        public class InspectionCheckListSaveViewModel
        {
            public string CheckListID { get; set; }
            public string ContentID { get; set; }
            public string ParentContentID { get; set; }
            public string CheckListContent { get; set; }
            public string IsNA { get; set; }

        }

        public ActionResult InspectionCheckListSaveData(string id, string UID, string TableValues, List<InspectionCheckListSaveViewModel> InspectionCheckListVal)
        {
            var Result = "";
            var Message = "";
            var query = "";
            try
            {
                //if (e.Item.OwnerTableView.Name == "ViewLevel1")
                if (TableValues == "ViewLevel1")
                {
                    // foreach (GridBatchEditingCommand command in e.Commands)
                    foreach (InspectionCheckListSaveViewModel item in InspectionCheckListVal)
                    {
                        var Array = item;

                        //string CheckListContent = Array.CheckListContent;
                        string CheckListContent = Array.CheckListContent.Replace("'", "''");

                        string ContentID = Array.ContentID;
                        string CheckIsNA = Array.IsNA;
                        string CheckListID = Array.CheckListID;



                        //  if ((command.Type == GridBatchEditingCommandType.Insert))
                        if (ContentID == "0")
                        {

                            // Hashtable newValues = command.NewValues;
                            //string CheckListContent = newValues["CheckListContent"].ToString();
                            //string CheckIsNA = newValues["IsNA"].ToString();
                            //var CheckListID = ddlCheckList.SelectedValue;

                            // Add On 22 Jan 2020 Check When User Add New Header Then Check In Checklist Is Use User Can't Be Add New Record
                            //string querydata = "select * from tblInspectionList where CheckListID='" + CheckListID + "'";
                            string querydata = "select 'X' from tblInspectionList where CheckListID='" + CheckListID + "'";
                            DataTable dt = databaseCon.GetData(querydata);
                            if (dt.Rows.Count == 0)
                            {

                                //                            var isAny = dbcontext.tblInspectionLists
                                //.Where(x => x.CheckListID == int.Parse(CheckListID)).Any();

                                //                            if (!isAny)
                                //                            {
                                //  query = "insert into tblChecklistContent(CheckListContent,itemOrder,ClientID,CheckListID,IsNA,Createdby,CreatedOn) values('" + CheckListContent + "',(select ISNULL( MAX(itemorder)+1,1)  from tblChecklistContent where checklistItem is null and ClientID='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and CheckListID='" + ddlCheckList.SelectedValue + "'),'" + databaseCon.Decrypt(Request.QueryString["id"]) + "','" + ddlCheckList.SelectedValue + "','" + CheckIsNA + "','" + databaseCon.Decrypt(Request.QueryString["UID"]) + "','" + DateTime.Now + "')";

                                // Query Change On 21 Jan 2020 Add Created By
                                //  query = "insert into tblChecklistContent(CheckListContent,itemOrder,ClientID,CheckListID,IsNA,Createdby,CreatedOn) values('" + CheckListContent + "',(select ISNULL( MAX(itemorder)+1,1)  from tblChecklistContent where checklistItem is null and ClientID='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and CheckListID='" + ddlCheckList.SelectedValue + "'),'" + databaseCon.Decrypt(Request.QueryString["id"]) + "','" + ddlCheckList.SelectedValue + "','" + CheckIsNA + "',(select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(Request.QueryString["UID"]) + "'),'" + DateTime.Now + "')";

                                query = "insert into tblChecklistContent(CheckListContent,itemOrder,ClientID,CheckListID,IsNA,Createdby,CreatedOn) values('" + CheckListContent + "',(select ISNULL( MAX(itemorder)+1,1)  from tblChecklistContent where checklistItem is null and ClientID='" + databaseCon.Decrypt(id) + "' and CheckListID='" + CheckListID + "'),'" + databaseCon.Decrypt(id) + "','" + CheckListID + "','" + CheckIsNA + "',(select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'),'" + DateTime.Now + "')";

                                int result = databaseCon.RunCmd(query);
                                if (result > 0)
                                {
                                    //art1.Style.Add("display", "block");
                                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                                    //alertlabel.Text = "<strong> Success ! </strong>Data saved successfully";
                                    Result = "Success";
                                    Message = "Data saved successfully .";
                                }

                            }
                            else
                            {
                                //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                                //art1.Style.Add("display", "block");
                                //alertlabel.Text = "<strong>Alert ! </strong> Record Can't be Added. Checklist is already in use.";
                                Result = "Failed";
                                Message = "Record Can't be Added. Checklist is already in use.";
                            }

                        }
                        // if ((command.Type == GridBatchEditingCommandType.Update))
                        else
                        {
                            //  Hashtable newValues = command.NewValues;
                            // Hashtable oldValues = command.OldValues;
                            //string CheckListContent = newValues["CheckListContent"].ToString();
                            //string ContentID = newValues["ContentID"].ToString();
                            //string CheckIsNA = newValues["IsNA"].ToString();
                            //string CheckListID = newValues["CheckListID"].ToString();

                            //string querydata = "select * from tblInspectionList where CheckListID='" + CheckListID + "' and ContentID='" + ContentID + "'";
                            string querydata = "select 'X' from tblinspectionlist where CheckListID='" + CheckListID + "' and ContentID='" + ContentID + "'";
                            DataTable dt = databaseCon.GetData(querydata);
                            if (dt.Rows.Count == 0)
                            {
                                //                                var isAny = dbcontext.tblInspectionLists
                                //.Where(x => x.CheckListID == int.Parse(CheckListID) &&  // Convert string to int
                                //      (x.ContentID == int.Parse(ContentID))).Any();

                                //                            if (!isAny)
                                //                            {
                                // query = "update tblChecklistContent set CheckListContent='" + CheckListContent + "',UpdatedBy='" + databaseCon.Decrypt(Request.QueryString["UID"]) + "',UpdatedOn='" + DateTime.Now + "',IsNA='" + CheckIsNA + "' where ContentID='" + ContentID + "'";
                                // Query Change On 21 Jan 2020 Add UpdatedBy And UpdatedOn
                                //query = "update tblChecklistContent set CheckListContent='" + CheckListContent + "',UpdatedBy = (select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(Request.QueryString["UID"]) + "'),UpdatedOn='" + DateTime.Now + "',IsNA='" + CheckIsNA + "' where ContentID='" + ContentID + "'";
                                query = "update tblChecklistContent set CheckListContent='" + CheckListContent + "',UpdatedBy = (select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'),UpdatedOn='" + DateTime.Now + "',IsNA='" + CheckIsNA + "' where ContentID='" + ContentID + "'";

                                int result = databaseCon.RunCmd(query);
                                if (result > 0)
                                {
                                    //art1.Style.Add("display", "block");
                                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                                    //alertlabel.Text = "<strong> Success ! </strong>Data Saved Successfully";
                                    Result = "Success";
                                    Message = "Data Saved Successfully .";
                                }
                            }
                            else
                            {
                                //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                                //art1.Style.Add("display", "block");
                                //alertlabel.Text = "<strong>Alert ! </strong> Can't be Changed. It is already in use.";
                                Result = "Failed";
                                Message = "Can't be Changed. It is already in use.";
                            }

                        }



                    }
                }
                // if (e.Item.OwnerTableView.Name == "ViewLevel2")
                if (TableValues == "ViewLevel2")
                {
                    // foreach (GridBatchEditingCommand command in e.Commands)
                    foreach (InspectionCheckListSaveViewModel item in InspectionCheckListVal)
                    {
                        var Array = item;

                        //string CheckListContent = Array.CheckListContent;
                        string CheckListContent = Array.CheckListContent.Replace("'", "''");
                        string ContentID = Array.ContentID;
                        string CheckIsNA = Array.IsNA;
                        string CheckListID = Array.CheckListID;
                        string PerentID = Array.ParentContentID;


                        // if ((command.Type == GridBatchEditingCommandType.Insert))
                        if (ContentID == "0")
                        {
                            //Hashtable newValues = command.NewValues;
                            //// Hashtable oldValues = command.OldValues;
                            //string CheckListContent = newValues["CheckListContent"].ToString();
                            //string CheckIsNA = newValues["IsNA"].ToString();

                            //GridDataItem parentItem = e.Item.OwnerTableView.ParentItem;
                            //string PerentID = parentItem.OwnerTableView.DataKeyValues[parentItem.ItemIndex]["ContentID"].ToString();

                            //var CheckListID = ddlCheckList.SelectedValue;

                            // Add On 22 Jan 2020 Check When User Add New Question Then Check In Checklist Is Use User Can't Be Add New Record
                            // string querydata = "select * from tblInspectionList where CheckListID='" + CheckListID + "'";
                            string querydata = "select 'X' from tblInspectionList where CheckListID='" + CheckListID + "'";
                            DataTable dt = databaseCon.GetData(querydata);
                            if (dt.Rows.Count == 0)
                            {
                                //                            var isAny = dbcontext.tblInspectionLists
                                //.Where(x => x.CheckListID == int.Parse(CheckListID)).Any();

                                //                            if (!isAny)
                                //                            {
                                // query = "insert into tblChecklistContent(CheckListContent,checklistItem,itemOrder,ClientID,CheckListID,IsNA,Createdby,CreatedOn)values('" + CheckListContent + "', '" + PerentID + "', (select ISNULL(MAX(itemorder) + 1, 1)  from tblChecklistContent where checklistItem = '" + PerentID + "'and ClientID = '" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and CheckListID = '" + ddlCheckList.SelectedValue + "'),'" + databaseCon.Decrypt(Request.QueryString["id"]) + "','" + ddlCheckList.SelectedValue + "','" + CheckIsNA + "','" + databaseCon.Decrypt(Request.QueryString["UID"]) + "','" + DateTime.Now + "')";

                                // Query Change On 21 Jan 2020 Add Created By
                                // query = "insert into tblChecklistContent(CheckListContent,checklistItem,itemOrder,ClientID,CheckListID,IsNA,Createdby,CreatedOn)values('" + CheckListContent + "', '" + PerentID + "', (select ISNULL(MAX(itemorder) + 1, 1)  from tblChecklistContent where checklistItem = '" + PerentID + "'and ClientID = '" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and CheckListID = '" + ddlCheckList.SelectedValue + "'),'" + databaseCon.Decrypt(Request.QueryString["id"]) + "','" + ddlCheckList.SelectedValue + "','" + CheckIsNA + "',(select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(Request.QueryString["UID"]) + "'),'" + DateTime.Now + "')";

                                query = "insert into tblChecklistContent(CheckListContent,checklistItem,itemOrder,ClientID,CheckListID,IsNA,Createdby,CreatedOn)values('" + CheckListContent + "', '" + PerentID + "', (select ISNULL(MAX(itemorder) + 1, 1)  from tblChecklistContent where checklistItem = '" + PerentID + "'and ClientID = '" + databaseCon.Decrypt(id) + "' and CheckListID = '" + CheckListID + "'),'" + databaseCon.Decrypt(id) + "','" + CheckListID + "','" + CheckIsNA + "',(select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'),'" + DateTime.Now + "')";

                                int result = databaseCon.RunCmd(query);
                                if (result > 0)
                                {
                                    //art1.Style.Add("display", "block");
                                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                                    //alertlabel.Text = "<strong> Success ! </strong>Data saved successfully";
                                    Result = "Success";
                                    Message = "Data saved successfully .";
                                }
                            }
                            else
                            {
                                Result = "Failed";
                                Message = "Record Can't be Added. Checklist is already in use.";
                                //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                                //art1.Style.Add("display", "block");
                                //alertlabel.Text = "<strong>Alert ! </strong> Record Can't be Added. Checklist is already in use.";
                            }

                        }

                        // if ((command.Type == GridBatchEditingCommandType.Update))
                        else
                        {
                            //Hashtable newValues = command.NewValues;
                            //// Hashtable oldValues = command.OldValues;
                            //string CheckListContent = newValues["CheckListContent"].ToString();
                            //string ContentID = newValues["ContentID"].ToString();
                            //string CheckIsNA = newValues["IsNA"].ToString();
                            //string CheckListID = newValues["CheckListID"].ToString();
                            //string querydata = "select * from tblInspectionList where CheckListID='" + CheckListID + "' and ContentID='" + ContentID + "'";
                            string querydata = "select 'X' from tblInspectionList where CheckListID='" + CheckListID + "' and ContentID='" + ContentID + "'";
                            DataTable dt = databaseCon.GetData(querydata);
                            if (dt.Rows.Count == 0)
                            {
                                //                            var isAny = dbcontext.tblInspectionLists
                                //.Where(x => x.CheckListID == int.Parse(CheckListID) &&  // Convert string to int
                                //      (x.ContentID == int.Parse(ContentID))).Any();

                                //                            if (!isAny)
                                //                            {
                                // query = "update tblChecklistContent set CheckListContent='" + CheckListContent + "',UpdatedBy='" + databaseCon.Decrypt(Request.QueryString["UID"]) + "',UpdatedOn='" + DateTime.Now + "',IsNA='" + CheckIsNA + "' where ContentID='" + ContentID + "'";
                                // Query Change On 21 Jan 2020 Add UpdatedBy And UpdatedOn
                                //query = "update tblChecklistContent set CheckListContent='" + CheckListContent + "',UpdatedBy = (select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(Request.QueryString["UID"]) + "'),UpdatedOn='" + DateTime.Now + "',IsNA='" + CheckIsNA + "' where ContentID='" + ContentID + "'";

                                query = "update tblChecklistContent set CheckListContent='" + CheckListContent + "',UpdatedBy = (select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'),UpdatedOn='" + DateTime.Now + "',IsNA='" + CheckIsNA + "' where ContentID='" + ContentID + "'";
                                int result = databaseCon.RunCmd(query);
                                if (result > 0)
                                {
                                    //art1.Style.Add("display", "block");
                                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                                    //alertlabel.Text = "<strong> Success ! </strong>Data Saved Successfully";
                                    Result = "Success";
                                    Message = "Data Saved Successfully .";
                                }
                            }
                            else
                            {
                                //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                                //art1.Style.Add("display", "block");
                                //alertlabel.Text = "<strong>Alert ! </strong> Can't be Changed. It is already in use.";
                                Result = "Failed";
                                Message = "Can't be Changed. It is already in use.";
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                //art1.Style.Add("display", "block");
                //alertlabel.Text = "<strong>Alert ! </strong>" + ex.Message;
                Result = "Failed";
                Message = ex.Message;
            }
            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult btnSave_Click(string id, string UID, string txtCheckList)
        {
            var Result = "";
            var Message = "";
            var CheckListID = "";
            string query = "";

            string QueryExist = "select 'x' from tblchecklist where  checklistdescription='" + txtCheckList.Replace("'", "''").Trim() + "' and clientID='" + databaseCon.Decrypt(id) + "'";
            DataTable dt = databaseCon.GetData(QueryExist);
            if (dt.Rows.Count != 0)
            {
                Result = "Failed";
                Message = "Already Exists .";
                //art1.Style.Add("display", "block");
                //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                //alertlabel.Text = "<strong> Alert ! </strong>Already Exists";
            }
            else
            {

                query = "insert into tblchecklist (checklistdescription,ClientID,CreatedBy,CreatedOn,status) values('" + txtCheckList.Replace("'", "''").Trim() + "','" + databaseCon.Decrypt(id) + "',(select Emailid from tbluser where UserId='" + databaseCon.Decrypt(UID) + "'),'" + DateTime.Now + "','Active')";
                int result = databaseCon.RunCmdID(query);
                if (result > 0)
                {
                    Result = "Success";
                    Message = "Data saved successfully .";
                    CheckListID = result.ToString();
                    //art1.Style.Add("display", "block");
                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                    //alertlabel.Text = "<strong> Success ! </strong>Data saved successfully";
                    // ddlCheckList.Visible = true;

                    //ddlCheckList.Items.Clear();
                    //DropdownBind();
                    //ddlCheckList.SelectedValue = Convert.ToString(result);
                    //txtCheckList.Text = "";
                    //GridInspectionCheckList.Rebind();
                    //RadioButtonBind();

                }
            }
            return Json(new { Result = Result, Message = Message, CheckListID = CheckListID }, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult btnCopyyes_Click(string id,string UID,string ddlCheckListFrom,string txtcopy)
        //{

        //    var Result = "";
        //    var Message = "";
        //    var CheckListID = "";
        //    string query = "exec Pro_MakeDuplicateCheckList  '" + ddlCheckListFrom + "','" + txtcopy.Replace("'", "''").Trim() + "','" + databaseCon.Decrypt(id) + "','" + databaseCon.Decrypt(UID) + "'";
        //    DataTable copydt = databaseCon.GetData(query);

        //    if (copydt.Rows.Count != 0)
        //    {
        //        if (copydt != null && copydt.Rows.Count > 0)
        //        {
        //            string firstCol = copydt.Rows[0][0].ToString();

        //            if (firstCol == "0") // duplicate or error message
        //            {
        //                Result = "Error";
        //                Message = copydt.Rows[0][1].ToString();
        //            }
        //        }
        //        else
        //        {
        //            //art1.Style.Add("display", "block");
        //            //art1.Attributes["class"] = "w3-section w3-card-12 fade in  j-alertBox-Success";
        //            //alertlabel.Text = "<strong>Success ! </strong>" + copydt.Rows[0][1].ToString();
        //            //// GridInspectionCheckList.Rebind();
        //            //DropdownBind();
        //            //dropdownChecklist();
        //            //RadioButtonBind();

        //            Result = "Success";
        //            Message = copydt.Rows[0][1].ToString();
        //            CheckListID = copydt.Rows[0][0].ToString();
        //        }

        //    }
        //    else
        //    {
        //        //art1.Attributes["class"] = "w3-section w3-card-12 fade in  j-alertBox-Failed";
        //        //art1.Style.Add("display", "block");
        //        //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator ";
        //        Result = "Failed";
        //        Message = "Syntax Error. Contact administrator .";
        //    }


        //    //ddlCheckList.SelectedValue = copydt.Rows[0][0].ToString();
        //    //divCopy.Style.Add("display", "none");
        //    //GridInspectionCheckList.Rebind();
        //    return Json(new { Result = Result, Message = Message, CheckListID= CheckListID }, JsonRequestBehavior.AllowGet);
        //}


        public ActionResult btnCopyyes_Click(string id, string UID, string ddlCheckListFrom, string txtcopy)
        {
            string Result = "";
            string Message = "";
            string CheckListID = "";

            try
            {
                //string clientId = databaseCon.Decrypt(id);
                //string doneBy = databaseCon.Decrypt(UID);
                //string sanitizedChecklistName = txtcopy.Replace("'", "''").Trim();

                string query = "exec Pro_MakeDuplicateCheckList  '" + ddlCheckListFrom + "','" + txtcopy.Replace("'", "''").Trim() + "','" + databaseCon.Decrypt(id) + "','" + databaseCon.Decrypt(UID) + "'";
                DataTable copydt = databaseCon.GetData(query);

                if (copydt != null && copydt.Rows.Count > 0)
                {
                    string status = copydt.Rows[0][0].ToString();
                    string msg = copydt.Rows[0][1].ToString();

                    if (status == "0")
                    {
                        Result = "Error";
                        Message = msg;
                    }
                    else
                    {
                        Result = "Success";
                        Message = msg;
                        CheckListID = status;
                    }

                }
                else
                {
                    Result = "Failed";
                    Message = "No data returned. Please contact the administrator.";
                }
            }
            catch (Exception ex)
            {
                Result = "Exception";
                Message = "An error occurred: " + ex.Message;
            }

            return Json(new { Result = Result, Message = Message, CheckListID = CheckListID }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult btnLoad_Click1(string id, string UID, string txtFileChecklist)
        {

            con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);

            var Result = "";
            var Message = "";
            var ddlCheckList = "";
            string filename = "";
            string filePath = "";
            var FileExtension = "";

            if (Request.Files.Count > 0)
            {
                //  Get all files from Request object  
                HttpFileCollectionBase files = Request.Files;
                for (int k = 0; k < 1; k++)
                {

                    HttpPostedFileBase file = files[k];


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

                    FileExtension = System.IO.Path.GetExtension(filename).ToLower();

                }
            }
            // string FileExtension = FileUploadSelectfile.FileName;
            if (FileExtension != "")
            {
                var ext = FileExtension.Split('.');
                var extension = ext[1];
                if (extension.ToUpper() == "xlsx".ToUpper())
                {
                    if (txtFileChecklist == "")
                    {
                        Result = "Failed";
                        Message = "Please Enter the name of the Checklist.";
                        //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Alert ! </strong>Please Enter the name of the Checklist.";
                    }
                    // else if (FileUploadSelectfile.HasFile == true)
                    else
                    {
                        // databaseCon.RunCmd("delete from tblTempCheckListDataUpload where UserId = " + databaseCon.Decrypt(Request.QueryString["UID"]));
                        databaseCon.RunCmd("delete from tblTempCheckListDataUpload where UserId = " + databaseCon.Decrypt(UID));
                        //  string filename = FileUploadSelectfile.FileName;
                        string Filename = filename;
                        // string filePath = Server.MapPath(FileUploadSelectfile.PostedFile.FileName);
                        //filePath = Server.MapPath("~/Files/") + filename;
                        //FileUploadSelectfile.SaveAs(filePath);
                        //filePath = Path.Combine(Server.MapPath("~/Files/"), filename + ".xlsx");
                        //file.SaveAs(filePath);
                        FileInfo workbook = new FileInfo(filePath);
                        using (ExcelPackage xlPackege = new ExcelPackage(workbook))
                        {
                            ExcelWorksheet worksheet = xlPackege.Workbook.Worksheets[1];
                            int totalRows = worksheet.Dimension.End.Row;
                            int totalCols = worksheet.Dimension.End.Column;
                            DataTable dtExcelData = new DataTable(worksheet.Name);
                            int validcount = 0;
                            int errorexist = 0;
                            for (int n = 1; n <= totalCols; n++)
                            {
                                string ColName = worksheet.Cells[1, n].Value.ToString().Trim();
                                dtExcelData.Columns.Add(ColName);
                                if (n == 1)
                                {
                                    if (ColName.ToLower() != "Section".ToLower())
                                    {
                                        errorexist = 1;
                                    }
                                }
                                if (n == 2)
                                {
                                    if (ColName.ToLower() != "Question".ToLower())
                                    {
                                        errorexist = 1;
                                    }
                                }
                                validcount = validcount + 1;
                            }
                            if (errorexist == 1)
                            {
                                Result = "Failed";
                                Message = "Columns do not match the columns of the Template.";
                                //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                                //art1.Style.Add("display", "block");
                                //alertlabel.Text = "<strong>Alert ! </strong>Columns do not match the columns of the Template.";
                            }
                            else
                            {
                                dtExcelData.Columns.Add("Clientid", typeof(System.Int32));
                                dtExcelData.Columns.Add("UserId", typeof(System.Int32));
                                DataRow dr = null;
                                for (int i = 2; i <= totalRows; i++)
                                {
                                    dr = dtExcelData.NewRow();
                                    for (int j = 1; j <= totalCols; j++)
                                    {
                                        if (worksheet.Cells[i, j].Value != null)
                                        {
                                            dr[j - 1] = worksheet.Cells[i, j].Value.ToString().Trim();
                                        }
                                        else
                                        {
                                            dr[j - 1] = null;
                                        }
                                    }
                                    dtExcelData.Rows.Add(dr);
                                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                                    {
                                        foreach (DataRow row in dtExcelData.Rows)
                                        {
                                            row["Clientid"] = databaseCon.Decrypt(id);
                                            row["UserId"] = databaseCon.Decrypt(UID);
                                        }
                                        sqlBulkCopy.DestinationTableName = "tblTempCheckListDataUpload";
                                        sqlBulkCopy.ColumnMappings.Add("Section", "Section");
                                        sqlBulkCopy.ColumnMappings.Add("Question", "Question");
                                        sqlBulkCopy.ColumnMappings.Add("clientid", "clientid");
                                        sqlBulkCopy.ColumnMappings.Add("UserId", "UserId");
                                        con.Open();
                                        sqlBulkCopy.WriteToServer(dtExcelData);
                                        con.Close();
                                        dtExcelData.Clear();
                                    }
                                }
                                //string query = "exec Pro_UploadChkList '" + txtFileChecklist.Text.Replace("'", "''").Trim() + "','" + databaseCon.Decrypt(Request.QueryString["UID"]) + "','" + DateTime.Now + "'";
                                string query = "exec Pro_UploadChkList '" + txtFileChecklist.Replace("'", "''").Trim() + "','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "'";
                                DataTable dtFileUpload = databaseCon.GetData(query);
                                if (dtFileUpload.Rows.Count != 0)
                                {
                                    if (Convert.ToInt32(dtFileUpload.Rows[0][0]) > 0)
                                    {
                                        Result = "Success";
                                        Message = dtFileUpload.Rows[0][1].ToString();
                                        ddlCheckList = dtFileUpload.Rows[0][0].ToString();

                                        //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                                        //art1.Style.Add("display", "block");
                                        //alertlabel.Text = "<strong>Success ! </strong>" + dtFileUpload.Rows[0][1].ToString();
                                        //DropdownBind();
                                        //RadioButtonBind();

                                        //txtFileChecklist.Text = "";
                                        //GridInspectionCheckList.Rebind();
                                    }
                                    else if (Convert.ToInt32(dtFileUpload.Rows[0][0]) == -1)
                                    {
                                        Result = "Failed";
                                        Message = dtFileUpload.Rows[0][1].ToString();
                                        //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                                        //art1.Style.Add("display", "block");
                                        //alertlabel.Text = "<strong>Alert ! </strong>" + dtFileUpload.Rows[0][1].ToString();

                                    }
                                    else
                                    {
                                        Result = "Failed";
                                        Message = dtFileUpload.Rows[0][1].ToString();
                                        //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                                        //art1.Style.Add("display", "block");
                                        //alertlabel.Text = "<strong>Alert ! </strong>" + dtFileUpload.Rows[0][1].ToString();
                                    }
                                }
                                else
                                {
                                    Result = "Failed";
                                    Message = "Syntax Error. Contact administrator .";
                                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                                    //art1.Style.Add("display", "block");
                                    //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator ";
                                }
                            }
                        }
                        // GridInspectionCheckList.Rebind();
                    }
                }
                else
                {
                    Result = "Failed";
                    Message = "Unable to Upload as Valid File Extensions Are .xlsx";
                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong>Unable to Upload as Valid File Extensions Are .xlsx";

                }
            }
            else
            {
                Result = "Failed";
                Message = "Please Select The File.";
                //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                //art1.Style.Add("display", "block");
                //alertlabel.Text = "<strong>Alert ! </strong>Please Select The File.";

            }
            return Json(new { Result = Result, Message = Message, ddlCheckList = ddlCheckList }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult btnDeleteYesLast_Click(string ddlDeleteList)
        {
            var Result = "";
            var Message = "";
            var query = "";
            query = "exec Pro_TblCheckList_DEL'" + ddlDeleteList + "'";
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count != 0)
            {
                if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                {
                    Result = "Success";
                    Message = dt.Rows[0][1].ToString();

                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Success ! </strong>" + dt.Rows[0][1].ToString();
                    //ddlDeleteList.ClearSelection();
                    //ddlDeleteList.Items.Clear();

                    //dropdownChecklist();
                    //ddlCheckList.ClearSelection();
                    //ddlCheckList.Items.Clear();
                    //DropdownBind();
                    //RadioButtonBind();
                    //GridInspectionCheckList.Rebind();

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
                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong>" + dt.Rows[0][1].ToString();
                }
            }
            else
            {
                Result = "Failed";
                Message = "Syntax Error. Contact administrator .";
                //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                //art1.Style.Add("display", "block");
                //alertlabel.Text = "<strong>Alert ! </strong>Syntax Error. Contact administrator ";
            }
            // divdeletechild.Style.Add("display", "none");
            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult btndeleteyes_Click(string id, string DeleteFlag, string ContentID)
        {
            var Result = "";
            var Message = "";
            if (DeleteFlag == "ViewLevel1")
            {
                //string querydata = "select * from tblInspectionList where ContentID='" + ViewState["ViewFordelete1"] + "'";
                //string querydata = "select * from tblInspectionList where ContentID='" + ContentID + "'";
                string querydata = "select 'X' from tblInspectionList where ContentID='" + ContentID + "'";
                DataTable dtdata = databaseCon.GetData(querydata);
                if (dtdata.Rows.Count == 0)
                {

                    //                var isAny = dbcontext.tblInspectionLists
                    //.Where(x => x.ContentID == int.Parse(ContentID)).Any();

                    //                if (!isAny)
                    //                {
                    //string query = "exec Pro_TBLCHECKLISTCONTENT_DEL '" + ViewState["ViewFordelete1"] + "'";
                    string query = "exec Pro_TBLCHECKLISTCONTENT_DEL '" + ContentID + "'";
                    DataTable dt = databaseCon.GetData(query);
                    if (dt.Rows.Count != 0)
                    {
                        if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                        {
                            //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                            //art1.Style.Add("display", "block");
                            //alertlabel.Text = "<strong>Success ! </strong>" + dt.Rows[0][1].ToString();
                            //GridInspectionCheckList.Rebind();
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
                        Message = "Syntax Error. Contact administrator .";
                    }
                }
                else
                {
                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong> Can't be Deleted. It is already in use.";
                    Result = "Failed";
                    Message = "Can't be Deleted. It is already in use.";
                }


                //divdelete.Style.Add("display", "none");
                //divChild.Style.Add("display", "none");
                //GridInspectionCheckList.Rebind();
            }
            if (DeleteFlag == "ViewLevel2")
            {
                // string querydata = "Select * from tblInspectionList where ContentID='" + ViewState["ViewFordelete2"] + "'";
                //string querydata = "Select * from tblInspectionList where ContentID='" + ContentID + "'";
                string querydata = "Select 'X' from tblInspectionList where ContentID='" + ContentID + "'";
                DataTable dtdata = databaseCon.GetData(querydata);
                if (dtdata.Rows.Count == 0)
                {

                    //                var isAny = dbcontext.tblInspectionLists
                    //.Where(x => x.ContentID == int.Parse(ContentID)).Any();

                    //                if (!isAny)
                    //                {

                    // string query = "exec Pro_TBLCHECKLISTCONTENT_DEL '" + ViewState["ViewFordelete2"] + "'";
                    string query = "exec Pro_TBLCHECKLISTCONTENT_DEL '" + ContentID + "'";
                    DataTable dt = databaseCon.GetData(query);
                    if (dt.Rows.Count != 0)
                    {
                        if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                        {
                            //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                            //art1.Style.Add("display", "block");
                            //alertlabel.Text = "<strong>Success ! </strong>" + dt.Rows[0][1].ToString();
                            //GridInspectionCheckList.Rebind();
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
                        Message = "Syntax Error. Contact administrator .";
                    }
                }
                else
                {
                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong> Can't be Deleted. It is already in use.";
                    Result = "Failed";
                    Message = "Can't be Deleted. It is already in use.";
                }
                //divdelete.Style.Add("display", "none");
                //divChild.Style.Add("display", "none");
                //GridInspectionCheckList.Rebind();
                //RadioButtonBind();
            }
            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult RadioButtonActive_CheckedChanged(string id, string UID, string ddlCheckList)
        {

            // query = "update tblchecklist set status='Active', where ClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and checklistID='" + ddlCheckList.SelectedValue + "'"; 
            // Query Change On 21 Jan 2020 Add UpdatedBy And UpdatedOn
            // string query = "update tblchecklist set status='Active',UpdatedBy = (select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(Request.QueryString["UID"]) + "'),UpdatedOn = '" + DateTime.Now + "' where ClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and checklistID='" + ddlCheckList.SelectedValue + "'";

            string query = "update tblchecklist set status='Active',UpdatedBy = (select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'),UpdatedOn = '" + DateTime.Now + "' where ClientId='" + databaseCon.Decrypt(id) + "' and checklistID='" + ddlCheckList.Replace("'", "''").Trim() + "'";

            int result = databaseCon.RunCmd(query);

            return Json(new { Result = "Success", Message = "Data Save Successfully ." }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RadioButtonInActive_CheckedChanged(string id, string UID, string ddlCheckList)
        {
            var Result = "";
            var Message = "";
            var StringVal = "";
            var lblChecklistDis = "";
            string query = "select ChecklistId from tblClient where ClientId = '" + databaseCon.Decrypt(id) + "'  ";
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows[0]["ChecklistId"] == null || dt.Rows[0]["ChecklistId"].ToString() == "")
            {
                query = "select  ChecklistId from tblregister where clientid = '" + databaseCon.Decrypt(id) + "' and CheckListId = '" + ddlCheckList.Replace("'", "''").Trim() + "'";
                dt = databaseCon.GetData(query);
                if (dt.Rows.Count > 0)
                {

                    query = "select tblRegister.CheckListId,checklistdescription, Right('000000' + Convert(varchar, ScaffRef), 6) as ScaffRef, [status],tblRegister.ClientID from tblchecklist join tblRegister on tblchecklist.ClientID=tblRegister.ClientID and tblchecklist.checklistID=tblRegister.checklistID where tblchecklist.ClientID='" + databaseCon.Decrypt(id) + "' and tblchecklist.checklistID='" + dt.Rows[0]["ChecklistId"].ToString() + "'";
                    dt = databaseCon.GetData(query);
                    //StringBuilder html = new StringBuilder();
                    //html.Append("<table style='width:100%; '>");
                    //html.Append("<thead  style='color: black'>");
                    //html.Append("<th>Scaf Ref #</th>");
                    //html.Append("<th>CheckList Name</th>");
                    //html.Append("</thead>");
                    //html.Append("<tbody style='color:grey'>");

                    string html = "";
                    html += "<table style='width:100%; '>";
                    html += "<thead  style='color: black'>";
                    html += "<th>Scaf Ref #</th>";
                    html += "<th>CheckList Name</th>";
                    html += "</thead>";
                    html += "<tbody style='color:grey'>";

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string ScaffRef = dt.Rows[i]["ScaffRef"].ToString();
                        string ChecklistName = dt.Rows[i]["checklistdescription"].ToString();
                        html += "<tr><td>" + ScaffRef + "</td>";
                        html += "<td>" + ChecklistName + "</td></tr>";

                    }

                    html += "</td></tr>";
                    html += "</tbody></table>";

                    // PlaceHolder2.Controls.Add(new Literal { Text = html.ToString() });
                    // divActivePopup.Style.Add("display", "block");

                    Result = "Failed";
                    Message = "Data Not Update .";
                    StringVal = html;
                }

                else
                {
                    // query = "update tblchecklist set status='InActive' where ClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and checklistID='" + ddlCheckList.SelectedValue + "'";
                    // Query Change On 21 Jan 2020 Add UpdatedBy And UpdatedOn
                    // query = "update tblchecklist set status='InActive',UpdatedBy =(select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(Request.QueryString["UID"]) + "'),UpdatedOn = '" + DateTime.Now + "' where ClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and checklistID='" + ddlCheckList.SelectedValue + "'";

                    query = "update tblchecklist set status='InActive',UpdatedBy =(select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'),UpdatedOn = '" + DateTime.Now + "' where ClientId='" + databaseCon.Decrypt(id) + "' and checklistID='" + ddlCheckList.Replace("'", "''").Trim() + "'";
                    int inActiveResult = databaseCon.RunCmd(query);

                    Result = "Success";
                    Message = "Data Update Successfull .";

                }
            }
            else
            {

                query = "select ChecklistId from tblClient where ClientId='" + databaseCon.Decrypt(id) + "' and ChecklistId='" + ddlCheckList.Replace("'", "''").Trim() + "'";
                dt = databaseCon.GetData(query);
                if (dt.Rows.Count > 0)
                {

                    //RadioButtonInActive.Checked = false;
                    //RadioButtonActive.Checked = true;
                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong>Please set another checklist as default for client and then mark this checklist as InActive.";
                    Result = "FailedForClient";
                    Message = "Please set another checklist as default for client and then mark this checklist as InActive.";
                }
                else
                {
                    query = "select  ChecklistId from tblregister where clientid = '" + databaseCon.Decrypt(id) + "' and CheckListId = '" + ddlCheckList.Replace("'", "''").Trim() + "'";
                    dt = databaseCon.GetData(query);
                    if (dt.Rows.Count > 0)
                    {
                        query = "select tblRegister.CheckListId,checklistdescription, Right('000000' + Convert(varchar, ScaffRef), 6) as ScaffRef, [status],tblRegister.ClientID from tblchecklist join tblRegister on tblchecklist.ClientID=tblRegister.ClientID and tblchecklist.checklistID=tblRegister.checklistID where tblchecklist.ClientID='" + databaseCon.Decrypt(id) + "' and tblchecklist.checklistID='" + dt.Rows[0]["ChecklistId"].ToString() + "'";
                        dt = databaseCon.GetData(query);
                        //StringBuilder html = new StringBuilder();
                        string html = "";
                        html += "<table style='width:100%; '>";
                        html += "<thead  style='color: black'>";
                        html += "<th>Scaf Ref #</th>";
                        html += "<th>CheckList Name</th>";
                        html += "</thead>";
                        html += "<tbody style='color:grey'>";
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string ScaffRef = dt.Rows[i]["ScaffRef"].ToString();
                            string ChecklistName = dt.Rows[i]["checklistdescription"].ToString();
                            html += "<tr><td>" + ScaffRef + "</td>";
                            html += "<td>" + ChecklistName + "</td></tr>";
                            //lbljobsID.InnerText = dt.Rows[i]["ScaffRef"].ToString();
                        }
                        html += "</td></tr>";
                        html += "</tbody></table>";
                        //  PlaceHolder1.Controls.Add(new Literal { Text = html.ToString() });



                        query = "select  ChecklistId from tblclient where clientid = '" + databaseCon.Decrypt(id) + "'";
                        dt = databaseCon.GetData(query);
                        if (dt.Rows.Count > 0)
                        {
                            query = "select tblclient.CheckListId,checklistdescription,tblclient.ClientID from tblchecklist join tblclient on tblchecklist.ClientID=tblclient.ClientID and tblchecklist.checklistID=tblclient.checklistID where tblchecklist.ClientID='" + databaseCon.Decrypt(id) + "' and tblchecklist.checklistID='" + dt.Rows[0]["ChecklistId"].ToString() + "'";
                            dt = databaseCon.GetData(query);
                            if (dt.Rows.Count > 0)
                            {
                                // lblChecklistDis.InnerText = dt.Rows[0]["checklistdescription"].ToString();
                                lblChecklistDis = dt.Rows[0]["checklistdescription"].ToString();
                            }

                        }
                        Result = "Failed";
                        Message = "Data Not Update .";
                        StringVal = html;


                        //  divActive.Style.Add("display", "block");
                    }
                    else
                    {
                        //query = "update tblchecklist set status='InActive' where ClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and checklistID='" + ddlCheckList.SelectedValue + "'";
                        // Query Change On 21 Jan 2020 Add UpdatedBy And UpdatedOn
                        query = "update tblchecklist set status='InActive',UpdatedBy =(select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'),UpdatedOn = '" + DateTime.Now + "' where ClientId='" + databaseCon.Decrypt(id) + "' and checklistID='" + ddlCheckList.Replace("'", "''").Trim() + "'";
                        int inActiveResult = databaseCon.RunCmd(query);

                        Result = "Success";
                        Message = "Data Update Successfull .";
                    }
                }
            }
            return Json(new { Result = Result, Message = Message, StringVal = StringVal, lblChecklistDis = lblChecklistDis }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult btnActiveYes_Click(string id, string UID, string ddlCheckList)
        {
            var Result = "";
            var Message = "";

            string query = "select ChecklistId from tblClient where ClientId='" + databaseCon.Decrypt(id) + "'";
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                //  query = "update tblregister set ChecklistId='" + dt.Rows[0]["ChecklistId"] + "' where ClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and checklistID='" + ddlCheckList.SelectedValue + "'";
                // Query Change On 21 Jan 2020 Add UpdatedBy And UpdatedOn

                // query = "update tblregister set ChecklistId='" + dt.Rows[0]["ChecklistId"] + "',UpdatedBy =(select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(Request.QueryString["UID"]) + "'),UpdatedOn = '" + DateTime.Now + "' where ClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and checklistID='" + ddlCheckList.SelectedValue + "'";

                query = "update tblregister set ChecklistId='" + dt.Rows[0]["ChecklistId"] + "',UpdatedBy =(select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'),UpdatedOn = '" + DateTime.Now + "' where ClientId='" + databaseCon.Decrypt(id) + "' and checklistID='" + ddlCheckList.Replace("'", "''").Trim() + "'";

                int result = databaseCon.RunCmd(query);
                if (result != 0)
                {
                    // query = "update tblchecklist set status='inactive' where clientid='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and checklistid='" + ddlCheckList.SelectedValue + "'";
                    // Query Change On 21 Jan 2020 Add UpdatedBy And UpdatedOn
                    query = "update tblchecklist set status='InActive',UpdatedBy =(select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'),UpdatedOn = '" + DateTime.Now + "' where clientid='" + databaseCon.Decrypt(id) + "' and checklistid='" + ddlCheckList + "'";
                    int result1 = databaseCon.RunCmd(query);

                    Result = "Success";
                    Message = "Data Save Successfully .";
                }
                else
                {
                    Result = "Failed";
                    Message = "Something want wrong";
                }
            }
            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
            // divActive.Style.Add("display", "none");
        }



        public ActionResult divActivePopupYes_Click(string id, string UID, string ddlCheckList)
        {
            var Result = "";
            var Message = "";
            //query = "update tblregister set ChecklistId=Null where ClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and checklistID='" + ddlCheckList.SelectedValue + "'";
            // Query Change On 21 Jan 2020 Add UpdatedBy And UpdatedOn

            // query = "update tblregister set ChecklistId=Null ,UpdatedBy =(select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(Request.QueryString["UID"]) + "'),UpdatedOn = '" + DateTime.Now + "' where ClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and checklistID='" + ddlCheckList.SelectedValue + "'";

            string query = "update tblregister set ChecklistId=Null ,UpdatedBy =(select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'),UpdatedOn = '" + DateTime.Now + "' where ClientId='" + databaseCon.Decrypt(id) + "' and checklistID='" + ddlCheckList.Replace("'", "''").Trim() + "'";
            int result = databaseCon.RunCmd(query);
            if (result != 0)
            {
                // query = "update tblchecklist set status='inactive' where clientid='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and checklistid='" + ddlCheckList.SelectedValue + "'";
                // Query Change On 21 Jan 2020 Add UpdatedBy And UpdatedOn
                query = "update tblchecklist set status='InActive' ,UpdatedBy =(select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'),UpdatedOn = '" + DateTime.Now + "' where clientid='" + databaseCon.Decrypt(id) + "' and checklistid='" + ddlCheckList + "'";
                int result1 = databaseCon.RunCmd(query);

                Result = "Success";
                Message = "Data Save Successfully .";

            }
            else
            {
                Result = "Failed";
                Message = "Something want wrong";
            }
            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
            // divActivePopup.Style.Add("display", "none");
        }

        public ActionResult RadioBtnColorGreenRed_CheckedChanged(string id, string UID)
        {
            //query = "update tblclient set InspectionCheckListColor=0  where ClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "'" ;
            // Query Change On 21 Jan 2020 Add UpdatedBy And UpdatedOn
            // string query = "update tblclient set InspectionCheckListColor=0 ,UpdatedBy =(select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(Request.QueryString["UID"]) + "'),UpdatedOn = '" + DateTime.Now + "'  where ClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "'";
            string query = "update tblclient set InspectionCheckListColor=0 ,UpdatedBy =(select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'),UpdatedOn = '" + DateTime.Now + "'  where ClientId='" + databaseCon.Decrypt(id) + "'";
            int result = databaseCon.RunCmd(query);
            return Json(new { Result = "Success", Message = "Data Save Successfully ." }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RadioBtnColorBlack_CheckedChanged(string id, string UID)
        {
            // query = "update tblclient set InspectionCheckListColor=1  where ClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "'";
            // Query Change On 21 Jan 2020 Add UpdatedBy And UpdatedOn
            string query = "update tblclient set InspectionCheckListColor=1,UpdatedBy =(select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'),UpdatedOn = '" + DateTime.Now + "'  where ClientId='" + databaseCon.Decrypt(id) + "'";
            int result = databaseCon.RunCmd(query);
            return Json(new { Result = "Success", Message = "Data Save Successfully ." }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult btnsetdefault_Click(string id, string UID, string RID, string ChkID, string RadioButtonInActive, string ddlCheckList)
        {
            var Result = "";
            var Message = "";
            var query = "";
            DataTable dt;
            //if (RadioButtonInActive.Checked == true)
            if (RadioButtonInActive == "True")
            {
                Result = "Failed";
                Message = "InActive checklist can not be set as default checklist.";
                //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                //art1.Style.Add("display", "block");
                //alertlabel.Text = "<strong>Alert ! </strong>InActive checklist can not be set as default checklist.";
            }
            else
            {
                //query = "select * from tblChecklistContent where checklistItem is null and ClientID='" + databaseCon.Decrypt(id) + "' and CheckListID='" + ddlCheckList + "' order by ItemOrder asc";
                query = "select 'X' from tblChecklistContent where checklistItem is null and ClientID='" + databaseCon.Decrypt(id) + "' and CheckListID='" + ddlCheckList + "' order by ItemOrder asc";
                dt = databaseCon.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    //if (Request.QueryString["ID"] != null && Request.QueryString["RID"] == null)
                    if (id != null && RID == null)
                    {
                        // query = "update tblClient set CheckListId='" + ddlCheckList.SelectedValue + "' where ClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "'";
                        // Query Change On 21 Jan 2020 Add UpdatedBy And UpdatedOn
                        //  query = "update tblClient set CheckListId='" + ddlCheckList.SelectedValue + "',UpdatedBy =(select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(Request.QueryString["UID"]) + "'),UpdatedOn = '" + DateTime.Now + "' where ClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "'";
                        query = "update tblClient set CheckListId='" + ddlCheckList + "',UpdatedBy =(select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'),UpdatedOn = '" + DateTime.Now + "' where ClientId='" + databaseCon.Decrypt(id) + "'";
                        int result = databaseCon.RunCmd(query);
                        if (result != 0)
                        {
                            Result = "Success";
                            Message = "Data Saved Successfully .";
                            //art1.Style.Add("display", "block");
                            //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                            //alertlabel.Text = "<strong> Success ! </strong>Data Saved Successfully";

                        }

                    }
                    //else if (Request.QueryString["ChkID"] != null)
                    else if (ChkID != null)
                    {
                        //query = "update tblregister set CheckListId='" + ddlCheckList.SelectedValue + "' where registerId='" + databaseCon.Decrypt(Request.QueryString["RID"]) + "'";
                        // Query Change On 21 Jan 2020 Add UpdatedBy And UpdatedOn
                        query = "update tblregister set CheckListId='" + ddlCheckList + "',UpdatedBy =(select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'),UpdatedOn = '" + DateTime.Now + "' where registerId='" + databaseCon.Decrypt(RID) + "'";
                        int result = databaseCon.RunCmd(query);
                        if (result != 0)
                        {
                            Result = "Success";
                            Message = "Data Saved Successfully .";
                            //art1.Style.Add("display", "block");
                            //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                            //alertlabel.Text = "<strong> Success ! </strong>Data Saved Successfully";

                        }
                        else
                        {
                            Result = "Failed";
                            Message = "Please Create Job";
                            //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                            //art1.Style.Add("display", "block");
                            //alertlabel.Text = "<strong>Alert ! </strong>Please Create Job";

                        }
                    }

                }
                else
                {
                    Result = "Failed";
                    Message = "You  can not be set as default checklist as does not have any checklist items.";
                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong>You  can not be set as default checklist as does not have any checklist items.";
                }
            }
            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowDeletePopup(string id, string ContentID, string TableValues)
        {

            var CheckDiv = "";
            // if (e.Item.OwnerTableView.Name == "ViewLevel1")
            if (TableValues == "ViewLevel1")
            {
                //if (e.CommandName == RadGrid.DeleteCommandName)
                //{
                //    DeleteFlag = "ViewLevel1";
                //    GridEditableItem item = (GridEditableItem)e.Item;
                //    ViewState["ViewFordelete1"] = Convert.ToInt32(item.GetDataKeyValue("ContentID"));
                //    string query = "select * from tblChecklistContent where ContentID ='" + ViewState["ViewFordelete1"] + "' or checklistItem='" + ViewState["ViewFordelete1"] + "' or subChildItem='" + ViewState["ViewFordelete1"] + "'";


                //string query = "select * from tblChecklistContent where ContentID ='" + ContentID + "' or checklistItem='" + ContentID + "' or subChildItem='" + ContentID + "'";
                string query = "select 'X' from tblChecklistContent where ContentID ='" + ContentID + "' or checklistItem='" + ContentID + "' or subChildItem='" + ContentID + "'";

                DataTable dt = databaseCon.GetData(query);
                if (dt.Rows.Count == 1)
                {
                    // divChild.Style.Add("display", "block");
                    CheckDiv = "divChild";
                }
                else if (dt.Rows.Count > 1)
                {
                    // divdelete.Style.Add("display", "block");
                    CheckDiv = "divdelete";
                }

                // }
            }
            //  if (e.Item.OwnerTableView.Name == "ViewLevel2")
            if (TableValues == "ViewLevel2")
            {
                //if (e.CommandName == RadGrid.DeleteCommandName)
                //{
                //    DeleteFlag = "ViewLevel2";

                //    var detailgrid = e.Item.OwnerTableView;
                //    GridEditableItem item = (GridEditableItem)e.Item;
                //    ViewState["ViewFordelete2"] = Convert.ToInt32(item.GetDataKeyValue("ContentID"));
                //string query = "select * from tblChecklistContent where ContentID ='" + ViewState["ViewFordelete2"] + "' or checklistItem='" + ViewState["ViewFordelete2"] + "' or subChildItem='" + ViewState["ViewFordelete2"] + "'";

               // string query = "select * from tblChecklistContent where ContentID ='" + ContentID + "' or checklistItem='" + ContentID + "' or subChildItem='" + ContentID + "'";
                string query = "select 'X' from tblChecklistContent where ContentID ='" + ContentID + "' or checklistItem='" + ContentID + "' or subChildItem='" + ContentID + "'";
                DataTable dt = databaseCon.GetData(query);
                if (dt.Rows.Count == 1)
                {
                    // divChild.Style.Add("display", "block");
                    CheckDiv = "divChild";
                }
                else if (dt.Rows.Count > 1)
                {
                    // divdelete.Style.Add("display", "block");
                    CheckDiv = "divdelete";
                }
                // }
            }
            return Json(CheckDiv, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShortOrderUpLevel1(string id, string UID, string OrderContentID, string OrderContentID2, string itemorder, string CheckListID)
        {

            string query = "";
            var Result = "";
            var Message = "";

            //GridDataItem item = (GridDataItem)e.Item;
            //int OrderContentID = Convert.ToInt32(item.GetDataKeyValue("ContentID"));
            //int itemorder = Convert.ToInt32(item.GetDataKeyValue("itemOrder"));
            //int itemIndex = Convert.ToInt32(e.Item.ItemIndex);
            //GridDataItem itemdata = (GridDataItem)GridInspectionCheckList.Items[itemIndex - 1];
            //int OrderContentID2 = Convert.ToInt32(itemdata.GetDataKeyValue("ContentID"));
            //int CheckListID = Convert.ToInt32(item.GetDataKeyValue("CheckListID"));

            // Code Add on 21 Jan 2020 Check When Chenge Order Inspectiion Header And qustion if Use Already Use Cannot be Change
            //string querydata = "select * from tblInspectionList where CheckListID='" + CheckListID + "' and (ContentID='" + OrderContentID + "' or ContentID='" + OrderContentID2 + "')";
            string querydata = "select 'X' from tblInspectionList where CheckListID='" + CheckListID + "' and (ContentID='" + OrderContentID + "' or ContentID='" + OrderContentID2 + "')";
            DataTable dt1 = databaseCon.GetData(querydata);
            if (dt1.Rows.Count == 0)
            {
                //var isAny = dbcontext.tblInspectionLists
                //    .Where(x => x.CheckListID == int.Parse(CheckListID) &&  // Convert string to int
                //    (x.ContentID == int.Parse(OrderContentID) || x.ContentID == int.Parse(OrderContentID2))).Any();

                //if (!isAny)
                //{
                // query = "update tblChecklistContent set itemOrder='" + (itemorder - 1) + "' where ContentID='" + OrderContentID + "'";

                // Query Change On 21 Jan 2020 Add UpdatedBy And UpdatedOn
                query = "update tblChecklistContent set itemOrder='" + (Convert.ToInt32(itemorder) - 1) + "',UpdatedBy =(select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'),UpdatedOn = '" + DateTime.Now + "' where ContentID='" + OrderContentID + "'";
                databaseCon.RunCmd(query);

                // query = "update tblChecklistContent set itemOrder='" + itemorder + "' where ContentID='" + OrderContentID2 + "'";

                // Query Change On 21 Jan 2020 Add UpdatedBy And UpdatedOn
                query = "update tblChecklistContent set itemOrder='" + itemorder + "',UpdatedBy =(select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'),UpdatedOn = '" + DateTime.Now + "' where ContentID='" + OrderContentID2 + "'";
                databaseCon.RunCmd(query);

                Result = "Success";
                Message = "Item Order Change Successfully .";
            }
            else
            {
                Result = "Failed";
                Message = "Order Can't be Changed. It is already in use.";
                //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                //art1.Style.Add("display", "block");
                //alertlabel.Text = "<strong>Alert ! </strong>Order Can't be Changed. It is already in use.";
            }
            // GridInspectionCheckList.MasterTableView.Rebind();
            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShortOrderDownLevel1(string id, string UID, string OrderContentID, string OrderContentID2, string itemorder, string CheckListID)
        {
            string query = "";
            var Result = "";
            var Message = "";

            //GridDataItem item = (GridDataItem)e.Item;
            //int OrderContentID = Convert.ToInt32(item.GetDataKeyValue("ContentID"));
            //int itemorder = Convert.ToInt32(item.GetDataKeyValue("itemOrder"));
            //int itemIndex = Convert.ToInt32(e.Item.ItemIndex);
            //GridDataItem itemdata = (GridDataItem)GridInspectionCheckList.Items[itemIndex + 1];
            //int OrderContentID2 = Convert.ToInt32(itemdata.GetDataKeyValue("ContentID"));

            //// Code Add on 21 Jan 2020 Check When Chenge Order Inspectiion Header And qustion if Use Already Use Cannot be Change
            //int CheckListID = Convert.ToInt32(item.GetDataKeyValue("CheckListID"));

            //string querydata = "select * from tblInspectionList where CheckListID='" + CheckListID + "' and (ContentID='" + OrderContentID + "' or ContentID='" + OrderContentID2 + "')";
            string querydata = "select 'X' from tblInspectionList where CheckListID='" + CheckListID + "' and (ContentID='" + OrderContentID + "' or ContentID='" + OrderContentID2 + "')";
            DataTable dt1 = databaseCon.GetData(querydata);
            if (dt1.Rows.Count == 0)
            {
                //        var isAny = dbcontext.tblInspectionLists
                //.Where(x => x.CheckListID == int.Parse(CheckListID) &&  // Convert string to int
                //            (x.ContentID == int.Parse(OrderContentID) || x.ContentID == int.Parse(OrderContentID2))).Any();

                //        if (!isAny)
                //        {
                // query = "update tblChecklistContent set itemOrder='" + (itemorder + 1) + "' where ContentID='" + OrderContentID + "'";
                // Query Change On 21 Jan 2020 Add UpdatedBy And UpdatedOn
                query = "update tblChecklistContent set itemOrder='" + (Convert.ToInt32(itemorder) + 1) + "',UpdatedBy =(select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'),UpdatedOn = '" + DateTime.Now + "' where ContentID='" + OrderContentID + "'";

                databaseCon.RunCmd(query);
                // query = "update tblChecklistContent set itemOrder='" + itemorder + "' where ContentID='" + OrderContentID2 + "'";
                // Query Change On 21 Jan 2020 Add UpdatedBy And UpdatedOn
                query = "update tblChecklistContent set itemOrder='" + itemorder + "',UpdatedBy =(select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'),UpdatedOn = '" + DateTime.Now + "' where ContentID='" + OrderContentID2 + "'";

                databaseCon.RunCmd(query);
                Result = "Success";
                Message = "Item Order Change Successfully .";
            }
            else
            {
                Result = "Failed";
                Message = "Order Can't be Changed. It is already in use.";
                //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                //art1.Style.Add("display", "block");
                //alertlabel.Text = "<strong>Alert ! </strong>Order Can't be Changed. It is already in use.";
            }
            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ShortOrderUpLevel2(string id, string UID, string OrderContentID, string OrderContentID2, string itemorder, string CheckListID)
        {
            string query = "";
            var Result = "";
            var Message = "";


            //GridDataItem item = (GridDataItem)e.Item;
            //int OrderContentID = Convert.ToInt32(item.GetDataKeyValue("ContentID"));
            //int itemorder = Convert.ToInt32(item.GetDataKeyValue("itemOrder"));
            //int itemIndex = Convert.ToInt32(e.Item.ItemIndex);
            //GridDataItem itemdata = (GridDataItem)e.Item.OwnerTableView.Items[itemIndex - 1];
            //int OrderContentID2 = Convert.ToInt32(itemdata.GetDataKeyValue("ContentID"));

            //// Code Add on 21 Jan 2020 Check When Chenge Order Inspectiion Header And qustion if Use Already Use Cannot be Change
            //int CheckListID = Convert.ToInt32(item.GetDataKeyValue("CheckListID"));

            //string querydata = "select * from tblInspectionList where CheckListID='" + CheckListID + "' and (ContentID='" + OrderContentID + "' or ContentID='" + OrderContentID2 + "')";
            string querydata = "select 'X' from tblInspectionList where CheckListID='" + CheckListID + "' and (ContentID='" + OrderContentID + "' or ContentID='" + OrderContentID2 + "')";
            DataTable dt1 = databaseCon.GetData(querydata);
            if (dt1.Rows.Count == 0)
            {
                //        var isAny = dbcontext.tblInspectionLists
                //.Where(x => x.CheckListID == int.Parse(CheckListID) &&  // Convert string to int
                //            (x.ContentID == int.Parse(OrderContentID) || x.ContentID == int.Parse(OrderContentID2))).Any();

                //        if (!isAny)
                //        {
                //query = "update tblChecklistContent set itemOrder='" + (itemorder - 1) + "' where ContentID='" + OrderContentID + "'";
                // Query Change On 21 Jan 2020 Add UpdatedBy And UpdatedOn
                query = "update tblChecklistContent set itemOrder='" + (Convert.ToInt32(itemorder) - 1) + "',UpdatedBy =(select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'),UpdatedOn = '" + DateTime.Now + "' where ContentID='" + OrderContentID + "'";
                databaseCon.RunCmd(query);
                // query = "update tblChecklistContent set itemOrder='" + itemorder + "' where ContentID='" + OrderContentID2 + "'";
                // Query Change On 21 Jan 2020 Add UpdatedBy And UpdatedOn
                query = "update tblChecklistContent set itemOrder='" + itemorder + "',UpdatedBy =(select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'),UpdatedOn = '" + DateTime.Now + "' where ContentID='" + OrderContentID2 + "'";
                databaseCon.RunCmd(query);
                Result = "Success";
                Message = "Item Order Change Successfully .";
            }
            else
            {
                Result = "Failed";
                Message = "Order Can't be Changed. It is already in use.";
                //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                //art1.Style.Add("display", "block");
                //alertlabel.Text = "<strong>Alert ! </strong>Order Can't be Changed. It is already in use.";
            }


            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ShortOrderDownLevel2(string id, string UID, string OrderContentID, string OrderContentID2, string itemorder, string CheckListID)
        {
            string query = "";
            var Result = "";
            var Message = "";

            //GridDataItem item = (GridDataItem)e.Item;
            //int OrderContentID = Convert.ToInt32(item.GetDataKeyValue("ContentID"));
            //int itemorder = Convert.ToInt32(item.GetDataKeyValue("itemOrder"));
            //int itemIndex = Convert.ToInt32(e.Item.ItemIndex);
            //GridDataItem itemdata = (GridDataItem)e.Item.OwnerTableView.Items[itemIndex + 1];
            //int OrderContentID2 = Convert.ToInt32(itemdata.GetDataKeyValue("ContentID"));

            //// Code Add on 21 Jan 2020 Check When Chenge Order Inspectiion Header And qustion if Use Already Use Cannot be Change
            //int CheckListID = Convert.ToInt32(item.GetDataKeyValue("CheckListID"));

            //string querydata = "select * from tblInspectionList where CheckListID='" + CheckListID + "' and (ContentID='" + OrderContentID + "' or ContentID='" + OrderContentID2 + "')";
            string querydata = "select 'X' from tblInspectionList where CheckListID='" + CheckListID + "' and (ContentID='" + OrderContentID + "' or ContentID='" + OrderContentID2 + "')";
            DataTable dt1 = databaseCon.GetData(querydata);
            if (dt1.Rows.Count == 0)
            {
                //      var isAny = dbcontext.tblInspectionLists
                //.Where(x => x.CheckListID == int.Parse(CheckListID) &&  // Convert string to int
                //            (x.ContentID == int.Parse(OrderContentID) || x.ContentID == int.Parse(OrderContentID2))).Any();

                //      if (!isAny)
                //      {

                // query = "update tblChecklistContent set itemOrder='" + (itemorder + 1) + "' where ContentID='" + OrderContentID + "'";
                // Query Change On 21 Jan 2020 Add UpdatedBy And UpdatedOn
                query = "update tblChecklistContent set itemOrder='" + (Convert.ToInt32(itemorder) + 1) + "',UpdatedBy =(select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'),UpdatedOn = '" + DateTime.Now + "' where ContentID='" + OrderContentID + "'";
                databaseCon.RunCmd(query);
                // query = "update tblChecklistContent set itemOrder='" + itemorder + "' where ContentID='" + OrderContentID2 + "'";
                // Query Change On 21 Jan 2020 Add UpdatedBy And UpdatedOn
                query = "update tblChecklistContent set itemOrder='" + itemorder + "',UpdatedBy =(select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'),UpdatedOn = '" + DateTime.Now + "' where ContentID='" + OrderContentID2 + "'";
                databaseCon.RunCmd(query);
                Result = "Success";
                Message = "Item Order Change Successfully .";
            }
            else
            {
                Result = "Failed";
                Message = "Order Can't be Changed. It is already in use.";
                //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                //art1.Style.Add("display", "block");
                //alertlabel.Text = "<strong>Alert ! </strong>Order Can't be Changed. It is already in use.";
            }

            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult btndownload_Click()
        {
            var Result = "";
            var Message = "";
            string filename = "CheckList Builder.xlsx";
            string path = databaseCon.registerdocumentlink + filename;
            var uri = new Uri(databaseCon.registerdocumentlink);
            var container = new CloudBlobContainer(uri);
            var blob = container.GetBlockBlobReference(filename);
            var exists = blob.Exists();
            if (exists == false)
            {
                Result = "Failed";
                Message = "File dose not exists.";
                //art1.Style.Add("display", "block");
                //art1.Attributes["class"] = "w3-section w3-card-12 j-close fade in j-alertBox-Failed";
                //alertlabel.Text = "<strong>Alert ! </strong> File dose not exists.";
            }
            else
            {
                Result = "Success";
                Message = path;
                // Response.Write("<script> window.open('" + path + "','_blank'); </script>");
            }
            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }
    }

}