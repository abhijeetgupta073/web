
var GetCount = 0;
var GetclientId;
var dbName = "scaffpac Job Register";
var dbVersion = 70;
var db;
var dropdownCheck = false;
var UID = getParameterByName('UID');
var id = getParameterByName('id');
let ClientID = OffDecrypt(id)
var RID = OffDecrypt(getParameterByName('RID'));
var OfflineUserRole = getParameterByName('OfflineUserRole');
var SiteLocID = getParameterByName('SiteLocId');
SiteLocID = OffDecrypt(SiteLocID);
var GetOfflineStatus = getParameterByName('offlinestatus');
var OfflineStatus = window.atob(GetOfflineStatus);

// Function to remove a parameter from the URL
function removeQueryParam(param) {
    const url = new URL(window.location.href);
    url.searchParams.delete(param);
    window.history.replaceState(null, '', url.toString());
}

$(document).ready(function () {

    //$.ajax({
    //    type: "POST",
    //    // url: "../WebApi/OfflineRegister.asmx/GetDecryptClientID",
    //    url: "/OfflineRegister/GetDecryptClientID",
    //    data: JSON.stringify({ id: CID }),
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    success: function (response) {
    //        ClientID = response;
    //        CreateDbTables();
    //        //    ShowOnlineDropDownList();
    //        getTblregisterDataById(RID)
    //    }

    //});

    // Added by Pooja Rai

    document.getElementById("txtlength").addEventListener('input', restrictToInteger);
    document.getElementById("txtwidth").addEventListener('input', restrictToInteger);
    document.getElementById("txtheight").addEventListener('input', restrictToInteger);
    document.getElementById("txtManhrserect").addEventListener('input', restrictToInteger);
    document.getElementById("txtmanDisman").addEventListener('input', restrictToInteger);
    document.getElementById("txtCrewSize").addEventListener('input', restrictToInteger);
    function restrictToInteger() {
        this.value = this.value.replace(/[^\d.]/g, '');
    }
    $("#txtManhrserect").on('change', function () {
        //var ClientID = $("#hdnClientID").val();
        GetManHrsRatio(ClientID);
    });

    $("#ddlEngineeredDesign").on('change', function () {
        DrawingNoDisable();
    });
    // End by Pooja Rai
    //$('#radUpdogImg').on('change', function () {
    //    checkImageSize(this);
    //});
    $('#FileUpload1').change(function (e) {
        var output = document.getElementById('fileList');
        //output.innerHTML = 'File Selected:';
        output.innerHTML = '<ul>';

        // Check if more than 2 files are selected
        if (e.target.files.length > 2) {
            CustomAlert({ Result: "Failed", Message: "You can upload a maximum of 2 files." });
            return;
        }

        // Loop through the selected files and list their names
        for (var i = 0; i < e.target.files.length; i++) {
            output.innerHTML += '<li>' + e.target.files.item(i).name + '</li>';
        }

        output.innerHTML += '</ul>';
    });
    $('#mobApp_fileimagellery').change(function (e) {
        var output = document.getElementById('fileList');
        //output.innerHTML = 'File Selected:';
        output.innerHTML = '<ul>';

        // Check if more than 2 files are selected
        if (e.target.files.length > 2) {
            CustomAlert({ Result: "Failed", Message: "You can upload a maximum of 2 files." });
            return;
        }

        // Loop through the selected files and list their names
        for (var i = 0; i < e.target.files.length; i++) {
            output.innerHTML += '<li>' + e.target.files.item(i).name + '</li>';
        }

        output.innerHTML += '</ul>';
    });
    $('#mob_file_imagegallery').change(function (e) {
        var output = document.getElementById('fileList');
        //output.innerHTML = 'File Selected:';
        output.innerHTML = '<ul>';

        // Check if more than 4 files are selected
        if (e.target.files.length > 2) {
            CustomAlert({ Result: "Failed", Message: "You can upload a maximum of 2 files." });
            return;
        }

        // Loop through the selected files and list their names
        for (var i = 0; i < e.target.files.length; i++) {
            output.innerHTML += '<li>' + e.target.files.item(i).name + '</li>';
        }

        output.innerHTML += '</ul>';
    });
    $('#rdpInspectiondate').val(getDateTime());
    $('#dateInspection').val(getDateTime());
    GetTblregisterDataById(RID)


    //shubham gupta
    //populateAddInspectionInspectionBy();
    //populateDeplankInspectionBy();
    //populateInspectionList();
    GetInspectionListData(RID);
    GetTblregisterImageGelleryDataById(RID);
    GetCycloneDataById(RID);
    GetCycloneTabHideShow(ClientID, SiteLocID);
    getInventoryList(RID, null);
    populatematerialDropdown('tblPipelineRequest', '#ddlpipelineMaterialType', 'MaterialType', 'id', RID);
    //Shubham changes start
    var today = new Date();
    var rdpInspectiondate = $('#rdpInspectiondate').val();
    if (rdpInspectiondate == "") {
        $('#rdpInspectiondate').val(getDateTime());
    }
    $('#hdnTimeOffSet').val(new Date().getTimezoneOffset());

    $('#rdpInspectiondate').datetimepicker({
        //    // datepicker: false,
        //    //format: 'd-M-Y H:i A',
        //    format: 'd-M-Y H:i A',
        format: 'd-M-Y h:i A', // 'h' is for 12-hour format
        maxDate: today, // Maximum time in 12-hour format
        step: 30, // Time intervals in minutes
        ampm: true // Show AM/PM toggle
        //    maxDate: today,
        //    //maxTime: '18:00'
    });
    //});
    $('.calendar-icon').click(function () {
        $('#rdpInspectiondate').focus();
    });

    //deplak date
    var rdpInspectiondate = $('#dateInspection').val();
    if (rdpInspectiondate == "") {
        $('#dateInspection').val(getDateTime());
    }
    $('#hdnTimeOffSet').val(new Date().getTimezoneOffset());

    $('#dateInspection').datetimepicker({
        //    // datepicker: false,
        //    //format: 'd-M-Y H:i A',
        //    format: 'd-M-Y H:i A',
        format: 'd-M-Y h:i A', // 'h' is for 12-hour format
        maxDate: today, // Maximum time in 12-hour format
        step: 30, // Time intervals in minutes
        ampm: true // Show AM/PM toggle
        //    maxDate: today,
        //    //maxTime: '18:00'
    });
    //});
    $('.calendar-icon').click(function () {
        $('#dateInspection').focus();
    });
    //Shubham changes end
    // Added by Pooja Rai
    var activateTab = getParameterByName('activateTab');
    if (activateTab === 'Inspection') {
        //$('#liInspection').removeClass('active').addClass('active');
        //$('#liCompany').removeClass('active');
        OfflineshowInspection();
        $('#AnchorInspection').click();
    }
    if (activateTab === 'Inventory') {
        showPipelineTab();
        $('#AnchorScafpacphase2').click();
    }
    if (activateTab === 'ImageGallery') {
        OfflineImageAdd();
        $('#AnchorImageGallery').click();
    }
    if (activateTab === 'Cyclone') {
        showCyclone();
        $('#AnchorCyclone').click();
    }
    removeQueryParam('activateTab');
    // End by Pooja Rai
    $("#radUpdogImg").change(function () {
        readURL(this);
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



    $('#btndeleteimagegallery').on('click', function () {

        $('#divdeleteimagegellery').css('display', 'block');
    });
    $("#getCameraimagegellery").on("click", function () {
        // alert();
        $("#cancelsnapimagegellery").on("click", function () {
            $("#btnOpenModel").css("display", "none");
            cameraStream.stop();
        });
        $("#btnOpenModel").css("display", "block");
        $(".hdnCamerabeforeimagegellery").css("display", "block");
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
            $('#hdncaptureimagegellery').val("true");
            $("#imageLicence").css("display", "none");
            context.drawImage(video, 0, 0, 640, 480);
            var image = document.getElementById("canvas").toDataURL("image/png");
            $("#snapimage").val(image);
            cameraStream.stop();
        });
    });



    var GetisSafari = navigator.vendor && navigator.vendor.indexOf('Apple') > -1 && navigator.userAgent && !navigator.userAgent.match('CriOS');
    if (GetisSafari == false) {
        $(".nonsafariimagegellery").css("display", "block");
        $(".safariimagegallery").css("display", "none");
        var isMobile = /iPhone|iPad|iPod|Android/i.test(navigator.userAgent);
        if (isMobile) {
            $(".mobileAppimagegallery").css("display", "block");
            $(".desktopAppimagegallery").css("display", "none");
        } else {
            $(".mobileAppimagegallery").css("display", "none");
            $(".desktopAppimagegallery").css("display", "block");
        }
    } else {
        $(".nonsafariimagegellery").css("display", "none");
        $(".safariimagegallery").css("display", "block");
    }

    $(document).on("change", "#mob_file_imagegallery", function (evt) {
        var maxSize = 2 * 1024 * 1024; // 2 MB in bytes
        var file = this.files[0];

        if (file) {
            var reader = new FileReader();
            reader.readAsDataURL(file);

            reader.onloadend = function () {
                var base64mobdata = reader.result;

                // Create an image element and load the selected file
                var img = new Image();
                img.src = base64mobdata;

                img.onload = function () {
                    var canvas = document.createElement("canvas");
                    var ctx = canvas.getContext("2d");

                    // Resize logic
                    var maxWidth = 1024; // Maximum width
                    var maxHeight = 1024; // Maximum height
                    var width = img.width;
                    var height = img.height;

                    if (width > maxWidth || height > maxHeight) {
                        if (width > height) {
                            height *= maxWidth / width;
                            width = maxWidth;
                        } else {
                            width *= maxHeight / height;
                            height = maxHeight;
                        }
                    }

                    canvas.width = width;
                    canvas.height = height;

                    // Draw the resized image on the canvas
                    ctx.drawImage(img, 0, 0, width, height);

                    // Compress the image (set quality to 70%)
                    var compressedData = canvas.toDataURL("image/jpeg", 0.7);

                    // Convert base64 to Blob to check file size
                    var byteString = atob(compressedData.split(',')[1]);
                    var arrayBuffer = new ArrayBuffer(byteString.length);
                    var uint8Array = new Uint8Array(arrayBuffer);

                    for (var i = 0; i < byteString.length; i++) {
                        uint8Array[i] = byteString.charCodeAt(i);
                    }

                    var blob = new Blob([uint8Array], { type: "image/jpeg" });

                    if (blob.size > maxSize) {
                        CustomAlert({
                            Result: "Failed",
                            Message: "The captured image exceeds 2 MB even after compression. Please capture a smaller image or reduce the file size before uploading."
                        });
                        $("#mob_file_imagegallery").val(''); // Clear the file input
                        return;
                    }

                    // If file size is acceptable, set the compressed image
                    $("#snapimage").val(compressedData);
                    $("#fileList").css("display", "block");

                };
            };
        }
    });

    $(document).on("change", "#mobApp_fileimagellery", function (evt) {
        var maxSize = 2 * 1024 * 1024; // 2 MB in bytes
        var file = this.files[0];

        if (file) {
            var reader = new FileReader();
            reader.readAsDataURL(file);

            reader.onloadend = function () {
                var base64mobdata = reader.result;

                // Create an image element and load the selected file
                var img = new Image();
                img.src = base64mobdata;

                img.onload = function () {
                    var canvas = document.createElement("canvas");
                    var ctx = canvas.getContext("2d");

                    // Resize logic
                    var maxWidth = 1024; // Maximum width
                    var maxHeight = 1024; // Maximum height
                    var width = img.width;
                    var height = img.height;

                    if (width > maxWidth || height > maxHeight) {
                        if (width > height) {
                            height *= maxWidth / width;
                            width = maxWidth;
                        } else {
                            width *= maxHeight / height;
                            height = maxHeight;
                        }
                    }

                    canvas.width = width;
                    canvas.height = height;

                    // Draw the resized image on the canvas
                    ctx.drawImage(img, 0, 0, width, height);

                    // Compress the image (set quality to 70%)
                    var compressedData = canvas.toDataURL("image/jpeg", 0.7);

                    // Convert base64 to Blob to check file size
                    var byteString = atob(compressedData.split(',')[1]);
                    var arrayBuffer = new ArrayBuffer(byteString.length);
                    var uint8Array = new Uint8Array(arrayBuffer);

                    for (var i = 0; i < byteString.length; i++) {
                        uint8Array[i] = byteString.charCodeAt(i);
                    }

                    var blob = new Blob([uint8Array], { type: "image/jpeg" });

                    if (blob.size > maxSize) {
                        CustomAlert({
                            Result: "Failed",
                            Message: "The captured image exceeds 2 MB even after compression. Please capture a smaller image or reduce the file size before uploading."
                        });
                        $("#mobApp_fileimagellery").val(''); // Clear the file input
                        return;
                    }

                    // If file size is acceptable, set the compressed image
                    $("#snapimage").val(compressedData);
                    $("#fileList").css("display", "block");

                };
            };
        }
    });

});
$(document).on("change", "#mob_file_image", function (evt) {
    var maxSize = 2 * 1024 * 1024; // 2 MB in bytes
    var file = this.files[0];

    if (file) {
        var reader = new FileReader();
        reader.readAsDataURL(file);

        reader.onload = function (event) {
            var img = new Image();
            img.src = event.target.result;

            img.onload = function () {
                var canvas = document.createElement("canvas");
                var ctx = canvas.getContext("2d");

                // Resize logic
                var maxWidth = 1024; // Maximum width
                var maxHeight = 1024; // Maximum height
                var width = img.width;
                var height = img.height;

                if (width > maxWidth || height > maxHeight) {
                    if (width > height) {
                        height *= maxWidth / width;
                        width = maxWidth;
                    } else {
                        width *= maxHeight / height;
                        height = maxHeight;
                    }
                }

                canvas.width = width;
                canvas.height = height;

                // Draw image on canvas
                ctx.drawImage(img, 0, 0, width, height);

                // Compress the image
                var compressedData = canvas.toDataURL("image/jpeg", 0.7); // Quality is set to 70%

                // Convert base64 to Blob to check size
                var byteString = atob(compressedData.split(',')[1]);
                var arrayBuffer = new ArrayBuffer(byteString.length);
                var uint8Array = new Uint8Array(arrayBuffer);

                for (var i = 0; i < byteString.length; i++) {
                    uint8Array[i] = byteString.charCodeAt(i);
                }

                var blob = new Blob([uint8Array], { type: "image/jpeg" });

                if (blob.size > maxSize) {
                    CustomAlert({
                        Result: "Failed",
                        Message: "The captured image exceeds 2 MB even after compression. Please capture a smaller image or reduce the file size before uploading."
                    });
                    $("#mob_file_image").val(''); // Clear the file input
                    return;
                }

                // If size is okay, set the compressed image
                $("#basetempimg").val(compressedData);
                $("#imga").attr('src', compressedData);
                $("#tempimg").val(compressedData);
                $('#deleteimage').css('display', 'block');
                $('#btnImageSave').css('display', 'block');
                $('#downloadButton').css('display', 'none');
            };
        };
    }
});
//$(document).on("change", ".mob_file_image", function (evt) {
//    var base64mobImagedata;
//    var reader = new FileReader();
//    reader.readAsDataURL(this.files[0]);
//    reader.onloadend = function () {
//        base64mobImagedata = reader.result;
//        $("#basetempimg").val(base64mobImagedata);
//        $("#imga").attr('src', base64mobImagedata);
//        $("#tempimg").val(base64mobImagedata);
//        $('#deleteimage').css('display', 'block');
//        $('#btnImageSave').css('display', 'block');
//        $('#downloadButton').css('display', 'none');

//    }
//});

$(document).on("change", "#mobApp_file", function (evt) {
    var maxSize = 2 * 1024 * 1024; // 2 MB in bytes
    var file = this.files[0];

    if (file) {
        var reader = new FileReader();
        reader.readAsDataURL(file);

        reader.onload = function (event) {
            var img = new Image();
            img.src = event.target.result;

            img.onload = function () {
                var canvas = document.createElement("canvas");
                var ctx = canvas.getContext("2d");

                // Resize logic
                var maxWidth = 1024; // Maximum width
                var maxHeight = 1024; // Maximum height
                var width = img.width;
                var height = img.height;

                if (width > maxWidth || height > maxHeight) {
                    if (width > height) {
                        height *= maxWidth / width;
                        width = maxWidth;
                    } else {
                        width *= maxHeight / height;
                        height = maxHeight;
                    }
                }

                canvas.width = width;
                canvas.height = height;

                // Draw image on canvas
                ctx.drawImage(img, 0, 0, width, height);

                // Compress the image
                var compressedData = canvas.toDataURL("image/jpeg", 0.7); // Quality is set to 70%

                // Convert base64 to Blob to check size
                var byteString = atob(compressedData.split(',')[1]);
                var arrayBuffer = new ArrayBuffer(byteString.length);
                var uint8Array = new Uint8Array(arrayBuffer);

                for (var i = 0; i < byteString.length; i++) {
                    uint8Array[i] = byteString.charCodeAt(i);
                }

                var blob = new Blob([uint8Array], { type: "image/jpeg" });

                if (blob.size > maxSize) {
                    CustomAlert({
                        Result: "Failed",
                        Message: "The captured image exceeds 2 MB even after compression. Please capture a smaller image or reduce the file size before uploading."
                    });
                    $("#mobApp_file").val(''); // Clear the file input
                    return;
                }

                // If size is okay, set the compressed image
                $("#basetempimg").val(compressedData);
                $("#imga").attr('src', compressedData);
                $("#tempimg").val(compressedData);
                $('#deleteimage').css('display', 'block');
                $('#btnImageSave').css('display', 'block');
                $('#downloadButton').css('display', 'none');
            };
        };
    }
});
$(document).on('change', '#radUpdogImg', function () {
    const file = this.files[0];
    if (!file) return;

    const maxWidth = 800; // Max width for the compressed image
    const maxHeight = 800; // Max height for the compressed image
    const quality = 0.7; // Compression quality (0.0 - 1.0)

    const reader = new FileReader();
    reader.onload = function (e) {
        const img = new Image();
        img.onload = function () {
            const canvas = document.createElement('canvas');
            let width = img.width;
            let height = img.height;

            // Maintain aspect ratio
            if (width > maxWidth || height > maxHeight) {
                if (width > height) {
                    height = Math.round((maxHeight / width) * height);
                    width = maxWidth;
                } else {
                    width = Math.round((maxWidth / height) * width);
                    height = maxHeight;
                }
            }

            canvas.width = width;
            canvas.height = height;
            const ctx = canvas.getContext('2d');
            ctx.drawImage(img, 0, 0, width, height);

            // Convert canvas to base64 string
            const base64Image = canvas.toDataURL('image/jpeg', quality);

            // Set the base64 string into the hidden input
            $('#basetempimg').val(base64Image);

            // Log or use the value
            console.log('Base64 image set to hidden input:', base64Image);
        };
        img.src = e.target.result;
    };
    reader.readAsDataURL(file);
});


function insertIntoIndexedDB(storeName, data) {
    var dbPromise = indexedDB.open(dbName, dbVersion);

    dbPromise.onsuccess = function (event) {
        db = event.target.result;

        // Check if the object store exists
        if (!db.objectStoreNames.contains(storeName)) {
            console.error("Error: Object store '" + storeName + "' not found in the database.");
            return; // Exit the function if the store is not found
        }

        // Start a transaction with the specified store
        var transaction = db.transaction(storeName, 'readwrite');
        var store = transaction.objectStore(storeName);

        // Handle transaction complete and errors
        transaction.oncomplete = function () {
            console.log("All data inserted successfully into: " + storeName);
        };

        transaction.onerror = function (event) {
            console.error("Transaction error:", event.target.error);
        };

        // Insert each item into the store
        data.forEach(function (item) {
            var request = store.put(item);

            request.onsuccess = function () {
                //    console.log("Item inserted successfully:", item);
            };

            //request.onerror = function (event) {
            //    console.error("Failed to insert item:", item, "Error:", event.target.error);
            //};
            request.onerror = function (event) {
                console.error("Failed to insert item:", item, "Error:", event.target.error);

                // Log the key path and the item to identify the missing key
                console.error("Key path issue with item:", item);
            };
        });
    };

    dbPromise.onerror = function (event) {
        console.error("Failed to open database:", event.target.errorCode);
    };
}

// Added by Pooja Rai
function GetManHrsRatio(ClientID) {
    const request = indexedDB.open(dbName, dbVersion);
    request.onsuccess = function (event) {
        const db = event.target.result;
        const transaction = db.transaction('tblClient', 'readonly');
        const objectStore = transaction.objectStore('tblClient');


        const cursorRequest = objectStore.openCursor();
        let found = false;

        cursorRequest.onsuccess = function (event) {
            const cursor = event.target.result;

            if (cursor) {
                const record = cursor.value;

                if (record.ClientId === parseInt(ClientID, 10)) {
                    found = true;
                    const manHrsRatio = record.ManHrsRatio;


                    calculateManHrs(manHrsRatio);


                    return;
                }
                cursor.continue();
            } else {

                if (!found) {
                    console.error('No record found for ClientID:', ClientID);
                }
            }
        };

        cursorRequest.onerror = function (event) {
            console.error('Error fetching data:', event.target.error);
        };
    };

    request.onerror = function (event) {
        console.error('Error opening database:', event.target.error);
    };
}

function calculateManHrs(manHrsRatio) {
    const manHrsErect = parseFloat($("#txtManhrserect").val()) || 0;
    const manHrsDismantle = manHrsRatio * manHrsErect;

    // Set calculated value to the input field
    $("#txtmanDisman").val(manHrsDismantle.toFixed(2));
}

function populateDropDowns() {
    populateDropdown('tblDesignList', '#ddlScaffoldDesign', 'id', 'DesignType'); // 'id'
    populateDropdown('tblDutyRating', '#ddlDutyrating', 'id', 'DutyRating'); // 'id'
    populateDropdown('tblFloorLevel', '#DropDownList1', 'FloorLevelId', 'FloorLevel'); // 'FloorLevelId'
    populateDropdown('tblMaterialType', '#ddlMaterialType', 'id', 'MaterialType'); // 'id'
    populateDropdown('tblSiteList', '#ddlsites', 'SiteLocID', 'SiteLocation'); // 'SiteLocID'
    populateDropdown('tblCheckList', '#ddlCheckListData', 'checklistID', 'checklistdescription'); // 'checklistID'
    populateDropdown('tblLocationList', '#ddlLocation', 'AreaID', 'LocationName'); // 'AreaID'
    populateDropdown('tblBuildingList', '#ddbuilding', 'BuildingID', 'BuildingNo'); // 'BuildingID'
    populateDropdown('tblAssetList', '#ddassets', 'Assetid', 'Assetno'); // 'Assetid'
    populateDropdown('tblSupervisor', '#ddlSupervisor', 'UserID', 'SupervisorName'); // 'UserID'
    populateDropdown('tblScaffoldOwner', '#ddlScaffoldOwner', 'id', 'OwnerName'); // 'id'
    populateDropdown('tblSubOwnerRep', '#ddlscaffoldowner', 'id', 'SubOwnerName'); // 'id'
    populateDropdown('tblWorkGroups', '#ddlWorkGroups', 'id', 'GroupName'); // 'id'
    populateDropdown('tblSubGroups', '#ddlSubGroups', 'id', 'SubGroupName'); // 'id'
}

function populateDropdown(storeName, dropdownSelector, valueField, textField) {
    // Clear the dropdown before fetching data
    $(dropdownSelector).empty();
    const dropdown = document.querySelector(dropdownSelector);

    // Clear existing options in the dropdown
    dropdown.innerHTML = '<option value="">Select Option</option>';

    // Open IndexedDB and fetch data
    var dbPromise = indexedDB.open(dbName, dbVersion);
    dbPromise.onsuccess = function (event) {
        var db = event.target.result;
        var transaction = db.transaction(storeName, 'readonly');
        var store = transaction.objectStore(storeName);
        var request = store.getAll();

        request.onsuccess = function (event) {
            var data = event.target.result;

            // Add default "Select" option
            $(dropdownSelector).append(
                $('<option></option>').attr('value', '').text('Select Option')
            );

            // Populate dropdown with data
            data.forEach(function (item) {
                $(dropdownSelector).append(
                    $('<option></option>').attr('value', item[valueField]).text(item[textField])
                );
            });
        };

        request.onerror = function (event) {
            console.error("Error retrieving data from", storeName, ":", event);
        };
    };

    dbPromise.onerror = function (event) {
        console.error("Error opening IndexedDB:", event);
    };
}



function changeFloorLevel(SelectedVal, sender) {
    var SiteLocID = parseInt($("#ddlsites").val(), 10);
    var LocationID = parseInt($("#ddlLocation").val(), 10);
    var BuildingID = $("#ddbuilding").val() ? parseInt($("#ddbuilding").val(), 10) : 0;


    const dbRequest = indexedDB.open(dbName, dbVersion);

    dbRequest.onsuccess = function (event) {
        const db = event.target.result;
        const transaction = db.transaction(["tblSiteList", "tblFloorLevel"], "readonly");

        const siteListStore = transaction.objectStore("tblSiteList");
        const siteQuery = siteListStore.index("SiteLocId").get(SiteLocID);

        siteQuery.onsuccess = function (event) {
            const siteData = event.target.result;

            if (siteData) {
                const { FloorLevel, GPS } = siteData;

                if (GPS === 1) {
                    // Query tblFloorLevel to check count
                    const floorStore = transaction.objectStore("tblFloorLevel");
                    const floorIndex = floorStore.index("SiteLocId_LocationID_BuildingID");
                    const keyRange = IDBKeyRange.only([newSiteLocId, LocationID, BuildingID]);

                    const floorCountQuery = floorIndex.count(keyRange);

                    floorCountQuery.onsuccess = function (event) {
                        const TotalCount = event.target.result;

                        if (TotalCount !== 0) {
                            // Query matching floor levels
                            const floorQuery = floorIndex.getAll(keyRange);

                            floorQuery.onsuccess = function (event) {
                                const floorLevels = event.target.result;
                                updateDropdown(floorLevels, SelectedVal);
                            };
                        } else {
                            // If no matching floors, fallback to buildingID '0'
                            const buildingFallbackKeyRange = IDBKeyRange.only([newSiteLocId, LocationID, 0]);
                            const fallbackQuery = floorIndex.getAll(buildingFallbackKeyRange);

                            fallbackQuery.onsuccess = function (event) {
                                const floorLevels = event.target.result;
                                updateDropdown(floorLevels, SelectedVal);
                            };
                        }
                    };
                } else if (FloorLevel === 1) {
                    // Handle FloorLevel === 1 case
                    const floorStore = transaction.objectStore("tblFloorLevel");
                    const floorIndex = floorStore.index("SiteLocId_LocationID_BuildingID");
                    const keyRange = IDBKeyRange.only([newSiteLocId, LocationID, BuildingID]);

                    const floorQuery = floorIndex.getAll(keyRange);

                    floorQuery.onsuccess = function (event) {
                        const floorLevels = event.target.result;
                        updateDropdown(floorLevels, SelectedVal);
                    };
                }
            }
        };
    };

    dbRequest.onerror = function () {
        console.error("Failed to open the database.");
    };
}

// Helper function to update the dropdown
function updateDropdown(floorLevels, SelectedVal) {
    // Clear existing dropdown options
    $("#DropDownList1 option").each(function () {
        $(this).remove();
    });

    if (floorLevels && floorLevels.length > 0) {
        floorLevels.forEach((floor) => {
            $("#DropDownList1").append(
                $("<option></option>").attr("value", floor.FloorLevelId).text(floor.FloorLevel)
            );
        });

        if (SelectedVal != null) {
            console.log("DropDownList1 " + SelectedVal);
            $("#DropDownList1").val(SelectedVal);
        }
    }
}



function changeSelectedLocation(SelectedVal, sender) {
    var checkLocFlag = false;
    var newSiteLocId = $("#ddlsites").val();

    // Open transaction
    var transaction = db.transaction(["tblLocationList"], "readonly");
    var objectStore = transaction.objectStore("tblLocationList");
    var index = objectStore.index("SiteLocId"); // Assuming you have an index on SiteLocID

    var request = index.getAll(newSiteLocId); // Get all records matching newSiteLocId

    request.onsuccess = function (event) {
        var result = event.target.result;

        if (result.length > 0) {
            $("#ddlLocation option").each(function () {   // Refreshing dropdown options
                $(this).remove();
            });

            // Populate LocationListArray with the result
            var LocationListArray = result;

            $("#ddlLocation").append("<option value=''></option>");
            $.each(LocationListArray, function (index, value) {
                $("#ddlLocation").append($("<option></option>").attr("value", value.areaid).text(value.area));

                if (LocationListArray.length == index + 1) {
                    checkLocFlag = true;
                }
            });

            if (SelectedVal != null) {
                console.log("Location" + SelectedVal);
                $("#ddlLocation").val(SelectedVal);
                $("#ddlLocation").find("option:contains(" + SelectedVal + ")").attr('selected', 'selected');
            }

            if (sender != null) {
                changeSelectedBuilding(null, sender);
            }
        } else {
            $("#ddlLocation option").each(function () {   // Reset dropdown if no results
                $(this).remove();
            });
        }
    };

    request.onerror = function (event) {
        console.log("Error in fetching location data:", event.target.error);
    };

    return true;
}


function changeSelectedBuilding(getSelectedVal, sender) {
    // Reopen the database if it's not initialized
    if (!db) {
        var request = indexedDB.open(dbName, dbVersion);

        request.onsuccess = function (event) {
            db = event.target.result;
            console.log("Database reopened successfully.");
            executeBuildingChange(getSelectedVal, sender); // Call the main logic after reopening
        };

        request.onerror = function (event) {
            console.error("Error reopening database:", event.target.error);
        };
    } else {
        executeBuildingChange(getSelectedVal, sender); // Proceed if db is already initialized
    }
}

function executeBuildingChange(getSelectedVal, sender) {
    var checkBuildFlag = false;
    var AreaId = $("#ddlLocation").val();

    try {
        var transaction = db.transaction(["tblBuildingList"], "readonly");
        var objectStore = transaction.objectStore("tblBuildingList");
        var index = objectStore.index("areaid"); // Ensure this index exists
        var request = index.getAll(AreaId);

        request.onsuccess = function (event) {
            var result = event.target.result;

            if (result.length > 0) {
                $("#ddbuilding").empty(); // Clear existing options

                $("#ddbuilding").append("<option value=''></option>");
                $.each(result, function (index, value) {
                    $("#ddbuilding").append($("<option></option>").attr("value", value.BuildingID).text(value.BuildingNo));
                    if (result.length == index + 1) {
                        checkBuildFlag = true;
                    }
                });

                if (getSelectedVal != null) {
                    $("#ddbuilding").val(getSelectedVal);
                    $("#ddbuilding").find("option:contains(" + getSelectedVal + ")").attr('selected', 'selected');
                }

                if (sender != null) {
                    var selectedBuildingId = $("#ddbuilding").val(); // Get the selected building ID
                    console.log("Selected Building ID:", selectedBuildingId);
                    changeFloorLevel(selectedBuildingId, sender); // Pass selectedBuildingId
                    changeSelectedAsset(null, sender);
                }
            } else {
                $("#ddbuilding").empty(); // Clear the dropdown if no results
            }
        };

        request.onerror = function (event) {
            console.log("Error fetching building data:", event.target.error);
        };
    } catch (error) {
        console.error("Error during transaction:", error);
    }
}



function changeSelectedAsset(getSelectedVal, sender) {
    var checkAssetFlag = false;
    var BuildingID = $("#ddbuilding").val();

    if (BuildingID != null) {
        // Open transaction
        var transaction = db.transaction(["tblAssetList"], "readonly");
        var objectStore = transaction.objectStore("tblAssetList");
        var index = objectStore.index("BuildingID"); // Assuming you have an index on BuildingID

        // Query based on BuildingID
        var request = index.getAll(BuildingID);

        request.onsuccess = function (event) {
            var result = event.target.result;

            if (result.length > 0) {
                // Clear existing options
                $("#ddassets option").each(function () {
                    $(this).remove();
                });

                var AssetListArray = result;

                // Add new options from result
                $("#ddassets").append("<option value=''></option>");
                $.each(AssetListArray, function (index, value) {
                    $("#ddassets").append($("<option></option>").attr("value", value.Assetid).text(value.Assetno));
                    if (AssetListArray.length == index + 1) {
                        checkAssetFlag = true;
                    }
                });

                if (getSelectedVal != null) {
                    console.log("AssetSelected: " + getSelectedVal);
                    $("#ddassets").val(getSelectedVal);
                    $("#ddassets").find("option:contains(" + getSelectedVal + ")").attr('selected', 'selected');
                }

                if (sender != null) {
                    changeAssetDesc();
                }

            } else {
                // Clear the dropdown if no results
                $("#ddassets option").each(function () {
                    $(this).remove();
                });

                if (sender != null) {
                    changeAssetDesc();
                }
            }
        };

        request.onerror = function (event) {
            console.log("Error fetching asset data:", event.target.error);
        };
    }

    $("#customLoader").css("display", "none");
}


function changeAssetDesc() {
    var newAssetNo = $('#ddassets option:selected').text().replace("'", "''");

    if (newAssetNo != null) {
        // Open a read-only transaction for the tblAssetList object store
        var transaction = db.transaction(["tblAssetList"], "readonly");
        var objectStore = transaction.objectStore("tblAssetList");
        var index = objectStore.index("Assetno"); // Assuming you have an index on Assetno

        // Query based on Assetno
        var request = index.get(newAssetNo);

        request.onsuccess = function (event) {
            var result = event.target.result;

            if (result) {
                // If an asset description is found, set it in the text field
                $("#txtassetsdesc").val(result.assetsdesc);
            } else {
                // If no asset description is found, clear the text field
                $("#txtassetsdesc").val('');
            }
        };

        request.onerror = function (event) {
            console.log("Error fetching asset description:", event.target.error);
            $("#txtassetsdesc").val(''); // Clear the text field on error
        };
    }
}


function AddJobRedirect() {

    UpdateRID(0);
}

function GetDate(date) {
    if (date != "" && date != null && date != undefined && date != "") {
        var newdate = date.split('/');
        return (newdate[2] + '-' + newdate[1] + '-' + newdate[0]);
    }
    else {
        return "";
    }

}

function CheckDate(InputID) {
    //var DateTagged = GetDate($('#rdpDateTagged').val());
    //var Datedis = GetDate($('#rdpdatedis').val());
    var DateTagged = $('#rdpDateTagged').val();
    var Datedis = $('#rdpdatedis').val();

    if (DateTagged != "" && DateTagged != undefined && DateTagged != null && Datedis != "" && Datedis != undefined && Datedis != null) {
        var DateTaggednew = new Date(DateTagged);
        var Datedisnew = new Date(Datedis);
        if (DateTaggednew >= Datedisnew) {
            $('#' + InputID).val('');
            if (InputID == "rdpdatedis") {
                //Commented on 14-09-2023 Renamed as Date Built
                //Res = { Result: 'Warning', Message: "Date Dismantle should be greater than Date Tagged" };
                Res = { Result: 'Warning', Message: "Date Dismantle should be greater than Date Built" };
                CustomAlert(Res);
            }
            else {
                //Commented on 14-09-2023 Renamed as Date Built
                //Res = { Result: 'Warning', Message: "Date Tagged should be less then Date Dismantle" };
                Res = { Result: 'Warning', Message: "Date Built should be less then Date Dismantle" };
                CustomAlert(Res);
            }
        }
    }



}

//function checkOnlineOffline() {
//    setTimeout(function () {
//        if (navigator.onLine) {

//            $("#onlineimg").css("display", "block");
//            $("#offlineimg").css("display", "none");
//        } else {
//            $("#onlineimg").css("display", "none");
//            $("#offlineimg").css("display", "block");
//        }
//        checkOnlineOffline();
//    }, 1000);
//}







$(function () {
    var NewRID = 0;
    //$("#rdpDateTagged,#rdpdatedis").datepicker();
    $("#rdpDateTagged").datepicker({
        dateFormat: 'dd-M-yy',
        maxDate: 0,
        onSelect: function (dateText) {
            CheckDate('rdpDateTagged');
            //added by ss on 07 feb 2025 add Regex  for enter valid date  

        },
        //added by ss on 07 feb 2025 add Regex  for enter valid date  

        onClose: function (selectedDate) {
            if (selectedDate.trim() !== "" && !isValidDateFormate(selectedDate)) {
                let todayDate = $.datepicker.formatDate('dd-M-yy', new Date());
                alert(`Invalid date! Please enter a valid date like ${todayDate} or select from the calendar.`);
                $(this).val('');
            }
        },

    });
    //$('#rdpInspectiondate').datetimepicker({
    //        format: 'd-M-Y H:i A',       // Format for date and time
    //        step: 10,                    // Time selection interval in minutes
    //        maxDate: new Date(),         // Prevent selection of future dates
    //        onSelectDate: function (ct) {
    //            console.log("Date selected: ", ct); // Log selected date
    //        },
    //        onSelectTime: function (ct) {
    //            console.log("Time selected: ", ct); // Log selected time
    //        }
    //    });

    //    // Initialize datetime picker for the #dateInspection element
    //    $('#dateInspection').datetimepicker({
    //        format: 'd-M-Y H:i A',       // Format for date and time
    //        step: 10,                    // Time selection interval in minutes
    //        maxDate: new Date(),         // Prevent selection of future dates
    //        onSelectDate: function (ct) {
    //            console.log("Date selected: ", ct); // Log selected date
    //        },
    //        onSelectTime: function (ct) {
    //            console.log("Time selected: ", ct); // Log selected time
    //        }
    //    });

    // Click handlers for calendar icon to trigger the datetime picker
    $('.calendar-icon').click(function () {
        $('#rdpInspectiondate').focus(); // Open datetime picker for #rdpInspectiondate
    });
    $('.calendar-icon').click(function () {
        $('#dateInspection').focus(); // Open datetime picker for #dateInspection
    });

    $("#rdpdatedis").datepicker({
        dateFormat: 'dd-M-yy',
        onSelect: function (dateText) {
            CheckDate('rdpdatedis');
        }
    });

    //// for Dismintal Approved Date
    $("#rddateDisAppro").datepicker({
        dateFormat: 'dd-M-yy',
        maxDate: 0,
        onSelect: function (dateText) {
            CheckDate('rddateDisAppro');
        }
    });
    $(".fa_AddNew").parent().on("click", function () {
        // Reset modal elements
        $("#OfflineListAdd").css("display", "block");
        $("#AssetDiv").css("display", "none");
        $("#assetAddnew, #customAddnew").val('');
        $("#AddlistHeader, .customAddnew").text('');
        $("#hdnNewCurrentId, #hdntblName").val('');

        const dropdownId = $(this).parent().find('select').attr('id');
        const buttonId = $(this).attr('id');
        // Map button IDs to table configurations
        const configMap = {
            btnDesign: { header: "Add Design Type", placeholder: "Add New Design Type", table: "tblDesignList" },
            btnRating: { header: "Add Duty Rating", placeholder: "Add New Duty Rating", table: "tblDutyRating" },
            btnMaterial: { header: "Add Material Type", placeholder: "Add New Material Type", table: "tblMaterialType" },
            btnLocation: { header: "Add Location", placeholder: "Add New Location", table: "tblLocationList" },
            btnBuilding: { header: "Add Building No", placeholder: "Add New Building No", table: "tblBuildingList" },
            btnAsset: { header: "Add Asset", placeholder: "Add New Asset", table: "tblAssetList", showAssetDiv: true },
            btnscaffoldowner: { header: "Add Sub Owner", placeholder: "Add New Sub Owner", table: "tblScaffoldOwner" },
            btnsubownerRep: { header: "Add Sub Owner Rep", placeholder: "Add New Sub Owner Rep", table: "tblSubOwnerRep" },
            btnWorkGroups: { header: "Add Work Groups", placeholder: "Add New Work Groups", table: "tblWorkGroups" },
            btnSubGroups: { header: "Add Sub Groups", placeholder: "Add New Sub Groups", table: "tblSubGroups" },
        };

        const config = configMap[buttonId];
        if (config) {
            $("#AddlistHeader").text(config.header);
            $(".customAddnew").text(config.placeholder);
            $("#hdnNewCurrentId").val(dropdownId);
            $("#hdntblName").val(config.table);
            if (config.showAssetDiv) $("#AssetDiv").css("display", "block");
        }

    });
    $("#btnsubmit").unbind("click").click(function () {
        const newValue = $("#customAddnew").val().trim();
        const clientId = parseInt(ClientID, 10);
        const siteLocId = $("#ddlsites").val(); // Fetch SiteLocID from dropdown
        const tableName = $("#hdntblName").val();
        const dropdownId = $("#hdnNewCurrentId").val();

        if (newValue !== "" && clientId) {
            // Route to the correct insertion function based on the table name
            handleTableInsert(tableName, newValue, clientId, siteLocId, dropdownId);
        } else {
            // Highlight missing input fields
            if (!newValue) $("#customAddnew").css("border-color", "red");
            if (!clientId) alert("ClientID must be selected.");
        }
    });

    // Central handler function
    function handleTableInsert(tableName, newValue, clientId, siteLocId, dropdownId) {
        if (tableName === "tblWorkGroups") {
            insertWorkGroups(newValue, clientId, siteLocId);

        } else if (tableName === "tblSubGroups") {
            insertSubGroup(newValue, clientId, siteLocId);
        }
        else if (tableName === "tblDesignList") {
            InsertDesignType(newValue, clientId);
        }
        else if (tableName === "tblDutyRating") {
            InsertDutyRating(newValue, clientId);
        }
        else if (tableName === "tblMaterialType") {
            InsertMaterialType(newValue, clientId);
        }
        else if (tableName === "tblMaterialType") {
            InsertMaterialType(newValue, clientId);
        }
        else if (tableName === "tblAssetList") {
            const assetsdesc = $("#assetAddnew").val();

            const dropdown = document.getElementById("ddlbuilding");
            const BuildingId = $('#ddbuilding').val();

            const BuildingNo = $('#ddbuilding option:selected').text();
            if (BuildingId) {
                InsertAsset(newValue, assetsdesc, BuildingNo, BuildingId, clientId);
            }
            else {
                const res = { Result: 'Failed', Message: 'Please select Building' };
                CustomAlert(res);
            }
        }
        else {
            console.error("No handler defined for table:", tableName);
            alert(`Unknown table: ${tableName}`);
        }
    }

    function InsertDesignType(DesignType, clientID) {
        // Open the IndexedDB
        const request = indexedDB.open(dbName, dbVersion);

        request.onsuccess = function (event) {
            const db = event.target.result;
            const transaction = db.transaction('tblDesignList', 'readwrite');
            const objectStore = transaction.objectStore('tblDesignList');
            const index = objectStore.index('ClientDesignType');

            const query = index.getAll([clientID, DesignType]);

            query.onsuccess = function (event) {
                const results = event.target.result;

                const isDuplicate = results.some(item => item.DesignType.trim().toUpperCase() === DesignType.trim().toUpperCase());
                if (isDuplicate) {
                    console.error('Duplicate DesignType found for ClientID:', clientID);
                    alert(`Error: The DesignType "${DesignType}" already exists.`);
                } else {
                    const newRecord = {
                        DesignType: DesignType,
                        ClientID: clientID,
                        insertedOffline: true
                    };

                    const addRequest = objectStore.add(newRecord);

                    addRequest.onsuccess = function () {
                        console.log('DesignType successfully added:', newRecord);
                        $("#OfflineListAdd").css("display", "none");

                        populateScaffoldDesignDropdown(clientID, function () {
                            $("#" + 'ddlScaffoldDesign').val(DesignType);
                            const res = { Result: 'Success', Message: `Successfully added: ${DesignType}` };
                            CustomAlert(res);

                        })
                    };

                    addRequest.onerror = function (error) {
                        console.error('Error adding DesignType:', error);
                    };
                }
            };

            query.onerror = function (error) {
                console.error('Error querying the database:', error);
            };
        };

        request.onerror = function (error) {
            console.error('Error opening database:', error);
        };
    }
    function InsertDutyRating(DutyRating, clientID) {
        const request = indexedDB.open(dbName, dbVersion);

        request.onsuccess = function (event) {
            const db = event.target.result;
            const transaction = db.transaction('tblDutyRating', 'readwrite');
            const objectStore = transaction.objectStore('tblDutyRating');
            const index = objectStore.index('ClientDutyRating');

            const query = index.getAll([clientID, DutyRating]);

            query.onsuccess = function (event) {
                const results = event.target.result;

                const isDuplicate = results.some(item => item.DutyRating.trim().toUpperCase() === DutyRating.trim().toUpperCase());
                if (isDuplicate) {
                    console.error('Duplicate DutyRating found for ClientID:', clientID);
                    //alert(`Error: The DutyRating "${DutyRating}" already exists for this ClientID (${clientID}).`);
                    alert(`Error: The DutyRating "${DutyRating}" already exists .`);
                } else {
                    const newRecord = {
                        DutyRating: DutyRating,
                        ClientID: parseInt(clientID, 10),
                        insertedOffline: true

                    };

                    const addRequest = objectStore.add(newRecord);

                    addRequest.onsuccess = function () {
                        console.log('DutyRating successfully added:', newRecord);
                        $("#OfflineListAdd").css("display", "none");

                        populateDutyRatingDropdown(clientID, function () {
                            $("#" + 'ddlDutyrating').val(DutyRating);
                            const res = { Result: 'Success', Message: `Successfully added: ${DutyRating}` };
                            CustomAlert(res);

                        })
                    };

                    addRequest.onerror = function (error) {
                        console.error('Error adding DutyRating:', error);
                    };
                }
            };

            query.onerror = function (error) {
                console.error('Error querying the database:', error);
            };
        };

        request.onerror = function (error) {
            console.error('Error opening database:', error);
        };
    }
    function InsertMaterialType(MaterialType, clientID) {
        const request = indexedDB.open(dbName, dbVersion);

        request.onsuccess = function (event) {
            const db = event.target.result;
            const transaction = db.transaction('tblMaterialType', 'readwrite');
            const objectStore = transaction.objectStore('tblMaterialType');
            const index = objectStore.index('ClientMaterialType');

            const query = index.getAll([clientID, MaterialType]);

            query.onsuccess = function (event) {
                const results = event.target.result;

                const isDuplicate = results.some(item => item.MaterialType.trim().toUpperCase() === MaterialType.trim().toUpperCase());
                if (isDuplicate) {
                    console.error('Duplicate MaterialType found for ClientID:', clientID);
                    //alert(`Error: The MaterialType "${MaterialType}" already exists for this ClientID (${clientID}).`);
                    alert(`Error: The MaterialType "${MaterialType}" already exists.`);
                } else {
                    const newRecord = {
                        MaterialType: MaterialType,
                        ClientID: clientID,
                        insertedOffline: true

                    };

                    const addRequest = objectStore.add(newRecord);

                    addRequest.onsuccess = function () {
                        console.log('MaterialType successfully added:', newRecord);
                        $("#OfflineListAdd").css("display", "none");

                        populateMaterialTypeDropdown(clientID, function () {
                            $("#" + 'ddlMaterialType').val(MaterialType);
                            const res = { Result: 'Success', Message: `Successfully added: ${MaterialType}` };
                            CustomAlert(res);

                        })
                    };

                    addRequest.onerror = function (error) {
                        console.error('Error adding MaterialType:', error);
                    };
                }
            };

            query.onerror = function (error) {
                console.error('Error querying the database:', error);
            };
        };

        request.onerror = function (error) {
            console.error('Error opening database:', error);
        };
    }


    function insertSubGroup(subGroup, clientID, siteLocID) {
        // Open the IndexedDB
        const request = indexedDB.open(dbName, dbVersion);

        request.onsuccess = function (event) {
            const db = event.target.result;
            const transaction = db.transaction('tblSubGroups', 'readwrite');
            const objectStore = transaction.objectStore('tblSubGroups');
            const index = objectStore.index('ClientSiteIndex');

            const query = index.getAll([clientID, siteLocID]);

            query.onsuccess = function (event) {
                const results = event.target.result;

                const isDuplicate = results.some(item => item.SubGroups.trim().toUpperCase() === subGroup.trim().toUpperCase());
                if (isDuplicate) {
                    console.error('Duplicate SubGroup found for ClientID:', clientID, 'and SiteLocID:', siteLocID);
                    //alert(`Error: The SubGroup "${subGroup}" already exists for this ClientID (${clientID}) and SiteLocID (${siteLocID}).`);
                    alert(`Error: The SubGroup "${subGroup}" already exists .`);
                } else {
                    const newRecord = {
                        SubGroups: subGroup,
                        ClientID: clientID,
                        SiteLocID: siteLocID,
                        insertedOffline: true

                    };

                    const addRequest = objectStore.add(newRecord);

                    addRequest.onsuccess = function () {
                        console.log('SubGroup successfully added:', newRecord);
                        $("#OfflineListAdd").css("display", "none");

                        populateSubGroup(parseInt(siteLocID, 10), function () {
                            $("#" + 'ddlSubGroups').val(subGroup);
                            const res = { Result: 'Success', Message: `Successfully added: ${subGroup}` };
                            CustomAlert(res);

                        })
                    };

                    addRequest.onerror = function (error) {
                        console.error('Error adding SubGroup:', error);
                    };
                }
            };

            query.onerror = function (error) {
                console.error('Error querying the database:', error);
            };
        };

        request.onerror = function (error) {
            console.error('Error opening database:', error);
        };
    }
    function insertWorkGroups(workgroup, clientID, siteLocID) {
        // Open the IndexedDB
        const request = indexedDB.open(dbName, dbVersion);

        request.onsuccess = function (event) {
            const db = event.target.result;
            const transaction = db.transaction('tblWorkGroups', 'readwrite');
            const objectStore = transaction.objectStore('tblWorkGroups');
            const index = objectStore.index('ClientSiteIndex');

            const query = index.getAll([clientID, siteLocID]);

            query.onsuccess = function (event) {
                const results = event.target.result;

                const isDuplicate = results.some(item => item.WorkGroups.trim().toUpperCase() === workgroup.trim().toUpperCase());
                if (isDuplicate) {
                    console.error('Duplicate SubGroup found for ClientID:', clientID, 'and SiteLocID:', siteLocID);
                    //alert(`Error: The SubGroup "${workgroup}" already exists for this ClientID (${clientID}) and SiteLocID (${siteLocID}).`);
                    alert(`Error: The SubGroup "${workgroup}" already exists.`);
                } else {
                    const newRecord = {
                        WorkGroups: workgroup,
                        ClientID: clientID,
                        SiteLocID: siteLocID,
                        insertedOffline: true

                    };

                    const addRequest = objectStore.add(newRecord);

                    addRequest.onsuccess = function () {
                        console.log('Work Group successfully added:', newRecord);
                        $("#OfflineListAdd").css("display", "none");

                        populateWorkGroup(parseInt(siteLocID, 10), function () {
                            $("#" + 'ddlWorkGroups').val(workgroup);
                            const res = { Result: 'Success', Message: `Successfully added: ${workgroup}` };
                            CustomAlert(res);

                        })
                    };

                    addRequest.onerror = function (error) {
                        console.error('Error adding SubGroup:', error);
                    };
                }
            };

            query.onerror = function (error) {
                console.error('Error querying the database:', error);
            };
        };

        request.onerror = function (error) {
            console.error('Error opening database:', error);
        };
    }
    function InsertAsset(Assetno, assetsdesc, BuildNo, BuildingID, clientID) {

        const request = indexedDB.open(dbName, dbVersion);
        request.onsuccess = function (event) {
            const db = event.target.result;
            const transaction = db.transaction('tblAssetList', 'readwrite');
            const objectStore = transaction.objectStore('tblAssetList');
            const index = objectStore.index('ClientBuildingID');

            const query = index.getAll([clientID, parseInt(BuildingID, 10)]);

            query.onsuccess = function (event) {
                const results = event.target.result;

                //const isDuplicate = results.some(item => item.Assetno.trim().toUpperCase() === Assetno.trim().toUpperCase());
                const isDuplicate = results.some(item => (item.Assetno ? item.Assetno.trim().toUpperCase() : "null") === (Assetno ? Assetno.trim().toUpperCase() : "null"));

                if (isDuplicate) {
                    console.error('Duplicate Assetno found for ClientID:', clientID);
                    //alert(`Error: The Assetno "${Assetno}" already exists for this ClientID (${clientID}).`);
                    alert(`Error: The Assetno "${Assetno}" already exists .`);
                } else {
                    const Assetid = Date.now().toString();

                    const newRecord = {
                        Assetid: Date.now().toString(),
                        Assetno: Assetno,
                        BuildNo: BuildNo,
                        BuildingID: parseInt(BuildingID, 10),
                        ClientID: clientID,
                        assetsdesc: assetsdesc,
                        insertedOffline: true


                    };

                    const addRequest = objectStore.add(newRecord);

                    addRequest.onsuccess = function () {
                        console.log('Assetno successfully added:', newRecord);
                        $("#OfflineListAdd").css("display", "none");
                        populateAssetListDropdown(parseInt(BuildingID, 10), function () {
                            $("#" + 'ddassets').val(Assetid);
                            const res = { Result: 'Success', Message: `Successfully added: ${Assetno}` };
                            CustomAlert(res);

                        })
                    };

                    addRequest.onerror = function (error) {
                        console.error('Error adding Assetno:', error);
                    };
                }
            };

            query.onerror = function (error) {
                console.error('Error querying the database:', error);
            };
        };

        request.onerror = function (error) {
            console.error('Error opening database:', error);
        };
    }

    // Reusable insert function for all tables
    function insertRecord(tableName, options) {
        const { keyField, newValue, clientId, siteLocId, dropdownId } = options;
        const dbRequest = indexedDB.open(dbName, dbVersion);

        dbRequest.onsuccess = function () {
            const db = dbRequest.result;
            const tx = db.transaction([tableName], "readwrite");
            const store = tx.objectStore(tableName);

            const getAllRequest = store.getAll(); // Fetch all records to check for duplicates

            getAllRequest.onsuccess = function () {
                const records = getAllRequest.result;
                let duplicateFound = false;

                // Dynamic duplicate check based on table and field availability
                records.forEach(record => {
                    if (record[keyField] === newValue && record.ClientID === clientId) {
                        if (!siteLocId || record.SiteLocID === siteLocId) {
                            duplicateFound = true;
                        }
                    }
                });

                if (duplicateFound) {
                    //alert(`The entry "${newValue}" already exists in ${tableName}.`);
                    alert(`The entry "${newValue}" already exists.`);
                } else {
                    // No duplicate, proceed to add the new record
                    const newRecord = {
                        [keyField]: newValue,
                        ClientID: clientId,
                    };

                    // Add SiteLocID only if applicable for the table
                    if (siteLocId) {
                        newRecord.SiteLocID = siteLocId;
                    }

                    const addRequest = store.add(newRecord);

                    addRequest.onsuccess = function () {
                        // Update the dropdown
                        $("#" + dropdownId).append($('<option></option>').attr("value", newValue).text(newValue));
                        $("#" + dropdownId).val(newValue);
                        $("#OfflineListAdd").css("display", "none");
                        const res = { Result: 'Success', Message: `Successfully added: ${newValue}` };
                        CustomAlert(res);
                    };

                    addRequest.onerror = function (event) {
                        console.error(`Error adding data to ${tableName}:`, event.target.error);
                        alert(`Error adding data to ${tableName}.`);
                    };
                }
            };

            getAllRequest.onerror = function (event) {
                console.error(`Error fetching records from ${tableName}:`, event.target.error);
                alert(`Error checking for duplicates in ${tableName}.`);
            };
        };

        dbRequest.onerror = function (event) {
            console.error("Error opening database:", event.target.error);
            alert("Error opening database.");
        };
    }





    setTimeout(function () {
        //if (navigator.onLine) {
        //    $("#onlineimg").css("display", "block");
        //    $("#offlineimg").css("display", "none");
        //    $("#btnAddRegister").css("display", "none");

        //    if (OfflineStatus == 1) {
        //        if (NewRID > 0) {
        //            $(".respText").text("Save");
        //            $("#btnAddRegister").find(".text").text("Add Job");
        //            $("#btnAddRegister").css("display", "block");
        //            $("#btnShowOfflineList").css("display", "block");
        //            showOfflineRegister();
        //            showOfflineCyclone();
        //        } else {
        //            $("#customLoader").css("display", "block");
        //            showOfflineDropDownList();
        //        }
        //    } else if (OfflineStatus == 0) {
        //        var transaction = db.transaction("tblRegisterDetails", "readonly");
        //        var store = transaction.objectStore("tblRegisterDetails");
        //        var request = store.getAll();

        //        request.onsuccess = function (event) {
        //            if (event.target.result.length > 0) {
        //                bootbox.confirm("Do you want to Sync your LOCAL data to CLOUD server?", function (result) {
        //                    if (result) {
        //                        $("#customLoader").css("display", "block");
        //                        updateLocationList();
        //                        $("#btnShowOfflineList").css("display", "none");
        //                    } else {
        //                        CreateDbTables();
        //                        $("#btnShowOfflineList").css("display", "none");
        //                        location.reload();
        //                    }
        //                });
        //            } else {
        //                bootbox.confirm("Do you want to save data on Local Server?", function (result) {
        //                    if (result) {
        //                        var NewUrl = window.location.href.split('offlinestatus');
        //                        $("#customLoader").css("display", "block");
        //                        CreateDbTables();
        //                        ShowOnlineDropDownList();

        //                        var interval = setInterval(function () {
        //                            if (dropdownCheck === true) {
        //                                var transaction = db.transaction("tblDesignList", "readonly");
        //                                var store = transaction.objectStore("tblDesignList");
        //                                var request = store.getAll();

        //                                request.onsuccess = function (event) {
        //                                    if (event.target.result.length > 0) {
        //                                        dropdownCheck = false;
        //                                        var encodedString = window.btoa("1");
        //                                        window.location = NewUrl[0] + "offlinestatus=" + encodedString + "";
        //                                    }
        //                                };
        //                            }
        //                        }, 2000);
        //                    } else {
        //                        CreateDbTables();
        //                        ShowOnlineDropDownList();
        //                        $("#btnShowOfflineList").css("display", "none");
        //                    }
        //                });
        //            }
        //        };
        //    }

        //    $(".respText").text("Save");
        //    $("#btnAddRegister").find(".text").text("Add Job");

        //} else {
        //    $("#onlineimg").css("display", "none");
        //    $("#offlineimg").css("display", "block");

        //    var transaction = db.transaction("tblRegisterDetails", "readonly");
        //    var store = transaction.objectStore("tblRegisterDetails");
        //    var request = store.getAll();

        //    request.onsuccess = function (event) {
        //        if (event.target.result.length > 0) {
        //            $("#btnShowOfflineList").css("display", "block");
        //        } else {
        //            $("#btnShowOfflineList").css("display", "none");
        //        }
        //    };

        //    if (NewRID > 0) {
        //        $(".respText").text("Save");
        //        $("#btnAddRegister").css("display", "block");
        //        $("#btnShowOfflineList").css("display", "block");
        //        $("#btnAddRegister").find(".text").text("Add Job");
        //        showOfflineRegister();
        //        showOfflineCyclone();
        //    } else {
        //        showOfflineDropDownList();
        //        $(".respText").text("Save");
        //        $("#btnAddRegister").find(".text").text("Add Job");
        //    }
        //}

        // Add or Update Register Details
        //$("#btnSaveRegister,#txtsaveBottom,#btnImageSave").on("click", function () {
        //    if (RID > 0) {
        //        var Site = $("#ddlsites").val();
        //        UpdateLocalData();
        //        //AddLocalCycloneData(RID);
        //    } else {
        //        AddLocalData();
        //    }
        //});

        $("#btnSaveRegister").on("click", function () {
            var workGroupId = $('#ddlWorkGroups').val();
            var subGroupId = $('#ddlSubGroups').val();

            if (subGroupId && (!workGroupId || workGroupId === "")) {
                CustomAlert({ Result: 'Warning', Message: "Please fill the Work Group first before the Sub Group." });
                $('#LoadingPanel1').hide();
                return;
            }
            if (RID > 0) {
                var Site = $("#ddlsites").val();
                UpdateLocalData();
                AddLocalCycloneData(RID);
                GetManHrsRatio(ClientID);
            } else {

            }
        });

        $("#txtsaveBottom,#btnImageSave").on("click", function () {
            var workGroupId = $('#ddlWorkGroups').val();
            var subGroupId = $('#ddlSubGroups').val();

            if (subGroupId && (!workGroupId || workGroupId === "")) {
                CustomAlert({ Result: 'Warning', Message: "Please fill the Work Group first before the Sub Group." });
                $('#LoadingPanel1').hide();
                return;
            }
            if (RID > 0) {
                var Site = $("#ddlsites").val();
                UpdateLocalData();
                GetManHrsRatio(ClientID);
            } else {

            }
        });

        $("#OfflinebtnSaveRegister").on("click", function () {
            var workGroupId = $('#ddlWorkGroups').val();
            var subGroupId = $('#ddlSubGroups').val();

            if (subGroupId && (!workGroupId || workGroupId === "")) {
                CustomAlert({ Result: 'Warning', Message: "Please fill the Work Group first before the Sub Group." });
                $('#LoadingPanel1').hide();
                return;
            }
            if (RID > 0) {
                var Site = $("#ddlsites").val();
                AddLocalRegisterimageData(RID);
            } else {

            }
        });



        $("#btnShowOfflineList").on("click", function () {  // On button Click Showing Offline Grid
            GetCount = 0;
            showOfflineGrid();
        });

        // Showing Confirmation Popup with Locked Image
        $("#ddlLockedIn").on("change", function () {
            if ($(this).val() == 1) {
                $("#PopLocked").css("display", "block");

                $("#btnlockedyes").on("click", function () {
                    if ($("#ddlLocksignedOffBy").val() != "") {
                        $("#PopLocked").css("display", "none");
                        $("#imgLocked").css("display", "block");
                        $("#divhideLockedtextbox").css("display", "block");
                        var Signedoff = $("#ddlLocksignedOffBy :selected").text();
                        $("#txtlockedsignby").val(Signedoff);
                    } else {
                        alert("Please fill the Signed Off By.");
                    }
                });

                $("#btnlockedno").on("click", function () {
                    $("#PopLocked").css("display", "none");
                    $("#divhideLockedtextbox").css("display", "none");
                    $("#ddlLockedIn").val(0);
                });
            } else {
                $("#imgLocked").css("display", "none");
                $("#divhideLockedtextbox").css("display", "none");
            }
        });

        //Update Cyclone Details in tblRegisterDetails
        $("#btnCycloneSave").on("click", function () {

            AddLocalCycloneData(RID);
            setTimeout(function () {
                const currentUrl = new URL(window.location.href);
                currentUrl.searchParams.set('activateTab', 'Cyclone');
                window.location.href = currentUrl.toString();
            }, 1500);

        });


    }, 1000);


});

function DrawingNoDisable() {
    const EngineeredDesign = $("#ddlEngineeredDesign").val();;
    if (EngineeredDesign === 'NO' || EngineeredDesign === 'N/A') {
        $('#txtDesignDrawingNo').prop('disabled', true);
        $('#txtDesignDrawingNo').val('');
    } else {
        $('#txtDesignDrawingNo').prop('disabled', false);
    }
}


function UpdateLocalData() {
    $('#LoadingPanel1').show();

    var workingDocks = 0;
    var updateCheckFlag = true;
    var CheckMessage = "";
    if (!RID) {
        CustomAlert({ Result: "Warning", Message: "Register ID is required for update!" });
        return;
    }
    if ($("#ddlsites").val() == "" ||
        $("#ddlLocation option:selected").text() == "" ||
        $("#DropDownList1 option:selected").text() == "" ||
        $("#txtplnstandDur").val() == "") {
        updateCheckFlag = false;
        CheckMessage = "RequriedFieldMessage";
    }

    if (updateCheckFlag) {
        if ($("#DropDownList2").val() === "N/A") {
            workingDocks = 0;
        } else {
            workingDocks = parseInt($("#DropDownList2").val());
        }

        if ($("#txtNoOfPlatform").val() === "") {
            noOfPlatform = 0;
        } else {
            noOfPlatform = parseInt($("#txtNoOfPlatform").val());
        }

        if (workingDocks >= noOfPlatform) {
            var EngineeredDesign = $("#ddlEngineeredDesign").val();
            var DrawingNo = $("#txtDesignDrawingNo").val();

            if (EngineeredDesign === 'YES' && (DrawingNo === '' || DrawingNo === 'N/A')) {
                updateCheckFlag = false;
                CheckMessage = "EngineeredDesignDrawingNoMessage"; // Message for invalid drawing number
            }
            //else if (EngineeredDesign === 'NO' || EngineeredDesign === 'N/A') {
            //    //updateCheckFlag = false;
            //    //CheckMessage = "EngineeredDesignDrawingNoMessage";
            //}
            else {
                updateCheckFlag = true;
            }
        } else {
            updateCheckFlag = false;
            CheckMessage = "WorkingDocksMessage";
        }
    }

    if (!updateCheckFlag) {
        if (CheckMessage === "RequriedFieldMessage") {
            CustomAlert({ Result: 'Warning', Message: "Please fill 'FloorLevel' And 'planstanding' required field then click on save." });
        }
        if (CheckMessage === "WorkingDocksMessage") {
            CustomAlert({ Result: 'Warning', Message: "Max Number of Decks loaded at one time cannot be greater than Working Decks." });
        }
        if (CheckMessage === "EngineeredDesignDrawingNoMessage") {
            CustomAlert({ Result: 'Warning', Message: "Design Drawing Number is required when Engineered Design is 'YES'." });
        }
        return;
    }
    const dbRequest = indexedDB.open(dbName, dbVersion);

    dbRequest.onerror = function (event) {
        console.error("Database error: " + event.target.errorCode);
    };

    dbRequest.onsuccess = function (event) {
        const db = event.target.result;

        // Begin a transaction
        const transaction = db.transaction(["tblRegisterDetails"], "readwrite");
        const store = transaction.objectStore("tblRegisterDetails");

        // Fetch the record by Register ID
        const getRequest = store.get(Number(RID)); // Ensure `txtRegisterId` is a number

        getRequest.onsuccess = function (event) {
            const record = event.target.result;

            if (!record) {
                CustomAlert({ Result: "Warning", Message: "No record found for the given Register ID!" });
                return;
            }


            // Update process
            record.SiteLocation = $('#txtSiteName').val();
            record.Location = $('#txtSiteLocation').val();
            record.InspectedBy = $('#txtInspectedBy').val();
            record.RecentInspDate = $('#txtRecentinspectiondate').val();
            record.DaysSinceLastInsp = $('#txtDaylastinspetion').val();
            record.Status = $('#txtstatus').val();
            record.PlanningStatus = $('#txtPlannedStatus').val();
            record.HireDaysToDate = $('#txtHireDays').val();
            record.TotalHireDays = $('#txtTotalhireday').val();
            record.NoOfItems = $('#txtNoOfItem').val();
            record.Tonnage = $('#txttonnage').val();
            record.OptimizeStatus = $('#txtoptstatus').val();
            record.ShowRegisterID = $('#txtscaffref').val();
            record.OwnerName = $('#txtOwnername').val();
            record.discipline = $('#txtdiscipline').val();
            record.Bearing = $("#txtbearning").val();
            record.WorkGroup = $("#ddlWorkGroups").val();
            record.SubGroup = $("#ddlSubGroups").val();
            record.RequestedBy = $("#txtRequestedBy").val();
            record.DesignType = $("#ddlScaffoldDesign").val();
            record.DutyRating = $("#ddlDutyrating").val();
            record.MaterialType = $("#ddlMaterialType").val();
            record.Description = $("#txtdesc").val();
            record.InspTimeframe = $("#ddlInspectionTimeFrame").val();
            record.WONumber = $("#txtWOnumb").val();
            record.Length = $("#txtlength").val();
            record.Width = $("#txtwidth").val();
            record.Height = $("#txtheight").val();
            record.NoOfWorkingDecks = $("#DropDownList2").val();
            record.ManHrsErect = $("#txtManhrserect").val();
            record.ManHrsDismantle = $("#txtmanDisman").val();
            record.PlannedStandingDuration = $("#txtplnstandDur").val();
            record.PlanningStatus = $("#ddlPlanningStatus").val();
            record.ShiftAllocation = $("#ddlShiftAllocation").val();
            record.EngineeredDesign = $("#ddlEngineeredDesign").val();
            record.DesignDrawingNo = $("#txtDesignDrawingNo").val();
            record.NumberOfPlatform = $("#txtNoOfPlatform").val();
            record.Supervisor = $('#ddlsupervisor option:selected').text();
            record.SupervisorId = $("#ddlsupervisor").val();
            record.Crew = $("#txtCrew").val();
            record.PermitNo = $("#txtPermit").val();
            record.CrewSize = $("#txtCrewSize").val();
            var AssitId = parseInt($('#ddassets').val(), 10);
            var Asset = $('#ddassets option:selected').text();
            var FloorLevelId = parseInt($('#DropDownList1').val(), 10);
            var FloorLevel = $('#DropDownList1 option:selected').text();
            var BuildingId = parseInt($('#ddbuilding').val(), 10);
            var Building = $('#ddbuilding option:selected').text()

            record.Comments = $("#txtcomment").val();

            // Checkboxes
            //record.MaintainInventory = !$('#checkboxMaintainInventory').prop('checked');
            record.LongTermStructure = $('#checkLTStructure').prop('checked');
            record.NoInventory = $('#chkNoInventory').prop('checked');
            record.ChkRequiresIsolation = $('#chkReqis').prop('checked');

            // Dates
            record.DateTagged = $('#rdpDateTagged').val();
            record.DateOFDismantle = $('#rdpdatedis').val();
            record.DismantleDateTXT = $('#txtDatedismantle').val();
            //image
            record.Image = $('#basetempimg').val() || record.Image;
            record.PhotoAngle = $('#imgangle').val();
            var txtOwnername = $('#txtOwnername').val();
            var tempimg = $('#tempimg').val();
            /*            var basetempimg = $('#basetempimg').val();*/
            var radUpdogImg = $('#radUpdogImg').val();
            var hdnimgname = $('#hdnimgname').val();
            //record.tempimg = tempimg;
            //record.basetempimg = basetempimg;
            record.radUpdogImg = radUpdogImg;
            record.hdnimgname = hdnimgname;


            record.AssetId = AssitId;
            record.AssetNo = Asset;
            record.FloorLevel = FloorLevel;
            record.FloorLevelId = FloorLevelId;
            record.BuildingID = BuildingId;
            record.BuildingNo = Building;
            record.UpdatedOffline = true;

            const updateRequest = store.put(record);

            CustomAlert({ Result: "Success", Message: "Record updated successfully!" });
        };

        getRequest.onerror = function (event) {
            console.error("Error fetching data for update:", event.target.error);
            //CustomAlert({ Result: "Error", Message: "Failed to fetch record for update!" });
            CustomAlert({ Result: "Error", Message: "Failed to update the record. Please try again." })
        };

        transaction.oncomplete = function () {
            console.log("Transaction completed: update operation finished.");
            GetTblregisterDataById(RID);

        };
    };
}

checkOnlineOffline();


//function checkImageSize(input) {
//    const maxFileSize = 2 * 1024 * 1024;
//    const file = input.files[0];

//    if (file) {
//        if (file.size > maxFileSize) {
//            input.value = '';
//            var Response = { Result: "Failed", Message: "The selected image exceeds the 2 MB size limit. Please upload an image smaller than 2 MB." };
//            CustomAlert(Response);
//            input.value = '';
//        } else {
//            console.log('Image size is within the allowed limit.');
//        }
//    }
//}

function deletecheck() {
    var REGISTERId = OffDecrypt(getParameterByName('RID'));

    if (!REGISTERId) {
        console.error("Invalid Register ID.");
        alert("Unable to process the request due to an invalid Register ID.");
        return;
    }
    const transaction = db.transaction(["tblRegisterDetails"], "readwrite");
    const objectStore = transaction.objectStore("tblRegisterDetails");

    const getRequest = objectStore.get(parseInt(REGISTERId, 10));

    getRequest.onsuccess = function (event) {
        const record = event.target.result;

        if (record) {
            record.Image = "";

            const updateRequest = objectStore.put(record);

            updateRequest.onsuccess = function () {
                console.log("Image column cleared successfully in IndexedDB.");
                document.getElementById("imga").src = "images/noimg.jpg";
                $("#tempimg").val("");
                $("#radUpdogImg").val("");
                $("#basetempimg").val("");
                $("#deleteimage").hide();
            };

            updateRequest.onerror = function (event) {
                console.error("Failed to update the record in IndexedDB:", event.target.error);
                alert("Failed to clear the image data. Please try again.");
            };
        } else {
            console.error("No record found with the given Register ID.");
            alert("No record found to delete the image.");
        }
    };

    getRequest.onerror = function (event) {
        console.error("Failed to retrieve the record from IndexedDB:", event.target.error);
        alert("Failed to retrieve the data. Please try again.");
    };
}



function checkOnlineOffline() {
    setTimeout(function () {
        if (navigator.onLine) {

            $("#onlineimg").css("display", "block");
            $("#offlineimg").css("display", "none");
            $("#btnUploadtoCloud").css("display", "block");
        } else {
            $("#onlineimg").css("display", "none");
            $("#offlineimg").css("display", "block");
            $("#btnUploadtoCloud").css("display", "none");
        }
        checkOnlineOffline();
    }, 1000);
}

function GetTblregisterDataById(REGISTERId) {
    if (!REGISTERId) {
        console.error("REGISTERId is required");
        return;
    }

    var request = indexedDB.open(dbName, dbVersion);

    request.onerror = function (event) {
        console.error("Error opening IndexedDB:", event);
    };

    request.onsuccess = function (event) {
        var db = event.target.result;
        var transaction = db.transaction(["tblRegisterDetails"], "readonly");
        var objectStore = transaction.objectStore("tblRegisterDetails");

        // Fetch the record where REGISTERId matches
        var getRequest = objectStore.get(Number(REGISTERId));
        getRequest.onerror = function (event) {
            console.error("Error retrieving record:", event);
        };

        getRequest.onsuccess = function (event) {
            var record = event.target.result;

            if (record) {
                // Process and display the fetched record data
                displayRegisterDetails(record);
            } else {
                console.log("No matching record found for REGISTERId:", REGISTERId);
            }
        };
    };

    request.onupgradeneeded = function (event) {
        console.log("Database upgrade needed.");
    };
}


function SetDateFormat(date) {
    if (date != null && date != undefined && date != "") {
        date = date.toString();
        var newDate = new Date(parseInt(date.substr(6)));
        var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
        var formattedDate =
            ("0" + newDate.getDate()).slice(-2) + '-' +
            monthNames[newDate.getMonth()] + '-' +
            newDate.getFullYear();
        return formattedDate.toString();
    } else {
        return "";
    }
}
function displayRegisterDetails(record) {
    // Dropdown population

    //populateBuildingListDropdown();
    //populateAssetListDropdown();
    //populateSubGroup();
    //populateWorkGroup();

    // Assign values to form fields based on the fetched record
    //var PhotoAngle = record.PhotoAngle || "";
    var DismantleApprovedDate = record.DismantleApprovedDate || "";
    var DismantleApprovedNotes = record.DismantleApprovedNotes || "";
    var DateTagged = record.DateTagged || "";
    var DateTaggedTXT = record.DateTaggedTXT || "";
    var DateOFDismantle = record.DateOFDismantle || "";
    var PipelineStartDate = record.PipelineStartDate || "";
    var PipeLineNote = record.PipeLineNote || "";
    var Plans = record.Plans || "";
    //var InventoryCount = record.InventoryCount || 0;
    //var AvailableQuantity = record.AvailableQuantity || 0;
    //var OptimizeStatus = record.OptimizeStatus || "";
    var MaintainInventory = record.MaintainInventory || false;
    var ClientScaffref = record.ClientScaffref || "";
    var NoInventory = record.NoInventory || false;
    var IsOfflineCurrent = record.IsOfflineCurrent || false;
    //var Status = record.Status || '';
    //var LockedIn = record.Status || '';
    //Job  Info 
    $('#txtSiteName').val(record.SiteLocation || '');
    $('#txtSiteLocation').val(record.Location || '');
    $('#txtInspectedBy').val(record.InspectedBy || '');
    $('#txtRecentinspectiondate').val(record.RecentInspDate || '');
    $('#txtDaylastinspetion').val(record.DaysSinceLastInsp || '');
    $('#txtstatus').val(record.Status || '');
    $('#txtPlannedStatus').val(record.PlanningStatus || '');
    $('#txtHireDays').val(record.HireDaysToDate || '');
    $('#txtTotalhireday').val(record.TotalHireDays || '');
    $('#txtNoOfItem').val(record.NoOfItems || '');
    $('#txttonnage').val(record.Tonnage || '');
    $('#txtoptstatus').val(record.OptimizeStatus || '');
    $('#txtscaffref').val(record.ShowRegisterID || '');


    // Handling form field assignments
    $('#txtOwnername').val(record.OwnerName || '');
    $('#txtdiscipline').val(record.discipline || '');
    $("#txtbearning").val(record.Bearing || '');
    $("#ddlWorkGroups").val(record.WorkGroup || '');
    $("#ddlSubGroups").val(record.SubGroup || '');
    $("#txtRequestedBy").val(record.RequestedBy || '');
    //details
    $("#ddlScaffoldDesign").val(record.DesignType || '');
    $("#ddlDutyrating").val(record.DutyRating || '');
    $("#ddlMaterialType").val(record.MaterialType || '');
    $("#txtdesc").val(record.Description || '');
    $("#ddlInspectionTimeFrame").val(record.InspTimeframe || '');
    //More Informetion 
    $("#txtWOnumb").val(record.WONumber || '');
    $("#txtlength").val(record.Length || '');
    $("#txtwidth").val(record.Width || '');
    $("#txtheight").val(record.Height || '');
    $("#DropDownList2").val(record.NoOfWorkingDecks || '');
    $("#txtManhrserect").val(record.ManHrsErect || '');
    $("#txtmanDisman").val(record.ManHrsDismantle || '');
    $("#txtplnstandDur").val(record.PlannedStandingDuration || '');
    $("#ddlPlanningStatus").val(record.PlanningStatus || '');
    $("#ddlShiftAllocation").val(record.ShiftAllocation || '');
    $("#ddlEngineeredDesign").val(record.EngineeredDesign || '');
    $("#txtDesignDrawingNo").val(record.DesignDrawingNo || '');
    $("#txtNoOfPlatform").val(record.NumberOfPlatform || '');
    $("#ddlsupervisor").val(record.Supervisor || '');
    $("#txtCrew").val(record.Crew || '');
    $("#txtPermit").val(record.PermitNo || '');
    $("#txtCrewSize").val(record.CrewSize || '');
    $("#ddlLockedIn").val(record.LockedIn || '');
    $("#txtcomment").val(record.Comments || '');

    const EngineeredDesign = record.EngineeredDesign || '';
    if (EngineeredDesign == 'NO' || EngineeredDesign == 'N/A') {
        $('#txtDesignDrawingNo').prop('disabled', true);
    } else {
        $('#txtDesignDrawingNo').prop('disabled', false);
    }

    // Checkboxes
    //const MaintainInventory = record.MaintainInventory || false;
    //$('#checkboxMaintainInventory').prop('checked', !MaintainInventory);
    //$('#chkNoInventory').prop('checked', record.NoInventory || false);
    //$('#chkReqis').prop('checked', record.ChkRequiresIsolation);
    //$('#checkboxMaintainInventory').prop('checked', !MaintainInventory);
    $('#checkLTStructure').prop('checked', record.LongTermStructure);
    $('#chkNoInventory').prop('checked', NoInventory);
    $('#chkReqis').prop('checked', record.ChkRequiresIsolation);

    // Dates
    $('#PlannedDate').prop('disabled', DateTagged === "");
    if (DateTagged) $('#rdpDateTagged').val(DateTagged);
    $('#txtdatetagged').val(DateTagged);
    if (DateOFDismantle) {
        $('#rdpdatedis').val(SetDateFormat(DateOFDismantle));
        $('#txtDatedismantle').val(SetDateFormat(DateOFDismantle));
    }
    // Handle Image
    const base64Image = record.Image
    //const Image = record.Image || "";
    const PhotoAngle = record.PhotoAngle || "0";
    $('#deleteimage, #btnImageSave, #downloadButton').css('display', base64Image ? 'block' : 'none');
    //$('#imga').attr("src", base64Image ? `/images/sites/${base64Image}` : "/images/noimg.jpg");
    $('#imga').attr("src", base64Image ? base64Image : "/images/noimg.jpg");

    $('#getimage').val(base64Image);

    // Rotate image if PhotoAngle is present
    $('#imgangle').val(PhotoAngle);
    if (PhotoAngle) ImageRotate(PhotoAngle);

    // Populate dependent dropdowns based on SiteId, LocationId, BuildingID, and FloorLevelId
    const SiteId = record.SiteLocID || "";
    const LocationId = record.LocationId || "";
    const BuildingID = record.BuildingID || "";
    const FloorLevelId = record.FloorLevelId || "";
    const ClientIds = record.ClientID || "";
    const AssitID = record.AssetId || "";
    const WorkGroup = record.WorkGroup || "";
    const SubGroup = record.SubGroupID || "";


    if (SiteId) {

        populateSiteListDropdown(function () {
            $('#ddlsites').val(SiteId);
            $('#txtOwnername').val(record.OwnerName);
            console.log("Site selected:", SiteId);
            populateScaffoldDesignDropdown(record.ClientID, function () {
                console.log("Design type  dropdown added success fully");
                $('#ddlScaffoldDesign').val(record.DesignType);

            });
            populateDutyRatingDropdown(record.ClientID, function () {
                console.log("Duty Rating  dropdown added success fully");
                $('#ddlDutyrating').val(record.DutyRating);

            });
            populateMaterialTypeDropdown(record.ClientID, function () {
                console.log("Material Type  dropdown added success fully");
                $('#ddlMaterialType').val(record.MaterialType);

            });
            populateSupervisorDropdown(record.ClientID, function () {
                console.log("SuperViser  dropdown added success fully");
                $('#ddlsupervisor').val(record.SupervisorId);

            });
            populateWorkGroup(SiteId, function () {
                console.log("Work Group dropdown added success fully")
                $('#ddlWorkGroups').val(record.WorkGroup);

            });
            populateSubGroup(SiteId, function () {
                console.log("sub group dropdown added success fully")
                $('#ddlSubGroups').val(record.SubGroup);

            });
            populateRequestedby(SiteId, function () {
                console.log("Requested By dropdown added success fully")
                $('#txtRequestedBy').val(record.RequestedBy);

            });
            populateLocationListDropdown(SiteId, function () {
                $('#ddlLocation').val(record.LocationId);
                console.log("Location selected:", record.LocationId);
                populateBuildingListDropdown(LocationId, function () {
                    $('#ddbuilding').val(record.BuildingID);
                    console.log("Building selected:", record.BuildingID);
                    //populateFloorLevelDropdown( BuildingID, function () {
                    //    $('#DropDownList1').val(record.FloorLevelId);
                    //    console.log("Floor Level selected:", record.FloorLevelId);
                    //});
                    populateFloorLevelDropdown(ClientIds, SiteId, LocationId, BuildingID, function () {
                        $('#DropDownList1').val(record.FloorLevelId);
                        console.log("Floor Level selected:", record.FloorLevelId);
                    });
                    populateAssetListDropdown(BuildingID, function () {
                        $('#ddassets').val(record.AssetId);
                        console.log("Floor asset selected:", record.AssetId);
                    });
                    populateSubGroup(SiteId, WorkGroup,function () {
                        $('#ddlSubGroups').val(record.SubGroup);
                    });

                });
            });
        });

        // Populate Location Dropdown
        //populateLocationListDropdown(SiteId, function () {
        //    $('#ddlLocation').val(LocationId);
        //    console.log("Location selected:", LocationId);
        //});

        // Populate Building Dropdown
        //populateBuildingListDropdown(LocationId, function () {
        //    $('#ddbuilding').val(BuildingID);
        //    console.log("Building selected:", BuildingID);
        //});

        // Populate Floor Level Dropdown
        //populateFloorLevelDropdown(SiteId, LocationId, BuildingID, function () {
        //    $('#DropDownList1').val(FloorLevelId);
        //    console.log("Floor Level selected:", FloorLevelId);
        //});
    } else {
        console.log("No Site selected.");
    }
    //populateLocationListDropdown(SiteId, function () {
    //    console.log("print ");

    //});
    //$('#ddlLocation').val(LocationId);
    //console.log("Location selected:", LocationId);

    // Handle Inventory Logic
    const OptimizeStatus = record.OptimizeStatus || "";
    const InventoryCount = record.InventoryCount || 0;
    const AvailableQuantity = record.AvailableQuantity || 0;
    handleInventoryLogic(MaintainInventory, InventoryCount, OptimizeStatus, AvailableQuantity, record.DateTagged);

    // Display Optimization Status
    if (OptimizeStatus.toUpperCase() === "GREEN") {
        $('#txtoptstatus').css('display', 'none');
        $('#imgopt').css('display', 'block').attr("src", "/images/green.jpg");
    } else {
        $('#txtoptstatus').css('display', 'none');
        $('#imgopt').css('display', 'block').attr("src", "/images/red.jpg");
    }

    // Display Job Status
    const Status = record.Status || "";
    if (["RED", "GREEN", "YELLOW"].includes(Status.toUpperCase())) {
        $('#txtstatus').css('display', 'none');
        $('#imgstaus').css('display', 'block').attr("src", `/images/${Status}.jpg`);
    } else {
        $('#txtstatus').css('display', 'block');
        $('#imgstaus').css('display', 'none');
    }

    // Handle LockedIn
    const LockedIn = record.LockedIn || false;
    $('#ddlLockedIn').val(LockedIn ? 1 : 0);
    if (LockedIn) {
        $('#imgLocked').css('display', 'block');
        $('#txtlckedsignbynone').css('display', 'block');
    } else {
        $('#imgLocked').css('display', 'none');
        $('#txtlckedsignbynone').css('display', 'none');
    }
}


function handleInventoryLogic(MaintainInventory, InventoryCount, OptimizeStatus, AvailableQuantity, DateTagged) {
    if (InventoryCount > 0 && MaintainInventory && OptimizeStatus === "Green" && AvailableQuantity === 0) {
        $('#rdpdatedis').prop('disabled', !DateTagged);
    } else {
        $('#rdpdatedis').prop('disabled', true);
        //FinalizedMessage();
    }
}



function populateDutyRatingDropdown(clientId, callback) {
    // Clear the dropdown
    clearDropdown('#ddlDutyrating');

    const dropdown = document.querySelector('#ddlDutyrating');
    // Add a blank option to handle null or blank values
    const blankOption = document.createElement('option');
    blankOption.value = '';
    blankOption.text = '';
    dropdown.appendChild(blankOption);
    const request = indexedDB.open(dbName, dbVersion);

    request.onsuccess = function (event) {
        const db = event.target.result;
        const transaction = db.transaction('tblDutyRating', 'readonly');
        const objectStore = transaction.objectStore('tblDutyRating');

        // Create an array to collect filtered and sorted results
        const dutyRatings = [];

        objectStore.openCursor().onsuccess = function (event) {
            const cursor = event.target.result;

            if (cursor) {
                const record = cursor.value;
                // Filter by clientId
                if (record.ClientID === clientId) {
                    dutyRatings.push({ id: record.id, DutyRating: record.DutyRating });
                }
                cursor.continue();
            } else {
                // Sort alphabetically by DutyRating
                dutyRatings.sort((a, b) => a.DutyRating.localeCompare(b.DutyRating));

                // Populate the dropdown
                dutyRatings.forEach(record => {
                    const option = document.createElement('option');
                    option.value = record.DutyRating;
                    option.text = record.DutyRating;
                    dropdown.appendChild(option);
                });

                // Call the callback if provided
                if (typeof callback === 'function') {
                    callback();
                }
            }
        };

        transaction.oncomplete = function () {
            db.close();
        };
    };

    request.onerror = function (event) {
        console.error('Error opening database:', event.target.error);
        if (typeof callback === 'function') {
            callback(new Error('Failed to open IndexedDB'));
        }
    };
}
function populateScaffoldDesignDropdown(clientId, callback) {
    // Clear the dropdown
    clearDropdown('#ddlScaffoldDesign');

    const dropdown = document.querySelector('#ddlScaffoldDesign');
    // Add a blank option to handle null or blank values
    const blankOption = document.createElement('option');
    blankOption.value = '';
    blankOption.text = '';
    dropdown.appendChild(blankOption);
    const request = indexedDB.open(dbName, dbVersion);

    request.onsuccess = function (event) {
        const db = event.target.result;
        const transaction = db.transaction('tblDesignList', 'readonly');
        const objectStore = transaction.objectStore('tblDesignList');

        // Create an array to collect filtered and sorted results
        const DesignType = [];

        objectStore.openCursor().onsuccess = function (event) {
            const cursor = event.target.result;

            if (cursor) {
                const record = cursor.value;
                // Filter by clientId
                if (record.ClientID === clientId) {
                    DesignType.push({ id: record.id, DesignType: record.DesignType });
                }
                cursor.continue();
            } else {
                // Sort alphabetically by DutyRating
                DesignType.sort((a, b) => a.DesignType.localeCompare(b.DesignType));

                // Populate the dropdown
                DesignType.forEach(record => {
                    const option = document.createElement('option');
                    option.value = record.DesignType;
                    option.text = record.DesignType;
                    dropdown.appendChild(option);
                });

                // Call the callback if provided
                if (typeof callback === 'function') {
                    callback();
                }
            }
        };

        transaction.oncomplete = function () {
            db.close();
        };
    };

    request.onerror = function (event) {
        console.error('Error opening database:', event.target.error);
        if (typeof callback === 'function') {
            callback(new Error('Failed to open IndexedDB'));
        }
    };
}
async function populateFloorLevelDropdown(id, SiteID, LocationID, BuildingID, callback) {
    try {
        clearDropdown('#DropDownList1');
        const db = await openIndexedDB();
        const siteLocationStore = db.transaction('tblSiteList', 'readonly').objectStore('tblSiteList');
        const siteLocationData = await getDataByKey(siteLocationStore, SiteID);

        let queryResult = [];

        if (siteLocationData) {
            if (siteLocationData.GPS === "True") {
                // Check Floors For Building
                queryResult = await getFloorLevelData(db, id, SiteID, LocationID, BuildingID, 'GPS');

                if (queryResult.length === 0) {
                    // Check Floors For Location
                    queryResult = await getFloorLevelData(db, id, SiteID, LocationID, 0, 'GPS');
                }

                if (queryResult.length === 0) {
                    // Check Floors For Sites only  
                    queryResult = await getFloorLevelData(db, id, SiteID, 0, 0, 'GPS');
                }
            }
            else {
                queryResult = await getFloorLevelData(db, id, SiteID, LocationID, BuildingID, 'Floor Level');
            }
        }

        // Sort results by FloorLevel
        queryResult.sort((a, b) => a.FloorLevel - b.FloorLevel);
        // Bind data to #DropDownList1
        const dropdown = document.getElementById('DropDownList1');
        //Add a blank option to handle null or blank values
        const blankOption = document.createElement('option');
        blankOption.value = '';
        blankOption.text = '';
        dropdown.appendChild(blankOption);

        queryResult.forEach((item) => {
            const option = document.createElement('option');
            option.value = item.FloorLevelId;
            option.textContent = item.FloorLevel;
            dropdown.appendChild(option);
        });

        // Execute callback with the result
        if (typeof callback === 'function') {
            callback(queryResult);
        }

    } catch (error) {
        console.error("Error fetching floor level data:", error);
        if (typeof callback === 'function') {
            callback([]); // Return empty array in case of error
        }
    }
}

async function openIndexedDB() {
    return new Promise((resolve, reject) => {
        const request = indexedDB.open(dbName, dbVersion);

        request.onsuccess = (event) => {
            resolve(event.target.result);
        };

        request.onerror = (event) => {
            reject(event.target.error);
        };
    });
}

async function getDataByKey(store, key) {
    return new Promise((resolve, reject) => {
        const request = store.get(key);

        request.onsuccess = (event) => {
            resolve(event.target.result);
        };

        request.onerror = (event) => {
            reject(event.target.error);
        };
    });
}

async function getFloorLevelData(db, clientId, siteId, locationId, buildingId, category) {
    return new Promise((resolve, reject) => {
        const store = db.transaction('tblFloorLevel', 'readonly').objectStore('tblFloorLevel');
        const index = store.index('Category'); // Assuming you have an index on Category
        const result = [];

        index.openCursor(category).onsuccess = (event) => {
            const cursor = event.target.result;
            if (cursor) {
                const record = cursor.value;
                if (
                    record.ClientID === clientId &&
                    record.SiteLocId === siteId &&
                    record.LocationID === locationId &&
                    record.BuildingID === buildingId &&
                    record.FloorLevelStatus === 'Active'
                ) {
                    result.push(record);
                }
                cursor.continue();
            } else {
                resolve(result);
            }
        };

        index.openCursor(category).onerror = (event) => {
            reject(event.target.error);
        };
    });
}


function populateMaterialTypeDropdown(clientId, callback) {
    clearDropdown('#ddlMaterialType');
    const dropdown = document.querySelector('#ddlMaterialType');

    const blankOption = document.createElement('option');
    blankOption.value = '';
    blankOption.text = '';
    dropdown.appendChild(blankOption);

    const request = indexedDB.open(dbName, dbVersion);

    request.onsuccess = function (event) {
        const db = event.target.result;
        const transaction = db.transaction('tblMaterialType', 'readonly');
        const objectStore = transaction.objectStore('tblMaterialType');

        // Collect filtered and sorted results
        const materialTypes = [];

        objectStore.openCursor().onsuccess = function (event) {
            const cursor = event.target.result;

            if (cursor) {
                const record = cursor.value;
                // Filter by clientId
                if (record.ClientID === clientId) {
                    materialTypes.push({ id: record.id, MaterialType: record.MaterialType });
                }
                cursor.continue();
            } else {
                // Sort alphabetically by MaterialType
                materialTypes.sort((a, b) => a.MaterialType.localeCompare(b.MaterialType));

                // Populate the dropdown
                materialTypes.forEach(record => {
                    const option = document.createElement('option');
                    option.value = record.MaterialType;
                    option.text = record.MaterialType;
                    dropdown.appendChild(option);
                });

                // Call the callback if provided
                if (typeof callback === 'function') {
                    callback();
                }
            }
        };

        transaction.oncomplete = function () {
            db.close();
        };
    };

    request.onerror = function (event) {
        console.error('Error opening database:', event.target.error);
        if (typeof callback === 'function') {
            callback(new Error('Failed to open IndexedDB'));
        }
    };
}


function populateSiteListDropdown(callback) {
    // Clear the dropdown before appending new options
    clearDropdown('#ddlsites');
    const dropdown = document.querySelector('#ddlsites');

    const request = indexedDB.open(dbName, dbVersion);

    request.onsuccess = function (event) {
        const db = event.target.result;
        const transaction = db.transaction('tblSiteList', 'readonly');
        const objectStore = transaction.objectStore('tblSiteList');

        const records = [];

        // Open cursor to read all records from IndexedDB
        objectStore.openCursor().onsuccess = function (event) {
            const cursor = event.target.result;
            if (cursor) {
                const record = cursor.value;
                records.push(record); // Collect records
                cursor.continue(); // Continue iterating
            } else {
                // After all records are collected, sort them by SiteLocation
                records.sort(function (a, b) {
                    const nameA = a.SiteLocation.toLowerCase();
                    const nameB = b.SiteLocation.toLowerCase();
                    return nameA.localeCompare(nameB); // Sort alphabetically
                });

                // Append sorted records to the dropdown
                records.forEach(function (record) {
                    const option = document.createElement('option');
                    option.value = record.SiteLocID;
                    option.text = record.SiteLocation;
                    dropdown.appendChild(option);
                });

                // Execute the callback after all records are processed and appended
                if (typeof callback === 'function') {
                    callback();
                }
            }
        };

        // Transaction error handling
        transaction.onerror = function (event) {
            console.error('Transaction failed', event.target.error);
            if (typeof callback === 'function') {
                callback(event.target.error); // Pass the error to the callback
            }
        };
    };

    request.onerror = function (event) {
        console.error('IndexedDB request failed', event.target.error);
        if (typeof callback === 'function') {
            callback(event.target.error); // Pass the error to the callback
        }
    };
}
function populateCheckListDataDropdown() {
    clearDropdown('#ddlCheckListData');
    const dropdown = document.querySelector('#ddlCheckListData');
    const request = indexedDB.open(dbName, dbVersion);
    request.onsuccess = function (event) {
        const db = event.target.result;
        const transaction = db.transaction('tblCheckList', 'readonly');
        const objectStore = transaction.objectStore('tblCheckList');
        objectStore.openCursor().onsuccess = function (event) {
            const cursor = event.target.result;
            if (cursor) {
                const record = cursor.value;
                const option = document.createElement('option');
                option.value = record.checklistID;
                option.text = record.checklistdescription;
                dropdown.appendChild(option);
                cursor.continue();
            }
        };
    };
}

function populateLocationListDropdown(SiteId, callback) {
    var areaddl = document.getElementById("ddlLocation");
    // Add a blank option to handle null or blank values
    const blankOption = document.createElement('option');
    blankOption.value = '';
    blankOption.text = '';
    areaddl.appendChild(blankOption);
    clearDropdown('#ddlLocation');

    if (!areaddl) {
        console.error("Dropdown element with id 'ddlLocation' not found.");
        if (callback) callback(); // Call callback even if the element is not found
        return;
    }

    // Clear the dropdown

    if (isNaN(SiteId) || SiteId === 0) {
        console.log("No SiteId provided. Exiting.");
        if (callback) callback(); // Call the callback after completing
        return;
    }
    SiteId = String(SiteId);

    // Open the database
    var request = indexedDB.open(dbName, dbVersion);

    request.onsuccess = function (event) {
        var db = event.target.result;

        // Transaction and object store
        var transaction = db.transaction(["tblLocationList"], "readonly");
        var objectStore = transaction.objectStore("tblLocationList");
        var index = objectStore.index("SiteLocId");
        var dbRequest = index.getAll(SiteId); // Fetch areas by SiteId

        dbRequest.onsuccess = function (event) {
            var data = event.target.result;
            data.sort(function (a, b) {
                const nameA = a.area.toLowerCase();
                const nameB = b.area.toLowerCase();
                return nameA.localeCompare(nameB); // Case-insensitive alphabetical sorting
            });
            data.forEach(function (item) {
                var option = document.createElement("option");
                option.value = item.areaid;
                option.text = item.area;
                areaddl.appendChild(option);
            });

            console.log("Dropdown populated with locations filtered by SiteId.");

            if (callback) callback(); // Call the callback after completion
        };

        dbRequest.onerror = function () {
            console.error("Failed to fetch areas by SiteId.");
            if (callback) callback(); // Call the callback even on error
        };
    };

    request.onerror = function (event) {
        console.error("Error opening database:", event.target.errorCode);
        if (callback) callback(); // Call the callback even on error
    };
}

function populateBuildingListDropdown(LocationId, callback) {
    var areaddl = document.getElementById("ddbuilding");
    clearDropdown('#ddbuilding');
    //Add a blank option to handle null or blank values
    const blankOption = document.createElement('option');
    blankOption.value = '';
    blankOption.text = '';
    areaddl.appendChild(blankOption);
    if (!areaddl) {
        console.error("Dropdown element with id 'ddlLocation' not found.");
        if (callback) callback(); // Call callback even if the element is not found
        return;
    }

    // Clear the dropdown

    if (isNaN(LocationId) || LocationId === 0) {
        console.log("No SiteId provided. Exiting.");
        if (callback) callback(); // Call the callback after completing
        return;
    }
    LocationId = String(LocationId);

    // Open the database
    var request = indexedDB.open(dbName, dbVersion);

    request.onsuccess = function (event) {
        var db = event.target.result;

        // Transaction and object store
        var transaction = db.transaction(["tblBuildingList"], "readonly");
        var objectStore = transaction.objectStore("tblBuildingList");
        var index = objectStore.index("areaid");
        var dbRequest = index.getAll(LocationId); // Fetch areas by SiteId

        dbRequest.onsuccess = function (event) {
            var data = event.target.result;
            data.sort(function (a, b) {
                //const nameA = a.BuildingNo.toLowerCase();
                //const nameB = b.BuildingNo.toLowerCase();
                const nameA = (a.BuildingNo === null ? "null" : a.BuildingNo.toLowerCase());
                const nameB = (b.BuildingNo === null ? "null" : b.BuildingNo.toLowerCase());

                return nameA.localeCompare(nameB); // Case-insensitive alphabetical sorting
            });
            data.forEach(function (item) {
                var option = document.createElement("option");
                option.value = item.BuildingID;
                option.text = item.BuildingNo;
                areaddl.appendChild(option);
            });

            console.log("Dropdown populated with BuildingID filtered by LocationId.");

            if (callback) callback(); // Call the callback after completion
        };

        dbRequest.onerror = function () {
            console.error("Failed to fetch areas by SiteId.");
            if (callback) callback(); // Call the callback even on error
        };
    };

    request.onerror = function (event) {
        console.error("Error opening database:", event.target.errorCode);
        if (callback) callback(); // Call the callback even on error
    };
}

function populateAssetListDropdown(BuildingId, callback) {
    clearDropdown('#ddassets'); // Clear existing options
    const dropdown = document.querySelector('#ddassets');
    // Add a blank option to handle null or blank values
    const blankOption = document.createElement('option');
    blankOption.value = '';
    blankOption.text = '';
    dropdown.appendChild(blankOption);
    const request = indexedDB.open(dbName, dbVersion);

    request.onsuccess = function (event) {
        const db = event.target.result;
        const transaction = db.transaction('tblAssetList', 'readonly');
        const objectStore = transaction.objectStore('tblAssetList');
        const matchingRecords = []; // Array to store matching records

        objectStore.openCursor().onsuccess = function (event) {
            const cursor = event.target.result;
            if (cursor) {
                const record = cursor.value;

                // Filter logic for SiteId, LocationId, and BuildingId
                if (

                    record.BuildingID === BuildingId
                ) {
                    matchingRecords.push(record); // Add record to array
                }
                cursor.continue();
            } else {
                // No more records, sort and populate the dropdown
                matchingRecords.sort((a, b) => {
                    if (a.Assetno < b.Assetno) return -1;
                    if (a.Assetno > b.Assetno) return 1;
                    return 0;
                });

                // Populate the dropdown with sorted records
                //matchingRecords.forEach(record => {
                //    const option = document.createElement('option');
                //    option.value = record.Assetid;
                //    option.text = record.Assetno +'-'+ record.assetsdesc;
                //    dropdown.appendChild(option);
                //});
                matchingRecords.forEach(record => {
                    const option = document.createElement('option');
                    option.value = record.Assetid;
                    option.text = record.assetsdesc ? `${record.Assetno} ~ ${record.assetsdesc}` : record.Assetno;
                    dropdown.appendChild(option);
                });


                console.log("Dropdown populated with sorted Assetno.");
            }
        };

        transaction.oncomplete = function () {
            console.log("Transaction completed. Assetno loaded and sorted.");
            if (callback) callback(); // Call the callback after completion

        };

        transaction.onerror = function (event) {
            console.error("Error loadingAssetno:", event.target.error);
            if (callback) callback(); // Call the callback after completion

        };
    };

    request.onerror = function (event) {
        console.error("Failed to open IndexedDB:", event.target.errorCode);
    };
}

function populateSupervisorDropdown(clientId, callback) {
    // Clear the dropdown before fetching data
    clearDropdown('#ddlsupervisor');
    const dropdown = document.querySelector('#ddlsupervisor');

    // Add a blank option to handle null or blank values
    const blankOption = document.createElement('option');
    blankOption.value = '';
    blankOption.text = '';
    dropdown.appendChild(blankOption);

    // Open IndexedDB and fetch data
    const request = indexedDB.open(dbName, dbVersion);

    request.onsuccess = function (event) {
        const db = event.target.result;
        const transaction = db.transaction('tblSupervisor', 'readonly');
        const objectStore = transaction.objectStore('tblSupervisor');

        // Use a cursor to iterate through all records and filter manually
        objectStore.openCursor().onsuccess = function (event) {
            const cursor = event.target.result;
            if (cursor) {
                const record = cursor.value;
                if (record.ClientID === parseInt(clientId, 10)) { // Apply filter manually
                    const option = document.createElement('option');
                    option.value = record.UserID;
                    option.text = record.Supervisor;
                    dropdown.appendChild(option);
                }
                cursor.continue();
            }
        };

        transaction.oncomplete = function () {
            if (callback && typeof callback === 'function') {
                callback(); // Execute the callback after the dropdown is populated
            }
        };
    };

    request.onerror = function (event) {
        console.error('Error opening IndexedDB:', event);
        if (callback && typeof callback === 'function') {
            callback(new Error('Failed to open IndexedDB'));
        }
    };
}

function populateScaffoldOwnerDropdown() {
    clearDropdown('#ddlScaffoldOwner');
    const dropdown = document.querySelector('#ddlScaffoldOwner');
    const request = indexedDB.open(dbName, dbVersion);
    request.onsuccess = function (event) {
        const db = event.target.result;
        const transaction = db.transaction('tblScaffoldOwner', 'readonly');
        const objectStore = transaction.objectStore('tblScaffoldOwner');
        objectStore.openCursor().onsuccess = function (event) {
            const cursor = event.target.result;
            if (cursor) {
                const record = cursor.value;
                const option = document.createElement('option');
                option.value = record.id;
                option.text = record.OwnerName;
                dropdown.appendChild(option);
                cursor.continue();
            }
        };
    };
}
function populateSubOwnerRepDropdown() {
    clearDropdown('#ddlSubOwnerRep');
    const dropdown = document.querySelector('#ddlSubOwnerRep');
    const request = indexedDB.open(dbName, dbVersion);
    request.onsuccess = function (event) {
        const db = event.target.result;
        const transaction = db.transaction('tblSubOwnerRep', 'readonly');
        const objectStore = transaction.objectStore('tblSubOwnerRep');
        objectStore.openCursor().onsuccess = function (event) {
            const cursor = event.target.result;
            if (cursor) {
                const record = cursor.value;
                const option = document.createElement('option');
                option.value = record.id;
                option.text = record.RepresentativeName;
                dropdown.appendChild(option);
                cursor.continue();
            }
        };
    };
}

//function populateSubGroup(SiteLocID, callback) {
//    // Clear the dropdown before populating
//    clearDropdown('#ddlSubGroups');
//    const dropdown = document.querySelector('#ddlSubGroups');
//    // Add a blank option to handle null or blank values
//    const blankOption = document.createElement('option');
//    blankOption.value = '';
//    blankOption.text = '';
//    dropdown.appendChild(blankOption);
//    const request = indexedDB.open(dbName, dbVersion);

//    request.onsuccess = function (event) {
//        const db = event.target.result;
//        const transaction = db.transaction('tblSubGroups', 'readonly');
//        const objectStore = transaction.objectStore('tblSubGroups');

//        const subGroupsArray = [];

//        objectStore.openCursor().onsuccess = function (event) {
//            const cursor = event.target.result;
//            if (cursor) {
//                const record = cursor.value;

//                if (parseInt(record.SiteLocID, 10) === SiteLocID) {
//                    subGroupsArray.push(record);
//                }
//                cursor.continue();
//            } else {
//                subGroupsArray.sort((a, b) => a.SubGroupName.localeCompare(b.SubGroupName));
//                subGroupsArray.forEach(record => {
//                    const option = document.createElement('option');
//                    option.value = record.SubGroupId;
//                    option.text = record.SubGroupName;
//                    dropdown.appendChild(option);
//                });

//                if (callback) callback(null, subGroupsArray);
//            }
//        };

//        transaction.onerror = function (errorEvent) {
//            console.error('Transaction failed:', errorEvent.target.error);
//            if (callback) callback(errorEvent.target.error);
//        };
//    };

//    request.onerror = function (event) {
//        console.error('IndexedDB request failed:', event.target.error);
//        if (callback) callback(event.target.error);
//    };
//}

function populateSubGroup(SiteLocID, WorkGroupId = 0, callback) {

    clearDropdown('#ddlSubGroups');

    const dropdown = document.getElementById('ddlSubGroups');
    if (!dropdown) {
        console.error('ddlSubGroups dropdown not found');
        return;
    }
    dropdown.appendChild(new Option('', ''));

    const request = indexedDB.open(dbName, dbVersion);

    request.onerror = function (e) {
        console.error('IndexedDB open failed:', e.target.error);
        if (callback) callback(e.target.error);
    };

    request.onsuccess = function (event) {
        const db = event.target.result;

        if (!db.objectStoreNames.contains('tblSubGroups')) {
            console.error('tblSubGroups store not found');
            return;
        }

        const transaction = db.transaction('tblSubGroups', 'readonly');
        const objectStore = transaction.objectStore('tblSubGroups');

        const subGroupsArray = [];

        objectStore.openCursor().onsuccess = function (event) {
            const cursor = event.target.result;

            if (cursor) {
                const record = cursor.value;

                const siteIdMatch =
                    Number(record.SiteLocID) === Number(SiteLocID);

                const workGroupMatch =
                    WorkGroupId === 0 ||
                    Number(record.WorkGroupId) === Number(WorkGroupId);

                if (siteIdMatch && workGroupMatch) {
                    subGroupsArray.push(record);
                }

                cursor.continue();
            }
            else {
                subGroupsArray.sort((a, b) =>
                    (a.SubGroupName || '').localeCompare(b.SubGroupName || '')
                );

                subGroupsArray.forEach(record => {
                    dropdown.appendChild(
                        new Option(record.SubGroupName, record.SubGroupId)
                    );
                });
                if (subGroupsArray.length === 1) {
                    dropdown.value = subGroupsArray[0].SubGroupId;
                }

                if (callback) callback(null, subGroupsArray);
            }
        };

        transaction.onerror = function (e) {
            console.error('Transaction failed:', e.target.error);
            if (callback) callback(e.target.error);
        };
    };
}
document.addEventListener('DOMContentLoaded', function () {

    const ddlWorkGroups = document.getElementById('ddlWorkGroups');

    if (!ddlWorkGroups) {
        console.error('ddlWorkGroups not found');
        return;
    }

    ddlWorkGroups.addEventListener('change', function () {

        const WorkGroupId = parseInt(this.value, 10) || 0;
        const siteLocId = SiteLocID;

        populateSubGroup(siteLocId, WorkGroupId);
    });

});
function populateWorkGroup(SiteLocID, callback) {
    clearDropdown('#ddlWorkGroups');
    const dropdown = document.querySelector('#ddlWorkGroups');
    // Add a blank option to handle null or blank values
    const blankOption = document.createElement('option');
    blankOption.value = '';
    blankOption.text = '';
    dropdown.appendChild(blankOption);
    const request = indexedDB.open(dbName, dbVersion);

    request.onsuccess = function (event) {
        const db = event.target.result;
        const transaction = db.transaction('tblWorkGroups', 'readonly');
        const objectStore = transaction.objectStore('tblWorkGroups');

        const WorkGroupsArray = [];

        objectStore.openCursor().onsuccess = function (event) {
            const cursor = event.target.result;
            if (cursor) {
                const record = cursor.value;

                // Filter by SiteLocID
                if (parseInt(record.SiteLocID, 10) === SiteLocID) {
                    WorkGroupsArray.push(record);
                }
                cursor.continue();
            } else {
                // Sort the WorkGroupsArray by WorkGroups
                WorkGroupsArray.sort((a, b) => {
                    return a.WorkGroups.localeCompare(b.WorkGroups);
                });

                // Populate the dropdown
                WorkGroupsArray.forEach(record => {
                    const option = document.createElement('option');
                    option.value = record.WorkGroupID;
                    option.text = record.WorkGroups;
                    dropdown.appendChild(option);
                });

                // Call the callback after the dropdown is populated
                if (callback) callback(null); // Pass `null` to indicate no error
            }
        };

        objectStore.openCursor().onerror = function (errorEvent) {
            console.error('Cursor operation failed', errorEvent.target.error);
            if (callback) callback(errorEvent.target.error);
        };
    };

    request.onerror = function (event) {
        console.error('IndexedDB request failed', event.target.error);
        if (callback) callback(event.target.error); // Ensure `event.target.error` is used here
    };
}
function populateRequestedby(SiteLocID, callback) {
    clearDropdown('#txtRequestedBy');
    const dropdown = document.querySelector('#txtRequestedBy');
    // Add a blank option to handle null or blank values
    const blankOption = document.createElement('option');
    blankOption.value = '';
    blankOption.text = '';
    dropdown.appendChild(blankOption);
    const request = indexedDB.open(dbName, dbVersion);

    request.onsuccess = function (event) {
        const db = event.target.result;
        const transaction = db.transaction('tblRequestedBy', 'readonly');
        const objectStore = transaction.objectStore('tblRequestedBy');

        const RequestedByArray = [];

        objectStore.openCursor().onsuccess = function (event) {
            const cursor = event.target.result;
            if (cursor) {
                const record = cursor.value;

                if (parseInt(record.SiteLocID, 10) === SiteLocID) {
                    RequestedByArray.push(record);
                }
                cursor.continue();
            } else {
                RequestedByArray.sort((a, b) => a.RequestBy.localeCompare(b.RequestBy));
                RequestedByArray.forEach(record => {
                    const option = document.createElement('option');
                    option.value = record.RequestById;
                    option.text = record.RequestBy;
                    dropdown.appendChild(option);
                });

                if (callback) callback(null, RequestedByArray);
            }
        };

        transaction.onerror = function (errorEvent) {
            console.error('Transaction failed:', errorEvent.target.error);
            if (callback) callback(errorEvent.target.error);
        };
    };

    request.onerror = function (event) {
        console.error('IndexedDB request failed:', event.target.error);
        if (callback) callback(event.target.error);
    };
}

function clearDropdown(selector) {
    const dropdown = document.querySelector(selector);
    while (dropdown.firstChild) {
        dropdown.removeChild(dropdown.firstChild);
    }
}


function ddlSitesChanged() {
    SiteId = parseInt($('#ddlsites').val(), 10);

    populateLocationListDropdown(SiteId, function () {
        console.log("Location dropdown loaded again");
    });

    getOwnerNameFromIndexedDB(SiteId, function (ownerName) {
        if (ownerName) {
            $('#txtOwnername').val(ownerName);


            console.log("Owner name updated: " + ownerName);
        } else {
            console.log("No owner name found for SiteId: " + SiteId);
            $('#txtOwnername').val("");
        }
    });
}
function getOwnerNameFromIndexedDB(siteId, callback) {
    const dbRequest = indexedDB.open(dbName, dbVersion);

    dbRequest.onsuccess = function (event) {
        const db = event.target.result;
        const transaction = db.transaction("tblSiteList", "readonly");
        const store = transaction.objectStore("tblSiteList");

        const query = store.get(siteId);

        query.onsuccess = function (event) {
            const record = event.target.result;
            if (record && record.OwnerName) {
                callback(record.OwnerName);
            } else {
                callback(null);
            }
        };

        query.onerror = function () {
            console.error("Error fetching record from IndexedDB");
            callback(null);
        };
    };

    dbRequest.onerror = function () {
        console.error("Error opening IndexedDB");
        callback(null);
    };
}

function ddlLocationChanged() {

    var LocationId = parseInt($('#ddlLocation').val(), 10)

    populateBuildingListDropdown(LocationId, function () {
        console.log("Building  drop down loaded again ")
    })
}
function ddlBuildingChanged() {
    var SiteLocId = parseInt($('#ddlsites').val(), 10)
    var LocationId = parseInt($('#ddlLocation').val(), 10)
    var ids = getParameterByName('id');
    let CId = OffDecrypt(ids)
    var BuildingId = parseInt($('#ddbuilding').val(), 10)
    populateFloorLevelDropdown(parseInt(CId, 10), SiteLocId, LocationId, BuildingId, function () {
        console.log("Floor level  drop down loaded again ")
    });
    populateAssetListDropdown(BuildingId, function () {
        console.log("Asset drop down loaded again ")
    });

}

function btnAddInspection() {
    //document.getElementById("Adddocs").style.display = "block";
    /* BindallDropdown();*/
    populateInspectionList();
    populateAddInspectionInspectionBy();
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
        var DecryptData = UserRole;
        var UserRole = DecryptData[0];
        if (UserRole != "10") {
            $('#btnAddInspection').css('display', 'none');
        }
        /*   $('#rdpInspectiondate').val(getDateTime());*/
        $('#Adddocs').css('display', 'block');
    }

}

