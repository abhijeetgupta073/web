var GetclientId; var dbName = "scaffpac Job Register";
var dbVersion = 70;
var db;


$(document).ready(function () {
    // Attach the change event on page load
    $('#confirmcheck').change(function () {
        if (this.checked) {
            $(this).val(true); // Set value to true
        } else {
            $(this).val(false); // Set value to false
        }
        console.log($(this).val()); // For debugging purposes
    });

});
function btnConfirm_Click() {
    // Check if the inspection date is not empty
    if ($('#rdpInspectiondate').val() != "") {
        // Check if the confirm checkbox is checked
        if ($('#confirmcheck').prop('checked') == true) {
            var sketch_data = $('#sketch_data').val();
            var signimga = $('#signimga').attr('src');

            // Ensure either sketch_data or signimga has valid content
            if (sketch_data.trim() != "" || (signimga && signimga.trim() != "")) {
                // Display the delete div
                $('#divdelete').css("display", "block");
                // You can call InspStatusCheck here if needed
                // InspStatusCheck();
            } else {
                // Show an error message for missing inspector signature
                var Responses = { Result: "Failed", Message: "Please provide Inspector signature." };
                CustomAlert(Responses);
            }
        } else {
            // Display the div2 element
            $('#div2').css("display", "block");
        }
    } else {
        // Show an error message for missing acknowledgment date
        var Response = { Result: "Failed", Message: "Please Select Acknowledgment Date." };
        CustomAlert(Response);
    }
}

function submit_Click() {
    // ViewState["hdnCurrentDate"] = Request.Form["hdnCurrentDate"];

    //  string inspstatusCheck = Request.Form["inspstatusCheck"];
    //  ConfirmStatus = inspstatusCheck;
    //  string InspectionStatusModal = "";

    var inspstatusCheck = "";
    if ($('#chckcnfrm').prop('checked') == true) {
        inspstatusCheck = "Confirmed";
    }
    if ($('#outServiceCheck').prop('checked') == true) {
        inspstatusCheck = "Out of Service";
    }
    if (inspstatusCheck != "") {
        $('#divdelete').css("display", "none");
        $('#confirmModal').css("display", "block");

        //  ViewState["InspectionStatus"] = inspstatusCheck;
    }
    $('#lblInspectionstatusmodal').text(inspstatusCheck);
    $('#divdelete').css("display", "none");
}

function btnConfirmok_Click() {
    AddLocalOffInspectionData();
    $('#confirmModal').css("display", "none");
    $('#confirmContinueModal').css("display", "block");

    //lblInspectionstatusmodal.InnerText = InspectionStatusModal;
}


function btnconfirmContinue_Click() {
    $('#confirmContinueModal').css("display", "none");
    fetchInspectionData();
    location.reload();

}

function btnConfirmCancel_Click() {
    $('#confirmModal').css("display", "none");

}
function cancel_Click() {
    $('#divdelete').css("display", "none");
}
function ok_Click() {
    $('#div2').css("display", "none");
}
//function btnBacktoInspection_Click() {
//    $('#LoadingPanel1').show();

//    var id = getParameterByName('id');
//    var UID = getParameterByName('UID');
//    var UserRole = getParameterByName('UserRole');
//    var RID = getParameterByName('RID');
//    var Siteid = getParameterByName('Siteid');

//    window.location.href = 'OfflineRegister.html?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&SiteLocId=' + Siteid + '&RID=' + RID + "&activateTab=Inspection";    // Response.Redirect(DomainName + "/" + "InspectionList?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RID=" + Request.QueryString["RID"], false);
//}

function btnBacktoInspection_Click() {
    $('#LoadingPanel1').show();

    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var RID = getParameterByName('RID');
    var Siteid = getParameterByName('SiteLocId');
    var OfflineUserRole = getParameterByName('OfflineUserRole');

    /*window.location.href = 'OfflineRegister.html?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&SiteLocId=' + Siteid + '&RID=' + RID + "&activateTab=Inspection";   */ // Response.Redirect(DomainName + "/" + "InspectionList?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RID=" + Request.QueryString["RID"], false);

    window.location.href = 'OfflineRegister.html?UID=' + encodeURIComponent(UID) + '&UserRole=' + encodeURIComponent(UserRole) + '&OfflineUserRole=' + encodeURIComponent(OfflineUserRole) + '&id=' + encodeURIComponent(id) + '&SiteLocId=' + encodeURIComponent(Siteid) + '&RID=' + encodeURIComponent(RID) + "&activateTab=Inspection";
}


function btnBacktoRegister_ServerClick() {
    $('#LoadingPanel1').show();
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var GetOfflineStatus = getParameterByName('offlinestatus');
    var Siteid = getParameterByName('SiteLocId');
    var OfflineUserRole = getParameterByName('OfflineUserRole');
    var RID = getParameterByName('RID');

    /* window.location.href = 'OfflineRegisterList.html?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + "&offlinestatus=" + GetOfflineStatus + "&Siteid=" + Siteid;*/

    //window.location.href = 'OfflineRegisterList.html?UID=' + encodeURIComponent(UID) + '&UserRole=' + encodeURIComponent(UserRole) + '&id=' + encodeURIComponent(id) + "&offlinestatus=" + encodeURIComponent(GetOfflineStatus) + "&Siteid=" + encodeURIComponent(Siteid);
    window.location.href = 'OfflineRegister.html?UID=' + encodeURIComponent(UID) + '&UserRole=' + encodeURIComponent(UserRole) + '&OfflineUserRole=' + encodeURIComponent(OfflineUserRole) + '&id=' + encodeURIComponent(id) + '&SiteLocId=' + encodeURIComponent(Siteid) + '&RID=' + encodeURIComponent(RID);

}


//shubham add 




