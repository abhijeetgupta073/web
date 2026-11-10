$(document).ready(function () {
    $("#getNavi").on("click", function () {
                setTimeout(function () {
                    getLocationImage();
                }, 5000);
            });
        });
            
function getLocationImage() {
            html2canvas(document.getElementById("map_canvas"), {
                useCORS: true,
                onrendered: function (canvas) {
                    var img = canvas.toDataURL("image/jpeg");
                    $("#map_image").val(img);
                }
            });
        }

         //function GetcurrentLocation(currentlat, currentlng)
          function GetcurrentLocation(currentlat, currentlng)
           {
             var currentdate = new Date();
             var datetime = currentdate.getDate() + "/" + (currentdate.getMonth() + 1) + "/" + currentdate.getFullYear() + " " + currentdate.getHours() + ":" + currentdate.getMinutes() + ":" + currentdate.getSeconds();

             $("#Label1").text(datetime);

             $(".hdnmapbefore").css("display", "block");
             $(".hdnmapafter").css("display", "none");

             //var newLat = 28.4972308;
             //var newLng = 77.02032;

             var newLat = currentlat;
             var newLng = currentlng;

             if (navigator.geolocation) {
                 navigator.geolocation.getCurrentPosition(success);

             } else {
                 alert("Geo Location is not supported on your current browser!");
             }

             function success(position) {

                 if (newLat != null && newLng != null) {

                     var lat = newLat;
                     var long = currentlng;

                 } else {
                      var lat = position.coords.latitude;
                      var long = position.coords.longitude;

                      if (lat == null) {
                          alert("GPS not activated!");
                      }

                 }
                 //var lat = 28.4972308;
                 //var long = 77.02032;

                
                 //alert(long);
                 //var city = position.coords.locality;


                 var myLatlng = new google.maps.LatLng(lat, long);
                 var myOptions = {
                     center: myLatlng,
                     zoom: 12,
                     mapTypeId: google.maps.MapTypeId.ROADMAP
                 };
                 var map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);
                 var marker = new google.maps.Marker({
                     position: myLatlng,
                     title: "lat: " + lat + " long: " + long
                 });

                 //alert(+lat);
                 document.getElementById("txtLat").innerHTML = +lat;
                 document.getElementById("TxtLong").innerHTML = +long;

                 document.getElementById("hdnlatitude").value = +lat;
                 document.getElementById("hdnlongitude").value = +long;
                 

                 //$("#txtLat").text(+lat);
                 //$("#TxtLong").text(+long);

				 //$("#hdnlatitude").val(+lat);
                 //$("#hdnlongitude").val(+long);

                 //document.getElementById("txtLat").value = +lat ;
                 //document.getElementById("TxtLong").value = +long;

                 var latlng = new google.maps.LatLng(lat, long);
                 var geocoder = geocoder = new google.maps.Geocoder();
                 geocoder.geocode({ 'latLng': latlng }, function (results, status) {
                     if (status == google.maps.GeocoderStatus.OK) {
                         //alert("Location: " + results[1].formatted_address);
                         //document.getElementById("TxtAddress").value = results[1].formatted_address;
                         document.getElementById("TxtAddress").innerHTML = results[1].formatted_address;
                         document.getElementById("hdnAddress").value = results[1].formatted_address;
                         //$("#TxtAddress").text(results[1].formatted_address);

                         //$("#hdnAddress").val(results[1].formatted_address);
                     }
                 });

                 marker.setMap(map);


                 var infowindow = new google.maps.InfoWindow({ content: "<b>User Address</b><br/> Latitude:" + lat + "<br /> Longitude:" + long + "" });
                 infowindow.open(map, marker);
             }

         }