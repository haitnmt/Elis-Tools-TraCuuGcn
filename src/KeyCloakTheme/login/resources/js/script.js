// script.js - JavaScript chức năng cho theme KeyCloak

document.addEventListener('DOMContentLoaded', function() {
    // Chức năng dropdown language selector
    const localeDropdown = document.getElementById('kc-locale-dropdown');
    const localeMenu = document.getElementById('locale-dropdown-menu');
    
    if (localeDropdown && localeMenu) {
        localeDropdown.addEventListener('click', function(e) {
            e.preventDefault();
            localeMenu.classList.toggle('hidden');
        });
        
        // Đóng dropdown khi click bên ngoài
        document.addEventListener('click', function(e) {
            if (!localeDropdown.contains(e.target)) {
                localeMenu.classList.add('hidden');
            }
        });
    }
    
    // Chức năng hiển thị/ẩn mật khẩu
    const togglePasswordButtons = document.querySelectorAll('.toggle-password');
    
    if (togglePasswordButtons.length > 0) {
        togglePasswordButtons.forEach(function(button) {
            button.addEventListener('click', function() {
                const input = this.previousElementSibling;
                const icon = this.querySelector('i');
                
                if (input.type === 'password') {
                    input.type = 'text';
                    icon.classList.remove('fa-eye');
                    icon.classList.add('fa-eye-slash');
                } else {
                    input.type = 'password';
                    icon.classList.remove('fa-eye-slash');
                    icon.classList.add('fa-eye');
                }
            });
        });
    }
    
    // Chức năng chế độ tối (dark mode)
    const darkModeToggle = document.getElementById('toggle-dark-mode');
    
    if (darkModeToggle) {
        // Kiểm tra trạng thái đã lưu
        const savedDarkMode = localStorage.getItem('darkMode') === 'true';
        
        // Áp dụng chế độ tối nếu đã được lưu
        if (savedDarkMode) {
            document.documentElement.classList.add('dark-mode');
            updateDarkModeToggle(true);
        }
        
        // Xử lý sự kiện click vào nút chuyển đổi
        darkModeToggle.addEventListener('click', function() {
            const isDarkMode = document.documentElement.classList.toggle('dark-mode');
            localStorage.setItem('darkMode', isDarkMode);
            updateDarkModeToggle(isDarkMode);
        });
    }
    
    // Cập nhật trạng thái nút chuyển đổi chế độ tối
    function updateDarkModeToggle(isDarkMode) {
        if (!darkModeToggle) return;
        
        const icon = darkModeToggle.querySelector('i');
        
        if (isDarkMode) {
            icon.classList.remove('fa-moon-o');
            icon.classList.add('fa-sun-o');
            darkModeToggle.querySelector('span').textContent = 'Chế độ sáng';
        } else {
            icon.classList.remove('fa-sun-o');
            icon.classList.add('fa-moon-o');
            darkModeToggle.querySelector('span').textContent = 'Chế độ tối';
        }
    }
    
    // Xác thực form đăng ký
    const registerForm = document.getElementById('kc-register-form');
    
    if (registerForm) {
        registerForm.addEventListener('submit', function(event) {
            const password = document.getElementById('password');
            const passwordConfirm = document.getElementById('password-confirm');
            
            if (password && passwordConfirm && password.value !== passwordConfirm.value) {
                event.preventDefault();
                
                // Tạo thông báo lỗi
                const errorSpan = document.getElementById('input-error-password-confirm') || document.createElement('span');
                errorSpan.id = 'input-error-password-confirm';
                errorSpan.className = 'text-red-500 text-xs mt-1';
                errorSpan.setAttribute('aria-live', 'polite');
                errorSpan.textContent = 'Mật khẩu không khớp';
                
                // Thêm thông báo lỗi nếu chưa tồn tại
                if (!document.getElementById('input-error-password-confirm')) {
                    passwordConfirm.parentNode.appendChild(errorSpan);
                }
                
                // Đánh dấu trường không hợp lệ
                passwordConfirm.setAttribute('aria-invalid', 'true');
                password.focus();
            }
        });
    }
    
    // Thiết lập độ mạnh mật khẩu
    const passwordInput = document.getElementById('password');
    
    if (passwordInput) {
        passwordInput.addEventListener('input', function() {
            // Tạo phần tử hiển thị độ mạnh mật khẩu nếu chưa có
            let strengthMeter = document.getElementById('password-strength');
            
            if (!strengthMeter) {
                strengthMeter = document.createElement('div');
                strengthMeter.id = 'password-strength';
                strengthMeter.className = 'mt-2';
                passwordInput.parentNode.appendChild(strengthMeter);
            }
            
            // Tính độ mạnh mật khẩu
            const password = passwordInput.value;
            let strength = 0;
            
            // Các tiêu chí đánh giá độ mạnh
            if (password.length >= 8) strength += 1;
            if (/[A-Z]/.test(password)) strength += 1;
            if (/[a-z]/.test(password)) strength += 1;
            if (/[0-9]/.test(password)) strength += 1;
            if (/[^A-Za-z0-9]/.test(password)) strength += 1;
            
            // Cập nhật giao diện hiển thị độ mạnh
            let strengthText = '';
            let strengthClass = '';
            
            switch (strength) {
                case 0:
                case 1:
                    strengthText = 'Rất yếu';
                    strengthClass = 'bg-red-500';
                    break;
                case 2:
                    strengthText = 'Yếu';
                    strengthClass = 'bg-orange-500';
                    break;
                case 3:
                    strengthText = 'Trung bình';
                    strengthClass = 'bg-yellow-500';
                    break;
                case 4:
                    strengthText = 'Mạnh';
                    strengthClass = 'bg-blue-500';
                    break;
                case 5:
                    strengthText = 'Rất mạnh';
                    strengthClass = 'bg-green-500';
                    break;
            }
            
            // Hiển thị kết quả
            strengthMeter.innerHTML = `
                <div class="w-full h-2 bg-gray-200 rounded-full overflow-hidden">
                    <div class="${strengthClass} h-full" style="width: ${strength * 20}%"></div>
                </div>
                <p class="text-xs mt-1 text-gray-600">${strengthText}</p>
            `;
        });
    }
});
