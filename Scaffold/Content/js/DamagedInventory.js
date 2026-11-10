$(function () {
    $('#ddlfrom').on('change', function () {
        GetDamageInventoryData();
    });

    $('#ddl1from').on('change', function () {
        GetDamageInventoryData();
    });

    $('#ddl1to').on('change', function () {
        ddl1to_SelectedIndexChanged();
    });
    SiteDropDownBind();
    var Href = window.location.href.split('?');
    if (Href.length == 1) {
        $('#UlNavTabSeciton li').css('display', 'none');
        $('#liThirdParty').css('display', 'block');
    }
    $('#UlNavTabSeciton a').css('cursor', 'pointer');
    $('#UlNavTabSeciton a').click(function (e) {
        $('#LoadingPanel1').css('display', 'block');
        var ClickAcnhorID = e.currentTarget.id;
        if (ClickAcnhorID == 'AnchorYardInventory') {
           // window.location.href = '/InventoryManagement.aspx?' + Href[1];
            window.location.href = '/InventoryManagement/InventoryManagement?' + Href[1];
        }

        if (ClickAcnhorID == 'AnchorReturnInventory') {
          //  window.location.href = '/ReturnInventory.aspx?' + Href[1];
            window.location.href = '/ReturnInventory/ReturnInventory?' + Href[1];
        }
        if (ClickAcnhorID == 'AnchorJobInventory') {
           // window.location.href = '/RequestInventory.aspx?' + Href[1];
            window.location.href = '/RequestInventory/RequestInventory?' + Href[1];
        }
        if (ClickAcnhorID == 'AnchorDepotInventory') {
           
            window.location.href = '/MainDepotInventory/MainDepotInventory?' + Href[1];
        }
        if (ClickAcnhorID == 'AnchorSiteInventory') {
           // window.location.href = '/SiteInventory.aspx?' + Href[1];
            window.location.href = '/SiteInventory/SiteInventory?' + Href[1];
        }
        if (ClickAcnhorID == 'AnchorDamagedInventory') {
          //  window.location.href = '/DamagedInventory.aspx?' + Href[1];
            window.location.href = '/DamagedInventory/DamagedInventory?' + Href[1];
        }
        if (ClickAcnhorID == 'AnchorPipeLineInventory') {
            //window.location.href = '/ReturnInventory/ReturnInventory?' + Href[1];
            window.location.href = '/PipeLineInventory/PipeLineInventory?UID=' + Href[1];
        }
    });

});

$(document).ready(function () {

    // SiteDropDownBind();
    //SitetoJobDropDown();
    PipeInventoryHide();
});

