
     $(document).ready(function () {

         var id = getParameterByName('id') == null ? "" : getParameterByName('id');
         var UID = getParameterByName('UID') == null ? "" : getParameterByName('UID');
         var from = getParameterByName('fromtocheck') == null ? "" : getParameterByName('fromtocheck');
         var FromId = getParameterByName('FromId') == null ? "" : getParameterByName('FromId');
         var t = getParameterByName('FromId');
         var ToId = getParameterByName('ToId') == null ? "" : getParameterByName('ToId');
         var To = getParameterByName('To') == null ? "" : getParameterByName('To');
         var RInvId = getParameterByName('RInvId') == null ? "" : getParameterByName('RInvId');
         var page = getParameterByName('page') == null ? "" : getParameterByName('page');

         var DecriptData = [id, UID, FromId, from, ToId, To, RInvId, page];
         var DecrData = Decrypt(DecriptData);
         id = DecrData[0];
         UID = DecrData[1];
         FromId = DecrData[2];
         from = DecrData[3];
         ToId = DecrData[4];
         To = DecrData[5];
         RInvId = DecrData[6];
         page = DecrData[7];

         $("#hdnid").val(id);
         $("#hdnUID").val(UID);
         $("#hdnfrom").val(from);
         $("#hdnfromid").val(FromId);
         if (To == "") {
             $("#hdnTo").val(0);
         } else {
             $("#hdnTo").val(To);
         }
         $("#hdnToId").val(ToId);
         if (RInvId == "") {
             $("#hdnRInvId").val(0);
         } else {
             $("#hdnRInvId").val(RInvId);
         }

         $("#hdnPage").val(page);

         if ($("#hdnRInvId").val() > 0) {
             $('#btnconfrminvtry').css('display', 'block');
         }
         else {
             $('#btnconfrminvtry').css('display', 'none');
         }
         
         if (from == "2") {
             $('#chkdisplay').css('display', 'block');
             $('#chkdisplay').css('display', 'inline-flex');
         }
         MaterialTupeDropDownBind();
     });

$(document).ready(function () {
    var UserRole = getParameterByName('UserRole');
    var DecUserRole = parseInt(Decrypt(UserRole));
    if (DecUserRole == 70 || DecUserRole == 75) {
        $('#AnchorYardInventory').css('display', 'none');
        $('#AnchorDepotInventory').css('display', 'none');
        $('#AnchorSiteInventory').css('display', 'none');
        $('#AnchorJobInventory').css('display', 'none');
        $('#AnchorDamagedInventory').css('display', 'none');

    }
    var currentDate = new Date();
    $('#txtreturndate').val(currentDate.DateMonthFormat());

    $("#txtreturndate").datepicker({
        dateFormat: 'dd-M-yy',
        // maxDate: 0,
        closeText: 'Close',

    });

    // $('#ContentPlaceHolder1_txtdesc').keydown(function (eve) {
    $('#txtdesc').keydown(function (eve) {
        if (eve.keyCode == 13) {
            return false;
        }
    })
});

$(document).ready(function () {
    //var id = $("#ContentPlaceHolder1_hdnid").val();
    //var RetId = $("#ContentPlaceHolder1_hdnRInvId").val();
    //var FID = $("#ContentPlaceHolder1_hdnfromid").val();
    //var Pge = $("#ContentPlaceHolder1_hdnfrom").val();
    //var TID = $("#ContentPlaceHolder1_hdnToId").val();
    //var returnall = $("#ContentPlaceHolder1_ckbox").prop("checked");

    var id = $("#hdnid").val();
    var RetId = $("#hdnRInvId").val();
    var FID = $("#hdnfromid").val();
    var Pge = $("#hdnfrom").val();
    var TID = $("#hdnToId").val();
    var returnall = $("#ckbox").prop("checked");


    //var MatType = $("#ContentPlaceHolder1_ddlMaterialType").children("option:selected").val();
    var MatType = $("#ddlMaterialType option:selected").val();
    if (MatType != "" && MatType != undefined) {
        datagrid(id, RetId, FID, TID, Pge, MatType, returnall);
    }
    else {
        datagrid(id, RetId, FID, TID, Pge, "All", returnall);
    }
});

function ckbox_CheckedChanged()
{
    if ($('#ckbox').prop('checked')==true)
    {
        if ($('#txtdesc').val() != "") {
            if ($('#hdnOptimizeStatus').val() == "Green") {
                $('#chkpopup').css("display", "block");
            } else {
                $('#PopupForNotOptimize').css("display", "block");
            }
        } else {
            $('#ckbox').prop('checked', false);
            var Response = { Result: "Failed", Message: "Please fill the Description." };
            CustomAlert(Response);
        }
      
    }

}
function PopupForNotOptimizeOK_Click() {
    $('#ckbox').prop('checked', false);
    $('#PopupForNotOptimize').css("display", "none");
}
function chkpopupyes_Click()
{
    if ($('#ckbox').prop('checked') == true)
    {
        $('#chkpopup').css("display", "none");
        ChangeMaterialType();
          
    } else {
        $('#chkpopup').css("display", "none");
    }
}
function chkpopupno_Click() {
    $('#chkpopup').css("display", "none");
    $('#ckbox').prop('checked', false);
}

