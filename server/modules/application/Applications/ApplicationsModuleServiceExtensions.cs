using Applications.Data;
using Applications.Data.Repositories;
using Applications.Features;
using Applications.Features.Application.CreateApplication;
using Applications.Grpc;
using Applications.Sagas;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SharedContracts;

namespace Applications;

public static class ApplicationsModuleServiceExtensions
{
    public static IServiceCollection AddApplicationsModule(this IServiceCollection services,
        ConfigurationManager config,
        ILogger logger,
        List<System.Reflection.Assembly> mediatRAssemblies)
    {
        const string propertiesBaseHttp2AddressKey = "Downstream:Properties:BaseHttp2Address";
        const string leasesBaseHttp2AddressKey = "Downstream:Leases:BaseHttp2Address";
        const string tenantsBaseHttp2AddressKey = "Downstream:Tenants:BaseHttp2Address";
        const string managersBaseHttp2AddressKey = "Downstream:Managers:BaseHttp2Address";

        var propertiesBaseHttp2Address = config.GetValue<string>(propertiesBaseHttp2AddressKey) ?? throw new BaseHttp2AddressNotFoundException(propertiesBaseHttp2AddressKey);
        var leasesBaseHttp2Address = config.GetValue<string>(leasesBaseHttp2AddressKey) ?? throw new BaseHttp2AddressNotFoundException(leasesBaseHttp2AddressKey);
        var tenantsBaseHttp2Address = config.GetValue<string>(tenantsBaseHttp2AddressKey) ?? throw new BaseHttp2AddressNotFoundException(tenantsBaseHttp2AddressKey);
        var managersBaseHttp2Address = config.GetValue<string>(managersBaseHttp2AddressKey) ?? throw new BaseHttp2AddressNotFoundException(managersBaseHttp2AddressKey);


        string? connectionString = config.GetConnectionString("ApplicationsConnectionString");
        services
            // Add and configure db context
            .AddDbContext<ApplicationDbContext>(options => options
                .UseNpgsql(connectionString)
            )
            .AddScoped<IPropertyGrpcServiceClient, PropertyGrpcServiceClient>()
            .AddScoped<ILeaseGrpcServiceClient, LeaseGrpcServiceClient>()
            .AddScoped<ITenantGrpcServiceClient, TenantGrpcServiceClient>()
            .AddScoped<IManagerGrpcServiceClient, ManagerGrpcServiceClient>()

            .AddScoped<IApplicationRepository, ApplicationRepository>()
            .AddScoped<ITenantRepository, TenantRepository>()
            .AddScoped<IPropertyRepository, PropertyRepository>()
            // Add features
            .AddCreateApplication()
            ;

            // Add gRPC client services
        services.AddGrpcClient<LeaseQueries.LeaseQueriesClient>(client => { client.Address = new Uri(leasesBaseHttp2Address); });
        services.AddGrpcClient<PropertyQueries.PropertyQueriesClient>(client => { client.Address = new Uri(propertiesBaseHttp2Address); });
        services.AddGrpcClient<TenantQueries.TenantQueriesClient>(client => { client.Address = new Uri(tenantsBaseHttp2Address); });
        services.AddGrpcClient<ManagerQueries.ManagerQueriesClient>(client => { client.Address = new Uri(managersBaseHttp2Address); });

        mediatRAssemblies.Add(typeof(ApplicationsModuleServiceExtensions).Assembly);
        logger.Information("{Module} module services registered", Constants.ModuleName);
        return services;
    }

    public static void AddApplicationModuleConsumers(this IRegistrationConfigurator configurator)
    {
        configurator.AddConsumers(typeof(ApplicationEventConsumers));
    }

    public static void AddApplicationSagaStateMachine(this IRegistrationConfigurator configurator)
    {
        configurator.AddSagaStateMachine<UpdateApplicationSaga, UpdateApplicationSagaData>()
            .EntityFrameworkRepository(r =>
            {
                r.ExistingDbContext<ApplicationDbContext>();

                r.UsePostgres(schemaName: Constants.ModuleName.ToLower());
            });
    }

    public class BaseHttp2AddressNotFoundException : NotSupportedException
    {
        public BaseHttp2AddressNotFoundException(string key)
            : base($"Cannot find the following settings key: '{key}'. Cannot start the program without it.")
        {

        }
    }


}