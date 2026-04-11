using Ardalis.Result;
using Leases.Data.Repositories;
using Mediator;

namespace Leases.Features.Tenant.UpdateTenant;

public class UpdateTenantCommandHandler : IRequestHandler<UpdateTenantCommand, Result>
{
    private readonly ITenantRepository _tenantRepository;

    public UpdateTenantCommandHandler(ITenantRepository tenantRepository)
    {
        _tenantRepository = tenantRepository;
    }

    public async ValueTask<Result> Handle(UpdateTenantCommand request, CancellationToken cancellationToken)
    {
        var tenantToUpdate = new Domain.Tenant(request.TenantId, request.Name, request.Email, request.PhoneNumber);
        await  _tenantRepository.UpdateTenant(tenantToUpdate, cancellationToken);

        return Result.Success();
    }
}