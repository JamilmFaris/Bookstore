# 📚 Online Library Microservices App

This repository hosts a **microservices-based application** designed to support an online library platform. The system is composed of four key services:

- **Products**
- **Orders**
- **Users**
- **Payments**

## 🖥️ Running the Application on Windows

Follow these instructions to get the application running on a Windows environment.

### ✅ Prerequisites

Make sure the following tools are installed and configured properly on your machine:

- [PostgreSQL](https://www.postgresql.org/download/)
- [.NET SDK](https://dotnet.microsoft.com/en-us/download)

### 🛠️ Getting Started
ensure that postgresql is in the env variables.
Open a terminal or command prompt and execute the following commands:

```bash
psql -U postgres -c "CREATE USER myuser WITH PASSWORD 'password' SUPERUSER;"
dotnet build
dotnet tool install dotnet-ef
cd .\src\Services\Products\Products.API\
dotnet ef migrations add InitialCreate --project ../Products.Infrastructure/Products.Infrastructure.csproj
dotnet ef database update
dotnet run
