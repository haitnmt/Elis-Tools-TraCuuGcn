# Hệ thống tra cứu Giấy chứng nhận theo QRCode - Web App

## Lưu ý cấu hình bắt buộc

**ConnectionStrings.Cache** (bắt buộc):
- Cần cấu hình kết nối đến Redis hoặc Valkey cho Web App.
- Nếu không cấu hình trường này, hệ thống sẽ không hoạt động.
- Ví dụ cấu hình:
  - "ConnectionStrings": { "Cache": "localhost:6379" } (Redis hoặc Valkey Connection String)

## Cấu hình

Hệ thống sử dụng file `appsettings.json` và `appsettings.Development.json` để cấu hình. Các trường bắt buộc (ghi rõ trong ngoặc):

- **ConnectionStrings.Cache** (bắt buộc): Kết nối Redis/Valkey.
- **OpenIdConnect.Authority** (bắt buộc): Địa chỉ máy chủ OpenID Connect.
- **OpenIdConnect.ClientId** (bắt buộc): ClientId của ứng dụng web.
- **OpenIdConnect.ClientSecret** (nếu cần): ClientSecret của ứng dụng web.
- **OpenIdConnect.ResponseType** (bắt buộc): Loại response (thường là "code").
- **OpenIdConnect.Scope** (bắt buộc): Các scope cần thiết (ví dụ: "openid profile email").
- **ApiEndpoint** (bắt buộc): URL của API.
- **IsDemo** (tùy chọn): Bật/tắt chế độ demo (true/false).
- **InstanceName** (bắt buộc): Tên định danh instance Web App.

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
    "ClientId": "tracuugcn-web",
    "ClientSecret": "<your-secret>",
    "ResponseType": "code",
    "Scope": "openid profile email"
  },
  "ApiEndpoint": "https://your-api-endpoint/",
  "IsDemo": false,
  "InstanceName": "TraCuuGcn-App"
}
```