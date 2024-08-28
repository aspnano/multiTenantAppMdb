using Microsoft.EntityFrameworkCore;
using multiTenantApp.Models;

namespace multiTenantApp.Persistence.Contexts
{
    // in a multi-database scenerio, this context manages tables that are generated in only the main database

    //---------------------------------- CLI COMMANDS --------------------------------------------------

    // when scaffolding database migrations, you must specify which context (TenantDbContext), -o is the output directory, use the following command:

    // add-migration -Context TenantDbContext -o Persistence/Migrations/TenantDb MigrationName
    // update-database -Context TenantDbContext

    //--------------------------------------------------------------------------------------------------

    public class TenantDbContext : DbContext
    {
        public TenantDbContext(DbContextOptions<TenantDbContext> options)
        : base(options)
        {
        }

        public DbSet<Tenant> Tenants { get; set; }

    }
}