function btnAllDmgReport_Click() {

    var rdodepottosite = $('#rdodepottosite').prop('checked');
    var rdositetojob = $('#rdositetojob').prop('checked');
    var rdodepottodepot = $('#rdodepottodepot').prop('checked');

    var ClientID = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');

    if ((rdodepottodepot == true) || (rdodepottosite == true) || rdositetojob == true) {
    $.ajax({
        type: "POST",
        url: "/DamagedInventory/GetAllDamageData",

        data: JSON.stringify({ id: ClientID, rdodepottosite: rdodepottosite, rdositetojob: rdositetojob, rdodepottodepot: rdodepottodepot }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {

            var GetData = response;
            if (response.result == "Success") {
                window.location.href = '/AllDamageReport/AllDamageReport?UID=' + UID + '&UserRole=' + UserRole + '&id=' + ClientID + '&Check=' + response.Message;
    }
            if (response.result == "Failed") {
                $('#art1').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
                $('#art1').css("display", "block");
                $('#alertlabel').text("Alert !" + response.Message);
    }
    }
    });
}
else
{
        $('#art1').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
    $('#art1').css("display", "block");
    $('#alertlabel').text("Alert ! Please Select Any One.");

}

}

// Depot To Depot Rediobutton Change
function rdodepottodepot_CheckedChanged() {
    //DtoS.Style.Add("display", " none");
    //StoJ.Style.Add("display", "none");
    //displaygrid.Style.Add("display", "block");
    $('#DtoS').css('display', 'none');
    $('#StoJ').css('display', 'none');
    $('#displaygrid').css('display', 'block');
     GetDamageInventoryData();
}

//Depot To site RadioButton Changed

function rdodepottosite_CheckedChanged() {
   // $('#LoadingPanel1').css('display', 'block');
    $('#DtoS').css('display', 'block');
    $('#StoJ').css('display', 'none');
    // gridReturn.Rebind();
    $('#displaygrid').css('display', 'block');
    GetDamageInventoryData();
    SiteDropDownBind();
}

// site to Job RadioButton Changed
function rdositetojob_CheckedChanged() {
    $('#displaygrid').css('display', 'block');
   // $('#LoadingPanel1').css('display', 'block');
    $('#StoJ').css('display', 'block');
    $('#DtoS').css('display', 'none');
    GetDamageInventoryData();
  SitetoJobDropDown();

}

function ddlfrom_SelectedIndexChanged() {


    GetDamageInventoryData();
}


function ddl1to_SelectedIndexChanged() {
    //function GetJobDropDownBind() {
    var ClientID = getParameterByName('id');
    var ddl1to = $('#ddl1to option:selected').val();
    if (ddl1to == "" || ddl1to == undefined) {
        ddl1to = "All";
    }
    $.ajax({
        type: "POST",
        url: "/DamagedInventory/GetJobDropDownBind",

        data: JSON.stringify({ id: ClientID, JobSiteid: ddl1to }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {

            var GetData = response;
            $("#ddl1from").empty();
            $("#ddl1from").append('<option value="">All</option>');
            if (GetData.length > 0) {
                for (var i = 0; i < GetData.length; i++) {
                    $("#ddl1from").append("<option value='" + GetData[i].RegisterId + "'>" + GetData[i].ScaffRef + "</option>");
                }
            }
        }
    });
    GetDamageInventoryData();
}

//}
function ddl1from_SelectedIndexChanged() {
    GetDamageInventoryData();
}

//Site Dropdown Bind
function SiteDropDownBind() {
    var ClientID = getParameterByName('id');
    var UID = getParameterByName('UID');
    $('#LoadingPanel1').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/DamagedInventory/SiteDropDown",

        data: JSON.stringify({ id: ClientID, UID: UID  }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {

            var SiteData = response;
            var SiteLocId = "";
            if (SiteData.length > 0) {

               
                $("#ddlfrom").empty();
                $("#ddl1to").empty();
                $("#ddl1to").append('<option value="">All</option>');
                //$("#ddlfrom").append('<option value="">All</option>');
                for (var i = 0; i < SiteData.length; i++) {
                    SiteLocId = SiteData[i].SiteLocID;
                    if (SiteLocId == 0) {
                        SiteLocId = "";
                    }
                    $("#ddlfrom").append("<option value='" + SiteLocId + "'>" + SiteData[i].SiteLocation + "</option>");

                }
                ddlfrom_SelectedIndexChanged();
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
        url: "/DamagedInventory/SitetoJobDropDown",
        data: JSON.stringify({ id: ClientID, UID: UID  }),
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
                    $("#ddl1to").append("<option value='" + SiteLocId + "'>" + SiteData[i].SiteLocation + "</option>");

                }

                ddl1to_SelectedIndexChanged();
            }
        }
    })
}


