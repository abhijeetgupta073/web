using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Controls.Primitives;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;


namespace Scaffold.Controllers
{
    public class SiteRequestController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: SiteRequest
        public ActionResult SiteRequest(string id, string UID, string UserRole, string fromtocheck, string from, string fromid, string ToId, string page, string To, string ReqId, string ReqStatus)
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
            string fromtime = "";
            string RequiredDate = "";




            if (databaseCon.Decrypt(page) == "2")
            {
                //  string querey = "delete from tblRequestInventoryTemp where userid=" + databaseCon.Decrypt(UID) ; //Commented 3-Nov-2021 For Refresh Same Page
                string querey = "delete from tblRequestInventoryTemp where userid=" + databaseCon.Decrypt(UID) + "  and JobId != " + databaseCon.Decrypt(fromid) + "";
                int delete = databaseCon.RunCmdID(querey);
            }

            // if (databaseCon.Decrypt(Request.QueryString["fromtocheck"]) == "1")
            if (databaseCon.Decrypt(fromtocheck) == "1")
            {
                // string query5 = "select distinct SiteLocID,SiteLocation from tblSiteLocation where SiteLocation <>'' and SiteLocation is not null and  ClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "'and SiteLocId=" + databaseCon.Decrypt(Request.QueryString["fromid"]);
                query = "select distinct SiteLocID,SiteLocation from tblSiteLocation where SiteLocation <>'' and SiteLocation is not null and  ClientId='" + databaseCon.Decrypt(id) + "'and SiteLocId=" + databaseCon.Decrypt(fromid);
                dt = databaseCon.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    ToName = "Depot";
                    fromName = dt.Rows[0]["SiteLocation"].ToString();
                    Siteid = Convert.ToInt32(dt.Rows[0]["SiteLocID"]);

                    // txtto.Text = "Depot";
                    // txtfrom.Text = dt5.Rows[0]["SiteLocation"].ToString();
                }


            }
            // else if (databaseCon.Decrypt(Request.QueryString["fromtocheck"]) == "2")
            else if (databaseCon.Decrypt(fromtocheck) == "2")
            {
                // string query6 = "select distinct SiteLocID,SiteLocation from tblSiteLocation where SiteLocation <>'' and SiteLocation is not null and  ClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "'and SiteLocId=" + databaseCon.Decrypt(Request.QueryString["ToId"]);

                query = "select distinct SiteLocID,SiteLocation from tblSiteLocation where SiteLocation <>'' and SiteLocation is not null and  ClientId='" + databaseCon.Decrypt(id) + "'and SiteLocId=" + databaseCon.Decrypt(ToId);
                dt = databaseCon.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    // txtto.Text = dt6.Rows[0]["SiteLocation"].ToString();
                    ToName = dt.Rows[0]["SiteLocation"].ToString();
                    Siteid = Convert.ToInt32(dt.Rows[0]["SiteLocID"]);
                }

                // string query7 = "select Convert(varchar,RIGHT('000000'+CONVERT(varchar,ScaffRef),6))+' - '+Substring(Description,1,80) as ScaffRef  from tblregister where RegisterId=" + databaseCon.Decrypt(Request.QueryString["fromid"]);

                // query = "select Convert(varchar,RIGHT('000000'+CONVERT(varchar,ScaffRef),6))+' - '+Substring(Description,1,80) as ScaffRef  from tblregister where RegisterId=" + databaseCon.Decrypt(fromid);
                query = "select case when isnull(Description,'')= '' then Convert(varchar, RIGHT('000000' + CONVERT(varchar, ScaffRef), 6)) else Convert(varchar, RIGHT('000000' + CONVERT(varchar, ScaffRef), 6)) + ' - ' + Substring(Description, 1, 80) end as ScaffRef  from tblregister where RegisterId =" + databaseCon.Decrypt(fromid);

                dt = databaseCon.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    // txtfrom.Text = dt7.Rows[0]["ScaffRef"].ToString();
                    fromName = dt.Rows[0]["ScaffRef"].ToString();
                }

            }
            else if (databaseCon.Decrypt(fromtocheck) == "3")
            {
                ToName = "ThirdParty";
                fromName = "Depot";
                Siteid = Convert.ToInt32(0);

            }

            // if (databaseCon.Decrypt(Request.QueryString["ReqId"]) != "0" && Request.QueryString["ReqId"] != null)
            if (databaseCon.Decrypt(ReqId) != "0" && ReqId != null)
            {
                //  string querydata = "select TransDesc,fromperson,CONVERT(VARCHAR(11),fromtime,106) AS fromtime from tblTransaction where TransId=" + databaseCon.Decrypt(Request.QueryString["ReqId"]);
                //  DataTable dt1 = databaseCon.GetData(querydata);
                //  txtrequestedby.Text = dt1.Rows[0]["fromperson"].ToString();
                //  txtrequesteddate.SelectedDate = Convert.ToDateTime(dt1.Rows[0]["fromtime"].ToString());
                //  txtdesc.Text = dt1.Rows[0]["TransDesc"].ToString();

                query = "select TransDesc,Comment,Isnull(Supplier,'') as Supplier,fromperson,FORMAT(fromtime, 'dd-MMM-yyyy') fromtime,FORMAT(RequiredDate, 'dd-MMM-yyyy') RequiredDate from tblTransaction where TransId=" + databaseCon.Decrypt(ReqId);
                dt = databaseCon.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    RequestedPerson = dt.Rows[0]["fromperson"].ToString();
                    Supplier = dt.Rows[0]["Supplier"].ToString();
                    Description = dt.Rows[0]["TransDesc"].ToString();
                    Comment = dt.Rows[0]["Comment"].ToString();
                    fromtime = dt.Rows[0]["fromtime"].ToString();
                    RequiredDate = dt.Rows[0]["RequiredDate"].ToString();
                    CommonInventoryColumn.RequestedDate = Convert.ToDateTime(dt.Rows[0]["fromtime"].ToString());

                    //CommonInventoryColumn.RequiredSDate = Convert.ToDateTime(dt.Rows[0]["RequiredDate"].ToString());
                    CommonInventoryColumn.RequiredSDate = string.IsNullOrEmpty(dt.Rows[0]["RequiredDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(dt.Rows[0]["RequiredDate"]);

                    //  txtrequestedby.Text = dt1.Rows[0]["fromperson"].ToString();
                    //  txtrequesteddate.SelectedDate = Convert.ToDateTime(dt1.Rows[0]["fromtime"].ToString());
                    //  txtdesc.Text = dt1.Rows[0]["TransDesc"].ToString();
                }

            }
            else
            {
                //query = "select (select firstName from tbluser where UserId='" + databaseCon.Decrypt(UID) + "') as fromperson ,CONVERT(VARCHAR(11),getdate(),106) AS fromtime";
                query = "select (select firstName from tbluser where UserId='" + databaseCon.Decrypt(UID) + "') as fromperson ,CONVERT(VARCHAR(11),getdate(),106) AS fromtime,CONVERT(VARCHAR(11),getdate(),106) AS RequiredDate";
                dt = databaseCon.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    RequestedPerson = dt.Rows[0]["fromperson"].ToString();
                    // RequestedDate= Convert.ToDateTime(dt.Rows[0]["fromtime"].ToString());
                    CommonInventoryColumn.RequestedDate = Convert.ToDateTime(dt.Rows[0]["fromtime"].ToString());
                    //CommonInventoryColumn.RequiredDate = dt.Rows[0]["RequiredDate"].ToString();
                    CommonInventoryColumn.RequiredSDate = string.IsNullOrEmpty(dt.Rows[0]["RequiredDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(dt.Rows[0]["RequiredDate"]);
                }
                query = "Select distinct descr,Comment from tblRequestInventoryTemp where  userid=" + databaseCon.Decrypt(UID) + "  and JobId ='" + databaseCon.Decrypt(fromid) + "'";
                dt = databaseCon.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    Description = dt.Rows[0]["descr"].ToString();
                    Comment = dt.Rows[0]["Comment"].ToString();
                }


            }


            var d = databaseCon.Decrypt(page);
            CommonInventoryColumn.ClientID = databaseCon.Decrypt(id);
            CommonInventoryColumn.UserID = databaseCon.Decrypt(UID);
            CommonInventoryColumn.pageID = databaseCon.Decrypt(page);
            // CommonInventoryColumn.from = databaseCon.Decrypt(from);
            CommonInventoryColumn.FromName = fromName;
            CommonInventoryColumn.FromNameID = databaseCon.Decrypt(fromid);
            // CommonInventoryColumn.To = databaseCon.Decrypt(To);
            CommonInventoryColumn.ToName = ToName;
            CommonInventoryColumn.ToNameId = databaseCon.Decrypt(ToId);
            CommonInventoryColumn.RequestedPerson = RequestedPerson;
            //CommonInventoryColumn.RequestedDate =RequestedDate;
            CommonInventoryColumn.Description = Description;
            CommonInventoryColumn.FromtocheckID = databaseCon.Decrypt(fromtocheck);
            CommonInventoryColumn.RequestStatus = databaseCon.Decrypt(ReqStatus);
            CommonInventoryColumn.RequestId = databaseCon.Decrypt(ReqId);
            CommonInventoryColumn.Supplier = Supplier;
            CommonInventoryColumn.Comment = Comment;
            CommonInventoryColumn.fromtime = fromtime;
            CommonInventoryColumn.RequiredDate = RequiredDate;
            return View(CommonInventoryColumn);
        }

        public ActionResult MaterialDropDown(string id, string page, string ToId)
        {
            if (databaseCon.Decrypt(page) == "2")
            {
                //(18-01-2023) Added To Remove Material Having Available Quantity Zero
                //var getMaterialType = dbContext.Database.SqlQuery<InventoryViewModel>("select distinct MaterialType from tblinventory join tblTransactionHistDelivery on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId  where tblinventory.MaterialType <> '' and tblinventory.MaterialType is not null and tblTransactionHistDelivery.[To] = 'site' and tblTransactionHistDelivery.DeliveryStatus = 'Confirm' and tblTransactionHistDelivery.ToId = '" + databaseCon.Decrypt(ToId) + "' and tblinventory.ClientId = '" + databaseCon.Decrypt(id) + "' order by MaterialType").ToList();

                dbContext.Database.CommandTimeout = 15 * 60;
                var getMaterialType = dbContext.Database.SqlQuery<InventoryViewModel>("select distinct  MaterialType from Vw_SiteInventory  join tblinventory on tblInventory.InventoryID = Vw_SiteInventory.InventoryId  join tblTransactionHistDelivery on tblInventory.InventoryID = tblTransactionHistDelivery.InventoryId and Vw_SiteInventory.siteid = tblTransactionHistDelivery.ToId where tblinventory.MaterialType <> '' and tblinventory.MaterialType is not null and tblTransactionHistDelivery.[To] = 'site' and tblTransactionHistDelivery.DeliveryStatus = 'Confirm' and tblTransactionHistDelivery.ToId = '" + databaseCon.Decrypt(ToId) + "' and tblinventory.ClientId = '" + databaseCon.Decrypt(id) + "' group by MaterialType having sum(availablequantity) <> 0 order by MaterialType").ToList();

                return Json(getMaterialType, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var getMaterialType = dbContext.Database.SqlQuery<InventoryViewModel>("select distinct MaterialType from tblinventory where MaterialType <>'' and MaterialType is not null and  ClientId='" + databaseCon.Decrypt(id) + "' order by MaterialType").ToList();
                return Json(getMaterialType, JsonRequestBehavior.AllowGet);
            }


        }

        public ActionResult dropdownSupplier(string id)
        {

            var GetData = dbContext.Database.SqlQuery<DropDownViewModel>("select distinct Supplier from TblTransaction where ClientId='" + databaseCon.Decrypt(id) + "' and  Supplier is not null and Supplier!= '' Order by Supplier").ToList();
            return Json(GetData, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetHistoricalJobMaterialList(string ToID, string FromId)
        {

            int DecTOid = Convert.ToInt32(databaseCon.Decrypt(ToID));
            int DecTOid1 = Convert.ToInt32(databaseCon.Decrypt(FromId));

            //String query;
            //DataTable dt;

            //query = "select Distinct TOId as JobId, Location,Isnull(BuildingNo,'') AS BuildingNo ,isnull(AssetNo,'') as AssetNo, Assetdesc, right('000000' + Convert(varchar, Scaffref), 6) as Scaffref,case when Vw_tblregister.OptimizeStatus is null or Vw_tblregister.OptimizeStatus='Red' then 'Not Finalised' else 'Finalised' end as OptimizeStatus  from tblTransactionHistDelivery left join Vw_tblregister on Vw_tblregister.RegisterId = tblTransactionHistDelivery.ToId and FromId=Vw_tblregister.SiteLocId  where fromId = '" + databaseCon.Decrypt(Request.QueryString["ToId"]) + "'and [From]='Site' and ToId !=" + databaseCon.Decrypt(Request.QueryString["FromId"]);
            //dt = databaseCon.GetData(query);

            var RequestInventoryData = dbContext.Database.SqlQuery<CopyHistoricalJobMaterialList>("select Distinct TOId as JobId, Location,Isnull(BuildingNo,'') AS BuildingNo ,isnull(AssetNo,'') as AssetNo, Assetdesc, right('000000' + Convert(varchar, Scaffref), 6) as Scaffref,case when Vw_tblregister.OptimizeStatus is null or Vw_tblregister.OptimizeStatus='Red' then 'Not Finalised' else 'Finalised' end as OptimizeStatus  from tblTransactionHistDelivery left join Vw_tblregister on Vw_tblregister.RegisterId = tblTransactionHistDelivery.ToId and FromId=Vw_tblregister.SiteLocId  where fromId = '" + databaseCon.Decrypt(ToID) + "'and [From]='Site' and ToId !=" + databaseCon.Decrypt(FromId)).ToList();
            return Json(RequestInventoryData, JsonRequestBehavior.AllowGet);
        }


        public int SetJobData(int UID, int id, int Toid, int jobid, int fromid, string txtComment, string txtdesc)
        {
            string querydel = "delete from tblRequestInventoryTemp where siteId='" + Toid + "' and JobId='" + fromid + "' and UserId='" + UID + "' ";
            int TempDelete = databaseCon.RunCmd(querydel);

            //Commented on 18-12-2023 CreatedBy is going NULL
            //string query = "insert into tblRequestInventoryTemp(UserId, ClientId, InventoryId,Quantity, siteId, JobId, CreatedBy, CreatedOn)(select '" + UID + "','" + id + "',VW_SiteInventory.InventoryID, OptimizedQuantity as Quantity,'" + Toid + "','" + fromid + "',NULL,getdate() from VW_SiteInventory join tblInventory on vw_SiteInventory.InventoryID = tblInventory.InventoryID join tblOptimizedInventory On Vw_SiteInventory.InventoryId = tblOptimizedInventory.InventoryId where VW_SiteInventory.siteId = '" + Toid + "' and tblOptimizedInventory.RegisterId = '" + jobid + "' and SendQuantity is not null)";

            //Added on 18-12-2023 to add Createdby and CreatedOn at time of Historical Copy.
            string query = "insert into tblRequestInventoryTemp(UserId, ClientId, InventoryId,Quantity, siteId, JobId, CreatedBy, CreatedOn,descr,Comment)(select '" + UID + "','" + id + "',VW_SiteInventory.InventoryID, OptimizedQuantity as Quantity,'" + Toid + "','" + fromid + "',(select EmailId from tblUser where UserId = '" + UID + "'),getdate(),'" + txtdesc.Replace("'", "''").Trim() + "','" + txtComment.Replace("'", "''").Trim() + "' from VW_SiteInventory join tblInventory on vw_SiteInventory.InventoryID = tblInventory.InventoryID join tblOptimizedInventory On Vw_SiteInventory.InventoryId = tblOptimizedInventory.InventoryId where VW_SiteInventory.siteId = '" + Toid + "' and tblOptimizedInventory.RegisterId = '" + jobid + "' and SendQuantity is not null)";
            int Tempinsert = databaseCon.RunCmd(query);
            return Tempinsert;
        }

        public ActionResult InsertHistoricalJobMaterialListData(string UID)
        {
            //  string query = "select UserId,AvailableQuantity,Quantity from tblRequestInventoryTemp join Vw_SiteInventory on tblRequestInventoryTemp.InventoryId=Vw_SiteInventory.InventoryID and tblRequestInventoryTemp.siteId=Vw_SiteInventory.SiteId where UserId='" + databaseCon.Decrypt(Request.QueryString["UID"]) + "'";
            string query = "select UserId,AvailableQuantity,Quantity from tblRequestInventoryTemp join Vw_SiteInventory on tblRequestInventoryTemp.InventoryId=Vw_SiteInventory.InventoryID and tblRequestInventoryTemp.siteId=Vw_SiteInventory.SiteId where UserId='" + databaseCon.Decrypt(UID) + "'";
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                int flagvalidQty = 1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToInt32(dt.Rows[i]["AvailableQuantity"]) < Convert.ToInt32(dt.Rows[i]["Quantity"]))
                    {
                        flagvalidQty = 0;
                        //return Json(new { result = "Failed", Message = "Please rectify the quantity marked in red." });
                    }
                }

                return Json(new { result = "Success", flagvalidQty = flagvalidQty });
            }
            else
            {
                //P1.Style.Add("display", "none");
                ////hdngetMsg.Value = "false";
                //string value = "false";
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp3", "<script>getdatachange('" + value + "');</script>", false);

                //Response.Redirect("ScaffoldMaterialListPhase2.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RID=" + Request.QueryString["FromId"]);

                return Json(new { result = "Failed", Message = "No Record Found." });
            }


        }
        public ActionResult btnConfirmReqYes_Click(string id, string UID, string ReqId, string page, string txtdesc, string UserRole, string hdnflagforjobinsertion, string FromId, string txtComment)
        {

            if (databaseCon.Decrypt(page) == "1" || databaseCon.Decrypt(page) == "3")
            {
                // string query = "select 'x' as TotalReqCount from tblTransactionHistRequest join tblTransaction on tblTransactionHistRequest.TransId=tblTransaction.TransId where tblTransaction.TransId='" + databaseCon.Decrypt(Request.QueryString["ReqId"]) + "'";
                string query = "select 'x' as TotalReqCount from tblTransactionHistRequest join tblTransaction on tblTransactionHistRequest.TransId=tblTransaction.TransId where tblTransaction.TransId='" + databaseCon.Decrypt(ReqId) + "'";
                DataTable dt = databaseCon.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    //string query4 = "update tblTransaction set RequestStatus='Confirm',TransDesc='" + txtdesc.Text.Replace("'", "''").Trim() + "' where TransId=" + databaseCon.Decrypt(Request.QueryString["ReqId"]);
                    //databaseCon.GetData(query4);
                    //string query5 = "update tblTransactionHistRequest set RequestStatus='Confirm' where TransId=" + databaseCon.Decrypt(Request.QueryString["ReqId"]);
                    //databaseCon.GetData(query5);
                    // Response.Redirect("RequestInventory.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&TransId=" + Request.QueryString["ReqId"]);

                    //string query4 = "update tblTransaction set RequestStatus='Confirm',TransDesc='" + txtdesc.Replace("'", "''").Trim() + "' where TransId=" + databaseCon.Decrypt(ReqId);
                    string query4 = "update tblTransaction set RequestStatus='Confirm',TransDesc='" + txtdesc.Replace("'", "''").Trim() + "',RequestConfirmedOn='" + DateTime.Now + "',RequestDoneBy =(select EmailId from tblUser where UserId='" + databaseCon.Decrypt(UID) + "')  where TransId=" + databaseCon.Decrypt(ReqId);
                    databaseCon.GetData(query4);
                    string query5 = "update tblTransactionHistRequest set RequestStatus='Confirm' where TransId=" + databaseCon.Decrypt(ReqId);
                    databaseCon.GetData(query5);

                    // return RedirectToAction("RequestInventory", "RequestInventory", new { id = id, UID = UID, UserRole = UserRole, TransId= ReqId });
                    return Json(new { result = "Success", Message = "RequestInventory" });

                }
                else
                {
                    //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Failed ! </strong>Please fill in Quantity to Request then click on Confirm Request.";
                    return Json(new { result = "Failed", Message = "Please fill in Quantity to Request then click on Confirm Request." });
                }
            }
            //  else if (databaseCon.Decrypt(Request.QueryString["page"]) == "2")
            else if (databaseCon.Decrypt(page) == "2")
            {
                bool flag = false;
                //string query = "select UserId,AvailableQuantity,Quantity from tblRequestInventoryTemp join Vw_SiteInventory on tblRequestInventoryTemp.InventoryId=Vw_SiteInventory.InventoryID and tblRequestInventoryTemp.siteId=Vw_SiteInventory.SiteId where UserId='" + databaseCon.Decrypt(Request.QueryString["UID"]) + "'";
                string query = "select UserId,AvailableQuantity,Quantity from tblRequestInventoryTemp join Vw_SiteInventory on tblRequestInventoryTemp.InventoryId=Vw_SiteInventory.InventoryID and tblRequestInventoryTemp.siteId=Vw_SiteInventory.SiteId where UserId='" + databaseCon.Decrypt(UID) + "'";
                DataTable dt = databaseCon.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    hdnflagforjobinsertion = "true";   // Since We Have Data Pending to Be Confirmed we Are Sitting The Flag true 
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (Convert.ToInt32(dt.Rows[i]["AvailableQuantity"]) < Convert.ToInt32(dt.Rows[i]["Quantity"]))
                        {
                            flag = true;
                            //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                            //art1.Style.Add("display", "block");
                            //alertlabel.Text = "<strong>Alert ! </strong>Please rectify the quantity marked in red.";
                            return Json(new { result = "Failed", Message = "Please rectify the quantity marked in red." });
                        }
                    }
                    if (flag != true)
                    {
                        // if (hdnflagforjobinsertion.Value == "true")
                        if (hdnflagforjobinsertion == "true")
                        {
                            //  string query3 = "exec Pro_RequestInventoryForJob '" + databaseCon.Decrypt(Request.QueryString["UID"]) + "','" + databaseCon.Decrypt(Request.QueryString["id"]) + "'";

                            string updtdesc = "update tblRequestInventoryTemp set  descr='" + txtdesc.Replace("'", "''").Trim() + "' where UserId='" + databaseCon.Decrypt(UID) + "' and jobid=" + databaseCon.Decrypt(FromId) + "   and  descr!='" + txtdesc.Replace("'", "''").Trim() + "'";
                            databaseCon.RunCmdID(updtdesc);

                            string updtcmmnt = "update tblRequestInventoryTemp set comment='" + txtComment.Replace("'", "''").Trim() + "'   where UserId='" + databaseCon.Decrypt(UID) + "' and jobid=" + databaseCon.Decrypt(FromId) + "   and comment!='" + txtComment.Replace("'", "''").Trim() + "' ";
                            databaseCon.RunCmdID(updtcmmnt);
                            string query3 = "exec Pro_RequestInventoryForJob '" + databaseCon.Decrypt(UID) + "','" + databaseCon.Decrypt(id) + "'";
                            int execpro = databaseCon.RunCmdID(query3);
                            flag = true;
                        }
                        //   Response.Redirect("ScaffoldMaterialListPhase2.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RID=" + Request.QueryString["FromId"]);
                        // return RedirectToAction("ScaffoldMaterialListPhase2", "ScaffoldMaterialListPhase2", new { UID = UID, UserRole = UserRole, id = id, RID = FromId });
                        string querey = "delete from tblRequestInventoryTemp where userid=" + databaseCon.Decrypt(UID); //Commented 3-Nov-2021 For Refresh Same Page
                        int delete = databaseCon.RunCmdID(querey);
                        return Json(new { result = "Success", Message = "ScaffoldMaterialListPhase2" });

                    }

                }
            }
            return Json(new { result = "Failed", Message = "Something went wrong Page ID Not Valid." });

        }



        // public ActionResult griddata(int id, int Reqstid, string Material, string Page, int ToID, int UID)
        //  public ActionResult griddata()
        public ActionResult griddata(string id, string Reqstid, string Material, string Page, string ToID, string UID, string Callfrom)
        {
            //  var RequestInventoryData = dbContext.Database.SqlQuery<SiteRequestViewModel>("select VW_DepotInventory.InventoryId, TransReqId, MaterialType, ItemCode, ItemDesc, Quantity, cast(AvailableQuantity as int) as AvailableQuantity,'' as CheckQty from VW_DepotInventory Left join tblTransactionHistRequest  on VW_DepotInventory.InventoryID = tblTransactionHistRequest.InventoryId and TransId = '0' where ClientID = '153'  order by case when Quantity is null then 1 else 0 end , MaterialType, ItemCode").ToList();
            // return Json(RequestInventoryData, JsonRequestBehavior.AllowGet);



            string addquery = "";
            string query = "";
            if (Material != "" && Material != "All")
            {
                addquery = " and MaterialType='" + Material.Replace("'", "''").Trim() + "'";

            }
            if (Page == "1")
            {
                //query = "select InventoryId,TransReqId, MaterialType, ItemCode,ItemDesc, Quantity, cast(AvailableQuantity as int) as AvailableQuantity,'' as CheckQty from(select VW_DepotInventory.InventoryID, NULL as TransReqId,MaterialType, ItemCode,ItemDesc, NULL as Quantity, AvailableQuantity from VW_DepotInventory  where InventoryID not in (select InventoryID from tblTransactionHistRequest where TransId='" + Reqstid + "') and ClientID = '" + id + "'union select VW_DepotInventory.InventoryID,tblTransactionHistRequest.TransReqId as TransReqId,MaterialType,ItemCode,ItemDesc,tblTransactionHistRequest.Quantity as Quantity, AvailableQuantity from VW_DepotInventory join tblTransactionHistRequest  on VW_DepotInventory.InventoryID = tblTransactionHistRequest.InventoryId  where TransId='" + Reqstid + "') UnionQuery where 1=1 " + addquery + " order by case when Quantity is null then 1 else 0 end ,MaterialType, ItemCode";                                         //By Anuj 31 Jan

                //  query = "select VW_DepotInventory.InventoryId,TransReqId, MaterialType, ItemCode,ItemDesc, Quantity, cast(AvailableQuantity as int) as AvailableQuantity,'' as CheckQty from VW_DepotInventory Left join tblTransactionHistRequest  on VW_DepotInventory.InventoryID = tblTransactionHistRequest.InventoryId  and TransId = '" + Reqstid + "' where ClientID = '" + id + "' " + addquery + " order by case when Quantity is null then 1 else 0 end ,MaterialType, ItemCode";

                //  var RequestInventoryData = dbContext.Database.SqlQuery<SiteRequestViewModel>("select VW_DepotInventory.InventoryId, TransReqId, MaterialType, ItemCode, ItemDesc, Quantity, cast(AvailableQuantity as int) as AvailableQuantity, '' as CheckQty from VW_DepotInventory Left join tblTransactionHistRequest  on VW_DepotInventory.InventoryID = tblTransactionHistRequest.InventoryId  and TransId = '" + Reqstid + "' where ClientID = '" + id + "' " + addquery + " order by case when Quantity is null then 1 else 0 end, MaterialType, ItemCode").ToList();

                dbContext.Database.CommandTimeout = 15 * 60;

                string query1 = "select VW_DepotInventory.InventoryId, TransReqId, MaterialType, ItemCode, ItemDesc, Quantity, cast(AvailableQuantity as int) as AvailableQuantity, '' as CheckQty , DisplaySortOrder from VW_DepotInventory Left join tblTransactionHistRequest  on VW_DepotInventory.InventoryID = tblTransactionHistRequest.InventoryId  and TransId = '" + Reqstid + "' where ClientID = '" + id + "' " + addquery + " order by case when isnull(Quantity, 0) = 0 then 1 else 0 end, MaterialType, isnull(DisplaySortOrder,9999999),ItemCode";

                var RequestInventoryData = dbContext.Database.SqlQuery<SiteRequestViewModel>(query1).ToList();
                return Json(RequestInventoryData, JsonRequestBehavior.AllowGet);
            }
            else if (Page == "2")
            {
                //  query = "select InventoryId, TransReqId, MaterialType, ItemCode, ItemDesc, Quantity, cast(AvailableQuantity as int) as AvailableQuantity, '' as CheckQty from (select VW_SiteInventory.InventoryID, NULL as TransReqId, MaterialType, ItemCode, ItemDesc, NULL as Quantity, AvailableQuantity from VW_SiteInventory join tblInventory on vw_SiteInventory.InventoryID = tblInventory.InventoryID where VW_SiteInventory.InventoryID not in (select InventoryID from tblRequestInventoryTemp where UserId ='" + UID + "') and VW_SiteInventory.siteId ='" + ToID + "' and SendQuantity is not null union select  VW_SiteInventory.InventoryID, tblRequestInventoryTemp.ReqTempId as TransReqId, MaterialType, ItemCode, ItemDesc, tblRequestInventoryTemp.Quantity as Quantity, AvailableQuantity from VW_SiteInventory join tblRequestInventoryTemp  on VW_SiteInventory.InventoryID = tblRequestInventoryTemp.InventoryId and tblRequestInventoryTemp.SiteId = Vw_SiteInventory.SiteId join tblInventory on vw_SiteInventory.InventoryID = tblInventory.InventoryID  where UserId = '" + UID + "' ) UnionQuery where 1 = 1 " + addquery + " order by case when Quantity is null then 1 else 0 end ,MaterialType, ItemCode";
                // UPDATE ON  20 JAN 2023
                //var RequestInventoryData = dbContext.Database.SqlQuery<SiteRequestViewModel>("select InventoryId, TransReqId, MaterialType, ItemCode, ItemDesc, Quantity, cast(AvailableQuantity as int) as AvailableQuantity, '' as CheckQty from (select VW_SiteInventory.InventoryID, NULL as TransReqId, MaterialType, ItemCode, ItemDesc, NULL as Quantity, AvailableQuantity from VW_SiteInventory join tblInventory on vw_SiteInventory.InventoryID = tblInventory.InventoryID where VW_SiteInventory.InventoryID not in (select InventoryID from tblRequestInventoryTemp where UserId ='" + UID + "') and VW_SiteInventory.siteId ='" + ToID + "' and SendQuantity is not null union select  VW_SiteInventory.InventoryID, tblRequestInventoryTemp.ReqTempId as TransReqId, MaterialType, ItemCode, ItemDesc, tblRequestInventoryTemp.Quantity as Quantity, AvailableQuantity from VW_SiteInventory join tblRequestInventoryTemp  on VW_SiteInventory.InventoryID = tblRequestInventoryTemp.InventoryId and tblRequestInventoryTemp.SiteId = Vw_SiteInventory.SiteId join tblInventory on vw_SiteInventory.InventoryID = tblInventory.InventoryID  where UserId = '" + UID + "' ) UnionQuery where 1 = 1 " + addquery + " order by case when Quantity is null then 1 else 0 end ,MaterialType, ItemCode").ToList();

                dbContext.Database.CommandTimeout = 15 * 60;

                if (Callfrom == "Continueclick")
                {

                    //string query2 = "select InventoryId, TransReqId, MaterialType, ItemCode, ItemDesc, Quantity, cast(AvailableQuantity as int) as AvailableQuantity, '' as CheckQty from (select VW_SiteInventory.InventoryID, NULL as TransReqId, MaterialType, ItemCode, ItemDesc, NULL as Quantity, AvailableQuantity from VW_SiteInventory join tblInventory on vw_SiteInventory.InventoryID = tblInventory.InventoryID where VW_SiteInventory.InventoryID not in (select InventoryID from tblRequestInventoryTemp where UserId ='" + UID + "') and VW_SiteInventory.siteId ='" + ToID + "' and SendQuantity is not null union select  VW_SiteInventory.InventoryID, tblRequestInventoryTemp.ReqTempId as TransReqId, MaterialType, ItemCode, ItemDesc, tblRequestInventoryTemp.Quantity as Quantity, AvailableQuantity from VW_SiteInventory join tblRequestInventoryTemp  on VW_SiteInventory.InventoryID = tblRequestInventoryTemp.InventoryId and tblRequestInventoryTemp.SiteId = Vw_SiteInventory.SiteId join tblInventory on vw_SiteInventory.InventoryID = tblInventory.InventoryID  where UserId = '" + UID + "' ) UnionQuery where 1 = 1 " + addquery + " order by case when Quantity is null then 1 else 0 end ,MaterialType,isnull(DisplaySortOrder,9999999), ItemCode";
                    // added on  22-04-2025 DisplaySortOrder 

                    string query2 = "select InventoryId, TransReqId, MaterialType, ItemCode, ItemDesc, Quantity, cast(AvailableQuantity as int) as AvailableQuantity, '' as CheckQty ,DisplaySortOrder from (select VW_SiteInventory.InventoryID, NULL as TransReqId, MaterialType, ItemCode, ItemDesc, NULL as Quantity, AvailableQuantity .DisplaySortOrderfrom VW_SiteInventory join tblInventory on vw_SiteInventory.InventoryID = tblInventory.InventoryID where VW_SiteInventory.InventoryID not in (select InventoryID from tblRequestInventoryTemp where UserId ='" + UID + "') and VW_SiteInventory.siteId ='" + ToID + "' and SendQuantity is not null union select  VW_SiteInventory.InventoryID, tblRequestInventoryTemp.ReqTempId as TransReqId, MaterialType, ItemCode, ItemDesc, tblRequestInventoryTemp.Quantity as Quantity, AvailableQuantity ,DisplaySortOrderfrom VW_SiteInventory join tblRequestInventoryTemp  on VW_SiteInventory.InventoryID = tblRequestInventoryTemp.InventoryId and tblRequestInventoryTemp.SiteId = Vw_SiteInventory.SiteId join tblInventory on vw_SiteInventory.InventoryID = tblInventory.InventoryID  where UserId = '" + UID + "' ) UnionQuery where 1 = 1 " + addquery + " order by case when Quantity is null then 1 else 0 end ,MaterialType,isnull(DisplaySortOrder,9999999), ItemCode";
                    var RequestInventoryData = dbContext.Database.SqlQuery<SiteRequestViewModel>(query2).ToList();

                    return Json(RequestInventoryData, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //string query4 = "select InventoryId, TransReqId, MaterialType, ItemCode, ItemDesc, Quantity, cast(AvailableQuantity as int) as AvailableQuantity, '' as CheckQty from (select VW_SiteInventory.InventoryID, NULL as TransReqId, MaterialType, ItemCode, ItemDesc, NULL as Quantity, AvailableQuantity from VW_SiteInventory join tblInventory on vw_SiteInventory.InventoryID = tblInventory.InventoryID where VW_SiteInventory.InventoryID not in (select InventoryID from tblRequestInventoryTemp where UserId = '" + UID + "') and VW_SiteInventory.siteId = '" + ToID + "' and SendQuantity is not null union select  VW_SiteInventory.InventoryID, tblRequestInventoryTemp.ReqTempId as TransReqId, MaterialType, ItemCode, ItemDesc, tblRequestInventoryTemp.Quantity as Quantity, AvailableQuantity from VW_SiteInventory join tblRequestInventoryTemp  on VW_SiteInventory.InventoryID = tblRequestInventoryTemp.InventoryId and tblRequestInventoryTemp.SiteId = Vw_SiteInventory.SiteId join tblInventory on vw_SiteInventory.InventoryID = tblInventory.InventoryID  where UserId = '" + UID + "') UnionQuery ,(select   MaterialType as MatType from Vw_SiteInventory join tblinventory on tblInventory.InventoryID = Vw_SiteInventory.InventoryId where SiteID = '" + ToID + "' and tblinventory.ClientId = '" + id + "' group by MaterialType having sum(availablequantity) <> 0) tmp_Material_type where 1 = 1 and UnionQuery.materialtype =tmp_Material_type.MatType " + addquery + " order by case when Quantity is null then 1 else 0 end ,MaterialType, ItemCode";
                    // added on  22-04-2025 DisplaySortOrder 
                    string query4 = "select InventoryId, TransReqId, MaterialType, ItemCode, ItemDesc, Quantity, cast(AvailableQuantity as int) as AvailableQuantity, '' as CheckQty ,DisplaySortOrder from (select VW_SiteInventory.InventoryID, NULL as TransReqId, MaterialType, ItemCode, ItemDesc, NULL as Quantity, AvailableQuantity ,DisplaySortOrder from VW_SiteInventory join tblInventory on vw_SiteInventory.InventoryID = tblInventory.InventoryID where VW_SiteInventory.InventoryID not in (select InventoryID from tblRequestInventoryTemp where UserId = '" + UID + "') and VW_SiteInventory.siteId = '" + ToID + "' and SendQuantity is not null union select  VW_SiteInventory.InventoryID, tblRequestInventoryTemp.ReqTempId as TransReqId, MaterialType, ItemCode, ItemDesc, tblRequestInventoryTemp.Quantity as Quantity, AvailableQuantity ,DisplaySortOrder from VW_SiteInventory join tblRequestInventoryTemp  on VW_SiteInventory.InventoryID = tblRequestInventoryTemp.InventoryId and tblRequestInventoryTemp.SiteId = Vw_SiteInventory.SiteId join tblInventory on vw_SiteInventory.InventoryID = tblInventory.InventoryID  where UserId = '" + UID + "') UnionQuery ,(select   MaterialType as MatType from Vw_SiteInventory join tblinventory on tblInventory.InventoryID = Vw_SiteInventory.InventoryId where SiteID = '" + ToID + "' and tblinventory.ClientId = '" + id + "' group by MaterialType having sum(availablequantity) <> 0) tmp_Material_type where 1 = 1 and UnionQuery.materialtype =tmp_Material_type.MatType " + addquery + " order by case when Quantity is null then 1 else 0 end ,MaterialType,isnull(DisplaySortOrder,'9999999'), ItemCode";

                    var RequestInventoryData = dbContext.Database.SqlQuery<SiteRequestViewModel>(query4).ToList();

                    return Json(RequestInventoryData, JsonRequestBehavior.AllowGet);
                }


            }
            else
            {
                // var RequestInventoryData = dbContext.Database.SqlQuery<SiteRequestViewModel>("select Vw_ThirdPartyInventory.InventoryId, TransReqId, MaterialType, ItemCode, ItemDesc, Quantity, cast(AvailableQuantity as int) as AvailableQuantity, '' as CheckQty from (select * from Vw_ThirdPartyInventory where ClientID = '" + id + "' " + addquery + ")Vw_ThirdPartyInventory Left join tblTransactionHistRequest  on Vw_ThirdPartyInventory.InventoryID = tblTransactionHistRequest.InventoryId  and TransId = '" + Reqstid + "' order by case when Quantity is null then 1 else 0 end, MaterialType, ItemCode").ToList();

                dbContext.Database.CommandTimeout = 15 * 60;
                string query5 = "select Vw_ThirdPartyInventory.InventoryId, TransReqId, MaterialType, ItemCode, ItemDesc, Quantity, cast(AvailableQuantity as int) as AvailableQuantity, '' as CheckQty,DisplaySortOrder from (select * from Vw_ThirdPartyInventory where ClientID = '" + id + "' " + addquery + ")Vw_ThirdPartyInventory Left join tblTransactionHistRequest  on Vw_ThirdPartyInventory.InventoryID = tblTransactionHistRequest.InventoryId  and TransId = '" + Reqstid + "' order by case when isnull(Quantity, 0) = 0 then 1 else 0 end, MaterialType,isnull(DisplaySortOrder,9999999), ItemCode";
                var RequestInventoryData = dbContext.Database.SqlQuery<SiteRequestViewModel>(query5).ToList();

                return Json(RequestInventoryData, JsonRequestBehavior.AllowGet);
            }

        }



        //public ActionResult GetTotalTotalTonnage(string ClientID, string ReqId)
        //{
        //    var TotalItemsValues = "";
        //    var TotalTonnageValues = "";
        //    string query = "select ISNULL(FORMAT(CONVERT(int,(Sum(Quantity))),'N0'),0) as TotalItems,convert(decimal(18,2),ISNULL(Sum(Quantity * ItemWeight) / 1000, 0)) as TotalTonnage from tblTransactionHistRequest join Vw_ThirdPartyInventory on tblTransactionHistRequest.InventoryID = Vw_ThirdPartyInventory.InventoryId where tblTransactionHistRequest.ToId = " + databaseCon.Decrypt(ClientID) + " and TransId = '" + ReqId + "'";
        //    DataTable dt = databaseCon.GetData(query);
        //    if (dt.Rows.Count > 0)
        //    {
        //        TotalItemsValues = dt.Rows[0]["TotalItems"].ToString();
        //        TotalTonnageValues = dt.Rows[0]["TotalTonnage"].ToString();
        //    }
        //    var Result = "TotalItemsValues" + TotalItemsValues + "|" + "TotalTonnageValues" + TotalTonnageValues;
        //    return Json(new { TotalItemsValues = TotalItemsValues, TotalTonnageValues = TotalTonnageValues });
        //}

        public ActionResult GetTotalTotalTonnage(string ClientID, string ReqId, string JobId , string UID)
        {
            var TotalItemsValues = "";
            var TotalTonnageValues = "";
            string query = "";
            string decryptedClientId = databaseCon.Decrypt(ClientID);
            //string decryptedJobId = databaseCon.Decrypt(JobId);

            // Check if ReqId is 0 or blank/null/whitespace
            if (string.IsNullOrWhiteSpace(ReqId) || ReqId == "0")
            {
                // Use the new query for ReqId = 0 or blank
                // Assuming ClientID and JobId are already decrypted or used safely in the query.
                // It's generally safer to use parameters, but I'm adapting the provided string concatenation style.
                query = "select ISNULL(FORMAT(CONVERT(int,(Sum(Quantity))),'N0'),0) as TotalItems,convert(decimal(18,2),ISNULL(Sum(Quantity * ItemWeight) / 1000, 0)) as TotalTonnage from tblRequestInventoryTemp join tblinventory on tblinventory.InventoryID = tblRequestInventoryTemp.InventoryId and tblRequestInventoryTemp.ClientId=tblinventory.ClientID where tblRequestInventoryTemp.ClientId=" + decryptedClientId + " and Userid= " +  UID + " and tblRequestInventoryTemp.jobid=" + JobId;
            }
            else
            {
                // Use the previous query for a valid ReqId
                query = "select ISNULL(FORMAT(CONVERT(int,(Sum(Quantity))),'N0'),0) as TotalItems,convert(decimal(18,2),ISNULL(Sum(Quantity * ItemWeight) / 1000, 0)) as TotalTonnage  from tblTransactionHistRequest join Vw_ThirdPartyInventory on tblTransactionHistRequest.InventoryID = Vw_ThirdPartyInventory.InventoryId  where tblTransactionHistRequest.ToId = " + decryptedClientId + " and TransId = '" + ReqId + "'";
            }

            // --- EXECUTION PART ---
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                TotalItemsValues = dt.Rows[0]["TotalItems"].ToString();
                TotalTonnageValues = dt.Rows[0]["TotalTonnage"].ToString();
            }

            // The 'Result' variable in the original code is not used for the return value, 
            // but the final return Json is correct.
            // var Result = "TotalItemsValues" + TotalItemsValues + "|" + "TotalTonnageValues" + TotalTonnageValues; 

            return Json(new { TotalItemsValues = TotalItemsValues, TotalTonnageValues = TotalTonnageValues });
        }

        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod(EnableSession = true)]
        //  public int insertion(int Qty, int OldQty, int InvId, int UID, string from, string fromid, string To, string ToId, int id, string TReqId, int ReqID, string Page, int tempval, string txtdesc)
        public int insertion(int Qty, int OldQty, string InvId, string UID, string from, string fromid, string To, string ToId, string id, string TReqId, string ReqID, string Page, string tempval, string txtdesc, string Supplier, string txtComment, string txtrequesteddate, string txtrequireddate)
        {
            int counI = 0;

            int counU = 0;
            int InsertId = 0;
            string qry, query1;



            // Page 1 - Site Inventory Request  
            // Page 2 - Job Inventory Request
            // Page 3 - Depot Inventory Request
            if (Page == "1" || Page == "3")
            {
                if (Convert.ToInt32(ReqID) == 0)
                {
                    if (Qty > 0)
                    {
                        //string query1 = "insert into tblTransaction(TransDesc,Supplier,TransType,FromPerson,FromTime,[From],FromId,[To],ToId,CreatedBy,CreatedOn,ClientId)  values('" + txtdesc.Replace("'", "''").Trim() + "','" + Supplier.Replace("'", "''").Trim() + "','Request',(select firstName+' '+LastName from tblUser where UserId='" + UID + "'),GETDATE(),'" + from + "','" + fromid + "','" + To + "','" + ToId + "',(select EmailId from tblUser where UserId='" + UID + "'),'" + DateTime.Now + "','" + id + "')";
                        //string query1 = "insert into tblTransaction(TransDesc, Supplier, TransType, FromPerson, FromTime,[From], FromId,[To], ToId, CreatedBy, CreatedOn, ClientId)Select '" + txtdesc.Replace("'", "''").Trim() + "','" + Supplier.Replace("'", "''").Trim() + "','Request',(select firstName + ' ' + LastName from tblUser where UserId = '" + UID + "'),GETDATE(),'" + from + "','" + fromid + "','" + To + "','" + ToId + "',(select EmailId from tblUser where UserId = '" + UID + "'),GETDATE(),'" + id + "' WHERE NOT EXISTS (SELECT  1 FROM    tblTransaction where clientId = '" + id + "' and[To] = '" + To + "'  and[From] = '" + from + "' and FRomId = '" + fromid + "' and CreatedOn > DateAdd(minute, -1, GETDATE()) and CreatedOn < GETDATE() and ToId = '" + ToId + "' and CreatedBy = (select EmailId from tblUser where UserId = '" + UID + "'))";
                        //string query1 = "insert into tblTransaction(TransDesc, Supplier,Comment, TransType, FromPerson, FromTime,[From], FromId,[To], ToId, CreatedBy, CreatedOn, ClientId)Select '" + txtdesc.Replace("'", "''").Trim() + "','" + Supplier.Replace("'", "''").Trim() + "','" + txtComment.Replace("'", "''").Trim() + "','Request',(select firstName + ' ' + LastName from tblUser where UserId = '" + UID + "'),'" + txtrequesteddate + "','" + from + "','" + fromid + "','" + To + "','" + ToId + "',(select EmailId from tblUser where UserId = '" + UID + "'),'" + DateTime.Now + "','" + id + "' WHERE NOT EXISTS (SELECT  1 FROM    tblTransaction where clientId = '" + id + "' and[To] = '" + To + "'  and[From] = '" + from + "' and FRomId = '" + fromid + "' and CreatedOn > DateAdd(minute, -1, '" + DateTime.Now + "') and CreatedOn <= '" + DateTime.Now + "' and ToId = '" + ToId + "' and CreatedBy = (select EmailId from tblUser where UserId = '" + UID + "'))"; 
                        if (string.IsNullOrEmpty(txtrequireddate))
                        {
                            query1 = "insert into tblTransaction(TransDesc, Supplier,Comment, TransType, FromPerson, FromTime,[From], FromId,[To], ToId, CreatedBy, CreatedOn, ClientId)Select '" + txtdesc.Replace("'", "''").Trim() + "','" + Supplier.Replace("'", "''").Trim() + "','" + txtComment.Replace("'", "''").Trim() + "','Request',(select firstName + ' ' + LastName from tblUser where UserId = '" + UID + "'),'" + txtrequesteddate + "','" + from + "','" + fromid + "','" + To + "','" + ToId + "',(select EmailId from tblUser where UserId = '" + UID + "'),'" + DateTime.Now + "','" + id + "' WHERE NOT EXISTS (SELECT  1 FROM    tblTransaction where clientId = '" + id + "' and[To] = '" + To + "'  and[From] = '" + from + "' and FRomId = '" + fromid + "' and CreatedOn > DateAdd(minute, -1, '" + DateTime.Now + "') and CreatedOn <= '" + DateTime.Now + "' and ToId = '" + ToId + "' and CreatedBy = (select EmailId from tblUser where UserId = '" + UID + "'))";
                        }
                        else
                        {
                            query1 = "insert into tblTransaction(TransDesc, Supplier,Comment, TransType, FromPerson, FromTime,[From], FromId,[To], ToId, CreatedBy, CreatedOn, ClientId,RequiredDate)Select '" + txtdesc.Replace("'", "''").Trim() + "','" + Supplier.Replace("'", "''").Trim() + "','" + txtComment.Replace("'", "''").Trim() + "','Request',(select firstName + ' ' + LastName from tblUser where UserId = '" + UID + "'),'" + txtrequesteddate + "','" + from + "','" + fromid + "','" + To + "','" + ToId + "',(select EmailId from tblUser where UserId = '" + UID + "'),'" + DateTime.Now + "','" + id + "','" + txtrequireddate + "' WHERE NOT EXISTS (SELECT  1 FROM    tblTransaction where clientId = '" + id + "' and[To] = '" + To + "'  and[From] = '" + from + "' and FRomId = '" + fromid + "' and CreatedOn > DateAdd(minute, -1, '" + DateTime.Now + "') and CreatedOn <= '" + DateTime.Now + "' and ToId = '" + ToId + "' and CreatedBy = (select EmailId from tblUser where UserId = '" + UID + "'))";
                        }
                        //Added on 03-June-2022
                        //string query1 = "insert into tblTransaction(TransDesc, Supplier,Comment, TransType, FromPerson, FromTime,[From], FromId,[To], ToId, CreatedBy, CreatedOn, ClientId)Select '" + txtdesc.Replace("'", "''").Trim() + "','" + Supplier.Replace("'", "''").Trim() + "','" + txtComment.Replace("'", "''").Trim() + "','Request',(select firstName + ' ' + LastName from tblUser where UserId = '" + UID + "'),'" + DateTime.Now + "','" + from + "','" + fromid + "','" + To + "','" + ToId + "',(select EmailId from tblUser where UserId = '" + UID + "'),'" + DateTime.Now + "','" + id + "' WHERE NOT EXISTS (SELECT  1 FROM    tblTransaction where clientId = '" + id + "' and[To] = '" + To + "'  and[From] = '" + from + "' and FRomId = '" + fromid + "' and CreatedOn > DateAdd(minute, -1, '" + DateTime.Now + "') and CreatedOn <= '" + DateTime.Now + "' and ToId = '" + ToId + "' and CreatedBy = (select EmailId from tblUser where UserId = '" + UID + "'))";//Commented on 03-June-2022

                        counI = databaseCon.RunCmdID(query1);
                    }

                    if (counI > 0)
                    {
                        if (Qty > 0)
                        {
                            string query = "insert into tblTransactionHistRequest([From],FromId,[To],ToId,Quantity,TransType,TransId,InventoryId ,CreatedBy,CreatedOn) values('" + from + "','" + fromid + "','" + To + "','" + ToId + "','" + Qty + "','Request','" + counI + "','" + InvId + "',(select EmailId from tblUser where UserId='" + UID + "') ,'" + DateTime.Now + "')";
                            InsertId = databaseCon.RunCmdID(query);
                        }
                    }
                    return counI;
                }
                else
                {
                    if (Convert.ToInt32(tempval) == 0)
                    {
                        //if (TReqId == 0)
                        //  if(TReqId == "0" && TReqId=="")
                        if (TReqId == "0" || TReqId == "")
                        {
                            if (Qty > 0)
                            {
                                string query = "MERGE INTO tblTransactionHistRequest AS Target USING(SELECT '" + from + "' as [From], '" + fromid + "' as FromId, '" + To + "' as [To], '" + ToId + "' as ToId, '" + Qty + "' as Quantity, 'Request' as TransType, '" + ReqID + "' as TransId, '" + InvId + "' as InventoryId, (select EmailId from tblUser where UserId = '" + UID + "')  as CreatedBy,'" + DateTime.Now + "' as CreatedOn ) AS Source ON Target.TransId = Source.TransId  and Target.InventoryId = Source.InventoryId WHEN NOT MATCHED THEN   INSERT([From],FromId,[To],ToId,Quantity,TransType,TransId,InventoryId ,CreatedBy,CreatedOn) VALUES(Source.[From], Source.FromId, [To], Source.ToId, Source.Quantity, Source.TransType, Source.TransId, Source.InventoryId , Source.CreatedBy, Source.CreatedOn); ";

                                //string query = "insert into tblTransactionHistRequest([From],FromId,[To],ToId,Quantity,TransType,TransId,InventoryId ,CreatedBy,CreatedOn) values('" + from + "','" + fromid + "','" + To + "','" + ToId + "','" + Qty + "','Request','" + ReqID + "','" + InvId + "',(select EmailId from tblUser where UserId='" + UID + "') ,'" + DateTime.Now + "')";
                                counU = databaseCon.RunCmdID(query);
                                if (string.IsNullOrEmpty(txtrequireddate))
                                {
                                    qry = "update tblTransaction set Supplier='" + Supplier.Replace("'", "''").Trim() + "',Comment='" + txtComment + "',fromtime= '" + txtrequesteddate + "', TransDesc = '" + txtdesc + "' where TransId='" + ReqID + "'";
                                }
                                else
                                {
                                    qry = "update tblTransaction set Supplier='" + Supplier.Replace("'", "''").Trim() + "',Comment='" + txtComment + "',fromtime= '" + txtrequesteddate + "' ,RequiredDate = '" + txtrequireddate + "',TransDesc = '" + txtdesc + "' where TransId='" + ReqID + "'";
                                }
                                //Added on 03-June-2022
                                //string qry = "update tblTransaction set Supplier='" + Supplier.Replace("'", "''").Trim() + "', where TransId='" + ReqID + "'";//Commnetd on 03-June-2022
                                int i = databaseCon.RunCmd(qry);
                            }

                        }
                        else
                        {
                            string query = "update tblTransactionHistRequest Set Quantity='" + Qty + "',UpdatedBy=(select EmailId from tblUser where UserId='" + UID + "'),UpdatedOn='" + DateTime.Now + "'Where TransId=" + ReqID + " and TransReqId=" + TReqId;
                            int dt = databaseCon.RunCmd(query);
                            if (string.IsNullOrEmpty(txtrequireddate))
                            {
                                qry = "update tblTransaction set Supplier='" + Supplier.Replace("'", "''").Trim() + "',Comment='" + txtComment + "',fromtime= '" + txtrequesteddate + "', TransDesc = '" + txtdesc + "' where TransId='" + ReqID + "'";
                            }
                            else
                            {
                                qry = "update tblTransaction set Supplier='" + Supplier.Replace("'", "''").Trim() + "',Comment='" + txtComment + "',fromtime= '" + txtrequesteddate + "' ,RequiredDate = '" + txtrequireddate + "',TransDesc = '" + txtdesc + "' where TransId='" + ReqID + "'";
                            }
                            //Added on 03-June-2022
                            //string qry = "update tblTransaction set Supplier='" + Supplier.Replace("'", "''").Trim() + "' where TransId='" + ReqID + "'";//Commented on 03-June-2022
                            int i = databaseCon.RunCmd(qry);
                        }

                    }
                    else
                    {
                        string query = "update tblTransactionHistRequest Set Quantity='" + Qty + "',UpdatedBy=(select EmailId from tblUser where UserId='" + UID + "'),UpdatedOn='" + DateTime.Now + "'Where TransId=" + ReqID + " and TransReqId=" + tempval;
                        int dt = databaseCon.RunCmd(query);
                        if (string.IsNullOrEmpty(txtrequireddate))
                        {
                            qry = "update tblTransaction set Supplier='" + Supplier.Replace("'", "''").Trim() + "',Comment='" + txtComment + "',fromtime= '" + txtrequesteddate + "' where TransId='" + ReqID + "'";
                        }
                        else
                        {
                            qry = "update tblTransaction set Supplier='" + Supplier.Replace("'", "''").Trim() + "',Comment='" + txtComment + "',fromtime= '" + txtrequesteddate + "',RequiredDate= '" + txtrequireddate + "' where TransId='" + ReqID + "'";
                        }
                        //Added on 03-June-2022
                        //string qry = "update tblTransaction set Supplier='" + Supplier.Replace("'", "''").Trim() + "' where TransId='" + ReqID + "'";//Commeted on 03-June-2022
                        int i = databaseCon.RunCmd(qry);
                    }
                    return counU;
                }

            }
            else
            {
                int Requestid = 0;
                //commented and added on 09sept2025 by pooja as opening multiple tabs causing create duplicate entries for same inventoryid and transid
                //if (Convert.ToInt32(tempval) == 0)
                //{
                //if (TReqId == 0)
                if (TReqId == "0" || TReqId == "")
                {
                    if (Qty > 0)
                    {
                        //string query = "insert into tblRequestInventoryTemp(UserId, ClientId, InventoryId, siteId, JobId, Quantity, CreatedBy, CreatedOn,descr,Comment) values(" + UID + "," + id + "," + InvId + "," + ToId + "," + fromid + "," + Qty + ",(select EmailId from tblUser where UserId='" + UID + "') ,'" + DateTime.Now + "','" + txtdesc.Replace("'", "''").Trim() + "','" + txtComment.Replace("'", "''").Trim() + "')";
                        //Requestid = databaseCon.RunCmdID(query);
                        //commented and added on 09sept2025 by pooja as opening multiple tabs causing create duplicate entries for same inventoryid and transid
                        string query = "MERGE tblRequestInventoryTemp AS target USING(SELECT  " + UID + " AS UserId," + id + " AS ClientId," + InvId + " AS InventoryId," + ToId + " AS SiteId," + fromid + " AS JobId," + Qty + " AS Quantity,(SELECT EmailId FROM tblUser WHERE UserId = '" + UID + "') AS CreatedBy,'" + DateTime.Now + "' AS CreatedOn,'" + txtdesc.Replace("'", "''").Trim() + "' AS descr,'" + txtComment.Replace("'", "''").Trim() + "' AS Comment) AS src ON(target.UserId = src.UserId AND target.ClientId = src.ClientId AND target.InventoryId = src.InventoryId AND target.SiteId = src.SiteId AND target.JobId = src.JobId) WHEN MATCHED THEN UPDATE SET target.Quantity = src.Quantity, target.UpdatedBy = (SELECT EmailId FROM tblUser WHERE UserId = '" + UID + "'), target.UpdatedOn = '" + DateTime.Now + "',target.descr = src.descr,target.Comment = src.Comment WHEN NOT MATCHED THEN INSERT(UserId, ClientId, InventoryId, SiteId, JobId, Quantity, CreatedBy, CreatedOn, descr, Comment) VALUES(src.UserId, src.ClientId, src.InventoryId, src.SiteId, src.JobId, src.Quantity, src.CreatedBy, src.CreatedOn, src.descr, src.Comment); ";
                        Requestid = databaseCon.RunCmdID(query);

                    }

                }
                else
                {
                    string query = "update tblRequestInventoryTemp Set Quantity='" + Qty + "',UpdatedBy=(select EmailId from tblUser where UserId='" + UID + "'),UpdatedOn='" + DateTime.Now + "'Where ReqTempId='" + TReqId + "'";
                    int TempTbl = databaseCon.RunCmdID(query);
                }
                //added by pooja on 23sept2025 to update desc and commens on adding next quantity ,not updating it was creating multiple entries in transaction table
                string updtdesc = "update tblRequestInventoryTemp set    descr='" + txtdesc.Replace("'", "''").Trim() + "' where UserId='" + UID + "' and jobid=" + fromid + " and siteId=" + ToId + " and    descr!='" + txtdesc.Replace("'", "''").Trim() + "'";
                databaseCon.RunCmdID(updtdesc);
                string updtcmmnt = "update tblRequestInventoryTemp set comment='" + txtComment.Replace("'", "''").Trim() + "'  where UserId='" + UID + "' and jobid=" + fromid + " and siteId=" + ToId + " and comment!='" + txtComment.Replace("'", "''").Trim() + "' ";
                databaseCon.RunCmdID(updtcmmnt);
                //}
                //else
                //{
                //    string query = "update tblRequestInventoryTemp Set Quantity='" + Qty + "',UpdatedBy=(select EmailId from tblUser where UserId='" + UID + "'),UpdatedOn='" + DateTime.Now + "'Where ReqTempId='" + tempval + "'";
                //    int TempTbl = databaseCon.RunCmdID(query);
                //}
                return Requestid;
            }
        }

        public void updateDesc(string ReqId, string Desccription, string Page, string txtComment, string txtrequesteddate, string txtrequireddate, string UID)
        {
            string updescqry;
            if (Page == "1" || Page == "3")
            {
                if (string.IsNullOrEmpty(txtrequireddate))
                {
                    updescqry = "update tblTransaction set TransDesc='" + Desccription.Replace("'", "''").Trim() + "',Comment='" + txtComment.Replace("'", "''").Trim() + "',FromTime='" + txtrequesteddate + "' where TransId='" + ReqId + "'";
                }
                else
                {
                    updescqry = "update tblTransaction set TransDesc='" + Desccription.Replace("'", "''").Trim() + "',Comment='" + txtComment.Replace("'", "''").Trim() + "',FromTime='" + txtrequesteddate + "',RequiredDate='" + txtrequireddate + "' where TransId='" + ReqId + "'";
                    int i = databaseCon.RunCmd(updescqry);
                }
            }
            else if (Page == "2")
            {
                string query = "update tblRequestInventoryTemp Set descr='" + Desccription.Replace("'", "''").Trim() + "',Comment='" + txtComment.Replace("'", "''").Trim() + "' where  UserId='" + databaseCon.Decrypt(UID) + "'";
                int TempTbl = databaseCon.RunCmdID(query);
            }

        }

        //Added on (04-03-2023) to show the Material Type Data in pop-up that is having Available Quantity zero.
        public ActionResult GetAvailableMaterial(int UID, int id, int Toid)
        {

            //var MaterialAvailabel = dbContext.Database.SqlQuery<SiteRequestViewModel>("select InventoryId, TransReqId, MaterialType, ItemCode, ItemDesc, Quantity, cast(AvailableQuantity as int) as AvailableQuantity, '' as CheckQty from (select VW_SiteInventory.InventoryID, NULL as TransReqId, MaterialType, ItemCode, ItemDesc, NULL as Quantity, AvailableQuantity from VW_SiteInventory join tblInventory on vw_SiteInventory.InventoryID = tblInventory.InventoryID where VW_SiteInventory.InventoryID not in (select InventoryID from tblRequestInventoryTemp where UserId = '" + UID + "') and VW_SiteInventory.siteId = '" + Toid + "' and SendQuantity is not null union select  VW_SiteInventory.InventoryID, tblRequestInventoryTemp.ReqTempId as TransReqId, MaterialType, ItemCode, ItemDesc, tblRequestInventoryTemp.Quantity as Quantity, AvailableQuantity from VW_SiteInventory join tblRequestInventoryTemp  on VW_SiteInventory.InventoryID = tblRequestInventoryTemp.InventoryId and tblRequestInventoryTemp.SiteId = Vw_SiteInventory.SiteId join tblInventory on vw_SiteInventory.InventoryID = tblInventory.InventoryID  where UserId = '" + UID + "') UnionQuery ,(select   MaterialType as MatType from Vw_SiteInventory join tblinventory on tblInventory.InventoryID = Vw_SiteInventory.InventoryId where SiteID = '" + Toid + "' and tblinventory.ClientId = '" + id + "' group by MaterialType having sum(availablequantity) = 0) tmp_Material_type where 1 = 1 and UnionQuery.materialtype =tmp_Material_type.MatType and Quantity is not null order by case when Quantity is null then 1 else 0 end ,MaterialType, ItemCode").ToList();

            dbContext.Database.CommandTimeout = 15 * 60;

            var MaterialAvailabel = dbContext.Database.SqlQuery<SiteRequestViewModel>("select InventoryId, TransReqId, MaterialType, ItemCode, ItemDesc, Quantity, cast(AvailableQuantity as int) as AvailableQuantity, '' as CheckQty from (select VW_SiteInventory.InventoryID, NULL as TransReqId, MaterialType, ItemCode, ItemDesc, NULL as Quantity, AvailableQuantity from VW_SiteInventory join tblInventory on vw_SiteInventory.InventoryID = tblInventory.InventoryID where VW_SiteInventory.InventoryID not in (select InventoryID from tblRequestInventoryTemp where UserId = '" + UID + "') and VW_SiteInventory.siteId = '" + Toid + "' and SendQuantity is not null union select  VW_SiteInventory.InventoryID, tblRequestInventoryTemp.ReqTempId as TransReqId, MaterialType, ItemCode, ItemDesc, tblRequestInventoryTemp.Quantity as Quantity, AvailableQuantity from VW_SiteInventory join tblRequestInventoryTemp  on VW_SiteInventory.InventoryID = tblRequestInventoryTemp.InventoryId and tblRequestInventoryTemp.SiteId = Vw_SiteInventory.SiteId join tblInventory on vw_SiteInventory.InventoryID = tblInventory.InventoryID  where UserId = '" + UID + "') UnionQuery ,(select   MaterialType as MatType from Vw_SiteInventory join tblinventory on tblInventory.InventoryID = Vw_SiteInventory.InventoryId where SiteID = '" + Toid + "' and tblinventory.ClientId = '" + id + "' group by MaterialType) tmp_Material_type where 1 = 1 and UnionQuery.materialtype =tmp_Material_type.MatType and Quantity is not null and AvailableQuantity < Quantity order by case when Quantity is null then 1 else 0 end ,MaterialType, isnull(DisplaySortOrder,9999999), ItemCode").ToList();

            return Json(MaterialAvailabel, JsonRequestBehavior.AllowGet);

        }

        public ActionResult btnCANCELMaterial_Click(string UID)
        {

            var message = "";
            string query = "delete from tblRequestInventoryTemp where UserId = '" + UID + "'";
            int dt = databaseCon.RunCmd(query);
            if (dt != 0)
            {
                message = "Success";
            }
            return Json(new { Result = message });
        }
        //End
    }
}