using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;

namespace Scaffold.Controllers
{
    public class ScaffoldMaterialListPhase3Controller : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: ScaffoldMaterialListPhase2
        public ActionResult ScaffoldMaterialListPhase3()
        {
            string query;
            DataTable dt;
            // query = "exec Pro_TBLREGISTER_GET '" + databaseCon.Decrypt(Request.QueryString["RID"]) + "','" + databaseCon.Decrypt(Request.QueryString["id"]) + "','',''";
            query = "select SiteLocId from tblRegister where ClientID='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and RegisterID='" + databaseCon.Decrypt(Request.QueryString["RID"]) + "'";
            dt = databaseCon.GetData(query);
            if (dt.Rows.Count != 0)
            {
                string Siteid = Convert.ToString(dt.Rows[0]["SiteLocID"]);
                ViewBag.SiteID = databaseCon.Encrypt(Siteid);
            }
            query = "exec Pro_UpdateRegisterForSiteMaterial '" + databaseCon.Decrypt(Request.QueryString["RID"]) + "'";
            //dt = databaseCon.GetData(query);
            var result = dbContext.Database.ExecuteSqlCommand(query);
            DataTable dtHireInventory = databaseCon.GetData("select isnull(HireInventory,0) as HireInventory from tblClient where clientid='" + (databaseCon.Decrypt(Request.QueryString["id"])) + "'");
            if (dtHireInventory.Rows.Count > 0)
            {
                ViewBag.HireInventory = dtHireInventory.Rows[0]["HireInventory"];
            }

            query = "Select convert(decimal(18, 2),SUM(TotalDailyHire))TotalDailyHireValue from ( select DeliveryAll.FromId ,DeliveryAll.ToId,MaterialType,ItemCode,ItemDesc , DeliveryAll.InventoryId,cast(DeliveryAll.DeliveryAvailableQuantity as int) as DeliveryAvailableQuantity,isnull(cast(ReturnAll.ReturnALLQuantity as int), 0) as ReturnALLQuantity,isnull(cast(DamagedAll.DamagedAllQuantity as int), 0) as DamagedALLQuantity,cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0) - ISNULL(DamagedAll.DamagedAllQuantity, 0)) as int)as AvailableQuantity,DeliveryAll.ItemWeight,cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0) - ISNULL(DamagedAll.DamagedAllQuantity, 0)) as int) * ItemWeight as TotalWeight,DeliveryAll.SubCategoryID  ,HireMeasurement, CASE WHEN HireMeasurement = 'Tonnage' THEN convert(decimal(18, 4),((ItemWeight) / 1000) * (isnull(HireRate, 0) / 7.0)) WHEN HireMeasurement = 'Each' THEN convert(decimal(18, 4),(isnull(HireRate, 0) / 7.0)) END as DailyHireRate,CASE WHEN HireMeasurement = 'Tonnage' THEN convert(decimal(18, 4),(((cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0) - ISNULL(DamagedAll.DamagedAllQuantity, 0)) as int)) * ItemWeight) / 1000) * (isnull(HireRate, 0) / 7.0)) WHEN HireMeasurement = 'Each' THEN convert(decimal(18, 4),(cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity, 0) - ISNULL(DamagedAll.DamagedAllQuantity, 0)) as int)) * (isnull(HireRate, 0) / 7.0))END as TotalDailyHire from(select FromId, ToId, MaterialType, ItemCode, ItemDesc, ItemWeight, tblTransactionHistDelivery.InventoryId, tblInventory.SubCategoryID, isnull(Sum(Quantity), 0) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Site' and[to] = 'Job' and TransType = 'Send' and ToId = '" + databaseCon.Decrypt(Request.QueryString["RID"]) + "' group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc, ItemWeight, SubCategoryID)DeliveryAll left join(select FromId, ToId, tblReturnInventoryDetails.InventoryId, isnull(Sum(Quantity), 0) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site' and FromId = '" + databaseCon.Decrypt(Request.QueryString["RID"]) + "' group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.InventoryId = ReturnAll.InventoryId left join(select FromId, ToId, tblDamagedInventoryDetails.InventoryId, ISNULL(Sum(DamagedQuantity), 0) as DamagedAllQuantity from tblDamagedInventoryDetails  join tblDamagedInventory on tblDamagedInventoryDetails.DamagedInvId = tblDamagedInventory.DamagedInvId where [FromName] = 'Job' and[toName] = 'Site' and FromId = '" + databaseCon.Decrypt(Request.QueryString["RID"]) + "' group by[FromName],[ToName], FromId, ToId, tblDamagedInventoryDetails.InventoryId) DamagedAll On DeliveryAll.FromId = DamagedAll.ToId and DeliveryAll.InventoryId = DamagedAll.InventoryId left join tblSubCategory on tblSubCategory.SubCategoryID = DeliveryAll.SubCategoryID where 1 = 1 )AllJobsHire ";
            dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                var TotalDailyHireValue = Convert.ToString(dt.Rows[0]["TotalDailyHireValue"]);
                if (TotalDailyHireValue == "")
                {
                    // txtTotalDailyHireCost.Text = "";
                    ViewBag.TotalDailyHireCost = "";
                }
                else
                {
                    ViewBag.TotalDailyHireCost = ("$" + TotalDailyHireValue);
                    //txtTotalDailyHireCost.Text = ("$" + TotalDailyHireValue);
                }
            }


