using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Tenants.Data.Repositories;
using Tenants.Features.Tenant.UpdateTenant.Handler;

namespace Tenants.Features.Tenant.UpdateTenant;

public static class UpdateTenantExtensions
{
    public static IServiceCollection AddUpdateTenant(this IServiceCollection services)
    {
        return services
                .AddValidatorsFromAssemblyContaining<UpdateTenantCommandValidator>(ServiceLifetime.Transient)
            ;
    }
}