var table;
var ArrayIdxArr = [0];
function GetDamageInventoryData() {
    ArrayIdxArr.length = 0;
    $('#LoadingPanel1').css('display', 'block');
    var ddlfrom = $('#ddlfrom option:selected').val();
    if (ddlfrom == "" || ddlfrom == undefined) {
        ddlfrom = "All";
    }


    var ddl1to = $('#ddl1to option:selected').val();
    if (ddl1to == "" || ddl1to == undefined) {
        ddl1to = "All";
    }
    var ddl1from = $('#ddl1from option:selected').val();
    if (ddl1from == "" || ddl1from == undefined) {
        ddl1from = "All";
    }
    var id = getParameterByName('id');
    var rdodepottosite = $('#rdodepottosite').prop('checked');
    var rdositetojob = $('#rdositetojob').prop('checked');
    var rdodepottodepot = $('#rdodepottodepot').prop('checked');

    $.ajax({
        type: "POST",
        url: "/DamagedInventory/GetDamageInventoryData",
        // data: JSON.stringify({ id: ClientID, FromId: FromId, ToId: ToId, ddl1from: ddl1from, ddl1to: ddl1to, JobSiteid: JobSiteid, ddlfrom: ddlfrom, rdodepottosite: rdodepottosite, rdositetojob: rdositetojob, RID: RID }),
        data: JSON.stringify({ id: id, ddlfrom: ddlfrom, ddl1to: ddl1to, ddl1from: ddl1from, rdodepottosite: rdodepottosite, rdositetojob: rdositetojob ,rdodepottodepot:rdodepottodepot}),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            var str = "";
            var GetData = response;
            //$('#tblDamagedInventoryGrid').DataTable().clear();
            //$('#tblDamagedInventoryGrid').DataTable().destroy();
            if ($.fn.DataTable.isDataTable('#tblDamagedInventoryGrid')) {
                $('#tblDamagedInventoryGrid').DataTable().clear();
                $('#tblDamagedInventoryGrid').DataTable().destroy();
            }

            if (GetData.length > 0) {
                $('#tblDamagedInventoryGrid').css("display", "block");

                for (var i = 0; i < GetData.length; i++) {
                    var FromId = GetData[i].FromId == null ? "" : GetData[i].FromId;
                    var ToId = GetData[i].ToId == null ? "" : GetData[i].ToId;
                    var DamagedInvId = GetData[i].DamagedInvId == null ? "" : GetData[i].DamagedInvId;
                    var FromName = GetData[i].FromName == null ? "" : GetData[i].FromName;
                    var ToName = GetData[i].ToName == null ? "" : GetData[i].ToName;
                    var CreatedBy = GetData[i].CreatedBy == null ? "" : GetData[i].CreatedBy;
                    var CreatedOn = GetData[i].CreatedOn == null ? "" : GetData[i].CreatedOn;
                    var Name = GetData[i].Name == null ? "" : GetData[i].Name;
                  
                    str += "<tr>"
                    str += "<td style='display:none'></td>"
                    str += "<td  style='width:5%;'><a id='DamagedInvIdexpend_" + DamagedInvId + "' class='expand fa fa-angle-right'  style='font-size:19px;'></a></td>";
                    str += "<td  id='DamagedInvId_" + i + "' style='text-align:Left;width:5%;'><input type='checkbox' class='chkDamaged setWidth' DamagedInvId_='" + DamagedInvId + "'/></td>"
                    str += "<td style='width:25%; text-align: Left' class='setWidth' id='FromName_" + DamagedInvId + "'>" + FromName + "</td>"
                    str += "<td style='width:20%; text-align: Left '  class='setWidth' id='ToName_" + DamagedInvId + "'>" + ToName + "</td>"
                    str += "<td style='width:25%; text-align: Left' class='setWidth'>" + Name + "</td>"
                    str += "<td style='width:20%; text-align: Left' class='setWidth' >" + CreatedOn + "</td>"
                    str += "<td id='FromID_" + DamagedInvId + "' style='display:none'>" + FromId + "</td>"
                    str += "<td id='ToID_" + DamagedInvId + "' style='display:none'>" + ToId + "</td>"
                    str += "</tr>"
                }

                $('#tbodyDamagedInventoryGrid').empty();
                $('#tbodyDamagedInventoryGrid').append(str);
                $('#LoadingPanel1').css('display', 'none');

               table= $('#tblDamagedInventoryGrid').DataTable({
                    //"bSort": false,
                    //"processing": true,
                    //searching: true,
                    //"bLengthChange": false,
                    //"bInfo": false,
                    //autoWidth: false,
                    //scrollX: true,
                    //scrollCollapse: true,
                    //scrollY: "60vh",
                    //"paging": false,
                    //"ordering": false,
                    //"info": false

                    searching: true,
                    paging: false,
                    info: false,
                    autoWidth: true,
                    length: false,
                    sort: false,

                    "drawCallback": function () {
                        $('.fa-angle-right').click(function (eve) {
                            eve.stopImmediatePropagation();
                            var tr = $(this).closest('tr');
                            var row = table.row(tr);
                            // var DamagedInvId = $(this).attr('#DamagedInvIdexpend_').split('_');
                            // var DamagedInvId = $(this).attr('#DamagedInvIdexpend_');
                            //  var DamagedInvId = $(this).attr('DamagedInvIdexpend_').split('_')[0];

                            // var DamagedInvId = $(this).id.split('_')[0];
                            var DamagedInvId = this.id.split('_')[1];

                            var FromID = $('#FromID_' + DamagedInvId).text();

                            var visible = $(this).parent().parent().hasClass("shown");
                            var anglechange = $(this).hasClass("fa-angle-right");
                            if (anglechange == true) {
                                var x = tr.children()[0];
                                $(this).removeClass("fa-angle-right");
                                $(this).addClass("fa-angle-down");
                            }
                            else {
                                $(this).addClass("fa-angle-right");
                                $(this).removeClass("fa-angle-down");
                            }
                            if (visible == true) {
                                // This row is already open - close it
                                $(this).parent().parent().next('tr').remove();
                                $(this).parent().parent().removeClass('shown');
                            }
                            else {
                                // Open this row
                                // row.child(format(row.data())).show();
                                row.child(DamageInventoryChildGrid(DamagedInvId)).show();
                                // DamageInventoryChildGrid(DamagedInvId).show();
                                row.child.show();
                                tr.addClass('shown');
                            }
                        });
                    }

                });

               


               $(".chkDamaged").click(function () {
                    event.stopImmediatePropagation();
                    var DamagedInvId = $(this).attr('DamagedInvId_').split('_')[0];
                    var ColumnChecked = $(this).prop('checked');
                    var FromID = $('#FromID_' + DamagedInvId).text();
                    var ToID = $('#ToID_' + DamagedInvId).text();

                    var ArryVal = [DamagedInvId, FromID, ToID];

                    if (ColumnChecked == true) {
                        //RequestInventoryIdxArr.push(transID);
                        ArrayIdxArr.push(ArryVal);
                    }
                    else {
                        for (var i = 0; i < ArrayIdxArr.length; i++) {
                            var TempArry = ArrayIdxArr[i];
                            if (TempArry[0] == DamagedInvId) {
                                ArrayIdxArr.splice(i, 1);
                            }
                        }
                    }
                });


            } else {
                // No Data Code here
                $('#tblDamagedInventoryGrid').css("display", "none");
                $('#LoadingPanel1').css('display', 'none');
            }
        }
    })

}



