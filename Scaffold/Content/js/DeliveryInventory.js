
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
    GetRequestComment();
    GetWhereFromWhereTo();
});

function Back_Click() {
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var fromtocheck = getParameterByName('fromtocheck');
    // Response.Redirect("RequestInventory.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"], true);
    //window.location.href = '/RequestInventory/RequestInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
    window.location.href = '/RequestInventory/RequestInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&fromtocheck=' + fromtocheck;
}

function btnconfirmno_Click() {
    $("#confmodal").css("display", "none");

}

function GetRequestComment() {
    $('#LoadingPanel1').css('display', 'block');
    var ReqId = $('#hdncheckId').val();

    $.ajax({
        type: "POST",
        url: "/DeliveryInventory/DeliveryInventorycommentData",
        data: JSON.stringify({ ReqId: ReqId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            if (response != null || response != "") {
                var Comment = response;
                $('#txtReqComment').val(Comment)
            }
            else {
                //alert
            }
            $('#LoadingPanel1').css('display', 'none');
        }
    });

    //window.location.href = '/DeliveryInventory/DeliveryInventory?ReqId=' + ReqId;

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
    /*validation_ondeliveryconfirm();*/ //Commented on 18-May-2022
}

function btnconfirmyes_Click() {
    //  string id,string UID,string DelStatus,string hdncheckId,string UserRole,string fromid,string fromtocheck
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
                // return RedirectToAction("DeliveryDocket", "DeliveryDocket", new { id = id, UID = UID, UserRole = UserRole, ToId = fromid, checkid = databaseCon.Encrypt(hdncheckId), DDId = databaseCon.Encrypt(DocketNo.ToString()) });
                window.location.href = '/DeliveryDocket/DeliveryDocket?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&ToId=' + FromId + '&checkid=' + checkid + '&DDId=' + DocketNo + '&To=' + To;
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
    $('#LoadingPanel1').css('display', 'block');
    var id = getParameterByName('id');
    $.ajax({
        type: "POST",
        url: "/DeliveryInventory/MaterialDropDown",
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
            $('#LoadingPanel1').css('display', 'none');
        }
    })

}

