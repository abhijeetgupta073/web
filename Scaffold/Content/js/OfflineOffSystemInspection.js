
const dbVersion = 45; // Increment the version number for upgrade
const dbName = 'scaffpac';
var db;


function setupDatabase() {
    showLoadingMessage();

    return new Promise((resolve, reject) => {
        const request = indexedDB.open(dbName, dbVersion);
        request.onupgradeneeded = (event) => {
            db = event.target.result;
            CreateTableForIndexDb(db);
        };
        request.onsuccess = (event) => {
            db = event.target.result;
            resolve(db);
            hideLoadingMessage();
            //populateUserDropdown();

        };
        request.onerror = reject;
        hideLoadingMessage();
    });
}


$(document).ready(function () {
    initializeApp();
});
async function initializeApp() {
    showLoadingMessage();

    var LoginUserId = getParameterByName("UserId");
    var clientId = getParameterByName("ClientId");
    var Siteid = getParameterByName("Siteid");

    let DecptClientid;
    let DecptSiteid;

    if (navigator.onLine) {

        var decryptedData = await fetchDecryptedClientIDs([clientId, Siteid]);

        DecptClientid = parseInt(decryptedData[clientId], 10);
        DecptSiteid = parseInt(decryptedData[Siteid], 10);

        localStorage.setItem("DecptClientid", DecptClientid);
        localStorage.setItem("DecptSiteid", DecptSiteid);

    } else {

        DecptClientid = parseInt(localStorage.getItem("DecptClientid"), 10);
        DecptSiteid = parseInt(localStorage.getItem("DecptSiteid"), 10);
    }

    try {
        const response = navigator.onLine
            ? await $.ajax({
                url: '/OfflineOffSystemInspection/CheckOfflineClearedDate',
                type: "POST",
                data: { LoginUserId, clientId, Siteid },
                async: false
            })
            : {
                formattedOfflineon: localStorage.getItem("formattedOfflineon")
            };

        if (navigator.onLine && response.formattedOfflineon) {
            localStorage.setItem("formattedOfflineon", response.formattedOfflineon);
        }

        const OfflineClearedOn = new Date(response.formattedOfflineon);

        const db = await setupDatabase();

        if (db.objectStoreNames.contains('tblRegister')) {
            const transaction = db.transaction(['tblRegister'], 'readonly');
            const objectStore = transaction.objectStore('tblRegister');

            const recordCount = await new Promise((resolve, reject) => {
                const countRequest = objectStore.count();
                countRequest.onsuccess = () => resolve(countRequest.result);
                countRequest.onerror = reject;
            });

            console.log("Record count in tblRegister:", recordCount);

            if (recordCount > 0) {
                console.log("Data found in tblRegister, checking site and client ID...");
                await checkSiteAndClientId(db, OfflineClearedOn, DecptClientid, DecptSiteid);
                handleDataLoading();
            } else {
                console.log("No data in tblRegister, loading from server...");

                if (navigator.onLine) {
                    await loadDataFromServer();
                    await verifyDataLoad();
                    handleDataLoading();
                } else {
                    console.log("Offline and no local data found");
                }
            }
        } else {
            console.log("tblRegister does not exist, loading from server...");

            if (navigator.onLine) {
                await loadDataFromServer();
                await verifyDataLoad();
                handleDataLoading();
            } else {
                console.log("Offline and table not found");
            }
        }
    } catch (error) {
        console.error("Error during initialization:", error);
    } finally {
        hideLoadingMessage();
    }
    // Register service worker
    registerServiceWorker('/service-worker.js?v=23')
}

function registerServiceWorker(scriptPath) {
    if ('serviceWorker' in navigator) {
        navigator.serviceWorker.register(scriptPath)
            .then(registration => {
                console.log('Service Worker registered with scope:', registration.scope);
            })
            .catch(err => {
                console.error('Service Worker registration failed:', err);
            });
    }
}

// Function to verify if data loaded into IndexedDB after calling loadDataFromServer
async function verifyDataLoad() {
    const db = await setupDatabase();
    const transaction = db.transaction(['tblRegister'], 'readonly');
    const objectStore = transaction.objectStore('tblRegister');

    const recordCount = await new Promise((resolve, reject) => {
        const countRequest = objectStore.count();
        countRequest.onsuccess = () => resolve(countRequest.result);
        countRequest.onerror = reject;
    });

    if (recordCount > 0) {
        console.log("Data successfully loaded into tblRegister:", recordCount);
    } else {
        console.warn("Data not loaded into tblRegister as expected.");
    }
}
async function fetchDecryptedClientIDs(ids) {
    try {
        const response = await $.ajax({
            type: "POST",
            url: "/OfflineRegister/GetDecryptClientIDs",
            data: JSON.stringify({ ids }),
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        });
        return response;
    } catch (error) {
        console.error("Error fetching decrypted client IDs:", error);
        throw error;
    }
}

async function checkSiteAndClientId(db, OfflineClearedOn, currentClientId, currentSiteId) {
    const transaction = db.transaction(['tblRegister'], 'readonly');
    const objectStore = transaction.objectStore('tblRegister');

    let matchFound = false;
    let record;

    const cursorRequest = objectStore.openCursor();

    cursorRequest.onsuccess = async function (event) {
        const cursor = event.target.result;
        if (cursor) {
            record = cursor.value;

            if (record.ClientID === currentClientId && record.SiteLocID === currentSiteId) {
                await compareGoOfflineAndClearedDates(db, OfflineClearedOn);
                matchFound = true;
            }
            cursor.continue();
        } else if (!matchFound) {
            const currentSiteLocationName = await getSiteLocationById(currentSiteId);
            alert(`You already have offline data for site ${record.SiteLocation} (localdb site), so you cannot go offline with the new site ${currentSiteLocationName}. Please click on upload to cloud.`);

            const response = {
                Result: "Failed",
                Message: `You already have offline data for site ${record.SiteLocation} (localdb site), so you cannot go offline with the new site ${currentSiteLocationName}. Please click on upload to cloud.`
            };

            $('#tblOffSystemInspectionList input, #tblOffSystemInspectionList select').prop('disabled', true);
            OfflineCustomAlert(response);

            //setTimeout(() => ExportToCSV(), 1000);
        }
    };
}




let isChecked = false;

async function compareGoOfflineAndClearedDates(db, OfflineClearedOn) {
    if (isChecked) return;
    isChecked = true;

    const transaction = db.transaction(['tblRegister'], 'readonly');
    const objectStore = transaction.objectStore('tblRegister');

    let goOfflineOnLatest = null;

    const cursorRequest = objectStore.openCursor();

    cursorRequest.onsuccess = function (event) {
        const cursor = event.target.result;
        if (cursor) {
            const record = cursor.value;

            // Parse GoOfflineOn to Date object
            const goOfflineOn = new Date(record.GoOfflineOn);

            // Update goOfflineOnLatest if goOfflineOn is more recent
            if (!goOfflineOnLatest || goOfflineOn > goOfflineOnLatest) {
                goOfflineOnLatest = goOfflineOn;
            }
            cursor.continue();
        }
    };

    transaction.oncomplete = async function () {
        const offlineClearedOnDate = new Date(OfflineClearedOn);
        debugger
        if (offlineClearedOnDate > goOfflineOnLatest) {
            //alert("This Inspection session has been voided due to time lapse. Online system data has been updated. Your current offline data will be downloaded in Excel and new data is being loaded.");
            //ExportToCSV();
            await loadDataFromServer();
            handleDataLoading(); // Bind after loading from server if no data
            alert("ON LAST SESSION YOU DID NOT UPLOAD SAVED OFFLINE INSPECTIONS TO THE CLOUD! LAST SESSION DATA NOW CANCELLED. \n\nFor a copy of the list of inspections: Contact Scafpac Admin at: admin@scafpac.com.au .");

            BackupOldInspectionData();

            //setTimeout(() => deleteAllTables(), 1000);
            //$('#tblOffSystemInspectionList').DataTable().clear();
            //$('#tblOffSystemInspectionList').DataTable().destroy();
            setTimeout(function () {
                deleteAllTables();
                $('#tblOffSystemInspectionList').DataTable().clear();
                $('#tblOffSystemInspectionList').DataTable().destroy();
            }, 1000);

        } else {
            alert("YOU DID NOT UPLOAD SAVED INSPECTIONS AT THE END OF LAST SESSION!\n\nRE-DIRECTING YOU TO THE SAVED INSPECTIONS FOR UPLOAD TO CLOUD.");
            console.log("Redirecting to already saved local data.");
            handleDataLoading();
        }
    };
}

// Utility function to format a date string to "YYYY-MM-DD HH:MM:SS" format
function formatDateToString(dateString) {
    const date = new Date(dateString);

    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');
    const seconds = String(date.getSeconds()).padStart(2, '0');

    return `${year}-${month}-${day} ${hours}:${minutes}:${seconds}`;
}



function getSiteLocationById(siteLocId, callback) {
    let request = indexedDB.open(dbName, dbVersion);

    request.onsuccess = function (event) {
        let db = event.target.result;
        let transaction = db.transaction(["tblSiteLocation"], "readonly");
        let store = transaction.objectStore("tblSiteLocation");

        let getRequest = store.get(siteLocId);

        getRequest.onsuccess = function (event) {
            if (getRequest.result) {
                let siteLocationName = getRequest.result.SiteLocation;
                callback(siteLocationName);
            } else {
                console.log("SiteLocID not found");
                callback(null);
            }
        };

        getRequest.onerror = function (event) {
            console.log("Error fetching SiteLocID", event);
            callback(null);
        };
    };

    request.onerror = function (event) {
        console.log("Error opening IndexedDB", event);
        callback(null);
    };
}

function extractDateFromSerializedFormat(serializedDate) {
    // Matches /Date(XXXXXX)/
    let timestampMatch = /\/Date\((\d+)\)\//.exec(serializedDate);
    if (timestampMatch && timestampMatch[1]) {
        let timestamp = parseInt(timestampMatch[1], 10); // Convert to integer
        return new Date(timestamp); // Convert to JavaScript Date
    }
    return null; // Return null if the format is incorrect
}


function clearPreviousDataAndLoadNew(db) {
    let transaction = db.transaction(['tblRegister'], 'readwrite');
    let objectStore = transaction.objectStore('tblRegister');

    objectStore.clear().onsuccess = function (event) {
        console.log("Previous data cleared.");
        loadDataFromServer(); // Now load the new data from the server
    };

    objectStore.clear().onerror = function (event) {
        console.error("Error clearing data:", event.target.error);
    };
}

function loadDataFromServer() {
    return new Promise((resolve) => {
        GetDataFromServerDb($('#clientid').val(), () => {
            localStorage.setItem('dataFetched', new Date().toISOString());
            resolve();
        });
    });
}

// Other functions (setupDatabase, handleDataLoading, etc.) remain the same as in your original code.


// Handle data loading based on table and data existence.
//function handleDataLoading() {
//    checkTableAndData('tblRegister').then(({ tableExists, dataExists }) => {
//        if (!tableExists || !dataExists) {
//            return loadDataFromServer();
//        } else {
//            return Promise.resolve(); // Data is present, no need to fetch.
//        }
//    }).then(() => {
//        loadDropdownsFromLocal();
//        populateUserDropdown(); // Ensure it is called only once here
//    }).catch(handleError);
//}
function handleDataLoading() {
    loadDropdownsFromLocal();
    populateUserDropdown();

}


// Check if table and data exist.
function checkTableAndData(tableName) {
    return new Promise((resolve) => {
        const transaction = db.transaction([tableName], 'readonly');
        const objectStore = transaction.objectStore(tableName);
        objectStore.count().onsuccess = (event) => {
            resolve({ tableExists: true, dataExists: event.target.result > 0 });
        };
        transaction.onerror = () => resolve({ tableExists: false, dataExists: false });
    });
}

function loadDataFromServer() {
    return new Promise((resolve) => {
        GetDataFromServerDb($('#clientid').val(), () => {
            localStorage.setItem('dataFetched', new Date().toISOString());
            resolve();
        });
    });
}

function loadDropdownsFromLocal() {

    setTimeout(populateSiteLocationDropdown, 2000);
    /* populateAreaDropdown();*/
    //populateScaffoldOwnerDropdown();
    //populateBuildingDropdown();
    /* populateFloorLevelDropdown();*/
    workgroupddl();
    //  populateUserDropdown();
    /*  populateBearingDropdown();*/
}

function showLoadingMessage() {
    $('#LoadingPanel1Foroffline').show();
}

function hideLoadingMessage() {
    $('#LoadingPanel1Foroffline').hide();
}

function handleError(error) {
    console.error('An error occurred:', error);
    alert('Failed to load data. Please try again.');
}

function checkIfDataExists(storeName, callback) {
    var transaction = db.transaction([storeName], 'readonly');
    var objectStore = transaction.objectStore(storeName);
    var countRequest = objectStore.count();

    countRequest.onsuccess = function () {
        if (countRequest.result > 0) {
            callback(true); // Data exists
        } else {
            callback(false); // No data
        }
    };

    countRequest.onerror = function () {
        console.error('Error checking data in', storeName);
        callback(false); // Assume no data on error
    };
}



