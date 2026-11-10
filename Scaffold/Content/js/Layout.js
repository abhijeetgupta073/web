var GblHideMainDepotInventory = "";
var IsSiteStatus = "";
$(document).ready(function () {
    $('#lblYear').html(new Date().getFullYear());
    $('#hdnTimeOffSetForAllPage').val(new Date().getTimezoneOffset());
    // $('#LoadingPanel1').show();
    var UserRole = getParameterByName("UserRole");
    var DecriptValues = Decrypt([UserRole]);
    UserRole = DecriptValues[0];
    $('#divLayoutScaReff').css('display', 'inline-flex');
    if (UserRole == 55) {
        $('#divLayoutScaReff').css('display', 'none');
        //$('#btnScafInspection').css('display', 'none');
    }
    if (UserRole == 95) {
        //added on 16-05-2023 for Off System Inspector View we donot show Job Ref # / FLOC / WO & Job Ref # searchbox
        $('#divLayoutScaReff').css('display', 'none');
        HideChildMenu();
    }

    //We need to keep this if else together as they are linked with each other.
    if (UserRole == 10) {
        $('#liThirdParty').css('display', 'block');
        $('#liDepotInventory').css('display', 'block');
        $('#Li_VoidedOfflineInspectionsList').css('display', 'block');
    }
    else {
        $('#liThirdParty').css('display', 'none');
        $('#liDepotInventory').css('display', 'none');
        //added on  10-07-2024 show only admin  
        $('#LiSetup_User').css('display', 'none');
        $('#LiSetup_UserAudit').css('display', 'none');
        $('#LiSetup_LoginAudit').css('display', 'none');
        $('#LiSetup_UserAccessLevel').css('display', 'none');
        $('#LiSetup_EncryptDecrypt').css('display', 'none');
        $('#Li_VoidedOfflineInspectionsList').css('display', 'none');
    }

    HideShowForAllPageButtons();

    // var Href = window.location.href.split('?');

    // var path = window.location.pathname;
    // var pageName = path.split("/").pop();
    //// console.log(page);

    //   //  var pageName = "Register";
    //     var UserRole = getParameterByName('UserRole');
    //     $.ajax({
    //         type: "POST",
    //         url: "/Layout/GetHideShowControl",
    //         data: JSON.stringify({ pageName: pageName, UserRole: UserRole }),
    //         contentType: "application/json; charset=utf-8",
    //         dataType: "json",
    //         async: false,
    //         success: function (response) {
    //             var GetData = response;
    //             $.each(response, function (index, obj) {
    //                 if (obj != null) {
    //                     var ButtonType = obj.ButtonType == null ? "" : obj.ButtonType;
    //                     var ButtonId = obj.ButtonId == null ? "" : obj.ButtonId;
    //                     //  $('#' + ButtonId).css('display', 'none');

    //                     if (ButtonType == "Div") {
    //                         $('#' + ButtonId).css('display', 'none');
    //                     }
    //                     if (ButtonType == "Button") {
    //                         $('#' + ButtonId).css('display', 'none');
    //                     }
    //                     if (ButtonType == "Label") {
    //                         $('#' + ButtonId).css('display', 'none');
    //                     }
    //                     if (ButtonType == "FileUpload") {
    //                         $('#' + ButtonId).css('display', 'none');
    //                     }
    //                     if (ButtonType == "LinkButton") {
    //                         $('#' + ButtonId).css('display', 'none');
    //                     }
    //                     if (ButtonType == "RadioButton") {
    //                         $('#' + ButtonId).css('display', 'none');
    //                     }
    //                     if (ButtonType == "Checkbox") {
    //                         $('#' + ButtonId).css('display', 'none');
    //                     }

    //                     if (ButtonType == "DropDownList") {
    //                         $('#' + ButtonId).attr('disabled', true);
    //                     }
    //                     if (ButtonType == "TextBox") {
    //                         $('#' + ButtonId).attr('disabled', true);
    //                     }
    //                     if (ButtonType == "RadNumericTextBox") {
    //                         $('#' + ButtonId).attr('disabled', true);
    //                     }
    //                     if (ButtonType == "RadComboboxBox") {
    //                         $('#' + ButtonId).attr('disabled', true);
    //                     }
    //                     if (ButtonType == "RadDatePicker") {
    //                         $('#' + ButtonId).attr('disabled', true);
    //                     }
    //                     if (ButtonType == "UniqueButtons") {
    //                         $('#' + ButtonId).attr('disabled', true);
    //                     }
    //                 }
    //             });


    //             //$('#' + ButtonId).css('display', 'none');
    //             //  $('#' + DDDocumentID)

    //         }

    //     });

    // Side Menu Hide Show Base On User Role

    var pageNameForSidemenu = "all";
    var id = getParameterByName('id');
    //  var pageName = "Register";
    var UserRole = getParameterByName('UserRole');
    /* var ClientId = getParameterByName('id');*/

    $.ajax({
        type: "POST",
        url: "/Layout/GetHideShowControl",
        //data: JSON.stringify({ pageName: pageNameForSidemenu, UserRole: UserRole }),
        data: JSON.stringify({ pageName: pageNameForSidemenu, UserRole: UserRole, id: id }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            var GetData = response;
            $.each(response, function (index, obj) {
                if (obj != null) {
                    var ButtonType = obj.ButtonType == null ? "" : obj.ButtonType;
                    var ButtonId = obj.ButtonId == null ? "" : obj.ButtonId;
                    var PageId = obj.PageId == null ? "" : obj.PageId;
                    if (ButtonType == "") {
                        $('#' + PageId).css('display', 'none');
                    }
                }
            });

        }

    });

    // Side Menu Hide On Base On Switches On Site And Client
    /*var id = getParameterByName('id');*/
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    $.ajax({
        type: "POST",
        url: "/Layout/GetHideSideMenuBar",
        data: JSON.stringify({ id: id, UID: UID, UserRole: UserRole }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            var GetData = response;

            var Li_OffSystem = GetData.Li_OffSystem == null ? "" : GetData.Li_OffSystem;
            var inventoryPlans = GetData.inventoryPlans == null ? "" : GetData.inventoryPlans;
            var CycloneManagement = GetData.CycloneManagement == null ? "" : GetData.CycloneManagement;
            var ManHoursManagement = GetData.ManHoursManagement == null ? "" : GetData.ManHoursManagement;
            var FloorLevel = GetData.FloorLevel == null ? "" : GetData.FloorLevel;
            var ScafMapGps = GetData.ScafMapGps == null ? "" : GetData.ScafMapGps;
            var MaintainInventoryAtJob = GetData.MaintainInventoryAtJob == null ? "" : GetData.MaintainInventoryAtJob;
            var HireInventory = GetData.HireInventory == null ? "" : GetData.HireInventory;
            var LongTermStructure = GetData.LongTermStructure == null ? "" : GetData.LongTermStructure;
            var ComparisonDashboard = GetData.ComparisonDashboard == null ? "" : GetData.ComparisonDashboard;
            var HideMainDepotInventory = GetData.HideMainDepotInventory == null ? "" : GetData.HideMainDepotInventory;
            GblHideMainDepotInventory = HideMainDepotInventory;
            if (Li_OffSystem == false) {
                $('#Li_OffSystem').css('display', 'none');
            }
            if (CycloneManagement == "0") {
                $('#LiCyloneDashBoard').css('display', 'none');
            }
            if (ManHoursManagement == false) {
                $('#LiManHrs').css('display', 'none');
            }
            if (HideMainDepotInventory == true) {
                $('#liDepotInventory').css('display', 'block');
            }
            if (FloorLevel == "0") {
                $('#LiFloorMap').css('display', 'none');
            }
            if (ScafMapGps == "0") {
                $('#LiMap').css('display', 'none');
            }
            if (MaintainInventoryAtJob == "0") {
                $('#LiDashBoard').css('display', 'none');
                $('#LiDashBoards').css('display', 'none');
                $('#LiItemUtilization').css('display', 'none');
                $('#LiWeeklyDashboard').css('display', 'none');
                $('#LiWeeklySummaryReport').css('display', 'none');
                $('#LiModularVsTubeFit').css('display', 'none');
                $('#LiHeadingReport').css('display', 'none');
                $('#LiSetup_InventoryManagement').css('display', 'none');

            }
            if (HireInventory == false) {
                $('#LiSubCategories').css('display', 'none');
            }
            if (LongTermStructure == false) {
                $('#LongTermStructure').css('display', 'none');
            }
            if (ComparisonDashboard == false) {
                $('#ComparisonDashboard').css('display', 'none');
            }

        }

    });
    var UserRole = getParameterByName('UserRole');
    var DecryptData = Decrypt([UserRole]);
    User = DecryptData[0];

    if (User == "75" || User == "95") {
        $('#leftbarsection').css('display', 'none');
        $('.nav-md .container.body .right_col').css('margin-left', '0px');
        $('#imgLogo').css('width', '44%');
        $('#menu_toggle').css('display', 'none');
        HideChildMenu();
    }
});

