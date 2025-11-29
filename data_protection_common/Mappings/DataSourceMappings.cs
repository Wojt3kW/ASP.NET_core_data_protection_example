using data_protection_common.DTOs;
using data_protection_common.Entities;

namespace data_protection_common.Mappings
{
    /// <summary>
    /// Extension methods for mapping between entities and DTOs
    /// </summary>
    public static class DataSourceMappings
    {
        /// <summary>
        /// Maps DataSource entity to appropriate DTO
        /// </summary>
        public static DataSourceDto ToDto(this DataSource entity)
        {
            return entity switch
            {
                UrlDataSource url => new UrlDataSourceDto
                {
                    Id = url.Id,
                    Name = url.Name,
                    Description = url.Description,
                    IsEnabled = url.IsEnabled,
                    CreatedAt = url.CreatedAt,
                    ModifiedAt = url.ModifiedAt,
                    SourceType = "Url",
                    Url = url.Url,
                    HttpMethod = url.HttpMethod,
                    Headers = url.Headers,
                    AuthType = url.AuthType,
                    Username = url.Username,
                    TimeoutSeconds = url.TimeoutSeconds
                },
                FileDataSource file => new FileDataSourceDto
                {
                    Id = file.Id,
                    Name = file.Name,
                    Description = file.Description,
                    IsEnabled = file.IsEnabled,
                    CreatedAt = file.CreatedAt,
                    ModifiedAt = file.ModifiedAt,
                    SourceType = "File",
                    FilePath = file.FilePath,
                    FileType = file.FileType,
                    Encoding = file.Encoding,
                    Delimiter = file.Delimiter,
                    HasHeader = file.HasHeader
                },
                FtpDataSource ftp => new FtpDataSourceDto
                {
                    Id = ftp.Id,
                    Name = ftp.Name,
                    Description = ftp.Description,
                    IsEnabled = ftp.IsEnabled,
                    CreatedAt = ftp.CreatedAt,
                    ModifiedAt = ftp.ModifiedAt,
                    SourceType = "Ftp",
                    Host = ftp.Host,
                    Port = ftp.Port,
                    Username = ftp.Username,
                    RemotePath = ftp.RemotePath,
                    UseSftp = ftp.UseSftp,
                    UsePassiveMode = ftp.UsePassiveMode
                },
                DatabaseDataSource db => new DatabaseDataSourceDto
                {
                    Id = db.Id,
                    Name = db.Name,
                    Description = db.Description,
                    IsEnabled = db.IsEnabled,
                    CreatedAt = db.CreatedAt,
                    ModifiedAt = db.ModifiedAt,
                    SourceType = "Database",
                    DatabaseType = db.DatabaseType,
                    Schema = db.Schema,
                    Query = db.Query,
                    CommandTimeoutSeconds = db.CommandTimeoutSeconds
                },
                AzureBlobDataSource azure => new AzureBlobDataSourceDto
                {
                    Id = azure.Id,
                    Name = azure.Name,
                    Description = azure.Description,
                    IsEnabled = azure.IsEnabled,
                    CreatedAt = azure.CreatedAt,
                    ModifiedAt = azure.ModifiedAt,
                    SourceType = "AzureBlob",
                    ContainerName = azure.ContainerName,
                    BlobName = azure.BlobName,
                    BlobPrefix = azure.BlobPrefix
                },
                S3DataSource s3 => new S3DataSourceDto
                {
                    Id = s3.Id,
                    Name = s3.Name,
                    Description = s3.Description,
                    IsEnabled = s3.IsEnabled,
                    CreatedAt = s3.CreatedAt,
                    ModifiedAt = s3.ModifiedAt,
                    SourceType = "S3",
                    BucketName = s3.BucketName,
                    Region = s3.Region,
                    ObjectKey = s3.ObjectKey,
                    Prefix = s3.Prefix
                },
                _ => throw new ArgumentException($"Unknown data source type: {entity.GetType().Name}")
            };
        }

        /// <summary>
        /// Creates UrlDataSource entity from DTO
        /// </summary>
        public static UrlDataSource ToEntity(this CreateUrlDataSourceDto dto)
        {
            return new UrlDataSource
            {
                Name = dto.Name,
                Description = dto.Description,
                IsEnabled = dto.IsEnabled,
                Url = dto.Url,
                HttpMethod = dto.HttpMethod,
                Headers = dto.Headers,
                AuthType = dto.AuthType,
                Username = dto.Username,
                Password = dto.Password,
                ApiKey = dto.ApiKey,
                BearerToken = dto.BearerToken,
                TimeoutSeconds = dto.TimeoutSeconds
            };
        }

        /// <summary>
        /// Creates FileDataSource entity from DTO
        /// </summary>
        public static FileDataSource ToEntity(this CreateFileDataSourceDto dto)
        {
            return new FileDataSource
            {
                Name = dto.Name,
                Description = dto.Description,
                IsEnabled = dto.IsEnabled,
                FilePath = dto.FilePath,
                FileType = dto.FileType,
                Encoding = dto.Encoding,
                Delimiter = dto.Delimiter,
                HasHeader = dto.HasHeader
            };
        }

