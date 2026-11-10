
function BindallDropdown() {
    var id = getParameterByName('id');
    $.ajax({
        url: '/InspectionList/BindallDropdown',
        async: false,
        data: { id: id },
        success: function (response) {

            data = response;

            var ddlInspectedBy = "<option value='' disabled selected='selected'>Select---</option>";
            $.each(response.ddlInspectedBy, function (idx, obj) {
                if (obj != null) {
                    ddlInspectedBy += '<option value="' + obj.UserId + '">' + obj.name + '</option>';
                }
            });

            var ddlChecklist = "<option value='' disabled selected='selected'>Select---</option>";
            $.each(response.ddlChecklistInsp, function (idx, obj) {
                if (obj != null) {
                    ddlChecklist += '<option value="' + obj.checklistID + '">' + obj.checklistdescription + '</option>';
                }
            });

            $('#ddlInspecdeplank').empty().append(ddlInspectedBy);
            $('#ddlddlInspectedByadd').empty().append(ddlInspectedBy);
            $('#ddlChecklistInsp').empty().append(ddlChecklist);
        }
    });
}
function GetInspectionListData() {
    $('#LoadingPanel1').show();
    var id = getParameterByName('id');
    var RID = getParameterByName('RID');
    var hdnTimeOffSet = $('#hdnTimeOffSet').val();
    var HistInspList = $('#HistInspList').prop('checked');
    $.ajax({
        type: "POST",
        url: "/InspectionList/ShowInspectionListDetails",
        data: JSON.stringify({ id: id, RID: RID, hdnTimeOffSet: hdnTimeOffSet, HistInspList: HistInspList }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            var str = "";
            var GetData = response;
            $('#tblInspectionList').DataTable().clear();
            $('#tblInspectionList').DataTable().destroy();

            if (GetData.length > 0) {

                for (var i = 0; i < GetData.length; i++) {

                    var InspectionID = GetData[i].InspectionID == null ? "" : GetData[i].InspectionID;
                    var InspectionDate = GetData[i].InspectionDate == null ? "" : GetData[i].InspectionDate;

                    var InspectionTime = GetData[i].InspectionTime == null ? "" : GetData[i].InspectionTime;
                    var InspectedById = GetData[i].InspectedById == null ? "" : GetData[i].InspectedById;
                    var InspectedBy = GetData[i].InspectedBy == null ? "" : GetData[i].InspectedBy;
                    var InspFileName = GetData[i].InspFileName == null ? "" : GetData[i].InspFileName;
                    var Comments = GetData[i].Comments == null ? "" : GetData[i].Comments;
                    var RegisterID = GetData[i].RegisterID == null ? "" : GetData[i].RegisterID;
                    var OnlineInsp = GetData[i].OnlineInsp == null ? "" : GetData[i].OnlineInsp;
                    if (OnlineInsp == false) {
                        OnlineInsp = "Yes";
                    } else {
                        OnlineInsp = "";
                    }
                    var CompleteInsp = GetData[i].CompleteInsp == null ? "" : GetData[i].CompleteInsp;
                    var Lat = GetData[i].Lat == null ? "" : GetData[i].Lat;
                    var Long = GetData[i].Long == null ? "" : GetData[i].Long;
                    var GPSAddress = GetData[i].GPSAddress == null ? "" : GetData[i].GPSAddress;
                    var Inspimage = GetData[i].Inspimage == null ? "" : GetData[i].Inspimage;
                    var InspAudio = GetData[i].InspAudio == null ? "" : GetData[i].InspAudio;
                    var Inspvedio = GetData[i].Inspvedio == null ? "" : GetData[i].Inspvedio;
                    var InspSign = GetData[i].InspSign == null ? "" : GetData[i].InspSign;
                    var InspComments1 = GetData[i].InspComments == null ? "" : GetData[i].InspComments;
                    // Escape single & double quotes so they don't break the title attribute
                    var InspCommentsEsc = InspComments1.replace(/&/g, '&amp;').replace(/'/g, '&#39;').replace(/"/g, '&quot;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
                    var InspComments = (InspComments1.length > 40) ? InspComments1.substring(0, 40) + '.....' : InspComments1;
                    var Acknowledgment = GetData[i].Acknowledgment == null ? "" : GetData[i].Acknowledgment;
                    var InspStatus = GetData[i].InspStatus == "Scheduled" ? "" : GetData[i].InspStatus;
                    var AckDate = GetData[i].AckDate == null ? "" : GetData[i].AckDate;
                    var AckDateTime = GetData[i].AckDateTime == null ? "" : GetData[i].AckDateTime;
                    var LastEmailSentOn = GetData[i].LastEmailSentOn == null ? "" : GetData[i].LastEmailSentOn;
                    var ScheduledOrAckDate = GetData[i].ScheduledOrAckDate == null ? "" : GetData[i].ScheduledOrAckDate;
                    var ScheduledOrAckTime = GetData[i].ScheduledOrAckTime == null ? "" : GetData[i].ScheduledOrAckTime;
                    var OffSystemInspection = GetData[i].OffSystemInspection == null ? "" : GetData[i].OffSystemInspection;

                    var OffSystInspection = "";
                    if (OffSystemInspection == true) {
                        OffSystInspection = "Yes";
                    }

                    if (ScheduledOrAckTime == 0) {
                        //InspectionDate = DateFormat(InspectionDate);//Commented on 03-July-2021
                        InspectionDate = DateFormat(ScheduledOrAckDate);
                    } else {
                        //InspectionDate = DateTimeFormat(InspectionDate);
                       InspectionDate = DateTimeFormat(ScheduledOrAckDate);
                    }

                    str += "<tr>"
                    str += "<td class='setWidth0' style='display:none'></td>"
                    str += "<td class='setWidth1' title='Click here for Delete Inspection' style='cursor:pointer;'><i class='fa fa-trash setWidth1' onclick=ShowDeletePopup('" + InspectionID + "')></i></td>"
                    str += "<td class='setWidth2'>" + InspStatus + "</td>"
                    str += "<td class='setWidth3'>" + OnlineInsp + "</td>"
                    str += "<td class='setWidth3'>" + OffSystInspection + "</td>"
                    str += "<td class='setWidth4'>" + InspectionDate + "</td>"
                    str += "<td class='setWidth5'>" + InspectedBy + "</td>"
                    /* str += "<td class='setWidth6' title='" + InspComments1 + "'>" + InspComments + "</td>"*/
                    str += "<td class='setWidth6' title=\"" + InspCommentsEsc + "\">" + InspComments + "</td>";

                    if (InspStatus == "De-Planked" || OffSystemInspection == true) {
                        str += "<td class='setWidth7'></td>"
                        str += "<td class='setWidth8'></td>"
                        str += "<td class='setWidth9'></td>"

                    } else {
                        str += "<td title='click here to open Inspection Form.' style='cursor:pointer;'><a type='button' id='tbnOpen' class='btnOpen' ChkInspectionID_='" + InspectionID + "' onclick=OpenInspectionForm('" + InspectionID + "') ><i class='fa fa-link'></i>&nbsp;Open</a></td>";

                        str += "<td title='click here to view Pdf Report.' style='cursor:pointer;'><button type='button' Class='w3-btn w3-teal' onclick=ViewPdfDocument('" + InspectionID + "') ><i class='fa fa-search'></i>&nbsp;View</button></td>"
                        if (LastEmailSentOn == "") {
                            str += "<td title='click here to email Inspection.' style='cursor:pointer;'><button type='button' Class='btn btn-danger' onclick=btnEmailClick('" + InspectionID + "') ><i class='fa fa-envelope'></i>&nbsp;Email</button></td>"
                        } else {
                            str += "<td title='click here to email Inspection.' style='cursor:pointer;'><button type='button' Class='btn btn-success' onclick=btnEmailClick('" + InspectionID + "') ><i class='fa fa-envelope'></i>&nbsp;Email</button></td>"
                        }
                    }

                    str += "<td class='setWidth10'>" + DateFormat(LastEmailSentOn) + "</td>";

                }

                $('#tbodyInspectionList').append(str);

            }
            $('#LoadingPanel1').hide();
            var table = $('#tblInspectionList').DataTable({
                "bSort": false,
                "processing": true,
                searching: true,
                "bLengthChange": true,
                "bInfo": false,
                autoWidth: false,
                scrollX: true,
                scrollY: true,
                scrollCollapse: true,
                scrollY: "60vh",
                "lengthMenu": [10, 25, 50],
                "pageLength": 10,

            });
            //var UserRole = getParameterByName("UserRole");
            //var DecriptValues = Decrypt([UserRole]);
            //UserRole = DecriptValues[0];
            //if (UserRole != 10) {
            //    table.column(1).visible(false);

            //}

            // Hide Show For DataTable Column And Buttons Start Here

            GridButtonHideShow(table, "tblInspectionList");

            // Hide Show For DataTable Column And Buttons End Here
        }
    })
}



function OpenInspectionForm(InspectionID) {
    if (IsSiteStatus == "InActive") {
        CustomAlert({
            Result: "Failed", Message: "This site is InActive. This Action is not allowed."
        });
        return false;
    }
    $.ajax({
        type: "POST",
        url: "/InspectionList/GetCheckListID",
        data: JSON.stringify({ InspectionID: InspectionID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            if (response.Result == "Success") {
                var id = getParameterByName('id');
                var UID = getParameterByName('UID');
                var UserRole = getParameterByName('UserRole');
                var RID = getParameterByName('RID');

                var GetDate = response.Message.split('_');
                var CheckListID = GetDate[0];
                InspectionID = GetDate[1];
                if (InspectionID != "") {
                    window.location.href = '/InspectionForm/InspectionForm?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID + '&CheckListID=' + CheckListID + '&InspID=' + InspectionID;
                }
            } else {
                CustomAlert(response);
                $('#LoadingPanel1').hide();
            }
        }
    })
}
function ViewPdfDocument(InspectionID) {
    $('#LoadingPanel1').show();
    var id = getParameterByName('id');
    $.ajax({
        type: "POST",
        url: "/InspectionList/ViewPdfDocument",
        data: JSON.stringify({ id: id, InspID: InspectionID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            if (response.Result == "Success") {
                var PdfPath = response.Message;
                if (PdfPath != "") {
                    window.open(PdfPath);
                    $('#LoadingPanel1').hide();
                }
            }
        }, error: function (x, y, z) {
            alert(z);
        }
    });

}
function btnEmailClick(InspectionID) {
    if (IsSiteStatus == "InActive") {
        CustomAlert({
            Result: "Failed", Message: "This site is InActive. This Action is not allowed."
        });
        return false;
    }
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var RID = getParameterByName('RID');
    var DomainName = window.location.origin;
    $.ajax({
        type: "POST",
        url: "/InspectionList/btnEmailClick",
        data: JSON.stringify({ id: id, UID: UID, UserRole: UserRole, InspID: InspectionID, RID: RID, DomainName: DomainName }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
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
                $('#hdnInspectionID').val(InspectionID);
                $('#emailConfirm').css('display', 'block');

            } else {
                var Response = { Result: "Failed", Message: "Please Sand Email again. " };
                CustomAlert(response);
                $('#LoadingPanel1').hide();
            }
        }
    });
}
function btnSend_Click() {
    if (IsSiteStatus == "InActive") {
        CustomAlert({
            Result: "Failed", Message: "This site is InActive. This Action is not allowed."
        });
        return false;
    }
    $('#LoadingPanel1').show();
    var txtClientEmail = $('#txtClientEmail').val();
    var txtcc = $('#txtcc').val();
    var txtsubject = $('#txtsubject').val();
    var txtEmailMSg = $('#txtEmailMSg').text();
    var txtFile = $('#txtFile').val();
    var InspectionID = $('#hdnInspectionID').val();
    $.ajax({
        type: "POST",
        url: "/InspectionList/btnSend_Click",
        data: JSON.stringify({ txtClientEmail: txtClientEmail, txtcc: txtcc, txtsubject: txtsubject, txtEmailMSg: txtEmailMSg, txtFile: txtFile, InspectionID: InspectionID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            if (response.Result == "Success") {
                CustomAlert(response);
                $('#LoadingPanel1').hide();
                $('#emailConfirm').css('display', 'none');
                GetInspectionListData();
            } else {
                CustomAlert(response);
                $('#LoadingPanel1').hide();
                $('#emailConfirm').css('display', 'none');
            }
        }
    });
}
function btnCancel_Click() {
    //        if (File.Exists(txtFile.Text))
    //{
    //            try
    //{
    //                File.Delete(txtFile.Text);

    //}
    //            catch { }
    //}
    //emailConfirm.Style.Add("display", "none");
    $('#emailConfirm').css('display', 'none');
}
function ShowDeletePopup(InspectionID) {
    if (IsSiteStatus == "InActive") {
        CustomAlert({
            Result: "Failed", Message: "This site is InActive. This Action is not allowed."
        });
        return false;
    }
    $('#hdnInspectionID').val(InspectionID);
    $('#divdelete').css('display', 'block');
}
function btndeleteyes_Click() {
    if (IsSiteStatus == "InActive") {
        CustomAlert({
            Result: "Failed", Message: "This site is InActive. This Action is not allowed."
        });
        return false;
    }
    $('#LoadingPanel1').show();
    var hdnInspectionID = $('#hdnInspectionID').val();
    $.ajax({
        type: "POST",
        url: "/InspectionList/btndeleteyes_Click",
        data: JSON.stringify({ hdnInspectionID: hdnInspectionID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            if (response.Result == "Success") {
                $('#divdelete').css('display', 'none');
                CustomAlert(response);
                GetInspectionListData();
                document.location.reload();
            } else {
                $('#divdelete').css('display', 'none');
                CustomAlert(response);
                $('#LoadingPanel1').hide();
            }
        }
    })
}

function btndeleteno_Click() {
    $('#divdelete').css('display', 'none');
}


//function btnDeplank_Click() {
//    if ($('#lblNewstatus').val() == "Completed") {
//        $('#divDeplank').css('display', 'none');
//        var Response = { Result: "Failed", Message: "Since Job has been Completed so no further action allowed." };
//        CustomAlert(Response);
//    }
//    else {
//        BindallDropdown();
//        if ($('#hdnLoginUserId').val() != "") {
//            $('#ddlInspecdeplank').val($('#hdnLoginUserId').val());
//        }
//        $('#divDeplank').css('display', 'block');
//        //divDeplank.Style.Add("display", "block");
//        // dateInspection.SelectedDate =DateTime.Now;
//        //dateInspection.SelectedDate = Convert.ToDateTime(hdnCurrentDate.Value);
//        $('#txtcomm').val('');
//        $('#dateInspection').val(getDateTime());
//    }

//}

function btndeplanksave_Click() {
    if (IsSiteStatus == "InActive") {
        CustomAlert({
            Result: "Failed", Message: "This site is InActive. This Action is not allowed."
        });
        return false;
    }
    $('#LoadingPanel1').show();
    var id = getParameterByName('id');
    var RID = getParameterByName('RID');
    var UID = getParameterByName('UID');
    var txtcomm = $('#txtcomm').val();
    var txtdeplanl = $('#txtdeplanl').val();
    var ddlInspecdeplank = $('#ddlInspecdeplank').val();
    var dateInspection = $('#dateInspection').val();
    var hdnTimeOffSet = $('#hdnTimeOffSet').val();

    // if ((ddlInspecdeplank != null) && dateInspection != "") {
    if (ddlInspecdeplank != null) {
        $('#ddlInspecdeplank').removeClass('requiredColumn');
        if (dateInspection != "") {
            $('#dateInspection').removeClass('requiredColumn');
            $.ajax({
                type: "POST",
                url: "/InspectionList/btndeplanksave_Click",
                data: JSON.stringify({ id: id, UID: UID, RID: RID, txtcomm: txtcomm, txtdeplanl: txtdeplanl, ddlInspecdeplank: ddlInspecdeplank, dateInspection: dateInspection, hdnTimeOffSet: hdnTimeOffSet }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                success: function (response) {
                    if (response.Result == "Success") {
                        $('#divDeplank').css("display", "none");
                        CustomAlert(response);
                        GetInspectionListData();
                    } else {
                        $('#divDeplank').css("display", "none");
                        CustomAlert(response);
                        $('#LoadingPanel1').hide();
                    }
                }
            })

        }
        else {
            var Response = { Result: "Failed", Message: "Please select Scheduled Inspection Date" };
            CustomAlert(Response);
            $('#dateInspection').addClass('requiredColumn');
            $('#LoadingPanel1').hide();
        }
    }
    else {
        var Response = { Result: "Failed", Message: "Please select Inspected By." };
        CustomAlert(Response);
        $('#ddlInspecdeplank').addClass('requiredColumn');
        $('#LoadingPanel1').hide();
    }

    //} else {
    //    var Response = { Result: "Failed", Message: "Please fill all  mandatory field." };
    //    CustomAlert(Response);
    //    $('#LoadingPanel1').hide();
    //}

};



function btnAddInspection() {
    if (IsSiteStatus == "InActive") {
        CustomAlert({
            Result: "Failed", Message: "This site is InActive. This Action is not allowed."
        });
        return false;
    }
    BindallDropdown();
    if ($('#hdnCheckListID').val() != "") {
        $('#ddlChecklistInsp').val($('#hdnCheckListID').val());
    }
    if ($('#hdnLoginUserId').val() != "") {
        $('#ddlddlInspectedByadd').val($('#hdnLoginUserId').val());
    }
    if ($('#lblNewstatus').val() == "Completed") {
        $('#Adddocs').css('display', 'none');
        var Response = { Result: "Failed", Message: "Since Job has been Completed so no further action allowed." };
        CustomAlert(Response);
    }
    else {
        //rdpInspectiondate.SelectedDate = Convert.ToDateTime(hdnCurrentDate.Value);
        $('#txtComments').val('');

        // Hide Add New User Button For All Role Only Shows Super Admin
        var UserRole = getParameterByName('UserRole');
        var DecryptData = Decrypt([UserRole]);
        var UserRole = DecryptData[0];
        if (UserRole != "10") {
            $('#btnAddInspection').css('display', 'none');
        }
        $('#rdpInspectiondate').val(getDateTime());
        $('#Adddocs').css('display', 'block');
    }

}

function btnsave_Click() {
    if (IsSiteStatus == "InActive") {
        CustomAlert({
            Result: "Failed", Message: "This site is InActive. This Action is not allowed."
        });
        return false;
    }
    $('#LoadingPanel1').show();
    var id = getParameterByName('id');
    var RID = getParameterByName('RID');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var ddlddlInspectedByadd = $('#ddlddlInspectedByadd').val();
    var dateInspection = $('#dateInspection').val();
    var hdnTimeOffSet = $('#hdnTimeOffSet').val();
    var ddlInspStatuspop = "Scheduled";
    var ddlChecklistInsp = $('#ddlChecklistInsp').val();
    var rdpInspectiondate = $('#rdpInspectiondate').val();
    // if (ddlddlInspectedByadd != null && ddlChecklistInsp!=null && rdpInspectiondate != "") {
    if (ddlChecklistInsp != null) {
        $('#ddlChecklistInsp').removeClass('requiredColumn');
        if (ddlddlInspectedByadd != null) {
            $('#ddlddlInspectedByadd').removeClass('requiredColumn');
            if (rdpInspectiondate != "") {
                $('#rdpInspectiondate').removeClass('requiredColumn');
                $.ajax({
                    type: "POST",
                    url: "/InspectionList/btnsave_Click",
                    data: JSON.stringify({ id: id, UID: UID, RID: RID, ddlddlInspectedByadd: ddlddlInspectedByadd, hdnTimeOffSet: hdnTimeOffSet, ddlInspStatuspop: ddlInspStatuspop, ddlChecklistInsp: ddlChecklistInsp, rdpInspectiondate: rdpInspectiondate }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true,
                    success: function (response) {
                        if (response.Result == "Success") {
                            $('#Adddocs').css("display", "none");
                            CustomAlert(response);
                            var GetDate = response.Data.split('_');
                            var CheckListID = GetDate[0];
                            var InsID = GetDate[1];
                            window.location.href = '/InspectionForm/InspectionForm?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID + '&CheckListID=' + CheckListID + '&InspID=' + InsID;


                        } else {
                            $('#Adddocs').css("display", "none");
                            CustomAlert(response);
                            $('#LoadingPanel1').hide();
                        }
                    }
                })
            }
            else {
                var Response = { Result: "Failed", Message: "Please select Scheduled Inspection Date" };
                CustomAlert(Response);
                $('#rdpInspectiondate').addClass('requiredColumn');
                $('#LoadingPanel1').hide();
            }
        }
        else {
            var Response = { Result: "Failed", Message: "Please select Inspected By." };
            CustomAlert(Response);
            $('#ddlddlInspectedByadd').addClass('requiredColumn');
            $('#LoadingPanel1').hide();
        }
    } else {
        var Response = { Result: "Failed", Message: "Please select Scaffold Inspection List." };
        CustomAlert(Response);
        $('#ddlChecklistInsp').addClass('requiredColumn');
        $('#LoadingPanel1').hide();
    }

    //} else {
    //        var Response = { Result: "Failed", Message: "Please fill all  mandatory field." };
    //CustomAlert(Response);
    //$('#LoadingPanel1').hide();
    //}
};
function btnAddInspection_Click() {
    if (IsSiteStatus == "InActive") {
        CustomAlert({
            Result: "Failed", Message: "This site is InActive. This Action is not allowed."
        });
        return false;
    }
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var RID = getParameterByName('RID');
    window.location.href = '/UserInfo/UserInfo?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID;

    // Response.Redirect("UserInfo.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&UserID=0" + Request.QueryString["UserId"] + "&RID=" + Request.QueryString["RID"], false);
}

$(function () {
    CheckIsOfflineCurrentForInspection();
});
function CheckIsOfflineCurrentForInspection() {
    $('#LoadingPanel1').show();
    var id = getParameterByName('id');
    var RID = getParameterByName('RID');
    var hdnTimeOffSet = $('#hdnTimeOffSetForAllPage').val();   // Getting Values From Layout Page
    $.ajax({
        type: "POST",
        url: "/Register/ShowregisterDetails",
        //data: JSON.stringify({ id: id, RID: RID }),
        data: JSON.stringify({ id: id, RID: RID, hdnTimeOffSet: hdnTimeOffSet }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            var str = "";
            var GetData = response;
            var IsOfflineCurrentForInspection = GetData.IsOfflineCurrentForInspection == null ? false : GetData.IsOfflineCurrentForInspection;
            if (IsOfflineCurrentForInspection == true) {
                var Response = { Result: "Warning", Message: "Please note this job is currently being updated for OFFLINE INSPECTION so no changes will be allowed ONLINE." }
                CustomAlert(Response);
                $('#DivAddDePlanked').css('display', 'none');
                $('#DivAddInspection').css('display', 'none');



                $('#hdnIsOfflineCurrentForInspection').val(IsOfflineCurrentForInspection);
            }
            else {
                IsOfflineCurrentForInspection = false;
                $('#hdnIsOfflineCurrentForInspection').val(IsOfflineCurrentForInspection);
                $('#DivAddDePlanked').css('display', 'block');
                $('#DivAddInspection').css('display', 'block');
            }
        }
    })

}