function Back_Click() {
    var page = getParameterByName('page');
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var RID = getParameterByName('FromId');

    var DecriptData = [page];
    var DecrData = Decrypt(DecriptData);
    page = DecrData[0];
    if (page != "2") {
        window.location.href = '/ReturnInventory/ReturnInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;

        // Response.Redirect("ReturnInventory.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"]);
    }
    else {
        window.location.href = '/ScaffoldMaterialListPhase2/ScaffoldMaterialListPhase2?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + "&RID=" + RID;

        // Response.Redirect("ScaffoldMaterialListPhase2.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RID=" + Request.QueryString["FromId"]);
    }
}

function btnconfrminvtry_Click() {

    if ($('#txtdesc').val() != "") {

        var RInvId = getParameterByName('RInvId');
        var DecryptData = Decrypt([RInvId]);
        RInvId = DecryptData[0];
        if (RInvId != 0) {
            $('#lbltrandid').html(RInvId);
            $('#divConfirmReturn').css("display", "block");
        }
        else {
            var Response = { Result: "Failed", Message: "Please fill in Quantity to Return then click on Confirm." };
            CustomAlert(Response);
        }
    }
    else {
        var Response = { Result: "Failed", Message: "Please fill the Description." };
        CustomAlert(Response);
    }
}

