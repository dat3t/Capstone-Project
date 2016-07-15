var listMarkers = [];
var map;
var bounds;
var userInfoWindow;
var infowindow;
var infowindowContent;

var myCurrentLocationMarker;
var markerCluster;

var listUserMarkers = [], listMaleMarkers = [], listFemaleMarkers = [], listLGBTMarkers = [];

var listType0Markers = [], listType1Markers = [], listType2Markers = [], listType3Markers = [], listType4Markers = [], listType5Markers = [];

var userMarkerCluster = [], maleMarkerCluster = [], femaleMarkerCluster = [], LGBTMarkerCluster = [];

var type0MarkerCluster = [], type1MarkerCluster = [], type2MarkerCluster = [], type3MarkerCluster = [], type4MarkerCluster = [], type5MarkerCluster = [];

//Declare an icon of sample marker
var image = 'https://developers.google.com/maps/documentation/javascript/examples/full/images/beachflag.png';

function checkAuthenticated() {
    if (isAuthenticated) {
        //Declare a new map
        map = new google.maps.Map(document.getElementById('map_canvas'), {
            center: { lat: 10.8114587, lng: 106.67885000000001 },
            zoom: 13,
            minZoom: 4,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        });
        myCurrentLocationMarker = new google.maps.Marker({
            map: map,
            title: "Vị trí của tôi",
            // icon:myLocationIcon
        });

    }
    else {
        //Declare a new map
        map = new google.maps.Map(document.getElementById('map_canvas'), {
            center: { lat: -34.397, lng: 150.644 },
            zoom: 13,
            minZoom: 4,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        });
        // Declare a myLocation marker using icon declared above, and bind it to the map

        myCurrentLocationMarker = new google.maps.Marker({
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

                myCurrentLocationMarker.setPosition(pos);
                //addMarker(pos);
                map.setCenter(pos);
                map.setZoom(7);


            }, function () {
                handleLocationError(true, myCurrentLocationMarker, map.getCenter());
            });
        } else {
            // Browser doesn't support Geolocation
            handleLocationError(false, myCurrentLocationMarker, map.getCenter());
        }
        //map.fitBounds(map.getBounds());

    }
}

