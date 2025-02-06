/*************************************************************
 * barcodeScannerInterop.js
 *
 * Sử dụng QuaggaJS (nhận diện Code128) và jsQR (nhận diện QR Code)
 * để quét mã vạch từ camera và từ ảnh tĩnh.
 *************************************************************/

/**
 * Kiểm tra khả năng truy cập camera.
 */
window.checkCameraAvailability = async () => {
    try {
        const devices = await navigator.mediaDevices.enumerateDevices();
        return devices.some(device => device.kind === 'videoinput');
    } catch {
        return false;
    }
};

/*************************************************************
 * Quét Code128 từ camera bằng QuaggaJS
 *************************************************************/
window.startCameraScanCode128 = async (videoElementId, dotNetHelper) => {
    // Thiết lập tham số constraints để cố gắng sử dụng camera (facingMode=environment)
    const constraints = {
        video: {
            width: { ideal: 1920 },
            height: { ideal: 1080 },
            facingMode: { ideal: "environment" }
        }
    };

    // Truy cập phần tử video trên DOM
    const videoElement = document.getElementById(videoElementId);
    if (!videoElement) {
        console.error(`Không tìm thấy phần tử video với id = ${videoElementId}`);
        dotNetHelper.invokeMethodAsync('HandleScanError', 'Video element not found.');
        return;
    }

    // Dừng camera nếu đang chạy
    window.stopCameraScanCode128();

    // Cấu hình Quagga
    Quagga.init(
        {
            inputStream: {
                type: 'LiveStream',
                constraints: constraints,
                target: videoElement
            },
            decoder: {
                // Chỉ định quét Code128
                readers: ['code_128_reader']
            }
        },
        function (err) {
            if (err) {
                console.error('Quagga init error:', err);
                dotNetHelper.invokeMethodAsync('HandleScanError', err.message);
                return;
            }
            console.log('QuaggaJS init thành công. Bắt đầu quét Code128...');
            Quagga.start();
        }
    );

    // Sự kiện khi quét được Code128
    Quagga.onDetected(data => {
        if (data && data.codeResult && data.codeResult.code) {
            console.log('Detected Code128:', data.codeResult.code);
            dotNetHelper.invokeMethodAsync('HandleScanResult', data.codeResult.code);
        }
    });

    // Sự kiện khi quét lỗi
    Quagga.onProcessed(result => {
        if (!result || typeof result === 'undefined') {
            // Trường hợp chưa quét được
        }
    });
};

/**
 * Dừng quét camera Code128.
 */
window.stopCameraScanCode128 = () => {
    if (Quagga && Quagga.stop) {
        Quagga.stop();
    }
};

/*************************************************************
 * Quét QR Code từ camera bằng jsQR
 *************************************************************/
window.startCameraScanQr = async (videoElementId, dotNetHelper) => {
    const videoElement = document.getElementById(videoElementId);
    if (!videoElement) {
        console.error(`Không tìm thấy phần tử video với id = ${videoElementId}`);
        dotNetHelper.invokeMethodAsync('HandleScanError', 'Video element not found.');
        return;
    }

    // Thử truy cập camera
    let stream = null;
    try {
        stream = await navigator.mediaDevices.getUserMedia({
            video: { facingMode: 'environment' }
        });
    } catch (err) {
        console.error('Không thể truy cập camera:', err);
        dotNetHelper.invokeMethodAsync('HandleScanError', err.message);
        return;
    }

    videoElement.srcObject = stream;
    videoElement.setAttribute('playsinline', true);
    await videoElement.play();

    const canvas = document.createElement('canvas');
    const context = canvas.getContext('2d');

    let scanning = true;

    const tick = () => {
        if (!scanning) return;

        if (videoElement.readyState === videoElement.HAVE_ENOUGH_DATA) {
            canvas.width = videoElement.videoWidth;
            canvas.height = videoElement.videoHeight;
            context.drawImage(videoElement, 0, 0, canvas.width, canvas.height);

            const imageData = context.getImageData(0, 0, canvas.width, canvas.height);
            const code = jsQR(imageData.data, imageData.width, imageData.height);

            if (code) {
                console.log('Detected QR Code:', code.data);
                dotNetHelper.invokeMethodAsync('HandleScanResult', code.data);
                scanning = false;

                // Dừng camera
                if (stream) {
                    stream.getTracks().forEach(track => track.stop());
                }
                return;
            }
        }
        requestAnimationFrame(tick);
    };

    requestAnimationFrame(tick);
};

/*************************************************************
 * Quét Code128 từ ảnh tĩnh bằng QuaggaJS
 *************************************************************/
window.scanImageForCode128 = async (imageUrl) => {
    return new Promise((resolve, reject) => {
        Quagga.decodeSingle(
            {
                src: imageUrl,
                // Quét riêng Code128
                decoder: {
                    readers: ['code_128_reader']
                }
            },
            (result) => {
                if (result && result.codeResult && result.codeResult.code) {
                    resolve(result.codeResult.code);
                } else {
                    reject(new Error('Không tìm thấy Code128 trong ảnh.'));
                }
            }
        );
    });
};

/*************************************************************
 * Quét QR Code từ ảnh tĩnh bằng jsQR
 *************************************************************/
window.scanImageForQr = async (imageUrl) => {
    return new Promise((resolve, reject) => {
        const img = new Image();
        img.crossOrigin = 'Anonymous';
        img.onload = function () {
            const canvas = document.createElement('canvas');
            const context = canvas.getContext('2d');
            canvas.width = img.width;
            canvas.height = img.height;
            context.drawImage(img, 0, 0, img.width, img.height);
            const imageData = context.getImageData(0, 0, img.width, img.height);
            const code = jsQR(imageData.data, imageData.width, imageData.height);
            if (code) {
                resolve(code.data);
            } else {
                reject(new Error('Không tìm thấy QR Code trong ảnh.'));
            }
        };
        img.onerror = function (err) {
            reject(new Error('Không thể tải ảnh.'));
        };
        img.src = imageUrl;
    });
};