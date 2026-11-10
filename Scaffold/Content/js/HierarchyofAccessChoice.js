
    var AccessValue = "";
    $(document).ajaxStart(function () {
        $('#LoadingPanel1').css('display', 'block');
    });
   
$(document).ready(function () {
    $('#LiHierarchyofAccessChoice').removeClass('active').addClass('active');
    ddldropdownbind();
    ShowJobInfoDetails();
    getAccesschoice();
    var screensize = $(window).width();

    if (screensize < 1150 && screensize > 650) {
        ($BODY.hasClass('nav-md'))
        $('#CreateNewDropDown').css("margin-top", "2px");
        $BODY.removeClass('nav-md').addClass('nav-sm');
        $LEFT_COL.removeClass('scroll-view');
        //$('#BlackSection').css('display', 'block');
        $('#imgLogo').parent().css('display', 'none');
        $('#imgLogo1').css('display', 'block').css("height", "38px");
    }


    else {
        if (screensize < (650)) {
            $BODY.removeClass('nav-sm')
            $('#imgLogo').parent().css('display', 'none');
            $('#imgLogo1').css('display', 'block').css("height", "52px");

            $('.xsfieldset').css('margin-left', '-22px').css("width", "100%");
        }
        else {
            $('#CreateNewDropDown').css("margin-top", "6px");
            $BODY.removeClass('nav-sm').addClass('nav-md');
            //$('#BlackSection').css('display', 'none');
            $('#imgLogo').parent().css('display', 'block');
            $('#imgLogo1').css('display', 'none');
            $('#leftbarsection').css('margin-top', '40px');
        }

    }
    checkanyone();
});

 
function getAccesschoice()
{
    var id=getParameterByName('id');
    var RID=getParameterByName('RID');

    $.ajax({
        url: '/HierarchyofAccessChoice/getAccesschoice',
        async: false,
        data: { id: id,RID:RID },
        success: function (response) {
            var compaire=response;
            if(compaire!=""){
                //  if (imgEngineer.Text == compaire)
                if ($('#imgEngineer').val() == compaire)
                {
                    //chkYesEngineer.Checked = true;
                    //chkYesEngineer.Enabled = false;
                    //chkNoEngineer.Enabled = false;
                    //txtComment1.Enabled = false;

                    $('#chkYesEngineer').prop('checked',true);
                    $('#chkYesEngineer').attr('disabled',true);
                    $('#chkNoEngineer').attr('disabled',true);
                    $('#txtComment1').attr('disabled',true);
                }
                    // else if (imgEngineer.Text != compaire)
                else if ($('#imgEngineer').val() != compaire)
                {
                    //chkNoEngineer.Checked = true;
                    //ImageHide1.Style.Add("display", "block");
                    //chkYesEngineer.Enabled = false;
                    //chkNoEngineer.Enabled = false;
                    //txtComment1.Enabled = false;

                    $('#chkNoEngineer').prop('checked', true);
                    $('#chkYesEngineer').attr('disabled', true);
                    $('#chkNoEngineer').attr('disabled', true);
                    $('#txtComment1').attr('disabled', true);
                    $('#ImageHide1').css('display', 'block');

                    // if (imgMechinical.Text == compaire)
                    if ($('#imgMechinical').val() == compaire)
                    {
                        //chkYesMechinical.Checked = true;
                        //chkYesMechinical.Enabled = false;
                        //chkNoMechinical.Enabled = false;
                        //txtComment2.Enabled = false;

                        $('#chkYesMechinical').prop('checked', true);
                        $('#chkYesMechinical').attr('disabled', true);
                        $('#chkNoMechinical').attr('disabled', true);
                        $('#txtComment2').attr('disabled', true);
                    }
                        //else if (imgMechinical.Text != compaire)
                    else if ($('#imgMechinical').val() != compaire)
                    {
                        //chkNoMechinical.Checked = true;
                        //ImageHide2.Style.Add("display", "block");
                        //chkYesMechinical.Enabled = false;
                        //chkNoMechinical.Enabled = false;
                        //txtComment2.Enabled = false;

                        $('#chkNoMechinical').prop('checked', true);
                        $('#chkYesMechinical').attr('disabled', true);
                        $('#chkNoMechinical').attr('disabled', true);
                        $('#txtComment2').attr('disabled', true);
                        $('#ImageHide2').css('display', 'block');

                        // if (imgRope.Text == compaire)
                        if ($('#imgRope').val() == compaire)
                        {
                            //chkYesRope.Checked = true;
                            //chkNoRope.Enabled = false;
                            //chkYesRope.Enabled = false;
                            //txtComment3.Enabled = false;

                            $('#chkYesRope').prop('checked', true);
                            $('#chkNoRope').attr('disabled', true);
                            $('#chkYesRope').attr('disabled', true);
                            $('#txtComment3').attr('disabled', true);
                        }
                            //else if (imgRope.Text != compaire)
                        else if ($('#imgRope').val() != compaire)
                        {
                            //chkNoRope.Checked = true;
                            //ImageHide3.Style.Add("display", "block");
                            //chkYesRope.Enabled = false;
                            //chkNoRope.Enabled = false;
                            //txtComment3.Enabled = false;

                            $('#chkNoRope').prop('checked', true);
                            $('#chkYesRope').attr('disabled', true);
                            $('#chkNoRope').attr('disabled', true);
                            $('#txtComment3').attr('disabled', true);
                            $('#ImageHide3').css('display', 'block');

                            // if (imgModularScaffold.Text == compaire)
                            if ($('#imgModularScaffold').val() == compaire)
                            {
                                chkYesModularScaffold.Checked = true;
                                chkNoModularScaffold.Enabled = false;
                                chkYesModularScaffold.Enabled = false;
                                txtComment4.Enabled = false;

                                $('#chkYesModularScaffold').prop('checked', true);
                                $('#chkNoModularScaffold').attr('disabled', true);
                                $('#chkYesModularScaffold').attr('disabled', true);
                                $('#txtComment4').attr('disabled', true);
                               
                            }
                                //  else if (imgModularScaffold.Text != compaire)
                            else if ($('#imgModularScaffold').val() != compaire)
                            {
                                //chkNoModularScaffold.Checked = true;
                                //ImageHide4.Style.Add("display", "block");
                                //chkYesModularScaffold.Enabled = false;
                                //chkNoModularScaffold.Enabled = false;
                                //txtComment4.Enabled = false;

                                $('#chkNoModularScaffold').prop('checked', true);
                                $('#chkYesModularScaffold').attr('disabled', true);
                                $('#chkNoModularScaffold').attr('disabled', true);
                                $('#txtComment4').attr('disabled', true);
                                $('#ImageHide4').css('display', 'block');

                                // if (imgTubeCoupler.Text == compaire)
                                if ($('#imgTubeCoupler').val() == compaire)
                                {
                                    //chkYesTubeCoupler.Checked = true;
                                    //chkYesTubeCoupler.Enabled = false;
                                    //chkNoTubeCoupler.Enabled = false;
                                    //txtComment5.Enabled = false;

                                    $('#chkYesTubeCoupler').prop('checked', true);
                                    $('#chkYesTubeCoupler').attr('disabled', true);
                                    $('#chkNoTubeCoupler').attr('disabled', true);
                                    $('#txtComment5').attr('disabled', true);
                                }
                                    // else if (imgTubeCoupler.Text != compaire)
                                else if ($('#imgTubeCoupler').val() != compaire)
                                {
                                    //chkNoTubeCoupler.Checked = true;
                                    //chkYesTubeCoupler.Enabled = false;
                                    //txtComment5.Enabled = false;

                                    $('#chkNoTubeCoupler').prop('checked', true);
                                    $('#chkYesTubeCoupler').attr('disabled', true);
                                    $('#txtComment5').attr('disabled', true);
                                }
                            }
                        }
                    }
                }

            }
            checkanyone();
        }
      
    });

}


