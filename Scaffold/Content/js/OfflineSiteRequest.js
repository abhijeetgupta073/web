
var dbName = "scaffpac Job Register";
var dbVersion = 70;
//var InspID = getParameterByName('InspID');
//var CheckListID = getParameterByName('CheckListID');
var CID = getParameterByName('id');
//var ClientID = getParameterByName('id');  
// var ClientID = getParameterByName('Cid'); 
var RID = getParameterByName('FromId');
RID = OffDecrypt(RID);
$(document).ready(function () {
    ChkPipelineExist(RID);
   /* $("#lblVersion").text("Version 1.0");*/
});
function ChkPipelineExist(REGISTERId) {
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
        var transaction = db.transaction(["tblPipelineDetails"], "readonly");
        var objectStore = transaction.objectStore("tblPipelineDetails");

        // Use an index or cursor to find the record matching REGISTERId
        var index = objectStore.index("REGISTERId"); // Assuming there's an index on REGISTERId
        var query = index.getAll(parseInt(REGISTERId, 10));

        query.onsuccess = function (event) {
            var records = event.target.result;

            //if (records && records.length >= 1) {
            // //   alert("Please Clear the Existing Pipeline to Request More Items.");
            //    Res = { Result: 'Warning', Message: "Please Clear the Existing Pipeline to Request More Items." };
            //    CustomAlert(Res);
            //} else {
                displayDetails(REGISTERId);
                displayComment(REGISTERId);
                populateDropdown('tblPipelineRequest', '#ddlMaterialType', 'MaterialType', 'id', REGISTERId);
                //ShowInventoryRequest(CID, RID);
                getInventoryRequest(REGISTERId, null);
           // }
        };

        query.onerror = function (event) {
            console.error("Error fetching records:", event);
        };
    };
}

function displayDetails(REGISTERId) {
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
        var request = objectStore.get(Number(REGISTERId)); 

        request.onsuccess = function (event) {
            const data = event.target.result;
            if (data) {
                if (data.PipelineStartDate === null || data.PipelineStartDate === "") {
                    // alert("Info! Please Note The Pipeline Start Date Has Not Been Filled.");
                    Res = { Result: 'Info', Message: "Please Note The Pipeline Start Date Has Not Been Filled." };
                    CustomAlert(Res);
                }

                document.getElementById("txtto").value = data.SiteLocation;
                var concatenatedValue = `${data.ShowRegisterID} - ${data.Description}`;
                if (data.Description === null || data.Description === "") {
                    concatenatedValue = data.ShowRegisterID;
                }
                document.getElementById("txtfrom").value = concatenatedValue;
                const today = new Date();

                // Get the day, month, and year
                const day = today.getDate().toString().padStart(2, '0'); // Ensure 2-digit day
                const monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
                const month = monthNames[today.getMonth()]; // Get month abbreviation
                const year = today.getFullYear();

                // Format as DD-MMM-YYYY
                const formattedDate = `${day}-${month}-${year}`;
                document.getElementById("txtrequesteddate").value = formattedDate;
                document.getElementById("txtrequestedby").value = data.RequestedBy;
            } else {
                console.error("No record found with ID:", id);
            }
        };

        request.onerror = function (event) {
            console.error("Failed to retrieve data:", event.target.errorCode);
        };
    }
}

function displayComment(REGISTERId) {
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
        var transaction = db.transaction(["tblPipelineRequest"], "readonly");
        var objectStore = transaction.objectStore("tblPipelineRequest");

        var getRequest = objectStore.openCursor();
        let maxInvRequestId = null; // Track the maximum InvRequestId
        let maxDesc = ""; // Track the corresponding description

        getRequest.onerror = function (event) {
            console.error("Error retrieving records:", event.target.error);
        };

        getRequest.onsuccess = function (event) {
            var cursor = event.target.result;
            if (cursor) {
                var record = cursor.value;

                if (record.REGISTERId === parseInt(REGISTERId, 10) && record.PipeLineDesc) {
                    if (maxInvRequestId === null || record.InvRequestId > maxInvRequestId) {
                        maxInvRequestId = record.InvRequestId;
                        maxDesc = record.PipeLineDesc;
                    }
                }

                cursor.continue();
            } else {
                // Cursor completed; update the field with the max description
                if (maxDesc) {
                    document.getElementById("txtdesc").value = maxDesc;
                } else {
                    document.getElementById("txtdesc").value = ""; // Clear if no match is found
                }
                console.log("Cursor completed.");
            }
        };
    };
}




