
$(document).ready(function () {

    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var FromId = getParameterByName('FromId');
    var from = getParameterByName('from');
    var ToId = getParameterByName('ToId');
    var To = getParameterByName('To');
    var checkid = getParameterByName('checkid');
    var fromtocheck = getParameterByName('fromtocheck');
    var DelStatus = getParameterByName('DelStatus');

    var DecriptData = [id, UID, FromId, from, ToId, To, checkid, fromtocheck, DelStatus];
    var DecrData = Decrypt(DecriptData);
    id = DecrData[0];
    UID = DecrData[1];
    FromId = DecrData[2];
    from = DecrData[3];
    ToId = DecrData[4];
    To = DecrData[5];
    checkid = DecrData[6];
    fromtocheck = DecrData[7];
    DelStatus = DecrData[8];

    $('#hdnClientId').val(id);
    $('#hdnUId').val(UID);
    $('#hdnFromID').val(FromId);
    $('#hdnFrom').val(from);
    $('#hdnToId').val(ToId);
    $('#hdnTo').val(To);
    $('#hdncheckId').val(checkid);
    $('#hdnfromcheck').val(fromtocheck);

    if (DelStatus != null && DelStatus != "NotConfirmed") {

        $("#btnAllocate").css("display", "none");
        $("#btnconfirmDelvry").css("display", "none");
        $("#lockimage").css("display", "block");
        $('#hdntextdisable').val(true);
    }

    MaterialTupeDropDownBind();
});

function Back_Click() {
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    window.location.href = '/DepotRequestInventory/DepotRequestInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
}

function btnconfirmno_Click() {
    $("#confmodal").css("display", "none");

}

