// Global Variables
var dbName = "scaffpac Job Register";
var dbVersion = 70;
var db;
var GetCount = 0;
var GetclientId;
var dropdownCheck = false;
var UID = getParameterByName('UID');
var Siteid = getParameterByName('Siteid');
var CID = getParameterByName('id');
var RID = getParameterByName('RID');
var GetOfflineStatus = getParameterByName('offlinestatus');
var OfflineStatus = window.atob(GetOfflineStatus);
var ClientID = "";
var UserId = "";
var SiteLocId = "";
var RegId = "";
let dbConnection = null; // Global reference to the active connection
var UserRole = getParameterByName('UserRole');
var OfflineUserRole = getParameterByName('UserRole');


// Document Ready Function
//$(document).ready(function () {


//    $.ajax({

//        type: "POST",
//        url: "/OfflineRegister/GetDecryptClientIDs",
//        data: JSON.stringify({ ids: [UID, Siteid, CID, OfflineUserRole] }),
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        async: false,
//        success: function (response) {
//            // Assuming the response is a dictionary with decrypted values

//            var x = response;
//            var decryptedUID = response[UID];
//            var decryptedSiteid = response[Siteid];
//            var decryptedCID = response[CID];
//            var decryptedRID = response[RID];
//            var decryptedOfflineUserRole = response[OfflineUserRole];

//            ClientID = decryptedCID;
//            SiteLocId = decryptedSiteid;
//            UserId = decryptedUID;
//            OfflineUserRole = decryptedOfflineUserRole;
//            //CreateDbTables(function () {console.log("table test pass") });
//            //ShowOnlineDropDownList(ClientID);
//            //getRegisterList();
//            CreateDbTablesAndCheckData(ClientID);
//        }
//    });
//    // code by s.sourabh 12-11-2024
//    //if ('serviceWorker' in navigator) {
//    //    window.addEventListener('load', () => {
//    //        navigator.serviceWorker
//    //            .register('../sw_cached_pages.js?V=10.0')
//    //            .then(reg => console.log('Service Worker: Registered'))
//    //            .catch(err => console.log('Service Worker: Error: ${err}'));
//    //    });
//    //}

//    if ('serviceWorker' in navigator) {
//        window.addEventListener('load', function () {
//            navigator.serviceWorker.register('../sw_cached_pages.js?V=22.0')
//                .then(function (registration) {
//                    console.log('Service Worker registered with scope:', registration.scope);
//                })
//                .catch(function (err) {
//                    console.log('Service Worker registration failed:', err);
//                });
//        });
//    }
//});

//function CreateDbTablesAndCheckData(ClientID) {
//    $("#customLoader").css("display", "block");

//    const request = indexedDB.open(dbName, dbVersion);

//    request.onsuccess = function (event) {
//        const db = event.target.result;

//        // Check if the tblRegisterDetails table exists and handle based on the data in it
//        if (db.objectStoreNames.contains("tblRegisterDetails")) {
//            const transaction = db.transaction("tblRegisterDetails", "readwrite");
//            const store = transaction.objectStore("tblRegisterDetails");

//            const getAllRequest = store.getAll();
//            getAllRequest.onsuccess = function () {
//                const data = getAllRequest.result;

//                // Handle data presence or absence
//                if (data && data.length > 0) {
//                    console.log("Data exists in tblRegisterDetails. Fetching register list...");
//                    getRegisterList();
//                } else {
//                    console.log("No data in tblRegisterDetails. Clearing and loading dropdown...");
//                    clearAllTables(() => {
//                        CreateDbTables(() => {
//                            console.log("Tables created successfully. Loading dropdowns...");
//                            ShowOnlineDropDownList(ClientID);
//                            getRegisterList();
//                        });
//                    });
//                }

//                $("#customLoader").css("display", "none");  // Hide loader in both cases
//            };

//            getAllRequest.onerror = function (error) {
//                console.error("Error reading tblRegisterDetails:", error);
//                $("#customLoader").css("display", "none");  // Hide loader in case of error
//            };

//        } else {
//            console.log("tblRegisterDetails not found. Creating tables...");
//            CreateDbTables(() => {
//                console.log("Tables created successfully.");
//                ShowOnlineDropDownList(ClientID);
//                getRegisterList();
//                $("#customLoader").css("display", "none");
//            });
//        }
//    };

//    request.onupgradeneeded = function () {
//        console.log("Upgrade needed. Creating tables...");
//        CreateDbTables();
//    };

//    request.onerror = function (error) {
//        console.error("Error opening IndexedDB:", error);
//        $("#customLoader").css("display", "none");  // Hide loader in case of error
//    };
//}

// Added by Pooja Rai

function setupDb() {
    //showLoadingMessage();

    return new Promise((resolve, reject) => {
        const request = indexedDB.open(dbName, dbVersion);
        request.onupgradeneeded = (event) => {
            db = event.target.result;
            CreateDbTables(db);
            //alert('Creating tables...');
            //CreateDbTables(() => {
            //    console.log("Tables created successfully.");
            //    ShowOnlineDropDownList(ClientID);
            //    getRegisterList();
            //});
        };
        request.onsuccess = (event) => {
            db = event.target.result;
            resolve(db);
            //hideLoadingMessage();
            //populateUserDropdown();

        };
        request.onerror = reject;
        //hideLoadingMessage();
    });
}
$(document).ready(function () {
    $("#customLoader").css("display", "block");
    initApplication();
    //sendIndexedDBDataToServer(dbName, dbVersion);

});

async function initApplication() {
    try {
        const decryptedData = await fetchDecryptedClientIDs([UID, Siteid, CID, OfflineUserRole, RID]);

        // Extract decrypted values
        const { [UID]: decryptedUID, [Siteid]: decryptedSiteid, [CID]: decryptedCID, [RID]: decryptedRID, [OfflineUserRole]: decryptedOfflineUserRole } = decryptedData;

        ClientID = decryptedCID;
        SiteLocId = decryptedSiteid;
        UserId = decryptedUID;
        OfflineUserRole = decryptedOfflineUserRole;
        RegId = decryptedRID;

        try {
            const response = await $.ajax({
                type: 'POST',
                url: '/OfflineRegister/checkOfflineJobClearedDate',
                data: JSON.stringify({ id: CID, UID: UID, SiteId: Siteid }),
                contentType: "application/json; charset=utf-8",
                dataType: "json"
            });

            const serverOfflineJobClearedOn = new Date(response.formattedOfflineJobOn);
            const db = await setupDb();

            if (db.objectStoreNames.contains("tblRegisterDetails")) {
                const transaction = db.transaction("tblRegisterDetails", "readonly");
                const store = transaction.objectStore("tblRegisterDetails");

                const data = await getAllData(store);
                if (data && data.length > 0) {
                    console.log("Data exists in tblRegisterDetails. Fetching register list...");
                    //alert('Data exists in tblRegisterDetails. Fetching register list...');
                    await checkOfflineJobClearedDate(db, RegId, serverOfflineJobClearedOn);
                } else {
                    console.log("No data in tblRegisterDetails. Clearing and loading dropdown...");
                    //alert('No data in tblRegisterDetails. Clearing and loading dropdown...');
                    await loadDataFromServerDb();
                    await verifyDataLoading();
                }
            } else {
                console.log("tblRegisterDetails not found. Creating tables...");
                //alert('tblRegisterDetails not found. Creating tables...');
                await loadDataFromServerDb();
                await verifyDataLoading();
            }

        } catch (error) {
                console.error("Error during initialization:", error);
        } finally {
                //hideLoadingMessage();
        }

        // Initialize tables and data checks
        //await createDbTablesAndCheckData(ClientID);

        // Register service worker
        registerServiceWorker('../sw_cached_pages.js?V=54.0');
    } catch (error) {
        console.error("Error during application initialization:", error);
    }
}

