using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Properties.Features.Property.CreateProperty.Handler;

namespace Properties.Features.Property.CreateProperty;

public static class CreatePropertyExtensions
{
    public static IServiceCollection AddCreateProperty(this IServiceCollection services)
    {
        return services
                .AddSingleton<CreatePropertyMapper>()
                .AddHttpClient()
                .AddValidatorsFromAssemblyContaining<CreatePropertyCommandValidator>(ServiceLifetime.Transient)
            ;
    }
}