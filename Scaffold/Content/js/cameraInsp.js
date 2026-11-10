$(document).ready(function () {
    $("#getCamera").on("click", function () {
        /*$(".snapshot").attr('id', 'snap');*/

        $("#cancelsnap").on("click", function () {
            $("#btnOpenModel4").css("display", "none");
            cameraStream.stop();
        });
        $("#btnOpenModel4").css("display", "block");

        $(".hdnCamerabefore").css("display", "none");
        // Grab elements, create settings, etc.
        var canvas = document.getElementById('canvas');
        var context = canvas.getContext('2d');
        var video = document.getElementById('video');

        var cameraStream = null;

        var mediaConfig = { video: true };
        var errBack = function (e) {
            console.log('An error has occurred!', e)
        };

        // Put video listeners into place
        if (navigator.mediaDevices && navigator.mediaDevices.getUserMedia) {
            navigator.mediaDevices.getUserMedia(mediaConfig).then(function (stream) {
                // video.src = window.URL.createObjectURL(stream);  // Apparently this functionality (of creating a URL from a MediaStream) is now deprecated and has been removed from current versions of Chrome and Firefox as of mid/late 2018. See 
                self.video.srcObject = stream;   // Use This
                video.play();
                cameraStream = stream.getTracks()[0];
            });
        }

        /* Legacy code below! */
        else if (navigator.getUserMedia) { // Standard
            navigator.getUserMedia(mediaConfig, function (stream) {
                video.src = stream;
                video.play();

                cameraStream = stream.getTracks()[0];

            }, errBack);
        } else if (navigator.webkitGetUserMedia) { // WebKit-prefixed
            navigator.webkitGetUserMedia(mediaConfig, function (stream) {
                video.src = window.webkitURL.createObjectURL(stream);
                video.play();

                cameraStream = stream.getTracks()[0];

            }, errBack);
        } else if (navigator.mozGetUserMedia) { // Mozilla-prefixed
            navigator.mozGetUserMedia(mediaConfig, function (stream) {
                video.src = window.URL.createObjectURL(stream);
                video.play();

                cameraStream = stream.getTracks()[0];

            }, errBack);
        }

        // Trigger photo take
        document.getElementById('snap').addEventListener('click', function () {
            //$("#myModal4 .modal-header .close").click();
            $("#btnOpenModel4").css("display", "none");
            $(".hdnCamerabefore").css("display", "block");
            $("#Imageblock").css("display", "none");
            $("#btnchngeimg").css("display", "block");
            $("#btnchngg").css("display", "block");
            context.drawImage(video, 0, 0, 300, 300);


            var image = document.getElementById("canvas").toDataURL("image/png");
            $("#tempimg").val(image);

            cameraStream.stop();

            //navigator.getUserMedia({ audio: false, video: true },
            //function (stream) {
            //    // can also use getAudioTracks() or getVideoTracks()
            //    var track = stream.getTracks()[0];  // if only one media track
            //    // ...
            //    track.stop();
            //},
            //function (error) {
            //    console.log('getUserMedia() error', error);
            //});

        });
    });

    $("#getCameraNew").on("click", function () {
        $("#cancelsnapNew").on("click", function () {
            $("#btnOpenModel4New").css("display", "none");
            cameraStream.stop();
        });

        $("#btnOpenModel4New").css("display", "block");
        $(".hdnCamerabeforeNew").css("display", "none");

        // Grab elements, create settings, etc.
        var canvas = document.getElementById('canvasNew');
        var context = canvas.getContext('2d');
        var video = document.getElementById('videoNew');

        var cameraStream = null;

        var mediaConfig = { video: true };

        var errBack = function (e) {
            console.log('An error has occurred!', e)
        };

        // Put video listeners into place
        if (navigator.mediaDevices && navigator.mediaDevices.getUserMedia) {
            navigator.mediaDevices.getUserMedia(mediaConfig).then(function (stream) {
                // video.src = window.URL.createObjectURL(stream);  // Apparently this functionality (of creating a URL from a MediaStream) is now deprecated and has been removed from current versions of Chrome and Firefox as of mid/late 2018. See 
                //self.video.srcObject = stream;   
                video.srcObject = stream;   // Use This

                video.play();
                cameraStream = stream.getTracks()[0];
            });
        }

        /* Legacy code below! */
        else if (navigator.getUserMedia) { // Standard
            navigator.getUserMedia(mediaConfig, function (stream) {
                video.src = stream;
                video.play();

                cameraStream = stream.getTracks()[0];

            }, errBack);
        } else if (navigator.webkitGetUserMedia) { // WebKit-prefixed
            navigator.webkitGetUserMedia(mediaConfig, function (stream) {
                video.src = window.webkitURL.createObjectURL(stream);
                video.play();

                cameraStream = stream.getTracks()[0];

            }, errBack);
        } else if (navigator.mozGetUserMedia) { // Mozilla-prefixed
            navigator.mozGetUserMedia(mediaConfig, function (stream) {
                video.src = window.URL.createObjectURL(stream);
                video.play();

                cameraStream = stream.getTracks()[0];

            }, errBack);
        }

        // Trigger photo take

        document.getElementById('snapnew').addEventListener('click', function () {

            //$("#myModal4 .modal-header .close").click();
            $("#btnOpenModel4New").css("display", "none");
            $(".hdnCamerabeforeNew").css("display", "block");
            $("#ImageblockNew").css("display", "none");
            $("#btnchngeimg1").css("display", "block");
            $("#btnchngg1").css("display", "block");
            context.drawImage(video, 0, 0, 300, 300);


            var image = document.getElementById("canvasNew").toDataURL("image/png");
            $("#tempimg2").val(image);

            cameraStream.stop();


        });
    });


});