using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace multiTenantApp.Persistence.Contexts
{
    public class TenantDbContextFactory : IDesignTimeDbContextFactory<TenantDbContext>
    {
        public TenantDbContext CreateDbContext(string[] args) // neccessary for EF migration designer to run on this context
        {

            // Build the configuration by reading from the appsettings.json file (requires Microsoft.Extensions.Configuration.Json Nuget Package)
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Retrieve the connection string from the configuration
            string connectionString = configuration.GetConnectionString("DefaultConnection");


            DbContextOptionsBuilder<TenantDbContext> optionsBuilder = new();
            _ = optionsBuilder.UseSqlServer(connectionString);
            return new TenantDbContext(optionsBuilder.Options);
        }
    }
}
