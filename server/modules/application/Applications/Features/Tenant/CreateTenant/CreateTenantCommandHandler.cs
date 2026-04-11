using Applications.Data.Repositories;
using Ardalis.Result;
using Mediator;

namespace Applications.Features.Tenant.CreateTenant;

public class CreateTenantCommandHandler: IRequestHandler<CreateTenantCommand, Result>
{
    private readonly ITenantRepository _tenantRepository;

    public CreateTenantCommandHandler(ITenantRepository tenantRepository)
    {
        _tenantRepository = tenantRepository;
    }

    public async ValueTask<Result> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        await _tenantRepository.AddTenant(request.TenantId, cancellationToken);

        return Result.Success();
    }
}