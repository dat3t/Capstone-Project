var listMarkers = [];
var map;

function initialize() {

    //Declare a new map
     map = new google.maps.Map(document.getElementById('map_canvas'), {
        center: {lat: -34.397, lng: 150.644},
        zoom: 13,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    });

    //Declare a bound
    var bounds = new google.maps.LatLngBounds();

   

    // Create the search box and link it to the UI element.
    var input = document.getElementById("pac-input");
    var searchBox = new google.maps.places.SearchBox(input);
    map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);
  
    // Bias the SearchBox results towards current map's viewport.
    map.addListener('bounds_changed', function () {
        searchBox.setBounds(map.getBounds());
    });

    var markers2 = [];

    //var p1 = { lat: 36.23081510000001, lng: 137.9643552 };

    //var p2 = { lat: 36.238666, lng: 137.96902209999996 };

    //var marker5= new google.maps.Marker({
    //    position: p1
    //});
    //var marker6 = new google.maps.Marker({
    //    position: p2
    //});
   

    //markers2.push(marker5);
    //markers2.push(marker6);
    for (var i = 0; i < 100; i++) {
       // var dataPhoto = data.photos[i];
        var latLng = new google.maps.LatLng(Math.floor(Math.random() * 50), Math.floor(Math.random() * 100));
        var marker = new google.maps.Marker({
            position: latLng
        });
        markers2.push(marker);
    }
    var markerCluster = new MarkerClusterer(map, markers2);

    // Listen for the event fired when the user selects a prediction and retrieve
    // more details for that place.
    var marker2 = [];
    searchBox.addListener('places_changed', function () {
        var places = searchBox.getPlaces();
        if (places.length == 0) {
            return;
        }

        // Clear out the old markers.
        marker2.forEach(function (marker) {
            marker.setMap(null);
        });
        marker2 = [];

        // For each place, get the icon, name and location.
        var bounds = new google.maps.LatLngBounds();
        places.forEach(function (place) {
            var icon = {
                url: place.icon,
                size: new google.maps.Size(71, 71),
                origin: new google.maps.Point(0, 0),
                anchor: new google.maps.Point(17, 34),
                scaledSize: new google.maps.Size(25, 25)
            };

            // Create a marker for each place.
            marker2.push(new google.maps.Marker({
                map: map,
                icon: icon,
                title: place.name,
                position: place.geometry.location
            }));

            if (place.geometry.viewport) {
                // Only geocodes have viewport.
                bounds.union(place.geometry.viewport);
            } else {
                bounds.extend(place.geometry.location);
            }
        });
        map.fitBounds(bounds);
    });

    //SAMPLE MARKERS WITH INFOWINDOWS

    //Declare an icon of sample marker
    var image = 'https://developers.google.com/maps/documentation/javascript/examples/full/images/beachflag.png';

    // Multiple Markers
    var markers = [
    ['Matsumoto Station, Matsumoto, Nagano Prefecture, Japan', 36.23081510000001, 137.9643552],
    ['4-1 Marunouchi, Matsumoto, Nagano Prefecture 390-0873, Japan', 36.238666, 137.96902209999996]
    ];

    // InfoWindow content
    var content ='<div style="overflow:hidden;">'+
        '<div id="iw-container">' +
                      '<div class="iw-title">Đạt đẹp zai\'s profile</div>' +
                      '<div class="iw-content">' +
                        '<div id="navInfo">' +
                            '<img src="../Images/dat.jpg" alt="Trần Trọng Tiến Đạt" height="115" width="115">' +
                            '</div>' +
                            '<div id="sectionInfo">' +
                            '<table style="width:200px">' +
                            '<tr>' +
                            '<td><b>Địa điểm:</b></td>' +
                            '<td>Oosaka</td> ' +
                            '</tr>' +
                            '<tr>' +
                            '<td><b>Facebook:</b></td>' +
                            '<td><a href="https://www.facebook.com/Dat3T">Dat3T</a></td>' +
                            '</tr>' +
                            '<tr>' +
                            '<td><b>Đang offer:</b></td>' +
                            '<td>Cho thuê nhà trọ</td>' +
                            '</tr>' +
                            '<tr>' +
                            '<td><b>Thông tin khác:</b></td>' +
                            '<td>abcxyz</td>' +
                            '</tr>' +
                            '</table>' +
                            '</div>'+
                      '</div>' +'</div>' +
                      '<div class="iw-bottom-gradient"></div>' +
            '</div>';

    // A new Info Window is created and set content
    var infowindow = new google.maps.InfoWindow({
        content: content,

        // Assign a maximum value for the width of the infowindow allows
        // greater control over the various content elements
        maxWidth: 350
    });

    
    // Loop through our array of markers & place each one on the map


    for (i = 0; i < markers.length; i++) {
        var position = new google.maps.LatLng(markers[i][1], markers[i][2]);
        bounds.extend(position);
        marker = new google.maps.Marker({
            position: position,
            map: map,
            title: markers[i][0],
            icon:image
        });

        // Allow each marker to have an info window
        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
               // infowindow.setContent(infoWindowContent[i][0]);
                infowindow.open(map, marker);
            }
        })(marker, i));

        // Automatically center the map fitting all markers on the screen
        map.fitBounds(bounds);
    }


    // This event expects a click on a marker
    // When this event is fired the Info Window is opened.
    google.maps.event.addListener(marker, 'click', function () {
        infowindow.open(map, marker);
    });

    // Event that closes the Info Window with a click on the map
    google.maps.event.addListener(map, 'click', function () {
        infowindow.close();
    });

    // *
    // START INFOWINDOW CUSTOMIZE.
    // The google.maps.event.addListener() event expects
    // the creation of the infowindow HTML structure 'domready'
    // and before the opening of the infowindow, defined styles are applied.
    // *
    google.maps.event.addListener(infowindow, 'domready', function () {

        // Reference to the DIV that wraps the bottom of infowindow
        var iwOuter = $('.gm-style-iw');

        /* Since this div is in a position prior to .gm-div style-iw.
         * We use jQuery and create a iwBackground variable,
         * and took advantage of the existing reference .gm-style-iw for the previous div with .prev().
        */
        var iwBackground = iwOuter.prev();

        // Removes background shadow DIV
        iwBackground.children(':nth-child(2)').css({ 'display': 'none' });

        // Removes white background DIV
        iwBackground.children(':nth-child(4)').css({ 'display': 'none' });

        // Moves the infowindow 115px to the right.
        iwOuter.parent().parent().css({ left: '115px' });

        // Moves the shadow of the arrow 76px to the left margin.
        iwBackground.children(':nth-child(1)').attr('style', function (i, s) { return s + 'left: 76px !important;' });

        // Moves the arrow 76px to the left margin.
        iwBackground.children(':nth-child(3)').attr('style', function (i, s) { return s + 'left: 76px !important;' });

        // Changes the desired tail shadow color.
        iwBackground.children(':nth-child(3)').find('div').children().css({ 'box-shadow': 'rgba(72, 181, 233, 0.6) 0px 1px 6px', 'z-index': '1' });

        // Reference to the div that groups the close button elements.
        var iwCloseBtn = iwOuter.next();

        // Apply the desired effect to the close button
        iwCloseBtn.css({ opacity: '1', right: '38px', top: '3px', border: '7px solid #48b5e9', 'border-radius': '13px', 'box-shadow': '0 0 5px #3990B9' });

        // If the content of infowindow not exceed the set maximum height, then the gradient is removed.
        if ($('.iw-content').height() < 140) {
            $('.iw-bottom-gradient').css({ display: 'none' });
        }

        // The API automatically applies 0.7 opacity to the button after the mouseout event. This function reverses this event to the desired value.
        iwCloseBtn.mouseout(function () {
            $(this).css({ opacity: '1' });
        });
        iwCloseBtn.css({ 'display': 'none' });
});

}

