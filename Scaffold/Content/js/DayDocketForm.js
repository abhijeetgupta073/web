  
$(document).ready(function () {

	GetDeliveryDocketData();
	showData();

});

function GetDeliveryDocketData() {
	var SiteId = getParameterByName('SiteID');
	var EntryDate = getParameterByName('EntryDate');

	$.ajax({
		type: "POST",
		url: "/DayDocketForm/TableData",

		data: JSON.stringify({ SiteId: SiteId, EntryDate: EntryDate }),
		contentType: "application/json; charset=utf-8",
		dataType: "json",
		async: false,
		success: function (response) {

			var GetData = response.html;
			$('#PlaceHolder1').empty().append(GetData);

		}
	});
}

function showData() {
	var SiteId = getParameterByName('SiteID');
	var EntryDate = getParameterByName('EntryDate');
	var DayDock = getParameterByName('DayDock');
	var ClientId = getParameterByName('id');
	$.ajax({
		type: "POST",
		url: "/DayDocketForm/showData",

		data: JSON.stringify({ SiteId: SiteId, EntryDate: EntryDate, DayDock: DayDock, id: ClientId }),
		contentType: "application/json; charset=utf-8",
		dataType: "json",
		async: false,
		success: function (response) {

			var GetData = response.GetAllData;

			var SupervisorSign = GetData.SupervisorSign == null ? "" : GetData.SupervisorSign;
			var ClientSign = GetData.ClientSign == null ? "" : GetData.ClientSign;
			var EntryDate = GetData.EntryDate == null ? "" : GetData.EntryDate;
			var SiteRef = GetData.SiteRef == null ? "" : GetData.SiteRef;
			var SiteLocation = GetData.SiteLocation == null ? "" : GetData.SiteLocation;
			var Address = GetData.Address == null ? "" : GetData.Address;
			var City = GetData.City == null ? false : GetData.City;
			var Comments = GetData.Comments == null ? "" : GetData.Comments;
			var State = GetData.State == null ? "" : GetData.State;
			var CellPhone = GetData.CellPhone == null ? "" : GetData.CellPhone;
			var ZIP = GetData.ZIP == null ? "" : GetData.ZIP;
			var Name = GetData.Name == null ? "" : GetData.Name;
			var CompanyRep = GetData.CompanyRep == null ? "" : GetData.CompanyRep;
			var ClientRep = GetData.ClientRep == null ? "" : GetData.ClientRep;
			var ClientSignDate = GetData.ClientSignDate == null ? "" : DateFormat(GetData.ClientSignDate);


			$('#lblSiteRef').text(SiteRef);
			$('#lblSiteLocation').text(SiteLocation);
			$('#lblDate1').text(EntryDate);
			$('#lblAddress').text(Address);
			if (City != "") {
				$('#lblCity').text(City);
				$('#lblComma').css('display', 'block');
			}
			else {
				$('#lblComma').css('display', 'none');
			}
			$('#txtcomment').val(Comments);
			$('#lblState').text(State);
			if (CellPhone != "") {
				$('#lblCellPhone').text(CellPhone);
				$('#lblPh').css('display', 'block');
			}
			else {
				$('#lblPh').css('display', 'none');
			}

			$('#lblzip').text(ZIP);
			$('#lblClient').text(Name);
			$('#txtsupervisor').val(CompanyRep);
			$('#txtclient').val(ClientRep);
			if (ClientSignDate != "") {
				$('#datepicketID').val(ClientSignDate);
			}
			if (SupervisorSign != "") {
				$('#signimga').attr("src", SupervisorSign);	
				$('#printcanvas').css('display', 'none');
				$('#hdnSignImageBlock2').css('display', 'block'); 
			}
			
			if (ClientSign != "") {
				$('#signimg2').attr("src", ClientSign);
				$('#ClientSign_Flag').val(ClientSign);
				$('#Canvas1').css('display', 'none');
				$('#hdnSignImageBlock1').css('display', 'block');
			}
			else {
				$('#ClientSign_Flag').val('');
			}

		}
	});
}

