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

            $('#ddlInspectedBy').empty().append(ddlInspectedBy);
           
        }
    });
}

function ddlInspectedBy_OnChange() {
    var id = getParameterByName('id');
    var InspectedByID = $('#ddlInspectedBy').val();
    $.ajax({
        url: '/InspectionForm/ddlInspectedBy_OnChange',
        async: false,
        data: { id: id, InspectedByID: InspectedByID },
        success: function (response) {
            var LicenceNo = response;
            $('#lbllicence').text(LicenceNo);
        }
    });
}

function showData() {
    $('#LoadingPanel1').show();
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var RID = getParameterByName('RID');
    var CheckListID = getParameterByName('CheckListID');
    var InspID = getParameterByName('InspID');
    var hdnTimeOffSet = $('#hdnTimeOffSet').val();
    $.ajax({
        type: "POST",
        url: '/InspectionForm/showData',
        data: JSON.stringify({ id: id, UID: UID, RID: RID, InspID: InspID, CheckListID: CheckListID,hdnTimeOffSet:hdnTimeOffSet}),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {

            var GetData = response.InspectionHeaderData;

            var InspectionTime = GetData.InspectionTime == null ? "" : GetData.InspectionTime;
            var InspectionDate = GetData.InspectionDate == null ? "" : GetData.InspectionDate;
            var SiteName = GetData.SiteName == null ? "" : GetData.SiteName;
            var OwnerName = GetData.OwnerName == null ? "" : GetData.OwnerName;
            var ScaffRef = GetData.ScaffRef == null ? "" : GetData.ScaffRef;
            var DesignType = GetData.DesignType == null ? "" : GetData.DesignType;
            var InspectionStatus = GetData.InspStatus == null ? "" : GetData.InspStatus;
            var InspComments = GetData.InspComments == null ? "" : GetData.InspComments;
            var Acknowledgment = GetData.Acknowledgment == null ? "" : GetData.Acknowledgment;
            var PhotoAngle = GetData.PhotoAngle == null ? "" : GetData.PhotoAngle;
            var Inspimage = GetData.Inspimage == null ? "" : GetData.Inspimage;
            var InspSign = GetData.InspSign == null ? "" : GetData.InspSign;
            var AckDate = GetData.AckDate == null ? "" : GetData.AckDate;
            var AckDateTime = GetData.AckDateTime == null ? "" : GetData.AckDateTime;
            var InspectedById = GetData.InspectedById == null ? "" : GetData.InspectedById;
            var LicenceNo = GetData.LicenceNo == null ? "" : GetData.LicenceNo;
            var inspstcode = GetData.CompleteInsp == null ? "" : GetData.CompleteInsp;
            var Inspimage2 = GetData.Inspimage2 == null ? "" : GetData.Inspimage2;
            var PhotoAngle1 = GetData.PhotoAngle1 == null ? "" : GetData.PhotoAngle1;

            if (InspectionTime == 0) {
                InspectionDate = DateFormat(InspectionDate);
            } else {
                InspectionDate = DateTimeFormat(InspectionDate);
            }

            if (AckDateTime == 0) {
                AckDate = DateFormat(AckDate);
            } else {
                AckDate = DateTimeFormat(AckDate);
            }


            $('#lblInspectedDate').text(InspectionDate);
            $('#lblSite').text(SiteName);
            $('#lblOwnerName').text(OwnerName);
            $('#lbltag').text(ScaffRef);
            $('#lbldesigntype').text(DesignType);
            $('#comment').val(InspComments);
            if(Acknowledgment=="true"){
                $('#confirmcheck').prop('checked',true);
            }
            if(PhotoAngle!=""){
                $('#imgangle').val(PhotoAngle);
                ImageRotate(PhotoAngle);
            }else{
                $('#imgangle').val('0');
            }
            if (PhotoAngle1 != "") {
                $('#imgangle1').val(PhotoAngle1);
                ImageRotate1(PhotoAngle1);
            } else {
                $('#imgangle1').val('0');
            }
            if(Inspimage!=""){
                $('#imga').attr("src", Inspimage);
                $('#imga').attr("src", Inspimage);

               
                SafariCheck(" + 1 + ",null,null);
                $('#btnchngeimg').css('display', 'block');
                $('#btnchngg').css('display', 'block');
                $('#Photoblock').css('display', 'none');
                $('#Imageblock').css('display','block');
                $('#getCamera').css('display', 'none');
            }

            if (Inspimage2 != "") {
                $('#imganew').attr("src", Inspimage2);
                $('#imganew').attr("src", Inspimage2);


                SafariCheck(" + 1 + ", null, null);
                $('#btnchngeimg1').css('display', 'block');
                $('#btnchngg1').css('display', 'block');
                $('#PhotoblockNew').css('display', 'none');
                $('#ImageblockNew').css('display', 'block');
                $('#getCameraNew').css('display', 'none');
            }
           
            if(InspSign!=""){
                 $('#signimga').attr("src", InspSign);
                $('#printcanvas').css('display','none');
                $('#hdnSignImageBlock').css('display','block');
                HideSignatureBlock();
            }
            if(AckDate!=""){
                $('#rdpInspectiondate').val(AckDate);
            }
            $('#ddlInspectedBy').val(InspectedById);
            $('#lbllicence').text(LicenceNo);
            
            if (((InspectionStatus == "Confirmed") || (InspectionStatus == "Out of Service")) && inspstcode == true) {
                $('#btnSave').css('display', 'none');
                $('#btnConfirm').css('display', 'none');
                $('#lockimg').css('display', 'block');
                $('#getCamera').css('display', 'none');
                $('#getCameraNew').css('display', 'none');
                $('#getAudio').css('display', 'block');
                $('#getVideo').css('display', 'block');
                $('#comment').attr('disabled', true);
                $('#confirmcheck').attr('disabled', true);
                $('#rdpInspectiondate').attr('disabled', true);
                $('#ddlInspectedBy').attr('disabled', true);
                
            }

            var InspectionFormData = response.InspectionFormData;

            $('#PlaceHolder1').empty().append(InspectionFormData);


            ////Making Only one checkbox checked for a particular Question
            $('input[type="checkbox"]').on('change', function () {
                $('input[name="' + this.name + '"]').not(this).prop('checked', false);
            });


            $('#LoadingPanel1').hide();
        }
    });
}
function UploadImagesSignature() {
    //   $('#LoadingPanel1').show();

    var fileData = new FormData();
    var files;

    //// Checking whether FormData is available in browser
    //if (window.FormData !== undefined) {
    //    if (files == null || files == "") {
    //        var fileUpload = $('#Image2').get(0);
    //        var test = $('#Image2').get(0);
    //        files = fileUpload.files;
    //    }
    //    var fileUpload = $('#Image2').get(0);
    //    // Create FormData object
    //    //  var fileData = new FormData();
    //    if (files.length != 0) {

    //        var FileName = files[0].name;
    //        var FileSize = files[0].size;
    //        var extension = FileName.substring(FileName.lastIndexOf('.') + 1).toLowerCase();
    //        fileData.append(files[0].name, files[0]);

    //    }
    //}

    var InspID = getParameterByName('InspID');
    var tempimg = $('#tempimg').val();
    var tempimg2 = $('#tempimg2').val();
  
    fileData.append('InspID', InspID);
    fileData.append('tempimg', tempimg);
    fileData.append('tempimg2', tempimg2);
    // fileData.append('image', image);
    $.ajax({
        url: "/InspectionForm/UploadImagesSignature",
        type: 'POST',
        contentType: false, // Not to set any content header
        processData: false, // Not to process data
        data: fileData,
        async:false,
        success: function (response) {
            var Result = response;
        }
    });
}
function UploadImagesSignature1() {
 
    var fileData = new FormData();
    var files;
    var InspID = getParameterByName('InspID');
    var tempimg2 = $('#tempimg2').val();

    fileData.append('InspID', InspID);
    fileData.append('tempimg2', tempimg2);
    $.ajax({
        url: "/InspectionForm/UploadImagesSignature1",
        type: 'POST',
        contentType: false, 
        processData: false, 
        data: fileData,
        async: false,
        success: function (response) {
            var Result = response;
        }
    });
}

