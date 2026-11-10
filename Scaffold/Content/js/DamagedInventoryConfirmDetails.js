

    $(function () {
        DamageInvConfirDetail();
        showtImage();
        GetUplodadPdfData();

        $('input[type="file"]').change(function (e) {
            var output = document.getElementById('fileList');
            //output.innerHTML = 'File Selected:';
            output.innerHTML = '<ul>';

            for (var i = 0; i <= e.target.files.length; i++) {
                output.innerHTML += '<li>' + e.target.files.item(i).name + '</li>';
            }
            output.innerHTML += '</ul>';
        });


        //$('img').on('click', function () {
        //      //alert(this.id);
        //      var imageid = this.id;
        //      if (imageid != 'ppdl') {
        //          $('#ppdl').attr("src", this.src);
        //          $('#popup').css('display', 'block');
        //      }
        //      else {

        //      }
        //  });

        //  $('.close').on('click', function () {
        //      $('#popup').css('display', 'none');
        //  });



        $('li.dropdown.user').on("click", function () {
            $('li.dropdown.user').addClass('open');
        });

        $('#btndelete').on('click', function () {
            var parentids = $(this).parent();
            var siblingids = parentids.siblings();
            var imglink = siblingids[0].currentSrc;
            var splitting = imglink.split('siteimage/');
            var imgname = splitting[1];
            $('#hdnimgname').val(imgname);
            $('#divdelete').css('display', 'block');
        });

        var GetisSafari = navigator.vendor && navigator.vendor.indexOf('Apple') > -1 && navigator.userAgent && !navigator.userAgent.match('CriOS');
        if (GetisSafari == false) {
            $(".nonsafari").css("display", "block");
            $(".safari").css("display", "none");
            var isMobile = /iPhone|iPad|iPod|Android/i.test(navigator.userAgent);
            if (isMobile) {
                $(".mobileApp").css("display", "block");
                $(".desktopApp").css("display", "none");
            } else {
                $(".mobileApp").css("display", "none");
                $(".desktopApp").css("display", "block");
            }
        } else {
            $(".nonsafari").css("display", "none");
            $(".safari").css("display", "block");
        }


        $(document).on("change", ".mob_file_image", function (evt) {
            var base64mobImagedata;
            var reader = new FileReader();
            reader.readAsDataURL(this.files[0]);
            var name = this.files[0].name;
            $("#nameget").val(name);
            reader.onloadend = function () {
                base64mobImagedata = reader.result;
                //$("#Image1").attr('src', base64mobImagedata);
                //Console.log(base64mobImagedata);
                $("#tempimg").val(base64mobImagedata);
            }
        });

        $(document).on("change", "#mobApp_file", function (evt) {
            var base64mobdata;
            var reader = new FileReader();
            reader.readAsDataURL(this.files[0]);
            var name = this.files[0].name;
            $("#nameget").val(name);
            reader.onloadend = function () {
                base64mobdata = reader.result;
                //console.log(base64mobdata);
                // $("#imga").attr('src', base64mobdata);

                $("#tempimg").val(base64mobdata);
            }
        });

        $("#getCamera").on("click", function () {
            // alert();
            $("#cancelsnap").on("click", function () {
                $("#btnOpenModel").css("display", "none");
                cameraStream.stop();
            });
            $("#btnOpenModel").css("display", "block");
            $(".hdnCamerabefore").css("display", "block");
            var canvas = document.getElementById('canvas');
            var context = canvas.getContext('2d');
            var video = document.querySelector('video#video');
            var cameraStream = null;
            var mediaConfig = { video: true };
            var errBack = function (e) {
                console.log('An error has occurred!', e)
            };
            // Put video listeners into place
            if (navigator.mediaDevices && navigator.mediaDevices.getUserMedia) {
                navigator.mediaDevices.getUserMedia(mediaConfig).then(function (stream) {
                    video.srcObject = stream;
                    video.play();
                    cameraStream = stream.getTracks()[0];
                });
            }
                /* Legacy code below! */
            else if (navigator.getUserMedia) { // Standard
                navigator.getUserMedia(mediaConfig, function (stream) {
                    video.src = stream;
                    video.play();
                    cameraStream = stream.getTracks()[0];
                }, errBack);
            }
            else if (navigator.mozGetUserMedia) {// Mozilla-prefixed
                navigator.mozGetUserMedia(mediaConfig, function (stream) {
                    video.src = window.URL.createObjectURL(stream);
                    video.play();
                    cameraStream = stream.getTracks()[0];
                }, errBack);
            }
            // Trigger photo take
            document.getElementById('snap').addEventListener('click', function () {
                $("#btnOpenModel").css("display", "none");
                $('#hdncapture').val("true");
                $("#imageLicence").css("display", "none");
                context.drawImage(video, 0, 0, 640, 480);
                var image = document.getElementById("canvas").toDataURL("image/png");
                $("#tempimg").val(image);
                // cameraStream.stop();

            });
        });

    });

