
# FinTech System

## Project Overview

This project implements a basic banking system backend designed for a small fintech startup. It supports multiple transaction types including deposit, withdrawal, and transfer. The system uses in-memory storage and can be extended for future features such as loan accounts and interest calculation. This system is intended as a prototype for managing customer accounts and transactions.

## Setup Instructions

To run the project locally, follow these steps:

1. **Prerequisites**:
   - .NET 8 SDK or later
   - Visual Studio or any code editor of your choice

2. **Clone the Repository**:
   ```
   git clone <repository-url>
   cd <project-directory>
   ```

3. **Build the Project**:
   Open the terminal or command prompt in the project directory and run:
   ```
   dotnet build
   ```

4. **Run the Project**:
   For running the project locally:
   ```
   dotnet run
   ```

   The API will be available on `https://localhost:5001`.

## Design Decisions

### Architecture

The system follows a **Clean Architecture** approach, dividing the project into different layers for maintainability and scalability:
- **Presentation Layer (API)**: This layer exposes the system's functionality via HTTP endpoints (REST API). It handles HTTP requests and delegates them to the business logic.
- **Domain Layer**: Contains the core business logic and models (e.g., Account, Transaction). This layer is independent of external frameworks or libraries.
- **Application Layer**: Contains the use case services (e.g., AccountService, TransactionService) that implement the business operations.
- **Infrastructure Layer**: Deals with data storage and external services. In this case, we use in-memory storage for simplicity.

### Key Design Patterns:
- **Repository Pattern**: Used to abstract the data storage and retrieval logic.
- **Dependency Injection**: Ensures that dependencies are injected into classes, promoting testability and flexibility.

## Assumptions

- **Account Numbers**: Each account has a unique account number.
- **In-memory Storage**: No database is used. Data is stored in memory for the purpose of this prototype.

## How to Test

For testing, we use **xUnit** framework. To run the tests:

1. Ensure the project is built.
2. Run the tests using the following command:
   ```
   dotnet test
   ```
