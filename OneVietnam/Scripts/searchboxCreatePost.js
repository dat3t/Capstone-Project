function getAddressFromSearchBox() {
    // Create the search box and link it to the UI element.
    var input = document.getElementById("search-location");
    var searchBox = new google.maps.places.SearchBox(input);

    searchBox.addListener('places_changed', function () {
        var places = searchBox.getPlaces();
        if (places.length == 0) {
            return;
        }

        // For each place, get the icon, name and location.
        places.forEach(function (place) {
            document.getElementById("address").innerText = place.name;
            document.getElementById("Xcoordinate").innerText = place.geometry.location.lat();
            document.getElementById("Xcoordinate").innerText = place.geometry.location.lng();
        });
    });
}

function getCurrentLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var pos = {
                lat: position.coords.latitude,
                lng: position.coords.longitude
            };

            var geocoder = new window.google.maps.Geocoder();             // create a geocoder object
            var location = new window.google.maps.LatLng(pos.lat, pos.lng);    // turn coordinates into an object          
            geocoder.geocode({ 'latLng': location }, function (results, status) {
                if (status === window.google.maps.GeocoderStatus.OK) {           // if geocode success
                    var detailedLocation = results[0].formatted_address;         // if address found, pass to processing function
                    document.getElementById("address").innerText = detailedLocation;
                    document.getElementById("Xcoordinate").innerText = pos.lat;
                    document.getElementById("Xcoordinate").innerText = pos.lng;
                } else {
                }
            })
        }, function () {
            // handleLocationError(true, myLocationMarker, map.getCenter());
            alert("Không thể định vị được vị trí của bạn. Bạn cần cho phép trình duyệt sử dụng định vị GPS.");
        });
    } else {
        // Browser doesn't support Geolocation
        //  handleLocationError(false, myLocationMarker, map.getCenter());
        alert("Trình duyệt của bạn không hỗ trợ định vị GPS. Vui lòng nâng cấp phiên bản mới nhất của trình duyệt và thử lại sau.");
    }
}

google.maps.event.addDomListener(window, 'load', getAddressFromSearchBox);