// Function to upload data to server when back online
function uploadDataToServer() {
    $('#LoadingPanel1Foroffline').show();

    var LoginUserId = getParameterByName("UserId");
    var clientId = getParameterByName("ClientId");
    var Siteid = getParameterByName("Siteid");
    if (document.getElementById('ddlSitesLoc').value != -1) {

        var LocalSiteID = $('#ddlSitesLoc').val();
        var encryData = Encrypt([LocalSiteID]);
        LocalSiteID = encryData[0];
        if (!navigator.onLine) {
            alert("You are currently offline. Please go online to upload data.");
            return; // Exit the function if the user is offline
        }
        console.log("User is online, uploading data to server...");

        // Open a transaction on the tblInspection object store
        var transaction = db.transaction(['tblInspection'], 'readonly');
        var objectStore = transaction.objectStore('tblInspection');

        // Initialize an array to store offline records
        var offlineDataArray = [];

        // Open a cursor to iterate over all records
        var cursorRequest = objectStore.openCursor();

        cursorRequest.onsuccess = function (event) {
            var cursor = event.target.result;

            if (cursor) {
                var record = cursor.value;

                // Step 1: Check if isOffline is true
                if (record.IsOffline === true) {
                    console.log("Offline record found: ", record);

                    // Step 2: Append the record to the offlineDataArray
                    offlineDataArray.push(record);
                }

                // Move to the next record
                cursor.continue();
            } else {
                // No more entries, process the collected offlineDataArray
                if (offlineDataArray.length > 0) {
                    $.ajax({
                        url: '/OfflineOffSystemInspection/UploadData',
                        type: "Post",
                        async: false,
                        data: { offlineDataArray: offlineDataArray, LoginUserId: LoginUserId, clientId: clientId, Siteid: LocalSiteID },
                        success: function (response) {

                            if (response.Result == "Success") {
                                console.log("Data Uploaded Successfully:", response);


                                updateOfflineInspectionFlag(LoginUserId, clientId, Siteid);

                                deleteAllTables();
                                var Res = { Result: response.Result, Message: response.Message };
                                OfflineCustomAlert(Res);
                                $('#tblOffSystemInspectionList').DataTable().clear().destroy();
                                // Step 1: Extract the link from the URL
                                const currentUrl = window.location.href;

                                // Step 2: Retrieve the domain name, ID, and userRole from URL parameters
                                const urlObj = new URL(currentUrl);
                                const domainName = urlObj.origin;
                                const id = urlObj.searchParams.get("ClientId");
                                const UID = urlObj.searchParams.get("UserId");
                                const UserRole = urlObj.searchParams.get("UserRole");
                                const newUrl = `${domainName}/OffSystemInspection/OffSystemInspection?UID=${UID}&UserRole=${UserRole}&id=${id}`;

                                setTimeout(function () {
                                    window.location.href = newUrl;
                                }, 1000);

                            }
                            else {
                                var Res = { Result: response.Result, Message: response.Message };
                                BackupOldInspectionData();
                                OfflineCustomAlert(Res);
                                alert(response.Message)
                                //ExportToCSV();
                                deleteAllTables();
                                $('#tblOffSystemInspectionList').DataTable().clear();
                                $('#tblOffSystemInspectionList').DataTable().destroy();
                            }

                        },
                        error: function (error) {
                            //console.error("Error uploading data to the server", error);
                            var Res = { Result: "Failed", Message: error };
                            OfflineCustomAlert(Res);
                        }
                    });

                } else {


                    updateOfflineInspectionFlag(LoginUserId, clientId, Siteid);
                    if (updateOfflineInspectionFlagValue.Result == 'AlreadyMoved') {
                        if ($('#tblOffSystemInspectionList').DataTable().data().count() > 0) {
                            var Resp = { Result: "Failed", Message: updateOfflineInspectionFlagValue.Message };
                            OfflineCustomAlert(Resp)
                            alert(updateOfflineInspectionFlagValue.Message);
                            //ExportToCSV();
                            deleteAllTables();
                            $('#tblOffSystemInspectionList').DataTable().clear();
                            $('#tblOffSystemInspectionList').DataTable().destroy();
                        } else {
                            var Resp = { Result: "Failed", Message: "There is no Data to Upload to Cloud." };
                            OfflineCustomAlert(Resp);
                            $('#tblOffSystemInspectionList').DataTable().clear();
                            $('#tblOffSystemInspectionList').DataTable().destroy();
                        }
                    }
                    else {
                        console.log("No offline data to upload.");
                        //var Res = { Result: "Failed", Message: "Looks like no data was entered using Offline Link on this system. No data to Upload to Cloud." };
                        //OfflineCustomAlert(Res);
                        alert("No data was entered using Offline Link on this system. No data to Upload to Cloud.")
                        deleteAllTables();
                        $('#tblOffSystemInspectionList').DataTable().clear();
                        $('#tblOffSystemInspectionList').DataTable().destroy();
                        var Resp = { Result: "Success", Message: "Offline Register Data has been moved Online." };
                        OfflineCustomAlert(Resp);
                        // Step 1: Extract the link from the URL
                        const currentUrl = window.location.href;

                        // Step 2: Retrieve the domain name, ID, and userRole from URL parameters
                        const urlObj = new URL(currentUrl);
                        const domainName = urlObj.origin;
                        const id = urlObj.searchParams.get("ClientId");
                        const UID = urlObj.searchParams.get("UserId");
                        const UserRole = urlObj.searchParams.get("UserRole");
                        const newUrl = `${domainName}/OffSystemInspection/OffSystemInspection?UID=${UID}&UserRole=${UserRole}&id=${id}`;

                        setTimeout(function () {
                            window.location.href = newUrl;
                        }, 1000);
                    }

                }
            }
            $('#LoadingPanel1Foroffline').hide();

        };

        cursorRequest.onerror = function () {
            console.error("Error fetching data from tblInspection.");
            $('#LoadingPanel1Foroffline').hide();

        };
    } else {
        var response = { Result: "Failed", Message: "Please Select Site." };
        OfflineCustomAlert(response);
    }
}


var updateOfflineInspectionFlagValue;
function updateOfflineInspectionFlag(LoginUserId, clientId, Siteid) {
    $('#LoadingPanel1Foroffline').show();

    $.ajax({
        url: '/OfflineOffSystemInspection/UpdateOfflineInspectionFlag',
        type: "Post",
        data: { LoginUserId: LoginUserId, clientId: clientId, Siteid: Siteid },
        async: false,
        success: function (response) {
            updateOfflineInspectionFlagValue = response;
            console.log("Inspection flag Updated Successfully.", response);
        },
        error: function (error) {
            console.error("Error updating inspection flag:", error);
        }
    });
    $('#LoadingPanel1Foroffline').hide();

}

function deleteAllTables() {
    console.log("Deleting all tables...");

    var transaction = db.transaction(['tblRegister', 'tblFloorLevel', 'tblBuilding', 'tblArea', 'tblSiteLocation', 'tblInspection', 'tblUser'], 'readwrite');

    transaction.objectStore('tblRegister').clear();
    transaction.objectStore('tblFloorLevel').clear();
    transaction.objectStore('tblBuilding').clear();
    transaction.objectStore('tblArea').clear();
    transaction.objectStore('tblSiteLocation').clear();
    transaction.objectStore('tblInspection').clear();
    transaction.objectStore('tblUser').clear();

    transaction.oncomplete = function () {
        console.log("All tables cleared successfully.");
    };

    transaction.onerror = function () {
        console.error("Error clearing tables.");
    };
}

