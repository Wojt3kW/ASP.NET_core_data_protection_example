using data_protection_common.Entities;
using data_protection_common.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;

#nullable disable

namespace data_protection_common.SqlMigrations
{
    /// <inheritdoc />
    public partial class Migrate_data_to_DataProtection : Migration
    {
        // Encrypted columns per SourceType (discriminator value)
        private static readonly Dictionary<string, string[]> EncryptedColumnsBySourceType = new()
        {
            { "Url", new[] { nameof(UrlDataSource.Password), nameof(UrlDataSource.ApiKey), nameof(UrlDataSource.BearerToken) } },
            { "Ftp", new[] { nameof(FtpDataSource.Password), nameof(FtpDataSource.PrivateKeyPath) } },
            { "Database", new[] { nameof(DatabaseDataSource.ConnectionString) } },
            { "AzureBlob", new[] { nameof(AzureBlobDataSource.ConnectionString) } },
            { "S3", new[] { nameof(S3DataSource.AccessKey), nameof(S3DataSource.SecretKey) } }
        };

        private readonly ApplicationDbContext _dbContext;
        private readonly ICryptographyService _legacyCryptoService;

        public Migrate_data_to_DataProtection(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _legacyCryptoService = new CryptographyService();
        }

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Migrate encrypted data from legacy AES to Data Protection API
            var connection = _dbContext.Database.GetDbConnection();

            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }

            // Step 1: Collect all data from all source types first (close all readers before encryption)
            var allRecordsToMigrate = new List<(string SourceType, int Id, Dictionary<string, string> EncryptedValues)>();

            foreach (var (sourceType, columns) in EncryptedColumnsBySourceType)
            {
                CollectRecordsToMigrate(connection, sourceType, columns, allRecordsToMigrate);
            }

            // If there's no data to migrate, skip creating the crypto service
            if (allRecordsToMigrate.Count == 0)
            {
                return;
            }

            // Step 2: Create Data Protection service
            var newCryptoService = CreateDataProtectionService();

            // Step 3: Re-encrypt and update all records
            foreach (var (sourceType, id, encryptedValues) in allRecordsToMigrate)
            {
                var newValues = new Dictionary<string, string>();

                foreach (var (columnName, encryptedValue) in encryptedValues)
                {
                    try
                    {
                        // Decrypt with legacy AES service
                        var plainText = _legacyCryptoService.Decrypt(encryptedValue);

                        // Re-encrypt with Data Protection API
                        var newEncrypted = newCryptoService.Encrypt(plainText);

                        newValues[columnName] = newEncrypted;
                    }
                    catch
                    {
                        // Skip if decryption fails (might already be migrated or invalid data)
                    }
                }

                if (newValues.Count > 0)
                {
                    UpdateRecord(connection, id, newValues);
                }
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Cannot automatically reverse the encryption migration
            // Data would need to be restored from backup
            migrationBuilder.Sql("-- WARNING: Encryption migration cannot be automatically reversed");
        }

        private IDataProtectionCryptographyService CreateDataProtectionService()
        {
            // Create Data Protection provider with the same configuration as the application
            // We need to use the same DbContext to access the same keys stored in database
            var services = new ServiceCollection();

            // Register the existing DbContext instance for key persistence
            services.AddSingleton<ApplicationDbContext>(_dbContext);

            // Configure Data Protection with same settings as application
            services.AddDataProtectionApiWithKeysStoredInDbContext();

            var sp = services.BuildServiceProvider();
            var provider = sp.GetRequiredService<IDataProtectionProvider>();
            return new DataProtectionCryptographyService(provider);
        }

        private void CollectRecordsToMigrate(
            System.Data.Common.DbConnection connection,
            string sourceType,
            string[] columns,
            List<(string SourceType, int Id, Dictionary<string, string> EncryptedValues)> recordsList)
        {
            // Build SELECT query for encrypted columns
            var selectColumns = string.Join(", ", columns.Select(c => $"[{c}]"));
            var selectSql = $"SELECT [Id], {selectColumns} FROM [DataSources] WHERE [SourceType] = @sourceType";

            using var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = selectSql;

            var sourceTypeParam = selectCmd.CreateParameter();
            sourceTypeParam.ParameterName = "@sourceType";
            sourceTypeParam.Value = sourceType;
            selectCmd.Parameters.Add(sourceTypeParam);

            using var reader = selectCmd.ExecuteReader();
            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var encryptedValues = new Dictionary<string, string>();

                for (int i = 0; i < columns.Length; i++)
                {
                    var columnName = columns[i];
                    var columnIndex = i + 1; // +1 because Id is at index 0

                    if (!reader.IsDBNull(columnIndex) && !string.IsNullOrEmpty(reader.GetString(columnIndex)))
                    {
                        encryptedValues[columnName] = reader.GetString(columnIndex);
                    }
                }

                if (encryptedValues.Count > 0)
                {
                    recordsList.Add((sourceType, id, encryptedValues));
                }
            }
        }

        private void UpdateRecord(System.Data.Common.DbConnection connection, int id, Dictionary<string, string> newValues)
        {
            var setClauses = string.Join(", ", newValues.Keys.Select(k => $"[{k}] = @{k}"));
            var updateSql = $"UPDATE [DataSources] SET {setClauses} WHERE [Id] = @Id";

            using var updateCmd = connection.CreateCommand();
            updateCmd.CommandText = updateSql;

            var idParam = updateCmd.CreateParameter();
            idParam.ParameterName = "@Id";
            idParam.Value = id;
            updateCmd.Parameters.Add(idParam);

            foreach (var (columnName, newValue) in newValues)
            {
                var param = updateCmd.CreateParameter();
                param.ParameterName = $"@{columnName}";
                param.Value = newValue;
                updateCmd.Parameters.Add(param);
            }

            updateCmd.ExecuteNonQuery();
        }
    }
}
