using Microsoft.EntityFrameworkCore;
using multiTenantApp.Models;

namespace multiTenantApp.Persistence.Contexts
{
    // in a multi-database scenerio, this context manages tables that are generated in only the main database

    //---------------------------------- CLI COMMANDS --------------------------------------------------

    // when scaffolding database migrations, you must specify which context (BaseDbContext), -o is the output directory, use the following command:

    // add-migration -Context BaseDbContext -o Persistence/Migrations/BaseDb MigrationName
    // update-database -Context BaseDbContext


    //--------------------------------------------------------------------------------------------------

    public class BaseDbContext : DbContext 
    {
        public BaseDbContext(DbContextOptions<BaseDbContext> options)
        : base(options)
        {
        }

        public DbSet<Tenant> Tenants { get; set; }

    }
}
