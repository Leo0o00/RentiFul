using FluentValidation;
using Leases.Features.Property.GetPropertyLeases.Handler;
using Microsoft.Extensions.DependencyInjection;

namespace Leases.Features.Property.GetPropertyLeases;

public static class GetPropertyLeasesExtensions
{
    public static IServiceCollection AddGetPropertyLeases(this IServiceCollection services)
    {
        return services
                .AddValidatorsFromAssemblyContaining<GetPropertyLeasesQueryValidator>(ServiceLifetime.Transient)
            ;
    }
}