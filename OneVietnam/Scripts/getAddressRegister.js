if (navigator.geolocation) {
    navigator.geolocation.getCurrentPosition(function (position) {
        var pos = {
            lat: position.coords.latitude,
            lng: position.coords.longitude
        };
        //  alert(pos.lat+'aa'+pos.lng);

        var geocoder = new google.maps.Geocoder();             // create a geocoder object
        var location = new google.maps.LatLng(pos.lat, pos.lng);    // turn coordinates into an object          
        geocoder.geocode({ 'latLng': location }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {           // if geocode success
                var detailedLocation = results[0].formatted_address;         // if address found, pass to processing function
                document.getElementById("detailedLocation").innerText = detailedLocation;
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