using Ardalis.Result;
using Mediator;

namespace Applications.Features.Application.LinkLeaseToApplication.Handler;

public record LinkLeaseToApplicationCommand(Guid ApplicationId, Guid LeaseId) : IRequest<Result<Guid>>;