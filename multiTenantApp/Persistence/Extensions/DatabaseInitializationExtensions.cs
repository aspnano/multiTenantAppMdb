using Microsoft.EntityFrameworkCore;
using multiTenantApp.Models;
using multiTenantApp.Persistence.Contexts;

namespace multiTenantApp.Persistence.Extensions
{
    public static class DatabaseInitializationExtensions // renamed from MultipleDatabaseExtentions in video
    {
        public static IServiceCollection AddAndMigrateTenantDatabases(this IServiceCollection services, IConfiguration configuration)
        {

            // Base Context (central db) - get a list of tenants
            using IServiceScope scopeTenant = services.BuildServiceProvider().CreateScope();
            BaseDbContext baseDbContext = scopeTenant.ServiceProvider.GetRequiredService<BaseDbContext>();

            if (baseDbContext.Database.GetPendingMigrations().Any())
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Applying BaseDb Migrations.");
                Console.ResetColor();
                baseDbContext.Database.Migrate(); // apply migrations on baseDbContext
            }


            List<Tenant> tenantsInDb = baseDbContext.Tenants.ToList();

            string defaultConnectionString = configuration.GetConnectionString("DefaultConnection"); // read default connection string from appsettings.json

            foreach (Tenant tenant in tenantsInDb)
            {
                string connectionString = string.IsNullOrEmpty(tenant.ConnectionString) ? defaultConnectionString : tenant.ConnectionString;

                // Application Db Context (app - per tenant)
                using IServiceScope scopeApplication = services.BuildServiceProvider().CreateScope();
                ApplicationDbContext dbContext = scopeApplication.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.SetConnectionString(connectionString);
                if (dbContext.Database.GetPendingMigrations().Any())
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"Applying Migrations for '{tenant.Id}' tenant.");
                    Console.ResetColor();
                    dbContext.Database.Migrate();
                }
            }

            return services;
        }

    }
}