// Function to get data from IndexDB
async function fetchInspectionData() {
    var InspectionID = getParameterByName('InspID');

    /*const dbRequest = indexedDB.open("YourDatabaseName");*/
    var dbRequest = indexedDB.open(dbName, dbVersion);

    return new Promise((resolve, reject) => {
        dbRequest.onsuccess = function (event) {
            const db = event.target.result;
            const transaction = db.transaction(["tblInspectionDetails", "tblRegisterDetails", "tblSiteList"], "readonly");

            const tblInspectionDetails = transaction.objectStore("tblInspectionDetails");
            const tblRegisterDetails = transaction.objectStore("tblRegisterDetails");
            const tblSiteList = transaction.objectStore("tblSiteList");

            const inspectionRequest = tblInspectionDetails.get(parseInt(InspectionID, 10));

            inspectionRequest.onsuccess = function (event) {
                const inspection = event.target.result;

                if (!InspectionID) {
                    resolve(null);
                    return;
                }

                // Get related VW_TblRegister record
                /* const registerRequest = tblRegisterDetails.get(inspection.REGISTERId);*/
                const registerRequest = tblRegisterDetails.get(Number(inspection.RegisterID));
                /* var getRequest = objectStore.get(Number(RegisterID));*/
                //getRequest.onerror = function (event) {
                //    console.error("Error retrieving record:", event);
                //};

                registerRequest.onsuccess = function (event) {
                    const register = event.target.result;

                    if (!register) {
                        resolve(null);
                        return;
                    }


                    const siteLocationRequest = tblSiteList.get(register.SiteLocID);

                    siteLocationRequest.onsuccess = function (event) {
                        const siteLocation = event.target.result;


                        const result = {
                            CurrentDate: new Date().toLocaleDateString("en-GB").replace(/ /g, "-"),
                            InspectionTime: inspection.InspectionTime,
                            Comments: inspection.Comments,

                            InspectedById: inspection.InspectedById,
                            CompleteInsp: inspection.CompleteInsp,
                            InspStatus: inspection.InspStatus,
                            InspectedBy: inspection.InspectedBy,
                            InspectedById: inspection.InspectedById,
                            CreatedBy: inspection.CreatedBy,
                            CreatedOn: inspection.CreatedOn,
                            OnlineInsp: inspection.OnlineInsp,
                            Acknowledgment: inspection.Acknowledgment,
                            Lat: inspection.Lat,
                            Long: inspection.Long,
                            GPSAddress: inspection.GPSAddress,
                            Inspimage: inspection.Inspimage,
                            Inspimage2: inspection.Inspimage2,
                            PhotoAngle: inspection.PhotoAngle,
                            PhotoAngle1: inspection.PhotoAngle1,
                            InspAudio: inspection.InspAudio,
                            InspVideo: inspection.InspVideo,
                            InspSign: inspection.InspSign,
                            InspComments: inspection.InspComments,
                            MapImage: inspection.MapImage,
                            InspectionDate: inspection.InspectionDate,
                            AckDate:
                                inspection.AckDate === "1900-01-01"
                                    ? null
                                    : inspection.AckDate,
                            AcknowledgeTimeZone: inspection.AcknowledgeTimeZone,
                            // AckDateTime: inspection.AckDateTime,
                            AckDateTime: inspection.AckDateTime === "1900-01-01"
                                ? SetDateFormat(new Date())
                                : inspection.AckDateTime,
                            LicenceNo: inspection.LicenceNo,
                            ScaffRef: String(inspection.ScaffRef).padStart(6, "0"), // Equivalent of RIGHT + Pad
                            DesignType: register.DesignType,
                            SiteName: register.SiteLocation,
                            ShowRegisterID: register.ShowRegisterID,
                            OwnerName: siteLocation ? siteLocation.OwnerName : null,
                        };

                        resolve(result);
                        showData();
                    };

                    siteLocationRequest.onerror = function () {
                        reject("Error retrieving site location.");
                    };
                };

                registerRequest.onerror = function () {
                    reject("Error retrieving register.");
                };
            };

            inspectionRequest.onerror = function () {
                reject("Error retrieving inspection.");
            };
        };

        dbRequest.onerror = function () {
            reject("Failed to open database.");
        };
    });
}



$(function () {
    /* GetJoinedDataFromInspectionAndRegisterDetails();*/
    /* fetchDataAndRender();*/
    /* fetchInspectionList();*/

    setTimeout(fetchDataAndRender, 2000);
    /*  fetchChecklistContent();*/
    fetchInspectionData();
    populateAddInspectionFormBy();

});


//show form data function
let inspectionData = null; // Store the data globally or in a higher scope

//async function showData() {
//    $('#LoadingPanel1').show();
//    try {
//        // Check if data is already fetched
//        if (!inspectionData) {
//            inspectionData = await fetchInspectionData(); // Fetch only if not already done
//        }

//        if (!inspectionData) {
//            console.error("No data found to display.");
//            return;
//        }

//        // Populate the UI with the fetched data
//        /*  $('#lblInspectedDate').text(SetDateFormat(inspectionData.AckDateTime) || "N/A");*/

//        $('#lblInspectedDate').text(inspectionData.InspectionDate);
//        $('#lblSite').text(inspectionData.SiteName || "N/A");
//        $('#lblOwnerName').text(inspectionData.OwnerName || "N/A");
//        $('#lbltag').text(inspectionData.ShowRegisterID || "N/A");
//        $('#lbldesigntype').text(inspectionData.DesignType || "N/A");
//        $('#comment').val(inspectionData.InspComments || "");
//        $('#ddlInspectedBy').val(inspectionData.InspectedBy);
//        $('#ddlInspectedBy').val(inspectionData.InspectedById);
//        $('#rdpInspectiondate').val(inspectionData.AckDateTime || "");
//        var InspStatus = inspectionData.InspStatus || "";
//        var Inspimage = inspectionData.Inspimage || "";
//        var Inspimage2 = inspectionData.Inspimage2 || "";
//        var inspstcode = inspectionData.CompleteInsp || "";
//        $('#imga').attr("src", Inspimage ? Inspimage : "/images/noimg.jpg");
//        $('#imganew').attr("src", Inspimage2 ? Inspimage2 : "/images/noimg.jpg");


//        //if ($("#ddlInspectedBy option[value='" + inspectionData.InspectedById + "']").length === 0) {
//        //    $("#ddlInspectedBy").append(
//        //        $("<option>").val(inspectionData.InspectedById).text(inspectionData.InspectedBy)
//        //    );
//        //}
//        //$('#ddlInspectedBy').val(inspectionData.InspectedById);

