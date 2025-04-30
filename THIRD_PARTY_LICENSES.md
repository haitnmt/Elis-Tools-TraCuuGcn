# THIRD PARTY LICENSES

Dưới đây là danh sách các thư viện/phần mềm bên thứ ba chính được sử dụng trong dự án này, đã được sắp xếp và gom nhóm theo tính năng. Vui lòng tham khảo trang chủ của từng thư viện để biết chi tiết về giấy phép sử dụng.

## 1. Kết nối và lưu trữ dữ liệu
| Tên thư viện                                      | Phiên bản   | Nguồn (link)                                                    | Giấy phép | Ghi chú |
|---------------------------------------------------|-------------|------------------------------------------------------------------|-----------|---------|
| Dapper                                            | 2.1.66      | https://www.nuget.org/packages/Dapper |           | ORM nhẹ, truy vấn dữ liệu SQL hiệu quả |
| Microsoft.Data.SqlClient                          | 6.0.2       | https://www.nuget.org/packages/Microsoft.Data.SqlClient |           | Kết nối và thao tác với SQL Server |
| Aspire.StackExchange.Redis.DistributedCaching     | 9.2.1       | https://www.nuget.org/packages/Aspire.StackExchange.Redis.DistributedCaching |           | Lưu trữ cache phân tán với Redis |
| ZiggyCreatures.FusionCache                        | 2.2.0       | https://www.nuget.org/packages/ZiggyCreatures.FusionCache |           | Thư viện cache đa tầng, hỗ trợ memory/distributed cache |

## 2. Xác thực, bảo mật và nhận diện
| Tên thư viện                                      | Phiên bản   | Nguồn (link)                                                    | Giấy phép | Ghi chú |
|---------------------------------------------------|-------------|------------------------------------------------------------------|-----------|---------|
| Microsoft.AspNetCore.Authentication.JwtBearer      | 9.0.4       | https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer |           | Xác thực API bằng JWT |
| Microsoft.AspNetCore.Authentication.OpenIdConnect | 9.0.4       | https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.OpenIdConnect |           | Xác thực OpenID Connect (Keycloak, Azure AD,...) |
| Microsoft.IdentityModel.JsonWebTokens             | 8.9.0       | https://www.nuget.org/packages/Microsoft.IdentityModel.JsonWebTokens |           | Xử lý, kiểm tra token JWT |
| Microsoft.AspNetCore.DataProtection.StackExchangeRedis | 9.0.4  | https://www.nuget.org/packages/Microsoft.AspNetCore.DataProtection.StackExchangeRedis |           | Bảo vệ dữ liệu, chia sẻ key qua Redis |

## 3. Logging, giám sát và công cụ hỗ trợ
| Tên thư viện                                      | Phiên bản   | Nguồn (link)                                                    | Giấy phép | Ghi chú |
|---------------------------------------------------|-------------|------------------------------------------------------------------|-----------|---------|
| Serilog.Enrichers.Environment                     | 3.0.1       | https://www.nuget.org/packages/Serilog.Enrichers.Environment |           | Bổ sung thông tin môi trường vào log |
| Serilog.Extensions                                | 8.0.3       | https://www.nuget.org/packages/Serilog.Extensions |           | Tích hợp logging Serilog vào .NET |
| Serilog.Settings.Configuration                    | 9.0.0       | https://www.nuget.org/packages/Serilog.Settings.Configuration |           | Cấu hình Serilog qua file cấu hình |
| Elastic.Serilog.Sinks                             | 8.12.3      | https://www.nuget.org/packages/Elastic.Serilog.Sinks |           | Gửi log lên Elasticsearch |
| OpenTelemetry.Exporter.OpenTelemetryProtocol      | 1.11.2      | https://www.nuget.org/packages/OpenTelemetry.Exporter.OpenTelemetryProtocol |           | Export metrics, traces qua OTLP |
| OpenTelemetry.Extensions.Hosting                  | 1.11.2      | https://www.nuget.org/packages/OpenTelemetry.Extensions.Hosting |           | Tích hợp OpenTelemetry vào .NET host |

## 4. Web, API, giao diện và framework
| Tên thư viện                                      | Phiên bản   | Nguồn (link)                                                    | Giấy phép | Ghi chú |
|---------------------------------------------------|-------------|------------------------------------------------------------------|-----------|---------|
| Microsoft.AspNetCore.Components.WebAssembly       | 9.0.4       | https://www.nuget.org/packages/Microsoft.AspNetCore.Components.WebAssembly |           | Xây dựng giao diện web Blazor |
| Microsoft.AspNetCore.Components.WebAssembly.Authentication | 9.0.4 | https://www.nuget.org/packages/Microsoft.AspNetCore.Components.WebAssembly.Authentication |           | Xác thực người dùng trên Blazor |
| MudBlazor                                         | 8.5.1       | https://www.nuget.org/packages/MudBlazor |           | Thư viện UI cho Blazor |
| Carter                                            | 9.0.0       | https://www.nuget.org/packages/Carter |           | Xây dựng API theo mô hình module |
| MediatR                                           | 12.5.0      | https://www.nuget.org/packages/MediatR |           | Triển khai CQRS, Mediator Pattern cho xử lý request/response và event trong API |