function btnSave_Click() {
	$('#LoadingPanel1').show();

	var UID = getParameterByName('UID');
	var UserRole = getParameterByName('UserRole');
	var DayDock = getParameterByName('DayDock');
	var ClientID = getParameterByName('id');

	var sketchData = $('#sketch_data').val();
	var sketchData1 = $('#sketch_data1').val();

	var txtsupervisor = $('#txtsupervisor').val();
	var txtclient = $('#txtclient').val();
	var ClientSignDate = $('#datepicketID').val();
	var txtcomment = $('#txtcomment').val();



	$.ajax({
		type: "POST",
		url: "/DayDocketForm/btnSave_Click",

		data: JSON.stringify({ DayDock: DayDock, sketchData: sketchData, sketchData1: sketchData1, txtsupervisor: txtsupervisor, txtclient: txtclient, ClientSignDate: ClientSignDate, txtcomment: txtcomment }),
		contentType: "application/json; charset=utf-8",
		dataType: "json",
		async: true,
		success: function (response) {

			var GetData = response;
			if (response > 0) {

				GetDeliveryDocketData();
				showData();
				var ClientSign_Flag = $('#ClientSign_Flag').val();
				if (ClientSign_Flag != "")
				{
					//Response.Redirect("DayDocket?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"]);
					window.location.href = '/DayDocket/DayDocket?UID=' + UID + '&UserRole=' + UserRole + '&id=' + ClientID;
				}
				else {

					//var Response = { Result: "Success", Message: "Data Saved Successfully." }
					//CustomAlert(Response);
					$('#saveContinueModal').css('display', 'Block');
					$('#LoadingPanel1').hide();
					//location.reload();

				}

			} else {

				$('#LoadingPanel1').hide();
				CustomAlert(response);
			}
		}
	});
}

function btnsavecontinue_Click() {
	$('#LoadingPanel1').show();
	$('#saveContinueModal').css("display", "none");
	var id = getParameterByName('id');
	var UID = getParameterByName('UID');
	var UserRole = getParameterByName('UserRole');
	var SiteId = getParameterByName('SiteID');
	var EntryDate = getParameterByName('EntryDate');
	var DayDock = getParameterByName('DayDock');

	window.location.href = '/DayDocketForm/DayDocketForm?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&DayDock=' + DayDock + '&SiteID=' + SiteId + '&EntryDate=' + EntryDate;
}

function backtoDayDocket_ServerClick() {
	$('#LoadingPanel1').show();
	var id = getParameterByName('id');
	var UID = getParameterByName('UID');
	var UserRole = getParameterByName('UserRole');

	window.location.href = '/DayDocket/DayDocket?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id ;
	//Response.Redirect("DayDocket?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"]);
}

//function SaveSketch() {
//	//alert("hello");
//	var mycanvas = document.getElementById('colors_sketch');
//	var canvas1 = document.getElementById('canvas1234');
//	//canvas1.restore();
//	var context1 = canvas1.getContext('2d');

//	var dataUri = $("#colors_sketch")[0].toDataURL();
//	$("#sketch_data").val(dataUri);

//	//$(".hdnAudiobefore").css("display", "block");
//	$("#sigModal .modal-header .close").click();

//	$(".hdnSignaturebefore").css("display", "block");

//	context1.drawImage(mycanvas, 0, 0, 640, 480);
//};

//$(document).ready(function () {
//	$("#getSign").on("click", function () {
//		$(".hdnSignaturebefore").css("display", "none");
//		var canvas = document.getElementById('canvas1234'),
//		 ctx = canvas.getContext("2d");
//		ctx.clearRect(0, 0, 640, 480);

//		$("#btnSigModal").click();
//	});
//});



//$(document).ready(function () {
//	$("#getSign1").on("click", function () {
//		// $("#btnOpenModel4").click();
//		$("#btnsignModal1").click();
//	});
//});

