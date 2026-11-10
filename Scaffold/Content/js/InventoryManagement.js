
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
            $('#LoadingPanel1').css('display', 'none');
        }

        if (ClickAcnhorID == 'AnchorReturnInventory') {
            // window.location.href = '/ReturnInventory.aspx?' + Href[1];
            //window.location.href = '/ReturnInventory/ReturnInventory?' + Href[1];
            window.location.href = '/ReturnInventory/ReturnInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
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
        if (ClickAcnhorID == 'AnchorSiteInventory') {
            // window.location.href = '/SiteInventory.aspx?' + Href[1];
            //window.location.href = '/SiteInventory/SiteInventory?' + Href[1];
            window.location.href = '/SiteInventory/SiteInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
        if (ClickAcnhorID == 'AnchorDamagedInventory') {
            // window.location.href = '/DamagedInventory.aspx?' + Href[1];
            //window.location.href = '/DamagedInventory/DamagedInventory?' + Href[1];
            window.location.href = '/DamagedInventory/DamagedInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
        if (ClickAcnhorID == 'AnchorPipeLineInventory') {
            //window.location.href = '/ReturnInventory/ReturnInventory?' + Href[1];
            window.location.href = '/PipeLineInventory/PipeLineInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        }
    });
    DdlMaterialType();
    GetTotalTotalTonnage(ClientID, "All");
    $('#btnaddinventorysss').on('click', function () {
        $('#btnaddinventory').trigger('click');
    });
});
    function closeAllOpenRows() {
        table.rows('.shown').every(function () {
            var editRow = this;
            editRow.child.hide();
            $(editRow.node()).removeClass('shown');
            $(editRow.node()).find('.fa-angle-down').removeClass('fa-angle-down').addClass('fa-angle-right');
        });
        table.rows('.Edit').every(function () {
            var editRow = this;
            editRow.child.hide();
            $(editRow.node()).removeClass('Edit');
        });
    }

    var table;

    function GetInventoryData(id, MaterialType) {
        $("#LoadingPanel1").css("display", "block");
        PipeInventoryHide(id);
        $.ajax({
            type: "POST",
            // url: "../WebApi/AllInventory.asmx/getinvData",
            url: "/InventoryManagement/GetInventoryData",
            data: JSON.stringify({ id: id, MaterialType: MaterialType }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                //  var getdata = response.d;
                var getdata = response;
                var str = "";
                var str1 = "";
                str += '<table id="dataforgrid" class="table table-striped display" style="width:120%;table-layout:fixed;font-size:12px">';
                str += "<thead><tr>";
                str1 += '<table id="dataforgrid" class="table table-striped display" style="width:120%;table-layout:fixed;font-size:12px">';
                str1 += "<thead><tr>";
                str += "<th style='width:2%' id='headerExpend' class='ClsheaderExpend'>#</th>";
                str += "<th style='width:2%' id='headEdit' class='ClsheadEdit'>##</th>";
                str += "<th  style='text-align:center;display:none'>InventoryId</th>";
                str += "<th  style='text-align:center;width:10%'>Material Type</th>";
                str1 += "<th  style='text-align:center;'>MaterialType</th>";
            str += "<th  style='text-align:center;width:10%'>Item Code</th>";
            str1 += "<th  style='text-align:center;'>ItemCode</th>";
                str += "<th  style='text-align:left;width:30%'>Item Description</th>";
                str1 += "<th  style='text-align:center;'>ItemDesc</th>";
                str += "<th  style='text-align:center;width:7%'>Quantity</th>";
                str1 += "<th  style='text-align:center;'>Quantity</th>";
                str += "<th  style='text-align:center;width:6%'>Damaged Quantity</th>";
                str += "<th  style='text-align:center;width:6%'>Item Weight</th>";
                str1 += "<th  style='text-align:center;'>ItemWeight</th>";
                str += "<th  style='text-align:center;width:6%'>Total Weight</th>";
                str += "<th  style='text-align:center;width:7%'>Available Quantity</th>";
                str += "<th style='display:none'>Comments</th>";
                str += "<th  style='text-align:center;width:7%'>Labour Erect</th>";
                str1 += "<th  style='text-align:center;'>LabourErect</th>";
                str += "<th  style='text-align:center;width:7%'>Labour Dismantle</th>";
                str1 += "<th  style='text-align:center;'>LabourDismantle</th>";
                str += "<th  style='text-align:center;display:none'>DisplaySortOrder</th>";
                str1 += "<th  style='text-align:center;display:none'>DisplaySortOrder</th>";
                str += "<th  style='display:none'>SubCategoryID</th>"; // Add On 17 March 2020

                // Add Code On 27/March/2020.
                if ($("#hdnhire").val() == "true") {
                    str += "<th  style='text-align:center;width:10%; '>Sub Category</th></tr></thead><tbody>";    // Add On 07 March 2020
                    str1 += "<th  style='text-align:center;'>SubCategory</th></tr></thead><tbody>";
                }
                else {
                    str += "<th  style='text-align:center;width:10%; display:none;'>Sub Category</th></tr></thead><tbody>";
                    str1 += "<th  style='text-align:center;display:none;'></th></tr></thead><tbody>";
                }

                //  str += "<th  style='text-align:center;width:10%'>Sub Category</th></tr></thead><tbody>";    // Add On 07 March 2020
                //  str1 += "<th  style='text-align:center;'>SubCategory</th></tr></thead><tbody>";             // Add On 07 March 2020
                if (getdata.length > 0) {
                    for (var i = 0; i < getdata.length; i++) {
                        var InventoryID = getdata[i].InventoryID == null ? "" : getdata[i].InventoryID;
                        var MaterialType = getdata[i].MaterialType == null ? "" : getdata[i].MaterialType;
                        var ItemCode = getdata[i].ItemCode == null ? "" : getdata[i].ItemCode;
                        var ItemDesc = getdata[i].ItemDesc == null ? "" : getdata[i].ItemDesc;
                        var OrgYardQuantity = getdata[i].OrgYardQuantity == null ? "" : getdata[i].OrgYardQuantity;
                        var TotalDamagedQuentity = getdata[i].TotalDamagedQuentity == null ? 0 : getdata[i].TotalDamagedQuentity;
                        var ItemWeight = getdata[i].ItemWeight == null ? "" : getdata[i].ItemWeight;
                        var AvailableQuantity = getdata[i].AvailableQuantity == null ? "" : getdata[i].AvailableQuantity;
                        var Comments = getdata[i].Comments == null ? "" : getdata[i].Comments;
                        var TotalWeight = ItemWeight * parseInt(AvailableQuantity);
                        //  var LabourErect = getdata[i].LabourErect == 0 ? "" : getdata[i].LabourErect.toFixed(4);
                        //  var LabourDismantle = getdata[i].LabourDismantle == 0 ? "" : getdata[i].LabourDismantle.toFixed(4);
                        var LabourErect = getdata[i].LabourErect == null ? "" : getdata[i].LabourErect.toFixed(4);
                        var LabourDismantle = getdata[i].LabourDismantle == null ? "" : getdata[i].LabourDismantle.toFixed(4);
                        var SubCategoryID = getdata[i].SubCategoryId == null ? "" : getdata[i].SubCategoryId; // Add On 17 March 2020
                        var SubCategory = getdata[i].SubCategory == null ? "" : getdata[i].SubCategory;  // Add on 07 March 2020
                        var DisplaySortOrder = getdata[i].DisplaySortOrder == null ? "" : getdata[i].DisplaySortOrder;  // Add on 07 March 2020
                        if (i == 0) {
                            str += "<tr id='row_9999' style='display:none'><td  style='width:2%;'><a class='expand fa fa-angle-right' style='font-size:19px;'></a></td>";
                            str += "<td  style='width:2%;'><a id='rowpencil' class='expand fa fa-pencil'></a></td>";
                            str += "<td  style='text-align:center;display:none'>0</td>";
                            str += "<td  style='text-align:center;width:10%'></td>";
                            str += "<td  style='text-align:center;width:10%'></td>";
                            str += "<td  style='text-align:center;width:30%'></td>";
                            str += "<td  style='text-align:center;width:6%'></td>";
                            str += "<td  style='text-align:center;width:7%'></td>";
                            str += "<td  style='text-align:center;width:6%'></td>";
                            str += "<td  style='text-align:center;width:7%'></td>";
                            str += "<td  style='text-align:center;width:6%'></td>";
                            str += "<td  style='text-align:center;display:none'></td>";
                            str += "<td  style='text-align:center;width:7%'></td>";
                            str += "<td  style='text-align:center;width:7%'></td>";
                            str += "<td  style='text-align:center;display:none;width:7%'></td>";
                            str += "<td  style='text-align:center;display:none'></td>"; // Add On 17 March 2020
                            // Add Code On 27/March/2020.
                            if ($("#hdnhire").val() == "true") {
                                str += "<td  style='text-align:center;width:10%'></td></tr>"; //Add on 07 March 2020
                            }
                            else {
                                str += "<td  style='text-align:center;width:10%;display:none;'></td></tr>";
                            }
                            str1 += "<td  style='text-align:center;'></td>";
                            str1 += "<td  style='text-align:center;'></td>";
                            str1 += "<td  style='text-align:center;'></td>";
                            str1 += "<td  style='text-align:center;'></td>";
                            str1 += "<td  style='text-align:center;'></td>";
                            str1 += "<td  style='text-align:center;'></td>";
                            str1 += "<td  style='text-align:center;'></td>";
                            str1 += "<td  style='text-align:center;'></td>";
                            // Add Code On 01/June/2020.
                            if ($("#hdnhire").val() == "true") {
                                str1 += "<td  style='text-align:center;'></td></tr>";// Add on 07 March 2020
                            }
                            else {
                                str1 += "<td  style='text-align:center;display:none;'></td></tr>";
                            }
                        }
                        str += "<tr id='row_" + i + "' ><td  style='width:2%;' class='ClsheaderExpend'><a class='expand fa fa-angle-right' style='font-size:19px;'></a></td>";
                        str += "<td  style='width:2%;' class='ClsheadEdit'><a id='rowpencil' class='expand fa fa-pencil'></a>&nbsp;&nbsp;<a id='rowdelete' class='fa fa-trash'></a></td>";
                        str += "<td  style='text-align:center;display:none'>" + InventoryID + "</td>";
                        str += "<td  style='text-align:center;width:10%'>" + MaterialType + "</td>";
                        str1 += "<td  style='text-align:center;'>" + MaterialType + "</td>";
                    str += "<td  style='text-align:center;width:10%'>" + ItemCode + "</td>";
                    str1 += "<td  style='text-align:center;'>" + ItemCode + "</td>";
                        str += "<td  style='text-align:left;width:30%'>" + ItemDesc + "</td>";
                        str1 += "<td  style='text-align:center;'>" + ItemDesc + "</td>";
                        str += "<td  style='text-align:center;width:7%'>" + numberWithCommas(OrgYardQuantity) + "</td>";
                        str1 += "<td  style='text-align:center;'>" + numberWithCommas(OrgYardQuantity) + "</td>";
                        str += "<td  style='text-align:center;width:6%'>" + numberWithCommas(TotalDamagedQuentity) + "</td>";
                        str += "<td  style='text-align:center;width:6%'>" + numberWithCommas(ItemWeight.toFixed(2)) + "</td>";
                        str1 += "<td  style='text-align:center;'>" + numberWithCommas(ItemWeight.toFixed(2)) + "</td>";
                        str += "<td  style='text-align:center;width:6%'>" + numberWithCommas(TotalWeight.toFixed(2)) + "</td>";
                        str += "<td  style='text-align:center;width:7%'>" + numberWithCommas(AvailableQuantity) + "</td>";
                        str += "<td  style='text-align:center;display:none'>" + Comments + "</td>";
                        str += "<td  style='text-align:center;width:7%'>" + LabourErect + "</td>";
                        str1 += "<td  style='text-align:center;'>" + LabourErect + "</td>";
                        str += "<td  style='text-align:center;width:7%'>" + LabourDismantle + "</td>";
                        str1 += "<td  style='text-align:center;'>" + LabourDismantle + "</td>";
                        str += "<td  style='text-align:center;display:none'>" + DisplaySortOrder + "</td>";
                        str1 += "<td  style='text-align:center;display:none'>" + DisplaySortOrder + "</td>";

                        str += "<td  style='text-align:center;display:none'>" + SubCategoryID + "</td>";// Add On 17 March 2020
                        //  str += "<td  style='text-align:center;width:10%'>" + SubCategory + "</td></tr>";  // Add on 07 March 2020
                        //  str1 += "<td  style='text-align:center;'>" + SubCategory + "</td></tr>";  // Add on 07 March 2020

                        // Add Code On 01/June/2020.
                        if ($("#hdnhire").val() == "true") {
                            str += "<td  style='text-align:center;width:10%;'>" + SubCategory + "</td></tr>";  // Add on 07 March 2020
                            str1 += "<td  style='text-align:center;'>" + SubCategory + "</td></tr>";  // Add on 07 March 2020
                        }
                        else {
                            str += "<td  style='text-align:center;width:10%;display:none;'></td></tr>";
                            str1 += "<td  style='text-align:center;display:none;'></td></tr>";
                        }
                    }
                    str += "</tbody></table>";
                    str1 += "</tbody></table>";
                    $("#loaddata").html(str);
                    $("#hdnTableData").val(str1);

                    $("#LoadingPanel1").css("display", "none");

                    $(".fa-trash").click(function () {
                        var inventry = $(this).parent().parent().children()[2].innerText;
                        $("#hdndeleteinv").val(inventry);
                        // $("#ContentPlaceHolder1_divdelete").css("display", "block");
                        $("#divdelete").css("display", "block");
                    });
                    //$(".fa-pencil").click(function () {
                    //    var visible = $(this).parent().parent().hasClass("Edit");
                    //    var tr = $(this).closest('tr');
                    //    var row = table.row(tr);
                    //    if (visible == true) {
                    //        $(this).parent().parent().next('tr').remove();
                    //        $(this).parent().parent().removeClass('Edit');
                    //    }
                    //    else {
                    //        row.child(Editing(row.data())).show();
                    //        row.child(BindSubCategory(row.data())).show();//Add code on 12-March-2020.
                    //        $('#txtComments').val("");
                    //        var oldqty = $('#txtquantity').val();
                    //        oldqty = oldqty.replaceAll(",", "");
                    //        $('#hdnOldQty').val(oldqty);
                    //        row.child.show();
                    //        tr.addClass('Edit');
                    //    }
                    //    // Add Code on 27-March-2020.
                    //    if ($("#hdnhire").val() == "true") {
                    //        $("#divEditSubcategory").css("display", "block");
                    //    }

                    //});
                    $('.fa-pencil').click(function () {
                        var tr = $(this).closest('tr');
                        var row = table.row(tr);
                        var isCurrentlyEditing = tr.hasClass('Edit');

                        closeAllOpenRows();

                        if (!isCurrentlyEditing) {
                            row.child(Editing(row.data())).show();
                            row.child(BindSubCategory(row.data())).show();
                            $('#txtComments').val("");

                            var oldqty = $('#txtquantity').val();
                            if (oldqty) {
                                oldqty = oldqty.replaceAll(",", "");
                                $('#hdnOldQty').val(oldqty);
                            }

                            tr.addClass('Edit');
                        }

                        if ($("#hdnhire").val() == "true") {
                            $("#divEditSubcategory").css("display", "block");
                        }
                    });

                    table = $('#dataforgrid').DataTable({
                        "bSort": false,
                        "processing": true,
                        searching: true,
                        info: false,
                        "pageLength": 50,
                        //paging: false,
                        autoWidth: true,
                        scrollX: true,
                        scrollCollapse: true,
                        scrollY: "60vh",
                        fixedHeader: true,
                        fixedColumns: {
                            leftColumns: 11,
                        },

                        initComplete: function () {
                            $(this.api().table().container()).find('input').parent().wrap('<form>').parent().attr('autocomplete', 'off'); // Auto Fill In Text box when ID Or Password Save on Crome Browser
                            //$("#dataforgrid_length").children().html('<button id="btnaddinventory" type="button" class="w3-btn w3-teal fa fa-plus">&nbsp;Add New Inventory Item</button><button id="ExportToCsvButton" type="button" class="w3-btn w3-teal Export" style="margin-left:23px;height:26px;padding-top:3px"><i class="fa fa-file-excel-o"></i>&nbsp;Export to CSV</button>');
                            $("#dataforgrid_length").children().html(
                                '<button id="btnaddinventory" type="button" class="w3-btn w3-teal fa fa-plus" style="display:None;"> Add New Inventory Item</button>' +
                                '<button id="ExportToCsvButton" type="button" class="w3-btn w3-teal Export" style="margin-left:23px;height:26px;padding-top:3px; display:None;">' +
                                '<i class="fa fa-file-excel-o"></i> Export to CSV</button>'
                            );
                            //$("#btnaddinventory").click(function () {
                            //    var searchVal = ($('#dataforgrid_filter input').val());
                            //    closeAllOpenRows();
                            //    if (searchVal != "") {
                            //        $("#divSave").css("display", "block");
                            //    } else {
                            //        var trow1 = $(this).parent().parent().siblings();
                            //        var trow = $(this).parent().parent().siblings()[2].children[1].children[0].children[1].childNodes[0];
                            //        var row = table.row(trow);
                            //        var c = 0;
                            //        row.child(Editing(c)).show();
                            //        row.child(BindSubCategory(c)).show();//Add code on 12-March-2020.
                            //        var oldqty = $('#txtquantity').val();
                            //        $('#hdnOldQty').val(oldqty);
                            //        //table.row.add(Editing(c)).draw();
                            //        // trow.addClass('Edit');
                            //    }
                            //    // Add Code on 01-June-2020.
                            //    if ($("#hdnhire").val() == "true") {
                            //        $("#divEditSubcategory").css("display", "block");
                            //    }
                            //});
                            $("#btnaddinventory").click(function () {
                                var searchVal = ($('#dataforgrid_filter input').val());
                                closeAllOpenRows();
                                if (searchVal != "") {
                                    $("#divSave").css("display", "block");
                                } else {
                                    var trow1 = $(this).parent().parent().siblings();
                                    var trow = $(this).parent().parent().siblings()[2].children[1].children[0].children[1].childNodes[0];
                                    var row = table.row(trow);
                                    var c = 0;
                                    row.child(Editing(c)).show();
                                    row.child(BindSubCategory(c)).show();
                                    var oldqty = $('#txtquantity').val();
                                    $('#hdnOldQty').val(oldqty);
                                    //table.row.add(Editing(c)).draw();
                                    // trow.addClass('Edit');
                                    $(row.node()).addClass('Edit');
                                }
                                if ($("#hdnhire").val() == "true") {
                                    $("#divEditSubcategory").css("display", "block");
                                }
                            });
                            $(".Export").click(function () {
                                $("#LoadingPanel1").css("display", "block");
                                fnExcelReport();
                            });
                        },
                        "columnDefs": [
                            { "width": "100px", "targets": 'fullsize' }
                        ],
                        "drawCallback": function () {

                            //$('.fa-angle-right').click(function (eve) {
                            //    eve.stopImmediatePropagation();
                            //    var tr = $(this).closest('tr');
                            //    var row = table.row(tr);
                            //    var visible = $(this).parent().parent().hasClass("shown");
                            //    var anglechange = $(this).hasClass("fa-angle-right");
                            //    if (anglechange == true) {
                            //        var x = tr.children()[0];
                            //        $(this).removeClass("fa-angle-right");
                            //        $(this).addClass("fa-angle-down");
                            //    }
                            //    else {
                            //        $(this).addClass("fa-angle-right");
                            //        $(this).removeClass("fa-angle-down");
                            //    }
                            //    if (visible == true) {
                            //        // This row is already open - close it
                            //        $(this).parent().parent().next('tr').remove();
                            //        $(this).parent().parent().removeClass('shown');
                            //    }
                            //    else {
                            //        // Open this row
                            //        row.child(format(row.data())).show();
                            //        row.child.show();
                            //        tr.addClass('shown');
                            //    }
                            //});
                            $('.fa-angle-right').click(function (eve) {
                                eve.stopImmediatePropagation();
                                var tr = $(this).closest('tr');
                                var row = table.row(tr);
                                var visible = tr.hasClass("shown");
                                var anglechange = $(this).hasClass("fa-angle-right");

                                closeAllOpenRows();

                                if (!visible) {
                                    if (anglechange) {
                                        $(this).removeClass("fa-angle-right").addClass("fa-angle-down");
                                    }
                                    row.child(format(row.data())).show();
                                    tr.addClass('shown');
                                }
                            });

                        }
                    });

                    // scrolling Top when Click On Next Button

                    table.on('page.dt', function () {
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
                    });
                    $("#LoadingPanel1").css("display", "none");
                }

                // Hide Show For DataTable Column And Buttons Start Here

                GridButtonHideShow(table, "dataforgrid");

                // Hide Show For DataTable Column And Buttons End Here

            }
        })
    }


    function BindSubCategory(d) {
        // var ClientID = $("#hdnid").val();
        //var ClientID = 153;
        var ClientID = getParameterByName('id');
        var mattype = $("#ddlmattype").val();
        $.ajax({
            type: "POST",
            //url: "../WebApi/AllInventory.asmx/SubCategoryDropDown",
            url: "/InventoryManagement/SubCategoryDropDown",

            data: JSON.stringify({ id: ClientID, mattype: mattype }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (response) {


                //  var SubCategory1 = response.d;
                var SubCategory1 = response;
                if (SubCategory1.length > 0) {
                    for (var m = 0; m < SubCategory1.length; m++) {
                        var sub = SubCategory1[m].SubCategory;
                        var rowmat1 = "";
                        if (d == 0) {
                            rowmat1 = "0";
                        }
                        else {
                            rowmat1 = d[15];
                        }

                        if (sub != rowmat1) {
                            $("#ddlSubCategory").append("<option value='" + SubCategory1[m].SubCategoryID + "'>" + SubCategory1[m].SubCategory + "</option>");

                        }
                    }
                }
            }
        })
    }

    function Editing(d) {
        // var id = $("#hdnid").val();
        // var id =153;
        var id = getParameterByName('id');
        $.ajax({
            type: "POST",
            url: "/InventoryManagement/MaterialDropDown",
            data: JSON.stringify({ id: id }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (response) {

                // var material = response.d;
                var material = response;
                if (material.length > 0) {
                    for (var m = 0; m < material.length; m++) {
                        // var mat = material[m].MType;
                        var mat = material[m].MaterialType;
                        var rowmat = "";
                        if (d == 0) {
                            rowmat = "0";
                        }
                        else {
                            rowmat = d[3];
                        }

                        if (mat != rowmat) {
                            $("#ddlmattype").append("<option value='" + material[m].MaterialType + "'>" + material[m].MaterialType + "</option>");

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

                    //$("#LoadingPanel1").css("display", "block");
                    var UID = $("#hdnUID").val();
                    var inventry = $(this).parent().parent().parent().parent().prev('tr').children()[2].innerText;
                    // var firstddl = $(this).parent().parent().prev('div').prev('div').children().find('select').val();
                    var firstddl = $(this).parent().parent().prev('div').prev('div').prev('div').children().find('select').val();// change on 13-march-2020
                    // var firstrow = $(this).parent().parent().prev('div').prev('div').children().find('input[type=text]');
                    var firstrow = $(this).parent().parent().prev('div').prev('div').prev('div').children().find('input[type=text]');// change on 13-march-2020
                    // if (firstrow.length > 0) {
                    var itemcode = firstrow[0].value;
                    var QuantityWithCommas = firstrow[1].value;
                    var Quantity = QuantityWithCommas.replaceAll(",", "");
                    var itemweight = firstrow[2].value;
                    //  }
                    //var secondrow = $(this).parent().parent().prev('div').children().find('input[type=text]');
                    var secondrow = $(this).parent().parent().prev('div').prev('div').children().find('input[type=text]');// change on 13-march-2020
                    var secondrownum = $(this).parent().parent().prev('div').prev('div').children().find('input[type=Number]');

                    // Add code on 01-June-2020
                    if ($("#hdnhire").val() == "true") {
                        var secondddl = $(this).parent().parent().prev('div').children().find('select').val(); // Add on 13-march-2020
                    }
                    else {
                        var secondddl = 0;
                    }
                    if (secondddl == "") {
                        secondddl = 0;
                    }
                    var thirdrow = $(this).parent().parent().prev('div').children().find('input[type=text]');// change on 13-march-2020

                    //var DisplaySortOrder = $(this).closest('td').find('#txtDisplaySortOrder').val();
                    var DisplaySortOrder = thirdrow[0].value;

                    //if (secondrow.length > 0) {
                    var itemdesc = secondrow[0].value;
                    var comments = secondrow[1].value;
                    var lerect = secondrownum[0].value;
                    var ldismantle = secondrownum[1].value;
                    // }
                    var oldqty = $('#hdnOldQty').val();
                    /* var specialChars = /[!@#$%^&*()_+\-=\[\]{};':"\\|,<>\/?]+/;*/
                    //var specialChars = /^[-_a-zA-Z0-9. ]+$/;
                    //Allow Spacel chhar "&" as per client request on  21 -04-2025
                    var specialChars = /^[-_a-zA-Z0-9.& ]+$/;
                    var x = $(this).closest('td').find('#txtDisplaySortOrder').val();

                    /* if (itemcode == "" || Quantity == "" || itemweight == "" || itemdesc == "" || comments == "" || secondddl == "") {*/
                    if (itemcode == "" || Quantity == "" || itemweight == "" || itemdesc == "" || comments == "" || firstddl == "" || ($("#hdnhire").val() == "true" && (secondddl == "" || secondddl == 0))) {
                        if (firstddl == "") {
                            $('#ddlmattype').css('border-color', 'red');
                        }
                        else if (itemcode == "") {
                            $('#txtitemcode').css('border-color', 'red');
                        }
                        else if (Quantity == "") {
                            $('#txtquantity').css('border-color', 'red');
                        }
                        else if (itemweight == "") {
                            $('#txtItemWeight').css('border-color', 'red');
                        }
                        else if (itemdesc == "") {
                            $('#txtitemdesc').css('border-color', 'red');
                        }
                        else if (comments == "") {
                            $('#txtComments').css('border-color', 'red');
                        }
                        //else if (secondddl == "") {
                        //    $('#ddlSubCategory').css('border-color', 'red');
                        //}
                        else if ($("#hdnhire").val() == "true" && (secondddl == "" || secondddl == 0)) {
                            $('#ddlSubCategory').css('border-color', 'red');
                        }

                        var Response = { Result: "Failed", Message: "Please fill required field then click on save." };
                        CustomAlert(Response);
                    }
                    else if (Quantity > 2147483647) {
                        $('#txtquantity').css('border-color', 'red');
                        var Response = { Result: "Failed", Message: "Quantity cannot be greater than 2147483647." };
                        CustomAlert(Response);
                    }
                    else if (oldqty != Quantity && comments == "") {
                        //  if (oldqty != Quantity && comments == "") {
                        //$("#LoadingPanel1").css("display", "none");
                        //$('#art1').removeClass("w3-section w3-card-12  j-close j-alertBox-Success").addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
                        //$('#alertlabel').text("Alert ! Please fill the Comment Section.");
                        var Response = { Result: "Failed", Message: "Please fill the Comment Section." };
                        CustomAlert(Response);
                        //   $('#ContentPlaceHolder1_art1').addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
                        //   $('#ContentPlaceHolder1_alertlabel').text("Alert ! Please fill the Comment Section.");
                    }
                    else if (!specialChars.test(itemcode)) {
                        $('#txtitemcode').css('border-color', 'red');
                        var Response = { Result: "Failed", Message: "You Can Not Use Special Characters In Item Code." };
                        CustomAlert(Response);
                    }
                    else {

                        if (firstddl != "" && itemcode != "" && Quantity != "" && itemweight != "" && itemdesc != "" && comments != "") {
                            $("#LoadingPanel1").css("display", "block");

                            $.ajax({
                                type: "POST",
                                // url: "../WebApi/AllInventory.asmx/InvManagementSaveData",
                                url: "/InventoryManagement/InvManagementSaveData",
                                data: JSON.stringify({ "id": id, "UID": UID, "Invid": inventry, "Mattype": firstddl, "itemcode": itemcode, "Quantity": Quantity, "itemweight": itemweight, "itemdesc": itemdesc, "comments": comments, "lerect": lerect, "ldismantle": ldismantle, "subcategory": secondddl, "DisplaySortOrder": DisplaySortOrder }),
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (response) {
                                    var result = response;
                                    // if (response.d > 0) {
                                    if (result > 0) {
                                        $("#LoadingPanel1").css("display", "none");
                                        //$('#art1').removeClass('w3-section fadeOut w3-card-12 j-alertBox-Failed').addClass("w3-section w3-card-12  j-close j-alertBox-Success").css('display', 'block');
                                        //$('#alertlabel').text("Success ! Data Saved Successfully.");
                                        var Response = { Result: "Success", Message: "Data Saved Successfully." };
                                        CustomAlert(Response);
                                        //$('#ContentPlaceHolder1_art1').addClass("w3-section w3-card-12  j-close j-alertBox-Success").css('display', 'block');
                                        //$('#ContentPlaceHolder1_alertlabel1').text("Success ! Data Saved Successfully.");
                                        //  datagrid(id);
                                        GetInventoryData(id,"All");
                                    }
                                    else if ($("#hdnhire").val() == "true" && (secondddl == "" || secondddl == 0)) {
                                        //else if ((result == 0 || itemcode == "") && (secondddl == "" || secondddl == 0)) {
                                        $("#LoadingPanel1").css("display", "none");
                                        var Response = { Result: "Failed", Message: "Please Choose SubCategory." };
                                        CustomAlert(Response);

                                    }
                                    else {
                                        $("#LoadingPanel1").css("display", "none");
                                        //$('#art1').removeClass("w3-section w3-card-12  j-close j-alertBox-Success").addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
                                        //$('#alertlabel').text("Alert ! Item Code already exists.");
                                        var Response = { Result: "Failed", Message: "Item Code already exists." };
                                        CustomAlert(Response);
                                        // $('#ContentPlaceHolder1_art1').addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
                                        //  $('#ContentPlaceHolder1_alertlabel').text("Alert ! Item Code already exists.");
                                    }
                                    $('#hdnOldQty').val("");
                                }
                            })
                        }

                        else {
                            $("#LoadingPanel1").css("display", "none");
                            $("#btnsubmit").click();
                        }
                    }

                });
            }
        });
        if (d == 0) {
            return '<div class="col-lg-12">'
                + '<div class="col-lg-3">'
                + ' Material Type<a style="color:red">*</a><select id="ddlmattype" style="width:100%" class="form-control" onchange="ddlmattype_SelectedIndexChanged()" required><option id="1" value=""</option></select>'
                + '</div><div class="col-lg-3">'
                + 'Item Code<a style="color:red">*</a><input id="txtitemcode" type="text"  class="form-control" value="" style="width:100%" required/>'
                + '</div><div class="col-lg-3">'
                + 'Quantity<a style="color:red">*</a><input id="txtquantity" type="text" class="form-control" value="" style="width:100%" required/>'
                + '</div><div class="col-lg-3">'
                + 'Item Weight<a style="color:red">*</a><input id="txtItemWeight" type="text" class="form-control" value="" style="width:100%" required/>'
                + '</div></div>'
                + '<div class="col-lg-12">'
                + '<div class="col-lg-3">'
                + 'Item Desc<a style="color:red">*</a><input id="txtitemdesc" type="text" class="form-control" value="" style="width:100%" required/>'
                + '</div><div class="col-lg-3">'
                + 'Comments<a style="color:red">*</a><input id="txtComments" type="text" class="form-control" value="" style="width:100%" required/>'
                + '</div><div class="col-lg-3">'
                + 'Labour Erect<input id="txtLabourErect" type="number" min="0" class="form-control" value="" style="width:100%" />'
                + '</div><div class="col-lg-3">'
                + 'Labour Dismantle<input id="txtLabourDismantle" type="number" min="0" class="form-control" value="" style="width:100%" />'
                + '</div></div>'

                //Add on 07 March 2020
                + '<div class="col-lg-12">'
                + '<div class="col-lg-3">'
                + 'Display Sort Order<input id="txtDisplaySortOrder" type="text" class="form-control" value="" style="width:100%" />'
                + '</div>'
                + '<div class="col-lg-9" id="divEditSubcategory"  style="display:none;">'
                + '<div class="col-lg-4">'
                + 'Sub Category<a style="color:red">*</a><select id="ddlSubCategory" style="width:100%" class="form-control"><option id="1" value=""</option></select>'
                + '</div></div>'
                + '</div>'
                + '<div class="col-lg-12">'
                + '<div class="col-lg-2">'
                + '<input type="button" id="savechange" class="btn btn-primary" value="Save" style="color:white;margin-top:6px;width:100%"/>'
                + '<input type="submit" id="btnsubmit" class="btn btn-primary" value="Save" style="color:white;margin-top:6px;width:100%;display:none"/>'
                + '</div><div class="col-lg-2">'
                + '<input type="button" id="cancelchange" class="btn CancelChangeClass" value="Cancel" style="color:black;background: lightgrey;margin-top:6px;width:100%"/>'
                + '</div><div class="col-lg-6"></div><div class="col-lg-2">'
                + '<input id="psudouserid" type="text" class="form-control" value="" style="width:1% ; margin-top:6px; float:right;opacity:0" />'
                + '</div></div>'

        }
        else {
            return '<div class="col-lg-12">'
                + '<div class="col-lg-3">'
                + ' Material Type<a style="color:red">*</a><select id="ddlmattype" style="width:100%" class="form-control" onchange="ddlmattype_SelectedIndexChanged()" required><option id="1" value="' + d[3] + '">' + d[3] + '</option></select>'
                + '</div><div class="col-lg-3">'
                + 'Item Code<a style="color:red">*</a><input id="txtitemcode" type="text" class="form-control" value="' + d[4] + '" style="width:100%" required/>'
                + '</div><div class="col-lg-3">'
                + 'Quantity<a style="color:red">*</a><input id="txtquantity" type="text" class="form-control" value="' + numberWithCommas(d[6]) + '" style="width:100%" required/>'
                + '</div><div class="col-lg-3">'
                + 'Item Weight<a style="color:red">*</a><input id="txtItemWeight" type="text" class="form-control" value="' + numberWithCommas(d[8]) + '" style="width:100%" required/>'
                + '</div></div>'
                + '<div class="col-lg-12">'
                + '<div class="col-lg-3">'
                + 'Item Desc<a style="color:red">*</a><input id="txtitemdesc" type="text" class="form-control" value="' + d[5].replace('"', '&quot;') + '" style="width:100%" required/>'
                + '</div><div class="col-lg-3">'
                + 'Comments<a style="color:red">*</a><input id="txtComments" type="text" class="form-control" value="' + d[11] + '" style="width:100%" required/>'
                + '</div><div class="col-lg-3">'
                + 'Labour Erect<input id="txtLabourErect" type="number" min="0" class="form-control" value="' + d[12] + '" style="width:100%" />'
                + '</div><div class="col-lg-3">'
                + 'Labour Dismantle<input id="txtLabourDismantle" type="number" min="0" class="form-control" value="' + d[13] + '" style="width:100%" />'
                + '</div></div>'
                //Add on 07 March 2020
                + '<div class="col-lg-12">'
                //add on  16 apr 2025
                + '<div class="col-lg-3">'
                + 'Display Sort Order<input id="txtDisplaySortOrder" type="text" class="form-control" value="' + d[14] + '" style="width:100%" />'
                + '</div>'
                + '<div class="col-lg-9" id="divEditSubcategory"  style="display:none;">'
                + '<div class="col-lg-4">'
                + ' Sub Category<a style="color:red">*</a><select id="ddlSubCategory" style="width:100%" class="form-control" required><option id="1" value="' + d[15] + '">' + d[16] + '</option></select>'
                + '</div></div>'
                + '</div>'
                + '<div class="col-lg-12">'
                + '<div class="col-lg-2">'
                + '<input type="button" id="savechange" class="btn btn-primary" value="Save" style="color:white;margin-top:6px;width:100%"/>'
                + '<input type="submit" id="btnsubmit" class="btn btn-primary" value="Save" style="color:white;margin-top:6px;width:100%;display:none"/>'
                + '</div><div class="col-lg-2">'
                + '<input type="button" id="cancelchange" class="btn CancelChangeClass" value="Cancel" style="color:black;background: lightgrey;margin-top:6px;width:100%"/>'
                + '</div><div class="col-lg-6"></div><div class="col-lg-2">'
                + '<input id="psudouserid" type="text" class="form-control" value="" style="width:1% ; margin-top:6px; float:right;opacity:0" />'
                + '</div></div>'

        }

    }

    function format(d) {
        $("#LoadingPanel1").css("display", "block");
        // var id = $("#ContentPlaceHolder1_hdnid").val();
        var id = getParameterByName('id');
        var str = "";
        str = '<table id="Clone_' + d.DT_RowId + '" class="clone" cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;width:100%">';
        str += '<thead><tr>';
        str += '<th style="width:10%;text-align:center;display:none">Inventory Id</th>';
        str += '<th style="width:10%;text-align:center">Old Value</th>';
        str += '<th style="width:20%;text-align:center">New Value</th>';
        str += '<th style="width:20%;text-align:center">Change Types</th>';
        str += '<th style="width:20%;text-align:center">Change Comments</th>';
        str += '<th style="width:20%;text-align:center">Changed By</th>';
        str += '<th style="width:20%;text-align:center">Changed On</th>';
        str += '</tr></thead><tbody>';
        str += '</tbody></table>';
        $.ajax({
            type: "POST",
            // url: "../WebApi/AllInventory.asmx/EditData",
            url: "/InventoryManagement/EditData",
            data: JSON.stringify({ id: id, Invid: d[2] }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (response) {
                // var data = response.d;
                var data = response;
                if (data.length > 0) {


                    for (var j = 0; j < data.length; j++) {
                        //var OldQuantity = data[j].OldQuantity == null ? "" : data[j].OldQuantity;
                        //var NewQuantity = data[j].NewQuantity == null ? "" : data[j].NewQuantity;
                        var OldValue = data[j].OldValue == null ? "" : data[j].OldValue;
                        var NewValue = data[j].NewValue == null ? "" : data[j].NewValue;
                        var ChangeType = data[j].ChangeType == null ? "" : data[j].ChangeType;
                        var ChangeComments = data[j].ChangeComments == null ? "" : data[j].ChangeComments;
                        var CreatedBy = data[j].CreatedBy == null ? "" : data[j].CreatedBy;
                        var CreatedOn = data[j].CreatedOn == null ? "" : data[j].CreatedOn;
                        str += '<tr>';
                        str += '<td style="width:10%;text-align:center;display:none">' + d[2] + '</td>';
                        //str += '<td style="width:10%;text-align:center">' + OldQuantity + '</td>';
                        //str += '<td style="width:20%;text-align:center">' + NewQuantity + '</td>';
                        str += '<td style="width:10%;text-align:center">' + OldValue + '</td>';
                        str += '<td style="width:20%;text-align:center">' + NewValue + '</td>';
                        str += '<td style="width:20%;text-align:center">' + ChangeType + '</td>';
                        str += '<td style="width:20%;text-align:center">' + ChangeComments + '</td>';
                        str += '<td style="width:20%;text-align:center">' + CreatedBy + '</td>';
                        str += '<td style="width:20%;text-align:center">' + DateFormat(CreatedOn) + '</td>';
                        str += '</tr>';
                        //row.child(format(st).show());
                    }
                    // str += st;

                }


                console.log(d);
                var check = d.DT_RowId;
                $("#Clone_" + d.DT_RowId).html(str);
                $("#LoadingPanel1").css("display", "none");
            }
        });
        return str;
        //    // `d` is the original data object for the row
    }



    function fnExcelReport() {

        var tab_text = $("#hdnTableData").val();
        var textRange;
        var j = 0;
        if ($("#hdnerror").val() == true) {
            var tab = document.getElementById('errgrid'); // id of table
        }
        else {
            var tab = document.getElementById('dataforgrid'); // id of table
        }

        var lines = tab.rows.length;

        //tab_text = tab_text + "</table>";
        tab_text = tab_text.replace(/<A[^>]*>|<\/A>/g, "");             //remove if u want links in your table
        tab_text = tab_text.replace(/<img[^>]*>/gi, "");                 // remove if u want images in your table
        tab_text = tab_text.replace(/<input[^>]*>|<\/input>/gi, "");    // reomves input params
        console.log(tab_text); // aktivate so see the result (press F12 in browser)

        var ua = window.navigator.userAgent;
        var msie = ua.indexOf("MSIE ");
        $("#LoadingPanel1").css("display", "none");
        // if Internet Explorer
        if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) {
            txtArea1.document.open("txt/html", "replace");
            txtArea1.document.write(tab_text);
            txtArea1.document.close();
            txtArea1.focus();
            sa = txtArea1.document.execCommand("SaveAs", true, "InventoryManagement.xls");
        }
        else // other browser not tested on IE 11
            // sa = window.open('data:application/vnd.ms-excel,' + encodeURIComponent(tab_text));
            var link = document.createElement("a");
        link.download = "InventoryManagement.xls";
        link.href = 'data:application/vnd.ms-excel,' + encodeURIComponent(tab_text);
        link.click();

        return (sa);
    }

    function deleterowyes() {
        $("#LoadingPanel1").css("display", "block");
        // var id = $("#ContentPlaceHolder1_hdnid").val();
        var id = getParameterByName('id');
        var MaterialType = $('#ddlMaterialType option:selected').val();
        var inventry = 0;

        if ($("#hdndeleteinv").val() != "") {
            inventry = $("#hdndeleteinv").val();
        }
        $.ajax({
            type: "POST",
            // url: "../WebApi/AllInventory.asmx/InvManagementDeleteData",
            url: "/InventoryManagement/InvManagementDeleteData",
            data: JSON.stringify({ id: id, Invid: inventry }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                var result = response;
                // if (response.d > 0) {
                if (result > 0) {
                    //$("#ContentPlaceHolder1_divdelete").css("display", "none");
                    $("#divdelete").css("display", "none");
                    //$('#art1').removeClass("w3-section w3-card-12 j-alertBox-Failed").addClass("w3-section w3-card-12  j-close j-alertBox-Success").css('display', 'block');
                    //$('#alertlabel').text("Success ! Data Delete Successfully.");
                    var Response = { Result: "Success", Message: "Data Delete Successfully." };
                    CustomAlert(Response);
                    GetInventoryData(id, MaterialType);
                }
                else {
                    // $("#ContentPlaceHolder1_divdelete").css("display", "none");
                    $("#divdelete").css("display", "none");
                    $("#LoadingPanel1").css("display", "none");
                    //$('#art1').removeClass("w3-section w3-card-12  j-close j-alertBox-Success").addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
                    //$('#alertlabel').text("Alert ! Record cannot be deleted as it is being used in Material List Details.");
                    var Response = { Result: "Failed", Message: "Record cannot be deleted as it is being used in Material List Details." };
                    CustomAlert(Response);
                }
                // alert("save successfully");

            }
        })
    }

    function deleterowno() {
        // $("#ContentPlaceHolder1_divdelete").css("display", "none");
        $("#divdelete").css("display", "none");
    }
    function btndelete_Click() {
        $("#divdelete").css("display", "block");
    }

    function btnsaveyes() {
        $("#divSave").css("display", "none");
    }

    function btnLoad_Click(IsSort) {
        $("#LoadingPanel1").css("display", "block");
        setTimeout(function () {
            var files;
            // Checking whether FormData is available in browser  
            if (window.FormData !== undefined) {
                if (files == null || files == "") {
                    var fileUpload = $('#FileUploadSelectfile').get(0);
                    files = fileUpload.files;
                }
                var fileUpload = $('#FileUploadSelectfile').get(0);
                // Create FormData object  
                var fileData = new FormData();

                if (files.length != 0) {

                    var FileName = files[0].name;
                    var FileSize = files[0].size;
                    var Extension = FileName.substring(FileName.lastIndexOf('.') + 1).toLowerCase();
                    if (Extension == "xlsx") {

                        fileData.append(files[0].name, files[0]);

                        var UID = getParameterByName('UID');
                        var UserRole = getParameterByName('UserRole');
                        var id = getParameterByName('id');

                        // Adding one more key to FormData object  
                        //fileData.append('UID', $("#hdnUID").val());
                        //fileData.append('ClientID', $("#hdnid").val());
                        fileData.append('UID', UID);
                        fileData.append('ClientID', id);
                        fileData.append('IsSort', IsSort);

                        $.ajax({
                            url: '/InventoryManagement/UploadInventoryData',
                            type: "POST",
                            contentType: false, // Not to set any content header  
                            processData: false, // Not to process data  
                            data: fileData,
                            success: function (response) {
                                var Result = response;
                                if (response.Result == "Success") {
                                    //$('#art1').removeClass("w3-section w3-card-12 j-alertBox-Failed").addClass("w3-section w3-card-12  j-close j-alertBox-Success").css('display', 'block');
                                    //$('#alertlabel').text("Success ! Data Saved Successfully.");
                                    setTimeout(function () {
                                        $('#CustomAlert').fadeOut('slow', function () {
                                            $('#CustomAlert').removeClass('j-alertBox-Success').css('display', 'none');
                                            $('#alertMsg').html('');

                                        });
                                    }, 30000);
                                    var Response = { Result: "Success", Message: "Data Saved Successfully." };
                                    CustomAlert(Response);
                                    location.reload();
                                }
                                else if (response.Message == "quantityZero") {
                                    var Res = {
                                        Result: "Failed", Message: "For only  setting the sort order, the quantity should be 0."
                                    };
                                    CustomAlert(Res);

                                    document.getElementById("FileUploadSelectfile").value = "";

                                    $("#LoadingPanel1").css("display", "none");

                                }
                                else {
                                    if (response.Message == "ErrorGrid") {
                                        //var Response = { Result: "Failed", Message: "The file needs to be rectified.Look for message on Comments." };
                                        //CustomAlert(Response);
                                        // hdnerror.Value = "true";
                                        $('#hdnerror').val(true);
                                        $("#LoadingPanel1").css("display", "none");
                                        $('#loaddata').css('display', 'none');
                                        var UID = getParameterByName('UID');
                                        var id = getParameterByName('id');
                                        errorgrid(id, UID);
                                        var Response = { Result: "Failed", Message: "The file needs to be rectified.Look for message on Comments." };
                                        CustomAlert(Response);
                                        // location.reload();
                                    } else {
                                        CustomAlert(response);
                                        $("#LoadingPanel1").css("display", "none");
                                        location.reload();
                                    }
                                }
                            },
                            error: function (err) {
                                bootbox.alert(err.statusText, function () {
                                    location.reload();
                                });
                            }
                        });

                    }
                    else {

                        //$('#art1').removeClass("w3-section w3-card-12  j-close j-alertBox-Success").addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
                        //$('#alertlabel').text("Alert ! Please upload only xlsx files.");
                        $("#LoadingPanel1").css("display", "none");
                        //var Response = { Result: "Failed", Message: "Please upload only xlsx files." };
                        var Response = {
                            Result: "Failed", Message: "Please Choose xlsx File and then click on the Load Button."
                        };
                        CustomAlert(Response);
                    }

                }
                else {
                    $('#CompanyChoose').val("");
                    //$('#art1').removeClass("w3-section w3-card-12  j-close j-alertBox-Success").addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
                    //$('#alertlabel').text("Alert ! Please upload only xlsx files.");
                    $("#LoadingPanel1").css("display", "none");
                    var Response = {
                        Result: "Failed", Message: "Please Choose xlsx File and then click on the Load Button."
                    };
                    CustomAlert(Response);
                }
            } else {
                $("#LoadingPanel1").css("display", "none");
                alert("Your browser does not support FormData. Update it for a better user experience.");
            }
        }, 1000);
    }
    function errorgrid(id, UID) {

        $.ajax({
            type: "POST",
            // url: "../WebApi/AllInventory.asmx/geterrorData",
            url: "/InventoryManagement/geterrorData",
            data: JSON.stringify({ id: id, UID: UID }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                // var getdata = response.d;
                var getdata = response;
                var str = "";
                str += '<table id="errgrid" class="table table-striped display" style="width:100%;table-layout:fixed;font-size:12px">';
                str += "<thead><tr>";
                str += "<th  style='text-align:center;width:3%;display:none'>InventoryId</th>";
                str += "<th style='text-align:left;width:25%;'>Comments</th>";
                str += "<th  style='text-align:center;width:10%'>Material Type</th>";
                str += "<th  style='text-align:center;width:5%'>Item Code</th>";
                str += "<th  style='text-align:center;width:30%'>Item Description</th>";
                str += "<th  style='text-align:center;width:6%'>Quantity</th>";
                str += "<th  style='text-align:center;width:10%'>Item Weight</th>";
                str += "<th  style='text-align:center;width:7%'>Labour Erect</th>";
                str += "<th  style='text-align:center;width:7%'>Labour Dismantle</th>";


                // Add Code On 27/March/2020.
                if ($("#hdnhire").val() == "true") {
                    str += "<th  style='text-align:center;width:10%'>Sub Category</th></tr></thead><tbody>"; // Add On 07 March 2020
                }
                else {
                    str += "<th  style='text-align:center;width:10%;display:none;'></th></tr></thead><tbody>";
                }

                if (getdata.length > 0) {
                    for (var i = 0; i < getdata.length; i++) {
                        var InventoryID = getdata[i].InventoryID == null ? "" : getdata[i].InventoryID;
                        var Comments = getdata[i].Comments == null ? "" : getdata[i].Comments;
                        var MaterialType = getdata[i].MaterialType == null ? "" : getdata[i].MaterialType;
                        var ItemCode = getdata[i].ItemCode == null ? "" : getdata[i].ItemCode;
                        var ItemDesc = getdata[i].ItemDesc == null ? "" : getdata[i].ItemDesc;
                        var OrgYardQuantity = getdata[i].OrgYardQuantity == null ? "" : getdata[i].OrgYardQuantity;
                        var ItemWeight = getdata[i].ItemWeight == null ? "" : getdata[i].ItemWeight;

                        var LabourErect = getdata[i].LabourErect == null ? "" : getdata[i].LabourErect;
                        var LabourDismantle = getdata[i].LabourDismantle == null ? "" : getdata[i].LabourDismantle;
                        var SubCategory = getdata[i].SubCategory == null ? "" : getdata[i].SubCategory;   // Add On 07 March 2020

                        str += "<tr id='row_" + i + "'>";
                        str += "<td  style='text-align:center;width:3%;display:none'>" + InventoryID + "</td>";
                        str += "<td  style='text-align:Left;width:25%'>" + Comments + "</td>";
                        str += "<td  style='text-align:center;width:10%'>" + MaterialType + "</td>";
                        str += "<td  style='text-align:center;width:5%'>" + ItemCode + "</td>";
                        str += "<td  style='text-align:center;width:10%'>" + ItemDesc + "</td>";
                        str += "<td  style='text-align:center;width:6%'>" + OrgYardQuantity + "</td>";
                        str += "<td  style='text-align:center;width:10%'>" + ItemWeight + "</td>";
                        str += "<td  style='text-align:center;width:7%'>" + LabourErect + "</td>";
                        str += "<td  style='text-align:center;width:7%'>" + LabourDismantle + "</td>";


                        // Add Code On 01/June/2020.
                        if ($("#hdnhire").val() == "true") {
                            str += "<td  style='text-align:center;width:10%'>" + SubCategory + "</td></tr>"; // add on 07 march 2020
                        }
                        else {
                            str += "<td  style='text-align:center;width:10%;display:none;'></td></tr>";
                        }

                    }
                    str += "</tbody></table>";
                    $("#errordata").html(str);
                    $("#hdnTableData").val(str);
                    $('#errgrid').DataTable({
                        "bSort": false,
                        "processing": true,
                        searching: true,
                        info: false,
                        "pageLength": 50,
                        //paging: false,
                        autoWidth: true,
                        scrollX: true,
                        scrollCollapse: true,
                        scrollY: "60vh",
                        initComplete: function () {
                            $("#errgrid_length").children().html('<a><b>Data Not Loaded Successfully (Reason mentioned in Comment)</b></a>&nbsp;&nbsp;<button id="btnerrorExport" type="button" class="w3-btn w3-teal Export" style="margin-left:23px;height:26px;padding-top:3px"><i class="fa fa-file-excel-o"></i>&nbsp;Export to CSV</button>');


                            $(".Export").click(function () {
                                $("#ajaxLoader").css("display", "block");
                                fnExcelReport();
                            });
                        }
                    });
                    //$('#ContentPlaceHolder1_art1').addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
                    //$('#ContentPlaceHolder1_alertlabel').text("Alert ! Data Not Loaded Successfully(reason mentioned in comment).");
                }
            }
        });
    }


    function PipeInventoryHide(id) {
        $.ajax({
            type: "POST",
            url: "/InventoryManagement/PipeInventoryHide",
            data: JSON.stringify({ id: id }),
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
    function DowloadTemplete() {
        var ClientID = getParameterByName('id');
        var UID = getParameterByName('UID');
        // var GetTempleteFile = "Template_InventoryManagement5896324.xlsx";
        //var GetTempleteFile = TempleteFile;
        // if (GetTempleteFile != "" && GetTempleteFile != null) {

        window.location.href = '/InventoryManagement/GetTempleteFile?UID=' + UID + '&ClientID=' + ClientID;
        // setTimeout(AlertRes, 1000);
        Res = { Result: "Success", Message: "File downloaded Successfully" };
        CustomAlert(Res);
        $('.selectFile').prop('selected', 'selected');
        // }
        //else {
        //    Res = { Result: "Warning", Message: "Please select any file for download" };
        //    CustomAlert(Res);
        //}
    }
    function ddlmattype_SelectedIndexChanged() {
        BindSubCategory(0);
        $('#ddlSubCategory').empty().append("<option value=''></option>");
    }
    function DdlMaterialType() {
        var id = getParameterByName('id');
        $.ajax({
            type: "POST",
            url: "/InventoryManagement/DdlMaterialType",
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
    function GetTotalTotalTonnage(ClientID, MaterialType) {
        $.ajax({
            type: "POST",
            url: "/InventoryManagement/GetTotalTotalTonnage",

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

    function ddlMaterialTypeOptionsChanged()
    {
        var ClientID = getParameterByName('id');
        var MaterialType = $('#ddlMaterialType option:selected').val();
        GetTotalTotalTonnage(ClientID, MaterialType);
        GetInventoryData(ClientID, MaterialType);
    }
