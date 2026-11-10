var GblQuantityCount = 0;

function Back_Click() {
    var Href = window.location.href.split('?');
    var id = getParameterByName('id');
    var FromId = getParameterByName('FromId');
    var page = getParameterByName('page');
    var txtdesc = $('#txtdesc').val();

    var DecriptData = [page];
    var DecrData = Decrypt(DecriptData);
    var pageID = DecrData[0];

    $.ajax({
        type: "POST",
        url: "/PipelineRequest/Back_Click",
        data: JSON.stringify({ FromId: FromId, page: page, txtdesc: txtdesc }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            if (response.result == "Success") {

                var UID = getParameterByName('UID');
                var UserRole = getParameterByName('UserRole');
                var id = getParameterByName('id');
                var RID = getParameterByName('FromId');

                window.location.href = '/ScaffoldMaterialListPhase3/ScaffoldMaterialListPhase3?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID;


            }

            window.location.href = '/ScaffoldMaterialListPhase3/ScaffoldMaterialListPhase3?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID;
        }
    })


}




function btnconfirm_Click() {
    // if (txtdesc.Text != "")
    if ($('#txtdesc').val() != "") {
        var RequestID = getParameterByName('ReqId');
        var DecryptData = Decrypt([RequestID]);
        RequestID = DecryptData[0];
        if (RequestID != 0) {

            $('#P2').css("display", "none");
            $('#btncancel').css("display", "block");
            // $('#lbltrandid').html(RequestID);
            $('#lbldescCon').html($('#txtdesc').val());
            $('#lblSupp').html($('#ddlSupplier option:selected').text());
            $('#divConfirmReq').css("display", "block");
        }
        else {
            var Response = { Result: "Failed", Message: "Please fill in Quantity to Request then click on Confirm Request." };
            CustomAlert(Response);
        }
    }
    else {
        var Response = { Result: "Failed", Message: "Please fill the Description." };
        CustomAlert(Response);
    }
}

function btnconfirmno_Click() {
    $('#confmodal').css("display", "block");
}



function btnredirection_Click() {


    var Href = window.location.href.split('?');
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var FromId = getParameterByName('FromId');
    var ToId = getParameterByName('ToId');
    var fromtocheck = getParameterByName('fromtocheck');
    var from = getParameterByName('from');
    var To = getParameterByName('To');
    var page = getParameterByName('page');
    var ReqId = $('#hdnRid').val();

    var EncryptData = [ReqId];
    var encryData = Encrypt(EncryptData);
    ReqId = encryData[0];

    window.location.href = '/PipelineRequest/PipelineRequest?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&ReqId=' + ReqId + '&FromId=' + FromId + '&ToId=' + ToId + '&fromtocheck=' + fromtocheck + '&from=' + from + '&To=' + To + '&page=' + page;

}

$(document).ready(function () {
    $('#txtdesc').keydown(function (eve) {
        if (eve.keyCode == 13) {
            return false;
        }
    })



    if ($('#hdnPage').val() == "2") {
        $('#btnbackSite').css("display", "none");
        $('#lblnote').css("display", "none");
        $('#ddlmat').css("display", "block");
        $('#lblJobDesc').css("display", "block");
        $('#jobdropdown').css("display", "block");

    }

    if ($('#hdnReqId').val() == "0") {
        $('#forinventory').css("display", "none");
    } else {
        $('#forinventory').css("display", "block");
        $('#forjobinventory').css("display", "none");
        $('#txtTransId').val($('#hdnReqId').val());

    }

    if (getParameterByName('ReqStatus') != null && $('#hdnconfirm').val() != "NotConfirmed" && getParameterByName('ReqStatus') != "") {
        $('#lockimage').css("display", "block");
        $('#hdntextdisable').val(true);
        $('#btnconfirm').css("display", "none");
        $('#btnSupplier').css("display", "none");
        //$('#btnrefresh').css("display", "none");

    }
    if ($('#hdnfromtocheck').val() == "1") {

        $('#lblto').text("To:");
        $('#lblfrom').text("From:");
        $('#jobdropdown').css("display", "none");

    }
    else if ($('#hdnfromtocheck').val() == "2") {
        // lblto.Text = "Site:";
        // lblfrom.Text = "Job:";
        $('#lblto').text("Site:");
        $('#lblfrom').text("Job:");
        $('#ddlSupp').css("display", "none");
        //$('#btnrefresh').css("display", "none");
    }
    else {
        $('#lblto').text("To:");
        $('#lblfrom').text("From:");
        $('#jobdropdown').css("display", "none");
    }

});


function handleMyInput(event) {

    if (event.key === 'Enter') {

        const { value, max } = event.currentTarget
        if (parseInt(value) > parseInt(max)) {
            var Response = { Result: "Failed", Message: 'Max value Allowed is : ' + max };
            CustomAlert(Response);

        }
    }
}

