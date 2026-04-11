using MassTransit;
using Mediator;
using Properties.Features.Manager.CreateManager;
using Serilog;
using SharedContracts.Events;

namespace Properties.Features;

public class PropertyEventsConsumer : IConsumer<ManagerCreated>
{
    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    public PropertyEventsConsumer(IMediator mediator, ILogger logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ManagerCreated> context)
    {
        _logger.Information("Consuming {eventName} for manager Id: {ManagerId}", nameof(ManagerCreated), context.Message.CognitoId);

        var command = new CreateManagerCommand(
            Id: context.Message.CognitoId);

        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {

            _logger.Information("Manager with Id:{ManagerId} created successfully", context.Message.CognitoId);

        }
        else
        {
            _logger.Error("Error creating manager with Id:{ManagerId}. Error: {ErrorMessage}", context.Message.CognitoId, result.Errors);

        }

        _logger.Information("Consumed {eventName} {eventId}", nameof(ManagerCreated), context.MessageId);
    }
}