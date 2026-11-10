

// Company Profile Section Code Start Here
$(document).ready(function () {
    BindallDropdown();
    
    setInputFilter(document.getElementById("txtManHrsRatio"), function (value) {
    return /^-?\d*[.,]?\d*$/.test(value);
    });
    setInputFilter(document.getElementById("txtManHrsAvai"), function (value) {
        return /^-?\d*[.,]?\d*$/.test(value);

    });
    

    $('#txtContact').inputmask({ "mask": "(999)999-9999" });
    $('#txtUk').inputmask({ "mask": "(99999)999999" });
    //$('#txtUk').inputmask({ "mask": "(#####) ######" });
    $('#txtzip').inputmask({ "mask": "9999" });
    $('#txtSitezip').inputmask({ "mask": "9999" });

    var SiteID = getParameterByName('SiteID');
    var LocationID = getParameterByName('LocationID');
    var BuildingID = getParameterByName('BuildingID');
    var AssetID = getParameterByName('AssetID');
    var AddSite = $('#TxtAddSite').val();
    var AddLocation = $('#TxtAddLocation').val();
    var AddBuilding = $('#TxtAddBuilding').val();
    var AddAsset = $('#TxtAddAsset').val();

    var AddSite1 = 'AddSite';
    var AddLocation1 = 'AddLocation';
    var AddBuilding1 = 'AddBuilding';
    var AddAsset1 = 'AddAsset';

    var DecryptData = Decrypt([SiteID, LocationID, BuildingID, AssetID]);
    AddSite = Decrypt(AddSite);
    AddLocation = Decrypt(AddLocation);
    AddBuilding = Decrypt(AddBuilding);
    AddAsset = Decrypt(AddAsset);
    SiteID = DecryptData[0];
    LocationID = DecryptData[1];
    BuildingID = DecryptData[2];
    AssetID = DecryptData[3];

    if (LocationID != "" || LocationID != null) {
        $('#hdnLocationID').val(LocationID);
    }
    if (BuildingID != "" || BuildingID != null) {
        $('#hdnBuildingID').val(BuildingID);
    }
    if (AssetID != "" || AssetID != null) {
        $('#hdnAssetID').val(AssetID);
    }

    ShowClientDetails();
    GetSiteData();
    //DisbSkipMat();

    if (AddSite1 == AddSite) {

        $('#btnAddNewSite').click();
        $('#divhideContent').css('display', 'none');
        $('#divsaveandbacksite').css('display', 'block');
        $('#divsavesite').css('display', 'none');
        $('#divCancel').css('display', 'none');
        $('#divCancelandgoback').css('display', 'block');
        $('#btngoback').css('display', 'block');
        $('#btnback').css('display', 'none');
    }

    if (AddLocation1 == AddLocation) {
        $('#divhideContent').css('display', 'none');
        $('#divClose').css('display', 'none');
        $('#divCloseandgoback').css('display', 'block');
        $('#SiteLocationExpend_' + SiteID).click();
        $('#divLocationSavePopup').css('display', 'block');
        $('#divSaveandBackLocation').css('display', 'block');
        $('#h8').css('display', 'none');
        $('#h8andgoback').css('display', 'block');
        $('#divSaveLocation').css('display', 'none');
        $('#TxtAddLocation').val("");
    }

    if (AddBuilding1 == AddBuilding) {
        $('#divhideContent').css('display', 'none');
        $('#SiteLocationExpend_' + SiteID).click();
        $('#divSaveandBackBuilding').css('display', 'block');
        $('#divSaveBuilding').css('display', 'none');
        $('#divBuildingClose').css('display', 'none');
        $('#btnclose').css('display', 'none');
        $('#btncloseback').css('display', 'block');
        $('#divBuildingCloseandback').css('display', 'block');

    }

    if (AddAsset1 == AddAsset) {
        $('#divhideContent').css('display', 'none');
        $('#SiteLocationExpend_' + SiteID).click();
        $('#divSaveandBackAsset').css('display', 'block');
        $('#divSaveAsset').css('display', 'none');
        $('#divAssetClose').css('display', 'none');
        $('#btnclose1').css('display', 'none');
        $('#btncloseandback').css('display', 'block');
        $('#divAssetCloseandback').css('display', 'block');
    }

   if(getParameterByName('RID')==null){
       $('#btnCAncel').css('display', 'none');
    }

});

function BindallDropdown() {
    var id = getParameterByName('id');
    $.ajax({
        url: '/EditClient/BindallDropdown',
        async: false,
        data: { id: id },
        success: function (response) {

            data = response;

            var ddlState = "<option value='' disabled selected='selected'></option>";
            $.each(response.StateDropdown, function (idx, obj) {
                if (obj != null) {
                    ddlState += '<option value="' + obj.state + '">' + obj.state + '</option>';
                }
            });

            var ddlpopCountry = "<option value='' disabled selected='selected'>select---</option>";
            $.each(response.CountryDropdown, function (idx, obj) {
                if (obj != null) {
                    ddlpopCountry += '<option value="' + obj.Country + '">' + obj.Country + '</option>';
                }
            });

            $('#ddlstate').empty().append(ddlState);
            $('#ddlstates').empty().append(ddlState);  // Site State Dropdown Bind
            $('#ddlpopCountry').empty().append(ddlpopCountry);
            $('#ddlCountry').empty().append(ddlpopCountry);
            
        }
    });
}

function btnaddnew_Click() {
    $('#Adddocs').css("display", "block");
}


function ShowClientDetails() {
    $('#LoadingPanel1').show();
    var id = getParameterByName('id');
    $.ajax({
        type: "POST",
        url: "/EditClient/ShowClientDetails",
        data: JSON.stringify({ id: id }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            var str = "";
            var GetData = response;

            var ClientId = GetData.ClientId == null ? "" : GetData.ClientId;
            var Name = GetData.Name == null ? "" : GetData.Name;
            var Address = GetData.Address == null ? "" : GetData.Address;
            var City = GetData.City == null ? "" : GetData.City;
            var State = GetData.State == null ? "" : GetData.State;
            var Zip = GetData.Zip == null ? "" : GetData.Zip;
            var CellPhone = GetData.CellPhone == null ? "" : GetData.CellPhone;
            var EmailID = GetData.EmailID == null ? "" : GetData.EmailID;
            var Status = GetData.Status == null ? "" : GetData.Status;
            var InspTimeFrame = GetData.InspTimeFrame == null ? "" : GetData.InspTimeFrame;
            var ClientImage = GetData.ClientImage == null ? "" : GetData.ClientImage;
            var ChecklistId = GetData.ChecklistId == null ? "" : GetData.ChecklistId;
            var StandardWorkingHrs = GetData.StandardWorkingHrs == null ? "" : GetData.StandardWorkingHrs;
            var Country = GetData.Country == null ? "" : GetData.Country;
            var Plans = GetData.Plans == null ? "" : GetData.Plans;
            var WeekStartDay = GetData.WeekStartDay == null ? "" : GetData.WeekStartDay;
            var PlannedStandingDuration = GetData.PlannedStandingDuration == null ? "" : GetData.PlannedStandingDuration;
            var ManHrsRatio = GetData.ManHrsRatio == null ? "" : GetData.ManHrsRatio;
            var IPAuthentication = GetData.IPAuthentication == null ? "" : GetData.IPAuthentication;
            var ManHoursManagement = GetData.ManHoursManagement == null ? "" : GetData.ManHoursManagement;
            var IsMainClient = GetData.IsMainClient == null ? "" : GetData.IsMainClient;
            var OfflineEnabled = GetData.OfflineEnabled == null ? "" : GetData.OfflineEnabled;
            var HireInventory=GetData.HireInventory==null ? false:GetData.HireInventory;
            var LongTermStructure = GetData.LongTermStructure == null ? false : GetData.LongTermStructure;
            var ClientInventoryMailingList = GetData.ClientInventoryMailingList == null ? "" : GetData.ClientInventoryMailingList;
            var ComparisonDashboard = GetData.ComparisonDashboard == null ? false : GetData.ComparisonDashboard;
            var OfflineInspEnable = GetData.OfflineInspEnable == null ? false : GetData.OfflineInspEnable;
            var NoOfScafRefGenerate = GetData.NoOfScafRefGenerate == null ? false : GetData.NoOfScafRefGenerate;
            var HideMainDepotInventory = GetData.HideMainDepotInventory == null ? false : GetData.HideMainDepotInventory;
          
            $('#txtclientName').val(Name);
            $('#lblclient').text(Name);
            $('#txtcity').val(City);
            $('#txtemail').val(EmailID);
            $('#txtClientInventoryMailingList').val(ClientInventoryMailingList);
            if (State != "") {
                $('#ddlstate').val(State);
            }
            if (Country != "") {
                $('#ddlCountry').val(Country);
            }

            if ($('#ddlCountry').val() == "Australia") {
                $('#txtContact').val(CellPhone);
                $('#txtContact').css('display', 'block');
                $('#txtzip').val(Zip);
                $('#txtzip').css('display', 'block');
                $('#lblstate').css('display', 'block');
                $('#lblcounty').css('display', 'none');

                //txtContact.Text = Convert.ToString(dt.Rows[0]["CellPhone"]);
                //txtContact.Visible = true;
                //txtzip.Text = Convert.ToString(dt.Rows[0]["Zip"]);
                //txtzip.Visible = true;
                //lblstate.Visible = true;
                //lblcounty.Visible = false;
            }
            else if ($('#ddlCountry').val() == "United Kingdom") {
                $('#txtUK').val(CellPhone);
                $('#txtUK').css('display', 'block');
                $('#txtpostcodeuk').val(Zip);
                $('#txtpostcodeuk').css('display', 'block');
                $('#lblstate').css('display', 'none');
                $('#lblcounty').css('display', 'block');

                //txtUK.Text = Convert.ToString(dt.Rows[0]["CellPhone"]);
                //txtUK.Visible = true;
                //txtpostcodeuk.Text = Convert.ToString(dt.Rows[0]["Zip"]);
                //txtpostcodeuk.Visible = true;
                //lblstate.Visible = false;
                //lblcounty.Visible = true;
            }
            else if ($('#ddlCountry').val() == "New Zealand") {
                $('#txtUK').val(CellPhone);
                $('#txtUK').css('display', 'block');
                $('#txtpostcodeuk').val(Zip);
                $('#txtpostcodeuk').css('display', 'block');
                $('#lblstate').css('display', 'none');
                $('#lblcounty').css('display', 'block');

                //txtUK.Text = Convert.ToString(dt.Rows[0]["CellPhone"]);
                //txtUK.Visible = true;
                //txtpostcodeuk.Text = Convert.ToString(dt.Rows[0]["Zip"]);
                //txtpostcodeuk.Visible = true;
                //lblstate.Visible = false;
                //lblcounty.Visible = true;
            }
            $('#txtAddress').val(Address);
            $('#txtStandardWorkingHrs').val(StandardWorkingHrs);
            if (InspTimeFrame!=""){
                $('#ddlInspectionTimeFrame').val(InspTimeFrame);
             }
            $('#ddlClientstatus').val(Status);
            if (HireInventory == true)
            {
                $('#chkHireInventory').prop('checked', true);
            }
            else
            {
                $('#chkHireInventory').prop('checked', false);
            }
            if (IPAuthentication == true) {
                $('#chkIPAuthentication').prop('checked', true);
            }
            else {
                $('#chkIPAuthentication').prop('checked', false);
            }
            if (ManHoursManagement == true) {
                $('#checkboxManHoursManagement').prop('checked', true);
            }
            else {
                $('#checkboxManHoursManagement').prop('checked', false);
            }
            if (OfflineEnabled == true) {
                $('#checkOfflineEnabled').prop('checked', true);
            }
            else {
                $('#checkOfflineEnabled').prop('checked', false);
            }
            if (ComparisonDashboard == true) {
                $('#chkComparisonDashboard').prop('checked', true);
            }
            else {
                $('#chkComparisonDashboard').prop('checked', false);
            }
            if (OfflineInspEnable == true) {
                $('#chkOfflineInspection').prop('checked', true);
            }
            else {
                $('#chkOfflineInspection').prop('checked', false);
            }
            if (NoOfScafRefGenerate == true) {
                $('#chkNOGenerate').prop('checked', true);
            }
            else {
                $('#chkNOGenerate').prop('checked', false);
            }
            if (HideMainDepotInventory == true) {
                $('#chkMainDepotInventory').prop('checked', true);
            }
            else {
                $('#chkMainDepotInventory').prop('checked', false);
            }
            if (LongTermStructure == true) {
                $('#ddlLongTermStructure').val(1);
            } else {
                $('#ddlLongTermStructure').val(0);
            }
            if (isMainClient == true) {
                $('#isMainClient').prop('checked', true);
            }
           if (Plans == "Plan1") {
                $('#Plan1').prop('checked', true);
            }
            else if (Plans == "Plan2") {
                $('#Plan2').prop('checked', true);
            }
            $('#ddlweekstartday').val(WeekStartDay);
            if (PlannedStandingDuration == "") {
                $('#txtplnstandDur').val(30);
            } else {
                $('#txtplnstandDur').val(PlannedStandingDuration);
            }
            if (ManHrsRatio == "") {
                $('#txtManHrsRatio').val(0.40);
            } else {
                $('#txtManHrsRatio').val(ManHrsRatio.toFixed(2));
            }
            $('#divdeleteimag').css('display', 'none');
            if (ClientImage != "") {
                $('#divdeleteimag').css('display', 'block');
                $('#btnImageSave').css('display', 'block');
                // var uri = new Uri(databaseCon.siteimagelink);

                //if (dt.Rows[0]["Image"].ToString() == "") {
                if (ClientImage == "") {
                    $('#imga').attr("src", "/images/noimg.jpg");
                    $('#getimage').val('');
                }
                else {
                    //imga.Attributes.Add("src", ("/images/sites/" + dt.Rows[0]["Image"].ToString()));
                    //getimage.Value = dt.Rows[0]["Image"].ToString();
                    //ViewState["deleteimage"] = dt.Rows[0]["Image"].ToString();
                    //ViewState["imgname"] = dt.Rows[0]["Image"].ToString();
                    $('#hdnimgname').val(ClientImage);
                    $('#imga').attr("src", "/images/clientlogo/" + ClientImage + "");
                    $('#getimage').val(ClientImage);
                }
            }
            else {
                $('#imga').attr("src", "/images/noimg.jpg");
                $('#getimage').val('');
            }
          $('#LoadingPanel1').hide();

        }
    })
}

