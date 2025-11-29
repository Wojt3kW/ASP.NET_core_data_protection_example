namespace data_protection_common.Entities
{
    /// <summary>
    /// Base class for all data sources
    /// </summary>
    public abstract class DataSource
    {
        public int Id { get; set; }
        
        /// <summary>
        /// Friendly name of the data source
        /// </summary>
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Description of the data source
        /// </summary>
        public string? Description { get; set; }
        
        /// <summary>
        /// Whether the data source is active
        /// </summary>
        public bool IsEnabled { get; set; } = true;
        
        /// <summary>
        /// Creation date
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// Last modification date
        /// </summary>
        public DateTime? ModifiedAt { get; set; }
    }

    /// <summary>
    /// Data source: URL/REST API
    /// </summary>
    public class UrlDataSource : DataSource
    {
        public string Url { get; set; } = string.Empty;
        public string? HttpMethod { get; set; } = "GET";
        public string? Headers { get; set; }  // JSON with headers
        public string? AuthType { get; set; }  // None, Basic, Bearer, ApiKey
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? ApiKey { get; set; }
        public string? BearerToken { get; set; }
        public int? TimeoutSeconds { get; set; } = 30;
    }

    /// <summary>
    /// Data source: File (JSON, XML, CSV, etc.)
    /// </summary>
    public class FileDataSource : DataSource
    {
        public string FilePath { get; set; } = string.Empty;
        public string? FileType { get; set; }  // JSON, XML, CSV, Excel
        public string? Encoding { get; set; } = "UTF-8";
        public string? Delimiter { get; set; }  // for CSV
        public bool HasHeader { get; set; } = true;  // for CSV
    }

    /// <summary>
    /// Data source: FTP/SFTP
    /// </summary>
    public class FtpDataSource : DataSource
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; } = 21;
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? RemotePath { get; set; }
        public bool UseSftp { get; set; } = false;
        public bool UsePassiveMode { get; set; } = true;
        public string? PrivateKeyPath { get; set; }  // for SFTP with key
    }

    /// <summary>
    /// Data source: Database
    /// </summary>
    public class DatabaseDataSource : DataSource
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string? DatabaseType { get; set; }  // SqlServer, PostgreSQL, MySQL, Oracle, SQLite
        public string? Schema { get; set; }
        public string? Query { get; set; }  // SQL query or table/view name
        public int? CommandTimeoutSeconds { get; set; } = 30;
    }

    /// <summary>
    /// Data source: Azure Blob Storage
    /// </summary>
    public class AzureBlobDataSource : DataSource
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string ContainerName { get; set; } = string.Empty;
        public string? BlobName { get; set; }
        public string? BlobPrefix { get; set; }  // for filtering blobs
    }

    /// <summary>
    /// Data source: Amazon S3
    /// </summary>
    public class S3DataSource : DataSource
    {
        public string BucketName { get; set; } = string.Empty;
        public string? AccessKey { get; set; }
        public string? SecretKey { get; set; }
        public string? Region { get; set; }
        public string? ObjectKey { get; set; }
        public string? Prefix { get; set; }
    }
}
