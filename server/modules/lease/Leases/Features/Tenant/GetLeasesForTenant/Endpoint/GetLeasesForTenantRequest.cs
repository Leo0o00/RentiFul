using FastEndpoints;
using FluentValidation;
using SharedKernel;

namespace Leases.Features.Tenant.GetLeasesForTenant.Endpoint;

public record GetLeasesForTenantRequest(
    string CognitoId,
    string PropertyId
);

public class GetLeasesForTenantRequestValidator : Validator<GetLeasesForTenantRequest>
{
    public GetLeasesForTenantRequestValidator()
    {
        RuleFor(x => x.CognitoId)
            .NotEmpty().WithMessage("CognitoId is required")
            .MustBeUuid();
        RuleFor(x => x.PropertyId)
            .NotEmpty().WithMessage("PropertyId is required")
            .MustBeUuid();
    }
}