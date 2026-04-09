#  LMNZ Library

A full-stack Library Management System built with **ASP.NET Core** and a **Spectre.Console** terminal interface.

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)
![C#](https://img.shields.io/badge/C%23-13-239120?logo=csharp)
![SQL Server](https://img.shields.io/badge/SQL%20Server-Express-CC2927?logo=microsoftsqlserver)

##  Features

- **Books Management** — Add, update, list, and delete books
- **Users Management** — Register, update, and remove users with email validation
- **Loan System** — Borrow and return books with automatic availability tracking
- **Hacker-style Terminal UI** — Built with Spectre.Console for a visually stunning CLI experience
- **RESTful API** — Clean API with Swagger documentation
- **Global Error Handling** — Middleware-based exception management

## 🏗️ Architecture

```
LibraryAPI (Backend)          LibraryCLI (Frontend)
┌──────────────────┐          ┌──────────────────┐
│  Controllers     │◄────────►│  ApiClient       │
│  Services        │   HTTP   │  Spectre.Console │
│  EF Core + SQL   │          │  Interactive UI  │
└──────────────────┘          └──────────────────┘
```

The project follows a **Client-Server architecture**:
- **LibraryAPI**: ASP.NET Core Web API with Entity Framework Core and SQL Server Express
- **LibraryCLI**: Terminal-based UI client using Spectre.Console

##  Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

### Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/khalilami2005-ctrl/LMNZ-Library.git
   cd LMNZ-Library
   ```

2. **Apply database migrations**
   ```bash
   dotnet ef database update
   ```

3. **Run the API**
   ```bash
   dotnet run
   ```

4. **Run the CLI** (in a separate terminal)
   ```bash
   dotnet run --project LibraryCLI
   ```

Or simply double-click `Avvia_Libreria.bat` to launch both automatically!

##  Project Structure

```
├── Controllers/          # API endpoints (Books, Users, Loans)
├── Services/             # Business logic layer
│   └── Interfaces/       # Service contracts
├── Models/               # Entity models (Book, User, Loan)
├── DTOs/                 # Data Transfer Objects
├── Data/                 # Database context (EF Core)
├── Middleware/            # Global error handling
├── Migrations/           # EF Core migrations
├── LibraryCLI/           # Terminal client application
│   ├── Models/           # CLI-side DTOs
│   └── Services/         # API client service
└── Avvia_Libreria.bat    # One-click launcher
```

##  Tech Stack

| Layer      | Technology              |
|------------|-------------------------|
| Backend    | ASP.NET Core (.NET 10)  |
| ORM        | Entity Framework Core   |
| Database   | SQL Server Express      |
| Frontend   | Spectre.Console (CLI)   |
| API Docs   | Swagger / OpenAPI       |

##  Author

**Lmnz** — [@khalilami2005-ctrl](https://github.com/khalilami2005-ctrl)

##  License

This project is open source and available under the [MIT License](LICENSE).