function ddldropdownbind() {
    var id = getParameterByName('id');
    $.ajax({
        url: '/HierarchyofAccessChoice/ddldropdownbind',
        async: false,
        data: { id: id },
        success: function (response) {

            var ddlsupervisor = "<option value=''></option>";
            $.each(response, function (idx, obj) {
                if (obj != null) {
                    ddlsupervisor += '<option value="' + obj.UserId + '">' + obj.Supervisor + '</option>';
                }
            });
            $('#ddlsupervisor').empty().append(ddlsupervisor);
        }
    });
}

function checkanyone() {
    if (IsSiteStatus == "InActive") {
        CustomAlert({
            Result: "Failed", Message: "This site is InActive. This Action is not allowed."
        });
        return false;
    }
    $(".check1").click(function () {
        $(".check1").not(this).prop('checked', false);
    });
    $(".check2").click(function () {
        $(".check2").not(this).prop('checked', false);
    });
    $(".check3").click(function () {
        $(".check3").not(this).prop('checked', false);
    });
    $(".check4").click(function () {
        $(".check4").not(this).prop('checked', false);
    });
    $(".check5").click(function () {
        $(".check5").not(this).prop('checked', false);
    });
}

function NoCheck1() {
    $('#LoadingPanel1').css('display', 'block');
    if ($("#chkNoEngineer").prop('checked') == true && $("#txtComment1").val() != "") {
        $("#ImageHide1").css("display", "block");
    }
    if ($("#chkNoEngineer").prop('checked') == true) {
        $("#chkYesEngineer").attr("disabled", true);
        $("#chkNoEngineer").prop('disabled', true);
        $('#LoadingPanel1').css('display', 'none');
    }
    if ($("#chkNoEngineer").prop('checked') == false) {
        $("#chkYesEngineer").attr("disabled", false);
    }

    chkvalue_CheckedChanged();
}


