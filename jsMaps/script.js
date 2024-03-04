// Define latitude, longitude and zoom level
const latitude = 55.750784;
const longitude = 37.622558;
const zoom = 15;
// Set DIV element to embed map
var mymap = L.map('mapWrap');

//popup window
var mmr = L.marker([55.740, 37.185]);
mmr.bindPopup('55.740, 37.185');

class marker{
	constructor(x, y, color = 'blue', title = '<dl><dt>'+x+'</dt><dt>'+y+'</dt></dl>'){
		this.x = x;
		this.y = y;
		this.color = color;
		this.title = title;
		var markerIcon = new L.Icon({
			iconUrl: 'img/marker-icon-2x-'+this.color+'.png',
			shadowUrl: 'img/marker-shadow.png',
			iconSize: [25, 41],
			iconAnchor: [12, 41],
			popupAnchor: [1, -34],
			shadowSize: [41, 41]
		});
		var marker = L.marker([this.x, this.y], {icon: markerIcon});
		marker.bindPopup(this.title);
		marker.addTo(mymap);
	}
	getX(){
		return this.x;
	}
	getY(){
		return this.y;
	}
}
class circle{
	constructor(circleCenter, radius, color='red', offset = 0.56, pointCount = 200){
		this.circleCenter = circleCenter;
		this.lst = [];
		this.color = color;
		this.offset = offset;
		this.pointCount = pointCount;
		this.radius = radius/62500;
		for (var i = 0;i<=Math.PI*2;i+=Math.PI*2/this.pointCount){
			var x = Math.cos(i);
			var y = Math.sin(i);
			this.lst.push([this.circleCenter[0]+x*this.radius*offset, this.circleCenter[1]+y*this.radius]);
		};
		addPoly(this.lst, this.color);
	}
}
class poly{
	constructor(latlngs, clr='red'){
		this.color = color;
		this.latlng = latlng;
		var polygon = L.polygon(this.latlngs, {color: this.color});
		polygon.addTo(mymap);
	}
}
class rectangle{
	constructor(leftX, leftY, rightX, rightY, clr = 'red'){
		this.color = clr;
		this.latlngs = [[leftX, leftY], [rightX, rightY]];
		this.rectOptions = {color: this.color, weight: 1}
		var rectangle = L.rectangle(this.latlngs, this.rectOptions);
		rectangle.addTo(mymap);
	}
}
//color = blue, gold, red, green, orange, yellow, violet, grey, black
function addMarker(x, y, color = 'blue', title = '<dl><dt>'+x+'</dt><dt>'+y+'</dt></dl>'){
	var markerIcon = new L.Icon({
		iconUrl: 'img/marker-icon-2x-'+color+'.png',
		shadowUrl: 'img/marker-shadow.png',
		iconSize: [25, 41],
		iconAnchor: [12, 41],
		popupAnchor: [1, -34],
		shadowSize: [41, 41]
	});
	var marker = L.marker([x, y], {icon: markerIcon});
	marker.bindPopup(title);
	marker.addTo(mymap);
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
function addEditableCircle(circleCenter, radius, offset = 0.56, pointCount = 150){
	var lst = [];
	radius = radius/62500;
	for (var i = 0;i<=Math.PI*2;i+=Math.PI*2/pointCount){
		var x = Math.cos(i);
		var y = Math.sin(i);
		lst.push([circleCenter[0]+x*radius*offset, circleCenter[1]+y*radius]);
	};
	addPoly(lst);
}
function addPoly(latlngs, clr='red'){
	var polygon = L.polygon(latlngs, {color: clr, fillOpacity: .07});
	polygon.addTo(mymap);
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
circleRad = 888;
L.tileLayer('http://localhost:8080/styles/basic-preview/{z}/{x}/{y}.png?{foo}', {
	foo: 'bar',
	attribution:'&copy; <a>OpenStreetMap</a>'}
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