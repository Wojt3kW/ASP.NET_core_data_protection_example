using data_protection_common.Entities;
using data_protection_common.Services;

namespace data_protection_common.Extensions
{
    /// <summary>
    /// Extension methods for encrypting and decrypting sensitive data in DataSource entities
    /// </summary>
    public static class DataSourceEncryptionExtensions
    {
        /// <summary>
        /// Encrypts sensitive fields in the data source entity using Data Protection API
        /// </summary>
        public static T EncryptSensitiveData<T>(this T entity, IDataProtectionCryptographyService cryptographyService) where T : DataSource
        {
            switch (entity)
            {
                case UrlDataSource url:
                    url.Password = EncryptIfNotEmpty(url.Password, cryptographyService);
                    url.ApiKey = EncryptIfNotEmpty(url.ApiKey, cryptographyService);
                    url.BearerToken = EncryptIfNotEmpty(url.BearerToken, cryptographyService);
                    break;

                case FtpDataSource ftp:
                    ftp.Password = EncryptIfNotEmpty(ftp.Password, cryptographyService);
                    ftp.PrivateKeyPath = EncryptIfNotEmpty(ftp.PrivateKeyPath, cryptographyService);
                    break;

                case DatabaseDataSource db:
                    db.ConnectionString = EncryptIfNotEmpty(db.ConnectionString, cryptographyService) ?? string.Empty;
                    break;

                case AzureBlobDataSource azure:
                    azure.ConnectionString = EncryptIfNotEmpty(azure.ConnectionString, cryptographyService) ?? string.Empty;
                    break;

                case S3DataSource s3:
                    s3.AccessKey = EncryptIfNotEmpty(s3.AccessKey, cryptographyService);
                    s3.SecretKey = EncryptIfNotEmpty(s3.SecretKey, cryptographyService);
                    break;
            }

            return entity;
        }

        /// <summary>
        /// Decrypts sensitive fields in the data source entity using Data Protection API
        /// </summary>
        public static T DecryptSensitiveData<T>(this T entity, IDataProtectionCryptographyService cryptographyService) where T : DataSource
        {
            switch (entity)
            {
                case UrlDataSource url:
                    url.Password = DecryptIfNotEmpty(url.Password, cryptographyService);
                    url.ApiKey = DecryptIfNotEmpty(url.ApiKey, cryptographyService);
                    url.BearerToken = DecryptIfNotEmpty(url.BearerToken, cryptographyService);
                    break;

                case FtpDataSource ftp:
                    ftp.Password = DecryptIfNotEmpty(ftp.Password, cryptographyService);
                    ftp.PrivateKeyPath = DecryptIfNotEmpty(ftp.PrivateKeyPath, cryptographyService);
                    break;

                case DatabaseDataSource db:
                    db.ConnectionString = DecryptIfNotEmpty(db.ConnectionString, cryptographyService) ?? string.Empty;
                    break;

                case AzureBlobDataSource azure:
                    azure.ConnectionString = DecryptIfNotEmpty(azure.ConnectionString, cryptographyService) ?? string.Empty;
                    break;

                case S3DataSource s3:
                    s3.AccessKey = DecryptIfNotEmpty(s3.AccessKey, cryptographyService);
                    s3.SecretKey = DecryptIfNotEmpty(s3.SecretKey, cryptographyService);
                    break;
            }

            return entity;
        }

        /// <summary>
        /// Decrypts sensitive fields in multiple data source entities using Data Protection API
        /// </summary>
        public static IEnumerable<T> DecryptSensitiveData<T>(this IEnumerable<T> entities, IDataProtectionCryptographyService cryptographyService) where T : DataSource
        {
            foreach (var entity in entities)
            {
                entity.DecryptSensitiveData(cryptographyService);
            }
            return entities;
        }

        /// <summary>
        /// Decrypts sensitive fields in the data source entity (null-safe version) using Data Protection API
        /// </summary>
        /// <returns>The same entity or null</returns>
        public static T? DecryptSensitiveDataOrNull<T>(this T? entity, IDataProtectionCryptographyService cryptographyService) where T : DataSource
        {
            if (entity == null)
                return null;

            return entity.DecryptSensitiveData(cryptographyService);
        }

        private static string? EncryptIfNotEmpty(string? value, IDataProtectionCryptographyService cryptographyService)
        {
            return string.IsNullOrEmpty(value) ? value : cryptographyService.Encrypt(value);
        }

        private static string? DecryptIfNotEmpty(string? value, IDataProtectionCryptographyService cryptographyService)
        {
            return string.IsNullOrEmpty(value) ? value : cryptographyService.Decrypt(value);
        }
    }
}
