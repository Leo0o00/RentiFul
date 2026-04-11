using Managers.Data;
using Managers.Data.Repositories;
using Managers.Features;
using Managers.Features.Manager.CreateManager;
using Managers.Features.Manager.GetManagerDetails;
using Managers.Features.Manager.UpdateManager;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Managers;

public static class ManagersModuleServiceExtensions
{
    public static IServiceCollection AddManagersModule(this IServiceCollection services,
        ConfigurationManager config,
        ILogger logger,
        List<System.Reflection.Assembly> mediatRAssemblies)
    {
        string? connectionString = config.GetConnectionString("ManagersConnectionString");
        services
            // Add and configure db context
            .AddDbContext<ManagerDbContext>(options => options
                .UseNpgsql(connectionString)
            )
            .AddScoped<IManagerRepository,  ManagerRepository>()
            // Add features
            .AddCreateManager()
            .AddGetManagerDetails()
            .AddUpdateManager()

            // Add Grpc service
            .AddGrpc()
            ;
        mediatRAssemblies.Add(typeof(ManagersModuleServiceExtensions).Assembly);
        logger.Information("{Module} module services registered", Constants.ModuleName);
        return services;
    }

    public static IEndpointRouteBuilder MapManagerModule(this IEndpointRouteBuilder endpoints)
    {
        _ = endpoints

            .MapGrpcService<ManagerGrpcService>();
        ;
        return endpoints;
    }
    
}