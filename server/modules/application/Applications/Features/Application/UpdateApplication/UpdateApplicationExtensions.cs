using Applications.Features.Application.UpdateApplication.Handler;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Applications.Features.Application.UpdateApplication;

public static class UpdateApplicationExtensions
{
    public static IServiceCollection AddCreateApplication(this IServiceCollection services)
    {
        return services
                .AddValidatorsFromAssemblyContaining<UpdateApplicationCommandValidator>(ServiceLifetime.Transient)
            ;
    }
}