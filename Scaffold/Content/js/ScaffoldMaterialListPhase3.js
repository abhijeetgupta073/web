$(document).ready(function () {
    $('#limaterialPhase3').removeClass('active').addClass('active');
    ShowJobInfoDetails();
    MaterialDropDown();

    var id = getParameterByName('id');
    var RID = getParameterByName('RID');
    GetInventoryData(id, RID, "All");

    $('#OptStatusHidden').val($('#txtoptstatus').val());

    var screensize = $(window).width();

    if (screensize < 1150 && screensize > 650) {
        ($BODY.hasClass('nav-md'))
        $('#CreateNewDropDown').css("margin-top", "2px");
        $BODY.removeClass('nav-md').addClass('nav-sm');
        $LEFT_COL.removeClass('scroll-view');
        //$('#BlackSection').css('display', 'block');
        $('#imgLogo').parent().css('display', 'none');
        $('#imgLogo1').css('display', 'block').css("height", "38px");
    }


    else {
        if (screensize < (650)) {
            $BODY.removeClass('nav-sm')
            $('#imgLogo').parent().css('display', 'none');
            $('#imgLogo1').css('display', 'block').css("height", "52px");


            $('.xsfieldset').css('margin-left', '-22px').css("width", "100%");
        }
        else {
            $('#CreateNewDropDown').css("margin-top", "6px");
            $BODY.removeClass('nav-sm').addClass('nav-md');
            //$('#BlackSection').css('display', 'none');
            $('#imgLogo').parent().css('display', 'block');
            $('#imgLogo1').css('display', 'none');
            $('#leftbarsection').css('margin-top', '40px');
        }

    }
    $("#BuildDate").datepicker({
        dateFormat: 'dd-M-yy',
        maxDate: new Date(),
        closeText: 'Close'
    });
    $('.ClassA').click(function () {
        $('#BuildDate').focus();
    });
    var today = new Date();
    $("#BuildDate").val(today.DateMonthFormat());
});