function DamageInventoryChildGrid(d) {
    $("#ajaxLoader").css("display", "block");
    // var id = $("#ContentPlaceHolder1_hdnid").val();
    var id = getParameterByName('id');
    var str = "";
    str = '<table id="Clone_' + d + '" class="clone"  cellspacing="0" border="0" style="padding-left:50px;width:100%">';
    str += '<thead><tr>';
    str += '<th style="display:none">DIDetailID</th>';
    str += '<th style="display:none">From Name</th>';
    str += '<th style="display:none">From ID</th>';
    str += '<th style="display:none">To ID</th>';
    str += '<th style="width:9%;text-align:center;">View</th>';
    str += '<th style="width:10%;text-align:center">Damage Report #</th>';
    str += '<th style="width:20%;text-align:center">Material Type</th>';
    str += '<th style="width:20%;text-align:center">Item Code</th>';
    str += '<th style="width:10%;text-align:center">Item Desc</th>';
    str += '<th style="width:10%;text-align:center">Quantity</th>';
    str += '<th style="width:20%;text-align:center">Reason for Damage</th>';
    
    str += '</tr></thead><tbody>';
    str += '</tbody></table>';
    $.ajax({
        type: "POST",
        // url: "../WebApi/AllInventory.asmx/EditData",
        url: "/DamagedInventory/DamageInventoryChildGridData",
        data: JSON.stringify({ id: id, DamagedInvId:d }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            // var data = response.d;
            var data = response;
            if (data.length > 0) {


                for (var j = 0; j < data.length; j++) {
                    var DIDetailID = data[j].DIDetailID == null ? "" : data[j].DIDetailID;
                    var FromId = data[j].FromId == null ? "" : data[j].FromId;
                    var ToId = data[j].ToId == null ? "" : data[j].ToId;
                    var FromName = data[j].FromName == null ? "" : data[j].FromName;
                    var DamagedInvId = data[j].DamagedInvId == null ? "" : data[j].DamagedInvId;
                    var MaterialType = data[j].MaterialType == null ? "" : data[j].MaterialType;
                    var ItemCode = data[j].ItemCode == null ? "" : data[j].ItemCode;
                    var ItemDesc = data[j].ItemDesc == null ? "" : data[j].ItemDesc;
                    var DamagedQuantity = data[j].DamagedQuantity == null ? "" : data[j].DamagedQuantity;
                    var Reason = data[j].Reason == null ? "" : data[j].Reason;
                    									
                    str += '<tr>';
                    str += '<td id="DIDetailIDView_' + DIDetailID + '" style="display:none">' + DIDetailID + '</td>';
                    str += '<td id="FromName_' + DIDetailID + '" style="display:none">' + FromName + '</td>';
                    str += '<td id="FromId_' + DIDetailID + '" style="display:none">' + FromId + '</td>';
                    str += '<td id="ToId_' + DIDetailID + '" style="display:none">' + ToId + '</td>';
                    str += "<td title='View Record' style='cursor:pointer;width:10%;'><button type='button' id='tbnView' Class='w3-btn w3-teal btnView' DIDetailID_='" + DIDetailID + "' ><i class='fa fa-search'></i>&nbsp;View</button></td>";
                    str += '<td id="DamagedInvId_' + DIDetailID + '" style="width:10%;text-align:center">' + DIDetailID + '</td>';
                    str += '<td style="width:20%;text-align:center">' + MaterialType + '</td>';
                    str += '<td style="width:20%;text-align:center">' + ItemCode + '</td>';
                    str += '<td style="width:10%;text-align:center">' + ItemDesc + '</td>';
                    str += '<td style="width:10%;text-align:center">' + DamagedQuantity + '</td>';
                    str += '<td style="width:20%;text-align:center">' + Reason + '</td>';
                    str += '</tr>';
                    //row.child(format(st).show());
                }
                // str += st;

            }


            console.log(d);
           // var check = d.DT_RowId;
            // $("#Clone_" + d.DT_RowId).html(str);
            $("#Clone_" + d).html(str);
            $("#ajaxLoader").css("display", "none");


            $('.btnView').click(function () {
                event.stopImmediatePropagation();
                var DIDetailID = $(this).attr('DIDetailID_').split('_')[0]; //get DamagedInvId
                var FromID = $('#FromId_' + DIDetailID).text();
                var ToID = $('#ToId_' + DIDetailID).text();
                var FromName = $('#FromName_' + DIDetailID).text();
                var DamagedInvId = $('#DamagedInvId_' + DIDetailID).text();  //get DIDetailID

                var ClientID = getParameterByName('id');
                var UID = getParameterByName('UID');
                var UserRole = getParameterByName('UserRole');

                var rdodepottosite = $('#rdodepottosite').prop('checked');
                var rdositetojob = $('#rdositetojob').prop('checked');
                var rdodepottodepot = $('#rdodepottodepot').prop('checked');

                var fromtocheck = "";

                if (rdodepottodepot == true)
                {
                    fromtocheck = "3";
                }
                else if (rdodepottosite == true)
                {
                    fromtocheck = "1";
                }
                else if (rdositetojob== true)
                {
                    fromtocheck = "2";
                }

                var RInvId = "";

                var EncryptData = [DIDetailID, FromName, FromID, ToID, fromtocheck, DamagedInvId,RInvId];
                var encryData = Encrypt(EncryptData);
                DIDetailID = encryData[0];
                FromName = encryData[1];
                FromID = encryData[2];
                ToID=encryData[3];
                fromtocheck=encryData[4];
                DamagedInvId = encryData[5];
                RInvId = encryData[6];
           
                if (DIDetailID != "") {
                    window.open('/DamagedReport/DamagedReport?UID=' + UID + '&UserRole=' + UserRole + '&id=' + ClientID + '&fromtocheck=' + fromtocheck + '&DIDetailID=' + DIDetailID + '&DamagedInvId=' + DamagedInvId + '&FromId=' + FromID + '&ToId=' + ToID + '&FromName=' + FromName + '&RInvId=' + RInvId);
                }
            });
        }
    });
    return str;
    //    // `d` is the original data object for the row
}