function dropObjectStoreIfExists(db, storeName) {
    if (db.objectStoreNames.contains(storeName)) {
        db.deleteObjectStore(storeName);
        console.log(`Object store '${storeName}' deleted successfully.`);
    } else {
        console.log(`Object store '${storeName}' does not exist.`);
    }
}
function CreateTableForIndexDb(db) {

    if (!db.objectStoreNames.contains('tblSiteLocation')) {
        //var mealsStore = db.createObjectStore('Meals', { keyPath: 'MeatTypeID', autoIncrement: false });

        let objectStore5 = db.createObjectStore('tblSiteLocation', { keyPath: 'SiteLocID' });

        // Define indexes without uniqueness
        objectStore5.createIndex('SiteLocation', 'SiteLocation', { unique: false });
        objectStore5.createIndex('ClientId', 'ClientId', { unique: false });
        objectStore5.createIndex('CreatedBy', 'CreatedBy', { unique: false });
        objectStore5.createIndex('CreatedOn', 'CreatedOn', { unique: false });
        objectStore5.createIndex('UpdatedBy', 'UpdatedBy', { unique: false });
        objectStore5.createIndex('UpdatedOn', 'UpdatedOn', { unique: false });
        objectStore5.createIndex('Address', 'Address', { unique: false });
        objectStore5.createIndex('City', 'City', { unique: false });
        objectStore5.createIndex('State', 'State', { unique: false });
        objectStore5.createIndex('Zip', 'Zip', { unique: false });
        objectStore5.createIndex('SiteLocationStatus', 'SiteLocationStatus', { unique: false });
        objectStore5.createIndex('OwnerName', 'OwnerName', { unique: false });
        objectStore5.createIndex('DistributionList', 'DistributionList', { unique: false });
        objectStore5.createIndex('Country', 'Country', { unique: false });
        objectStore5.createIndex('SiteLat', 'SiteLat', { unique: false });
        objectStore5.createIndex('SiteLong', 'SiteLong', { unique: false });
        objectStore5.createIndex('ManHoursAvailable', 'ManHoursAvailable', { unique: false });
        objectStore5.createIndex('MaintainInventory', 'MaintainInventory', { unique: false });
        objectStore5.createIndex('CycloneManagement', 'CycloneManagement', { unique: false });
        objectStore5.createIndex('ManHoursManagement', 'ManHoursManagement', { unique: false });
        objectStore5.createIndex('MaintainInventoryAtJob', 'MaintainInventoryAtJob', { unique: false });
        objectStore5.createIndex('GPS', 'GPS', { unique: false });
        objectStore5.createIndex('FloorLevel', 'FloorLevel', { unique: false });
        objectStore5.createIndex('EnabledGPSSitePin', 'EnabledGPSSitePin', { unique: false });
        objectStore5.createIndex('EnabledCaptureGps', 'EnabledCaptureGps', { unique: false });
        objectStore5.createIndex('OffSystemInspection', 'OffSystemInspection', { unique: false });
        objectStore5.createIndex('InventoryMailingList', 'InventoryMailingList', { unique: false });
        objectStore5.createIndex('SkipMaterial', 'SkipMaterial', { unique: false });
        objectStore5.createIndex('PipeLineInvHide', 'PipeLineInvHide', { unique: false });
        console.log('tblSiteLocation table created');


    }


    if (!db.objectStoreNames.contains('tblArea')) {
        let objectStore4 = db.createObjectStore('tblArea', { keyPath: 'AreaID' });

        // Define indexes without uniqueness
        objectStore4.createIndex('Area', 'Area', { unique: false });
        objectStore4.createIndex('ClientID', 'ClientID', { unique: false });
        objectStore4.createIndex('CreatedBy', 'CreatedBy', { unique: false });
        objectStore4.createIndex('CreatedOn', 'CreatedOn', { unique: false });
        objectStore4.createIndex('UpdatedBy', 'UpdatedBy', { unique: false });
        objectStore4.createIndex('UpdatedOn', 'UpdatedOn', { unique: false });
        objectStore4.createIndex('AreaStatus', 'AreaStatus', { unique: false });
        objectStore4.createIndex('SiteLocId', 'SiteLocId', { unique: false });


        console.log('tblArea order table created');
    }

    if (!db.objectStoreNames.contains('tblBuilding')) {
        // Create the LunchOrders object store
        let objectStore3 = db.createObjectStore('tblBuilding', { keyPath: 'BuildingID' });

        // Define indexes without uniqueness
        objectStore3.createIndex('BuildingNo', 'BuildingNo', { unique: false });
        objectStore3.createIndex('BuildingStatus', 'BuildingStatus', { unique: false });
        objectStore3.createIndex('ClientId', 'ClientId', { unique: false });
        objectStore3.createIndex('AreaId', 'AreaId', { unique: false });
        objectStore3.createIndex('CreatedBy', 'CreatedBy', { unique: false });
        objectStore3.createIndex('CreatedOn', 'CreatedOn', { unique: false });
        objectStore3.createIndex('UpdatedBy', 'UpdatedBy', { unique: false });
        objectStore3.createIndex('UpdatedOn', 'UpdatedOn', { unique: false });

        console.log('tblBuilding table created');
    }
    if (!db.objectStoreNames.contains('tblFloorLevel')) {
        let objectStore2 = db.createObjectStore('tblFloorLevel', { keyPath: 'FloorLevelId' });

        // Define indexes without uniqueness and auto-increment
        objectStore2.createIndex('Category', 'Category', { unique: false });
        objectStore2.createIndex('FloorLevel', 'FloorLevel', { unique: false });
        objectStore2.createIndex('ClientID', 'ClientID', { unique: false });
        objectStore2.createIndex('SiteLocId', 'SiteLocId', { unique: false });
        objectStore2.createIndex('FloorLevelStatus', 'FloorLevelStatus', { unique: false });
        objectStore2.createIndex('SortOrder', 'SortOrder', { unique: false });
        objectStore2.createIndex('CreatedBy', 'CreatedBy', { unique: false });
        objectStore2.createIndex('CreatedOn', 'CreatedOn', { unique: false });
        objectStore2.createIndex('UpdatedBy', 'UpdatedBy', { unique: false });
        objectStore2.createIndex('UpdatedOn', 'UpdatedOn', { unique: false });
        objectStore2.createIndex('LocationID', 'LocationID', { unique: false });
        objectStore2.createIndex('BuildingID', 'BuildingID', { unique: false });
        console.log('tblFloorLevel table created');


    }

    if (!db.objectStoreNames.contains('tblRegister')) {
        let objectStore1 = db.createObjectStore('tblRegister', { keyPath: 'REGISTERId' });


        console.log('tblRegister table created');


    }
    if (!db.objectStoreNames.contains('tblInspection')) {

        let objectStore6 = db.createObjectStore("tblInspection", { keyPath: "InspectionID", autoIncrement: true });

        // Define the indexes based on the table structure
        objectStore6.createIndex("InspectionDate", "InspectionDate", { unique: false });
        objectStore6.createIndex("InspectedBy", "InspectedBy", { unique: false });
        objectStore6.createIndex("RegisterID", "RegisterID", { unique: false });
        objectStore6.createIndex("CreatedBy", "CreatedBy", { unique: false });
        objectStore6.createIndex("CreatedOn", "CreatedOn", { unique: false });
        objectStore6.createIndex("UpdatedBy", "UpdatedBy", { unique: false });
        objectStore6.createIndex("UpdatedOn", "UpdatedOn", { unique: false });
        objectStore6.createIndex("OnlineInsp", "OnlineInsp", { unique: false });
        objectStore6.createIndex("CompleteInsp", "CompleteInsp", { unique: false });
        objectStore6.createIndex("Lat", "Lat", { unique: false });
        objectStore6.createIndex("Long", "Long", { unique: false });
        objectStore6.createIndex("GPSAddress", "GPSAddress", { unique: false });
        objectStore6.createIndex("InspStatus", "InspStatus", { unique: false });
        objectStore6.createIndex("AckDate", "AckDate", { unique: false });
        objectStore6.createIndex("LicenceNo", "LicenceNo", { unique: false });
        objectStore6.createIndex("InspectedById", "InspectedById", { unique: false });
        objectStore6.createIndex("LastEmailSentOn", "LastEmailSentOn", { unique: false });
        objectStore6.createIndex("IsOffline", "IsOffline", { unique: false });
        objectStore6.createIndex("UserId", "UserId", { unique: false });
        objectStore6.createIndex("Acknowledgment", "Acknowledgment", { unique: false });
        objectStore6.createIndex("AcknowledgeTimeZone", "AcknowledgeTimeZone", { unique: false });
        objectStore6.createIndex("OffSystemInspection", "OffSystemInspection", { unique: false });
        console.log('tblInspection  table created');

    }
    if (!db.objectStoreNames.contains('tblInspectionList')) {
        let objectStore7 = db.createObjectStore("tblInspectionList", { keyPath: "InspectionListID", autoIncrement: true });

        // Define the indexes based on the table structure
        objectStore7.createIndex("InspectionID", "InspectionID", { unique: false });
        objectStore7.createIndex("CheckListID", "CheckListID", { unique: false });
        objectStore7.createIndex("ContentID", "ContentID", { unique: false });
        objectStore7.createIndex("ClientID", "ClientID", { unique: false });
        objectStore7.createIndex("Y", "Y", { unique: false });
        objectStore7.createIndex("N", "N", { unique: false });
        objectStore7.createIndex("NA", "NA", { unique: false });
        objectStore7.createIndex("ActionRequired", "ActionRequired", { unique: false });
        objectStore7.createIndex("CreatedBy", "CreatedBy", { unique: false });
        objectStore7.createIndex("CreatedOn", "CreatedOn", { unique: false });
        objectStore7.createIndex("UpdatedBy", "UpdatedBy", { unique: false });
        objectStore7.createIndex("UpdatedOn", "UpdatedOn", { unique: false });
        objectStore7.createIndex("RegisterId", "RegisterId", { unique: false });
    }
    if (!db.objectStoreNames.contains('tblUser')) {
        objectStore8 = db.createObjectStore("tblUser", { keyPath: "UserId", autoIncrement: false });

        objectStore8.createIndex("Name", "Name", { unique: false });
        objectStore8.createIndex("LicenceNo", "LicenceNo", { unique: false });
    }
    if (!db.objectStoreNames.contains('tblStatusColourMapping')) {
        let objectStore9 = db.createObjectStore("tblStatusColourMapping", {
            keyPath: "SCMID", // Primary key, which is auto-incremented
            autoIncrement: false
        });
        objectStore9.createIndex("Timeframe", "Timeframe", { unique: false });
        objectStore9.createIndex("DaysDiffFrom", "DaysDiffFrom", { unique: false });
        objectStore9.createIndex("DaysDiffTo", "DaysDiffTo", { unique: false });
        objectStore9.createIndex("Value", "Value", { unique: false });
        objectStore9.createIndex("Colour", "Colour", { unique: false });
    }
}

function GetDataFromServerDb(clientid, callback) {
    $('#LoadingPanel1Foroffline').show();

    var id = clientid;
    var Siteid = getParameterByName('Siteid');

    $.ajax({
        url: '/OfflineOffSystemInspection/GetAllDataFromServerDB',
        type: 'get',
        async: false, // Note: Consider changing `async: false` to `true` if possible for better user experience.
        data: { clientid: id, Siteid: Siteid },
        success: function (response) {
            var data = response;

            var Getsitelocationdata = data.tblsitelocationdata;
            var GetAreadata = data.tblAreadata;
            var GetBuildingData = data.tblbuildingdata;
            var GetFloorLevelData = data.tblfloorleveldata;
            var GetRegisterData = data.tblregisterdata;
            var Getuserdata = data.Tbluserdata;

            if (!GetRegisterData || GetRegisterData.length === 0) {
                alert('Register data not available. Please go online and load the data first.');

                // Optionally, you can drop all tables here if needed
                deleteAllTables();
                $('#LoadingPanel1Foroffline').hide();

                return;  // Exit the function if tblregisterdata is not available
            } else {
                // Insert data into IndexedDB tables
                insertTblSiteLocationData(Getsitelocationdata);
                insertTblAreaData(GetAreadata);
                insertTblBuildingData(GetBuildingData);
                insertTblFloorLevelData(GetFloorLevelData);
                insertTblRegisterData(GetRegisterData);
                insertTbluserdata(Getuserdata);

                console.log('Data loaded successfully from server.');

                // Call the callback function if provided
                if (callback && typeof callback === 'function') {
                    callback();
                }
                $('#LoadingPanel1Foroffline').hide();

            }
        },
        error: function (error) {
            console.error('Error loading data from server:', error);
        }
    });
    $('#LoadingPanel1Foroffline').hide();

}

function insertTblSiteLocationData(data) {
    const dbName = 'scaffpac'; // Your IndexedDB database name
    const request = indexedDB.open(dbName, dbVersion); // Open the IndexedDB database

    request.onsuccess = function (event) {
        db = event.target.result;
        const transaction = db.transaction(['tblSiteLocation'], 'readwrite');
        const objectStore = transaction.objectStore('tblSiteLocation');

        data.forEach(item => {
            objectStore.put(item); // Insert or update data in the table
        });

        transaction.oncomplete = function () {
            console.log('tblSiteLocation data inserted successfully');
        };

        transaction.onerror = function (event) {
            console.error('Error inserting tblSiteLocation data:', event.target.error);
        };
    };

    request.onerror = function (event) {
        console.error('Error opening IndexedDB:', event.target.error);
    };
}


function insertTbluserdata(data) {
    const dbName = 'scaffpac'; // Your IndexedDB database name
    const request = indexedDB.open(dbName, dbVersion); // Open the IndexedDB database

    request.onsuccess = function (event) {
        db = event.target.result;
        const transaction = db.transaction(['tblUser'], 'readwrite');
        const objectStore = transaction.objectStore('tblUser');

        data.forEach(item => {
            objectStore.put(item); // Insert or update data in the table
        });

        transaction.oncomplete = function () {
            console.log('tblUser data inserted successfully');
            //loadDropdownsFromLocal()
            console.log('all dropdown load successfully');
        };

        transaction.onerror = function (event) {
            console.error('Error inserting tblUser data:', event.target.error);
        };
    };

    request.onerror = function (event) {
        console.error('Error opening IndexedDB:', event.target.error);
    };
}


function insertTblAreaData(data) {
    const dbName = 'scaffpac'; // Your IndexedDB database name
    const request = indexedDB.open(dbName, dbVersion); // Open the IndexedDB database

    request.onsuccess = function (event) {
        db = event.target.result;
        const transaction = db.transaction(['tblArea'], 'readwrite');
        const objectStore = transaction.objectStore('tblArea');

        data.forEach(item => {
            objectStore.put(item); // Insert or update data in the table
        });

        transaction.oncomplete = function () {
            console.log('tblArea data inserted successfully');
        };

        transaction.onerror = function (event) {
            console.error('Error inserting tblArea data:', event.target.error);
        };
    };

    request.onerror = function (event) {
        console.error('Error opening IndexedDB:', event.target.error);
    };
}

function insertTblBuildingData(data) {
    const dbName = 'scaffpac'; // Your IndexedDB database name
    const request = indexedDB.open(dbName, dbVersion); // Open the IndexedDB database

    request.onsuccess = function (event) {
        db = event.target.result;
        const transaction = db.transaction(['tblBuilding'], 'readwrite');
        const objectStore = transaction.objectStore('tblBuilding');

        data.forEach(item => {
            objectStore.put(item); // Insert or update data in the table
        });

        transaction.oncomplete = function () {
            console.log('tblBuilding data inserted successfully');
        };

        transaction.onerror = function (event) {
            console.error('Error inserting tblBuilding data:', event.target.error);
        };
    };

    request.onerror = function (event) {
        console.error('Error opening IndexedDB:', event.target.error);
    };
}

function insertTblFloorLevelData(data) {
    const dbName = 'scaffpac'; // Your IndexedDB database name
    const request = indexedDB.open(dbName, dbVersion); // Open the IndexedDB database

    request.onsuccess = function (event) {
        db = event.target.result;
        const transaction = db.transaction(['tblFloorLevel'], 'readwrite');
        const objectStore = transaction.objectStore('tblFloorLevel');

        data.forEach(item => {
            objectStore.put(item); // Insert or update data in the table
        });

        transaction.oncomplete = function () {
            console.log('tblFloorLevel data inserted successfully');

        };

        transaction.onerror = function (event) {
            console.error('Error inserting tblFloorLevel data:', event.target.error);
        };
    };

    request.onerror = function (event) {
        console.error('Error opening IndexedDB:', event.target.error);
    };
}

function insertTblRegisterData(data) {
    const dbName = 'scaffpac'; // Your IndexedDB database name
    const request = indexedDB.open(dbName, dbVersion); // Open the IndexedDB database

    request.onsuccess = function (event) {
        db = event.target.result;
        const transaction = db.transaction(['tblRegister'], 'readwrite');
        const objectStore = transaction.objectStore('tblRegister');

        data.forEach(item => {
            objectStore.put(item); // Insert or update data in the table
        });

        transaction.oncomplete = function () {
            console.log('tblRegister data inserted successfully');
            //    getInspectionList();
        };

        transaction.onerror = function (event) {
            console.error('Error inserting tblRegister data:', event.target.error);
        };
    };

    request.onerror = function (event) {
        console.error('Error opening IndexedDB:', event.target.error);
    };
}




function populateSiteLocationDropdown() {
    $('#LoadingPanel1Foroffline').show();

    var ClientId = parseInt(document.getElementById('clientid').value, 10);
    var siteddl = document.getElementById("ddlSitesLoc");
    var transaction = db.transaction(["tblSiteLocation"], "readonly");
    var objectStore = transaction.objectStore("tblSiteLocation");

    objectStore.getAll().onsuccess = function (event) {
        var data = event.target.result;
        siteddl.innerHTML = '';
        siteddl.innerHTML = '<option value="-1">All</option>';

        var filteredData = data.filter(function (item) {
            return item.ClientId === ClientId;
        });

        filteredData.forEach(function (item) {
            var option = document.createElement("option");
            option.value = item.SiteLocID;
            option.text = item.SiteLocation;
            siteddl.appendChild(option);
        });

        // Automatically select the site if only one is available
        if (filteredData.length === 1) {
            siteddl.value = filteredData[0].SiteLocID;
            //ddlSites_SelectedIndexChanged();
            populateAreaDropdown();
            populateScaffoldOwnerDropdown();
            populateBuildingDropdown();
            populateFloorLevelDropdown();
            workgroupddl();
            getInspectionList();
            populateBearingDropdown();
            getInspectionList();
            $('#LoadingPanel1Foroffline').hide();

        }
        else {
            $('#LoadingPanel1Foroffline').hide();

        }
    };
}




