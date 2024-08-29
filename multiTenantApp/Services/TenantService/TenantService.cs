using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using multiTenantApp.Models;
using multiTenantApp.Persistence.Contexts;
using multiTenantApp.Services.TenantService.DTOs;

namespace multiTenantApp.Services.TenantService
{
    public class TenantService : ITenantService
    {

        private readonly BaseDbContext _baseDbContext; // database context
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        public TenantService(BaseDbContext baseDbContext, IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _baseDbContext = baseDbContext;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        public Tenant CreateTenant(CreateTenantRequest request)
        {

            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            SqlConnectionStringBuilder builder = new(connectionString);
            string mainDatabaseName = builder.InitialCatalog; // retrieve the database name
            string tenantDbName = mainDatabaseName + "-" + request.Id;
            builder.InitialCatalog = tenantDbName; // set new database name
            string modifiedConnectionString = builder.ConnectionString; // create new connection string

            Tenant tenant = new() // create a new tenant entity
            {
                Id = request.Id,
                Name = request.Name,
                ConnectionString = request.Isolated ? modifiedConnectionString : null,
            };

            
            try
            {
                if (request.Isolated == true)
                {
                    // create a new tenant database and bring current with any pending migrations from ApplicationDbContext
                    using IServiceScope scopeTenant = _serviceProvider.CreateScope();
                    ApplicationDbContext dbContext = scopeTenant.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    dbContext.Database.SetConnectionString(modifiedConnectionString);
                    if (dbContext.Database.GetPendingMigrations().Any()) 
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine($"Applying ApplicationDB Migrations for New '{request.Id}' tenant.");
                        Console.ResetColor();
                        dbContext.Database.Migrate();
                    }
                }

                // apply changes to base db context
                _baseDbContext.Add(tenant); // save tenant info
                _baseDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return tenant;
        }
    }
}
