/**
 * Module để lắng nghe sự thay đổi localStorage giữa các tab trình duyệt
 */

// Tham chiếu đến đối tượng .NET
let dotNetHelper = null;

/**
 * Hàm xử lý sự kiện storage thay đổi
 * @param {StorageEvent} e - Sự kiện storage
 */
function handleStorageChange(e) {
    console.log(`Storage changed: ${e.key}`);
    
    // Chỉ xử lý khi có dotNetHelper và key không null
    if (dotNetHelper && e.key) {
        // Gọi phương thức .NET để xử lý sự thay đổi
        dotNetHelper.invokeMethodAsync('OnStorageChanged', e.key, e.newValue, e.oldValue)
            .catch(error => console.error('Lỗi khi gọi phương thức .NET:', error));
    }
}

/**
 * Khởi tạo dịch vụ lắng nghe sự thay đổi localStorage
 * @param {DotNetObjectReference} helper - Tham chiếu đến đối tượng .NET
 */
export function initialize(helper) {
    dotNetHelper = helper;
    
    // Đăng ký lắng nghe sự kiện storage
    window.addEventListener('storage', handleStorageChange);
    
    console.log('Đã khởi tạo dịch vụ lắng nghe localStorage');
}

/**
 * Hủy đăng ký lắng nghe sự kiện
 */
export function dispose() {
    // Hủy đăng ký lắng nghe sự kiện
    window.removeEventListener('storage', handleStorageChange);
    dotNetHelper = null;
    
    console.log('Đã hủy dịch vụ lắng nghe localStorage');
}