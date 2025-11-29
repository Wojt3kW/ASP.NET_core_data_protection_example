using data_protection_common.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace data_protection_common.Services
{
    /// <summary>
    /// Database seeder for populating sample data
    /// </summary>
    public interface IDbSeeder
    {
        Task SeedAsync();
    }

    /// <summary>
    /// Seeds the database with sample DataSource entries
    /// </summary>
    public class DbSeeder : IDbSeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly IDataSourceService _dataSourceService;
        private readonly ILogger<DbSeeder> _logger;

        public DbSeeder(
            ApplicationDbContext context,
            IDataSourceService dataSourceService,
            ILogger<DbSeeder> logger)
        {
            _context = context;
            _dataSourceService = dataSourceService;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            // Skip seeding if data already exists
            if (await _context.DataSources.AnyAsync())
            {
                _logger.LogInformation("Database already contains data. Skipping seed.");
                return;
            }

            _logger.LogInformation("Seeding database with sample data...");

            await SeedUrlDataSourcesAsync();
            await SeedFileDataSourcesAsync();
            await SeedFtpDataSourcesAsync();
            await SeedDatabaseDataSourcesAsync();
            await SeedAzureBlobDataSourcesAsync();
            await SeedS3DataSourcesAsync();

            _logger.LogInformation("Database seeding completed.");
        }

        private async Task SeedUrlDataSourcesAsync()
        {
            var urlSources = new[]
            {
                new CreateUrlDataSourceDto
                {
                    Name = "Public REST API",
                    Description = "Sample public REST API endpoint",
                    Url = "https://api.example.com/data",
                    HttpMethod = "GET",
                    AuthType = "None",
                    TimeoutSeconds = 30,
                    IsEnabled = true
                },
                new CreateUrlDataSourceDto
                {
                    Name = "Secured API with Bearer Token",
                    Description = "API endpoint requiring JWT authentication",
                    Url = "https://api.secure-example.com/v1/resources",
                    HttpMethod = "GET",
                    Headers = "{\"Accept\": \"application/json\", \"X-Custom-Header\": \"value\"}",
                    AuthType = "Bearer",
                    BearerToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.sample.token",
                    TimeoutSeconds = 60,
                    IsEnabled = true
                },
                new CreateUrlDataSourceDto
                {
                    Name = "API with Basic Auth",
                    Description = "Legacy API with basic authentication",
                    Url = "https://legacy.example.com/api/export",
                    HttpMethod = "POST",
                    AuthType = "Basic",
                    Username = "api_user",
                    Password = "SecureP@ssw0rd!",
                    TimeoutSeconds = 120,
                    IsEnabled = false
                },
                new CreateUrlDataSourceDto
                {
                    Name = "Weather API",
                    Description = "External weather data provider",
                    Url = "https://api.weather.com/v2/current",
                    HttpMethod = "GET",
                    AuthType = "ApiKey",
                    ApiKey = "wth_api_key_12345abcdef",
                    TimeoutSeconds = 15,
                    IsEnabled = true
                }
            };

            foreach (var dto in urlSources)
            {
                await _dataSourceService.CreateUrlAsync(dto);
                _logger.LogDebug("Created URL data source: {Name}", dto.Name);
            }
        }

        private async Task SeedFileDataSourcesAsync()
        {
            var fileSources = new[]
            {
                new CreateFileDataSourceDto
                {
                    Name = "Configuration JSON",
                    Description = "Application configuration file",
                    FilePath = "/data/config/settings.json",
                    FileType = "JSON",
                    Encoding = "UTF-8",
                    IsEnabled = true
                },
                new CreateFileDataSourceDto
                {
                    Name = "Sales Data CSV",
                    Description = "Monthly sales export",
                    FilePath = "/data/exports/sales_2024.csv",
                    FileType = "CSV",
                    Encoding = "UTF-8",
                    Delimiter = ",",
                    HasHeader = true,
                    IsEnabled = true
                },
                new CreateFileDataSourceDto
                {
                    Name = "Legacy XML Feed",
                    Description = "XML data feed from legacy system",
                    FilePath = "/data/feeds/legacy_feed.xml",
                    FileType = "XML",
                    Encoding = "UTF-8",
                    IsEnabled = false
                }
            };

            foreach (var dto in fileSources)
            {
                await _dataSourceService.CreateFileAsync(dto);
                _logger.LogDebug("Created File data source: {Name}", dto.Name);
            }
        }

        private async Task SeedFtpDataSourcesAsync()
        {
            var ftpSources = new[]
            {
                new CreateFtpDataSourceDto
                {
                    Name = "Partner FTP Server",
                    Description = "Daily data exchange with partner",
                    Host = "ftp.partner.example.com",
                    Port = 21,
                    Username = "data_exchange",
                    Password = "FtpP@ss2024!",
                    RemotePath = "/incoming/daily",
                    UseSftp = false,
                    UsePassiveMode = true,
                    IsEnabled = true
                },
                new CreateFtpDataSourceDto
                {
                    Name = "Secure SFTP Server",
                    Description = "Secure file transfer with SSH key",
                    Host = "sftp.secure-example.com",
                    Port = 22,
                    Username = "sftp_user",
                    Password = "SftpSecure!Key",
                    RemotePath = "/uploads",
                    UseSftp = true,
                    UsePassiveMode = false,
                    PrivateKeyPath = "/keys/sftp_private_key.pem",
                    IsEnabled = true
                }
            };

            foreach (var dto in ftpSources)
            {
                await _dataSourceService.CreateFtpAsync(dto);
                _logger.LogDebug("Created FTP data source: {Name}", dto.Name);
            }
        }

        private async Task SeedDatabaseDataSourcesAsync()
        {
            var dbSources = new[]
            {
                new CreateDatabaseDataSourceDto
                {
                    Name = "Production SQL Server",
                    Description = "Main production database",
                    ConnectionString = "Server=prod-sql.example.com;Database=ProductionDB;User Id=app_reader;Password=Pr0dR3ad3r!;Encrypt=True;",
                    DatabaseType = "SqlServer",
                    Schema = "dbo",
                    Query = "SELECT * FROM Customers WHERE IsActive = 1",
                    CommandTimeoutSeconds = 60,
                    IsEnabled = true
                },
                new CreateDatabaseDataSourceDto
                {
                    Name = "Analytics PostgreSQL",
                    Description = "Analytics data warehouse",
                    ConnectionString = "Host=analytics.example.com;Database=analytics;Username=analyst;Password=An@lyt1cs2024;",
                    DatabaseType = "PostgreSQL",
                    Schema = "public",
                    Query = "SELECT * FROM sales_summary",
                    CommandTimeoutSeconds = 120,
                    IsEnabled = true
                },
                new CreateDatabaseDataSourceDto
                {
                    Name = "Legacy MySQL Database",
                    Description = "Legacy system database (read-only)",
                    ConnectionString = "Server=legacy-mysql.example.com;Database=legacy_app;Uid=legacy_reader;Pwd=L3g@cyR34d;",
                    DatabaseType = "MySQL",
                    Query = "orders",
                    CommandTimeoutSeconds = 30,
                    IsEnabled = false
                }
            };

            foreach (var dto in dbSources)
            {
                await _dataSourceService.CreateDatabaseAsync(dto);
                _logger.LogDebug("Created Database data source: {Name}", dto.Name);
            }
        }

        private async Task SeedAzureBlobDataSourcesAsync()
        {
            var azureSources = new[]
            {
                new CreateAzureBlobDataSourceDto
                {
                    Name = "Azure Document Storage",
                    Description = "Document archive in Azure Blob",
                    ConnectionString = "DefaultEndpointsProtocol=https;AccountName=examplestorage;AccountKey=abc123xyz456/BASE64KEY==;EndpointSuffix=core.windows.net",
                    ContainerName = "documents",
                    BlobPrefix = "archive/2024/",
                    IsEnabled = true
                },
                new CreateAzureBlobDataSourceDto
                {
                    Name = "Azure Backup Container",
                    Description = "Database backups storage",
                    ConnectionString = "DefaultEndpointsProtocol=https;AccountName=backupstorage;AccountKey=backup789key/SECRETKEY==;EndpointSuffix=core.windows.net",
                    ContainerName = "db-backups",
                    BlobName = "latest.bak",
                    IsEnabled = true
                }
            };

            foreach (var dto in azureSources)
            {
                await _dataSourceService.CreateAzureBlobAsync(dto);
                _logger.LogDebug("Created Azure Blob data source: {Name}", dto.Name);
            }
        }

        private async Task SeedS3DataSourcesAsync()
        {
            var s3Sources = new[]
            {
                new CreateS3DataSourceDto
                {
                    Name = "AWS Data Lake",
                    Description = "Main data lake bucket",
                    BucketName = "company-data-lake",
                    AccessKey = "AKIAIOSFODNN7EXAMPLE",
                    SecretKey = "wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY",
                    Region = "us-east-1",
                    Prefix = "raw-data/",
                    IsEnabled = true
                },
                new CreateS3DataSourceDto
                {
                    Name = "AWS Log Archive",
                    Description = "Application logs archive",
                    BucketName = "app-logs-archive",
                    AccessKey = "AKIAI44QH8DHBEXAMPLE",
                    SecretKey = "je7MtGbClwBF/2Zp9Utk/h3yCo8nvbEXAMPLEKEY",
                    Region = "eu-west-1",
                    Prefix = "logs/2024/",
                    IsEnabled = true
                }
            };

            foreach (var dto in s3Sources)
            {
                await _dataSourceService.CreateS3Async(dto);
                _logger.LogDebug("Created S3 data source: {Name}", dto.Name);
            }
        }
    }
}
