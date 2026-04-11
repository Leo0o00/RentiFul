using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Properties.Features.Property.GetPropertyDetails.Handler;

namespace Properties.Features.Property.GetPropertyDetails;

public static class GetPropertyDetailsExtensions
{
    public static IServiceCollection AddGetPropertyDetails(this IServiceCollection services)
    {
        return services
                // .AddSingleton<GetAllPropertiesPagedMapper>()
                .AddValidatorsFromAssemblyContaining<GetPropertyDetailsQueryValidator>(ServiceLifetime.Transient)
            ;
    }
}