function loadScript() {
    var script = document.createElement("script");
    script.src = "http://maps.googleapis.com/maps/api/js?libraries=places,geometry&callback=initialize";
    document.body.appendChild(script);
}

window.onload = loadScript;

function showCurrentLocation() {
    
    var p1 = { lat: 36.23081510000001, lng: 137.9643552 };
    
    var p2 = { lat: 36.238666, lng: 137.96902209999996};
    //var R = 6378137; // Earth’s mean radius in meter
    //var dLat = rad(p2.lat - p1.lat);
    //var dLong = rad(p2.lng - p1.lng);
    //var a = Math.sin(dLat / 2) * Math.sin(dLat / 2) +
    //  Math.cos(rad(p1.lat)) * Math.cos(rad(p2.lat)) *
    //  Math.sin(dLong / 2) * Math.sin(dLong / 2);
    //var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
    //var d = R * c;
    //return d; // returns the distance in meter
   document.getElementById('cal').innerHTML = getDistance(p1,p2);
   
    
    // getDistance({ lat: -36.23081510000001, lng: 137.9643552 }, { lat: -36.238666, lng: 137.96902209999996 });
    ////Declare a myLocation icon 
    //var myLocationIcon = {
    //    url: "images/myLocation.png", // url
    //    scaledSize: new google.maps.Size(30, 30), // scaled size
    //    origin: new google.maps.Point(0,0), // origin
    //    anchor: new google.maps.Point(0, 0) // anchor
    //};

    // clearMarkers();
    deleteMarkers();
    // Declare a myLocation marker using icon declared above, and bind it to the map
    var myLocationMarker = new google.maps.Marker({
        map: map,
        title: "Vị trí của tôi",
        // icon:myLocationIcon
    });

    //Identify current user's location and bind it to the map
    //Using HTML5 geolocation.
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var pos = {
                lat: position.coords.latitude,
                lng: position.coords.longitude
            };

            //   myLocationMarker.setPosition(pos);
            addMarker(pos);
            map.zoom = 14;
            map.setCenter(pos);
            
           
        }, function () {
            handleLocationError(true, myLocationMarker, map.getCenter());
        });
    } else {
        // Browser doesn't support Geolocation
        handleLocationError(false, myLocationMarker, map.getCenter());
    }

}

