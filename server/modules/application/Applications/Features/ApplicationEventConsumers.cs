using Applications.Features.Application.LinkLeaseToApplication.Handler;
using Applications.Features.Application.RollbackApplicationStatus.Handler;
using Applications.Features.Application.UnlinkLeaseToApplication.Handler;
using Applications.Features.Property.CreateProperty;
using Applications.Features.Tenant.CreateTenant;
using MassTransit;
using Mediator;
using Serilog;
using SharedContracts.Commands;
using SharedContracts.Events;

namespace Applications.Features;

public class ApplicationEventConsumers : IConsumer<LinkLeaseToApplication>, IConsumer<ApproveApplicationProcessCompleted>, IConsumer<UpdateApplicationStatusRollback>, IConsumer<UnlinkLeaseToApplication>, IConsumer<TenantCreated>, IConsumer<PropertyCreated>
{
    private readonly ILogger _logger;
    private readonly IBus _bus;
    private readonly IMediator _mediator;

    public ApplicationEventConsumers(ILogger logger, IBus bus, IMediator mediator)
    {
        _logger = logger;
        _bus = bus;
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<LinkLeaseToApplication> context)
    {
        _logger.Information("Consuming {eventName} for Application:{applicationId}", nameof(LinkLeaseToApplication), context.Message.ApplicationId);

        var command = new LinkLeaseToApplicationCommand(ApplicationId:  context.Message.ApplicationId, LeaseId: context.Message.LeaseId);

        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {

            _logger.Information("Lease with Id:{leaseId} linked to application with Id:{applicationId}", context.Message.LeaseId, context.Message.ApplicationId);

            await _bus.Publish(new LeaseLinkedToApplication
            {
                ApplicationId = context.Message.ApplicationId
            });

        }
        else
        {
            _logger.Error("Error linking lease with Id:{leaseId} to Application with Id:{applicationId}. Error: {ErrorMessage}", context.Message.LeaseId, context.Message.ApplicationId, result.Errors);
            await _bus.Publish(new LinkLeaseToApplicationFailed
            {
                ApplicationId = context.Message.ApplicationId,
                LeaseId = context.Message.LeaseId
            });
        }

        _logger.Information("Consumed {eventName} {eventId}", nameof(LinkLeaseToApplication), context.MessageId);

    }

    public Task Consume(ConsumeContext<ApproveApplicationProcessCompleted> context)
    {
        _logger.Information("Consumed {eventName}. Application with Id:{applicationId} for Tenant with Id:{tenantCognitoId} and Property with Id:{propertyId} has been successfully approved", nameof(ApproveApplicationProcessCompleted), context.Message.ApplicationId, context.Message.TenantCognitoId, context.Message.PropertyId);

        return Task.CompletedTask;
    }

    public async Task Consume(ConsumeContext<UpdateApplicationStatusRollback> context)
    {
        _logger.Information("Consuming {eventName} for Application:{applicationId}", nameof(UpdateApplicationStatusRollback), context.Message.ApplicationId);

        var command = new RollbackApplicationStatusCommand(ApplicationId:  context.Message.ApplicationId);

        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {

            _logger.Information("Application status with Id:{applicationId} successfully rolled back", context.Message.ApplicationId);

            await _bus.Publish(new UpdateApplicationStatusRolledBack
            {
                ApplicationId = context.Message.ApplicationId
            });

        }
        else
        {
            _logger.Error("Error rolling back application status with Id:{applicationId}. Error: {ErrorMessage}", context.Message.ApplicationId, result.Errors);

        }

        _logger.Information("Consumed {eventName} {eventId}", nameof(UpdateApplicationStatusRollback), context.MessageId);
    }

    public async Task Consume(ConsumeContext<UnlinkLeaseToApplication> context)
    {
        _logger.Information("Consuming {eventName} for Application:{applicationId}", nameof(UnlinkLeaseToApplication), context.Message.ApplicationId);

        var command = new UnlinkLeaseToApplicationCommand(ApplicationId: context.Message.ApplicationId, LeaseId: context.Message.LeaseId);

        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {

            _logger.Information("Lease with Id:{leaseId} unlinked from Application with Id:{applicationId}", context.Message.LeaseId, context.Message.ApplicationId);

            await _bus.Publish(new UnlinkedLeaseToApplication
            {
                ApplicationId = context.Message.ApplicationId,
                LeaseId = context.Message.LeaseId
            });

        }
        else
        {
            _logger.Error("Error unlinking lease with Id:{leaseId} from Application with Id:{applicationId}. Error: {ErrorMessage}", context.Message.LeaseId, context.Message.ApplicationId, result.Errors);

        }

        _logger.Information("Consumed {eventName} {eventId}", nameof(UnlinkLeaseToApplication), context.MessageId);
    }

    public async Task Consume(ConsumeContext<TenantCreated> context)
    {
        _logger.Information("Consuming {eventName} for tenant Id: {TenantId}", nameof(TenantCreated), context.Message.TenantId);

        var command = new CreateTenantCommand(TenantId: context.Message.TenantId);

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

    public async Task Consume(ConsumeContext<PropertyCreated> context)
    {
        _logger.Information("Consuming {eventName} for property Id: {PropertyId}", nameof(PropertyCreated), context.Message.Id);

        var command = new CreatePropertyCommand(
            PropertyId: context.Message.Id,
            ManagerId: context.Message.ManagerId
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