function HideShowForAllPageButtons() {
    var Href = window.location.href.split('?');

    var path = window.location.pathname;
    var pageName = path.split("/").pop();
    // console.log(page);

    //  var pageName = "Register";
    var UserRole = getParameterByName('UserRole');
    var id = getParameterByName('id');

    $.ajax({
        type: "POST",
        url: "/Layout/GetHideShowControl",
        data: JSON.stringify({ pageName: pageName, UserRole: UserRole, id: id }),
        //data: JSON.stringify({ pageName: pageName, UserRole: UserRole, ClientId: ClientId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            var GetData = response;
            $.each(response, function (index, obj) {
                if (obj != null) {
                    var ButtonType = obj.ButtonType == null ? "" : obj.ButtonType;
                    var ButtonId = obj.ButtonId == null ? "" : obj.ButtonId;
                    //  $('#' + ButtonId).css('display', 'none');

                    if (ButtonType == "Div") {
                        $('#' + ButtonId).css('display', 'none');
                    }
                    if (ButtonType == "Button") {
                        $('#' + ButtonId).css('display', 'none');
                    }
                    if (ButtonType == "Label") {
                        $('#' + ButtonId).css('display', 'none');
                    }
                    if (ButtonType == "FileUpload") {
                        $('#' + ButtonId).css('display', 'none');
                    }
                    if (ButtonType == "LinkButton") {
                        $('#' + ButtonId).css('display', 'none');
                    }
                    if (ButtonType == "RadioButton") {
                        $('#' + ButtonId).css('display', 'none');
                    }
                    if (ButtonType == "Checkbox") {
                        $('#' + ButtonId).css('display', 'none');
                    }

                    if (ButtonType == "DropDownList") {
                        $('#' + ButtonId).attr('disabled', true);
                    }
                    if (ButtonType == "TextBox") {
                        $('#' + ButtonId).attr('disabled', true);
                    }
                    if (ButtonType == "RadNumericTextBox") {
                        $('#' + ButtonId).attr('disabled', true);
                    }
                    if (ButtonType == "RadComboboxBox") {
                        $('#' + ButtonId).attr('disabled', true);
                    }
                    if (ButtonType == "RadDatePicker") {
                        $('#' + ButtonId).attr('disabled', true);
                        if (ButtonId == 'rdpdatedis') {
                            $('#' + ButtonId).prop("readonly", true);
                        }
                        else {
                            $('#' + ButtonId).prop("readonly", false);
                        }
                    }
                    if (ButtonType == "UniqueButtons") {
                        $('#' + ButtonId).attr('disabled', true);
                    }
                    if (ButtonType == "ClassToDisableGroup") {
                        $('.' + ButtonId).attr('disabled', true);
                    }
                }
            });


            //$('#' + ButtonId).css('display', 'none');
            //  $('#' + DDDocumentID)

        }

    });
}


// Side Menu Code Start Here
function GridButtonHideShow(DataTableObj, TableName) {
    var Href = window.location.href.split('?');

    var path = window.location.pathname;
    var pageName = path.split("/").pop();
    var UserRole = getParameterByName('UserRole');
    var id = getParameterByName('id');
    $.ajax({
        type: "POST",
        url: "/Layout/GetHideShowControl",
        //data: JSON.stringify({ pageName: pageName, UserRole: UserRole }),
        data: JSON.stringify({ pageName: pageName, UserRole: UserRole, id: id }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            var GetData = response;
            var arrayIndex = [];
            $.each(response, function (index, obj) {
                if (obj != null) {
                    var ButtonType = obj.ButtonType == null ? "" : obj.ButtonType;
                    var ButtonId = obj.ButtonId == null ? "" : obj.ButtonId;
                    var GridId = obj.Gridid == null ? "" : obj.Gridid;
                    // var indexNumber = $('#tblRequestInventory th#' + ButtonId).index();
                    var indexNumber = $('#' + TableName + ' th#' + ButtonId).index();

                    if (ButtonType == "thClassButton") {
                        $('.' + ButtonId).css('display', 'none');
                    }
                    else if (ButtonType == "LengthButtons") {
                        $('#' + ButtonId).attr("disabled", true);
                        $('#' + ButtonId).css('display', 'none');
                    }
                    else {
                        if (indexNumber != "-1") {
                            // DataTableObj.column(indexNumber).visible(false);
                            arrayIndex.push(indexNumber);
                        }
                    }

                }
            });
            for (var i = 0; i < arrayIndex.length; i++) {
                DataTableObj.column(arrayIndex[i]).visible(false);
            }

        }

    });

}
function ClientRegister() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    // var Clientid = $('#<%=ddlClient.ClientID%>').val();
    var Clientid = $('#ddlClient').val();
    // document.getElementById("clickbtn").value = "RegisterList";
    // document.getElementById("btnredirect").click();
    // if (location.pathname == "/Document") {
    Clientid = getParameterByName('id');
    // window.location.href = '/RegisterList.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    window.location.href = '/RegisterList/RegisterList?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    // }
}

function ManageUsers() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');

    window.location.href = '/ClientUsers/ClientUsers?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;

    //var Clientid = $('#<%=ddlClient.ClientID%>').val();
    //document.getElementById("clickbtn").value = "ClientUsers";
    //document.getElementById("btnredirect").click();
    //if (location.pathname == "/Document") {
    //    Clientid = getParameterByName('id');
    //    window.location.href = '/ClientUsers.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //}
}
//function AreaMaster() {
//    var UID = getParameterByName('UID');
//    var UserRole = getParameterByName('UserRole');
//    var Clientid = $('#<%=ddlClient.ClientID%>').val();
//    document.getElementById("clickbtn").value = "Area";
//    document.getElementById("btnredirect").click();
//    if (location.pathname == "/Document") {
//        Clientid = getParameterByName('id');
//        window.location.href = '/Area.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
//    }
//}

function ActualWeekPlan() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    window.location.href = '/ActualPlanningWeek/ActualPlanningWeek?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;

    //var Clientid = $('#<%=ddlClient.ClientID%>').val();
    //document.getElementById("clickbtn").value = "DesignType";
    //document.getElementById("btnredirect").click();
    //if (location.pathname == "/Document") {
    //    Clientid = getParameterByName('id');
    //    window.location.href = '/DesignType.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //}
}

