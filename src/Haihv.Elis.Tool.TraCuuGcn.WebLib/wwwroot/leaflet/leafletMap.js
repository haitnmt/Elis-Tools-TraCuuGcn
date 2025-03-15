window.leafletMapInterop = {
    map: null,
    centerDefault: [21.178, 106.070],
    zoomDefault: 16,
    layers: {
        thuaDats: null,
        netNhas: null,
        longDuongs: null,
        tamThuaDats: null
    },

    // Khởi tạo map
    initializeMap: function (mapContainerId) {
        this.map = L.map(mapContainerId).setView(this.centerDefault, this.zoomDefault);

        const streets = L.tileLayer('https://{s}.google.com/vt/lyrs=m&x={x}&y={y}&z={z}', {
            maxZoom: 22,
            subdomains: ['mt0', 'mt1', 'mt2', 'mt3'],
            attribution: '&copy; Google Maps'
        });

        const satellite = L.tileLayer('https://{s}.google.com/vt/lyrs=s&x={x}&y={y}&z={z}', {
            maxZoom: 22,
            subdomains: ['mt0', 'mt1', 'mt2', 'mt3'],
            attribution: '&copy; Google Maps'
        });

        streets.addTo(this.map);

        const baseMaps = {
            "Bản đồ đường phố": streets,
            "Ảnh vệ tinh": satellite
        };

        this.layerControl = L.control.layers(baseMaps, null).addTo(this.map);
    },

    // Cập nhật dữ liệu lên map
    updateMap: function (geoJsonData) {
        if (!this.map) {
            console.error("Map chưa được khởi tạo");
            return;
        }
        if (!geoJsonData) {
            // Hiển thị vị trí mặc định khi không có dữ liệu:
            this.map.setView(this.centerDefault, this.zoomDefault);
        }
        try {
            const parsedData = typeof geoJsonData === 'string' ? JSON.parse(geoJsonData) : geoJsonData;

            const convertVN2000ToWGS84 = this.convertVN2000ToWGS84;

            const processLayer = (layerData) => {
                if (Array.isArray(layerData.features)) {
                    layerData.features.forEach(feature => {
                        if (feature.geometry) {
                            feature.geometry = this.convertGeometry(feature.geometry, convertVN2000ToWGS84);
                        } else {
                            console.error("Feature không có geometry:", feature);
                        }
                    });
                } else {
                    console.error("Dữ liệu GeoJSON không hợp lệ: Không tìm thấy mảng features");
                }
                return layerData;
            };

            const thuaDats = processLayer(parsedData.thuaDats);
            const netNhas = processLayer(parsedData.netNhas);
            const longDuongs = processLayer(parsedData.longDuongs);
            const tamThuaDats = processLayer(parsedData.tamThuaDats);

            this.removeOldLayers();

            this.layers.thuaDats = this.createGeoJsonLayer(thuaDats, 'red', 1, 0.1);
            this.layers.netNhas = this.createGeoJsonLayer(netNhas, 'blue', 2, null, '5,5');
            this.layers.longDuongs = this.createGeoJsonLayer(longDuongs, 'red', 5, null, '5,5');
            this.layers.tamThuaDats = this.createGeoJsonLayer(tamThuaDats, 'orange', 2, null, '15,15');

            this.addLayersToMap();

            if (tamThuaDats.features.length === 1) {
                const center = this.calculateCenter(tamThuaDats.features[0].geometry);
                if (center) {
                    this.map.setView(center, this.zoomDefault);
                } else {
                    console.warn("Không thể tính toán được center của tamThuaDats, sử dụng view mặc định");
                    this.map.setView(this.centerDefault, this.zoomDefault);
                }
            } else {
                const group = L.featureGroup([this.layers.thuaDats, this.layers.netNhas, this.layers.longDuongs, this.layers.tamThuaDats]);
                const bounds = group.getBounds();
                if (bounds.isValid()) {
                    this.map.fitBounds(bounds);
                } else {
                    console.warn("Bounds không hợp lệ, sử dụng view mặc định");
                    this.map.setView(this.centerDefault, this.zoomDefault);
                }
            }
        } catch (error) {
            console.error("Lỗi khi xử lý hoặc hiển thị GeoJSON:", error);
            // Hiển thị vị trí mặc định khi khởi tạo:
            this.map.setView(this.centerDefault, this.zoomDefault);
        }
    },
    // Tính toán tọa độ trung tâm của geometry
    calculateCenter: function (geometry) {
        let latSum = 0, lngSum = 0, count = 0;
        const addCoordinates = (coordinates) => {
            coordinates.forEach(coord => {
                latSum += coord[1];
                lngSum += coord[0];
                count++;
            });
        };

        switch (geometry.type) {
            case "Point":
                return [geometry.coordinates[1], geometry.coordinates[0]];
            case "MultiPoint":
            case "LineString":
                addCoordinates(geometry.coordinates);
                break;
            case "Polygon":
            case "MultiLineString":
                geometry.coordinates.forEach(ring => addCoordinates(ring));
                break;
            case "MultiPolygon":
                geometry.coordinates.forEach(polygon => polygon.forEach(ring => addCoordinates(ring)));
                break;
            default:
                console.error("Loại geometry không được hỗ trợ cho việc tính toán center:", geometry.type);
                return null;
        }

        return count > 0 ? [latSum / count, lngSum / count] : null;
    },
    
    // Chuyển đổi tọa độ VN2000 sang WGS84
    convertVN2000ToWGS84: function (x, y) {
        const a = 6378137.0;
        const f = 1 / 298.257223563;
        const e2 = 2 * f - f * f;
        const dx = -191.90441429;
        const dy = -39.30318279;
        const dz = -111.45032835;
        const rx = -0.00928836 / 3600 * Math.PI / 180;
        const ry = 0.01975479 / 3600 * Math.PI / 180;
        const rz = -0.00427372 / 3600 * Math.PI / 180;
        const s = 1 + 0.252906278 / 1e6;
        const lon0 = 105.5 * Math.PI / 180;
        const k0 = 0.9999;
        const E0 = 500000;
        const N0 = 0;
        const e = Math.sqrt(e2);
        const n = f / (2 - f);
        const A = a / (1 + n) * (1 + n * n / 4 + n * n * n * n / 64);

        const M = y - N0;
        const mu = M / (A * k0);

        const phi1 = mu + (3 * n / 2 - 27 * n * n * n / 32) * Math.sin(2 * mu)
            + (21 * n * n / 16 - 55 * n * n * n * n / 32) * Math.sin(4 * mu)
            + (151 * n * n * n / 96) * Math.sin(6 * mu);

        const C1 = e2 * Math.cos(phi1) * Math.cos(phi1) / (1 - e2);
        const T1 = Math.tan(phi1) * Math.tan(phi1);
        const N1 = a / Math.sqrt(1 - e2 * Math.sin(phi1) * Math.sin(phi1));
        const R1 = a * (1 - e2) / Math.pow(1 - e2 * Math.sin(phi1) * Math.sin(phi1), 1.5);
        const D = (x - E0) / (N1 * k0);

        const phi = phi1 - (N1 * Math.tan(phi1) / R1) * (D * D / 2 - (5 + 3 * T1 + 10 * C1 - 4 * C1 * C1 - 9 * e2) * D * D * D * D / 24
            + (61 + 90 * T1 + 298 * C1 + 45 * T1 * T1 - 252 * e2 - 3 * C1 * C1) * D * D * D * D * D * D / 720);
        const lambda = lon0 + (D - (1 + 2 * T1 + C1) * D * D * D / 6
            + (5 - 2 * C1 + 28 * T1 - 3 * C1 * C1 + 8 * e2 + 24 * T1 * T1) * D * D * D * D * D / 120) / Math.cos(phi1);

        const N = a / Math.sqrt(1 - e2 * Math.sin(phi) * Math.sin(phi));
        const X = (N + 0) * Math.cos(phi) * Math.cos(lambda);
        const Y_coord = (N + 0) * Math.cos(phi) * Math.sin(lambda);
        const Z = (N * (1 - e2) + 0) * Math.sin(phi);

        const X2 = dx + s * (X + rz * Y_coord - ry * Z);
        const Y2 = dy + s * (-rz * X + Y_coord + rx * Z);
        const Z2 = dz + s * (ry * X - rx * Y_coord + Z);

        const p = Math.sqrt(X2 * X2 + Y2 * Y2);
        const theta = Math.atan2(Z2 * a, p * (1 - f) * a);
        const lonWGS84 = Math.atan2(Y2, X2);
        const latWGS84 = Math.atan2(Z2 + e2 * (1 - f) / (1 - e2) * a * Math.pow(Math.sin(theta), 3),
            p - e2 * a * Math.pow(Math.cos(theta), 3));

        return [lonWGS84 * 180 / Math.PI, latWGS84 * 180 / Math.PI];
    },

    // Chuyển đổi tọa độ của geometry từ VN2000 sang WGS84
    convertGeometry: function (geometry, convertVN2000ToWGS84) {
        if (!geometry || !geometry.type) {
            console.error("Geometry không hợp lệ:", geometry);
            return geometry;
        }

        try {
            switch (geometry.type) {
                case "Point":
                    geometry.coordinates = convertVN2000ToWGS84(geometry.coordinates[0], geometry.coordinates[1]);
                    break;
                case "LineString":
                case "MultiPoint":
                    geometry.coordinates = geometry.coordinates.map(coord => convertVN2000ToWGS84(coord[0], coord[1]));
                    break;
                case "Polygon":
                case "MultiLineString":
                    geometry.coordinates = geometry.coordinates.map(ring => ring.map(coord => convertVN2000ToWGS84(coord[0], coord[1])));
                    break;
                case "MultiPolygon":
                    geometry.coordinates = geometry.coordinates.map(polygon => polygon.map(ring => ring.map(coord => convertVN2000ToWGS84(coord[0], coord[1]))));
                    break;
                case "GeometryCollection":
                    geometry.geometries = geometry.geometries.map(g => this.convertGeometry(g, convertVN2000ToWGS84));
                    break;
                default:
                    console.error("Loại geometry không được hỗ trợ:", geometry.type);
            }
        } catch (error) {
            console.error("Lỗi khi chuyển đổi geometry:", error);
        }
        return geometry;
    },

    // Xóa các layer cũ
    removeOldLayers: function () {
        for (const layerName in this.layers) {
            if (this.layers[layerName]) {
                this.map.removeLayer(this.layers[layerName]);
                this.layerControl.removeLayer(this.layers[layerName]);
            }
        }
    },

    // Tạo layer từ dữ liệu GeoJSON
    createGeoJsonLayer: function (data, color, weight, fillOpacity, dashArray) {
        return L.geoJSON(data, {
            style: function () {
                return { color: color, weight: weight, fillOpacity: fillOpacity, dashArray: dashArray };
            },
            onEachFeature: function (feature, layer) {
                if (feature.properties) {
                    const popupContent = Object.entries(feature.properties)
                        .map(([key, value]) => `<strong>${key}:</strong> ${value}`)
                        .join('<br>');
                    layer.bindPopup(popupContent);
                }
            }
        });
    },

    
    // Thêm các layer vào map
    addLayersToMap: function () {
        for (const layerName in this.layers) {
            if (this.layers[layerName]) {
                this.layers[layerName].addTo(this.map);
            }
        }
    },

    // Chia sẻ tọa độ trung tâm và zoom hiện tại lên Google Maps
    shareToGoogleMaps: function () {
        if (!this.map) {
            console.error("Map chưa được khởi tạo");
            return;
        }

        const center = this.map.getCenter();
        const zoom = this.map.getZoom();
        const url = `https://www.google.com/maps/search/?api=1&query=${center.lat},${center.lng}&zoom=${zoom}`;
        window.open(url, '_blank');
    },

    // Xóa map
    disposeMap: function () {
        if (this.map) {
            this.map.remove();
            this.map = null;
        }
    }
};