using Ardalis.Result;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Tenants.Contracts;
using Tenants.Data.Repositories;
using Tenants.ExceptionMessages;
using Tenants.Features.Tenant.CreateTenant.Handler;

namespace Tenants.Features.Tenant.GetTenantDetails.Handler;

public class GetTenantDetailsQueryHandler : IRequestHandler<GetTenantDetailsQuery, Result<TenantDetailsDto>>
{
    private readonly ITenantRepository _tenantRepository;

    public GetTenantDetailsQueryHandler(ITenantRepository tenantRepository)
    {
        _tenantRepository = tenantRepository;
    }
    
    public async ValueTask<Result<TenantDetailsDto>> Handle(GetTenantDetailsQuery request, CancellationToken ct)
    {
        var tenant = await _tenantRepository.GetByIdDetailsWithFavoriteProperties(cognitoId: request.CognitoId);

        return tenant is null ? Result.NotFound() : Result.Success(tenant);
    }
}