function btnDeplank_Click() {
    //document.getElementById("divDeplank").style.display = "block";
    if ($('#lblNewstatus').val() == "Completed") {
        $('#divDeplank').css('display', 'none');
        var Response = { Result: "Failed", Message: "Since Job has been Completed so no further action allowed." };
        CustomAlert(Response);
    }
    else {
        populateDeplankInspectionBy();
        if ($('#hdnLoginUserId').val() != "") {
            $('#ddlInspecdeplank').val($('#hdnLoginUserId').val());
        }
        $('#divDeplank').css('display', 'block');
        //divDeplank.Style.Add("display", "block");
        // dateInspection.SelectedDate =DateTime.Now;
        //dateInspection.SelectedDate = Convert.ToDateTime(hdnCurrentDate.Value);
        $('#txtcomm').val('');
        $('#dateInspection').val(getDateTime());
    }
    //ShowJobInfoDetails();

}


//function populateAddInspectionInspectionBy() {
//    clearDropdown('#ddlddlInspectedByadd'); // Clear existing dropdown options

//    const dropdown = document.querySelector('#ddlddlInspectedByadd');

//    const defaultOption = document.createElement('option');
//    defaultOption.value = '';
//    defaultOption.text = 'Select---';
//    defaultOption.disabled = true;
//    defaultOption.selected = true;
//    dropdown.appendChild(defaultOption);

