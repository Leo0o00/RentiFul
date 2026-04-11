using Ardalis.Result;
using Mediator;

namespace Tenants.Features.Property.CreateProperty;

public record CreatePropertyCommand(Guid PropertyId): IRequest<Result>;