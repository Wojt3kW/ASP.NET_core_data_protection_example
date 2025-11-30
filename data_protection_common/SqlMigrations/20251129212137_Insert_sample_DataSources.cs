using data_protection_common.Services;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace data_protection_common.SqlMigrations
{
    /// <inheritdoc />
    public partial class Insert_sample_DataSources : Migration
    {
        private readonly ICryptographyService _cryptographyService = new CryptographyService();

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var now = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

            // URL Data Sources
            InsertUrlDataSource(migrationBuilder, now,
                name: "Public REST API",
                description: "Sample public REST API endpoint",
                url: "https://api.example.com/data",
                httpMethod: "GET",
                authType: "None",
                timeoutSeconds: 30,
                isEnabled: true);

            InsertUrlDataSource(migrationBuilder, now,
                name: "Secured API with Bearer Token",
                description: "API endpoint requiring JWT authentication",
                url: "https://api.secure-example.com/v1/resources",
                httpMethod: "GET",
                headers: "{\"Accept\": \"application/json\", \"X-Custom-Header\": \"value\"}",
                authType: "Bearer",
                bearerToken: "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.sample.token",
                timeoutSeconds: 60,
                isEnabled: true);

            InsertUrlDataSource(migrationBuilder, now,
                name: "API with Basic Auth",
                description: "Legacy API with basic authentication",
                url: "https://legacy.example.com/api/export",
                httpMethod: "POST",
                authType: "Basic",
                username: "api_user",
                password: "SecureP@ssw0rd!",
                timeoutSeconds: 120,
                isEnabled: false);

            InsertUrlDataSource(migrationBuilder, now,
                name: "Weather API",
                description: "External weather data provider",
                url: "https://api.weather.com/v2/current",
                httpMethod: "GET",
                authType: "ApiKey",
                apiKey: "wth_api_key_12345abcdef",
                timeoutSeconds: 15,
                isEnabled: true);

            // File Data Sources
            InsertFileDataSource(migrationBuilder, now,
                name: "Configuration JSON",
                description: "Application configuration file",
                filePath: "/data/config/settings.json",
                fileType: "JSON",
                encoding: "UTF-8",
                isEnabled: true);

            InsertFileDataSource(migrationBuilder, now,
                name: "Sales Data CSV",
                description: "Monthly sales export",
                filePath: "/data/exports/sales_2024.csv",
                fileType: "CSV",
                encoding: "UTF-8",
                delimiter: ",",
                hasHeader: true,
                isEnabled: true);

            InsertFileDataSource(migrationBuilder, now,
                name: "Legacy XML Feed",
                description: "XML data feed from legacy system",
                filePath: "/data/feeds/legacy_feed.xml",
                fileType: "XML",
                encoding: "UTF-8",
                isEnabled: false);

            // FTP Data Sources
            InsertFtpDataSource(migrationBuilder, now,
                name: "Partner FTP Server",
                description: "Daily data exchange with partner",
                host: "ftp.partner.example.com",
                port: 21,
                username: "data_exchange",
                password: "FtpP@ss2024!",
                remotePath: "/incoming/daily",
                useSftp: false,
                usePassiveMode: true,
                isEnabled: true);

            InsertFtpDataSource(migrationBuilder, now,
                name: "Secure SFTP Server",
                description: "Secure file transfer with SSH key",
                host: "sftp.secure-example.com",
                port: 22,
                username: "sftp_user",
                password: "SftpSecure!Key",
                remotePath: "/uploads",
                useSftp: true,
                usePassiveMode: false,
                privateKeyPath: "/keys/sftp_private_key.pem",
                isEnabled: true);

            // Database Data Sources
            InsertDatabaseDataSource(migrationBuilder, now,
                name: "Production SQL Server",
                description: "Main production database",
                connectionString: "Server=prod-sql.example.com;Database=ProductionDB;User Id=app_reader;Password=Pr0dR3ad3r!;Encrypt=True;",
                databaseType: "SqlServer",
                schema: "dbo",
                query: "SELECT * FROM Customers WHERE IsActive = 1",
                commandTimeoutSeconds: 60,
                isEnabled: true);

            InsertDatabaseDataSource(migrationBuilder, now,
                name: "Analytics PostgreSQL",
                description: "Analytics data warehouse",
                connectionString: "Host=analytics.example.com;Database=analytics;Username=analyst;Password=An@lyt1cs2024;",
                databaseType: "PostgreSQL",
                schema: "public",
                query: "SELECT * FROM sales_summary",
                commandTimeoutSeconds: 120,
                isEnabled: true);

            InsertDatabaseDataSource(migrationBuilder, now,
                name: "Legacy MySQL Database",
                description: "Legacy system database (read-only)",
                connectionString: "Server=legacy-mysql.example.com;Database=legacy_app;Uid=legacy_reader;Pwd=L3g@cyR34d;",
                databaseType: "MySQL",
                query: "orders",
                commandTimeoutSeconds: 30,
                isEnabled: false);

            // Azure Blob Data Sources
            InsertAzureBlobDataSource(migrationBuilder, now,
                name: "Azure Document Storage",
                description: "Document archive in Azure Blob",
                connectionString: "DefaultEndpointsProtocol=https;AccountName=examplestorage;AccountKey=abc123xyz456/BASE64KEY==;EndpointSuffix=core.windows.net",
                containerName: "documents",
                blobPrefix: "archive/2024/",
                isEnabled: true);

            InsertAzureBlobDataSource(migrationBuilder, now,
                name: "Azure Backup Container",
                description: "Database backups storage",
                connectionString: "DefaultEndpointsProtocol=https;AccountName=backupstorage;AccountKey=backup789key/SECRETKEY==;EndpointSuffix=core.windows.net",
                containerName: "db-backups",
                blobName: "latest.bak",
                isEnabled: true);

            // S3 Data Sources
            InsertS3DataSource(migrationBuilder, now,
                name: "AWS Data Lake",
                description: "Main data lake bucket",
                bucketName: "company-data-lake",
                accessKey: "AKIAIOSFODNN7EXAMPLE",
                secretKey: "wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY",
                region: "us-east-1",
                prefix: "raw-data/",
                isEnabled: true);

            InsertS3DataSource(migrationBuilder, now,
                name: "AWS Log Archive",
                description: "Application logs archive",
                bucketName: "app-logs-archive",
                accessKey: "AKIAI44QH8DHBEXAMPLE",
                secretKey: "je7MtGbClwBF/2Zp9Utk/h3yCo8nvbEXAMPLEKEY",
                region: "eu-west-1",
                prefix: "logs/2024/",
                isEnabled: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [DataSources]");
        }

        #region Helper Methods

        private void InsertUrlDataSource(MigrationBuilder migrationBuilder, string createdAt,
            string name, string description, string url, string httpMethod, string authType,
            int timeoutSeconds, bool isEnabled,
            string? headers = null, string? username = null, string? password = null,
            string? apiKey = null, string? bearerToken = null)
        {
            var encryptedPassword = EncryptIfNotEmpty(password);
            var encryptedApiKey = EncryptIfNotEmpty(apiKey);
            var encryptedBearerToken = EncryptIfNotEmpty(bearerToken);

            var sql = $@"
INSERT INTO [DataSources] 
    ([Name], [Description], [IsEnabled], [CreatedAt], [SourceType], 
     [Url], [HttpMethod], [Headers], [AuthType], [Username], [Password], [ApiKey], [BearerToken], [TimeoutSeconds])
VALUES 
    (N'{EscapeSql(name)}', N'{EscapeSql(description)}', {(isEnabled ? 1 : 0)}, '{createdAt}', 'Url',
     N'{EscapeSql(url)}', N'{EscapeSql(httpMethod)}', {NullOrString(headers)}, N'{EscapeSql(authType)}', 
     {NullOrString(username)}, {NullOrString(encryptedPassword)}, {NullOrString(encryptedApiKey)}, 
     {NullOrString(encryptedBearerToken)}, {timeoutSeconds})";

            migrationBuilder.Sql(sql);
        }

        private void InsertFileDataSource(MigrationBuilder migrationBuilder, string createdAt,
            string name, string description, string filePath, string fileType, string encoding,
            bool isEnabled, string? delimiter = null, bool? hasHeader = null)
        {
            var sql = $@"
INSERT INTO [DataSources] 
    ([Name], [Description], [IsEnabled], [CreatedAt], [SourceType], 
     [FilePath], [FileType], [Encoding], [Delimiter], [HasHeader])
VALUES 
    (N'{EscapeSql(name)}', N'{EscapeSql(description)}', {(isEnabled ? 1 : 0)}, '{createdAt}', 'File',
     N'{EscapeSql(filePath)}', N'{EscapeSql(fileType)}', N'{EscapeSql(encoding)}', 
     {NullOrString(delimiter)}, {(hasHeader.HasValue ? (hasHeader.Value ? 1 : 0).ToString() : "NULL")})";

            migrationBuilder.Sql(sql);
        }

        private void InsertFtpDataSource(MigrationBuilder migrationBuilder, string createdAt,
            string name, string description, string host, int port, string username, string password,
            string remotePath, bool useSftp, bool usePassiveMode, bool isEnabled,
            string? privateKeyPath = null)
        {
            var encryptedPassword = EncryptIfNotEmpty(password);
            var encryptedPrivateKeyPath = EncryptIfNotEmpty(privateKeyPath);

            var sql = $@"
INSERT INTO [DataSources] 
    ([Name], [Description], [IsEnabled], [CreatedAt], [SourceType], 
     [Host], [Port], [Username], [Password], [RemotePath], [UseSftp], [UsePassiveMode], [PrivateKeyPath])
VALUES 
    (N'{EscapeSql(name)}', N'{EscapeSql(description)}', {(isEnabled ? 1 : 0)}, '{createdAt}', 'Ftp',
     N'{EscapeSql(host)}', {port}, N'{EscapeSql(username)}', N'{EscapeSql(encryptedPassword)}', 
     N'{EscapeSql(remotePath)}', {(useSftp ? 1 : 0)}, {(usePassiveMode ? 1 : 0)}, {NullOrString(encryptedPrivateKeyPath)})";

            migrationBuilder.Sql(sql);
        }

        private void InsertDatabaseDataSource(MigrationBuilder migrationBuilder, string createdAt,
            string name, string description, string connectionString, string databaseType,
            string query, int commandTimeoutSeconds, bool isEnabled, string? schema = null)
        {
            var encryptedConnectionString = _cryptographyService.Encrypt(connectionString);

            var sql = $@"
INSERT INTO [DataSources] 
    ([Name], [Description], [IsEnabled], [CreatedAt], [SourceType], 
     [ConnectionString], [DatabaseType], [Schema], [Query], [CommandTimeoutSeconds])
VALUES 
    (N'{EscapeSql(name)}', N'{EscapeSql(description)}', {(isEnabled ? 1 : 0)}, '{createdAt}', 'Database',
     N'{EscapeSql(encryptedConnectionString)}', N'{EscapeSql(databaseType)}', 
     {NullOrString(schema)}, N'{EscapeSql(query)}', {commandTimeoutSeconds})";

            migrationBuilder.Sql(sql);
        }

        private void InsertAzureBlobDataSource(MigrationBuilder migrationBuilder, string createdAt,
            string name, string description, string connectionString, string containerName,
            bool isEnabled, string? blobName = null, string? blobPrefix = null)
        {
            var encryptedConnectionString = _cryptographyService.Encrypt(connectionString);

            var sql = $@"
INSERT INTO [DataSources] 
    ([Name], [Description], [IsEnabled], [CreatedAt], [SourceType], 
     [ConnectionString], [ContainerName], [BlobName], [BlobPrefix])
VALUES 
    (N'{EscapeSql(name)}', N'{EscapeSql(description)}', {(isEnabled ? 1 : 0)}, '{createdAt}', 'AzureBlob',
     N'{EscapeSql(encryptedConnectionString)}', N'{EscapeSql(containerName)}', 
     {NullOrString(blobName)}, {NullOrString(blobPrefix)})";

            migrationBuilder.Sql(sql);
        }

        private void InsertS3DataSource(MigrationBuilder migrationBuilder, string createdAt,
            string name, string description, string bucketName, string accessKey, string secretKey,
            string region, string prefix, bool isEnabled, string? objectKey = null)
        {
            var encryptedAccessKey = _cryptographyService.Encrypt(accessKey);
            var encryptedSecretKey = _cryptographyService.Encrypt(secretKey);

            var sql = $@"
INSERT INTO [DataSources] 
    ([Name], [Description], [IsEnabled], [CreatedAt], [SourceType], 
     [BucketName], [AccessKey], [SecretKey], [Region], [ObjectKey], [Prefix])
VALUES 
    (N'{EscapeSql(name)}', N'{EscapeSql(description)}', {(isEnabled ? 1 : 0)}, '{createdAt}', 'S3',
     N'{EscapeSql(bucketName)}', N'{EscapeSql(encryptedAccessKey)}', N'{EscapeSql(encryptedSecretKey)}', 
     N'{EscapeSql(region)}', {NullOrString(objectKey)}, N'{EscapeSql(prefix)}')";

            migrationBuilder.Sql(sql);
        }

        private string? EncryptIfNotEmpty(string? value)
        {
            return string.IsNullOrEmpty(value) ? null : _cryptographyService.Encrypt(value);
        }

        private static string NullOrString(string? value)
        {
            return value == null ? "NULL" : $"N'{EscapeSql(value)}'";
        }

        private static string EscapeSql(string value)
        {
            return value.Replace("'", "''");
        }

        #endregion
    }
}