//        if ($("#ddlInspectedBy option[value='" + inspectionData.InspectedBy + "']").length === 0) {
//            $("#ddlInspectedBy").append(
//                $("<option>").val(inspectionData.InspectedBy).text(inspectionData.InspectedBy)
//            );
//        }
//        $('#ddlInspectedBy').val(inspectionData.InspectedBy);


//        if (inspectionData.Acknowledgment === "true") {
//            $('#confirmcheck').prop('checked', true);
//            $("#selected").css("background-color", "rgba(176, 255, 175, 0.88)");
//        }
//        if (inspectionData.InspSign) {

//            $('#signimga').attr("src", inspectionData.InspSign);
//            $('#hdnSignImageBlock').css('display', 'block');
//            $('.signature-pad').css('display', 'none');
//        } else {

//            $('#hdnSignImageBlock').css('display', 'none');
//            $('.signature-pad').css('display', 'block');
//        }
//        if (inspectionData.PhotoAngle) {
//            $('#imgangle').val(inspectionData.PhotoAngle);
//            ImageRotate(inspectionData.PhotoAngle);
//        } else {
//            $('#imgangle').val('0');
//        }

//        if (inspectionData.PhotoAngle1) {
//            $('#imgangle1').val(inspectionData.PhotoAngle1);
//            ImageRotate1(inspectionData.PhotoAngle1);
//        } else {
//            $('#imgangle1').val('0');
//        }
//        if (Inspimage != "") {
//            $('#imga').attr("src", Inspimage);
//            $('#imga').attr("src", Inspimage);


//            SafariCheck(" + 1 + ", null, null);
//            $('#btnchngeimg').css('display', 'block');
//            $('#btnchngg').css('display', 'block');
//            $('#Photoblock').css('display', 'none');
//            $('#Imageblock').css('display', 'block');
//            $('#getCamera').css('display', 'none');
//        }

//        if (Inspimage2 != "") {
//            $('#imganew').attr("src", Inspimage2);
//            $('#imganew').attr("src", Inspimage2);


//            SafariCheck(" + 1 + ", null, null);
//            $('#btnchngeimg1').css('display', 'block');
//            $('#btnchngg1').css('display', 'block');
//            $('#PhotoblockNew').css('display', 'none');
//            $('#ImageblockNew').css('display', 'block');
//            $('#getCameraNew').css('display', 'none');
//        }
//        if (inspectionData.Inspimage) {
//            $('#imga').attr("src", inspectionData.Inspimage);
//            $('#btnchngeimg').css('display', 'block');
//        }

//        if (inspectionData.Inspimage2) {
//            $('#imganew').attr("src", inspectionData.Inspimage2);
//            $('#btnchngeimg1').css('display', 'block');
//        }

//        if (inspectionData.InspSign) {
//            $('#signimga').attr("src", inspectionData.InspSign);
//            $('#hdnSignImageBlock').css('display', 'block');
//        }

//        if (inspectionData.AckDate) {
//            $('#rdpInspectiondate').val(inspectionData.AckDate);
//        }

//        $('#ddlInspectedBy').val(inspectionData.InspectedBy);
//        $('#lbllicence').text(inspectionData.LicenceNo);

//        // Additional logic to handle inspection status
//        if (((InspStatus == "Confirmed") || (InspStatus == "Out of Service"))) {
//            $('#btnSave').css('display', 'none');
//            $('#btnConfirm').css('display', 'none');
//            $('#lockimg').css('display', 'block');
//            $('#getCamera').css('display', 'none');
//            $('#getCameraNew').css('display', 'none');
//            $('#getAudio').css('display', 'block');
//            $('#getVideo').css('display', 'block');
//            $('#comment').attr('disabled', true);
//            $('#confirmcheck').attr('disabled', true);
//            $('#rdpInspectiondate').attr('disabled', true);

//            $('#ddlInspectedBy').attr('disabled', true);
//            RemoveSignatureBlock();
//        }

//        var InspectionFormData = inspectionData.InspectionFormData;

//        $('#PlaceHolder1').empty().append(InspectionFormData);

//        //Making Only one checkbox checked for a particular Question
//        $('input[type="checkbox"]').on('change', function () {
//            $('input[name="' + this.name + '"]').not(this).prop('checked', false);
//        });


//        $('#LoadingPanel1').hide();

//    } catch (error) {
//        console.error("Error displaying data:", error);
//    }
//}