function populateAreaDropdown() {
    var SiteLocId = parseInt(document.getElementById("ddlSitesLoc").value, 10); // Convert the value to an integer
    var ClientId = parseInt(document.getElementById('clientid').value, 10);
    var areaddl = document.getElementById("ddlLocaSite");

    // Clear the dropdown before loading new options
    console.log("Clearing the area dropdown options.");
    areaddl.innerHTML = '';
    areaddl.innerHTML = '<option value="">All</option>'; // Clear the dropdown and add the default "All" option

    var transaction = db.transaction(["tblArea"], "readonly");
    var objectStore = transaction.objectStore("tblArea");

    if (isNaN(SiteLocId) || SiteLocId === 0) {
        // If SiteLocId is null, blank, or empty, filter by ClientId
        var request = objectStore.getAll();

        request.onsuccess = function (event) {
            var data = event.target.result;
            var filteredData = data
                .filter(item => item.ClientID === ClientId)
                .sort((a, b) => a.Area.localeCompare(b.Area));

            // Populate the dropdown with the filtered and sorted data
            filteredData.forEach(function (item) {
                var option = document.createElement("option");
                option.value = item.AreaID;
                option.text = item.Area;
                areaddl.appendChild(option);
            });

            console.log("Areas filtered by ClientId have been added to the dropdown.");
        };
    } else {
        // Filter by both ClientId and SiteLocId
        var index = objectStore.index("SiteLocId");
        var request = index.getAll(SiteLocId);

        request.onsuccess = function (event) {
            var data = event.target.result;
            var filteredData = data
                .filter(item => item.ClientID === ClientId)
                .sort((a, b) => a.Area.localeCompare(b.Area));

            // Populate the dropdown with the filtered and sorted data
            filteredData.forEach(function (item) {
                var option = document.createElement("option");
                option.value = item.AreaID;
                option.text = item.Area;
                areaddl.appendChild(option);
            });

            console.log("Areas filtered by both ClientId and SiteLocId have been added to the dropdown.");
        };
    }
}




//function populateBuildingDropdown() {
//    var ClientId = parseInt(document.getElementById('clientid').value, 10);
//    var sitelocationid = parseInt(document.getElementById("ddlSitesLoc").value, 10);
//    var areaId = parseInt(document.getElementById("ddlLocaSite").value, 10);
//    var buildingddl = document.getElementById("ddlBuilding");

//    var transaction = db.transaction(["tblBuilding", "tblArea"], "readonly");
//    var buildingStore = transaction.objectStore("tblBuilding");
//    var areaStore = transaction.objectStore("tblArea");

//    buildingddl.innerHTML = '<option value="">All</option>'; // Clear existing options

//    if (!areaId || areaId === "") {
//        // Case 1: If AreaId is empty, find buildings based on SiteLocId via tblArea
//        var areaRequest = areaStore.index("SiteLocId").getAll(sitelocationid);

//        areaRequest.onsuccess = function (event) {
//            var areas = event.target.result;

//            buildingStore.getAll().onsuccess = function (event) {
//                var buildings = event.target.result;

//                var filteredBuildings = [];
//                areas.forEach(function (area) {
//                    buildings.forEach(function (building) {
//                        if (building.ClientId === ClientId && building.AreaId === area.AreaID) {
//                            filteredBuildings.push({
//                                BuildingID: building.BuildingID,
//                                BuildingNo: building.BuildingNo,
//                                Area: area.Area
//                            });
//                        }
//                    });
//                });

//                // Sort filteredBuildings by BuildingNo
//                filteredBuildings.sort((a, b) => a.BuildingNo.localeCompare(b.BuildingNo));

//                // Add sorted options to the dropdown
//                filteredBuildings.forEach(function (building) {
//                    var option = document.createElement("option");
//                    option.value = building.BuildingID;
//                    option.text = building.BuildingNo + " (" + building.Area + ")";
//                    buildingddl.appendChild(option);
//                });

//                // Initialize or refresh Select2 for the dropdown
//                $('#ddlBuilding').select2({
//                    placeholder: 'Select buildings',
//                    allowClear: false,
//                    closeOnSelect: false,
//                    dropdownAutoWidth: true,
//                    width: '100%',
//                });
//            };
//        };
//    } else {
//        // Case 2: If AreaId is provided, find buildings based on AreaId
//        var buildingIndex = buildingStore.index("AreaId");
//        var buildingRequest = buildingIndex.getAll(areaId);

//        buildingRequest.onsuccess = function (event) {
//            var buildings = event.target.result;

//            areaStore.get(areaId).onsuccess = function (event) {
//                var area = event.target.result;
//                var areaName = area ? area.Area : '';

//                var filteredBuildings = buildings.filter(function (building) {
//                    return building.ClientId === ClientId;
//                });

//                // Sort filteredBuildings by BuildingNo
//                filteredBuildings.sort((a, b) => a.BuildingNo.localeCompare(b.BuildingNo));

//                // Add sorted options to the dropdown
//                filteredBuildings.forEach(function (building) {
//                    var option = document.createElement("option");
//                    option.value = building.BuildingID;
//                    option.text = building.BuildingNo + " (" + areaName + ")";
//                    buildingddl.appendChild(option);
//                });

//                // Initialize or refresh Select2 for the dropdown
//                $('#ddlBuilding').select2({
//                    placeholder: 'Select buildings',
//                    allowClear: false,
//                    closeOnSelect: true,
//                    dropdownAutoWidth: true,
//                    width: '100%'
//                });
//            };
//        };
//    }
//}


function populateBuildingDropdown() {
    const ClientId = parseInt(document.getElementById('clientid').value, 10);
    const areaId = parseInt(document.getElementById("ddlLocaSite").value, 10);
    const buildingddl = document.getElementById("ddlBuilding");

    // Clear existing options and reset the Select2 placeholder
    $('#ddlBuilding').empty().select2({
        placeholder: 'Select buildings',
        allowClear: false,
        closeOnSelect: true,
        dropdownAutoWidth: true,
        width: '100%'
    });

    if (!areaId || isNaN(areaId)) {
        // If AreaId is not provided or is invalid, just return without populating buildings.
        return;
    }

    // Create a transaction for the 'tblBuilding' object store.
    const transaction = db.transaction(["tblBuilding"], "readonly");
    const buildingStore = transaction.objectStore("tblBuilding");

    // Get buildings based on the provided AreaId.
    const buildingIndex = buildingStore.index("AreaId");
    const buildingRequest = buildingIndex.getAll(areaId);

    buildingRequest.onsuccess = function (event) {
        const buildings = event.target.result;

        // Clear the dropdown before adding new options
        buildingddl.innerHTML = '';

        // Filter buildings by ClientId.
        const filteredBuildings = buildings.filter(building => building.ClientId === ClientId);

        // Sort buildings by BuildingNo.
        filteredBuildings.sort((a, b) => a.BuildingNo.localeCompare(b.BuildingNo));

        // Add sorted building options to the dropdown.
        filteredBuildings.forEach(function (building) {
            const option = document.createElement("option");
            option.value = building.BuildingID;
            option.text = building.BuildingNo;
            buildingddl.appendChild(option);
        });

        // Reinitialize Select2 to update it with the new options
        $('#ddlBuilding').select2({
            placeholder: 'Select buildings',
            allowClear: false,
            closeOnSelect: true,
            dropdownAutoWidth: true,
            width: '100%'
        });
    };
}


//function populateFloorLevelDropdown() {
//    var sitelocationid = document.getElementById("ddlSitesLoc").value !== '' ? parseInt(document.getElementById("ddlSitesLoc").value, 10) : '';
//    var areaId = document.getElementById("ddlLocaSite").value !== '' ? parseInt(document.getElementById("ddlLocaSite").value, 10) : '';

//    // Get an array of selected building IDs from the multi-select dropdown and convert them to integers.
//    var selectedBuildingIds = $('#ddlBuilding').val() || [];
//    selectedBuildingIds = selectedBuildingIds.map(id => parseInt(id, 10));

//    var floorLevelddl = document.getElementById("ddlFloorLevel");

//    // Clear the current dropdown options and add a default "All" option.
//    floorLevelddl.innerHTML = '<option value="">--All--</option>';

//    // Open a transaction and access the tblFloorLevel object store.
//    var transaction = db.transaction(["tblFloorLevel"], "readonly");
//    var objectStore = transaction.objectStore("tblFloorLevel");

//    // Get all records from tblFloorLevel.
//    var request = objectStore.getAll();

//    request.onsuccess = function (event) {
//        var data = event.target.result;

//        // Filter data based on the provided conditions.
//        var filteredData = data.filter(function (item) {
//            if (selectedBuildingIds.length === 0) {
//                if (!areaId) {
//                    return (!sitelocationid || item.SiteLocId === sitelocationid);
//                } else {
//                    return item.LocationID === areaId;
//                }
//            } else {
//                return selectedBuildingIds.includes(item.BuildingID);
//            }
//        });

//        // Sort the filtered data by FloorLevel before populating the dropdown.
//        filteredData.sort((a, b) => a.FloorLevel.localeCompare(b.FloorLevel));

//        // Populate the dropdown with sorted floor level data.
//        filteredData.forEach(function (item) {
//            var option = document.createElement("option");
//            option.value = item.FloorLevelId;
//            option.text = item.FloorLevel;
//            floorLevelddl.appendChild(option);
//        });

//        // If no matching data is found, just keep the default "All" option.
//        if (filteredData.length === 0) {
//            console.log("No matching floor levels found.");
//        }
//    };

//    request.onerror = function (event) {
//        console.error("Error fetching data from IndexedDB", event);
//    };
//}
function populateFloorLevelDropdown() {
    // Retrieve values from the dropdowns and convert them to integers where applicable.
    var sitelocationid = document.getElementById("ddlSitesLoc").value !== '' ? parseInt(document.getElementById("ddlSitesLoc").value, 10) : '';
    var areaId = document.getElementById("ddlLocaSite").value !== '' ? parseInt(document.getElementById("ddlLocaSite").value, 10) : '';

    // Get an array of selected building IDs from the multi-select dropdown and convert them to integers.
    var selectedBuildingIds = $('#ddlBuilding').val() || [];
    selectedBuildingIds = selectedBuildingIds.map(id => parseInt(id, 10)).filter(id => !isNaN(id));

    var floorLevelddl = document.getElementById("ddlFloorLevel");

    // Clear the current dropdown options and add a default "All" option.
    floorLevelddl.innerHTML = '<option value="">--All--</option>';

    // Only proceed if at least one valid building ID is selected.
    if (selectedBuildingIds.length === 0) {
        console.log("Please select a building before selecting a floor level.");
        return;
    }

    // Open a transaction and access the tblFloorLevel object store.
    var transaction = db.transaction(["tblFloorLevel"], "readonly");
    var objectStore = transaction.objectStore("tblFloorLevel");

    // Get all records from tblFloorLevel.
    var request = objectStore.getAll();

    request.onsuccess = function (event) {
        var data = event.target.result;

        // Filter data based on the selected building IDs, sitelocationid, and areaId.
        var filteredData = data.filter(function (item) {
            return (
                selectedBuildingIds.includes(item.BuildingID) &&
                (!sitelocationid || item.SiteLocId === sitelocationid) &&
                (!areaId || item.LocationID === areaId)
            );
        });

        // Sort the filtered data by FloorLevel before populating the dropdown.
        filteredData.sort((a, b) => a.FloorLevel.localeCompare(b.FloorLevel));

        // Populate the dropdown with sorted floor level data.
        filteredData.forEach(function (item) {
            var option = document.createElement("option");
            option.value = item.FloorLevelId;
            option.text = item.FloorLevel;
            floorLevelddl.appendChild(option);
        });

        // If no matching data is found, just keep the default "All" option.
        if (filteredData.length === 0) {
            console.log("No matching floor levels found.");
        }
    };

    request.onerror = function (event) {
        console.error("Error fetching data from IndexedDB", event);
    };
}

//function populateFloorLevelDropdown() {
//    var sitelocationid = document.getElementById("ddlSitesLoc").value !== '' ? parseInt(document.getElementById("ddlSitesLoc").value, 10) : '';
//    var areaId = document.getElementById("ddlLocaSite").value !== '' ? parseInt(document.getElementById("ddlLocaSite").value, 10) : '';

//    // Get an array of selected building IDs from the multi-select dropdown and convert them to integers.
//    var selectedBuildingIds = $('#ddlBuilding').val() || [];
//    selectedBuildingIds = selectedBuildingIds.map(id => parseInt(id, 10)).filter(id => !isNaN(id));

