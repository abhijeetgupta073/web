
$(document).ready(function () {
    IsTransactionInactive();
          $(".dropdown.user").click(function () {
              $(this).toggleClass("open");
          });
         // $('#lblclientname').val(@ViewBag.ClientName);
         

          var currentDate = new Date();
          $('#datePicker1').val(currentDate.DateMonthFormat());

          $("#datePicker1").datepicker({
              dateFormat: 'dd-M-yy',
              // maxDate: 0,
              closeText: 'Close',
              maxDate: currentDate,

          });
          $("#Datepicker").datepicker({
              dateFormat: 'dd-M-yy',
              //  maxDate: 0,
              closeText: 'Close',

          });

          ClientRepresentationDropDownBind();
          GetDropDownBind();
          GetDeliveryData();
          GetDeliveryDocketData();
          //setTimeout(function () {
          var ThirdParty = getParameterByName('ThirdParty');
          var ToName = getParameterByName('To');
          var ToData = Decrypt(ToName);
          var encryData = Decrypt(ThirdParty);
         // var ThirdParty = $('#rdoThirdPartytoDepot').prop('checked');
          //}, 3000);
          if (encryData == "true" || ToData == "ThirdParty") {
              $('#divlblfrom').css('display', 'block');
              $('#divlblfrom1').css('display', 'block');
              $('#divlblto').css('display', 'block');
              $('#divlvlto1').css('display', 'block');
              $('#divlblthirdparty').css('display', 'block');
              $('#divlblsitename').css('display', 'none');
              $('#divlblsitename1').css('display', 'none');
              $('#divlblsiteaddress').css('display', 'none');
              $('#divlblsiteaddress1').css('display', 'none');
              $('#divlblmaindepot').css('display', 'none');
              
          }
          else{
              $('#divlblfrom').css('display', 'none');
              $('#divlblfrom1').css('display', 'none');
              $('#divlblto').css('display', 'none');
              $('#divlvlto1').css('display', 'none');
              $('#divlblthirdparty').css('display', 'none');
              $('#divlblsitename').css('display', 'block');
              $('#divlblsitename1').css('display', 'block');
              $('#divlblsiteaddress').css('display', 'block');
              $('#divlblsiteaddress1').css('display', 'block');
              $('#divlblmaindepot').css('display', 'block');
          }

      });
      function ClientRepresentationDropDownBind() {
          var ClientID = getParameterByName('id');
         // $('#hid').val('@viewBag.hid');
          $.ajax({
              type: "POST",
              url: "/DeliveryDocket/GetClientRepresentationDropDownBind",

              data: JSON.stringify({ ClientID: ClientID }),
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              async: false,
              success: function (response) {

                  var GetData = response;
                  if (GetData.length > 0) {
                      $("#txtposition").append('<option value=""></option>');
                      $("#txtAcceptedposi").append('<option value=""></option>');
                    for (var i = 0; i < GetData.length; i++) {
                         $("#txtposition").append("<option value='" + GetData[i].Position + "'>" + GetData[i].Position + "</option>");
                         $("#txtAcceptedposi").append("<option value='" + GetData[i].Position + "'>" + GetData[i].Position + "</option>");
                      }
                  }
              }
          })
      }
      
      function GetDropDownBind() {
          var ClientID = getParameterByName('id');
          // $('#hid').val('@viewBag.hid');
          $.ajax({
              type: "POST",
              url: "/DeliveryDocket/GetDropDownBind",

              data: JSON.stringify({ ClientID: ClientID }),
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              async: false,
              success: function (response) {

                  var GetData = response;
                  if (GetData.length > 0) {
                      $("#txtpickedby").append('<option value=""></option>');
                      for (var i = 0; i < GetData.length; i++) {
                          $("#txtpickedby").append("<option value='" + GetData[i].FullName + "'>" + GetData[i].FullName + "</option>");

                      }
                  }
              }
          })
      }
      function GetDeliveryData() {
          var ClientID = getParameterByName('id');
          var checkid = getParameterByName('checkid');
          $.ajax({
              type: "POST",
              url: "/DeliveryDocket/GetDeliveryData",

              data: JSON.stringify({ ClientID: ClientID, checkid: checkid }),
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              async: true,
              success: function (response) {

                  var GetData = response;
                  if (GetData.length > 0) {
                      var totalQuntity = 0;
                      var SumOfTotalWeight = 0.0;
                      var str = "";
                      $.each(response, function (index, obj) {
                          str += "<tr>";
                          str += "<td>" + obj.MaterialType + "</td>"
                          str += "<td>" + obj.ItemCode + "</td>"
                          str += "<td>" + obj.ItemDesc + "</td>"
                          str += "<td>" + obj.Quantity + "</td>"
                          str += "<td>" + obj.ItemWeight.toFixed(2) + "</td>"
                          str += "<td>" + obj.TotalWeight.toFixed(2) + "</td>"
                          str += "</tr>";
                          totalQuntity += obj.Quantity;
                          SumOfTotalWeight += obj.TotalWeight;
                      })
                      $('#txtTotalItem').val(totalQuntity);
                     // var totalweights = String.Format("{0:#,##0.00}", (Convert.ToDouble(totalQuntity / 1000)));
                      var totalweights = (SumOfTotalWeight / 1000);
                      $('#txtTotalWeight').val(totalweights.toFixed(2));
                     // txtTotalWeight.Text = totalweights;
                      $('#tbodyDeliveryDocket').empty().html(str);

                  }
                  else {
                      // $('#df').empty();                  }
                  }
              }
          });
      
      }

      function GetDeliveryDocketData() {
          var ClientID = getParameterByName('id');
          var checkid = getParameterByName('checkid');
          $.ajax({
              type: "POST",
              url: "/DeliveryDocket/GetDeliveryDocketData",

              data: JSON.stringify({ ClientID: ClientID, checkid: checkid }),
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              async: true,
              success: function (response) {

                  var GetData = response;
                 
                  var DepotPersonSign = GetData.DepotPersonSign == null ? "" : GetData.DepotPersonSign;
                  var TransportDriverSign = GetData.TransportDriverSign == null ? "" : GetData.TransportDriverSign;
                  var SitePersonSign = GetData.SitePersonSign == null ? "" : GetData.SitePersonSign;
                  var AcceptorSign = GetData.AcceptorSign == null ? "" : GetData.AcceptorSign;
                  var DepotPic = GetData.DepotPic == null ? "" : GetData.DepotPic;
                  var SitePic = GetData.SitePic == null ? "" : GetData.SitePic;
                  var ReceiptConfirmation = GetData.ReceiptConfirmation == null ? false : GetData.ReceiptConfirmation;
                  var DepotPickedBy = GetData.DepotPickedBy == null ? "" : GetData.DepotPickedBy;
                  var TransportBy = GetData.TransportBy == null ? "" : GetData.TransportBy;
                  var SiteReceivedBy = GetData.SiteReceivedBy == null ? "" : GetData.SiteReceivedBy;
                  var AcceptedBy = GetData.AcceptedBy == null ? "" : GetData.AcceptedBy;
                  var PickDate = GetData.PickDate == null ? "" : GetData.PickDate;
                  var SiteReceivedDate = GetData.SiteReceivedDate == null ? "" : GetData.SiteReceivedDate;
                  var ReceivedByPosition = GetData.ReceivedByPosition == null ? "" : GetData.ReceivedByPosition;
                  var ClientPosition = GetData.ClientPosition == null ? "" : GetData.ClientPosition;
                  var TransportCompName = GetData.TransportCompName == null ? "" : GetData.TransportCompName;
                  //txtpickedby.SelectedValue = dt3.Rows[0]["DepotPickedBy"].ToString();
                  //txtTransport1.Text = dt3.Rows[0]["TransportBy"].ToString();
                  //txtReceived.Text = dt3.Rows[0]["SiteReceivedBy"].ToString();
                  //txtAccepted.Text = dt3.Rows[0]["AcceptedBy"].ToString();
                  // datePicker1.SelectedDate = Convert.ToDateTime(dt2.Rows[0]["Date"].ToString());
                  // Datepicker.SelectedDate = Convert.ToDateTime(dt.Rows[0]["SiteReceivedDate"].ToString());
                  // txtposition.SelectedValue = dat1.Rows[0]["ReceivedByPosition"].ToString();
                  // txtAcceptedposi.SelectedValue = dat1.Rows[0]["ClientPosition"].ToString();
                 
                   
                  $('#txtpickedby').val(DepotPickedBy);
                  $('#txtTransport1').val(TransportBy);
                  $('#txtTransport2').val(TransportCompName);
                  $('#txtReceived').val(SiteReceivedBy);
                  $('#txtAccepted').val(AcceptedBy);
                  $('#confirmcheck').prop('checked', ReceiptConfirmation);
                  $('#txtposition').val(ReceivedByPosition);
                  $('#txtAcceptedposi').val(ClientPosition);
                 
                   PickDate= DateFormat(PickDate)
                   if (PickDate != null && PickDate != '01-Jan-1900' && PickDate != '01/01/1900 12:00:00 AM' && PickDate != '31-Dec-1899') {
                       if (PickDate != "") {
                           $('#datePicker1').val(PickDate);
                       }
                     
                  }
                   SiteReceivedDate = DateFormat(SiteReceivedDate)
                   if (SiteReceivedDate != null && SiteReceivedDate != '01-Jan-1900' && SiteReceivedDate != '01/01/1900 12:00:00 AM' && SiteReceivedDate != '31-Dec-1899') {
                       $('#Datepicker').val(SiteReceivedDate);
                       $('#lbldate').val(SiteReceivedDate);
                   }

                   if (TransportDriverSign != null && TransportDriverSign != "") {
                       //$('#mainsite').css('visibility', 'visible');
                        $('#mainsite').show();
                   
                   } else {
                       $('#mainsite').hide();
                   }
                  

                  if (ReceiptConfirmation == true) {
                     // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp12", "<script>RemoveSignatureBlock();</script>", false);
                      //Confirmation.Style.Add("display", "block");
                      //btnSubmit.Style.Add("display", "none");
                      //TopSubmit.Style.Add("display", "none");
                      //datePicker1.Enabled = false;
                      //txttransferNo.Enabled = false;
                      //txtTotalItem.Enabled = false;
                      //txtTotalWeight.Enabled = false;
                    
                      //RemoveSignatureBlock();

                      $('#Confirmation').css("display", "block");
                      $('#btnSubmit').css("display", "none");
                      $('#TopSubmit').css("display", "none");
                      $('#datePicker1').css("disabled", true);
                      $('#txttransferNo').css("disabled", true);
                      $('#txtTotalItem').css("disabled", true);
                      $('#txtTotalWeight').css("disabled", true);
                  
                  }

                  if (DepotPersonSign != "") {
                      $('#signimg1').attr("src", GetData.DepotPersonSign);
                      $('#printcanvas').css("display", "none");
                      $('#hdnSignImageBlock').css("display", "block");
                  }

                  if (TransportDriverSign != "") {
                      $('#signimg2').attr("src", GetData.TransportDriverSign);
                      $('#printcanvas1').css("display", "none");
                      $('#hdnSignImageBlock1').css("display", "block");
                  }

                  if (SitePersonSign != "") {
                      $('#signimg3').attr("src", GetData.SitePersonSign);
                      $('#printcanvas2').css("display", "none");
                      $('#hdnSignImageBlock2').css("display", "block");
                  }

                  if (AcceptorSign != "") {
                      $('#signimg4').attr("src", GetData.AcceptorSign);
                      $('#printcanvas3').css("display", "none");
                      $('#hdnSignImageBlock3').css("display", "block");
                  }

                  if (DepotPic != "") {
                      $('#imagetruck').attr("src", GetData.DepotPic);
                      $('#Photoblock').css("display", "none");
                      $('#imagetruck').css("display", "block");
                  }
                  if (SitePic != "") {
                      $('#image').attr("src", GetData.SitePic);
                      $('#photoblock2').css("display", "none");
                      $('#image').css("display", "block");
                     // SafariCheck(" + 1 + ", null, null);
                  }
  
              }
          });
      }

      function DelieveryDocketUpload() {
      
          var confirmcheck = $('#confirmcheck').prop('checked');
         // var hdnconfirmcheck = $('#hdnconfirmcheck').val();
          if (confirmcheck == true)
          {
              $('#confirmModal').css("display", "block");
              
          }
          else {
              DelieveryDocketUploadData();
          }

      }


      function DelieveryDocketUploadData() {
          $('#LoadingPanel1').show();
          //bool flag = false;
          //if (confirmcheck.Checked == true)
          //{
          //    confirmModal.Style.Add("display", "block");
          //}


          var sketchData = $('#sketch_data').val();
          var sketchData1 = $('#sketch_data1').val(); 
          var sketchData2 = $('#sketch_data2').val(); 
          var sketchData3 = $('#sketch_data3').val(); 

          var imageData = $('#tempimg').val();
          var imageData1 = $('#tempimg2').val();
          
          var datePicker1=$('#datePicker1').val();
          var SiteReceivedDate=$('#Datepicker').val();
          var txtReceived=$('#txtReceived').val();
          var txtposition = $('#txtposition option:selected').val();
          var txtpickedby = $('#txtpickedby option:selected').val();
          var txtTransport1=$('#txtTransport1').val();
          var txtTransport2=$('#txtTransport2').val();

          var ClientID = getParameterByName('id');
          var checkid = getParameterByName('checkid');
          
          $.ajax({
              type: "POST",
              url: "/DeliveryDocket/DelieveryDocketUpload",

              data: JSON.stringify({ ClientID: ClientID, checkid: checkid, imageData: imageData, imageData1: imageData1, sketchData: sketchData, sketchData1: sketchData1, sketchData2: sketchData2, sketchData3: sketchData3, datePicker1: datePicker1, SiteReceivedDate: SiteReceivedDate, txtReceived: txtReceived, txtposition: txtposition, txtpickedby: txtpickedby, txtTransport1: txtTransport1, txtTransport2: txtTransport2}),
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              async: true,
              success: function (response) {

                  var GetData = response;
                  if (response>0) {
                  //$('#art1').addClass("w3-section w3-card-12  j-close j-alertBox-Success").css('display', 'block');
                  //$('#alertlabel1').text("Success ! Data Saved Successfully.");
                      var Response = { Result: "Success", Message: "Data Saved Successfully." }
                      CustomAlert(Response);
                  location.reload();
                  
              }else{
                //$('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
                //$('#art1').css("display", "block");
                      //$('#alertlabel').text("Alert !" + response.Message);
                      $('#LoadingPanel1').hide();
              CustomAlert(response);
          }
              }
          });
      }

      function btnConfirmok_Click() {
          $('#LoadingPanel1').show();
          var txtAccepted = $('#txtAccepted').val();
          var txtAcceptedposi = $('#txtAcceptedposi option:selected').val();
          var confirmcheck = $('#confirmcheck').prop('checked');

          var ClientID = getParameterByName('id');
          var checkid = getParameterByName('checkid');

          $.ajax({
              type: "POST",
              url: "/DeliveryDocket/btnConfirmok_Click",

              data: JSON.stringify({ ClientID: ClientID, checkid: checkid, txtAccepted: txtAccepted, txtAcceptedposi: txtAcceptedposi, confirmcheck: confirmcheck }),
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              async: true,
              success: function (response) {

                  if (response > 0) {
                      $('#confirmModal').css("display", "none");
                      DelieveryDocketUploadData();
                  } else {
                      //$('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
                      //$('#art1').css("display", "block");
                      //$('#alertlabel').text("Alert ! Something Error.");
                      $('#LoadingPanel1').hide();
                      var Response = { Result: "Failed", Message: "Something Error." }
                      CustomAlert(Response);

                  }
               }
          });
      }

      function btnConfirmCancel_Click()
      {
          $('#confirmModal').css("display", "none");
      }
      function btnclose_Click()
      {
          var UID = getParameterByName('UID');
          var UserRole = getParameterByName('UserRole');
          var id = getParameterByName('id');
          var ThirdParty = getParameterByName('ThirdParty');
          window.location.href = '/RequestInventory/RequestInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&ThirdParty=' + ThirdParty;
         // Response.Redirect(DomainName + "/" + "RequestInventory.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"]);
      }
      function btnPrint_Click() {
          var UID = getParameterByName('UID');
          var UserRole = getParameterByName('UserRole');
          var id = getParameterByName('id');
          var ToId = getParameterByName('ToId');
          var checkid = getParameterByName('checkid');
          var ThirdParty = getParameterByName('ThirdParty');
          

         // Response.Write("<script> window.open('DeliveryDocketReport.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&ToId=" + Request.QueryString["ToId"] + "&checkid=" + Request.QueryString["checkid"] + "&DDID=" + Request.QueryString["DDId"] + "&btn=prv','_blank');</script>");
          window.open('/DeliveryDocketReport/DeliveryDocketReport?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&ToId=' + ToId + '&checkid=' + checkid + '&ThirdParty=' + ThirdParty);
      }

       $(document).ready(function () {
           var GetisSafari = navigator.vendor && navigator.vendor.indexOf('Apple') > -1 && navigator.userAgent && !navigator.userAgent.match('CriOS');
           if (GetisSafari == false) {
               $(".nonsafari").css("display", "block");
               $(".safari").css("display", "none");

               var isMobile = /iPhone|iPad|iPod|Android/i.test(navigator.userAgent);
               if (isMobile) {
                   $(".mobileApp").css("display", "block");
                   $(".desktopApp").css("display", "none");
                   $(".hideAudioMob").css("display", "none");
               } else {
                   $(".mobileApp").css("display", "none");
                   $(".desktopApp").css("display", "block");
               }
           } else {
               $(".nonsafari").css("display", "none");
               $(".safari").css("display", "block");
               $(".hideAudioMob").css("display", "none");
           }



           $("#getSign").on("click", function () {
               // $("#btnOpenModel4").click();
               $("#btnsignModal").click();
           });
       });

