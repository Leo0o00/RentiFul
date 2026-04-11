using Ardalis.Result;
using Mediator;

namespace Properties.Features.Manager.CreateManager;

public record CreateManagerCommand(Guid Id): IRequest<Result>;