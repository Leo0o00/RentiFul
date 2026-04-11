using Ardalis.Result;
using Mediator;

namespace Leases.Features.Tenant.UpdateTenant;

public record UpdateTenantCommand(
    Guid TenantId,
    string Name,
    string Email,
    string PhoneNumber
) : IRequest<Result>;