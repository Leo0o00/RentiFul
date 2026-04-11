using Applications.Features.Application.CreateApplication.Handler;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Applications.Features.Application.CreateApplication;

public static class CreateApplicationExtensions
{
    public static IServiceCollection AddCreateApplication(this IServiceCollection services)
    {
        return services
                .AddValidatorsFromAssemblyContaining<CreateApplicationCommandValidator>(ServiceLifetime.Transient)
            ;
    }
}