//    const request = indexedDB.open(dbName, dbVersion);

//    request.onsuccess = function (event) {
//        const db = event.target.result;
//        const transaction = db.transaction('tblUserList', 'readonly');
//        const objectStore = transaction.objectStore('tblUserList');

//        const records = [];

//        objectStore.openCursor().onsuccess = function (event) {
//            const cursor = event.target.result;
//            if (cursor) {
//                records.push(cursor.value);
//                cursor.continue();
//            } else {
//                records.sort((a, b) => a.FullName.localeCompare(b.FullName));

//                records.forEach(record => {
//                    const option = document.createElement('option');
//                    option.value = record.UserId;
//                    option.text = record.FullName;
//                    dropdown.appendChild(option);
//                });
//            }
//        };
//    };

//    request.onerror = function (event) {
//        console.error('Error opening database:', event.target.errorCode);
//        CustomAlert({ Result: "Failed", Message: "Failed to load inspection users." });
//    };
//}

function populateAddInspectionInspectionBy() {
    clearDropdown('#ddlddlInspectedByadd'); // Clear existing dropdown options

    const dropdown = document.querySelector('#ddlddlInspectedByadd');

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

//function populateDeplankInspectionBy() {
//    clearDropdown('#ddlInspecdeplank');
//    const dropdown = document.querySelector('#ddlInspecdeplank');

//    const defaultOption = document.createElement('option');
//    defaultOption.value = '';
//    defaultOption.text = 'Select---';
//    defaultOption.disabled = true;
//    defaultOption.selected = true;
//    dropdown.appendChild(defaultOption);
//    var Userid = parseInt(OffDecrypt(getParameterByName('UID')), 10);

//    const request = indexedDB.open(dbName, dbVersion);
//    request.onsuccess = function (event) {
//        const db = event.target.result;
//        const transaction = db.transaction('tblUserList', 'readonly');
//        const objectStore = transaction.objectStore('tblUserList');

//        const records = [];

//        objectStore.openCursor().onsuccess = function (event) {
//            const cursor = event.target.result;
//            if (cursor) {
//                records.push(cursor.value);
//                cursor.continue();
//            } else {
//                records.sort((a, b) => a.FullName.localeCompare(b.FullName));

//                records.forEach(record => {
//                    const option = document.createElement('option');
//                    option.value = record.UserID;
//                    option.text = record.FullName;
//                    dropdown.appendChild(option);

//                });
//            }
//        };
//    };
//}
function populateDeplankInspectionBy() {
    clearDropdown('#ddlInspecdeplank'); // Clear existing dropdown options

    const dropdown = document.querySelector('#ddlInspecdeplank');

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

function populateInspectionList() {
    clearDropdown('#ddlChecklistInsp'); // Clear existing options
    const dropdown = document.querySelector('#ddlChecklistInsp');

    const request = indexedDB.open(dbName, dbVersion);
    request.onsuccess = function (event) {
        const db = event.target.result;
        const transaction = db.transaction('tblCheckList', 'readonly');
        const objectStore = transaction.objectStore('tblCheckList');

        const records = [];

        objectStore.openCursor().onsuccess = function (event) {
            const cursor = event.target.result;
            if (cursor) {
                records.push(cursor.value);
                cursor.continue();
            } else {
                records.sort((a, b) => a.checklistID.localeCompare(b.checklistID)); // Sorting by ID

                records.forEach(record => {
                    const option = document.createElement('option');
                    option.value = record.checklistID;
                    option.text = record.checklistdescription;
                    dropdown.appendChild(option);
                });
            }
        };
    };

    request.onerror = function (event) {
        console.error('Error opening database:', event.target.errorCode);
        CustomAlert({ Result: "Failed", Message: "Failed to load inspection checklists." });
    };
}

function GetInspectionListData(RID) {
    var dbPromise = indexedDB.open(dbName, dbVersion);

    dbPromise.onsuccess = function (event) {
        db = event.target.result;

        var transaction = db.transaction(["tblInspectionDetails"], "readonly");
        var objectStore = transaction.objectStore("tblInspectionDetails");
        var result = [];

        // Open cursor to iterate over records
        var cursorRequest = objectStore.openCursor();
        cursorRequest.onsuccess = function (event) {
            var cursor = event.target.result;
            if (cursor) {
                var record = cursor.value;
                console.log("Fetched Record:", record); // Log all records

                if (String(record.RegisterID) === String(RID)) {
                    console.log("Matching record:", record); // Log matching records
                    result.push(record);
                }
                cursor.continue();
            } else {
                console.log("Cursor iteration complete. Total matches:", result.length);
                // Pass result to the display function

                displayInspectionListDetails(result);
            }
        };

        cursorRequest.onerror = function (event) {
            console.error("Cursor request failed", event);
            // Optional: Display an error message in the table
            displayInspectionListDetails([]); // Ensure the table shows no data
        };
    };

    dbPromise.onerror = function (event) {
        console.error("Database connection failed", event);
        // Optional: Display an error message in the table
        displayInspectionListDetails([]); // Ensure the table shows no data
    };
}


function displayInspectionListDetails(result) {
    $('#LoadingPanel1').show();
    var id = getParameterByName('id');
    var RID = getParameterByName('RID');
    //var hdnTimeOffSet = $('#hdnTimeOffSet').val();
    //var HistInspList = $('#HistInspList').prop('checked');

    var str = "";
    var GetData = result;
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
            var InspComments = (InspComments1.length > 40) ? InspComments1.substring(0, 40) + '.....' : InspComments1;
            var Acknowledgment = GetData[i].Acknowledgment == null ? "" : GetData[i].Acknowledgment;
            var InspStatus = GetData[i].InspStatus == "Scheduled" ? "" : GetData[i].InspStatus;
            var AckDate = GetData[i].AckDate == null ? "" : GetData[i].AckDate;
            var AckDateTime = GetData[i].AckDateTime == null ? "" : GetData[i].AckDateTime;
            var LastEmailSentOn = GetData[i].LastEmailSentOn == null ? "" : GetData[i].LastEmailSentOn;
            var ScheduledOrAckDate = GetData[i].InspectionDate == null ? "" : GetData[i].InspectionDate;
            var ScheduledOrAckTime = GetData[i].ScheduledOrAckTime == null ? "" : GetData[i].ScheduledOrAckTime;
            var OffSystemInspection = GetData[i].OffSystemInspection == null ? "" : GetData[i].OffSystemInspection;

            var OffSystInspection = "";
            if (OffSystemInspection == true) {
                OffSystInspection = "Yes";
            }

            //if (ScheduledOrAckTime == 0) {
            //    //InspectionDate = DateFormat(InspectionDate);//Commented on 03-July-2021
            //    InspectionDate = SetInspectionDateFormat(ScheduledOrAckDate);
            //} else {
            //    //InspectionDate = DateTimeFormat(InspectionDate);
            //    /* InspectionDate = SetDateTimeFormat(ScheduledOrAckDate);*/
            //}

            if (ScheduledOrAckDate) {
                if (isJsonDate(ScheduledOrAckDate)) {
                    // Convert JSON date to desired format
                    InspectionDate = formatJsonDate(ScheduledOrAckDate);
                } else if (ScheduledOrAckTime === 0) {
                    //InspectionDate = DateFormat(InspectionDate);//Commented on 03-July-2021
                    InspectionDate = SetInspectionDateFormat(ScheduledOrAckDate);
                } else {
                    //InspectionDate = DateTimeFormat(InspectionDate);
                    InspectionDate = ScheduledOrAckDate;
                }
            }

            str += "<tr>"
            str += "<td class='setWidth0' style='display:none'></td>"
            /*   str += "<td class='setWidth1' title='Click here for Delete Inspection' style='cursor:pointer;'><i class='fa fa-trash setWidth1' onclick=ShowDeletePopup('" + InspectionID + "')></i></td>"*/
            str += "<td class='setWidth1'>" + InspStatus + "</td>"
            str += "<td class='setWidth2'>" + OnlineInsp + "</td>"
            str += "<td class='setWidth3'>" + OffSystInspection + "</td>"
            str += "<td class='setWidth4'>" + InspectionDate + "</td>"
            str += "<td class='setWidth5'>" + InspectedBy + "</td>"
            str += "<td class='setWidth6' title='" + InspComments1 + "'>" + InspComments + "</td>"

            if (InspStatus == "De-Planked" || OffSystemInspection == true) {
                str += "<td class='setWidth7'></td>"
                /*str += "<td class='setWidth8'></td>"*/
                //str += "<td class='setWidth9'></td>"

            } else {
                str += "<td class='setWidth7' title='click here to open Inspection Form.' style='cursor:pointer;'><a type='button' id='tbnOpen' class='btnOpen' ChkInspectionID_='" + InspectionID + "' onclick=OpenOfflineInspectionForm('" + InspectionID + "') ><img src='/images/fonts/fa_link.png' class='img-responsive' style='height: 15px;width: 15px;margin-top: 2px;float: left;' />&nbsp;Open</a></td>";


            }

        }

        $('#tbodyInspectionList').append(str);
    } else {
        // No data available message
        str = "<tr><td colspan='7' style='text-align:center;'>No data available</td></tr>";
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
}


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

function SetDateTimeFormat(date) {
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



function SetInspectionDateFormat(date) {
    if (date != null && date != undefined && date != "") {
        date = date.toString();
        var newDate = new Date(parseInt(date.substr(6)));
        var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
        var day = ("0" + newDate.getDate()).slice(-2);
        var month = monthNames[newDate.getMonth()];
        var year = newDate.getFullYear();
        var DateTo = day + '-' + month + '-' + year;
        return DateTo.toString();
    } else {
        return "";
    }
}



function OpenOfflineInspectionForm(InspectionID) {

    const dbRequest = indexedDB.open(dbName, dbVersion);

    dbRequest.onsuccess = function (event) {
        const db = event.target.result;
        const transaction = db.transaction(["tblInspectionListDetails"], "readonly");
        const objectStore = transaction.objectStore("tblInspectionListDetails");


        const request = objectStore.index("InspectionID").get(parseInt(InspectionID), 10);


        request.onsuccess = function () {
            const result = request.result;
            if (result) {
                const ChecklistId = result.CheckListID;
                const encryptedInspectionID = InspectionID;
                const UID = getParameterByName("UID");
                const UserRole = getParameterByName("UserRole");
                var id = getParameterByName('id');

                const RID = getParameterByName("RID");
                const SiteLocId = getParameterByName('SiteLocId');
                /* var GetDate = response.Message.split('_');*/
                /* }*/
                /* window.location.href = `/OfflineInspectionForm.html?UID=${UID}&UserRole=${UserRole}&id=${id}&SiteLocId=${SiteLocId}&RID=${RID}&CheckListID=${ChecklistId}&InspID=${encryptedInspectionID}`;*/


                //window.location.href = `/OfflineInspectionForm.html?UID=${encodeURIComponent(UID)}&UserRole=${encodeURIComponent(UserRole)}&id=${encodeURIComponent(id)}&SiteLocId=${encodeURIComponent(SiteLocId)}&RID=${encodeURIComponent(RID)}&CheckListID=${encodeURIComponent(ChecklistId)}&InspID=${encodeURIComponent(encryptedInspectionID)}`;
                window.location.href = `OfflineInspectionForm.html?UID=${encodeURIComponent(UID)}&UserRole=${encodeURIComponent(UserRole)}&OfflineUserRole=${encodeURIComponent(OfflineUserRole)}&id=${encodeURIComponent(id)}&SiteLocId=${encodeURIComponent(SiteLocId)}&RID=${encodeURIComponent(RID)}&CheckListID=${encodeURIComponent(ChecklistId)}&InspID=${encodeURIComponent(encryptedInspectionID)}`;


            } else {
                CustomAlert({ Result: "Failed", Message: "No Record Found." });
                $("#LoadingPanel1").hide();
            }
        };

        request.onerror = function () {
            CustomAlert({ Result: "Failed", Message: "Error fetching data." });
            $("#LoadingPanel1").hide();
        };
    };

    dbRequest.onerror = function () {
        CustomAlert({ Result: "Failed", Message: "Error opening database." });
        $("#LoadingPanel1").hide();
    };
}



function Offlinebtnsave_Click() {
    $('#LoadingPanel1').show();

    var id = getParameterByName('id');
    let RID = getParameterByName('RID');
    var SiteLocId = getParameterByName('SiteLocId');
    console.log("Encrypted RID from URL:", RID);
    const RegisterID = OffDecrypt(RID);
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var InspectedById = $('#ddlddlInspectedByadd').val();
    var InspectedBy = $('#ddlddlInspectedByadd option:selected').text();
    var dateInspection = $('#dateInspection').val();
    var InspStatus = "Scheduled";
    var ddlChecklistInsp = $('#ddlChecklistInsp').val();
    var ScheduledOrAckDate = $('#rdpInspectiondate').val();
    var OfflineUserRole = getParameterByName('OfflineUserRole');

    // Validation for required fields
    if (!ddlChecklistInsp) {
        CustomAlert({ Result: "Failed", Message: "Please select Scaffold Inspection List." });
        $('#ddlChecklistInsp').addClass('requiredColumn');
        $('#LoadingPanel1').hide();
        return;
    }

    if (!InspectedById) {
        CustomAlert({ Result: "Failed", Message: "Please select Inspected By." });
        $('#ddlddlInspectedByadd').addClass('requiredColumn');
        $('#LoadingPanel1').hide();
        return;
    }

    if (!ScheduledOrAckDate) {
        CustomAlert({ Result: "Failed", Message: "Please select Scheduled Inspection Date." });
        $('#rdpInspectiondate').addClass('requiredColumn');
        $('#LoadingPanel1').hide();
        return;
    }

    // Convert ScheduledOrAckDate to the required format
    const inspectionDate = new Date(ScheduledOrAckDate);
    const formattedDate = formatToUTCWithAMPM(inspectionDate);

    // Open IndexedDB
    const dbRequest = indexedDB.open(dbName, dbVersion);

    dbRequest.onerror = function (event) {
        console.error("Database error: " + event.target.errorCode);
        CustomAlert({ Result: "Failed", Message: "Failed to open database." });
        $('#LoadingPanel1').hide();
    };

    dbRequest.onsuccess = function (event) {
        const db = event.target.result;

        const transaction = db.transaction(["tblInspectionDetails", "tblInspectionListDetails", "tblcheckListContentList", "tblUserList"], "readwrite");
        const inspectionDetailsStore = transaction.objectStore("tblInspectionDetails");
        const inspectionListDetailsStore = transaction.objectStore("tblInspectionListDetails");
        const checkListContentListStore = transaction.objectStore("tblcheckListContentList");
        const userListStore = transaction.objectStore("tblUserList");

        // Fetch LicenceNo based on UID from tblUserList
        const userRequest = userListStore.get(parseInt(InspectedById), 10);

        userRequest.onsuccess = function (event) {
            const user = event.target.result;
            let LicenceNo = user ? user.LicenceNo : null;

            if (!LicenceNo) {
                console.warn("LicenceNo not found for the given UID.");
                CustomAlert({ Result: "Warning", Message: "Licence No not found for the user." });
                $('#LoadingPanel1').hide();
                return;
            }

            console.log("LicenceNo fetched for UID:", LicenceNo);

            // Insert new record into tblInspectionDetails
            const newInspectionRecord = {
                RegisterID: parseInt(RegisterID, 10),
                InspectedById: InspectedById,
                AckDateTime: "1900-01-01",
                InspectedBy: InspectedBy,
                ddlChecklistInsp: ddlChecklistInsp,
                InspectionDate: formattedDate,
                InspStatus: InspStatus,
                LicenceNo: LicenceNo,
                insertedOffline: true
            };

            const addInspectionRequest = inspectionDetailsStore.add(newInspectionRecord);

            addInspectionRequest.onsuccess = function (event) {
                const InspectionID = event.target.result;

                console.log("New record inserted successfully with InspectionID:", InspectionID);

                // Fetch data from tblCheckListContentList based on ddlChecklistInsp
                const checklistRequest = checkListContentListStore.getAll();

                checklistRequest.onsuccess = function (event) {
                    const checklistItems = event.target.result.filter(item => item.checklistID === parseInt(ddlChecklistInsp, 10));

                    if (checklistItems.length > 0) {
                        checklistItems.forEach(item => {
                            const newInspectionListDetail = {
                                RegisterID: parseInt(RegisterID, 10),
                                InspectionID: InspectionID,
                                CheckListID: item.checklistID,
                                ContentID: item.ContentID,
                                insertedOffline: true
                            };

                            inspectionListDetailsStore.add(newInspectionListDetail).onsuccess = function () {
                                console.log("Record added to tblInspectionListDetails:", newInspectionListDetail);
                            };
                        });
                        GetInspectionListData(RegisterID);

                        $('#Adddocs').hide();
                        CustomAlert({ Result: "Success", Message: "Inspection data inserted successfully!" });
                    } else {
                        CustomAlert({ Result: "Warning", Message: "No checklist items found for the selected Checklist ID." });
                    }

                    // Redirect after success
                    //const redirectUrl = '/OfflineInspectionForm.html?UID=' + UID +
                    //    '&UserRole=' + UserRole +
                    //    '&id=' + id +
                    //    '&SiteLocId=' + SiteLocId +
                    //    '&RID=' + RID +
                    //    '&CheckListID=' + ddlChecklistInsp +
                    //    '&InspID=' + InspectionID;

                    const redirectUrl = 'OfflineInspectionForm.html?UID=' + encodeURIComponent(UID) +
                        '&UserRole=' + encodeURIComponent(UserRole) +
                        '&OfflineUserRole=' + encodeURIComponent(OfflineUserRole) +
                        '&id=' + encodeURIComponent(id) +
                        '&SiteLocId=' + encodeURIComponent(SiteLocId) +
                        '&RID=' + encodeURIComponent(RID) +
                        '&CheckListID=' + encodeURIComponent(ddlChecklistInsp) +
                        '&InspID=' + encodeURIComponent(InspectionID);

                    console.log("Redirect URL:", redirectUrl);
                    window.location.href = redirectUrl;
                };

                checklistRequest.onerror = function (event) {
                    console.error("Error fetching checklist content:", event.target.error);
                    CustomAlert({ Result: "Failed", Message: "Error fetching checklist content." });
                    $('#LoadingPanel1').hide();
                };
            };

            addInspectionRequest.onerror = function (event) {
                console.error("Error inserting into tblInspectionDetails:", event.target.error);
                CustomAlert({ Result: "Failed", Message: "Error inserting inspection details." });
                $('#LoadingPanel1').hide();
            };
        };

        userRequest.onerror = function (event) {
            console.error("Error fetching user data:", event.target.error);
            CustomAlert({ Result: "Failed", Message: "Error fetching user data." });
            $('#LoadingPanel1').hide();
        };

        transaction.oncomplete = function () {
            console.log("Transaction completed: Inspection update operation finished.");
            GetInspectionListData(RegisterID);

        };
    };
}

function formatToUTCWithAMPM(date) {
    const months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

    const day = String(date.getDate()).padStart(2, '0');
    const month = months[date.getMonth()]; // always 3-letter month
    const year = date.getFullYear();

    let hours = date.getHours();
    const minutes = String(date.getMinutes()).padStart(2, '0');
    const ampm = hours >= 12 ? 'PM' : 'AM';

    hours = hours % 12;
    hours = hours ? hours : 12; // convert 0 to 12
    hours = String(hours).padStart(2, '0');

    return `${day}-${month}-${year} ${hours}:${minutes} ${ampm}`;
}



function Offinebtndeplanksave_Click() {
    $('#LoadingPanel1').show();

    // Extract required values
    var id = getParameterByName('id');
    /* var RID = getParameterByName('RID');*/
    let RID = getParameterByName('RID'); // Get RID from the URL
    console.log("Encrypted RID from URL:", RID);
    const RegisterID = OffDecrypt(RID);
    var UID = getParameterByName('UID');
    var InspComments = $('#txtcomm').val(); // Comments
    var txtdeplanl = $('#txtdeplanl').val(); // Deplank details
    var ddlInspecdeplank = $('#ddlInspecdeplank').val(); // Inspected by
    var InspectedBy = $('#ddlInspecdeplank option:selected').text();
    var InspectedById = $('#ddlInspecdeplank').val();
    var ScheduledOrAckDate = $('#dateInspection').val(); // Inspection date
    var hdnTimeOffSet = $('#hdnTimeOffSet').val(); // Timezone offset

    const inspectionDate = new Date(ScheduledOrAckDate);
    const formattedDate = formatToUTCWithAMPM(inspectionDate);

    // Validation
    if (!ddlInspecdeplank || ddlInspecdeplank.trim() === "") {
        CustomAlert({ Result: "Failed", Message: "Please select Inspected By." });
        $('#ddlInspecdeplank').addClass('requiredColumn'); // Highlight the field
        $('#LoadingPanel1').hide();
        return;
    }

    //if (!ddlInspecdeplank) {
    if (!formattedDate || formattedDate.trim() === "") {
        CustomAlert({ Result: "Failed", Message: "Please select Scheduled Inspection Date." });
        $('#dateInspection').addClass('requiredColumn');
        $('#LoadingPanel1').hide();
        return;
    }
    if (!InspectedById) {
        CustomAlert({ Result: "Failed", Message: "Please select Inspected By." });
        $('#ddlInspecdeplank').addClass('requiredColumn');
        $('#LoadingPanel1').hide();
        return;
    }
    // Open IndexedDB
    const dbRequest = indexedDB.open(dbName, dbVersion);

    dbRequest.onerror = function (event) {
        console.error("Database error: " + event.target.errorCode);
        CustomAlert({ Result: "Failed", Message: "Failed to open database." });
        $('#LoadingPanel1').hide();
    };

    dbRequest.onsuccess = function (event) {
        const db = event.target.result;
        const transaction = db.transaction(["tblInspectionDetails"], "readwrite");
        const objectStore = transaction.objectStore("tblInspectionDetails");

        // Check for existing record by RID
        /*const getRequest = registerStore.get(parseInt(RID, 10));*/
        /*  var getRequest = objectStore.get(Number(REGISTERId));*/
        const getRequest = objectStore.get(parseInt(RegisterID, 10));

        getRequest.onsuccess = function (event) {
            const existingRecord = event.target.result;

            if (existingRecord) {
                CustomAlert({ Result: "Failed", Message: "Record already exists. Cannot insert duplicate." });
                $('#LoadingPanel1').hide();
            } else {
                // Insert new record
                const newRecord = {
                    RegisterID: parseInt(RegisterID, 10), // Ensure key matches the store keyPath
                    InspComments: InspComments,
                    InspStatus: txtdeplanl,
                    InspectedBy: ddlInspecdeplank,
                    InspectedById: InspectedById,
                    InspectionDate: formattedDate,
                    TimeZoneOffset: hdnTimeOffSet,
                    InspectedBy: InspectedBy,
                    insertedOffline: true
                };

                const addRequest = objectStore.add(newRecord);

                addRequest.onsuccess = function () {
                    console.log("New record inserted successfully:", newRecord);
                    CustomAlert({ Result: "Success", Message: "Inspection data inserted successfully!" });
                    $('#LoadingPanel1').hide(); // Hide the loading panel after success
                    $('#divDeplank').hide();
                    OfflineshowInspection();
                    $('#AnchorInspection').click();
                    GetInspectionListData(RegisterID)
                };

                addRequest.onerror = function (event) {
                    console.error("Error inserting new record:", event.target.error);
                    CustomAlert({ Result: "Failed", Message: "Error inserting new record." });
                    $('#LoadingPanel1').hide();
                };
            }
        };

        getRequest.onerror = function (event) {
            console.error("Error checking record existence:", event.target.error);
            CustomAlert({ Result: "Failed", Message: "Error checking record existence." });
            $('#LoadingPanel1').hide();
        };
    };
}



async function OpenOfflineInspection(InspectionID) {

    // Open IndexedDB
    return new Promise((resolve, reject) => {
        const dbRequest = indexedDB.open(dbName, dbVersion);

        dbRequest.onerror = function (event) {
            console.error("Error opening database:", event.target.errorCode);
            reject({ Result: "Failed", Message: "Failed to open database." });
        };

        dbRequest.onsuccess = function (event) {
            const db = event.target.result;
            const transaction = db.transaction(["tblUserList"], "readonly");
            const objectStore = transaction.objectStore("tblUserList");

            // Open cursor to find the matching InspectionID
            const cursorRequest = objectStore.openCursor();

            let found = false;
            cursorRequest.onsuccess = function (event) {
                const cursor = event.target.result;

                if (cursor) {
                    const record = cursor.value;

                    if (String(record.InspectionID) === String(inspectionId)) {
                        found = true;

                        // Encrypt ChecklistId and InspectionID if necessary
                        const checklistId = record.ChecklistId;
                        const encryptedChecklistId = encrypt(checklistId);
                        const encryptedInspectionId = encrypt(InspectionID);

                        console.log("ChecklistId found:", checklistId);
                        resolve({
                            Result: "Success",
                            Message: `${encryptedChecklistId}_${encryptedInspectionId}`,
                        });
                        return;
                    }
                    cursor.continue(); // Continue searching
                } else {
                    // Cursor iteration complete, no match found
                    if (!found) {
                        console.log("No record found for InspectionID:", inspectionId);
                        resolve({ Result: "Failed", Message: "No Record Found." });
                    }
                }
            };

            cursorRequest.onerror = function (event) {
                console.error("Cursor error:", event.target.errorCode);
                reject({ Result: "Failed", Message: "Error reading data." });
            };
        };
    });
}


//neeraj changes


function GetTblregisterImageGelleryDataById(RegisterId) {
    if (!RegisterId || isNaN(RegisterId)) {
        console.error("Invalid RegisterId:", RegisterId);
        return;
    }

    var dbPromise = indexedDB.open(dbName, dbVersion);

    dbPromise.onsuccess = function (event) {
        var db = event.target.result;

        var transaction = db.transaction(["tblRegisterGallery"], "readonly");
        var objectStore = transaction.objectStore("tblRegisterGallery");

        // Open a cursor to iterate over records
        var request = objectStore.openCursor();
        var imagesData = [];

        request.onsuccess = function (event) {
            var cursor = event.target.result;
            if (cursor) {
                // Check if the current record has the same RegisterId
                if (cursor.value.RegisterId === parseInt(RegisterId, 10)) {
                    imagesData.push(cursor.value); // Collect matching image data
                }
                cursor.continue(); // Move to the next record
            } else {
                // All records have been iterated
                if (imagesData.length > 0) {
                    console.log("Images found for RegisterId:", RegisterId, imagesData);
                    displayRegisterImageDetails(imagesData); // Display images
                } else {
                    console.warn("No images found for RegisterId:", RegisterId);
                    //    $('#placeholderforimage').html("<p>No images available for this Register ID.</p>");
                }
            }
        };

        request.onerror = function (event) {
            console.error("Failed to retrieve images for RegisterId:", RegisterId, event);
        };
    };

    dbPromise.onerror = function (event) {
        console.error("Failed to open database:", event);
    };
}

function displayRegisterImageDetails(imagesData) {
    $('#placeholderforimage').html(""); // Clear the image container

    // Loop through the imagesData array to display each image
    imagesData.forEach(function (image) {
        var FileName = image.FileName || "";
        var Description = image.Description || "";
        var ID = image.ID || "";
        var imageUrl = FileName; // FileName contains the full URL
        var Images = image.Images;
        // Append each image and its description to the container
        $('#placeholderforimage').append(
            "<div class='col-lg-4 col-md-6 col-sm-12 col-xs-12' style='margin-top: 5px;'>" +
            "<img class='img-container imageGallery' data-my-image-id='" + ID + "' src='" + Images + "' style='width: 300px; object-fit: contain; height: 250px' />" +
            "<div class='row' style='margin-top: -18px; text-align: left;'>" +
            "<div class='col-lg-12 col-md-12 col-sm-6 col-xs-12'>" +
            "<a class='descr' style='color: black; font-size: 16px; font-weight: 600; margin-left: 32px;'>" + Description + "</a>" +
            "</div>" +
            "</div>" +
            "</div>"
        );
    });

    console.log("Images Data: ", imagesData);

    // Click event for images to open the popup
    $('.imageGallery').on('click', function () {
        var imageSrc = this.src; // Get the clicked image source
        var imageId = $(this).data('my-image-id'); // Retrieve the ID using data attribute

        $('#hdnimagegallery').val(imageId); // Set the hidden field value to the image ID
        $('#ppdl').attr("src", imageSrc); // Set the popup image to clicked image
        $('#popup').css('display', 'block'); // Show the popup
    });
    $('.crossbtn').on('click', function () {
        $('#popup').css('display', 'none');
    });
    $('#popup').on('click', function (event) {
        if (event.target == this) {
            $('#popup').css('display', 'none');
        }
    });
}

function AddLocalRegisterimageData(REGISTERId) {
    const fileInput = document.getElementById("FileUpload1");
    const capturedImageString = $("#snapimage").val(); // Captured image in base64 format
    const description = $("#txtdescimage").val();

    if (!description.trim()) {
        CustomAlert({ Result: "Failed", Message: "Please provide a description before adding the image." });
        return;
    }

    const files = fileInput.files;
    let base64mobImagedata = null;
    const recordsToInsert = [];

    // Handling the case for a captured image (without file upload)
    if (capturedImageString && files.length === 0) {
        compressImage(capturedImageString, function (compressedBase64) {
            if (compressedBase64) {
                recordsToInsert.push({
                    RegisterId: parseInt(REGISTERId, 10),
                    FileName: "Captured Image",
                    Description: description,
                    Images: compressedBase64,
                    insertedOffline: true
                });
                proceedWithDatabaseOperations(recordsToInsert);
            } else {
                CustomAlert({ Result: "Failed", Message: "Compressed image exceeds size limit." });
            }
        });
        return;
    }

    // Handling the case for file uploads
    if (files && files.length > 0) {
        Array.from(files).forEach((file) => {
            const reader = new FileReader();

            reader.onloadend = function () {
                base64mobImagedata = reader.result;
                compressImage(base64mobImagedata, function (compressedBase64) {
                    if (compressedBase64) {
                        recordsToInsert.push({
                            RegisterId: parseInt(REGISTERId, 10),
                            FileName: file.name,
                            Description: description,
                            Images: compressedBase64,
                            insertedOffline: true
                        });

                        // Check if all files are processed and add them to the database
                        if (recordsToInsert.length === files.length) {
                            proceedWithDatabaseOperations(recordsToInsert);
                        }
                    } else {
                        CustomAlert({ Result: "Failed", Message: "Compressed image exceeds size limit." });
                    }
                });
            };

            reader.onerror = function (event) {
                console.error("Error reading file:", event.target.error);
                CustomAlert({ Result: "Failed", Message: "Error reading file." });
            };

            reader.readAsDataURL(file);
        });
    } else {
        CustomAlert({ Result: "Failed", Message: "No file uploaded or image captured." });
    }

    // Function to compress the image
    function compressImage(base64Image, callback) {
        const img = new Image();
        img.onload = function () {
            // Create a canvas to resize the image
            const canvas = document.createElement("canvas");
            const ctx = canvas.getContext("2d");

            const MAX_WIDTH = 1024;  // Max width for the compressed image
            const MAX_HEIGHT = 1024; // Max height for the compressed image
            let width = img.width;
            let height = img.height;

            // Calculate the scaling ratio to maintain aspect ratio
            if (width > height) {
                if (width > MAX_WIDTH) {
                    height *= MAX_WIDTH / width;
                    width = MAX_WIDTH;
                }
            } else {
                if (height > MAX_HEIGHT) {
                    width *= MAX_HEIGHT / height;
                    height = MAX_HEIGHT;
                }
            }

            // Resize the image to the new dimensions
            canvas.width = width;
            canvas.height = height;
            ctx.drawImage(img, 0, 0, width, height);

            // Convert the image back to base64 with reduced quality
            const compressedBase64 = canvas.toDataURL("image/jpeg", 0.7); // 0.7 is the quality (0-1)

            // Check if the compressed image size is under 2MB
            const byteSize = (compressedBase64.length * 3) / 4;
            if (byteSize <= 2 * 1024 * 1024) {  // 2MB limit
                callback(compressedBase64);
            } else {
                callback(null); // Return null if the image exceeds 2MB
            }
        };
        img.src = base64Image;  // Set the source to the base64 image
    }

    // Function to handle database operations
    function proceedWithDatabaseOperations(records) {
        const dbRequest = indexedDB.open(dbName, dbVersion);

        dbRequest.onerror = function (event) {
            console.error("Database error: " + event.target.errorCode);
            CustomAlert({ Result: "Failed", Message: "Failed to open database." });
        };

        dbRequest.onsuccess = function (event) {
            const db = event.target.result;
            const transaction = db.transaction(["tblRegisterGallery"], "readwrite");
            const registerStore = transaction.objectStore("tblRegisterGallery");
            const getAllRequest = registerStore.getAll();

            getAllRequest.onsuccess = function () {
                const allRecords = getAllRequest.result || [];
                const matchingRecords = allRecords.filter(
                    (record) => record.RegisterId === parseInt(REGISTERId, 10)
                );

                // Check if there are already 4 or more records for this register
                if (matchingRecords.length + records.length > 2) {
                    CustomAlert({ Result: "Failed", Message: "Maximum of two images can be saved!" });
                    return; // Stop further processing if the limit is reached
                }

                // Insert records into the database
                records.forEach((newRecord) => {
                    const addRequest = registerStore.add(newRecord);

                    addRequest.onsuccess = function () {
                        console.log("New record inserted successfully.");
                    };

                    addRequest.onerror = function (event) {
                        console.error("Error inserting new record:", event.target.error);
                        CustomAlert({ Result: "Failed", Message: "Error inserting new record." });
                    };
                });

                // When all records are added successfully, show success message
                transaction.oncomplete = function () {
                    console.log("All records inserted successfully.");
                    CustomAlert({ Result: "Success", Message: "Image Gallery data inserted successfully!" });
                    $('#divCreatePop').css('display', 'none');
                    GetTblregisterImageGelleryDataById(REGISTERId);
                    //location.reload();
                    setTimeout(function () {
                        const currentUrl = new URL(window.location.href);
                        currentUrl.searchParams.set('activateTab', 'ImageGallery');
                        window.location.href = currentUrl.toString();
                    }, 1500);
                };

                transaction.onerror = function () {
                    console.error("Error in batch transaction.");
                    CustomAlert({ Result: "Failed", Message: "Error inserting data into database." });
                };
            };
        };
    }
}

function deleteno() {
    $('#divdelete').css('display', 'none');
    $('#divdeleteimagegellery').css('display', 'none');
}


function DeleteLocalRegisterImageData() {
    const dbRequest = indexedDB.open(dbName, dbVersion); // Open the database
    /*const RegisterId = getParameterByName('RID');*/ // Get the RegisterId from the URL
    let RID = getParameterByName('RID'); // Get RID from the URL
    console.log("Encrypted RID from URL:", RID);
    const RegisterId = OffDecrypt(RID);
    const ID = $('#hdnimagegallery').val(); // Get the Image ID from the hidden field

    if (!RegisterId || !ID) {
        console.error('Missing RegisterId or Image ID.');
        CustomAlert({ Result: "Failed", Message: "Missing required data: RegisterId or Image ID." });
        return;
    }

    console.log('Attempting to delete record with RegisterId:', RegisterId, 'Image ID:', ID);

    dbRequest.onerror = function (event) {
        console.error("Database error:", event.target.errorCode);
        CustomAlert({ Result: "Failed", Message: "Failed to open database." });
    };

    dbRequest.onsuccess = function (event) {
        const db = event.target.result;

        // Open a transaction for tblRegisterGallery
        const transaction = db.transaction(["tblRegisterGallery"], "readwrite");
        const registerStore = transaction.objectStore("tblRegisterGallery");

        // Open a cursor to iterate through records
        const cursorRequest = registerStore.openCursor();

        cursorRequest.onsuccess = function (event) {
            const cursor = event.target.result;

            if (cursor) {
                // Match ID and RegisterID
                const record = cursor.value;
                if (record.ID === parseInt(ID, 10) && record.RegisterId === parseInt(RegisterId, 10)) {
                    console.log('Matching record found:', record);

                    // Delete the record
                    const deleteRequest = cursor.delete();

                    deleteRequest.onsuccess = function () {
                        /* console.log(Record with ID ${ ID } and RegisterId ${ RegisterId } deleted successfully.);*/
                        CustomAlert({ Result: "Success", Message: "Record deleted successfully!" });
                        $('#divdeleteimagegellery').css('display', 'none');
                        GetTblregisterImageGelleryDataById(RegisterId);
                        setTimeout(function () {
                            const currentUrl = new URL(window.location.href);
                            currentUrl.searchParams.set('activateTab', 'ImageGallery');
                            window.location.href = currentUrl.toString();
                        }, 1500);
                    };

                    deleteRequest.onerror = function (event) {
                        console.error("Error deleting record:", event.target.error);
                        CustomAlert({ Result: "Failed", Message: "Error deleting the record." });
                    };
                } else {
                    // Continue to the next record
                    cursor.continue();
                }
            } else {
                // If no matching record is found
                /*  console.warn(No matching record found for ID ${ ID } and RegisterId ${ RegisterId }.);*/
                CustomAlert({ Result: "Failed", Message: "No matching record found to delete." });
            }
        };

        cursorRequest.onerror = function (event) {
            console.error("Error iterating records:", event.target.error);
            CustomAlert({ Result: "Failed", Message: "Error iterating records." });
        };
    };
}

function addnewimage() {
    var RID = OffDecrypt(getParameterByName('RID'));
    checkimagegelleryregister(RID);
}

function btnclose() {
    var output = document.getElementById('fileList');
    output.innerHTML = '';
    $('#txtdescimage').val('');
    $('#fileList').val('');
    $('#divCreatePop').css('display', 'none');
    $("#Photoblock").css('display', 'none');
    $('#txtdesc').val('');
    $('#TextBox1').val('');
}




function GetCycloneDataById(REGISTERId) {
    if (!REGISTERId) {
        console.error("REGISTERId is required");
        return;
    }

    var request = indexedDB.open(dbName, dbVersion);

    request.onerror = function (event) {
        console.error("Error opening IndexedDB:", event);
    };

    request.onsuccess = function (event) {
        var db = event.target.result;
        var transaction = db.transaction(["tblRegisterDetails"], "readonly");
        var objectStore = transaction.objectStore("tblRegisterDetails");

        // Fetch the record where REGISTERId matches
        var getRequest = objectStore.get(Number(REGISTERId));
        getRequest.onerror = function (event) {
            console.error("Error retrieving record:", event);
        };

        getRequest.onsuccess = function (event) {
            var record = event.target.result;

            if (record) {
                // Process and display the fetched record data
                displayCycloneDetails(record);
            } else {
                console.log("No matching record found for REGISTERId:", REGISTERId);
            }
        };
    };

    request.onupgradeneeded = function (event) {
        console.log("Database upgrade needed.");
    };
}

function showOfflineCyclone() {
    var NewRID;

    // Retrieve Current RID from IndexedDB
    var transaction1 = db.transaction(["tblCurrentRID"], "readonly");
    var objectStore1 = transaction1.objectStore("tblCurrentRID");

    objectStore1.openCursor().onsuccess = function (event) {
        var cursor = event.target.result;
        if (cursor) {
            NewRID = cursor.value.RID;
        }
    };

    // Use a timeout to ensure the first transaction has completed
    setTimeout(function () {
        var transaction2 = db.transaction(["tblRegisterDetails"], "readonly");
        var objectStore2 = transaction2.objectStore("tblRegisterDetails");

        // Retrieve Register Details using NewRID
        var request = objectStore2.index("rowid").getAll(NewRID); // Assuming "rowid" is an index

        request.onsuccess = function (event) {
            var result = event.target.result;

            if (result.length > 0) {
                console.log(result);
                $("#customLoader").css("display", "block");

                result.forEach(function (data) {

                    if (data.ddlCycloneAction) $("#ddlCycloneAction").val(data.ddlCycloneAction);
                    if (data.txtTotalDeBoardEdit) $("#txtTotalDeBoardEdit").val(data.txtTotalDeBoardEdit);
                    if (data.ddlRangeOfOutBoard) $("#ddlRangeOfOutBoard").val(data.ddlRangeOfOutBoard);
                    if ($("#chkuseentervalue").prop('checked')) {
                        $("#chkuseentervalue").prop('checked', data.chkuseentervalue);
                    }
                });
            }
            else {
                alert('No Cyclone Details Found');
            }
        };
    }, 500);
}
function readURL(input) {


    var radUpdogImg = $('#radUpdogImg').val();
    var fileUpload = $('#radUpdogImg').get(0);
    var file = fileUpload.files;
    if (file.length != 0) {

        var FileName = file[0].name;
        var extension = FileName.substring(FileName.lastIndexOf('.') + 1);

        if (extension == "jpg" || extension == "jpeg" || extension == "png" || extension == "ico" || extension == "gif" || extension == "bmp" || extension == "JPG" || extension == "JPEG" || extension == "PNG" || extension == "ICO" || extension == "GIF" || extension == "BMP") {

            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    $('#imga').attr('src', e.target.result);
                    var base64mobImagedata1 = e.target.result;
                    $("#basetempimg").val(base64mobImagedata1);
                }

                reader.readAsDataURL(input.files[0]); // convert to base64 string

                $('#deleteimage').css('display', 'block');

                var hdnSaveIsOffline = $('#hdnSaveIsOffline').val();
                if (hdnSaveIsOffline == "true") {
                    $('#btnImageSave').css('display', 'none');
                    $('#downloadButton').css('display', 'none');

                }
                else {
                    $('#btnImageSave').css('display', 'block');
                    $('#downloadButton').css('display', 'none');

                }

            }
        }
        else {
            var response = { Result: "Failed", Message: "<strong>Failed to upload Image! </strong>Valid File Extensions Are jpg, jpeg, png, gif, bmp" };
            CustomAlert(response)
            $("#imga").attr("src", "/images/noimg.jpg");
        }

    } else {
        // $('#img').attr('src', "images/noimg.jpg");
        $('#deleteimage').css('display', 'none');
        $('#btnImageSave').css('display', 'none');
        $('#downloadButton').css('display', 'none');

    }

}

