using multiTenantApp.Models;
using multiTenantApp.Services.TenantService.DTOs;

namespace multiTenantApp.Services.TenantService
{
    public interface ITenantService
    {
        Tenant CreateTenant(CreateTenantRequest request);
    }
}
