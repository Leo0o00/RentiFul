using Ardalis.Result;
using Mediator;
using Payments.Data.Repositories;
using Payments.Domain;

namespace Payments.Features.CreateLease;

public class CreateLeaseCommandHandler: IRequestHandler<CreateLeaseCommand, Result>
{
    private readonly ILeaseRepository _leaseRepository;

    public CreateLeaseCommandHandler(ILeaseRepository leaseRepository)
    {
        _leaseRepository = leaseRepository;
    }

    public async ValueTask<Result> Handle(CreateLeaseCommand request, CancellationToken cancellationToken)
    {
        var leaseToCreate = new Lease
        {
            Id = request.LeaseId
        };

        await _leaseRepository.Create(leaseToCreate);

        return Result.Success();
    }
}