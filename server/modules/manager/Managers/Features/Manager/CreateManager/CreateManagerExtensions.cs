using FluentValidation;
using Managers.Features.Manager.CreateManager.Handler;
using Microsoft.Extensions.DependencyInjection;

namespace Managers.Features.Manager.CreateManager;

public static class CreateManagerExtensions
{
    public static IServiceCollection AddCreateManager(this IServiceCollection services)
    {
        return services
                .AddValidatorsFromAssemblyContaining<CreateManagerCommandValidator>(ServiceLifetime.Transient)
            ;
    }
}