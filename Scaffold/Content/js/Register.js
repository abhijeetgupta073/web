$(function () {

    var CallFrom = getParameterByName('CallFrom');
    var DecryptData = [CallFrom];
    var decryData = Decrypt(DecryptData);
    CallFrom = decryData[0];
    if (CallFrom == 'FloorMap') {
        $('#BackTofloorMap').css('display', 'block');
    }
    else if (CallFrom == 'OffSystemInspection') {
        $('#BackToOffSystemInspection').css('display', 'block');
        $('#btnCAncel').css('display', 'none');
    }
    else {
        $('#btnCAncel').css('display', 'block');
        //$('#BackTofloorMap').css('display', 'none');
    }
    //For No Test Values Set On Text Box Only Set number
    //setInputFilter(document.getElementById("txtlength"), function (value) {
    //    return /^-?\d*[.,]?\d*$/.test(value);
    //});
    //setInputFilter(document.getElementById("txtwidth"), function (value) {
    //    return /^-?\d*[.,]?\d*$/.test(value);
    //});
    //setInputFilter(document.getElementById("txtheight"), function (value) {
    //    return /^-?\d*[.,]?\d*$/.test(value);
    //});
    document.getElementById("txtlength").addEventListener('input', restrictToInteger);
    document.getElementById("txtwidth").addEventListener('input', restrictToInteger);
    document.getElementById("txtheight").addEventListener('input', restrictToInteger);
    document.getElementById("txtManhrserect").addEventListener('input', restrictToInteger);
    document.getElementById("txtmanDisman").addEventListener('input', restrictToInteger);
    document.getElementById("txtCrewSize").addEventListener('input', restrictToInteger);
    function restrictToInteger() {
        this.value = this.value.replace(/[^\d.]/g, '');
    }

    $('#liCompany').removeClass('active').addClass('active');
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var RID = getParameterByName('RID');

    var DecryptData = Decrypt([RID, id]);
    RID = DecryptData[0];
    var ClientID = DecryptData[1];
    $('#hdnClientID').val(ClientID);
    $('#hdnRID').val(RID);
    BindallDropdown();
    //Added on (06-12-2023) We are now creating Drop Dow of Shift Allocation Dynamically.
    ShiftAllocationDropdownBind();
    if (RID == "0") {
        $('#liCyclone').css('display', 'none');
        $('#QrCodePrint').css('display', 'none');
        $('#btnPre').css('display', 'none');
        $('#btnOfflineLink').css('display', 'none');
        $('#btnClearOfflineJobData').css('display', 'none');
        $('#btnNext').css('display', 'none');
        $('#qrcodediv').css('display', 'none');
        $('#btnAddFloorLevelImage').css('display', 'none');
        $('#LiInspection').css('display', 'none');
        $('#liFinalList').css('display', 'none');
        $('#LiSacafpac').css('display', 'none');
        $('#LiHandOver').css('display', 'none');
        $('#liimage').css('display', 'none');
        $('#LiHierarchyofAccessChoice').css('display', 'none');
        $('#limaterialPhase3').css('display', 'none');
        $('#btnCloseOutJob').css('display', 'none');

        $('#limaterialPhase2').css('display', 'none');
        $('#LiDocuments').css('display', 'none');
        $('#lidrawings').css('display', 'none');
        $('#lblLocaked').css('display', 'none');
        $('#ddlLockedIn').css('display', 'none');

        $('#rdpdatedis').attr('disabled', 'true');

        $('#ddlPlanningStatus').val('Planned');
        $('#MainDivJob').css('display', 'block');


        //query = "exec Pro_TBLCLIENT_GET '" + databaseCon.Decrypt(Request.QueryString["id"]) + "'";
        //dt = databaseCon.GetData(query);
        //if (dt.Rows.Count != 0)
        //{
        //    ddlInspectionTimeFrame.SelectedValue = Convert.ToString(dt.Rows[0]["InspTimeFrame"]);
        //    ddlChecklistInsp.SelectedValue = Convert.ToString(dt.Rows[0]["ChecklistId"]);
        //}

    } else {
        TabHideShow();
        ShowregisterDetails();
        //QRCodeGenerate();
        // getFloorImage();
        // FloorLabelMarksGet();

    }
    //if (Request.QueryString["ChkID"] != null && Request.QueryString["ChkID"] != "")
    //{
    //    string chkID = databaseCon.Decrypt(Request.QueryString["ChkID"].ToString());
    //    ddlChecklistInsp.SelectedValue = chkID;
    //    //ddlChecklistInsp.SelectedValue = Request.QueryString["ChkID"];
    //}

   /* ShowJobInfoDetails();*/

    $("#rdpDateTagged").datepicker({
        dateFormat: 'dd-M-yy',
        maxDate: new Date(),
        //added by ss on 07 feb 2025 add Regex  for enter valid date  

        onClose: function (selectedDate) {
            if (selectedDate.trim() !== "" && !isValidDate(selectedDate)) {
                let todayDate = $.datepicker.formatDate('dd-M-yy', new Date());
                alert(`Invalid date! Please enter a valid date like ${todayDate} or select from the calendar.`);
                $(this).val('');
            }
        }

    });

    $("#rdpdatedis").datepicker({
        dateFormat: 'dd-M-yy',
        maxDate: new Date(),
        /*closeText: 'Close',*/
        //added by ss on 07 feb 2025 add Regex  for enter valid date  

        onClose: function (selectedDate) {
            if (selectedDate.trim() !== "" && !isValidDate(selectedDate)) {
                let todayDate = $.datepicker.formatDate('dd-M-yy', new Date());
                alert(`Invalid date! Please enter a valid date like ${todayDate} or select from the calendar.`);
                $(this).val('');
            }
        }

    });
    $('#rdpdatedis').off('keydown').on('keydown', function (e) {
        e.preventDefault();
    });


    $("#newWorkCarriedOutDate").datepicker({
        dateFormat: 'dd-M-yy',
        maxDate: new Date(),
        //closeText: 'Close',
        //added by ss on 07 feb 2025 add Regex  for enter valid date  

        onClose: function (selectedDate) {
            if (selectedDate.trim() !== "" && !isValidDate(selectedDate)) {
                let todayDate = $.datepicker.formatDate('dd-M-yy', new Date());
                alert(`Invalid date! Please enter a valid date like ${todayDate} or select from the calendar.`);
                $(this).val('');
            }
        }

    });

    $("#rddateDisAppro").datepicker({
        dateFormat: 'dd-M-yy',
        //  maxDate: 0,
        //closeText: 'Close',
        //added by ss on 07 feb 2025 add Regex  for enter valid date  

        onClose: function (selectedDate) {
            if (selectedDate.trim() !== "" && !isValidDate(selectedDate)) {
                let todayDate = $.datepicker.formatDate('dd-M-yy', new Date());
                alert(`Invalid date! Please enter a valid date like ${todayDate} or select from the calendar.`);
                $(this).val('');
            }
        }

    });

    $("#PlannedDate").datepicker({
        dateFormat: 'dd-M-yy',
        /* maxDate: new Date(),*/
        //closeText: 'Close',
        //added by ss on 07 feb 2025
        onClose: function (selectedDate) {
            if (selectedDate.trim() !== "" && !isValidDate(selectedDate)) {
                let todayDate = $.datepicker.formatDate('dd-M-yy', new Date());
                alert(`Invalid date! Please enter a valid date like ${todayDate} or select from the calendar.`);
                $(this).val('');
            }
        }

    });
    $(document).on('click', '#chkNoInventory', function () {
        var $checkbox = $(this);

        if ($checkbox.prop('checked') == true) {

            var id = getParameterByName('id');
            var RID = getParameterByName('RID');

            var DecryptData = [RID];
            var decryData = Decrypt(DecryptData);
            var DecryptedRID = decryData[0];

            if (!DecryptedRID || DecryptedRID == "0") {
                return;
            }

            $('#LoadingPanel1').show();

            $.ajax({
                type: "POST",
                url: "/Register/CheckJobInventoryExists",
                data: JSON.stringify({ id: id, RID: RID }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (response) {
                    $('#LoadingPanel1').hide();

                    if (response.HasInventory == true) {
                        $checkbox.prop('checked', false);

                        var Response = {
                            Result: "Failed",
                            Message: "This job already has Material / Inventory requested or delivered on it. You cannot mark 'No Inventory Required' for this job."
                        };
                        CustomAlert(Response);
                    }
                },
                error: function () {
                    $('#LoadingPanel1').hide();
                    $checkbox.prop('checked', false);
                    var Response = { Result: "Failed", Message: "Unable to verify inventory status. Please try again." };
                    CustomAlert(Response);
                }
            });
        }
    });


    $('input[name="SelectJobType"]').click(function () {
        let datepickerOptions = {
            dateFormat: 'dd-M-yy',
            closeText: 'Close'
        };

        if ($('input[name="SelectJobType"]:checked').val() != "0") {
            datepickerOptions.minDate = new Date();
            $("#BuildDate").attr("disabled", true).datepicker('destroy');
            $("#PipelineStartDate").attr("disabled", false).datepicker('destroy').datepicker(datepickerOptions);
            $("#WithInventoryForLiveJob").attr("disabled", true);
            $("#WithInventoryForPipelineJob").attr("disabled", false);
            if ($("#WithInventoryForLiveJob").is(":checked")) {
                $("#WithInventoryForLiveJob").attr("checked", false);
            }

        } else {
            datepickerOptions.maxDate = new Date();
            $("#BuildDate").attr("disabled", false).datepicker('destroy').datepicker(datepickerOptions);
            $("#PipelineStartDate").attr("disabled", true).datepicker('destroy');
            $("#WithInventoryForPipelineJob").attr("disabled", true);
            $("#WithInventoryForLiveJob").attr("disabled", false);
            if ($("#WithInventoryForPipelineJob").is(":checked")) {
                $("#WithInventoryForPipelineJob").attr("checked", false);
            }

        }
    });
    $('.ClassB').click(function () {
        $('#PipelineStartDate').focus();
    });
    $('.ClassA').click(function () {
        $('#BuildDate').focus();
    });
    var today = new Date();

    $("#PipelineStartDate").val(today.DateMonthFormat());
    $("#BuildDate").val(today.DateMonthFormat());

    $(document).on("change", ".mob_file_image", function (evt) {
        var base64mobImagedata;
        var reader = new FileReader();
        reader.readAsDataURL(this.files[0]);
        reader.onloadend = function () {
            base64mobImagedata = reader.result;
            $("#imga").attr('src', base64mobImagedata);
            $("#tempimg").val(base64mobImagedata);
            $('#deleteimage').css('display', 'block');
            $('#btnImageSave').css('display', 'block');
            $('#downloadButton').css('display', 'none');

        }
    });

    $(document).on("change", "#mobApp_file", function (evt) {
        var base64mobdata;
        var reader = new FileReader();
        reader.readAsDataURL(this.files[0]);
        reader.onloadend = function () {
            base64mobdata = reader.result;
            $("#imga").attr('src', base64mobdata);
            $("#tempimg").val(base64mobdata);
            $('#deleteimage').css('display', 'block');
            $('#btnImageSave').css('display', 'block');
            $('#downloadButton').css('display', 'none');

        }
    });


});

$(function () {
    var $select = $(".1-20");
    for (i = 1; i <= 20; i++) {
        $select.append($('<option></option>').val(i).html(i))
    }
});

function BindallDropdown() {
    var id = $("#hdnClientID").val();
    $.ajax({
        url: '/Register/BindallDropdown',
        async: false,
        data: { id: id },
        success: function (response) {

            data = response;

            var ddSite = "<option value='' disabled selected='selected'></option>";
            $.each(response.ddlsites, function (idx, obj) {
                if (obj != null) {
                    ddSite += '<option value="' + obj.SiteLocID + '">' + obj.SiteLocation + '</option>';
                }
            });

            var ddDesign = "<option value=''></option>";
            $.each(response.ddlScaffoldDesign, function (idx, obj) {
                if (obj != null) {
                    ddDesign += '<option value="' + obj.DesignType + '">' + obj.DesignType + '</option>';
                }
            });

            var ddDuty = "<option value=''></option>";
            $.each(response.ddlDutyrating, function (idx, obj) {
                if (obj != null) {
                    ddDuty += '<option value="' + obj.DutyRating + '">' + obj.DutyRating + '</option>';
                }
            });

            var ddMaterial = "<option value=''></option>";
            $.each(response.ddlMaterialType, function (idx, obj) {
                if (obj != null) {
                    ddMaterial += '<option value="' + obj.MaterialType + '">' + obj.MaterialType + '</option>';
                }
            });

            var ddSupervisor = "<option value=''></option>";
            $.each(response.ddlsupervisor, function (idx, obj) {
                if (obj != null) {
                    ddSupervisor += '<option value="' + obj.UserId + '">' + obj.Supervisor + '</option>';
                }
            });

            var ddSignBySpr = "<option value=''></option>";
            $.each(response.ddlsignbysprvsr, function (idx, obj) {
                if (obj != null) {
                    ddSignBySpr += '<option value="' + obj.UserId + '">' + obj.Supervisor + '</option>';
                }
            });

            var ddlChecklist = "<option value=''></option>";
            $.each(response.ddlChecklistInsp, function (idx, obj) {
                if (obj != null) {
                    ddlChecklist += '<option value="' + obj.checklistID + '">' + obj.checklistdescription + '</option>';
                }
            });

            var ddlScaffoldOwnerRep = "<option value=''></option>";
            $.each(response.dropdownScaffoldOwnerRep, function (idx, obj) {
                if (obj != null) {
                    ddlScaffoldOwnerRep += '<option value="' + obj.ScaffoldOwner + '">' + obj.ScaffoldOwner + '</option>';
                }
            });

            var ddldiscipline = "<option value=''></option>";
            $.each(response.dropdowndiscipline, function (idx, obj) {
                if (obj != null) {
                    ddldiscipline += '<option value="' + obj.discipline + '">' + obj.discipline + '</option>';
                }
            });
            var dropdownSubGroup = "<option value=''></option>";
            //if (response.dropdownSubGroup && response.dropdownSubGroup.length === 1) {
            //    var obj = response.dropdownSubGroup[0];
            //    dropdownSubGroup += '<option value="' + obj.SubGroupId + '" selected>' + obj.SubGroup + '</option>';
            //} else {
            //    $.each(response.dropdownSubGroup, function (idx, obj) {
            //        if (obj != null) {
            //            dropdownSubGroup += '<option value="' + obj.SubGroupId + '">' + obj.SubGroup + '</option>';
            //        }
            //    });
            /* }*/
            var dropdownRequestedby = "<option value=''></option>";
            $.each(response.dropdownRequestedby, function (idx, obj) {
                if (obj != null) {
                    dropdownRequestedby += '<option value="' + obj.RequestById + '">' + obj.RequestBy + '</option>';
                }
            });
            $('#ddlsites').empty().append(ddSite);
            $('#ddlScaffoldDesign').empty().append(ddDesign);
            $('#ddlDutyrating').empty().append(ddDuty);
            $('#ddlMaterialType').empty().append(ddMaterial);
            $('#ddlsupervisor').empty().append(ddSupervisor);
            $('#ddlsignbysprvsr').empty().append(ddSignBySpr);
            $('#ddlChecklistInsp').empty().append(ddlChecklist);
            $('#txtScaffoldOwner').empty().append(ddlScaffoldOwnerRep);
            $('#txtdiscipline').empty().append(ddldiscipline);
            /* $('#ddlWorkGroups').empty().append(dropdownWorkGroup);*/
            $('#ddlSubGroups').empty().append(dropdownSubGroup);
            $('#ddlRequestedBy').empty().append(dropdownRequestedby);
        }
    });
}
function LocationDropdownBind() {
    var id = $("#hdnClientID").val();
    var SiteID = $('#ddlsites').val();
    $.ajax({
        url: '/Register/LocationDropdownBind',
        async: false,
        data: { id: id, SiteID: SiteID },
        success: function (response) {

            var ddlLocation = "<option value=''></option>";
            // var ddlLocation = "";
            $.each(response, function (idx, obj) {
                if (obj != null) {
                    ddlLocation += '<option value="' + obj.areaid + '">' + obj.area + '</option>';
                }
            });

            $('#ddlLocation').empty().append(ddlLocation);
        }
    });
}
function BuildingDropdownBind() {
    var id = getParameterByName('id');
    var LocationID = $('#ddlLocation').val();
    $.ajax({
        url: '/Register/BuildingDropdownBind',
        async: false,
        data: { id: id, LocationID: LocationID },
        success: function (response) {

            var GetData = response;
            var ddlBld = "<option value=''></option>";
            $.each(response, function (idx, obj) {
                if (obj != null) {
                    ddlBld += '<option value="' + obj.buildingid + '">' + obj.buildingno + '</option>';
                }

            });
            $('#ddbuilding').empty().append(ddlBld);
        }
    });
}
function AssetsDropdownBind() {
    var id = getParameterByName('id');
    var BuildingID = $('#ddbuilding').val();
    $.ajax({
        url: '/Register/AssetsDropdownBind',
        async: false,
        data: { id: id, BuildingID: BuildingID },
        success: function (response) {

            var GetData = response;
            var ddlassest = "<option value=''></option>";
            $.each(response, function (idx, obj) {
                if (obj != null) {
                    ddlassest += '<option value="' + obj.Assetid + '">' + obj.Assetno + '</option>';
                }

            });
            $('#ddassets').empty().append(ddlassest);
        }

    });
}
function FloorLevelDropdown() {
    var id = $("#hdnClientID").val();
    var SiteID = $('#ddlsites').val();
    var LocationID = $('#ddlLocation').val();
    var BuildingID = $('#ddbuilding').val();
    $.ajax({
        url: '/Register/FloorLevelDropdown',
        async: false,
        data: { id: id, SiteID: SiteID, LocationID: LocationID, BuildingID: BuildingID },
        success: function (response) {

            var GetData = response;
            var ddlFloorLebel = "<option value=''></option>";
            var ddlFloorLebel = "";
            $.each(response, function (idx, obj) {
                if (obj != null) {
                    // ddlFloorLebel += '<option value="' + obj.FloorLevel + '">' + obj.FloorLevel + '</option>';
                    ddlFloorLebel += '<option value="' + obj.FloorLevelId + '">' + obj.FloorLevel + '</option>';
                }
            });
            $('#DropDownList1').empty().append(ddlFloorLebel);
        }
    });
}
function ShowregisterDetails() {
    $('#LoadingPanel1').show();
    var id = getParameterByName('id');
    var RID = getParameterByName('RID');
    var hdnTimeOffSet = $('#hdnTimeOffSetForAllPage').val();
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
            var OwnerName = GetData.OwnerName == null ? "" : GetData.OwnerName;
            var discipline = GetData.discipline == null ? "" : GetData.discipline;
            var MaintainInventory = GetData.MaintainInventory == null ? "" : GetData.MaintainInventory;
            var SiteLocation = GetData.SiteLocation == null ? "" : GetData.SiteLocation;
            var SiteLocID = GetData.SiteLocID == null ? "" : GetData.SiteLocID;
            var LocationId = GetData.LocationId == null ? "" : GetData.LocationId;
            var Location = GetData.Location == null ? "" : GetData.Location;
            var BuildingID = GetData.BuildingID == null ? "" : GetData.BuildingID;
            var BuildingNo = GetData.BuildingNo == null ? "" : GetData.BuildingNo;
            var AssetID = GetData.AssetId == null ? "" : GetData.AssetId;
            var AssetNo = GetData.AssetNo == null ? "" : GetData.AssetNo;
            var ClientRef = GetData.ClientRef == null ? "" : GetData.ClientRef;
            var Description = GetData.Description == null ? "" : GetData.Description;
            var Location = GetData.Location == null ? "" : GetData.Location;
            var AssetDesc = GetData.AssetDesc == null ? "" : GetData.AssetDesc;
            var Length = GetData.Length == null ? "" : GetData.Length.toFixed(3);
            var Width = GetData.Width == null ? "" : GetData.Width.toFixed(3);
            var Height = GetData.Height == null ? "" : GetData.Height.toFixed(2);
            var FloorLevel = GetData.FloorLevel == null ? "" : GetData.FloorLevel;
            var FloorLevelId = GetData.FloorLevelId == null ? "" : GetData.FloorLevelId;
            var NoOfWorkingDecks = GetData.NoOfWorkingDecks == null ? "" : GetData.NoOfWorkingDecks;
            var EngineeredDesign = GetData.EngineeredDesign == null ? "" : GetData.EngineeredDesign;
            var DesignType = GetData.DesignType == null ? "" : GetData.DesignType;
            var DutyRating = GetData.DutyRating == null ? "" : GetData.DutyRating;
            var MaterialType = GetData.MaterialType == null ? "" : GetData.MaterialType;
            var InspectedBy = GetData.InspectedBy == null ? "" : GetData.InspectedBy;
            var RecentInspDate = GetData.RecentInspDate == null ? "" : GetData.RecentInspDate;
            var Lat = GetData.Lat == null ? "" : GetData.Lat;
            var Long = GetData.Long == null ? "" : GetData.Long;
            var NoOfItems = GetData.NoOfItems == null ? "" : GetData.NoOfItems;
            var Tonnage = GetData.Tonnage == null ? "" : GetData.Tonnage;
            var OptimizeStatus = GetData.OptimizeStatus == null ? "" : GetData.OptimizeStatus;
            var ScafRefNo = GetData.ShowRegisterID == null ? "" : GetData.ShowRegisterID;
            var HireDaysToDate = GetData.HireDaysToDate == null ? "" : GetData.HireDaysToDate;
            var TotalHireDays = GetData.TotalHireDays == null ? "" : GetData.TotalHireDays;
            var DaysSinceLastInsp = GetData.DaysSinceLastInsp == null ? "" : GetData.DaysSinceLastInsp;
            var Status = GetData.Status == null ? "" : GetData.Status;
            var OnlineApplied = GetData.OnlineApplied == null ? "" : GetData.OnlineApplied;
            var CheckListId = GetData.CheckListId == null ? "" : GetData.CheckListId;
            var PlanningStatus = GetData.PlanningStatus == null ? "" : GetData.PlanningStatus;
            var ShiftAllocation = GetData.ShiftAllocation == null ? "" : GetData.ShiftAllocation;
            var InspTimeFrame = GetData.InspTimeframe == null ? "" : GetData.InspTimeframe;
            var WONumber = GetData.WONumber == null ? "" : GetData.WONumber;
            var ClientScaffref = GetData.ClientScaffref == null ? "" : GetData.ClientScaffref;
            var ScafType = GetData.ScafType == null ? "" : GetData.ScafType;
            var LockedIn = GetData.LockedIn == null ? "" : GetData.LockedIn;
            var PlannedStandingDuration = GetData.PlannedStandingDuration == null ? "" : GetData.PlannedStandingDuration;
            var CrewSize = GetData.CrewSize == null ? "" : GetData.CrewSize;
            var ManHrsErect = GetData.ManHrsErect == null ? "" : GetData.ManHrsErect.toFixed(2);
            var ManHrsDismantle = GetData.ManHrsDismantle == null ? "" : GetData.ManHrsDismantle.toFixed(2);
            var SupervisorId = GetData.SupervisorId == null ? "" : GetData.SupervisorId;
            var Crew = GetData.Crew == null ? "" : GetData.Crew;
            var PermitNo = GetData.PermitNo == null ? "" : GetData.PermitNo;
            var Comments = GetData.Comments == null ? "" : GetData.Comments;
            var SAPNumber = GetData.SAPNumber == null ? "" : GetData.SAPNumber;
            var ScaffoldOwner = GetData.ScaffoldOwner == null ? "" : GetData.ScaffoldOwner;
            var LockedInSignedOffName = GetData.LockedInSignedOffName == null ? "" : GetData.LockedInSignedOffName;
            var DesignDrawingNo = GetData.DesignDrawingNo == null ? "" : GetData.DesignDrawingNo;
            var NumberOfPlatform = GetData.NumberOfPlatform == null ? "" : GetData.NumberOfPlatform;
            var Image = GetData.Image == null ? "" : GetData.Image;
            var PhotoAngle = GetData.PhotoAngle == null ? "" : GetData.PhotoAngle;
            var LockedInSignedOff = GetData.LockedInSignedOff == null ? "" : GetData.LockedInSignedOff;
            var DateTagged = GetData.DateTagged == null ? "" : GetData.DateTagged;
            var DateOFDismantle = GetData.DateOFDismantle == null ? "" : GetData.DateOFDismantle;
            var Plans = GetData.Plans == null ? "" : GetData.Plans;
            var InventoryCount = GetData.InventoryCount == null ? 0 : GetData.InventoryCount;
            var AvailableQuantity = GetData.AvailableQuantity == null ? 0 : GetData.AvailableQuantity;
            var WorkGroupid = GetData.WorkGroupid == null ? "" : GetData.WorkGroupid;
            var SubGroupId = GetData.SubGroupId == null ? "" : GetData.SubGroupId;
            var RequestedById = GetData.RequestedById == null ? "" : GetData.RequestedById;
            var SubGroup = GetData.SubGroup == null ? "" : GetData.SubGroup;
            var RequestedBy = GetData.RequestedBy == null ? "" : GetData.RequestedBy;
            var DismantleApprovedDate = GetData.DismantleApprovedDate == null ? "" : GetData.DismantleApprovedDate;
            var DismantleApprovedNotes = GetData.DismantleApprovedNotes == null ? "" : GetData.DismantleApprovedNotes;
            var Bearing = GetData.Bearing == null ? "" : GetData.Bearing;
            var LongTermStructure = GetData.LongTermStructure == null ? "" : GetData.LongTermStructure;
            var IsOfflineCurrent = GetData.IsOfflineCurrent == null ? "" : GetData.IsOfflineCurrent;
            var NoInventory = GetData.NoInventory == null ? "" : GetData.NoInventory;
            var PipelineStartDate = GetData.PipelineStartDate == null ? "" : GetData.PipelineStartDate;
            var PipeLineNote = GetData.PipeLineNote == null ? "" : GetData.PipeLineNote;
            var DateTaggedTXT = GetData.DateTaggedTXT == null ? "" : GetData.DateTaggedTXT;
            var ChkRequiresIsolation = GetData.ChkRequiresIsolation == null ? "" : GetData.ChkRequiresIsolation;
            var DismantledBy = GetData.DismantledBy == null ? "" : GetData.DismantledBy;
            var BuiltBy = GetData.BuiltBy == null ? "" : GetData.BuiltBy;
            var BuildDuringShutdown = GetData.BuildDuringShutdown == null ? "" : GetData.BuildDuringShutdown;
            IsSiteStatus = GetData.SiteLocationStatus == null ? "Active" : GetData.SiteLocationStatus;
            var IsMasterJob = GetData.IsMasterJob == null ? false : GetData.IsMasterJob;
            var CopiedScafRefNo = GetData.ShowCopiedRegisterID == null ? "" : GetData.ShowCopiedRegisterID;
            var RTIONotificationNumber = GetData.RTIONotificationNumber == null ? "" : GetData.RTIONotificationNumber;
            $('#Ownername').val(OwnerName);
            $('#txtdiscipline').val(discipline);
            if (MaintainInventory == "") {
                $('#checkboxMaintainInventory').prop('checked', false);

            }
            else {
                $('#checkboxMaintainInventory').prop('checked', true);
            }

            if (LongTermStructure == "") {
                $('#checkLTStructure').prop('checked', false);

            }
            else {
                $('#checkLTStructure').prop('checked', true);
            }
            if (NoInventory == "") {
                $('#chkNoInventory').prop('checked', false);

            }
            else {
                $('#chkNoInventory').prop('checked', true);
            }
            if (IsMasterJob == true) {
                $('#chkIsMasterJob').prop('checked', true);
            }
            else {
                $('#chkIsMasterJob').prop('checked', false);
            }

            if (DateTagged == "") {
                $('#PlannedDate').prop('disabled', false);
            }
            else {
                $('#PlannedDate').prop('disabled', true);
            }

            if (InventoryCount > 0) {
                $('#chkNoInventory').prop('disabled', true);
                InventoryFilled();
            }
            else if (DateOFDismantle != '') {
                $('#chkNoInventory').prop('disabled', true);
                DateDismantleFilled();
            }

            if (ChkRequiresIsolation == "") {
                $('#chkReqis').prop('checked', false);

            }
            else {
                $('#chkReqis').prop('checked', true);
            }

            //else {
            //    $('#chkNoInventory').prop('disabled', false);
            //}
            if (SiteLocID != "") {
                $('#ddlsites').val(SiteLocID);
            }

            if (NoOfWorkingDecks != "") {
                $('#DropDownList2').val(NoOfWorkingDecks);
            }
            if (EngineeredDesign != "") {
                $('#ddlEngineeredDesign').val(EngineeredDesign);
            } else {
                $('#ddlEngineeredDesign').val('NO');
            }
            if (DesignType != "") {
                // $('#ddlScaffoldDesign').val(DesignType);
                // $("#ddlScaffoldDesign option:contains(" + DesignType + ")").attr('selected', 'selected');
                // $('#ddlScaffoldDesign').find("option[value=" + DesignType + "]").attr('selected', 'selected');
                $('[id=ddlScaffoldDesign] option').filter(function () {
                    return ($(this).text() == DesignType);
                }).prop('selected', true);
            }

            if (DutyRating != "") {
                $('#ddlDutyrating').val(DutyRating);
            }
            if (MaterialType != "") {
                $('#ddlMaterialType').val(MaterialType);
            }
            if (OnlineApplied != "") {
                $('#chkOnline').val(OnlineApplied);
            }
            if (CheckListId != "") {
                $('#ddlChecklistInsp').val(CheckListId);
            }
            if (PlanningStatus != "") {
                $('#ddlPlanningStatus').val(PlanningStatus);
            } else {
                $('#ddlPlanningStatus').val('Planned');
            }
            if (ShiftAllocation != "") {
                $('#ddlShiftAllocation').val(ShiftAllocation);
            }
            if (InspTimeFrame != "") {
                $('#ddlInspectionTimeFrame').val(InspTimeFrame);
            }
            if (LockedIn == true) {
                $('#ddlLockedIn').val(1);
                $('#imgLocked').css('display', 'block');
                $('#txtlckedsignbynone').css('display', 'block');
            } else {
                $('#ddlLockedIn').val(0);
                $('#imgLocked').css('display', 'none');
                $('#txtlckedsignbynone').css('display', 'none');
            }
            if (LockedInSignedOff != "") {
                $('#ddlsignbysprvsr').val(LockedInSignedOff);
            }
            if (SupervisorId != "") {
                $('#ddlsupervisor').val(SupervisorId);
            }
            if (BuildDuringShutdown == "") {
                $('#checkBuildDuringShutdown').prop('checked', false);

            }
            else {
                $('#checkBuildDuringShutdown').prop('checked', true);
            }
            if (CopiedScafRefNo != "") {
                $('#txtcopiedscaffref').val(CopiedScafRefNo);
            }
            $('#txtSiteName').val(SiteLocation);
            $('#txtclientReference').val(ClientRef);
            $('#txtdesc').val(Description);
            $('#txtSiteLocation').val(Location);
            $('#txtassetsdesc').val(AssetDesc);
            $('#txtlength').val(Length);
            $('#txtwidth').val(Width);
            $('#txtheight').val(Height);
            $('#txtInspectedBy').val(InspectedBy);
            $('#txtRecentinspectiondate').val(RecentInspDate);
            $('#txtNoOfItem').val(NoOfItems);
            $('#txttonnage').val(Tonnage);
            $('#txtoptstatus').val(OptimizeStatus);
            $('#txtscaffref').val(ScafRefNo);
            $('#txtHireDays').val(HireDaysToDate);
            $('#txtTotalhireday').val(TotalHireDays);
            $('#txtDaylastinspetion').val(DaysSinceLastInsp);
            $('#txtstatus').text(Status);
            $('#txtdismantleby').val(DismantledBy);
            $('#txtBuiltBy').val(BuiltBy);
            if (PlanningStatus != null) {
                $('#txtPlannedStatus').val(PlanningStatus);
            } else {
                $('#txtPlannedStatus').val('Planned');
            }
            $('#txtWOnumb').val(WONumber);
            $('#txtClientScaff').val(ClientScaffref);
            $('#txtScaffType').val(ScafType);
            $('#txtplnstandDur').val(PlannedStandingDuration);
            $('#txtCrewSize').val(CrewSize);
            $('#txtManhrserect').val(ManHrsErect);
            $('#txtmanDisman').val(ManHrsDismantle);
            $('#txtCrew').val(Crew);
            $('#txtPermit').val(PermitNo);
            $('#txtcomments').val(Comments);
            $('#txtSapNumber').val(SAPNumber);
            $('#txtScaffoldOwner').val(ScaffoldOwner);
            $('#txtlckedsignby').val(LockedInSignedOffName);
            $('#txtDesignDrawingNo').val(DesignDrawingNo);
            $('#txtNoOfPlatform').val(NumberOfPlatform);
            $("#txtBearing").val(Bearing);
            $("#txtRTIONotificationNumber").val(RTIONotificationNumber);
            if (WorkGroupid != "") {
                $('#ddlWorkGroups').val(WorkGroupid);
            }
            if (SubGroupId != "") {
                $('#ddlSubGroups').val(SubGroupId);
            }
            $('#ddlRequestedBy').val(RequestedById);
            /* var WorkGroupid = GetData.WorkGroupid == null ? "" : GetData.WorkGroupid;*/
            /* var SubGroup = GetData.SubGroup == null ? "" : GetData.SubGroup;*/
            var RequestedBy = GetData.RequestedBy == null ? "" : GetData.RequestedBy;

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

            if (Status.toUpperCase() == "Red".toUpperCase() || Status.toUpperCase() == "Green".toUpperCase() || Status.toUpperCase() == "Yellow".toUpperCase()) {

                $('#txtstatus').css('display', 'none');
                $('#imgstaus').css('display', 'block');
                $('#imgstaus').attr("src", "/images/" + Status + ".jpg");
            }
            else {
                $('#txtstatus').css('display', 'block');
                $('#imgstaus').css('display', 'none');

            }


            $('#deleteimage').css('display', 'none');
            $('#btnImageSave').css('display', 'none');
            $('#downloadButton').css('display', 'none');


            if (Image != "") {
                $('#deleteimage').css('display', 'block');
                $('#btnImageSave').css('display', 'block');
                $('#downloadButton').css('display', 'block');

                // var uri = new Uri(databaseCon.siteimagelink);

                //if (dt.Rows[0]["Image"].ToString() == "") {
                if (Image == "") {
                    $('#imga').attr("src", "/images/noimg.jpg");
                    $('#getimage').val('');
                    $('#downloadButton').css('display', 'none');

                }
                else {
                    //imga.Attributes.Add("src", ("/images/sites/" + dt.Rows[0]["Image"].ToString()));
                    //getimage.Value = dt.Rows[0]["Image"].ToString();
                    //ViewState["deleteimage"] = dt.Rows[0]["Image"].ToString();
                    //ViewState["imgname"] = dt.Rows[0]["Image"].ToString();
                    $('#hdnimgname').val(Image);
                    $('#imga').attr("src", "/images/sites/" + Image + "");
                    $('#getimage').val(Image);
                }
            }
            else {
                $('#imga').attr("src", "/images/noimg.jpg");
                $('#getimage').val('');
            }

            if (PhotoAngle != "") {
                // imgangle.Value = dt.Rows[0]["PhotoAngle"].ToString();
                // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp1", "<script>ImageRotate(" + dt.Rows[0]["PhotoAngle"].ToString() + ");</script>", false);
                $('#imgangle').val(PhotoAngle);
                ImageRotate(PhotoAngle);
            }
            else {
                $('#imgangle').val("0");
            }

            // if (dt.Rows[0]["DismantleApprovedDate"] != DBNull.Value && Convert.ToString(dt.Rows[0]["DismantleApprovedDate"]) != "01/01/1900 12:00:00 AM") {
            //   rddateDisAppro.SelectedDate = Convert.ToDateTime(dt.Rows[0]["DismantleApprovedDate"]);
            if (DismantleApprovedDate != "") {
                $('#rddateDisAppro').val(DateFormat(DismantleApprovedDate));
            }
            // txtdiappronotes.Text = Convert.ToString(dt.Rows[0]["DismantleApprovedNotes"]);
            $('#txtdiappronotes').val(DismantleApprovedNotes);

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

            if (PipelineStartDate != "") {
                $('#PlannedDate').val(DateFormat(PipelineStartDate));
            }
            $('#txtPipelinenotes').val(PipeLineNote);
            if (Plans == "Plan1") {
                // if (txtdatetagged.Text != "")
                if (DateTagged != "") {
                    // rdpdatedis.Enabled = true;
                    $('#rdpdatedis').prop('disabled', false);
                }
                else {
                    // rdpdatedis.Enabled = false;
                    $('#rdpdatedis').prop('disabled', true);
                }
            } else if (Plans == "Plan2") {
                MaintainInvCheckBoxMessage();
                // if (dt.Rows[0]["DateOFDismantle"] == DBNull.Value && Convert.ToString(dt.Rows[0]["DateOFDismantle"]) != "01/01/1900 12:00:00 AM")

                if (DateOFDismantle == "") {
                    if (NoInventory == true) {
                        $('#rdpdatedis').prop('disabled', false);
                    }
                    else {
                        if (InventoryCount > 0) {
                            if (MaintainInventory == true)   // Change 10 July For MaintainInventory True 
                            {
                                //TabHideShow.TabHideShow(ClientID, RegisterID, userRole);  // Need To Add this Condition
                                if (OptimizeStatus == "Green") {
                                    if (AvailableQuantity == 0) {
                                        // if (dt.Rows[0]["DateTagged"] != DBNull.Value && Convert.ToString(dt.Rows[0]["DateTagged"]) != "01/01/1900 12:00:00 AM")
                                        if (DateTagged != "") {
                                            //rdpdatedis.Enabled = true;
                                            $('#rdpdatedis').prop('disabled', false);
                                        }
                                        else {
                                            // rdpdatedis.Enabled = false;
                                            $('#rdpdatedis').prop('disabled', true);
                                        }
                                    }
                                    else {
                                        //rdpdatedis.Enabled = false;
                                        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Signtemp", "<script>ReturnMessage();</script>", false);
                                        $('#rdpdatedis').prop('disabled', true);
                                        ReturnMessage();
                                    }

                                }
                                else {
                                    //rdpdatedis.Enabled = false;
                                    // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Signtemp", "<script>FinalizedMessage();</script>", false);
                                    $('#rdpdatedis').prop('disabled', true);
                                    FinalizedMessage();
                                }
                            }
                            else {
                                // TabHideShow.TabHideShow(ClientID, RegisterID, userRole);
                                // if (txtdatetagged.Text != "")
                                if (DateTagged != "") {
                                    //rdpdatedis.Enabled = true;
                                    $('#rdpdatedis').prop('disabled', false);
                                }
                                else {
                                    // rdpdatedis.Enabled = false;
                                    $('#rdpdatedis').prop('disabled', true);
                                }
                            }
                        }
                        else if (InventoryCount == 0 && MaintainInventory == true) {
                            if (ClientScaffref != "") {
                                //if (txtdatetagged.Text != "")
                                if (DateTagged != "") {
                                    // rdpdatedis.Enabled = true;
                                    $('#rdpdatedis').prop('disabled', false);
                                }
                                else {
                                    // rdpdatedis.Enabled = false;
                                    $('#rdpdatedis').prop('disabled', true);
                                }
                                // TabHideShow.TabHideShow(ClientID, RegisterID, userRole);  // Need To Add This Condition
                            } else {
                                // rdpdatedis.Enabled = false;
                                //  ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Signtemp", "<script>FinalizedMessage();</script>", false);
                                $('#rdpdatedis').prop('disabled', true);
                                FinalizedMessage();
                                //  TabHideShow.TabHideShow(ClientID, RegisterID, userRole);  // Need To Add This Condition
                            }
                        }
                        else {
                            // TabHideShow.TabHideShow(ClientID, RegisterID, userRole);
                            // if (txtdatetagged.Text != "")
                            if (DateTagged != "") {
                                // rdpdatedis.Enabled = true;
                                $('#rdpdatedis').prop('disabled', false);
                            }
                            else {
                                // rdpdatedis.Enabled = false;
                                $('#rdpdatedis').prop('disabled', true);
                            }
                        }
                    }
                }  // Date Of Dismantal
                else {
                    //checkboxMaintainInventory.Enabled = false;
                    $('#checkboxMaintainInventory').prop('disabled', true);
                    // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Signtemp", "<script>MaintainInvDateDismantle();;</script>", false);
                    MaintainInvDateDismantle();
                    if (MaintainInventory == true) {
                        //  TabHideShow.TabHideShow(ClientID, RegisterID, userRole);
                    }
                    else {
                        // TabHideShow.TabHideShow(ClientID, RegisterID, userRole);
                    }
                }
            }

            var id = getParameterByName('id');
            var RID = getParameterByName('RID');
            SiteChangesCondition(id, RID, SiteLocID, "");
            LocationDropdownBind();
            /* workgroupdropdown(SiteLocID);*/
            // Load WorkGroups dropdown AND apply selected value correctly
            workgroupdropdown(WorkGroupid);
            SubGroupDropdown(SubGroupId)
            RequestedbyDropdown(RequestedById)
            if (LocationId != "") {
                // ddlLocation_SelectedIndexChanged();
                $('#ddlLocation').val(LocationId);
                BuildingDropdownBind();
            }
            if (BuildingID != "") {
                $('#ddbuilding').val(BuildingID);
                //  ddbuilding_SelectedIndexChanged();
                AssetsDropdownBind();
            }
            if (AssetID != "") {
                $('#ddassets').val(AssetID);
                ddassets_SelectedIndexChanged();
            }
            FloorLevelDropdown();
            if (FloorLevelId != "") {
                $('#DropDownList1').val(FloorLevelId);
                FloorLabelMarksGet();
            }

            //if (FloorLevel != "") {
            //    $('#DropDownList1').val(FloorLevel);
            //    FloorLabelMarksGet();
            //}
            if (Lat != null && Long != null && Lat != "" && Long != "" && Lat != "0" && Long != "0") {
                geolocatewithparameter(Lat, Long);
            }

            QRCodeGenerate();

            if (IsOfflineCurrent == true) {
                var Response = { Result: "Warning", Message: "Please note this job is currently being updated OFFLINE so no changes will be allowed ONLINE." }
                CustomAlert(Response);
                $('#btnSave').css('display', 'none');
                $('#btnsaveBottom').css('display', 'none');
                $('#btnImageSave').css('display', 'none');
                $('#downloadButton').css('display', 'none');

                $('#hdnSaveIsOffline').val(IsOfflineCurrent);
            }
            else {
                IsOfflineCurrent = false;
                $('#hdnSaveIsOffline').val(IsOfflineCurrent);
            }


            // Hide Some Buttons  On After Page Load

            var UserRole = getParameterByName("UserRole");
            var DecriptValues = Decrypt([UserRole]);
            UserRole = DecriptValues[0];
            //if (UserRole==30 || UserRole==40 || UserRole == 32 || UserRole == 34) {
            //    $('#btnImageSave').css('display', 'none');
            //    $('#deleteimage').css('display', 'none');
            //    $('#gpsbutton').css('display', 'none');
            //    $('#gpsbutton1').css('display', 'none');
            //    $('#btnAddFloorLevelImage').css('display', 'none');
            //}//Commented on 30-Aug-2022 because add div on above button and create their insert on basis of user from database. Table name is Tblhideshow
            $('#MainDivJob').css('display', 'block');
            $('#LoadingPanel1').hide();
        }
    })

}
function QRCodeGenerate() {
    var id = getParameterByName('id');
    var RID = getParameterByName('RID');
    var scafRef = $('#txtscaffref').val();
    $.ajax({
        url: '/Register/QRCodeGenerate',
        async: false,
        data: { id: id, RID: RID, scafRef: scafRef },
        success: function (response) {

            var GetData = response;
            var ImageUrl = GetData.ImageUrl;

            $('#QRCodeImage').attr("src", ImageUrl);
            //  $('#QRCodeImage').empty().append(response);
        }
    });
}