//    var floorLevelddl = document.getElementById("ddlFloorLevel");

//    floorLevelddl.innerHTML = '';
//    // Clear the current dropdown options and add a default "All" option.
//    floorLevelddl.innerHTML = '<option value="">--All--</option>';

//    // Open a transaction and access the tblFloorLevel object store.
//    var transaction = db.transaction(["tblFloorLevel"], "readonly");
//    var objectStore = transaction.objectStore("tblFloorLevel");

//    // Get all records from tblFloorLevel.
//    var request = objectStore.getAll();

//    request.onsuccess = function (event) {
//        var data = event.target.result;

//        // Filter data based on the provided conditions.
//        var filteredData = data.filter(function (item) {
//            if (selectedBuildingIds.length === 0) {
//                if (areaId) {
//                    return item.LocationID === areaId;
//                } else {
//                    return (!sitelocationid || item.SiteLocId === sitelocationid);
//                }
//            } else {
//                return selectedBuildingIds.includes(item.BuildingID);
//            }
//        });

//        // Sort the filtered data by FloorLevel before populating the dropdown.
//        filteredData.sort((a, b) => a.FloorLevel.localeCompare(b.FloorLevel));

//        // Populate the dropdown with sorted floor level data.
//        filteredData.forEach(function (item) {
//            var option = document.createElement("option");
//            option.value = item.FloorLevelId;
//            option.text = item.FloorLevel;
//            floorLevelddl.appendChild(option);
//        });

//        // If no matching data is found, just keep the default "All" option.
//        if (filteredData.length === 0) {
//            console.log("No matching floor levels found.");
//        }
//    };

//    request.onerror = function (event) {
//        console.error("Error fetching data from IndexedDB", event);
//    };
//}

//function populateScaffoldOwnerDropdown() {
//    // Retrieve the selected values from the dropdowns.
//    var clientId = parseInt($('#clientid').val(), 10);
//    var siteLocId = parseInt(document.getElementById("ddlSitesLoc").value, 10);
//    var areaId = parseInt(document.getElementById("ddlLocaSite").value, 10);
//    var floorLevelId = parseInt(document.getElementById("ddlFloorLevel").value, 10);
//    var Bearingddl = document.getElementById("ddlBearing");

//    // Retrieve selected values from the multi-select buildingId dropdown.
//    var buildingSelect = document.getElementById("ddlBuilding");
//    var selectedBuildingIds = Array.from(buildingSelect.selectedOptions).map(option => parseInt(option.value, 10));

//    // Start a transaction to the tblRegister object store.
//    var transaction = db.transaction(["tblRegister"], "readonly");
//    var objectStore = transaction.objectStore("tblRegister");

//    // Fetch all records from tblRegister and filter them based on the selected values.
//    var request = objectStore.getAll();

//    request.onsuccess = function (event) {
//        var data = event.target.result;

//        // Filter data based on the provided conditions.
//        var filteredData = data.filter(function (item) {
//            return item.ClientID === clientId &&
//                item.SiteLocID === siteLocId &&
//                (isNaN(areaId) || item.LocationId === areaId) &&
//                (selectedBuildingIds.length === 0 || selectedBuildingIds.includes(item.BuildingID)) &&
//                (isNaN(floorLevelId) || item.FloorLevelId === floorLevelId);
//        });

//        // Extract unique ScaffoldOwner values.
//        var scaffoldOwners = [...new Set(filteredData.map(item => item.ScaffoldOwner))];

//        // Populate the ScaffoldOwner dropdown.
//        Bearingddl.innerHTML = '<option value="">All</option>';
//        scaffoldOwners.forEach(function (owner) {
//            var option = document.createElement("option");
//            option.value = owner;
//            option.text = owner;
//            Bearingddl.appendChild(option);
//        });

//        console.log("Filtered and loaded ScaffoldOwners into the dropdown.");
//    };

//    request.onerror = function (event) {
//        console.error("Failed to load data: ", event.target.errorCode);
//    };
//}
//function populateBearingDropdown() {
//    // Retrieve the selected values from the dropdowns
//    var clientId = parseInt($('#clientid').val(), 10);
//    var siteLocId = parseInt(document.getElementById("ddlSitesLoc").value, 10);
//    var areaId = parseInt(document.getElementById("ddlLocaSite").value, 10);
//    var buildingId = parseInt(document.getElementById("ddlBuilding").value, 10);
//    var floorLevelId = parseInt(document.getElementById("ddlFloorLevel").value, 10);
//    var Bearingddl = document.getElementById("ddlBearing");

//    // Start a transaction to the tblRegister object store
//    var transaction = db.transaction(["tblRegister"], "readonly");
//    var objectStore = transaction.objectStore("tblRegister");

//    // Fetch all records from tblRegister and filter them based on the selected values
//    var request = objectStore.getAll();

//    request.onsuccess = function (event) {
//        var data = event.target.result;

//        // Filter data based on the selected values
//        var filteredData = data.filter(function (item) {
//            return item.ClientID === clientId &&
//                item.SiteLocID === siteLocId &&
//                item.LocationId === areaId &&
//                item.BuildingID === buildingId &&
//                item.FloorLevelId === floorLevelId;
//        });

//        // Extract unique ScaffoldOwner values
//        var scaffoldOwners = [...new Set(filteredData.map(item => item.ScaffoldOwner))];

//        // Populate the ScaffoldOwner dropdown
//        Bearingddl.innerHTML = '<option value="">All</option>';
//        scaffoldOwners.forEach(function (owner) {
//            var option = document.createElement("option");
//            option.value = owner;
//            option.text = owner;
//            Bearingddl.appendChild(option);
//        });
//    };
//}



//function populateBearingDropdown() {
//    // Retrieve the selected values from the dropdowns.
//    var clientId = parseInt($('#clientid').val(), 10);
//    var siteLocId = parseInt(document.getElementById("ddlSitesLoc").value, 10);
//    var areaId = parseInt(document.getElementById("ddlLocaSite").value, 10);
//    var floorLevelId = parseInt(document.getElementById("ddlFloorLevel").value, 10);
//    var Bearingddl = document.getElementById("ddlBearing");

//    // Retrieve selected values from the multi-select buildingId dropdown.
//    var buildingSelect = document.getElementById("ddlBuilding");
//    var selectedBuildingIds = Array.from(buildingSelect.selectedOptions).map(option => parseInt(option.value, 10));

//    // Start a transaction to the tblRegister object store.
//    var transaction = db.transaction(["tblRegister"], "readonly");
//    var objectStore = transaction.objectStore("tblRegister");

//    // Fetch all records from tblRegister and filter them based on the selected values.
//    var request = objectStore.getAll();

//    request.onsuccess = function (event) {
//        var data = event.target.result;

//        // Filter data based on the provided conditions.
//        var filteredData = data.filter(function (item) {
//            return item.ClientID === clientId &&
//                item.SiteLocID === siteLocId &&
//                (isNaN(areaId) || item.LocationId === areaId) &&
//                (selectedBuildingIds.length === 0 || selectedBuildingIds.includes(item.BuildingID)) &&
//                (isNaN(floorLevelId) || item.FloorLevelId === floorLevelId);
//        });

//        // Extract unique ScaffoldOwner values.
//        var scaffoldOwners = [...new Set(filteredData.map(item => item.Bearing))];

//        // Populate the ScaffoldOwner dropdown.
//        Bearingddl.innerHTML = '<option value="">All</option>';
//        scaffoldOwners.forEach(function (owner) {
//            var option = document.createElement("option");
//            option.value = owner;
//            option.text = owner;
//            Bearingddl.appendChild(option);
//        });

//        console.log("Filtered and loaded ScaffoldOwners into the dropdown.");
//    };

//    request.onerror = function (event) {
//        console.error("Failed to load data: ", event.target.errorCode);
//    };
//}
function populateBearingDropdown() {
    // Retrieve the selected values from the dropdowns.
    var clientId = parseInt($('#clientid').val(), 10);
    var siteLocId = parseInt(document.getElementById("ddlSitesLoc").value, 10);
    var areaId = parseInt(document.getElementById("ddlLocaSite").value, 10);
    var floorLevelId = parseInt(document.getElementById("ddlFloorLevel").value, 10);
    var Bearingddl = document.getElementById("ddlBearing");

    // Retrieve selected values from the multi-select buildingId dropdown.
    var buildingSelect = document.getElementById("ddlBuilding");
    var selectedBuildingIds = Array.from(buildingSelect.selectedOptions).map(option => parseInt(option.value, 10));

    // Start a transaction to the tblRegister object store.
    var transaction = db.transaction(["tblRegister"], "readonly");
    var objectStore = transaction.objectStore("tblRegister");

    // Fetch all records from tblRegister and filter them based on the selected values.
    var request = objectStore.getAll();

    request.onsuccess = function (event) {
        var data = event.target.result;

        // Filter data based on the provided conditions.
        var filteredData = data.filter(function (item) {
            return item.ClientID === clientId &&
                item.SiteLocID === siteLocId &&
                (isNaN(areaId) || item.LocationId === areaId) &&
                (selectedBuildingIds.length === 0 || selectedBuildingIds.includes(item.BuildingID)) &&
                (isNaN(floorLevelId) || item.FloorLevelId === floorLevelId);
        });

        // Extract unique Bearing values, excluding empty or blank values.
        var bearings = [...new Set(
            filteredData
                .map(item => item.Bearing)
                .filter(bearing => bearing && bearing.trim() !== "")
        )];

        // Populate the Bearing dropdown.
        Bearingddl.innerHTML = '<option value="">All</option>';
        bearings.forEach(function (bearing) {
            var option = document.createElement("option");
            option.value = bearing;
            option.text = bearing;
            Bearingddl.appendChild(option);
        });

        console.log("Filtered and loaded Bearings into the dropdown.");
    };

    request.onerror = function (event) {
        console.error("Failed to load data: ", event.target.errorCode);
    };
}


//function populateScaffoldOwnerDropdown() {
//    // Retrieve the selected values from the dropdowns
//    var clientId = parseInt($('#clientid').val(), 10);
//    var siteLocId = parseInt(document.getElementById("ddlSitesLoc").value, 10);
//    var areaId = parseInt(document.getElementById("ddlLocaSite").value, 10);
//    var selectedBuildingIds = $('#ddlBuilding').val() || []; // Get selected values from multiple-select dropdown
//    selectedBuildingIds = selectedBuildingIds.map(id => parseInt(id, 10)); // Convert each selected value to an integer
//    var floorLevelId = parseInt(document.getElementById("ddlFloorLevel").value, 10);
//    var scaffoldOwnerddl = document.getElementById("ddlSubOwner");

//    // Start a transaction to the tblRegister object store
//    var transaction = db.transaction(["tblRegister"], "readonly");
//    var objectStore = transaction.objectStore("tblRegister");

//    // Fetch all records from tblRegister and filter them based on the selected values
//    var request = objectStore.getAll();

//    request.onsuccess = function (event) {
//        var data = event.target.result;

//        // Filter data based on the selected values
//        var filteredData = data.filter(function (item) {
//            return item.ClientID === clientId &&
//                item.SiteLocID === siteLocId &&
//                (isNaN(areaId) || item.LocationId === areaId) &&
//                (selectedBuildingIds.length === 0 || selectedBuildingIds.includes(item.BuildingID)) &&
//                (isNaN(floorLevelId) || item.FloorLevelId === floorLevelId);
//        });

//        // Extract unique ScaffoldOwner values and sort them
//        var scaffoldOwners = [...new Set(filteredData.map(item => item.ScaffoldOwner))]
//            .sort((a, b) => a.localeCompare(b)); // Sort the scaffold owners alphabetically

//        // Populate the ScaffoldOwner dropdown
//        scaffoldOwnerddl.innerHTML = '<option value="">All</option>';
//        if (scaffoldOwners.length > 0) {
//            scaffoldOwnerddl.disabled = false;


//            scaffoldOwners.forEach(function (owner) {
//                var option = document.createElement("option");
//                option.value = owner;
//                option.text = owner;
//                scaffoldOwnerddl.appendChild(option);
//            });
//        } else {
//            scaffoldOwnerddl.disabled = true;

//        }
//    };

//    request.onerror = function (event) {
//        console.error("Error fetching data from IndexedDB", event);
//    };
//}

