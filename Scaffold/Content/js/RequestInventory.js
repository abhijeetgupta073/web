$(function () {
    var PrevDvirIds = [];
    var NextDvirIds = [];
    var idx = 0;
    var pipeline = function (opts) {
        var conf = $.extend({
            pages: 3,   
            url: '',    
            data: null,
           
            method: 'POST' 
        }, opts);

        var cacheLower = -1;
        var cacheUpper = null;
        var cacheLastRequest = null;
        var cacheLastJson = null;

        return function (request, drawCallback, settings) {
            var ajax = false;
            var requestStart = request.start;
            var drawStart = request.start;
            var requestLength = request.length;
            var requestEnd = requestStart + requestLength;

            if (settings.clearCache) {
                ajax = true;
                settings.clearCache = false;
            }
            else if (cacheLower < 0 || requestStart < cacheLower || requestEnd > cacheUpper) {
                ajax = true;
            }
            else if (JSON.stringify(request.order) !== JSON.stringify(cacheLastRequest.order) ||
                JSON.stringify(request.columns) !== JSON.stringify(cacheLastRequest.columns) ||
                JSON.stringify(request.search) !== JSON.stringify(cacheLastRequest.search)
            ) {
                ajax = true;
            }
            cacheLastRequest = $.extend(true, {}, request);

            if (ajax) {
                if (requestStart < cacheLower) {
                    requestStart = requestStart - (requestLength * (conf.pages - 1));

                    if (requestStart < 0) {
                        requestStart = 0;
                    }
                }

                cacheLower = requestStart;
                cacheUpper = requestStart + (requestLength * conf.pages);

                request.start = requestStart;
                request.length = requestLength * conf.pages;
                if (typeof conf.data === 'function') {
                    var d = conf.data(request);
                    if (d) {
                        $.extend(request, d);
                    }
                }
                else if ($.isPlainObject(conf.data)) {
                    $.extend(request, conf.data);
                }
                console.log(request);
                settings.jqXHR = $.ajax({
                    "type": conf.method,
                    "url": conf.url,
                    "data": request,
                    "dataType": "json",
                    "cache": false,
                    "success": function (json) {

                        cacheLastJson = $.extend(true, {}, json);
                        console.log(cacheLastJson);
                        if (cacheLower != drawStart) {
                            json.aaData.splice(0, drawStart - cacheLower);
                        }
                        if (json.aaData != undefined && json.aaData.length > 0 && requestLength >= -1) {
                            json.aaData.splice(requestLength, json.aaData.length);
                        }

                        drawCallback(json);
                    }
                });
            }
            else {
                json = $.extend(true, {}, cacheLastJson);
                json.draw = request.draw; // Update the echo for each response
                //json.data.splice(0, requestStart - cacheLower);
                //json.data.splice(requestLength, json.data.length);
                if (cacheLower != drawStart) {
                    json.aaData.splice(0, drawStart - cacheLower);
                }
                if (json.aaData != undefined && json.aaData.length > 0 && requestLength >= -1) {
                    json.aaData.splice(requestLength, json.aaData.length);
                    drawCallback(json);
                }
            }
        }
    };
    $.fn.dataTable.Api.register('clearPipeline()', function () {
        return this.iterator('table', function (settings) {
            settings.clearCache = true;
        });
    });
    var Href = window.location.href.split('?');
    if (Href.length == 1) {
        $('#UlNavTabSeciton li').css('display', 'none');
        $('#liThirdParty').css('display', 'block');
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
            //window.location.href = '/InventoryManagement.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
            window.location.href = '/InventoryManagement/InventoryManagement?' + Href[1];

        }
        if (ClickAcnhorID == 'AnchorReturnInventory') {
            //  window.location.href = '/ReturnInventory.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
            window.location.href = '/ReturnInventory/ReturnInventory?' + Href[1];
        }
        if (ClickAcnhorID == 'AnchorJobInventory') {
            $('#LoadingPanel1').css('display', 'none');
        }

        if (ClickAcnhorID == 'AnchorDepotInventory') {

            window.location.href = '/MainDepotInventory/MainDepotInventory?' + Href[1];
        }
        if (ClickAcnhorID == 'AnchorSiteInventory') {
            // window.location.href = '/SiteInventory.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
            window.location.href = '/SiteInventory/SiteInventory?' + Href[1];
        }
        if (ClickAcnhorID == 'AnchorDamagedInventory') {
            // window.location.href = '/DamagedInventory.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
            window.location.href = '/DamagedInventory/DamagedInventory?' + Href[1];
        }
        if (ClickAcnhorID == 'AnchorPipeLineInventory') {
            //window.location.href = '/ReturnInventory/ReturnInventory?' + Href[1];
            window.location.href = '/PipeLineInventory/PipeLineInventory?' + Href[1];
        }
    });
});