function initialize() {

    checkAuthenticated();


    //Declare a bound
    bounds = new google.maps.LatLngBounds();

    //var iw = new map.InfoWindow();
    var oms = new OverlappingMarkerSpiderfier(map);

    createListUserMarkers();
    createListMaleMarkers();
    createListFemaleMarkers();
    createListLGBTMarkers();
    createListType0Markers();
    createListType1Markers();
    createListType2Markers();
    createListType3Markers();
    createListType4Markers();
    createListType5Markers();

    userMarkerCluster = new MarkerClusterer(map, listUserMarkers);
    maleMarkerCluster = new MarkerClusterer(map, listMaleMarkers);
    femaleMarkerCluster = new MarkerClusterer(map, listFemaleMarkers);
    LGBTMarkerCluster = new MarkerClusterer(map, listLGBTMarkers);
    type0MarkerCluster = new MarkerClusterer(map, listType0Markers);
    type1MarkerCluster = new MarkerClusterer(map, listType1Markers);
    type2MarkerCluster = new MarkerClusterer(map, listType2Markers);
    type3MarkerCluster = new MarkerClusterer(map, listType3Markers);
    type4MarkerCluster = new MarkerClusterer(map, listType4Markers);
    type5MarkerCluster = new MarkerClusterer(map, listType5Markers);

    userMarkerCluster.setMaxZoom(9);
    showUsers();

    var markers = [];
    var marker5 = new google.maps.Marker({
        position: { lat: 16.0544068, lng: 108.20216670000002 },
        map: map,
        title: 'First Marker'
    });
    var marker6 = new google.maps.Marker({
        position: { lat: 16.0544068, lng: 108.20216670000002 },
        map: map,
        title: 'Second Marker'
    });
    var marker7 = new google.maps.Marker({
        // position: { lat: 15.8800584, lng: 108.3380469 },
        position: { lat: 16.0544068, lng: 108.20216670000002 },
        map: map,
        title: 'Second Marker'
    });
    // there is better way of doing things, Below is done to show you in detail whats going on
    markers.push(marker5);
    markers.push(marker6);
    markers.push(marker7);
    // now we have 2 markers with different title and same position

    // lets now create our markerClusterer instance with markers array
    //var markerCluster = new MarkerClusterer(map, markers,{zoomOnClick:false,maxZoom:15});
    //var oms = new OverlappingMarkerSpiderfier(map);
    oms.addMarker(marker5);
    oms.addMarker(marker6);
    oms.addMarker(marker7);
    markerCluster = new MarkerClusterer(map, markers);
    markerCluster.setMaxZoom(11);

    userInfoWindow = new google.maps.InfoWindow({
        content: "",
        maxWidth: 350
    });
    //google.maps.event.addListener(markerCluster, "clusterover", function (mCluster) {
    //    //infowindow.content += "aa";
    //    //infowindow.setPosition(mCluster.getCenter());
    //    //infowindow.open(map);
    //    alert(10);
    //});

    // *
    // START INFOWINDOW CUSTOMIZE.
    // The google.maps.event.addListener() event expects
    // the creation of the infowindow HTML structure 'domready'
    // and before the opening of the infowindow, defined styles are applied.
    // *

    // Create the search box and link it to the UI element.
    var input = document.getElementById("pac-input2");
    var searchBox = new google.maps.places.SearchBox(input);
    //map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);

    // Bias the SearchBox results towards current map's viewport.
    map.addListener('bounds_changed', function () {
        // searchBox.setBounds(map.getBounds());
        // map.setZoom(14);
    });

    var markers2 = [];

    //var p1 = { lat: 36.23081510000001, lng: 137.9643552 };

    //var p2 = { lat: 36.238666, lng: 137.96902209999996 };

    for (var i = 0; i < 1000; i++) {
        // var dataPhoto = data.photos[i];
        var latLng = new google.maps.LatLng(Math.floor(Math.random() * 50), Math.floor(Math.random() * 100));
        var marker = new google.maps.Marker({
            position: latLng
        });
        markers2.push(marker);
    }

    markerCluster2 = new MarkerClusterer(map, null);
    markerCluster2.addMarkers(markers2);

    // Listen for the event fired when the user selects a prediction and retrieve
    // more details for that place.
    var marker2 = [];
    searchBox.addListener('places_changed', function () {
        var places = searchBox.getPlaces();
        if (places.length == 0) {
            return;
        }

        //// Clear out the old markers.
        //marker2.forEach(function (marker) {
        //    marker.setMap(null);
        //});
        //marker2 = [];

        //// For each place, get the icon, name and location.
        //var bounds = new google.maps.LatLngBounds();
        ////places.forEach(function (place) {
        ////    var icon = {
        ////        url: place.icon,
        ////        size: new google.maps.Size(71, 71),
        ////        origin: new google.maps.Point(0, 0),
        ////        anchor: new google.maps.Point(17, 34),
        ////        scaledSize: new google.maps.Size(25, 25)
        ////    };

        //    //// Create a marker for each place.
        //    //marker2.push(new google.maps.Marker({
        //    //    map: map,
        //    //    icon: icon,
        //    //    title: place.name,
        //    //    position: place.geometry.location
        //    //}));

        //    if (place.geometry.viewport) {
        //        // Only geocodes have viewport.
        //        bounds.union(place.geometry.viewport);
        //    } else {
        //        bounds.extend(place.geometry.location);
        //    }
        //});
        map.fitBounds(bounds);
    });

    // A new Info Window is created and set content
    infowindow = new google.maps.InfoWindow({
        //content: content,
        //content: '@Html.Partial("CustomInfoWindow")',
        // Assign a maximum value for the width of the infowindow allows
        // greater control over the various content elements
        maxWidth: 350
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

        // Reference to the DIV that wraps the bottom of infowindow--
        var iwOuter = $('.gm-style-iw');

        /* Since this div is in a position prior to .gm-div style-iw.
         * We use jQuery and create a iwBackground variable,
         * and took advantage of the existing reference .gm-style-iw for the previous div with .prev().
        */
        iwOuter.children(':nth-child(1)').css({ 'max-width': '350px' });
        var iwBackground = iwOuter.prev();

        //iwBackground.parent().css({ height: '218px' });
        iwBackground.parent().css({ width: '350px' });
        //iwBackground.parent().css({ bottom: '218px' });

        // Removes background shadow DIV
        iwBackground.children(':nth-child(2)').css({ 'display': 'none' });

        // Removes white background DIV
        //iwBackground.children(':nth-child(4)').css({ width: '350px !important' });
        //iwBackground.children(':nth-child(4)').css({ height: '218px !important' });
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

    //    // Automatically center the map fitting all markers on the screen
    //    map.fitBounds(bounds);
    //}

    //google.maps.event.addListenerOnce(map, 'bounds_changed', function () {
    //  //  map.setZoom(14);

    //    map.setCenter({ lat: 36.238666, lng: 137.96902209999996 });
    //});

    // This event expects a click on a marker
    // When this event is fired the Info Window is opened.
    //google.maps.event.addListener(marker, 'click', function () {
    //    infowindow.open(map, marker);
    //});


}

function loadScript() {
    var script = document.createElement("script");
    script.src = "http://maps.googleapis.com/maps/api/js?key=AIzaSyBiPDMBCKXsusl5-BgCw1nIyHwu5u3j8xw&libraries=places,geometry&callback=initialize";
    document.body.appendChild(script);

}

//window.onload = initialize;
google.maps.event.addDomListener(window, 'load', initialize);


function showCurrentLocation() {
    //alert(aa);
    // alert(array[0].x);
    var p1 = { lat: 36.23081510000001, lng: 137.9643552 };

    var p2 = { lat: 36.238666, lng: 137.96902209999996 };

    //document.getElementById('cal').innerHTML = getDistance(p1,p2);

    // myCurrentLocationMarker.setMap(null);
    // Declare a myLocation marker using icon declared above, and bind it to the map


    //Identify current user's location and bind it to the map
    //Using HTML5 geolocation.
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var pos = {
                lat: position.coords.latitude,
                lng: position.coords.longitude
            };

            myCurrentLocationMarker.setPosition(pos);
            // addMarker(pos);
            map.setZoom(14);
            map.setCenter(pos);

        }, function () {
            alert("aa");
            handleLocationError(true, myCurrentLocationMarker, map.getCenter());
        });
    } else {
        // Browser doesn't support Geolocation
        handleLocationError(false, myCurrentLocationMarker, map.getCenter());
    }

}