function DesignType() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    window.location.href = '/DesignType/DesignType?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;

    //var Clientid = $('#<%=ddlClient.ClientID%>').val();
    //document.getElementById("clickbtn").value = "DesignType";
    //document.getElementById("btnredirect").click();
    //if (location.pathname == "/Document") {
    //    Clientid = getParameterByName('id');
    //    window.location.href = '/DesignType.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //}
}
function DutyRating() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    window.location.href = '/DutyRating/DutyRating?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;

    //var Clientid = $('#<%=ddlClient.ClientID%>').val();
    //document.getElementById("clickbtn").value = "DutyRating";
    //document.getElementById("btnredirect").click();
    //if (location.pathname == "/Document") {
    //    Clientid = getParameterByName('id');
    //    window.location.href = '/DutyRating.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //}
}
//function InspectedBy() {
//    var UID = getParameterByName('UID');
//    var UserRole = getParameterByName('UserRole');
//    var Clientid = $('#<%=ddlClient.ClientID%>').val();
//    document.getElementById("clickbtn").value = "InspectedBy";
//    document.getElementById("btnredirect").click();
//    if (location.pathname == "/Document") {
//        Clientid = getParameterByName('id');
//        window.location.href = '/InspectedBy.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
//    }
//}
//function Inventory() {
//    var UID = getParameterByName('UID');
//    var UserRole = getParameterByName('UserRole');
//    var Clientid = $('#<%=ddlClient.ClientID%>').val();
//    document.getElementById("clickbtn").value = "Inventory";
//    document.getElementById("btnredirect").click();
//    if (location.pathname == "/Document") {
//        Clientid = getParameterByName('id');
//        window.location.href = '/Inventory.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
//    }
//}
function InventoryManagement() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    var DecryptData = Decrypt(UserRole);
    // window.location.href = '/InventoryManagement.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    // window.location.href = '/InventoryManagement.cshtml?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;

    if (DecryptData == 10) {
        window.location.href = '/InventoryManagement/InventoryManagement?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    }
    else if (GblHideMainDepotInventory == true) {
        window.location.href = '/MainDepotInventory/MainDepotInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    }
    else {
        //window.location.href = '/MainDepotInventory/MainDepotInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
        window.location.href = '/SiteInventory/SiteInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    }
}

function ManHrs() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    window.location.href = '/ManHrs/ManHrs?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //    var Clientid = $('#<%=ddlClient.ClientID%>').val();
    //    document.getElementById("clickbtn").value = "ManHrs";
    //    document.getElementById("btnredirect").click();
    //    if (location.pathname == "/Document") {
    //        Clientid = getParameterByName('id');
    //        window.location.href = '/ManHrs.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //    }
}
function InspectionCheckList() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    window.location.href = '/InspectionCheckListContent/InspectionCheckListContent?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;

    //var Clientid = $('#<%=ddlClient.ClientID%>').val();
    //document.getElementById("clickbtn").value = "InspectionCheckListContent";
    //document.getElementById("btnredirect").click();
    //if (location.pathname == "/Document") {
    //    Clientid = getParameterByName('id');
    //    window.location.href = '/InspectionCheckListContent.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //}
}
//function Inspection() {
//    var UID = getParameterByName('UID');
//    var UserRole = getParameterByName('UserRole');
//    var Clientid = $('#<%=ddlClient.ClientID%>').val();
//    document.getElementById("clickbtn").value = "InspectionCheckList";
//    document.getElementById("btnredirect").click();
//    if (location.pathname == "/Document") {
//        Clientid = getParameterByName('id');
//        window.location.href = '/InspectionCheckList.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
//    }
//}
function MaterialType() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    window.location.href = '/MaterialType/MaterialType?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;

    //var Clientid = $('#<%=ddlClient.ClientID%>').val();
    //document.getElementById("clickbtn").value = "MaterialType";
    //document.getElementById("btnredirect").click();
    //if (location.pathname == "/Document") {
    //    Clientid = getParameterByName('id');
    //    window.location.href = '/MaterialType.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //}
}
function DayDocket() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    window.location.href = '/DayDocket/DayDocket?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;

    //var Clientid = $('#<%=ddlClient.ClientID%>').val();
    //document.getElementById("clickbtn").value = "DayDocket";
    //document.getElementById("btnredirect").click();
    //if (location.pathname == "/Document") {
    //    Clientid = getParameterByName('id');
    //    window.location.href = '/DayDocket.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //}
}
//// New Changes Add on 24 July 2019
function LoginAudit() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    window.location.href = '/LoginAudit/LoginAudit?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;

    //var Clientid = $('#<%=ddlClient.ClientID%>').val();
    //document.getElementById("clickbtn").value = "LoginAudit";
    //document.getElementById("btnredirect").click();
    //if (location.pathname == "/Document") {
    //    Clientid = getParameterByName('id');
    //    window.location.href = '/LoginAudit.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //}
}
function UserAudit() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    window.location.href = '/UserAudit/UserAudit?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;

    //var Clientid = $('#<%=ddlClient.ClientID%>').val();
    //document.getElementById("clickbtn").value = "UserAudit";
    //document.getElementById("btnredirect").click();
    //if (location.pathname == "/Document") {
    //    Clientid = getParameterByName('id');
    //    window.location.href = '/UserAudit.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //}
}

////Change On 07Nov2019
function FloorLevel() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    window.location.href = '/FloorLevel/FloorLevel?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;

    //var Clientid = $('#<%=ddlClient.ClientID%>').val();
    //document.getElementById("clickbtn").value = "FloorLevel";
    //document.getElementById("btnredirect").click();
    //if (location.pathname == "/Document") {
    //    Clientid = getParameterByName('id');
    //    window.location.href = '/FloorLevel.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //}
}
function WorkGroups() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    window.location.href = '/WorkGroups/WorkGroups?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
}
function RequestedBy() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    window.location.href = '/RequestedBy/RequestedBy?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
}
//Added on 06-12-2023 for new master onlick function for navigation
function ShiftAllocation() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    window.location.href = '/ShiftAllocation/ShiftAllocation?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
}

function Documentation() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    window.location.href = '/Documentation/Documentation?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;

}
function ScafpacDocumentation() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    window.location.href = '/ScafpacDocumentation/ScafpacDocumentation?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;

}
function SiteLocation() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    window.location.href = '/SiteLocation/SiteLocation?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;

    //var Clientid = $('#<%=ddlClient.ClientID%>').val();
    //document.getElementById("clickbtn").value = "SiteLocation";
    //document.getElementById("btnredirect").click();
    //if (location.pathname == "/Document") {
    //    Clientid = getParameterByName('id');
    //    window.location.href = '/SiteLocation.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //}
}
function State() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    window.location.href = '/State/State?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;

    //var Clientid = $('#<%=ddlClient.ClientID%>').val();
    //document.getElementById("clickbtn").value = "State";
    //document.getElementById("btnredirect").click();
    //if (location.pathname == "/Document") {
    //    Clientid = getParameterByName('id');
    //    window.location.href = '/State.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //}
}
function Representative_Position() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    window.location.href = '/Representative_Position/Representative_Position?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;

    //var Clientid = $('#<%=ddlClient.ClientID%>').val();
    //document.getElementById("clickbtn").value = "Representative_Position";
    //document.getElementById("btnredirect").click();
    //if (location.pathname == "/Document") {
    //    Clientid = getParameterByName('id');
    //    window.location.href = '/Representative_Position.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //}
}
function Email_Content() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    window.location.href = '/Email_Content/Email_Content?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;

    //var Clientid = $('#<%=ddlClient.ClientID%>').val();
    //document.getElementById("clickbtn").value = "Email_Content";
    //document.getElementById("btnredirect").click();
    //if (location.pathname == "/Document") {
    //    Clientid = getParameterByName('id');
    //    window.location.href = '/Email_Content.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //}

}
function ClientProfile() {

    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    window.location.href = '/EditClient/EditClient?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;

    //var Clientid = $('#<%=ddlClient.ClientID%>').val();
    //document.getElementById("clickbtn").value = "EditClient";
    //document.getElementById("btnredirect").click();

    //if (location.pathname == "/Document") {
    //    Clientid = getParameterByName('id');
    //    window.location.href = '/EditClient.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //}

}

function OffSystemInsp() {

    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    window.location.href = '/OffSystemInspection/OffSystemInspection?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;

    //var Clientid = $('#<%=ddlClient.ClientID%>').val();
    //document.getElementById("clickbtn").value = "OffSystemInspection";
    //document.getElementById("btnredirect").click();

    //if (location.pathname == "/Document") {
    //    Clientid = getParameterByName('id');
    //    window.location.href = '/OffSystemInspection.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //}

}


