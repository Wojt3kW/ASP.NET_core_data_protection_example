using data_protection_common.DTOs;

namespace data_protection_common.Services
{
    /// <summary>
    /// Service interface for managing data sources
    /// </summary>
    public interface IDataSourceService
    {
        // Read operations
        Task<IEnumerable<DataSourceDto>> GetAllAsync();
        Task<DataSourceDto?> GetByIdAsync(int id);
        Task<IEnumerable<DataSourceDto>> GetByTypeAsync(string sourceType);

        // URL Data Source
        Task<int> CreateUrlAsync(CreateUrlDataSourceDto dto);
        Task<bool> UpdateUrlAsync(int id, CreateUrlDataSourceDto dto);

        // File Data Source
        Task<int> CreateFileAsync(CreateFileDataSourceDto dto);
        Task<bool> UpdateFileAsync(int id, CreateFileDataSourceDto dto);

        // FTP Data Source
        Task<int> CreateFtpAsync(CreateFtpDataSourceDto dto);
        Task<bool> UpdateFtpAsync(int id, CreateFtpDataSourceDto dto);

        // Database Data Source
        Task<int> CreateDatabaseAsync(CreateDatabaseDataSourceDto dto);
        Task<bool> UpdateDatabaseAsync(int id, CreateDatabaseDataSourceDto dto);

        // Azure Blob Data Source
        Task<int> CreateAzureBlobAsync(CreateAzureBlobDataSourceDto dto);
        Task<bool> UpdateAzureBlobAsync(int id, CreateAzureBlobDataSourceDto dto);

        // S3 Data Source
        Task<int> CreateS3Async(CreateS3DataSourceDto dto);
        Task<bool> UpdateS3Async(int id, CreateS3DataSourceDto dto);

        // Common operations
        Task<bool> DeleteAsync(int id);
        Task<(bool Success, bool IsEnabled)> ToggleAsync(int id);
    }
}