function btnPre_Click() {
    $('#LoadingPanel1').show();
    var id = getParameterByName('id');
    var RID = getParameterByName('RID');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var CallFrom = getParameterByName('CallFrom');
    $.ajax({
        url: '/Register/btnPre_Click',
        async: false,
        data: { id: id, RID: RID, UID: UID, UserRole: UserRole },
        success: function (response) {

            var GetData = response;
            if (response.Result == "Success") {
                var RegisterID = response.Message;
                //if (RegisterID != "") {
                //    window.location.href = '/Register/Register?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RegisterID;
                //}

                if (RegisterID != "") {
                    var url = '/Register/Register?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RegisterID;
                    if (CallFrom) {
                        url += '&CallFrom=' + CallFrom;
                    }
                    window.location.href = url;
                }
            } else {
                CustomAlert(response);
                $('#LoadingPanel1').hide();
            }
        }
    });
}
function btnNext_Click() {
    $('#LoadingPanel1').show();
    var id = getParameterByName('id');
    var RID = getParameterByName('RID');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var CallFrom = getParameterByName('CallFrom');
    $.ajax({
        url: '/Register/btnNext_Click',
        async: false,
        data: { id: id, RID: RID, UID: UID, UserRole: UserRole },
        success: function (response) {
            if (response.Result == "Success") {
                var GetData = response;
                var RegisterID = response.Message;
                //if (RegisterID != "") {
                //    window.location.href = '/Register/Register?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RegisterID;
                //}
                if (RegisterID != "") {
                    var url = '/Register/Register?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RegisterID;
                    if (CallFrom) {
                        url += '&CallFrom=' + CallFrom;
                    }
                    window.location.href = url;
                }
            }
            else {
                CustomAlert(response);
                $('#LoadingPanel1').hide();
            }
        }
    });
}
function btnaddnew_Click() {
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var RID = "0";
    var EncryptData = [RID];
    var encryData = Encrypt(EncryptData);
    RID = encryData[0];

    window.location.href = '/Register/Register?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID;

    // Response.Redirect("Register.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RID=" + databaseCon.Encrypt("0"), false);
}
function btnCAncel_Click() {
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');

    window.location.href = '/RegisterList/RegisterList?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
}


//function btnSave_Click() {
//    checklatlong();

//    var site = $('#ddlsites').text();
//    var location = $('#ddlLocation').text();
//    var FloorLevel = $('#DropDownList1').text();
//    var planstanding = $("#txtplnstandDur").val();
//    if (site == "" || location == "" || planstanding == "" || FloorLevel == "") {
//        var response = { Result: "Failed", Message: "Please fill all required field then click on save." }
//        CustomAlert(response);
//    } else {

//    }

//}
function UpdateImagePath(RID) {
    //   $('#LoadingPanel1').show();

    var fileData = new FormData();
    var files;

    // Checking whether FormData is available in browser
    if (window.FormData !== undefined) {
        if (files == null || files == "") {
            var fileUpload = $('#radUpdogImg').get(0);
            files = fileUpload.files;
        }
        var fileUpload = $('#radUpdogImg').get(0);
        // Create FormData object
        //  var fileData = new FormData();
        if (files.length != 0) {

            var FileName = files[0].name;
            var FileSize = files[0].size;
            var extension = FileName.substring(FileName.lastIndexOf('.') + 1).toLowerCase();
            fileData.append(files[0].name, files[0]);

        }
    }

    var id = getParameterByName('id');
    var RID = RID;
    var txtOwnername = $('#txtOwnername').val();
    var tempimg = $('#tempimg').val();
    var basetempimg = $('#basetempimg').val();// For store base 64 image value also.
    var radUpdogImg = $('#radUpdogImg').val();

    var hdnimgname = $('#hdnimgname').val();

    fileData.append('id', id);

    fileData.append('RID', RID);

    fileData.append('txtOwnername', txtOwnername);

    fileData.append('radUpdogImg', radUpdogImg);

    fileData.append('imgname', hdnimgname);
    fileData.append('tempimg', tempimg);
    fileData.append('basetempimg', basetempimg);
    // fileData.append('image', image);
    $.ajax({
        url: "/Register/UpdateImagePath",
        type: 'POST',
        contentType: false, // Not to set any content header
        processData: false, // Not to process data
        data: fileData,
        success: function (response) {

            var Result = response;
            var EncryptData = [RID];
            var encryData = Encrypt(EncryptData);
            RID = encryData[0];
            var ClientID = getParameterByName('id');
            var UID = getParameterByName('UID');
            var UserRole = getParameterByName('UserRole');
            var Response = { Result: "Success", Message: "Data save successfully ." }
            CustomAlert(Response);

            window.location.href = '/Register/Register?UID=' + UID + '&UserRole=' + UserRole + '&id=' + ClientID + '&RID=' + RID;

        }
    });

}





//$(document).on("input change", "#rdpdatedis, #rdpDateTagged", function () {

//    var dateVal = $(this).val();
//    var currentId = $(this).attr("id");

//    if (!dateVal) {
//        if (currentId === "rdpdatedis") {
//            $("#txtdismantleby").val("");
//        }
//        else if (currentId === "rdpDateTagged") {
//            $("#txtBuiltBy").val("");
//        }
//        return;
//    }

//    var UID = getParameterByName('UID');

//    if (UID) {
//        $.ajax({
//            url: '/Register/GetUserEmailByUID',
//            type: 'GET',
//            data: { UID: UID },
//            success: function (response) {

//                if (response && response.name) {

//                    if (currentId === "rdpdatedis") {
//                        $("#txtdismantleby").val(response.name);
//                    }
//                    else if (currentId === "rdpDateTagged") {
//                        $("#txtBuiltBy").val(response.name);
//                    }

//                } else {
//                    alert("User name not found!");
//                }
//            },
//            error: function () {
//                alert("Error fetching user name!");
//            }
//        });
//    }

//});

//$(document).on("input change", " #rdpDateTagged", function () {

//    var dateVal = $(this).val();
//    var currentId = $(this).attr("id");

//    if (!dateVal) {

//        if (currentId === "rdpDateTagged") {
//            $("#txtBuiltBy").val("");
//        }
//        return;
//    }

//    var UID = getParameterByName('UID');

//    if (UID) {
//        $.ajax({
//            url: '/Register/GetUserEmailByUID',
//            type: 'GET',
//            data: { UID: UID },
//            success: function (response) {

//                if (response && response.name) {


//                    if (currentId === "rdpDateTagged") {
//                        $("#txtBuiltBy").val(response.name);
//                    }

//                } else {
//                    alert("User name not found!");
//                }
//            },
//            error: function () {
//                alert("Error fetching user name!");
//            }
//        });
//    }

//});

