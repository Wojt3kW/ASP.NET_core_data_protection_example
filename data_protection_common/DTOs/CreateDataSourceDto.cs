using System.ComponentModel.DataAnnotations;

namespace data_protection_common.DTOs
{
    /// <summary>
    /// Base DTO for creating/updating data sources
    /// </summary>
    public abstract class CreateDataSourceDto
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        public bool IsEnabled { get; set; } = true;
    }

    /// <summary>
    /// DTO for creating URL/REST API data source
    /// </summary>
    public class CreateUrlDataSourceDto : CreateDataSourceDto
    {
        [Required]
        [MaxLength(2048)]
        public string Url { get; set; } = string.Empty;

        [MaxLength(10)]
        public string? HttpMethod { get; set; } = "GET";

        public string? Headers { get; set; }

        [MaxLength(50)]
        public string? AuthType { get; set; }

        [MaxLength(255)]
        public string? Username { get; set; }

        [MaxLength(500)]
        public string? Password { get; set; }

        [MaxLength(500)]
        public string? ApiKey { get; set; }

        [MaxLength(2000)]
        public string? BearerToken { get; set; }

        public int? TimeoutSeconds { get; set; } = 30;
    }

    /// <summary>
    /// DTO for creating File data source
    /// </summary>
    public class CreateFileDataSourceDto : CreateDataSourceDto
    {
        [Required]
        [MaxLength(1024)]
        public string FilePath { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? FileType { get; set; }

        [MaxLength(50)]
        public string? Encoding { get; set; } = "UTF-8";

        [MaxLength(10)]
        public string? Delimiter { get; set; }

        public bool HasHeader { get; set; } = true;
    }

    /// <summary>
    /// DTO for creating FTP/SFTP data source
    /// </summary>
    public class CreateFtpDataSourceDto : CreateDataSourceDto
    {
        [Required]
        [MaxLength(255)]
        public string Host { get; set; } = string.Empty;

        public int Port { get; set; } = 21;

        [MaxLength(255)]
        public string? Username { get; set; }

        [MaxLength(500)]
        public string? Password { get; set; }

        [MaxLength(1024)]
        public string? RemotePath { get; set; }

        public bool UseSftp { get; set; } = false;

        public bool UsePassiveMode { get; set; } = true;

        [MaxLength(1024)]
        public string? PrivateKeyPath { get; set; }
    }

    /// <summary>
    /// DTO for creating Database data source
    /// </summary>
    public class CreateDatabaseDataSourceDto : CreateDataSourceDto
    {
        [Required]
        [MaxLength(2048)]
        public string ConnectionString { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? DatabaseType { get; set; }

        [MaxLength(128)]
        public string? Schema { get; set; }

        [MaxLength(4000)]
        public string? Query { get; set; }

        public int? CommandTimeoutSeconds { get; set; } = 30;
    }

    /// <summary>
    /// DTO for creating Azure Blob Storage data source
    /// </summary>
    public class CreateAzureBlobDataSourceDto : CreateDataSourceDto
    {
        [Required]
        [MaxLength(2048)]
        public string ConnectionString { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string ContainerName { get; set; } = string.Empty;

        [MaxLength(1024)]
        public string? BlobName { get; set; }

        [MaxLength(1024)]
        public string? BlobPrefix { get; set; }
    }

    /// <summary>
    /// DTO for creating Amazon S3 data source
    /// </summary>
    public class CreateS3DataSourceDto : CreateDataSourceDto
    {
        [Required]
        [MaxLength(255)]
        public string BucketName { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? AccessKey { get; set; }

        [MaxLength(500)]
        public string? SecretKey { get; set; }

        [MaxLength(50)]
        public string? Region { get; set; }

        [MaxLength(1024)]
        public string? ObjectKey { get; set; }

        [MaxLength(1024)]
        public string? Prefix { get; set; }
    }
}
