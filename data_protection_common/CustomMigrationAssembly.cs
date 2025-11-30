using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace data_protection_common
{
#pragma warning disable EF1001 // Internal EF Core API usage.
    /// <summary>
    /// Custom migration assembly that injects DbContext into migrations.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <b>WARNING:</b> This class inherits from <c>MigrationsAssembly</c>, which is an internal API in Entity Framework Core.
    /// Internal APIs may change or be removed in future EF Core releases, potentially breaking this code.
    /// This implementation was tested with EF Core 8.0.0.
    /// If upgrading EF Core, review this class for compatibility and update as needed.
    /// See: https://docs.microsoft.com/en-us/ef/core/miscellaneous/unsupported-apis
    /// </para>
    /// </remarks>
    internal class CustomMigrationAssembly : MigrationsAssembly
    {
        private readonly DbContext _context;

        public CustomMigrationAssembly(
            ICurrentDbContext currentContext,
            IDbContextOptions options,
            IMigrationsIdGenerator idGenerator,
            IDiagnosticsLogger<DbLoggerCategory.Migrations> logger)
            : base(currentContext, options, idGenerator, logger)
        {
            _context = currentContext.Context;
        }

        public override Migration CreateMigration(TypeInfo migrationClass, string activeProvider)
        {
            if (activeProvider == null)
            {
                throw new ArgumentNullException(nameof(activeProvider));
            }

            // Check if migration has constructor that accepts DbContext
            var constructor = migrationClass.GetConstructors()
                .FirstOrDefault(c => c.GetParameters().Length == 1 
                    && c.GetParameters()[0].ParameterType.IsAssignableTo(typeof(DbContext)));

            if (constructor != null)
            {
                var instance = (Migration)Activator.CreateInstance(migrationClass.AsType(), _context)!;
                instance.ActiveProvider = activeProvider;
                return instance;
            }

            return base.CreateMigration(migrationClass, activeProvider);
        }
    }
#pragma warning restore EF1001 // Internal EF Core API usage.
}