function setMapToAMarkerCluster(markerCluster) {
    userMarkerCluster.setMap(null);
    maleMarkerCluster.setMap(null);
    femaleMarkerCluster.setMap(null);
    LGBTMarkerCluster.setMap(null);
    type0MarkerCluster.setMap(null);
    type1MarkerCluster.setMap(null);
    type2MarkerCluster.setMap(null);
    type3MarkerCluster.setMap(null);
    type4MarkerCluster.setMap(null);
    type5MarkerCluster.setMap(null);

    markerCluster.setMap(map);
}

function showUsers() {
    setMapToAMarkerCluster(userMarkerCluster);
}

function showMales() {
    setMapToAMarkerCluster(maleMarkerCluster);
}

function showFemales() {
    setMapToAMarkerCluster(femaleMarkerCluster);
}

function showLGBT() {
    setMapToAMarkerCluster(LGBTMarkerCluster);
}

function showAccommodation() {
    setMapToAMarkerCluster(type0MarkerCluster);
}

function showJobOffer() {
    setMapToAMarkerCluster(type1MarkerCluster);
}

function showFurnitureOffer() {
    setMapToAMarkerCluster(type2MarkerCluster);
}

function showHandGoodsOffer() {
    setMapToAMarkerCluster(type3MarkerCluster);
}