            return View();
        }
        public ActionResult MaterialDropDown(string id)
        {
            var getMaterialType = dbContext.Database.SqlQuery<InventoryViewModel>("select distinct MaterialType from tblInventory  where clientid = " + databaseCon.Decrypt(id) + " order by MaterialType").ToList();
            return Json(getMaterialType, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetTotalTotalTonnage(string ClientID, string RID,string ddlMaterialType)
        {
            var TotalItemsValues = "";
            var TotalTonnageValues = "";
            string addquery = "";

            if (ddlMaterialType != "" && ddlMaterialType != "All")
            {
                addquery = " and tblPipelineRequest.MaterialType='" + ddlMaterialType + "' ";

            }

            //string query = "select FORMAT(CONVERT(int,(Sum(Quantity))),'N0') as TotalItems,convert(decimal(18,2),Sum(Quantity * ItemWeight)/1000) as TotalTonnage from (select* from tblPipelineRequest where JobId = '" + databaseCon.Decrypt(RID) + "')  tblPipelineRequest join Vw_ThirdPartyInventory on tblPipelineRequest.InventoryID = Vw_ThirdPartyInventory.InventoryId  where tblPipelineRequest.ClientID = " + databaseCon.Decrypt(ClientID) + "";

            //string query = "select FORMAT(CONVERT(int, (Sum(Quantity))),'N0') as TotalItems,convert(decimal(18, 2), Sum(Quantity * ItemWeight) / 1000) as TotalTonnage from(select * from tblPipelineRequest where JobId = '" + databaseCon.Decrypt(RID) + "')  tblPipelineRequest join Vw_ThirdPartyInventory on tblPipelineRequest.InventoryID = Vw_ThirdPartyInventory.InventoryId  where tblPipelineRequest.ClientID = " + databaseCon.Decrypt(ClientID) + addquery + "";

            string query = "select FORMAT(CONVERT(int, (Sum(tblPipelineRequest.Quantity))),'N0') as TotalItems, " +
                  "convert(decimal(18, 2), Sum(tblPipelineRequest.Quantity * Vw_ThirdPartyInventory.ItemWeight) / 1000) as TotalTonnage " +
                  "from tblPipelineRequest " +
                  "join Vw_ThirdPartyInventory on tblPipelineRequest.InventoryID = Vw_ThirdPartyInventory.InventoryId " +
                  "where tblPipelineRequest.JobId = '" + databaseCon.Decrypt(RID) + "' " +
                  "and tblPipelineRequest.ClientID = " + databaseCon.Decrypt(ClientID) + addquery;

            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                TotalItemsValues = dt.Rows[0]["TotalItems"].ToString();
                TotalTonnageValues = dt.Rows[0]["TotalTonnage"].ToString();
            }

            var Result = "TotalItemsValues" + TotalItemsValues + "|" + "TotalTonnageValues" + TotalTonnageValues;
            return Json(new { TotalItemsValues = TotalItemsValues, TotalTonnageValues = TotalTonnageValues });
        }
        //public ActionResult GetInventoryData(string id, string RID, string ddlMaterialType, string fromid)
        //{


        //    string addquery = "";
        //    string query;
        //    DataTable dt;


        //    if (ddlMaterialType != "" && ddlMaterialType != "All")
        //    {
        //        addquery = " and Vw_ThirdPartyInventory.MaterialType='" + ddlMaterialType + "' ";

        //    }
        //    query = "select Vw_ThirdPartyInventory.MaterialType,Vw_ThirdPartyInventory.ItemCode,Vw_ThirdPartyInventory.ItemDesc, Quantity ,ItemWeight,(Quantity*ItemWeight) as TotalWeight  from(select* from tblPipelineRequest where JobId = '" + databaseCon.Decrypt(RID) + "') tblPipelineRequest Left join(select * from Vw_ThirdPartyInventory where clientid = '" + databaseCon.Decrypt(id) + "') Vw_ThirdPartyInventory on tblPipelineRequest.InventoryID = Vw_ThirdPartyInventory.InventoryId where 1 = 1 " + addquery + " and Quantity!=0 order by MaterialType,isnull(Vw_ThirdPartyInventory.DisplaySortOrder,9999999),ItemCode";
        //    var GetData = dbContext.Database.SqlQuery<ScaffoldMaterialListPhase2ViewModel>(query).ToList();
        //    return Json(GetData, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult GetInventoryData(string id, string RID, string ddlMaterialType, string fromid)
        {
            string addquery = "";

            if (ddlMaterialType != "" && ddlMaterialType != "All")
            {
                addquery = " and v.MaterialType='" + ddlMaterialType + "' ";
            }

            string query = "select v.MaterialType, v.ItemCode, v.ItemDesc, p.Quantity, v.ItemWeight, (p.Quantity * v.ItemWeight) as TotalWeight " +
                           "from tblPipelineRequest p " +
                           "left join Vw_ThirdPartyInventory v on p.InventoryID = v.InventoryId and v.ClientId = '" + databaseCon.Decrypt(id) + "' " +
                           "where p.JobId = '" + databaseCon.Decrypt(RID) + "' " + addquery + " and p.Quantity != 0 " +
                           "order by v.MaterialType, isnull(v.DisplaySortOrder, 9999999), v.ItemCode";

            var GetData = dbContext.Database.SqlQuery<ScaffoldMaterialListPhase2ViewModel>(query).ToList();
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult btnoptimizeyes_Click(string id, string RID, string ddlMaterialType)
        {
            var Result = "";
            var Message = "";
            string addqury = "";
            if (ddlMaterialType != "" && ddlMaterialType != "All")
            {
                addqury = " and MaterialType='" + ddlMaterialType + "' ";

            }
            string checkquery = "select Sum(cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity,0)) as int)) as TotAvailableQuantity  from (select FromId, ToId, MaterialType, ItemCode, ItemDesc, ItemWeight, tblTransactionHistDelivery.InventoryId, isnull(Sum(Quantity),0) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Site' and[to] = 'Job' and TransType = 'Send' and ToId =  '" + databaseCon.Decrypt(RID) + "' group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc, ItemWeight)DeliveryAll left join(select FromId, ToId, tblReturnInventoryDetails.InventoryId, isnull(Sum(Quantity),0) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site' and FromId = '" + databaseCon.Decrypt(RID) + "' group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.InventoryId = ReturnAll.InventoryId";
            DataTable dtcheck = databaseCon.GetData(checkquery);
            if (dtcheck.Rows.Count > 0)
            {
                int totalava = Convert.ToInt32(dtcheck.Rows[0]["TotAvailableQuantity"]);
                if (totalava > 0)
                {

                    string qury = "insert into tblOptimizedInventory(InventoryId,OptimizedQuantity,ClientId,RegisterId)select DeliveryAll.InventoryId,cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity,0)- ISNULL(DamagedAll.DamagedAllQuantity,0)) as int) as AvailableQuantity," + databaseCon.Decrypt(id) + "," + databaseCon.Decrypt(RID) + "  from (select FromId, ToId, MaterialType, ItemCode, ItemDesc, ItemWeight, tblTransactionHistDelivery.InventoryId, isnull(Sum(Quantity),0) as DeliveryAvailableQuantity from tblTransactionHistDelivery join tblInventory on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId where [From] = 'Site' and[to] = 'Job' and TransType = 'Send' and ToId = '" + databaseCon.Decrypt(RID) + "' group by[From],[To], FromId, ToId, tblTransactionHistDelivery.InventoryId, MaterialType, ItemCode, ItemDesc, ItemWeight)DeliveryAll left join(select FromId, ToId, tblReturnInventoryDetails.InventoryId, isnull(Sum(Quantity),0) as ReturnAllQuantity from tblReturnInventoryDetails  join tblReturnInventory on tblReturnInventoryDetails.ReturnInvId = tblReturnInventory.ReturnInvId where [From] = 'Job' and[to] = 'Site' and FromId = '" + databaseCon.Decrypt(RID) + "' group by[From],[To], FromId, ToId, tblReturnInventoryDetails.InventoryId)ReturnAll On DeliveryAll.FromId = ReturnAll.ToId and DeliveryAll.InventoryId = ReturnAll.InventoryId left join (select FromId, ToId, tblDamagedInventoryDetails.InventoryId, ISNULL(Sum(DamagedQuantity), 0) as DamagedAllQuantity from tblDamagedInventoryDetails  join tblDamagedInventory on tblDamagedInventoryDetails.DamagedInvId = tblDamagedInventory.DamagedInvId where [FromName] = 'Job' and[toName] = 'Site' and FromId = '" + databaseCon.Decrypt(RID) + "' group by[FromName],[ToName], FromId, ToId, tblDamagedInventoryDetails.InventoryId) DamagedAll On DeliveryAll.FromId = DamagedAll.ToId and DeliveryAll.InventoryId = DamagedAll.InventoryId where 1=1  and cast((DeliveryAll.DeliveryAvailableQuantity - Isnull(ReturnAll.ReturnALLQuantity,0)) as int)>0 " + addqury + " order by MaterialType,ItemCode";
                    int freezing = databaseCon.RunCmdID(qury);
                    if (freezing > 0)
                    {
                        string query = "update tblregister set optimizestatus='Green' where registerid=" + databaseCon.Decrypt(RID);
                        int updatestatus = databaseCon.RunCmdID(query);
                        //Session["Insert"] = "Insert";
                        //Response.Redirect("FinalMaterialList.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RID=" + Request.QueryString["RID"]);
                        Result = "Success";
                        Message = "Record Insert Successfully.";
                    }
                    else
                    {
                        //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                        //art1.Style.Add("display", "block");
                        //alertlabel.Text = "<strong>Alert ! </strong>There is no data to finalise.";
                        //freezepopup.Style.Add("display", "none");
                        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp", "<script>hideAlert();</script>", false);
                        Result = "Failed";
                        Message = "There is no data to finalise.";
                    }
                }
                else
                {
                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Alert ! </strong>There is no any Material present on job to finalise.";
                    //freezepopup.Style.Add("display", "none");

                    Result = "Failed";
                    Message = "There is no any Material present on job to finalise.";
                }
            }

            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        /* Added by Neeraj joshi on 22 -08 - 2024*/
        public ActionResult ConvertPipeLineJob(string id, string UID, string RegisterID, string Siteid, string startdate)
        {
            var Result = "";
            var Message = "";
            string NewRegId = "";
            var ClientId = databaseCon.Decrypt(id);
            var UserId = databaseCon.Decrypt(UID);
            var RegisterId = databaseCon.Decrypt(RegisterID);
            var SiteLocationId = databaseCon.Decrypt(Siteid);
            var StartDate = (startdate);
            string query1 = "exec Pro_ConvertPipelineToLiveJob " + "'" + ClientId + "'," + "'" + SiteLocationId + "'," + "'" + UserId + "'," + "'" + RegisterId + "'," + "'" + StartDate + "'";
            DataTable dt1 = databaseCon.GetData(query1);

            if (dt1.Rows.Count != 0)
            {
                if (Convert.ToInt32(dt1.Rows[0][0]) == -1)
                {
                    Result = "Failed";
                    Message = dt1.Rows[0][1].ToString().Replace("|", "<br>");
                }
                else if (Convert.ToInt32(dt1.Rows[0][0]) == -2)
                {
                    Result = "Insufficient";
                    Message = dt1.Rows[0][1].ToString().Replace("|", "<br>");

                }
                else if (Convert.ToInt32(dt1.Rows[0][0]) != -1 && Convert.ToInt32(dt1.Rows[0][0]) != 0)
                {
                    string queryforupdatetonnage = "exec Pro_UpdateRegisterForSiteMaterial '" + databaseCon.Decrypt(RegisterID) + "'";
                    databaseCon.GetData(queryforupdatetonnage);
                    NewRegId = dt1.Rows[0][0].ToString();
                    Result = "Success";
                    Message = dt1.Rows[0][1].ToString();
                }
            }
            else
            {
                Result = "Failed";
                Message = "Convert  Copy Job Process Failed. Please Contact Admin";
            }

            return Json(new { Result = Result, Message = Message, NewRegId = NewRegId }, JsonRequestBehavior.AllowGet);
        }

        //Added on 3March2025
        public ActionResult checkNoInventoryRequired(string id, string UID, string RegisterID, string Siteid)
        {
            string NoInventoryquery = "select isnull(NoInventory,0) as NoInventory from tblRegister where SiteLocID='" + databaseCon.Decrypt(Siteid) + "'and RegisterId='" + RegisterID + "'";
            DataTable NoInventorydt = databaseCon.GetData(NoInventoryquery);
            var NoInventory = NoInventorydt.Rows[0][0];
            return Json(new { Result = NoInventory });
        }

    }
}