$(document).on("change", ".mob_file_image", function (evt) {
    $("#Imageblock").css("display", "none");
    // var $mobImagesource = $('#ContentPlaceHolder1_imagetruck');
    var $mobImagesource = $('#imagetruck');
    var base64mobdata;
    var reader = new FileReader();
    reader.readAsDataURL(this.files[0]);
    reader.onloadend = function () {
        base64mobdata = reader.result;
        console.log(base64mobdata);
        $mobImagesource[0].src = base64mobdata;
        // $("#ContentPlaceHolder1_tempimg").val(base64mobdata);
        $("#tempimg").val(base64mobdata);
    }

    // $("#ContentPlaceHolder1_imagetruck").css("display", "block");
    $("#imagetruck").css("display", "block");

});

$(document).on("change", ".file_video", function (evt) {
    $("#Imageblock").css("display", "none");
    // var $mobImagesource = $('#ContentPlaceHolder1_imagetruck');
    var $mobImagesource = $('#imagetruck');
    var base64mobdata;
    var reader = new FileReader();
    reader.readAsDataURL(this.files[0]);
    reader.onloadend = function () {
        base64mobdata = reader.result;
        console.log(base64mobdata);
        $mobImagesource[0].src = base64mobdata;
        $("#tempimg").val(base64mobdata);
    }

    $("#imagetruck").css("display", "block");
});

