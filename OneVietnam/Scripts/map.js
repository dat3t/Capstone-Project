var listMarkers = [];
var map;
var bounds;
var userInfoWindow;
var infowindow;
var infowindowContent;
var isFirstTime = true;
var myCurrentLocationMarker, myHomeMarker;
var markerCluster;

var listUserMarkers = [], listMaleMarkers = [], listFemaleMarkers = [], listLGBTMarkers = [];

var listType0Markers = [], listType1Markers = [], listType2Markers = [], listType3Markers = [], listType4Markers = [], listType5Markers = [];

var userMarkerCluster = [], maleMarkerCluster = [], femaleMarkerCluster = [], LGBTMarkerCluster = [];

var type0MarkerCluster = [], type1MarkerCluster = [], type2MarkerCluster = [], type3MarkerCluster = [], type4MarkerCluster = [], type5MarkerCluster = [];

var overlappingType0 = [], overlappingType1 = [], overlappingType2 = [], overlappingType3 = [], overlappingType4 = [], overlappingType5 = [];

var overlappingMale = [], overlappingFemale = [], overlappingLGBT = [], overlappingUsers = [];

function checkAuthenticated() {
    var icon = {
        url: "../Content/Icon/location.png",
        size: new google.maps.Size(71, 71),
        origin: new google.maps.Point(0, 0),
        anchor: new google.maps.Point(17, 34),
        scaledSize: new google.maps.Size(25, 25)
    };

    var myhomeicon = {
        url: "../Content/Icon/myhome2.png",
        size: new google.maps.Size(71, 71),
        origin: new google.maps.Point(0, 0),
        anchor: new google.maps.Point(17, 34),
        scaledSize: new google.maps.Size(50, 50)
    };



    // Declare a myLocation marker using icon declared above, and bind it to the map
    myCurrentLocationMarker = new google.maps.Marker({
        title: "Vị trí hiện tại của tôi",
        icon: icon
    });

    myHomeMarker = new google.maps.Marker({
        title: "Vị trí của tôi",
        icon: myhomeicon
    });

    if (isAuthenticated) {
        //Declare a new map
        map = new google.maps.Map(document.getElementById('map_canvas'), {
            center: { lat: authenticatedUser.x, lng: authenticatedUser.y },
            zoom: 13,
            minZoom: 4,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        });
        myCurrentLocationMarker.setMap(map);
        myHomeMarker.setPosition({ lat: authenticatedUser.x, lng: authenticatedUser.y });
        myHomeMarker.setMap(map);

    }
    else {
        //Declare a new map
        map = new google.maps.Map(document.getElementById('map_canvas'), {
            center: { lat: -34.397, lng: 150.644 },
            zoom: 13,
            minZoom: 2,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        });

        myCurrentLocationMarker.setMap(map);
        showCurrentLocation();

    }
}

function initialize() {
    checkAuthenticated();

    //Declare a bound
    bounds = new google.maps.LatLngBounds();

    google.maps.event.addListener(map, 'bounds_changed', function () {
        bounds = map.getBounds();
    });

    google.maps.event.addListener(map, 'idle', function () {
        bounds = map.getBounds();
    });

    //Delcare overlapping, markerClusterer
    overlappingUsers = new OverlappingMarkerSpiderfier(map);
    overlappingMale = new OverlappingMarkerSpiderfier(map);
    overlappingLGBT = new OverlappingMarkerSpiderfier(map);
    overlappingFemale = new OverlappingMarkerSpiderfier(map);
    overlappingType0 = new OverlappingMarkerSpiderfier(map);
    overlappingType1 = new OverlappingMarkerSpiderfier(map);
    overlappingType2 = new OverlappingMarkerSpiderfier(map);
    overlappingType3 = new OverlappingMarkerSpiderfier(map);
    overlappingType4 = new OverlappingMarkerSpiderfier(map);
    overlappingType5 = new OverlappingMarkerSpiderfier(map);

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

    //Init user makers first load
    userMarkerCluster.setMaxZoom(9);
    showUsers();


    // Create the search box and link it to the UI element.
    var input = document.getElementById("pac-input2");
    var searchBox = new google.maps.places.SearchBox(input);

    //marker2 = [];
    //for (var i = 0; i < 1000; i++) {
    //    // var dataPhoto = data.photos[i];
    //    var latLng = new google.maps.LatLng(Math.floor(Math.random() * 50), Math.floor(Math.random() * 100));
    //    var marker = new google.maps.Marker({
    //        position: latLng
    //    });
    //    markers2.push(marker);
    //}

    //markerCluster2 = new MarkerClusterer(map, null);
    //markerCluster2.addMarkers(markers2);

    // Listen for the event fired when the user selects a prediction and retrieve
    // more details for that place.
    searchBox.addListener('places_changed', function () {
        var places = searchBox.getPlaces();
        if (places.length == 0) {
            return;
        }
        // For each place, get the icon, name and location.
        var bounds = new google.maps.LatLngBounds();
        places.forEach(function (place) {

            if (place.geometry.viewport) {
                // Only geocodes have viewport.
                bounds.union(place.geometry.viewport);
            } else {
                bounds.extend(place.geometry.location);
            }
        });
        //map.setZoom(12);
        map.fitBounds(bounds);
    });


}

