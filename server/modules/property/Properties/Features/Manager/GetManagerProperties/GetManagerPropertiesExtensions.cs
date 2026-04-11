using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Properties.Features.Manager.GetManagerProperties.Handler;

namespace Properties.Features.Manager.GetManagerProperties;

public static class GetManagerPropertiesExtensions
{
    public static IServiceCollection AddGetManagerProperties(this IServiceCollection services)
    {
        return services
                // .AddSingleton<GetManagerPropertiesMapper>()
                .AddValidatorsFromAssemblyContaining<GetManagerPropertiesQueryValidator>(ServiceLifetime.Transient)
            ;
    }
}