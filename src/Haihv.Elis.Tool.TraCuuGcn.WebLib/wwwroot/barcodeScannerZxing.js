// Kiểm tra camera
window.checkCameraAvailability = async () => {
    try {
        const devices = await navigator.mediaDevices.enumerateDevices();
        return devices.some(device => device.kind === 'videoinput');
    } catch {
        return false;
    }
};

// Khởi tạo ZXing Reader
window.initZxingReader = async () => {
    // Thiết lập các hints tối ưu cho QR Code và Code 128
    // const hints = new Map();
    // hints.set(ZXing.DecodeHintType.TRY_HARDER, true);           // Cố gắng quét kỹ hơn
    // hints.set(ZXing.DecodeHintType.PURE_BARCODE, false);        // Tắt chế độ pure barcode để xử lý tốt hơn với hình ảnh không hoàn hảo
    // hints.set(ZXing.DecodeHintType.CHARACTER_SET, 'UTF-8');     // Hỗ trợ ký tự UTF-8
    // hints.set(ZXing.DecodeHintType.POSSIBLE_FORMATS, ['QR_CODE', 'CODE_128']); // Chỉ tập trung vào 2 định dạng này
    // hints.set(ZXing.DecodeHintType.ALSO_INVERTED, true);        // Hỗ trợ cả mã đảo ngược màu
    let reader = new window.ZXing.BrowserMultiFormatReader();
    reader.timeBetweenDecodingAttempts = 200;
    return reader;
}

// Xử lý Video (bổ sung auto-focus và tăng độ phân giải stream)
window.startVideo = async (videoElement) => {
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
};

// Xử lý camera
window.startCameraScan = async (reader, videoElement, dotNetHelper) => {
    try {
        const stream = await window.startVideo(videoElement);
        
        const decodeCallback = (result, error) => {
            if (result) {
                dotNetHelper.invokeMethodAsync('HandleScanResult', result.text);
            }
            if (error && error.message !== 'No MultiFormat Readers were able to detect the code.') {
                console.error('Scanning error:', error);
            }
        };
        
        // 5. Bắt đầu decode
        await reader.decodeFromStream(stream, videoElement, decodeCallback);

        return {
            reader,
            stream,  // Lưu stream để cleanup sau này
            stop: () => {
                stream.getTracks().forEach(track => track.stop());
                reader.reset();
            }
        };
    } catch (error) {
        console.error('Camera scan error:', error);
        throw error;
    }
};

// Thêm hàm stop để cleanup
window.stopCameraScan = (scannerObj) => {
    if (scannerObj && scannerObj.stop) {
        scannerObj.stop();
    }
};

// Khởi tạo ZXing Reader
window.initZxingReaderDecodeFromImage = async () => {
    // Thiết lập các hints tối ưu cho QR Code và Code 128
    const hints = new Map();
    hints.set(ZXing.DecodeHintType.TRY_HARDER, true);           // Cố gắng quét kỹ hơn
    hints.set(ZXing.DecodeHintType.PURE_BARCODE, false);        // Tắt chế độ pure barcode để xử lý tốt hơn với hình ảnh không hoàn hảo
    hints.set(ZXing.DecodeHintType.CHARACTER_SET, 'UTF-8');     // Hỗ trợ ký tự UTF-8
    hints.set(ZXing.DecodeHintType.POSSIBLE_FORMATS, ['QR_CODE', 'CODE_128']); // Chỉ tập trung vào 2 định dạng này
    hints.set(ZXing.DecodeHintType.ALSO_INVERTED, true);        // Hỗ trợ cả mã đảo ngược màu
    return new window.ZXing.BrowserMultiFormatReader(hints);
}

// Xử lý ảnh từ URL
window.scanFromImage = async (imageUrl) => {
    const decodeImage = async (reader) => {
        try {
            const result = await reader.decodeFromImageUrl(imageUrl);
            if (result && result.text) {
                return result.text;
            }
            return null;
        } catch (error)
        {
            console.error('Error when decoding image:', error);
            return null;
        }
    };
    let result = null;
    const reader = await initZxingReaderDecodeFromImage();
    result = await decodeImage(reader);
    if (!result) {
        const codeReader = new ZXing.BrowserMultiFormatReader()
        result = await decodeImage(codeReader);
    }
    if (!result) {
        console.error('No barcode found');
        throw new Error('No barcode found');
    }
    return result;
};