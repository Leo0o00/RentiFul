using Ardalis.Result;
using Mediator;

namespace Applications.Features.Application.UnlinkLeaseToApplication.Handler;

public record UnlinkLeaseToApplicationCommand(Guid ApplicationId, Guid LeaseId) : IRequest<Result>;