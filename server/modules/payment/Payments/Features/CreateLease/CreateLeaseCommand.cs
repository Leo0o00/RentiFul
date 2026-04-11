using Ardalis.Result;
using Mediator;

namespace Payments.Features.CreateLease;

public record CreateLeaseCommand(Guid LeaseId): IRequest<Result>;