function showTradeOffer() {
    setMapToAMarkerCluster(type4MarkerCluster);
}

function showSOS() {
    setMapToAMarkerCluster(type5MarkerCluster);
}

function createListUserMarkers() {
    var length = allUsers.length;
    var icon = {
        url: "../Content/Icon/Users.png",
        size: new google.maps.Size(71, 71),
        origin: new google.maps.Point(0, 0),
        anchor: new google.maps.Point(17, 34),
        scaledSize: new google.maps.Size(25, 25)
    };
    for (var i = 0; i < length; i++) {
        var position = new google.maps.LatLng(allUsers[i].x, allUsers[i].y);
        bounds.extend(position);
        marker = new google.maps.Marker({
            position: position,
            map: null,
            title: allUsers[i].userID,

            icon: icon
        });
        listUserMarkers.push(marker);
        // Allow each marker to have an info window
        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                getUserInfo(allUsers[i].userID);

                infowindow.open(map, marker);
            }
        })(marker, i));

        // add the double-click event listener
        //google.maps.event.addListener(marker, 'dblclick', function (event) {
        //    map = marker.getMap();
        //    map.setCenter(marker.getPosition()); // set map center to marker position
        //    smoothZoom(map, 10, map.getZoom()); // call smoothZoom, parameters map, final zoomLevel, and starting zoom level
        //});

        // Automatically center the map fitting all markers on the screen
        //    map.fitBounds(bounds);

    }
}

function createListMaleMarkers() {
    var icon = {
        url: "../Content/Icon/male.png",
        size: new google.maps.Size(71, 71),
        origin: new google.maps.Point(0, 0),
        anchor: new google.maps.Point(17, 34),
        scaledSize: new google.maps.Size(25, 25)
    };
    var length = males.length;
    for (var i = 0; i < length; i++) {
        var position = new google.maps.LatLng(males[i].x, males[i].y);
        bounds.extend(position);
        marker = new google.maps.Marker({
            position: position,
            map: null,
            // title: array[i].address,
            icon: icon
        });
        listMaleMarkers.push(marker);
        // Allow each marker to have an info window
        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                // infowindow.setContent(infoWindowContent[i][0]);
                // AjaxDisplayString(userInfoWindow, marker)
                infowindow.open(map, marker);
            }
        })(marker, i));
    }
}

function createListFemaleMarkers() {
    var icon = {
        url: "../Content/Icon/female.png",
        size: new google.maps.Size(71, 71),
        origin: new google.maps.Point(0, 0),
        anchor: new google.maps.Point(17, 34),
        scaledSize: new google.maps.Size(25, 25)
    };
    var length = females.length;
    for (var i = 0; i < length; i++) {
        var position = new google.maps.LatLng(females[i].x, females[i].y);
        bounds.extend(position);
        marker = new google.maps.Marker({
            position: position,
            map: null,
            //  title: array[i].address,
            icon: icon
        });
        listFemaleMarkers.push(marker);
        // Allow each marker to have an info window
        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                // AjaxDisplayString(userInfoWindow, marker)
                infowindow.open(map, marker);
            }
        })(marker, i));
    }
}

function createListLGBTMarkers() {
    var icon = {
        url: "../Content/Icon/LGBT.png",
        size: new google.maps.Size(71, 71),
        origin: new google.maps.Point(0, 0),
        anchor: new google.maps.Point(17, 34),
        scaledSize: new google.maps.Size(25, 25)
    };
    var length = LGBT.length;
    for (var i = 0; i < length; i++) {
        var position = new google.maps.LatLng(LGBT[i].x, LGBT[i].y);
        bounds.extend(position);
        marker = new google.maps.Marker({
            position: position,
            map: null,
            //   title: array[i].address,
            icon: icon
        });
        listLGBTMarkers.push(marker);
        // Allow each marker to have an info window
        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                // infowindow.setContent(infoWindowContent[i][0]);
                AjaxDisplayString(userInfoWindow, marker)
                // infowindow.open(map, marker);
            }
        })(marker, i));
    }
}

