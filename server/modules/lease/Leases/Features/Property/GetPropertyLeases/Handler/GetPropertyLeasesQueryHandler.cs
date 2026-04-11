using Ardalis.Result;
using Leases.Contracts;
using Leases.Data.Repositories;
using Leases.Features.Tenant.GetLeasesForTenant.Handler;
using Mediator;

namespace Leases.Features.Property.GetPropertyLeases.Handler;

public class GetPropertyLeasesQueryHandler : IRequestHandler<GetPropertyLeasesQuery, Result<PropertyLeasesDto>>
{
    private readonly ILeaseRepository _leaseRepository;
    private const int PageSize = 10;

    public GetPropertyLeasesQueryHandler(ILeaseRepository leaseRepository)
    {
        _leaseRepository = leaseRepository;
    }

    public async ValueTask<Result<PropertyLeasesDto>> Handle(GetPropertyLeasesQuery request,
        CancellationToken ct)
    {
        var propertyId = Guid.Parse(request.PropertyId);
        var propertyExist = await _leaseRepository.CheckPropertyExistence(propertyId);

        if (!propertyExist)
        {
            return Result.NotFound("Property not found with the provided id.");
        }

        var result = await _leaseRepository.GetLeasesByPropertyId(propertyId, PageSize);

        return result;

    }
}