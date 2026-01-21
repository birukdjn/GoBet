# GoBet API 🚌

GoBet is a professional-grade .NET 10 Web API designed to modernize **Public Autobus Transportation**. Built on the principles of **Clean Architecture**, this platform manages real-time route scheduling, passenger ticketing, and secure fleet management.



---

## 🚀 Mission & Capabilities
GoBet aims to streamline the public transport experience by providing a scalable backend for:
* **Passenger Management:** Secure profiles and history.
* **Route Logic:** Automated scheduling and station management.
* **Real-time Access:** Lightweight API endpoints for mobile and web clients.

## 🛠 Tech Stack
* **Framework:** .NET 10 (ASP.NET Core)
* **Data Access:** Entity Framework Core (Code First)
* **Database:** SQL Server
* **Security:** ASP.NET Core Identity + JWT (JSON Web Tokens)
* **Architecture:** Domain-Driven Design (DDD) & Repository Pattern

---

## 🏗 Project structure
The solution is divided into four layers to ensure high maintainability and testability:

- **GoBet.Domain**: The heart of the system. Contains core entities (Bus, Route, Ticket), custom exceptions, and repository interfaces.
- **GoBet.Application**: Coordinates task execution. Contains DTOs, AutoMapper profiles, and business logic interfaces.
- **GoBet.Infrastructure**: Handles external concerns. Includes DB Context, Identity implementation, and third-party services (JWT/Email).
- **GoBet.Api**: The presentation layer. Handles HTTP request routing, middleware, and Swagger documentation.

---

## 💻 Developer Getting Started

### Prerequisites
* [.NET 10 SDK](https://dotnet.microsoft.com/download)
* [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or LocalDB)
* [EF Core Tools](https://learn.microsoft.com/en-us/ef/core/cli/dotnet) (`dotnet tool install --global dotnet-ef`)

### Local Setup
1. **Clone the repository:**
   ```bash
   git clone [https://github.com/birukdjn/GoBet.git](https://github.com/birukdjn/GoBet.git)
   cd GoBet
   ```
2. **Initialize User Secrets: Never commit sensitive keys. Initialize and set your local secrets:**
```bash
cd GoBet.Api
dotnet user-secrets init
dotnet user-secrets set "JWT:Secret" "A_Long_Random_Security_Key_32_Chars"
dotnet user-secrets set "Authentication:Google:ClientSecret" "YourGoogleSecret"
dotnet user-secrets set "Authentication:Google:ClientId" "Your_ClientId"
dotnet user-secrets set "Authentication:Facebook:AppId" "Your_AppId"
dotnet user-secrets set "Authentication:Facebook:AppSecret" "Your_App_Secret"

```
3. **Apply Database Migration:**
Ensure your connection string in appsettings.Development.json points to your local SQL instance:
```bash
dotnet ef database update --project ../GoBet.Infrastructure --startup-project .
```
4. **Run the App:**
```bash
dotnet run
```
---

## 🔐 Authentication Flow
The **GoBet API** provides a secure and flexible authentication system:
* **Identity:** Managed by ASP.NET Core Identity with custom ApplicationUser.
* **Traditional:** Standard registration and login using email and hashed passwords via ASP.NET Core Identity.
* **Social:** Support for external providers including **Google** and **Facebook** OAuth2.
* **Password Recovery:** Secure, token-based "Forgot Password" and "Reset Password" workflow.
* Role-Based Access (RBAC): Distinct permissions for Passenger, Driver, and Admin.


## 📝 API Documentation
When the application is running, you can explore, test, and interact with the API endpoints using the Swagger UI:

> **URL:** [http://localhost:5000/swagger](http://localhost:5000/swagger)

---

## 🤝 Contributing
We welcome contributions to the project! Please follow these steps:

1.  **Create a Feature Branch**
    ```bash
    git checkout -b feature/AmazingFeature
    ```
2.  **Commit your Changes**
    ```bash
    git commit -m 'feat: Add some AmazingFeature'
    ```
3.  **Push to the Branch**
    ```bash
    git push origin feature/AmazingFeature
    ```
4.  **Open a Pull Request** against the main branch.

---