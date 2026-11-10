

$(function () {
    var ClientID = getParameterByName('id');
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
            //window.location.href = '/InventoryManagement/InventoryManagement?' + Href[1];
            window.location.href = '/InventoryManagement/InventoryManagement?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
        if (ClickAcnhorID == 'AnchorSiteInventory') {
            //window.location.href = '/SiteInventory/SiteInventory?' + Href[1];
            window.location.href = '/SiteInventory/SiteInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
        if (ClickAcnhorID == 'AnchorJobInventory') {
            //window.location.href = '/RequestInventory/RequestInventory?' + Href[1];
            window.location.href = '/RequestInventory/RequestInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
        if (ClickAcnhorID == 'AnchorDepotInventory') {
            $('#LoadingPanel1').css('display', 'none');
        }
        if (ClickAcnhorID == 'AnchorReturnInventory') {
            //window.location.href = '/ReturnInventory/ReturnInventory?' + Href[1];
            window.location.href = '/ReturnInventory/ReturnInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
        if (ClickAcnhorID == 'AnchorDamagedInventory') {
            //window.location.href = '/DamagedInventory/DamagedInventory?' + Href[1];
            window.location.href = '/ReturnInventory/ReturnInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
        if (ClickAcnhorID == 'AnchorPipeLineInventory') {
            //window.location.href = '/ReturnInventory/ReturnInventory?' + Href[1];
            window.location.href = '/PipeLineInventory/PipeLineInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
    });
    DdlMaterialType();
    GetTotalTotalTonnage(ClientID, "All")
});


function btnreportClose() {
    $("#divInvoicePopup").css("display", "none");
    $("#divWeeklyPopup").css("display", "none");
    $("#divSAPCSVPopup").css("display", "none");
}

function GetDepotInventoryData(ClientID, MaterialType) {
    $("#LoadingPanel1").css("display", "block");

    var ClientID = getParameterByName('id');

    GetTotalTotalTonnage(ClientID,MaterialType);
    PipeInventoryHide();

    $.ajax({
        type: "POST",
        url: "/MainDepotInventory/GetDepotInventoryData",
        data: JSON.stringify({ ClientID: ClientID, MaterialType: MaterialType }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            var strTbodyTblSiteInventory = "";
            var DepotInventoryData = response;
            $('#tblSiteInventory').DataTable().clear();
            $('#tblSiteInventory').DataTable().destroy();
            $('#divSiteInventory').css("display", "block");
            if (DepotInventoryData.length > 0) {
                for (var i = 0; i < DepotInventoryData.length; i++) {
                    var ClientId = DepotInventoryData[i].ClientId == null ? "" : DepotInventoryData[i].ClientId;
                    var MaterialType = DepotInventoryData[i].MaterialType == null ? "" : DepotInventoryData[i].MaterialType;
                    var ItemCode = DepotInventoryData[i].ItemCode == null ? "" : DepotInventoryData[i].ItemCode;
                    var ItemDesc = DepotInventoryData[i].ItemDesc == null ? "" : DepotInventoryData[i].ItemDesc;
                    var OrgYardQuantity = DepotInventoryData[i].OrgYardQuantity == null ? "" : DepotInventoryData[i].OrgYardQuantity;
                    var QuantityToBeSent = DepotInventoryData[i].QuantityToBeSent == null ? "" : DepotInventoryData[i].QuantityToBeSent;

                    var QuantityReturned = DepotInventoryData[i].QuantityReturned == null ? "" : DepotInventoryData[i].QuantityReturned;
                    var AvailableQuantity = DepotInventoryData[i].AvailableQuantity == null ? "" : DepotInventoryData[i].AvailableQuantity;
                    var TotalDamagedQuentity = DepotInventoryData[i].TotalDamagedQuentity == null ? "" : DepotInventoryData[i].TotalDamagedQuentity;
                    var ItemWeight = DepotInventoryData[i].ItemWeight == null ? "" : DepotInventoryData[i].ItemWeight;
                    var TotalWeight = DepotInventoryData[i].TotalWeight == null ? "" : DepotInventoryData[i].TotalWeight;


                    strTbodyTblSiteInventory += "<tr>"
                    //strTbodyTblSiteInventory += "<td>" + ClientId + "</td>"
                    strTbodyTblSiteInventory += "<td style='display:none'></td>"
                    strTbodyTblSiteInventory += "<td>" + MaterialType + "</td>"
                    strTbodyTblSiteInventory += "<td>" + ItemCode + "</td>"




                    strTbodyTblSiteInventory += "<td>" + ItemDesc + "</td>"
                    strTbodyTblSiteInventory += "<td>" + numberWithCommas(OrgYardQuantity) + "</td>"
                    strTbodyTblSiteInventory += "<td>" + numberWithCommas(QuantityToBeSent) + "</td>"
                    strTbodyTblSiteInventory += "<td>" + numberWithCommas(QuantityReturned) + "</td>"
                    //strTbodyTblSiteInventory += "<td>" + numberWithCommas(JobQuantityDamaged) + "</td>"
                    //strTbodyTblSiteInventory += "<td>" + numberWithCommas(OrgYardAvailableWithDamage) + "</td>"
                    //if (flag == 1) {
                    //    strTbodyTblSiteInventory += "<td style='background-color:red'>" + numberWithCommas(AvailableQuantity) + "</td>"
                    //} else {
                    strTbodyTblSiteInventory += "<td>" + numberWithCommas(AvailableQuantity) + "</td>"
                    //}
                    strTbodyTblSiteInventory += "<td>" + numberWithCommas(TotalDamagedQuentity) + "</td>"
                    strTbodyTblSiteInventory += "<td>" + numberWithCommas(ItemWeight.toFixed(2)) + "</td>"
                    strTbodyTblSiteInventory += "<td>" + numberWithCommas(TotalWeight.toFixed(2)) + "</td>"

                    strTbodyTblSiteInventory += "</tr>"

                }

                $('#tbodyTblSiteInventory').append(strTbodyTblSiteInventory);

                $("#LoadingPanel1").css("display", "none");
            }
            else {
                // No Data Code here
                // $('#tblSiteInventory').css("display", "none");
                $('#tblSiteInventory').hide();
            }
            // $('#tblSiteInventory').css("display", "block");
            $('#tblSiteInventory').show();
            var table = $('#tblSiteInventory').DataTable({
                "bSort": false,
                "processing": true,
                "bLengthChange": false,
                "bInfo": false,
                searching: true,
                //autoWidth: false,

                scrollX: true,
                scrollCollapse: true,
                scrollY: "65vh",

                dom: 'Bfrtip',
                buttons: [
                    {
                        extend: 'csvHtml5',
                        title: 'Depot Inventory',
                                }
                    ],

            });
            var HireInventory = $('#hdnHireInventory').val();

            if (HireInventory == "False") {
                //table.column(13).visible(false);
                //table.column(14).visible(false);
                //table.column(15).visible(false);
            }
            $("#LoadingPanel1").css("display", "none");
        }
    })


}

function GetTotalTotalTonnage(ClientID, MaterialType) {
    $.ajax({
        type: "POST",
        url: "/MainDepotInventory/GetTotalTotalTonnage",

        data: JSON.stringify({ ClientID: ClientID, MaterialType: MaterialType }),
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

function btnprint_Click() {
    $("#LoadingPanel1").css("display", "block");
    var Href = window.location.href.split('?');
    var ClientID = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var MaterialType = $('#ddlMaterialType option:selected').val();
    window.open('/MainDepotInventoryReport/MainDepotInventoryReport?UID=' + UID + '&UserRole=' + UserRole + '&ClientId=' + ClientID + '&MaterialType=' + MaterialType);
    $("#LoadingPanel1").css("display", "none");


}

function ExportToCsvButton_Click() {
    $('.dt-button.buttons-csv.buttons-html5').trigger('click');
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
function DdlMaterialType() {
    var id = getParameterByName('id');
    $.ajax({
        type: "POST",
        url: "/MainDepotInventory/DdlMaterialType",
        data: JSON.stringify({ id: id }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {

            var ddlMaterialType = "<option value='All'>All</option>";
            $.each(response, function (idx, obj) {
                if (obj != null) {
                    ddlMaterialType += '<option value="' + obj.MaterialType + '">' + obj.MaterialType + '</option>';
                }
            });

            $('#ddlMaterialType').empty().append(ddlMaterialType);
        }
    })
}

function ddlMaterialType_SelectedIndexChanged() {
    var ClientID = getParameterByName('id');
    var MaterialType = $('#ddlMaterialType option:selected').val();
    GetTotalTotalTonnage(ClientID, MaterialType);
    GetDepotInventoryData(ClientID, MaterialType);
}