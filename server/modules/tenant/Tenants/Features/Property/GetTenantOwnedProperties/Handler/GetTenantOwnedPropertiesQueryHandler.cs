using Ardalis.Result;
using Mediator;
using Tenants.Contracts;
using Tenants.Data.Repositories;
using Tenants.ExceptionMessages;
using Tenants.Features.Tenant.CreateTenant.Handler;

namespace Tenants.Features.Property.GetTenantOwnedProperties.Handler;

public class GetTenantOwnedPropertiesQueryHandler : IRequestHandler<GetTenantOwnedPropertiesQuery, Result<TenantOwnedPropertiesDto>>
{
    private readonly ITenantRepository _tenantRepository;

    public GetTenantOwnedPropertiesQueryHandler(ITenantRepository tenantRepository)
    {
        _tenantRepository = tenantRepository;
    }
    
    public async ValueTask<Result<TenantOwnedPropertiesDto>> Handle(GetTenantOwnedPropertiesQuery request, CancellationToken cancellationToken)
    {
        var tenant = await _tenantRepository.GetByIdWithOwnedProperties(cognitoId: request.CognitoId);

        return tenant is null 
            ? Result.NotFound(TenantExceptionsMessages.TenantNotFound) 
            : Result.Success(tenant);
    }
}