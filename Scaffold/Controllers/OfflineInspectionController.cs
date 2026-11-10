using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using Scaffold.App_Code;

namespace Scaffold.Controllers
{
    public class OfflineInspectionController : Controller
    {
        // GET: OfflineInspection
        public ActionResult Index()
        {
            return View();
        }

        public class checkListQues
        {
            //Declarations For Checklist Questions
            public int contentId;
            public string CheckListContent;
            public int CheckListId;
            public int SiteLocId;
            public string Location;
            public string ScaffRef;
            public string SiteLocation;
            public string Y;
            public string N;
            public string NA;
            public string IsNA;
            public string ActionRequired;
            public int ChecklistItem;
            public string itemOrder;
            public string ClientID;
            public string RegisterID;
            public string InspectionID;
            public string InspectionCheckListColor;
            public string OwnerName;
            public string DesignType;
        }

        public class checkListQuestions
        {
            //Declarations For Checklist Questions
            public int contentId;
            public string CheckListContent;
            public string Y;
            public string N;
            public string NA;
            public string IsNA;
            public string ActionRequired;
            public int ChecklistItem;
            public string itemOrder;

        }

        public class checkListHeaders
        {
            //Declarations for checkList Headers
            public int contentId;
            public string CheckListContent;
            public string InspectionID;
            public string itemOrder;
        }

        public class Inspection
        {
            public int InspectionID;
            public int RegisterID;
            public string CurrentDate;
            public string InspectionDate;
            public int InspectedById;
            public string CompleteInsp;
            public string InspStatus;
            public string IsInspector;
            public string name;
            public string InspectedBy;
            public string CreatedBy;
            public string CreatedOn;
            public string OnlineInsp;
            public string Lat;
            public string Long;
            public string Inspimage;
            public string PhotoAngle;
            public string InspAudio;
            public string Inspvedio;
            public string InspSign;
            public string InspComments;
            public string AckDate;
            public string LicenceNo;
            public string Acknowledgment;
            public string InspectionTime;
            public string OnlyInspectionDate;
            public string BuildingNo;
            public string AssetNo;
            public string FloorLevel;
            public string MaterialType;
            public string ReasonForAccess;

        }

        public class tblInspectionViewModel {
            public string InspectionID { get; set; }
            public string RegisterID { get; set; }
            public string CurrentDate { get; set; }
            public string InspectionDate { get; set; }
            public string InspectedById { get; set; }
            public string CompleteInsp { get; set; }
            public string LicenceNo { get; set; }
            public string AckDate { get; set; }
            public string OnlineInsp { get; set; }
            public string Acknowledgment { get; set; }
            public string InspSign { get; set; }
            public string InspComments { get; set; }
            public string Inspimage { get; set; }
            public string PhotoAngle { get; set; }
            public string name { get; set; }
            public string IsInspector { get; set; }
            public string InspStatus { get; set; }
            public string IsDelete { get; set; }
            public string InspectionTime { get; set; }
            public string OnlyInspectionDate { get; set; }
            public string AcknowledgeTimeZone { get; set; }
        }

        public class tblInspectionListViewModel
        {

            public string InspectionID { get; set; }
            public string RegisterID { get; set; }
            public string CurrentDate { get; set; }
            public string InspectionDate { get; set; }
            public string InspectedById { get; set; }
            public string CompleteInsp { get; set; }
            public string LicenceNo { get; set; }
            public string AckDate { get; set; }
            public string OnlineInsp { get; set; }
            public string Acknowledgment { get; set; }
            public string InspSign { get; set; }
            public string InspComments { get; set; }
            public string Inspimage { get; set; }
            public string PhotoAngle { get; set; }
            public string name { get; set; }
            public string IsInspector { get; set; }
            public string InspStatus { get; set; }
            public string IsDelete { get; set; }
            public string InspectionTime { get; set; }
            public string OnlyInspectionDate { get; set; }

            public string ContentID { get; set; }
            public string ClientID { get; set; }
            public string SiteLocId { get; set; }
            public string ScaffRef { get; set; }
            public string SiteLocation { get; set; }
            public string Location { get; set; }
            public string CheckListId { get; set; }
            public string CheckListContent { get; set; }
            public string itemOrder { get; set; }
            public string InspectionCheckListColor { get; set; }
            public string Y { get; set; }
            public string N { get; set; }
            public string NA { get; set; }
            public string IsNA { get; set; }
            public string ActionRequired { get; set; }
            public string ChecklistItem { get; set; }
            public string DesignType { get; set; }
            public string OwnerName { get; set; }
           
        }
        
        // Add New Code ON 16 Dec 2019.
        //List<Inspection> Inspections = new List<Inspection> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod]

