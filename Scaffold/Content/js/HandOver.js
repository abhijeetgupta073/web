

function GetHandOverListData() {
    $('#LoadingPanel1').show();
    var id = getParameterByName('id');
    var RID = getParameterByName('RID');
    var hdnTimeOffSet = $('#hdnTimeOffSet').val();
    $.ajax({
        type: "POST",
        url: "/HandOver/ShowHandOverListDetails",
        data: JSON.stringify({ id: id, RID: RID, hdnTimeOffSet: hdnTimeOffSet }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            var str = "";
            var GetData = response;
            $('#tblHandOverList').DataTable().clear();
            $('#tblHandOverList').DataTable().destroy();

            if (GetData.length > 0) {

                for (var i = 0; i < GetData.length; i++) {

                    var HandOverID = GetData[i].ID == null ? "" : GetData[i].ID;
                    var HandOverDate = GetData[i].HandOverDate == null ? "" : GetData[i].HandOverDate;
                    var HandOverTime = GetData[i].HandOverTime == null ? "" : GetData[i].HandOverTime;
                    var HandOverDesc1 = GetData[i].HandOverDesc == null ? "" : GetData[i].HandOverDesc;
                    var HandOverDesc = (HandOverDesc1.length > 25) ? HandOverDesc1.substring(0, 25) + '.....' : HandOverDesc1;
                    var CertificateNo = GetData[i].CertificateNo == null ? "" : GetData[i].CertificateNo;
                    var HandOverComments1 = GetData[i].HandOverComments == null ? "" : GetData[i].HandOverComments;
                    var HandOverComments = (HandOverComments1.length > 50) ? HandOverComments1.substring(0, 50) + '.....' : HandOverComments1;
                    var IsConfirmed = GetData[i].IsConfirmed == null ? "" : GetData[i].IsConfirmed;
                    var ContractorSign = GetData[i].ContractorSign == null ? "" : GetData[i].ContractorSign;        
                    if (IsConfirmed == true) {
                        IsConfirmed = "Confirmed";
                    } else {
                        IsConfirmed = "";
                    }
                  
                    if (HandOverTime == 0) {
                        HandOverDate = DateFormat(HandOverDate);
                    } else {
                        HandOverDate = DateTimeFormat(HandOverDate);
                    }



                    str += "<tr>"
                    str += "<td class='setWidth0' style='display:none'></td>"
                    //str += "<td class='setWidth1' id='UniqueDelete' title='Click here for Delete HandOver' style='cursor:pointer;'><i class='fa fa-trash setWidth1' onclick=ShowDeletePopup('" + HandOverID + "')></i></td>"
                    if (IsConfirmed != "Confirmed") {
                        str += "<td class='setWidth1' id='UniqueDelete' title='Click here for Delete HandOver' style='cursor:pointer;'><i class='fa fa-trash setWidth1' onclick=ShowDeletePopup('" + HandOverID + "')></i></td>"
                    } else {
                        str += "<td class='setWidth1'></td>"
                    }
                    str += "<td class='setWidth2'>" + IsConfirmed + "</td>"
                    str += "<td class='setWidth3'>" + HandOverDate + "</td>"
                    str += "<td class='setWidth4' title='" + HandOverDesc1 + "'>" + HandOverDesc + "</td>"
                    str += "<td class='setWidth5'>" + CertificateNo + "</td>"
                    str += "<td class='setWidth6' title='" + HandOverComments1 + "'>" + HandOverComments + "</td>"

                    if (IsConfirmed != "Confirmed") {
                        //str += "<td title='click here to open Inspection Form.' style='cursor:pointer;'><a type='button' id='tbnOpen' class='btnOpen' onclick=OpenHandOverForm('" + HandOverID + "') ><i class='fa fa-link'></i>&nbsp;Open</a></td>";
                        str += "<td title='click here to open Hand Over Form.' style='cursor:pointer;'><a type='button' id='tbnOpen' class='btnOpen' onclick=OpenHandOverForm('" + HandOverID + "') ><i class='fa fa-link'></i>&nbsp;Open</a></td>";
                        str += "<td class='setWidth8'></td>"
                       
                    } else {
                       
                        str += "<td class='setWidth7'></td>"
                        str += "<td title='click here to view Pdf Report.' style='cursor:pointer;'><button type='button' Class='w3-btn w3-teal' onclick=ViewPdfDocument('" + HandOverID + "') ><i class='fa fa-search'></i>&nbsp;View</button></td>"
                    }
         /*           str += "<td title='click here to email Inspection.' style='cursor:pointer;'><button type='button' Class='btn btn-danger' onclick=btnEmailClick('" + HandOverID + "') ><i class='fa fa-envelope'></i>&nbsp;Email</button></td>"*/
                    if (ContractorSign != "") {
                        str += "<td title='click here to email Inspection.' style='cursor:pointer;'><button type='button' Class='btn btn-danger' onclick=btnEmailClick('" + HandOverID + "') ><i class='fa fa-envelope'></i>&nbsp;Email</button></td>";
                    } else {
                        str += "<td></td>";
                    }
                    //Added on 26Feb2025
                    if (IsConfirmed != "Confirmed") {
                        str += "<td class='setWidth10'></td>"

                    } else {
                        //str += "<td title='click here to unconfirm Hand Over.'><button type='button' Class='w3-btn w3-blue' onclick=ShowUnconfirmPopup('" + HandOverID + "') >Unconfirm</button></td>"
                        str += "<td title='click here to unconfirm Hand Over.'><a class='cross-icon' onclick=ShowUnconfirmPopup('" + HandOverID + "')><b>X</b></a></td>"
                    }


                    str += "</tr>";
                }

             
            }
               $('#tbodyHandOverList').append(str);
               $('#LoadingPanel1').hide();
            var table = $('#tblHandOverList').DataTable({
                "bSort": false,
                "processing": true,
                searching: true,
                "bLengthChange": true,
                "bInfo": false,
                autoWidth: false,
                scrollX: true,
                scrollY: true,
                scrollCollapse: true,
                scrollY: "60vh"
               
            });

                // Hide Show For DataTable Column And Buttons Start Here

            GridButtonHideShow(table, "tblHandOverList");

                // Hide Show For DataTable Column And Buttons End Here
            }
    })
}

