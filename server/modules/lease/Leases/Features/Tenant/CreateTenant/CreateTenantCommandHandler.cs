using Ardalis.Result;
using Leases.Data.Repositories;
using Mediator;

namespace Leases.Features.Tenant.CreateTenant;

public class CreateTenantCommandHandler: IRequestHandler<CreateTenantCommand, Result>
{
    private readonly ITenantRepository _tenantRepository;

    public CreateTenantCommandHandler(ITenantRepository tenantRepository)
    {
        _tenantRepository = tenantRepository;
    }

    public async ValueTask<Result> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        await _tenantRepository.AddTenant(
            tenantCognitoId: request.TenantId,
            name: request.Name,
            email: request.Email,
            phoneNumber: request.PhoneNumber,
            cancellationToken);

        return Result.Success();
    }
}