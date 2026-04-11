using System.Net;
using Amazon;
using Amazon.S3;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Properties.Data;
using Properties.Data.Repositories;
using Properties.Features;
using Properties.Features.Manager.GetManagerProperties;
using Properties.Features.Property.CreateProperty;
using Properties.Features.Property.GetAllPropertiesInIdList;
using Properties.Features.Property.GetAllPropertiesPaged;
using Properties.Features.Property.GetPropertyDetails;
using Properties.Features.Property.GetPropertyMinInfo;
using Serilog;

namespace Properties;

public static class PropertiesModuleServiceExtensions
{
    public static IServiceCollection AddPropertiesModule(this IServiceCollection services,
        ConfigurationManager config,
        ILogger logger,
        List<System.Reflection.Assembly> mediatRAssemblies)
    {
        string? connectionString = config.GetConnectionString("PropertiesConnectionString");
        services
            // Add and configure db context
            .AddDbContext<PropertyDbContext>(options => options
                .UseNpgsql(connectionString,
                    o => o.UseNetTopologySuite())
            )
            .AddScoped<IPropertyRepository,  PropertyRepository>()
            .AddScoped<IManagerRepository,  ManagerRepository>()
            // Add S3 service
            .Configure<S3Settings>(config.GetSection("S3Settings"))
            .AddSingleton<IAmazonS3>(sp =>
            {
                var s3Settings = sp.GetRequiredService<IOptions<S3Settings>>().Value;
                var accessKey = s3Settings.AccessKey;
                var secretAccessKey = s3Settings.SecretAccessKey;
                var s3Config = new AmazonS3Config
                {

                    RegionEndpoint = RegionEndpoint.GetBySystemName(s3Settings.Region),

                };
                return new AmazonS3Client(
                    awsAccessKeyId: accessKey,
                    awsSecretAccessKey: secretAccessKey,
                    s3Config
                    );
            })
            // Add features
            .AddCreateProperty()
            .AddGetAllPropertiesPaged()
            .AddGetPropertyDetails()
            .AddGetManagerProperties()
            .AddGetAllPropertiesInIdList()
            .AddGetPropertyMinInfo()

            // Add gRPC service
            .AddGrpc()
            ;
        mediatRAssemblies.Add(typeof(PropertiesModuleServiceExtensions).Assembly);
        logger.Information("{Module} module services registered", Constants.ModuleName);
        return services;
    }

    public static void AddPropertyModuleConsumers(this IRegistrationConfigurator configurator)
    {
        configurator.AddConsumers(typeof(PropertyEventsConsumer));
    }

    public static IEndpointRouteBuilder MapPropertyModule(this IEndpointRouteBuilder endpoints)
    {
        _ = endpoints
            .MapGrpcService<PropertyGrpcService>();
            ;
        return endpoints;
    }
}