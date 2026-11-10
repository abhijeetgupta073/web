using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scaffold.App_Code;
using Scaffold_DataAccess;
using Scaffold.Models;

namespace Scaffold.Controllers
{
    public class SiteFloorLevelPlanController : Controller
    {
        ScaffoldEntities dbcontext = new ScaffoldEntities();
        // GET: SiteFloorLevelPlan
        public ActionResult SiteFloorLevelPlan()
        {
            return View();
        }

        public ActionResult SiteDropDownBind(string id)
        {
            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select SiteLocID,SiteLocation from tblsitelocation where ClientId='" +id + "' and FloorLevel=1  Order by SiteLocation").ToList();
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }
        // public ActionResult GetClientDefaultFloorLabelImage(string id)
        public ActionResult GetClientDefaultFloorLabelImage(string id,string SiteID)
        {
            var SiteFloorImages = "";
           // string query1 = "select ImageName from tblSiteImages  where ClientID='" + id + "' and SiteId=0";
           string query1 = "select ImageName from tblSiteImages  where ClientID='" + id + "' and ( SiteId='" + SiteID + "' and SiteId != '0') and isnull(LocId,0)='0' and isnull(BuildingId,0)='0' and isnull(FloorLevelId,0) = '0' ";
            DataTable dt = databaseCon.GetData(query1);
            if (dt.Rows.Count > 0)
            {

                SiteFloorImages = dt.Rows[0]["ImageName"].ToString();

            }
            // return scaffold.ClientDefaultFloorLabelImage = SiteFloorImages;
            return Json(SiteFloorImages, JsonRequestBehavior.AllowGet);
        }

        // public ActionResult ddlFloorLevelDropdown(string id, string SiteId)
        public ActionResult ddlFloorLevelDropdown(string id, string SiteId, string LocationId, string BuildingId)
        {
            // var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select distinct FloorLevelId,FloorLevel from tblRegister where  ClientID='" + id + "' and SiteLocId='" + SiteId + "'  Order by FloorLevel").ToList();
           
             var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select distinct tblsiteImages.FloorLevelId,FloorLevel,SortOrder from tblsiteImages join tblFloorLevel on TblsiteImages.FloorLevelId = tblFloorLevel.FloorLevelId where tblsiteImages.ClientID = '" + id + "'  and tblsiteImages.SiteId = '" + SiteId + "' and tblsiteImages.LocId = '" + LocationId + "' and tblsiteImages.BuildingId = '" + BuildingId + "'  Order by SortOrder").ToList();
            return Json(GetData, JsonRequestBehavior.AllowGet);
           
        }
        public ActionResult ddllocationDropdown(string id, string SiteId)
        {
            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select area,areaid from tblArea where ClientID='" + id + "'and SiteLocId = '" + SiteId + "' order by area").ToList();
            return Json(GetData, JsonRequestBehavior.AllowGet);

        }
        public ActionResult ddlbuildingDropdown(string id, string SiteId, string LocationId)
        {
            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select buildingid,buildingno  from tblbuilding where BuildingStatus='Active' and ClientID='" + id + "' and Areaid='" + LocationId + "' order by buildingno").ToList();
            return Json(GetData, JsonRequestBehavior.AllowGet);

        }
        