function savepopAddnewcompany_Click() {
    $('#LoadingPanel1').show();
    var UID = getParameterByName('UID');
    var txtCompanyName = $('#txtCompanyName').val();
    var ddlpopCountry = $('#ddlpopCountry').val();
    var ddlweekstartday = $('#ddlweekstartday').val();
    if (txtCompanyName != "") {
        if (ddlpopCountry != "") {

            $.ajax({
                type: "POST",
                url: "/EditClient/savepopAddnewcompany_Click",
                data: JSON.stringify({ UID: UID,txtCompanyName:txtCompanyName, ddlpopCountry: ddlpopCountry, ddlweekstartday: ddlweekstartday }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                success: function (response) {
                    var ClientID = response.ClientID;
                    var UID = getParameterByName('UID');
                    var UserRole = getParameterByName('UserRole');
                    //var SiteID=$('#hdnSiteLocationID').val();
                    if (response.Result == "Success") {
                        CustomAlert(response);
                        window.location.href = '/EditClient/EditClient?UID=' + UID + '&UserRole=' + UserRole + '&id=' + ClientID;
                        //window.location.href = '/EditClient/EditClient?UID=' + UID + '&UserRole=' + UserRole + '&id=' + ClientID + '&SiteID=' + SiteID;
                    } else {
                        CustomAlert(response);
                        $('#LoadingPanel1').hide();
                    }
                }
            });
        } else {
            var Response = { Result: "Failed", Message: "Please Enter Company Name." }
            CustomAlert(Response);
        }
    } else {
        var Response = { Result: "Failed", Message: "Please Enter Company Name." }
        CustomAlert(Response);
    }

}

function validateEmail(email) {
    if (email != "") {

        const re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        //return re.test(String(email).toLowerCase());
        if (re.test(String(email).toLowerCase())) {
            return true;
        } else {
            return false;
        }
    } else {
        return true;
    }
}

function checkActualPlanningWeek() {
    var id = getParameterByName('id');
    $.ajax({
        type: "POST",
        url: "/EditClient/checkActualPlanningWeek",
        data: JSON.stringify({ id: id }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response.Result == "Success") {
                
            } else {
                CustomAlert(response);
                ShowClientDetails();
            }
        }
    });
}

