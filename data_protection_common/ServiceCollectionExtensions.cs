using data_protection_common.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace data_protection_common
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds ApplicationDbContext, DataSourceService, CryptographyService and DbSeeder to the service collection
        /// </summary>
        public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddSingleton<ICryptographyService, CryptographyService>();
            services.AddScoped<IDataSourceService, DataSourceService>();
            services.AddScoped<IDbSeeder, DbSeeder>();

            return services;
        }
    }
}