        List<ScaffoldAllDashBoardViewModel> GetSiteFloorLevelPlan = new List<ScaffoldAllDashBoardViewModel> { };
        // [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        // [WebMethod(EnableSession = true)]
        // public List<Scaffold> GetSiteFloorLevelPlanData(string id, string siteID, string FloorLevel)
       // public ActionResult GetSiteFloorLevelPlanData(string id, string siteID, string FloorLevel)
        public ActionResult GetSiteFloorLevelPlanData(string id, string siteID, string FloorLevelId, string Locationid, string Buildingid, string checkDrawing)
        {
            string query = "";
            string query1 = "";
            string DrawingName = "";

            if (FloorLevelId != "")
            {
                //query1 = "select DrawingName from tblSiteImages where ClientId='" + id + "' and SiteId='" + siteID + "' and FloorLevelId='" + FloorLevelId + "'";
                query1 = "select DrawingName from tblSiteImages where ClientId='" + id + "'";
                if (siteID != "")
                {
                    query1 = query1 + " and SiteId='" + siteID + "'";
                }
                if (FloorLevelId != "")
                {
                    query1 = query1 + " and FloorLevelId='" + FloorLevelId + "'";
                }

                if (Locationid != "")
                {
                    query1 = query1 + " and LocId='" + Locationid + "'";
                }
                if (Buildingid != "")
                {
                    query1 = query1 + " and BuildingId='" + Buildingid + "'";
                }
                else
                {
                    query1 = query1 + "";
                }

                DataTable dt = databaseCon.GetData(query1);
                if (dt.Rows.Count > 0)
                {
                    DrawingName = dt.Rows[0]["DrawingName"].ToString();
                }
            }

            if (checkDrawing == "True")
            {
                //Commented On (15-03-2023) To only display Jobs that are not Dismantled.
                //query = "select Right ('000000' + convert (varchar, [scaffref]),6) as ScaffRef,SiteFloorImage,XPosition,YPosition,RegisterId,DesignType,MaterialType,ManHrsDismantle,DateTagged, case when dateofdismantle is not null then Totalhiredays else hiredaystodate end PlannedStandingDuration,Convert(Decimal(18, 2), (Tonnage / 1000)) as Tonnage,BuildingNo,FloorLevel,AssetNo from VW_TblRegister where ClientID='" + id + "' and XPosition!='' and YPosition!='' and FloorLevelId in (Select FloorLevelId from TblSiteImages where ClientId = '" + id + "'  and DrawingName = '" + DrawingName + "' )";

                query = "select Right ('000000' + convert (varchar, [scaffref]),6) as ScaffRef,SiteFloorImage,XPosition,YPosition,RegisterId,DesignType,MaterialType,ManHrsDismantle,DateTagged, case when (dateofdismantle is not null and dateofdismantle !='1900-01-01') then Totalhiredays else hiredaystodate end PlannedStandingDuration,Convert(Decimal(18, 2), (Tonnage / 1000)) as Tonnage,BuildingNo,FloorLevel,AssetNo from VW_TblRegister where ClientID='" + id + "' and (dateofdismantle is  null or dateofdismantle ='1900-01-01') and XPosition!='' and YPosition!='' and FloorLevelId in (Select FloorLevelId from TblSiteImages where ClientId = '" + id + "'  and DrawingName = '" + DrawingName + "' )";

            }
            else
            {
                // Query change On 07 May 2020 
                // query = "select Right ('000000' + convert (varchar, [scaffref]),6) as ScaffRef,SiteFloorImage,XPosition,YPosition,RegisterId,DesignType,MaterialType,ManHrsDismantle,DateTagged, case when dateofdismantle is not null then Totalhiredays else hiredaystodate end PlannedStandingDuration,Convert(Decimal(18, 2), (Tonnage / 1000)) as Tonnage,BuildingNo,FloorLevel,AssetNo from VW_TblRegister where ClientID='" + id + "' and SiteLocID='" + siteID + "' and FloorLevel='" + FloorLevel + "' and XPosition!='' and YPosition!=''";

                //Commented on (15-03-2023) To only display Jobs that are not Dismantled. 
                //query = "select Right ('000000' + convert (varchar, [scaffref]),6) as ScaffRef,SiteFloorImage,XPosition,YPosition,RegisterId,DesignType,MaterialType,ManHrsDismantle,DateTagged, case when dateofdismantle is not null then Totalhiredays else hiredaystodate end PlannedStandingDuration,Convert(Decimal(18, 2), (Tonnage / 1000)) as Tonnage,BuildingNo,FloorLevel,AssetNo from VW_TblRegister where ClientID='" + id + "' and SiteLocID='" + siteID + "' and FloorLevelId='" + FloorLevelId + "' and XPosition!='' and YPosition!=''";

                query = "select Right ('000000' + convert (varchar, [scaffref]),6) as ScaffRef,SiteFloorImage,XPosition,YPosition,RegisterId,DesignType,MaterialType,ManHrsDismantle,DateTagged, case when (dateofdismantle is not null and dateofdismantle !='1900-01-01') then Totalhiredays else hiredaystodate end PlannedStandingDuration,Convert(Decimal(18, 2), (Tonnage / 1000)) as Tonnage,BuildingNo,FloorLevel,AssetNo from VW_TblRegister where ClientID='" + id + "' and (dateofdismantle is  null or dateofdismantle ='1900-01-01') and SiteLocID='" + siteID + "' and FloorLevelId='" + FloorLevelId + "' and XPosition!='' and YPosition!=''";

                //Add code on 30-May-2020
                if (Locationid != "")
                {
                    query = query + " and LocationId='" + Locationid + "'";
                }
                if (Buildingid != "")
                {
                    query = query + " and BuildingId='" + Buildingid + "'";
                }
                else
                {
                    query = query + "";
                }

            }

            //string query = "select Right ('000000' + convert (varchar, [scaffref]),6) as ScaffRef,SiteFloorImage,XPosition,YPosition,RegisterId,DesignType,MaterialType,ManHrsDismantle,DateTagged, case when dateofdismantle is not null then Totalhiredays else hiredaystodate end PlannedStandingDuration from VW_TblRegister where ClientID='" + id + "' and SiteLocID='" + siteID + "' and FloorLevel='"+ FloorLevel + "' and XPosition!='' and YPosition!=''";

            // Query change On 07 May 2020 
            //string query = "select Right ('000000' + convert (varchar, [scaffref]),6) as ScaffRef,SiteFloorImage,XPosition,YPosition,RegisterId,DesignType,MaterialType,ManHrsDismantle,DateTagged, case when dateofdismantle is not null then Totalhiredays else hiredaystodate end PlannedStandingDuration,Convert(Decimal(18, 2), (Tonnage / 1000)) as Tonnage,BuildingNo,FloorLevel from VW_TblRegister where ClientID='" + id + "' and SiteLocID='" + siteID + "' and FloorLevel='" + FloorLevel + "' and XPosition!='' and YPosition!=''";

            DataSet ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
            {

                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ScaffoldAllDashBoardViewModel scaffold = new ScaffoldAllDashBoardViewModel();
                    if (!(item["SiteFloorImage"] is DBNull) && item["SiteFloorImage"].ToString() != "")
                    {
                        scaffold.SiteFloorLevelImage = item["SiteFloorImage"].ToString();
                    }

                    if (!(item["XPosition"] is DBNull) && item["XPosition"].ToString() != "")
                    {
                        scaffold.XPosition = item["XPosition"].ToString();
                    }
                    if (!(item["YPosition"] is DBNull) && item["YPosition"].ToString() != "")
                    {
                        scaffold.YPosition = item["YPosition"].ToString();
                    }
                    if (!(item["RegisterId"] is DBNull) && item["RegisterId"].ToString() != "")
                    {
                        scaffold.RegisterID = item["RegisterId"].ToString();
                    }
                    if (!(item["ScaffRef"] is DBNull) && item["ScaffRef"].ToString() != "")
                    {
                        scaffold.ScaffRef = item["ScaffRef"].ToString();
                    }
                    if (!(item["DesignType"] is DBNull) && item["DesignType"].ToString() != "")
                    {
                        scaffold.DesignType = item["DesignType"].ToString();
                    }
                    if (!(item["MaterialType"] is DBNull) && item["MaterialType"].ToString() != "")
                    {
                        scaffold.MaterialType = item["MaterialType"].ToString();
                    }
                    if (!(item["ManHrsDismantle"] is DBNull) && item["ManHrsDismantle"].ToString() != "")
                    {
                        scaffold.ManHrsDismantle = Convert.ToDecimal(item["ManHrsDismantle"]);
                    }
                    if (!(item["PlannedStandingDuration"] is DBNull) && item["PlannedStandingDuration"].ToString() != "")
                    {
                        scaffold.PlannedStandingDuration = item["PlannedStandingDuration"].ToString();
                    }
                    if (!(item["DateTagged"] is DBNull) && item["DateTagged"].ToString() != "")
                    {
                        scaffold.DateTagged = item["DateTagged"].ToString();
                    }
                    if (!(item["Tonnage"] is DBNull) && item["Tonnage"].ToString() != "")
                    {
                        scaffold.Tonnage = item["Tonnage"].ToString();
                    }
                    if (!(item["BuildingNo"] is DBNull) && item["BuildingNo"].ToString() != "")
                    {
                        scaffold.BuildingNo = item["BuildingNo"].ToString();
                    }
                    if (!(item["FloorLevel"] is DBNull) && item["FloorLevel"].ToString() != "")
                    {
                        scaffold.FloorLevel = item["FloorLevel"].ToString();
                    }
                    if (!(item["AssetNo"] is DBNull) && item["AssetNo"].ToString() != "")
                    {
                        scaffold.AssetNo = item["AssetNo"].ToString();
                    }

                    scaffold.DrawingName = DrawingName; // For Drawing Name Textbox

                    GetSiteFloorLevelPlan.Add(scaffold);
                }
            }

