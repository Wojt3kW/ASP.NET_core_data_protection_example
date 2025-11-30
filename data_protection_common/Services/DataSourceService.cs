using data_protection_common.DTOs;
using data_protection_common.Entities;
using data_protection_common.Extensions;
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
        private readonly IDataProtectionCryptographyService _cryptographyService;
        private readonly ILogger<DataSourceService> _logger;

        public DataSourceService(
            ApplicationDbContext context,
            IDataProtectionCryptographyService cryptographyService,
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
            return dataSources.DecryptSensitiveData(_cryptographyService).ToDto();
        }

        public async Task<DataSourceDto?> GetByIdAsync(int id)
        {
            var dataSource = await _context.DataSources.FindAsync(id);
            return dataSource.DecryptSensitiveDataOrNull(_cryptographyService)?.ToDto();
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
            return dataSources.DecryptSensitiveData(_cryptographyService).ToDto();
        }

        #endregion

        #region URL Data Source

        public async Task<int> CreateUrlAsync(CreateUrlDataSourceDto dto)
        {
            var entity = dto.ToEntity().EncryptSensitiveData(_cryptographyService);
            _context.DataSources.Add(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created URL data source with ID {Id}", entity.Id);
            return entity.Id;
        }

        public async Task<bool> UpdateUrlAsync(int id, CreateUrlDataSourceDto dto)
        {
            var entity = await _context.DataSources.OfType<UrlDataSource>().FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return false;
            }

            entity.UpdateFrom(dto).EncryptSensitiveData(_cryptographyService);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Updated URL data source with ID {Id}", id);
            return true;
        }

        #endregion

        #region File Data Source

        public async Task<int> CreateFileAsync(CreateFileDataSourceDto dto)
        {
            var entity = dto.ToEntity();
            _context.DataSources.Add(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created File data source with ID {Id}", entity.Id);
            return entity.Id;
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

        public async Task<int> CreateFtpAsync(CreateFtpDataSourceDto dto)
        {
            var entity = dto.ToEntity().EncryptSensitiveData(_cryptographyService);
            _context.DataSources.Add(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created FTP data source with ID {Id}", entity.Id);
            return entity.Id;
        }

        public async Task<bool> UpdateFtpAsync(int id, CreateFtpDataSourceDto dto)
        {
            var entity = await _context.DataSources.OfType<FtpDataSource>().FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return false;
            }

            entity.UpdateFrom(dto).EncryptSensitiveData(_cryptographyService);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Updated FTP data source with ID {Id}", id);
            return true;
        }

        #endregion

        #region Database Data Source

        public async Task<int> CreateDatabaseAsync(CreateDatabaseDataSourceDto dto)
        {
            var entity = dto.ToEntity().EncryptSensitiveData(_cryptographyService);
            _context.DataSources.Add(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created Database data source with ID {Id}", entity.Id);
            return entity.Id;
        }

        public async Task<bool> UpdateDatabaseAsync(int id, CreateDatabaseDataSourceDto dto)
        {
            var entity = await _context.DataSources.OfType<DatabaseDataSource>().FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return false;
            }

            entity.UpdateFrom(dto).EncryptSensitiveData(_cryptographyService);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Updated Database data source with ID {Id}", id);
            return true;
        }

        #endregion

        #region Azure Blob Data Source

        public async Task<int> CreateAzureBlobAsync(CreateAzureBlobDataSourceDto dto)
        {
            var entity = dto.ToEntity().EncryptSensitiveData(_cryptographyService);
            _context.DataSources.Add(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created Azure Blob data source with ID {Id}", entity.Id);
            return entity.Id;
        }

        public async Task<bool> UpdateAzureBlobAsync(int id, CreateAzureBlobDataSourceDto dto)
        {
            var entity = await _context.DataSources.OfType<AzureBlobDataSource>().FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return false;
            }

            entity.UpdateFrom(dto).EncryptSensitiveData(_cryptographyService);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Updated Azure Blob data source with ID {Id}", id);
            return true;
        }

        #endregion

        #region S3 Data Source

        public async Task<int> CreateS3Async(CreateS3DataSourceDto dto)
        {
            var entity = dto.ToEntity().EncryptSensitiveData(_cryptographyService);
            _context.DataSources.Add(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created S3 data source with ID {Id}", entity.Id);
            return entity.Id;
        }

        public async Task<bool> UpdateS3Async(int id, CreateS3DataSourceDto dto)
        {
            var entity = await _context.DataSources.OfType<S3DataSource>().FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return false;
            }

            entity.UpdateFrom(dto).EncryptSensitiveData(_cryptographyService);
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
    }
}