async function showData() {
    $('#LoadingPanel1').show();
    try {
        // Check if data is already fetched
        if (!inspectionData) {
            inspectionData = await fetchInspectionData(); // Fetch only if not already done
        }

        if (!inspectionData) {
            console.error("No data found to display.");
            return;
        }

        // Populate the UI with the fetched data
        $('#lblInspectedDate').text(inspectionData.InspectionDate);
        $('#lblSite').text(inspectionData.SiteName || "N/A");
        $('#lblOwnerName').text(inspectionData.OwnerName || "N/A");
        $('#lbltag').text(inspectionData.ShowRegisterID || "N/A");
        $('#lbldesigntype').text(inspectionData.DesignType || "");
        $('#comment').val(inspectionData.InspComments || "");
        $('#rdpInspectiondate').val(inspectionData.AckDateTime || "");

        // Ensure InspectedById and InspectedBy are correctly displayed
        if ($("#ddlInspectedBy option[value='" + inspectionData.InspectedById + "']").length === 0) {
            $("#ddlInspectedBy").append(
                $("<option>").val(inspectionData.InspectedById).text(inspectionData.InspectedBy)
            );
        }
        $('#ddlInspectedBy').val(inspectionData.InspectedById);

        var InspStatus = inspectionData.InspStatus || "";
        var Inspimage = inspectionData.Inspimage || "";
        var Inspimage2 = inspectionData.Inspimage2 || "";
        $('#imga').attr("src", Inspimage ? Inspimage : "/images/noimg.jpg");
        $('#imganew').attr("src", Inspimage2 ? Inspimage2 : "/images/noimg.jpg");
        $('#Image3').attr("src", Inspimage ? Inspimage : "/images/noimg.jpg");
        $('#Image4').attr("src", Inspimage2 ? Inspimage2 : "/images/noimg.jpg");

        if (inspectionData.Acknowledgment === "true") {
            $('#confirmcheck').prop('checked', true);
            $("#selected").css("background-color", "rgba(176, 255, 175, 0.88)");
        }

        if (inspectionData.InspSign) {
            $('#signimga').attr("src", inspectionData.InspSign);
            $('#hdnSignImageBlock').css('display', 'block');
            $('.signature-pad').css('display', 'none');
        } else {
            $('#hdnSignImageBlock').css('display', 'none');
            $('.signature-pad').css('display', 'block');
        }

        if (inspectionData.PhotoAngle) {
            $('#imgangle').val(inspectionData.PhotoAngle);
            ImageRotate(inspectionData.PhotoAngle);
        } else {
            $('#imgangle').val('0');
        }

        if (inspectionData.PhotoAngle1) {
            $('#imgangle1').val(inspectionData.PhotoAngle1);
            ImageRotate1(inspectionData.PhotoAngle1);
        } else {
            $('#imgangle1').val('0');
        }

        if (Inspimage !== "") {
            $('#imga').attr("src", Inspimage);
            $('#Image3').attr("src", Inspimage);
            SafariCheck(" + 1 + ", null, null);
            $('#btnchngeimg').css('display', 'block');
            $('#btnchngg').css('display', 'block');
            $('#Photoblock').css('display', 'none');
            $('#Imageblock').css('display', 'block');
            $('#getCamera').css('display', 'none');
            $('.custom-file-upload').css("display", "none");
        }

        if (Inspimage2 !== "") {
            $('#imganew').attr("src", Inspimage2);
            $('#Image4').attr("src", Inspimage2);
            SafariCheck(" + 1 + ", null, null);
            $('#btnchngeimg1').css('display', 'block');
            $('#btnchngg1').css('display', 'block');
            $('#PhotoblockNew').css('display', 'none');
            $('#ImageblockNew').css('display', 'block');
            $('#getCameraNew').css('display', 'none');
            $('.custom-file-uploadNew').css("display", "none");
        }

        if (inspectionData.Inspimage) {
            $('#imga').attr("src", inspectionData.Inspimage);
            $('#Image3').attr("src", inspectionData.Inspimage);
            $('#btnchngeimg').css('display', 'block');
        }

        if (inspectionData.Inspimage2) {
            $('#imganew').attr("src", inspectionData.Inspimage2);
            $('#Image4').attr("src", inspectionData.Inspimage2);
            $('#btnchngeimg1').css('display', 'block');
        }

        if (inspectionData.InspSign) {
            $('#signimga').attr("src", inspectionData.InspSign);
            $('#hdnSignImageBlock').css('display', 'block');
        }

        if (inspectionData.AckDate) {
            $('#rdpInspectiondate').val(inspectionData.AckDate);
        }

        $('#lbllicence').text(inspectionData.LicenceNo);

        // Additional logic to handle inspection status
        if (InspStatus === "Confirmed" || InspStatus === "Out of Service") {
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
            $('.custom-file-uploadNew').css("display", "none");
            $('.custom-file-upload').css("display", "none");
            RemoveSignatureBlock();

        }

        var InspectionFormData = inspectionData.InspectionFormData;
        $('#PlaceHolder1').empty().append(InspectionFormData);


        // Making only one checkbox checked for a particular question
        $('input[type="checkbox"]').on('change', function () {
            $('input[name="' + this.name + '"]').not(this).prop('checked', false);
        });

        $('#LoadingPanel1').hide();
    } catch (error) {
        console.error("Error displaying data:", error);
        $('#LoadingPanel1').hide();
    }
}

function RemoveSignatureBlock() {
    $(".signature-pad").css("display", "none");
}
//function SetDateFormat(date) {
//    if (date != null && date != undefined && date != "") {
//        date = date.toString();
//        var newDate = new Date(parseInt(date.substr(6)));
//        var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
//        var formattedDate =
//            ("0" + newDate.getDate()).slice(-2) + '-' +
//            monthNames[newDate.getMonth()] + '-' +
//            newDate.getFullYear();
//        return formattedDate.toString();
//    } else {
//        return "";
//    }
//}

//function SetDateFormat(date) {
//    if (date != null && date != undefined && date != "") {
//        // Check if 'date' is already a valid date string
//        var newDate = new Date(date);
//        if (isNaN(newDate)) {
//            console.error("Invalid date format:", date);
//            return ""; // Return an empty string for invalid dates
//        }

//        var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
//        var formattedDate =
//            ("0" + newDate.getDate()).slice(-2) + '-' +
//            monthNames[newDate.getMonth()] + '-' +
//            newDate.getFullYear();

//        return formattedDate.toString();
//    } else {
//        return "";
//    }
//}

function SetDateFormat(date) {
    if (date != null && date != undefined && date != "") {
        // Convert the input date into a proper Date object
        var newDate = new Date(date);

        // Validate if it's a proper date
        if (isNaN(newDate)) {
            console.error("Invalid date format:", date);
            return ""; // Return an empty string for invalid dates
        }

        // Month names array
        var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

        // Format date (dd-MMM-yyyy)
        var formattedDate =
            ("0" + newDate.getDate()).slice(-2) + '-' +
            monthNames[newDate.getMonth()] + '-' +
            newDate.getFullYear();

        // Format time (hh:mm AM/PM)
        var hours = newDate.getHours();
        var minutes = newDate.getMinutes();
        var ampm = hours >= 12 ? "PM" : "AM";
        hours = hours % 12;
        hours = hours ? hours : 12; // Handle midnight (0 hours)
        var formattedTime = ("0" + hours).slice(-2) + ":" + ("0" + minutes).slice(-2) + " " + ampm;

        // Combine date and time
        return formattedDate + " " + formattedTime;
    } else {
        return "";
    }
}




//function populateAddInspectionFormBy() {
//    /*  clearDropdown('#ddlInspectedBy'); */// Clear existing dropdown options
//    /* clearDropdown('#ddlInspecdeplank');*/
//    const dropdown = document.querySelector('#ddlInspectedBy');
//    /*const dropdown = document.querySelector('#ddlInspecdeplank');*/
//    const request = indexedDB.open(dbName, dbVersion);

