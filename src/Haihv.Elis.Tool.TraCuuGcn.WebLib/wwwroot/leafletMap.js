window.leafletMapInterop = {
    map: null,  // Global variable to hold the map object
    centerDefault: [21.178, 106.070],
    zoomDefault: 16,
    // Biến để lưu trữ các lớp GeoJSON
    thuaDatsLayer: null,
    netNhasLayer: null,
    longDuongsLayer: null,
    addOverlayMaps: true,
    initializeMap: function () {
        // Khởi tạo bản đồ với trung tâm là Bắc Ninh
        this.map = L.map('map').setView(this.centerDefault, this.zoomDefault);  // Tọa độ trung tâm Bắc Ninh, zoom level 16

        // Định nghĩa lớp bản đồ đường phố sử dụng Google Maps
        var streets = L.tileLayer('https://{s}.google.com/vt/lyrs=m&x={x}&y={y}&z={z}', {
            maxZoom: 22,
            subdomains: ['mt0', 'mt1', 'mt2', 'mt3'],
            attribution: '&copy; Google Maps'
        });

        // Định nghĩa lớp ảnh vệ tinh sử dụng Google Maps
        var satellite = L.tileLayer('https://{s}.google.com/vt/lyrs=s&x={x}&y={y}&z={z}', {
            maxZoom: 22,
            subdomains: ['mt0', 'mt1', 'mt2', 'mt3'],
            attribution: '&copy; Google Maps'
        });

        // Thêm lớp bản đồ đường phố vào bản đồ mặc định
        streets.addTo(this.map);

        // Tạo điều khiển lớp và thêm vào bản đồ
        var baseMaps = {
            "Bản đồ đường phố": streets,
            "Ảnh vệ tinh": satellite
        };


        // Tạo và lưu trữ điều khiển lớp để có thể cập nhật sau
        this.layerControl = L.control.layers(baseMaps, null).addTo(this.map);

        // Thêm đánh dấu cho trung tâm thành phố Bắc Ninh
        // L.marker([21.1861, 106.0763]).addTo(map)
        //     .bindPopup('Thành phố Bắc Ninh')
        //     .openPopup();
    },
    updateMap: function (geoJsonData) {
        if (!this.map) {
            console.error("Map chưa được khởi tạo");
            return;
        }

        console.log("Dữ liệu GeoJSON gốc:", geoJsonData);

        try {
            let parsedData = typeof geoJsonData === 'string' ? JSON.parse(geoJsonData) : geoJsonData;

            // Các hằng số và tham số được định nghĩa trước để tối ưu hiệu suất
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

            function convertVN2000ToWGS84(x, y) {
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
            }

            // Hàm chuyển đổi geometry
            function convertGeometry(geometry) {
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
                            geometry.coordinates = geometry.coordinates.map(coord =>
                                convertVN2000ToWGS84(coord[0], coord[1])
                            );
                            break;
                        case "Polygon":
                        case "MultiLineString":
                            geometry.coordinates = geometry.coordinates.map(ring =>
                                ring.map(coord => convertVN2000ToWGS84(coord[0], coord[1]))
                            );
                            break;
                        case "MultiPolygon":
                            geometry.coordinates = geometry.coordinates.map(polygon =>
                                polygon.map(ring =>
                                    ring.map(coord => convertVN2000ToWGS84(coord[0], coord[1]))
                                )
                            );
                            break;
                        case "GeometryCollection":
                            geometry.geometries = geometry.geometries.map(convertGeometry);
                            break;
                        default:
                            console.error("Loại geometry không được hỗ trợ:", geometry.type);
                    }
                } catch (error) {
                    console.error("Lỗi khi chuyển đổi geometry:", error);
                }
                return geometry;
            }

            // Chuyển đổi tọa độ trong GeoJSON cho từng lớp
            function processLayer(layerData) {
                if (Array.isArray(layerData.features)) {
                    layerData.features.forEach(feature => {
                        if (feature.geometry) {
                            feature.geometry = convertGeometry(feature.geometry);
                        } else {
                            console.error("Feature không có geometry:", feature);
                        }
                    });
                } else {
                    console.error("Dữ liệu GeoJSON không hợp lệ: Không tìm thấy mảng features");
                }
                return layerData;
            }

            // Tách riêng từng FeatureCollection
            const thuaDats = processLayer(parsedData.thuaDats);
            const netNhas = processLayer(parsedData.netNhas);
            const longDuongs = processLayer(parsedData.longDuongs);

            // Xóa các lớp GeoJSON cũ nếu có
            if (this.thuaDatsLayer) {
                this.map.removeLayer(this.thuaDatsLayer);
                this.layerControl.removeLayer(this.thuaDatsLayer); // Xóa lớp khỏi Layer Control
            }
            if (this.netNhasLayer) {
                this.map.removeLayer(this.netNhasLayer);
                this.layerControl.removeLayer(this.netNhasLayer); // Xóa lớp khỏi Layer Control
            }
            if (this.longDuongsLayer) {
                this.map.removeLayer(this.longDuongsLayer);
                this.layerControl.removeLayer(this.longDuongsLayer); // Xóa lớp khỏi Layer Control
            }

            // Tạo các lớp GeoJSON mới
            this.thuaDatsLayer = L.geoJSON(thuaDats, {
                style: function () {
                    return { color: 'red', weight: 1, fillOpacity: 0.1 };
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

            this.netNhasLayer = L.geoJSON(netNhas, {
                style: function () {
                    return { color: 'blue', weight: 2, dashArray: '5,5' };
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

            this.longDuongsLayer = L.geoJSON(longDuongs, {
                style: function () {
                    return { color: 'orange', weight: 2, dashArray: '5,5' };
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
            // Đầu tiên, xóa toàn bộ các lớp overlay hiện tại
            this.map.eachLayer(layer => {
                if (layer.options && layer.options.pane === "overlayPane") {
                    this.map.removeLayer(layer);
                }
            });


            // Cập nhật điều khiển lớp

            // Thêm các lớp overlay vào Layer Control
            this.layerControl.addOverlay(this.thuaDatsLayer, "Thửa đất");
            this.layerControl.addOverlay(this.netNhasLayer, "Nét nhà");
            this.layerControl.addOverlay(this.longDuongsLayer, "Lòng đường");

            // Thêm các lớp vào bản đồ để luôn được hiển thị
            this.thuaDatsLayer.addTo(this.map);
            this.netNhasLayer.addTo(this.map);
            this.longDuongsLayer.addTo(this.map);


            // Điều chỉnh view của bản đồ dựa trên bounds của tất cả các lớp
            var group = L.featureGroup([this.thuaDatsLayer, this.netNhasLayer, this.longDuongsLayer]);
            var bounds = group.getBounds();
            if (bounds.isValid()) {
                this.map.fitBounds(bounds);
            } else {
                console.warn("Bounds không hợp lệ, sử dụng view mặc định");
                this.map.setView(this.centerDefault, this.zoomDefault); // Mặc định về Bắc Ninh
            }
        } catch (error) {
            console.error("Lỗi khi xử lý hoặc hiển thị GeoJSON:", error);
        }
    },

    shareToGoogleMaps: function() {
        if (!this.map) {
            console.error("Map chưa được khởi tạo");
            return;
        }

        var center = this.map.getCenter();
        var zoom = this.map.getZoom();
        var url = `https://www.google.com/maps/search/?api=1&query=${center.lat},${center.lng}&zoom=${zoom}`;
        window.open(url, '_blank');
    },
    
    disposeMap: function() {
        if (this.map) {
            this.map.remove();
            this.map = null;
        }
    }
};