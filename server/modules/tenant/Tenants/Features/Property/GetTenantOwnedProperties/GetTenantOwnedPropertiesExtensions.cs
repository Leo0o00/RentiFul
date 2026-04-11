using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Tenants.Features.Property.GetTenantOwnedProperties.Handler;

namespace Tenants.Features.Property.GetTenantOwnedProperties;

public static class GetTenantOwnedPropertiesExtensions
{
    public static IServiceCollection AddGetTenantOwnedProperties(this IServiceCollection services)
    {
        return services
                .AddValidatorsFromAssemblyContaining<GetTenantOwnedPropertiesQueryValidator>(ServiceLifetime.Transient)
            ;
    }
}