function NoCheck2() {
    if ($("#chkNoMechinical").prop('checked') == true && $("#txtComment2").val() != "") {
        $("#ImageHide2").css("display", "block");
    }
    if ($("#chkNoMechinical").prop('checked') == true) {
        $("#chkYesMechinical").attr("disabled", true);
        $("#chkNoMechinical").attr("disabled", true);
    }
    if ($("#chkNoMechinical").prop('checked') == false) {
        $("#chkYesMechinical").attr("disabled", false);
    }

    chkvalue_CheckedChanged();
}

function NoCheck3() {
    if ($("#chkNoRope").prop('checked') == true && $("#txtComment3").val() != "") {
        $("#ImageHide3").css("display", "block");
    }
    if ($("#chkNoRope").prop('checked') == true) {
        $("#chkYesRope").attr("disabled", true);
        $("#chkNoRope").attr("disabled", true);
    }
    if ($("#chkNoRope").prop('checked') == false) {
        $("#chkYesRope").attr("disabled", false);
    }

    chkvalue_CheckedChanged();
}


function NoCheck4() {
    if ($("#chkNoModularScaffold").prop('checked') == true && $("#txtComment4").val() != "") {
        $("#ImageHide4").css("display", "block");
    }
    if ($("#chkNoModularScaffold").prop('checked') == true) {
        $("#chkYesModularScaffold").attr("disabled", true);
        $("#chkNoModularScaffold").attr("disabled", true);
    }
    if ($("#chkNoModularScaffold").prop('checked') == false) {
        $("#chkYesModularScaffold").attr("disabled", false);
    }

    chkvalue_CheckedChanged();
}


$(document).ready(function () {
    $("#btnReset").click(function () {
        if (IsSiteStatus == "InActive") {
            CustomAlert({
                Result: "Failed", Message: "This site is InActive. This Action is not allowed."
            });
            return false;
        }
        if (($("#chkNoEngineer").prop('checked') == true) || ($("#chkYesEngineer").prop('checked') == true) || $("#txtComment1").val() != "") {
            $("#DivConfirm").css("display", "block");
        }
        else {
            $('#art1').removeClass("w3-section w3-card-12 j-alertBox-Success").addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
            $('#alertlabel').text("Alert ! No Hierarchy has been selected so there is nothing to the set.");
        }
    });

    $("#btnNoCon").click(function () {
        if (IsSiteStatus == "InActive") {
            CustomAlert({
                Result: "Failed", Message: "This site is InActive. This Action is not allowed."
            });
            return false;
        }
        $("#DivConfirm").css("display", "none");
    });

});

