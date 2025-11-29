using data_protection_common.DTOs;
using data_protection_common.Entities;
using data_protection_common.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace data_protection_common.Services
{
    /// <summary>
    /// Service implementation for managing data sources
    /// </summary>
    public class DataSourceService : IDataSourceService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICryptographyService _cryptographyService;
        private readonly ILogger<DataSourceService> _logger;

        public DataSourceService(
            ApplicationDbContext context,
            ICryptographyService cryptographyService,
            ILogger<DataSourceService> logger)
        {
            _context = context;
            _cryptographyService = cryptographyService;
            _logger = logger;
        }

        #region Read Operations

        public async Task<IEnumerable<DataSourceDto>> GetAllAsync()
        {
            var dataSources = await _context.DataSources.ToListAsync();
            DecryptSensitiveData(dataSources);
            return dataSources.Select(ds => ds.ToDto());
        }

        public async Task<DataSourceDto?> GetByIdAsync(int id)
        {
            var dataSource = await _context.DataSources.FindAsync(id);
            if (dataSource != null)
            {
                DecryptSensitiveData(dataSource);
            }
            return dataSource?.ToDto();
        }

        public async Task<IEnumerable<DataSourceDto>> GetByTypeAsync(string sourceType)
        {
            var query = sourceType.ToLower() switch
            {
                "url" => _context.DataSources.OfType<UrlDataSource>().Cast<DataSource>(),
                "file" => _context.DataSources.OfType<FileDataSource>().Cast<DataSource>(),
                "ftp" => _context.DataSources.OfType<FtpDataSource>().Cast<DataSource>(),
                "database" => _context.DataSources.OfType<DatabaseDataSource>().Cast<DataSource>(),
                "azureblob" => _context.DataSources.OfType<AzureBlobDataSource>().Cast<DataSource>(),
                "s3" => _context.DataSources.OfType<S3DataSource>().Cast<DataSource>(),
                _ => null
            };

            if (query == null)
            {
                return Enumerable.Empty<DataSourceDto>();
            }

            var dataSources = await query.ToListAsync();
            DecryptSensitiveData(dataSources);
            return dataSources.Select(ds => ds.ToDto());
        }

        #endregion

        #region URL Data Source

        public async Task<DataSourceDto> CreateUrlAsync(CreateUrlDataSourceDto dto)
        {
            var entity = dto.ToEntity();
            EncryptSensitiveData(entity);
            _context.DataSources.Add(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created URL data source with ID {Id}", entity.Id);
            
            // Return decrypted version for response
            DecryptSensitiveData(entity);
            return entity.ToDto();
        }

        public async Task<bool> UpdateUrlAsync(int id, CreateUrlDataSourceDto dto)
        {
            var entity = await _context.DataSources.OfType<UrlDataSource>().FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return false;
            }

            entity.UpdateFrom(dto);
            EncryptSensitiveData(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Updated URL data source with ID {Id}", id);
            return true;
        }

        #endregion

        #region File Data Source

        public async Task<DataSourceDto> CreateFileAsync(CreateFileDataSourceDto dto)
        {
            var entity = dto.ToEntity();
            _context.DataSources.Add(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created File data source with ID {Id}", entity.Id);
            return entity.ToDto();
        }

        public async Task<bool> UpdateFileAsync(int id, CreateFileDataSourceDto dto)
        {
            var entity = await _context.DataSources.OfType<FileDataSource>().FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return false;
            }

            entity.UpdateFrom(dto);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Updated File data source with ID {Id}", id);
            return true;
        }

        #endregion

        #region FTP Data Source

        public async Task<DataSourceDto> CreateFtpAsync(CreateFtpDataSourceDto dto)
        {
            var entity = dto.ToEntity();
            EncryptSensitiveData(entity);
            _context.DataSources.Add(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created FTP data source with ID {Id}", entity.Id);
            
            DecryptSensitiveData(entity);
            return entity.ToDto();
        }

        public async Task<bool> UpdateFtpAsync(int id, CreateFtpDataSourceDto dto)
        {
            var entity = await _context.DataSources.OfType<FtpDataSource>().FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return false;
            }

            entity.UpdateFrom(dto);
            EncryptSensitiveData(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Updated FTP data source with ID {Id}", id);
            return true;
        }

        #endregion

        #region Database Data Source

        public async Task<DataSourceDto> CreateDatabaseAsync(CreateDatabaseDataSourceDto dto)
        {
            var entity = dto.ToEntity();
            EncryptSensitiveData(entity);
            _context.DataSources.Add(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created Database data source with ID {Id}", entity.Id);
            
            DecryptSensitiveData(entity);
            return entity.ToDto();
        }

        public async Task<bool> UpdateDatabaseAsync(int id, CreateDatabaseDataSourceDto dto)
        {
            var entity = await _context.DataSources.OfType<DatabaseDataSource>().FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return false;
            }

            entity.UpdateFrom(dto);
            EncryptSensitiveData(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Updated Database data source with ID {Id}", id);
            return true;
        }

        #endregion

        #region Azure Blob Data Source

        public async Task<DataSourceDto> CreateAzureBlobAsync(CreateAzureBlobDataSourceDto dto)
        {
            var entity = dto.ToEntity();
            EncryptSensitiveData(entity);
            _context.DataSources.Add(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created Azure Blob data source with ID {Id}", entity.Id);
            
            DecryptSensitiveData(entity);
            return entity.ToDto();
        }

        public async Task<bool> UpdateAzureBlobAsync(int id, CreateAzureBlobDataSourceDto dto)
        {
            var entity = await _context.DataSources.OfType<AzureBlobDataSource>().FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return false;
            }

            entity.UpdateFrom(dto);
            EncryptSensitiveData(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Updated Azure Blob data source with ID {Id}", id);
            return true;
        }

        #endregion

        #region S3 Data Source

        public async Task<DataSourceDto> CreateS3Async(CreateS3DataSourceDto dto)
        {
            var entity = dto.ToEntity();
            EncryptSensitiveData(entity);
            _context.DataSources.Add(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created S3 data source with ID {Id}", entity.Id);
            
            DecryptSensitiveData(entity);
            return entity.ToDto();
        }

        public async Task<bool> UpdateS3Async(int id, CreateS3DataSourceDto dto)
        {
            var entity = await _context.DataSources.OfType<S3DataSource>().FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return false;
            }

            entity.UpdateFrom(dto);
            EncryptSensitiveData(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Updated S3 data source with ID {Id}", id);
            return true;
        }

        #endregion

        #region Common Operations

        public async Task<bool> DeleteAsync(int id)
        {
            var dataSource = await _context.DataSources.FindAsync(id);

            if (dataSource == null)
            {
                return false;
            }

            _context.DataSources.Remove(dataSource);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Deleted data source with ID {Id}", id);
            return true;
        }

        public async Task<(bool Success, bool IsEnabled)> ToggleAsync(int id)
        {
            var dataSource = await _context.DataSources.FindAsync(id);

            if (dataSource == null)
            {
                return (false, false);
            }

            dataSource.IsEnabled = !dataSource.IsEnabled;
            dataSource.ModifiedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Toggled data source {Id} to {IsEnabled}", id, dataSource.IsEnabled);
            return (true, dataSource.IsEnabled);
        }

        #endregion

        #region Encryption/Decryption Helpers

        private void EncryptSensitiveData(DataSource entity)
        {
            switch (entity)
            {
                case UrlDataSource url:
                    url.Password = EncryptIfNotEmpty(url.Password);
                    url.ApiKey = EncryptIfNotEmpty(url.ApiKey);
                    url.BearerToken = EncryptIfNotEmpty(url.BearerToken);
                    break;

                case FtpDataSource ftp:
                    ftp.Password = EncryptIfNotEmpty(ftp.Password);
                    ftp.PrivateKeyPath = EncryptIfNotEmpty(ftp.PrivateKeyPath);
                    break;

                case DatabaseDataSource db:
                    db.ConnectionString = EncryptIfNotEmpty(db.ConnectionString) ?? string.Empty;
                    break;

                case AzureBlobDataSource azure:
                    azure.ConnectionString = EncryptIfNotEmpty(azure.ConnectionString) ?? string.Empty;
                    break;

                case S3DataSource s3:
                    s3.AccessKey = EncryptIfNotEmpty(s3.AccessKey);
                    s3.SecretKey = EncryptIfNotEmpty(s3.SecretKey);
                    break;
            }
        }

        private void DecryptSensitiveData(DataSource entity)
        {
            switch (entity)
            {
                case UrlDataSource url:
                    url.Password = DecryptIfNotEmpty(url.Password);
                    url.ApiKey = DecryptIfNotEmpty(url.ApiKey);
                    url.BearerToken = DecryptIfNotEmpty(url.BearerToken);
                    break;

                case FtpDataSource ftp:
                    ftp.Password = DecryptIfNotEmpty(ftp.Password);
                    ftp.PrivateKeyPath = DecryptIfNotEmpty(ftp.PrivateKeyPath);
                    break;

                case DatabaseDataSource db:
                    db.ConnectionString = DecryptIfNotEmpty(db.ConnectionString) ?? string.Empty;
                    break;

                case AzureBlobDataSource azure:
                    azure.ConnectionString = DecryptIfNotEmpty(azure.ConnectionString) ?? string.Empty;
                    break;

                case S3DataSource s3:
                    s3.AccessKey = DecryptIfNotEmpty(s3.AccessKey);
                    s3.SecretKey = DecryptIfNotEmpty(s3.SecretKey);
                    break;
            }
        }

        private void DecryptSensitiveData(IEnumerable<DataSource> entities)
        {
            foreach (var entity in entities)
            {
                DecryptSensitiveData(entity);
            }
        }

        private string? EncryptIfNotEmpty(string? value)
        {
            return string.IsNullOrEmpty(value) ? value : _cryptographyService.Encrypt(value);
        }

        private string? DecryptIfNotEmpty(string? value)
        {
            return string.IsNullOrEmpty(value) ? value : _cryptographyService.Decrypt(value);
        }

        #endregion
    }
}
