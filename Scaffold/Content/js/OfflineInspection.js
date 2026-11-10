  
        var db = openDatabase("scaffpac", "0.1", "scaffpac", 1024 * 1024);
        var CreateCheckListHeader = "CREATE TABLE IF NOT EXISTS tblChecklistContent(ContentID INTEGER,CheckListContent TcEXT,InspectionID TcEXT,itemOrder TcEXT)";
        var InsertCheckListHeader = "Insert INTO tblChecklistContent(ContentID,CheckListContent,InspectionID,itemOrder) Values(?,?,?,?)";
        var SelectCheckListHeader = "select ContentID,CheckListContent,itemOrder from tblChecklistContent";
        var dropCheckListHeader = "DROP TABLE if exists tblChecklistContent";

        var CreateCheckListContent = "CREATE TABLE IF NOT EXISTS tblInspectionList(ContentID INTEGER,ClientID TcEXT,SiteLocId INTEGER,ScaffRef TcEXT,DesignType TcEXT,OwnerName TcEXT,SiteLocation TEcXT,Location TcEXT,CheckListId INTEGER,RegisterID TcEXT,InspectionID TcEXT,CheckListContent TcEXT,itemOrder TcEXT,InspectionCheckListColor TcEXT,Y TcEXT,N TcEXT,NA TcEXT,IsNA TcEXT,ActionRequired TcEXT,ChecklistItem INTEGER)";
        var CreateTempCheckListContent = "CREATE TABLE IF NOT EXISTS tbltempInspectionList(ContentID INTEGER,Y TcEXT,N TcEXT,NA TcEXT,ActionRequired TcEXT)";

        var InsertCheckListContent = "Insert INTO tblInspectionList(ContentID,ClientID,SiteLocId,ScaffRef,SiteLocation,Location,CheckListId,RegisterID,InspectionID,CheckListContent,itemOrder,InspectionCheckListColor,Y,N,NA,IsNA,ActionRequired,ChecklistItem,DesignType,OwnerName) Values(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

        var UpdateCheckListContent = "UPDATE tblInspectionList SET Y=?,N=?,NA=?,ActionRequired=? where ContentID=?";
        // var SelectCheckListContent = "select * from tblInspectionList  where ContentID=? and ClientID=? and RegisterID=? and InspectionID=?";
        // var UpdateCheckListContent = "UPDATE tblInspectionList SET Y=?,N=?,NA=?,ActionRequired=? where ContentID=? and ClientID=? and RegisterID=? and InspectionID=?";
          
        // Create table tblInspection.(16 Dec 2019.)
        var CreateInspection = "CREATE TABLE IF NOT EXISTS tblInspection( InspectionID INTEGER ,RegisterID INTEGER,CurrentDate TcEXT,InspectionDate TcEXT,InspectedById INTEGER,CompleteInsp TcEXT,AckDate TcEXT,LicenceNo TcEXT,OnlineInsp TcEXT,Acknowledgment TcEXT,InspSign TcEXT,InspComments TcEXT ,Inspimage TcEXT,PhotoAngle TcEXT,name TcEXT,IsInspector TcEXT,InspStatus TcEXT,IsDelete TcEXT,AcknowledgeTimeZone TcEXT,InspectionTime TcEXT,OnlyInspectionDate TcEXT,BuildingNo TcEXT,AssetNo TcEXT,FloorLevel TcEXT,MaterialType TcEXT,ReasonForAccess TcEXT)";

        var InsertInspection = "Insert INTO tblInspection(InspectionID,RegisterID,CurrentDate,InspectionDate,InspectedById,CompleteInsp,LicenceNo,AckDate,OnlineInsp,Acknowledgment,InspSign,InspComments,Inspimage,PhotoAngle,name,IsInspector,InspStatus,IsDelete,InspectionTime,OnlyInspectionDate) Values(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";
        // var SelectInspection = "select * from tblInspection where InspStatus == 'Confirmed' Or InspStatus == 'Out of Service'";
        // var SelectInspection = "select * from tblInspection where InspStatus == 'Confirmed' Or InspStatus == 'Out of Service' or IsDelete=1";  // Comment On 10 Jan 2020
         var SelectInspection = "select * from tblInspection"; //Comment On 25 Aug 2020
       // var SelectInspection = "select * from tblInspection where IsDelete=0";
        var dropInspection = "DROP TABLE if exists tblInspection";

        // var SelectCheckListContent = "select * from tblInspectionList";

        // Comment On 4 Feb 2020
        // var SelectCheckListContent = "select * from tblInspection Left join tblInspectionList on tblInspection.InspectionID=tblInspectionList.InspectionID   where InspStatus == 'Confirmed' Or InspStatus == 'Out of Service'";
        //  var SelectCheckListContent = "select * from tblInspection Left join tblInspectionList on tblInspection.InspectionID=tblInspectionList.InspectionID";
        //var SelectCheckListContent = "select * from tblInspectionList  where InspStatus == 'Confirmed' Or InspStatus == 'Out of Service'";
        var SelectCheckListContent = "select tblInspectionList.* from tblInspection  join tblInspectionList on tblInspection.InspectionID=tblInspectionList.InspectionID   where tblInspection.InspStatus == 'Confirmed' Or tblInspection.InspStatus == 'Out of Service'";


        var SelectCheckListContentId = "select * from tblInspectionList where ChecklistItem=?";
        var dropCheckListContent = "DROP TABLE if exists tblInspectionList";
        var dropTempCheckListContent = "DROP TABLE if exists tbltempInspectionList";
        var checkListHeaderDataset;
        var checkListContentDataset;
        var checkListdatasetById;
        var JsonContentDataArray = [];
        var JsonHeaderDataArray = [];
        var InspectionArray = [];


        //Getting Parameters From the Url
        var InspID = getParameterByName('InspID');
        var CheckListID = getParameterByName('CheckListID');
        var ClientID = getParameterByName('id');
        var RegisterID = getParameterByName('RID');
        var GetOfflineStatus = getParameterByName('offlinestatus');
        var OfflineStatus = window.atob(GetOfflineStatus);
        var InspectionID = getParameterByName('InspectionID');
        var UID = getParameterByName('UID');

        var url = window.location.href;

        var checkFlag = false;

        function getParameterByName(name, url) {
            if (!url) url = window.location.href;
            name = name.replace(/[\[\]]/g, "\\$&");
            var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, " "));
        }

        function onError(tx, error) // Function for Hendeling Error...
        {
            alert(error.message);
        }
        function loadAndReset() //Function for Load and Reset...
        {

        }

        //$(document).ready(function () {
        //    $('#InspectionList').DataTable({
        //        "bPaginate": false,
        //        "bFilter": false,
        //        "bInfo": false,
        //        //scrollX:true,
        //        scrollY: "60vh",
        //    });
        //});

      
        $(function (){

            updatebtn();

        });

        function updatebtn() {
            //if (navigator.onLine == false) {
            if (navigator.onLine) {
                //  $("#offlinebtnSave").text("Save To Server");
                // $("#offlinebtnSave").text("Save To Local");
                $("#onlineimg").css("display", "block");
                $("#offlineimg").css("display", "none");
            } else {
                // $("#offlinebtnSave").text("Save To Local");
                $("#onlineimg").css("display", "none");
                $("#offlineimg").css("display", "block");
            }
            setTimeout(function () {
                updatebtn();
            }, 1000);
        }

        function saveLocalToServer(callback) {
            db.transaction(function (tx) {

                var arrInspForm = "";
                tx.executeSql(SelectInspection, [], function (tx, results) {

                    if (results.rows.length > 0) {
                        var bb = results.rows.length;
                        var a = jQuery.makeArray(results.rows.item(0));
                        var len = results.rows.length;
                        for (var i = 1; i < len; i++) {
                            a[i] = results.rows.item(i);
                        }

                        arrInspForm = a;

                    }
                });

                // Comment on 23 Oct 2019
               // tx.executeSql(SelectCheckListContent, [], function (tx, results) {
                 //    tx.executeSql("select * from tblInspectionList where InspectionID = '" + InspectionID + "'", [], function (tx, results) {
                tx.executeSql(SelectCheckListContent, [], function (tx, results) {
                    if (results.rows.length > 0) {
                        var bb = results.rows.length;
                        var a = jQuery.makeArray(results.rows.item(0));
                        var len = results.rows.length;
                        for (var i = 1; i < len; i++) {
                            a[i] = results.rows.item(i);
                        }

                        var arForm = a;

                        $.ajax({
                            type: "POST",
                            url: "/OfflineInspection/updateCheckList",
                          //  url: "../WebApi/OfflineInspection.asmx/updateCheckList",
                            //data: JSON.stringify({ ListData: arForm, InspID: InspID, GetOfflineStatus: GetOfflineStatus }),
                            //data: JSON.stringify({ ListData: arForm, ListDataInsp: arrInspForm, InspID: InspID, GetOfflineStatus: GetOfflineStatus, UID: UID }),
                             data: { ListData: arForm, ListDataInsp: arrInspForm, InspID: InspID, GetOfflineStatus: GetOfflineStatus, UID: UID },
                           // data: { ListData: arForm },
                            //data:{ ListDataInsp: arrInspForm},
                            //contentType: "application/json",
                            //dataType: "json",
                            success: function (response) {
                                //var result = response.d;
                                // var result = response.d.split('_');
                                var result = response.split('_');
                                callback(result);
                                if (result[0] == "success") {
                                   // var Res = { Result: "Success", Message: "Your Data Moved From Local To Server!" };
                                    //CustomAlert(Res);

                                    db.transaction(function (t) {
                                        t.executeSql(dropCheckListHeader);
                                        t.executeSql(dropCheckListContent);
                                        t.executeSql(dropInspection);
                                    });

                                    setTimeout(function () {
                                        if (result[1] != "") {
                                            var Res = { Result: "Success", Message: "The Data has been copied From Local To Server!" };
                                            CustomAlert(Res);
                                            location.reload();
                                        }
                                            // For When Click On  Upload To Cloud Button Then Page Refresh
                                        else {
                                            var Res = { Result: "Success", Message: "The Data has been copied From Local To Server!" };
                                            CustomAlert(Res);
                                            location.reload();
                                        }
                                    }, 1000);
                                   
                                } else {
                                    //var Res = { Result: "Error", Message: "SomeThing went wrong!" };
                                    //CustomAlert(Res);
                                    alert(response);
                                }
                           },
                            error:function(x,y,z)
                            {
                                alert(z);
                            }
                        });

                    }
                });
            });
        }


        $(function () {

             /*Code By Nitin For Offline Changes in 7/6/2021*/
   
          
            db.transaction(function (t) {

                   //// Comment On 22 Oct 2019
                   //t.executeSql(dropCheckListHeader);
                  //t.executeSql(dropCheckListContent);

                t.executeSql(CreateCheckListHeader);
                t.executeSql(CreateCheckListContent);
                t.executeSql(CreateInspection);
            });


            // For Register Code

            setTimeout(function () {
               

                if (navigator.onLine)
                    //if (navigator.onLine)
                   {
                    if (OfflineStatus == 1) {

                        showOfflineInspectionListGrid();

                    } else if (OfflineStatus == 0) {


                        //CreateDbTables();
                        db.transaction(function (tx) {

                            tx.executeSql(SelectCheckListContent, [], function (tx, result) {
                                var NewUrl = window.location.href.split('offlinestatus');
                                if (result.rows.length > 0) {
                                    bootbox.confirm("Do you want to Sync your LOCAL data to CLOUD server?", function (result) {
                                        if (result) {
                                            $("#customLoader").css("display", "block");
                                          //  saveLocalToServer();

                                            saveLocalToServer(function (d) {
                                               
                                            });
                                        }
                                        else {
                                            //db.transaction(function (t) {

                                            //    // Comment On 22 Oct 2019
                                            //    t.executeSql(dropCheckListHeader);
                                            //    t.executeSql(dropCheckListContent);
                                            //});
                                            //location.reload();
                                            ////  GetServerData();
                                          //  showOfflineInspectionListGrid();
                                            setInterval(function () {
                                                db.transaction(function (tx) {
                                                    tx.executeSql("Select * from tblInspectionList", [], function (tx, results) {
                                                        if (results.rows.length > 0) {
                                                            var encodedString = window.btoa("1");
                                                            window.location = NewUrl[0] + "offlinestatus=" + encodedString + "";
                                                        }
                                                    });
                                                });
                                            }, 2000);
                                        }
                                    });
                                    //console.log(checkListContentDataset);
                                }
                                else {
                                    bootbox.confirm("Do you want to save data on Local Server?", function (result) {
                                        if (result) {
                                            var NewUrl = window.location.href.split('offlinestatus');
                                            $("#customLoader").css("display", "block");
                                            DownloadServerData();
                                            setInterval(function () {
                                               db.transaction(function (tx) {
                                                        tx.executeSql("Select * from tblInspectionList", [], function (tx, results) {
                                                            if (results.rows.length > 0) {
                                                                var encodedString = window.btoa("1");
                                                                window.location = NewUrl[0] + "offlinestatus=" + encodedString + "";
                                                            } else {
                                                                var encodedString = window.btoa("1");
                                                                window.location = NewUrl[0] + "offlinestatus=" + encodedString + "";
                                                            }
                                                        });
                                                    });
                                            }, 2000);
                                       
                                        }
                               
                                    });
                                   
                                }
                      
                            });
                        });
                    }
                } else {
                    showOfflineInspectionListGrid();
                }

            }, 1000);



        });


        //Saving Server Data into the Local Server
        function DownloadServerData()
        {
            db.transaction(function (t) {
                 // Comment On 22 Oct 2019
                t.executeSql(dropCheckListHeader);
                t.executeSql(CreateCheckListHeader);

                t.executeSql(dropCheckListContent);
                t.executeSql(CreateCheckListContent);

                t.executeSql(dropInspection);

                t.executeSql(CreateInspection);
                
            });

            // Add code on 16 Dec 2019.
            $.ajax({
                type: "POST",
                // url: "../WebApi/OfflineInspection.asmx/GetAllInspectionDetails",
                url: "/OfflineInspection/GetAllInspectionDetails",
                data: JSON.stringify({ ClientID: ClientID, UID: UID }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (response) {
                    // InspectionArray = response.d;
                    InspectionArray = response;
                    if (InspectionArray != null && InspectionArray != undefined) {
                        var SqlInspectionSt = "";
                        for (var i = 0; i < InspectionArray.length; i++) {
                           
                            var InspectionID = InspectionArray[i].InspectionID;
                            var RegisterID = InspectionArray[i].RegisterID;
                            var CurrentDate = InspectionArray[i].CurrentDate;
                            var InspectedById = InspectionArray[i].InspectedById;
                            var InspectionDate = InspectionArray[i].InspectionDate;
                            var LicenceNo = InspectionArray[i].LicenceNo;
                            var AckDate = InspectionArray[i].AckDate;
                            var OnlineInsp = InspectionArray[i].OnlineInsp;
                            var CompleteInsp = InspectionArray[i].CompleteInsp;
                            var InspComments = InspectionArray[i].InspComments;
                            var Acknowledgment = InspectionArray[i].Acknowledgment;
                            var InspSign = InspectionArray[i].InspSign;
                            var Inspimage= InspectionArray[i].Inspimage;
                            var PhotoAngle = InspectionArray[i].PhotoAngle;
                            var name = InspectionArray[i].name;
                            var IsInspector = InspectionArray[i].IsInspector;
                            var InspStatus = InspectionArray[i].InspStatus;
                            var InspectionTime = InspectionArray[i].InspectionTime;
                            var OnlyInspectionDate = InspectionArray[i].OnlyInspectionDate;
                            var BuildingNo =InspectionArray[i].BuildingNo;
                            var AssetNo =InspectionArray[i].AssetNo;
                            var FloorLevel =InspectionArray[i].FloorLevel;
                            var MaterialType =InspectionArray[i].MaterialType;
                            var ReasonForAccess =InspectionArray[i].ReasonForAccess;
                           
                            if (InspectionArray.length == 1) {
                                // SqlInspectionSt += "Insert INTO tblInspection(InspectionID,RegisterID,CurrentDate,InspectionDate,InspectedById,CompleteInsp,LicenceNo,AckDate,OnlineInsp,Acknowledgment,InspSign,InspComments,Inspimage,PhotoAngle,name,IsInspector,InspStatus,InspectionTime,OnlyInspectionDate,IsDelete) Values(" + InspectionID + "," + RegisterID + ",'" + CurrentDate + "','" + InspectionDate + "','" + InspectedById + "','" + CompleteInsp + "','" + LicenceNo + "','" + AckDate + "','" + OnlineInsp + "','" + Acknowledgment + "','" + InspSign + "','" + InspComments.replace("'", "''") + "','" + Inspimage + "','" + PhotoAngle + "','" + name.replace("'", "''") + "','" + IsInspector + "','" + InspStatus + "','" + InspectionTime + "','" + OnlyInspectionDate + "',0)";
                                // Query Change on 07 Feb 2020
                                SqlInspectionSt += "Insert INTO tblInspection(InspectionID,RegisterID,CurrentDate,InspectionDate,InspectedById,CompleteInsp,LicenceNo,AckDate,OnlineInsp,Acknowledgment,InspSign,InspComments,Inspimage,PhotoAngle,name,IsInspector,InspStatus,InspectionTime,OnlyInspectionDate,IsDelete,BuildingNo,AssetNo,FloorLevel,MaterialType,ReasonForAccess) Values(" + InspectionID + "," + RegisterID + ",'" + CurrentDate + "','" + InspectionDate + "','" + InspectedById + "','" + CompleteInsp + "','" + LicenceNo + "','" + AckDate + "','" + OnlineInsp + "','" + Acknowledgment + "','" + InspSign + "','" + InspComments.replace("'", "''") + "','" + Inspimage + "','" + PhotoAngle + "','" + name.replace("'", "''") + "','" + IsInspector + "','" + InspStatus + "','" + InspectionTime + "','" + OnlyInspectionDate + "',0,'" + BuildingNo.replace("'", "''") + "','" + AssetNo.replace("'", "''") + "','" + FloorLevel.replace("'", "''") + "','" + MaterialType.replace("'", "''") + "','" + ReasonForAccess.replace(/'/g, "''") + "')";
                                db.transaction(function (tx) {
                                    tx.executeSql(SqlInspectionSt, [], function (tx, results) {
                                        if (results.rows.length > 0) {
                                            //GetCheckContentData();
                                        }
                                    });
                                });
                            }
                            //if ($("#tempimg").val() != "") {
                            //    tempimg = $("#tempimg").val();
                            //    updateInspectionQuery += "Inspimage='" + tempimg.replace("'", "''") + "',";
                            else {
                                if (i == 0) {
                                    //SqlInspectionSt += "Insert INTO tblInspection(InspectionID,RegisterID,CurrentDate,InspectionDate,InspectedById,CompleteInsp,LicenceNo,AckDate,OnlineInsp,Acknowledgment,InspSign,InspComments,Inspimage,PhotoAngle,name,IsInspector,InspStatus,InspectionTime,OnlyInspectionDate,IsDelete) Values(" + InspectionID + "," + RegisterID + ",'" + CurrentDate + "','" + InspectionDate + "','" + InspectedById + "','" + CompleteInsp + "','" + LicenceNo + "','" + AckDate + "','" + OnlineInsp + "','" + Acknowledgment + "','" + InspSign + "','" + InspComments.replace("'", "''") + "','" + Inspimage + "','" + PhotoAngle + "','" + name.replace("'", "''") + "','" + IsInspector + "','" + InspStatus + "','" + InspectionTime + "','" + OnlyInspectionDate + "',0),";
                                    // Query Change on 07 Feb 2020
                                    SqlInspectionSt += "Insert INTO tblInspection(InspectionID,RegisterID,CurrentDate,InspectionDate,InspectedById,CompleteInsp,LicenceNo,AckDate,OnlineInsp,Acknowledgment,InspSign,InspComments,Inspimage,PhotoAngle,name,IsInspector,InspStatus,InspectionTime,OnlyInspectionDate,IsDelete,BuildingNo,AssetNo,FloorLevel,MaterialType,ReasonForAccess) Values(" + InspectionID + "," + RegisterID + ",'" + CurrentDate + "','" + InspectionDate + "','" + InspectedById + "','" + CompleteInsp + "','" + LicenceNo + "','" + AckDate + "','" + OnlineInsp + "','" + Acknowledgment + "','" + InspSign + "','" + InspComments.replace("'", "''") + "','" + Inspimage + "','" + PhotoAngle + "','" + name.replace("'", "''") + "','" + IsInspector + "','" + InspStatus + "','" + InspectionTime + "','" + OnlyInspectionDate + "',0,'" + BuildingNo.replace("'", "''") + "','" + AssetNo.replace("'", "''") + "','" + FloorLevel.replace("'", "''") + "','" + MaterialType.replace("'", "''") + "','" + ReasonForAccess.replace(/'/g, "''") + "'),";
                                } else if (i != InspectionArray.length - 1) {
                                   // SqlInspectionSt += "(" + InspectionID + "," + RegisterID + ",'" + CurrentDate + "','" + InspectionDate + "','" + InspectedById + "','" + CompleteInsp + "','" + LicenceNo + "','" + AckDate + "','" + OnlineInsp + "','" + Acknowledgment + "','" + InspSign + "','" + InspComments.replace("'", "''") + "','" + Inspimage + "','" + PhotoAngle + "','" + name.replace("'", "''") + "','" + IsInspector + "','" + InspStatus + "','" + InspectionTime + "','" + OnlyInspectionDate + "',0),";
                                    // Query Change on 07 Feb 2020
                                    SqlInspectionSt += "(" + InspectionID + "," + RegisterID + ",'" + CurrentDate + "','" + InspectionDate + "','" + InspectedById + "','" + CompleteInsp + "','" + LicenceNo + "','" + AckDate + "','" + OnlineInsp + "','" + Acknowledgment + "','" + InspSign + "','" + InspComments.replace("'", "''") + "','" + Inspimage + "','" + PhotoAngle + "','" + name.replace("'", "''") + "','" + IsInspector + "','" + InspStatus + "','" + InspectionTime + "','" + OnlyInspectionDate + "',0,'" + BuildingNo.replace("'", "''") + "','" + AssetNo.replace("'", "''") + "','" + FloorLevel.replace("'", "''") + "','" + MaterialType.replace("'", "''") + "','" + ReasonForAccess.replace(/'/g, "''") + "'),";
                                }

                                if (i == InspectionArray.length - 1) {
                                   // SqlInspectionSt += "(" + InspectionID + "," + RegisterID + ",'" + CurrentDate + "','" + InspectionDate + "','" + InspectedById + "','" + CompleteInsp + "','" + LicenceNo + "','" + AckDate + "','" + OnlineInsp + "','" + Acknowledgment + "','" + InspSign + "','" + InspComments.replace("'", "''") + "','" + Inspimage + "','" + PhotoAngle + "','" + name.replace("'", "''") + "','" + IsInspector + "','" + InspStatus + "','" + InspectionTime + "','" + OnlyInspectionDate + "',0);";
                                    // Query Change on 07 Feb 2020
                                    SqlInspectionSt += "(" + InspectionID + "," + RegisterID + ",'" + CurrentDate + "','" + InspectionDate + "','" + InspectedById + "','" + CompleteInsp + "','" + LicenceNo + "','" + AckDate + "','" + OnlineInsp + "','" + Acknowledgment + "','" + InspSign + "','" + InspComments.replace("'", "''") + "','" + Inspimage + "','" + PhotoAngle + "','" + name.replace("'", "''") + "','" + IsInspector + "','" + InspStatus + "','" + InspectionTime + "','" + OnlyInspectionDate + "',0,'" + BuildingNo.replace("'", "''") + "','" + AssetNo.replace("'", "''") + "','" + FloorLevel.replace("'", "''") + "','" + MaterialType.replace("'", "''") + "','" + ReasonForAccess.replace(/'/g, "''") + "');";
                                    db.transaction(function (tx) {
                                        tx.executeSql(SqlInspectionSt, [], function (tx, results) {
                                            if (results.rows.length > 0) {
                                                //GetCheckContentData();
                                            }
                                        });
                                    });
                                }
                            }
                        }
                       
                    }
                }
            });
         

            //setTimeout(function () {
            $.ajax({
                type: "POST",
                url: "/OfflineInspection/GetAllCheckListHeadersMultiple",
               // url: "../WebApi/OfflineInspection.asmx/GetAllCheckListHeadersMultiple",
               // data: JSON.stringify({ ClientID: ClientID, CheckListID: CheckListID, InspID: InspID }),   //Change Line On 26 Nov 2019
                data: JSON.stringify({ ClientID: ClientID,UID:UID }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (response) {
                    //alert(response.d);
                    JsonHeaderDataArray = [];
                    CheckListHeaderArray = response;
                   // CheckListHeaderArray = response.d;
                    $("#customLoader").css("display", "block");
                    if (CheckListHeaderArray != null && CheckListHeaderArray != undefined) {
                        var SqlContentHeaderSt = "";
                        for (var i = 0; i < CheckListHeaderArray.length; i++) {
                            var contentId = CheckListHeaderArray[i].contentId;
                            var CheckListContent = CheckListHeaderArray[i].CheckListContent;
                            var HederInspectionID = CheckListHeaderArray[i].InspectionID;
                            var itemOrder = CheckListHeaderArray[i].itemOrder;
                            var hdrArr = { "ContentID": contentId, "CheckListContent": CheckListContent, "HederInspectionID": HederInspectionID };
                            JsonHeaderDataArray.push(hdrArr);

                            if (i == 0) {
                                SqlContentHeaderSt += "Insert INTO tblChecklistContent(ContentID,CheckListContent,InspectionID,itemOrder) Values(" + contentId + ",'" + CheckListContent + "','" + HederInspectionID + "','" + itemOrder + "'),";
                            } else if (i != CheckListHeaderArray.length - 1) {
                                SqlContentHeaderSt += "(" + contentId + ",'" + CheckListContent + "','" + HederInspectionID + "','" + itemOrder + "'),";
                            }

                            if (i == CheckListHeaderArray.length - 1) {
                                SqlContentHeaderSt += "(" + contentId + ",'" + CheckListContent + "','" + HederInspectionID + "','" + itemOrder + "');";
                                db.transaction(function (tx) {
                                    tx.executeSql(SqlContentHeaderSt, [], function (tx, results) {
                                        if (results.rows.length > 0) {
                                            //GetCheckContentData();
                                        }
                                    });
                                });
                            }
                        }
                        //GetCheckHeaderData();
                       // showOfflineInspectionListGrid();
                    }
                }
            });
            //}, 1000);


            setTimeout(function () {
                $.ajax({
                    type: "POST",
                    url: "/OfflineInspection/GetAllCheckListContentMultiple",
                   // url: "../WebApi/OfflineInspection.asmx/GetAllCheckListContentMultiple",
                   // data: JSON.stringify({ ClientID: ClientID, CheckListID: CheckListID, InspID: InspID }),    //Line Change On 26 Nov 2019
                    data: JSON.stringify({ ClientID: ClientID, UID: UID }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (response) {
                        //alert(JSON.stringify(CheckListHeaderArray));
                        JsonContentDataArray = [];
                        // CheckListContentArray = response.d;
                        CheckListContentArray = response;
                        var SqlContentDataSt = "";
                              if (CheckListContentArray != null && CheckListContentArray != undefined) {

                                    $("#customLoader").css("display", "block");
                                    for (var i = 0; i < CheckListContentArray.length; i++) {

                                        var contentId = CheckListContentArray[i].contentId;
                                        var ClientID = CheckListContentArray[i].ClientID;

                                        var SiteLocId = CheckListContentArray[i].SiteLocId;
                                        var ScaffRef = CheckListContentArray[i].ScaffRef;
                                        var SiteLocation = CheckListContentArray[i].SiteLocation;
                                        var Location = CheckListContentArray[i].Location;
                                        var CheckListId = CheckListContentArray[i].CheckListId;

                                        var RegisterID = CheckListContentArray[i].RegisterID;
                                        var InspectionID = CheckListContentArray[i].InspectionID;
                                        var CheckListContent = CheckListContentArray[i].CheckListContent;
                                        var CheckListContent1 = CheckListContent.replace(/\'/g, "\"");
                                        var Y = CheckListContentArray[i].Y;
                                        var N = CheckListContentArray[i].N;
                                        var NA = CheckListContentArray[i].NA;
                                        var IsNA = CheckListContentArray[i].IsNA;
                                        var ActionRequired = CheckListContentArray[i].ActionRequired == null ? "" : CheckListContentArray[i].ActionRequired;
                                        var ChecklistItem = CheckListContentArray[i].ChecklistItem;
                                        var itemOrder = CheckListContentArray[i].itemOrder;
                                        var InspectionCheckListColor = CheckListContentArray[i].InspectionCheckListColor;
                                        var DesignType = CheckListContentArray[i].DesignType;
                                        var OwnerName = CheckListContentArray[i].OwnerName;
                                        strArray = { "ContentID": contentId, "CheckListContent": CheckListContent1, "itemOrder": itemOrder, "InspectionCheckListColor": InspectionCheckListColor, "Y": Y, "N": N, "NA": NA, "IsNA": IsNA, "ActionRequired": ActionRequired, "ChecklistItem": ChecklistItem, "DesignType": DesignType, "OwnerName": OwnerName };
                                        JsonContentDataArray.push(strArray);

                                        //  if (p == 0 && i == 0) {
                                        if (i == 0) {
                                            
                                            SqlContentDataSt += "Insert INTO tblInspectionList(ContentID,ClientID,SiteLocId,ScaffRef,SiteLocation,Location,CheckListId,RegisterID,InspectionID,CheckListContent,itemOrder,InspectionCheckListColor,Y,N,NA,IsNA,ActionRequired,ChecklistItem,DesignType,OwnerName) Values(" + contentId + ",'" + ClientID + "','" + SiteLocId + "','" + ScaffRef + "','" + SiteLocation.replace("'", "''") + "','" + Location.replace("'", "''") + "','" + CheckListId + "','" + RegisterID + "','" + InspectionID + "','" + CheckListContent1 + "'," + itemOrder + "," + InspectionCheckListColor + "," + Y + "," + N + "," + NA + "," + IsNA + ",'" + ActionRequired + "'," + ChecklistItem + ",'" + DesignType.replace("'", "''") + "','" + OwnerName.replace("'", "''") + "'),";
                                            // } else if (p != NewCheckListHeaderArray.length - 1) {
                                        } else if (i != CheckListContentArray.length - 1) {
                                            SqlContentDataSt += "(" + contentId + ",'" + ClientID + "','" + SiteLocId + "','" + ScaffRef + "','" + SiteLocation.replace("'", "''") + "','" + Location.replace("'", "''") + "','" + CheckListId + "','" + RegisterID + "','" + InspectionID + "','" + CheckListContent1 + "'," + itemOrder + "," + InspectionCheckListColor + "," + Y + "," + N + "," + NA + "," + IsNA + ",'" + ActionRequired + "'," + ChecklistItem + ",'" + DesignType.replace("'", "''") + "','" + OwnerName.replace("'", "''") + "'),";
                                        }
                                        //  if (p == NewCheckListHeaderArray.length - 1 && i == filterResult.length - 1) {
                                        if (i == CheckListContentArray.length - 1 ) {
                                            SqlContentDataSt += "(" + contentId + ",'" + ClientID + "','" + SiteLocId + "','" + ScaffRef + "','" + SiteLocation.replace("'", "''") + "','" + Location.replace("'", "''") + "','" + CheckListId + "','" + RegisterID + "','" + InspectionID + "','" + CheckListContent1 + "'," + itemOrder + "," + InspectionCheckListColor + "," + Y + "," + N + "," + NA + "," + IsNA + ",'" + ActionRequired + "'," + ChecklistItem + ",'" + DesignType.replace("'", "''") + "','" + OwnerName.replace("'", "''") + "');";
                                            console.log(SqlContentDataSt);

                                            db.transaction(function (tx) {
                                                tx.executeSql(SqlContentDataSt, [], function (tx, results) {
                                                    if (results.rows.length > 0) {
                                                        //GetCheckContentData();
                                                    }
                                                });
                                            });
                                        }



                                    }
                                    //GetCheckContentData();
                                }

                          //  }
                     //   }

                        //GetCheckContentData();
                       // showOfflineInspectionListGrid();
                    }
                });
            }, 1000);

        }


     // convert function for Append padding Zeros in JS.
        String.prototype.lpad = function(padString, length) {
            var str = this;
            while (str.length < length)
                str = padString + str;
            return str;
        }


        function DeleteGridData(InspectionID) {
            var flag=false;
            db.transaction(function (tx) {
                // tx.executeSql("select * from tblInspection where (InspStatus == 'Confirmed' Or InspStatus == 'Out of Service') and InspectionID='" + InspectionID + "'", [], function (tx, results) {

                // Comment On 03 Feb 2020 Delete Only  Scheduled inspection in Offline
               //  tx.executeSql("select * from tblInspection where InspStatus='Scheduled' and InspectionID='" + InspectionID + "'", [], function (tx, results) {
               // Code Add  On 03 Feb 2020 Delete All Type  inspection in Offline
                 tx.executeSql("select * from tblInspection where InspectionID='" + InspectionID + "'", [], function (tx, results) {
                    if (results.rows.length > 0) {
                        flag = true;
                        $("#customLoader").css("display", "block");
                        db.transaction(function (tx) {
                            // tx.executeSql("update tblInspection set IsDelete=true where InspectionID = '" + InspectionID + "'");
                            tx.executeSql("update tblInspection set IsDelete=true,InspImage='',InspSign='' where InspectionID = '" + InspectionID + "'");
                            setTimeout(function () {
                                var Res = { Result: "Success", Message: "Inspection is deleted Successfully ." };
                                CustomAlert(Res);
                                showOfflineInspectionListGrid();
                            }, 2000);
                        });
                    } else {
                 
                        var Res = { Result: "Failed", Message: "Cannot be deleted.This Inspection is Completed ." };
                        CustomAlert(Res);
                    
                    }
                });
            });

        }

        function showOfflineInspectionListGrid() {
            
            db.transaction(function (tx) {
                //tx.executeSql("Select distinct InspectionID,SiteLocId,SiteLocation,Location, ScaffRef ,RegisterID,CheckListId from tblInspectionList", [], function (tx, results) {

                // tx.executeSql("Select distinct tblInspectionList.InspectionID,SiteLocId,SiteLocation,Location, ScaffRef ,tblInspectionList.RegisterID,CheckListId,InspectionDate,InspStatus,name as InspectedBy,InspComments from tblInspectionList left join tblInspection on tblInspection.InspectionID=tblInspectionList.InspectionID", [], function (tx, results) {
                // Query Change On 07 Feb 2020
                // tx.executeSql("Select distinct tblInspectionList.InspectionID,SiteLocId,SiteLocation,Location, ScaffRef ,tblInspectionList.RegisterID,CheckListId,InspectionDate,InspStatus,name as InspectedBy,InspComments from tblInspectionList left join tblInspection on tblInspection.InspectionID=tblInspectionList.InspectionID where Isdelete==0", [], function (tx, results) {
                tx.executeSql("Select distinct tblInspectionList.InspectionID,SiteLocId,SiteLocation,Location, ScaffRef ,tblInspectionList.RegisterID,CheckListId,InspectionDate,InspStatus,name as InspectedBy,InspComments,BuildingNo,AssetNo,FloorLevel,MaterialType,ReasonForAccess from tblInspectionList left join tblInspection on tblInspection.InspectionID=tblInspectionList.InspectionID where Isdelete==0", [], function (tx, results) {

                    $('#InspectionList').DataTable().clear();
                    $('#InspectionList').DataTable().destroy();

                    if (results.rows.length > 0) {
                        var InspectionNewArray = jQuery.makeArray(results.rows.item(0));
                        var len = results.rows.length;
                        for (var count = 1; count < len; count++) {
                            InspectionNewArray[count] = results.rows.item(count);
                        }

                        var InspectionNewDataSet = InspectionNewArray;
                        var str = "";
                        for (var i = 0; i < InspectionNewDataSet.length; i++) {
                            var data = InspectionNewDataSet[i];
                            var strscaff1 = data.ScaffRef.toString();
                            var strscaff = strscaff1.lpad("0", 6); //(padding '00000' in scafref)
                            str += "<tr><td style='display:inline-flex'><img src='/images/fonts/fa_edit.png' id='edit_" + data.InspectionID + "_" + data.CheckListId + "' class='img-responsive gridActions' style='height: 22px;width: 22px;float: left;'/>&nbsp;<img src='/images/fonts/fa_offlinedelete.png' id='delete_" + data.InspectionID + "' class='img-responsive gridActions' style='height: 20px;width: 22px;'/></td>";
                            str += "<td>" + data.InspStatus + "</td>";
                            str += "<td>" + strscaff + "</td>";
                            // str += "<td>" + data.InspectionDate + "</td>";                      
                            //str += "<td>" + data.InspectedBy + "</td>";
                            //str += "<td>" + data.InspComments + "</td>";
                            //str += "<td>" + data.SiteLocation + "</td>";
                            //str += "<td>" + data.Location + "</td>";
                            str += "<td>" + data.BuildingNo + "</td>";
                            str += "<td>" + data.AssetNo + "</td>";
                            str += "<td>" + data.FloorLevel + "</td>";
                            str += "<td>" + data.MaterialType + "</td>";
                            str += "<td>" + data.ReasonForAccess + "</td></tr>";
                            //str += "<td>" + data.ScaffRef + "</td>";
                            //str += "<td>" + data.RegisterID + "</td>";
                            //str += "<td>" + data.CheckListId + "</td>";
                            //str += "<td>" + data.InspectionID + "</td>";
                            //str += "<td><img src='/images/fonts/fa_edit.png' id='edit_" + data.InspectionID + "_" + data.CheckListId + "' class='img-responsive gridActions' style='height: 22px;width: 22px;float: left;'/><img src='/images/fonts/fa_offlinedelete.png' id='delete_" + data.InspectionID + "' class='img-responsive gridActions' style='height: 20px;width: 22px;float: right;'/></td></tr>";

                        }

                        if (str != "") {
                            $("#loadOfflineInspectionList").html(str);
                            $("#customLoader").css("display", "none");
                            $("#OfflineInspectionList").css("display", "block");
                           
                          $(".gridActions").click(function () {     //Adding gridActions Class to Get RowId and Type Of Action
                                var GridAction = $(this).attr('id').split('_')[0];
                                var InspectionID = $(this).attr('id').split('_')[1];
                                var CheckListId = $(this).attr('id').split('_')[2];
                                var NewUrl = window.location.href.split('RID')[0];

                                var GetUrl = url.split("&offlinestatus")[0];
                                var RedirectUrl = GetUrl.split('OfflineInspectionList.html');
                              
                                if (GridAction == "edit") {
                                    //  UpdateRID(InspectionID);
                                    //alert(InspectionID);
                                    // window.location = RedirectUrl[0] + "Offline.html" + RedirectUrl[1] + "&InspectionID=" + InspectionID + "&CheckListId=" + CheckListId;

                                    // Store
                                    // sessionStorage.setItem("InspectionID", '+InspectionID+');
                                    sessionStorage.setItem("InspectionID", InspectionID);
                                    // Retrieve
                                    // document.getElementById("result").innerHTML = sessionStorage.getItem("lastname");
                                   

                                    window.location = RedirectUrl[0] + "Offline.html" + RedirectUrl[1];
                                    
                                 
                                } if (GridAction == "delete") {
                                    DeleteGridData(InspectionID);  //calling this function outside of executeSql block[only one execution is possible at a time]
                                }

                            });
                        }
                  
                    }
                    setTimeout(function () {
                        table = $('#InspectionList').DataTable({
                            "bPaginate": false,
                            "bFilter": false,
                            "bInfo": false,
                            scrollX:true,
                            scrollY: "60vh",
                        });
                        var NewUrl = window.location.href.split('RID')[0];

                        var GetUrl = url.split("&offlinestatus")[0];
                        var RedirectUrl = GetUrl.split('OfflineInspectionList.html');
                       var DefaultPageLoad= RedirectUrl[0] + "Offline.html" + RedirectUrl[1];

                        ////  $('#defaultLoadIframe').attr('src', 'http://localhost:10454/Offline.html?UID=2LDeoXcCqCeHgZss1xmrGw==&UserRole=utmkgkoFULt8gTbrH%20uQ6A==&id=iEKccYo70rw3HmhXUsJ7OQ==');
                       $('#defaultLoadIframe').attr('src', DefaultPageLoad);

                    }, 1000);
                });
                $("#customLoader").css("display", "none");
            });

        }

        