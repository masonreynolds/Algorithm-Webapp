
function displayLocations(locations)  {
    const par = document.getElementById('locations');
    par.innerHTML = "";

    locations.forEach(function(location) {
        par.innerHTML += " - " + location.name + ": " + location.lat + ", " + location.lon + "\n";
    });
}