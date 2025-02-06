// Kiểm tra camera
window.checkCameraAvailability = async () => {
    try {
        const devices = await navigator.mediaDevices.enumerateDevices();
        return devices.some(device => device.kind === 'videoinput');
    } catch {
        return false;
    }
};

// Xử lý camera
window.startCameraScan = async (reader, videoId, canvasId, dotNetHelper) => {
    const maxRetries = 3;
    let retryCount = 0;
    let lastResult = null;
    let resultCounter = 0;

    // Thiết lập các gợi ý cho ZXing: Cố gắng nhận diện Code 128 và QR Code
    const hints = new Map();
    hints.set('TRY_HARDER', true);
    hints.set('PURE_BARCODE', false);
    hints.set('CHARACTER_SET', 'UTF-8');
    hints.set('POSSIBLE_FORMATS', ['CODE_128', 'QR_CODE']); // Tăng cường khả năng nhận diện Code128 và QRCode

    // Trước tiên, lấy độ phân giải tối đa mà camera hỗ trợ thông qua một stream tạm thời
    let constraints;
    try {
        const tempStream = await navigator.mediaDevices.getUserMedia({ video: { facingMode: "environment" } });
        const videoTrack = tempStream.getVideoTracks()[0];
        const capabilities = videoTrack.getCapabilities();
        // Sử dụng độ phân giải tối đa nếu có, ngược lại dùng giá trị mặc định
        const maxWidth = capabilities.width && capabilities.width.max ? capabilities.width.max : 1920;
        const maxHeight = capabilities.height && capabilities.height.max ? capabilities.height.max : 1080;
        // Tính tỉ lệ khung hình tối ưu
        const aspectRatio = maxWidth / maxHeight;
        // Tắt stream tạm thời
        videoTrack.stop();

        constraints = {
            video: {
                width: { ideal: maxWidth },
                height: { ideal: maxHeight },
                facingMode: "environment",
                aspectRatio: { ideal: aspectRatio },
                frameRate: { ideal: 30 }
            }
        };
    } catch (err) {
        // Nếu không lấy được độ phân giải tối đa, sử dụng giá trị dự phòng
        constraints = {
            video: {
                width: { min: 1280, ideal: 1920, max: 2560 },
                height: { min: 720, ideal: 1080, max: 1440 },
                facingMode: "environment",
                aspectRatio: { ideal: 1.777777778 },
                frameRate: { ideal: 30 }
            }
        };
    }

    // Thiết lập vùng quét nếu canvas tồn tại
    const scanCanvas = canvasId != null ? document.getElementById(canvasId) : null;
    const scanRegion = scanCanvas != null ? {
        x: scanCanvas.offsetLeft,
        y: scanCanvas.offsetTop,
        width: scanCanvas.offsetWidth,
        height: scanCanvas.offsetHeight
    } : null;

    // Hàm callback giải mã với kiểm tra kết quả liên tục
    const decodeCallback = (result, error) => {
        if (result) {
            // Kiểm tra tính nhất quán của kết quả
            if (lastResult === result.text) {
                resultCounter++;
                if (resultCounter >= 2) { // Cần 2 kết quả giống nhau liên tiếp
                    dotNetHelper.invokeMethodAsync('HandleScanResult', result.text);
                    lastResult = null;
                    resultCounter = 0;
                }
            } else {
                lastResult = result.text;
                resultCounter = 1;
            }
        } else if (error) {
            console.error('Scan error:', error);
            if (retryCount < maxRetries) {
                retryCount++;
                setTimeout(() => {
                    reader.decodeFromVideoDevice(null, videoId, decodeCallback, constraints, scanRegion);
                }, 500); // Tạm dừng nhẹ trước khi thử lại
            } else {
                dotNetHelper.invokeMethodAsync('HandleScanError', error.message);
            }
        }
    };

    try {
        // Cấu hình cài đặt cho reader
        if (reader.setHints) {
            reader.setHints(hints);
        }

        // Thiết lập các tham số video nếu hỗ trợ
        if (reader.setVideoTrackConstraints) {
            reader.setVideoTrackConstraints(constraints);
        }

        await reader.decodeFromVideoDevice(null, videoId, decodeCallback, constraints, scanRegion);
        return reader;
    } catch (err) {
        dotNetHelper.invokeMethodAsync('HandleScanError', err.message);
        return null;
    }
};

window.stopCameraScan = (reader) => {
    reader.reset();
};

// Xử lý ảnh từ URL
window.scanFromImage = async (reader, imageUrl) => {
    try {
        // Thiết lập các hints tối ưu cho QR Code và Code 128
        const hints = new Map();
        hints.set('TRY_HARDER', true);           // Cố gắng quét kỹ hơn
        hints.set('PURE_BARCODE', false);        // Tắt chế độ pure barcode để xử lý tốt hơn với hình ảnh không hoàn hảo
        hints.set('CHARACTER_SET', 'UTF-8');     // Hỗ trợ ký tự UTF-8
        hints.set('POSSIBLE_FORMATS', ['QR_CODE', 'CODE_128']); // Chỉ tập trung vào 2 định dạng này
        hints.set('ALSO_INVERTED', true);        // Hỗ trợ cả mã đảo ngược màu

        // Thiết lập cấu hình bổ sung nếu có
        if (reader.options) {
            reader.options.tryHarder = true;
            reader.options.returnCodes = ['QR_CODE', 'CODE_128'];
        }
        
        // Tải và xử lý ảnh
        const result = await reader.decodeFromImageUrl(imageUrl);
        return result.text;
    } catch (error) {
        throw new Error('Không tìm thấy mã QR hoặc Code 128 trong ảnh');
    }
};