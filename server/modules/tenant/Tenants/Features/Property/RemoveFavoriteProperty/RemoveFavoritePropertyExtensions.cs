using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Tenants.Data.Repositories;
using Tenants.Features.Property.AddFavoriteProperty.Handler;
using Tenants.Features.Property.RemoveFavoriteProperty.Handler;

namespace Tenants.Features.Property.RemoveFavoriteProperty;

public static class RemoveFavoritePropertyExtensions
{
    public static IServiceCollection AddRemoveFavoriteProperty(this IServiceCollection services)
    {
        return services
                .AddScoped<IPropertyRepository, PropertyRepository>()
                .AddValidatorsFromAssemblyContaining<RemoveFavoritePropertyCommandValidator>(ServiceLifetime.Transient)
            ;
    }
}