//    request.onsuccess = function (event) {
//        const db = event.target.result;
//        const transaction = db.transaction('tblUserList', 'readonly');
//        const objectStore = transaction.objectStore('tblUserList');

//        objectStore.openCursor().onsuccess = function (event) {
//            const cursor = event.target.result;
//            if (cursor) {
//                const record = cursor.value;
//                const option = document.createElement('option');
//                option.value = record.UserId;
//                /*option.value = record.FullName;*/ // Set UserID as the value
//                option.text = record.FullName; // Display FullName in the dropdown
//                dropdown.appendChild(option);
//                cursor.continue();
//            }
//        };
//    };

//    request.onerror = function (event) {
//        console.error('Error opening database:', event.target.errorCode);
//        CustomAlert({ Result: "Failed", Message: "Failed to load inspection users." });
//    };
//}

function populateAddInspectionFormBy() {
    clearDropdown('#ddlInspectedBy'); // Clear existing dropdown options

    const dropdown = document.querySelector('#ddlInspectedBy');

    // Add the default "Select---" option
    const defaultOption = document.createElement('option');
    defaultOption.value = '';
    defaultOption.text = 'Select---';
    defaultOption.disabled = true;
    defaultOption.selected = true;
    dropdown.appendChild(defaultOption);

    // Get UID from the URL
    var Userid = parseInt(OffDecrypt(getParameterByName('UID')), 10);


    // Open IndexedDB
    const request = indexedDB.open(dbName, dbVersion);

    request.onsuccess = function (event) {
        const db = event.target.result;
        const transaction = db.transaction('tblUserList', 'readonly');
        const objectStore = transaction.objectStore('tblUserList');

        const records = [];

        objectStore.openCursor().onsuccess = function (event) {
            const cursor = event.target.result;
            if (cursor) {
                records.push(cursor.value);
                cursor.continue();
            } else {
                // Sort records alphabetically by FullName
                records.sort((a, b) => a.FullName.localeCompare(b.FullName));

                records.forEach(record => {
                    const option = document.createElement('option');
                    option.value = record.UserId;
                    option.text = record.FullName;

                    // Auto-select the option if the UID matches
                    if (record.UserId === Userid) {
                        option.selected = true;
                    }

                    dropdown.appendChild(option);
                });
            }
        };
    };

    request.onerror = function (event) {
        console.error('Error opening database:', event.target.errorCode);
        CustomAlert({ Result: "Failed", Message: "Failed to load inspection users." });
    };
}
//Save the local data 

function clearDropdown(selector) {
    const dropdown = document.querySelector(selector);
    while (dropdown.firstChild) {
        dropdown.removeChild(dropdown.firstChild);
    }
}
function AddLocalOffInspectionData() {
    // Retrieve all parameters
    const InspectionID = parseInt(getParameterByName('InspID'), 10);
    const id = parseInt(getParameterByName('id'), 10);
    const CheckListID = parseInt(getParameterByName('CheckListID'), 10);
    const hdnlatitude = $('#hdnlatitude').val();
    const hdnlongitude = $('#hdnlongitude').val();
    const sketch_data = $('#sketch_data').val();
    const tempAud = $('#tempAud').val();
    const tempVideo = $('#tempVideo').val();
    const map_image = $('#map_image').val();
    const image = $('#tempimg').val();
    const imgangle1 = $('#tempimg2').val();
    const InspectedById = $('#ddlInspectedBy').val();// Store value
    const InspectedBy = $('#ddlInspectedBy option:selected').text();

    const txtcomment = $('#comment').val();
    const LicenceNo = $('#lbllicence').text();
    const rdpInspectiondate = $('#rdpInspectiondate').val();

    let confirmcheck = ""; // Declare confirmcheck variable
    let inspstatusCheck = ""; // Declare inspstatusCheck variable

    // Check if "Confirmed" checkbox is checked
    if ($('#chckcnfrm').prop('checked') == true) {
        inspstatusCheck = "Confirmed";
        confirmcheck = $('#confirmcheck').val(); // Retrieve the value from the confirmcheck input field
    }

    // Check if "Out of Service" checkbox is checked
    if ($('#outServiceCheck').prop('checked') == true) {
        inspstatusCheck = "Out of Service";
        confirmcheck = $('#confirmcheck').val(); // Retrieve the value from the confirmcheck input field
    }

    // Display confirmation modal if status is set
    if (inspstatusCheck) {
        $('#divdelete').css("display", "none");
        $('#confirmModal').css("display", "block");
    }

    const InspectionQuestionDataArr = [];
    // Collect data for Inspection Questions
    $("#PlaceHolder1 .InspectionForm").each(function () {
        const checkboxid = $(this).find('input[type=checkbox]').attr('id');
        const ContentID = parseInt(checkboxid.split('~')[1], 10);
        const checkboxYes = $(this).find('.checkboxYes').prop('checked');
        const checkboxNo = $(this).find('.checkboxNo').prop('checked');
        /* const checkboxNA = $(this).find('.checkboxNA').prop('checked');*/
        const checkboxNA = $(this).find('.checkboxNA').length > 0 ? $(this).find('.checkboxNA').prop('checked') : false;
        const Action = $(this).find('.inputTypeActionRequired').val();

        const ArryVal = {
            InspectionID,
            ContentID,
            CheckListID,
            ClientID: id,
            Y: checkboxYes,
            N: checkboxNo,
            NA: checkboxNA,
            ActionRequired: Action
        };
        InspectionQuestionDataArr.push(ArryVal);
    });

    // Open the IndexedDB database
    const dbRequest = indexedDB.open(dbName, dbVersion);

    dbRequest.onerror = function (event) {
        console.error("Database error: " + event.target.errorCode);
    };

    dbRequest.onsuccess = function (event) {
        const db = event.target.result;

        // Start a transaction on tblInspectionDetails and tblInspectionListDetails
        const transaction = db.transaction(["tblInspectionDetails", "tblInspectionListDetails"], "readwrite");
        const InspectionStore = transaction.objectStore("tblInspectionDetails");
        const InspectionListStore = transaction.objectStore("tblInspectionListDetails");

        // Create or update the record in tblInspectionDetails
        const getRequest = InspectionStore.get(InspectionID);

        getRequest.onsuccess = function (event) {
            let inspectionRecord = event.target.result || { InspectionID };

            if (inspectionRecord.InspectedById !== InspectedById) {
                inspectionRecord.InspectedById = InspectedById;
                inspectionRecord.InspectedBy = InspectedBy; // Update the associated name
            }

            inspectionRecord.InspComments = txtcomment;
            inspectionRecord.InspSign = sketch_data || inspectionRecord.InspSign;
            inspectionRecord.Acknowledgment = confirmcheck; // Store the confirmcheck value
            //inspectionRecord.InspectedBy = InspectedBy;
            //inspectionRecord.InspectedById = InspectedById;
            /*inspectionRecord.AckDateTime = rdpInspectiondate;*/
            inspectionRecord.AckDateTime = "1900-01-01";
            inspectionRecord.Inspimage = image || inspectionRecord.Inspimage;
            inspectionRecord.Inspimage2 = imgangle1 || inspectionRecord.Inspimage2;
            inspectionRecord.InspStatus = inspstatusCheck;
            inspectionRecord.LicenceNo = LicenceNo;

            const AckinspDate = rdpInspectiondate;
            const formattedDate = AckinspDate;
            if (($('#chckcnfrm').prop('checked') == true) || ($('#outServiceCheck').prop('checked') == true)) {
                inspectionRecord.AckDateTime = formattedDate;
            }

            // Save or update the record
            const saveRequest = InspectionStore.put(inspectionRecord);

            saveRequest.onsuccess = function () {
                var Response = {
                    Result: "Success", Message: "Inspection data saved successfully."
                };
                CustomAlert(Response);
                $('#saveContinueModal').css("display", "block");

            };

            saveRequest.onerror = function () {
                console.error("Error saving inspection data:", saveRequest.error);
            };
        };

        getRequest.onerror = function () {
            console.error("Error fetching record:", getRequest.error);
        };

        // Process InspectionQuestionDataArr for tblInspectionListDetails
        InspectionQuestionDataArr.forEach(item => {
            const indexKeyRange = IDBKeyRange.only([item.InspectionID, item.ContentID, item.CheckListID]);

            const cursorRequest = InspectionListStore.index("InspectionID_ContentID_CheckListID").openCursor(indexKeyRange);

            cursorRequest.onsuccess = function (event) {
                const cursor = event.target.result;

                if (cursor) {
                    // Record exists, update it
                    const existingRecord = cursor.value;

                    existingRecord.Y = item.Y;
                    existingRecord.N = item.N;
                    existingRecord.NA = item.NA;
                    existingRecord.ActionRequired = item.ActionRequired;

                    const updateRequest = cursor.update(existingRecord);

                    updateRequest.onsuccess = function () {
                        console.log("Record updated successfully:", existingRecord);
                    };

                    updateRequest.onerror = function () {
                        console.error("Error updating record:", updateRequest.error);
                    };
                } else {
                    // Record does not exist, add a new one
                    const addRequest = InspectionListStore.add(item);

                    addRequest.onsuccess = function () {
                        console.log("New record added successfully:", item);
                    };

                    addRequest.onerror = function () {
                        console.error("Error adding new record:", addRequest.error);
                    };
                }
            };

            cursorRequest.onerror = function () {
                console.error("Error querying tblInspectionListDetails:", cursorRequest.error);
            };
        });
    };
}

