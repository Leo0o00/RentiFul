using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Tenants.Data.Repositories;
using Tenants.Features.Tenant.CreateTenant.Handler;

namespace Tenants.Features.Tenant.CreateTenant;

public static class CreateTenantExtensions
{
    public static IServiceCollection AddCreateTenant(this IServiceCollection services)
    {
        return services
                .AddSingleton<CreateTenantMapper>()
                .AddValidatorsFromAssemblyContaining<CreateTenantCommandValidator>(ServiceLifetime.Transient)
            ;
    }
}