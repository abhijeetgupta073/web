$(function () {
    var page = getParameterByName('page');

    var UserRole = getParameterByName('UserRole');
    var DecriptData = [page, UserRole];
    var DecrData = Decrypt(DecriptData);
    var pageID = DecrData[0];
    var DecrUserRole = DecrData[1];

    if (pageID == "3" && DecrUserRole == "10") {
        document.getElementById("chkdisplay").style.display = "block";
    }
    else {
        document.getElementById("chkdisplay").style.display = "none";

    }
    if (pageID == "1" || pageID == "3") {


    }
    else {
        $("#lblrequestdate").text("Date");
        $("#requiredDateDiv").hide();

    }

});

function hasAnyUnsavedChanges() {
    var totalRows = parseInt($("#hdntablelength").val()) || 0;
    for (var idx = 0; idx < totalRows; idx++) {
        var tid = '#txtqty_' + idx;
        var OldQtyval = '#OldQtyval_' + idx;
        if ($(tid).length == 0) { continue; }
        var qtyWithCommas = $(tid).val();
        var qty = qtyWithCommas ? qtyWithCommas.replace(/,/g, '') : "";
        var oldqtywithcommas = $(OldQtyval).val();
        var oldqty = oldqtywithcommas ? oldqtywithcommas.replace(/,/g, '') : "";
        if (oldqty == "") { oldqty = 0; }
        var qtyNum = qty === "" ? 0 : parseFloat(qty);
        var oldqtyNum = parseFloat(oldqty) || 0;
        if (qtyNum === oldqtyNum) {
            continue;
        }
        return true;
    }
    return false;
}

var pendingPageButton = null;
var pendingRefreshAction = false;

window.onbeforeunload = function (e) {
    if (hasAnyUnsavedChanges()) {
        var message = "You have unsaved inventory changes on this page.";
        e = e || window.event;
        if (e) {
            e.returnValue = message;
        }
        return message;
    }
};

$(document).on('submit', 'form', function () {
    window.onbeforeunload = null;
});

if (typeof window.__doPostBack === "function" && !window.__doPostBack.__unsavedGuardWrapped) {
    var __originalDoPostBack = window.__doPostBack;
    window.__doPostBack = function (eventTarget, eventArgument) {
        window.onbeforeunload = null;
        __originalDoPostBack(eventTarget, eventArgument);
    };
    window.__doPostBack.__unsavedGuardWrapped = true;
}

var __unsavedGuardRearmTimer = null;
$(document).ajaxSuccess(function (event, xhr, settings) {
    window.onbeforeunload = null;

    clearTimeout(__unsavedGuardRearmTimer);
    __unsavedGuardRearmTimer = setTimeout(function () {
        window.onbeforeunload = function (e) {
            if (hasAnyUnsavedChanges()) {
                var message = "You have unsaved inventory changes on this page.";
                e = e || window.event;
                if (e) { e.returnValue = message; }
                return message;
            }
        };
    }, 2000);
});

function ShowBackPopup() {
    if (!hasAnyUnsavedChanges()) {
        proceedWithoutSave();
        return;
    }
    $('#divUnsavedChanges').css("display", "block");
}

function saveAndContinue() {
    if ($("#txtdesc").val() == "") {
        $('#divUnsavedChanges').css("display", "none");
        var Response = { Result: "Failed", Message: "Please fill the Description." };
        CustomAlert(Response);
        return;
    }
    window.onbeforeunload = null;

    $('#divUnsavedChanges').css("display", "none");
    SaveAllChangedRows(function () {
        if (pendingRefreshAction) {
            pendingRefreshAction = false;
            doRefresh();
        } else if (pendingPageButton) {
            var btn = pendingPageButton;
            pendingPageButton = null;
            $(btn).trigger('click');
        } else {
            Back_Click();
        }
    });
}

function dontSaveAndContinue() {
    window.onbeforeunload = null;

    $('#divUnsavedChanges').css("display", "none");
    if (pendingRefreshAction) {
        pendingRefreshAction = false;
        clearUnsavedQtyInputs();
        doRefresh();
    } else if (pendingPageButton) {
        clearUnsavedQtyInputs();
        var btn = pendingPageButton;
        pendingPageButton = null;
        $(btn).trigger('click');
    } else {
        clearUnsavedQtyInputs();
        proceedWithoutSave();
    }
}

function clearUnsavedQtyInputs() {
    var totalRows = parseInt($("#hdntablelength").val()) || 0;
    for (var idx = 0; idx < totalRows; idx++) {
        var tid = '#txtqty_' + idx;
        var OldQtyval = '#OldQtyval_' + idx;
        if ($(tid).length == 0) { continue; }
        var oldqtywithcommas = $(OldQtyval).val();
        var oldqty = oldqtywithcommas ? oldqtywithcommas.replace(/,/g, '') : "";
        if (oldqty == "" || oldqty == "0" || oldqty == 0) {
            $(tid).val("");
        } else {
            $(tid).val(oldqtywithcommas);
        }
        $(tid).removeClass("invalidCol");
        $(tid).css({ "background": "", "color": "", "border": "" });
    }
}

function cancelLeave() {
    $('#divUnsavedChanges').css("display", "none");
    pendingPageButton = null;
    pendingRefreshAction = false;
    window.onbeforeunload = function (e) {
        if (hasAnyUnsavedChanges()) {
            var message = "You have unsaved inventory changes on this page.";
            e = e || window.event;
            if (e) { e.returnValue = message; }
            return message;
        }
    };
}