function VoidedOfflineInspectionsList() {

    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    window.location.href = '/VoidedOfflineInspectionsList/VoidedOfflineInspectionsList?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;

}
function ManHrsDashboard() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    window.location.href = '/ManHrsDashboard/ManHrsDashboard?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;

    //var Clientid = $('#<%=ddlClient.ClientID%>').val();
    //document.getElementById("clickbtn").value = "ManHrsDashboard";
    //document.getElementById("btnredirect").click();
    //if (location.pathname == "/Document") {
    //    Clientid = getParameterByName('id');
    //    window.location.href = '/ManHrsDashboard.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //}


}

function Weeklyprogress() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');

    window.location.href = '/WeeklyprogressDashboard/WeeklyprogressDashboard?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;

    //var Clientid = $('#<%=ddlClient.ClientID%>').val();
    //document.getElementById("clickbtn").value = "WeeklyprogressDashboard";
    //document.getElementById("btnredirect").click();
    //if (location.pathname == "/Document") {
    //    Clientid = getParameterByName('id');
    //    window.location.href = '/WeeklyprogressDashboard.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //}
}

function ModularVsTubeFit() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    window.location.href = '/ModularVsTubeFitDashboard/ModularVsTubeFitDashboard?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;

    //var Clientid = $('#<%=ddlClient.ClientID%>').val();
    //document.getElementById("clickbtn").value = "ModularVsTubeFitDashboard";
    //document.getElementById("btnredirect").click();
    //if (location.pathname == "/Document") {
    //    Clientid = getParameterByName('id');
    //    window.location.href = '/ModularVsTubeFitDashboard.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //}
}
function InspectionDashboard() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    //var Clientid = $('#<%=ddlClient.ClientID%>').val();
    //document.getElementById("clickbtn").value = "InspectionDashboard";
    //document.getElementById("btnredirect").click();
    //if (location.pathname == "/Document") {
    //    Clientid = getParameterByName('id');
    //  window.location.href = '/InspectionDashboard.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    window.location.href = '/InspectionDashboard/InspectionDashboard?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    // }
}

function LongTermStructureDashboard() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    window.location.href = '/LongTermStructureDashboard/LongTermStructureDashboard?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
}
function ComparisonDashboard() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    window.location.href = '/ComparisonDashboard/ComparisonDashboard?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
}


function PIMaintenanceDashboard() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    window.location.href = '/PIVsMaintenanceDashboard/PIVsMaintenanceDashboard?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;

    //var Clientid = $('#<%=ddlClient.ClientID%>').val();
    //document.getElementById("clickbtn").value = "PIVsMaintenanceDashboard";
    //document.getElementById("btnredirect").click();
    //if (location.pathname == "/Document") {
    //    Clientid = getParameterByName('id');
    //    window.location.href = '/PIVsMaintenanceDashboard.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //}
}

function WeeklySummaryReport() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    window.location.href = '/WeeklySummaryReport/WeeklySummaryReport?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;

    //var Clientid = $('#<%=ddlClient.ClientID%>').val();
    //document.getElementById("clickbtn").value = "WeeklySummaryReport";
    //document.getElementById("btnredirect").click();
    //if (location.pathname == "/Document") {
    //    Clientid = getParameterByName('id');
    //    window.location.href = '/WeeklySummaryReport.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //}


}

function SiteAnalysis() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    //var Clientid = $('#<%=ddlClient.ClientID%>').val();
    //document.getElementById("clickbtn").value = "SiteAnalysisDashboard";
    //document.getElementById("btnredirect").click();
    //if (location.pathname == "/Document") {
    //    Clientid = getParameterByName('id');
    //   window.location.href = '/SiteAnalysisDashboard.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    window.location.href = '/SiteAnalysisDashboard/SiteAnalysisDashboard?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    // }
}

function MultipleReport() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');

    window.location.href = '/MultipleReport/MultipleReport?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //var Clientid = $('#<%=ddlClient.ClientID%>').val();
    //document.getElementById("clickbtn").value = "MultipleReport";
    //document.getElementById("btnredirect").click();
    //if (location.pathname == "/Document") {
    //    Clientid = getParameterByName('id');
    //    window.location.href = '/MultipleReport.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //}
}

function SiteManHrs() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');

    window.location.href = '/SiteManHrs/SiteManHrs?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;

    //var Clientid = $('#<%=ddlClient.ClientID%>').val();
    //document.getElementById("clickbtn").value = "SiteManHrs";
    //document.getElementById("btnredirect").click();
    //if (location.pathname == "/Document") {
    //    Clientid = getParameterByName('id');
    //    window.location.href = '/SiteManHrs.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //}

}
function ReconcileHrs() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');

    window.location.href = '/ManHrsReconcile/ManHrsReconcile?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;

    //var Clientid = $('#<%= ddlClient.ClientID%>').val();
    //document.getElementById("clickbtn").value = "ManHrsReconcile";
    //document.getElementById("btnredirect").click();
    //if (location.pathname == "/Document") {
    //    Clientid = getParameterByName('id');
    //    window.location.href = '/ManHrsReconcile.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //}
}

function Dashboard() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    window.location.href = '/Default/Default?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //var Clientid = $('#<%=ddlClient.ClientID%>').val();
    //document.getElementById("clickbtn").value = "Default";
    //document.getElementById("btnredirect").click();
    //if (location.pathname == "/Document") {
    //    Clientid = getParameterByName('id');
    //    window.location.href = '/Default.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //}
}
function Dashboards() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    // var Clientid = $('#<%=ddlClient.ClientID%>').val();
    // var Clientid = $('#ddlClient').val();
    // document.getElementById("clickbtn").value = "DashboardHere";
    // document.getElementById("btnredirect").click();
    // if (location.pathname == "/Document") {

    // window.location.href = '/DashboardHere.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    window.location.href = '/DashboardHere/DashboardHere?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //  }
}


function CyloneDashBoard() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    // var Clientid = $('#ddlClient').val();
    // document.getElementById("clickbtn").value = "CycloneDashboard";
    // document.getElementById("btnredirect").click();
    //  if (location.pathname == "/Document") {

    // window.location.href = '/CycloneDashboard.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    window.location.href = '/CycloneDashboard/CycloneDashboard?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //  }
}

function ItemUtilization() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    //var Clientid = $('#<%=ddlClient.ClientID%>').val();
    //document.getElementById("clickbtn").value = "ItemUtilizeDashboard";
    // document.getElementById("btnredirect").click();
    // if (location.pathname == "/Document") {

    //     window.location.href = '/ItemUtilizeDashboard.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    window.location.href = '/ItemUtilizeDashboard/ItemUtilizeDashboard?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    // }
}

function ManHrs() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    window.location.href = '/ManHrs/ManHrs?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;

    //var Clientid = $('#<%=ddlClient.ClientID%>').val();
    //document.getElementById("clickbtn").value = "ManHrs";
    //document.getElementById("btnredirect").click();
    //if (location.pathname == "/Document") {
    //    Clientid = getParameterByName('id');
    //    window.location.href = '/ManHrs.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //}


}
function UserCategory() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    window.location.href = '/UserCategory/UserCategory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;

    //var Clientid = $('#<%=ddlClient.ClientID%>').val();
    //document.getElementById("clickbtn").value = "UserCategory";
    //document.getElementById("btnredirect").click();
    //if (location.pathname == "/Document") {
    //    Clientid = getParameterByName('id');
    //    window.location.href = '/UserCategory.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //}
}

function TermsNCondition() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    window.location.href = '/TermsAndConditionType/TermsAndConditionType?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;

    //var Clientid = $('#<%=ddlClient.ClientID%>').val();
    //document.getElementById("clickbtn").value = "TermsAndConditionType";
    //document.getElementById("btnredirect").click();
    //if (location.pathname == "/Document") {
    //    Clientid = getParameterByName('id');
    //    window.location.href = '/TermsAndConditionType.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //}
}