function btnDamagedInventory_Click()


{
    
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
        else if (IsSelectedDamagedRecordInactive()) {
            CustomAlert({
                Result: "Failed",
                Message: "This site is InActive. Editing is not allowed."
            });
            return false;
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
        else if (IsSelectedDamagedRecordInactive()) {
            CustomAlert({
                Result: "Failed",
                Message: "This site is InActive. Editing is not allowed."
            });
            return false;
        }
    }
    if (IsSelectedDamagedRecordInactive()) {
        CustomAlert({
            Result: "Failed",Message: "This site is InActive. Editing is not allowed."});
        return false;
    }
        var DamagedInvId = 0, count = 0;
        var Flag = false;
        var siteselection = "", jobselection = "", fromtocheck = "", page = "1";
        var TempArry = [];

        var ddlfrom = $('#ddlfrom option:selected').val();
        var ddl1to = $('#ddl1to option:selected').val();
        var ddl1from = $('#ddl1from option:selected').val();

        var rdodepottosite = $('#rdodepottosite').prop('checked');
        var rdositetojob = $('#rdositetojob').prop('checked');
        var rdodepottodepot = $('#rdodepottodepot').prop('checked');

        if(rdodepottosite==true)
        {
            fromtocheck="1";
        }
        if(rdositetojob==true){
            fromtocheck="2";
        }
        if(rdodepottodepot==true)
        {
            fromtocheck="3";
        }


        for (var i = 0; i < ArrayIdxArr.length; i++) {
            Flag = true;
            TempArry = ArrayIdxArr[i];
            count += 1;
        }
        DamagedInvId = TempArry[0];
        siteselection = TempArry[1];
        jobselection = TempArry[2];

        var UID = getParameterByName('UID');
        var UserRole = getParameterByName('UserRole');
        var id = getParameterByName('id');

        var Data = [fromtocheck, ddlfrom, ddl1to, ddl1from, page, siteselection, jobselection, DamagedInvId];
        var EncriptData = Encrypt(Data);
          
        fromtocheck = EncriptData[0];
        ddlfrom = EncriptData[1];
        ddl1to = EncriptData[2];
        ddl1from = EncriptData[3];
        page = EncriptData[4];
        siteselection=EncriptData[5];
        jobselection=EncriptData[6];
        DamagedInvId = EncriptData[7];

        if (rdodepottosite == true) {
            if ($('#ddlfrom option:selected').val() != "" && $('#ddlfrom option:selected').val() != undefined){

                window.location.href = '/DamagedInventoryDetails/DamagedInventoryDetails?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&fromtocheck=' + fromtocheck + '&FromId=' + ddlfrom + '&ToId=' + id + '&DamagedInvId=' + DamagedInvId + '&page=' + page;
              //  Response.Redirect("DamagedInventoryDetails.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&fromtocheck=" + databaseCon.Encrypt("1") + "&id=" + Request.QueryString["id"] + "&FromId=" + databaseCon.Encrypt(ddlfrom.SelectedValue) + "&ToId=" + Request.QueryString["id"] + "&DamagedInvId=" + databaseCon.Encrypt(DamagedInvId.ToString()) + "&page=" + databaseCon.Encrypt("1"));
            }
            else if (Flag == true) {
                if (count == 1) {
                    window.location.href = '/DamagedInventoryDetails/DamagedInventoryDetails?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&fromtocheck=' + fromtocheck + '&FromId=' + siteselection + '&ToId=' + id + '&DamagedInvId=' + DamagedInvId + '&page=' + page;

                   // Response.Redirect("DamagedInventoryDetails.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&fromtocheck=" + databaseCon.Encrypt("1") + "&id=" + Request.QueryString["id"] + "&FromId=" + databaseCon.Encrypt(siteselection) + "&ToId=" + Request.QueryString["id"] + "&DamagedInvId=" + databaseCon.Encrypt(DamagedInvId.ToString()) + "&page=" + databaseCon.Encrypt("1"));
                }
                else {
                    $('#art1').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed').css("display", "block");
                    $('#alertlabel').text("Alert ! Please Select one Request.Multiple Selection Not Allowed.");
                }

            }
            else {
                $('#art1').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed').css("display", "block");
                $('#alertlabel').text("Alert ! Please Select Site.");
            }
        }
        else if (rdositetojob == true)
        {
           // if (ddl1from.SelectedValue != "" && ddl1to.SelectedValue != "")
            if ($('#ddl1from option:selected').val() != "" && $('#ddl1from option:selected').val() != undefined && $('#ddl1to option:selected').val() != "" && $('#ddl1to option:selected').val() != undefined)
            {
                window.location.href = '/DamagedInventoryDetails/DamagedInventoryDetails?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&fromtocheck=' + fromtocheck + '&FromId=' + ddl1from + '&ToId=' + ddl1to + '&DamagedInvId=' + DamagedInvId + '&page=' + page;

               // Response.Redirect("DamagedInventoryDetails.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&fromtocheck=" + databaseCon.Encrypt("2") + "&id=" + Request.QueryString["id"] + "&FromId=" + databaseCon.Encrypt(ddl1from.SelectedValue) + "&ToId=" + databaseCon.Encrypt(ddl1to.SelectedValue) + "&DamagedInvId=" + databaseCon.Encrypt(DamagedInvId.ToString()) + "&page=" + databaseCon.Encrypt("1"));
            }
            else if (Flag == true)
            {
                if (count == 1)
                {
                    window.location.href = '/DamagedInventoryDetails/DamagedInventoryDetails?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&fromtocheck=' + fromtocheck + '&FromId=' + siteselection + '&ToId=' + jobselection + '&DamagedInvId=' + DamagedInvId + '&page=' + page;
                    //Response.Redirect("DamagedInventoryDetails.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&fromtocheck=" + databaseCon.Encrypt("2") + "&id=" + Request.QueryString["id"] + "&FromId=" + databaseCon.Encrypt(siteselection) + "&ToId=" + databaseCon.Encrypt(jobselection) + "&DamagedInvId=" + databaseCon.Encrypt(DamagedInvId.ToString()) + "&page=" + databaseCon.Encrypt("1"));
                }
                else
                {
                     $('#art1').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed').css("display", "block");
                    $('#alertlabel').text("Alert !  Please Select one Request.Multiple Selection Not Allowed.");
                }

            }
            else
            {
                $('#art1').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed').css("display", "block");
                $('#alertlabel').text("Alert ! Please Select Site/Job.");
            }
        }
        else if (rdodepottodepot == true)
        {

            if (Flag == false)
            {
                window.location.href = '/DamagedInventoryDetails/DamagedInventoryDetails?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&fromtocheck=' + fromtocheck + '&FromId=' + id + '&ToId=' + id + '&DamagedInvId=' + DamagedInvId + '&page=' + page;
               // Response.Redirect("DamagedInventoryDetails.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&fromtocheck=" + databaseCon.Encrypt("3") + "&id=" + Request.QueryString["id"] + "&FromId=" + Request.QueryString["id"] + "&ToId=" + Request.QueryString["id"] + "&DamagedInvId=" + databaseCon.Encrypt(DamagedInvId.ToString()) + "&page=" + databaseCon.Encrypt("1"));
            }
            // else if (rdodepottodepot.Checked == true && Flag == true)
            else if (Flag == true)
            {
                if (count == 1)
                {
                    window.location.href = '/DamagedInventoryDetails/DamagedInventoryDetails?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&fromtocheck=' + fromtocheck + '&FromId=' + siteselection + '&ToId=' + jobselection + '&DamagedInvId=' + DamagedInvId + '&page=' + page;
                   // Response.Redirect("DamagedInventoryDetails.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&fromtocheck=" + databaseCon.Encrypt("3") + "&id=" + Request.QueryString["id"] + "&FromId=" + databaseCon.Encrypt(siteselection) + "&ToId=" + databaseCon.Encrypt(jobselection) + "&DamagedInvId=" + databaseCon.Encrypt(DamagedInvId.ToString()) + "&page=" + databaseCon.Encrypt("1"));
                }
                else
                {
                    $('#art1').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed').css("display", "block");
                    $('#alertlabel').text("Alert ! Please Select one Request.Multiple Selection Not Allowed.");
                }
            }
            else
            {
                 $('#art1').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed').css("display", "block");
                 $('#alertlabel').text("Alert ! Please Select Site.");
            }

        }



        else
        {
            $('#art1').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed').css("display", "block");
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

function IsDamagedInventoryInactive(DamagedInvId) {

    if ($('#rdodepottodepot').prop('checked')) {
        return false;
    }
    if ($('#rdodepottosite').prop('checked')) {
        if ($("#ddlfrom").val() != "" && $("#ddlfrom").val() != "All") {
            return $("#ddlfrom option:selected").text().indexOf("(InActive)") !== -1;
        }
        var fromText = $("#FromName_" + DamagedInvId).text();
        if (!fromText) {
            fromText = $("#FromName_" + DamagedInvId).attr("title");
        }
        return fromText.indexOf("(InActive)") !== -1;
    }

    if ($('#rdositetojob').prop('checked')) {
        if ($("#ddl1to").val() != "" && $("#ddl1to").val() != "All") {
            return $("#ddl1to option:selected").text().indexOf("(InActive)") !== -1;
        }
        var toText = $("#ToName_" + DamagedInvId).text();
        if (!toText) {
            toText = $("#ToName_" + DamagedInvId).attr("title");
        }
        return toText.indexOf("(InActive)") !== -1;
    }
    return false;
}

function IsSelectedDamagedRecordInactive() {
    var TempArry = [];
    for (var i = 0; i < ArrayIdxArr.length; i++) {
        TempArry = ArrayIdxArr[i];
    }
    if (!TempArry || TempArry.length == 0)
        return false;
    var DamagedInvId = TempArry[0];
    return IsDamagedInventoryInactive(DamagedInvId);
}