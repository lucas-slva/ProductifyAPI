# Productify API 📦

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=flat-square&logo=dotnet&logoColor=white)
![Entity Framework Core](https://img.shields.io/badge/Entity%20Framework%20Core-9.0-512BD4?style=flat-square&logo=dotnet&logoColor=white)
![Clean Architecture](https://img.shields.io/badge/Clean%20Architecture-Pattern-4CAF50?style=flat-square)
![JWT](https://img.shields.io/badge/JWT-Secure-000000?style=flat-square&logo=jsonwebtokens&logoColor=white)
![Swagger](https://img.shields.io/badge/Swagger-UI-85EA2D?style=flat-square&logo=swagger&logoColor=black)
![xUnit](https://img.shields.io/badge/xUnit-Testing-5B2A89?style=flat-square&logo=xunit&logoColor=white)
![Moq](https://img.shields.io/badge/Moq-Mocking-512BD4?style=flat-square&logo=dotnet&logoColor=white)
![FluentValidation](https://img.shields.io/badge/FluentValidation-9.0-0072C6?style=flat-square&logo=azuredevops&logoColor=white)

## 📖 Overview
A robust RESTful API built with ASP.NET Core and Entity Framework Core to manage products and categories with full CRUD operations, authentication, and authorization using JWT.

## 🚀 Features
- ✅ Full CRUD operations for **Products** and **Categories**.
- ✅ JWT Authentication and Authorization.
- ✅ **Entity Framework Core** with SQLite.
- ✅ **AutoMapper** for clean DTO mappings.
- ✅ **FluentValidation** for request validation.
- ✅ **Swagger UI** for API documentation.
- ✅ Complete Unit Testing with **xUnit** and **Moq**.

&nbsp;

## 🏗️ **Project Architecture**
- **src/**: Core API and domain projects.
  - `Productify.Api` – Entry point for the API.
  - `Productify.Application` – Business logic and service layer.
  - `Productify.Domain` – Entity definitions and core logic.
  - `Productify.Infrastructure` – Data access layer (EF Core, Repositories).
    
- **tests/**: Unit tests with xUnit.
- **.github/**: CI/CD with GitHub Actions.

&nbsp;

## 📦 **Tech Stack**
- **.NET 9.0**
- **Entity Framework Core**
- **SQLite**
- **JWT Authentication**
- **AutoMapper**
- **FluentValidation**
- **xUnit & Moq**

&nbsp;

## 🔧 **Getting Started**
### Prerequisites:
- .NET SDK 9.0+
- SQLite Installed (optional for local inspection)

### Running the Project:
```bash
# Clone the repository
git clone https://github.com/lucas-slva/ProductifyAPI.git
cd ProductifyAPI

# Run the API
dotnet build
dotnet run --project src/Productify.Api
```

### Run Tests:
```bash
dotnet test tests/Productify.Tests
```

&nbsp;

## 🔑 **Authentication:**
1. Register a user using `/api/auth/register`.
2. Log in using `/api/auth/login` and retrieve a JWT token.
3. Use the token to authorize secure endpoints in Swagger (`Authorize` button on top right).

&nbsp;

## ✅ **API Endpoints (Example)**
### **Categories**
- `GET /api/category/{id}`
- `POST /api/category`
- `PUT /api/category/{id}`
- `DELETE /api/category/{id}`

### **Products**
- `GET /api/product/{id}`
- `POST /api/product`
- `PUT /api/product/{id}`
- `DELETE /api/product/{id}`

&nbsp;

## 📊 **Testing Coverage**
The following tests are implemented with **xUnit** and **Moq**:
- ✅ **CategoryControllerTests** (100% coverage)
- ✅ **ProductControllerTests** (100% coverage)

&nbsp;

## 📧 **Contact**
My LinkedIn: [Lucas Silva](https://www.linkedin.com/in/-lucassva/)