function populateScaffoldOwnerDropdown() {
    // Retrieve the selected values from the dropdowns
    var clientId = parseInt($('#clientid').val(), 10);
    var siteLocId = parseInt(document.getElementById("ddlSitesLoc").value, 10);
    var areaId = parseInt(document.getElementById("ddlLocaSite").value, 10);
    var selectedBuildingIds = $('#ddlBuilding').val() || []; // Get selected values from multi-select dropdown
    selectedBuildingIds = selectedBuildingIds.map(id => parseInt(id, 10)); // Convert each selected value to an integer
    var floorLevelId = parseInt(document.getElementById("ddlFloorLevel").value, 10);
    var scaffoldOwnerddl = document.getElementById("ddlSubOwner");

    // Start a transaction to the tblRegister object store
    var transaction = db.transaction(["tblRegister"], "readonly");
    var objectStore = transaction.objectStore("tblRegister");

    // Fetch all records from tblRegister and filter them based on the selected values
    var request = objectStore.getAll();

    request.onsuccess = function (event) {
        var data = event.target.result;

        // Filter data based on the selected values
        var filteredData = data.filter(function (item) {
            return item.ClientID === clientId &&
                item.SiteLocID === siteLocId &&
                (isNaN(areaId) || item.LocationId === areaId) &&
                (selectedBuildingIds.length === 0 || selectedBuildingIds.includes(item.BuildingID)) &&
                (isNaN(floorLevelId) || item.FloorLevelId === floorLevelId);
        });

        // Extract unique ScaffoldOwner values, exclude empty or blank values, and sort them
        var scaffoldOwners = [...new Set(filteredData
            .map(item => item.ScaffoldOwner)
            .filter(owner => owner && owner.trim() !== ""))] // Filter out empty or blank ScaffoldOwner values
            .sort((a, b) => a.localeCompare(b)); // Sort the scaffold owners alphabetically

        // Populate the ScaffoldOwner dropdown
        scaffoldOwnerddl.innerHTML = '<option value="">All</option>';
        if (scaffoldOwners.length > 0) {
            scaffoldOwnerddl.disabled = false;
            scaffoldOwners.forEach(function (owner) {
                var option = document.createElement("option");
                option.value = owner;
                option.text = owner;
                scaffoldOwnerddl.appendChild(option);
            });
        } else {
            scaffoldOwnerddl.disabled = true;
        }
    };

    request.onerror = function (event) {
        console.error("Error fetching data from IndexedDB", event);
    };
}


//function populateUserDropdown() {
//    let request = indexedDB.open(dbName, dbVersion);
//    var LoginUserId = parseInt($('#Userid').val(), 10);

//    request.onsuccess = function (event) {
//        let db = event.target.result;
//        let transaction = db.transaction(["tblUser"], "readonly");
//        let objectStore = transaction.objectStore("tblUser");

//        let dropdown = document.getElementById("ddlddlInspectedByadd");

//        // Clear existing options (destroy dropdown content)
//        console.log("Clearing dropdown options.");
//        dropdown.innerHTML = '';  // This removes all existing options from the dropdown.

//        // Add default option "--Select--"
//        let defaultOption = document.createElement("option");
//        defaultOption.value = '';
//        defaultOption.text = '--Select--';
//        dropdown.appendChild(defaultOption);

//        let users = [];

//        // Gather all users from the cursor into an array.
//        objectStore.openCursor().onsuccess = function (event) {
//            let cursor = event.target.result;
//            if (cursor) {
//                users.push(cursor.value); // Add each user object to the array.
//                cursor.continue();
//            } else {
//                // Sort the users array by Name field.
//                users.sort((a, b) => a.Name.localeCompare(b.Name));

//                // Populate the dropdown with sorted user data.
//                users.forEach(user => {
//                    let option = document.createElement("option");
//                    option.value = user.UserId;
//                    option.text = user.Name;
//                    dropdown.appendChild(option);
//                    if (user.UserId === LoginUserId) {
//                        option.selected = true;
//                        console.log(`Selected user: ${user.Name}`);
//                    }
//                });


//                console.log("All users have been added to the dropdown.");
//            }
//        };
//    };

//    request.onerror = function (event) {
//        console.error("Database error: ", event.target.errorCode);
//    };
//}

//function populateUserDropdown() {
//    let request = indexedDB.open(dbName, dbVersion);
//    var LoginUserId = pa $('#Userid').val();

//    request.onsuccess = function (event) {
//        let db = event.target.result;
//        let transaction = db.transaction(["tblUser"], "readonly");
//        let objectStore = transaction.objectStore("tblUser");

//        let dropdown = document.getElementById("ddlddlInspectedByadd");

//        // Clear existing options (destroy dropdown content)
//        console.log("Clearing dropdown options.");
//        dropdown.innerHTML = '';  // This removes all existing options from the dropdown.

//        // Add default option "--Select--"
//        let defaultOption = document.createElement("option");
//        defaultOption.value = '';
//        defaultOption.text = '--Select--';
//        dropdown.appendChild(defaultOption);

//        let users = [];

//        // Gather all users from the cursor into an array.
//        objectStore.openCursor().onsuccess = function (event) {
//            let cursor = event.target.result;
//            if (cursor) {
//                users.push(cursor.value); // Add each user object to the array.
//                cursor.continue();
//            } else {
//                // Sort the users array by Name field.
//                users.sort((a, b) => a.Name.localeCompare(b.Name));

//                // Populate the dropdown with sorted user data.
//                users.forEach(user => {
//                    let option = document.createElement("option");
//                    option.value = user.UserId;
//                    option.text = user.Name;

//                    // If the user matches the login ID and has IsInspect = true, select this option.
//                    if (user.UserId === LoginUserId) {
//                        option.selected = true;
//                        console.log(`Selected user: ${user.Name}`);
//                    }

//                    dropdown.appendChild(option);
//                });

//                console.log("All users have been added to the dropdown.");
//            }
//        };
//    };

//    request.onerror = function (event) {
//        console.error("Database error: ", event.target.errorCode);
//    };
//}
function populateUserDropdown() {
    let request = indexedDB.open(dbName, dbVersion);
    var LoginUserId = parseInt($('#Userid').val(), 10);

    request.onsuccess = function (event) {
        let db = event.target.result;
        let transaction = db.transaction(["tblUser"], "readonly");
        let objectStore = transaction.objectStore("tblUser");

        let dropdown = document.getElementById("ddlddlInspectedByadd");

        // Clear existing options (destroy dropdown content)
        console.log("Clearing dropdown options.");
        dropdown.innerHTML = '';  // This removes all existing options from the dropdown.

        // Add default option "--Select--"
        let defaultOption = document.createElement("option");
        defaultOption.value = '';
        defaultOption.text = '--Select--';
        dropdown.appendChild(defaultOption);

        // Gather users in an array
        let users = [];
        let userIds = {}; // Use an object to track unique user IDs

        // Open cursor to gather all users
        objectStore.openCursor().onsuccess = function (event) {
            let cursor = event.target.result;
            if (cursor) {
                let user = cursor.value;

                // Check for uniqueness using userIds object
                if (!userIds[user.UserId]) {
                    userIds[user.UserId] = user; // Store user object keyed by UserId
                    users.push(user); // Add user to the array
                }

                cursor.continue(); // Move to the next record
            } else {
                // Sort users alphabetically by their Name field
                users.sort((a, b) => a.Name.localeCompare(b.Name));

                // Populate the dropdown with sorted user data
                users.forEach(user => {
                    let option = document.createElement("option");
                    option.value = user.UserId;
                    option.text = user.Name;
                    dropdown.appendChild(option);

                    // Preselect the option if it matches LoginUserId
                    if (user.UserId === LoginUserId) {
                        option.selected = true;
                    }
                });

                console.log("All unique users have been added to the dropdown.");
            }
        };

        transaction.oncomplete = function () {
            console.log("Transaction completed successfully.");
        };

        transaction.onerror = function (event) {
            console.error("Transaction error: ", event.target.errorCode);
        };
    };

    request.onerror = function (event) {
        console.error("Database error: ", event.target.errorCode);
    };
}


//function populateUserDropdown() {
//    let request = indexedDB.open(dbName, dbVersion);
//    var LoginUserId = parseInt($('#Userid').val(), 10);

//    request.onsuccess = function (event) {
//        let db = event.target.result;
//        let transaction = db.transaction(["tblUser"], "readonly");
//        let objectStore = transaction.objectStore("tblUser");

//        let dropdown = document.getElementById("ddlddlInspectedByadd");

//        // Clear existing options (destroy dropdown content)
//        console.log("Clearing dropdown options.");
//        dropdown.innerHTML = '';  // This removes all existing options from the dropdown.

//        // Add default option "--Select--"
//        let defaultOption = document.createElement("option");
//        defaultOption.value = '';
//        defaultOption.text = '--Select--';
//        dropdown.appendChild(defaultOption);

//        let users = [];
//        let userIds = new Set();

//        // Gather all users from the cursor into an array.
//        objectStore.openCursor().onsuccess = function (event) {
//            let cursor = event.target.result;
//            if (cursor) {
//                let user = cursor.value;


//                if (!userIds.has(user.UserId)) {
//                    userIds.add(user.UserId);
//                    users.push(user);
//                }

//                cursor.continue();
//            } else {
//                // Sort the users array by Name field.
//                users.sort((a, b) => a.Name.localeCompare(b.Name));

//                // Populate the dropdown with sorted user data.
//                users.forEach(user => {
//                    let option = document.createElement("option");
//                    option.value = user.UserId;
//                    option.text = user.Name;
//                    dropdown.appendChild(option);
//                    if (user.UserId === LoginUserId) {
//                        option.selected = true;
//                    }
//                });

//                console.log("All users have been added to the dropdown.");
//            }
//        };
//    };

//    request.onerror = function (event) {
//        console.error("Database error: ", event.target.errorCode);
//    };
//}
function filterWorkGroup(clientId, siteId, locationId, buildingIds, floorLevelId) {
    // Open a transaction and access the tblRegister object store
    var transaction = db.transaction(["tblRegister"], "readonly");
    var objectStore = transaction.objectStore("tblRegister");

    // Get all records from tblRegister
    var request = objectStore.getAll();

    request.onsuccess = function (event) {
        var data = event.target.result;

        // Filter the data based on provided parameters, ensuring non-empty checks
        var filteredData = data.filter(function (item) {
            return item.ClientID === clientId &&  // Always match ClientID
                (!siteId || item.SiteLocID === siteId) &&  // If SiteLocId is provided, filter by it
                (!locationId || item.LocationId === locationId) &&  // If LocationId is provided, filter by it
                (!floorLevelId || item.FloorLevelId === floorLevelId) &&  // If FloorLevelId is provided, filter by it
                (buildingIds.length === 0 || buildingIds.includes(item.BuildingID));  // Handle multi-select BuildingIDs
        });

        // Extract distinct WorkGroup values
        var workGroups = [...new Set(filteredData
            .filter(item => item.WorkGroup && item.WorkGroup.trim() !== "")  // Filter out null/empty WorkGroups
            .map(item => item.WorkGroup))];  // Get the WorkGroup values

        // Sort the WorkGroup values alphabetically
        workGroups.sort((a, b) => a.localeCompare(b));

        // Populate the WorkGroup dropdown
        var workGroupDdl = document.getElementById("ddlWorkgroup");
        workGroupDdl.innerHTML = '<option value="">All</option>';  // Default option

        workGroups.forEach(function (workGroup) {
            var option = document.createElement("option");
            option.value = workGroup;
            option.text = workGroup;
            workGroupDdl.appendChild(option);  // Append each sorted WorkGroup to the dropdown
        });
    };

    request.onerror = function (event) {
        console.error("Error fetching data from IndexedDB", event);
    };
}

function workgroupddl() {
    var clientId = parseInt($('#clientid').val(), 10);
    var siteLocId = parseInt(document.getElementById("ddlSitesLoc").value, 10);
    var areaId = parseInt(document.getElementById("ddlLocaSite").value, 10);
    var floorLevelId = parseInt(document.getElementById("ddlFloorLevel").value, 10);

    // Get selected building IDs from the multi-select dropdown and convert them to integers.
    var selectedBuildingIds = $('#ddlBuilding').val() || []; // Select2 returns an array of selected values (as strings).
    selectedBuildingIds = selectedBuildingIds.map(id => parseInt(id, 10)); // Convert each selected value to an integer.

    // Call the filterWorkGroup function with the selected values.
    filterWorkGroup(clientId, siteLocId, areaId, selectedBuildingIds, floorLevelId);
}




