using Microsoft.AspNetCore.DataProtection;

namespace data_protection_common.Services
{
    /// <summary>
    /// Cryptography service using ASP.NET Core Data Protection API
    /// </summary>
    internal class DataProtectionCryptographyService : IDataProtectionCryptographyService
    {
        private readonly IDataProtector _protector;

        // Purpose string for creating the protector - should be unique per application/use case
        private const string Purpose = "DataSource.SensitiveFields.v2";

        public DataProtectionCryptographyService(IDataProtectionProvider dataProtectionProvider)
        {
            _protector = dataProtectionProvider.CreateProtector(Purpose);
        }

        /// <inheritdoc/>
        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException(nameof(plainText));

            return _protector.Protect(plainText);
        }

        /// <inheritdoc/>
        public string Decrypt(string protectedText)
        {
            if (string.IsNullOrEmpty(protectedText))
                throw new ArgumentNullException(nameof(protectedText));

            return _protector.Unprotect(protectedText);
        }
    }
}