function MapSiteLocation() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    //var Clientid = $('#<%=ddlClient.ClientID%>').val();
    //document.getElementById("clickbtn").value = "SiteLocationMap";
    //document.getElementById("btnredirect").click();
    //if (location.pathname == "/Document") {
    Clientid = getParameterByName('id');
    //  window.location.href = '/SiteLocationMap.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    window.location.href = '/SiteLocationMap/SiteLocationMap?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //}
}
function ScafMapJobstatus() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    //var Clientid = $('#<%=ddlClient.ClientID%>').val();
    //document.getElementById("clickbtn").value = "SiteLocationMap";
    //document.getElementById("btnredirect").click();
    //if (location.pathname == "/Document") {
    Clientid = getParameterByName('id');
    //  window.location.href = '/SiteLocationMap.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    window.location.href = '/ScafMapJobStatus/ScafMapJobStatus?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //}
}
function MapSiteFloorLevel() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');

    window.location.href = '/SiteFloorLevelPlan/SiteFloorLevelPlan?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;

    //var Clientid = $('#<%=ddlClient.ClientID%>').val();
    //document.getElementById("clickbtn").value = "SiteFloorLevelPlan";
    //document.getElementById("btnredirect").click();
    //if (location.pathname == "/Document") {
    //    Clientid = getParameterByName('id');
    //    window.location.href = '/SiteFloorLevelPlan.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //}
}

function TermsAndCondition() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    window.location.href = '/TermsNCondition/TermsNCondition?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;

    //var Clientid = $('#<%=ddlClient.ClientID%>').val();
    //document.getElementById("clickbtn").value = "TermsNCondition";
    //document.getElementById("btnredirect").click();
    //if (location.pathname == "/Document") {
    //    Clientid = getParameterByName('id');
    //    window.location.href = '/TermsNCondition.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //}
}


function EncryptDecrypt() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    window.location.href = '/Encrypt/Encrypt?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;

    //var Clientid = $('#<%=ddlClient.ClientID%>').val();
    //document.getElementById("clickbtn").value = "Encrypt";
    //document.getElementById("btnredirect").click();
    //if (location.pathname == "/Document") {
    //    Clientid = getParameterByName('id');
    //    window.location.href = '/Encrypt.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //}
}

function UserAccessLevel() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    window.location.href = '/UserAccessLevel/UserAccessLevel?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;

    //var Clientid = $('#<%=ddlClient.ClientID%>').val();
    //document.getElementById("clickbtn").value = "UserAccessLevel";
    //document.getElementById("btnredirect").click();
    //if (location.pathname == "/Document") {
    //    Clientid = getParameterByName('id');
    //    window.location.href = '/UserAccessLevel.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //}
}
//// adding new function on 11 march 2020 function UserAccessLevel() {

function SubCategories() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    window.location.href = '/SubCategories/SubCategories?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;

    //    var Clientid = $('#<%=ddlClient.ClientID%>').val();
    //    document.getElementById("clickbtn").value = "SubCategories";
    //    document.getElementById("btnredirect").click();
    //    if (location.pathname == "/Document") {
    //        Clientid = getParameterByName('id');
    //        window.location.href = '/SubCategories.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //    }
}
function RegisterListSorting() {
    debugger;
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    window.location.href = '/RegisterListSorting/RegisterListSorting?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
}

function InvoicedSurgeReport() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    window.location.href = '/InvoicedSurgeReport/InvoicedSurgeReport?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;

    //    var Clientid = $('#<%=ddlClient.ClientID%>').val();
    //    document.getElementById("clickbtn").value = "SubCategories";
    //    document.getElementById("btnredirect").click();
    //    if (location.pathname == "/Document") {
    //        Clientid = getParameterByName('id');
    //        window.location.href = '/SubCategories.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
    //    }
}

function VariationHoursReport() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    window.location.href = '/VariationHoursReport/VariationHoursReport?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
}
function StandingVsAvailablebyMaterialType() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    window.location.href = '/StandingVsAvailablebyMaterialType/StandingVsAvailablebyMaterialType?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
}
function OpenChangePassWordPopup() {
    $('#ChangePasWorkModel').css('display', 'block');
    $('#txtCurrentPassWord').val('');
    $('#txtNewPassword').val('');
    $('#txtConfirmNewPassword').val('');
    //$('#<%=txtCurrentPassWord.ClientID%>').val('');
    //$('#<%=txtNewPassword.ClientID%>').val('');
    //$('#<%=txtConfirmNewPassword.ClientID%>').val('');
}
function CloseChangePassWordPopup() {
    $('#ChangePasWorkModel').css('display', 'none');
}

