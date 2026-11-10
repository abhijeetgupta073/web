
$(function () {

    var Href = window.location.href.split('?');
    if (Href.length == 1) {
        $('#UlNavTabSeciton li').css('display', 'none');
        $('#liThirdParty').css('display', 'block');
        $('#liJob').css('display', 'block');
    }
    $('#UlNavTabSeciton a').css('cursor', 'pointer');
    $('#UlNavTabSeciton a').click(function (e) {
        $('#LoadingPanel1').css('display', 'block');
        var ClickAcnhorID = e.currentTarget.id;
        var UID = getParameterByName('UID');
        var UserRole = getParameterByName('UserRole');
        var id = getParameterByName('id');

        if (ClickAcnhorID == 'AnchorYardInventory') {
            //  window.location.href = '/InventoryManagement.aspx?' + Href[1];
            //window.location.href = '/InventoryManagement/InventoryManagement?' + Href[1];
            window.location.href = '/InventoryManagement/InventoryManagement?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
        if (ClickAcnhorID == 'AnchorJobInventory') {
            // window.location.href = '/RequestInventory.aspx?' + Href[1];
            //window.location.href = '/RequestInventory/RequestInventory?' + Href[1];
            window.location.href = '/RequestInventory/RequestInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
        if (ClickAcnhorID == 'AnchorReturnInventory') {
            $('#LoadingPanel1').css('display', 'none');
        }
        if (ClickAcnhorID == 'AnchorDepotInventory') {

            //window.location.href = '/MainDepotInventory/MainDepotInventory?' + Href[1];
            window.location.href = '/MainDepotInventory/MainDepotInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
        if (ClickAcnhorID == 'AnchorSiteInventory') {
            // window.location.href = '/SiteInventory.aspx?' + Href[1];
            //window.location.href = '/SiteInventory/SiteInventory?' + Href[1];
            window.location.href = '/SiteInventory/SiteInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
        if (ClickAcnhorID == 'AnchorDamagedInventory') {
            //  window.location.href = '/DamagedInventory.aspx?' + Href[1];
            //window.location.href = '/DamagedInventory/DamagedInventory?' + Href[1];
            window.location.href = '/DamagedInventory/DamagedInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
        if (ClickAcnhorID == 'AnchorPipeLineInventory') {
            //window.location.href = '/ReturnInventory/ReturnInventory?' + Href[1];
            window.location.href = '/PipeLineInventory/PipeLineInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
    });
});

$(document).ready(function () {

    //SiteDropDownBind();//Commented  on 02-September-2021
    //SitetoJobDropDown();//Commented  on 02-September-2021
    PipeInventoryHide();


});
//Depot To site RadioButton Changed



function rdodepottosite_CheckedChanged() {
    $('#LoadingPanel1').css('display', 'block');
    $('#DtoS').css('display', 'block');
    $('#StoJ').css('display', 'none');
    // gridReturn.Rebind();
    SiteDropDownBind();//Added on 02-September-2021
    GetReturnInventoryData();
}

// site to Job RadioButton Changed
function rdositetojob_CheckedChanged() {
    $('#LoadingPanel1').css('display', 'block');
    $('#StoJ').css('display', 'block');
    $('#DtoS').css('display', 'none');
    SitetoJobDropDown();//Added on 02-September-2021
    GetReturnInventoryData();

}

function rdomaindepottodepot_CheckedChanged() {
    $('#LoadingPanel1').css('display', 'block');
    $('#StoJ').css('display', 'none');
    $('#DtoS').css('display', 'none');
    GetReturnInventoryData();

}

function ddlfrom_SelectedIndexChanged() {
    //DtoS.Style.Add("Display", "block");
    //StoJ.Style.Add("display", "none");
    //ddl1from.ClearSelection();
    //ddl1to.ClearSelection();
    ////ddlfrom.SelectedValue = Request.QueryString["FromId"];
    //displaygrid.Style.Add("display", "block");
    //gridReturn.Rebind();
    GetReturnInventoryData();
}


function ddl1to_SelectedIndexChanged() {
    //function GetJobDropDownBind() {
    var ClientID = getParameterByName('id');
    var ddl1to = $('#ddl1to option:selected').val();
    if (ddl1to == "" || ddl1to == undefined) {
        ddl1to = "All";
        $("#ddl1from").empty();
    } else {
        //$("#ddl1from").trigger("change").val();//Added on 02-September-2021
        $.ajax({
            type: "POST",
            url: "/ReturnInventory/GetJobDropDownBind",

            data: JSON.stringify({ id: ClientID, JobSiteid: ddl1to }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (response) {

                var GetData = response;
                if (GetData.length > 0) {
                    $("#ddl1from").empty();
                    $("#ddl1from").append('<option value="">All</option>');
                    for (var i = 0; i < GetData.length; i++) {
                        $("#ddl1from").append("<option value='" + GetData[i].RegisterId + "'>" + GetData[i].ScaffRef + "</option>");
                    }
                }
            }
        });
    }
    GetReturnInventoryData();
}

//}
function ddl1from_SelectedIndexChanged() {
    GetReturnInventoryData();
}

//Site Dropdown Bind
function SiteDropDownBind() {
    var ClientID = getParameterByName('id');
    var UID = getParameterByName('UID');
    $('#LoadingPanel1').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/ReturnInventory/SiteDropDown",

        data: JSON.stringify({ id: ClientID, UID: UID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            var SiteData = response;
            var SiteLocId = "";
            if (SiteData.length > 0) {

                $("#ddl1to").empty();
                $("#ddlfrom").empty();
                //$("#ddlfrom").append('<option value="">All</option>');
                for (var i = 0; i < SiteData.length; i++) {
                    SiteLocId = SiteData[i].SiteLocID;
                    if (SiteLocId == 0) {
                        SiteLocId = "";
                    }
                    $("#ddlfrom").append("<option value='" + SiteLocId + "'>" + SiteData[i].SiteLocation + "</option>");
                    //   $("#ddlfrom").append("<option value='" + SiteData[i].SiteLocID + "'>" + SiteData[i].SiteLocation + "</option>");

                }
            }
            $('#LoadingPanel1').css('display', 'none');
        }
    })
}
function SitetoJobDropDown() {
    var ClientID = getParameterByName('id');
    var UID = getParameterByName('UID');

    $.ajax({
        type: "POST",
        url: "/ReturnInventory/SitetoJobDropDown",
        data: JSON.stringify({ id: ClientID, UID: UID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {

            var SiteData = response;
            var SiteLocId = "";
            if (SiteData.length > 0) {

                $("#ddl1to").empty();
                $("#ddlfrom").empty();
                for (var i = 0; i < SiteData.length; i++) {
                    SiteLocId = SiteData[i].SiteLocID;
                    if (SiteLocId == 0) {
                        SiteLocId = "";
                    }
                    // $("#ddl1to").append("<option value='" + SiteData[i].SiteLocID + "'>" + SiteData[i].SiteLocation + "</option>");
                    $("#ddl1to").append("<option value='" + SiteLocId + "'>" + SiteData[i].SiteLocation + "</option>");
                }

                ddl1to_SelectedIndexChanged();
            }
        }
    })
}



var ArrayIdxArr = [0];
//function GetReturnInventoryData() {
//    ArrayIdxArr.length = 0;
//    $('#LoadingPanel1').css('display', 'block');
//    $('#displaygrid').css('display', 'block');

//    var ddlfrom = $('#ddlfrom option:selected').val();
//    if (ddlfrom == "" && ddlfrom == undefined) {
//        ddlfrom = "All";
//    }
//    var ddl1to = $('#ddl1to option:selected').val();
//    if (ddl1to == "" && ddl1to == undefined) {
//        ddl1to = "All";
//    }
//    var ddl1from = $('#ddl1from option:selected').val();
//    if (ddl1from == "" && ddl1from == undefined) {
//        ddl1from = "All";
//    }

//    var id = getParameterByName('id');
//    var rdodepottosite = $('#rdodepottosite').prop('checked');
//    var rdositetojob = $('#rdositetojob').prop('checked');
//    var rdomaindepottodepot = $('#rdomaindepottodepot').prop('checked');

//    $.ajax({
//        type: "POST",
//        url: "/ReturnInventory/GetReturnInventoryData",
//        // data: JSON.stringify({ id: ClientID, FromId: FromId, ToId: ToId, ddl1from: ddl1from, ddl1to: ddl1to, JobSiteid: JobSiteid, ddlfrom: ddlfrom, rdodepottosite: rdodepottosite, rdositetojob: rdositetojob, RID: RID }),
//        data: JSON.stringify({ id: id, ddlfrom: ddlfrom, ddl1to: ddl1to, ddl1from: ddl1from, rdodepottosite: rdodepottosite, rdositetojob: rdositetojob, rdomaindepottodepot: rdomaindepottodepot }),
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        async: true,
//        success: function (response) {
//            var str = "";
//            var GetData = response;
//            $('#tblReturnInventoryGrid').DataTable().clear();
//            $('#tblReturnInventoryGrid').DataTable().destroy();

//            if (GetData.length > 0) {

//                for (var i = 0; i < GetData.length; i++) {
//                    var FromId = GetData[i].FromId == null ? "" : GetData[i].FromId;
//                    var ToId = GetData[i].ToId == null ? "" : GetData[i].ToId;
//                    var ReturnInvId = GetData[i].ReturnInvId == null ? "" : GetData[i].ReturnInvId;
//                    var FromName = GetData[i].FromName == null ? "" : GetData[i].FromName;
//                    var ToName = GetData[i].ToName == null ? "" : GetData[i].ToName;
//                    var ReturnDoneBy = GetData[i].ReturnDoneBy == null ? "" : GetData[i].ReturnDoneBy;
//                    var ReturnDoneOn = GetData[i].ReturnDoneOn == null ? "" : GetData[i].ReturnDoneOn;
//                    var ReturnStatus = GetData[i].ReturnStatus == null ? "" : GetData[i].ReturnStatus;
//                    var ReturnDescription = GetData[i].ReturnDescription == null ? "" : GetData[i].ReturnDescription;
//                    var NoofItems = GetData[i].NoofItems == null ? "" : GetData[i].NoofItems;
//                    var TotalWeight = GetData[i].TotalWeight == null ? "" : GetData[i].TotalWeight;

//                    str += "<tr>"
//                    str += "<td style='display:none'></td>"
//                    str += "<td class='setWidth' id='ReturnInvId_" + i + "' style='text-align:center;width:5%;'><input type='checkbox' class='ChkTransID' ReturnInvId_='" + ReturnInvId + "'/></td>"
//                    str += "<td class='setWidth' id='transImgId_" + i + "' title='Images' style='cursor:pointer;width:5%;' onclick=OpenDedliveryDocument(" + ReturnInvId + ")><i class='fa fa-image'></i></td>"
//                    if (ReturnStatus != "Confirm") {
//                        str += "<td id='ReturnInvId_" + i + "' title='Delete' style='cursor:pointer;'><i class='fa fa-trash' onclick=ShowDeletePopup('" + ReturnInvId + "')></i></td>";
//                    }
//                    else {
//                        str += "<td id='ReturnInvId_" + i + "' title='Return Confirmed.' style='cursor:pointer;'><i class='fa fa-check' onclick=ShowAlertMsg('" + ReturnInvId + "')></i></td>";
//                    }
//                    str += "<td style='width:5%;' class='setWidth'>" + ReturnInvId + "</td>"
//                    str += "<td class='setWidth' id='transdltIdViewID_" + i + "' title='View Record' style='cursor:pointer;width:10%;'><button type='button' id='tbnView' Class='w3-btn w3-teal btnJobView' ReturnInventoryId_='" + ReturnInvId + "' ><i class='fa fa-search'></i>&nbsp;View</button></td>";
//                    str += "<td style='width:10%;' class='setWidth'>" + ReturnDescription + "</td>"
//                    str += "<td style='width:25%;' class='setWidth' id='FromName_" + ReturnInvId + "'>" + FromName + "</td>"
//                    str += "<td style='width:15%;' class='setWidth' id='ToName_" + ReturnInvId + "'>" + ToName + "</td>"
//                    str += "<td style='width:10%;' class='setWidth'>" + ReturnStatus + "</td>"
//                    str += "<td style='width:15%;' class='setWidth'>" + ReturnDoneBy + "</td>"
//                    str += "<td style='width:10%;' class='setWidth'>" + ReturnDoneOn + "</td>"
//                    str += "<td style='width:10%;' class='setWidth'>" + NoofItems + "</td>"
//                    str += "<td style='width:10%;' class='setWidth'>" + TotalWeight + "</td>"
//                    str += "<td id='ReturnInvId_" + ReturnInvId + "' style='display:none'>" + FromId + "</td>"
//                    str += "<td id='FromID_" + ReturnInvId + "' style='display:none'>" + FromId + "</td>"
//                    str += "<td id='ToID_" + ReturnInvId + "' style='display:none'>" + ToId + "</td>"
//                    str += "</tr>"
//                }

//            }

//            $('#tbodyReturnInventoryGrid').append(str);


//            var table = $('#tblReturnInventoryGrid').DataTable({
//                "bSort": false,
//                "processing": true,
//                searching: true,
//                "bLengthChange": false,
//                "bInfo": false,
//                autoWidth: false,
//                scrollX: true,
//                scrollCollapse: true,
//                scrollY: "60vh",
//                "paging": false,
//                "ordering": false,
//                "info": false,

//                "language": {
//                    "emptyTable": "No scheduled data/reports available"
//                }

//            });

//            // Hide Show For DataTable Column And Buttons Start Here
//            GridButtonHideShow(table, "tblReturnInventoryGrid");

//            // Hide Show For DataTable Column And Buttons End Here

//            $(".ChkTransID").click(function () {
//                event.stopImmediatePropagation();
//                var ReturnInvId = $(this).attr('ReturnInvId_').split('_')[0];
//                var ColumnChecked = $(this).prop('checked');
//                var FromID = $('#FromID_' + ReturnInvId).text();
//                var ToID = $('#ToID_' + ReturnInvId).text();

//                var ArryVal = [ReturnInvId, FromID, ToID];

//                if (ColumnChecked == true) {
//                    //RequestInventoryIdxArr.push(transID);
//                    ArrayIdxArr.push(ArryVal);
//                }
//                else {
//                    for (var i = 0; i < ArrayIdxArr.length; i++) {
//                        var TempArry = ArrayIdxArr[i];
//                        if (TempArry[0] == ReturnInvId) {
//                            // if (RequestInventoryIdxArr[i] == transID) {
//                            ArrayIdxArr.splice(i, 1);
//                        }
//                    }
//                }
//            });

//            $('.btnJobView').click(function () {
//                event.stopImmediatePropagation();
//                var ReturnInvId = $(this).attr('ReturnInventoryId_').split('_')[0];
//                var FromID = $('#FromID_' + ReturnInvId).text();
//                var ToID = $('#ToID_' + ReturnInvId).text();
//                var FromName = $('#FromName_' + ReturnInvId).text();
//                var ToName = $('#ToName_' + ReturnInvId).text();

//                var ClientID = getParameterByName('id');
//                var UID = getParameterByName('UID');
//                var UserRole = getParameterByName('UserRole');


//                var EncryptData = [ReturnInvId, FromName, ToName];
//                var encryData = Encrypt(EncryptData);
//                ReturnInvId = encryData[0];
//                FromName = encryData[1];
//                ToName = encryData[2];

//                if (ReturnInvId != "") {
//                    window.open('/ReturnDocket/ReturnDocket?UID=' + UID + '&UserRole=' + UserRole + '&id=' + ClientID + '&RInvId=' + ReturnInvId + '&FromName=' + FromName + '&ToName=' + ToName);
//                }
//            });

//            $('#LoadingPanel1').css('display', 'none');
//        }

//    })

//}
function GetReturnInventoryData() {
    ArrayIdxArr.length = 0;
    $('#LoadingPanel1').css('display', 'block');
    $('#displaygrid').css('display', 'block');

    var ddlfrom = $('#ddlfrom option:selected').val();
    if (ddlfrom == "" && ddlfrom == undefined) {
        ddlfrom = "All";
    }
    var ddl1to = $('#ddl1to option:selected').val();
    if (ddl1to == "" && ddl1to == undefined) {
        ddl1to = "All";
    }
    var ddl1from = $('#ddl1from option:selected').val();
    if (ddl1from == "" && ddl1from == undefined) {
        ddl1from = "All";
    }

    var id = getParameterByName('id');
    var rdodepottosite = $('#rdodepottosite').prop('checked');
    var rdositetojob = $('#rdositetojob').prop('checked');
    var rdomaindepottodepot = $('#rdomaindepottodepot').prop('checked');
    if ($.fn.DataTable.isDataTable('#tblReturnInventoryGrid')) {
        $('#tblReturnInventoryGrid').DataTable().clear();
        $('#tblReturnInventoryGrid').DataTable().destroy();
    }
    $('#tbodyReturnInventoryGrid').empty();
    if (!$('#dtHiddenColStyle').length) {
        $('head').append("<style id='dtHiddenColStyle'>.dt-hidden-col{display:none !important;}</style>");
    }

    var table = $('#tblReturnInventoryGrid').DataTable({
        "bSort": false,
        "processing": true,
        "serverSide": true,
        "serverMethod": "POST",
        searching: true,
        "bLengthChange": false,
        "bInfo": false,
        autoWidth: false,
        scrollX: true,
        scrollCollapse: true,
        scrollY: "60vh",
        "paging": true,
        "pageLength": 10,
        "ordering": false,
        "info": false,

        "language": {
            "emptyTable": "No scheduled data/reports available"
        },

        ajax: {
            url: "/ReturnInventory/GetReturnInventoryData",
            data: function (d) {
                d.id = id;
                d.ddlfrom = ddlfrom;
                d.ddl1to = ddl1to;
                d.ddl1from = ddl1from;
                d.rdodepottosite = rdodepottosite;
                d.rdositetojob = rdositetojob;
                d.rdomaindepottodepot = rdomaindepottodepot;
            },
            error: function (xhr, error, thrown) {
                $('#LoadingPanel1').css('display', 'none');
                console.error("GetReturnInventoryData ajax error:", xhr.responseText || thrown);
            }
        },
        columns: [
            { title: "", data: null, defaultContent: "", orderable: false, className: "dt-hidden-col" },   
            {
                title: "#", data: "ReturnInvId", className: "setWidth",
                render: function (data, type, row, meta) {
                    return "<input type='checkbox' class='ChkTransID' ReturnInvId_='" + (data == null ? "" : data) + "'/>";
                }
            },
            {
                title: "", data: "ReturnInvId", className: "setWidth",
                render: function (data) {
                    return "<span title='Images' style='cursor:pointer;' onclick=OpenDedliveryDocument(" + (data == null ? "" : data) + ")><i class='fa fa-image'></i></span>";
                }
            },
            {
                title: "#", data: "ReturnStatus",
                render: function (data, type, row) {
                    var ReturnInvId = row.ReturnInvId == null ? "" : row.ReturnInvId;
                    if (data != "Confirm") {
                        return "<span title='Delete' style='cursor:pointer;'><i class='fa fa-trash' onclick=ShowDeletePopup('" + ReturnInvId + "')></i></span>";
                    }
                    return "<span title='Return Confirmed.' style='cursor:pointer;'><i class='fa fa-check' onclick=ShowAlertMsg('" + ReturnInvId + "')></i></span>";
                }
            },
            { title: "Trans Id", data: "ReturnInvId", className: "setWidth", defaultContent: "" },
            {
                title: "View", data: "ReturnInvId", className: "setWidth",
                render: function (data) {
                    return "<button type='button' id='tbnView' Class='w3-btn w3-teal btnJobView' ReturnInventoryId_='" + (data == null ? "" : data) + "' ><i class='fa fa-search'></i>&nbsp;View</button>";
                }
            },
            { title: "Description", data: "ReturnDescription", className: "setWidth", defaultContent: "" },
            {
                title: "From", data: "FromName", className: "setWidth",
                render: function (data, type, row) {
                    return "<span id='FromName_" + row.ReturnInvId + "'>" + (data == null ? "" : data) + "</span>";
                }
            },
            {
                title: "To", data: "ToName", className: "setWidth",
                render: function (data, type, row) {
                    return "<span id='ToName_" + row.ReturnInvId + "'>" + (data == null ? "" : data) + "</span>";
                }
            },
            { title: "Return Status", data: "ReturnStatus", className: "setWidth", defaultContent: "" },
            { title: "Return Done By", data: "ReturnDoneBy", className: "setWidth", defaultContent: "" },
            { title: "Return Done On", data: "ReturnDoneOn", className: "setWidth", defaultContent: "" },
            { title: "(Returned)<br> No. of Items", data: "NoofItems", className: "setWidth", defaultContent: "" },
            { title: "Total Weight<br> (Tonnes)", data: "TotalWeight", className: "setWidth", defaultContent: "" },
            {
                title: "ReturnInvID", data: "FromId", className: "dt-hidden-col",
                render: function (data, type, row) {
                    return "<span id='ReturnInvId_" + row.ReturnInvId + "'>" + (data == null ? "" : data) + "</span>";
                }
            },
            {
                title: "FromID", data: "FromId", className: "dt-hidden-col",
                render: function (data, type, row) {
                    return "<span id='FromID_" + row.ReturnInvId + "'>" + (data == null ? "" : data) + "</span>";
                }
            },
            {
                title: "ToID", data: "ToId", className: "dt-hidden-col",
                render: function (data, type, row) {
                    return "<span id='ToID_" + row.ReturnInvId + "'>" + (data == null ? "" : data) + "</span>";
                }
            }
        ],

        createdRow: function (row) {
            $('td:eq(0)', row).css('display', 'none');
            $('td:eq(14)', row).css('display', 'none');
            $('td:eq(15)', row).css('display', 'none');
            $('td:eq(16)', row).css('display', 'none');
        },

        drawCallback: function () {
            GridButtonHideShow(table, "tblReturnInventoryGrid");


            $(".ChkTransID").off('click').on('click', function (event) {
                event.stopImmediatePropagation();
                var ReturnInvId = $(this).attr('ReturnInvId_').split('_')[0];
                var ColumnChecked = $(this).prop('checked');
                var FromID = $('#FromID_' + ReturnInvId).text();
                var ToID = $('#ToID_' + ReturnInvId).text();

                var ArryVal = [ReturnInvId, FromID, ToID];

                if (ColumnChecked == true) {
                    ArrayIdxArr.push(ArryVal);
                }
                else {
                    for (var i = 0; i < ArrayIdxArr.length; i++) {
                        var TempArry = ArrayIdxArr[i];
                        if (TempArry[0] == ReturnInvId) {
                            ArrayIdxArr.splice(i, 1);
                        }
                    }
                }
            });

            $('.btnJobView').off('click').on('click', function (event) {
                event.stopImmediatePropagation();
                var ReturnInvId = $(this).attr('ReturnInventoryId_').split('_')[0];
                var FromID = $('#FromID_' + ReturnInvId).text();
                var ToID = $('#ToID_' + ReturnInvId).text();
                var FromName = $('#FromName_' + ReturnInvId).text();
                var ToName = $('#ToName_' + ReturnInvId).text();

                var ClientID = getParameterByName('id');
                var UID = getParameterByName('UID');
                var UserRole = getParameterByName('UserRole');


                var EncryptData = [ReturnInvId, FromName, ToName];
                var encryData = Encrypt(EncryptData);
                ReturnInvId = encryData[0];
                FromName = encryData[1];
                ToName = encryData[2];

                if (ReturnInvId != "") {
                    window.open('/ReturnDocket/ReturnDocket?UID=' + UID + '&UserRole=' + UserRole + '&id=' + ClientID + '&RInvId=' + ReturnInvId + '&FromName=' + FromName + '&ToName=' + ToName);
                }
            });

            $('#LoadingPanel1').css('display', 'none');

            table.columns.adjust();
        }
    });

}//added on 21-July-2021
function ShowDeletePopup(transId) {
    if (IsInactive(transId)) {
        CustomAlert({
            Result: "Failed", Message: "This site is InActive. You cannot delete requests."
        });
        return false;
    }
    $('#hdnReturnInvId').val(transId);
    $("#divdelete").css("display", "block");
}

function btndeleteyes_Click() {
    $('#LoadingPanel1').show();
    var ReturnInvId = $('#hdnReturnInvId').val();
    var ClientId = getParameterByName('id');
    btndeleteyes_Click

    $.ajax({
        type: "POST",
        url: "/ReturnInventory/btndeleteyes_Click",
        data: JSON.stringify({ ClientId: ClientId, ReturnInvId: ReturnInvId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            var Result = response;
            $("#divdelete").css("display", "none");
            CustomAlert(response);
            GetReturnInventoryData();
        }
    })
}
function btndeleteno_Click() {
    $("#divdelete").css("display", "none");
}
//added on 21-July-2021 end
////added on 27-July-2021 start
function ShowAlertMsg() {
    //bootbox.alert({
    //     message: "You can't delete. This item is confirmed.",
    //})
    var Response = { Result: "Failed", Message: "You can't delete. This return is confirmed." };
    CustomAlert(Response);
}
//added on 27-July-2021 end
function ExportToReturnCsvButton_Click() {


    var ddlfrom = $('#ddlfrom option:selected').val();
    if (ddlfrom == "" && ddlfrom == undefined) {
        ddlfrom = "All";
    }
    var ddl1to = $('#ddl1to option:selected').val();
    if (ddl1to == "" && ddl1to == undefined) {
        ddl1to = "All";
    }
    var ddl1from = $('#ddl1from option:selected').val();
    if (ddl1from == "" || ddl1from == undefined) {
        ddl1from = "";
    }
    var id = getParameterByName('id');
    var rdodepottosite = $('#rdodepottosite').prop('checked');
    var rdositetojob = $('#rdositetojob').prop('checked');
    var rdomaindepottodepot = $('#rdomaindepottodepot').prop('checked');

    if ($('#rdositetojob').prop('checked') == true || $('#rdodepottosite').prop('checked') == true || $('#rdomaindepottodepot').prop('checked') == true) {

        window.location.href = '/ReturnInventory/ExportToReturnCsvButton_Click?id=' + id + '&ddlfrom=' + ddlfrom + '&ddl1to=' + ddl1to + '&ddl1from=' + ddl1from + '&rdodepottosite=' + rdodepottosite + '&rdositetojob=' + rdositetojob + '&rdomaindepottodepot=' + rdomaindepottodepot;
    }

    else {
        $('#art1').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
        $('#art1').css("display", "block");
        $('#alertlabel').text("Alert ! Please Select Any One.");

    }

}



function OpenDedliveryDocument(ReturnInvId) {
    var Href = window.location.href.split('?');
    var ClientID = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var SiteVal = 1;
    var JobVal = 2;
    var DepotVal = 3;

    var EncryptData = [ReturnInvId, SiteVal, JobVal, DepotVal];
    var encryData = Encrypt(EncryptData);
    ReturnInvId = encryData[0];
    SiteVal = encryData[1];
    JobVal = encryData[2];
    DepotVal = encryData[3];

    if ($('#rdomaindepottodepot').prop('checked') == true) {
        window.location.href = '/DeliveryDocketDocument/DeliveryDocketDocument?UID=' + UID + '&UserRole=' + UserRole + '&id=' + ClientID + '&RInvId=' + ReturnInvId + '&Depot=' + DepotVal;
    }
    if ($('#rdodepottosite').prop('checked') == true) {
        window.location.href = '/DeliveryDocketDocument/DeliveryDocketDocument?UID=' + UID + '&UserRole=' + UserRole + '&id=' + ClientID + '&RInvId=' + ReturnInvId + '&Site=' + SiteVal;
    }
    if ($('#rdositetojob').prop('checked') == true) {
        window.location.href = '/DeliveryDocketDocument/DeliveryDocketDocument?UID=' + UID + '&UserRole=' + UserRole + '&id=' + ClientID + '&RInvId=' + ReturnInvId + '&Job=' + JobVal;
    }

    //GridEditableItem gdi = (GridEditableItem)e.Item;
    //RInvId = Convert.ToInt32(gdi.GetDataKeyValue("ReturnInvId").ToString());
    //if (rdodepottosite.Checked == true)
    //{
    //    Response.Redirect("DeliveryDocketDocument.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RInvId=" + databaseCon.Encrypt(Convert.ToString(RInvId)) + "&Site=" + databaseCon.Encrypt("1"), false);
    //}
    //if (rdositetojob.Checked == true)
    //{
    //    Response.Redirect("DeliveryDocketDocument.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RInvId=" + databaseCon.Encrypt(Convert.ToString(RInvId)) + "&Job=" + databaseCon.Encrypt("2"), false);
    //}

}

function Return_Click() {
    if ($('#rdodepottosite').prop('checked')) {
        if ($("#ddlfrom").val() != "" && $("#ddlfrom").val() != "All") {
            if ($("#ddlfrom option:selected").text().indexOf("(InActive)") !== -1) {
                CustomAlert({
                    Result: "Failed",
                    Message: "This site is InActive. You cannot make any changes."
                });
                return false;
            }
        }
        else {
            if (IsSelectedRecordInactive()) {
                CustomAlert({
                    Result: "Failed",
                    Message: "This site is InActive. You cannot make any changes."
                });
                return false;
            }
        }
    }

    if ($('#rdositetojob').prop('checked')) {
        if ($("#ddl1to").val() != "" && $("#ddl1to").val() != "All") {

            if ($("#ddl1to option:selected").text().indexOf("(InActive)") !== -1) {
                CustomAlert({
                    Result: "Failed",
                    Message: "This site is InActive. You cannot make any changes."
                });
                return false;
            }
        }
        else {

            if (IsSelectedRecordInactive()) {
                CustomAlert({
                    Result: "Failed",
                    Message: "This site is InActive. You cannot make any changes."
                });
                return false;
            }
        }
    }
    var RInvId = 0, count = 0;
    var Flag = false;
    var siteselection = "", jobselection = "", page = 1, fromtocheck = 1;
    var TempArry = [];

    var ddlfrom = $('#ddlfrom option:selected').val();
    var ddl1to = $('#ddl1to option:selected').val();
    var ddl1from = $('#ddl1from option:selected').val();

    if ($('#rdositetojob').prop('checked') == true) {
        fromtocheck = 2;
    }
    if ($('#rdomaindepottodepot').prop('checked') == true) {
        fromtocheck = 3;
    }

    var CheckBoxLength = ArrayIdxArr.length;
    if (CheckBoxLength == 1) {


    }

    for (var i = 0; i < ArrayIdxArr.length; i++) {
        Flag = true;
        TempArry = ArrayIdxArr[i];
        count += 1;
    }
    RInvId = TempArry[0];
    siteselection = TempArry[1];
    jobselection = TempArry[2];



    var ClientID = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');

    var EncryptData = [RInvId, siteselection, jobselection, page, ddlfrom, fromtocheck, ddl1to, ddl1from];
    var encryData = Encrypt(EncryptData);
    RInvId = encryData[0];
    siteselection = encryData[1];
    jobselection = encryData[2];
    page = encryData[3];
    var SiteID = encryData[4];
    fromtocheck = encryData[5];
    ddl1to = encryData[6];
    ddl1from = encryData[7];


    //    foreach (GridDataItem item in gridReturn.Items)
    //{
    //            CheckBox chk = (CheckBox)item.FindControl("Chkselected");
    //    if (chk.Checked)
    //    {
    //        Flag = true;
    //        Response.Write(item["ReturnInvId"].Text.ToString());
    //        RInvId = Convert.ToInt32(item.GetDataKeyValue("ReturnInvId").ToString());
    //        Response.Write(item["FromId"].Text.ToString());
    //        siteselection = item.GetDataKeyValue("FromId").ToString();
    //        Response.Write(item["ToId"].Text.ToString());
    //        jobselection = item.GetDataKeyValue("ToId").ToString();
    //        count++;
    //    }

    //}


    //  if (rdodepottosite.Checked == true)
    if ($('#rdodepottosite').prop('checked') == true) {
        //  if (ddlfrom.SelectedValue != null && ddlfrom.SelectedValue != "")

        if ($('#ddlfrom option:selected').val() != "" && $('#ddlfrom option:selected').val() != undefined) {

            window.location.href = '/ReturnInventoryDetails/ReturnInventoryDetails?UID=' + UID + '&UserRole=' + UserRole + '&id=' + ClientID + '&RInvId=' + RInvId + '&fromtocheck=' + fromtocheck + '&FromId=' + SiteID + '&ToId=' + ClientID + '&page=' + page;

            //  Response.Redirect("ReturnInventoryDetails.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&fromtocheck=" + databaseCon.Encrypt("1") + "&id=" + Request.QueryString["id"] + "&FromId=" + databaseCon.Encrypt(ddlfrom.SelectedValue) + "&ToId=" + Request.QueryString["id"] + "&RInvId=" + databaseCon.Encrypt(RInvId.ToString()) + "&page=" + databaseCon.Encrypt("1"));

        }
        else if (Flag == true) {
            if (count == 1) {
                window.location.href = '/ReturnInventoryDetails/ReturnInventoryDetails?UID=' + UID + '&UserRole=' + UserRole + '&id=' + ClientID + '&RInvId=' + RInvId + '&fromtocheck=' + fromtocheck + '&FromId=' + siteselection + '&ToId=' + ClientID + '&page=' + page;

                //Response.Redirect("ReturnInventoryDetails.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&fromtocheck=" + databaseCon.Encrypt("1") + "&id=" + Request.QueryString["id"] + "&FromId=" + databaseCon.Encrypt(siteselection) + "&ToId=" + Request.QueryString["id"] + "&RInvId=" + databaseCon.Encrypt(RInvId.ToString()) + "&page=" + databaseCon.Encrypt("1"));
            }
            else {
                $('#art1').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
                $('#art1').css("display", "block");
                $('#alertlabel').text("Alert ! Please Select one Request.Multiple Selection Not Allowed.");

            }

        }
        else {
            $('#art1').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
            $('#art1').css("display", "block");
            $('#alertlabel').text("Alert ! Please Select Site.");

        }
    }

    //else if (rdositetojob.Checked == true)
    else if ($('#rdositetojob').prop('checked') == true) {
        //if (ddl1from.SelectedValue != "" && ddl1to.SelectedValue != "")
        if ($('#ddl1from option:selected').val() != "" && $('#ddl1from option:selected').val() != undefined && $('#ddl1to option:selected').val() != "" && $('#ddl1to option:selected').val() != undefined) {
            window.location.href = '/ReturnInventoryDetails/ReturnInventoryDetails?UID=' + UID + '&UserRole=' + UserRole + '&id=' + ClientID + '&RInvId=' + RInvId + '&fromtocheck=' + fromtocheck + '&FromId=' + ddl1from + '&ToId=' + ddl1to + '&page=' + page;

            //Response.Redirect("ReturnInventoryDetails.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&fromtocheck=" + databaseCon.Encrypt("2") + "&id=" + Request.QueryString["id"] + "&FromId=" + databaseCon.Encrypt(ddl1from.SelectedValue) + "&ToId=" + databaseCon.Encrypt(ddl1to.SelectedValue) + "&RInvId=" + databaseCon.Encrypt(RInvId.ToString()) + "&page=" + databaseCon.Encrypt("1"));
        }
        else if (Flag == true) {
            if (count == 1) {
                window.location.href = '/ReturnInventoryDetails/ReturnInventoryDetails?UID=' + UID + '&UserRole=' + UserRole + '&id=' + ClientID + '&RInvId=' + RInvId + '&fromtocheck=' + fromtocheck + '&FromId=' + siteselection + '&ToId=' + jobselection + '&page=' + page;

                // Response.Redirect("ReturnInventoryDetails.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&fromtocheck=" + databaseCon.Encrypt("2") + "&id=" + Request.QueryString["id"] + "&FromId=" + databaseCon.Encrypt(siteselection) + "&ToId=" + databaseCon.Encrypt(jobselection) + "&RInvId=" + databaseCon.Encrypt(RInvId.ToString()) + "&page=" + databaseCon.Encrypt("1"));
            }
            else {
                $('#art1').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
                $('#art1').css("display", "block");
                $('#alertlabel').text("Alert ! Please Select one Request.Multiple Selection Not Allowed.");
            }

        }
        else {
            $('#art1').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
            $('#art1').css("display", "block");
            $('#alertlabel').text("Alert ! Please Select Site/Job.");
        }
    }

    else if ($('#rdomaindepottodepot').prop('checked') == true) {

        if (Flag == true) {
            if (count == 1) {
                window.location.href = '/ReturnInventoryDetails/ReturnInventoryDetails?UID=' + UID + '&UserRole=' + UserRole + '&id=' + ClientID + '&RInvId=' + RInvId + '&fromtocheck=' + fromtocheck + '&FromId=' + ClientID + '&ToId=' + ClientID + '&page=' + page;


            }
            else {
                $('#art1').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
                $('#art1').css("display", "block");
                $('#alertlabel').text("Alert ! Please Select one Request.Multiple Selection Not Allowed.");
            }



        }
        else {
            window.location.href = '/ReturnInventoryDetails/ReturnInventoryDetails?UID=' + UID + '&UserRole=' + UserRole + '&id=' + ClientID + '&RInvId=' + RInvId + '&fromtocheck=' + fromtocheck + '&FromId=' + ClientID + '&ToId=' + ClientID + '&page=' + page;

        }

    }

    else {
        $('#art1').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
        $('#art1').css("display", "block");
        $('#alertlabel').text("Alert ! Please Select Any One.");

    }

}

function PipeInventoryHide() {
    var ClientID = getParameterByName('id');
    $.ajax({
        type: "POST",
        url: "/InventoryManagement/PipeInventoryHide",
        data: JSON.stringify({ id: ClientID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            var Result = response.result;
            if (Result == "success") {
                $('#AnchorPipeLineInventory').css('display', 'Block');
            }
            else {
                $('#AnchorPipeLineInventory').css('display', 'none');
            }
        }
    });
}



function IsInactive(ReturnInvId) {
    if ($('#rdomaindepottodepot').prop('checked')) {
        return false;
    }
    if ($('#rdodepottosite').prop('checked')) {
        var SiteId = $('#ddlfrom').val();
        if (SiteId != "" && SiteId != null && SiteId != "All") {
            return $("#ddlfrom option:selected").text().indexOf("(InActive)") !== -1;
        }
        var fromText = $("#FromName_" + ReturnInvId).text();
        return fromText.indexOf("(InActive)") !== -1;
    }
    if ($('#rdositetojob').prop('checked')) {
        var SiteId = $('#ddl1to').val();
        if (SiteId != "" && SiteId != null && SiteId != "All") {
            return $("#ddl1to option:selected").text().indexOf("(InActive)") !== -1;
        }
        var toText = $("#ToName_" + ReturnInvId).text();
        return toText.indexOf("(InActive)") !== -1;
    }
    return false;
}

function IsSelectedRecordInactive() {
    var TempArry = [];
    for (var i = 0; i < ArrayIdxArr.length; i++) {
        TempArry = ArrayIdxArr[i];
    }
    if (!TempArry || TempArry.length == 0)
        return false;
    var ReturnInvId = TempArry[0];
    return IsInactive(ReturnInvId);
}

