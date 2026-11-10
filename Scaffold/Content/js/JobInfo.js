

    function TabHideShow() {
        var id = getParameterByName('id');
        var RID = getParameterByName('RID');
        var userRole = getParameterByName('UserRole');
        $.ajax({
            type: "POST",
            url: "/Register/TabHideShow",
            data: JSON.stringify({ ClientID: id, RegisterID: RID, userRole: userRole }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (response) {
                var GetData = response;
                var CycloneCount = GetData.CycloneCount == null ? 0 : GetData.CycloneCount;
                var Plans = GetData.Plans == null ? "" : GetData.Plans;
                var CycloneManagement = GetData.CycloneManagement == null ? "" : GetData.CycloneManagement;
                var MaintainInventory = GetData.MaintainInventory == null ? "" : GetData.MaintainInventory;
                var RoleNumber = GetData.userRole == null ? "" : GetData.userRole;
                var NoInventory = GetData.NoInventory == null ? "" : GetData.NoInventory;
                var PipeLineInvHide = GetData.PipeLineInvHide == null ? "" : GetData.PipeLineInvHide;
                //if (NoInventory == "True") {
                //    $('#limaterialPhase2').css('display', 'none');
                //        }
                if (RoleNumber == 10) {
                    //LiSacafpac.Visible = false; 
                    //limaterialPhase2.Visible = true;
                    //liFinalList.Visible = true;
                    $('#LiSacafpac').css('display', 'none');
                    $('#limaterialPhase2').css('display', 'block');
                    $('#liFinalList').css('display', 'block');
                    if (CycloneManagement != "True") {
                        $('#liCyclone').css('display', 'none');
                    }
                }
                else if (RoleNumber == 32) {
                    //LiHierarchyofAccessChoice.Visible = false;
                    //LiSacafpac.Visible = false;
                    //limaterialPhase2.Visible = false;
                    //liFinalList.Visible = false;
                    //liCyclone.Visible = false;

                    $('#LiHierarchyofAccessChoice').css('display', 'none');
                    $('#LiSacafpac').css('display', 'none');
                    $('#limaterialPhase2').css('display', 'none');
                    $('#liFinalList').css('display', 'none');
                    $('#liCyclone').css('display', 'none');
                }
                else if (RoleNumber == 34) {
                    //LiHierarchyofAccessChoice.Visible = false;
                    //LiSacafpac.Visible = false;
                    //limaterialPhase2.Visible = false;
                    //liFinalList.Visible = false;
                    //DataTable dt = databaseCon.GetData("select Count(*) as Count from tblRegister join tblSiteLocation on tblSiteLocation.SiteLocID=tblRegister.SiteLocId  where tblSiteLocation.ClientID='" + ClientID + "' and CycloneManagement=1  and RegisterId='" + RegisterID + "'");
                    //if (dt.Rows[0]["Count"].ToString() == "0")
                    //{
                    //    liCyclone.Visible = false;
                    //}

                    $('#LiHierarchyofAccessChoice').css('display', 'none');
                    $('#LiSacafpac').css('display', 'none');
                    $('#limaterialPhase2').css('display', 'none');
                    $('#liFinalList').css('display', 'none');
                    if (CycloneCount == 0) {
                        $('#liCyclone').css('display', 'none');
                    }
                }
                else if (RoleNumber == 60 || RoleNumber == 61) {
                    $('#limaterialPhase2').css('display', 'none');
                    $('#liFinalList').css('display', 'none');
                    $('#limaterialPhase3').css('display', 'none');
                }
                else {
                    if (Plans == "Plan1") {
                        //liFinalList.Visible = false;
                        //limaterialPhase2.Visible = false;
                        $('#liFinalList').css('display', 'none');
                        $('#limaterialPhase2').css('display', 'none');
                    }
                    else if (Plans == "Plan2") {
                        $('#LiSacafpac').css('display', 'none');
                        // LiSacafpac.Visible = false;
                        
                    }

                    if (CycloneManagement != "True") {
                        // liCyclone.Visible = false;
                        $('#liCyclone').css('display', 'none');
                    }
                    if (MaintainInventory!= "True") {
                        //LiHierarchyofAccessChoice.Visible = false;
                        //LiSacafpac.Visible = false;
                        //limaterialPhase2.Visible = false;
                        //liFinalList.Visible = false;

                        $('#LiHierarchyofAccessChoice').css('display', 'none');
                        $('#LiSacafpac').css('display', 'none');
                        $('#limaterialPhase2').css('display', 'none');
                        $('#liFinalList').css('display', 'none');
                    }
                }
                if (NoInventory == "True") {
                    $('#limaterialPhase2').css('display', 'none');
                    $('#liFinalList').css('display', 'none');
                }

                if (PipeLineInvHide == "True" && RoleNumber != 60 && RoleNumber != 61 ) {
                    $('#limaterialPhase3').css('display', 'block');

                }
                else {
                    $('#limaterialPhase3').css('display', 'none');
                }
            }
        });
    }


function ShowJobInfoDetails() {

    TabHideShow();
    $('#LoadingPanel1').show();
    var id = getParameterByName('id');
    var RID = getParameterByName('RID');
    var hdnTimeOffSet = $('#hdnTimeOffSetForAllPage').val();   // Getting Values From Layout Page
    $.ajax({
        type: "POST",
        url: "/Register/ShowregisterDetails",
        //data: JSON.stringify({ id: id, RID: RID }),
        data: JSON.stringify({ id: id, RID: RID, hdnTimeOffSet: hdnTimeOffSet }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            var str = "";
            var GetData = response;
            var REGISTERId = GetData.REGISTERId == null ? "" : GetData.REGISTERId;
            var SiteLocation = GetData.SiteLocation == null ? "" : GetData.SiteLocation;
            var Location = GetData.Location == null ? "" : GetData.Location;
            var InspectedBy = GetData.InspectedBy == null ? "" : GetData.InspectedBy;
            var RecentInspDate = GetData.RecentInspDate == null ? "" : GetData.RecentInspDate;
            var NoOfItems = GetData.NoOfItems == null ? "" : GetData.NoOfItems;
            var Tonnage = GetData.Tonnage == null ? "" : GetData.Tonnage;
            var OptimizeStatus = GetData.OptimizeStatus == null ? "" : GetData.OptimizeStatus;
            var ScafRefNo = GetData.ShowRegisterID == null ? "" : GetData.ShowRegisterID;
            var HireDaysToDate = GetData.HireDaysToDate == null ? "" : GetData.HireDaysToDate;
            var TotalHireDays = GetData.TotalHireDays == null ? "" : GetData.TotalHireDays;
            var DaysSinceLastInsp = GetData.DaysSinceLastInsp == null ? "" : GetData.DaysSinceLastInsp;
            var Status = GetData.Status == null ? "" : GetData.Status;
            var OnlineApplied = GetData.OnlineApplied == null ? "" : GetData.OnlineApplied;
            var PlanningStatus = GetData.PlanningStatus == null ? "" : GetData.PlanningStatus;
            var DateTagged = GetData.DateTagged == null ? "" : GetData.DateTagged;
            var DateTaggedTXT = GetData.DateTaggedTXT == null ? "" : GetData.DateTaggedTXT;
            var DateOFDismantle = GetData.DateOFDismantle == null ? "" : GetData.DateOFDismantle;
            var CheckListID = GetData.CheckListId == null ? "" : GetData.CheckListId;
            
            // Get Hierarchy Of Access Choice Data

            var HierarchyComment1 = GetData.HierarchyComment1 == null ? "" : GetData.HierarchyComment1;
            var HierarchyComment2 = GetData.HierarchyComment2 == null ? "" : GetData.HierarchyComment2;
            var HierarchyComment3 = GetData.HierarchyComment3 == null ? "" : GetData.HierarchyComment3;
            var HierarchyComment4 = GetData.HierarchyComment4 == null ? "" : GetData.HierarchyComment4;
            var HierarchyComment5 = GetData.HierarchyComment5 == null ? "" : GetData.HierarchyComment5;
            var HierarchySignedOff = GetData.HierarchySignedOff == null ? "" : GetData.HierarchySignedOff;
          
           // Get Cyclone Data
          
            var TotalLabourDismantleEdit = GetData.TotalLabourDismantleEdit == null ? "" : GetData.TotalLabourDismantleEdit;
            var CycloneAction = GetData.CycloneAction == null ? "" : GetData.CycloneAction;
            var PriorityRiskRating = GetData.PriorityRiskRating == null ? "" : GetData.PriorityRiskRating;
            var TotalLabourErectEdit = GetData.TotalLabourErectEdit == null ? "" : GetData.TotalLabourErectEdit;
            var TotalErectCountEdit = GetData.TotalErectCountEdit == null ? "" : GetData.TotalErectCountEdit;
            var TotalDismantleCountEdit = GetData.TotalDismantleCountEdit == null ? "" : GetData.TotalDismantleCountEdit;
            var CycloneComment = GetData.CycloneComment == null ? "" : GetData.CycloneComment;
            var UseEnteredValue = GetData.UseEnteredValue == null ? "" : GetData.UseEnteredValue;
            var TotalLabourDismantle = GetData.TotalLabourDismantle == null ? "" : GetData.TotalLabourDismantle;
            var TotalLabourErect = GetData.TotalLabourErect == null ? "" : GetData.TotalLabourErect;
            var TotalDismantleCount = GetData.TotalDismantleCount == null ? "" : GetData.TotalDismantleCount;
            var TotalErectCount = GetData.TotalErectCount == null ? "" : GetData.TotalErectCount;
            var ddlRangeOfOutBoard = GetData.WithInRangeOfOutBoard == null ? false : GetData.WithInRangeOfOutBoard;
            var ManHrsErect = GetData.ManHrsErect == null ? "" : GetData.ManHrsErect.toFixed(2);
            var ManHrsDismantle = GetData.ManHrsDismantle == null ? "" : GetData.ManHrsDismantle.toFixed(2);
            IsSiteStatus = GetData.SiteLocationStatus == null ? "Active" : GetData.SiteLocationStatus;
            var CopiedScafRefNo = GetData.ShowCopiedRegisterID == null ? "" : GetData.ShowCopiedRegisterID;
            if (TotalLabourDismantleEdit == null || TotalLabourDismantleEdit == 0) {
                $('#numberofhours').css('display', 'block');
            }
            else {
                $('#numberofhours').css('display', 'none');

            }
            var ChangesStatus = "";

            if (CycloneAction == "Tie-Down") {

                var ChangesStatus = CycloneAction.replace("Tie-Down", "'Tied-Down'")
            }

            else {
                var ChangesStatus = CycloneAction.replace("De-Plank Only", "'De-Planked'")
            }

            if (CycloneAction != "") {
                $('#cycloneactionStatus').html('NOTE:The Cyclone Action for this Job is: ' + ChangesStatus);
            }
            else {
                $('#cycloneactionStatus').html('NOTE:No Cyclone Action has been set for this Job');
            }
            if (CopiedScafRefNo != "") {
                $('#txtcopiedscaffref').val(CopiedScafRefNo);
            }
            $('#txtManHrsErect').val(ManHrsErect);
            $('#txrManHrsDis').val(ManHrsDismantle);
            $('#ddlCycloneAction').val(CycloneAction);
            $('#ddlPriorityRiskRating').val(PriorityRiskRating);
            if (TotalLabourDismantleEdit != "") {
                $('#txtTotalDeBoardEdit').val(TotalLabourDismantleEdit.toFixed(5));
            }
            if (TotalLabourErectEdit != "") {
                $('#txtTotalReInstateEdit').val(TotalLabourErectEdit.toFixed(5));
            }
            if (TotalErectCountEdit!="") {
                $('#txtreInstateEdit').val(TotalErectCountEdit.toFixed(2));
            }
            if (TotalDismantleCountEdit != "") {
                $('#txtDeBoardItemEdit').val(TotalDismantleCountEdit.toFixed(2));
             }
            $('#txtcomments').val(CycloneComment);
            if (ddlRangeOfOutBoard==true) {
                $('#ddlRangeOfOutBoard').val(1); 
            } else {
                $('#ddlRangeOfOutBoard').val(0);
            }

            if (UseEnteredValue != "") {
                // chkuseentervalue.Checked = true;
                if (UseEnteredValue == true) {
                    $('#chkuseentervalue').prop('checked', true);
                } else {
                    $('#chkuseentervalue').prop('checked', false);
                }
            }
           
            if (TotalLabourDismantle != "") {
                $('#txtTotalDeboardTotal').val(TotalLabourDismantle.toFixed(5));
            } if (TotalLabourErect != "") {
                $('#txtTotalreinstate').val(TotalLabourErect.toFixed(5));
            } if (TotalDismantleCount != "") {
                $('#txtCountDeBoard').val(TotalDismantleCount.toFixed(2));
            }
            if (TotalErectCount != "") {
                $('#txtCountReinstate').val(TotalErectCount.toFixed(2));
            }
           
          //  txtTotalDeboardTotal.Text = Convert.ToString(dt.Rows[0]["TotalLabourDismantle"]);
          //  txtTotalreinstate.Text = Convert.ToString(dt.Rows[0]["TotalLabourErect"]);
          //  txtCountDeBoard.Text = Convert.ToString(dt.Rows[0]["TotalDismantleCount"]);
          //  txtCountReinstate.Text = Convert.ToString(dt.Rows[0]["TotalErectCount"]);
          // // ddlCycloneAction.SelectedValue = Convert.ToString(dt.Rows[0]["CycloneAction"]);
          // // ddlPriorityRiskRating.Text= Convert.ToString(dt.Rows[0]["PriorityRiskRating"]);


          ////  txtTotalDeBoardEdit.Text = Convert.ToString(dt.Rows[0]["TotalLabourDismantleEdit"]);
          ////  txtTotalReInstateEdit.Text = Convert.ToString(dt.Rows[0]["TotalLabourErectEdit"]);
          //  txtDeBoardItemEdit.Text = Convert.ToString(dt.Rows[0]["TotalDismantleCountEdit"]);
          ////  txtreInstateEdit.Text = Convert.ToString(dt.Rows[0]["TotalErectCountEdit"]);
          //  txtManHrsErect.Text = Convert.ToString(dt.Rows[0]["ManHrsErect"]);
          //  txrManHrsDis.Text = Convert.ToString(dt.Rows[0]["ManHrsDismantle"]);
          ////  txtcomments.Text = Convert.ToString(dt.Rows[0]["CycloneComment"]);
            
            



            $('#txtSiteName').val(SiteLocation);
            $('#txtSiteLocation').val(Location);
            $('#txtInspectedBy').val(InspectedBy);
            $('#txtRecentinspectiondate').val(RecentInspDate);
            $('#txtDaylastinspetion').val(DaysSinceLastInsp);
            $('#txtstatus').text(Status);
            if (Status.toUpperCase() == "Red".toUpperCase() || Status.toUpperCase() == "Green".toUpperCase() || Status.toUpperCase() == "Yellow".toUpperCase()) {

                $('#txtstatus').css('display', 'none');
                $('#imgstaus').css('display', 'block');
                $('#imgstaus').attr("src", "/images/" + Status + ".jpg");
            }
            else {
                $('#txtstatus').css('display', 'block');
                $('#imgstaus').css('display', 'none');

            }
            $('#txtPlannedStatus').val(PlanningStatus);
            $('#txtHireDays').val(HireDaysToDate);
            $('#txtTotalhireday').val(TotalHireDays);
            $('#txtNoOfItem').val(NoOfItems);
            $('#txttonnage').val(Tonnage);
            $('#txtoptstatus').val(OptimizeStatus);
            if (OptimizeStatus.toUpperCase() == "Green".toUpperCase()) {
                $('#txtoptstatus').css('display', 'none');
                $('#imgopt').css('display', 'block');
                $('#imgopt').attr("src", "/images/green.jpg");
            }
            else {
                $('#txtoptstatus').css('display', 'none');
                $('#imgopt').css('display', 'block');
                $('#imgopt').attr("src", "/images/Red.jpg");
            }
            $('#txtscaffref').val(ScafRefNo);
           
           // if (dt.Rows[0]["DateTagged"] != DBNull.Value && Convert.ToString(dt.Rows[0]["DateTagged"]) != "01/01/1900 12:00:00 AM") {
            if (DateTagged != "") {
                //rdpDateTagged.SelectedDate = Convert.ToDateTime(dt.Rows[0]["DateTagged"]);
                //txtdatetagged.Text = Convert.ToDateTime(dt.Rows[0]["DateTagged"]).ToString("dd-MMM-yyyy");
                $('#rdpDateTagged').val(DateFormat(DateTagged));
            }
            $('#txtdatetagged').val(DateTaggedTXT);
            // if (dt.Rows[0]["DateOFDismantle"] != DBNull.Value && Convert.ToString(dt.Rows[0]["DateOFDismantle"]) != "01/01/1900 12:00:00 AM") {
            if (DateOFDismantle != "") {
                //rdpdatedis.SelectedDate = Convert.ToDateTime(dt.Rows[0]["DateOFDismantle"]);
                //txtDatedismantle.Text = Convert.ToDateTime(dt.Rows[0]["DateOFDismantle"]).ToString("dd-MMM-yyyy");
                $('#rdpdatedis').val(DateFormat(DateOFDismantle));
                $('#txtDatedismantle').val(DateFormat(DateOFDismantle));
            }

            $('#txtComment1').val(HierarchyComment1);
            $('#txtComment2').val(HierarchyComment2);
            $('#txtComment3').val(HierarchyComment3);
            $('#txtComment4').val(HierarchyComment4);
            $('#txtComment5').val(HierarchyComment5);
            if (HierarchySignedOff != "") {
                $('#ddlsupervisor').val(HierarchySignedOff);
            }
           
            $('#hdnCheckListID').val(CheckListID);
            

            //txtComment1.Text = Convert.ToString(dt.Rows[0]["HierarchyComment1"]);
            //txtComment2.Text = Convert.ToString(dt.Rows[0]["HierarchyComment2"]);
            //txtComment3.Text = Convert.ToString(dt.Rows[0]["HierarchyComment3"]);
            //txtComment4.Text = Convert.ToString(dt.Rows[0]["HierarchyComment4"]);
            //txtComment5.Text = Convert.ToString(dt.Rows[0]["HierarchyComment5"]);
            //ddlsupervisor.SelectedValue = Convert.ToString(dt.Rows[0]["HierarchySignedOff"]);

            // ScaffoldMaterialListPhase2 Condation
            if (Status == "Completed") {
                //btnrequest.Visible = false;
                //btnReturn.Visible = false;
                //btnDamage.Visible = false;
                //freeze.Visible = false;
                $('#btnrequest').css('display', 'none');
                $('#btnReturn').css('display', 'none');
                $('#btnDamage').css('display', 'none');
                $('#freeze').css('display', 'none');
            }

            $('#LoadingPanel1').hide();
        }
    })
}


$(function () {

    var Href = window.location.href.split('?');
    if (Href.length == 1) {
        $('#UlNavTabSeciton li').css('display', 'none');
        $('#liCompany').css('display', 'block');
    }
    $('#UlNavTabSeciton a').css('cursor', 'pointer');
    $('#UlNavTabSeciton a').click(function (e) {
       // $('#LoadingPanel1').css('display', 'block');
        var ClickAcnhorID = e.currentTarget.id;
        if (ClickAcnhorID == 'AnchorRegister') {
            window.location.href = '/Register/Register?' + Href[1];
           // $('#liCompany').removeClass('active').addClass('active');
            $('#LoadingPanel1').css('display', 'none');
        }
        if (ClickAcnhorID == 'AnchorCyclone') {
           // window.location.href = '/Cyclone.aspx?' + Href[1];
            window.location.href = '/Cyclone/Cyclone?' + Href[1];
        }
        //if (ClickAcnhorID == 'AnchorScafpacDetails') {
        //  //  window.location.href = '/ScafpacDetails.aspx?' + Href[1];
        //    window.location.href = '/ScafpacDetails/ScafpacDetails?' + Href[1];
        //}
        if (ClickAcnhorID == 'AnchorHierarchyofAccessChoice') {
            //  window.location.href = '/HierarchyofAccessChoice.aspx?' + Href[1];
           // $('#LiHierarchyofAccessChoice').removeClass('active').addClass('active');
            window.location.href = '/HierarchyofAccessChoice/HierarchyofAccessChoice?' + Href[1];           
        }
        if (ClickAcnhorID == 'AnchorScafpacphase3') {
            // window.location.href = '/ScaffoldMaterialListPhase2.aspx?' + Href[1];
            window.location.href = '/ScaffoldMaterialListPhase3/ScaffoldMaterialListPhase3?' + Href[1];
        }
        if (ClickAcnhorID == 'AnchorScafpacphase2') {
           // window.location.href = '/ScaffoldMaterialListPhase2.aspx?' + Href[1];
            window.location.href = '/ScaffoldMaterialListPhase2/ScaffoldMaterialListPhase2?' + Href[1];
        }
        if (ClickAcnhorID == 'AnchorFinallist') {
           // window.location.href = '/FinalMaterialList.aspx?' + Href[1];
            window.location.href = '/FinalMaterialList/FinalMaterialList?' + Href[1];
        }

        if (ClickAcnhorID == 'AnchorInspection') {
            //window.location.href = '/InspectionList.aspx?' + Href[1];
            window.location.href = '/InspectionList/InspectionList?' + Href[1];
        }
        if (ClickAcnhorID == 'AnchorHandOver') {
           // window.location.href = '/HandOver.aspx?' + Href[1];
            window.location.href = '/HandOver/HandOver?' + Href[1];
        }
        if (ClickAcnhorID == 'Anchordocument') {
            //window.location.href = '/Document.aspx?' + Href[1];
            window.location.href = '/Document/Document?' + Href[1];
        }
        if (ClickAcnhorID == 'AnchorImg') {
          //  window.location.href = '/ImageGallery.aspx?' + Href[1];
            window.location.href = '/ImageGallery/ImageGallery?' + Href[1];
        }
        if (ClickAcnhorID == 'Anchordrawings') {
           // window.location.href = '/Drawings.aspx?' + Href[1];
            window.location.href = '/Drawings/Drawings?' + Href[1];
        }

    });


    // getmap(hideen);

});