        /// <summary>
        /// Creates FtpDataSource entity from DTO
        /// </summary>
        public static FtpDataSource ToEntity(this CreateFtpDataSourceDto dto)
        {
            return new FtpDataSource
            {
                Name = dto.Name,
                Description = dto.Description,
                IsEnabled = dto.IsEnabled,
                Host = dto.Host,
                Port = dto.Port,
                Username = dto.Username,
                Password = dto.Password,
                RemotePath = dto.RemotePath,
                UseSftp = dto.UseSftp,
                UsePassiveMode = dto.UsePassiveMode,
                PrivateKeyPath = dto.PrivateKeyPath
            };
        }

        /// <summary>
        /// Creates DatabaseDataSource entity from DTO
        /// </summary>
        public static DatabaseDataSource ToEntity(this CreateDatabaseDataSourceDto dto)
        {
            return new DatabaseDataSource
            {
                Name = dto.Name,
                Description = dto.Description,
                IsEnabled = dto.IsEnabled,
                ConnectionString = dto.ConnectionString,
                DatabaseType = dto.DatabaseType,
                Schema = dto.Schema,
                Query = dto.Query,
                CommandTimeoutSeconds = dto.CommandTimeoutSeconds
            };
        }

        /// <summary>
        /// Creates AzureBlobDataSource entity from DTO
        /// </summary>
        public static AzureBlobDataSource ToEntity(this CreateAzureBlobDataSourceDto dto)
        {
            return new AzureBlobDataSource
            {
                Name = dto.Name,
                Description = dto.Description,
                IsEnabled = dto.IsEnabled,
                ConnectionString = dto.ConnectionString,
                ContainerName = dto.ContainerName,
                BlobName = dto.BlobName,
                BlobPrefix = dto.BlobPrefix
            };
        }

        /// <summary>
        /// Creates S3DataSource entity from DTO
        /// </summary>
        public static S3DataSource ToEntity(this CreateS3DataSourceDto dto)
        {
            return new S3DataSource
            {
                Name = dto.Name,
                Description = dto.Description,
                IsEnabled = dto.IsEnabled,
                BucketName = dto.BucketName,
                AccessKey = dto.AccessKey,
                SecretKey = dto.SecretKey,
                Region = dto.Region,
                ObjectKey = dto.ObjectKey,
                Prefix = dto.Prefix
            };
        }

        /// <summary>
        /// Updates UrlDataSource entity from DTO
        /// </summary>
        public static void UpdateFrom(this UrlDataSource entity, CreateUrlDataSourceDto dto)
        {
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.IsEnabled = dto.IsEnabled;
            entity.Url = dto.Url;
            entity.HttpMethod = dto.HttpMethod;
            entity.Headers = dto.Headers;
            entity.AuthType = dto.AuthType;
            entity.Username = dto.Username;
            entity.Password = dto.Password;
            entity.ApiKey = dto.ApiKey;
            entity.BearerToken = dto.BearerToken;
            entity.TimeoutSeconds = dto.TimeoutSeconds;
            entity.ModifiedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Updates FileDataSource entity from DTO
        /// </summary>
        public static void UpdateFrom(this FileDataSource entity, CreateFileDataSourceDto dto)
        {
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.IsEnabled = dto.IsEnabled;
            entity.FilePath = dto.FilePath;
            entity.FileType = dto.FileType;
            entity.Encoding = dto.Encoding;
            entity.Delimiter = dto.Delimiter;
            entity.HasHeader = dto.HasHeader;
            entity.ModifiedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Updates FtpDataSource entity from DTO
        /// </summary>
        public static void UpdateFrom(this FtpDataSource entity, CreateFtpDataSourceDto dto)
        {
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.IsEnabled = dto.IsEnabled;
            entity.Host = dto.Host;
            entity.Port = dto.Port;
            entity.Username = dto.Username;
            entity.Password = dto.Password;
            entity.RemotePath = dto.RemotePath;
            entity.UseSftp = dto.UseSftp;
            entity.UsePassiveMode = dto.UsePassiveMode;
            entity.PrivateKeyPath = dto.PrivateKeyPath;
            entity.ModifiedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Updates DatabaseDataSource entity from DTO
        /// </summary>
        public static void UpdateFrom(this DatabaseDataSource entity, CreateDatabaseDataSourceDto dto)
        {
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.IsEnabled = dto.IsEnabled;
            entity.ConnectionString = dto.ConnectionString;
            entity.DatabaseType = dto.DatabaseType;
            entity.Schema = dto.Schema;
            entity.Query = dto.Query;
            entity.CommandTimeoutSeconds = dto.CommandTimeoutSeconds;
            entity.ModifiedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Updates AzureBlobDataSource entity from DTO
        /// </summary>
        public static void UpdateFrom(this AzureBlobDataSource entity, CreateAzureBlobDataSourceDto dto)
        {
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.IsEnabled = dto.IsEnabled;
            entity.ConnectionString = dto.ConnectionString;
            entity.ContainerName = dto.ContainerName;
            entity.BlobName = dto.BlobName;
            entity.BlobPrefix = dto.BlobPrefix;
            entity.ModifiedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Updates S3DataSource entity from DTO
        /// </summary>
        public static void UpdateFrom(this S3DataSource entity, CreateS3DataSourceDto dto)
        {
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.IsEnabled = dto.IsEnabled;
            entity.BucketName = dto.BucketName;
            entity.AccessKey = dto.AccessKey;
            entity.SecretKey = dto.SecretKey;
            entity.Region = dto.Region;
            entity.ObjectKey = dto.ObjectKey;
            entity.Prefix = dto.Prefix;
            entity.ModifiedAt = DateTime.UtcNow;
        }
    }
}