function Save_Click() {
    $('#LoadingPanel1').show();
    UploadImagesSignature1();
    setTimeout(function () {
        UploadImagesSignature();
        Upload("Save");
    }, 1000);
}
function Upload(Check) {
    $('#LoadingPanel1').show();

    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var RID = getParameterByName('RID');
    var CheckListID = getParameterByName('CheckListID');
    var InspID = getParameterByName('InspID');
    var hdnlatitude = $('#hdnlatitude').val();
    var hdnlongitude = $('#hdnlongitude').val();
    var hdnAddress = $('#hdnAddress').val();
    var sketch_data = $('#sketch_data').val();
    var tempAud = $('#tempAud').val();
    var tempVideo = $('#tempVideo').val();
    var map_image = $('#map_image').val();
    var imgangle = $('#imgangle').val();
    var imgangle1 = $('#imgangle1').val();
    var ddlInspectedBy = $('#ddlInspectedBy').val();
    var lblInspectionLicence = $('#lbllicence').text();
    var confirmcheck = $('#confirmcheck').val();
    var txtcomment=$('#comment').val();
    var InspectionQuestionDataArr = [];
    
    $("#PlaceHolder1 .InspectionForm").each(function () {
        var checkboxid = $(this).find('input[type=checkbox]').attr('id');
        var ContentID = checkboxid.split('~')[1];
        var checkboxYes = $(this).find('.checkboxYes').prop('checked');
        var checkboxNo = $(this).find('.checkboxNo').prop('checked');
        var checkboxNA = $(this).find('.checkboxNA').prop('checked');
        var Action = $(this).find('.inputTypeActionRequired').val();
         
        var ChkBoxYes = 0;
        var ChkBoxNo = 0;
        var ChkBoxNA = 0;
        if (checkboxYes == true) {
            ChkBoxYes = 1;
        }
        if (checkboxNo == true) {
            ChkBoxNo = 1;
        } 
        if (checkboxNA == true) {
            ChkBoxNA = 1;
        }

        var ArryVal = {ContentID:ContentID, ChkBoxYes:ChkBoxYes, ChkBoxNo:ChkBoxNo, ChkBoxNA:ChkBoxNA, Action:Action};
       InspectionQuestionDataArr.push(ArryVal);
       });
    

    $.ajax({
       type:'post',
        url: '/InspectionForm/Upload',
        data: JSON.stringify({ id: id, UID: UID, RID: RID, InspID: InspID, CheckListID: CheckListID, hdnlatitude: hdnlatitude, hdnlongitude: hdnlongitude, hdnAddress: hdnAddress, sketch_data: sketch_data, tempAud: tempAud, tempVideo: tempVideo, map_image: map_image, imgangle: imgangle, imgangle1: imgangle1, ddlInspectedBy: ddlInspectedBy, lblInspectionLicence: lblInspectionLicence, confirmcheck: confirmcheck,txtcomment:txtcomment, InsFormModel: InspectionQuestionDataArr }),

        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            if (response.Result == "Success") {

                if (Check == "Save") {
                    $('#saveContinueModal').css("display", "block");
                    $('#LoadingPanel1').hide();
                } else {
                }
                
            } else {
                CustomAlert(response);
                $('#LoadingPanel1').hide();
            }
        }
    //    , beforeSend: function () {
    //        $("#LoadingPanel1").show();
    //},
    //complete: function () {
    //    $("#LoadingPanel1").hide();
    //}
    });
}

