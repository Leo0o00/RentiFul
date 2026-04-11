using Ardalis.Result;
using Leases.Contracts;
using Leases.Data.Repositories;
using Mediator;

namespace Leases.Features.Lease.CreateLease.Handler;

public class CreateLeaseCommandHandler : IRequestHandler<CreateLeaseCommand, Result<CreateLeaseResponseDto>>
{
    private readonly ILeaseRepository _leaseRepository;

    public CreateLeaseCommandHandler(ILeaseRepository leaseRepository)
    {
        _leaseRepository = leaseRepository;
    }

    public async ValueTask<Result<CreateLeaseResponseDto>> Handle(CreateLeaseCommand request, CancellationToken cancellationToken)
    {
        var property = await _leaseRepository.GetPropertyById(request.PropertyId);

        if (property is null)
        {
            return Result.NotFound("Property not found with the provided id");
        }

        var tenant = await _leaseRepository.GetTenantByCognitoId(request.TenantCognitoId);

        if (tenant is null)
        {
            return Result.NotFound("Tenant not found with the provided id");
        }

        var leaseToCreate = new Domain.Lease(
            startDate: request.StartDate,
            endDate: request.EndDate,
            rent: property.PricePerMonth,
            deposit: property.SecurityDeposit,
            propertyId: property.Id,
            tenantId: request.TenantCognitoId);

        var result = await _leaseRepository.CreateLease(leaseToCreate);

        return new CreateLeaseResponseDto(result);
    }
}