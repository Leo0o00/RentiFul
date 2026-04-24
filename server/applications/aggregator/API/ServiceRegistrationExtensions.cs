using Applications;
using Leases;
using MassTransit;
using Payments;
using Properties;
using Tenants;

namespace API;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddMassTransitConfig(this IServiceCollection services,
        IConfiguration configuration)
    {
        var rabbitMqUri = configuration.GetConnectionString("RabbitMq")
            ?? throw new InvalidOperationException("ConnectionStrings:RabbitMq must be configured.");
        var rabbitMqUsername = configuration["RabbitMq:Username"] ?? "guest";
        var rabbitMqPassword = configuration["RabbitMq:Password"] ?? "guest";

        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            busConfigurator.AddPropertyModuleConsumers();
            busConfigurator.AddApplicationModuleConsumers();
            busConfigurator.AddLeaseModuleConsumers();
            busConfigurator.AddTenantModuleConsumers();
            busConfigurator.AddPaymentModuleConsumers();


            busConfigurator.AddApplicationSagaStateMachine();

            busConfigurator.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(rabbitMqUri), hst =>
                {
                    hst.Username(rabbitMqUsername);
                    hst.Password(rabbitMqPassword);
                });

                cfg.UseInMemoryOutbox(context);

                cfg.ConfigureEndpoints(context);
            });
        });
        return services;
    }
}
