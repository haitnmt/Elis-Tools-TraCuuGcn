# Tracuugcn Web API

Dịch vụ Web API để tra cứu thông tin Giấy chứng nhận từ cơ sở dữ liệu địa chính trên nền phần mềm ELIS SQL

## Dịch vụ

### Web API

- **Image**: `haitnmt/tracuugcn-api`
- **Ports**: Exposed on `8089`, mapped to container port `8089`
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

Để mở rộng dịch vụ `web-api`, chạy lệnh:

```sh
docker-compose up -d --scale web-api=<số_lượng_bản_sao>
```

Thay `<số_lượng_bản_sao>` bằng số lượng bản sao mong muốn.

## Ví dụ Docker Compose

Dưới đây là ví dụ về tệp `docker-compose.yml`:

```yaml
version: '3.8'

services:
  web-api:
    image: haitnmt/tracuugcn-api
    ports:
      - "8089:8089"
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
  "Serilog": {
    "Using": [
      "Serilog.Sinks.Console"
    ],
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Information",
        "System": "Information",
        "Microsoft.AspNetCore": "Warning",
        "Microsoft.AspNetCore.Hosting.Diagnostics": "Warning",
        "Microsoft.AspNetCore.Routing": "Warning",
        "Microsoft.AspNetCore.Mvc": "Warning"
      }
    },
    "WriteTo": [
      // Ghi log ra tệp nếu cần thiết
      //      {
      //        "Name": "File",
      //        "Args": {
      //          "path": "Logs/log-.txt",
      //          "rollingInterval": "Day",
      //          "outputTemplate": "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj} {NewLine}{Exception}"
      //        }
      //      },
      {
        "Name": "Console",
        "Args": {
          "outputTemplate": "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj} {NewLine}{Exception}"
        }
      }
    ],
    "Enrich": [
      "FromLogContext",
      "WithMachineName",
      "WithEnvironmentName"
    ]
  },
  // Ghi log ra Elasticsearch (Nếu có)
  //  "Elasticsearch": {
  //    "Uris": [
  //      "https://localhost:9200",
  //      "https://192.168.1.1:9200"
  //    ],
  //    "Token": "--==",
  //    "Namespace": "Elis-TraCuuGcn"
  //  },
  // Sử dụng Cache Redis (nếu có), mặc định chỉ lưu cache trong Memory
  //  "Redis": {
  //    "ConnectionString": "localhost:6379,user=admin,password=admin@Sa-123"
  //  },
  "JwtTokenSettings": {
    "SecretKeys": ["SecretKey"],
    "Issuers": ["https://localhost:5001"],
    "Audiences": ["https://localhost:5001"]
  },
  // Cấu hình các kết nối đến dữ liệu ELIS SQL
  "ElisSql": {
    "Databases": [
      {
        "Name": "elis",
        "MaDvhc": "127",
        "ElisConnectionString": "Server=localhost;Database=elis;User Id=sa;Password=admin@Sa-123;TrustServerCertificate=True;",
        "SdeDatabase": "sde"
      }
    ],
    // Cấu hình để kết nối đến dịch vụ lấy thông tin vị trí thửa từ SDE
    "ApiSde": "http://localhost:8151"
  },
  "AllowedHosts": "*",
  "FrontendUrl": [
    "https://tracuugcn.tentinh.gov.vn/"
  ],
  "BackendUrl": "https://api-tracuugcn.tentinh.gov.vn",
  "UserAdmin": [
    "admin"
  ]
}
```

## Giấy phép

Dự án này được cấp phép theo Giấy phép MIT.