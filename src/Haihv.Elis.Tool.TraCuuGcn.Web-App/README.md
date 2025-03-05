# Tracuugcn Web App

Trang thông tin để tra cứu thông tin Giấy chứng nhận từ cơ sở dữ liệu địa chính trên nền phần mềm ELIS SQL

## Dịch vụ

### Web API

- **Image**: `haitnmt/tracuugcn-app`
- **Ports**: Exposed on `8088`, mapped to container port `8088`
- **Volume**: Tệp cấu hình `api-setting` được gắn vào `/app/appsettings.json`
- **Replicas**: 2 (Có thể sử dụng nếu cần thiết)

## Cấu hình

Đảm bảo bạn có tệp `appsettings.json` trong cùng thư mục với tệp `docker-compose.yml`.

## Sử dụng

Để khởi động các dịch vụ, chạy lệnh:

```sh
docker-compose up -d
```

Để dừng các dịch vụ, chạy lệnh:

```sh
docker-compose down
```

## Mở rộng

Để mở rộng dịch vụ `web-app`, chạy lệnh:

```sh
docker-compose up -d --scale web-app=<số_lượng_bản_sao>
```

Thay `<số_lượng_bản_sao>` bằng số lượng bản sao mong muốn.

## Ví dụ Docker Compose

Dưới đây là ví dụ về tệp `docker-compose.yml`:

```yaml
version: '3.8'

services:
  web-app:
    image: haitnmt/tracuugcn-app
    ports:
      - "8088:8088"
    volumes:
      - type: config
        source: api-setting
        target: /app/appsettings.json
        volume:
          nocopy: true

configs:
  api-setting:
    file: ./appsettings.json
```

## appsettings.json mẫu:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ApiEndpoint": "https://localhost:8089",
  "AuthEndpoint": "http://localhost:8080",
  "IsDemo": true
}
```

## Giấy phép

Dự án này được cấp phép theo Giấy phép MIT.