function createListType0Markers() {
    var length = postType0.length;
    var icon = {
        url: "../Content/Icon/home.png",
        size: new google.maps.Size(71, 71),
        origin: new google.maps.Point(0, 0),
        anchor: new google.maps.Point(17, 34),
        scaledSize: new google.maps.Size(25, 25)
    };
    for (var i = 0; i < length; i++) {
        var position = new google.maps.LatLng(postType0[i].x, postType0[i].y);
        bounds.extend(position);
        marker = new google.maps.Marker({
            position: position,
            map: null,
            // title: postType0[i].address,
            icon: icon
        });
        listType0Markers.push(marker);
        // Allow each marker to have an info window
        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                // infowindow.setContent(infoWindowContent[i][0]);
                // AjaxDisplayString(userInfoWindow, marker)
                getPostInfo(postType0[i].postID, 0);
                infowindow.open(map, marker);
            }
        })(marker, i));

    }

}

function createListType1Markers() {
    var icon = {
        url: "../Content/Icon/job.png",
        size: new google.maps.Size(71, 71),
        origin: new google.maps.Point(0, 0),
        anchor: new google.maps.Point(17, 34),
        scaledSize: new google.maps.Size(25, 25)
    };
    var length = postType1.length;
    for (var i = 0; i < length; i++) {
        var position = new google.maps.LatLng(postType1[i].x, postType1[i].y);
        bounds.extend(position);
        marker = new google.maps.Marker({
            position: position,
            map: null,
            //title: postType1[i].address,
            icon: icon
        });
        listType1Markers.push(marker);
        // Allow each marker to have an info window
        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                // infowindow.setContent(infoWindowContent[i][0]);
                //AjaxDisplayString(userInfoWindow, marker)
                //   createPostInfoWindowContent(postType1[i].username, postType1[i].postType, "Giới thiệu arubaito", postType1[i].address);
                infowindow.open(map, marker);
            }
        })(marker, i));

    }
    //  alert(listType1Markers[0].getTitle());

}

function createListType2Markers() {
    var icon = {
        url: "../Content/Icon/free.png",
        size: new google.maps.Size(71, 71),
        origin: new google.maps.Point(0, 0),
        anchor: new google.maps.Point(17, 34),
        scaledSize: new google.maps.Size(25, 25)
    };
    var length = postType2.length;
    for (var i = 0; i < length; i++) {
        var position = new google.maps.LatLng(postType2[i].x, postType2[i].y);
        bounds.extend(position);
        marker = new google.maps.Marker({
            position: position,
            map: null,
            //title: postType1[i].address,
            icon: icon
        });
        listType2Markers.push(marker);
        // Allow each marker to have an info window
        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                // infowindow.setContent(infoWindowContent[i][0]);
                //AjaxDisplayString(userInfoWindow, marker)
                //   createPostInfoWindowContent(postType1[i].username, postType1[i].postType, "Giới thiệu arubaito", postType1[i].address);
                infowindow.open(map, marker);
            }
        })(marker, i));

    }
    //  alert(listType1Markers[0].getTitle());

}

function createListType3Markers() {
    var icon = {
        url: "../Content/Icon/ship.png",
        size: new google.maps.Size(71, 71),
        origin: new google.maps.Point(0, 0),
        anchor: new google.maps.Point(17, 34),
        scaledSize: new google.maps.Size(25, 25)
    };
    var length = postType3.length;
    for (var i = 0; i < length; i++) {
        var position = new google.maps.LatLng(postType3[i].x, postType3[i].y);
        bounds.extend(position);
        marker = new google.maps.Marker({
            position: position,
            map: null,
            //title: postType1[i].address,
            icon: icon
        });
        listType3Markers.push(marker);
        // Allow each marker to have an info window
        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                // infowindow.setContent(infoWindowContent[i][0]);
                //AjaxDisplayString(userInfoWindow, marker)
                //   createPostInfoWindowContent(postType1[i].username, postType1[i].postType, "Giới thiệu arubaito", postType1[i].address);
                infowindow.open(map, marker);
            }
        })(marker, i));

    }
    //  alert(listType1Markers[0].getTitle());

}

