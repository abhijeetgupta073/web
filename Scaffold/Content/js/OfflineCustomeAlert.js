function OfflineCustomAlert(response) {
    // Get the CustomAlert and alertMsg elements
    var customAlert = document.getElementById('CustomAlert');
    var alertMsg = document.getElementById('alertMsg');

    // Check if both elements exist before proceeding
    if (customAlert && alertMsg) {
        // Reset classes and hide the alert box initially
        customAlert.classList.remove('j-alertBox-Failed1', 'j-alertBox-Warning1', 'j-alertBox-Success1');
        customAlert.style.display = 'none';

        // Determine the response type (Success, Warning, Failed)
        //if (response.Result === "Success") {
        //    customAlert.classList.add('j-alertBox-Success1');
        //    alertMsg.innerHTML = '<strong>Success! </strong>' + response.Message;
        //    alertMsg.style.textAlign = 'center'; // Center the text
        //    customAlert.style.display = 'block';

        //} else if (response.Result === "Warning") {
        //    customAlert.classList.add('j-alertBox-Warning1');
        //    alertMsg.innerHTML = '<strong>Warning! </strong>' + response.Message;
        //    alertMsg.style.textAlign = 'center'; // Center the text
        //    customAlert.style.display = 'block';

        //} else if (response.Result === "Failed") {
        //    customAlert.classList.add('j-alertBox-Failed1');
        //    alertMsg.innerHTML = '<strong>Alert! </strong>' + response.Message;
        //    alertMsg.style.textAlign = 'center'; // Center the text
        //    customAlert.style.display = 'block';
        //}
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

        // Set timeout to fade out the alert after 5 seconds
        setTimeout(function () {
            customAlert.style.transition = 'opacity 0.5s'; // Optional smooth fade-out
            customAlert.style.opacity = '0'; // Fade out

            setTimeout(function () {
                customAlert.style.display = 'none';
                customAlert.style.opacity = '1'; // Reset opacity for next use
                alertMsg.innerHTML = ''; // Clear the message after fade out
            }, 500); // Match the transition duration
        }, 5000); // Alert stays for 5 seconds before fade out
    } else {
        console.error("CustomAlert element or alertMsg element not found.");
    }
}
