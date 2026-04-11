using Ardalis.Result;
using Mediator;

namespace Leases.Features.Tenant.CreateTenant;

public record CreateTenantCommand(
    Guid TenantId,
    string Name,
    string Email,
    string PhoneNumber
    ): IRequest<Result>;