function displayCycloneDetails(record) {
    // Assign values to form fields based on the fetched record
    var RangeOfOutBoard = record.WithInRangeOfOutBoard || '';
    var CycloneAction = record.CycloneAction || '';
    var TotalLabourDismantleEdit = record.TotalLabourDismantleEdit || '';

    //Cyclone Info
    $("#ddlCycloneAction").val(record.CycloneAction || '');
    $("#ddlRangeOfOutBoard").val(record.WithInRangeOfOutBoard || '');
    $("#ddlPriorityRiskRating").val(record.PriorityRiskRating || '');

    // Checkbox
    $("#chkuseentervalue").prop('checked', record.UseEnteredValue);

    if (RangeOfOutBoard == true) {
        $('#ddlRangeOfOutBoard').val(1);
    } else {
        $('#ddlRangeOfOutBoard').val(0);
    }

    if (CycloneAction == "Full Strip") {
        $('#divAlert').css('visibility', 'visible');
        $('#chkuseentervalue').prop('disabled', true);
        $('#chkuseentervalue').prop('checked', false);
    }
    else if (CycloneAction == "De-Plank Only" || CycloneAction == "Tie-Down") {
        $('#divAlert').css('visibility', 'hidden');
        $('#chkuseentervalue').prop('disabled', false);
        $('#chkuseentervalue').prop('checked', true);

    }
    else {
        $('#divAlert').css('visibility', 'hidden');
        $('#chkuseentervalue').prop('disabled', true);
        $('#chkuseentervalue').prop('checked', false);
    }

    if (TotalLabourDismantleEdit == null || TotalLabourDismantleEdit == 0) {
        $('#numberofhours').css('display', 'block');
    }
    else {
        $('#numberofhours').css('display', 'none');

    }

    var ChangesStatus = "";

    if (CycloneAction == "Tie-Down") {

        var ChangesStatus = CycloneAction.replace("Tie-Down", "'Tied-Down'")
    }

    else {
        var ChangesStatus = CycloneAction.replace("De-Plank Only", "'De-Planked'")
    }

    if (CycloneAction != "") {
        $('#cycloneactionStatus').html('NOTE:The Cyclone Action for this Job is: ' + ChangesStatus);
    }
    else {
        $('#cycloneactionStatus').html('NOTE:No Cyclone Action has been set for this Job');
    }

    if (TotalLabourDismantleEdit != "") {
        $('#txtTotalDeBoardEdit').val(TotalLabourDismantleEdit);
    }
}

