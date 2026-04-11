using Applications.Contracts;
using Ardalis.Result;
using Mediator;

namespace Applications.Features.Application.ListTenantApplications.Handler;

public record ListTenantApplicationsQuery(Guid UserId, int Page, int Limit) : IRequest<Result<ListApplicationsResponseDto>>;