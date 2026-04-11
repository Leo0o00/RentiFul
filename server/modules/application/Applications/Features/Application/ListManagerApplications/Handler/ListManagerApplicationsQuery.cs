using Applications.Contracts;
using Ardalis.Result;
using Mediator;

namespace Applications.Features.Application.ListManagerApplications.Handler;

public record ListManagerApplicationsQuery(Guid UserId, int Page, int Limit): IRequest<Result<ListApplicationsResponseDto>>;