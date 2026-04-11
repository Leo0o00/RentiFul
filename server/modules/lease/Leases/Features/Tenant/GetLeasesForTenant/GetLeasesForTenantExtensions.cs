using FluentValidation;
using Leases.Features.Tenant.GetLeasesForTenant.Handler;
using Microsoft.Extensions.DependencyInjection;

namespace Leases.Features.Tenant.GetLeasesForTenant;

public static class GetLeasesForTenantExtensions
{
    public static IServiceCollection AddGetLeasesForTenant(this IServiceCollection services)
    {
        return services
                .AddValidatorsFromAssemblyContaining<GetLeasesForTenantQueryValidator>(ServiceLifetime.Transient)
            ;
    }
}