function btnSaveChangePassword_Click() {
    var txtConfirmNewPassword = $('#txtConfirmNewPassword').val();
    var txtNewPassword = $('#txtNewPassword').val();
    var txtCurrentPassWord = $('#txtCurrentPassWord').val();
    $.ajax({
        type: "POST",
        url: "/Layout/btnSaveChangePassword_Click",
        data: JSON.stringify({ txtConfirmNewPassword: txtConfirmNewPassword, txtNewPassword: txtNewPassword, txtCurrentPassWord: txtCurrentPassWord }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            var GetData = response;
            if (response.Result == "SessionOut") {
                window.location.href = '/Login/Login';
            } else if (response.Result == "Success") {
                $('#ChangePasWorkModel').css('display', 'none');
                CustomAlert(response);
            } else {
                CustomAlert(response);
            }

        }
    });
}
$(document).ready(function () {

    //$(function () {
    //    setInterval(function () {
    //        $('.rcbDisabled').parent().css('background-color', '#e5e5e5');
    //    }, 1000);
    //});
    //function AddButtonClickOfGrid(Grid) {//For Click On Add Button Of Radgrid
    //    $("" + Grid + " .RadGrid_Metro .rgAdd").click();
    //}


    //Auto Call When Export Data of Radgrid Start
    function onRequestStart(sender, args) {
        if (args.get_eventTarget().indexOf("ExportToCsvButton") >= 0)
            args.set_enableAjax(false);
    }
    //Auto Call When Export Data of Radgrid End

    //add active class by match url Start
    //  var pathname = window.location.pathname;
    // var path = (pathname.substr(0, pathname.lastIndexOf('.')) || pathname).replace('/', '');
    var Href = window.location.href.split('?');
    var pathname = window.location.pathname;
    var path = pathname.split("/").pop();

    path = path.toLowerCase();
    if (path == 'default') {
        $('#LiDashnoard').addClass('active');
        $('#LiDashnoard ul').css('display', 'block');
        $('#LiDashBoard a').addClass('LiChildActive');
    }
    else if (path == 'sitelocationmap') {
        $('#LiDashnoard').addClass('active');
        $('#LiDashnoard ul').css('display', 'block');
        $('#LiMap a').addClass('LiChildActive');
    }
    else if (path == 'ScafMapJobStatus') {
        $('#LiDashnoard').addClass('active');
        $('#LiDashnoard ul').css('display', 'block');
        $('#LiScafMapJobstatus a').addClass('LiChildActive');
    }
    //add active class 09/Nov/2019
    else if (path == 'sitefloorlevelplan') {
        $('#LiDashnoard').addClass('active');
        $('#LiDashnoard ul').css('display', 'block');
        $('#LiFloorMap a').addClass('LiChildActive');
    }

    else if (path == 'floorlevel') {
        $('#LiDashnoard').addClass('active');
        $('#LiDashnoard ul').css('display', 'block');
        $('#LiFloorLevel a').addClass('LiChildActive');
    }
    else if (path == 'cyclonedashboard') {
        $('#LiDashnoard').addClass('active');
        $('#LiDashnoard ul').css('display', 'block');
        $('#LiCyloneDashBoard a').addClass('LiChildActive');
    }
    else if (path == 'itemutilizedashboard') {
        $('#LiDashnoard').addClass('active');
        $('#LiDashnoard ul').css('display', 'block');
        $('#LiItemUtilization a').addClass('LiChildActive');
    }
    else if (path == 'siteanalysisdashboard') {
        $('#LiDashnoard').addClass('active');
        $('#LiDashnoard ul').css('display', 'block');
        $('#LiSiteAnalysis a').addClass('LiChildActive');
    }
    else if (path == 'weeklyprogressdashboard') {
        $('#LiDashnoard').addClass('active');
        $('#LiDashnoard ul').css('display', 'block');
        $('#LiWeeklyDashboard a').addClass('LiChildActive');
    }
    else if (path == 'weeklysummaryreport') {
        $('#LiDashnoard').addClass('active');
        $('#LiDashnoard ul').css('display', 'block');
        $('#LiWeeklySummaryReport a').addClass('LiChildActive');
    }
    else if (path == 'modularvstubefitdashboard') {
        $('#LiDashnoard').addClass('active');
        $('#LiDashnoard ul').css('display', 'block');
        $('#LiModularVsTubeFit a').addClass('LiChildActive');
    }
    else if (path == 'inspectiondashboard') {
        $('#LiDashnoard').addClass('active');
        $('#LiDashnoard ul').css('display', 'block');
        $('#LiInspectionDashboard a').addClass('LiChildActive');
    }
    else if (path == 'pivsmaintenancedashboard') {
        $('#LiDashnoard').addClass('active');
        $('#LiDashnoard ul').css('display', 'block');
        $('#Li a').addClass('LiChildActive');
    }


    //man hour
    else if (path == 'manhrsdashboard') {
        $('#LiManHrs').addClass('active');
        $('#LiManHrs ul').css('display', 'block');
        $('#LiManHrs_ManHrsDashboard a').addClass('LiChildActive');
    }
    else if (path == 'sitemanhrs') {
        $('#LiManHrs').addClass('active');
        $('#LiManHrs ul').css('display', 'block');
        $('#LiManHrs_SiteManHrs a').addClass('LiChildActive');
    }
    else if (path == 'manhrs') {
        $('#LiManHrs').addClass('active');
        $('#LiManHrs ul').css('display', 'block');
        $('#LiManHrs_ManHrs a').addClass('LiChildActive');
    }
    else if (path == 'manhrsreconcile') {
        $('#LiManHrs').addClass('active');
        $('#LiManHrs ul').css('display', 'block');
        $('#LiManHrs_ReconcileHrs a').addClass('LiChildActive');
    }
    else if (path == 'daydocket') {
        $('#LiManHrs').addClass('active');
        $('#LiManHrs ul').css('display', 'block');
        $('#LiManHrs_DayDocket a').addClass('LiChildActive');
    }
    //man end
    //Setup
    else if (path == 'clientusers') {
        $('#LiSetup').addClass('active');
        $('#LiSetup ul').css('display', 'block');
        $('#LiSetup_User a').addClass('LiChildActive');
    }
    else if (path == 'actualplanningweek') {
        $('#LiSetup').addClass('active');
        $('#LiSetup ul').css('display', 'block');
        $('#LiSetup_ActualPlanningWeek a').addClass('LiChildActive');
    }
    else if (path == 'designtype') {
        $('#LiSetup').addClass('active');
        $('#LiSetup ul').css('display', 'block');
        $('#LiSetup_DesignType a').addClass('LiChildActive');
    }
    else if (path == 'dutyrating') {
        $('#LiSetup').addClass('active');
        $('#LiSetup ul').css('display', 'block');
        $('#LiSetup_DutyRating a').addClass('LiChildActive');
    }
    else if (path == 'materialtype') {
        $('#LiSetup').addClass('active');
        $('#LiSetup ul').css('display', 'block');
        $('#LiSetup_MaterialType a').addClass('LiChildActive');
    }
    else if (path == 'state') {
        $('#LiSetup').addClass('active');
        $('#LiSetup ul').css('display', 'block');
        $('#LiSetup_State a').addClass('LiChildActive');
    }
    else if (path == 'representative_position') {
        $('#LiSetup').addClass('active');
        $('#LiSetup ul').css('display', 'block');
        $('#LiSetup_Representative_Position a').addClass('LiChildActive');
    }
    else if (path == 'email_content') {
        $('#LiSetup').addClass('active');
        $('#LiSetup ul').css('display', 'block');
        $('#LiSetup_Email_Content a').addClass('LiChildActive');
    }
    else if (path == 'inspectionchecklistcontent') {
        $('#LiSetup').addClass('active');
        $('#LiSetup ul').css('display', 'block');
        $('#LiSetup_checklist a').addClass('LiChildActive');
    }
    else if (path == 'usercategory') {
        $('#LiSetup').addClass('active');
        $('#LiSetup ul').css('display', 'block');
        $('#LiSetup_UserCategory a').addClass('LiChildActive');
    }
    else if (path == 'termsandconditiontype') {
        $('#LiSetup').addClass('active');
        $('#LiSetup ul').css('display', 'block');
        $('#LiSetup_TermsNCondition a').addClass('LiChildActive');
    }
    else if (path == 'encrypt') {
        $('#LiSetup').addClass('active');
        $('#LiSetup ul').css('display', 'block');
        $('#LiSetup_EncryptDecrypt a').addClass('LiChildActive');
    }
    else if (path == 'useraccesslevel') {
        $('#LiSetup').addClass('active');
        $('#LiSetup ul').css('display', 'block');
        $('#LiSetup_UserAccessLevel a').addClass('LiChildActive');
    }
    else if (path == 'loginaudit') {
        $('#LiSetup').addClass('active');
        $('#LiSetup ul').css('display', 'block');
        $('#LiSetup_LoginAudit a').addClass('LiChildActive');
    }
    else if (path == 'useraudit') {
        $('#LiSetup').addClass('active');
        $('#LiSetup ul').css('display', 'block');
        $('#LiSetup_UserAudit a').addClass('LiChildActive');
    }
    else if (path == 'FloorLevel') {
        $('#LiSetup').addClass('active');
        $('#LiSetup ul').css('display', 'block');
        $('#LiSetup_FloorLevel a').addClass('LiChildActive');
    }

    // adding new active class on 11 March 2020
    else if (path == 'SubCategories') {
        $('#LiSetup').addClass('active');
        $('#LiSetup ul').css('display', 'block');
        $('#LiSetup_SubCategories a').addClass('LiChildActive');
    }

    //end setup

    else if (path == 'multiplereport') {
        $('#LiHeadingReport').addClass('active');
    }
    else if (path == 'editclient') {
        $('#Li_Client').addClass('active');
    }
    else if (path == 'offsystemInspection') {
        $('#Li_OffSystem').addClass('active');
    }

    else if (path == 'VoidedOfflineInspectionsList') {
        $('#Li_VoidedOfflineInspectionsList').addClass('active');
    }
    else if (path == 'registerlist') {
        $('#LiSales_Register').addClass('active');
    }
    else if (path == 'inventory') {
        $('#LiSetup_InventoryPlan1').addClass('active');
    }
    else if (path == 'inventorymanagement') {
        $('#LiSetup_InventoryManagement').addClass('active');
    }

    else if (path == 'project/project' || path == "project/overview" || path == "project/dashboard" || path == "project/shedule" || path == "project/detail" || path == "project/attachment" || path == "project/expenses" || path == "project/billing" || path == "project/notesandcomment") {
        $('#LiProject').addClass('active');
    }


    else if (path == "manage/employee" || path == "manage/employee_documents" || path == "manage/employeelist" || path == "manage/ptosummary" || path == "manage/securityacess") {
        $('#LiManage,#LIHr').addClass('active');
        $('#UlManage,#UlHr').css('display', 'block');
        $('#LIHr_Employee a').addClass('LiChildActive');
        // $('#Lihra').addClass('LiChildActive');


    }
    else if (path == "oprationalsetup/userrole") {
        $('#LiOprationalSetup').addClass('active');
        $('#LiOprationalSetup ul').css('display', 'block');
        $('#lijobroletype a').addClass('LiChildActive');

    }
    else if (path == "oprationalsetup/jobandroles") {
        $('#LiOprationalSetup').addClass('active');
        $('#LiOprationalSetup ul').css('display', 'block');
        $('#lijobandRole a').addClass('LiChildActive');

    }
    else if (path == 'oprationalsetup/companyholiday') {
        $('#LiOprationalSetup').addClass('active');
        $('#LiOprationalSetup ul').css('display', 'block');
        $('#LiCompanyHoliday a').addClass('LiChildActive');

    }
    else if (path == 'timesheet') {
        $('#LiTimeSheet').addClass('active');
    }
    else if (path == 'inspection') {
        $('#LiInspection').addClass('active');
    }
    else if (path == "oprationalsetup/companysitelist" || path == "oprationalsetup/editmycompany" || path == "oprationalsetup/mycompanydocuments" || path == "oprationalsetup/mycompanyemployeelist" || path == "oprationalsetup/mycompanylist") {
        $('#LiOprationalSetup').addClass('active');
        $('#LiOprationalSetup ul').css('display', 'block');
        $('#LIMyCompany a').addClass('LiChildActive');

    }
    else if (path == 'projecttype') {
        $('#LiOprationalSetup').addClass('active');
        $('#LiOprationalSetup ul').css('display', 'block');
        $('#LiOprationalSetup_LiProjectType a').addClass('LiChildActive');
    }
    else if (path == 'projecttemplatelist' || path == "editprojecttemplate") {
        $('#LiOprationalSetup').addClass('active');
        $('#LiOprationalSetup ul').css('display', 'block');
        $('#LiOprationalSetup_LiProjectTemplate a').addClass('LiChildActive');
    }
    else if (path == 'stage') {
        $('#LiOprationalSetup').addClass('active');
        $('#LiOprationalSetup ul').css('display', 'block');
        $('#LiOprationalSetup_Listages a').addClass('LiChildActive');
    }
    else if (path == 'type') {
        $('#LiOprationalSetup').addClass('active');
        $('#LiOprationalSetup ul').css('display', 'block');
        $('#LiOprationalSetup_Type a').addClass('LiChildActive');
    }
    else if (path == 'company' || path == "editcompany" || path == "clientsitelist" || path == "companycontactlist" || path == "companyprojectlist" || path == "comapny_documents") {
        $('#LiSales').addClass('active');
        $('#LiSales ul').css('display', 'block');
        $('#LiSales_Company a').addClass('LiChildActive');
    }
    else if (path == 'contact' || path == "editcontact" || path == "contact_documents") {
        $('#LiSales').addClass('active');
        $('#LiSales ul').css('display', 'block');
        $('#LiSales_Contacts a').addClass('LiChildActive');
    }
    else if (path == 'employeelist' || path == 'employee' || path == "employee_documents" || path == "securityaccess") {
        $('#LiManage,#LIHr').addClass('active');
        $('#UlManage,#UlHr').css('display', 'block');
        $('#LIHr_Employee a').addClass('LiChildActive');
    }
    else if (path == 'jobandroles') {
        $('#LiManage,#LIHr').addClass('active');
        $('#UlManage,#UlHr').css('display', 'block');
        $('#lijobandRole a').addClass('LiChildActive');
    }
    else if (path == 'userrole') {
        $('#LiManage,#LIHr').addClass('active');
        $('#UlManage,#UlHr').css('display', 'block');
        $('#LIHr_Role a').addClass('LiChildActive');
    }
    else if (path == 'billingratecode') {
        $('#LiManage,#LibillingNext').addClass('active');
        $('#UlManage,#UlbillingChild').css('display', 'block');
        $('#liBillingrate a').addClass('LiChildActive');
    }
    $('#LiSales ul').css('display', 'block');
    //add active class by match url End


});

