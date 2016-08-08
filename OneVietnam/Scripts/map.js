var listMarkers = [];
var map;
var bounds;
var userInfoWindow;
var infowindow;
var infowindowContent;
var isFirstTime = true;
var myCurrentLocationMarker, myHomeMarker;
var markerCluster;
var isClickOnSpiderfier = true;

var listUserMarkers = [], listMaleMarkers = [], listFemaleMarkers = [], listLGBTMarkers = [];

var listType3Markers = [], listType4Markers = [], listType5Markers = [], listType6Markers = [], listType7Markers = [], listType8Markers = [], listType9Markers = [];

var userMarkerCluster = [], maleMarkerCluster = [], femaleMarkerCluster = [], LGBTMarkerCluster = [];

var type3MarkerCluster = [], type4MarkerCluster = [], type5MarkerCluster = [], type6MarkerCluster = [], type7MarkerCluster = [], type8MarkerCluster = [], type9MarkerCluster = [];

var overlappingType3 = [], overlappingType4 = [], overlappingType5 = [], overlappingType6 = [], overlappingType7 = [], overlappingType8 = [], overlappingType9 = [];

var overlappingMale = [], overlappingFemale = [], overlappingLGBT = [], overlappingUsers = [];

function checkAuthenticated() {
    var icon = {
        url: "../Content/Icon/location.png",
        origin: new google.maps.Point(0, 0),
        anchor: new google.maps.Point(17, 34),
        scaledSize: new google.maps.Size(50, 50)
    };

    var myhomeicon = {
        url: "../Content/Icon/myhome.png",
        origin: new google.maps.Point(0, 0),
        anchor: new google.maps.Point(17, 34),
        scaledSize: new google.maps.Size(50, 50)
    };



    // Declare a myLocation marker using icon declared above, and bind it to the map
    myCurrentLocationMarker = new google.maps.Marker({
        title: "Vị trí hiện tại của tôi",
        optimized: false,
        icon: icon
    });

    myHomeMarker = new google.maps.Marker({
        title: "Vị trí của tôi",
        optimized: false,
        icon: myhomeicon
    });

    if (isAuthenticated) {
        //Declare a new map
        map = new google.maps.Map(document.getElementById('map_canvas'), {
            center: { lat: authenticatedUser.x, lng: authenticatedUser.y },
            zoom: 12,
            minZoom: 2,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        });
        myCurrentLocationMarker.setMap(map);
        myHomeMarker.setPosition({ lat: authenticatedUser.x, lng: authenticatedUser.y });
        myHomeMarker.setMap(map);

    }
    else {
        //Declare a new map
        map = new google.maps.Map(document.getElementById('map_canvas'), {
            center: { lat: 21.0277644, lng: 105.83415979999995 },
            zoom: 12,
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

    //google.maps.event.addListener(map, 'bounds_changed', function () {
    //    bounds = map.getBounds();
    //});

    google.maps.event.addListener(map, 'idle', function () {
        bounds = map.getBounds();
    });

    //Delcare overlapping, markerClusterer
    overlappingUsers = new OverlappingMarkerSpiderfier(map, { circleFootSeparation: 60 });
    overlappingMale = new OverlappingMarkerSpiderfier(map, { circleFootSeparation: 60 });
    overlappingLGBT = new OverlappingMarkerSpiderfier(map, { circleFootSeparation: 60 });
    overlappingFemale = new OverlappingMarkerSpiderfier(map, { circleFootSeparation: 60 });
    overlappingType3 = new OverlappingMarkerSpiderfier(map, { circleFootSeparation: 60 });
    overlappingType4 = new OverlappingMarkerSpiderfier(map, { circleFootSeparation: 60 });
    overlappingType5 = new OverlappingMarkerSpiderfier(map, { circleFootSeparation: 60 });
    overlappingType6 = new OverlappingMarkerSpiderfier(map, { circleFootSeparation: 60 });
    overlappingType7 = new OverlappingMarkerSpiderfier(map, { circleFootSeparation: 60 });
    overlappingType8 = new OverlappingMarkerSpiderfier(map, { circleFootSeparation: 60 });
    overlappingType9 = new OverlappingMarkerSpiderfier(map, { circleFootSeparation: 60 });

    createListUserMarkers();
    createListMaleMarkers();
    createListFemaleMarkers();
    createListLGBTMarkers();
    createlistType3Markers();
    createlistType4Markers();
    createlistType5Markers();
    createListType6Markers();
    createListType7Markers();
    createListType8Markers();
    createListType9Markers();

    userMarkerCluster = new MarkerClusterer(map, listUserMarkers);
    maleMarkerCluster = new MarkerClusterer(map, listMaleMarkers);
    femaleMarkerCluster = new MarkerClusterer(map, listFemaleMarkers);
    LGBTMarkerCluster = new MarkerClusterer(map, listLGBTMarkers);
    type3MarkerCluster = new MarkerClusterer(map, listType3Markers);
    type4MarkerCluster = new MarkerClusterer(map, listType4Markers);
    type5MarkerCluster = new MarkerClusterer(map, listType5Markers);
    type6MarkerCluster = new MarkerClusterer(map, listType6Markers);
    type7MarkerCluster = new MarkerClusterer(map, listType7Markers);
    type8MarkerCluster = new MarkerClusterer(map, listType8Markers);
    type9MarkerCluster = new MarkerClusterer(map, listType9Markers);

    //Init user makers first load
    userMarkerCluster.setMaxZoom(9);
    showUsers();

    // Create the search box and link it to the UI element.
    var input = document.getElementById("pac-input2");
    var searchBox = new google.maps.places.SearchBox(input);

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

    google.maps.event.addListener(map, 'zoom_changed', function () {
        setTimeout(function () {
            var cnt = map.getCenter();
            cnt.e += 0.000001;
            map.panTo(cnt);
            cnt.e -= 0.000001;
            map.panTo(cnt);
        }, 400);
    });

}

function loadScript() {
    var script = document.createElement("script");
    script.src = "http://maps.googleapis.com/maps/api/js?key=AIzaSyBiPDMBCKXsusl5-BgCw1nIyHwu5u3j8xw&libraries=places,geometry&callback=initialize";
    document.body.appendChild(script);

}

function showCurrentLocation() {
    // setMapToAMarkerCluster(null);
    myHomeMarker.setMap(null);

    //Identify current user's location and bind it to the map
    //Using HTML5 geolocation.
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var pos = {
                lat: position.coords.latitude,
                lng: position.coords.longitude
            };
            var pos2 = new google.maps.LatLng(position.coords.latitude, position.coords.longitude)

            myCurrentLocationMarker.setMap(map);
            myCurrentLocationMarker.setPosition(pos);
            google.maps.event.addListener(myCurrentLocationMarker, 'click', (function (marker) {
                return function () {
                    map.setCenter(pos2);
                    map.setZoom(14);
                }
            })(marker));

            //map.setZoom(12);
            checkIfBoundContainPosition(pos2);
            //bounds.extend(pos);
            //map.fitBounds(bounds);
            //map.setCenter(pos);
            // map.setCenter(pos);

        }, function () {
            handleLocationError(true, "Không thể định vị được vị trí của bạn. Bạn cần cho phép trình duyệt sử dụng định vị GPS.", map.getCenter());
        });
    } else {
        // Browser doesn't support Geolocation
        handleLocationError(false, "Trình duyệt của bạn không hỗ trợ định vị GPS. Vui lòng nâng cấp phiên bản mới nhất của trình duyệt và thử lại sau.", map.getCenter());
    }

}