function populateDropdown(storeName, dropdownSelector, valueField, textField, REGISTERId) {
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
        var request = store.getAll();

        request.onsuccess = function (event) {
            var data = event.target.result;
            data.forEach(function (item) {
                distinctValues.add(item[valueField]);
            });

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
//function ShowInventoryRequest(CID, RID) {
//    var UID = getParameterByName('UID');
//    var Siteid = getParameterByName('Siteid');

//    $.ajax({
//        type: "POST",
//        url: "/OfflineRegister/ShowInventoryRequest", // Adjust URL if needed
//        data: JSON.stringify({ id: CID, RID: RID }),
//        contentType: "application/json; charset=utf-8",
//        dataType: "json", // Expecting a JSON response
//        async: false, // It's better to use async: true in most cases
//        success: function (response) {
//            console.log(response); // Inspect the response

//            if (response && response.length > 0) {
//                // If the response is valid, pass the data to IndexedDB
//                insertIntoIndexedDB('tblPipelineRequest', response);
//            } else {
//                console.log('No data received from server.');
//            }
//        },
//        error: function (xhr, status, error) {
//            console.log("AJAX Error: ", status, error);
//        }
//    });
//}

function MaterialChange() {
    var materialType = document.getElementById('ddlMaterialType').value;
    console.log("Selected Material Type:", materialType);
    var RID = getParameterByName('FromId');
    RID = OffDecrypt(RID);
    getInventoryRequest(RID, materialType);
    // Optionally, you can call getInventoryRequest() or other functions here
}


function getInventoryRequest(REGISTERId, materialType) {

    $("#customLoader").css("display", "block");
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

                if ((record.REGISTERId === parseInt(REGISTERId, 10) || record.REGISTERId  === 0) && (!materialType || record.MaterialType === materialType)) {
                    if (!addedRecords.has(inventoryId)) {
                        // First record for this InventoryId, add it to the map
                        addedRecords.set(inventoryId, record);
                    } else {
                        let existingRecord = addedRecords.get(inventoryId);

                        // Check if the current record is better (registerid !== 0)
                        if (record.REGISTERId === parseInt(REGISTERId, 10) || existingRecord.registerid === 0) {
                            addedRecords.set(inventoryId, record);
                        }
                    }
                }
                cursor.continue();
            } else {
              
                    // Convert Map values to an array
                    let recordsArray = Array.from(addedRecords.values());

                    // Sort the array based on InvRequestId and then MaterialType
                    recordsArray.sort((a, b) => {
                        if (a.Quantity === 0 && b.Quantity !== 0) return 1; // Move zero quantities to the bottom
                        if (a.Quantity !== 0 && b.Quantity === 0) return -1; // Keep non-zero quantities at the top


                        // If InvRequestId is equal, sort by MaterialType (alphabetical order ascending)
                        if (a.MaterialType < b.MaterialType) return -1;
                        if (a.MaterialType > b.MaterialType) return 1;
                        return 0;
                    });

                    console.log("Records found:", addedRecords);
                    // Call a function to process and bind the data, passing the unique records
                displayInventoryDetails(recordsArray);
               // }

                $("#customLoader").css("display", "none");
            }
        };

        request.onupgradeneeded = function (event) {
            console.log("Database upgrade needed.");
        };
    };

}