async function verifyDataLoading() {
    const db = await setupDb();
    const transaction = db.transaction(['tblRegisterDetails'], 'readonly');
    const objectStore = transaction.objectStore('tblRegisterDetails');

    const recordCount = await new Promise((resolve, reject) => {
        const countRequest = objectStore.count();
        countRequest.onsuccess = () => resolve(countRequest.result);
        countRequest.onerror = reject;
    });

    if (recordCount > 0) {
        console.log("Data successfully loaded into tblRegisterDetails:", recordCount);
        //alert('Data successfully loaded into tblRegisterDetails');
    } else {
        console.warn("Data not loaded into tblRegisterDetails as expected.");
        //alert('Data not loaded into tblRegisterDetails as expected.');
    }
}

function loadDataFromServerDb() {
    return new Promise((resolve) => {
        //alert('Getting data from server & redirecting to job');
        ShowOnlineDropDownList(ClientID);
        getRegisterList();
            resolve();
        });
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

//async function createDbTablesAndCheckData(clientID) {
//    $("#customLoader").css("display", "block");

//    const request = indexedDB.open(dbName, dbVersion);

//    request.onsuccess = async function (event) {
//        const db = event.target.result;

//        // Check if the tblRegisterDetails table exists and handle based on the data in it
//        if (db.objectStoreNames.contains("tblRegisterDetails")) {
//            const transaction = db.transaction("tblRegisterDetails", "readwrite");
//            const store = transaction.objectStore("tblRegisterDetails");

//            const data = await getAllData(store);
//            if (data && data.length > 0) {
//                console.log("Data exists in tblRegisterDetails. Fetching register list...");
//                await checkOfflineJobClearedDate(db, data[0].REGISTERId);
//            } else {
//                console.log("No data in tblRegisterDetails. Clearing and loading dropdown...");
//                clearAllTables(() => {
//                    CreateDbTables(() => {
//                        console.log("Tables created successfully. Loading dropdowns...");
//                        ShowOnlineDropDownList(clientID);
//                        getRegisterList();

//                    });
//                });
//            }
//            //$("#customLoader").css("display", "none");
//        } else {
//            console.log("tblRegisterDetails not found. Creating tables...");
//            CreateDbTables(() => {
//                ShowOnlineDropDownList(clientID);
//                getRegisterList();
//                //$("#customLoader").css("display", "none");
//            });
//        }
//    };

//    request.onupgradeneeded = function () {
//        console.log("Upgrade needed. Creating tables...");
//        CreateDbTables();
//    };

//    request.onerror = function (error) {
//        console.error("Error opening IndexedDB:", error);
//        $("#customLoader").css("display", "none");
//    };
//}

async function getAllData(store) {
    return new Promise((resolve, reject) => {
        const request = store.getAll();
        request.onsuccess = () => resolve(request.result);
        request.onerror = (error) => reject(error);
    });
}

async function checkOfflineJobClearedDate(db, localREGISTERId, serverOfflineJobClearedOn) {
    const transaction = db.transaction(['tblRegisterDetails'], 'readonly');
    const objectStore = transaction.objectStore('tblRegisterDetails');

    let matchFound = false;
    let record;

    const cursorRequest = objectStore.openCursor();

    cursorRequest.onsuccess = async function (event) {
        const cursor = event.target.result;
        if (cursor) {
            record = cursor.value;

            if (parseInt(localREGISTERId, 10) === record.REGISTERId) {
                await compareGoOfflineJobAndClearedDates(db, serverOfflineJobClearedOn);
                matchFound = true;
            } else {
                //const message = "There is already data in your local database.\n\n1. Click OK to clear local data\n2. Click Cancel to continue on previous data";
                //if (confirm(message)) {
                //    clearAllTables();
                //    CreateDbTables(() => {
                //        console.log("Tables created successfully.");
                //        ShowOnlineDropDownList(ClientID);
                //        getRegisterList();
                //    });
                //} else {
                //    getRegisterList();
                //}
                $("#customLoader").css("display", "none");
                $('#clearLocalDataModal').show();
            }
            cursor.continue();
        } 
    }
    
}

async function compareGoOfflineJobAndClearedDates(db, serverOfflineJobClearedOn) {
    const transaction = db.transaction(['tblRegisterDetails'], 'readonly');
    const store = transaction.objectStore('tblRegisterDetails');

    let goOfflineJobOnLatest = null;

    const cursorRequest = store.openCursor();
    cursorRequest.onsuccess = function (event) {
        const cursor = event.target.result;
        if (cursor) {
            const record = cursor.value;
            const goOfflineJobOn = new Date(record.GoOfflineJobOn);
            if (!goOfflineJobOnLatest || goOfflineJobOn > goOfflineJobOnLatest) {
                goOfflineJobOnLatest = goOfflineJobOn;
                goOfflineJobOnLatest = new Date(goOfflineJobOnLatest);
            }
            //cursor.continue();
        }
    };

    transaction.oncomplete = function () {
        if (serverOfflineJobClearedOn > goOfflineJobOnLatest) {
            alert("ON LAST SESSION YOU DID NOT UPLOAD SAVED OFFLINE JOB TO THE CLOUD! LAST SESSION DATA NOW CANCELLED.\n\nFor a copy of the Job: Contact Scafpac Admin at: admin@scafpac.com.au .");
            clearAllTables();
            $('#tblOfflineregisterlist').DataTable().clear().destroy();
            $("#customLoader").css("display", "none");
            //window.location.href = '/RegisterList/RegisterList?UID=' + UID + '&UserRole=' + UserRole + '&id=' + CID ;
            updateOfflineCurrent();
            window.location.href = '/Register/Register?UID=' + UID + '&UserRole=' + UserRole + '&id=' + CID + '&RID=' + RID;
            
        } else {
            alert("YOU DID NOT UPLOAD SAVED JOB AT THE END OF LAST SESSION!\n\nRE-DIRECTING YOU TO THE SAVED JOB FOR UPLOAD TO CLOUD.");
            getRegisterList();
        }
    };
}

function BtnClearLocalDataClickyes() {

    $('#clearLocalDataModal').hide();
    $("#customLoader").css("display", "block");
    clearAllTables(() => {
        loadDataFromServerDb();
    });
    //clearAllTables(() => {
    //CreateDbTables(() => {
    //    console.log("Tables created successfully.");
    //    ShowOnlineDropDownList(ClientID);
    //    getRegisterList();

    //    });
    //});
    
}
function BtnClearLocalDataClickno() {
    $('#clearLocalDataModal').hide();
    $("#customLoader").css("display", "block");
    //getRegisterList();
    alert("ON LAST SESSION YOU DID NOT UPLOAD SAVED OFFLINE JOB TO THE CLOUD! LAST SESSION DATA NOW CANCELLED.\n\nFor a copy of the Job: Contact Scafpac Admin at: admin@scafpac.com.au .");
    clearAllTables();
    $('#tblOfflineregisterlist').DataTable().clear().destroy();
    $("#customLoader").css("display", "none");
    updateOfflineCurrent();
    window.location.href = '/Register/Register?UID=' + UID + '&UserRole=' + UserRole + '&id=' + CID + '&RID=' + RID;
}

function updateOfflineCurrent() {
    try {
        $.ajax({
            type: 'POST',
            url: '/OfflineRegister/UpdateOfflineInspectionFlag',
            data: JSON.stringify({ LoginUserId: UserId, clientId: ClientID, Siteid: SiteLocId, UserRole: OfflineUserRole, RID: RegId }),
            contentType: "application/json; charset=utf-8",
            dataType: "json"

        });
    } catch (error) {
        console.error("Error to update offlineCurrent job:", error);
    }
}
// End by Pooja Rai

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

//function CreateDbTablesAndCheckData(ClientID) {
//    $("#customLoader").css("display", "block");

//    const request = indexedDB.open(dbName, dbVersion);

//    request.onsuccess = function (event) {
//        const db = event.target.result;

//        // Check if the tblRegisterDetails table exists
//        if (db.objectStoreNames.contains("tblRegisterDetails")) {
//            const transaction = db.transaction("tblRegisterDetails", "readwrite");
//            const store = transaction.objectStore("tblRegisterDetails");

//            // Check if data exists in the table
//            const getAllRequest = store.getAll();

//            getAllRequest.onsuccess = function () {
//                const data = getAllRequest.result;

//                if (data && data.length > 0) {
//                    console.log("Data exists in tblRegisterDetails. Fetching register list...");
//                    getRegisterList();
//                    $("#customLoader").css("display", "none");

//                } else {
//                    console.log("No data in tblRegisterDetails. Clearing and loading dropdown...");
//                    clearAllTables(function () {
//                        CreateDbTables(function () {
//                            console.log("Tables created successfully. Loading dropdowns...");
//                            ShowOnlineDropDownList(ClientID);
//                            getRegisterList();
//                            $("#customLoader").css("display", "none");

//                        });
//                    });
//                }
//            };
            

//            getAllRequest.onerror = function (error) {
//                console.error("Error reading tblRegisterDetails:", error);
//            };
//        } else {
//            console.log("tblRegisterDetails not found. Creating tables...");
//            CreateDbTables(function () {
//                console.log("Tables created successfully.");
//                ShowOnlineDropDownList(ClientID);
//                getRegisterList();
//                $("#customLoader").css("display", "none");

//            });
           
//        }
//    };

//    request.onupgradeneeded = function (event) {
//        console.log("Upgrade needed. Creating tables...");
//        CreateDbTables();
        
//    };

//    request.onerror = function (error) {
//        console.error("Error opening IndexedDB:", error);
//    };
//}
function closeDbConnection(dbName, dbVersion, callback) {
    const request = indexedDB.open(dbName, dbVersion);

    request.onsuccess = function (event) {
        const db = event.target.result;

        if (db) {
            console.log(`Closing database connection for ${dbName}, version ${dbVersion}.`);
            db.close();
            callback && callback(true); 
        } else {
            console.warn(`No open connection found for ${dbName}.`);
            callback && callback(false);
        }
    };

    request.onerror = function (event) {
        console.error(`Error while checking database connection for ${dbName}:`, event.target.errorCode);
        callback && callback(false);
    };
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

function CreateDbTables(db) {
    // tblSiteList
    if (!db.objectStoreNames.contains('tblSiteList')) {
        let objectStore = db.createObjectStore('tblSiteList', { keyPath: 'SiteLocID' });
        console.log('tblSiteList table created');
    }

    // tblDutyRating
    if (!db.objectStoreNames.contains('tblDutyRating')) {
        let objectStore = db.createObjectStore('tblDutyRating', { keyPath: 'id', autoIncrement: true });
        objectStore.createIndex('ClientDutyRating', ['ClientID', 'DutyRating'], { unique: false });
        console.log('tblDutyRating table created');
    }

    // tblMaterialType
    if (!db.objectStoreNames.contains('tblMaterialType')) {
        let objectStore = db.createObjectStore('tblMaterialType', { keyPath: 'id', autoIncrement: true });
        objectStore.createIndex('ClientMaterialType', ['ClientID', 'MaterialType'], { unique: false });
        console.log('tblMaterialType table created');
    }

    // tblCheckList
    if (!db.objectStoreNames.contains('tblCheckList')) {
        let objectStore = db.createObjectStore('tblCheckList', { keyPath: 'checklistID' });
        console.log('tblCheckList table created');
    }

    // tblDesignList
    if (!db.objectStoreNames.contains('tblDesignList')) {
        let objectStore = db.createObjectStore('tblDesignList', { keyPath: 'id', autoIncrement: true });
        objectStore.createIndex('ClientDesignType', ['ClientID', 'DesignType'], { unique: false });
        console.log('tblDesignList table created');
    }

    // tblLocationList
    if (!db.objectStoreNames.contains('tblLocationList')) {
        let objectStore = db.createObjectStore('tblLocationList', { keyPath: 'areaid' });
        objectStore.createIndex('SiteLocId', 'SiteLocId', { unique: false });
        console.log('tblLocationList table created');
    }

    // tblBuildingList
    if (!db.objectStoreNames.contains('tblBuildingList')) {
        let objectStore = db.createObjectStore('tblBuildingList', { keyPath: 'BuildingID' });
        objectStore.createIndex('areaid', 'areaid', { unique: false });
        objectStore.createIndex('ClientSiteIndex', ['ClientID', 'SiteLocID'], { unique: false });
        console.log('tblBuildingList table created');
    }

    // tblAssetList
    if (!db.objectStoreNames.contains('tblAssetList')) {
        let objectStore = db.createObjectStore('tblAssetList', { keyPath: 'Assetid' });
        objectStore.createIndex('Assetno', 'Assetno', { unique: false });
        objectStore.createIndex('BuildingID', 'BuildingID', { unique: false });
        objectStore.createIndex('BuildNo', 'BuildNo', { unique: false });
        objectStore.createIndex('ClientBuildingID', ['ClientID', 'BuildingID'], { unique: false });
        objectStore.createIndex('ClientSiteIndex', ['ClientID', 'SiteLocID'], { unique: false });
        console.log('tblAssetList table created');
    }

    // tblRegisterDetails
    if (!db.objectStoreNames.contains('tblRegisterDetails')) {
        let objectStore = db.createObjectStore('tblRegisterDetails', { keyPath: 'REGISTERId' });
        console.log('tblRegisterDetails table created');
    }

    // tblSupervisor
    if (!db.objectStoreNames.contains('tblSupervisor')) {
        let objectStore = db.createObjectStore('tblSupervisor', { keyPath: 'UserID' });
        console.log('tblSupervisor table created');
    }

    // tblScaffoldOwner
    if (!db.objectStoreNames.contains('tblScaffoldOwner')) {
        let objectStore = db.createObjectStore('tblScaffoldOwner', { keyPath: 'id', autoIncrement: true });
        console.log('tblScaffoldOwner table created');
    }

    // tblSubOwnerRep
    if (!db.objectStoreNames.contains('tblSubOwnerRep')) {
        let objectStore = db.createObjectStore('tblSubOwnerRep', { keyPath: 'id', autoIncrement: true });
        console.log('tblSubOwnerRep table created');
    }

    // tblFloorLevel
    if (!db.objectStoreNames.contains('tblFloorLevel')) {
        let objectStore = db.createObjectStore('tblFloorLevel', { keyPath: 'FloorLevelId' });
        objectStore.createIndex('ClientID', 'ClientID', { unique: false });
        objectStore.createIndex('SiteLocId', 'SiteLocId', { unique: false });
        objectStore.createIndex('LocationID', 'LocationID', { unique: false });
        objectStore.createIndex('BuildingID', 'BuildingID', { unique: false });
        objectStore.createIndex('Category', 'Category', { unique: false });
        objectStore.createIndex('SiteLocId_LocationID_BuildingID', ['SiteLocId', 'LocationID', 'BuildingID'], { unique: false });
        objectStore.createIndex('ClientSiteIndex', ['ClientID', 'SiteLocID'], { unique: false });
        console.log('tblFloorLevel table created');
    }

    // tblWorkGroups and tblSubGroups
    for (let tableName of ['tblWorkGroups', 'tblSubGroups']) {
        if (!db.objectStoreNames.contains(tableName)) {
            let objectStore = db.createObjectStore(tableName, { keyPath: 'id', autoIncrement: true });
            objectStore.createIndex('ClientSiteIndex', ['ClientID', 'SiteLocID'], { unique: false });
            console.log(`${tableName} table created`);
        }
    }

    // tblInspectionDetails
    if (!db.objectStoreNames.contains('tblInspectionDetails')) {
        let objectStore = db.createObjectStore('tblInspectionDetails', { keyPath: 'InspectionID', autoIncrement: true });
        console.log('tblInspectionDetails table created');
    }

    // tblUserList
    if (!db.objectStoreNames.contains('tblUserList')) {
        let objectStore = db.createObjectStore('tblUserList', { keyPath: 'UserId', autoIncrement: true });
        objectStore.createIndex('UserId', 'UserId', { unique: false });
        console.log('tblUserList table created');
    }

    // tblcheckListContentList
    if (!db.objectStoreNames.contains('tblcheckListContentList')) {
        let objectStore = db.createObjectStore('tblcheckListContentList', { keyPath: 'ContentID', autoIncrement: true });
        console.log('tblcheckListContentList table created');
    }

    // tblInspectionListDetails
    if (!db.objectStoreNames.contains('tblInspectionListDetails')) {
        let objectStore = db.createObjectStore('tblInspectionListDetails', { keyPath: 'InspectionListID', autoIncrement: true });
        objectStore.createIndex('InspectionID', 'InspectionID', { unique: false });
        objectStore.createIndex('ContentID', 'ContentID', { unique: false });
        objectStore.createIndex('CheckListID', 'CheckListID', { unique: false });
        objectStore.createIndex('InspectionID_ContentID_CheckListID',
            ['InspectionID', 'ContentID', 'CheckListID'],
            { unique: false }
        );
        console.log('tblInspectionListDetails table created');
    }

    // tblRegisterGallery
    if (!db.objectStoreNames.contains('tblRegisterGallery')) {
        let objectStore = db.createObjectStore('tblRegisterGallery', { keyPath: 'ID', autoIncrement: true });
        console.log('tblRegisterGallery table created');
    }

    // tblClient
    if (!db.objectStoreNames.contains('tblClient')) {
        let objectStore = db.createObjectStore('tblClient', { keyPath: 'ClientId' });
        console.log('tblClient table created');
    }

    // tblPipelineDetails
    if (!db.objectStoreNames.contains('tblPipelineDetails')) {
        let objectStore = db.createObjectStore('tblPipelineDetails', { keyPath: 'InvDetailId', autoIncrement: true });
        objectStore.createIndex('REGISTERId', 'REGISTERId', { unique: false });
        console.log('tblPipelineDetails table created');
    }

    // tblPipelineRequest
    if (!db.objectStoreNames.contains('tblPipelineRequest')) {
        let objectStore = db.createObjectStore('tblPipelineRequest', { keyPath: 'InvRequestId', autoIncrement: true });
        objectStore.createIndex('InventoryId', 'InventoryId', { unique: false });
        console.log('tblPipelineRequest table created');
    }
    if (!db.objectStoreNames.contains('tblRequestedBy')) {
        let objectStore = db.createObjectStore('tblRequestedBy', { keyPath: 'id', autoIncrement: true });
        console.log('tblClient table created');
    }

}



//function CreateDbTables(callback) {
//    indexedDB.deleteDatabase(dbName);

//    console.log(`Opening database: ${dbName} (version ${dbVersion})`);
//    const request = indexedDB.open(dbName, dbVersion);

//    request.onupgradeneeded = function (event) {
//        const db = event.target.result;
//        console.log(`Database upgrade to version ${db.version} in progress...`);

//        const stores = [
//            { name: "tblSiteList", keyPath: "SiteLocID" },
//            { name: "tblDutyRating", keyPath: "id", autoIncrement: true },
//            { name: "tblMaterialType", keyPath: "id", autoIncrement: true },
//            { name: "tblCheckList", keyPath: "checklistID" },
//            { name: "tblDesignList", keyPath: "id", autoIncrement: true },
//            { name: "tblLocationList", keyPath: "areaid" },
//            { name: "tblBuildingList", keyPath: "BuildingID" },
//            { name: "tblAssetList", keyPath: "Assetid" },
//            { name: "tblRegisterDetails", keyPath: "REGISTERId" },
//            { name: "tblSupervisor", keyPath: "UserID" },
//            { name: "tblScaffoldOwner", keyPath: "id", autoIncrement: true },
//            { name: "tblSubOwnerRep", keyPath: "id", autoIncrement: true },
//            { name: "tblFloorLevel", keyPath: "FloorLevelId" },
//            { name: "tblWorkGroups", keyPath: "id", autoIncrement: true },
//            { name: "tblSubGroups", keyPath: "id", autoIncrement: true },
//            { name: "tblInspectionDetails", keyPath: "InspectionID", autoIncrement: true },
//            { name: "tblUserList", keyPath: "UserId", autoIncrement: true },
//            { name: "tblcheckListContentList", keyPath: "ContentID", autoIncrement: true },
//            { name: "tblInspectionListDetails", keyPath: "InspectionListID", autoIncrement: true },
//            { name: "tblRegisterGallery", keyPath: "ID", autoIncrement: true },
//            { name: "tblClient", keyPath: "ClientId" },

//			 { name: "tblPipelineDetails", keyPath: "InvDetailId", autoIncrement: true },
//            { name: "tblPipelineRequest", keyPath: "InvRequestId", autoIncrement: true }
//        ];

//        stores.forEach(store => {
//            if (db.objectStoreNames.contains(store.name)) {
//                console.log(`Deleting existing store: ${store.name}`);
//                db.deleteObjectStore(store.name);
//            }

//            const objectStore = db.createObjectStore(store.name, {
//                keyPath: store.keyPath,
//                autoIncrement: store.autoIncrement || false
//            });

//            if (store.name === "tblLocationList") {
//                objectStore.createIndex("SiteLocId", "SiteLocId", { unique: false });
//            }
//            if (store.name === "tblBuildingList") {
//                objectStore.createIndex("areaid", "areaid", { unique: false });
//            }
//            if (store.name === "tblAssetList") {
//                objectStore.createIndex("Assetno", "Assetno", { unique: false });
//                objectStore.createIndex("BuildingID", "BuildingID", { unique: false });
//                objectStore.createIndex("BuildNo", "BuildNo", { unique: false });
//            }
//            if (store.name === "tblFloorLevel") {
//                objectStore.createIndex("ClientID", "ClientID", { unique: false });
//                objectStore.createIndex("SiteLocId", "SiteLocId", { unique: false });
//                objectStore.createIndex("LocationID", "LocationID", { unique: false });
//                objectStore.createIndex("BuildingID", "BuildingID", { unique: false });
//                objectStore.createIndex("Category", "Category", { unique: false });
//                objectStore.createIndex(
//                    "SiteLocId_LocationID_BuildingID",
//                    ["SiteLocId", "LocationID", "BuildingID"],
//                    { unique: false }
//                );
//            }
//            if (["tblBuildingList", "tblAssetList", "tblFloorLevel", "tblSubGroups", "tblWorkGroups"].includes(store.name)) {
//                objectStore.createIndex("ClientSiteIndex", ["ClientID", "SiteLocID"], { unique: false });
//            }
//            if (store.name === "tblDesignList") {
//                objectStore.createIndex("ClientDesignType", ["ClientID", "DesignType"], { unique: false });
//            }
//            if (store.name === "tblMaterialType") {
//                objectStore.createIndex("ClientMaterialType", ["ClientID", "MaterialType"], { unique: false });
//            }
//            if (store.name === "tblDutyRating") {
//                objectStore.createIndex("ClientDutyRating", ["ClientID", "DutyRating"], { unique: false });
//            }
//            if (store.name === "tblAssetList") {
//                objectStore.createIndex("ClientBuildingID", ["ClientID", "BuildingID"], { unique: false });
//            };
//            if (store.name === "tblInspectionListDetails") {
//                objectStore.createIndex("InspectionID", "InspectionID", { unique: false });
//                objectStore.createIndex("ContentID", "ContentID", { unique: false });
//                objectStore.createIndex("CheckListID", "CheckListID", { unique: false });
//                objectStore.createIndex(
//                    "InspectionID_ContentID_CheckListID",
//                    ["InspectionID", "ContentID", "CheckListID"],
//                    { unique: false }
//                );
//            }
//            if (store.name === "tblPipelineDetails") {
//                objectStore.createIndex("REGISTERId", "REGISTERId", { unique: false });
//            }
//            if (store.name === "tblPipelineRequest") {
//                objectStore.createIndex("InventoryId", "InventoryId", { unique: false });
//            }
            
//             if (store.name === "tblUserList") {
//                objectStore.createIndex("UserId", "UserId", { unique: false });
//            }
            
//        });

//        console.log("All object stores created successfully!");
//    };

//    request.onsuccess = function (event) {
//        console.log("Database opened successfully!");
//        const db = event.target.result;
//        db.close(); // Close the connection after creation

//        if (callback) callback(true); // Notify success via callback
//    };

//    request.onerror = function (event) {
//        console.error("Error opening database:", event.target.errorCode);
//        if (callback) callback(false);
//    };
//}


function clearAllTables(callback) {

    const stores = [
        "tblSiteList", "tblDutyRating", "tblMaterialType", "tblCheckList", "tblDesignList",
        "tblLocationList", "tblBuildingList", "tblAssetList", "tblRegisterDetails", "tblSupervisor",
        "tblScaffoldOwner", "tblSubOwnerRep", "tblFloorLevel", "tblWorkGroups", "tblSubGroups", "tblInspectionDetails", "tblUserList", "tblcheckListContentList", "tblInspectionListDetails", "tblRegisterGallery", "tblClient", "tblPipelineRequest", "tblPipelineDetails","tblRequestedBy"
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
                        //location.reload(true);

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


// Function to fetch data and insert into IndexedDB
function ShowOnlineDropDownList(ClientID) {
    $("#customLoader").css("display", "block");

    var UID = getParameterByName('UID');
    var Siteid = getParameterByName('Siteid');

    $.ajax({
        //type: "POST",
        //url: "/OfflineRegister/BindallDropdown",
        //data: JSON.stringify({ id: ClientID, Siteid: Siteid, RID: RID }),
        //contentType: "application/json; charset=utf-8",
        //dataType: "json",
        //async: false,

        url: '/OfflineRegister/BindallDropdown',
        type: 'get',
        async: false, 
        data: { id: ClientID, Siteid: Siteid, RID: RID },
        success: function (response) {
            var AllList = response[0];
            if (AllList.GetRegisterListData && AllList.GetRegisterListData.length > 0) {

                insertIntoIndexedDB('tblDesignList', AllList.GetDesignList);
                insertIntoIndexedDB('tblDutyRating', AllList.GetDutyRating);
                insertIntoIndexedDB('tblFloorLevel', AllList.GetFloorLevelList);
                insertIntoIndexedDB('tblMaterialType', AllList.GetMaterialType);
                insertIntoIndexedDB('tblSiteList', AllList.GetSiteList);
                insertIntoIndexedDB('tblCheckList', AllList.GetCheckListData);
                insertIntoIndexedDB('tblLocationList', AllList.GetAllLocationListData);
                insertIntoIndexedDB('tblBuildingList', AllList.GetAllBuildingListData);
                insertIntoIndexedDB('tblAssetList', AllList.GetAllAssetListData);
                insertIntoIndexedDB('tblSupervisor', AllList.GetSupervisorData);
                insertIntoIndexedDB('tblScaffoldOwner', AllList.GetScaffoldOwnerData);
                insertIntoIndexedDB('tblSubOwnerRep', AllList.GetSubOwnerRepData);
                insertIntoIndexedDB('tblWorkGroups', AllList.GetWorkGroupsData);
                insertIntoIndexedDB('tblSubGroups', AllList.GetSubGroupsData);
                insertIntoIndexedDB('tblRegisterDetails', AllList.GetRegisterListData);
                insertIntoIndexedDB('tblInspectionDetails', AllList.GetInspectionListData);
                insertIntoIndexedDB('tblUserList', AllList.GetUserListData);
            insertIntoIndexedDB('tblcheckListContentList', AllList.GetcheckListContentData);
                insertIntoIndexedDB('tblInspectionListDetails', AllList.GetInspectionListDetailsData);
                insertIntoIndexedDB('tblRegisterGallery', AllList.GetRegisterGalleryData);
            insertIntoIndexedDB('tblClient', AllList.GetClientData);
             insertIntoIndexedDB('tblPipelineRequest', AllList.GetInventoryRequests);
                insertIntoIndexedDB('tblPipelineDetails', AllList.GetInventoryListdropdowns);
                insertIntoIndexedDB('tblRequestedBy', AllList.GetRequestedByData);
                $("#customLoader").css("display", "none");

            }
            else {
                alert("No data available in offline mode.");
                $("#customLoader").css("display", "none");

            }
        }
       
    });
    $("#customLoader").css("display", "none");

}



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


function getRegisterList() {
    var dbPromise = indexedDB.open(dbName, dbVersion);

    dbPromise.onsuccess = function (event) {
        db = event.target.result;

        var transaction = db.transaction(["tblRegisterDetails"], "readonly");
        var objectStore = transaction.objectStore("tblRegisterDetails");
        var result = [];

        // Open cursor to iterate over records
        var cursorRequest = objectStore.openCursor();
        cursorRequest.onsuccess = function (event) {
            var cursor = event.target.result;
            if (cursor) {
                var record = cursor.value;

                
                result.push(record);
                cursor.continue();
            } else {
                // Process and bind the results after cursor completes
                if (result.length === 0) {
                    console.log("No records found.");
                } else {
                    console.log("Records found:", result);
                    processAndBindData(result);
                }
            }
        };

        cursorRequest.onerror = function (event) {
            console.error("Cursor request failed", event);
        };
    }
}

function processAndBindData(data) {
    var str = "";
    for (var i = 0; i < data.length; i++) {
        var record = data[i];
        var RegisterId = record.REGISTERId || "";
        var ShowRegisterID = record.ShowRegisterID || "";
        var ClientID = record.ClientID || "";
        var Status = record.Status || "";
        var ScaffRef = record.ScaffRef || "";
        var ClientScaffref = record.ClientScaffref || "";
        var WONumber = record.WONumber || "";
        var Location = record.Location || "";
        var BuildingNo = record.BuildingNo || "";
        var FloorLevel = record.FloorLevel || "";
        var Description1 = record.Description || "";
        var Description = (Description1.length > 50) ? Description1.substring(0, 50) + '.....' : Description1;
        var AssetNo = record.AssetNo || "";
        var DateTagged = record.DateTagged || "";
        var DaysSinceLastInsp = record.DaysSinceLastInsp || "";
        var Tonnage = record.Tonnage || "";
        var DesignType = record.DesignType || "";
        var MaterialType = record.MaterialType || "";
        var DutyRating = record.DutyRating || "";
        var DateOFDismantle = record.DateOFDismantle || "";
        var InspectedBy = record.InspectedBy || "";
        var HireDaysToDate = record.HireDaysToDate || "";
        var TotalHireDays = record.TotalHireDays || "";
        var SAPNumber = record.SAPNumber || "";
        var ScafType = record.ScafType || "";
        var ClientRef = record.ClientRef || "";
        var OwnerName = record.OwnerName || "";
        var SiteLocation = record.SiteLocation || "";
        var ScaffoldOwner = record.ScaffoldOwner || "";
        var WorkGroup = record.WorkGroup || "";
        var SubGroup = record.SubGroup || "";
        var discipline = record.discipline || "";
        var ManHrsErect = record.ManHrsErect || "";
        var ManHrsDismantle = record.ManHrsDismantle || "";
        var ShiftAllocation = record.ShiftAllocation || "";
        var CreatedBy = record.CreatedBy || "";
        var CreatedOn = record.CreatedOn || "";
        var UpdatedBy = record.UpdatedBy || "";
        var UpdatedOn = record.UpdatedOn || "";
        var Length = record.Length || "";
        var Width = record.Width || "";
        var Height = record.Height || "";
        var SiteLocationStatus = record.SiteLocationStatus || "";
        var LongTermStructure = record.LongTermStructure ? "Yes" : "";
        var ChkRequiresIsolation = record.ChkRequiresIsolation ? "Yes" : "";
        var LocationId = record.LocationId || "";
        var SiteLocID = record.SiteLocID || "";
        var Bearing = record.Bearing || "";
        
        var FloorLevelId = record.FloorLevelId || "";
        var LockedIn = record.LockedIn ? "Yes" : "No";

        str += "<tr>";
        str += "<td class='setWidth0' style='display:none'></td>";
        str += "<td class='setWidth2' style='text-align: center;' title='Click here to edit Job.' style='cursor:pointer;'><a id='tbnEdit' class='btnJobEdit' tbnEditRegisterId_='" + RegisterId + "' ><i class='fa fa-pencil'></i>&nbsp;</a></td>";

        if (["RED", "GREEN", "YELLOW"].includes(Status.toUpperCase())) {
            str += "<td class='setWidth1' style='text-align: center;'><img src='/images/" + Status + ".jpg' class='img-responsive' style='height: 22px; width: 22px; display: block; margin: auto;'/></td>";
        } else {
            str += "<td class='setWidth1' style='text-align: center;'>" + Status + "</td>";
        }

        str += "<td class='setWidth3' style='display:none;'>" + Status + "</td>"; // For Export Column Functionality 
        str += "<td class='setWidth4'style='text-align: center;'>" + ShowRegisterID + "</td>";
        str += "<td class='setWidth5'style='text-align: center;'>" + ClientScaffref + "</td>";
        str += "<td class='setWidth6'>" + WONumber + "</td>";
        str += "<td class='setWidth7'>" + Location + "</td>";
        str += "<td class='setWidth8'>" + BuildingNo + "</td>";
        str += "<td class='setWidth9'style='text-align: center;'>" + FloorLevel + "</td>";
        str += "<td class='setWidth9'>" + Bearing + "</td>";
        str += "<td class='setWidth11'style='text-align: center;' title='" + Description1 + "'>" + Description + "</td>";
        str += "<td class='setWidth11' style='display:none;'>" + Description1 + "</td>"; // For Export Column Functionality 
        str += "<td class='setWidth12'>" + AssetNo + "</td>";
        str += "<td class='setWidth13'>" + DateTagged + "</td>";
        str += "<td class='setWidth14'style='text-align: center;'>" + DaysSinceLastInsp + "</td>";
        str += "<td class='setWidth15'>" + Tonnage + "</td>";
        str += "<td class='setWidth16'>" + DesignType + "</td>";
        str += "<td class='setWidth17'style='text-align: center;'>" + MaterialType + "</td>";
        str += "<td class='setWidth18'style='text-align: center;'>" + DutyRating + "</td>";
        str += "<td class='setWidth19'style='text-align: center;'>" + DateFormat(DateOFDismantle) + "</td>";
        str += "<td class='setWidth20'style='text-align: center;'>" + InspectedBy + "</td>";
        str += "<td class='setWidth21'style='text-align: center;'>" + HireDaysToDate + "</td>";
        str += "<td class='setWidth22'>" + TotalHireDays + "</td>";
        str += "<td class='setWidth23'>" + SAPNumber + "</td>";
        str += "<td class='setWidth24'>" + ScafType + "</td>";
        str += "<td class='setWidth25'>" + ClientRef + "</td>";
        str += "<td class='setWidth26'style='text-align: center;'>" + OwnerName + "</td>";
        str += "<td class='setWidth27'>" + SiteLocation + "</td>";
        str += "<td class='setWidth42'>" + LongTermStructure + "</td>";
        str += "<td class='setWidth28'>" + ScaffoldOwner + "</td>";
        str += "<td class='setWidth29'>" + discipline + "</td>";
        str += "<td class='setWidth29'>" + WorkGroup + "</td>";
        str += "<td class='setWidth29'>" + SubGroup + "</td>";
        str += "<td class='setWidth30'style='text-align: center;'>" + ManHrsErect + "</td>";
        str += "<td class='setWidth31'style='text-align: center;'>" + ManHrsDismantle + "</td>";
        str += "<td class='setWidth32'>" + ShiftAllocation + "</td>";
        str += "<td class='setWidth33'style='text-align: center;'>" + CreatedBy + "</td>";
        str += "<td class='setWidth34'style='text-align: center;'>" + DateFormat(CreatedOn) + "</td>";
        str += "<td class='setWidth35'>" + UpdatedBy + "</td>";
        str += "<td class='setWidth36'style='text-align: center;'>" + DateFormat(UpdatedOn) + "</td>";
        str += "<td class='setWidth37'style='text-align: center;'>" + Length + "</td>";
        str += "<td class='setWidth38'style='text-align: center;'>" + Width + "</td>";
        str += "<td class='setWidth39'style='text-align: center;'>" + Height + "</td>";
        str += "<td class='setWidth40'style='text-align: center;'>" + SiteLocationStatus + "</td>";
        str += "<td class='setWidth41'style='display:none'>" + LockedIn + "</td>";
        str += "<td class='setWidth41' style='text-align: center;'>" + LongTermStructure + "</td>";
        str += "<td class='setWidth41' style='text-align: center;'>" + ChkRequiresIsolation + "</td>";
        
        str += "</tr>";
    }

    bindDataToTable(str); // Ensure this function is correctly implemented
}

function bindDataToTable(str) {
    if ($.fn.DataTable.isDataTable('#tblOfflineregisterlist')) {
        $('#tblOfflineregisterlist').DataTable().clear().destroy();
    }

    $('#tbodyRegisterList').html(str);
   
    $('.btnJobEdit').click(function () {
        event.stopImmediatePropagation();
        var RegisterId = $(this).attr('tbnEditRegisterId_').split('_')[0];

        var UserRole = getParameterByName('UserRole');

      

        //if (RegisterId != "") {
        //    window.location.href = 'OfflineRegister.html?UID=' + OffEncrypt(UserId.toString()) + '&UserRole=' + UserRole + '&id='+OffEncrypt(ClientID.toString())+ '&SiteLocId=' + OffEncrypt(SiteLocId.toString()) +'&RID=' + OffEncrypt( RegisterId.toString());

        //}

        if (RegisterId != "") {

            sendIndexedDBDataToServer(dbName,dbVersion);

            window.location.href = 'OfflineRegister.html?UID=' + encodeURIComponent(OffEncrypt(UserId.toString())) +
                '&UserRole=' + encodeURIComponent(UserRole) +
                '&id=' + encodeURIComponent(OffEncrypt(ClientID.toString())) +
                '&SiteLocId=' + encodeURIComponent(OffEncrypt(SiteLocId.toString())) +
                '&OfflineUserRole=' + encodeURIComponent(OffEncrypt(OfflineUserRole.toString())) +
                '&RID=' + encodeURIComponent(OffEncrypt(RegisterId.toString()));
        }

    });
    table = $('#tblOfflineregisterlist').DataTable({
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
                title: 'Register List',

                exportOptions: {
                    
                    columns: [5, 6, 7, 8, 9, 10, 11, 12, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 43, 44, 45, 46, 47],
                }

            }

        ],

    });
    if (table.rows().count() === 1) {
        var firstRow = table.row(0).node();  
        var RegisterId = $(firstRow).find('.btnJobEdit').attr('tbnEditRegisterId_').split('_')[0];  //
        if (RegisterId) {
         
            var UserRole = getParameterByName('UserRole');

            //sendIndexedDBDataToServer(dbName, dbVersion);
            sendIndexedDBDataToServer(dbName, dbVersion , function (error, response) {
                if (error) {
                    console.error("Failed to save data:", error);
                } else {
                    console.log("Successfully saved data:", response);
                    window.location.href = 'OfflineRegister.html?UID=' + encodeURIComponent(OffEncrypt(UserId.toString())) +
                        '&UserRole=' + encodeURIComponent(UserRole) +
                        '&id=' + encodeURIComponent(OffEncrypt(ClientID.toString())) +
                        '&SiteLocId=' + encodeURIComponent(OffEncrypt(SiteLocId.toString())) +
                        '&OfflineUserRole=' + encodeURIComponent(OffEncrypt(OfflineUserRole.toString())) +
                        '&RID=' + encodeURIComponent(OffEncrypt(RegisterId.toString()));                }
            });

            //window.location.href = 'OfflineRegister.html?UID=' + encodeURIComponent(OffEncrypt(UserId.toString())) +
            //    '&UserRole=' + encodeURIComponent(UserRole) +
            //    '&id=' + encodeURIComponent(OffEncrypt(ClientID.toString())) +
            //    '&SiteLocId=' + encodeURIComponent(OffEncrypt(SiteLocId.toString())) +
            //    '&OfflineUserRole=' + encodeURIComponent(OffEncrypt(OfflineUserRole.toString())) +
            //    '&RID=' + encodeURIComponent(OffEncrypt(RegisterId.toString()));
        }
    }


}
function uploadDataToServer() {
    const tblRegisterDetailsData = [];
    const tblRegisterGalleryData = [];
    const tblMaterialTypeData = [];
    const tblDutyRatingData = [];
    const tblAssetListData = [];
	const tblPipelineRequestData = [];
    const tblInspectionDetailsData = [];
    const tblInspectionListDetailsData = [];
    let tablesWithData = 0;
    const finalDataArray = [];

    function fetchTableData(storeName, field, dataArray) {
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
                        if (cursor.value[field] === true) {
                            dataArray.push(cursor.value);
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
                alert(uploadMessage);//uploadMessage += finalDataArray.map(item => item.TableName).join(", ");

                sendAllDataToServer(finalDataArray);
            } else {
                alert("There is no data to upload to the server.");
            }
                })
        .catch(error => {
            console.error("Error fetching table data:", error);
    });
            }

function sendAllDataToServer(finalDataArray) {
    $.ajax({
        url: "/OfflineRegister/UploadToCloud",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({
            data: finalDataArray, 
            UserId: UserId,
            ClientID: ClientID,
            //Siteid: SiteLocId
            Siteid: Siteid
        }),
        async: false,
        success: function (response) {
            if (response.success) {
                console.log("All data uploaded successfully:", response.message);
                clearAllTables();
                $('#tblOfflineregisterlist').DataTable().clear().destroy();


            } else {
                console.error("Error from server:", response.message);
                CustomAlert({ Result: "Failed", Message: response.message });

            }
        },
        error: function (xhr, status, error) {
            console.error("Failed to upload all data:", error);
        }
    });
}


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
//function sendIndexedDBDataToServer(dbName, dbVersion) {
//    return new Promise((resolve, reject) => {
//        let request = indexedDB.open(dbName, dbVersion);

//        request.onsuccess = function (event) {
//            let db = event.target.result;
//            let tables = db.objectStoreNames;
//            let tableData = [];
//            let promises = [];

//            for (let i = 0; i < tables.length; i++) {
//                let storeName = tables[i];

//                promises.push(
//                    new Promise((res, rej) => {
//                        let transaction = db.transaction(storeName, "readonly");
//                        let store = transaction.objectStore(storeName);
//                        let countRequest = store.count();

//                        countRequest.onsuccess = function () {
//                            tableData.push({
//                                TableName: storeName,
//                                EntryCount: countRequest.result
//                            });
//                            res();
//                        };

//                        countRequest.onerror = function () {
//                            rej(`Error counting records in ${storeName}`);
//                        };
//                    })
//                );
//            }

//            Promise.all(promises)
//                .then(() => {
//                    console.log("Table Data:", tableData);
//                    sendDataToServer(tableData, UserId.toString());
//                    resolve(tableData);
//                })
//                .catch((error) => reject(error));

//            db.close();
//        };

//        request.onerror = function () {
//            reject("Failed to open IndexedDB");
//        };
//    });
//}

//function sendDataToServer(tableData, UserId) {
//    $.ajax({
//        url: "/OfflineRegister/SaveIndexedDBData",
//        type: "POST",
//        contentType: "application/json",
//        data: JSON.stringify({ Tables: tableData, UserId: UserId }),
//        success: function (response) {
//            console.log("Data saved successfully:", response);

//        },
//        error: function (xhr, status, error) {
//            console.error("Error sending data:", error);

//        }
//    });
//}

//function sendIndexedDBDataToServer(dbName, dbVersion) {
//    return new Promise((resolve, reject) => {
//        let request = indexedDB.open(dbName, dbVersion);

//        request.onsuccess = function (event) {
//            let db = event.target.result;
//            let tables = db.objectStoreNames;
//            let tableData = [];
//            let promises = [];

//            let pipelineZeroCount = 0;
//            let pipelineNonZeroCount = 0;

//            for (let i = 0; i < tables.length; i++) {
//                let storeName = tables[i];

//                promises.push(
//                    new Promise((res, rej) => {
//                        let transaction = db.transaction(storeName, "readonly");
//                        let store = transaction.objectStore(storeName);
//                        let countRequest = store.count();

//                        if (storeName === "tblPipelineRequest") {
//                            let totalPipelineCount = 0;
//                            let cursorRequest = store.openCursor();

//                            cursorRequest.onsuccess = function (event) {
//                                let cursor = event.target.result;
//                                if (cursor) {
//                                    let record = cursor.value;
//                                    if (record.REGISTERId === 0) {
//                                        pipelineZeroCount++;
//                                    } else {
//                                        pipelineNonZeroCount++;
//                                    }
//                                    cursor.continue();
//                                } else {
//                                    // After processing all records, get the total count
//                                    let pipelineCountRequest = store.count();
//                                    pipelineCountRequest.onsuccess = function () {
//                                        totalPipelineCount = pipelineCountRequest.result;
//                                        tableData.push({
//                                            TableName: storeName,
//                                            EntryCount: totalPipelineCount // ✅ Now added in array
//                                        });
//                                        res();
//                                    };
//                                    pipelineCountRequest.onerror = function () {
//                                        rej(`Error counting total records in ${storeName}`);
//                                    };
//                                }
//                            };

//                            cursorRequest.onerror = function () {
//                                rej(`Error processing PipelineRequest records`);
//                            };
//                        } else {
//                            countRequest.onsuccess = function () {
//                                tableData.push({
//                                    TableName: storeName,
//                                    EntryCount: countRequest.result
//                                });
//                                res();
//                            };

//                            countRequest.onerror = function () {
//                                rej(`Error counting records in ${storeName}`);
//                            };
//                        }
//                    })
//                );
//            }

//            Promise.all(promises)
//                .then(() => {
//                    console.log("Table Data:", tableData);
//                    sendDataToServer(tableData, UserId.toString(), pipelineZeroCount, pipelineNonZeroCount);
//                    resolve(tableData);
//                })
//                .catch((error) => reject(error));

//            db.close();
//        };

//        request.onerror = function () {
//            reject("Failed to open IndexedDB");
//        };
//    });
//}


//function sendDataToServer(tableData, UserId, pipelineZeroCount, pipelineNonZeroCount) {
//    $.ajax({
//        url: "/OfflineRegister/SaveIndexedDBData",
//        type: "POST",
//        contentType: "application/json",
//        data: JSON.stringify({
//            Tables: tableData,
//            UserId: UserId,
//            ZeroRegisterCount: pipelineZeroCount,
//            NonZeroRegisterCount: pipelineNonZeroCount
//        }),
//        success: function (response) {
//            console.log("Data saved successfully:", response);
//        },
//        error: function (xhr, status, error) {
//            console.error("Error sending data:", error);
//        }
//    });
//}

function sendIndexedDBDataToServer(dbName, dbVersion, callback) {
    return new Promise((resolve, reject) => {
        let request = indexedDB.open(dbName, dbVersion);

        request.onsuccess = function (event) {
            let db = event.target.result;
            let tables = db.objectStoreNames;
            let tableData = [];
            let promises = [];

            let pipelineZeroCount = 0;
            let pipelineNonZeroCount = 0;

            for (let i = 0; i < tables.length; i++) {
                let storeName = tables[i];

                promises.push(
                    new Promise((res, rej) => {
                        let transaction = db.transaction(storeName, "readonly");
                        let store = transaction.objectStore(storeName);
                        let countRequest = store.count();

                        if (storeName === "tblPipelineRequest") {
                            let cursorRequest = store.openCursor();

                            cursorRequest.onsuccess = function (event) {
                                let cursor = event.target.result;
                                if (cursor) {
                                    let record = cursor.value;
                                    if (record.REGISTERId === 0) {
                                        pipelineZeroCount++;
                                    } else {
                                        pipelineNonZeroCount++;
                                    }
                                    cursor.continue();
                                } else {
                                    let pipelineCountRequest = store.count();
                                    pipelineCountRequest.onsuccess = function () {
                                        tableData.push({
                                            TableName: storeName,
                                            EntryCount: pipelineCountRequest.result
                                        });
                                        res();
                                    };
                                    pipelineCountRequest.onerror = function () {
                                        rej(`Error counting total records in ${storeName}`);
                                    };
                                }
                            };

                            cursorRequest.onerror = function () {
                                rej(`Error processing PipelineRequest records`);
                            };
                        } else {
                            countRequest.onsuccess = function () {
                                tableData.push({
                                    TableName: storeName,
                                    EntryCount: countRequest.result
                                });
                                res();
                            };

                            countRequest.onerror = function () {
                                rej(`Error counting records in ${storeName}`);
                            };
                        }
                    })
                );
            }

            Promise.all(promises)
                .then(() => {
                    console.log("Table Data:", tableData);
                    sendDataToServer(tableData, UserId.toString(), pipelineZeroCount, pipelineNonZeroCount, RegId, callback);
                    resolve(tableData);
                })
                .catch((error) => reject(error));

            db.close();
        };

        request.onerror = function () {
            reject("Failed to open IndexedDB");
        };
    });
}

function sendDataToServer(tableData, UserId,  pipelineZeroCount, pipelineNonZeroCount, RegId, callback) {
    var upload = "0";
    $.ajax({
        url: "/OfflineRegister/SaveIndexedDBData",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({
            Tables: tableData,
            UserId: UserId,
            ZeroRegisterCount: pipelineZeroCount,
            NonZeroRegisterCount: pipelineNonZeroCount,
            RID: RegId,
            upload: upload
        }),
        success: function (response) {
            console.log("Data saved successfully:", response);
            if (callback) callback(null, response); 
        },
        error: function (xhr, status, error) {
            console.error("Error sending data:", error);
            if (callback) callback(error, null); 
        }
    });
}
