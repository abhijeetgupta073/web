$(document).ready(function () {

});
function DropdownBind() {
    var id = getParameterByName('id');
    $.ajax({
        url: '/InspectionCheckListContent/DropdownBind',
        async: false,
        data: { id: id },
        success: function (response) {

            data = response;
            var ddlCheckList = "";
            var ddlCheckListFrom = "<option value='' disabled selected='selected'>Select---</option>";
            var ddlDeleteList = "<option value='' disabled selected='selected'>Select---</option>";
            $.each(response, function (idx, obj) {
                if (obj != null) {
                    ddlCheckList += '<option value="' + obj.checklistID + '">' + obj.checklistdescription + '</option>';
                    ddlCheckListFrom += '<option value="' + obj.checklistID + '">' + obj.checklistdescription + '</option>';
                    ddlDeleteList += '<option value="' + obj.checklistID + '">' + obj.checklistdescription + '</option>';
                }
            });


            $('#ddlCheckList').empty().append(ddlCheckList);
            $('#ddlCheckListFrom').empty().append(ddlCheckListFrom);
            $('#ddlDeleteList').empty().append(ddlDeleteList);
        }
    });
}
function RadioButtonBind() {
    var id = getParameterByName('id');
    var ddlCheckList = $('#ddlCheckList').val();
    $.ajax({
        url: '/InspectionCheckListContent/RadioButtonBind',
        async: false,
        data: { id: id, ddlCheckList: ddlCheckList },
        success: function (response) {

            var CheckListStatus = response;
            if (CheckListStatus == "Active") {
                $('#RadioButtonActive').prop('checked', true);
                $('#RadioButtonInActive').prop('checked', false);
            } else {
                $('#RadioButtonActive').prop('checked', false);
                $('#RadioButtonInActive').prop('checked', true);
            }

        }
    });
}
function ddlCheckList_SelectedIndexChanged() {
    RadioButtonBind();
    GetInspectionCheckListData();
}
function AddNewRow() {

    //var str = "<tr><td style='display:none;'></td><td></td><td></td><td></td><td></td><td></td><td><input type='text' id='txtCheckListContent_0' class='form-control txtEditColor'/></td><td><input id='checkboxNA_0' type='checkbox'/><span>&nbsp NA</span></td></tr>";
    var str = "<tr style='background: #CCFFFF;'><td style='display:none;'></td><td></td><td><input type='text' id='txtCheckListContent_0' class='form-control txtEditColor'/></td><td><input id='checkboxNA_0' type='checkbox'/><span>&nbsp NA</span></td></tr>";
    $('#tbodyInspectionCheckList').prepend(str);
}
var tableInspCheckList = "";
var tableCheckListContentData = "";
var InspectionCheckListArryData = [];


