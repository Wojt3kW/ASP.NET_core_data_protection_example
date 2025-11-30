using data_protection_common.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;

namespace data_protection_common
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register dependencies for ApplicationDbContext and related services
        /// </summary>
        public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);

#if DEBUG
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
#endif

                options.ReplaceService<IMigrationsAssembly, CustomMigrationAssembly>();
            }, ServiceLifetime.Scoped);

            return services;
        }

        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            // Cryptography service (AES-256-CBC)
            services.AddScoped<ICryptographyService, CryptographyService>();

            // New Data Protection based cryptography service
            services.AddScoped<IDataProtectionCryptographyService, DataProtectionCryptographyService>();

            services.AddScoped<IDataSourceService, DataSourceService>();

            return services;
        }

        public static IServiceCollection AddDataProtectionApiWithKeysStoredInDbContext(this IServiceCollection services)
        {
            // Configure ASP.NET Core Data Protection with EF Core persistence
            // Keys are rotated every 10 days (default is 90 days, minimum is 7 days)
            services.AddDataProtection()
                .SetApplicationName("DataProtectionExample")
                .SetDefaultKeyLifetime(TimeSpan.FromDays(10))
                .PersistKeysToDbContext<ApplicationDbContext>();

            return services;
        }
    }
}