function showMyLocation() {
    //setMapToAMarkerCluster(null);
    myCurrentLocationMarker.setMap(null);

    myHomeMarker.setMap(map);
    myHomeMarker.setPosition({ lat: authenticatedUser.x, lng: authenticatedUser.y });

    var pos2 = new google.maps.LatLng(authenticatedUser.x, authenticatedUser.y)
    google.maps.event.addListener(myHomeMarker, 'click', (function (marker) {
        return function () {
            map.setCenter(pos2);
            map.setZoom(14);
        }
    })(marker));
    checkIfBoundContainPosition(pos2);
    // map.setCenter({ lat: authenticatedUser.x, lng: authenticatedUser.y });
}

function setMapToAMarkerCluster(markerCluster) {

    userMarkerCluster.setMap(null);
    maleMarkerCluster.setMap(null);
    femaleMarkerCluster.setMap(null);
    LGBTMarkerCluster.setMap(null);
    type3MarkerCluster.setMap(null);
    type4MarkerCluster.setMap(null);
    type5MarkerCluster.setMap(null);
    type6MarkerCluster.setMap(null);
    type7MarkerCluster.setMap(null);
    type8MarkerCluster.setMap(null);
    type9MarkerCluster.setMap(null);
    //myCurrentLocationMarker.setMap(null);
    //myHomeMarker.setMap(null);

    if (markerCluster != null) {
        markerCluster.setMap(map);
    }
}

