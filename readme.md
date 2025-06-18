# 📚 Online Library Microservices App

## 🛠️ Windows Setup with PostgreSQL

### ✅ Prerequisites
1. [PostgreSQL](https://www.postgresql.org/download/) (v14+)
2. [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
3. Ensure `psql` is in your PATH

### 🔐 Database Setup
```bash
# Create databases and user (run as postgres user)
psql -U postgres -c "CREATE DATABASE BookstoreProducts;"
psql -U postgres -c "CREATE DATABASE BookstoreUsers;"
psql -U postgres -c "CREATE USER myuser WITH PASSWORD 'password' SUPERUSER;"
psql -U postgres -c "GRANT ALL PRIVILEGES ON DATABASE BookstoreProducts TO myuser;"
psql -U postgres -c "GRANT ALL PRIVILEGES ON DATABASE BookstoreUsers TO myuser;"
```
### 🚀 Service Startup Order
Run in separate terminals:
1. Users Service (Auth)
```bash
cd src/Services/Users/Users.API/
dotnet ef migrations add InitialCreate --project ../Users.Infrastructure
dotnet ef database update
dotnet run
```
2. Products Service
```bash
cd src/Services/Products/Products.API/
dotnet ef migrations add InitialCreate --project ../Products.Infrastructure
dotnet ef database update
dotnet run
```

## 🌐 API Ports

| Service  | Port | Swagger URL                           |
|----------|------|----------------------------------------|
| Users    | 5000 | http://localhost:5000/swagger          |
| Products | 5001 | http://localhost:5001/swagger          |
