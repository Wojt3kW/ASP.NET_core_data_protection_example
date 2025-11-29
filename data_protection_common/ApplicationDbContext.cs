using data_protection_common.Entities;
using Microsoft.EntityFrameworkCore;

namespace data_protection_common
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<DataSource> DataSources { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // TPH (Table-Per-Hierarchy) configuration - all types in a single table
            modelBuilder.Entity<DataSource>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Description).HasMaxLength(1000);
                
                // Discriminator determines the data source type
                entity.HasDiscriminator<string>("SourceType")
                    .HasValue<UrlDataSource>("Url")
                    .HasValue<FileDataSource>("File")
                    .HasValue<FtpDataSource>("Ftp")
                    .HasValue<DatabaseDataSource>("Database")
                    .HasValue<AzureBlobDataSource>("AzureBlob")
                    .HasValue<S3DataSource>("S3");
            });

            // UrlDataSource configuration
            // Sensitive fields: Password, ApiKey, BearerToken (encrypted in DataSourceService)
            // Encrypted fields need extra space for Base64 encoding (~1.4x original + IV overhead)
            modelBuilder.Entity<UrlDataSource>(entity =>
            {
                entity.Property(e => e.Url).HasMaxLength(2048);
                entity.Property(e => e.HttpMethod).HasMaxLength(10);
                entity.Property(e => e.AuthType).HasMaxLength(50);
                entity.Property(e => e.Username).HasMaxLength(255);
                entity.Property(e => e.Password).HasMaxLength(2048);      // Encrypted - increased from 1024
                entity.Property(e => e.ApiKey).HasMaxLength(2048);        // Encrypted - increased from 1024
                entity.Property(e => e.BearerToken).HasMaxLength(8000);   // Encrypted - increased from 4000
            });

            // FileDataSource configuration
            modelBuilder.Entity<FileDataSource>(entity =>
            {
                entity.Property(e => e.FilePath).HasMaxLength(1024);
                entity.Property(e => e.FileType).HasMaxLength(50);
                entity.Property(e => e.Encoding).HasMaxLength(50);
                entity.Property(e => e.Delimiter).HasMaxLength(10);
            });

            // FtpDataSource configuration
            // Sensitive fields: Password, PrivateKeyPath (encrypted in DataSourceService)
            // Encrypted fields need extra space for Base64 encoding (~1.4x original + IV overhead)
            modelBuilder.Entity<FtpDataSource>(entity =>
            {
                entity.Property(e => e.Host).HasMaxLength(255);
                entity.Property(e => e.Username).HasMaxLength(255);
                entity.Property(e => e.Password).HasMaxLength(2048);      // Encrypted - increased from 1024
                entity.Property(e => e.RemotePath).HasMaxLength(1024);
                entity.Property(e => e.PrivateKeyPath).HasMaxLength(4096); // Encrypted - increased from 2048
            });

            // DatabaseDataSource configuration
            // Sensitive fields: ConnectionString (encrypted in DataSourceService)
            // Encrypted fields need extra space for Base64 encoding (~1.4x original + IV overhead)
            modelBuilder.Entity<DatabaseDataSource>(entity =>
            {
                entity.Property(e => e.ConnectionString).HasMaxLength(8000); // Encrypted - increased from 4000
                entity.Property(e => e.DatabaseType).HasMaxLength(50);
                entity.Property(e => e.Schema).HasMaxLength(128);
                entity.Property(e => e.Query).HasMaxLength(4000);
            });

            // AzureBlobDataSource configuration
            // Sensitive fields: ConnectionString (encrypted in DataSourceService)
            // Encrypted fields need extra space for Base64 encoding (~1.4x original + IV overhead)
            modelBuilder.Entity<AzureBlobDataSource>(entity =>
            {
                entity.Property(e => e.ConnectionString).HasMaxLength(8000); // Encrypted - increased from 4000
                entity.Property(e => e.ContainerName).HasMaxLength(255);
                entity.Property(e => e.BlobName).HasMaxLength(1024);
                entity.Property(e => e.BlobPrefix).HasMaxLength(1024);
            });

            // S3DataSource configuration
            // Sensitive fields: AccessKey, SecretKey (encrypted in DataSourceService)
            // Encrypted fields need extra space for Base64 encoding (~1.4x original + IV overhead)
            modelBuilder.Entity<S3DataSource>(entity =>
            {
                entity.Property(e => e.BucketName).HasMaxLength(255);
                entity.Property(e => e.AccessKey).HasMaxLength(1024);     // Encrypted - increased from 512
                entity.Property(e => e.SecretKey).HasMaxLength(2048);     // Encrypted - increased from 1024
                entity.Property(e => e.Region).HasMaxLength(50);
                entity.Property(e => e.ObjectKey).HasMaxLength(1024);
                entity.Property(e => e.Prefix).HasMaxLength(1024);
            });
        }
    }
}
