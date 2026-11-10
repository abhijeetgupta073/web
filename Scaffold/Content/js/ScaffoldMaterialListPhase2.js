$(document).ready(function () {
    $('#limaterialPhase2').removeClass('active').addClass('active');
    ShowJobInfoDetails();
    MaterialDropDown();

    var id = getParameterByName('id');
    var RID = getParameterByName('RID');
    GetInventoryData(id, RID, "All");

    $('#OptStatusHidden').val($('#txtoptstatus').val());

    var screensize = $(window).width();

    if (screensize < 1150 && screensize > 650) {
        ($BODY.hasClass('nav-md'))
        $('#CreateNewDropDown').css("margin-top", "2px");
        $BODY.removeClass('nav-md').addClass('nav-sm');
        $LEFT_COL.removeClass('scroll-view');
        //$('#BlackSection').css('display', 'block');
        $('#imgLogo').parent().css('display', 'none');
        $('#imgLogo1').css('display', 'block').css("height", "38px");
    }


    else {
        if (screensize < (650)) {
            $BODY.removeClass('nav-sm')
            $('#imgLogo').parent().css('display', 'none');
            $('#imgLogo1').css('display', 'block').css("height", "52px");


            $('.xsfieldset').css('margin-left', '-22px').css("width", "100%");
        }
        else {
            $('#CreateNewDropDown').css("margin-top", "6px");
            $BODY.removeClass('nav-sm').addClass('nav-md');
            //$('#BlackSection').css('display', 'none');
            $('#imgLogo').parent().css('display', 'block');
            $('#imgLogo1').css('display', 'none');
            $('#leftbarsection').css('margin-top', '40px');
        }

    }
});

//function show(txtQty, lblItemWeight, lblTotalweight) {
//    var textQuantity = $find(txtQty);
//    var lblItemWeight = $find(lblItemWeight);
//    var lblTotalweight = $find(lblTotalweight);
//    lblTotalweight.val(textQuantity.val() * lblItemWeight.val());

//}

//Added On (22-11-2023) to Restrict multiple insert this function will disable the button and will enable it once the process is complete.
$(document).ready(function () {
    $('#btnoptimizeyes').click(function () {
        $(this).prop('disabled', true); // Disable the button

        // Perform any necessary actions here

        setTimeout(function () {
            $('#btnoptimizeyes').prop('disabled', false); // Re-enable the button after 2 seconds
        }, 6000); // Set the time interval in milliseconds
    });
});

function MaterialDropDown() {

    var id = getParameterByName('id');
    $.ajax({
        type: "POST",
        url: "/ScaffoldMaterialListPhase2/MaterialDropDown",
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

    });
}