function isJsonDate(dateStr) {
    return /^\/Date\(\d+\)\/$/.test(dateStr);
}

function formatJsonDate(jsonDate) {
    var timestamp = parseInt(jsonDate.replace(/\/Date\((\d+)\)\//, "$1"), 10);
    var dateObj = new Date(timestamp);

    // Extract date components
    var day = dateObj.toLocaleDateString("en-GB", { day: "2-digit" });
    var month = dateObj.toLocaleDateString("en-GB", { month: "short" });
    var year = dateObj.getFullYear();

    // Extract time components
    var hours = dateObj.getHours();
    var minutes = dateObj.getMinutes();
    var period = hours >= 12 ? "PM" : "AM";

    // Convert to 12-hour format
    hours = hours % 12 || 12; // Convert 0 to 12 for midnight

    // Ensure two-digit minutes
    minutes = minutes.toString().padStart(2, "0");

    // Combine date and time
    return `${day}-${month}-${year} ${hours}:${minutes} ${period}`;
}




function AddLocalCycloneData(REGISTERId) {

    // Open the IndexedDB database
    const dbRequest = indexedDB.open(dbName, dbVersion);

    dbRequest.onerror = function (event) {
        console.error("Database error: " + event.target.errorCode);
        CustomAlert({ Result: "Failed", Message: "Failed to open database." });
    };

    dbRequest.onsuccess = function (event) {
        const db = event.target.result;

        // Start a transaction on tblRegisterDetails
        const transaction = db.transaction(["tblRegisterDetails"], "readwrite");
        const registerStore = transaction.objectStore("tblRegisterDetails");

        // Fetch the record by REGISTERId
        const getRequest = registerStore.get(parseInt(REGISTERId, 10));

        getRequest.onsuccess = function (event) {
            const registerRecord = event.target.result;

            if (registerRecord) {
                var txtTotalDeBoardEdit = $('#txtTotalDeBoardEdit').val();
                var chkuseentervalue = $('#chkuseentervalue').prop('checked');

                // Update the fields with form values
                if (txtTotalDeBoardEdit != "" && chkuseentervalue || txtTotalDeBoardEdit == "" && !chkuseentervalue) {
                    registerRecord.CycloneAction = $("#ddlCycloneAction").val();
                    registerRecord.WithInRangeOfOutBoard = $("#ddlRangeOfOutBoard").val();
                    registerRecord.PriorityRiskRating = $("#ddlPriorityRiskRating").val();
                    registerRecord.TotalLabourDismantleEdit = txtTotalDeBoardEdit;
                    registerRecord.UseEnteredValue = chkuseentervalue;
                    registerRecord.UpdatedOffline = true;

                    // Save the updated record
                    const updateRequest = registerStore.put(registerRecord);

                    updateRequest.onsuccess = function () {
                        console.log("Cyclone data updated successfully for REGISTERId:", REGISTERId);
                        //alert("Cyclone data updated successfully!");
                        var Result = { Result: "Success", Message: "Record updated successfully!" };
                        CustomAlert(Result);

                        // Optional: Verify the updated record
                        const verifyRequest = registerStore.get(REGISTERId);
                        verifyRequest.onsuccess = function (event) {
                            const updatedRecord = event.target.result;
                            console.log("Updated Cyclone record:", updatedRecord);
                        };
                    };
                    updateRequest.onerror = function () {
                        console.error("Error updating cyclone data for REGISTERId:", REGISTERId);
                        CustomAlert({ Result: "Failed", Message: "Error updating cyclone data." });
                    };
                }
                else {
                    if (txtTotalDeBoardEdit == "" && chkuseentervalue) {
                        var Result = { Result: "Failed", Message: "You have checked the Box for <strong>Use Entered Value (For De-Plank Only)</strong>. Please fill <strong>Entered Value </br>Preparation Hrs" };
                        CustomAlert(Result);
                    }
                    else {
                        var Result = { Result: "Failed", Message: "You have Entered Preparation Hrs Please select the <strong>Use Entered Value (For De-Plank Only)." };
                        CustomAlert(Result);
                    }
                }

            } else {
                console.warn("Record not found for REGISTERId:", REGISTERId);
                CustomAlert({ Result: "Failed", Message: `No record found for REGISTERId: ${REGISTERId}` });
            }
        };

        getRequest.onerror = function () {
            console.error("Error fetching record for REGISTERId:", REGISTERId);
            CustomAlert({ Result: "Failed", Message: "Error fetching record from tblRegisterDetails." });
        };
        transaction.oncomplete = function () {
            console.log("Transaction completed: Cyclone update operation finished.");
            GetCycloneDataById(REGISTERId);

        };
    };
}

function GetCycloneTabHideShow(ClientID, SiteLocID) {
    const request = indexedDB.open(dbName, dbVersion);
    request.onsuccess = function (event) {
        const db = event.target.result;
        const transaction = db.transaction('tblSiteList', 'readonly');
        const objectStore = transaction.objectStore('tblSiteList');

        const siteRequest = objectStore.get(parseInt(SiteLocID, 10));
        //const cursorRequest = objectStore.openCursor();
        let found = false;

        siteRequest.onsuccess = function () {
            const siteData = siteRequest.result;

            if (siteData.SiteLocID === parseInt(SiteLocID, 10) && siteData.CycloneManagement == true) {
                found = true;
                $('#liCyclone').css('display', 'block');
                $('#btnDeplank').css('display', 'block');
            }
            else {
                $('#liCyclone').css('display', 'none');
                $('#btnDeplank').css('display', 'none');
            }

            if (siteData.SiteLocID === parseInt(SiteLocID, 10) && siteData.PipeLineInvHide == false) {

                $('#limaterialPhase2').css('display', 'none');
            }
            else {
                $('#limaterialPhase2').css('display', 'block');
            }
            if (!found) {
                console.error('No record found for ClientID:', parseInt(ClientID, 10));
            }

        };

        siteRequest.onerror = function (event) {
            console.error('Error fetching data:', event.target.error);
        };
    };

    request.onerror = function (event) {
        console.error('Error opening database:', event.target.error);
    };
}

/*pipeline tab start*/

function MaterialChange() {
    //var materialType = document.getElementById('ddlMaterialType').value;
    var materialType = document.getElementById('ddlpipelineMaterialType').value;

    console.log("Selected Material Type:", materialType);
    var RID = getParameterByName('RID');
    RID = OffDecrypt(RID);
    getInventoryList(RID, materialType);
    // Optionally, you can call getInventoryRequest() or other functions here
}
function populatematerialDropdown(storeName, dropdownSelector, valueField, textField, REGISTERId) {
    if (!REGISTERId) {
        console.error("REGISTERId is required");
        return;
    }
    var distinctValues = new Set();

    // Clear the dropdown before fetching data
    $(dropdownSelector).empty();
    const dropdown = document.querySelector(dropdownSelector);

    // Clear existing options in the dropdown
    // dropdown.innerHTML = '<option value="">All</option>';

    // Open IndexedDB and fetch data
    var dbPromise = indexedDB.open(dbName, dbVersion);
    dbPromise.onsuccess = function (event) {
        var db = event.target.result;
        var transaction = db.transaction(storeName, 'readonly');
        var store = transaction.objectStore(storeName);
        //const index = store.index('REGISTERId');
        // Fetch the record where REGISTERId matches


        // const request = index.getAll(parseInt(REGISTERId, 10));
        var request = store.openCursor();

        request.onsuccess = function (event) {
            //var data = event.target.result;
            //data.forEach(function (item) {
            //    distinctValues.add(item[valueField]);
            //});

            var cursor = event.target.result;
            if (cursor) {
                let record = cursor.value;

                if (record.REGISTERId === parseInt(REGISTERId, 10)) {
                    distinctValues.add(record.MaterialType, record);
                }

                cursor.continue();
            }


            // Clear the dropdown before appending new options
            $(dropdownSelector).empty();

            // Append a default "Select" option
            $(dropdownSelector).append('<option value="">Select</option>');

            // Loop through the distinct values and append them to the dropdown
            distinctValues.forEach(function (value) {
                $(dropdownSelector).append(
                    $('<option></option>').attr('value', value).text(value)
                );
            });
        };

        request.onerror = function (event) {
            console.error("Error retrieving data from", storeName, ":", event);
        };
    };

    dbPromise.onerror = function (event) {
        console.error("Error opening IndexedDB:", event);
    };
}
function getInventoryList(REGISTERId, materialType) {
    if (!REGISTERId) {
        console.error("REGISTERId is required");
        return;
    }

    var request = indexedDB.open(dbName, dbVersion);

    request.onerror = function (event) {
        console.error("Error opening IndexedDB:", event);
    };

    request.onsuccess = function (event) {
        var db = event.target.result;

        // Begin a transaction and access the object store
        var transaction = db.transaction(["tblPipelineRequest"], "readonly");
        var objectStore = transaction.objectStore("tblPipelineRequest");
        var getRequest = objectStore.openCursor();
        let addedRecords = new Map(); // Track added records to prevent duplicates

        getRequest.onerror = function (event) {
            console.error("Error retrieving records:", event.target.error);
        };

        getRequest.onsuccess = function (event) {
            var cursor = event.target.result;
            if (cursor) {
                let record = cursor.value;
                let inventoryId = record.InventoryId;

                if (!materialType || record.MaterialType === materialType) {

                    let existingRecord = addedRecords.get(inventoryId);

                    // Check if the current record is better (registerid !== 0)
                    if (record.REGISTERId === parseInt(REGISTERId, 10)) {
                        if (record.Quantity !== "0") {
                            addedRecords.set(inventoryId, record);
                        }
                    }

                }
                cursor.continue();
            } else {
                // Once the cursor has iterated through all entries, process the records
                //if (addedRecords.size === 0) {
                //    console.log("No records found.");
                //} else {
                console.log("Records found:", addedRecords);
                // Call a function to process and bind the data, passing the unique records
                displayInventoryDetails(Array.from(addedRecords.values()));
                //}
            }
        };

        request.onupgradeneeded = function (event) {
            console.log("Database upgrade needed.");
        };
    };
}

function displayInventoryDetails(data) {
    var str = "";
    for (var i = 0; i < data.length; i++) {
        var record = data[i];

        var MaterialType = record.MaterialType || "";
        var ItemCode = record.ItemCode || "";
        var ItemDesc = record.ItemDesc || "";

        var Quantity = record.Quantity || 0;
        var ItemWeight = record.ItemWeight || "";
        var TotalWeight = record.TotalWeight || "";

        str += "<tr>";


        str += "<td class='setWidthI1'>" + MaterialType + "</td>";
        str += "<td class='setWidthI2'>" + ItemCode + "</td>";
        str += "<td class='setWidthI3'>" + ItemDesc + "</td>";

        str += "<td class='setWidthI4'>" + parseInt(Quantity, 10) + "</td>";
        str += "<td class='setWidthI5'>" + (isNaN(parseFloat(ItemWeight)) ? 0 : parseFloat(ItemWeight).toFixed(2)) + "</td>";
        str += "<td class='setWidthI6'>" + (isNaN(parseFloat(TotalWeight)) ? 0 : parseFloat(TotalWeight).toFixed(2)) + "</td>";

        str += "</tr>";
    }

    bindDataToTable(str); // Ensure this function is correctly implemented

}

function bindDataToTable(str) {
    if ($.fn.DataTable.isDataTable('#tblRegisterInventoryList')) {
        $('#tblRegisterInventoryList').DataTable().clear().destroy();
    }

    //if (!str || str.trim() === "") {
    //    // If no data, show "No data available" in the table
    //    $('#tbodyRegisterInventoryList').html('<tr><td colspan="7" class="text-center">No data available</td></tr>');
    //} else {
    // If data exists, update the table rows
    $('#tbodyRegisterInventoryList').html(str);  // Assuming 'str' contains the table rows
    //}

    table = $('#tblRegisterInventoryList').DataTable({
        "bSort": false,
        "processing": true,
        searching: true,
        "bLengthChange": true,
        autoWidth: true,
        scrollX: true,
        scrollY: "60vh",
        scrollCollapse: true,
        paging: false,
        dom: 'Bfrtip',
        buttons: [

            {
                extend: 'csvHtml5',
                title: 'Job Inventory List',

                exportOptions: {

                    columns: [1, 2, 3, 4, 5, 6],
                }

            }

        ],


    });


}// Function to process and display the fetched record data

function btnrequest_Click() {
    var ClientID = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var RID = getParameterByName('RID');

    var Siteid = getParameterByName('SiteLocId');

    var fromtocheck = "2";
    var page = "2";
    var from = "Job";
    var To = "Site";

    //var EncryptData = OffEncrypt([ from, To, RID, Siteid]);

    //from = EncryptData[0];
    //To = EncryptData[1];
    //RID = EncryptData[2];
    //Siteid = EncryptData[3];
    /*window.location.href = 'OfflineSiteRequest.html?UID=' + UID + '&UserRole=' + UserRole + '&id=' + ClientID + '&FromId=' + RID + '&ToId=' + Siteid + '&from=' + from + '&To=' + To + '&fromtocheck=' + fromtocheck + '&page=' + page;*/
    window.location.href = `OfflineSiteRequest.html?UID=${encodeURIComponent(UID)}&UserRole=${encodeURIComponent(UserRole)}&OfflineUserRole=${encodeURIComponent(OfflineUserRole)}&id=${encodeURIComponent(ClientID)}&FromId=${encodeURIComponent(RID)}&ToId=${encodeURIComponent(Siteid)}&from=${encodeURIComponent(from)}&To=${encodeURIComponent(To)}&fromtocheck=${encodeURIComponent(fromtocheck)}&page=${encodeURIComponent(page)}`;



}
/* added by pooja for jobinventory listing */

function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}
//pipeline tab end



//upload to cloude module Start by s.sourabh 
function uploadDataToServer() {
    $("#customLoader").css("display", "block");

    const tblRegisterDetailsData = [];
    const tblRegisterGalleryData = [];
    const tblMaterialTypeData = [];
    const tblDutyRatingData = [];
    const tblAssetListData = [];
    const tblPipelineRequestData = [];
    const tblPipelineDetailtData = [];
    let tablesWithData = 0;
    const finalDataArray = [];
    const tblInspectionDetailsData = [];
    const tblInspectionListDetailsData = [];

    function fetchTableData(storeName, field, dataArray) {

        REGISTERId = OffDecrypt(getParameterByName('RID'));
        return new Promise((resolve, reject) => {
            const request = indexedDB.open(dbName, dbVersion);
            request.onsuccess = function (event) {
                const db = event.target.result;
                const transaction = db.transaction(storeName, "readonly");
                const objectStore = transaction.objectStore(storeName);
                const query = objectStore.openCursor();

                query.onsuccess = function (event) {
                    const cursor = event.target.result;
                    if (cursor) {
                        let record = cursor.value;
                        if (cursor.value[field] === true) {
                            dataArray.push(cursor.value);
                        }
                        if (field === true) {
                            if (record.REGISTERId === parseInt(REGISTERId, 10)) {
                                dataArray.push(cursor.value);
                            }
                        }
                        cursor.continue();
                    } else {
                        resolve();
                    }
                };

                query.onerror = function () {
                    reject(`Error fetching data from ${storeName}`);
                };
            };

            request.onerror = function () {
                reject("Error opening database");
            };
        });
    }

    const tables = [
        { name: "tblRegisterDetails", field: "UpdatedOffline", dataArray: tblRegisterDetailsData },
        { name: "tblRegisterGallery", field: "insertedOffline", dataArray: tblRegisterGalleryData },
        { name: "tblMaterialType", field: "insertedOffline", dataArray: tblMaterialTypeData },
        { name: "tblDutyRating", field: "insertedOffline", dataArray: tblDutyRatingData },
        { name: "tblAssetList", field: "insertedOffline", dataArray: tblAssetListData },
        { name: "tblAssetList", field: "insertedOffline", dataArray: tblAssetListData },
        { name: "tblPipelineRequest", field: "insertedOffline", dataArray: tblPipelineRequestData },
        { name: "tblPipelineDetails", field: true, dataArray: tblPipelineDetailtData },
        { name: "tblInspectionDetails", field: "insertedOffline", dataArray: tblInspectionDetailsData },
        { name: "tblInspectionListDetails", field: "insertedOffline", dataArray: tblInspectionListDetailsData }
    ];

    const tablePromises = tables.map(table => {
        return fetchTableData(table.name, table.field, table.dataArray);
    });

    Promise.all(tablePromises)
        .then(() => {
            if (tblRegisterDetailsData.length > 0) {
                finalDataArray.push({ TableName: "tblRegisterDetails", Data: tblRegisterDetailsData });
                tablesWithData++;
            }
            if (tblRegisterGalleryData.length > 0) {
                finalDataArray.push({ TableName: "tblRegisterGallery", Data: tblRegisterGalleryData });
                tablesWithData++;
            }
            if (tblMaterialTypeData.length > 0) {
                finalDataArray.push({ TableName: "tblMaterialType", Data: tblMaterialTypeData });
                tablesWithData++;
            }
            if (tblDutyRatingData.length > 0) {
                finalDataArray.push({ TableName: "tblDutyRating", Data: tblDutyRatingData });
                tablesWithData++;
            }
            if (tblAssetListData.length > 0) {
                finalDataArray.push({ TableName: "tblAssetList", Data: tblAssetListData });
                tablesWithData++;
            }
            if (tblPipelineDetailtData.length > 0) {
                $.ajax({
                    url: "/OfflineRegister/DeletePipelinedata",
                    type: "POST",
                    contentType: "application/json",
                    data: JSON.stringify({
                        LoginUserId: OffDecrypt(UID),
                        clientId: ClientID,
                        //Siteid: SiteLocId
                        Siteid: SiteLocID,
                        UserRole: OffDecrypt(OfflineUserRole),
                        RID: RID
                    }),
                    async: false,
                    success: function (response) {
                        if (response.Success) {
                            CustomAlert({ Result: "Success", Message: response.message });

                        } else {
                            CustomAlert({ Result: "Failed", Message: response.message });

                        }
                    }
                });
            }

            if (tblPipelineRequestData.length > 0) {
                finalDataArray.push({ TableName: "tblPipelineRequest", Data: tblPipelineRequestData });
                tablesWithData++;
            }
            if (tblInspectionDetailsData.length > 0) {
                finalDataArray.push({ TableName: "tblInspectionDetails", Data: tblInspectionDetailsData });
                tablesWithData++;
            }
            if (tblInspectionListDetailsData.length > 0) {
                finalDataArray.push({ TableName: "tblInspectionListDetails", Data: tblInspectionListDetailsData });
                tablesWithData++;
            }
            if (tablesWithData > 0) {
                let uploadMessage = "We are uploading data for: ";
                //uploadMessage += finalDataArray.map(item => item.TableName).join(", ");
                const tableNameMapping = {
                    tblRegisterDetails: "Job",
                    tblPipelineRequest: "Offline Job Inventory",
                    tblInspectionListDetails: "Inspection",
                    tblInspectionDetails: "Inspection List",
                    tblRegisterGallery: "Image Gallery"
                };
                uploadMessage += finalDataArray
                    .map(item => tableNameMapping[item.TableName] || item.TableName)
                    .join(", ");
                alert(uploadMessage);

                sendAllDataToServer(finalDataArray);
            } else {
                $("#customLoader").css("display", "none");
                showConfirmationModal({
                    title: '<i class="fa fa-cloud-upload"></i>&nbsp;No Updates Found in Offline Data to Upload',
                    message: 'Would you like to clear the offline data?<br><br>' +
                        '1. Click <b>Yes</b> to clear the offline data.<br>' +
                        '2. Click <b>No</b> to keep the offline data.',

                    onYes: function () {
                        $.ajax({
                            url: "/OfflineRegister/UpdateOfflineInspectionFlag",
                            type: "POST",
                            contentType: "application/json",
                            data: JSON.stringify({
                                LoginUserId: OffDecrypt(UID),
                                clientId: ClientID,
                                //Siteid: SiteLocId
                                Siteid: SiteLocID,
                                UserRole: OffDecrypt(OfflineUserRole),
                                RID: RID
                            }),
                            async: false,
                            success: function (response) {
                                if (response.Result == 'Success') {
                                    CustomAlert({ Result: "Success", Message: response.Message });

                                    clearAllTables(function () {
                                        CustomAlert({
                                            Result: "Success", Message: "Data has been updated on cloud and has been cleared from the local device."
                                        });
                                        setTimeout(function () {
                                            window.location.href = '/Register/Register?UID=' + response.UID + '&UserRole=' + response.UserRole + '&id=' + response.id + '&RID=' + response.RID;
                                        }, 2000);
                                    });


                                } else {
                                    CustomAlert({ Result: "Failed", Message: response.Message });
                                    setTimeout(function () {
                                        window.location.href = '/Register/Register?UID=' + response.UID + '&UserRole=' + response.UserRole + '&id=' + response.id + '&RID=' + response.RID;
                                    }, 2000);
                                }
                            },
                            error: function (xhr, status, error) {
                                console.error("Failed to upload all data:", error);
                            }
                        });
                    },
                    onNo: function () {
                        console.log('Upload cancelled');
                    }
                });
            }
        })
        .catch(error => {
            console.error("Error fetching table data:", error);
        });
}

// Split method for upload to cloud - without export error
//function sendAllDataToServer(finalDataArray) {
//    //$("#customLoader").css("display", "block");
//    // Added on 7Feb2025 to store Indexdb table count
//    sendIndexDbEntryCountToServer(finalDataArray);

//    let currentRowIndex = 0;

//    function uploadNextRow() {
//        if (currentRowIndex < finalDataArray.length) {
//            const currentRow = finalDataArray[currentRowIndex];

//            const payload = {
//                data: [currentRow],
//                UserId: OffDecrypt(UID),
//                ClientID: ClientID,
//                //Siteid: SiteLocId
//                Siteid: SiteLocID,
//                UserRole: OffDecrypt(OfflineUserRole),
//                RID: RID
//            };

//            $.ajax({
//                url: "/OfflineRegister/UploadToCloud",
//                type: "POST",
//                contentType: "application/json",
//                data: JSON.stringify(payload),
//                success: function (response) {
//                    if (response.success) {
//                        CustomAlert({ Result: response.Result, Message: response.Message });
//                        currentRowIndex++;
//                        uploadNextRow();
//                    } else {
//                        console.error(
//                            `Error uploading row ${currentRowIndex + 1}:`,
//                            response.message
//                        );
//                        CustomAlert({ Result: "Failed", Message: response.message });
//                    }
//                },
//                error: function (xhr, status, error) {
//                    const tableNameMapping = {
//                        tblRegisterDetails: "Job",
//                        tblPipelineRequest: "Job Inventory",
//                        tblInspectionListDetails: "Inspection",
//                        tblInspectionDetails: "Inspection List",
//                        tblRegisterGallery: "Image Gallery"
//                    };
//                    alert(
//                        'Failed to upload row for ' +
//                        (tableNameMapping[currentRow.TableName] || currentRow.TableName) +
//                        ' data : ' + error
//                    );

//                    console.error(
//                        `Failed to upload row ${currentRowIndex + 1}:`,
//                        error
//                    );
//                    $("#customLoader").css("display", "none");

//                },
//            });
//        } else {
//            console.log("All rows uploaded successfully.");
//            $.ajax({
//                url: "/OfflineRegister/UpdateOfflineInspectionFlag",
//                type: "POST",
//                contentType: "application/json",
//                data: JSON.stringify({
//                    LoginUserId: OffDecrypt(UID),
//                    clientId: ClientID,
//                    //Siteid: SiteLocId
//                    Siteid: SiteLocID,
//                    UserRole: OffDecrypt(OfflineUserRole),
//                    RID: RID
//                }),
//                async: false,
//                success: function (response) {
//                    if (response.Result == 'Success') {
//                        CustomAlert({ Result: "Success", Message: response.Message });

//                        clearAllTables(function () {
//                            $("#customLoader").css("display", "none");

//                            CustomAlert({
//                                Result: "Success", Message: "Data has been updated on cloud and has been cleared from the local device."
//                            });

//                            clearCache();

//                            setTimeout(function () {
//                                window.location.href = '/Register/Register?UID=' + response.UID + '&UserRole=' + response.UserRole + '&id=' + response.id + '&RID=' + response.RID;
//                            }, 3000);
//                        });


//                    } else {
//                        //CustomAlert({ Result: "Failed", Message: response.Message });
//                        //setTimeout(function () {
//                        //    window.location.href = '/Register/Register?UID=' + response.UID + '&UserRole=' + response.UserRole + '&id=' + response.id + '&RID=' + response.RID;
//                        //}, 2000);
//                        //$("#customLoader").css("display", "none");

//                        console.error("Error from server:", response.message);
//                        CustomAlert({ Result: "Failed", Message: response.message });
//                        $("#customLoader").css("display", "none");
//                    }
//                },
//                error: function (xhr, status, error) {
//                    console.error("Failed to upload all data:", error);
//                    console.error("XHR Response:", xhr);
//                    console.error("Status:", status);
//                    alert('Failed to upload all data: ' + error + '\nResponse: ' + xhr.responseText);
//                    $("#customLoader").css("display", "none");

//                }
//            });

//            $("#customLoader").css("display", "none");
//        }
//    }

//    // Start uploading the first row
//    if (finalDataArray.length > 0) {
//        uploadNextRow();
//    } else {
//        alert("There is no data to upload to the server.");
//        $("#customLoader").css("display", "none");

//    }
//}

// Split method for upload to cloud - with export error
//function sendAllDataToServer(finalDataArray) {
//    //$("#customLoader").css("display", "block");
//    // Added on 7Feb2025 to store Indexdb table count
//    sendIndexDbEntryCountToServer(finalDataArray);

//    let currentRowIndex = 0;

//    function uploadNextRow() {
//        if (currentRowIndex < finalDataArray.length) {
//            const currentRow = finalDataArray[currentRowIndex];

//            const payload = {
//                data: [currentRow],
//                UserId: OffDecrypt(UID),
//                ClientID: ClientID,
//                //Siteid: SiteLocId
//                Siteid: SiteLocID,
//                UserRole: OffDecrypt(OfflineUserRole),
//                RID: RID
//            };

//            var formData = new FormData();
//            formData.append("jsonData", JSON.stringify(payload));

//            const jsonData = JSON.stringify(payload);

//            const byteSize = new TextEncoder().encode(jsonData).length;

//            $.ajax({
//                url: "/OfflineRegister/UploadToCloud",
//                type: "POST",
//                contentType: "application/json",
                //data: formData,
                //contentType: false,
                //processData: false,
//                success: function (response) {
//                    if (response.success) {
//                        CustomAlert({ Result: response.Result, Message: response.Message });
//                        currentRowIndex++;
//                        uploadNextRow();
//                    } else {
//                        console.error(
//                            `Error uploading row ${currentRowIndex + 1}:`,
//                            response.message
//                        );
//                        CustomAlert({ Result: "Failed", Message: response.message });
//                    }
//                },
//                error: function (xhr, status, error) {
//                    const tableNameMapping = {
//                        tblRegisterDetails: "Job",
//                        tblPipelineRequest: "Job Inventory",
//                        tblInspectionListDetails: "Inspection",
//                        tblInspectionDetails: "Inspection List",
//                        tblRegisterGallery: "Image Gallery"
//                    };
//                    alert(
//                        'Failed to upload row for ' +
//                        (tableNameMapping[currentRow.TableName] || currentRow.TableName) +
//                        ' data : ' + error
//                    );

//                    console.error(`Failed to upload row ${currentRowIndex + 1}:`, error);
//                    console.error("XHR Response:", xhr);
//                    console.error("Status:", status);

//                    const errorDetails = {
//                        error: error,
//                        status: status,
//                        xhrResponseText: xhr.responseText,
//                        responseSizeInBytes: byteSize,
//                        requestData: payload,
//                        jsonString: jsonData,
//                        timestamp: new Date().toISOString()
//                    };

//                    const fileContent = JSON.stringify(errorDetails, null, 2);
//                    const blob = new Blob([fileContent], { type: 'text/plain' });
//                    const link = document.createElement('a');
//                    link.href = URL.createObjectURL(blob);
//                    link.download = 'error-details.txt';
//                    link.click();
//                    alert('Failed to upload all data: ' + error + '\nResponse: ' + xhr.responseText);
//                    $("#customLoader").css("display", "none");

//                },
//            });
//        } else {
//            console.log("All rows uploaded successfully.");
//            $.ajax({
//                url: "/OfflineRegister/UpdateOfflineInspectionFlag",
//                type: "POST",
//                contentType: "application/json",
//                data: JSON.stringify({
//                    LoginUserId: OffDecrypt(UID),
//                    clientId: ClientID,
//                    //Siteid: SiteLocId
//                    Siteid: SiteLocID,
//                    UserRole: OffDecrypt(OfflineUserRole),
//                    RID: RID
//                }),
//                async: false,
//                success: function (response) {
//                    if (response.Result == 'Success') {
//                        CustomAlert({ Result: "Success", Message: response.Message });

//                        clearAllTables(function () {
//                            $("#customLoader").css("display", "none");

//                            CustomAlert({
//                                Result: "Success", Message: "Data has been updated on cloud and has been cleared from the local device."
//                            });

//                            clearCache();

//                            setTimeout(function () {
//                                window.location.href = '/Register/Register?UID=' + response.UID + '&UserRole=' + response.UserRole + '&id=' + response.id + '&RID=' + response.RID;
//                            }, 3000);
//                        });


//                    } else {
//                        //CustomAlert({ Result: "Failed", Message: response.Message });
//                        //setTimeout(function () {
//                        //    window.location.href = '/Register/Register?UID=' + response.UID + '&UserRole=' + response.UserRole + '&id=' + response.id + '&RID=' + response.RID;
//                        //}, 2000);
//                        //$("#customLoader").css("display", "none");

//                        console.error("Error from server:", response.message);
//                        CustomAlert({ Result: "Failed", Message: response.message });
//                        $("#customLoader").css("display", "none");
//                    }
//                },
//                error: function (xhr, status, error) {
//                    console.error("Failed to upload all data:", error);
//                    console.error("XHR Response:", xhr);
//                    console.error("Status:", status);
//                    alert('Failed to upload all data: ' + error + '\nResponse: ' + xhr.responseText);
//                    $("#customLoader").css("display", "none");

//                }
//            });

//            $("#customLoader").css("display", "none");
//        }
//    }

//    // Start uploading the first row
//    if (finalDataArray.length > 0) {
//        uploadNextRow();
//    } else {
//        alert("There is no data to upload to the server.");
//        $("#customLoader").css("display", "none");

//    }
//}

// Non-Split method for upload to cloud
//function sendAllDataToServer(finalDataArray) {
//    // Added on 7Feb2025 to store Indexdb table count
//    sendIndexDbEntryCountToServer(finalDataArray);

//    //processPipelineData(dbName, dbVersion, OffDecrypt(UID));
//    RID = OffDecrypt(getParameterByName('RID'));
//            $.ajax({
//        url: "/OfflineRegister/UploadToCloud",
//                type: "POST",
//                contentType: "application/json",
//                data: JSON.stringify({
//		            data: finalDataArray,
//		            UserId: OffDecrypt(UID),
//		            ClientID: ClientID,
//                    //Siteid: SiteLocId
//                    Siteid: SiteLocID,
//                    UserRole: OffDecrypt(OfflineUserRole),
//                    RID: RID
//                }),
//                async: false,
//                success: function (response) {
//            if (response.success) {
//                CustomAlert({ Result: "Success", Message: response.message });

//                        clearAllTables(function () {
//                            CustomAlert({
//                                Result: "Success", Message: "Data has been updated on cloud and has been cleared from the local device."
//                            });
//                    //clearCache();
//                    //        setTimeout(function () {
//                    //            window.location.href = '/Register/Register?UID=' + response.UID + '&UserRole=' + response.UserRole + '&id=' + response.id + '&RID=' + response.RID;
//                            //}, 3000);
//                            setTimeout(function () {
//                                window.location.href = '/Register/Register?UID=' + response.UID + '&UserRole=' + response.UserRole + '&id=' + response.id + '&RID=' + response.RID;
//                    }, 3000);

//                    $("#customLoader").css("display", "none");
//                        });


//                    } else {
//                        console.error("Error from server:", response.message);
//                        CustomAlert({ Result: "Failed", Message: response.message });
//                        $("#customLoader").css("display", "none");
//                    }
//                },
//                error: function (xhr, status, error) {
//                    console.error("Failed to upload all data:", error);
//                    console.error("XHR Response:", xhr);
//                    console.error("Status:", status);
//                    alert('Failed to upload all data: ' + error + '\nResponse: ' + xhr.responseText);
//                    $("#customLoader").css("display", "none");
//                }
//            });
//}

//17 feb 2025 start by ss
function sendAllDataToServer(finalDataArray) {
    // Added on 7Feb2025 to store Indexdb table count
    sendIndexDbEntryCountToServer(finalDataArray);

    RID = OffDecrypt(getParameterByName('RID'));

    const requestData = {
        data: finalDataArray,
        UserId: OffDecrypt(UID),
        ClientID: ClientID,
        Siteid: SiteLocID,
        UserRole: OffDecrypt(OfflineUserRole),
        RID: RID
    };
    var formData = new FormData();
    formData.append("jsonData", JSON.stringify(requestData));

    const jsonData = JSON.stringify(requestData);

    const byteSize = new TextEncoder().encode(jsonData).length;
    $.ajax({
        url: "/OfflineRegister/UploadToCloud",
        type: "POST",
        contentType: "application/json",
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            if (response.success) {
                CustomAlert({ Result: "Success", Message: response.message });

                clearAllTables(function () {
                    CustomAlert({
                        Result: "Success", Message: "Data has been updated on cloud and has been cleared from the local device."
                    });

                    setTimeout(function () {
                        window.location.href = '/Register/Register?UID=' + response.UID + '&UserRole=' + response.UserRole + '&id=' + response.id + '&RID=' + response.RID;
                    }, 3000);

                    $("#customLoader").css("display", "none");
                });
            } else {
                console.error("Error from server:", response.message);
                CustomAlert({ Result: "Failed", Message: response.message });
                $("#customLoader").css("display", "none");
            }
        },
        error: function (xhr, status, error) {
            console.error("Failed to upload all data:", error);
            console.error("XHR Response:", xhr);
            console.error("Status:", status);

            const errorDetails = {
                error: error,
                status: status,
                xhrResponseText: xhr.responseText,
                responseSizeInBytes: byteSize,
                requestData: requestData,
                jsonString: jsonData,
                timestamp: new Date().toISOString()
            };

            const fileContent = JSON.stringify(errorDetails, null, 2);
            const blob = new Blob([fileContent], { type: 'text/plain' });
            const link = document.createElement('a');
            link.href = URL.createObjectURL(blob);
            link.download = 'error-details.txt';
            link.click();
            alert('Failed to upload all data: ' + error + '\nResponse: ' + xhr.responseText);
            $("#customLoader").css("display", "none");
        }
    });
}
//end  17 feb 