function btnSave_Click(){
    $('#LoadingPanel1').show();
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var ddlCountry = $('#ddlCountry').val();
    var txtclientName = $('#txtclientName').val();
    var txtAddress= $('#txtAddress').val();
    var txtcity = $('#txtcity').val();
    var ddlstate = $('#ddlstate').val();
    var txtzip= $('#txtzip').val();
    var txtpostcodeuk = $('#txtpostcodeuk').val();
    var txtUK = $('#txtUK').val();
    var txtContact = $('#txtContact').val();
    var txtemail= $('#txtemail').val();
    var ddlClientstatus = $('#ddlClientstatus').val();
    var ddlInspectionTimeFrame = $('#ddlInspectionTimeFrame').val();
    var txtStandardWorkingHrs= $('#txtStandardWorkingHrs').val();
    var ddlweekstartday = $('#ddlweekstartday').val();
    var txtplnstandDur = $('#txtplnstandDur').val();
    var txtManHrsRatio = $('#txtManHrsRatio').val();
    var chkIPAuthentication = $('#chkIPAuthentication').is(':checked');
    var checkboxManHoursManagement = $('#checkboxManHoursManagement').is(':checked');
    var isMainClient = $('#isMainClient').is(':checked');
    var checkOfflineEnabled = $('#checkOfflineEnabled').is(':checked');
    var ddlLongTermStructure = $('#ddlLongTermStructure').val();
    var txtClientInventoryMailingList = $('#txtClientInventoryMailingList').val();
    var plan = "Plan2";
    var chkHireInventory = $('#chkHireInventory').is(':checked');
    var chkComparisonDashboard = $('#chkComparisonDashboard').is(':checked');
    var chkOfflineInspection = $('#chkOfflineInspection').is(':checked');
    var chkNOGenerate = $('#chkNOGenerate').is(':checked');
    var chkMainDepotInventory = $('#chkMainDepotInventory').is(':checked');
    
    if (txtclientName != "") {
        if (ddlpopCountry != "") {
            if (ddlstate != "" && ddlstate != null) {
               // if (IsEmailValid(txtemail) == true) {
                if (validateEmail(txtemail) == true) {
            $.ajax({
                type: "POST",
                url: "/EditClient/btnSave_Click",
                data: JSON.stringify({ ClientID: id, UID: UID, plan: plan, ddlCountry: ddlCountry, txtclientName: txtclientName, txtAddress: txtAddress, txtcity: txtcity, ddlstate: ddlstate, txtzip: txtzip, txtpostcodeuk: txtpostcodeuk, txtUK: txtUK, txtContact: txtContact, txtemail: txtemail, ddlClientstatus: ddlClientstatus, ddlInspectionTimeFrame: ddlInspectionTimeFrame, txtStandardWorkingHrs: txtStandardWorkingHrs, ddlweekstartday: ddlweekstartday, txtplnstandDur: txtplnstandDur, txtManHrsRatio: txtManHrsRatio, chkIPAuthentication: chkIPAuthentication, checkboxManHoursManagement: checkboxManHoursManagement, isMainClient: isMainClient, checkOfflineEnabled: checkOfflineEnabled, chkHireInventory: chkHireInventory, ddlLongTermStructure: ddlLongTermStructure, txtClientInventoryMailingList: txtClientInventoryMailingList, chkComparisonDashboard: chkComparisonDashboard, chkOfflineInspection: chkOfflineInspection, chkNOGenerate: chkNOGenerate, chkMainDepotInventory: chkMainDepotInventory }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                success: function (response) {
                    UpdateImagePath();
                    var ClientID = response.EncriptClientID;
                    var UID = getParameterByName('UID');
                    var UserRole = getParameterByName('UserRole');
                    if (response.Result == "Success") {
                        CustomAlert(response);
                        window.location.href = '/EditClient/EditClient?UID=' + UID + '&UserRole=' + UserRole + '&id=' + ClientID;
                    } else {
                        CustomAlert(response);
                        $('#LoadingPanel1').hide();
                    }
                }
            });
                } else {
                    var Response = { Result: "Failed", Message: "Please Enter Valid Email." }
                    CustomAlert(Response);
                    $('#txtemail').addClass('requiredColumn');
                    $('#LoadingPanel1').hide();
                }
            } else {
                var Response = { Result: "Failed", Message: "Please Select State." }
                CustomAlert(Response);
                $('#ddlstate').addClass('requiredColumn');
                $('#LoadingPanel1').hide();
            }
        } else {
            var Response = { Result: "Failed", Message: "Please Select Country." }
            CustomAlert(Response);
            $('#ddlpopCountry').addClass('requiredColumn');
            $('#LoadingPanel1').hide();
        }
    } else {
        var Response = { Result: "Failed", Message: "Please Enter Company Name." }
        CustomAlert(Response);
        $('#txtclientName').addClass('requiredColumn');
        $('#LoadingPanel1').hide();
    }
}

function UpdateImagePath() {
 //   $('#LoadingPanel1').show();
  
    var fileData = new FormData();
    var files;

    // Checking whether FormData is available in browser
    if (window.FormData !== undefined) {
        if (files == null || files == "") {
            var fileUpload = $('#radUpdogImg').get(0);
            files = fileUpload.files;
        }
        var fileUpload = $('#radUpdogImg').get(0);
        // Create FormData object
        //  var fileData = new FormData();
        if (files.length != 0) {

            var FileName = files[0].name;
            var FileSize = files[0].size;
            var extension = FileName.substring(FileName.lastIndexOf('.') + 1).toLowerCase();
            fileData.append(files[0].name, files[0]);

        }
    }
   
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var txtclientName=$('#txtclientName').val();
    var imagename=$('#imagename').val();
    var radUpdogImg = $('#radUpdogImg').val();
    var hdnimgname=$('#hdnimgname').val();
    fileData.append('id', id);
    fileData.append('txtclientName', txtclientName);
    fileData.append('imagename', imagename);
    fileData.append('imgname', hdnimgname);
    fileData.append('radUpdogImg', radUpdogImg);

   // fileData.append('image', image);
    $.ajax({
        url: "/EditClient/UpdateImagePath",
        type: 'POST',
        contentType: false, // Not to set any content header
        processData: false, // Not to process data
        data: fileData,
        success: function (response) {
            var Result = response;
        }
    });
}

// Company Profile Section Code End Here



// Site Code Start Here
var tableSite = "";
var tableblLocation = "";
var tableBuilding = "";
var tableAsset = "";

function GetSiteData() {
    var Siteid = getParameterByName('SiteID');
    var DecryptData = Decrypt([Siteid]);
    Siteid = DecryptData[0];
    $('#LoadingPanel1').show();
 var id=getParameterByName('id');
    $.ajax({
        type: "POST",
        url: "/EditClient/GetSiteData",
        data: JSON.stringify({ id: id}),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            var str = "";
            var GetData = response;
            $('#tblSiteGrid').DataTable().clear();
            $('#tblSiteGrid').DataTable().destroy();

            if (GetData.length > 0) {
                $('#tblSiteGrid').css("display", "block");
                $('#SiteLocationExpend_' + Siteid).click();

                for (var i = 0; i < GetData.length; i++) {
                    var SiteID = GetData[i].ID == null ? "" : GetData[i].ID;
                    var SiteLocation = GetData[i].SiteLocation == null ? "" : GetData[i].SiteLocation;
                    var Address = GetData[i].Address == null ? "" : GetData[i].Address;
                    var City = GetData[i].City == null ? "" : GetData[i].City;
                    var State = GetData[i].State == null ? "" : GetData[i].State;
                    var Zip = GetData[i].Zip == null ? "" : GetData[i].Zip;
                    var ClientID = GetData[i].ClientID == null ? "" : GetData[i].ClientID;
                    var SiteLocationStatus = GetData[i].SiteLocationStatus == null ? "" : GetData[i].SiteLocationStatus;
                    var OwnerName = GetData[i].OwnerName == null ? "" : GetData[i].OwnerName;
                    var Area = GetData[i].Area == null ? "" : GetData[i].Area;
                    var DistributionList = GetData[i].DistributionList == null ? "" : GetData[i].DistributionList;
                    var Country = GetData[i].Country == null ? "" : GetData[i].Country;
                    var SiteLat = GetData[i].SiteLat == null ? "" : GetData[i].SiteLat;
                    var SiteLong = GetData[i].SiteLong == null ? "" : GetData[i].SiteLong;
                    var ManHoursAvailable = GetData[i].ManHoursAvailable == null ? "" : GetData[i].ManHoursAvailable;
                    var MaintainInventory = GetData[i].MaintainInventory == null ? false : GetData[i].MaintainInventory;
                    var CycloneManagement = GetData[i].CycloneManagement == null ? false : GetData[i].CycloneManagement;
                    var ManHoursManagement = GetData[i].ManHoursManagement == null ? false : GetData[i].ManHoursManagement;
                    var MaintainInventoryAtJob = GetData[i].MaintainInventoryAtJob == null ? false : GetData[i].MaintainInventoryAtJob;
                    var GPS = GetData[i].GPS == null ? false : GetData[i].GPS;
                    var FloorLevel = GetData[i].FloorLevel == null ? false : GetData[i].FloorLevel;
                    var EnabledGPSSitePin = GetData[i].EnabledGPSSitePin == null ? false : GetData[i].EnabledGPSSitePin;
                    var EnabledCaptureGps = GetData[i].EnabledCaptureGps == null ? false : GetData[i].EnabledCaptureGps;
                    var chkOffSystemInspection = GetData[i].OffSystemInspection == null ? "false" : GetData[i].OffSystemInspection;
                    var InventoryMailingList = GetData[i].InventoryMailingList == null ? "" : GetData[i].InventoryMailingList;
                    var SkipMaterial = GetData[i].SkipMaterial == null ? "" : GetData[i].SkipMaterial;
                    var chkPipelineInv = GetData[i].PipeLineInvHide == null ? false : GetData[i].PipeLineInvHide;

                    str += '<tr " class="clone"  cellspacing="0" border="0" style="width:100%">'
                    str += '<td style="display:none"></td>'
                    str += '<td  class="tdbtnExpend" style="width:5%;"><a id="SiteLocationExpend_' + SiteID + '" class="expand fa fa-angle-right"  style="font-size:19px;"></a></td>';
                    str += '<td title="Click here to edit Site." style="cursor:pointer;"><a id="SiteID_' + SiteID + '" class="btnSiteEdit"><i class="fa fa-pencil"></i>&nbsp;</a></td>';
                    str += '<td  title="Click here for Delete Location" style="cursor:pointer;width:5%;"><i class="fa fa-trash " onclick=ShowDeletePopup("' + SiteID + '","SiteMaster")></i></td>';
                    str += '<td id="OwnerName_' + SiteID + '">' + OwnerName + '</td>'
                    str += '<td style="width:100%;" id="SiteLocation_' + SiteID + '">' + SiteLocation + '</td>'
                    str += '<td id="Address_' + SiteID + '">' + Address + '</td>'
                    str += '<td id="City_' + SiteID + '">' + City + '</td>'
                    str += '<td id="State_' + SiteID + '">' + State + '</td>'
                    str += '<td id="Zip_' + SiteID + '">' + Zip + '</td>'
                    str += '<td id="SiteLocationStatus_' + SiteID + '">' + SiteLocationStatus + '</td>'
                    str += '<td id="DistributionList_' + SiteID + '" style="display:none">' + DistributionList + '</td>'
                    str += '<td id="SiteLat_' + SiteID + '" style="display:none">' + SiteLat + '</td>'
                    str += '<td id="SiteLong_' + SiteID + '" style="display:none">' + SiteLong + '</td>'
                    str += '<td id="ManHoursAvailable_' + SiteID + '" style="display:none">' + ManHoursAvailable + '</td>'
                    str += '<td id="MaintainInventory_' + SiteID + '" style="display:none">' + MaintainInventory + '</td>'
                    str += '<td id="MaintainInventoryAtJob_' + SiteID + '" style="display:none">' + MaintainInventoryAtJob + '</td>'
                    str += '<td id="CycloneManagement_' + SiteID + '" style="display:none">' + CycloneManagement + '</td>'
                    str += '<td id="ManHoursManagement_' + SiteID + '" style="display:none">' + ManHoursManagement + '</td>'
                    str += '<td id="GPS_' + SiteID + '" style="display:none">' + GPS + '</td>'
                    str += '<td id="FloorLevel_' + SiteID + '" style="display:none">' + FloorLevel + '</td>'
                    str += '<td id="EnabledGPSSitePin_' + SiteID + '" style="display:none">' + EnabledGPSSitePin + '</td>'
                    str += '<td id="EnabledCaptureGps_' + SiteID + '" style="display:none">' + EnabledCaptureGps + '</td>'
                    str += '<td id="chkOffSystemInspection_' + SiteID + '" style="display:none">' + chkOffSystemInspection + '</td>'
                    str += '<td id="InventoryMailingList_' + SiteID + '" style="display:none">' + InventoryMailingList + '</td>'
                    str += '<td id="SkipMaterialList_' + SiteID + '" style="display:none">' + SkipMaterial + '</td>'
                    str += '<td id="chkPipelineInv_' + SiteID + '" style="display:none">' + chkPipelineInv + '</td>'
                    str += '</tr>'
                }

                $('#SiteLocationExpend_' + Siteid).click();

                $('#tbodySiteGrid').append(str);
                $('#LoadingPanel1').css('display', 'none');

            }
            tableSite = $('#tblSiteGrid').DataTable({
                
                searching: true,
                paging: true,
                info: false,
                autoWidth: true,
                length: false,
                sort: false,
                scrollX: true,
                scrollCollapse: true,
                scrollY: "60vh",
                //"pageLength": 150,
                //Added on (31-05-2024) Pagelength Set to 300 because it was not displaying all the records on front end..
                //"pageLength": 300,
                //(27-06-2024 by s.sourabh)we can set the pageLength option to display all rows by setting it to a very high number or by using the special value -1, which is recognized by DataTables as "display all records."
                "pageLength": -1,

                initComplete: function () {
                    // $("#dataforgrid_length").children().html('<button id="btnAddNewSite"  type="button" class="w3-btn w3-teal fa fa-plus">&nbsp;Add New Sites</button>');
                    $("#tblSiteGrid_length").children().html('<button id="btnAddNewSite"  type="button" class="w3-btn w3-teal fa fa-plus">&nbsp;Add New Sites</button>');

                    $("#btnAddNewSite").click(function () {
                        // hideCheckBox();
                        $('#chkboxSkipMaterial').prop('disabled', false);
                        SetdefaultValues();
                        var UserRole = getParameterByName("UserRole");
                        var DecriptValues = Decrypt([UserRole]);
                        UserRole = DecriptValues[0];
                        if (UserRole != 10) {
                           hidedivGpsSitePin();
                        }

                        if ($('#ddlCountry').val() == "Australia") {
                            $("#txtSitezip").css('display', 'block');
                        } else {
                            $("#txtukSitepost").css('display', 'block');
                        }
                        $('#hdnSiteLocationID').val("");
                        $("#checkboxMaintainInventory").prop('checked', true);
                        var hdnManHoursManagementCheck = $('#hdnManHoursManagementCheck').val();
                        if (hdnManHoursManagementCheck == "True") {
                            $("#chkboxManHoursManagement").prop('checked', true);
                        } else {
                            hideManHour();
                            $("#chkboxManHoursManagement").prop('checked', false);
                        }
                        $("#btngps").prop('checked', true);
                        //$('#SitePopup').show();
                        $('#SitePopup').modal('show');
                       
                    });

                    $(".btnSiteEdit").click(function () {
                        event.stopImmediatePropagation();
                        //hideCheckBox();
                        var hdnManHoursManagementCheck = $('#hdnManHoursManagementCheck').val();
                        if (hdnManHoursManagementCheck == "False") {
                            hideManHour();
                        }
                        var UserRole = getParameterByName("UserRole");
                        var DecriptValues = Decrypt([UserRole]);
                        UserRole = DecriptValues[0];
                        if (UserRole != 10) {
                            hidedivGpsSitePin();
                        }
                       
                        if ($('#ddlCountry').val()=="Australia"){
                            $("#txtSitezip").css('display','block');
                        } else {
                            $("#txtukSitepost").css('display','block');
                        }
                        var SiteID = this.id.split('_')[1];
                        var OwnerName = $('#OwnerName_' + SiteID).text();
                        var SiteLocation = $('#SiteLocation_' + SiteID).text();
                        var Address = $('#Address_' + SiteID).text();
                        var City = $('#City_' + SiteID).text();
                        var State = $('#State_' + SiteID).text();
                        var Zip = $('#Zip_' + SiteID).text();
                        var SiteLocationStatus = $('#SiteLocationStatus_' + SiteID).text();
                        var DistributionList = $('#DistributionList_' + SiteID).text();
                        var SiteLat = $('#SiteLat_' + SiteID).text();
                        var SiteLong = $('#SiteLong_' + SiteID).text();
                        var ManHoursAvailable = $('#ManHoursAvailable_' + SiteID).text();
                        if (ManHoursAvailable == "") {
                            ManHoursAvailable = "0";
                        }
                        var CycloneManagement = $('#CycloneManagement_' + SiteID).text();
                        var ManHoursManagement = $('#ManHoursManagement_' + SiteID).text();
                        var MaintainInventory = $('#MaintainInventory_' + SiteID).text();
                        var MaintainInventoryAtJob = $('#MaintainInventoryAtJob_' + SiteID).text();
                        var GPS = $('#GPS_' + SiteID).text();
                        var FloorLevel = $('#FloorLevel_' + SiteID).text();
                        var EnabledGPSSitePin = $('#EnabledGPSSitePin_' + SiteID).text();
                        var EnabledCaptureGps = $('#EnabledCaptureGps_' + SiteID).text();
                        var chkOffSystemInspection = $('#chkOffSystemInspection_' + SiteID).text();
                        var InventoryMailingList = $('#InventoryMailingList_' + SiteID).text();
                        var SkipMaterial = $('#SkipMaterialList_' + SiteID).text();
                        
                        $('#chkboxSkipMaterial').prop('disabled', false);
                        DisbSkipMat(SiteID);

                        var chkPipelineInv = $('#chkPipelineInv_' + SiteID).text();

                        $('#chkPipelineInv').prop('disabled', false);
                        PipeInventory(SiteID);

                        $('#hdnSiteLocationID').val(SiteID);
                        $("#txtOwnerName").val(OwnerName);
                        $("#txtsitelocation").val(SiteLocation);
                        $("#txtaddress").val(Address);
                        $("#txtSitecity").val(City);
                        $("#ddlstates").val(State);
                        $("#txtSitezip").val(Zip);
                        $("#txtukSitepost").val(Zip);
                        $("#ddlStatuss").val(SiteLocationStatus);
                        $("#txtdistriList").val(DistributionList);
                        $("#txtlat").val(SiteLat);
                        $("#txtlong").val(SiteLong);
                        $("#txtManHrsAvai").val(ManHoursAvailable);
                        // $("#txtManHrsAvai").val(ManHoursAvailable.toFixed(2));
                        $("#txtManHrsAvai").val(numberWithCommas(parseFloat(ManHoursAvailable).toFixed(2)));
                        if (MaintainInventory == "true") {
                         $("#checkboxMaintainInventory").prop('checked',true);
                        } else {
                            $("#checkboxMaintainInventory").prop('checked', false);
                        }
                        if (CycloneManagement == "true") {
                            $("#checkboxCycloneManagement").prop('checked', true);
                        } else {
                            $("#checkboxCycloneManagement").prop('checked', false);
                        }
                        if (ManHoursManagement == "true") {
                            $("#chkboxManHoursManagement").prop('checked', true);
                        } else {
                            $("#chkboxManHoursManagement").prop('checked', false);
                        }
                        if (MaintainInventoryAtJob == "true") {
                            $("#checkboxMaintainInventoryAtJob").prop('checked', true);
                        } else {
                            $("#checkboxMaintainInventoryAtJob").prop('checked', false);
                        }
                        if (GPS == "true") {
                         $("#btngps").prop('checked', true);
                        } else {
                            $("#btngps").prop('checked', false);
                        }
                        if (FloorLevel == "true") {
                        
                        $("#btnfloorlevel").prop('checked', true);
                        } else {
                        $("#btnfloorlevel").prop('checked', false);
                        }
                        if (EnabledGPSSitePin == "true") {
                            $("#chkboxEnabledGPSSitePin").prop('checked', true);
                        } else {
                            $("#chkboxEnabledGPSSitePin").prop('checked', false);
                        }
                        if (EnabledCaptureGps == "true") {
                            $("#chkboxEnabledCaptureGps").prop('checked', true);
                        }
                         else {
                         $("#chkboxEnabledCaptureGps").prop('checked', false);
                        }
                        if (chkOffSystemInspection == "true") {
                         $("#chkOffSystemInspection").prop('checked', true);
                        } else {
                         $("#chkOffSystemInspection").prop('checked', false);
                        }

                        if (chkPipelineInv == "true") {
                            $("#chkPipelineInv").prop('checked', true);
                        } else {
                            $("#chkPipelineInv").prop('checked', false);
                        }

                        $("#txtMailingList").val(InventoryMailingList);
                        if (SkipMaterial == "true") {
                            $("#chkboxSkipMaterial").prop('checked', true);
                        } else {
                            $("#chkboxSkipMaterial").prop('checked', false);
                        }
                      

                        //$('#SitePopup').show();
                        $('#SitePopup').modal('show');
                    });
                },
                "drawCallback": function () {
                    $('.fa-angle-right').click(function (eve) {
                        eve.stopImmediatePropagation();
                        var tr = $(this).closest('tr');
                        //var row = table.row(tr);
                        var row = tableSite.row(tr);
                        var SiteID = this.id.split('_')[1];

                        var visible = $(this).parent().parent().hasClass("shown");
                        var anglechange = $(this).hasClass("fa-angle-right");
                        if (anglechange == true) {
                            var x = tr.children()[0];
                            $(this).removeClass("fa-angle-right");
                            $(this).addClass("fa-angle-down");
                        }
                        else {
                            $(this).addClass("fa-angle-right");
                            $(this).removeClass("fa-angle-down");
                        }
                        if (visible == true) {
                              $(this).parent().parent().next('tr').remove();
                          //  $(this).parent().parent().next('tr').empty();
                            $(this).parent().parent().removeClass('shown');
                        }
                        else {
                            var Temp=GetLocationData(SiteID);
                            // row.child(GetLocationData(SiteID)).show();
                            row.child(Temp).show();
                            row.child.show();
                            tr.addClass('shown');
                        }
                    });
                }

            });

           // else {
            //    // No Data Code here
            //    $('#tblSiteGrid').css("display", "none");
            //    $('#LoadingPanel1').css('display', 'none');
            //}
        }
    })

}