function ddlMaterialType_SelectedIndexChanged() {
    var id = getParameterByName('id');
    var RID = getParameterByName('RID');
    var Siteid = $('#hdnSiteLocID').val();
    var ddlMaterialType = $('#ddlMaterialType').val();
    if (ddlMaterialType == "" || ddlMaterialType == undefined) {
        ddlMaterialType = "All";
    }

    $.ajax({
        type: "POST",
        url: "/ScaffoldMaterialListPhase2/GetTextTotalData",
        data: JSON.stringify({ id: id, RID: RID, Siteid: Siteid, ddlMaterialType: ddlMaterialType }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            var GetData = response;
            $("#txtTotalDailyHireCost").val(GetData);
        }
    })

    GetInventoryData(id, RID, ddlMaterialType);
}
function GetInventoryData(id, RID, ddlMaterialType) {

    $('#LoadingPanel1').show();
    var Siteid = $('#hdnSiteLocID').val();

    $.ajax({
        type: "POST",
        url: "/ScaffoldMaterialListPhase2/GetInventoryData",
        data: JSON.stringify({ id: id, RID: RID, ddlMaterialType: ddlMaterialType, Siteid: Siteid }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            var str = "";
            var GetData = response;
            $('#tblRegisterInventoryList').DataTable().clear();
            $('#tblRegisterInventoryList').DataTable().destroy();
            var count = 0;
            if (GetData.length > 0) {
                $('#divTotalDailyHireCost').css('display', 'block');

                for (var i = 0; i < GetData.length; i++) {
                    var FromId = GetData[i].FromId == null ? "" : GetData[i].FromId;
                    var ToId = GetData[i].ToId == null ? "" : GetData[i].ToId;
                    var MaterialType = GetData[i].MaterialType == null ? "" : GetData[i].MaterialType;
                    var ItemCode = GetData[i].ItemCode == null ? "" : GetData[i].ItemCode;
                    var ItemDesc = GetData[i].ItemDesc == null ? "" : GetData[i].ItemDesc;
                    var InventoryId = GetData[i].InventoryId == null ? "" : GetData[i].InventoryId;
                    var DeliveryAvailableQuantity = GetData[i].DeliveryAvailableQuantity == null ? "" : GetData[i].DeliveryAvailableQuantity;
                    var ReturnALLQuantity = GetData[i].ReturnALLQuantity == null ? "" : GetData[i].ReturnALLQuantity;
                    var DamagedALLQuantity = GetData[i].DamagedALLQuantity == null ? "" : GetData[i].DamagedALLQuantity;
                    var AvailableQuantity = GetData[i].AvailableQuantity == null ? "" : GetData[i].AvailableQuantity;
                    var ItemWeight = GetData[i].ItemWeight == null ? "" : GetData[i].ItemWeight.toFixed(2);
                    var TotalWeight = GetData[i].TotalWeight == null ? "" : GetData[i].TotalWeight.toFixed(2);
                    var SubCategoryID = GetData[i].SubCategoryID == null ? "" : GetData[i].SubCategoryID;
                    var HireMeasurement = GetData[i].HireMeasurement == null ? "" : GetData[i].HireMeasurement;
                    var DailyHireRate = GetData[i].DailyHireRate == null ? 0 : GetData[i].DailyHireRate;
                    var TotalDailyHire = GetData[i].TotalDailyHire == null ? 0 : GetData[i].TotalDailyHire;


                    str += "<tr>"
                    str += "<td class='setWidth0' style='display:none'></td>"
                    str += "<td class='setWidth1'>" + MaterialType + "</td>"
                    str += "<td class='setWidth2'>" + ItemCode + "</td>"
                    str += "<td class='setWidth3'>" + ItemDesc + "</td>"
                    str += "<td class='setWidth4'>" + numberWithCommas(DeliveryAvailableQuantity) + "</td>"
                    str += "<td class='setWidth5'>" + numberWithCommas(ReturnALLQuantity) + "</td>"
                    str += "<td class='setWidth6'>" + numberWithCommas(DamagedALLQuantity) + "</td>"
                    str += "<td class='setWidth7'>" + numberWithCommas(AvailableQuantity) + "</td>"
                    str += "<td class='setWidth8'>" + numberWithCommas(ItemWeight) + "</td>"
                    str += "<td class='setWidth9'>" + numberWithCommas(TotalWeight) + "</td>"
                    str += "<td class='setWidth10'>" + HireMeasurement + " </td>"
                    str += "<td class='setWidth11'><span>$</span>" + DailyHireRate.toFixed(4) + " </td>"
                    str += "<td class='setWidth12'><span>$</span>" + TotalDailyHire.toFixed(4) + " </td>"

                    count += 1;
                }

                $('#tbodyRegisterInventoryList').append(str);

            }

            $('#LoadingPanel1').hide();
            $('#rowcount').val(count);

            var table = $('#tblRegisterInventoryList').DataTable({
                "bSort": false,
                "processing": true,
                searching: true,
                "bLengthChange": false,
                "bInfo": false,
                autoWidth: true,
                scrollX: true,
                scrollY: true,
                scrollCollapse: true,
                scrollY: "60vh",
                //"lengthMenu": [10, 25, 50, 100, 200],
                //"pageLength": 200,

            });
            if ($('#hdnHireInventory').val() == "False") {
                table.column(10).visible(false);
                table.column(11).visible(false);
                table.column(12).visible(false);
                $('#divTotalDailyHireCost').css('display', 'none');
            }
            // table.columns.adjust().draw();
        }
    })
}
function ExportTo_Click() {

    //  Response.Write("<script> window.open('ScafffoldMaterialListPhase2Report?UID=" + Request.QueryString["UID"] + "&RID=" + Request.QueryString["RID"] + "&id=" + Request.QueryString["id"] + "&btn=prv','_blank');</script>");
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var RegisterID = getParameterByName('RID');

    var path = '/ScafffoldMaterialListPhase2Report/ScafffoldMaterialListPhase2Report?UID=' + UID + '&RID=' + RegisterID + '&id=' + id;
    window.open(path);
}