function calculateNearestMarker(listLocation) {

    //bounds = map.getBounds();
    var centerOfCurrentBound = bounds.getCenter();
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

function showUsers() {
    setMapToAMarkerCluster(userMarkerCluster);
    showAlertNoUser(allUsers);

    if (isFirstTime == false) {
        var pos = calculateNearestMarker(allUsers);
        checkIfBoundContainPosition(pos);
    }
    isFirstTime = false;
}

function showMales() {
    setMapToAMarkerCluster(maleMarkerCluster);
    showAlertNoUser(males);
    maleMarkerCluster.setMaxZoom(9);
    var pos = calculateNearestMarker(males);
    checkIfBoundContainPosition(pos);
}

function showFemales() {
    setMapToAMarkerCluster(femaleMarkerCluster);
    showAlertNoUser(females);
    femaleMarkerCluster.setMaxZoom(9);
    var pos = calculateNearestMarker(females);
    checkIfBoundContainPosition(pos);
}

function showLGBT() {
    setMapToAMarkerCluster(LGBTMarkerCluster);
    showAlertNoUser(LGBT);
    LGBTMarkerCluster.setMaxZoom(9);
    var pos = calculateNearestMarker(LGBT);
    checkIfBoundContainPosition(pos);
}

function showAccommodation() {
    setMapToAMarkerCluster(type3MarkerCluster);
    showAlertNoPost(postType3);
    type3MarkerCluster.setMaxZoom(9);
    // openAllClusters(overlappingType3);
    var pos = calculateNearestMarker(postType3);
    checkIfBoundContainPosition(pos);


}

function showJobOffer() {
    setMapToAMarkerCluster(type4MarkerCluster);
    showAlertNoPost(postType4);
    type4MarkerCluster.setMaxZoom(9);
    var pos = calculateNearestMarker(postType4);
    checkIfBoundContainPosition(pos);
}

function showFurnitureOffer() {

    setMapToAMarkerCluster(type5MarkerCluster);
    showAlertNoPost(postType5);
    type5MarkerCluster.setMaxZoom(9);
    var pos = calculateNearestMarker(postType5);
    checkIfBoundContainPosition(pos);
}

function showHandGoodsOffer() {
    setMapToAMarkerCluster(type6MarkerCluster);
    showAlertNoPost(postType6);
    type6MarkerCluster.setMaxZoom(9);
    var pos = calculateNearestMarker(postType6);
    checkIfBoundContainPosition(pos);

}

function showTradeOffer() {
    setMapToAMarkerCluster(type7MarkerCluster);
    showAlertNoPost(postType7);
    type7MarkerCluster.setMaxZoom(9);
    var pos = calculateNearestMarker(postType7);
    checkIfBoundContainPosition(pos);
}
function showSOS() {
    setMapToAMarkerCluster(type8MarkerCluster);
    showAlertNoPost(postType8);
    type8MarkerCluster.setMaxZoom(9);
    var pos = calculateNearestMarker(postType8);
    checkIfBoundContainPosition(pos);

}
function showWarning() {
    setMapToAMarkerCluster(type9MarkerCluster);
    showAlertNoPost(postType9);
    type9MarkerCluster.setMaxZoom(9);
    var pos = calculateNearestMarker(postType9);
    checkIfBoundContainPosition(pos);
}

function showAlertNoPost(postTypeArray) {
    if (postTypeArray.length == 0) {
        $("#postEmptyAlertModal").modal('show');
    }
}

function showAlertNoUser(postTypeArray) {
    if (postTypeArray.length == 0) {
        $("#userEmptyAlertModal").modal('show');
    }
}

function createListUserMarkers() {
    var length = allUsers.length;
    var icon = {
        url: "/Content/Icon/users.png",
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
            optimized: false,
            title: "Nhấp để xem chi tiết",
            icon: icon
        });
        listUserMarkers.push(marker);
        // Allow each marker to have an info window
        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                setTimeout(function () {
                    getUserInfo(allUsers[i].userID);
                }, 100);
            }
        })(marker, i));

        overlappingUsers.addMarker(marker);
    }

    overlappingUsers.addListener('click', function (marker) {
        isClickOnSpiderfier = false;
    });
    overlappingUsers.addListener('spiderfy', function (markers) {
        isClickOnSpiderfier = true;
    });

}