function Back_Click() {
    if (hasAnyUnsavedChanges()) {
        pendingRefreshAction = false;
        pendingPageButton = null;
        $('#divUnsavedChanges').css("display", "block");
        return;
    }
    window.onbeforeunload = null;
    proceedWithoutSave();
}
function proceedWithoutSave() {
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
    var txtdesc = $('#txtdesc').val();
    var txtComment = $('#txtComment').val();
    //var EncryptData = [page];
    //var encryData = Encrypt(EncryptData);
    //page = encryData[0];


    var DecriptData = [page];
    var DecrData = Decrypt(DecriptData);
    var pageID = DecrData[0];

    if (pageID == "1" || pageID == "3") {
        window.location.href = '/RequestInventory/RequestInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&fromtocheck=' + fromtocheck;
        // window.location.href = '/RequestInventory/RequestInventory?' + Href[1];
    }
    else if (pageID == "2") {
        if (txtdesc != "") {
            $.ajax({
                type: "POST",
                url: "/SiteRequest/InsertHistoricalJobMaterialListData",
                data: JSON.stringify({ txtdesc: txtdesc, UID: UID }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                success: function (response) {

                    // var material = response.d;
                    var Result = response.result;
                    if (Result == "Success") {
                        if (response.flagvalidQty == 0) {
                            //$('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
                            //$('#art1').css("display", "block");
                            //$('#alertlabel').text("Alert ! Please rectify the quantity marked in red.");
                            var Response = { Result: "Failed", Message: "Please rectify the quantity marked in red." };
                            CustomAlert(Response);
                        }
                        if (response.flagvalidQty == 1) {
                            $('#P1').css("display", "none");
                            getdatachange(true);
                        }
                    }
                    else if (Result == "Failed") {
                        $('#P1').css("display", "none");
                        //$('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
                        //$('#art1').css("display", "block");
                        //$('#alertlabel').text("Alert !" + response.Message);
                        getdatachange(false);
                    }
                }
            })
        } else {

            if ($('#txtTransId').val() == "") {
                var UID = getParameterByName('UID');
                var UserRole = getParameterByName('UserRole');
                var id = getParameterByName('id');
                var RID = getParameterByName('FromId');
                window.location.href = '/ScaffoldMaterialListPhase2/ScaffoldMaterialListPhase2?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID;
            } else {
                //$('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
                //$('#art1').css("display", "block");
                //$('#alertlabel').html('<strong>Alert ! </strong>Please fill the Description.');
                var Response = { Result: "Failed", Message: "Please fill the Description." };
                CustomAlert(Response);
            }
        }

    }
}

function btnconfirm_Click() {
    // if (txtdesc.Text != "")
    if ($('#txtdesc').val() != "") {
        // if (databaseCon.Decrypt(Request.QueryString["ReqId"]) != "0")
        var RequestID = getParameterByName('ReqId');
        var DecryptData = Decrypt([RequestID]);
        RequestID = DecryptData[0];
        if (RequestID != 0) {
            //P2.Style.Add("display", "none");
            //btncancel.Style.Add("display", "none");

            //lbltrandid.InnerText = "'" + databaseCon.Decrypt(Request.QueryString["ReqId"]) + "'";
            //lbldescCon.InnerText = "'" + txtdesc.Text.Replace("'", "''").Trim() + "'";
            // divConfirmReq.Style.Add("display", "block");

            $('#P2').css("display", "none");
            $('#btncancel').css("display", "block");
            //lbltrandid.InnerText = "'" + databaseCon.Decrypt(Request.QueryString["ReqId"]) + "'";
            //lbldescCon.InnerText = "'" + txtdesc.Text.Replace("'", "''").Trim() + "'";
            // $('#lbltrandid').val(RequestID);
            $('#lbltrandid').html(RequestID);
            $('#lbldescCon').html($('#txtdesc').val());
            $('#lblSupp').html($('#ddlSupplier option:selected').text());
            $('#divConfirmReq').css("display", "block");
        }
        else {
            //$('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
            //$('#art1').css("display", "block");
            //$('#alertlabel').html('<strong>Alert ! </strong>Please fill in Quantity to Request then click on Confirm Request.');
            var Response = { Result: "Failed", Message: "Please fill in Quantity to Request then click on Confirm Request." };
            CustomAlert(Response);
        }
    }
    else {
        //           $('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
        //$('#art1').css("display", "block");
        //$('#alertlabel').html('<strong>Alert ! </strong>Please fill the Description.');
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

    window.location.href = '/SiteRequest/SiteRequest?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&ReqId=' + ReqId + '&FromId=' + FromId + '&ToId=' + ToId + '&fromtocheck=' + fromtocheck + '&from=' + from + '&To=' + To + '&page=' + page;

}

$(document).ready(function () {
    $('#btnSaveAll').off('click').on('click', function (eve) {
        eve.preventDefault();
        SaveAllChangedRows();
    });
});
$(document).on('input', '[id^="txtqty_"]', function () {
    if ($(this).val() == "") {
        $(this).removeClass("invalidCol");
        $(this).css({ "background": "", "color": "", "border": "1px solid #9e9898" });
    }
});
//Added Ended on 08-Sep-2026

$(document).ready(function () {
    $('.dt-button buttons-csv buttons-html5').removeClass('rgExpCSV').addClass('btn w3-btn w3-teal').attr('value', 'Export to CSV').css('width', '110px');
    //$('#ContentPlaceHolder1_txtdesc').keydown(function (eve) {
    $('#txtdesc').keydown(function (eve) {
        if (eve.keyCode == 13) {
            return false;
        }
    })

    //if (Session["MaterialType"] != null) {
    //    ddlMaterialType.SelectedValue = Session["MaterialType"].ToString();
    //    Session["MaterialType"] = null;
    //}

    if ($('#hdnPage').val() == "1" || $('#hdnPage').val() == "3") {
        // Back.Style.Add("display", "none");
        //    lblnote.Style.Add("display", "block");
        //    ddlmat.Style.Add("display", "block");
        //    lbldesc.Style.Add("display", "block");
        //    lblJobMsg.Style.Add("display", "none");
        $('#Back').css("display", "none");
        $('#lblnote').css("display", "block");
        $('#ddlmat').css("display", "block");
        $('#lbldesc').css("display", "block");
        $('#lblJobMsg').css("display", "none");

    }
    else if ($('#hdnPage').val() == "2") {
        // btnbackSite.Style.Add("display", "none");
        //lblnote.Style.Add("display", "none");
        //    ddlmat.Style.Add("display", "block");
        //    lblJobDesc.Style.Add("display", "block");
        $('#btnbackSite').css("display", "none");
        $('#lblnote').css("display", "none");
        $('#ddlmat').css("display", "block");
        $('#lblJobDesc').css("display", "block");
        $('#jobdropdown').css("display", "block");
        $('#btnSaveAll').css("display", "block");

    }

    if ($('#hdnReqId').val() == "0") {
        // forinventory.Style.Add("display", "none");
        $('#forinventory').css("display", "none");
    } else {
        // forinventory.Style.Add("display", "block");
        // forjobinventory.Style.Add("display", "none");
        $('#forinventory').css("display", "block");
        $('#forjobinventory').css("display", "none");
        $('#txtTransId').val($('#hdnReqId').val());
        $('#btnSaveAll').css("display", "block"); 

    }
    //if (Request.QueryString["ReqStatus"] != null && databaseCon.Decrypt(Request.QueryString["ReqStatus"]) != "NotConfirmed" && Request.QueryString["ReqStatus"] != "")

    if (getParameterByName('ReqStatus') != null && $('#hdnconfirm').val() != "NotConfirmed" && getParameterByName('ReqStatus') != "") {
        //lockimage.Style.Add("display", "block");
        //hdntextdisable.Value = "true";
        //btnconfirm.Style.Add("display", "none");
        $('#lockimage').css("display", "block");
        $('#hdntextdisable').val(true);
        $('#btnconfirm').css("display", "none");
        $('#btnSupplier').css("display", "none");
        $('#btnrefresh').css("display", "none");
        $('#btnSaveAll').css("display", "none"); 

    }
    //if (Convert.ToInt32(databaseCon.Decrypt(Request.QueryString["fromtocheck"])) == 1)
    if ($('#hdnfromtocheck').val() == "1") {
        //lblto.Text = "To:";
        //lblfrom.Text = "From:";
        //jobdropdown.Style.Add("display", "none");

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
        /*$('#btnrefresh').css("display", "none");*/

        //$('#txtitemsHide').css("display", "none");
        //$('#txttotaltonnageHide').css("display", "none");
    }
    else {
        $('#lblto').text("To:");
        $('#lblfrom').text("From:");
        $('#jobdropdown').css("display", "none");
    }

});


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
            // window.location.href = '/InventoryManagement.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
            window.location.href = '/InventoryManagement/InventoryManagement?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
        //if (ClickAcnhorID == 'AnchorDelInventory') {
        //    window.location.href = '/DeliveryInventory.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        //}
        if (ClickAcnhorID == 'AnchorJobInventory') {
            $('#LoadingPanel1').css('display', 'none');
        }
        if (ClickAcnhorID == 'AnchorDepotInventory') {
            window.location.href = '/MainDepotInventory/MainDepotInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
        if (ClickAcnhorID == 'AnchorSiteInventory') {
            // window.location.href = '/SiteInventory.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
            window.location.href = '/SiteInventory/SiteInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
    });
});


//      function MaterialTupeDropDownBind()
//      {
//          var id = getParameterByName('id');
//          var page = getParameterByName('page');
//          var ToId = getParameterByName('ToId');
//          $.ajax({
//              type: "POST",
//              url: "/SiteRequest/MaterialDropDown",
//              data: JSON.stringify({ id: id, page: page, ToId: ToId }),
//              contentType: "application/json; charset=utf-8",
//              dataType: "json",
//              async: true,
//              success: function (response) {

//                  // var material = response.d;
//                  var material = response;
//                  if (material.length > 0) {
//                      $("#ddlMaterialType").append('<option value="">All</option>');
//                      for (var m = 0; m < material.length; m++) {
//                          $("#ddlMaterialType").append("<option value='" + material[m].MaterialType + "'>" + material[m].MaterialType + "</option>");

//                          }
//                  }
//              }
//          })
//}