function chkvalue_CheckedChanged() {
    if (IsSiteStatus == "InActive") {
        CustomAlert({
            Result: "Failed", Message: "This site is InActive. This Action is not allowed."
        });
        return false;
    }
    AccessValue = null;
    if ($('#chkYesEngineer').prop('checked') == true) {
        AccessValue = $('#imgEngineer').val();
        $("#chkNoEngineer").attr("disabled", true);
        $("#chkYesEngineer").attr("disabled", true);
    }
    else if ($('#chkNoEngineer').prop('checked') == true) {
        AccessValue = $('#imgEngineer').val();
        $("#chkNoEngineer").attr("disabled", true);
        $("#chkYesEngineer").attr("disabled", true);
    }

    if ($('#chkYesMechinical').prop('checked') == true) {
        AccessValue = $('#imgMechinical').val();
        $("#chkNoMechinical").attr("disabled", true);
        $("#chkYesMechinical").attr("disabled", true);
    }
    else if ($('#chkNoMechinical').prop('checked') == true) {
        AccessValue = $('#imgMechinical').val();
        $("#chkNoMechinical").attr("disabled", true);
        $("#chkYesMechinical").attr("disabled", true);
    }

    if ($('#chkYesRope').prop('checked') == true) {
        AccessValue = $('#imgRope').val();
        $("#chkNoRope").attr("disabled", true);
        $("#chkYesRope").attr("disabled", true);
    }
    else if ($('#chkNoRope').prop('checked') == true) {
        AccessValue = $('#imgRope').val();
        $("#chkNoRope").attr("disabled", true);
        $("#chkYesRope").attr("disabled", true);
    }

    if ($('#chkYesModularScaffold').prop('checked') == true) {
        AccessValue = $('#imgModularScaffold').val();
        $("#chkNoModularScaffold").attr("disabled", true);
        $("#chkYesModularScaffold").attr("disabled", true);
    }
    else if ($('#chkNoModularScaffold').prop('checked') == true) {
        AccessValue = $('#imgModularScaffold').val();
        $("#chkNoModularScaffold").attr("disabled", true);
        $("#chkYesModularScaffold").attr("disabled", true);
    }

    if ($('#chkYesTubeCoupler').prop('checked') == true) {
        AccessValue = $('#imgTubeCoupler').val();
        $("#chkNoTubeCoupler").attr("disabled", true);
        $("#chkYesTubeCoupler").attr("disabled", true);
    }
    else if ($('#chkNoTubeCoupler').prop('checked') == true) {
        $("#chkNoTubeCoupler").attr("checked", false);
        //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
        //art1.Style.Add("display", "block");
        //alertlabel.Text = "<strong>Alert ! </strong>This is last Access level. Please Select 'YES'.";
        $('#art1').removeClass("w3-section w3-card-12 j-alertBox-Success").addClass("w3-section w3-card-12 j-alertBox-Failed").css('display', 'block');
        $('#alertlabel').text("Alert ! This is last Access level. Please Select 'YES'.");
    }
   
}

function btnyesCon_Click() {
    if (IsSiteStatus == "InActive") {
        CustomAlert({
            Result: "Failed", Message: "This site is InActive. This Action is not allowed."
        });
        return false;
    }
    var RID = getParameterByName('RID');
    $.ajax({
        url: '/HierarchyofAccessChoice/btnyesCon_Click',
        data: { RID: RID },
        async: false,
        success: function (response) {
            if (response == "Delete") {
                //art1.Attributes["class"] = "w3-section fadeOut w3-card-12 j-alertBox-Success";
                //art1.Style.Add("display", "block");
                //alertlabel.Text = "<strong>Success ! </strong> Data cleared Successfully.";
                var Response = { Result: "Success", Message: "Data cleared Successfully." };
                CustomAlert(Response);
                var Href = window.location.href.split('?');
                window.location.href = '/HierarchyofAccessChoice/HierarchyofAccessChoice?' + Href[1];

            }
        }
    });

    DivConfirm.Style.Add("display", "none");
}

