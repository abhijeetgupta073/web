
$(document).ready(function () {

    var id = getParameterByName('id') == null ? "" : getParameterByName('id');
    var UID = getParameterByName('UID') == null ? "" : getParameterByName('UID');
    var from = getParameterByName('From') == null ? "" : getParameterByName('From');
    var FromId = getParameterByName('FromId') == null ? "" : getParameterByName('FromId');
    var ToId = getParameterByName('ToId') == null ? "" : getParameterByName('ToId');
    var To = getParameterByName('To') == null ? "" : getParameterByName('To');
    var DamagedInvId = getParameterByName('DamagedInvId') == null ? "" : getParameterByName('DamagedInvId');
    var RInvId = getParameterByName('RInvId') == null ? "" : getParameterByName('RInvId');
    var page = getParameterByName('page') == null ? "" : getParameterByName('page');
    var fromtocheck = getParameterByName('fromtocheck') == null ? "" : getParameterByName('fromtocheck');


    var DecriptData = [id, UID, FromId, from, ToId, To, RInvId, page, DamagedInvId, fromtocheck];
    var DecrData = Decrypt(DecriptData);
    id = DecrData[0];
    UID = DecrData[1];
    FromId = DecrData[2];
    from = DecrData[3];
    ToId = DecrData[4];
    To = DecrData[5];
    RInvId = DecrData[6];
    page = DecrData[7];
    DamagedInvId = DecrData[8];
    fromtocheck = DecrData[9];


    $("#hdnid").val(id);
    $("#hdnUID").val(UID);
    $("#hdnfrom").val(from);
    $("#hdnfromid").val(FromId);
    $("#hdnTo").val(To);
    $("#hdnToId").val(ToId);
    if (RInvId == "") {
        $("#hdnRInvId").val(0);
    } else {
        $("#hdnRInvId").val(RInvId);
    }
    if (DamagedInvId == "") {
        $('#hdnDamagedInvId').val(0);
    } else {
        $('#hdnDamagedInvId').val(DamagedInvId);
    }

    $("#hdnPage").val(page);

    $('#hdnfromtocheck').val(fromtocheck);

    //       if ($("#hdnRInvId").val() > 0) {
    //            $('#btnconfrminvtry').css('display', 'block');
    //        }
    //    else {
    //            $('#btnconfrminvtry').css('display', 'none');
    //    }
    //        var ReturnStatus = ('@ViewBag.ReturnStatus');
    //if (ReturnStatus != "" && ReturnStatus != null) {
    //    $('#lockimage').css('display', 'block');
    //    $('#hdntextdisable').val(true);
    //    $('#btnconfrminvtry').css('display', 'none');
    //    $('#ckbox').attr('disabled', 'disabled');
    //    $('#txtdesc').attr('disabled', 'disabled');
    //}

    //if (from == "2") {
    //    $('#chkdisplay').css('display', 'block');

    //}
    MaterialTupeDropDownBind();

    GetTotalTonnage();
});

$(document).ready(function () {

    $('#txtdesc').keydown(function (eve) {
        if (eve.keyCode == 13) {
            return false;
        }
    })
});