function displayInventoryDetails(data) {

    str = ""; // Start with an empty string
    str += '<table id="dataforgrid" class="table table-striped" cellspacing="0" width="100%" style="background-color: #ffffff; color: #000000;">'; // Ensure table has white background and black text
    str += "<thead style='background-color: #efefef; color: #716262;'>"; // Header styling
    str += "<tr>";
    str += "<th style='width:10%;text-align:center;display:none'>Inventory Id</th>";
    str += "<th style='width:15%;text-align:left'>Material Type</th>";
    str += "<th style='width:15%;text-align:left'>Item Code</th>";
    str += "<th style='width:30%;text-align:left'>Item Description</th>";
    str += "<th style='display:none;'>Available Quantity</th>";
    str += "<th style='display:none;'>Item Weight</th>";
    str += "<th class='no-sort' style='width:10%;text-align:center'>Quantity</th>";
    str += "<th style='width:3%;text-align:center'>#</th>";
    str += "</tr>";
    str += "</thead>";
    str += "<tbody style='background-color: #ffffff; color: #000000;'>"; // Row styling


    // Loop to populate rows dynamically
    for (let i = 0; i < data.length; i++) {
        let row = data[i]; // Assuming data is an array of objects
        str += "<tr class='tableRow'> ";
        str += "<td style='width:10%;text-align:center;display:none' id='lblinventoryid'>" + row.InventoryId + "</td>";
        str += "<td style='width:15%;text-align:left' name='MateralType' id='lblMaterialType'>" + row.MaterialType + "</td>";
        str += "<td style='width:15%;text-align:left' name='ItemCode' id='lblItemCode'>" + row.ItemCode + "</td>";
        str += "<td style='width:30%;text-align:left' name='ItemDesc' id='lblItemDesc'>" + row.ItemDesc + "</td>";
        str += "<td style='display: none;' id='lblavailqty'>" + numberWithCommas(row.AvailableQuantity) +
            "<input id='hdnAvalid_" + i + "' type='hidden' value=" + numberWithCommas(row.AvailableQuantity) + " /></td>";
        str += "<td style='display: none;' id='lblItemWeight'>" + row.ItemWeight + "</td>";
        if ($("#hdntextdisable").val() == "true") {
            str += "<td style='width:10%;text-align:center;background-color:LightCyan;'> <input id='txtqty_" + i + "' type='text' value=" + Quantity + " class='EnterQuantity' readonly='true'  max='2147483647' min='0'  onKeyDown='handleMyInput(event," + i + ")'/><input id='hdnid_" + i + "' type='hidden' value='0' /></td>";

            /*str += "<td style='width:3%;'><a id='rowSave_" + i + "' style='pointer-events: none; cursor: default;' class='fa fa-save rowSaveButton' onclick='Back_Click(" + i +")'></a><input id='OldQtyval_" + i + "' type='hidden' value=" + Quantity + " /></td></tr>";*/
            // Replace Font Awesome icon with PNG image
            str += "<td style='width:3%;'><a id='rowSave_" + i + "' style='pointer-events: none; cursor: default;'>" +
                "<img src='/images/fonts/FaFaSave.png' alt='Save' style='width: 20px; height: 20px;' onclick='Back_Click(" + i + ")'/></a>" +
                "<input id='OldQtyval_" + i + "' type='hidden' value=" + Quantity + " /></td></tr>";
        }
        else {
            str += "<td style='width:10%;text-align:center;background-color:LightCyan;'>" +
                "<input id='txtqty_" + i + "' type='text' autocomplete='off' value=" + parseInt(row.Quantity, 10) +
                " class='EnterQuantity'   max='2147483647' min='0'  onKeyDown='handleMyInput(event," + i + ")'/>" +
                "<input id='hdnid_" + i + "' type='hidden' value='0' /></td>";
            //str += "<td style='width:3%;'><a id='rowSave_" + i + "' class='fa fa-save rowSaveButton' style='color: black;' onclick='Back_Click(" + i +")'></a>" +
            //    "<input id='OldQtyval_" + i + "' type='hidden' value=" + parseInt(row.Quantity, 10) + " /></td>";
            // Replace Font Awesome icon with PNG image
            str += "<td style='width:3%;'><a id='rowSave_" + i + "'>" +
                "<img src='/images/fonts/FaFaSave.png' alt='Save' style='width: 20px; height: 20px;' onclick='Back_Click(" + i + ")'/></a>" +
                "<input id='OldQtyval_" + i + "' type='hidden' value=" + parseInt(row.Quantity, 10) + " /></td>";

        }
        str += "</tr>";
    }

    str += "</tbody>";
    str += "</table>";

    // Call the function to bind data

    bindDataToTable(str);

}
function handleMyInput(event, i) {
    Back_Click(i);
    if (event.key === "Enter") {
        event.preventDefault(); // Prevent the default Enter behavior (like form submission)

        // Get the current input element
        let currentInput = event.target;

        // Extract the row number from the current input's ID (e.g., "txtqty_1" -> 1)
        let currentRowIndex = parseInt(currentInput.id.split('_')[1], 10);

        // Get the next row's input by constructing its ID
        let nextInput = document.getElementById('txtqty_' + (currentRowIndex + 1));

        // If the next input exists, focus on it
        if (nextInput) {
            nextInput.select();
        }
    }
}