function handleLocationError(browserHasGeolocation, infoWindow, pos) {
    infoWindow.setPosition(pos);
    infoWindow.setContent(browserHasGeolocation ?
                          'Error: The Geolocation service failed.' :
                          'Error: Your browser doesn\'t support geolocation.');
}

var rad = function (x) {    
    return x * Math.PI / 180;
};

var getDistance = function(p1, p2) {
    var R = 6378137; // Earth’s mean radius in meter
    var dLat = rad(p2.lat - p1.lat);
    var dLong = rad(p2.lng - p1.lng);
    var a = Math.sin(dLat / 2) * Math.sin(dLat / 2) +
      Math.cos(rad(p1.lat)) * Math.cos(rad(p2.lat)) *
      Math.sin(dLong / 2) * Math.sin(dLong / 2);
    var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
    var d = R * c;
    return d; // returns the distance in meter
};

// Adds a marker to the map and push to the array.
function addMarker(location) {
    var marker = new google.maps.Marker({
        position: location,
        map: map
    });
    listMarkers.push(marker);
}

// Sets the map on all markers in the array.
function setMapOnAll(map) {
    for (var i = 0; i < listMarkers.length; i++) {
        listMarkers[i].setMap(map);
    }
}

// Removes the markers from the map, but keeps them in the array.
function clearMarkers() {
    setMapOnAll(null);
}

// Shows any markers currently in the array.
function showMarkers() {
    setMapOnAll(map);
}

// Deletes all markers in the array by removing references to them.
function deleteMarkers() {
    clearMarkers();
    listMarkers = [];
}