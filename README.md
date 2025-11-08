# Dự án Dịch vụ Rút gọn URL

Đây là một dự án triển khai dịch vụ rút gọn URL sử dụng .NET 8 cho backend và React cho frontend, được thiết kế theo kiến trúc Clean Architecture.

## 🚀 Tổng quan

Dịch vụ cho phép người dùng nhập một URL dài và nhận lại một URL ngắn hơn. Khi truy cập vào URL ngắn, người dùng sẽ được tự động chuyển hướng đến URL gốc.

- **Backend**: .NET 8 Web API, Clean Architecture, Entity Framework Core, SQL Server.
- **Frontend**: React, Axios.

## ✅ Tính năng

- **Tạo URL rút gọn**: Chuyển đổi một URL dài thành một mã code ngắn, duy nhất.
- **Chuyển hướng**: Tự động chuyển hướng từ URL ngắn đến URL gốc.
- **Xác thực URL**: Đảm bảo URL đầu vào là hợp lệ.
- **Lưu trữ**: Sử dụng SQL Server để lưu trữ các URL đã rút gọn.
- **API**: Cung cấp RESTful API để tương tác với dịch vụ.
- **Giao diện người dùng**: Giao diện web đơn giản được xây dựng bằng React.

## 🏗️ Kiến trúc Backend (Clean Architecture)

Dự án backend được cấu trúc theo các lớp rõ ràng:

1.  **`UrlShortener.Core`**: Chứa logic nghiệp vụ cốt lõi (entities, interfaces, services). Lớp này không phụ thuộc vào bất kỳ lớp nào khác.
2.  **`UrlShortener.Infrastructure`**: Chịu trách nhiệm về các vấn đề kỹ thuật như truy cập cơ sở dữ liệu (EF Core DbContext, Repositories).
3.  **`UrlShortener.API`**: Là điểm vào của ứng dụng, chứa các Controllers, DTOs và cấu hình dịch vụ. Lớp này phụ thuộc vào cả Core và Infrastructure.

## 🛠️ Hướng dẫn Cài đặt và Chạy dự án

### Yêu cầu

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js và npm](https://nodejs.org/)
- [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads)

### 1. Cấu hình Backend

1.  **Mở SQL Server Management Studio (SSMS)** và tạo một cơ sở dữ liệu mới với tên `UrlShortenerDb`.

2.  **Cập nhật chuỗi kết nối**: Mở file `Backend/UrlShortener.API/appsettings.json` và chỉnh sửa `DefaultConnection` để trỏ đến instance SQL Server của bạn.

    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=TEN_SERVER_CUA_BAN;Database=UrlShortenerDb;Integrated Security=True;TrustServerCertificate=True"
    }
    ```

3.  **Áp dụng Migrations**: Mở terminal, điều hướng đến thư mục `Backend` và chạy các lệnh sau:

    ```sh
    # Lệnh này tạo các file migration dựa trên DbContext
    dotnet ef migrations add InitialCreate --project UrlShortener.Infrastructure --startup-project UrlShortener.API

    # Lệnh này áp dụng migration vào database đã tạo
    dotnet ef database update --startup-project UrlShortener.API
    ```

4.  **Chạy Backend**: Điều hướng đến thư mục `Backend/UrlShortener.API` và chạy lệnh:

    ```sh
    dotnet run
    ```

    Backend sẽ khởi động tại `http://localhost:5234`.

### 2. Cấu hình Frontend

1.  **Cài đặt dependencies**: Mở một terminal mới, điều hướng đến thư mục `Frontend/React` và chạy lệnh:

    ```sh
    npm install
    ```

2.  **Chạy Frontend**: Vẫn trong thư mục `Frontend/React`, chạy lệnh:

    ```sh
    npm start
    ```

    Ứng dụng React sẽ mở trong trình duyệt tại `http://localhost:3000`.

## 📖 Cách sử dụng

1.  Truy cập `http://localhost:3000`.
2.  Dán một URL dài vào ô nhập liệu và nhấn nút "Shorten".
3.  URL rút gọn sẽ xuất hiện bên dưới.
4.  Nhấp vào liên kết ngắn hoặc dán nó vào thanh địa chỉ của trình duyệt để được chuyển hướng đến URL gốc.

## Phase 1: Kiến Trúc Monolith Cơ Bản (Đạt mức Pass)

- [x] **Ứng dụng & Cấu trúc**: Hoàn thành ứng dụng monolith với kiến trúc Clean Architecture.
- [x] **Tính năng**: Sinh mã code, chuyển hướng, validation, lưu trữ, và RESTful API.
- [x] **Web UI**: Xây dựng giao diện người dùng đơn giản bằng React.
- [ ] **DevOps & CI/CD**: Các bước tiếp theo sẽ bao gồm Docker, Unit Tests và GitHub Actions.
