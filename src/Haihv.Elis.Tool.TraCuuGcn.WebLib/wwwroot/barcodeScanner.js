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
window.startCameraScan = async (reader, videoId, dotNetHelper) => {
    const decodeCallback = (result, error) => {
        if (result) dotNetHelper.invokeMethodAsync('HandleScanResult', result.text);
        if (error) console.error(error);
    };

    await reader.decodeFromVideoDevice(null, videoId, decodeCallback);
    return reader;
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