namespace data_protection_common.Services
{
    /// <summary>
    /// Interface for Data Protection based cryptography service
    /// </summary>
    public interface IDataProtectionCryptographyService
    {
        /// <summary>
        /// Encrypts plain text using ASP.NET Core Data Protection
        /// </summary>
        /// <param name="plainText">Text to encrypt</param>
        /// <returns>Protected string</returns>
        string Encrypt(string plainText);

        /// <summary>
        /// Decrypts protected text using ASP.NET Core Data Protection
        /// </summary>
        /// <param name="protectedText">Protected string</param>
        /// <returns>Original plain text</returns>
        string Decrypt(string protectedText);
    }
}
