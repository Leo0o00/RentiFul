using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Properties.Features.Property.GetAllPropertiesInIdList.Handler;

namespace Properties.Features.Property.GetAllPropertiesInIdList;

public static class GetAllPropertiesInIdListExtensions
{
    public static IServiceCollection AddGetAllPropertiesInIdList(this IServiceCollection services)
    {
        return services
                .AddValidatorsFromAssemblyContaining<GetAllPropertiesInIdListQueryValidator>(ServiceLifetime.Transient)
            ;
    }
}