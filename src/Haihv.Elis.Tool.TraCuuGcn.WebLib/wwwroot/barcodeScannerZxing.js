// Kiểm tra camera
window.checkCameraAvailability = async () => {
    try {
        const devices = await navigator.mediaDevices.enumerateDevices();
        return devices.some(device => device.kind === 'videoinput');
    } catch {
        return false;
    }
};

window.startCameraScan = async (videoElement, canvasElement, dotNetHelper) => {

    // // Thiết lập các hints tối ưu cho QR Code và Code 128
    // const hints = new Map();
    // hints.set(ZXing.DecodeHintType.TRY_HARDER, true);           // Cố gắng quét kỹ hơn
    // hints.set(ZXing.DecodeHintType.PURE_BARCODE, false);        // Tắt chế độ pure barcode để xử lý tốt hơn với hình ảnh không hoàn hảo
    // hints.set(ZXing.DecodeHintType.CHARACTER_SET, 'UTF-8');     // Hỗ trợ ký tự UTF-8
    // hints.set(ZXing.DecodeHintType.POSSIBLE_FORMATS, ['QR_CODE', 'CODE_128']); // Chỉ tập trung vào 2 định dạng này
    // //hints.set(ZXing.DecodeHintType.ALSO_INVERTED, true);        // Hỗ trợ cả mã đảo ngược màu

    const constraints = {
        video: {
            width: { min: 1280, ideal: 1920, max: 2560 },
            height: { min: 720, ideal: 1080, max: 1440 },
            frameRate: { min: 15, ideal: 30 },
            aspectRatio: { ideal: 1.777777778 }
        } 
    };
    
    let scanRegion = null;
    // // Đợi video được load
    // videoElement.addEventListener('loadedmetadata', () => {
    //     if (canvasElement) {
    //         // Tính toán lại vùng quét dựa trên kích thước thực của video
    //         const videoAspectRatio = videoElement.videoWidth / videoElement.videoHeight;
    //         const canvasAspectRatio = canvasElement.width / canvasElement.height;
    //
    //         let scanWidth, scanHeight;
    //         if (videoAspectRatio > canvasAspectRatio) {
    //             scanHeight = videoElement.videoHeight;
    //             scanWidth = scanHeight * canvasAspectRatio;
    //         } else {
    //             scanWidth = videoElement.videoWidth;
    //             scanHeight = scanWidth / canvasAspectRatio;
    //         }
    //
    //         scanRegion = {
    //             x: (videoElement.videoWidth - scanWidth) / 2,
    //             y: (videoElement.videoHeight - scanHeight) / 2,
    //             width: scanWidth,
    //             height: scanHeight
    //         };
    //     }
    // });
    // Khởi tạo reader
    const reader = new ZXing.BrowserQRCodeReader();
    const decodeCallback = (result, error) => {
        if (result) dotNetHelper.invokeMethodAsync('HandleScanResult', result.text);
        if (error) console.error(error);
    };
    try {

        // Khởi động camera
        await reader.decodeFromVideoDevice(null, videoElement, decodeCallback, constraints);
        
        return reader;
    } catch (err) {
        console.error('Camera initialization error:', err);
        dotNetHelper.invokeMethodAsync('HandleScanError', 'Không thể khởi tạo camera. Vui lòng kiểm tra quyền truy cập camera.');
        return null;
    }
};


window.stopCameraScan = (reader) => {
    if (reader) {
        try {
            reader.reset();
            const stream = document.querySelector('video').srcObject;
            if (stream) {
                stream.getTracks().forEach(track => track.stop());
            }
        } catch (err) {
            console.error('Error stopping camera:', err);
        }
    }
};

// Xử lý ảnh từ URL
window.scanFromImage = async (imageUrl) => {
    try {
        // Thiết lập các hints tối ưu cho QR Code và Code 128
        const hints = new Map();
        hints.set(ZXing.DecodeHintType.TRY_HARDER, true);           // Cố gắng quét kỹ hơn
        hints.set(ZXing.DecodeHintType.PURE_BARCODE, false);        // Tắt chế độ pure barcode để xử lý tốt hơn với hình ảnh không hoàn hảo
        hints.set(ZXing.DecodeHintType.CHARACTER_SET, 'UTF-8');     // Hỗ trợ ký tự UTF-8
        hints.set(ZXing.DecodeHintType.POSSIBLE_FORMATS, ['QR_CODE', 'CODE_128']); // Chỉ tập trung vào 2 định dạng này
        hints.set(ZXing.DecodeHintType.ALSO_INVERTED, true);        // Hỗ trợ cả mã đảo ngược màu
        
        // Khởi tạo reader
        const reader = new ZXing.BrowserQRCodeReader(hints);
        // Tải và xử lý ảnh
        const result = await reader.decodeFromImageUrl(imageUrl);
        if(result === null || result.text === null || result.text === '' || result.text === undefined) {
            return new Error('Không tìm thấy mã QR hoặc Code 128 trong ảnh');
        }
        return result.text;
    } catch (error) {
        throw new Error('Không tìm thấy mã QR hoặc Code 128 trong ảnh');
    }
};