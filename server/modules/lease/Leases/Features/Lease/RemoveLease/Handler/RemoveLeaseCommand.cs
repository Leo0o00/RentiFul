using Ardalis.Result;
using Mediator;

namespace Leases.Features.Lease.RemoveLease.Handler;

public record RemoveLeaseCommand(Guid LeaseId): IRequest<Result<Guid>>;