//function SaveSketch() {
//	//alert("hello");
//	var mycanvas = document.getElementById('colors_sketch');
//	var canvas1 = document.getElementById('canvas12345');
//	//canvas1.restore();
//	var context = canvas1.getContext('2d');

//	var dataUri = $("#colors_sketch")[0].toDataURL();
//	$("#sketch_data1").val(dataUri);

//	//$(".hdnAudiobefore").css("display", "block");
//	$("#sigModal .modal-header .close").click();

//	$(".hdnSignaturebefore").css("display", "block");

//	context.drawImage(mycanvas, 0, 0, 640, 480);
//};

//$(document).ready(function () {
//	$("#getSign1").on("click", function () {
//		$(".hdnSignaturebefore").css("display", "none");
//		var canvas = document.getElementById('canvas12345'),
//		 ctx = canvas.getContext("2d");
//		ctx.clearRect(0, 0, 640, 480);

//		$("#btnSigModal1").click();
//	});
//});


//var wrapper = document.getElementById("signature-pad");
//var clearButton = wrapper.querySelector("[data-action=clear]");
//var changeColorButton = wrapper.querySelector("[data-action=change-color]");
//var undoButton = wrapper.querySelector("[data-action=undo]");
//var savePNGButton = wrapper.querySelector("[data-action=save-png]");
//var saveJPGButton = wrapper.querySelector("[data-action=save-jpg]");
//var saveSVGButton = wrapper.querySelector("[data-action=save-svg]");
//var canvas = document.getElementById("drawcanvas");

//var mycanvas = document.getElementById("printcanvas");
//var context = mycanvas.getContext('2d');
//var signaturePad = new SignaturePad(canvas, {
//	// It's Necessary to use an opaque color when saving image as JPEG;
//	// this option can be omitted if only saving as PNG or SVG
//	backgroundColor: 'rgb(255, 255, 255)'
//});

//// Adjust canvas coordinate space taking into account pixel ratio,
//// to make it look crisp on mobile devices.
//// This also causes canvas to be cleared.
//function resizeCanvas() {

//	// When zoomed out to less than 100%, for some very strange reason,
//	// some browsers report devicePixelRatio as less than 1
//	// and only part of the canvas is cleared then.
//	var ratio = Math.max(window.devicePixelRatio || 1, 1);

//	//This part causes the canvas to be cleared
//	canvas.width = canvas.offsetWidth * ratio;
//	canvas.height = canvas.offsetHeight * ratio;
//	//canvas.height = canvas.offsetHeight * ratio;
//	canvas.getContext("2d").scale(ratio, ratio);

//	// This library does not listen for canvas changes, so after the canvas is automatically
//	// cleared by the browser, SignaturePad#isEmpty might still return false, even though the
//	// canvas looks empty, because the internal data of this library wasn't cleared. To make sure
//	// that the state of this library is consistent with visual state of the canvas, you
//	// have to clear it manually.
//	signaturePad.clear();
//}

//// On mobile devices it might make more sense to listen to orientation change,
//// rather than window resize events.
//window.onresize = resizeCanvas;
//resizeCanvas();

//function download(dataURL, filename) {
//	var blob = dataURLToBlob(dataURL);
//	var url = window.URL.createObjectURL(blob);

//	var a = document.createElement("a");
//	a.style = "display: none";
//	a.href = url;
//	a.download = filename;

//	document.body.appendChild(a);
//	a.click();

//	window.URL.revokeObjectURL(url);
//}

//function GetSign() {
//	// alert("hello");
//	if (signaturePad.isEmpty()) {
//		alert("Please provide a signature first.");
//	} else {
//		$(".hdnSignImageBlock2").css("display", "none");
//		$("#printcanvas").css("display", "block");
//		context.drawImage(canvas, 0, 0, 640, 480);
//		var dataURL = signaturePad.toDataURL();
//		console.log(dataURL);
//		$("#sketch_data").val(dataURL);
//		//alert(dataURL);
//		// download(dataURL, "signature.png");
//	}
//}

