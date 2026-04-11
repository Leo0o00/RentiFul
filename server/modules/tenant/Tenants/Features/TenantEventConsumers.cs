using MassTransit;
using Mediator;
using Serilog;
using SharedContracts.Commands;
using SharedContracts.Events;
using Tenants.Features.Property.AddOwnedProperty.Handler;
using Tenants.Features.Property.CreateProperty;

namespace Tenants.Features;

public class TenantEventConsumers : IConsumer<AddTenantOwnedProperty>, IConsumer<PropertyCreated>
{
    private readonly ILogger _logger;
    private readonly IBus _bus;
    private readonly IMediator _mediator;

    public TenantEventConsumers(ILogger logger, IBus bus, IMediator mediator)
    {
        _logger = logger;
        _bus = bus;
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<AddTenantOwnedProperty> context)
    {
        _logger.Information("Consuming {eventName} for Application:{applicationId}", nameof(AddTenantOwnedProperty), context.Message.ApplicationId);

        var command = new AddOwnedPropertyCommand(TenantCognitoId: context.Message.TenantCognitoId, PropertyId: context.Message.PropertyId);

        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {

            _logger.Information("The property with Id:{productId} was added successfully to owned properties list from Tenant with Id: {tenantCognitoId}.", context.Message.PropertyId, context.Message.TenantCognitoId);

            await _bus.Publish(new TenantOwnedPropertyAdded
            {
                ApplicationId = context.Message.ApplicationId,
                PropertyId = context.Message.PropertyId,
                TenantCognitoId = context.Message.TenantCognitoId
            });

        }
        else
        {
            _logger.Error("Error adding property with Id:{propertyId} to owned properties list from Tenant with Id: {tenantCognitoId}. Error: {ErrorMessage}", context.Message.PropertyId, context.Message.TenantCognitoId, result.Errors);
            await _bus.Publish(new AddTenantOwnedPropertyFailed
            {
                ApplicationId = context.Message.ApplicationId,
                PropertyId = context.Message.PropertyId,
                TenantCognitoId = context.Message.TenantCognitoId
            });
        }

        _logger.Information("Consumed {eventName} {eventId}", nameof(AddTenantOwnedProperty), context.MessageId);

    }

    public async Task Consume(ConsumeContext<PropertyCreated> context)
    {
        _logger.Information("Consuming {eventName} for property Id: {PropertyId}", nameof(PropertyCreated), context.Message.Id);

        var command = new CreatePropertyCommand(
            PropertyId: context.Message.Id
            );

        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {

            _logger.Information("Property with Id:{PropertyId} created successfully", context.Message.Id);

        }
        else
        {
            _logger.Error("Error creating property with Id:{PropertyId}. Error: {ErrorMessage}", context.Message.Id, result.Errors);

        }

        _logger.Information("Consumed {eventName} {eventId}", nameof(PropertyCreated), context.MessageId);
    }
}