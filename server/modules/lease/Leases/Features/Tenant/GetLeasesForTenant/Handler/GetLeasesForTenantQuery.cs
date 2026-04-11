using Ardalis.Result;
using FluentValidation;
using Leases.Contracts;
using Mediator;
using SharedKernel;

namespace Leases.Features.Tenant.GetLeasesForTenant.Handler;

public record GetLeasesForTenantQuery(
    string CognitoId,
    string PropertyId
    ) : IRequest<Result<LeasesResponseDto>>;

public class GetLeasesForTenantQueryValidator : AbstractValidator<GetLeasesForTenantQuery>
{
    public GetLeasesForTenantQueryValidator()
    {
        RuleFor(x => x.CognitoId)
            .NotEmpty()
            .WithMessage("CognitoId is required")
            .MustBeUuid();
        RuleFor(x => x.PropertyId)
            .NotEmpty()
            .WithMessage("PropertyId is required")
            .MustBeUuid();
    }
}