$(document).on("input change", "#rdpdatedis, #rdpDateTagged", function () {

    var dateVal = $(this).val();
    var currentId = $(this).attr("id");

    if (!dateVal) {
        if (currentId === "rdpdatedis") {
            $("#txtdismantleby").val("");
        }
        else if (currentId === "rdpDateTagged") {
            $("#txtBuiltBy").val("");
        }
        return;
    }

    var UID = getParameterByName('UID');

    if (UID) {
        $.ajax({
            url: '/Register/GetUserEmailByUID',
            type: 'GET',
            data: { UID: UID },
            success: function (response) {

                if (response && response.name) {

                    if (currentId === "rdpdatedis") {
                        $("#txtdismantleby").val(response.name);
                    }
                    else if (currentId === "rdpDateTagged") {
                        $("#txtBuiltBy").val(response.name);
                    }

                } else {
                    alert("User name not found!");
                }
            },
            error: function () {
                alert("Error fetching user name!");
            }
        });
    }

});


function btnSave_Click() {
    checklatlong();

    $('#LoadingPanel1').show();

    var RegisterId = getParameterByName('RID');
    var ClientID = getParameterByName('id');
    var DoneBy = getParameterByName('UID');

    var Description = $("#txtdesc").val();

    var InspectedBy = $("#txtInspectedBy").val();
    //var DesignType = $("#ddlScaffoldDesign").val();
    var DesignType = $("#ddlScaffoldDesign option:selected").text();
    var DutyRating = $("#ddlDutyrating").val();
    var MaterialType = $("#ddlMaterialType").val();
    var DateTagged = $("#rdpDateTagged").val();
    var DateOfDis = $("#rdpdatedis").val();

    var ExpectedStartDate = "";

    var DoneOn = new Date();

    var ClientRef = $("#txtclientReference").val();
    var InspTimeFrame = $("#ddlInspectionTimeFrame").val();
    var SiteLocId = $("#ddlsites").val();

    //var PhotoAngle = 0;
    var PhotoAngle = $("#imgangle").val();

    var BuildingID = $("#ddbuilding").val();
    var AssetId = $("#ddassets").val();
    var AssetDesc = $("#txtassetsdesc").val();
    var Length = $("#txtlength").val();
    var Height = $("#txtheight").val();
    var Width = $("#txtwidth").val();
    var NoOfWorkingDecks = $("#DropDownList2").val();
    var FloorLevel = $("#DropDownList1").val();
    var LocationId = $("#ddlLocation").val();
    var OnlineApplied = $("#chkOnline").val();
    var CheckListId = $("#ddlChecklistInsp").val();
    var PlanningStatus = $("#ddlPlanningStatus").val();
    var Lat = $("#hdnlatitude").val();
    var Long = $("#hdnlongitude").val();
    var LockedIn = $("#ddlLockedIn").val();
    var PlannedStandingDuration = $("#txtplnstandDur").val();
    var CrewSize = $("#txtCrewSize").val();
    var ManHrsErect = $("#txtManhrserect").val();
    var ManHrsDismantle = $("#txtmanDisman").val();
    var SupervisorId = $("#ddlsupervisor").val();
    var Crew = $("#txtCrew").val();
    var PermitNo = $("#txtPermit").val();
    var Comments = $("#txtcomments").val();
    var SAPNumber = $("#txtSapNumber").val();
    var ScaffoldOwner = $("#txtScaffoldOwner").val();
    var DismantleApprovedDate = $("#rddateDisAppro").val();
    var DismantleApprovedNotes = $("#txtdiappronotes").val();
    var Bearing = $("#txtBearing").val();

    //disscuss
    //var LockedInSignedOff = Session["Valuesuper"].ToString();
    var LockedInSignedOff = $('#ddlsignbysprvsr').val();

    var MaintainInventory = $("#checkboxMaintainInventory").prop('checked');
    var DesignDrawingNo = $("#txtDesignDrawingNo").val();
    var NumberOfPlatform = $("#txtNoOfPlatform").val();
    var EngineeredDesign = $("#ddlEngineeredDesign").val();
    var WONumber = $("#txtWOnumb").val();
    var ScafType = $("#txtScaffType").val();
    var discipline = $("#txtdiscipline").val();
    var ShiftAllocation = $("#ddlShiftAllocation").val();
    var txtClientScaff = $('#txtClientScaff').val();
    var ddlWorkGroups = $('#ddlWorkGroups').val();
    var ddlSubGroups = $('#ddlSubGroups').val();
    var txtRequestedBy = $('#ddlRequestedBy').val();
    var checkLTStructure = $('#checkLTStructure').prop('checked');
    var chkNoInventory = $('#chkNoInventory').prop('checked');
    var chkReqis = $('#chkReqis').prop('checked');
    var NoOfScafref = $("#NoOFScaf").val();
    var PipelineStartDate = $("#PlannedDate").val();
    var PipeLineNote = $("#txtPipelinenotes").val();
    var DismantledBy = $("#txtdismantleby").val();
    var BuiltBy = $("#txtBuiltBy").val();
    //Added on 19-12-2023 to add it in current date to compare the Date Tagged and Current Date.
    var hdnTimeOffSet = $('#hdnTimeOffSetForAllPage').val();
    var BuildDuringShutdown = $('#checkBuildDuringShutdown').prop('checked');
    var IsMasterJob = $('#chkIsMasterJob').prop('checked');
    var RTIONotificationNumber = $("#txtRTIONotificationNumber").val();
    if (ddlWorkGroups == undefined || ddlWorkGroups == null) {
        ddlWorkGroups = "";
    }
    if (ddlSubGroups == undefined || ddlSubGroups == null) {
        ddlSubGroups = "";
    }

    if (ddlWorkGroups == "" && ddlSubGroups != "") {
        var Response = { Result: "Failed", Message: "Please fill the Work Group first before the Sub Group." };
        CustomAlert(Response);
        $('#ddlWorkGroups').addClass('requiredColumn');
        $('#LoadingPanel1').hide();
        return false;
    }
    var arr = [];
    arr = { RegisterId: RegisterId, Description: Description, ClientID: ClientID, InspectedBy: InspectedBy, DesignType: DesignType, DutyRating: DutyRating, MaterialType: MaterialType, DateTagged: DateTagged, DateOfDis: DateOfDis, ExpectedStartDate: ExpectedStartDate, DoneBy: DoneBy, DoneOn: DoneOn, ClientRef: ClientRef, InspTimeFrame: InspTimeFrame, SiteLocId: SiteLocId, PhotoAngle: PhotoAngle, BuildingID: BuildingID, AssetId: AssetId, AssetDesc: AssetDesc, Length: Length, Height: Height, Width: Width, NoOfWorkingDecks: NoOfWorkingDecks, FloorLevel: FloorLevel, LocationId: LocationId, OnlineApplied: OnlineApplied, CheckListId: CheckListId, PlanningStatus: PlanningStatus, Lat: Lat, Long: Long, LockedIn: LockedIn, PlannedStandingDuration: PlannedStandingDuration, CrewSize: CrewSize, ManHrsErect: ManHrsErect, ManHrsDismantle: ManHrsDismantle, SupervisorId: SupervisorId, Crew: Crew, PermitNo: PermitNo, Comments: Comments, SAPNumber: SAPNumber, ScaffoldOwner: ScaffoldOwner, DismantleApprovedDate: DismantleApprovedDate, DismantleApprovedNotes: DismantleApprovedNotes, Bearing: Bearing, LockedInSignedOff: LockedInSignedOff, MaintainInventory: MaintainInventory, DesignDrawingNo: DesignDrawingNo, NumberOfPlatform: NumberOfPlatform, EngineeredDesign: EngineeredDesign, WONumber: WONumber, ScafType: ScafType, discipline: discipline, ShiftAllocation: ShiftAllocation, txtClientScaff: txtClientScaff, ddlWorkGroups: ddlWorkGroups, ddlSubGroups: ddlSubGroups, txtRequestedBy: txtRequestedBy, checkLTStructure: checkLTStructure, chkNoInventory: chkNoInventory, NoOfScafref: NoOfScafref, PipelineStartDate: PipelineStartDate, PipeLineNote: PipeLineNote, hdnTimeOffSet: hdnTimeOffSet, chkReqis: chkReqis, DismantledBy: DismantledBy, BuiltBy: BuiltBy, BuildDuringShutdown: BuildDuringShutdown, IsMasterJob: IsMasterJob, RTIONotificationNumber: RTIONotificationNumber };

    //fileData.append('model', arr);

    var site = $('#ddlsites').val();
    var location = $('#ddlLocation').val();
    var FloorLevel = $('#DropDownList1').val();
    var planstanding = $("#txtplnstandDur").val();
    if (site == "" || site == null || site == undefined || location == "" || location == null || location == undefined) {
        var response = {
            Result: "Failed", Message: "YOU MUST POPULATE ‘SITE’ AND ‘LOCATIONS’ BEFORE YOU CAN GENERATE AND SAVE SCAF REF #’S."
        }
        CustomAlert(response);
        $('#LoadingPanel1').hide();
    }
    else if (planstanding == "" || FloorLevel == "" || FloorLevel == null || FloorLevel == undefined) {
        var response = { Result: "Failed", Message: "Please fill ‘FloorLevel’ And ‘planstanding’ required field then click on save." }
        CustomAlert(response);
        $('#LoadingPanel1').hide();
    } else {

        $.ajax({
            url: '/Register/btnSave_Click',
            data: { model: arr },
            //data: fileData,
            async: true,
            type: "POST",
            success: function (response) {
                if (response.Result == "Success") {
                    // var ClientID = getParameterByName('id');
                    // var UID = getParameterByName('UID');
                    // var UserRole = getParameterByName('UserRole');

                    var RID = response.RegisterID;
                    UpdateImagePath(RID);
                    // var EncryptData = [RID];
                    // var encryData = Encrypt(EncryptData);
                    // RID = encryData[0];
                    // var Response = { Result: "Success", Message: "Data save successfully ." }
                    // CustomAlert(Response);
                    // setTimeout(function () {
                    //     window.location.href = '/Register/Register?UID=' + UID + '&UserRole=' + UserRole + '&id=' + ClientID + '&RID=' + RID;
                    // }, 400);

                } else {
                    $('#LoadingPanel1').hide();
                    CustomAlert(response);
                }

            }
        });

    }

}

function ddlsites_SelectedIndexChanged() {
    var id = getParameterByName('id');
    var RID = getParameterByName('RID');
    var ddlsites = $('#ddlsites').val();
    /*LocationDropdownBind();*/
    SiteChangesCondition(id, RID, "", ddlsites);

    LocationDropdownBind();
    workgroupdropdown();
    SubGroupDropdown();
    RequestedbyDropdown();
}
//function ddlsites_SelectedIndexChanged() {
//    LocationDropdownBind();
//    workgroupdropdown();
//    SubGroupDropdown();
//    RequestedbyDropdown();

//}
function ddlWorkGroup_SelectedIndexChanged() {
    SubGroupDropdown();
}
function workgroupdropdown(selectedWorkGroupId) {
    var id = $("#hdnClientID").val();
    var sitelocid = $("#ddlsites").val();
    $.ajax({
        url: '/Register/getworkgroupdropdown',
        async: false,
        data: { id: id, sitelocid: sitelocid },
        success: function (response) {

            data = response;
            var dropdownWorkGroup = "<option value=''></option>";
            $.each(response.dropdownWorkGroup, function (idx, obj) {
                if (obj != null) {
                    dropdownWorkGroup += '<option value="' + obj.WorkGroupID + '">' + obj.WorkGroup + '</option>';
                }
            });
            $('#ddlWorkGroups').empty().append(dropdownWorkGroup);

            if (selectedWorkGroupId != null && selectedWorkGroupId != "") {
                $('#ddlWorkGroups').val(String(selectedWorkGroupId).trim());
            }
        }
    });
}
function RequestedbyDropdown(selectedRequestedById) {
    var id = $("#hdnClientID").val();
    var sitelocid = $("#ddlsites").val();
    $.ajax({
        url: '/Register/getRequestedbydropdown',
        async: false,
        data: { id: id, sitelocid: sitelocid },
        success: function (response) {

            data = response;
            var dropdownRequestedby = "<option value=''></option>";
            $.each(response.dropdownRequestedby, function (idx, obj) {
                if (obj != null) {
                    dropdownRequestedby += '<option value="' + obj.RequestById + '">' + obj.RequestBy + '</option>';
                }
            });
            $('#ddlRequestedBy').empty().append(dropdownRequestedby);

            if (selectedRequestedById != null && selectedRequestedById != "") {
                $('#ddlRequestedBy').val(String(selectedRequestedById).trim());
            }
        }
    });
}
function SubGroupDropdown(SelectedSubGroupid) {
    var id = $("#hdnClientID").val();
    var sitelocid = $("#ddlsites").val();
    var WorkGroupid = $("#ddlWorkGroups").val() || 0;

    if (!WorkGroupid) {
        $('#ddlSubGroups')
            .empty()
            .append("<option value=''></option>");
        return;
    }
    $.ajax({
        url: '/Register/getSubgroupdropdown',
        async: false,
        data: { id: id, sitelocid: sitelocid, WorkGroupid: WorkGroupid },
        success: function (response) {

            data = response;
            var dropdownSubGroup = "<option value=''></option>";

            if (response.dropdownSubGroup && response.dropdownSubGroup.length === 1) {
                var obj = response.dropdownSubGroup[0];
                dropdownSubGroup += '<option value="' + obj.SubGroupId + '" selected>' + obj.SubGroup + '</option>';

            } else {
                $.each(response.dropdownSubGroup, function (idx, obj) {
                    if (obj != null) {
                        dropdownSubGroup += '<option value="' + obj.SubGroupId + '">' + obj.SubGroup + '</option>';
                    }
                });
            }

            $('#ddlSubGroups').empty().append(dropdownSubGroup);
            if (SelectedSubGroupid != null && SelectedSubGroupid !== "") {
                $('#ddlSubGroups').val(String(SelectedSubGroupid).trim());
            }
            $('#ddlSubGroups').trigger('change');
        }

    });
}

function SiteChangesCondition(id, RID, SiteID, ddlsites) {
    $.ajax({
        url: '/Register/GetSiteSelectIndexChangeData',
        async: false,
        data: { id: id, SiteID: SiteID, RID: RID, ddlsites: ddlsites },
        success: function (response) {

            data = response;

            var GetData = response.GetAllSiteData;

            var GPS = GetData.GPS;
            var FloorLevel = GetData.FloorLevel;
            var EnabledGPSSitePin = GetData.EnabledGPSSitePin;
            var EnabledCaptureGps = GetData.EnabledCaptureGps;
            var XPosition = GetData.XPosition;
            var YPosition = GetData.YPosition;
            var Sitelocid = GetData.Sitelocid;
            var LocationId = GetData.LocationId;
            var BuildingID = GetData.BuildingID;
            var FloorLevelID = GetData.FloorLevelID;
            var TotalRecord = GetData.TotalRecord;
            var Address = GetData.Address == null ? "" : GetData.Address;
            var city = GetData.city == null ? "" : GetData.city;
            var state = GetData.state == null ? "" : GetData.state;
            var OwnerName = GetData.OwnerName == null ? "" : GetData.OwnerName;
            var Zip = GetData.Zip == null ? "" : GetData.Zip;
            var MaintainInventory = GetData.MaintainInventory;
            var MaintainInventoryAtJob = GetData.MaintainInventoryAtJob;
            var Lat = GetData.Lat;
            var Long = GetData.Long;
            var AssetId = GetData.AssetId == null ? "" : GetData.AssetId;
            var AssetDesc = GetData.AssetDesc == null ? "" : GetData.AssetDesc;
            var SkipMaterial = GetData.SkipMaterial;

            // var ddlFloorLebel = "<option value=''></option>";
            // var ddlFloorLebel = "";
            // $.each(response.GetFloorLebelData, function (idx, obj) {
            //     if (obj != null) {
            //         // ddlFloorLebel += '<option value="' + obj.FloorLevel + '">' + obj.FloorLevel + '</option>';
            //         ddlFloorLebel += '<option value="' + obj.FloorLevelId + '">' + obj.FloorLevel + '</option>';
            //     }
            // });

            // //var ddlLocation = "<option value=''></option>";
            // var ddlLocation = "";
            // $.each(response.GetLocationData, function (idx, obj) {
            //     if (obj != null) {
            //         ddlLocation += '<option value="' + obj.areaid + '">' + obj.area + '</option>';
            //     }
            // });

            //// $('#DropDownList1').empty().append(ddlFloorLebel);
            // $('#ddlLocation').empty().append(ddlLocation);



            // if (TotalRecord == 0) {
            if ((TotalRecord == 0) || (TotalRecord != 0 && SiteID == Sitelocid)) {

                if (GPS == true) {
                    //if ((Lat == "" || Lat == null) && (Long == "" || Long == null)) {
                    if (((Lat == "" || Lat == null) && (Long == "" || Long == null)) || (((Lat != "" || Lat != null) && (Long != "" || Long != null)) && (SiteID == Sitelocid))) {
                        if (EnabledGPSSitePin == true) {
                            $('#gpsbutton1').css("display", "block");
                        } else {
                            $('#gpsbutton1').css("display", "none");
                        }
                        if (EnabledCaptureGps == true) {
                            $('#gpsbutton').css("display", "block");
                        } else {
                            $('#gpsbutton').css("display", "none");
                        }
                        if (SkipMaterial == true) {
                            $('#NoInventoryReq').css("display", "block");
                        } else {
                            $('#NoInventoryReq').css("display", "none");
                        }

                        $('#divbtnZoomfloor').css("display", "none");
                        $('#btnAddFloorLevelImage').css("display", "none");
                        $('#divCanvasFloorImage').css("display", "none");
                        $('#FloorImage_hidden').css("display", "none");
                        $('#divimagetext').css("display", "none");
                        $('#divbtnflrimg').css("display", "none");
                        $('#FloorLevelImage').css("display", "none");
                        $('#divscafoldgps').css("display", "block");
                    } else {
                        if (Sitelocid != null) {
                            $('#ddlsites').val(Sitelocid);
                        }
                        var Response = { Result: "Failed", Message: "The Site cannot be changed as Gps Map has been checked for this site." }
                        CustomAlert(Response);
                    }
                }
                if (FloorLevel == true) {
                    // if ((XPosition == "" || XPosition == null) && (YPosition == "" || YPosition == null)) {
                    if (((XPosition == "" || XPosition == null) && (YPosition == "" || YPosition == null)) || (((XPosition != "" || XPosition != null) && (YPosition != "" || YPosition != null)) && (SiteID == Sitelocid))) {
                        $('#divscafoldgps').css("display", "none");
                        $('#map_populate_hidden').css("display", "none");
                        $('#btnAddFloorLevelImage').css("display", "block");
                        FloorLabelMarksGet();
                    }
                    else {
                        if (Sitelocid != null) {
                            $('#ddlsites').val(Sitelocid);
                        }
                        var Response = { Result: "Failed", Message: "The Site cannot be changed as Floor Lavel Map has been checked for this site." }
                        CustomAlert(Response);
                        // $('#DropDownList1').val(FloorLevel);
                    }
                }

            } else {
                if (Sitelocid != null) {
                    $('#ddlsites').val(Sitelocid);
                }
                var Response = { Result: "Failed", Message: "The Site cannot be changed as Material has already been requested for this site." }
                CustomAlert(Response);
            }


            $('#txtsiteAddress').val(Address);
            $('#txtsiteCity').val(city);
            $('#MapState').val(state);
            $('#txtOwnername').val(OwnerName);
            $('#txtZip').val(Zip);
            //if (Sitelocid != null) {
            //    $('#ddlsites').val(Sitelocid);
            //}
            LocationDropdownBind();
            if (LocationId != null) {

                $('#ddlLocation').val(LocationId);
            }
            BuildingDropdownBind();
            if (BuildingID != "") {
                $('#ddbuilding').val(BuildingID);
            }
            AssetsDropdownBind();
            if (AssetId != "") {
                $('#ddassets').val(AssetId);
                $('#txtassetsdesc').val(AssetDesc);
            }
            ddassets_SelectedIndexChanged();
            FloorLevelDropdown();
            if (FloorLevelID != "") {
                $('#DropDownList1').val(FloorLevelID);
            }
            if (SkipMaterial == true) {
                $('#NoInventoryReq').css("display", "block");
            }
            else {
                $('#NoInventoryReq').css("display", "none");
            }


            if (MaintainInventoryAtJob == true) {
                // checkboxMaintainInventory.Checked = Convert.ToBoolean(dtSiteDetails.Rows[0]["MaintainInventoryAtJob"]);
                $('#checkboxMaintainInventory').prop('checked', true);

            }
            else {
                // checkboxMaintainInventory.Checked = false;
                $('#checkboxMaintainInventory').prop('checked', false);
            }

            // ddlLocation_SelectedIndexChanged();

        }
    });
}

function ddlLocation_SelectedIndexChanged() {
    //var id = getParameterByName('id');
    //var LocationID = $('#ddlLocation').val();
    //$.ajax({
    //    url: '/Register/BuildingDropdownBind',
    //    async: false,
    //    data: { id: id, LocationID: LocationID },
    //    success: function (response) {

    //        var GetData = response;
    //        var ddlBld = "<option value=''></option>";
    //        $.each(response, function (idx, obj) {
    //            if (obj != null) {
    //                ddlBld += '<option value="' + obj.buildingid + '">' + obj.buildingno + '</option>';
    //            }

    //        });
    //        $('#ddbuilding').empty().append(ddlBld);
    //    }
    //});

    var id = getParameterByName('id');
    var RID = getParameterByName('RID');
    var LocationID = $('#ddlLocation').val();
    var SiteID = $('#ddbSites').val();
    $.ajax({
        url: '/Register/ddlLocation_SelectedIndexChanged',
        async: false,
        data: { id: id, SiteID: SiteID, LocationID: LocationID, RID: RID },
        success: function (response) {

            var LocationFlag = response.Result;
            var GetData = response.LocationData;

            var Sitelocid = GetData.Sitelocid;
            var LocationId = GetData.LocationId;
            var BuildingID = GetData.BuildingID;
            var FloorLevelID = GetData.FloorLevelID;
            var AssetId = GetData.AssetId == null ? "" : GetData.AssetId;
            var AssetDesc = GetData.AssetDesc == null ? "" : GetData.AssetDesc;

            if (LocationFlag == "True") {
                LocationDropdownBind();
                if (LocationId != null) {

                    $('#ddlLocation').val(LocationId);
                }
                BuildingDropdownBind();
                if (BuildingID != "") {
                    $('#ddbuilding').val(BuildingID);
                }
                AssetsDropdownBind();
                if (AssetId != "") {
                    $('#ddassets').val(AssetId);
                    $('#txtassetsdesc').val(AssetDesc);
                }
                ddassets_SelectedIndexChanged();
                FloorLevelDropdown();
                if (FloorLevelID != "") {
                    $('#DropDownList1').val(FloorLevelID);
                }
                //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
                //art1.Style.Add("display", "block");
                //alertlabel.Text = "<strong>Alert ! </strong> The Location cannot be changed as Floor Level Map has been checked for this location.";
                var Response = { Result: "Failed", Message: "The Location cannot be changed as Floor Level Map has been checked for this location." };
                CustomAlert(Response);
            } else {
                // LocationDropdownBind();
                BuildingDropdownBind();
                AssetsDropdownBind();
                ddassets_SelectedIndexChanged();
                FloorLevelDropdown();
                DropDownList1_SelectedIndexChanged(true);
            }
        }
    });

    // BuildingDropdownBind();
    //// ddbuilding_SelectedIndexChanged();
    // FloorLevelDropdown();
    // DropDownList1_SelectedIndexChanged(true);
}
function ddbuilding_SelectedIndexChanged() {
    //var id = getParameterByName('id');
    //var BuildingID = $('#ddbuilding').val();
    //$.ajax({
    //    url: '/Register/AssetsDropdownBind',
    //    async: false,
    //    data: { id: id, BuildingID: BuildingID },
    //    success: function (response) {

    //        var GetData = response;
    //        var ddlassest = "<option value=''></option>";
    //        $.each(response, function (idx, obj) {
    //            if (obj != null) {
    //                ddlassest += '<option value="' + obj.Assetid + '">' + obj.Assetno + '</option>';
    //            }

    //        });
    //        $('#ddassets').empty().append(ddlassest);
    //    }

    //});
    // ddassets_SelectedIndexChanged();

    var id = getParameterByName('id');
    var RID = getParameterByName('RID');
    var LocationID = $('#ddlLocation').val();
    var BuildingID = $('#ddbuilding').val();
    var SiteID = $('#ddbSites').val();
    $.ajax({
        url: '/Register/ddbuilding_SelectedIndexChanged',
        async: false,
        data: { id: id, SiteID: SiteID, LocationID: LocationID, BuildingID: BuildingID, RID: RID },
        success: function (response) {
            var BuildingFlag = response.Result;
            var GetData = response.BuildingData;

            var Sitelocid = GetData.Sitelocid;
            var LocationId = GetData.LocationId;
            var BuildingID = GetData.BuildingID;
            var FloorLevelID = GetData.FloorLevelID;
            var AssetId = GetData.AssetId == null ? "" : GetData.AssetId;
            var AssetDesc = GetData.AssetDesc == null ? "" : GetData.AssetDesc;

            if (BuildingFlag == "True") {
                //LocationDropdownBind();
                //if (LocationId != null) {
                //  $('#ddlLocation').val(LocationId);
                //}
                BuildingDropdownBind();
                if (BuildingID != "") {
                    $('#ddbuilding').val(BuildingID);
                }
                AssetsDropdownBind();
                if (AssetId != "") {
                    $('#ddassets').val(AssetId);
                    $('#txtassetsdesc').val(AssetDesc);
                }
                ddassets_SelectedIndexChanged();
                FloorLevelDropdown();
                if (FloorLevelID != "") {
                    $('#DropDownList1').val(FloorLevelID);
                }
                var Response = { Result: "Failed", Message: "The Building / Module No. cannot be changed as Floor Level Map has been checked for this building or location." };
                CustomAlert(Response);
            } else {
                // BuildingDropdownBind();
                AssetsDropdownBind();
                ddassets_SelectedIndexChanged();
                FloorLevelDropdown();
                DropDownList1_SelectedIndexChanged(true);
            }
        }
    });

    //AssetsDropdownBind();
    //FloorLevelDropdown();

}
function ddassets_SelectedIndexChanged() {
    var id = getParameterByName('id');
    var assestID = $('#ddassets').val();
    $.ajax({
        url: '/Register/ddassets_SelectedIndexChanged',
        async: false,
        data: { id: id, assestID: assestID },
        success: function (response) {

            var GetData = response;
            $('#txtassetsdesc').val(GetData);
        }
    });
}
function DropDownList1_SelectedIndexChanged(flagValues) {
    var id = getParameterByName('id');
    var RID = getParameterByName('RID');
    $.ajax({
        url: '/Register/DropDownList1_SelectedIndexChanged',
        async: false,
        data: { id: id, RID: RID },
        success: function (response) {
            if (response != "" || response != null) {
                var GetData = response.split('_');
                var XPosition = GetData[0];
                var YPosition = GetData[1];

                if (flagValues == true) {
                    if ((XPosition != "" || YPosition != "") && (flagValues != true)) {
                        // if ((XPosition != "" || YPosition != "") && (flag == undefined)){
                        // if (((XPosition != "" || YPosition != "") && (flag != null)) ||((XPosition != "" || YPosition != "") && (flag == undefined))) {
                        // divConformationforSiteFloor.Style.Add("display", "block");
                        // bllMessage.InnerText = "Are you sure you want to Change Floor Level ? If You change The Floor Level Your Floor Map Data Will be Lost.";
                        $('#divConformationforSiteFloor').css('display', 'block');
                        $('#bllMessage').text('Are you sure you want to Change Floor Level ? If You change The Floor Level Your Floor Map Data Will be Lost.');
                    }
                    else {
                        FloorLabelMarksGet();
                    }
                } else {
                    if ((XPosition != "" || YPosition != "")) {
                        // if ((XPosition != "" || YPosition != "") && (flag == undefined)){
                        // if (((XPosition != "" || YPosition != "") && (flag != null)) ||((XPosition != "" || YPosition != "") && (flag == undefined))) {
                        // divConformationforSiteFloor.Style.Add("display", "block");
                        // bllMessage.InnerText = "Are you sure you want to Change Floor Level ? If You change The Floor Level Your Floor Map Data Will be Lost.";
                        $('#divConformationforSiteFloor').css('display', 'block');
                        $('#bllMessage').text('Are you sure you want to Change Floor Level ? If You change The Floor Level Your Floor Map Data Will be Lost.');
                    }
                    else {
                        FloorLabelMarksGet();
                    }
                }
                // // if (XPosition != "" || YPosition != "") {
                // // if ((XPosition != "" || YPosition != "") && (flag != null || flag == undefined) || (XPosition != "" || YPosition != "") && (flag == undefined)) {
                // if ((XPosition != "" || YPosition != "") && (flag != null)){
                // // if ((XPosition != "" || YPosition != "") && (flag == undefined)){
                //// if (((XPosition != "" || YPosition != "") && (flag != null)) ||((XPosition != "" || YPosition != "") && (flag == undefined))) {
                //    // divConformationforSiteFloor.Style.Add("display", "block");
                //    // bllMessage.InnerText = "Are you sure you want to Change Floor Level ? If You change The Floor Level Your Floor Map Data Will be Lost.";
                //    $('#divConformationforSiteFloor').css('display', 'block');
                //    $('#bllMessage').text('Are you sure you want to Change Floor Level ? If You change The Floor Level Your Floor Map Data Will be Lost.');
                // }
                // else {
                //    FloorLabelMarksGet();
                //}
            }
        }
    });
}

