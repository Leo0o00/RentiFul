using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Properties.Features.Property.GetPropertyMinInfo.Handler;

namespace Properties.Features.Property.GetPropertyMinInfo;

public static class GetPropertyMinInfo
{
    public static IServiceCollection AddGetPropertyMinInfo(this IServiceCollection services)
    {
        return services
                // .AddSingleton<GetAllPropertiesPagedMapper>()
                .AddValidatorsFromAssemblyContaining<GetPropertyMinInfoQueryValidator>(ServiceLifetime.Transient)
            ;
    }
}