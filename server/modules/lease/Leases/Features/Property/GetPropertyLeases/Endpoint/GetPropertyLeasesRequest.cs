using FastEndpoints;
using FluentValidation;
using SharedKernel;

namespace Leases.Features.Property.GetPropertyLeases.Endpoint;

public record GetPropertyLeasesRequest(
    string PropertyId
);

public class GetPropertyLeasesRequestValidator : Validator<GetPropertyLeasesRequest>
{
    public GetPropertyLeasesRequestValidator()
    {
        RuleFor(x => x.PropertyId)
            .NotEmpty().WithMessage("PropertyId is required")
            .MustBeUuid();
    }
}