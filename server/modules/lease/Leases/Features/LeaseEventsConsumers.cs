using Ardalis.Result;
using Leases.Features.Lease.CreateLease.Handler;
using Leases.Features.Lease.RemoveLease.Handler;
using Leases.Features.Property.CreateProperty;
using Leases.Features.Tenant.CreateTenant;
using Leases.Features.Tenant.UpdateTenant;
using MassTransit;
using Mediator;
using SharedContracts.Commands;
using SharedContracts.Events;
using Serilog;

namespace Leases.Features;

public class LeaseEventsConsumers : IConsumer<CreateLease>, IConsumer<CreateLeaseRollback>, IConsumer<TenantCreated>, IConsumer<TenantUpdated>, IConsumer<PropertyCreated>
{
    private readonly IMediator _mediator;
    private readonly ILogger _logger;
    private readonly IBus _bus;

    public LeaseEventsConsumers(ILogger logger, IBus bus, IMediator mediator)
    {
        _logger = logger;
        _bus = bus;
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<CreateLease> context)
    {
        _logger.Information("Consuming {eventName} for Application:{applicationId}", nameof(CreateLease), context.Message.ApplicationId);

        var command = new CreateLeaseCommand(
            PropertyId: context.Message.PropertyId,
            TenantCognitoId: context.Message.TenantCognitoId,
            StartDate: context.Message.StartDate,
            EndDate: context.Message.EndDate
            );

        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            await _bus.Publish(new LeaseCreated
            {
                ApplicationId = context.Message.ApplicationId,
                LeaseId = result.Value.LeaseId
            });
            _logger.Information("The lease with Id:{leaseId} was created successfully", result.Value.LeaseId);

        }
        else
        {
            _logger.Error("Error creating lease with Id:{leaseId}. Error: {ErrorMessage}", result.Value.LeaseId, result.Errors);
            await _bus.Publish(new CreateLeaseFailed
            {
                ApplicationId = context.Message.ApplicationId
            });
        }

        _logger.Information("Consumed {eventName} {eventId}", nameof(CreateLease), context.MessageId);
    }

    public async Task Consume(ConsumeContext<CreateLeaseRollback> context)
    {
        _logger.Information("Consuming {eventName} for Application:{applicationId}", nameof(CreateLeaseRollback), context.Message.ApplicationId);

        var command = new RemoveLeaseCommand(
            LeaseId: context.Message.LeaseId
        );

        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            await _bus.Publish(new LeaseRemoved
            {
                ApplicationId = context.Message.ApplicationId,
                LeaseId = result.Value
            });
            _logger.Information("The lease with Id:{leaseId} was removed successfully", result.Value);

        }
        else
        {
            _logger.Error("Error removing lease with Id:{leaseId}. Error: {ErrorMessage}", result.Value, result.Errors);

        }

        _logger.Information("Consumed {eventName} {eventId}", nameof(CreateLeaseRollback), context.MessageId);
    }

    public async Task Consume(ConsumeContext<TenantCreated> context)
    {
        _logger.Information("Consuming {eventName} for tenant Id: {TenantId}", nameof(TenantCreated), context.Message.TenantId);

        var command = new CreateTenantCommand(
            TenantId: context.Message.TenantId,
            Name: context.Message.Name,
            Email: context.Message.Email,
            PhoneNumber: context.Message.PhoneNumber);

        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {

            _logger.Information("Tenant with Id:{TenantId} created successfully", context.Message.TenantId);

        }
        else
        {
            _logger.Error("Error creating tenant with Id:{TenantId}. Error: {ErrorMessage}", context.Message.TenantId, result.Errors);

        }

        _logger.Information("Consumed {eventName} {eventId}", nameof(TenantCreated), context.MessageId);
    }

    public async Task Consume(ConsumeContext<TenantUpdated> context)
    {
        _logger.Information("Consuming {eventName} for tenant Id: {TenantId}", nameof(TenantUpdated), context.Message.TenantId);

        var command = new UpdateTenantCommand(
            TenantId: context.Message.TenantId,
            Name: context.Message.Name,
            Email: context.Message.Email,
            PhoneNumber: context.Message.PhoneNumber);

        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {

            _logger.Information("Tenant with Id:{TenantId} updated successfully", context.Message.TenantId);

        }
        else
        {
            _logger.Error("Error updating tenant with Id:{TenantId}. Error: {ErrorMessage}", context.Message.TenantId, result.Errors);

        }

        _logger.Information("Consumed {eventName} {eventId}", nameof(TenantUpdated), context.MessageId);
    }

    public async Task Consume(ConsumeContext<PropertyCreated> context)
    {
        _logger.Information("Consuming {eventName} for property Id: {PropertyId}", nameof(PropertyCreated), context.Message.Id);

        var command = new CreatePropertyCommand(
            PropertyId: context.Message.Id,
            PricePerMonth:  context.Message.PricePerMonth,
            SecurityDeposit:    context.Message.SecurityDeposit
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