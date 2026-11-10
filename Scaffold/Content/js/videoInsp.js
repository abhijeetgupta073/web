 $(document).ready(function () {
            $("#getVideo").on("click", function () {
                $(".hdnVideobefore").css("display", "none");
                // $("#btnVideoModal").click();
                $("#btnVideoModal").css("display", "block");
                $("#cancelvideo").css("display", "block");

                $("#cancelvideo").on("click", function () {
                    $("#btnVideoModal").css("display", "none");
                    $("#tempVideo").val("");

                    var mediaStream = window.stream;

                    //Media Stream Stop Code starts here

                    mediaStream.getAudioTracks()[0].stop();
                    mediaStream.getVideoTracks()[0].stop();
                });

                var mediaSource = new MediaSource();
                mediaSource.addEventListener('sourceopen', handleSourceOpen, false);
                var mediaRecorder;
                var recordedBlobs;
                var sourceBuffer;

                //var imga = document.getElementById('myvideo');

                var gumVideo = document.querySelector('video#gum');
                var recordedVideo = document.querySelector('video#recorded');

                var recordButton = document.querySelector('button#record1');
                var playButton = document.querySelector('button#play');
                var downloadButton = document.querySelector('button#download');
                recordButton.onclick = toggleRecording;
                playButton.onclick = play;
                downloadButton.onclick = download;
                
                // Use old-style gUM to avoid requirement to enable the
                // Enable experimental Web Platform features flag in Chrome 49

                navigator.getUserMedia = navigator.getUserMedia ||
                  navigator.webkitGetUserMedia || navigator.mozGetUserMedia;

                var constraints = {
                    audio: true,
                    video: true
                };

                navigator.getUserMedia(constraints, successCallback, errorCallback);

                function successCallback(stream) {
                    console.log('getUserMedia() got stream: ', stream);
                    window.stream = stream;
                    if (window.URL) {
                        gumVideo.src = window.URL.createObjectURL(stream);
                    } else {
                        gumVideo.src = stream;
                    }
                }

                function errorCallback(error) {
                    console.log('navigator.getUserMedia error: ', error);
                }

                function handleSourceOpen(event) {
                    console.log('MediaSource opened');
                    sourceBuffer = mediaSource.addSourceBuffer('video/webm; codecs="vp8"');
                    console.log('Source buffer: ', sourceBuffer);
                }

                function handleDataAvailable(event) {
                    if (event.data && event.data.size > 0) {
                        recordedBlobs.push(event.data);
                    }
                }

                function handleStop(event) {
                    console.log('Recorder stopped: ', event);
                }

                function toggleRecording() {
                    if (recordButton.textContent === 'Start Recording') {
                        $("#cancelvideo").css("display", "none");
                        startRecording();
                    } else {
                        stopRecording();
                        recordButton.textContent = 'Start Recording';
                        playButton.disabled = false;
                        downloadButton.disabled = false;
                        $("#cancelvideo").css("display", "none");
                        //$("#videoModal .modal-header .close").click();
                        $("#btnVideoModal").css("display", "none");
                        $(".hdnVideobefore").css("display", "block");
                        $("#VideoBlock").css("display", "none");
                        play();
                    }
                }

                // The nested try blocks will be simplified when Chrome 47 moves to Stable
                function startRecording() {
                    var options = { mimeType: 'video/webm', bitsPerSecond: 100000 };
                    recordedBlobs = [];
                    try {
                        mediaRecorder = new MediaRecorder(window.stream, options);
                    } catch (e0) {
                        console.log('Unable to create MediaRecorder with options Object: ', e0);
                        try {
                            options = { mimeType: 'video/webm,codecs=vp9', bitsPerSecond: 100000 };
                            mediaRecorder = new MediaRecorder(window.stream, options);
                        } catch (e1) {
                            console.log('Unable to create MediaRecorder with options Object: ', e1);
                            try {
                                options = 'video/vp8'; // Chrome 47
                                mediaRecorder = new MediaRecorder(window.stream, options);
                            } catch (e2) {
                                alert('MediaRecorder is not supported by this browser.\n\n' +
                                    'Try Firefox 29 or later, or Chrome 47 or later, with Enable experimental Web Platform features enabled from chrome://flags.');
                                console.error('Exception while creating MediaRecorder:', e2);
                                return;
                            }
                        }
                    }
                    console.log('Created MediaRecorder', mediaRecorder, 'with options', options);
                    recordButton.textContent = 'Stop Recording';
                    playButton.disabled = true;
                    downloadButton.disabled = true;
                    mediaRecorder.onstop = handleStop;
                    mediaRecorder.ondataavailable = handleDataAvailable;
                    mediaRecorder.start(10); // collect 10ms of data
                    console.log('MediaRecorder started', mediaRecorder);
                }

                function stopRecording() {

                    mediaRecorder.stop();
                    console.log('Recorded Blobs: ', recordedBlobs);

                    var mediaStream = window.stream;

                    //Media Stream Stop Code starts here

                    mediaStream.getAudioTracks()[0].stop();
                    mediaStream.getVideoTracks()[0].stop();

                    //Media Stream Stop Code Ends Here
                    


                    recordedVideo.controls = true;
                    var base64data;
                    var superBuffer = new Blob(recordedBlobs, { type: 'video/webm' });

                    var as = window.URL.createObjectURL(superBuffer);
                    //alert(as);
                    var reader = new window.FileReader();
                    reader.readAsDataURL(superBuffer);

                    var reader = new FileReader();
                    reader.readAsDataURL(superBuffer);
                    reader.onloadend = function () {
                        base64data = reader.result;

                        //base64data = base64data.replace('data:video/webm;base64,', '');
                        console.log(base64data);
                       // alert(base64data);
                        // imga.setAttribute('src', "data:video/webm;base64," + baseStr64);
                        //$("#tempimg").val('src'+','+ '"data:image/jpg;base64,"' + baseStr64);
                        
                        $("#tempVideo").val(base64data);
                    }

                }

                function play() {
                    var superBuffer = new Blob(recordedBlobs, { type: 'video/webm' });
                    recordedVideo.src = window.URL.createObjectURL(superBuffer);
                    //gumVideo.src = window.URL.createObjectURL(superBuffer);
                }

                function download() {
                    var blob = new Blob(recordedBlobs, { type: 'video/webm' });
                    var url = window.URL.createObjectURL(blob);
                    var a = document.createElement('a');
                    a.style.display = 'block';
                    a.href = url;

                    a.download = 'test.webm';
                    document.body.appendChild(a);
                    a.click();

                    setTimeout(function () {
                        document.body.removeChild(a);
                        window.URL.revokeObjectURL(url);
                    }, 100);
                }
            });
        });