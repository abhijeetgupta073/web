 $(document).ajaxStart(function () {
        $('#LoadingPanel1').show();
    });
    $(document).ajaxStop(function () {
        $('#LoadingPanel1').hide();
    });

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
            // window.location.href = '/InventoryManagement.aspx?' + Href[1];
            //window.location.href = '/InventoryManagement/InventoryManagement?' + Href[1];
            window.location.href = '/InventoryManagement/InventoryManagement?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
        if (ClickAcnhorID == 'AnchorSiteInventory') {
            $('#LoadingPanel1').css('display', 'none');
        }
        if (ClickAcnhorID == 'AnchorJobInventory') {
            // window.location.href = '/RequestInventory.aspx?' + Href[1];
            //window.location.href = '/RequestInventory/RequestInventory?' + Href[1];
            window.location.href = '/RequestInventory/RequestInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
        if (ClickAcnhorID == 'AnchorDepotInventory') {
            //window.location.href = '/MainDepotInventory/MainDepotInventory?' + Href[1];
            window.location.href = '/MainDepotInventory/MainDepotInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
        if (ClickAcnhorID == 'AnchorReturnInventory') {
            // window.location.href = '/ReturnInventory.aspx?' + Href[1];
            //window.location.href = '/ReturnInventory/ReturnInventory?' + Href[1];
            window.location.href = '/ReturnInventory/ReturnInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
        if (ClickAcnhorID == 'AnchorDamagedInventory') {
            //// window.location.href = '/DamagedInventory.aspx?' + Href[1];
            //window.location.href = '/DamagedInventory/DamagedInventory?' + Href[1];
            window.location.href = '/DamagedInventory/DamagedInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
        if (ClickAcnhorID == 'AnchorPipeLineInventory') {
            //window.location.href = '/ReturnInventory/ReturnInventory?' + Href[1];
            window.location.href = '/PipeLineInventory/PipeLineInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
    });
});
// add 19cot 2022 by s.sourabh
$(document).ready(function () {
    DdlMaterialType(id)
});
//$(document).ready(function () {
//    //    $("#ulalwaysactive").css("display", "none");
//    var screensize = $(window).width();
//    if (screensize < 1150 && screensize > 650) {
//        ($BODY.hasClass('nav-md'))
//        $('#CreateNewDropDown').css("margin-top", "2px");
//        $BODY.removeClass('nav-md').addClass('nav-sm');
//        $LEFT_COL.removeClass('scroll-view');
//        //$('#BlackSection').css('display', 'block');
//        $('#imgLogo').parent().css('display', 'none');
//        $('#imgLogo1').css('display', 'block').css("height", "38px");
//        $('#leftbarsection').css('margin-top', '0px');
//    }
//    else {
//        if (screensize < (650)) {
//            $BODY.removeClass('nav-sm')
//            $('#imgLogo').parent().css('display', 'none');
//            $('#imgLogo1').css('display', 'block').css("height", "52px");
//            $('#btnsave').css('width', '130%').css("font-size", "8px");
//            $('#btncancel').css('width', '144%').css("font-size", "8px");
//            $('#ExportTo').css('width', '118%').css("font-size", "8px").css('margin-left', '7px');
//            $('.xsfieldset').css('margin-left', '-22px').css("width", "100%");
//            $('#btnconfirmDelvry').css('display', 'none !important');
//            $('#btnConfirmD').css('display', 'block').css('font-size', '10px !important').css('width', '100%').css('padding-left', '0px');
//        }
//        else {
//            $('#CreateNewDropDown').css("margin-top", "6px");
//            $BODY.removeClass('nav-sm').addClass('nav-md');
//            //$('#BlackSection').css('display', 'none');
//            $('#imgLogo').parent().css('display', 'block');
//            $('#imgLogo1').css('display', 'none');
//            $('#leftbarsection').css('margin-top', '40px');
//        }
//    }
//});
function btnreportClose() {
    //$("#ContentPlaceHolder1_divInvoicePopup").css("display", "none");
    //$("#ContentPlaceHolder1_divWeeklyPopup").css("display", "none");
    //$("#ContentPlaceHolder1_divSAPCSVPopup").css("display", "none");
    $("#divInvoicePopup").css("display", "none");
    $("#divWeeklyPopup").css("display", "none");
    $("#divSAPCSVPopup").css("display", "none");
    $("#divInvoiceSurgePopup").css("display", "none");
}
function OpenSAPCSVReport() {
    $("#LoadingPanel1").css("display", "block");
    // var siteID = $('#<%=ddlSite.ClientID %> option:selected').val();
    var siteID = $('#ddlSite option:selected').val();
    // if (siteID != "" && siteID != null) {
    if (siteID != "Select Site") {
        SapGenerateLastGenerated();
        // $("#ContentPlaceHolder1_divInvoicePopup").css("display", "block");
        $("#divSAPCSVPopup").css("display", "block");
        $("#LoadingPanel1").css("display", "none");
    }
    else {
        //$('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
        // $('#art1').css("display", "block");
        // $('#alertlabel').html('<strong>Alert ! </strong>Please Select the Site.');
        var Response = { Result: "Failed", Message: "Please Select the Site." };
        CustomAlert(Response);
        $("#LoadingPanel1").css("display", "none");
    }
}
function OpenInvoiceReport() {
    $("#LoadingPanel1").css("display", "block");
    // var siteID = $('#<%=ddlSite.ClientID %> option:selected').val();
    var siteID = $('#ddlSite option:selected').val();
    // if (siteID != "" && siteID != null) {
    if (siteID != "Select Site") {
        // $("#ContentPlaceHolder1_divInvoicePopup").css("display", "block");
        $("#divInvoicePopup").css("display", "block");
        $("#LoadingPanel1").css("display", "none");
    }
    else {
        //$('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
        // $('#art1').css("display", "block");
        // $('#alertlabel').html('<strong>Alert ! </strong>Please Select the Site.');
        var Response = { Result: "Failed", Message: "Please Select the Site." };
        CustomAlert(Response);
        $("#LoadingPanel1").css("display", "none");
    }
}
//surge report start
function OpenInvoiceSurgeReport() {
    $("#LoadingPanel1").css("display", "block");
    // var siteID = $('#<%=ddlSite.ClientID %> option:selected').val();
    var siteID = $('#ddlSite option:selected').val();
    // if (siteID != "" && siteID != null) {
    if (siteID != "Select Site") {
        GenerateLastGenerated();
        // $("#ContentPlaceHolder1_divInvoicePopup").css("display", "block");
        $("#divInvoiceSurgePopup").css("display", "block");
        $("#LoadingPanel1").css("display", "none");
    }
    else {
        //$('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
        // $('#art1').css("display", "block");
        // $('#alertlabel').html('<strong>Alert ! </strong>Please Select the Site.');
        var Response = { Result: "Failed", Message: "Please Select the Site." };
        CustomAlert(Response);
        $("#LoadingPanel1").css("display", "none");
    }
}
//surge report end
function OpenWeeklyReport() {
    $("#LoadingPanel1").css("display", "block");
    WhrGenerateLastGenerated();
    // var siteID = $('#<%=ddlSite.ClientID %> option:selected').val();
    var siteID = $('#ddlSite option:selected').val();
    //if (siteID != "" && siteID != null) {
    if (siteID != "Select Site") {
        $("#divWeeklyPopup").css("display", "block");
        $("#LoadingPanel1").css("display", "none");
    }
    else {
        //$('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
        //$('#art1').css("display", "block");
        //$('#alertlabel').html('<strong>Alert ! </strong>Please Select the Site.');
        var Response = { Result: "Failed", Message: "Please Select the Site." };
        CustomAlert(Response);
        $("#LoadingPanel1").css("display", "none");
    }
}
//Site Dropdown Bind
function SiteDropDownBind() {
    PipeInventoryHide();
    var ClientID = getParameterByName('id');
    var UID = getParameterByName('UID');
    $.ajax({
        type: "POST",
        url: "/SiteInventory/SiteDropDown",
        data: JSON.stringify({ id: ClientID, UID: UID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            var SiteData = response;
            var SiteLocId = "";
            if (SiteData.length > 0) {
                $("#ddlfrom").empty();
                $("#ddl1to").empty();
                //$("#ddlfrom").append('<option value="">All</option>');
                for (var i = 0; i < SiteData.length; i++) {
                    SiteLocId = SiteData[i].SiteLocID;
                    if (SiteLocId == 0) {
                        // SiteLocId = "";
                        //  $("#ddlSite").prepend("<option value='0' selected='selected'>All</option>");
                    }
                    $("#ddlSite").append("<option value='" + SiteLocId + "'>" + SiteData[i].SiteLocation + "</option>");
                }
                // $("#ddlSite").prepend("<option value='0' selected='selected'>All</option>");
                $("#ddlSite").prepend("<option value='Select Site' selected='selected' disabled>Select Site</option>");
                //  GetSiteInventoryData();
            }
        }
    })
}
function SiteChange() {
    DdlMaterialType();
    GetSiteInventoryData();
}
function GetSiteInventoryData() {
    $("#LoadingPanel1").css("display", "block");
    var siteID = $('#ddlSite option:selected').val();
    var MaterialType = $('#ddlMaterialType').val();//19oct 2022 by S.Sourabh
    if (siteID != "Select Site") {
        var ClientID = getParameterByName('id');
        GetTotalTotalTonnage(ClientID, siteID);
        $.ajax({
            type: "POST",
            url: "/SiteInventory/GetSiteInventoryData",
            data: JSON.stringify({ ClientID: ClientID, siteID: siteID, MaterialType: MaterialType }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (response) {
                var strTbodyTblSiteInventory = "";
                var SiteInventoryData = response;
                $('#tblSiteInventory').DataTable().clear();
                $('#tblSiteInventory').DataTable().destroy();
                $('#divSiteInventory').css("display", "block");
                if (SiteInventoryData.length > 0) {
                    for (var i = 0; i < SiteInventoryData.length; i++) {
                        var SiteID = SiteInventoryData[i].SiteId == null ? "" : SiteInventoryData[i].SiteId;
                        var MaterialType = SiteInventoryData[i].MaterialType == null ? "" : SiteInventoryData[i].MaterialType;
                        var ItemCode = SiteInventoryData[i].ItemCode == null ? "" : SiteInventoryData[i].ItemCode;
                        var ItemDesc = SiteInventoryData[i].ItemDesc == null ? "" : SiteInventoryData[i].ItemDesc;
                        var OrgSiteQuantity = SiteInventoryData[i].OrgSiteQuantity == null ? "" : SiteInventoryData[i].OrgSiteQuantity;
                        var QuantityToBeSent = SiteInventoryData[i].QuantityToBeSent == null ? "" : SiteInventoryData[i].QuantityToBeSent;
                        //  var QuantityReturned = SiteInventoryData[i].QuantityReturned == null ? "" : SiteInventoryData[i].QuantityReturned;
                        var QuantityReturnedToDepot = SiteInventoryData[i].QuantityReturnedToDepot == null ? "" : SiteInventoryData[i].QuantityReturnedToDepot;
                        var JobQuantityDamaged = SiteInventoryData[i].JobQuantityDamaged == null ? "" : SiteInventoryData[i].JobQuantityDamaged;
                        var SiteDamagedQuantityReturnToDepot = SiteInventoryData[i].SiteDamagedQuantityReturnToDepot == null ? "" : SiteInventoryData[i].SiteDamagedQuantityReturnToDepot;
                        var AvailableQuantity = SiteInventoryData[i].AvailableQuantity == null ? "" : SiteInventoryData[i].AvailableQuantity;
                        var OnSite = SiteInventoryData[i].OnSite == null ? "" : SiteInventoryData[i].OnSite;
                        var ItemWeight = SiteInventoryData[i].ItemWeight == null ? "" : SiteInventoryData[i].ItemWeight;
                        var TotalWeight = SiteInventoryData[i].TotalWeight == null ? "" : SiteInventoryData[i].TotalWeight;
                        var HireMeasurement = SiteInventoryData[i].HireMeasurement == null ? "" : SiteInventoryData[i].HireMeasurement;
                        var DailyHireRate = SiteInventoryData[i].DailyHireRate == null ? 0 : SiteInventoryData[i].DailyHireRate;
                        var TotalDailyHire = SiteInventoryData[i].TotalDailyHire == null ? 0 : SiteInventoryData[i].TotalDailyHire;
                        var flag = SiteInventoryData[i].flag == null ? 0 : SiteInventoryData[i].flag;


                        strTbodyTblSiteInventory += "<tr>"
                        //strTbodyTblSiteInventory += "<td>" + SiteID + "</td>"
                        strTbodyTblSiteInventory += "<td style='display:none'></td>"
                        strTbodyTblSiteInventory += "<td>" + MaterialType + "</td>"
                        strTbodyTblSiteInventory += "<td>" + ItemCode + "</td>"
                        strTbodyTblSiteInventory += "<td>" + ItemDesc + "</td>"
                        strTbodyTblSiteInventory += "<td>" + numberWithCommas(OrgSiteQuantity) + "</td>"
                        strTbodyTblSiteInventory += "<td>" + numberWithCommas(QuantityToBeSent) + "</td>"
                        strTbodyTblSiteInventory += "<td>" + numberWithCommas(QuantityReturnedToDepot) + "</td>"
                        strTbodyTblSiteInventory += "<td>" + numberWithCommas(JobQuantityDamaged) + "</td>"
                        strTbodyTblSiteInventory += "<td>" + numberWithCommas(SiteDamagedQuantityReturnToDepot) + "</td>"
                        if (flag == 1) {
                            strTbodyTblSiteInventory += "<td style='background-color:red'>" + numberWithCommas(AvailableQuantity) + "</td>"
                        }
                        else if (flag == 2) {
                            strTbodyTblSiteInventory += "<td style='background-color: orange;'>" + numberWithCommas(AvailableQuantity) + "</td>";
                        }
                        else {
                            strTbodyTblSiteInventory += "<td>" + numberWithCommas(AvailableQuantity) + "</td>"
                        }
                        strTbodyTblSiteInventory += "<td>" + numberWithCommas(OnSite) + "</td>"
                        strTbodyTblSiteInventory += "<td>" + numberWithCommas(ItemWeight.toFixed(2)) + "</td>"
                        strTbodyTblSiteInventory += "<td>" + numberWithCommas(TotalWeight.toFixed(2)) + "</td>"
                        strTbodyTblSiteInventory += "<td>" + HireMeasurement + "</td>"
                        strTbodyTblSiteInventory += "<td><span>$</span>" + numberWithCommas(DailyHireRate.toFixed(4)) + "</td>"
                        strTbodyTblSiteInventory += "<td><span>$</span>" + numberWithCommas(TotalDailyHire.toFixed(4)) + "</td>"


                        strTbodyTblSiteInventory += "</tr>"

                    }

                    $('#tbodyTblSiteInventory').append(strTbodyTblSiteInventory);

                    //$('#tblSiteInventory').DataTable({
                    //    "bSort": false,
                    //    "processing": true,
                    //    "bLengthChange": false,
                    //    "bInfo": false,
                    //    searching: true,
                    //    autoWidth: false,
                    //    scrollX: true,
                    //    scrollCollapse: true,
                    //    scrollY: "60vh",

                    //});
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
                    //buttons: [

                    //    {
                    //        extend: 'csvHtml5',
                    //        title: 'Site Inventory',
                    //    }
                    //],
                    //buttons: [
                    //    {
                    //        extend: 'csvHtml5',
                    //        title: 'Site Inventory',
                    //        exportOptions: {
                    //            format: {
                    //                body: function (data, row, column, node) {
                    //                    var cleanData = data.replace(/<\/?[^>]+(>|$)/g, "");

                    //                    // Preserve ItemCode formatting by treating it as text
                    //                    if (column === 2) {
                    //                        return '="' + cleanData + '"';
                    //                    }

                    //                    // Remove currency symbols for hire rate columns
                    //                    if (column === 15 || column === 16) {
                    //                        return cleanData.replace('$', '');
                    //                    }

                    //                    return cleanData;
                    //                }
                    //            }
                    //        }
                    //    }
                    //]
                    buttons: [
                        {
                            extend: 'csvHtml5',
                            title: 'Site Inventory',
                            fieldSeparator: ',',
                            exportOptions: {
                                format: {
                                    body: function (data, row, column, node) {
                                        // Remove HTML tags
                                        var cleanData = data.replace(/<\/?[^>]+(>|$)/g, "");

                                        // Decode HTML entities like &amp; &quot; etc.
                                        var tempDiv = document.createElement("div");
                                        tempDiv.innerHTML = cleanData;
                                        cleanData = tempDiv.textContent || tempDiv.innerText || "";

                                        // Escape double quotes by doubling them
                                        cleanData = cleanData.replace(/"/g, '""');

                                        // Wrap in ="..." for Excel-safe text
                                        return '="' + cleanData + '"';
                                    }
                                }
                            }
                        }
                    ]


                });
                var HireInventory = $('#hdnHireInventory').val();

                if (HireInventory == "False") {
                    table.column(13).visible(false);
                    table.column(14).visible(false);
                    table.column(15).visible(false);
                }
                $("#LoadingPanel1").css("display", "none");
            }
        })
    }

    else {
        $("#LoadingPanel1").css("display", "none");
        //$('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
        //$('#art1').css("display", "block");
        //$('#alertlabel').html('<strong>Alert ! </strong>Please Select the Site.');
    }

}


function GetTotalTotalTonnage(ClientID, siteID) {
    var MaterialType = $('#ddlMaterialType option:selected').val();
    $.ajax({
        type: "POST",
        url: "/SiteInventory/GetTotalTotalTonnage",
        data: JSON.stringify({ ClientID: ClientID, siteID: siteID, MaterialType: MaterialType }),
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
    $("#divInvoicePopup").css("display", "none");
    var Href = window.location.href.split('?');
    var siteID = $('#ddlSite option:selected').val();
    var MaterialType = $('#ddlMaterialType option:selected').val();
    if (siteID != "Select Site") {
        var ClientID = getParameterByName('id');
        var UID = getParameterByName('UID');
        var UserRole = getParameterByName('UserRole');
        var EncryptData = [siteID];
        var encry = Encrypt(EncryptData);
        siteID = encry[0];
        window.open('/SiteInventoryReport/SiteInventoryReport?UID=' + UID + '&UserRole=' + UserRole + '&ClientId=' + ClientID + '&SiteId=' + siteID + '&MaterialType=' + MaterialType);
        $("#LoadingPanel1").css("display", "none");
    }
    else {
        //$('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
        //$('#art1').css("display", "block");
        //$('#alertlabel').html('<strong>Alert ! </strong>Please Select the Site.');
        var Response = { Result: "Failed", Message: "Please Select the Site." };
        CustomAlert(Response);
        $("#LoadingPanel1").css("display", "none");
    }
}

function btnInvoicePrint_Click() {
    $("#LoadingPanel1").css("display", "block");
    $("#divInvoicePopup").css("display", "none");
    var siteID = $('#ddlSite option:selected').val();
    var ReportStartDate = $('#rdpReportStartDate').val();
    var ReportEndDate = $('#rdpReportEndDate').val();
    var MaterialType = $('#ddlMaterialType').val();
    var RID = getParameterByName('RID');
    if (siteID != "Select Site") {
        var ClientID = getParameterByName('id');
        var UID = getParameterByName('UID');
        var UserRole = getParameterByName('UserRole');
        var EncryptData = [siteID, ReportStartDate, ReportEndDate];
        var encry = Encrypt(EncryptData);
        siteID = encry[0];
        ReportStartDate = encry[1];
        ReportEndDate = encry[2];
        window.open('/SiteInventoiryInvoiceCostAlloctionReport/SiteInventoryInvoiceCostAllocationReport?UID=' + UID + '&UserRole=' + UserRole + '&ClientId=' + ClientID + '&SiteId=' + siteID + '&ReportStrDt=' + ReportStartDate + '&ReportEndDt=' + ReportEndDate + '&RID=' + RID + '&MaterialType=' + MaterialType);

        $("#LoadingPanel1").css("display", "none");
    }
    else {
        //$('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
        //$('#art1').css("display", "block");
        //$('#alertlabel').html('<strong>Alert ! </strong>Please Select the Site.');
        var Response = { Result: "Failed", Message: "Please Select the Site." };
        CustomAlert(Response);
        $("#LoadingPanel1").css("display", "none");
    }
}

//surge cost print button

function btnInvoiceSurgePrint_Click(type) {
    $("#LoadingPanel1").css("display", "block");
    $("#divInvoiceSurgePopup").css("display", "none");

    var SummaryDetails = "";

    if (type == 'D') {

        SummaryDetails = "D";
    }
    else {
        SummaryDetails = "S";
    }

    var SiteID = $('#ddlSite option:selected').val();
    var rdpReportStartDate = $('#rdpSurgeReportStartDate').val();
    var rdpReportEndDate = $('#rdpSurgeReportEndDate').val();
    var MaterialType = $('#ddlMaterialType').val();
    var RID = getParameterByName('RID');
    if (SiteID != "Select Site") {
        var ClientID = getParameterByName('id');
        var UID = getParameterByName('UID');
        var UserRole = getParameterByName('UserRole');
        var EncryptData = [SiteID, rdpReportStartDate, rdpReportEndDate, MaterialType];
        var encry = Encrypt(EncryptData);
        SiteID = encry[0];
        rdpReportStartDate = encry[1];
        rdpReportEndDate = encry[2];
        MaterialType = encry[3];
        window.open('/SiteInventoryInvoiceSurgeCost/SiteInventoryInvoiceSurgeCostAllocationReport?UID=' + UID + '&UserRole=' + UserRole + '&ClientId=' + ClientID + '&SiteId=' + SiteID + '&rdpReportStartDate=' + rdpReportStartDate + '&rdpReportEndDate=' + rdpReportEndDate + '&RID=' + RID + '&SummaryDetails=' + SummaryDetails + '&materialType=' + MaterialType);

        $("#LoadingPanel1").css("display", "none");
    }
    else {
        //$('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
        //$('#art1').css("display", "block");
        //$('#alertlabel').html('<strong>Alert ! </strong>Please Select the Site.');
        var Response = { Result: "Failed", Message: "Please Select the Site." };
        CustomAlert(Response);
        $("#LoadingPanel1").css("display", "none");
    }
}
// surge cost print button end here
function btnWeelkyReport_Click() {
    $("#LoadingPanel1").css("display", "block");
    $("#divWeeklyPopup").css("display", "none");
    var siteID = $('#ddlSite option:selected').val();

    var MaterialType = $('#ddlMaterialType').val();
    var RID = getParameterByName('RID');
    if (siteID != "Select Site") {
        var ClientID = getParameterByName('id');
        var UID = getParameterByName('UID');
        var UserRole = getParameterByName('UserRole');
        var EncryptData = [siteID, MaterialType];
        var encry = Encrypt(EncryptData);
        siteID = encry[0];
        MaterialType = encry[1];
        window.open('/WeeklySiteInventoryStockReport/WeeklySiteInventoryStockReport?UID=' + UID + '&UserRole=' + UserRole + '&ClientId=' + ClientID + '&SiteId=' + siteID + '&RID=' + RID + '&materialType=' + MaterialType);
        $("#LoadingPanel1").css("display", "none");
    }
    else {
        //$('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
        //$('#art1').css("display", "block");
        //$('#alertlabel').html('<strong>Alert ! </strong>Please Select the Site.');
        var Response = { Result: "Failed", Message: "Please Select the Site." };
        CustomAlert(Response);
        $("#LoadingPanel1").css("display", "none");
    }
}

// Added  by Shivam on 06-04-2020
function btnWeeklyReportExportToCsv_Click(type) {
    var WeeklySummaryDetail = "";

    if (type == 'weeklyperday') {

        WeeklySummaryDetail = "weeklyperday";
    }
    else {
        WeeklySummaryDetail = "WeeklySummary";
    }

    $("#LoadingPanel1").css("display", "block");
    $("#divWeeklyPopup").css("display", "none");
    var siteID = $('#ddlSite option:selected').val();
    if (siteID != "Select Site") {
        var MaterialType = $('#ddlMaterialType').val();
        var ClientID = getParameterByName('id');
        var EncryptData = [siteID, MaterialType];
        var encry = Encrypt(EncryptData);
        var RID = getParameterByName('RID');
        if (RID == null) {
            RID = 0;
        }
        siteID = encry[0];
        MaterialType = encry[1];
        window.location.href = '/WeeklySiteInventoryStockReport/btnWeeklyReportExportToCsv_Click?ClientID=' + ClientID + '&SiteID=' + siteID + '&WeeklySummaryDetail=' + WeeklySummaryDetail + '&materialType=' + MaterialType + '&RID=' + RID ;

        $("#LoadingPanel1").css("display", "none");
    }
    else {
        //$('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
        //$('#art1').css("display", "block");
        //$('#alertlabel').html('<strong>Alert ! </strong>Please Select the Site.');
        var Response = { Result: "Failed", Message: "Please Select the Site." };
        CustomAlert(Response);
        $("#LoadingPanel1").css("display", "none");
    }
}


function btnInvoiceReportExportToCsv_Click() {
    $("#LoadingPanel1").css("display", "block");
    $("#divInvoicePopup").css("display", "none");
    var siteID = $('#ddlSite option:selected').val();
    var rdpReportStartDate = $('#rdpReportStartDate').val();
    var rdpReportEndDate = $('#rdpReportEndDate').val();
    var MaterialType = $('#ddlMaterialType').val();
    var RID = getParameterByName('RID');
    if (siteID != "Select Site") {
        if ((rdpReportStartDate != "") && (rdpReportEndDate != "")) {
            var ClientID = getParameterByName('id');
            window.location.href = '/SiteInventoiryInvoiceCostAlloctionReport/btnInvoiceReportExportToCsv_Click?ClientID=' + ClientID + '&SiteID=' + siteID + '&rdpReportStartDate=' + rdpReportStartDate + '&rdpReportEndDate=' + rdpReportEndDate + '&RID=' + RID +'&MaterialType=' + MaterialType;
            $('#divInvoicePopup').css("display", "none");
            $("#LoadingPanel1").css("display", "none");
        }
        else {
            //$('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
            //$('#art1').css("display", "block");
            //$('#alertlabel').html('<strong>Alert ! </strong>Please Select Report Start Date and Report End Date.');
            var Response = { Result: "Failed", Message: "Please Select Report Start Date and Report End Date." };
            CustomAlert(Response);
            $("#LoadingPanel1").css("display", "none");
        }
    }
    else {
        //$('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
        //$('#art1').css("display", "block");
        //$('#alertlabel').html('<strong>Alert ! </strong>Please Select the Site.');
        var Response = { Result: "Failed", Message: "Please Select the Site." };
        CustomAlert(Response);
        $("#LoadingPanel1").css("display", "none");
    }
}




//surge report code start here
function btnInvoiceSurgeReportExportToCsv_Click(type) {

    var SummaryDetails = "";

    if (type == 'D') {

        SummaryDetails = "D";
    }
    else {
        SummaryDetails = "S";
    }
    $("#LoadingPanel1").css("display", "block");
    $("#divInvoiceSurgePopup").css("display", "none");
    var siteID = $('#ddlSite option:selected').val();
    var ReportStartDate = $('#rdpSurgeReportStartDate').val();
    var ReportEndDate = $('#rdpSurgeReportEndDate').val();
    var MaterialType = $('#ddlMaterialType').val();
    var RID = getParameterByName('RID');
    if (siteID != "Select Site") {
        if ((ReportStartDate != "") && (ReportEndDate != "")) {
            var ClientID = getParameterByName('id');
            $.ajax({
                url: "/SiteInventoryInvoiceSurgeCost/btnInvoiceSurgeReportExportToCsv_Click",
                data: { ClientID: ClientID, siteID: siteID, ReportStartDate: ReportStartDate, ReportEndDate: ReportEndDate, RID: RID, SummaryDetails: SummaryDetails, MaterialType: MaterialType },
                async: true,
                success: function (response) {
                    if (response.result == "failed") {
                        $('#divInvoiceSurgePopup').css("display", "none");
                        $("#LoadingPanel1").css("display", "none");
                        var Response = { Result: "Failed", Message: "Data Not Generated !!! Kindly click on Generate Button." };
                        CustomAlert(Response);
                    }
                    else
                    {

                        window.location.href = '/SiteInventoryInvoiceSurgeCost/downloadforcsv?SummaryDetails=' + SummaryDetails;
                        $("#LoadingPanel1").css("display", "none");
                    }
                },
              

            })
           /* window.location.href = '/SiteInventoryInvoiceSurgeCost/btnInvoiceSurgeReportExportToCsv_Click?ClientID=' + ClientID + '&siteID=' + siteID + '&ReportStartDate=' + rdpReportStartDate + '&ReportEndDate=' + rdpReportEndDate + '&RID=' + RID + '&SummaryDetails=' + SummaryDetails + '&MaterialType=' + MaterialType;*/
        }
        else {
            //$('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
            //$('#art1').css("display", "block");
            //$('#alertlabel').html('<strong>Alert ! </strong>Please Select Report Start Date and Report End Date.');
            var Response = { Result: "Failed", Message: "Please Select Report Start Date and Report End Date." };
            CustomAlert(Response);
            $("#LoadingPanel1").css("display", "none");
        }
    }
    else {
        //$('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
        //$('#art1').css("display", "block");
        //$('#alertlabel').html('<strong>Alert ! </strong>Please Select the Site.');
        var Response = { Result: "Failed", Message: "Please Select the Site." };
        CustomAlert(Response);
        $("#LoadingPanel1").css("display", "none");
    }
}

function btnGenerateReport_Click() {
    /*$("#LoadingPanel1").css("display", "block");*/
    var Response = { Result: "Success", Message: "Report Will Take Around 10 minutes to Generate" };
    CustomAlert(Response);

    //No more required for closing the popup (21-12-2022)
    /*$("#divInvoiceSurgePopup").css("display", "none");*/

    var siteID = $('#ddlSite option:selected').val();
    var rdpReportStartDate = $('#rdpSurgeReportStartDate').val();
    var rdpReportEndDate = $('#rdpSurgeReportEndDate').val();
    var MaterialType = $('#ddlMaterialType').val();
    if (siteID != "Select Site") {
        if ((rdpReportStartDate != "") && (rdpReportEndDate != "")) {
            var ClientID = getParameterByName('id');
            $.ajax({
                url: "/SiteInventoryInvoiceSurgeCost/btnGenerateReport_Click",
                data: { ClientID: ClientID, siteID: siteID, rdpReportStartDate: rdpReportStartDate, rdpReportEndDate: rdpReportEndDate, MaterialType: MaterialType },
                async: true,
                success: function (response) {
                    var Result = response;
                    if (Result == "success") {

                        //No more required for closing the popup (21-12-2022)
                        //$('#divInvoiceSurgePopup').css("display", "none");
                        /*$("#LoadingPanel1").css("display", "none");*/

                        GenerateLastGenerated();

                        var Res = { Result: "Success", Message: "Report Generated Successfully" };
                        CustomAlert(Res);
                    }
                    else {
                        var Res = { Result: "Failed", Message: "Report Generated Failed" };
                        CustomAlert(Res);
                    }
                }, 
            })
            //window.location.href = '/SiteInventoryInvoiceSurgeCost/btnGenerateReport_Click?ClientID=' + ClientID + '&siteID=' + siteID + '&rdpReportStartDate=' + rdpReportStartDate + '&rdpReportEndDate=' + rdpReportEndDate;
            //$('#divInvoiceSurgePopup').css("display", "none");
            //$("#LoadingPanel1").css("display", "none");
        }
        else {
            //$('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
            //$('#art1').css("display", "block");
            //$('#alertlabel').html('<strong>Alert ! </strong>Please Select Report Start Date and Report End Date.');
            var Response = { Result: "Failed", Message: "Please Select Report Start Date and Report End Date." };
            CustomAlert(Response);
            /*$("#LoadingPanel1").css("display", "none");*/
        }
    }
    else {
        //$('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
        //$('#art1').css("display", "block");
        //$('#alertlabel').html('<strong>Alert ! </strong>Please Select the Site.');
        var Response = { Result: "Failed", Message: "Please Select the Site." };
        CustomAlert(Response);
       /* $("#LoadingPanel1").css("display", "none");*/
    }
}

//Added on (16-12-2022)

function btnSapGenerateReport_Click() {

    var Response = { Result: "Success", Message: "Report Will Take Around 10 minutes to Generate" };
    CustomAlert(Response);

    //$("#LoadingPanel1").css("display", "block");
    //$("#divInvoiceSurgePopup").css("display", "none");

    var siteID = $('#ddlSite option:selected').val();
    var rdpReportStartDate = $('#rdpReportStartDate1').val();
    var rdpReportEndDate = $('#rdpReportEndDate1').val();
    var MaterialType = $('#ddlMaterialType').val();
    if (siteID != "Select Site") {
        if ((rdpReportStartDate != "") && (rdpReportEndDate != "")) {
            var ClientID = getParameterByName('id');

            $.ajax({
                url: "/SiteInventoryInvoiceSurgeCost/btnGenerateReport_Click",
                data: { ClientID: ClientID, siteID: siteID, rdpReportStartDate: rdpReportStartDate, rdpReportEndDate: rdpReportEndDate, MaterialType: MaterialType },
                async: true,
                success: function (response) {
                    var Result = response;
                    if (Result == "success") {

                        //$('#divInvoiceSurgePopup').css("display", "none");
                        //$("#LoadingPanel1").css("display", "none");

                        SapGenerateLastGenerated();
                        var Response = { Result: "Success", Message: "Report Generated Successfully" };
                        CustomAlert(Response);
                    }
                    else {
                        CustomAlert(response);
                    }
                },
            })

            //window.location.href = '/SiteInventoryInvoiceSurgeCost/btnGenerateReport_Click?ClientID=' + ClientID + '&siteID=' + siteID + '&rdpReportStartDate=' + rdpReportStartDate + '&rdpReportEndDate=' + rdpReportEndDate;
            //$('#divInvoiceSurgePopup').css("display", "none");

        }
        else {
            //$('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
            //$('#art1').css("display", "block");
            //$('#alertlabel').html('<strong>Alert ! </strong>Please Select Report Start Date and Report End Date.');
            var Response = { Result: "Failed", Message: "Please Select Report Start Date and Report End Date." };
            CustomAlert(Response);
            $("#LoadingPanel1").css("display", "none");
        }
    }
    else {
        //$('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
        //$('#art1').css("display", "block");
        //$('#alertlabel').html('<strong>Alert ! </strong>Please Select the Site.');
        var Response = { Result: "Failed", Message: "Please Select the Site." };
        CustomAlert(Response);
        $("#LoadingPanel1").css("display", "none");
    }
}

function btnWhrGenerateReport_Click() {

    var Response = { Result: "Success", Message: "Report Will Take Around 10 minutes to Generate" };
    CustomAlert(Response);

    //$("#LoadingPanel1").css("display", "block");
    //$("#divInvoiceSurgePopup").css("display", "none");

    var siteID = $('#ddlSite option:selected').val();
    var MaterialType = $('#ddlMaterialType').val();

    if (siteID != "Select Site") {
        if ((rdpReportStartDate != "") && (rdpReportEndDate != "")) {
            var ClientID = getParameterByName('id');

            $.ajax({
                url: "/WeeklySiteInventoryStockReport/btnWhrGenerateReport_Click",
                data: { ClientID: ClientID, siteID: siteID, MaterialType: MaterialType },
                async: true,
                success: function (response) {
                    var Result = response;
                    if (Result == "success")
                    {
                        //$('#divInvoiceSurgePopup').css("display", "none");
                        //$("#LoadingPanel1").css("display", "none");

                        WhrGenerateLastGenerated();

                        var Response = { Result: "Success", Message: "Report Generated Successfully" };
                        CustomAlert(Response);
                    }
                    else
                    {
                        CustomAlert(response);
                    }
                },
            })

            //window.location.href = '/WeeklySiteInventoryStockReport/btnGenerateReport_Click?ClientID=' + ClientID + '&siteID=' + siteID;
            //$('#divInvoiceSurgePopup').css("display", "none");

        }
        else {
            //$('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
            //$('#art1').css("display", "block");
            //$('#alertlabel').html('<strong>Alert ! </strong>Please Select Report Start Date and Report End Date.');
            var Response = { Result: "Failed", Message: "Please Select Report Start Date and Report End Date." };
            CustomAlert(Response);
            $("#LoadingPanel1").css("display", "none");
        }
    }
    else {
        //$('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
        //$('#art1').css("display", "block");
        //$('#alertlabel').html('<strong>Alert ! </strong>Please Select the Site.');
        var Response = { Result: "Failed", Message: "Please Select the Site." };
        CustomAlert(Response);
        $("#LoadingPanel1").css("display", "none");
    }
}

// Sap Repoprt Code Start here
function btnSAPCSVPrint_Click() {
    $("#LoadingPanel1").css("display", "block");
    $("#divSAPCSVPopup").css("display", "none");
    var siteID = $('#ddlSite').val();
    var SiteName = $('#ddlSite option:Selected').text();
    var ReportStartDate = $('#rdpReportStartDate1').val();
    var ReportEndDate = $('#rdpReportEndDate1').val();
    if (siteID != "Select Site") {
        var ClientID = getParameterByName('id');
        var UID = getParameterByName('UID');
        var UserRole = getParameterByName('UserRole');
        var MaterialType = $('#ddlMaterialType').val();
        var RID = getParameterByName('RID');
        var EncryptData = [siteID, ReportStartDate, ReportEndDate, SiteName, MaterialType];
        var encry = Encrypt(EncryptData);
        siteID = encry[0];
        ReportStartDate = encry[1];
        ReportEndDate = encry[2];
        SiteName = encry[3];
        MaterialType = encry[4];
        if (ReportStartDate != "" && ReportEndDate != "") {
            window.open('/UploadSAPReport/showreport?UID=' + UID + '&UserRole=' + UserRole + '&id=' + ClientID + '&SiteID=' + siteID + '&ReportStartDate1=' + ReportStartDate + '&ReportEndDate1=' + ReportEndDate + '&SiteName=' + SiteName + '&RID=' + RID + '&materialType=' + MaterialType);
            $("#LoadingPanel1").css("display", "none");
        } else {
            var Response = { Result: "Failed", Message: "Please Select Report Start Date and Report End Date." };
            CustomAlert(Response);
            $("#LoadingPanel1").css("display", "none");
        }

    }
    else {
        var Response = { Result: "Failed", Message: "Please Select the Site." };
        CustomAlert(Response);
        $("#LoadingPanel1").css("display", "none");
    }
}
function btnSAPCSVReportExportToCsv_Click() {
    $("#LoadingPanel1").css("display", "block");
    $("#divSAPCSVPopup").css("display", "none");
    var siteID = $('#ddlSite').val();
    var SiteName = $('#ddlSite option:selected').text();
    var rdpReportStartDate = $('#rdpReportStartDate1').val();
    var rdpReportEndDate = $('#rdpReportEndDate1').val();
    var MaterialType = $('#ddlMaterialType').val();
    var RID = getParameterByName('RID');

    var EncryptData = [MaterialType];
    var encry = Encrypt(EncryptData);
    MaterialType = encry[0];

    if (RID == null) {
        RID = 0;
    }
    if (siteID != "Select Site") {
        if ((rdpReportStartDate != "") && (rdpReportEndDate != "")) {
            var ClientID = getParameterByName('id');
            window.location.href = '/UploadSAPReport/btnSAPCSVReportExportToCsv_Click?id=' + ClientID + '&SiteID=' + siteID + '&rdpReportStartDate1=' + rdpReportStartDate + '&rdpReportEndDate1=' + rdpReportEndDate + '&SiteName=' + SiteName + '&RID=' + RID + '&materialType=' + MaterialType;
            $('#divSAPCSVPopup').css("display", "none");
            $("#LoadingPanel1").css("display", "none");
        }
        else {
            //$('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
            //$('#art1').css("display", "block");
            //$('#alertlabel').html('<strong>Alert ! </strong>Please Select Report Start Date and Report End Date.');
            var Response = { Result: "Failed", Message: "Please Select Report Start Date and Report End Date." };
            CustomAlert(Response);
            $("#LoadingPanel1").css("display", "none");
        }
    }
    else {
        //$('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
        //$('#art1').css("display", "block");
        //$('#alertlabel').html('<strong>Alert ! </strong>Please Select the Site.');
        var Response = { Result: "Failed", Message: "Please Select the Site." };
        CustomAlert(Response);
        $("#LoadingPanel1").css("display", "none");
    }
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

///add by s.sourabh 19 oct 2022
function ddlMaterialType_SelectedIndexChanged() {
    var id = getParameterByName('id');
    var RID = getParameterByName('RID');
    var siteID = $('#ddlSite option:selected').val();
    GetSiteInventoryData();
}
var siteID = $('#ddlSite option:selected').val();
var id = getParameterByName('id');
var RID = getParameterByName('RID');

//add by s.sourabh 19 oct 2022
function DdlMaterialType(id) {
    var id = getParameterByName('id');
    var siteID = $('#ddlSite option:selected').val();
    $.ajax({
        type: "POST",
        url: "/SiteInventory/DdlMaterialType",
        data: JSON.stringify({ id: id, siteID: siteID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
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



function GenerateLastGenerated() {
    var siteID = $('#ddlSite option:selected').val();
    var rdpReportStartDate = $('#rdpSurgeReportStartDate').val();
    var rdpReportEndDate = $('#rdpSurgeReportEndDate').val();
    if (siteID != "Select Site") {
        if ((rdpReportStartDate != "") && (rdpReportEndDate != "")) {
            var ClientID = getParameterByName('id');
            $.ajax({
                type: "POST",
                url: "/SiteInventory/GenerateLastGenerated",

                data: JSON.stringify({ ClientID: ClientID, siteID: siteID, rdpReportStartDate: rdpReportStartDate, rdpReportEndDate: rdpReportEndDate }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                success: function (response) {

                    var GetLastGenerated = response;

                    $('#rdpLastGeneratedDate').val(GetLastGenerated.GeneratedValue);
                }

            })
        }
    }
}
function SapGenerateLastGenerated() {
    var siteID = $('#ddlSite option:selected').val();
    var rdpReportStartDate = $('#rdpReportStartDate1').val();
    var rdpReportEndDate = $('#rdpReportEndDate1').val();
    if (siteID != "Select Site") {
        if ((rdpReportStartDate != "") && (rdpReportEndDate != "")) {
            var ClientID = getParameterByName('id');
            $.ajax({
                type: "POST",
                url: "/UploadSAPReport/SapGenerateLastReport",
                data: JSON.stringify({ ClientID: ClientID, siteID: siteID, rdpReportStartDate: rdpReportStartDate, rdpReportEndDate: rdpReportEndDate }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                success: function (response) {

                    var GetLastGenerated = response;

                    $('#SapLastGeneratedDate').val(GetLastGenerated.GeneratedValue);
                }

            })
        }
    }
}

function WhrGenerateLastGenerated() {
    $("#LoadingPanel1").css("display", "block");
    $("#divInvoiceSurgePopup").css("display", "none");
    var siteID = $('#ddlSite option:selected').val();

    if (siteID != "Select Site") {
        if ((rdpReportStartDate != "") && (rdpReportEndDate != "")) {
            var ClientID = getParameterByName('id');
            $.ajax({
                type: "POST",
                url: "/WeeklySiteInventoryStockReport/WHRGenerateLastReport",
                data: JSON.stringify({ ClientID: ClientID, siteID: siteID }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                success: function (response) {

                    var GetLastGenerated = response;

                    $('#WhrLastGeneratedDate').val(GetLastGenerated.GeneratedValue);
                }

            })
        }
    }
}

function rdpSurgeReportEndDate_IndexChanged() {
    GenerateLastGenerated();
}

function rdpSAPReportEndDate_IndexChanged() {
    SapGenerateLastGenerated();
}