function createListMaleMarkers() {
    var icon = {
        url: "../Content/Icon/male.png",
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
            optimized: false,
            title: "Nhấp để xem chi tiết",
            icon: icon
        });
        listMaleMarkers.push(marker);

        // Allow each marker to have an info window
        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                setTimeout(function () {
                    getUserInfo(males[i].userID);
                }, 100);
            }
        })(marker, i));

        overlappingMale.addMarker(marker);
    }

    overlappingMale.addListener('click', function (marker) {
        isClickOnSpiderfier = false;
    });
    overlappingMale.addListener('spiderfy', function (markers) {
        isClickOnSpiderfier = true;
    });
}

function createListFemaleMarkers() {
    var icon = {
        url: "../Content/Icon/female.png",
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
            optimized: false,
            map: null,
            title: "Nhấp để xem chi tiết",
            icon: icon
        });
        listFemaleMarkers.push(marker);

        // Allow each marker to have an info window
        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                setTimeout(function () {
                    getUserInfo(females[i].userID);
                }, 100);
            }
        })(marker, i));

        overlappingFemale.addMarker(marker);
    }

    overlappingFemale.addListener('click', function (marker) {
        isClickOnSpiderfier = false;
    });
    overlappingFemale.addListener('spiderfy', function (markers) {
        isClickOnSpiderfier = true;
    });
}

function createListLGBTMarkers() {
    var icon = {
        url: "../Content/Icon/LGBT.png",
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
            optimized: false,
            title: "Nhấp để xem chi tiết",
            icon: icon
        });
        listLGBTMarkers.push(marker);

        // Allow each marker to have an info window
        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                setTimeout(function () {
                    getUserInfo(LGBT[i].userID);
                }, 100);
            }
        })(marker, i));

        overlappingLGBT.addMarker(marker);
    }
    
    overlappingLGBT.addListener('click', function (marker) {
        isClickOnSpiderfier = false;
    });
    overlappingLGBT.addListener('spiderfy', function (markers) {
        isClickOnSpiderfier = true;
    });
}

function createlistType3Markers() {
    var length = postType3.length;
    var icon = {
        url: "../Content/Icon/home.png",
        origin: new google.maps.Point(0, 0),
        anchor: new google.maps.Point(17, 34),
        scaledSize: new google.maps.Size(50, 50)
    };
    for (var i = 0; i < length; i++) {
        var position = new google.maps.LatLng(postType3[i].x, postType3[i].y);
        bounds.extend(position);
        marker = new google.maps.Marker({
            position: position,
            map: null,
            optimized: false,
            title: "Nhấp để xem chi tiết",
            icon: icon
        });
        listType3Markers.push(marker);

        // Allow each marker to have an info window
        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                setTimeout(function () {
                    getPostInfo(postType3[i].postID);
                }, 100);
            }
        })(marker, i));

        overlappingType3.addMarker(marker);
    }
    
    overlappingType3.addListener('click', function (marker) {
        isClickOnSpiderfier = false;
    });
    overlappingType3.addListener('spiderfy', function (markers) {
        isClickOnSpiderfier = true;
    });

}

function createlistType4Markers() {
    var icon = {
        url: "../Content/Icon/job.png",
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
            optimized: false,
            title: "Nhấp để xem chi tiết",
            icon: icon
        });
        listType4Markers.push(marker);

        // Allow each marker to have an info window
        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                setTimeout(function () {
                    getPostInfo(postType4[i].postID);
                }, 100);
            }
        })(marker, i));

        overlappingType4.addMarker(marker);
    }
    +
    overlappingType4.addListener('click', function (marker) {
        isClickOnSpiderfier = false;
    });
    overlappingType4.addListener('spiderfy', function (markers) {
        isClickOnSpiderfier = true;
    });

    //  alert(listType4Markers[0].getTitle());

}