function createListType4Markers() {
    var icon = {
        url: "../Content/Icon/sale.png",
        size: new google.maps.Size(71, 71),
        origin: new google.maps.Point(0, 0),
        anchor: new google.maps.Point(17, 34),
        scaledSize: new google.maps.Size(25, 25)
    };
    var length = postType4.length;
    for (var i = 0; i < length; i++) {
        var position = new google.maps.LatLng(postType4[i].x, postType4[i].y);
        bounds.extend(position);
        marker = new google.maps.Marker({
            position: position,
            map: null,
            //title: postType1[i].address,
            icon: icon
        });
        listType4Markers.push(marker);
        // Allow each marker to have an info window
        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                // infowindow.setContent(infoWindowContent[i][0]);
                //AjaxDisplayString(userInfoWindow, marker)
                //   createPostInfoWindowContent(postType1[i].username, postType1[i].postType, "Giới thiệu arubaito", postType1[i].address);
                infowindow.open(map, marker);
            }
        })(marker, i));

    }
    //  alert(listType1Markers[0].getTitle());

}

function createListType5Markers() {
    var icon = {
        url: "../Content/Icon/help.png",
        size: new google.maps.Size(71, 71),
        origin: new google.maps.Point(0, 0),
        anchor: new google.maps.Point(17, 34),
        scaledSize: new google.maps.Size(25, 25)
    };
    var length = postType5.length;
    for (var i = 0; i < length; i++) {
        var position = new google.maps.LatLng(postType5[i].x, postType5[i].y);
        bounds.extend(position);
        marker = new google.maps.Marker({
            position: position,
            map: null,
            //title: postType1[i].address,
            icon: icon
        });
        listType5Markers.push(marker);
        // Allow each marker to have an info window
        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                // infowindow.setContent(infoWindowContent[i][0]);
                //AjaxDisplayString(userInfoWindow, marker)
                //   createPostInfoWindowContent(postType1[i].username, postType1[i].postType, "Giới thiệu arubaito", postType1[i].address);
                infowindow.open(map, marker);
            }
        })(marker, i));

    }
    //  alert(listType1Markers[0].getTitle());

}

function handleLocationError(browserHasGeolocation, infoWindow, pos) {
    alert("bb");
    infoWindow.setPosition(pos);
    infoWindow.setContent(browserHasGeolocation ?
                          'Error: The Geolocation service failed.' :
                          'Error: Your browser doesn\'t support geolocation.');
}

var rad = function (x) {
    return x * Math.PI / 180;
};