//Site Dropdown Bind
function SiteDropDownBind() {
    var ClientID = getParameterByName('id');
    var UID = getParameterByName('UID');
    $('#hid').val('@viewBag.hid');
    $.ajax({
        type: "POST",
        url: "/RequestInventory/SiteDropDown",
        data: JSON.stringify({ id: ClientID, UID: UID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            var SiteData = response;
            var SiteLocId = "";
            if (SiteData.length > 0) {
                //  $("#ddlSite").append('<option value="">All</option>');
                for (var i = 0; i < SiteData.length; i++) {
                    SiteLocId = SiteData[i].SiteLocID;
                    if (SiteLocId == 0) {
                        SiteLocId = "";
                    }
                    // $("#ddlSite").append("<option value='" + SiteData[i].SiteLocID + "'>" + SiteData[i].SiteLocation + "</option>");
                    $("#ddlSite").append("<option value='" + SiteLocId + "'>" + SiteData[i].SiteLocation + "</option>");

                }
            }
        }
    })
}


// Get RequestinventoryData
var ArrayIdxArr = [0];
var RequestStatusGlobal = '';
//function GetRequestInventoryData() {
//    PipeInventoryHide();
//    ArrayIdxArr.length = 0;
//    $('#LoadingPanel1').show();
//    var Siteid = $('#ddlSite option:selected').val();
//    if (Siteid == "" && Siteid == undefined) {
//        Siteid = "All";
//    }
//    var ClientID = getParameterByName('id');
//    var FromId = getParameterByName('FromId');
//    var ToId = getParameterByName('ToId');
//    var ddl1from = getParameterByName('ddl1from');
//    var ddl1to = getParameterByName('ddl1to');
//    var ddlfrom = Siteid;
//    // var rdodepottosite = "True";
//    var rdoThirdPartytoDepot = $('#rdoThirdPartytoDepot').prop('checked');
//    var rdodepottosite = $('#rdodepottosite').prop('checked');
//    var rdositetojob = $('#rdositetojob').prop('checked');

//    // var rdositetojob = "False";
//    var JobSiteid = getParameterByName('Siteid');
//    // var RID = getParameterByName('RID');
//    var RID = getParameterByName('RID') == null ? "" : getParameterByName('RID');

//    var ArrayData = [RID];
//    var DecryptData = Decrypt(ArrayData);

//    var RegisterID = DecryptData[0];
//    var EmailFlag = getParameterByName('EmailFlag');


//    $.ajax({
//        type: "POST",
//        // url: "/SiteInventory/GetSiteInventoryData",
//        url: "/RequestInventory/GetRequestInventoryData",
//        data: JSON.stringify({ id: ClientID, FromId: FromId, ToId: ToId, ddl1from: ddl1from, ddl1to: ddl1to, JobSiteid: JobSiteid, ddlfrom: ddlfrom, rdodepottosite: rdodepottosite, rdositetojob: rdositetojob, RID: RID, rdoThirdPartytoDepot: rdoThirdPartytoDepot }),
//        //  public ActionResult GetRequestInventoryData(string id,string FromId,string ToId,string ddl1from,string ddl1to,string Siteid,string ddlfrom,string rdodepottosite,string rdositetojob)

//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        async: true,
//        success: function (response) {
//            var str = "";
//            var GetData = response;
//            $('#tblRequestInventory').DataTable().clear();
//            $('#tblRequestInventory').DataTable().destroy();

//            if (GetData.length > 0) {
//                // $('#tblRequestInventory').css("display", "block");

//                for (var i = 0; i < GetData.length; i++) {
//                    var transId = GetData[i].transId == null ? "" : GetData[i].transId;
//                    var RequestStatus = GetData[i].RequestStatus == null ? "" : GetData[i].RequestStatus;
//                    var transtype = GetData[i].transtype == null ? "" : GetData[i].transtype;
//                    var FromId = GetData[i].FromId == null ? "" : GetData[i].FromId;
//                    var TransDesc = GetData[i].TransDesc == null ? "" : GetData[i].TransDesc;
//                    var DeliveryStatus = GetData[i].DeliveryStatus == null ? "" : GetData[i].DeliveryStatus;
//                    var FromName1 = GetData[i].FromName == null ? "" : GetData[i].FromName;
//                    var FromName = (FromName1.length > 25) ? FromName1.substring(0, 25) + '.....' : FromName1;
//                    var ToName = GetData[i].ToName == null ? "" : GetData[i].ToName;
//                    var ToId = GetData[i].ToId == null ? "" : GetData[i].ToId;
//                    var To = GetData[i].To == null ? "" : GetData[i].To;
//                    var FromPerson = GetData[i].FromPerson == null ? "" : GetData[i].FromPerson;
//                    var ToPerson = GetData[i].ToPerson == null ? "" : GetData[i].ToPerson;
//                    var FromTime = GetData[i].FromTime == null ? "" : GetData[i].FromTime;
//                    var RequiredDate = GetData[i].RequiredDate == null ? "" : GetData[i].RequiredDate;
//                    var ToTime = GetData[i].ToTime == null ? "" : GetData[i].ToTime;
//                    var NoofItems = GetData[i].NoofItems == null ? "" : GetData[i].NoofItems;
//                    var TotalWeight = GetData[i].TotalWeight == null ? "" : GetData[i].TotalWeight.toFixed(2);
//                    var ReceiptConfirmation = GetData[i].ReceiptConfirmation == null ? "" : GetData[i].ReceiptConfirmation;
//                    var Supplier = GetData[i].Supplier == null ? "" : GetData[i].Supplier;
//                    var LastEmailSentOn = GetData[i].LastEmailSentOn == null ? "" : GetData[i].LastEmailSentOn;
//                    var PickDate = GetData[i].PickDate == null ? "" : GetData[i].PickDate;
//                    var Comment1 = GetData[i].Comment == null ? "" : GetData[i].Comment; //Added on 06-06-2023 Client wanted this Columns in editing field so we added this into the list also.
//                    var Comment = (Comment1.length > 40) ? Comment1.substring(0, 40) + '.....' : Comment1; //Added on 06-06-2023 Client wanted this Columns in editing field so we added this into the list also.

//                    var today = new Date();

//                    str += "<tr>"
//                    str += "<td class='setWidth0' style='display:none'></td>"
//                    //Added on 06-06-2023 to Add the Edit Functionality On Request Inventory List Page To Enable Editing For Supplier,Comment and Description Columns
//                    /* str += "<td id='transEdtId_" + i + "' title='Edit' style='cursor:pointer;' class='ClassbtnRequestEdit setWidth1'><i class='fa fa-pencil'></i></td>";*/
//                    str += "<td id='transEdtId_" + i + "' title='Edit' style='cursor:pointer;' class='ClassbtnRequestEdit setWidth1'><i class='fa fa-pencil' data-transid='" + transId + "'></i></td>";
//                    str += "<td id='transdltId_" + i + "' title='Delete' style='cursor:pointer;' class='setWidth1'><i class='fa fa-trash' onclick=ShowDeletePopup('" + transId + "')></i></td>";
//                    str += "<td id='transImgId_" + i + "' class='setWidth2' title='Images' style='cursor:pointer;' onclick=OpenDedliveryDocument(" + transId + ")><i class='fa fa-image'></i></td>";
//                    str += "<td id='transChkId_" + i + "' class='setWidth3' style='text-align:center;'><input type='checkbox' class='ChkTransID' transChkId_='" + transId + "'/></td>"
//                    str += "<td class='setWidth4'>" + transId + "</td>"
//                    //delivered date was not visible on jo list page
//                    //  str += "<td class='setWidth5'>" + DateFormat(PickDate) + "</td>"
//                    str += "<td class='setWidth5'>" + DateFormat(PickDate) + "</td>"

//                    if (DeliveryStatus == "Confirmed") {
//                        if (ReceiptConfirmation != true) {
//                            str += "<td title='Open Delivery Docket' class='setWidth6' style='cursor:pointer;'><a type='button' id='tbnOpen' class='btnsiteView'  onclick=OpenDeliveryDocket('" + transId + "','" + FromId + "') ><i class='fa fa-link'></i>&nbsp;Open</a></td>";
//                        } else {
//                            str += "<td title='View Material List Report' class='setWidth6' style='cursor:pointer;'><button type='button' id='tbnView' Class='w3-btn w3-teal btnsiteView' onclick=ViewDeliveryDocketDocument('" + transId + "','" + FromId + "') ><i class='fa fa-search'></i>&nbsp;View</button></td>";
//                        }
//                    }

//                    else {
//                        str += "<td class='setWidth6'>" + DeliveryStatus + "</td>"
//                    }


//                    str += "<td title='View Record' style='cursor:pointer;' class='btnJobView setWidth7'><button type='button' id='tbnJobMaterialListView' Class='w3-btn w3-teal btnJobView' onclick=ViewJobMaterialListReport('" + transId + "') ><i class='fa fa-search'></i>&nbsp;View</button></td>";

//                    if (RequestStatus == "Confirmed") {
//                        str += "<td title='View Request Docket  Report' class='setWidth21' style='cursor:pointer;'><button type='button' id='tbnViewRequest' Class='w3-btn w3-teal btnsiteView' onclick=ViewRequestDocketReport('" + transId + "','" + FromId + "') ><i class='fa fa-search'></i>&nbsp;View</button></td>";
//                    }
//                    else {
//                        str += "<td class='setWidth21'>" + RequestStatus + "</td>"
//                    }

//                    str += "<td class='setWidth8'>" + Supplier + "</td>"
//                    str += "<td class='setWidth9'>" + TransDesc + "</td>"
//                    str += "<td id='FromID_" + transId + "' class='setWidth10' style='display:none'>" + FromId + "</td>"
//                    str += "<td id='From_" + transId + "' class='setWidth11' title='" + FromName1 + "'>" + FromName + "</td>"
//                    str += "<td id='To_" + transId + "' class='setWidth12' >" + ToName + "</td>"
//                    str += "<td id='ToID_" + transId + "' style='display:none' class='setWidth13' >" + ToId + "</td>"
//                    //str += "<td>" + RequestStatus + "</td>"
//                    str += "<td id='RequestStatus_" + transId + "' class='setWidth14'>" + RequestStatus + "</td>"
//                    str += "<td id='DeliveryStatus_" + transId + "' class='setWidth15'>" + DeliveryStatus + "</td>"
//                    str += "<td class='setWidth16'>" + FromPerson + "</td>"
//                    str += "<td class='setWidth17'>" + ToPerson + "</td>"
//                    str += "<td class='setWidth18'>" + DateFormat(FromTime) + "</td>"
//                    /* if (!RID) {*/
//                    str += "<td class='setWidth23'>" + DateFormat(RequiredDate) + "</td>"

//                    //}
//                    //else {
//                    //    str += "<td style='display:none' class='setWidth18'>" + DateFormat(RequiredDate) + "</td>"

//                    //}

//                    str += "<td class='setWidth19'>" + numberWithCommas(NoofItems) + "</td>"
//                    str += "<td class='setWidth20'>" + numberWithCommas(TotalWeight) + "</td>"
//                    str += "<td class='setWidth21'>" + DateFormat(LastEmailSentOn) + "</td>"
//                    //Added on 06-06-2023 Client wanted this Columns in editing field so we added this into the list also.
//                    str += "<td class='setWidth22' title='" + Comment1 + "'>" + Comment + "</td>"
//                    str += "<td class='setWidth22' style='display:none;'>" + Comment1 + "</td>"
//                    str += "</tr>"
//                }

//                $('#tbodyTblRequestInventory').append(str);

//                $(".ChkTransID").click(function () {
//                    event.stopImmediatePropagation();
//                    var transID = $(this).attr('transChkId_').split('_')[0];
//                    var columnCU = $(this).prop('checked');
//                    var RequestStatus = $('#RequestStatus_' + transID).text();
//                    var FromID = $('#FromID_' + transID).text();
//                    var ToID = $('#ToID_' + transID).text();
//                    //Commented on (01-02-2024) we dont want site name instead we want from where we are requesting data ex(Site,Depot).
//                    //var From = $('#From_' + transID).text();

//                    //Added on (01-02-2024) we want (Site,Depot) instead of Site Name when we are requesting data.
//                    var From = "";
//                    if ($('#rdodepottosite').prop('checked') == true) {
//                        From = "Site";
//                    }
//                    else if ($('#rdoThirdPartytoDepot').prop('checked') == true) {
//                        From = "Depot";
//                    }
//                    var To = $('#To_' + transID).text();
//                    var DeliveryStatus = $('#DeliveryStatus_' + transID).text();
//                    RequestStatusGlobal = RequestStatus;

//                    var ArryVal = [transID, RequestStatus, FromID, ToID, From, To, DeliveryStatus];

//                    if (columnCU == true) {
//                        //ArrayIdxArr.push(transID);
//                        ArrayIdxArr.push(ArryVal);
//                    }
//                    else {
//                        for (var i = 0; i < ArrayIdxArr.length; i++) {
//                            var TempArry = ArrayIdxArr[i];
//                            if (TempArry[0] == transID) {
//                                // if (ArrayIdxArr[i] == transID) {
//                                ArrayIdxArr.splice(i, 1);
//                            }
//                        }
//                    }
//                });
//                $('#LoadingPanel1').hide();

//                //Added on 06-06-2023 to Add the Edit Functionality On Request Inventory List Page To Enable Editing For Supplier,Comment and Description Columns
//                //    $(".fa-pencil").click(function () {
//                //        var tr = $(this).closest("tr");

//                //        var visible = $(this).parent().parent().hasClass("Edit");
//                //        var tr = $(this).closest('tr');
//                //        var row = table.row(tr);
//                //        if (row.child.isShown()) {
//                //            $(this).parent().parent().next('tr').remove();
//                //            $(this).parent().parent().removeClass('Edit');
//                //            GetRequestInventoryData();
//                //        }
//                //        else {
//                //            row.child(Editing(row.data())).show();
//                //            row.child(BindSubCategory(row.data())).show();
//                //            row.child.show();
//                //            tr.addClass('Edit');
//                //        }

//                //    });

//                $(".fa-pencil").click(function () {

//                    var tr = $(this).closest('tr');
//                    var row = table.row(tr);

//                    var transId = $(this).attr('data-transid');
//                    if (IsInactive(transId)) {
//                        CustomAlert({
//                            Result: "Failed", Message: "This site is InActive. Edit is not allowed."
//                        });
//                        return false;
//                    }

//                    var visible = $(this).parent().parent().hasClass("Edit");

//                    if (row.child.isShown()) {
//                        $(this).parent().parent().next('tr').remove();
//                        $(this).parent().parent().removeClass('Edit');
//                        GetRequestInventoryData();
//                    }
//                    else {
//                        row.child(Editing(row.data())).show();
//                        row.child(BindSubCategory(row.data())).show();
//                        row.child.show();
//                        tr.addClass('Edit');
//                    }
//                });
//            }

//            var table = $('#tblRequestInventory').DataTable({
//                "bSort": false,
//                "processing": true,
//                searching: true,
//                "bLengthChange": false,
//                "bInfo": false,
//                autoWidth: false,
//                scrollX: true,
//                scrollCollapse: true,
//                scrollY: "60vh",

//            });

//            if (RegisterID == "" || RegisterID == null) {
//                table.column(8).visible(false);
//            } else {
//                table.column(7).visible(false);
//            }

//            $('#LoadingPanel1').hide();

//            // Hide Show For DataTable Column And Buttons Start Here
//            GridButtonHideShow(table, "tblRequestInventory");

//            // Hide Show For DataTable Column And Buttons End Here
//        }
//    })

//    if (EmailFlag == "1") {
//        var chckEmail = 1;
//        btnEmailClick(chckEmail);
//        //countEmail++;
//        // $('#divEmailConfirm').css("display", "block");
//    }

//}

function GetRequestInventoryData() {
    PipeInventoryHide();
    ArrayIdxArr.length = 0;
    $('#LoadingPanel1').show();

    var Siteid = $('#ddlSite option:selected').val();
    if (!Siteid) {
        Siteid = "All";
    }

    var ClientID = getParameterByName('id');
    var FromId = getParameterByName('FromId');
    var ToId = getParameterByName('ToId');
    var ddl1from = getParameterByName('ddl1from');
    var ddl1to = getParameterByName('ddl1to');
    var ddlfrom = Siteid;
    var rdoThirdPartytoDepot = $('#rdoThirdPartytoDepot').prop('checked');
    var rdodepottosite = $('#rdodepottosite').prop('checked');
    var rdositetojob = $('#rdositetojob').prop('checked');
    var JobSiteid = getParameterByName('Siteid');
    var RID = getParameterByName('RID') == null ? "" : getParameterByName('RID');
    var EmailFlag = getParameterByName('EmailFlag');

    var ArrayData = [RID];
    var DecryptData = Decrypt(ArrayData);
    var RegisterID = DecryptData[0];

    if ($.fn.DataTable.isDataTable('#tblRequestInventory')) {
        $('#tblRequestInventory').DataTable().destroy();
        $('#tblRequestInventory').empty();
    }

    var table = $('#tblRequestInventory').DataTable({
        processing: true,
        serverSide: true,
        serverMethod: "POST",
        ajax: {
            url: "/RequestInventory/GetRequestInventoryData",
            data: function (d) {
                d.id = ClientID;
                d.FromId = FromId;
                d.ToId = ToId;
                d.ddl1from = ddl1from;
                d.ddl1to = ddl1to;
                d.JobSiteid = JobSiteid;
                d.ddlfrom = ddlfrom;
                d.rdodepottosite = rdodepottosite;
                d.rdositetojob = rdositetojob;
                d.RID = RID;
                d.rdoThirdPartytoDepot = rdoThirdPartytoDepot;
            },
            error: function (xhr, error, thrown) {
                $('#LoadingPanel1').hide();
                console.error("GetRequestInventoryData ajax error:", xhr.responseText || thrown);
            }
        },
        searching: true,
        ordering: false,
        lengthChange: false,
        info: false,
        autoWidth: false,
        paging: true,
        scrollX: true,
        scrollCollapse: true,
        scrollY: "60vh",
        pageLength: 10,
        columns: [
            { title: "", data: null, visible: false, orderable: false, className: "setWidth0", defaultContent: "" },
            {
                title: "", data: null, orderable: false, className: "setWidth1 ClassbtnRequestEdit",
                render: function (data, type, row, meta) {
                    return "<span id='transEdtId_" + meta.row + "' title='Edit' style='cursor:pointer;'><i class='fa fa-pencil'></i></span>";
                }
            },
            {
                title: "", data: "transId", orderable: false, className: "setWidth1",
                render: function (data) {
                    return "<span title='Delete' style='cursor:pointer;'><i class='fa fa-trash' onclick=\"ShowDeletePopup('" + (data || "") + "')\"></i></span>";
                }
            },
            {
                title: "", data: "transId", orderable: false, className: "setWidth2",
                render: function (data, type, row, meta) {
                    return "<span id='transImgId_" + meta.row + "' title='Images' style='cursor:pointer;' onclick='OpenDedliveryDocument(" + (data || 0) + ")'><i class='fa fa-image'></i></span>";
                }
            },
            {
                title: "#", data: "transId", orderable: false, className: "setWidth3",
                render: function (data) {
                    return "<input type='checkbox' class='ChkTransID' transChkId_='" + (data || "") + "' />";
                }
            },
            { title: "Trans Id", data: "transId", className: "setWidth4", defaultContent: "" },
            {
                title: "Delivered <br>Date", data: "PickDate", className: "setWidth5",
                render: function (data) { return data ? DateFormat(data) : ""; }
            },
            {
                title: "Delivery<br> Docket", data: null, name: "deliverydocket", className: "setWidth6",
                render: function (data, type, row) {
                    var deliveryStatus = row.DeliveryStatus || "";
                    var transId = row.transId || "";
                    var fromId = row.FromId || "";

                    if (deliveryStatus === "Confirmed") {
                        if (row.ReceiptConfirmation !== true) {
                            return "<a type='button' class='btnsiteView' onclick=\"OpenDeliveryDocket('" + transId + "','" + fromId + "')\"><i class='fa fa-link'></i>&nbsp;Open</a>";
                        } else {
                            return "<button type='button' class='w3-btn w3-teal btnsiteView' onclick=\"ViewDeliveryDocketDocument('" + transId + "','" + fromId + "')\"><i class='fa fa-search'></i>&nbsp;View</button>";
                        }
                    }
                    return deliveryStatus;
                }
            },
            {
                title: "Material List", data: null, orderable: false, name: "jobview", className: "setWidth7",
                render: function (data, type, row) {
                    return "<button type='button' class='w3-btn w3-teal btnJobView' title='View Record' onclick=\"ViewJobMaterialListReport('" + (row.transId || "") + "')\"><i class='fa fa-search'></i>&nbsp;View</button>";
                }
            },
            {
                title: "Request Docket", data: null, className: "setWidth21",
                render: function (data, type, row) {
                    var requestStatus = row.RequestStatus || "";
                    var transId = row.transId || "";
                    var fromId = row.FromId || "";

                    if (requestStatus === "Confirmed") {
                        return "<button type='button' class='w3-btn w3-teal btnsiteView' onclick=\"ViewRequestDocketReport('" + transId + "','" + fromId + "')\"><i class='fa fa-search'></i>&nbsp;View</button>";
                    }
                    return requestStatus;
                }
            },
            { title: "Supplier", data: "Supplier", className: "setWidth8", defaultContent: "" },
            { title: "Description", data: "TransDesc", className: "setWidth9", defaultContent: "" },
            { title: "From ID", data: "FromId", visible: false, className: "setWidth10", defaultContent: "" },
            {
                title: "From", data: "FromName", className: "setWidth11",
                render: function (data, type, row) {
                    var fullText = data || "";
                    var shortText = fullText.length > 25 ? fullText.substring(0, 25) + "....." : fullText;
                    return "<span id='From_" + (row.transId || "") + "' title='" + fullText + "'>" + shortText + "</span>";
                }
            },
            {
                title: "To", data: "ToName", className: "setWidth12",
                render: function (data, type, row) {
                    return "<span id='To_" + (row.transId || "") + "'>" + (data || "") + "</span>";
                }
            },
            { title: "To ID", data: "ToId", visible: false, className: "setWidth13", defaultContent: "" },
            {
                title: "Request Status", data: "RequestStatus", className: "setWidth14",
                render: function (data, type, row) {
                    return "<span id='RequestStatus_" + (row.transId || "") + "'>" + (data || "") + "</span>";
                }
            },
            {
                title: "Delivery<br> Status", data: "DeliveryStatus", className: "setWidth15",
                render: function (data, type, row) {
                    return "<span id='DeliveryStatus_" + (row.transId || "") + "'>" + (data || "") + "</span>";
                }
            },
            { title: "Requestor", data: "FromPerson", className: "setWidth16", defaultContent: "" },
            { title: "Deliverer", data: "ToPerson", className: "setWidth17", defaultContent: "" },
            {
                title: "Requested <br>Date", data: "FromTime", className: "setWidth18",
                render: function (data) { return data ? DateFormat(data) : ""; }
            },
            {
                title: "Required <br>Date", data: "RequiredDate", className: "setWidth23",
                render: function (data) { return data ? DateFormat(data) : ""; }
            },
            {
                title: "(Requested)<br> No. of Items", data: "NoofItems", className: "setWidth19",
                render: function (data) { return numberWithCommas(data || 0); }
            },
            {
                title: "Total Weight<br> (Tonnes)", data: "TotalWeight", className: "setWidth20",
                render: function (data) { return numberWithCommas(data != null ? parseFloat(data).toFixed(2) : 0); }
            },
            {
                title: "Last Email Sent On", data: "LastEmailSentOn", className: "setWidth21",
                render: function (data) { return data ? DateFormat(data) : ""; }
            },
            {
                title: "Comment", data: "Comment", className: "setWidth22",
                render: function (data) {
                    var fullText = data || "";
                    var shortText = fullText.length > 40 ? fullText.substring(0, 40) + "....." : fullText;
                    return "<span title='" + fullText + "'>" + shortText + "</span>";
                }
            },
            { title: "", data: "Comment", visible: false, className: "setWidth22", defaultContent: "" }
        ],
        initComplete: function () {
            $(table.column(1).header()).attr('id', 'UniqueEdit');
            $(table.column(2).header()).attr('id', 'UniqueDelete');
            $(table.column(3).header()).attr('id', 'UniqueImage');

            if (RegisterID == "" || RegisterID == null) {
                table.column('jobview:name').visible(false);
            } else {
                table.column('deliverydocket:name').visible(false);
            }
        },
        initComplete: function () {
            if (RegisterID == "" || RegisterID == null) {
                table.column('jobview:name').visible(false);
            } else {
                table.column('deliverydocket:name').visible(false);
            }
        },
        drawCallback: function () {
            $('#LoadingPanel1').hide();

            $(".ChkTransID").each(function () {
                var t = $(this).attr('transChkId_');
                for (var i = 0; i < ArrayIdxArr.length; i++) {
                    if (ArrayIdxArr[i][0] == t) {
                        $(this).prop('checked', true);
                        break;
                    }
                }
            });

            $(".ChkTransID").off('click').on('click', function (event) {
                event.stopImmediatePropagation();

                var transID = $(this).attr('transChkId_');
                var columnCU = $(this).prop('checked');
                var tr = $(this).closest('tr');
                var rowData = table.row(tr).data();

                var RequestStatus = rowData.RequestStatus || "";
                var FromID = rowData.FromId || "";
                var ToID = rowData.ToId || "";
                var To = rowData.ToName || "";
                var DeliveryStatus = rowData.DeliveryStatus || "";

                var From = "";
                if ($('#rdodepottosite').prop('checked') == true) {
                    From = "Site";
                } else if ($('#rdoThirdPartytoDepot').prop('checked') == true) {
                    From = "Depot";
                }

                RequestStatusGlobal = RequestStatus;

                var ArryVal = [transID, RequestStatus, FromID, ToID, From, To, DeliveryStatus];

                if (columnCU == true) {
                    ArrayIdxArr.push(ArryVal);
                } else {
                    for (var i = 0; i < ArrayIdxArr.length; i++) {
                        if (ArrayIdxArr[i][0] == transID) {
                            ArrayIdxArr.splice(i, 1);
                            break;
                        }
                    }
                }
            });

            $(".fa-pencil").off('click').on('click', function () {
                var tr = $(this).closest('tr');
                var row = table.row(tr);
                var rowData = row.data();

                if (IsInactive(rowData.transId)) {
                    CustomAlert({ Result: "Failed", Message: "This site is InActive. Edit is not allowed." });
                    return false;
                }

                if (row.child.isShown()) {
                    row.child.hide();
                    tr.removeClass('Edit');
                    table.ajax.reload(null, false);
                } else {
                    row.child(Editing(rowData)).show();
                    row.child(BindSubCategory(rowData)).show();
                    row.child.show();
                    tr.addClass('Edit');
                }
            });

            GridButtonHideShow(table, "tblRequestInventory");
        }
    });

    if (EmailFlag == "1") {
        btnEmailClick(1);
    }
}
function OpenDeliveryDocket(transId, FromId) {
    $('#LoadingPanel1').show();
    var Href = window.location.href.split('?');
    var ClientID = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var ThirdParty = $('#rdoThirdPartytoDepot').prop('checked');
    // var ToId = getParameterByName('FromId');
    var EncryptData = [transId, FromId, ThirdParty];
    var encryData = Encrypt(EncryptData);
    transId = encryData[0];
    FromId = encryData[1];
    ThirdParty = encryData[2];
    window.location.href = '/DeliveryDocket/DeliveryDocket?UID=' + UID + '&UserRole=' + UserRole + '&id=' + ClientID + '&checkid=' + transId + '&ToId=' + FromId + '&ThirdParty=' + ThirdParty;
    $('#LoadingPanel1').hide();
}
function ViewDeliveryDocketDocument(transId, FromId) {
    $('#LoadingPanel1').show();
    var Href = window.location.href.split('?');
    var ClientID = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var SiteId = getParameterByName('ToId');
    // var ToId = getParameterByName('FromId');
    var ThirdParty = $('#rdoThirdPartytoDepot').prop('checked');
    var EncryptData = [transId, FromId, ThirdParty];
    var encryData = Encrypt(EncryptData);
    transId = encryData[0];
    FromId = encryData[1];
    ThirdParty = encryData[2];

    window.open('/DeliveryDocketReport/DeliveryDocketReport?UID=' + UID + '&UserRole=' + UserRole + '&id=' + ClientID + '&ToId=' + FromId + '&checkid=' + transId + '&ThirdParty=' + ThirdParty);
    $('#LoadingPanel1').hide();

}
function ViewRequestDocketReport(transId, FromId) {
    $('#LoadingPanel1').show();
    var Href = window.location.href.split('?');
    var ClientID = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var SiteId = getParameterByName('ToId');
    // var ToId = getParameterByName('FromId');
    var ThirdParty = $('#rdoThirdPartytoDepot').prop('checked');
    var EncryptData = [transId, FromId, ThirdParty];
    var encryData = Encrypt(EncryptData);
    transId = encryData[0];
    FromId = encryData[1];
    ThirdParty = encryData[2];

    window.open('/RequestDocketReport/RequestDocketReport?UID=' + UID + '&UserRole=' + UserRole + '&id=' + ClientID + '&ToId=' + FromId + '&checkid=' + transId + '&ThirdParty=' + ThirdParty);
    $('#LoadingPanel1').hide();
}
function ViewJobMaterialListReport(transId) {
    $('#LoadingPanel1').show();
    var Href = window.location.href.split('?');
    var ClientID = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var SiteId = getParameterByName('Siteid');
    var RID = getParameterByName('RID');
    var EncryptData = [transId];
    var encryData = Encrypt(EncryptData);
    transId = encryData[0];
    window.open('/JobMaterialListReport/JobMaterialListReport?UID=' + UID + '&UserRole=' + UserRole + '&id=' + ClientID + '&SiteId=' + SiteId + '&RID=' + RID + '&checkid=' + transId);

    $('#LoadingPanel1').hide();
    // string path = "/JobMaterialListReport.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&SiteId=" + databaseCon.Encrypt(ToId) + "&RID=" + databaseCon.Encrypt(FromId) + "&checkid=" + databaseCon.Encrypt(TranId);
}

function OpenDedliveryDocument(transId) {
    $('#LoadingPanel1').show();
    var Href = window.location.href.split('?');
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var ThirdParty = $('#rdoThirdPartytoDepot').prop('checked');

    var EncryptData = [transId, ThirdParty];
    var encryData = Encrypt(EncryptData);
    transId = encryData[0];
    ThirdParty = encryData[1];

    //window.location.href = '/DeliveryDocketDocument/DeliveryDocketDocument?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&TranId=' + transId;
    window.location.href = '/DeliveryDocketDocument/DeliveryDocketDocument?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&TranId=' + transId + '&ThirdParty=' + ThirdParty;

    $('#LoadingPanel1').hide();
}



function ShowDeletePopup(transId) {
    if (IsInactive(transId)) {
        CustomAlert({
            Result: "Failed", Message: "This site is InActive. You cannot delete requests."
        });
        return false;
    }
    $('#hdnTransID').val(transId);
    $("#divdelete").css("display", "block");
}

function btndeleteyes_Click() {
    $('#LoadingPanel1').show();
    var transID = $('#hdnTransID').val();
    var ClientID = getParameterByName('id');
    btndeleteyes_Click

    $.ajax({
        type: "POST",
        url: "/RequestInventory/btndeleteyes_Click",
        data: JSON.stringify({ ClientID: ClientID, transID: transID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            var Result = response;
            $("#divdelete").css("display", "none");
            CustomAlert(response);
            GetRequestInventoryData();
        }
    })
}
function btndeleteno_Click() {
    $("#divdelete").css("display", "none");
}



function btnnewrequest_Click1() {
    if ($("#ddlSite option:selected").text().indexOf("(InActive)") !== -1) {
        CustomAlert({
            Result: "Failed",
            Message: "This site is InActive. You cannot make any changes."
        });
        return false;
    }

    $('#LoadingPanel1').show();
    var Href = window.location.href.split('?');
    var siteID = $('#ddlSite option:selected').val();
    var rdoThirdPartytoDepot = $('#rdoThirdPartytoDepot').prop('checked');
    var rdodepottosite = $('#rdodepottosite').prop('checked');
    if (rdodepottosite == true) {
        if (siteID != "") {
            var id = getParameterByName('id');
            var UID = getParameterByName('UID');
            var UserRole = getParameterByName('UserRole');
            var UID = getParameterByName('UID');
            var ReqId = 0;
            var FromId = siteID;
            var ToId = id;
            var fromtocheck = 1;
            var from = "Site";
            var To = "Depot";
            var ReqStatus = "NotConfirmed";
            var page = 1;
            var EncryptData = [ReqId, FromId, fromtocheck, from, To, ReqStatus, page];
            var encryData = Encrypt(EncryptData);
            ReqId = encryData[0];
            FromId = encryData[1];
            fromtocheck = encryData[2];
            from = encryData[3];
            To = encryData[4];
            ReqStatus = encryData[5];
            page = encryData[6];
            // Response.Redirect("SiteRequest.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&ReqId=" + databaseCon.Encrypt("0") + " &FromId=" + databaseCon.Encrypt(ddlfrom.SelectedValue) + "&ToId=" + Request.QueryString["id"] + "&fromtocheck=" + databaseCon.Encrypt("1") + "&from=" + databaseCon.Encrypt("Site") + "&To=" + databaseCon.Encrypt("Depot") + "&ReqStatus=" + databaseCon.Encrypt("NotConfirmed") + "&page=" + databaseCon.Encrypt("1"), false);

            // window.location.href = '/SiteRequest/SiteRequest?' + Href[1];
            window.location.href = '/SiteRequest/SiteRequest?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&ReqId=' + ReqId + '&FromId=' + FromId + '&ToId=' + ToId + '&fromtocheck=' + fromtocheck + '&from=' + from + '&To=' + To + '&ReqStatus=' + ReqStatus + '&page=' + page;
            $('#LoadingPanel1').hide();
        }
        else {
            var Response = { Result: "Failed", Message: "Please Select the Site." };
            CustomAlert(Response);
            $('#LoadingPanel1').hide();
        }
    }
    else if (rdoThirdPartytoDepot == true) {
        var id = getParameterByName('id');
        var UID = getParameterByName('UID');
        var UserRole = getParameterByName('UserRole');
        var ReqId = 0;
        var FromId = id;
        var ToId = id;
        var fromtocheck = 3;
        var from = "Depot";
        var To = "ThirdParty";
        var ReqStatus = "NotConfirmed";
        var page = 3;
        var EncryptData = [ReqId, fromtocheck, from, To, ReqStatus, page];
        var encryData = Encrypt(EncryptData);
        ReqId = encryData[0];
        // FromId = encryData[1];
        fromtocheck = encryData[1];
        from = encryData[2];
        To = encryData[3];
        ReqStatus = encryData[4];
        page = encryData[5];

        window.location.href = '/SiteRequest/SiteRequest?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&ReqId=' + ReqId + '&FromId=' + FromId + '&ToId=' + ToId + '&fromtocheck=' + fromtocheck + '&from=' + from + '&To=' + To + '&ReqStatus=' + ReqStatus + '&page=' + page;
        $('#LoadingPanel1').hide();

    }

}


function btnrequest_Click() {
    if (IsSelectedRecordInactive(transId)) {
        CustomAlert({
            Result: "Failed", Message: "Selected record belongs to an InActive site. Edit is not allowed."
        });
        return false;
    }
    if ($("#ddlSite option:selected").text().indexOf("(InActive)") !== -1) {
        CustomAlert({
            Result: "Failed",
            Message: "This site is InActive. You cannot edit requests."
        });
        return false;
    }
    $('#LoadingPanel1').show();

    var count = 0, fromtocheck = 1, fromid = 0, toid = 0;

    var fromvalue = "", tovalue = ""; Status = "";

    var FromIdvalue = "", SiteSelection = "", JobSelection = "";
    var TempArry = [];

    //if ($('#rdodepottosite').prop('checked') == false) {
    //    fromtocheck = 2;
    //}
    var page = "1";
    if ($('#rdositetojob').prop('checked') == true) {
        fromtocheck = 2;
    }
    else if ($('#rdoThirdPartytoDepot').prop('checked') == true) {
        fromtocheck = 3;
        page = "3";
    }

    // var siteID = $('#ddlSite option:selected').val();
    for (var i = 0; i < ArrayIdxArr.length; i++) {
        TempArry = ArrayIdxArr[i];

    }
    var transId = TempArry[0];
    Status = TempArry[1];
    fromid = TempArry[2];
    toid = TempArry[3];
    fromvalue = TempArry[4];
    tovalue = TempArry[5];

    var reqstatus = RequestStatusGlobal;
    var reqLe = ArrayIdxArr.length;
    //var page = "1";

    if (Status == "") {
        Status = "NotConfirmed";
    }

    var EncryptData = [transId, Status, fromid, toid, fromvalue, tovalue, fromtocheck, page];
    var encryData = Encrypt(EncryptData);
    transId = encryData[0];
    Status = encryData[1];
    fromid = encryData[2];
    toid = encryData[3];
    fromvalue = encryData[4];
    tovalue = encryData[5];
    fromtocheck = encryData[6];
    page = encryData[7];

    if (reqLe == 1) {

        var id = getParameterByName('id');
        var UID = getParameterByName('UID');
        var UserRole = getParameterByName('UserRole');

        window.location.href = '/SiteRequest/SiteRequest?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&ReqId=' + transId + '&FromId=' + fromid + '&ToId=' + toid + '&fromtocheck=' + fromtocheck + '&from=' + fromvalue + '&To=' + tovalue + '&ReqStatus=' + Status + '&page=' + page;
        $('#LoadingPanel1').hide();
        // Response.Redirect("SiteRequest.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&ReqId=" + databaseCon.Encrypt(trid) + "&FromId=" + databaseCon.Encrypt(SiteSelection) + "&ToId=" + databaseCon.Encrypt(toid.ToString()) + "&fromtocheck=" + databaseCon.Encrypt(fromtocheck.ToString()) + "&from=" + databaseCon.Encrypt(fromvalue) + "&To=" + databaseCon.Encrypt(tovalue) + "&ReqStatus=" + databaseCon.Encrypt(Status) + "&page=" + databaseCon.Encrypt("1"), false);

    } else {
        var Response = { Result: "Failed", Message: "Please Select One Record ." };
        CustomAlert(Response);
        $('#LoadingPanel1').hide();
    }


}

function Delivery_Click() {
    if (IsSelectedRecordInactive()) {
        CustomAlert({
            Result: "Failed", Message: "Selected record belongs to an InActive site. Delivery is not allowed."
        });
        return false;
    }
    if ($("#ddlSite option:selected").text().indexOf("(InActive)") !== -1) {
        CustomAlert({
            Result: "Failed",
            Message: "This site is InActive. You cannot make deliveries."
        });
        return false;
    }

    $('#LoadingPanel1').show();
    var count = 0, fromtocheck = 1, fromid = 0, toid = 0;
    var FromIdvalue = "", SiteSelection = "", JobSelection = ""; fromvalue = "Site", tovalue = "Depot"; Status = "", DelStatus = "";


    var TempArry = [];

    //if ($('#rdodepottosite').prop('checked') == false) {
    //    fromtocheck = 2;
    //    fromvalue = "Job";
    //    tovalue = "Site";
    //}
    if ($('#rdositetojob').prop('checked') == true) {
        fromtocheck = 2;
        fromvalue = "Job";
        tovalue = "Site";
    }
    else if ($('#rdoThirdPartytoDepot').prop('checked') == true) {
        fromtocheck = 3;
        fromvalue = "Depot";
        tovalue = "ThirdParty";
    }

    var Href = window.location.href.split('?');
    // var siteID = $('#ddlSite option:selected').val();
    for (var i = 0; i < ArrayIdxArr.length; i++) {
        TempArry = ArrayIdxArr[i];

    }
    var transId = TempArry[0];
    Status = TempArry[1];
    fromid = TempArry[2];
    toid = TempArry[3];
    // fromvalue = TempArry[4];
    // tovalue = TempArry[5];
    var DeleveryStatus = TempArry[6];
    if (DeleveryStatus == "") {
        DelStatus = "NotConfirmed";
    }

    var reqstatus = RequestStatusGlobal;
    var CheckBoxLength = ArrayIdxArr.length;

    var EncryptData = [transId, Status, fromid, toid, fromvalue, tovalue, fromtocheck, DelStatus];
    var encryData = Encrypt(EncryptData);
    transId = encryData[0];
    Status = encryData[1];
    fromid = encryData[2];
    toid = encryData[3];
    fromvalue = encryData[4];
    tovalue = encryData[5];
    fromtocheck = encryData[6];
    DelStatus = encryData[7];

    if (CheckBoxLength == 1) {
        if (reqstatus == "Confirmed") {
            var id = getParameterByName('id');
            var UID = getParameterByName('UID');
            var UserRole = getParameterByName('UserRole');

            window.location.href = '/DeliveryInventory/DeliveryInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&checkid=' + transId + '&FromId=' + fromid + '&ToId=' + toid + '&fromtocheck=' + fromtocheck + '&from=' + fromvalue + '&To=' + tovalue + '&DelStatus=' + DelStatus;
            $('#LoadingPanel1').hide();
            //  Response.Redirect("DeliveryInventory.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&FromId=" + databaseCon.Encrypt(fromid.ToString()) + "&ToId=" + databaseCon.Encrypt(toid.ToString()) + "&from=" + databaseCon.Encrypt(fromvalue) + "&To=" + databaseCon.Encrypt(tovalue) + "&fromtocheck=" + databaseCon.Encrypt(fromtocheck.ToString()) + "&checkid=" + databaseCon.Encrypt(checkid) + "&DelStatus=" + databaseCon.Encrypt(DelStatus));

        } else {
            var Response = { Result: "Failed", Message: "Request Not Confirmed." };
            CustomAlert(Response);
            $('#LoadingPanel1').hide();
        }

    } else {
        var Response = { Result: "Failed", Message: "Please Select One Record ." };
        CustomAlert(Response);
        $('#LoadingPanel1').hide();
    }

}
function backtojob_Click() {
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var RID = getParameterByName('RID');

    window.location.href = '/ScaffoldMaterialListPhase2/ScaffoldMaterialListPhase2?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID;

    //  Response.Redirect("ScaffoldMaterialListPhase2.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RID=" + Request.QueryString["RID"]);
}
function ExportToReqCsvButton_Click() {
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
    // var rdodepottosite = "True";
    var rdoThirdPartytoDepot = $('#rdoThirdPartytoDepot').prop('checked');
    var rdodepottosite = $('#rdodepottosite').prop('checked');
    var rdositetojob = $('#rdositetojob').prop('checked');
    // var rdositetojob = "False";
    var JobSiteid = getParameterByName('Siteid');
    // var RID = getParameterByName('RID');
    var RID = getParameterByName('RID') == null ? "" : getParameterByName('RID');
    var ArrayData = [RID];
    var DecryptData = Decrypt(ArrayData);

    var RegisterID = DecryptData[0];


    window.location.href = '/RequestInventory/ExportToReqCsvButton_Click?id=' + ClientID + '&FromId=' + FromId + '&ToId=' + ToId + '&ddl1from=' + ddl1from + '&ddl1to=' + ddl1to + '&JobSiteid=' + JobSiteid + '&ddlfrom=' + ddlfrom + '&rdodepottosite=' + rdodepottosite + '&rdositetojob=' + rdositetojob + '&RID=' + RID + '&rdoThirdPartytoDepot=' + rdoThirdPartytoDepot;
}



function CheckboxEmailSend_Click() {

    if (IsSelectedRecordInactive()) {
        CustomAlert({
            Result: "Failed", Message: "Selected record belongs to an InActive site. Delivery is not allowed."
        });
        return false;
    }
    if ($("#ddlSite option:selected").text().indexOf("(InActive)") !== -1) {
        CustomAlert({
            Result: "Failed",
            Message: "This site is InActive. Email is not allowed."
        });
        return false;
    }


    // $('#LoadingPanel1').show();
    var chckEmail = 2;
    var reqstatus = RequestStatusGlobal;
    var CheckBoxLength = ArrayIdxArr.length;

    if (CheckBoxLength == 1) {
        if (reqstatus == "Confirmed") {
            btnEmailClick(chckEmail);

        } else {
            var Response = { Result: "Failed", Message: "Request Not Confirmed." };
            CustomAlert(Response);
            $('#LoadingPanel1').hide();
        }

    } else {
        var Response = { Result: "Failed", Message: "Please Select One Record ." };
        CustomAlert(Response);
        $('#LoadingPanel1').hide();
    }
}

function btnEmailClick(chckEmail) {

    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var TransId = getParameterByName('TransId');
    var EmailCheckTo = getParameterByName('To');
    var DomainName = window.location.origin;

    var count = 0, fromtocheck = 1, fromid = 0, toid = 0;
    //var FromIdvalue = "", SiteSelection = "", JobSelection = ""; fromvalue = "Site", tovalue = "Depot"; Status = "", DelStatus = "";
    var FromIdvalue = "", SiteSelection = "", JobSelection = ""; fromvalue = "Site", tovalue = "Depot"; Status = "", DelStatus = ""; TransID1 = "", StatusTrans = "", fromid = "", toid = "", To = "";


    var TempArry = [];

    //if ($('#rdodepottosite').prop('checked') == false) {
    //    fromtocheck = 2;
    //    fromvalue = "Job";
    //    tovalue = "Site";
    //}

    if ($('#rdositetojob').prop('checked') == true) {
        fromtocheck = 2;
        fromvalue = "Job";
        tovalue = "Site";
    }
    else if ($('#rdoThirdPartytoDepot').prop('checked') == true) {
        fromtocheck = 3;
        fromvalue = "Depot";
        tovalue = "ThirdParty";
    }
    if (Decrypt(EmailCheckTo) != "" && Decrypt(EmailCheckTo) == "ThirdParty") {
        fromtocheck = 3;
        fromvalue = "Depot";
        tovalue = "ThirdParty";
    }

    if (chckEmail != 1) {

        for (var i = 0; i < ArrayIdxArr.length; i++) {
            TempArry = ArrayIdxArr[i];

        }
        var TransID1 = TempArry[0];
        Status = TempArry[1];
        fromid = TempArry[2];
        toid = TempArry[3];

        var DeleveryStatus = TempArry[6];
        if (DeleveryStatus == "") {
            DelStatus = "NotConfirmed";
        }

        var EncryptData = [TransID1, Status, fromid, toid, fromvalue, tovalue, fromtocheck, DelStatus];
        var encryData = Encrypt(EncryptData);
        TransID1 = encryData[0];
        StatusTrans = encryData[1];
        fromid = encryData[2];
        toid = encryData[3];
        fromvalue = encryData[4];
        tovalue = encryData[5];
        fromtocheck = encryData[6];
        DelStatus = encryData[7];

    }
    $.ajax({
        type: "POST",
        url: "/RequestInventory/btnEmailClick",
        //data: JSON.stringify({ id: id, UID: UID, UserRole: UserRole, TransId: TransId, DomainName: DomainName, transId: transId, StatusTrans: StatusTrans, fromid: fromid, toid: toid, fromvalue: fromvalue, tovalue: tovalue, fromtocheck: fromtocheck, DelStatus: DelStatus, chckEmail: chckEmail }),
        data: { id: id, UID: UID, UserRole: UserRole, TransId: TransId, DomainName: DomainName, TransID1: TransID1, StatusTrans: StatusTrans, fromid: fromid, toid: toid, fromvalue: fromvalue, tovalue: tovalue, fromtocheck: fromtocheck, DelStatus: DelStatus, chckEmail: chckEmail, EmailCheckTo: EmailCheckTo },
        // contentType: "application/json; charset=utf-8",
        // dataType: "json",
        async: false,
        success: function (response) {
            if (response.Result == "Success") {
                var txtFile = response.txtFile;
                var txttempfile = response.txttempfile;
                var txtcc = response.txtcc;
                var txtClientEmail = response.txtClientEmail;
                var txtsubject = response.txtsubject;
                var txtEmailMSg = response.txtEmailMSg;
                $('#txtFile').val(txtFile);
                $('#txttempfile').val(txttempfile);
                $('#txtcc').val(txtcc);
                $('#txtClientEmail').val(txtClientEmail);
                $('#txtsubject').val(txtsubject);
                $('#txtEmailMSg').text(txtEmailMSg);
                if (chckEmail != 1) {
                    $('#hdnEmailTransID').val(TransID1);
                }
                else {
                    //var encryTransid = Decrypt(TransId);
                    //TransID2= encryTransid[0];
                    $('#hdnEmailTransID').val(TransId);
                }

                $('#divEmailConfirm').css('display', 'block');

            } else {
                var Response = { Result: "Failed", Message: "Please Send Email again. " };
                CustomAlert(response);
                $('#LoadingPanel1').hide();
            }

            if ((txtClientEmail == null || txtClientEmail == "") && (Decrypt(tovalue) == "ThirdParty" || tovalue == "ThirdParty")) {
                $('#alertlabel2').css("display", "block");
                $('#alertlabel2').html('<Strong style="color:Red">To Auto Fill The Email Address Please Fill Inventory Mailing List On The Company Profile Screen </Strong>')

            }

            else if ((txtClientEmail == null || txtClientEmail == "") && (Decrypt(tovalue) == "Depot" || tovalue == "Depot")) {
                $('#alertlabel2').css("display", "block");
                $('#alertlabel2').html('<Strong style="color:Red">To Auto Fill The Email Address Please Fill Inventory Mailing List On The Company Profile in Site Screen </Strong>')

            }
            else {
                $('#alertlabel2').css("display", "none");
                $('#alertlabel2').html('')

            }
        }
    });
    // }, 3000);
}


function btnSend_Click() {
    $('#LoadingPanel1').show();
    var txtClientEmail = $('#txtClientEmail').val();
    var txtcc = $('#txtcc').val();
    var txtsubject = $('#txtsubject').val();
    var txtEmailMSg = $('#txtEmailMSg').text();
    var txtFile = $('#txtFile').val();
    var EmailTransID = $('#hdnEmailTransID').val();
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var TransId = getParameterByName('TransId');
    var txtEmailAdditionalText = $('#txtEmailAdditionalText').val();
    var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if (txtClientEmail === "" || !emailRegex.test(txtClientEmail)) {
        $('#txtClientEmail').css("border", "1px solid red");
        var Response = { Result: "Failed", Message: "Please enter a valid Email address (e.g., name@example.com)." }
        CustomAlert(Response);
        $('#LoadingPanel1').hide();
        return;
    }


    $.ajax({
        type: "POST",
        url: "/RequestInventory/btnSend_Click",
        data: JSON.stringify({ txtClientEmail: txtClientEmail, txtcc: txtcc, txtsubject: txtsubject, txtEmailMSg: txtEmailMSg, txtFile: txtFile, EmailTransID: EmailTransID, txtEmailAdditionalText: txtEmailAdditionalText }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            if (response.Result == "Success") {
                CustomAlert(response);
                $('#LoadingPanel1').hide();
                $('#divEmailConfirm').css('display', 'none');
                // GetInspectionListData();
                var EmailFlag = "0";
                window.location.href = '/RequestInventory/RequestInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&TransId=' + TransId + '&EmailFlag=' + EmailFlag;
            } else {
                CustomAlert(response);
                $('#LoadingPanel1').hide();
                $('#divEmailConfirm').css('display', 'none');
            }

        }
    });

}

function btnCancel_Click() {
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var TransId = getParameterByName('TransId');
    var EmailFlag = getParameterByName('EmailFlag');
    if (EmailFlag == "1") {
        var EmailFlag1 = "0";
        $('#divEmailConfirm').css('display', 'none');
        window.location.href = '/RequestInventory/RequestInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&TransId=' + TransId + '&EmailFlag=' + EmailFlag1;
    }
    else {
        $('#divEmailConfirm').css('display', 'none');
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

//Added on 06-06-2023 to Add the Edit Functionality On Request Inventory List Page To Enable Editing For Supplier,Comment and Description Columns
//function Editing(d) {
//    var id = getParameterByName('id');
//    $.ajax({
//        type: "POST",
//        url: "/SiteRequest/dropdownSupplier",
//        data: JSON.stringify({ id: id }),
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        async: true,
//        success: function (response) {

//            var Supplier = response;
//            if (Supplier.length > 0) {
//                for (var m = 0; m < Supplier.length; m++) {
//                    var mat = Supplier[m].Supplier;
//                    var rowmat = "";
//                    if (d == 0) {
//                        rowmat = "0";
//                    }
//                    else {
//                        rowmat = d[10];
//                    }
//                    if (mat != rowmat) {
//                        $("#ddlSupplier").append("<option value='" + Supplier[m].Supplier + "'>" + Supplier[m].Supplier + "</option>");
//                    }
//                }
//            }
//        },
//        complete: function () {
//            $(".CancelChangeClass").click(function () {
//                $(this).parent().parent().parent().parent().prev('tr').removeClass('Edit');
//                $(this).parent().parent().parent().parent().remove();
//            });

//            $("#savechange").click(function () {
//                $('#LoadingPanel1').show();
//                var transID = d[5];
//                var ClientID = getParameterByName('id');
//                var Supplier = $("#ddlSupplier").val();
//                var TransDesc = $("#txtDiscription").val();
//                var Comment = $("#txtcomment").val();

//                $.ajax({
//                    type: "POST",
//                    url: "/RequestInventory/btnEdityes_Click",
//                    data: JSON.stringify({ ClientID: ClientID, transID: transID, Supplier: Supplier, TransDesc: TransDesc, Comment: Comment }),
//                    contentType: "application/json; charset=utf-8",
//                    dataType: "json",
//                    async: true,
//                    success: function (response) {
//                        var Result = response;

//                        CustomAlert(response);
//                        GetRequestInventoryData();
//                    }
//                })
//            });
//        }
//    });
//    if (d == 0) {
//        return '<div class="col-lg-10">'
//            + '<div class="col-lg-2">'
//            + ' Supplier<a style="color:red">*</a><select id="ddlSupplier" style="width:100%" class="form-control" required><option id="1" value="' + d[10] + '">' + d[10] + '</option></select></div>'
//            + '<div class="col-lg-4">Comment <a style="color:red">*</a><input id="txtcomment" type="text" class="form-control" value="' + d[25] + '" style="width:100%" required/></div>'
//            + '<div class="col-lg-4">Description <a style="color:red">*</a><input id="txtDiscription" type="text" class="form-control" value="' + d[11] + '" style="width:100%" required/>'
//            + '</div></div>'
//            + '<div class="col-lg-8">'
//            + '<div class="col-lg-1">'
//            + '<input type="button" id="savechange" class="btn btn-primary" value="Save" style="color:white;margin-top:6px;width:100%"/>'
//            + '<input type="submit" id="btnsubmit" class="btn btn-primary" value="Save" style="color:white;margin-top:6px;width:100%;display:none"/>'
//            + '</div><div class="col-lg-1">'
//            + '<input type="button" id="cancelchange" class="btn CancelChangeClass" value="Cancel" style="color:black;background: lightgrey;margin-top:6px;width:100%"/>'
//            + '</div><div class="col-lg-6"></div><div class="col-lg-2">'
//            + '<input id="psudouserid" type="text" class="form-control" value="" style="width:1% ; margin-top:6px; float:right;opacity:0" />'
//            + '</div></div>'

//    }
//    else {
//        return '<div class="col-lg-10">'
//            + '<div class="col-lg-1">'
//            + ' Supplier<a style="color:red">*</a><select id="ddlSupplier" style="width:100%" class="form-control" required><option id="1" value="' + d[10] + '">' + d[10] + '</option></select></div>'
//            + '<div class="col-lg-4">Comment <a style="color:red">*</a><input id="txtcomment" type="text" class="form-control" value="' + d[25] + '" style="width:100%" required/></div>'
//            + '<div class="col-lg-4">Description <a style="color:red">*</a><input id="txtDiscription" type="text" class="form-control" value="' + d[11] + '" style="width:100%" required/>'
//            + '</div></div>'
//            + '<div class="col-lg-8">'
//            + '<div class="col-lg-1">'
//            + '<input type="button" id="savechange" class="btn btn-primary" value="Save" style="color:white;margin-top:6px;width:100%"/>'
//            + '<input type="submit" id="btnsubmit" class="btn btn-primary" value="Save" style="color:white;margin-top:6px;width:100%;display:none"/>'
//            + '</div><div class="col-lg-1">'
//            + '<input type="button" id="cancelchange" class="btn CancelChangeClass" value="Cancel" style="color:black;background: lightgrey;margin-top:6px;width:100%"/>'
//            + '</div><div class="col-lg-6"></div><div class="col-lg-2">'
//            + '<input id="psudouserid" type="text" class="form-control" value="" style="width:1% ; margin-top:6px; float:right;opacity:0" />'
//            + '</div></div>'

//    }
//}
function Editing(d) {
    var id = getParameterByName('id');
    $.ajax({
        type: "POST",
        url: "/SiteRequest/dropdownSupplier",
        data: JSON.stringify({ id: id }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {

            var Supplier = response;
            if (Supplier.length > 0) {
                for (var m = 0; m < Supplier.length; m++) {
                    var mat = Supplier[m].Supplier;
                    var rowmat = "";
                    if (d == 0) {
                        rowmat = "0";
                    }
                    else {
                        rowmat = d.Supplier;
                    }
                    if (mat != rowmat) {
                        $("#ddlSupplier").append("<option value='" + Supplier[m].Supplier + "'>" + Supplier[m].Supplier + "</option>");
                    }
                }
            }
        },
        complete: function () {
            $(".CancelChangeClass").click(function () {
                $(this).parent().parent().parent().parent().prev('tr').removeClass('Edit');
                $(this).parent().parent().parent().parent().remove();
            });

            $("#savechange").click(function () {
                $('#LoadingPanel1').show();
                var transID = d.transId;
                var ClientID = getParameterByName('id');
                var Supplier = $("#ddlSupplier").val();
                var TransDesc = $("#txtDiscription").val();
                var Comment = $("#txtcomment").val();

                $.ajax({
                    type: "POST",
                    url: "/RequestInventory/btnEdityes_Click",
                    data: JSON.stringify({ ClientID: ClientID, transID: transID, Supplier: Supplier, TransDesc: TransDesc, Comment: Comment }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true,
                    success: function (response) {
                        var Result = response;

                        CustomAlert(response);
                        GetRequestInventoryData();
                    }
                })
            });
        }
    });
    if (d == 0) {
        return '<div class="col-lg-10">'
            + '<div class="col-lg-2">'
            + ' Supplier<a style="color:red">*</a><select id="ddlSupplier" style="width:100%" class="form-control" required><option id="1" value="' + d.Supplier + '">' + d.Supplier + '</option></select></div>'
            + '<div class="col-lg-4">Comment <a style="color:red">*</a><input id="txtcomment" type="text" class="form-control" value="' + d.Comment + '" style="width:100%" required/></div>'
            + '<div class="col-lg-4">Description <a style="color:red">*</a><input id="txtDiscription" type="text" class="form-control" value="' + d.TransDesc + '" style="width:100%" required/>'
            + '</div></div>'
            + '<div class="col-lg-8">'
            + '<div class="col-lg-1">'
            + '<input type="button" id="savechange" class="btn btn-primary" value="Save" style="color:white;margin-top:6px;width:100%"/>'
            + '<input type="submit" id="btnsubmit" class="btn btn-primary" value="Save" style="color:white;margin-top:6px;width:100%;display:none"/>'
            + '</div><div class="col-lg-1">'
            + '<input type="button" id="cancelchange" class="btn CancelChangeClass" value="Cancel" style="color:black;background: lightgrey;margin-top:6px;width:100%"/>'
            + '</div><div class="col-lg-6"></div><div class="col-lg-2">'
            + '<input id="psudouserid" type="text" class="form-control" value="" style="width:1% ; margin-top:6px; float:right;opacity:0" />'
            + '</div></div>'

    }
    else {
        return '<div class="col-lg-10">'
            + '<div class="col-lg-1">'
            + ' Supplier<a style="color:red">*</a><select id="ddlSupplier" style="width:100%" class="form-control" required><option id="1" value="' + d.Supplier + '">' + d.Supplier + '</option></select></div>'
            + '<div class="col-lg-4">Comment <a style="color:red">*</a><input id="txtcomment" type="text" class="form-control" value="' + d.Comment + '" style="width:100%" required/></div>'
            + '<div class="col-lg-4">Description <a style="color:red">*</a><input id="txtDiscription" type="text" class="form-control" value="' + d.TransDesc + '" style="width:100%" required/>'
            + '</div></div>'
            + '<div class="col-lg-8">'
            + '<div class="col-lg-1">'
            + '<input type="button" id="savechange" class="btn btn-primary" value="Save" style="color:white;margin-top:6px;width:100%"/>'
            + '<input type="submit" id="btnsubmit" class="btn btn-primary" value="Save" style="color:white;margin-top:6px;width:100%;display:none"/>'
            + '</div><div class="col-lg-1">'
            + '<input type="button" id="cancelchange" class="btn CancelChangeClass" value="Cancel" style="color:black;background: lightgrey;margin-top:6px;width:100%"/>'
            + '</div><div class="col-lg-6"></div><div class="col-lg-2">'
            + '<input id="psudouserid" type="text" class="form-control" value="" style="width:1% ; margin-top:6px; float:right;opacity:0" />'
            + '</div></div>'

    }
}
function IsInactive(transId) {
    var siteId = $("#ddlSite").val();
    if (siteId != "" && siteId != "All") {
        return $("#ddlSite option:selected").text().indexOf("(InActive)") !== -1;
    }
    var fromText = $("#From_" + transId).attr("title");

    if (!fromText) {
        fromText = $("#From_" + transId).text();
    }
    return fromText.indexOf("(InActive)") !== -1;
}
function IsSelectedRecordInactive() {
    var TempArry = [];
    for (var i = 0; i < ArrayIdxArr.length; i++) {
        TempArry = ArrayIdxArr[i];
    }
    if (!TempArry || TempArry.length == 0)
        return false;
    var transId = TempArry[0];
    return IsInactive(transId);
}