function createlistType5Markers() {
    var icon = {
        url: "../Content/Icon/free.png",
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
            optimized: false,
            title: "Nhấp để xem chi tiết",
            icon: icon
        });
        listType5Markers.push(marker);

        // Allow each marker to have an info window
        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                setTimeout(function () {
                    getPostInfo(postType5[i].postID);
                }, 100);
            }
        })(marker, i));

        overlappingType5.addMarker(marker);
    }
    +
    overlappingType5.addListener('click', function (marker) {
        isClickOnSpiderfier = false;
    });
    overlappingType5.addListener('spiderfy', function (markers) {
        isClickOnSpiderfier = true;
    });


}

function createListType6Markers() {
    var icon = {
        url: "../Content/Icon/ship.jpg",
        origin: new google.maps.Point(0, 0),
        anchor: new google.maps.Point(17, 34),
        scaledSize: new google.maps.Size(50, 50)
    };
    var length = postType6.length;
    for (var i = 0; i < length; i++) {
        var position = new google.maps.LatLng(postType6[i].x, postType6[i].y);
        bounds.extend(position);
        marker = new google.maps.Marker({
            position: position,
            map: null,
            optimized: false,
            title: "Nhấp để xem chi tiết",
            icon: icon
        });
        listType6Markers.push(marker);

        // Allow each marker to have an info window
        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                setTimeout(function () {
                    getPostInfo(postType6[i].postID);
                }, 100);
            }
        })(marker, i));

        overlappingType6.addMarker(marker);
    }
    +
    overlappingType6.addListener('click', function (marker) {
        isClickOnSpiderfier = false;
    });
    overlappingType6.addListener('spiderfy', function (markers) {
        isClickOnSpiderfier = true;
    });


}

function createListType7Markers() {
    var icon = {
        url: "../Content/Icon/sale.png",
        origin: new google.maps.Point(0, 0),
        anchor: new google.maps.Point(17, 34),
        scaledSize: new google.maps.Size(50, 50)
    };
    var length = postType7.length;
    for (var i = 0; i < length; i++) {
        var position = new google.maps.LatLng(postType7[i].x, postType7[i].y);
        bounds.extend(position);
        marker = new google.maps.Marker({
            position: position,
            map: null,
            optimized: false,
            icon: icon
        });
        listType7Markers.push(marker);

        // Allow each marker to have an info window
        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                setTimeout(function () {
                    getPostInfo(postType7[i].postID);
                }, 100);
            }
        })(marker, i));

        overlappingType7.addMarker(marker);
    }
    +
    overlappingType7.addListener('click', function (marker) {
        isClickOnSpiderfier = false;
    });
    overlappingType7.addListener('spiderfy', function (markers) {
        isClickOnSpiderfier = true;
    });


}

function createListType8Markers() {
    var icon = {
        url: "../Content/Icon/help.png",
        origin: new google.maps.Point(0, 0),
        anchor: new google.maps.Point(17, 34),
        scaledSize: new google.maps.Size(50, 50)
    };
    var length = postType8.length;
    for (var i = 0; i < length; i++) {
        var position = new google.maps.LatLng(postType8[i].x, postType8[i].y);
        bounds.extend(position);
        marker = new google.maps.Marker({
            position: position,
            map: null,
            optimized: false,
            icon: icon
        });
        listType8Markers.push(marker);

        // Allow each marker to have an info window
        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                setTimeout(function () {
                    getPostInfo(postType8[i].postID);
                }, 100);
            }
        })(marker, i));

        overlappingType8.addMarker(marker);
    }
    +
    overlappingType8.addListener('click', function (marker) {
        isClickOnSpiderfier = false;
    });
    overlappingType8.addListener('spiderfy', function (markers) {
        isClickOnSpiderfier = true;
    });

}

function createListType9Markers() {
    var icon = {
        url: "../Content/Icon/Warning.png",
        origin: new google.maps.Point(0, 0),
        anchor: new google.maps.Point(17, 34),
        scaledSize: new google.maps.Size(50, 50)
    };
    var length = postType9.length;
    for (var i = 0; i < length; i++) {
        var position = new google.maps.LatLng(postType9[i].x, postType9[i].y);
        bounds.extend(position);
        marker = new google.maps.Marker({
            position: position,
            map: null,
            optimized: false,
            //title: postType1[i].address,
            icon: icon
        });
        listType9Markers.push(marker);

        // Allow each marker to have an info window
        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                setTimeout(function () {
                    getPostInfo(postType9[i].postID);
                }, 100);
            }
        })(marker, i));

        overlappingType9.addMarker(marker);
    }
    +
    overlappingType9.addListener('click', function (marker) {
        isClickOnSpiderfier = false;
    });
    overlappingType9.addListener('spiderfy', function (markers) {
        isClickOnSpiderfier = true;
    });

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
    if (isClickOnSpiderfier == false) {
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
}