function loadScript() {
    var script = document.createElement("script");
    script.src = "http://maps.googleapis.com/maps/api/js?key=AIzaSyBiPDMBCKXsusl5-BgCw1nIyHwu5u3j8xw&libraries=places,geometry&callback=initialize";
    document.body.appendChild(script);

}

function showCurrentLocation() {

    myHomeMarker.setMap(null);
    //Identify current user's location and bind it to the map
    //Using HTML5 geolocation.
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var pos = {
                lat: position.coords.latitude,
                lng: position.coords.longitude
            };

            myCurrentLocationMarker.setPosition(pos);
            myCurrentLocationMarker.setMap(map);
            map.setZoom(12);
            map.setCenter(pos);

        }, function () {
            handleLocationError(true, "Không thể định vị được vị trí của bạn. Bạn cần cho phép trình duyệt sử dụng định vị GPS.", map.getCenter());
        });
    } else {
        // Browser doesn't support Geolocation
        handleLocationError(false, "Trình duyệt của bạn không hỗ trợ định vị GPS. Vui lòng nâng cấp phiên bản mới nhất của trình duyệt và thử lại sau.", map.getCenter());
    }

}

function showMyLocation() {
    myCurrentLocationMarker.setMap(null);
    myHomeMarker.setPosition({ lat: authenticatedUser.x, lng: authenticatedUser.y });
    myHomeMarker.setMap(map);
    map.setCenter({ lat: authenticatedUser.x, lng: authenticatedUser.y });
    map.setZoom(14);
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
    if (isFirstTime == false) {
        bounds.extend(calculateNearestMarker(allUsers));
        map.fitBounds(bounds);
    }
    isFirstTime = false;
}

function showMales() {
    setMapToAMarkerCluster(maleMarkerCluster);
    bounds.extend(calculateNearestMarker(males));
    map.fitBounds(bounds);
}

function showFemales() {
    setMapToAMarkerCluster(femaleMarkerCluster);
    bounds.extend(calculateNearestMarker(females));
    map.fitBounds(bounds);
}

function showLGBT() {
    setMapToAMarkerCluster(LGBTMarkerCluster);
    bounds.extend(calculateNearestMarker(LGBT));
    map.fitBounds(bounds);
}

function showAccommodation() {
    setMapToAMarkerCluster(type0MarkerCluster);

    bounds.extend(calculateNearestMarker(postType0));
    map.fitBounds(bounds);

}

function calculateNearestMarker(listLocation) {

    //bounds = map.getBounds();
    var centerOfCurrentBound = bounds.getCenter();
    var k;
    if (listLocation.length > 0) {
        var position = new google.maps.LatLng(listLocation[0].x, listLocation[0].y);
        var min = getDistance(centerOfCurrentBound, position);
        var length = listLocation.length;
        for (var i = 1; i < length; i++) {
            var position2 = new google.maps.LatLng(listLocation[i].x, listLocation[i].y);
            var distance2 = getDistance(centerOfCurrentBound, position2)
            if (min > distance2) {
                min = distance2;
                position = position2;
            }
        }
        return position;
    }
}

function showJobOffer() {
    setMapToAMarkerCluster(type1MarkerCluster);
    bounds.extend(calculateNearestMarker(postType1));
    map.fitBounds(bounds);
}

function showFurnitureOffer() {
    setMapToAMarkerCluster(type2MarkerCluster);
    bounds.extend(calculateNearestMarker(postType2));
    map.fitBounds(bounds);
}

function showHandGoodsOffer() {
    setMapToAMarkerCluster(type3MarkerCluster);
    bounds.extend(calculateNearestMarker(postType3));
    map.fitBounds(bounds);
}