//// One could simply use Canvas#toBlob method instead, but it's just to show
//// that it can be done using result of SignaturePad#toDataURL.
//function dataURLToBlob(dataURL) {
//	// Code taken from https://github.com/ebidel/filer.js
//	var parts = dataURL.split(';base64,');
//	var contentType = parts[0].split(":")[1];
//	var raw = window.atob(parts[1]);
//	var rawLength = raw.length;
//	var uInt8Array = new Uint8Array(rawLength);

//	for (var i = 0; i < rawLength; ++i) {
//		uInt8Array[i] = raw.charCodeAt(i);
//	}

//	return new Blob([uInt8Array], { type: contentType });
//}

//clearButton.addEventListener("click", function (event) {
//	signaturePad.clear();
//	$("#printcanvas").css("display", "none");
//	$(".hdnSignImageBlock").css("display", "none");
//});

//undoButton.addEventListener("click", function (event) {
//	var data = signaturePad.toData();

//	if (data) {
//		data.pop(); // remove the last dot or line
//		signaturePad.fromData(data);
//	}
//});

//changeColorButton.addEventListener("click", function (event) {
//	var r = Math.round(Math.random() * 255);
//	var g = Math.round(Math.random() * 255);
//	var b = Math.round(Math.random() * 255);
//	var color = "rgb(" + r + "," + g + "," + b + ")";

//	signaturePad.penColor = color;
//});

//savePNGButton.addEventListener("click", function (event) {
//	if (signaturePad.isEmpty()) {
//		alert("Please provide a signature first.");
//	} else {
//		var dataURL = signaturePad.toDataURL();
//		download(dataURL, "signature.png");
//	}
//});

//saveJPGButton.addEventListener("click", function (event) {
//	if (signaturePad.isEmpty()) {
//		alert("Please provide a signature first.");
//	} else {
//		var dataURL = signaturePad.toDataURL("image/jpeg");
//		download(dataURL, "signature.jpg");
//	}
//});

//saveSVGButton.addEventListener("click", function (event) {
//	if (signaturePad.isEmpty()) {
//		alert("Please provide a signature first.");
//	} else {
//		var dataURL = signaturePad.toDataURL('image/svg+xml');
//		download(dataURL, "signature.svg");
//	}
//});

//$(window).resize(function (e) {
//	resizeCanvas1();
//	resizeCanvas();
//});

////Signature Code 2 Starts Here
//var wrapper1 = document.getElementById("signature-pad1");
//var clearButton1 = wrapper1.querySelector("[data-action=clear1]");
//var changeColorButton1 = wrapper1.querySelector("[data-action=change-color1]");
//var undoButton1 = wrapper1.querySelector("[data-action=undo1]");
//var savePNGButton1 = wrapper1.querySelector("[data-action=save-png1]");
//var saveJPGButton1 = wrapper1.querySelector("[data-action=save-jpg1]");
//var saveSVGButton1 = wrapper1.querySelector("[data-action=save-svg1]");
//var canvas1 = document.getElementById("drawcanvas1");

//var mycanvas1 = document.getElementById("Canvas1");
//var context1 = mycanvas1.getContext('2d');
//var signaturePad1 = new SignaturePad(canvas1, {
//	// It's Necessary to use an opaque color when saving image as JPEG;
//	// this option can be omitted if only saving as PNG or SVG
//	backgroundColor: 'rgb(255, 255, 255)'
//});

//// Adjust canvas coordinate space taking into account pixel ratio,
//// to make it look crisp on mobile devices.
//// This also causes canvas to be cleared.
//function resizeCanvas1() {

//	//alert("bye");

//	// When zoomed out to less than 100%, for some very strange reason,
//	// some browsers report devicePixelRatio as less than 1
//	// and only part of the canvas is cleared then.
//	var ratio = Math.max(window.devicePixelRatio || 1, 1);

