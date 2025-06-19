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
psql -U postgres -c "CREATE DATABASE BookstoreOrders;"
psql -U postgres -c "CREATE DATABASE BookstorePayments;"
psql -U postgres -c "CREATE USER myuser WITH PASSWORD 'password' SUPERUSER;"
psql -U postgres -c "GRANT ALL PRIVILEGES ON DATABASE BookstoreProducts TO myuser;"
psql -U postgres -c "GRANT ALL PRIVILEGES ON DATABASE BookstoreUsers TO myuser;"
psql -U postgres -c "GRANT ALL PRIVILEGES ON DATABASE BookstoreOrders TO myuser;"
psql -U postgres -c "GRANT ALL PRIVILEGES ON DATABASE BookstorePayments TO myuser;"
```
### 🚀 Service Startup Order
```powershell
dotnet tool install --global dotnet-ef
```

Run in separate terminals:

1. Users Service (Auth)
```powershell
cd .\src\Services\Users\Users.API\
dotnet ef migrations add InitialCreate --project ../Users.Infrastructure
dotnet ef database update
dotnet run
```

2. Products Service
```powershell
cd .\src\Services\Orders\Orders.API\Products\Products.API\
dotnet ef migrations add InitialCreate --project ../Products.Infrastructure
dotnet ef database update
dotnet run
```

3. Orders Service
```powershell
cd .\src\Services\Orders\Orders.API\

# First-time setup (run once)
dotnet ef migrations add InitialCreate --project ../Orders.Infrastructure
dotnet ef database update

# Run the service
dotnet run
```

4. Payments Service
```powershell
cd .\src\Services\Payments\Payments.API\

# First-time setup (run once)
dotnet ef migrations add InitialCreate --project ../Payments.Infrastructure
dotnet ef database update

# Run the service
dotnet run
```

## 🌐 API Ports

| Service  | Port | Swagger URL                           |
|----------|------|----------------------------------------|
| Users    | 5001 | http://localhost:5001/swagger          |
| Products | 5008 | http://localhost:5008/swagger          |
| Orders   | 5003 | http://localhost:5003/swagger          |
| Payment  | 5004 | http://localhost:5004/swagger          |
