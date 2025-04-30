# Hệ thống tra cứu Giấy chứng nhận theo QRCode - API

## Lưu ý cấu hình bắt buộc

**ConnectionStrings.Cache** (bắt buộc):
- Cần cấu hình kết nối đến Redis hoặc Valkey cho API.
- Nếu không cấu hình trường này, hệ thống sẽ không hoạt động.
- Ví dụ cấu hình:
  - "ConnectionStrings": { "Cache": "localhost:6379" } (Redis hoặc Valkey Connection String)

## Cấu hình

Hệ thống sử dụng file `appsettings.json` và `appsettings.Development.json` để cấu hình. Các trường bắt buộc (ghi rõ trong ngoặc):

- **ConnectionStrings.Cache** (bắt buộc): Kết nối Redis/Valkey.
- **OpenIdConnect.Authority** (bắt buộc): Địa chỉ máy chủ OpenID Connect.
- **OpenIdConnect.Audience** (bắt buộc): Định danh ứng dụng (clientId) dùng cho xác thực API.
- **ElisSql.Databases** (bắt buộc): Danh sách các kết nối đến cơ sở dữ liệu ELIS SQL.
- **ElisSql.ApiSde** (bắt buộc): URL của API SDE.
- **FrontendUrl** (bắt buộc): URL của ứng dụng web.
- **BackendUrl** (bắt buộc): URL của API.
- **UserAdmins** (bắt buộc): Danh sách các tài khoản admin.
- **InstanceName** (bắt buộc): Tên định danh instance API.

> **Lưu ý:**
> - `appsettings.Development.json` dùng cho môi trường phát triển, có thể khác với `appsettings.json` (production).
> - Đảm bảo trường `ConnectionStrings.Cache` luôn có mặt và hợp lệ ở cả hai file cấu hình.
> - Nếu không cấu hình cache, ứng dụng sẽ không thể khởi động.

## Tham khảo cấu hình mẫu
```json
{
  "ConnectionStrings": {
    "Cache": "localhost:6379"
  },
  "OpenIdConnect": {
    "Authority": "https://sso.example.com/realms/your-realm",
    "Audience": "account"
  },
  "ElisSql": {
    "Databases": [ ... ],
    "ApiSde": "http://localhost:8151"
  },
  "FrontendUrl": ["https://your-frontend-url/"],
  "BackendUrl": "https://your-backend-url/",
  "UserAdmins": ["admin@example.com"],
  "InstanceName": "TraCuuGcn-Api"
}
```