function OpenHandOverForm(HandOverID) {
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

    var EncryptData = Encrypt([HandOverID]);
    HandOverID = EncryptData[0];

    window.location.href = '/HandOverForm/HandOverForm?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID + '&HandOverId=' + HandOverID;
    //Response.Redirect(DomainName + "/" + "HandOverForm?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RID=" + Request.QueryString["RID"] + "&HandOverId=" + databaseCon.Encrypt(HandOverID) + "&FormPage=" + "");
  
}
function ViewPdfDocument(HandOverID) {
    $('#LoadingPanel1').show();
    var id = getParameterByName('id');
    var RID = getParameterByName('RID');
    $.ajax({
        type: "POST",
        url: "/HandOver/ViewPdfDocument",
        data: JSON.stringify({ id: id,RID:RID, HandOverID: HandOverID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            if (response.Result == "Success") {
                var PdfPath = response.Message;
                if (PdfPath != "") {
                    window.open(PdfPath);
                    $('#LoadingPanel1').hide();
                } else {
                    var Response = { Result: "Failed", Message: "Pdf document not found." }
                    CustomAlert(Response);
                }
            }
        }
    });

}
function btnEmailClick(HandOverID) {
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
        url: "/HandOver/btnEmailClick",
        data: JSON.stringify({ id: id, UID: UID, UserRole: UserRole, HandOverID: HandOverID, RID: RID, DomainName: DomainName }),
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
                $('#hdnHandOverID').val(HandOverID);
                $('#emailConfirm').css('display', 'block');

            } else {
                var Response = { Result: "Failed", Message: "Please Sand Email again. " };
                CustomAlert(Response);
                $('#LoadingPanel1').hide();
            }
            if ((txtClientEmail == null || txtClientEmail == "") ) {
                $('#alertlabel2').css("display", "block");
                $('#alertlabel2').html('<Strong style="color:Red">To Auto Fill The Email Address Please Fill Hand Over & Inspection Distribution List On The Company Profile Screen </Strong>')

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
    var hdnHandOverID = $('#hdnHandOverID').val();
    var txttempfile = $('#txttempfile').val();

    $('#txtClientEmail').css("border", "");

    var emailRegex = /^([^\s@]+@[^\s@]+\.[^\s@]+)(\s*[;,]\s*[^\s@]+@[^\s@]+\.[^\s@]+)*$/;

    if (txtClientEmail === "" || !emailRegex.test(txtClientEmail)) {
        $('#txtClientEmail').css("border", "1px solid red");
        var Response = { Result: "Failed", Message: "Please enter valid Email addresses (e.g., a@b.com; c@d.com)." }
        CustomAlert(Response);
        $('#LoadingPanel1').hide();
        return;
    }



    $.ajax({
        type: "POST",
        url: "/HandOver/btnSend_Click",
        data: JSON.stringify({ txtClientEmail: txtClientEmail, txtcc: txtcc, txtsubject: txtsubject, txtEmailMSg: txtEmailMSg, txtFile: txtFile, HandOverID: hdnHandOverID, txttempfile: txttempfile }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            if (response.Result == "Success") {
                CustomAlert(response);
                $('#LoadingPanel1').hide();
                $('#emailConfirm').css('display', 'none');

            } else {
                CustomAlert(response);
                $('#LoadingPanel1').hide();
                $('#emailConfirm').css('display', 'none');
            }
        }
    });
}
function btnCancel_Click() {
    var txttempfile = $('#txttempfile').val();
    var txtFile = $('#txtFile').val();
    $.ajax({
        type: "POST",
        url: "/HandOver/btnCancel_Click",
        data: JSON.stringify({ txttempfile: txttempfile,txtFile:txtFile }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {

        }
    });
   
    $('#emailConfirm').css('display', 'none');
}
function ShowDeletePopup(HandOverID) {
    if (IsSiteStatus == "InActive") {
        CustomAlert({
            Result: "Failed", Message: "This site is InActive. This Action is not allowed."
        });
        return false;
    }
    $('#hdnHandOverID').val(HandOverID);
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
    var hdnHandOverID = $('#hdnHandOverID').val();
    $.ajax({
        type: "POST",
        url: "/HandOver/btndeleteyes_Click",
        data: JSON.stringify({ hdnHandOverID: hdnHandOverID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            if (response.Result == "Success") {
                $('#divdelete').css('display', 'none');
                CustomAlert(response);
                GetHandOverListData();
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

function btnadddocs_Click() {
    if (IsSiteStatus == "InActive") {
        CustomAlert({
            Result: "Failed", Message: "This site is InActive. This Action is not allowed."
        });
        return false;
    }
    $('#LoadingPanel1').show();
    var id=getParameterByName('id');
    var UID=getParameterByName('UID');
    var UserRole=getParameterByName('UserRole');
    var RID=getParameterByName('RID');
    var hdnTimeOffSet = $('#hdnTimeOffSet').val();
    var DomainName = window.location.origin;
    $.ajax({
        type: "POST",
        url: "/HandOver/btnadddocs_Click",
        data: JSON.stringify({ id: id,UID:UID,UserRole:UserRole,RID:RID,hdnTimeOffSet:hdnTimeOffSet,DomainName:DomainName }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            if(response.Result=="Success"){
                var Path = response.Message;
                //  window.location.href = '/HandOverForm/HandOverForm?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID + '&HandOverId=' + HandOverID;
                window.location.href = Path;
              } else{
                CustomAlert(response);
                $('#LoadingPanel1').hide();
               
            }
        }
    })
}

//Added on 26Feb2025
function ShowUnconfirmPopup(HandOverID) {
    if (IsSiteStatus == "InActive") {
        CustomAlert({
            Result: "Failed", Message: "This site is InActive. This Action is not allowed."
        });
        return false;
    }
    $('#hdnHandOverID').val(HandOverID);
    $('#divUnconfirm').css('display', 'block');
}
function btnUnconfirmyes_Click() {
    if (IsSiteStatus == "InActive") {
        CustomAlert({
            Result: "Failed", Message: "This site is InActive. This Action is not allowed."
        });
        return false;
    }
    $('#LoadingPanel1').show();
    var hdnHandOverID = $('#hdnHandOverID').val();
    $.ajax({
        type: "POST",
        url: "/HandOver/btnUnconfirmyes_Click",
        data: JSON.stringify({ hdnHandOverID: hdnHandOverID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            if (response.Result == "Success") {
                $('#divUnconfirm').css('display', 'none');
                CustomAlert(response);
                GetHandOverListData();
            } else {
                $('#divUnconfirm').css('display', 'none');
                CustomAlert(response);
                $('#LoadingPanel1').hide();
            }
        }
    })
}

function btnUnconfirmno_Click() {
    if (IsSiteStatus == "InActive") {
        CustomAlert({
            Result: "Failed", Message: "This site is InActive. This Action is not allowed."
        });
        return false;
    }
    $('#divUnconfirm').css('display', 'none');
}