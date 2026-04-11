using Ardalis.Result;
using Leases.Data.Repositories;
using Mediator;

namespace Leases.Features.Lease.RemoveLease.Handler;

public class RemoveLeaseCommandHandler(ILeaseRepository leaseRepository) : IRequestHandler<RemoveLeaseCommand, Result<Guid>>
{


    public async ValueTask<Result<Guid>> Handle(RemoveLeaseCommand request, CancellationToken cancellationToken)
    {
        var lease = await leaseRepository.GetLeaseById(request.LeaseId);
        if (lease is null)
        {
            return Result.NotFound("Lease not found with the provided Id.");
        }

        await leaseRepository.RemoveLease(lease: lease);

        return lease.Id;
    }
}