//Added on 7Feb2025 
function sendIndexDbEntryCountToServer(finalDataArray) {

    let finalTableCountsArray = [];
    let pipelineZeroCount = 0;
    let pipelineNonZeroCount = 0;

    finalDataArray.forEach(table => {
        finalTableCountsArray.push({
            TableName: table.TableName,
            EntryCount: table.Data.length
        });

        if (table.TableName === "tblPipelineRequest") {
            table.Data.forEach(record => {
                if (record.REGISTERId === 0) {
                    pipelineZeroCount++;
                } else {
                    pipelineNonZeroCount++;
                }
            });
        }
    });

    var upload = "1";

    $.ajax({
        url: "/OfflineRegister/SaveIndexedDBData",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({
            Tables: finalTableCountsArray,
            UserId: OffDecrypt(UID),
            ZeroRegisterCount: pipelineZeroCount,
            NonZeroRegisterCount: pipelineNonZeroCount,
            RID: RID,
            upload: upload
        }),
        success: function (response) {
            console.log("Data saved successfully:", response);
        },
        error: function (xhr, status, error) {
            console.error("Failed to upload all data:", error);
            console.error("XHR Response:", xhr);
            console.error("Status:", status);
            alert('Error sending data: ' + error + '\nResponse: ' + xhr.responseText);
            $("#customLoader").css("display", "none");
        }
    });
}
//function sendAllDataToServer(finalDataArray) {
//    RID = OffDecrypt(getParameterByName('RID'));
//    $.ajax({
//        url: "/OfflineRegister/UploadToCloud",
//        type: "POST",
//        contentType: "application/json",
//        data: JSON.stringify({
//            data: finalDataArray,
//            UserId: OffDecrypt(UID),
//            ClientID: ClientID,
//            //Siteid: SiteLocId
//            Siteid: SiteLocID,
//            UserRole: OffDecrypt(OfflineUserRole),
//            RID: RID
//        }),
//        async: false,
//        success: function (response) {
//            if (response.success) {
//                CustomAlert({ Result: "Success", Message: response.message });

