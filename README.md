# ASP.NET Core Data Protection Example

A comprehensive example demonstrating **ASP.NET Core Data Protection API** with Entity Framework Core key persistence and SQL Server in Docker.

## Features

- 🔐 **ASP.NET Core Data Protection API** with EF Core key persistence
- 🔄 **Automatic key rotation** (configurable, default 10 days)
- 🗄️ **SQL Server** with Docker Compose
- 📦 **Entity Framework Core 8.0** with TPH (Table-Per-Hierarchy) pattern
- 🔒 **Encryption migration** from legacy AES-256-CBC to Data Protection API
- 🧪 **Unit tests** for key rotation and encryption functionality

## Project Structure

```
├── data_protection_with_EF/          # Main Web API project
│   ├── Controllers/                  # API controllers
│   └── Program.cs                    # Application entry point
├── data_protection_common/           # Shared library
│   ├── Entities/                     # Entity classes (DataSource hierarchy)
│   ├── DTOs/                         # Data Transfer Objects
│   ├── Services/                     # Business logic
│   │   ├── CryptographyService.cs              # Legacy AES-256-CBC encryption
│   │   └── DataProtectionCryptographyService.cs # New Data Protection API encryption
│   ├── Extensions/                   # Extension methods
│   ├── Mappings/                     # Entity-DTO mappings
│   └── SqlMigrations/                # EF Core migrations with DI support
├── data_protection_with_EF_tests/    # Unit tests
└── docker-compose.yml                # Docker configuration
```

## Encryption Strategy

This project demonstrates a **migration strategy** from custom encryption to ASP.NET Core Data Protection API.

### Phase 1: Initial Data Encryption (Legacy)

Sample data is initially encrypted using `CryptographyService` which implements **AES-256-CBC** encryption with PBKDF2-SHA256 key derivation (600,000 iterations per OWASP 2023 recommendations).

```
┌─────────────────────────────────────────────────────────────┐
│  SQL Migration: 20251129212137_Insert_sample_DataSources    │
│                                                             │
│  1. Create CryptographyService instance                     │
│  2. Encrypt sensitive fields (Password, ApiKey, etc.)       │
│  3. Insert encrypted data into DataSources table            │
└─────────────────────────────────────────────────────────────┘
```

### Phase 2: Migration to Data Protection API

A subsequent migration re-encrypts all data using the new `DataProtectionCryptographyService` which leverages ASP.NET Core Data Protection API with automatic key management.

```
┌─────────────────────────────────────────────────────────────┐
│  SQL Migration: 20251130185512_Migrate_data_to_DataProtection│
│                                                             │
│  For each encrypted record:                                 │
│  1. Decrypt using legacy CryptographyService (AES-256-CBC)  │
│  2. Re-encrypt using DataProtectionCryptographyService      │
│  3. Update record with new encrypted value                  │
└─────────────────────────────────────────────────────────────┘
```

### Encrypted Fields by Entity Type

| Entity Type | Encrypted Fields |
|-------------|------------------|
| UrlDataSource | Password, ApiKey, BearerToken |
| FtpDataSource | Password, PrivateKeyPath |
| DatabaseDataSource | ConnectionString |
| AzureBlobDataSource | ConnectionString |
| S3DataSource | AccessKey, SecretKey |

## Key Rotation

Keys are automatically rotated every **10 days** (configurable). The Data Protection system:

- ✅ Generates new encryption keys automatically
- ✅ Keeps old keys valid for decryption (backward compatibility)
- ✅ Uses the latest key for new encryption operations
- ✅ Stores keys in the `DataProtectionKeys` database table

```csharp
services.AddDataProtection()
    .SetApplicationName("DataProtectionExample")
    .SetDefaultKeyLifetime(TimeSpan.FromDays(10))
    .PersistKeysToDbContext<ApplicationDbContext>();
```

## Dependency Injection in Migrations

This project demonstrates an advanced technique for **injecting services into EF Core migrations** using a custom `CustomMigrationAssembly`. This allows migrations to:

- Access `ApplicationDbContext` for database operations
- Use encryption services for data transformation
- Perform complex data migrations with business logic

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- Docker Desktop

### Running the Application

1. **Start SQL Server container:**
   ```powershell
   docker-compose up -d
   ```

2. **Run the application:**
   ```powershell
   cd data_protection_with_EF
   dotnet run
   ```

3. **Access Swagger UI:**
   ```
   https://localhost:5001/swagger
   ```

### Running Tests

```powershell
dotnet test
```

## Technology Stack

- **.NET 8.0** / ASP.NET Core Web API
- **Entity Framework Core 8.0** with SQL Server provider
- **ASP.NET Core Data Protection API** with EF Core key persistence
- **Docker Compose** with SQL Server 2022
- **xUnit** for unit testing

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/datasources` | Get all data sources |
| GET | `/api/datasources/{id}` | Get data source by ID |
| POST | `/api/datasources/url` | Create URL data source |
| POST | `/api/datasources/file` | Create File data source |
| POST | `/api/datasources/ftp` | Create FTP data source |
| POST | `/api/datasources/database` | Create Database data source |
| POST | `/api/datasources/azureblob` | Create Azure Blob data source |
| POST | `/api/datasources/s3` | Create S3 data source |
| PUT | `/api/datasources/{id}` | Update data source |
| DELETE | `/api/datasources/{id}` | Delete data source |

## License

This project is licensed under the MIT License - see the [LICENSE.txt](LICENSE.txt) file for details.