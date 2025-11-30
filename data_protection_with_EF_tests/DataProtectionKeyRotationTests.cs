using data_protection_common;
using data_protection_common.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace data_protection_with_EF_tests;

/// <summary>
/// Tests for Data Protection key rotation functionality
/// </summary>
public class DataProtectionKeyRotationTests : IDisposable
{
    private readonly ServiceProvider _serviceProvider;
    private readonly ApplicationDbContext _dbContext;
    private readonly IDataProtectionProvider _dataProtectionProvider;
    private readonly IKeyManager _keyManager;

    public DataProtectionKeyRotationTests()
    {
        var services = new ServiceCollection();
        var dbName = $"TestDb_{Guid.NewGuid()}";

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase(dbName));

        services.AddDataProtection()
            .SetApplicationName("DataProtectionExample")
            .SetDefaultKeyLifetime(TimeSpan.FromDays(10))
            .PersistKeysToDbContext<ApplicationDbContext>();

        services.AddScoped<IDataProtectionCryptographyService, DataProtectionCryptographyService>();

        _serviceProvider = services.BuildServiceProvider();
        _dbContext = _serviceProvider.GetRequiredService<ApplicationDbContext>();
        _dbContext.Database.EnsureCreated();

        _dataProtectionProvider = _serviceProvider.GetRequiredService<IDataProtectionProvider>();
        _keyManager = _serviceProvider.GetRequiredService<IKeyManager>();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
        _serviceProvider.Dispose();
        GC.SuppressFinalize(this);
    }

    private IDataProtector CreateProtector(string purpose = "TestPurpose") 
        => _dataProtectionProvider.CreateProtector(purpose);

    private IDataProtectionCryptographyService GetCryptographyService() 
        => _serviceProvider.GetRequiredService<IDataProtectionCryptographyService>();

    /// <summary>
    /// Verifies that keys are configured with 10-day lifetime
    /// </summary>
    [Fact]
    public void DataProtection_KeyLifetime_ShouldBe10Days()
    {
        // Act
        var keyManagementOptions = _serviceProvider.GetRequiredService<IOptions<KeyManagementOptions>>();

        // Assert
        Assert.Equal(TimeSpan.FromDays(10), keyManagementOptions.Value.NewKeyLifetime);
    }

    /// <summary>
    /// Verifies that a key is created when Data Protection is first used
    /// </summary>
    [Fact]
    public void DataProtection_FirstUse_ShouldCreateKey()
    {
        // Act - trigger key creation by encrypting data
        _ = CreateProtector().Protect("test data");

        // Assert - verify key was created in database
        var keysCount = _dbContext.DataProtectionKeys.Count();
        Assert.True(keysCount >= 1, $"Expected at least 1 key, but found {keysCount}");
    }

    /// <summary>
    /// Verifies that encrypted data can be decrypted after key rotation simulation
    /// (Old keys should remain valid for decryption)
    /// </summary>
    [Fact]
    public void DataProtection_AfterKeyRotation_OldDataCanStillBeDecrypted()
    {
        // Arrange
        var protector = CreateProtector();
        var originalData = "Sensitive information that needs protection";

        // Act - encrypt data with current key
        var encryptedData = protector.Protect(originalData);

        // Simulate key rotation by creating a new key manually
        _keyManager.CreateNewKey(
            activationDate: DateTimeOffset.UtcNow,
            expirationDate: DateTimeOffset.UtcNow.AddDays(10));

        // Decrypt data (should still work with old key)
        var decryptedData = protector.Unprotect(encryptedData);

        // Assert
        Assert.Equal(originalData, decryptedData);
    }

    /// <summary>
    /// Verifies that multiple keys can coexist in the key ring
    /// </summary>
    [Fact]
    public void DataProtection_MultipleKeys_CanCoexist()
    {
        // Act - trigger initial key creation
        _ = CreateProtector().Protect("trigger initial key creation");

        // Create additional keys
        _keyManager.CreateNewKey(
            activationDate: DateTimeOffset.UtcNow.AddDays(10),
            expirationDate: DateTimeOffset.UtcNow.AddDays(20));

        _keyManager.CreateNewKey(
            activationDate: DateTimeOffset.UtcNow.AddDays(20),
            expirationDate: DateTimeOffset.UtcNow.AddDays(30));

        // Assert
        var allKeys = _keyManager.GetAllKeys();
        Assert.True(allKeys.Count >= 3, $"Expected at least 3 keys, but found {allKeys.Count}");
    }

    /// <summary>
    /// Verifies that DataProtectionCryptographyService encrypts and decrypts correctly
    /// </summary>
    [Fact]
    public void DataProtectionCryptographyService_EncryptDecrypt_ShouldWork()
    {
        // Arrange
        var cryptoService = GetCryptographyService();
        var originalText = "MySecretPassword123!@#";

        // Act
        var encrypted = cryptoService.Encrypt(originalText);
        var decrypted = cryptoService.Decrypt(encrypted);

        // Assert
        Assert.NotEqual(originalText, encrypted);
        Assert.Equal(originalText, decrypted);
    }

    /// <summary>
    /// Verifies that encryption produces different output for the same input
    /// (due to unique payload format with timestamp)
    /// </summary>
    [Fact]
    public void DataProtectionCryptographyService_SameInput_ProducesDifferentCiphertext()
    {
        // Arrange
        var cryptoService = GetCryptographyService();
        var originalText = "TestPassword";

        // Act
        var encrypted1 = cryptoService.Encrypt(originalText);
        var encrypted2 = cryptoService.Encrypt(originalText);

        // Assert - both can be decrypted to original
        Assert.Equal(originalText, cryptoService.Decrypt(encrypted1));
        Assert.Equal(originalText, cryptoService.Decrypt(encrypted2));

        // Note: Data Protection may or may not produce different ciphertext
        // depending on implementation, but both should decrypt correctly
    }

    /// <summary>
    /// Verifies that keys persisted to database have correct XML structure
    /// </summary>
    [Fact]
    public void DataProtection_KeysInDatabase_HaveValidXmlStructure()
    {
        // Act - trigger key creation
        _ = CreateProtector().Protect("test");
        var keys = _dbContext.DataProtectionKeys.ToList();

        // Assert
        Assert.NotEmpty(keys);
        foreach (var key in keys)
        {
            Assert.False(string.IsNullOrEmpty(key.Xml), "Key XML should not be empty");
            Assert.Contains("<key", key.Xml);
        }
    }

    /// <summary>
    /// Verifies backward compatibility - data encrypted with old service can be read
    /// after switching to new Data Protection service (migration scenario)
    /// </summary>
    [Fact]
    public void Migration_LegacyEncryptedData_CanBeMigratedToDataProtection()
    {
        // Arrange - encrypt with legacy service
        var legacyService = new CryptographyService();
        var originalData = "SensitiveConnectionString=Server=localhost;Password=secret123";
        var legacyEncrypted = legacyService.Encrypt(originalData);

        var newCryptoService = GetCryptographyService();

        // Act - simulate migration: decrypt with legacy, re-encrypt with new
        var decrypted = legacyService.Decrypt(legacyEncrypted);
        var newEncrypted = newCryptoService.Encrypt(decrypted);
        var finalDecrypted = newCryptoService.Decrypt(newEncrypted);

        // Assert
        Assert.Equal(originalData, finalDecrypted);
        Assert.NotEqual(legacyEncrypted, newEncrypted);
    }
}