function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}
function numberWithCommas(x) {
    var parts = x.toString().split(".");
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join(".");
}
function HideChildMenu() {
    $('#ulalwaysdashboardactive').css("display", "none");
    $('#ulalwaysactive').css("display", "none");
    $('#ulalwaysact').css("display", "none");

}
function hideAlert() {
    setTimeout(function () {
        $(".fadeOut").fadeOut(2000);
    }, 3000);
}
function GoToJob() {
    $('#LoadingPanel1').show();
    var txtSacff = $("#txtSacff").val();
    var id = $('#ddlClient').val();
    var UID = getParameterByName('UID');

    if (!/^\d+$/.test(txtSacff)) {
        var response = {
            Result: "Failed",
            Message: "Invalid Scaf Ref. Please enter a numeric Scaf Ref."
        };
        CustomAlert(response);
        $('#LoadingPanel1').hide();
        return;
    }

    if (txtSacff == "" || txtSacff == null) {
        var response = { Result: "Failed", Message: "Please Enter The Scaf Ref Number to Search ." };
        CustomAlert(response);
        $('#LoadingPanel1').hide();
    } else {
        $.ajax({
            url: '/Layout/GoToJob',
            type: 'Post',
            data: { id: id, txtSacff: txtSacff, UID: UID },
            async: false,
            success: function (response) {
                var resp = response;
                if (response.Result == "Success") {
                    var RID = response.Message;
                    var UID = getParameterByName('UID');
                    var UserRole = getParameterByName('UserRole');

                    var EncryptData = Encrypt([id, RID]);
                    id = EncryptData[0];
                    RID = EncryptData[1];
                    window.location.href = '/Register/Register?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID;
                } else {
                    CustomAlert(response);
                    $('#LoadingPanel1').hide();
                }
            }
        });

    }
    //  return flag;
}
function GoToInspection() {
    $('#LoadingPanel1').show();
    var txtSacff = $("#txtSacff").val();
    var UID = getParameterByName('UID');

    if (!/^\d+$/.test(txtSacff)) {
        var response = {
            Result: "Failed",
            Message: "Invalid Scaf Ref. Please enter a numeric Scaf Ref."
        };
        CustomAlert(response);
        $('#LoadingPanel1').hide();
        return;
    }
    var id = $('#ddlClient').val();
    if (txtSacff == "" || txtSacff == null) {
        var response = { Result: "Failed", Message: "Please Enter The Scaf Ref Number to Search ." };
        CustomAlert(response);
        $('#LoadingPanel1').hide();
    }
    else {
        $.ajax({
            url: '/Layout/GoToInspection',
            type: 'Post',
            data: { id: id, txtSacff: txtSacff, UID: UID },
            async: false,
            success: function (response) {
                var resp = response;
                if (response.Result == "Success") {
                    var RID = response.Message;
                    var UID = getParameterByName('UID');
                    var UserRole = getParameterByName('UserRole');

                    var EncryptData = Encrypt([id, RID]);
                    id = EncryptData[0];
                    RID = EncryptData[1];
                    window.location.href = '/InspectionList/InspectionList?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID;
                } else {
                    CustomAlert(response);
                    $('#LoadingPanel1').hide();
                }
            }
        });
    }
}
function btnLogout_Click() {
    //Redirect To Login Page
    // window.location.href = '/Login/Login';
    var UID = getParameterByName('UID');
    var hdnoffset = $('#hdnTimeOffSetForAllPage').val();
    $.ajax({
        type: 'POST',
        data: JSON.stringify({ UID: UID, hdnoffset: hdnoffset }),
        // data: JSON.stringify({ UID: UID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        url: '/Layout/LogOutClick',
        success: function (response) {
            if (response.Result == "SessionOut") {
                window.location.href = '/Login/Login';
            }

        }
    });
}

function DropdownBindClient() {
    // var ID = Session["ClientId"];
    //  var sValue = '<%=HttpContext.Current.Session["ClientId"]%>';
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    $.ajax({
        type: "POST",
        data: JSON.stringify({ UID: UID, UserRole: UserRole }),
        url: "/Layout/DropdownBindClient",
        //data: JSON.stringify({ id: ClientID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            if (response.Result == "SessionOut") {
                window.location.href = '/Login/Login';
            } else {
                $('#lblUserName').text(response.Fullname);
                //  var SubCategory1 = response.d;
                var ClientData = response.getData;

                //  var SubCategory1 = response.d;
                // var ClientData = response;
                if (ClientData.length > 0) {
                    for (var i = 0; i < ClientData.length; i++) {

                        $("#ddlClient").append("<option value='" + ClientData[i].ClientId + "'>" + ClientData[i].Name + "</option>");

                    }
                    var ClientID = getParameterByName('id');
                    var ClientID = getParameterByName('id') == null ? "" : getParameterByName('id');
                    var UserRole = getParameterByName('UserRole');
                    var DecryptData = Decrypt([ClientID, UserRole]);
                    UserRole = DecryptData[1];
                    if (UserRole != 10 && UserRole != 80) {
                        // $('#ddlClient').css("disabled", "disabled");
                        $("#ddlClient").prop("disabled", true);
                    }

                    if (DecryptData[0] != "") {
                        $("#ddlClient").val(DecryptData[0]);

                    }
                }
            }
        }

    })
}