//                clearAllTables(function () {
//                    CustomAlert({
//                        Result: "Success", Message: "Data has been updated on cloud and has been cleared from the local device."
//                    });
//                    setTimeout(function () {
//                        window.location.href = '/Register/Register?UID=' + response.UID + '&UserRole=' + response.UserRole + '&id=' + response.id + '&RID=' + response.RID;
//                    }, 2000);
//                });


//            } else {
//                console.error("Error from server:", response.message);
//                CustomAlert({ Result: "Failed", Message: response.message });

//            }
//        },
//        error: function (xhr, status, error) {
//            console.error("Failed to upload all data:", error);
//        }
//    });
//}


function uploadDataToServerbtn() {
    $('#Uploadtocloudmodel').show();
}


function btnYes() {
    $('#Uploadtocloudmodel').hide();
    uploadDataToServer();
}

function btnNo() {
    $('#Uploadtocloudmodel').hide();

}
//upload to cloude module End

// clear data  table start
function clearAllTables(callback) {

    const stores = [
        "tblSiteList", "tblDutyRating", "tblMaterialType", "tblCheckList", "tblDesignList",
        "tblLocationList", "tblBuildingList", "tblAssetList", "tblRegisterDetails", "tblSupervisor",
        "tblScaffoldOwner", "tblSubOwnerRep", "tblFloorLevel", "tblWorkGroups", "tblSubGroups", "tblInspectionDetails", "tblUserList", "tblcheckListContentList", "tblInspectionListDetails", "tblRegisterGallery", "tblClient", "tblPipelineRequest", "tblPipelineDetails", "tblRequestedBy"
    ];

    const request = indexedDB.open(dbName, dbVersion);

    request.onsuccess = event => {
        const db = event.target.result;
        let clearedCount = 0;

        stores.forEach(store => {
            if (db.objectStoreNames.contains(store)) {
                const transaction = db.transaction(store, "readwrite");
                const objectStore = transaction.objectStore(store);

                objectStore.clear().onsuccess = () => {
                    clearedCount++;
                    console.log(`${store} cleared.`);
                    if (clearedCount === stores.length) {
                        callback(true);
                    }
                };

                objectStore.clear().onerror = () => {
                    console.error(`Failed to clear ${store}.`);
                    callback(false);
                };
            } else {
                console.log(`${store} does not exist.`);
            }
        });
    };

    request.onerror = error => {
        console.error("Error opening IndexedDB:", error);
        callback(false);
    };
}

