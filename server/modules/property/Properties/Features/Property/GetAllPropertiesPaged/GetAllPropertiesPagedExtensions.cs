using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Properties.Features.Property.GetAllPropertiesPaged.Handler;
using GetAllPropertiesPagedMapper = Properties.Features.Property.GetAllPropertiesPaged.Handler.GetAllPropertiesPagedMapper;

namespace Properties.Features.Property.GetAllPropertiesPaged;

public static class GetAllPropertiesPagedExtensions
{
    public static IServiceCollection AddGetAllPropertiesPaged(this IServiceCollection services)
    {
        return services
                .AddSingleton<GetAllPropertiesPagedMapper>()
                .AddValidatorsFromAssemblyContaining<GetAllPropertiesPagedQueryValidator>(ServiceLifetime.Transient)
            ;
    }
}