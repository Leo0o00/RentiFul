using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Tenants.Data.Repositories;
using Tenants.Features.Tenant.CreateTenant.Handler;

namespace Tenants.Features.Tenant.GetTenantDetails;

public static class GetTenantDetailsExtensions
{
    public static IServiceCollection AddGetTenantDetails(this IServiceCollection services)
    {
        return services
                .AddValidatorsFromAssemblyContaining<GetTenantDetailsQueryValidator>(ServiceLifetime.Transient)
            ;
    }
}