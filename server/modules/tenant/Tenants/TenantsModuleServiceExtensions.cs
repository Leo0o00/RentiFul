using FastEndpoints;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Tenants.Data;
using Tenants.Data.Repositories;
using Tenants.Domain;
using Tenants.Features;
using Tenants.Features.Property;
using Tenants.Features.Property.AddFavoriteProperty;
using Tenants.Features.Property.GetTenantOwnedProperties;
using Tenants.Features.Property.RemoveFavoriteProperty;
using Tenants.Features.Tenant.CreateTenant;
using Tenants.Features.Tenant.GetTenantDetails;
using Tenants.Features.Tenant.UpdateTenant;

namespace Tenants;

public static class TenantsModuleServiceExtensions
{
    public static IServiceCollection AddTenantsModule(this IServiceCollection services,
        ConfigurationManager config,
        ILogger logger,
        List<System.Reflection.Assembly> mediatRAssemblies)
    {
        string? connectionString = config.GetConnectionString("TenantsConnectionString");
        services
            // Add and configure db context
            .AddDbContext<TenantDbContext>(options => options
                .UseNpgsql(connectionString)
            )
            .AddScoped<ITenantRepository,  TenantRepository>()
            .AddScoped<IPropertyRepository,  PropertyRepository>()
            // Add features
            .AddCreateTenant()
            .AddGetTenantDetails()
            .AddUpdateTenant()
            .AddGetTenantOwnedProperties()
            .AddAddFavoriteProperty()
            .AddRemoveFavoriteProperty()

            // Add gRPC service
            .AddGrpc()
            ;
        mediatRAssemblies.Add(typeof(TenantsModuleServiceExtensions).Assembly);
        logger.Information("{Module} module services registered", Constants.ModuleName);
        return services;
    }

    public static void AddTenantModuleConsumers(this IRegistrationConfigurator configurator)
    {
        configurator.AddConsumers(typeof(TenantEventConsumers));
    }

    public static IEndpointRouteBuilder MapTenantModule(this IEndpointRouteBuilder endpoints)
    {
        _ = endpoints
            .MapGrpcService<TenantGrpcService>();
        ;
        return endpoints;
    }
}