//protected void btnSiteFloorLevelChangeYes_Click(object sender, EventArgs e)
//{
//      string query = "update tblregister set XPosition='',YPosition='' where registerID='" + databaseCon.Decrypt(Request.QueryString["RID"]) + "'";
//int i = databaseCon.RunCmd(query);
//divConformationforSiteFloor.Style.Add("display", "none");
//ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp63", "<script>FloorLabelMarksGet();</script>", false);
//}

function btnSiteFloorLevelChangeYes_Click() {
    var RID = getParameterByName('RID');
    $.ajax({
        url: '/Register/btnSiteFloorLevelChangeYes_Click',
        async: false,
        data: { RID: RID },
        success: function (response) {
            if (response.Result == "Success")
                $('#divConformationforSiteFloor').css('display', 'none');
            FloorLabelMarksGet();
        }

    });
}

function btnSiteFloorLevelChangeNo_Click() {
    var RID = getParameterByName('RID');
    $.ajax({
        url: '/Register/btnSiteFloorLevelChangeNo_Click',
        async: false,
        data: { RID: RID },
        success: function (response) {
            if (response != "") {
                $('#DropDownList1').val(response);
                FloorLabelMarksGet();
            }

            $('#divConformationforSiteFloor').css('display', 'none');

        }

    });

}


function ddlLockedIn_SelectedIndexChanged() {
    $('#PopLocked').css('display', 'none');
    //if (ddlLockedIn.SelectedValue == "1")
    if ($('#ddlLockedIn').val() == "1") {
        // ddlLockedIn.SelectedValue = "1";
        // imgLocked.Visible = true;

        //$('#ddlLockedIn').val('1');
        //$('#imgLocked').css('display','block');

        ////start-7-mar-2019
        //string querystr = "select DesignType,MaterialType,ManHrsErect,ManHrsDismantle,HierarchySignedOff  from tblRegister where RegisterId='" + databaseCon.Decrypt(Request.QueryString["RID"]) + "'";

        //DataTable dt1 = databaseCon.GetData(querystr);
        //string str = "";
        //string str1 = "";
        //string accschcs = "";
        //if (dt1.Rows.Count > 0)
        //{
        //    //if (ddlScaffoldDesign.SelectedValue == "")
        //    if (dt1.Rows[0]["DesignType"].ToString() == "")
        //    {
        //        str += "Design Type, ";
        //        str1 += "Design Type" + ",";
        //    }
        //    if (dt1.Rows[0]["MaterialType"].ToString() == "")
        //    {
        //        str += "Material Type, ";
        //        str1 += "Material Type" + ",";
        //    }
        //    if (dt1.Rows[0]["ManHrsErect"].ToString() == "")
        //    {
        //        str += "Man Hrs Erect, ";
        //        str1 += "Man Hrs Erect" + ",";
        //    }
        //    if (dt1.Rows[0]["ManHrsDismantle"].ToString() == "")
        //    {
        //        str += "Man Hrs Dismantle";
        //        str1 += "Man Hrs Dismantle";
        //    }
        //    accschcs = dt1.Rows[0]["HierarchySignedOff"].ToString();
        //    //if (accschcs == "")
        //    //{
        //    //    str += "Set access choice on next tab)";
        //    //}
        //}



        var str = "";
        var str1 = "";
        var accschcs = "";
        var id = getParameterByName('id');
        var RID = getParameterByName('RID');
        $.ajax({
            type: "POST",
            url: "/Register/GetLockDownSignOffData",
            data: JSON.stringify({ id: id, RID: RID }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                var GetData = response;

                var DesignType = response.DesignType == null ? "" : response.DesignType;
                var MaterialType = response.MaterialType == null ? "" : response.MaterialType;
                var ManHrsErect = response.ManHrsErect == null ? "" : response.ManHrsErect;
                var ManHrsDismantle = response.ManHrsDismantle == null ? "" : response.ManHrsDismantle;
                var HierarchySignedOff = response.HierarchySignedOff == null ? "" : response.HierarchySignedOff;

                if (DesignType == "") {
                    str += "Design Type, ";
                    str1 += "Design Type" + ",";
                }
                if (MaterialType == "") {
                    str += "Material Type, ";
                    str1 += "Material Type" + ",";
                }
                if (ManHrsErect == "") {
                    str += "Man Hrs Erect, ";
                    str1 += "Man Hrs Erect" + ",";
                }
                if (ManHrsDismantle == "") {
                    str += "Man Hrs Dismantle";
                    str1 += "Man Hrs Dismantle";
                }
                accschcs = HierarchySignedOff;


                if (str != "" || accschcs == "") {

                    //ddlLockedIn.SelectedValue = "0";
                    //imgLocked.Visible = false;

                    // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp", "<script>borderchange('" + str1 + "');</script>", false);
                    // str = "All Data Entry (" + str + "), including 'Hierarchy of Access Choice', must be completed and 'Saved' before job can be 'Locked In'.";
                    //PopLocked.Style.Add("display", "none");

                    $('#ddlLockedIn').val('0');
                    $('#imgLocked').css('display', 'none');
                    borderchange(str1);
                    str = "All Data Entry (" + str + "), including 'Hierarchy of Access Choice', must be completed and 'Saved' before job can be 'Locked In'.";

                    //$('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
                    //$('#art1').css("display", "block");
                    //$('#alertlabel').html('<strong>Info ! </strong>' + str);
                    var Response = { Result: "Failed", Message: str };
                    CustomAlert(Response);
                }

                else {
                    $('#PopLocked').css('display', 'block');
                }

            }
        });



        //End-7-mar-2019
    }
    //else if (ddlLockedIn.SelectedValue == "0")
    else if ($('#ddlLockedIn').val() == "0") {
        $('#ddlLockedIn').val('0');
        $('#imgLocked').css('display', 'none');
        $('#txtlckedsignbynone').css('display', 'none');
        //ddlLockedIn.SelectedValue = "0";
        //imgLocked.Visible = false;
        //txtlckedsignbynone.Style.Add("display", "none");
    }
}

function btnlockedyes_Click() {
    //  if (ddlLockedIn.SelectedValue == "1")
    if ($('#ddlLockedIn').val() == "1") {
        //ddlLockedIn.SelectedValue = "1";
        //imgLocked.Visible = true;
        $('#ddlLockedIn').val('1');
        $('#imgLocked').css('display', 'block');
    }

    //start-7-mar-2019
    // if (ddlsignbysprvsr.SelectedValue == "")
    if ($('#ddlsignbysprvsr').val() == "" || $('#ddlsignbysprvsr').val() == null || $('#ddlsignbysprvsr').val() == undefined) {
        // ddlLockedIn.SelectedValue = "0";
        // imgLocked.Visible = false;
        $('#ddlLockedIn').val('0');
        $('#imgLocked').css('display', 'none');
        //art1.Style.Add("display", "block");
        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
        //alertlabel.Text = "<strong> Info ! </strong> Please fill the Signed Off By.  ";
        var Response = { Result: "Failed", Message: "Please fill the Signed Off By." };
        CustomAlert(Response);
    }
    if (ddlsignbysprvsr.SelectedValue != "") {
        //txtlckedsignby.Text = ddlsignbysprvsr.SelectedItem.Text; //7-Mar-2019
        //Session["Valuesuper"] = ddlsignbysprvsr.SelectedValue;
        //txtlckedsignbynone.Style.Add("display", "block");
        //PopLocked.Style.Add("display", "none");

        $('#txtlckedsignby').val($("#ddlsignbysprvsr option:selected").text());
        $('#txtlckedsignbynone').css('display', 'block');
        $('#PopLocked').css('display', 'none');
    }
    //end-7-mar-2019
}

function btnlockedno_Click() {
    // if (ddlLockedIn.SelectedValue == "1")
    if ($('#ddlLockedIn').val() == "1") {
        //ddlLockedIn.SelectedValue = "0";
        //imgLocked.Visible = false;
        $('#ddlLockedIn').val('0');
        $('#imgLocked').css('display', 'none');
    }
    // PopLocked.Style.Add("display", "none");
    $('#PopLocked').css('display', 'none');
}
$(document).ready(function () {

    //FloorLabelMarksGet();
});

function MuseClick(XPosition, YPosition, SiteFloorLevelImage) {


    var hdnFloorImage = SiteFloorLevelImage;

    var FloorImage = "";

    var canvas = document.getElementById('CanvasFloorImage');
    var context = canvas.getContext("2d");
    var canvas1 = document.getElementById('CanvasFloorZoomImage');
    var context1 = canvas1.getContext("2d");
    var canvas2 = document.getElementById('CanvasFloorImage_hidden');
    var context2 = canvas2.getContext("2d");

    // Map sprite
    var mapSprite = new Image();
    var mapSprite1 = new Image();
    var mapSprite2 = new Image();
    // mapSprite.src = "http://www.retrogameguide.com/images/screenshots/snes-legend-of-zelda-link-to-the-past-8.jpg";
    // mapSprite.src = "/images/MapImageTest.png";

    //  var hdnFloorImage = $("#ContentPlaceHolder1_hdnFloorImage").val();
    //   if (hdnFloorImage != "") {
    if (hdnFloorImage != "") {
        var siteimagelink = "/images/SiteFloorlevelimg/";
        FloorImage = siteimagelink + hdnFloorImage;
        //// mapSprite.src = "/images/MapImageTest.png";
        $('#divimagetext').css("display", "block");
        $('#divbtnflrimg').css("display", "block");
        $('#divbtnZoomfloor').css("display", "block"); //Add code on 11 May 2020
    }
    else {
        FloorImage = "/images/noimg.jpg";
        $('#divimagetext').css("display", "none");
        $('#divbtnflrimg').css("display", "none");
        $('#divbtnZoomfloor').css("display", "none"); //Add code on 11 May 2020
    }
    mapSprite.src = FloorImage
    mapSprite1.src = FloorImage
    mapSprite2.src = FloorImage

    $('#forcheckFloarImageSize').attr("src", FloorImage)




    var Marker = function () {
        this.Sprite = new Image();
        //this.Sprite.src = "http://www.clker.com/cliparts/w/O/e/P/x/i/map-marker-hi.png"
        this.Sprite.src = "/images/SiteFloorlevelimg/RedMarkerPointer.png";
        //this.Width = 25;
        this.Width = 25;
        this.Height = 50;
        this.XPos = 0;
        this.YPos = 0;
        this.XPos1 = 0;
        this.YPos1 = 0;
    }


    var Markers = new Array();
    if (XPosition != "" && YPosition != "") {

        var marker = new Marker();
        //marker.XPos = XPosition;
        marker.XPos = XPosition - (marker.Width / 2);  // Line Comment on 8 May 2020
        marker.YPos = YPosition - marker.Height;

        Markers.push(marker);
    }

    var Markers1 = new Array();
    if (XPosition != "" && YPosition != "") {

        var marker1 = new Marker();
        //marker1.XPos1 = XPosition - (marker1.Width / 2);
        //marker1.YPos1 = YPosition - marker1.Height;
        marker1.XPos1 = (XPosition * 3) - (marker1.Width / 2);
        marker1.YPos1 = (YPosition * 3) - marker1.Height;
        //marker1.YPos1 = YPosition ;
        Markers1.push(marker1);
    }

    var Markers2 = new Array();
    if (XPosition != "" && YPosition != "") {

        var marker2 = new Marker();
        //marker1.XPos1 = XPosition - (marker1.Width / 2);
        //marker1.YPos1 = YPosition - marker1.Height;
        marker2.XPos1 = (XPosition * 3) - (marker2.Width / 2);
        marker2.YPos1 = (YPosition * 3) - marker2.Height;
        //marker1.YPos1 = YPosition ;
        Markers2.push(marker2);
    }

    var mouseClicked = function (mouse) {
        // Get corrent mouse coords
        var rect = canvas.getBoundingClientRect();
        var mouseXPos = (mouse.x - rect.left);
        var mouseYPos = (mouse.y - rect.top);


        //console.log("Marker added");
        //alert("Mouse X Position  " + mouseXPos1+ "  Mouse Y Position  " + mouseYPos1+ "   New Canvas Width   " + canvas1.width + " New Canvas Height  " + canvas1.height + " (mouse.x )  " + mouse.x + " (mouse.y)   " +  mouse.y + " (rect1.left)  " + rect1.left + " (rect1.top)  " + rect1.top );
        // alert("Mouse X Position  " + mouseXPos+ "  Mouse Y Position  " + mouseYPos+ "   Old Canvas Width   " + canvas.width + " Old Canvas Height  " + canvas.height + " (mouse.x )  " + mouse.x + " (mouse.y)   " +  mouse.y + " (rect.left)  " + rect.left + " (rect.top)  " + rect.top );

        // Move the marker when placed to a better location

        var marker = new Marker();
        var mouseXPosition = mouseXPos;
        //var mouseXPosition = mouseXPos - (marker.Width / 2);  // Line Comment on 8 May 2020
        // var mouseXPosition = mouseXPos - (marker.Width-3);  // Line Add on 8 May 2020
        var mouseYPosition = mouseYPos - marker.Height;
        $("#hdnmouseXPos").val(mouseXPos);
        $("#hdnmouseYPos").val(mouseYPos);


        if ($("#hdnXPosition").val() != "" && $("#hdnYPosition").val() != "") {
            // alert(' ');
            $('#divFloorPopup').css("display", "block");


        } else {
            marker.XPos = mouseXPos;
            // marker.XPos = mouseXPos - (marker.Width / 2);   // Line Comment on 8 May 2020
            // marker.XPos = mouseXPos - (marker.Width-3);  // Line Add on 8 May 2020
            marker.YPos = mouseYPos - marker.Height;

            Markers.push(marker);
            canvas.removeEventListener("mousedown", mouseClicked);
            SaveFloorLabelMarks(mouseXPos, mouseYPos);
            //  FloorLabelMarksGet();
        }


    }

    var mouseClicked1 = function (mouse) {

        // Get corrent mouse coords                
        var rect1 = canvas1.getBoundingClientRect();
        //var left= rect1.left;
        //var top= rect1.top;
        var mouseXPos1 = (mouse.x - rect1.left);
        var mouseYPos1 = (mouse.y - rect1.top);

        var marker1 = new Marker();

        var mouseXPosition1 = mouseXPos1;
        var mouseYPosition1 = mouseYPos1 - marker1.Height;
        $("#hdnmouseXPos1").val(mouseXPos1);
        $("#hdnmouseYPos1").val(mouseYPos1);

        //if ($("#ContentPlaceHolder1_hdnXPosition").val() != "" && $("#ContentPlaceHolder1_hdnYPosition").val() != "") {
        //    // alert(' ');
        //    $('#ContentPlaceHolder1_divFloorPopup').css("display", "block");


        //} else {

        marker1.XPos1 = mouseXPos1;
        // marker1.XPos1 = mouseXPos1 - (marker.Width / 2); 
        // marker1.YPos1 = mouseYPos1 - marker1.Height;
        marker1.YPos1 = mouseYPos1;

        var ImageRationWidthForMarkerSave;
        var ImageRationHeightForMarkerSave;

        ImageRationWidthForMarkerSave = canvas.width / canvas1.width;
        ImageRationHeightForMarkerSave = canvas.height / canvas1.height;

        var xPosSave1 = (mouseXPos1 * ImageRationWidthForMarkerSave);
        var yPosSave1 = ((mouseYPos1) * ImageRationHeightForMarkerSave);
        //var yPosSave1 = ((mouseYPos1 - (marker1.Width / 2)) * ImageRationHeightForMarkerSave);

        Markers1.push(marker1);
        canvas1.removeEventListener("mousedown", mouseClicked1);
        // SaveFloorLabelMarks(xPosSave1, yPosSave1);

        var mouseXPosZoomVal = mouseXPos1 / 3;
        var mouseYPosZoomVal = mouseYPos1 / 3;
        SaveFloorLabelMarks(mouseXPosZoomVal, mouseYPosZoomVal);

        //alert("Mouse X Position  " + mouseXPos1+ "  Mouse Y Position  " + mouseYPos1+ "   New Canvas Width   " + canvas1.width + " New Canvas Height  " + canvas1.height + " (mouse.x )  " + mouse.x + " (mouse.y)   " +  mouse.y + " (rect1.left)  " + rect1.left + " (rect1.top)  " + rect1.top );
        // }
    }

    var mouseClicked2 = function (mouse) {
        // Get corrent mouse coords                
        var rect1 = canvas2.getBoundingClientRect();
        var mouseXPos1 = (mouse.x - rect1.left);
        var mouseYPos1 = (mouse.y - rect1.top);
        var marker2 = new Marker();
        var mouseXPosition1 = mouseXPos1;
        var mouseYPosition1 = mouseYPos1 - marker2.Height;
        $("#hdnmouseXPos1").val(mouseXPos1);
        $("#hdnmouseYPos1").val(mouseYPos1);
        marker2.XPos1 = mouseXPos1;
        marker2.YPos1 = mouseYPos1;

        var ImageRationWidthForMarkerSave;
        var ImageRationHeightForMarkerSave;

        ImageRationWidthForMarkerSave = canvas.width / canvas2.width;
        ImageRationHeightForMarkerSave = canvas.height / canvas2.height;

        var xPosSave1 = (mouseXPos1 * ImageRationWidthForMarkerSave);
        var yPosSave1 = ((mouseYPos1) * ImageRationHeightForMarkerSave);
        Markers2.push(marker2);
        canvas2.removeEventListener("mousedown", mouseClicked1);
        // SaveFloorLabelMarks(xPosSave1, yPosSave1);

        var mouseXPosZoomVal = mouseXPos1 / 3;
        var mouseYPosZoomVal = mouseYPos1 / 3;
        SaveFloorLabelMarks(mouseXPosZoomVal, mouseYPosZoomVal);

        //alert("Mouse X Position  " + mouseXPos1+ "  Mouse Y Position  " + mouseYPos1+ "   New Canvas Width   " + canvas1.width + " New Canvas Height  " + canvas1.height + " (mouse.x )  " + mouse.x + " (mouse.y)   " +  mouse.y + " (rect1.left)  " + rect1.left + " (rect1.top)  " + rect1.top );
        // }
    }


    if (hdnFloorImage != "") {

        // Add mouse click event listener to canvas
        canvas.addEventListener("mousedown", mouseClicked, false);
        canvas1.addEventListener("mousedown", mouseClicked1, false);
        canvas2.addEventListener("mousedown", mouseClicked1, false);
    }



    var firstLoad = function () {
        context.font = "15px Georgia";
        context.textAlign = "center";
    }

    firstLoad();

    var main = function () {
        draw();
    };


    var draw = function () {


        var RealFloorImageWidth = $('#forcheckFloarImageSize').width();
        var RealFloorImageHeight = $('#forcheckFloarImageSize').height();

        var imageWidth = canvas.width;
        var imageWidth1 = canvas1.width;
        var imageWidth2 = canvas2.width;
        //var imageHeight = (canvas.width * RealFloorImageHeight) / RealFloorImageWidth;
        var imageHeight = canvas.width * (RealFloorImageHeight / RealFloorImageWidth);
        var imageHeight1 = canvas1.width * (RealFloorImageHeight / RealFloorImageWidth);
        var imageHeight2 = canvas2.width * (RealFloorImageHeight / RealFloorImageWidth);

        console.log(RealFloorImageHeight)
        console.log(RealFloorImageWidth);

        context.canvas.height = imageHeight;
        context1.canvas.height = imageHeight1;
        context2.canvas.height = imageHeight2;

        // Clear Canvas
        context.fillStyle = "#000";
        context.fillRect(0, 0, imageWidth, imageHeight);
        context1.fillStyle = "#000";
        context1.fillRect(0, 0, imageWidth1, imageHeight1);
        context2.fillStyle = "#000";
        context2.fillRect(0, 0, imageWidth2, imageHeight2);
        // context.fillRect(0, 0, canvas.width, canvas.height);

        // Draw map
        // Sprite, X location, Y location, Image width, Image height
        // You can leave the image height and width off, if you do it will draw the image at default size

        //context.drawImage(mapSprite, 0, 0, 300, 300);
        context.drawImage(mapSprite, 0, 0, imageWidth, imageHeight);
        context1.drawImage(mapSprite1, 0, 0, imageWidth1, imageHeight1);
        context2.drawImage(mapSprite2, 0, 0, imageWidth2, imageHeight2);
        //  canvas.width = 300;
        //  canvas.height = 300;


        for (var i = 0; i < Markers.length; i++) {
            var tempMarker = Markers[i];
            // Draw marker
            context.drawImage(tempMarker.Sprite, tempMarker.XPos, tempMarker.YPos, tempMarker.Width, tempMarker.Height);
            //context.drawImage(tempMarker.Sprite, tempMarker.XPos-(tempMarker.Width/2), tempMarker.YPos, tempMarker.Width, tempMarker.Height);               
            //context1.drawImage(tempMarker.Sprite, (tempMarker.XPos1-(tempMarker.Width/2))*  (canvas1.width/canvas.width), tempMarker.YPos1*  (canvas1.height/canvas.height), tempMarker.Width, tempMarker.Height);

        }

        for (var i = 0; i < Markers1.length; i++) {
            var tempMarker1 = Markers1[i];

            var ImageRationWidthForMarker;
            var ImageRationHeightForMarker;
            ImageRationWidthForMarker = imageWidth1 / imageWidth;
            ImageRationHeightForMarker = imageHeight1 / imageHeight;

            // var xPosShow1 = (tempMarker1.XPos1 * ImageRationWidthForMarker) +(marker1.Width / 2);
            // var yPosShow1 = (tempMarker1.YPos1 * ImageRationHeightForMarker) + marker1.Height ;
            var xPosShow1 = (tempMarker1.XPos1 * ImageRationWidthForMarker) - (marker1.Width / 2);
            var yPosShow1 = (tempMarker1.YPos1 * ImageRationHeightForMarker) - marker1.Height;
            //var xPosShow1 = (tempMarker1.XPos1 * ImageRationWidthForMarker) ;
            //var yPosShow1 = (tempMarker1.YPos1 * ImageRationHeightForMarker) ;
            context1.drawImage(tempMarker1.Sprite, tempMarker1.XPos1, tempMarker1.YPos1, tempMarker1.Width, tempMarker1.Height);
            //  context1.drawImage(tempMarker1.Sprite, xPosShow1, yPosShow1, tempMarker1.Width, tempMarker1.Height);

        }

        for (var i = 0; i < Markers2.length; i++) {
            var tempMarker2 = Markers2[i];

            var ImageRationWidthForMarker;
            var ImageRationHeightForMarker;
            ImageRationWidthForMarker = imageWidth2 / imageWidth;
            ImageRationHeightForMarker = imageHeight2 / imageHeight;
            var xPosShow1 = (tempMarker2.XPos1 * ImageRationWidthForMarker) - (marker2.Width / 2);
            var yPosShow1 = (tempMarker2.YPos1 * ImageRationHeightForMarker) - marker2.Height;
            context2.drawImage(tempMarker2.Sprite, tempMarker2.XPos1, tempMarker2.YPos1, tempMarker2.Width, tempMarker2.Height);

        }

        // alert("Mouse X Position  " + tempMarker.XPos + "  Mouse Y Position  " + tempMarker.YPos + "   Canvas Width   " + canvas.width + "  Canvas Height  " + canvas.height + "  RealFloorImageHeight   " + imageHeight + "  RealFloorImageWidth   " + imageWidth );
        // alert("Mouse X Position  " + tempMarker1.XPos1 + "  Mouse Y Position  " + tempMarker1.YPos1 + "   Canvas Width   " + canvas1.width + "  Canvas Height  " + canvas1.height + "  RealFloorImageHeight   " + imageHeight1 + "  RealFloorImageWidth   " + imageWidth1 );
        // console.log("Mouse X Position  " + tempMarker.XPos + "  Mouse Y Position  " + tempMarker.YPos + "   Canvas Width   " + canvas1.width + "  Canvas Height  " + canvas1.height + "  RealFloorImageHeight   " + imageHeight1 + "  RealFloorImageWidth   " + imageWidth1);
        // Draw markers

    };

    setTimeout(function () {
        draw();
    }, 1000);

    //setInterval(main, (1000)); // Refresh 60 times a second

}

