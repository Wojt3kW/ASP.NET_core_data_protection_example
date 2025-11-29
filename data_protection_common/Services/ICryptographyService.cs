namespace data_protection_common.Services
{
    /// <summary>
    /// Interface for cryptography service
    /// </summary>
    public interface ICryptographyService
    {
        /// <summary>
        /// Encrypts plain text using AES-256
        /// </summary>
        /// <param name="plainText">Text to encrypt</param>
        /// <returns>Base64 encoded encrypted string (salt + IV + ciphertext)</returns>
        string Encrypt(string plainText);

        /// <summary>
        /// Decrypts encrypted text
        /// </summary>
        /// <param name="encryptedText">Base64 encoded encrypted string</param>
        /// <returns>Original plain text</returns>
        string Decrypt(string encryptedText);

        /// <summary>
        /// Encrypts plain text with custom salt
        /// </summary>
        /// <param name="plainText">Text to encrypt</param>
        /// <param name="salt">Custom salt bytes</param>
        /// <returns>Base64 encoded encrypted string</returns>
        string Encrypt(string plainText, byte[] salt);

        /// <summary>
        /// Generates a random salt
        /// </summary>
        /// <param name="size">Salt size in bytes (default 16)</param>
        /// <returns>Random salt bytes</returns>
        byte[] GenerateSalt(int size = 16);
    }
}
