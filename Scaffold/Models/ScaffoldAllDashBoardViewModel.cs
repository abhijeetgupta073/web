using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scaffold.Models
{
    public class ScaffoldAllDashBoardViewModel
    {
        // Dashboard Start
        public string MaterialType;
        public string Description;
        public decimal TonnageStanding;
        public decimal HireDays;
        public string SiteLocation;
        public string ScaffRef;
        public string Registerid;
        public decimal PctAllSite1;
        public string AvgHireDays;
        public decimal TonnagebySitePct;
        public decimal TotalTonnageAllsite;
        public decimal TonnageAllsitePct;
        public decimal TonnageByjobPct;
        public decimal TotalTonnageByjob;
        public decimal MaterialType1Quantity;  //19-Mar-2019 -Baans
        public decimal MaterialType2Quantity;  //19-Mar-2019 -Baans
        public decimal MaterialType1Tonnage;   //19-Mar-2019 -Baans
        public decimal MaterialType2Tonnage;   //19-Mar-2019 -Baans
        public int StartDayOfWeek;  //19-Mar-2019 -Baans
                                    // Dashboard End

        // Man Hrs Dashborad Start
        public decimal ModificationHrs;
        public decimal AllModificationHrs;
        public decimal ErectionHrs;
        public decimal AllErectionHrs;
        public decimal DismantleHrs;
        public decimal AllDismantleHrs;
        public decimal AdditionalHrs;
        public decimal AllAdditionalHrs;
        public string Scaffref;
        public string TypeofVariation;
        public string EntryDate;
        public decimal SpentHours;
        public decimal NoOfHrs;
        public decimal total;
        // Man Hrs Dashborad End

        // Dashboard2 Start
        public string AreaID;
        public string SiteLoca;
        public string SiteLocID;
        public string BuildNo;
        public string BuiliID;
        public string ErectedDate1;
        public string ScaffoldsStanding1;
        public string TonnageErected1;
        public string TonnageStanding1;
        public string AverageTonnage1;
        public string HireDays1;
        public string TotalRegisterforsite;
        public string LockedinReg;
        public string LockedPerct;
        public string TotalCompliance;  //16-Aug-2019
        public string TotaNonCompliance;  //16-Aug-2019
        public string InspectDashTotal;   //16-Aug-2019
        public string CompliancePct;   //16-Aug-2019
        public string NonCompliancePct;   //16-Aug-2019

        public string ErectedDate2;
        public string ScaffoldsStanding2;
        public string TonnageErected2;
        public string TonnageStanding2;
        public string AverageTonnage2;
        public string HireDays2;
        public decimal PctAllSite;
        public decimal TonnageByJob1Pct;
        public decimal totalTonnageStanding;
        public string AvgHireDays1;


        // Dashboard2 End

        //Map Location 
        public string Lat;
        public string Long;
        public string Lattitude;
        public string Longitude;
        public string MapSiteLocation;
        public decimal RID;
        //public string ScaffRef;
        public string Status;
        public string BuildDuringShutdown;

        public string StartDate;
        public string EndDate;
        public string PlanDuration;
        public string DesignType;
        public string MaterialTypeMap;
        public string BuildingMap;
        public string Tonnage;
        public string FloorLevelMap;
        public string StandingDaysinEditclient;
        public decimal ManHrsDismantle;
        //RegisterRatioValueGet Start
        public decimal ManHrsRatio;
        //RegisterRatioValueGet End

        //incpectionlicence
        public string Licencepic;


        //Cyclone
        public string Cyclone3Scafref;
        public decimal Cyclone3Tonnage;
        public decimal Cyclone3Manhrs;
        public string CycloneAction;
        public decimal TotalCounts;
        public string CycloneActionHr;
        public decimal TotalCountsDismantle;
        public string CycloneColor;
        public string Mahhrsavail;
        public string totalprephrs;
        public string ColorStatus;
        public decimal ManHrs5;
        public string Scafpac;
        public decimal Tonnage5;
        public string color5;
        public string ScaffRefAction;
        public string DescriptionAction;
        public string Location;
        public string FloorLevel;
        public string BuildingNo;
        public string assetId;
        public string PreparationHrs;
        public string RiskRating;
        public string WONumber;
        public string ClientScaffref;

        //Item Utilize
        public string Material;
        public decimal TotalTonnes;
        public decimal Material1;
        public decimal TotalTonnes1;
        public decimal Material2;
        public decimal TotalTonnes2;
        public decimal Material3;
        public decimal TotalTonnes3;

        public decimal PctTonnage;
        public string TypeList1;
        public string TypeList2;
        public string TypeList3;
        //Site Structure Analysis
        public string designtype;
        public int rquantity;
        public string Chart1difficultyrating;
        public string difficultyrating;
        public int RCount;
        public string Height;
        public string DRating;
        public int DRCount;

        //Weekly progress Dashboard
        public int WeekNo;
        public decimal TotErectedTonnage;
        public decimal TotDismantleTonnage;
        public decimal TotManHrsErect;
        public decimal TotManHrsDismantle;
        public decimal TotErectedRegister;
        public decimal TotDismantleRegister;
        public string WeekStartDate;
        public string WeekEndDate;

        public int InventoryId;
        public string MateralType;
        public string ItemCode;
        public int AvailableQuantity;
        public int Quantity;
        public int TransReqId;

        public string SiteTotal;
        public string LocationTotal;

        public string XPosition;
        public string YPosition;
        public string SiteFloorLevelImage;
        public string RegisterID;
        public string PlannedStandingDuration;
        public string DateTagged;

        public string DaysSinceLastInsp;
        public string RecentInspDate;

        public string ClientDefaultFloorLabelImage;


        public string AssetNo;
        public string DrawingName;
        public string Series;
        public decimal SeriesValue;
        public decimal Pct;

        public string StandingScaffold;
        public string StandingTonnage;
        public string ATPACTonnage;
        public string TubeFitTonnage;
        public string AvgHireDays2;
        public string GraphName;
        public string OtherTonnage;
        public string AverageStandingTonnage1;
        public string AvgStandingHireDays1;
        public string AverageStandingTonnage2;
        public string AvgStandingHireDays2;
        public string HireDaysToDate;
        public string PipelineStartDate;
        public string Image;
    }
}