function btnrequest_Click() {

     //Added On 30-08-2023 To add a check that if date tagged is not filled then you can not request material from job.
    var DateTagged = $('#txtdatetagged').val();
    if (DateTagged != "Not Yet Tagged") {
        var ClientID = getParameterByName('id');
        var UID = getParameterByName('UID');
        var UserRole = getParameterByName('UserRole');
        var RID = getParameterByName('RID');

        var Siteid = $('#hdnSiteLocID').val();

        var fromtocheck = "2";
        var page = "2";
        var from = "Job";
        var To = "Site";

        var EncryptData = Encrypt([fromtocheck, page, from, To]);
        fromtocheck = EncryptData[0];
        page = EncryptData[1];
        from = EncryptData[2];
        To = EncryptData[3];

        window.location.href = '/SiteRequest/SiteRequest?UID=' + UID + '&UserRole=' + UserRole + '&id=' + ClientID + '&FromId=' + RID + '&ToId=' + Siteid + '&from=' + from + '&To=' + To + '&fromtocheck=' + fromtocheck + '&page=' + page;
    }
    else
    {
        var Response = { Result: "Failed", Message: "You Can Not Request Item Without Filling Date Built." }
        CustomAlert(Response);
    }
    //      string query;
    //DataTable dt;
    //query = "exec Pro_TBLREGISTER_GET '" + databaseCon.Decrypt(Request.QueryString["RID"]) + "','" + databaseCon.Decrypt(Request.QueryString["id"]) + "','',''";
    //dt = databaseCon.GetData(query);
    //if (dt.Rows.Count != 0)
    //{
    //    string Siteid = Convert.ToString(dt.Rows[0]["SiteLocID"]);
    //    Response.Redirect("SiteRequest.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&FromId=" + Request.QueryString["RID"] + "&ToId=" + databaseCon.Encrypt(Siteid) + "&from=" + databaseCon.Encrypt("Job") + "&To=" + databaseCon.Encrypt("Site") + "&fromtocheck=" + databaseCon.Encrypt("2") + "&page=" + databaseCon.Encrypt("2") + "&gridpager=1");
    //}
}

function btnReturn_Click() {
    var ClientID = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var RID = getParameterByName('RID');

    var Siteid = $('#hdnSiteLocID').val();

    var fromtocheck = "2";
    var page = "2";

    var EncryptData = Encrypt([fromtocheck, page]);
    fromtocheck = EncryptData[0];
    page = EncryptData[1];

    window.location.href = '/ReturnInventoryDetails/ReturnInventoryDetails?UID=' + UID + '&UserRole=' + UserRole + '&id=' + ClientID + '&FromId=' + RID + '&ToId=' + Siteid + '&fromtocheck=' + fromtocheck + '&page=' + page;

    //      string query;
    //DataTable dt;
    //query = "exec Pro_TBLREGISTER_GET '" + databaseCon.Decrypt(Request.QueryString["RID"]) + "','" + databaseCon.Decrypt(Request.QueryString["id"]) + "','',''";
    //dt = databaseCon.GetData(query);
    //if (dt.Rows.Count != 0)
    //{
    //    string Siteid = Convert.ToString(dt.Rows[0]["SiteLocID"]);
    //    Response.Redirect("ReturnInventoryDetails.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&FromId=" + Request.QueryString["RID"] + "&ToId=" + databaseCon.Encrypt(Siteid) + "&fromtocheck=" + databaseCon.Encrypt("2") + "&page=" + databaseCon.Encrypt("2"));
    //}
}

function btnDamage_Click() {
    var ClientID = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var RID = getParameterByName('RID');

    var Siteid = $('#hdnSiteLocID').val();

    var fromtocheck = "2";
    var page = "2";

    var EncryptData = Encrypt([fromtocheck, page]);
    fromtocheck = EncryptData[0];
    page = EncryptData[1];

    window.location.href = '/DamagedInventoryDetails/DamagedInventoryDetails?UID=' + UID + '&UserRole=' + UserRole + '&id=' + ClientID + '&FromId=' + RID + '&ToId=' + Siteid + '&fromtocheck=' + fromtocheck + '&page=' + page;

    //       string query;
    //DataTable dt;
    //query = "exec Pro_TBLREGISTER_GET '" + databaseCon.Decrypt(Request.QueryString["RID"]) + "','" + databaseCon.Decrypt(Request.QueryString["id"]) + "','',''";
    //dt = databaseCon.GetData(query);
    //if (dt.Rows.Count != 0)
    //{
    //    string Siteid = Convert.ToString(dt.Rows[0]["SiteLocID"]);
    //    Response.Redirect("DamagedInventoryDetails.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&FromId=" + Request.QueryString["RID"] + "&ToId=" + databaseCon.Encrypt(Siteid) + "&fromtocheck=" + databaseCon.Encrypt("2") + "&page=" + databaseCon.Encrypt("2"));
    //}
}

