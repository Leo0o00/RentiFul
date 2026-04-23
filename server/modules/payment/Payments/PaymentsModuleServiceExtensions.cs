using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payments.Data;
using Payments.Data.Repositories;
using Payments.Features;
using Serilog;

namespace Payments;

public static class PaymentsModuleServiceExtensions
{
    public static IServiceCollection AddPaymentsModule(this IServiceCollection services,
        ConfigurationManager config,
        ILogger logger,
        List<System.Reflection.Assembly> mediatRAssemblies)
    {
        string? connectionString = config.GetConnectionString("PaymentsConnectionString");
        services
            // Add and configure db context
            .AddDbContext<PaymentDbContext>(options => options
                .UseNpgsql(connectionString)
            )
            .AddScoped<IPaymentRepository, PaymentRepository>()
            .AddScoped<ILeaseRepository, LeaseRepository>()
            // Add features
            ;
        mediatRAssemblies.Add(typeof(PaymentsModuleServiceExtensions).Assembly);
        logger.Information("{Module} module services registered", Constants.ModuleName);
        return services;
    }

    public static void AddPaymentModuleConsumers(this IRegistrationConfigurator configurator)
    {
        configurator.AddConsumers(typeof(PaymentEventsConsumers));
    }

    public static async Task MigratePaymentsDbAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        PaymentDbContext dbContext = scope.ServiceProvider
            .GetRequiredService<PaymentDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}