function MaterialTupeDropDownBind() {
    var id = getParameterByName('id');
    $.ajax({
        type: "POST",
        url: "/PipelineRequest/MaterialDropDown",
        data: JSON.stringify({ id: id }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {

            // var material = response.d;
            var material = response;
            if (material.length > 0) {
                $("#ddlMaterialType").append('<option value="">All</option>');
                for (var m = 0; m < material.length; m++) {
                    $("#ddlMaterialType").append("<option value='" + material[m].MaterialType + "'>" + material[m].MaterialType + "</option>");

                }
            }
        }
    })
}

var data;
var getmsg;
var valuesave;
var getvalue;
var getnotvalue;
var pageget;
var isExecuted = false;
function getdatachange(valuesave) {
    getvalue = valuesave;
    getnotvalue = valuesave;
    $("#hdngetMsg").val(getvalue);
    $("#hdnredirect").val(getnotvalue);
    getdatachangestart();
    if (getmsg == false && data == false) {
        $('#art1').addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
        $('#alertlabel').text("Alert! Please rectify the quantity marked in red.");
    }
    if (data == false) {
        $('#art1').addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
        $('#alertlabel').text("Alert! Please 'Click' on Save Icon for records that are highlighted in red.\n Then click on save and back Button.");
        $('#art1').delay(5000).fadeOut('slow');
        return false;
    }

}
function getdatachangestart() {
    var datatablelen = $("#hdntablelength").val();
    data = true;   
    getmsg = true;  
    for (var i = 0; i < datatablelen; i++) {
        var newQty = "#txtqty_" + i;
        var newQtyvalRaw = $(newQty).val();
        var newQtyval = (newQtyvalRaw == "" || newQtyvalRaw == undefined) ? "0" : newQtyvalRaw.toString().replace(/,/g, '').trim();

        var OldQty = "#OldQtyval_" + i;
        var OldQtyvalRaw = $(OldQty).val();
        var OldQtyval = (OldQtyvalRaw == "" || OldQtyvalRaw == undefined) ? "0" : OldQtyvalRaw.toString().replace(/,/g, '').trim();

        var Aval = "#hdnAvalid_" + i;
        var AvalQty = $(Aval).val();
        if (AvalQty != undefined) {
            AvalQty = AvalQty.replace(",", "");
        }
        if (parseFloat(newQtyval) !== parseFloat(OldQtyval)) {
            if (parseInt(newQtyval) > parseInt(AvalQty)) {
                getmsg = false;
            }
            data = false;
            return false;
        }
    }
    var pagedata = $("#hdngetpage").val();
    var msg = $("#hdngetMsg").val();
    var fromid = $("#hdnfromid").val();
    var redirect = $("#hdnredirect").val();
    if (redirect == "false") {
        var UID = getParameterByName('UID');
        var UserRole = getParameterByName('UserRole');
        var id = getParameterByName('id');
        var RID = getParameterByName('FromId');
        $("#ajaxLoader").css("display", "block");
        window.location.href = '/ScaffoldMaterialListPhase3/ScaffoldMaterialListPhase3?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID;
        return false;
    }
    if (pagedata == "") {
        $("#divConfirmReq").css("display", "block");
    }

    $("#hdngetpage").val("");
}
//function getdatachangestart() {
//    var datatablelen = $("#hdntablelength").val();
//    for (var i = 0; i < datatablelen; i++) {
//        var newQty = "#txtqty_" + i;
//        var newQtyval = $(newQty).val() == "" ? "0" : $(newQty).val();
//        var OldQty = "#OldQtyval_" + i;
//        var OldQtyval = $(OldQty).val();
//        var Aval = "#hdnAvalid_" + i;
//        var AvalQty = $(Aval).val();
//        if (AvalQty != undefined) {
//            AvalQty = AvalQty.replace(",", "");
//        }

//        if (newQtyval != OldQtyval) {
//            if (parseInt(newQtyval) > parseInt(AvalQty)) {
//                getmsg = false;
//            }
//            data = false;
//            return false;
//        }
//    }
//    data = true;
//    getmsg = true;
//    var pagedata = $("#hdngetpage").val();
//    var msg = $("#hdngetMsg").val();
//    var fromid = $("#hdnfromid").val();
//    var redirect = $("#hdnredirect").val();
//    if (redirect == "false") {
//        var UID = getParameterByName('UID');
//        var UserRole = getParameterByName('UserRole');
//        var id = getParameterByName('id');
//        var RID = getParameterByName('FromId');
//        $("#ajaxLoader").css("display", "block");
//        window.location.href = '/ScaffoldMaterialListPhase3/ScaffoldMaterialListPhase3?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID;
//        return false;
//    }
//    if (pagedata == "") {
//        $("#divConfirmReq").css("display", "block");
//    }

//    $("#hdngetpage").val("");
//}


function SaveDataProc_Click() {
    $("#divConfirmReq").css("display", "block");
}

function btnConfirmAndBack_Click() {
    var Href = window.location.href.split('?');
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var FromId = getParameterByName('FromId');
    var ToId = getParameterByName('ToId');
    var fromtocheck = getParameterByName('fromtocheck');
    var from = getParameterByName('from');
    var To = getParameterByName('To');
    var page = getParameterByName('page');
    var ReqId = getParameterByName('ReqId');
    // var ReqId =$('#hdnRid').val() ;
    var txtdesc = $('#txtdesc').val();
    var hdnflagforjobinsertion = $("#hdnflagforjobinsertion").val();

    $.ajax({
        type: "POST",
        url: "/PipelineRequest/btnConfirmAndBack_Click",
        data: JSON.stringify({ id: id, UID: UID, ReqId: ReqId, page: page, txtdesc: txtdesc, UserRole: UserRole, hdnflagforjobinsertion: hdnflagforjobinsertion, FromId: FromId, ToId: ToId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            var Result = response.result;
            if (Result == "Success") {

                if (response.Message == "ScaffoldMaterialListPhase3") {
                    var id = getParameterByName('id');
                    var UID = getParameterByName('UID');
                    var UserRole = getParameterByName('UserRole');
                    var FromId = getParameterByName('FromId');
                    window.location.href = '/ScaffoldMaterialListPhase3/ScaffoldMaterialListPhase3?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + FromId;
                }

            }
            else if (Result == "Failed") {
                if (response.Message == "Invalid Quantity") {
                    if (response.Data && response.Data.length > 0) {
                        var datamsg = ``;
                        $.each(response.Data, function (index, obj) {
                            datamsg += `<tr>`;
                            datamsg += `<td style="text-align: center;">${obj.MaterialType}</td>`;
                            datamsg += `<td style="text-align: center;">${obj.ItemCode}</td>`;
                            datamsg += `<td style="text-align: center;">${obj.ItemDesc}</td>`;
                            datamsg += `<td style="text-align: center;">${obj.AvailableQuantity}</td>`;
                            datamsg += `<td style="text-align: center;">${obj.Quantity}</td>`;
                            datamsg += `<td style="text-align: center;">${obj.RequestQuantity}</td>`;
                            datamsg += `</tr>`
                        });
                        $("#tbody_tbDetails").html(datamsg);
                        $("#ItemCodeModal").modal("show");
                    }
                }
                else {
                    $('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
                    $('#art1').css("display", "block");
                    $('#alertlabel').text("Alert !" + response.Message);
                }
            }
        }
    })
}


//function btnConfirmReqNo_Click() {
//    var UID = getParameterByName('UID');
//    var UserRole = getParameterByName('UserRole');
//    var id = getParameterByName('id');
//    var RID = getParameterByName('FromId');
//    var page = getParameterByName('page');

//    var DecriptData = [page];
//    var DecrData = Decrypt(DecriptData);
//    page = DecrData[0];


//    if (page == "2") {

//        window.location.href = '/ScaffoldMaterialListPhase3/ScaffoldMaterialListPhase3?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID;
//    }
//}
function btncancel_Click() {
    $("#divConfirmReq").css("display", "none");
}

var siteData;
function numberWithCommas(x) {
    var parts = x.toString().split(".");
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join(".");
}

function datagrid(id, Reqids, Material, Page, TOID, UID) {
    $("#ajaxLoader").css("display", "block");
    var fromid = $("#hdnfromid").val();
    GetTotalTotalTonnage();
    $.ajax({
        type: "POST",
        url: "/PipelineRequest/griddata",
        data: JSON.stringify({ id: parseInt(id), Reqstid: Reqids, Material: Material, Page: Page, ToID: TOID, UID: UID, fromid: fromid }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var getdata = response;
            var str = "";
            str += '<table id="dataforgrid" class="table table-striped" cellspacing="0" width="100%">';
            str += "<thead><tr>";
            str += "<th style='width:10%;text-align:center;display:none'>Trans ReqId</th>";
            str += "<th style='width:10%;text-align:center;display:none'>Inventory Id</th>";
            str += "<th style='width:15%;text-align:left'>Material Type</th>";
            str += "<th style='width:15%px;text-align:left'>Item Code</th>";
            str += "<th style='width:30%;text-align:left'>Item Description</th>";
            str += "<th style='display:none;'>Available Quantity</th>";
            str += "<th class='no-sort' style='width:10%px;text-align:center'>Quantity</th>";
            str += "<th style='width:3%;text-align:center'>#</th></tr></thead><tbody>";
            if (getdata.length > 0) {
                // $("#ContentPlaceHolder1_hdntablelength").val(getdata.length);
                $("#hdntablelength").val(getdata.length);
                for (var i = 0; i < getdata.length; i++) {
                    var TransReqId = getdata[i].TransReqId == null ? "" : getdata[i].TransReqId;
                    var InventoryId = getdata[i].InventoryId == null ? "" : getdata[i].InventoryId;
                    var MateralType = getdata[i].MaterialType == null ? "" : getdata[i].MaterialType;
                    var ItemCode = getdata[i].ItemCode == null ? "" : getdata[i].ItemCode;
                    var ItemDesc = getdata[i].ItemDesc == null ? "" : getdata[i].ItemDesc;
                    var AvailableQuantity = getdata[i].AvailableQuantity == null ? "" : getdata[i].AvailableQuantity;
                    /*var Quantity = getdata[i].Quantity == null ? 0 : getdata[i].Quantity;*/
                    var Quantity = getdata[i].Quantity == null ? "" : getdata[i].Quantity;
                    if (Quantity > 0) {
                        GblQuantityCount++;
                    }

                    str += "<tr><td style='width:10%;text-align:center;display:none' >" + TransReqId + "</td>";
                    str += "<td style='width:10%;text-align:center;display:none' id='lblinventoryid'>" + InventoryId + "</td>";
                    str += "<td style='width:15%;text-align:left' name='MateralType'>" + MateralType + "</td>";
                    str += "<td style='width:15%;text-align:left' name='ItemCode'>" + ItemCode + "</td>";
                    str += "<td style='width:30%;text-align:left' name='ItemDesc'>" + ItemDesc + "</td>";
                    str += "<td style='display: none;'>" + numberWithCommas(AvailableQuantity) + "<input id='hdnAvalid_" + i + "' type='hidden' value=" + numberWithCommas(AvailableQuantity) + " /></td>";
                    // if ($("#ContentPlaceHolder1_hdntextdisable").val() == "true") {
                    if ($("#hdntextdisable").val() == "true") {
                        str += "<td style='width:10%;text-align:center;background-color:LightCyan;'> <input id='txtqty_" + i + "' type='text' value='" + Quantity + "' class='EnterQuantity' readonly='true' onKeyDown='handleMyInput(event)' max='2147483647' min='0'/><input id='hdnid_" + i + "' type='hidden' value='0' /></td>";

                        str += "<td style='width:3%;'><a id='rowSave_" + i + "' style='pointer-events: none; cursor: default;' class='fa fa-save rowSaveButton'></a></a><input id='OldQtyval_" + i + "' type='hidden' value='" + Quantity + "' /></td></tr>";
                    }
                    else {
                        var tr = "";
                        if (parseInt(AvailableQuantity) >= parseInt(Quantity || 0)) {
                            tr += "<td style='width:10%;text-align:center;background-color:LightCyan;'> <input id='txtqty_" + i + "' type='text' autocomplete='off' value='" + Quantity + "' class='EnterQuantity' onKeyDown='handleMyInput(event)' max='2147483647' min='0' /><input id='hdnid_" + i + "' type='hidden' value='0' /></td>";
                            tr += "<td style='width:3%;'><a id='rowSave_" + i + "' class='fa fa-save rowSaveButton'></a><input id='OldQtyval_" + i + "' type='hidden' value='" + Quantity + "' /></td></tr>";

                        }
                        else {
                            if (Page == 2) {
                                /*tr += "<td style='width:10%;text-align:center;background-color:LightCyan;'> <input id='txtqty_" + i + "' type='text' autocomplete='off' value=" + Quantity + " style='background-color:red;color:white' class='EnterQuantity' min='0'/><input id='hdnid_" + i + "' type='hidden' value='0' /></td>";*/
                                //commented on 26july2022 as client no more wanted availableqty to be greater than our entered quantity
                                tr += "<td style='width:10%;text-align:center;background-color:LightCyan;'> <input id='txtqty_" + i + "' type='text' autocomplete='off' value='" + Quantity + "'  class='EnterQuantity' onKeyDown='handleMyInput(event)' max='2147483647' min='0'/><input id='hdnid_" + i + "' type='hidden' value='0' /></td>";
                                /*tr += "<td style='width:3%;'><a id='rowSave_" + i + "' class='fa fa-save rowSaveButton'></a></a><input id='OldQtyval_" + i + "' type='hidden' value==" + Quantity + " /></td></tr>";*/
                                //commented on 26july2022 as client no more wanted availableqty to be greater than our entered quantity
                                tr += "<td style='width:3%;'><a id='rowSave_" + i + "' class='fa fa-save rowSaveButton'></a></a><input id='OldQtyval_" + i + "' type='hidden' value='" + Quantity + "' /></td></tr>";
                            }
                            else {
                                tr += "<td style='width:10%;text-align:center;background-color:LightCyan;'> <input id='txtqty_" + i + "' type='text' autocomplete='off' value='" + Quantity + "' style='background-color:yellow' class='EnterQuantity' onKeyDown='handleMyInput(event)' max='2147483647' min='0'/><input id='hdnid_" + i + "' type='hidden' value='0' /></a><input id='OldQtyval_" + i + "' type='hidden' value='0' /></td>";
                                tr += "<td style='width:3%;'><a id='rowSave_" + i + "' class='fa fa-save rowSaveButton'></a></a><input id='OldQtyval_" + i + "' type='hidden' value=='" + Quantity + "' /></td></tr>";
                            }

                        }
                        str += tr;

                    }

                }

                str += "</tbody></table>";
                $("#loaddata").html(str);

                siteData = $('#dataforgrid').DataTable({
                    "bSort": false,
                    "processing": true,
                    searching: true,
                    info: false,
                    "pageLength": 50,
                    //paging: false,
                    scrollX: true,
                    scrollY: "60vh",
                    "drawCallback": function () {

                        $("input[type='search']").keyup(function (eve) {
                            if (eve.currentTarget.value == "") {
                                MaterialChange();
                            }
                        });

                        $('.EnterQuantity').keydown(function (eve) {
                            if (eve.keyCode != 8) {
                                var value = eve.key
                                var regex = /^[-_!':.,;"/|<>?+=@#$%^&*({})a-zA-Z[]]+$/;
                                if (value.match(regex)) {
                                    eve.preventDefault();
                                }
                                if (eve.keyCode == 189) {
                                    eve.preventDefault();
                                }
                            }
                        });
                        $(".EnterQuantity").keyup(function (eve) {
                            var txtAlocatid = eve.currentTarget.id;
                            var txtenterval = $("#" + txtAlocatid).val();
                            var colIndx = txtAlocatid.split('_')[1]
                            var tid = '#txtqty_' + colIndx;
                            var OldQty = '#OldQtyval_' + colIndx;
                            var OldQtyval = $(OldQty).val();
                            var oldqty = $(this).parents().find('#OldQtyval_' + colIndx).val();
                            var Enterqty = eve.currentTarget.parentNode.parentNode.childNodes[6].childNodes[1].value;
                            var saveicon = '#rowSave_' + colIndx;
                            if (Enterqty == oldqty) {
                                $(tid).css("border", "1px solid Gray");
                            }
                            else {
                                $(tid).css("border", "1px solid red");
                            }
                        });
                        $(".EnterQuantity").keyup(function (eve) {
                            eve.stopImmediatePropagation();
                            var txtAlocatid = eve.currentTarget.id;
                            var txtenterval = $("#" + txtAlocatid).val();
                            var colIndx = txtAlocatid.split('_')[1]
                            var tid = '#txtqty_' + colIndx;
                            var OldQty = '#OldQtyval_' + colIndx;
                            //var AvQty = '#hdnAvalid_' + colIndx;
                            var AvQty = $(this).parents().find('#hdnAvalid_' + colIndx).val();
                            /*var AvalQty = AvQty.replace(",", "");*/  // Commented on 10-May-2022
                            var AvalQty = AvQty.replace(/,/g, ''); // Added on 10-May-2022
                            var OldQtyval = $(OldQty).val();
                            var EQty = eve.currentTarget.parentNode.parentNode.childNodes[6].childNodes[1].value;
                            /*var Enterqty = EQty.replace(",", "");*/  // Commented on 10-May-2022
                            var Enterqty = EQty.replace(/,/g, ''); // Added on 10-May-2022
                            var saveicon = '#rowSave_' + colIndx;

                            //commented on 26july2022 as client no more wanted available quantity to be greater than our enetered quantity
                            //if ((Page == "2") && parseInt(Enterqty) > parseInt(AvalQty)) {
                            //    $(tid).css({ "background": "Red", "color": "White" });
                            //    $('#art1').addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
                            //    $('#alertlabel').text("Alert! Please Make Sure Entered Quantity is NOT More Than Available Quantity.");
                            //    $('#art1').delay(5000).fadeOut('slow');
                            //    $("#" + saveid).css('color', 'rgb(11, 11, 11)');

                            //} else {
                            //    $(tid).css({ "background": "white", "color": "black" });
                            //    $("#" + saveid).css('color', 'rgb(11, 11, 11)');

                            //}

                        });

                        var hdnsavedata;
                        var saveid;
                        $('.rowSaveButton').click(function (eve) {
                            eve.stopImmediatePropagation();  // Stop Calling Multiple Time When click search and insert the values 05 April 2019
                            saveid = eve.currentTarget.id;
                            if (saveid != "") {
                                hdnsavedata = saveid;
                                if (hdnsavedata == saveid) {
                                    eve.preventDefault();
                                    eve.stopPropagation();
                                    eve.stopImmediatePropagation();
                                    var colIndx = hdnsavedata.split('_')[1]
                                    tid = '#txtqty_' + colIndx;
                                    tempid = '#hdnid_' + colIndx;
                                    var OldQtyval = '#OldQtyval_' + colIndx;
                                    txtid = tid.split('_');
                                    var qtyWithCommas = $(this).parents().find('#txtqty_' + colIndx).val();
                                    /*var qty = qtyWithCommas.replace(",", "");*/  // Commeneted on 10-ay-2022
                                    var qty = qtyWithCommas.replace(/,/g, ''); // Added on 10-May-2022
                                    var tempval = $(tempid).val();
                                    //var oldqty = 0;
                                    var oldqty = $(this).parents().find('#OldQtyval_' + colIndx).val();
                                    if (oldqty == "") {
                                        oldqty = 0;
                                    }
                                    var TReqId = eve.currentTarget.parentNode.parentNode.childNodes[0].innerText;
                                    var inv = eve.currentTarget.parentNode.parentNode.childNodes[1].innerText;
                                    var AvaQtyWithCommas = eve.currentTarget.parentNode.parentNode.childNodes[5].innerText;
                                    /*var AvQty = AvaQtyWithCommas.replace(",", "");*/ // Commented on 10-May-2022
                                    var AvQty = AvaQtyWithCommas.replace(/,/g, '');  // Added on 10-May-2022
                                    //if ($("#ContentPlaceHolder1_hdnconfirm").val() == "Confirmed") {
                                    if ($("#hdnconfirm").val() == "Confirmed") {
                                        $('*').off('keyup keydown keypress');
                                    }
                                    else {
                                        if ((TReqId == "" && qty == "" && ((oldqty == "0" || oldqty == "")) || ((TReqId == "" && ((oldqty == "0" || oldqty == "") && qty == "0"))))) {
                                            $("#" + tid).removeClass("invalidCol");
                                            $("#" + tid).css("border", "1px solid #9e9898");
                                        }
                                        else if ((TReqId != "") || (TReqId == "" && qty != "" && qty != "0") || (TReqId == "" && (qty == "" || qty == "0"))) {
                                            if (qty != "" || qty == "") {
                                                if (qty == "") {
                                                    qty = 0;
                                                }

                                                $("#ajaxLoader").css("display", "block");

                                                var id = $("#hdnid").val();
                                                var from = $("#hdnfrom").val();
                                                var fromid = $("#hdnfromid").val();
                                                var TO = $("#hdnTo").val();
                                                var ToId = $("#hdnToId").val();
                                                var UID = $("#hdnUID").val();
                                                var ReqId = $("#hdnReqId").val();
                                                var Page = $("#hdnPage").val();
                                                var txtdesc = $("#txtdesc").val();
                                                var txtComment = $("#txtComment").val();
                                                var ddlSupplier = $("#ddlSupplier option:selected").text();

                                                if (Page == "2" && parseInt(AvQty) < parseInt(qty)) {
                                                    $(tid).addClass("invalidCol");
                                                    $("#ajaxLoader").css("display", "none");
                                                }
                                                else {
                                                    $(tid).removeClass("invalidCol");

                                                    var MateralType = $(this).closest("tr").find("td[name=MateralType]").text();
                                                    var ItemCode = $(this).closest("tr").find("td[name=ItemCode]").text();
                                                    var ItemDesc = $(this).closest("tr").find("td[name=ItemDesc]").text();
                                                    var from = $("#hdnfrom").val();
                                                    from

                                                    $.ajax({
                                                        type: "POST",
                                                        // url: "../WebApi/AllInventory.asmx/insertion",
                                                        url: "/PipelineRequest/insertion",
                                                        data: JSON.stringify({ Qty: qty, OldQty: oldqty, InvId: inv, UID: UID, from: from, fromid: fromid, To: TO, ToId: ToId, id: id, TReqId: TReqId, ReqID: ReqId, Page: Page, tempval: tempval, txtdesc: txtdesc, Supplier: ddlSupplier, txtComment: txtComment, MateralType, ItemCode, ItemDesc }),
                                                        contentType: "application/json; charset=utf-8",
                                                        dataType: "json",
                                                        success: function (response) {
                                                            //var getdata = response.d;
                                                            var getdata = response;
                                                            if (getdata > 0) {
                                                                //if ($("#ContentPlaceHolder1_hdnPage").val() == "1") {
                                                                if ($("#hdnPage").val() == "1" || $("#hdnPage").val() == "3") {
                                                                    if (ReqId == "0") {
                                                                        // $("#ContentPlaceHolder1_hdnRid").val(getdata);
                                                                        $("#hdnRid").val(getdata);
                                                                    }
                                                                    else {
                                                                        $(tempid).val(getdata);
                                                                        $("#hdnRid").val("");
                                                                    }

                                                                    if ($("#hdnRid").val() != "") {
                                                                        var coun = parseInt(txtid[1]) + 1;
                                                                        var typeid = "txtqty_" + coun;
                                                                        $("#hdnprevid").val(typeid);
                                                                        $("#btnredirection").click();
                                                                    }
                                                                    else {
                                                                        $("#ajaxLoader").css("display", "none");
                                                                    }

                                                                }
                                                                else {
                                                                    $(tempid).val(getdata);

                                                                    $("#hdnflagforjobinsertion").val("true");
                                                                    $("#ajaxLoader").css("display", "none");
                                                                }

                                                            }
                                                            else {
                                                                $("#ajaxLoader").css("display", "none");
                                                            }
                                                            $(OldQtyval).val(qty);
                                                            $(tid).css("border", "1px solid #9e9898");
                                                            $("#" + saveid).css('color', 'rgb(11, 11, 11)');
                                                        }
                                                    });
                                                }
                                            }
                                            else {
                                                $(tid).removeClass("invalidCol");
                                            }
                                        }
                                        else {
                                            $("#" + tid).removeClass("invalidCol");
                                            $("#" + tid).css("border", "1px solid #9e9898");
                                        }

                                    }

                                    var coun = $(tid).parent().parent().next('tr').find('.EnterQuantity').attr('id');
                                    $(coun).select();
                                    $("#" + saveid).css('color', 'rgb(11, 11, 11)');
                                }
                            }
                        });
                        $('.EnterQuantity').keydown(function (eve) {
                            eve.stopImmediatePropagation();  // Stop Calling Multiple Time When click search and insert the values 05 April 2019

                            var txtid = "";
                            var tempid = "";
                            if ($("#txtdesc").val() == "") {
                                if (eve.keyCode != 8) {
                                    eve.preventDefault();
                                    $('#art1').addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
                                    $('#alertlabel').text("Alert ! Please fill the Description.");
                                }
                            }

                            else {
                                if ($("#hdntextdisable").val() != "true" && (eve.keyCode == 9 || eve.keyCode == 13)) {
                                    eve.preventDefault();
                                    eve.stopPropagation();
                                    eve.stopImmediatePropagation();
                                    tid = eve.currentTarget.id;
                                    tempid = eve.currentTarget.nextSibling.id;
                                    var ColIndx = tempid.split('_')[1];
                                    var saveid = "#rowSave_" + ColIndx;
                                    var OldQtyval = '#OldQtyval_' + ColIndx;
                                    txtid = tid.split('_');
                                    var qty = eve.currentTarget.value;
                                    var tempval = eve.currentTarget.nextSibling.value;
                                    //var oldqty = 0;
                                    var oldqty = $(this).parents().find('#OldQtyval_' + ColIndx).val();
                                    if (oldqty == "") {
                                        oldqty = 0;
                                    }
                                    var TReqId = eve.currentTarget.parentNode.parentNode.childNodes[0].innerText;
                                    var inv = eve.currentTarget.parentNode.parentNode.childNodes[1].innerText;
                                    var AvaQtyWithCommas = eve.currentTarget.parentNode.parentNode.childNodes[5].innerText;
                                    /*var AvQty = AvaQtyWithCommas.replace(",", "");*/ //Commented on 10-May-2022
                                    var AvQty = AvaQtyWithCommas.replace(/,/g, ''); // Added on 10-May-2022
                                    //if ($("#ContentPlaceHolder1_hdnconfirm").val() == "Confirmed") {
                                    if ($("#hdnconfirm").val() == "Confirmed") {
                                        $('*').off('keyup keydown keypress');
                                    }
                                    else {
                                        //if ((TReqId == "" && qty == "") || ((TReqId == "" && qty == "0"))) {
                                        if ((TReqId == "" && qty == "" && ((oldqty == "0" || oldqty == "")) || ((TReqId == "" && ((oldqty == "0" || oldqty == "") && qty == "0"))))) {
                                            $("#" + tid).removeClass("invalidCol");
                                            $("#" + tid).css("border", "1px solid #9e9898");
                                        }

                                        //else if ((TReqId != "") || (TReqId == "" && qty != "" && qty != "0")) {
                                        else if ((TReqId != "") || (TReqId == "" && qty != "" && qty != "0") || (TReqId == "" && (qty == "" || qty == "0"))) {
                                            if (qty != "" || qty == "") {
                                                if (qty == "") {
                                                    qty = 0;
                                                }


                                                $("#ajaxLoader").css("display", "block");

                                                var id = $("#hdnid").val();
                                                var from = $("#hdnfrom").val();
                                                var fromid = $("#hdnfromid").val();
                                                var TO = $("#hdnTo").val();
                                                var ToId = $("#hdnToId").val();
                                                var UID = $("#hdnUID").val();
                                                var ReqId = $("#hdnReqId").val();
                                                var Page = $("#hdnPage").val();
                                                var txtdesc = $("#txtdesc").val();
                                                var txtComment = $("#txtComment").val();
                                                var ddlSupplier = $("#ddlSupplier option:selected").text();


                                                //commented on 26july2022 as client no more wanted availableqty to be greater than our entered quantity
                                                //if (Page == "2" && parseInt(AvQty) < parseInt(qty)) 
                                                //{
                                                //    $("#" + tid).addClass("invalidCol");
                                                //    $("#ajaxLoader").css("display", "none");
                                                //}
                                                /*else {*/
                                                $("#" + tid).removeClass("invalidCol");
                                                var MateralType = $(this).closest("tr").find("td[name=MateralType]").text();
                                                var ItemCode = $(this).closest("tr").find("td[name=ItemCode]").text();
                                                var ItemDesc = $(this).closest("tr").find("td[name=ItemDesc]").text();



                                                $.ajax({
                                                    type: "POST",
                                                    // url: "../WebApi/AllInventory.asmx/insertion",
                                                    url: "/PipelineRequest/insertion",
                                                    data: JSON.stringify({ Qty: qty, OldQty: oldqty, InvId: inv, UID: UID, from: from, fromid: fromid, To: TO, ToId: ToId, id: id, TReqId: TReqId, ReqID: ReqId, Page: Page, tempval: tempval, txtdesc: txtdesc, Supplier: ddlSupplier, txtComment: txtComment, MateralType, ItemCode, ItemDesc }),
                                                    contentType: "application/json; charset=utf-8",
                                                    dataType: "json",
                                                    success: function (response) {
                                                        $("#" + tid).css("border", "1px solid #9e9898");
                                                        //  var getdata = response.d;
                                                        var getdata = response;
                                                        if (getdata > 0) {
                                                            //if ($("#ContentPlaceHolder1_hdnPage").val() == "1") {
                                                            if ($("#hdnPage").val() == "1" || $("#hdnPage").val() == "3") {
                                                                if (ReqId == "0") {
                                                                    $("#hdnRid").val(getdata);
                                                                }
                                                                else {
                                                                    $("#" + tempid).val(getdata);
                                                                    $("#hdnRid").val("");
                                                                }



                                                                if ($("#hdnRid").val() != "") {
                                                                    var coun = parseInt(txtid[1]) + 1;
                                                                    var typeid = "txtqty_" + coun;
                                                                    $("#hdnprevid").val(typeid);
                                                                    $("#btnredirection").click();
                                                                }
                                                                else {
                                                                    $("#ajaxLoader").css("display", "none");
                                                                }

                                                            }
                                                            else {
                                                                $("#" + tempid).val(getdata);

                                                                $("#hdnflagforjobinsertion").val("true");
                                                                $("#ajaxLoader").css("display", "none");
                                                            }

                                                        }
                                                        else {
                                                            $("#ajaxLoader").css("display", "none");
                                                        }
                                                        $(OldQtyval).val(qty);
                                                        $("#" + tid).css("border", "1px solid #9e9898");
                                                        $(saveid).css("color", "black");
                                                    }
                                                });
                                                /*}*/

                                            }
                                            else {
                                                $("#" + tid).removeClass("invalidCol");
                                                $("#" + tid).css("border", "1px solid #9e9898");
                                            }
                                        }

                                        else {
                                            $("#" + tid).removeClass("invalidCol");
                                            $("#" + tid).css("border", "1px solid #9e9898");
                                        }

                                    }

                                    var coun = $("#" + tid).parent().parent().next('tr').find('.EnterQuantity').attr('id');
                                    $("#" + coun).select();

                                }
                                else {

                                    if (eve.keyCode == 8) {
                                        oldqty = eve.currentTarget.defaultValue;
                                        $("#hdnOldQty").val(eve.currentTarget.defaultValue);
                                    }
                                }
                            }
                            if ($("#hdntextdisable").val() == "true" && (eve.keyCode == 13)) {
                                eve.preventDefault();
                            }//16-apr-2019-baans-end
                        });

                    }
                });

                pagingclick();

                // scrolling Top when Click On Next Button

                siteData.on('page.dt', function () {
                    $('.dataTables_scrollBody').animate({
                        scrollTop: 0
                    }, 300);
                });
            }
            else {
                str += "</tbody></table>";
                $("#loaddata").html(str);
                $('#dataforgrid').DataTable({
                    "bSort": false,
                    "processing": true,
                    searching: true,
                    info: false,
                    "pageLength": 50,
                    scrollX: true,
                    scrollY: "60vh",
                    "bPaginate": false,
                    //paging: false,
                    //scrollX: true,
                    //scrollY: "60vh",
                    //scrollCollapse: true,
                });
            }
            $("#ajaxLoader").css("display", "none");
        },
        complete: function () {
            if ($("#hdnprevid").val() != "") {
                var focusid = $("#hdnprevid").val();
                $("#hdnprevid").val("");
                $("#" + focusid).select();
            }
            else {
                // $("#txtqty_0").select();
            }

        }
    })

}


function MaterialChange() {
    $("#ajaxLoader").css("display", "block");
    var id = $("#hdnid").val();
    var RIDs = $("#hdnReqId").val();
    var MatType = $('#ddlMaterialType option:selected').val();
    $("#hdnmaterialTypeval").val(MatType);
    var page = $("#hdnPage").val();
    var Toid = $("#hdnToId").val();
    var UID = $("#hdnUID").val();

    datagrid(id, RIDs, MatType, page, Toid, UID);
}


function OnRadioClick(rb) {
    var grid = $find("<%=SiteJobRadGrid.ClientID %>");
    var element = grid.get_element();
    var rbs = element.getElementsByTagName("input");
    for (var i = 0; i < rbs.length; i++) {
        if (rbs[i].type == "radio") {
            if (rbs[i].checked && rbs[i] != rb) {
                rbs[i].checked = false;
                break;
            }
        }
    }
}
var count = 0;
var NumRows = 0;

$(document).ready(function () {
    PipelineDateCheck();
    GetPipelineDesc1();
    MaterialTupeDropDownBind();
    rowclick();
    hideAlert();
    var id = $("#hdnid").val();
    var RIDs = $("#hdnReqId").val();
    var page = $("#hdnPage").val();
    var Toid = $("#hdnToId").val();
    var UID = $("#hdnUID").val();
    var MatType = $('#ddlMaterialType option:selected').val();
    if (MatType != "" && MatType != undefined) {
        datagrid(id, RIDs, MatType, page, Toid, UID);
    }
    else {
        datagrid(id, RIDs, "All", page, Toid, UID);
    }


});

function pagingclick() {
    $('.dataTables_wrapper').on('mousedown', '.paginate_button', function () {
        $("#hdngetpage").val("1");
        $("#hdngetMsg").val("1");
        getdatachangestart();
        if (data == false) {
            bootbox.alert({
                message: "Please Adjust qty, (If required), and then click on Save Icon for records that are highlighted in red.",

            })
        }
    })
}
function hideAlert() {
    setTimeout(function () {
        $("#art1").fadeOut(2000);
    }, 3000);
}
function rowclick() {
    if ($("#getgridcount").val() != "") {
        NumRows = $("#getgridcount").val();
    }

}

//function btnSelect_Click()
//{
//    $("#myModal").css("display", "block");
//    GetHistoricalJobMaterialList();
//}

function btnSelect_Click() {
    var Check = '';
    var datatablelen = $("#hdntablelength").val();
    for (var i = 0; i < datatablelen; i++) {
        var newQty = "#txtqty_" + i;
        var EtQnty = $(newQty).val();
        if (EtQnty == 0 || EtQnty == '' || EtQnty == null) {
            Check = false;
        }
        else {
            Check = true;
            break;
        }
    }
    if (Check == false) {
        $("#myModal").css("display", "block");
        GetHistoricalJobMaterialList();
    }
    else {
        var Response = { Result: "Failed", Message: "To Copy Historical Job Material List - FIRST - 'Click' on 'Clear All' button and Clear Out the already added Pipeline Items." };
        CustomAlert(Response);
        $("#myModal").css("display", "none");
    }
}

function btnclose_Click() {
    $("#myModal").css("display", "none");
}
function GetHistoricalJobMaterialList() {

    var Siteid = $('#ddlSite option:selected').val();
    if (Siteid == "" && Siteid == undefined) {
        Siteid = "All";
    }
    var ClientID = getParameterByName('id');
    var FromId = getParameterByName('FromId');
    var ToId = getParameterByName('ToId');
    var ddl1from = getParameterByName('ddl1from');
    var ddl1to = getParameterByName('ddl1to');
    var ddlfrom = Siteid;
    var rdodepottosite = "True";
    var rdositetojob = "False";
    var JobSiteid = getParameterByName('Siteid');
    var RID = getParameterByName('RID');
    $.ajax({
        type: "POST",
        url: "/PipelineRequest/GetHistoricalJobMaterialList",
        // data: JSON.stringify({ id: ClientID, FromId: FromId, ToId: ToId, ddl1from: ddl1from, ddl1to: ddl1to, JobSiteid: JobSiteid, ddlfrom: ddlfrom, rdodepottosite: rdodepottosite, rdositetojob: rdositetojob, RID: RID }),
        data: JSON.stringify({ ToId: ToId, FromId: FromId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            var str = "";
            var GetData = response;
            $('#tblSiteJobGrid').DataTable().clear();
            $('#tblSiteJobGrid').DataTable().destroy();

            if (GetData.length > 0) {
                $('#tblSiteJobGrid').css("display", "block");

                for (var i = 0; i < GetData.length; i++) {
                    var JobId = GetData[i].JobId == null ? "" : GetData[i].JobId;
                    var Location = GetData[i].Location == null ? "" : GetData[i].Location;
                    var BuildingNo = GetData[i].BuildingNo == null ? "" : GetData[i].BuildingNo;
                    var AssetNo = GetData[i].AssetNo == null ? "" : GetData[i].AssetNo;
                    var Assetdesc = GetData[i].Assetdesc == null ? "" : GetData[i].Assetdesc;
                    var Scaffref = GetData[i].Scaffref == null ? "" : GetData[i].Scaffref;
                    var OptimizeStatus = GetData[i].OptimizeStatus == null ? "" : GetData[i].OptimizeStatus;

                    str += "<tr>"
                    str += "<td style='display:none'></td>"
                    str += "<td id='JobId_" + i + "' style='text-align:center;'><input type='Radio' name='JonSelected' class='ChkJobIdID' JobId_='" + JobId + "'/></td>"
                    //str += "<td>" + JobId + "</td>"
                    str += "<td>" + Location + "</td>"
                    str += "<td>" + BuildingNo + "</td>"
                    str += "<td>" + AssetNo + "</td>"
                    str += "<td>" + Assetdesc + "</td>"
                    str += "<td id='Scaffref_" + JobId + "'>" + Scaffref + "</td>"
                    str += "<td id='OptimizeStatus_" + JobId + "'>" + OptimizeStatus + "</td>"
                    str += "</tr>"
                }

                $('#tbodySiteJobGrid').append(str);


                $('#tblSiteJobGrid').DataTable({
                    "bSort": false,
                    "processing": true,
                    searching: true,
                    "bLengthChange": false,
                    "bInfo": false,
                    autoWidth: false,
                    scrollX: true,
                    scrollCollapse: true,
                    scrollY: "60vh",
                    "paging": false,
                    "ordering": false,
                    "info": false

                });


                $(".ChkJobIdID").click(function () {
                    event.stopImmediatePropagation();
                    var RegisterID = $(this).attr('JobId_').split('_')[0];
                    var OptimizeStatus = $('#OptimizeStatus_' + RegisterID).text();
                    var Scaffref = $('#Scaffref_' + RegisterID).text();
                    //   var OptimizeStatus1 = $('#JobId_' + RegisterID).Val();
                    $('#hdnlblOptimizeStatus').val(OptimizeStatus);
                    $('#hdfieldId').val(RegisterID);
                    $('#hdnScaffref').val(Scaffref);


                });


            } else {
                // No Data Code here
                $('#tblSiteJobGrid').css("display", "none");
            }
        }
    })

}

function btnSelectJob_Click() {
    if ($('#hdnlblOptimizeStatus').val() == "Not Finalised") {
    }
    else {
        $('#ddljob').val($('#hdnScaffref').val());
    }
    jobchange();
}

function jobchange() {

    var jobid = $("#hdfieldId").val();
    var jobtext = $("#ddljob").val();
    var id = $("#hdnid").val();
    var Toid = $("#hdnToId").val();
    var UID = $("#hdnUID").val();
    var fromid = $("#hdnfromid").val();
    $("#ajaxLoader").css("display", "block");
    $.ajax({
        type: "POST",
        // url: "../WebApi/AllInventory.asmx/SetJobData",
        url: "/PipelineRequest/SetJobData",
        data: JSON.stringify({ UID: UID, id: id, Toid: Toid, jobid: jobid, fromid: fromid }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var Result = response;
            // if (parseInt(response.d) > 0) {
            if (parseInt(response) > 0) {
                $("#hdnflagforjobinsertion").val("true");
                var MatType = "All";
                var page = $("#hdnPage").val();
                var id = $("#hdnid").val();
                var Toid = $("#hdnToId").val();
                var UID = $("#hdnUID").val();
                datagrid(id, "0", MatType, page, Toid, UID);
            }
        }
    })
    $("#myModal").css("display", "none");
}


function addnewSupplier() {
    $('#txtSupplier').val("");
    $("#divCreateSupplier").css("display", "block");
}
function btnSupplierClose() {
    $('#txtSupplier').val("");
    $('#divCreateSupplier').css('display', 'none');
}
function btnSupplierSave_Click() {
    var txtSupplier = $('#txtSupplier').val();

    if (txtSupplier != "") {
        $('#ddlSupplier').append(`<option value="${txtSupplier}" selected='selected'>
                                       ${txtSupplier}
                                  </option>`);
        $('#divCreateSupplier').css('display', 'none');
        var Response = { Result: "Success", Message: "Record Save Successfully." };
        CustomAlert(Response);

    } else {
        var Response = { Result: "Failed", Message: "Please fill Supplier." };
        CustomAlert(Response);
    }
}



function btnrefresh_Click() {
    var ReqId = $("#hdnfromid").val();
    var Desccription = $("#txtdesc").val();
    //location.reload();
    if (ReqId == "0") {
        var Response = { Result: "Failed", Message: "At least one quantity should be filled to save description." };
        CustomAlert(Response);
    }
    else if (Desccription == "") {
        var Response = { Result: "Failed", Message: "Please fill Description." };
        CustomAlert(Response);
    }
    else {
        $.ajax({
            type: "POST",
            // url: "../WebApi/AllInventory.asmx/SetJobData",
            url: "/PipelineRequest/updateDesc",
            data: JSON.stringify({ ReqId: ReqId, Desccription: Desccription }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (response) {
                var Result = response;
            }
        })
        location.reload();
    }
}

function btnClearAll_Click() {
    $("#divClearAllData").css("display", "block");
}

function btnClearcancel_Click() {
    $("#divClearAllData").css("display", "none");

}

function btnClearAllPipeLineData_Click() {
    var ReqId = $("#hdnfromid").val();
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

function PipelineDateCheck() {

    var ReqId = $("#hdnfromid").val();
    var ClientID = getParameterByName('id');
    var Toid = $("#hdnToId").val();
    $.ajax({
        type: "POST",
        url: "/PipelineRequest/PipelineDateCheck",
        data: JSON.stringify({ ReqId: ReqId, ClientID: ClientID, Toid: Toid }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {

            if (response.result == "Success") {
                $('#art1').addClass("w3-section w3-card-12  j-alertBox-Warning").css('display', 'block');
                /* $('#art1').addClass("w3-section w3-card-12 w3-yellow j-alertBox-Warning").css('display', 'block');*/
                $('#alertlabel').text("Info! Please Note The Pipeline Start Date Has Not Been Filled.");
                $('#art1').delay(6000).fadeOut('slow');

            }
            else {

            }

        }
    })

}




function GetPipelineDesc1() {

    var FromId = getParameterByName('FromId');
    $.ajax({
        type: "POST",
        url: "/PipelineRequest/GetPipelineDesc1",
        data: JSON.stringify({ FromId: FromId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            var GetPipelineStartDate = response;
            $('#txtdesc').val(GetPipelineStartDate.PipelineDesc);

        }
    })
}
function GetTotalTotalTonnage() {
    var ClientID = getParameterByName('id');
    var ReqId = $("#hdnReqId").val();
    var JobId = $("#hdnfromid").val();
    $.ajax({
        type: "POST",
        url: "/PipelineRequest/GetTotalTotalTonnage",
        data: JSON.stringify({ ClientID: ClientID, ReqId: ReqId, JobId: JobId }),
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