function formatToUTCWithAMPM(date) {
    const options = {
        year: 'numeric',
        month: 'short',
        day: '2-digit',
        hour: '2-digit',
        minute: '2-digit',
        hour12: true,
        timeZone: 'UTC'
    };

    // Format the date and time without a comma and with AM/PM in uppercase
    const formattedDate = date.toLocaleString('en-GB', options).replace(',', '');
    const parts = formattedDate.split(' ');
    return `${parts[0]}-${parts[1]}-${parts[2]} ${parts[3]} ${parts[4].toUpperCase()}`;
}



async function fetchDataAndRender(id, checklistId, inspId) {
    var InspID = getParameterByName('InspID');
    var ClientId = getParameterByName('id');
    ClientId = OffDecrypt(ClientId);
    checklistId = getParameterByName('CheckListID');

    const db = await openIndexedDB(); // Open your IndexedDB instance
    let html = "";

    // Fetch InspectionCheckListColor
    const client = await getFromIndexedDB(db, "tblClient", ClientId);
    const inspectionCheckListColor = client ? client.InspectionCheckListColor : 0;

    // Fetch Headers (Header Logic)
    const headers = await getFromIndexedDBWithConditions(db, "tblcheckListContentList", {
        condition: (item) =>
            !item.checklistItem && !item.subChildItem && item.checklistID === parseInt(checklistId, 10)
    });

    for (const header of headers) {
        const headerId = header.ContentID;
        const headerName = header.CheckListContent || "Unnamed Header";

        html += `<div class='row' style='margin-top: 20px;'><div class='col-lg-12 col-md-12 col-sm-12 col-xs-12'><span class='InspectionListTitle'>${headerName}</span></div></div>`;


        var items = [];
        items = await fetchChecklistContent(headerId, ClientId, checklistId, InspID);

        if (items.length > 0) {
            html += "<table style='width:100%;' border='1'>";

            for (const item of items) {
                const questName = item.CheckListContent || "Unnamed Item";
                const contentId = item.ContentID;
                const checkY = item.Y;
                const checkN = item.N;
                const checkNA = item.NA;
                const isNA = item.IsNA;
                const actionRequired = item.ActionRequired || "";
                const itemInspectionCheckListColor = item.inspectionCheckListColor || "False";

                if (!inspectionData) {
                    inspectionData = await fetchInspectionData(); // Fetch only if not already done
                }

                if (!inspectionData) {
                    console.error("No data found to display.");
                    return;
                }

                var InspStatus = inspectionData.InspStatus || "";
                var inspstcode = inspectionData.CompleteInsp || "";

                html += `<tr><th style='width:40%;'>${questName}</th><td style='width:60%;'><div class='row InspectionForm'>`;
                if (((InspStatus == "Confirmed") || (InspStatus == "Out of Service"))) {

                    // Yes Checkbox
                    html += `<div class='col-lg-2 col-md-2 col-sm-2 col-xs-2' style='margin-left:50px;'>
    <input class='checkboxYes' ' disabled type='checkbox'
        ${checkY?.toString().toLowerCase() === "true" ? "checked='checked'" : ""} 
        value='YES' id='chckY~${contentId}' 
        onchange="handleCheckboxChange(this, '${contentId}')">
    <label class='${itemInspectionCheckListColor?.toString().toLowerCase() === "true" ? "chckyes1" : "chckyes"}' 
        for='chckY~${contentId}'></label>
</div>`;

                    // No Checkbox
                    html += `<div class='col-lg-2 col-md-2 col-sm-2 col-xs-2'>
    <input class='checkboxNo'  disabled type='checkbox'
        ${checkN?.toString().toLowerCase() === "true" ? "checked='checked'" : ""} 
        value='NO' id='chckN~${contentId}' 
        onchange="handleCheckboxChange(this, '${contentId}')">
    <label class='${itemInspectionCheckListColor?.toString().toLowerCase() === "true" ? "chckNo1" : "chckNo"}' 
        for='chckN~${contentId}'></label>
</div>`;

                    if (isNA == true) {
                        // NA Checkbox
                        html += `<div class='col-lg-2 col-md-2 col-sm-2 col-xs-2'>
    <input class='checkboxNA'  disabled type='checkbox'
        ${checkNA?.toString().toLowerCase() === "true" ? "checked='checked'" : ""} 
        value='NA' id='chckNA~${contentId}' 
        onchange="handleCheckboxChange(this, '${contentId}')">
    <label class='${itemInspectionCheckListColor?.toString().toLowerCase() === "true" ? "chckNA1" : "chckNA"}' 
        for='chckNA~${contentId}'></label>
</div>`;
                    } else {
                        // NA Checkbox
                        html += `<div class='col-lg-2 col-md-2 col-sm-2 col-xs-2'></div>`;
                    }

                    // Action Textbox
                    html += `<div class='col-lg-4 col-md-4 col-sm-4 col-xs-4'>
                <input class='inputTypeActionRequired'  type='text'
                    value='${actionRequired}' style='width: 100%;' id='Action~${contentId}' readonly>
            </div></div></td></tr>`;
                }
                else {
                    html += `<div class='col-lg-2 col-md-2 col-sm-2 col-xs-2' style='margin-left:50px;'>
    <input class='checkboxYes'  type='checkbox'
        ${checkY?.toString().toLowerCase() === "true" ? "checked='checked'" : ""} 
        value='YES' id='chckY~${contentId}' 
        onchange="handleCheckboxChange(this, '${contentId}')">
    <label class='${itemInspectionCheckListColor?.toString().toLowerCase() === "true" ? "chckyes1" : "chckyes"}' 
        for='chckY~${contentId}'></label>
</div>`;

                    // No Checkbox
                    html += `<div class='col-lg-2 col-md-2 col-sm-2 col-xs-2'>
    <input class='checkboxNo'  type='checkbox'
        ${checkN?.toString().toLowerCase() === "true" ? "checked='checked'" : ""} 
        value='NO' id='chckN~${contentId}' 
        onchange="handleCheckboxChange(this, '${contentId}')">
    <label class='${itemInspectionCheckListColor?.toString().toLowerCase() === "true" ? "chckNo1" : "chckNo"}' 
        for='chckN~${contentId}'></label>
</div>`;

                    if (isNA == true) {
                        // NA Checkbox
                        html += `<div class='col-lg-2 col-md-2 col-sm-2 col-xs-2'>
    <input class='checkboxNA'  type='checkbox'
        ${checkNA?.toString().toLowerCase() === "true" ? "checked='checked'" : ""} 
        value='NA' id='chckNA~${contentId}' 
        onchange="handleCheckboxChange(this, '${contentId}')">
    <label class='${itemInspectionCheckListColor?.toString().toLowerCase() === "true" ? "chckNA1" : "chckNA"}' 
        for='chckNA~${contentId}'></label>
</div>`;
                    } else {
                        html += `<div class='col-lg-2 col-md-2 col-sm-2 col-xs-2'></div>`;
                    }

                    // Action Textbox
                    html += `<div class='col-lg-4 col-md-4 col-sm-4 col-xs-4'>
                <input class='inputTypeActionRequired'  type='text'
                    value='${actionRequired}' style='width: 100%;' id='Action~${contentId}'>
            </div></div></td></tr>`;
                }


            }
            html += "</table>";
        }
    }

    document.getElementById("PlaceHolder1").innerHTML = html; // Append to your target element
}