function bindDataToTable(str) {
    if ($.fn.DataTable.isDataTable('#dataforgrid')) {
        $('#dataforgrid').DataTable().clear().destroy();
    }

    $('#loaddata').html(str);  // Assuming 'str' contains the table rows



    table = $('#dataforgrid').DataTable({
        "bSort": false,
        "processing": true,
        searching: true,
        info: false,
        "pageLength": 50,
        scrollX: true,
        scrollY: "60vh",
        "bPaginate": true,
    });


}
function btnClearAll_Click() {
    $("#divClearAllData").css("display", "block");
}

function btnClearcancel_Click() {
    $("#divClearAllData").css("display", "none");

}
function btnClearAllPipeLineData_Click() {
    $("#customLoader").css("display", "block");
    let dbRequest = indexedDB.open(dbName, dbVersion);
    var RID = getParameterByName('FromId');
    RID = parseInt(OffDecrypt(RID));

    dbRequest.onsuccess = function (event) {
        let db = event.target.result;
        let transaction = db.transaction("tblPipelineRequest", "readwrite");
        let store = transaction.objectStore("tblPipelineRequest");

        let table = $('#dataforgrid').DataTable();
        let rows = table.rows().nodes();
        totalRows = rows.length;

        Array.from(rows).forEach(row => {
            let rowId = parseInt(row.querySelector("#lblinventoryid").textContent.trim(), 10); // Get InventoryId
            let qty = row.querySelector("input[type='text']").value.trim(); // Get quantity
            let rowMaterial = row.querySelector("#lblMaterialType").textContent.trim();
            let rowCode = row.querySelector("#lblItemCode").textContent.trim();
            let rowIdDesc = row.querySelector("#lblItemDesc").textContent.trim();
            let rowavailqty = row.querySelector("#lblavailqty").textContent.trim();

            // Open a cursor to find the record
            var getRequest = store.openCursor();

            getRequest.onsuccess = function (event) {
                let cursor = event.target.result;

                if (cursor) {
                    let record = cursor.value;

                    // Check if record matches current REGISTERId (RID) and other conditions
                    if (record.REGISTERId === RID) {
                        // Delete the record
                        let deleteRequest = store.delete(cursor.key);

                        deleteRequest.onsuccess = function () {
                            console.log(`Record for InventoryId ${rowId} deleted successfully.`);
                        };

                        deleteRequest.onerror = function (error) {
                            console.error("Error deleting record:", error);
                        };
                    }

                    cursor.continue();
                } else {
                    // No more records to process
                    $("#customLoader").css("display", "none");
                    location.reload();
                }
            };

            getRequest.onerror = function (error) {
                console.error("Error retrieving record:", error);
            };
        });
    };

    dbRequest.onerror = function (error) {
        console.error("Error opening IndexedDB:", error);
    };
}



