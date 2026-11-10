
$(function () {
   $('#hdnTimeOffSet').val(new Date().getTimezoneOffset());
         
    BindallDropdown();
    GetHandOverFormData();
});

function BindallDropdown() {
    var id = getParameterByName('id');
    $.ajax({
        url: '/HandOverForm/BindallDropdown',
        async: false,
        data: { id: id },
        success: function (response) {

            data = response;
            var ddlsiteManager = "";
           // var ddlsiteManager = "<option value='' disabled selected='selected'>select---</option>";
            $.each(response.ddlsiteManager, function (idx, obj) {
                if (obj != null) {
                    ddlsiteManager += '<option value="' + obj.CRId + '">' + obj.Position + '</option>';
                }
            });

            var ddlContrRepresentative = "<option value='' disabled selected='selected'>select---</option>";
            $.each(response.ddlContractorRepresentative, function (idx, obj) {
                if (obj != null) {
                    ddlContrRepresentative += '<option value="' + obj.UserId + '">' + obj.name + '</option>';
                }
            });

            $('#ddlsiteManager').empty().append(ddlsiteManager);
            $('#ddlContractorRepresentative').empty().append(ddlContrRepresentative);
          
        }
    });
}

function GetHandOverFormData() {
    $('#LoadingPanel1').show();
    $('#lockimg').css('display', 'none');
    var id = getParameterByName('id');
    var RID = getParameterByName('RID');
    var HandOverId = getParameterByName('HandOverId');
    var hdnTimeOffSet = $('#hdnTimeOffSet').val();
    $.ajax({
        type: "POST",
        url: "/HandOverForm/GetHandOverFormData",
        data: JSON.stringify({ id: id, RID: RID, HandOverId: HandOverId, hdnTimeOffSet: hdnTimeOffSet }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            var GetData = response;
            if (GetData != "") {

                var CurrentDate=GetData.CurrentDate==null ? "" :GetData.CurrentDate;
                var IsConfirmed=GetData.IsConfirmed==null ? "" :GetData.IsConfirmed;
                var CertificateNo=GetData.CertificateNo==null ? "" :GetData.CertificateNo;
                var HandOverDate=GetData.HandOverDate==null ? "" :GetData.HandOverDate;
                var HandOverTime=GetData.HandOverTime==null ? "" :GetData.HandOverTime;
                var HasEncapsulated=GetData.HasEncapsulated==null ? "" :GetData.HasEncapsulated;
                var EngineeredDesign=GetData.EngineeredDesign==null ? "" :GetData.EngineeredDesign;
                var DesignDrawingNo=GetData.DesignDrawingNo==null ? "" :GetData.DesignDrawingNo;
                var HandOverComments=GetData.HandOverComments==null ? "" :GetData.HandOverComments;
                var NumberOfPlatform=GetData.NumberOfPlatform==null ? "" :GetData.NumberOfPlatform;
                var ReasonForAccess=GetData.ReasonForAccess==null ? "" :GetData.ReasonForAccess;
                var Length=GetData.Length==null ? "" :GetData.Length;
                var Width = GetData.Width == null ? "" : GetData.Width;
                var Height = GetData.Height == null ? "" : GetData.Height;
                var RepresentativeName=GetData.RepresentativeName==null ? "" :GetData.RepresentativeName;
                var HandOverDesc=GetData.HandOverDesc==null ? "" :GetData.HandOverDesc;
                var ScaffoldTag=GetData.ScaffoldTag==null ? "" :GetData.ScaffoldTag;
                var SiteLocation=GetData.SiteLocation==null ? "" :GetData.SiteLocation;
                var BuildingNo=GetData.BuildingNo==null ? "" :GetData.BuildingNo;
                var Location = GetData.Location == null ? "" : GetData.Location;
                var NoOfWorkingDecks = GetData.NoOfWorkingDecks == null ? "" : GetData.NoOfWorkingDecks;
                var OwnerName = GetData.OwnerName == null ? "" : GetData.OwnerName;
                var FloorLevel=GetData.FloorLevel==null ? "" :GetData.FloorLevel;
                var DutyRating=GetData.DutyRating==null ? "" :GetData.DutyRating;
                var ContractorRepName = GetData.ContractorRepName == null ? "" : GetData.ContractorRepName;
                var ContractorRepLicence = GetData.ContractorRepLicence == null ? "" : GetData.ContractorRepLicence;
                var ContractorRepPosition = GetData.ContractorRepPosition == null ? "" : GetData.ContractorRepPosition;
                var HandOverLat = GetData.HandOverLat == null ? "" : GetData.HandOverLat;
                var HandOverLong = GetData.HandOverLong == null ? "" : GetData.HandOverLong;
                var CRId = GetData.CRId == null ? "" : GetData.CRId;
                var PhotoAngle = GetData.PhotoAngle == null ? "" : GetData.PhotoAngle;
                var HandOverAcknowledgment = GetData.HandOverAcknowledgment == null ? "" : GetData.HandOverAcknowledgment;

                if (HandOverTime == 0) {
                    HandOverDate = DateFormat(HandOverDate);
                } else {
                    HandOverDate = DateTimeFormat(HandOverDate);
                }

            


                $('#Label2').text(CurrentDate);
                $('#lblCertificateNo').text(CertificateNo);
                $('#lbldesigndNo').text(DesignDrawingNo);
                $('#comment').text(HandOverComments);
                $('#lblNumberOfPlatform').text(NumberOfPlatform);
                $('#lblReasonforAccess').text(ReasonForAccess);
                if (Length != "") {
                    $('#lblLength').text(Length.toFixed(2));
                } if (Width!="") {
                    $('#lblWidth').text(Width.toFixed(2));
                } if (Height != "") {
                 $('#lblHeight').text(Height.toFixed(2));
                }
                $('#txtsiteManager').val(RepresentativeName);
                $('#handovrDesc').val(HandOverDesc);
                $('#scaffreflbl').text(ScaffoldTag);
                $('#sitelbl').text(SiteLocation);
                $('#BuildingNolbl').text(BuildingNo);
                $('#Locationlbl').text(Location);
                $('#lblWrokingDeck').text(NoOfWorkingDecks);
                $('#lblOwnerName').text(OwnerName);
                $('#lblfloor').text(FloorLevel);
                $('#lblDutyRating').text(DutyRating);
                $('#ddlContractorRepresentative').val(ContractorRepName);
                //if (ContractorRepName != "") {
                //    $("#ddlContractorRepresentative option:selected").text(ContractorRepName);
                //}
                $('#lbllicence').text(ContractorRepLicence);
                $('#lblrepresentPost').text(ContractorRepPosition);

                if (HandOverLat!= "" && HandOverLong != "")
                {
                    GetcurrentLocation(""+HandOverLat+ "","" + HandOverLong+ "");
                    // ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "GetcurrentLocation(" + dt.Rows[0]["HandOverLat"].ToString() + "," + dt.Rows[0]["HandOverLong"].ToString() + ");", true);
                }
                if(CRId!=""){
                    $('#ddlsiteManager').val(CRId);
                }

                if (PhotoAngle!= "")
                {
                    $('#imgangle').val(PhotoAngle);
                    // imgangle.Value = dt.Rows[0]["PhotoAngle"].ToString();
                    // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp1", "<script>ImageRotate(" + dt.Rows[0]["PhotoAngle"].ToString() + ");</script>", false);
                    ImageRotate("" +PhotoAngle+"");
                }
                else
                {
                    // imgangle.Value = "0";
                    $('#imgangle').val('0');
                }
                if (IsConfirmed!= "")
                {
                    if(IsConfirmed==true){
                        RemoveSignatureBlock();
                        $('#btnSave').css("display", "none");
                        $('#btnConfirm').css("display", "none");
                        $('#lockimg').css('display', 'block');
                    }
                }
                if(HandOverDate!=""){
                    if (IsConfirmed==true)
                    {
                        $('#rdpInspectiondate').val(HandOverDate);
                        $('#rdpInspectiondate').attr('disabled', true);
                    }else{
                        $('#rdpInspectiondate').val(HandOverDate);
                    }
                }
                if(HasEncapsulated!=""){
                
                    if (HasEncapsulated == "YES")
                    {
                        //    checkEncapsulateY.Checked = true;
                        //    checkEncapsulateN.Checked = false;
                        //    checkEncapsulateNA.Checked = false;
                        $('#checkEncapsulateY').prop('checked',true);
                        $('#checkEncapsulateN').prop('checked',false);
                        $('#checkEncapsulateNA').prop('checked',false);
                    }
                    else if (HasEncapsulated == "NO")
                    {
                        //checkEncapsulateY.Checked = false;
                        //checkEncapsulateN.Checked = true;
                        //checkEncapsulateNA.Checked = false;
                        $('#checkEncapsulateY').prop('checked',false);
                        $('#checkEncapsulateN').prop('checked',true);
                        $('#checkEncapsulateNA').prop('checked',false);
                    }
                    else if (HasEncapsulated == "NA")
                    {
                        //checkEncapsulateY.Checked = false;
                        //checkEncapsulateN.Checked = false;
                        //checkEncapsulateNA.Checked = true;
                        $('#checkEncapsulateY').prop('checked',false);
                        $('#checkEncapsulateN').prop('checked',false);
                        $('#checkEncapsulateNA').prop('checked',true);
                    }
                }
                if(EngineeredDesign!=""){
                    if (EngineeredDesign.toUpperCase() == "YES")
                    {
                        //checkBoxDesignY.Checked = true;
                        //checkBoxDesignN.Checked = false;
                        //checkBoxDesignNA.Checked = false;
                        $('#checkBoxDesignY').prop('checked',true);
                        $('#checkBoxDesignN').prop('checked',false);
                        $('#checkBoxDesignNA').prop('checked',false);
                    }
                    else if (EngineeredDesign.toUpperCase() == "NO")
                    {
                        //checkBoxDesignY.Checked = false;
                        //checkBoxDesignN.Checked = true;
                        //checkBoxDesignNA.Checked = false;
                        $('#checkBoxDesignY').prop('checked',false);
                        $('#checkBoxDesignN').prop('checked',true);
                        $('#checkBoxDesignNA').prop('checked',false);
                    }
                    else if (EngineeredDesign.toUpperCase() == "N/A")
                    {
                        //checkBoxDesignY.Checked = false;
                        //checkBoxDesignN.Checked = false;
                        //checkBoxDesignNA.Checked = true;
                        $('#checkBoxDesignY').prop('checked',false);
                        $('#checkBoxDesignN').prop('checked',false);
                        $('#checkBoxDesignNA').prop('checked',true);
                    }
                }else{
                    //checkBoxDesignY.Checked = false;
                    //checkBoxDesignN.Checked = true;
                    //checkBoxDesignNA.Checked = false;
                    $('#checkBoxDesignY').prop('checked',false);
                    $('#checkBoxDesignN').prop('checked',true);
                    $('#checkBoxDesignNA').prop('checked',false);
                }

                if(HandOverAcknowledgment!=""){
                    if(HandOverAcknowledgment=="True"){
                        $('#confirmcheck').prop('checked',true);
                    }else{
                        $('#confirmcheck').prop('checked',false);
                    }
                    
                }


                var HandOverImage = GetData.HandOverImage == null ? "" : GetData.HandOverImage;
                var HandOverAudio = GetData.HandOverAudio == null ? "" : GetData.HandOverAudio;
                var HandOverVedio = GetData.HandOverVedio == null ? "" : GetData.HandOverVedio;
                var ContractorSign = GetData.ContractorSign == null ? "" : GetData.ContractorSign;
                var RepresentativeSign = GetData.RepresentativeSign == null ? "" : GetData.RepresentativeSign;
                
                //HandOver Image Code Starts Here 
                if (HandOverImage!= "")
                {
                    $('#btnchngg').css('display','block');
                    $('#btnchngeimg').css('display','block');
                    $('#imga').attr('src',HandOverImage);
                    $('#Image3').attr('src',HandOverImage);
                    SafariCheck(1,null,null);
                    $('#Photoblock').css('display','none');
                    $('#Imageblock').css('display','block');
                    if (IsConfirmed== true)
                    {
                        $('#getCamera').css('display','none');
                        $('#mob_file_image').css('display','none');
                        $('#mob_file_image1').css('display','none');
                        $('.photoupload').css('display','none');
                    }
                }
                //HandOver Image Code Ends Here

                //HandOver Audio Code Starts Here 
                if (HandOverAudio!= "")
                {
                    $('#getAudioBlob').attr('src',HandOverAudio);
                    $('#hdnAudioUrl').attr('src',HandOverAudio);
                    SafariCheck(null,null,1);
                    GetAudiofromblob(0);
                    $('#AudioBlock').css('display','block');
                    if (IsConfirmed==true)
                    {
                        $('#getAudio').css('display','none');
                    }
                }
                //HandOver Audio Code Ends Here

                //HandOver Video Code Starts Here
                if (HandOverVedio != "")
                {
                    $('#dbrecorded').attr('src', HandOverVedio);
                    $('#hdnvideoUrl').attr('src', HandOverVedio);
                    SafariCheck(null,1,null);
                    $('#VideoBlock').css('display', 'block');
                    if (IsConfirmed==true)
                    {
                        $('#getVideo').css('display', 'none');
                    }
                    
                }
                //HandOver Video Code Ends Here


                //HandOver Signature 1 Code Starts Here
                if (ContractorSign!= "")
                {
                    $('#signimga').attr('src',ContractorSign);
                    $('#printcanvas').css('display', 'none');
                    $("#signature-pad").css("display", "none");
                    $('#hdnSignImageBlock2').css('display','block');
                }
                //HandOver Signature 1 Code Ends Here

                //HandOver Signature 2 Code Starts Here
                if (RepresentativeSign!= "")
                {
                    $('#Image1').attr('src',RepresentativeSign);
                    $('#printcanvas1').css('display', 'none');
                    $("#signature-pad1").css("display", "none");
                    $('#hdnSignImageBlock').css('display','block');
                }
                //HandOver Signature 2 Code Ends Here
                if (ContractorSign !== "") {
                    $('#confirmcheck').prop('disabled', true);
                }
                $('#LoadingPanel1').hide();
            }
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

    var HandOverId = getParameterByName('HandOverId');
    var tempimg = $('#tempimg').val();

    fileData.append('HandOverId', HandOverId);
    fileData.append('tempimg', tempimg);

    // fileData.append('image', image);
    $.ajax({
        url: "/HandOverForm/UploadImagesSignature",
        type: 'POST',
        contentType: false, // Not to set any content header
        processData: false, // Not to process data
        data: fileData,
        async: false,
        success: function (response) {
            var Result = response;
        }
    });
}

function HandOverUpload(valuescheck) {
    $('#LoadingPanel1').show();
    var id=getParameterByName('id');
    var UID=getParameterByName('UID');
    var HandOverId=getParameterByName('HandOverId');

    var hdnlatitude=$('#hdnlatitude').val();
    var hdnlongitude=$('#hdnlongitude').val();
    var hdnAddress=$('#hdnAddress').val();
    var sketch_data=$('#sketch_data').val();
    var sketch_data2=$('#sketch_data2').val();
    //var tempimg=$('#tempimg').val();
    var tempAud=$('#tempAud').val();
    var tempVideo=$('#tempVideo').val();
    var map_image=$('#map_image').val();
    var imgangle=$('#imgangle').val();
    var rdpInspectiondate=$('#rdpInspectiondate').val();
    var hdnTimeOffSet=$('#hdnTimeOffSet').val();
    var txtsiteManager=$('#txtsiteManager').val();
    var ddlsiteManager=$('#ddlsiteManager').val();
    var lblOwnerName=$('#lblOwnerName').text();
    var comment=$('#comment').val();
    var handovrDesc=$('#handovrDesc').val();
    var confirmcheck=$('#confirmcheck').prop('checked');
    var ddlContractorRepresentative=$('#ddlContractorRepresentative').val();
    var lbllicence=$('#lbllicence').text();
    var lblrepresentPost=$('#lblrepresentPost').text();
    var EncapsulateValue="";

    //if ($('#checkEncapsulateY').val() != "" && $('#checkEncapsulateY').val()!= null)
    if ($('#checkEncapsulateY').prop('checked') == true)
    {
        EncapsulateValue = "YES";
    }
   // else if ($('#checkEncapsulateN').val() != "" && $('#checkEncapsulateN').val() != null)
       else if ($('#checkEncapsulateN').prop('checked') == true)
    {
        EncapsulateValue = "NO";
    }
      //  else if ($('#checkEncapsulateNA').val() != "" && $('#checkEncapsulateNA').val() != null)
       else if ($('#checkEncapsulateNA').prop('checked') == true)
    {
        EncapsulateValue = "NA";
    }

    var arr = [];

    // arr = { id: id, UID: UID, HandOverId: HandOverId, hdnlatitude: hdnlatitude, hdnlongitude: hdnlongitude, hdnAddress: hdnAddress, sketch_data: sketch_data, sketch_data2: sketch_data2, tempimg: tempimg, tempAud: tempAud, tempVideo: tempVideo, map_image: map_image, imgangle: imgangle, rdpInspectiondate: rdpInspectiondate, hdnTimeOffSet: hdnTimeOffSet, txtsiteManager: txtsiteManager, ddlsiteManager: ddlsiteManager, lblOwnerName: lblOwnerName, comment: comment, handovrDesc: handovrDesc, confirmcheck: confirmcheck, ddlContractorRepresentative: ddlContractorRepresentative, lbllicence: lbllicence, lblrepresentPost: lblrepresentPost, EncapsulateValue: EncapsulateValue };
    arr = { id: id, UID: UID, HandOverId: HandOverId, hdnlatitude: hdnlatitude, hdnlongitude: hdnlongitude, hdnAddress: hdnAddress, sketch_data: sketch_data, sketch_data2: sketch_data2,tempAud: tempAud, tempVideo: tempVideo, map_image: map_image, imgangle: imgangle, rdpInspectiondate: rdpInspectiondate, hdnTimeOffSet: hdnTimeOffSet, txtsiteManager: txtsiteManager, ddlsiteManager: ddlsiteManager, lblOwnerName: lblOwnerName, comment: comment, handovrDesc: handovrDesc, confirmcheck: confirmcheck, ddlContractorRepresentative: ddlContractorRepresentative, lbllicence: lbllicence, lblrepresentPost: lblrepresentPost, EncapsulateValue: EncapsulateValue };

    $.ajax({
        type: "POST",
        url: "/HandOverForm/HandOverUpload",
        data: JSON.stringify({ model: arr }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            var GetData = response;
            if (response.Result == "Success") {
                if (valuescheck == "Save") {
                    $('#saveContinueModal').css('display', 'Block');
                    $("#LoadingPanel1").css("display", "none");
                } else {

                }
            } else {
                $("#LoadingPanel1").css("display", "none");
                CustomAlert(response);
            }
        }
    });

}

function btnSave_Click(){
    var planstanding = $("#lbllicence").text();
    var ddlContractorRepresentative = $('#ddlContractorRepresentative').val();
    if (planstanding == "" || ddlContractorRepresentative == null || ddlsiteManager == null) {
        //$('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
        //$('#art1').css("display", "block");
        //$('#alertlabel').html('<strong>Alert ! </strong>Please fill all required field then click on save.');
        //hideAlert();

        var Response = { Result: "Failed", Message: "Please fill all required field then click on save." };
        CustomAlert(Response);
    } else {
        $('#LoadingPanel1').show();
        setTimeout(function () {
            UploadImagesSignature();
            HandOverUpload("Save");
        }, 1000);
       
    }
}

function btnConfirm_ClickOk()
{
    $('#confirmModal').css("display", "none");
    $('#LoadingPanel1').show();
    setTimeout(function () {
    UploadImagesSignature();
    HandOverUpload("Confirm");
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var HandOverId = getParameterByName('HandOverId');
    var sitelbl=$('#sitelbl').text();
    var BuildingNolbl=$('#BuildingNolbl').text();
    var Locationlbl = $('#Locationlbl').text();
    var lblfloor=$('#lblfloor').text();
    var lblReasonforAccess=$('#lblReasonforAccess').text();
    var lblDutyRating=$('#lblDutyRating').text();
    var lblWrokingDeck=$('#lblWrokingDeck').text();
    var lblNumberOfPlatform=$('#lblNumberOfPlatform').text();
    var lbldesigndNo=$('#lbldesigndNo').text();
    var lblLength=$('#lblLength').text();
    var lblWidth=$('#lblWidth').text();
    var lblHeight=$('#lblHeight').text();
    var scaffreflbl = $('#scaffreflbl').text();
    var lblOwnerName = $('#lblOwnerName').text();
    var hdnTimeOffSet = $('#hdnTimeOffSet').val();
    var confirmcheck = $('#confirmcheck').prop('checked');
    var EncapsulateValue = "";

    if ($('#checkEncapsulateY').val() != "" && $('#checkEncapsulateY').val() != null) {
        EncapsulateValue = "YES";
    }
    else if ($('#checkEncapsulateN').val() != "" && $('#checkEncapsulateN').val() != null) {
        EncapsulateValue = "NO";
    }
    else if ($('#checkEncapsulateNA').val() != "" && $('#checkEncapsulateNA').val() != null) {
        EncapsulateValue = "NA";
    }

    var arr = [];

        arr = { id: id, UID: UID, HandOverId: HandOverId, sitelbl: sitelbl, BuildingNolbl: BuildingNolbl, Locationlbl: Locationlbl, lblfloor: lblfloor, lblReasonforAccess: lblReasonforAccess, lblDutyRating: lblDutyRating, lblWrokingDeck: lblWrokingDeck, lblNumberOfPlatform: lblNumberOfPlatform, lbldesigndNo: lbldesigndNo, lblLength: lblLength, lblWidth: lblWidth, lblHeight: lblHeight, scaffreflbl: scaffreflbl, lblOwnerName: lblOwnerName, hdnTimeOffSet:hdnTimeOffSet,EncapsulateValue: EncapsulateValue };

    $.ajax({
        type: "POST",
        url: "/HandOverForm/btnConfirm_ClickOk",
        data: JSON.stringify({ model: arr }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            var GetData = response;
            if (response.Result == "Success") {
                var id = getParameterByName('id');
                var UID = getParameterByName('UID');
                var UserRole = getParameterByName('UserRole');
                var RID = getParameterByName('RID');
                var HandOverId = getParameterByName('HandOverId');
                var InspID = getParameterByName('InspID');
                var FormPage = getParameterByName('FormPage');

                var EncriptData = Encrypt(["Email"]);
                var From = EncriptData[0];
                if (FormPage != null) {
                    window.location.href = '/HandOverForm/HandOverForm?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID + '&HandOverId=' + HandOverId + '&FormPage=' + From;
                } else {
                    window.location.href = '/HandOverForm/HandOverForm?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID + '&HandOverId=' + HandOverId;
                }
            } else {
                $('#LoadingPanel1').hide();
                CustomAlert(response);
            }
         }
    });
    }, 1000);
 }

function btnConfirm_Click()
{
    var planstanding = $("#lbllicence").text();
    var ddlContractorRepresentative = $('#ddlContractorRepresentative').val();
    var ddlsiteManager = $('#ddlsiteManager').val();

    if (planstanding == "" || ddlContractorRepresentative==null || ddlsiteManager==null) {
        //$('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
        //$('#art1').css("display", "block");
        //$('#alertlabel').html('<strong>Alert ! </strong>Please fill all required field then click on save.');
        //hideAlert();
        var Response = { Result: "Failed", Message: "Please fill all required field then click on save." };
        CustomAlert(Response);
    }else {
        if ($('#confirmcheck').prop('checked')== true)
        {
            $('#confirmModal').css('display','block');
        }
        else
        {
            $('#div2').css('display', 'block');
        }
    }
   
}
function btnsavecontinue_Click()
{
    $('#LoadingPanel1').show();
    $('#saveContinueModal').css("display", "none");
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var RID = getParameterByName('RID');
    var HandOverId = getParameterByName('HandOverId');
    var InspID = getParameterByName('InspID');
    var FormPage = getParameterByName('FormPage');

    var EncriptData = Encrypt(["Email"]);
    var From = EncriptData[0];
    if (FormPage != null) {
        window.location.href = '/HandOverForm/HandOverForm?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID + '&HandOverId=' + HandOverId +'&FormPage=' + From;
    } else {
        window.location.href = '/HandOverForm/HandOverForm?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID + '&HandOverId=' + HandOverId;
    }
}
function btnConfirmCancel_Click() {
    $('#confirmModal').css('display', 'none');
}
function ok_Click() {
    $('#div2').css('display', 'none');
}

function ddlContractorRepresentative_SelectedIndexChanged() {
    var ddlContractorRepresentative = $('#ddlContractorRepresentative').val();
    $.ajax({
        type: "POST",
        url: "/HandOverForm/ddlContractorRepresentative_SelectedIndexChanged",
        data: JSON.stringify({ ddlContractorRepresentative: ddlContractorRepresentative }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            var GetData = response;
            if (GetData != "") {
                var arr = GetData.split('_');
                lbllicence = arr[0];
                lblrepresentPost = arr[1];
                $('#lbllicence').text(lbllicence);
                $('#lblrepresentPost').text(lblrepresentPost);
            }
        }
    });
}

function btnBacktoInspection_Click() {
    $('#LoadingPanel1').show();
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var RID = getParameterByName('RID');
    var HandOverId = getParameterByName('HandOverId');
   
        window.location.href = '/HandOver/HandOver?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID ;
   
}

//function closeCurrentTab() {
//        window.close();
//}
function closeCurrentTab() {
    if (window.close()) {
        console.log("Window closed successfully.");
    } else {
        window.location.href = "/Login/Login";
    }
}