// Site Save Start Here
  function CheckRequiredfields() {
       var requiredInput = false;
       $('#SitePopup').find('input, select').each(function () {
           if ($(this).prop('required') == true && ($(this).val() == "" || $(this).val() == null)) {
               if (requiredInput == false) {
                   requiredInput = true;
               }
               $(this).addClass('requiredColumn');
           } else {
               $(this).removeClass('requiredColumn');
           }
       });
       return requiredInput;
   }

function btnSiteSave(check, showpopup) {

    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var sitelocdid = $('#hdnSiteLocationID').val();
    var txtsitelocation = $("#txtsitelocation").val();
    var txtaddress = $("#txtaddress").val();
    var txtcity = $("#txtSitecity").val();
    var ddlstates = $("#ddlstates").val();
    //var txtzip = $("#txtSitezip").val();
    //var txtukpost = $("#txtukSitepost").val();
    var txtzip = "";
    var txtukpost = "";
    if ($('#ddlCountry').val() == "Australia") {
        txtzip = $("#txtSitezip").val();
        txtukpost = "";
    } else {
        txtzip = "";
        txtukpost = $("#txtukSitepost").val();
    }
    var ddlStatuss = $("#ddlStatuss").val();
    var txtOwnerName = $("#txtOwnerName").val();
    var txtdistriList = $("#txtdistriList").val();
    var txtlat = $("#txtlat").val();
    var txtlong = $("#txtlong").val();
    var txtManHrsAvai = $("#txtManHrsAvai").val();
    var checkboxMaintainInventory = $("#checkboxMaintainInventory").is(':checked');
    var checkboxCycloneManagement = $("#checkboxCycloneManagement").is(':checked');
    var chkboxManHoursManagement = $("#chkboxManHoursManagement").is(':checked');
    var chkboxEnabledGPSSitePin = $("#chkboxEnabledGPSSitePin").is(':checked');
    var checkboxMaintainInventoryAtJob = $("#checkboxMaintainInventoryAtJob").is(':checked');
    var chkboxEnabledCaptureGps = $("#chkboxEnabledCaptureGps").is(':checked');
    var btngps = $("#btngps").is(':checked');
    var btnfloorlevel = $("#btnfloorlevel").is(':checked');
    var chkOffSystemInspection = $("#chkOffSystemInspection").is(':checked');
    var txtMailingList = $("#txtMailingList").val();
    var chkboxSkipMaterial = $("#chkboxSkipMaterial").is(':checked');
    var chkPipelineInv = $("#chkPipelineInv").is(':checked');
    if (CheckRequiredfields() == false) {
        $.ajax({
            type: "POST",
            url: "/EditClient/SiteSave",
            data: JSON.stringify({ id: id, UID: UID, txtsitelocation: txtsitelocation, txtaddress: txtaddress, txtcity: txtcity, ddlstates: ddlstates, txtzip: txtzip, txtukpost: txtukpost, ddlStatuss: ddlStatuss, txtOwnerName: txtOwnerName, txtdistriList: txtdistriList, txtlat: txtlat, txtlong: txtlong, txtManHrsAvai: txtManHrsAvai.replace(/,/g, ""), checkboxMaintainInventory: checkboxMaintainInventory, checkboxCycloneManagement: checkboxCycloneManagement, chkboxManHoursManagement: chkboxManHoursManagement, chkboxEnabledGPSSitePin: chkboxEnabledGPSSitePin, checkboxMaintainInventoryAtJob: checkboxMaintainInventoryAtJob, chkboxEnabledCaptureGps: chkboxEnabledCaptureGps, btngps: btngps, btnfloorlevel: btnfloorlevel, sitelocdid: sitelocdid, chkOffSystemInspection: chkOffSystemInspection, txtMailingList: txtMailingList, chkboxSkipMaterial: chkboxSkipMaterial, chkPipelineInv: chkPipelineInv, showpopup: showpopup }),

            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                var ClientID = response.ClientID;
                var UID = getParameterByName('UID');
                var UserRole = getParameterByName('UserRole');
                if (response.Result == "Success") {
                    CustomAlert(response);
                    //$('#SitePopup').hide();
                    $('#SitePopup').modal('hide');
                    GetSiteData();

                    if (check == 1) {
                        btnCAncel_Click();
                    }
                }

                else {
                    CustomAlert(response);
                    //$('#SitePopup').hide();
                    $('#SitePopup').modal('hide');
                    /*$('#LoadingPanel1').hide();*/

                }
            }
        });

    }
    else {
        var Response = { Result: "Failed", Message: "Please fill All Requried Field." };
        CustomAlert(Response);
        $('#SitePopup').modal('show');
        $('#LoadingPanel1').hide();
    }

}