function btnConfirmRetYes_Click() {

        var id = getParameterByName('id');
        var UID = getParameterByName('UID');
        var hdnRInvId = $('#hdnRInvId').val();
        $.ajax({
            type: "POST",
            url: "/ReturnInventoryDetails/btnconfrminvtry_Click",
            data: JSON.stringify({ id: id, hdnRInvId: hdnRInvId, UID: UID }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (response) {
                var Result = response;
                if (response > 0) {
                    $('#btnrefresh').css("display", "none");
                    $('#lockimage').css('display', 'block');
                    $('#btnconfrminvtry').css('display', 'none');
                    $('#hdntextdisable').val(true);
                    $('#art1').addClass('w3-section w3-card-12 j-alertBox-Success');
                    $('#art1').css("display", "block");
                    $('#alertlabel').html('<strong>Success ! </strong>Return confirmation saved successfully.');
                    //$("#btnredirection").click();
                    window.location.reload();
                } else {
                    $('#art1').addClass('w3-section w3-card-12 j-alertBox-Failed');
                    $('#art1').css("display", "block");
                    //Commented on (30-11-2023) error message was not correct
                    //$('#alertlabel').html('<strong>Alert ! </strong>Something want Error.');
                    $('#alertlabel').html('<strong>Alert ! </strong>Something Went Wrong .');
                }
            }
        })

}

function btnConfirmRetNo_Click() {
    $("#divConfirmReturn").css("display", "none");
}
//function btnconfrminvtry_Click() {
//    var id = getParameterByName('id');
//    var UID = getParameterByName('UID');
//    var hdnRInvId = $('#hdnRInvId').val();
//    $.ajax({
//        type: "POST",
//        url: "/ReturnInventoryDetails/btnconfrminvtry_Click",
//        data: JSON.stringify({ id: id, hdnRInvId: hdnRInvId, UID: UID }),
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        async: true,
//        success: function (response) {
//            var Result = response;
//            if (response>0) {
//                $('#lockimage').css('display', 'block');
//                $('#btnconfrminvtry').css('display', 'none');
//                $('#hdntextdisable').val(true);
//                $('#art1').addClass('w3-section w3-card-12 j-alertBox-Success');
//                $('#art1').css("display", "block");
//                $('#alertlabel').html('<strong>Success ! </strong>Return confirmation saved successfully.');
//                //$("#btnredirection").click();
//                window.location.reload();
//            } else {
//                $('#art1').addClass('w3-section w3-card-12 j-alertBox-Failed');
//                $('#art1').css("display", "block");
//                $('#alertlabel').html('<strong>Alert ! </strong>Something want Error.');
//            }
//        }
//    })
//}
function btnredirection_Click() {
    // if (hdnReturnid.Value != "") {
    if ($('#hdnReturnid').val() != "") {

        var id = getParameterByName('id');
        var UID = getParameterByName('UID');
        var UserRole = getParameterByName('UserRole');
        var fromtocheck = getParameterByName('fromtocheck');
        var FromId = getParameterByName('FromId');
        var ToId = getParameterByName('ToId');
        var To = getParameterByName('To');
        //  var RInvId = getParameterByName('RInvId');
        var page = getParameterByName('page');

        var RInvId = $('#hdnReturnid').val();

        var EncryptData = Encrypt([RInvId]);
        RInvId = EncryptData[0];

        window.location.href = '/ReturnInventoryDetails/ReturnInventoryDetails?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&fromtocheck=' + fromtocheck + '&FromId=' + FromId + '&ToId=' + ToId + '&RInvId=' + RInvId + '&page=' + page;

        // Session["GetMateType"] = hdnGetMaterialType.Value;
        // Response.Redirect("ReturnInventoryDetails.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&fromtocheck=" + Request.QueryString["fromtocheck"] + "&id=" + Request.QueryString["id"] + "&FromId=" + Request.QueryString["FromId"] + "&ToId=" + Request.QueryString["ToId"] + "&RInvId=" + databaseCon.Encrypt(hdnReturnid.Value) + "&page=" + Request.QueryString["page"]);


    }
}

//function MaterialTupeDropDownBind() {
//    var id = getParameterByName('id');
//    $.ajax({
//        type: "POST",
//        url: "/ReturnInventoryDetails/MaterialDropDown",
//        data: JSON.stringify({ id: id }),
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        async: true,
//        success: function (response) {

//            // var material = response.d;
//            var material = response;
//            if (material.length > 0) {
//                $("#ddlMaterialType").append('<option value="">All</option>');
//                for (var m = 0; m < material.length; m++) {
//                    $("#ddlMaterialType").append("<option value='" + material[m].MaterialType + "'>" + material[m].MaterialType + "</option>");

//                }
//            }
//        }
//    })
//}

function MaterialTupeDropDownBind() {
    var id = getParameterByName('id');
    $.ajax({
        type: "POST",
        url: "/ReturnInventoryDetails/MaterialDropDown",
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
    })
}

var returnTableData;
function numberWithCommas(x) {
    var parts = x.toString().split(".");
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join(".");
}
function datagrid(id, ReqInvid, FromID, ToId, Page, Material, returncheck) {
    $("#ajaxLoader").css("display", "block");
    $.ajax({
        type: "POST",
        // url: "../WebApi/AllInventory.asmx/reqgriddata",
        url: "/ReturnInventoryDetails/reqgriddata",
        data: JSON.stringify({ id: id, ReqInvid: ReqInvid, FromID: FromID, ToId: ToId, Page: Page, Material: Material, returncheck: returncheck }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            // var getdata = response.d;
            var getdata = response;
            var str = "";
            str += '<table id="dataforgrid" class="table table-striped" cellspacing="0" width="100%">';
            str += "<thead><tr>";
            str += "<th style='width:10%;text-align:center;display:none'>Trans ReqId</th>";
            str += "<th style='width:10%;text-align:center;display:none'>Inventory Id</th>";
            str += "<th style='width:10%;text-align:center'>Material Type</th>";
            str += "<th style='width:15%;text-align:center'>Item Code</th>";
            str += "<th style='width:25%;text-align:left;'>Item Description</th>";
            str += "<th style='width:10%;text-align:center'>Available Quantity</th>";
            str += "<th style='width:10%;text-align:center'>Return On This Docket</th>";
            str += "<th style='width:10%;text-align:center'>Return Quantity</th>";
            str += "<th style='width:10%;text-align:center;display:none'>Old Return Quantity</th>";
            str += "<th style='width:10%;text-align:center;'>#</th></tr></thead><tbody>";
            if (getdata.length > 0) {
                for (var i = 0; i < getdata.length; i++) {
                    var RIDetailID = getdata[i].RIDetailID == null ? "" : getdata[i].RIDetailID;
                    var InventoryId = getdata[i].InventoryId == null ? "" : getdata[i].InventoryId;
                    var MateralType = getdata[i].MaterialType == null ? "" : getdata[i].MaterialType;
                    var ItemCode = getdata[i].ItemCode == null ? "" : getdata[i].ItemCode;
                    var ItemDesc = getdata[i].ItemDesc == null ? "" : getdata[i].ItemDesc;
                    var AvailableQuantity = getdata[i].AvailableQuantity == null ? "" : getdata[i].AvailableQuantity;
                    //var ReturnQuantity = getdata[i].ReturnQuantity == null ? 0 : getdata[i].ReturnQuantity;
                    //var ReturnQuantity1 = getdata[i].ReturnQuantity1 == null ? 0 : getdata[i].ReturnQuantity1;
                    var ReturnQuantity = getdata[i].ReturnQuantity == null ? "" : getdata[i].ReturnQuantity;
                    var ReturnQuantity1 = getdata[i].ReturnQuantity1 == null ? "" : getdata[i].ReturnQuantity1;

                    str += "<tr><td style='width:10%;text-align:center;display:none' >" + RIDetailID + "</td>";
                    str += "<td style='width:10%;text-align:center;display:none' id='lblinventoryid'>" + InventoryId + "</td>";
                    str += "<td style='width:10%;text-align:center'>" + MateralType + "</td>";
                    str += "<td style='width:15%;text-align:center'>" + ItemCode + "</td>";
                    str += "<td style='width:25%;text-align:left;'>" + ItemDesc + "</td>";
                    str += "<td style='width:10%;text-align:center' id='Availableqty_" + i + "'>" + numberWithCommas(AvailableQuantity) + "</td>";
                    str += "<td style='width:10%;text-align:center' id='docketqty_" + i + "'>" + ReturnQuantity + "</td>";
                    //13-apr-2019-baans-change-start
                    // if ($("#ContentPlaceHolder1_hdntextdisable").val() == "true") {
                    if ($("#hdntextdisable").val() == "true") {
                        str += "<td style='width:10%;text-align:center;background-color:LightCyan;'> <input id='txtqty_" + i + "' type='text'  value='" + ReturnQuantity1 + "'  readonly='true' ><input id='hdnid_" + i + "' type='hidden' value='0' /></td>";
                        str += "<td style='width:10%;text-align:center;display:none;'>" + ReturnQuantity1 + "</td>";
                        str += "<td style='width:3%;'><a id='rowSave_" + i + "' class='fa fa-save rowSaveButton'></a></td></tr>";


                    }
                    else {
                        str += "<td style='width:10%;text-align:center;background-color:LightCyan;'> <input id='txtqty_" + i + "' type='text'  value='" + ReturnQuantity1 + "' ><input id='hdnid_" + i + "' type='hidden' value='0' /></td>";
                        str += "<td style='width:10%;text-align:center;display:none;'>" + ReturnQuantity1 + " </td>";
                        str += "<td style='width:3%;'><a id='rowSave_" + i + "' class='fa fa-save rowSaveButton'></a></td></tr>";
                    }
                    //str += "<td style='width:10%;text-align:center;background-color:LightCyan;'> <input id='txtqty_" + i + "' type='text' autocomplete='off' value=" + numberWithCommas(ReturnQuantity) + " ><input id='hdnid_" + i + "' type='hidden' value='0' /></td>";
                    //str += "<td style='width:10%;text-align:center;display:none;'>" + ReturnQuantity + "</td>";
                    //str += "<td style='width:3%;'><a id='rowSave_" + i + "' class='fa fa-save rowSaveButton'></a></td></tr>";
                    //13-apr-2019-baans-change-end
                }

                str += "</tbody></table>";
                $("#loaddata").html(str);
                returnTableData = $('#dataforgrid').DataTable({
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


                        $('input[type=text]').keydown(function (eve) {
                            if (eve.keyCode != 8) {
                                var value = eve.key
                                /*var regex = /^[-_!@@#$%^&*()a-zA-Z]+$/;*/
                                var regex = /^[_!@@#$%^&*()a-zA-Z]+$/;
                                if (value.match(regex)) {
                                    eve.preventDefault();
                                }
                                //if (eve.keyCode == 189) {   Commented Because Minus Button Not Working
                                //    eve.preventDefault();
                                //}

                            }

                        });
                        $('input[type=text]').keyup(function (eve) {
                            var txtqtyid = eve.currentTarget.id;
                            var colIndx = txtqtyid.split('_')[1]
                            var tid = '#txtqty_' + colIndx;
                            $(tid).css("border", "1px solid red");
                        });
                        var hdnsavedata;
                        var saveid;
                        $('.rowSaveButton').click(function (eve) {
                            eve.stopImmediatePropagation();  // Stop Calling Multiple Time When click search and insert the values

                            var txtid = "";
                            var tempid = "";
                            saveid = eve.currentTarget.id;
                            if (saveid != "") {
                                hdnsavedata = saveid;
                                if (hdnsavedata == saveid) {
                                    eve.preventDefault();
                                    var colIndx = hdnsavedata.split('_')[1]
                                    var colIndx1 = hdnsavedata.split('_')[1]
                                    var colIndx2 = hdnsavedata.split('_')[1]
                                    tid = '#txtqty_' + colIndx;
                                    tempid = '#hdnid_' + colIndx;
                                    txtid = tid.split('_');
                                    var qtyWithCommas = $(this).parents().find('#txtqty_' + colIndx).val();
                                    /*var qtyCommas = qtyWithCommas.replace(",", "");*/  //Commented on 17-May-2022
                                    var qty = qtyWithCommas.replace(/,/g, ''); // Added on 17-May-20222
                                    var qty1 = parseInt(qty);
                                    var docqtyWithCommas = $(this).parents().find('#docketqty_' + colIndx1).text();
                                    var Availableqtywithc = $(this).parents().find('#Availableqty_' + colIndx2).text();
                                    /*var strDocketQty = docqtyWithCommas.replace(",", "");*/ // Commented on 17-May-2022
                                    var strDocketQty = docqtyWithCommas.replace(/,/g,''); //Added on 17-May-2022
                                    /*var Availableqty = parseInt(Availableqtywithc.replace(",", ""));*/ // Commented on 17-May-2022
                                    var Availableqty = parseInt(Availableqtywithc.replace(/,/g,'')); //Added on 17-May-2022
                                    var DocketQty = parseInt(strDocketQty);
                                    var MinDocketQty = -DocketQty;
                                    var htmqty = parseInt(qty) + DocketQty;
                                    var finalavlqty = (Availableqty - (qty1));
                                    if (DocketQty != 0) {
                                        var qty = parseInt(qty) + DocketQty;
                                    }
                                    var tempval = $(tempid).val();
                                    var oldqty = 0;
                                    // var RowRInvId = eve.currentTarget.parentNode.parentNode.childNodes[0].innerText;
                                    var RowRInvId = eve.currentTarget.parentNode.parentNode.childNodes[0].innerText;
                                    if (RowRInvId == "") {
                                        RowRInvId = 0;
                                    }
                                    var inv = eve.currentTarget.parentNode.parentNode.childNodes[1].innerText;
                                    if (inv == "") {
                                        inv = 0;
                                    }
                                    var AvaQtyWithCommas = eve.currentTarget.parentNode.parentNode.childNodes[5].innerText;
                                    /*var AvQty = AvaQtyWithCommas.replace(",", "");*/ // Commented on 17-May-2022
                                    var AvQty = AvaQtyWithCommas.replace(/,/g,''); // Added on 17-May-2022
                                    var oldQtyWithCommas = eve.currentTarget.parentNode.parentNode.childNodes[7].innerText;
                                    // var oldQty = dmgQtyWithCommas.replace(",", "");
                                    /*var oldQty = oldQtyWithCommas.replace(",", "");*/ //Commented on 17-May-2022
                                    var oldQty = oldQtyWithCommas.replace(/,/g,''); //Added on 17-May-2022

                                    // 03-Jan-2020.
                                    if ($("#txtdesc").val() == "") {
                                        $('*').off('keyup keydown keypress');
                                    }
                                    else {
                                        if (isNaN(parseFloat(qty))) {
                                            qty = ('#txtqty_' + colIndx).val('');
                                        }
                                        /*if (qty != "") {*/
                                        if (qty != null) {

                                            $("#ajaxLoader").css("display", "block");
                                            //var id = $("#ContentPlaceHolder1_hdnid").val();
                                            //var from = $("#ContentPlaceHolder1_hdnfrom").val();
                                            //var fromid = $("#ContentPlaceHolder1_hdnfromid").val();
                                            //var TO = $("#ContentPlaceHolder1_hdnTo").val();
                                            //var ToId = $("#ContentPlaceHolder1_hdnToId").val();
                                            //var UID = $("#ContentPlaceHolder1_hdnUID").val();
                                            //var PRInvId = $("#ContentPlaceHolder1_hdnRInvId").val();
                                            //var Page = $("#ContentPlaceHolder1_hdnPage").val();
                                            //// 03-Jan-2020.
                                            //var txtdesc = $("#ContentPlaceHolder1_txtdesc").val();


                                            var id = $("#hdnid").val();
                                            var from = $("#hdnfrom").val();
                                            var fromid = $("#hdnfromid").val();
                                            var TO = $("#hdnTo").val();
                                            var ToId = $("#hdnToId").val();
                                            var UID = $("#hdnUID").val();
                                            var PRInvId = $("#hdnRInvId").val();
                                            var Page = $("#hdnPage").val();
                                            // 03-Jan-2020.
                                            var txtdesc = $("#txtdesc").val();



                                            //if (PRInvId == "0") {
                                            //    $('*').off('keyup keydown keypress');
                                            //}
                                            if (PRInvId == 0 && Page == "1" && parseInt(qty) > 0) {
                                                $('*').off('keyup keydown keypress');
                                            }
                                            else {
                                                $('*').on('keyup keydown keypress');
                                            }
                                            if (PRInvId == 0 && Page == "3" && parseInt(qty) > 0) {
                                                $('*').off('keyup keydown keypress');
                                            }
                                            else {
                                                $('*').on('keyup keydown keypress');
                                            }
                                            //if (parseInt(AvQty) >= parseInt(qty)) {
                                            //    if (parseInt(oldQty) <= parseInt(qty)) {
                                                    if (parseInt(AvQty) >= parseInt(qty1)) {
                                                        if (parseInt(qty1) >= parseInt(MinDocketQty)) {
                                                    $(tid).removeClass("invalidCol");
                                                    $.ajax({
                                                        type: "POST",
                                                        // url: "../WebApi/AllInventory.asmx/ReturnInsertion",
                                                        url: "/ReturnInventoryDetails/ReturnInsertion",
                                                        data: JSON.stringify({ id: id, from: from, fromid: fromid, TO: TO, ToId: ToId, UID: UID, RInvId: PRInvId, Page: Page, RowRInvId: RowRInvId, inv: inv, tempval: tempval, Quantity: qty, txtdesc: txtdesc }),
                                                        contentType: "application/json; charset=utf-8",
                                                        dataType: "json",
                                                        success: function (response) {
                                                            // var getdata = response.d;
                                                            var getdata = response;
                                                            $('#docketqty_' + colIndx1).html(htmqty);
                                                            $('#Availableqty_' + colIndx2).html(finalavlqty);
                                                            if (getdata > 0) {
                                                                if (PRInvId == "0") {
                                                                    //$("#ContentPlaceHolder1_hdnReturnid").val(getdata);
                                                                    // $("#ContentPlaceHolder1_btnredirection").click();
                                                                    $("#hdnReturnid").val(getdata);
                                                                    $("#btnredirection").click();
                                                                }
                                                                else {
                                                                    $(tempid).val(getdata);
                                                                    //  $("#ContentPlaceHolder1_hdnRid").val("");
                                                                    $("#hdnRid").val("");
                                                                    $("#ajaxLoader").css("display", "none");
                                                                }
                                                            }
                                                            else {
                                                                $("#ajaxLoader").css("display", "none");
                                                            }
                                                            $(tid).css("border", "1px solid #9e9898");
                                                        }
                                                    });
                                                }
                                                else {
                                                    $(tid).addClass("invalidCol");
                                                    $("#ajaxLoader").css("display", "none");
                                                    $('#art1').addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
                                                    $('#alertlabel').text("Alert ! You cannot reduce the quantity to a value more than the quantity on the Docket.");
                                                    $('#art1').delay(5000).fadeOut('slow');
                                                }
                                            }
                                            else {
                                                $(tid).addClass("invalidCol");
                                                $("#ajaxLoader").css("display", "none");
                                                $('#art1').addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
                                                $('#alertlabel').text("Alert ! Please rectify the quantity marked in red.");
                                                $('#art1').delay(5000).fadeOut('slow');
                                            }
                                            /*  $(tid).val(0);*/
                                            $(tid).val('');
                                        }
                                    }

                                }
                            }
                        });

                        $('input[type=text]').keydown(function (eve) {
                            eve.stopImmediatePropagation();  // Stop Calling Multiple Time When click search and insert the values

                            var txtid = "";
                            var tempid = "";
                            // 03 Jan 2020.
                            // if ($("#ContentPlaceHolder1_txtdesc").val() == "") {
                            if ($("#txtdesc").val() == "") {
                                if (eve.keyCode != 8) {
                                    eve.preventDefault();
                                    $('#art1').addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
                                    $('#alertlabel').text("Alert ! Please fill the Description.");
                                }
                            }
                            else {
                                if (eve.keyCode == 9 || eve.keyCode == 13) {
                                    eve.preventDefault();
                                    tid = eve.currentTarget.id;
                                    btid = tid;
                                    tempid = eve.currentTarget.nextSibling.id;
                                    txtid = tid.split('_');
                                    var colIndx = txtid[1];
                                    var qtyWithCommas = eve.currentTarget.value;
                                    /*var qtyr = qtyWithCommas.replace(",", "");*/ //Commented on 17-May-2022
                                    var qtyr = qtyWithCommas.replace(/,/g,''); //Added on 17-May-2022
                                    var qty = parseInt(qtyr);
                                    var qty1 = parseInt(qty);
                                    var docqtyWithCommas = eve.currentTarget.parentNode.parentNode.childNodes[6].innerText;
                                    var DocketQty = parseInt(docqtyWithCommas);//Added on 09-11-2021
                                    var MinDocketQty = -DocketQty;
                                    var htmqty = parseInt(qty) + DocketQty;

                                    if (DocketQty != 0) {
                                        var qty = parseInt(qty) + DocketQty;
                                    }
                                    var tempval = eve.currentTarget.nextSibling.value;
                                    var oldqty = 0;
                                    // var RowRInvId = eve.currentTarget.parentNode.parentNode.childNodes[0].innerText;
                                    // var inv = eve.currentTarget.parentNode.parentNode.childNodes[1].innerText;
                                    var RowRInvId = eve.currentTarget.parentNode.parentNode.childNodes[0].innerText;
                                    if (RowRInvId == "") {
                                        RowRInvId = 0;
                                    }
                                    var inv = eve.currentTarget.parentNode.parentNode.childNodes[1].innerText;
                                    if (inv == "") {
                                        inv = 0;
                                    }
                                    var AvaQtyWithCommas = eve.currentTarget.parentNode.parentNode.childNodes[5].innerText;
                                    /*var AvQty = AvaQtyWithCommas.replace(",", "");*/  //Commented  on 17-May-2022
                                    var AvQty = AvaQtyWithCommas.replace(/,/g,'');  //Added on 17-May-2022
                                    var finalavlqty = (parseInt(AvQty) - (qty1));
                                    var oldQtyWithCommas = eve.currentTarget.parentNode.parentNode.childNodes[7].innerText;
                                    // var oldQty = dmgQtyWithCommas.replace(",", "");
                                    /*var oldQty = oldQtyWithCommas.replace(",", "");*/ // Commented on 17-May-2022
                                    var oldQty = oldQtyWithCommas.replace(",", "");  // Added on 17-May-2022
                                    if (isNaN(parseFloat(qty))) {
                                        qty = ('#txtqty_' + colIndx).val('');
                                    }
                                    if (qty != null) {

                                        $("#ajaxLoader").css("display", "block");
                                        //var id = $("#ContentPlaceHolder1_hdnid").val();
                                        //var from = $("#ContentPlaceHolder1_hdnfrom").val();
                                        //var fromid = $("#ContentPlaceHolder1_hdnfromid").val();
                                        //var TO = $("#ContentPlaceHolder1_hdnTo").val();
                                        //var ToId = $("#ContentPlaceHolder1_hdnToId").val();
                                        //var UID = $("#ContentPlaceHolder1_hdnUID").val();
                                        //var PRInvId = $("#ContentPlaceHolder1_hdnRInvId").val();
                                        //var Page = $("#ContentPlaceHolder1_hdnPage").val();
                                        //var txtdesc = $("#ContentPlaceHolder1_txtdesc").val();

                                        var id = $("#hdnid").val();
                                        var from = $("#hdnfrom").val();
                                        var fromid = $("#hdnfromid").val();
                                        var TO = $("#hdnTo").val();
                                        var ToId = $("#hdnToId").val();
                                        var UID = $("#hdnUID").val();
                                        var PRInvId = $("#hdnRInvId").val();
                                        var Page = $("#hdnPage").val();
                                        var txtdesc = $("#txtdesc").val();

                                        if (PRInvId == 0 && Page == "1" && parseInt(qty) > 0) {
                                            $('*').off('keyup keydown keypress');
                                        }
                                        else {
                                            $('*').on('keyup keydown keypress');
                                        }
                                        if (PRInvId == 0 && Page == "3" && parseInt(qty) > 0) {
                                            $('*').off('keyup keydown keypress');
                                        }
                                        else {
                                            $('*').on('keyup keydown keypress');
                                        }

                                        //if (parseInt(AvQty) >= parseInt(qty1)) {
                                        //    if (parseInt(qty1) >= parseInt(MinDocketQty)) {

                                        if (parseInt(AvQty) >= parseInt(qty1)) {
                                            if (parseInt(qty1) >= parseInt(MinDocketQty)) {
                                                $("#" + tid).removeClass("invalidCol");
                                                $.ajax({
                                                    type: "POST",
                                                    // url: "../WebApi/AllInventory.asmx/ReturnInsertion",
                                                    url: "/ReturnInventoryDetails/ReturnInsertion",
                                                    data: JSON.stringify({ id: id, from: from, fromid: fromid, TO: TO, ToId: ToId, UID: UID, RInvId: PRInvId, Page: Page, RowRInvId: RowRInvId, inv: inv, tempval: tempval, Quantity: qty, txtdesc: txtdesc }),
                                                    contentType: "application/json; charset=utf-8",
                                                    dataType: "json",
                                                    success: function (response) {
                                                        //var getdata = response.d;
                                                        var getdata = response;
                                                        $('#docketqty_' + colIndx).html(htmqty);
                                                        $('#Availableqty_' + colIndx).html(finalavlqty);
                                                        if (getdata > 0) {
                                                            if (PRInvId == "0") {
                                                                //$("#ContentPlaceHolder1_hdnReturnid").val(getdata);
                                                                //$("#ContentPlaceHolder1_btnredirection").click();
                                                                $("#hdnReturnid").val(getdata);
                                                                $("#btnredirection").click();
                                                            }
                                                            else {
                                                                $("#" + tempid).val(getdata);
                                                                //$("#ContentPlaceHolder1_hdnRid").val("");
                                                                $("#hdnRid").val("");
                                                                $("#ajaxLoader").css("display", "none");
                                                            }
                                                        }
                                                        else {
                                                            $("#ajaxLoader").css("display", "none");
                                                        }
                                                        $("#" + tid).css("border", "1px solid #9e9898");
                                                    }
                                                });
                                            }
                                            else {
                                                $("#" + tid).addClass("invalidCol");
                                                $("#ajaxLoader").css("display", "none");
                                                $('#art1').addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
                                                $('#alertlabel').text("Alert ! You cannot reduce the quantity to a value more than the quantity on the Docket.");
                                                $('#art1').delay(5000).fadeOut('slow');
                                            }
                                        }
                                        else {
                                            $("#" + tid).addClass("invalidCol");
                                            $("#ajaxLoader").css("display", "none");
                                            $('#art1').addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
                                            $('#alertlabel').text("Alert ! Please rectify the quantity marked in red.");
                                            $('#art1').delay(5000).fadeOut('slow');
                                        }

                                    }
                                    var coun = parseInt(txtid[1]) + 1;
                                    $("#txtqty_" + coun).select();
                                    /* $('#' + btid).val(0);*/
                                    $('#' + btid).val('');
                                }
                            }
                        });
                    }


                });

                // scrolling Top when Click On Next Button

                returnTableData.on('page.dt', function () {
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
                    searching: false,
                    paging: false,
                    info: false,
                    scrollX: true,
                    scrollY: "60vh",
                    scrollCollapse: true,
                });
            }
            GetTotalTotalTonnage();
            $("#ajaxLoader").css("display", "none");

        }

    })
}


function ChangeMaterialType() {
    $("#ajaxLoader").css("display", "block");
    var id = $("#hdnid").val();
    var UID = $("#hdnUID").val();
    var RetId = $("#hdnRInvId").val();
    var FID = $("#hdnfromid").val();
    var Pge = $("#hdnfrom").val();
    var TID = $("#hdnToId").val();
    /*var MatType = $("#ddlMaterialType option:selected").val();*/
    var MatType = $('#ddlMaterialType').val();
    $("#hdnGetMaterialType").val(MatType);
    var returnall = $("#ckbox").prop("checked");
    // add code on 06-JAn 2020.
    var txtdesc = $("#txtdesc").val();
    if (returnall == true) {
        $.ajax({
            type: "POST",
            //url: "../WebApi/AllInventory.asmx/ReturnAllInsertion",
            url: "/ReturnInventoryDetails/ReturnAllInsertion",
            //data: JSON.stringify({ fromid: FID, ToId: TID, RInvId: RetId, returnall: returnall, UID: UID, id: id }),
            // add code on 06-JAn 2020.
            data: JSON.stringify({ fromid: FID, ToId: TID, RInvId: RetId, returnall: returnall, UID: UID, id: id, txtdesc: txtdesc }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                // var getdata = response.d;
                var getdata = response;
                if (getdata > 0) {
                    if (RetId == "0") {
                        //$("#ContentPlaceHolder1_hdnReturnid").val(getdata);
                        //$("#ContentPlaceHolder1_btnredirection").click();
                        $("#hdnReturnid").val(getdata);
                        $("#btnredirection").click();
                    }
                }
                else {
                    //$("#ContentPlaceHolder1_hdnReturnid").val(RetId);
                    //$("#ContentPlaceHolder1_btnredirection").click();
                    $("#hdnReturnid").val(RetId);
                    $("#btnredirection").click();
                }
            }
        })

    }
    else {
        datagrid(id, RetId, FID, TID, Pge, MatType, returnall);
    }
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

            window.location.href = '/MainDepotInventory/MainDepotInventory?' + Href[1];
        }
        if (ClickAcnhorID == 'AnchorJobInventory') {
            //  window.location.href = '/RequestInventory.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
            window.location.href = '/RequestInventory/RequestInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
        if (ClickAcnhorID == 'AnchorReturnInventory') {
            $('#LoadingPanel1').css('display', 'none');

        }
        if (ClickAcnhorID == 'AnchorSiteInventory') {
            //  window.location.href = '/SiteInventory.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
            window.location.href = '/SiteInventory/SiteInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
        if (ClickAcnhorID == 'AnchorDamagedInventory') {
            //  window.location.href = '/DamagedInventory.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
            window.location.href = '/DamagedInventory/DamagedInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
    });
});

function GetTotalTotalTonnage() {
    var ClientID = getParameterByName('id');
    var RetId = $("#hdnRInvId").val();

    $.ajax({
        type: "POST",
        url: "/ReturnInventoryDetails/GetTotalTotalTonnage",
        data: JSON.stringify({ ClientID: ClientID, RetId: RetId }),
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
    var RInvId = $("#hdnRInvId").val();
    var Desccription = $("#txtdesc").val();
    //location.reload();
    if (RInvId == "0") {
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
            url: "/ReturnInventoryDetails/updateDesc",
            data: JSON.stringify({ RInvId: RInvId, Desccription: Desccription }),
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
