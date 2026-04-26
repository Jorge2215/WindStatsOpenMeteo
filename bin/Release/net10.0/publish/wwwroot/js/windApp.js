// Geolocation interop
window.getGeolocation = function () {
    return new Promise(function (resolve, reject) {
        if (!navigator.geolocation) {
            reject('Geolocation is not supported by this browser.');
            return;
        }
        navigator.geolocation.getCurrentPosition(
            function (position) {
                resolve({
                    latitude: position.coords.latitude,
                    longitude: position.coords.longitude,
                    accuracy: position.coords.accuracy
                });
            },
            function (error) {
                var messages = {
                    1: 'Location permission denied. Please search for a city below.',
                    2: 'Position unavailable. Please search for a city below.',
                    3: 'Location request timed out. Please search for a city below.'
                };
                reject(messages[error.code] || error.message);
            },
            { enableHighAccuracy: true, timeout: 10000, maximumAge: 60000 }
        );
    });
};

// Map interop (Leaflet + OpenStreetMap)
window.windMapInterop = (function () {
    var map = null;
    var coverageCircle = null;
    var locationMarker = null;

    function speedColor(kmh) {
        if (kmh < 20) return '#28a745';
        if (kmh < 50) return '#fd7e14';
        return '#dc3545';
    }

    return {
        initMap: function (elementId, lat, lng, radiusKm, direction, speed) {
            if (typeof L === 'undefined') {
                console.error('Leaflet (L) is not loaded. Cannot initialise map.');
                return;
            }

            if (map) {
                map.remove();
                map = null;
            }

            map = L.map(elementId, { zoomControl: true }).setView([lat, lng], 9);

            L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
                attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
                maxZoom: 18
            }).addTo(map);

            // 40 km coverage circle
            coverageCircle = L.circle([lat, lng], {
                color: '#0d6efd',
                fillColor: '#0d6efd',
                fillOpacity: 0.08,
                radius: radiusKm * 1000,
                weight: 2,
                dashArray: '6 4'
            }).addTo(map);

            // Wind direction arrow marker
            var arrowHtml = '<div style="transform:rotate(' + (direction || 0) + 'deg);' +
                'font-size:28px;line-height:1;color:' + speedColor(speed || 0) + ';' +
                'text-shadow:0 0 3px #fff;filter:drop-shadow(0 1px 2px rgba(0,0,0,.4));">&#8679;</div>';

            var windIcon = L.divIcon({
                html: arrowHtml,
                className: '',
                iconSize: [32, 32],
                iconAnchor: [16, 16]
            });

            locationMarker = L.marker([lat, lng], { icon: windIcon })
                .addTo(map)
                .bindPopup('<b>Your Location</b><br>' +
                    lat.toFixed(4) + '&deg;, ' + lng.toFixed(4) + '&deg;<br>' +
                    'Wind: ' + (speed || 0).toFixed(1) + ' km/h &bull; ' + (direction || 0).toFixed(0) + '&deg;')
                .openPopup();

            // Scale control
            L.control.scale({ imperial: false }).addTo(map);
        },

        updateWindArrow: function (lat, lng, direction, speed) {
            if (!map) return;

            var arrowHtml = '<div style="transform:rotate(' + direction + 'deg);' +
                'font-size:28px;line-height:1;color:' + speedColor(speed) + ';' +
                'text-shadow:0 0 3px #fff;filter:drop-shadow(0 1px 2px rgba(0,0,0,.4));">&#8679;</div>';

            var windIcon = L.divIcon({
                html: arrowHtml,
                className: '',
                iconSize: [32, 32],
                iconAnchor: [16, 16]
            });

            if (locationMarker) {
                locationMarker.setIcon(windIcon);
                locationMarker.setPopupContent('<b>Your Location</b><br>' +
                    lat.toFixed(4) + '&deg;, ' + lng.toFixed(4) + '&deg;<br>' +
                    'Wind: ' + speed.toFixed(1) + ' km/h &bull; ' + direction.toFixed(0) + '&deg;');
            }
        }
    };
})();
