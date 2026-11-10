using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scaffold.App_Code;
using Scaffold_DataAccess;

namespace Scaffold.Controllers
{
    public class FinalMaterialListController : Controller
    {
        ScaffoldEntities dbcontext = new ScaffoldEntities();
        // GET: FinalMaterialList
        public ActionResult FinalMaterialList()
        {
            string querytotal = "select SUM(optimizedQuantity) as Quantity ,cast(SUM(TotalWeight) / 1000 as decimal(18, 2)) as TotalWeight from(select optimizedQuantity, caST((optimizedQuantity * ItemWeight) as decimal(18, 2)) as TotalWeight from tblOptimizedInventory join tblInventory on tblOptimizedInventory.inventoryid = tblInventory.InventoryID where Registerid = '" + databaseCon.Decrypt(Request.QueryString["RID"]) + "' and tblOptimizedInventory.Clientid = '" + databaseCon.Decrypt(Request.QueryString["id"]) + "' )TotalQuery";
            DataTable dt = databaseCon.GetData(querytotal);
            if (dt.Rows.Count > 0)
            {
                //txtQuantity.Text = Convert.ToString(dt.Rows[0]["Quantity"]);
                //txtTotalweight.Text = Convert.ToString(dt.Rows[0]["TotalWeight"]);
                ViewBag.Quantity= Convert.ToString(dt.Rows[0]["Quantity"]);
                ViewBag.TotalWeight = Convert.ToString(dt.Rows[0]["TotalWeight"]);
            }
            return View();
        }
        public class FinalMaterialViewModel {
           public Nullable<int> optimizedQuantity { get; set; }
            public string MaterialType { get; set; }
            public string ItemCode { get; set; }
            public string ItemDesc { get; set; }
            public Nullable<double> ItemWeight { get; set; }
            public Nullable<Decimal> TotalWeight { get; set; }
      
        }
        public ActionResult GetMaterialFinalizedData(string id,string RID) {
            // string query = "select optimizedQuantity,MaterialType,ItemCode,ItemDesc,ItemWeight,CAST((optimizedQuantity*ItemWeight) as decimal(18,2)) as TotalWeight from tblOptimizedInventory join tblInventory on tblOptimizedInventory.inventoryid=tblInventory.InventoryID where Registerid=" + databaseCon.Decrypt(RID) + "and tblOptimizedInventory.Clientid=" + databaseCon.Decrypt(id) + "order by MaterialType,ItemCode";

            String query = "select optimizedQuantity,MaterialType,ItemCode,ItemDesc,ItemWeight,CAST((optimizedQuantity*ItemWeight) as decimal(18,2)) as TotalWeight from tblOptimizedInventory join tblInventory on tblOptimizedInventory.inventoryid=tblInventory.InventoryID where Registerid=" + databaseCon.Decrypt(RID) + "and tblOptimizedInventory.Clientid=" + databaseCon.Decrypt(id) + "order by MaterialType,isnull(DisplaySortOrder,9999999),ItemCode";

            var GetData = dbcontext.Database.SqlQuery<FinalMaterialViewModel>(query);
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult btndltoptimizeyes_Click(string id, string RID)
        {
            var Result = "";
            var Message = "";
            string query = "delete from tblOptimizedInventory where ClientId=" + databaseCon.Decrypt(id) + "and RegisterId=" + databaseCon.Decrypt(RID);
            int deletefreezing = databaseCon.RunCmdID(query);
            if (deletefreezing > 0)
            {
                string qury = "update tblregister set optimizestatus='Red' where registerid=" + databaseCon.Decrypt(RID);
                int updatestatus = databaseCon.RunCmdID(qury);
                Session["Delete"] = "Delete";
                Result = "Success";
                Message = "Data Delete Successfully.";
                // Response.Redirect("ScaffoldMaterialListPhase2.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RID=" + Request.QueryString["RID"]);
            }
            else {
                Result = "Failed";
                Message = "Data Not Delete.";
            }
            return Json(new { Result=Result,Message=Message}, JsonRequestBehavior.AllowGet);
        }
    }
}