function ddlClient_SelectedIndexChanged() {
    var ClientID = $("#ddlClient").val();
    if (ClientID == "" || ClientID == null) {
        //$('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
        //$('#art1').css("display", "block");
        //$('#alertlabel').html('<strong>Alert ! </strong>Please Select Client.');
        //hideAlert();
        var Response = { Result: "Failed", Message: "Please Select Client." };
        CustomAlert(Response);
    } else {
        var UID = getParameterByName('UID');
        var UserRole = getParameterByName('UserRole');

        var EncryptData = Encrypt([ClientID]);
        ClientID = EncryptData[0];
        window.location.href = '/Layout/ddlClient_SelectedIndexChanged?UID=' + UID + '&UserRole=' + UserRole + '&id=' + ClientID;

    }
}

// Get Date Format for Json Date

function DateFormat(date) {

    if (date != null && date != undefined && date != "") {
        date = date.toString();
        var newDate = new Date(parseInt(date.substr(6)));
        var DateTo = ("0" + (newDate.getMonth() + 1)).slice(-2) + '-' + ("0" + newDate.getDate()).slice(-2) + '-' + newDate.getFullYear();
        // const months = ["JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC"];
        const months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

        var DateTo2 = ("0" + newDate.getDate()).slice(-2) + '-' + months[parseInt(newDate.getMonth())] + '-' + newDate.getFullYear();

        // var newDate=DateTo.toShortFormat();
        return DateTo2.toString();
    }
    else {
        return "";
    }
}

// Get DateTime Format for Json Datetime
function DateTimeFormat(date) {
    if (date != null && date != undefined && date != "") {
        date = date.toString();
        var newDate = new Date(parseInt(date.substr(6)));
        // var DateTo = ("0" + (newDate.getMonth() + 1)).slice(-2) + '/' + ("0" + newDate.getDate()).slice(-2) + '/' + newDate.getFullYear() + ' ' + tConvert(("0" + newDate.getHours()).slice(-2) + ':' + ("0" + newDate.getMinutes()).slice(-2));

        const months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

        var DateTo2 = ("0" + newDate.getDate()).slice(-2) + '-' + months[parseInt(newDate.getMonth())] + '-' + newDate.getFullYear() + ' ' + tConvert(("0" + newDate.getHours()).slice(-2) + ':' + ("0" + newDate.getMinutes()).slice(-2));

        return DateTo2.toString();
        //return DateTo.toString();
    }
    else {
        return "";
    }
}

function tConvert(time) {
    // Check correct time format and split into components
    time = time.toString().match(/^([01]\d|2[0-3])(:)([0-5]\d)(:[0-5]\d)?$/) || [time];

    if (time.length > 1) { // If time format correct
        time = time.slice(1); // Remove full string match value
        time[5] = +time[0] < 12 ? ' AM' : ' PM'; // Set AM/PM
        time[0] = +time[0] % 12 || 12; // Adjust hours
    }
    return time.join(''); // return adjusted time or original string
}

// Get Date Format  DD-MM-YYYY
Date.prototype.DateMonthFormat = function () {

    var month_names = ["Jan", "Feb", "Mar",
        "Apr", "May", "Jun",
        "Jul", "Aug", "Sep",
        "Oct", "Nov", "Dec"];

    var day = this.getDate();
    var month_index = this.getMonth();
    var year = this.getFullYear();

    return "" + ('0' + day).slice(-2) + "-" + month_names[month_index] + "-" + year;
}

// Get DateTime Format  DD-MM-YYYY h:m
function getDateTime() {
    var month_names = ["Jan", "Feb", "Mar",
        "Apr", "May", "Jun",
        "Jul", "Aug", "Sep",
        "Oct", "Nov", "Dec"];

    var d = new Date();
    d.setHours(d.getHours()); // offset from local time
    var h = (d.getHours() % 12) || 12; // show midnight & noon as 12
    return (
        +d.getDate() + "-" + month_names[d.getMonth()] + "-" + d.getFullYear() + ' ' +
        //  +d.getDate() + "-" + d.getMonth() + "-" + d.getFullYear() + ' ' +
        (h < 10 ? '0' : '') + h +
        (d.getMinutes() < 10 ? ':0' : ':') + d.getMinutes() +
        (d.getHours() < 12 ? ' AM' : ' PM')
    );
}

function Encrypt(EncryptData) {
    var result = null;
    $.ajax({
        url: '/Layout/Encrypt',
        type: 'Post',
        data: { encrypt: EncryptData },
        async: false,
        success: function (response) {
            var resp = response;
            result = resp;
        }
    });
    return result;
}


function Decrypt(DecryptData) {
    var result = null;
    $.ajax({
        url: '/Layout/Decrypt',
        type: 'Post',
        data: { decrypt: DecryptData },
        async: false,
        success: function (response) {
            var resp = response;
            result = resp;
        }
    });
    return result;
}


// Restricts input for the given textbox to the given inputFilter.
function setInputFilter(textbox, inputFilter) {
    ["input", "keydown", "keyup", "mousedown", "mouseup", "select", "contextmenu", "drop"].forEach(function (event) {
        textbox.addEventListener(event, function () {
            if (inputFilter(this.value)) {
                this.oldValue = this.value;
                this.oldSelectionStart = this.selectionStart;
                this.oldSelectionEnd = this.selectionEnd;
            } else if (this.hasOwnProperty("oldValue")) {
                this.value = this.oldValue;
                this.setSelectionRange(this.oldSelectionStart, this.oldSelectionEnd);
            } else {
                this.value = "";
            }
        });
    });
}
/*Added By Neeraj Joshi on 22 /08 / 1024*/
function showCustomAlert(message) {
    console.log("showCustomAlert function called");
    let formattedMessage = message.replace(/<br\s*\/?>/gi, "\n");
    console.log("Formatted Message: ", formattedMessage);
    $('#customAlertMessage').text(formattedMessage);
    $('#customAlertBox').css('display', 'block');
}

function customAlertClose() {
    $('#customAlertBox').css('display', 'none');
}
function InventorySiteHistory() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var id = getParameterByName('id');
    window.location.href = '/InventorySiteHistory/InventorySiteHistory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
}

//// Install input filters.
//setInputFilter(document.getElementById("intTextBox"), function(value) {
//  return /^-?\d*$/.test(value); });
//setInputFilter(document.getElementById("uintTextBox"), function(value) {
//  return /^\d*$/.test(value); });
//setInputFilter(document.getElementById("intLimitTextBox"), function(value) {
//  return /^\d*$/.test(value) && (value === "" || parseInt(value) <= 500); });
//setInputFilter(document.getElementById("floatTextBox"), function(value) {
//  return /^-?\d*[.,]?\d*$/.test(value); });
//setInputFilter(document.getElementById("currencyTextBox"), function(value) {
//  return /^-?\d*[.,]?\d{0,2}$/.test(value); });
//setInputFilter(document.getElementById("latinTextBox"), function(value) {
//  return /^[a-z]*$/i.test(value); });
//setInputFilter(document.getElementById("hexTextBox"), function(value) {
//  return /^[0-9a-f]*$/i.test(value); });

function ScafMapJobPipelinestatus() {
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var Clientid = getParameterByName('id');
    Clientid = getParameterByName('id');
    window.location.href = '/ScafMapJobPipelinestatus/ScafMapJobPipelinestatus?UID=' + UID + '&UserRole=' + UserRole + '&id=' + Clientid;
}