function Back_Click(i) {
    var currrow = i;
    $("#customLoader").css("display", "block");
    let dbRequest = indexedDB.open(dbName, dbVersion);
    var RID = getParameterByName('FromId');
    RID = parseInt(OffDecrypt(RID), 10);

    dbRequest.onsuccess = function (event) {
        let db = event.target.result;
        let transaction = db.transaction("tblPipelineRequest", "readwrite");
        let store = transaction.objectStore("tblPipelineRequest");

        let table = $('#dataforgrid').DataTable();
        let rows = table.rows().nodes();
        totalRows = rows.length;

    

        let updatedRow = rows[i];
        let rowId = parseInt(updatedRow.querySelector("#lblinventoryid").textContent.trim(), 10);
        let qty = updatedRow.querySelector("input[type='text']").value.trim();
        let rowMaterial = updatedRow.querySelector("#lblMaterialType").textContent.trim();
        let rowCode = updatedRow.querySelector("#lblItemCode").textContent.trim();
        let rowIdDesc = updatedRow.querySelector("#lblItemDesc").textContent.trim();
        let rowavailqty = updatedRow.querySelector("#lblavailqty").textContent.trim();
        let rowitemweight = updatedRow.querySelector("#lblItemWeight").textContent.trim();

        let desc = document.getElementById("txtdesc").value;
        if (desc === "") {
            // alert("Alert! Please fill the Description");
            Res = { Result: 'Failed', Message: "Please fill the Description" };
            CustomAlert(Res);
            updatedRow.querySelector("input[type='text']").value = 0;
            $("#customLoader").css("display", "none");
            return;
        }
        if (parseInt(qty,10) > 2147483647) {
            // alert("Alert! Please fill the Description");
            Res = { Result: 'Failed', Message: "Max value Allowed is: 2147483647" };
            CustomAlert(Res);
            updatedRow.querySelector("input[type='text']").value = 0;
            $("#customLoader").css("display", "none");
            return;
        }
        if (isNaN(qty) || !Number.isInteger(parseFloat(qty)) || qty.includes(".")) {
            // Highlight the text box in red
            const inputBox = updatedRow.querySelector("input[type='text']");
            inputBox.style.border = "2px solid red";

            // Reset loader and return
            $("#customLoader").css("display", "none");
            return;
        } 
        var index = store.index("InventoryId");
        var keyRange = IDBKeyRange.only(rowId);
        var getRequest = index.openCursor(keyRange);

        getRequest.onsuccess = function (event) {
            let cursor = event.target.result;

            if (!cursor) {
                // No records found, add new record
                    if (parseInt(qty, 10) !== 0) {
                addNewRecord();
                    }
                    else {

                        $("#customLoader").css("display", "none");
                    }

                return;
            }

            let record = cursor.value;

            // Using switch case for different record scenarios
            switch (true) {
                case (record.InventoryId === rowId && record.REGISTERId === RID):
                    const inputBox = updatedRow.querySelector("input[type='text']");
                    inputBox.style.border = "1px solid black";
                    // Update existing record
                    if (parseInt(qty, 10) !== 0) {
                    updateExistingRecord(record);
                    }
                    else {
                        DeleteBlankQuantity(record);
                    }
                    break;

                case (record.InventoryId === rowId && record.REGISTERId === 0):
                    // Handle case where REGISTERId is 0
                    console.log("Record found with REGISTERId 0");
                    cursor.continue();
                    break;

                default:
                    console.log("No update required");
                    cursor.continue();
            }
        };

        function DeleteBlankQuantity(record1) {
            // Open a cursor to find the record
            var getRequest = store.openCursor();

            getRequest.onsuccess = function (event) {
                let cursor = event.target.result;

                if (cursor) {
                    let record = cursor.value;

                    // Check if record matches current REGISTERId (RID) and other conditions
                    if (record.REGISTERId === record1.REGISTERId && record.InventoryId === record1.InventoryId) {
                        // Delete the record
                        let deleteRequest = store.delete(cursor.key);

                        deleteRequest.onsuccess = function () {
                            console.log(`Record for InventoryId ${rowId} deleted successfully.`);
                        };

                        deleteRequest.onerror = function (error) {
                            console.error("Error deleting record:", error);
                        };
                    }

                    cursor.continue();
                } else {
                    // No more records to process
                    $("#customLoader").css("display", "none");
                    location.reload();
                }
            };

            getRequest.onerror = function (error) {
                console.error("Error retrieving record:", error);
            };
        }

        function updateExistingRecord(record) {
            record.PipeLineDesc = desc;
            record.AvailableQuantity = parseInt(rowavailqty, 10) - parseInt(qty, 10);
            record.TotalWeight = rowitemweight * parseInt(qty, 10);
            record.Quantity = qty;

            const updateRequest = store.put(record);

            updateRequest.onsuccess = function () {
                console.log("Item successfully updated");
                $("#customLoader").css("display", "none");
            };

            updateRequest.onerror = function (error) {
                console.error("Error updating Item:", error);
                $("#customLoader").css("display", "none");
            };
        }

        function addNewRecord() {
            var addRequest="";
            const newRecord = {
                InventoryId: rowId,
                PipeLineDesc: desc,
                ItemCode: rowCode,
                MaterialType: rowMaterial,
                ItemDesc: rowIdDesc,
                AvailableQuantity: parseInt(rowavailqty, 10) - parseInt(qty, 10),
                REGISTERId: RID,
                Quantity: qty,
                ItemWeight: rowitemweight,
                TotalWeight: rowitemweight * parseInt(qty, 10),
                insertedOffline: true
            };
                 addRequest = store.add(newRecord);


            addRequest.onsuccess = function () {
                $("#customLoader").css("display", "none");
                console.log("Item successfully added:", newRecord);
            };

            addRequest.onerror = function (error) {
                console.error("Error adding Item:", error);
                $("#customLoader").css("display", "none");
            };
        }

        getRequest.onerror = function (error) {
            console.error('Error querying the database:', error);
            $("#customLoader").css("display", "none");
        };
    };

    dbRequest.onerror = function (error) {
        console.error("Error opening IndexedDB:", error);
        $("#customLoader").css("display", "none");
    };

    if (i === undefined) {
        Back_to_List();
    }
}
function btnrefresh_Click() {
    location.reload();
}
function Back_to_List() {
    var ClientID = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var RID = getParameterByName('FromId');

    var Siteid = getParameterByName('ToId');
    redirect_click(UID, UserRole, ClientID, Siteid, RID);
}

