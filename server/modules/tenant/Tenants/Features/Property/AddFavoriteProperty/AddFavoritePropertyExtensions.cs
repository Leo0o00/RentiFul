using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Tenants.Data.Repositories;
using Tenants.Features.Property.AddFavoriteProperty.Handler;

namespace Tenants.Features.Property.AddFavoriteProperty;

public static class AddFavoritePropertyExtensions
{
    public static IServiceCollection AddAddFavoriteProperty(this IServiceCollection services)
    {
        return services
                .AddScoped<IPropertyRepository, PropertyRepository>()
                .AddValidatorsFromAssemblyContaining<AddFavoritePropertyCommandValidator>(ServiceLifetime.Transient)
            ;
    }
}