function btnCancel(check) {
    SetdefaultValues();
    $('#SitePopup').hide();
    if (check == 1)
    {
        btnCAncel_Click();
    }
}

function SetdefaultValues() {

    $('#SitePopup :input[type="text"]').val('');
    $('#SitePopup :input[type="number"]').val('');
    $("#ddlstates").val('');
    $("#checkboxMaintainInventoryAtJob").prop('checked', false);
    $("#checkboxCycloneManagement").prop('checked', false);
    $("#chkboxEnabledGPSSitePin").prop('checked', false);
    $("#chkboxEnabledCaptureGps").prop('checked', false);
    $("#chkOffSystemInspection").prop('checked', false);
    $("#chkPipelineInv").prop('checked', false);
   
}


// Site Save End Here

function GetLocationData(SiteID) {
    $("#ajaxLoader").css("display", "block");
    // var id = $("#ContentPlaceHolder1_hdnid").val();
    var id = getParameterByName('id');
    var AreaID = $('#hdnLocationID').val();

    var str = "";
    str = '<table id="tblLocation_' + SiteID + '" class="clone"  cellspacing="0" border="0" style="padding-left:50px;width:100%">';
	str = '<table id="tblLocation_' + SiteID + '" class="clone"  cellspacing="0" border="0" style="padding:0px; margin:0px; width:100%">';
    str += '<thead><tr style="background-color: darkblue;color: white;">';
    str += '<th style="display:none">LocationID</th>';
    str += '<th style="width:5%;text-align:center;"></th>';
    str += '<th style="width:5%;text-align:center;"></th>';
    str += '<th style="width:5%;text-align:center;"></th>';
    str += '<th style="width:4%;text-align:left;"></th>';
    str += '<th style="width:62%;text-align:left;">Location</th>';
    str += '<th style="width:6%;text-align:left;"></th>';
    str += '<th style="width:13%;text-align:left;">Status</th>';

    str += '</tr></thead><tbody>';
    //str += '</tbody></table>';
    $.ajax({
        type: "POST",
        // url: "../WebApi/AllInventory.asmx/EditData",
        url: "/EditClient/GetLocationData",
        data: JSON.stringify({ id: id, sitelocaID: SiteID, AreaID: AreaID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            // var data = response.d;
            var data = response;


            if ($.fn.DataTable.isDataTable("#tblLocation_" + SiteID)) {
                $("#tblLocation_" + SiteID).DataTable().destroy();
            }


            if (data.length > 0) {
                for (var j = 0; j < data.length; j++) {
                    var LocationID = data[j].ID == null ? "" : data[j].ID;
                    var Area = data[j].Area == null ? "" : data[j].Area;
                    var ClientID = data[j].ClientID == null ? "" : data[j].ClientID;
                    var AreaStatus = data[j].AreaStatus == null ? "" : data[j].AreaStatus;

                    str += '<tr>';
                    str += '<td id="LocationID_' + LocationID + '" style="display:none">' + LocationID + '</td>';
                    str += '<td  class="tdbtnExpend" style="width:5%;text-align:center"><a id="LocationGeid_' + LocationID + '" class="expand fa fa-angle-right"  style="font-size:19px;"></a></td>';
                    str += '<td title="Click here to edit Location." style="cursor:pointer;width:5%;"><a id="tbnEditLocationID_' + LocationID + '" Class="btnLocationEdit"><i class="fa fa-pencil"></i>&nbsp;</a></td>';
                    str += '<td  title="Click here for Delete Location" style="cursor:pointer;width:5%;"><i class="fa fa-trash " onclick=ShowDeletePopup("' + LocationID + '","LocationMaster")></i></td>';
                    str += '<td style="width:4%;text-align:left;"></td>';
                    str += '<td id="Area_' + LocationID + '" style="width:62%;text-align:left;">' + Area + '</td>';
                    str += '<td style="width:6%;text-align:left;"></td>';
                    str += '<td id="AreaStatus_' + LocationID + '" style="width:13%;text-align:left; ">' + AreaStatus + '</td>';
                    str += '</tr>';
                    //row.child(format(st).show());
                }
                // str += st;

            }



           
           // var check = d.DT_RowId;
            // $("#Clone_" + d.DT_RowId).html(str);
            str += '</tbody></table>';
            $("#tblLocation_" + SiteID).html(str);
            $("#ajaxLoader").css("display", "none");

            tableblLocation = $("#tblLocation_" + SiteID).DataTable({

                searching: false,
                paging: true,
                info: false,
                autoWidth: true,
                length: false,
                sort: false,
                scrollX: true,
                scrollCollapse: true,
                scrollY: "60vh",
                //"pageLength": 150,
                //Added on (31-05-2024) Pagelength Set to 300 because it was not displaying all the records on front end..
                //"pageLength": 300,
                //(27-06-2024 by s.sourabh)we can set the pageLength option to display all rows by setting it to a very high number or by using the special value -1, which is recognized by DataTables as "display all records."
                "pageLength": -1,


                initComplete: function () {
                    $("#tblLocation_" + SiteID + "_length").children().html('<button id="btnAddNewLocation"  type="button" class="w3-btn w3-teal fa fa-plus btnAddNewLocation">&nbsp;Add New Location</button>');
                    //$("#btnAddNewLocation").click(function () {
                    $(".btnAddNewLocation").click(function () {
                        $('#hdnSiteLocationID').val(SiteID);
                        $('#hdnLocationID').val("");
                        $('#txtarea').val("");
                        $('#divLocationSavePopup').show();
                    });

                    $(".btnLocationEdit").click(function () {
                        event.stopImmediatePropagation();
                        var LocationID = this.id.split('_')[1];
                        var Area = $('#Area_' + LocationID).text();
                        var AreaStatus = $('#AreaStatus_' + LocationID).text();

                        $('#hdnSiteLocationID').val(SiteID);
                        $('#txtarea').val(Area);
                        $("#ddlAreaStatus").val(AreaStatus);
                        $('#hdnLocationID').val(LocationID);
                        $('#divLocationSavePopup').show();

                    });
                },


                "drawCallback": function () {
                    $('.fa-angle-right').click(function (eve) {
                        eve.stopImmediatePropagation();
                        var tr = $(this).closest('tr');
                        // var row = table.row(tr);
                        var row=tableblLocation.row(tr);   
                        var LocationID = this.id.split('_')[1];

                        var visible = $(this).parent().parent().hasClass("shown");
                        var anglechange = $(this).hasClass("fa-angle-right");
                        if (anglechange == true) {
                            var x = tr.children()[0];
                            $(this).removeClass("fa-angle-right");
                            $(this).addClass("fa-angle-down");
                        }
                        else {
                            $(this).addClass("fa-angle-right");
                            $(this).removeClass("fa-angle-down");
                        }
                        if (visible == true) {
                            $(this).parent().parent().next('tr').remove();
                            $(this).parent().parent().removeClass('shown');
                        }
                        else {
                            row.child(GetBuildingData(LocationID)).show();
                            row.child.show();
                            tr.addClass('shown');
                        }
                    });
                }
                
                
            });

            var LocationID = getParameterByName('LocationID');
            var AddBuilding1 = 'AddBuilding';
            var AddBuilding = $('#TxtAddBuilding').val();
            var AddAsset1 = 'AddAsset';
            var AddAsset = $('#TxtAddAsset').val();
            AddAsset = Decrypt(AddAsset);
            AddBuilding = Decrypt(AddBuilding);
            LocationID = Decrypt(LocationID);

            if (AddBuilding1 == AddBuilding) {
                $('#LocationGeid_' + LocationID).click();
                $('#divBuildingSavePopup').css('display', 'block');
                $('#TxtAddBuilding').val("");
            }

            if (AddAsset1 == AddAsset) {
                $('#LocationGeid_' + LocationID).click();
            }
           
          }
    });
    return str;
    //    // `d` is the original data object for the row
   
}

function btnSaveLocation(check) {

    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var SiteID=$('#hdnSiteLocationID').val();
    var txtarea = $('#txtarea').val();
    var ddlAreaStatus = $("#ddlAreaStatus").val();
    var LocationID = $('#hdnLocationID').val();

    if (SiteID == "" || SiteID == null || SiteID == 0) {
        var SiteID = getParameterByName('SiteID');
        var DecryptData = Decrypt([SiteID]);
        SiteID = DecryptData[0];
    }

    if (txtarea != "") {
      $.ajax({
            type: "POST",
            url: "/EditClient/LocationSave",
            data: JSON.stringify({ id: id, UID: UID, SiteID: SiteID, txtarea: txtarea, ddlStatuss: ddlAreaStatus, LocationID: LocationID}),

            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (response) {
                if (response.Result == "Success") {
                    CustomAlert(response);
                    GetLocationData(SiteID);
                    $('#divLocationSavePopup').hide();
                    if (check == 1) {
                        btnCAncel_Click();
                    }
                } else {
                    CustomAlert(response);
                    $('#LoadingPanel1').hide();
                }
            }
        });
    } else {
        var Response = { Result: "Failed", Message: "Please fill Location." };
        CustomAlert(Response);
        $('#LoadingPanel1').hide();
        $('#txtarea').addClass('requiredColumn');
    }
}

function btnLocationclose(check) {
    $('#divLocationSavePopup').hide();
    if (check == 1)
    {
        btnCAncel_Click();
    }
}

function GetBuildingData(LocationID) {
    $("#ajaxLoader").css("display", "block");
    // var id = $("#ContentPlaceHolder1_hdnid").val();
    var id = getParameterByName('id');
    var str = "";
	str = '<table id="tblBuilding_' + LocationID + '" class="clone"  cellspacing="0" border="0" style="padding:0px; margin:0px; width:100%">';
    str += '<thead><tr style="background-color: darkblue;color: white;">';
    str += '<th style="display:none">BuildingID</th>';
    str += '<th style="width:5%;text-align:center;"></th>';
    str += '<th style="width:5%;text-align:center"></th>';
    str += '<th style="width:2%;text-align:center"></th>';
    str += '<th style="width:71%;text-align:left;">Building / Module No.</th>';
    str += '<th style="width:4%;text-align:left"></th>';
    str += '<th style="width:13%;text-align:left;">Status</th>';
  
    str += '</tr></thead><tbody>';
    //str += '</tbody></table>';
    $.ajax({
        type: "POST",
        url: "/EditClient/GetBuildingData",
        data: JSON.stringify({ id: id, AreaLocaID: LocationID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            // var data = response.d;
            var data = response;

            if ($.fn.DataTable.isDataTable("#tblBuilding_" + LocationID)) {
                $("#tblBuilding_" + LocationID).DataTable().destroy();
            }

            if (data.length > 0) {
                for (var j = 0; j < data.length; j++) {
                    var BuildingID = data[j].ID == null ? "" : data[j].ID;
                    var BuildingNo = data[j].BuildingNo == null ? "" : data[j].BuildingNo;
                    var BuildingStatus = data[j].BuildingStatus == null ? "" : data[j].BuildingStatus;
                    var ClientId = data[j].ClientId == null ? "" : data[j].ClientId;
                    var AreaId = data[j].AreaId == null ? "" : data[j].AreaId;

                    str += '<tr>';
                    str += '<td id="BuildingID_' + BuildingID + '" style="display:none">' + BuildingID + '</td>';
                    str += '<td  class="tdbtnExpend" style="width:5%;text-align:center"><a id="BuildingIdexpend_' + BuildingID + '" class="expand fa fa-angle-right"  style="font-size:19px;"></a></td>';
                    str += '<td title="Click here to edit Building." style="cursor:pointer;width:5%;"><a id="BuildingID_'+BuildingID+'" Class="btnBuieldingEdit"><i class="fa fa-pencil"></i>&nbsp;</a></td>';
                    str += "<td  title='Click here for Delete Location' style='cursor:pointer;style='width:2%;'><i class='fa fa-trash ' onclick=ShowDeletePopup('" + BuildingID + "','BuildingMaster')></i></td>";
                    str += '<td id="BuildingNo_' + BuildingID + '" style="width:71%;text-align:left;">' + BuildingNo + '</td>';
                    str += '<td style="width:4%;text-align:left;"></td>';
                    str += '<td id="BuildingStatus_' + BuildingID + '" style="width:13%;text-align:left;">' + BuildingStatus + '</td>';
                    str += '</tr>';
                    //row.child(format(st).show());
                }
                // str += st;

            }


            
            // var check = d.DT_RowId;
            // $("#Clone_" + d.DT_RowId).html(str);
            str += '</tbody></table>';
            $("#tblBuilding_" + LocationID).html(str);
            $("#ajaxLoader").css("display", "none");

            tableBuilding = $("#tblBuilding_" + LocationID).DataTable({
                searching: false,
                paging: true,
                info: false,
                autoWidth: true,
                length: false,
                sort: false,
                scrollX: true,
                scrollCollapse: true,
                scrollY: "30vh",
                //Added on (12-01-2024) Pagelength Set to 300 because it was not displaying all the records on front end..
                //"pageLength": 150,
                //"pageLength": 300,
                //(27-06-2024 by s.sourabh)we can set the pageLength option to display all rows by setting it to a very high number or by using the special value -1, which is recognized by DataTables as "display all records."
                "pageLength": -1,

                initComplete: function () {
                    $("#tblBuilding_" + LocationID + "_length").children().html('<button id="btnAddNewBuildingNo"  type="button" class="w3-btn w3-teal fa fa-plus btnAddNewBuildingNo">&nbsp;Add New Building / Module No.</button>');
                    $(".btnAddNewBuildingNo").click(function () {
                   //$("#btnAddNewBuildingNo").click(function () {
                       $('#hdnLocationID').val(LocationID);
                       $('#hdnBuildingID').val("");
                       $('#txtBuilding').val("");
                       $('#divBuildingSavePopup').show();
                   }),
                    $(".btnBuieldingEdit").click(function () {
                        var BuildingID = this.id.split('_')[1];
                        var BuildingNo = $('#BuildingNo_' + BuildingID).text();
                        var BuildingStatus = $('#BuildingStatus_' + BuildingID).text();

                        $('#txtBuilding').val(BuildingNo);
                        $("#ddlBuildingStatus").val(BuildingStatus);
                        $('#hdnLocationID').val(LocationID);
                        $('#hdnBuildingID').val(BuildingID);

                        $('#divBuildingSavePopup').show();
                    });
                    
                },

                "drawCallback": function () {
                    $('.fa-angle-right').click(function (eve) {
                        eve.stopImmediatePropagation();
                        var tr = $(this).closest('tr');
                        //var row = table.row(tr);
                        var row = tableBuilding.row(tr);

                        var BuildingID = this.id.split('_')[1];

                        var visible = $(this).parent().parent().hasClass("shown");
                        var anglechange = $(this).hasClass("fa-angle-right");
                        if (anglechange == true) {
                            var x = tr.children()[0];
                            $(this).removeClass("fa-angle-right");
                            $(this).addClass("fa-angle-down");
                        }
                        else {
                            $(this).addClass("fa-angle-right");
                            $(this).removeClass("fa-angle-down");
                        }
                        if (visible == true) {
                            $(this).parent().parent().next('tr').remove();
                            $(this).parent().parent().removeClass('shown');
                        }
                        else {
                            row.child(GetAssestData(BuildingID)).show();
                            row.child.show();
                            tr.addClass('shown');
                        }
                    });
                }

            });

            var BuildingID = getParameterByName('BuildingID');
            var AddAsset1 = 'AddAsset';
            var AddAsset = $('#TxtAddAsset').val();
            AddAsset = Decrypt(AddAsset);
            BuildingID = Decrypt(BuildingID);

            if (AddAsset1 == AddAsset) {
                $('#BuildingIdexpend_' + BuildingID).click();
                $('#divAssetSavePopup').css('display', 'block');
                $('#TxtAddAsset').val("");
            }

        }

    });
    return str;
    //    // `d` is the original data object for the row
}

function btnSaveBuilding(check) {
        var id = getParameterByName('id');
        var UID = getParameterByName('UID');
        var txtBuilding = $('#txtBuilding').val();
        var ddlBuildingStatus = $("#ddlBuildingStatus").val();
        var AreaID = $('#hdnLocationID').val();
        var buildingid = $('#hdnBuildingID').val();
        if (txtBuilding != "") {
            $.ajax({
                type: "POST",
                url: "/EditClient/BuildingSave",
                data: JSON.stringify({ id: id, UID: UID, AreaID: AreaID, txtBuilding: txtBuilding, ddlStatuss1: ddlBuildingStatus, buildingid: buildingid }),

                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                success: function (response) {
                    if (response.Result == "Success") {
                        CustomAlert(response);
                        /*GetSiteData();*/
                        $('#divBuildingSavePopup').hide();
                        GetBuildingData(AreaID);

                        $(this).addClass("fa-angle-right");
                        $(this).removeClass("fa-angle-down");
                        if (check == 1) { 
                            btnCAncel_Click();
                        }
                    } else {
                        CustomAlert(response);
                        $('#LoadingPanel1').hide();
                    }
                }
            });
        } else {
            var Response = { Result: "Failed", Message: "Please fill Building No." };
            CustomAlert(Response);
            $('#LoadingPanel1').hide();
            $('#txtBuilding').addClass('requiredColumn');
        }
    }

function btnBuildingclose(check) {
    $('#divBuildingSavePopup').hide();
    if (check == 1) {
        btnCAncel_Click();
    }
}

function GetAssestData(BuildingID) {
    $("#ajaxLoader").css("display", "block");
    // var id = $("#ContentPlaceHolder1_hdnid").val();
    var id = getParameterByName('id');
    var str = "";
    str = '<table id="tblAssest_' + BuildingID + '" class="clone"  cellspacing="0" border="0" style="padding:0px; margin:0px; width:100%">';
    str += '<thead><tr style="background-color: darkblue;color: white;">';
    str += '<th style="display:none">AssestID</th>';
    str += '<th style="width:3%;text-align:center;"></th>';
    str += '<th style="width:0%;text-align:center;"></th>';
    // str += '<th style="width:0%;text-align:left;"></th>';
    str += '<th style="width:34%;text-align:left;"># Asset No</th>';
    // str += '<th style="width:2%;text-align:right;"></th>';
    str += '<th style="width:50%;text-align:left">Asset Description</th>';
    // str += '<th style="width:0%;text-align:right;"></th>';
    str += '<th style="width:13%;text-align:left">Status</th>';
    

    str += '</tr></thead><tbody>';
   // str += '</tbody></table>';
    $.ajax({
        type: "POST",
        url: "/EditClient/GetAssetData",
        data: JSON.stringify({ id: id, BuildingID: BuildingID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            // var data = response.d;
            var data = response;
            if ($.fn.DataTable.isDataTable("#tblAssest_" + BuildingID)) {
                $("#tblAssest_" + BuildingID).DataTable().destroy();
            }
            if (data.length > 0) {
             for (var j = 0; j < data.length; j++) {
                    var AssestID = data[j].ID == null ? "" : data[j].ID;
                    var AssetNo = data[j].AssetNo == null ? "" : data[j].AssetNo;
                    var AssetDesc = data[j].AssetDesc == null ? "" : data[j].AssetDesc;
                    var AssetStatus = data[j].AssetStatus == null ? "" : data[j].AssetStatus;
                    var ClientId = data[j].ClientId == null ? "" : data[j].ClientId;
                    var BuildingId = data[j].BuildingId == null ? "" : data[j].BuildingId;
                   
                    str += '<tr>';
                    str += '<td id="AssestID_' + AssestID + '" style="display:none">' + AssestID + '</td>';
                    str += '<td title="Click here to edit Assest." style="cursor:pointer;width:3%;"><a id="AssestID_' + AssestID + '" Class="btnAssestEdit"><i class="fa fa-pencil"></i></a></td>';
                    str += "<td  title='Click here for Delete Assest' style='cursor:pointer;style='width:0%;'><i class='fa fa-trash ' onclick=ShowDeletePopup('" + AssestID + "','AssestMaster')></i></td>";
                 //str += '<td style="width:0%;text-align:center;"></td>';
                    str += '<td id="AssetNo_' + AssestID + '" style="width:34%;text-align:left;">' + AssetNo + '</td>';
                 //str += '<td style="width:2%;text-align:right;"></td>';
                    str += '<td id="AssetDesc_' + AssestID + '" style="width:50%;text-align:left">' + AssetDesc + '</td>';
                 //  str += '<td style="width:0%;text-align:right;"></td>';
                    str += '<td id="AssetStatus_' + AssestID + '" style="width:13%;text-align:left">' + AssetStatus + '</td>';
                    str += '</tr>';
                    //row.child(format(st).show());
                }
                // str += st;

            }
            str += '</tbody></table>';
            $("#tblAssest_" + BuildingID).html(str);
            $("#ajaxLoader").css("display", "none");

            tableAsset = $("#tblAssest_" + BuildingID).DataTable({
                searching: false,
                paging: true,
                info: false,
                autoWidth: true,
                length: false,
                sort: false,
                scrollX: true,
                scrollCollapse: true,
                scrollY: "60vh",
                //"pageLength": 150,
                //Added on (31-05-2024) Pagelength Set to 300 because it was not displaying all the records on front end..
                //"pageLength": 300,
                //(27-06-2024 by s.sourabh)we can set the pageLength option to display all rows by setting it to a very high number or by using the special value -1, which is recognized by DataTables as "display all records."
                "pageLength": -1,

                initComplete: function () {
                    $("#tblAssest_" + BuildingID + "_length").children().html('<button id="btnAddNewAssest"  type="button" class="w3-btn w3-teal fa fa-plus btnAddNewAssest">&nbsp;Add New Asset</button>');
                    //$("#btnAddNewAssest").click(function () {
                    $(".btnAddNewAssest").click(function () {
                        $('#hdnBuildingID').val(BuildingID);
                        $('#hdnAssetID').val("");
                        $('#txtAssetNo').val("");
                        $('#txtAssetDesc').val("");
                        $('#divAssetSavePopup').show();

                    });

                    $(".btnAssestEdit").click(function () {
                        var AssestID = this.id.split('_')[1];
                        var AssetNo = $('#AssetNo_' + AssestID).text();
                        var AssetDesc = $('#AssetDesc_' + AssestID).text();
                        var AssetStatus = $('#AssetStatus_' + AssestID).text();

                         $('#txtAssetNo').val(AssetNo);
                         $('#txtAssetDesc').val(AssetDesc);
                         $("#ddlAssetStatus").val(AssetStatus);
                         $('#hdnBuildingID').val(BuildingID);
                         $('#hdnAssetID').val(AssestID);

                        $('#divAssetSavePopup').show();
                    });
                },

                "drawCallback": function () {
                    $('.fa-angle-right').click(function (eve) {
                        eve.stopImmediatePropagation();
                        var tr = $(this).closest('tr');
                       // var row = table.row(tr);
                        var row = tableAsset.row(tr);
                        var AssetID = this.id.split('_')[1];

                        var visible = $(this).parent().parent().hasClass("shown");
                        var anglechange = $(this).hasClass("fa-angle-right");
                        if (anglechange == true) {
                            var x = tr.children()[0];
                            $(this).removeClass("fa-angle-right");
                            $(this).addClass("fa-angle-down");
                        }
                        else {
                            $(this).addClass("fa-angle-right");
                            $(this).removeClass("fa-angle-down");
                        }
                        if (visible == true) {
                            $(this).parent().parent().next('tr').remove();
                            $(this).parent().parent().removeClass('shown');
                        }
                        else {
                            row.child(GetAssestData(AssetID)).show();
                            row.child.show();
                            tr.addClass('shown');
                        }
                    });
                }

            });
        }
    });
    return str;
    //    // `d` is the original data object for the row
}

function btnSaveAsset(check) {
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var txtAssetNo = $('#txtAssetNo').val();
    var txtAssetDesc= $('#txtAssetDesc').val();
    var ddlAssetStatus = $("#ddlAssetStatus").val();
    var BuildingID = $('#hdnBuildingID').val();
    var Assestid = $('#hdnAssetID').val();
        if (txtAssetNo != "") {
            $.ajax({
                type: "POST",
                url: "/EditClient/AssetSave",
                data: JSON.stringify({ id: id, UID: UID, BuildingID: BuildingID, txtAssetNo: txtAssetNo, txtAssetDesc: txtAssetDesc, ddlStatuss2: ddlAssetStatus, Assestid: Assestid }),

                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                success: function (response) {
                    if (response.Result == "Success") {
                        CustomAlert(response);
                        $('#divAssetSavePopup').hide();
                        /* GetSiteData();*/
                        GetAssestData(BuildingID);
                        if (check == 1) {
                            btnCAncel_Click();
                        }
                    } else {
                        CustomAlert(response);
                        $('#LoadingPanel1').hide();
                    }
                }
            });

        }
        else {
            var Response = { Result: "Failed", Message: "Please fill Asset." };
            CustomAlert(Response);
            $('#LoadingPanel1').hide();
            $('#txtAssetNo').addClass('requiredColumn');
        }
}

function btnAssetclose(check){
    $('#divAssetSavePopup').hide();
    if (check == 1) {
        btnCAncel_Click();
    }
}

// Common Delete Popup For Site,Location,Building,AssestNo Master Pages
function ShowDeletePopup(deleteID,DeleteFlag) {
    $('#hdndeleteID').val(deleteID);
    $('#hdnDeleteFlag').val(DeleteFlag);
    $('#divdelete').css("display", "block");
}

function btndeleteno_Click() {
    $('#divdelete').css("display", "none");
}

function btndeleteyes_Click() {
    var id = getParameterByName('id');
    var DeleteFlag= $('#hdnDeleteFlag').val();
    var deleteID = $('#hdndeleteID').val();
   $.ajax({
    type: "POST",
    url: "/EditClient/btndeleteyes_Click",
    data: JSON.stringify({ id:id,deleteID:deleteID,DeleteFlag:DeleteFlag}),
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    async: true,
    success: function (response) {
        if (response.Result == "Success") {
            CustomAlert(response);
            GetSiteData();
            $('#divdelete').css("display", "none");
        } else {
            CustomAlert(response);
            $('#LoadingPanel1').hide();
            $('#divdelete').css("display", "none");
        }
    }
});

}


// Site Code End Here

function btnFloorLevelImage_Click()
{
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
   
    window.location.href = '/FloorLevelGallery/FloorLevelGallery?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;

    //  Response.Redirect("FloorLevelGallery.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"]);
}

function btnCAncel_Click()
{
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
    var RID = getParameterByName('RID');

    window.location.href = '/Register/Register?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id + '&RID=' + RID;

     //  Response.Redirect("Register.aspx?UID=" + Request.QueryString["UID"] + "&UserRole=" + Request.QueryString["UserRole"] + "&id=" + Request.QueryString["id"] + "&RID=" + Request.QueryString["RID"], false);
}

function btndeleteImageYes_Click() {
    var id = getParameterByName('id');
    var imgname = $('#hdnimgname').val();
   
    $.ajax({
        type: "POST",
        url: "/EditClient/btndeleteImageYes_Click",
        data: JSON.stringify({ id: id, imgname: imgname}),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            if (response.Result == "Success") {
                $('#divImage').css("display", "none");
                CustomAlert(response);
                ShowClientDetails();
                GetSiteData();
            } else {
                CustomAlert(response);
                $('#LoadingPanel1').hide();
                $('#divImage').css("display", "none");
            }
        }
    });
}
function btndeleteImageNo_Click() {
    $('#divImage').css('display','none');
}

function DisbSkipMat(SiteID) {
    //var SiteID = $('#hdnSiteLocationID').val();
    $.ajax({
        type: "POST",
        url: "/EditClient/DisbSkipMat",
        data: JSON.stringify({ SiteID: SiteID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            var GetData = response;
            var NoInventory = GetData.NoInventory == null ? "" : GetData.NoInventory;
            if (NoInventory == true) {
                $('#chkboxSkipMaterial').prop('disabled', true);
                NoInvCheck();
            }
            else if (NoInventory == "") {
                $('#chkboxSkipMaterial').prop('disabled', false);
            }
            else {
                $('#chkboxSkipMaterial').prop('disabled', false);
            }
        }
    });
}

function NoInvCheck() {
    $('.clsSkipMat').hover(function () {
        $('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
        $('#art1').css("display", "block");
        $('#alertlabel').html('<strong>Alert ! </strong>There is at least one job which has  "No Inventory Required" checked.');
        hideAlert();
    });
}


function PipeInventory(SiteID) {
    var id = getParameterByName('id');
    $.ajax({
        type: "POST",
        url: "/EditClient/PipeInventory",
        data: JSON.stringify({ SiteID: SiteID, id: id }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            var Result = response.result;
            var Message = response.Message;
            if (Result == "Failed") {
                $('#chkPipelineInv').prop('disabled', true);
                HoverAlertCheck(Message);
            }
            else {
                $('#chkPipelineInv').prop('disabled', false);
                HoverAlertUnCheck();
            }
        }
    });
}
function HoverAlertCheck(Message) {
    $('.divPipelineInv').hover(function () {
        $('#art1').removeClass('w3-section w3-card-12 j-alertBox-Success').addClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
        $('#art1').css("display", "block");
        $('#alertlabel').html('<strong>Alert ! ' + Message + ' </strong>');

        hideAlert();
    });
}
function HoverAlertUnCheck() {
    $('.divPipelineInv').hover(function () {
        $('#art1').removeClass('w3-section fadeOut w3-card-12 j-alertBox-Failed');
        $('#art1').css("display", "none");
    });
}

function btnChangeUsernno_Click() {
    $('#ChangeUserconfirm').css("display", "none");
}


function btnChangeUsernyes_Click() {
    debugger;
    $('#ChangeUserconfirm').css("display", "none");
    btnSiteSave(0, 1);
}

function btnCheckChangeUser(check) {
    var txtOwnerName = $("#txtOwnerName").val();
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var sitelocdid = $('#hdnSiteLocationID').val();
    $.ajax({
        type: "POST",
        url: "/EditClient/btnCheckChangeUser",
        data: JSON.stringify({ id: id, txtOwnerName: txtOwnerName, sitelocdid: sitelocdid }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            if (response.Result == "Success") {
                $('#SitePopup').modal('hide');
                debugger;
                $('#ChangeUserconfirm').css("display", "block");
                $('#ChangeUserconfirm').find('#Label2').text(response.Message);

            } else {
                btnSiteSave(check, 0);
                $('#ChangeUserconfirm').css("display", "none");

            }
        }
    });

}



function DeleteInactiveSiteDocuments() {
    var clientId = getParameterByName('id');
    var siteLocId = $('#hdnSiteLocationID').val();

    if (siteLocId == "") {
        alert("Please select a site.");
        return;
    }

    var confirmation = prompt("To delete all Site Documents,type the password in CAPITAL LETTERS and click OK:");
    if (!confirmation || confirmation.trim() !== "PAUL") {
        alert("Incorrect password.");
        return;
    }

    $("#LoadingPanel1").css({ "position": "fixed", "z-index": "999999", "top": "0", "left": "0", "width": "100%", "height": "100%" }).show();
    $.ajax({
        url: "/EditClient/DeleteInactiveSiteDocuments",
        type: "POST",
        data: {
            clientId: clientId,
            siteLocId: siteLocId
        },
        success: function (response) {
            $("#LoadingPanel1").hide();
            alert(response.Message);
        },
        error: function () {
            $("#LoadingPanel1").hide();
            alert("Something went wrong. Please try again.");
        }
    });
}
function DeleteSiteImages() {
    var clientId = getParameterByName('id');
    var siteLocId = $('#hdnSiteLocationID').val();
    if (siteLocId == "") {
        alert("Please select a site.");
        return;
    }
    var confirmation = prompt("To delete all Site Images,type the password in CAPITAL LETTERS and click OK:");
    if (!confirmation || confirmation.trim() !== "PAUL") {
        alert("Incorrect password.");
        return;
    }
    $("#LoadingPanel1").css({ "position": "fixed", "z-index": "999999", "top": "0", "left": "0", "width": "100%", "height": "100%" }).show();

    $.ajax({
        url: "/EditClient/DeleteSiteImages",
        type: "POST",
        data: {
            clientId: clientId,
            siteLocId: siteLocId
        },
        success: function (response) {
            $("#LoadingPanel1").hide();
            alert(response.Message);
        },
        error: function () {
            $("#LoadingPanel1").hide();
            alert("Something went wrong. Please try again.");
        }
    });
}
function CheckMissingBlobFiles() {
    var clientId = getParameterByName('id');
    var siteLocId = $('#hdnSiteLocationID').val();
    $("#btnCheckMissingBlob").prop("disabled", true);
    $("#LoadingPanel1").css({ "position": "fixed", "z-index": "999999", "top": "0", "left": "0", "width": "100%", "height": "100%" }).show();
    $.ajax({
        url: "/EditClient/CheckMissingSiteDocumentsAndImages",
        type: "POST",
        data: {
            clientId: clientId,
            siteLocId: siteLocId
        },
        success: function (response) {
            $("#LoadingPanel1").hide();
            if (response.Result === "Success") {
                alert("Check completed successfully. " + response.Message);
            }
            else if (response.Result === "Warning") {
                alert("Warning: " + response.Message);
            }
            else {
                alert("Error: " + response.Message);
            }
        },
        error: function () {
            $("#LoadingPanel1").hide();
            alert("Something went wrong while checking Blob files.");

        },
        complete: function () {
            $("#btnCheckMissingBlob").prop("disabled", false);
            $("#LoadingPanel1").hide();
        }
    });
}
function DeleteInactiveClient() {
    var clientId = getParameterByName('id');
    var userId = getParameterByName('UID');
    var decryptclientId = Decrypt(clientId);
    var decryptuserId = Decrypt(userId);
    document.getElementById("btnDeleteInactiveClient").disabled = true;
    if (!clientId || !userId) {
        alert("Client ID or User ID is missing.");
        document.getElementById("btnDeleteInactiveClient").disabled = false;
        return;
    }
    console.log("Clientid:" + clientId);
    console.log("UserId:" + userId);
    $.ajax({
        url: "/EditClient/GetClientDetails",
        type: "POST",
        data: {
            clientId: clientId,
            userId: userId
        },
        success: function (details) {

            var confirmation = prompt(
                "Client: " + decryptclientId + " - " + details.ClientName +
                "\nUser: " + decryptuserId + " - " + details.UserRole +
                "\n\nTo delete the client, type the password in CAPITAL LETTERS and click OK:"
            );

            if (!confirmation || confirmation.trim() !== "PAUL") {
                alert("Incorrect password.");
                document.getElementById("btnDeleteInactiveClient").disabled = false;
                return;
            }
            $("#LoadingPanel1").css({
                "position": "fixed",
                "z-index": "999999",
                "top": "0",
                "left": "0",
                "width": "100%",
                "height": "100%"
            }).show();

            $.ajax({
                url: "/EditClient/DeleteClientData",
                type: "POST",
                data: {
                    clientId: clientId,
                    userId: userId
                },
                success: function (response) {

                    if (response.Result === "BlobPending") {

                        $("#LoadingPanel1").hide();

                        $("#blobMsg").text(response.Count + " file(s) still exist. Please delete these first:");

                        var listHtml = "<ul style='padding-left:20px;'>";

                        if (response.Files && response.Files.length > 0) {
                            response.Files.forEach(function (fileName) {
                                listHtml += "<li>" + fileName + "</li>";
                            });
                        }

                        listHtml += "</ul>";

                        $("#fileListContainer").html(listHtml);
                        $("#blobFilesModal").show();
                        document.getElementById("btnDeleteInactiveClient").disabled = false;
                    }
                    else if (response.Result === "Started") {

                        pollDeleteClientStatus(response.JobId);
                    }
                    else if (response.Result === "Failed") {
                        $("#LoadingPanel1").hide();
                        alert(response.Message);
                        document.getElementById("btnDeleteInactiveClient").disabled = false;
                    }
                    else {
                        $("#LoadingPanel1").hide();
                        alert("Unexpected response received.");
                        document.getElementById("btnDeleteInactiveClient").disabled = false;
                    }
                },
                error: function (xhr, status, error) {

                    $("#LoadingPanel1").hide();

                    console.log(xhr);
                    console.log(status);
                    console.log(error);
                    console.log(xhr.responseText);

                    alert("Error : " + xhr.status + "\n" + xhr.responseText);

                    document.getElementById("btnDeleteInactiveClient").disabled = false;
                }
            });

        },
        error: function () {
            alert("Unable to fetch client details.");
            document.getElementById("btnDeleteInactiveClient").disabled = false;
        }
    });
}
function pollDeleteClientStatus(jobId) {
    var pollInterval = setInterval(function () {
        $.ajax({
            url: "/EditClient/GetDeleteJobStatus",
            type: "GET",
            data: { jobId: jobId },
            success: function (res) {
                if (res.Result === "Running") {
                    return;
                }

                clearInterval(pollInterval);
                $("#LoadingPanel1").hide();

                if (res.Result === "Success") {
                    alert(res.Message);
                    window.location.href = "/Login/Login";
                }
                else if (res.Result === "Failed") {
                    alert("Delete failed: " + res.Message);
                    document.getElementById("btnDeleteInactiveClient").disabled = false;
                }
                else {
                    alert("Unexpected status: " + res.Result);
                    document.getElementById("btnDeleteInactiveClient").disabled = false;
                }
            },
            error: function () {
                clearInterval(pollInterval);
                $("#LoadingPanel1").hide();
                alert("Error checking delete status. Please refresh and check manually.");
                document.getElementById("btnDeleteInactiveClient").disabled = false;
            }
        });
    }, 3000);
}