function btnsavecontinue_Click()
{
    $('#LoadingPanel1').show();
    $('#saveContinueModal').css("display", "none");
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var RID = getParameterByName('RID');
    var CheckListID = getParameterByName('CheckListID');
    var InspID = getParameterByName('InspID');
    var FormPage = getParameterByName('FormPage');
    
    var EncriptData = Encrypt(["Email"]);
    var From = EncriptData[0];
    if (FormPage!=null){
        window.location.href = '/InspectionForm/InspectionForm?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID + '&CheckListID=' + CheckListID + '&InspID=' + InspID + '&FormPage=' + From;
    } else {
        window.location.href = '/InspectionForm/InspectionForm?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID + '&CheckListID=' + CheckListID + '&InspID=' + InspID;
    }
//if (databaseCon.Decrypt(Request.QueryString["FormPage"]) != "")
//{
//    Response.Redirect("InspectionForm?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RID=" + Request.QueryString["RID"] + "&CheckListID=" + Request.QueryString["CheckListID"] + "&InspID=" + Request.QueryString["InspID"] + "&FormPage=" + databaseCon.Encrypt("Email"));
//}
//else
//{
//    Response.Redirect("InspectionForm?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RID=" + Request.QueryString["RID"] + "&CheckListID=" + Request.QueryString["CheckListID"] + "&InspID=" + Request.QueryString["InspID"] + "&FormPage=" + "");
//}
}

function btnConfirm_Click()
{
// ScriptManager.RegisterStartupScript(this, this.GetType(), "temp102", "GetCurrentDate();", true);
//var date = hdnCurrentDate.Value;
    //if (confirmcheck.Checked == true)

    if ($('#rdpInspectiondate').val() != "") {
        if ($('#confirmcheck').prop('checked') == true) {
            var sketch_data = $('#sketch_data').val();
            var signimga= $('#signimga').attr('src');
            if (sketch_data != "" || signimga !="") {
              
                // divdelete.Style.Add("display", "block");
            $('#divdelete').css("display", "block");
            // ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "InspStatusCheck();", true);
            } else {
                var Response = { Result: "Failed", Message: "Please provide Inspector signature ." };
                CustomAlert(Response);
            }
        }
        else {
            // div2.Style.Add("display", "block");
            $('#div2').css("display", "block");
        }
    }
    else {
        var Response = { Result: "Failed", Message: "Please Select Acknowledgment Date." };
        CustomAlert(Response);
    }
}
function submit_Click()
    {
       // ViewState["hdnCurrentDate"] = Request.Form["hdnCurrentDate"];

      //  string inspstatusCheck = Request.Form["inspstatusCheck"];
      //  ConfirmStatus = inspstatusCheck;
    //  string InspectionStatusModal = "";

    var inspstatusCheck="";
       if ($('#chckcnfrm').prop('checked') == true) {
        inspstatusCheck="Confirmed";
        }
       if ($('#outServiceCheck').prop('checked') == true) {
        inspstatusCheck = "Out of Service";
        }
        if (inspstatusCheck != "")
        {
             $('#divdelete').css("display", "none");
             $('#confirmModal').css("display", "block");

          //  ViewState["InspectionStatus"] = inspstatusCheck;
        }

        $('#divdelete').css("display", "none");
}

