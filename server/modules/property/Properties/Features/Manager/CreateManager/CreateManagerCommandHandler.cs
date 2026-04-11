using Ardalis.Result;
using Mediator;
using Properties.Data.Repositories;

namespace Properties.Features.Manager.CreateManager;

public class CreateManagerCommandHandler : IRequestHandler<CreateManagerCommand, Result>
{
    private readonly IManagerRepository _managerRepository;

    public CreateManagerCommandHandler(IManagerRepository managerRepository)
    {
        _managerRepository = managerRepository;
    }

    public async ValueTask<Result> Handle(CreateManagerCommand request, CancellationToken cancellationToken)
    {
        var managerToCreate = new Domain.Manager
        {
            Id = request.Id,
        };

        await _managerRepository.Create(managerToCreate, cancellationToken);
        return Result.Success();
    }
}