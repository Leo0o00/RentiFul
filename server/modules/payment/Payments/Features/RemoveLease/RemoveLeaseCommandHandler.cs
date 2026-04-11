using Ardalis.Result;
using Mediator;
using Payments.Data.Repositories;

namespace Payments.Features.RemoveLease;

public class RemoveLeaseCommandHandler: IRequestHandler<RemoveLeaseCommand, Result>
{
    private readonly ILeaseRepository  _leaseRepository;

    public RemoveLeaseCommandHandler(ILeaseRepository leaseRepository)
    {
        _leaseRepository = leaseRepository;
    }

    public async ValueTask<Result> Handle(RemoveLeaseCommand request, CancellationToken cancellationToken)
    {
        var leaseToRemove = await _leaseRepository.GetLeaseById(leaseId: request.LeaseId);

        if (leaseToRemove is null)
        {
            return Result.NotFound("Lease not found with the provided Id.");
        }

        await _leaseRepository.Remove(leaseToRemove);

        return Result.Success();
    }
}