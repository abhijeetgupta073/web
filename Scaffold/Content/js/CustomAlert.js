function CustomAlert(response) {
    if (response.Result == "Success") {
        $('#CustomAlert').removeClass('j-alertBox-Failed1').removeClass('j-alertBox-Warning1').css('display', 'none');
        $('#CustomAlert').addClass('j-alertBox-Success1').css('display', 'block');
        $('#alertMsg').html('<strong>Success! </strong>' + response.Message);
        setTimeout(function () {
            $('#CustomAlert').fadeOut('slow', function () {
                $('#CustomAlert').removeClass('j-alertBox-Success').css('display', 'none');
                $('#alertMsg').html('');

            });
        }, 5000);
    }
    //else if (response.Result == "Warning") {
    //    $('#CustomAlert').removeClass('j-alertBox-Failed1').removeClass('j-alertBox-Success1').css('display', 'none');
    //    $('#CustomAlert').addClass('j-alertBox-Warning1').css('display', 'block');
    //    $('#alertMsg').html('<strong>Warning! </strong>' + response.Message);
    //    setTimeout(function () {
    //        $('#CustomAlert').fadeOut('slow', function () {
    //            $('#CustomAlert').removeClass('j-alertBox-Warning1').css('display', 'none');
    //            $('#alertMsg').html('');

    //        });
    //    }, 5000);
        //}

            else if (response.Result == "Warning") {
                $('#CustomAlert').removeClass('j-alertBox-Failed1').removeClass('j-alertBox-Success1').css('display', 'none');
                $('#CustomAlert').addClass('j-alertBox-Failed1').css('display', 'block');

                $('#alertMsg').html('<strong>Warning ! </strong>' + response.Message);
                setTimeout(function () {
                    $('#CustomAlert').fadeOut('slow', function () {
                        $('#CustomAlert').removeClass('j-alertBox-Warning1').css('display', 'none');
                        $('#alertMsg').html('');

                    });
                }, 5000);
            }

    else if (response.Result == "Failed") {
        $('#CustomAlert').removeClass('j-alertBox-Warning1').removeClass('j-alertBox-Success1').css('display', 'none');
        $('#CustomAlert').addClass('j-alertBox-Failed1').css('display', 'block');
        $('#alertMsg').html('<strong>Alert! </strong>' + response.Message);
        setTimeout(function () {
            $('#CustomAlert').fadeOut('slow', function () {
                $('#CustomAlert').removeClass('j-alertBox-Failed').css('display', 'none');
                $('#alertMsg').html('');

            });
        }, 5000);
    }

    else if (response.Result == "Info") {
        $('#CustomAlert').removeClass('j-alertBox-Warning1').removeClass('j-alertBox-Success1').removeClass('j-alertBox-Failed1').css('display', 'none');
        $('#CustomAlert').addClass('j-alertBox-Info1').css('display', 'block');
        $('#alertMsg').html('<strong>Info! </strong>' + response.Message);
        setTimeout(function () {
            $('#CustomAlert').fadeOut('slow', function () {
                $('#CustomAlert').removeClass('j-alertBox-Failed').css('display', 'none');
                $('#alertMsg').html('');
              

            });
        }, 5000);
    }
}
