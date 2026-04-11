using FluentValidation;
using Managers.Features.Manager.UpdateManager.Handler;
using Microsoft.Extensions.DependencyInjection;

namespace Managers.Features.Manager.UpdateManager;

public static class UpdateManagerExtensions
{
    public static IServiceCollection AddUpdateManager(this IServiceCollection services)
    {
        return services
                .AddValidatorsFromAssemblyContaining<UpdateManagerCommandValidator>(ServiceLifetime.Transient)
            ;
    }
}