$(document).on("change", ".mob_file_image1", function (evt) {
    //alert();
    $("#Imageblock1").css("display", "none");
    // var $mobImagesource1 = $('#ContentPlaceHolder1_image');
    var $mobImagesource1 = $('#image');
    var reader = new FileReader();
    reader.readAsDataURL(this.files[0]);
    reader.onloadend = function () {
        base64mobdata = reader.result;
        console.log(base64mobdata);
        $mobImagesource1[0].src = base64mobdata;
        // $("#ContentPlaceHolder1_tempimg2").val(base64mobdata);
        $("#tempimg2").val(base64mobdata);
    }

    // $("#ContentPlaceHolder1_image").css("display", "block");
    $("#image").css("display", "block");
});

$(document).on("change", ".file_video1", function (evt) {
    //alert();
    $("#Imageblock1").css("display", "none");
    // var $mobImagesource1 = $('#ContentPlaceHolder1_image');
    var $mobImagesource1 = $('#image');
    var reader = new FileReader();
    reader.readAsDataURL(this.files[0]);
    reader.onloadend = function () {
        base64mobdata = reader.result;
        console.log(base64mobdata);
        $mobImagesource1[0].src = base64mobdata;
        // $("#ContentPlaceHolder1_tempimg2").val(base64mobdata);
        $("#tempimg2").val(base64mobdata);
    }

    // $("#ContentPlaceHolder1_image").css("display", "block");
    $("#image").css("display", "block");

});


flag = false;
function maindepothide() {
    if (flag == false) {
        //$('#maindepot').css('visibility', 'hidden');
        $('#maindepot').hide();
        flag = true;
    }
    else if (flag == true) {
        //$('#maindepot').css('visibility', 'visible');
        $('#maindepot').show();
        flag = false;
    }

}
flag1 = false;
function mainsitehide() {
    if (flag1 == false) {
        //$('#mainsite').css('visibility', 'hidden');
        $('#mainsite').hide();
        flag1 = true;
    }
    else if (flag1 == true) {
        //$('#mainsite').css('visibility', 'visible');
        $('#mainsite').show();
        flag1 = false;
    }
}

function SaveSketch() {
    //alert("hello");
    var mycanvas = document.getElementById('colors_sketch');
    var canvas1 = document.getElementById('canvas1234');
    //canvas1.restore();
    var context1 = canvas1.getContext('2d');

    var dataUri = $("#colors_sketch")[0].toDataURL();
    $("#sketch_data").val(dataUri);

    //$(".hdnAudiobefore").css("display", "block");
    $("#sigModal .modal-header .close").click();

    $(".hdnSignaturebefore").css("display", "block");

    context1.drawImage(mycanvas, 0, 0, 640, 480);
};