function getPostInfo(postID) {
    if (isClickOnSpiderfier == false) {
        $.ajax({
            url: '/Map/GetPostPartialView?postId=' + postID,
            type: 'GET',
            dataType: 'text',
            success: function (result) {
                //   createUserInfoWindowContent(json.UserName, 23, json.Gender, json.Location.Address);
                if (result != '') {

                    $("#postModal").empty();

                    $("#forModal").html(result);

                    $('#forModal').modal({
                        inverted: true
                    }).modal({
                        duration: 400,
                        onHide: function () {
                            history.back();
                        }
                    }).modal('show')
                    ;
                    var $carousel = $('.carousel').flickity({
                        imagesLoaded: true,
                        percentPosition: false
                    });

                    // get transform property
                    var docStyle = document.documentElement.style;
                    var transformProp = typeof docStyle.transform == 'string' ?
                      'transform' : 'WebkitTransform';

                    history.pushState(null, null, "../Newsfeed/ShowPost/" + postID);
                    var flkty = $carousel.data('flickity');
                    var $imgs = $('.carousel-cell img');
                    $carousel.on('scroll.flickity', function () {
                        flkty.slides.forEach(function (slide, i) {
                            var img = $imgs[i];
                            var x = (slide.target + flkty.x) * -1 / 3;
                            img.style[transformProp] = 'translateX(' + x + 'px)';
                        });
                    });
                }
                window.addEventListener('popstate', function (e) {
                });
            },
            error: function (xhr, status, error) {
                alert(xhr.responseText);
            }
        });
    }
}

function showSelectedPostOnMap(Lat, Lng, PostType, PostId, isCallFromPostDetail) {
    isClickOnSpiderfier = false;
    switch (PostType) {
        case 3: showAccommodation(); break;
        case 4: showJobOffer(); break;
        case 5: showFurnitureOffer(); break;
        case 6: showHandGoodsOffer(); break;
        case 7: showTradeOffer(); break;
        case 8: showSOS(); break;
        case 9: showWarning(); break;
    }

    if (isCallFromPostDetail != 1) {
        setTimeout(function () {
            var position = new google.maps.LatLng(Lat, Lng);
            smoothlyCenterPosition(position);
            getPostInfo(PostId);

        }, 1000);
    } else {
        map.setZoom(14);
        map.setCenter({ lat: Lat, lng: Lng });
    }


}


function smoothlyCenterPosition(pos) {
    if (bounds) {
        var sw = bounds.getSouthWest();
        var ne = bounds.getNorthEast();

        var lat1 = sw.lat();
        var lng1 = sw.lng();
        var lat2 = ne.lat();
        var lng2 = ne.lng();

        var dx = (lng1 - lng2) / 2.;
        var dy = (lat1 - lat2) / 2.;
        var cx = (lng1 + lng2) / 2.;
        var cy = (lat1 + lat2) / 2.;

        // work around a bug in google maps...///
        lng1 = cx + dx / 1.5;
        lng2 = cx - dx / 1.5;
        lat1 = cy + dy / 1.5;
        lat2 = cy - dy / 1.5;
        /////////////////////////////////////////

        sw = new google.maps.LatLng(lat1, lng1);
        ne = new google.maps.LatLng(lat2, lng2);
        bounds = new google.maps.LatLngBounds(sw, ne);
        map.panTo(pos);
        map.fitBounds(bounds);
        map.setCenter(pos);
        setTimeout(function () {
            smoothZoom(this.map, 13, map.getZoom());

        }, 200);
    }
}

function checkIfBoundContainPosition(pos) {
    if (map.getBounds().contains(pos) == false) {
        bounds.extend(pos);
        map.fitBounds(bounds);
        map.setCenter(pos);
        setTimeout(function () {
            smoothZoom(this.map, 13, map.getZoom());
        }, 500);
    }
    else {
        //   map.fitBounds(map.getBounds());
        smoothlyCenterPosition(pos);
    }
}
function hideModel() {
    $("#userModal").modal('hide');        
}
//window.onload = initialize;
google.maps.event.addDomListener(window, 'load', initialize);
//$(document).ready(initialize);