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
    
    // Enhanced scan configuration
    const hints = new Map();
    hints.set('TRY_HARDER', true);
    hints.set('PURE_BARCODE', false);
    hints.set('CHARACTER_SET', 'UTF-8');
    
    // Advanced constraints for better quality
    const constraints = {
        video: {
            width: { min: 1280, ideal: 1920, max: 2560 },
            height: { min: 720, ideal: 1080, max: 1440 },
            facingMode: "environment",
            aspectRatio: { ideal: 1.777777778 },
            frameRate: { ideal: 30 }
        }
    };

    // Setup scan region if canvas exists
    const scanCanvas = canvasId != null ? document.getElementById(canvasId) : null;
    const scanRegion = scanCanvas != null ? {
        x: scanCanvas.offsetLeft,
        y: scanCanvas.offsetTop,
        width: scanCanvas.offsetWidth,
        height: scanCanvas.offsetHeight
    } : null;

    // Enhanced decode callback with result validation
    const decodeCallback = (result, error) => {
        if (result) {
            // Validate result consistency
            if (lastResult === result.text) {
                resultCounter++;
                if (resultCounter >= 2) { // Require 2 consecutive same results
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
                }, 500); // Delay retry
            } else {
                dotNetHelper.invokeMethodAsync('HandleScanError', error.message);
            }
        }
    };

    try {
        // Configure reader settings
        if (reader.setHints) {
            reader.setHints(hints);
        }
        
        // Set enhanced parameters if supported
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

// Xử lý ảnh
window.scanFromImage = async (reader, imageUrl) => {
    try {
        const result = await reader.decodeFromImageUrl(imageUrl);
        return result.text;
    } catch (error) {
        throw new Error('Không tìm thấy mã trong ảnh');
    }
};