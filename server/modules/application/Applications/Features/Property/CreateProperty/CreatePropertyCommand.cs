using Ardalis.Result;
using Mediator;

namespace Applications.Features.Property.CreateProperty;

public record CreatePropertyCommand(Guid PropertyId, Guid ManagerId): IRequest<Result>;