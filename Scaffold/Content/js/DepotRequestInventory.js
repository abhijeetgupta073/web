$(function () {

    var Href = window.location.href.split('?');
    if (Href.length == 1) {
        $('#UlNavTabSeciton li').css('display', 'none');
       // $('#liDepotRequest').css('display', 'block');
        
    }
    $('#UlNavTabSeciton a').css('cursor', 'pointer');
    $('#UlNavTabSeciton a').click(function (e) {
        var UID = getParameterByName('UID');
        var UserRole = getParameterByName('UserRole');
        var id = getParameterByName('id');
        $('#LoadingPanel1').css('display', 'block');
        var ClickAcnhorID = e.currentTarget.id;
        if (ClickAcnhorID == 'AnchorYardInventory') {
            window.location.href = '/InventoryManagement/InventoryManagement?' + Href[1];

        }
        if (ClickAcnhorID == 'AnchorReturnInventory') {
            //  window.location.href = '/ReturnInventory.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
            window.location.href = '/ReturnInventory/ReturnInventory?' + Href[1];
        }
        if (ClickAcnhorID == 'AnchorJobInventory') {
            window.location.href = '/RequestInventory/RequestInventory?' + Href[1];
        }

        if (ClickAcnhorID == 'AnchorDepotRequestInventory') {
            // window.location.href = '/RequestInventory.aspx?' + Href[1];
            $('#LoadingPanel1').css('display', 'none');
        }
        if (ClickAcnhorID == 'AnchorSiteInventory') {
            // window.location.href = '/SiteInventory.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
            window.location.href = '/SiteInventory/SiteInventory?' + Href[1];
        }
        if (ClickAcnhorID == 'AnchorDamagedInventory') {
            // window.location.href = '/DamagedInventory.aspx?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
            window.location.href = '/DamagedInventory/DamagedInventory?' + Href[1];
        }
    });
});

function btnnewrequest_Click1() {
    $('#LoadingPanel1').show();
    var Href = window.location.href.split('?');
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');

    window.location.href = '/MainDepotRequestDetails/MainDepotRequestDetails?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        $('#LoadingPanel1').hide();
    
}
function btnrequest_Click() {
    $('#LoadingPanel1').show();
    var Href = window.location.href.split('?');
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');

     window.location.href = '/MainDepotRequestDetails/MainDepotRequestDetails?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        $('#LoadingPanel1').hide();
    
}

function Delivery_Click() {
    $('#LoadingPanel1').show();
    var Href = window.location.href.split('?');
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');

    window.location.href = '/RequestDelivery/RequestDelivery?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
        $('#LoadingPanel1').hide();
    
}