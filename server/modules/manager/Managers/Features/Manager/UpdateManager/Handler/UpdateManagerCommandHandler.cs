using Ardalis.Result;
using Managers.Data.Repositories;
using Managers.ExceptionMessages;
using Mediator;

namespace Managers.Features.Manager.UpdateManager.Handler;

public class UpdateManagerCommandHandler : IRequestHandler<UpdateManagerCommand, Result>
{
    private readonly IManagerRepository _managerRepository;

    public UpdateManagerCommandHandler(IManagerRepository managerRepository)
    {
        _managerRepository = managerRepository;
    }
    
    public async ValueTask<Result> Handle(UpdateManagerCommand request, CancellationToken cancellationToken)
    {
        var manager = await _managerRepository.GetDetailsByCognitoId(
            cognitoId: request.CognitoId,
            noTracking: false
        );

        if (manager is null)
        {
            return Result.NotFound(ManagerExceptionsMessages.ManagerNotFound);
        }

        if (manager.Email != request.Email)
        {
            var managerIsNotAvailable = await _managerRepository.CheckManagerAvailability(
                email: request.Email
            );
            
            if (managerIsNotAvailable)
            {
                return Result.Conflict(ManagerExceptionsMessages.ManagerNotAvailable);
            }
            
        }
        
        manager.Update(
            name: request.Name,
            email: request.Email,
            phoneNumber: request.PhoneNumber);
        
        await  _managerRepository.SaveChangesAsync();

        return Result.Success();
    }
}