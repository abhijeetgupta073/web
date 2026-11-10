using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using DocumentFormat.OpenXml.Office2010.Excel;


namespace Scaffold.Controllers
{
    public class ShiftAllocationController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: ShiftAllocation
        public ActionResult ShiftAllocation()
        {
            return View();
        }

        public ActionResult GetShiftAllocationData(string id)
        {
            string query = "";
            query = "exec [Pro_TBLShiftAllocation_GET] '','" + databaseCon.Decrypt(id) + "'";
            var getData = dbContext.Database.SqlQuery<ShiftAllocationViewModel>(query).ToList();
            return Json(getData, JsonRequestBehavior.AllowGet);

        }

        //public JsonResult CompanyDropdown()
        //{
        //    var getData = dbContext.Database.SqlQuery<ClientViewModel>("select distinct(Name+' '+'('+status+')') as name,ClientId from tblclient where ClientId!='1' Order by name").ToList();
        //    return Json(getData, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult SaveShiftAllocation(string ShiftAllocationId, string ShiftAllocation, string id, string UID,string Status)
        {
            string query = "";
            DataTable dt;
            string Result = "";
            string Message = "";

            if (ShiftAllocationId == "0")
            {
                query = "exec Pro_TBLShiftAllocation_SAVE '0','" + ShiftAllocation.Replace("'", "''").Trim() + "','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "','" + databaseCon.Decrypt(id) + "','" + Status + "'";
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
                    Message = "Syntax Error. Contact administrator ";
                }

            }
            else
            {
                query = "exec Pro_TBLShiftAllocation_SAVE '" + ShiftAllocationId + "','" + ShiftAllocation.Replace("'", "''").Trim() + "','" + databaseCon.Decrypt(UID) + "','" + DateTime.Now + "','" + databaseCon.Decrypt(id) + "','" + Status + "'";
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
                    Message = "Syntax Error. Contact administrator ";
                }
            }

            var response = new { Result = Result, Message = Message };
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        public JsonResult DeleteShiftAllocation(string ShiftAllocationId, string id)
        {
            string query = "";
            DataTable dt;
            string Result = "";
            string Message = "";
            query = "exec Pro_TBLShiftAllocation_DEL '" + ShiftAllocationId + "','" + databaseCon.Decrypt(id) + "'";
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
                Message = "Syntax Error. Contact administrator ";
            }

            var response = new { Result = Result, Message = Message };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public class ShiftAllocationViewModel
        {
            public Nullable<int> ShiftAllocationId { get; set; }
            public string ShiftAllocation { get; set; }
            public string Name { get; set; }
            public Nullable<int> ClientID { get; set; }
            public string Status { get; set; }
        }
    }
}