function showTradeOffer() {
    setMapToAMarkerCluster(type4MarkerCluster);
    bounds.extend(calculateNearestMarker(postType4));
    map.fitBounds(bounds);
}
function showSOS() {
    setMapToAMarkerCluster(type5MarkerCluster);
    bounds.extend(calculateNearestMarker(postType5));
    map.fitBounds(bounds);
}

function createListUserMarkers() {
    var length = allUsers.length;
    var icon = {
        url: "/Content/Icon/Users.png",
        size: new google.maps.Size(71, 71),
        origin: new google.maps.Point(0, 0),
        anchor: new google.maps.Point(17, 34),
        scaledSize: new google.maps.Size(50, 50)
    };
    for (var i = 0; i < length; i++) {
        var position = new google.maps.LatLng(allUsers[i].x, allUsers[i].y);
        bounds.extend(position);
        marker = new google.maps.Marker({
            position: position,
            map: null,
            title: "Nhấp để xem chi tiết",
            icon: icon
        });
        listUserMarkers.push(marker);
        overlappingUsers.addMarker(marker);

        // Allow each marker to have an info window
        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                getUserInfo(allUsers[i].userID);
                // infowindow.open(map, marker);
            }
        })(marker, i));

    }
}

function createListMaleMarkers() {
    var icon = {
        url: "../Content/Icon/male.png",
        size: new google.maps.Size(71, 71),
        origin: new google.maps.Point(0, 0),
        anchor: new google.maps.Point(17, 34),
        scaledSize: new google.maps.Size(50, 50)
    };
    var length = males.length;
    for (var i = 0; i < length; i++) {
        var position = new google.maps.LatLng(males[i].x, males[i].y);
        bounds.extend(position);
        marker = new google.maps.Marker({
            position: position,
            map: null,
            title: "Nhấp để xem chi tiết",
            icon: icon
        });
        listMaleMarkers.push(marker);
        overlappingMale.addMarker(marker);
        // Allow each marker to have an info window
        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                // infowindow.setContent(infoWindowContent[i][0]);
                // AjaxDisplayString(userInfoWindow, marker)
                getUserInfo(allUsers[i].userID);
                //infowindow.open(map, marker);
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
        scaledSize: new google.maps.Size(50, 50)
    };
    var length = females.length;
    for (var i = 0; i < length; i++) {
        var position = new google.maps.LatLng(females[i].x, females[i].y);
        bounds.extend(position);
        marker = new google.maps.Marker({
            position: position,
            map: null,
            title: "Nhấp để xem chi tiết",
            icon: icon
        });
        listFemaleMarkers.push(marker);
        overlappingFemale.addMarker(marker);
        // Allow each marker to have an info window
        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                // AjaxDisplayString(userInfoWindow, marker)
                getUserInfo(allUsers[i].userID);
                //infowindow.open(map, marker);
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
        scaledSize: new google.maps.Size(50, 50)
    };
    var length = LGBT.length;
    for (var i = 0; i < length; i++) {
        var position = new google.maps.LatLng(LGBT[i].x, LGBT[i].y);
        bounds.extend(position);
        marker = new google.maps.Marker({
            position: position,
            map: null,
            title: "Nhấp để xem chi tiết",
            icon: icon
        });
        listLGBTMarkers.push(marker);
        overlappingLGBT.addMarker(marker);
        // Allow each marker to have an info window
        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
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
            title: "Nhấp để xem chi tiết",
            icon: icon
        });
        listType0Markers.push(marker);
        overlappingType0.addMarker(marker);
        // Allow each marker to have an info window
        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                getPostInfo(postType0[i].postID);
            }
        })(marker, i));

    }

}

function createListType1Markers() {
    var icon = {
        url: "../Content/Icon/job5.png",
        size: new google.maps.Size(71, 71),
        origin: new google.maps.Point(0, 0),
        anchor: new google.maps.Point(17, 34),
        scaledSize: new google.maps.Size(50, 50)
    };
    var length = postType1.length;
    for (var i = 0; i < length; i++) {
        var position = new google.maps.LatLng(postType1[i].x, postType1[i].y);
        bounds.extend(position);
        marker = new google.maps.Marker({
            position: position,
            map: null,
            title: "Nhấp để xem chi tiết",
            icon: icon
        });
        listType1Markers.push(marker);
        overlappingType1.addMarker(marker);
        // Allow each marker to have an info window
        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                getPostInfo(postType1[i].postID);
            }
        })(marker, i));

    }
    //  alert(listType1Markers[0].getTitle());

}

