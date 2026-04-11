using Ardalis.Result;
using Mediator;

namespace Leases.Features.Property.CreateProperty;

public record CreatePropertyCommand(Guid PropertyId, decimal PricePerMonth, decimal SecurityDeposit): IRequest<Result>;