using FluentValidation;
using Managers.Features.Manager.CreateManager.Handler;
using Managers.Features.Manager.GetManagerDetails.Handler;
using Microsoft.Extensions.DependencyInjection;

namespace Managers.Features.Manager.GetManagerDetails;

public static class GetManagerDetailsExtensions
{
    public static IServiceCollection AddGetManagerDetails(this IServiceCollection services)
    {
        return services
                .AddSingleton<GetManagerDetailsMapper>()
                .AddValidatorsFromAssemblyContaining<GetManagerDetailsQueryValidator>(ServiceLifetime.Transient)
            ;
    }
}