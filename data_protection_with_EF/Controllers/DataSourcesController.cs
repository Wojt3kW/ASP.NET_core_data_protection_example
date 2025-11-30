using data_protection_common.DTOs;
using data_protection_common.Services;
using Microsoft.AspNetCore.Mvc;

namespace data_protection_with_EF.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataSourcesController : ControllerBase
    {
        private readonly IDataSourceService _dataSourceService;

        public DataSourcesController(IDataSourceService dataSourceService)
        {
            _dataSourceService = dataSourceService;
        }

        /// <summary>
        /// Get all data sources
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DataSourceDto>>> GetAll()
        {
            var dataSources = await _dataSourceService.GetAllAsync();
            return Ok(dataSources);
        }

        /// <summary>
        /// Get data source by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<DataSourceDto>> GetById(int id)
        {
            var dataSource = await _dataSourceService.GetByIdAsync(id);

            if (dataSource == null)
            {
                return NotFound();
            }

            return Ok(dataSource);
        }

        /// <summary>
        /// Get data sources by type
        /// </summary>
        [HttpGet("type/{sourceType}")]
        public async Task<ActionResult<IEnumerable<DataSourceDto>>> GetByType(string sourceType)
        {
            var dataSources = await _dataSourceService.GetByTypeAsync(sourceType);
            return Ok(dataSources);
        }

        // ==================== URL Data Source ====================

        /// <summary>
        /// Create URL data source
        /// </summary>
        [HttpPost("url")]
        public async Task<ActionResult<int>> CreateUrl([FromBody] CreateUrlDataSourceDto dto)
        {
            var id = await _dataSourceService.CreateUrlAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        /// <summary>
        /// Update URL data source
        /// </summary>
        [HttpPut("url/{id}")]
        public async Task<IActionResult> UpdateUrl(int id, [FromBody] CreateUrlDataSourceDto dto)
        {
            var success = await _dataSourceService.UpdateUrlAsync(id, dto);
            return success ? NoContent() : NotFound();
        }

        // ==================== File Data Source ====================

        /// <summary>
        /// Create File data source
        /// </summary>
        [HttpPost("file")]
        public async Task<ActionResult<int>> CreateFile([FromBody] CreateFileDataSourceDto dto)
        {
            var id = await _dataSourceService.CreateFileAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        /// <summary>
        /// Update File data source
        /// </summary>
        [HttpPut("file/{id}")]
        public async Task<IActionResult> UpdateFile(int id, [FromBody] CreateFileDataSourceDto dto)
        {
            var success = await _dataSourceService.UpdateFileAsync(id, dto);
            return success ? NoContent() : NotFound();
        }

        // ==================== FTP Data Source ====================

        /// <summary>
        /// Create FTP data source
        /// </summary>
        [HttpPost("ftp")]
        public async Task<ActionResult<int>> CreateFtp([FromBody] CreateFtpDataSourceDto dto)
        {
            var id = await _dataSourceService.CreateFtpAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        /// <summary>
        /// Update FTP data source
        /// </summary>
        [HttpPut("ftp/{id}")]
        public async Task<IActionResult> UpdateFtp(int id, [FromBody] CreateFtpDataSourceDto dto)
        {
            var success = await _dataSourceService.UpdateFtpAsync(id, dto);
            return success ? NoContent() : NotFound();
        }

        // ==================== Database Data Source ====================

        /// <summary>
        /// Create Database data source
        /// </summary>
        [HttpPost("database")]
        public async Task<ActionResult<int>> CreateDatabase([FromBody] CreateDatabaseDataSourceDto dto)
        {
            var id = await _dataSourceService.CreateDatabaseAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        /// <summary>
        /// Update Database data source
        /// </summary>
        [HttpPut("database/{id}")]
        public async Task<IActionResult> UpdateDatabase(int id, [FromBody] CreateDatabaseDataSourceDto dto)
        {
            var success = await _dataSourceService.UpdateDatabaseAsync(id, dto);
            return success ? NoContent() : NotFound();
        }

        // ==================== Azure Blob Data Source ====================

        /// <summary>
        /// Create Azure Blob data source
        /// </summary>
        [HttpPost("azureblob")]
        public async Task<ActionResult<int>> CreateAzureBlob([FromBody] CreateAzureBlobDataSourceDto dto)
        {
            var id = await _dataSourceService.CreateAzureBlobAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        /// <summary>
        /// Update Azure Blob data source
        /// </summary>
        [HttpPut("azureblob/{id}")]
        public async Task<IActionResult> UpdateAzureBlob(int id, [FromBody] CreateAzureBlobDataSourceDto dto)
        {
            var success = await _dataSourceService.UpdateAzureBlobAsync(id, dto);
            return success ? NoContent() : NotFound();
        }

        // ==================== S3 Data Source ====================

        /// <summary>
        /// Create S3 data source
        /// </summary>
        [HttpPost("s3")]
        public async Task<ActionResult<int>> CreateS3([FromBody] CreateS3DataSourceDto dto)
        {
            var id = await _dataSourceService.CreateS3Async(dto);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        /// <summary>
        /// Update S3 data source
        /// </summary>
        [HttpPut("s3/{id}")]
        public async Task<IActionResult> UpdateS3(int id, [FromBody] CreateS3DataSourceDto dto)
        {
            var success = await _dataSourceService.UpdateS3Async(id, dto);
            return success ? NoContent() : NotFound();
        }

        // ==================== Common Operations ====================

        /// <summary>
        /// Delete data source by ID
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _dataSourceService.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }

        /// <summary>
        /// Toggle data source enabled/disabled
        /// </summary>
        [HttpPatch("{id}/toggle")]
        public async Task<IActionResult> Toggle(int id)
        {
            var (success, isEnabled) = await _dataSourceService.ToggleAsync(id);

            if (!success)
            {
                return NotFound();
            }

            return Ok(new { Id = id, IsEnabled = isEnabled });
        }
    }
}