function btnConfirmok_Click()
{
    $('#LoadingPanel1').show();
    UploadImagesSignature1();
    setTimeout(function () {
   
    UploadImagesSignature();


    Upload("Confirm");
 
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var RID = getParameterByName('RID');
    var CheckListID = getParameterByName('CheckListID');
    var InspID = getParameterByName('InspID');
    var hdnTimeOffSet = $('#hdnTimeOffSet').val();
    var rdpInspectiondate = $('#rdpInspectiondate').val();

    var inspstatusCheck = "";
    if ($('#chckcnfrm').prop('checked') == true) {
        inspstatusCheck = "Confirmed";
    }
    if ($('#outServiceCheck').prop('checked') == true) {
        inspstatusCheck = "Out of Service";
    }
      $.ajax({
       type:'post',
       url: '/InspectionForm/btnConfirmok_Click',
       data: JSON.stringify({ id: id, InspID: InspID, UID: UID, hdnTimeOffSet: hdnTimeOffSet, rdpInspectiondate: rdpInspectiondate, InspectionStatus: inspstatusCheck }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            var getData = response;
            if (response.Result == "Success") {
                $('#lblInspectionstatusmodal').text(inspstatusCheck);
                $('#confirmModal').css("display", "none");
                $('#confirmContinueModal').css("display", "block");
                //lblInspectionstatusmodal.InnerText = InspectionStatusModal;
                //confirmModal.Style.Add("display", "none");
                //confirmContinueModal.Style.Add("display", "block");
                $('#LoadingPanel1').hide();
            } else {
                CustomAlert(response);
                $('#LoadingPanel1').hide();
            }
        }
    });
      $('#divdelete').css("display", "none");
    }, 1000);
}


function btnconfirmContinue_Click()
{

  $('#LoadingPanel1').show();
$('#confirmContinueModal').css("display", "none");
var id = getParameterByName('id');
var UID = getParameterByName('UID');
var UserRole = getParameterByName('UserRole');
var RID = getParameterByName('RID');
var CheckListID = getParameterByName('CheckListID');
var InspID = getParameterByName('InspID');
var FormPage = getParameterByName('FormPage');
    
var EncriptData = Encrypt(["Email"]);
var From = EncriptData[0];
if (FormPage!=null){
    window.location.href = '/InspectionForm/InspectionForm?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID + '&CheckListID=' + CheckListID + '&InspID=' + InspID + '&FormPage=' + From;
} else {
    window.location.href = '/InspectionForm/InspectionForm?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID + '&CheckListID=' + CheckListID + '&InspID=' + InspID;
}
        confirmContinueModal.Style.Add("display", "none");
//if (databaseCon.Decrypt(Request.QueryString["FormPage"]) != "")
//{
//    Response.Redirect("InspectionForm?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RID=" + Request.QueryString["RID"] + "&CheckListID=" + Request.QueryString["CheckListID"] + "&InspID=" + Request.QueryString["InspID"] + "&FormPage=" + databaseCon.Encrypt("Email"));
//}
//else
//{
//    Response.Redirect("InspectionForm?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RID=" + Request.QueryString["RID"] + "&CheckListID=" + Request.QueryString["CheckListID"] + "&InspID=" + Request.QueryString["InspID"] + "&FormPage=" + "");
            
//}

}

function btnConfirmCancel_Click()
{
    $('#confirmModal').css("display", "none");
}
function cancel_Click()
{
    $('#divdelete').css("display", "none");
}
function ok_Click()
{
    $('#div2').css("display", "none");
}
function btnBacktoInspection_Click()
{
    $('#LoadingPanel1').show();
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var RID = getParameterByName('RID');

    window.location.href = '/InspectionList/InspectionList?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID;
   // Response.Redirect(DomainName + "/" + "InspectionList?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RID=" + Request.QueryString["RID"], false);
}
 function btnBacktoRegister_ServerClick()
{
     $('#LoadingPanel1').show();
     var id = getParameterByName('id');
     var UID = getParameterByName('UID');
     var UserRole = getParameterByName('UserRole');

      window.location.href ='/RegisterList/RegisterList?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
   
    }