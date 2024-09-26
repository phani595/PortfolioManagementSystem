# Financial Portfolio Management System

## Overview

This project built with **ASP.NET Core Web API**, **Entity Framework Core**, and **Clean Architecture** principles. The application allows users to manage financial portfolios, assets, and transactions with features like CRUD operations, portfolio performance calculations, and background processing for asset price updates.

## Architecture

The solution follows the **Clean Architecture** pattern, which enforces a clear separation of concerns by organizing the project into different layers:

- **Presentation**: The ASP.NET Core Web API project that exposes endpoints for interacting with portfolios, assets, and transactions.
- **Application**: Contains business logic, service interfaces, and contracts.
- **Domain**: Contains the core business models and entities, free of dependencies on any other layers.
- **Infrastructure**: Implements data persistence using **Entity Framework Core** and external dependencies, such as database context (DbContext).

---

## Features

- **CRUD operations** for Portfolios, Assets, and Transactions
- **User portfolio management** and transaction history tracking
- **Pagination** and **filtering** for retrieving transactions
- **Background processing** using **Hangfire** to periodically update asset prices
- **JWT authentication** and role-based authorization (Admin, Advisor, User)
- **Unit and integration testing** using **xUnit**

---

## Technologies Used

- **ASP.NET Core Web API**
- **Entity Framework Core**
- **SQL Server**
- **Clean Architecture**
- **Hangfire** 
- **JWT Authentication**
- **FluentValidation** 
- **xUnit** 

---

## Project Setup

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (or your preferred IDE)

---

### Step-by-Step Guide

1. **Clone the repository**:
    ```bash
    git clone https://github.com/your-username/portfolio-management-system.git
    cd portfolio-management-system
    ```

2. **Restore the dependencies**:
    ```bash
    dotnet restore
    ```

3. **Update the database connection string** in the `appsettings.json` file located in the `Presentation` project:
    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Server=your_server;Database=PortfolioManagementDb;Trusted_Connection=True;"
      }
    }
    ```

4. **Apply database migrations** to create the necessary tables:
    ```bash
    dotnet ef database update --project Infrastructure
    ```

5. **Run the application**:
    ```bash
    dotnet run --project Presentation
    ```

6. The API will be available at: `https://localhost:<port>/`.

7. Swagger will be avilable at : `https://localhost:<port>/swagger/index.html`.

7. Hangfire dashboard at : `https://localhost:<port>/hangfire`.

---

### Project Structure

```bash
├── Application
│   ├── Interfaces         # Service and repository interfaces
│   ├── Services           # Application services and business logic
│   └── ServiceRegistration.cs   # DI setup for Application layer
├── Domain
│   ├── Entities           # Core business entities (Portfolio, Asset, Transaction, User)
│   └── ValueObjects       # Domain-driven value objects
├── Infrastructure
│   ├── Persistence        # Entity Framework Core DbContext and repositories
│   └── Repositories       # Implementation of repository interfaces
├── Presentation
│   ├── Controllers        # Web API controllers
│   └── Program.cs         # Main entry point for the API project
└── README.md              # Project documentation