        //public List<Inspection> GetAllInspectionDetails(string ClientID, string UID)
        List<Inspection> Inspections = new List<Inspection> { };
        public ActionResult GetAllInspectionDetails(string ClientID, string UID)
        {

            //string query = "select InspectionID,tblInspection.RegisterID,REPLACE(CONVERT(VARCHAR(11), getdate(), 106), ' ', '-') as CurrentDate,InspectionDate,InspectedById,CompleteInsp,InspStatus,tblInspection.InspectedBy,tblInspection.CreatedBy,tblInspection.CreatedOn,OnlineInsp,tblregister.ClientID,tblInspection.Lat,tblInspection.Long,Inspimage,tblInspection.PhotoAngle,InspAudio,Inspvedio,InspSign,InspComments,case when AckDate = '1900-01-01' then null else REPLACE(CONVERT(VARCHAR(11), AckDate, 106), ' ', '-') end as AckDate,Acknowledgment,LicenceNo from tblInspection left join tblregister on tblRegister.RegisterId = tblInspection.RegisterID where tblregister.ClientID = '" + databaseCon.Decrypt(ClientID) + "'and UserId = '" + databaseCon.Decrypt(UID) + "' and IsOffline = 1";

            //string query = 
            //     "Select InspectionID,tblInspection.RegisterID,REPLACE(CONVERT(VARCHAR(11), getdate(), 106), ' ', '-') as CurrentDate,InspectionDate,InspectedById,firstName+ ' '+LastName as name,IsInspector,CompleteInsp,InspStatus,tblInspection.InspectedBy,tblInspection.CreatedBy,tblInspection.CreatedOn,OnlineInsp,tblregister.ClientID,tblInspection.Lat,tblInspection.Long,Inspimage,tblInspection.PhotoAngle,InspAudio,Inspvedio,InspSign,InspComments,case when AckDate = '1900-01-01' then null else REPLACE(CONVERT(VARCHAR(11), AckDate, 106), ' ', '-') end as AckDate,Acknowledgment,tblInspection.LicenceNo from tblInspection left join tblregister on tblRegister.RegisterId = tblInspection.RegisterID left join tblUser on tblInspection.InspectedById=tblUser.UserId where tblregister.ClientID = '" + databaseCon.Decrypt(ClientID) + "'and tblInspection.UserId = '" + databaseCon.Decrypt(UID) + "' and IsOffline = 1";

            //// Change On 24 Jan 2020 Get InspectionDate And Acknowladgement Date Using vw_TblInspection
            //string query =
            //     "Select InspectionID, Vw_TblInspection.RegisterID,REPLACE(CONVERT(VARCHAR(11), getdate(), 106), ' ', '-') as CurrentDate,InspectionDate,InspectedById,firstName + ' ' + LastName as name,IsInspector,CompleteInsp,InspStatus,Vw_TblInspection.InspectedBy,Vw_TblInspection.CreatedBy,Vw_TblInspection.CreatedOn,OnlineInsp,tblregister.ClientID,Vw_TblInspection.Lat,Vw_TblInspection.Long,Inspimage,Vw_TblInspection.PhotoAngle,InspAudio,Inspvedio,InspSign,InspComments,case when AckDate = '1900-01-01' then null else REPLACE(CONVERT(VARCHAR(11), AckDate, 106), ' ', '-') end as AckDate,Acknowledgment,Vw_TblInspection.LicenceNo from Vw_TblInspection left join tblregister on tblRegister.RegisterId = Vw_TblInspection.RegisterID left join tblUser on Vw_TblInspection.InspectedById = tblUser.UserId where tblregister.ClientID ='" + databaseCon.Decrypt(ClientID) + "' and Vw_TblInspection.UserId = '" + databaseCon.Decrypt(UID) + "' and IsOffline = 1 ";

            // Change On 29 Jan 2020 add InspectionTime,OnlyInspectionDate columns.
            // string query =
            //      "Select InspectionID, Vw_TblInspection.RegisterID,REPLACE(CONVERT(VARCHAR(11), getdate(), 106), ' ', '-') as CurrentDate,InspectionTime,REPLACE(CONVERT(VARCHAR(11), InspectionDate, 106), ' ', '-') as OnlyInspectionDate,InspectionDate,InspectedById,firstName + ' ' + LastName as name,IsInspector,CompleteInsp,InspStatus,Vw_TblInspection.InspectedBy,Vw_TblInspection.CreatedBy,Vw_TblInspection.CreatedOn,OnlineInsp,tblregister.ClientID,Vw_TblInspection.Lat,Vw_TblInspection.Long,Inspimage,Vw_TblInspection.PhotoAngle,InspAudio,Inspvedio,InspSign,InspComments,case when AckDate = '1900-01-01' then null else REPLACE(CONVERT(VARCHAR(11), AckDate, 106), ' ', '-') end as AckDate,Acknowledgment,Vw_TblInspection.LicenceNo from Vw_TblInspection left join tblregister on tblRegister.RegisterId = Vw_TblInspection.RegisterID left join tblUser on Vw_TblInspection.InspectedById = tblUser.UserId where tblregister.ClientID ='" + databaseCon.Decrypt(ClientID) + "' and Vw_TblInspection.UserId = '" + databaseCon.Decrypt(UID) + "' and IsOffline = 1 ";

            //Query Change On 07 Feb 2020 add BuildingNo,AssetNo,FloorLevel,MaterialType,Description as ReasonForAccess columns.
            string query = "Select InspectionID, Vw_TblInspection.RegisterID,REPLACE(CONVERT(VARCHAR(11), getdate(), 106), ' ', '-') as CurrentDate,InspectionTime,REPLACE(CONVERT(VARCHAR(11), InspectionDate, 106), ' ', '-') as OnlyInspectionDate,InspectionDate,InspectedById,firstName + ' ' + LastName as name,IsInspector,CompleteInsp,InspStatus,Vw_TblInspection.InspectedBy,Vw_TblInspection.CreatedBy,Vw_TblInspection.CreatedOn,OnlineInsp,VW_TblRegister.ClientID,Vw_TblInspection.Lat,Vw_TblInspection.Long,Inspimage,Vw_TblInspection.PhotoAngle,InspAudio,Inspvedio,InspSign,InspComments,case when AckDate = '1900-01-01' then null else REPLACE(CONVERT(VARCHAR(11), AckDate, 106), ' ', '-') end as AckDate,Acknowledgment,Vw_TblInspection.LicenceNo,BuildingNo,AssetNo,FloorLevel,MaterialType,Description as ReasonForAccess from Vw_TblInspection left join VW_TblRegister on VW_TblRegister.RegisterId = Vw_TblInspection.RegisterID left join tblUser on Vw_TblInspection.InspectedById = tblUser.UserId where VW_TblRegister.ClientID ='" + databaseCon.Decrypt(ClientID) + "' and Vw_TblInspection.UserId = '" + databaseCon.Decrypt(UID) + "' and IsOffline = 1 ";

            DataSet ds = null;

            ds = databaseCon.GetDataSet(query);

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    Inspection ListInspection = new Inspection();

                    if (!(item["InspectionID"] is DBNull) && item["InspectionID"].ToString() != "")
                    {
                        ListInspection.InspectionID = Convert.ToInt32(item["InspectionID"].ToString());
                    }

                    if (!(item["RegisterID"] is DBNull) && item["RegisterID"].ToString() != "")
                    {
                        ListInspection.RegisterID = Convert.ToInt32(item["RegisterID"].ToString());
                    }

                    if (!(item["CurrentDate"] is DBNull) && item["CurrentDate"].ToString() != "")
                    {
                        ListInspection.CurrentDate = item["CurrentDate"].ToString();
                    }
                    else
                    {
                        ListInspection.CurrentDate = "";
                    }

                    // add on 29/Jan/2020.
                    if (!(item["InspectionTime"] is DBNull) && item["InspectionTime"].ToString() != "")
                    {
                        ListInspection.InspectionTime = item["InspectionTime"].ToString();
                    }
                    else
                    {
                        ListInspection.InspectionTime = "";
                    }

                    if (!(item["OnlyInspectionDate"] is DBNull) && item["OnlyInspectionDate"].ToString() != "")
                    {
                        ListInspection.OnlyInspectionDate = item["OnlyInspectionDate"].ToString();
                    }
                    else
                    {
                        ListInspection.OnlyInspectionDate = "";
                    }

                    if (!(item["InspectionDate"] is DBNull) && item["InspectionDate"].ToString() != "")
                    {
                        ListInspection.InspectionDate = item["InspectionDate"].ToString();
                    }
                    else
                    {
                        ListInspection.InspectionDate = "";
                    }
                    if (!(item["InspectedById"] is DBNull) && item["InspectedById"].ToString() != "")
                    {
                        ListInspection.InspectedById = Convert.ToInt32(item["InspectedById"].ToString());
                    }

                    if (!(item["name"] is DBNull) && item["name"].ToString() != "")
                    {
                        ListInspection.name = item["name"].ToString();
                    }
                    else
                    {
                        ListInspection.name = "";
                    }
                    if (!(item["IsInspector"] is DBNull) && item["IsInspector"].ToString() != "")
                    {
                        ListInspection.IsInspector = item["IsInspector"].ToString();
                    }

                    if (!(item["CompleteInsp"] is DBNull) && item["CompleteInsp"].ToString() != "")
                    {
                        ListInspection.CompleteInsp = item["CompleteInsp"].ToString();
                    }

                    if (!(item["InspStatus"] is DBNull) && item["InspStatus"].ToString() != "")
                    {
                        ListInspection.InspStatus = item["InspStatus"].ToString();
                    }
                    if (!(item["InspectedBy"] is DBNull) && item["InspectedBy"].ToString() != "")
                    {
                        ListInspection.InspectedBy = item["InspectedBy"].ToString();
                    }
                    else
                    {
                        ListInspection.InspectedBy = "";
                    }
                    if (!(item["CreatedBy"] is DBNull) && item["CreatedBy"].ToString() != "")
                    {
                        ListInspection.CreatedBy = item["CreatedBy"].ToString();
                    }
                    if (!(item["CreatedOn"] is DBNull) && item["CreatedOn"].ToString() != "")
                    {
                        ListInspection.CreatedOn = item["CreatedOn"].ToString();
                    }
                    if (!(item["OnlineInsp"] is DBNull) && item["OnlineInsp"].ToString() != "")
                    {
                        ListInspection.OnlineInsp = item["OnlineInsp"].ToString();
                    }
                    else
                    {
                        ListInspection.OnlineInsp = "";
                    }
                    if (!(item["Lat"] is DBNull) && item["Lat"].ToString() != "")
                    {
                        ListInspection.Lat = item["Lat"].ToString();
                    }
                    else
                    {
                        ListInspection.Lat = "";
                    }
                    if (!(item["Long"] is DBNull) && item["Long"].ToString() != "")
                    {
                        ListInspection.Long = item["Long"].ToString();
                    }
                    else
                    {
                        ListInspection.Long = "";
                    }
                    if (!(item["Inspimage"] is DBNull) && item["Inspimage"].ToString() != "")
                    {
                        ListInspection.Inspimage = item["Inspimage"].ToString();
                    }
                    else
                    {
                        ListInspection.Inspimage = "";
                    }
                    if (!(item["PhotoAngle"] is DBNull) && item["PhotoAngle"].ToString() != "")
                    {
                        ListInspection.PhotoAngle = item["PhotoAngle"].ToString();
                    }
                    else
                    {
                        ListInspection.PhotoAngle = "";
                    }
                    if (!(item["InspAudio"] is DBNull) && item["InspAudio"].ToString() != "")
                    {
                        ListInspection.InspAudio = item["InspAudio"].ToString();
                    }
                    else
                    {
                        ListInspection.InspAudio = "";
                    }
                    if (!(item["Inspvedio"] is DBNull) && item["Inspvedio"].ToString() != "")
                    {
                        ListInspection.Inspvedio = item["Inspvedio"].ToString();
                    }
                    else
                    {
                        ListInspection.Inspvedio = "";
                    }
                    if (!(item["InspSign"] is DBNull) && item["InspSign"].ToString() != "")
                    {
                        ListInspection.InspSign = item["InspSign"].ToString();
                    }
                    else
                    {
                        ListInspection.InspSign = "";
                    }
                    if (!(item["InspComments"] is DBNull) && item["InspComments"].ToString() != "")
                    {
                        ListInspection.InspComments = item["InspComments"].ToString();
                    }
                    else
                    {
                        ListInspection.InspComments = "";
                    }
                    if (!(item["AckDate"] is DBNull) && item["AckDate"].ToString() != "")
                    {
                        ListInspection.AckDate = item["AckDate"].ToString();
                    }
                    else
                    {
                        ListInspection.AckDate = "";
                    }
                    if (!(item["Acknowledgment"] is DBNull) && item["Acknowledgment"].ToString() != "")
                    {
                        ListInspection.Acknowledgment = item["Acknowledgment"].ToString();
                    }
                    else
                    {
                        ListInspection.Acknowledgment = "";
                    }
                    if (!(item["LicenceNo"] is DBNull) && item["LicenceNo"].ToString() != "")
                    {
                        ListInspection.LicenceNo = item["LicenceNo"].ToString();
                    }
                    else
                    {
                        ListInspection.LicenceNo = "";
                    }
                    // Line Add On 07 Feb 2020 BuildingNo,AssetNo,FloorLevel,MaterialType,Description as ReasonForAccess
                    if (!(item["BuildingNo"] is DBNull) && item["BuildingNo"].ToString() != "")
                    {
                        ListInspection.BuildingNo = item["BuildingNo"].ToString();
                    }
                    else
                    {
                        ListInspection.BuildingNo = "";
                    }
                    if (!(item["AssetNo"] is DBNull) && item["AssetNo"].ToString() != "")
                    {
                        ListInspection.AssetNo = item["AssetNo"].ToString();
                    }
                    else
                    {
                        ListInspection.AssetNo = "";
                    }
                    if (!(item["FloorLevel"] is DBNull) && item["FloorLevel"].ToString() != "")
                    {
                        ListInspection.FloorLevel = item["FloorLevel"].ToString();
                    }
                    else
                    {
                        ListInspection.FloorLevel = "";
                    }
                    if (!(item["MaterialType"] is DBNull) && item["MaterialType"].ToString() != "")
                    {
                        ListInspection.MaterialType = item["MaterialType"].ToString();
                    }
                    else
                    {
                        ListInspection.MaterialType = "";
                    }
                    if (!(item["ReasonForAccess"] is DBNull) && item["ReasonForAccess"].ToString() != "")
                    {
                        ListInspection.ReasonForAccess = item["ReasonForAccess"].ToString();
                    }
                    else
                    {
                        ListInspection.ReasonForAccess = "";
                    }
                    Inspections.Add(ListInspection);
                }
            }
            // return Inspections;
            return Json(Inspections, JsonRequestBehavior.AllowGet);
        }


        // change on 27Nov 2019
        //List<checkListHeaders> CheckListHeader = new List<checkListHeaders> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod]

        //public List<checkListHeaders> GetAllCheckListHeadersMultiple(string ClientID, string UID)
        List<checkListHeaders> CheckListHeader = new List<checkListHeaders> { };
        public ActionResult GetAllCheckListHeadersMultiple(string ClientID, string UID)
        {
            // Query Change On 27 Nov 2019
            string query = " SELECT InspectionID,tblInspectionList.CheckListID,a.contentId,ChecklistItem,itemOrder,CheckListContent,Case when checklistItem IS null and subChildItem IS null then 1 else 0 end as Header FROM tblChecklistContent   a left join tblInspectionList On tblInspectionList.ContentID = a.ContentID where Case when checklistItem IS null and subChildItem IS null then 1 else 0 end = 1 and a.ClientID = '" + databaseCon.Decrypt(ClientID) + "' and a.checklistID In (  select distinct  tblInspectionList.CheckListID from tblInspection left join tblInspectionList on tblInspection.InspectionID = tblInspectionList.InspectionID where IsOffline = 1 and UserId = '" + databaseCon.Decrypt(UID) + "' ) and tblInspectionList.InspectionID In (select distinct tblInspection.InspectionID from tblInspection left join tblInspectionList on tblInspection.InspectionID = tblInspectionList.InspectionID where IsOffline = 1 and UserId = '" + databaseCon.Decrypt(UID) + "' )";


            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    checkListHeaders ListHeaders = new checkListHeaders();
                    if (!(item["contentId"] is DBNull) && item["contentId"].ToString() != "")
                    {
                        ListHeaders.contentId = Convert.ToInt32(item["contentId"].ToString());
                    }

                    if (!(item["CheckListContent"] is DBNull) && item["CheckListContent"].ToString() != "")
                    {
                        ListHeaders.CheckListContent = item["CheckListContent"].ToString();
                    }
                    if (!(item["InspectionID"] is DBNull) && item["InspectionID"].ToString() != "")
                    {
                        ListHeaders.InspectionID = item["InspectionID"].ToString();
                    }
                    if (!(item["itemOrder"] is DBNull) && item["itemOrder"].ToString() != "")
                    {
                        ListHeaders.itemOrder = item["itemOrder"].ToString();
                    }
                    CheckListHeader.Add(ListHeaders);
                }
            }
            // return CheckListHeader;
            return Json(CheckListHeader,JsonRequestBehavior.AllowGet);
        }


        List<checkListQues> CheckListContent = new List<checkListQues> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod]
        // public List<checkListQues> GetAllCheckListContentMultiple(string ClientID, string UID)
        public ActionResult GetAllCheckListContentMultiple(string ClientID, string UID)
        {
            //Line Change On 27 Nov 2019

            //string query3 = "SELECT InspectionListID,tblInspectionList.RegisterId,VW_TblRegister.SiteLocId,VW_TblRegister.SiteLocation,VW_TblRegister.Location,VW_TblRegister.ScaffRef,InspectionID,a.contentId,a.ClientID,ChecklistItem,SubChildItem, case when checklistItem is null and subChildItem is null then CONVERT(varchar, itemorder)+'.0' Else case when checklistItem is not null and subChildItem is not null then Convert(varchar, (select Convert(varchar, itemOrder) from tblChecklistContent where ContentID = a.checklistItem) )+'.' + (select Convert(varchar, itemOrder) from tblChecklistContent where ContentID = a.subChildItem)+'.' + CONVERT(varchar, itemorder) Else Convert(varchar, (select Convert(varchar, itemOrder) from tblChecklistContent where ContentID = a.checklistItem) )+'.' + CONVERT(varchar, itemorder) End End as itemorder, CheckListContent,a.ChecklistID,ISNULL(Y, 'False') as Y,ISNULL(N, 'False') as N,ISNULL(NA, 'False') as NA,ActionRequired,IsNA FROM tblChecklistContent a left join tblInspectionList On  tblInspectionList.ContentID = a.ContentID left join VW_TblRegister on VW_TblRegister.RegisterId = tblInspectionList.RegisterId where SubChildItem is null and a.ClientID = '" + databaseCon.Decrypt(ClientID) + "' and a.checklistID In (select distinct  tblInspectionList.CheckListID from tblInspection left join tblInspectionList on tblInspection.InspectionID = tblInspectionList.InspectionID where IsOffline = 1 and UserId ='" + databaseCon.Decrypt(UID) + "' ) and tblInspectionList.InspectionID In (select distinct tblInspection.InspectionID from tblInspection left join tblInspectionList on tblInspection.InspectionID = tblInspectionList.InspectionID where IsOffline = 1 and UserId ='" + databaseCon.Decrypt(UID) + "') ";

            // Line Change On 10 Dec 2019. for InspectionCheckListColor
            //string query3 = "SELECT InspectionListID,tblInspectionList.RegisterId,VW_TblRegister.SiteLocId,VW_TblRegister.SiteLocation,VW_TblRegister.Location,VW_TblRegister.ScaffRef,InspectionID,a.contentId,a.ClientID,ChecklistItem,SubChildItem, case when checklistItem is null and subChildItem is null then CONVERT(varchar, itemorder)+'.0' Else case when checklistItem is not null and subChildItem is not null then Convert(varchar, (select Convert(varchar, itemOrder) from tblChecklistContent where ContentID = a.checklistItem) )+'.' + (select Convert(varchar, itemOrder) from tblChecklistContent where ContentID = a.subChildItem)+'.' + CONVERT(varchar, itemorder) Else Convert(varchar, (select Convert(varchar, itemOrder) from tblChecklistContent where ContentID = a.checklistItem) )+'.' + CONVERT(varchar, itemorder) End End as itemorder, CheckListContent,a.ChecklistID,ISNULL(Y, 'False') as Y,ISNULL(N, 'False') as N,ISNULL(NA, 'False') as NA,ActionRequired,IsNA,isnull(InspectionCheckListColor,0) as InspectionCheckListColor FROM tblChecklistContent a left join tblInspectionList On  tblInspectionList.ContentID = a.ContentID left join VW_TblRegister on VW_TblRegister.RegisterId = tblInspectionList.RegisterId left join tblClient on tblClient.ClientId=a.ClientID where SubChildItem is null and a.ClientID = '" + databaseCon.Decrypt(ClientID) + "' and a.checklistID In (select distinct  tblInspectionList.CheckListID from tblInspection left join tblInspectionList on tblInspection.InspectionID = tblInspectionList.InspectionID where IsOffline = 1 and UserId ='" + databaseCon.Decrypt(UID) + "' ) and tblInspectionList.InspectionID In (select distinct tblInspection.InspectionID from tblInspection left join tblInspectionList on tblInspection.InspectionID = tblInspectionList.InspectionID where IsOffline = 1 and UserId ='" + databaseCon.Decrypt(UID) + "') ";


            // Line Change On 16 Dec 2019.
            // string query3 = 
            //   "SELECT InspectionListID,tblInspectionList.RegisterId,VW_TblRegister.SiteLocId,VW_TblRegister.SiteLocation,VW_TblRegister.Location,Right('000000'+Convert(varchar,ScaffRef),6) as ScaffRef,VW_TblRegister.DesignType,VW_TblRegister.OwnerName,InspectionID,a.contentId,a.ClientID,ChecklistItem,SubChildItem, case when checklistItem is null and subChildItem is null then CONVERT(varchar, itemorder)+'.0' Else case when checklistItem is not null and subChildItem is not null then Convert(varchar, (select Convert(varchar, itemOrder) from tblChecklistContent where ContentID = a.checklistItem) )+'.' + (select Convert(varchar, itemOrder) from tblChecklistContent where ContentID = a.subChildItem)+'.' + CONVERT(varchar, itemorder) Else Convert(varchar, (select Convert(varchar, itemOrder) from tblChecklistContent where ContentID = a.checklistItem) )+'.' + CONVERT(varchar, itemorder) End End as itemorder, CheckListContent,a.ChecklistID,ISNULL(Y, 'False') as Y,ISNULL(N, 'False') as N,ISNULL(NA, 'False') as NA,ActionRequired,IsNA,isnull(InspectionCheckListColor,0) as InspectionCheckListColor FROM tblChecklistContent a left join tblInspectionList On  tblInspectionList.ContentID = a.ContentID left join VW_TblRegister on VW_TblRegister.RegisterId = tblInspectionList.RegisterId left join tblClient on tblClient.ClientId=a.ClientID where SubChildItem is null and a.ClientID = '" + databaseCon.Decrypt(ClientID) + "' and a.checklistID In (select distinct  tblInspectionList.CheckListID from tblInspection left join tblInspectionList on tblInspection.InspectionID = tblInspectionList.InspectionID where IsOffline = 1 and UserId ='" + databaseCon.Decrypt(UID) + "' ) and tblInspectionList.InspectionID In (select distinct tblInspection.InspectionID from tblInspection left join tblInspectionList on tblInspection.InspectionID = tblInspectionList.InspectionID where IsOffline = 1 and UserId ='" + databaseCon.Decrypt(UID) + "') ";

            string query3 = "SELECT InspectionListID, tblInspectionList.RegisterId,VW_TblRegister.SiteLocId,VW_TblRegister.SiteLocation,VW_TblRegister.Location,Right('000000' + Convert(varchar, ScaffRef), 6) as ScaffRef,VW_TblRegister.DesignType,VW_TblRegister.OwnerName,InspectionID,a.contentId,a.ClientID,ChecklistItem,SubChildItem,  case when checklistItem is null and subChildItem is null then Right('00' + CONVERT(varchar, itemorder), 2)+'.0' Else case when checklistItem is not null and subChildItem is not null then Convert(varchar, (select Right('00' + Convert(varchar, itemOrder), 2) from tblChecklistContent where ContentID = a.checklistItem) )+'.' + (select Right('00' + Convert(varchar, itemOrder), 2) from tblChecklistContent where ContentID = a.subChildItem)+'.' + Right('000' + CONVERT(varchar, itemorder), 3) Else Right('00' + Convert(varchar, (select Convert(varchar, itemOrder) from tblChecklistContent where ContentID = a.checklistItem)), 2)+'.' + Right('000' + CONVERT(varchar, itemorder), 3) End End as itemorder, CheckListContent,a.ChecklistID,ISNULL(Y, 'False') as Y,ISNULL(N, 'False') as N,ISNULL(NA, 'False') as NA,ActionRequired,IsNA,isnull(InspectionCheckListColor, 0) as InspectionCheckListColor FROM tblChecklistContent a left join tblInspectionList On tblInspectionList.ContentID = a.ContentID left join VW_TblRegister on VW_TblRegister.RegisterId = tblInspectionList.RegisterId left join tblClient on tblClient.ClientId = a.ClientID where SubChildItem is null and a.ClientID = '" + databaseCon.Decrypt(ClientID) + "' and a.checklistID In (select distinct  tblInspectionList.CheckListID from tblInspection left join tblInspectionList on tblInspection.InspectionID = tblInspectionList.InspectionID where IsOffline = 1 and UserId ='" + databaseCon.Decrypt(UID) + "') and tblInspectionList.InspectionID In (select distinct tblInspection.InspectionID from tblInspection left join tblInspectionList on tblInspection.InspectionID = tblInspectionList.InspectionID where IsOffline = 1 and UserId = '" + databaseCon.Decrypt(UID) + "') ";


            DataSet ds = null;
            ds = databaseCon.GetDataSet(query3);

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    checkListQues ListContent = new checkListQues();
                    if (!(item["contentId"] is DBNull) && item["contentId"].ToString() != "")
                    {
                        ListContent.contentId = Convert.ToInt32(item["contentId"].ToString());
                    }
                    if (!(item["ClientID"] is DBNull) && item["ClientID"].ToString() != "")
                    {
                        ListContent.ClientID = item["ClientID"].ToString();
                    }
                    if (!(item["contentId"] is DBNull) && item["contentId"].ToString() != "")
                    {
                        ListContent.RegisterID = "";  // For Testing
                    }
                    if (!(item["InspectionID"] is DBNull) && item["InspectionID"].ToString() != "")
                    {
                        ListContent.InspectionID = item["InspectionID"].ToString();
                    }
                    if (!(item["ChecklistItem"] is DBNull) && item["ChecklistItem"].ToString() != "")
                    {
                        ListContent.ChecklistItem = Convert.ToInt32(item["ChecklistItem"].ToString());
                    }

                    // Changes Done  on 28-Nov-2019

                    if (!(item["SiteLocId"] is DBNull) && item["SiteLocId"].ToString() != "")
                    {
                        ListContent.SiteLocId = Convert.ToInt32(item["SiteLocId"].ToString());
                    }
                    if (!(item["Location"] is DBNull) && item["Location"].ToString() != "")
                    {
                        ListContent.Location = item["Location"].ToString();
                    }
                    else
                    {
                        ListContent.Location = "";
                    }
                    if (!(item["SiteLocation"] is DBNull) && item["SiteLocation"].ToString() != "")
                    {
                        ListContent.SiteLocation = item["SiteLocation"].ToString();
                    }
                    else
                    {
                        ListContent.SiteLocation = "";
                    }
                    if (!(item["ScaffRef"] is DBNull) && item["ScaffRef"].ToString() != "")
                    {
                        ListContent.ScaffRef = item["ScaffRef"].ToString();
                    }
                    else
                    {
                        ListContent.ScaffRef = "";
                    }
                    if (!(item["CheckListId"] is DBNull) && item["CheckListId"].ToString() != "")
                    {
                        ListContent.CheckListId = Convert.ToInt32(item["CheckListId"].ToString());
                    }
                    if (!(item["RegisterID"] is DBNull) && item["RegisterID"].ToString() != "")
                    {
                        ListContent.RegisterID = item["RegisterID"].ToString();
                    }
                    // changes done on 28-NOV-2019

                    if (!(item["CheckListContent"] is DBNull) && item["CheckListContent"].ToString() != "")
                    {
                        ListContent.CheckListContent = item["CheckListContent"].ToString();
                    }
                    if (!(item["Y"] is DBNull) && item["Y"].ToString() != "")
                    {
                        ListContent.Y = item["Y"].ToString().ToLower();
                    }
                    if (!(item["N"] is DBNull) && item["N"].ToString() != "")
                    {
                        ListContent.N = item["N"].ToString().ToLower();
                    }
                    if (!(item["NA"] is DBNull) && item["NA"].ToString() != "")
                    {
                        ListContent.NA = item["NA"].ToString().ToLower();
                    }
                    if (!(item["IsNA"] is DBNull) && item["IsNA"].ToString() != "")
                    {
                        ListContent.IsNA = item["IsNA"].ToString().ToLower();
                    }
                    if (!(item["ActionRequired"] is DBNull) && item["ActionRequired"].ToString() != "")
                    {
                        var Action = item["ActionRequired"].ToString();
                        ListContent.ActionRequired = item["ActionRequired"].ToString();
                    }
                    if (!(item["itemOrder"] is DBNull) && item["itemOrder"].ToString() != "")
                    {
                        ListContent.itemOrder = item["itemOrder"].ToString();
                    }
                    if (!(item["InspectionCheckListColor"] is DBNull) && item["InspectionCheckListColor"].ToString() != "")
                    {
                        ListContent.InspectionCheckListColor = item["InspectionCheckListColor"].ToString().ToLower();
                    }
                    if (!(item["DesignType"] is DBNull) && item["DesignType"].ToString() != "")
                    {
                        ListContent.DesignType = item["DesignType"].ToString();
                    }
                    else
                    {
                        ListContent.DesignType = "";
                    }
                    if (!(item["OwnerName"] is DBNull) && item["OwnerName"].ToString() != "")
                    {
                        ListContent.OwnerName = item["OwnerName"].ToString();
                    }
                    else
                    {
                        ListContent.OwnerName = "";
                    }
                    CheckListContent.Add(ListContent);
                }
            }
            // return CheckListContent;
            return Json(CheckListContent, JsonRequestBehavior.AllowGet);

        }
        

        public string updateCheckList(List<tblInspectionListViewModel> ListData, List<tblInspectionViewModel> ListDataInsp, string InspID, string GetOfflineStatus, string UID)
        {
            try
            {

                foreach (tblInspectionListViewModel item in ListData)
                {
                    var Array = item;
                }

                foreach (tblInspectionViewModel item in ListDataInsp)
                {
                    var Array = item;
                }

                var UserEmail = "";
                string userqry = "select EmailID from tbluser where UserId = '" + databaseCon.Decrypt(UID) + "'";
                DataTable dt = databaseCon.GetData(userqry);
                if (dt.Rows.Count > 0)
                {
                    UserEmail = dt.Rows[0]["EmailID"].ToString();
                }
                string jsonString = JsonConvert.SerializeObject(ListData);
                dynamic x = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonString);
                var checkflag = true;
                var NewRID = 0;
                var NewInspectionID = 0;
                var Message = "";
                var ClientID = "";
                bool InspectionStatusFlag = false;
                // foreach (var item in x)
                foreach (tblInspectionListViewModel item in ListData)
                {

                    string Y = item.Y;
                    string N = item.N;
                    string NA = item.NA;
                    string Action = item.ActionRequired;
                    string ContentID = item.ContentID;
                    // add change on 30 NOV 2019
                    int InspectionID = Convert.ToInt32(item.InspectionID);
                    int RegisterID = Convert.ToInt32(item.RegisterID);
                    ClientID = item.ClientID;

                    if ((NewRID != RegisterID) && (NewInspectionID != InspectionID))
                    {
                        NewRID = RegisterID;
                        NewInspectionID = InspectionID;
                        // Add code on 25 DEC 2019.
                        string querySelect = "Select COUNT(*) as InspectionCount from tblInspection where InspectionID = '" + InspectionID + "' and CompleteInsp != 1 and InspStatus not in ('Confirmed', 'Out Of Service')";
                        DataTable resultSelect = databaseCon.GetData(querySelect);
                        if (Convert.ToInt32(resultSelect.Rows[0]["InspectionCount"]) > 0)
                        {
                            InspectionStatusFlag = true;
                        }
                    }

                    // string query = "update tblInspectionList set UpdatedOn='" + DateTime.Now + "',Y='" + Y + "',N='" + N + "',NA='" + NA + "',ActionRequired='" + Action + "' where ContentID='" + ContentID + "' and InspectionID = '" + databaseCon.Decrypt(InspID) + "'";


                    if (InspectionStatusFlag == true)
                    {
                        string query = "update tblInspectionList set UpdatedBy='" + UserEmail + "', UpdatedOn='" + DateTime.Now + "',Y='" + Y + "',N='" + N + "',NA='" + NA + "',ActionRequired='" + Action + "' where ContentID='" + ContentID + "' and InspectionID = '" + InspectionID + "'";

                        // add change on 30 NOV 2019
                        int result = databaseCon.RunCmd(query);
                    }
                }

                string jsonStringInsp = JsonConvert.SerializeObject(ListDataInsp);
                dynamic y = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonStringInsp);

               // foreach (var item1 in y)

                    foreach (tblInspectionViewModel item1 in ListDataInsp)
                    {
                    int InspectionID = Convert.ToInt32(item1.InspectionID);
                    int RegisterID = Convert.ToInt32(item1.RegisterID);
                    string CompleteInsp = item1.CompleteInsp;
                    string AckDate = item1.AckDate;
                    string LicenceNo = item1.LicenceNo;
                    string Acknowledgment = item1.Acknowledgment;
                    string InspSign = item1.InspSign;
                    string InspComments = item1.InspComments;
                    string Inspimage = item1.Inspimage;
                    string PhotoAngle = item1.PhotoAngle;
                    string InspStatus = item1.InspStatus;
                    string IsDelete = item1.IsDelete;
                    string InspectedBy = item1.name;
                    string AcknowledgeTimeZone = item1.AcknowledgeTimeZone;

                    // Add code on 25 DEC 2019.
                    string querySelect = "Select COUNT(*) as InspectionCount from tblInspection where InspectionID = '" + InspectionID + "' and CompleteInsp != 1 and InspStatus not in ('Confirmed', 'Out Of Service')";
                    DataTable resultSelect = databaseCon.GetData(querySelect);
                    if (Convert.ToInt32(resultSelect.Rows[0]["InspectionCount"]) > 0)
                    {
                        //  if ((InspStatus == "Scheduled"))
                        if ((InspStatus == "Scheduled") || (IsDelete == "1"))
                        {
                            //if (IsDelete == "1")
                            //    {
                            string query = "exec  Pro_TBLInspection_DEL'" + InspectionID + "'";
                            DataTable dtinsp = databaseCon.GetData(query);
                            if (dtinsp.Rows.Count != 0)
                            {

                            }
                        }
                        else
                        {
                          
                            if (Inspimage != "" && Inspimage !=null)
                            {
                                var Image = Inspimage.Split('.');
                                if (Image.Length == 1)
                                {
                                    imageUpload(Inspimage, InspectionID);
                                }

                            }
                            if (InspSign != "" && InspSign != null)
                            {
                                var Signature = InspSign.Split('.');

                                // if(Signature[1] != "png")
                                if (Signature.Length == 1)
                                {
                                    SignatureUpload(InspSign, InspectionID);
                                }
                            }

                            // string qry = "update tblInspection set InspectedBy='"+ InspectedBy + "', CompleteInsp ='" + CompleteInsp + "',AckDate='" + AckDate + "',InspComments='" + InspComments + "',PhotoAngle='" + PhotoAngle + "',Acknowledgment='" + Acknowledgment + "',InspStatus='" + InspStatus + "',OnlineInsp=0,IsOffline=0,UpdatedBy='" + UserEmail + "', UpdatedOn='" + DateTime.Now + "' where InspectionID= '" + InspectionID + "' and RegisterID= '" + RegisterID + "'";

                            // Change On 24 Jan 2020 Add InspectionTimeZone To Diffrent Between TimeZone  And Inspection Date Save To Utc Time Zone
                            string qry = "update tblInspection set InspectedBy='" + InspectedBy + "', CompleteInsp ='" + CompleteInsp + "',AckDate='" + Convert.ToDateTime(AckDate).AddMinutes(Convert.ToDouble(AcknowledgeTimeZone)) + "',InspComments='" + InspComments + "',PhotoAngle='" + PhotoAngle + "',Acknowledgment='" + Acknowledgment + "',InspStatus='" + InspStatus + "',OnlineInsp=0,IsOffline=0,UpdatedBy='" + UserEmail + "', UpdatedOn='" + DateTime.Now + "',AcknowledgeTimeZone=" + ((Convert.ToDouble(AcknowledgeTimeZone)) * (-1)) + " where InspectionID= '" + InspectionID + "' and RegisterID= '" + RegisterID + "'";

                            int id = databaseCon.RunCmd(qry);

                            // Comment On 10 Feb 2020 No Pdf Genrate in Online Case Pdf Generate In Console Application

                            //if (id > 0)
                            //{
                            //    //if ((InspStatus == "Confirmed") || (InspStatus == "Out of Service"))
                            //    //{

                            //    // Genrate PDF If Inspection Is Confirm And Out OF Service

                            //    var Result = databaseCon.InspectionPdfReportCreate(Convert.ToInt32(ClientID), Convert.ToInt32(InspectionID));

                            //    var NewResult = Result.Split('|');
                            //    var FileName = NewResult[0];
                            //    var PdfFileFullPath = NewResult[1];


                            //    string path = databaseCon.registerdocumentlink + FileName;
                            //    var uri = new Uri(databaseCon.registerdocumentlink);
                            //    var container = new CloudBlobContainer(uri);
                            //    var blob = container.GetBlockBlobReference(FileName);
                            //    var exists = blob.Exists();
                            //    if (exists == false)
                            //    {
                            //    }
                            //    else
                            //    {
                            //        databaseCon.deletefileonblob(FileName, "registerdocument");
                            //    }
                            //    string extension = Path.GetExtension(".pdf");
                            //    if (extension.Replace(".", "") == "pdf")
                            //    {
                            //        extension = "application/pdf";
                            //    }

                            //    databaseCon.savefileonblob(FileName, "Inspection-Document", PdfFileFullPath, extension.Replace(".", ""));
                            //    if (File.Exists(PdfFileFullPath))
                            //    {
                            //        try
                            //        {
                            //            File.Delete(PdfFileFullPath);
                            //        }
                            //        catch
                            //        {

                            //        }
                            //    }

                            //    string query = "update tblinspection set UpdatedBy='" + UserEmail + "', UpdatedOn='" + DateTime.Now + "', InspFileName='" + FileName + "' where InspectionID ='" + InspectionID + "'";
                            //    int result = databaseCon.RunCmd(query);
                            //    //  }
                            //}
                        }
                    }
                }


                Message = "success_" + "" + GetOfflineStatus + "";
                // return "success";
                return Message;

            }
            catch (Exception ex)
            {
                var Message = "Failed_" + ex.Message;
                return Message;
            }

        }

        //public string updateCheckList(dynamic ListData, dynamic ListDataInsp, string InspID, string GetOfflineStatus, string UID)
        //{
        //    try
        //    {
        //        var UserEmail = "";
        //        string userqry = "select EmailID from tbluser where UserId = '" + databaseCon.Decrypt(UID) + "'";
        //        DataTable dt = databaseCon.GetData(userqry);
        //        if (dt.Rows.Count > 0)
        //        {
        //            UserEmail = dt.Rows[0]["EmailID"].ToString();
        //        }
        //        string jsonString = JsonConvert.SerializeObject(ListData);
        //        dynamic x = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonString);
        //        var checkflag = true;
        //        var NewRID = 0;
        //        var NewInspectionID = 0;
        //        var Message = "";
        //        var ClientID = "";
        //        bool InspectionStatusFlag = false;
        //        foreach (var item in x)
        //        {

        //            string Y = item.Y;
        //            string N = item.N;
        //            string NA = item.NA;
        //            string Action = item.ActionRequired;
        //            string ContentID = item.ContentID;
        //            // add change on 30 NOV 2019
        //            int InspectionID = item.InspectionID;
        //            int RegisterID = item.RegisterID;
        //            ClientID = item.ClientID;

        //            if ((NewRID != RegisterID) && (NewInspectionID != InspectionID))
        //            {
        //                NewRID = RegisterID;
        //                NewInspectionID = InspectionID;
        //                // Add code on 25 DEC 2019.
        //                string querySelect = "Select COUNT(*) as InspectionCount from tblInspection where InspectionID = '" + InspectionID + "' and CompleteInsp != 1 and InspStatus not in ('Confirmed', 'Out Of Service')";
        //                DataTable resultSelect = databaseCon.GetData(querySelect);
        //                if (Convert.ToInt32(resultSelect.Rows[0]["InspectionCount"]) > 0)
        //                {
        //                    InspectionStatusFlag = true;
        //                }
        //            }

        //            // string query = "update tblInspectionList set UpdatedOn='" + DateTime.Now + "',Y='" + Y + "',N='" + N + "',NA='" + NA + "',ActionRequired='" + Action + "' where ContentID='" + ContentID + "' and InspectionID = '" + databaseCon.Decrypt(InspID) + "'";


        //            if (InspectionStatusFlag == true)
        //            {
        //                string query = "update tblInspectionList set UpdatedBy='" + UserEmail + "', UpdatedOn='" + DateTime.Now + "',Y='" + Y + "',N='" + N + "',NA='" + NA + "',ActionRequired='" + Action + "' where ContentID='" + ContentID + "' and InspectionID = '" + InspectionID + "'";

        //                // add change on 30 NOV 2019
        //                int result = databaseCon.RunCmd(query);
        //            }
        //        }

        //        string jsonStringInsp = JsonConvert.SerializeObject(ListDataInsp);
        //        dynamic y = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonStringInsp);

        //        foreach (var item1 in y)
        //        {
        //            int InspectionID = item1.InspectionID;
        //            int RegisterID = item1.RegisterID;
        //            string CompleteInsp = item1.CompleteInsp;
        //            string AckDate = item1.AckDate;
        //            string LicenceNo = item1.LicenceNo;
        //            string Acknowledgment = item1.Acknowledgment;
        //            string InspSign = item1.InspSign;
        //            string InspComments = item1.InspComments;
        //            string Inspimage = item1.Inspimage;
        //            string PhotoAngle = item1.PhotoAngle;
        //            string InspStatus = item1.InspStatus;
        //            string IsDelete = item1.IsDelete;
        //            string InspectedBy = item1.name;
        //            string AcknowledgeTimeZone = item1.AcknowledgeTimeZone;

        //            // Add code on 25 DEC 2019.
        //            string querySelect = "Select COUNT(*) as InspectionCount from tblInspection where InspectionID = '" + InspectionID + "' and CompleteInsp != 1 and InspStatus not in ('Confirmed', 'Out Of Service')";
        //            DataTable resultSelect = databaseCon.GetData(querySelect);
        //            if (Convert.ToInt32(resultSelect.Rows[0]["InspectionCount"]) > 0)
        //            {
        //                if ((InspStatus == "Scheduled"))
        //                {
        //                    //if (IsDelete == "1")
        //                    //    {
        //                    string query = "exec  Pro_TBLInspection_DEL'" + InspectionID + "'";
        //                    DataTable dtinsp = databaseCon.GetData(query);
        //                    if (dtinsp.Rows.Count != 0)
        //                    {

        //                    }
        //                }
        //                else
        //                {
        //                    var Signature = InspSign.Split('.');
        //                    var Image = Inspimage.Split('.');
        //                    if (Inspimage != "")
        //                    {

        //                        if (Image.Length == 1)
        //                        {
        //                            imageUpload(Inspimage, InspectionID);
        //                        }

        //                    }
        //                    if (InspSign != "")
        //                    {
        //                        // if(Signature[1] != "png")
        //                        if (Signature.Length == 1)
        //                        {
        //                            SignatureUpload(InspSign, InspectionID);
        //                        }
        //                    }

        //                    // string qry = "update tblInspection set InspectedBy='"+ InspectedBy + "', CompleteInsp ='" + CompleteInsp + "',AckDate='" + AckDate + "',InspComments='" + InspComments + "',PhotoAngle='" + PhotoAngle + "',Acknowledgment='" + Acknowledgment + "',InspStatus='" + InspStatus + "',OnlineInsp=0,IsOffline=0,UpdatedBy='" + UserEmail + "', UpdatedOn='" + DateTime.Now + "' where InspectionID= '" + InspectionID + "' and RegisterID= '" + RegisterID + "'";

        //                    // Change On 24 Jan 2020 Add InspectionTimeZone To Diffrent Between TimeZone  And Inspection Date Save To Utc Time Zone
        //                    string qry = "update tblInspection set InspectedBy='" + InspectedBy + "', CompleteInsp ='" + CompleteInsp + "',AckDate='" + Convert.ToDateTime(AckDate).AddMinutes(Convert.ToDouble(AcknowledgeTimeZone)) + "',InspComments='" + InspComments + "',PhotoAngle='" + PhotoAngle + "',Acknowledgment='" + Acknowledgment + "',InspStatus='" + InspStatus + "',OnlineInsp=0,IsOffline=0,UpdatedBy='" + UserEmail + "', UpdatedOn='" + DateTime.Now + "',AcknowledgeTimeZone=" + ((Convert.ToDouble(AcknowledgeTimeZone)) * (-1)) + " where InspectionID= '" + InspectionID + "' and RegisterID= '" + RegisterID + "'";

        //                    int id = databaseCon.RunCmd(qry);

        //                    // Comment On 10 Feb 2020 No Pdf Genrate in Online Case Pdf Generate In Console Application

        //                    //if (id > 0)
        //                    //{
        //                    //    //if ((InspStatus == "Confirmed") || (InspStatus == "Out of Service"))
        //                    //    //{

        //                    //    // Genrate PDF If Inspection Is Confirm And Out OF Service

        //                    //    var Result = databaseCon.InspectionPdfReportCreate(Convert.ToInt32(ClientID), Convert.ToInt32(InspectionID));

        //                    //    var NewResult = Result.Split('|');
        //                    //    var FileName = NewResult[0];
        //                    //    var PdfFileFullPath = NewResult[1];


        //                    //    string path = databaseCon.registerdocumentlink + FileName;
        //                    //    var uri = new Uri(databaseCon.registerdocumentlink);
        //                    //    var container = new CloudBlobContainer(uri);
        //                    //    var blob = container.GetBlockBlobReference(FileName);
        //                    //    var exists = blob.Exists();
        //                    //    if (exists == false)
        //                    //    {
        //                    //    }
        //                    //    else
        //                    //    {
        //                    //        databaseCon.deletefileonblob(FileName, "registerdocument");
        //                    //    }
        //                    //    string extension = Path.GetExtension(".pdf");
        //                    //    if (extension.Replace(".", "") == "pdf")
        //                    //    {
        //                    //        extension = "application/pdf";
        //                    //    }

        //                    //    databaseCon.savefileonblob(FileName, "Inspection-Document", PdfFileFullPath, extension.Replace(".", ""));
        //                    //    if (File.Exists(PdfFileFullPath))
        //                    //    {
        //                    //        try
        //                    //        {
        //                    //            File.Delete(PdfFileFullPath);
        //                    //        }
        //                    //        catch
        //                    //        {

        //                    //        }
        //                    //    }

        //                    //    string query = "update tblinspection set UpdatedBy='" + UserEmail + "', UpdatedOn='" + DateTime.Now + "', InspFileName='" + FileName + "' where InspectionID ='" + InspectionID + "'";
        //                    //    int result = databaseCon.RunCmd(query);
        //                    //    //  }
        //                    //}
        //                }
        //            }
        //        }


        //        Message = "success_" + "" + GetOfflineStatus + "";
        //        // return "success";
        //        return Message;

        //    }
        //    catch (Exception ex)
        //    {
        //        var Message = "Failed_" + ex.Message;
        //        return Message;
        //    }

        //}
        protected void SignatureUpload(string sketchData, int InspectionID)
        {
            string path = "~/uploads/";
            path = Server.MapPath(path);
            string FileName = "SketchInsp" + "_" + DateTime.Now.Ticks + "_" + InspectionID + ".png";
            string SketchnewPath = path + FileName;


            byte[] bytes = Convert.FromBase64String(sketchData.Split(',')[1]);

            var uri = new Uri(databaseCon.siteimagelink);
            var container = new CloudBlobContainer(uri);
            var blob = container.GetBlockBlobReference(FileName);
            var exists = blob.Exists();
            if (exists == false)
            {
                using (var imageFile = new FileStream(SketchnewPath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }

                if (System.IO.File.Exists(SketchnewPath))
                {
                    string fpath = path + FileName;
                    databaseCon.savefileonblob(FileName, "Site-images", fpath, "image/png");
                    System.IO.File.Delete(fpath);
                }
            }
            else
            {
                databaseCon.deletefileonblob(FileName, "siteimage");

                using (var imageFile = new FileStream(SketchnewPath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }

                if (System.IO.File.Exists(SketchnewPath))
                {
                    string fpath = path + FileName;
                    databaseCon.savefileonblob(FileName, "Site-images", fpath, "image/png");
                    System.IO.File.Delete(fpath);
                }
            }


            string querystr = "update tblinspection set  InspSign='" + FileName + "' where InspectionID ='" + InspectionID + "'";
            int count = databaseCon.RunCmd(querystr);
        }
        protected void imageUpload(string imageData, int InspectionID)
        {
            //string path = "~/uploads/";
            string path = "~/uploads/Temp/";
            path = Server.MapPath(path);
            string FileName = "SnapInsp" + "_" + DateTime.Now.Ticks + "_" + InspectionID + ".jpg";
            string newPath = path + FileName;
            var PhotoAngle = "0";

            byte[] bytes = Convert.FromBase64String(imageData.Split(',')[1]);

            var uri = new Uri(databaseCon.siteimagelink);
            var container = new CloudBlobContainer(uri);
            var blob = container.GetBlockBlobReference(FileName);
            var exists = blob.Exists();
            if (exists == false)
            {
                using (var imageFile = new FileStream(newPath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }

                Image img = Image.FromFile(newPath);
                
                if (img.PropertyIdList.Contains(0x0112))
                {
                    PropertyItem propOrientation = img.GetPropertyItem(0x0112);
                    short orientation = BitConverter.ToInt16(propOrientation.Value, 0);
                    if (orientation == 6)
                    {
                        // img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        // img.RotateFlip(RotateFlipType.RotateNoneFlipNone);
                        PhotoAngle = "90";
                    }
                }


                if (System.IO.File.Exists(newPath))
                {
                    string fpath = path + FileName;
                    databaseCon.savefileonblob(FileName, "Site-images", fpath, "image/jpeg");
                   // System.IO.File.Delete(fpath);
                }
            }
            else
            {
                databaseCon.deletefileonblob(FileName, "siteimage");

                using (var imageFile = new FileStream(newPath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }


                if (System.IO.File.Exists(newPath))
                {
                    string fpath = path + FileName;
                    databaseCon.savefileonblob(FileName, "Site-images", fpath, "image/jpeg");
                    System.IO.File.Delete(fpath);
                }
            }

            string querystr = "update tblinspection set Inspimage='" + FileName + "',SafariPhotoAngle='" + PhotoAngle + "' where InspectionID ='" + InspectionID + "'";
            int count = databaseCon.RunCmd(querystr);
        }

    }
}