function numberWithCommas(x) {
    var parts = x.toString().split(".");
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join(".");
}
var table;
function griddeliverydata(check, fromcheck) {
    GetTotalTotalTonnage();
    debugger;
    var from = getParameterByName('from');
    var To = getParameterByName('To');
    var UserRole = getParameterByName('UserRole');
    var DecriptData = [from, To, UserRole];
    var DecrData = Decrypt(DecriptData);

    from = DecrData[0];
    To = DecrData[1];
    var DecrUserRole = DecrData[2];
    debugger;
    if (To == "ThirdParty" && from == "Depot" && DecrUserRole == "10") {
        document.getElementById("chkdisplay").style.display = "block";
    }
    else {
        document.getElementById("chkdisplay").style.display = "none";

    }
    $("#ajaxLoader").css("display", "block");
    $.ajax({
        type: "POST",
        //  url: "../WebApi/AllInventory.asmx/getDeliverydata",
        url: "/DeliveryInventory/getDeliverydata",
        //data: JSON.stringify({ check: check }),
        data: JSON.stringify({ check: check, fromcheck: fromcheck }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            // var getgriddelivery = response.d;
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
                    //var ReqQty = getgriddelivery[i].ReqQty == null ? "" : getgriddelivery[i].ReqQty;
                    var ReqQty = getgriddelivery[i].Quantity == null ? "" : getgriddelivery[i].Quantity;
                    /*  var AllocatedQty = getgriddelivery[i].AllocatedQuantity == null ? 0 : getgriddelivery[i].AllocatedQuantity;*/
                    var AllocatedQty = getgriddelivery[i].AllocatedQuantity == null ? "" : getgriddelivery[i].AllocatedQuantity;
                    var DisplayAllocatedQty = AllocatedQty == "" ? "" : numberWithCommas(AllocatedQty);

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
                    // str += "<tr><td style='width:5%;text-align:center;color:mediumblue'>" + TransId + "</td>";

                    str += "<td style='width:5%;display:none;text-align:left;'>" + TransReqId + "</td>";
                    str += "<td style='width:5%;display:none;text-align:left;'>" + TransDeliId + "</td>";
                    str += "<td style='width:5%;display:none;text-align:left;'>" + InvtoyId + "</td>";
                    str += "<td style='width:12%;text-align:left;'>" + MaterialType + "</td>";
                    str += "<td style='width:10%;text-align:left;'>" + ItemCode + "</td>";
                    str += "<td style='width:30%;text-align:left;'>" + ItemDesc + "</td>";
                    str += "<td style='width:10%;text-align:center;'>" + numberWithCommas(AvailableQuantity + AllocatedQty) + "</td>";
                    //str += "<td style='width:10%;text-align:center;'>" + numberWithCommas(ReqQty) + "</td>";
                    str += "<td style='width:10%;text-align:center;'>" + "<input style='width:70%;' id='txtReqqty_" + i + "' type='label'  value='" + numberWithCommas(ReqQty) + "' readonly='true' />" + "</td>";
                    if ($("#hdntextdisable").val() == "true") {
                        str += "<td style='width:5%;text-align:center;background-color:LightCyan'> <input style='width:70%;' id='txtqty_" + i + "' type='text' class='txtAllocateQty' value='" + DisplayAllocatedQty + "' readonly='true' /><input style='width:70%;' id='hdnid_" + i + "' type='hidden' value='0' /></td>";
                        str += "<td style='width:3%;'><a id='rowSave_" + i + "' style='pointer-events: none; cursor: default;' class='fa fa-save rowSaveButton'></a></td></tr>";
                    }
                    else {
                        str += "<td style='width:5%;text-align:center;background-color:LightCyan'><input style='width:70%;' id='txtqty_" + i + "' type='text' class='txtAllocateQty' autocomplete='off' value='" + DisplayAllocatedQty + "'><input style='width:70%;' id='hdnid_" + i + "' type='hidden' value='0' /></td>";
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
                    //paging: false,
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
                                    //$('.txtAllocateQty').keydown();
                                    eve.preventDefault();
                                    eve.stopImmediatePropagation();
                                    var colIndx = hdnsavedata.split('_')[1]
                                    var tid = '#txtqty_' + colIndx;
                                    var txtid = tid.split('_');
                                    var AllocatedQtyWithCommas = $(this).parents().find('#txtqty_' + colIndx).val();
                                    var AllocatedQtywithcommas = AllocatedQtyWithCommas.replace(",", "");
                                    var AllocatedQty = AllocatedQtywithcommas.replace(/,/g, '');//Added on 17-May-2022
                                    var tempid = '#hdnid_' + colIndx;
                                    var TransReqId = eve.currentTarget.parentNode.parentNode.childNodes[1].innerText;
                                    //var TransDeliId = eve.currentTarget.parentNode.parentNode.childNodes[2].innerText;
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
                                    /*var AvaQty = AvaQtyWithCommas.replace(",", "");*/  //Commented on 17-May-2022
                                    var AvaQty = AvaQtyWithCommas.replace(/,/g, '');  // Added on 17-May-2022
                                    //var Id = $("#ContentPlaceHolder1_hdnClientId").val();
                                    //var From = $("#ContentPlaceHolder1_hdnFrom").val();
                                    //var FromId = $("#ContentPlaceHolder1_hdnFromID").val();
                                    //var To = $("#ContentPlaceHolder1_hdnTo").val();
                                    //var ToId = $("#ContentPlaceHolder1_hdnToId").val();
                                    //var UID = $("#ContentPlaceHolder1_hdnUId").val();
                                    //var CheckID = $("#ContentPlaceHolder1_hdncheckId").val();

                                    var Id = $("#hdnClientId").val();
                                    var From = $("#hdnFrom").val();
                                    var FromId = $("#hdnFromID").val();
                                    var To = $("#hdnTo").val();
                                    var ToId = $("#hdnToId").val();
                                    var UID = $("#hdnUId").val();
                                    var CheckID = $("#hdncheckId").val();
                                    GetTotalTotalTonnage();
                                    if (parseInt(AvaQty) < parseInt(AllocatedQty)) {
                                        $(tid).addClass("invalidCol");
                                        $(tid).css({ "background-color": "red", "color": "white" });
                                        $('#art1').addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
                                        $('#alertlabel').text("Alert ! Please rectify the quantity marked in red.");
                                        $('#art1').delay(5000).fadeOut('slow');
                                    }
                                    else {
                                        if (AllocatedQty != "") {

                                            $("#ajaxLoader").css("display", "block");
                                            $(tid).removeClass("invalidCol");
                                            $(tid).css({ "background": "white", "color": "black" });
                                            $.ajax({
                                                type: "POST",
                                                // url: "../WebApi/AllInventory.asmx/InsertDeliveryData",
                                                url: "/DeliveryInventory/InsertDeliveryData",
                                                data: JSON.stringify({ AllocatedQty: AllocatedQty, InvId: InvId, UID: UID, From: From, FromId: FromId, To: To, ToId: ToId, Id: Id, CheckID: CheckID, TransReqId: TransReqId, DeliId: DeliId, TransDeliId: TransDeliId }),
                                                contentType: "application/json; charset=utf-8",
                                                dataType: "json",
                                                success: function (response) {
                                                    // var getdata = response.d;
                                                    var getdata = response;
                                                    if (getdata > 0) {
                                                        $(tempid).val(getdata);
                                                    }
                                                    $(tid).css("border", "1px solid #9e9898");
                                                    GetTotalTotalTonnage();
                                                    $("#ajaxLoader").css("display", "none");
                                                }
                                            });
                                        }
                                    }
                                }
                            }
                        });
                        $('.txtAllocateQty').keydown(function (eve) {

                            eve.stopImmediatePropagation();  // Stop Calling Multiple Time When click search and insert the values 05 April 2019
                            if (eve.keyCode == 9 || eve.keyCode == 13) {

                                eve.preventDefault();
                                eve.stopImmediatePropagation();
                                var tid = eve.currentTarget.id;
                                var txtid = tid.split('_');
                                var AllocatedQtyWithCommas = eve.currentTarget.value;
                                /*var AllocatedQty = AllocatedQtyWithCommas.replace(",", "");*/ //ccommented on 17-May-2022
                                var AllocatedQty = AllocatedQtyWithCommas.replace(/,/g, ''); // Added on 17-May-2022
                                var tempid = eve.currentTarget.nextSibling.id;
                                var TransReqId = eve.currentTarget.parentNode.parentNode.childNodes[1].innerText;
                                //var TransDeliId = eve.currentTarget.parentNode.parentNode.childNodes[2].innerText;
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
                                /*var AvaQty = AvaQtyWithCommas.replace(",", "");*/ //Commented on 17-May-2022
                                var AvaQty = AvaQtyWithCommas.replace(/,/g, ''); // Added on 17-MAy-2022
                                //var Id = $("#ContentPlaceHolder1_hdnClientId").val();
                                //var From = $("#ContentPlaceHolder1_hdnFrom").val();
                                //var FromId = $("#ContentPlaceHolder1_hdnFromID").val();
                                //var To = $("#ContentPlaceHolder1_hdnTo").val();
                                //var ToId = $("#ContentPlaceHolder1_hdnToId").val();
                                //var UID = $("#ContentPlaceHolder1_hdnUId").val();
                                //var CheckID = $("#ContentPlaceHolder1_hdncheckId").val();

                                var Id = $("#hdnClientId").val();
                                var From = $("#hdnFrom").val();
                                var FromId = $("#hdnFromID").val();
                                var To = $("#hdnTo").val();
                                var ToId = $("#hdnToId").val();
                                var UID = $("#hdnUId").val();
                                var CheckID = $("#hdncheckId").val();

                                if (parseInt(AvaQty) < parseInt(AllocatedQty)) {
                                    $("#" + tid).addClass("invalidCol");
                                    $("#" + tid).css({ "background-color": "red", "color": "white" });
                                    $('#art1').addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
                                    $('#alertlabel').text("Alert ! Please rectify the quantity marked in red.");
                                    $('#art1').delay(5000).fadeOut('slow');
                                }
                                else {
                                    if (AllocatedQty != "") {

                                        $("#ajaxLoader").css("display", "block");
                                        $("#" + tid).removeClass("invalidCol");
                                        $("#" + tid).css({ "background": "white", "color": "black" });
                                        $.ajax({
                                            type: "POST",
                                            //url: "../WebApi/AllInventory.asmx/InsertDeliveryData",
                                            url: "/DeliveryInventory/InsertDeliveryData",

                                            data: JSON.stringify({ AllocatedQty: AllocatedQty, InvId: InvId, UID: UID, From: From, FromId: FromId, To: To, ToId: ToId, Id: Id, CheckID: CheckID, TransReqId: TransReqId, DeliId: DeliId, TransDeliId: TransDeliId }),
                                            contentType: "application/json; charset=utf-8",
                                            dataType: "json",
                                            success: function (response) {
                                                //  var getdata = response.d;
                                                var getdata = response;
                                                if (getdata > 0) {
                                                    $("#" + tempid).val(getdata);
                                                }
                                                $("#" + tid).css("border", "1px solid #9e9898");
                                                $("#ajaxLoader").css("display", "none");
                                                GetTotalTotalTonnage();
                                            }
                                        });
                                    }
                                }
                                var coun = parseInt(txtid[1]) + 1;
                                $("#txtqty_" + coun).select();
                            }

                            // $("#ajaxLoader").css("display", "none");
                        });

                    }
                });

                // scrolling Top when Click On Next Button

                table.on('page.dt', function () {
                    $('.dataTables_scrollBody').animate({
                        scrollTop: 0
                    }, 300);
                    // }, 'slow');
                });

            }
            $("#ajaxLoader").css("display", "none");
        }
    })

}



