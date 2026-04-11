using Ardalis.Result;
using Mediator;

namespace Applications.Features.Tenant.CreateTenant;

public record CreateTenantCommand(Guid TenantId): IRequest<Result>;