using Microsoft.EntityFrameworkCore;
using multiTenantApp.Middleware;
using multiTenantApp.Persistence.Contexts;
using multiTenantApp.Persistence.Extensions;
using multiTenantApp.Services;
using multiTenantApp.Services.ProductService;
using multiTenantApp.Services.TenantService;

// NOTE: In this simple example app there is no seed method,
// so be sure to create a tenant before trying to create a product (use the create tenant endpoint)

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Current tenant service with scoped lifetime (created per each request)
builder.Services.AddScoped<ICurrentTenantService, CurrentTenantService>();

// adding a database service with configuration -- connection string read from appsettings.json
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<TenantDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAndMigrateTenantDatabases(builder.Configuration);

// CRUD services with transient lifetime
builder.Services.AddTransient<ITenantService, TenantService>();
builder.Services.AddTransient<IProductService, ProductService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<TenantResolver>();
app.MapControllers();

app.Run();
