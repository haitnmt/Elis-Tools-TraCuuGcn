# Tracuugcn Sde Api

Dịch vụ Web API để lấy tọa độ của Thửa đất trên dữ liệu SDE

## Dịch vụ

### Web API

- **Image**: `haitnmt/tracuugcn-api-sde`
- **Ports**: Exposed on `8151`, mapped to container port `8151`
- **Volume**: Tệp cấu hình `data-init` được gắn vào `/app/config/data.ini`
- **Replicas**: 2 (Có thể sử dụng nếu cần thiết)

## Cấu hình

Đảm bảo bạn có tệp `data.ini` trong cùng thư mục với tệp `docker-compose.yml`.

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

Để mở rộng dịch vụ `sde-api`, chạy lệnh:

```sh
docker-compose up -d --scale sde-api=<số_lượng_bản_sao>
```

Thay `<số_lượng_bản_sao>` bằng số lượng bản sao mong muốn.

## Ví dụ Docker Compose

Dưới đây là ví dụ về tệp `docker-compose.yml`:

```yaml
version: '3.8'

services:
  sde-api:
    image: haitnmt/tracuugcn-app
    ports:
      - "8151:8151"
    volumes:
      - type: config
        source: data-init
        target: /app/config/data.ini
        volume:
          nocopy: true

configs:
  data-init:
    file: ./data.ini
```

## data.ini mẫu:
```init
[database]
server = your_server
port = 1433
user = your_readonly_user
password = your_password
;Mật khẩu không nên có ký tự đặc biệt

[databases]
; Danh sách các database được phép truy cập
allowed = sde1,sde2,sde3
```

## Giấy phép

Dự án này được cấp phép theo Giấy phép MIT.