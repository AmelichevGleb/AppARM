<html>
<head>
	<link rel="stylesheet" href="leaflet/leaflet.css"/>
	<script src="leaflet/leaflet.js"></script>
	<link rel="stylesheet" href="style.css">
</head>
<body>
	<div id="mapWrap"></div>
	<?php
	$host = 'localhost';
	$port = 5432;
	$user = 'postgres';
	$pass = '11223344';
	$db = 'DataBase';
	$conn = pg_connect("host=localhost port=5432 dbname=DataBase user=postgres password=11223344");
	if ($conn === false) {
		echo '<script>alert("Connect error");</script>';
		exit;
	}
	echo "
	<script>
		// Define latitude, longitude and zoom level
		const latitude = 55.738172;
		const longitude = 37.185;
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
			constructor(circleCenter, radius, offset = 0.56, pointCount = 200){
				this.circleCenter = circleCenter;
				this.lst = [];
				this.offset = offset;
				this.pointCount = pointCount;
				this.radius = radius/62500;
				for (var i = 0;i<=Math.PI*2;i+=Math.PI*2/this.pointCount){
					var x = Math.cos(i);
					var y = Math.sin(i);
					this.lst.push([this.circleCenter[0]+x*this.radius*offset, this.circleCenter[1]+y*this.radius]);
				};
				addPoly(this.lst);
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
			console.log(latlngs);
			var polygon = L.polygon(latlngs, {color: clr});
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
		//alert(insideCircle(distance(55.738172, 37.185, 55.738172, 37.199), circleRad));
		


		//addRectangle(55.738172, 37.185, 55.739, 37.19);

		// Add copyright attribution
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
		
	";
	$q = pg_query($conn, 'SELECT * FROM "Station";');
	echo 'markersArray = [];';
	while ($row = pg_fetch_row($q)) {
		echo 'm = new marker('.$row[3].', '.$row[4].', "blue", "<dl><dt><b>Name: </b>'.$row[1].'</dt><dt> <b>X: </b>'.$row[3].'</dt><dt><b>Y: </b>'.$row[4].'</dt><dt><b>Temperature: </b>'.$row[5].'</dt><dt><b>Wind Speed: </b>'.$row[6].'</dt><dt><b>Direction: </b>'.$row[7].'</dt></dl>");';
		echo 'markersArray.push(m);';
	}
	echo 'm2 = new marker(55.740572, 37.185, "green");';
	echo 'addCircle(55.738172, 37.185, 500);';
	echo 'c = new circle([55.738172, 37.185], 400);';
	echo '</script>';
	pg_close($conn);
?>
</body>
</html>