function handleCheckboxChange(currentCheckbox, contentId) {
    const parentRow = currentCheckbox.closest('.InspectionForm');
    const checkboxes = parentRow.querySelectorAll(`input[id*='~${contentId}']`);

    // Uncheck all checkboxes except the one just clicked
    checkboxes.forEach((checkbox) => {
        if (checkbox !== currentCheckbox) {
            checkbox.checked = false;
        }
    });
}


// Helper Functions
function openIndexedDB() {
    return new Promise((resolve, reject) => {
        const request = indexedDB.open("scaffpac Job Register");
        request.onsuccess = (event) => resolve(event.target.result);
        request.onerror = (event) => reject(event.target.error);
    });
}

function getFromIndexedDB(db, storeName, id) {
    return new Promise((resolve, reject) => {
        const transaction = db.transaction(storeName, "readonly");
        const store = transaction.objectStore(storeName);
        const request = store.get(parseInt(id, 10));

        request.onsuccess = () => resolve(request.result);
        request.onerror = () => reject(request.error);
    });
}

function getFromIndexedDBWithConditions(db, storeName, { condition }) {
    return new Promise((resolve, reject) => {
        const transaction = db.transaction(storeName, "readonly");
        const store = transaction.objectStore(storeName);
        const request = store.openCursor();
        const results = [];

        request.onsuccess = (event) => {
            const cursor = event.target.result;
            if (cursor) {
                if (condition(cursor.value)) results.push(cursor.value);
                cursor.continue();
            } else {
                resolve(results);
            }
        };

        request.onerror = () => reject(request.error);
    });
}


