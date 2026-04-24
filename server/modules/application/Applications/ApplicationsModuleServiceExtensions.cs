using Applications.Data;
using Applications.Data.Repositories;
using Applications.Features;
using Applications.Features.Application.CreateApplication;
using Applications.Grpc;
using Applications.Sagas;
using MassTransit;
using Microsoft.AspNetCore.Builder;
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
        var allowInsecureTls = ShouldAllowInsecureTls(config);


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

        // Allow containerized loopback gRPC to work with mounted development certificates.
        AddGrpcClient<LeaseQueries.LeaseQueriesClient>(services, leasesBaseHttp2Address, allowInsecureTls);
        AddGrpcClient<PropertyQueries.PropertyQueriesClient>(services, propertiesBaseHttp2Address, allowInsecureTls);
        AddGrpcClient<TenantQueries.TenantQueriesClient>(services, tenantsBaseHttp2Address, allowInsecureTls);
        AddGrpcClient<ManagerQueries.ManagerQueriesClient>(services, managersBaseHttp2Address, allowInsecureTls);

        mediatRAssemblies.Add(typeof(ApplicationsModuleServiceExtensions).Assembly);
        logger.Information("{Module} module services registered", Constants.ModuleName);
        return services;
    }

    private static void AddGrpcClient<TClient>(
        IServiceCollection services,
        string address,
        bool allowInsecureTls)
        where TClient : class
    {
        var grpcClientBuilder = services.AddGrpcClient<TClient>(client => { client.Address = new Uri(address); });

        if (!allowInsecureTls ||
            !Uri.TryCreate(address, UriKind.Absolute, out var downstreamUri) ||
            !string.Equals(downstreamUri.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase) ||
            !downstreamUri.IsLoopback)
        {
            return;
        }

        grpcClientBuilder.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        });
    }

    private static bool ShouldAllowInsecureTls(IConfiguration configuration)
    {
        if (configuration.GetValue<bool>("Downstream:AllowInsecureTls"))
        {
            return true;
        }

        return string.Equals(
            Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"),
            bool.TrueString,
            StringComparison.OrdinalIgnoreCase);
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

    public static async Task MigrateApplicationsDbAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        ApplicationDbContext dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    public class BaseHttp2AddressNotFoundException : NotSupportedException
    {
        public BaseHttp2AddressNotFoundException(string key)
            : base($"Cannot find the following settings key: '{key}'. Cannot start the program without it.")
        {

        }
    }



}