function GetInspectionCheckListData() {

    $('#LoadingPanel1').show();
    var id = getParameterByName('id');
    var ddlCheckList = $('#ddlCheckList').val();
    $.ajax({
        type: "POST",
        url: "/InspectionCheckListContent/GetInspectionCheckListData",
        data: JSON.stringify({ id: id, ddlCheckList: ddlCheckList }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            var str = "";
            var GetData = response;
            $('#tblInspectionCheckList').DataTable().clear();
            $('#tblInspectionCheckList').DataTable().destroy();

            if (GetData.length > 0) {
                $('#tblInspectionCheckList').css("display", "block");

                for (var i = 0; i < GetData.length; i++) {
                    var ContentID = GetData[i].ContentID == null ? "" : GetData[i].ContentID;
                    var ClientID = GetData[i].ClientID == null ? "" : GetData[i].ClientID;
                    var CheckListID = GetData[i].CheckListID == null ? "" : GetData[i].CheckListID;
                    var CheckListContent = GetData[i].CheckListContent == null ? "" : GetData[i].CheckListContent;
                    var checklistItem = GetData[i].checklistItem == null ? "" : GetData[i].checklistItem;
                    var subChildItem = GetData[i].subChildItem == null ? "" : GetData[i].subChildItem;
                    var itemOrder = GetData[i].itemOrder == null ? "" : GetData[i].itemOrder;
                    var CopyFromCheckListItem = GetData[i].CopyFromCheckListItem == null ? "" : GetData[i].CopyFromCheckListItem;
                    var CopyFromSubChilditem = GetData[i].CopyFromSubChilditem == null ? "" : GetData[i].CopyFromSubChilditem;
                    var CopyFromitemOrder = GetData[i].CopyFromitemOrder == null ? "" : GetData[i].CopyFromitemOrder;
                    var CopyFromChecklistId = GetData[i].CopyFromChecklistId == null ? "" : GetData[i].CopyFromChecklistId;
                    var IsNA = GetData[i].IsNA == null ? "" : GetData[i].IsNA;
                    var CopyFromContentID = GetData[i].CopyFromContentID == null ? "" : GetData[i].CopyFromContentID;
                    CheckListContent = htmlAttributeEncode(CheckListContent);

                    //str += '<tr>'
                    str += '<tr style="background:#CCFFFF;">'
                    str += '<td id="ContentID_' + i + '" style="display:none">' + ContentID + '</td>'
                    str += '<td id="CheckListID_' + ContentID + '" style="display:none">' + CheckListID + '</td>'
                    str += '<td id="itemOrder_' + ContentID + '" style="display:none">' + itemOrder + '</td>'
                    //str += '<td  class="tdbtnExpend" style="width:5%;"><a id="CheckListInspectionExpend_' + ContentID + '" class="expand fa fa-angle-right"  style="font-size:19px;"></a></td>';
                    //str += '<td style="cursor:pointer;"><a id="btnUp_' + i + '" class="btnUp"><i style="color:grey" class="fa fa-arrow-up"></i></a></td>';
                    //str += '<td style="cursor:pointer;"><a id="btnDown_' + i + '" class="btnDown"><i style="color:grey" class="fa fa-arrow-down"></i>&nbsp;</a></td>';
                    //str += '<td title="Click here for Delete" style="cursor:pointer;"><i class="fa fa-trash" onclick=ShowDeletePopup("'+ ContentID + '","ViewLevel1")></i></td>';
                    //str += '<td title="Click here to edit CheckList." style="cursor:pointer;"><a id="btnEdit_' + ContentID + '" class="btnCheckListEdit"><i class="fa fa-pencil"></i>&nbsp;</a></td>';

                    str += '<td  class="tdbtnExpend" style="width:5%;display:flex;"><a id="CheckListInspectionExpend_' + ContentID + '" class="expand fa fa-angle-right"  style="font-size:19px;"></a>&nbsp;&nbsp;<a id="btnUp_' + i + '" class="btnUp"><i style="color:grey" class="fa fa-arrow-up"></i></a>&nbsp;&nbsp;<a id="btnDown_' + i + '" class="btnDown"><i style="color:grey" class="fa fa-arrow-down"></i>&nbsp;</a>&nbsp;&nbsp;<i class="fa fa-trash ClassbtnCheckListDelete" onclick=ShowDeletePopup("' + ContentID + '","ViewLevel1","' + ContentID + '")></i>&nbsp;&nbsp;<a id="btnEdit_' + ContentID + '" class="btnCheckListEdit"><i class="fa fa-pencil"></i></a></td>';

                    str += '<td id="CheckListContentInputType_' + ContentID + '"><input id="txtCheckListContent_' + ContentID + '" type="text" style="display:none;" class="form-control" value="' + CheckListContent + '"><label id="lblCheckListContent_' + ContentID + '">' + CheckListContent + '</label></td>'
                    if (IsNA == true) {
                        str += '<td><input type="checkbox" id="checkboxNA_' + ContentID + '" checked disabled/><span> &nbspNA</span></td>'
                    } else {
                        str += '<td><input id="checkboxNA_' + ContentID + '" type="checkbox" disabled/><span>&nbsp NA</span></td>'
                    }

                    str += '</tr>'
                }

                $('#tbodyInspectionCheckList').append(str);
                $('#LoadingPanel1').css('display', 'none');

            }
            tableInspCheckList = $('#tblInspectionCheckList').DataTable({
                searching: true,
                paging: false,
                info: false,
                autoWidth: true,
                length: false,
                sort: false,


                initComplete: function () {

                    //$("#tblInspectionCheckList_filter").children().html('<button id="btnAddNewCheckList"  type="button" class="fa fa-plus"></button><button id="btnSave"  type="button" class="fa fa-save"></button><button id="btnRefresh"  type="button" class="fa fa-remove"></button>');

                    $("#tblInspectionCheckList_filter").children().html('<a id="btnAddNewCheckList" style="cursor:pointer;" class="fa fa-plus"></a>&nbsp;&nbsp<a id="btnCheckListSave" style="cursor:pointer;" class="fa fa-save"></a>&nbsp;&nbsp<a id="btnRefresh" style="cursor:pointer;" class="fa fa-remove"></a>');

                    $("#btnAddNewCheckList").click(function () {
                        let txtCheckListContent_0 = $('#txtCheckListContent_0').val();
                        if (txtCheckListContent_0 != '') {
                            AddNewRow();
                        } else {
                            //var Response = { Result: "Failed", Message: "Please fill the fields" };
                            //CustomAlert(Response);
                            $('#txtCheckListContent_0').css('border-color', 'red');
                        }
                    });
                    $("#btnCheckListSave").click(function () {
                        $('#LoadingPanel1').show();

                        $('#tblInspectionCheckList td input').each(function () {
                            if ($(this).hasClass("txtEditColor")) {
                                var ContentID = this.id.split('_')[1];
                                var CheckListContent = this.value;
                                var IsNA = $('#checkboxNA_' + ContentID).prop('checked');
                                var CheckListID = $('#CheckListID_' + ContentID).text();
                                //alert(this.value);

                                if (ContentID == 0) {
                                    CheckListID = $('#ddlCheckList').val();
                                }

                                if (CheckListContent != '') {
                                    var ArryVal = { ContentID: ContentID, CheckListID: CheckListID, CheckListContent: CheckListContent, IsNA: IsNA };
                                    InspectionCheckListArryData.push(ArryVal);
                                } else {
                                    $('#txtCheckListContent_' + ContentID).css('border-color', 'red');
                                }

                                //var ArryVal = {ContentID:ContentID, CheckListID:CheckListID, CheckListContent:CheckListContent, IsNA:IsNA};
                                //  InspectionCheckListArryData.push(ArryVal);
                            }

                        });

                        var id = getParameterByName('id');
                        var UID = getParameterByName('UID');
                        var TableValues = "ViewLevel1";
                        if (InspectionCheckListArryData.length > 0) {
                            $.ajax({
                                type: "POST",
                                url: "/InspectionCheckListContent/InspectionCheckListSaveData",
                                data: JSON.stringify({ id: id, UID: UID, TableValues: TableValues, InspectionCheckListVal: InspectionCheckListArryData }),
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                async: true,
                                success: function (response) {
                                    if (response.Result == "Success") {
                                        CustomAlert(response);
                                        GetInspectionCheckListData();
                                        clearArray(InspectionCheckListArryData);
                                    } else {
                                        CustomAlert(response);
                                        $('#LoadingPanel1').hide();
                                        clearArray(InspectionCheckListArryData);
                                    }
                                }
                            });
                        } else {
                            $('#LoadingPanel1').hide();
                        }
                    });
                    $("#btnRefresh").click(function () {
                        GetInspectionCheckListData();
                    });
                    $(".btnCheckListEdit").click(function () {
                        event.stopImmediatePropagation();

                        var ContentID = this.id.split('_')[1];
                        $('#txtCheckListContent_' + ContentID).show();
                        $('#lblCheckListContent_' + ContentID).hide();
                        $('#txtCheckListContent_' + ContentID).addClass('txtEditColor');
                        $('#checkboxNA_' + ContentID).prop('disabled', false);

                    });
                    $(".btnUp").click(function () {
                        event.stopImmediatePropagation();

                        var rowindex = this.id.split('_')[1];
                        var ContentID = $('#ContentID_' + rowindex).text();
                        var CheckListID = $('#CheckListID_' + ContentID).text();
                        var itemOrder = $('#itemOrder_' + ContentID).text();

                        var OrderContentID2 = $('#ContentID_' + (rowindex - 1)).text();

                        var id = getParameterByName('id');
                        var UID = getParameterByName('UID');
                        $.ajax({
                            type: "POST",
                            url: "/InspectionCheckListContent/ShortOrderUpLevel1",
                            data: JSON.stringify({ id: id, UID: UID, OrderContentID: ContentID, OrderContentID2: OrderContentID2, itemorder: itemOrder, CheckListID: CheckListID }),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: true,
                            success: function (response) {
                                if (response.Result == "Success") {
                                    GetInspectionCheckListData();
                                } else {
                                    CustomAlert(response);
                                }
                            }
                        });
                    });
                    $(".btnDown").click(function () {
                        event.stopImmediatePropagation();

                        var rowindex = this.id.split('_')[1];
                        var ContentID = $('#ContentID_' + rowindex).text();
                        var CheckListID = $('#CheckListID_' + ContentID).text();
                        var itemOrder = $('#itemOrder_' + ContentID).text();
                        var OrderContentID2 = $('#ContentID_' + (parseInt(rowindex) + 1)).text();

                        var id = getParameterByName('id');
                        var UID = getParameterByName('UID');

                        $.ajax({
                            type: "POST",
                            url: "/InspectionCheckListContent/ShortOrderDownLevel1",
                            data: JSON.stringify({ id: id, UID: UID, OrderContentID: ContentID, OrderContentID2: OrderContentID2, itemorder: itemOrder, CheckListID: CheckListID }),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: true,
                            success: function (response) {
                                if (response.Result == "Success") {
                                    GetInspectionCheckListData();
                                } else {
                                    CustomAlert(response);
                                }
                            }
                        });
                    });

                },
                "drawCallback": function () {
                    $('.fa-angle-right').click(function (eve) {
                        eve.stopImmediatePropagation();
                        var tr = $(this).closest('tr');
                        //var row = table.row(tr);
                        var row = tableInspCheckList.row(tr);
                        var ContentID = this.id.split('_')[1];

                        var visible = $(this).parent().parent().hasClass("shown");
                        var anglechange = $(this).hasClass("fa-angle-right");
                        if (anglechange == true) {
                            var x = tr.children()[0];
                            $(this).removeClass("fa-angle-right");
                            $(this).addClass("fa-angle-down");
                        }
                        else {
                            $(this).addClass("fa-angle-right");
                            $(this).removeClass("fa-angle-down");
                        }
                        if (visible == true) {
                            $(this).parent().parent().next('tr').remove();
                            //  $(this).parent().parent().next('tr').empty();
                            $(this).parent().parent().removeClass('shown');
                        }
                        else {

                            var Temp = GetInspectionCheckListContentData(ContentID);
                            row.child(Temp).show();
                            row.child.show();
                            tr.addClass('shown');
                        }
                    });
                }

            });
            $('#LoadingPanel1').hide();

            // Hide Show For DataTable Column And Buttons Start Here

            GridButtonHideShow(tableInspCheckList, "tblInspectionCheckList");

            // Hide Show For DataTable Column And Buttons End Here
        }
    })

}
function clearArray(array) {
    while (array.length) {
        array.pop();
    }
}
function htmlAttributeEncode(str) {
    if (!str) return '';
    return str.replace(/&/g, '&amp;')  // always escape &
        .replace(/"/g, '&quot;')
        .replace(/'/g, '&#39;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;');
}
function ShowDeletePopup(ContentID, TableValues, hdnChildContentID) {

    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    $('#hdnDeleteFlag').val(TableValues);
    $('#hdnContentID').val(ContentID);
    $('#hdnChildContentID').val(hdnChildContentID);
    $.ajax({
        type: "POST",
        url: "/InspectionCheckListContent/ShowDeletePopup",
        data: JSON.stringify({ id: id, ContentID: ContentID, TableValues: TableValues }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            var checkDiv = response;
            if (checkDiv == "divChild") {
                $('#divChild').css('display', 'block');
            } if (checkDiv == "divdelete") {
                $('#divdelete').css('display', 'block');
            }
        }
    });
}

function AddNewRowLevel2(ContentID) {
    //var str = "<tr><td style='display:none;'></td><td></td><td></td><td></td><td></td><td><input type='text' id='txtCheckListContentLevel2_0' class='form-control txtEditColor'/></td><td><input id='checkboxNALevel2_0' type='checkbox'/><span>&nbsp NA</span></td></tr>";
    //$("#tbodycheclListCont_" + ContentID).prepend(str);
    var str = "<tr><td style='display:none;'></td><td></td><td><input type='text' id='txtCheckListContentLevel2_0' class='form-control txtEditColor'/></td><td><input id='checkboxNALevel2_0' type='checkbox'/><span>&nbsp NA</span></td></tr>";
    $("#tbodycheclListCont_" + ContentID).prepend(str);
}
function GetInspectionCheckListContentData(ContentID) {

    $('#LoadingPanel1').show();
    var id = getParameterByName('id');
    var str = "";
    str = '<table id="tblcheclListCont_' + ContentID + '" class="clone"  cellspacing="0" border="0" style="padding-left:50px;width:100%; background:antiquewhite;">';
    str += '<thead><tr style="background:antiquewhite;">';
    str += '<th style="display:none"></th>';
    str += '<th style="display:none"></th>';
    str += '<th style="display:none"></th>';
    str += '<th style="width:25%;"></th>';
    //str += '<th style="width:5%;text-align:center;"></th>';
    //str += '<th style="width:5%;text-align:center;"></th>';
    //str += '<th style="width:5%;text-align:center;"></th>';
    str += '<th style="width:60%;text-align:center">Level 2</th>';
    str += '<th style="width:15%;text-align:center"></th>';

    str += '</tr></thead><tbody id="tbodycheclListCont_' + ContentID + '">';
    //str += '</tbody></table>';
    $.ajax({
        type: "POST",
        url: "/InspectionCheckListContent/GetInspectionCheckListContentData",
        data: JSON.stringify({ id: id, ContentID: ContentID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            // var data = response.d;
            $("#tblcheclListCont_" + ContentID).DataTable().clear();
            $("#tblcheclListCont_" + ContentID).DataTable().destroy();
            var data = response;
            if (data.length > 0) {


                for (var j = 0; j < data.length; j++) {

                    var ContentIDLevel2 = data[j].ContentID == null ? "" : data[j].ContentID;
                    var CheckListContent = data[j].CheckListContent == null ? "" : data[j].CheckListContent;
                    var CheckListID = data[j].CheckListID == null ? "" : data[j].CheckListID;
                    var checklistItem = data[j].checklistItem == null ? "" : data[j].checklistItem;
                    var subChildItem = data[j].subChildItem == null ? "" : data[j].subChildItem;
                    var ClientID = data[j].ClientID == null ? "" : data[j].ClientID;
                    var ItemOrder = data[j].itemOrder == null ? "" : data[j].itemOrder;
                    var IsNA = data[j].IsNA == null ? "" : data[j].IsNA;

                    CheckListContent = htmlAttributeEncode(CheckListContent);
                    str += '<tr style="background:antiquewhite;">';
                    str += '<td id="ContentIDLevel2_' + j + '" style="display:none">' + ContentIDLevel2 + '</td>';
                    str += '<td id="CheckListIDLevel2_' + ContentIDLevel2 + '" style="display:none">' + CheckListID + '</td>';
                    str += '<td id="ItemOrderLevel2_' + ContentIDLevel2 + '" style="display:none">' + ItemOrder + '</td>';

                    str += '<td style="cursor:pointer;"><a id="ContentIDUP_' + j + '" class="btnUpLevel2"><i style="color:grey" class="fa fa-arrow-up"></i></a>&nbsp;&nbsp;<a id="ContentIDDown_' + j + '" class="btnDownLevel2"><i style="color:grey" class="fa fa-arrow-down"></i>&nbsp;</a>&nbsp;&nbsp;<i class="fa fa-trash ClassbtnCheckListLebel2Delete" onclick=ShowDeletePopup("' + ContentIDLevel2 + '","ViewLevel2","' + ContentID + '")></i>&nbsp;&nbsp;<a id="SiteID_' + ContentIDLevel2 + '" class="btnCheckListEditLevel2 "><i class="fa fa-pencil"></i>&nbsp;</a></td>';

                    //str += '<td style="cursor:pointer;"><a id="ContentIDUP_' + j + '" class="btnUpLevel2"><i style="color:grey" class="fa fa-arrow-up"></i></a></td>';
                    //str += '<td style="cursor:pointer;"><a id="ContentIDDown_' + j + '" class="btnDownLevel2"><i style="color:grey" class="fa fa-arrow-down"></i>&nbsp;</a></td>';
                    //str += '<td title="Click here for Delete" style="cursor:pointer;"><i class="fa fa-trash" onclick=ShowDeletePopup("'+ContentIDLevel2 + '","ViewLevel2")></i></td>';
                    //str += '<td title="Click here to edit CheckList." style="cursor:pointer;"><a id="SiteID_' + ContentIDLevel2 + '" class="btnCheckListEditLevel2"><i class="fa fa-pencil"></i>&nbsp;</a></td>';
                    str += '<td id="CheckListContentInputTypeLevel2_' + ContentIDLevel2 + '"><input id="txtCheckListContentLevel2_' + ContentIDLevel2 + '" type="text" style="display:none;" class="form-control" value="' + CheckListContent + '"><label id="lblCheckListContentLevel2_' + ContentIDLevel2 + '">' + CheckListContent + '</label></td>'
                    if (IsNA == true) {
                        str += '<td><input type="checkbox" id="checkboxNALevel2_' + ContentIDLevel2 + '" checked disabled/><span> &nbspNA</span></td>'
                    } else {
                        str += '<td><input id="checkboxNALevel2_' + ContentIDLevel2 + '" type="checkbox" disabled/><span>&nbsp NA</span></td>'
                    }
                    str += '</tr>';
                }

            }

            str += '</tbody></table>';
            $("#tblcheclListCont_" + ContentID).html(str);
            $('#LoadingPanel1').hide();

            tableCheckListContentData = $("#tblcheclListCont_" + ContentID).DataTable({

                searching: true,
                paging: false,
                info: false,
                autoWidth: true,
                length: false,
                sort: false,

                initComplete: function () {
                    //$("#tblLocation_" + ContentID + "_length").children().html('<button id="btnAddNewLocation"  type="button" class="w3-btn w3-teal fa fa-plus btnAddNewLocation">&nbsp;Add New Location</button>');

                    $("#tblcheclListCont_" + ContentID + "_filter").children().html('<a id="btnAddNewCheckListLevel2" style="cursor:pointer;" class="fa fa-plus"></a>&nbsp;&nbsp<a id="btnCheckListSaveLevel2" style="cursor:pointer;" class="fa fa-save"></a>&nbsp;&nbsp<a id="btnRefreshLevel2" style="cursor:pointer;" class="fa fa-remove"></a>');

                    $("#btnAddNewCheckListLevel2").click(function () {
                        //alert('btnAddNewCheckList ');
                        // AddNewRowLevel2(ContentID);
                        let txtCheckListContentLevel2_0 = $('#txtCheckListContentLevel2_0').val();
                        if (txtCheckListContentLevel2_0 != '') {
                            AddNewRowLevel2(ContentID);
                        } else {
                            $('#txtCheckListContentLevel2_0').css('border-color', 'red');
                        }
                    });

                    $("#btnCheckListSaveLevel2").click(function () {
                        $('#LoadingPanel1').show();

                        $('#tblcheclListCont_' + ContentID + ' td input').each(function () {
                            if ($(this).hasClass("txtEditColor")) {
                                var ContentIDLevel2 = this.id.split('_')[1];
                                var CheckListContent = this.value;
                                var IsNA = $('#checkboxNALevel2_' + ContentIDLevel2).prop('checked');
                                var CheckListID = $('#CheckListIDLevel2_' + ContentIDLevel2).text();
                                //alert(this.value);
                                var ParentContentID = "";
                                if (ContentIDLevel2 == 0) {
                                    CheckListID = $('#ddlCheckList').val();
                                    ParentContentID = ContentID;
                                }
                                if (CheckListContent != '') {
                                    var ArryVal = { ContentID: ContentIDLevel2, CheckListID: CheckListID, CheckListContent: CheckListContent, IsNA: IsNA, ParentContentID: ParentContentID };
                                    InspectionCheckListArryData.push(ArryVal);
                                } else {
                                    $('#txtCheckListContent_' + ContentIDLevel2).css('border-color', 'red');
                                }
                                //var ArryVal = { ContentID: ContentIDLevel2, CheckListID: CheckListID, CheckListContent: CheckListContent, IsNA: IsNA, ParentContentID: ParentContentID };
                                //InspectionCheckListArryData.push(ArryVal);
                            }

                        });

                        var id = getParameterByName('id');
                        var UID = getParameterByName('UID');
                        var TableValues = "ViewLevel2";
                        if (InspectionCheckListArryData.length > 0) {
                            $.ajax({
                                type: "POST",
                                url: "/InspectionCheckListContent/InspectionCheckListSaveData",
                                data: JSON.stringify({ id: id, UID: UID, TableValues: TableValues, InspectionCheckListVal: InspectionCheckListArryData }),
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                async: true,
                                success: function (response) {
                                    if (response.Result == "Success") {
                                        CustomAlert(response);
                                        // GetInspectionCheckListData();
                                        GetInspectionCheckListContentData(ContentID);
                                        clearArray(InspectionCheckListArryData);
                                    } else {
                                        CustomAlert(response);
                                        $('#LoadingPanel1').hide();
                                        clearArray(InspectionCheckListArryData);
                                    }
                                }
                            });
                        } else {
                            $('#LoadingPanel1').hide();
                        }
                    });
                    $("#btnRefreshLevel2").click(function () {
                        // GetInspectionCheckListData();
                        GetInspectionCheckListContentData(ContentID);
                    });
                    $(".btnCheckListEditLevel2").click(function () {
                        event.stopImmediatePropagation();

                        var ContentID = this.id.split('_')[1];
                        $('#txtCheckListContentLevel2_' + ContentID).show();
                        $('#lblCheckListContentLevel2_' + ContentID).hide();
                        $('#txtCheckListContentLevel2_' + ContentID).addClass('txtEditColor');
                        $('#checkboxNALevel2_' + ContentID).prop('disabled', false);

                    });
                    $(".btnUpLevel2").click(function () {
                        event.stopImmediatePropagation();


                        var rowindex = this.id.split('_')[1];
                        var ContentIDLevel2 = $('#ContentIDLevel2_' + rowindex).text();
                        var CheckListID = $('#CheckListIDLevel2_' + ContentIDLevel2).text();
                        var itemOrder = $('#ItemOrderLevel2_' + ContentIDLevel2).text();
                        var OrderContentID2 = $('#ContentIDLevel2_' + (parseInt(rowindex) - 1)).text();

                        var id = getParameterByName('id');
                        var UID = getParameterByName('UID');

                        $.ajax({
                            type: "POST",
                            url: "/InspectionCheckListContent/ShortOrderUpLevel2",
                            data: JSON.stringify({ id: id, UID: UID, OrderContentID: ContentIDLevel2, OrderContentID2: OrderContentID2, itemorder: itemOrder, CheckListID: CheckListID }),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: true,
                            success: function (response) {
                                if (response.Result == "Success") {
                                    // GetInspectionCheckListData();
                                    GetInspectionCheckListContentData(ContentID);
                                } else {
                                    CustomAlert(response);
                                }
                            }
                        });
                    });
                    $(".btnDownLevel2").click(function () {
                        event.stopImmediatePropagation();


                        var rowindex = this.id.split('_')[1];
                        var ContentIDLevel2 = $('#ContentIDLevel2_' + rowindex).text();
                        var CheckListID = $('#CheckListIDLevel2_' + ContentIDLevel2).text();
                        var itemOrder = $('#ItemOrderLevel2_' + ContentIDLevel2).text();
                        var OrderContentID2 = $('#ContentIDLevel2_' + (parseInt(rowindex) + 1)).text();

                        var id = getParameterByName('id');
                        var UID = getParameterByName('UID');

                        $.ajax({
                            type: "POST",
                            url: "/InspectionCheckListContent/ShortOrderDownLevel2",
                            data: JSON.stringify({ id: id, UID: UID, OrderContentID: ContentIDLevel2, OrderContentID2: OrderContentID2, itemorder: itemOrder, CheckListID: CheckListID }),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: true,
                            success: function (response) {
                                if (response.Result == "Success") {
                                    // GetInspectionCheckListData();
                                    GetInspectionCheckListContentData(ContentID);
                                } else {
                                    CustomAlert(response);
                                }
                            }
                        });
                    });
                },

            });

            // Hide Show For DataTable Column And Buttons Start Here

            GridButtonHideShow(tableCheckListContentData, "tblInspectionCheckListLebel2");

            // Hide Show For DataTable Column And Buttons End Here
        }
    });
    return str;
    //    // `d` is the original data object for the row
}

function RadiobtnCreateNew_CheckedChanged() {

    $('#rowChecklistNew').css('display', 'block');
    $('#lblChecklistName').css('display', 'block');
    $('#txtCheckList').css('display', 'block');
    $('#btnSave').css('display', 'block');
    //rowChecklistNew.Style.Add("display", "block");
    //lblChecklistName.Style.Add("display", "block");
    //txtCheckList.Style.Add("display", "block");
    //btnSave.Style.Add("display", "block");
    //RadioButtonBind();

    $('#rowCheckListCopy').css('display', 'none');
    $('#lblCheckListCopy').css('display', 'none');
    $('#lblcopy').css('display', 'none');
    $('#ddlCheckListFrom').css('display', 'none');
    $('#txtcopy').css('display', 'none');
    $('#btncopy').css('display', 'none');

    //rowCheckListCopy.Style.Add("display", "none");
    //lblCheckListCopy.Style.Add("display", "none");
    //lblcopy.Style.Add("display", "none");
    //ddlCheckListFrom.Style.Add("display", "none");
    //txtcopy.Style.Add("display", "none");
    //btncopy.Style.Add("display", "none");

    $('#rowCheckload').css('display', 'none');
    $('#lblLoad').css('display', 'none');
    $('#txtFileChecklist').css('display', 'none');
    $('#btnLoad').css('display', 'none');
    $('#lblselectfile').css('display', 'none');
    $('#FileUploadSelectfile').css('display', 'none');
    $('#btndownload').css('display', 'none');

    //rowCheckload.Style.Add("display", "none");
    //lblLoad.Style.Add("display", "none");
    //txtFileChecklist.Style.Add("display", "none");
    //btnLoad.Style.Add("display", "none");
    //lblselectfile.Style.Add("display", "none");
    //FileUploadSelectfile.Style.Add("display", "none");
    //btndownload.Style.Add("display", "none");

    $('#RowCheckListDelete').css('display', 'none');
    $('#lblCheckListCopy').css('display', 'none');
    $('#lblcopy').css('display', 'none');
    $('#ddlCheckListFrom').css('display', 'none');

    //RowCheckListDelete.Style.Add("display", "none");
    //lblCheckListCopy.Style.Add("display", "none");
    //lblcopy.Style.Add("display", "none");
    //ddlCheckListFrom.Style.Add("display", "none");
}

function RadiobtnCopy_CheckedChanged() {

    DropdownBind();
    // RadioButtonBind();


    $('#rowCheckListCopy').css('display', 'block');
    $('#lblCheckListCopy').css('display', 'block');
    $('#lblcopy').css('display', 'block');
    $('#ddlCheckListFrom').css('display', 'block');
    $('#txtcopy').css('display', 'block');
    $('#btncopy').css('display', 'block');

    //rowCheckListCopy.Style.Add("display", "block");
    //lblCheckListCopy.Style.Add("display", "block");
    //lblcopy.Style.Add("display", "block");
    //ddlCheckListFrom.Style.Add("display", "block");
    //txtcopy.Style.Add("display", "block");
    //btncopy.Style.Add("display", "block");



    //ddlCheckListFrom.ClearSelection();
    //txtcopy.Text = "";
    //ddlCheckListFrom.Text = "";

    $('#rowCheckload').css('display', 'none');
    $('#lblLoad').css('display', 'none');
    $('#txtFileChecklist').css('display', 'none');
    $('#btnLoad').css('display', 'none');
    $('#lblselectfile').css('display', 'none');
    $('#FileUploadSelectfile').css('display', 'none');
    $('#btndownload').css('display', 'none');

    //rowCheckload.Style.Add("display", "none");
    //lblLoad.Style.Add("display", "none");
    //txtFileChecklist.Style.Add("display", "none");
    //btnLoad.Style.Add("display", "none");
    //lblselectfile.Style.Add("display", "none");
    //FileUploadSelectfile.Style.Add("display", "none");
    //btndownload.Style.Add("display", "none");

    $('#rowChecklistNew').css('display', 'none');
    $('#lblChecklistName').css('display', 'none');
    $('#txtCheckList').css('display', 'none');
    $('#btnSave').css('display', 'none');

    //rowChecklistNew.Style.Add("display", "none");
    //lblChecklistName.Style.Add("display", "none");
    //txtCheckList.Style.Add("display", "none");
    //btnSave.Style.Add("display", "none");

    $('#RowCheckListDelete').css('display', 'none');
    $('#lblDeleteList').css('display', 'none');
    $('#ddlDeleteList').css('display', 'none');
    $('#btnDeleteList').css('display', 'none');

    //RowCheckListDelete.Style.Add("display", "none");
    //lblDeleteList.Style.Add("display", "none");
    ////  lblcopy.Style.Add("display", "block");
    //ddlDeleteList.Style.Add("display", "none");
    //btnDeleteList.Style.Add("display", "none");
}

function RadiobtnLoad_CheckedChanged() {
    //RadioButtonBind();
    $('#rowCheckload').css('display', 'block');
    $('#lblLoad').css('display', 'block');
    $('#txtFileChecklist').css('display', 'block');
    $('#btnLoad').css('display', 'block');
    $('#lblselectfile').css('display', 'block');
    $('#FileUploadSelectfile').css('display', 'block');
    $('#btndownload').css('display', 'block');
    $('#txtFileChecklist').val("");

    //            rowCheckload.Style.Add("display", "block");
    //lblLoad.Style.Add("display", "block");
    //txtFileChecklist.Style.Add("display", "block");
    //btnLoad.Style.Add("display", "block");
    //lblselectfile.Style.Add("display", "block");
    //FileUploadSelectfile.Style.Add("display", "block");
    //btndownload.Style.Add("display", "block");
    //txtFileChecklist.Text = "";
    //RadioButtonBind();

    $('#rowChecklistNew').css('display', 'none');
    $('#lblChecklistName').css('display', 'none');
    $('#txtCheckList').css('display', 'none');
    $('#btnSave').css('display', 'none');


    //rowChecklistNew.Style.Add("display", "none");
    //lblChecklistName.Style.Add("display", "none");
    //txtCheckList.Style.Add("display", "none");
    //btnSave.Style.Add("display", "none");

    $('#rowCheckListCopy').css('display', 'none');
    $('#lblCheckListCopy').css('display', 'none');
    $('#lblcopy').css('display', 'none');
    $('#ddlCheckListFrom').css('display', 'none');
    $('#txtcopy').css('display', 'none');
    $('#btncopy').css('display', 'none');

    //rowCheckListCopy.Style.Add("display", "none");
    //lblCheckListCopy.Style.Add("display", "none");
    //lblcopy.Style.Add("display", "none");
    //ddlCheckListFrom.Style.Add("display", "none");
    //txtcopy.Style.Add("display", "none");
    //btncopy.Style.Add("display", "none");

    $('#RowCheckListDelete').css('display', 'none');
    $('#lblDeleteList').css('display', 'none');
    $('#lblcopy').css('display', 'none');
    $('#ddlDeleteList').css('display', 'none');
    $('#btnDeleteList').css('display', 'none');

    //RowCheckListDelete.Style.Add("display", "none");
    //lblCheckListCopy.Style.Add("display", "none");
    //lblcopy.Style.Add("display", "none");
    //ddlCheckListFrom.Style.Add("display", "none");
    //txtcopy.Style.Add("display", "none");
    //btncopy.Style.Add("display", "none");
}
function RadiobtnDelete_CheckedChanged() {
    //  dropdownChecklist();
    //  DropdownBind();
    //rowChecklistNew.Style.Add("display", "none");
    //lblChecklistName.Style.Add("display", "none");
    //txtCheckList.Style.Add("display", "none");
    //btnSave.Style.Add("display", "none");

    $('#rowChecklistNew').css('display', 'none');
    $('#lblChecklistName').css('display', 'none');
    $('#txtCheckList').css('display', 'none');
    $('#btnSave').css('display', 'none');


    //rowCheckListCopy.Style.Add("display", "none");
    //lblCheckListCopy.Style.Add("display", "none");
    //lblcopy.Style.Add("display", "none");
    //ddlCheckListFrom.Style.Add("display", "none");
    //txtcopy.Style.Add("display", "none");
    //btncopy.Style.Add("display", "none");

    $('#rowCheckListCopy').css('display', 'none');
    $('#lblCheckListCopy').css('display', 'none');
    $('#lblcopy').css('display', 'none');
    $('#ddlCheckListFrom').css('display', 'none');
    $('#txtcopy').css('display', 'none');
    $('#btncopy').css('display', 'none');

    //rowCheckload.Style.Add("display", "none");
    //lblLoad.Style.Add("display", "none");
    //txtFileChecklist.Style.Add("display", "none");
    //btnLoad.Style.Add("display", "none");
    //lblselectfile.Style.Add("display", "none");
    //FileUploadSelectfile.Style.Add("display", "none");
    //btndownload.Style.Add("display", "none");

    $('#rowCheckload').css('display', 'none');
    $('#lblLoad').css('display', 'none');
    $('#txtFileChecklist').css('display', 'none');
    $('#btnLoad').css('display', 'none');
    $('#lblselectfile').css('display', 'none');
    $('#FileUploadSelectfile').css('display', 'none');
    $('#btndownload').css('display', 'none');

    //RowCheckListDelete.Style.Add("display", "block");
    //lblDeleteList.Style.Add("display", "block");
    //ddlDeleteList.Style.Add("display", "block");
    //ddlCheckListFrom.Style.Add("display", "block");
    //btnDeleteList.Style.Add("display", "block");

    // dropdownDelete();

    $('#RowCheckListDelete').css('display', 'block');
    $('#lblDeleteList').css('display', 'block');
    $('#ddlDeleteList').css('display', 'block');
    $('#ddlCheckListFrom').css('display', 'block');
    $('#btnDeleteList').css('display', 'block');

}

function btnSave_Click() {
    $('#LoadingPanel1').show();
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var txtCheckList = $('#txtCheckList').val();

    if (txtCheckList != "") {
        $.ajax({
            type: "POST",
            url: "/InspectionCheckListContent/btnSave_Click",
            data: JSON.stringify({ id: id, UID: UID, txtCheckList: txtCheckList }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (response) {
                if (response.Result == "Success") {
                    $('#txtCheckList').val("");
                    var CheckListID = response.CheckListID;
                    DropdownBind();
                    CustomAlert(response);
                    setTimeout(function () {
                        if (CheckListID != "") {
                            $('#ddlCheckList').val(CheckListID);
                        }
                        GetInspectionCheckListData();
                        RadioButtonBind();
                    }, 500);

                } else {
                    CustomAlert(response);
                    $('#LoadingPanel1').hide();
                }
            }
        });
    } else {
        var Response = { Result: "Failed", Message: "Please fill Location." };
        CustomAlert(Response);
        $('#LoadingPanel1').hide();
        $('#txtCheckList').addClass('requiredColumn');
    }
}

function btncopy_Click() {
    var ddlCheckList = $('#ddlCheckListFrom').val();
    var ddlCheckListFrom = $('#ddlCheckListFrom option:selected').text();
    var txtcopy = $('#txtcopy').val()
    if (ddlCheckList != "") {
        if (txtcopy != "") {
            $('#c1').text("Copying question from checklist ''" + ddlCheckListFrom + "'' to new checklist ''" + txtcopy + "'' ?")
            $('#divCopy').css('display', 'block');
            //c1.InnerText = "Copying question from checklist ''" + ddlCheckListFrom.Text + "'' to new checklist ''" + txtcopy.Text + "'' ?";
            //divCopy.Style.Add("display", "block");
        } else {
            var Response = { Result: "Failed", Message: "Please select any checklist." };
            CustomAlert(Response);
        }
    } else {
        var Response = { Result: "Failed", Message: "Please enter Checklist Name ." };
        CustomAlert(Response);
    }


}

function btnCopyyes_Click() {
    $('#LoadingPanel1').show();
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var ddlCheckListFrom = $('#ddlCheckListFrom').val();
    var txtcopy = $('#txtcopy').val();

    $.ajax({
        type: "POST",
        url: "/InspectionCheckListContent/btnCopyyes_Click",
        data: JSON.stringify({
            id: id,
            UID: UID,
            ddlCheckListFrom: ddlCheckListFrom,
            txtcopy: txtcopy
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            $('#LoadingPanel1').hide(); // Hide loader at the top for all cases

            if (response.Result === "Success") {
                $('#divCopy').css("display", "none");
                $('#txtcopy').val("");
                var CheckListID = response.CheckListID;
                DropdownBind();
                CustomAlert(response);
                setTimeout(function () {
                    if (CheckListID != "") {
                        $('#ddlCheckList').val(CheckListID);
                    }
                    RadioButtonBind();
                    GetInspectionCheckListData();

                }, 500);

            } else {

                var Response = { Result: "Failed", Message: response.Message };
                CustomAlert(Response);
            }
        },
        error: function (xhr, status, error) {
            $('#LoadingPanel1').hide();
            CustomAlert({ Result: "Error", Message: "An unexpected error occurred: " + error });
        }
    });
}

function btnCopyno_Click() {
    $('#divCopy').css("display", "none");
}

function btnDeleteList_Click() {
    var ddlDeleteList = $('#ddlDeleteList').val();
    if (ddlDeleteList != "" && ddlDeleteList != null) {
        $('#divdeletechild').css("display", "block");
        //    divdeletechild.Style.Add("display", "block");
    } else {
        var Response = { Result: "Failed", Message: "Please select any checklist." };
        CustomAlert(Response);
    }
}
function btnDeleteYesLast_Click() {
    $('#LoadingPanel1').show();
    var ddlDeleteList = $('#ddlDeleteList').val();
    $.ajax({
        type: "POST",
        url: "/InspectionCheckListContent/btnDeleteYesLast_Click",
        data: JSON.stringify({ ddlDeleteList: ddlDeleteList }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            if (response.Result == "Success") {
                $('#divdeletechild').css("display", "none");
                DropdownBind();
                RadioButtonBind();
                GetInspectionCheckListData();
                CustomAlert(response);

            } else {
                CustomAlert(response);
                $('#divdeletechild').css("display", "none");
                $('#LoadingPanel1').hide();
            }
        }
    });

}
function btnDeleteNoLast_Click() {
    $('#divdeletechild').css("display", "none");
    //    divdeletechild.Style.Add("display", "none");
}

function btndeleteyes_Click() {

    $('#LoadingPanel1').show();
    var DeleteFlag = $('#hdnDeleteFlag').val();
    var ContentID = $('#hdnContentID').val();
    var id = getParameterByName('id');
    $.ajax({
        type: "POST",
        url: "/InspectionCheckListContent/btndeleteyes_Click",
        data: JSON.stringify({ id: id, DeleteFlag: DeleteFlag, ContentID: ContentID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            if (response.Result == "Success") {
                $('#divdelete').css("display", "none");
                $('#divChild').css("display", "none");
                // DropdownBind();
                RadioButtonBind();
                if ($('#hdnDeleteFlag').val() == "ViewLevel2") {
                    var hdnChildContentID = $('#hdnChildContentID').val();
                    GetInspectionCheckListContentData(hdnChildContentID);

                } else {
                    GetInspectionCheckListData();
                }
                CustomAlert(response);

            } else {
                CustomAlert(response);
                $('#LoadingPanel1').hide();
                $('#divdelete').css("display", "none");
                $('#divChild').css("display", "none");
            }
        }
    });

}
function btndeleteno_Click() {
    $('#divdelete').css('display', 'none');
    $('#divChild').css('display', 'none');
    //divdelete.Style.Add("display", "none");
    //divChild.Style.Add("display", "none");
}

function RadioButtonActive_CheckedChanged() {
    $('#LoadingPanel1').show();
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var ddlCheckList = $('#ddlCheckList').val();
    $.ajax({
        type: "POST",
        url: "/InspectionCheckListContent/RadioButtonActive_CheckedChanged",
        data: JSON.stringify({ id: id, UID: UID, ddlCheckList: ddlCheckList }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            $('#LoadingPanel1').hide();
            //if (response.Result == "Success") {

            //}
        }
    });

}

function RadioButtonInActive_CheckedChanged() {

    $('#LoadingPanel1').show();
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var ddlCheckList = $('#ddlCheckList').val();
    $.ajax({
        type: "POST",
        url: "/InspectionCheckListContent/RadioButtonInActive_CheckedChanged",
        data: JSON.stringify({ id: id, UID: UID, ddlCheckList: ddlCheckList }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {

            if (response.Result == "Success") {
                // PlaceHolder2.Controls.Add(new Literal { Text = html.ToString() });
                //if (response.StringVal != "") {
                //    $('#PlaceHolder2').empty().append(response.StringVal);
                //    $('#divActivePopup').css("display", "block");

                //} 
                // CustomAlert(response);
                $('#LoadingPanel1').hide();
            }
            else if (response.Result == "FailedForClient") {
                var Response = { Result: "Failed", Message: response.Message };
                CustomAlert(Response);
                $('#LoadingPanel1').hide();
                $('#RadioButtonInActive').prop('checked', false);
                $('#RadioButtonActive').prop('checked', true);
            }
            else {
                if (response.lblChecklistDis != "") {
                    //PlaceHolder1.Controls.Add(new Literal { Text = html.ToString() });
                    // lblChecklistDis.InnerText = dt.Rows[0]["checklistdescription"].ToString();
                    // divActive.Style.Add("display", "block");
                    $('#PlaceHolder1').empty().append(response.StringVal);
                    $('#lblChecklistDis').text(response.lblChecklistDis);
                    $('#divActive').css("display", "block");

                } else {
                    $('#PlaceHolder2').empty().append(response.StringVal);
                    $('#divActivePopup').css("display", "block");
                }
                $('#LoadingPanel1').hide();
            }
        }

    });

}

function btnActiveYes_Click() {

    $('#LoadingPanel1').show();
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var ddlCheckList = $('#ddlCheckList').val();
    $.ajax({
        type: "POST",
        // url: "/InspectionCheckListContent/RadioButtonActive_CheckedChanged",
        url: "/InspectionCheckListContent/btnActiveYes_Click",
        data: JSON.stringify({ id: id, UID: UID, ddlCheckList: ddlCheckList }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            CustomAlert(response);
            $('#LoadingPanel1').hide();
            //$('#RadioButtonInActive').prop('checked', false);
            //$('#RadioButtonActive').prop('checked', true);
            $('#divActive').css("display", "none");
            //DropdownBind();
            RadioButtonBind();
            GetInspectionCheckListData();

        }
    });

}

function btnInActiveNo_Click() {
    $('#divActive').css("display", "none");
    $('#RadioButtonInActive').prop('checked', false);
    $('#RadioButtonActive').prop('checked', true);
    //            divActive.Style.Add("display", "none");
    //RadioButtonInActive.Checked = false;
    //RadioButtonActive.Checked = true;
}

function divActivePopupNo_Click() {
    $('#divActivePopup').css("display", "none");
    $('#RadioButtonInActive').prop('checked', false);
    $('#RadioButtonActive').prop('checked', true);
    //  divActivePopup.Style.Add("display", "none");
    //RadioButtonInActive.Checked = false;
    //RadioButtonActive.Checked = true;

}

function divActivePopupYes_Click() {

    $('#LoadingPanel1').show();
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var ddlCheckList = $('#ddlCheckList').val();
    $.ajax({
        type: "POST",
        url: "/InspectionCheckListContent/divActivePopupYes_Click",
        data: JSON.stringify({ id: id, UID: UID, ddlCheckList: ddlCheckList }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            $('#LoadingPanel1').hide();
            // DropdownBind();
            RadioButtonBind();
            GetInspectionCheckListData();
            CustomAlert(response);
            // divActivePopup.Style.Add("display", "none");
            $('#divActivePopup').css("display", "none");
        }
    });

    //    //query = "update tblregister set ChecklistId=Null where ClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and checklistID='" + ddlCheckList.SelectedValue + "'";
    //    // Query Change On 21 Jan 2020 Add UpdatedBy And UpdatedOn
    //    query = "update tblregister set ChecklistId=Null ,UpdatedBy =(select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(Request.QueryString["UID"]) + "'),UpdatedOn = '" + DateTime.Now + "' where ClientId='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and checklistID='" + ddlCheckList.SelectedValue + "'";
    //int result = databaseCon.RunCmd(query);
    //if (result != 0)
    //{
    //    // query = "update tblchecklist set status='inactive' where clientid='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and checklistid='" + ddlCheckList.SelectedValue + "'";
    //    // Query Change On 21 Jan 2020 Add UpdatedBy And UpdatedOn
    //    query = "update tblchecklist set status='InActive' ,UpdatedBy =(select EmailId from tblUser where UserId = '" + databaseCon.Decrypt(Request.QueryString["UID"]) + "'),UpdatedOn = '" + DateTime.Now + "' where clientid='" + databaseCon.Decrypt(Request.QueryString["id"]) + "' and checklistid='" + ddlCheckList.SelectedValue + "'";
    //    int result1 = databaseCon.RunCmd(query);
    //}
    //divActivePopup.Style.Add("display", "none");
}

function RadioBtnColorGreenRed_CheckedChanged() {
    $('#LoadingPanel1').show();
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    $.ajax({
        type: "POST",
        url: "/InspectionCheckListContent/RadioBtnColorGreenRed_CheckedChanged",
        data: JSON.stringify({ id: id, UID: UID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            $('#LoadingPanel1').hide();
            // DropdownBind();
            RadioButtonBind();
            GetInspectionCheckListData();
            // CustomAlert(response);
        }
    });

}

function RadioBtnColorBlack_CheckedChanged() {
    $('#LoadingPanel1').show();
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');

    $.ajax({
        type: "POST",
        url: "/InspectionCheckListContent/RadioBtnColorBlack_CheckedChanged",
        data: JSON.stringify({ id: id, UID: UID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {

            $('#LoadingPanel1').hide();
            // DropdownBind();
            RadioButtonBind();
            GetInspectionCheckListData();

        }
    });

}

function btnsetdefault_Click() {
    $('#LoadingPanel1').show();
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var RID = getParameterByName('RID');
    var ChkID = getParameterByName('ChkID');
    var RadioButtonInActive = $('#RadioButtonInActive').is(':checked');
    var ddlCheckList = $('#ddlCheckList').val();
    $.ajax({
        type: "POST",
        url: "/InspectionCheckListContent/btnsetdefault_Click",
        data: JSON.stringify({ id: id, UID: UID, RID: RID, ChkID: ChkID, RadioButtonInActive: RadioButtonInActive, ddlCheckList: ddlCheckList }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            CustomAlert(response);
            $('#LoadingPanel1').hide();
        }
    });
}

function btnLoad_Click1() {
    $('#LoadingPanel1').show();
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

                var id = getParameterByName('id');
                var UID = getParameterByName('UID');
                var UserRole = getParameterByName('UserRole');
                var txtFileChecklist = $('#txtFileChecklist').val();

                fileData.append('id', id);
                fileData.append('UID', UID);
                fileData.append('txtFileChecklist', txtFileChecklist);
                if (txtFileChecklist != "") {
                    $.ajax({
                        url: '/InspectionCheckListContent/btnLoad_Click1',
                        type: "POST",
                        contentType: false, // Not to set any content header  
                        processData: false, // Not to process data  
                        data: fileData,
                        success: function (response) {
                            var Result = response;
                            if (response.Result == "Success") {
                                CustomAlert(response);
                                location.reload();
                            }
                            else {
                                CustomAlert(response);
                                // location.reload();
                                $('#LoadingPanel1').hide();
                            }
                        },
                        error: function (err) {
                            bootbox.alert(err.statusText, function () {
                                //location.reload();
                                $('#LoadingPanel1').hide();
                            });
                        }
                    });
                } else {
                    var Response = { Result: "Failed", Message: "Please Enter CheckList Name." };
                    CustomAlert(Response);
                    $('#LoadingPanel1').hide();
                }

            }
            else {

                //$('#art1').removeClass("w3-section w3-card-12  j-close j-alertBox-Success").addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
                //$('#alertlabel').text("Alert ! Please upload only xlsx files.");
                var Response = { Result: "Failed", Message: "Please upload only xlsx files." };
                CustomAlert(Response);
                $('#LoadingPanel1').hide();
            }

        }
        else {
            // $('#CompanyChoose').val("");
            var Response = { Result: "Failed", Message: "Please select files." };
            CustomAlert(Response);
            $('#LoadingPanel1').hide();
        }
    } else {
        alert("Your browser does not support FormData. Update it for a better user experience.");
        $('#LoadingPanel1').hide();
    }

}
function btndownload_Click() {
    $('#LoadingPanel1').show();
    $.ajax({
        type: "POST",
        url: "/InspectionCheckListContent/btndownload_Click",
        // data: JSON.stringify({ id: id, UID: UID, RID: RID, ChkID: ChkID, RadioButtonInActive: RadioButtonInActive, ddlCheckList: ddlCheckList }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            if (response.Result == "Success") {
                // CustomAlert(response);
                var path = response.Message;
                if (path != "") {
                    window.open(path);
                }
                $('#LoadingPanel1').hide();
            } else {
                CustomAlert(response);
                $('#LoadingPanel1').hide();
            }

        }
    });
}
function btnCAncel_Click() {
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var RID = getParameterByName('RID');

    window.location.href = '/Register/Register?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID;

    // Response.Redirect("Register.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RID=" + Request.QueryString["RID"], false);
}
