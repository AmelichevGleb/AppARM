// Define latitude, longitude and zoom level
const latitude = 55.738172;
const longitude = 37.185;
const zoom = 14;
// Set DIV element to embed map
var mymap = L.map('mapWrap');

//popup window
var mmr = L.marker([55.740, 37.185]);
mmr.bindPopup('55.740, 37.185');


//color = blue, gold, red, green, orange, yellow, violet, grey, black
function addMarker(x, y, color = "blue"){
	var markerIcon = new L.Icon({
	iconUrl: 'img/marker-icon-2x-'+color+'.png',
	shadowUrl: 'img/marker-shadow.png',
	iconSize: [25, 41],
	iconAnchor: [12, 41],
	popupAnchor: [1, -34],
	shadowSize: [41, 41]
	});
	L.marker([x, y], {icon: markerIcon}).addTo(mymap);
}
function addCircle(centerX, centerY, radius, clr = 'red'){
	var circleCenter = [centerX, centerY];
	var circleOptions = {
	   color: clr,
	   fillColor: '#f03',
	   fillOpacity: 0
	}
	var circle = L.circle(circleCenter, radius, circleOptions);
	circle.addTo(mymap);
}
function addRectangle(leftX, leftY, rightX, rightY, clr = 'red'){
	var latlngs = [[leftX, leftY], [rightX, rightY]];
	var rectOptions = {color: clr, weight: 1}
	var rectangle = L.rectangle(latlngs, rectOptions);
	rectangle.addTo(mymap);
}
function distance(x1, y1, x2, y2){
	return  Math.round(Math.sqrt((x2-x1)**2+(y2-y1)**2)*1000000)/1000*63;
}
function insideCircle(dis, circleRad){
	if (dis<=circleRad)
		return true;
	else return false;
}
//test
addMarker(55.738172, 37.185, 'green');
addMarker(55.738172, 37.199, 'blue');
circleRad = 888;
//alert(insideCircle(distance(55.738172, 37.185, 55.738172, 37.199), circleRad));



addCircle(55.738172, 37.185, circleRad);
//addRectangle(55.738172, 37.185, 55.739, 37.19);
//marker L.marker([55.739, 37.186]).addTo(mymap);

// Add copyright attribution
L.tileLayer('http://localhost:8080/styles/basic-preview/{z}/{x}/{y}.png?{foo}', {
    foo: 'bar',
    attribution:'&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a>'}
).addTo(mymap);

// Set lat lng position and zoom level of map 
mmr.setLatLng(L.latLng(latitude, longitude));
mymap.setView([latitude, longitude], zoom);

// Set popup window content
mmr.setPopupContent('Latitude: '+latitude+' <br /> Longitude: '+longitude).openPopup();

// Set marker onclick event
mmr.on('click', openPopupWindow);

// Marker click event handler
function openPopupWindow(e) {
    mmr.setPopupContent('Latitude: '+e.latlng.lat+' <br /> Longitude: '+e.latlng.lng).openPopup();
}