//add function 11-May-2020
function floorimagepinZoom() {
    $('#divZoomFloorPopup').css("display", "block");
    FloorLabelMarksGet();
}




function SaveFloorLabelMarks(XPosition, YPosition) {

    var clientID = $("#hdnClientID").val();
    var siteID = $('#ddlsites').val();
    var RID = $("#hdnRID").val();
    var FloorLabel = $("#DropDownList1").val();

    var XPosition = XPosition.toString();
    var YPosition = YPosition.toString();
    $.ajax({
        type: "POST",
        //url: "../WebApi/Dashboard.asmx/FloorLabelMarksSave",
        url: "/Register/FloorLabelMarksSave",
        data: JSON.stringify({ id: clientID, RID: RID, XPosition: XPosition, YPosition: YPosition }),
        // data: JSON.stringify({ id: clientID}),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            // var id = response.d;
            var id = response;
            if (id == 1) {
                FloorLabelMarksGet();
            }

        }
    });
}

function FloorLabelMarksGet() {

    var clientID = $("#hdnClientID").val();
    var siteID = $('#ddlsites').val();
    var RID = $("#hdnRID").val();
    // var FloorLabel = $("#DropDownList1").val();
    var FloorLabelID = $("#DropDownList1").val();
    var LocationId = $("#ddlLocation").val();
    var BuildingId = $("#ddbuilding").val();

    $.ajax({
        type: "POST",
        //url: "../WebApi/Dashboard.asmx/FloorLabelMarksGet",
        url: "/Register/FloorLabelMarksGet",
        // data: JSON.stringify({ id: clientID, siteID: siteID, FloorLabel: FloorLabel, RID: RID }),
        data: JSON.stringify({ id: clientID, siteID: siteID, FloorLabelID: FloorLabelID, RID: RID, LocationId: LocationId, BuildingId: BuildingId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",

        success: function (response) {
            //var GetData = response.d;
            var GetData = response;
            // if (GetData.length > 0) {
            // If Data Get Then We Show The Data
            // var XPosition = GetData[0].XPosition == null ? "" : GetData[0].XPosition;
            // var YPosition = GetData[0].YPosition == null ? "" : GetData[0].YPosition;
            //  var SiteFloorLevelImage = GetData[0].SiteFloorLevelImage == null ? "" : GetData[0].SiteFloorLevelImage;
            var XPosition = GetData.XPosition == null ? "" : GetData.XPosition;
            var YPosition = GetData.YPosition == null ? "" : GetData.YPosition;
            var SiteFloorLevelImage = GetData.SiteFloorLevelImage == null ? "" : GetData.SiteFloorLevelImage;
            $("#hdnXPosition").val(XPosition);
            $("#hdnYPosition").val(YPosition);

            if (XPosition != "" && YPosition != "") {
                //  ShowImagePointer(XPosition, YPosition);
                MuseClick(XPosition, YPosition, SiteFloorLevelImage);

            }
            else {
                MuseClick(XPosition, YPosition, SiteFloorLevelImage);

            }
            // }
        }
    });
}

function btnFloorMarkYesClick() {
    var canvas = document.getElementById('CanvasFloorImage');

    //var rect = canvas.getBoundingClientRect();
    //var mouseXPos = (mouse.x - rect.left);
    //var mouseYPos = (mouse.y - rect.top);


    //console.log("Marker added");

    //// Move the marker when placed to a better location
    //  var marker = new Marker();
    //var mouseXPosition = mouseXPos - (marker.Width / 2);
    //var mouseYPosition = mouseYPos - marker.Height;


    //marker.XPos = mouseXPos;
    //marker.YPos = mouseYPos;

    //marker.XPos = 122.671875;
    //marker.YPos = 54.328125;
    var mouseXPosition = $("#hdnmouseXPos").val();
    var mouseYPosition = $("#hdnmouseYPos").val();

    //marker.XPos = mouseXPosition;
    //marker.YPos = mouseYPosition;
    //Markers.push(marker);

    //  canvas.removeEventListener("mousedown", mouseClicked);
    SaveFloorLabelMarks(mouseXPosition, mouseYPosition);
    $('#divFloorPopup').css("display", "none");

    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var RID = getParameterByName('RID');
    window.location.href = '/Register/Register?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID;
}

function btnFloorMarkNoClick() {
    $('#divFloorPopup').css("display", "none");
}

function btnSiteFloorLevelChangeNo() {
    $('#divConformationforSiteFloor').css("display", "none");
}


// Add Code on 10 Sep 2019
function addnewScaffoldOwner() {
    $('#txtScaffoldOwnerRep').val("");
    $("#divCreateScaffoldOwnere").css("display", "block");
}

function btnScaffoldOwnerClose() {

    $('#txtScaffoldOwnerRep').val("");
    $('#divCreateScaffoldOwnere').css('display', 'none');

}
// Add Code On 26 August 2019 Start

function addnewdiscipline() {
    $('#txtnewDiscipline').val("");
    $("#divCreatePopup").css("display", "block");
}

function btnclose() {
    $('#txtnewDiscipline').val("");
    $('#divCreatePopup').css('display', 'none');
}

// Add Code On 26 August 2019 End

function addnewWorkGroups() {
    //$('#txtWorkGroups').val("")
    //$("#divCreateWorkGroups").css("display", "block");
    var UID = getParameterByName('UID');
    var RID = getParameterByName('RID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    var sitelocid = $('#ddlsites').val();
    if (sitelocid == "" || sitelocid == "0" || sitelocid == null) {
        var Response = { Result: "Failed", Message: "Please select Site first before adding WorkGroup." };
        CustomAlert(Response);
        return;
    }
    window.location.href = '/WorkGroups/WorkGroups?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid + '&RID=' + RID + '&SiteLocId=' + sitelocid;
}
function btnCloseWorkGroups() {

    $('#txtWorkGroups').val("")
    $("#divCreateWorkGroups").css("display", "none");
}

//function addnewSubGroups() {
//    $('#txtSubGroups').val("");
//    $("#divCreateSubGroups").css("display", "block");

//}
function addnewSubGroups() {
    var UID = getParameterByName('UID');
    var RID = getParameterByName('RID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    var sitelocid = $('#ddlsites').val() || 0;
    var workGroupId = $('#ddlWorkGroups').val() || 0;
    if (sitelocid == "" || sitelocid == 0 || sitelocid == null) {
        var Response = { Result: "Failed", Message: "Please select Site first before adding WorkGroup." };
        CustomAlert(Response);
        return;
    }
    if (workGroupId > 0) {
        window.location.href = '/WorkGroups/WorkGroups?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid + '&RID=' + RID + '&SiteLocId=' + sitelocid + '&WorkGroupId=' + workGroupId;
    } else {
        window.location.href = '/WorkGroups/WorkGroups?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid + '&RID=' + RID + '&SiteLocId=' + sitelocid;
    }
}

function addRequestedBy() {
    var UID = getParameterByName('UID');
    var RID = getParameterByName('RID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    var sitelocid = $('#ddlsites').val() || 0;
    if (sitelocid == "" || sitelocid == 0 || sitelocid == null) {
        var Response = { Result: "Failed", Message: "Please select Site first before adding Requested By." };
        CustomAlert(Response);
        return;
    }
    window.location.href = '/RequestedBy/RequestedBy?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid + '&RID=' + RID + '&SiteLocId=' + sitelocid;
}

function btnCloseSubGroups() {
    $('#txtSubGroups').val("");
    $("#divCreateSubGroups").css("display", "none");
}

function btnSaveDiscipline_Click() {
    var txtnewDiscipline = $('#txtnewDiscipline').val();

    if (txtnewDiscipline != "") {
        $('#txtdiscipline').append(`<option value="${txtnewDiscipline}" selected='selected'>
                                       ${txtnewDiscipline}
                                  </option>`);
        $('#divCreatePopup').css('display', 'none');
        var Response = { Result: "Success", Message: "Record Save Successfully." };
        CustomAlert(Response);

    } else {
        var Response = { Result: "Failed", Message: "Please fill Sub Owner Rep." };
        CustomAlert(Response);
    }
}
function btnScaffoldOwnerSave_Click() {
    var txtScaffoldOwnerRep = $('#txtScaffoldOwnerRep').val();

    if (txtScaffoldOwnerRep != "") {
        $('#txtScaffoldOwner').append(`<option value="${txtScaffoldOwnerRep}" selected='selected'>
                                       ${txtScaffoldOwnerRep}
                                  </option>`);
        $('#divCreateScaffoldOwnere').css('display', 'none');
        var Response = { Result: "Success", Message: "Record Save Successfully." };
        CustomAlert(Response);

    } else {
        var Response = { Result: "Failed", Message: "Please fill Sub Owner." };
        CustomAlert(Response);
    }
}

function btnSaveWorkGroups_Click() {
    var txtWorkGroups = $('#txtWorkGroups').val();

    if (txtWorkGroups != "") {
        $('#ddlWorkGroups').append(`<option value="${txtWorkGroups}" selected='selected'>
                                       ${txtWorkGroups}
                                  </option>`);
        $('#divCreateWorkGroups').css('display', 'none');
        var Response = { Result: "Success", Message: "Record Save Successfully." };
        CustomAlert(Response);

    } else {
        var Response = { Result: "Failed", Message: "Please fill Work Groups." };
        CustomAlert(Response);
    }
}

function btnSaveSubGroups_Click() {
    var txtSubGroups = $('#txtSubGroups').val();

    if (txtSubGroups != "") {
        $('#ddlSubGroups').append(`<option value="${txtSubGroups}" selected='selected'>
                                       ${txtSubGroups}
                                  </option>`);
        $('#divCreateSubGroups').css('display', 'none');
        var Response = { Result: "Success", Message: "Record Save Successfully." };
        CustomAlert(Response);

    } else {
        var Response = { Result: "Failed", Message: "Please fill Sub Groups." };
        CustomAlert(Response);
    }
}
function floorimagepinRemove() {

    // document.getElementById("lblMessage").textContent = 'Are you sure you want to Remove the Job Location ?';
    $('#lblMessage').text('Are you sure you want to Remove the Job Location ?');
    $('#divFloorPopup').css("display", "block");
}

function QRCodePrint_click() {

    printWindow = window.open("", "mywindow", "location=1,status=1,scrollbars=1,width=600,height=600");
    printWindow.document.write("<div style='width:100%;'>");
    // printWindow.document.write("<img id='img' src='" + document.getElementById('imageQRCode').src + "'/>");
    printWindow.document.write("<img id='img' src='" + document.getElementById('QRCodeImage').src + "'/>");
    printWindow.document.write("</div>");
    printWindow.document.close();
    printWindow.print();

    //var id = getParameterByName('id');
    //var RID = getParameterByName('RID');
    //var UID = getParameterByName('UID');
    //var UserRole = getParameterByName('UserRole');
    //window.location.href = '/Register/Register?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID;
}

function borderchange(str1) {
    var type = str1.split(",");
    for (var i = 0; i < type.length; i++) {
        if (type[i] == "Design Type") {
            $("#ddlScaffoldDesign").css("border-color", "red");
        }
        if (type[i] == "Material Type") {
            $("#ddlMaterialType").css("border-color", "red");
        }
        if (type[i] == "Man Hrs Erect") {
            $("#txtManhrserect").css("border-color", "red");
        }
        if (type[i] == "Man Hrs Dismantle") {
            $("#txtmanDisman").css("border-color", "red");
        }
    }
}

//function ButtonClient() {
//    $('#LoadingPanel1').css('display', 'block');
//}

function checklatlong() {
    var lat = $("#lbllat").val();
    var long = $("#lbllong").val();
    geolocatewithparameter(lat, long);

}



//function ActiveMultiSelect() {
//    $("select.chosen-select").select2();
//}

function MaintainInvCheckBoxMessage() {
    $('.maintainInventory').hover(function () {
        $('#ContentPlaceHolder1_art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
        $('#ContentPlaceHolder1_art1').css("display", "block");
        $('#ContentPlaceHolder1_alertlabel').html('<strong>Alert ! </strong>Material has already been loaded on Job Inventory so you cannot uncheck Maintain Inventory.');
        hideAlert();
    });

}

function MaintainInvDateDismantle() {
    $('.maintainInventory').hover(function () {
        $('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
        $('#art1').css("display", "block");
        $('#alertlabel').html('<strong>Alert ! </strong>The job has been dismantled no changes should be made.');
        hideAlert();
    });

}

function FinalizedMessage() {
    $('.messagedatadis').hover(function () {
        $('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
        $('#art1').css("display", "block");
        $('#alertlabel').html('<strong>Alert ! </strong>Please ensure to mark material list as Finalized before filling in Dismantle Date.');
        hideAlert();
    });

}

function ReturnMessage() {
    $('.messagedatadis').hover(function () {
        $('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
        $('#art1').css("display", "block");
        $('#alertlabel').html('<strong>Alert ! </strong>Please ensure to return all Material before filling in Dismantle Date.');
        hideAlert();
    });
}

function InventoryFilled() {
    $('.clsNoInventory').hover(function () {
        $('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
        $('#art1').css("display", "block");
        $('#alertlabel').html('<strong>Alert ! </strong>Inventory has Already been loaded.');
        hideAlert();
    });
}

function DateDismantleFilled() {
    $('.clsNoInventory').hover(function () {
        $('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
        $('#art1').css("display", "block");
        $('#alertlabel').html('<strong>Alert ! </strong>Date Dismantle is already filled for this job.');
        hideAlert();
    });
}

function hideAlert() {
    //alert();
    setTimeout(function () {
        $(".fadeOut").fadeOut(3000);
    }, 4000);
}

function searchlocation() {
    var enterlat = $("#lbllat").val();
    var enterlong = $("#lbllong").val();
    geolocatewithparameter(enterlat, enterlong);
}

//On Map Delete button Click

function resetimage() {
    $('#ContentPlaceHolder1_divdelete').css("display", "block");
}

//On Image Delete button Click
function resetlocation() {
    $('#map_populate').css("display", "none");
    // $('#gpsbutton').css("display", "block"); // Line Change on 12 March 2020
    // $('#gpsbutton').css("display", "inline-flex");  // Line Change on 12 March 2020
    $('#lbl1').css("display", "none");
    $('#lbl2').css("display", "none");
    $("#hdnlatitude").val(" ");
    $("#hdnlongitude").val(" ");
}
//On GPS Button CLick
function displaymap() {
    // displaymapCenterOfSite();
    if (navigator.geolocation) {
        //  navigator.geolocation.getCurrentPosition(success); // LIne Comment on 13 March 2020
        navigator.geolocation.getCurrentPosition(showPosition, showError);
    } else {
        alert("Geo Location is not supported on your current browser!");
    }
    var newLat = null;
    var newLng = null;

    // Code Comment on 13 March 2020 Start

    //function success(position) {

    //    if (newLat != null && newLng != null) {

    //        var lat = newLat;
    //        var long = currentlng;

    //    } else {
    //        var lat = position.coords.latitude;
    //        var long = position.coords.longitude;

    //        if (lat == null) {
    //            alert("GPS not activated!");
    //        }
    //    }
    //    geolocatewithparameter(lat, long);

    //    $('#map_populate').css("display", "block");
    //    $('#ContentPlaceHolder1_gpsbutton').css("display", "none");
    //    $('#ContentPlaceHolder1_gpsbutton1').css("display", "none");
    //    $('#ContentPlaceHolder1_lbl1').css("display", "block");
    //    $('#ContentPlaceHolder1_lbl2').css("display", "block");

    //}

    // Code Comment on 13 March 2020 End

    // Code Add on 13 March 2020
    function showPosition(position) {
        if (newLat != null && newLng != null) {

            var lat = newLat;
            var long = currentlng;

        } else {
            var lat = position.coords.latitude;
            var long = position.coords.longitude;

            if (lat == null) {
                alert("GPS not activated!");
            }
        }
        geolocatewithparameter(lat, long);

        $('#map_populate').css("display", "block");
        $('#gpsbutton').css("display", "none");
        $('#gpsbutton1').css("display", "none");
        $('#lbl1').css("display", "block");
        $('#lbl2').css("display", "block");
    }

    function showError(error) {
        switch (error.code) {
            case error.PERMISSION_DENIED:
                alert("Location has been blocked on your browser. Please follow these steps \n 1. On your browser At the top right, click More and then Settings. \n 2. Under Privacy and security, click Site settings. \n 3. Select the Location and give permission by clicking on Blocked. \n 4.Status show Ask before accessing (recommended).");
                break;
            case error.POSITION_UNAVAILABLE:
                alert("Location information is unavailable.");
                break;
            case error.TIMEOUT:
                alert("The request to get user location timed out.");
                break;
            case error.UNKNOWN_ERROR:
                alert("An unknown error occurred.");
                break;
        }
    }

}

//On "Pin To Center Of Site" Button CLick  Add on 30 Jan 2020
function displaymapCenterOfSite() {

    var clientID = $("#hdnClientID").val();
    var SiteID = $('#ddlsites').val();
    if (SiteID != "") {

        $.ajax({
            type: "POST",
            url: "/Register/GetSiteLatOrLong",
            data: JSON.stringify({ clientID: clientID, SiteID: SiteID }),
            // data: JSON.stringify({ id: clientID}),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                var result = response;
                if (result != "" || result != null) {
                    var arr = result.split('_');
                    var SiteLat = arr[0];
                    var SiteLong = arr[1];

                    if (navigator.geolocation) {
                        // navigator.geolocation.getCurrentPosition(success, showError);  // Line Comment On 13 March 2020
                        navigator.geolocation.getCurrentPosition(showPosition, showError);

                    } else {
                        alert("Geo Location is not supported on your current browser!");
                    }

                    // Code Comment on 13 March 2020 Start
                    //function success(position) {

                    //    if (SiteLat != null && SiteLong != null) {

                    //        geolocatewithparameter(SiteLat, SiteLong);

                    //    }

                    //    $('#map_populate').css("display", "block")  
                    //    $('#ContentPlaceHolder1_gpsbutton').css("display", "none");
                    //    $('#ContentPlaceHolder1_gpsbutton1').css("display", "none");
                    //    $('#ContentPlaceHolder1_lbl1').css("display", "block");
                    //    $('#ContentPlaceHolder1_lbl2').css("display", "block");

                    //}
                    // Code Comment on 13 March 2020 End


                    // Code Add on 13 March 2020

                    function showPosition(position) {
                        if (SiteLat != null && SiteLong != null) {
                            geolocatewithparameter(SiteLat, SiteLong);
                        }
                        $('#map_populate').css("display", "block")
                        $('#gpsbutton').css("display", "none");
                        $('#gpsbutton1').css("display", "none");
                        $('#lbl1').css("display", "block");
                        $('#lbl2').css("display", "block");
                    }

                    function showError(error) {
                        switch (error.code) {
                            case error.PERMISSION_DENIED:
                                alert("Location has been blocked on your browser. Please follow these steps \n 1. On your browser At the top right, click More and then Settings. \n 2. Under Privacy and security, click Site settings. \n 3. Select the Location and give permission by clicking on Blocked. \n 4.Status show Ask before accessing (recommended).");
                                break;
                            case error.POSITION_UNAVAILABLE:
                                alert("Location information is unavailable.");
                                break;
                            case error.TIMEOUT:
                                alert("The request to get user location timed out.");
                                break;
                            case error.UNKNOWN_ERROR:
                                alert("An unknown error occurred.");
                                break;
                        }
                    }

                }
            }
        });
    } else {
        var Response = { Result: "Failed", Message: "Please Select Site ." }
        CustomAlert(Response);
    }
}

// Map Location With latitude and longitude

function geolocatewithparameter(lattitude, longitude) {
    if (lattitude != null && longitude != null && lattitude != "" && longitude != "") {
        $('#map_populate').css("display", "block");
        $('#gpsbutton').css("display", "none");
        $('#gpsbutton1').css("display", "none");
        $('#lbl1').css("display", "block");
        $('#lbl2').css("display", "block");
    }
    else {
        $('#map_populate').css("display", "none");
        // $('#gpsbutton').css("display", "block");   // Line Comment On 12 March 2020
        //  $('#gpsbutton').css("display", "inline-flex");//add   // Line Comment On 12 March 2020
    }

    if (navigator.geolocation) {
        if (lattitude == null && longitude == null) {
            navigator.geolocation.getCurrentPosition(function (position) {
                var geolocation = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);
                var geocoder = new google.maps.Geocoder();
                latLng = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);
            });
        }
        var geolocation = new google.maps.LatLng(lattitude, longitude);
        var geocoder = new google.maps.Geocoder();
        latLng = new google.maps.LatLng(lattitude, longitude);
        $("#hdnlatitude").val(lattitude);
        $("#hdnlongitude").val(longitude);
        $("#lbllat").val(lattitude);
        $("#lbllong").val(longitude);
        if (geocoder) {
            geocoder.geocode({ 'latLng': latLng }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    flag = true;
                    currentaddess = results[0].formatted_address;
                    var mapOptions = {
                        center: new google.maps.LatLng(lattitude, longitude),
                        zoom: 19,
                        mapTypeId: google.maps.MapTypeId.SATELLITE
                    };
                    var map = new google.maps.Map(document.getElementById("map_populate"),
                        mapOptions);
                    var maphidden = new google.maps.Map(document.getElementById("map_populate_hidden"), mapOptions); // Added on 09/Mar2022
                    // create a marker
                    var latlng = new google.maps.LatLng(lattitude, longitude);
                    //
                    var marker = new google.maps.Marker({
                        position: latlng,
                        map: map,
                        draggable: true,
                        title: results[0].formatted_address
                    });
                    var Hiddenmarker = new google.maps.Marker({ // Added on 09/Mar/2022
                        position: latlng,
                        map: maphidden,
                        draggable: true,
                        title: results[0].formatted_address
                    });
                    //baans added on 05 aug 2019 for drag location
                    google.maps.event.addListener(marker, 'dragend', function (event) {
                        $('#lbllat').val(this.getPosition().lat());
                        $('#lbllong').val(this.getPosition().lng());
                    });
                }
            });
        };
    }


    var mapcode;
    var diag;
    var flag;
    //function initialize(flag) {
    //    if (flag) {
    //        mapcode = new google.maps.Geocoder();
    //        var lnt = new google.maps.LatLng(26.45, 82.85);
    //        var diagChoice = {
    //            zoom: 15,
    //            center: lnt,
    //            diagId: google.maps.MapTypeId.SATELLITE
    //        }
    //        diag = new google.maps.Map(document.getElementById('map_populate'), diagChoice);
    //        flag = false;
    //    }

    //}
}

//Map Location End

var currentaddess;

$(document).ready(function () {
    $("#radUpdogImg").change(function () {
        readURL(this);
    });

    var GetisSafari = navigator.vendor && navigator.vendor.indexOf('Apple') > -1 && navigator.userAgent && !navigator.userAgent.match('CriOS');
    if (GetisSafari == false) {
        $(".nonsafari").css("display", "block");
        $(".safari").css("display", "none");
        var isMobile = /iPhone|iPad|iPod|Android/i.test(navigator.userAgent);
        if (isMobile) {
            $(".mobileApp").css("display", "block");
            $(".desktopApp").css("display", "none");
        } else {
            $(".mobileApp").css("display", "none");
            $(".desktopApp").css("display", "block");
        }
    } else {
        $(".nonsafari").css("display", "none");
        $(".safari").css("display", "block");
    }


    $(document).on("change", ".mob_file_image", function (evt) {
        var base64mobImagedata;
        var reader = new FileReader();
        reader.readAsDataURL(this.files[0]);
        reader.onloadend = function () {
            base64mobImagedata = reader.result;
            $("#imga").attr('src', base64mobImagedata);

            $("#tempimg").val(base64mobImagedata);
            $('#deleteimage').css('display', 'block');
            $('#btnImageSave').css('display', 'block');
            $('#downloadButton').css('display', 'none');

        }
    });

    $(document).on("change", "#mobApp_file", function (evt) {
        var base64mobdata;
        var reader = new FileReader();
        reader.readAsDataURL(this.files[0]);
        reader.onloadend = function () {
            base64mobdata = reader.result;
            $("#imga").attr('src', base64mobdata);

            $("#tempimg").val(base64mobdata);
            $('#deleteimage').css('display', 'block');
            $('#btnImageSave').css('display', 'block');
            $('#downloadButton').css('display', 'none');

        }
    });





    var imageflag = true;
    var screensize = $(window).width();
    if (screensize < 1150 && screensize > 650) {
        ($BODY.hasClass('nav-md'))
        $('#CreateNewDropDown').css("margin-top", "2px");
        $BODY.removeClass('nav-md').addClass('nav-sm');
        $LEFT_COL.removeClass('scroll-view');
        //$('#BlackSection').css('display', 'block');
        $('#imgLogo').parent().css('display', 'none');
        $('#imgLogo1').css('display', 'block').css("height", "38px");
        $('#leftbarsection').css('margin-top', '0px');
        //$('.xshight').css('height', '520px').css('margin-top', '0px').css('width', '100%');
        $('#btnchngeimg').css('margin-left', '12px');

        //Baans Change 2-Apr-2018
        //$('#ContentPlaceHolder1_btnSave').css('font-size', '11px');
        //$('#ContentPlaceHolder1_btnaddnew').css('font-size', '11px');
        //$('#ContentPlaceHolder1_btnCAncel').css('font-size', '11px');
    }
    else {
        if (screensize < (650)) {
            $BODY.removeClass('nav-sm')
            $('#imgLogo').parent().css('display', 'none');
            $('#imgLogo1').css('display', 'block').css("height", "52px");
            //  $('#QRCodeImage').css('width', '100px !important');
            //$('.xsfieldset').css('margin-left', '-22px').css("width", "100%");
            //$('.xshight').css('height', '520px').css('margin-top', '0px').css('width', '100%');
            //$('#map_populate').css('width', '290px').css('height', '173px');


            //Baans Change 2-Apr-2018
            //$('#ContentPlaceHolder1_btnaddnew').css('display', 'none');
            //$('#ContentPlaceHolder1_btnaddregjob').css('display', 'block').css('font-size', '10px').css('width', '100%').css('padding-left', '7px');
            //$('#ContentPlaceHolder1_btnCAncel').css('display', 'none');
            //$('#ContentPlaceHolder1_btnback').css('display', 'block').css('font-size', '10px').css('width', '100%').css('padding-left', '2px');
        }

        else {
            $('#CreateNewDropDown').css("margin-top", "6px");
            $BODY.removeClass('nav-sm').addClass('nav-md');
            $('#imgLogo').parent().css('display', 'block');
            $('#imgLogo1').css('display', 'none');
            $('#leftbarsection').css('margin-top', '40px');

        }

    }

    //function exitHandler() {
    //    if (!document.webkitIsFullScreen && !document.mozFullScreen && !document.msFullscreenElement) {
    //        $("#imga").removeClass("zoomin");
    //        if ($("#imga").hasClass("abc") == true) {
    //            $('#imga').addClass("rotatedimg");
    //            $('#imga').removeClass("abc");
    //        }
    //    }
    //}

    //function toggleFullscreen(elem) {
    //    elem = elem || document.documentElement;
    //    if (!document.fullscreenElement && !document.mozFullScreenElement &&
    //        !document.webkitFullscreenElement && !document.msFullscreenElement) {

    //        if ($("#imga").hasClass("rotatedimg") == true) {
    //            //alert("added");
    //            $('#imga').removeClass("rotatedimg");
    //            $('#imga').addClass("abc");
    //            imageflag = false;
    //        }

    //        $("#imga").addClass("zoomin");
    //        if (elem.requestFullscreen) {
    //            elem.requestFullscreen();
    //        } else if (elem.msRequestFullscreen) {
    //            elem.msRequestFullscreen();
    //        } else if (elem.mozRequestFullScreen) {
    //            elem.mozRequestFullScreen();
    //        } else if (elem.webkitRequestFullscreen) {
    //            elem.webkitRequestFullscreen(Element.ALLOW_KEYBOARD_INPUT);
    //        }
    //        document.addEventListener('fullscreenchange', exitHandler);
    //        document.addEventListener('webkitfullscreenchange', exitHandler);
    //        document.addEventListener('mozfullscreenchange', exitHandler);
    //        document.addEventListener('MSFullscreenChange', exitHandler);

    //    } else {
    //        if ($("#imga").hasClass("abc") == true) {
    //            $('#imga').addClass("rotatedimg");
    //            $('#imga').removeClass("abc");
    //        }

    //        if (document.exitFullscreen) {
    //            document.exitFullscreen();
    //        } else if (document.msExitFullscreen) {
    //            document.msExitFullscreen();
    //        } else if (document.mozCancelFullScreen) {
    //            document.mozCancelFullScreen();
    //        } else if (document.webkitExitFullscreen) {
    //            document.webkitExitFullscreen();
    //        }
    //    }
    //}

    function toggleFullscreen(elem) {

        // Get the modal
        var modal = document.getElementById('myModal');

        // Get the image and insert it inside the modal - use its "alt" text as a caption
        var img = document.getElementById('imga');
        var modalImg = document.getElementById("img01");
        //var captionText = document.getElementById("caption");
        //var angleRotation = 45; // Define the angle of rotation
        img.onclick = function () {
            modalImg.style.transform = 'rotate(' + angelRotetion + 'deg)';

            modal.style.display = "block";
            modalImg.src = this.src;
            modalImg.alt = this.alt;
            //captionText.innerHTML = this.alt;

            // Apply rotation to the image using plain JavaScript
        }

        // When the user clicks on the modal, close it
        modal.onclick = function () {
            modalImg.className += " out";
            setTimeout(function () {
                modal.style.display = "none";
                modalImg.className = "modal-content";
                // Reset the rotation when closing the modal
                modalImg.style.transform = 'rotate(0deg)';
            }, 400);
        }
    }

    document.getElementById('imga').addEventListener('click', function () {
        var isSafari = navigator.vendor && navigator.vendor.indexOf('Apple') > -1 && navigator.userAgent && !navigator.userAgent.match('CriOS');
        if (isSafari != true) {
            toggleFullscreen(this);
        }
    });

});


var angelRotetion = 0;
function ImageRotate(angle) {
    if (angle == 90 || angle == -90 || angle == 270 || angle == -270) {
        $('#imga').addClass("rotatedimg");
        angelRotetion = angle;

    }
    var isFirefox = navigator.userAgent.toLowerCase().indexOf('firefox') > -1;
    if (isFirefox == true) {
        $(".changeimg").css('image-orientation', +angle + 'deg');
    } else {
        $(".changeimg").css({ 'transform': 'rotate(' + angle + 'deg)' });
        angelRotetion = angle;
    }
}


function newclickforward() {
    var isFirefox = navigator.userAgent.toLowerCase().indexOf('firefox') > -1;
    var imgsrc = $('#imga').attr('src');

    var angle = 90;
    var newangle = $("#imgangle").val();

    if (newangle == 0) {
        newangle = 90;
    } else {
        newangle = parseInt(newangle) + angle;
    }
    if (newangle == 90 || newangle == -90 || newangle == 270 || newangle == -270) {
        $('#imga').addClass("rotatedimg");
    }
    else {
        $('#imga').removeClass("rotatedimg");
    }

    if (isFirefox == true) {
        $(".changeimg").css('image-orientation', +newangle + 'deg');
    } else {
        $(".changeimg").css({ 'transform': 'rotate(' + newangle + 'deg)' });
    }

    if (newangle > -360 && newangle < 360) {
        $("#imgangle").val(newangle);
    } else {
        newangle = 0;
        $("#imgangle").val(newangle);
    }
}

function newclickbackward() {
    var isFirefox = navigator.userAgent.toLowerCase().indexOf('firefox') > -1;
    var angle = -90;
    var newangle = $("#imgangle").val();

    if (newangle == 0) {
        newangle = -90;
    } else {
        newangle = parseInt(newangle) + angle;
    }

    if (newangle == 90 || newangle == -90 || newangle == 270 || newangle == -270) {

        $('#imga').addClass("rotatedimg");
    } else {
        $('#imga').removeClass("rotatedimg");

    }

    if (isFirefox == true) {
        $(".changeimg").css('image-orientation', +newangle + 'deg');
    } else {
        $(".changeimg").css({ 'transform': 'rotate(' + newangle + 'deg)' });
    }

    if (newangle > -360 && newangle < 360) {
        $("#imgangle").val(newangle);
    } else {
        newangle = 0;
        $("#imgangle").val(newangle);

    }
}



$(document).ready(function () {
    $("select.chosen-select").select2();

});

//function getmap(currentaddess) {
//    flag = true;
//    google.maps.event.addDomListener(window, 'load', initialize(flag));
//    var address = hdnfld;
//    var geocoder = new google.maps.Geocoder();
//    var location;
//    if (address != undefined) {
//        geocoder.geocode({ 'address': address }, function (results, status) {
//            if (results.length > 0) {
//                location = results[0].geometry.location;
//                var mapOptions = {
//                    center: new google.maps.LatLng(location.lat(), location.lng()),
//                    zoom: 25,
//                    mapTypeId: google.maps.MapTypeId.SATELLITE
//                };
//                var map = new google.maps.Map(document.getElementById("map_populate"),
//                  mapOptions);
//                // create a marker
//                var latlng = new google.maps.LatLng(location.lat(), location.lng());
//                var marker = new google.maps.Marker({
//                    position: latlng,
//                    map: map,
//                    title: 'My Place'
//                });
//            }
//        });
//    }
//}

//function OnClientFileRemoved() {
//   // var progress = $('.ruUploadProgress').text();
//   // if (progress == "") {
//        var img = document.getElementById("imga");
//        var source = $('#getimage').val();
//        if (source != "") {
//            img.src = "images/sites/" + source + "";
//            //$('#ContentPlaceHolder1_getimage').val("");
//            $('#deleteimage').css('display', 'block');
//            $('#btnImageSave').css('display', 'block');

//        }
//        else {
//            img.src = "images/noimg.jpg";
//            $('#deleteimage').css('display', 'none');
//            $('#btnImageSave').css('display', 'none');
//        }
//  //  }
//};

function btndeleteyes_Click() {
    if (IsSiteStatus == "InActive") {
        CustomAlert({
            Result: "Failed", Message: "This site is InActive. This Action is not allowed."
        });
        return false;
    }
    var id = getParameterByName('id');
    var RID = getParameterByName('RID');
    var ImageName = $('#hdnimgname').val();

    $('#divdelete').css('display', 'none');
    $.ajax({
        type: "POST",
        url: "/Register/btndeleteyes_Click",
        data: JSON.stringify({ id: id, RID: RID, ImageName: ImageName }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var result = response;
            if (response.Result == "Success") {
                $('#divdelete').css('display', 'none');
                CustomAlert(response);
                ShowregisterDetails();
            } else {
                $('#divdelete').css('display', 'none');
                var Response = { Result: "Failed", Message: "Image Not Deleted." }
                CustomAlert(Response);
            }

        }
    });

}
function btndeleteno_Click() {
    $('#divdelete').css('display', 'none');
}

function deletecheck() {
    if (IsSiteStatus == "InActive") {
        CustomAlert({
            Result: "Failed", Message: "This site is InActive. This Action is not allowed."
        });
        return false;
    }
    var imageData = $('#getimage').val();
    if (imageData != "") {
        $('#divdelete').css('display', 'block');
    }
    //var green = $('.ruUploadSuccess').text();
    //if (green == "") {
    //    var a = $('.ruUploadProgress').text();
    //    if (a != "") {
    //        $('.ruCancel').click();
    //        $('.ruButton').click();
    //    }
    //    else {
    //        $('#divdelete').css('display', 'block');
    //    }
    //}
    //else {
    //    $('.ruButton').click();
    //}
}

function readURL(input) {


    var radUpdogImg = $('#radUpdogImg').val();
    var fileUpload = $('#radUpdogImg').get(0);
    var file = fileUpload.files;
    if (file.length != 0) {

        var FileName = file[0].name;
        var extension = FileName.substring(FileName.lastIndexOf('.') + 1);

        if (extension == "jpg" || extension == "jpeg" || extension == "png" || extension == "ico" || extension == "gif" || extension == "bmp" || extension == "JPG" || extension == "JPEG" || extension == "PNG" || extension == "ICO" || extension == "GIF" || extension == "BMP") {

            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    $('#imga').attr('src', e.target.result);
                    var base64mobImagedata1 = e.target.result;
                    $("#basetempimg").val(base64mobImagedata1);
                }

                reader.readAsDataURL(input.files[0]); // convert to base64 string

                $('#deleteimage').css('display', 'block');

                var hdnSaveIsOffline = $('#hdnSaveIsOffline').val();
                if (hdnSaveIsOffline == "true") {
                    $('#btnImageSave').css('display', 'none');
                    $('#downloadButton').css('display', 'none');

                }
                else {
                    $('#btnImageSave').css('display', 'block');
                    $('#downloadButton').css('display', 'none');

                }

            }
        }
        else {
            var response = { Result: "Failed", Message: "<strong>Failed to upload Image! </strong>Valid File Extensions Are jpg, jpeg, png, gif, bmp" };
            CustomAlert(response)
            $("#imga").attr("src", "/images/noimg.jpg");
        }

    } else {
        // $('#img').attr('src', "images/noimg.jpg");
        $('#deleteimage').css('display', 'none');
        $('#btnImageSave').css('display', 'none');
        $('#downloadButton').css('display', 'none');

    }

}


//function OnClientFileSelected() {

//    var radUpdogImg = $('#radUpdogImg').val();
//    var fileUpload = $('#radUpdogImg').get(0);
//    var file = fileUpload.files;
//    if (file.length != 0) {

//        var FileName = file[0].name;
//        var extension = FileName.substring(FileName.lastIndexOf('.') + 1);

//        if (extension == "jpg" || extension == "jpeg" || extension == "png" || extension == "ico" || extension == "gif" || extension == "bmp" || extension == "JPG" || extension == "JPEG" || extension == "PNG" || extension == "ICO" || extension == "GIF" || extension == "BMP") {
//            showThumbnail(file);
//        }
//        else {
//            var response = { Result: "Failed", Message: "<strong>Failed to upload Image! </strong>Valid File Extensions Are jpg, jpeg, png, gif, bmp" };
//            CustomAlert(response)
//            $("#imga").attr("src", "/images/noimg.jpg");
//        }

//    }
//}

//function showThumbnail(file) {
//    var image = document.getElementById("imga"),
//      thumbnail = document.getElementById("imga");
//    image.file = file;
//    //thumbnail.appendChild(image);

//    var reader = new FileReader()
//    reader.onload = (function (aImg) {
//        return function (e) {
//            aImg.src = e.target.result;
//        };
//    }(image))
//    //var ret = reader.readAsDataURL(file),
//    canvas = document.createElement("canvas");

//    ctx = canvas.getContext("2d");
//    image.onload = function () {
//        ctx.drawImage(image, 100, 100);
//    }
//    var MAX_WIDTH = 200,
//        MAX_HEIGHT = 100,
//        width = image.width,
//        height = image.height;
//    if (width > height) {
//        if (width > MAX_WIDTH) {
//            height *= MAX_WIDTH / width;
//            width = MAX_WIDTH;
//        }
//    } else {
//        if (height > MAX_HEIGHT) {
//            width *= MAX_HEIGHT / height;
//            height = MAX_HEIGHT;
//        }
//    }

//    canvas.width = width;
//    canvas.height = height;

//    ctx.drawImage(image, 0, 0, width, height);

//    var dataurl = canvas.toDataURL("image/png");
//}

$(function () {
    $("#txtManhrserect").on('change', function () {
        var ClientID = $("#hdnClientID").val();
        ManHrsRatioGet(ClientID);
    });
});

function ManHrsRatioGet(ClientID) {
    $.ajax({
        type: "POST",
        // url: "../WebApi/Dashboard.asmx/ClientErectRatio",
        url: "/Register/ClientErectRatio",
        data: JSON.stringify({ ClientID: ClientID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            // var Ratio = response.d;
            // var ManHrsRatio = Ratio["0"].ManHrsRatio;

            var ManHrsRatio = response;

            var ManHrsErect = $("#txtManhrserect").val();
            var ManHrsDismantle = $("#txtmanDisman").val();
            //if (ManHrsDismantle == "") {
            var ManDismantel = (ManHrsRatio) * (ManHrsErect);
            var ManHrsDismantel = ManDismantel.toFixed(2);


            $("#txtmanDisman").val(ManHrsDismantel);
            //}
        }
    });
}

function btnmeterialType_Click() {
    $('#hdnAddbtnAlert').val('');
    $('#hdnAddbtnAlert').val('MaterialType');
    $('#divAddBtnAlert').css('display', 'block');
    //var id = getParameterByName('id');
    //var UID = getParameterByName('UID');
    //var UserRole = getParameterByName('UserRole');
    //var RID = getParameterByName('RID');
    //window.location.href = '/MaterialType/MaterialType?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID;

    //  Response.Redirect("MaterialType.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RID=" + Request.QueryString["RID"]);
}

function btnDesign_Click() {
    $('#hdnAddbtnAlert').val('');
    $('#hdnAddbtnAlert').val('DesignType');
    $('#divAddBtnAlert').css('display', 'block');
    //var id = getParameterByName('id');
    //var UID = getParameterByName('UID');
    //var UserRole = getParameterByName('UserRole');
    //var RID = getParameterByName('RID');
    //window.location.href = '/DesignType/DesignType?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID;

    //  Response.Redirect("DesignType.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RID=" + Request.QueryString["RID"]);
}

function btndutyrating_Click() {
    $('#hdnAddbtnAlert').val('');
    $('#hdnAddbtnAlert').val('DutyRating');
    $('#divAddBtnAlert').css('display', 'block');
    //var id = getParameterByName('id');
    //var UID = getParameterByName('UID');
    //var UserRole = getParameterByName('UserRole');
    //var RID = getParameterByName('RID');
    //window.location.href = '/DutyRating/DutyRating?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID;

    // Response.Redirect("DutyRating.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RID=" + Request.QueryString["RID"]);
}

function btnAddnewSIte_Click() {
    $('#hdnAddbtnAlert').val('');
    $('#hdnAddbtnAlert').val('Site');
    $('#divAddBtnAlert').css('display', 'block');

    //var id = getParameterByName('id');
    // var UID = getParameterByName('UID');
    //var UserRole = getParameterByName('UserRole');
    //var RID = getParameterByName('RID');
    //window.location.href = '/EditClient/EditClient?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID ;

    // Response.Redirect("EditClient.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RID=" + Request.QueryString["RID"]);
}

function btnLocation_Click() {
    $('#hdnAddbtnAlert').val('');
    $('#hdnAddbtnAlert').val('Location');
    $('#divAddBtnAlert').css('display', 'block');
    //var id = getParameterByName('id');
    //var UID = getParameterByName('UID');
    //var UserRole = getParameterByName('UserRole');
    //var RID = getParameterByName('RID');
    //window.location.href = '/EditClient/EditClient?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID ;

    // Response.Redirect("EditClient.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RID=" + Request.QueryString["RID"]);
}

function BuldingNo_Click() {
    $('#hdnAddbtnAlert').val('');
    $('#hdnAddbtnAlert').val('Building');
    $('#divAddBtnAlert').css('display', 'block');
    //var id = getParameterByName('id');
    //var UID = getParameterByName('UID');
    //var UserRole = getParameterByName('UserRole');
    //var RID = getParameterByName('RID');
    //window.location.href = '/EditClient/EditClient?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID ;

    // Response.Redirect("EditClient.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RID=" + Request.QueryString["RID"]);
}

//function BuldingNo_Click() {
//    $('#hdnAddbtnAlert').val('');
//    $('#hdnAddbtnAlert').val('Building');
//    $('#divAddBtnAlert').css('display', 'block');

//}


function addnewWorkGroupsPopup() {
    $('#divAddBtnWorkGroupAlert').css('display', 'block');
}

function btnAlertWorkGroupsyes_Click() {
    $('#divAddBtnWorkGroupAlert').css('display', 'none');

}
function addnewSubGroupsPopup() {
    $('#divAddBtnSubGroupsAlert').css('display', 'block');
}

function btnAlertSubGroupsyes_Click() {
    $('#divAddBtnSubGroupsAlert').css('display', 'none');

}
function addnewRequestedByPopup() {
    $('#divAddBtnRequestedByAlert').css('display', 'block');
}

function btnAlertRequestedByyes_Click() {
    $('#divAddBtnRequestedByAlert').css('display', 'none');

}



function ShiftAllocation_Click() {
    $('#hdnAddbtnAlert').val('');
    $('#hdnAddbtnAlert').val('ShiftAllocation');
    $('#divAddBtnAlert').css('display', 'block');
}

function NewAssets_Click() {
    $('#hdnAddbtnAlert').val('');
    $('#hdnAddbtnAlert').val('Asset');
    $('#divAddBtnAlert').css('display', 'block');
    //var id = getParameterByName('id');
    //var UID = getParameterByName('UID');
    //var UserRole = getParameterByName('UserRole');
    //var RID = getParameterByName('RID');
    //window.location.href = '/EditClient/EditClient?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID ;

    // Response.Redirect("EditClient.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RID=" + Request.QueryString["RID"]);
}

function btnAddNewSupervisor_Click() {
    $('#hdnAddbtnAlert').val('');
    $('#hdnAddbtnAlert').val('Supervisor');
    $('#divAddBtnAlert').css('display', 'block');
    //var id = getParameterByName('id');
    //var UID = getParameterByName('UID');
    //var UserRole = getParameterByName('UserRole');
    //var RID = getParameterByName('RID');
    //window.location.href = '/UserInfo/UserInfo?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID + '&from=' + "Job";

    //   Response.Redirect("UserInfo.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RID=" + Request.QueryString["RID"] + "&from=" +"Job");
}


function btnAddFloorLevelImage_Click() {

    $('#hdnAddbtnAlert').val('');
    $('#hdnAddbtnAlert').val('FloorLevel');
    $('#divAddBtnAlert').css('display', 'block');
    //var id = getParameterByName('id');
    //var UID = getParameterByName('UID');
    //var UserRole = getParameterByName('UserRole');
    //var RID = getParameterByName('RID');
    //window.location.href = '/FloorLevelGallery/FloorLevelGallery?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID + '&from=' + "Job";

    // Response.Redirect("FloorLevelGallery.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RID=" + Request.QueryString["RID"] + "&from=" + "Job");
}
function btnview_Click() {
    if ($('#ddlChecklistInsp').val() == "") {
        //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Failed";
        //art1.Style.Add("display", "block");
        //alertlabel.Text = "<strong>Alert ! </strong> Please Select Checklist";
        var Response = { Result: "Failed", Message: "Please Select Checklist" };
        CustomAlert(Response);
    }
    else {
        var id = getParameterByName('id');
        var UID = getParameterByName('UID');
        var UserRole = getParameterByName('UserRole');
        var RID = getParameterByName('RID');

        var ddlChecklistInsp = $('#ddlChecklistInsp').val();
        var EncryptValues = Encrypt([ddlChecklistInsp]);
        ddlChecklistInsp = EncryptValues[0];

        window.location.href = '/InspectionCheckListContent/InspectionCheckListContent?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID + '&ChkID=' + ddlChecklistInsp;

        // Response.Redirect("InspectionCheckListContent?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RID=" + Request.QueryString["RID"] + "&ChkID=" + databaseCon.Encrypt(ddlChecklistInsp.SelectedValue));
    }
}

function btnAlertyes_Click() {
    $('#divAddBtnAlert').css('display', 'none');

}

function btnAlertno_Click() {

    var FlagRedirect = $('#hdnAddbtnAlert').val();
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var RID = getParameterByName('RID');
    var SiteID = $('#ddlsites').val();
    var LocationID = $('#ddlLocation').val();
    var BuildingID = $('#ddbuilding').val();
    var AssetID = $('#ddassets').val();
    var AddSite = 'AddSite';
    var AddLocation = 'AddLocation';
    var AddBuilding = 'AddBuilding';
    var AddAsset = 'AddAsset';
    var EncryptData = Encrypt([SiteID, LocationID, BuildingID, AssetID, AddSite, AddLocation, AddBuilding, AddAsset]);
    SiteID = EncryptData[0];
    LocationID = EncryptData[1];
    BuildingID = EncryptData[2];
    AssetID = EncryptData[3];
    AddSite = EncryptData[4];
    AddLocation = EncryptData[5];
    AddBuilding = EncryptData[6];
    AddAsset = EncryptData[7];

    //Commented On (15-11-2022) For Opening the Data Tabel
    //if (FlagRedirect == "Site" || FlagRedirect == "Location" || FlagRedirect == "Building" || FlagRedirect == "Asset") {
    //  window.location.href = '/EditClient/EditClient?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID;
    //}

    if (FlagRedirect == "Site") {
        window.location.href = '/EditClient/EditClient?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID + '&AddSite=' + AddSite;
    }
    else if (FlagRedirect == "Location") {
        window.location.href = '/EditClient/EditClient?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID + '&SiteID=' + SiteID + '&AddLocation=' + AddLocation;
    }
    else if (FlagRedirect == "Building") {
        window.location.href = '/EditClient/EditClient?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID + '&SiteID=' + SiteID + '&LocationID=' + LocationID + '&AddBuilding=' + AddBuilding;
    }
    else if (FlagRedirect == "Asset") {
        window.location.href = '/EditClient/EditClient?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID + '&SiteID=' + SiteID + '&LocationID=' + LocationID + '&BuildingID=' + BuildingID + '&AddAsset=' + AddAsset;
    }
    else if (FlagRedirect == "MaterialType") {
        window.location.href = '/MaterialType/MaterialType?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID;
    }
    else if (FlagRedirect == "DesignType") {
        window.location.href = '/DesignType/DesignType?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID;
    }
    else if (FlagRedirect == "DutyRating") {
        window.location.href = '/DutyRating/DutyRating?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID;
    }
    else if (FlagRedirect == "FloorLevel") {
        window.location.href = '/FloorLevelGallery/FloorLevelGallery?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID + '&from=' + "Job";
    }
    else if (FlagRedirect == "Supervisor") {
        window.location.href = '/UserInfo/UserInfo?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID + '&from=' + "Job";
    }

    //Added on (06-12-2023) to add navigation of Shift Allocation 
    else if (FlagRedirect == "ShiftAllocation") {
        window.location.href = '/ShiftAllocation/ShiftAllocation?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID + '&from=' + "Job";
    }

}



//added for Workgroup 09-Mar-2022 start here 
function RunReport() {
    $("#LoadingPanel1").css("display", "block");
    //setTimeout(function () {
    CreateDataURI();
    //btnWorkPack_Click();
    //}, 30000);

}

function CreateDataURI() {
    var canvas2 = document.getElementById("map_populate_hidden");
    html2canvas(canvas2, {
        useCORS: true,
        onrendered: function (canvas) {
            var img = canvas.toDataURL("image/jpeg,1.0");
            var Floorcanvas = document.getElementById('CanvasFloorImage_hidden');
            CreateSessionForImage("img", img);
            var img2 = Floorcanvas.toDataURL();
            CreateSessionForImage("img2", img2);
            btnWorkPack_Click();
        }
    });
}

function CreateSessionForImage(TempSessionName, TempSessionValue) {
    $.ajax({
        url: "/WorkPackReport/Index",
        async: false,
        data: { SessionName: TempSessionName, SessionValue: TempSessionValue },
        type: "POST",
        success: function (response) {
            console.log("Data Sended");
        }
    });
}

//function btnWorkPack_Click() {
//    var id = getParameterByName('id');
//    var UID = getParameterByName('UID');
//    var UserRole = getParameterByName('UserRole');
//    var RID = getParameterByName('RID');
//    var hdnTimeOffSet = $('#hdnTimeOffSetForAllPage').val();
//    var SiteID = $('#ddlsites').val();


//    $.ajax({
//        type: "POST",
//        url: "/WorkPackReport/showreport",
//        async: false,
//        data: JSON.stringify({ ClientID: id, UID: UID, UserRole: UserRole, RegisterID: RID, hdnTimeOffSet: hdnTimeOffSet, SiteID: SiteID }),
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        success: function (response) {
//            var FinalPath = response;

//            //window.open(FinalPath, '_blank');
//            window.open('/uploads/TempUpload/' + FinalPath);
//            $("#LoadingPanel1").css("display", "none");
//        }
//    })
//}
//  method copy  from inpex  for delete file after downlaod from the system on 01 marchh by ss
function btnWorkPack_Click() {
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var RID = getParameterByName('RID');
    var hdnTimeOffSet = $('#hdnTimeOffSetForAllPage').val();
    var SiteID = $('#ddlsites').val();


    $.ajax({
        type: "POST",
        url: "/WorkPackReport/showreport",
        async: false,
        data: JSON.stringify({ ClientID: id, UID: UID, UserRole: UserRole, RegisterID: RID, hdnTimeOffSet: hdnTimeOffSet, SiteID: SiteID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response.Result != "Fail") {
                var FinalPath = response.Result;
                window.open('/uploads/TempUpload/' + FinalPath);
                $("#LoadingPanel1").css("display", "none");

                if (response.Message != null && response.Message != "") {
                    alert(response.Message);
                }
                $("#LoadingPanel1").css("display", "none");
                setTimeout(function () {
                    $.ajax({
                        type: "GET",
                        url: "/WorkPackReport/DeleteLastFile",
                        async: true,
                        data: { FileName: FinalPath },
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            console.log(response)
                        }
                    });
                }, 10000);
            }
            else {

                alert(response.Message);
                $("#LoadingPanel1").css("display", "none");
            }

        }
    });
}

//added for Workgroup 09-Mar-2022 end here

//add on 31 jan 2023 by s.sourabh 
function BtnBackToFloorMap_Click() {
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var SiteID = $('#ddlsites').val();
    var LocationID = $('#ddlLocation').val();
    var BuildingID = $('#ddbuilding').val();
    var FloorLevel = $('#DropDownList1').val();
    var FromReg = "FromReg";
    var EncryptData = [SiteID, LocationID, BuildingID, FloorLevel, FromReg];
    var encryData = Encrypt(EncryptData);
    SiteID = encryData[0];
    LocationID = encryData[1];
    BuildingID = encryData[2];
    FloorLevel = encryData[3];
    FromReg = encryData[4];

    window.location.href = '/SiteFloorLevelPlan/SiteFloorLevelPlan?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&SiteID=' + SiteID + '&LocationID=' + LocationID + '&BuildingID=' + BuildingID + '&FloorLevel=' + FloorLevel + '&FromReg=' + FromReg;
}
//end

//Added on 06-12-2023 We are now creating Drop Dow of Shift Allocation Dynamically.
function ShiftAllocationDropdownBind() {
    var id = $("#hdnClientID").val();
    $.ajax({
        url: '/Register/ShiftAllocationDropdownBind',
        async: false,
        data: { id: id },
        success: function (response) {

            var ddlShiftAllocation = "<option value=''></option>";
            $.each(response, function (idx, obj) {
                if (obj != null) {
                    ddlShiftAllocation += '<option value="' + obj.ShiftAllocation + '">' + obj.ShiftAllocation + '</option>';
                }
            });

            $('#ddlShiftAllocation').empty().append(ddlShiftAllocation);
        }
    });
}
//upgrate function  for increase performance on 01-08-2024
//function CopyHistorical() {
//    $('#LoadingPanel1').show();
//    var id = getParameterByName('id');
//    var UID = getParameterByName('UID');
//    var UserRole = getParameterByName('UserRole');
//    var hdnTimeOffSet = $('#hdnTimeOffSetForAllPage').val();
//    $.ajax({
//        type: "POST",
//        url: "/RegisterList/GetRegisterListData",
//        data: JSON.stringify({ id: id, UID: UID, showCompleted: "", showCurrent: "", showBuilt1: "", showAll: false, ddlSitesLoc: "", ddlLocaSite: "", checkHideCompleted: "", hdnTimeOffSet: hdnTimeOffSet, WorkGroup: "", SubGroup: "" }),
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        async: true,
//        success: function (response) {
//            var str = "";
//            var GetData = response;
//            $('#tblRegisterList').DataTable().clear();
//            $('#tblRegisterList').DataTable().destroy();

//            if (GetData.length > 0) {
//                $("#MyHistoricalJobModal").css("display", "block");
//                for (var i = 0; i < GetData.length; i++) {
//                    var JobId = GetData[i].REGISTERId == null ? "" : GetData[i].REGISTERId;
//                    var ClientID = GetData[i].ClientID == null ? "" : GetData[i].ClientID;
//                    var Status = GetData[i].Status == null ? "" : GetData[i].Status;
//                    var ScaffRef = GetData[i].ShowRegisterID == null ? "" : GetData[i].ShowRegisterID;
//                    var ClientScaffref = GetData[i].ClientScaffref == null ? "" : GetData[i].ClientScaffref;
//                    var WONumber = GetData[i].WONumber == null ? "" : GetData[i].WONumber;
//                    var Location = GetData[i].Location == null ? "" : GetData[i].Location;
//                    var BuildingNo = GetData[i].BuildingNo == null ? "" : GetData[i].BuildingNo;
//                    var FloorLevel = GetData[i].FloorLevel == null ? "" : GetData[i].FloorLevel;
//                    var FloorLevelId = GetData[i].FloorLevelId == null ? "" : GetData[i].FloorLevelId;
//                    var AssetNo = GetData[i].AssetNo == null ? "" : GetData[i].AssetNo;
//                    var BuildingID = GetData[i].BuildingID == null ? "" : GetData[i].BuildingID;
//                    var LocationId = GetData[i].LocationId == null ? "" : GetData[i].LocationId;
//                    var SiteLocID = GetData[i].SiteLocID == null ? "" : GetData[i].SiteLocID;
//                    var DesignType = GetData[i].DesignType == null ? "" : GetData[i].DesignType;
//                    var MaterialType = GetData[i].MaterialType == null ? "" : GetData[i].MaterialType;
//                    var DutyRating = GetData[i].DutyRating == null ? "" : GetData[i].DutyRating;
//                    var DateOFDismantle = GetData[i].DateOFDismantle == null ? "" : GetData[i].DateOFDismantle;
//                    var InspectedBy = GetData[i].InspectedBy == null ? "" : GetData[i].InspectedBy;
//                    var HireDaysToDate = GetData[i].HireDaysToDate == null ? "" : GetData[i].HireDaysToDate;
//                    var TotalHireDays = GetData[i].TotalHireDays == null ? "" : GetData[i].TotalHireDays;
//                    var SAPNumber = GetData[i].SAPNumber == null ? "" : GetData[i].SAPNumber;
//                    var ScafType = GetData[i].ScafType == null ? "" : GetData[i].ScafType;
//                    var ClientRef = GetData[i].ClientRef == null ? "" : GetData[i].ClientRef;
//                    var OwnerName = GetData[i].OwnerName == null ? "" : GetData[i].OwnerName;
//                    var SiteLocation = GetData[i].SiteLocation == null ? "" : GetData[i].SiteLocation;
//                    var ScaffoldOwner = GetData[i].ScaffoldOwner == null ? "" : GetData[i].ScaffoldOwner;
//                    var discipline = GetData[i].discipline == null ? "" : GetData[i].discipline;
//                    var ManHrsErect = GetData[i].ManHrsErect == null ? "" : GetData[i].ManHrsErect;
//                    var ManHrsDismantle = GetData[i].ManHrsDismantle == null ? "" : GetData[i].ManHrsDismantle;
//                    var ShiftAllocation = GetData[i].ShiftAllocation == null ? "" : GetData[i].ShiftAllocation;
//                    var CreatedBy = GetData[i].CreatedBy == null ? "" : GetData[i].CreatedBy;
//                    var CreatedOn = GetData[i].CreatedOn == null ? "" : GetData[i].CreatedOn;
//                    var UpdatedBy = GetData[i].UpdatedBy == null ? "" : GetData[i].UpdatedBy;
//                    var UpdatedOn = GetData[i].UpdatedOn == null ? "" : GetData[i].UpdatedOn;
//                    var Length = GetData[i].Length == null ? "" : GetData[i].Length;
//                    var Width = GetData[i].Width == null ? "" : GetData[i].Width;
//                    var Height = GetData[i].Height == null ? "" : GetData[i].Height;
//                    var SiteLocationStatus = GetData[i].SiteLocationStatus == null ? "" : GetData[i].SiteLocationStatus;
//                    var WorkGroup = GetData[i].WorkGroup == null ? "" : GetData[i].WorkGroup;
//                    var SubGroup = GetData[i].SubGroup == null ? "" : GetData[i].SubGroup;
//                    var LongTermStructure = GetData[i].LongTermStructure == null ? "" : GetData[i].LongTermStructure;
//                    if (LongTermStructure == true) {
//                        LongTermStructure = "Yes";
//                    }
//                    else {
//                        LongTermStructure = "";
//                    }
//                    var LockedIn = GetData[i].LockedIn == null ? "" : GetData[i].LockedIn;
//                    if (LockedIn == true) {
//                        var LockedIn = "Yes";
//                    }
//                    else {
//                        var LockedIn = "No";
//                    }
//                    var RID = getParameterByName('RID');

//                    var DecryptData = Decrypt([RID]);
//                    RID = DecryptData[0];
//                    $('#hdnRID').val(RID);
//                    if (RID != JobId) {

//                        str += "<tr>"
//                        str += "<td style='display:none'></td>"
//                        str += "<td id='JobId_" + JobId + "' style='text-align:center;'><input type='Radio' name='JonSelected' class='ChkJobIdID' JobId_='" + JobId + "' ChkRegisterId_='" + JobId + "' ChkScaffRef_='" + ScaffRef + "'  ChkOldLocId_='" + LocationId + "'  ChkOldSiteId_='" + SiteLocID + "'  ChkOldFloorid_='" + FloorLevelId + "' /></td>"
//                        str += "<td id='Scaffref_" + JobId + "'>" + ScaffRef + "</td>"
//                        //str += "<td>" + JobId + "</td>"
//                        str += "<td>" + SiteLocation + "</td>"
//                        str += "<td>" + Location + "</td>"
//                        str += "<td>" + BuildingNo + "</td>"
//                        str += "<td>" + FloorLevel + "</td>"
//                        str += "<td>" + AssetNo + "</td>"
//                        str += "<td id='OptimizeStatus_" + JobId + "'>" + Status + "</td>"
//                        str += "</tr>"
//                    }
//                }
//                $('#LoadingPanel1').hide();
//                $('#tbodyRegisterList').append(str);
//                $('#LoadingPanel1').hide();

//                $('.ChkJobIdID').click(function () {
//                    event.stopImmediatePropagation();
//                    var RegisterId = $(this).attr('ChkRegisterId_').split('_')[0];
//                    var OldLocId = $(this).attr('ChkOldLocId_').split('_')[0];
//                    var OldSiteId = $(this).attr('ChkOldSiteId_').split('_')[0];
//                    var OldFloorid = $(this).attr('ChkOldFloorid_').split('_')[0];
//                    var ScaffReff = $(this).attr('ChkScaffRef_').split('_')[0];
//                    $("#divCopyHistPopup").css('display', 'block');
//                    $("#lblMessagecopyhist").html('<strong>Are you sure you want to copy Scaf Ref = ' + ScaffReff + ' ?<strong>');
//                    $("#hiddenOldRegid").val(RegisterId);
//                    $("#hiddenOldFloorid").val(OldFloorid);
//                    $("#hiddenOldSiteId").val(OldSiteId);
//                    $("#hiddenOldLocId").val(OldLocId);
//                })
//            }
//            $('#LoadingPanel1').hide();
//            table = $('#tblRegisterList').DataTable({
//                "bSort": false,
//                "processing": true,
//                searching: true,
//                "bLengthChange": false,
//                "bInfo": false,
//                autoWidth: true,
//                scrollX: true,
//                scrollCollapse: true,
//                scrollY: "60vh",
//                "paging": true,
//                pageLength: 15,
//                "ordering": false,
//                "info": false
//            });
//        }
//    })
//}
function CopyHistorical() {
    $('#LoadingPanel1').show();
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var hdnTimeOffSet = $('#hdnTimeOffSetForAllPage').val();
    var RID = getParameterByName('RID');

    var DecryptData = Decrypt([RID]);
    RID = DecryptData[0];
    $.ajax({
        type: "POST",
        url: "/RegisterList/GetRegisterListData",
        data: JSON.stringify({ id: id, UID: UID, showCompleted: "", showCurrent: "", showBuilt1: "", showAll: false, ddlSitesLoc: "", ddlLocaSite: "", checkHideCompleted: "", hdnTimeOffSet: hdnTimeOffSet, WorkGroup: "", SubGroup: "", Supervisor: "", BuildingID: "", IsMasterJob :""}),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            var str = "";
            var GetData = response;
            $('#tblRegisterList').DataTable().clear();
            $('#tblRegisterList').DataTable().destroy();

            if (GetData.length > 0) {
                $("#MyHistoricalJobModal").css("display", "block");
                for (var i = 0; i < GetData.length; i++) {


                    var JobId = GetData[i].REGISTERId == null ? "" : GetData[i].REGISTERId;
                    if (RID != JobId) {
                        var Status = GetData[i].Status == null ? "" : GetData[i].Status;
                        var ScaffRef = GetData[i].ShowRegisterID == null ? "" : GetData[i].ShowRegisterID;
                        var Location = GetData[i].Location == null ? "" : GetData[i].Location;
                        var BuildingNo = GetData[i].BuildingNo == null ? "" : GetData[i].BuildingNo;
                        var FloorLevel = GetData[i].FloorLevel == null ? "" : GetData[i].FloorLevel;
                        var FloorLevelId = GetData[i].FloorLevelId == null ? "" : GetData[i].FloorLevelId;
                        var AssetNo = GetData[i].AssetNo == null ? "" : GetData[i].AssetNo;
                        var LocationId = GetData[i].LocationId == null ? "" : GetData[i].LocationId;
                        var SiteLocation = GetData[i].SiteLocation == null ? "" : GetData[i].SiteLocation;
                        var SiteLocID = GetData[i].SiteLocID == null ? "" : GetData[i].SiteLocID;
                        str += "<tr>"
                        str += "<td style='display:none'></td>"
                        str += "<td id='JobId_" + JobId + "' style='text-align:center;'><input type='Radio' name='JonSelected' class='ChkJobIdID' JobId_='" + JobId + "' ChkRegisterId_='" + JobId + "' ChkScaffRef_='" + ScaffRef + "'  ChkOldLocId_='" + LocationId + "'  ChkOldSiteId_='" + SiteLocID + "'  ChkOldFloorid_='" + FloorLevelId + "' /></td>"
                        str += "<td id='Scaffref_" + JobId + "'>" + ScaffRef + "</td>"
                        str += "<td>" + SiteLocation + "</td>"
                        str += "<td>" + Location + "</td>"
                        str += "<td>" + BuildingNo + "</td>"
                        str += "<td>" + FloorLevel + "</td>"
                        str += "<td>" + AssetNo + "</td>"
                        str += "<td id='OptimizeStatus_" + JobId + "'>" + Status + "</td>"
                        str += "</tr>"
                    }
                }
                $('#LoadingPanel1').hide();
                $('#tbodyRegisterList').append(str);

                $('.ChkJobIdID').click(function () {
                    event.stopImmediatePropagation();
                    var RegisterId = $(this).attr('ChkRegisterId_').split('_')[0];
                    var OldLocId = $(this).attr('ChkOldLocId_').split('_')[0];
                    var OldSiteId = $(this).attr('ChkOldSiteId_').split('_')[0];
                    var OldFloorid = $(this).attr('ChkOldFloorid_').split('_')[0];
                    var ScaffReff = $(this).attr('ChkScaffRef_').split('_')[0];

                    $.ajax({
                        type: 'Post',
                        url: '/Register/CheckIsPipeLineJob',
                        data: JSON.stringify({ id: id, UID: UID, RegisterId: RegisterId }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (responce) {
                            if (responce.Result == true) {
                                $('input[name="SelectJobType"][value="1"]').prop('checked', true).click();
                            } else {
                                $('input[name="SelectJobType"][value="0"]').prop('checked', true).click();
                            }

                            // Destroy the response after use
                            responce = null;
                        }
                    });
                    $("#divCopyHistPopup").css('display', 'block');
                    $("#lblMessagecopyhist").html('<h3><strong>Are you sure you want to copy Scaf Ref # ' + ScaffReff + ' ?</h3><strong>');
                    $("#hiddenOldRegid").val(RegisterId);
                    $("#hiddenOldFloorid").val(OldFloorid);
                    $("#hiddenOldSiteId").val(OldSiteId);
                    $("#hiddenOldLocId").val(OldLocId);
                });
            }
            $('#LoadingPanel1').hide();
            table = $('#tblRegisterList').DataTable({
                "bSort": false,
                "processing": true,
                searching: true,
                "bLengthChange": false,
                "bInfo": false,
                autoWidth: true,
                scrollX: true,
                scrollCollapse: true,
                scrollY: "60vh",
                "paging": true,
                pageLength: 15,
                "ordering": false,
                "info": false
            });
        }
    })
}
function btnCANCEL_Click() {

    $("#MyHistoricalJobModal").css("display", "none");
    ('#tblRegisterList').DataTable().clear();
    $('#tblRegisterList').DataTable().destroy();
}
//function btncopyhistNoClick() {
//    $("#divCopyHistPopup").css('display', 'none');
//    $("#lblMessagecopyhist").val('');
//    $("#hiddenRegid").val('');
//    $("#CheckInvnt").prop('checked', false);
//}
function btncopyhistNoClick() {
    $("#lblMessagecopyhist").val('');
    $("#hiddenRegid").val('');
    $("#WithInventoryForPipelineJob").prop('checked', false);
    $("#WithInventoryForLiveJob").prop('checked', false);
    var today = new Date();

    $("#PipelineStartDate").val(today.DateMonthFormat());
    $("#BuildDate").val(today.DateMonthFormat());
    $("#divCopyHistPopup").css('display', 'none');


}
function btncopyhistYesClick() {

    $('#LoadingPanel1').show();
    var OldRegisterid = $("#hiddenOldRegid").val();
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var OldSiteId = $('#hiddenOldSiteId').val();
    var OldFloorid = $('#hiddenOldFloorid').val();
    var OldLocId = $('#hiddenOldLocId').val();
    var CheckInvnt = "";
    var NewRegistId = getParameterByName('RID');
    var NewSiteLocId = $("#ddlsites").val();
    var NewFloorLevel = $("#DropDownList1").val();
    var NewLocationId = $("#ddlLocation").val();

    //if ($('#CheckInvnt').is(":checked")) {
    //    CheckInvnt = 1;
    //}
    //else {
    //    CheckInvnt = 0;
    //}
    var SelectLiveJobOrPipelineJob = $('input[name="SelectJobType"]:checked').val();
    var startdate = String.empty;
    if (SelectLiveJobOrPipelineJob == "0") {
        startdate = $('#BuildDate').val();
        if ($('#WithInventoryForLiveJob').is(":checked")) {
            CheckInvnt = 1;
        }
        else {
            CheckInvnt = 0;
        }
    }
    else {
        startdate = $('#PipelineStartDate').val();
        if ($('#WithInventoryForPipelineJob').is(":checked")) {
            CheckInvnt = 1;
        }
        else {
            CheckInvnt = 0;
        }

    }

    $.ajax({
        type: 'Post',
        url: '/Register/btnCopyHistorical',
        data: JSON.stringify({ id: id, UID: UID, UserRole: UserRole, OldFloorid: OldFloorid, OldSiteId: OldSiteId, OldLocId: OldLocId, OldRegisterid: OldRegisterid, CheckInvnt: CheckInvnt, NewRegistId: NewRegistId, NewSiteLocId: NewSiteLocId, NewFloorLevel: NewFloorLevel, NewLocationId: NewLocationId, SelectLiveJobOrPipelineJob: SelectLiveJobOrPipelineJob, startdate: startdate }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            //var Response = { Result: response.Result, Message: response.Message };
            //CustomAlert(Response);

            var results = response.Result
            var messages = response.Message
            RegistId = response.NewRegId
            if (results == "Success") {
                var Response = { Result: results, Message: messages };
                CustomAlert(Response);

                setTimeout(function () {
                    if (RegistId != "" && RegistId != null && RegistId != 0) {
                        var ClientID = getParameterByName('id');
                        var UID = getParameterByName('UID');
                        var UserRole = getParameterByName('UserRole');

                        var EncryptData = [RegistId];
                        var encryData = Encrypt(EncryptData);
                        RegistId = encryData[0];

                        window.location.href = '/Register/Register?UID=' + UID + '&UserRole=' + UserRole + '&id=' + ClientID + '&RID=' + RegistId;
                    }
                    else {
                        $('#LoadingPanel1').hide();
                        CustomAlert('Failed', 'Syntax Error Contact Admin');
                    }
                }, 6000); // Set the time interval in milliseconds
            }
            else if (results == "Insufficient") {
                console.log("Results are Insufficient");
                showCustomAlert(messages);

                // Hide the popup and reset the table
                $("#divCopyHistPopup").css('display', 'none');
                $('#tblRegisterList').DataTable().clear();
                $('#tblRegisterList').DataTable().destroy();
                $("#MyHistoricalJobModal").css("display", "none");
                $('#LoadingPanel1').css("display", "none");

                // Uncheck the checkboxes
                $('#WithInventoryForLiveJob').prop('checked', false);
                $('#WithInventoryForPipelineJob').prop('checked', false);
                $('input[name="SelectJobType"]:checked').prop('checked', false);

                // Set the dates in the input fields
                var today = new Date();
                $("#PipelineStartDate").val(today.DateMonthFormat());
                $("#BuildDate").val(today.DateMonthFormat());
            }


            else {
                var Response = { Result: results, Message: messages };
                CustomAlert(Response);
                $("#divCopyHistPopup").css('display', 'none');
                $('#tblRegisterList').DataTable().clear();
                $('#tblRegisterList').DataTable().destroy();
                $("#MyHistoricalJobModal").css("display", "none");
                //$('#LoadingPanel1').hide();
                $('#LoadingPanel1').css("display", "none");
                $('#WithInventoryForLiveJob').prop('checked', false);
                $('#WithInventoryForPipelineJob').prop('checked', false);
                $('input[name="SelectJobType"]:checked').prop('checked', false);
                var today = new Date();

                $("#PipelineStartDate").val(today.DateMonthFormat());
                $("#BuildDate").val(today.DateMonthFormat());
            }
        }
    })
}


function downloadImage() {
    var registerId = getParameterByName('RID');
    window.location.href = '/Register/DownloadImage?registerId=' + registerId;

    Res = { Result: "Success", Message: "File downloaded Successfully" };
    CustomAlert(Res);
    $('.selectFile').prop('selected', 'selected');

}


function GetVariationHours() {
    $("#LoadingPanel1").css("display", "block");
    var RegisterId = getParameterByName('RID');
    $.ajax({
        url: '/Register/GetVariationHours',
        type: 'POST',
        data: { RegisterId: RegisterId },
        success: function (response) {
            var totalVariationInHrs = 0;
            var str = "";
            str += '<div style="margin-bottom: 10px;">';
            str += '<button id="btnAddRecord" class="btn btn-primary" style="margin-top: 10px;margin-left: 10px;"> <i class="fa fa-plus"></i> New Record</button>';
            str += '</div>';
            str += '<table id="gridMaterialType" class="table table-striped display" style="width:100%;table-layout:fixed;font-size:12px; margin-bottom: 20px;">';
            str += "<thead><tr>";
            str += "<th style='width:3%;padding: 0px;' class='ClassbtnMaterialTypeEdit'></th>";
            str += "<th style='width:3%;padding: 0px;' class='ClassbtnMaterialTypeDelete'></th>";
            str += "<th style='text-align:center;display:none'>WorkCarriedOutVariationId</th>";
            /* str += "<th style='text-align:left;width:20%'>Work Carried Out<span style="color: red">&nbsp*</span></th>";*/
            str += "<th style='text-align:left;width:20%'>Work Carried Out<span style='color: red'>&nbsp*</span></th>";
            /* str += "<th style='text-align:center;width:20%'>Variation (In Hours)<span style="color: red">&nbsp*</span></th>";*/
            str += "<th style='text-align:center;width:20%'>Variation (In Hours)<span style='color: red'>&nbsp*</span></th>";
            str += "<th style='text-align:center;width:20%'>Entered By</th>";
            str += "<th style='text-align:center;width:20%'>Date Work Carried Out</th>";
            str += "</tr></thead><tbody>";

            if (response.length > 0) {
                for (var i = 0; i < response.length; i++) {
                    var workCarriedOutVariationId = response[i].WorkCarriedOutVariation || "";
                    var workCarriedOut = response[i].WorkCarriedOut || "";
                    var variationInHrs = response[i].VariationInHrs || 0; // Default to 0 if no hours provided
                    var createdBy = response[i].CreatedBy || "";
                    var createdOn = response[i].CreatedOn ? (response[i].CreatedOn) : "";
                    var workCarriedOutDate = response[i].WorkCarriedOutDate ? DateFormat(response[i].WorkCarriedOutDate) : "";
                    totalVariationInHrs += parseFloat(variationInHrs);

                    str += "<tr data-id='" + workCarriedOutVariationId + "'>";
                    str += "<td style='width:3%;' class='ClassbtnMaterialTypeEdit'><a class='edit fa fa-pencil'></a></td>";
                    str += "<td style='width:3%;' class='ClassbtnMaterialTypeDelete'><a class='fa fa-trash'></a></td>";
                    str += "<td style='text-align:center;display:none'>" + workCarriedOutVariationId + "</td>";
                    str += "<td style='text-align:left;width:20%' class='editableWorkCarriedOut'>" + workCarriedOut + "</td>";
                    str += "<td style='text-align:center;width:20%' class='editableVariationInHrs '>" + variationInHrs + "</td>";
                    str += "<td style='text-align:center;width:20%'>" + createdBy + "</td>";
                    str += "<td style='text-align:center;width:20%' class='editableVariationWorkCarriedOutDate'>" + workCarriedOutDate + "</td>";
                    str += "</tr>";
                }
            } else {
                str += "<tr><td colspan='7' style='text-align:center;'>No data found.</td></tr>";
            }
            str += "</tbody>";
            str += "<tfoot>";
            str += "<tr>";
            str += "<td colspan='3' style='text-align:right;'><strong>Total Variation In Hours:</strong></td>";
            str += "<td style='text-align:center;'><strong>" + totalVariationInHrs.toFixed(2) + "</strong></td>";
            str += "<td colspan='2'></td>";
            str += "</tr>";
            str += "</tfoot>";

            str += "</table>";
            $("#loaddata").html(str);
            $("#LoadingPanel1").css("display", "none");
            $("#variationHoursModal").css("display", "block");
            var isEditMode = false;
            $(".edit").click(function () {
                if (IsSiteStatus == "InActive") {
                    CustomAlert({
                        Result: "Failed", Message: "This site is InActive. This Action is not allowed."
                    });
                    return false;
                }
                event.preventDefault();
                hideModalAlert();
                var $row = $(this).closest('tr');
                var workCarriedOut = $row.find('.editableWorkCarriedOut').text();
                var variationInHrs = $row.find('.editableVariationInHrs').text();
                var workCarriedOutDate = $row.find('.editableVariationWorkCarriedOutDate').text();
                var rowId = $row.data('id');

                isEditMode = true;
                openModal(true, {
                    WorkCarriedOut: workCarriedOut,
                    VariationInHrs: variationInHrs,
                    WorkCarriedOutDate: workCarriedOutDate,
                    WorkCarriedOutVariationId: rowId
                });

            });

            $("#btnAddRecord").click(function (event) {
                if (IsSiteStatus == "InActive") {
                    CustomAlert({
                        Result: "Failed", Message: "This site is InActive. This Action is not allowed."
                    });
                    return false;
                }
                event.preventDefault();
                openModal(false);
            });

            $("#btnSaveRecord").click(function () {
                if (IsSiteStatus == "InActive") {
                    CustomAlert({
                        Result: "Failed", Message: "This site is InActive. This Action is not allowed."
                    });
                    return false;
                }
                event.preventDefault();
                var workCarriedOut = $("#newWorkCarriedOut").val().trim();
                var variationInHrs = $("#newVariationInHrs").val().trim();
                var workCarriedOutDate = $("#newWorkCarriedOutDate").val();
                var rowId = $(this).attr('data-id');

                var errorMessage = validateForm(workCarriedOut, variationInHrs, workCarriedOutDate);
                if (errorMessage !== "") {
                    CustomAlert({ Result: "Failed", Message: errorMessage });
                    return;
                }

                if (rowId) {
                    saveEditedData(rowId, workCarriedOut, variationInHrs, workCarriedOutDate);
                } else {
                    savevariationrecord();
                }
                closeModal();
            });
            $("#btnAddRecord").click(function () {
                if (IsSiteStatus == "InActive") {
                    CustomAlert({
                        Result: "Failed", Message: "This site is InActive. This Action is not allowed."
                    });
                    return false;
                }
                openModal(false);
            });

            $(".fa-trash").click(function () {
                if (IsSiteStatus == "InActive") {
                    CustomAlert({
                        Result: "Failed", Message: "This site is InActive. This Action is not allowed."
                    });
                    return false;
                }
                var $row = $(this).closest('tr');
                var workCarriedOutVariationId = $row.data('id');
                $.ajax({
                    url: '/Register/DeleteVariationHours',
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ WorkCarriedOutVariationId: workCarriedOutVariationId }),
                    success: function (response) {
                        if (response.success) {
                            var Response = { Result: "Success", Message: "Data deleted successfully!" };
                            CustomAlert(Response);
                            GetVariationHours();
                            $row.remove();
                        } else {
                            alert("Error deleting record: " + response.message);
                        }
                    },
                    error: function (xhr, status, error) {
                        console.error("Error deleting record:", status, error);
                    }
                });

            });
            $("#btnAddRecord").click(function () {
                if (IsSiteStatus == "InActive") {
                    CustomAlert({
                        Result: "Failed", Message: "This site is InActive. This Action is not allowed."
                    });
                    return false;
                }
                $("#addRecordModal").css("display", "block");
            });
        },
        error: function (xhr, status, error) {
            console.error("Error fetching data:", status, error);
            $("#LoadingPanel1").css("display", "none");
        }
    });
}
function savevariationrecord() {
    var workCarriedOut = $("#newWorkCarriedOut").val().trim();
    var variationInHrs = $("#newVariationInHrs").val().trim();
    var workCarriedOutDate = $("#newWorkCarriedOutDate").val();
    var validWorkCarriedOutRegex = /^[a-zA-Z0-9_. ]+$/;
    hideModalAlert();

    if (workCarriedOut === "" && variationInHrs === "") {
        showModalAlert("Please fill in both the 'Work Carried Out' and 'Variation In Hours' fields.");
        return;
    }

    if (workCarriedOut === "") {
        showModalAlert("Please fill in the 'Work Carried Out' field.");
        return;
    }

    if (variationInHrs === "") {
        showModalAlert("Please fill in the 'Variation In Hours' field.");
        return;
    }

    var variationInHrsNum = parseFloat(variationInHrs);
    if (isNaN(variationInHrsNum) || variationInHrsNum <= 0) {
        showModalAlert("The 'Variation In Hours' should be greater than 0.");
        $("#newVariationInHrs").val('');
        return;
    }

    if (!validWorkCarriedOutRegex.test(workCarriedOut)) {
        showModalAlert("The 'Work Carried Out' field contains invalid characters. Only alphanumeric characters, decimals and underscores are allowed.");
        return;
    }

    var RegisterId = getParameterByName('RID');
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    $.ajax({
        url: '/Register/AddVariationHours',
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({
            WorkCarriedOut: workCarriedOut,
            VariationInHrs: variationInHrs,
            RegisterId: RegisterId,
            id: id,
            UID: UID,
            WorkCarriedOutDate: workCarriedOutDate

        }),
        success: function (response) {
            if (response.success) {

                var newRow = "<tr data-id='" + response.id + "'>";
                newRow += "<td style='width:3%;' class='ClassbtnMaterialTypeEdit'><a class='edit fa fa-pencil'></a></td>";
                newRow += "<td style='width:3%;' class='ClassbtnMaterialTypeDelete'><a class='fa fa-trash'></a></td>";
                newRow += "<td style='text-align:left;width:20%' class='editableWorkCarriedOut'>" + response.workCarriedOut + "</td>";
                newRow += "<td style='text-align:center;width:20%' class='editableVariationInHrs'>" + response.variationInHrs + "</td>";
                newRow += "</tr>";
                $("#gridMaterialType tbody").append(newRow);
                $("#newWorkCarriedOut").val('');
                $("#newVariationInHrs").val('');
                $("#newWorkCarriedOutDate").val('');
                GetVariationHours();

                $("#addRecordModal").css("display", "none");
            } else {
                showModalAlert("Error adding new record: " + response.message);
            }
        },
        error: function (xhr, status, error) {
            console.error("Error adding new record:", status, error);
            showModalAlert("Error adding new record: " + error);
        }
    });
}

function closeModal() {
    $("#variationHoursModal").css("display", "none");
}

function CancelNewRecordBtn() {
    $("#addRecordModal").css("display", "none");
    $("#modalAlert").css("display", "none");
};

$(function () {
    const today = new Date();
    const formattedDate = today.toISOString().split('T')[0];
    $("#newWorkCarriedOutDate").val(formattedDate);

    $(".calendar-icon").on("click", function () {
        $("#newWorkCarriedOutDate").datepicker("show");
    });
});

function saveEditedData(rowId, updatedWorkCarriedOut, updatedVariationInHrs, workCarriedOutDate) {
    $("#addRecordModal").css("display", "Block");
    var UID = getParameterByName('UID');
    var workCarriedOut = $("#newWorkCarriedOut").val().trim();
    var variationInHrs = $("#newVariationInHrs").val().trim();
    var workCarriedOutDate = $("#newWorkCarriedOutDate").val();
    var validWorkCarriedOutRegex = /^[a-zA-Z0-9_. ]+$/;

    hideModalAlert();

    if (workCarriedOut === "" && variationInHrs === "") {
        showModalAlert("Please fill in both the 'Work Carried Out' and 'Variation In Hours' fields.");
        return;
    }

    if (workCarriedOut === "") {
        showModalAlert("Please fill in the 'Work Carried Out' field.");
        return;
    }

    if (variationInHrs === "") {
        showModalAlert("Please fill in the 'Variation In Hours' field.");
        return;
    }

    var variationInHrsNum = parseFloat(variationInHrs);
    if (isNaN(variationInHrsNum) || variationInHrsNum <= 0) {
        showModalAlert("The 'Variation In Hours' should be greater than 0.");
        $("#newVariationInHrs").val('');
        return;
    }

    if (!validWorkCarriedOutRegex.test(workCarriedOut)) {
        showModalAlert("The 'Work Carried Out' field contains invalid characters. Only alphanumeric characters, decimals  and underscores are allowed.");
        return;
    }
    $.ajax({
        url: '/Register/UpdateVariationHours',
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({
            WorkCarriedOutVariationId: rowId,
            WorkCarriedOut: updatedWorkCarriedOut,
            VariationInHrs: updatedVariationInHrs,
            WorkCarriedOutDate: workCarriedOutDate,
            UID: UID
        }),
        success: function (response) {
            if (response.success) {
                var Response = { Result: "Success", Message: "Data updated successfully!" };
                CustomAlert(Response);
                GetVariationHours();
            } else {
                var Response = { Result: "Failed", Message: "Error saving data: " + response.message };
                CustomAlert(Response);
            }
            $("#addRecordModal").css("display", "none");
        },
        error: function (xhr, status, error) {
            console.error("Error saving data:", status, error);
            var Response = { Result: "Failed", Message: "Error saving data. Please try again." };
            CustomAlert(Response);
        }
    });
}
var isEditMode = false;
function openModal(isEdit, data = {}) {
    $("#addRecordModal").css("display", "block");
    if (isEdit) {
        $("#modalHeader").html('<i class="fa fa-pencil-square-o" aria-hidden="true"></i> Edit Record');
        $("#newWorkCarriedOut").val(data.WorkCarriedOut || '');
        $("#newVariationInHrs").val(data.VariationInHrs || '');
        $("#newWorkCarriedOutDate").val(data.WorkCarriedOutDate || '');
        $("#saveNewRecordBtn").text("Update");
        $("#saveNewRecordBtn").attr('data-id', data.WorkCarriedOutVariationId);

        console.log('Editing record with ID:', data.WorkCarriedOutVariationId);

        $("#saveNewRecordBtn").off("click").on("click", function () {
            var rowId = $(this).attr('data-id');
            var updatedWorkCarriedOut = $("#newWorkCarriedOut").val();
            var updatedVariationInHrs = $("#newVariationInHrs").val();
            var workCarriedOutDate = $("#newWorkCarriedOutDate").val();
            console.log('Update button clicked with ID:', rowId);
            console.log('Updated values:', updatedWorkCarriedOut, updatedVariationInHrs, workCarriedOutDate);
            saveEditedData(rowId, updatedWorkCarriedOut, updatedVariationInHrs, workCarriedOutDate);
        });
    } else {
        $("#modalHeader").html('<i class="fa fa-plus-circle" aria-hidden="true"></i> Add New Record');
        $("#newWorkCarriedOut").val('');
        $("#newVariationInHrs").val('');

        var today = new Date();

        $("#newWorkCarriedOutDate").val(today.DateMonthFormat());

        /*  $("#newWorkCarriedOutDate").val(formattedDate);*/
        $("#saveNewRecordBtn").text("Save");
        $("#saveNewRecordBtn").removeAttr('data-id');

        console.log('Adding new record');

        $("#saveNewRecordBtn").off("click").on("click", function () {
            console.log('Save button clicked');
            savevariationrecord();
        });
    }
}

function showModalAlert(message) {
    var modalAlert = $("#modalAlert");
    modalAlert.text(message);
    modalAlert.show();
}
function hideModalAlert() {
    var modalAlert = $("#modalAlert");
    modalAlert.hide();
}
$("#btnAddRecord").click(function () {
    if (IsSiteStatus == "InActive") {
        CustomAlert({
            Result: "Failed", Message: "This site is InActive. This Action is not allowed."
        });
        return false;
    }
    hideModalAlert();
    openModal(false);
});


function btnGoOfflineClick() {
    $('#GetOfflineModal').show();

}
function btnGoOfflineYesClick() {
    $('#GetOfflineModal').show();
}
function btnGoOfflineNoClick() {
    $('#GetOfflineModal').hide();
}


function btnofflineconfirm_Click() {
    $('#LoadingPanel1').show();
    var Siteid = $('#ddlsites').val();
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var RegisterID = getParameterByName('RID');

    $.ajax({
        type: 'Post',
        url: '/Register/CheckuserUserisOfflineMode',
        data: JSON.stringify({ id: id, UID: UID, Siteid: Siteid, RegisterID: RegisterID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            if (response.VerifyedUser == 1) {
                $.ajax({
                    type: 'Post',
                    url: '/Register/btnofflineconfirmYes_Click',
                    data: JSON.stringify({ id: id, UID: UID, UserRole: UserRole, Siteid: Siteid, RegisterID: RegisterID }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true,
                    success: function (response) {
                        var GetData = response;
                        if (response.Result == "Success") {

                            $('#GetOfflineModal').css('display', 'none');
                            console.log(response.Link)
                            window.location.href = response.Link;
                        } else {
                            $('#LoadingPanel1').hide();
                            $('#GetOfflineModal').css('display', 'none');
                            CustomAlert(response);
                        }
                    },
                    error: function (x, y, z) {
                        $('#LoadingPanel1').hide();
                        $('#GetOfflineModal').css('display', 'none');
                        alert(z);
                    }
                });
            } else if (response.UserIsOnOfflineInspectionAlso) {
                var ress = {
                    Result: "Failed", Message: "A user is only allowed to go offline either on inspection or on a Job at any given time. Please upload inspection to cloud before going offline with Job."
                }
                CustomAlert(ress);
                $('#GetOfflineModal').hide();
                $('#LoadingPanel1').hide();

            }
            else if (response.JobIsCompleted) {
                var ress = {
                    Result: "Failed", Message: "User is not allowed to take Completed Jobs Offline."
                }
                CustomAlert(ress);
                $('#GetOfflineModal').hide();
                $('#LoadingPanel1').hide();

            }
            else if (response.UserIsInspector) {
                var ress = {
                    Result: "Failed", Message: "Data cannot be taken offline as the loggined User is not an Inspector, Kindly Contact administrator ."
                }
                CustomAlert(ress);
                $('#GetOfflineModal').hide();
                $('#LoadingPanel1').hide();

            }
            else if (response.ScaffoldInspectionList) {
                var ress = {
                    Result: "Failed", Message: "Please Select and Save the Scaffold Inspection List  before going Offline."
                }
                CustomAlert(ress);
                $('#GetOfflineModal').hide();
                $('#LoadingPanel1').hide();

            }
            else if (response.JobHasInventory) {
                var ress = {
                    Result: "Failed", Message: "You Already have Job Inventory for this job. You can go offline only with new jobs.."
                }
                CustomAlert(ress);
                $('#GetOfflineModal').hide();
                $('#LoadingPanel1').hide();

            }

            else {
                var res = {
                    Result: "Failed", Message: response.Message
                }
                CustomAlert(res);
                $('#GetOfflineModal').hide();
                $('#LoadingPanel1').hide();
                alert(response.Message)

            }
        },
        error: function (x, y, z) {
            $('#LoadingPanel1').hide();
            $('#GetOfflineModal').css('display', 'none');
            alert(z);
        }
    });

}
function BringDataFromOfflineToOnline() {
    // Step 1: Extract the link from the URL
    const currentUrl = window.location.href;
    // Step 2: Retrieve the domain name, ID, and userRole from URL parameters
    const urlObj = new URL(currentUrl);
    var ClientId = getParameterByName('id');
    var RID = getParameterByName('RID');
    var Siteid = $('#ddlsites').val();
    var encryData = Encrypt([Siteid]);
    Siteid = encryData[0];
    var UserId = urlObj.searchParams.get("UID");
    $.ajax({
        type: 'Post',
        url: '/Register/UpdateOfflineInspectionFlag',
        async: false,
        data: { LoginUserId: UserId, clientId: ClientId, Siteid: Siteid, RID: RID },
        success: function (response) {
            if (response.Result == 'Success') {
                var Res = { Result: "Success", Message: response.Message }
                CustomAlert(Res);
                window.location.reload();
            }
            else {
                var Res = {
                    //    Result: "Failed", Message: "Moving back from Offline to Online failed.Please contact Admin........."
                    Result: "Failed", Message: response.Message
                }
                CustomAlert(Res);
            }

        }
    });
}


function btnClearOfflineJobData() {
    var Siteid = $('#ddlSitesLoc').val();
    if (Siteid != '-1') {
        $('#clearOfflineDatainspectionModal').show();

    }
    else {
        var response = { Result: "Failed", Message: "Please Select Site." }
        CustomAlert(response);
        $('#GetOfflineModal').modal('hide');
        $('#LoadingPanel1').hide();
    }
}
function BtnClerdataClickyes() {
    $('#clearOfflineDatainspectionModal').hide();
    BringDataFromOfflineToOnline();
}
function BtnClerdataClickNo() {
    $('#clearOfflineDatainspectionModal').hide();
}
function isValidDate(dateStr) {
    const regex = /^(0[1-9]|[12]\d|3[01])-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)-\d{4}$/;
    return regex.test(dateStr);
}

function BtnBackToOffSystemInspection_Click() {
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var SiteId = getParameterByName('SiteId');
    window.location.href = '/OffSystemInspection/OffSystemInspection?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&SiteId=' + SiteId;
}

function btnCloseOutJob_Click() {
    if ($('#hdnRID').val() == "0") {
        var Response = { Result: "Failed", Message: "Please Save the Job first before Closing Out." };
        CustomAlert(Response);
        return;
    }

    $('#txtCloseOutDismantleDate').datepicker('destroy');
    $('#txtCloseOutDismantleDate').datepicker({
        dateFormat: 'dd-M-yy',
        maxDate: 0,   // calendar mein future dates disable
        closeText: 'Close',
        onSelect: function (dateText) {
            var selected = $.datepicker.parseDate('dd-M-yy', dateText);
            var today = new Date();
            today.setHours(0, 0, 0, 0);
            selected.setHours(0, 0, 0, 0);
            if (selected > today) {
                $(this).val('');
            }
        }
    });

    // Keyboard se manual typing block, sirf calendar click allowed
    $('#txtCloseOutDismantleDate').off('keydown').on('keydown', function (e) {
        e.preventDefault();
    });

    var today = new Date();
    $('#txtCloseOutDismantleDate').val($.datepicker.formatDate('dd-M-yy', today));
    $('#divCloseOutJob').css('display', 'block');
}
function btnCloseOutJobCancel_Click() {
    $('#divCloseOutJob').css('display', 'none');
}

function btnCloseOutJobConfirm_Click() {
    var id = getParameterByName('id');
    var RID = getParameterByName('RID');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var DismantleDate = $('#txtCloseOutDismantleDate').val();
    var hdnTimeOffSet = $('#hdnTimeOffSetForAllPage').val();

    if (DismantleDate == "") {
        var Response = { Result: "Failed", Message: "Please select Dismantle Date." };
        CustomAlert(Response);
        return;
    }

    var selectedDate = new Date(DismantleDate);
    var todayDate = new Date();
    todayDate.setHours(0, 0, 0, 0);
    selectedDate.setHours(0, 0, 0, 0);

    //if (selectedDate > todayDate) {
    //    var Response = { Result: "Failed", Message: "Future date is not allowed for Dismantle Date." };
    //    CustomAlert(Response);
    //    return;
    //}

    $('#divCloseOutJob').css('display', 'none');
    $('#LoadingPanel1').show();

    $.ajax({
        type: "POST",
        url: "/Register/CloseOutJob",
        data: JSON.stringify({ id: id, RID: RID, UID: UID, DismantleDate: DismantleDate, hdnTimeOffSet: hdnTimeOffSet }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            $('#LoadingPanel1').hide();
            CustomAlert(response);
            if (response.Result == "Success") {
                setTimeout(function () {
                    window.location.href = '/Register/Register?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID;
                }, 800);
            }
        },
        error: function () {
            $('#LoadingPanel1').hide();
            var Response = { Result: "Failed", Message: "Something went wrong while closing out the job." };
            CustomAlert(Response);
        }
    });
}

