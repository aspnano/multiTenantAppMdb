using Microsoft.EntityFrameworkCore;
using multiTenantApp.Models;

namespace multiTenantApp.Extensions
{
    public static class MultipleDatabaseExtensions
    {
        public static IServiceCollection AddAndMigrateTenantDatabases(this IServiceCollection services, IConfiguration configuration)
        {

            // Tenant Db Context (reference context) - get a list of tenants
            using IServiceScope scopeTenant = services.BuildServiceProvider().CreateScope();
            TenantDbContext tenantDbContext = scopeTenant.ServiceProvider.GetRequiredService<TenantDbContext>();

            if (tenantDbContext.Database.GetPendingMigrations().Any())
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Applying BaseDb Migrations.");
                Console.ResetColor();
                tenantDbContext.Database.Migrate(); // apply migrations on baseDbContext
            }


            List<Tenant> tenantsInDb = tenantDbContext.Tenants.ToList();

            string defaultConnectionString = configuration.GetConnectionString("DefaultConnection"); // read default connection string from appsettings.json

            foreach (Tenant tenant in tenantsInDb) // loop through all tenants, apply migrations on applicationDbContext
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
