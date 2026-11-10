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
    public class PipelineRequestController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: PipelineRequest      
        public ActionResult PipelineRequest(string id, string UID, string UserRole, string fromtocheck, string from, string fromid, string ToId, string page, string To, string ReqId, string ReqStatus)
        {
            CommonInventoryColumnViewMoidel CommonInventoryColumn = new CommonInventoryColumnViewMoidel();

            string query = "";
            DataTable dt;
            string fromName = "";
            string ToName = "";
            int Siteid = 0;
            string RequestedPerson = "";
            string Description = "";
            string Supplier = "";
            string Comment = "";




            if (databaseCon.Decrypt(fromtocheck) == "1")
            {
                query = "select distinct SiteLocID,SiteLocation from tblSiteLocation where SiteLocation <>'' and SiteLocation is not null and  ClientId='" + databaseCon.Decrypt(id) + "'and SiteLocId=" + databaseCon.Decrypt(fromid);
                dt = databaseCon.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    ToName = "Depot";
                    fromName = dt.Rows[0]["SiteLocation"].ToString();
                    Siteid = Convert.ToInt32(dt.Rows[0]["SiteLocID"]);

                }


            }
            else if (databaseCon.Decrypt(fromtocheck) == "2")
            {

                query = "select distinct SiteLocID,SiteLocation from tblSiteLocation where SiteLocation <>'' and SiteLocation is not null and  ClientId='" + databaseCon.Decrypt(id) + "'and SiteLocId=" + databaseCon.Decrypt(ToId);
                dt = databaseCon.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    ToName = dt.Rows[0]["SiteLocation"].ToString();
                    Siteid = Convert.ToInt32(dt.Rows[0]["SiteLocID"]);
                }


                query = "select case when isnull(Description,'')= '' then Convert(varchar, RIGHT('000000' + CONVERT(varchar, ScaffRef), 6)) else Convert(varchar, RIGHT('000000' + CONVERT(varchar, ScaffRef), 6)) + ' - ' + Substring(Description, 1, 80) end as ScaffRef  from tblregister where RegisterId =" + databaseCon.Decrypt(fromid);

                dt = databaseCon.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    fromName = dt.Rows[0]["ScaffRef"].ToString();
                }

            }
            else if (databaseCon.Decrypt(fromtocheck) == "3")
            {
                ToName = "ThirdParty";
                fromName = "Depot";
                Siteid = Convert.ToInt32(0);

            }

            if (databaseCon.Decrypt(fromid) != "0" && fromid != null)
            {

                query = "select PipeLineDesc from tblPipelineRequest where JobId=" + databaseCon.Decrypt(fromid);
                dt = databaseCon.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    //RequestedPerson = dt.Rows[0]["fromperson"].ToString();
                    //Supplier = dt.Rows[0]["Supplier"].ToString();
                    ViewBag.PipeDescription = dt.Rows[0]["PipeLineDesc"].ToString();
                    //Description = dt.Rows[0]["PipeLineDesc"].ToString();
                    //Comment = dt.Rows[0]["Comment"].ToString();
                    //CommonInventoryColumn.RequestedDate = Convert.ToDateTime(dt.Rows[0]["fromtime"].ToString());
                }

            }
            else
            {
                query = "select (select firstName from tbluser where UserId='" + databaseCon.Decrypt(UID) + "') as fromperson ,CONVERT(VARCHAR(11),getdate(),106) AS fromtime";
                dt = databaseCon.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    RequestedPerson = dt.Rows[0]["fromperson"].ToString();
                    CommonInventoryColumn.RequestedDate = Convert.ToDateTime(dt.Rows[0]["fromtime"].ToString());
                }

            }


            var d = databaseCon.Decrypt(page);
            CommonInventoryColumn.ClientID = databaseCon.Decrypt(id);
            CommonInventoryColumn.UserID = databaseCon.Decrypt(UID);
            CommonInventoryColumn.pageID = databaseCon.Decrypt(page);
            CommonInventoryColumn.FromName = fromName;
            CommonInventoryColumn.FromNameID = databaseCon.Decrypt(fromid);
            CommonInventoryColumn.ToName = ToName;
            CommonInventoryColumn.ToNameId = databaseCon.Decrypt(ToId);
            CommonInventoryColumn.RequestedPerson = RequestedPerson;
            CommonInventoryColumn.Description = Description;
            CommonInventoryColumn.FromtocheckID = databaseCon.Decrypt(fromtocheck);
            CommonInventoryColumn.RequestStatus = databaseCon.Decrypt(ReqStatus);
            CommonInventoryColumn.RequestId = databaseCon.Decrypt(ReqId);
            CommonInventoryColumn.Supplier = Supplier;
            CommonInventoryColumn.Comment = Comment;
            return View(CommonInventoryColumn);
        }

        public ActionResult MaterialDropDown(string id)
        {
            var getMaterialType = dbContext.Database.SqlQuery<InventoryViewModel>("select distinct MaterialType from tblinventory where MaterialType <>'' and MaterialType is not null and  ClientId='" + databaseCon.Decrypt(id) + "' order by MaterialType").ToList();
            return Json(getMaterialType, JsonRequestBehavior.AllowGet);

        }

        public ActionResult dropdownSupplier(string id)
        {

            var GetData = dbContext.Database.SqlQuery<DropDownViewModel>("select distinct Supplier from TblTransaction where ClientId='" + databaseCon.Decrypt(id) + "' and  Supplier is not null and Supplier!= '' Order by Supplier").ToList();
            return Json(GetData, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetHistoricalJobMaterialList(string ToID, string FromId)
        {


            var RequestInventoryData = dbContext.Database.SqlQuery<CopyHistoricalJobMaterialList>("select Distinct TOId as JobId, Location,Isnull(BuildingNo,'') AS BuildingNo ,isnull(AssetNo,'') as AssetNo, Assetdesc, right('000000' + Convert(varchar, Scaffref), 6) as Scaffref,case when Vw_tblregister.OptimizeStatus is null or Vw_tblregister.OptimizeStatus='Red' then 'Not Finalised' else 'Finalised' end as OptimizeStatus  from tblTransactionHistDelivery left join Vw_tblregister on Vw_tblregister.RegisterId = tblTransactionHistDelivery.ToId and FromId=Vw_tblregister.SiteLocId  where fromId = '" + databaseCon.Decrypt(ToID) + "'and [From]='Site' and ToId !=" + databaseCon.Decrypt(FromId)).ToList();
            return Json(RequestInventoryData, JsonRequestBehavior.AllowGet);
        }


        public int SetJobData(int UID, int id, int Toid, int jobid, int fromid)
        {
            string query = "insert into tblPipelineRequest(UserId, ClientId, InventoryId,Quantity, siteId, JobId, CreatedBy, CreatedOn,ItemCode,ItemDesc,MaterialType)(select '" + UID + "','" + id + "',VW_SiteInventory.InventoryID, OptimizedQuantity as Quantity,'" + Toid + "','" + fromid + "',(Select Createdby from tblUser Where Userid = " + UID + "),getdate(),ItemCode,ItemDesc,MaterialType from VW_SiteInventory join tblInventory on vw_SiteInventory.InventoryID = tblInventory.InventoryID join tblOptimizedInventory On Vw_SiteInventory.InventoryId = tblOptimizedInventory.InventoryId where VW_SiteInventory.siteId = '" + Toid + "' and tblOptimizedInventory.RegisterId = '" + jobid + "' and SendQuantity is not null)";
            int Tempinsert = databaseCon.RunCmd(query);
            return Tempinsert;
        }

        public ActionResult InsertHistoricalJobMaterialListData(string UID)
        {
            string query = "select UserId,AvailableQuantity,Quantity from tblPipelineRequest join Vw_ThirdPartyInventory on tblPipelineRequest.InventoryId=Vw_ThirdPartyInventory.InventoryID  where UserId='" + databaseCon.Decrypt(UID) + "'";
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                int flagvalidQty = 1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToInt32(dt.Rows[i]["AvailableQuantity"]) < Convert.ToInt32(dt.Rows[i]["Quantity"]))
                    {
                        flagvalidQty = 0;
                    }
                }

                return Json(new { result = "Success", flagvalidQty = flagvalidQty });
            }
            else
            {

                return Json(new { result = "Failed", Message = "No Record Found." });
            }


        }

        public class ItemCodeList
        {
            public string MaterialType { get; set; }
            public string ItemCode { get; set; }
            public string ItemDesc { get; set; }
            public decimal? AvailableQuantity { get; set; }
            public int? Quantity { get; set; }

            public int? RequestQuantity { get; set; }
        }
        public ActionResult btnConfirmAndBack_Click(string id, string UID, string ReqId, string page, string txtdesc, string UserRole, string hdnflagforjobinsertion, string FromId, string ToId)
        {
            if (databaseCon.Decrypt(page) == "2")
            {
                List<ItemCodeList> ItemCodeList = new List<ItemCodeList>();
                bool flag = false;
                //string query = "select ClientId, tblPipelineRequest.InventoryId,UserId,ISNULL(AvailableQuantity,0) AvailableQuantity,Quantity, Quantity-ISNULL(AvailableQuantity,0) as RequestQuantity,MaterialType,ItemCode,ItemDesc from tblPipelineRequest Left join (Select * from Vw_SiteInventory  where SiteId  = '" + databaseCon.Decrypt(ToId) + "')  Vw_SiteInventory on tblPipelineRequest.InventoryId=Vw_SiteInventory.InventoryID  where UserId='" + databaseCon.Decrypt(UID) + "' and JobId  = '" + databaseCon.Decrypt(FromId) + "' and  tblPipelineRequest.SiteId  = '" + databaseCon.Decrypt(ToId) + "' And Quantity > ISNULL(AvailableQuantity,0) ";

                string query = "select p.ClientId, p.InventoryId, p.UserId, ISNULL(s.AvailableQuantity,0) AvailableQuantity, p.Quantity, " +
               "p.Quantity - ISNULL(s.AvailableQuantity,0) as RequestQuantity, p.MaterialType, p.ItemCode, p.ItemDesc " +
               "from tblPipelineRequest p " +
               "left join Vw_SiteInventory s on p.InventoryId = s.InventoryID and s.SiteId = '" + databaseCon.Decrypt(ToId) + "' " +
               "where p.UserId = '" + databaseCon.Decrypt(UID) + "' " +
               "and p.JobId = '" + databaseCon.Decrypt(FromId) + "' " +
               "and p.SiteId = '" + databaseCon.Decrypt(ToId) + "' " +
               "and p.Quantity > ISNULL(s.AvailableQuantity,0)";
                DataTable dt = databaseCon.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (Convert.ToInt32(dt.Rows[i]["AvailableQuantity"]) < Convert.ToInt32(dt.Rows[i]["Quantity"]))
                        {
                            flag = true;
                            ItemCodeList ListData = new ItemCodeList()
                            {
                                MaterialType = dt.Rows[i]["MaterialType"].ToString(),
                                ItemCode = dt.Rows[i]["ItemCode"].ToString(),
                                ItemDesc = dt.Rows[i]["ItemDesc"].ToString(),
                                AvailableQuantity = Convert.ToDecimal(dt.Rows[i]["AvailableQuantity"]),
                                Quantity = Convert.ToInt32(dt.Rows[i]["Quantity"]),
                                RequestQuantity = Convert.ToInt32(dt.Rows[i]["RequestQuantity"]),
                            };
                            ItemCodeList.Add(ListData);
                        }
                    }
                    if (flag && ItemCodeList != null && ItemCodeList.Count > 0)
                    {
                        return Json(new { Data = ItemCodeList, result = "Failed", Message = "Invalid Quantity" });
                    }
                }
                else
                {
                    if (flag != true)
                    {
                        string query1 = "select ClientId, tblPipelineRequest.InventoryId,UserId,ISNULL(AvailableQuantity,0) AvailableQuantity,Quantity,ItemCode,ItemDesc from tblPipelineRequest  join  Vw_SiteInventory on tblPipelineRequest.InventoryId=Vw_SiteInventory.InventoryID  where UserId='" + databaseCon.Decrypt(UID) + "' and JobId  = '" + databaseCon.Decrypt(FromId) + "' and  tblPipelineRequest.SiteId  = '" + databaseCon.Decrypt(ToId) + "' and  Vw_SiteInventory.SiteId  = '" + databaseCon.Decrypt(ToId) + "' ";
                        DataTable dt1 = databaseCon.GetData(query1);
                        if (dt1.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                if (Convert.ToInt32(dt1.Rows[i]["AvailableQuantity"]) < Convert.ToInt32(dt1.Rows[i]["Quantity"]))
                                {
                                    flag = true;
                                    return Json(new { result = "Failed", Message = "Please rectify the quantity marked in red." });
                                }
                            }
                            string query3 = "exec Pro_PipelineRequestInventory '" + databaseCon.Decrypt(UID) + "','" + databaseCon.Decrypt(id) + "'";
                            int execpro = databaseCon.RunCmdID(query3);
                            flag = true;
                        }
                        return Json(new { result = "Success", Message = "ScaffoldMaterialListPhase3" });


                    }
                }
            }
            return Json(new { result = "Success", Message = "ScaffoldMaterialListPhase3" });
        }
        public ActionResult griddata(string id, string Reqstid, string Material, string Page, string ToID, string UID, string fromid)
        {

            string addquery = "";
            if (Material != "" && Material != "All")
            {
                addquery = " and MaterialType='" + Material + "'";

            }

            //string query1 = "select Vw_ThirdPartyInventory.InventoryId, Vw_ThirdPartyInventory.MaterialType, Vw_ThirdPartyInventory.ItemCode,Vw_ThirdPartyInventory.ItemDesc,Quantity, cast(AvailableQuantity as int) as AvailableQuantity, '' as CheckQty from(select * from Vw_ThirdPartyInventory where ClientID = '" + id + "' " + addquery + ") Vw_ThirdPartyInventory Left join(select * from tblPipelineRequest where JobId = '" + fromid + "') tblPipelineRequest on Vw_ThirdPartyInventory.InventoryID = tblPipelineRequest.InventoryId order by case when Quantity is null then 1 else 0 end,Vw_ThirdPartyInventory.MaterialType, Vw_ThirdPartyInventory.ItemCode";
            // added  order  by DisplaySortOrder on 22-04-2025
            //string query1 = "select Vw_ThirdPartyInventory.InventoryId, Vw_ThirdPartyInventory.MaterialType, Vw_ThirdPartyInventory.ItemCode,Vw_ThirdPartyInventory.ItemDesc,Quantity, cast(AvailableQuantity as int) as AvailableQuantity, '' as CheckQty from(select * from Vw_ThirdPartyInventory where ClientID = '" + id + "' " + addquery + ") Vw_ThirdPartyInventory Left join(select * from tblPipelineRequest where JobId = '" + fromid + "') tblPipelineRequest on Vw_ThirdPartyInventory.InventoryID = tblPipelineRequest.InventoryId order by case when Quantity is null then 1 else 0 end,Vw_ThirdPartyInventory.MaterialType,isnull(Vw_ThirdPartyInventory.DisplaySortOrder,9999999), Vw_ThirdPartyInventory.ItemCode";

            string query1 = "select v.InventoryId, v.MaterialType, v.ItemCode, v.ItemDesc, p.Quantity, cast(v.AvailableQuantity as int) as AvailableQuantity ,'' as CheckQty " +
                   "from Vw_ThirdPartyInventory v " +
                   "left join tblPipelineRequest p on v.InventoryId = p.InventoryId and p.JobId = '" + fromid + "' " +
                   "where v.ClientID = '" + id + "' " + addquery + " " +
                   "order by case when p.Quantity is null then 1 else 0 end, v.MaterialType, isnull(v.DisplaySortOrder, 9999999), v.ItemCode";


            var RequestInventoryData = dbContext.Database.SqlQuery<SiteRequestViewModel>(query1).ToList();

            return Json(RequestInventoryData, JsonRequestBehavior.AllowGet);

        }

        public int insertion(int Qty, int OldQty, string InvId, string UID, string from, string fromid, string To, string ToId,
            string id, string TReqId, string ReqID, string Page, string tempval, string txtdesc, string Supplier, string txtComment,
            string MateralType, string ItemCode, string ItemDesc)
        {

            int Requestid = 0;

            if (Qty > 0)
            {
                // if not exists based on jobid and itemid  else update
                //string query = " if Not Exists(select * from tblPipelineRequest Where ItemCode='" + ItemCode + "' and JobId = '" + fromid + "')Begin insert into tblPipelineRequest(UserId, ClientId, InventoryId, siteId, JobId, Quantity, CreatedBy,CreatedOn, ItemCode, ItemDesc,MaterialType) values(" + UID + "," + id + "," + InvId + "," + ToId + "," + fromid + "," + Qty + ",(select EmailId from tblUser where UserId='" + UID + "'),'" + DateTime.Now + "','" + ItemCode.Replace("'", "''").Trim() + "','" + ItemDesc.Replace("'", "''").Trim() + "','" + MateralType.Replace("'", "''").Trim() + "') End Else  BEGIN update tblPipelineRequest Set Quantity='" + Qty + "', UpdatedBy = (select EmailId from tblUser  where UserId='" + UID + "'),UpdatedOn='" + DateTime.Now + "' Where ItemCode='" + ItemCode + "' and JobId = '" + fromid + "'End";
                //string query = " if Not Exists(select * from tblPipelineRequest Where ItemCode='" + ItemCode + "' and JobId = '" + fromid + "')Begin insert into tblPipelineRequest(UserId, ClientId, InventoryId, siteId, JobId, Quantity, CreatedBy,CreatedOn, ItemCode, ItemDesc,MaterialType,PipeLineDesc) values(" + UID + "," + id + "," + InvId + "," + ToId + "," + fromid + "," + Qty + ",(select EmailId from tblUser where UserId='" + UID + "'),'" + DateTime.Now + "','" + ItemCode.Replace("'", "''").Trim() + "','" + ItemDesc.Replace("'", "''").Trim() + "','" + MateralType.Replace("'", "''").Trim() + "','" + txtdesc.Replace("'", "''").Trim() + "') End Else  BEGIN update tblPipelineRequest Set Quantity='" + Qty + "', UpdatedBy = (select EmailId from tblUser  where UserId='" + UID + "'),UpdatedOn='" + DateTime.Now + "' Where ItemCode='" + ItemCode + "' and JobId = '" + fromid + "'End";

                string query = " if Not Exists(select 1 from tblPipelineRequest Where ItemCode='" + ItemCode + "' and JobId = '" + fromid + "')Begin insert into tblPipelineRequest(UserId, ClientId, InventoryId, siteId, JobId, Quantity, CreatedBy,CreatedOn, ItemCode, ItemDesc,MaterialType,PipeLineDesc) values(" + UID + "," + id + "," + InvId + "," + ToId + "," + fromid + "," + Qty + ",(select EmailId from tblUser where UserId='" + UID + "'),'" + DateTime.Now + "','" + ItemCode.Replace("'", "''").Trim() + "','" + ItemDesc.Replace("'", "''").Trim() + "','" + MateralType.Replace("'", "''").Trim() + "','" + txtdesc.Replace("'", "''").Trim() + "') End Else  BEGIN update tblPipelineRequest Set Quantity='" + Qty + "', UpdatedBy = (select EmailId from tblUser  where UserId='" + UID + "'),UpdatedOn='" + DateTime.Now + "' Where ItemCode='" + ItemCode + "' and JobId = '" + fromid + "'End";
                Requestid = databaseCon.RunCmdID(query);
            }
            //else
            //{
            //    string query1 = "update tblPipelineRequest Set Quantity='" + Qty + "',UpdatedBy=(select EmailId from tblUser where UserId='" + UID + "'),UpdatedOn='" + DateTime.Now + "' Where ItemCode='" + ItemCode + "' and JobId = '" + fromid + "'";
            //int TempTbl = databaseCon.RunCmdID(query1);

            //}                
            if (Qty == 0)
            {
                string query = "delete From tblPipelineRequest Where ItemCode='" + ItemCode + "' and JobId = '" + fromid + "'";
                Requestid = databaseCon.RunCmdID(query);
            }
            return Requestid;
        }
        public class CopyJobViewModel
        {
            public string ScaffRef { get; set; }
            public Nullable<int> JobId { get; set; }
        }

        public void updateDesc(string ReqId, string Desccription)
        {
            if (ReqId != "")
            {
                string updescqry = "update tblPipelineRequest set PipeLineDesc='" + Desccription.Replace("'", "''").Trim() + "' where JobId='" + ReqId + "'";
                int i = databaseCon.RunCmd(updescqry);
            }
            else
            {

            }

        }
        public void btnClearAllPipeLineData_Click(string ClientID, string ReqId)
        {
            string Query1 = "Delete From tblPipelineRequest where Clientid = '" + databaseCon.Decrypt(ClientID) + "' and  JobId='" + ReqId + "'";
            //Added By Neeraj Joshi On 22-aug-2024
            string Query2 = "Update tblRegister set PipelineStartDate ='1900-01-01' where Clientid = '" + databaseCon.Decrypt(ClientID) + "' and  RegisterId='" + ReqId + "'";
            int i = databaseCon.RunCmd(Query1);
            //Added By Neeraj Joshi On 22-aug-2024
            int j = databaseCon.RunCmd(Query2);

        }



        public ActionResult Back_Click(string txtdesc, string FromId)
        {
            if ((txtdesc != "") || (txtdesc == ""))
            {

                string updescqry1 = "update tblPipelineRequest set PipeLineDesc='" + txtdesc.Replace("'", "''").Trim() + "' where JobId='" + databaseCon.Decrypt(FromId) + "' and  isnull(PipeLineDesc,'') != '" + txtdesc + "'";
                DataTable dt = databaseCon.GetData(updescqry1);
            }

            return Json(new { result = "Success" });



        }



        public ActionResult PipelineDateCheck(string ReqId, string Toid)
        {

            string query = "select  Distinct RegisterId from tblRegister, tblPipelineRequest where tblRegister.registerid = tblPipelineRequest.jobid and tblRegister.RegisterId = " + ReqId + " and SiteLocId = " + Toid + "  and(PipelineStartDate = '01/01/1900' or PipelineStartDate is null)";
            DataTable dt = databaseCon.GetData(query);

            if (dt.Rows.Count > 0)
            {
                return Json(new { result = "Success" });
            }

            return Json(new { result = "Failed" });

        }




        public ActionResult GetPipelineDesc1(string FromId)
        {

            var PipelineDesc = "";

            string query = "select PipeLineDesc from tblPipelineRequest where JobId = " + databaseCon.Decrypt(FromId) + "";
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {

                PipelineDesc = dt.Rows[0]["PipeLineDesc"].ToString();
            }

            var Result = "PipelineDesc" + PipelineDesc;
            return Json(new { PipelineDesc = PipelineDesc });
        }

        public ActionResult GetTotalTotalTonnage(string ClientID, string ReqId, string JobId)
        {
            var TotalItemsValues = "";
            var TotalTonnageValues = "";
            string query = "";

            // Decrypt both ClientID and JobId
            string decryptedClientId = databaseCon.Decrypt(ClientID);
           // string decryptedJobId = databaseCon.Decrypt(JobId);


            query = " select ISNULL(FORMAT(CONVERT(int, (Sum(Quantity))),'N0'),0) as TotalItems,convert(decimal(18, 2), ISNULL(Sum(Quantity * ItemWeight) / 1000, 0)) as TotalTonnage    from tblPipelineRequest join tblinventory on tblinventory.InventoryID = tblPipelineRequest.InventoryId and  tblPipelineRequest.ClientId=" + decryptedClientId + " and tblPipelineRequest.jobid = " + JobId; // Using decryptedJobId


            // --- EXECUTION PART ---
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                TotalItemsValues = dt.Rows[0]["TotalItems"].ToString();
                TotalTonnageValues = dt.Rows[0]["TotalTonnage"].ToString();
            }

            return Json(new { TotalItemsValues = TotalItemsValues, TotalTonnageValues = TotalTonnageValues });
        }
    }
}