function Viewlist_Click() {
    var ClientID = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var RID = getParameterByName('RID');

    var Siteid = $('#hdnSiteLocID').val();

    window.location.href = '/RequestInventory/RequestInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + ClientID + '&Siteid=' + Siteid + '&RID=' + RID;

    //      string query;
    //DataTable dt;
    //query = "exec Pro_TBLREGISTER_GET '" + databaseCon.Decrypt(Request.QueryString["RID"]) + "','" + databaseCon.Decrypt(Request.QueryString["id"]) + "','',''";
    //dt = databaseCon.GetData(query);
    //if (dt.Rows.Count != 0)
    //{
    //    string Siteid = Convert.ToString(dt.Rows[0]["SiteLocID"]);
    //    string Scafref = Convert.ToString(dt.Rows[0]["ShowRegisterId"]);
    //    Response.Redirect("RequestInventory.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&Siteid=" + databaseCon.Encrypt(Siteid) + "&RID=" + Request.QueryString["RID"]);
    //}

}

function btnHireReport_Click() {
    //      string path = "/DailyJobInventoryHireReport?UID=" + Request.QueryString["UID"] + "&RID=" + Request.QueryString["RID"] + "&id=" + Request.QueryString["id"] + "";
    //ScriptManager.RegisterStartupScript(this, GetType(), "newtab", "window.open('" + path + "')", true);
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var RegisterID = getParameterByName('RID');

    var path = '/DailyJobInventoryHireReport/DailyJobInventoryHireReport?UID=' + UID + '&id=' + id + '&RID=' + RegisterID;
    window.open(path);
}


function freeze_Click() {
    //  if (OptStatusHidden.Value != "Green" && Convert.ToInt32(rowcount.Value) > 0)
    // if ($('#OptStatusHidden').val() != "Green" && parseInt($('#rowcount').val()) >0)
    if ($('#txtoptstatus').val() != "Green" && parseInt($('#rowcount').val()) > 0) {
        // freezepopup.Style.Add("display", "block");
        $('#freezepopup').css("display", "block");
    }
    else {
        //  if (Convert.ToInt32(rowcount.Value) > 0)
        if (parseInt($('#rowcount').val()) > 0) {
            $('#freezepopup').css("display", "none");
            var Response = { Result: "Failed", Message: "This Data has already been Finalised." }
            CustomAlert(Response);
        }
        else {
            $('#freezepopup').css("display", "none");
            var Response = { Result: "Failed", Message: "There is no Data to finalise." }
            CustomAlert(Response);
        }
    }
}

function btnoptimizeyes_Click() {
    //  btnoptimizeyes_Click(string id,string RID,string ddlMaterialType)
    var id = getParameterByName('id');
    var UserRole = getParameterByName('UserRole');
    var UID = getParameterByName('UID');
    var RID = getParameterByName('RID');
    var Siteid = $('#hdnSiteLocID').val();

    var ddlMaterialType = $('#ddlMaterialType').val();
    if (ddlMaterialType == "" || ddlMaterialType == undefined) {
        ddlMaterialType = "All";
    }

    $.ajax({
        type: "POST",
        url: "/ScaffoldMaterialListPhase2/btnoptimizeyes_Click",
        data: JSON.stringify({ id: id, RID: RID, ddlMaterialType: ddlMaterialType, UID: UID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            var GetData = response;
            if (response.Result == "Success") {
                window.location.href = '/FinalMaterialList/FinalMaterialList?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&Siteid=' + Siteid + '&RID=' + RID;
            } else {
                $('#freezepopup').css("display", "none");
                CustomAlert(response);
            }
        }
    });

}

function btnoptimizeno_Click() {
    // freezepopup.Style.Add("display", "none");
    $('#freezepopup').css("display", "none");
}
