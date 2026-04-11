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
                cfg.Host(new Uri(configuration.GetConnectionString("RabbitMq")!), hst =>
                {
                    hst.Username("guest");
                    hst.Password("guest");
                });

                cfg.UseInMemoryOutbox(context);

                cfg.ConfigureEndpoints(context);
            });
        });
        return services;
    }
}