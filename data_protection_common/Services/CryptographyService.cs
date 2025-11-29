using System.Security.Cryptography;
using System.Text;

namespace data_protection_common.Services
{
    /// <summary>
    /// Cryptography service using AES-256-CBC with PBKDF2 key derivation
    /// </summary>
    public class CryptographyService : ICryptographyService
    {
        // Hardcoded encryption key (in production, use Key Vault)
        // This is a 256-bit key encoded as Base64
        private const string HardcodedKey = "K7gNU3sdo+OL0wNhqoVWhr3g6s1xYv72ol/pe/Unols=";

        private const int Pbkdf2Iterations = 600000;

        // Salt size in bytes (128 bits)
        private const int SaltSize = 16;

        // IV size for AES (128 bits)
        private const int IvSize = 16;

        // Key size for AES-256 (256 bits)
        private const int KeySize = 32;

        /// <inheritdoc/>
        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException(nameof(plainText));

            var salt = GenerateSalt(SaltSize);
            return Encrypt(plainText, salt);
        }

        /// <inheritdoc/>
        public string Encrypt(string plainText, byte[] salt)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException(nameof(plainText));

            if (salt == null || salt.Length < SaltSize)
                throw new ArgumentException($"Salt must be at least {SaltSize} bytes", nameof(salt));

            // Derive key from hardcoded key using PBKDF2 (FIPS-compliant)
            var derivedKey = DeriveKey(salt);

            // Generate random IV
            var iv = GenerateSalt(IvSize);

            // Encrypt using AES-256-CBC (FIPS-compliant)
            byte[] encryptedBytes;
            using (var aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.KeySize = 256;
                aes.Key = derivedKey;
                aes.IV = iv;

                using var encryptor = aes.CreateEncryptor();
                var plainBytes = Encoding.UTF8.GetBytes(plainText);
                encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
            }

            // Combine: salt (16) + IV (16) + ciphertext
            var result = new byte[salt.Length + iv.Length + encryptedBytes.Length];
            Buffer.BlockCopy(salt, 0, result, 0, salt.Length);
            Buffer.BlockCopy(iv, 0, result, salt.Length, iv.Length);
            Buffer.BlockCopy(encryptedBytes, 0, result, salt.Length + iv.Length, encryptedBytes.Length);

            return Convert.ToBase64String(result);
        }

        /// <inheritdoc/>
        public string Decrypt(string encryptedText)
        {
            if (string.IsNullOrEmpty(encryptedText))
                throw new ArgumentNullException(nameof(encryptedText));

            var fullCipher = Convert.FromBase64String(encryptedText);

            if (fullCipher.Length < SaltSize + IvSize + 1)
                throw new ArgumentException("Invalid encrypted text format", nameof(encryptedText));

            // Extract salt, IV, and ciphertext
            var salt = new byte[SaltSize];
            var iv = new byte[IvSize];
            var cipherBytes = new byte[fullCipher.Length - SaltSize - IvSize];

            Buffer.BlockCopy(fullCipher, 0, salt, 0, SaltSize);
            Buffer.BlockCopy(fullCipher, SaltSize, iv, 0, IvSize);
            Buffer.BlockCopy(fullCipher, SaltSize + IvSize, cipherBytes, 0, cipherBytes.Length);

            // Derive key from hardcoded key using PBKDF2
            var derivedKey = DeriveKey(salt);

            // Decrypt using AES-256-CBC
            using var aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.KeySize = 256;
            aes.Key = derivedKey;
            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor();
            var plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

            return Encoding.UTF8.GetString(plainBytes);
        }

        /// <inheritdoc/>
        public byte[] GenerateSalt(int size = 16)
        {
            var salt = new byte[size];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);
            return salt;
        }

        /// <summary>
        /// Derives encryption key using PBKDF2-SHA256 (FIPS-compliant)
        /// </summary>
        private byte[] DeriveKey(byte[] salt)
        {
            var masterKey = Convert.FromBase64String(HardcodedKey);

            // PBKDF2 with SHA-256 is FIPS 140-2 compliant
            using var pbkdf2 = new Rfc2898DeriveBytes(
                password: masterKey,
                salt: salt,
                iterations: Pbkdf2Iterations,
                hashAlgorithm: HashAlgorithmName.SHA256);

            return pbkdf2.GetBytes(KeySize);
        }
    }
}
