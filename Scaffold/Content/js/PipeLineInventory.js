$(function () {

    var Href = window.location.href.split('?');
    if (Href.length == 1) {
        $('#UlNavTabSeciton li').css('display', 'none');
        $('#liThirdParty').css('display', 'block');
    }
    $('#UlNavTabSeciton a').css('cursor', 'pointer');
    $('#UlNavTabSeciton a').click(function (e) {
        $('#LoadingPanel1').css('display', 'block');
        var ClickAcnhorID = e.currentTarget.id;
        var UID = getParameterByName('UID');
        var UserRole = getParameterByName('UserRole');
        var id = getParameterByName('id');

        if (ClickAcnhorID == 'AnchorYardInventory') {
            window.location.href = '/InventoryManagement/InventoryManagement?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
        if (ClickAcnhorID == 'AnchorSiteInventory') {
            window.location.href = '/SiteInventory/SiteInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
        if (ClickAcnhorID == 'AnchorJobInventory') {
            window.location.href = '/RequestInventory/RequestInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
        if (ClickAcnhorID == 'AnchorDepotInventory') {
            window.location.href = '/MainDepotInventory/MainDepotInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
        if (ClickAcnhorID == 'AnchorReturnInventory') {
            window.location.href = '/ReturnInventory/ReturnInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
        if (ClickAcnhorID == 'AnchorDamagedInventory') {
            window.location.href = '/ReturnInventory/ReturnInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
        if (ClickAcnhorID == 'AnchorPipeLineInventory') {
            $('#LoadingPanel1').css('display', 'block');
        }



    });

});

function SiteDropDownBind() {
    var ClientID = getParameterByName('id');
    var UID = getParameterByName('UID');
    var decUID = Decrypt(UID);
    $('#hid').val('@viewBag.hid');
    $.ajax({
        type: "POST",
        url: "/PipeLineInventory/SiteDropDown",
        data: JSON.stringify({ id: ClientID, UID: UID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            var SiteData = response;
            var SiteLocId = "";
            if (SiteData.length > 0) {
                for (var i = 0; i < SiteData.length; i++) {
                    SiteLocId = SiteData[i].SiteLocID;
                    if (SiteLocId == 0) {
                        SiteLocId = "";
                    }
                    $("#ddlSite").append("<option value='" + SiteLocId + "'>" + SiteData[i].SiteLocation + "</option>");
                }
            }
        }
    })
    GetPipeLineInventoryData();
}


function JobDropDown() {
    var ClientID = getParameterByName('id');
    var SiteId = $('#ddlSite').val();
   
    $.ajax({
        type: "POST",
        url: "/PipeLineInventory/GetJobDropDownBind",

        data: JSON.stringify({ id: ClientID, SiteId: SiteId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {

            var GetData = response;
            if (GetData.length > 0) {
                $("#ddlJob").empty();
                $("#ddlJob").append('<option value="">All </option>');
                for (var i = 0; i < GetData.length; i++) {
                    $("#ddlJob").append("<option value='" + GetData[i].RegisterId + "'>" + GetData[i].ScaffRef + "</option>");
                }
            }
        }
    });
}

function MaterialDropDown() {
    var ClientID = getParameterByName('id');
    var SiteId = $('#ddlSite').val();
    var JobId = $('#ddlJob').val();
    $.ajax({
        type: "POST",
        url: "/PipeLineInventory/GetMaterialDropDown",

        data: JSON.stringify({ id: ClientID, SiteId: SiteId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {

            var GetData = response;
            if (GetData.length > 0) {
                $("#ddlMaterial").empty();
                $("#ddlMaterial").append('<option value=""> </option>');
                for (var i = 0; i < GetData.length; i++) {
                    $("#ddlMaterial").append("<option value='" + GetData[i].MaterialType + "'>" + GetData[i].MaterialType + "</option>");
                }
            }
        }
    });
}


function AddNewRequest() {
    var ClientID = getParameterByName('id');
    var SiteId = $('#ddlSite').val();
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var JobId = $('#ddlJob').val();
    $.ajax({
        type: "POST",
        url: "/PipeLineInventory/AddNewRequest",
        data: JSON.stringify({
            id: ClientID, SiteId: SiteId, UID: UID, JobId: JobId, UserRole: UserRole
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            var ClientID = getParameterByName('id');
            var SiteId = $('#ddlSite').val();
            var UID = getParameterByName('UID');
            var UserRole = getParameterByName('UserRole');
            var JobId = $('#ddlJob').val();
            window.location.href = '/PipeLineInventory/PipeLineInventory?UID=' + UID + '&id=' + ClientID + '&UserRole=' + UserRole;
        }
    });
}

function PipelineDateDropDown() {
    var ClientID = getParameterByName('id');
    var SiteId = $('#ddlSite').val();
    var JobId = $('#ddlJob').val();
    $.ajax({
        type: "POST",
        url: "/PipeLineInventory/GetPipelineDateData",

        data: JSON.stringify({ id: ClientID, SiteId: SiteId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {

            var GetData = response;
            if (GetData.length > 0) {
                $("#ddlPipeline").empty();
                $("#ddlPipeline").append('<option value=""> </option>');
                for (var i = 0; i < GetData.length; i++) {
                    $("#ddlPipeline").append("<option value='" + DateFormat(GetData[i].PipelineStartDate) + "'>" + DateFormat(GetData[i].PipelineStartDate) + "</option>");
                }
            }
        }
    });
}
function GetTotalTotalTonnage(ClientID) {
    var ClientID = getParameterByName('id');
    var SiteId = $('#ddlSite').val();
    var JobID = $('#ddlJob').val();
    $.ajax({
        type: "POST",
        url: "/PipeLineInventory/GetTotalTotalTonnage",

        data: JSON.stringify({ ClientID: ClientID, SiteId: SiteId, JobID: JobID }),
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
function GetPipeLineInventoryData() {
    $("#LoadingPanel1").css("display", "block");

    var ClientID = getParameterByName('id');
    var SiteId = $('#ddlSite').val();
    var JobID = $('#ddlJob').val();
    var PipelineDate = $('#ddlPipeline').val();
    GetTotalTotalTonnage(ClientID);
    $.ajax({
        type: "POST",
        url: "/PipeLineInventory/GetPipeLineInventoryData",
        data: JSON.stringify({ ClientID: ClientID, SiteId: SiteId, JobID: JobID, PipelineDate: PipelineDate }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        //async: true,
        success: function (response) {
            var strTbodyTblPipeLineInventory = "";
            var PipeLineInventoryData = response;
            $('#tblPipeLineInventory').DataTable().clear();
            $('#tblPipeLineInventory').DataTable().destroy();
            $('#divSiteInventory').css("display", "block");
            if (PipeLineInventoryData.length > 0) {
                for (var i = 0; i < PipeLineInventoryData.length; i++) {
                    var ScaffRef = PipeLineInventoryData[i].ScaffRef == null ? "" : PipeLineInventoryData[i].ScaffRef;
                    var MaterialType = PipeLineInventoryData[i].MaterialType == null ? "" : PipeLineInventoryData[i].MaterialType;
                    var SiteLocation = PipeLineInventoryData[i].SiteLocation == null ? "" : PipeLineInventoryData[i].SiteLocation;
                    var ItemCode = PipeLineInventoryData[i].ItemCode == null ? "" : PipeLineInventoryData[i].ItemCode;
                    var ItemDesc = PipeLineInventoryData[i].ItemDesc == null ? "" : PipeLineInventoryData[i].ItemDesc;
                    //  var PipelineStartDate = PipeLineInventoryData[i].PipelineStartDate == null ? "" : PipeLineInventoryData[i].PipelineStartDate;
                    //  var PipeLineNote = PipeLineInventoryData[i].PipeLineNote == null ? "" : PipeLineInventoryData[i].PipeLineNote;
                    var Quantity = PipeLineInventoryData[i].Quantity == null ? "" : PipeLineInventoryData[i].Quantity;
                    var ItemWeight = PipeLineInventoryData[i].ItemWeight == null ? "" : PipeLineInventoryData[i].ItemWeight;
                    var TotalWeight = PipeLineInventoryData[i].TotalWeight == null ? "" : PipeLineInventoryData[i].TotalWeight;


                    strTbodyTblPipeLineInventory += "<tr>"
                    strTbodyTblPipeLineInventory += "<td style='display:none'></td>"
                    strTbodyTblPipeLineInventory += "<td>" + MaterialType + "</td>"
                    strTbodyTblPipeLineInventory += "<td>" + SiteLocation + "</td>"
                    strTbodyTblPipeLineInventory += "<td>" + ItemCode + "</td>"
                    strTbodyTblPipeLineInventory += "<td>" + ItemDesc + "</td>"
                    //  strTbodyTblPipeLineInventory += "<td>" + DateFormat(PipelineStartDate) + "</td>"
                    // strTbodyTblPipeLineInventory += "<td>" + PipeLineNote + "</td>"
                    strTbodyTblPipeLineInventory += "<td>" + numberWithCommas(Quantity) + "</td>"
                    strTbodyTblPipeLineInventory += "<td>" + numberWithCommas(ItemWeight.toFixed(2)) + "</td>"
                    strTbodyTblPipeLineInventory += "<td>" + numberWithCommas(TotalWeight.toFixed(2)) + "</td>"

                    strTbodyTblPipeLineInventory += "</tr>"

                }

                $('#tbodyTblPipeLineInventory').append(strTbodyTblPipeLineInventory);

            }
            else {
                $('#tblPipeLineInventory').hide();
            }
            $('#tblPipeLineInventory').show();
            var table = $('#tblPipeLineInventory').DataTable({
                "bSort": false,
                "processing": true,
                "bLengthChange": false,
                "bInfo": false,
                searching: true,

                scrollX: true,
                scrollCollapse: true,
                scrollY: "65vh",

                dom: 'Bfrtip',
                buttons: [

                    {
                        extend: 'csvHtml5',
                        title: 'Pipeline Inventory',
                    }
                ],

            });
            $("#LoadingPanel1").css("display", "none");
        }
    })
}

function ExportToCsvButton_Click() {
    $('.dt-button.buttons-csv.buttons-html5').trigger('click');
}
function AddNewRequest2() {
    $('#divConfirmReq').css("display", "Block");
}
function btncancel_Click() {
    $('#divConfirmReq').css("display", "none");
}

function btnbacktoPipelineJob_Click() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    var from = getParameterByName('from');
    var RID = getParameterByName('RID');
    // window.location.href = '/FloorLevelGallery/FloorLevelGallery?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid + '&from=' + from + '&RID=' + RID;
    window.location.href = '/ScaffoldMaterialListPhase3/ScaffoldMaterialListPhase3?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid + '&from=' + from + '&RID=' + RID;

    //Response.Redirect("FloorLevelGallery.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&from=" + Request.QueryString["from"] + "&RID=" + Request.QueryString["RID"]);
}


function GetPipelineStartDate() {
    var ClientID = getParameterByName('id');
    var SiteId = $('#ddlSite').val();
    var JobID = $('#ddlJob').val();
    $.ajax({
        type: "POST",
        url: "/PipeLineInventory/GetPipelineStartDate",
        data: JSON.stringify({ ClientID: ClientID, SiteId: SiteId, JobID: JobID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            var GetPipelineStartDate = response;

            $('#txtPipelineStartDate').val(GetPipelineStartDate.TotalItemsValues);
            $('#txtpipelineNotes').val(GetPipelineStartDate.PipelineDateValues).attr("title", GetPipelineStartDate.PipelineDateValues);
            $('#txtDescription').val(GetPipelineStartDate.PipelineDesc).attr("title", GetPipelineStartDate.PipelineDesc);

        }
    })
}



