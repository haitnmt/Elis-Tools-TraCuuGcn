# THIRD PARTY LICENSES

Dưới đây là danh sách các thư viện/phần mềm bên thứ ba chính được sử dụng trong dự án này, đã được sắp xếp và gom nhóm theo tính năng. Vui lòng tham khảo trang chủ của từng thư viện để biết chi tiết về giấy phép sử dụng.

## 1. Kết nối và lưu trữ dữ liệu
| Tên thư viện & mô tả                              | Phiên bản   | Nguồn (link)                                                    | Giấy phép |
|---------------------------------------------------|-------------|------------------------------------------------------------------|-----------|
| Dapper – ORM nhẹ, truy vấn dữ liệu SQL hiệu quả    | 2.1.66      | https://www.nuget.org/packages/Dapper | Apache-2.0 |
| Microsoft.Data.SqlClient – Kết nối và thao tác với SQL Server | 6.0.2 | https://www.nuget.org/packages/Microsoft.Data.SqlClient | MIT |
| Aspire.StackExchange.Redis.DistributedCaching – Lưu trữ cache phân tán với Redis | 9.2.1 | https://www.nuget.org/packages/Aspire.StackExchange.Redis.DistributedCaching | MIT |
| ZiggyCreatures.FusionCache – Thư viện cache đa tầng, hỗ trợ memory/distributed cache | 2.2.0 | https://www.nuget.org/packages/ZiggyCreatures.FusionCache | MIT |

## 2. Xác thực, bảo mật và nhận diện
| Tên thư viện & mô tả                              | Phiên bản   | Nguồn (link)                                                    | Giấy phép |
|---------------------------------------------------|-------------|------------------------------------------------------------------|-----------|
| Microsoft.AspNetCore.Authentication.JwtBearer – Xác thực API bằng JWT | 9.0.4 | https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer | MIT |
| Microsoft.AspNetCore.Authentication.OpenIdConnect – Xác thực OpenID Connect (Keycloak, Azure AD,...) | 9.0.4 | https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.OpenIdConnect | MIT |
| Microsoft.IdentityModel.JsonWebTokens – Xử lý, kiểm tra token JWT | 8.9.0 | https://www.nuget.org/packages/Microsoft.IdentityModel.JsonWebTokens | MIT |
| Microsoft.AspNetCore.DataProtection.StackExchangeRedis – Bảo vệ dữ liệu, chia sẻ key qua Redis | 9.0.4 | https://www.nuget.org/packages/Microsoft.AspNetCore.DataProtection.StackExchangeRedis | MIT |

## 3. Logging, giám sát và công cụ hỗ trợ
| Tên thư viện & mô tả                              | Phiên bản   | Nguồn (link)                                                    | Giấy phép |
|---------------------------------------------------|-------------|------------------------------------------------------------------|-----------|
| Serilog.Enrichers.Environment – Bổ sung thông tin môi trường vào log | 3.0.1 | https://www.nuget.org/packages/Serilog.Enrichers.Environment | Apache-2.0 |
| Serilog.Extensions – Tích hợp logging Serilog vào .NET | 8.0.3 | https://www.nuget.org/packages/Serilog.Extensions | Apache-2.0 |
| Serilog.Settings.Configuration – Cấu hình Serilog qua file cấu hình | 9.0.0 | https://www.nuget.org/packages/Serilog.Settings.Configuration | Apache-2.0 |
| Elastic.Serilog.Sinks – Gửi log lên Elasticsearch  | 8.12.3      | https://www.nuget.org/packages/Elastic.Serilog.Sinks | Apache-2.0 |
| OpenTelemetry.Exporter.OpenTelemetryProtocol – Export metrics, traces qua OTLP | 1.11.2 | https://www.nuget.org/packages/OpenTelemetry.Exporter.OpenTelemetryProtocol | Apache-2.0 |
| OpenTelemetry.Extensions.Hosting – Tích hợp OpenTelemetry vào .NET host | 1.11.2 | https://www.nuget.org/packages/OpenTelemetry.Extensions.Hosting | Apache-2.0 |

## 4. Web, API, giao diện và framework
| Tên thư viện & mô tả                              | Phiên bản   | Nguồn (link)                                                    | Giấy phép |
|---------------------------------------------------|-------------|------------------------------------------------------------------|-----------|
| Microsoft.AspNetCore.Components.WebAssembly – Xây dựng giao diện web Blazor | 9.0.4 | https://www.nuget.org/packages/Microsoft.AspNetCore.Components.WebAssembly | MIT |
| Microsoft.AspNetCore.Components.WebAssembly.Authentication – Xác thực người dùng trên Blazor | 9.0.4 | https://www.nuget.org/packages/Microsoft.AspNetCore.Components.WebAssembly.Authentication | MIT |
| MudBlazor – Thư viện UI cho Blazor                | 8.5.1       | https://www.nuget.org/packages/MudBlazor | MIT |
| Carter – Xây dựng API theo mô hình module         | 9.0.0       | https://www.nuget.org/packages/Carter | MIT |
| MediatR – Triển khai CQRS, Mediator Pattern cho xử lý request/response và event trong API | 12.5.0 | https://www.nuget.org/packages/MediatR | Apache-2.0 |