function createListType2Markers() {
    var icon = {
        url: "../Content/Icon/free5.png",
        size: new google.maps.Size(50, 50),
        origin: new google.maps.Point(0, 0),
        anchor: new google.maps.Point(17, 34),
        scaledSize: new google.maps.Size(50, 50)
    };
    var length = postType2.length;
    for (var i = 0; i < length; i++) {
        var position = new google.maps.LatLng(postType2[i].x, postType2[i].y);
        bounds.extend(position);
        marker = new google.maps.Marker({
            position: position,
            map: null,
            title: "Nhấp để xem chi tiết",
            icon: icon
        });
        listType2Markers.push(marker);
        overlappingType2.addMarker(marker);
        // Allow each marker to have an info window
        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                getPostInfo(postType2[i].postID);
            }
        })(marker, i));

    }
    //  alert(listType1Markers[0].getTitle());

}

function createListType3Markers() {
    var icon = {
        url: "../Content/Icon/ship4.jpg",
        size: new google.maps.Size(71, 71),
        origin: new google.maps.Point(0, 0),
        anchor: new google.maps.Point(17, 34),
        scaledSize: new google.maps.Size(50, 50)
    };
    var length = postType3.length;
    for (var i = 0; i < length; i++) {
        var position = new google.maps.LatLng(postType3[i].x, postType3[i].y);
        bounds.extend(position);
        marker = new google.maps.Marker({
            position: position,
            map: null,
            title: "Nhấp để xem chi tiết",
            icon: icon
        });
        listType3Markers.push(marker);
        overlappingType3.addMarker(marker);
        // Allow each marker to have an info window
        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                getPostInfo(postType3[i].postID);
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
        scaledSize: new google.maps.Size(50, 50)
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
        overlappingType4.addMarker(marker);
        // Allow each marker to have an info window
        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                getPostInfo(postType4[i].postID);
            }
        })(marker, i));

    }
    //  alert(listType1Markers[0].getTitle());

}

function createListType5Markers() {
    var icon = {
        url: "../Content/Icon/help3.png",
        size: new google.maps.Size(71, 71),
        origin: new google.maps.Point(0, 0),
        anchor: new google.maps.Point(17, 34),
        scaledSize: new google.maps.Size(50, 50)
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
        overlappingType5.addMarker(marker);
        // Allow each marker to have an info window
        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                getPostInfo(postType5[i].postID);
            }
        })(marker, i));

    }
}

function handleLocationError(browserHasGeolocation, message, pos) {
    alert(message);
}

var rad = function (x) {
    return x * Math.PI / 180;
};

var getDistance = function (p1, p2) {
    var R = 6378137; // Earth’s mean radius in meter
    var dLat = rad(p2.lat() - p1.lat());
    var dLong = rad(p2.lng() - p1.lng());
    var a = Math.sin(dLat / 2) * Math.sin(dLat / 2) +
      Math.cos(rad(p1.lat())) * Math.cos(rad(p2.lat())) *
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

function getUserInfo(userId) {
    $.ajax({
        url: 'GetUserPartialView?userId=' + userId,
        type: 'GET',
        success: function (result) {
            if (result != '') {
                $("#userModal").empty();
                $("#userModal").html(result);
                $("#userModal").modal('show');
            }
        },
        error: function (xhr, status, error) {
            alert(xhr.responseText);
        }
    });

}

function getPostInfo(postID) {
    $.ajax({
        url: '/Map/GetPostPartialView?postId=' + postID,
        type: 'GET',
        dataType: 'text',
        success: function (result) {
            //   createUserInfoWindowContent(json.UserName, 23, json.Gender, json.Location.Address);
            if (result != '') {

                $("#postModal").empty();

                $("#postModal").html(result);

                $("#postModal").modal('show');
                // alert(result);
            }

        },
        error: function (xhr, status, error) {
            alert(xhr.responseText);
        }
    });
}

function showSelectedPostOnMap(Lat, Lng, PostType, PostId, isCallFromPostDetail) {
    switch (PostType) {
        case 0: showAccommodation(); break;
        case 1: showJobOffer(); break;
        case 2: showFurnitureOffer(); break;
        case 3: showHandGoodsOffer(); break;
        case 4: showTradeOffer(); break;
        case 5: showSOS(); break;
    }
    map.setZoom(14);
    map.setCenter({ lat: Lat, lng: Lng });
    if (isCallFromPostDetail != 1) {
        setTimeout(function () { getPostInfo(PostId); }, 1000);
    }


}

//window.onload = initialize;
google.maps.event.addDomListener(window, 'load', initialize);
//$(document).ready(initialize);