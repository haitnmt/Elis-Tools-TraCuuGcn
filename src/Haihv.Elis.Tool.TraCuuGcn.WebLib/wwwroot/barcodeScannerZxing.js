// Kiểm tra camera
window.checkCameraAvailability = async () => {
    try {
        const devices = await navigator.mediaDevices.enumerateDevices();
        return devices.some(device => device.kind === 'videoinput');
    } catch {
        return false;
    }
};

window.initZxingReader = async () => {

    // // Thiết lập các hints tối ưu cho QR Code và Code 128
    //const hints = new Map();
    //hints.set(ZXing.DecodeHintType.TRY_HARDER, true);           // Cố gắng quét kỹ hơn
    //hints.set(ZXing.DecodeHintType.PURE_BARCODE, false);        // Tắt chế độ pure barcode để xử lý tốt hơn với hình ảnh không hoàn hảo
    //hints.set(ZXing.DecodeHintType.CHARACTER_SET, 'UTF-8');     // Hỗ trợ ký tự UTF-8
    //hints.set(ZXing.DecodeHintType.POSSIBLE_FORMATS, ['QR_CODE', 'CODE_128']); // Chỉ tập trung vào 2 định dạng này
    //hints.set(ZXing.DecodeHintType.ALSO_INVERTED, true);        // Hỗ trợ cả mã đảo ngược màu
    return new window.ZXing.BrowserMultiFormatReader();
}

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


// Xử lý ảnh từ URL
window.scanFromImage = async (imageUrl) => {
    try {
        const reader = await initZxingReader();
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