$(document).ready(function () {
    $("#getSign").on("click", function () {
        $(".hdnSignaturebefore").css("display", "none");
        var canvas = document.getElementById('canvas1234'),
         ctx = canvas.getContext("2d");
        ctx.clearRect(0, 0, 640, 480);

        $("#btnSigModal").click();
    });
});

$(document).ready(function () {
    $("#getSign1").on("click", function () {
        // $("#btnOpenModel4").click();
        $("#btnsignModal1").click();
    });
});

function SaveSketch() {
    //alert("hello");
    var mycanvas = document.getElementById('colors_sketch');
    var canvas1 = document.getElementById('canvas12345');
    //canvas1.restore();
    var context = canvas1.getContext('2d');

    var dataUri = $("#colors_sketch")[0].toDataURL();
    $("#sketch_data1").val(dataUri);

    //$(".hdnAudiobefore").css("display", "block");
    $("#sigModal .modal-header .close").click();

    $(".hdnSignaturebefore").css("display", "block");

    context.drawImage(mycanvas, 0, 0, 640, 480);
};

$(document).ready(function () {
    $("#getSign1").on("click", function () {
        $(".hdnSignaturebefore").css("display", "none");
        var canvas = document.getElementById('canvas12345'),
         ctx = canvas.getContext("2d");
        ctx.clearRect(0, 0, 640, 480);

        $("#btnSigModal1").click();
    });
});


$(document).ready(function () {
    $("#getSignature2").on("click", function () {
        // $("#btnOpenModel4").click();
        $("#btnsignModal2").click();
    });
});


function SaveSketch() {
    //alert("hello");
    var mycanvas2 = document.getElementById('colors_sketch');
    var canvas2 = document.getElementById('canvas123456');
    //canvas1.restore();
    var context2 = canvas2.getContext('2d');

    var dataUri = $("#colors_sketch")[0].toDataURL();
    $("#sketch_data2").val(dataUri);

    //$(".hdnAudiobefore").css("display", "block");
    $("#sigModal .modal-header .close").click();

    $(".hdnSignaturebefore").css("display", "block");

    context2.drawImage(mycanvas2, 0, 0, 640, 480);
};

$(document).ready(function () {
    $("#getSignature2").on("click", function () {
        $(".hdnSignaturebefore").css("display", "none");
        var canvas2 = document.getElementById('canvas123456'),
         ctx = canvas2.getContext("2d");
        ctx.clearRect(0, 0, 640, 480);

        $("#btnSigModal2").click();
    });
});

$(document).ready(function () {
    $("#getSignature3").on("click", function () {
        // $("#btnOpenModel4").click();
        $("#btnsignModal3").click();
    });
});


function SaveSketch() {
    //alert("hello");
    var mycanvas3 = document.getElementById('colors_sketch');
    var canvas3 = document.getElementById('canvas1234567');
    //canvas1.restore();
    var context3 = canvas3.getContext('2d');

    var dataUri = $("#colors_sketch")[0].toDataURL();
    $("#sketch_data3").val(dataUri);

    //$(".hdnAudiobefore").css("display", "block");
    $("#sigModal .modal-header .close").click();

    $(".hdnSignaturebefore").css("display", "block");

    context3.drawImage(mycanvas3, 0, 0, 640, 480);
};

$(document).ready(function () {
    $("#getSignature3").on("click", function () {
        $(".hdnSignaturebefore").css("display", "none");
        var canvas3 = document.getElementById('canvas1234567'),
         ctx = canvas3.getContext("2d");
        ctx.clearRect(0, 0, 640, 480);

        $("#btnSigModal3").click();
    });
});



