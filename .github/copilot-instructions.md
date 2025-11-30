# Copilot Instructions

## Project Overview

This is an **ASP.NET Core 8.0** Web API project demonstrating Data Protection with Entity Framework Core and SQL Server in Docker.

## Technology Stack

- **.NET 8.0** / ASP.NET Core Web API
- **Entity Framework Core 8.0** with SQL Server provider
- **ASP.NET Core Data Protection API** with EF Core key persistence
- **Docker Compose** with SQL Server 2022
- **TPH (Table-Per-Hierarchy)** pattern for entity inheritance

## Project Structure

- `data_protection_with_EF/` - Main Web API project
- `data_protection_common/` - Shared library with:
  - `Entities/` - Entity classes (DataSource hierarchy)
  - `DTOs/` - Data Transfer Objects
  - `Services/` - Business logic (DataSourceService, CryptographyService, DataProtectionCryptographyService)
  - `Extensions/` - Extension methods (encryption, mappings, service collection)
  - `Mappings/` - Entity-DTO mappings
  - `SqlMigrations/` - EF Core migrations with DI support

## Coding Conventions

### No Magic Strings

Always use `nameof()` instead of hardcoded strings:

```csharp
// ✅ Good
entity.Property(e => e.ConnectionString).HasColumnName(nameof(DatabaseDataSource.ConnectionString));

// ❌ Bad
entity.Property(e => e.ConnectionString).HasColumnName("ConnectionString");
```

### Fluent API Pattern

Use fluent API pattern for method chaining:

```csharp
// ✅ Good - single line with fluent API
var entity = dto.ToEntity().EncryptSensitiveData(_cryptographyService);
return dataSources.DecryptSensitiveData(_cryptographyService).ToDto();

// ❌ Bad - multiple statements
var entity = dto.ToEntity();
entity.EncryptSensitiveData(_cryptographyService);
```

### Extension Methods

- Extension methods should return the entity for fluent chaining
- Use generic type constraints where applicable: `where T : DataSource`

### Comments

- All comments must be in **English**
- Use XML documentation for public APIs

## Database Migrations

### Creating Migrations

Use the batch script in `data_protection_common/` folder:

```powershell
cd data_protection_common
.\add_sql_migration.bat <MigrationName>
```

Example:
```powershell
.\add_sql_migration.bat AddNewColumn
```

### Migration Location

Migrations are stored in `data_protection_common/SqlMigrations/` folder.

### Auto-Migration

Migrations are automatically applied at application startup in `Program.cs`.

## Encryption

### ASP.NET Core Data Protection API

The project uses **ASP.NET Core Data Protection API** for encrypting sensitive fields. Keys are persisted in the database using `IDataProtectionKeyContext`.

**Key components:**
- `IDataProtectionCryptographyService` / `DataProtectionCryptographyService` - Main encryption service using Data Protection API
- `ICryptographyService` / `CryptographyService` - Legacy AES-256-CBC service (kept for migration purposes)
- `ApplicationDbContext` implements `IDataProtectionKeyContext` for key persistence
- `DataProtectionKeys` table stores encryption keys

**Encrypted fields per entity type:**
- **UrlDataSource**: Password, ApiKey, BearerToken
- **FtpDataSource**: Password, PrivateKeyPath
- **DatabaseDataSource**: ConnectionString
- **AzureBlobDataSource**: ConnectionString
- **S3DataSource**: AccessKey, SecretKey

### Configuration

Data Protection is configured in `ServiceCollectionExtensions.AddDataProtectionApiWithKeysStoredInDbContext()`:
```csharp
services.AddDataProtection()
    .SetApplicationName("DataProtectionExample")
    .SetDefaultKeyLifetime(TimeSpan.FromDays(10))
    .PersistKeysToDbContext<ApplicationDbContext>();
```

**Key rotation:**
- Keys rotate automatically every **10 days** (default is 90 days, minimum is 7 days)
- Old keys remain valid for decryption (backward compatibility)
- New data is encrypted with the latest key

## Documentation & Context

### Always Use Context7

When looking up library documentation, **always use `io.github.upstash/context7`** MCP server:

1. First resolve library ID:
   ```
   mcp_io_github_ups_resolve-library-id with libraryName
   ```

2. Then get documentation:
   ```
   mcp_io_github_ups_get-library-docs with context7CompatibleLibraryID
   ```

Example for Entity Framework Core:
```
1. Resolve: libraryName = "entity framework core"
2. Get docs: context7CompatibleLibraryID = "/dotnet/efcore"
```

## Docker

To run the application with SQL Server:

```powershell
docker-compose up -d
```

Connection string is configured in `appsettings.json`.