//	//This part causes the canvas to be cleared
//	canvas1.width = canvas1.offsetWidth * ratio;
//	canvas1.height = canvas1.offsetHeight * ratio;
//	//canvas.height = canvas.offsetHeight * ratio;
//	canvas1.getContext("2d").scale(ratio, ratio);

//	// This library does not listen for canvas changes, so after the canvas is automatically
//	// cleared by the browser, SignaturePad#isEmpty might still return false, even though the
//	// canvas looks empty, because the internal data of this library wasn't cleared. To make sure
//	// that the state of this library is consistent with visual state of the canvas, you
//	// have to clear it manually.
//	signaturePad1.clear();
//}

//// On mobile devices it might make more sense to listen to orientation change,
//// rather than window resize events.
//window.onresize = resizeCanvas1;
//resizeCanvas1();


//function download(dataURL, filename) {
//	var blob = dataURLToBlob(dataURL);
//	var url = window.URL.createObjectURL(blob);

//	var a = document.createElement("a");
//	a.style = "display: none";
//	a.href = url;
//	a.download = filename;

//	document.body.appendChild(a);
//	a.click();

//	window.URL.revokeObjectURL(url);
//}

//function GetSignature() {
//	//alert("hello");
//	if (signaturePad1.isEmpty()) {
//		alert("Please provide a signature first.");
//	} else {
//		$(".hdnSignImageBlock1").css("display", "none");
//		$("#Canvas1").css("display", "block");
//		context1.drawImage(canvas1, 0, 0, 640, 480);
//		var dataURL1 = signaturePad1.toDataURL();

//		$("#sketch_data1").val(dataURL1);
//		//alert(dataURL);
//		// download(dataURL, "signature.png");
//	}
//}

//// One could simply use Canvas#toBlob method instead, but it's just to show
//// that it can be done using result of SignaturePad#toDataURL.
//function dataURLToBlob(dataURL) {
//	// Code taken from https://github.com/ebidel/filer.js
//	var parts = dataURL.split(';base64,');
//	var contentType = parts[0].split(":")[1];
//	var raw = window.atob(parts[1]);
//	var rawLength = raw.length;
//	var uInt8Array = new Uint8Array(rawLength);

//	for (var i = 0; i < rawLength; ++i) {
//		uInt8Array[i] = raw.charCodeAt(i);
//	}

//	return new Blob([uInt8Array], { type: contentType });
//}

//clearButton1.addEventListener("click", function (event) {
//	signaturePad1.clear();
//	$("#Canvas1").css("display", "none");
//	$(".hdnSignImageBlock1").css("display", "none");
//});

//undoButton1.addEventListener("click", function (event) {
//	var data = signaturePad1.toData();

//	if (data) {
//		data.pop(); // remove the last dot or line
//		signaturePad1.fromData(data);
//	}
//});

//changeColorButton1.addEventListener("click", function (event) {
//	var r = Math.round(Math.random() * 255);
//	var g = Math.round(Math.random() * 255);
//	var b = Math.round(Math.random() * 255);
//	var color = "rgb(" + r + "," + g + "," + b + ")";

//	signaturePad.penColor = color;
//});

//savePNGButton1.addEventListener("click", function (event) {
//	if (signaturePad1.isEmpty()) {
//		alert("Please provide a signature first.");
//	} else {
//		var dataURL = signaturePad1.toDataURL();
//		download(dataURL, "signature.png");
//	}
//});

//saveJPGButton1.addEventListener("click", function (event) {
//	if (signaturePad1.isEmpty()) {
//		alert("Please provide a signature first.");
//	} else {
//		var dataURL = signaturePad1.toDataURL("image/jpeg");
//		download(dataURL, "signature.jpg");
//	}
//});

//saveSVGButton1.addEventListener("click", function (event) {
//	if (signaturePad1.isEmpty()) {
//		alert("Please provide a signature first.");
//	} else {
//		var dataURL = signaturePad1.toDataURL('image/svg+xml');
//		download(dataURL, "signature.svg");
//	}
//});
////Signature Code 2 Ends Here