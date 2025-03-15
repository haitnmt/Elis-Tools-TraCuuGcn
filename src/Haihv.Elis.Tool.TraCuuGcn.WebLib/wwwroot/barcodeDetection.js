// barcodeDetection.js
window.BarcodeDetectionService = {
    isBarcodeDetectionSupported: function () {
        return 'BarcodeDetector' in window;
    },

    isCameraAvailable : async ()=> {
        try {
            const devices = await navigator.mediaDevices.enumerateDevices();
            return devices.some(device => device.kind === 'videoinput');
        } catch {
            return false;
        }
    },
    
    getSupportedFormats: async function () {
        if (!this.isBarcodeDetectionSupported()) {
            return [];
        }

        try {
            // Lấy các định dạng được hỗ trợ
            return await BarcodeDetector.getSupportedFormats();
        } catch (error) {
            console.error("Lỗi khi lấy các định dạng được hỗ trợ:", error);
            return [];
        }
    },

    detectFromImage: async function (imageDataUrl, formats) {
        if (!this.isBarcodeDetectionSupported()) {
            return { success: false, error: "Barcode Detection API không được hỗ trợ trên trình duyệt này." };
        }
    
        try {
            // Tạo một phần tử hình ảnh tạm thời để quét
            const imageElement = new Image();
            
            // Tạo một Promise để đảm bảo hình ảnh được tải xong
            const imageLoaded = new Promise((resolve, reject) => {
                imageElement.onload = resolve;
                imageElement.onerror = () => reject(new Error("Không thể tải hình ảnh"));
            });
            
            // Đặt src cho hình ảnh từ dữ liệu base64
            imageElement.src = imageDataUrl;
            
            // Đợi cho đến khi hình ảnh được tải
            await imageLoaded;
    
            // Tạo detector với các định dạng được chỉ định (nếu có)
            const barcodeDetector = formats && formats.length > 0
                ? new BarcodeDetector({ formats: formats })
                : new BarcodeDetector();
    
            // Thực hiện quét mã vạch
            const barcodes = await barcodeDetector.detect(imageElement);
    
            // Chuyển đổi kết quả thành định dạng phù hợp để trả về cho Blazor
            const results = barcodes.map(barcode => ({
                format: barcode.format,
                rawValue: barcode.rawValue,
                cornerPoints: barcode.cornerPoints,
                boundingBox: {
                    x: barcode.boundingBox.x,
                    y: barcode.boundingBox.y,
                    width: barcode.boundingBox.width,
                    height: barcode.boundingBox.height
                }
            }));
    
            return { success: true, results: results };
        } catch (error) {
            console.error("Lỗi khi quét mã vạch:", error);
            return { success: false, error: error.message };
        }
    },

    detectFromVideo: async (videoElementId, formats) => {
        if (!this.isBarcodeDetectionSupported()) {
            return { success: false, error: "Barcode Detection API không được hỗ trợ trên trình duyệt này." };
        }

        try {
            const videoElement = document.getElementById(videoElementId);
            if (!videoElement) {
                return { success: false, error: "Không tìm thấy phần tử video." };
            }
            
            // Check if video is ready and has valid content
            if (videoElement.readyState < 2) { // HAVE_CURRENT_DATA or higher
                return { success: false, error: "Video chưa sẵn sàng để quét. Vui lòng đợi video được tải." };
            }

            if (videoElement.videoWidth === 0 || videoElement.videoHeight === 0) {
                return { success: false, error: "Video không có nội dung hợp lệ để quét." };
            }
            
            // Tạo detector với các định dạng được chỉ định (nếu có)
            const barcodeDetector = formats && formats.length > 0
                ? new BarcodeDetector({ formats: formats })
                : new BarcodeDetector();

            // Thực hiện quét mã vạch
            const barcodes = await barcodeDetector.detect(videoElement);

            // Chuyển đổi kết quả thành định dạng phù hợp để trả về cho Blazor
            const results = barcodes.map(barcode => ({
                format: barcode.format,
                rawValue: barcode.rawValue,
                cornerPoints: barcode.cornerPoints,
                boundingBox: {
                    x: barcode.boundingBox.x,
                    y: barcode.boundingBox.y,
                    width: barcode.boundingBox.width,
                    height: barcode.boundingBox.height
                }
            }));

            return { success: true, results: results };
        } catch (error) {
            console.error("Lỗi khi quét mã vạch:", error);
            return { success: false, error: error.message };
        }
    },
    // Xử lý Video (bổ sung auto-focus và tăng độ phân giải stream)
    startVideo: async (videoElement) => {
        try {
            // 1. Đảm bảo cleanup resources cũ
            if (videoElement.srcObject) {
                const tracks = videoElement.srcObject.getTracks();
                tracks.forEach(track => track.stop());
            }
            // 2. Khởi tạo stream với cấu hình phù hợp
            const stream = await navigator.mediaDevices.getUserMedia({
                video: {
                    facingMode: 'environment',
                    width: { ideal: 1920 },
                    height: { ideal: 1080 }
                }
            });

            videoElement.srcObject = stream;

            // Wait for video to be properly initialized
            return new Promise((resolve) => {
                videoElement.onloadeddata = () => {
                    videoElement.play();
                    // Add a small delay to ensure video is playing
                    setTimeout(() => resolve(stream), 100);
                };
            });
        } catch (error) {
            throw new Error('Cannot access camera: ' + error.message);
        }
    },
    
    startLiveDetection: async function(dotNetHelper, resultCallbackName, videoElement, formats) {
        if (!this.isBarcodeDetectionSupported()) {
            return { success: false, error: "Barcode Detection API không được hỗ trợ trên trình duyệt này." };
        }
    
        try {
            await window.BarcodeDetectionService.startVideo(videoElement);
    
            // Tạo detector với các định dạng được chỉ định (nếu có)
            const barcodeDetector = formats && formats.length > 0
                ? new BarcodeDetector({ formats: formats })
                : new BarcodeDetector();
    
            // Lưu trữ ID của interval để có thể dừng nó sau này
            this.detectionInterval = setInterval(async () => {
                try 
                {
                    if (videoElement.readyState === videoElement.HAVE_ENOUGH_DATA)
                    {
                        const barcodes = await barcodeDetector.detect(videoElement);
                        if (Array.isArray(barcodes) && barcodes.length > 0)
                        {
                            // Chuyển đổi kết quả thành định dạng phù hợp để trả về cho Blazor
                            const results = barcodes.map(barcode => ({
                                format: barcode.format,
                                rawValue: barcode.rawValue,
                                cornerPoints: barcode.cornerPoints,
                                boundingBox: {
                                    x: barcode.boundingBox.x,
                                    y: barcode.boundingBox.y,
                                    width: barcode.boundingBox.width,
                                    height: barcode.boundingBox.height
                                }
                            }));
                            dotNetHelper.invokeMethodAsync(resultCallbackName,
                                { success: true, results: results });
                        }
                    }
                } catch (error) {
                    console.error("Lỗi trong quá trình phát hiện mã vạch trực tiếp:", error);
                    dotNetHelper.invokeMethodAsync(resultCallbackName,
                        { success: false, error: error.message });
                }
            }, 100); // Quét mỗi 100ms
            return { success: true };
        } catch (error) {
            console.error("Lỗi khi bắt đầu phát hiện mã vạch trực tiếp:", error);
            dotNetHelper.invokeMethodAsync(resultCallbackName,
                { success: false, error: error.message });
        }
    },

    stopLiveDetection: function (videoElement) {
        try {
            if (this.detectionInterval) {
                clearInterval(this.detectionInterval);
                this.detectionInterval = null;
                // Tắt camera
                if (videoElement && videoElement.srcObject) {
                    const tracks = videoElement.srcObject.getTracks();
                    tracks.forEach(track => track.stop());
                }
                return {success: true};
            }
            return {success: false, error: "Không có phiên phát hiện mã vạch trực tiếp nào đang chạy."};
        } catch (error) {
            console.error("Lỗi khi dừng phát hiện mã vạch trực tiếp:", error);
            return {
                success: false, error: error.message
            }
        }
    }
};