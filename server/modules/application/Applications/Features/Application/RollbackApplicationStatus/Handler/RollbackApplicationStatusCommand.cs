using Ardalis.Result;
using Mediator;

namespace Applications.Features.Application.RollbackApplicationStatus.Handler;

public record RollbackApplicationStatusCommand(Guid ApplicationId) : IRequest<Result>;