function initialize2() {
    // Create the search box and link it to the UI element.
    var input = document.getElementsByClassName("input-location");
    var searchBox = new google.maps.places.SearchBox(input);
}

google.maps.event.addDomListener(window, 'load', initialize2);