function btnconfirmDelvry_Click() {
    var hdncheckId = $('#hdncheckId').val();
    $.ajax({
        type: "POST",
        url: "/DeliveryInventory/btnconfirmDelvry_Click",
        data: JSON.stringify({ hdncheckId: hdncheckId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {

            if (response.result == "Success") {
                $("#confmodal").css("display", "block");
            } else {
                $('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
                $('#art1').css("display", "block");
                $('#alertlabel').text("Alert !" + response.Message);
            }
        }
    })
}

function btnconfirmyes_Click() {
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var FromId = getParameterByName('FromId');
    var from = getParameterByName('from');
    var ToId = getParameterByName('ToId');
    var To = getParameterByName('To');
    var checkid = getParameterByName('checkid');
    var fromtocheck = getParameterByName('fromtocheck');
    var DelStatus = getParameterByName('DelStatus');

    $.ajax({
        type: "POST",
        url: "/DeliveryInventory/btnconfirmyes_Click",
        data: JSON.stringify({ id: id, UID: UID, DelStatus: DelStatus, hdncheckId: checkid, UserRole: UserRole, fromid: FromId, fromtocheck: fromtocheck }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            if (response.result == "Success") {
                var DocketNo = response.DocketNo;
                window.location.href = '/DeliveryDocket/DeliveryDocket?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&ToId=' + FromId + '&checkid=' + checkid + '&DDId=' + DocketNo;
            }
            else {
                $('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
                $('#art1').css("display", "block");
                $('#alertlabel').text("Alert !" + response.Message);
            }
        }
    })
}

function MaterialTupeDropDownBind() {
    var id = getParameterByName('id');
    $.ajax({
        type: "POST",
        url: "/DeliveryInventory/MaterialDropDown",
        data: JSON.stringify({ id: id }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {

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

function numberWithCommas(x) {
    var parts = x.toString().split(".");
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join(".");
}
var table;
function griddeliverydata(check) {
    $("#ajaxLoader").css("display", "block");
    $.ajax({
        type: "POST",
        url: "/DeliveryInventory/getDeliverydata",
        data: JSON.stringify({ check: check }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var getgriddelivery = response;
            if (getgriddelivery.length > 0) {
                var str = "";
                var transcheck = "";
                var transcolor = "";
                str += '<table id="deliverydata" cellspacing="0" width="100%" class="table table-striped">';
                str += "<thead><tr>";

                str += "<th style='width:5%;text-align:left;'>TransId</th>";
                str += "<th style='width:5%;display:none;text-align:left;'>TransReqId</th>";
                str += "<th style='width:5%;display:none;text-align:left;'>TransDeliId</th>";
                str += "<th style='width:5%;display:none;text-align:left;'>InventoryId</th>";
                str += "<th style='width:12%;text-align:left;'>Item</th>";
                str += "<th style='width:10%;text-align:left;'>Item Code</th>";
                str += "<th style='width:30%;text-align:left;'>Item Desc</th>";
                str += "<th style='width:10%;text-align:center;'>Available Quantity</th>";
                str += "<th style='width:10%;text-align:center;'>Requested Quantity</th>";
                str += "<th style='width:5%;text-align:center;'>Allocated Quantity</th>";
                str += "<th style='width:3%;text-align:left;'>#</th></tr></thead><tbody>";
                for (var i = 0; i < getgriddelivery.length; i++) {
                    var TransId = getgriddelivery[i].TransId == null ? "" : getgriddelivery[i].TransId;
                    var TransReqId = getgriddelivery[i].TransReqId == null ? "" : getgriddelivery[i].TransReqId;
                    var TransDeliId = getgriddelivery[i].TransDeliId == null ? "" : getgriddelivery[i].TransDeliId;
                    var InvtoyId = getgriddelivery[i].InventoryId == null ? "" : getgriddelivery[i].InventoryId;
                    var MaterialType = getgriddelivery[i].MaterialType == null ? "" : getgriddelivery[i].MaterialType;
                    var ItemCode = getgriddelivery[i].ItemCode == null ? "" : getgriddelivery[i].ItemCode;
                    var ItemDesc = getgriddelivery[i].ItemDesc == null ? "" : getgriddelivery[i].ItemDesc;
                    var AvailableQuantity = getgriddelivery[i].AvailableQuantity == null ? "" : getgriddelivery[i].AvailableQuantity;
                    var ReqQty = getgriddelivery[i].Quantity == null ? "" : getgriddelivery[i].Quantity;
                    var AllocatedQty = getgriddelivery[i].AllocatedQuantity == null ? 0 : getgriddelivery[i].AllocatedQuantity;

                    if (transcheck == "") {
                        transcheck = TransId;
                    }

                    if (transcheck != TransId) {
                        transcheck = TransId;
                        if (transcolor == "") {
                            transcolor = "lightseagreen";
                            str += "<tr><td style='width:5%;text-align:center;color:lightseagreen'>" + TransId + "</td>";
                        }
                        else {
                            if (transcolor == "lightseagreen") {
                                transcolor = "mediumblue";
                                str += "<tr><td style='width:5%;text-align:center;color:mediumblue'>" + TransId + "</td>";
                            }
                            else {
                                transcolor = "lightseagreen";
                                str += "<tr><td style='width:5%;text-align:center;color:lightseagreen'>" + TransId + "</td>";
                            }
                        }

                    }
                    else {
                        if (transcolor == "lightseagreen") {

                            str += "<td style='width:5%;text-align:center;color:lightseagreen'>" + TransId + "</td>";
                        }
                        else {

                            str += "<td style='width:5%;text-align:center;color:mediumblue'>" + TransId + "</td>";
                        }
                    }

                    str += "<td style='width:5%;display:none;text-align:left;'>" + TransReqId + "</td>";
                    str += "<td style='width:5%;display:none;text-align:left;'>" + TransDeliId + "</td>";
                    str += "<td style='width:5%;display:none;text-align:left;'>" + InvtoyId + "</td>";
                    str += "<td style='width:12%;text-align:left;'>" + MaterialType + "</td>";
                    str += "<td style='width:10%;text-align:left;'>" + ItemCode + "</td>";
                    str += "<td style='width:30%;text-align:left;'>" + ItemDesc + "</td>";
                    str += "<td style='width:10%;text-align:center;'>" + numberWithCommas(AvailableQuantity) + "</td>";
                    str += "<td style='width:10%;text-align:center;'>" + numberWithCommas(ReqQty) + "</td>";
                    if ($("#hdntextdisable").val() == "true") {
                        str += "<td style='width:5%;text-align:center;background-color:LightCyan'> <input style='width:70%;' id='txtqty_" + i + "' type='text' class='txtAllocateQty' value=" + numberWithCommas(AllocatedQty) + " readonly='true' /><input style='width:70%;' id='hdnid_" + i + "' type='hidden' value='0' /></td>";
                        str += "<td style='width:3%;'><a id='rowSave_" + i + "' style='pointer-events: none; cursor: default;' class='fa fa-save rowSaveButton'></a></td></tr>";
                    }
                    else {
                        str += "<td style='width:5%;text-align:center;background-color:LightCyan'><input style='width:70%;' id='txtqty_" + i + "' type='text' class='txtAllocateQty' autocomplete='off' value=" + numberWithCommas(AllocatedQty) + "><input style='width:70%;' id='hdnid_" + i + "' type='hidden' value='0' /></td>";
                        str += "<td style='width:3%;'><a id='rowSave_" + i + "' class='fa fa-save rowSaveButton'></a></td></tr>";
                    }

                }
                str += "</tbody></table>";
                $('#loaddata').html(str);
                $('#deliverydata thead tr').clone(true).appendTo('#deliverydata thead');
                $('#deliverydata thead tr:eq(1) th').each(function (i) {
                    var title = $(this).text();
                    if (title == "TransId") {
                        title = "Id";
                    }
                    if (title == "Item") {
                        title = "Search Item";
                    }
                    if (title == "Item Desc") {
                        title = "Search Item Desc";
                    }
                    if (title == "Available Quantity") {
                        title = "Available";
                    }
                    $(this).html('<input id="filter_' + i + '" style="width:90%; font-weight: normal; opacity: 0.7; border: 1px solid #797575;" type="text" placeholder=" ' + title + '" />');

                    $('input', this).on('keyup change', function () {
                        if (table.column(i).search() !== this.value) {
                            table
                                .column(i)
                                .search(this.value)
                                .draw();
                        }
                    });
                    if (i > 7) {
                        $("#filter_" + i).css("display", "none");
                    }
                });
                table = $('#deliverydata').DataTable({
                    "bSort": false,
                    "processing": true,
                    searching: true,
                    info: false,
                    "pageLength": 50,
                    scrollX: true,
                    scrollY: "60vh",
                    "drawCallback": function () {


                        $('.txtAllocateQty').keydown(function (eve) {
                            if (eve.keyCode != 8 && eve.keyCode != undefined) {
                                var value = eve.key
                                var regex = /^[-_!@#$%^&*()a-zA-Z]+$/;
                                if (value.match(regex)) {
                                    eve.preventDefault();
                                }
                                if (eve.keyCode == 189) {
                                    eve.preventDefault();
                                }
                            }

                        });
                        $(".txtAllocateQty").keyup(function (eve) {
                            var txtAlocatid = eve.currentTarget.id;
                            var colIndx = txtAlocatid.split('_')[1]
                            var tid = '#txtqty_' + colIndx;
                            $(tid).css("border", "1px solid red");
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
                                    eve.stopImmediatePropagation();
                                    var colIndx = hdnsavedata.split('_')[1]
                                    var tid = '#txtqty_' + colIndx;
                                    var txtid = tid.split('_');
                                    var AllocatedQtyWithCommas = $(this).parents().find('#txtqty_' + colIndx).val();
                                    var AllocatedQty = AllocatedQtyWithCommas.replace(",", "");
                                    var tempid = '#hdnid_' + colIndx;
                                    var TransReqId = eve.currentTarget.parentNode.parentNode.childNodes[1].innerText;
                                    var TransDeliId = eve.currentTarget.parentNode.parentNode.childNodes[2].innerText;
                                    if (TransDeliId == "") {
                                        TransDeliId = 0;
                                    }
                                    if ($(tempid).val() != "0") {
                                        TransDeliId = $(tempid).val();
                                    }
                                    var DeliId = TransDeliId;
                                    var InvId = eve.currentTarget.parentNode.parentNode.childNodes[3].innerText;
                                    var AvaQtyWithCommas = eve.currentTarget.parentNode.parentNode.childNodes[7].innerText;
                                    var AvaQty = AvaQtyWithCommas.replace(",", "");

                                    var Id = $("#hdnClientId").val();
                                    var From = $("#hdnFrom").val();
                                    var FromId = $("#hdnFromID").val();
                                    var To = $("#hdnTo").val();
                                    var ToId = $("#hdnToId").val();
                                    var UID = $("#hdnUId").val();
                                    var CheckID = $("#hdncheckId").val();

                                    if (parseInt(AvaQty) < parseInt(AllocatedQty)) {
                                        $(tid).addClass("invalidCol");
                                        $(tid).css({ "background-color": "red", "color": "black" });
                                        $('#art1').addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
                                        $('#alertlabel').text("Alert ! Please rectify the quantity marked in red.");
                                    }
                                    else {
                                        if (AllocatedQty != "") {
                                            $("#ajaxLoader").css("display", "block");
                                            $(tid).removeClass("invalidCol");
                                            $(tid).css({ "background": "white", "color": "black" });
                                            $.ajax({
                                                type: "POST",
                                                url: "/DeliveryInventory/InsertDeliveryData",
                                                data: JSON.stringify({ AllocatedQty: AllocatedQty, InvId: InvId, UID: UID, From: From, FromId: FromId, To: To, ToId: ToId, Id: Id, CheckID: CheckID, TransReqId: TransReqId, DeliId: DeliId, TransDeliId: TransDeliId }),
                                                contentType: "application/json; charset=utf-8",
                                                dataType: "json",
                                                success: function (response) {
                                                    var getdata = response;
                                                    if (getdata > 0) {
                                                        $(tempid).val(getdata);
                                                    }
                                                    $(tid).css("border", "1px solid #9e9898");
                                                    $("#ajaxLoader").css("display", "none");
                                                }
                                            });
                                        }
                                    }
                                }
                            }
                        });
                        $('.txtAllocateQty').keydown(function (eve) {
                            eve.stopImmediatePropagation(); 
                            if (eve.keyCode == 9 || eve.keyCode == 13) {
                                eve.preventDefault();
                                eve.stopImmediatePropagation();
                                var tid = eve.currentTarget.id;
                                var txtid = tid.split('_');
                                var AllocatedQtyWithCommas = eve.currentTarget.value;
                                var AllocatedQty = AllocatedQtyWithCommas.replace(",", "");
                                var tempid = eve.currentTarget.nextSibling.id;
                                var TransReqId = eve.currentTarget.parentNode.parentNode.childNodes[1].innerText;
                                var TransDeliId = eve.currentTarget.parentNode.parentNode.childNodes[2].innerText;
                                if (TransDeliId == "") {
                                    TransDeliId = 0;
                                }
                                if ($("#" + tempid).val() != "0") {
                                    TransDeliId = $("#" + tempid).val();
                                }
                                var DeliId = TransDeliId;
                                var InvId = eve.currentTarget.parentNode.parentNode.childNodes[3].innerText;
                                var AvaQtyWithCommas = eve.currentTarget.parentNode.parentNode.childNodes[7].innerText;
                                var AvaQty = AvaQtyWithCommas.replace(",", "");

                                var Id = $("#hdnClientId").val();
                                var From = $("#hdnFrom").val();
                                var FromId = $("#hdnFromID").val();
                                var To = $("#hdnTo").val();
                                var ToId = $("#hdnToId").val();
                                var UID = $("#hdnUId").val();
                                var CheckID = $("#hdncheckId").val();

                                if (parseInt(AvaQty) < parseInt(AllocatedQty)) {
                                    $("#" + tid).addClass("invalidCol");
                                    $("#" + tid).css({ "background-color": "red", "color": "black" });
                                    $('#art1').addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
                                    $('#alertlabel').text("Alert ! Please rectify the quantity marked in red.");
                                }
                                else {
                                    if (AllocatedQty != "") {

                                        $("#ajaxLoader").css("display", "block");
                                        $("#" + tid).removeClass("invalidCol");
                                        $("#" + tid).css({ "background": "white", "color": "black" });
                                        $.ajax({
                                            type: "POST",
                                            url: "/DeliveryInventory/InsertDeliveryData",

                                            data: JSON.stringify({ AllocatedQty: AllocatedQty, InvId: InvId, UID: UID, From: From, FromId: FromId, To: To, ToId: ToId, Id: Id, CheckID: CheckID, TransReqId: TransReqId, DeliId: DeliId, TransDeliId: TransDeliId }),
                                            contentType: "application/json; charset=utf-8",
                                            dataType: "json",
                                            success: function (response) {
                                                var getdata = response;
                                                if (getdata > 0) {
                                                    $("#" + tempid).val(getdata);
                                                }
                                                $("#" + tid).css("border", "1px solid #9e9898");
                                                $("#ajaxLoader").css("display", "none");
                                            }
                                        });
                                    }
                                }
                                var coun = parseInt(txtid[1]) + 1;
                                $("#txtqty_" + coun).select();
                            }

                        });

                    }
                });


                table.on('page.dt', function () {
                    $('.dataTables_scrollBody').animate({
                        scrollTop: 0
                    }, 300);
                });

            }
            $("#ajaxLoader").css("display", "none");
        }
    })

}
var count = 0;
var NumRows = 0;
$(document).ready(function () {
    var check = $("#hdncheckId").val();
    griddeliverydata(check);
    if ($("#getgridcount").val() != "") {
        NumRows = $("#getgridcount").val();
    }

    $(".currentRow").on("click", function () {
        for (i = 0; i < NumRows; i++) {
            if ("ctl00_ContentPlaceHolder1_delivery_ctl00__" + i == $(this).parent().attr("id")) {
                count = i;
                break;
            }
        }
    });

});
function filterhide() {
    $(".rgFilterRow").css("display", "none");
}

function onclientblur1(sender, args) {
    var Id = sender._clientID;
    var rowid = $("#" + Id).parent().parent().parent().parent().attr('id');
    var convert = $("#" + rowid + "").find(".AvailableQty").text();
    convert = convert.replace(",", "");
    availVal = parseInt(convert);
    var checkwithconvert = $("#" + rowid + "").find(".AllocateQty").closest('input[type="text"]').val();
    checkwithconvert = checkwithconvert.replace(",", "");
    allocval = parseInt(checkwithconvert);
    if (availVal >= allocval) {
        $("#" + rowid + "").find(".AllocateQty").parent().parent().parent().removeClass("invalidCol");
    }
    else if (availVal < allocval) {
        $("#" + rowid + "").find(".AllocateQty").parent().parent().parent().addClass("invalidCol");
    }
    else {
        $("#" + rowid + "").find(".AllocateQty").parent().parent().parent().removeClass("invalidCol");
    }


}

$(document).ready(function () {

    $('.double-scroll').doubleScroll();
    checkbox1();
});
function checkbox1() {
    $("input[type='checkbox']").attr('disabled', false);
}
function RowSelect() {

    $("tr.rgSelectedRow input[type='checkbox']").prop('checked', true);
}

$(document).ready(function () {
    var screensize = $(window).width();
    if (screensize < 1150 && screensize > 650) {
        ($BODY.hasClass('nav-md'))
        $('#CreateNewDropDown').css("margin-top", "2px");
        $BODY.removeClass('nav-md').addClass('nav-sm');
        $LEFT_COL.removeClass('scroll-view');
        $('#imgLogo').parent().css('display', 'none');
        $('#imgLogo1').css('display', 'block').css("height", "38px");
        $('#leftbarsection').css('margin-top', '0px');
    }
    else {
        if (screensize < (650)) {
            $BODY.removeClass('nav-sm')
            $('#imgLogo').parent().css('display', 'none');
            $('#imgLogo1').css('display', 'block').css("height", "52px");
            $('#btnsave').css('width', '130%').css("font-size", "8px");
            $('#btncancel').css('width', '144%').css("font-size", "8px");
            $('#ExportTo').css('width', '118%').css("font-size", "8px").css('margin-left', '7px');
            $('.xsfieldset').css('margin-left', '-22px').css("width", "100%");
            $('#btnconfirmDelvry').css('display', 'none !important');
            $('#btnConfirmD').css('display', 'block').css('font-size', '10px !important').css('width', '100%').css('padding-left', '0px');
        }
        else {
            $('#CreateNewDropDown').css("margin-top", "6px");
            $BODY.removeClass('nav-sm').addClass('nav-md');
            $('#imgLogo').parent().css('display', 'block');
            $('#imgLogo1').css('display', 'none');
            $('#leftbarsection').css('margin-top', '40px');
        }

    }
});


$(function () {

    var Href = window.location.href.split('?');
    if (Href.length == 1) {
        $('#UlNavTabSeciton li').css('display', 'none');
        $('#liYard').css('display', 'block');
    }
    $('#UlNavTabSeciton a').css('cursor', 'pointer');
    $('#UlNavTabSeciton a').click(function (e) {
        var UID = getParameterByName('UID');
        var UserRole = getParameterByName('UserRole');
        var id = getParameterByName('id');
        $('#LoadingPanel1').css('display', 'block');
        var ClickAcnhorID = e.currentTarget.id;
        if (ClickAcnhorID == 'AnchorYardInventory') {
            window.location.href = '/InventoryManagement/InventoryManagement?' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
        if (ClickAcnhorID == 'AnchorDelInventory') {
            $('#LoadingPanel1').css('display', 'none');
        }
        if (ClickAcnhorID == 'AnchorJobInventory') {
            window.location.href = '/RequestInventory/RequestInventory?' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
        if (ClickAcnhorID == 'AnchorSiteInventory') {
            window.location.href = '/SiteInventory/SiteInventory?' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
    });

});
$(document).ready(function () {
    $("select.chosen-select").select2();
});
function show(txtQty, lblItemWeight, lblTotalweight) {
    var textQuantity = $find(txtQty);
    var lblItemWeight = $find(lblItemWeight);
    var lblTotalweight = $find(lblTotalweight);
    lblTotalweight.val(textQuantity.val() * lblItemWeight.val());

}
function checked() {
    Chkselected.Checked = "True";
}

displaypopup = false;
function Deliverypopup() {
    for (i = 0; i < NumRows; i++) {
        var lbldelquant = $("#ctl00_ContentPlaceHolder1_delivery_ctl00__" + i).find(".currentRow").text();
        // var lbldelquant = eve.currentTarget.value;
        var delqty = lbldelquant.trim();
        if (delqty != "") {
            displaypopup = true
        }
    }
    if (displaypopup == true) {
        $('#confmodal').css('display', 'block');
        displaypopup = false;
    }
    else {
        $('#art1').addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
        $('#alertlabel').text("Alert ! Please fill in Quantity to Deliver then click on Confirm Delivery.");
    }

}
function selectmattype() {

    $('#btnconfirm').click();


}
