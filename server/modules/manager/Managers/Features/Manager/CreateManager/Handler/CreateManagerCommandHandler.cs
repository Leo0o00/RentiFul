using Ardalis.Result;
using Managers.Data.Repositories;
using Mediator;
using Managers.ExceptionMessages;
using MassTransit;
using SharedContracts.Events;

namespace Managers.Features.Manager.CreateManager.Handler;

public class CreateManagerCommandHandler : IRequestHandler<CreateManagerCommand, Result>
{
    private readonly IManagerRepository _managerRepository;
    private readonly IBus _bus;

    public CreateManagerCommandHandler(IManagerRepository managerRepository, IBus bus)
    {
        _managerRepository = managerRepository;
        _bus = bus;
    }
    
    public async ValueTask<Result> Handle(CreateManagerCommand request, CancellationToken cancellationToken)
    {
        var managerIsNotAvailable = await _managerRepository.CheckManagerAvailability(
            cognitoId: request.CognitoId,
            email: request.Email
            );

        if (managerIsNotAvailable)
        {
            return Result.Conflict(ManagerExceptionsMessages.ManagerNotAvailable);
        }

        var manager = new Domain.Manager(
            cognitoId: request.CognitoId,
            email: request.Email,
            name: request.Name,
            phoneNumber: request.PhoneNumber
            );

        await _managerRepository.Create(manager);

        await _bus.Publish(new ManagerCreated
        {
            CognitoId = Guid.Parse(manager.CognitoId)
        }, cancellationToken);
        
        return Result.Success();
    }
}