function MaterialTupeDropDownBind() {
    var id = getParameterByName('id');
    var page = getParameterByName('page');
    var ToId = getParameterByName('ToId');
    $.ajax({
        type: "POST",
        url: "/SiteRequest/MaterialDropDown",
        data: JSON.stringify({ id: id, page: page, ToId: ToId }),
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
        //$('#ContentPlaceHolder1_art1').addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
        //$('#ContentPlaceHolder1_alertlabel').text("Alert! Please rectify the quantity marked in red.");
        $('#art1').addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
        $('#alertlabel').text("Alert! Please rectify the quantity marked in red.");
    }
    if (data == false) {
        //  $('#ContentPlaceHolder1_art1').addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
        //  $('#ContentPlaceHolder1_alertlabel').text("Alert! Please 'Click' on Save Icon for records that are highlighted in red.\n Then click on save and back Button.");
        $('#art1').addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
        $('#alertlabel').text("Alert! Please 'Click' on Save Icon for records that are highlighted in red.\n Then click on save and back Button.");
        $('#art1').delay(5000).fadeOut('slow');
        return false;
    }

}

function getdatachangestart() {
    var datatablelen = $("#hdntablelength").val();
    //var datatablelen = $('#dataforgrid tr').length;
    for (var i = 0; i < datatablelen; i++) {
        //var newQty = "#txtqty_" + i;
        //var newQtyval = $(newQty).val() == "" ? "0" : $(newQty).val();
        //var OldQty = "#OldQtyval_" + i;
        //var OldQtyval = $(OldQty).val();
        var newQty = "#txtqty_" + i;
        var newQtyval = $(newQty).val() == "" ? "0" : $(newQty).val();
        var OldQty = "#OldQtyval_" + i;
        var OldQtyval = $(OldQty).val() == "" ? "0" : $(OldQty).val();

        var Aval = "#hdnAvalid_" + i;
        var AvalQty = $(Aval).val();
        if (AvalQty != undefined) {
            AvalQty = AvalQty.replace(",", "");
        }

        if (newQtyval != OldQtyval) {
            if (parseInt(newQtyval) > parseInt(AvalQty)) {
                getmsg = false;
            }
            data = false;
            return false;
        }
    }
    data = true;
    getmsg = true;
    var pagedata = $("#hdngetpage").val();
    var msg = $("#hdngetMsg").val();
    var redirect = $("#hdnredirect").val();
    if (redirect == "false") {
        var UID = getParameterByName('UID');
        var UserRole = getParameterByName('UserRole');
        var id = getParameterByName('id');
        var RID = getParameterByName('FromId');
        $("#ajaxLoader").css("display", "block");
        // window.location.href = '/ScaffoldMaterialListPhase2.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID;
        window.location.href = '/ScaffoldMaterialListPhase2/ScaffoldMaterialListPhase2?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID;
        return false;
    }
    if (pagedata == "") {
        $("#divConfirmReq").css("display", "block");
    }

    $("#hdngetpage").val("");
}

function btnConfirmReqYes_Click(type) {
    if (type == "once") {
        if (isExecuted == true) {
            return;
        }
        isExecuted = true;
    }
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
    var txtComment = $('#txtComment').val();
    var hdnflagforjobinsertion = $("#hdnflagforjobinsertion").val();
    //var EncryptData = [page];
    //var encryData = Encrypt(EncryptData);
    //page = encryData[0];


    //var DecriptData = [page];
    //var DecrData = Decrypt(DecriptData);
    //var pageID = DecrData[0];

    $.ajax({
        type: "POST",
        url: "/SiteRequest/btnConfirmReqYes_Click",
        data: JSON.stringify({ id: id, UID: UID, ReqId: ReqId, page: page, txtdesc: txtdesc, UserRole: UserRole, hdnflagforjobinsertion: hdnflagforjobinsertion, FromId: FromId, txtComment: txtComment }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            var Result = response.result;
            if (Result == "Success") {
                if (response.Message == "RequestInventory") {
                    var id = getParameterByName('id');
                    var UID = getParameterByName('UID');
                    var UserRole = getParameterByName('UserRole');
                    var EmailFlag = "1";

                    // localStorage.setItem("EmailFlag", "1");                    
                    //window.location.href = '/RequestInventory/RequestInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&TransId=' + ReqId;
                    //window.location.href = '/RequestInventory/RequestInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&TransId=' + ReqId + '&EmailFlag=' + EmailFlag ;

                    window.location.href = '/RequestInventory/RequestInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&TransId=' + ReqId + '&EmailFlag=' + EmailFlag + '&To=' + To;


                } else if (response.Message == "ScaffoldMaterialListPhase2") {
                    // return RedirectToAction("ScaffoldMaterialListPhase2", "ScaffoldMaterialListPhase2", new { UID = UID, UserRole = UserRole, id = id, RID = FromId });
                    var id = getParameterByName('id');
                    var UID = getParameterByName('UID');
                    var UserRole = getParameterByName('UserRole');
                    var FromId = getParameterByName('FromId');
                    window.location.href = '/ScaffoldMaterialListPhase2/ScaffoldMaterialListPhase2?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + FromId;
                }

            }
            else if (Result == "Failed") {
                $('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
                $('#art1').css("display", "block");
                $('#alertlabel').text("Alert !" + response.Message);
                $("#divConfirmReq").css("display", "none");
            }
        }
    })
}


function btnConfirmReqNo_Click() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var id = getParameterByName('id');
    var RID = getParameterByName('FromId');
    var page = getParameterByName('page');

    var DecriptData = [page];
    var DecrData = Decrypt(DecriptData);
    page = DecrData[0];


    if (page == "1" || page == "3") {
        $("#divConfirmReq").css("display", "none");
        //divConfirmReq.Style.Add("display", "none");
    }
    //else if (databaseCon.Decrypt(Request.QueryString["page"]) == "2")
    else if (page == "2") {
        // Response.Redirect("ScaffoldMaterialListPhase2.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RID=" + Request.QueryString["FromId"]);

        window.location.href = '/ScaffoldMaterialListPhase2/ScaffoldMaterialListPhase2?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID;
    }
}
function btncancel_Click() {
    $("#divConfirmReq").css("display", "none");
}

function SaveAllChangedRows(callback, skipGridRefresh) {
    if ($("#txtdesc").val() == "") {
        $('#art1').addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
        $('#alertlabel').text("Alert ! Please fill the Description.");
        if (typeof callback === "function") { callback(); }
        return;
    }
    if (!hasAnyUnsavedChanges()) {
        $('#art1').removeClass("j-alertBox-Success").addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
        $('#alertlabel').text("Alert ! All values are already saved.");
        $('#art1').delay(3000).fadeOut('slow');
        if (typeof callback === "function") {
            callback();
        }
        return;
    }
    var totalRows = parseInt($("#hdntablelength").val()) || 0;
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
    var txtrequesteddate = $("#txtrequesteddate").val();
    var txtrequireddate = $("#txtrequireddate").val();
    var ddlSupplier = $("#ddlSupplier option:selected").text();
    var hasBlankQty = false;
    var hasExceededQty = false;

    var pendingSaveCalls = 0;
    var loopFinished = false;

    function finishSaveAllIfDone() {
        if (!loopFinished || pendingSaveCalls > 0) { return; }
        $("#ajaxLoader").css("display", "none");
        if (hasBlankQty && hasExceededQty) {
            $('#art1').removeClass("j-alertBox-Success").addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
            $('#alertlabel').text("Please enter quantity for the highlighted rows, and make sure quantity is NOT more than Available Quantity for rows marked in red.");
        } else if (hasExceededQty) {
            $('#art1').removeClass("j-alertBox-Success").addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
            $('#alertlabel').text("Alert! Please Make Sure Entered Quantity is NOT More Than Available Quantity for rows marked in red.");
        } else if (hasBlankQty) {
            $('#art1').removeClass("j-alertBox-Success").addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
            $('#alertlabel').text("Please enter quantity for the highlighted rows.");
        } else {
            $('#art1').removeClass("j-alertBox-Failed").addClass("w3-section w3-card-12 j-alertBox-Success").css('display', 'block');
            $('#alertlabel').text("All entered quantities have been saved.");
        }
        $('#art1').delay(3000).fadeOut('slow');

        //Updated on 08-Sep-2026 - don't auto-refresh the grid when the save came from the master search box
        if (!skipGridRefresh && (Page == "1" || Page == "3")) {
            MaterialChange();
        }

        if (typeof callback === "function") {
            callback();
        }
    }

    $("#ajaxLoader").css("display", "block");
    for (var idx = 0; idx < totalRows; idx++) {
        var tid = '#txtqty_' + idx;
        var tempid = '#hdnid_' + idx;
        var OldQtyval = '#OldQtyval_' + idx;
        if ($(tid).length == 0) { continue; }
        var qtyWithCommas = $(tid).val();
        var qty = qtyWithCommas ? qtyWithCommas.replace(/,/g, '') : "";
        var oldqtywithcommas = $(OldQtyval).val();
        var oldqty = oldqtywithcommas ? oldqtywithcommas.replace(/,/g, '') : "";
        if (oldqty == "") { oldqty = 0; }
        var qtyNumChk = qty === "" ? 0 : parseFloat(qty);
        var oldqtyNumChk = parseFloat(oldqty) || 0;
        if (qtyNumChk === oldqtyNumChk) {
            continue;
        }
        if (qty == "") {
            hasBlankQty = true;
            $(tid).removeClass("invalidCol");
            $(tid).css({ "background": "", "color": "", "border": "1px solid #9e9898" });
            continue;
        }
        var row = $(tid).closest('tr');
        var TReqId = row.find('td').eq(0).text();
        var inv = row.find('td').eq(1).text();
        var AvaQtyWithCommas = row.find('td').eq(5).text();
        var AvQty = AvaQtyWithCommas.replace(/,/g, '');
        var tempval = $(tempid).val();
        if (Page == "2" && parseInt(AvQty) < parseInt(qty)) {
            $(tid).addClass("invalidCol");
            hasExceededQty = true;
            continue;
        }
        $(tid).removeClass("invalidCol");
        pendingSaveCalls++;
        (function (tid, tempid, OldQtyval, qty, TReqId) {
            $.ajax({
                type: "POST",
                url: "/SiteRequest/insertion",
                async: true,
                data: JSON.stringify({ Qty: qty, OldQty: oldqty, InvId: inv, UID: UID, from: from, fromid: fromid, To: TO, ToId: ToId, id: id, TReqId: TReqId, ReqID: ReqId, Page: Page, tempval: tempval, txtdesc: txtdesc, Supplier: ddlSupplier, txtComment: txtComment, txtrequesteddate: txtrequesteddate, txtrequireddate: txtrequireddate }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var getdata = response;
                    if (getdata > 0) {
                        $(tempid).val(getdata);
                        $(OldQtyval).val(qty);
                        $(tid).css({ "background": "white", "color": "black" });
                        $(tid).css("border", "1px solid #9e9898");
                    }
                    pendingSaveCalls--;
                    finishSaveAllIfDone();
                },
                error: function () {
                    pendingSaveCalls--;
                    finishSaveAllIfDone();
                }
            });
        })(tid, tempid, OldQtyval, qty, TReqId);
    }
    loopFinished = true;
    finishSaveAllIfDone();
}
//Added Ended on 08-Sep-2026

var siteData;
function numberWithCommas(x) {
    var parts = x.toString().split(".");
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join(".");
}
var isMaterialChangeRunning = false;
function datagrid(id, Reqids, Material, Page, TOID, UID, Callfrom) {
    $("#ajaxLoader").css("display", "block");
    GetTotalTotalTonnage();
    $.ajax({
        type: "POST",
        async: false,
        // url: "../WebApi/AllInventory.asmx/griddata",
        url: "/SiteRequest/griddata",
        data: JSON.stringify({ id: parseInt(id), Reqstid: Reqids, Material: Material, Page: Page, ToID: TOID, UID: UID, Callfrom }),
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
            str += "<th style='width:15%;text-align:left'>Material Type</th>";
            str += "<th style='width:15%px;text-align:left'>Item Code</th>";
            str += "<th style='width:30%;text-align:left'>Item Description</th>";
            str += "<th style='width:10%;text-align:center'>Available Quantity</th>";
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
                    /* var Quantity = getdata[i].Quantity == null ? 0 : getdata[i].Quantity;*/
                    var Quantity = getdata[i].Quantity == null ? "" : getdata[i].Quantity;
                    var DisplayQty = Quantity == "" ? "" : numberWithCommas(Quantity);
                    str += "<tr><td style='width:10%;text-align:center;display:none' >" + TransReqId + "</td>";
                    str += "<td style='width:10%;text-align:center;display:none' id='lblinventoryid'>" + InventoryId + "</td>";
                    str += "<td style='width:15%;text-align:left'>" + MateralType + "</td>";
                    str += "<td style='width:15%;text-align:left'>" + ItemCode + "</td>";
                    str += "<td style='width:30%;text-align:left'>" + ItemDesc + "</td>";
                    str += "<td style='width:10%px;text-align:center'>" + numberWithCommas(AvailableQuantity) + "<input id='hdnAvalid_" + i + "' type='hidden' value=" + numberWithCommas(AvailableQuantity) + " /></td>";
                    // if ($("#ContentPlaceHolder1_hdntextdisable").val() == "true") {
                    if ($("#hdntextdisable").val() == "true") {
                        str += "<td style='width:10%;text-align:center;background-color:LightCyan;'> <input id='txtqty_" + i + "' type='text' value='" + DisplayQty + "' class='EnterQuantity' readonly='true' min='0'/><input id='hdnid_" + i + "' type='hidden' value='0' /></td>";

                        str += "<td style='width:3%;'><a id='rowSave_" + i + "' style='pointer-events: none; cursor: default;' class='fa fa-save rowSaveButton'></a></a><input id='OldQtyval_" + i + "' type='hidden' value='" + DisplayQty + "' /></td></tr>";
                    }
                    else {
                        var tr = "";
                        if (parseInt(AvailableQuantity) >= parseInt(Quantity || 0)) {
                            tr += "<td style='width:10%;text-align:center;background-color:LightCyan;'> <input id='txtqty_" + i + "' type='text' autocomplete='off' value='" + DisplayQty + "' class='EnterQuantity' min='0' /><input id='hdnid_" + i + "' type='hidden' value='0' /></td>";
                            tr += "<td style='width:3%;'><a id='rowSave_" + i + "' class='fa fa-save rowSaveButton'></a><input id='OldQtyval_" + i + "' type='hidden' value='" + DisplayQty + "' /></td></tr>";

                        }
                        else {
                            if (Page == 2) {
                                tr += "<td style='width:10%;text-align:center;background-color:LightCyan;'> <input id='txtqty_" + i + "' type='text' autocomplete='off' value='" + DisplayQty + "' style='background-color:red;color:white' class='EnterQuantity' min='0'/><input id='hdnid_" + i + "' type='hidden' value='0' /></td>";
                                /* tr += "<td style='width:3%;'><a id='rowSave_" + i + "' class='fa fa-save rowSaveButton'></a></a><input id='OldQtyval_" + i + "' type='hidden' value==" + numberWithCommas(Quantity) + " /></td></tr>";*/
                                //commented on 02july 2022 as = was coming with oldqty in ajax values
                                tr += "<td style='width:3%;'><a id='rowSave_" + i + "' class='fa fa-save rowSaveButton'></a></a><input id='OldQtyval_" + i + "' type='hidden' value='" + DisplayQty + "' /></td></tr>";
                            }
                            else {
                                tr += "<td style='width:10%;text-align:center;background-color:LightCyan;'> <input id='txtqty_" + i + "' type='text' autocomplete='off' value='" + DisplayQty + "' style='background-color:yellow' class='EnterQuantity' min='0'/><input id='hdnid_" + i + "' type='hidden' value='0' /></a><input id='OldQtyval_" + i + "' type='hidden' value='0' /></td>";
                                tr += "<td style='width:3%;'><a id='rowSave_" + i + "' class='fa fa-save rowSaveButton'></a></a><input id='OldQtyval_" + i + "' type='hidden' value='" + DisplayQty + "' /></td></tr>";
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
                    ////25 Feb 2019  no Shoted needed
                    //"ordering": true,
                    //"order":[[3,"asc"]],
                    //columnDefs: [{
                    //    orderable: false,
                    //    targets: "no-sort"
                    //}],
                    //scrollCollapse: true,
                    "drawCallback": function () {

                        $("input[type='search']").off('keyup').on('keyup', function (eve) {

                            var input = eve.currentTarget;

                            if (input.value == "") {

                                if (!isMaterialChangeRunning) {

                                    var cursorPos = input.selectionStart;
                                    window.searchCursorPos = cursorPos;

                                    if (hasAnyUnsavedChanges()) {
                                        $('#LoadingPanel1').show();
                                        SaveAllChangedRows(function () {
                                            MaterialChange();
                                        });
                                    } else {
                                        $('#LoadingPanel1').show();
                                        MaterialChange();
                                    }
                                } else {
                                    if (hasAnyUnsavedChanges()) {
                                        SaveAllChangedRows(function () { }, true); 
                                    }
                                    window.pendingMaterialChange = true;
                                }
                            }
                            else {
                                if (hasAnyUnsavedChanges()) {
                                    SaveAllChangedRows(function () { }, true);
                                }
                            }
                        });
                        $("input[type='search']").off('paste').on('paste', function (eve) {
                            if (hasAnyUnsavedChanges()) {
                                SaveAllChangedRows(function () { }, true);
                            }
                        });
                        $("input[type='search']").off('cut').on('cut', function (eve) {
                            if (hasAnyUnsavedChanges()) {
                                SaveAllChangedRows(function () { }, true);
                            }
                        });
                        //Added Ended on 08-Sep-2026
                        $('.EnterQuantity').off('keydown').on('keydown', function (eve) {
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
                        $(".EnterQuantity").off('keyup').on('keyup', function (eve) {
                            var txtAlocatid = eve.currentTarget.id;
                            var txtenterval = $("#" + txtAlocatid).val();
                            var colIndx = txtAlocatid.split('_')[1]
                            var tid = '#txtqty_' + colIndx;
                            var OldQty = '#OldQtyval_' + colIndx;
                            var OldQtyval = $(OldQty).val();
                            var oldqtywithcommas = $(this).parents().find('#OldQtyval_' + colIndx).val();
                            var oldqty = oldqtywithcommas.replace(/,/g, '');//Added on 17-May-2022
                            var Enterqty = eve.currentTarget.parentNode.parentNode.childNodes[6].childNodes[1].value;
                            var saveicon = '#rowSave_' + colIndx;
                            if (Enterqty == oldqty) {
                                // $(tid).css("border", "1px solid white");
                                $(tid).css("border", "1px solid Gray");
                            }
                            else {
                                $(tid).css("border", "1px solid red");
                                //$(saveicon).css("color", "red");
                            }
                        });
                        $(".EnterQuantity").off('keyup.avCheck').on('keyup.avCheck', function (eve) {
                            eve.stopImmediatePropagation();
                            var txtAlocatid = eve.currentTarget.id;
                            var txtenterval = $("#" + txtAlocatid).val();
                            var colIndx = txtAlocatid.split('_')[1]
                            var tid = '#txtqty_' + colIndx;
                            var OldQty = '#OldQtyval_' + colIndx;
                            //var AvQty = '#hdnAvalid_' + colIndx;
                            var AvQty = $(this).parents().find('#hdnAvalid_' + colIndx).val();
                            /*var AvalQty = AvQty.replace(",", "");*/ //Commented on 17-May-2022
                            var AvalQty = AvQty.replace(/,/g, '');//Added  on 17-May-2022
                            var OldQtyval = $(OldQty).val();
                            var EQty = eve.currentTarget.parentNode.parentNode.childNodes[6].childNodes[1].value;
                            /*var Enterqty = EQty.replace(",", "");*/  //Commented on 17-May-2022
                            var Enterqty = EQty.replace(/,/g, '');  //Added  on 17-May-2022
                            var saveicon = '#rowSave_' + colIndx;


                            if ((Page == "2") && parseInt(Enterqty) > parseInt(AvalQty)) {
                                $(tid).css({ "background": "Red", "color": "White" });
                                $('#art1').addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
                                $('#alertlabel').text("Alert! Please Make Sure Entered Quantity is NOT More Than Available Quantity.");
                                $('#art1').delay(5000).fadeOut('slow');
                                $("#" + saveid).css('color', 'rgb(11, 11, 11)');

                            } else {
                                $(tid).css({ "background": "white", "color": "black" });
                                $("#" + saveid).css('color', 'rgb(11, 11, 11)');

                            }

                        });

                        var hdnsavedata;
                        var saveid;
                        $('.rowSaveButton').off('click').on('click', function (eve) {
                            eve.stopImmediatePropagation();  // Stop Calling Multiple Time When click search and insert the values 05 April 2019
                            if ($("#txtdesc").val() == "") {
                                $('#art1').addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
                                $('#alertlabel').text("Alert ! Please fill the Description.");
                                $('#art1').delay(5000).fadeOut('slow');
                                return;
                            }
                            if ($("#hdnPage").val() == "2" || ($("#hdnReqId").val() != "0" && $("#hdnReqId").val() != 0)) {
                                eve.preventDefault();
                                eve.stopPropagation();
                                SaveAllChangedRows();
                                return;
                            }
                            //Added Ended on 08-Sep-2026
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
                                    /*var qty = qtyWithCommas.replace(",", "");*/ //Commented on 17-May-2022
                                    var qty = qtyWithCommas.replace(/,/g, ''); //Added on 17-May2022
                                    var tempval = $(tempid).val();
                                    //var oldqty = 0;
                                    var oldqtywithcommas = $(this).parents().find('#OldQtyval_' + colIndx).val();
                                    var oldqty = oldqtywithcommas.replace(/,/g, ''); //Added on 17-May-2022
                                    if (oldqty == "") {
                                        oldqty = 0;
                                    }
                                    var TReqId = eve.currentTarget.parentNode.parentNode.childNodes[0].innerText;
                                    var inv = eve.currentTarget.parentNode.parentNode.childNodes[1].innerText;
                                    var AvaQtyWithCommas = eve.currentTarget.parentNode.parentNode.childNodes[5].innerText;
                                    /*var AvQty = AvaQtyWithCommas.replace(",", ""); */ // Commneted on 17-May-2022
                                    var AvQty = AvaQtyWithCommas.replace(/,/g, ''); //Added on 17-May-2022
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
                                        // else if ((TReqId != "") || (TReqId == "" && qty != "" && qty != "0")) {
                                        else if ((TReqId != "") || (TReqId == "" && qty != "" && qty != "0") || (TReqId == "" && (qty == "" || qty == "0"))) {
                                            if (qty != "" || qty == "") {
                                                if (qty == "") {
                                                    qty = 0;
                                                }

                                                $("#ajaxLoader").css("display", "block");
                                                //var id = $("#ContentPlaceHolder1_hdnid").val();
                                                //var from = $("#ContentPlaceHolder1_hdnfrom").val();
                                                //var fromid = $("#ContentPlaceHolder1_hdnfromid").val();
                                                //var TO = $("#ContentPlaceHolder1_hdnTo").val();
                                                //var ToId = $("#ContentPlaceHolder1_hdnToId").val();
                                                //var UID = $("#ContentPlaceHolder1_hdnUID").val();
                                                //var ReqId = $("#ContentPlaceHolder1_hdnReqId").val();
                                                //var Page = $("#ContentPlaceHolder1_hdnPage").val();
                                                //var txtdesc = $("#ContentPlaceHolder1_txtdesc").val();

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
                                                var txtrequesteddate = $("#txtrequesteddate").val();
                                                var txtrequireddate = $("#txtrequireddate").val();

                                                var ddlSupplier = $("#ddlSupplier option:selected").text();

                                                if (ReqId == 0 && Page == "1" && parseInt(qty) > 0) {
                                                    $('*').off('keyup keydown keypress');
                                                }
                                                else {
                                                    $('*').on('keyup keydown keypress');
                                                }
                                                if (ReqId == 0 && Page == "3" && parseInt(qty) > 0) {
                                                    $('*').off('keyup keydown keypress');
                                                }
                                                else {
                                                    $('*').on('keyup keydown keypress');
                                                }

                                                if (Page == "2" && parseInt(AvQty) < parseInt(qty)) {
                                                    $(tid).addClass("invalidCol");
                                                    $("#ajaxLoader").css("display", "none");
                                                }
                                                else {
                                                    $(tid).removeClass("invalidCol");

                                                    $.ajax({
                                                        type: "POST",
                                                        // url: "../WebApi/AllInventory.asmx/insertion",
                                                        url: "/SiteRequest/insertion",
                                                        data: JSON.stringify({ Qty: qty, OldQty: oldqty, InvId: inv, UID: UID, from: from, fromid: fromid, To: TO, ToId: ToId, id: id, TReqId: TReqId, ReqID: ReqId, Page: Page, tempval: tempval, txtdesc: txtdesc, Supplier: ddlSupplier, txtComment: txtComment, txtrequesteddate: txtrequesteddate, txtrequireddate: txtrequireddate }),
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
                                                                        // $("#ContentPlaceHolder1_hdnRid").val("");
                                                                        $("#hdnRid").val("");
                                                                    }

                                                                    if ((Page == "1" || Page == "3") && parseInt(AvQty) < parseInt(qty)) {
                                                                        $(tid).css({ "background": "yellow", "color": "black" });
                                                                        $("#" + saveid).css('color', 'rgb(11, 11, 11)');
                                                                    } else {
                                                                        $(tid).css({ "background": "white", "color": "black" });
                                                                        $("#" + saveid).css('color', 'rgb(11, 11, 11)');
                                                                    }

                                                                    // if ($("#ContentPlaceHolder1_hdnRid").val() != "") {
                                                                    if ($("#hdnRid").val() != "") {
                                                                        var coun = parseInt(txtid[1]) + 1;
                                                                        var typeid = "txtqty_" + coun;
                                                                        //  $("#ContentPlaceHolder1_hdnprevid").val(typeid);
                                                                        //  $("#ContentPlaceHolder1_btnredirection").click();
                                                                        $("#hdnprevid").val(typeid);
                                                                        $("#btnredirection").click();
                                                                    }
                                                                    else {
                                                                        $("#ajaxLoader").css("display", "none");
                                                                    }

                                                                }
                                                                else {
                                                                    $(tempid).val(getdata);

                                                                    // $("#ContentPlaceHolder1_hdnflagforjobinsertion").val("true");
                                                                    $("#hdnflagforjobinsertion").val("true");
                                                                    $("#ajaxLoader").css("display", "none");
                                                                }

                                                            }
                                                            else {
                                                                if ((Page == "1" || Page == "3") && parseInt(AvQty) < parseInt(qty)) {
                                                                    $(tid).css({ "background": "yellow", "color": "black" });
                                                                    $("#" + saveid).css('color', 'rgb(11, 11, 11)');
                                                                } else {
                                                                    $(tid).css({ "background": "white", "color": "black" });
                                                                    $("#" + saveid).css('color', 'rgb(11, 11, 11)');
                                                                }
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
                        $('.EnterQuantity').off('keydown.nav').on('keydown.nav', function (eve) {
                            eve.stopImmediatePropagation();  // Stop Calling Multiple Time When click search and insert the values 05 April 2019

                            if ((eve.keyCode == 9 || eve.keyCode == 13) &&
                                ($("#hdnPage").val() == "2" || ($("#hdnReqId").val() != "0" && $("#hdnReqId").val() != 0))) {
                                eve.preventDefault();
                                eve.stopPropagation();

                                var tid = eve.currentTarget.id;
                                var ColIndx = tid.split('_')[1];
                                var oldqtywithcommas = $(this).parents().find('#OldQtyval_' + ColIndx).val();
                                var oldqty = oldqtywithcommas ? oldqtywithcommas.replace(/,/g, '') : "";
                                if (oldqty == "") { oldqty = 0; }
                                var qty = eve.currentTarget.value;

                                if (qty == "" || qty == oldqty) {
                                    $("#" + tid).css("border", "1px solid Gray");
                                } else {
                                    $("#" + tid).css("border", "1px solid red");
                                }

                                var coun = $("#" + tid).parent().parent().next('tr').find('.EnterQuantity').attr('id');
                                if (coun) {
                                    $("#" + coun).select();
                                }
                                return;
                            }
                            //Added Ended on 08-Sep-2026

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
                                //if ($("#ContentPlaceHolder1_hdntextdisable").val() != "true" && (eve.keyCode == 9 || eve.keyCode == 13)) {
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
                                    var oldqtywithcommas = $(this).parents().find('#OldQtyval_' + ColIndx).val();
                                    var oldqty = oldqtywithcommas.replace(/,/g, '');//Added on 17-May-2022
                                    if (oldqty == "") {
                                        oldqty = 0;
                                    }
                                    var TReqId = eve.currentTarget.parentNode.parentNode.childNodes[0].innerText;
                                    var inv = eve.currentTarget.parentNode.parentNode.childNodes[1].innerText;
                                    var AvaQtyWithCommas = eve.currentTarget.parentNode.parentNode.childNodes[5].innerText;
                                    /*var AvQty = AvaQtyWithCommas.replace(",", "");*///commnetd on 17-May-2022
                                    var AvQty = AvaQtyWithCommas.replace(/,/g, ''); //Added on 17-May-2022
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
                                                //var id = $("#ContentPlaceHolder1_hdnid").val();
                                                //var from = $("#ContentPlaceHolder1_hdnfrom").val();
                                                //var fromid = $("#ContentPlaceHolder1_hdnfromid").val();
                                                //var TO = $("#ContentPlaceHolder1_hdnTo").val();
                                                //var ToId = $("#ContentPlaceHolder1_hdnToId").val();
                                                //var UID = $("#ContentPlaceHolder1_hdnUID").val();
                                                //var ReqId = $("#ContentPlaceHolder1_hdnReqId").val();
                                                //var Page = $("#ContentPlaceHolder1_hdnPage").val();
                                                //var txtdesc = $("#ContentPlaceHolder1_txtdesc").val();

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
                                                var txtrequesteddate = $("#txtrequesteddate").val();
                                                var txtrequireddate = $("#txtrequireddate").val();
                                                var ddlSupplier = $("#ddlSupplier option:selected").text();

                                                //}

                                                if (ReqId == 0 && Page == "1" && parseInt(qty) > 0) {
                                                    $('*').off('keyup keydown keypress');
                                                }
                                                else {
                                                    $('*').on('keyup keydown keypress');
                                                }

                                                if (ReqId == 0 && Page == "3" && parseInt(qty) > 0) {
                                                    $('*').off('keyup keydown keypress');
                                                }
                                                else {
                                                    $('*').on('keyup keydown keypress');
                                                }

                                                if (Page == "2" && parseInt(AvQty) < parseInt(qty)) {
                                                    $("#" + tid).addClass("invalidCol");
                                                    $("#ajaxLoader").css("display", "none");
                                                }
                                                else {
                                                    $("#" + tid).removeClass("invalidCol");

                                                    $.ajax({
                                                        type: "POST",
                                                        // url: "../WebApi/AllInventory.asmx/insertion",
                                                        url: "/SiteRequest/insertion",
                                                        data: JSON.stringify({ Qty: qty, OldQty: oldqty, InvId: inv, UID: UID, from: from, fromid: fromid, To: TO, ToId: ToId, id: id, TReqId: TReqId, ReqID: ReqId, Page: Page, tempval: tempval, txtdesc: txtdesc, Supplier: ddlSupplier, txtComment: txtComment, txtrequesteddate: txtrequesteddate, txtrequireddate: txtrequireddate }),
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
                                                                        // $("#ContentPlaceHolder1_hdnRid").val(getdata);
                                                                        $("#hdnRid").val(getdata);
                                                                    }
                                                                    else {
                                                                        $("#" + tempid).val(getdata);
                                                                        // $("#ContentPlaceHolder1_hdnRid").val("");
                                                                        $("#hdnRid").val("");
                                                                    }

                                                                    if ((Page == "1" || Page == "3") && parseInt(AvQty) < parseInt(qty)) {
                                                                        $("#" + tid).css({ "background": "yellow", "color": "black" });
                                                                        $(saveid).css("color", "black");
                                                                    } else {
                                                                        $("#" + tid).css({ "background": "white", "color": "black" });
                                                                        $(saveid).css("color", "black");
                                                                    }

                                                                    //if ($("#ContentPlaceHolder1_hdnRid").val() != "") {
                                                                    if ($("#hdnRid").val() != "") {
                                                                        var coun = parseInt(txtid[1]) + 1;
                                                                        var typeid = "txtqty_" + coun;
                                                                        //$("#ContentPlaceHolder1_hdnprevid").val(typeid);
                                                                        //$("#ContentPlaceHolder1_btnredirection").click();
                                                                        $("#hdnprevid").val(typeid);
                                                                        $("#btnredirection").click();
                                                                    }
                                                                    else {
                                                                        $("#ajaxLoader").css("display", "none");
                                                                    }

                                                                }
                                                                else {
                                                                    $("#" + tempid).val(getdata);

                                                                    //$("#ContentPlaceHolder1_hdnflagforjobinsertion").val("true");
                                                                    $("#hdnflagforjobinsertion").val("true");
                                                                    $("#ajaxLoader").css("display", "none");
                                                                }

                                                            }
                                                            else {
                                                                if ((Page == "1" || Page == "3") && parseInt(AvQty) < parseInt(qty)) {
                                                                    $("#" + tid).css({ "background": "yellow", "color": "black" });
                                                                } else {
                                                                    $("#" + tid).css({ "background": "white", "color": "black" });
                                                                }
                                                                $("#ajaxLoader").css("display", "none");
                                                            }
                                                            $(OldQtyval).val(qty);
                                                            $("#" + tid).css("border", "1px solid #9e9898");
                                                            $(saveid).css("color", "black");
                                                        }
                                                    });
                                                }

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
                                        //$("#ContentPlaceHolder1_hdnOldQty").val(eve.currentTarget.defaultValue);
                                        $("#hdnOldQty").val(eve.currentTarget.defaultValue);
                                    }
                                }
                            }
                            //16-apr-2019-baans-start
                            //if ($("#ContentPlaceHolder1_hdntextdisable").val() == "true" && (eve.keyCode == 13)) {
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
            $('#LoadingPanel1').hide();
            isMaterialChangeRunning = false;
            if (window.pendingMaterialChange) {
                window.pendingMaterialChange = false;
                MaterialChange();
            }
            //Added Ended on 08-Sep-2026
            var searchInput = $("input[type='search']");
            searchInput.focus();
            if (searchInput.length > 0 && window.searchCursorPos != undefined) {
                searchInput[0].setSelectionRange(window.searchCursorPos, window.searchCursorPos);
            }
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
    if (isMaterialChangeRunning) return;

    isMaterialChangeRunning = true;

    $("#ajaxLoader").css("display", "block");
    //var id = $("#ContentPlaceHolder1_hdnid").val();
    //var RIDs = $("#ContentPlaceHolder1_hdnReqId").val();
    //var MatType = $("#ContentPlaceHolder1_ddlMaterialType").children("option:selected").val();
    //$("#ContentPlaceHolder1_hdnmaterialTypeval").val(MatType);
    //var page = $("#ContentPlaceHolder1_hdnPage").val();

    //var Toid = $("#ContentPlaceHolder1_hdnToId").val();
    //var UID = $("#ContentPlaceHolder1_hdnUID").val();
    var id = $("#hdnid").val();
    var RIDs = $("#hdnReqId").val();
    // var MatType = $("#ddlMaterialType").children("option:selected").val();
    /*var MatType = $('#ddlMaterialType option:selected').val();*/
    var MatType = $('#ddlMaterialType').val();
    $("#hdnmaterialTypeval").val(MatType);
    var page = $("#hdnPage").val();
    var Toid = $("#hdnToId").val();
    var UID = $("#hdnUID").val();

    datagrid(id, RIDs, MatType, page, Toid, UID, "");
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


    MaterialTupeDropDownBind();
    // dropdownSupplier();
    // $('#ddlSupplier').val('@Model.Supplier');
    // $('#ddlSupplier').val('@Model.Supplier');
    rowclick();
    hideAlert();
    var id = $("#hdnid").val();
    var RIDs = $("#hdnReqId").val();
    var page = $("#hdnPage").val();
    var Toid = $("#hdnToId").val();
    var UID = $("#hdnUID").val();
    // var MatType = $("#ddlMaterialType").children("option:selected").val()
    var MatType = $('#ddlMaterialType option:selected').val();
    if (MatType != "" && MatType != undefined) {
        datagrid(id, RIDs, MatType, page, Toid, UID, "");
    }
    else {
        datagrid(id, RIDs, "All", page, Toid, UID, "");
    }


});

function pagingclick() {
    $('.dataTables_wrapper').on('mousedown', '.paginate_button', function (eve) {
        $("#hdngetpage").val("1");
        $("#hdngetMsg").val("1");
        getdatachangestart();
    
        var Page = $("#hdnPage").val();
        if (Page == "2" && hasAnyUnsavedChanges()) {
            eve.stopImmediatePropagation();
            eve.preventDefault();
            pendingPageButton = this;
            $('#divUnsavedChanges').css("display", "block");
            return false;
        }
        if (data == false) {
            eve.stopImmediatePropagation();
            eve.preventDefault();
            pendingPageButton = this;
            $('#divUnsavedChanges').css("display", "block");
            return false;
            //Commented on 08-Sep-2026 - replaced by the unsaved-changes popup above
            //bootbox.alert({
            //    message: "Please Adjust qty, (If required), and then click on Save Icon for records that are highlighted in red.",
            //})
        }
        //Added Ended on 08-Sep-2026
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
function btnSelect_Click() {
    var Desccription = $("#txtdesc").val();
    if ($("#txtdesc").val() != "") {
        $("#myModal").css("display", "block");
        $('#txtdesc').css('border-color', '');

        GetHistoricalJobMaterialList();

    }
    else {
        $('#art1').addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
        $('#alertlabel').text("Please fill out The Description BEFORE adding Inventory Items ");
        $('#txtdesc').css('border-color', 'red');

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
        url: "/SiteRequest/GetHistoricalJobMaterialList",
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
                    if (OptimizeStatus != "Not Finalised") {
                        // Commented on (14-08-2023) because the i index was not working fine and we were facing issue in picking up the id from 2-3 page of grid when we use pageing in datatable property.
                        //str += "<td id='JobId_" + i + "' style='text-align:center;'><input type='Radio' name='JonSelected' class='ChkJobIdID' JobId_='" + JobId + "'/></td>"
                        str += "<td id='JobId_" + JobId + "' style='text-align:center;'><input type='Radio' name='JonSelected' class='ChkJobIdID' onclick = Checkjob(" + JobId + "); JobId_='" + JobId + "'/></td>"
                    }
                    else {
                        // Commented on (14-08-2023) because the i index was not working fine and we were facing issue in picking up the id from 2-3 page of grid when we use pageing in datatable property.
                        //str += "<td id='JobId_" + i + "' style='text-align:center;'><input type='Radio' name='JonSelected' disabled class='ChkJobIdID' JobId_='" + JobId + "'/></td>"
                        str += "<td id='JobId_" + JobId + "' style='text-align:center;'><input type='Radio' name='JonSelected' disabled class='ChkJobIdID' onclick = Checkjob(" + JobId + "); JobId_='" + JobId + "'/></td>"
                    }
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
                    "paging": true,
                    pageLength: 7,
                    "ordering": false,
                    "info": false

                });

                // Commented on (14 - 08 - 2023) We were having issue in picking up the id from second page of Historical Job Material List Data Grid.
                //$(".ChkJobIdID").click(function () {
                //    event.stopImmediatePropagation();
                //    var RegisterID = $(this).attr('JobId_').split('_')[0];
                //    var OptimizeStatus = $('#OptimizeStatus_' + RegisterID).text();
                //    var Scaffref = $('#Scaffref_' + RegisterID).text();
                //    //   var OptimizeStatus1 = $('#JobId_' + RegisterID).Val();
                //    $('#hdnlblOptimizeStatus').val(OptimizeStatus);
                //    $('#hdfieldId').val(RegisterID);
                //    $('#hdnScaffref').val(Scaffref);
                //    $('#hdnWoNumber').val(WoNumber);
                //});
                // Commented Ended on (14 - 08 - 2023) We were having issue in picking up the id from second page of Historical Job Material List Data Grid.

            } else {
                // No Data Code here
                $('#tblSiteJobGrid').css("display", "none");
                $('#HistoricalMessage').css("display", "none");
                $('#NoJobHeader').css("display", "block");
            }
        }
    })

}

// Added On (14-08-2023) We were having issue in picking up the id from second page of Historical Job Material List Data Grid. 
function Checkjob(JobId) {
    event.stopImmediatePropagation();
    var RegisterID = JobId;
    var OptimizeStatus = $('#OptimizeStatus_' + JobId).text();
    var Scaffref = $('#Scaffref_' + JobId).text();
    var WoNumber = $('#WoNumber_' + JobId).text();
    $('#hdnlblOptimizeStatus').val(OptimizeStatus);
    $('#hdfieldId').val(RegisterID);
    $('#hdnScaffref').val(Scaffref);
    $('#hdnWoNumber').val(WoNumber);
};
// Add Ended On (14-08-2023) We were having issue in picking up the id from second page of Historical Job Material List Data Grid.

function btnSelectJob_Click() {
    // if (lblOptimizeStatus.Text == "Not Finalised") {
    if ($('#hdnlblOptimizeStatus').val() == "Not Finalised") {
        //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
        //art1.Style.Add("display", "block");
        //alertlabel.Text = "<strong>Failed ! </strong>Selected job material list is not finalised.";
    }
    else {
        // hdfieldId.Value = JobId;
        // ddljob.SelectedValue = hdfieldId.Value;
        //   ddljob.Text = lblScaffReff.Text;
        // myModal.Style.Add("display", "none");
        $('#ddljob').val($('#hdnScaffref').val());
    }
    jobchange();
}

function jobchange() {
    //var jobid = $("#ContentPlaceHolder1_hdfieldId").val();
    //var jobtext = $("#ctl00_ContentPlaceHolder1_ddljob_Input").val();
    //var id = $("#ContentPlaceHolder1_hdnid").val();
    //var Toid = $("#ContentPlaceHolder1_hdnToId").val();
    //var UID = $("#ContentPlaceHolder1_hdnUID").val();
    //var fromid = $("#ContentPlaceHolder1_hdnfromid").val();

    var jobid = $("#hdfieldId").val();
    var jobtext = $("#ddljob").val();
    var id = $("#hdnid").val();
    var Toid = $("#hdnToId").val();
    var UID = $("#hdnUID").val();
    var fromid = $("#hdnfromid").val();
    $("#ajaxLoader").css("display", "block");

    //Added on 18-12-2023 to insert in temp table at time of Historical Copy.
    var txtdesc = $("#txtdesc").val();
    var txtComment = $("#txtComment").val();

    $.ajax({
        type: "POST",
        // url: "../WebApi/AllInventory.asmx/SetJobData",
        url: "/SiteRequest/SetJobData",
        data: JSON.stringify({ UID: UID, id: id, Toid: Toid, jobid: jobid, fromid: fromid, txtdesc: txtdesc, txtComment: txtComment }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var Result = response;
            // if (parseInt(response.d) > 0) {
            if (parseInt(response) > 0) {
                GetAvailableMaterial();

                //Commented On (04-03-2023) Added To Show data having 0 Quantity.
                //$("#hdnflagforjobinsertion").val("true");
                //var MatType = "All";
                //var page = $("#hdnPage").val();
                //var id = $("#hdnid").val();
                //var Toid = $("#hdnToId").val();
                //var UID = $("#hdnUID").val();
                //datagrid(id, "0", MatType, page, Toid, UID);
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
//Commented on 08-Sep-2026 - replaced below with unsaved-changes-guarded version, same as ported guard system
//function btnrefresh_Click() {
//    var ReqId = $("#hdnReqId").val();
//    var Desccription = $("#txtdesc").val();
//    var txtrequesteddate = $("#txtrequesteddate").val();
//    var txtrequireddate = $("#txtrequireddate").val();
//    var txtComment = $("#txtComment").val();
//    var UID = getParameterByName('UID');//Added on 18-July-2022
//    var Page = $("#hdnPage").val();
//
//    location.reload();
//    if (Desccription == "") {
//        var Response = { Result: "Failed", Message: "At least one quantity should be filled to save description." };
//        CustomAlert(Response);
//    }
//    else
//        if (Desccription == "") {
//            var Response = { Result: "Failed", Message: "Please fill Description." };
//            CustomAlert(Response);
//        }
//        else {
//            $.ajax({
//                type: "POST",
//                // url: "../WebApi/AllInventory.asmx/SetJobData",
//                url: "/SiteRequest/updateDesc",
//                data: JSON.stringify({ ReqId: ReqId, Desccription: Desccription, Page: Page, txtComment: txtComment, txtrequesteddate: txtrequesteddate, txtrequireddate: txtrequireddate, UID: UID }),
//                contentType: "application/json; charset=utf-8",
//                dataType: "json",
//                async: true,
//                success: function (response) {
//                    var Result = response;
//                }
//            })
//            location.reload();
//        }
//}
//Commented Ended on 08-Sep-2026


function btnrefresh_Click() {
    if (hasAnyUnsavedChanges()) {
        pendingRefreshAction = true;
        pendingPageButton = null;
        $('#divUnsavedChanges').css("display", "block");
        return;
    }
    doRefresh();
}

function doRefresh() {
    var ReqId = $("#hdnReqId").val();
    var Desccription = $("#txtdesc").val();
    var txtrequesteddate = $("#txtrequesteddate").val();
    var txtrequireddate = $("#txtrequireddate").val();
    var txtComment = $("#txtComment").val();
    var UID = getParameterByName('UID');
    var Page = $("#hdnPage").val();

    if (Desccription == "") {
        var Response = { Result: "Failed", Message: "Please fill Description." };
        CustomAlert(Response);
        return;
    }
    $.ajax({
        type: "POST",
        url: "/SiteRequest/updateDesc",
        data: JSON.stringify({ ReqId: ReqId, Desccription: Desccription, Page: Page, txtComment: txtComment, txtrequesteddate: txtrequesteddate, txtrequireddate: txtrequireddate, UID: UID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            location.reload();
        },
        error: function () {
            location.reload();
        }
    });
}
//Added Ended on 08-Sep-2026

//Added on (04-03-2023) to show the Material Type Data in pop-up that is having Available Quantity zero. 
function GetAvailableMaterial() {
    var id = $("#hdnid").val();
    var Toid = $("#hdnToId").val();
    var UID = $("#hdnUID").val();
    $.ajax({
        type: "POST",
        url: "/SiteRequest/GetAvailableMaterial",
        data: JSON.stringify({ UID: UID, id: id, Toid: Toid }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var GetData = response;
            var str = "";
            if (GetData.length != 0) {
                $("#MyMaterialAvailableModal").css("display", "block");
                $('#tblMaterialAvailableGrid').DataTable().clear();
                $('#tblMaterialAvailableGrid').DataTable().destroy();
                if (GetData.length > 0) {
                    for (var i = 0; i < GetData.length; i++) {
                        var InventoryId = GetData[i].InventoryId == null ? "" : GetData[i].InventoryId;
                        var MateralType = GetData[i].MaterialType == null ? "" : GetData[i].MaterialType;
                        var ItemCode = GetData[i].ItemCode == null ? "" : GetData[i].ItemCode;
                        var ItemDesc = GetData[i].ItemDesc == null ? "" : GetData[i].ItemDesc;
                        var Quantity = GetData[i].Quantity == null ? "" : GetData[i].Quantity;
                        var AvailableQuantity = GetData[i].AvailableQuantity == null ? "" : GetData[i].AvailableQuantity;

                        str += "<tr>"
                        str += "<td style='display:none;'>" + InventoryId + "</td>"
                        str += "<td>" + MateralType + "</td>"
                        str += "<td>" + ItemCode + "</td>"
                        str += "<td>" + ItemDesc + " </td>"
                        str += "<td>" + Quantity + "</td>";
                        str += "<td>" + AvailableQuantity + "</td></tr>";
                    }
                    $('#tblMaterialAvailableGrid').append(str);
                }
                table = $('#tblMaterialAvailableGrid').DataTable({
                    "bSort": false,
                    "processing": true,
                    searching: true,
                    "bLengthChange": true,
                    autoWidth: true,
                    dom: 'Bfrtip',
                    buttons: [
                        {
                            extend: 'csvHtml5',
                            title: 'Material List',
                            exportOptions: {
                                columns: [1, 2, 3, 4, 5],
                            }
                        }

                    ],
                });
            }
            else {
                $("#hdnflagforjobinsertion").val("true");
                var MatType = "All";
                var page = $("#hdnPage").val();
                var id = $("#hdnid").val();
                var Toid = $("#hdnToId").val();
                var UID = $("#hdnUID").val();
                datagrid(id, "0", MatType, page, Toid, UID, "");
            }
        }
    })
}

function GetTotalTotalTonnage() {
    var ClientID = getParameterByName('id');
    var ReqId = $("#hdnReqId").val();
    var JobId = $("#hdnfromid").val();
    var UID = $("#hdnUID").val();
    $.ajax({
        type: "POST",
        url: "/SiteRequest/GetTotalTotalTonnage",
        data: JSON.stringify({ ClientID: ClientID, ReqId: ReqId, JobId: JobId, UID: UID }),
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

function btnCANCELMaterial_Click() {
    var UID = $("#hdnUID").val();
    $.ajax({
        type: "POST",
        url: "/SiteRequest/btnCANCELMaterial_Click",
        data: JSON.stringify({ UID: UID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            if (response.Result == "Success") {
                $("#MyMaterialAvailableModal").css("display", "none");
            }
        }
    })
}

function btnCONTINUEMaterial_Click() {
    var Callfrom = "Continueclick";
    $("#MyMaterialAvailableModal").css("display", "none");
    $("#hdnflagforjobinsertion").val("true");
    var MatType = "All";
    var page = $("#hdnPage").val();
    var id = $("#hdnid").val();
    var Toid = $("#hdnToId").val();
    var UID = $("#hdnUID").val();
    datagrid(id, "0", MatType, page, Toid, UID, Callfrom);
}

function ExportToCsvButton_Click() {
    $('.dt-button.buttons-csv.buttons-html5').trigger('click');
}

function ckbox_CheckedChanged() {
    debugger;
    if ($('#ckbox').prop('checked') == true) {
        if ($('#txtdesc').val() != "") {
            $('#chkpopup').css("display", "block");
        } else {
            $('#ckbox').prop('checked', false);
            var Response = { Result: "Failed", Message: "Please fill the Description." };
            CustomAlert(Response);
        }

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

    var table = $('#dataforgrid').DataTable();
    var totalPages = table.page.info().pages;

    if ($('#txtTransId').val() == "") {

        $('#ckbox').prop('checked', false);
        var Response = { Result: "Failed", Message: "Please fill quantity for first row to Request all quantities" };
        CustomAlert(Response);


    } else {
        console.log("Total pages:", totalPages);
        processAllPages(table); 

        $('#btnconfirm').css("display", "block");
    }

}

function processAllPages(table) {
    showFullScreenLoader(true);

    var totalPages = table.page.info().pages;
    var currentPage = 0;

    function processPageSequentially() {
        if (currentPage >= totalPages) {
            showFullScreenLoader(false);
            console.log("All rows saved across all pages.");
            setTimeout(() => window.location.reload(), 1500);
            return;
        }

        table.page(currentPage).draw(false);
        console.log("Processing page:", currentPage + 1);

        setTimeout(() => {
            var rows = $('#dataforgrid tbody tr');
            processRowSequentially(rows, 0, function () {
                currentPage++;
                setTimeout(processPageSequentially, 800);
            });
        }, 500); 
    }

    function processRowSequentially(rows, index, callback) {
        if (index >= rows.length) {
            return callback(); 
        }

        var row = $(rows[index]);
        var availableQty = row.find('td:eq(5)').text().replace(/,/g, '').trim();
        var qtyInput = row.find('input[type="text"]');

        if (availableQty !== "" && availableQty !== "0") {
            qtyInput.val(availableQty);
            row.css({ "background": "white", "color": "black" });
            var saveIcon = row.find('.rowSaveButton');
            saveIcon.css("color", "rgb(11, 11, 11)");
            saveRow(saveIcon, function () {
                processRowSequentially(rows, index + 1, callback);
            });
        } else {
            processRowSequentially(rows, index + 1, callback);
        }
    }

    function saveRow(saveIcon, done) {
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