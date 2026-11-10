$(function () {

    var Href = window.location.href.split('?');
    if (Href.length == 1) {
        $('#UlNavTabSeciton li').css('display', 'none');
        $('#liCompany').css('display', 'block');
    }
    $('#UlNavTabSeciton a').css('cursor', 'pointer');
    $('#UlNavTabSeciton a').click(function (e) {
        $('#LoadingPanel1').css('display', 'block');
        var ClickAcnhorID = e.currentTarget.id;
        if (ClickAcnhorID == 'AnchorYardInventory') {
            // window.location.href = '/InventoryManagement.aspx?' + Href[1];
            window.location.href = '/InventoryManagement/InventoryManagement?' + Href[1];
        }
        
        
        if (ClickAcnhorID == 'AnchorDepotRequestInventory') {
            // window.location.href = '/RequestInventory.aspx?' + Href[1];
              $('#LoadingPanel1').css('display', 'none');
        }
    });

});

function Back_Click()
{
  var Href = window.location.href.split('?');
    var id = getParameterByName('id');
    var UID = getParameterByName('UID');
    var UserRole = getParameterByName('UserRole');
 
    window.location.href = '/DepotRequestInventory/DepotRequestInventory?UID=' + UID + '&UserRole=' + UserRole + '&id=' + id;
  
}
