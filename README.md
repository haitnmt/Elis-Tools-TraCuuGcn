# Hệ thống tra cứu Giấy chứng nhận theo QRCode

## Mô tả
Hệ thống tra cứu giấy chứng nhận theo mã QRCode giúp người dùng (Chủ sử dụng đất) có thể tra cứu thông tin của giấy chứng nhận một cách nhanh chóng và chính xác. Hệ thống cung cấp các thông tin chi tiết về giấy chứng nhận, thửa đất, chủ sử dụng đất và vị trí thửa đất trên bản đồ.

## Kiến trúc hệ thống

Dự án được xây dựng theo kiến trúc microservices với các thành phần chính:

### 1. Web API (Haihv.Elis.Tool.TraCuuGcn.Api)
- Xây dựng trên nền tảng ASP.NET Core 9.0
- Cung cấp các endpoint để tra cứu thông tin GCN, thửa đất, chủ sử dụng đất
- Tích hợp xác thực JWT và phân quyền người dùng
- Sử dụng caching (Redis hoặc Memory) để tối ưu hiệu suất

### 2. Web App (Haihv.Elis.Tool.TraCuuGcn.Web-App)
- Xây dựng trên nền tảng Blazor WebAssembly và Blazor Server
- Giao diện người dùng sử dụng thư viện MudBlazor
- Hỗ trợ quét mã QR trực tiếp từ camera
- Hiển thị thông tin GCN, thửa đất, chủ sử dụng đất

### 3. API SDE (Haihv.Elis.Tool.TraCuuGcn.Api.Sde)
- Xây dựng bằng ngôn ngữ Go
- Cung cấp dịch vụ lấy thông tin vị trí thửa đất từ cơ sở dữ liệu không gian (SDE)

### 4. Thư viện chung
- Models (Haihv.Elis.Tool.TraCuuGcn.Models): Chứa các model dữ liệu dùng chung
- WebLib (Haihv.Elis.Tool.TraCuuGcn.WebLib): Cung cấp các component và service dùng chung cho ứng dụng web

## Tính năng chính

### 1. Tra cứu thông tin GCN
- Tra cứu theo mã QR (quét trực tiếp từ camera)
- Tra cứu theo số serial
- Tra cứu theo mã vạch

### 2. Xem thông tin chi tiết
- Thông tin GCN (số phát hành, người ký, ngày ký, số vào sổ)
- Thông tin thửa đất (tờ bản đồ, thửa đất số, diện tích, loại đất, thời hạn)
- Thông tin chủ sử dụng đất
- Vị trí thửa đất trên bản đồ

### 3. Quản lý GCN (dành cho người dùng có quyền)
- Cập nhật thông tin GCN
- Xóa mã QR

### 4. Xác thực người dùng
- Xác thực thông qua SSO OpenID Connect (ví dụ: Keycloak, Azure AD, ...)

## Yêu cầu hệ thống

### Phát triển
- .NET SDK 9.0 trở lên
- Go 1.24.0 trở lên (cho API SDE)
- Visual Studio 2022 hoặc Visual Studio Code
- Docker (tùy chọn)

### Triển khai
- Docker và Docker Compose
- Redis (tùy chọn, cho caching)
- Elasticsearch (tùy chọn, cho logging)

## Tính năng

- Giao diện đăng nhập/đăng ký hoàn toàn bằng tiếng Việt
- Thiết kế theo Tailwind CSS, responsive trên mọi thiết bị
- Hỗ trợ chế độ tối (Dark Mode)
- Tùy chỉnh đăng nhập, đăng ký và quên mật khẩu
- Tùy chỉnh trang quản lý tài khoản người dùng

## Cấu trúc thư mục
### Phát triển

1. Clone repository:
```bash
git clone https://github.com/yourusername/Elis-Tools-TraCuuGcn.git
cd Elis-Tools-TraCuuGcn
```

2. Khôi phục các gói NuGet:
```bash
dotnet restore
```

3. Chạy ứng dụng:
```bash
cd src/AppHost
dotnet run
```

### Triển khai bằng Docker

1. Xây dựng và đẩy image API:
```bash
cd src
./BuildAndPushImageToRegistryApi.ps1
```

2. Xây dựng và đẩy image Web App:
```bash
cd src
./BuildAndPushImageToRegistryApp.ps1
```

3. Xây dựng và đẩy image API SDE:
```bash
cd src
./BuildAndPushImageToRegistryApiSde.ps1
```

4. Tạo file `docker-compose.yml` với nội dung tương tự như trong file README của API.

5. Tạo file `appsettings.json` với cấu hình phù hợp.

6. Chạy ứng dụng:
```bash
docker-compose up -d
```

## Cấu hình

Hệ thống sử dụng file `appsettings.json` để cấu hình. Dưới đây là một số cấu hình quan trọng:

### API
- `OpenIdConnect.Authority`: Địa chỉ máy chủ OpenID Connect (ví dụ: https://sso.example.com/realms/your-realm)
- `OpenIdConnect.Audience`: Định danh ứng dụng (clientId) dùng cho xác thực API
- `ElisSql.Databases`: Danh sách các kết nối đến cơ sở dữ liệu ELIS SQL
- `ElisSql.ApiSde`: URL của API SDE
- `Redis.ConnectionString`: Kết nối đến Redis (nếu sử dụng)
- `FrontendUrl`: URL của ứng dụng web
- `BackendUrl`: URL của API
- `UserAdmin`: Danh sách các tài khoản admin

### Web App
- `OpenIdConnect.Authority`: Địa chỉ máy chủ OpenID Connect
- `OpenIdConnect.ClientId`: ClientId của ứng dụng web
- `OpenIdConnect.ClientSecret`: ClientSecret của ứng dụng web (nếu cần)
- `OpenIdConnect.ResponseType`: Loại response (thường là "code")
- `OpenIdConnect.Scope`: Các scope cần thiết (ví dụ: "openid profile email")
- `ApiEndpoint`: URL của API

## Giấy phép

Dự án này được cấp phép theo Giấy phép MIT.