           // return GetJobFloorData;
            return Json(GetSiteFloorLevelPlan, JsonRequestBehavior.AllowGet);
        }

      //  public ActionResult GetRedirectToJob(string id, string Xpos, string Ypos, string FloorLevel)
         public ActionResult GetRedirectToJob(string id, string Xpos, string Ypos, string FloorLevelId, string chckDrawing, string DrawingName)
        {
            var RedirectRegisterID = "";

            string qryRedirect = "";

            //string qryRedirect = "select RegisterID,ScaffRef,XPosition,YPosition,abs(convert(decimal(18, 2), XPosition) - '" + Xpos + "') as XPosition1 from tblregister where Clientid = '" + id + "' and FloorLevel = '" + FloorLevel + "' and XPosition is not null and RegisterID in ( select RegisterID from tblregister where Clientid = '" + id + "' and FloorLevel = '" + FloorLevel + "' and YPosition is not null and abs(convert(decimal(18, 2), YPosition) - '" + Ypos + "') < 5 ) and abs(convert(decimal(18, 2),XPosition) - '" + Xpos + "') < 5  order by  abs(convert(decimal(18, 2), XPosition) - '" + Xpos + "')";
            //string qryRedirect = "select RegisterID,ScaffRef,XPosition,YPosition,('" + Xpos + "' - convert(decimal(18, 2), XPosition)) as XPosition1 from tblregister where Clientid = '" + id + "' and FloorLevel = '" + FloorLevel + "' and XPosition is not null and RegisterID in ( select RegisterID from tblregister where Clientid = '" + id + "' and FloorLevel = '" + FloorLevel + "' and YPosition is not null and ('" + Ypos + "' - convert(decimal(18, 2), YPosition)) < 50 ) and ('" + Xpos + "' - convert(decimal(18, 2), XPosition)) < 35  order by  ('" + Xpos + "' - convert(decimal(18, 2), XPosition))";


            if (chckDrawing == "True")
            {
                qryRedirect = "select RegisterID, ScaffRef, XPosition, YPosition, abs(convert(decimal(18, 2), XPosition) - '" + Xpos + "') as XPosition1 from tblregister where Clientid = '" + id + "'  And FloorLevelId in (Select FloorLevelID from tblSiteImages where DrawingName = '" + DrawingName + "'  and FloorLevelId is Not null) and XPosition is not null and RegisterID in (select RegisterID from tblregister where Clientid = '" + id + "' And FloorLevelId in (Select FloorLevelID from tblSiteImages where DrawingName = '" + DrawingName + "' and FloorLevelId is Not null) and YPosition is not null and abs(convert(decimal(18, 2), YPosition) - '" + Ypos + "') < 5 ) and abs(convert(decimal(18, 2), XPosition) - '" + Xpos + "') < 5 order by  abs(convert(decimal(18, 2), XPosition) - '" + Xpos + "')";
            }
            else
            {
                qryRedirect = "select RegisterID,ScaffRef,XPosition,YPosition,abs(convert(decimal(18, 2), XPosition) - '" + Xpos + "') as XPosition1 from tblregister where Clientid = '" + id + "' and FloorLevelId = '" + FloorLevelId + "' and XPosition is not null and RegisterID in ( select RegisterID from tblregister where Clientid = '" + id + "' and FloorLevelId = '" + FloorLevelId + "' and YPosition is not null and abs(convert(decimal(18, 2), YPosition) - '" + Ypos + "') < 5 ) and abs(convert(decimal(18, 2),XPosition) - '" + Xpos + "') < 5  order by  abs(convert(decimal(18, 2), XPosition) - '" + Xpos + "')";
            }



            DataTable dt = databaseCon.GetData(qryRedirect);
            if (dt.Rows.Count > 0)
            {
                RedirectRegisterID = dt.Rows[0]["RegisterID"].ToString();
            }
            // return scaffold.ClientRegisterJob = RedirectRegisterID;
            return Json(RedirectRegisterID, JsonRequestBehavior.AllowGet);
        }
    }
}