function MaterialDropDown() {

    var id = getParameterByName('id');
    $.ajax({
        type: "POST",
        url: "/ScaffoldMaterialListPhase3/MaterialDropDown",
        data: JSON.stringify({ id: id }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {

            var ddlMaterialType = "<option value=''>All</option>";
            $.each(response, function (idx, obj) {
                if (obj != null) {
                    ddlMaterialType += '<option value="' + obj.MaterialType + '">' + obj.MaterialType + '</option>';
                }
            });

            $('#ddlMaterialType').empty().append(ddlMaterialType);
        }

    });
}

function ddlMaterialType_SelectedIndexChanged() {
    var id = getParameterByName('id');
    var RID = getParameterByName('RID');
    var ddlMaterialType = $('#ddlMaterialType').val();
    if (ddlMaterialType == "" || ddlMaterialType == undefined) {
        ddlMaterialType = "All";
    }
    GetInventoryData(id, RID, ddlMaterialType);
    GetTotalTotalTonnage(id, ddlMaterialType)
}
function GetInventoryData(id, RID, ddlMaterialType) {

    $('#LoadingPanel1').show();
    GetTotalTotalTonnage(id, ddlMaterialType);
    $.ajax({
        type: "POST",
        url: "/ScaffoldMaterialListPhase3/GetInventoryData",
        data: JSON.stringify({ id: id, RID: RID, ddlMaterialType: ddlMaterialType }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            var str = "";
            var GetData = response;
            $('#tblRegisterInventoryList').DataTable().clear();
            $('#tblRegisterInventoryList').DataTable().destroy();
            var count = 0;
            if (GetData.length > 0) {
                $('#divTotalDailyHireCost').css('display', 'block');

                for (var i = 0; i < GetData.length; i++) {
                    var FromId = GetData[i].FromId == null ? "" : GetData[i].FromId;
                    var ToId = GetData[i].ToId == null ? "" : GetData[i].ToId;
                    var MaterialType = GetData[i].MaterialType == null ? "" : GetData[i].MaterialType;
                    var ItemCode = GetData[i].ItemCode == null ? "" : GetData[i].ItemCode;
                    var ItemDesc = GetData[i].ItemDesc == null ? "" : GetData[i].ItemDesc;
                    var InventoryId = GetData[i].InventoryId == null ? "" : GetData[i].InventoryId;
                    var Quantity = GetData[i].Quantity == null ? "" : GetData[i].Quantity;
                    var ItemWeight = GetData[i].ItemWeight == null ? "" : GetData[i].ItemWeight.toFixed(2);
                    var TotalWeight = GetData[i].TotalWeight == null ? "" : GetData[i].TotalWeight.toFixed(2);
                    var SubCategoryID = GetData[i].SubCategoryID == null ? "" : GetData[i].SubCategoryID;


                    str += "<tr>"
                    str += "<td class='setWidth0' style='display:none'></td>"
                    str += "<td class='setWidth1'>" + MaterialType + "</td>"
                    str += "<td class='setWidth2'>" + ItemCode + "</td>"
                    str += "<td class='setWidth3'>" + ItemDesc + "</td>"
                    str += "<td class='setWidth4'>" + numberWithCommas(Quantity) + "</td>"
                    str += "<td class='setWidth8'>" + numberWithCommas(ItemWeight) + "</td>"
                    str += "<td class='setWidth9'>" + numberWithCommas(TotalWeight) + "</td>"

                    count += 1;
                }

                $('#tbodyRegisterInventoryList').append(str);

            }

            $('#LoadingPanel1').hide();
            $('#rowcount').val(count);

            var table = $('#tblRegisterInventoryList').DataTable({
                "bSort": false,
                "processing": true,
                searching: true,
                "bLengthChange": false,
                "bInfo": false,
                autoWidth: true,
                scrollX: true,
                scrollY: true,
                scrollCollapse: true,
                scrollY: "60vh",
                //"lengthMenu": [10, 25, 50, 100, 200],
                //"pageLength": 200,

            });
            if ($('#hdnHireInventory').val() == "False") {
                table.column(10).visible(false);
                table.column(11).visible(false);
                table.column(12).visible(false);
                $('#divTotalDailyHireCost').css('display', 'none');
            }
            // table.columns.adjust().draw();
        }
    })
}
function GetTotalTotalTonnage(ClientID, ddlMaterialType) {
    var ClientID = getParameterByName('id');
    var RID = getParameterByName('RID');

    $.ajax({
        type: "POST",
        url: "/ScaffoldMaterialListPhase3/GetTotalTotalTonnage",

        data: JSON.stringify({ ClientID: ClientID, RID: RID, ddlMaterialType: ddlMaterialType }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {

            var GetTotalTonnageData = response;
            $('#txtitems').val(GetTotalTonnageData.TotalItemsValues);
            $('#txttotaltonnage').val(GetTotalTonnageData.TotalTonnageValues);
        }
    })
}

function btnrequest_Click() {
    if (IsSiteStatus == "InActive") {
        CustomAlert({
            Result: "Failed", Message: "This site is InActive. This Action is not allowed."
        });
        return false;
    }
    var ClientID = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var RID = getParameterByName('RID');

    var Siteid = $('#hdnSiteLocID').val();

    var fromtocheck = "2";
    var page = "2";
    var from = "Job";
    var To = "Site";

    var EncryptData = Encrypt([fromtocheck, page, from, To]);
    fromtocheck = EncryptData[0];
    page = EncryptData[1];
    from = EncryptData[2];
    To = EncryptData[3];

    window.location.href = '/PipelineRequest/PipelineRequest?UID=' + UID + '&UserRole=' + UserRole + '&id=' + ClientID + '&FromId=' + RID + '&ToId=' + Siteid + '&from=' + from + '&To=' + To + '&fromtocheck=' + fromtocheck + '&page=' + page;
}

function btnBack12() {

    var ClientID = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Siteid = $('#hdnSiteLocID').val();
    var from = "Job";
    var RID = getParameterByName('RID');

    window.location.href = '/PipeLineInventory/PipeLineInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + ClientID + '&ToId=' + Siteid + '&from=' + from + '&RID=' + RID;

}

function ExportTo_Click() {

    //  Response.Write("<script> window.open('ScafffoldMaterialListPhase2Report?UID=" + Request.QueryString["UID"] + "&RID=" + Request.QueryString["RID"] + "&id=" + Request.QueryString["id"] + "&btn=prv','_blank');</script>");
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var RegisterID = getParameterByName('RID');
    var ddlMaterialType = $('#ddlMaterialType').val();
    if (ddlMaterialType == "" || ddlMaterialType == undefined) {
        ddlMaterialType = "All";
    }

    var path = '/ScafffoldMaterialListPhase3Report/ScafffoldMaterialListPhase3Report?UID=' + UID + '&RID=' + RegisterID + '&id=' + id + '&ddlMaterialType=' + ddlMaterialType;
    window.open(path);
}
//Added By Neeraj Joshi on 22/08/1024
function btnClearAll_Click() {
    if (IsSiteStatus == "InActive") {
        CustomAlert({
            Result: "Failed", Message: "This site is InActive. This Action is not allowed."
        });
        return false;
    }
    $("#divClearAllData").css("display", "block");
}
function btnClearcancel_Click() {
    $("#divClearAllData").css("display", "none");

}

function btnClearAllPipeLineData_Click() {
    if (IsSiteStatus == "InActive") {
        CustomAlert({
            Result: "Failed", Message: "This site is InActive. This Action is not allowed."
        });
        return false;
    }
    //var ReqId = $("#hdnfromid").val();
    // Retrieve encrypted RID from URL parameters
    var ReqId = getParameterByName('RID');

    // Decrypt the RID
    var DecryptData = Decrypt([ReqId]);
    ReqId = DecryptData[0];

    var ClientID = getParameterByName('id');
    $.ajax({
        type: "POST",
        url: "/PipelineRequest/btnClearAllPipeLineData_Click",
        data: JSON.stringify({ ReqId: ReqId, ClientID: ClientID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            var Result = response;

        }
    })
    location.reload();

}


function ConvertPipeLineJob() {
    if (IsSiteStatus == "InActive") {
        CustomAlert({
            Result: "Failed", Message: "This site is InActive. This Action is not allowed."
        });
        return false;
    }
    $("#divConvertPipeLineJob").css("display", "block");
}
function ConvertPipeLineJobNo() {
    $("#divConvertPipeLineJob").css("display", "none");
    var today = new Date();
    $("#BuildDate").val(today.DateMonthFormat());
}



//function ConvertPipeLineJobyes() {

//    $('#LoadingPanel1').show();
//    var id = getParameterByName('id');
//    var UID = getParameterByName('UID');
//    var RegisterID = getParameterByName('RID');
//    var Siteid = $('#hdnSiteLocID').val();
//    var startdate = $('#BuildDate').val();

//    $.ajax({
//        type: 'POST',
//        url: '/ScaffoldMaterialListPhase3/ConvertPipeLineJob',
//        data: JSON.stringify({
//            id: id, UID: UID, RegisterID: RegisterID, Siteid: Siteid, startdate: startdate
//        }),
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        async: true,
//        success: function (response) {
//            var results = response.Result;
//            var messages = response.Message;
//            var RegistId = response.NewRegId;

//            if (results === "Success") {
//                var Response = { Result: results, Message: messages };
//                CustomAlert(Response);

//                setTimeout(function () {
//                    if (RegistId && RegistId !== "0") {
//                        var ClientID = getParameterByName('id');
//                        var UID = getParameterByName('UID');
//                        var UserRole = getParameterByName('UserRole');

//                        var EncryptData = [RegistId];
//                        var encryData = Encrypt(EncryptData);
//                        RegistId = encryData[0];

//                        window.location.href = '/Register/Register?UID=' + UID + '&UserRole=' + UserRole + '&id=' + ClientID + '&RID=' + RegistId;
//                    } else {
//                        $('#LoadingPanel1').hide();
//                        CustomAlert('Failed', 'Syntax Error Contact Admin');
//                    }
//                }, 6000);
//            }
//            else if (results == "Insufficient") {
//                console.log("Results are Insufficient");
//                showCustomAlert(messages);

//                // Hide the popup and reset the table
//                //$("#divCopyHistPopup").css('display', 'none');
//                // $('#tblRegisterList').DataTable().clear();
//                //$('#tblRegisterList').DataTable().destroy();
//                //$("#MyHistoricalJobModal").css("display", "none");
//                $('#LoadingPanel1').css("display", "none");

//                // Uncheck the checkboxes
//                // $('#WithInventoryForLiveJob').prop('checked', false);
//                // $('#WithInventoryForPipelineJob').prop('checked', false);
//                // $('input[name="SelectJobType"]:checked').prop('checked', false);

//                // Set the dates in the input fields
//                //var today = new Date();
//                //$("#PipelineStartDate").val(today.DateMonthFormat());
//                //$("#BuildDate").val(today.DateMonthFormat());
//            }
//            else {
//                var Response = { Result: results, Message: messages };
//                CustomAlert(Response);
//                $("#divConvertPipeLineJob").css("display", "none");
//                $('#LoadingPanel1').hide();
//                var today = new Date();
//                $("#BuildDate").val(today.DateMonthFormat());
//            }
//        },
//        error: function (xhr, status, error) {
//            CustomAlert('Error', 'An error occurred: ' + error);
//        }
//    });
//}


//Added By Neeraj Joshi on 22/08/1024
function checkGoPipelineToActiveJobStatus() {
    if (IsSiteStatus == "InActive") {
        CustomAlert({
            Result: "Failed", Message: "This site is InActive. This Action is not allowed."
        });
        return false;
    }
    $('#LoadingPanel1').show();
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var RegisterId = getParameterByName('RID');
    var DecryptData = Decrypt([RegisterId]);
    RegisterId = DecryptData[0];
    $.ajax({
        type: 'POST',
        url: '/Register/CheckIsPipeLineJob',
        data: JSON.stringify({ id: id, UID: UID, RegisterId: RegisterId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            if (response.Result == true) {
                $('#LoadingPanel1').hide();
                ConvertPipeLineJob();
            } else {
                $('#LoadingPanel1').hide();
                CustomAlert({ Result: 'Failed', Message: 'This job is already has Job Inventory so cannot copy Pipeline data to Job inventory' });
            }
        },
        error: function (xhr, status, error) {
            $('#LoadingPanel1').hide();
            CustomAlert({ Result: 'Error', Message: 'An error occurred: ' + error });
        }
    });
}

function ConvertPipeLineJobyes() {
    if (IsSiteStatus == "InActive") {
        CustomAlert({
            Result: "Failed", Message: "This site is InActive. This Action is not allowed."
        });
        return false;
    }
    $('#LoadingPanel1').show();
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var RegisterID = getParameterByName('RID');
    var Siteid = $('#hdnSiteLocID').val();
    var startdate = $('#BuildDate').val();
    

    $.ajax({
        type: 'POST',
        url: '/ScaffoldMaterialListPhase3/ConvertPipeLineJob',
        data: JSON.stringify({
            id: id, UID: UID, RegisterID: RegisterID, Siteid: Siteid, startdate: startdate
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            var results = response.Result;
            var messages = response.Message;
            var RegistId = response.NewRegId;

            if (results === "Success") {
                var Response = { Result: results, Message: messages };
                CustomAlert(Response);

                setTimeout(function () {
                    if (RegistId && RegistId !== "0") {
                        var ClientID = getParameterByName('id');
                        var UID = getParameterByName('UID');
                        var UserRole = getParameterByName('UserRole');

                        var EncryptData = [RegistId];
                        var encryData = Encrypt(EncryptData);
                        RegistId = encryData[0];

                        window.location.href = '/Register/Register?UID=' + UID + '&UserRole=' + UserRole + '&id=' + ClientID + '&RID=' + RegistId;
                    } else {
                        $('#LoadingPanel1').hide();
                        CustomAlert('Failed', 'Syntax Error Contact Admin');
                    }
                }, 6000);
            }
            else if (results == "Insufficient") {
                console.log("Results are Insufficient");
                showCustomAlert(messages);

                // Hide the popup and reset the table
                //$("#divCopyHistPopup").css('display', 'none');
                // $('#tblRegisterList').DataTable().clear();
                //$('#tblRegisterList').DataTable().destroy();
                //$("#MyHistoricalJobModal").css("display", "none");
                $('#LoadingPanel1').css("display", "none");
            }
            else {
                var Response = { Result: results, Message: messages };
                CustomAlert(Response);
                $("#divConvertPipeLineJob").css("display", "none");
                $('#LoadingPanel1').hide();
                var today = new Date();
                $("#BuildDate").val(today.DateMonthFormat());
            }
        },
        error: function (xhr, status, error) {
            CustomAlert('Error', 'An error occurred: ' + error);
        }
    });
}

//Added on 3March2025
function checkNoInventoryRequired() {
    if (IsSiteStatus == "InActive") {
        CustomAlert({
            Result: "Failed", Message: "This site is InActive. This Action is not allowed."
        });
        return false;
    }
    $('#LoadingPanel1').show();
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var RegisterId = getParameterByName('RID');
    var DecryptData = Decrypt([RegisterId]);
    RegisterId = DecryptData[0];
    var Siteid = $('#hdnSiteLocID').val();

    $.ajax({
        type: 'POST',
        url: '/ScaffoldMaterialListPhase3/checkNoInventoryRequired',
        data: JSON.stringify({ id: id, UID: UID, RegisterId: RegisterId, Siteid: Siteid }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            if (response.Result == false) {
                $('#LoadingPanel1').hide();
                checkGoPipelineToActiveJobStatus();
            } else {
                $('#LoadingPanel1').hide();
                CustomAlert({ Result: 'Failed', Message: 'The "No Inventory Required" box is "ticked" for this job.<br><span style="margin-left: 40px;">If you want to add inventory "untick" the box!</span>' });
            }
        },
        error: function (xhr, status, error) {
            $('#LoadingPanel1').hide();
            CustomAlert({ Result: 'Error', Message: 'An error occurred: ' + error });
        }
    });
}