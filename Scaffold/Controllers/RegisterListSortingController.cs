using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scaffold.Controllers
{
    public class RegisterListSortingController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: SubCategories
        public ActionResult RegisterListSorting(string UID, string UserRole, string id)
        {
            return View();
        }
        public ActionResult GetRegisterListSortingData(string id)
        {

            var GetData = dbContext.Database.SqlQuery<RegisterColumnsViewModel>("select cast(RegisterListSortingId  as nvarchar(20))RegisterListSortingId,cast(ClientId as nvarchar(20))ClientId,DisplayColumnName,cast(ItemOrder as nvarchar(20))ItemOrder,cast(IsNA  as nvarchar(20))IsNA,CreatedOn,Createdby,UpdatedOn,UpdatedBy from tblRegisterListSorting where itemorder not in (0)   and ClientID='" + databaseCon.Decrypt(id) + "'  order by caST(ItemOrder as int) asc").ToList();
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult GetRegisterListDisplayColumnNameData(string id, string RegisterListSortingId)
        //{

        //    var GetData = dbContext.Database.SqlQuery<RegisterColumnsViewModel>("select RegisterListSortingId,DisplayColumnName,ColumnID, ColumnItem,subChildItem,ClientID,ItemOrder,IsNA from tblRegisterListSorting where ColumnItem='" + RegisterListSortingId + "' and subChildItem is null and ClientID='" + databaseCon.Decrypt(id) + "' order by ItemOrder").ToList();
        //    return Json(GetData, JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        public JsonResult SaveNAStatus(string id, string UID, int RegisterListSortingId, bool IsNA)
        {
            var Result = "";
            var Message = "";

            var query = "update tblRegisterListSorting set UpdatedBy = (select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'),UpdatedOn='" + DateTime.Now + "',IsNA='" + IsNA + "' where RegisterListSortingId='" + RegisterListSortingId + "' and ClientId='" + databaseCon.Decrypt(id) + "'";

            int result = databaseCon.RunCmd(query);
            if (result > 0)
            {
                Result = "Success";
                Message = "Data saved successfully .";
            }
            else
            {
                Result = "Failed";
                Message = "Update failed. No rows updated."; 
            }
            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
            //return Json("OK", JsonRequestBehavior.AllowGet);
        }

        public ActionResult RegisterListSortingSaveData(string id, string UID, string TableValues, List<RegisterColumnsViewModel> RegisterListSortingVal)
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
                    foreach (RegisterColumnsViewModel item in RegisterListSortingVal)
                    {
                        var Array = item;
                         
                        string DisplayColumnName = Array.DisplayColumnName.Replace("'", "''");

                        string RegisterListSortingId = Array.RegisterListSortingId;
                        string CheckIsNA = Array.IsNA;
                         
                        if (RegisterListSortingId == "0")
                        {

                         query = "insert into tblRegisterListSorting(ClientId,ItemOrder,DisplayColumnName,IsNA,Createdby,CreatedOn) values('" + databaseCon.Decrypt(id) + "' ,(select ISNULL( MAX(ItemOrder)+1,1)  from tblRegisterListSorting where ClientID='" + databaseCon.Decrypt(id) + "'),'" + DisplayColumnName + "','" + CheckIsNA + "',(select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'),'" + DateTime.Now + "')";

                                int result = databaseCon.RunCmd(query);
                                if (result > 0)
                                {
                                    Result = "Success";
                                    Message = "Data saved successfully .";
                                }



                        }
                     
                        else
                        {

                                query = "update tblRegisterListSorting set DisplayColumnName='" + DisplayColumnName + "',UpdatedBy = (select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'),UpdatedOn='" + DateTime.Now + "',IsNA='" + CheckIsNA + "' where RegisterListSortingId='" + RegisterListSortingId + "' and ClientId='" + databaseCon.Decrypt(id) + "'";

                                int result = databaseCon.RunCmd(query);
                                if (result > 0)
                                {
 
                                    Result = "Success";
                                    Message = "Data Saved Successfully .";
                                }


                        }



                    }
                }
 

            }
            catch (Exception ex)
            {
 
                Result = "Failed";
                Message = ex.Message;
            }
            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ShortOrderUpLevel1(string id, string UID, string OrderRegisterListSortingId, string OrderRegisterListSortingId2, string ItemOrder )
        {

            string query1 = "";
            string query2 = "";
            var Result = "";
            var Message = "";

            //itemorder=100 has been set for [delete], async it would be frozen at the end,iTextSharp sorting order will not be changed on adding a new column
            string maxquery = "select MAX(ItemOrder)maxvalue from tblRegisterListSorting where itemorder!=100 and clientid='" + databaseCon.Decrypt(id) + "'   ";
            DataTable dtmaxquery = databaseCon.GetData(maxquery);
            string maxvalue = dtmaxquery.Rows[0]["maxvalue"].ToString();

            if (ItemOrder == "6")

            {
                query1 = "update tblRegisterListSorting set ItemOrder= '" + maxvalue + "'+1,UpdatedBy =(select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'),UpdatedOn = '" + DateTime.Now + "' where RegisterListSortingId='" + OrderRegisterListSortingId + "' and ClientID='" + databaseCon.Decrypt(id) + "'   ";

                query2 = "update tblRegisterListSorting set ItemOrder=ItemOrder-1,UpdatedBy =(select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'),UpdatedOn = '" + DateTime.Now + "' where    ClientID='" + databaseCon.Decrypt(id) + "' and  ItemOrder>=6  and ItemOrder!=100 ";

            }
            else
            {
                query1 = "update tblRegisterListSorting set ItemOrder='" + (Convert.ToInt32(ItemOrder) - 1) + "',UpdatedBy =(select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'),UpdatedOn = '" + DateTime.Now + "' where RegisterListSortingId='" + OrderRegisterListSortingId + "' and ClientID='" + databaseCon.Decrypt(id) + "' and ItemOrder>=6 and ItemOrder<='" + maxvalue + "' ";


                query2 = "update tblRegisterListSorting set ItemOrder='" + ItemOrder + "',UpdatedBy =(select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'),UpdatedOn = '" + DateTime.Now + "' where RegisterListSortingId='" + OrderRegisterListSortingId2 + "' and ClientID='" + databaseCon.Decrypt(id) + "' and ItemOrder>=6 and  ItemOrder<='" + maxvalue + "' ";
             
            }

            databaseCon.RunCmd(query1);
            databaseCon.RunCmd(query2);
            Result = "Success";
                Message = "Item Order Change Successfully .";
  
            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShortOrderDownLevel1(string id, string UID, string OrderRegisterListSortingId, string OrderRegisterListSortingId2, string ItemOrder)
        {
            string query1 = "";
            string query2 = "";
            var Result = "";
            var Message = "";

            //itemorder=100 has been set for [delete], async it would be frozen at the end,iTextSharp sorting order will not be changed on adding a new column
            string maxquery = "select MAX(ItemOrder)maxvalue from tblRegisterListSorting where itemorder!=100 and clientid='" + databaseCon.Decrypt(id) + "'   ";
            DataTable dtmaxquery = databaseCon.GetData(maxquery);
            string maxvalue = dtmaxquery.Rows[0]["maxvalue"].ToString();


            if (ItemOrder == maxvalue)

            {


                query1 = "update tblRegisterListSorting set ItemOrder= ItemOrder+1,UpdatedBy =(select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'),UpdatedOn = '" + DateTime.Now + "' where ClientID='" + databaseCon.Decrypt(id) + "' and ItemOrder<='" + maxvalue + "' and ItemOrder>=6 ";


                query2 = "update tblRegisterListSorting set ItemOrder=6,UpdatedBy =(select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'),UpdatedOn = '" + DateTime.Now + "' where  RegisterListSortingId='" + OrderRegisterListSortingId + "' and ClientID='" + databaseCon.Decrypt(id) + "'   ";



            }
            else {
                query1 = "update tblRegisterListSorting set ItemOrder=isnull('" + (Convert.ToInt32(ItemOrder)) + "',0)+1,UpdatedBy =(select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'),UpdatedOn = '" + DateTime.Now + "' where RegisterListSortingId='" + OrderRegisterListSortingId + "' and ClientID='" + databaseCon.Decrypt(id) + "' and ItemOrder>=6 and ItemOrder<='" + maxvalue + "' ";
                 

                query2 = "update tblRegisterListSorting set ItemOrder='" + ItemOrder + "',UpdatedBy =(select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(UID) + "'),UpdatedOn = '" + DateTime.Now + "' where RegisterListSortingId='" + OrderRegisterListSortingId2 + "' and ClientID='" + databaseCon.Decrypt(id) + "' and ItemOrder>=6 and ItemOrder<='" + maxvalue + "' ";
                 
            }
            databaseCon.RunCmd(query1);
            databaseCon.RunCmd(query2);
            Result = "Success";
            Message = "Item Order Change Successfully .";

            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ShowDeletePopup(string id, string RegisterListSortingId, string TableValues)
        {

            var CheckDiv = ""; 
            if (TableValues == "ViewLevel1")
            {
 
                //string query = "select * from tblRegisterListSorting where RegisterListSortingId ='" + RegisterListSortingId + "' ";
                string query = "select 'X' as DeleteFlag from tblRegisterListSorting where RegisterListSortingId ='" + RegisterListSortingId + "' ";

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

            }

            return Json(CheckDiv, JsonRequestBehavior.AllowGet);
        }
        public ActionResult btndeleteyes_Click(string id, string DeleteFlag, string RegisterListSortingId)
        {
            var Result = "";
            var Message = "";
            if (DeleteFlag == "ViewLevel1")
            {
                //string querydata = "select * from tblRegisterListList where RegisterListSortingId='" + ViewState["ViewFordelete1"] + "'";
               // string querydata = "select * from tblRegisterListSorting where RegisterListSortingId='" + RegisterListSortingId + "'";
                string querydata = "select 'X' as DeleteFlag from tblRegisterListSorting where RegisterListSortingId='" + RegisterListSortingId + "'";
                DataTable dtdata = databaseCon.GetData(querydata);
                if (dtdata.Rows.Count != 0)
                { 
                    string query = "delete  from tblRegisterListSorting where  RegisterListSortingId='" + RegisterListSortingId + "'";
                    DataTable dt = databaseCon.GetData(query);

 
                    Result = "Success";
                    Message = "Record Deleted SuccessFully ";
                    }

                else
                    {
 
                        Result = "Failed";
                        Message = "Record Not Found";
                    }


 
            }

            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }
        public class RegisterColumnsViewModel
        {
            public string RegisterListSortingId { get; set; }
            public string DisplayColumnName { get; set; }
            public string IsNA { get; set; }
            public string ClientId { get; set; }
            public string ItemOrder { get; set; }

        }

    }
}