async function getFromIndexedDBWithCondition(db, storeName, callback) {
    return new Promise((resolve, reject) => {
        const transaction = db.transaction(storeName, "readonly");
        const store = transaction.objectStore(storeName);
        const request = store.openCursor();
        const results = [];

        request.onsuccess = (event) => {
            const cursor = event.target.result;
            if (cursor) {
                const item = cursor.value;
                if (callback(item)) {
                    results.push(item);
                }
                cursor.continue();
            } else {
                resolve(results);
            }
        };

        request.onerror = () => reject(request.error);
    });
}


// Helper function to compute itemOrder
function computeItemOrder(record, checklistContentData) {
    if (!record.ChecklistItem && !record.SubChildItem) {
        return pad(record.ItemOrder, 2) + ".0";
    } else if (record.ChecklistItem && record.SubChildItem) {
        const parentOrder = checklistContentData.find(
            (item) => item.ContentID === record.ChecklistItem
        )?.ItemOrder;
        const childOrder = checklistContentData.find(
            (item) => item.ContentID === record.SubChildItem
        )?.ItemOrder;
        return (
            pad(parentOrder, 2) +
            "." +
            pad(childOrder, 2) +
            "." +
            pad(record.ItemOrder, 3)
        );
    } else {
        const parentOrder = checklistContentData.find(
            (item) => item.ContentID === record.ChecklistItem
        )?.ItemOrder;
        return pad(parentOrder, 2) + "." + pad(record.ItemOrder, 3);
    }
}

// Helper function to pad numbers with leading zeros
function pad(number, length) {
    return String(number).padStart(length, "0");
}

// IndexedDB helpers
function openDatabase(dbName) {
    return new Promise((resolve, reject) => {
        const request = indexedDB.open(dbName);

        request.onsuccess = (event) => {
            resolve(event.target.result);
        };

        request.onerror = (event) => {
            reject(event.target.error);
        };
    });
}


async function fetchChecklistContent(headerId, clientId, checklistId, inspId) {

    const tableName = "tblcheckListContentList";
    const inspectionTable = "tblInspectionListDetails";

    // Open the database
    const db = await openDatabase(dbName);

    const result = [];

    // Fetch checklist content and inspection data sequentially
    const checklistContentData = await getRecords(db, tableName);
    const inspectionData = await getRecords(db, inspectionTable);

    // Filter and process data
    checklistContentData.forEach((record) => {
        if (
            record.subChildItem === null &&
            record.checklistItem === headerId &&
            record.ClientID === parseInt(clientId, 10) &&
            record.checklistID === parseInt(checklistId, 10)
        ) {
            const inspectionRecord = inspectionData.find(
                (insp) =>
                    insp.ContentID === record.ContentID &&
                    insp.InspectionID === parseInt(inspId, 10)
            );

            if (inspectionRecord) {
                const itemOrder = computeItemOrder(
                    record,
                    checklistContentData
                );

                result.push({
                    InspectionListID: inspectionRecord.InspectionListID,
                    InspectionID: inspectionRecord.InspectionID,
                    ContentID: record.ContentID,
                    ChecklistItem: record.ChecklistItem,
                    SubChildItem: record.SubChildItem,
                    ItemOrder: itemOrder,
                    CheckListContent: record.CheckListContent,
                    ChecklistID: record.ChecklistID,
                    Y: inspectionRecord.Y || "False",
                    N: inspectionRecord.N || "False",
                    NA: inspectionRecord.NA || "False",
                    ActionRequired: inspectionRecord.ActionRequired,
                    IsNA: record.IsNA,
                });
            }
        }
    });

    // Return sorted results
    return result.sort((a, b) => a.ItemOrder.localeCompare(b.ItemOrder));
}

// Helper function to fetch records from a store
function getRecords(db, tableName) {
    return new Promise((resolve, reject) => {
        const transaction = db.transaction(tableName, "readonly");
        const store = transaction.objectStore(tableName);
        const request = store.getAll();

        request.onsuccess = (event) => {
            resolve(event.target.result);
        };

        request.onerror = (event) => {
            reject(event.target.error);
        };
    });
}



function ddlInspectedBy_OnChange() {
    // Get the selected inspector's ID (UserId)
    const InspectedByID = parseInt(document.querySelector('#ddlInspectedBy').value, 10);

    // Open IndexedDB and fetch the license number
    const request = indexedDB.open('scaffpac Job Register'); // Replace with your IndexedDB name and version

    request.onsuccess = function (event) {
        const db = event.target.result;

        // Start a transaction
        const transaction = db.transaction(['tblUserList'], 'readonly');
        const store = transaction.objectStore('tblUserList');

        // Query the IndexedDB using InspectedByID
        const query = store.get(InspectedByID);

        query.onsuccess = function (e) {
            const user = e.target.result;

            if (user) {
                // Get LicenceNo and update the label
                const LicenceNo = user.LicenceNo || '';
                /* document.querySelector('#lbllicence').textConte = LicenceNo; */// Update label
                $('#lbllicence').text(LicenceNo);
            } else {
                // If no record is found, clear the label
                document.querySelector('#lbllicence').text = '';
            }
        };

        query.onerror = function (e) {
            console.error('Error fetching data from IndexedDB', e);
        };
    };

    request.onerror = function (event) {
        console.error('Error opening IndexedDB', event);
    };
}


function btnsavecontinue_Click() {
    $('#LoadingPanel1').show();
    $('#saveContinueModal').css("display", "none");

    var id = getParameterByName('id');
    var RID = getParameterByName('RID');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var InspId = getParameterByName('InspID');
    var ChecklistId = getParameterByName('CheckListID');
    var Siteid = getParameterByName('SiteLocId');
    var OfflineUserRole = getParameterByName('OfflineUserRole');

    window.location.href = '/OfflineInspectionForm.html?UID=' + encodeURIComponent(UID) +
        '&UserRole=' + encodeURIComponent(UserRole) +
        '&OfflineUserRole=' + encodeURIComponent(OfflineUserRole) +
        '&id=' + encodeURIComponent(id) +
        '&SiteLocId=' + encodeURIComponent(Siteid) +
        '&RID=' + encodeURIComponent(RID) +
        '&CheckListID=' + encodeURIComponent(ChecklistId) +
        '&InspID=' + encodeURIComponent(InspId);

}