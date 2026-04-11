using Ardalis.Result;
using Mediator;

namespace Payments.Features.RemoveLease;

public record RemoveLeaseCommand(Guid LeaseId): IRequest<Result>;