function btnsave_Click() {
    if (IsSiteStatus == "InActive") {
        CustomAlert({
            Result: "Failed", Message: "This site is InActive. This Action is not allowed."
        });
        return false;
    }
    var RID = getParameterByName('RID');
    var ddlSupervisor = $('#ddlsupervisor').val();
    var txt1 = $('#txtComment1').val();
    var txt2 = $('#txtComment2').val();
    var txt3 = $('#txtComment3').val();
    var txt4 = $('#txtComment4').val();
    var txt5 = $('#txtComment5').val();

    if (ddlSupervisor != "") {
        if (AccessValue != null) {
            var CommentField = "";
            if (AccessValue == "Engineer Out") {
                CommentField = txt1;
            }
            if (AccessValue == "Mechanical Means") {
                CommentField = txt2;
            }
            if (AccessValue == "Rope Access") {
                CommentField = txt3;
            }
            if (AccessValue == "Modular Scaffold Build") {
                CommentField = txt4;
            }
            if (AccessValue == "Tube & Coupler Build") {
                CommentField = txt5;
            }
            if (CommentField != "") {

                $.ajax({
                    url: '/HierarchyofAccessChoice/btnsave_Click',
                    async: false,
                    data: { AccessValue, txt1, txt2, txt3, txt4, txt5, ddlSupervisor, RID },
                    success: function (response) {
                        if (response == "Update") {
                            //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Success";
                            //art1.Style.Add("display", "block");
                            //alertlabel.Text = "<strong>Success ! </strong>Data Saved Successfully.";
                            var Response = { Result: "Success", Message: "Data Saved Successfully." };
                            CustomAlert(Response);
                            var Href = window.location.href.split('?');
                            window.location.href = '/HierarchyofAccessChoice/HierarchyofAccessChoice?' + Href[1];

                        }
                    }

                });
            }
            else {
                //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
                //art1.Style.Add("display", "block");
                //alertlabel.Text = "<strong>Alert ! </strong>Please Fill Comment.";
                var Response = { Result: "Failed", Message: "Please Fill Comment." };
                CustomAlert(Response);
            }
        }
        else {
            //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
            //art1.Style.Add("display", "block");
            //alertlabel.Text = "<strong>Alert ! </strong>Please Select 'YES' or 'NO'.";
            var Response = { Result: "Failed", Message: "Please Select 'YES' or 'NO'." };
            CustomAlert(Response);
        }
    }
    else {
        //art1.Attributes["class"] = "w3-section w3-card-12 j-alertBox-Failed";
        //art1.Style.Add("display", "block");
        //alertlabel.Text = "<strong>Alert ! </strong>Please Fill the Signed Off By.";
        var Response = { Result: "Failed", Message: "Please Fill the Signed Off By." };
        CustomAlert(Response);
    }

    //getAccesschoice();
}

function txtComment1_TextChanged() {
  
    if ($('#chkNoEngineer').prop('checked') == true && $('#txtComment1').val() != "") {
        $("#ImageHide1").css("display", "block");
        AccessValue = null;
    }
    else if ($('#chkYesEngineer').prop('checked') == true) {
        AccessValue = $('#imgEngineer').val();
    }
}

function txtComment2_TextChanged() {
    if ($('#chkNoMechinical').prop('checked') == true && $('#txtComment2').val() != "") {
        //ImageHide2.Style.Add("display", "block");
        $("#ImageHide2").css("display", "block");
        AccessValue = null;
    }
    else if ($('#chkYesMechinical').prop('checked') == true) {
        AccessValue = $('#imgMechinical').val();
    }
}

function txtComment3_TextChanged() {
    if ($('#chkNoRope').prop('checked') == true && $('#txtComment3').val() != "") {
        //ImageHide3.Style.Add("display", "block");
        $("#ImageHide3").css("display", "block");
        AccessValue = null;
    }
    else if ($('#chkYesRope').prop('checked') == true) {
        AccessValue = $('#imgRope').val();
    }
}

function txtComment4_TextChanged() {
    if ($('#chkNoModularScaffold').prop('checked') == true && $('#txtComment4').val() != "") {
        //ImageHide4.Style.Add("display", "block");
        $("#ImageHide4").css("display", "block");
        AccessValue = null;
    }
    else if (chkYesModularScaffold.Checked == true) {
        AccessValue = $('#imgModularScaffold').val();
    }
}

function txtComment5_TextChanged() {
    if ($('#chkYesTubeCoupler').prop('checked') == true) {
        ViewState["value"] = $('#imgTubeCoupler').val();
    }
}