function getInspectionList() {
    var clientId = parseInt($('#clientid').val(), 10);
    var siteLocId = parseInt(document.getElementById("ddlSitesLoc").value, 10);
    var areaId = parseInt(document.getElementById("ddlLocaSite").value, 10);

    // Get an array of selected building IDs from the multi-select dropdown, converted to integers.
    var selectedBuildingIds = $('#ddlBuilding').val() || []; // Select2 returns an array of selected values (as strings).
    selectedBuildingIds = selectedBuildingIds.map(id => parseInt(id, 10)); // Convert each selected value to an integer.

    var floorLevelId = parseInt(document.getElementById("ddlFloorLevel").value, 10);
    var workGroupId = document.getElementById("ddlWorkgroup").value;
    var sabOwnerId = document.getElementById("ddlSubOwner").value;
    var ddlBearing = document.getElementById("ddlBearing").value;

    // Start IndexedDB transaction for tblRegister.
    var transaction = db.transaction(["tblRegister"], "readonly");
    var objectStore = transaction.objectStore("tblRegister");

    var result = [];

    // Open cursor to iterate over records.
    var cursorRequest = objectStore.openCursor();
    cursorRequest.onsuccess = function (event) {
        var cursor = event.target.result;
        if (cursor) {
            var record = cursor.value;

            // Apply mandatory ClientID filter.
            if (record.ClientID === clientId) {
                // Apply optional filters if provided.
                if ((!siteLocId || record.SiteLocID === siteLocId) &&
                    (!areaId || record.LocationId === areaId) &&
                    (selectedBuildingIds.length === 0 || selectedBuildingIds.includes(record.BuildingID)) &&
                    (!floorLevelId || record.FloorLevelId === floorLevelId) &&
                    (!workGroupId || record.WorkGroup === workGroupId) &&
                    (!sabOwnerId || record.ScaffoldOwner === sabOwnerId) &&
                    (!ddlBearing || record.Bearing === ddlBearing)) {

                    // If all conditions pass, push the record to the result.
                    result.push(record);
                }
            }
            cursor.continue(); // Continue to the next record.
        } else {
            // Sort the filtered results in descending order by ShowRegisterID.
            result.sort((a, b) => b.ShowRegisterID - a.ShowRegisterID);

            // Process and bind the sorted results after cursor completes.
            if (result.length === 0) {
                console.log("No records found.");
            } else {
                console.log("Records found:", result);
            }
            processAndBindData(result); // Pass sorted results to the next step.
        }
    };

    cursorRequest.onerror = function (event) {
        console.error("Cursor request failed", event);
    };
}

async function fetchAdditionalData(records) {
    const promises = records.map(async (record) => {
        try {
            // Fetch data from object stores
            const siteLocationPromise = getDataFromObjectStore('tblSiteLocation', record.SiteLocID);
            const areaPromise = getDataFromObjectStore('tblArea', record.LocationId);
            const buildingPromise = record.BuildingID ? getDataFromObjectStore('tblBuilding', record.BuildingID) : Promise.resolve(null);
            const floorLevelPromise = getDataFromObjectStore('tblFloorLevel', record.FloorLevelId);

            // Await all promises
            const [siteLocation, area, building, floorLevel] = await Promise.all([
                siteLocationPromise,
                areaPromise,
                buildingPromise,
                floorLevelPromise
            ]);

            // Combine data
            record.SiteLocationName = siteLocation ? siteLocation.SiteLocation : 'Unknown';
            record.AreaName = area ? area.Area : 'Unknown';
            record.BuildingName = building ? building.BuildingNo : null; // Keep null if data not found
            record.FloorLevelName = floorLevel ? floorLevel.FloorLevel : 'Unknown';

            return record;
        } catch (error) {
            console.error(`Error processing record with SiteLocId ${record.SiteLocID}:`, error);
            // Return the record with potentially missing data but include it in results
            return {
                ...record,
                SiteLocationName: 'Unknown',
                AreaName: 'Unknown',
                BuildingName: null,
                FloorLevelName: 'Unknown'
            };
        }
    });

    try {
        // Wait for all promises to resolve
        let results = await Promise.all(promises);

        // Process or bind the final data as needed
        processAndBindData(results);
    } catch (error) {
        console.error('Error processing records:', error);
    }
}



function getDataFromObjectStore(storeName, key) {
    return new Promise((resolve, reject) => {
        if (key === undefined || key === null) {
            reject("Invalid key provided for " + storeName);
            return;
        }

        var transaction = db.transaction([storeName], "readonly");
        var objectStore = transaction.objectStore(storeName);
        var request = objectStore.get(key);

        request.onsuccess = function (event) {
            resolve(event.target.result);
        };

        request.onerror = function (event) {
            reject("Error fetching data from " + storeName + ": " + event.target.errorCode);
        };
    });
}

function processAndBindData(data) {
    var str = "";

    data.forEach(function (record) {
        var Status = record.Status || "";
        if (Status !== 'NotStarted' && Status !== 'Pipeline') {
            var RegisterId = record.REGISTERId || "";
            var ScaffRef = record.ShowRegisterID || "";
            var ClientScaffref = record.ClientScaffref || "";
            var Location = record.Location || "";  // Use joined data
            var BuildingNo = record.BuildingNo || "";    // Use joined data
            var FloorLevel = record.FloorLevel || "";  // Use joined data
            var Description1 = record.Description || "";
            var AssetNo = record.AssetNo || "";
            var InspectedBy = record.InspectedBy || "";
            var SiteLocation = record.SiteLocation || "";  // Use joined data
            var RecentInspDate = record.RecentInspDate || "";
            var Bearing = record.Bearing || "";
            var WorkGroup = record.WorkGroup || "";

            str += "<tr>";
            str += "<td class='setWidth0' style='display:none'></td>";

            if (["RED", "GREEN", "YELLOW"].includes(Status.toUpperCase())) {
                str += "<td class='setWidth1'><img src='/images/" + Status + ".jpg' class='img-responsive' style='height: 22px;width: 22px;float: left;'/></td>";
            } else {
                str += "<td class='setWidth1'>" + Status + "</td>";
            }

            str += "<td class='setWidth1' style='display:none;'>" + Status + "</td>";
            str += "<td class='setWidth1'>" + ScaffRef + "</td>";
            str += "<td class='setWidth2'>" + SiteLocation + "</td>";
            str += "<td class='setWidth3'>" + Location + "</td>";
            str += "<td class='setWidth4'>" + BuildingNo + "</td>";
            str += "<td class='setWidth5'>" + AssetNo + "</td>";
            str += "<td class='setWidth6'>" + FloorLevel + "</td>";
            str += "<td class='setWidth6'>" + WorkGroup + "</td>";
            str += "<td class='setWidth6'>" + Bearing + "</td>";
            str += "<td class='setWidth10' style='cursor:pointer;' title='Click here to do Offline Quick Inspection Update.'><input type='checkbox' id='ChkTransID_" + RegisterId + "' class='ChkTransID setWidth10' value='" + RegisterId + "' ChkRegisterId_='" + RegisterId + "' /></td>";
            str += "<td class='setWidth9' style='cursor:pointer;' title='Click here to mark Inspection as Out Of Service.'><input type='checkbox' class='ChkTransIDOutOffServ setWidth9' id='OutOffServiceRegisterId_" + RegisterId + "'/></td>";

            str += "<td class='setWidth9' style='cursor:pointer;' title='Click here to mark Inspection as De-Planked.'><input type='checkbox' class=' ChkTransIDDePlank setWidth9' id='DePlankRegisterId_" + RegisterId + "'/></td>";
            str += "<td class='setWidth9' style='cursor:pointer;' title='Click here to mark Inspection as Tied-Down.'><input type='checkbox' class=' ChkTransIDTieDown setWidth9' id='TieDownRegisterId_" + RegisterId + "'/></td>";
            str += "<td class='setWidth10' style='cursor:pointer;' title='Click here to mark Inspection as Restricted Access.'><input type='checkbox' id='ChkTransIDRestrictedAction_" + RegisterId + "' class='ChkTransIDRestrictedAction setWidth10' value=" + RegisterId + " ChkRegisterIdRestrictedAction_='" + RegisterId + "' /></td>"
            str += "<td class='setWidth11'><input id='txtcomment_" + RegisterId + "' type='text' class='setWidth11' /></td>";
            str += "<td class='setWidth7'>" + RecentInspDate + "</td>";
            str += "<td class='setWidth8'>" + InspectedBy + "</td>";
            str += "<td class='setWidth8'>" + ClientScaffref + "</td></tr>";
        }
    });

    bindDataToTable(str);
}

function bindDataToTable(str) {
    // Destroy existing DataTable if it exists
    if ($.fn.DataTable.isDataTable('#tblOffSystemInspectionList')) {
        $('#tblOffSystemInspectionList').DataTable().clear().destroy();
    }

    $('#tbodyOffSystemInspectionList').html(str);
    $('.ChkAllID').click(function (event) {
        event.stopImmediatePropagation();
        var CheckBoxAll = $(this).prop('checked');

        if (CheckBoxAll == true) {
            $('.ChkTransID').prop('checked', true);
        } else {
            $('.ChkTransID').prop('checked', false);
        }
    });

    $('.ChkTransIDOutOffServ').click(function (event) {
        event.stopImmediatePropagation();
        var CheckBoxAll = $(this).prop('checked');
        var id = this.id.split('_')[1];

        if (CheckBoxAll == true) {
            $('#ChkTransID_' + id).prop('checked', true);
            $('#DePlankRegisterId_' + id).prop('checked', false);
            $('#TieDownRegisterId_' + id).prop('checked', false);
            $('#ChkTransIDRestrictedAction_' + id).prop('checked', false);
        } else {
            $('#ChkTransID_' + id).prop('checked', false);
        }
    });

    // De-Plank
    $('.ChkTransIDDePlank').click(function (event) {
        event.stopImmediatePropagation();
        var CheckBoxAll = $(this).prop('checked');
        var id = this.id.split('_')[1];

        if (CheckBoxAll == true) {
            $('#ChkTransID_' + id).prop('checked', true);
            $('#OutOffServiceRegisterId_' + id).prop('checked', false);
            $('#TieDownRegisterId_' + id).prop('checked', false);
            $('#ChkTransIDRestrictedAction_' + id).prop('checked', false);
        } else {
            $('#ChkTransID_' + id).prop('checked', false);
        }
    });

    $('.ChkTransIDTieDown').click(function (event) {
        event.stopImmediatePropagation();
        var CheckBoxAll = $(this).prop('checked');
        var id = this.id.split('_')[1];

        if (CheckBoxAll == true) {
            $('#ChkTransID_' + id).prop('checked', true);
            $('#DePlankRegisterId_' + id).prop('checked', false);
            $('#OutOffServiceRegisterId_' + id).prop('checked', false);
            $('#ChkTransIDRestrictedAction_' + id).prop('checked', false);
        } else {
            $('#ChkTransID_' + id).prop('checked', false);
        }
    });

    // Restricted Action
    $('.ChkTransIDRestrictedAction').click(function (event) {
        event.stopImmediatePropagation();
        var CheckBoxAll = $(this).prop('checked');
        var id = this.id.split('_')[1];

        if (CheckBoxAll == true) {
            $('#ChkTransID_' + id).prop('checked', true);
            $('#DePlankRegisterId_' + id).prop('checked', false);
            $('#TieDownRegisterId_' + id).prop('checked', false);
            $('#OutOffServiceRegisterId_' + id).prop('checked', false);
        } else {
            $('#ChkTransID_' + id).prop('checked', false);
        }
    });


    $('#tblOffSystemInspectionList').DataTable({
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
                title: 'Off System Inspection List',
                exportOptions: {
                    columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 15, 16, 17, 18]
                }
            }

        ],
    });


    //$('#LoadingPanel1Foroffline').hide();
}
function ExportToCSV() {
    $('.dt-button.buttons-csv.buttons-html5').trigger('click');
}
checkOnlineOffline();
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
function SetDateFormat(date) {
    if (date != null && date != undefined && date != "") {
        date = date.toString();
        var newDate = new Date(parseInt(date.substr(6)));
        var DateTo = ("0" + (newDate.getMonth() + 1)).slice(-2) + '-' + ("0" + newDate.getDate()).slice(-2) + '-' + newDate.getFullYear();
        return DateTo.toString();
    }
    else {
        return "";
    }
}
async function btnSaveInspect_Click() {
    var OffSystemDataArr = [];
    var ChkUpdateAllflag = false;

    if (document.getElementById('ddlSitesLoc').value != -1) {
        if (document.getElementById('ddlddlInspectedByadd').value != "" && document.getElementById('ddlddlInspectedByadd').value != null) {
            if (document.getElementById('rdpInspDate').value != "") {
                var InspectedById = document.getElementById('ddlddlInspectedByadd').value;
                var userid = parseInt(InspectedById.trim(), 10);
                const userDetails = await getUserDetailsById(userid);
                var LicenceNo = userDetails.LicenceNo;
                var InspectedBy = userDetails.Name;

                var checkboxes = document.querySelectorAll('.ChkTransID');
                checkboxes.forEach(function (checkbox) {
                    if (checkbox.checked) {
                        ChkUpdateAllflag = true;
                        var ClientID = $('#clientid').val();
                        var txtUpperComment = document.getElementById('txtComment').value;
                        var rdpInspDate = document.getElementById('rdpInspDate').value;
                        //var hdnTimeOffSet = document.getElementById('hdnTimeOffSet').value;
                        var InspectionComments = "";
                        var RegisterID = checkbox.value;
                        var comment = document.getElementById("txtcomment_" + RegisterID).value;
                        var StatusConfirmed = "Confirmed";

                        if (comment != "") {
                            InspectionComments = comment;
                        } else {
                            InspectionComments = txtUpperComment;
                        }

                        if (document.getElementById("OutOffServiceRegisterId_" + RegisterID).checked) {
                            StatusConfirmed = 'Out of Service';
                        }
                        if (document.getElementById("DePlankRegisterId_" + RegisterID).checked) {
                            StatusConfirmed = 'De-Planked';
                        }
                        if (document.getElementById("TieDownRegisterId_" + RegisterID).checked) {
                            StatusConfirmed = 'Tied-Down';
                        }
                        if (document.getElementById("ChkTransIDRestrictedAction_" + RegisterID).checked) {
                            StatusConfirmed = 'Restricted Access';
                        }

                        // Create the inspection data object
                        var ArryVal = {
                            ClientID: ClientID,
                            InspectionDate: rdpInspDate,
                            InspectedBy: InspectedBy,
                            Acknowledgment: true,
                            AckDate: rdpInspDate,

                            RegisterID: RegisterID,
                            StatusConfirmed: StatusConfirmed,
                            InspectionComments: InspectionComments,

                            CreatedOn: new Date(),
                            UpdatedOn: new Date(),

                            OnlineInsp: false,
                            CompleteInsp: true,
                            InspStatus: StatusConfirmed,
                            LicenceNo: LicenceNo, // Set the license number fetched from IndexedDB
                            InspectedById: InspectedById,
                            IsOffline: true,
                            UserId: userid,
                            OffSystemInspection: true
                        };

                        OffSystemDataArr.push(ArryVal);
                    }
                });

                if (ChkUpdateAllflag) {
                    // Store the data in IndexedDB after the loop has completed
                    insertInspectionDataToIndexedDB(OffSystemDataArr);
                } else {
                    var response = { Result: "Failed", Message: "At least One Job should have an 'Out Of Service' or 'Inspection' or 'De-Planked' or 'Tied-Down' Box ticked before clicking on 'Save' Button." };
                    OfflineCustomAlert(response);
                }

            } else {
                var response = { Result: "Failed", Message: "Please Select Inspected Date." };
                OfflineCustomAlert(response);
            }
        } else {
            var response = { Result: "Failed", Message: "Please Select Inspected By." };
            OfflineCustomAlert(response);
        }
    } else {
        var response = { Result: "Failed", Message: "Please Select Site." };
        OfflineCustomAlert(response);
    }
}