function RemoveSignatureBlock() {
    $(".signature-pad").css("display", "none");
}
function HideSignatureBlock() {
    $("#signature-pad").css("display", "none");
}


    $(document).ready(function () {
        //alert("hello");
        //$("#getCamera").on("click", function () {
        //    alert("hello");
        //});
        //$("#ContentPlaceHolder1_getCamera").on("click", function () {
        $("#getCamera").on("click", function () {
            $("#cancelsnap").on("click", function () {
                $("#btnOpenModel4").css("display", "none");
                cameraStream.stop();

            });
            $("#btnOpenModel4").css("display", "block");

            $(".hdnCamerabefore").css("display", "none");
            // Grab elements, create settings, etc.
            var canvas = document.getElementById('canvas');
            var context = canvas.getContext('2d');
            // var video = document.getElementById('video');
            var video = document.querySelector('video#video');

            var cameraStream = null;

            var mediaConfig = { video: true };
            var errBack = function (e) {
                console.log('An error has occurred!', e)
            };

            // Put video listeners into place
            if (navigator.mediaDevices && navigator.mediaDevices.getUserMedia) {
                navigator.mediaDevices.getUserMedia(mediaConfig).then(function (stream) {
                    //video.src = window.URL.createObjectURL(stream);
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
            } else if (navigator.webkitGetUserMedia) { // WebKit-prefixed
                navigator.webkitGetUserMedia(mediaConfig, function (stream) {
                    video.src = window.webkitURL.createObjectURL(stream);
                    video.play();

                    cameraStream = stream.getTracks()[0];

                }, errBack);
            } else if (navigator.mozGetUserMedia) { // Mozilla-prefixed
                navigator.mozGetUserMedia(mediaConfig, function (stream) {
                    video.src = window.URL.createObjectURL(stream);
                    video.play();

                    cameraStream = stream.getTracks()[0];

                }, errBack);
            }

            // Trigger photo take
            document.getElementById('snap').addEventListener('click', function () {
                //$("#myModal4 .modal-header .close").click();
                $("#btnOpenModel4").css("display", "none");
                $(".hdnCamerabefore").css("display", "block");
                $("#imagetruck").css("display", "none");
                context.drawImage(video, 0, 0, 640, 480);


                var image = document.getElementById("canvas").toDataURL("image/png");
                $("#tempimg").val(image);

                cameraStream.stop();
            });
        });
    });


    $(document).ready(function () {

        $("#getCamera1").on("click", function () {

            $("#cancelsnap1").on("click", function () {
                $("#btnModel2").css("display", "none");
                cameraStream.stop();
            });
            $("#btnModel2").css("display", "block");

            $(".hdnSecondCameraBefore").css("display", "none");
            // Grab elements, create settings, etc.
            var canvas = document.getElementById('SecondCanvas');
            var context = canvas.getContext('2d');
            // var video = document.getElementById('video1');
            var video = document.querySelector('video#video1');

            var cameraStream = null;

            var mediaConfig = { video: true };
            var errBack = function (e) {
                console.log('An error has occurred!', e)
            };

            // Put video listeners into place
            if (navigator.mediaDevices && navigator.mediaDevices.getUserMedia) {
                navigator.mediaDevices.getUserMedia(mediaConfig).then(function (stream) {
                    //video.src = window.URL.createObjectURL(stream);
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
            } else if (navigator.webkitGetUserMedia) { // WebKit-prefixed
                navigator.webkitGetUserMedia(mediaConfig, function (stream) {
                    video.src = window.webkitURL.createObjectURL(stream);
                    video.play();

                    cameraStream = stream.getTracks()[0];

                }, errBack);
            } else if (navigator.mozGetUserMedia) { // Mozilla-prefixed
                navigator.mozGetUserMedia(mediaConfig, function (stream) {
                    video.src = window.URL.createObjectURL(stream);
                    video.play();

                    cameraStream = stream.getTracks()[0];

                }, errBack);
            }

            // Trigger photo take
            document.getElementById('Snap2').addEventListener('click', function () {
                //$("#myModal4 .modal-header .close").click();
                $("#btnModel2").css("display", "none");
                $(".hdnSecondCameraBefore").css("display", "block");
                $("#image").css("display", "none");
                context.drawImage(video, 0, 0, 640, 480);


                var image = document.getElementById("SecondCanvas").toDataURL("image/png");
                $("#tempimg2").val(image);

                cameraStream.stop();
            });
        });
    });

    //Video Code Starts Here

    $(document).ready(function () {

        $("#getVideo").on("click", function () {
            $(".hdnVideobefore").css("display", "none");
            // $("#btnVideoModal").click();
            $("#btnVideoModal").css("display", "block");
            $("#cancelvideo").css("display", "block");

            $("#cancelvideo").on("click", function () {
                $("#btnVideoModal").css("display", "none");
                $("#tempVideo").val("");

                var mediaStream = window.stream;

                //Media Stream Stop Code starts here

                mediaStream.getAudioTracks()[0].stop();
                mediaStream.getVideoTracks()[0].stop();
            });

            var mediaSource = new MediaSource();
            mediaSource.addEventListener('sourceopen', handleSourceOpen, false);
            var mediaRecorder;
            var recordedBlobs;
            var sourceBuffer;

            //var imga = document.getElementById('myvideo');

            var gumVideo = document.querySelector('video#gum');
            var recordedVideo = document.querySelector('video#recorded');

            var recordButton = document.querySelector('button#record1');
            var playButton = document.querySelector('button#play');
            var downloadButton = document.querySelector('button#download');
            recordButton.onclick = toggleRecording;
            playButton.onclick = play;
            downloadButton.onclick = download;

            // Use old-style gUM to avoid requirement to enable the
            // Enable experimental Web Platform features flag in Chrome 49

            navigator.getUserMedia = navigator.getUserMedia ||
              navigator.webkitGetUserMedia || navigator.mozGetUserMedia;

            var constraints = {
                audio: true,
                video: true
            };

            navigator.getUserMedia(constraints, successCallback, errorCallback);

            function successCallback(stream) {
                console.log('getUserMedia() got stream: ', stream);
                window.stream = stream;
                if (window.URL) {
                    //gumVideo.src = window.URL.createObjectURL(stream);
                    gumVideo.srcObject = stream;
                } else {
                    // gumVideo.src = stream;
                    gumVideo.srcObject = stream;
                }
            }

            function errorCallback(error) {
                console.log('navigator.getUserMedia error: ', error);
            }

            function handleSourceOpen(event) {
                console.log('MediaSource opened');
                sourceBuffer = mediaSource.addSourceBuffer('video/webm; codecs="vp8"');
                console.log('Source buffer: ', sourceBuffer);
            }

            function handleDataAvailable(event) {
                if (event.data && event.data.size > 0) {
                    recordedBlobs.push(event.data);
                }
            }

            function handleStop(event) {
                console.log('Recorder stopped: ', event);
            }

            function toggleRecording() {
                if (recordButton.textContent === 'Start Recording') {
                    $("#cancelvideo").css("display", "none");
                    startRecording();
                } else {
                    stopRecording();
                    recordButton.textContent = 'Start Recording';
                    playButton.disabled = false;
                    downloadButton.disabled = false;
                    $("#cancelvideo").css("display", "none");
                    $("#btnVideoModal").css("display", "none");
                    $(".hdnVideobefore").css("display", "block");
                    $("#VideoBlock").css("display", "none");
                    play();
                }
            }

            //The nested try blocks will be simplified when Chrome 47 moves to Stable
            function startRecording() {
                var options = { mimeType: 'video/webm', bitsPerSecond: 100000 };
                recordedBlobs = [];
                try {
                    mediaRecorder = new MediaRecorder(window.stream, options);
                } catch (e0) {
                    console.log('Unable to create MediaRecorder with options Object: ', e0);
                    try {
                        options = { mimeType: 'video/webm,codecs=vp9', bitsPerSecond: 100000 };
                        mediaRecorder = new MediaRecorder(window.stream, options);
                    } catch (e1) {
                        console.log('Unable to create MediaRecorder with options Object: ', e1);
                        try {
                            options = 'video/vp8'; // Chrome 47
                            mediaRecorder = new MediaRecorder(window.stream, options);
                        } catch (e2) {
                            alert('MediaRecorder is not supported by this browser.\n\n' +
                                'Try Firefox 29 or later, or Chrome 47 or later, with Enable experimental Web Platform features enabled from chrome://flags.');
                            console.error('Exception while creating MediaRecorder:', e2);
                            return;
                        }
                    }
                }
                console.log('Created MediaRecorder', mediaRecorder, 'with options', options);
                recordButton.textContent = 'Stop Recording';
                playButton.disabled = true;
                downloadButton.disabled = true;
                mediaRecorder.onstop = handleStop;
                mediaRecorder.ondataavailable = handleDataAvailable;
                mediaRecorder.start(10); // collect 10ms of data
                console.log('MediaRecorder started', mediaRecorder);
            }

            function stopRecording() {

                mediaRecorder.stop();
                console.log('Recorded Blobs: ', recordedBlobs);

                var mediaStream = window.stream;

                //Media Stream Stop Code starts here

                mediaStream.getAudioTracks()[0].stop();
                mediaStream.getVideoTracks()[0].stop();

                //Media Stream Stop Code Ends Here

                recordedVideo.controls = true;
                var base64data;
                var superBuffer = new Blob(recordedBlobs, { type: 'video/webm' });

                //var as = window.URL.createObjectURL(superBuffer);
                //alert(as);
                var reader = new window.FileReader();
                reader.readAsDataURL(superBuffer);

                var reader = new FileReader();
                reader.readAsDataURL(superBuffer);
                reader.onloadend = function () {
                    base64data = reader.result;

                    console.log(base64data);
                    $("#tempVideo").val(base64data);
                }

            }

            function play() {
                var superBuffer = new Blob(recordedBlobs, { type: 'video/webm' });
                //recordedVideo.src = window.URL.createObjectURL(superBuffer);

                var reader = new FileReader();
                reader.readAsDataURL(superBuffer);
                reader.onloadend = function () {
                    var base64data1 = reader.result;

                    //console.log(base64data);
                    // $("#tempVideo").val(base64data);
                    recordedVideo.src = base64data1;
                }

                //recordedVideo.srcObject = superBuffer;

                //recordedVideo.src = window.URL.createObjectURL(superBuffer);
            }

            function download() {
                var blob = new Blob(recordedBlobs, { type: 'video/webm' });
                var url = window.URL.createObjectURL(blob);
                var a = document.createElement('a');
                a.style.display = 'block';
                a.href = url;

                a.download = 'test.webm';
                document.body.appendChild(a);
                a.click();

                setTimeout(function () {
                    document.body.removeChild(a);
                    window.URL.revokeObjectURL(url);
                }, 100);
            }
        });
    });

//Video Code Ends Here

    var wrapper = document.getElementById("signature-pad");
var clearButton = wrapper.querySelector("[data-action=clear]");
var changeColorButton = wrapper.querySelector("[data-action=change-color]");
var undoButton = wrapper.querySelector("[data-action=undo]");
var savePNGButton = wrapper.querySelector("[data-action=save-png]");
var saveJPGButton = wrapper.querySelector("[data-action=save-jpg]");
var saveSVGButton = wrapper.querySelector("[data-action=save-svg]");
var canvas = document.getElementById("drawcanvas");

var mycanvas = document.getElementById("printcanvas");
var context = mycanvas.getContext('2d');
var signaturePad = new SignaturePad(canvas, {
    // It's Necessary to use an opaque color when saving image as JPEG;
    // this option can be omitted if only saving as PNG or SVG
    backgroundColor: 'rgb(255, 255, 255)'
});

// Adjust canvas coordinate space taking into account pixel ratio,
// to make it look crisp on mobile devices.
// This also causes canvas to be cleared.
function resizeCanvas() {

    // When zoomed out to less than 100%, for some very strange reason,
    // some browsers report devicePixelRatio as less than 1
    // and only part of the canvas is cleared then.
    var ratio = Math.max(window.devicePixelRatio || 1, 1);

    //This part causes the canvas to be cleared
    canvas.width = canvas.offsetWidth * ratio;
    canvas.height = canvas.offsetHeight * ratio;
    //canvas.height = canvas.offsetHeight * ratio;
    canvas.getContext("2d").scale(ratio, ratio);

    // This library does not listen for canvas changes, so after the canvas is automatically
    // cleared by the browser, SignaturePad#isEmpty might still return false, even though the
    // canvas looks empty, because the internal data of this library wasn't cleared. To make sure
    // that the state of this library is consistent with visual state of the canvas, you
    // have to clear it manually.
    signaturePad.clear();
}

// On mobile devices it might make more sense to listen to orientation change,
// rather than window resize events.
window.onresize = resizeCanvas;
resizeCanvas();

function download(dataURL, filename) {
    var blob = dataURLToBlob(dataURL);
    var url = window.URL.createObjectURL(blob);

    var a = document.createElement("a");
    a.style = "display: none";
    a.href = url;
    a.download = filename;

    document.body.appendChild(a);
    a.click();

    window.URL.revokeObjectURL(url);
}

function GetSign() {
    // alert("hello");
    if (signaturePad.isEmpty()) {
        alert("Please provide a signature first.");
    } else {
        $(".hdnSignImageBlock").css("display", "none");
        $("#printcanvas").css("display", "block");
        context.drawImage(canvas, 0, 0, 640, 480);
        var dataURL = signaturePad.toDataURL();
        console.log(dataURL);
        $("#sketch_data").val(dataURL);
        //alert(dataURL);
        // download(dataURL, "signature.png");
    }
}

// One could simply use Canvas#toBlob method instead, but it's just to show
// that it can be done using result of SignaturePad#toDataURL.
function dataURLToBlob(dataURL) {
    // Code taken from https://github.com/ebidel/filer.js
    var parts = dataURL.split(';base64,');
    var contentType = parts[0].split(":")[1];
    var raw = window.atob(parts[1]);
    var rawLength = raw.length;
    var uInt8Array = new Uint8Array(rawLength);

    for (var i = 0; i < rawLength; ++i) {
        uInt8Array[i] = raw.charCodeAt(i);
    }

    return new Blob([uInt8Array], { type: contentType });
}

clearButton.addEventListener("click", function (event) {
    signaturePad.clear();
    $("#printcanvas").css("display", "none");
    $(".hdnSignImageBlock").css("display", "none");
});

undoButton.addEventListener("click", function (event) {
    var data = signaturePad.toData();

    if (data) {
        data.pop(); // remove the last dot or line
        signaturePad.fromData(data);
    }
});

changeColorButton.addEventListener("click", function (event) {
    var r = Math.round(Math.random() * 255);
    var g = Math.round(Math.random() * 255);
    var b = Math.round(Math.random() * 255);
    var color = "rgb(" + r + "," + g + "," + b + ")";

    signaturePad.penColor = color;
});

savePNGButton.addEventListener("click", function (event) {
    if (signaturePad.isEmpty()) {
        alert("Please provide a signature first.");
    } else {
        var dataURL = signaturePad.toDataURL();
        download(dataURL, "signature.png");
    }
});

saveJPGButton.addEventListener("click", function (event) {
    if (signaturePad.isEmpty()) {
        alert("Please provide a signature first.");
    } else {
        var dataURL = signaturePad.toDataURL("image/jpeg");
        download(dataURL, "signature.jpg");
    }
});

saveSVGButton.addEventListener("click", function (event) {
    if (signaturePad.isEmpty()) {
        alert("Please provide a signature first.");
    } else {
        var dataURL = signaturePad.toDataURL('image/svg+xml');
        download(dataURL, "signature.svg");
    }
});

    $(document).ready(function () {
        //Making Acknowlege Section Green On load
        //if ($("#confirmcheck").prop("checked") == true) {
        //    $("#selected").css("background-color", "rgba(176, 255, 175, 0.88)");
        //} else {
        //    $("#selected").css("background-color", "#fff");
        //}

        //Code to make one checkbox checked at a Time starts
        $('.encapsulatecheck').on("change", function () {
            $('.encapsulatecheck').not(this).prop("checked", false);
        });
        //Code to make one checkbox checked at a Time Ends

        //$("#confirmcheck").on("change", function () {

        //    if ($("#confirmcheck").prop("checked") == true) {
        //        $("#selected").css("background-color", "rgba(176, 255, 175, 0.88)");
        //    } else {
        //        $("#selected").css("background-color", "#fff");
        //    }
        //});

    });


    $(window).resize(function (e) {
        resizeCanvas1();
        resizeCanvas();
    });

//Signature Code 2 Starts Here
var wrapper1 = document.getElementById("signature-pad1");
var clearButton1 = wrapper1.querySelector("[data-action=clear1]");
var changeColorButton1 = wrapper1.querySelector("[data-action=change-color1]");
var undoButton1 = wrapper1.querySelector("[data-action=undo1]");
var savePNGButton1 = wrapper1.querySelector("[data-action=save-png1]");
var saveJPGButton1 = wrapper1.querySelector("[data-action=save-jpg1]");
var saveSVGButton1 = wrapper1.querySelector("[data-action=save-svg1]");
var canvas1 = document.getElementById("drawcanvas1");

var mycanvas1 = document.getElementById("Canvas1");
var context1 = mycanvas1.getContext('2d');
var signaturePad1 = new SignaturePad(canvas1, {
    // It's Necessary to use an opaque color when saving image as JPEG;
    // this option can be omitted if only saving as PNG or SVG
    backgroundColor: 'rgb(255, 255, 255)'
});

// Adjust canvas coordinate space taking into account pixel ratio,
// to make it look crisp on mobile devices.
// This also causes canvas to be cleared.
function resizeCanvas1() {

    //alert("bye");

    // When zoomed out to less than 100%, for some very strange reason,
    // some browsers report devicePixelRatio as less than 1
    // and only part of the canvas is cleared then.
    var ratio = Math.max(window.devicePixelRatio || 1, 1);

    //This part causes the canvas to be cleared
    canvas1.width = canvas1.offsetWidth * ratio;
    canvas1.height = canvas1.offsetHeight * ratio;
    //canvas.height = canvas.offsetHeight * ratio;
    canvas1.getContext("2d").scale(ratio, ratio);

    // This library does not listen for canvas changes, so after the canvas is automatically
    // cleared by the browser, SignaturePad#isEmpty might still return false, even though the
    // canvas looks empty, because the internal data of this library wasn't cleared. To make sure
    // that the state of this library is consistent with visual state of the canvas, you
    // have to clear it manually.
    signaturePad1.clear();
}

// On mobile devices it might make more sense to listen to orientation change,
// rather than window resize events.
window.onresize = resizeCanvas1;
resizeCanvas1();


function download(dataURL, filename) {
    var blob = dataURLToBlob(dataURL);
    var url = window.URL.createObjectURL(blob);

    var a = document.createElement("a");
    a.style = "display: none";
    a.href = url;
    a.download = filename;

    document.body.appendChild(a);
    a.click();

    window.URL.revokeObjectURL(url);
}

function GetSignature() {
    //alert("hello");
    if (signaturePad1.isEmpty()) {
        alert("Please provide a signature first.");
    } else {
        $(".hdnSignImageBlock1").css("display", "none");
        $("#Canvas1").css("display", "block");
        context1.drawImage(canvas1, 0, 0, 640, 480);
        var dataURL1 = signaturePad1.toDataURL();

        $("#sketch_data1").val(dataURL1);
        //alert(dataURL);
        // download(dataURL, "signature.png");
    }
}

// One could simply use Canvas#toBlob method instead, but it's just to show
// that it can be done using result of SignaturePad#toDataURL.
function dataURLToBlob(dataURL) {
    // Code taken from https://github.com/ebidel/filer.js
    var parts = dataURL.split(';base64,');
    var contentType = parts[0].split(":")[1];
    var raw = window.atob(parts[1]);
    var rawLength = raw.length;
    var uInt8Array = new Uint8Array(rawLength);

    for (var i = 0; i < rawLength; ++i) {
        uInt8Array[i] = raw.charCodeAt(i);
    }

    return new Blob([uInt8Array], { type: contentType });
}

clearButton1.addEventListener("click", function (event) {
    signaturePad1.clear();
    $("#Canvas1").css("display", "none");
    $(".hdnSignImageBlock1").css("display", "none");
});

undoButton1.addEventListener("click", function (event) {
    var data = signaturePad1.toData();

    if (data) {
        data.pop(); // remove the last dot or line
        signaturePad1.fromData(data);
    }
});

changeColorButton1.addEventListener("click", function (event) {
    var r = Math.round(Math.random() * 255);
    var g = Math.round(Math.random() * 255);
    var b = Math.round(Math.random() * 255);
    var color = "rgb(" + r + "," + g + "," + b + ")";

    signaturePad.penColor = color;
});

savePNGButton1.addEventListener("click", function (event) {
    if (signaturePad1.isEmpty()) {
        alert("Please provide a signature first.");
    } else {
        var dataURL = signaturePad1.toDataURL();
        download(dataURL, "signature.png");
    }
});

saveJPGButton1.addEventListener("click", function (event) {
    if (signaturePad1.isEmpty()) {
        alert("Please provide a signature first.");
    } else {
        var dataURL = signaturePad1.toDataURL("image/jpeg");
        download(dataURL, "signature.jpg");
    }
});

saveSVGButton1.addEventListener("click", function (event) {
    if (signaturePad1.isEmpty()) {
        alert("Please provide a signature first.");
    } else {
        var dataURL = signaturePad1.toDataURL('image/svg+xml');
        download(dataURL, "signature.svg");
    }
});
//Signature Code 2 Ends Here


    $(window).resize(function (e) {
        resizeCanvas1();
        resizeCanvas2();
        resizeCanvas();
    });

//Signature Code 2 Starts Here
var wrapper2 = document.getElementById("signature-pad2");
var clearButton2 = wrapper2.querySelector("[data-action=clear2]");
var changeColorButton2 = wrapper2.querySelector("[data-action=change-color2]");
var undoButton2 = wrapper2.querySelector("[data-action=undo2]");
var savePNGButton2 = wrapper2.querySelector("[data-action=save-png2]");
var saveJPGButton2 = wrapper2.querySelector("[data-action=save-jpg2]");
var saveSVGButton2 = wrapper2.querySelector("[data-action=save-svg2]");
var canvas2 = document.getElementById("drawcanvas2");

var mycanvas2 = document.getElementById("Canvas2");
var context2 = mycanvas2.getContext('2d');
var signaturePad2 = new SignaturePad(canvas2, {
    // It's Necessary to use an opaque color when saving image as JPEG;
    // this option can be omitted if only saving as PNG or SVG
    backgroundColor: 'rgb(255, 255, 255)'
});

// Adjust canvas coordinate space taking into account pixel ratio,
// to make it look crisp on mobile devices.
// This also causes canvas to be cleared.
function resizeCanvas2() {

    //alert("bye");

    // When zoomed out to less than 100%, for some very strange reason,
    // some browsers report devicePixelRatio as less than 1
    // and only part of the canvas is cleared then.
    var ratio = Math.max(window.devicePixelRatio || 1, 1);

    //This part causes the canvas to be cleared
    canvas2.width = canvas2.offsetWidth * ratio;
    canvas2.height = canvas2.offsetHeight * ratio;
    //canvas.height = canvas.offsetHeight * ratio;
    canvas2.getContext("2d").scale(ratio, ratio);

    // This library does not listen for canvas changes, so after the canvas is automatically
    // cleared by the browser, SignaturePad#isEmpty might still return false, even though the
    // canvas looks empty, because the internal data of this library wasn't cleared. To make sure
    // that the state of this library is consistent with visual state of the canvas, you
    // have to clear it manually.
    signaturePad2.clear();
}

// On mobile devices it might make more sense to listen to orientation change,
// rather than window resize events.
window.onresize = resizeCanvas2;
resizeCanvas2();


function download(dataURL, filename) {
    var blob = dataURLToBlob(dataURL);
    var url = window.URL.createObjectURL(blob);

    var a = document.createElement("a");
    a.style = "display: none";
    a.href = url;
    a.download = filename;

    document.body.appendChild(a);
    a.click();

    window.URL.revokeObjectURL(url);
}

function GetSignature2() {
    //alert("hello");
    if (signaturePad2.isEmpty()) {
        alert("Please provide a signature first.");
    } else {
        $(".hdnSignImageBlock2").css("display", "none");
        $("#Canvas2").css("display", "block");
        context2.drawImage(canvas2, 0, 0, 640, 480);
        var dataURL2 = signaturePad2.toDataURL();
        //alert(dataURL2);
        $("#sketch_data2").val(dataURL2);
        //alert(dataURL);
        // download(dataURL, "signature.png");
        var currentDate = new Date();
        var months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul",
            "Aug", "Sep", "Oct", "Nov", "Dec"];
        var dd = currentDate.getDate();
        if (dd < 10) {
            dd = '0' + dd;
        }
        var date = dd + '-' + months[currentDate.getMonth()] + '-' + currentDate.getFullYear();
        var datereceived = $("#Datepicker").val();
        //alert(daterecived);
        if (datereceived == "") {
            $("#Datepicker").val(date);
        }
        $("#hdnfld").val(date);
    }
}

// One could simply use Canvas#toBlob method instead, but it's just to show
// that it can be done using result of SignaturePad#toDataURL.
function dataURLToBlob(dataURL) {
    // Code taken from https://github.com/ebidel/filer.js
    var parts = dataURL.split(';base64,');
    var contentType = parts[0].split(":")[1];
    var raw = window.atob(parts[1]);
    var rawLength = raw.length;
    var uInt8Array = new Uint8Array(rawLength);

    for (var i = 0; i < rawLength; ++i) {
        uInt8Array[i] = raw.charCodeAt(i);
    }

    return new Blob([uInt8Array], { type: contentType });
}

clearButton2.addEventListener("click", function (event) {
    signaturePad2.clear();
    $("#Canvas2").css("display", "none");
    $(".hdnSignImageBlock2").css("display", "none");
});

undoButton2.addEventListener("click", function (event) {
    var data = signaturePad2.toData();

    if (data) {
        data.pop(); // remove the last dot or line
        signaturePad2.fromData(data);
    }
});

changeColorButton2.addEventListener("click", function (event) {
    var r = Math.round(Math.random() * 255);
    var g = Math.round(Math.random() * 255);
    var b = Math.round(Math.random() * 255);
    var color = "rgb(" + r + "," + g + "," + b + ")";

    signaturePad.penColor = color;
});

savePNGButton2.addEventListener("click", function (event) {
    if (signaturePad2.isEmpty()) {
        alert("Please provide a signature first.");
    } else {
        var dataURL = signaturePad2.toDataURL();
        download(dataURL, "signature.png");
    }
});

saveJPGButton2.addEventListener("click", function (event) {
    if (signaturePad2.isEmpty()) {
        alert("Please provide a signature first.");
    } else {
        var dataURL = signaturePad2.toDataURL("image/jpeg");
        download(dataURL, "signature.jpg");
    }
});

saveSVGButton2.addEventListener("click", function (event) {
    if (signaturePad2.isEmpty()) {
        alert("Please provide a signature first.");
    } else {
        var dataURL = signaturePad2.toDataURL('image/svg+xml');
        download(dataURL, "signature.svg");
    }
});
//Signature Code 2 Ends Here


    $(window).resize(function (e) {
        resizeCanvas1();
        resizeCanvas2();
        resizeCanvas3();
        resizeCanvas();
    });

//Signature Code 2 Starts Here
var wrapper3 = document.getElementById("signature-pad3");
var clearButton3 = wrapper3.querySelector("[data-action=clear3]");
var changeColorButton3 = wrapper3.querySelector("[data-action=change-color3]");
var undoButton3 = wrapper3.querySelector("[data-action=undo3]");
var savePNGButton3 = wrapper3.querySelector("[data-action=save-png3]");
var saveJPGButton3 = wrapper3.querySelector("[data-action=save-jpg3]");
var saveSVGButton3 = wrapper3.querySelector("[data-action=save-svg3]");
var canvas3 = document.getElementById("drawcanvas3");

var mycanvas3 = document.getElementById("Canvas3");
var context3 = mycanvas3.getContext('2d');
var signaturePad3 = new SignaturePad(canvas3, {
    // It's Necessary to use an opaque color when saving image as JPEG;
    // this option can be omitted if only saving as PNG or SVG
    backgroundColor: 'rgb(255, 255, 255)'
});

// Adjust canvas coordinate space taking into account pixel ratio,
// to make it look crisp on mobile devices.
// This also causes canvas to be cleared.
function resizeCanvas3() {

    //alert("bye");

    // When zoomed out to less than 100%, for some very strange reason,
    // some browsers report devicePixelRatio as less than 1
    // and only part of the canvas is cleared then.
    var ratio = Math.max(window.devicePixelRatio || 1, 1);

    //This part causes the canvas to be cleared
    canvas3.width = canvas3.offsetWidth * ratio;
    canvas3.height = canvas3.offsetHeight * ratio;
    //canvas.height = canvas.offsetHeight * ratio;
    canvas3.getContext("2d").scale(ratio, ratio);

    // This library does not listen for canvas changes, so after the canvas is automatically
    // cleared by the browser, SignaturePad#isEmpty might still return false, even though the
    // canvas looks empty, because the internal data of this library wasn't cleared. To make sure
    // that the state of this library is consistent with visual state of the canvas, you
    // have to clear it manually.
    signaturePad3.clear();
}

// On mobile devices it might make more sense to listen to orientation change,
// rather than window resize events.
window.onresize = resizeCanvas3;
resizeCanvas3();


function download(dataURL, filename) {
    var blob = dataURLToBlob(dataURL);
    var url = window.URL.createObjectURL(blob);

    var a = document.createElement("a");
    a.style = "display: none";
    a.href = url;
    a.download = filename;

    document.body.appendChild(a);
    a.click();

    window.URL.revokeObjectURL(url);
}

function GetSignature3() {
    //alert("hello");
    if (signaturePad3.isEmpty()) {
        alert("Please provide a signature first.");
    } else {
        $(".hdnSignImageBlock3").css("display", "none");
        $("#Canvas3").css("display", "block");
        context3.drawImage(canvas3, 0, 0, 640, 480);
        var dataURL3 = signaturePad3.toDataURL();

        $("#sketch_data3").val(dataURL3);
        $("#chkpopup").css("display", "block");
    }
}

// One could simply use Canvas#toBlob method instead, but it's just to show
// that it can be done using result of SignaturePad#toDataURL.
function dataURLToBlob(dataURL) {
    // Code taken from https://github.com/ebidel/filer.js
    var parts = dataURL.split(';base64,');
    var contentType = parts[0].split(":")[1];
    var raw = window.atob(parts[1]);
    var rawLength = raw.length;
    var uInt8Array = new Uint8Array(rawLength);

    for (var i = 0; i < rawLength; ++i) {
        uInt8Array[i] = raw.charCodeAt(i);
    }

    return new Blob([uInt8Array], { type: contentType });
}

clearButton3.addEventListener("click", function (event) {
    signaturePad3.clear();
    $("#Canvas3").css("display", "none");
    $(".hdnSignImageBlock3").css("display", "none");
});

undoButton3.addEventListener("click", function (event) {
    var data = signaturePad3.toData();

    if (data) {
        data.pop(); // remove the last dot or line
        signaturePad3.fromData(data);
    }
});

changeColorButton3.addEventListener("click", function (event) {
    var r = Math.round(Math.random() * 255);
    var g = Math.round(Math.random() * 255);
    var b = Math.round(Math.random() * 255);
    var color = "rgb(" + r + "," + g + "," + b + ")";

    signaturePad.penColor = color;
});

savePNGButton3.addEventListener("click", function (event) {
    if (signaturePad3.isEmpty()) {
        alert("Please provide a signature first.");
    } else {
        var dataURL = signaturePad3.toDataURL();
        download(dataURL, "signature.png");
    }
});

saveJPGButton3.addEventListener("click", function (event) {
    if (signaturePad3.isEmpty()) {
        alert("Please provide a signature first.");
    } else {
        var dataURL = signaturePad3.toDataURL("image/jpeg");
        download(dataURL, "signature.jpg");
    }
});

saveSVGButton3.addEventListener("click", function (event) {
    if (signaturePad3.isEmpty()) {
        alert("Please provide a signature first.");
    } else {
        var dataURL = signaturePad3.toDataURL('image/svg+xml');
        download(dataURL, "signature.svg");
    }
});
//Signature Code 2 Ends Here


function chkpopupyes_Click()
{
    $('#confirmcheck').prop('checked',true);
    //   confirmcheck.Checked = true;
    $('#chkpopup').css('display','none');
    //chkpopup.Style.Add("display","none");
}

function chkpopupno_Click()
{
       // chkpopup.Style.Add("display", "none");
        $('#chkpopup').css('display','none');
}

function IsTransactionInactive() {
    var siteName = $('#lblsitename').val();

    if (siteName && siteName.trim().endsWith("(InActive)")) {
        $('#TopSubmit').hide();
        $('#btnSubmit').hide();
    }
}


