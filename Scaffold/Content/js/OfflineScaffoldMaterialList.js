


var GetCount = 0;
var GetclientId; var dbName = "scaffpac Job Register";
var dbVersion = 65;
var db;
var dropdownCheck = false;
//Getting Parameters From the Url
var UID = getParameterByName('UID');
//var InspID = getParameterByName('InspID');
//var CheckListID = getParameterByName('CheckListID');
var CID = getParameterByName('id');
var ClientID = getParameterByName('id');
// var ClientID = getParameterByName('Cid'); 
ClientID = OffDecrypt(ClientID)
var RID = getParameterByName('RID');
RID = OffDecrypt(RID);
var GetOfflineStatus = getParameterByName('offlinestatus');
var OfflineStatus = window.atob(GetOfflineStatus);
//var OfflineStatus = getParameterByName('offlinestatus');

$(document).ready(function () {
    getInventoryList(RID,null);
    populateDropdown('tblInventoryDetails', '#ddlMaterialType', 'MaterialType', 'id', RID);
});
function MaterialChange() {
    var materialType = document.getElementById('ddlMaterialType').value;
    console.log("Selected Material Type:", materialType);
    var RID = getParameterByName('RID');
    RID = OffDecrypt(RID);
    getInventoryList(RID, materialType);
    // Optionally, you can call getInventoryRequest() or other functions here
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
        var index = store.index("REGISTERId");
        var keyRange = IDBKeyRange.only(parseInt(REGISTERId, 10)); // Filter records by REGISTERId
       // var getRequest = index.openCursor(keyRange);

        // const request = index.getAll(parseInt(REGISTERId, 10));
        var request = index.getAll(keyRange);

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
        var transaction = db.transaction(["tblInventoryDetails"], "readonly");
        var objectStore = transaction.objectStore("tblInventoryDetails");
        var index = objectStore.index("REGISTERId");
        var keyRange = IDBKeyRange.only(parseInt(REGISTERId, 10)); // Filter records by REGISTERId
        var getRequest = index.openCursor(keyRange);
        //var getRequest = objectStore.openCursor();
        var result = [];
        getRequest.onerror = function (event) {
            console.error("Error retrieving records:", event.target.error);
        };

        getRequest.onsuccess = function (event) {
            var cursor = event.target.result;
            if (cursor) {
                var record = cursor.value;

                // Apply optional filters like SiteLocId if provided
                if (record.REGISTERId === 0 || record.REGISTERId === parseInt(REGISTERId, 10)) {
                    if (!materialType || record.MaterialType === materialType) {
                        // Push the record to the result array if all conditions are met
                        result.push(record);
                    }
                }

                // Move the cursor to the next entry
                cursor.continue();
            } else {
                // Once the cursor has iterated through all entries
                if (result.length === 0) {
                    console.log("No records found.");
                } else {
                    console.log("Records found:", result);
                    displayInventoryDetails(result); // Call a function to process and bind the data
                }
            }
        };



        request.onupgradeneeded = function (event) {
            console.log("Database upgrade needed.");
        };
    }
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



        str += "<td class='setWidth6'>" + MaterialType + "</td>";
        str += "<td class='setWidth7'>" + ItemCode + "</td>";
        str += "<td class='setWidth8'>" + ItemDesc + "</td>";

        str += "<td class='setWidth14'style='text-align: center;'>" + Quantity + "</td>";
        str += "<td class='setWidth15'>" + ItemWeight + "</td>";
        str += "<td class='setWidth16'>" + TotalWeight + "</td>";

        str += "</tr>";
    }

    bindDataToTable(str); // Ensure this function is correctly implemented

}

function bindDataToTable(str) {
    if ($.fn.DataTable.isDataTable('#tblRegisterInventoryList')) {
        $('#tblRegisterInventoryList').DataTable().clear().destroy();
    }

    $('#tbodyRegisterInventoryList').html(str);  // Assuming 'str' contains the table rows




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


}
// Function to process and display the fetched record data

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
    window.location.href = 'OfflineSiteRequest.html?UID=' + UID + '&UserRole=' + UserRole + '&id=' + ClientID + '&FromId=' + RID + '&ToId=' + Siteid + '&from=' + from + '&To=' + To + '&fromtocheck=' + fromtocheck + '&page=' + page;


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