function btndeleteyes_Click() {

    $('#LoadingPanel1').css('display', 'block');
    var fileData = new FormData();

    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var id = getParameterByName('id');
    var transId = getParameterByName('transId');
    var RInvId = getParameterByName('RInvId');
    var DIDDetailID = DIDDetailIDs
    var hdnimagname = $('#hdnimgname').val();

    fileData.append('UID', UID);
    fileData.append('ClientID', id);
    fileData.append('TransId', transId);
    fileData.append('RInvId', RInvId);
    fileData.append('UserRole', UserRole);
    fileData.append('hdnimgname', hdnimgname);

    $.ajax({
        url: '/DamagedInventoryConfirmDetails/btndeleteyes_Click',
        type: "POST",
        data: JSON.stringify({ DIDDetailID: DIDDetailID, hdnimgname: hdnimagname }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,

        success: function (response) {
            var res = response;
            if (res = "Success") {
                $('#divdelete').css('display', 'none');
                var response = { Result: "Success", Message: "Record Deleted Successfully." };
                CustomAlert(response);
                DamageInvConfirDetail();
                showtImage();
                GetUplodadPdfData();
               
            }
            
            $('#LoadingPanel1').css('display', 'block');
        }
    });

}

function btnclose() {
    var output = document.getElementById('fileList');
    output.innerHTML = '';
    $('#divCreatePop').css('display', 'none');
    $("#Photoblock").css('display', 'none');
}


function deleteno() {
    $('#divdelete').css('display', 'none');
}

function ButtonClient() {
   
    var desc = $("#txtdesc").val();
    if (desc != "") {
        $('#LoadingPanel1').css('display', 'block');


        var fileData = new FormData();
        var files;

        // Checking whether FormData is available in browser
        if (window.FormData !== undefined) {
            if (files == null || files == "") {
                var fileUpload = $('#FileUpload1').get(0);
                files = fileUpload.files;
            }
            var fileUpload = $('#FileUpload1').get(0);
            // Create FormData object
          //  var fileData = new FormData();
            if (files.length != 0) {

                var FileName = files[0].name;
                var FileSize = files[0].size;
                var extension = FileName.substring(FileName.lastIndexOf('.') + 1).toLowerCase();
                fileData.append(files[0].name, files[0]);
                //if (extension.toLowerCase() == "jpg" || extension.toLowerCase() == "jpeg" || extension.toLowerCase() == "png" || extension.toLowerCase() == "ico" || extension.ToLower() == "gif" || extension.toLowerCase() == "bmp") {

                //    fileData.append(files[0].name, files[0]);

                //}
            }
        }

        var UID = getParameterByName('UID');
        var UserRole = getParameterByName('UserRole');
        var id = getParameterByName('id');
        var DIDetailID = getParameterByName('DIDetailID');
        var RID = getParameterByName('RID');

        var tempimg = $('#tempimg').val();
        var hdndocument = $('#hdndocument').val();
        var FileUpload1 = $('#FileUpload1').val();
        var hdncapture = $('#hdncapture').val();
        var nameget = $('#nameget').val();


        fileData.append('UID', UID);
        fileData.append('id', id);
        fileData.append('DIDetailID', DIDetailID);
        fileData.append('txtdesc', desc);
        fileData.append('RID', RID);
        fileData.append('UserRole', UserRole);
        fileData.append('tempimg', tempimg);
        fileData.append('hdndocument', hdndocument);
        fileData.append('FileUpload1', FileUpload1);
        fileData.append('hdncapture', hdncapture);
        fileData.append('nameget', nameget);

        if (desc != null && desc != '') {
            $.ajax({
                url: '/DamagedInventoryConfirmDetails/btnSave_Click',
                type: "POST",
                contentType: false, // Not to set any content header
                processData: false, // Not to process data
                data: fileData,
                success: function (response) {
                    var res = response;
                    var response = { Result: "Success", Message: "Data Saved Successfully." };
                    CustomAlert(response);
                    $('#divCreatePop').hide();
                    showtImage();
                    GetUplodadPdfData();
                    $("#LoadingPanel1").css("display", "none");
                }
            })
        } else {
            $("#LoadingPanel1").css("display", "none");
            var response = {Result:"Failed",Message:"Fill Description."};
            CustomAlert(response);
        }

    }
}


function addnewimage() {
    //alert();
    $("#txtdesc").val('');
    $("#divCreatePop").css("display", "block");
    var doctype = "Image";
    $("#hdndocument").val(doctype);
    $("#lblimgdoc").text("Image");
    $(".rowhide").css("display", "block")
    $(".rowheight").css("height", "435px");
}
function addnewDocument() {
    $("#txtdesc").val('');
    $("#divCreatePop").css("display", "block");
    var doctype = "AllTypedoc";
    $("#hdndocument").val(doctype);
    $(".rowhide").css("display", "none");
    $("#lblimgdoc").text("Document");
    $(".rowheight").css("height", "250px");
}


function showtImage() {
    var id = getParameterByName('id');
    var DIDetailID = getParameterByName('DIDetailID');
    $.ajax({
        url: '/DamagedInventoryConfirmDetails/showtImage',
        type: 'POST',
        data: JSON.stringify({ DIDetailID: DIDetailID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            $('#placeholderforimage').empty().append(response);


            $('img').on('click', function () {
                //alert(this.id);
                var imageid = this.id;
                if (imageid != 'ppdl') {
                    $('#ppdl').attr("src", this.src);
                    $('#popup').css('display', 'block');
                }
                else {

                }
            });

            $('.close').on('click', function () {
                $('#popup').css('display', 'none');
            });
        }
    });
}

function margin() {
    $('.footermargin').css('margin-top', '700px');
}


var DIDDetailIDs = '';
function DamageInvConfirDetail() {
    $('#LoadingPanel1').css('display', 'block');
    var UserRole = getParameterByName('UserRole');
    var id = getParameterByName('id');
    var RInvId = getParameterByName('RInvId');
    var DamagedInvId = getParameterByName('DamagedInvId');
    var DIDetailID = getParameterByName('DIDetailID');
    var FromId = getParameterByName('FromId');
    var ToId = getParameterByName('ToId');
    var fromtocheck = getParameterByName('fromtocheck');

    $.ajax({
        url: '/DamagedInventoryConfirmDetails/DamagedConfDetailsData',
        type: 'POST',
        data: JSON.stringify({id:id, RInvId:RInvId, DamagedInvId:DamagedInvId, DIDetailID:DIDetailID, FromId:FromId, UserRole:UserRole, ToId:ToId, fromtocheck}),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            var data = response;

            $('#tblDamageInventoryConfirmDetails').DataTable().clear();
            $('#tblDamageInventoryConfirmDetails').DataTable().destroy();

            if (data.length != 0) {
                var str = '';
                $.each(data, function (index, obj) {

                    var DIDetailID = obj.DIDetailID == null ? "" : obj.DIDetailID;
                    var MaterialType = obj.MaterialType == null ? "" : obj.MaterialType;
                    var ItemCode = obj.ItemCode == null ? "" : obj.ItemCode;
                    var ItemDesc = obj.ItemDesc == null ? "" : obj.ItemDesc;
                    var ItemWeight = obj.ItemWeight == null ? "" : obj.ItemWeight;
                    var Supplier = obj.Supplier == null ? "" : obj.Supplier;
                    var SupplierDocketNo = obj.SupplierDocketNo == null ? "" : obj.SupplierDocketNo;
                    var Reason = obj.Reason == null ? "" : obj.Reason;
                    var DateActioned = obj.DateActioned == null ? "" : obj.DateActioned;
                    var AvaQty = obj.AvaQty == null ? "" : obj.AvaQty;
                    var DamagedQuantity = obj.DamagedQuantity == null ? "" : obj.DamagedQuantity;
                    var TotalItemWeight = obj.TotalItemWeight == null ? "" : obj.TotalItemWeight;

                    DIDDetailIDs = DIDetailID;
                    str += "<str>";
                    str += "<td style='display:none'></td>";
                    str += "<td>" + MaterialType + "</td>";
                    str += "<td>" + ItemCode + "</td>";
                    str += "<td>" + ItemDesc + "</td>";
                    str += "<td>" + AvaQty + "</td>";
                    str += "<td>" + DamagedQuantity + "</td>";
                    str += "<td>" + Reason + "</td>";
                    //str += "<td>" + Supplier + "</td>";
                    str += "<td style='background-color: LightCyan'><input type='text' class='InpSupplier' value='" + Supplier + "' style='width:100%;'></td>";
                    str += "<td style='background-color: LightCyan'><input type='text' class='InpSupplierDocketNo'  value='" + SupplierDocketNo + "' style='width:100%'> </td>";
                    str += "<td style='background-color: LightCyan'><input type='text' class='InpDateActioned'  value='" + DateActioned + "' style='width:100%'></td>";
                    str += "<td>" + ItemWeight + "</td>";
                    str += "<td>" + TotalItemWeight.toFixed(2); + "</td>";
                    str += "</str>";

                });

                $('#tbodyTblDamageInventoryConfirmDetails').html(str);
                table = $('#tblDamageInventoryConfirmDetails').dataTable({
                    "bSort": false,
                    "processing": true,
                    scrollY: '25vh',
                    searching: false,
                    paging: false,
                    info: false,
                    autoWidth: false,

                });

                $('.InpDateActioned').click(function () {
                    $(this).datepicker({
                        maxDate: 0,
                        changeMonth: true,
                        changeYear: true,
                        dateFormat: 'dd-M-yy',
                        //onClose: function (selectedDate) {
                        //   // table.draw();
                        //}
                    });
                });

            }
            $('#LoadingPanel1').css('display', 'none');
        }
    });
}


function BatchEditCommand() {
    $('#LoadingPanel1').css('display', 'block');
    var Supplier = $('.InpSupplier').val();
    var SupplierDocketNo = $('.InpSupplierDocketNo').val();
    var DateActioned = $('.InpDateActioned').val();
    var DIDDetail = DIDDetailIDs;

    $.ajax({
        url: '/DamagedInventoryConfirmDetails/BatchEditCommand',
        type: 'POST',
        data: JSON.stringify({ DIDDetail: DIDDetail, Supplier: Supplier, SupplierDocketNo: SupplierDocketNo, DateActioned: DateActioned }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,

        success: function (response) {

            if (response.Result == "Success") {
                
                CustomAlert(response);
                DamageInvConfirDetail();
            }
        }
    });
}
function btnprint_Click()
{
    $('#LoadingPanel1').css('display', 'block');
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var fromtocheck = getParameterByName('fromtocheck');
    var DIDetailID = getParameterByName('DIDetailID');
    var DamagedInvId = getParameterByName('DamagedInvId');
    var FromId = getParameterByName('FromId');
    var ToId = getParameterByName('ToId');
    var FromName = getParameterByName('FromName');

    var RInvId = "";

    var encryData = Encrypt([RInvId]);
    RInvId = encryData[0];

    if (DIDetailID != "") {
        window.open('/DamagedReport/DamagedReport?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&fromtocheck=' + fromtocheck + '&DIDetailID=' + DIDetailID + '&DamagedInvId=' + DamagedInvId + '&FromId=' + FromId + '&ToId=' + ToId + '&FromName=' + FromName + '&RInvId=' + RInvId);

        $('#LoadingPanel1').css('display', 'none');
    }
    $('#LoadingPanel1').css('display', 'none');

    //  Response.Write("<script> window.open('DamagedReport.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&fromtocheck=" + Request.QueryString["fromtocheck"] + "&id=" + Request.QueryString["id"] + "&DIDetailID=" + Request.QueryString["DIDetailID"] + "&DamagedInvId=" + Request.QueryString["DamagedInvId"] + "&FromId=" + Request.QueryString["FromId"] + "&ToId=" + Request.QueryString["ToId"] + "&FromName=" + Request.QueryString["FromName"] + "&fileName=" + databaseCon.Encrypt(hdntemppdffile.Value) + "&btn=prv','_blank');</script>");


}
function btnBack_Click()
{

    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var fromtocheck = getParameterByName('fromtocheck');
    var DamagedInvId = getParameterByName('DamagedInvId');
    var FromId = getParameterByName('FromId');
    var ToId = getParameterByName('ToId');
     var page = getParameterByName('page');
   

     window.location.href = '/DamagedInventoryDetails/DamagedInventoryDetails?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&fromtocheck=' + fromtocheck + '&FromId=' + FromId + '&ToId=' + ToId + '&DamagedInvId=' + DamagedInvId + '&page=' + page;
   
      // Response.Redirect("DamagedInventoryDetails.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&fromtocheck=" + Request.QueryString["fromtocheck"] + "&id=" + Request.QueryString["id"] + "&FromId=" + Request.QueryString["FromId"] + "&ToId=" + Request.QueryString["ToId"] + "&DamagedInvId=" + Request.QueryString["DamagedInvId"] + "&page=" + Request.QueryString["page"]);
}

function GetUplodadPdfData() {
    $('#LoadingPanel1').css('display', 'block');

    var id = getParameterByName('id');
    var DIDetailID = getParameterByName('DIDetailID');
 
    $.ajax({
        url: '/DamagedInventoryConfirmDetails/GetUplodadPdfData',
        type: 'POST',
        data: JSON.stringify({DIDetailID: DIDetailID}),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            var data = response;

            $('#tblAddPDfData').DataTable().clear();
            $('#tblAddPDfData').DataTable().destroy();

            if (data.length != 0) {
                var str = '';
                $.each(data, function (index, obj) {

                    var DDDocumentID = obj.DDDocumentID == null ? "" : obj.DDDocumentID;
                    var DocumentName = obj.DocumentName == null ? "" : obj.DocumentName;
                    var Description = obj.Description == null ? "" : obj.Description;
                    var DateOfUpload = obj.DateOfUpload == null ? "" : obj.DateOfUpload;
                    var DocumentPdfPath = obj.DocumentPdfPath == null ? "" : obj.DocumentPdfPath;

                    str += "<tr>";
                    str += "<td style='display:none'></td>";
                    str += "<td id=" + DDDocumentID + " title='Click here for Edit' style='cursor:pointer;' DDescription='" + Description + "'><i class='fa fa-pencil' onclick=OpeEditPopup('" + DDDocumentID + "','" + DocumentName + "')></i></td>";

                    str += "<td title='Click here for Delete' style='cursor:pointer;'><i class='fa fa-trash' onclick=OpeDeletePDfPopup('" + DDDocumentID + "','" + DocumentName + "')></i></td>";
                    str += "<td>" + Description + "</td>";
                    str += "<td>" +DateFormat(DateOfUpload) + "</td>";
                    str += "<td title='View Pdf Document' style='cursor:pointer;'><button type='button' id='tbnView' Class='w3-btn w3-teal' onclick=ViewDocument('" + DocumentPdfPath + "')><i class='fa fa-search'></i>&nbsp;View</button></td>";
                    str += "</tr>";

                });

                $('#tbodyTblAddPDfDatatbl').html(str);
                $('#tblAddPDfData').dataTable({
                    "bSort": false,
                    "processing": true,
                    scrollY: '25vh',
                    searching: false,
                    paging: false,
                    info: false,
                    autoWidth: false,

                });
            }
            $('#LoadingPanel1').css('display', 'none');
        }
    });
}

function ViewDocument(DocumentPdfPath){
    $('#LoadingPanel1').css('display', 'block');
    if (DocumentPdfPath != "") {
        window.open(DocumentPdfPath);
        $('#LoadingPanel1').css('display', 'none');
    }
}
function OpeDeletePDfPopup(DDDocumentID,DocumentName)
{
    $('#hdnDDDocumentID').val(DDDocumentID);
    $('#hdnDocumentName').val(DocumentName);
    $('#confmodal').css("display", "block");
}

function btnyes_Click()
{
    $('#confmodal').css("display", "none");
    $('#LoadingPanel1').css('display', 'block');
    var id = getParameterByName('id');
    var hdnDDDocumentID = $('#hdnDDDocumentID').val();
    var hdnDocumentName = $('#hdnDocumentName').val();
    $.ajax({
        url: '/DamagedInventoryConfirmDetails/btnyes_Click',
        type: 'POST',
        data: JSON.stringify({ id: id,hdnDDDocumentID: hdnDDDocumentID,hdnDocumentName: hdnDocumentName }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,

        success: function (response) {
            var getdata = response;
            if (response>0) {

                var Response = { Result: "Success", Message: "Record Deleted Successfully." };
                CustomAlert(Response);
                DamageInvConfirDetail();
                showtImage();
                GetUplodadPdfData();
                $('#confmodal').css("display", "none");
            }
            $('#LoadingPanel1').css('display', 'none');
        }
    });

}
function btnno_Click()
{
    $('#confmodal').css("display", "none");
}

//function OpeEditPopup(DDDocumentID, Description, DocumentName) {
function OpeEditPopup(DDDocumentID, DocumentName) {
    $('#EditPopup').css("display", "block");

    var Description = $('#' + DDDocumentID).attr("DDescription");

    $('#hdnDDDocumentID').val(DDDocumentID);
    $('#txtdescription').val(Description);
    $('#hdnDocumentName').val(DocumentName);
}

function btneditSave()
{
  $('#LoadingPanel1').css('display', 'block');
  var id = getParameterByName('id');
  var UID = getParameterByName('UID');
  var hdnDDDocumentID = $('#hdnDDDocumentID').val();
  var txtdescription = $('#txtdescription').val();
  var hdnDocumentName = $('#hdnDocumentName').val();
 
  $.ajax({
      url: '/DamagedInventoryConfirmDetails/btneditSave',
      type: 'POST',
      data: JSON.stringify({ id: id, UID: UID, hdnDDDocumentID: hdnDDDocumentID, txtdescription: txtdescription, hdnDocumentName: hdnDocumentName }),
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      async: true,

      success: function (response) {
        var  gat = response;
          if (response == "Success") {

              $('#EditPopup').css("display", "none");
              
              var Response = { Result: "Success", Message: "Record Saved Successfully." };
              CustomAlert(Response);
              DamageInvConfirDetail();
              showtImage();
              GetUplodadPdfData();
              $('#LoadingPanel1').css('display', 'none');
          }
      }
  });
}
function btnCancel()
{
    $('#EditPopup').css("display", "none");
}
