using MassTransit;
using Mediator;
using Payments.Features.CreateLease;
using Payments.Features.RemoveLease;
using Serilog;
using SharedContracts.Events;

namespace Payments.Features;

public class PaymentEventsConsumers : IConsumer<LeaseCreated>, IConsumer<LeaseRemoved>
{
    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    public PaymentEventsConsumers(IMediator mediator, ILogger logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<LeaseCreated> context)
    {
        _logger.Information("Consuming {eventName} for lease Id: {LeaseId}", nameof(LeaseCreated), context.Message.LeaseId);

        var command = new CreateLeaseCommand(
           LeaseId: context.Message.LeaseId
        );

        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {

            _logger.Information("Lease with Id:{LeaseId} created successfully", context.Message.LeaseId);

        }
        else
        {
            _logger.Error("Error creating lease with Id:{LeaseId}. Error: {ErrorMessage}", context.Message.LeaseId, result.Errors);

        }

        _logger.Information("Consumed {eventName} {eventId}", nameof(LeaseCreated), context.MessageId);
    }

    public async Task Consume(ConsumeContext<LeaseRemoved> context)
    {
        _logger.Information("Consuming {eventName} for lease Id: {LeaseId}", nameof(LeaseCreated), context.Message.LeaseId);

        var command = new RemoveLeaseCommand(
            LeaseId: context.Message.LeaseId
        );

        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {

            _logger.Information("Lease with Id:{LeaseId} removed successfully", context.Message.LeaseId);

        }
        else
        {
            _logger.Error("Error removing lease with Id:{LeaseId}. Error: {ErrorMessage}", context.Message.LeaseId, result.Errors);

        }

        _logger.Information("Consumed {eventName} {eventId}", nameof(LeaseCreated), context.MessageId);
    }
}