// Comment 18-May-20222 Start here
//function validation_ondeliveryconfirm() {
//    $.ajax({
//        type: "POST",
//        url: "/DeliveryInventory/getDeliverydata",
//        //data: JSON.stringify({ }),
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        success: function (response) {
//            var getgriddelivery = response;
//            if (getgriddelivery.length > 0) {
//                for (var colIndx = 0; i < getgriddelivery.length; colIndx++) {

//                    var tid = '#txtqty_' + colIndx;
//                    var txtid = tid.split('_');
//                    var AllocatedQtyWithCommas = $(this).parents().find('#txtqty_' + colIndx).val();
//                    var AllocatedQty = AllocatedQtyWithCommas.replace(",", "");
//                    var AvaQtyWithCommas = eve.currentTarget.parentNode.parentNode.childNodes[7].innerText;
//                    var AvaQty = AvaQtyWithCommas.replace(",", "");
//                    var ReqQty = '#txtReqqty_' + colIndx;

//                }
//            }
//        }
//            })
//}
// Comment 18-May-20222 End here

var count = 0;
var NumRows = 0;
$(document).ready(function () {
    var check = $("#hdncheckId").val();
    var fromcheck = $("#hdnfromcheck").val();
    //griddeliverydata(check);
    griddeliverydata(check, fromcheck);
    if ($("#getgridcount").val() != "") {
        NumRows = $("#getgridcount").val();
        //alert(NumRows);
    }

    $(".currentRow").on("click", function () {
        //alert();
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
            // window.location.href = '/InventoryManagement.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
            window.location.href = '/InventoryManagement/InventoryManagement?' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
        if (ClickAcnhorID == 'AnchorDelInventory') {
            $('#LoadingPanel1').css('display', 'none');
        }
        if (ClickAcnhorID == 'AnchorJobInventory') {
            //$('#LoadingPanel1').css('display', 'none');
            //  window.location.href = '/RequestInventory.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
            window.location.href = '/RequestInventory/RequestInventory?' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
        if (ClickAcnhorID == 'AnchorDepotInventory') {
            window.location.href = '/MainDepotInventory/MainDepotInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
        if (ClickAcnhorID == 'AnchorSiteInventory') {
            //  window.location.href = '/SiteInventory.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
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
function GetTotalTotalTonnage() {
    var ClientID = getParameterByName('id');
    var ReqId = $('#hdncheckId').val();
    $.ajax({
        type: "POST",
        url: "/DeliveryInventory/GetTotalTotalTonnage",
        data: JSON.stringify({ ClientID: ClientID, ReqId: ReqId }),
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

    location.reload();
}
//<%--
//Line Comment On 06 March

//function SaveChangesInGrids(sender, args) {
//    $('#ContentPlaceHolder1_hdnfield').val('1');
//    var grid = $find('<%=delivery.ClientID%>');
//    grid.get_batchEditingManager().saveChanges(grid.get_masterTableView());


//}
//function SaveChangesInGrid(sender, args) {
//    $('#ContentPlaceHolder1_hdnfield').val();
//    var grid = $find('<%=delivery.ClientID%>');
//    grid.get_batchEditingManager().saveChanges(grid.get_masterTableView());


//}--%>

function GetWhereFromWhereTo() {
    $('#LoadingPanel1').css('display', 'block');
    var ClientID = getParameterByName('id');
    //var ReqId = $('#hdncheckId').val();
    var ReqId = getParameterByName('checkid');
    var FromId = getParameterByName('FromId');
    var ToId = getParameterByName('ToId');
    var fromtocheck = getParameterByName('fromtocheck');
    var from = getParameterByName('from');
    var To = getParameterByName('To');

    $.ajax({
        type: "POST",
        url: "/DeliveryInventory/GetWhereToWhereFrom",
        data: JSON.stringify({ ClientID: ClientID, ReqId: ReqId, FromId: FromId, ToId: ToId, from: from, To: To, fromtocheck: fromtocheck }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            var WhereFromWhereTo = response;
            $('#txtfrom').val(WhereFromWhereTo.fromName);
            $('#txtto').val(WhereFromWhereTo.ToName);
        }
    })
    $('#LoadingPanel1').css('display', 'none');
}

function ckbox_CheckedChanged() {
    debugger;
    if ($('#ckbox').prop('checked') == true) {
        $('#chkpopup').css("display", "block");

    }

}
function chkpopupno_Click() {
    $('#chkpopup').css("display", "none");
    $('#ckbox').prop('checked', false);
}
function chkpopupyes_Click() {
    if ($('#ckbox').prop('checked') == true) {
        $('#chkpopup').css("display", "none");
        ChangeMaterialType();

    } else {
        $('#chkpopup').css("display", "none");
    }
}
function ChangeMaterialType() {
    debugger;

    var id = $("#hdnid").val();
    var UID = $("#hdnUID").val();
    var ReqId = $("#hdnReqId").val();
    var FID = $("#hdnfromid").val();
    var Pge = $("#hdnfrom").val();
    var TID = $("#hdnToId").val();
    var from = getParameterByName('from');
    var To = getParameterByName('To');
    var Material = $('#ddlMaterialType').val();
    $("#hdnmaterialTypeval").val(Material);
    var reqall = $("#ckbox").prop("checked");
    var txtrequesteddate = $("#txtrequesteddate").val();
    var txtrequireddate = $("#txtrequireddate").val();
    var ddlSupplier = $('#txtSupplier').val();
    var txtComment = $('#txtComment').val();
    var txtdesc = $("#txtdesc").val();

    var table = $('#deliverydata').DataTable();
    var totalPages = table.page.info().pages;


    processAllDeliveryPages(table); // ✅ Start saving from first page


}
function processAllDeliveryPages(table) {
    showFullScreenLoader(true);


    var totalPages = table.page.info().pages;
    var currentPage = 0;

    // Process one page at a time
    function processPageSequentially() {
        if (currentPage >= totalPages) {
            showFullScreenLoader(false);
            console.log("✅ All rows saved across all pages.");
            setTimeout(() => window.location.reload(), 1500);
            return;
        }

        // Navigate to current page
        table.page(currentPage).draw(false);
        console.log("Processing page:", currentPage + 1);

        setTimeout(() => {
            var rows = $('#deliverydata tbody tr');
            processRowSequentially(rows, 0, function () {
                currentPage++;
                // small delay between pages for AJAX cleanup
                setTimeout(processPageSequentially, 800);
            });
        }, 500); // Wait for DataTables redraw
    }

    // Process rows within one page sequentially
    function processRowSequentially(rows, index, callback) {
        if (index >= rows.length) {
            return callback(); // Done with this page
        }

        var row = $(rows[index]);
        var requestedQty = row.find('td:eq(8)').find('input[type="label"]').val()?.trim();
        var qtyInput = row.find('td:eq(9)').find('input[type="text"]');

        if (requestedQty && requestedQty !== "0") {
            qtyInput.val(requestedQty);
            row.css({ "background": "white", "color": "black" });

            var saveIcon = row.find('.rowSaveButton');
            if (saveIcon.length) {
                saveIcon.css("color", "rgb(11, 11, 11)");

                // Wait for AJAX completion before continuing
                saveRow(saveIcon, function () {
                    processRowSequentially(rows, index + 1, callback);
                });
                return; // exit early to wait for AJAX
            }
        }

        // Move to next row immediately if no save
        processRowSequentially(rows, index + 1, callback);
    }

    // Hook for AJAX-based save
    function saveRow(saveIcon, done) {
        // Wait for *this* AJAX to complete (not all)
        $(document).one('ajaxStop', function () {
            done();
        });
        saveIcon.trigger('click');
    }

    processPageSequentially();
}


function showFullScreenLoader(show) {
    if (show) {
        if ($('#blockScreenLoader').length === 0) {
            $('body').append(`
                <div id="blockScreenLoader" style="
                    position: fixed;
                    top: 0;
                    left: 0;
                    width: 100%;
                    height: 100%;
                    background: rgba(0, 0, 0, 0.4);
                    z-index: 9999;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    color: white;
                    font-size: 20px;
                    font-weight: bold;
                ">
                    <div>
                        <div class="spinner-border text-light" role="status"></div>
                        <div style="margin-top:10px;">Processing... Please wait</div>
                    </div>
                </div>
            `);
        }
        $('#blockScreenLoader').show();
    } else {
        $('#blockScreenLoader').hide();
    }
}