//clear  data  table end
function showConfirmationModal(options) {
    const $modal = $('#ConfirmationModal');

    // Set modal title
    $modal.find('#modalTitle').html(options.title ||
        '<i class="fa fa-exclamation-circle w3-text-yellow"></i>&nbsp;Alert');

    // Set modal message (using .html to allow HTML content)
    $modal.find('#modalMessage').html(options.message || 'Are you sure?');

    // Set actions for Yes button
    $modal.find('#btnYes').off('click').on('click', function () {
        if (typeof options.onYes === 'function') {
            options.onYes(); // Execute the provided Yes action
        }
        closeModal();
    });

    // Set actions for No button
    $modal.find('#btnNo').off('click').on('click', function () {
        if (typeof options.onNo === 'function') {
            options.onNo(); // Execute the provided No action
        }
        closeModal();
    });

    // Show the modal
    $modal.show();
}

function closeModal() {
    $('#ConfirmationModal').hide();
}

function clearCache() {
    caches.keys().then(function (cacheNames) {
        return Promise.all(
            cacheNames.map(function (cacheName) {
                if (cacheName.startsWith('my-OfflineJobRegister-cache-')) {
                    console.log('Deleting cache:', cacheName);
                    return caches.delete(cacheName);
                }
            })
        );
    }).then(() => {
        console.log('Cache cleared successfully');
    }).catch((error) => {
        console.error('Error clearing cache:', error);
    });
}
function isValidDateFormate(dateStr) {
    const regex = /^(0[1-9]|[12]\d|3[01])-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)-\d{4}$/;
    return regex.test(dateStr);
}



function checkimagegelleryregister(RID) {
    const maxImages = 2;

    const request = indexedDB.open(dbName);

    request.onsuccess = function (event) {
        const db = event.target.result;
        const txn = db.transaction("tblRegisterGallery", "readonly");
        const registerStore = txn.objectStore("tblRegisterGallery");
        const getAllRequest = registerStore.getAll();

        getAllRequest.onsuccess = function () {
            const allRecords = getAllRequest.result || [];
            const matchingRecords = allRecords.filter(
                (record) => record.RegisterId === parseInt(RID, 10)
            );

            if (matchingRecords.length < maxImages) {
                $("#divCreatePop").css("display", "block");
            } else {
                CustomAlert({ Result: "Failed", Message: "Maximum of two images can be saved!" });
            }
        };

        getAllRequest.onerror = function () {
            CustomAlert({ Result: "Failed", Message: "Error fetching records!" });
        };
    };

    request.onerror = function () {
        CustomAlert({ Result: "Failed", Message: "Database error occurred!" });
    };
}
