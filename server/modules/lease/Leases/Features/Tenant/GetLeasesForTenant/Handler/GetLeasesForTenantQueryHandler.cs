using Ardalis.Result;
using Leases.Contracts;
using Leases.Data.Repositories;
using Mediator;

namespace Leases.Features.Tenant.GetLeasesForTenant.Handler;

public class GetLeasesForTenantQueryHandler : IRequestHandler<GetLeasesForTenantQuery, Result<LeasesResponseDto>>
{
    private readonly ILeaseRepository _leaseRepository;
    private const int PageSize = 10;

    public GetLeasesForTenantQueryHandler(ILeaseRepository leaseRepository)
    {
        _leaseRepository = leaseRepository;
    }

    public async ValueTask<Result<LeasesResponseDto>> Handle(GetLeasesForTenantQuery request,
        CancellationToken ct)
    {
        var tenantCognitoId = Guid.Parse(request.CognitoId);
        var propertyId = Guid.Parse(request.PropertyId);

        var tenantExist = await _leaseRepository.CheckTenantExistence(tenantCognitoId);

        if (!tenantExist)
        {
            return Result.NotFound("Tenant doesn't exist with the provided id.");
        }

        var propertyExist = await _leaseRepository.CheckPropertyExistence(propertyId);

        if (!propertyExist)
        {
            return Result.NotFound("Property not found with the provided id.");
        }

        var result = await _leaseRepository.GetLeasesByTenantIdAndPropertyId(tenantCognitoId, propertyId, PageSize);

        return result;

    }
}