function MaterialTupeDropDownBind() {
    var id = getParameterByName('id');
    $.ajax({
        type: "POST",
        url: "/DamagedInventoryDetails/MaterialDropDown",
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
function hideAlert() {
    setTimeout(function () {
        $(".fadeOut").fadeOut(1000);
    }, 2000);
}
var damageTableData;
function numberWithCommas(x) {
    var parts = x.toString().split(".");
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join(".");
}
function damageDataGrid(id, toid, fromid, rInvId, dInvId, fromtocheck, MatType) {
    $("#ajaxLoader").css("display", "block");
    $.ajax({
        type: "POST",
        // url: "../WebApi/AllInventory.asmx/getDamageData",
        url: "/DamagedInventoryDetails/getDamageData",
        data: JSON.stringify({ id: id, toid: toid, fromid: fromid, rInvId: rInvId, dInvId: dInvId, fromtocheck: fromtocheck, Material: MatType }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            // var getdata = response.d;
            var getdata = response;
            var str = "";
            str += '<table id="dataforgrid" class="table table-striped" cellspacing="0" width="100%">';
            str += "<thead><tr>";
            str += "<th style='width:5%;text-align:center;'></th>";
            str += "<th style='text-align:center;display:none '>DIDetailID</th>";
            str += "<th style='text-align:center;display:none'>InventoryId</th>";
            str += "<th style='width:15%;text-align:left;'>Material Type</th>";
            str += "<th style='width:10%;text-align:left;'>Item Code</th>";
            str += "<th style='width:25%;text-align:left;'>Item Desc</th>";
            str += "<th style='width:10%;text-align:left;'>Available Quantity</th>";
            str += "<th style='width:5%;text-align:left;'>Damaged Quantity</th>";
            str += "<th style='text-align:left;display:none'>Old Damaged Quantity</th>";
            str += "<th style='width:30%;text-align:left;'>Reason for Damage</th>";
            str += "<th style='width:10%;text-align:center;'>#</th></tr></thead><tbody>";
            if (getdata.length > 0) {
                for (var i = 0; i < getdata.length; i++) {
                    var DIDetailID = getdata[i].DIDetailID == null ? "" : getdata[i].DIDetailID;
                    var InventoryId = getdata[i].InventoryId == null ? "" : getdata[i].InventoryId;
                    var MateralType = getdata[i].MaterialType == null ? "" : getdata[i].MaterialType;
                    var ItemCode = getdata[i].ItemCode == null ? "" : getdata[i].ItemCode;
                    var ItemDesc = getdata[i].ItemDesc == null ? "" : getdata[i].ItemDesc;
                    // var AvailableQuantity = getdata[i].AvailableQuantityDamaged == null ? "" : getdata[i].AvailableQuantityDamaged;
                    var AvailableQuantity = getdata[i].AvailableQuantity == null ? "" : getdata[i].AvailableQuantity;
                    /*  var DamagedQuantity = getdata[i].DamagedQuantity == null ? '' : getdata[i].DamagedQuantity;*/
                    var DamagedQuantity = getdata[i].DamagedQuantity == null ? "" : getdata[i].DamagedQuantity;
                    var DisplayDamagedQty = DamagedQuantity == "" ? "" : numberWithCommas(DamagedQuantity);
                    var Reason = getdata[i].Reason == null ? "" : getdata[i].Reason;
                    str += "<td id='edit' style='width:5%' class='glyphicon glyphicon-pencil' ></td>";
                    str += "<td style='text-align:center;display:none'>" + DIDetailID + "<input id='TempDIDetailsID_" + i + "' type='hidden' value='0' /></td>";
                    str += "<td style='text-align:center;display:none'>" + InventoryId + "</td>";
                    str += "<td style='width:15%;text-align:left;'>" + MateralType + "</td>";
                    str += "<td style='width:10%;text-align:left;'>" + ItemCode + "</td>";
                    str += "<td style='width:25%;text-align:left;'>" + ItemDesc + "</td>";
                    str += "<td style='width:10%;text-align:left;'>" + numberWithCommas(AvailableQuantity) + "</td>";
                    str += "<td style='width:5%;text-align:left;background-color:LightCyan;'> <input style='width:90%;' id='txtDmgQty_" + i + "' type='text' value='" + DisplayDamagedQty + "' class='DamageQty' /></td>";
                    str += "<td style='text-align:left;display:none;'>" + DamagedQuantity + "</td>";
                    str += "<td style='width:30%;text-align:left;background-color:LightCyan;'> <input style='width:100%;' id='txtReason_" + i + "' type='text' value='" + Reason + "' class='Reason'></td>";
                    str += "<td style='width:3%;'><a id='rowSave_" + i + "' class='fa fa-save rowSaveButton'></a></td></tr>";
                    $("#hdnAvaQty").val(AvailableQuantity);
                    $("#hdnDamagedQty").val(AvailableQuantity);
                }
                str += "</tbody></table>";
                $("#damageLoadData").html(str);
                damageTableData = $('#dataforgrid').DataTable({
                    "bSort": false,
                    "processing": true,
                    searching: true,
                    info: false,
                    "pageLength": 50,
                    //paging: false,
                    scrollX: true,
                    scrollY: "60vh",
                    //scrollCollapse: true,
                    "drawCallback": function () {

                        $('.DamageQty').keydown(function (eve) {
                            if (eve.keyCode != 8) {
                                var value = eve.key
                                var regex = /^[-_!@@#$%^&*()a-zA-Z]+$/;
                                if (value.match(regex)) {
                                    eve.preventDefault();
                                }
                                if (eve.keyCode == 189) {
                                    eve.preventDefault();
                                }
                            }

                        });
                        $('.DamageQty').keyup(function (eve) {
                            var txtqtyid = eve.currentTarget.id;
                            var colIndx = txtqtyid.split('_')[1]
                            var tid = '#txtDmgQty_' + colIndx;
                            var dqtywithcommas = eve.currentTarget.parentNode.parentNode.childNodes[7].childNodes[1].value;
                            var dqty = dqtywithcommas.replace(/,/g, '');//Added on 17-May-2022
                            if (dqty == "" || dqty == "0") {
                                // $(tid).css("border", "1px solid red");
                            } else {
                                $(tid).css("border", "1px solid red");
                            }

                        });
                        $('.Reason').keyup(function (eve) {
                            var reason = eve.currentTarget.parentNode.parentNode.childNodes[9].childNodes[1].value;
                            var txtReasonid = eve.currentTarget.id;
                            var colIndx = txtReasonid.split('_')[1]
                            var tid = '#txtReason_' + colIndx;
                            if (reason != "") {
                                $(tid).css("border", "1px solid red");
                            }
                        });
                        var hdnsavedata;
                        var saveid;

                        $('.rowSaveButton').click(function (eve) {
                            eve.stopImmediatePropagation();  // Stop Calling Multiple Time When click search and insert the values 05 April 2019
                            var dmgQty = "";
                            var olddmgQty = "";
                            var avail = "";
                            var olddmgQtyWithCommas = "";
                            saveid = eve.currentTarget.id;
                            if (saveid != "") {
                                hdnsavedata = saveid;
                                if (hdnsavedata == saveid) {
                                    eve.preventDefault();
                                    var colIndx = saveid.split('_')[1]
                                    var dmgid = '#txtDmgQty_' + colIndx;
                                    var txtid = dmgid.split('_');
                                    var dmgQtyWithCommas = $(this).parents().find('#txtDmgQty_' + colIndx).val();
                                    /*dmgQty = dmgQtyWithCommas.replace(",", "");*/ //Commented on 17-May-2022
                                    dmgQty = dmgQtyWithCommas.replace(/,/g,'');//Added on 17-May-2022
                                    var reasonid = '#txtReason_' + colIndx;
                                    var reason = $(this).parents().find('#txtReason_' + colIndx).val();

                                    var id = $("#hdnid").val();
                                    var fromId = $("#hdnfromid").val();
                                    var ToId = $("#hdnToId").val();
                                    var UID = $("#hdnUID").val();
                                    var dmgInvId = $("#hdnDamagedInvId").val();
                                    var Page = $("#hdnPage").val();
                                    var txtdesc = $("#txtdesc").val();

                                    //var id = $("#ContentPlaceHolder1_hdnid").val();
                                    //var fromId = $("#ContentPlaceHolder1_hdnfromid").val();
                                    //var ToId = $("#ContentPlaceHolder1_hdnToId").val();
                                    //var UID = $("#ContentPlaceHolder1_hdnUID").val();
                                    //var dmgInvId = $("#ContentPlaceHolder1_hdnDamagedInvId").val();
                                    //var Page = $("#ContentPlaceHolder1_hdnPage").val();
                                    //var txtdesc = $("#ContentPlaceHolder1_txtdesc").val();

                                    var invID = eve.currentTarget.parentNode.parentNode.childNodes[2].innerText;
                                    olddmgQtyWithCommas = eve.currentTarget.parentNode.parentNode.childNodes[7].childNodes[1].defaultValue;
                                    /*olddmgQty = olddmgQtyWithCommas.replace(",", "");*/ //Commented on 17-May-2022
                                    /*  olddmgQty = olddmgQtyWithCommas.replace(/,/g,''); //Added on 17-May-2022*/
                                    olddmgQty = olddmgQtyWithCommas.replace(/,/g, '');

                                    if (olddmgQty == "") {
                                        olddmgQty = 0;
                                    }
                                    var AvaiQuantitywithCommas = eve.currentTarget.parentNode.parentNode.childNodes[6].innerText;
                                    /*var AvaiQuantity = AvaiQuantitywithCommas.replace(",", "");*/ //Commented on 17-May-2022
                                    var AvaiQuantity = AvaiQuantitywithCommas.replace(/,/g,'');// Added on 17-May-2022
                                    avail = parseInt(AvaiQuantity) + parseInt(olddmgQty);
                                    var DID = eve.currentTarget.parentNode.parentNode.childNodes[1].innerText;
                                    var TempDID = eve.currentTarget.parentNode.parentNode.childNodes[1].lastChild.value;
                                    if (DID == 0) {
                                        if (TempDID != "") {
                                            $("#hdnDIdamagedID").val(TempDID);
                                        }
                                    }
                                    else {
                                        $("#hdnDIdamagedID").val(DID);
                                    }
                                    var rowid = $("#hdnDIdamagedID").val();
                                    if (parseInt(avail) < parseInt(dmgQty)) {
                                        $(dmgid).addClass("invalidCol");
                                        $('#art1').addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
                                        $('#alertlabel').text("Alert ! Damaged Quantity cannot be more than available quantity.");

                                    }
                                    /*   else if (parseInt(olddmgQty) > parseInt(dmgQty)) {*/
                                    else if (parseInt(olddmgQty || 0) > parseInt(dmgQty || 0)) {
                                        $("#" + dmgid).addClass("invalidCol");
                                        $('#art1').addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
                                        $('#alertlabel').text("Alert ! Damaged Quantity cannot be reduced. It can be increased. For Reducing please raise a new request.");

                                    } else {
                                        $(dmgid).removeClass("invalidCol");
                                    }/* if (parseInt(avail) >= parseInt(dmgQty)) {*/
                                    if (parseInt(avail || 0) >= parseInt(dmgQty || 0)) {
                                        /* if (parseInt(olddmgQty) <= parseInt(dmgQty)) {*/
                                        if (parseInt(olddmgQty || 0) <= parseInt(dmgQty || 0)) {
                                            /*if (olddmgQty != dmgQty) {*/
                                            if ((olddmgQty || "0") != (dmgQty || "0")) {
                                                $("#ajaxLoader").css("display", "block");

                                                hideAlert();
                                                $.ajax({
                                                    type: "POST",
                                                    //  url: "../WebApi/AllInventory.asmx/insertDamage",
                                                    url: "/DamagedInventoryDetails/insertDamage",

                                                    //  data: JSON.stringify({ dmgQty: dmgQty, InvID: invID, UID: UID, id: id, toid: ToId, fromid: fromId, fromtocheck: fromtocheck, DamagedInvId: dmgInvId, DIDetailID: rowid, reason: reason, avlQty: AvaiQuantity }),
                                                    data: JSON.stringify({ dmgQty: dmgQty, InvID: invID, UID: UID, id: id, toid: ToId, fromid: fromId, fromtocheck: fromtocheck, DamagedInvId: dmgInvId, DIDetailID: rowid, reason: reason, avlQty: AvaiQuantity, txtdesc: txtdesc }),
                                                    contentType: "application/json; charset=utf-8",
                                                    dataType: "json",
                                                    success: function (response) {
                                                        // var getdata = response.d;
                                                        var getdata = response;
                                                        if (getdata > 0) {
                                                            if (dmgInvId == 0) {
                                                                $("#hdnDamagedInvId").val(getdata);
                                                                $("#btnRedirect").click();
                                                            }
                                                            else {
                                                                $("#ajaxLoader").css("display", "none");
                                                            }

                                                        }
                                                        else {
                                                            $("#ajaxLoader").css("display", "none");
                                                        }
                                                        //$("#ContentPlaceHolder1_lblSavemsg").css("display", "block");
                                                        //$('#ContentPlaceHolder1_lblSavemsg').addClass('fadeOut');
                                                        //$("#ContentPlaceHolder1_lblSavemsg").css("display", "block");
                                                        //$('#ContentPlaceHolder1_lblSavemsg').text('Record Saved Successfully.');

                                                        $("#lblSavemsg").css("display", "block");
                                                        $('#lblSavemsg').addClass('fadeOut');
                                                        $("#lblSavemsg").css("display", "block");
                                                        $('#lblSavemsg').text('Record Saved Successfully.');

                                                        $(reasonid).css("border", "1px solid #9e9898");
                                                        $(dmgid).css("border", "1px solid #9e9898");
                                                    }
                                                });

                                            }
                                        }
                                    }
                                    var coun = parseInt(txtid[1]);
                                    if ($("#txtReason_" + coun).val() == 0) {
                                        $("#txtReason_" + coun).val('');
                                    }
                                    if ((dmgQty == "" || dmgQty == "0") && reason != "") {
                                        if (parseInt(avail) != parseInt(dmgQty)) {
                                            $(dmgid).addClass("invalidCol");

                                            $('#art1').addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
                                            $('#alertlabel').text("Alert ! Please fill the Damaged Quantity.");
                                        }
                                    }
                                    $(dmgid).css("border", "1px solid #9e9898");
                                    $(reasonid).css("border", "1px solid #9e9898");
                                    $("#txtReason_" + coun).focus();
                                }
                            }
                        });
                        $('td#edit').click(function (eve) {
                            var DID = eve.currentTarget.parentNode.childNodes[1].innerText;
                            var TempDID = eve.currentTarget.parentNode.childNodes[1].lastChild.value;
                            if (DID == 0) {
                                if (TempDID != "") {
                                    $("#hdnDIdamagedID").val(TempDID);
                                }
                            }
                            else {
                                $("#hdnDIdamagedID").val(DID);
                            }

                            $("#hdnAvaQty").val(eve.currentTarget.parentNode.childNodes[6].innerText);
                            //  $("#ContentPlaceHolder1_hdndamaged").val(eve.currentTarget.parentNode.childNodes[2].innerText);
                            $("#hdnDamagedQty").val(eve.currentTarget.parentNode.childNodes[7].childNodes[1].value);
                            $('#btnedit').click();
                        });
                        var dmgQty = "";
                        var olddmgQty = "";
                        var avail = "";
                        var olddmgQtyWithCommas = "";
                        $('.DamageQty').keydown(function (eve) {
                            eve.stopImmediatePropagation();  // Stop Calling Multiple Time When click search and insert the values 05 April 2019
                            // Line Add On 02 Jan 2020 For Description Check Start
                            if ($("#txtdesc").val() == "") {
                                if (eve.keyCode != 8) {
                                    eve.preventDefault();
                                    $('#art1').addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
                                    $('#alertlabel').text("Alert ! Please fill the Description.");
                                }
                            } else {
                                if (eve.keyCode == 9 || eve.keyCode == 13) {
                                    // Line Add On 02 Jan 2020 For Description Check End
                                    //  if (eve.keyCode == 9 || eve.keyCode == 13) {  // Comment On 03 Jan 2020
                                    eve.preventDefault();
                                    dmgid = eve.currentTarget.id;
                                    var txtid = dmgid.split('_');
                                    dmgQtyWithCommas = eve.currentTarget.value;
                                    /*dmgQty = dmgQtyWithCommas.replace(",", "");*///Commented on 17-May-2022
                                    dmgQty = dmgQtyWithCommas.replace(/,/g,''); //Added on 17-May-2022
                                    var reason = eve.currentTarget.parentNode.parentNode.childNodes[9].childNodes[1].value;

                                    var id = $("#hdnid").val();
                                    var fromId = $("#hdnfromid").val();
                                    var ToId = $("#hdnToId").val();
                                    var UID = $("#hdnUID").val();
                                    var dmgInvId = $("#hdnDamagedInvId").val();
                                    var Page = $("#hdnPage").val();
                                    var txtdesc = $("#txtdesc").val();

                                    //var id = $("#ContentPlaceHolder1_hdnid").val();
                                    //var fromId = $("#ContentPlaceHolder1_hdnfromid").val();
                                    //var ToId = $("#ContentPlaceHolder1_hdnToId").val();
                                    //var UID = $("#ContentPlaceHolder1_hdnUID").val();
                                    //var dmgInvId = $("#ContentPlaceHolder1_hdnDamagedInvId").val();
                                    //var Page = $("#ContentPlaceHolder1_hdnPage").val();
                                    //var txtdesc = $("#ContentPlaceHolder1_txtdesc").val();

                                    var DID = eve.currentTarget.parentNode.parentNode.childNodes[1].innerText;
                                    var TempDID = eve.currentTarget.parentNode.parentNode.childNodes[1].lastChild.value;
                                    var invID = eve.currentTarget.parentNode.parentNode.childNodes[2].innerText;

                                    // olddmgQty = eve.currentTarget.defaultValue;
                                    //  olddmgQty = $("#ContentPlaceHolder1_hdnOldQty").val();
                                    olddmgQtyWithCommas = eve.currentTarget.parentNode.parentNode.childNodes[7].childNodes[1].defaultValue;
                                    /*olddmgQty = olddmgQtyWithCommas.replace(",", "");*/ //Commented on 17-May-2022
                                    /* olddmgQty = olddmgQtyWithCommas.replace(/,/g,''); *///Added on 17-May-2022
                                    olddmgQty = olddmgQtyWithCommas.replace(/,/g, '');

                                    if (olddmgQty == "") {
                                        olddmgQty = 0;
                                    }
                                    var AvaiQuantitywithCommas = eve.currentTarget.parentNode.parentNode.childNodes[6].innerText;
                                    /*var AvaiQuantity = AvaiQuantitywithCommas.replace(",", "");*/ //Commented on 17-May-2022
                                    var AvaiQuantity = AvaiQuantitywithCommas.replace(/,/g,''); // Added on 17-May-2022
                                    avail = parseInt(AvaiQuantity) + parseInt(olddmgQty);
                                    var DID = eve.currentTarget.parentNode.parentNode.childNodes[1].innerText;
                                    var TempDID = eve.currentTarget.parentNode.parentNode.childNodes[1].lastChild.value;
                                    if (DID == 0) {
                                        if (TempDID != "") {
                                            $("#hdnDIdamagedID").val(TempDID);
                                        }
                                    }
                                    else {
                                        $("#hdnDIdamagedID").val(DID);
                                    }
                                    var rowid = $("#hdnDIdamagedID").val();
                                    if (parseInt(avail) < parseInt(dmgQty)) {
                                        $("#" + dmgid).addClass("invalidCol");
                                        $('#art1').addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
                                        $('#alertlabel').text("Alert ! Damaged Quantity cannot be more than available quantity.");
                                    }
                                    else if (parseInt(olddmgQty) > parseInt(dmgQty)) {
                                        $("#" + dmgid).addClass("invalidCol");
                                        $('#art1').addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
                                        $('#alertlabel').text("Alert ! Damaged Quantity cannot be reduced. It can be increased. For Reducing please raise a new request.");

                                    } else {
                                        $("#" + dmgid).removeClass("invalidCol");
                                    }
                                    if (parseInt(avail) >= parseInt(dmgQty)) {
                                        if (parseInt(olddmgQty) <= parseInt(dmgQty)) {
                                            if (olddmgQty != dmgQty && (reason == "" || reason != "") && rowid > "0") {
                                                $("#ajaxLoader").css("display", "block");
                                                hideAlert();
                                                $.ajax({
                                                    type: "POST",
                                                    //url: "../WebApi/AllInventory.asmx/insertDamage",
                                                    url: "/DamagedInventoryDetails/insertDamage",

                                                    //  data: JSON.stringify({ dmgQty: dmgQty, InvID: invID, UID: UID, id: id, toid: ToId, fromid: fromId, fromtocheck: fromtocheck, DamagedInvId: dmgInvId, DIDetailID: rowid, reason: reason, avlQty: AvaiQuantity }),
                                                    data: JSON.stringify({ dmgQty: dmgQty, InvID: invID, UID: UID, id: id, toid: ToId, fromid: fromId, fromtocheck: fromtocheck, DamagedInvId: dmgInvId, DIDetailID: rowid, reason: reason, avlQty: AvaiQuantity, txtdesc: txtdesc }),
                                                    contentType: "application/json; charset=utf-8",
                                                    dataType: "json",
                                                    success: function (response) {
                                                        var getdata = response.d;
                                                        if (getdata > 0) {
                                                            if (dmgInvId == 0) {
                                                            }
                                                            else {
                                                                $("#ajaxLoader").css("display", "none");
                                                            }
                                                        }
                                                        else {
                                                            $("#ajaxLoader").css("display", "none");
                                                        }
                                                        $("#lblSavemsg").css("display", "block");
                                                        $('#lblSavemsg').addClass('fadeOut');
                                                        $("#lblSavemsg").css("display", "block");
                                                        $('#lblSavemsg').text('Record Saved Successfully.');
                                                        $("#" + dmgid).css("border", "1px solid #9e9898");
                                                    }


                                                });

                                            }
                                        }
                                    }

                                    var coun = parseInt(txtid[1]);
                                    if ($("#txtReason_" + coun).val() == 0) {
                                        $("#txtReason_" + coun).val('');
                                    }
                                    if (dmgQty == "0" && reason != "") {
                                        if (parseInt(avail) != parseInt(dmgQty)) {
                                            $("#" + dmgid).addClass("invalidCol");
                                            $('#art1').addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
                                            $('#alertlabel').text("Alert ! Please fill the Damaged Quantity.");
                                        }
                                    }
                                    $("#txtReason_" + coun).focus();

                                }
                                else {
                                    if (eve.keyCode == 8) {
                                        oldqty = eve.currentTarget.defaultValue;
                                        $("#hdnOldQty").val(eve.currentTarget.value);
                                    }
                                }
                            }  // Check Description Check Clse bracket


                        });


                        $('.Reason').keydown(function (eve) {
                            eve.stopImmediatePropagation();  // Stop Calling Multiple Time When click search and insert the values 05 April 2019

                            // Line Add On 02 Jan 2020 For Description Check Start
                            if ($("#txtdesc").val() == "") {
                                if (eve.keyCode != 8) {
                                    eve.preventDefault();
                                    $('#art1').addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
                                    $('#alertlabel').text("Alert ! Please fill the Description.");
                                }
                            } else {
                                if (eve.keyCode == 9 || eve.keyCode == 13) {
                                    // Line Add On 02 Jan 2020 For Description Check End
                                    //  if (eve.keyCode == 9 || eve.keyCode == 13) {  // Comment On 03 Jan 2020

                                    eve.preventDefault();
                                    dmgid = eve.currentTarget.id;
                                    var txtid = dmgid.split('_');
                                    var couIndx = dmgid.split('_')[1]
                                    var damagedid = "#txtDmgQty_" + couIndx;
                                    var reason = eve.currentTarget.value;
                                    // var reason=eve.currentTarget.value;
                                    // var DIDetailID = eve.currentTarget.parentNode.parentNode.childNodes[1].innerText;
                                    var invID = eve.currentTarget.parentNode.parentNode.childNodes[2].innerText;
                                    var AvaiQuantitywithCommas = eve.currentTarget.parentNode.parentNode.childNodes[6].innerText;
                                    /*var AvaiQuantity = AvaiQuantitywithCommas.replace(",", "");*/ //Commented  on 17-May-2022
                                    var AvaiQuantity = AvaiQuantitywithCommas.replace(/,/g,''); //Added on 17-May-2022
                                    if (dmgQty > 0) {

                                        var id = $("#hdnid").val();
                                        var fromId = $("#hdnfromid").val();
                                        var ToId = $("#hdnToId").val();
                                        var UID = $("#hdnUID").val();
                                        var dmgInvId = $("#hdnDamagedInvId").val();
                                        var Page = $("#hdnPage").val();
                                        var txtdesc = $("#txtdesc").val();

                                        //var id = $("#ContentPlaceHolder1_hdnid").val();
                                        //var fromId = $("#ContentPlaceHolder1_hdnfromid").val();
                                        //var ToId = $("#ContentPlaceHolder1_hdnToId").val();
                                        //var UID = $("#ContentPlaceHolder1_hdnUID").val();
                                        //var dmgInvId = $("#ContentPlaceHolder1_hdnDamagedInvId").val();
                                        //var Page = $("#ContentPlaceHolder1_hdnPage").val();
                                        //var txtdesc = $("#ContentPlaceHolder1_txtdesc").val();

                                        var DID = eve.currentTarget.parentNode.parentNode.childNodes[1].innerText;
                                        var TempDID = eve.currentTarget.parentNode.parentNode.childNodes[1].lastChild.value;
                                        //if (dmgInvId == 0) {
                                        //    $('*').off('keyup keydown keypress');
                                        //}
                                        if (dmgInvId == 0 && Page == "1" && parseInt(dmgQty) > 0) {
                                            $('*').off('keyup keydown keypress');
                                        }
                                        else {
                                            $('*').on('keyup keydown keypress');
                                        }
                                        if (DID == 0) {
                                            if (TempDID != "") {
                                                $("#hdnDIdamagedID").val(TempDID);
                                            }
                                        }
                                        else {
                                            $("#hdnDIdamagedID").val(DID);
                                        }

                                        var rowid = $("#hdnDIdamagedID").val();
                                        if (parseInt(avail) >= parseInt(dmgQty)) {
                                            if (parseInt(olddmgQty) <= parseInt(dmgQty)) {
                                                $("#ajaxLoader").css("display", "block");

                                                hideAlert();
                                                $.ajax({
                                                    type: "POST",
                                                    // url: "../WebApi/AllInventory.asmx/insertDamage",
                                                    url: "/DamagedInventoryDetails/insertDamage",

                                                    //  data: JSON.stringify({ dmgQty: dmgQty, InvID: invID, UID: UID, id: id, toid: ToId, fromid: fromId, fromtocheck: fromtocheck, DamagedInvId: dmgInvId, DIDetailID: rowid, reason: reason, avlQty: AvaiQuantity }),
                                                    data: JSON.stringify({ dmgQty: dmgQty, InvID: invID, UID: UID, id: id, toid: ToId, fromid: fromId, fromtocheck: fromtocheck, DamagedInvId: dmgInvId, DIDetailID: rowid, reason: reason, avlQty: AvaiQuantity, txtdesc: txtdesc }),
                                                    contentType: "application/json; charset=utf-8",
                                                    dataType: "json",
                                                    success: function (response) {
                                                        // var getdata = response.d;
                                                        var getdata = response;
                                                        if (getdata > 0) {
                                                            if (dmgInvId == 0) {

                                                                $("#hdnDamagedInvId").val(getdata);
                                                                $("#btnRedirect").click();
                                                            } else {

                                                                $("#TempDIDetailsID_" + txtid[1]).val(getdata);
                                                                $("#ajaxLoader").css("display", "none");
                                                                var coun = parseInt(txtid[1]) + 1;
                                                                $("#txtDmgQty_" + coun).select();
                                                            }
                                                        }
                                                        else {
                                                            var coun = parseInt(txtid[1]) + 1;
                                                            $("#txtDmgQty_" + coun).select();
                                                            $("#ajaxLoader").css("display", "none");
                                                        }
                                                        $(damagedid).removeClass("invalidCol");
                                                        $("#lblSavemsg").css("display", "block");
                                                        $('#lblSavemsg').addClass('fadeOut');
                                                        $("#lblSavemsg").css("display", "block");
                                                        $('#lblSavemsg').text('Record Saved Successfully.');
                                                        $(damagedid).css("border", "1px solid #9e9898");
                                                        $("#" + dmgid).css("border", "1px solid #9e9898");
                                                    }
                                                });
                                            }
                                        }
                                    }
                                    else {

                                        if ((dmgQty == "" || dmgQty == "0") && reason != "") {
                                            if (parseInt(avail) != parseInt(dmgQty)) {
                                                $(damagedid).addClass("invalidCol");
                                                $('#art1').addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
                                                $('#alertlabel').text("Alert ! Please fill the Damaged Quantity.");
                                            }
                                        }
                                        else {
                                            $(damagedid).removeClass("invalidCol");
                                            var coun = parseInt(txtid[1]) + 1;
                                            $("#txtDmgQty_" + coun).select();
                                            $("#ajaxLoader").css("display", "none");
                                            $("#" + dmgid).css("border", "1px solid #9e9898");
                                        }

                                    }
                                }
                                else {
                                    if (eve.keyCode == 8) {
                                        oldqty = eve.currentTarget.defaultValue;
                                        $("#hdnOldQty").val(eve.currentTarget.defaultValue);
                                    }
                                }
                            }
                            $(damagedid).css("border", "1px solid #9e9898");
                            $("#" + dmgid).css("border", "1px solid #9e9898");
                        });
                    }
                });

                // scrolling Top when Click On Next Button

                damageTableData.on('page.dt', function () {
                    $('.dataTables_scrollBody').animate({
                        scrollTop: 0
                    }, 300);
                });
            }
            else {
                str += "</tbody></table>";
                $("#damageLoadData").html(str);
                $('#dataforgrid').DataTable({
                    "bSort": false,
                    "processing": true,
                    searching: false,
                    paging: false,
                    info: false,
                    scrollX: true,
                    scrollY: "60vh",
                    scrollCollapse: true,
                });
            }
            $("#ajaxLoader").css("display", "none");
        }
    })

}
$(document).ready(function () {
    var id = $("#hdnid").val();
    var toid = $("#hdnToId").val();
    var fromid = $("#hdnfromid").val();
    var rInvId = $("#hdnRInvId").val();
    var dInvId = $("#hdnDamagedInvId").val();
    var fromtocheck = $("#hdnfromtocheck").val();
    var MatType = $("#ddlMaterialType option:selected").val();
    if (MatType != "" && MatType != undefined) {
        damageDataGrid(id, toid, fromid, rInvId, dInvId, fromtocheck, MatType);
    }
    else {
        damageDataGrid(id, toid, fromid, rInvId, dInvId, fromtocheck, "All");
    }

});
function ddlChangeMaterialType() {
    var id = $("#hdnid").val();
    var toid = $("#hdnToId").val();
    var fromid = $("#hdnfromid").val();
    var rInvId = $("#hdnRInvId").val();
    var dInvId = $("#hdnDamagedInvId").val();
    var fromtocheck = $("#hdnfromtocheck").val();
    var MatType = $("#ddlMaterialType option:selected").val();
    $("#hdnGetMaterialType").val(MatType);
    damageDataGrid(id, toid, fromid, rInvId, dInvId, fromtocheck, MatType);
}

$(function () {

    var Href = window.location.href.split('?');
    if (Href.length == 1) {
        $('#UlNavTabSeciton li').css('display', 'none');
        $('#liYard').css('display', 'block');
        $('#liJob').css('display', 'block');
    }
    $('#UlNavTabSeciton a').css('cursor', 'pointer');
    $('#UlNavTabSeciton a').click(function (e) {
        var UID = getParameterByName('UID');
        var UserRole = getParameterByName('UserRole');
        var id = getParameterByName('id');
        $('#LoadingPanel1').css('display', 'block');
        var ClickAcnhorID = e.currentTarget.id;
        if (ClickAcnhorID == 'AnchorYardInventory') {
            //  window.location.href = '/InventoryManagement.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
            window.location.href = '/InventoryManagement/InventoryManagement?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
        if (ClickAcnhorID == 'AnchorDepotInventory') {
            window.location.href = '/MainDepotInventory/MainDepotInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
        if (ClickAcnhorID == 'AnchorJobInventory') {
            //  window.location.href = '/RequestInventory.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
            window.location.href = '/RequestInventory/RequestInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
        if (ClickAcnhorID == 'AnchorReturnInventory') {
            //  window.location.href = '/ReturnInventory.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
            window.location.href = '/ReturnInventory/ReturnInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
        if (ClickAcnhorID == 'AnchorSiteInventory') {
            //  window.location.href = '/SiteInventory.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
            window.location.href = '/SiteInventory/SiteInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
        if (ClickAcnhorID == 'AnchorDamagedInventory') {
            //  window.location.href = '/DamagedInventory.aspx?' + Href[1];
            window.location.href = '/DamagedInventory/DamagedInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
    });
});


function btnRedirect_Click() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var id = getParameterByName('id');
    var fromtocheck = getParameterByName('fromtocheck');
    var FromId = getParameterByName('FromId');
    var ToId = getParameterByName('ToId');
    var page = getParameterByName('page');

    var hdnDamagedInvId = $('#hdnDamagedInvId').val();

    var EncriptData = Encrypt([hdnDamagedInvId]);
    hdnDamagedInvId = EncriptData[0];

    $('#LoadingPanel1').css('display', 'block');

    window.location.href = '/DamagedInventoryDetails/DamagedInventoryDetails?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&fromtocheck=' + fromtocheck + '&FromId=' + FromId + '&ToId=' + ToId + '&DamagedInvId=' + hdnDamagedInvId + '&page=' + page;

    // Session["MateriType"] = hdnGetMaterialType.Value;
    //  Response.Redirect("DamagedInventoryDetails.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&fromtocheck=" + Request.QueryString["fromtocheck"] + "&id=" + Request.QueryString["id"] + "&FromId=" + Request.QueryString["FromId"] + "&ToId=" + Request.QueryString["ToId"] + "&DamagedInvId=" + databaseCon.Encrypt(hdnDamagedInvId.Value) + "&page=" + Request.QueryString["page"]);


}
function btnedit_Click() {
    if ($('#hdnDIdamagedID').val() != "") {
        var DamagedQuantity = 0;
        if ($('#hdnDamagedQty').val() != "") {
            //  DamagedQuantity = Convert.ToInt32($('#hdnDamagedQty').val().Replace(",",""));
            /*DamagedQuantity = parseInt($('#hdnDamagedQty').val().replace(",", ""));*/ //Commented on 18-May-2022
            DamagedQuantity = parseInt($('#hdnDamagedQty').val().replace(/,/g,'')); //Added on 18-May-2022
        }

        // int DamagedQuantity = 0;
        if (DamagedQuantity != 0) {
            //Session["AvailableQty"] = hdnAvaQty.Value;
            //Response.Redirect("DamagedInventoryConfirmDetails.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&fromtocheck=" + Request.QueryString["fromtocheck"] + "&id=" + Request.QueryString["id"] + "&FromId=" + Request.QueryString["FromId"] + "&ToId=" + Request.QueryString["ToId"] + "&DamagedInvId=" + databaseCon.Encrypt(hdnDamagedInvId.Value) + "&page=" + Request.QueryString["page"] + "&DIDetailID=" + databaseCon.Encrypt(hdnDIdamagedID.Value) + "&FromName=" + databaseCon.Encrypt(txtfrom.Text.Replace("'", "''").Trim()));

            var UID = getParameterByName('UID');
            var UserRole = getParameterByName('UserRole');
            var id = getParameterByName('id');
            var fromtocheck = getParameterByName('fromtocheck');
            var FromId = getParameterByName('FromId');
            var ToId = getParameterByName('ToId');
            var page = getParameterByName('page');

            var hdnDamagedInvId = $('#hdnDamagedInvId').val();
            var hdnDIdamagedID = $('#hdnDIdamagedID').val();
            var txtfrom = $('#txtfrom').val();


            var EncriptData = Encrypt([hdnDamagedInvId, hdnDIdamagedID, txtfrom]);
            hdnDamagedInvId = EncriptData[0];
            hdnDIdamagedID = EncriptData[1];
            txtfrom = EncriptData[2];

            $('#LoadingPanel1').css('display', 'block');

            window.location.href = '/DamagedInventoryConfirmDetails/DamagedInventoryConfirmDetails?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&fromtocheck=' + fromtocheck + '&FromId=' + FromId + '&ToId=' + ToId + '&DamagedInvId=' + hdnDamagedInvId + '&page=' + page + '&DIDetailID=' + hdnDIdamagedID + '&FromName=' + txtfrom;

        }
        else {
            $('#art1').addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
            $('#alertlabel').text("Alert ! Please fill the Damaged Quantity.");
        }
    }
}

function Back_Click() {

    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var id = getParameterByName('id');
    var fromtocheck = getParameterByName('fromtocheck');
    var FromId = getParameterByName('FromId');

    var page = getParameterByName('page');

    var DecriptData = Decrypt([page]);
    page = DecriptData[0];
    if (page != "2") {
        //  Response.Redirect("DamagedInventory.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"]);
        window.location.href = '/DamagedInventory/DamagedInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
    }
    else {
        // Response.Redirect("ScaffoldMaterialListPhase2.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RID=" + Request.QueryString["FromId"]);
        window.location.href = '/ScaffoldMaterialListPhase2/ScaffoldMaterialListPhase2?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + FromId;
    }
}



function GetTotalTonnage() {
    var ClientID = getParameterByName('id');
    var dmgInvId = $("#hdnDamagedInvId").val();
    $.ajax({
        type: "POST",
        url: "/DamagedInventoryDetails/GetTotalTonnage",
        data: JSON.stringify({ ClientID: ClientID, dmgInvId: dmgInvId }),
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


function btnrefresh_Click() {

    var ClientID = getParameterByName('id');
    var dmgInvId = $("#hdnDamagedInvId").val();
    var txtdesc = $("#txtdesc").val();
    $.ajax({
        type: "POST",
        url: "/DamagedInventoryDetails/btnrefresh_Click",
        data: JSON.stringify({ ClientID: ClientID, dmgInvId: dmgInvId, txtdesc: txtdesc }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {


        }

    })
    location.reload();
}