function redirect_click(UID, UserRole, ClientID, Siteid, RID) {
    var regid = getParameterByName('FromId');
    var OfflineUserRole = getParameterByName('OfflineUserRole');
    /*    window.location.href = 'OfflineScaffoldMaterialList.html?UID=' + UID + '&UserRole=' + UserRole + '&id=' + ClientID + '&SiteLocId=' + Siteid + '&RID=' + regid + '';*/
   /* window.location.href = 'OfflineRegister.html?UID=' + UID + '&UserRole=' + UserRole + '&id=' + ClientID + '&SiteLocId=' + Siteid + '&RID=' + regid + '';*/

    window.location.href = 'OfflineRegister.html?UID=' + encodeURIComponent(UID) + '&UserRole=' + encodeURIComponent(UserRole) + '&OfflineUserRole=' + encodeURIComponent(OfflineUserRole) + '&id=' + encodeURIComponent(ClientID) + '&SiteLocId=' + encodeURIComponent(Siteid) + '&RID=' + encodeURIComponent(regid) + '&activateTab=Inventory';

}
function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}
//function insertIntoIndexedDB(storeName, data) {
//    var dbPromise = indexedDB.open(dbName, dbVersion);

//    dbPromise.onsuccess = function (event) {
//        db = event.target.result;

//        // Check if the object store exists
//        if (!db.objectStoreNames.contains(storeName)) {
//            console.error("Error: Object store '" + storeName + "' not found in the database.");
//            return; // Exit the function if the store is not found
//        }

//        // Start a transaction with the specified store
//        var transaction = db.transaction(storeName, 'readwrite');
//        var store = transaction.objectStore(storeName);

//        // Handle transaction complete and errors
//        transaction.oncomplete = function () {
//            console.log("All data inserted successfully into: " + storeName);
//        };

//        transaction.onerror = function (event) {
//            console.error("Transaction error:", event.target.error);
//        };

//        // Insert each item into the store
//        data.forEach(function (item) {
//            var request = store.put(item);

//            request.onsuccess = function () {
//                //    console.log("Item inserted successfully:", item);
//            };

//            //request.onerror = function (event) {
//            //    console.error("Failed to insert item:", item, "Error:", event.target.error);
//            //};
//            request.onerror = function (event) {
//                console.error("Failed to insert item:", item, "Error:", event.target.error);

//                // Log the key path and the item to identify the missing key
//                console.error("Key path issue with item:", item);
//            };
//        });
//    };

//    dbPromise.onerror = function (event) {
//        console.error("Failed to open database:", event.target.errorCode);
//    };
//}
function numberWithCommas(x) {
    var parts = x.toString().split(".");
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join(".");
}