namespace data_protection_common.DTOs
{
    /// <summary>
    /// Base DTO for data source responses
    /// </summary>
    public abstract class DataSourceDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string SourceType { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO for URL/REST API data source
    /// </summary>
    public class UrlDataSourceDto : DataSourceDto
    {
        public string Url { get; set; } = string.Empty;
        public string? HttpMethod { get; set; }
        public string? Headers { get; set; }
        public string? AuthType { get; set; }
        public string? Username { get; set; }
        public int? TimeoutSeconds { get; set; }
        // Sensitive data
        public string? Password { get; set; }
        public string? ApiKey { get; set; }
        public string? BearerToken { get; set; }
    }

    /// <summary>
    /// DTO for File data source
    /// </summary>
    public class FileDataSourceDto : DataSourceDto
    {
        public string FilePath { get; set; } = string.Empty;
        public string? FileType { get; set; }
        public string? Encoding { get; set; }
        public string? Delimiter { get; set; }
        public bool HasHeader { get; set; }
    }

    /// <summary>
    /// DTO for FTP/SFTP data source
    /// </summary>
    public class FtpDataSourceDto : DataSourceDto
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public string? Username { get; set; }
        public string? RemotePath { get; set; }
        public bool UseSftp { get; set; }
        public bool UsePassiveMode { get; set; }
        // Sensitive data
        public string? Password { get; set; }
        public string? PrivateKeyPath { get; set; }
    }

    /// <summary>
    /// DTO for Database data source
    /// </summary>
    public class DatabaseDataSourceDto : DataSourceDto
    {
        public string? DatabaseType { get; set; }
        public string? Schema { get; set; }
        public string? Query { get; set; }
        public int? CommandTimeoutSeconds { get; set; }
        // Sensitive data
        public string? ConnectionString { get; set; }
    }

    /// <summary>
    /// DTO for Azure Blob Storage data source
    /// </summary>
    public class AzureBlobDataSourceDto : DataSourceDto
    {
        public string ContainerName { get; set; } = string.Empty;
        public string? BlobName { get; set; }
        public string? BlobPrefix { get; set; }
        // Sensitive data
        public string? ConnectionString { get; set; }
    }

    /// <summary>
    /// DTO for Amazon S3 data source
    /// </summary>
    public class S3DataSourceDto : DataSourceDto
    {
        public string BucketName { get; set; } = string.Empty;
        public string? Region { get; set; }
        public string? ObjectKey { get; set; }
        public string? Prefix { get; set; }
        // Sensitive data
        public string? AccessKey { get; set; }
        public string? SecretKey { get; set; }
    }
}
