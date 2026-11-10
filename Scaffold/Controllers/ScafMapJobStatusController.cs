using Scaffold.App_Code;
using Scaffold.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scaffold_DataAccess;

namespace Scaffold.Controllers
{
    public class ScafMapJobStatusController : Controller
    {
        ScaffoldEntities dbcontext = new ScaffoldEntities();

        // GET: ScafMapJobStatus
        public ActionResult ScafMapJobStatus()
        {
            return View();
        }
        public ActionResult SiteDropdownBind(string id, string UID)
        {
            var SiteOwnerName = "";
            string query1 = "select SiteOwnerName from tblUser where ClientId='" + id + "' and UserId='" + databaseCon.Decrypt(UID) + "'";
            DataTable dt1 = databaseCon.GetData(query1);
            if (dt1.Rows.Count > 0)
            {
                SiteOwnerName = dt1.Rows[0]["SiteOwnerName"].ToString();
            }
            if (SiteOwnerName != "")
            {
                // Escape single quote by replacing ' with ''
                SiteOwnerName = SiteOwnerName.Replace("'", "''");
                var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select Distinct SiteLocation, SiteLocID from tblSiteLocation where ClientId = '" + id + "' and OwnerName='" + SiteOwnerName + "' and GPS = 1 order by SiteLocation").ToList();
                return Json(GetData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select Distinct SiteLocation, SiteLocID from tblSiteLocation where ClientId = '" + id + "' and GPS = 1 order by SiteLocation").ToList();
                return Json(GetData, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult ddlLocationScafMap(string ClientID, string SiteId)
        {
            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>("select area,areaid from tblArea where ClientID='" + ClientID + "'and SiteLocId = '" + SiteId + "' order by area").ToList();
            return Json(GetData, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ddlBuildingScafMap(string ClientID, string SiteId, string LocationId)
        {
            string query = "";
            if (LocationId == "All")
            {
                query = "select buildingid, buildingno  from tblbuilding where BuildingStatus = 'Active' and ClientID = '" + ClientID + "'  and Areaid in (select AreaID from tblarea where AreaStatus = 'Active' and SiteLocId = '" + SiteId + "') order by buildingno";
            }
            else
            {
                query = "select buildingid, buildingno  from tblbuilding where BuildingStatus = 'Active' and ClientID = '" + ClientID + "'  and Areaid= '" + LocationId + "' order by buildingno";
            }

            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>(query);
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }

        //Added on 26-Apr-2023 Start here 
        public ActionResult ddlFloorbind(string ClientID, string SiteId, string LocationId, string ddlBuilding)
        {
            string query = "";

            query = "select distinct concat(floorlevel,' (',buildingno,case when buildingno is not null then '-' else '' end,location,')') as floorlevel,floorlevelid from vw_tblregister where ClientId ='" + ClientID + "' and ( sitelocid='" + SiteId + "' or '" + SiteId + "' = 'All') and  (LocationId='" + LocationId + "' or '" + LocationId + "' ='ALL') and (BuildingID = '" + ddlBuilding + "' or '" + ddlBuilding + "' = 'ALL') order by floorlevel ";

            var GetData = dbcontext.Database.SqlQuery<DropDownViewModel>(query);
            return Json(GetData, JsonRequestBehavior.AllowGet);
        }

        //Added on 30-May-2022 End here 

        //Site Map Start
        List<ScaffoldAllDashBoardViewModel> GoogleMap = new List<ScaffoldAllDashBoardViewModel> { };
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //[WebMethod(EnableSession = true)]
        public ActionResult GetMapLocation(string ClientID, int id, string Location, string Building, string FloorLevelID)

        {
            //string query = "select VW_TblRegister.DateTagged as DateTagged,VW_TblRegister.DateOfDismantle as DateOfDismantle,VW_TblRegister.PlannedStandingDuration as PlannedStandingDuration,VW_TblRegister.DesignType as DesignType,VW_TblRegister.MaterialType as MaterialType, tblSiteLocation.SiteLocation,VW_tblRegister.Lat as Lat ,VW_tblRegister.long as Long,VW_tblRegister.RegisterId as RID,tblSiteLocation.SiteLat as initLat,tblSiteLocation.SiteLong as initLong,Right ('000000' + convert (varchar, [scaffref]),6) as Description,VW_tblRegister.Status as Status,VW_tblRegister.ManHrsDismantle as ManHrsDismantle,Convert(Decimal(18, 2), (Tonnage / 1000)) as Tonnage,VW_TblRegister.BuildingNo as BuildingNo ,VW_TblRegister.FloorLevel as FloorLevel from tblSiteLocation left join VW_tblRegister on tblSiteLocation.SiteLocID=VW_tblRegister.SiteLocId and (datetagged is not null and datetagged!='01/01/1900') and (dateofdismantle='01/01/1900' or dateofdismantle is null) where tblSiteLocation.SiteLocID='" + id + "'  and ( VW_TblRegister.LocationId = '" + Location + "' or 'All' = '" + Location + "' ) and ( VW_TblRegister.BuildingId = '" + Building + "' or 'All' = '" + Building + "' )";

            //Added on 26-04-2023  for adding Floor Level Filter
            //string query = "select VW_TblRegister.DateTagged as DateTagged,VW_TblRegister.DateOfDismantle as DateOfDismantle,VW_TblRegister.PlannedStandingDuration as PlannedStandingDuration,VW_TblRegister.DesignType as DesignType,VW_TblRegister.MaterialType as MaterialType, tblSiteLocation.SiteLocation,VW_tblRegister.Lat as Lat ,VW_tblRegister.long as Long,VW_tblRegister.RegisterId as RID,tblSiteLocation.SiteLat as initLat,tblSiteLocation.SiteLong as initLong,Right ('000000' + convert (varchar, [scaffref]),6) as Description,VW_tblRegister.Status as Status,VW_tblRegister.ManHrsDismantle as ManHrsDismantle,Convert(Decimal(18, 2), (Tonnage / 1000)) as Tonnage,VW_TblRegister.BuildingNo as BuildingNo ,VW_TblRegister.FloorLevel as FloorLevel from tblSiteLocation left join VW_tblRegister on tblSiteLocation.SiteLocID=VW_tblRegister.SiteLocId and (datetagged is not null and datetagged!='01/01/1900') and (dateofdismantle='01/01/1900' or dateofdismantle is null) where tblSiteLocation.SiteLocID='" + id + "'  and ( VW_TblRegister.LocationId = '" + Location + "' or 'All' = '" + Location + "' ) and ( VW_TblRegister.BuildingId = '" + Building + "' or 'All' = '" + Building + "' ) and ( VW_TblRegister.FloorLevelId = '" + FloorLevelID + "' or 'All' = '" + FloorLevelID + "' )";

            string query = "select VW_TblRegister.DateTagged as DateTagged,VW_TblRegister.DateOfDismantle as DateOfDismantle,VW_TblRegister.HireDaysToDate as HireDaysToDate,VW_TblRegister.Image as Image,VW_TblRegister.DaysSinceLastInsp as DaysSinceLastInsp,VW_TblRegister.PlannedStandingDuration as PlannedStandingDuration,VW_TblRegister.DesignType as DesignType,VW_TblRegister.MaterialType as MaterialType, tblSiteLocation.SiteLocation,VW_tblRegister.Lat as Lat ,VW_tblRegister.long as Long,VW_tblRegister.RegisterId as RID,tblSiteLocation.SiteLat as initLat,tblSiteLocation.SiteLong as initLong,Right ('000000' + convert (varchar, [scaffref]),6) as Description,VW_tblRegister.Status as Status,VW_tblRegister.ManHrsDismantle as ManHrsDismantle,Convert(Decimal(18, 2), (Tonnage / 1000)) as Tonnage,VW_TblRegister.BuildingNo as BuildingNo ,VW_TblRegister.FloorLevel as FloorLevel from tblSiteLocation left join VW_tblRegister on tblSiteLocation.SiteLocID=VW_tblRegister.SiteLocId and (datetagged is not null and datetagged!='01/01/1900') and (dateofdismantle='01/01/1900' or dateofdismantle is null) where tblSiteLocation.SiteLocID='" + id + "' and ( VW_TblRegister.LocationId = '" + Location + "' or 'All' = '" + Location + "' ) and ( VW_TblRegister.BuildingId = '" + Building + "' or 'All' = '" + Building + "') and ( VW_TblRegister.FloorLevelId = '" + FloorLevelID + "' or 'All' = '" + FloorLevelID + "') UNION select VW_TblRegister.DateTagged as  DateTagged,VW_TblRegister.DateOfDismantle as DateOfDismantle ,VW_TblRegister.HireDaysToDate as HireDaysToDate,VW_TblRegister.Image as Image,VW_TblRegister.DaysSinceLastInsp as DaysSinceLastInsp,VW_TblRegister.PlannedStandingDuration as PlannedStandingDuration,VW_TblRegister.DesignType as DesignType,VW_TblRegister.MaterialType as MaterialType , tblSiteLocation.SiteLocation,VW_tblRegister.Lat as Lat  ,VW_tblRegister.long as Long,VW_tblRegister.RegisterId as RID ,tblSiteLocation.SiteLat as initLat,tblSiteLocation.SiteLong as initLong ,Right ('000000' + convert (varchar, [scaffref]),6) as Description,VW_tblRegister.Status as Status,VW_tblRegister.ManHrsDismantle as ManHrsDismantle,Convert(Decimal(18, 2), (Tonnage / 1000)) as Tonnage,VW_TblRegister.BuildingNo as BuildingNo ,VW_TblRegister.FloorLevel as FloorLevel from tblSiteLocation left join VW_tblRegister on tblSiteLocation.SiteLocID = VW_tblRegister.SiteLocId  and  VW_TblRegister.Registerid = 0 where tblSiteLocation.SiteLocID = '" + id + "'";

            // string query = "select top 700 VW_TblRegister.DateTagged as DateTagged,VW_TblRegister.DateOfDismantle as DateOfDismantle,VW_TblRegister.PlannedStandingDuration as PlannedStandingDuration,VW_TblRegister.DesignType as DesignType,VW_TblRegister.MaterialType as MaterialType, tblSiteLocation.SiteLocation,VW_tblRegister.Lat as Lat ,VW_tblRegister.long as Long,VW_tblRegister.RegisterId as RID,tblSiteLocation.SiteLat as initLat,tblSiteLocation.SiteLong as initLong,Right ('000000' + convert (varchar, [scaffref]),6) as Description,VW_tblRegister.Status as Status,VW_tblRegister.ManHrsDismantle as ManHrsDismantle,Convert(Decimal(18, 2), (Tonnage / 1000)) as Tonnage,VW_TblRegister.BuildingNo as BuildingNo ,VW_TblRegister.FloorLevel as FloorLevel from tblSiteLocation left join VW_tblRegister on tblSiteLocation.SiteLocID=VW_tblRegister.SiteLocId and (datetagged is not null and datetagged!='01/01/1900') and (dateofdismantle='01/01/1900' or dateofdismantle is null) ";

            //  string query = "select VW_TblRegister.DateTagged as DateTagged,VW_TblRegister.DateOfDismantle as DateOfDismantle,VW_TblRegister.PlannedStandingDuration as PlannedStandingDuration,VW_TblRegister.DesignType as DesignType,VW_TblRegister.MaterialType as MaterialType, tblSiteLocation.SiteLocation,VW_tblRegister.Lat as Lat ,VW_tblRegister.long as Long,VW_tblRegister.RegisterId as RID,tblSiteLocation.SiteLat as initLat,tblSiteLocation.SiteLong as initLong,Right ('000000' + convert (varchar, [scaffref]),6) as Description,VW_tblRegister.Status as Status,VW_tblRegister.ManHrsDismantle as ManHrsDismantle,Convert(Decimal(18, 2), (Tonnage / 1000)) as Tonnage,VW_TblRegister.BuildingNo as BuildingNo ,VW_TblRegister.FloorLevel as FloorLevel from tblSiteLocation left join VW_tblRegister on 
            //  string query = "select VW_TblRegister.DateTagged as DateTagged,VW_TblRegister.DateOfDismantle as DateOfDismantle,VW_TblRegister.PlannedStandingDuration as PlannedStandingDuration,VW_TblRegister.DesignType as DesignType,VW_TblRegister.MaterialType as MaterialType, tblSiteLocation.SiteLocation,VW_tblRegister.Lat as Lat ,VW_tblRegister.long as Long,VW_tblRegister.RegisterId as RID,tblSiteLocation.SiteLat as initLat,tblSiteLocation.SiteLong as initLong,Right ('000000' + convert (varchar, [scaffref]),6) as Description,VW_tblRegister.Status as Status,VW_tblRegister.ManHrsDismantle as ManHrsDismantle,Convert(Decimal(18, 2), (Tonnage / 1000)) as Tonnage,VW_TblRegister.BuildingNo as BuildingNo ,VW_TblRegister.FloorLevel as FloorLevel from tblSiteLocation left join VW_tblRegister on tblSiteLocation.SiteLocID=VW_tblRegister.SiteLocId and (datetagged is not null and datetagged!='01/01/1900') and (dateofdismantle='01/01/1900' or dateofdismantle is null) where tblSiteLocation.SiteLocID='" + id + "'  and  ('" + SubOwner.Replace("'", "''") + "' = 'All'  or Case when ScaffoldOwner = 'P&I' then 'P&I' else 'Maintenance' End = '" + SubOwner.Replace("'", "''") + "')";


            DataSet ds = null;
            ds = databaseCon.GetDataSet(query);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ScaffoldAllDashBoardViewModel pro1 = new ScaffoldAllDashBoardViewModel();
                    //if (!(item["SiteLocation"] is DBNull) && item["SiteLocation"].ToString() != "")
                    //{
                    //    pro1.MapSiteLocation = item["SiteLocation"].ToString();
                    //}
                    if (!(item["Description"] is DBNull) && item["Description"].ToString() != "")
                    {
                        pro1.ScaffRef = item["Description"].ToString();
                    }
                    if (!(item["ManHrsDismantle"] is DBNull) && item["ManHrsDismantle"].ToString() != "")
                    {
                        pro1.ManHrsDismantle = Convert.ToDecimal(item["ManHrsDismantle"]);
                    }
                    if (!(item["Lat"] is DBNull) && item["Lat"].ToString() != "")
                    {
                        pro1.Lat = item["Lat"].ToString();
                    }
                    if (!(item["Long"] is DBNull) && item["Long"].ToString() != "")
                    {
                        pro1.Long = item["Long"].ToString();
                    }
                    if (!(item["RID"] is DBNull) && item["RID"].ToString() != "")
                    {
                        pro1.RID = Convert.ToInt32(item["RID"]);
                    }
                    if (!(item["initLat"] is DBNull) && item["initLat"].ToString() != "")
                    {
                        pro1.Lattitude = item["initLat"].ToString();
                    }
                    if (!(item["initLong"] is DBNull) && item["initLong"].ToString() != "")
                    {
                        pro1.Longitude = item["initLong"].ToString();
                    }
                    if (!(item["Status"] is DBNull) && item["Status"].ToString() != "")
                    {
                        pro1.Status = item["Status"].ToString();
                    }
                    if (!(item["DateTagged"] is DBNull) && item["DateTagged"].ToString() != "")
                    {
                        pro1.StartDate = item["DateTagged"].ToString();
                    }
                    if (!(item["DateOfDismantle"] is DBNull) && item["DateOfDismantle"].ToString() != "")
                    {
                        pro1.EndDate = item["DateOfDismantle"].ToString();
                    }
                    if (!(item["PlannedStandingDuration"] is DBNull) && item["PlannedStandingDuration"].ToString() != "")
                    {
                        pro1.PlanDuration = item["PlannedStandingDuration"].ToString();
                    }
                    if (!(item["DesignType"] is DBNull) && item["DesignType"].ToString() != "")
                    {
                        pro1.DesignType = item["DesignType"].ToString();
                    }
                    if (!(item["MaterialType"] is DBNull) && item["MaterialType"].ToString() != "")
                    {
                        pro1.MaterialTypeMap = item["MaterialType"].ToString();
                    }
                    if (!(item["Tonnage"] is DBNull) && item["Tonnage"].ToString() != "")
                    {
                        pro1.Tonnage = item["Tonnage"].ToString();
                    }
                    if (!(item["BuildingNo"] is DBNull) && item["BuildingNo"].ToString() != "")
                    {
                        pro1.BuildingMap = item["BuildingNo"].ToString();
                    }
                    if (!(item["FloorLevel"] is DBNull) && item["FloorLevel"].ToString() != "")
                    {
                        pro1.FloorLevelMap = item["FloorLevel"].ToString();
                    }
                    if (!(item["Status"] is DBNull) && item["Status"].ToString() != "")
                    {
                        pro1.Status = item["Status"].ToString();
                    }
                    if (!(item["HireDaysToDate"] is DBNull) && item["HireDaysToDate"].ToString() != "")
                    {
                        pro1.HireDaysToDate = item["HireDaysToDate"].ToString();
                    }
                    if (!(item["DaysSinceLastInsp"] is DBNull) && item["DaysSinceLastInsp"].ToString() != "")
                    {
                        pro1.DaysSinceLastInsp = item["DaysSinceLastInsp"].ToString();
                    }
                    if (!(item["Image"] is DBNull) && item["Image"].ToString() != "")
                    {
                        pro1.Image = item["Image"].ToString();
                    }
                    GoogleMap.Add(pro1);
                }
            }
            //return GoogleMap;
            // return Json(GoogleMap, JsonRequestBehavior.AllowGet);
            var jsonResult = Json(GoogleMap, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }
        //Site Map End
    }
}