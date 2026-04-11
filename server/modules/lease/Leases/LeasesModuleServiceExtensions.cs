using Leases.Data;
using Leases.Data.Repositories;
using Leases.Features;
using Leases.Features.Property.GetPropertyLeases;
using Leases.Features.Tenant.GetLeasesForTenant;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Leases;

public static class LeasesModuleServiceExtensions
{
    public static IServiceCollection AddLeasesModule(this IServiceCollection services,
        ConfigurationManager config,
        ILogger logger,
        List<System.Reflection.Assembly> mediatRAssemblies)
    {
        string? connectionString = config.GetConnectionString("LeasesConnectionString");
        services
            // Add and configure db context
            .AddDbContext<LeaseDbContext>(options => options
                .UseNpgsql(connectionString)
            )
            .AddScoped<ILeaseRepository, LeaseRepository>()
            .AddScoped<IPropertyRepository, PropertyRepository>()
            .AddScoped<ITenantRepository, TenantRepository>()

            // Add features
            .AddGetLeasesForTenant()
            .AddGetPropertyLeases()

            // Add gRPC service
            .AddGrpc()
            ;
        mediatRAssemblies.Add(typeof(LeasesModuleServiceExtensions).Assembly);
        logger.Information("{Module} module services registered", Constants.ModuleName);
        return services;
    }

    public static void AddLeaseModuleConsumers(this IRegistrationConfigurator configurator)
    {
        configurator.AddConsumers(typeof(LeaseEventsConsumers));
    }

    public static IEndpointRouteBuilder MapLeaseModule(this IEndpointRouteBuilder endpoints)
    {
        _ = endpoints
            .MapGrpcService<LeaseGrpcService>();
        ;
        return endpoints;
    }


}