function getUserDetailsById(userId) {
    return new Promise(function (resolve, reject) {
        const dbName = 'scaffpac'; // Your IndexedDB database name
        const request = indexedDB.open(dbName, dbVersion); // Open the IndexedDB database

        request.onsuccess = function (event) {
            const db = event.target.result;
            const transaction = db.transaction(['tblUser'], 'readonly'); // Readonly transaction
            const objectStore = transaction.objectStore('tblUser');

            const userRequest = objectStore.get(userId); // Get user data by userId

            userRequest.onsuccess = function (event) {
                if (userRequest.result) {
                    resolve(userRequest.result); // Return user data on success
                } else {
                    reject("User not found");
                }
            };

            userRequest.onerror = function (event) {
                reject("Error retrieving user data: " + event.target.error);
            };
        };

        request.onerror = function (event) {
            reject("Error opening IndexedDB: " + event.target.error);
        };
    });
}




function insertInspectionDataToIndexedDB(dataArray) {
    // Open the database
    var request = indexedDB.open(dbName, dbVersion);

    request.onsuccess = function (event) {
        db = event.target.result;
        var transaction = db.transaction(["tblInspection"], "readwrite"); // Access the tblInspection object store
        var inspectionStore = transaction.objectStore("tblInspection");



        // Log the array size to make sure it's correct
        console.log("Data Array Length:", dataArray.length);
        console.log("Data Array Contents:", dataArray);

        // Insert each item in the array into the object store
        dataArray.forEach(function (item) {
            console.log("Inserting item:", item); // Logging each item to see what's being inserted

            var addRequest = inspectionStore.add(item);

            addRequest.onsuccess = function (event) {
                var inspectionId = event.target.result; // Get the ID of the newly added record
                console.log("Inspection data successfully inserted with ID:", inspectionId);

                var registerid = parseInt(item.RegisterID.trim(), 10);
                // After inserting into tblInspection, update tblRegister
                updateRegister(registerid, item.InspectionDate, item.InspectedBy, item.InspStatus);
            };

            addRequest.onerror = function () {
                console.log("Error inserting Data into tblInspection.");
                OfflineCustomAlert({ Result: "Failed", Message: "Error inserting data into tblInspection." });
            };
        });

        transaction.oncomplete = function () {
            console.log("All data successfully inserted into tblInspection.");
            OfflineCustomAlert({ Result: "Success", Message: "Data Saved!" });
            document.getElementById('txtComment').value = ''; // Clear comment input
            var checkboxes = document.querySelectorAll('.ChkAllID');
            checkboxes.forEach(function (checkbox) {
                checkbox.checked = false; // Uncheck all checkboxes
            });
            getInspectionList();
            // Hide loading panel
        };

        transaction.onerror = function () {
            console.log("Transaction error for tblInspection.");
            OfflineCustomAlert({ Result: "Failed", Message: "Transaction error for tblInspection." });
        };
    };

    request.onerror = function () {
        console.log("Failed to open IndexedDB.");
        OfflineCustomAlert({ Result: "Failed", Message: "Unable to open local database." });
        document.getElementById('LoadingPanel1Foroffline').style.display = 'none';
    };
}

function updateRegister(registerId, recentInspectionDate, inspectedBy, InspStatus) {
    if (InspStatus == 'Confirmed') {
        InspStatus = "Green"
    }
    // Open a new transaction for tblRegister
    var transaction = db.transaction(["tblRegister"], "readwrite");
    var registerStore = transaction.objectStore("tblRegister");

    // Get the existing record from tblRegister
    var getRequest = registerStore.get(registerId);

    getRequest.onsuccess = function (event) {
        var registerRecord = event.target.result;

        if (registerRecord) {
            // Validate and update the RecentInspDate
            //if (recentInspectionDate) {
            //    // Parse the incoming date and store it in ISO 8601 format
            //    var parsedDate = new Date(recentInspectionDate); // Assuming recentInspectionDate is in "YYYY-MM-DD"
            //    var dateObject = new Date(recentInspectionDate);

            //    // Check if date parsing is valid
            //    if (!isNaN(dateObject.getTime())) {
            //        //var jsonDateFormat = "/Date(" + dateObject.getTime() + ")/"; // Convert to /Date(x)/ format
            //        registerRecord.RecentInspDate = dateObject.getTime();
            //    } else {
            //        console.error("Invalid date format:", recentInspectionDate);
            //    }
            //}
            if (recentInspectionDate) {
                // Parse the incoming date, assuming it's in "YYYY-MM-DD" format
                var dateObject = new Date(recentInspectionDate);

                // Check if date parsing is valid
                if (!isNaN(dateObject.getTime())) {
                    // Convert the date to "DD-MMM-YYYY" format
                    var day = dateObject.getDate();
                    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
                    var month = monthNames[dateObject.getMonth()];
                    var year = dateObject.getFullYear();

                    // Create the formatted date string: "05-Oct-2024"
                    var formattedDate = (day < 10 ? '0' + day : day) + '-' + month + '-' + year;

                    // Update the RecentInspDate with the formatted date
                    registerRecord.RecentInspDate = formattedDate;
                } else {
                    console.error("Invalid date format:", recentInspectionDate);
                }
            }


            // Update other fields
            registerRecord.InspectedBy = inspectedBy;
            registerRecord.Status = InspStatus;

            // Put the updated record back into tblRegister
            var updateRequest = registerStore.put(registerRecord);

            updateRequest.onsuccess = function () {
                console.log("Register updated successfully for RegisterId:", registerId);

                // Optional: Immediately fetch and verify the updated record to confirm persistence
                var verifyRequest = registerStore.get(registerId);
                verifyRequest.onsuccess = function (event) {
                    var updatedRecord = event.target.result;
                    console.log("Updated record:", updatedRecord);
                };
            };

            updateRequest.onerror = function () {
                console.log("Error updating register for RegisterId:", registerId);
                OfflineCustomAlert({ Result: "Failed", Message: "Error updating tblRegister for RegisterId: " + registerId });
            };
        } else {
            console.log("Register record not found for RegisterId:", registerId);
            OfflineCustomAlert({ Result: "Failed", Message: "No record found in tblRegister for RegisterId: " + registerId });
        }
    };

    getRequest.onerror = function () {
        console.log("Error fetching register record for RegisterId:", registerId);
        OfflineCustomAlert({ Result: "Failed", Message: "Error fetching record from tblRegister." });
    };
}


function ddlSites_SelectedIndexChanged() {
    populateAreaDropdown();
    populateScaffoldOwnerDropdown();
    populateBuildingDropdown();
    populateFloorLevelDropdown();
    workgroupddl();
    getInspectionList();
    populateBearingDropdown();
}
function ddlArea_SelectedIndexChanged() {
    populateBuildingDropdown();
    populateScaffoldOwnerDropdown();
    populateFloorLevelDropdown();
    workgroupddl();
    getInspectionList();
    populateBearingDropdown();
}
function ddlBuilding_SelectedIndexChanged() {
    populateScaffoldOwnerDropdown();
    populateFloorLevelDropdown();
    workgroupddl();
    getInspectionList();
    populateBearingDropdown();
}
function ddlSubOwner_SelectedIndexChanged() {
    getInspectionList();

}
function ddlFloorLevel_SelectedIndexChanged() {
    /*dropdownSubOwner();*/
    workgroupddl();//Added on 02-August-2022
    getInspectionList();
}
function ddlWorkGroup_SelectedIndexChanged() {
    getInspectionList();
}
function ddlBearing_SelectedIndexChanged() {
    getInspectionList();
}


function msgbuilding() {
    var LocationID = $('#ddlLocaSite').val();
    if (LocationID === 'All' || LocationID === "" || LocationID === null || LocationID === undefined) {
        var response = { Result: "Failed", Message: "Please Select Location Before Selecting Building." };
        OfflineCustomAlert(response);
    }
}

function msgfloorlevel() {
    var BuildingId = $('#ddlBuilding').val();
    if (BuildingId == 'All' || BuildingId == "" || BuildingId == null || BuildingId == undefined) {
        var response = { Result: "Failed", Message: "Please Select Building Before Selecting FloorLevel." }
        OfflineCustomAlert(response);
    }
}

function msglocation() {
    var SiteID = $('#ddlSitesLoc').val();
    if (SiteID == '-1' || SiteID == "" || SiteID == null || SiteID == undefined) {
        var response = { Result: "Failed", Message: "Please Select Site Before Selecting Location." }
        OfflineCustomAlert(response);
    }
}

function uploadDataToServerbtn() {
    /* $('#Uploadtocloudmodel').modal('show');*/
    $('#Uploadtocloudmodel').show();
}

function btnYes() {
    $('#Uploadtocloudmodel').hide();
    uploadDataToServer();
}

function btnNo() {
    $('#Uploadtocloudmodel').hide();

}

function BackupOldInspectionData() {
    $('#LoadingPanel1Foroffline').show();

    var LoginUserId = getParameterByName("UserId");
    var clientId = getParameterByName("ClientId");
    var Siteid = getParameterByName("Siteid");
    if (document.getElementById('ddlSitesLoc').value != -1) {

        var LocalSiteID = $('#ddlSitesLoc').val();
        var encryData = Encrypt([LocalSiteID]);
        LocalSiteID = encryData[0];
        if (!navigator.onLine) {
            alert("You are currently offline. Please go online to upload data.");
            return; // Exit the function if the user is offline
        }
        console.log("User is online, uploading data to server...");

        // Open a transaction on the tblInspection object store
        var transaction = db.transaction(['tblInspection'], 'readonly');
        var objectStore = transaction.objectStore('tblInspection');

        // Initialize an array to store offline records
        var offlineDataArray = [];

        // Open a cursor to iterate over all records
        var cursorRequest = objectStore.openCursor();

        cursorRequest.onsuccess = function (event) {
            var cursor = event.target.result;

            if (cursor) {
                var record = cursor.value;

                if (record.IsOffline === true) {
                    console.log("Offline record found: ", record);

                    offlineDataArray.push(record);
                }

                cursor.continue();
            }
            else {
                if (offlineDataArray.length > 0) {
                    $.ajax({
                        url: '/OfflineOffSystemInspection/BackupOldInspectionData',
                        type: "Post",
                        async: false,
                        data: { offlineDataArray: offlineDataArray, LoginUserId: LoginUserId, clientId: clientId, Siteid: LocalSiteID },
                        success: function (response) {

                            if (response.Result == "Success") {

                                /*alert(response.Message)*/
                                console.log(response.Message);
                            }
                            else {
                                alert(response.Message)

                            }

                        },
                        error: function (error) {
                            //console.error("Error uploading data to the server", error);
                            var Res = { Result: "Failed", Message: error };
                            OfflineCustomAlert(Res);
                        }
                    });

                } else {

                    /*  alert("there is not data for back up ")*/
                    console.log("there is not data for back up");

                }
            }
            $('#LoadingPanel1Foroffline').hide();

        };

        cursorRequest.onerror = function () {
            console.error("Error fetching data from tblInspection.");
            $('#LoadingPanel1Foroffline').hide();

        };
    } else {
        var response = { Result: "Failed", Message: "Please Select Site." };
        OfflineCustomAlert(response);
    }
}