var getDistance = function (p1, p2) {
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
function setMapOnAll(map, listMarkers) {
    for (var i = 0; i < listMarkers.length; i++) {
        listMarkers[i].setMap(map);
    }
}

// Removes the markers from the map, but keeps them in the array.
function clearMarkers(listMarkers) {
    setMapOnAll(null, listMarkers);
}

// Shows any markers currently in the array.
function showMarkers() {
    setMapOnAll(map);
}

// Deletes all markers in the array by removing references to them.
function deleteMarkers(listMarkers) {
    clearMarkers(listMarkersTest);
    //listMarkers = [];
    listMarkers = [];
}

// the smooth zoom function
function smoothZoom(map, max, cnt) {
    if (cnt >= max) {
        return;
    }
    else {
        z = google.maps.event.addListener(map, 'zoom_changed', function (event) {
            google.maps.event.removeListener(z);
            smoothZoom(map, max, cnt + 1);
        });
        setTimeout(function () { map.setZoom(cnt) }, 80); // 80ms is what I found to work well on my system -- it might not work well on all systems
    }
}

function createUserInfoWindowContent(name, age, gender, address) {

    var genderOfInfoWindow;

    if (gender == 0) {
        genderOfInfoWindow = "Nữ";
    } else if (gender == 1) {
        genderOfInfoWindow = "Nam";
    } else if (gender == 2) {
        genderOfInfoWindow = "LGBT";
    }

    // InfoWindow content
    var content = '<div style="overflow:hidden;">' +
        '<div id="iw-container">' +
                      '<div class="iw-title">' + name + '</div>' +
                      '<div class="iw-content">' +
                        '<div id="navInfo">' +
                            '<img src="../Content/Images/dat.jpg" alt="Trần Trọng Tiến Đạt" height="115" width="115">' +
                            '</div>' +
                        '<div id="sectionInfo" stype="padding:10px;">' +
                            '<table style="width:220px;height:115px;">' +
                            '<tr>' +
                            '<td><b>Tuổi:</b></td>' +
                            '<td>' + age + '</td> ' +
                            '</tr>' +
                            '<tr>' +
                            '<td><b>Facebook:</b></td>' +
                            '<td><a href="https://www.facebook.com/Dat3T">Dat3T</a></td>' +
                            '</tr>' +
                            '<tr>' +
                            '<td><b>Giới tính:</b></td>' +
                            '<td>' + genderOfInfoWindow + '</td>' +
                            '</tr>' +
                            '<tr>' +
                            '<td><b>Địa chỉ:</b></td>' +
                            '<td>' + address + '</td>' +
                            '</tr>' +
                            '</table>' +
                            '</div>' +
                      '</div>' + '</div>' +
                      '<div class="iw-bottom-gradient"></div>' +
            '</div>';

    infowindow.setContent(content);
}

function createPostInfoWindowContent(username, postType, postTitle, address) {

    var postTypeInfoWindow;

    if (postType == 0) {
        postTypeInfoWindow = "Nhà ở";
    } else if (postType == 1) {
        postTypeInfoWindow = "Việc làm";
    } else if (postType == 2) {
        postTypeInfoWindow = "Cho đồ";
    }

    // InfoWindow content
    var content = '<div style="overflow:hidden;">' +
        '<div id="iw-container">' +
                      '<div class="iw-title">' + 'Bài đăng' + '</div>' +
                      '<div class="iw-content">' +
                        '<div id="navInfo">' +
                            '<img src="../Content/Images/pikachu.jpg" alt="Trần Trọng Tiến Đạt" height="115" width="115">' +
                            '</div>' +
                        '<div id="sectionInfo" stype="padding:10px;">' +
                            '<table style="width:220px;height:115px;">' +
                            '<tr>' +
                            '<td><b>Người đăng:</b></td>' +
                            '<td>' + username + '</td> ' +
                            '</tr>' +
                            '<tr>' +
                            '<td><b>Mục:</b></td>' +
                            '<td>' + postTypeInfoWindow + '</td>' +
                            '</tr>' +
                            '<tr>' +
                            '<td><b>Nội dung:</b></td>' +
                            '<td>' + postTitle + '</td>' +
                            '</tr>' +
                            '<tr>' +
                            '<td><b>Nơi đăng:</b></td>' +
                            '<td>' + address + '</td>' +
                            '</tr>' +
                            '</table>' +
                            '</div>' +
                      '</div>' + '</div>' +
                      '<div class="iw-bottom-gradient"></div>' +
            '</div>';

    infowindow.setContent(content);
}

function getUserInfo(userId) {
    $.ajax({
        url: 'GetUserInfo?userId=' + userId,
        type: 'GET',
        contentType: 'application/json;',
        dataType: 'json',
        success: function (json) {
            createUserInfoWindowContent(json.UserName, 23, json.Gender, json.Location.Address);
        }
    });
}

function getPostInfo(postID, postType) {
    $.ajax({
        url: 'GetPostInfo?postId=' + postID,
        type: 'GET',
        contentType: 'application/json;',
        dataType: 'json',
        success: function (json) {
            createPostInfoWindowContent(json.UserName, postType, json.Title, json.Address);
        }
    });
}